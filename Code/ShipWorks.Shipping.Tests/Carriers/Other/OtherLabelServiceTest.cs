using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Other;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Other
{
    public class OtherLabelServiceTest
    {
        readonly AutoMock mock;

        public OtherLabelServiceTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void OtherLabelService_Throws_ShippingException_WithEmptyCarrier()
        {
            var shipment = new ShipmentEntity() { Other = new OtherShipmentEntity() { Carrier = string.Empty } };
            var testObject = mock.Create<OtherLabelService>();
            ShippingException ex = Assert.Throws<ShippingException>(() => testObject.Create(shipment));

            Assert.Equal("No carrier is specified.", ex.Message);
        }

        [Fact]
        public void OtherLabelService_Throws_ShippingException_WithEmptyService()
        {
            var shipment = new ShipmentEntity() { Other = new OtherShipmentEntity() { Carrier = "USPS", Service = string.Empty} };
            var testObject = mock.Create<OtherLabelService>();
            ShippingException ex = Assert.Throws<ShippingException>(() => testObject.Create(shipment));

            Assert.Equal("No service is specified.", ex.Message);
        }

        [Fact]
        public void OtherLabelService_DoesNotThrow_OnVoid()
        {
            var testObject = mock.Create<OtherLabelService>();
            testObject.Void(null);
        }
    }
}