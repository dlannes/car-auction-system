using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarAuctionManagement.Core.Common;

namespace CarAuctionManagement.Core.Exceptions
{
    public class AuctionAlreadyActiveException : DomainException
    {
        public AuctionAlreadyActiveException(Guid vehicleId)
            : base($"An auction for the vehicle with ID `{vehicleId}` is already active.")
        {
        }
    }
}
