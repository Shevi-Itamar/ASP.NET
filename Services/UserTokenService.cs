using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace lessson1.Services
{
    public static class UserTokenService
    {
        public static string GenerateToken(int userId, string name, string role, IConfiguration config)
        {
            var jwtKey = config["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is missing in configuration");
            var issuer = config["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer is missing in configuration");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, name),
                new Claim("Role", role)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                expires: DateTime.UtcNow.AddMonths(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration config)
        {
            var jwtKey = config["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is missing in configuration");
            var issuer = config["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer is missing in configuration");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = issuer,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}
