using System.Runtime.CompilerServices;
namespace webAPI
{
    public class Auth
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public List<User> UserId { get; set; }
    }
}