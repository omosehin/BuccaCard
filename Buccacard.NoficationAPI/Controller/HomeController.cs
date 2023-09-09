using Buccacard.Infrastructure.DTO.User;
using Buccacard.Infrastructure.Utility;
using Buccacard.Services.NotificationService;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace Buccacard.NoficationAPI.Controller
{
    [ApiController]
    [Route("[controller]")]
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
            return Ok(new ResponseService().SuccessResponse("Comfirmation Notification", "Mail Sent!"));
        }
    }
}
