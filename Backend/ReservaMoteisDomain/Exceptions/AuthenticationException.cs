using System;

namespace BookMotelsDomain.Exceptions
{
    public class AuthenticationException : DomainException
    {
        public AuthenticationException(string message) : base(message)
        {
        }
    }
}
