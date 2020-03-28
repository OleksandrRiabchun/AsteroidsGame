using System;

namespace AsteroidsGame
{
    public class LogMessage : EventArgs
    {
        public DateTime Timestamp { get; private set; }
        public object Sender { get; private set; }
        public string Message { get; private set; }

        public LogMessage(object sender, string message, DateTime timestamp)
        {
            Sender = sender;
            Message = message;
            Timestamp = timestamp;
        }
    }
}