using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Web.Models;

namespace LendingLibrary.Web.Bootstrappers.AutoMapperProfiles
{
    public class ItemTypeMappings : Profile
    {
        protected override void Configure()
        {
            CreateMap<ItemTypeViewModel, ItemType>();
            CreateMap<ItemType, ItemTypeViewModel>();
        }
    }
}