using CarAuctionManagement.Core.Common;

namespace CarAuctionManagement.Core.Models
{
    public class Hatchback : Vehicle
    {
        private enum BodyStyleDoors
        {
            ThreeDoor = 3,
            FiveDoor = 5
        }

        public int NumberOfDoors { get; private set; }

        public Hatchback(Guid id, string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
            : base(id, manufacturer, model, year, startingBid)
        {
            if (numberOfDoors != (int)BodyStyleDoors.ThreeDoor && numberOfDoors != (int)BodyStyleDoors.FiveDoor)
                throw new ArgumentOutOfRangeException(nameof(numberOfDoors), numberOfDoors, $"Value must be either {(int)BodyStyleDoors.ThreeDoor} or {(int)BodyStyleDoors.FiveDoor}.");

            NumberOfDoors = numberOfDoors;
        }
    }
}