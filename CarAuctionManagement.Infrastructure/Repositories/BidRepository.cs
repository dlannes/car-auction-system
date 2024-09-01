using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<IEnumerable<Bid>> GetAllByAuctionId(Guid auctionId)
        {
            throw new NotImplementedException();
        }
    }
}
