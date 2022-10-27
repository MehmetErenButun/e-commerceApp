using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private  Hashtable _repositories;
        private readonly ShopContext _shopContext;
        public UnitOfWork(ShopContext shopContext)
        {
            _shopContext = shopContext;
           
        }

        public async Task<int> Complete()
        {
            return await _shopContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _shopContext.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_repositories==null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if(!_repositories.Contains(type)){
                var repoType =typeof(GenericRepository<>);
                var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(TEntity)),_shopContext);

                _repositories.Add(type,repoInstance);
            }

            return (IGenericRepository<TEntity>) _repositories[type];

        }
    }
}