using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Linq;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcOrderChargeLoaderTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcOrderChargeLoaderTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void Load_WithNullMap_ThrowsArgumentNullException()
        {
            OdbcOrderChargeLoader testObject = new OdbcOrderChargeLoader();
            Assert.Throws<ArgumentNullException>(() => testObject.Load(null, null));
        }

        [Fact]
        public void Load_WithNullOrderEntity_DoesNotThrowsArgumentNullException()
        {
            Mock<IOdbcFieldMap> map = mock.Mock<IOdbcFieldMap>();
            OdbcOrderChargeLoader testObject = new OdbcOrderChargeLoader();
            testObject.Load(map.Object, null);
        }

        [Fact]
        public void Load_GetsChargeEntriesFromMap()
        {
            ShipWorksOdbcMappableField shipworksField = new ShipWorksOdbcMappableField(OrderChargeFields.Amount, "Tax");
            shipworksField.LoadValue(123);
            ExternalOdbcMappableField externalField = new ExternalOdbcMappableField(new OdbcTable("Order"), new OdbcColumn("Tax Column"));
            OdbcFieldMapEntry entry1 = new OdbcFieldMapEntry(shipworksField, externalField);

            Mock<IOdbcFieldMap> map = mock.Mock<IOdbcFieldMap>();
            map.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { entry1 });

            OdbcOrderChargeLoader testObject = new OdbcOrderChargeLoader();

            OrderEntity order = new OrderEntity();

            testObject.Load(map.Object, order);

            map.Verify(m => m.FindEntriesBy(It.IsAny<EntityField2>()));
        }

        [Fact]
        public void Load_SetsChargeAmountFromShipWorksFieldValue()
        {
            ShipWorksOdbcMappableField shipworksField = new ShipWorksOdbcMappableField(OrderChargeFields.Amount, "Tax");
            shipworksField.LoadValue(123);
            ExternalOdbcMappableField externalField = new ExternalOdbcMappableField(new OdbcTable("Order"), new OdbcColumn("Tax Column"));
            OdbcFieldMapEntry entry1 = new OdbcFieldMapEntry(shipworksField, externalField);

            Mock<IOdbcFieldMap> map = mock.Mock<IOdbcFieldMap>();
            map.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { entry1 });

            OdbcOrderChargeLoader testObject = new OdbcOrderChargeLoader();

            OrderEntity order = new OrderEntity();

            testObject.Load(map.Object, order);

            var orderCharge = order.OrderCharges.FirstOrDefault();

            Assert.Equal(123, orderCharge.Amount);
        }

        [Fact]
        public void Load_WhenChargeDisplayNameIsTaxAmount_SetsChargeTypeToTax()
        {
            ShipWorksOdbcMappableField shipworksField = new ShipWorksOdbcMappableField(OrderChargeFields.Amount, "Tax Amount");
            shipworksField.LoadValue(123);
            ExternalOdbcMappableField externalField = new ExternalOdbcMappableField(new OdbcTable("Order"), new OdbcColumn("Tax Column"));
            OdbcFieldMapEntry entry1 = new OdbcFieldMapEntry(shipworksField, externalField);

            Mock<IOdbcFieldMap> map = mock.Mock<IOdbcFieldMap>();
            map.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { entry1 });

            OdbcOrderChargeLoader testObject = new OdbcOrderChargeLoader();

            OrderEntity order = new OrderEntity();

            testObject.Load(map.Object, order);

            var orderCharge = order.OrderCharges.FirstOrDefault();

            Assert.Equal("TAX", orderCharge.Type);
        }

        [Fact]
        public void Load_WhenChargeDisplayNameIsShippingAmount_SetsChargeTypeToShipping()
        {
            ShipWorksOdbcMappableField shipworksField = new ShipWorksOdbcMappableField(OrderChargeFields.Amount, "Shipping Amount");
            shipworksField.LoadValue(123);
            ExternalOdbcMappableField externalField = new ExternalOdbcMappableField(new OdbcTable("Order"), new OdbcColumn("Tax Column"));
            OdbcFieldMapEntry entry1 = new OdbcFieldMapEntry(shipworksField, externalField);

            Mock<IOdbcFieldMap> map = mock.Mock<IOdbcFieldMap>();
            map.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { entry1 });

            OdbcOrderChargeLoader testObject = new OdbcOrderChargeLoader();

            OrderEntity order = new OrderEntity();

            testObject.Load(map.Object, order);

            var orderCharge = order.OrderCharges.FirstOrDefault();

            Assert.Equal("SHIPPING", orderCharge.Type);
        }

        [Fact]
        public void Load_WhenChargeDisplayNameIsInsuranceAmount_SetsChargeTypeToInsurance()
        {
            ShipWorksOdbcMappableField shipworksField = new ShipWorksOdbcMappableField(OrderChargeFields.Amount, "Insurance Amount");
            shipworksField.LoadValue(123);
            ExternalOdbcMappableField externalField = new ExternalOdbcMappableField(new OdbcTable("Order"), new OdbcColumn("Tax Column"));
            OdbcFieldMapEntry entry1 = new OdbcFieldMapEntry(shipworksField, externalField);

            Mock<IOdbcFieldMap> map = mock.Mock<IOdbcFieldMap>();
            map.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { entry1 });

            OdbcOrderChargeLoader testObject = new OdbcOrderChargeLoader();

            OrderEntity order = new OrderEntity();

            testObject.Load(map.Object, order);

            var orderCharge = order.OrderCharges.FirstOrDefault();

            Assert.Equal("INSURANCE", orderCharge.Type);
        }

        [Fact]
        public void Load_WhenChargeDisplayNameIsUnknown_SetsChargeTypeToAdjust()
        {
            ShipWorksOdbcMappableField shipworksField = new ShipWorksOdbcMappableField(OrderChargeFields.Amount, "Foo");
            shipworksField.LoadValue(123);
            ExternalOdbcMappableField externalField = new ExternalOdbcMappableField(new OdbcTable("Order"), new OdbcColumn("Tax Column"));
            OdbcFieldMapEntry entry1 = new OdbcFieldMapEntry(shipworksField, externalField);

            Mock<IOdbcFieldMap> map = mock.Mock<IOdbcFieldMap>();
            map.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { entry1 });

            OdbcOrderChargeLoader testObject = new OdbcOrderChargeLoader();

            OrderEntity order = new OrderEntity();

            testObject.Load(map.Object, order);

            var orderCharge = order.OrderCharges.FirstOrDefault();

            Assert.Equal("ADJUST", orderCharge.Type);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
