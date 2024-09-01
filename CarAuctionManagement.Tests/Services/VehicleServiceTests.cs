using CarAuctionManagement.Application.DTOs;
using CarAuctionManagement.Application.Exceptions;
using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Application.Mappings;
using CarAuctionManagement.Application.Services;
using CarAuctionManagement.Core.Common;
using CarAuctionManagement.Core.Exceptions;
using CarAuctionManagement.Core.Models;
using Moq;
using System.Reflection;

namespace CarAuctionManagement.Tests.Services
{
    public class VehicleServiceTests
    {
        private readonly Mock<IVehicleRepository> _mockRepository;
        private readonly IVehicleService _vehicleService;
        private readonly VehicleMapper _vehicleMapper;

        public VehicleServiceTests()
        {
            _mockRepository = new Mock<IVehicleRepository>();
            _vehicleMapper = new VehicleMapper();
            _vehicleService = new VehicleService(_mockRepository.Object, _vehicleMapper);
        }

        [Fact]
        public async Task AddVehicle_ShouldAddVehicleSuccessfully()
        {
            var expectedId = Guid.NewGuid();
            var vehicleDTO = new VehicleDTO {Id = expectedId, VehicleType = "Sedan", Manufacturer = "Toyota", Model = "Camry", Year = 2022, NumberOfDoors = 4, StartingBid = 5000m };
            var vehicle = new Sedan(expectedId, "Toyota", "Camry", 2022, 5000m, 4);

            _mockRepository.Setup(repo => repo.Exists(expectedId)).ReturnsAsync(false);
            _mockRepository.Setup(repo => repo.Add(It.Is<Vehicle>(v => v.Id == expectedId))).Returns(Task.CompletedTask);

            await _vehicleService.AddVehicle(vehicleDTO);

            _mockRepository.Verify(repo => repo.Add(It.Is<Vehicle>(v => v.Id == expectedId)), Times.Once);
        }

        [Fact]
        public async Task AddVehicle_ShouldThrowExceptionOnDuplicateId()
        {
            var vehicleMapper = new VehicleMapper();
            var vehicleDTO = new VehicleDTO { Id = Guid.NewGuid(), VehicleType = "Sedan", Manufacturer = "Toyota", Model = "Camry", Year = 2022, NumberOfDoors = 4, StartingBid = 5000m };
            var vehicle = new Sedan((Guid)vehicleDTO.Id, "Toyota", "Camry", 2022, 5000m, 4);

            _mockRepository.Setup(repo => repo.Exists(It.IsAny<Guid>())).ReturnsAsync(true);

            await Assert.ThrowsAsync<ValidationException>(() => _vehicleService.AddVehicle(vehicleDTO));
        }

        [Fact]
        public async Task SearchVehicles_ShouldReturnCorrectResults()
        {
            var vehicles = new List<Vehicle>
            {
                new Sedan(Guid.NewGuid(), "Toyota", "Camry", 2020, 5000m, 4),
                new SUV(Guid.NewGuid(), "Toyota", "Highlander", 2022, 10000m, 7)
            };

            _mockRepository.Setup(repo => repo.Search("SUV", "Toyota", null, null)).ReturnsAsync(vehicles.Where(v => v.GetType().Name.Equals("suv", StringComparison.OrdinalIgnoreCase) && v.Manufacturer == "Toyota").ToList());

            var result = await _vehicleService.SearchVehicles("SUV", "Toyota", null, null);

            Assert.Single(result);
            Assert.Equal("Toyota", result.First().Manufacturer);
            Assert.Equal("Highlander", result.First().Model);
        }
    }
}
