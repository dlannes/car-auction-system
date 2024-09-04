using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Core.Models;

namespace CarAuctionManagement.Infrastructure.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        public Task Add(Auction auction)
        {
            throw new NotImplementedException();
        }

        public Task AddBid(Bid bid)
        {
            throw new NotImplementedException();
        }

        public Task CloseAuction(Guid auctionId)
        {
            throw new NotImplementedException();
        }

        public Task<Auction?> FindActiveByVehicleId(Guid vehicleId)
        {
            throw new NotImplementedException();
        }

        public Task<Auction?> FindById(Guid auctionId)
        {
            throw new NotImplementedException();
        }
    }
}