using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Stores.Platforms.Jet.OnlineUpdating;
using ShipWorks.Tests.Shared;
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

            order = new OrderEntity() { OrderNumber = 42 };
            shipment = new ShipmentEntity { Order = order };
            mock.Mock<IOrderManager>()
                .Setup(m => m.GetLatestActiveShipment(It.IsAny<long>()))
                .Returns(() => shipment);

            mock.Mock<IJetOrderSearchProvider>()
                .Setup(x => x.GetOrderIdentifiers(It.IsAny<IOrderEntity>()))
                .ReturnsAsync(new[] { "foo" });
        }

        [Fact]
        public async Task UpdateShipmentDetailsWithShipment_DelegatesToOrderSearchProvider()
        {
            testObject = mock.Create<JetOnlineUpdater>();
            await testObject.UpdateShipmentDetails(42, store);

            mock.Mock<IJetOrderSearchProvider>()
                .Verify(x => x.GetOrderIdentifiers(order));
        }

        [Fact]
        public async Task UpdateShipmentDetailsWithShipment_DoesNotSaveOrder_WhenNoOrdersFound()
        {
            mock.Mock<IJetOrderSearchProvider>()
                .Setup(x => x.GetOrderIdentifiers(It.IsAny<IOrderEntity>()))
                .ReturnsAsync(Enumerable.Empty<string>());

            testObject = mock.Create<JetOnlineUpdater>();
            await testObject.UpdateShipmentDetails(42, store);

            mock.Mock<IOrderRepository>()
                .Verify(x => x.Save(It.IsAny<OrderEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateShipmentDetailsWithShipment_UploadsMultipleTimes_WhenOrderWasCombined()
        {
            mock.Mock<IJetOrderSearchProvider>()
                .Setup(x => x.GetOrderIdentifiers(It.IsAny<IOrderEntity>()))
                .ReturnsAsync(new[] { "foo", "bar" });

            testObject = mock.Create<JetOnlineUpdater>();
            await testObject.UpdateShipmentDetails(42, store);

            mock.Mock<IJetWebClient>().Verify(r => r.UploadShipmentDetails("foo", shipment, store));
            mock.Mock<IJetWebClient>().Verify(r => r.UploadShipmentDetails("bar", shipment, store));
        }

        [Fact]
        public async Task UpdateShipmentDetailsWithShipment_CallsUpdateShipmentDetails_WhenOrderIsNotManual()
        {
            testObject = mock.Create<JetOnlineUpdater>();
            await testObject.UpdateShipmentDetails(42, store);

            mock.Mock<IJetWebClient>().Verify(r => r.UploadShipmentDetails(It.IsAny<string>(), shipment, store), Times.Once);
        }

        [Fact]
        public async Task UpdateShipmentDetailsWithShipment_SetsOnlineStatusToComplete()
        {
            testObject = mock.Create<JetOnlineUpdater>();
            await testObject.UpdateShipmentDetails(42, store);

            Assert.Equal("Complete", order.OnlineStatus);
        }

        [Fact]
        public async Task UpdateShipmentDetailsWithShipment_SavesOrderToRepository()
        {
            testObject = mock.Create<JetOnlineUpdater>();
            await testObject.UpdateShipmentDetails(42, store);

            mock.Mock<IOrderRepository>().Verify(r => r.Save(order));
        }

        [Fact]
        public async Task UpdateShipmentDetailsWithShipment_LogsSqlException()
        {
            var sqlException = UninitializeObjectCreator.Create<SqlException>();
            mock.Mock<IOrderRepository>()
                .Setup(r => r.Save(It.IsAny<OrderEntity>()))
                .Throws(sqlException);

            var log = mock.CreateMock<ILog>();
            mock.MockFunc<Type, ILog>(log);

            testObject = mock.Create<JetOnlineUpdater>();
            await Assert.ThrowsAsync<JetException>(() => testObject.UpdateShipmentDetails(42, store));

            string errorMessage = "Error saving online status for order 42.";
            log.Verify(l => l.Error(errorMessage, sqlException), Times.Once);
        }

        [Fact]
        public async Task UpdateShipmentDetailsWithShipment_ThrowsExpectedException()
        {
            var sqlException = Instantiate<SqlException>();
            mock.Mock<IOrderRepository>()
                .Setup(r => r.Save(It.IsAny<OrderEntity>()))
                .Throws(sqlException);

            var log = mock.CreateMock<ILog>();
            mock.MockFunc<Type, ILog>(log);

            testObject = mock.Create<JetOnlineUpdater>();
            var exception = await Assert.ThrowsAsync<JetException>(() => testObject.UpdateShipmentDetails(42, store));

            string errorMessage = "Error saving online status for order 42.";
            Assert.Equal(errorMessage, exception.Message);
            Assert.Equal(sqlException, exception.InnerException);
        }

        [Fact]
        public async Task UpdateShipmentDetails_DoesNotUploadShipmentDetail_IfOrderNotFound()
        {
            shipment = null;

            testObject = mock.Create<JetOnlineUpdater>();
            await testObject.UpdateShipmentDetails(22, store);

            mock.Mock<IJetWebClient>().Verify(r => r.UploadShipmentDetails(It.IsAny<string>(), shipment, store), Times.Never);
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