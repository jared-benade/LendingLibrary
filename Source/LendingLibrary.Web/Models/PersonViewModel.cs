using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace LendingLibrary.Web.Models
{
    public class PersonViewModel
    {
        public int PersonId { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Contact Number")]
        [RequiredIf("Email", Operator.EqualTo, null,
            ErrorMessage = @"Please fill in  the required text box")]
        public string ContactNumber { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RequiredIf("ContactNumber", Operator.EqualTo, null,
            ErrorMessage = @"Please fill in  the required text box")]
        public string Email { get; set; }
        public string Photo { get; set; }
    }
}