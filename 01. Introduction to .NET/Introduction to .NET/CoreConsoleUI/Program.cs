using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using HelloMessageLogic;

namespace CoreConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddCommandLine(args, new Dictionary<string, string> { 
                ["-Name"] = "Name"
            });
            var config = builder.Build();

            string name = config["Name"];

            Console.WriteLine(HelloMessageService.HelloMessage(DateTime.Now, name));
            Console.ReadKey(true);
        }
    }
}
