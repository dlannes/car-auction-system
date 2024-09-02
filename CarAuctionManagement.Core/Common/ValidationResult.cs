namespace CarAuctionManagement.Core.Common
{
    public class ValidationResult
    {
        public List<string> Errors { get; } = [];
        public bool IsValid => Errors.Count == default;

        public void AddError(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                Errors.Add(message);
            }
        }

        public string GetErrorMessage()
        {
            return IsValid ? string.Empty : $"Validation errors: {string.Join(" | ", Errors)}";
        }
    }
}
