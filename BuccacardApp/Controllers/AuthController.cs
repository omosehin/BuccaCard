using Buccacard.Infrastructure.DTO;
using Buccacard.Services.UserManagementService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Buccacard.UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Login"), AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO login)=>Ok(await _authService.Login(login));

        [HttpPost("Register_User"), AllowAnonymous]
        public async Task<IActionResult> RegisterUser(RegisterDTO register) => Ok(await _authService.Register(register));

        [HttpPost("Register_Admin"), Authorize]
        public async Task<IActionResult> RegisterAdmin(RegisterDTO register) => Ok(await _authService.Register_Admin(register));


        [HttpPost("Comfirm_Account"), AllowAnonymous]
        public async Task<IActionResult> ComfirmAccount(string userId,string token) => Ok(await _authService.ComfirmToken(userId,token));
    }
}
