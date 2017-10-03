using Autofac.Extras.Moq;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Moq;
using ShipEngine.ApiClient.Api;
using ShipEngine.ApiClient.Model;
using Xunit;

namespace ShipWorks.Shipping.Tests.ShipEngine
{
    public class ShipEngineClientTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<IShipEngineApiKey> apiKey;
        private readonly Mock<IShipEngineCarrierAccountsApiFactory> accountsApiFactory;
        private readonly Mock<ICarrierAccountsApi> accountsApi;

        private readonly ShipEngineClient testObject;

        public ShipEngineClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            apiKey = mock.Mock<IShipEngineApiKey>();
            apiKey.SetupGet(k => k.Value).Returns("abcd");

            accountsApi = mock.Mock<ICarrierAccountsApi>();

            accountsApiFactory = mock.Mock<IShipEngineCarrierAccountsApiFactory>();
            accountsApiFactory.Setup(c => c.CreateCarrierAccountsApi()).Returns(accountsApi);
            
            testObject = mock.Create<ShipEngineClient>();
        }

        [Fact]
        public void ConnectDHLAccount_DelegatesToIShipEngineApiKey()
        {
            testObject.ConnectDHLAccount("abcd");

            apiKey.Verify(i => i.Configure());
        }

        [Fact]
        public void ConnectDHLAccount_DelegatesToIShipEngineCarrierAccountsApiFactory()
        {
            testObject.ConnectDHLAccount("abcd");

            accountsApiFactory.Verify(i => i.CreateCarrierAccountsApi());
        }

        [Fact]
        public async void ConnectDHLAccount_ReturnsFailureWhenConnectAccountThrowsException()
        {
            accountsApi.Setup(a =>
                a.DHLExpressAccountCarrierConnectAccountAsync(It.IsAny<DHLExpressAccountInformationDTO>(),
                    It.IsAny<string>())).Throws(new Exception("you dun goofed"));

            GenericResult<string> result = await testObject.ConnectDHLAccount("abcd");
            
            Assert.False(result.Success);
            Assert.Equal("you dun goofed", result.Message);
        }

        [Fact]
        public void ConnectDHLAccount_DelegatesToICarrierAccountsApiWithAccountNumber()
        {
            testObject.ConnectDHLAccount("AccountNumber");

            accountsApi.Verify(i =>
                i.DHLExpressAccountCarrierConnectAccountAsync(
                    It.Is<DHLExpressAccountInformationDTO>(d => d.AccountNumber == "AccountNumber"), "abcd"));
        }

        [Fact]
        public void ConnectDHLAccount_SetsLoggingActionsOnCarrierAccountsApi()
        {
            var apiEntry = mock.CreateMock<IApiLogEntry>();
            mock.MockFunc(apiEntry);
            testObject.ConnectDHLAccount("AccountNumber");
            
            accountsApi.VerifySet(i => i.Configuration.ApiClient.RequestLogger = apiEntry.Object.LogRequest);
            accountsApi.VerifySet(i => i.Configuration.ApiClient.ResponseLogger = apiEntry.Object.LogResponse);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
