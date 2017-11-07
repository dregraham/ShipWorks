using System.Linq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Response
{
    public class FedExGlobalShipAddressResponseTest
    {
        [Fact]
        public void Process_ReturnsError_ErrorInReply()
        {
            var reply = new SearchLocationsReply
            {
                Notifications = new[]
                {
                   new Notification()
                   {
                       Message = "message"
                   }
                },
                HighestSeverity = NotificationSeverityType.FAILURE
            };

            var testObject = new FedExGlobalShipAddressResponse(reply);

            var result = testObject.Process();

            Assert.True(result.Failure);
            Assert.IsAssignableFrom<FedExApiCarrierException>(result.Exception);
        }

        [Fact]
        public void Process_ReturnsError_NoLocationFound()
        {
            var reply = new SearchLocationsReply
            {
                HighestSeverity = NotificationSeverityType.SUCCESS,
                ResultsReturned = "0"
            };
            var testObject = new FedExGlobalShipAddressResponse(reply);

            var result = testObject.Process();

            Assert.True(result.Failure);
            Assert.IsAssignableFrom<CarrierException>(result.Exception);
        }

        [Fact]
        public void Process_TwoAddressesReturned()
        {
            var reply = new SearchLocationsReply()
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
            var testObject = new FedExGlobalShipAddressResponse(reply);

            var result = testObject.Process();

            Assert.Equal(2, result.Value.Count());
        }
    }
}
