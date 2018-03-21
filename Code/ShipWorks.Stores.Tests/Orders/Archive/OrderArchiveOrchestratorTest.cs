using System;
using System.Reactive;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Stores.Orders.Archive;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Security;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Orders.Archive
{
    public class OrderArchiveOrchestratorTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly OrderArchiveOrchestrator testObject;

        public OrderArchiveOrchestratorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mock.Mock<ISecurityContext>()
                .Setup(x => x.RequestPermission(PermissionType.DatabaseArchive, null))
                .Returns(Result.FromSuccess());
            mock.Mock<IOrderArchiveViewModel>()
                .Setup(x => x.GetArchiveDateFromUser())
                .ReturnsAsync(new DateTime(2018, 3, 13, 12, 30, 00));

            testObject = mock.Create<OrderArchiveOrchestrator>();
        }

        [Fact]
        public async Task Archive_DoesNotCallArchiver_WhenUserDoesNotHavePermissions()
        {
            mock.Mock<ISecurityContext>()
                .Setup(x => x.RequestPermission(PermissionType.DatabaseArchive, null))
                .Returns(Result.FromError("No permission"));

            await testObject.Archive().Recover(ex => Unit.Default);

            mock.Mock<IOrderArchiver>().Verify(x => x.Archive(AnyDate), Times.Never);
        }

        [Theory]
        [InlineData("2018-1-1T12:30:00Z")]
        [InlineData("2012-12-12T09:30:00Z")]
        public async Task Archive_CallsArchiver_WithSelectedDate(string cutoffDate)
        {
            DateTime parsedDate = DateTime.Parse(cutoffDate);

            mock.Mock<IOrderArchiveViewModel>()
                .Setup(x => x.GetArchiveDateFromUser())
                .ReturnsAsync(parsedDate);

            await testObject.Archive();

            mock.Mock<IOrderArchiver>().Verify(x => x.Archive(parsedDate));
        }

        [Fact]
        public async Task Archive_ShowsError_WhenProcessFails()
        {
            mock.Mock<IOrderArchiver>()
                .Setup(x => x.Archive(AnyDate))
                .ThrowsAsync(new InvalidOperationException("Failed"));

            await testObject.Archive().Recover(ex => Unit.Default);

            mock.Mock<IAsyncMessageHelper>().Verify(x => x.ShowError("Failed"));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
