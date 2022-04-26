using Autofac.Extras.Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.ShipEngine.API;
using ShipWorks.Shipping.ShipEngine.DTOs;

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
        private TelemetricResult<IDownloadedLabelData> telemetricResult;

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
                    new Carrier(accountNumber: "1234", carrierId: "se-12345")
                });

            carriersApi.Setup(c => c.CarriersListAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(carriers));

            shipEngineApiFactory = mock.Mock<IShipEngineApiFactory>();
            shipEngineApiFactory.Setup(c => c.CreateCarrierAccountsApi()).Returns(accountsApi);
            shipEngineApiFactory.Setup(c => c.CreateCarrierApi()).Returns(carriersApi);
            shipEngineApiFactory.Setup(c => c.CreateRatesApi()).Returns(ratesApi);
            shipEngineApiFactory.Setup(c => c.CreateLabelsApi()).Returns(labelsApi);

            testObject = mock.Create<ShipEngineWebClient>();

            telemetricResult = new TelemetricResult<IDownloadedLabelData>("testing");
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
        public async Task ConnectDHLAccount_ReturnsFailureWhenConnectAccountThrowsException()
        {
            string error =
                "{\r\n  \"request_id\": \"c3d0f656-1ec8-4f1f-935c-25599e1e8d2a\",\r\n  \"errors\": [\r\n    {\r\n      \"error_code\": \"\",\r\n      \"message\": \"\'account_number\' must be 9 characters in length. You entered 3 characters.\"\r\n    }\r\n  ]\r\n}";

            accountsApi.Setup(a =>
                a.DHLExpressAccountCarrierConnectAccountAsync(It.IsAny<DHLExpressAccountInformationDTO>(),
                    It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception($"500 {error}"));

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
                    It.Is<DHLExpressAccountInformationDTO>(d => d.AccountNumber == "AccountNumber"), "abcd", It.IsAny<string>()));
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
        public async Task RateShipment_ThrowsShippingException_WhenRateShipmentThrowsException()
        {
            string error =
                "{\r\n  \"request_id\": \"c3d0f656-1ec8-4f1f-935c-25599e1e8d2a\",\r\n  \"errors\": [\r\n    {\r\n      \"error_code\": \"\",\r\n      \"message\": \"Rating Error\"\r\n    }\r\n  ]\r\n}";

            ratesApi.Setup(a =>
                a.RatesRateShipmentAsync(It.IsAny<RateShipmentRequest>(),
                    It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception($"500 {error}"));

            await Assert.ThrowsAsync<ShipEngineException>(() => testObject.RateShipment(new RateShipmentRequest(), ApiLogSource.ShipEngine));
        }

        [Fact]
        public void PurchaseLabelRequest_DelegatesToIShipEngineApiKey()
        {
            apiKey.SetupGet(k => k.Value).Returns("");

            testObject.PurchaseLabel(new PurchaseLabelRequest(), ApiLogSource.ShipEngine, telemetricResult);

            apiKey.Verify(i => i.Configure());
        }

        [Fact]
        public void PurchaseLabelRequest_DelegatesToIShipEngineApiFactory()
        {
            testObject.PurchaseLabel(new PurchaseLabelRequest(), ApiLogSource.ShipEngine, telemetricResult);

            shipEngineApiFactory.Verify(i => i.CreateLabelsApi());
        }

        [Fact]
        public async Task PurchaseLabelRequest_ThrowsShippingException_WhenRateShipmentThrowsException()
        {
            string error =
                "{\r\n  \"request_id\": \"c3d0f656-1ec8-4f1f-935c-25599e1e8d2a\",\r\n  \"errors\": [\r\n    {\r\n      \"error_code\": \"\",\r\n      \"message\": \"Rating Error\"\r\n    }\r\n  ]\r\n}";

            labelsApi.Setup(a =>
                a.LabelsPurchaseLabelAsync(It.IsAny<PurchaseLabelRequest>(),
                    It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception($"500 {error}"));

            await Assert.ThrowsAsync<ShipEngineException>(() => testObject.PurchaseLabel(new PurchaseLabelRequest(), ApiLogSource.ShipEngine, telemetricResult));
        }

        [Fact]
        public void Download_ThrowsShipEngineExceptionWithLogSource_WhenWebRequestThrowsException()
        {
            ShipEngineException ex = Assert.Throws<ShipEngineException>(() => testObject.Download(null));

            Assert.Equal("An error occured while attempting to download resource.", ex.Message);
        }

        [Fact]
        public void ConnectAsendia_DelegatesToIShipEngineApiKey()
        {
            apiKey.SetupGet(k => k.Value).Returns("");

            testObject.ConnectAsendiaAccount("abcd", "username", "password");

            apiKey.Verify(i => i.Configure());
        }

        [Fact]
        public void ConnectAsendia_DelegatesToIShipEngineApiFactory()
        {
            testObject.ConnectAsendiaAccount("abcd", "username", "password");

            shipEngineApiFactory.Verify(i => i.CreateCarrierAccountsApi());
        }

        [Fact]
        public async Task ConnectAsendia_ReturnsFailureWhenConnectAccountThrowsException()
        {
            string error =
                "{\r\n  \"request_id\": \"c3d0f656-1ec8-4f1f-935c-25599e1e8d2a\",\r\n  \"errors\": [\r\n    {\r\n      \"error_code\": \"\",\r\n      \"message\": \"\'account_number\' must be 9 characters in length. You entered 3 characters.\"\r\n    }\r\n  ]\r\n}";

            accountsApi.Setup(a =>
                a.AsendiaAccountCarrierConnectAccountAsync(It.IsAny<AsendiaAccountInformationDTO>(),
                    It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception($"500 {error}"));

            GenericResult<string> result = await testObject.ConnectAsendiaAccount("abcd", "username", "password");

            Assert.False(result.Success);
            Assert.Equal("'account_number' must be 9 characters in length. You entered 3 characters.", result.Message);
        }

        [Fact]
        public async Task ConnectAsendia_ReturnsFriendlyErrorWhenConnectAccountThrows530Error()
        {
            string error =
                "{\r\n  \"request_id\": \"c3d0f656-1ec8-4f1f-935c-25599e1e8d2a\",\r\n  \"errors\": [\r\n    {\r\n      \"error_code\": \"\",\r\n      \"message\": \"(530) Not logged in\"\r\n    }\r\n  ]\r\n}";

            accountsApi.Setup(a =>
                a.AsendiaAccountCarrierConnectAccountAsync(It.IsAny<AsendiaAccountInformationDTO>(),
                    It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception($"500 {error}"));

            GenericResult<string> result = await testObject.ConnectAsendiaAccount("abcd", "username", "password");

            Assert.False(result.Success);
            Assert.Equal("Unable to connect to Asendia. Please check your account information and try again.", result.Message);
        }

        [Fact]
        public void ConnectAsendia_DelegatesToICarrierAccountsApiWithAccountNumber()
        {
            testObject.ConnectAsendiaAccount("AccountNumber", "username", "password");

            accountsApi.Verify(i =>
                i.AsendiaAccountCarrierConnectAccountAsync(
                    It.Is<AsendiaAccountInformationDTO>(d => d.AccountNumber == "AccountNumber"), "abcd", It.IsAny<string>()));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
