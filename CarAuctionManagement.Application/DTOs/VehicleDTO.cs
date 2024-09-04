namespace CarAuctionManagement.Application.DTOs
{
    public class VehicleDTO
    {
        public Guid? Id { get; set; }
        public required string VehicleType { get; set; }
        public required string Manufacturer { get; set; }
        public required string Model { get; set; }
        public required int Year { get; set; }
        public decimal StartingBid { get; set; }
        public int? NumberOfDoors { get; set; }
        public int? NumberOfSeats { get; set; }
        public double? LoadCapacity { get; set; }
    }
}
