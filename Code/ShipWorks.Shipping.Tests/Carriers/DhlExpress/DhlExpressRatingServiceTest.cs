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
        readonly Mock<ICarrierRateShipmentRequestFactory> requestFactory;
        readonly RateShipmentRequest request;

        public DhlExpressRatingServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            request = new RateShipmentRequest();

            requestFactory = mock.CreateKeyedMockOf<ICarrierRateShipmentRequestFactory>()
                .For(ShipmentTypeCode.DhlExpress);

            requestFactory.Setup(f => f.Create(It.IsAny<ShipmentEntity>()))
                .Returns(new RateShipmentRequest());

            mock.Mock<IDhlExpressAccountRepository>()
                .SetupGet(r => r.Accounts)
                .Returns(new[] { new DhlExpressAccountEntity() });
        }

        [Fact]
        public void GetRates_DelegatesToRequestFactoryForRequest()
        {
            DhlExpressRatingService testObject = mock.Create<DhlExpressRatingService>();

            ShipmentEntity shipment = new ShipmentEntity();

            testObject.GetRates(shipment);

            requestFactory.Verify(r => r.Create(shipment), Times.Once);
        }

        [Fact]
        public void GetRates_DelegatesToShipEngineWebClientForRateShipmentResponse()
        {
            ShipmentEntity shipment = new ShipmentEntity();

            DhlExpressRatingService testObject = mock.Create<DhlExpressRatingService>();
            
            testObject.GetRates(shipment);

            mock.Mock<IShipEngineWebClient>().Verify(r => r.RateShipment(request, ApiLogSource.DHLExpress));
        }

        [Fact]
        public void GetRates_DelegatesToRateGroupFactoryForRateGroup()
        {
            
            ShipmentEntity shipment = new ShipmentEntity();
            RateShipmentResponse rateResponse = new RateShipmentResponse();
            mock.Mock<IShipEngineWebClient>().Setup(w => w.RateShipment(It.IsAny<RateShipmentRequest>(), ApiLogSource.DHLExpress)).Returns(Task.FromResult(rateResponse));
            DhlExpressRatingService testObject = mock.Create<DhlExpressRatingService>();

            testObject.GetRates(shipment);

            mock.Mock<IShipEngineRateGroupFactory>().Verify(r => r.Create(rateResponse, ShipmentTypeCode.DhlExpress));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
