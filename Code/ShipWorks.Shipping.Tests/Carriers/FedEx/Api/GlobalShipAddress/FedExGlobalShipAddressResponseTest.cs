using System.Linq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.GlobalShipAddress
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
            var reply = new SearchLocationsReply
            {
                HighestSeverity = NotificationSeverityType.SUCCESS,
                ResultsReturned = "2",
                AddressToLocationRelationships = new[]
                {
                    new AddressToLocationRelationshipDetail
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

        [Fact]
        public void Process_ReturnsSuccess_StoreNumberIsString()
        {
            var reply = new SearchLocationsReply
            {
                HighestSeverity = NotificationSeverityType.SUCCESS,
                ResultsReturned = "1",

                AddressToLocationRelationships = new[]
                {
                    new AddressToLocationRelationshipDetail
                    {
                        DistanceAndLocationDetails = new[]
                        {
                            new DistanceAndLocationDetail
                            {
                                Distance = null,
                                LocationDetail = new LocationDetail
                                {
                                    StoreNumber = "Foo"
                                }

                            }
                        }
                    }
                }
            };
            var testObject = new FedExGlobalShipAddressResponse(reply);
            var result = testObject.Process();

            Assert.True(result.Success);
        }
    }
}
