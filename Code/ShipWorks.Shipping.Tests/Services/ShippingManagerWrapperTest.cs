using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Configuration;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services
{
    public class ShippingManagerWrapperTest : IDisposable
    {
        readonly AutoMock mock;

        public ShippingManagerWrapperTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void SaveShipmentToDatabase_ReturnsEmptyErrors_WhenNullShipment()
        {
            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            var errors = wrapper.SaveShipmentToDatabase(null, true);

            Assert.Empty(errors);
        }

        [Fact]
        public void SaveShipmentsToDatabase_ReturnsEmptyErrors_WhenListHasNullShipment()
        {
            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            ShipmentEntity shipment = null;

            var errors = wrapper.SaveShipmentsToDatabase(new[] { shipment }, true);
            Assert.Empty(errors);

            // Verify it wasn't an All check by adding a non null shipment to the list
            errors = wrapper.SaveShipmentsToDatabase(new[] { shipment, new ShipmentEntity() }, true);
            Assert.Empty(errors);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
