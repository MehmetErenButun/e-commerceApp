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
        public ProductsWithTypesAndBrandsSpesification(ProductSpecParams productParams)
        :base(x=>
        (string.IsNullOrEmpty(productParams.Search)  || (x.Name.ToLower().Contains(productParams.Search))) &&
        (!productParams.brandId.HasValue || x.ProductBrandId == productParams.brandId) && 
        (!productParams.typeId.HasValue || x.ProductTypeId==productParams.typeId))
        {
            AddInclude(x=> x.ProductBrand);
            AddInclude(x=> x.ProductType);
            AddOrderBy(x=> x.Name);

            if(!string.IsNullOrEmpty(productParams.sort))
            {
                switch (productParams.sort)
                {
                    case "priceAsc" :
                    AddOrderBy(p=>p.Price);
                    break;
                    case "priceDesc" :
                    AddOrderByDescending(p=>p.Price);
                    break;
                    default : 
                    AddOrderBy(n=>n.Name);
                    break;
                }
            }
        }

        public ProductsWithTypesAndBrandsSpesification(int id) : base(x=>x.Id==id)
        {
            AddInclude(x=> x.ProductBrand);
            AddInclude(x=> x.ProductType);
        }
    }
}