using Buccacard.Domain.UserManagement;
using Buccacard.Infrastructure.DTO.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Buccacard.Services.UserManagementService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(AppUser applicationUser, IList<string> roles);
    }
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        public string GenerateToken(AppUser applicationUser, IList<string> roles)
        {

            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

         
            var claims = new[]
                {
                    new Claim(ClaimTypes.Name, applicationUser.UserName),
                    new Claim(ClaimTypes.Email,applicationUser.Email),
                    new Claim(ClaimTypes.Role, "Admin"), 
                    new Claim("Role", "Admin"),
                    new Claim("userId", applicationUser.Id),
                   
                };
            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials : new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                );
           
           // var token = tokenHandler.CreateToken(tokenDescriptor);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}
