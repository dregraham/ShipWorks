using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Configuration;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services
{
    public class ShipmentFactoryTest : IDisposable
    {
        readonly AutoMock mock;

        public ShipmentFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void AutoCreateIfNecessary_DelegatesToShippingConfiguration()
        {
            var order = new OrderEntity();

            var testObject = mock.Create<ShipmentFactory>();
            testObject.AutoCreateIfNecessary(order);

            mock.Mock<IShippingConfiguration>()
                .Verify(x => x.ShouldAutoCreateShipment(order));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
