using CarAuctionManagement.Application.DTOs;
using CarAuctionManagement.Core.Models;

namespace CarAuctionManagement.Application.Interfaces
{
    public interface IVehicleService
    {
        Task AddVehicle(VehicleDTO vehicle);
        Task<List<VehicleDTO>> SearchVehicles(string? vehicleType, string? manufacturer, string? model, int? year);
    }
}
