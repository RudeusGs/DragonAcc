using DragonAcc.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DragonAcc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("get-by-user")]
        public async Task<IActionResult> GetAllByUserId([FromQuery] int userId)
        {
            try
            {
                var result = await _notificationService.GetAllByUserId(userId);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }

        [Authorize]
        [HttpPut("read")]
        public async Task<IActionResult> Read([FromQuery] int id)
        {
            try
            {
                var result = await _notificationService.Read(id);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
    }
}
