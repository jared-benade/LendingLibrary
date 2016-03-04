using System;
using System.Collections.Generic;
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
using NUnit.Framework;
using PeanutButter.RandomGenerators;

namespace LendingLibrary.Web.Tests.Controllers
{
    [TestFixture]
    public class TestLendingTransactionController
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
        public void Construct_GivenLendingTransactionRepoIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => CreateControllerBuilder().WithLendingTransactionRepository(null).Build());
            //---------------Test Result -----------------------
            Assert.IsNotNull(ex);
            Assert.AreEqual("lendingTransactionRepository", ex.ParamName);
        }

        [Test]
        public void Construct_GivenMappingEngineIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => CreateControllerBuilder().WithMappingEngine(null).Build());
            //---------------Test Result -----------------------
            Assert.IsNotNull(ex);
            Assert.AreEqual("mappingEngine", ex.ParamName);
        }

        [Test]
        public void Index_ShouldReturnViewWithViewModel()
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
            Assert.IsInstanceOf<List<LendingTransactionViewModel>>(viewModels);
        }

        [Test]
        public void Index_GivenCallToRepo_ShouldMapAndReturnViewModels()
        {
            //---------------Set up test pack-------------------
            var lendingTransaction = LendingTransactionBuilder.BuildRandom();
            var lendingTransactions = new List<LendingTransaction> {lendingTransaction};
            var viewModel = LendingTransactionViewModelBuilder.BuildRandom();
            var viewModels = new List<LendingTransactionViewModel> {viewModel};
            var lendingTransactionRepository = Substitute.For<ILendingTransactionRepository>();
            lendingTransactionRepository.GetAllActive().Returns(lendingTransactions);
            var mappingEngine = Substitute.For<IMappingEngine>();
            mappingEngine.Map<List<LendingTransaction>, List<LendingTransactionViewModel>>(lendingTransactions)
                .Returns(viewModels);
            var controller = CreateControllerBuilder().WithLendingTransactionRepository(lendingTransactionRepository)
                            .WithMappingEngine(mappingEngine).Build();
            //---------------Assert Precondition----------------S
            //---------------Execute Test ----------------------
            var result = controller.Index() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var returnedViewModels = result.Model as List<LendingTransactionViewModel>;
            Assert.IsNotNull(returnedViewModels);
            Assert.AreEqual(viewModels, returnedViewModels);
        }

        [Test]
        public void Create_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var controller = CreateControllerBuilder().Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Create();
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void CreatePost_GivenModelStateErrors_ShouldReturnViewWithViewModel()
        {
            //---------------Set up test pack-------------------
            var viewModel = LendingTransactionViewModelBuilder.BuildRandom();
            var controller = CreateControllerBuilder().Build();
            controller.ModelState.AddModelError("Error", "I am an error, fear me");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void CreatePost_GivenNoModelStateErrors_ShouldCallMapOnMappingEngine()
        {
            //---------------Set up test pack-------------------
            var viewModel = LendingTransactionViewModelBuilder.BuildRandom();
            var mappingEngine = Substitute.For<IMappingEngine>();
            var controller = CreateControllerBuilder().WithMappingEngine(mappingEngine).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<LendingTransactionViewModel, LendingTransaction>(viewModel);
        }

        [Test]
        public void CreatePost_GivenNoModelStateErrors_ShouldCallSaveOnLendingTransactionRepository()
        {
            //---------------Set up test pack-------------------
            var viewModel = LendingTransactionViewModelBuilder.BuildRandom();
            var mappingEngine = _windsorContainer.Resolve<IMappingEngine>();
            var lendingTransactionRepository = Substitute.For<ILendingTransactionRepository>();
            var controller = CreateControllerBuilder().WithMappingEngine(mappingEngine).WithLendingTransactionRepository(lendingTransactionRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            lendingTransactionRepository.Received().Save(Arg.Any<LendingTransaction>());
        }

        [Test]
        public void Edit_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var id = RandomValueGen.GetRandomInt();
            var controller = CreateControllerBuilder().Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(id) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void Edit_GivenId_ShouldCallGetByIdOnLendingTransactionRepository()
        {
            //---------------Set up test pack-------------------
            var id = RandomValueGen.GetRandomInt();
            var lendingTransactionRepository = Substitute.For<ILendingTransactionRepository>();
            var controller = CreateControllerBuilder().WithLendingTransactionRepository(lendingTransactionRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(id) as ViewResult;
            //---------------Test Result -----------------------
            lendingTransactionRepository.Received().GetById(id);
        }

        [Test]
        public void Edit_GivenLendingTransactionReturnedFromRepo_ShouldCallMapIdOnMappingEngine()
        {
            //---------------Set up test pack-------------------
            var lendingTransaction = LendingTransactionBuilder.BuildRandom();
            var id = lendingTransaction.Id;
            var lendingTransactionRepository = Substitute.For<ILendingTransactionRepository>();
            lendingTransactionRepository.GetById(id).Returns(lendingTransaction);
            var mappingEngine = Substitute.For<IMappingEngine>();
            var controller = CreateControllerBuilder().WithLendingTransactionRepository(lendingTransactionRepository).WithMappingEngine(mappingEngine).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(id) as ViewResult;
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<LendingTransaction, LendingTransactionViewModel>(lendingTransaction);
        }

        [Test]
        public void Edit_GivenSuccessfulCallToMap_ShouldReturnViewWithViewModel()
        {
            //---------------Set up test pack-------------------
            var lendingTransaction = LendingTransactionBuilder.BuildRandom();
            var id = lendingTransaction.Id;
            var lendingTransactionRepository = Substitute.For<ILendingTransactionRepository>();
            lendingTransactionRepository.GetById(id).Returns(lendingTransaction);
            var mappingEngine = _windsorContainer.Resolve<IMappingEngine>();
            var controller = CreateControllerBuilder().WithLendingTransactionRepository(lendingTransactionRepository).WithMappingEngine(mappingEngine).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(id) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModel = result.Model as LendingTransactionViewModel;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(lendingTransaction.Id, viewModel.Id);
        }

        [Test]
        public void EditPost_GivenModelStateErrors_ShouldReturnViewWithViewModel()
        {
            //---------------Set up test pack-------------------
            var viewModel = LendingTransactionViewModelBuilder.BuildRandom();
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
            var viewModel = LendingTransactionViewModelBuilder.BuildRandom();
            var controller = CreateControllerBuilder().Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(viewModel) as RedirectToRouteResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("LendingTransaction", result.RouteValues["controller"]);
        }

        [Test]
        public void EditPost_GivenLendingTransactionViewModel_ShouldMapOnMappingEngine()
        {
            //---------------Set up test pack-------------------
            var viewModel = LendingTransactionViewModelBuilder.BuildRandom();
            var mappingEngine = Substitute.For<IMappingEngine>();
            var controller = CreateControllerBuilder().WithMappingEngine(mappingEngine).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(viewModel);
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<LendingTransactionViewModel, LendingTransaction>(viewModel);
        }

        [Test]
        public void EditPost_GivenLendingTransactionViewModel_ShouldCallSaveOnLendingTransactionRepository()
        {
            //---------------Set up test pack-------------------
            var viewModel = LendingTransactionViewModelBuilder.BuildRandom();
            var lendingTransactionRepository = Substitute.For<ILendingTransactionRepository>();
            var mappingEngine = _windsorContainer.Resolve<IMappingEngine>();
            var controller = CreateControllerBuilder().WithMappingEngine(mappingEngine).WithLendingTransactionRepository(lendingTransactionRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(viewModel);
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            lendingTransactionRepository.Received().Save(Arg.Any<LendingTransaction>());
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
        public void Delete_GivenIdIsNotEqualToZero_ShouldCallDeleteByIdOnLendingTransactionRepo()
        {
            //---------------Set up test pack-------------------
            var id = RandomValueGen.GetRandomInt(1);
            var lendingTransactionRepository = Substitute.For<ILendingTransactionRepository>();
            var controller = CreateControllerBuilder().WithLendingTransactionRepository(lendingTransactionRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var jsonResult = controller.Delete(id);
            //---------------Test Result -----------------------
            lendingTransactionRepository.Received().DeleteById(id);
        }

        [Test]
        public void Delete_GivenIdIsEqualToZero_ShouldNotCallDeleteByIdOnLendingTransactionRepo()
        {
            //---------------Set up test pack-------------------
            const int id = 0;
            var lendingTransactionRepository = Substitute.For<ILendingTransactionRepository>();
            var controller = CreateControllerBuilder().WithLendingTransactionRepository(lendingTransactionRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var jsonResult = controller.Delete(id);
            //---------------Test Result -----------------------
            lendingTransactionRepository.DidNotReceive().DeleteById(id);
        }

        private static LendingTransactionControllerBuilder CreateControllerBuilder()
        {
            return new LendingTransactionControllerBuilder();
        }
    }
}