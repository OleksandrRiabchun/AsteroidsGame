using System;

namespace OOP2.Exceptions
{
    internal class CustomException : Exception
    {
        public CustomException(string message) : base(message)
        {
        }
    }
}