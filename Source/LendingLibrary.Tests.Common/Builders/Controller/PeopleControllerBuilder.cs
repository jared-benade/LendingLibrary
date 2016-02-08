using AutoMapper;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Web.Controllers;
using NSubstitute;

namespace LendingLibrary.Tests.Common.Builders.Controller
{
    public class PeopleControllerBuilder
    {
        public PeopleControllerBuilder()
        {
            MappingEngine = Substitute.For<IMappingEngine>();
            PersonRepository = Substitute.For<IPersonRepository>();
        }

        private IMappingEngine MappingEngine { get; set; }  
        private IPersonRepository PersonRepository { get; set; }

        public PeopleControllerBuilder WithMappingEngine(IMappingEngine mappingEngine)
        {
            MappingEngine = mappingEngine;
            return this;
        }

        public PeopleControllerBuilder WithPersonRepository(IPersonRepository personRepository)
        {
            PersonRepository = personRepository;
            return this;
        }

        public PeopleController Build()
        {
            return new PeopleController(MappingEngine, PersonRepository);
        }
    }
}