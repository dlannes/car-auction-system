using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CarAuctionManagement.Core.Common
{
    public abstract class Validator<T> : IValidator<T>
    {
        protected string BuildErrorString(string messageFormat, params object[] args)
        {
            return string.Format(messageFormat, args);
        }
        protected ValidationResult CreateValidationResult(List<string> errors) 
        { 
            return errors.Count > 0 ? new ValidationResult(errors) : new ValidationResult(); 
        }

        public abstract ValidationResult Validate(T value);
    }
}
