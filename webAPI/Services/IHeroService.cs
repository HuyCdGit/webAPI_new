using System.Security;
using Microsoft.AspNetCore.Mvc;

namespace webAPI.Services
{
    public interface IHeroService
    {
        Datacontext Context { get; set; }
        public SuperHero findHero(int id);
        public Task<ActionResult<List<SuperHero>>> showLst();
        public Task<ActionResult<Boolean>> save();
        public Task<SuperHero> AddAsync(SuperHero data);
    }
}