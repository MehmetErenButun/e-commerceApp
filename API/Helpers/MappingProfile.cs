using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;

namespace API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product,ProductToReturnDto>()
            .ForMember(x=>x.ProductBrand, i=>i.MapFrom(p=>p.ProductBrand.Name))
            .ForMember(x=>x.ProductType, i=>i.MapFrom(p=>p.ProductType.Name))
            .ForMember(x=>x.PictureUrl, i=>i.MapFrom<ProductUrlResolver>());

            CreateMap<Address,AdressDto>().ReverseMap();
            CreateMap<CustomerBasketDto,CustomerBasket>();
            CreateMap<BasketItemDto,BasketItem>();
        }


    }
}