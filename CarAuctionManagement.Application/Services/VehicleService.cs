using CarAuctionManagement.Application.DTOs;
using CarAuctionManagement.Application.Exceptions;
using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Application.Mappings;
using CarAuctionManagement.Core.Common;
using CarAuctionManagement.Core.Validators;

namespace CarAuctionManagement.Application.Services
{
    public class VehicleService(IVehicleRepository vehicleRepository, VehicleMapper vehicleMapper) : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly VehicleMapper _vehicleMapper = vehicleMapper;

        public async Task AddVehicle(VehicleDTO vehicleDTO)
        {
            vehicleDTO.Id ??= Guid.NewGuid();
            var vehicle = _vehicleMapper.MapToVehicle(vehicleDTO);

            var validator = new VehicleValidator().Validate(vehicle);
            if (!validator.IsValid)
            {
                throw new ValidationException(validator.GetErrorMessage());
            }

            if (await _vehicleRepository.Exists(vehicle.Id))
            {
                throw new ValidationException($"A vehicle with the ID `{vehicle.Id}` already exists.");
            }

            await _vehicleRepository.Add(vehicle);
        }

        public async Task<List<VehicleDTO>> SearchVehicles(string? vehicleType, string? manufacturer, string? model, int? year)
        {
            if (!string.IsNullOrEmpty(vehicleType) && !_vehicleMapper.GetRegisteredVehicleTypes().Contains(vehicleType))
            {
                throw new ArgumentException("Invalid vehicle type provided.", nameof(vehicleType));
            }

            if (year.HasValue && (year.Value < 1886 || year.Value > DateTime.UtcNow.Year))
            {
                throw new ArgumentException("Year must be between 1886 and the current year.", nameof(year));
            }

            var vehicles = await _vehicleRepository.Search(vehicleType, manufacturer, model, year);

            return vehicles.Select(_vehicleMapper.MapToVehicleDTO).ToList();
        }
    }
}
