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

        [TestInitialize]
        public void Initialize()
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
        [ExpectedException(typeof(FedExApiCarrierException))]
        public void Process_ErrorThrown_ErrorInReply_Test()
        {
            reply.Notifications = new[]
            {
               new Notification()
               {
                   Message = "message"
               }
            };
            reply.HighestSeverity=NotificationSeverityType.FAILURE;

            testObject.Process();
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Process_ErrorThrown_NoLocationFound()
        {
            reply.ResultsReturned = "0";

            testObject.Process();
        }

        [Fact]
        public void Process_TwoAddressesReturned_Test()
        {
            testObject.Process();

            Assert.AreEqual(2,testObject.DistanceAndLocationDetails.Count());
        }
    }
}
