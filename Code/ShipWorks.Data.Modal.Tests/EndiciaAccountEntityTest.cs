using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests
{
    public class EndiciaAccountEntityTest
    {
        [Fact]
        public void PersonAdapter_HasCorrecPersonNameValues()
        {
            EndiciaAccountEntity entity = new EndiciaAccountEntity()
            {
                FirstName = "John",
                LastName = "Doe"
            };

            PersonAdapter personAdapter = new PersonAdapter(entity, "");

            Assert.Equal("John", personAdapter.FirstName);
            Assert.Equal("Doe", personAdapter.LastName);
            Assert.Equal(PersonNameParseStatus.Simple, personAdapter.NameParseStatus);
        }
    }
}
