using Interapptive.Shared.Net;
using Xunit;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.OnTrac.Net;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Shipment;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.Shipment;

namespace ShipWorks.Tests.Shipping.Carriers.OnTrac.Shipment
{
    public class OnTracShipmentRequestTest
    {
        Mock<IHttpRequestSubmitterFactory> mockedHttpRequestSubmitterFactory;

        Mock<IHttpResponseReader> mockedHttpResponseReader;

        Mock<IApiLogEntry> mockedLogger;

        Mock<HttpVariableRequestSubmitter> mockedSubmitter;

        OnTracShipmentRequest testObject;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            OnTracRequest.UseTestServer = true;
        }

        [TestInitialize]
        public void Initialize()
        {
            //Setup mock object that holds response from request
            mockedHttpResponseReader = new Mock<IHttpResponseReader>();

            //Setup mock Request Submitter
            mockedSubmitter = new Mock<HttpVariableRequestSubmitter>();
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
        public void GetNewShipment_GetsShipmentFromOnTrac_WhenParametersAndResultsAreValid_Test()
        {
            var shippingResult = RunSuccessfullGetNewShipment();

            //Check the object deserialized it properly
            Assert.AreEqual("D90000000006295", shippingResult.Tracking);
        }

        [Fact]
        public void GetNewShipment_RequestingUrlIsCorrect_WhenParametersAndResultsAreValid_Test()
        {
            RunSuccessfullGetNewShipment();

            //Validate URI was in correct format given the parameters
            Assert.AreEqual(
                "https://www.shipontrac.net/OnTracTestWebServices/OnTracServices.svc/v2/37/shipments?pw=testpass",
                mockedSubmitter.Object.Uri.ToString());
        }

        [Fact]
        public void GetNewShipment_RequestVerbIsPost_WhenParametersAndResultsAreValid_Test()
        {
            RunSuccessfullGetNewShipment();

            Assert.AreEqual(HttpVerb.Post, mockedSubmitter.Object.Verb);
        }

        [Fact]
        public void GetNewShipment_RequestIsLogged_WhenParametersAndResultsAreValid_Test()
        {
            RunSuccessfullGetNewShipment();

            mockedLogger.Verify(x => x.LogRequest(It.IsAny<HttpRequestSubmitter>()), Times.Once());
        }

        [Fact]
        public void GetNewShipment_ResponseIsLogged_WhenParametersAndResultsAreValid_Test()
        {
            RunSuccessfullGetNewShipment();

            mockedLogger.Verify(x => x.LogResponse(It.IsAny<string>()));
        }

        ShipmentResponse RunSuccessfullGetNewShipment()
        {
            //fake string response from OnTrac
            const string fakedResponseXml =
                "<OnTracShipmentResponse xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Error/><Shipments><Shipment><Tracking>D90000000006295</Tracking><Error/><TransitDays>2</TransitDays><ServiceChrg>30.65</ServiceChrg><FuelChrg>5.21</FuelChrg><TotalChrg>35.86</TotalChrg><TariffChrg>35.86</TariffChrg><Label/><SortCode>SEA</SortCode></Shipment></Shipments></OnTracShipmentResponse>";

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(fakedResponseXml);

            //Get result
            var shippingResult = testObject.ProcessShipment(new ShipmentRequestList());
            return shippingResult;
        }

        [Fact]
        [ExpectedException(typeof(OnTracException))]
        public void GetNewShipment_OnTracError_ReturnedXmlInBadFormat_Test()
        {
            //fake string response from OnTrac
            const string fakedResponseXml =
                "<OnTracShipmenXtResponse xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Error/><Shipments><Shipment><Tracking>D90000000006295</Tracking><Error/><TransitDays>2</TransitDays><ServiceChrg>30.65</ServiceChrg><FuelChrg>5.21</FuelChrg><TotalChrg>35.86</TotalChrg><TariffChrg>35.86</TariffChrg><Label/><SortCode>SEA</SortCode></Shipment></Shipments></OnTracShipmentResponse>";

            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(fakedResponseXml);

            //Get result
            testObject.ProcessShipment(new ShipmentRequestList());
        }
    }
}