using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace ReportGenerator.Tests
{
	[TestClass]
	public class ReportGeneratorTests
	{
		[TestMethod]
		public async System.Threading.Tasks.Task Test_Generate_Excel_File()
		{
			var client = new HttpClient();

			string url = "http://localhost/handler?customerID=ALFKI";

			HttpRequestMessage request = new HttpRequestMessage
			{
				RequestUri = new Uri(url),
				Method = HttpMethod.Get
			};

			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));		   


			using (var response = await client.SendAsync(request))
			{
				Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);				
				StringAssert.Contains(".xlsx", response.Content.Headers.ContentDisposition.FileName);
			}
		}

		public async System.Threading.Tasks.Task Test_Generate_XML_File()
		{
			var client = new HttpClient();

			string url = "http://localhost/handler?customerID=ALFKI";

			HttpRequestMessage request = new HttpRequestMessage
			{
				RequestUri = new Uri(url),
				Method = HttpMethod.Get
			};

			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));


			using (var response = await client.SendAsync(request))
			{
				Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
				StringAssert.Contains(".xml", response.Content.Headers.ContentDisposition.FileName);
			}
		}
	}
}
