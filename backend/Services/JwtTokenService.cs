namespace Backend.Services  // ✅ Ensure this matches across all files
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;

    public interface IJwtTokenService
    {
        string GenerateToken(string username);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private const string Key = "YourSuperSecureLongKey12345678901234"; // ✅ Must be 32+ characters
        private const string Issuer = "your-app";
        private const string Audience = "your-audience";

        public string GenerateToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("username", username)
            };

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials,
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
