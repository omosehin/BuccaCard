namespace Buccacard.GateWayAPI
{
    using Buccacard.Infrastructure.Utility;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;

    [Route("[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(err => err.ErrorMessage));
                context.Result = Error(message);
            }
            base.OnActionExecuting(context);
        }
        protected IActionResult Error(string message)
       => Ok(new ResponseService().ErrorResponse<string>(message));

        protected string Token()
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var id = string.Empty;
            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                return Request.Headers["Authorization"].ToString().Replace("Bearer", string.Empty).Trim();
            }
            else return default;
        }
        protected string LoginUser()
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenBearer = Token(); 
                var tokenValue = handler.ReadToken(tokenBearer) as JwtSecurityToken; 

                var userIdClaim = tokenValue.Claims.First(c => c.Type == "userId").Value;
                return userIdClaim;
        }
    }

}
