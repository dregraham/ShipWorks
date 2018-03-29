using System;
using System.Data;
using System.Data.Common;
using System.Reactive;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Data;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Moq;
using Moq.Protected;
using ShipWorks.Archiving;
using ShipWorks.Data.Administration;
using ShipWorks.Filters;
using ShipWorks.Stores.Orders.Archive;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Orders.Archive
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
                .Setup(x => x.WithSingleUserConnectionAsync(It.IsAny<Func<DbConnection, Task<Unit>>>()))
                .Callback((Func<DbConnection, Task<Unit>> x) => x(connectionMock.Object))
                .ReturnsAsync(Unit.Default);

            mock.Mock<IOrderArchiveDataAccess>()
                .Setup(x => x.WithMultiUserConnection(It.IsAny<Action<DbConnection>>()))
                .Callback((Action<DbConnection> x) => x(connectionMock.Object));

            preparingProgress = mock.CreateMock<IProgressReporter>();
            archivingProgress = mock.CreateMock<IProgressReporter>();
            filterProgress = mock.CreateMock<IProgressReporter>();

            var scope = mock.Mock<IUserLoginWorkflow>();
            scope.Setup(x => x.Logoff(AnyBool)).Returns(true);

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
                        AnyString,
                        AnyString),
                    Times.Once);

            mock.Mock<IOrderArchiveDataAccess>()
                .Verify(x => x.ExecuteSqlAsync(
                        It.IsAny<DbConnection>(),
                        archivingProgress.Object,
                        AnyString,
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
        public void ArchivalSettingsXml_VersionIsCorrect()
        {
            SqlScriptLoader sqlLoader = new SqlScriptLoader("ShipWorks.Res.Data.Administration.Scripts.Update");
            Version version = new Version(0, 0, 0, 0);

            foreach (var script in SqlSchemaUpdater.GetUpdateScripts())
            {
                if (sqlLoader[script.ScriptName].Content.Contains("ArchivalSettingsXml"))
                {
                    version = script.SchemaVersion;
                    break;
                }
            }

            Assert.Equal(new Version(5, 23, 1, 6), version);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
