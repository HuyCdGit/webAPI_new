using System;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace webAPI.DataService.IRepository
{
    public interface  IGenericRepository<T> where T : class
    {
        //Get all entity
        Task<IEnumerable<T>> All();
        Task<T> GetById(Guid id);
        Task<bool> Add (T entity);
        Task<bool> Delete(Guid id, string name);
        //Update and add if it does not exist
        Task<bool> Upsert(T entity);
    }
}