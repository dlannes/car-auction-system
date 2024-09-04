using CarAuctionManagement.Application.DTOs;
using CarAuctionManagement.Application.Exceptions;
using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Application.Mappings;
using CarAuctionManagement.Application.Services;
using CarAuctionManagement.Core.Common;
using CarAuctionManagement.Core.Models;
using Moq;

namespace CarAuctionManagement.Tests.Application
{
    public class VehicleServiceTests
    {
        private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
        private readonly VehicleMapper _vehicleMapper;
        private readonly VehicleService _vehicleService;

        public VehicleServiceTests()
        {
            _vehicleRepositoryMock = new Mock<IVehicleRepository>();
            _vehicleMapper = new VehicleMapper();
            _vehicleService = new VehicleService(_vehicleRepositoryMock.Object, _vehicleMapper);
        }

        //AddToInventory

        [Fact]
        public async Task AddToIventory_ShouldThrowArgumentException_WhenManufacturerIsEmpty()
        {
            var vehicleDTO = new VehicleDTO
            {
                Manufacturer = "",
                Model = "ModelX",
                Year = 2021,
                StartingBid = 50000M,
                VehicleType = "SUV"
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _vehicleService.AddToIventory(vehicleDTO));
        }

        [Fact]
        public async Task AddToIventory_ShouldThrowArgumentException_WhenModelIsEmpty()
        {
            var vehicleDTO = new VehicleDTO
            {
                Manufacturer = "Tesla",
                Model = "",
                Year = 2021,
                StartingBid = 50000M,
                VehicleType = "SUV"
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _vehicleService.AddToIventory(vehicleDTO));
        }

        [Fact]
        public async Task AddToIventory_ShouldThrowArgumentOutOfRangeException_WhenYearIsBelow1886()
        {
            var vehicleDTO = new VehicleDTO
            {
                Manufacturer = "Ford",
                Model = "ModelT",
                Year = 1800,
                StartingBid = 50000M,
                VehicleType = "Truck"
            };

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _vehicleService.AddToIventory(vehicleDTO));
        }

