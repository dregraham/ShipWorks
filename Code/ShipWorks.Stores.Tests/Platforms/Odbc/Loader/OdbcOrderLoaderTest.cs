using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
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
                var odbcRecords = new[] {new OdbcRecord(string.Empty)};
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
                var odbcRecords = new[] {new OdbcRecord(string.Empty)};
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
                var odbcRecords = new[] {new OdbcRecord(string.Empty)};
                var testObject = mock.Create<OdbcOrderLoader>();

                testObject.Load(map.Object, order, odbcRecords);

                map.Verify(m => m.CopyToEntity(order));
            }
        }

        [Fact]
        public void Load_OrderDateIsSetToNow_WhenOrderIsNew_AndDateNotMapped()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var now = new DateTime(2016, 4, 20, 16, 20, 1, DateTimeKind.Utc);

                var fieldMap = mock.Mock<IOdbcFieldMap>();
                var orderEntity = new OrderEntity() { IsNew = true };
                var odbcRecords = new[] { new OdbcRecord(string.Empty) };
                var testObject = mock.Create<OdbcOrderLoader>();

                mock.Mock<IDateTimeProvider>().Setup(d => d.UtcNow).Returns(now);

                testObject.Load(fieldMap.Object, orderEntity, odbcRecords);

                Assert.Equal(now, orderEntity.OrderDate);
            }
        }

        [Fact]
        public void Load_OrderDateIsSetToMappedValue_WhenOrderIsNew_AndDateMapped()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var now = new DateTime(2016, 4, 20, 16, 20, 1, DateTimeKind.Utc);
                var mappedDate = new DateTime(1999, 4, 20, 16, 20, 1, DateTimeKind.Utc);


                var fieldMap = mock.Mock<IOdbcFieldMap>();
                var orderEntity = new OrderEntity() { IsNew = true };
                fieldMap.Setup(m => m.CopyToEntity(orderEntity))
#pragma warning disable S3215 // "interface" instances should not be cast to concrete types
                    .Callback<IEntity2>(entity2 => ((OrderEntity) entity2).OrderDate = mappedDate);
#pragma warning restore S3215 // "interface" instances should not be cast to concrete types

                var odbcRecords = new[] { new OdbcRecord(string.Empty) };
                var testObject = mock.Create<OdbcOrderLoader>();

                mock.Mock<IDateTimeProvider>().Setup(d => d.UtcNow).Returns(now);

                testObject.Load(fieldMap.Object, orderEntity, odbcRecords);

                Assert.Equal(mappedDate, orderEntity.OrderDate);
            }
        }

        [Fact]
        public void Load_OrderDateNotSet_WhenOrderIsNotNew_AndDateNotMapped()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var fieldMap = mock.Mock<IOdbcFieldMap>();
                var orderEntity = new OrderEntity() {IsNew = false};
                var odbcRecords = new[] {new OdbcRecord(string.Empty)};
                var testObject = mock.Create<OdbcOrderLoader>();

                mock.Mock<IDateTimeProvider>().Setup(d => d.UtcNow).Returns(DateTime.Now);

                testObject.Load(fieldMap.Object, orderEntity, odbcRecords);

                Assert.False(orderEntity.Fields[(int) OrderFieldIndex.OrderDate].IsChanged);
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
                var odbcRecords = new[] {new OdbcRecord(string.Empty)};

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
                var odbcRecords = new[] {new OdbcRecord(string.Empty)};
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
                var odbcRecords = new[] {new OdbcRecord(string.Empty)};
                var testObject = mock.Create<OdbcOrderLoader>();

                testObject.Load(fieldMap.Object, orderEntity, odbcRecords);

                orderItemLoader.Verify(l => l.Load(fieldMap.Object, orderEntity, odbcRecords), Times.Never);
            }
        }

        [Fact]
        public void Load_SetsBillingCountryCodeToUS_WhenCountryCodeIsEmptryString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var fieldMap = mock.Mock<IOdbcFieldMap>();
                var orderEntity = new OrderEntity() { BillCountryCode = string.Empty };
                var odbcRecords = new[] { new OdbcRecord(string.Empty) };
                var testObject = mock.Create<OdbcOrderLoader>();

                testObject.Load(fieldMap.Object, orderEntity, odbcRecords);

                Assert.Equal("US", orderEntity.BillCountryCode);
            }
        }

        [Fact]
        public void Load_SetsShippingCountryCodeToUS_WhenCountryCodeIsEmptryString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var fieldMap = mock.Mock<IOdbcFieldMap>();
                var orderEntity = new OrderEntity() { ShipCountryCode = string.Empty };
                var odbcRecords = new[] { new OdbcRecord(string.Empty) };
                var testObject = mock.Create<OdbcOrderLoader>();

                testObject.Load(fieldMap.Object, orderEntity, odbcRecords);

                Assert.Equal("US", orderEntity.ShipCountryCode);
            }
        }
    }
}