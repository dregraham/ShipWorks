﻿using System;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Loader
{
    public class OdbcOrderChargeLoaderTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcOrderChargeLoaderTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void Load_ThrowsArgumentNullException_WithNullMap()
        {
            OdbcOrderChargeLoader testObject = new OdbcOrderChargeLoader();
            Assert.Throws<ArgumentNullException>(() => testObject.Load(null, new OrderEntity()));
        }

        [Fact]
        public void Load_ThrowsArgumentNullException_WhenNullOrderEntity()
        {
            Mock<IOdbcFieldMap> map = mock.Mock<IOdbcFieldMap>();
            OdbcOrderChargeLoader testObject = new OdbcOrderChargeLoader();
            Assert.Throws<ArgumentNullException>(() => testObject.Load(map.Object, null));
        }

        [Fact]
        public void Load_GetsChargeEntriesFromMap()
        {
            ShipWorksOdbcMappableField shipworksField = new ShipWorksOdbcMappableField(OrderChargeFields.Amount, "Tax", OdbcFieldValueResolutionStrategy.Default);
            shipworksField.LoadValue(123);
            ExternalOdbcMappableField externalField = new ExternalOdbcMappableField(new OdbcColumn("Tax Column", "unknown"));
            OdbcFieldMapEntry entry1 = new OdbcFieldMapEntry(shipworksField, externalField);

            Mock<IOdbcFieldMap> map = mock.Mock<IOdbcFieldMap>();
            map.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { entry1 });

            OdbcOrderChargeLoader testObject = new OdbcOrderChargeLoader();

            OrderEntity order = new OrderEntity();

            testObject.Load(map.Object, order);

            map.Verify(m => m.FindEntriesBy(It.IsAny<EntityField2>(), false));
        }

        [Fact]
        public void Load_SetsChargeAmountFromShipWorksFieldValue()
        {
            ShipWorksOdbcMappableField shipworksField = new ShipWorksOdbcMappableField(OrderChargeFields.Amount, "Tax Amount", OdbcFieldValueResolutionStrategy.Default);
            shipworksField.LoadValue(123);
            ExternalOdbcMappableField externalField = new ExternalOdbcMappableField(new OdbcColumn("Tax Column", "unknown"));
            OdbcFieldMapEntry entry1 = new OdbcFieldMapEntry(shipworksField, externalField);

            Mock<IOdbcFieldMap> map = mock.Mock<IOdbcFieldMap>();
            map.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>(), false)).Returns(new[] { entry1 });

            OdbcOrderChargeLoader testObject = new OdbcOrderChargeLoader();

            OrderEntity order = new OrderEntity();

            testObject.Load(map.Object, order);

            var orderCharge = order.OrderCharges.FirstOrDefault();

            Assert.Equal(123, orderCharge.Amount);
        }

        [Fact]
        public void Load_SetsChargeTypeToTax_WhenChargeDisplayNameIsTaxAmount()
        {
            ShipWorksOdbcMappableField shipworksField = new ShipWorksOdbcMappableField(OrderChargeFields.Amount, "Tax Amount", OdbcFieldValueResolutionStrategy.Default);
            shipworksField.LoadValue(123);
            ExternalOdbcMappableField externalField = new ExternalOdbcMappableField(new OdbcColumn("Tax Column", "unknown"));
            OdbcFieldMapEntry entry1 = new OdbcFieldMapEntry(shipworksField, externalField);

            Mock<IOdbcFieldMap> map = mock.Mock<IOdbcFieldMap>();
            map.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>(), false)).Returns(new[] { entry1 });

            OdbcOrderChargeLoader testObject = new OdbcOrderChargeLoader();

            OrderEntity order = new OrderEntity();

            testObject.Load(map.Object, order);

            var orderCharge = order.OrderCharges.FirstOrDefault();

            Assert.Equal("TAX", orderCharge.Type);
        }

        [Fact]
        public void Load_SetsDescriptionToTax_WhenChargeDisplayNameIsTaxAmount()
        {
            ShipWorksOdbcMappableField shipworksField = new ShipWorksOdbcMappableField(OrderChargeFields.Amount, "Tax Amount", OdbcFieldValueResolutionStrategy.Default);
            shipworksField.LoadValue(123);
            ExternalOdbcMappableField externalField = new ExternalOdbcMappableField(new OdbcColumn("Tax Column", "unknown"));
            OdbcFieldMapEntry entry1 = new OdbcFieldMapEntry(shipworksField, externalField);

            Mock<IOdbcFieldMap> map = mock.Mock<IOdbcFieldMap>();
            map.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>(), false)).Returns(new[] { entry1 });

            OdbcOrderChargeLoader testObject = new OdbcOrderChargeLoader();

            OrderEntity order = new OrderEntity();

            testObject.Load(map.Object, order);

            var orderCharge = order.OrderCharges.FirstOrDefault();

            Assert.Equal("Tax", orderCharge.Description);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
