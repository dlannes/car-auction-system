using CarAuctionManagement.Core.Common;

namespace CarAuctionManagement.Core.Validators
{
    public interface IValidator<T>
    {
        ValidationResult Validate(T value);
    }
}
