using CarAuctionManagement.Application.Exceptions;
using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Application.Services;
using CarAuctionManagement.Core.Common;
using CarAuctionManagement.Core.Models;
using Moq;

namespace CarAuctionManagement.Tests.Application
{
    public class AuctionServiceTests
    {
        private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
        private readonly Mock<IAuctionRepository> _auctionRepositoryMock;
        private readonly Mock<IBidRepository> _bidRepositoryMock;
        private readonly AuctionService _auctionService;

        public AuctionServiceTests()
        {
            _vehicleRepositoryMock = new Mock<IVehicleRepository>();
            _auctionRepositoryMock = new Mock<IAuctionRepository>();
            _bidRepositoryMock = new Mock<IBidRepository>();
            _auctionService = new AuctionService(
                _vehicleRepositoryMock.Object,
                _auctionRepositoryMock.Object,
                _bidRepositoryMock.Object
            );
        }

        // StartAuction
        [Fact]
        public async Task StartAuction_ShouldThrowEntityNotFoundException_WhenVehicleNotFound()
        {
            var vehicleId = Guid.NewGuid();
            _vehicleRepositoryMock.Setup(repo => repo.FindById(vehicleId))
                .ReturnsAsync(null as Vehicle);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => _auctionService.StartAuction(vehicleId));
        }

        [Fact]
        public async Task StartAuction_ShouldThrowValidationException_WhenActiveAuctionExists()
        {
            var vehicleId = Guid.NewGuid();
            var mockVehicle = new Mock<Vehicle>(vehicleId, "Toyota", "Corolla", 2022, 10000M);
            var existingAuction = Auction.Create(vehicleId);

            _vehicleRepositoryMock.Setup(repo => repo.FindById(vehicleId)).ReturnsAsync(mockVehicle.Object);
            _auctionRepositoryMock.Setup(repo => repo.FindActiveByVehicleId(vehicleId)).ReturnsAsync(existingAuction);

            await Assert.ThrowsAsync<ValidationException>(() => _auctionService.StartAuction(vehicleId));
        }

