using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Spesifications;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> GetEntityWithSpec(ISpesification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpesification<T> spec);
        Task<int> CountAsync(ISpesification<T> spec);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        
    }
}