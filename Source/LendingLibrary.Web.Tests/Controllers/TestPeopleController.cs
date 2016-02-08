using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Castle.Windsor;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Tests.Common.Builders;
using LendingLibrary.Tests.Common.Builders.Controller;
using LendingLibrary.Web.Bootstrappers.Ioc.Installers;
using LendingLibrary.Web.Controllers;
using LendingLibrary.Web.Models;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;

namespace LendingLibrary.Web.Tests.Controllers
{
    [TestFixture]
    public class TestPeopleController
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
            //--------------Execute Test -----------------------
            Assert.DoesNotThrow(() => CreateControllerBuilder().Build());
            //---------------Test Result -----------------------
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
            Assert.AreEqual("mappingEngine",ex.ParamName);
        }

        [Test]
        public void Construct_GivenPersonRepoIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            var mappingEngine = Substitute.For<IMappingEngine>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => CreateControllerBuilder().WithPersonRepository(null).Build());
            //---------------Test Result -----------------------
            Assert.IsNotNull(ex);
            Assert.AreEqual("personRepository", ex.ParamName);
        }

        [Test]
        public void Index_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var controller = CreateControllerBuilder().Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Index() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        [Test]
        public void Index_ShouldCallGetAllOnPersonRepo()
        {
            //---------------Set up test pack-------------------
            var personRepository = Substitute.For<IPersonRepository>();
            var controller = CreateControllerBuilder().WithPersonRepository(personRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Index() as ViewResult;
            //---------------Test Result -----------------------
            personRepository.Received().GetAll();
        }

        [Test]
        public void Index_GivenPeopleReturnedFromPeopleRepo_ShouldCallMapOnMappingEngine()
        {
            //---------------Set up test pack-------------------
            var person = PersonBuilder.BuildRandom();
            var people = new List<Person> {person};
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetAll().Returns(people);
            var mappingEngine = Substitute.For<IMappingEngine>();
            var controller = CreateControllerBuilder().WithPersonRepository(personRepository)
                            .WithMappingEngine(mappingEngine).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Index() as ViewResult;
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<List<Person>, List<PersonViewModel>>(people);
        }

        [Test]
        public void Index_ShouldReturnViewWithListOfViewModels()
        {
            //---------------Set up test pack-------------------
            var person = PersonBuilder.BuildRandom();
            var people = new List<Person> {person};
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetAll().Returns(people);
            var mappingEngine = _windsorContainer.Resolve<IMappingEngine>();
            var controller = CreateControllerBuilder().WithPersonRepository(personRepository).WithMappingEngine(mappingEngine).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Index() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModels = result.Model;
            Assert.IsNotNull(viewModels);
            Assert.IsInstanceOf<List<PersonViewModel>>(viewModels);
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
            var viewModel = PersonViewModelBuilder.BuildRandom();
            var controller = CreateControllerBuilder().Build();
            controller.ModelState.AddModelError("Error","I am an error, fear me");
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
            var viewModel = PersonViewModelBuilder.BuildRandom();
            var mappingEngine = Substitute.For<IMappingEngine>();
            var controller = CreateControllerBuilder().WithMappingEngine(mappingEngine).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<PersonViewModel, Person>(viewModel);
        }

        [Test]
        public void CreatePost_GivenNoModelStateErrors_ShouldCallSaveOnPersonRepository()
        {
            //---------------Set up test pack-------------------
            var viewModel = PersonViewModelBuilder.BuildRandom();
            var mappingEngine = _windsorContainer.Resolve<IMappingEngine>();
            var personRepository = Substitute.For<IPersonRepository>();
            var controller = CreateControllerBuilder().WithMappingEngine(mappingEngine).WithPersonRepository(personRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Create(viewModel) as ViewResult;
            //---------------Test Result -----------------------
            personRepository.Received().Save(Arg.Any<Person>());
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
        public void Edit_GivenId_ShouldCallGetByIdOnPersonRepository()
        {
            //---------------Set up test pack-------------------
            var id = RandomValueGen.GetRandomInt();
            var personRepository = Substitute.For<IPersonRepository>();
            var controller = CreateControllerBuilder().WithPersonRepository(personRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(id) as ViewResult;
            //---------------Test Result -----------------------
            personRepository.Received().GetById(id);
        }

        [Test]
        public void Edit_GivenPersonReturnedFromRepo_ShouldCallMapIdOnMappingEngine()
        {
            //---------------Set up test pack-------------------
            var person = PersonBuilder.BuildRandom();
            var id = person.Id;
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(id).Returns(person);
            var mappingEngine = Substitute.For<IMappingEngine>();
            var controller = CreateControllerBuilder().WithPersonRepository(personRepository).WithMappingEngine(mappingEngine).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(id) as ViewResult;
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<Person, PersonViewModel>(person);
        }

        [Test]
        public void Edit_GivenSuccessfulCallToMap_ShouldReturnViewWithViewModel()
        {
            //---------------Set up test pack-------------------
            var person = PersonBuilder.BuildRandom();
            var id = person.Id;
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(id).Returns(person);
            var mappingEngine = _windsorContainer.Resolve<IMappingEngine>();
            var controller = CreateControllerBuilder().WithPersonRepository(personRepository).WithMappingEngine(mappingEngine).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(id) as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModel = result.Model as PersonViewModel;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(person.Id, viewModel.Id);
        }

        [Test]
        public void EditPost_GivenModelStateErrors_ShouldReturnViewWithViewModel()
        {
            //---------------Set up test pack-------------------
            var viewModel = PersonViewModelBuilder.BuildRandom();
            var controller = CreateControllerBuilder().Build();
            controller.ModelState.AddModelError("Error","Yet another error");
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
            var viewModel = PersonViewModelBuilder.BuildRandom();
            var controller = CreateControllerBuilder().Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(viewModel) as RedirectToRouteResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("People", result.RouteValues["controller"]);
        }

        [Test]
        public void EditPost_GivenPersonViewModel_ShouldMapOnMappingEngine()
        {
            //---------------Set up test pack-------------------
            var viewModel = PersonViewModelBuilder.BuildRandom();
            var mappingEngine = Substitute.For<IMappingEngine>();
            var controller = CreateControllerBuilder().WithMappingEngine(mappingEngine).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(viewModel); 
            //---------------Test Result -----------------------
            mappingEngine.Received().Map<PersonViewModel, Person>(viewModel);
        }

        [Test]
        public void EditPost_GivenPersonViewModel_ShouldCallSaveOnPersonRepository()
        {
            //---------------Set up test pack-------------------
            var viewModel = PersonViewModelBuilder.BuildRandom();
            var personRepository = Substitute.For<IPersonRepository>();
            var mappingEngine = _windsorContainer.Resolve<IMappingEngine>();
            var controller = CreateControllerBuilder().WithMappingEngine(mappingEngine).WithPersonRepository(personRepository).Build();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Edit(viewModel);
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            personRepository.Received().Save(Arg.Any<Person>());
        }

        private static PeopleControllerBuilder CreateControllerBuilder()
        {
            return new PeopleControllerBuilder();
        }
    }
}