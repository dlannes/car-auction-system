using CarAuctionManagement.Core.Common;

namespace CarAuctionManagement.Core.Validators
{
    public class VehicleValidator : Validator<Vehicle>
    {
        private const int YearOfTheFirstCar = 1886;
        private static int MaxModelYear => DateTime.UtcNow.Year + 1;
        
        public override ValidationResult Validate(Vehicle vehicle)
        {
            List<string> errors = [];

            if (vehicle.Id == default)
                errors.Add(BuildErrorString($"The default {{0}} value is not valid.", nameof(vehicle.Id)));

            if (string.IsNullOrWhiteSpace(vehicle.Manufacturer))
                errors.Add(BuildErrorString($"{{0}} can't be empty or whitespace.", nameof(vehicle.Manufacturer)));

            if (string.IsNullOrWhiteSpace(vehicle.Model))
                errors.Add(BuildErrorString($"{{0}} can't be empty or whitespace.", nameof(vehicle.Model)));

            if (vehicle.Year < YearOfTheFirstCar || vehicle.Year > MaxModelYear)
                errors.Add(BuildErrorString($"{{0}} must be between {{1}} and {{2}}.", nameof(vehicle.Year), YearOfTheFirstCar, MaxModelYear));

            if (vehicle.StartingBid <= 0)
                errors.Add(BuildErrorString($"{{0}} must be greater than zero.", nameof(vehicle.StartingBid)));

            return CreateValidationResult(errors);
        }
    }

}
