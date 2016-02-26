using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Web.Models;

namespace LendingLibrary.Web.Bootstrappers.AutoMapperProfiles
{
    public class ItemMappings : Profile
    {
        protected override void Configure()
        {
            CreateMap<ItemViewModel, Item>()
                .ForMember(dest => dest.ItemType, opt => opt.MapFrom(src => src.ItemTypeViewModel));
            CreateMap<Item, ItemViewModel>()
                .ForMember(dest => dest.ItemTypeViewModel, opt => opt.MapFrom(src => src.ItemType));
        }
    }
}