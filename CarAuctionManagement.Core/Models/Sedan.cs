using CarAuctionManagement.Core.Common;

namespace CarAuctionManagement.Core.Models
{
    public class Sedan : Vehicle
    {
        public int NumberOfDoors { get; set; }

        public Sedan(Guid id, string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
            : base(id, manufacturer, model, year, startingBid)
        {
            NumberOfDoors = numberOfDoors;
        }
    }
}
