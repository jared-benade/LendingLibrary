using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Tests.Common.Builders;
using LendingLibrary.Tests.Common.Builders.Controller;
using LendingLibrary.Tests.Common.Builders.ViewModels;
using LendingLibrary.Web.Models;
using NSubstitute;
using NUnit.Framework;

namespace LendingLibrary.Web.Tests.Controllers
{
    [TestFixture]
    public class TestLendingTransactionController
    {
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

        private static LendingTransactionControllerBuilder CreateControllerBuilder()
        {
            return new LendingTransactionControllerBuilder();
        }
    }
}