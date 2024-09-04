using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Core.Models;

namespace CarAuctionManagement.Infrastructure.Repositories
{
    public class BidRepository : IBidRepository
    {
        public Task Add(Bid bid)
        {
            throw new NotImplementedException();
        }

        public Task<Bid?> FindHighestByAuctionId(Guid auctionId)
        {
            throw new NotImplementedException();
        }
    }
}