        [Fact]
        public async Task AddToIventory_ShouldThrowArgumentOutOfRangeException_WhenYearIsBeyondNextYear()
        {
            var futureYear = DateTime.UtcNow.Year + 2;
            var vehicleDTO = new VehicleDTO
            {
                Manufacturer = "Tesla",
                Model = "ModelX",
                Year = futureYear,
                StartingBid = 50000M,
                VehicleType = "SUV"
            };

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _vehicleService.AddToIventory(vehicleDTO));
        }

        [Fact]
        public async Task AddToIventory_ShouldThrowArgumentOutOfRangeException_WhenStartingBidIsNegative()
        {
            var vehicleDTO = new VehicleDTO
            {
                Manufacturer = "Tesla",
                Model = "ModelX",
                Year = 2021,
                StartingBid = -10000M,
                VehicleType = "SUV"
            };

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _vehicleService.AddToIventory(vehicleDTO));
        }
        [Fact]
        public async Task AddToIventory_ShouldThrowValidationException_WhenVehicleWithSameIdExists()
        {
            var vehicleDTO = new VehicleDTO
            {
                Manufacturer = "Toyota",
                Model = "Corolla",
                Year = 2021,
                StartingBid = 15000M,
                VehicleType = "Sedan",
                NumberOfDoors = 4
            };

            _vehicleRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Guid>())).ReturnsAsync(true);

            await Assert.ThrowsAsync<ValidationException>(() => _vehicleService.AddToIventory(vehicleDTO));
        }


        [Fact]
        public async Task AddToIventory_ShouldAddVehicle_WhenValid()
        {
            var vehicleDTO = new VehicleDTO { Manufacturer = "Tesla", Model = "ModelX", Year = 2023, StartingBid = 50000m, VehicleType = "SUV", NumberOfSeats = 7 };
            var vehicle = new SUV(Guid.NewGuid(), "Tesla", "ModelX", 2023, 50000m, 7);

            _vehicleRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Guid>())).ReturnsAsync(false);

            await _vehicleService.AddToIventory(vehicleDTO);

            _vehicleRepositoryMock.Verify(repo => repo.Add(It.Is<SUV>(s =>
                s.Manufacturer == "Tesla" &&
                s.Model == "ModelX" &&
                s.Year == 2023 &&
                s.StartingBid == 50000m &&
                s.NumberOfSeats == 7)), Times.Once);
        }

        // SearchVehicles
        [Fact]
        public async Task SearchVehicles_ShouldThrowArgumentException_WhenNoSearchCriteriaProvided()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _vehicleService.SearchVehicles(null, null, null, null));
        }

        [Fact]
        public async Task SearchVehicles_ShouldReturnEmptyList_WhenNoVehiclesFound()
        {
            _vehicleRepositoryMock.Setup(repo =>
                repo.Search(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>()))
                .ReturnsAsync(new List<Vehicle>());

            var result = await _vehicleService.SearchVehicles("SUV", "Toyota", null, 2021);

            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchVehicles_ShouldReturnVehicles_WhenMatchingCriteria()
        {
            var vehicles = new List<Vehicle> { new SUV(Guid.NewGuid(), "Toyota", "RAV4", 2021, 25000M, 7) };
            var vehicleDTO = new VehicleDTO { Id = vehicles.First().Id, VehicleType = vehicles.First().GetType().Name, Manufacturer = vehicles.First().Manufacturer, Model = vehicles.First().Model, Year = vehicles.First().Year };

            _vehicleRepositoryMock.Setup(repo =>
                repo.Search(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>()))
                .ReturnsAsync(vehicles);

            var result = await _vehicleService.SearchVehicles("SUV", "Toyota", "RAV4", 2021);

            Assert.Single(result);
            Assert.Equal("Toyota", result.First().Manufacturer);
        }

        //vehicle type specific tests
        // Truck tests
        [Fact]
        public async Task AddToIventory_ShouldThrowArgumentOutOfRangeException_WhenTruckLoadCapacityIsOutOfRange()
        {
            var vehicleDTO = new VehicleDTO { Manufacturer = "Ford", Model = "F-150", Year = 2021, StartingBid = 30000M, LoadCapacity = 10000, VehicleType = "Truck" };

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _vehicleService.AddToIventory(vehicleDTO));
        }

        [Fact]
        public async Task AddToIventory_ShouldAddTruck_WhenLoadCapacityIsAtMinimumBoundary()
        {
            var vehicleDTO = new VehicleDTO { Manufacturer = "Ford", Model = "F-150", Year = 2021, StartingBid = 30000M, LoadCapacity = 500, VehicleType = "Truck" };
            var truck = new Truck(Guid.NewGuid(), "Ford", "F-150", 2021, 30000M, 500);

            _vehicleRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Guid>())).ReturnsAsync(false);

            await _vehicleService.AddToIventory(vehicleDTO);

            _vehicleRepositoryMock.Verify(repo => repo.Add(It.Is<Truck>(s =>
                s.Manufacturer == "Ford" &&
                s.Model == "F-150" &&
                s.Year == 2021 &&
                s.StartingBid == 30000m &&
                s.LoadCapacity == 500)), Times.Once);
        }

        [Fact]
        public async Task AddToIventory_ShouldAddTruck_WhenLoadCapacityIsAtMaximumBoundary()
        {
            var vehicleDTO = new VehicleDTO { Manufacturer = "Ford", Model = "F-150", Year = 2021, StartingBid = 30000M, LoadCapacity = 5000, VehicleType = "Truck" };
            var truck = new Truck(Guid.NewGuid(), "Ford", "F-150", 2021, 30000M, 5000);

            _vehicleRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Guid>())).ReturnsAsync(false);

            await _vehicleService.AddToIventory(vehicleDTO);

            _vehicleRepositoryMock.Verify(repo => repo.Add(It.Is<Truck>(s =>
                s.Manufacturer == "Ford" &&
                s.Model == "F-150" &&
                s.Year == 2021 &&
                s.StartingBid == 30000m &&
                s.LoadCapacity == 5000)), Times.Once);
        }

        // SUV 
        [Fact]
        public async Task AddToIventory_ShouldThrowArgumentOutOfRangeException_WhenSUVNumberOfSeatsIsOutOfRange()
        {
            var vehicleDTO = new VehicleDTO { Manufacturer = "Toyota", Model = "RAV4", Year = 2021, StartingBid = 25000M, NumberOfSeats = 2, VehicleType = "SUV" };

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _vehicleService.AddToIventory(vehicleDTO));
        }

        [Fact]
        public async Task AddToIventory_ShouldAddSUV_WhenNumberOfSeatsIsAtMinimumBoundary()
        {
            var vehicleDTO = new VehicleDTO { Manufacturer = "Toyota", Model = "RAV4", Year = 2021, StartingBid = 25000M, NumberOfSeats = 4, VehicleType = "SUV" };
            var suv = new SUV(Guid.NewGuid(), "Toyota", "RAV4", 2021, 25000M, 4);

            _vehicleRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Guid>())).ReturnsAsync(false);

            await _vehicleService.AddToIventory(vehicleDTO);

            _vehicleRepositoryMock.Verify(repo => repo.Add(It.Is<SUV>(s =>
                s.Manufacturer == "Toyota" &&
                s.Model == "RAV4" &&
                s.Year == 2021 &&
                s.StartingBid == 25000m &&
                s.NumberOfSeats == 4)), Times.Once);
        }

        [Fact]
        public async Task AddToIventory_ShouldAddSUV_WhenNumberOfSeatsIsAtMaximumBoundary()
        {
            var vehicleDTO = new VehicleDTO { Manufacturer = "Toyota", Model = "RAV4", Year = 2021, StartingBid = 25000m, NumberOfSeats = 9, VehicleType = "SUV" };

            _vehicleRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Guid>())).ReturnsAsync(false);

            await _vehicleService.AddToIventory(vehicleDTO);

            _vehicleRepositoryMock.Verify(repo => repo.Add(It.Is<SUV>(s =>
                s.Manufacturer == "Toyota" &&
                s.Model == "RAV4" &&
                s.Year == 2021 &&
                s.StartingBid == 25000m &&
                s.NumberOfSeats == 9)), Times.Once);
        }

        // Sedan
        [Fact]
        public async Task AddToIventory_ShouldThrowArgumentOutOfRangeException_WhenSedanNumberOfDoorsIsOutOfRange()
        {
            var vehicleDTO = new VehicleDTO { Manufacturer = "Toyota", Model = "Camry", Year = 2021, StartingBid = 20000M, NumberOfDoors = 5, VehicleType = "Sedan" };

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _vehicleService.AddToIventory(vehicleDTO));
        }

        [Fact]
        public async Task AddToIventory_ShouldAddSedan_WhenNumberOfDoorsIsAtMinimumBoundary()
        {
            var vehicleDTO = new VehicleDTO { Manufacturer = "Toyota", Model = "Camry", Year = 2021, StartingBid = 20000m, NumberOfDoors = 2, VehicleType = "Sedan" };
            var sedan = new Sedan(Guid.NewGuid(), "Toyota", "Camry", 2021, 20000M, 2);

            _vehicleRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Guid>())).ReturnsAsync(false);

            await _vehicleService.AddToIventory(vehicleDTO);

            _vehicleRepositoryMock.Verify(repo => repo.Add(It.Is<Sedan>(s =>
                s.Manufacturer == "Toyota" &&
                s.Model == "Camry" &&
                s.Year == 2021 &&
                s.StartingBid == 20000m &&
                s.NumberOfDoors == 2)), Times.Once);
        }

        [Fact]
        public async Task AddToIventory_ShouldAddSedan_WhenNumberOfDoorsIsAtMaximumBoundary()
        {
            var vehicleDTO = new VehicleDTO { Manufacturer = "Toyota", Model = "Camry", Year = 2021, StartingBid = 20000M, NumberOfDoors = 4, VehicleType = "Sedan" };
            var sedan = new Sedan(Guid.NewGuid(), "Toyota", "Camry", 2021, 20000M, 4);

            _vehicleRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Guid>())).ReturnsAsync(false);

            await _vehicleService.AddToIventory(vehicleDTO);

            _vehicleRepositoryMock.Verify(repo => repo.Add(It.Is<Sedan>(s =>
                s.Manufacturer == "Toyota" &&
                s.Model == "Camry" &&
                s.Year == 2021 &&
                s.StartingBid == 20000m &&
                s.NumberOfDoors == 4)), Times.Once);
        }

        // Hatchback
        [Fact]
        public async Task AddToIventory_ShouldThrowArgumentOutOfRangeException_WhenHatchbackNumberOfDoorsIsInvalid()
        {
            var vehicleDTO = new VehicleDTO { Manufacturer = "Volkswagen", Model = "Golf", Year = 2021, StartingBid = 18000M, NumberOfDoors = 4, VehicleType = "Hatchback" };

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _vehicleService.AddToIventory(vehicleDTO));
        }

        [Fact]
        public async Task AddToIventory_ShouldAddHatchback_WhenNumberOfDoorsIsValid()
        {
            var vehicleDTO = new VehicleDTO { Manufacturer = "Volkswagen", Model = "Golf", Year = 2021, StartingBid = 18000M, NumberOfDoors = 3, VehicleType = "Hatchback" };
            var hatchback = new Hatchback(Guid.NewGuid(), "Volkswagen", "Golf", 2021, 18000M, 3);

            _vehicleRepositoryMock.Setup(repo => repo.Exists(It.IsAny<Guid>())).ReturnsAsync(false);

            await _vehicleService.AddToIventory(vehicleDTO);

            _vehicleRepositoryMock.Verify(repo => repo.Add(It.Is<Hatchback>(s =>
                s.Manufacturer == "Volkswagen" &&
                s.Model == "Golf" &&
                s.Year == 2021 &&
                s.StartingBid == 18000m &&
                s.NumberOfDoors == 3)), Times.Once);
        }
    }
}