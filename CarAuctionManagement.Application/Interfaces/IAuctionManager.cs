using CarAuctionManagement.Application.DTOs;

namespace CarAuctionManagement.Application.Interfaces
{
    public interface IAuctionManager
    {
        Task StartAuction(Guid vehicleId);
        Task CloseActiveAuction(Guid vehicleId);
        Task<BidDTO> PlaceBid(Guid auctionId, decimal amount, string bidder);
    }

}
