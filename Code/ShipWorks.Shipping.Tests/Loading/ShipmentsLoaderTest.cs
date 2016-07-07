//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Autofac.Extras.Moq;
//using Moq;
//using SD.LLBLGen.Pro.ORMSupportClasses;
//using ShipWorks.AddressValidation;
//using ShipWorks.Data.Model.EntityClasses;
//using ShipWorks.Shipping.Loading;
//using ShipWorks.Stores;
//using ShipWorks.Stores.Content;
//using ShipWorks.Tests.Shared;
//using Xunit;

//namespace ShipWorks.Shipping.Tests.Loading
//{
//    public class ShipmentsLoaderTest : IDisposable
//    {
//        private readonly AutoMock mock;
//        private ShipmentsLoader testObject;
//        private readonly OrderEntity orderEntity;
//        private readonly ShipmentEntity shipmentEntity;

//        public ShipmentsLoaderTest()
//        {
//            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

//            orderEntity = new OrderEntity(1006)
//            {
//                StoreID = 1,
//                Store = new StoreEntity(1)
//            };

//            shipmentEntity = new ShipmentEntity(1031)
//            {
//                Processed = false,
//                Order = orderEntity
//            };

//            mock.Mock<IStoreTypeManager>()
//                .Setup(x => x.GetType(It.IsAny<StoreEntity>()))
//                .Returns(mock.CreateMock<TestStoreType>().Object);

//            mock.Mock<IOrderManager>()
//                .Setup(x => x.LoadOrder(It.IsAny<long>(), It.IsAny<IPrefetchPath2>()))
//                .Returns(orderEntity);
//        }

//        [Fact]
//        public async Task ShipmentsReturned_Correct_WhenOrderHasMultipleShipments_Test()
//        {
//            orderEntity.Shipments.Add(new ShipmentEntity());

//            var adapter1 = new ShipmentEntity();
//            var adapter2 = new ShipmentEntity();
//            orderEntity.Shipments.AddRange(new[] { adapter1, adapter2 });

//            testObject = mock.Create<ShipmentsLoader>();

//            var orderSelectionLoaded = await testObject.LoadAsync(new[] { orderEntity.OrderID });

//            Assert.Equal(2, orderSelectionLoaded.Shipments.Count());
//            Assert.Contains(adapter1, orderSelectionLoaded.Shipments);
//            Assert.Contains(adapter2, orderSelectionLoaded.Shipments);
//        }

//        [Fact]
//        public async Task NoShipmentsReturned_WhenAutoCreateIsFalse_Test()
//        {
//            orderEntity.Shipments.Clear();

//            testObject = mock.Create<ShipmentsLoader>();

//            var orderSelectionLoaded = await testObject.LoadAsync(new[] { orderEntity.OrderID });

//            Assert.Equal(0, orderSelectionLoaded.Shipments.Count());
//        }

//        [Fact]
//        public async Task ShipmentsReturned_WhenAutoCreateIsTrueAndHasPermission_Test()
//        {
//            orderEntity.Shipments.Clear();

//            testObject = mock.Create<ShipmentsLoader>();

//            await testObject.LoadAsync(new[] { orderEntity.OrderID });

//            mock.Mock<IShipmentFactory>()
//                .Verify(x => x.AutoCreateIfNecessary(orderEntity));
//        }

//        [Fact]
//        public async Task AddressValidation_Performed_WhenAddressValidationAllowed_Test()
//        {
//            testObject = mock.Create<ShipmentsLoader>();

//            await testObject.LoadAsync(new[] { orderEntity.OrderID });

//            mock.Mock<IValidatedAddressManager>().Verify(av => av.ValidateShipmentAsync(It.IsAny<ShipmentEntity>()), Times.Once);
//        }

//        [Fact]
//        public async Task AddressValidation_NotPerformed_WhenNoShipmentsAndAddressValidationAllowed_Test()
//        {
//            orderEntity.Shipments.Clear();

//            testObject = mock.Create<ShipmentsLoader>();

//            await testObject.LoadAsync(new[] { orderEntity.OrderID });

//            mock.Mock<IValidatedAddressManager>().Verify(av => av.ValidateShipmentAsync(It.IsAny<ShipmentEntity>()), Times.Never);
//        }

//        [Fact]
//        public async Task OrderSelectionLoaded_HasException_WhenInvalidOrderID_Test()
//        {
//            mock.Mock<IOrderManager>()
//                .Setup(x => x.LoadOrder(It.IsAny<long>(), It.IsAny<IPrefetchPath2>()))
//                .Throws<InvalidOperationException>();

//            testObject = mock.Create<ShipmentsLoader>();

//            var orderSelectionLoaded = await testObject.LoadAsync(new[] { 0L });

//            Assert.NotNull(orderSelectionLoaded.Error);
//        }

//        public void Dispose()
//        {
//            mock.Dispose();
//        }
//    }
//}
