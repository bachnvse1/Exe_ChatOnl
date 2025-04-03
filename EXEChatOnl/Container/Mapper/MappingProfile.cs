using System.Globalization;
using System.Text;
using AutoMapper;
using EXEChatOnl.DTOs;
using EXEChatOnl.Models;
using AutoMapper.Configuration;
namespace Container.DependencyInjection.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductSearchRequest>()
            .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => RemoveDiacritics(src.Name.ToLower())));
        CreateMap<User, ProfileUserRequest>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Customer.FullName))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Customer.Phone))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Customer.Address))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.UserRoles.Select(x => x.RoleName).ToList()));
        CreateMap<Cart, CartRequest>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));
        
        CreateMap<Order, OrdersRequest>()
            .ForMember(dest => dest.customerName, opt => opt.MapFrom(src => src.Customer.FullName));
        
        CreateMap<OrderDetail, OrderDetailsRequest>()
            .ForMember(dest => dest.productName, opt => opt.MapFrom(src => src.Product.Name));
    }

    public static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();
        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }
        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}