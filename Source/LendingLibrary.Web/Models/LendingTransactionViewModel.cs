using System;
using System.Collections.Generic;
using LendingLibrary.Core.Domain;

namespace LendingLibrary.Web.Models
{
    public class LendingTransactionViewModel : ViewModelBase
    {
        public List<int> ItemIds { get; set; }
        public Item Item { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public DateTime DateBorrowed { get; set; }
        public DateTime? DateReturned { get; set; }
    }
}