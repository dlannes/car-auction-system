namespace CarAuctionManagement.Application.Interfaces
{
    public interface IAuctionService
    {
        Task StartAuction(Guid vehicleId);
        Task CloseActiveAuction(Guid vehicleId);
        Task PlaceBid(Guid vehicleId, decimal bidAmount);
    }

}
