using LendingLibrary.Core.Domain;
using PeanutButter.RandomGenerators;

namespace LendingLibrary.Tests.Common.Builders
{
    public class ItemBuilder : GenericBuilder<ItemBuilder, Item>
    {
        public ItemBuilder IsActive()
        {
            return WithProp(x => x.IsActive = true);
        }

        public ItemBuilder WithItemTypeId(int id)
        {
            return WithProp(x => x.ItemTypeId = id);
        }

        public ItemBuilder IsNotActive()
        {
            return WithProp(x => x.IsActive = false);
        }

        public ItemBuilder WithId(int id)
        {
            return WithProp(x => x.Id = id);
        }
    }
}