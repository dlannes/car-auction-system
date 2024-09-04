using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Core.Common;

namespace CarAuctionManagement.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        public Task Add(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(Guid vehicleId)
        {
            throw new NotImplementedException();
        }

        public Task<Vehicle?> FindById(Guid vehicleId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Vehicle>> Search(string? vehicleType, string? manufacturer, string? model, int? year)
        {
            throw new NotImplementedException();
        }
    }
}
