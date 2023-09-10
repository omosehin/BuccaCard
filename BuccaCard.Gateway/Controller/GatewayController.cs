using Buccacard.GateWayAPI;
using Buccacard.Infrastructure.Constant;
using Buccacard.Infrastructure.DTO.Product;
using Buccacard.Infrastructure.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuccaCard.Gateway.Controller
{

    public class GatewayController : BaseController
    {
        private readonly IBaseHttpClient _baseHttpClient;
        private readonly IConfiguration _configuration;

        public GatewayController(IBaseHttpClient baseHttpClient, IConfiguration configuration)
        {
            _baseHttpClient = baseHttpClient;
            _configuration = configuration;
        }

        [Route("login"),AllowAnonymous]
        public async Task<ActionResult> LoginAsyc(LoginDTO login) =>
                Ok(await _baseHttpClient.JSendPostAsync<Token>(_configuration.GetValue<string>(Urls.AuthBase), Urls.LoginUser, login));

        [Route("register-user"), AllowAnonymous]
        public async Task<ActionResult> RegisterUserAsync(RegisterDTO model) => 
                Ok(await _baseHttpClient.JSendPostAsync<string>(_configuration.GetValue<string>(Urls.AuthBase), Urls.RegisterUser, model));

        [Route("register-admin"),Authorize(Roles ="Admin")]
        public async Task<ActionResult> RegisterAdminAsync(RegisterDTO model) => 
                Ok(await _baseHttpClient.JSendPostAsync<string>(_configuration.GetValue<string>(Urls.AuthBase), Urls.RegisterAdmin, model, Token()));

        [Route("comfirm-account"), AllowAnonymous]
        public async Task<ActionResult> ComfirmAccountAsync(ComfirmDTO model) =>
                Ok(await _baseHttpClient.JSendPostAsync<string>(_configuration.GetValue<string>(Urls.AuthBase), Urls.ComfirmUser, model));

        [Route("create-card"),Authorize(policy: "CanCreateCard")]
        public async Task<ActionResult> CreateCardAsync(CreateCard model) => 
                Ok(await _baseHttpClient.JSendPostAsync<string>(_configuration.GetValue<string>(Urls.ProductBase), Urls.CreateCard, model,Token()));
    }
}
