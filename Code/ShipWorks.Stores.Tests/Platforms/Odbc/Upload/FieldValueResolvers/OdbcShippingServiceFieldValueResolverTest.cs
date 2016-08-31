using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Odbc.Upload.FieldValueResolvers;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Upload.FieldValueResolvers
{
    public class OdbcShippingServiceFieldValueResolverTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcShippingServiceFieldValueResolverTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void GetValue_ReturnsNull_WhenEntityIsNull()
        {
            OdbcShippingServiceFieldValueResolver testObject = mock.Create<OdbcShippingServiceFieldValueResolver>();
            Assert.Null(testObject.GetValue(null, null));
        }

        [Fact]
        public void GetValue_ReturnsNull_WhenEntityNotShipmentEntity()
        {
            OdbcShippingServiceFieldValueResolver testObject = mock.Create<OdbcShippingServiceFieldValueResolver>();
            Assert.Null(testObject.GetValue(null, new OrderEntity()));
        }

        [Fact]
        public void GetValue_DelegatesToShippingManagerForShippingService()
        {
            Mock<IShippingManager> shippingManager = mock.Mock<IShippingManager>();
            ShipmentEntity shipment = new ShipmentEntity();

            OdbcShippingServiceFieldValueResolver testObject = mock.Create<OdbcShippingServiceFieldValueResolver>();
            testObject.GetValue(null, shipment);

            shippingManager.Verify(s => s.GetOverriddenSerivceUsed(shipment));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}