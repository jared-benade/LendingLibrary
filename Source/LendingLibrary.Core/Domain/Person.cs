using LendingLibrary.Core.Interfaces;

namespace LendingLibrary.Core.Domain
{
    public class Person : EntityBase, IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
    }
}