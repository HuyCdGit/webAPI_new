using System.Runtime.CompilerServices;
namespace webAPI.Entites.Dbset
{
    public class Auth : BaseSuperHero
    {
        public string UserName { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }    
        public byte[] PasswordSalt { get; set; }
        public List<User> UserId { get; set; }
    }
}