using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Services;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]  // ✅ This sets the route to /api/auth
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "testuser" && request.Password == "password123")
            {
                var token = _jwtTokenService.GenerateToken(request.Username);
                return Ok(new { token = token }); // ✅ Changed "Token" to "token"
            }

            return Unauthorized("Invalid username or password.");
        }
    }

    public record LoginRequest(string Username, string Password);
}
