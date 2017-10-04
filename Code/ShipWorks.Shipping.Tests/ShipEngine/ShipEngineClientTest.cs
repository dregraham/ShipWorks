using Autofac.Extras.Moq;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly Mock<ICarriersApi> carriersApi;

        private readonly ShipEngineClient testObject;

        public ShipEngineClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            apiKey = mock.Mock<IShipEngineApiKey>();
            apiKey.SetupGet(k => k.Value).Returns("abcd");

            accountsApi = mock.Mock<ICarrierAccountsApi>();
            carriersApi = mock.Mock<ICarriersApi>();

            CarrierListResponse carriers =
                new CarrierListResponse(new List<Carrier>()
                {
                    new Carrier(AccountNumber: "1234", CarrierId: "se-12345")
                });

            carriersApi.Setup(c => c.CarriersListAsync(It.IsAny<string>())).Returns(Task.FromResult(carriers));

            accountsApiFactory = mock.Mock<IShipEngineCarrierAccountsApiFactory>();
            accountsApiFactory.Setup(c => c.CreateCarrierAccountsApi()).Returns(accountsApi);
            accountsApiFactory.Setup(c => c.CreateCarrierApi()).Returns(carriersApi);

            testObject = mock.Create<ShipEngineClient>();
        }

        [Fact]
        public void ConnectDHLAccount_DelegatesToIShipEngineApiKey()
        {
            apiKey.SetupGet(k => k.Value).Returns("");

            testObject.ConnectDhlAccount("abcd");

            apiKey.Verify(i => i.Configure());
        }

        [Fact]
        public void ConnectDHLAccount_DelegatesToIShipEngineCarrierAccountsApiFactory()
        {
            testObject.ConnectDhlAccount("abcd");

            accountsApiFactory.Verify(i => i.CreateCarrierAccountsApi());
        }

        [Fact]
        public async void ConnectDHLAccount_ReturnsFailureWhenConnectAccountThrowsException()
        {
            accountsApi.Setup(a =>
                a.DHLExpressAccountCarrierConnectAccountAsync(It.IsAny<DHLExpressAccountInformationDTO>(),
                    It.IsAny<string>())).Throws(new Exception("you dun goofed"));

            GenericResult<string> result = await testObject.ConnectDhlAccount("abcd");
            
            Assert.False(result.Success);
            Assert.Equal("you dun goofed", result.Message);
        }

        [Fact]
        public void ConnectDHLAccount_DelegatesToICarrierAccountsApiWithAccountNumber()
        {
            testObject.ConnectDhlAccount("AccountNumber");

            accountsApi.Verify(i =>
                i.DHLExpressAccountCarrierConnectAccountAsync(
                    It.Is<DHLExpressAccountInformationDTO>(d => d.AccountNumber == "AccountNumber"), "abcd"));
        }

        [Fact]
        public void ConnectDHLAccount_SetsLoggingActionsOnCarrierAccountsApi()
        {
            var apiEntry = mock.CreateMock<IApiLogEntry>();
            mock.MockFunc(apiEntry);
            testObject.ConnectDhlAccount("AccountNumber");

            // This does not work due to a bug in moq
            // https://github.com/moq/moq4/issues/430
            //accountsApi.VerifySet(i => i.Configuration.ApiClient.RequestLogger = apiEntry.Object.LogRequest);
            //accountsApi.VerifySet(i => i.Configuration.ApiClient.ResponseLogger = apiEntry.Object.LogResponse);

            Assert.NotNull(accountsApi.Object.Configuration.ApiClient.ResponseLogger);
            Assert.NotNull(accountsApi.Object.Configuration.ApiClient.RequestLogger);
        }

        [Fact]
        public void ConnectDHLAccount_DelegatesToICarriersApiForExistingAccounts()
        {
            testObject.ConnectDhlAccount("1234");

            accountsApi.Verify(a =>
                a.DHLExpressAccountCarrierConnectAccountAsync(It.IsAny<DHLExpressAccountInformationDTO>(),
                    It.IsAny<string>()),Times.Never);

            carriersApi.Verify(c => c.CarriersListAsync("abcd"));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
