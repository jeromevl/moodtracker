using Microsoft.AspNetCore.Mvc;
using MoodTracker.Core.Interfaces.Services;
using MoodTracker.Core.Models._Dtos;

namespace MoodTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoodsController(IMoodService moodService) : ControllerBase
    {
        private readonly IMoodService _moodService = moodService;

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUserMoodsAsync()
        {
            IActionResult result;

            try
            {
                var results = await _moodService.GetUserMoodsAsync();
                result = Ok(results);
            }
            catch (Exception ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return result;
        }

        [HttpPost("users")]
        public async Task<IActionResult> SubmitAsync([FromBody] UserMoodDto userMood)
        {
            IActionResult result;

            try
            {
                await _moodService.SubmitMoodByUsernameAsync(userMood.Username, userMood.Mood.Id, userMood.Date, userMood.Remarks);
                result = Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)    
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return result;
        }
    }
}