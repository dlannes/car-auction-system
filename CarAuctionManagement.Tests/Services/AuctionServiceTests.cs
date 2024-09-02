using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Application.Services;
using CarAuctionManagement.Core.Exceptions;
using CarAuctionManagement.Core.Models;
using Moq;

namespace CarAuctionManagement.Tests.Services
{
    public class AuctionServiceTests
    {
        private readonly Mock<IVehicleRepository> _mockVehicleRepository;
        private readonly Mock<IAuctionRepository> _mockAuctionRepository;
        private readonly Mock<IBidRepository> _mockBidRepository;
        private readonly AuctionService _auctionService;

        public AuctionServiceTests()
        {
            _mockVehicleRepository = new Mock<IVehicleRepository>();
            _mockAuctionRepository = new Mock<IAuctionRepository>();
            _mockBidRepository = new Mock<IBidRepository>();

            _auctionService = new AuctionService(
                _mockVehicleRepository.Object,
                _mockAuctionRepository.Object,
                _mockBidRepository.Object);
        }

        [Fact]
        public async Task StartAuction_ShouldStartAuction_WhenNoActiveAuctionExists()
        {
            var vehicleId = Guid.NewGuid();
            var vehicle = new Sedan(Guid.NewGuid(), "Toyota", "Camry", 2022, 5000m, 4);

            _mockVehicleRepository.Setup(repo => repo.GetById(vehicleId)).ReturnsAsync(vehicle);
            _mockAuctionRepository.Setup(repo => repo.FindActiveByVehicleId(vehicleId)).ReturnsAsync(null as Auction);

            await _auctionService.StartAuction(vehicleId);

            _mockAuctionRepository.Verify(repo => repo.Add(It.Is<Auction>(a => a.VehicleId == vehicleId)), Times.Once);
        }

        [Fact]
        public async Task StartAuction_ShouldThrowException_WhenVehicleNotFound()
        {
            var vehicleId = Guid.NewGuid();
            _mockVehicleRepository.Setup(repo => repo.GetById(vehicleId)).ReturnsAsync(null as Vehicle);

            await Assert.ThrowsAsync<VehicleNotFoundException>(() => _auctionService.StartAuction(vehicleId));
        }

        [Fact]
        public async Task StartAuction_ShouldThrowException_WhenActiveAuctionExists()
        {
            var vehicleId = Guid.NewGuid();
            var vehicle = new Sedan(Guid.NewGuid(), "Toyota", "Camry", 2022, 5000m, 4);
            var activeAuction = new Auction { Id = Guid.NewGuid(), VehicleId = vehicleId, StartTime = DateTime.UtcNow };

            _mockVehicleRepository.Setup(repo => repo.GetById(vehicleId)).ReturnsAsync(vehicle);
            _mockAuctionRepository.Setup(repo => repo.FindActiveByVehicleId(vehicleId)).ReturnsAsync(activeAuction);

            await Assert.ThrowsAsync<AuctionAlreadyActiveException>(() => _auctionService.StartAuction(vehicleId));
        }

