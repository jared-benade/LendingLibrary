using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Web.Models;

namespace LendingLibrary.Web.Bootstrappers.AutoMapperProfiles
{
    public class PersonMappings : Profile
    {
        protected override void Configure()
        {
            CreateMap<PersonViewModel, Person>();
            CreateMap<Person, PersonViewModel>();
        }
    }
}