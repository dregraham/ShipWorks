using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Business;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Loader
{
    public class OdbcOrderNameAndAddressLoaderTest : IDisposable
    {
        private readonly OrderEntity order = new OrderEntity();
        private readonly OdbcOrderNameAndAddressLoader testObject;

        private readonly string firstName = "Homer";
        private readonly string middleName = "J.";
        private readonly string lastName = "Simpson";
        private readonly string fullName = "Homer J. Simpson";

        private readonly string countryName = "Canada";
        private readonly string countryCode = "CA";

        private readonly string stateName = "Missouri";
        private readonly string stateCode = "MO";

        private AutoMock mock;
        private Mock<IOdbcFieldMap> map;
        public OdbcOrderNameAndAddressLoaderTest()
        {
            mock = AutoMock.GetLoose();

            var entry = mock.Mock<IOdbcFieldMapEntry>();
            entry.Setup(e => e.ExternalField.Value).Returns("Homer J. Simpson");

            map = mock.Mock<IOdbcFieldMap>();
            map.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>()))
                .Returns(new List<IOdbcFieldMapEntry> {entry.Object});
            testObject = mock.Create<OdbcOrderNameAndAddressLoader>();
        }

        [Fact]
        public void Load_ShipFirstNameSet_WhenUnparsedNameSet()
        {
            order.ShipUnparsedName = fullName;
            testObject.Load(map.Object, order);
            Assert.Equal(firstName, order.ShipFirstName);
        }

        [Fact]
        public void Load_ShipLastNameSet_WhenUnparsedNameSet()
        {
            order.ShipUnparsedName = fullName;
            testObject.Load(map.Object, order);
            Assert.Equal(lastName, order.ShipLastName);
        }

        [Fact]
        public void Load_ShipMiddleNameSet_WhenUnparsedNameSet()
        {
            order.ShipUnparsedName = fullName;
            testObject.Load(map.Object, order);
            Assert.Equal(middleName, order.ShipMiddleName);
        }

        [Fact]
        public void Load_ShipNameParseStatusIsParsed_WhenUnparsedNameSet()
        {
            order.ShipUnparsedName = fullName;
            testObject.Load(map.Object, order);
            Assert.Equal(PersonNameParseStatus.Simple, (PersonNameParseStatus) order.ShipNameParseStatus);
        }

        [Fact]
        public void Load_ShipFirstNameSet_WhenUnparsedNameNotSet()
        {
            order.ShipFirstName = firstName;
            testObject.Load(map.Object, order);
            Assert.Equal(firstName, order.ShipFirstName);
        }

        [Fact]
        public void Load_ShipNameParseStatusIsSimple_WhenUnparsedNameNotSet()
        {
            testObject.Load(map.Object, order);
            Assert.Equal(PersonNameParseStatus.Simple, (PersonNameParseStatus)order.ShipNameParseStatus);
        }

        [Fact]
        public void Load_ShipStateSet_WhenShipStateIsCode()
        {
            order.ShipStateProvCode = stateCode;
            testObject.Load(map.Object, order);
            Assert.Equal(stateCode, order.ShipStateProvCode);
        }

        [Fact]
        public void Load_ShipStateSet_WhenShipStateIsName()
        {
            order.ShipStateProvCode = stateName;
            testObject.Load(map.Object, order);
            Assert.Equal(stateCode, order.ShipStateProvCode);
        }

        [Fact]
        public void Load_ShipCountrySet_WhenShipCountryIsCode()
        {
            order.ShipCountryCode = countryCode;
            testObject.Load(map.Object, order);
            Assert.Equal(countryCode, order.ShipCountryCode);
        }

        [Fact]
        public void Load_ShipCountrySet_WhenShipCountryIsName()
        {
            order.ShipCountryCode = countryName;
            testObject.Load(map.Object, order);
            Assert.Equal(countryCode, order.ShipCountryCode);
        }

        [Fact]
        public void Load_BillFirstNameSet_WhenUnparsedNameSet()
        {
            order.BillUnparsedName = fullName;
            testObject.Load(map.Object, order);
            Assert.Equal(firstName, order.BillFirstName);
        }

        [Fact]
        public void Load_BillLastNameSet_WhenUnparsedNameSet()
        {
            order.BillUnparsedName = fullName;
            testObject.Load(map.Object, order);
            Assert.Equal(lastName, order.BillLastName);
        }

        [Fact]
        public void Load_BillMiddleNameSet_WhenUnparsedNameSet()
        {
            order.BillUnparsedName = fullName;
            testObject.Load(map.Object, order);
            Assert.Equal(middleName, order.BillMiddleName);
        }

        [Fact]
        public void Load_BillNameParseStatusIsParsed_WhenUnparsedNameSet()
        {
            order.BillUnparsedName = fullName;
            testObject.Load(map.Object, order);
            Assert.Equal(PersonNameParseStatus.Simple, (PersonNameParseStatus)order.BillNameParseStatus);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
