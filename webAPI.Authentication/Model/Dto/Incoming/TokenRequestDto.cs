using System.ComponentModel.DataAnnotations;
namespace webAPI.Authentication.Model.Dto.Incoming
{
    public class TokenRequestDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}