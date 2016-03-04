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
    public class TestLendingTransactionRepository : EntityPersistenceTestFixtureBase<LendingLibraryDbContext>
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
            Assert.DoesNotThrow(() => CreateRepository(Substitute.For<ILendingLibraryDbContext>()));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Construct_GivenDbContextIsNull_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var ex = Assert.Throws<ArgumentNullException>(() => CreateRepository(null));
            //---------------Test Result -----------------------
            Assert.IsNotNull(ex);
            Assert.AreEqual("dbContext", ex.ParamName);
        }

        [Test]
        public void GetAllActive_GivenNoLendingTransactionsInTheDbContext_ShouldReturnEmptyList()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var lendingTransactionRepository = CreateRepository(ctx);
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                var lendingTransactions = lendingTransactionRepository.GetAllActive();
                //---------------Test Result -----------------------
                CollectionAssert.IsEmpty(lendingTransactions);
            }
        }

        [Test]
        public void GetAllActive_GivenActiveLendingTransactionsInTheDbContext_ShouldReturnListOfLendingTransactions()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var lendingTransaction1 = new LendingTransactionBuilder().WithRandomProps().IsActive().Build();
                var lendingTransaction2 = new LendingTransactionBuilder().WithRandomProps().IsActive().Build();
                var lendingTransaction3 = new LendingTransactionBuilder().WithRandomProps().IsNotActive().Build();
                ctx.LendingTransactions.AddRange(lendingTransaction1, lendingTransaction2, lendingTransaction3);
                ctx.SaveChangesWithErrorReporting();
                var lendingTransactionRepository = CreateRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreEqual(3, ctx.LendingTransactions.Count());
                //---------------Execute Test ----------------------
                var lendingTransactions = lendingTransactionRepository.GetAllActive();
                //---------------Test Result -----------------------
                Assert.AreEqual(2, lendingTransactions.Count);
            }
        }

        [Test]
        public void GetAllActive_GivenNoActiveLendingTransactionsInTheDbContext_ShouldReturnEmptyList()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var lendingTransaction1 = new LendingTransactionBuilder().WithRandomProps().IsNotActive().Build();
                var lendingTransaction2 = new LendingTransactionBuilder().WithRandomProps().IsNotActive().Build();
                ctx.LendingTransactions.AddRange(lendingTransaction1, lendingTransaction2);
                ctx.SaveChangesWithErrorReporting();
                var lendingTransactionRepository = CreateRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreEqual(2, ctx.LendingTransactions.Count());
                //---------------Execute Test ----------------------
                var lendingTransactions = lendingTransactionRepository.GetAllActive();
                //---------------Test Result -----------------------
                Assert.AreEqual(0, lendingTransactions.Count);
            }
        }


        [Test]
        public void Save_GivenLendingTransaction_ShouldSaveToDbContext()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var lendingTransaction = LendingTransactionBuilder.BuildRandom();
                var lendingTransactionRepository = CreateRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreEqual(0, ctx.LendingTransactions.Count());
                //---------------Execute Test ----------------------
                lendingTransactionRepository.Save(lendingTransaction);
                //---------------Test Result -----------------------
                Assert.AreEqual(1, ctx.LendingTransactions.Count());
            }
        }

        [Test]
        public void Save_GivenLendingTransactionIsNull_ShouldThrow()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var lendingTransactionRepository = CreateRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreEqual(0, ctx.LendingTransactions.Count());
                //---------------Execute Test ----------------------
                var ex = Assert.Throws<ArgumentNullException>(() => lendingTransactionRepository.Save(null));
                //---------------Test Result -----------------------
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void Save_GivenExistingLendingTransaction_ShouldUpdate()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var person = PersonBuilder.BuildRandom();
                var item = ItemBuilder.BuildRandom();
                var existingLendingTransaction = new LendingTransactionBuilder().WithRandomProps()
                                                .WithPersonId(person.Id).WithItemId(item.Id).Build();
                ctx.LendingTransactions.Add(existingLendingTransaction);
                ctx.People.Add(person);
                ctx.Items.Add(item);
                ctx.SaveChanges();
                var updatedLendingTransaction = new LendingTransactionBuilder().WithRandomProps()
                                                    .WithId(existingLendingTransaction.Id).WithPersonId(person.Id)
                                                    .WithItemId(item.Id).Build();
                var lendingTransactionRepository = CreateRepository(ctx);
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                lendingTransactionRepository.Save(updatedLendingTransaction);
                //---------------Test Result -----------------------
                Assert.AreEqual(1, ctx.LendingTransactions.Count());
                var lendingTransactionInDb = ctx.LendingTransactions.FirstOrDefault();
                Assert.IsNotNull(lendingTransactionInDb);
                Assert.AreEqual(updatedLendingTransaction.Id, lendingTransactionInDb.Id);
            }
        }

        [Test]
        public void GetById_GivenNoLendingTransactionsInDbContext_ShouldReturnNull()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var id = RandomValueGen.GetRandomInt();
                var lendingTransactionRepository = CreateRepository(ctx);
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                var returnedLendingTransaction = lendingTransactionRepository.GetById(id);
                //---------------Test Result -----------------------
                Assert.IsNull(returnedLendingTransaction);
            }
        }

        [Test]
        public void GetById_GivenNoMatchingLendingTransactionInDbContext_ShouldReturnNull()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var lendingTransaction = LendingTransactionBuilder.BuildRandom();
                ctx.LendingTransactions.Add(lendingTransaction);
                var notMatchingId = lendingTransaction.Id + 1;
                var lendingTransactionRepository = CreateRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreNotEqual(lendingTransaction.Id, notMatchingId);
                //---------------Execute Test ----------------------
                var returnedLendingTransaction = lendingTransactionRepository.GetById(notMatchingId);
                //---------------Test Result -----------------------
                Assert.IsNull(returnedLendingTransaction);
            }
        }

        [Test]
        public void GetById_GivenMatchingLendingTransactionInDbContext_ShouldReturnThatLendingTransaction()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var lendingTransaction = LendingTransactionBuilder.BuildRandom();
                ctx.LendingTransactions.Add(lendingTransaction);
                ctx.SaveChanges();
                var id = lendingTransaction.Id;
                var lendingTransactionRepository = CreateRepository(ctx);
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                var returnedLendingTransaction = lendingTransactionRepository.GetById(id);
                //---------------Test Result -----------------------
                Assert.AreEqual(lendingTransaction, returnedLendingTransaction);
            }
        }

        [Test]
        public void DeleteById_GivenMatchingLendingTransactionInContext_ShouldSoftDeleteLendingTransaction()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var lendingTransaction = LendingTransactionBuilder.BuildRandom();
                ctx.LendingTransactions.Add(lendingTransaction);
                ctx.SaveChanges();
                var id = lendingTransaction.Id;
                var lendingTransactionRepository = CreateRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreEqual(1, ctx.LendingTransactions.Count());
                //---------------Execute Test ----------------------
                lendingTransactionRepository.DeleteById(id);
                //---------------Test Result -----------------------
                Assert.AreEqual(1, ctx.LendingTransactions.Count());
                Assert.IsFalse(ctx.LendingTransactions.First().IsActive);
            }
        }

        [Test]
        public void DeleteById_GivenNoMatchingLendingTransactionInContext_ShouldNotSoftDeleteAnyLendingTransactions()
        {
            using (var ctx = GetContext())
            {
                //---------------Set up test pack-------------------
                var lendingTransaction = new LendingTransactionBuilder().WithRandomProps().IsActive().Build();
                ctx.LendingTransactions.Add(lendingTransaction);
                ctx.SaveChanges();
                var notMatchingId = lendingTransaction.Id + 1;
                var lendingTransactionRepository = CreateRepository(ctx);
                //---------------Assert Precondition----------------
                Assert.AreEqual(1, ctx.LendingTransactions.Count());
                //---------------Execute Test ----------------------
                lendingTransactionRepository.DeleteById(notMatchingId);
                //---------------Test Result -----------------------
                Assert.AreEqual(1, ctx.LendingTransactions.Count());
                Assert.IsTrue(ctx.LendingTransactions.First().IsActive);
            }
        }

        private static void Clear(LendingLibraryDbContext ctx)
        {
            ctx.LendingTransactions.Clear();
            ctx.SaveChangesWithErrorReporting();
        }

        private static LendingTransactionRepository CreateRepository(ILendingLibraryDbContext dbContext)
        {
            return new LendingTransactionRepository(dbContext);
        }
    }
}