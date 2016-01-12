using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Loading;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Loading
{
    public class ShipmentLoaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private ShipmentLoader testObject;
        private readonly OrderEntity orderEntity;
        private readonly ShipmentEntity shipmentEntity;

        public ShipmentLoaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            orderEntity = new OrderEntity(1006)
            {
                StoreID = 1,
                Store = new StoreEntity(1)
            };

            shipmentEntity = new ShipmentEntity(1031)
            {
                Processed = false,
                Order = orderEntity
            };

            mock.Mock<IStoreTypeManager>()
                .Setup(x => x.GetType(It.IsAny<StoreEntity>()))
                .Returns(mock.CreateMock<TestStoreType>().Object);

            mock.Mock<IOrderManager>()
                .Setup(x => x.LoadOrder(It.IsAny<long>(), It.IsAny<IPrefetchPath2>()))
                .Returns(orderEntity);
        }

        [Fact]
        public async Task ShipmentsReturned_Correct_WhenOrderHasMultipleShipments_Test()
        {
            orderEntity.Shipments.Add(new ShipmentEntity());

            ICarrierShipmentAdapter adapter1 = mock.CreateMock<ICarrierShipmentAdapter>().Object;
            ICarrierShipmentAdapter adapter2 = mock.CreateMock<ICarrierShipmentAdapter>().Object;

            mock.Mock<ICarrierShipmentAdapterFactory>()
                .Setup(x => x.Get(orderEntity.Shipments.ElementAt(0)))
                .Returns(adapter1);

            mock.Mock<ICarrierShipmentAdapterFactory>()
                .Setup(x => x.Get(orderEntity.Shipments.ElementAt(1)))
                .Returns(adapter2);

            testObject = mock.Create<ShipmentLoader>();

            OrderSelectionLoaded orderSelectionLoaded = await testObject.Load(orderEntity.OrderID);

            Assert.Equal(2, orderSelectionLoaded.ShipmentAdapters.Count());
            Assert.Contains(adapter1, orderSelectionLoaded.ShipmentAdapters);
            Assert.Contains(adapter2, orderSelectionLoaded.ShipmentAdapters);
        }

        [Fact]
        public async Task NoShipmentsReturned_WhenAutoCreateIsFalse_Test()
        {
            orderEntity.Shipments.Clear();

            testObject = mock.Create<ShipmentLoader>();

            OrderSelectionLoaded orderSelectionLoaded = await testObject.Load(orderEntity.OrderID);

            Assert.Equal(0, orderSelectionLoaded.ShipmentAdapters.Count());
            Assert.NotEqual(null, orderSelectionLoaded.Order);
        }

        [Fact]
        public async Task ShipmentsReturned_WhenAutoCreateIsTrueAndHasPermission_Test()
        {
            orderEntity.Shipments.Clear();

            testObject = mock.Create<ShipmentLoader>();

            await testObject.Load(orderEntity.OrderID);

            mock.Mock<IShipmentFactory>()
                .Verify(x => x.AutoCreateIfNecessary(orderEntity));
        }

        [Fact]
        public async Task AddressValidation_Performed_WhenAddressValidationAllowed_Test()
        {
            testObject = mock.Create<ShipmentLoader>();

            await testObject.Load(orderEntity.OrderID);

            mock.Mock<IValidatedAddressManager>().Verify(av => av.ValidateShipmentAsync(It.IsAny<ShipmentEntity>()), Times.Once);
        }

        [Fact]
        public async Task AddressValidation_NotPerformed_WhenNoShipmentsAndAddressValidationAllowed_Test()
        {
            orderEntity.Shipments.Clear();

            testObject = mock.Create<ShipmentLoader>();

            await testObject.Load(orderEntity.OrderID);

            mock.Mock<IValidatedAddressManager>().Verify(av => av.ValidateShipmentAsync(It.IsAny<ShipmentEntity>()), Times.Never);
        }

        [Fact]
        public async Task OrderSelectionLoaded_HasException_WhenInvalidOrderID_Test()
        {
            mock.Mock<IOrderManager>()
                .Setup(x => x.LoadOrder(It.IsAny<long>(), It.IsAny<IPrefetchPath2>()))
                .Throws<InvalidOperationException>();

            testObject = mock.Create<ShipmentLoader>();

            OrderSelectionLoaded orderSelectionLoaded = await testObject.Load(0);

            Assert.NotNull(orderSelectionLoaded.Exception);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
