using System;
using System.Linq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Xunit;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Track;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.Tracking;
using ShipWorks.Shipping.Tracking;

namespace ShipWorks.Tests.Shipping.Carriers.OnTrac.Tracking
{
    public class OnTracTrackingRequestTest
    {
        Mock<IHttpResponseReader> mockedHttpResponseReader;

        Mock<IApiLogEntry> mockedLogger;

        Mock<HttpVariableRequestSubmitter> mockedSubmitter;

        OnTracTrackedShipment testObject;

        TrackingShipmentList validOnTracResponse;

        public OnTracTrackingRequestTest()
        {
            //Setup mock object that holds response from request
            mockedHttpResponseReader = new Mock<IHttpResponseReader>();

            //Setup mock Request Submitter
            mockedSubmitter = new Mock<HttpVariableRequestSubmitter>();
            mockedSubmitter.Setup(f => f.GetResponse()).Returns(mockedHttpResponseReader.Object);

            //Setup Logger
            mockedLogger = new Mock<IApiLogEntry>();

            Mock<ILogEntryFactory> mockedLogFactory = new Mock<ILogEntryFactory>();
            mockedLogFactory
                .Setup(f => f.GetLogEntry(It.IsAny<ApiLogSource>(), It.IsAny<string>(), It.IsAny<LogActionType>()))
                .Returns(mockedLogger.Object);

            //Create Actual OnTracTrackingRequest with mock submitter and mock logger
            testObject = new OnTracTrackedShipment(37, "testpass", mockedSubmitter.Object, mockedLogFactory.Object);

            validOnTracResponse = new TrackingShipmentList
            {
                Shipments = new[]
                {
                    new TrackingShipment
                    {
                        Delivered = true,
                        Exp_Del_Date = DateTime.Parse("1/2/2012"),
                        Events = new[]
                        {
                            new Event
                            {
                                Description = "First Event Desc",
                                EventTime = DateTime.Parse("1/1/2012 2:30AM"),
                                City = "First City",
                                State = "MO",
                                Zip = "63102"
                            },
                            new Event
                            {
                                Description = "second Event Desc",
                                EventTime = DateTime.Parse("1/1/2011 2:30AM"),
                                City = "Second City",
                                State = "IL",
                                Zip = "60035"
                            }
                        }
                    }
                }
            };
        }

        [Fact]
        public void RequestTracking_ResultDeserializedProperly_WhenParametersAndResultsAreValid()
        {
            var trackingResult = RunSuccessfullRequestTracking();

            //Verify the xml deseralized correctly
            Assert.True(trackingResult.Details.Count == 2);
            Assert.Equal(trackingResult.Details.First().Activity, "First Event Desc");
            Assert.Equal(
                trackingResult.Summary,
                "<b>First Event Desc</b> on 1/01/2012 2:30 AM ");
        }

        [Fact]
        public void RequestTracking_RequestLogged_WhenParametersAndResultsAreValid()
        {
            RunSuccessfullRequestTracking();

            mockedLogger.Verify(x => x.LogRequest(It.IsAny<HttpRequestSubmitter>()), Times.Once());
        }

        [Fact]
        public void RequestTracking_ResponseLogged_WhenParametersAndResultsAreValid()
        {
            RunSuccessfullRequestTracking();

            mockedLogger.Verify(x => x.LogResponse(It.IsAny<string>()));
        }

        [Fact]
        public void RequestTracking_UriInCorrectFormat_WhenParametersAndResultsAreValid()
        {
            RunSuccessfullRequestTracking();

            //Validate URI was in correct format given the parameters
            Assert.Equal(
                "https://www.shipontrac.net/OnTracTestWebServices/OnTracServices.svc/v2/37/shipments?pw=testpass&tn=123456&requestType=track",
                mockedSubmitter.Object.Uri.ToString());
        }

        [Fact]
        public void RequestTracking_RequestUsingHttpVerbGet_WhenParametersAndResultsAreValid()
        {
            RunSuccessfullRequestTracking();

            Assert.Equal(HttpVerb.Get, mockedSubmitter.Object.Verb);
        }

        TrackingResult RunSuccessfullRequestTracking()
        {
            string validOnTracResponseString = SerializationUtility.SerializeToXml(validOnTracResponse);

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(validOnTracResponseString);

            //Get result
            TrackingResult trackingResult = testObject.GetTrackingResults("123456");
            return trackingResult;
        }

        [Fact]
        public void RequestTracking_ThrowsException_WhenReturnedXmlIsInvalid()
        {
            //fake string response from OnTrac
            const string invalidFakedResponseXml = "blah";

            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(invalidFakedResponseXml);

            //Get result
            Assert.Throws<OnTracException>(() => testObject.GetTrackingResults("123456"));
        }

        [Fact]
        public void RequestTracking_GetsTrackingInfo_WhenPackageNotDelivered()
        {
            validOnTracResponse.Shipments[0].Delivered = false;

            string validOnTracResponseString = SerializationUtility.SerializeToXml(validOnTracResponse);

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(validOnTracResponseString);

            //Get result
            TrackingResult trackingResult = testObject.GetTrackingResults("123456");

            Assert.True(trackingResult.Details.Count == 2);
            Assert.Equal(trackingResult.Details.First().Activity, "First Event Desc");
            Assert.Equal(
                trackingResult.Summary,
                "<b>First Event Desc</b><br/><span style='color: rgb(80, 80, 80);'>Should arrive: 1/02/2012 12:00 AM</span>");
        }

        [Fact]
        public void TrackShipment_GetsTrackingInfoWithSignature_WhenPackageSignedAndDelivered()
        {
            validOnTracResponse.Shipments[0].POD = "Bob";

            string validOnTracResponseString = SerializationUtility.SerializeToXml(validOnTracResponse);

            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(validOnTracResponseString);

            //Get result
            TrackingResult trackingResult = testObject.GetTrackingResults("123456");

            Assert.True(trackingResult.Details.Count == 2);
            Assert.Equal(trackingResult.Details.First().Activity, "First Event Desc");
            Assert.Equal(
                trackingResult.Summary,
                "<b>First Event Desc</b> on 1/01/2012 2:30 AM <br/><span style='color: rgb(80, 80, 80);'>Signed by: Bob</span>");
        }

        [Fact]
        public void TrackShipment_ThrowsException_WhenError()
        {
            validOnTracResponse.Error = "Error!";
            string validOnTracResponseString = SerializationUtility.SerializeToXml(validOnTracResponse);

            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(validOnTracResponseString);

            //Get result
            Assert.Throws<OnTracApiErrorException>(() => testObject.GetTrackingResults("123456"));
        }

        [Fact]
        public void TrackShipment_ThrowsException_WhenNoShipments()
        {
            validOnTracResponse.Shipments = new TrackingShipment[0];
            string validOnTracResponseString = SerializationUtility.SerializeToXml(validOnTracResponse);

            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(validOnTracResponseString);

            //Get result
            Assert.Throws<OnTracException>(() => testObject.GetTrackingResults("123456"));
        }

        [Fact]
        public void TrackShipment_ThrowsException_WhenNoEvents()
        {
            validOnTracResponse.Shipments.First().Events = new Event[0];
            string validOnTracResponseString = SerializationUtility.SerializeToXml(validOnTracResponse);

            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(validOnTracResponseString);

            //Get result
            Assert.Throws<OnTracException>(() => testObject.GetTrackingResults("123456"));
        }
    }
}
