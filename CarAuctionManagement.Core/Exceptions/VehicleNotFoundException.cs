using CarAuctionManagement.Core.Common;

namespace CarAuctionManagement.Core.Exceptions
{
    public class VehicleNotFoundException : DomainException
    {
        public VehicleNotFoundException(Guid vehicleId)
            : base($"The vehicle with ID `{vehicleId}` was not found.")
        {
        }
    }
}
