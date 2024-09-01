using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<List<Vehicle>> GetAll()
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
