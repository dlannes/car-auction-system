namespace CarAuctionManagement.Core.Models
{
    public class Sedan : Vehicle
    {
        private const int MinDoors = 2;
        private const int MaxDoors = 4;

        public int NumberOfDoors { get; private set; }

        public Sedan(Guid id, string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
            : base(id, manufacturer, model, year, startingBid)
        {
            if (numberOfDoors < MinDoors || numberOfDoors > MaxDoors)
                throw new ArgumentOutOfRangeException(nameof(numberOfDoors), numberOfDoors, $"Value must be within {MinDoors} and {MaxDoors}.");

            NumberOfDoors = numberOfDoors;
        }
    }
}
