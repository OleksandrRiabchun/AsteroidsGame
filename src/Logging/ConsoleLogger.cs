using System;

namespace AsteroidsGame.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Write(string message) => Console.WriteLine(message);
    }
}