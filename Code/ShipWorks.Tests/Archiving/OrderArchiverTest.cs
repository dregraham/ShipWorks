using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using Moq.Protected;
using ShipWorks.Archiving;
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
                .Setup(x => x.WithSingleUserConnectionAsync<int>(It.IsAny<Func<DbConnection, Task<int>>>()))
                .Callback((Func<DbConnection, Task<int>> x) => x(connectionMock.Object))
                .ReturnsAsync(0);
        }

        [Fact]
        public async Task Test()
        {
            await testObject.Archive(DateTime.Now);

            mock.Mock<IOrderArchiveDataAccess>()
                .Verify(x => x.ExecuteSqlAsync(
                        It.IsAny<DbTransaction>(),
                        It.Is<IProgressReporter>(p => p.Name == "Preparing archive"),
                        AnyString),
                    Times.Once);

            mock.Mock<IOrderArchiveDataAccess>()
                .Verify(x => x.ExecuteSqlAsync(
                        It.IsAny<DbTransaction>(),
                        It.Is<IProgressReporter>(p => p.Name == "Archiving orders"),
                        AnyString),
                    Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
