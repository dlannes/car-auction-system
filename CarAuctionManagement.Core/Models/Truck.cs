namespace CarAuctionManagement.Core.Models
{
    public class Truck : Vehicle
    {
        public decimal LoadCapacity { get; set; }

        public Truck(Guid id, string manufacturer, string model, int year, decimal startingBid, decimal loadCapacity)
            : base(id, manufacturer, model, year, startingBid)
        {
            LoadCapacity = loadCapacity;
        }
    }

}
