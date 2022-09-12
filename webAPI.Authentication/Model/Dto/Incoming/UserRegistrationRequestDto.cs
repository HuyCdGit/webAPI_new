using System.ComponentModel.DataAnnotations;
namespace webAPI.Authentication.Model.Dto.Incoming
{
    public class UserRegistrationRequestDto
    {
        [Required]
        public string Name {get; set;}
        [Required]
        public string LastName{get; set;}
        [Required]
        public string FirstName{get; set;}
        [Required]
        public string Password {get; set;}
        [Required]
        public string Email { get; set; }
        
    }
}