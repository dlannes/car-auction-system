using CarAuctionManagement.Core.Common;

namespace CarAuctionManagement.Core.Exceptions
{
    public class NegativeBidAmountException : DomainException
    {
        public NegativeBidAmountException(decimal bidValue)
            : base($"The bid of `{bidValue:C}` is not valid because it is less than zero.")
        {
        }
    }
}
