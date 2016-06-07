using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcOrderDateLoaderTest
    {
        [Fact]
        public void Load_LastModifiedDateSetToNow_WhenLastModifiedNotInitialized()
        {
            DateTime utcNow = DateTime.Parse("1/1/2016 4:55:04 PM");
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDateTimeProvider>().Setup(d => d.UtcNow).Returns(utcNow);
                var order = new OrderEntity() {IsNew = false};
                var map = mock.Mock<IOdbcFieldMap>();

                var testObject = mock.Create<OdbcOrderDateLoader>();
                testObject.Load(map.Object, order);

                Assert.Equal(utcNow, order.OnlineLastModified);
            }
        }

        [Fact]
        public void Load_LastModifiedDateStaysTheSame_WhenLastModifiedDateInitialized()
        {
            DateTime initialLastModifiedDate = DateTime.Parse("1/1/2016 4:55:04 PM");
            DateTime utcNow = DateTime.Parse("1/1/2017 4:55:04 PM");
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDateTimeProvider>().Setup(d => d.UtcNow).Returns(utcNow);
                var order = new OrderEntity() {OnlineLastModified = initialLastModifiedDate};
                var map = mock.Mock<IOdbcFieldMap>();

                var testObject = mock.Create<OdbcOrderDateLoader>();
                testObject.Load(map.Object, order);

                Assert.Equal(initialLastModifiedDate, order.OnlineLastModified);
            }
        }

        [Fact]
        public void Load_OrderDateSetToNow_WhenNotInitialized_AndDateFieldsNotMapped()
        {
            DateTime utcNow = DateTime.Parse("1/1/2016 4:55:04 PM");
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDateTimeProvider>().Setup(d => d.UtcNow).Returns(utcNow);
                var order = new OrderEntity { IsNew = false };
                var map = mock.Mock<IOdbcFieldMap>();

                var testObject = mock.Create<OdbcOrderDateLoader>();
                testObject.Load(map.Object, order);

                Assert.Equal(utcNow, order.OrderDate);
            }
        }

        [Fact]
        public void Load_OrderDateStaysTheSame_WhenInitialized_AndDateFieldsNotMapped()
        {
            DateTime initialOrderDate = DateTime.Parse("1/1/2016 4:55:04 PM");
            DateTime utcNow = DateTime.Parse("1/1/2017 4:55:04 PM");
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDateTimeProvider>().Setup(d => d.UtcNow).Returns(utcNow);
                var order = new OrderEntity() { OrderDate = initialOrderDate };
                var map = mock.Mock<IOdbcFieldMap>();

                var testObject = mock.Create<OdbcOrderDateLoader>();
                testObject.Load(map.Object, order);

                Assert.Equal(initialOrderDate, order.OrderDate);
            }
        }

        [Fact]
        public void Load_OrderDateSaved_WhenOrderDateTimeInMap()
        {
            DateTime mappedOrderDate = DateTime.Parse("1/1/2016 4:55:04 PM");
            using (var mock = AutoMock.GetLoose())
            {
                var orderEntity = new OrderEntity() { IsNew = false };
                var fieldMap = mock.Mock<IOdbcFieldMap>();

                fieldMap.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>(), false))
                    .Returns(new List<IOdbcFieldMapEntry>()
                    {
                        GetOdbcFieldMapEntry(mock, ShipWorksOdbcMappableField.OrderDateAndTimeDisplayName, mappedOrderDate)
                    });

                var testObject = mock.Create<OdbcOrderDateLoader>();

                testObject.Load(fieldMap.Object, orderEntity);

                Assert.Equal(mappedOrderDate, orderEntity.OrderDate);
            }
        }

        [Fact]
        public void Load_OrderDateSaved_WhenOrderDateAndOrderTimeInMap()
        {
            DateTime expectedOrderDate = DateTime.Parse("1/1/2016 4:55:04 PM");
            DateTime mappedDate = DateTime.Parse("1/1/2016");
            DateTime mappedTime = DateTime.Parse("4:55:04 PM");

            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var orderEntity = new OrderEntity() { IsNew = false };
                var fieldMap = mock1.Mock<IOdbcFieldMap>();

                fieldMap.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>(), false))
                    .Returns(new List<IOdbcFieldMapEntry>()
                    {
                        GetOdbcFieldMapEntry(mock1, ShipWorksOdbcMappableField.OrderDateDisplayName, mappedDate),
                        GetOdbcFieldMapEntry(mock2, ShipWorksOdbcMappableField.OrderTimeDisplayName, mappedTime)
                    });

                var testObject = mock1.Create<OdbcOrderDateLoader>();

                testObject.Load(fieldMap.Object, orderEntity);

                Assert.Equal(expectedOrderDate, orderEntity.OrderDate);
            }
        }

        [Fact]
        public void Load_OrderDateSaved_WhenOrderDateInMap_AndOrderTimeNotInMap()
        {
            DateTime mappedDate = DateTime.Parse("1/1/2016");

            using (var mock1 = AutoMock.GetLoose())
            {
                var orderEntity = new OrderEntity() { IsNew = false };
                var fieldMap = mock1.Mock<IOdbcFieldMap>();

                fieldMap.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>(), false))
                    .Returns(new List<IOdbcFieldMapEntry>()
                    {
                        GetOdbcFieldMapEntry(mock1, ShipWorksOdbcMappableField.OrderDateDisplayName, mappedDate)
                    });

                var testObject = mock1.Create<OdbcOrderDateLoader>();

                testObject.Load(fieldMap.Object, orderEntity);

                Assert.Equal(mappedDate, orderEntity.OrderDate);
            }
        }

        private IOdbcFieldMapEntry GetOdbcFieldMapEntry(AutoMock mock, string displayName, object value)
        {
            var shipworksField = mock.Mock<IShipWorksOdbcMappableField>();
            shipworksField.SetupGet(f => f.DisplayName).Returns(displayName);
            shipworksField.SetupGet(f => f.Value).Returns(value);

            var odbcFieldMapEntry = mock.Mock<IOdbcFieldMapEntry>();
            odbcFieldMapEntry.SetupGet(e => e.ShipWorksField).Returns(shipworksField.Object);

            return odbcFieldMapEntry.Object;
        }
    }
}
