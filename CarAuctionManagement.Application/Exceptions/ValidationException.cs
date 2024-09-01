namespace CarAuctionManagement.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message,string innerMessage, Exception innerException)
            : base($"{message} {innerMessage}", innerException) { }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException) { }

        public ValidationException(string message) : base(message) { }
    }
}
