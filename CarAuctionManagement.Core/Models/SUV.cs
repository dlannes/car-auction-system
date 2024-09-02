namespace CarAuctionManagement.Core.Models
{
    public class SUV : Vehicle
    {
        public int NumberOfSeats { get; set; }

        public SUV(Guid id, string manufacturer, string model, int year, decimal startingBid, int numberOfSeats)
            : base(id, manufacturer, model, year, startingBid)
        {
            NumberOfSeats = numberOfSeats;
        }
    }
}
