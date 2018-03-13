using System;
using System.Data;
using System.Data.Common;
using System.Reactive;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Moq;
using Moq.Protected;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Archiving;
using ShipWorks.Filters;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Tests.Archiving
{
    public class OrderArchiverTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly OrderArchiver testObject;
        private readonly Mock<DbTransaction> transactionMock;
        private readonly Mock<DbConnection> connectionMock;
        private readonly Mock<IProgressReporter> preparingProgress;
        private readonly Mock<IProgressReporter> archivingProgress;
        private readonly Mock<IProgressReporter> filterProgress;

        public OrderArchiverTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<OrderArchiver>();

            transactionMock = mock.Mock<DbTransaction>();
            connectionMock = mock.Mock<DbConnection>();
            connectionMock.CallBase = true;
            connectionMock.Protected()
                .Setup<DbTransaction>("BeginDbTransaction", It.IsAny<IsolationLevel>())
                .Returns(transactionMock);

            mock.Mock<IOrderArchiveDataAccess>()
                .Setup(x => x.WithSingleUserConnectionAsync<Unit>(It.IsAny<Func<DbConnection, Task<Unit>>>()))
                .Callback((Func<DbConnection, Task<Unit>> x) => x(connectionMock.Object))
                .ReturnsAsync(Unit.Default);

            preparingProgress = mock.CreateMock<IProgressReporter>();
            archivingProgress = mock.CreateMock<IProgressReporter>();
            filterProgress = mock.CreateMock<IProgressReporter>();

            var progressProvider = mock.FromFactory<IAsyncMessageHelper>()
                .Mock(x => x.CreateProgressProvider());

            progressProvider.Setup(x => x.AddItem("Preparing archive")).Returns(preparingProgress);
            progressProvider.Setup(x => x.AddItem("Archiving orders")).Returns(archivingProgress);
            progressProvider.Setup(x => x.AddItem("Regenerating filters")).Returns(filterProgress);
        }

        [Fact]
        public async Task Archive_DelegatesToOrderArchiveDataAccess()
        {
            await testObject.Archive(DateTime.Now);

            mock.Mock<IOrderArchiveDataAccess>()
                .Verify(x => x.ExecuteSqlAsync(
                        It.IsAny<DbConnection>(),
                        preparingProgress.Object,
                        AnyString),
                    Times.Once);

            mock.Mock<IOrderArchiveDataAccess>()
                .Verify(x => x.ExecuteSqlAsync(
                        It.IsAny<DbConnection>(),
                        archivingProgress.Object,
                        AnyString),
                    Times.Once);
        }

        [Fact]
        public async Task Archive_DelegatesToFilterHelper()
        {
            await testObject.Archive(DateTime.Now);

            mock.Mock<IFilterHelper>().Verify(x => x.RegenerateFilters(It.IsAny<DbConnection>()));
        }

        [Fact]
        public async Task Archive_CallsFailedOnAllProgress_WhenArchiveFails()
        {
            var ex = new ORMException();

            mock.Mock<IOrderArchiveDataAccess>()
                .Setup(x => x.WithSingleUserConnectionAsync<Unit>(It.IsAny<Func<DbConnection, Task<Unit>>>()))
                .ThrowsAsync(ex);

            await testObject.Archive(DateTime.Now).Recover(e => Unit.Default);

            preparingProgress.Verify(x => x.Failed(ex));
            archivingProgress.Verify(x => x.Failed(ex));
            filterProgress.Verify(x => x.Failed(ex));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
