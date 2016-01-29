using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Tracking.Response.Manipulators
{
    public class FedExTrackingResponseManipulatorTest
    {
        private FedExTrackingResponseManipulator testObject;

        FedExTrackingResponse fedExTrackingResponse;
        TrackReply nativeResponse;
        private Mock<CarrierRequest> carrierRequest;
        private ShipmentEntity shipment;

        public FedExTrackingResponseManipulatorTest()
        {
            nativeResponse = FedExTrackingUtilities.BuildSuccessTrackReply();
            shipment = new ShipmentEntity
            {
                OriginCountryCode = "US",
                ShipCountryCode = "US"
            };

            carrierRequest = new Mock<CarrierRequest>(null, null);
            fedExTrackingResponse = new FedExTrackingResponse(null, shipment, nativeResponse, carrierRequest.Object);

            testObject = new FedExTrackingResponseManipulator();
        }

        [Fact]
        public void Manipulate_ThrowsFedExApiException_WhenHighestSeverityIsError()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.ERROR;

            Assert.Throws<FedExApiCarrierException>(() => testObject.Manipulate(fedExTrackingResponse));
        }

        [Fact]
        public void Manipulate_ThrowsFedExApiException_WhenHighestSeverityIsFailure()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.FAILURE;

            Assert.Throws<FedExApiCarrierException>(() => testObject.Manipulate(fedExTrackingResponse));
        }

        [Fact]
        public void Manipulate_ResultSummaryIsUnknown_WhenTrackDetailsIsNull()
        {
            nativeResponse.CompletedTrackDetails[0].TrackDetails = null;

            testObject.Manipulate(fedExTrackingResponse);

            Assert.Equal("Unknown", fedExTrackingResponse.TrackingResult.Summary);
        }

        [Fact]
        public void Manipulate_ResultSummaryIsUnknown_WhenTrackDetailsIsEmpty()
        {
            nativeResponse.CompletedTrackDetails[0].TrackDetails = new TrackDetail[0];

            testObject.Manipulate(fedExTrackingResponse);

            Assert.Equal("Unknown", fedExTrackingResponse.TrackingResult.Summary);
        }

        [Fact]
        public void Manipulate_ResultSummaryIsNoTrackingInfoReturned_WhenTrackDetailsStatusDescriptionIsEmpty()
        {
            nativeResponse.CompletedTrackDetails[0].TrackDetails[0].StatusDetail.Description = string.Empty;

            testObject.Manipulate(fedExTrackingResponse);

            Assert.Equal("No tracking information was returned.", fedExTrackingResponse.TrackingResult.Summary);
        }

        [Fact]
        public void Manipulate_LocationContainsCountryName_ForInternationalShipments()
        {
            // Setup the shipment to be international
            shipment.ShipCountryCode = "CA";
            shipment.OriginCountryCode = "US";

            // Change the country code in the native response to Canada
            nativeResponse.CompletedTrackDetails[0].TrackDetails[0].Events[0].Address.CountryCode = "CA";

            testObject.Manipulate(fedExTrackingResponse);

            Assert.True(fedExTrackingResponse.TrackingResult.Details[0].Location.ToLower().Contains("canada"));
        }

        [Fact]
        public void Manipulate_LocationDoesNotContainCountryName_ForDomesticShipments()
        {
            // Setup the shipment to be a domestic Canadian shipment
            shipment.ShipCountryCode = "CA";
            shipment.OriginCountryCode = "CA";

            // Change the country code in the native response to Canada
            nativeResponse.CompletedTrackDetails[0].TrackDetails[0].Events[0].Address.CountryCode = "CA";

            testObject.Manipulate(fedExTrackingResponse);

            Assert.False(fedExTrackingResponse.TrackingResult.Details[0].Location.ToLower().Contains("canada"));
        }

        [Fact]
        public void Manipulate_DetailIsEmpty_WhenTrackingEventsIsNull()
        {
            nativeResponse.CompletedTrackDetails[0].TrackDetails[0].Events = null;

            testObject.Manipulate(fedExTrackingResponse);

            Assert.Equal(0, fedExTrackingResponse.TrackingResult.Details.Count);
        }
    }
}
