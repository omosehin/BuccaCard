using Buccacard.Domain;
using Buccacard.Infrastructure.DTO;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            //var claimList = new List<Claim>
            //{
            //    new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
            //     new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id),
            //     new Claim(JwtRegisteredClaimNames.Name, applicationUser.UserName.ToString()),
            //     new Claim("appRole", "user")
            //};
         var claimList =   new ClaimsIdentity(new Claim[] {
                     new Claim("Roles", "User"),
                    });
            //foreach (var userRole in roles)
            //{
            //    claimList.Add(new Claim(ClaimTypes.Role, userRole));
            //}
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
