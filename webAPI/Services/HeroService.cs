// using System;
// using Microsoft.AspNetCore.Mvc;

// namespace webAPI.Services
// {
//     public class HeroService : IHeroService
//     {
        
//         private DataContext _context;
//         public DataContext Context {
//             get => _context;
//             set => _context = value;
//         }
//         public HeroService(DataContext context)
//         {
//             _context = context;
//         }
//         public SuperHero findHero(int id)
//         {
//             var hero =  _context.SuperHeroes.Find(id);
//             //var hero = lstHero.Find(h => h.Id == id);
//             return hero;
//         }           

//         public async Task<ActionResult<Boolean>> save()
//         {
//             await _context.SaveChangesAsync();
//             return true;
//         }

//         public async Task<ActionResult<List<SuperHero>>> showLst()
//         {
//             return await _context.SuperHeroes.ToListAsync();
//         }

//         public async Task<SuperHero> AddAsync(SuperHero data)
//         {
//             Console.WriteLine($"{data.Name}");

//             await _context.SuperHeroes.AddAsync(data);
//             return data;
//         }
        
//     }
// }