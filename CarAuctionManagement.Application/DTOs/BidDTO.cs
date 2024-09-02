namespace CarAuctionManagement.Application.DTOs
{
    public class BidDTO
    {
        public Guid BidId { get; set; }
        public Guid VehicleId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
