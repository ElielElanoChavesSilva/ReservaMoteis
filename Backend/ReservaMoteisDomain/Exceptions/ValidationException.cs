using System;

namespace BookMotelsDomain.Exceptions
{
    public class ValidationException : DomainException
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}
