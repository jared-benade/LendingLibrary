using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Web.Models;

namespace LendingLibrary.Web.Bootstrappers.AutoMapperProfiles
{
    public class LendingTransactionMappings : Profile
    {
        protected override void Configure()
        {
            CreateMap<LendingTransactionViewModel, LendingTransaction>();
            CreateMap<LendingTransaction, LendingTransactionViewModel>();
        }
    }
}