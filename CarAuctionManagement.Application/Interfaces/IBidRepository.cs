using CarAuctionManagement.Core.Models;

namespace CarAuctionManagement.Application.Interfaces
{
    public interface IBidRepository
    {
        Task Add(Bid bid);
        Task<Bid?> FindHighestByAuctionId(Guid auctionId);
    }
}
