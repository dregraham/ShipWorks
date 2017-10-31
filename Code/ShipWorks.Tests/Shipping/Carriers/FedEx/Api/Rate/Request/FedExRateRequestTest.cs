using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request
{
    public class FedExRateRequestTest
    {
        private Mock<IFedExRateRequestManipulator> firstManipulator;
        private Mock<IFedExRateRequestManipulator> secondManipulator;
        private Mock<Func<ICarrierSettingsRepository, IFedExRateRequestManipulator>> firstManipulatorFactory;
        private Mock<Func<ICarrierSettingsRepository, IFedExRateRequestManipulator>> secondManipulatorFactory;

        private readonly AutoMock mock;
        private IShipmentEntity shipmentEntity;

        public FedExRateRequestTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipmentEntity = Create.Shipment().AsFedEx(f => f.Set(x => x.ReferencePO, "testPO")).Build();

            firstManipulator = mock.CreateMock<IFedExRateRequestManipulator>();
            firstManipulator.Setup(x => x.ShouldApply(AnyShipment, It.IsAny<FedExRateRequestOptions>())).Returns(true);
            secondManipulator = mock.CreateMock<IFedExRateRequestManipulator>();
            secondManipulator.Setup(x => x.ShouldApply(AnyShipment, It.IsAny<FedExRateRequestOptions>())).Returns(true);

            firstManipulatorFactory = mock.MockFunc<ICarrierSettingsRepository, IFedExRateRequestManipulator>(firstManipulator);
            secondManipulatorFactory = mock.MockFunc<ICarrierSettingsRepository, IFedExRateRequestManipulator>(secondManipulator);

            mock.Provide<IEnumerable<Func<ICarrierSettingsRepository, IFedExRateRequestManipulator>>>(
                new List<Func<ICarrierSettingsRepository, IFedExRateRequestManipulator>>
                {
                    firstManipulatorFactory.Object,
                    secondManipulatorFactory.Object,
                });
        }

        [Fact]
        public void Submit_DelegatesToManipulators()
        {
            var testObject = mock.Create<FedExRateRequest>();
            testObject.Submit(shipmentEntity);

            // Verify all of the manipulators were called and our test object was passed to them
            firstManipulator.Verify(m => m.Manipulate(shipmentEntity, It.IsAny<RateRequest>()), Times.Once());
            secondManipulator.Verify(m => m.Manipulate(shipmentEntity, It.IsAny<RateRequest>()), Times.Once());
        }

        [Fact]
        public void Submit_DelegatesToFedExService()
        {
            var service = mock.FromFactory<IFedExServiceGatewayFactory>()
                .Mock(x => x.Create(It.IsAny<ICarrierSettingsRepository>()));

            var testObject = mock.Create<FedExRateRequest>();
            testObject.Submit(shipmentEntity);

            service.Verify(s => s.GetRates(It.IsAny<RateRequest>(), shipmentEntity), Times.Once());
        }

        [Fact]
        public void Submit_ReturnsResponseFromRateResult()
        {
            var rateReply = new RateReply();
            var response = mock.CreateMock<IFedExRateResponse>();

            mock.FromFactory<IFedExServiceGatewayFactory>()
                .Mock(x => x.Create(It.IsAny<ICarrierSettingsRepository>()))
                .Setup(x => x.GetRates(It.IsAny<RateRequest>(), AnyIShipment))
                .Returns(rateReply);

            mock.MockFunc<RateReply, IFedExRateResponse>()
                .Setup(f => f(rateReply)).Returns(() => response.Object);

            var testObject = mock.Create<FedExRateRequest>();
            var result = testObject.Submit(shipmentEntity);

            Assert.Equal(response.Object, result);
        }
    }
}
