using CarAuctionManagement.Core.Common;

namespace CarAuctionManagement.Core.Exceptions
{
    public class VehicleAlreadyExistsException : DomainException
    {
        public VehicleAlreadyExistsException(Guid vehicleId)
            : base($"A vehicle with the ID `{vehicleId}` already exists.")
        {
        }
    }

}
