using DragonAcc.Service.Interfaces;
using DragonAcc.Service.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DragonAcc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticalController : BaseController
    {
        private readonly IStatisticalService _statisticalService;

        public StatisticalController(IStatisticalService statisticalService)
        {
            _statisticalService = statisticalService;
        }

        [HttpGet("get-by-user")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            var result = await _statisticalService.GetByUserId(id);
            return Response(result);
        }
    }
}
