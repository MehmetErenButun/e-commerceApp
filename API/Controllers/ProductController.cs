using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Spesifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
   
    public class ProductController : BaseApiController
    {
        
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IMapper _mapper;
        
        public ProductController(IGenericRepository<Product> productRepo, IGenericRepository<ProductBrand> productBrandRepo,IGenericRepository<ProductType> productTypeRepo,IMapper mapper)
        {
            _mapper = mapper;
            _productBrandRepo = productBrandRepo;
            _productRepo = productRepo;
            _productTypeRepo = productTypeRepo;
          
           
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams productParams){

            var spec = new ProductsWithTypesAndBrandsSpesification(productParams);

            var products = await _productRepo.ListAsync(spec);

            var countSpec = new ProductsWithFilterForCountSpesification(productParams);

            var totalItems = await _productRepo.CountAsync(countSpec);
        
            var data = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,productParams.PageSize,totalItems,data));
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProducts(int id){

             var spec = new ProductsWithTypesAndBrandsSpesification(id);
             var product = await _productRepo.GetEntityWithSpec(spec);

             if(product==null) return NotFound(new ApiResponse(404));
            
            return _mapper.Map<Product,ProductToReturnDto>(product);
        }

        [HttpGet("brands")]

        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        
        [HttpGet("types")]

        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }
        
    }
}