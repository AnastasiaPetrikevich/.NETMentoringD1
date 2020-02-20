using System;

namespace HelloMessageLogic
{
    public static class HelloMessageService
    {
        public static string HelloMessage(DateTime date, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "anonim";
            }            

            return $"{date} Hello, {name}!";
        }
    }
}
