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
    }
}