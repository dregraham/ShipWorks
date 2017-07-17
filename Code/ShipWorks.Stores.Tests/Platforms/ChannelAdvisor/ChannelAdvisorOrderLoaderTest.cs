using System;
using System.Collections.Generic;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ChannelAdvisor
{
    public class ChannelAdvisorOrderLoaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ChannelAdvisorOrderEntity orderToSave;
        private readonly ChannelAdvisorOrder downloadedOrder;
        private readonly Mock<IOrderElementFactory> orderElementFactory;
        private readonly ChannelAdvisorOrderLoader testObject;
        private readonly Mock<IChannelAdvisorRestClient> channelAdvisorRestClient;


        public ChannelAdvisorOrderLoaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            orderToSave = new ChannelAdvisorOrderEntity();
            downloadedOrder = new ChannelAdvisorOrder();
            downloadedOrder.Fulfillments = new List<ChannelAdvisorFulfillment>();
            downloadedOrder.Items = new List<ChannelAdvisorOrderItem>();
            orderElementFactory = mock.Mock<IOrderElementFactory>();
            channelAdvisorRestClient = mock.Mock<IChannelAdvisorRestClient>();
            testObject = mock.Create<ChannelAdvisorOrderLoader>();

            channelAdvisorRestClient.Setup(c => c.GetProduct(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new ChannelAdvisorProduct()
                {
                    Attributes = new List<ChannelAdvisorProductAttribute>(),
                    Images = new List<ChannelAdvisorProductImage>()
                });

            orderElementFactory.Setup(f => f.CreateItem(It.IsAny<OrderEntity>()))
                .Returns<OrderEntity>(order =>
                {
                    order.OrderItems.Add(new ChannelAdvisorOrderItemEntity());
                    return order.OrderItems[0];
                });
        }

        #region Order Level Tests
        [Fact]
        public void LoadOrder_OrderNumberIsSet()
        {
            downloadedOrder.ID = 123;
            
            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal(123, orderToSave.OrderNumber);
        }
        #endregion

        #region Order Item Level Tests

        [Fact]
        public void LoadOrder_OrderItemNameIsSet()
        {
            downloadedOrder.Items.Add(new ChannelAdvisorOrderItem() {Title = "My Title"});

            testObject.LoadOrder(orderToSave, downloadedOrder, orderElementFactory.Object, string.Empty);

            Assert.Equal(downloadedOrder.Items[0].Title, orderToSave.OrderItems[0].Name);
        }

        #endregion

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}