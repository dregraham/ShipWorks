using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Connection
{
    public class DownloadStartingPointTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Mock<IStoreEntity> store;

        public DownloadStartingPointTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            store = mock.CreateMock<IStoreEntity>();
        }

        [Fact]
        public async Task OnlineLastModified_DelegatesToSqlAdapter()
        {
            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(x => x.Create());

            store.SetupGet(x => x.StoreID).Returns(123);
            var testObject = mock.Create<DownloadStartingPoint>();

            await testObject.OnlineLastModified(store.Object);

            sqlAdapter.Verify(x => x.FetchScalarAsync<DateTime?>(It.IsAny<DynamicQuery>()));
        }

        [Fact]
        public async Task OnlineLastModified_ReturnsDateTime_WhenSqlReturnsDateTime()
        {
            SetupSqlGetScalerToReturn(new DateTime(2017, 4, 21, 15, 30, 15));
            var testObject = mock.Create<DownloadStartingPoint>();

            var result = await testObject.OnlineLastModified(store.Object);

            Assert.Equal(new DateTime(2017, 4, 21, 15, 30, 15), result);
        }

        [Fact]
        public async Task OnlineLastModified_ReturnsInitialDownloadDays_WhenSqlReturnsNull()
        {
            SetupSqlGetScalerToReturn(null);

            store.SetupGet(x => x.InitialDownloadDays).Returns(3);

            mock.Mock<IDateTimeProvider>()
                .SetupGet(x => x.UtcNow)
                .Returns(new DateTime(2017, 4, 21, 15, 30, 15));

            var testObject = mock.Create<DownloadStartingPoint>();

            var result = await testObject.OnlineLastModified(store.Object);

            Assert.Equal(new DateTime(2017, 4, 18, 17, 30, 15), result);
        }

        [Fact]
        public async Task OnlineLastModified_SpecifiesUtc_WhenDateTypeIsUnspecified()
        {
            SetupSqlGetScalerToReturn(new DateTime(2017, 4, 21, 15, 30, 15, DateTimeKind.Unspecified));

            mock.Mock<IDateTimeProvider>()
                .SetupGet(x => x.UtcNow)
                .Returns(new DateTime(2017, 4, 21, 15, 30, 15));

            var testObject = mock.Create<DownloadStartingPoint>();

            var result = await testObject.OnlineLastModified(store.Object);

            Assert.Equal(DateTimeKind.Utc, result.Value.Kind);
        }

        [Fact]
        public async Task OrderDate_DelegatesToSqlAdapter()
        {
            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(x => x.Create());

            store.SetupGet(x => x.StoreID).Returns(123);
            var testObject = mock.Create<DownloadStartingPoint>();

            await testObject.OrderDate(store.Object);

            sqlAdapter.Verify(x => x.FetchScalarAsync<DateTime?>(It.IsAny<DynamicQuery>()));
        }

        [Fact]
        public async Task OrderDate_ReturnsDateTime_WhenSqlReturnsDateTime()
        {
            SetupSqlGetScalerToReturn(new DateTime(2017, 4, 21, 15, 30, 15));
            var testObject = mock.Create<DownloadStartingPoint>();

            var result = await testObject.OrderDate(store.Object);

            Assert.Equal(new DateTime(2017, 4, 21, 15, 30, 15), result);
        }

        [Fact]
        public async Task OrderDate_ReturnsInitialDownloadDays_WhenSqlReturnsNull()
        {
            SetupSqlGetScalerToReturn(null);

            store.SetupGet(x => x.InitialDownloadDays).Returns(3);

            mock.Mock<IDateTimeProvider>()
                .SetupGet(x => x.UtcNow)
                .Returns(new DateTime(2017, 4, 21, 15, 30, 15));

            var testObject = mock.Create<DownloadStartingPoint>();

            var result = await testObject.OrderDate(store.Object);

            Assert.Equal(new DateTime(2017, 4, 18, 17, 30, 15), result);
        }

        [Fact]
        public async Task OrderDate_SpecifiesUtc_WhenDateTypeIsUnspecified()
        {
            SetupSqlGetScalerToReturn(new DateTime(2017, 4, 21, 15, 30, 15, DateTimeKind.Unspecified));

            mock.Mock<IDateTimeProvider>()
                .SetupGet(x => x.UtcNow)
                .Returns(new DateTime(2017, 4, 21, 15, 30, 15));

            var testObject = mock.Create<DownloadStartingPoint>();

            var result = await testObject.OrderDate(store.Object);

            Assert.Equal(DateTimeKind.Utc, result.Value.Kind);
        }

        /// <summary>
        /// Setup the SqlAdapter to return a specified value when GetScalar is called
        /// </summary>
        private void SetupSqlGetScalerToReturn(DateTime? value)
        {
            mock.FromFactory<ISqlAdapterFactory>()
                .Mock(x => x.Create())
                .Setup(x => x.FetchScalarAsync<DateTime?>(It.IsAny<DynamicQuery>()))
                .ReturnsAsync(value);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
