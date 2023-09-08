using Buccacard.Infrastructure.DTO;
using Buccacard.Services.NotificationService;
using Microsoft.AspNetCore.Mvc;

namespace Buccacard.NoficationAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public HomeController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("SendEmail")]
        public IActionResult SendMail(EmailDTO payload)
        {
            _emailService.Send(payload.Email, payload.Subject, payload.Message,payload.Email);
            return Ok();
        }
    }
}
