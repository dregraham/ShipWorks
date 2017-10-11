using Autofac.Extras.Moq;
using Moq;
using ShipEngine.ApiClient.Model;
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
    class DhlExpressRatingServiceTest
    {
        readonly AutoMock mock;
        public DhlExpressRatingServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        private void GetRates_DelegatesToRequestFactoryForRequest()
        {
            DhlExpressRatingService testObject = mock.Create<DhlExpressRatingService>();

            ShipmentEntity shipment = new ShipmentEntity();

            testObject.GetRates(shipment);

            mock.Mock<IRateShipmentRequestFactory>().Verify(r => r.Create(shipment));
        }

        [Fact]
        private void GetRates_DelegatesToShipEngineWebClientForRateShipmentResponse()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            RateShipmentRequest request = new RateShipmentRequest();

            Mock<IRateShipmentRequestFactory> requestFactory = mock.Mock<IRateShipmentRequestFactory>();
            requestFactory.Setup(r => r.Create(shipment)).Returns(request);

            DhlExpressRatingService testObject = mock.Create<DhlExpressRatingService>();
            
            testObject.GetRates(shipment);

            mock.Mock<IShipEngineWebClient>().Verify(r => r.RateShipment(request));
        }

        [Fact]
        private void GetRates_DelegatesToRateGroupFactoryForRateGroup()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            RateShipmentResponse rateResponse = new RateShipmentResponse();
            mock.Mock<IShipEngineWebClient>().Setup(w => w.RateShipment(It.IsAny<RateShipmentRequest>())).Returns(Task.FromResult(rateResponse));
            DhlExpressRatingService testObject = mock.Create<DhlExpressRatingService>();

            testObject.GetRates(shipment);

            mock.Mock<IShipEngineRateGroupFactory>().Verify(r => r.Create(rateResponse, ShipmentTypeCode.DhlExpress));
        }
    }
}
