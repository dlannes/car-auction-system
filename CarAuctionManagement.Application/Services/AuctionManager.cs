using CarAuctionManagement.Application.DTOs;
using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Core.Exceptions;
using CarAuctionManagement.Core.Models;
using CarAuctionManagement.Core.Validators;

namespace CarAuctionManagement.Application.Services
{
    public class AuctionManager(
        IVehicleRepository vehicleRepository,
        IAuctionRepository auctionRepository) : IAuctionManager
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly IAuctionRepository _auctionRepository = auctionRepository;

        public async Task StartAuction(Guid vehicleId)
        {
            var vehicle = await _vehicleRepository.GetById(vehicleId);
            if(vehicle == null)
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
            await _auctionRepository.Add(newAuction);
        }

        public async Task CloseActiveAuction(Guid vehicleId)
        {
            var auction = await _auctionRepository.FindActiveByVehicleId(vehicleId);
            if (auction == null)
            {
                throw new AuctionNotFoundException(vehicleId);
            }

            await _auctionRepository.CloseAuction(auction.Id, DateTime.UtcNow);
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

            var highestBid = await _auctionRepository.FindHighestBidById(vehicleId);
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

        public async Task<BidDTO> PlaceBid(Guid auctionId, decimal amount, string bidder)
        {
            var test = await _auctionRepository.Get
            var activeAuction = await _auctionRepository.FindActiveByVehicleId(auctionId);
            if (activeAuction == null)
            {
                throw new ValidationException($"No active auction found for {nameof(auctionId)}: `vehicleId`.");
            }

            var bid = new Bid(Guid.NewGuid(), activeAuction.Id, amount, bidder, DateTime.UtcNow);

            var validator = new BidValidator().Validate(bid);
            if (!validator.IsValid)
            {
                throw new ValidationException(validator.GetErrorMessage());
            }

            var highestBid = await _bidRepository.FindHighestBidByAuctionId(activeAuction.Id);
            if (highestBid != null && bid.Amount <= highestBid.Amount)
            {
                throw new ValidationException(string.Format("{0} must be greater than the current highest bid.", nameof(bid.Amount)));
            }

            await _bidRepository.Add(bid);
            return;
        }
    }

}