namespace CarAuctionManagement.Core.Models
{
    public class Hatchback : Vehicle
    {
        private const int ThreeDoor = 3;
        private const int FiveDoor = 5;

        public int NumberOfDoors { get; private set; }

        public Hatchback(Guid id, string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
            : base(id, manufacturer, model, year, startingBid)
        {
            if (numberOfDoors != ThreeDoor && numberOfDoors != FiveDoor)
                throw new ArgumentOutOfRangeException(nameof(numberOfDoors), numberOfDoors, $"Value must be either {ThreeDoor} or {FiveDoor}.");

            NumberOfDoors = numberOfDoors;
        }
    }
}