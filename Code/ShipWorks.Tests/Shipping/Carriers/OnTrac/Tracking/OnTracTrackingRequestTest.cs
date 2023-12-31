using System;
using System.Globalization;
using System.Linq;
using CultureAttribute;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.OnTrac.Net;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Track;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.TrackingResponse;
using ShipWorks.Shipping.Tracking;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.OnTrac.Tracking
{
	[UseCulture("en-US")]
	public class OnTracTrackingRequestTest
	{
		private readonly Mock<IHttpResponseReader> mockedHttpResponseReader;

		private readonly Mock<IApiLogEntry> mockedLogger;
		private readonly Mock<HttpVariableRequestSubmitter> mockedSubmitter;
		private readonly OnTracTrackedShipment testObject;
		private readonly OnTracTrackingResult validOnTracResponse;

		public OnTracTrackingRequestTest()
		{
			OnTracRequest.UseTestServer = true;

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

			validOnTracResponse = new OnTracTrackingResult
			{
				Shipments = new[]
				{
					new ShipWorks.Shipping.Carriers.OnTrac.Schemas.TrackingResponse.Shipment
					{
						Delivered = true,
						Exp_Del_Date = DateTime.Parse("1/2/2012", new CultureInfo("en-US")),
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
			Assert.Equal(2, trackingResult.Details.Count);
			Assert.Equal("First Event Desc", trackingResult.Details.First().Activity);
			Assert.Equal("<b>First Event Desc</b> on 1/01/2012 2:30 AM ", trackingResult.Summary);
		}

		[Fact]
		public void RequestTracking_RequestLogged_WhenParametersAndResultsAreValid()
		{
			RunSuccessfullRequestTracking();

			mockedLogger.Verify(x => x.LogRequest(It.IsAny<IHttpRequestSubmitter>()), Times.Once());
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

		private TrackingResult RunSuccessfullRequestTracking()
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

			Assert.Equal(2, trackingResult.Details.Count);
			Assert.Equal("First Event Desc", trackingResult.Details.First().Activity);
			Assert.Equal(
				"<b>First Event Desc</b><br/><span style='color: rgb(80, 80, 80);'>Should arrive: 1/02/2012 12:00 AM</span>",
				trackingResult.Summary);
		}

		[Fact]
		public void TrackShipment_GetsTrackingInfoWithSignature_WhenPackageSignedAndDelivered()
		{
			validOnTracResponse.Shipments[0].POD = "Bob";

			string validOnTracResponseString = SerializationUtility.SerializeToXml(validOnTracResponse);

			mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(validOnTracResponseString);

			//Get result
			TrackingResult trackingResult = testObject.GetTrackingResults("123456");

			Assert.Equal(2, trackingResult.Details.Count);
			Assert.Equal("First Event Desc", trackingResult.Details.First().Activity);
			Assert.Equal(
				"<b>First Event Desc</b> on 1/01/2012 2:30 AM <br/><span style='color: rgb(80, 80, 80);'>Signed by: Bob</span>",
				trackingResult.Summary);
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
			validOnTracResponse.Shipments = new ShipWorks.Shipping.Carriers.OnTrac.Schemas.TrackingResponse.Shipment[0];
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