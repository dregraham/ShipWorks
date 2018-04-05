using System;
using System.Data.Common;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Orders.Archive;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Orders.Archive
{
    public class ArchiveNotificationViewModelTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ArchiveNotificationViewModel testObject;

        public ArchiveNotificationViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ArchiveNotificationViewModel>();

            var dbGuid = Guid.NewGuid();

            var sqlSession = mock.Mock<ISqlSession>();
            sqlSession.SetupGet(x => x.DatabaseIdentifier).Returns(dbGuid);
            sqlSession.SetupGet(x => x.DatabaseName).Returns("Foo");

            mock.Mock<IShipWorksDatabaseUtility>()
                .Setup(x => x.GetDatabaseDetails(It.IsAny<DbConnection>()))
                .ReturnsAsync(new[] {
                    CreateDatabaseDetail(d =>
                    {
                        d.SetupGet(x => x.Guid).Returns(dbGuid);
                        d.SetupGet(x => x.Name).Returns("Bar");
                        d.SetupGet(x => x.IsArchive).Returns(false);
                    })
                });
        }

        [Fact]
        public void ConnectToLiveDatabase_GetsProgressDialog_FromMessageHelper()
        {
            testObject.ConnectToLiveDatabase.Execute(null);

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowProgressDialog(AnyString, AnyString));
        }

        [Fact]
        public void ConnectToLiveDatabase_DelegatesToDatabaseUtility_ToGetPossibleDatabases()
        {
            var connection = mock.Mock<DbConnection>();
            mock.Mock<ISqlSession>().Setup(x => x.OpenConnection()).Returns(connection);

            testObject.ConnectToLiveDatabase.Execute(null);

            mock.Mock<IShipWorksDatabaseUtility>()
                .Verify(x => x.GetDatabaseDetails(connection.Object));
        }

        [Fact]
        public void ConnectToLiveDatabase_ShowsErrorMessage_WhenLiveDatabaseIsNotFound()
        {
            mock.Mock<IShipWorksDatabaseUtility>()
                .Setup(x => x.GetDatabaseDetails(It.IsAny<DbConnection>()))
                .ReturnsAsync(new[] {
                    CreateDatabaseDetail(d =>
                    {
                        d.SetupGet(x => x.Guid).Returns(Guid.NewGuid());
                        d.SetupGet(x => x.Name).Returns("Bar");
                        d.SetupGet(x => x.IsArchive).Returns(false);
                    })
                });

            testObject.ConnectToLiveDatabase.Execute(null);

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError(AnyString));
        }

        [Fact]
        public void ConnectToLiveDatabase_CallsLogoff_WhenLiveDatabaseIsFound()
        {
            testObject.ConnectToLiveDatabase.Execute(null);

            mock.Mock<IUserLoginWorkflow>()
                .Verify(x => x.Logoff(false));
        }

        [Fact]
        public void ConnectToLiveDatabase_DoesNotSaveNewDatabase_WhenLogoffFails()
        {
            mock.Mock<IUserLoginWorkflow>()
                .Setup(x => x.Logoff(AnyBool))
                .Returns(false);

            var copiedSession = mock.Mock<ISqlSession>();
            mock.Mock<ISqlSession>()
                .Setup(x => x.CreateCopy())
                .Returns(copiedSession);

            testObject.ConnectToLiveDatabase.Execute(null);

            copiedSession.VerifySet(x => x.DatabaseName = AnyString, Times.Never);
            copiedSession.Verify(x => x.SaveAsCurrent(), Times.Never);
        }

        [Fact]
        public void ConnectToLiveDatabase_SavesNewDatabase_WhenLogoffSucceeds()
        {
            mock.Mock<IUserLoginWorkflow>()
                .Setup(x => x.Logoff(AnyBool))
                .Returns(true);

            var copiedSession = mock.Mock<ISqlSession>();
            mock.Mock<ISqlSession>()
                .Setup(x => x.CreateCopy())
                .Returns(copiedSession);

            testObject.ConnectToLiveDatabase.Execute(null);

            copiedSession.VerifySet(x => x.DatabaseName = "Bar");
            copiedSession.Verify(x => x.SaveAsCurrent());
        }

        [Fact]
        public void ConnectToLiveDatabase_CallsLogon_WhenLogoffSucceeds()
        {
            mock.Mock<IUserLoginWorkflow>()
                .Setup(x => x.Logoff(AnyBool))
                .Returns(true);

            testObject.ConnectToLiveDatabase.Execute(null);

            mock.Mock<IUserLoginWorkflow>()
                .Verify(x => x.Logon(null));
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

            var result = ArchiveNotificationViewModel.IsLiveDatabase(sqlSession.Object)(databaseItem);

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
