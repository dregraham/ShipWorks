using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship
{
    public class FedExShipRequestTest
    {
        private Mock<IFedExShipRequestManipulator> firstManipulator;
        private Mock<IFedExShipRequestManipulator> secondManipulator;
        private Mock<Func<IFedExSettingsRepository, IFedExShipRequestManipulator>> firstManipulatorFactory;
        private Mock<Func<IFedExSettingsRepository, IFedExShipRequestManipulator>> secondManipulatorFactory;

        private readonly AutoMock mock;
        private readonly ShipmentEntity shipmentEntity;

        public FedExShipRequestTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipmentEntity = Create.Shipment().AsFedEx(f => f.Set(x => x.ReferencePO, "testPO")).Build();

            firstManipulator = mock.CreateMock<IFedExShipRequestManipulator>();
            firstManipulator.Setup(x => x.ShouldApply(AnyShipment, AnyInt)).Returns(true);
            firstManipulator.Setup(x => x.Manipulate(AnyShipment, It.IsAny<ProcessShipmentRequest>(), AnyInt))
                .Returns(new ProcessShipmentRequest());
            secondManipulator = mock.CreateMock<IFedExShipRequestManipulator>();
            secondManipulator.Setup(x => x.ShouldApply(AnyShipment, AnyInt)).Returns(true);
            secondManipulator.Setup(x => x.Manipulate(AnyShipment, It.IsAny<ProcessShipmentRequest>(), AnyInt))
                .Returns(new ProcessShipmentRequest());

            firstManipulatorFactory = mock.MockFunc<IFedExSettingsRepository, IFedExShipRequestManipulator>(firstManipulator);
            secondManipulatorFactory = mock.MockFunc<IFedExSettingsRepository, IFedExShipRequestManipulator>(secondManipulator);

            mock.Provide<IEnumerable<Func<IFedExSettingsRepository, IFedExShipRequestManipulator>>>(
                new List<Func<IFedExSettingsRepository, IFedExShipRequestManipulator>>
                {
                    firstManipulatorFactory.Object,
                    secondManipulatorFactory.Object,
                });
        }

        [Fact]
        public void Submit_DelegatesToManipulators()
        {
            var testObject = mock.Create<FedExShipRequest>();
            testObject.Submit(shipmentEntity, 2);

            // Verify all of the manipulators were called and our test object was passed to them
            firstManipulator.Verify(m => m.Manipulate(shipmentEntity, It.IsAny<ProcessShipmentRequest>(), 2), Times.Once());
            secondManipulator.Verify(m => m.Manipulate(shipmentEntity, It.IsAny<ProcessShipmentRequest>(), 2), Times.Once());
        }

        [Fact]
        public void Submit_DelegatesToFedExService()
        {
            var service = mock.FromFactory<IFedExServiceGatewayFactory>()
                .Mock(x => x.Create(It.IsAny<IFedExSettingsRepository>()));

            var testObject = mock.Create<FedExShipRequest>();
            testObject.Submit(shipmentEntity, 0);

            service.Verify(s => s.Ship(It.IsAny<ProcessShipmentRequest>()), Times.Once());
        }

        [Fact]
        public void Submit_ReturnsResponseFromShipResult()
        {
            var shipmentReply = new ProcessShipmentReply();
            var response = mock.CreateMock<IFedExShipResponse>();
            response.Setup(x => x.ApplyManipulators())
                .Returns(GenericResult.FromSuccess(response.Object));

            mock.FromFactory<IFedExServiceGatewayFactory>()
                .Mock(x => x.Create(It.IsAny<IFedExSettingsRepository>()))
                .Setup(x => x.Ship(It.IsAny<ProcessShipmentRequest>()))
                .Returns(shipmentReply);

            var function = mock.MockRepository
                .Create<Func<ShipmentEntity, ProcessShipmentReply, IFedExShipResponse>>();
            function.Setup(f => f(AnyShipment, It.IsAny<ProcessShipmentReply>()))
                .Returns(() => response.Object);
            mock.Provide(function.Object);

            var testObject = mock.Create<FedExShipRequest>();
            var result = testObject.Submit(shipmentEntity, 0);

            Assert.Equal(response.Object, result.Value);
        }
    }
}
