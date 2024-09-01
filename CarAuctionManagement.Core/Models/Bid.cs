namespace CarAuctionManagement.Core.Models
{
    public class Bid
    {
        public required Guid Id { get; set; }
        public required Guid AuctionId { get; set; }
        public required DateTime Timestamp { get; set; }
        public required decimal Amount { get; set; }
    }
}
