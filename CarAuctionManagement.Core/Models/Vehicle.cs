namespace CarAuctionManagement.Core.Models
{
    public abstract class Vehicle
    {
        private const int YearOfTheFirstCar = 1886;
        private static int MaxModelYear => DateTime.UtcNow.Year + 1;

        public Guid Id { get; protected set; }
        public string Manufacturer { get; protected set; }
        public string Model { get; protected set; }
        public int Year { get; protected set; }
        public decimal StartingBid { get; protected set; }

        protected Vehicle(Guid id, string manufacturer, string model, int year, decimal startingBid)
        {
            if (id == default)
                throw new ArgumentException($"A default (empty) Guid is not valid.", nameof(id));

            if (string.IsNullOrWhiteSpace(manufacturer))
                throw new ArgumentException($"Value can't be empty or whitespace.", nameof(manufacturer));

            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException($"Value can't be empty or whitespace.", nameof(model));

            if (year < YearOfTheFirstCar || year > MaxModelYear)
                throw new ArgumentOutOfRangeException(nameof(year), year, $"Value must be within {YearOfTheFirstCar} and {MaxModelYear}.");

            if (startingBid < 0)
                throw new ArgumentOutOfRangeException(nameof(startingBid), startingBid, $"Value can't be negative.");

            Id = id;
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
            StartingBid = startingBid;
        }
    }
}
