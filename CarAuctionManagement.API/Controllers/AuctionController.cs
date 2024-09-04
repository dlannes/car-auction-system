using CarAuctionManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarAuctionManagement.API.Controllers
{
    using CarAuctionManagement.Application.DTOs;
    using CarAuctionManagement.Application.Exceptions;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class AuctionController(IAuctionService auctionService) : ControllerBase
    {
        private readonly IAuctionService _auctionService = auctionService;

        [HttpGet("start/{vehicleId}")]
        public async Task<IActionResult> StartAuction(Guid vehicleId)
        {
            await _auctionService.StartAuction(vehicleId);
            return Ok($"Auction for vehicle with ID '{vehicleId}' has started.");
        }

        [HttpGet("close/{vehicleId}")]
        public async Task<IActionResult> CloseAuction(Guid vehicleId)
        {
            await _auctionService.CloseActiveAuction(vehicleId);
            return Ok($"Auction for vehicle with ID '{vehicleId}' has been closed.");
        }

        [HttpPost]
        public async Task<IActionResult> PlaceBid([FromBody] BidDTO dto)
        {
            await _auctionService.PlaceBid(dto.AuctionId, dto.Amount);
            return Ok($"Bid of {dto.Amount} placed on auction '{dto.AuctionId}'.");
        }
    }
}
