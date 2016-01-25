using AutoMapper;
using Castle.Windsor;
using LendingLibrary.Core.Domain;
using LendingLibrary.Tests.Common.Builders;
using LendingLibrary.Web.Bootstrappers.Ioc.Installers;
using LendingLibrary.Web.Models;
using LendingLibrary.Web.Tests.Bootstrappers.Ioc;
using NUnit.Framework;
using PeanutButter.TestUtils.Generic;

namespace LendingLibrary.Web.Tests.Bootstrappers.AutoMapperProfiles
{
    [TestFixture]
    public class TestPersonMappings
    {
        private IWindsorContainer _container;
        private readonly WindsorTestHelpers _windsorTestHelpers = new WindsorTestHelpers();

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _container = _windsorTestHelpers.CreateContainerWith(new AutoMapperInstaller());
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            if (_container != null) _container.Dispose();
        }

        [TestCase("PersonId")]
        [TestCase("FirstName")]
        [TestCase("LastName")]
        [TestCase("ContactNumber")]
        [TestCase("Email")]
        public void Map_ShouldMap_Person_To_PersonViewModel(string propertyName)
        {
            //---------------Set up test pack-------------------
            var src = PersonBuilder.BuildRandom();
            var mapper = GetMapper();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var dst = mapper.Map<PersonViewModel>(src);
            //---------------Test Result -----------------------
            PropertyAssert.AreEqual(src, dst, propertyName);
        }

        [TestCase("PersonId")]
        [TestCase("FirstName")]
        [TestCase("LastName")]
        [TestCase("ContactNumber")]
        [TestCase("Email")]
        public void Map_ShouldMap_PersonViewModel_To_Person(string propertyName)
        {
            //---------------Set up test pack-------------------
            var src = PersonViewModelBuilder.BuildRandom();
            var mapper = GetMapper();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var dst = mapper.Map<Person>(src);
            //---------------Test Result -----------------------
            PropertyAssert.AreEqual(src, dst, propertyName);
        }

        private IMappingEngine GetMapper()
        {
            return _container.Resolve<IMappingEngine>();
        }

    }
}