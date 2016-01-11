using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.None;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.None
{
    public class NoneLabelServiceTest
    {
        readonly AutoMock mock;

        public NoneLabelServiceTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void NoneLabelService_Throws_ShippingException_OnProcess()
        {
            var testObject = mock.Create<NoneLabelService>();
            ShippingException ex = Assert.Throws<ShippingException>(() => testObject.Create(null));

            Assert.Equal("No carrier is selected for the shipment.", ex.Message);
        }

        [Fact]
        public void NoneLabelService_DoesNotThrow_OnVoid()
        {
            var testObject = mock.Create<NoneLabelService>();
            testObject.Void(null);
        }
    }
}