using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Spesifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ShopContext _context;
        public GenericRepository(ShopContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

       

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

         public async Task<T> GetEntityWithSpec(ISpesification<T> spec)
        {
            return await ApplySpesification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpesification<T> spec)
        {
            return await ApplySpesification(spec).ToListAsync();
        }

        private IQueryable<T> ApplySpesification(ISpesification<T> spec)
        {
            return SpesificationEvaulator<T>.GetQuery(_context.Set<T>().AsQueryable(),spec);
        }

        public async Task<int> CountAsync(ISpesification<T> spec)
        {
            return await ApplySpesification(spec).CountAsync();
        }

        public void Add(T entity)
        {
           _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}