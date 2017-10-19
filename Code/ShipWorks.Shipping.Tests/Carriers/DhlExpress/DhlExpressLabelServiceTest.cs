using Autofac;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using System.Threading.Tasks;
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

        public DhlExpressLabelServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipment = new ShipmentEntity() { DhlExpress = new DhlExpressShipmentEntity() };
            request = new PurchaseLabelRequest();
            label = new Label();

            shipmentRequestFactory = mock.CreateKeyedMockOf<ICarrierShipmentRequestFactory>().For(ShipmentTypeCode.DhlExpress);
            shipmentRequestFactory.Setup(f => f.CreatePurchaseLabelRequest(AnyShipment)).Returns(request);            
        }

        [Fact]
        public void Create_ThrowsArgumentNullException_WithNullShipment()
        {
            var testObject = mock.Create<DhlExpressLabelService>();

            Assert.Throws<ArgumentNullException>(() => testObject.Create(null));
        }

        [Fact]
        public void Create_DelegatesToCarrierShipmentRequestFactory()
        {
            var testObject = mock.Create<DhlExpressLabelService>();
            testObject.Create(shipment);
            
            shipmentRequestFactory.Verify(f => f.CreatePurchaseLabelRequest(shipment), Times.Once);
        }

        [Fact]
        public void Create_DelegatesToShipEngineWebClient()
        {
            DhlExpressLabelService testObject = mock.Create<DhlExpressLabelService>();

            testObject.Create(shipment);

            mock.Mock<IShipEngineWebClient>().Verify(r => r.PurchaseLabel(request, ApiLogSource.DHLExpress));
        }
        
        [Fact]
        public void Create_CreatesDownloadedLabelData_WithShipmentAndLabel()
        {
            var labelData = mock.Create<DhlExpressDownloadedLabelData>(TypedParameter.From(shipment));
            var labelDataFactory = mock.MockRepository.Create<Func<ShipmentEntity, Label, DhlExpressDownloadedLabelData>>();
            labelDataFactory.Setup(f => f(AnyShipment, It.IsAny<Label>())).Returns(labelData);
            mock.Provide(labelDataFactory.Object);
            mock.Mock<IShipEngineWebClient>().Setup(c => c.PurchaseLabel(request, ApiLogSource.DHLExpress)).Returns(Task.FromResult(label));

            var testObject = mock.Create<DhlExpressLabelService>();

            testObject.Create(shipment);

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
