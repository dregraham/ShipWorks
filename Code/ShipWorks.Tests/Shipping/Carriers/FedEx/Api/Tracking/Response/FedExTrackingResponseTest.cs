using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Tracking.Response
{
    public class FedExTrackingResponseTest
    {
        private FedExTrackingResponse testObject;

        private List<IFedExTrackingResponseManipulator> manipulators;
        private Mock<IFedExTrackingResponseManipulator> mockedShipmentManipulator;
        private Mock<CarrierRequest> carrierRequest;
        TrackReply nativeResponse = null;
        private ShipmentEntity shipment;

        public FedExTrackingResponseTest()
        {
            mockedShipmentManipulator = new Mock<IFedExTrackingResponseManipulator>();
            manipulators = new List<IFedExTrackingResponseManipulator>
            {
                mockedShipmentManipulator.Object
            };

            carrierRequest = new Mock<CarrierRequest>(null, null);

            nativeResponse = new TrackReply
            {
                HighestSeverity = NotificationSeverityType.SUCCESS,
                Notifications = new Notification[]
                {
                    new Notification {
                        Code = "0",
                        Severity = NotificationSeverityType.SUCCESS
                    }
                },
                //TrackDetails = new TrackDetail[1]
                //    {
                //        new TrackDetail
                //            {
                //                TrackingNumber =  "999999999999999",
                //                Events = new TrackEvent[1]
                //                    {
                //                       new TrackEvent
                //                           {
                //                               Timestamp = DateTime.Now.ToUniversalTime(),
                //                               EventType = "AR",
                //                               EventDescription = "Arrived at FedEx location",
                //                               Address = new Address
                //                                   {
                //                                       City = "CHICAGO",
                //                                       StateOrProvinceCode = "IL",
                //                                       PostalCode = "60638",
                //                                       CountryCode = "US",
                //                                       Residential = false
                //                                   }
                //                           }

                //                    }
                //            }

                //    }
            };

            shipment = new ShipmentEntity
            {
                OriginCountryCode = "US",
                ShipCountryCode = "US"
            };

            testObject = new FedExTrackingResponse(manipulators, shipment, nativeResponse, carrierRequest.Object);
        }

        [Fact]
        public void Request_ReturnsRequestProvidedToConstructor()
        {
            Assert.Equal(carrierRequest.Object, testObject.Request);
        }

        [Fact]
        public void NativeResponse_ReturnsRateReplyProvidedToConstructor()
        {
            Assert.Equal(nativeResponse, testObject.NativeResponse);
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenResponseContainsError()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.ERROR;
            Assert.Throws<FedExApiCarrierException>(() => testObject.Process());
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenResponseContainsFailure()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.FAILURE;
            Assert.Throws<FedExApiCarrierException>(() => testObject.Process());
        }
    }
}
