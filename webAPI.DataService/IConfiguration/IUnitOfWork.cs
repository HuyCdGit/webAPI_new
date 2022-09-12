using webAPI.DataService.IRepository;
using System.Threading.Tasks;
namespace webAPI.DataService.IConfiguration
{
    public interface IUnitOfWork
    {
        IHeroRepository Heroes {get;}
        IRefeshTokenRepository RefreshToken {get;}

        Task CompleteAsnyc();
    }
}