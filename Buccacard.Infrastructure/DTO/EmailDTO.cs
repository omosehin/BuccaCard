using System.ComponentModel.DataAnnotations;

namespace Buccacard.Infrastructure.DTO
{
  
   public class EmailDTO
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
    }
}
