using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.OrderAggregate;

namespace API.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()
            .ForMember(x => x.ProductBrand, o => o.MapFrom(x => x.ProductBrand.Name))
            .ForMember(x => x.ProductType, o => o.MapFrom(x => x.ProductType.Name))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
            //product brand/type is to be set to something
            //second one is for options 
            // then we pass in the source
            CreateMap<Core.Entities.Identity.Address, AddressDTO>();
            CreateMap<AddressDTO, Core.Entities.Identity.Address>();
            CreateMap<CustomerBasketDTO, CustomerBasket>();
            CreateMap<BasketItemDTO, BasketItem>();
            CreateMap<AddressDTO, Core.Entities.OrderAggregate.Address>();
            CreateMap<Order, OrderToReturnDTO>()
            .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
            .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
            CreateMap<OrderItem,OrderItemDTO>()
            .ForMember(d => d.ProductId, o => o.MapFrom(s=> s.ItemOrdered.ProductItemId))
            .ForMember(d => d.ProductName, o => o.MapFrom(s=> s.ItemOrdered.ProductName))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(s=> s.ItemOrdered.PictureUrl))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
            



        }
    }
}