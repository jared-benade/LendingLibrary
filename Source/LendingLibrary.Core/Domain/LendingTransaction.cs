using System;
using LendingLibrary.Core.Interfaces;

namespace LendingLibrary.Core.Domain
{
    public class LendingTransaction : EntityBase, IEntity
    {
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
        public DateTime DateBorrowed { get; set; }
        public DateTime? DateReturned { get; set; }
    }
}