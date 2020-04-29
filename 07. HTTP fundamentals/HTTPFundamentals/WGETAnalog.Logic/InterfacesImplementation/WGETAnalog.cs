using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.IO;
using HtmlAgilityPack;
using WGETAnalog.Logic.Enums;
using WGETAnalog.Logic.Interfaces;

namespace WGETAnalog.Logic.InterfacesImplementation
{
    public class WGETAnalog : IWGETAnalog
    {
        private const string HtmlDocumentMediaType = "text/html";

        private readonly ISaver saver;
        private readonly List<IRestrictionHelper> urlRestrictions;
        private readonly List<IRestrictionHelper> fileRestrictions;
        private readonly ILogger logger;
        private readonly ISet<Uri> downloadedUrls = new HashSet<Uri>();
        private readonly int maxDeepLevel;

        public WGETAnalog(ISaver saver, IEnumerable<IRestrictionHelper> restrictions, ILogger logger, int maxDeepLevel = 0)
        {
            this.saver = saver;
            urlRestrictions = restrictions
                .Where(c => (c.RestrictionType & Restriction.UrlRestriction) != 0)
                .ToList();
            this.logger = logger;
            fileRestrictions = restrictions
                .Where(c => (c.RestrictionType & Restriction.FileRestriction) != 0)
                .ToList();
            this.maxDeepLevel = maxDeepLevel;
        }

        public void DownloadSite(string url)
        {
            downloadedUrls.Clear();

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(url);
                ScanningUrl(httpClient, httpClient.BaseAddress, 0);
            }
        }

        private void ScanningUrl(HttpClient httpClient, Uri url, int deepLevel)
        {
            if (deepLevel > maxDeepLevel
                || downloadedUrls.Contains(url)
                || !IsValidScheme(url.Scheme))
            {
                return;
            }

            downloadedUrls.Add(url);

            var responseMessage = httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url)).Result;

            if (!responseMessage.IsSuccessStatusCode)
            {
                return;
            }

            if (responseMessage.Content.Headers.ContentType?.MediaType == HtmlDocumentMediaType)
            {
                ProcessHtml(httpClient, url, deepLevel);
            }

            else
            {
                ProcessFile(httpClient, url);
            }
        }

        private void ProcessFile(HttpClient httpClient, Uri url)
        {
            logger.Log($"Found file: {url}");

            if (!IsValidUrl(url, fileRestrictions))
            {
                return;
            }

            var response = httpClient.GetAsync(url).Result;

            logger.Log($"Loaded file: {url}");
            saver.SaveFile(url, response.Content.ReadAsStreamAsync().Result);
        }

        private void ProcessHtml(HttpClient httpClient, Uri url, int level)
        {
            logger.Log($"Found url: {url}");

            if (!IsValidUrl(url, urlRestrictions))
            {
                return;
            }

            var responseMessage = httpClient.GetAsync(url).Result;
            var document = new HtmlDocument();

            document.Load(responseMessage.Content.ReadAsStreamAsync().Result, Encoding.UTF8);
            logger.Log($"Loaded html: {url}");
            saver.SaveHtml(url, GetHtmlFileName(document), GetDocumentStream(document));

            var attributesWithLinks = document.DocumentNode.Descendants()
                .SelectMany(d => d.Attributes
                .Where(a => a.Name == "src" || a.Name == "href"));

            foreach (var attributesWithLink in attributesWithLinks)
            {
                ScanningUrl(httpClient, new Uri(httpClient.BaseAddress, attributesWithLink.Value), level + 1);
            }
        }

        private string GetHtmlFileName(HtmlDocument document)
        {
            return document.DocumentNode.Descendants("title").FirstOrDefault()?.InnerText + ".html";
        }

        private Stream GetDocumentStream(HtmlDocument document)
        {
            var memoryStream = new MemoryStream();

            document.Save(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        private bool IsValidScheme(string scheme)
        {
            if (scheme.Equals("http") || scheme.Equals("https"))
            {
                return true;
            }

            return false;
        }

        private bool IsValidUrl(Uri url, IEnumerable<IRestrictionHelper> restrictions)
        {
            return restrictions.All(c => c.IsRestricted(url));
        }
    }
}