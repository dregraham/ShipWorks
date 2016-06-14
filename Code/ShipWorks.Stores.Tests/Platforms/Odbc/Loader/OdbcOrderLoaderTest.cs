﻿using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Loader
{
    public class OdbcOrderLoaderTest
    {
        [Fact]
        public void Load_CalculateTotalCalled_WhenOrderIsNew()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var orderUtility = mock.Mock<IOrderChargeCalculator>();
                var fieldMap = mock.Mock<IOdbcFieldMap>();
                var orderEntity = new OrderEntity() {IsNew = true};
                var odbcRecords = new[] {new OdbcRecord()};
                var testObject = mock.Create<OdbcOrderLoader>();

                testObject.Load(fieldMap.Object, orderEntity, odbcRecords);

                orderUtility.Verify(u => u.CalculateTotal(orderEntity), Times.Once);
            }
        }

        [Fact]
        public void Load_CalculateTotalNotCalled_WhenOrderIsNotNew()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var orderUtility = mock.Mock<IOrderChargeCalculator>();
                var fieldMap = mock.Mock<IOdbcFieldMap>();
                var orderEntity = new OrderEntity() {IsNew = false};
                var odbcRecords = new[] {new OdbcRecord()};
                var testObject = mock.Create<OdbcOrderLoader>();

                testObject.Load(fieldMap.Object, orderEntity, odbcRecords);

                orderUtility.Verify(u => u.CalculateTotal(orderEntity), Times.Never);
            }
        }

        [Fact]
        public void Load_CopyToEntityCalled()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var map = mock.Mock<IOdbcFieldMap>();
                var order = new OrderEntity() {IsNew = false};
                var odbcRecords = new[] {new OdbcRecord()};
                var testObject = mock.Create<OdbcOrderLoader>();

                testObject.Load(map.Object, order, odbcRecords);

                map.Verify(m => m.CopyToEntity(order));
            }
        }

        [Fact]
        public void Load_LoadCalledOnEachOrderDetailLoader()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var fieldMap = mock1.Mock<IOdbcFieldMap>();
                var orderEntity = new OrderEntity() {IsNew = false};
                var odbcRecords = new[] {new OdbcRecord()};

                var detailLoader1 = mock1.Mock<IOdbcOrderDetailLoader>();
                var detailLoader2 = mock2.Mock<IOdbcOrderDetailLoader>();
                mock1.Provide(detailLoader2.Object);

                var testObject = mock1.Create<OdbcOrderLoader>();

                testObject.Load(fieldMap.Object, orderEntity, odbcRecords);

                detailLoader1.Verify(d => d.Load(fieldMap.Object, orderEntity));
                detailLoader2.Verify(d => d.Load(fieldMap.Object, orderEntity));
            }
        }

        [Fact]
        public void Load_OrderItemLoadCalled_WhenOrderIsNew()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var orderItemLoader = mock.Mock<IOdbcOrderItemLoader>();

                var fieldMap = mock.Mock<IOdbcFieldMap>();
                var orderEntity = new OrderEntity() {IsNew = true};
                var odbcRecords = new[] {new OdbcRecord()};
                var testObject = mock.Create<OdbcOrderLoader>();

                testObject.Load(fieldMap.Object, orderEntity, odbcRecords);

                orderItemLoader.Verify(l => l.Load(fieldMap.Object, orderEntity, odbcRecords), Times.Once);
            }
        }

        [Fact]
        public void Load_OrderItemLoadNotCalled_WhenOrderIsNotNew()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var orderItemLoader = mock.Mock<IOdbcOrderItemLoader>();

                var fieldMap = mock.Mock<IOdbcFieldMap>();
                var orderEntity = new OrderEntity() {IsNew = false};
                var odbcRecords = new[] {new OdbcRecord()};
                var testObject = mock.Create<OdbcOrderLoader>();

                testObject.Load(fieldMap.Object, orderEntity, odbcRecords);

                orderItemLoader.Verify(l => l.Load(fieldMap.Object, orderEntity, odbcRecords), Times.Never);
            }
        }
    }
}