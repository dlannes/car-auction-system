using CarAuctionManagement.Core.Common;

namespace CarAuctionManagement.Core.Models
{
    public class Auction
    {
        public required Guid Id { get; set; }
        public required Guid VehicleId { get; set; }
        public required DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsActive => EndTime == null;
        public Vehicle? Vehicle { get; set; }
        public List<Bid> Bids { get; set; } = [];
    }
}
