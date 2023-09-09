using Buccacard.Infrastructure.DTO.Product;
using Buccacard.Services;
using Buccacard.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Buccacard.ProductAPI.Controller
{
    
    public class CardsController : BaseController
    {
        private readonly ICardService _card; 
        public CardsController(ICardService card,JwtService jwtService):base(jwtService)
        {
            _card = card;
        }


        [HttpPost("CreateCard"),Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(CreateCard card) => Ok(await _card.CreateCardASync(card, LoginUser()));
    }
}
