using DragonAcc.Service.Interfaces;
using DragonAcc.Service.Models.Auction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DragonAcc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : BaseController
    {
        private readonly IAuctionService _auctionService;
        public AuctionController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _auctionService.GetAll();
            return Response(result);
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            try
            {
                var result = await _auctionService.GetById(id);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] AddAuctionModel model)
        {
            try
            {
                var result = await _auctionService.Add(model);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
        [Authorize]
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _auctionService.Delete(id);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateAuctionModel model)
        {
            try
            {
                var result = await _auctionService.Update(model);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
        [Authorize]
        [HttpPost("update-current-price")]
        public async Task<IActionResult> UpdateCurrentPrice([FromBody] UpdateCurrentPriceModel model)
        {
            try
            {
                var result = await _auctionService.UpdateCurrentPrice(model);
                return Response(result);

            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
        [HttpGet("get-winner-by-auction-id")]
        public async Task<IActionResult> GetWinnerByAuctionId([FromQuery] int auctionId)
        {
            try
            {
                var result = await _auctionService.GetWinnerByAuctionId(auctionId);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
        [HttpGet("get-all-auction-detail")]
        public async Task<IActionResult> GetAllAuctionDetail([FromQuery] int auctionId)
        {
            try
            {
                var result = await _auctionService.GetAllAuctionDetail(auctionId);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
        [Authorize]
        [HttpPost("add-auction-detail")]
        public async Task<IActionResult> AddAuctionDetail([FromBody] AddAuctionDetailModel model)
        {
            try
            {
                var result = await _auctionService.AddAuctionDetail(model);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
        [Authorize]
        [HttpPut("end-auction")]
        public async Task<IActionResult> EndAuction(int id)
        {
            try
            {
                var result = await _auctionService.EndAuction(id);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
    }
}