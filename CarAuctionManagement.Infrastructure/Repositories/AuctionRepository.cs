using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAuctionManagement.Infrastructure.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        public Task Add(Auction auction)
        {
            throw new NotImplementedException();
        }

        public Task CloseAuction(Guid auctionId, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public Task<Auction?> FindActiveByVehicleId(Guid vehicleId)
        {
            throw new NotImplementedException();
        }

        public Task GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task AddBid(Bid bid)
        {
            throw new NotImplementedException();
        }

        public Task<List<Bid>> GetAllBids(Guid auctionId)
        {
            throw new NotImplementedException();
        }

        public Task<Bid?> FindHighestBidById(Guid auctionId)
        {
            throw new NotImplementedException();
        }
    }
}