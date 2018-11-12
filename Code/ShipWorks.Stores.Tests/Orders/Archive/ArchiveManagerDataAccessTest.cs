using System;
using System.Data.Common;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Orders.Archive;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.ExtensionMethods;
using ShipWorks.Users;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Orders.Archive
{
    public class ArchiveManagerDataAccessTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly ArchiveManagerDataAccess testObject;

        public ArchiveManagerDataAccessTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            Mock<DbConnection> dbConnection = mock.CreateMock<DbConnection>();
            mock.Mock<ISqlSession>()
                .Setup(s => s.OpenConnection())
                .Returns(dbConnection.Object);

            testObject = mock.Create<ArchiveManagerDataAccess>();
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

            var databaseItem = CreateDatabaseDetail(detail =>
            {
                detail.SetupGet(x => x.Status).Returns(status);
                detail.SetupGet(x => x.IsArchive).Returns(isArchive);
                detail.SetupGet(x => x.Guid).Returns(guid);
                detail.SetupGet(x => x.Name).Returns(name);
            });

            var result = ArchiveManagerDataAccess.IsLiveDatabase("Foo", databaseGuid)(databaseItem);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ChangeDatabase_CallsLogoff_WhenLiveDatabaseIsFound()
        {
            testObject.ChangeDatabase(mock.Build<ISqlDatabaseDetail>());

            mock.Mock<IUserLoginWorkflow>()
                .Verify(x => x.Logoff(false));
        }

        [Fact]
        public void ChangeDatabase_DoesNotSaveNewDatabase_WhenLogoffFails()
        {
            mock.Mock<IUserLoginWorkflow>()
                .Setup(x => x.Logoff(AnyBool))
                .Returns(false);

            var copiedSession = mock.Mock<ISqlSession>();
            mock.Mock<ISqlSession>()
                .Setup(x => x.CreateCopy())
                .Returns(copiedSession);

            testObject.ChangeDatabase(mock.Build<ISqlDatabaseDetail>());

            copiedSession.VerifySet(x => x.DatabaseName = AnyString, Times.Never);
            copiedSession.Verify(x => x.SaveAsCurrent(), Times.Never);
        }

        [Fact]
        public void ChangeDatabase_SavesNewDatabase_WhenLogoffSucceeds()
        {
            mock.Mock<IUserLoginWorkflow>()
                .Setup(x => x.Logoff(AnyBool))
                .Returns(true);

            var copiedSession = mock.Mock<ISqlSession>();
            mock.Mock<ISqlSession>()
                .Setup(x => x.CreateCopy())
                .Returns(copiedSession);

            testObject.ChangeDatabase(CreateDatabaseDetail(detail => detail.SetupGet(x => x.Name).Returns("Bar")));

            copiedSession.VerifySet(x => x.DatabaseName = "Bar");
            copiedSession.Verify(x => x.SaveAsCurrent());
        }

        [Fact]
        public void ChangeDatabase_CallsLogon_WhenLogoffSucceeds()
        {
            mock.Mock<IUserLoginWorkflow>()
                .Setup(x => x.Logoff(AnyBool))
                .Returns(true);

            testObject.ChangeDatabase(mock.Build<ISqlDatabaseDetail>());

            mock.Mock<IUserLoginWorkflow>()
                .Verify(x => x.Logon(null));
        }

        [Fact]
        public void GetLiveDatabase_CallsSelectSingleDatabase_WithFilteredListOfDatabases()
        {
            var databaseGuid = Guid.Parse("D1D2D159-5447-45EC-A70B-F5FCA08CB0A8");

            mock.Mock<IDatabaseIdentifier>().Setup(x => x.Get()).Returns(databaseGuid);
            mock.Mock<ISqlSession>().SetupGet(x => x.DatabaseName).Returns("Foo");

            var liveDatabase = CreateDatabaseDetail(detail => detail.SetupGet(x => x.Guid).Returns(databaseGuid));
            var otherDatabase = CreateDatabaseDetail(detail => detail.SetupGet(x => x.Guid).Returns(Guid.NewGuid()));

            mock.Mock<IShipWorksDatabaseUtility>()
                .Setup(x => x.GetDatabaseDetails(It.IsAny<DbConnection>()))
                .ReturnsAsync(new[] { liveDatabase, otherDatabase });

            testObject.GetLiveDatabase();

            mock.Mock<ISingleDatabaseSelectorViewModel>()
                .Verify(x => x.SelectSingleDatabase(ItIs.Enumerable(liveDatabase)));
        }

        /// <summary>
        /// Create a database detail entry
        /// </summary>
        private ISqlDatabaseDetail CreateDatabaseDetail(Action<Mock<ISqlDatabaseDetail>> configure)
        {
            var detail = mock.CreateMock<ISqlDatabaseDetail>();
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
