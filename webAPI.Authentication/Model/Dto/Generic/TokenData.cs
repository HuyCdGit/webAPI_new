using System.ComponentModel.DataAnnotations;
namespace webAPI.Authentication.Model.Dto.Generic
{
    public class TokenData
    {
        
        public string JwtToken { get; set; }

        public string RefreshToken { get; set; }
    }
}