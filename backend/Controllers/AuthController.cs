using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Services;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly DataContext _context;

        public AuthController(IJwtTokenService jwtTokenService, DataContext context)
        {
            _jwtTokenService = jwtTokenService;
            _context = context;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            string hashedPassword = HashPassword(request.Password);
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);

            if (user == null || user.PasswordHash != hashedPassword)
            {
                return Unauthorized("Invalid username or password.");
            }

            // ✅ Ensure `user` is not null before generating a token
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var token = _jwtTokenService.GenerateToken(user);
            return Ok(new { token });
        }



        // ✅ Hashing function (Ensure it's in `AuthController.cs`)
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }


        public record LoginRequest(string Username, string Password);
    }
}
