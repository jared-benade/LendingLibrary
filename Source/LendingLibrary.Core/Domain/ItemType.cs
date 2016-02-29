using LendingLibrary.Core.Interfaces;

namespace LendingLibrary.Core.Domain
{
    public class ItemType : EntityBase, IEntity
    {
        public string Description { get; set; }
    }
}