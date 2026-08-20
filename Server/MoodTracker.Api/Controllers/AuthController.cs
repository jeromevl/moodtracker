using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MoodTracker.Core.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoodTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IUserService userService,
        IConfiguration configuration) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IConfiguration _configuration = configuration;

        [HttpPost("login")]
        public async Task<IActionResult> LogInAsync([FromBody] LoginInfo loginInfo)
        {
            IActionResult result;

            var user = await _userService.GetUserByAccountAsync(loginInfo.UserName, loginInfo.Password);
            if (user != null)
            {
                // Generate JWT token
                var claims = new[]
                {
                    new Claim("userid", user.Id!.ToString()),
                    new Claim("role", user.Role)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Jwt:SigningKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Tokens:Jwt:Issuer"],
                    audience: _configuration["Tokens:Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(24),
                    signingCredentials: creds);

                result = Ok(new
                {
                    access_token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = (DateTime?)null
                });
            }
            else
                result = Unauthorized("Invalid client credentials.");

            return result;
        }
        //
        // Helper classes
        //
        public class LoginInfo
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}