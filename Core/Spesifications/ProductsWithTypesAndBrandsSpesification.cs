using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Spesifications
{
    public class ProductsWithTypesAndBrandsSpesification : BaseSpesification<Product>
    {
        public ProductsWithTypesAndBrandsSpesification()
        {
            AddInclude(x=> x.ProductBrand);
            AddInclude(x=> x.ProductType);
        }

        public ProductsWithTypesAndBrandsSpesification(int id) : base(x=>x.Id==id)
        {
            AddInclude(x=> x.ProductBrand);
            AddInclude(x=> x.ProductType);
        }
    }
}