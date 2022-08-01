// using Internal;
using System.Reflection.Emit;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System;
using webAPI.DataService.IConfiguration;
using webAPI.Entites.Dbset;

namespace webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SupperHeroController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        //private readonly HeroService _heroService;

        public readonly IMapper _mapper; 
        public SupperHeroController(
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetHero()
        {
            var users = await _unitOfWork.Heroes.All();
            return Ok(users);
            //return Ok(await _heroService.showLst());
        }

        [HttpGet]
        [Route("GetHero", Name = "GetHero")]
        public async Task<IActionResult> GetHeroById(Guid id)
        {
            var hero = await _unitOfWork.Heroes.GetById(id);
            return Ok(hero);
            // var hero = await _context.SuperHeroes.FindAsync(id);
            // if(hero == null)
            // {
            //     return BadRequest("cant found hero");
            // }
            // return Ok(hero);
        }

        [HttpPost]
        public async Task<IActionResult> AddHero(SuperHeroDto hero)
        {
            var _hero = new SuperHero();
            _hero.Name = hero.Name;
            _hero.FirstName = hero.FirstName;
            _hero.LastName = hero.LastName;
            _hero.Places = hero.Places;

            await _unitOfWork.Heroes.Add(_hero);
            await _unitOfWork.CompleteAsnyc();
            return CreatedAtRoute("GetHero", new {id = _hero.Id}, hero); // return a 201
            // var _MapperHero = _mapper.Map<SuperHero>(hero);
            // var props = _MapperHero.GetType().GetProperties();
            // foreach (var p in props)
            // {
            //     Console.WriteLine(p.Name + ": " + p.GetValue(_MapperHero, null));
            // }
            
            
            // await _heroService.AddAsync(_MapperHero);
            // await _heroService.save();
            // return Ok(await _heroService.showLst());
        }
        
        // [HttpPut]
        // public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero request)
        // {
        //     var hero = _heroService.findHero(request.Id);

        //     if(hero == null)
        //     {
        //         return BadRequest("cant update hero");
        //     }

        //     //hero.Id = request.Id;
        //     hero.Name = request.Name;
        //     hero.FirstName = request.FirstName;
        //     hero.LastName = request.LastName;
        //     hero.Places = request.Places;

        //     // await _context.SaveChangesAsync();
        //     _heroService.save();
        //     return Ok(hero);
        //     // await _context.SuperHeroes.ToListAsync()
        // }

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