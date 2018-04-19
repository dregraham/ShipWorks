using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Administration;
using ShipWorks.Stores.Orders.Archive;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Orders.Archive
{
    public class ArchiveNotificationViewModelTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ArchiveNotificationViewModel testObject;
        private readonly ISqlDatabaseDetail liveDatabase;

        public ArchiveNotificationViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ArchiveNotificationViewModel>();

            var dbGuid = Guid.NewGuid();
            liveDatabase = CreateDatabaseDetail(d =>
                    {
                        d.SetupGet(x => x.Guid).Returns(dbGuid);
                        d.SetupGet(x => x.Name).Returns("Bar");
                        d.SetupGet(x => x.IsArchive).Returns(false);
                    });

            mock.Mock<IArchiveManagerDataAccess>()
                .Setup(x => x.GetLiveDatabase())
                .ReturnsAsync(liveDatabase);
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
            testObject.ConnectToLiveDatabase.Execute(null);

            mock.Mock<IArchiveManagerDataAccess>()
                .Verify(x => x.GetLiveDatabase());
        }

        [Fact]
        public void ConnectToLiveDatabase_ShowsErrorMessage_WhenLiveDatabaseIsNotFound()
        {
            mock.Mock<IArchiveManagerDataAccess>()
                .Setup(x => x.GetLiveDatabase())
                .Returns(Task.FromException<ISqlDatabaseDetail>(new InvalidOperationException("Foo")));

            testObject.ConnectToLiveDatabase.Execute(null);

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowError(AnyString));
        }

        [Fact]
        public void ConnectToLiveDatabase_CallsChangeDatabase_WhenLiveDatabaseIsFound()
        {
            testObject.ConnectToLiveDatabase.Execute(null);

            mock.Mock<IArchiveManagerDataAccess>()
                .Verify(x => x.ChangeDatabase(liveDatabase));
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
