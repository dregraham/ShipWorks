using Autofac.Extras.Moq;
using Moq;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress
{
    public class DhlExpressRatingServiceTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Mock<ICarrierShipmentRequestFactory> requestFactory;
        readonly RateShipmentRequest request;

        public DhlExpressRatingServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            request = new RateShipmentRequest();

            requestFactory = mock.CreateKeyedMockOf<ICarrierShipmentRequestFactory>()
                .For(ShipmentTypeCode.DhlExpress);

            requestFactory.Setup(f => f.CreateRateShipmentRequest(It.IsAny<ShipmentEntity>()))
                .Returns(new RateShipmentRequest());

            mock.Mock<IDhlExpressAccountRepository>()
                .SetupGet(r => r.Accounts)
                .Returns(new[] { new DhlExpressAccountEntity() });
        }

        [Fact]
        public void GetRates_DelegatesToRequestFactoryForRequest()
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

            DhlExpressRatingService testObject = mock.Create<DhlExpressRatingService>();

            testObject.GetRates(shipment);

            requestFactory.Verify(r => r.CreateRateShipmentRequest(shipment), Times.Once);
        }

        [Fact]
        public void GetRates_DelegatesToShipEngineWebClientForRateShipmentResponse()
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

            DhlExpressRatingService testObject = mock.Create<DhlExpressRatingService>();
            
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

            RateShipmentResponse rateResponse = new RateShipmentResponse();
            mock.Mock<IShipEngineWebClient>().Setup(w => w.RateShipment(It.IsAny<RateShipmentRequest>(), ApiLogSource.DHLExpress)).Returns(Task.FromResult(rateResponse));
            DhlExpressRatingService testObject = mock.Create<DhlExpressRatingService>();

            testObject.GetRates(shipment);

            mock.Mock<IShipEngineRateGroupFactory>().Verify(r => r.Create(rateResponse, ShipmentTypeCode.DhlExpress, It.IsAny<IEnumerable<string>>()));
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

            RateShipmentResponse rateResponse = new RateShipmentResponse();
            mock.Mock<IShipEngineWebClient>().Setup(w => w.RateShipment(It.IsAny<RateShipmentRequest>(), ApiLogSource.DHLExpress)).Returns(Task.FromResult(rateResponse));
            DhlExpressRatingService testObject = mock.Create<DhlExpressRatingService>();

            testObject.GetRates(shipment);

            mock.Mock<IShipEngineRateGroupFactory>().Verify(r => r.Create(rateResponse, ShipmentTypeCode.DhlExpress,
                It.Is<IEnumerable<string>>(apiValues =>
                    apiValues.Contains("express_envelope") &&
                    apiValues.Contains("express_worldwide"))));
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

            RateShipmentResponse rateResponse = new RateShipmentResponse();
            mock.Mock<IShipEngineWebClient>().Setup(w => w.RateShipment(It.IsAny<RateShipmentRequest>(), ApiLogSource.DHLExpress)).Returns(Task.FromResult(rateResponse));
            DhlExpressRatingService testObject = mock.Create<DhlExpressRatingService>();

            testObject.GetRates(shipment);

            mock.Mock<IShipEngineRateGroupFactory>().Verify(r => r.Create(rateResponse, ShipmentTypeCode.DhlExpress,
                It.Is<IEnumerable<string>>(apiValues => apiValues.SingleOrDefault() == "express_envelope")));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
