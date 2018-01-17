using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Moq;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests.ShipEngine
{
    public class ShipEnginePartnerWebClientTest : IDisposable
    {
        readonly AutoMock mock;

        public ShipEnginePartnerWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public async Task CreateNewAccount_ReturnsAccountID_FromResponse()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(string.Empty, "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Returns("{\"account_id\":\"1234\"}");

            request.Setup(r => r.GetResponseAsync())
                .ReturnsAsync(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerWebClient>();

            var accountId = await testObject.CreateNewAccount("partnerKey");

            Assert.Equal("1234", accountId);
        }

        [Fact]
        public void CreateNewAccount_PartnerKeyIsAddedToTheRequest()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(string.Empty, "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Returns("{\"account_id\":\"1234\"}");

            request.Setup(r => r.GetResponseAsync())
                .ReturnsAsync(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerWebClient>();

            testObject.CreateNewAccount("partnerKey");

            request.Verify(r => r.Headers.Add("api-key", "partnerKey"), Times.Once);
        }

        [Fact]
        public async Task CreateNewAccount_ThrowsShipEngineException_WhenResponseDoesNotReturnJson()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(string.Empty, "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Returns("not json");

            request.Setup(r => r.GetResponseAsync())
                .ReturnsAsync(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerWebClient>();

            await Assert.ThrowsAsync<ShipEngineException>(() => testObject.CreateNewAccount("partnerKey"));        
        }

        [Fact]
        public async Task CreateNewAccount_ThrowsShipEngineException_WhenWebExceptionEncountered()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(string.Empty, "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Throws(new WebException());

            request.Setup(r => r.GetResponseAsync())
                .ReturnsAsync(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerWebClient>();

            await Assert.ThrowsAsync<ShipEngineException>(() => testObject.CreateNewAccount("partnerKey"));
        }


        [Fact]
        public async Task GetApiKey_ReturnsApiKey_FromResponse()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Returns("{\"encrypted_api_key\":\"1234\"}");

            request.Setup(r => r.GetResponseAsync())
                .ReturnsAsync(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerWebClient>();

            var apiKey = await testObject.GetApiKey("partnerKey", "accountId");

            Assert.Equal("1234", apiKey);
        }

        [Fact]
        public void GetApiKey_PartnerKeyIsAddedToTheRequest()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Returns("{\"encrypted_api_key\":\"1234\"}");

            request.Setup(r => r.GetResponseAsync())
                .ReturnsAsync(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerWebClient>();

            testObject.GetApiKey("partnerKey", "accountId");

            request.Verify(r => r.Headers.Add("api-key", "partnerKey"), Times.Once);
        }

        [Fact]
        public async Task GetApiKey_ThrowsShipEngineException_WhenResponseDoesNotReturnJson()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Returns("not json");

            request.Setup(r => r.GetResponseAsync())
                .ReturnsAsync(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerWebClient>();

            await Assert.ThrowsAsync<ShipEngineException>(() => testObject.GetApiKey("partnerKey", "accountId"));
        }

        [Fact]
        public async Task GetApiKey_ThrowsShipEngineException_WhenWebExceptionEncountered()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Throws(new WebException());

            request.Setup(r => r.GetResponseAsync())
                .ReturnsAsync(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerWebClient>();

            await Assert.ThrowsAsync<ShipEngineException>(() => testObject.GetApiKey("partnerKey", "accountId"));
        }


        public void Dispose()
        {
            mock.Dispose();
        }

    }
}
