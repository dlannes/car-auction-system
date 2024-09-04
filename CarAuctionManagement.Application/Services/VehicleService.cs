using CarAuctionManagement.Application.DTOs;
using CarAuctionManagement.Application.Exceptions;
using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Application.Mappings;

namespace CarAuctionManagement.Application.Services
{
    public class VehicleService(IVehicleRepository vehicleRepository, VehicleMapper vehicleMapper) : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly VehicleMapper _vehicleMapper = vehicleMapper;

        public async Task AddToIventory(VehicleDTO vehicleDTO)
        {
            vehicleDTO.Id = Guid.NewGuid();
            var vehicle = _vehicleMapper.MapToVehicle(vehicleDTO);

            if (await _vehicleRepository.Exists(vehicle.Id))
            {
                throw new ValidationException($"A {nameof(vehicle)} with the {nameof(vehicle.Id)} '{vehicle.Id}' already exists.");
            }

            await _vehicleRepository.Add(vehicle);
        }

        public async Task<List<VehicleDTO>> SearchVehicles(string? vehicleType, string? manufacturer, string? model, int? year)
        {
            if (vehicleType == null && manufacturer == null && model == null && year == null)
            {
                throw new ArgumentException("At least one of the search arguments must have a value.");
            }

            if (!string.IsNullOrEmpty(vehicleType) && !_vehicleMapper.GetRegisteredVehicleTypes().Contains(vehicleType))
            {
                throw new ArgumentException($"Invalid vehicle type provided.", nameof(vehicleType));
            }

            var vehicles = await _vehicleRepository.Search(vehicleType, manufacturer, model, year);

            return vehicles.Any() ? vehicles.Select(_vehicleMapper.MapToDTO).ToList() : [];
        }
    }
}
