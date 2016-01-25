using System;
using AutoMapper;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.Web.Controllers;
using NSubstitute;
using NUnit.Framework;

namespace LendingLibrary.Web.Tests.Controllers
{
    [TestFixture]
    public class TestPeopleController
    {
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
            var ex = Assert.Throws<ArgumentNullException>(() => CreatePeopleController(null));
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
    }
}