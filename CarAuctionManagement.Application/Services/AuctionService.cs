using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Core.Exceptions;
using CarAuctionManagement.Core.Models;

namespace CarAuctionManagement.Application.Services
{
    public class AuctionService(
        IVehicleRepository vehicleRepository,
        IAuctionRepository auctionRepository,
        IBidRepository bidRepository) : IAuctionService
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly IAuctionRepository _auctionRepository = auctionRepository;
        private readonly IBidRepository _bidRepository = bidRepository;

        public async Task StartAuction(Guid vehicleId)
        {
            var vehicle = await _vehicleRepository.GetById(vehicleId);
            if (vehicle == null)
            {
                throw new VehicleNotFoundException(vehicleId);
            }

            var existingAuction = await _auctionRepository.FindActiveByVehicleId(vehicleId);
            if (existingAuction != null)
            {
                throw new AuctionAlreadyActiveException(vehicleId);
            }

            var newAuction = new Auction
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicleId,
                StartTime = DateTime.UtcNow,
            };
            await _auctionRepository.Start(newAuction);
        }

        public async Task CloseActiveAuction(Guid vehicleId)
        {
            var auction = await _auctionRepository.FindActiveByVehicleId(vehicleId);
            if (auction == null)
            {
                throw new AuctionNotFoundException(vehicleId);
            }

            await _auctionRepository.Close(auction.Id, DateTime.UtcNow);
        }

        public async Task PlaceBid(Guid vehicleId, decimal bidAmount)
        {
            if (bidAmount < 0)
            {
                throw new NegativeBidAmountException(bidAmount);
            }

            var auction = await _auctionRepository.FindActiveByVehicleId(vehicleId);
            if (auction == null)
            {
                throw new AuctionNotFoundException(vehicleId);
            }

            var highestBid = await _bidRepository.FindHighestByAuctionId(vehicleId);
            if (highestBid != null && bidAmount <= highestBid.Amount)
            {
                throw new BidLowerThanCurrentException(bidAmount, highestBid.Amount);
            }

            var newBid = new Bid
            {
                Id = Guid.NewGuid(),
                AuctionId = auction.Id,
                Timestamp = DateTime.Now,
                Amount = bidAmount
            };

            await _bidRepository.Add(newBid);
        }
    }

}