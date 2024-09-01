using System.Reflection;
using System.Collections.Generic;
using CarAuctionManagement.Application.DTOs;
using CarAuctionManagement.Core.Common;

namespace CarAuctionManagement.Application.Mappings
{
    public sealed class VehicleMapper
    {
        private readonly Dictionary<string, Func<VehicleDTO, Vehicle>> _vehicleTypeToConstructor = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Func<Vehicle, VehicleDTO>> _vehicleToDTOMapper = new(StringComparer.OrdinalIgnoreCase);

        public VehicleMapper()
        {
            RegisterTypes();
        }

        public HashSet<string> GetRegisteredVehicleTypes() => _vehicleTypeToConstructor.Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);

        public Vehicle MapToVehicle(VehicleDTO vehicleDTO)
        {
            if (string.IsNullOrWhiteSpace(vehicleDTO.VehicleType))
            {
                throw new ArgumentException("`VehicleType` is required.", nameof(vehicleDTO.VehicleType));
            }

            if (!_vehicleTypeToConstructor.TryGetValue(vehicleDTO.VehicleType, out var constructor))
            {
                throw new ArgumentException($"Vehicle type `{vehicleDTO.VehicleType}` is not recognized.", nameof(vehicleDTO.VehicleType));
            }

            return constructor(vehicleDTO);
        }

        public VehicleDTO MapToVehicleDTO(Vehicle vehicle)
        {
            var vehicleType = vehicle.GetType().Name.ToLower();
            if (!_vehicleToDTOMapper.TryGetValue(vehicleType, out var mapper))
            {
                throw new ArgumentException($"Vehicle type `{vehicleType}` is not recognized.", nameof(vehicleType));
            }

            return mapper(vehicle);
        }

        private void RegisterTypes() {
            var assembly = typeof(Vehicle).Assembly;
            var vehicleTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Vehicle)));

            foreach (var type in vehicleTypes)
            {
                RegisterVehicleType(type);
                RegisterDTOType(type);
            }
        }

        private void RegisterVehicleType(Type vehicleType)
        {
            var ctor = vehicleType.GetConstructors().FirstOrDefault();
            if (ctor == null)
            {
                throw new InvalidOperationException($"Vehicle type `{vehicleType.Name}` does not have a public constructor.");
            }

            _vehicleTypeToConstructor[vehicleType.Name.ToLower()] = dto =>
            {
                var ctorParams = ctor.GetParameters();
                var parameters = new object?[ctorParams.Length];

                for (int i = 0; i < ctorParams.Length; i++)
                {
                    var paramName = ctorParams[i].Name;
                    if (string.IsNullOrWhiteSpace(paramName)) continue;
                    parameters[i] = GetParameterValue(dto, paramName, vehicleType);
                }

                return (Vehicle)ctor.Invoke(parameters);
            };
        }

        private void RegisterDTOType(Type vehicleType)
        {
            _vehicleToDTOMapper[vehicleType.Name] = vehicle =>
            {
                var dto = new VehicleDTO
                {
                    VehicleType = vehicleType.Name,
                    Manufacturer = string.Empty,
                    Model = string.Empty,
                    Year = default
                };

                var vehicleProperties = vehicleType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in vehicleProperties)
                {
                    var dtoProperty = typeof(VehicleDTO).GetProperty(prop.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (dtoProperty != null && dtoProperty.CanWrite)
                    {
                        var value = prop.GetValue(vehicle);
                        dtoProperty.SetValue(dto, value);
                    }
                }

                return dto;
            };
        }

        private static object? GetParameterValue(VehicleDTO dto, string paramName, Type vehicleType)
        {
            var property = typeof(VehicleDTO).GetProperty(paramName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                throw new ArgumentException($"Parameter `{paramName}` is not supported for the vehicle type `{vehicleType.Name}`.");
            }

            var value = property.GetValue(dto);
            if (value == null && Nullable.GetUnderlyingType(property.PropertyType) == null)
            {
                throw new ArgumentException($"`{paramName}` is required for the specified vehicle type `{vehicleType.Name}`.");
            }

            return value;
        }
    }
}
