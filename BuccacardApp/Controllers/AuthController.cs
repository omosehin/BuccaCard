﻿using Buccacard.Infrastructure.DTO;
using Buccacard.Services.UserManagementService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Buccacard.UserManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO login)=>Ok(await _authService.Login(login));

        [HttpPost("Register-User"), AllowAnonymous]
        public async Task<IActionResult> RegisterUser(RegisterDTO register) => Ok(await _authService.Register(register));

        [HttpPost("Register-Admin"), Authorize(Roles ="User")]
        public async Task<IActionResult> RegisterAdmin(RegisterDTO register) => Ok(await _authService.Register_Admin(register));

        [HttpPost("Comfirm-User"), AllowAnonymous]
        public async Task<IActionResult> ComfirmAccount(ComfirmDTO token) => Ok(await _authService.ComfirmToken(token.UserId,token.Token));
    }
}
