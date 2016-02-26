using System;
using System.Linq;
using LendingLibrary.Core.Domain;
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
    public class TestItemRepository : EntityPersistenceTestFixtureBase<LendingLibraryDbContext>
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
            Assert.DoesNotThrow(() => CreateItemRepository(dbContext));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenDbContextIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => CreateItemRepository(null));
            //---------------Test Result -----------------------
            Assert.IsNotNull(ex);
            Assert.AreEqual("dbContext", ex.ParamName);
        }

        [Test]
        public void GetAllActive_GivenNoItemsInTheDbContext_ShouldReturnEmptyList()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var itemRepository = CreateItemRepository(ctx);
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                var items = itemRepository.GetAllActive();
                //---------------Test Result -----------------------
                CollectionAssert.IsEmpty(items);
            }
        }

        [Test]
        public void GetAllActive_GivenActiveItemsInTheDbContext_ShouldReturnListOfItems()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var itemType = ItemTypeBuilder.BuildRandom();
                var item1 = new ItemBuilder().IsActive().WithItemTypeId(itemType.Id).Build();
                var item2 = new ItemBuilder().IsActive().WithItemTypeId(itemType.Id).Build();
                var item3 = new ItemBuilder().IsNotActive().WithItemTypeId(itemType.Id).Build();
                ctx.ItemTypes.Add(itemType);
                ctx.Items.AddRange(item1, item2, item3);
                ctx.SaveChangesWithErrorReporting();
                var itemRepository = CreateItemRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreEqual(3, ctx.Items.Count());
                //---------------Execute Test ----------------------
                var items = itemRepository.GetAllActive();
                //---------------Test Result -----------------------
                Assert.AreEqual(2, items.Count);
            }
        }

        [Test]
        public void GetAllActive_GivenNoActiveItemsInTheDbContext_ShouldReturnEmptyList()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var itemType = ItemTypeBuilder.BuildRandom();
                var item1 = new ItemBuilder().IsNotActive().WithItemTypeId(itemType.Id).Build();
                var item2 = new ItemBuilder().IsNotActive().WithItemTypeId(itemType.Id).Build();
                ctx.ItemTypes.Add(itemType);
                ctx.Items.AddRange(item1, item2);
                ctx.SaveChangesWithErrorReporting();
                var itemRepository = CreateItemRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreEqual(2, ctx.Items.Count());
                //---------------Execute Test ----------------------
                var items = itemRepository.GetAllActive();
                //---------------Test Result -----------------------
                Assert.AreEqual(0, items.Count);
            }
        }

        [Test]
        public void Save_GivenItem_ShouldSaveToDbContext()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var item = ItemBuilder.BuildRandom();
                var itemRepository = CreateItemRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreEqual(0, ctx.Items.Count());
                //---------------Execute Test ----------------------
                itemRepository.Save(item);
                //---------------Test Result -----------------------
                Assert.AreEqual(1, ctx.Items.Count());
            }
        }

        [Test]
        public void Save_GivenItemIsNull_ShouldThrow()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var itemRepository = CreateItemRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreEqual(0, ctx.Items.Count());
                //---------------Execute Test ----------------------
                var ex = Assert.Throws<ArgumentNullException>(() => itemRepository.Save(null));
                //---------------Test Result -----------------------
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void Save_GivenExistingItem_ShouldUpdate()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var existingItem = ItemBuilder.BuildRandom();
                ctx.Items.Add(existingItem);
                ctx.SaveChanges();
                var updatedItem = new ItemBuilder().WithRandomProps().WithId(existingItem.Id).WithItemTypeId(existingItem.ItemTypeId).Build();
                var itemRepository = CreateItemRepository(ctx);
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                itemRepository.Save(updatedItem);
                //---------------Test Result -----------------------
                Assert.AreEqual(1, ctx.Items.Count());
                var itemInDb = ctx.Items.FirstOrDefault();
                Assert.IsNotNull(itemInDb);
                Assert.AreEqual(updatedItem.Id, itemInDb.Id);
                Assert.AreEqual(updatedItem.Artist, itemInDb.Artist);
            }
        }

        [Test]
        public void GetById_GivenNoItemsInDbContext_ShouldReturnNull()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var id = RandomValueGen.GetRandomInt();
                var itemRepository = CreateItemRepository(ctx);
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                var returnedItem = itemRepository.GetById(id);
                //---------------Test Result -----------------------
                Assert.IsNull(returnedItem);
            }
        }

        [Test]
        public void GetById_GivenNoMatchingItemInDbContext_ShouldReturnNull()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var item = ItemBuilder.BuildRandom();
                ctx.Items.Add(item);
                var notMatchingId = item.Id + 1;
                var itemRepository = CreateItemRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreNotEqual(item.Id, notMatchingId);
                //---------------Execute Test ----------------------
                var returnedItem = itemRepository.GetById(notMatchingId);
                //---------------Test Result -----------------------
                Assert.IsNull(returnedItem);
            }
        }

        [Test]
        public void GetById_GivenMatchingItemInDbContext_ShouldReturnThatItem()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var item = ItemBuilder.BuildRandom();
                ctx.Items.Add(item);
                ctx.SaveChanges();
                var id = item.Id;
                var itemRepository = CreateItemRepository(ctx);
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                var returnedItem = itemRepository.GetById(id);
                //---------------Test Result -----------------------
                Assert.AreEqual(item, returnedItem);
            }
        }

        [Test]
        public void DeleteById_GivenMatchingItemInContext_ShouldSoftDeleteItem()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var item = ItemBuilder.BuildRandom();
                ctx.Items.Add(item);
                ctx.SaveChanges();
                var id = item.Id;
                var itemRepository = CreateItemRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreEqual(1, ctx.Items.Count());
                //---------------Execute Test ----------------------
                itemRepository.DeleteById(id);
                //---------------Test Result -----------------------
                Assert.AreEqual(1, ctx.Items.Count());
                Assert.IsFalse(ctx.Items.First().IsActive);
            }
        }

        [Test]
        public void DeleteById_GivenNoMatchingItemInContext_ShouldNotSoftDeleteAnyItems()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var item = new ItemBuilder().WithRandomProps().IsActive().Build();
                ctx.Items.Add(item);
                ctx.SaveChanges();
                var notMatchingId = item.Id + 1;
                var itemRepository = CreateItemRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreEqual(1, ctx.Items.Count());
                //---------------Execute Test ----------------------
                itemRepository.DeleteById(notMatchingId);
                //---------------Test Result -----------------------
                Assert.AreEqual(1, ctx.Items.Count());
                Assert.IsTrue(ctx.Items.First().IsActive);
            }
        }

        private static void Clear(LendingLibraryDbContext ctx)
        {
            ctx.Items.Clear();
            ctx.SaveChangesWithErrorReporting();
        }

        private static ItemRepository CreateItemRepository(ILendingLibraryDbContext dbContext)
        {
            return new ItemRepository(dbContext);
        }
    }
}