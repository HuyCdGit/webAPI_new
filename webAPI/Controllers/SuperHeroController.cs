using System.Reflection.Emit;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SupperHeroCotroller : ControllerBase
    {
        private static List<SuperHero> heroes = new List<SuperHero>
            {
                new SuperHero {
                    Id = 1,
                    Name = "spider man",
                    FirstName = "Peter",
                    LastName = "Parker",
                    Places = "New york city"
                },
                new SuperHero{
                    Id  = 2,
                    Name = "Ironman",
                    FirstName = "Tony",
                    LastName = "Stark",
                    Places = "Long Island"
                }
            };
        private readonly Datacontext _context;
        public SupperHeroCotroller(Datacontext context)
        {
            _context = context;
        }

        [HttpGet]   
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            return Ok(await _context.SuperHeroes.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> Get(int id)
        {
            var hero = await _context.SuperHeroes.FindAsync(id);
            if(hero == null)
            {
                return BadRequest("cant found hero");
            }
            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            _context.SuperHeroes.Add(hero);
            await _context.SaveChangesAsync();
            return Ok(await _context.SuperHeroes.ToListAsync());
        }
        
        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero request)
        {
            var hero = heroes.Find(h => h.Id == request.Id);
            if(hero == null)
            {
                return BadRequest("cant update hero");
            }

            hero.Id = request.Id;
            hero.Name = request.Name;
            hero.FirstName = request.FirstName;
            hero.LastName = request.LastName;
            hero.Places = request.Places;

            await _context.SaveChangesAsync();
            return Ok(await _context.SuperHeroes.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteHero(int id)
        {
            var hero = await _context.SuperHeroes.FindAsync(id);
            if(hero == null)
            {
                return BadRequest("cant found hero");
            }

            _context.SuperHeroes.Remove(hero);
            await _context.SaveChangesAsync();
            heroes.Remove(hero);
            return Ok(await _context.SuperHeroes.ToListAsync());
        }
    }
}