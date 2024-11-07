using DragonAcc.Service.Interfaces;
using DragonAcc.Service.Models.Lol_GameModel;
using DragonAcc.Service.Models.NgocRong_GameModel;
using DragonAcc.Service.Models.TocChien_GameModel;
using DragonAcc.Service.Models.Valorant_GameModel;
using DragonAcc.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DragonAcc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Valorant_GameController : BaseController
    {
        private readonly IValorant_GameService _valorant_GameService;
        public Valorant_GameController(IValorant_GameService valorant_GameService)
        {
            _valorant_GameService = valorant_GameService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _valorant_GameService.GetAll();
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            try
            {
                var result = await _valorant_GameService.GetById(id);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }

        [Authorize]
        [HttpPost("add-for-user")]
        public async Task<IActionResult> AddForUser([FromForm] AddValorant_GameModel model)
        {
            try
            {
                var result = await _valorant_GameService.AddForUser(model);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
        [Authorize]
        [HttpPost("update-for-user")]
        public async Task<IActionResult> UpdateForUser([FromForm] UpdateValorant_GameModel model)
        {
            try
            {
                var result = await _valorant_GameService.UpdateForUser(model);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
        [Authorize]
        [HttpPost("delete-for-user-and-admin")]
        public async Task<IActionResult> DeleteForUser([FromQuery] int id)
        {
            try
            {
                var result = await _valorant_GameService.DeleteForUserAndAdmin(id);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
        [Authorize]
        [HttpPost("update-for-admin")]
        public async Task<IActionResult> UpdateForAdmin([FromQuery] int id)
        {
            try
            {
                var result = await _valorant_GameService.UpdateForAdmin(id);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
        [HttpGet("get-full-name")]
        public async Task<IActionResult> GetFullName([FromQuery] int id)
        {
            try
            {
                var result = await _valorant_GameService.GetFullName(id);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
        [Authorize]
        [HttpPost("Purchase")]
        public async Task<IActionResult> BuyGameAccount([FromBody] BuyAccountValorant_GameModel model)
        {
            try
            {
                var result = await _valorant_GameService.BuyGameAccount(model);
                return Response(result);
            }
            catch (Exception e)
            {
                return Response(e.Message, 500);
            }
        }
        [HttpGet("get-by-user/{userId}")]
        public async Task<IActionResult> GetAllByUser(int userId)
        {
            var result = await _valorant_GameService.GetAllByUser(userId);
            return Ok(result);
        }
    }
}
