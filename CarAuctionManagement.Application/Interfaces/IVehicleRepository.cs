using CarAuctionManagement.Core.Common;

namespace CarAuctionManagement.Application.Interfaces
{
    public interface IVehicleRepository
    {
        Task Add(Vehicle vehicle);
        Task<Vehicle?> FindById(Guid vehicleId);
        Task<IEnumerable<Vehicle>> Search(string? vehicleType, string? manufacturer, string? model, int? year);
        Task<bool> Exists(Guid vehicleId);
    }
}
