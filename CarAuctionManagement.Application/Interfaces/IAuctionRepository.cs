using CarAuctionManagement.Core.Models;

namespace CarAuctionManagement.Application.Interfaces
{
    public interface IAuctionRepository
    {
        Task Start(Auction auction);
        Task Close(Guid auctionId, DateTime endTime);
        Task<Auction?> FindActiveByVehicleId(Guid vehicleId);
    }
}
