using LendingLibrary.Core.Domain;
using PeanutButter.RandomGenerators;

namespace LendingLibrary.Tests.Common.Builders
{
    public class ItemTypeBuilder : GenericBuilder<ItemTypeBuilder, ItemType>
    {
        public ItemTypeBuilder IsNotActive()
        {
            return WithProp(x => x.IsActive = false);
        }

        public ItemTypeBuilder IsActive()
        {
            return WithProp(x => x.IsActive = true);
        }
    }
}