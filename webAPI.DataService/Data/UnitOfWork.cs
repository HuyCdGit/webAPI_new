using Microsoft.Extensions.Logging;
using webAPI.DataService.IConfiguration;
using webAPI.DataService.IRepository;
using webAPI.DataService.Repository;
using System.Threading.Tasks;
namespace webAPI.DataService.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;
        public IHeroRepository Heroes{get; private set;}
        public IRefeshTokenRepository RefreshToken{get; private set;}
        public UnitOfWork(DataContext context, ILoggerFactory loggerFactory)
        {
             _context  = context;
            _logger = loggerFactory.CreateLogger("db_logs");
            Heroes = new HeroRepository(context, _logger);
            RefreshToken = new RefreshRepository(context, _logger);
        }

        public async Task CompleteAsnyc()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}