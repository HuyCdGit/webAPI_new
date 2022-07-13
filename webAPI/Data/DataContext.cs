
using Microsoft.EntityFrameworkCore;

namespace webAPI
{
    public class Datacontext : DbContext
    {
        public Datacontext(DbContextOptions<Datacontext> options) : base(options) { }
        public DbSet<SuperHero> SuperHeroes { get; set; }
        public DbSet<User> Users {get; set;}
        public DbSet<UserDto> UserDtos {get; set;}
        public DbSet<RefreshToken> RefreshTokens {get; set;}
        
    }
}