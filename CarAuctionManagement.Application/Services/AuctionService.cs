using CarAuctionManagement.Application.Exceptions;
using CarAuctionManagement.Application.Interfaces;
using CarAuctionManagement.Core.Common;
using CarAuctionManagement.Core.Models;

namespace CarAuctionManagement.Application.Services
{
    public class AuctionService(
        IVehicleRepository vehicleRepository,
        IAuctionRepository auctionRepository,
        IBidRepository bidRepository
        ) : IAuctionService
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
        private readonly IAuctionRepository _auctionRepository = auctionRepository;
        private readonly IBidRepository _bidRepository = bidRepository;


        public async Task StartAuction(Guid vehicleId)
        {
            var vehicle = await _vehicleRepository.FindById(vehicleId)
                ?? throw new EntityNotFoundException(nameof(Vehicle), nameof(Vehicle.Id), vehicleId);

            var activeAuction = await _auctionRepository.FindActiveByVehicleId(vehicleId);
            if (activeAuction != null)
                throw new ValidationException($"There is already an active {nameof(Auction)} for the vehicle with ID '{vehicleId}'.");

            var newAuction = Auction.Create(vehicleId);
            var initalBid = Bid.Create(newAuction.Id, vehicle.StartingBid);

            await _auctionRepository.Add(newAuction);
            await _bidRepository.Add(initalBid);
        }

        public async Task CloseActiveAuction(Guid vehicleId)
        {
            var auction = await _auctionRepository.FindActiveByVehicleId(vehicleId)
                ?? throw new EntityNotFoundException($"Active {nameof(Auction)}", nameof(Auction.VehicleId), vehicleId);

            await _auctionRepository.CloseAuction(auction.Id);
        }

        public async Task PlaceBid(Guid auctionId, decimal amount)
        {
            var auction = await _auctionRepository.FindById(auctionId)
                ?? throw new EntityNotFoundException(nameof(Auction), nameof(Auction.Id), auctionId);
            if (!auction.IsActive)
                throw new ValidationException($"{nameof(Auction)} with ID '{auctionId}' is not active.");

            var highestBid = await _bidRepository.FindHighestByAuctionId(auctionId);
            if (highestBid != null && amount <= highestBid.Amount)
                throw new ValidationException($"{nameof(Bid)} {nameof(amount)} must be greater than the current highest bid.");

            var newBid = Bid.Create(auctionId, amount);
            await _bidRepository.Add(newBid);
        }
    }
}