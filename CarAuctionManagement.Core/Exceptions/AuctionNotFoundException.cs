using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarAuctionManagement.Core.Common;

namespace CarAuctionManagement.Core.Exceptions
{
    public class AuctionNotFoundException : DomainException
    {
        public AuctionNotFoundException(Guid vehicleId)
            : base($"No active auction found for the vehicle with ID `{vehicleId}`.")
        {
        }
    }
}
