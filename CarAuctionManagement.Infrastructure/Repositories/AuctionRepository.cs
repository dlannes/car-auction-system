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
        public Task Close(Guid auctionId, DateTime endTime)
        {
            throw new NotImplementedException();
        }

        public Task<Auction?> FindActiveByVehicleId(Guid vehicleId)
        {
            throw new NotImplementedException();
        }

        public Task Start(Auction auction)
        {
            throw new NotImplementedException();
        }
    }
}
