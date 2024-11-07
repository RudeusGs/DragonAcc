using DragonAcc.Service.Interfaces;
using DragonAcc.Infrastructure.Entities; // Ensure this namespace is correct
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DragonAcc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // GET api/message/get-conversation?user1=user1&user2=user2
        [HttpGet("get-conversation")]
        public async Task<IActionResult> GetConversation([FromQuery] string user1, [FromQuery] string user2)
        {
            try
            {
                var result = await _messageService.GetConversationAsync(user1, user2);
                return Ok(result); // Return 200 OK with the messages
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message); // Return 500 Internal Server Error
            }
        }

        // GET api/message/get-by-id?id=1
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            try
            {
                var message = await _messageService.GetMessageByIdAsync(id);
                if (message == null)
                {
                    return NotFound(); // Return 404 Not Found if message doesn't exist
                }
                return Ok(message); // Return 200 OK with the message
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message); // Return 500 Internal Server Error
            }
        }

        // POST api/message/send
        [Authorize] // Requires authorization to send a message
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            try
            {
                if (message == null)
                {
                    return BadRequest("Message cannot be null."); // Return 400 Bad Request if message is null
                }

                var result = await _messageService.SendMessageAsync(message);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result); // Return 201 Created with the new message
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteMessage([FromQuery] int id)
        {
            try
            {
                var result = await _messageService.DeleteMessageAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent(); 
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
