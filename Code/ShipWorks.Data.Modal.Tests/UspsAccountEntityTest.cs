using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests
{
    public class UspsAccountEntityTest
    {
        [Fact]
        public void PersonAdapter_HasCorrecPersonNameValues()
        {
            UspsAccountEntity entity = new UspsAccountEntity()
            {
                FirstName = "John",
                MiddleName = "Mid",
                LastName = "Doe"
            };

            PersonAdapter personAdapter = new PersonAdapter(entity, "");

            Assert.Equal("John", personAdapter.FirstName);
            Assert.Equal("Mid", personAdapter.MiddleName);
            Assert.Equal("Doe", personAdapter.LastName);
            Assert.Equal(PersonNameParseStatus.Simple, personAdapter.NameParseStatus);
        }
    }
}
