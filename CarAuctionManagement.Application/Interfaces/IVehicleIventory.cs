using CarAuctionManagement.Application.DTOs;
using CarAuctionManagement.Core.Models;

namespace CarAuctionManagement.Application.Interfaces
{
    public interface IVehicleIventory
    {
        Task AddVehicle(VehicleDTO vehicle);
        Task<List<VehicleDTO>> SearchVehicles(string? vehicleType, string? manufacturer, string? model, int? year);
    }
}
