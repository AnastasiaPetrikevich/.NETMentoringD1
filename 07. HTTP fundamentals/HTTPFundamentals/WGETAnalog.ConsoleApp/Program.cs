using WGETAnalog.ConsoleApp.Restrictions;
using WGETAnalog.Logic.Interfaces;
using WGETAnalog.Logic.InterfacesImplementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WGETAnalog.Logic.Enums;

namespace WGETAnalog.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var directory = new DirectoryInfo(@"D:\HttpTest\Optika");
            var saver = new Saver(directory);
            var restrictions = new List<IRestrictionHelper>();
            var logger = new Logger();

            restrictions.Add(new FileRestrictionHelper("js".Select(r => "." + r)));
            restrictions.Add(new DomainRestrictionHelper(DomainRestriction.All, new Uri("http://optikazolushka.by/")));

            var downloader = new Logic.InterfacesImplementation.WGETAnalog(saver, restrictions, logger, 1);

            try
            {
                downloader.DownloadSite("http://optikazolushka.by/");
            }

            catch (Exception ex)
            {
                logger.Log($"Error occured during downloading: {ex.Message}");
            }
        }
    }
}