using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Castle.Windsor;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Tests.Common.Builders;
using LendingLibrary.Web.Bootstrappers.Ioc.Installers;
using LendingLibrary.Web.Controllers;
using LendingLibrary.Web.Models;
using NSubstitute;
using NUnit.Framework;

namespace LendingLibrary.Web.Tests.Controllers
{
    [TestFixture]
    public class TestPeopleController
    {
        private WindsorContainer _windsorContainer;

        [TestFixtureSetUp]
        public void TestSetUpFixture()
        {
            _windsorContainer = new WindsorContainer();
            _windsorContainer.Install(new AutoMapperInstaller());
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _windsorContainer = null;
        }

        [Test]
        public void Construct_ShouldNotThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //--------------Execute Test -----------------------
            Assert.DoesNotThrow(() => CreatePeopleController());
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenMappingEngineIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => CreatePeopleController((IMappingEngine)null));
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
            var ex = Assert.Throws<ArgumentNullException>(() => CreatePeopleController(mappingEngine, null));
            //---------------Test Result -----------------------
            Assert.IsNotNull(ex);
            Assert.AreEqual("personRepository", ex.ParamName);
        }

        [Test]
        public void Index_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var controller = CreatePeopleController();
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
            var controller = CreatePeopleController(personRepository);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Index() as ViewResult;
            //---------------Test Result -----------------------
            personRepository.Received().GetAll();
        }

        [Test]
        public void Index_GivenPeopleReturnedFromPeopleRepo_ShouldCallMappingEngine()
        {
            //---------------Set up test pack-------------------
            var person = PersonBuilder.BuildRandom();
            var people = new List<Person> {person};
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetAll().Returns(people);
            var mappingEngine = Substitute.For<IMappingEngine>();
            var controller = CreatePeopleController(mappingEngine,personRepository);
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
            var controller = CreatePeopleController(mappingEngine,personRepository);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Index() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
            var viewModels = result.Model;
            Assert.IsNotNull(viewModels);
            Assert.IsInstanceOf<List<PersonViewModel>>(viewModels);
        }


        private PeopleController CreatePeopleController()
        {
            return new PeopleController(Substitute.For<IMappingEngine>(), Substitute.For<IPersonRepository>());  
        }

        private static PeopleController CreatePeopleController(IMappingEngine mappingEngine)
        {
            return new PeopleController(mappingEngine, Substitute.For<IPersonRepository>());
        }

        private static PeopleController CreatePeopleController(IMappingEngine mappingEngine, IPersonRepository personRepo)
        {
            return new PeopleController(mappingEngine, personRepo);
        }

        private static PeopleController CreatePeopleController(IPersonRepository personRepo)
        {
            return new PeopleController(Substitute.For<IMappingEngine>(), personRepo);
        }
    }
}