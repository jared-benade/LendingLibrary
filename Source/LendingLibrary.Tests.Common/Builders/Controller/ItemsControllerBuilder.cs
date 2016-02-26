using AutoMapper;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Web.Controllers;
using NSubstitute;

namespace LendingLibrary.Tests.Common.Builders.Controller
{
    public class ItemsControllerBuilder
    {
        public ItemsControllerBuilder()
        {
            ItemRepository = Substitute.For<IItemRepository>();
            MappingEngine = Substitute.For<IMappingEngine>();
            ItemTypeRepository = Substitute.For<IItemTypeRepository>();
        }

        private IItemRepository ItemRepository { get; set; }
        private IMappingEngine MappingEngine { get; set; }
        private IItemTypeRepository ItemTypeRepository { get; set; }

        public ItemsControllerBuilder WithItemRepository(IItemRepository itemRepository)
        {
            ItemRepository = itemRepository;
            return this;
        }

        public ItemsControllerBuilder WithMappingEngine(IMappingEngine mappingEngine)
        {
            MappingEngine = mappingEngine;
            return this;
        }

        public ItemsController Build()
        {
            return new ItemsController(ItemRepository, MappingEngine, ItemTypeRepository);
        }

        public ItemsControllerBuilder WithItemTypeRepository(IItemTypeRepository itemTypeRepository)
        {
            ItemTypeRepository = itemTypeRepository;
            return this;
        }
    }
}