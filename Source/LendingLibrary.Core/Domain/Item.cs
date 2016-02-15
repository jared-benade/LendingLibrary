using LendingLibrary.Core.Interfaces;

namespace LendingLibrary.Core.Domain
{
    public class Item : EntityBase, IEntity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Edition { get; set; }
        public string Year { get; set; }
        public string Artist { get; set; }
        public bool Available { get; set; }
        public int ItemTypeId { get; set; }
        public virtual ItemType ItemType { get; set; }
    }
}