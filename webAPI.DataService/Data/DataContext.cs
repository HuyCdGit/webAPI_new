using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using webAPI.Entites.Dbset;

namespace webAPI.DataService.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<SuperHero> SuperHeroes { get; set; }
        public DbSet<Auth> Auths { get; set; }
        public DbSet<User> Users {get; set;}
        //public DbSet<UserDto> UserDtos {get; set;}
        public DbSet<RefreshToken> RefreshTokens {get; set;}        
    }
}