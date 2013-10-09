using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.GlobalShipAddress.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using Moq;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.GlobalShipAddress.Response
{
    [TestClass]
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

        [TestMethod]
        [ExpectedException(typeof(FedExApiException))]
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

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Process_ErrorThrown_NoLocationFound()
        {
            reply.ResultsReturned = "0";

            testObject.Process();
        }

        [TestMethod]
        public void Process_TwoAddressesReturned_Test()
        {
            testObject.Process();

            Assert.AreEqual(2,testObject.DistanceAndLocationDetails.Count());
        }
    }
}
