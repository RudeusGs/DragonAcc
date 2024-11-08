using DragonAcc.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DragonAcc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasedAccountController : BaseController
    {
        private readonly IPurchasedAccountService _purchasedAccountService;
        public PurchasedAccountController(IPurchasedAccountService purchasedAccountService)
        {
            _purchasedAccountService = purchasedAccountService;
        }
        [HttpGet("get-by-user")]
        public async Task<IActionResult> GetAllByUser(int userId)
        {
            var result = await _purchasedAccountService.GetAllByUser(userId);
            return Ok(result);
        }

    }
}