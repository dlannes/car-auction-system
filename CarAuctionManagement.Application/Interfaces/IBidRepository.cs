using CarAuctionManagement.Core.Models;

namespace CarAuctionManagement.Application.Interfaces
{
    public interface IBidRepository
    {
        Task Add(Bid bid);
        Task<IEnumerable<Bid>> GetAllByAuctionId(Guid auctionId);
        Task<Bid?> FindHighestByAuctionId(Guid auctionId);
    }
}
