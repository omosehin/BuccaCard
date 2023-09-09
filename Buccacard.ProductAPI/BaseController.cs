namespace Buccacard.ProductAPI
{
    using Buccacard.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;

    [Route("[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly JwtService JwtService;

        public BaseController(JwtService jwtService)
        {
            JwtService = jwtService;
        }

        //protected ClaimsPrincipal GetUserClaims()
        //{
        //    var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        //    if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
        //    {
        //        var token = authorizationHeader[7..]; // Remove "Bearer " prefix
        //        return JwtService.ValidateToken(token);
        //    }

        //    return null;
        //}
        protected string LoginUser()
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var id = string.Empty;
            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                var handler = new JwtSecurityTokenHandler();
                string headerData = Request.Headers["Authorization"].ToString().Replace("Bearer", string.Empty).Trim();
                var tokenValue = handler.ReadToken(headerData) as JwtSecurityToken; 

                    var userIdClaim = tokenValue.Claims.First(c => c.Type == "sub").Value;
                    return id;

                //var handler = new JwtSecurityTokenHandler();
                //string headerData = Request.Headers["Authorization"].ToString().Replace("Bearer", string.Empty).Trim();
                //var token = handler.ReadToken(headerData) as JwtSecurityToken;
                //return token.Claims.First(claim => claim.Type == "sub").Value.FromJson<TokenDataDTO>();

            }
            else return id;
        }
    }

}
