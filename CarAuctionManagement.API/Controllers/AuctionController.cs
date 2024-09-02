using CarAuctionManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarAuctionManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuctionController(IAuctionManager auctionService) : ControllerBase
    {
        private readonly IAuctionManager _auctionService = auctionService;

        [HttpGet]
        public async Task<IActionResult> Start(Guid vehicleId)
        {
            await _auctionService.StartAuction(vehicleId);
            return Ok();
        }

        // GET api/<AuctionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuctionController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuctionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuctionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
