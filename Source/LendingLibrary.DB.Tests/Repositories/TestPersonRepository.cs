using System;
using System.Linq;
using LendingLibrary.DB.Migrations;
using LendingLibrary.DB.Repositories;
using LendingLibrary.Tests.Common.Builders;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;
using PeanutButter.TestUtils.Entity;
using PeanutButter.Utils.Entity;

namespace LendingLibrary.DB.Tests.Repositories
{
    [TestFixture]
    public class TestPersonRepository : EntityPersistenceTestFixtureBase<LendingLibraryDbContext>
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()   
        {
            Configure(true, connectionString => new Migrator(connectionString));
            RunBeforeFirstGettingContext(Clear); 
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
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
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
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
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

        [Test]
        public void Save_GivenPerson_ShouldSaveToDbContext()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var person = PersonBuilder.BuildRandom();
                var personRepository = CreatePersonRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreEqual(0, ctx.People.Count());
                //---------------Execute Test ----------------------
                personRepository.Save(person);
                //---------------Test Result -----------------------
                Assert.AreEqual(1, ctx.People.Count());
            }
        }

        [Test]
        public void Save_GivenPersonIsNull_ShouldThrow()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var personRepository = CreatePersonRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreEqual(0, ctx.People.Count());
                //---------------Execute Test ----------------------
                var ex = Assert.Throws<ArgumentNullException>(() => personRepository.Save(null));
                //---------------Test Result -----------------------
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void GetById_GivenNoPeopleInDbContext_ShouldReturnNull()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var id = RandomValueGen.GetRandomInt();
                var personRepository = CreatePersonRepository(ctx);
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                var returnedPerson = personRepository.GetById(id);
                //---------------Test Result -----------------------
                Assert.IsNull(returnedPerson);
            }
        }

        [Test]
        public void GetById_GivenNoMatchingPersonInDbContext_ShouldReturnNull()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var person = PersonBuilder.BuildRandom();
                ctx.People.Add(person);
                var notMatchingId = RandomValueGen.GetRandomInt();
                var personRepository = CreatePersonRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreNotEqual(person.Id, notMatchingId);
                //---------------Execute Test ----------------------
                var returnedPerson = personRepository.GetById(notMatchingId);
                //---------------Test Result -----------------------
                Assert.IsNull(returnedPerson);
            }
        }

        [Test]
        public void GetById_GivenMatchingPersonInDbContext_ShouldReturnThatPerson()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var person = PersonBuilder.BuildRandom();
                ctx.People.Add(person);
                ctx.SaveChanges();
                var id = person.Id;
                var personRepository = CreatePersonRepository(ctx);
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                var returnedPerson = personRepository.GetById(id);
                //---------------Test Result -----------------------
                Assert.AreEqual(person, returnedPerson);
            }
        }

        private static void Clear(LendingLibraryDbContext ctx)
        {
            ctx.People.Clear();
            ctx.SaveChangesWithErrorReporting(); 
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