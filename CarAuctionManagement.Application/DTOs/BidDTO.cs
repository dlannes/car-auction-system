namespace CarAuctionManagement.Application.DTOs
{
    public class BidDTO
    {
        public Guid? Id { get; }
        public Guid AuctionId { get; }
        public decimal Amount { get; }
    }
}
