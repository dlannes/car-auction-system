using CarAuctionManagement.Core.Common;

namespace CarAuctionManagement.Core.Models
{
    public class Truck : Vehicle
    {
        private const double MinLoadCapacity = 500;
        private const double MaxLoadCapacity = 5000;

        public double LoadCapacity { get; private set; }

        public Truck(Guid id, string manufacturer, string model, int year, decimal startingBid, double loadCapacity)
            : base(id, manufacturer, model, year, startingBid)
        {
            if (loadCapacity < MinLoadCapacity || loadCapacity > MaxLoadCapacity)
            {
                throw new ArgumentOutOfRangeException(nameof(loadCapacity), loadCapacity, $"Value must be between {MinLoadCapacity} and {MaxLoadCapacity} kilograms.");
            }

            LoadCapacity = loadCapacity;
        }
    }
}
