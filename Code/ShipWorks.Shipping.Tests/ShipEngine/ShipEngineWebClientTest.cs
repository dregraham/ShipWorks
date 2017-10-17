using Autofac.Extras.Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ShipEngine.ApiClient.Api;
using ShipEngine.ApiClient.Client;
using ShipEngine.ApiClient.Model;
using Xunit;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Shipping.Tests.ShipEngine
{
    public class ShipEngineWebClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IShipEngineApiKey> apiKey;
        private readonly Mock<IShipEngineApiFactory> shipEngineApiFactory;
        private readonly Mock<ICarrierAccountsApi> accountsApi;
        private readonly Mock<ICarriersApi> carriersApi;
        private readonly Mock<IRatesApi> ratesApi;
        private readonly Mock<ILabelsApi> labelsApi;

        private readonly ShipEngineWebClient testObject;

        public ShipEngineWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            apiKey = mock.Mock<IShipEngineApiKey>();
            apiKey.SetupGet(k => k.Value).Returns("abcd");

            accountsApi = mock.Mock<ICarrierAccountsApi>();
            carriersApi = mock.Mock<ICarriersApi>();
            ratesApi = mock.Mock<IRatesApi>();
            labelsApi = mock.Mock<ILabelsApi>();

            CarrierListResponse carriers =
                new CarrierListResponse(new List<Carrier>()
                {
                    new Carrier(AccountNumber: "1234", CarrierId: "se-12345")
                });

            carriersApi.Setup(c => c.CarriersListAsync(It.IsAny<string>())).Returns(Task.FromResult(carriers));

            shipEngineApiFactory = mock.Mock<IShipEngineApiFactory>();
            shipEngineApiFactory.Setup(c => c.CreateCarrierAccountsApi()).Returns(accountsApi);
            shipEngineApiFactory.Setup(c => c.CreateCarrierApi()).Returns(carriersApi);
            shipEngineApiFactory.Setup(c => c.CreateRatesApi()).Returns(ratesApi);
            shipEngineApiFactory.Setup(c => c.CreateLabelsApi()).Returns(labelsApi);
            
            testObject = mock.Create<ShipEngineWebClient>();
        }

        [Fact]
        public void ConnectDHLAccount_DelegatesToIShipEngineApiKey()
        {
            apiKey.SetupGet(k => k.Value).Returns("");

            testObject.ConnectDhlAccount("abcd");

            apiKey.Verify(i => i.Configure());
        }

        [Fact]
        public void ConnectDHLAccount_DelegatesToIShipEngineApiFactory()
        {
            testObject.ConnectDhlAccount("abcd");

            shipEngineApiFactory.Verify(i => i.CreateCarrierAccountsApi());
        }

        [Fact]
        public async void ConnectDHLAccount_ReturnsFailureWhenConnectAccountThrowsException()
        {
            string error =
                "{\r\n  \"request_id\": \"c3d0f656-1ec8-4f1f-935c-25599e1e8d2a\",\r\n  \"errors\": [\r\n    {\r\n      \"error_code\": \"\",\r\n      \"message\": \"\'account_number\' must be 9 characters in length. You entered 3 characters.\"\r\n    }\r\n  ]\r\n}";

            accountsApi.Setup(a =>
                a.DHLExpressAccountCarrierConnectAccountAsync(It.IsAny<DHLExpressAccountInformationDTO>(),
                    It.IsAny<string>())).Throws(new ApiException(500, "", error));

            GenericResult<string> result = await testObject.ConnectDhlAccount("abcd");
            
            Assert.False(result.Success);
            Assert.Equal("'account_number' must be 9 characters in length. You entered 3 characters.", result.Message);
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
        public void RateShipment_DelegatesToIShipEngineApiKey()
        {
            apiKey.SetupGet(k => k.Value).Returns("");

            testObject.RateShipment(new RateShipmentRequest(), ApiLogSource.ShipEngine);

            apiKey.Verify(i => i.Configure());
        }

        [Fact]
        public void RateShipment_DelegatesToIShipEngineApiFactory()
        {
            testObject.RateShipment(new RateShipmentRequest(), ApiLogSource.ShipEngine);

            shipEngineApiFactory.Verify(i => i.CreateRatesApi());
        }

        [Fact]
        public async void RateShipment_ThrowsShippingException_WhenRateShipmentThrowsException()
        {
            string error =
                "{\r\n  \"request_id\": \"c3d0f656-1ec8-4f1f-935c-25599e1e8d2a\",\r\n  \"errors\": [\r\n    {\r\n      \"error_code\": \"\",\r\n      \"message\": \"Rating Error\"\r\n    }\r\n  ]\r\n}";

            ratesApi.Setup(a =>
                a.RatesRateShipmentAsync(It.IsAny<RateShipmentRequest>(),
                    It.IsAny<string>())).ThrowsAsync(new ApiException(500, "", error));

            await Assert.ThrowsAsync<ShipEngineException>(() => testObject.RateShipment(new RateShipmentRequest(), ApiLogSource.ShipEngine));
        }
        
        [Fact]
        public async void RateShipment_SetsLoggingAction()
        {
            var apiEntry = mock.CreateMock<IApiLogEntry>();
            mock.MockFunc(apiEntry);
            await testObject.RateShipment(new RateShipmentRequest(), ApiLogSource.ShipEngine);

            // This does not work due to a bug in moq
            // https://github.com/moq/moq4/issues/430
            //accountsApi.VerifySet(i => i.Configuration.ApiClient.RequestLogger = apiEntry.Object.LogRequest);
            //accountsApi.VerifySet(i => i.Configuration.ApiClient.ResponseLogger = apiEntry.Object.LogResponse);

            Assert.NotNull(ratesApi.Object.Configuration.ApiClient.ResponseLogger);
            Assert.NotNull(ratesApi.Object.Configuration.ApiClient.RequestLogger);
        }

        [Fact]
        public void PurchaseLabelRequest_DelegatesToIShipEngineApiKey()
        {
            apiKey.SetupGet(k => k.Value).Returns("");

            testObject.PurchaseLabel(new PurchaseLabelRequest(), ApiLogSource.ShipEngine);

            apiKey.Verify(i => i.Configure());
        }

        [Fact]
        public void PurchaseLabelRequest_DelegatesToIShipEngineApiFactory()
        {
            testObject.PurchaseLabel(new PurchaseLabelRequest(), ApiLogSource.ShipEngine);

            shipEngineApiFactory.Verify(i => i.CreateLabelsApi());
        }

        [Fact]
        public async void PurchaseLabelRequest_ThrowsShippingException_WhenRateShipmentThrowsException()
        {
            string error =
                "{\r\n  \"request_id\": \"c3d0f656-1ec8-4f1f-935c-25599e1e8d2a\",\r\n  \"errors\": [\r\n    {\r\n      \"error_code\": \"\",\r\n      \"message\": \"Rating Error\"\r\n    }\r\n  ]\r\n}";

            labelsApi.Setup(a =>
                a.LabelsPurchaseLabelAsync(It.IsAny<PurchaseLabelRequest>(),
                    It.IsAny<string>())).Throws(new ApiException(500, "", error));

            await Assert.ThrowsAsync<ShipEngineException>(() => testObject.PurchaseLabel(new PurchaseLabelRequest(), ApiLogSource.ShipEngine));
        }

        [Fact]
        public void PurchaseLabelRequest_SetsLoggingAction()
        {
            var apiEntry = mock.CreateMock<IApiLogEntry>();
            mock.MockFunc(apiEntry);
            testObject.PurchaseLabel(new PurchaseLabelRequest(), ApiLogSource.ShipEngine);

            // This does not work due to a bug in moq
            // https://github.com/moq/moq4/issues/430
            //accountsApi.VerifySet(i => i.Configuration.ApiClient.RequestLogger = apiEntry.Object.LogRequest);
            //accountsApi.VerifySet(i => i.Configuration.ApiClient.ResponseLogger = apiEntry.Object.LogResponse);

            Assert.NotNull(labelsApi.Object.Configuration.ApiClient.ResponseLogger);
            Assert.NotNull(labelsApi.Object.Configuration.ApiClient.RequestLogger);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
