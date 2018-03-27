using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Archiving;
using ShipWorks.Stores.Orders.Archive;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Orders.Archive
{
    public class OrderArchiveViewModelTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<ISchedulerProvider> testScheduler;
        private readonly OrderArchiveViewModel testObject;

        public OrderArchiveViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testScheduler = mock.WithMockImmediateScheduler();

            mock.Mock<IOrderArchiveDataAccess>()
                .Setup(x => x.GetCountOfOrdersToArchive(AnyDate))
                .ReturnsAsync(1);

            mock.Mock<IDateTimeProvider>()
                .SetupGet(x => x.Now)
                .Returns(new DateTime(2018, 3, 14, 12, 30, 00));

            testObject = mock.Create<OrderArchiveViewModel>();
        }

        [Fact]
        public void Constructor_SetsArchiveDateTo90DaysAgo()
        {
            Assert.Equal(new DateTime(2017, 12, 14, 12, 30, 00), testObject.ArchiveDate);
        }

        [Theory]
        [InlineData("2018-3-15T00:00:00-600", true)]
        [InlineData("2018-3-14T00:00:00-600", false)]
        [InlineData("2018-3-13T00:00:00-600", false)]
        public void ArchiveDate_SetsIsDateInFuture_DependingOnSpecifiedDate(string newDate, bool expected)
        {
            testObject.ArchiveDate = DateTime.Parse(newDate);

            Assert.Equal(expected, testObject.IsDateInFuture);
        }

        [Fact]
        public void ArchiveDate_DelegatesToDataAccess_ToGetOrderCounts()
        {
            var newDate = new DateTime(2018, 3, 1);
            testObject.ArchiveDate = newDate;

            mock.Mock<IOrderArchiveDataAccess>()
                .Verify(x => x.GetCountOfOrdersToArchive(newDate));
        }

        [Fact]
        public async Task GetArchiveDataFromUser_DelegatesToMessageHelper_ToOpenDialog()
        {
            var dialog = mock.Mock<IOrderArchiveDialog>().Object;
            IDialog calledDialog = null;

            mock.Mock<IAsyncMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<Func<IDialog>>()))
                .Callback((Func<IDialog> f) => calledDialog = f())
                .ReturnsAsync(true);

            await testObject.GetArchiveDateFromUser();

            Assert.Equal(dialog, calledDialog);
        }

        [Theory]
        [InlineData(DateTimeKind.Utc, "2018-1-1T12:30:00Z", "2018-1-1T12:30:00Z")]
        [InlineData(DateTimeKind.Local, "2018-1-1T00:30:00-600", "2018-1-1T12:30:00Z")]
        public async Task GetArchiveDataFromUser_ReturnsSelectedDate(DateTimeKind kind, string selectedDate, string expectedDate)
        {
            mock.Mock<IAsyncMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<Func<IDialog>>()))
                .ReturnsAsync(true);

            testObject.ArchiveDate = DateTime.SpecifyKind(DateTime.Parse(selectedDate), kind);
            var result = await testObject.GetArchiveDateFromUser();

            var expected = DateTime.SpecifyKind(DateTime.Parse(expectedDate), DateTimeKind.Utc);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public async Task GetArchiveDataFromUser_ReturnsValue_DependingOnReturnOfShowDialog(bool? dialogResult)
        {
            mock.Mock<IAsyncMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<Func<IDialog>>()))
                .ReturnsAsync(dialogResult);

            await testObject.GetArchiveDateFromUser()
                .Do(x =>
                    Assert.True(dialogResult),
                    ex => Assert.False(dialogResult != true))
                .Recover(ex => DateTime.Now);
        }

        [Fact]
        public void ConfirmArchive_SetsDialogResultToTrue_WhenExecuted()
        {
            testObject.ConfirmArchive.Execute(null);

            mock.Mock<IOrderArchiveDialog>()
                .VerifySet(x => x.DialogResult = true);
        }

        [Fact]
        public void ConfirmArchive_ClosesDialog_WhenExecuted()
        {
            testObject.ConfirmArchive.Execute(null);

            mock.Mock<IOrderArchiveDialog>()
                .Verify(x => x.Close());
        }

        [Fact]
        public void CancelArchive_ClosesDialog_WhenExecuted()
        {
            testObject.CancelArchive.Execute(null);

            mock.Mock<IOrderArchiveDialog>()
                .Verify(x => x.Close());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
