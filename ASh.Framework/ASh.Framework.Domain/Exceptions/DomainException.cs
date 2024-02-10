namespace ASh.Framework.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public int? Code { get; }
        public DomainException(string message, int? code = null) : base(message) 
        {
            Code = code;
        }
    }
}
