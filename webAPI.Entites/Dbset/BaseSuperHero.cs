using System;
namespace webAPI.Entites.Dbset
{
    public abstract class BaseSuperHero
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}