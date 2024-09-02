namespace CarAuctionManagement.Core.Models
{
    public class Hatchback : Vehicle
    {
        public int NumberOfDoors { get; set; }

        public Hatchback(Guid id, string manufacturer, string model, int year, decimal startingBid, int numberOfDoors)
            : base(id, manufacturer, model, year, startingBid)
        {
            NumberOfDoors = numberOfDoors;
        }
    }

}