using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
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

        [Theory]
        // Ignore order items
        [InlineData(true, false, 0, 0, 0, 0, 0, 0)]
        [InlineData(true, false, 0, 0, 0, 1, 0, 0)]
        [InlineData(false, false, 1, 0, 0, 0, 0, 0)]
        [InlineData(false, false, 1, 0, 0, 1, 0, 0)]
        // Check with order items
        [InlineData(true, true, 0, 0, 0, 0, 0, 0)]
        [InlineData(true, true, 0, 0, 0, 1, 0, 0)]
        [InlineData(true, true, 1, 0, 0, 1, 0, 0)]
        [InlineData(false, true, 1, 0, 0, 0, 0, 0)]
        public void DimensionsAreDefaults_ReturnsTrue_WhenBlankDims(bool expectedValue, bool hasItems, 
            decimal length, decimal width, decimal height,
            decimal itemLength, decimal itemWidth, decimal itemHeight)
        {
            ShippingManagerWrapper wrapper = mock.Create<ShippingManagerWrapper>();

            ShipmentEntity shipment = new ShipmentEntity()
            {
                Order = new OrderEntity()
                {
                    OrderItems =
                    {
                        new OrderItemEntity()
                        {
                            Length = itemLength,
                            Width = itemWidth,
                            Height = itemHeight
                        }
                    }
                },
                Ups = new UpsShipmentEntity()
                {
                    Packages = { new UpsPackageEntity()
                    {
                        DimsLength = (double) length, 
                        DimsWidth = (double) width, 
                        DimsHeight = (double) height
                    }}
                }
            };

            if (!hasItems)
            {
                shipment.Order.OrderItems.Clear();
            }

            List<IPackageAdapter> dimensions = new List<IPackageAdapter>()
            {
                new UpsPackageAdapter(shipment, shipment.Ups.Packages.First(), 0)
            };

            var areDefaults = ShippingManagerWrapper.DimensionsAreDefaults(dimensions, shipment);

            Assert.Equal(expectedValue, areDefaults);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
