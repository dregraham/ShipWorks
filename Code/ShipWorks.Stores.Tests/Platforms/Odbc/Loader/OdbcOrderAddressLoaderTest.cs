using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Loader
{
    public class OdbcOrderAddressLoaderTest
    {
        OrderEntity order = new OrderEntity();
        OdbcOrderAddressLoader testObject = new OdbcOrderAddressLoader();

        private readonly string firstName = "Homer";
        private readonly string middleName = "J.";
        private readonly string lastName = "Simpson";
        private readonly string fullName = "Homer J. Simpson";

        private readonly string countryName = "Canada";
        private readonly string countryCode = "CA";

        private readonly string stateName = "Missouri";
        private readonly string stateCode = "MO";

        [Fact]
        public void Load_ShipFirstNameSet_WhenUnparsedNameSet()
        {
            order.ShipUnparsedName = fullName;
            testObject.Load(null, order);
            Assert.Equal(firstName, order.ShipFirstName);
        }

        [Fact]
        public void Load_ShipLastNameSet_WhenUnparsedNameSet()
        {
            order.ShipUnparsedName = fullName;
            testObject.Load(null, order);
            Assert.Equal(lastName, order.ShipLastName);
        }

        [Fact]
        public void Load_ShipMiddleNameSet_WhenUnparsedNameSet()
        {
            order.ShipUnparsedName = fullName;
            testObject.Load(null, order);
            Assert.Equal(middleName, order.ShipMiddleName);
        }

        [Fact]
        public void Load_ShipNameParseStatusIsParsed_WhenUnparsedNameSet()
        {
            order.ShipUnparsedName = fullName;
            testObject.Load(null, order);
            Assert.Equal(PersonNameParseStatus.Simple, (PersonNameParseStatus) order.ShipNameParseStatus);
        }

        [Fact]
        public void Load_ShipFirstNameSet_WhenUnparsedNameNotSet()
        {
            order.ShipFirstName = firstName;
            testObject.Load(null, order);
            Assert.Equal(firstName, order.ShipFirstName);
        }

        [Fact]
        public void Load_ShipNameParseStatusIsSimple_WhenUnparsedNameNotSet()
        {
            testObject.Load(null, order);
            Assert.Equal(PersonNameParseStatus.Simple, (PersonNameParseStatus)order.ShipNameParseStatus);
        }

        [Fact]
        public void Load_ShipStateSet_WhenShipStateIsCode()
        {
            order.ShipStateProvCode = stateCode;
            testObject.Load(null, order);
            Assert.Equal(stateCode, order.ShipStateProvCode);
        }

        [Fact]
        public void Load_ShipStateSet_WhenShipStateIsName()
        {
            order.ShipStateProvCode = stateName;
            testObject.Load(null, order);
            Assert.Equal(stateCode, order.ShipStateProvCode);
        }

        [Fact]
        public void Load_ShipCountrySet_WhenShipCountryIsCode()
        {
            order.ShipCountryCode = countryCode;
            testObject.Load(null, order);
            Assert.Equal(countryCode, order.ShipCountryCode);
        }

        [Fact]
        public void Load_ShipCountrySet_WhenShipCountryIsName()
        {
            order.ShipCountryCode = countryName;
            testObject.Load(null, order);
            Assert.Equal(countryCode, order.ShipCountryCode);
        }

        [Fact]
        public void Load_BillFirstNameSet_WhenUnparsedNameSet()
        {
            order.BillUnparsedName = fullName;
            testObject.Load(null, order);
            Assert.Equal(firstName, order.BillFirstName);
        }

        [Fact]
        public void Load_BillLastNameSet_WhenUnparsedNameSet()
        {
            order.BillUnparsedName = fullName;
            testObject.Load(null, order);
            Assert.Equal(lastName, order.BillLastName);
        }

        [Fact]
        public void Load_BillMiddleNameSet_WhenUnparsedNameSet()
        {
            order.BillUnparsedName = fullName;
            testObject.Load(null, order);
            Assert.Equal(middleName, order.BillMiddleName);
        }

        [Fact]
        public void Load_BillNameParseStatusIsParsed_WhenUnparsedNameSet()
        {
            order.BillUnparsedName = fullName;
            testObject.Load(null, order);
            Assert.Equal(PersonNameParseStatus.Simple, (PersonNameParseStatus)order.BillNameParseStatus);
        }

    }
}
