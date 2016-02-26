using System.ComponentModel;
using System.Web.Mvc;

namespace LendingLibrary.Web.Models
{
    public class ItemViewModel : ViewModelBase
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Edition { get; set; }
        public string Year { get; set; }
        public string Artist { get; set; }
        public bool Available { get; set; }
        public int ItemTypeId { get; set; }
        [DisplayName("Item Types")]
        public SelectList ItemTypeSelectList { get; set; }
        public ItemTypeViewModel ItemTypeViewModel { get; set; }
    }
}