using Buccacard.Domain.ProductManagement;
using Buccacard.Infrastructure.DTO;
using Buccacard.Infrastructure.DTO.Product;
using Buccacard.Infrastructure.Utility;
using System;
using System.Threading.Tasks;

namespace Buccacard.Services.ProductService
{
    public interface ICardService
    {
        Task<ServiceResponse<string>> CreateCardASync(CreateCard card, string loginUser);
    }

    public class CardService : ICardService
    {
        private readonly ProductDbContext _db;
        private readonly IResponseService _responseService;

        public CardService(ProductDbContext db, IResponseService responseService)
        {
            _db = db;
            _responseService = responseService;
        }
        public async Task<ServiceResponse<string>> CreateCardASync(CreateCard card, string loginUser)
        {
            var organisation = await _db.Organisations.FindAsync(card.OrganisationId);
            var newCard = new Card
            {
                AppUserId = card.AppUserId,
                CardType = card.CardType,
                Organisation = organisation,
                Created = DateTime.Now,
                CreatedBy = loginUser,
                CreditLimit = card.CreditLimit
            };
            await _db.Cards.AddAsync(newCard);

            var saved = await _db.SaveChangesAsync() > 0;
            return saved ? _responseService.SuccessResponse("Successfully Created.") : _responseService.ErrorResponse<string>("Not successful,please try again.");
        }
    }
}
