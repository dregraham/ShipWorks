using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcOrderPaymentLoaderTest
    {
        [Fact]
        public void Load_AddsPaymentDetailsToOrder_WhenMapContainsPaymentFields()
        {
            using (var mock = AutoMock.GetLoose())
            {
                OdbcFieldMap map = mock.Create<OdbcFieldMap>();
                var shipworksfield = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksfield.Setup(e => e.GetQualifiedName()).Returns("OrderPaymentDetail.Value");
                shipworksfield.Setup(e => e.DisplayName).Returns("Payment Method");
                shipworksfield.Setup(e => e.Value).Returns("Credit Card");
                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ShipWorksField).Returns(shipworksfield.Object);
                map.AddEntry(mapEntry.Object);
                OrderEntity order = new OrderEntity();
                OdbcOrderPaymentLoader testObject = new OdbcOrderPaymentLoader();

                testObject.Load(map, order);

                Assert.Equal("Payment Method", order.OrderPaymentDetails.First().Label);
                Assert.Equal("Credit Card", order.OrderPaymentDetails.First().Value);
            }
        }

        [Fact]
        public void Load_AddsMultiplePaymentDetailsToOrder_WhenMapContainsMultiplePaymentFields()
        {
            using (var mock = AutoMock.GetLoose())
            {
                OdbcFieldMap map = mock.Create<OdbcFieldMap>();

                var shipworksfield = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksfield.Setup(e => e.GetQualifiedName()).Returns("OrderPaymentDetail.Value");
                shipworksfield.Setup(e => e.DisplayName).Returns("Payment Method");
                shipworksfield.Setup(e => e.Value).Returns("Credit Card");
                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ShipWorksField).Returns(shipworksfield.Object);

                var shipworksfield2 = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksfield2.Setup(e => e.GetQualifiedName()).Returns("OrderPaymentDetail.Value");
                shipworksfield2.Setup(e => e.DisplayName).Returns("Credit Card Type");
                shipworksfield2.Setup(e => e.Value).Returns("Visa");
                var mapEntry2 = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry2.Setup(e => e.ShipWorksField).Returns(shipworksfield2.Object);

                map.AddEntry(mapEntry.Object);
                map.AddEntry(mapEntry2.Object);
                OrderEntity order = new OrderEntity();
                OdbcOrderPaymentLoader testObject = new OdbcOrderPaymentLoader();

                testObject.Load(map, order);

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
                OdbcFieldMap map = mock.Create<OdbcFieldMap>();
                var shipworksfield = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksfield.Setup(e => e.GetQualifiedName()).Returns("Blah");
                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ShipWorksField).Returns(shipworksfield.Object);
                map.AddEntry(mapEntry.Object);
                OrderEntity order = new OrderEntity();
                OdbcOrderPaymentLoader testObject = new OdbcOrderPaymentLoader();

                testObject.Load(map, order);

                Assert.Equal(0, order.OrderPaymentDetails.Count);
            }
        }
    }
}