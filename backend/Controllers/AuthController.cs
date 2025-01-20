using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Services;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;

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
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            Console.WriteLine($"Login Attempt: {request.Username}");

            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);

            if (user == null)
            {
                Console.WriteLine("❌ User Not Found!");
                return Unauthorized("Invalid username or password.");
            }

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                Console.WriteLine("❌ User has NULL password hash!");
                return Unauthorized("Invalid username or password.");
            }

            string hashedPassword = HashPassword(request.Password);
            Console.WriteLine($"🔑 Entered Password Hash: {hashedPassword}");
            Console.WriteLine($"🔐 Stored Password Hash: {user.PasswordHash}");

            if (user.PasswordHash != hashedPassword)
            {
                Console.WriteLine("❌ Password Mismatch!");
                return Unauthorized("Invalid username or password.");
            }

            var token = _jwtTokenService.GenerateToken(user);

            return Ok(new { token = token, userId = user.Id });
        }


        // ✅ Hashing function (Ensure it's in `AuthController.cs`)
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            if (_context.Users.Any(u => u.Username == request.Username))
            {
                return BadRequest(new { message = "Username already exists." });
            }

            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = HashPassword(request.Password), // ✅ Hash password before storing
                Name = request.Name
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(new { message = "User registered successfully!" });
        }



        private static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            using var sha256 = SHA256.Create();

            byte[] hashWithSalt = Convert.FromBase64String(storedHash);

            // ✅ Extract the salt (first 16 bytes)
            byte[] salt = new byte[16];
            Buffer.BlockCopy(hashWithSalt, 0, salt, 0, salt.Length);

            // ✅ Hash the entered password with the extracted salt
            byte[] passwordBytes = Encoding.UTF8.GetBytes(enteredPassword);
            byte[] combinedBytes = new byte[salt.Length + passwordBytes.Length];
            Buffer.BlockCopy(salt, 0, combinedBytes, 0, salt.Length);
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, salt.Length, passwordBytes.Length);

            byte[] enteredHash = sha256.ComputeHash(combinedBytes);

            // ✅ Extract stored hash part (ignore salt)
            byte[] storedHashBytes = new byte[hashWithSalt.Length - salt.Length];
            Buffer.BlockCopy(hashWithSalt, salt.Length, storedHashBytes, 0, storedHashBytes.Length);

            return enteredHash.SequenceEqual(storedHashBytes); // ✅ Compare hashes
        }


        public record LoginRequest(string Username, string Password);
    }
}
