using System;
using WGETAnalog.Logic.Interfaces;

namespace WGETAnalog.Logic.InterfacesImplementation
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}