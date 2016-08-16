using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Response
{
    public class FedExGlobalShipAddressResponseTest
    {
        private FedExGlobalShipAddressResponse testObject;

        private SearchLocationsReply reply;
        private Mock<CarrierRequest> carrierRequest;

        public FedExGlobalShipAddressResponseTest()
        {
            reply = new SearchLocationsReply()
            {
                HighestSeverity = NotificationSeverityType.SUCCESS,
                ResultsReturned = "2",
                AddressToLocationRelationships = new[]
                {
                    new AddressToLocationRelationshipDetail()
                    {
                        DistanceAndLocationDetails = new []
                        {
                            new DistanceAndLocationDetail(),
                            new DistanceAndLocationDetail()
                        }
                    }
                }
            };

            carrierRequest = new Mock<CarrierRequest>(null, null);

            testObject = new FedExGlobalShipAddressResponse(reply, carrierRequest.Object);
        }

        [Fact]
        public void Process_ErrorThrown_ErrorInReply()
        {
            reply.Notifications = new[]
            {
               new Notification()
               {
                   Message = "message"
               }
            };
            reply.HighestSeverity = NotificationSeverityType.FAILURE;

            Assert.Throws<FedExApiCarrierException>(() => testObject.Process());
        }

        [Fact]
        public void Process_ErrorThrown_NoLocationFound()
        {
            reply.ResultsReturned = "0";

            Assert.Throws<CarrierException>(() => testObject.Process());
        }

        [Fact]
        public void Process_TwoAddressesReturned()
        {
            testObject.Process();

            Assert.Equal(2, testObject.DistanceAndLocationDetails.Count());
        }
    }
}
