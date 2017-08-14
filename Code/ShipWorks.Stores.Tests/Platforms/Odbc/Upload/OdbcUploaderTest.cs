using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Stores.Content;
using Xunit;
using System.Threading.Tasks;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Upload
{
    public class OdbcUploaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private Mock<ICarrierShipmentAdapter> shipmentAdapter;
        private Mock<IShippingManager> shippingManager;
        private Mock<IOrderManager> orderManager;
        private Mock<IOdbcUploadCommandFactory> commandFactory;
        private readonly ShipmentEntity shipment;

        public OdbcUploaderTest()
        {
            mock = AutoMock.GetLoose();

            Mock<ICombineOrderSearchProvider<string>>  combinedOrderSearchProvider = new Mock<ICombineOrderSearchProvider<string>>();
            combinedOrderSearchProvider.Setup(sp => sp.GetOrderIdentifiers(It.IsAny<IOrderEntity>())).Returns(Task.FromResult(new[] { "1" }.AsEnumerable()));
            mock.Provide(combinedOrderSearchProvider.Object);

            shipment = new ShipmentEntity()
            {
                Processed = true,
                Order = new OrderEntity()
                {
                    OrderNumber = 1
                }
            };
        }

        [Fact]
        public async Task UploadLatestShipment_ShipWorksOdbcExceptionThrown_WhenNoRowsAffected()
        {
            SetupCommand(0);
            SetupOrderManagerToGetLatestActiveShipment();

            OdbcUploader testObject = mock.Create<OdbcUploader>();

            await Assert.ThrowsAsync<ShipWorksOdbcException>(async () => await testObject.UploadLatestShipment(new OdbcStoreEntity(), 2).ConfigureAwait(false));
        }

        [Fact]
        public async Task UploadLatestShipment_CommandCreatedWithStoreAndFieldMap()
        {
            SetupCommand(5);
            SetupOrderManagerToGetLatestActiveShipment();
            var store = new OdbcStoreEntity();

            OdbcUploader testObject = mock.Create<OdbcUploader>();
            await testObject.UploadLatestShipment(store, 2);

            commandFactory.Verify(f => f.CreateUploadCommand(store, It.IsAny<ShipmentEntity>()));
        }

        [Fact]
        public async Task UploadLatestShipment_GetsShipment()
        {
            SetupCommand(5);
            SetupOrderManagerToGetLatestActiveShipment();

            OdbcUploader testObject = mock.Create<OdbcUploader>();
            await testObject.UploadLatestShipment(new OdbcStoreEntity(), 5);

            orderManager.Verify(m => m.GetLatestActiveShipment(5), Times.Once);
            orderManager.Verify(m => m.GetLatestActiveShipment(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async Task UploadShipments_GetsEachShipment()
        {
            SetupShippingManagerToGetShipment(shipment);
            SetupCommand(5);

            OdbcUploader testObject = mock.Create<OdbcUploader>();
            await testObject.UploadShipments(new OdbcStoreEntity(), new long[] { 1, 2 }).ConfigureAwait(false);

            shippingManager.Verify(m => m.GetShipment(1), Times.Once);
            shippingManager.Verify(m => m.GetShipment(2), Times.Once);
            shippingManager.Verify(m => m.GetShipment(It.IsAny<long>()), Times.Exactly(2));
        }

        [Theory]
        [InlineData(CombineSplitStatusType.None, 2)]
        [InlineData(CombineSplitStatusType.Combined, 4)]
        public async Task UploadShipments_CallsUploadCorrectNumberOfTimes(CombineSplitStatusType combineSplitStatusType, int expectedCallCount)
        {
            SetupShippingManagerToGetShipment(shipment);
            SetupCommand(5);
            shipment.Order.CombineSplitStatus = combineSplitStatusType;

            IEnumerable<string> combinedOrderIDs = combineSplitStatusType == CombineSplitStatusType.None ? new List<string> {"1"} : new List<string> { "1", "2" };

            Mock<ICombineOrderSearchProvider<string>> combinedOrderSearchProvider = new Mock<ICombineOrderSearchProvider<string>>();
            combinedOrderSearchProvider.Setup(sp => sp.GetOrderIdentifiers(It.IsAny<IOrderEntity>())).Returns(Task.FromResult(combinedOrderIDs));
            mock.Provide(combinedOrderSearchProvider.Object);
            shipment.Order.CombineSplitStatus = CombineSplitStatusType.None;

            OdbcUploader testObject = mock.Create<OdbcUploader>();
            await testObject.UploadShipments(new OdbcStoreEntity(), new long[] { 1, 2 }).ConfigureAwait(false);

            commandFactory.Verify(f => f.CreateUploadCommand(It.IsAny<OdbcStoreEntity>(), It.IsAny<ShipmentEntity>()), Times.Exactly(expectedCallCount));
        }

        private void SetupOrderManagerToGetLatestActiveShipment()
        {
            shipment.Order.ChangeOrderNumber("1");

            orderManager = mock.Mock<IOrderManager>();
            orderManager.Setup(m => m.GetLatestActiveShipment(It.IsAny<long>())).Returns(shipment);
        }

        private void SetupCommand(int rowsAffected)
        {
            var uploadCommand = mock.Mock<IOdbcUploadCommand>();
            uploadCommand.Setup(c => c.Execute()).Returns(rowsAffected);

            commandFactory = mock.Mock<IOdbcUploadCommandFactory>();
            commandFactory.Setup(f => f.CreateUploadCommand(It.IsAny<OdbcStoreEntity>(), It.IsAny<ShipmentEntity>()))
                .Returns(uploadCommand.Object);
        }

        private void SetupShippingManagerToGetShipment(ShipmentEntity shipmentToReturn)
        {
            shipmentAdapter = mock.MockRepository.Create<ICarrierShipmentAdapter>();
            shipmentAdapter.SetupGet(a => a.Shipment).Returns(shipmentToReturn);

            shippingManager = mock.Mock<IShippingManager>();
            shippingManager.Setup(m => m.GetShipment(It.IsAny<long>())).Returns(shipmentAdapter.Object);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}