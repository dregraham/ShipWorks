using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Orders.Archive;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Orders.Archive
{
    public class ArchiveManagerDataAccessTest : IDisposable
    {
        readonly AutoMock mock;

        public ArchiveManagerDataAccessTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(SqlDatabaseStatus.ShipWorks, false, "D1D2D159-5447-45EC-A70B-F5FCA08CB0A8", "Bar", true)]
        [InlineData(SqlDatabaseStatus.NonShipWorks, false, "D1D2D159-5447-45EC-A70B-F5FCA08CB0A8", "Bar", false)]
        [InlineData(SqlDatabaseStatus.ShipWorks, true, "D1D2D159-5447-45EC-A70B-F5FCA08CB0A8", "Bar", false)]
        [InlineData(SqlDatabaseStatus.ShipWorks, false, "A1D2D159-5447-45EC-A70B-F5FCA08CB0A8", "Bar", false)]
        [InlineData(SqlDatabaseStatus.ShipWorks, false, "D1D2D159-5447-45EC-A70B-F5FCA08CB0A8", "Foo", false)]
        public void IsLiveDatabase_ReturnsCorrectValue_BasedOnInput(SqlDatabaseStatus status, bool isArchive, string guidValue, string name, bool expected)
        {
            var guid = Guid.Parse(guidValue);
            var databaseGuid = Guid.Parse("D1D2D159-5447-45EC-A70B-F5FCA08CB0A8");

            var sqlSession = mock.Mock<ISqlSession>();
            sqlSession.SetupGet(x => x.DatabaseIdentifier).Returns(databaseGuid);
            sqlSession.SetupGet(x => x.DatabaseName).Returns("Foo");

            var databaseItem = CreateDatabaseDetail(detail =>
            {
                detail.SetupGet(x => x.Status).Returns(status);
                detail.SetupGet(x => x.IsArchive).Returns(isArchive);
                detail.SetupGet(x => x.Guid).Returns(guid);
                detail.SetupGet(x => x.Name).Returns(name);
            });

            var result = ArchiveManagerDataAccess.IsLiveDatabase(sqlSession.Object)(databaseItem);

            Assert.Equal(expected, result);
        }

        /// <summary>
        /// Create a database detail entry
        /// </summary>
        private ISqlDatabaseDetail CreateDatabaseDetail(Action<Mock<ISqlDatabaseDetail>> configure)
        {
            var detail = mock.Mock<ISqlDatabaseDetail>();
            detail.SetupGet(x => x.Status).Returns(SqlDatabaseStatus.ShipWorks);
            detail.SetupGet(x => x.Guid).Returns(Guid.NewGuid());
            detail.SetupGet(x => x.IsArchive).Returns(false);

            configure?.Invoke(detail);
            return detail.Object;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
