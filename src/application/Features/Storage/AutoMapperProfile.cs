
using Domain.Enums;
using Application.Features.Storage.DTOs;
using Application.Features.Storage.Commands;

using AutoMapper;
using Domain.Models;

namespace Application.Features.Storage
{
    public partial class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Stores, opt => opt.Ignore());
            CreateMap<Product, ProductDto>();
            // CreateMap<SourceModel, DestinationModel>()
            //     .ForMember(dest => dest.prop, opt => opt.MapFrom(src => src.otherProp))
            //     .ForMember(dest => dest.Prop, opt => opt.Ignore())
            //     .ForMember(dest => dest.date, opt => opt.MapFrom(src => (src.otherDate + "Z")));
        }
    }
}



