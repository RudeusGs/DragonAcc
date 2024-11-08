using DragonAcc.Service.Interfaces;
using DragonAcc.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace DragonAcc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LuckyWheelListPrizeController : BaseController
    {
        private readonly ILuckyWheelListPrizeService _luckyWheelListPrizeService;
        public LuckyWheelListPrizeController(ILuckyWheelListPrizeService luckyWheelListPrizeService)
        {
            _luckyWheelListPrizeService = luckyWheelListPrizeService;
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _luckyWheelListPrizeService.GetAll();
            return Response(result);
        }

        [HttpGet("get-by-user")]
        public async Task<IActionResult> GetAllByUser(int userId)
        {
            var result = await _luckyWheelListPrizeService.GetAllByUser(userId);
            return Ok(result);
        }
    }
}