        [Fact]
        public async Task CloseActiveAuction_ShouldCloseAuction_WhenActiveAuctionExists()
        {
            var vehicleId = Guid.NewGuid();
            var auction = new Auction { Id = Guid.NewGuid(), VehicleId = vehicleId, StartTime = DateTime.UtcNow };

            _mockAuctionRepository.Setup(repo => repo.FindActiveByVehicleId(vehicleId)).ReturnsAsync(auction);

            await _auctionService.CloseActiveAuction(vehicleId);

            _mockAuctionRepository.Verify(repo => repo.CloseAuction(auction.Id, It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async Task CloseActiveAuction_ShouldThrowException_WhenNoActiveAuctionExists()
        {
            var vehicleId = Guid.NewGuid();

            _mockAuctionRepository.Setup(repo => repo.FindActiveByVehicleId(vehicleId)).ReturnsAsync(null as Auction);

            await Assert.ThrowsAsync<AuctionNotFoundException>(() => _auctionService.CloseActiveAuction(vehicleId));
        }

        [Fact]
        public async Task PlaceBid_ShouldPlaceBid_WhenBidAmountIsHigherThanCurrent()
        {
            var vehicleId = Guid.NewGuid();
            var auction = new Auction { Id = Guid.NewGuid(), VehicleId = vehicleId, StartTime = DateTime.UtcNow };
            var highestBid = new Bid { Id = Guid.NewGuid(), AuctionId = auction.Id, Amount = 100m, Timestamp = DateTime.UtcNow };
            var bidAmount = 150m;

            _mockAuctionRepository.Setup(repo => repo.FindActiveByVehicleId(vehicleId)).ReturnsAsync(auction);
            _mockBidRepository.Setup(repo => repo.FindHighestBidByAuctionId(vehicleId)).ReturnsAsync(highestBid);

            await _auctionService.PlaceBid(vehicleId, bidAmount);

            _mockBidRepository.Verify(repo => repo.Add(It.Is<Bid>(b => b.Amount == bidAmount && b.AuctionId == auction.Id)), Times.Once);
        }

        [Fact]
        public async Task PlaceBid_ShouldThrowException_WhenNoActiveAuctionExists()
        {
            var vehicleId = Guid.NewGuid();

            _mockAuctionRepository.Setup(repo => repo.FindActiveByVehicleId(vehicleId)).ReturnsAsync(null as Auction);

            await Assert.ThrowsAsync<AuctionNotFoundException>(() => _auctionService.PlaceBid(vehicleId, 100m));
        }

        [Fact]
        public async Task PlaceBid_ShouldThrowException_WhenBidAmountIsLowerThanCurrentHighestBid()
        {
            var vehicleId = Guid.NewGuid();
            var auction = new Auction { Id = Guid.NewGuid(), VehicleId = vehicleId, StartTime = DateTime.UtcNow };
            var highestBid = new Bid { Id = Guid.NewGuid(), AuctionId = auction.Id, Amount = 100m, Timestamp = DateTime.UtcNow };
            var bidAmount = 90m;

            _mockAuctionRepository.Setup(repo => repo.FindActiveByVehicleId(vehicleId)).ReturnsAsync(auction);
            _mockBidRepository.Setup(repo => repo.FindHighestBidByAuctionId(vehicleId)).ReturnsAsync(highestBid);

            await Assert.ThrowsAsync<BidLowerThanCurrentException>(() => _auctionService.PlaceBid(vehicleId, bidAmount));
        }

        [Fact]
        public async Task PlaceBid_ShouldHandleConcurrentBidsCorrectly()
        {
            var vehicleId = Guid.NewGuid();
            var auctionId = Guid.NewGuid();
            var auction = new Auction { Id = auctionId, VehicleId = vehicleId, StartTime = DateTime.UtcNow };
            var highestBid = new Bid { Id = Guid.NewGuid(), AuctionId = auctionId, Amount = 100m, Timestamp = DateTime.UtcNow };

            _mockAuctionRepository.Setup(repo => repo.FindActiveByVehicleId(vehicleId)).ReturnsAsync(auction);
            _mockBidRepository.Setup(repo => repo.FindHighestBidByAuctionId(vehicleId)).ReturnsAsync(highestBid);

            var tasks = new[]
            {
                Task.Run(() => _auctionService.PlaceBid(vehicleId, 150m)),
                Task.Run(() => _auctionService.PlaceBid(vehicleId, 160m))
            };

            await Task.WhenAll(tasks);

            _mockBidRepository.Verify(repo => repo.Add(It.IsAny<Bid>()), Times.Exactly(2));
            _mockBidRepository.Verify(repo => repo.Add(It.Is<Bid>(b => b.Amount == 160m && b.AuctionId == auctionId)), Times.Once);
            _mockBidRepository.Verify(repo => repo.Add(It.Is<Bid>(b => b.Amount == 150m && b.AuctionId == auctionId)), Times.Once);
        }
    }
}
