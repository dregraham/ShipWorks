using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api
{
    public class FedExShippingClerkFactoryTest
    {
        private readonly AutoMock mock;

        public FedExShippingClerkFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void FedExShippingClerkReturned_WhenNullShipmentRequested()
        {
            IFedExShippingClerk shippingClerk = mock.Create<FedExShippingClerkFactory>().Create();
            Assert.IsAssignableFrom<IFedExShippingClerk>(shippingClerk);

            Assert.True(shippingClerk is IFedExShippingClerk);
        }

        [Fact]
        public void FakeFimsShippingClerkReturned_WhenFimsShipmentRequested()
        {
            var shipment = Create.Shipment()
                .AsFedEx(f => f.Set(x => x.Service, (int) FedExServiceType.FedExFimsMailView))
                .Build();
            IFedExShippingClerk shippingClerk = mock.Create<FedExShippingClerkFactory>().Create(shipment);

            Assert.True(shippingClerk is IFimsShippingClerk);
        }
    }
}
