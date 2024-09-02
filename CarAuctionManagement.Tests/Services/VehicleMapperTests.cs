using CarAuctionManagement.Application.DTOs;
using CarAuctionManagement.Application.Mappings;
using CarAuctionManagement.Core.Models;
using System.Reflection;

namespace CarAuctionManagement.Tests.Services
{
    public class VehicleMapperTests
    {
        private readonly VehicleMapper _vehicleMapper;

        public VehicleMapperTests() {
            _vehicleMapper = new VehicleMapper();
        }

        [Fact]
        public void MapToVehicle_ShouldReturnSedan_WhenValidSedanDTOProvided()
        {
            var vehicleDTO = new VehicleDTO
            {
                VehicleType = "sedan",
                Manufacturer = "Toyota",
                Model = "Camry",
                Year = 2022,
                StartingBid = 10000m,
                NumberOfDoors = 4
            };

            var result = _vehicleMapper.MapToVehicle(vehicleDTO);

            var sedan = Assert.IsType<Sedan>(result);
            Assert.Equal(vehicleDTO.Manufacturer, sedan.Manufacturer);
            Assert.Equal(vehicleDTO.Model, sedan.Model);
            Assert.Equal(vehicleDTO.Year, sedan.Year);
            Assert.Equal(vehicleDTO.StartingBid, sedan.StartingBid);
            Assert.Equal(vehicleDTO.NumberOfDoors, sedan.NumberOfDoors);
        }

        [Fact]
        public void MapToVehicle_ShouldThrowException_WhenVehicleTypeIsUnknown()
        {
            var vehicleDTO = new VehicleDTO
            {
                VehicleType = "unknown",
                Manufacturer = "Toyota",
                Model = "Camry",
                Year = 2022,
                StartingBid = 10000m
            };

            var exception = Assert.Throws<ArgumentException>(() => _vehicleMapper.MapToVehicle(vehicleDTO));
            Assert.Equal("Vehicle type `unknown` is not recognized. (Parameter 'VehicleType')", exception.Message);
        }

        [Fact]
        public void MapToVehicleDTO_ShouldReturnCorrectDTO_WhenValidVehicleProvided()
        {
            // Arrange
            var sedan = new Sedan(Guid.NewGuid(), "Toyota", "Camry", 2022, 10000m, 4);

            // Act
            var result = _vehicleMapper.MapToVehicleDTO(sedan);

            // Assert
            Assert.Equal("sedan", result.VehicleType, ignoreCase: true);
            Assert.Equal(sedan.Manufacturer, result.Manufacturer);
            Assert.Equal(sedan.Model, result.Model);
            Assert.Equal(sedan.Year, result.Year);
            Assert.Equal(sedan.StartingBid, result.StartingBid);
            Assert.Equal(sedan.NumberOfDoors, result.NumberOfDoors);
        }


        [Fact]
        public void AllVehicleTypes_ShouldBeRegisteredInVehicleTypeToConstructor()
        {
            var assembly = Assembly.GetAssembly(typeof(Vehicle))!;
            var vehicleTypes = assembly.GetTypes().Where(t => t.IsClass && t.IsSubclassOf(typeof(Vehicle)));

            foreach (var vehicleType in vehicleTypes)
            {
                Assert.True(_vehicleMapper.GetRegisteredVehicleTypes().Contains(vehicleType.Name),
                    $"Vehicle type `{vehicleType.Name}` is not registered in the `VehicleMapper`.");
            }
        }
    }
}