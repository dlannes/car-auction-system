using CarAuctionManagement.Core.Models;

namespace CarAuctionManagement.Application.Interfaces
{
    public interface IAuctionRepository
    {
        Task Add(Auction auction);
        Task<Auction?> FindById(Guid auctionId);
        Task CloseAuction(Guid auctionId);
        Task<Auction?> FindActiveByVehicleId(Guid vehicleId);
    }
}
