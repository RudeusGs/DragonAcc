using DragonAcc.Service.Interfaces;
using DragonAcc.Service.Models.WithDrawMoeny;
using DragonAcc.Service.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DragonAcc.Service.Services;

namespace DragonAcc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WithDrawMoneyController : BaseController
    {
        private readonly IWithDrawMoneyService _withDrawMoneyService;

        public WithDrawMoneyController(IWithDrawMoneyService withDrawMoneyService)
        {
            _withDrawMoneyService = withDrawMoneyService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddWithDrawMoneyModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _withDrawMoneyService.Add(model);
            return Response(result);
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _withDrawMoneyService.GetAll();
            return Response(result);
        }
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var result = await _withDrawMoneyService.UpdateStatus(id);
            return Response(result);
        }
    }
}
