﻿using Autofac;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress
{
    public class DhlExpressLabelServiceTest
    {
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;
        private readonly Mock<ICarrierShipmentRequestFactory> shipmentRequestFactory;
        private readonly PurchaseLabelRequest request;
        private readonly Label label;
        private TelemetricResult<IDownloadedLabelData> telemetricResult;

        public DhlExpressLabelServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipment = new ShipmentEntity() { DhlExpress = new DhlExpressShipmentEntity() };
            request = new PurchaseLabelRequest();
            label = new Label();

            shipmentRequestFactory = mock.CreateKeyedMockOf<ICarrierShipmentRequestFactory>().For(ShipmentTypeCode.DhlExpress);
            shipmentRequestFactory.Setup(f => f.CreatePurchaseLabelRequest(AnyShipment)).Returns(request);

            telemetricResult = new TelemetricResult<IDownloadedLabelData>("testing");
        }

        [Fact]
        public async Task Create_ThrowsArgumentNullException_WithNullShipment()
        {
            var testObject = mock.Create<DhlExpressLabelService>();

            await Assert.ThrowsAsync<ArgumentNullException>(() => testObject.Create(null));
        }

        [Fact]
        public async Task Create_DelegatesToCarrierShipmentRequestFactory()
        {
            var testObject = mock.Create<DhlExpressLabelService>();
            await testObject.Create(shipment);
            
            shipmentRequestFactory.Verify(f => f.CreatePurchaseLabelRequest(shipment), Times.Once);
        }

        [Fact]
        public async Task Create_DelegatesToShipEngineWebClient()
        {
            DhlExpressLabelService testObject = mock.Create<DhlExpressLabelService>();

            await testObject.Create(shipment);

            mock.Mock<IShipEngineWebClient>().Verify(r => r.PurchaseLabel(request, ApiLogSource.DHLExpress, It.IsAny<TelemetricResult<IDownloadedLabelData>>()));
        }

        [Fact]
        public async Task Create_ThrowsShippingException_WhenPurchaseLabelFails()
        {
            mock.Mock<IShipEngineWebClient>().Setup(w => w.PurchaseLabel(It.IsAny<PurchaseLabelRequest>(), ApiLogSource.DHLExpress, It.IsAny<TelemetricResult<IDownloadedLabelData>>()))
                .Throws(new Exception("Something broke"));

            DhlExpressLabelService testObject = mock.Create<DhlExpressLabelService>();
            
            var ex = await Assert.ThrowsAsync<ShippingException>(() => testObject.Create(shipment));
            Assert.Equal("Something broke", ex.Message);
        }

        [Fact]
        public async Task Create_ThrowsShippingExceptionWithPrettyError_WhenShipEngineErrorIsCryptic()
        {
            mock.Mock<IDhlExpressAccountRepository>().Setup(r => r.GetAccount(shipment)).Returns(new DhlExpressAccountEntity() { ShipEngineCarrierId = "se-182974" });

            mock.Mock<IShipEngineWebClient>().Setup(w => w.PurchaseLabel(It.IsAny<PurchaseLabelRequest>(), ApiLogSource.DHLExpress, It.IsAny<TelemetricResult<IDownloadedLabelData>>()))
                .Throws(new Exception("A shipping carrier reported an error when processing your request. Carrier ID: se-182974, Carrier: DHL Express. One or more errors occurred."));

            DhlExpressLabelService testObject = mock.Create<DhlExpressLabelService>();

            var ex = await Assert.ThrowsAsync<ShippingException>(() => testObject.Create(shipment));
            Assert.Equal("There was a problem creating the label while communicating with the DHL Express API", ex.Message);
        }

        [Fact]
        public async Task Create_ThrowsShippingExceptionWithPrettyError_WhenShipEngineErrorHasTooMuchText()
        {
            mock.Mock<IDhlExpressAccountRepository>().Setup(r => r.GetAccount(shipment)).Returns(new DhlExpressAccountEntity() { ShipEngineCarrierId = "se-182974" });

            mock.Mock<IShipEngineWebClient>().Setup(w => w.PurchaseLabel(It.IsAny<PurchaseLabelRequest>(), ApiLogSource.DHLExpress, It.IsAny<TelemetricResult<IDownloadedLabelData>>()))
                .Throws(new Exception("A shipping carrier reported an error when processing your request. Carrier ID: se-182974, Carrier: DHL Express. SOMETHING WENT WRONG OMG"));

            DhlExpressLabelService testObject = mock.Create<DhlExpressLabelService>();

            var ex = await Assert.ThrowsAsync<ShippingException>(() => testObject.Create(shipment));
            Assert.Equal("SOMETHING WENT WRONG OMG", ex.Message);
        }

        [Fact]
        public async Task Create_ThrowsShippingExceptionWithPrettyError_WhenDhlExpressErrorHasSeOrderId()
        {
            mock.Mock<IDhlExpressAccountRepository>().Setup(r => r.GetAccount(shipment)).Returns(new DhlExpressAccountEntity() { ShipEngineCarrierId = "se-182974" });

            mock.Mock<IShipEngineWebClient>().Setup(w => w.PurchaseLabel(It.IsAny<PurchaseLabelRequest>(), ApiLogSource.DHLExpress, It.IsAny<TelemetricResult<IDownloadedLabelData>>()))
                .Throws(new Exception("Unable to create label. Order ID: se-164205986. \"K1A 0G9\" is an invalid postal code for the country \"US\"."));

            DhlExpressLabelService testObject = mock.Create<DhlExpressLabelService>();

            var ex = await Assert.ThrowsAsync<ShippingException>(() => testObject.Create(shipment));
            Assert.Equal("\"K1A 0G9\" is an invalid postal code for the country \"US\".", ex.Message);
        }

        [Fact]
        public async Task Create_CreatesDownloadedLabelData_WithShipmentAndLabel()
        {
            var telemetricLabel = new TelemetricResult<Label>("API.Milliseconds");
            telemetricLabel.SetValue(label);
            var labelData = mock.Create<DhlExpressDownloadedLabelData>(TypedParameter.From(shipment));
            var labelDataFactory = mock.MockRepository.Create<Func<ShipmentEntity, Label, DhlExpressDownloadedLabelData>>();
            labelDataFactory.Setup(f => f(AnyShipment, It.IsAny<Label>())).Returns(labelData);
            mock.Provide(labelDataFactory.Object);
            mock.Mock<IShipEngineWebClient>().Setup(c => c.PurchaseLabel(request, ApiLogSource.DHLExpress, It.IsAny<TelemetricResult<IDownloadedLabelData>>())).Returns(Task.FromResult(telemetricLabel.Value));

            var testObject = mock.Create<DhlExpressLabelService>();

            await testObject.Create(shipment);

            labelDataFactory.Verify(f => f(shipment, label));
        }

        [Fact]
        public void Void_DelegatesToWebClient()
        {
            var webClient = mock.Mock<IShipEngineWebClient>();
            webClient
                .Setup(c => c.VoidLabel(AnyString, ApiLogSource.DHLExpress))
                .Returns(Task.FromResult(new VoidLabelResponse(true)));

            var testObject = mock.Create<DhlExpressLabelService>();

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

            var testObject = mock.Create<DhlExpressLabelService>();
            testObject.Void(shipment);

            iLog.Verify(l => l.Error(exception), Times.Once);
        }
    }
}
