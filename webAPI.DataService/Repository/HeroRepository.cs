using Microsoft.Extensions.Logging;
using webAPI.DataService.Data;
using webAPI.DataService.IRepository;
using webAPI.Entites.Dbset;
using System;
namespace webAPI.DataService.Repository
{
    public class HeroRepository : GenericRepository<SuperHero>, IHeroRepository
    {
        public HeroRepository
        (
            DataContext context,
            ILogger logger
        ) : base(context, logger)
        {

        }

        // public override async Task<IEnumerable<SuperHero>> All()
        // {
        //     try
        //     {
        //         return await dbSet.Where()
        //     }
        //     catch (Exception ex)
        //     {
        //          // TODO
        //     }
        // }
    }
}