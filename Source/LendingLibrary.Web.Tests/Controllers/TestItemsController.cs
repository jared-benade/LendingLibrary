using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Castle.Windsor;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Tests.Common.Builders;
using LendingLibrary.Tests.Common.Builders.Controller;
using LendingLibrary.Tests.Common.Builders.ViewModels;
using LendingLibrary.Web.Bootstrappers.Ioc.Installers;
using LendingLibrary.Web.Models;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using PeanutButter.RandomGenerators;

namespace LendingLibrary.Web.Tests.Controllers
{
    [TestFixture]
    public class TestItemsController
    {
        private WindsorContainer _windsorContainer;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _windsorContainer = new WindsorContainer();
            _windsorContainer.Install(new AutoMapperInstaller());
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _windsorContainer = null;
        }

        [Test]
        public void Construct_ShouldNotThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => CreateControllerBuilder().Build());
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenItemRepoIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => CreateControllerBuilder().WithItemRepository(null).Build());
            //---------------Test Result -----------------------
            Assert.IsNotNull(ex);
            Assert.AreEqual("itemRepository", ex.ParamName);
        }

        [Test]
        public void Construct_GivenItemTypeRepoIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => CreateControllerBuilder().WithItemTypeRepository(null).Build());
            //---------------Test Result -----------------------
            Assert.IsNotNull(ex);
            Assert.AreEqual("itemTypeRepository", ex.ParamName);
        }

        [Test]
        public void Index_ShouldReturnViewWithViewModels()
        {
            //---------------Set up test pack-------------------
            var controller = CreateControllerBuilder().Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Index() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModels = result.Model;
            Assert.IsNotNull(viewModels);
            Assert.IsInstanceOf<List<ItemViewModel>>(viewModels);
        }

        [Test]
        public void Index_ShouldCallGetAllActiveOnIndexRepo()
        {
            //---------------Set up test pack-------------------
            var itemRepository = Substitute.For<IItemRepository>();
            var controller = CreateControllerBuilder().WithItemRepository(itemRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Index() as ViewResult;
            //---------------Test Result -----------------------
            itemRepository.Received().GetAllActive();
        }

        [Test]
        public void Index_GivenItemsReturnedFromRepo_ShouldCallMapOnMappingEngine()
        {
            //---------------Set up test pack-------------------
            var mappingEngine = Substitute.For<IMappingEngine>();
            var itemRepository = Substitute.For<IItemRepository>();
            var item = ItemBuilder.BuildRandom();
            var items = new List<Item> {item};
            itemRepository.GetAllActive().Returns(items);
            var controller = CreateControllerBuilder().WithItemRepository(itemRepository).WithMappingEngine(mappingEngine).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Index() as ViewResult;
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<List<Item>, List<ItemViewModel>>(Arg.Any<List<Item>>());
        }

        [Test]
        public void Index_GivenNullReturnedFromRepo_ShouldNotCallMapOnMappingEngine()
        {
            //---------------Set up test pack-------------------
            var mappingEngine = Substitute.For<IMappingEngine>();
            var itemRepository = Substitute.For<IItemRepository>();
            itemRepository.GetAllActive().ReturnsNull();
            var controller = CreateControllerBuilder().WithItemRepository(itemRepository).WithMappingEngine(mappingEngine).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Index() as ViewResult;
            //---------------Test Result -----------------------
            mappingEngine.DidNotReceive().Map<List<Item>, List<ItemViewModel>>(Arg.Any<List<Item>>());
        }

        [Test]
        public void Index_GivenViewModelsReturnedFromMapping_ShouldSetOnView()
        {
            //---------------Set up test pack-------------------
            var item = ItemBuilder.BuildRandom();
            var items = new List<Item> { item };
            var viewModel = ItemViewModelBuilder.BuildRandom();
            var mappedViewModels = new List<ItemViewModel> {viewModel};
            var mappingEngine = Substitute.For<IMappingEngine>();
            mappingEngine.Map<List<Item>, List<ItemViewModel>>(items).Returns(mappedViewModels);
            var itemRepository = Substitute.For<IItemRepository>();
            itemRepository.GetAllActive().Returns(items);
            var controller = CreateControllerBuilder().WithItemRepository(itemRepository).WithMappingEngine(mappingEngine).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Index() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModels = result.Model as List<ItemViewModel>;
            Assert.IsNotNull(viewModels);
            CollectionAssert.AreEquivalent(mappedViewModels, viewModels);
        }

        [Test]
        public void Create_ShouldReturnViewWithViewModel()
        {
            //---------------Set up test pack-------------------
            var controller = CreateControllerBuilder().Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Create() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModel = result.Model;
            Assert.IsNotNull(viewModel);
            Assert.IsInstanceOf<ItemViewModel>(viewModel);
        }

        [Test]
        public void Create_GivenItemTypes_ShouldCallGetAllActiveOnItemTypeRepo()
        {
            //---------------Set up test pack-------------------
            var itemTypeRepository = Substitute.For<IItemTypeRepository>();
            var controller = CreateControllerBuilder().WithItemTypeRepository(itemTypeRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Create() as ViewResult;
            //---------------Test Result -----------------------
            itemTypeRepository.Received().GetAllActive();
        }

        [Test]
        public void Create_GivenItemTypesReturnedFromRepo_ShouldSetItemTypeSelectListOnViewModel()
        {
            //---------------Set up test pack-------------------
            var itemTypeRepository = Substitute.For<IItemTypeRepository>();
            var itemType = ItemTypeBuilder.BuildRandom();
            var itemTypes = new List<ItemType> {itemType};
            itemTypeRepository.GetAllActive().Returns(itemTypes);
            var controller = CreateControllerBuilder().WithItemTypeRepository(itemTypeRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Create() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModel = result.Model as ItemViewModel;
            Assert.IsNotNull(viewModel);
            Assert.IsNotNull(viewModel.ItemTypeSelectList);
            Assert.AreEqual(1, viewModel.ItemTypeSelectList.Count());
            Assert.AreEqual(itemType.Description, viewModel.ItemTypeSelectList.First().Text);
            Assert.AreEqual(itemType.Id.ToString(), viewModel.ItemTypeSelectList.First().Value);
        }

        [Test]
        public void CreatePost_GivenModelStateHasErrors_ShouldReturnViewWithViewModel()
        {
            //---------------Set up test pack-------------------
            var viewModel = ItemViewModelBuilder.BuildRandom();
            var controller = CreateControllerBuilder().Build();
            controller.ModelState.AddModelError("ErrorKey", "I am an error");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var itemViewModel = result.Model;
            Assert.IsNotNull(itemViewModel);
            Assert.IsInstanceOf<ItemViewModel>(itemViewModel);
        }

        [Test]
        public void CreatePost_GivenModelStateHasErrors_ShouldSetItemTypeSelectListOnViewModel()
        {
            //---------------Set up test pack-------------------
            var viewModel = ItemViewModelBuilder.BuildRandom();
            var itemTypeRepository = Substitute.For<IItemTypeRepository>();
            var itemType = ItemTypeBuilder.BuildRandom();
            var itemTypes = new List<ItemType> { itemType };
            itemTypeRepository.GetAllActive().Returns(itemTypes);
            var controller = CreateControllerBuilder().WithItemTypeRepository(itemTypeRepository).Build();
            controller.ModelState.AddModelError("ErrorKey", "I am an error");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var itemViewModel = result.Model as ItemViewModel;
            Assert.IsNotNull(itemViewModel);
            Assert.IsNotNull(itemViewModel.ItemTypeSelectList);
            Assert.AreEqual(1, itemViewModel.ItemTypeSelectList.Count());
            Assert.AreEqual(itemType.Description, itemViewModel.ItemTypeSelectList.First().Text);
            Assert.AreEqual(itemType.Id.ToString(), itemViewModel.ItemTypeSelectList.First().Value);
        }
        
        [Test]
        public void CreatePost_GivenViewModel_ShouldCallMapOnMappingEngine()
        {
            //---------------Set up test pack-------------------
            var viewModel = ItemViewModelBuilder.BuildRandom();
            var mappingEngine = Substitute.For<IMappingEngine>();
            var controller = CreateControllerBuilder().WithMappingEngine(mappingEngine).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<ItemViewModel, Item>(viewModel);
        }
                
        [Test]
        public void CreatePost_GivenSuccessfulMapping_ShouldCallSaveOnItemRepo()
        {
            //---------------Set up test pack-------------------
            var viewModel = ItemViewModelBuilder.BuildRandom();
            var mappingEngine = _windsorContainer.Resolve<IMappingEngine>();
            var itemRepository = Substitute.For<IItemRepository>();
            var controller = CreateControllerBuilder().WithMappingEngine(mappingEngine).WithItemRepository(itemRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            itemRepository.Received().Save(Arg.Any<Item>());
        }

        [Test]
        public void CreatePost_GivenUnsuccessfulMapping_ShouldNotCallSaveOnItemRepo()
        {
            //---------------Set up test pack-------------------
            var viewModel = ItemViewModelBuilder.BuildRandom();
            var mappingEngine = Substitute.For<IMappingEngine>();
            var itemRepository = Substitute.For<IItemRepository>();
            var controller = CreateControllerBuilder().WithMappingEngine(mappingEngine).WithItemRepository(itemRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            itemRepository.DidNotReceive().Save(Arg.Any<Item>());
        }

        [Test]
        public void Edit_GivenItemId_ShouldReturnViewWithViewModel()
        {
            //---------------Set up test pack-------------------
            var itemId = RandomValueGen.GetRandomInt();
            var controller = CreateControllerBuilder().Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(itemId) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModel = result.Model;
            Assert.IsNotNull(viewModel);
            Assert.IsInstanceOf<ItemViewModel>(viewModel);
        }

        [Test]
        public void Edit_GivenItemId_ShouldCallGetByIdOnItemRepo()
        {
            //---------------Set up test pack-------------------
            var itemId = RandomValueGen.GetRandomInt();
            var itemRepository = Substitute.For<IItemRepository>();
            var controller = CreateControllerBuilder().WithItemRepository(itemRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(itemId) as ViewResult;
            //---------------Test Result -----------------------
            itemRepository.Received().GetById(itemId);
        }

        [Test]
        public void Edit_GivenItemReturnedFromRepo_ShouldCallMapOnMappingEngine()
        {
            //---------------Set up test pack-------------------
            var item = ItemBuilder.BuildRandom();
            var itemId = item.Id;
            var itemRepository = Substitute.For<IItemRepository>();
            itemRepository.GetById(itemId).Returns(item);
            var mappingEngine = Substitute.For<IMappingEngine>();
            var controller = CreateControllerBuilder().WithMappingEngine(mappingEngine).WithItemRepository(itemRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(itemId) as ViewResult;
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<Item, ItemViewModel>(item);
        }

        [Test]
        public void Edit_GivenItemTypesReturnedFromRepo_ShouldSetItemTypeSelectListOnViewModel()
        {
            //---------------Set up test pack-------------------
            var item = ItemBuilder.BuildRandom();
            var itemId = item.Id;
            var itemRepository = Substitute.For<IItemRepository>();
            itemRepository.GetById(itemId).Returns(item);
            var itemTypeRepository = Substitute.For<IItemTypeRepository>();
            var itemType = ItemTypeBuilder.BuildRandom();
            var itemTypes = new List<ItemType> { itemType };
            itemTypeRepository.GetAllActive().Returns(itemTypes);
            var mappingEngine = _windsorContainer.Resolve<IMappingEngine>();
            var controller = CreateControllerBuilder().WithMappingEngine(mappingEngine).WithItemRepository(itemRepository).WithItemTypeRepository(itemTypeRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(itemId) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var itemViewModel = result.Model as ItemViewModel;
            Assert.IsNotNull(itemViewModel);
            Assert.IsNotNull(itemViewModel.ItemTypeSelectList);
            Assert.AreEqual(1, itemViewModel.ItemTypeSelectList.Count());
            Assert.AreEqual(itemType.Description, itemViewModel.ItemTypeSelectList.First().Text);
            Assert.AreEqual(itemType.Id.ToString(), itemViewModel.ItemTypeSelectList.First().Value);
        }

        [Test]
        public void EditPost_GivenModelStateErrors_ShouldReturnViewWithViewModel()
        {
            //---------------Set up test pack-------------------
            var viewModel = ItemViewModelBuilder.BuildRandom();
            var controller = CreateControllerBuilder().Build();
            controller.ModelState.AddModelError("Error", "Yet another error");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void EditPost_GivenNoModelStateErrors_ShouldReturnRedirectAction()
        {
            //---------------Set up test pack-------------------
            var viewModel = ItemViewModelBuilder.BuildRandom();
            var controller = CreateControllerBuilder().Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(viewModel) as RedirectToRouteResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Items", result.RouteValues["controller"]);
        }

        [Test]
        public void EditPost_GivenItemViewModel_ShouldMapOnMappingEngine()
        {
            //---------------Set up test pack-------------------
            var viewModel = ItemViewModelBuilder.BuildRandom();
            var mappingEngine = Substitute.For<IMappingEngine>();
            var controller = CreateControllerBuilder().WithMappingEngine(mappingEngine).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(viewModel);
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<ItemViewModel, Item>(viewModel);
        }

        [Test]
        public void EditPost_GivenSuccessfulMapping_ShouldCallSaveOnItemRepository()
        {
            //---------------Set up test pack-------------------
            var viewModel = ItemViewModelBuilder.BuildRandom();
            var itemRepository = Substitute.For<IItemRepository>();
            var mappingEngine = _windsorContainer.Resolve<IMappingEngine>();
            var controller = CreateControllerBuilder().WithMappingEngine(mappingEngine).WithItemRepository(itemRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(viewModel);
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            itemRepository.Received().Save(Arg.Any<Item>());
        }

        [Test]
        public void Edit_GivenModelErrors_ShouldSetItemTypeSelectListOnViewModel()
        {
            //---------------Set up test pack-------------------
            var viewModel = ItemViewModelBuilder.BuildRandom();
            var item = ItemBuilder.BuildRandom();
            var itemId = item.Id;
            var itemRepository = Substitute.For<IItemRepository>();
            itemRepository.GetById(itemId).Returns(item);
            var itemTypeRepository = Substitute.For<IItemTypeRepository>();
            var itemType = ItemTypeBuilder.BuildRandom();
            var itemTypes = new List<ItemType> { itemType };
            itemTypeRepository.GetAllActive().Returns(itemTypes);
            var mappingEngine = _windsorContainer.Resolve<IMappingEngine>();
            var controller = CreateControllerBuilder().WithMappingEngine(mappingEngine).WithItemRepository(itemRepository).WithItemTypeRepository(itemTypeRepository).Build();
            controller.ModelState.AddModelError("Error", "Yet another error");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var itemViewModel = result.Model as ItemViewModel;
            Assert.IsNotNull(itemViewModel);
            Assert.IsNotNull(itemViewModel.ItemTypeSelectList);
            Assert.AreEqual(1, itemViewModel.ItemTypeSelectList.Count());
            Assert.AreEqual(itemType.Description, itemViewModel.ItemTypeSelectList.First().Text);
            Assert.AreEqual(itemType.Id.ToString(), itemViewModel.ItemTypeSelectList.First().Value);
        }

        [Test]
        public void Delete_ShouldReturnJsonResult()
        {
            //---------------Set up test pack-------------------
            var id = RandomValueGen.GetRandomInt();
            var controller = CreateControllerBuilder().Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var jsonResult = controller.Delete(id) as JsonResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(jsonResult);
        }

        [Test]
        public void Delete_GivenIdIsNotEqualToZero_ShouldCallDeleteByIdOnItemRepo()
        {
            //---------------Set up test pack-------------------
            var id = RandomValueGen.GetRandomInt(1);
            var itemRepository = Substitute.For<IItemRepository>();
            var controller = CreateControllerBuilder().WithItemRepository(itemRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var jsonResult = controller.Delete(id);
            //---------------Test Result -----------------------
            itemRepository.Received().DeleteById(id);
        }

        [Test]
        public void Delete_GivenIdIsEqualToZero_ShouldNotCallDeleteByIdOnItemRepo()
        {
            //---------------Set up test pack-------------------
            const int id = 0;
            var itemRepository = Substitute.For<IItemRepository>();
            var controller = CreateControllerBuilder().WithItemRepository(itemRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var jsonResult = controller.Delete(id);
            //---------------Test Result -----------------------
            itemRepository.DidNotReceive().DeleteById(id);
        }


        private static ItemsControllerBuilder CreateControllerBuilder()
        {
            return new ItemsControllerBuilder();
        }
    }
}