        [Fact]
        public async Task StartAuction_ShouldThrowArgumentOutOfRange_WhenStartingBidIsInvalid()
        {
            var vehicleId = Guid.NewGuid();
            var mockVehicle = new Mock<Vehicle>(vehicleId, "Toyota", "Corolla", 2022, 0M);

            _vehicleRepositoryMock.Setup(repo => repo.FindById(vehicleId)).ReturnsAsync(mockVehicle.Object);
            _auctionRepositoryMock.Setup(repo => repo.FindActiveByVehicleId(vehicleId)).ReturnsAsync(null as Auction);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _auctionService.StartAuction(vehicleId));
        }

        [Fact]
        public async Task StartAuction_ShouldStartAuction_WhenNoActiveAuctionExists()
        {
            var vehicleId = Guid.NewGuid();
            var mockVehicle = new Mock<Vehicle>(vehicleId, "Toyota", "Corolla", 2022, 10000M);

            _vehicleRepositoryMock.Setup(repo => repo.FindById(vehicleId)).ReturnsAsync(mockVehicle.Object);
            _auctionRepositoryMock.Setup(repo => repo.FindActiveByVehicleId(vehicleId)).ReturnsAsync(null as Auction);

            await _auctionService.StartAuction(vehicleId);

            _auctionRepositoryMock.Verify(repo => repo.Add(It.IsAny<Auction>()), Times.Once);
            _bidRepositoryMock.Verify(repo => repo.Add(It.IsAny<Bid>()), Times.Once);
        }

        // CloseActiveAuction
        [Fact]
        public async Task CloseActiveAuction_ShouldThrowEntityNotFoundException_WhenNoActiveAuctionExists()
        {
            var vehicleId = Guid.NewGuid();

            _auctionRepositoryMock.Setup(repo => repo.FindActiveByVehicleId(vehicleId)).ReturnsAsync(null as Auction);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => _auctionService.CloseActiveAuction(vehicleId));
        }

        [Fact]
        public async Task CloseActiveAuction_ShouldCloseActiveAuction_WhenAuctionExists()
        {
            var vehicleId = Guid.NewGuid();
            var activeAuction = Auction.Create(vehicleId);

            _auctionRepositoryMock.Setup(repo => repo.FindActiveByVehicleId(vehicleId)).ReturnsAsync(activeAuction);

            await _auctionService.CloseActiveAuction(vehicleId);

            _auctionRepositoryMock.Verify(repo => repo.CloseAuction(activeAuction.Id), Times.Once);
        }

        // PlaceBid
        [Fact]
        public async Task PlaceBid_ShouldThrowEntityNotFoundException_WhenNoAuctionExists()
        {
            var auctionId = Guid.NewGuid();
            var amount = 5000M;

            _auctionRepositoryMock.Setup(repo => repo.FindById(auctionId)).ReturnsAsync(null as Auction);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => _auctionService.PlaceBid(auctionId, amount));
        }

        [Fact]
        public async Task PlaceBid_ShouldThrowValidationException_WhenAuctionIsNotActive()
        {
            var auctionId = Guid.NewGuid();
            var amount = 5000M;
            var inactiveAuction = new Auction(auctionId, Guid.NewGuid(), false);

            _auctionRepositoryMock.Setup(repo => repo.FindById(auctionId)).ReturnsAsync(inactiveAuction);

            await Assert.ThrowsAsync<ValidationException>(() => _auctionService.PlaceBid(auctionId, amount));
        }

        [Fact]
        public async Task PlaceBid_ShouldThrowValidationException_WhenBidIsNotHigherThanHighestBid()
        {
            var auctionId = Guid.NewGuid();
            var amount = 5000M;
            var activeAuction = new Auction(auctionId, Guid.NewGuid(), true);
            var highestBid = Bid.Create(auctionId, 6000M);

            _auctionRepositoryMock.Setup(repo => repo.FindById(auctionId)).ReturnsAsync(activeAuction);
            _bidRepositoryMock.Setup(repo => repo.FindHighestByAuctionId(auctionId)).ReturnsAsync(highestBid);

            await Assert.ThrowsAsync<ValidationException>(() => _auctionService.PlaceBid(auctionId, amount));
        }

        [Fact]
        public async Task PlaceBid_ShouldThrowValidationException_WhenBidIsEqualToHighestBid()
        {
            var auctionId = Guid.NewGuid();
            var amount = 6000m;
            var activeAuction = new Auction(auctionId, Guid.NewGuid(), true);
            var highestBid = Bid.Create(auctionId, amount);

            _auctionRepositoryMock.Setup(repo => repo.FindById(auctionId)).ReturnsAsync(activeAuction);
            _bidRepositoryMock.Setup(repo => repo.FindHighestByAuctionId(auctionId)).ReturnsAsync(highestBid);

            await Assert.ThrowsAsync<ValidationException>(() => _auctionService.PlaceBid(auctionId, amount));
        }

        [Fact]
        public async Task PlaceBid_ShouldThrowArgumentOutOfRange_WhenBidAmountIsInvalid()
        {
            var auctionId = Guid.NewGuid();
            var invalidAmount = -1000m;
            var activeAuction = new Auction(auctionId, Guid.NewGuid(), true);

            _auctionRepositoryMock.Setup(repo => repo.FindById(auctionId)).ReturnsAsync(activeAuction);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException> (() => _auctionService.PlaceBid(auctionId, invalidAmount));
        }

        [Fact]
        public async Task PlaceBid_ShouldPlaceBid_WhenValid()
        {
            var auctionId = Guid.NewGuid();
            var amount = 7000m;
            var activeAuction = new Auction(auctionId, Guid.NewGuid(), true);
            var highestBid = Bid.Create(auctionId, 6000M);

            _auctionRepositoryMock.Setup(repo => repo.FindById(auctionId)).ReturnsAsync(activeAuction);
            _bidRepositoryMock.Setup(repo => repo.FindHighestByAuctionId(auctionId)).ReturnsAsync(highestBid);

            await _auctionService.PlaceBid(auctionId, amount);
            _bidRepositoryMock.Verify(repo => repo.Add(It.Is<Bid>(b => b.Amount == amount)), Times.Once);
        }

        [Fact]
        public async Task PlaceBid_ShouldPlaceBid_WhenNoPreviousHighestBidExists()
        {
            var auctionId = Guid.NewGuid();
            var amount = 5000m;
            var activeAuction = new Auction(auctionId, Guid.NewGuid(), true);

            _auctionRepositoryMock.Setup(repo => repo.FindById(auctionId)).ReturnsAsync(activeAuction);
            _bidRepositoryMock.Setup(repo => repo.FindHighestByAuctionId(auctionId)).ReturnsAsync(null as Bid);

            await _auctionService.PlaceBid(auctionId, amount);

            _bidRepositoryMock.Verify(repo => repo.Add(It.Is<Bid>(b => b.Amount == amount)), Times.Once);
        }
    }
}