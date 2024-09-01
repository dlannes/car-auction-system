using CarAuctionManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAuctionManagement.Application.Interfaces
{
    public interface IBidRepository
    {
        Task Add(Bid bid);
        Task<IEnumerable<Bid>> GetAllByAuctionId(Guid auctionId);
        Task<Bid?> FindHighestByAuctionId(Guid auctionId);
    }
}
