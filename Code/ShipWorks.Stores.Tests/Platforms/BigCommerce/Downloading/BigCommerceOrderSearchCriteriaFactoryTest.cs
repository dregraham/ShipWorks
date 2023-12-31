﻿using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.BigCommerce.Downloading;
using ShipWorks.Stores.Platforms.BigCommerce.Enums;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce.Downloading
{
    public class BigCommerceOrderSearchCriteriaFactoryTest : IDisposable
    {
        readonly AutoMock mock;
        readonly DateTime now;

        public BigCommerceOrderSearchCriteriaFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            now = new DateTime(2017, 4, 23, 8, 12, 6, DateTimeKind.Utc);
            mock.Mock<IDateTimeProvider>().SetupGet(x => x.UtcNow).Returns(now);
        }

        [Fact]
        public async Task Create_DelegatesToDownloaderStartingPoint()
        {
            var store = mock.Build<IBigCommerceStoreEntity>();
            var testObject = mock.Create<BigCommerceOrderSearchCriteriaFactory>();

            await testObject.Create(store, BigCommerceWebClientOrderDateSearchType.CreatedDate);

            mock.Mock<IDownloadStartingPoint>().Verify(x => x.OrderDate(store));
            mock.Mock<IDownloadStartingPoint>().Verify(x => x.OnlineLastModified(store));
        }

        [Fact]
        public async Task Create_BothToDatesShouldBeNow()
        {
            var testObject = mock.Create<BigCommerceOrderSearchCriteriaFactory>();

            var result = await testObject.Create(mock.Build<IBigCommerceStoreEntity>(), BigCommerceWebClientOrderDateSearchType.CreatedDate);

            Assert.Equal(now, result.LastCreatedToDate);
            Assert.Equal(now, result.LastModifiedToDate);
        }

        [Theory]
        [InlineData("2017-04-22 10:04:06Z", "2017-04-22 10:04:06Z")]
        [InlineData("2017-04-22 10:04:06-600", "2017-04-22 16:04:06Z")]
        public async Task Create_CreatedDateFromShouldBeSet_WhenStartingPointReturnsValue(string databaseDate, string expected)
        {
            var date = DateTime.Parse(databaseDate);
            mock.Mock<IDownloadStartingPoint>().Setup(x => x.OrderDate(It.IsAny<IStoreEntity>())).ReturnsAsync(date);
            var testObject = mock.Create<BigCommerceOrderSearchCriteriaFactory>();

            var result = await testObject.Create(mock.Build<IBigCommerceStoreEntity>(), BigCommerceWebClientOrderDateSearchType.CreatedDate);

            Assert.Equal(expected, result.LastCreatedFromDate.ToString("u"));
        }

        [Fact]
        public async Task Create_CreatedDateFromShouldBeSixMonthsAgo_WhenStartingPointReturnsNull()
        {
            var testObject = mock.Create<BigCommerceOrderSearchCriteriaFactory>();

            var result = await testObject.Create(mock.Build<IBigCommerceStoreEntity>(), BigCommerceWebClientOrderDateSearchType.CreatedDate);

            Assert.Equal(new DateTime(2016, 10, 23, 8, 12, 6, DateTimeKind.Utc), result.LastCreatedFromDate);
        }

        [Theory]
        [InlineData("2017-04-22 10:04:06Z", "2017-04-22 10:04:06Z")]
        [InlineData("2017-04-22 10:04:06-600", "2017-04-22 16:04:06Z")]
        public async Task Create_ModifiedDateFromShouldBeSet_WhenStartingPointReturnsValue(string databaseDate, string expected)
        {
            var date = DateTime.Parse(databaseDate);
            mock.Mock<IDownloadStartingPoint>().Setup(x => x.OnlineLastModified(It.IsAny<IStoreEntity>())).ReturnsAsync(date);
            var testObject = mock.Create<BigCommerceOrderSearchCriteriaFactory>();

            var result = await testObject.Create(mock.Build<IBigCommerceStoreEntity>(), BigCommerceWebClientOrderDateSearchType.CreatedDate);

            Assert.Equal(expected, result.LastModifiedFromDate.ToString("u"));
        }

        [Fact]
        public async Task Create_ModifiedDateFromShouldBeSixMonthsAgo_WhenStartingPointReturnsNull()
        {
            var testObject = mock.Create<BigCommerceOrderSearchCriteriaFactory>();

            var result = await testObject.Create(mock.Build<IBigCommerceStoreEntity>(), BigCommerceWebClientOrderDateSearchType.CreatedDate);

            Assert.Equal(new DateTime(2016, 10, 23, 8, 12, 6, DateTimeKind.Utc), result.LastModifiedFromDate);
        }

        [Fact]
        public async Task Create_PageDetails_AreSet()
        {
            var testObject = mock.Create<BigCommerceOrderSearchCriteriaFactory>();

            var result = await testObject.Create(mock.Build<IBigCommerceStoreEntity>(), BigCommerceWebClientOrderDateSearchType.CreatedDate);

            Assert.Equal(1, result.Page);
            Assert.Equal(50, result.PageSize);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
