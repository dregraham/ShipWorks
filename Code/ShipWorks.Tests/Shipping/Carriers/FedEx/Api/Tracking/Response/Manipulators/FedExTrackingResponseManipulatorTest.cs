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

        [TestInitialize]
        public void Initialize()
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
        [ExpectedException(typeof(FedExApiCarrierException))]
        public void Manipulate_ThrowsFedExApiException_WhenHighestSeverityIsError_Test()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.ERROR;
            
            testObject.Manipulate(fedExTrackingResponse);
        }

        [Fact]
        [ExpectedException(typeof(FedExApiCarrierException))]
        public void Manipulate_ThrowsFedExApiException_WhenHighestSeverityIsFailure_Test()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.FAILURE;
            
            testObject.Manipulate(fedExTrackingResponse);
        }

        [Fact]
        public void Manipulate_ResultSummaryIsUnknown_WhenTrackDetailsIsNull_Test()
        {
            nativeResponse.CompletedTrackDetails[0].TrackDetails = null;
            
            testObject.Manipulate(fedExTrackingResponse);
            
            Assert.AreEqual("Unknown", fedExTrackingResponse.TrackingResult.Summary);
        }

        [Fact]
        public void Manipulate_ResultSummaryIsUnknown_WhenTrackDetailsIsEmpty_Test()
        {
            nativeResponse.CompletedTrackDetails[0].TrackDetails = new TrackDetail[0];
            
            testObject.Manipulate(fedExTrackingResponse);
            
            Assert.AreEqual("Unknown", fedExTrackingResponse.TrackingResult.Summary);
        }

        [Fact]
        public void Manipulate_ResultSummaryIsNoTrackingInfoReturned_WhenTrackDetailsStatusDescriptionIsEmpty_Test()
        {
            nativeResponse.CompletedTrackDetails[0].TrackDetails[0].StatusDetail.Description = string.Empty;
            
            testObject.Manipulate(fedExTrackingResponse);

            Assert.AreEqual("No tracking information was returned.", fedExTrackingResponse.TrackingResult.Summary);
        }

        [Fact]
        public void Manipulate_LocationContainsCountryName_ForInternationalShipments_Test()
        {
            // Setup the shipment to be international
            shipment.ShipCountryCode = "CA";
            shipment.OriginCountryCode = "US";

            // Change the country code in the native response to Canada
            nativeResponse.CompletedTrackDetails[0].TrackDetails[0].Events[0].Address.CountryCode = "CA";

            testObject.Manipulate(fedExTrackingResponse);

            Assert.IsTrue(fedExTrackingResponse.TrackingResult.Details[0].Location.ToLower().Contains("canada"));
        }

        [Fact]
        public void Manipulate_LocationDoesNotContainCountryName_ForDomesticShipments_Test()
        {
            // Setup the shipment to be a domestic Canadian shipment
            shipment.ShipCountryCode = "CA";
            shipment.OriginCountryCode = "CA";

            // Change the country code in the native response to Canada
            nativeResponse.CompletedTrackDetails[0].TrackDetails[0].Events[0].Address.CountryCode = "CA";

            testObject.Manipulate(fedExTrackingResponse);

            Assert.IsFalse(fedExTrackingResponse.TrackingResult.Details[0].Location.ToLower().Contains("canada"));
        }

        [Fact]
        public void Manipulate_DetailIsEmpty_WhenTrackingEventsIsNull_Test()
        {
            nativeResponse.CompletedTrackDetails[0].TrackDetails[0].Events = null;

            testObject.Manipulate(fedExTrackingResponse);

            Assert.AreEqual(0, fedExTrackingResponse.TrackingResult.Details.Count);
        }
    }
}
