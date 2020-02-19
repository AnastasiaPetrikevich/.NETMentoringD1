using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

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
            if(string.IsNullOrWhiteSpace(name))
                name="anonim";

            Console.WriteLine($"Hello, {name}!");
        }
    }
}
