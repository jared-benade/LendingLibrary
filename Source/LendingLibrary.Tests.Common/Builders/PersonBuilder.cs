using LendingLibrary.Core.Domain;
using PeanutButter.RandomGenerators;

namespace LendingLibrary.Tests.Common.Builders
{
    public class PersonBuilder : GenericBuilder<PersonBuilder, Person>
    {
        public PersonBuilder WithId(int id)
        {
            return WithProp(x => x.Id = id);
        }

        public PersonBuilder IsNotActive()
        {
            return WithProp(x => x.IsActive = false);
        }

        public PersonBuilder IsActive()
        {
            return WithProp(x => x.IsActive = true);
        }
    }
}