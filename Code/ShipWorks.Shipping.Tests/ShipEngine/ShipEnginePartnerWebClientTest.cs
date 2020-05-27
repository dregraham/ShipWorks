using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Moq;
using ShipWorks.ApplicationCore.Licensing.WebClientEnvironments;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.ShipEngine
{
    public class ShipEnginePartnerWebClientTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly WebClientEnvironmentFactory webClientEnvironmentFactory;

        public ShipEnginePartnerWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            webClientEnvironmentFactory = mock.Create<WebClientEnvironmentFactory>();
            webClientEnvironmentFactory.SelectedEnvironment = new WebClientEnvironment()
            {
                ProxyUrl = "https://proxy.hub.shipworks.com/"
            };
        }

        [Fact]
        public async Task CreateNewAccount_ReturnsAccountID_FromResponse()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Returns(@"{
                              'api_key': {
                                'encrypted_api_key': '1234',
                                'created_at': '2017-12-04T23:06:39.197Z',
                                'description': 'Account API Key',
                                'account_id': 262088,
                                'api_key_id': 4069
                              },
                              'account_id': 200089,
                              'external_account_id': '819748723192',
                              'created_at': '2017-12-04T23:06:39.197Z',
                              'modified_at': '2017-12-04T23:06:39.197Z',
  
                              'active': true
                            }");

            request.Setup(r => r.GetResponseAsync())
                .ReturnsAsync(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerWebClient>(new TypedParameter(typeof(WebClientEnvironmentFactory), webClientEnvironmentFactory));

            var accountId = await testObject.CreateNewAccount();

            Assert.Equal("1234", accountId);
        }

        [Fact]
        public async Task CreateNewAccount_OriginalRequestUrlIsAddedToTheRequest()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Returns(@"{
                              'api_key': {
                                'encrypted_api_key': '1234',
                                'created_at': '2017-12-04T23:06:39.197Z',
                                'description': 'Account API Key',
                                'account_id': 262088,
                                'api_key_id': 4069
                              },
                              'account_id': 200089,
                              'external_account_id': '819748723192',
                              'created_at': '2017-12-04T23:06:39.197Z',
                              'modified_at': '2017-12-04T23:06:39.197Z',
  
                              'active': true
                            }");

            request.Setup(r => r.GetResponseAsync())
                .ReturnsAsync(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerWebClient>(new TypedParameter(typeof(WebClientEnvironmentFactory), webClientEnvironmentFactory));

            await testObject.CreateNewAccount();

            request.Verify(r => r.Headers.Add("SW-originalRequestUrl", "https://api.shipengine.com/v1/partners/accounts"), Times.Once);
        }

        [Fact]
        public async Task CreateNewAccount_ThrowsShipEngineException_WhenResponseDoesNotReturnJson()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Returns("not json");

            request.Setup(r => r.GetResponseAsync())
                .ReturnsAsync(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerWebClient>(new TypedParameter(typeof(WebClientEnvironmentFactory), webClientEnvironmentFactory));

            await Assert.ThrowsAsync<ShipEngineException>(() => testObject.CreateNewAccount());        
        }

        [Fact]
        public async Task CreateNewAccount_ThrowsShipEngineException_WhenWebExceptionEncountered()
        {
            var request = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult())
                .Throws(new WebException());

            request.Setup(r => r.GetResponseAsync())
                .ReturnsAsync(responseReader.Object);

            var testObject = mock.Create<ShipEnginePartnerWebClient>(new TypedParameter(typeof(WebClientEnvironmentFactory), webClientEnvironmentFactory));

            await Assert.ThrowsAsync<ShipEngineException>(() => testObject.CreateNewAccount());
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}
