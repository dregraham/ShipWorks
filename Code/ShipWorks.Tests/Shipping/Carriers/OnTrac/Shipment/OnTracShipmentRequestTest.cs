using System;
using System.Text;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Xunit;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.OnTrac.Net;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Shipment;
using ShipmentRequest = ShipWorks.Shipping.Carriers.OnTrac.Schemas.ShipmentRequest;
using ShipmentResponse = ShipWorks.Shipping.Carriers.OnTrac.Schemas.ShipmentResponse;

namespace ShipWorks.Tests.Shipping.Carriers.OnTrac.Shipment
{
    public class OnTracShipmentRequestTest
    {
        Mock<IHttpRequestSubmitterFactory> mockedHttpRequestSubmitterFactory;

        Mock<IHttpResponseReader> mockedHttpResponseReader;

        Mock<IApiLogEntry> mockedLogger;

        Mock<IHttpRequestSubmitter> mockedSubmitter;

        OnTracShipmentRequest testObject;
        
        static OnTracShipmentRequestTest()
        {
            OnTracRequest.UseTestServer = true;
        }

        public OnTracShipmentRequestTest()
        {
            //Setup mock object that holds response from request
            mockedHttpResponseReader = new Mock<IHttpResponseReader>();

            //Setup mock Request Submitter
            mockedSubmitter = new Mock<IHttpRequestSubmitter>();
            mockedSubmitter.Setup(f => f.GetResponse()).Returns(mockedHttpResponseReader.Object);

            //Setup mock HttpRequestSubmitterFactory
            mockedHttpRequestSubmitterFactory = new Mock<IHttpRequestSubmitterFactory>();

            mockedHttpRequestSubmitterFactory
                .Setup(factory => factory.GetHttpBinaryPostRequestSubmitter(It.IsAny<byte[]>()))
                .Returns(mockedSubmitter.Object);

            //Setup Logger           
            mockedLogger = new Mock<IApiLogEntry>();

            Mock<ILogEntryFactory> mockedLogFactory = new Mock<ILogEntryFactory>();
            mockedLogFactory
                .Setup(f => f.GetLogEntry(It.IsAny<ApiLogSource>(), It.IsAny<string>(), It.IsAny<LogActionType>()))
                .Returns(mockedLogger.Object);

            //Create Actual OnTracTrackingRequest with mock submitter and mock logger
            testObject = new OnTracShipmentRequest(
                37, "testpass", mockedHttpRequestSubmitterFactory.Object, mockedLogFactory.Object);
        }

        [Fact]
        public void GetNewShipment_GetsShipmentFromOnTrac_WhenParametersAndResultsAreValid()
        {
            var shippingResult = RunSuccessfullGetNewShipment();

            //Check the object deserialized it properly
            Assert.Equal("D90000000006295", shippingResult.Tracking);
        }

        [Fact]
        public void GetNewShipment_RequestingUrlIsCorrect_WhenParametersAndResultsAreValid()
        {
            RunSuccessfullGetNewShipment();

            //Validate URI was in correct format given the parameters
            string expectedUri = "https://www.shipontrac.net/OnTracTestWebServices/OnTracServices.svc/v2/37/shipments?pw=testpass";
            mockedSubmitter.VerifySet(s=>s.Uri = new Uri(expectedUri));
        }

        [Fact]
        public void GetNewShipment_HttpBinaryPostRequestSubmitterReqested_WhenParametersAndResultsAreValid()
        {
            RunSuccessfullGetNewShipment();

            mockedHttpRequestSubmitterFactory.Verify(f=>f.GetHttpBinaryPostRequestSubmitter(It.IsAny<byte[]>()));
        }

        [Fact]
        public void GetNewShipment_RequestIsLogged_WhenParametersAndResultsAreValid()
        {
            RunSuccessfullGetNewShipment();

            mockedLogger.Verify(x => x.LogRequest(It.IsAny<IHttpRequestSubmitter>()), Times.Once());
        }

        [Fact]
        public void GetNewShipment_ResponseIsLogged_WhenParametersAndResultsAreValid()
        {
            RunSuccessfullGetNewShipment();

            mockedLogger.Verify(x => x.LogResponse(It.IsAny<string>()));
        }

        ShipmentResponse.Shipment RunSuccessfullGetNewShipment()
        {
            //fake string response from OnTrac
            const string fakedResponseXml =
                "<OnTracShipmentResponse xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Error/><Shipments><Shipment><Tracking>D90000000006295</Tracking><Error/><TransitDays>2</TransitDays><ServiceChrg>30.65</ServiceChrg><FuelChrg>5.21</FuelChrg><TotalChrg>35.86</TotalChrg><TariffChrg>35.86</TariffChrg><Label/><SortCode>SEA</SortCode></Shipment></Shipments></OnTracShipmentResponse>";

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(fakedResponseXml);

            //Get result
            var shippingResult = testObject.ProcessShipment(new ShipmentRequest.OnTracShipmentRequest(), new TelemetricResult<IDownloadedLabelData>("testing"));
            return shippingResult;
        }

        [Fact]
        public void GetNewShipment_OnTracError_ReturnedXmlInBadFormat()
        {
            //fake string response from OnTrac
            const string fakedResponseXml =
                "<OnTracShipmenXtResponse xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Error/><Shipments><Shipment><Tracking>D90000000006295</Tracking><Error/><TransitDays>2</TransitDays><ServiceChrg>30.65</ServiceChrg><FuelChrg>5.21</FuelChrg><TotalChrg>35.86</TotalChrg><TariffChrg>35.86</TariffChrg><Label/><SortCode>SEA</SortCode></Shipment></Shipments></OnTracShipmentResponse>";

            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(fakedResponseXml);

            //Get result
            Assert.Throws<OnTracException>(() => testObject.ProcessShipment(new ShipmentRequest.OnTracShipmentRequest(), new TelemetricResult<IDownloadedLabelData>("testing")));
        }
    }
}