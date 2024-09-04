using CarAuctionManagement.Application.DTOs;
using CarAuctionManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace CarAuctionManagement.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class VehicleController(IVehicleService vehicleService) : ControllerBase
    {
        private readonly IVehicleService _vehicleService = vehicleService;

        [HttpPost("add")]
        public async Task<IActionResult> AddToInventory([FromBody] VehicleDTO vehicleDTO)
        {
            await _vehicleService.AddToIventory(vehicleDTO);
            return Ok($"Vehicle '{vehicleDTO.Manufacturer} {vehicleDTO.Model}' has been added to inventory.");
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchVehicles(
            [FromQuery] string? vehicleType,
            [FromQuery] string? manufacturer,
            [FromQuery] string? model,
            [FromQuery] int? year)
        {
            var vehicles = await _vehicleService.SearchVehicles(vehicleType, manufacturer, model, year);
            return Ok(vehicles);
        }
    }

}
