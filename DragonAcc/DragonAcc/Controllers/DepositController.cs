using DragonAcc.Service.Interfaces;
using DragonAcc.Service.Models.Deposit;
using DragonAcc.Service.Models.Lol_GameModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DragonAcc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositController : BaseController
    {
        private readonly IDepositService _depositService;
        public DepositController(IDepositService depositService)
        {
            _depositService = depositService;
        }

        [HttpGet("get-all-by-UserId")]
        public async Task<IActionResult> GetAllByUserId(int id)
        {
            try
            {
                var result = await _depositService.GetAllByUserId(id);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _depositService.GetAll();
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
        [Authorize]
        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatus([FromQuery] UpdateStatus_Model model)
        {
            try
            {
                var result = await _depositService.UpdateStatus(model);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromForm] AddDepositModel model)
        {
            try
            {
                var result = await _depositService.Add(model);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
    }
}
