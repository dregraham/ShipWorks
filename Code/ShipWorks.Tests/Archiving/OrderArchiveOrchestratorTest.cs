using System;
using System.Reactive;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Archiving;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Security;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Tests.Archiving
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
            mock.Mock<IDateTimeProvider>()
                .SetupGet(x => x.Now)
                .Returns(DateTime.Now);

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

        [Fact]
        public async Task Archive_CallsArchiver_WithDate90DaysAgo()
        {
            mock.Mock<IDateTimeProvider>()
                .SetupGet(x => x.Now)
                .Returns(new DateTime(2018, 3, 8, 12, 30, 0, DateTimeKind.Local));

            await testObject.Archive();

            mock.Mock<IOrderArchiver>().Verify(x => x.Archive(new DateTime(2017, 12, 8, 12, 30, 0)));
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

        [Fact]
        public async Task Archive_ShowsSuccess_WhenProcessSucceeds()
        {
            await testObject.Archive().Recover(ex => Unit.Default);

            mock.Mock<IAsyncMessageHelper>().Verify(x => x.ShowMessage("Archive finished"));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
