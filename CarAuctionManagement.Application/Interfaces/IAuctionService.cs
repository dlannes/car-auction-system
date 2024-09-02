using CarAuctionManagement.Application.DTOs;

namespace CarAuctionManagement.Application.Interfaces
{
    public interface IAuctionService
    {
        Task StartAuction(Guid vehicleId);
        Task CloseActiveAuction(Guid vehicleId);
        Task<BidDTO> PlaceBid(Guid auctionId, decimal amount, string bidder);
    }

}
