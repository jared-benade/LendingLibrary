using System;
using LendingLibrary.DB.Migrations;
using LendingLibrary.DB.Repositories;
using LendingLibrary.Tests.Common.Builders;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.TestUtils.Entity;
using PeanutButter.Utils.Entity;

namespace LendingLibrary.DB.Tests.Repositories
{
    [TestFixture]
    public class TestItemTypeRepository : EntityPersistenceTestFixtureBase<LendingLibraryDbContext>
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
            var dbContext = Substitute.For<ILendingLibraryDbContext>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => CreateItemTypeRepository(dbContext));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenDbContextIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => CreateItemTypeRepository(null));
            //---------------Test Result -----------------------
            Assert.IsNotNull(ex);
            Assert.AreEqual("dbContext",ex.ParamName);
        }

        [Test]
        public void GetAllActive_GivenNoItemTypes_ShouldReturnEmptyList()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var itemTypeRepository = CreateItemTypeRepository(ctx);
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                var itemTypes = itemTypeRepository.GetAllActive();
                //---------------Test Result -----------------------
                CollectionAssert.IsEmpty(itemTypes);
            }
        }

        [Test]
        public void GetAllActive_GivenNoActiveItemTypes_ShouldReturnEmptyList()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var itemType = new ItemTypeBuilder().IsNotActive().Build();
                ctx.ItemTypes.Add(itemType);
                var itemTypeRepository = CreateItemTypeRepository(ctx);
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                var itemTypes = itemTypeRepository.GetAllActive();
                //---------------Test Result -----------------------
                CollectionAssert.IsEmpty(itemTypes);
            }
        }

        [Test]
        public void GetAllActive_GivenActiveItemTypes_ShouldReturnEmptyList()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var itemType1 = new ItemTypeBuilder().IsActive().Build();
                var itemType2 = new ItemTypeBuilder().IsActive().Build();
                ctx.ItemTypes.AddRange(itemType1, itemType2);
                ctx.SaveChanges();
                var itemTypeRepository = CreateItemTypeRepository(ctx);
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                var itemTypes = itemTypeRepository.GetAllActive();
                //---------------Test Result -----------------------
                Assert.AreEqual(2, itemTypes.Count);
            }
        }

        private static void Clear(LendingLibraryDbContext ctx)
        {
            ctx.ItemTypes.Clear();
            ctx.SaveChangesWithErrorReporting();
        }


        private static ItemTypeRepository CreateItemTypeRepository(ILendingLibraryDbContext dbContext)
        {
            return new ItemTypeRepository(dbContext);
        }
    }
}