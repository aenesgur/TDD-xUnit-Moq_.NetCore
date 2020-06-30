using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StationaryStore.DAL.Abstractions
{
    public interface IDbService<T> where T:class
    {
        Task<bool> CreateAsync(T model);
        Task<bool> UpdateAsync(string id, T model);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task<bool> DeleteAsync(string id);
    }
}
