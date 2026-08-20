using Microsoft.AspNetCore.Mvc;
using MoodTracker.Core.Interfaces.Services;

namespace MoodTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet("{username}")]
        public async Task<IActionResult> GetByUsernameAsync(string username)
        {
            IActionResult result;

            try
            {
                var user = await _userService.GetUserByUsernameAsync(username);
                if (user != null)
                    result = Ok(user);
                else
                    result = NoContent();
            }
            catch (Exception ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return result;
        }
    }
}