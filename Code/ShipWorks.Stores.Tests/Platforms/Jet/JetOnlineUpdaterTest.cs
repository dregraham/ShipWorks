using System;
using System.Data.SqlClient;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Jet;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JetOnlineUpdaterTest : IDisposable
    {
        private readonly AutoMock mock;

        public JetOnlineUpdaterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void UpdateShipmentDetailsWithShipment_PopulateOrderDetailsCalledWithOrder()
        {
            var order = new OrderEntity();
            var shipment = new ShipmentEntity { Order = order };
            var testObject = mock.Create<JetOnlineUpdater>();
            testObject.UpdateShipmentDetails(shipment);

            mock.Mock<IOrderRepository>().Verify(r => r.PopulateOrderDetails(order), Times.Once);
        }

        [Fact]
        public void UpdateShipmentDetailsWithShipment_DoesNotUpdateShipmentDetails_WhenOrderIsManual()
        {
            var order = new OrderEntity { IsManual = true };
            var shipment = new ShipmentEntity { Order = order };
            var testObject = mock.Create<JetOnlineUpdater>();
            testObject.UpdateShipmentDetails(shipment);

            mock.Mock<IJetWebClient>().Verify(r => r.UpdateShipmentDetails(shipment), Times.Never);
        }

        [Fact]
        public void UpdateShipmentDetailsWithShipment_CallsUpdateShipmentDetails_WhenOrderIsNotManual()
        {
            var order = new OrderEntity();
            var shipment = new ShipmentEntity { Order = order };
            var testObject = mock.Create<JetOnlineUpdater>();
            testObject.UpdateShipmentDetails(shipment);

            mock.Mock<IJetWebClient>().Verify(r => r.UpdateShipmentDetails(shipment), Times.Once);
        }

        [Fact]
        public void UpdateShipmentDetailsWithShipment_SetsOnlineStatusToComplete()
        {
            var order = new OrderEntity();
            var shipment = new ShipmentEntity { Order = order };
            var testObject = mock.Create<JetOnlineUpdater>();
            testObject.UpdateShipmentDetails(shipment);

            Assert.Equal("Complete", order.OnlineStatus);
        }

        [Fact]
        public void UpdateShipmentDetailsWithShipment_SavesOrderToRepository()
        {
            var order = new OrderEntity();
            var shipment = new ShipmentEntity { Order = order };
            var testObject = mock.Create<JetOnlineUpdater>();
            testObject.UpdateShipmentDetails(shipment);

            mock.Mock<IOrderRepository>().Verify(r => r.Save(order));
        }

        [Fact]
        public void UpdateShipmentDetailsWithShipment_LogsSqlException()
        {
            var order = new OrderEntity { OrderNumber = 42 };
            var shipment = new ShipmentEntity { Order = order };
            
            var sqlException = UninitializeObjectCreator.Create<SqlException>();
            mock.Mock<IOrderRepository>()
                .Setup(r => r.Save(It.IsAny<OrderEntity>()))
                .Throws(sqlException);

            var log = mock.CreateMock<ILog>();
            mock.MockFunc<Type, ILog>(log);

            var testObject = mock.Create<JetOnlineUpdater>();
            Assert.Throws<JetException>(() => testObject.UpdateShipmentDetails(shipment));

            string errorMessage = "Error saving online status for order 42.";
            log.Verify(l => l.Error(errorMessage, sqlException), Times.Once);
        }

        [Fact]
        public void UpdateShipmentDetailsWithShipment_ThrowsExpectedException()
        {
            var order = new OrderEntity { OrderNumber = 42 };
            var shipment = new ShipmentEntity { Order = order };

            var sqlException = Instantiate<SqlException>();
            mock.Mock<IOrderRepository>()
                .Setup(r => r.Save(It.IsAny<OrderEntity>()))
                .Throws(sqlException);

            var log = mock.CreateMock<ILog>();
            mock.MockFunc<Type, ILog>(log);

            var testObject = mock.Create<JetOnlineUpdater>();
            var exception = Assert.Throws<JetException>(() => testObject.UpdateShipmentDetails(shipment));

            string errorMessage = "Error saving online status for order 42.";
            Assert.Equal(errorMessage, exception.Message);
            Assert.Equal(sqlException, exception.InnerException);
        }

        [Fact]
        public void UpdateShipmentDetailsWithOrderId_UpdatesShipmentWhenShipmentFound()
        {
            var order = new OrderEntity { OrderNumber = 42 };
            var shipment = new ShipmentEntity { Order = order };

            mock.Mock<IOrderManager>()
                .Setup(m => m.GetLatestActiveShipment(22))
                .Returns(shipment);

            var testObject = mock.Create<JetOnlineUpdater>();
            testObject.UpdateShipmentDetails(22);

            mock.Mock<IOrderRepository>().Verify(r => r.PopulateOrderDetails(order), Times.Once);
        }

        [Fact]
        public void UpdateShipmentDetailsWithOrderId_DoesNotPoulateOrder_IfOrderNotFound()
        {
            mock.Mock<IOrderManager>()
                .Setup(m => m.GetLatestActiveShipment(22))
                .Returns<ShipmentEntity>(null);

            var testObject = mock.Create<JetOnlineUpdater>();
            testObject.UpdateShipmentDetails(22);

            mock.Mock<IOrderRepository>().Verify(r => r.PopulateOrderDetails(It.IsAny<OrderEntity>()), Times.Never);
        }

        public static T Instantiate<T>() where T : class
        {
            return System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(T)) as T;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}