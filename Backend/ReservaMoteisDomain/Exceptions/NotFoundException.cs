namespace BookMotelsDomain.Exceptions
{
    public class NotFoundException : DomainException
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
