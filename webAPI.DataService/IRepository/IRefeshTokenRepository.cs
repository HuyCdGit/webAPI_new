using webAPI.Entites.Dbset;
namespace webAPI.DataService.IRepository
{
    public interface IRefeshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken> GetByRefreshToken(string refreshToken);
        Task<bool> MarkRefreshTokenAsUsed(RefreshToken refreshToken);
    }
}