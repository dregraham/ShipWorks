using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using Xunit;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress.API.ShipEngine
{
    public class DhlExpressShipEngineRatingClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<ICarrierShipmentRequestFactory> requestFactory;
        private readonly RateShipmentRequest request;

        public DhlExpressShipEngineRatingClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            request = new RateShipmentRequest();

            requestFactory = mock.CreateKeyedMockOf<ICarrierShipmentRequestFactory>()
                .For(ShipmentTypeCode.DhlExpress);

            requestFactory.Setup(f => f.CreateRateShipmentRequest(It.IsAny<ShipmentEntity>()))
                .Returns(new RateShipmentRequest());

            mock.Mock<IDhlExpressAccountRepository>()
                .SetupGet(r => r.AccountsReadOnly)
                .Returns(new[] { new DhlExpressAccountEntity() });
        }

        [Fact]
        public void GetRates_DelegatesToRequestFactoryForRequest()
        {
            var shipmentType = mock.CreateMock<ShipmentType>();
            shipmentType.Setup(s => s.GetAvailableServiceTypes())
                .Returns(new[] { (int) DhlExpressServiceType.ExpressWorldWide });

            RateShipmentResponse rateShipmentResponse = new RateShipmentResponse { RateResponse = new RateResponse() };
            mock.Mock<IShipEngineWebClient>().Setup(w => w.RateShipment(It.IsAny<RateShipmentRequest>(), ApiLogSource.DHLExpress)).Returns(Task.FromResult(rateShipmentResponse));

            mock.Mock<IShipmentTypeManager>().Setup(m => m.Get(ShipmentTypeCode.DhlExpress))
                .Returns(shipmentType.Object);

            ShipmentEntity shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity()
                {
                    Service = 0
                }
            };

            DhlExpressShipEngineRatingClient testObject = mock.Create<DhlExpressShipEngineRatingClient>();

            testObject.GetRates(shipment);

            requestFactory.Verify(r => r.CreateRateShipmentRequest(shipment), Times.Once);
        }

        [Fact]
        public void GetRates_DelegatesToShipEngineWebClientForRateShipmentResponse()
        {
            var shipmentType = mock.CreateMock<ShipmentType>();
            shipmentType.Setup(s => s.GetAvailableServiceTypes())
                .Returns(new[] { (int) DhlExpressServiceType.ExpressWorldWide });

            RateShipmentResponse rateShipmentResponse = new RateShipmentResponse { RateResponse = new RateResponse() };
            mock.Mock<IShipEngineWebClient>().Setup(w => w.RateShipment(It.IsAny<RateShipmentRequest>(), ApiLogSource.DHLExpress)).Returns(Task.FromResult(rateShipmentResponse));

            mock.Mock<IShipmentTypeManager>().Setup(m => m.Get(ShipmentTypeCode.DhlExpress))
                .Returns(shipmentType.Object);

            ShipmentEntity shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity()
                {
                    Service = 0
                }
            };

            DhlExpressShipEngineRatingClient testObject = mock.Create<DhlExpressShipEngineRatingClient>();

            testObject.GetRates(shipment);

            mock.Mock<IShipEngineWebClient>().Verify(r => r.RateShipment(request, ApiLogSource.DHLExpress));
        }

        [Fact]
        public void GetRates_DelegatesToRateGroupFactoryForRateGroup()
        {
            var shipmentType = mock.CreateMock<ShipmentType>();
            shipmentType.Setup(s => s.GetAvailableServiceTypes())
                .Returns(new[] { (int) DhlExpressServiceType.ExpressWorldWide });

            mock.Mock<IShipmentTypeManager>().Setup(m => m.Get(ShipmentTypeCode.DhlExpress))
                .Returns(shipmentType.Object);

            ShipmentEntity shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity()
                {
                    Service = 0
                }
            };

            RateShipmentResponse rateShipmentResponse = new RateShipmentResponse { RateResponse = new RateResponse() };
            mock.Mock<IShipEngineWebClient>().Setup(w => w.RateShipment(It.IsAny<RateShipmentRequest>(), ApiLogSource.DHLExpress)).Returns(Task.FromResult(rateShipmentResponse));
            DhlExpressShipEngineRatingClient testObject = mock.Create<DhlExpressShipEngineRatingClient>();

            testObject.GetRates(shipment);

            mock.Mock<IShipEngineRateGroupFactory>().Verify(r => r.Create(rateShipmentResponse.RateResponse, ShipmentTypeCode.DhlExpress, It.IsAny<IEnumerable<string>>()));
        }

        [Fact]
        public void GetRates_SendsApiValueForServiceNotAvailableRate_ThatIsShipmentService()
        {
            var shipmentType = mock.CreateMock<ShipmentType>();
            shipmentType.Setup(s => s.GetAvailableServiceTypes())
                .Returns(new[] { (int) DhlExpressServiceType.ExpressEnvelope });

            mock.Mock<IShipmentTypeManager>().Setup(m => m.Get(ShipmentTypeCode.DhlExpress))
                .Returns(shipmentType.Object);

            ShipmentEntity shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity()
                {
                    Service = (int) DhlExpressServiceType.ExpressWorldWide
                }
            };

            RateShipmentResponse rateShipmentResponse = new RateShipmentResponse { RateResponse = new RateResponse() };
            mock.Mock<IShipEngineWebClient>().Setup(w => w.RateShipment(It.IsAny<RateShipmentRequest>(), ApiLogSource.DHLExpress)).Returns(Task.FromResult(rateShipmentResponse));
            DhlExpressShipEngineRatingClient testObject = mock.Create<DhlExpressShipEngineRatingClient>();

            testObject.GetRates(shipment);

            mock.Mock<IShipEngineRateGroupFactory>().Verify(r => r.Create(rateShipmentResponse.RateResponse, ShipmentTypeCode.DhlExpress,
                It.Is<IEnumerable<string>>(apiValues =>
                    apiValues.Contains("express_envelope"))));
        }

        [Fact]
        public void GetRates_DoesNotSendApiValueForRateNotAvailableRateAndNotShipmentRate()
        {
            var shipmentType = mock.CreateMock<ShipmentType>();
            shipmentType.Setup(s => s.GetAvailableServiceTypes())
                .Returns(new[] { (int) DhlExpressServiceType.ExpressEnvelope });

            mock.Mock<IShipmentTypeManager>().Setup(m => m.Get(ShipmentTypeCode.DhlExpress))
                .Returns(shipmentType.Object);

            ShipmentEntity shipment = new ShipmentEntity()
            {
                DhlExpress = new DhlExpressShipmentEntity()
                {
                    Service = (int) DhlExpressServiceType.ExpressEnvelope
                }
            };

            RateShipmentResponse rateShipmentResponse = new RateShipmentResponse { RateResponse = new RateResponse() };
            mock.Mock<IShipEngineWebClient>().Setup(w => w.RateShipment(It.IsAny<RateShipmentRequest>(), ApiLogSource.DHLExpress)).Returns(Task.FromResult(rateShipmentResponse));
            DhlExpressShipEngineRatingClient testObject = mock.Create<DhlExpressShipEngineRatingClient>();

            testObject.GetRates(shipment);

            mock.Mock<IShipEngineRateGroupFactory>().Verify(r => r.Create(rateShipmentResponse.RateResponse, ShipmentTypeCode.DhlExpress,
                It.Is<IEnumerable<string>>(apiValues => apiValues.SingleOrDefault() == "express_envelope")));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
