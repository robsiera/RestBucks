using System;

namespace Infrastructure.Exceptions
{
    public class InvalidOrderOperationException : Exception
    {
        public InvalidOrderOperationException(string message)
            : base(message)
        {}
    }
}