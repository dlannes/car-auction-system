using CarAuctionManagement.Core.Common;
using CarAuctionManagement.Core.Models;

namespace CarAuctionManagement.Core.Validators
{
    public class BidValidator : IValidator<Bid>
    {
        public ValidationResult Validate(Bid bid)
        {
            ValidationResult result = new();

            if (bid.Amount <= 0)
            {
                result.AddError("Bid amount must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(bid.Bidder))
            {
                result.AddError("Bidder must be specified.");
            }

            if (bid.AuctionId == Guid.Empty)
            {
                result.AddError("Auction ID must be a valid GUID.");
            }

            return result;
        }
    }

}
