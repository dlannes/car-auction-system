namespace CarAuctionManagement.Core.Models
{
    public class SUV : Vehicle
    {
        private const int MinSeats = 4;
        private const int MaxSeats = 9;

        public int NumberOfSeats { get; private set; }

        public SUV(Guid id, string manufacturer, string model, int year, decimal startingBid, int numberOfSeats)
            : base(id, manufacturer, model, year, startingBid)
        {
            if (numberOfSeats < MinSeats || numberOfSeats > MaxSeats)
                throw new ArgumentOutOfRangeException(nameof(numberOfSeats), numberOfSeats, $"Value must be within {MinSeats} and {MaxSeats}.");

            NumberOfSeats = numberOfSeats;
        }
    }
}
