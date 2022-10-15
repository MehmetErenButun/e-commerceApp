using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Spesifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpesificationEvaulator<Tentity> where Tentity : BaseEntity
    {
        public static IQueryable<Tentity> GetQuery(IQueryable<Tentity> inputQuery,ISpesification<Tentity> spec)
        {
            var query = inputQuery;

            if(spec.Criteria!=null)
            {
                query = query.Where(spec.Criteria);
            }

            if(spec.OrderBy!=null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if(spec.OrderByDescending!=null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if(spec.IspagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            query = spec.Includes.Aggregate(query,(current,include)=>current.Include(include));

            return query;
        }
    }
}