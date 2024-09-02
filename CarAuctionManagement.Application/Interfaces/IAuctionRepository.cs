using CarAuctionManagement.Core.Models;

namespace CarAuctionManagement.Application.Interfaces
{
    public interface IAuctionRepository
    {
        Task Add(Auction auction);
        Task<Auction?> GetById(int id);
        Task CloseAuction(Guid auctionId, DateTime endTime);
        Task AddBid(Bid bid);
        Task<List<Bid>> GetBidsById(Guid auctionId);
        Task<Bid?> FindHighestBidById(Guid auctionId);
        Task<Auction?> FindActiveByVehicleId(Guid vehicleId);
    }
}
