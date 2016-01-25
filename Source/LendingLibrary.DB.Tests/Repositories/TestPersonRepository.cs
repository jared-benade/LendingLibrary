using System;
using System.Linq;
using LendingLibrary.Core.Domain;
using LendingLibrary.DB.Migrations;
using LendingLibrary.DB.Repositories;
using LendingLibrary.Tests.Common.Builders;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.FluentMigrator;
using PeanutButter.TestUtils.Entity;
using PeanutButter.Utils.Entity;

namespace LendingLibrary.DB.Tests.Repositories
{
    [TestFixture]
    public class TestPersonRepository : EntityPersistenceTestFixtureBase<LendingLibraryDbContext>
    {
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            DisableDatabaseRegeneration();
        }

        [Test]
        public void Construct_ShouldNotThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => CreatePersonRepository());
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenDbContextIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => CreatePersonRepository(null));
            //---------------Test Result -----------------------
            Assert.IsNotNull(ex);
            Assert.AreEqual("dbContext",ex.ParamName);
        }

        [Test]
        public void GetAll_GivenNoPeopleInDbContext_ShouldReturnEmptyList()
        {
            Configure(true, connectionString => new DBMigrationsRunner(connectionString));
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                Clear(ctx);
                var personRepository = CreatePersonRepository(ctx);
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                var persons = personRepository.GetAll();
                //---------------Test Result -----------------------
                CollectionAssert.IsEmpty(persons);
            }
        }

        [Test]
        public void GetAll_GivenPeopleInDbContext_ShouldReturnListOfPeople()
        {
            Configure(true, connectionString => new DBMigrationsRunner(connectionString));
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                Clear(ctx);
                var person = PersonBuilder.BuildRandom();
                ctx.People.Add(person);
                ctx.SaveChanges();
                var personRepository = CreatePersonRepository(ctx);
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                var persons = personRepository.GetAll();
                //---------------Test Result -----------------------
                Assert.AreEqual(1, persons.Count);
                Assert.AreEqual(person, persons.First());
            }
        }

        private static void Clear(ILendingLibraryDbContext ctx)
        {
            ctx.People.Clear();
        }

        private PersonRepository CreatePersonRepository(ILendingLibraryDbContext dbContext)
        {
            return new PersonRepository(dbContext);
        }

        private static PersonRepository CreatePersonRepository()
        {
            return new PersonRepository(Substitute.For<ILendingLibraryDbContext>());
        }
    }
}