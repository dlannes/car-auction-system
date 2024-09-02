using CarAuctionManagement.Core.Common;
using CarAuctionManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAuctionManagement.Application.Interfaces
{
    public interface IVehicleRepository
    {
        Task Add(Vehicle vehicle);
        Task<List<Vehicle>> GetAll();
        Task<Vehicle?> GetById(Guid vehicleId);
        Task<List<Vehicle>> Search(string? vehicleType, string? manufacturer, string? model, int? year);
        Task<bool> Exists(Guid vehicleId);
    }

}
