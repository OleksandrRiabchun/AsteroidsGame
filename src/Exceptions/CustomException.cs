using System;

namespace AsteroidsGame.Exceptions
{
    internal class CustomException : Exception
    {
        public CustomException(string message) : base(message)
        {
        }
    }
}