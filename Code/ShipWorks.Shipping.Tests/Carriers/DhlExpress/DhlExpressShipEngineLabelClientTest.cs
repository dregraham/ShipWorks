using Autofac;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Dhl.API.ShipEngine;
using ShipWorks.Shipping.Tracking;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress
{
    public class DhlExpressShipEngineLabelClientTest
    {
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;
        private readonly Mock<ICarrierShipmentRequestFactory> shipmentRequestFactory;
        private readonly ShipWorks.Shipping.ShipEngine.DTOs.PurchaseLabelRequest request;
        private readonly ShipWorks.Shipping.ShipEngine.DTOs.Label label;
        private TelemetricResult<IDownloadedLabelData> telemetricResult;

        public DhlExpressShipEngineLabelClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipment = new ShipmentEntity() { DhlExpress = new DhlExpressShipmentEntity() };
            request = new ShipWorks.Shipping.ShipEngine.DTOs.PurchaseLabelRequest();
            label = new ShipWorks.Shipping.ShipEngine.DTOs.Label();

            shipmentRequestFactory = mock.CreateKeyedMockOf<ICarrierShipmentRequestFactory>().For(ShipmentTypeCode.DhlExpress);
            shipmentRequestFactory.Setup(f => f.CreatePurchaseLabelRequest(AnyShipment)).Returns(request);

            telemetricResult = new TelemetricResult<IDownloadedLabelData>("testing");
        }

        [Fact]
        public async Task Create_ThrowsArgumentNullException_WithNullShipment()
        {
            var testObject = mock.Create<DhlExpressShipEngineLabelClient>();

            await Assert.ThrowsAsync<ArgumentNullException>(() => testObject.Create(null)).ConfigureAwait(false);
        }

        [Fact]
        public async Task Create_DelegatesToCarrierShipmentRequestFactory()
        {
            var testObject = mock.Create<DhlExpressShipEngineLabelClient>();
            await testObject.Create(shipment).ConfigureAwait(false);

            shipmentRequestFactory.Verify(f => f.CreatePurchaseLabelRequest(shipment), Times.Once);
        }

        [Fact]
        public async Task Create_DelegatesToShipEngineWebClient()
        {
            var testObject = mock.Create<DhlExpressShipEngineLabelClient>();

            await testObject.Create(shipment).ConfigureAwait(false);

            mock.Mock<IShipEngineWebClient>().Verify(r => r.PurchaseLabel(request, ApiLogSource.DHLExpress, It.IsAny<TelemetricResult<IDownloadedLabelData>>()));
        }

        [Fact]
        public async Task Create_ThrowsShippingException_WhenPurchaseLabelFails()
        {
            mock.Mock<IShipEngineWebClient>().Setup(w => w.PurchaseLabel(It.IsAny<ShipWorks.Shipping.ShipEngine.DTOs.PurchaseLabelRequest>(), ApiLogSource.DHLExpress, It.IsAny<TelemetricResult<IDownloadedLabelData>>()))
                .Throws(new Exception("Something broke"));

            var testObject = mock.Create<DhlExpressShipEngineLabelClient>();

            var ex = await Assert.ThrowsAsync<ShippingException>(() => testObject.Create(shipment)).ConfigureAwait(false);
            Assert.Equal("Something broke", ex.Message);
        }

        [Fact]
        public async Task Create_ThrowsShippingExceptionWithPrettyError_WhenShipEngineErrorIsCryptic()
        {
            mock.Mock<IDhlExpressAccountRepository>().Setup(r => r.GetAccount(shipment)).Returns(new DhlExpressAccountEntity() { ShipEngineCarrierId = "se-182974" });

            mock.Mock<IShipEngineWebClient>().Setup(w => w.PurchaseLabel(It.IsAny<ShipWorks.Shipping.ShipEngine.DTOs.PurchaseLabelRequest>(), ApiLogSource.DHLExpress, It.IsAny<TelemetricResult<IDownloadedLabelData>>()))
                .Throws(new Exception("A shipping carrier reported an error when processing your request. Carrier ID: se-182974, Carrier: DHL Express. One or more errors occurred."));

            var testObject = mock.Create<DhlExpressShipEngineLabelClient>();

            var ex = await Assert.ThrowsAsync<ShippingException>(() => testObject.Create(shipment)).ConfigureAwait(false);
            Assert.Equal("There was a problem creating the label while communicating with the DHL Express API", ex.Message);
        }

        [Fact]
        public async Task Create_ThrowsShippingExceptionWithPrettyError_WhenShipEngineErrorHasTooMuchText()
        {
            mock.Mock<IDhlExpressAccountRepository>().Setup(r => r.GetAccount(shipment)).Returns(new DhlExpressAccountEntity() { ShipEngineCarrierId = "se-182974" });

            mock.Mock<IShipEngineWebClient>().Setup(w => w.PurchaseLabel(It.IsAny<ShipWorks.Shipping.ShipEngine.DTOs.PurchaseLabelRequest>(), ApiLogSource.DHLExpress, It.IsAny<TelemetricResult<IDownloadedLabelData>>()))
                .Throws(new Exception("A shipping carrier reported an error when processing your request. Carrier ID: se-182974, Carrier: DHL Express. SOMETHING WENT WRONG OMG"));

            var testObject = mock.Create<DhlExpressShipEngineLabelClient>();

            var ex = await Assert.ThrowsAsync<ShippingException>(() => testObject.Create(shipment)).ConfigureAwait(false);
            Assert.Equal("SOMETHING WENT WRONG OMG", ex.Message);
        }

        [Fact]
        public async Task Create_ThrowsShippingExceptionWithPrettyError_WhenDhlExpressErrorHasSeOrderId()
        {
            mock.Mock<IDhlExpressAccountRepository>().Setup(r => r.GetAccount(shipment)).Returns(new DhlExpressAccountEntity() { ShipEngineCarrierId = "se-182974" });

            mock.Mock<IShipEngineWebClient>().Setup(w => w.PurchaseLabel(It.IsAny<ShipWorks.Shipping.ShipEngine.DTOs.PurchaseLabelRequest>(), ApiLogSource.DHLExpress, It.IsAny<TelemetricResult<IDownloadedLabelData>>()))
                .Throws(new Exception("Unable to create label. Order ID: se-164205986. \"K1A 0G9\" is an invalid postal code for the country \"US\"."));

            var testObject = mock.Create<DhlExpressShipEngineLabelClient>();

            var ex = await Assert.ThrowsAsync<ShippingException>(() => testObject.Create(shipment)).ConfigureAwait(false);
            Assert.Equal("\"K1A 0G9\" is an invalid postal code for the country \"US\".", ex.Message);
        }

        [Fact]
        public async Task Create_CreatesDownloadedLabelData_WithShipmentAndLabel()
        {
            var telemetricLabel = new TelemetricResult<ShipWorks.Shipping.ShipEngine.DTOs.Label>("API.Milliseconds");
            telemetricLabel.SetValue(label);
            var labelData = mock.Create<DhlExpressShipEngineDownloadedLabelData>(TypedParameter.From(shipment));
            var labelDataFactory = mock.MockRepository.Create<Func<ShipmentEntity, ShipWorks.Shipping.ShipEngine.DTOs.Label, DhlExpressShipEngineDownloadedLabelData>>();
            labelDataFactory.Setup(f => f(AnyShipment, It.IsAny<ShipWorks.Shipping.ShipEngine.DTOs.Label>())).Returns(labelData);
            mock.Provide(labelDataFactory.Object);
            mock.Mock<IShipEngineWebClient>().Setup(c => c.PurchaseLabel(request, ApiLogSource.DHLExpress, It.IsAny<TelemetricResult<IDownloadedLabelData>>())).Returns(Task.FromResult(telemetricLabel.Value));

            var testObject = mock.Create<DhlExpressShipEngineLabelClient>();

            await testObject.Create(shipment).ConfigureAwait(false);

            labelDataFactory.Verify(f => f(shipment, label));
        }

        [Fact]
        public void Void_DelegatesToWebClient()
        {
            var webClient = mock.Mock<IShipEngineWebClient>();
            webClient
                .Setup(c => c.VoidLabel(AnyString, ApiLogSource.DHLExpress))
                .Returns(Task.FromResult(new ShipWorks.Shipping.ShipEngine.DTOs.VoidLabelResponse(true)));

            var testObject = mock.Create<DhlExpressShipEngineLabelClient>();

            shipment.DhlExpress.ShipEngineLabelID = "blah";

            testObject.Void(shipment);

            webClient.Verify(c => c.VoidLabel("blah", ApiLogSource.DHLExpress), Times.Once);
        }

        [Fact]
        public void Void_DelegatesToWebClient_LogsException_WhenWebClientThrowsShipEngineException()
        {
            var iLog = mock.CreateMock<ILog>();
            mock.MockFunc<Type, ILog>(iLog);

            var webClient = mock.Mock<IShipEngineWebClient>();
            ShipEngineException exception = new ShipEngineException("sadf");
            webClient
                .Setup(c => c.VoidLabel(AnyString, ApiLogSource.DHLExpress))
                .ThrowsAsync(exception);

            var testObject = mock.Create<DhlExpressShipEngineLabelClient>();
            testObject.Void(shipment);

            iLog.Verify(l => l.Error(exception), Times.Once);
        }
    }
}
