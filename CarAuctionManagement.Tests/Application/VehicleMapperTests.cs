using CarAuctionManagement.Application.DTOs;
using CarAuctionManagement.Application.Mappings;
using CarAuctionManagement.Core.Models;

namespace CarAuctionManagement.Tests.Application
{
    public class VehicleMapperTests
    {
        private readonly VehicleMapper _vehicleMapper;

        public VehicleMapperTests()
        {
            _vehicleMapper = new VehicleMapper();
        }

        // Map to vehicle
        [Fact]
        public void MapToVehicle_ShouldMapTruckDTOToTruck_WhenValid()
        {
            var vehicleDTO = new VehicleDTO
            {
                Id = Guid.NewGuid(),
                VehicleType = "Truck",
                Manufacturer = "Ford",
                Model = "F-150",
                Year = 2021,
                StartingBid = 30000M,
                LoadCapacity = 2000
            };

            var vehicle = _vehicleMapper.MapToVehicle(vehicleDTO);

            Assert.IsType<Truck>(vehicle);
            Assert.Equal("Ford", vehicle.Manufacturer);
            Assert.Equal("F-150", vehicle.Model);
            Assert.Equal(2021, vehicle.Year);
            Assert.Equal(30000m, vehicle.StartingBid);
            Assert.Equal(2000, ((Truck)vehicle).LoadCapacity);
        }

        [Fact]
        public void MapToVehicle_ShouldMapSUVDTOToSUV_WhenValid()
        {
            var vehicleDTO = new VehicleDTO
            {
                Id = Guid.NewGuid(),
                VehicleType = "SUV",
                Manufacturer = "Toyota",
                Model = "RAV4",
                Year = 2021,
                StartingBid = 25000M,
                NumberOfSeats = 7
            };

            var vehicle = _vehicleMapper.MapToVehicle(vehicleDTO);

            Assert.IsType<SUV>(vehicle);
            Assert.Equal("Toyota", vehicle.Manufacturer);
            Assert.Equal("RAV4", vehicle.Model);
            Assert.Equal(2021, vehicle.Year);
            Assert.Equal(25000M, vehicle.StartingBid);
            Assert.Equal(7, ((SUV)vehicle).NumberOfSeats);
        }

        [Fact]
        public void MapToVehicle_ShouldMapSedanDTOToSedan_WhenValid()
        {
            var vehicleDTO = new VehicleDTO
            {
                Id = Guid.NewGuid(),
                VehicleType = "Sedan",
                Manufacturer = "Toyota",
                Model = "Camry",
                Year = 2021,
                StartingBid = 20000M,
                NumberOfDoors = 4
            };

            var vehicle = _vehicleMapper.MapToVehicle(vehicleDTO);

            Assert.IsType<Sedan>(vehicle);
            Assert.Equal("Toyota", vehicle.Manufacturer);
            Assert.Equal("Camry", vehicle.Model);
            Assert.Equal(2021, vehicle.Year);
            Assert.Equal(20000M, vehicle.StartingBid);
            Assert.Equal(4, ((Sedan)vehicle).NumberOfDoors);
        }

        [Fact]
        public void MapToVehicle_ShouldMapHatchbackDTOToHatchback_WhenValid()
        {
            var vehicleDTO = new VehicleDTO
            {
                Id = Guid.NewGuid(),
                VehicleType = "Hatchback",
                Manufacturer = "Volkswagen",
                Model = "Golf",
                Year = 2021,
                StartingBid = 18000M,
                NumberOfDoors = 5
            };

            var vehicle = _vehicleMapper.MapToVehicle(vehicleDTO);

            Assert.IsType<Hatchback>(vehicle);
            Assert.Equal("Volkswagen", vehicle.Manufacturer);
            Assert.Equal("Golf", vehicle.Model);
            Assert.Equal(2021, vehicle.Year);
            Assert.Equal(18000M, vehicle.StartingBid);
            Assert.Equal(5, ((Hatchback)vehicle).NumberOfDoors);
        }

        // Mapping to DTO
        [Fact]
        public void MapToDTO_ShouldMapTruckToDTO_WhenValid()
        {
            var truck = new Truck(Guid.NewGuid(), "Ford", "F-150", 2021, 30000M, 2000);

            var vehicleDTO = _vehicleMapper.MapToDTO(truck);

            Assert.Equal("Truck", vehicleDTO.VehicleType);
            Assert.Equal("Ford", vehicleDTO.Manufacturer);
            Assert.Equal("F-150", vehicleDTO.Model);
            Assert.Equal(2021, vehicleDTO.Year);
            Assert.Equal(30000M, vehicleDTO.StartingBid);
            Assert.Equal(2000, vehicleDTO.LoadCapacity);
        }

        [Fact]
        public void MapToDTO_ShouldMapSUVToDTO_WhenValid()
        {
            var suv = new SUV(Guid.NewGuid(), "Toyota", "RAV4", 2021, 25000M, 7);

            var vehicleDTO = _vehicleMapper.MapToDTO(suv);

            Assert.Equal("SUV", vehicleDTO.VehicleType);
            Assert.Equal("Toyota", vehicleDTO.Manufacturer);
            Assert.Equal("RAV4", vehicleDTO.Model);
            Assert.Equal(2021, vehicleDTO.Year);
            Assert.Equal(25000M, vehicleDTO.StartingBid);
            Assert.Equal(7, vehicleDTO.NumberOfSeats);
        }

        //
        [Fact]
        public void MapToVehicle_ShouldThrowArgumentException_WhenVehicleTypeIsEmpty()
        {
            var vehicleDTO = new VehicleDTO
            {
                Manufacturer = "Ford",
                Model = "F-150",
                Year = 2021,
                StartingBid = 30000M,
                LoadCapacity = 2000,
                VehicleType = ""
            };

            var exception = Assert.Throws<ArgumentException>(() => _vehicleMapper.MapToVehicle(vehicleDTO));
            Assert.Equal("VehicleType is required. (Parameter 'VehicleType')", exception.Message);
        }

        [Fact]
        public void MapToVehicle_ShouldThrowArgumentException_WhenVehicleTypeIsUnrecognized()
        {
            var vehicleDTO = new VehicleDTO
            {
                Id = Guid.NewGuid(),
                VehicleType = "InvalidType",
                Manufacturer = "Ford",
                Model = "F-150",
                Year = 2021,
                StartingBid = 30000M,
                LoadCapacity = 2000
            };

            var exception = Assert.Throws<ArgumentException>(() => _vehicleMapper.MapToVehicle(vehicleDTO));
            Assert.Equal("Vehicle type InvalidType is not recognized. (Parameter 'VehicleType')", exception.Message);
        }
    }
}