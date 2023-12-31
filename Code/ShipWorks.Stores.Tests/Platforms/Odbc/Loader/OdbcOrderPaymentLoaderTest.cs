﻿using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Loader
{
    public class OdbcOrderPaymentLoaderTest
    {
        [Fact]
        public void Load_AddsPaymentDetailsToOrder_WhenMapContainsPaymentFields()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var map = mock.Mock<IOdbcFieldMap>();

                var shipworksField = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksField.SetupGet(e => e.QualifiedName).Returns("OrderPaymentDetail.Value");
                shipworksField.Setup(e => e.DisplayName).Returns("Payment Method");
                shipworksField.Setup(e => e.Value).Returns("Credit Card");

                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ShipWorksField).Returns(shipworksField.Object);

                map.Object.AddEntry(mapEntry.Object);
                map.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>(), false)).
                    Returns(new List<IOdbcFieldMapEntry> { mapEntry.Object });

                OdbcOrderPaymentLoader testObject = new OdbcOrderPaymentLoader();
                OrderEntity order = new OrderEntity();

                testObject.Load(map.Object, order);

                Assert.Equal("Payment Method", order.OrderPaymentDetails.First().Label);
                Assert.Equal("Credit Card", order.OrderPaymentDetails.First().Value);
            }
        }

        [Fact]
        public void Load_AddsMultiplePaymentDetailsToOrder_WhenMapContainsMultiplePaymentFields()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var map = mock.Mock<IOdbcFieldMap>();

                var shipworksField = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksField.SetupGet(e => e.QualifiedName).Returns("OrderPaymentDetail.Value");
                shipworksField.Setup(e => e.DisplayName).Returns("Payment Method");
                shipworksField.Setup(e => e.Value).Returns("Credit Card");

                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ShipWorksField).Returns(shipworksField.Object);

                var shipworksfield2 = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksfield2.SetupGet(e => e.QualifiedName).Returns("OrderPaymentDetail.Value");
                shipworksfield2.Setup(e => e.DisplayName).Returns("Credit Card Type");
                shipworksfield2.Setup(e => e.Value).Returns("Visa");

                var mapEntry2 = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry2.Setup(e => e.ShipWorksField).Returns(shipworksfield2.Object);

                map.Object.AddEntry(mapEntry.Object);
                map.Object.AddEntry(mapEntry2.Object);
                map.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>(), false)).
                    Returns(new List<IOdbcFieldMapEntry> { mapEntry.Object, mapEntry2.Object });

                OdbcOrderPaymentLoader testObject = new OdbcOrderPaymentLoader();
                OrderEntity order = new OrderEntity();

                testObject.Load(map.Object, order);

                Assert.NotNull(order.OrderPaymentDetails.Where(x => x.Label == "Payment Method"));
                Assert.NotNull(order.OrderPaymentDetails.Where(x => x.Value == "Credit Card"));
                Assert.NotNull(order.OrderPaymentDetails.Where(x => x.Label == "Credit Card Type"));
                Assert.NotNull(order.OrderPaymentDetails.Where(x => x.Value == "Visa"));
            }
        }

        [Fact]
        public void Load_DoesNotAddPaymentDetailsToOrder_WhenMapDoesNotContainPaymentFields()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var map = mock.Mock<IOdbcFieldMap>();

                var shipworksField = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksField.SetupGet(e => e.QualifiedName).Returns("Blah");

                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ShipWorksField).Returns(shipworksField.Object);

                map.Object.AddEntry(mapEntry.Object);

                OdbcOrderPaymentLoader testObject = new OdbcOrderPaymentLoader();
                OrderEntity order = new OrderEntity();

                testObject.Load(map.Object, order);

                Assert.Empty(order.OrderPaymentDetails);
            }
        }
    }
}