using LibraryManagementSystem.Application.Common;
using LibraryManagementSystem.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Infrastructure.Helper
{
    public class AuthHelper
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        public AuthHelper(IConfiguration configuration, UserManager<User> userManager)
        {
              _configuration = configuration;
             _userManager = userManager;
        }
        public string GenerateToken(User user)
        {
            var secret = _configuration["JwtSettings:Secret"]!;
            var issuer = _configuration["JwtSettings:Issuer"]!;
            var audience = _configuration["JwtSettings:Audience"]!;
            var expiry = Convert.ToDouble(_configuration["JwtSettings:ExpiryInMinutes"]);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(JwtRegisteredClaimNames.UniqueName, user.UserName!),

               
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Role, user.Roles.ToString()),

                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiry),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string PasswordGenerator(string name)
        {
            var random = new Random();

            var number = random.Next(1000, 9999);
            var firstLetter = char.ToUpper(name[0]);

            return $"{firstLetter}{name.Substring(1)}@{number}";
        }
    }
}
