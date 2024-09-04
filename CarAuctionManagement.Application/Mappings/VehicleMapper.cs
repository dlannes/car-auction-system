using CarAuctionManagement.Application.DTOs;
using CarAuctionManagement.Core.Common;
using System.Reflection;

namespace CarAuctionManagement.Application.Mappings
{
    public sealed class VehicleMapper
    {
        private readonly Dictionary<string, Func<VehicleDTO, Vehicle>> DTOToVehicleMappers = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Func<Vehicle, VehicleDTO>> VehicleToDTOMappers = new(StringComparer.OrdinalIgnoreCase);

        public VehicleMapper()
        {
            var vehicleTypes = typeof(Vehicle).Assembly
                .GetTypes().Where(t => t.IsSubclassOf(typeof(Vehicle)));

            foreach (var vehicleType in vehicleTypes)
            {
                RegisterDTOToVehicleMapper(vehicleType);
                RegisterVehicleToDTOMapper(vehicleType);
            }
        }

        public HashSet<string> GetRegisteredVehicleTypes() => DTOToVehicleMappers.Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);

        public Vehicle MapToVehicle(VehicleDTO vehicleDTO)
        {
            if (string.IsNullOrWhiteSpace(vehicleDTO.VehicleType))
            {
                throw new ArgumentException("VehicleType is required.", nameof(VehicleDTO.VehicleType));
            }

            if (!DTOToVehicleMappers.TryGetValue(vehicleDTO.VehicleType, out var constructor))
            {
                throw new ArgumentException($"Vehicle type {vehicleDTO.VehicleType} is not recognized.", nameof(vehicleDTO.VehicleType));
            }

            return constructor(vehicleDTO);
        }

        public VehicleDTO MapToDTO(Vehicle vehicle)
        {
            var vehicleType = vehicle.GetType().Name.ToLower();
            if (!VehicleToDTOMappers.TryGetValue(vehicleType, out var mapper))
            {
                throw new ArgumentException($"Vehicle type {vehicleType} is not recognized.", nameof(vehicleType));
            }

            return mapper(vehicle);
        }

        private void RegisterDTOToVehicleMapper(Type vehicleType)
        {
            var ctor = vehicleType.GetConstructors().FirstOrDefault();
            if (ctor == null)
            {
                throw new InvalidOperationException($"Vehicle type {vehicleType.Name} does not have a public constructor.");
            }

            DTOToVehicleMappers[vehicleType.Name.ToLower()] = dto =>
            {
                var ctorParams = ctor.GetParameters();
                var parameters = new object?[ctorParams.Length];

                for (int i = 0; i < ctorParams.Length; i++)
                {
                    var paramName = ctorParams[i].Name;
                    if (string.IsNullOrWhiteSpace(paramName)) continue;
                    parameters[i] = GetParameterValue(dto, paramName, vehicleType);
                }
                try
                {
                    return (Vehicle)ctor.Invoke(parameters);
                }
                catch (TargetInvocationException ex) when (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
            };
        }

        private static object? GetParameterValue(VehicleDTO dto, string paramName, Type vehicleType)
        {
            var property = typeof(VehicleDTO).GetProperty(paramName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                throw new ArgumentException($"Property {paramName} is not supported for the vehicle type {vehicleType.Name}.");
            }

            var value = property.GetValue(dto);
            if (value == null && Nullable.GetUnderlyingType(property.PropertyType) == null)
            {
                throw new ArgumentException($"Property {paramName} is required for the specified vehicle type {vehicleType.Name}.");
            }

            return value;
        }

        private void RegisterVehicleToDTOMapper(Type vehicleType)
        {
            VehicleToDTOMappers[vehicleType.Name] = vehicle =>
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
                    var dtoProperty = typeof(VehicleDTO).GetProperty(prop.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        ?? throw new InvalidOperationException($"Property '{prop.Name}' is required for the vehicle type '{vehicleType.Name}' but is missing in {nameof(VehicleDTO)}.");

                    if (dtoProperty.CanWrite)
                    {
                        var value = prop.GetValue(vehicle);
                        dtoProperty.SetValue(dto, value);
                    }
                }

                return dto;
            };
        }
    }
}
