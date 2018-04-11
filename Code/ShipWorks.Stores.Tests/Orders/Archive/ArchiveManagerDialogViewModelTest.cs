using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Administration;
using ShipWorks.Stores.Orders.Archive;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Orders.Archive
{
    public class ArchiveManagerDialogViewModelTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly ArchiveManagerDialogViewModel testObject;

        public ArchiveManagerDialogViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ArchiveManagerDialogViewModel>();
        }

        [Fact]
        public void ShowManager_GetsArchivedDatabases()
        {
            testObject.ShowManager();

            mock.Mock<IArchiveManagerDataAccess>()
                .Verify(x => x.GetArchiveDatabases());
        }

        [Fact]
        public void ShowManager_CompletesTask_WhenDialogClosesNormally()
        {
            mock.Mock<IAsyncMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<Func<IDialog>>()))
                .ReturnsAsync(true);

            var result = testObject.ShowManager();

            Assert.True(result.IsCompleted);
        }

        [Fact]
        public void ArchiveNow_ClosesExistingDialog()
        {
            testObject.ShowManager();

            testObject.ArchiveNow.Execute(null);

            mock.Mock<IArchiveManagerDialog>().Verify(x => x.Close());
        }

        [Fact]
        public void ArchiveNow_PerformsArchive()
        {
            testObject.ShowManager();

            testObject.ArchiveNow.Execute(null);

            mock.Mock<IOrderArchiveOrchestrator>()
                .Verify(x => x.Archive());
        }

        [Fact]
        public void ArchiveNow_ReopensDialog()
        {
            testObject.ShowManager();

            mock.Mock<IAsyncMessageHelper>()
                .ResetCalls();

            testObject.ArchiveNow.Execute(null);

            mock.Mock<IAsyncMessageHelper>()
                .Verify(x => x.ShowDialog(It.IsAny<Func<IDialog>>()));
        }

        [Fact]
        public void ArchiveNow_DoesNotCompleteShowDialogTask()
        {
            mock.Mock<IAsyncMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<Func<IDialog>>()))
                .Returns(new TaskCompletionSource<bool?>().Task);

            var result = testObject.ShowManager();

            testObject.ArchiveNow.Execute(null);

            Assert.False(result.IsCompleted);
        }

        [Fact]
        public void ConnectToArchive_ClosesExistingDialog()
        {
            testObject.ShowManager();
            testObject.SelectedArchive = mock.Build<ISqlDatabaseDetail>();

            testObject.ConnectToArchive.Execute(null);

            mock.Mock<IArchiveManagerDialog>().Verify(x => x.Close());
        }

        [Fact]
        public void ConnectToArchive_ChangesDatabase()
        {
            testObject.ShowManager();
            testObject.SelectedArchive = mock.Build<ISqlDatabaseDetail>();

            testObject.ConnectToArchive.Execute(null);

            mock.Mock<IArchiveManagerDataAccess>()
                .Verify(x => x.ChangeDatabase(It.IsAny<ISqlDatabaseDetail>()));
        }

        [Fact]
        public void ConnectToArchive_ReopensDialog_WhenChangeDatabaseReturnsFalse()
        {
            testObject.ShowManager();
            testObject.SelectedArchive = mock.Build<ISqlDatabaseDetail>();

            mock.Mock<IAsyncMessageHelper>()
                .ResetCalls();

            testObject.ConnectToArchive.Execute(null);

            mock.Mock<IAsyncMessageHelper>()
                .Verify(x => x.ShowDialog(It.IsAny<Func<IDialog>>()));
        }

        [Fact]
        public void ConnectToArchiveAction_DoesNotCompleteShowDialogTask_WhenChangeDatabaseReturnsFalse()
        {
            mock.Mock<IAsyncMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<Func<IDialog>>()))
                .Returns(new TaskCompletionSource<bool?>().Task);

            var result = testObject.ShowManager();

            testObject.ConnectToArchive.Execute(null);

            Assert.False(result.IsCompleted);
        }

        [Fact]
        public void ConnectToArchive_CompletesTask_WhenChangeDatabaseReturnsTrue()
        {
            mock.Mock<IAsyncMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<Func<IDialog>>()))
                .Returns(new TaskCompletionSource<bool?>().Task);

            var result = testObject.ShowManager();
            testObject.SelectedArchive = mock.Build<ISqlDatabaseDetail>();

            mock.Mock<IArchiveManagerDataAccess>()
                .Setup(x => x.ChangeDatabase(It.IsAny<ISqlDatabaseDetail>()))
                .Returns(true);

            testObject.ConnectToArchive.Execute(null);

            Assert.True(result.IsCompleted);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
