using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Core.Models;

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

        public Task<Vehicle?> GetById(Guid vehicleId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Vehicle>> Search(string? vehicleType, string? manufacturer, string? model, int? year)
        {
            throw new NotImplementedException();
        }
    }
}
