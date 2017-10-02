using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Moq;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests.ShipEngine
{
    public class ShipEnginePartnerClientTest : IDisposable
    {
        readonly AutoMock mock;

        public ShipEnginePartnerClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void CreateNewAccount_ReturnsAccountID_FromResponse()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(string.Empty, "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Returns("{\"account_id\":\"1234\"}");

            request.Setup(r => r.GetResponse())
                .Returns(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerClient>();

            var accountId = testObject.CreateNewAccount("partnerKey");

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

            request.Setup(r => r.GetResponse())
                .Returns(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerClient>();

            testObject.CreateNewAccount("partnerKey");

            request.Verify(r => r.Headers.Add("api-key", "partnerKey"), Times.Once);
        }

        [Fact]
        public void CreateNewAccount_ThrowsShipEngineException_WhenResponseDoesNotReturnJson()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(string.Empty, "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Returns("not json");

            request.Setup(r => r.GetResponse())
                .Returns(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerClient>();

            Assert.Throws<ShipEngineException>(()=>testObject.CreateNewAccount("partnerKey"));           
        }

        [Fact]
        public void CreateNewAccount_ThrowsShipEngineException_WhenWebExceptionEncountered()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(string.Empty, "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Throws(new WebException());

            request.Setup(r => r.GetResponse())
                .Returns(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerClient>();

            Assert.Throws<ShipEngineException>(()=>testObject.CreateNewAccount("partnerKey"));
        }


        [Fact]
        public void GetApiKey_ReturnsApiKey_FromResponse()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Returns("{\"encrypted_api_key\":\"1234\"}");

            request.Setup(r => r.GetResponse())
                .Returns(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerClient>();

            var apiKey = testObject.GetApiKey("partnerKey", "accountId");

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

            request.Setup(r => r.GetResponse())
                .Returns(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerClient>();

            testObject.GetApiKey("partnerKey", "accountId");

            request.Verify(r => r.Headers.Add("api-key", "partnerKey"), Times.Once);
        }

        [Fact]
        public void GetApiKey_ThrowsShipEngineException_WhenResponseDoesNotReturnJson()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Returns("not json");

            request.Setup(r => r.GetResponse())
                .Returns(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerClient>();

            Assert.Throws<ShipEngineException>(() => testObject.GetApiKey("partnerKey", "accountId"));
        }

        [Fact]
        public void GetApiKey_ThrowsShipEngineException_WhenWebExceptionEncountered()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Throws(new WebException());

            request.Setup(r => r.GetResponse())
                .Returns(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerClient>();

            Assert.Throws<ShipEngineException>(() => testObject.GetApiKey("partnerKey", "accountId"));
        }


        public void Dispose()
        {
            mock.Dispose();
        }

    }
}
