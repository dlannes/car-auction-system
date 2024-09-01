using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarAuctionManagement.Core.Common;

namespace CarAuctionManagement.Core.Exceptions
{
    public class BidLowerThanCurrentException : DomainException
    {
        public BidLowerThanCurrentException(decimal currentBid, decimal newBid)
            : base($"The bid of `{newBid:C}` is not valid. The current highest bid is `{currentBid:C}`.")
        {
        }
    }

}
