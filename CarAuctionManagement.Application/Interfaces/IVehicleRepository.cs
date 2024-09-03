using CarAuctionManagement.Core.Models;

namespace CarAuctionManagement.Application.Interfaces
{
    public interface IVehicleRepository
    {
        Task Add(Vehicle vehicle);
        Task<Vehicle?> GetById(Guid vehicleId);
        Task<List<Vehicle>> Search(string? vehicleType, string? manufacturer, string? model, int? year);
        Task<bool> Exists(Guid vehicleId);
    }
}
