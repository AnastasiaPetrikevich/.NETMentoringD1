using System;
using FileSystemWatcher.Interfaces;

namespace FileSystemWatcher.Services
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
