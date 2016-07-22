#region

using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using System;
using ShipWorks.Stores.Content;
using Xunit;

#endregion

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Upload
{
    public class OdbcUploaderTest : IDisposable
    {
        public OdbcUploaderTest()
        {
            mock = AutoMock.GetLoose();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        private readonly AutoMock mock;
        private Mock<ICarrierShipmentAdapter> shipmentAdapter;
        private Mock<IShippingManager> shippingManager;
        private Mock<IOrderManager> orderManager;
        private Mock<IOdbcUploadCommandFactory> commandFactory;

        private void SetupOrderManagerToGetLatestActiveShipment()
        {
            orderManager = mock.Mock<IOrderManager>();
            orderManager.Setup(m => m.GetLatestActiveShipment(It.IsAny<long>())).Returns(new ShipmentEntity() {Processed = true});
        }

        private void SetupCommand(int rowsAffected)
        {
            var uploadCommand = mock.Mock<IOdbcUploadCommand>();
            uploadCommand.Setup(c => c.Execute()).Returns(rowsAffected);

            commandFactory = mock.Mock<IOdbcUploadCommandFactory>();
            commandFactory.Setup(f => f.CreateUploadCommand(It.IsAny<OdbcStoreEntity>(), It.IsAny<IOdbcFieldMap>()))
                .Returns(uploadCommand.Object);
        }

        private void SetupShippingManagerToGetShipment(ShipmentEntity shipmentToReturn)
        {
            shipmentAdapter = mock.MockRepository.Create<ICarrierShipmentAdapter>();
            shipmentAdapter.SetupGet(a => a.Shipment).Returns(shipmentToReturn);

            shippingManager = mock.Mock<IShippingManager>();
            shippingManager.Setup(m => m.GetShipment(It.IsAny<long>())).Returns(shipmentAdapter.Object);
        }

        [Fact]
        public void UploadLatestShipment_ShipWorksOdbcExceptionThrown_WhenNoRowsAffected()
        {
            SetupCommand(0);
            SetupOrderManagerToGetLatestActiveShipment();

            OdbcUploader testObject = mock.Create<OdbcUploader>();

            Assert.Throws<ShipWorksOdbcException>(()=>testObject.UploadLatestShipment(new OdbcStoreEntity(), 2));
        }

        [Fact]
        public void UploadLatestShipment_CommandCreatedWithStoreAndFieldMap()
        {
            SetupCommand(5);
            SetupOrderManagerToGetLatestActiveShipment();
            var store = new OdbcStoreEntity();
            var fieldMap = mock.Mock<IOdbcFieldMap>();

            OdbcUploader testObject = mock.Create<OdbcUploader>();
            testObject.UploadLatestShipment(store, 2);

            commandFactory.Verify(f=>f.CreateUploadCommand(store, fieldMap.Object));
        }

        [Fact]
        public void UploadLatestShipment_FieldMapLoadCalledOnceWithCorrectFieldMap()
        {
            string fieldMapText = "I'm a map!!!";

            SetupOrderManagerToGetLatestActiveShipment();
            SetupCommand(5);

            var fieldMap = mock.Mock<IOdbcFieldMap>();

            OdbcUploader testObject = mock.Create<OdbcUploader>();
            testObject.UploadLatestShipment(new OdbcStoreEntity {UploadMap = fieldMapText}, 2);

            fieldMap.Verify(f => f.Load(fieldMapText), Times.Once);
            fieldMap.Verify(f => f.Load(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void UploadLatestShipment_GetsShipment()
        {
            SetupCommand(5);
            SetupOrderManagerToGetLatestActiveShipment();

            OdbcUploader testObject = mock.Create<OdbcUploader>();
            testObject.UploadLatestShipment(new OdbcStoreEntity(), 5);

            orderManager.Verify(m => m.GetLatestActiveShipment(5), Times.Once);
            orderManager.Verify(m => m.GetLatestActiveShipment(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void UploadShipments_FieldMapLoadCalledOnceWithCorrectFieldMap()
        {
            string fieldMapText = "I'm a map!!!";

            SetupShippingManagerToGetShipment(new ShipmentEntity());
            SetupCommand(5);

            var fieldMap = mock.Mock<IOdbcFieldMap>();

            OdbcUploader testObject = mock.Create<OdbcUploader>();
            testObject.UploadShipments(new OdbcStoreEntity {UploadMap = fieldMapText}, new long[] {1, 2});

            fieldMap.Verify(f => f.Load(fieldMapText), Times.Once);
            fieldMap.Verify(f => f.Load(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void UploadShipments_GetsEachShipment()
        {
            SetupShippingManagerToGetShipment(new ShipmentEntity());
            SetupCommand(5);

            OdbcUploader testObject = mock.Create<OdbcUploader>();
            testObject.UploadShipments(new OdbcStoreEntity(), new long[] {1, 2});

            shippingManager.Verify(m => m.GetShipment(1), Times.Once);
            shippingManager.Verify(m => m.GetShipment(2), Times.Once);
            shippingManager.Verify(m => m.GetShipment(It.IsAny<long>()), Times.Exactly(2));
        }
    }
}