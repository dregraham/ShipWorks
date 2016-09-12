using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Tests.Shared;
using System.Xml;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.OnLineTools
{
    public class UpsApiTrackResponseTest
    {
        private XmlDocument trackingResponse;
        private ShipmentEntity shipment;
        private UpsApiTrackResponse testObject;

        public UpsApiTrackResponseTest()
        {
            string trackingResponseString = EmbeddedResourceHelper.GetEmbeddedResourceString(
                "ShipWorks.Tests.Shipping.Carriers.UPS.OnLineTools.TrackResponse.xml");

            trackingResponse = new XmlDocument();
            trackingResponse.LoadXml(trackingResponseString);

            shipment = new ShipmentEntity() {OriginCountryCode = "US"};

            testObject = new UpsApiTrackResponse();

        }

        [Fact]
        public void TrackingResult_ContainsDeliveredStatus_WhenStatusIsDelivered()
        {
            testObject.LoadResponse(trackingResponse, shipment);

            Assert.Contains("Delivered", testObject.TrackingResult.Summary);
        }

        [Fact]
        public void TrackingResult_ContainsDeliveryDate_WhenStatusIsDelivered()
        {
            testObject.LoadResponse(trackingResponse, shipment);

            Assert.Contains("on 6/10/2010 06:30:00 AM", testObject.TrackingResult.Summary);
        }

        [Fact]
        public void TrackingResult_ContainsSignedBy_WhenStatusIsDelivered()
        {
            testObject.LoadResponse(trackingResponse, shipment);

            Assert.Contains("Signed by: Helen Smith", testObject.TrackingResult.Summary);
        }

        [Fact]
        public void TrackingResult_ContainsTwoActivityNodes_WhenResponseHasTwoActivities()
        {
            testObject.LoadResponse(trackingResponse, shipment);

            Assert.Equal(2, testObject.TrackingResult.Details.Count);
        }
    }
}