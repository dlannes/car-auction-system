using CarAuctionManagement.Core.Common;
using CarAuctionManagement.Core.Models;

namespace CarAuctionManagement.Core.Validators
{
    public class VehicleValidator : IValidator<Vehicle>
    {
        private const int YearOfTheFirstCar = 1886;
        private static int MaxModelYear => DateTime.UtcNow.Year + 1;
        
        public ValidationResult Validate(Vehicle vehicle)
        {
            var result = new ValidationResult();

            if (vehicle.Id == default)
                result.AddError($"{nameof(vehicle.Id)} value is not valid.");

            if (string.IsNullOrWhiteSpace(vehicle.Manufacturer))
                result.AddError($"{nameof(vehicle.Manufacturer)} can't be empty or whitespace.");

            if (string.IsNullOrWhiteSpace(vehicle.Model))
                result.AddError($"{nameof(vehicle.Model)} can't be empty or whitespace.");

            if (vehicle.Year < YearOfTheFirstCar || vehicle.Year > MaxModelYear)
                result.AddError($"{nameof(vehicle.Year)} must be between {YearOfTheFirstCar} and {MaxModelYear}.");

            if (vehicle.StartingBid <= 0)
                result.AddError($"{nameof(vehicle.StartingBid)} must be greater than zero.");

            return result;
        }
    }

}
