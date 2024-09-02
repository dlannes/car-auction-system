namespace CarAuctionManagement.Core.Models
{
    public abstract class Vehicle
    {
        public Guid Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal StartingBid { get; set; }

        protected Vehicle(Guid id, string manufacturer, string model, int year, decimal startingBid)
        {
            Id = id;
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
            StartingBid = startingBid;
        }
    }

}
