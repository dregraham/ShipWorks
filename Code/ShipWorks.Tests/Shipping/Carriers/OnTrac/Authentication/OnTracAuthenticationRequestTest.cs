using Interapptive.Shared.Net;
using Xunit;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Authentication;

namespace ShipWorks.Tests.Shipping.Carriers.OnTrac.Authentication
{
    public class OnTracAuthenticationRequestTest
    {
        Mock<IHttpResponseReader> mockedHttpResponseReader;

        Mock<IApiLogEntry> mockedLogger;

        Mock<HttpVariableRequestSubmitter> mockedSubmitter;

        OnTracAuthentication testObject;

        const string onTracAuthenticationResponseFormat =
               "<OnTracZipResponse xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Zips/><Error>{0}</Error></OnTracZipResponse>";


        public OnTracAuthenticationRequestTest()
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

            testObject = new OnTracAuthentication(42, "test", mockedSubmitter.Object, mockedLogFactory.Object);
        }

        [Fact]
        public void IsValidUser_RequestIsLogged_ReceiveValidResponseFromOnTrac_Test()
        {
            SuccessfullyValidateUser();

            mockedLogger.Verify(x => x.LogRequest(It.IsAny<HttpRequestSubmitter>()), Times.Once());
        }
        [Fact]
        public void IsValidUser_ResponseIsLogged_ReceiveValidResponseFromOnTrac_Test()
        {
            SuccessfullyValidateUser();

            mockedLogger.Verify(x => x.LogResponse(It.IsAny<string>()));
        }

        [Fact]
        public void IsValidUser_RequestVerbIsGet_ReceiveValidResponseFromOnTrac_Test()
        {
            SuccessfullyValidateUser();

            Assert.Equal(HttpVerb.Get, mockedSubmitter.Object.Verb);
        }

        [Fact]
        public void IsValidUser_ValidUrl_ReceiveValidResponseFromOnTrac_Test()
        {
            SuccessfullyValidateUser();

            //validate url
            Assert.True(
                mockedSubmitter.Object.Uri.ToString().EndsWith("/OnTracServices.svc/v2/42/zips?pw=test&lastupdate=3000-1-1"));
        }

        void SuccessfullyValidateUser()
        {
            string validResponse = string.Format(
                onTracAuthenticationResponseFormat,
                "No Zip Updates Available");

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(validResponse);

            testObject.IsValidUser();
        }

        [Fact]
        public void AuthenticateUser_AuthenticationFail_ReceiveInvalidResponseFromOnTrac_Test()
        {
            const string invalidResponse =
                "<OnTracZXXXipResponse xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Zips/><Error>No Zip Updates Available</Error></OnTracZipResponse>";

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(invalidResponse);

            Assert.Throws<OnTracException>(() => testObject.IsValidUser());
        }
    }
}