namespace CarAuctionManagement.Core.Common
{
    public interface IValidator<T>
    {
        ValidationResult Validate(T value);
    }
}
