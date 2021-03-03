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
        public void IsValidUser_RequestIsLogged_ReceiveValidResponseFromOnTrac()
        {
            SuccessfullyValidateUser();

            mockedLogger.Verify(x => x.LogRequest(It.IsAny<IHttpRequestSubmitter>()), Times.Once());
        }
        [Fact]
        public void IsValidUser_ResponseIsLogged_ReceiveValidResponseFromOnTrac()
        {
            SuccessfullyValidateUser();

            mockedLogger.Verify(x => x.LogResponse(It.IsAny<string>()));
        }

        [Fact]
        public void IsValidUser_RequestVerbIsGet_ReceiveValidResponseFromOnTrac()
        {
            SuccessfullyValidateUser();

            Assert.Equal(HttpVerb.Get, mockedSubmitter.Object.Verb);
        }

        [Fact]
        public void IsValidUser_ValidUrl_ReceiveValidResponseFromOnTrac()
        {
            SuccessfullyValidateUser();

            //validate url
            Assert.True(
                mockedSubmitter.Object.Uri.ToString().EndsWith("/rates?pw=test&packages=ID1;90210;90001;false;0.00;false;0;5;4X3X10;S;0;0"));
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
        public void AuthenticateUser_AuthenticationFail_ReceiveInvalidResponseFromOnTrac()
        {
            const string invalidResponse =
                "<OnTracRateResponse><Shipments><Error>Invalid Username or Password</Error></Shipments></OnTracRateResponse>";

            //Setup mock object that holds response from request
            mockedHttpResponseReader.Setup(x => x.ReadResult()).Returns(invalidResponse);

            Assert.Throws<OnTracApiErrorException>(() => testObject.IsValidUser());
        }
    }
}