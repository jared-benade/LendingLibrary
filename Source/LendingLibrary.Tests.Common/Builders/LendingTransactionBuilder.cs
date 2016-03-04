using LendingLibrary.Core.Domain;
using PeanutButter.RandomGenerators;

namespace LendingLibrary.Tests.Common.Builders
{
    public class LendingTransactionBuilder : GenericBuilder<LendingTransactionBuilder, LendingTransaction>
    {
        public LendingTransactionBuilder IsActive()
        {
            return WithProp(x => x.IsActive = true);
        }

        public LendingTransactionBuilder IsNotActive()
        {
            return WithProp(x => x.IsActive = false);
        }

        public LendingTransactionBuilder WithId(int id)
        {
            return WithProp(x => x.Id = id);
        }

        public LendingTransactionBuilder WithPersonId(int id)
        {
            return WithProp(x => x.PersonId = id);
        }

        public LendingTransactionBuilder WithItemId(int id)
        {
            return WithProp(x => x.ItemId = id);
        }
    }
}