using System.Security.Cryptography;

namespace CarAuctionManagement.Core.Models
{
    public class Bid
    {
        public Guid Id { get; }
        public Guid AuctionId { get;}
        public decimal Amount { get; }

        public Bid(Guid id, Guid auctionId, decimal amount)
        {
            if (id == Guid.Empty)
                throw new ArgumentException($"{nameof(Guid)} cannot be empty.", nameof(id));

            if (auctionId == Guid.Empty)
                throw new ArgumentException($"{nameof(Guid)} cannot be empty.", nameof(auctionId));

            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Value must be greater than zero.");

            Id = id;
            AuctionId = auctionId;
            Amount = amount;
        }

        public static Bid Create(Guid auctionId, decimal amount)
        {
            return new Bid(Guid.NewGuid(), auctionId, amount);
        }
    }
}
