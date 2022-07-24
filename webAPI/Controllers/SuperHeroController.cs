// using Internal;
using System.Reflection.Emit;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using webAPI.Services;
using AutoMapper;
using System;

namespace webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SupperHeroController : ControllerBase
    {
        // private static List<SuperHero> heroes = new List<SuperHero>
        //     {
        //         // new SuperHero {
        //         //     Id = 1,
        //         //     Name = "spider man",
        //         //     FirstName = "Peter",
        //         //     LastName = "Parker",
        //         //     Places = "New york city"
        //         // },
        //         // new SuperHero{
        //         //     Id  = 2,
        //         //     Name = "Ironman",
        //         //     FirstName = "Tony",
        //         //     LastName = "Stark",
        //         //     Places = "Long Island"
        //         // }
        //     };
        private readonly HeroService _heroService;
        public readonly IMapper _mapper; 
        public SupperHeroController(
            HeroService heroServices,
            IMapper mapper
        )
        {
            _mapper = mapper;
            _heroService = heroServices;
        }
        [HttpGet]   
        public async Task<ActionResult<List<SuperHero>>> Get()
        {
            return Ok(await _heroService.showLst());
        }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<SuperHero>> Get(int id)
        // {
        //     var hero = await _context.SuperHeroes.FindAsync(id);
        //     if(hero == null)
        //     {
        //         return BadRequest("cant found hero");
        //     }
        //     return Ok(hero);
        // }

        [HttpPost]
        public async Task<ActionResult<SuperHero>> AddHero([FromBody]SuperHeroDto hero)
        {
            var _MapperHero = _mapper.Map<SuperHero>(hero);
            var props = _MapperHero.GetType().GetProperties();
            foreach (var p in props)
            {
                Console.WriteLine(p.Name + ": " + p.GetValue(_MapperHero, null));
            }
            
            
            await _heroService.AddAsync(_MapperHero);
            await _heroService.save();
            return Ok(await _heroService.showLst());
        }
        
        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero request)
        {
            var hero = _heroService.findHero(request.Id);

            if(hero == null)
            {
                return BadRequest("cant update hero");
            }

            //hero.Id = request.Id;
            hero.Name = request.Name;
            hero.FirstName = request.FirstName;
            hero.LastName = request.LastName;
            hero.Places = request.Places;

            // await _context.SaveChangesAsync();
            _heroService.save();
            return Ok(hero);
            // await _context.SuperHeroes.ToListAsync()
        }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult<List<SuperHero>>> DeleteHero(int id)
        // {
        //     var hero = await _context.SuperHeroes.FindAsync(id);
        //     if(hero == null)
        //     {
        //         return BadRequest("cant found hero");
        //     }

        //     _context.SuperHeroes.Remove(hero);
        //     await _context.SaveChangesAsync();
        //     heroes.Remove(hero);
        //     return Ok(await _context.SuperHeroes.ToListAsync());
        // }
    }
}