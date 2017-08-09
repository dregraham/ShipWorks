﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Stores.Platforms.BigCommerce.DTO;
using ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce.OnlineUpdating
{
    public class ShipmentDetailsUpdaterClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShipmentDetailsUpdaterClient testObject;
        private readonly Mock<IBigCommerceStoreEntity> store;
        private readonly OnlineOrderDetails orderDetail;
        private readonly ShipmentEntity shipment;
        private readonly Dictionary<long, IEnumerable<IBigCommerceOrderItemEntity>> allItems;

        public ShipmentDetailsUpdaterClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ShipmentDetailsUpdaterClient>();
            store = mock.CreateMock<IBigCommerceStoreEntity>();

            mock.Mock<IItemLoader>()
                .Setup(x => x.LoadItems(It.IsAny<IEnumerable<IBigCommerceOrderItemEntity>>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<IBigCommerceWebClient>()))
                .ReturnsAsync(GenericResult.FromSuccess(new OnlineItems(456, new[] { new BigCommerceItem() })));

            mock.Mock<IItemLoader>()
                .Setup(x => x.GetShippingMethod(It.IsAny<ShipmentEntity>()))
                .Returns(Tuple.Create("carrier", "service"));

            orderDetail = new OnlineOrderDetails(1006, false, 123, "123");
            shipment = new ShipmentEntity();
            allItems = new Dictionary<long, IEnumerable<IBigCommerceOrderItemEntity>>
            {
                { 123, new [] { new BigCommerceOrderItemEntity() } }
            };
        }

        [Fact]
        public async Task UpdateOnline_LogsInfo_WhenNoItemsExistForOrder()
        {
            allItems.Clear();

            await testObject.UpdateOnline(store.Object, orderDetail, "123", shipment, allItems);

            mock.Mock<ILog>()
                .Verify(x => x.InfoFormat(It.Is<string>(s => s.Contains("no items")), orderDetail.OrderNumberComplete));
        }

        [Fact]
        public async Task UpdateOnline_UpdatesOrderStatus_WhenItemsAreAllDigital()
        {
            foreach (var item in allItems.SelectMany(x => x.Value).Cast<BigCommerceOrderItemEntity>())
            {
                item.IsDigitalItem = true;
            }

            var client = mock.FromFactory<IBigCommerceWebClientFactory>()
                .Mock(x => x.Create(store.Object));

            await testObject.UpdateOnline(store.Object, orderDetail, "123", shipment, allItems);

            client.Verify(x => x.UpdateOrderStatus(123, BigCommerceConstants.OrderStatusCompleted));
        }

        [Fact]
        public async Task UpdateOnline_LogsWarning_WhenProductLoadFails()
        {
            mock.Mock<IItemLoader>()
                .Setup(x => x.LoadItems(It.IsAny<IEnumerable<IBigCommerceOrderItemEntity>>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<IBigCommerceWebClient>()))
                .ReturnsAsync(GenericResult.FromError<OnlineItems>("Foo"));

            await testObject.UpdateOnline(store.Object, orderDetail, "123", shipment, allItems);

            mock.Mock<ILog>()
                .Verify(x => x.Warn("Foo"));
        }

        [Fact]
        public async Task UpdateOnline_DelegatesToLoader_ToGetShipmentCarrierDetails()
        {
            await testObject.UpdateOnline(store.Object, orderDetail, "123", shipment, allItems);

            mock.Mock<IItemLoader>()
                .Verify(x => x.GetShippingMethod(shipment));
        }

        [Fact]
        public async Task UpdateOnline_DelegatesToWebClient_ToUploadShipmentDetails()
        {
            shipment.TrackingNumber = "track";
            var items = new[] { new BigCommerceItem() };

            mock.Mock<IItemLoader>()
                .Setup(x => x.LoadItems(It.IsAny<IEnumerable<IBigCommerceOrderItemEntity>>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<IBigCommerceWebClient>()))
                .ReturnsAsync(GenericResult.FromSuccess(new OnlineItems(456, items)));

            var client = mock.FromFactory<IBigCommerceWebClientFactory>()
                .Mock(x => x.Create(store.Object));

            await testObject.UpdateOnline(store.Object, orderDetail, "123", shipment, allItems);

            client.Verify(x => x.UploadOrderShipmentDetails(123, 456, "track", Tuple.Create("carrier", "service"), It.Is<List<BigCommerceItem>>(m => m.SequenceEqual(items))));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
