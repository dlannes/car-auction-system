namespace CarAuctionManagement.Core.Models
{
    public class Bid
    {
        public Guid Id { get; }
        public Guid AuctionId { get; }
        public decimal Amount { get; }
        public string Bidder { get; }
        public DateTime Timestamp { get; }

        public Bid(Guid id, Guid auctionId, decimal amount, string bidder, DateTime timestamp)
        {
            Id = id;
            AuctionId = auctionId;
            Amount = amount;
            Bidder = bidder;
            Timestamp = timestamp;
        }
    }
}
