using webAPI.DataService.IRepository;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using webAPI.DataService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace webAPI.DataService.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DataContext _context;
        internal DbSet<T> dbSet;
        protected readonly ILogger _logger;
        public GenericRepository
        (
            DataContext context,
            ILogger logger
        )
        {
            _context = context;
            _logger = logger;
            dbSet = context.Set<T>();
        }
        public virtual async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            return await dbSet.ToListAsync();
        }

        public Task<bool> Delete(Guid id, string name)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> GetById(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public Task<bool> Upsert(T entity)
        {
            throw new NotImplementedException();
        }
    }
}