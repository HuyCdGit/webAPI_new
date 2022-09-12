using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace webAPI.Entites.Dbset
{
    public class RefreshToken : BaseSuperHero
    {
        public string UserId { get; set; } // User Id when logged in
        public string Token { get; set; } = string.Empty;
        public string JwtId { get; set; } // the id generated when a jwt id has been request
        public bool IsUsed { get; set; }// To make sure that the token is only used once
        public bool IsRevoked { get; set; } // make sure they are valid
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiryDate { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User {get; set;}
    }
}