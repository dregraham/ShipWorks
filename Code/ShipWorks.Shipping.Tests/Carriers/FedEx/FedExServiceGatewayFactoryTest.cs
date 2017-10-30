using System;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx
{
    public class FedExServiceGatewayFactoryTest : IDisposable
    {
        readonly AutoMock mock;

        public FedExServiceGatewayFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void ChooseFedExServiceGateway_ReturnsShipGateway_PrintReturn()
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f.Set(x => x.ReturnType, (int) FedExReturnType.PrintReturnLabel))
                .Set(x => x.ReturnShipment, true)
                .Build();
            var testObject = mock.Create<FedExServiceGatewayFactory>();

            IFedExServiceGateway chosenGateway = testObject.Create(shipment, null);

            Assert.IsAssignableFrom<IFedExServiceGateway>(chosenGateway);
        }

        [Fact]
        public void ChooseFedExServiceGateway_ReturnsShipGateway_NotReturnButEmailReturnLabelReturnType()
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f.Set(x => x.ReturnType, (int) FedExReturnType.EmailReturnLabel))
                .Set(x => x.ReturnShipment, false)
                .Build();

            var testObject = mock.Create<FedExServiceGatewayFactory>();

            IFedExServiceGateway chosenGateway = testObject.Create(shipment, null);

            Assert.IsAssignableFrom<IFedExServiceGateway>(chosenGateway);
        }

        [Fact]
        public void ChooseFedExServiceGateway_ReturnsOpenShipGateway_EmailReturn()
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f.Set(x => x.ReturnType, (int) FedExReturnType.EmailReturnLabel))
                .Set(x => x.ReturnShipment, true)
                .Build();

            var testObject = mock.Create<FedExServiceGatewayFactory>();

            IFedExServiceGateway chosenGateway = testObject.Create(shipment, null);

            Assert.IsAssignableFrom<IFedExOpenShipServiceGateway>(chosenGateway);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
