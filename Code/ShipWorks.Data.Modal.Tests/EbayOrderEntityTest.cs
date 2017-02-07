using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests
{
    public class EbayOrderEntityTest
    {
        [Fact]
        public void ShipPersonAdapter_HasCorrecPersonNameValues()
        {
            EbayOrderEntity order = new EbayOrderEntity()
            {
                ShipFirstName = "John",
                ShipMiddleName = "Mid",
                ShipLastName = "Doe",
                BillFirstName = "asdfasdf",
                BillMiddleName = "asdf",
                BillLastName = "asdf",
                GspFirstName = "xxx",
                GspLastName = "yyy"
            };

            PersonAdapter personAdapter = new PersonAdapter(order, "Ship");

            Assert.Equal("John", personAdapter.FirstName);
            Assert.Equal("Mid", personAdapter.MiddleName);
            Assert.Equal("Doe", personAdapter.LastName);
            Assert.Equal(PersonNameParseStatus.Simple, personAdapter.NameParseStatus);
        }

        [Fact]
        public void BillPersonAdapter_HasCorrecPersonNameValues()
        {
            EbayOrderEntity order = new EbayOrderEntity()
            {
                ShipFirstName = "asdfasdf",
                ShipMiddleName = "asdf",
                ShipLastName = "asdf",
                BillFirstName = "John",
                BillMiddleName = "Mid",
                BillLastName = "Doe",
                GspFirstName = "xxx",
                GspLastName = "yyy"
            };

            PersonAdapter personAdapter = new PersonAdapter(order, "Bill");

            Assert.Equal("John", personAdapter.FirstName);
            Assert.Equal("Mid", personAdapter.MiddleName);
            Assert.Equal("Doe", personAdapter.LastName);
            Assert.Equal(PersonNameParseStatus.Simple, personAdapter.NameParseStatus);
        }

        [Fact]
        public void GspPersonAdapter_HasCorrecPersonNameValues()
        {
            EbayOrderEntity order = new EbayOrderEntity()
            {
                ShipFirstName = "asdfasdf",
                ShipMiddleName = "asdf",
                ShipLastName = "asdf",
                BillFirstName = "bbb",
                BillMiddleName = "ccc",
                BillLastName = "ddd",
                GspFirstName = "John",
                GspLastName = "Doe"
            };

            PersonAdapter personAdapter = new PersonAdapter(order, "Gsp");

            Assert.Equal("John", personAdapter.FirstName);
            Assert.Equal("Doe", personAdapter.LastName);
            Assert.Equal(PersonNameParseStatus.Simple, personAdapter.NameParseStatus);
        }
    }
}
