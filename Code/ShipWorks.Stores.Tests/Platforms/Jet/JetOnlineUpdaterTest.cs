﻿using System;
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
        private readonly JetStoreEntity store;
        private readonly OrderEntity order;
        private JetOnlineUpdater testObject;
        private ShipmentEntity shipment;

        public JetOnlineUpdaterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            store = new JetStoreEntity();

            order = new OrderEntity() {OrderNumber = 42};
            shipment = new ShipmentEntity { Order = order };
            mock.Mock<IOrderManager>()
                .Setup(m => m.GetLatestActiveShipment(It.IsAny<long>()))
                .Returns(() => shipment);
        }

        [Fact]
        public void UpdateShipmentDetailsWithShipment_DoesNotUpdateShipmentDetails_WhenOrderIsManual()
        {
            order.IsManual = true;

            testObject = mock.Create<JetOnlineUpdater>();
            testObject.UpdateShipmentDetails(42, store);

            mock.Mock<IJetWebClient>().Verify(r => r.UploadShipmentDetails(shipment, store), Times.Never);
        }

        [Fact]
        public void UpdateShipmentDetailsWithShipment_CallsUpdateShipmentDetails_WhenOrderIsNotManual()
        {
            testObject = mock.Create<JetOnlineUpdater>();
            testObject.UpdateShipmentDetails(42, store);

            mock.Mock<IJetWebClient>().Verify(r => r.UploadShipmentDetails(shipment, store), Times.Once);
        }

        [Fact]
        public void UpdateShipmentDetailsWithShipment_SetsOnlineStatusToComplete()
        {
            testObject = mock.Create<JetOnlineUpdater>();
            testObject.UpdateShipmentDetails(42, store);

            Assert.Equal("Complete", order.OnlineStatus);
        }

        [Fact]
        public void UpdateShipmentDetailsWithShipment_SavesOrderToRepository()
        {
            testObject = mock.Create<JetOnlineUpdater>();
            testObject.UpdateShipmentDetails(42, store);

            mock.Mock<IOrderRepository>().Verify(r => r.Save(order));
        }

        [Fact]
        public void UpdateShipmentDetailsWithShipment_LogsSqlException()
        {
            var sqlException = UninitializeObjectCreator.Create<SqlException>();
            mock.Mock<IOrderRepository>()
                .Setup(r => r.Save(It.IsAny<OrderEntity>()))
                .Throws(sqlException);

            var log = mock.CreateMock<ILog>();
            mock.MockFunc<Type, ILog>(log);

            testObject = mock.Create<JetOnlineUpdater>();
            Assert.Throws<JetException>(() => testObject.UpdateShipmentDetails(42, store));

            string errorMessage = "Error saving online status for order 42.";
            log.Verify(l => l.Error(errorMessage, sqlException), Times.Once);
        }

        [Fact]
        public void UpdateShipmentDetailsWithShipment_ThrowsExpectedException()
        {
            var sqlException = Instantiate<SqlException>();
            mock.Mock<IOrderRepository>()
                .Setup(r => r.Save(It.IsAny<OrderEntity>()))
                .Throws(sqlException);

            var log = mock.CreateMock<ILog>();
            mock.MockFunc<Type, ILog>(log);

            testObject = mock.Create<JetOnlineUpdater>();
            var exception = Assert.Throws<JetException>(() => testObject.UpdateShipmentDetails(42, store));

            string errorMessage = "Error saving online status for order 42.";
            Assert.Equal(errorMessage, exception.Message);
            Assert.Equal(sqlException, exception.InnerException);
        }

        [Fact]
        public void UpdateShipmentDetails_DoesNotUploadShipmentDetail_IfOrderNotFound()
        {
            shipment = null;

            testObject = mock.Create<JetOnlineUpdater>();
            testObject.UpdateShipmentDetails(22, store);

            mock.Mock<IJetWebClient>().Verify(r => r.UploadShipmentDetails(shipment, store), Times.Never);
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