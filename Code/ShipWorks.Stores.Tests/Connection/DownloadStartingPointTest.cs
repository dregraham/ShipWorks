using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
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

        /// <summary>
        /// Does the called field match the specified field
        /// </summary>
        private IEntityField2 MatchesField(IEntityField2 onlineLastModified) =>
            It.Is<IEntityField2>(x => x.Name == onlineLastModified.Name);

        /// <summary>
        /// Does the called predicate match the specified predicate
        /// </summary>
        private IPredicate MatchesPredicate(IPredicateExpression expected) =>
            It.Is<IPredicateExpression>(p => DoPredicatesMatch(p, expected));

        /// <summary>
        /// Tests whether two predicate expressions match
        /// </summary>
        private bool DoPredicatesMatch(IPredicateExpression actual, IPredicateExpression expected)
        {
            var expectedPredicates = expected.OfType<IPredicateExpressionElement>()
                .Select(x => x.Contents).OfType<FieldCompareValuePredicate>().ToList();
            var actualPredicates = actual.OfType<IPredicateExpressionElement>()
                .Select(x => x.Contents).OfType<FieldCompareValuePredicate>().ToList();

            if (expectedPredicates.Count != actualPredicates.Count)
            {
                return false;
            }

            return expectedPredicates.Any(p =>
                actualPredicates.None(x => x.FieldCore.Name == p.FieldCore.Name && x.Value == p.Value));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
