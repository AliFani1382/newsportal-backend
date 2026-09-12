using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewsPortal.Application.Interfaces;
using NewsPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Infrastructure.Services
{
    public sealed class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = jwtSettings["Key"]
                ?? throw new InvalidOperationException(
                    "JWT Kew is not configured.");

            var issuer = jwtSettings["Issuer"]
                  ?? throw new InvalidOperationException(
                    "JWT Issuer is not configured.");

            var audience = jwtSettings["Audience"]
                 ?? throw new InvalidOperationException(
                   "JWT Audience is not configured.");

            var expiryInDays = jwtSettings.GetValue<int?>("ExpiryInDays")
                ?? throw new InvalidOperationException(
                    "JWT ExpiryInDays is not configured.");

            var claims = new List<Claim>
            {
                new(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()),

                 new(
                    ClaimTypes.Name,
                    user.Username),

                  new(
                    ClaimTypes.Email,
                    user.Email),

                   new(
                    ClaimTypes.Role,
                    user.Role?.Name
                    ?? throw new InvalidOperationException(
                        "User role is not loaded.")),

                    new(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())

            };

            var sercurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(
                sercurityKey,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(expiryInDays),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);

        }
    }
}
