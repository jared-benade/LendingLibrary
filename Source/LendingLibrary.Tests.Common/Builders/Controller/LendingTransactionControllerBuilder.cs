using AutoMapper;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Web.Controllers;
using NSubstitute;

namespace LendingLibrary.Tests.Common.Builders.Controller
{
    public class LendingTransactionControllerBuilder
    {
        public LendingTransactionControllerBuilder()
        {
            LendingTransactionRepository = Substitute.For<ILendingTransactionRepository>();
            MappingEngine = Substitute.For<IMappingEngine>();
        }

        private ILendingTransactionRepository LendingTransactionRepository { get; set; }
        private IMappingEngine MappingEngine { get; set; }

        public LendingTransactionController Build()
        {
            return new LendingTransactionController(LendingTransactionRepository, MappingEngine);
        }

        public LendingTransactionControllerBuilder WithLendingTransactionRepository(ILendingTransactionRepository lendingTransactionRepo)
        {
            LendingTransactionRepository = lendingTransactionRepo;
            return this;
        }

        public LendingTransactionControllerBuilder WithMappingEngine(IMappingEngine mappingEngine)
        {
            MappingEngine = mappingEngine;
            return this;
        }
    }
}