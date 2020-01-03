using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.OnTrac
{
    public class OnTracShipmentTypeTest : IDisposable
    {
        private AutoMock mock;

        public OnTracShipmentTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData("", true, "")]
        [InlineData("foo", false, "")]
        [InlineData("foo", true, "https://www.ontrac.com/trackingresults.asp?tracking_number=foo")]
        public void GetCarrierTrackingUrl_ReturnsCorrectTrackingUrl(string trackingNumber, bool processed, string expectedUrl)
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                TrackingNumber = trackingNumber,
                Processed = processed
            };

            var testObject = mock.Create<OnTracShipmentType>();
            var trackingUrl = testObject.GetCarrierTrackingUrl(shipment);
            Assert.Equal(expectedUrl, trackingUrl);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
