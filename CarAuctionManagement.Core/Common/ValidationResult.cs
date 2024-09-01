namespace CarAuctionManagement.Core.Common
{
    public class ValidationResult
    {
        public bool IsValid => Errors.Count == default;
        public string ErrorMessage => GetErrorMessage();
        public List<string> Errors { get; } = [];

        public ValidationResult() { }

        public ValidationResult(string message)
        {
            Errors.Add(message);
        }

        public ValidationResult(IEnumerable<string> messages)
        {
            Errors.AddRange(messages);
        }

        private string GetErrorMessage() 
        {
            if (IsValid) return string.Empty;
            return $"Validation errors: {string.Join(" | ", Errors)}";
        }
    }
}
