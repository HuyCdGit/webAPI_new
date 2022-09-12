using Microsoft.Extensions.Logging;
using webAPI.DataService.Data;
using webAPI.DataService.IRepository;
using webAPI.Entites.Dbset;
using System;
using Microsoft.EntityFrameworkCore;

namespace webAPI.DataService.Repository
{
    public class RefreshRepository : GenericRepository<RefreshToken>, IRefeshTokenRepository
    {
        public RefreshRepository
        (
            DataContext context,
            ILogger logger
        ) : base(context, logger)
        {

        }
        public async Task<RefreshToken> GetByRefreshToken(string refreshToken)
        {
            try
            {
                return await dbSet.Where(x => x.Token.ToLower() == refreshToken.ToLower()).AsNoTracking().FirstOrDefaultAsync();
                
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, "{Repo} GetByRefreshToken method has generated an error" , typeof(RefreshToken));
                return null;
            }
        }

        public async Task<bool> MarkRefreshTokenAsUsed(RefreshToken refreshToken)
        {
            try
            {
                var token = await dbSet.Where(x => x.Token.ToLower() == refreshToken.Token.ToLower()).AsNoTracking().FirstOrDefaultAsync();
                if(token == null) return false;
                token.IsUsed = refreshToken.IsUsed;
                return true;
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, "{Repo} GetByRefreshToken method has generated an error" , typeof(RefreshToken));
                return false;
            }    
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