using System;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net.RestSharp;
using Interapptive.Shared.Utility;
using Moq;
using RestSharp;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.ShipEngine
{
    public class ShipEngineWebClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IRestClientFactory> restClientFactory;
        private readonly Mock<IRestClient> restClient;
        private readonly Mock<IRestRequestFactory> restRequestFactory;
        private readonly Mock<IRestRequest> restRequest;

        public ShipEngineWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            restClient = mock.Mock<IRestClient>();
            restRequest = mock.Mock<IRestRequest>();

            restClientFactory = mock.Mock<IRestClientFactory>();
            restRequestFactory = mock.Mock<IRestRequestFactory>();

            restClientFactory.Setup(x => x.Create(It.IsAny<string>())).Returns(restClient.Object);
            restRequestFactory.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<Method>())).Returns((string url, Method method) =>
            {
                restRequest.Setup(x => x.Resource).Returns(url);
                restRequest.Setup(x => x.Method).Returns(method);
                return restRequest.Object;
            });
        }

        [Fact]
        public async Task ConnectDHLAccount_ReturnsFailureWhenConnectAccountReturnsError()
        {
            var error = "{\r\n  \"request_id\": \"c3d0f656-1ec8-4f1f-935c-25599e1e8d2a\",\r\n  \"errors\": [\r\n    {\r\n      \"error_code\": \"\",\r\n      \"message\": \"\'account_number\' must be 9 characters in length. You entered 3 characters.\"\r\n    }\r\n  ]\r\n}";

            restClient.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).ReturnsAsync(new RestResponse() { Content = error, StatusCode = HttpStatusCode.BadRequest });

            var testObject = mock.Create<ShipEngineWebClient>();

            GenericResult<string> result = await testObject.ConnectDhlAccount("abcd");

            Assert.False(result.Success);
            Assert.Equal("'account_number' must be 9 characters in length. You entered 3 characters.", result.Message);
        }

        [Fact]
        public void ConnectDHLAccount_CallsShipEngineWithAccountNumber()
        {
            var testObject = mock.Create<ShipEngineWebClient>();

            testObject.ConnectDhlAccount("AccountNumber");

            restRequest.Verify(x => x.AddJsonBody(It.Is<object>(y => (y as DHLExpressAccountInformationDTO).AccountNumber == "AccountNumber")));
        }

        [Fact]
        public async Task RateShipment_ThrowsShippingException_WhenRateShipmentReturnsError()
        {
            string error =
                "{\r\n  \"request_id\": \"c3d0f656-1ec8-4f1f-935c-25599e1e8d2a\",\r\n  \"errors\": [\r\n    {\r\n      \"error_code\": \"\",\r\n      \"message\": \"Rating Error\"\r\n    }\r\n  ]\r\n}";

            restClient.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).ReturnsAsync(new RestResponse() { Content = error, StatusCode = HttpStatusCode.BadRequest });

            var testObject = mock.Create<ShipEngineWebClient>();

            await Assert.ThrowsAsync<ShipEngineException>(() => testObject.RateShipment(new RateShipmentRequest(), ApiLogSource.ShipEngine));
        }

        [Fact]
        public async Task PurchaseLabelRequest_ThrowsShippingException_WhenRateShipmentThrowsException()
        {
            string error =
                "{\r\n  \"request_id\": \"c3d0f656-1ec8-4f1f-935c-25599e1e8d2a\",\r\n  \"errors\": [\r\n    {\r\n      \"error_code\": \"\",\r\n      \"message\": \"Rating Error\"\r\n    }\r\n  ]\r\n}";

            restClient.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).ReturnsAsync(new RestResponse() { Content = error, StatusCode = HttpStatusCode.BadRequest });

            var testObject = mock.Create<ShipEngineWebClient>();

            await Assert.ThrowsAsync<ShipEngineException>(() => testObject.PurchaseLabel(new PurchaseLabelRequest(), ApiLogSource.ShipEngine, new TelemetricResult<IDownloadedLabelData>("testing")));
        }

        [Fact]
        public void Download_ThrowsShipEngineExceptionWithLogSource_WhenWebRequestThrowsException()
        {
            var testObject = mock.Create<ShipEngineWebClient>();

            ShipEngineException ex = Assert.Throws<ShipEngineException>(() => testObject.Download(null));

            Assert.Equal("An error occured while attempting to download resource.", ex.Message);
        }

        [Fact]
        public async Task ConnectAsendia_ReturnsFailureWhenConnectAccountThrowsException()
        {
            string error =
                "{\r\n  \"request_id\": \"c3d0f656-1ec8-4f1f-935c-25599e1e8d2a\",\r\n  \"errors\": [\r\n    {\r\n      \"error_code\": \"\",\r\n      \"message\": \"\'account_number\' must be 9 characters in length. You entered 3 characters.\"\r\n    }\r\n  ]\r\n}";

            restClient.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).ReturnsAsync(new RestResponse() { Content = error, StatusCode = HttpStatusCode.BadRequest });

            var testObject = mock.Create<ShipEngineWebClient>();

            GenericResult<string> result = await testObject.ConnectAsendiaAccount("abcd", "username", "password", "foo", "ord");

            Assert.False(result.Success);
            Assert.Equal("'account_number' must be 9 characters in length. You entered 3 characters.", result.Message);
        }

        [Fact]
        public async Task ConnectAsendia_ReturnsFriendlyErrorWhenConnectAccountThrows530Error()
        {
            string error =
                "{\r\n  \"request_id\": \"c3d0f656-1ec8-4f1f-935c-25599e1e8d2a\",\r\n  \"errors\": [\r\n    {\r\n      \"error_code\": \"\",\r\n      \"message\": \"(530) Not logged in\"\r\n    }\r\n  ]\r\n}";

            restClient.Setup(x => x.ExecuteTaskAsync(It.IsAny<IRestRequest>())).ReturnsAsync(new RestResponse() { Content = error, StatusCode = HttpStatusCode.BadRequest });

            var testObject = mock.Create<ShipEngineWebClient>();

            GenericResult<string> result = await testObject.ConnectAsendiaAccount("abcd", "username", "password", "foo", "ord");

            Assert.False(result.Success);
            Assert.Equal("Unable to connect to Asendia. Please check your account information and try again.", result.Message);
        }

        [Fact]
        public void ConnectAsendia_CallsShipEngineWithAccountNumber()
        {
            var testObject = mock.Create<ShipEngineWebClient>();

            testObject.ConnectAsendiaAccount("AccountNumber", "username", "password", "foo", "ord");

            restRequest.Verify(x => x.AddJsonBody(It.Is<object>(y => (y as AsendiaAccountInformationDTO).AccountNumber == "AccountNumber")));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
