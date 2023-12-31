﻿using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Yahoo;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Yahoo
{
    public class YahooDownloaderFactoryTest : IDisposable
    {
        private readonly AutoMock mock;

        public YahooDownloaderFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Constructor_CreatesEmailDownloader_WhenStoreIDIsSet()
        {
            var store = new YahooStoreEntity();
            var factoryMock = mock.MockFunc<YahooStoreEntity, IYahooEmailDownloader>();

            mock.Create<YahooDownloaderFactory>(TypedParameter.From<StoreEntity>(store));

            factoryMock.Verify(x => x(store));
        }

        [Fact]
        public void Constructor_CreatesApiDownloader_WhenStoreIDIsEmpty()
        {
            var store = new YahooStoreEntity { YahooStoreID = "Foo" };
            var factoryMock = mock.MockFunc<YahooStoreEntity, IYahooApiDownloader>();

            mock.Create<YahooDownloaderFactory>(TypedParameter.From<StoreEntity>(store));

            factoryMock.Verify(x => x(store));
        }

        [Fact]
        public void Download_DelegatesToDownloader()
        {
            StoreEntity store = new YahooStoreEntity { YahooStoreID = "Foo" };
            var testObject = mock.Create<YahooDownloaderFactory>(TypedParameter.From(store));

            var progress = mock.Mock<IProgressReporter>().Object;
            var dbConnection = mock.Mock<DbConnection>().Object;
            var downloadLog = mock.Build<IDownloadEntity>();

            testObject.Download(progress, downloadLog, dbConnection);
            mock.Mock<IYahooApiDownloader>()
                .Verify(x => x.Download(progress, downloadLog, dbConnection));
        }

        [Fact]
        public void DownloadWithOrderNumber_ReturnsCompletedTask()
        {
            StoreEntity store = new YahooStoreEntity();
            var testObject = mock.Create<YahooDownloaderFactory>(TypedParameter.From(store));

            var dbConnection = mock.Mock<DbConnection>().Object;

            var task = testObject.Download("blah", 1, dbConnection);

            Assert.Equal(Task.CompletedTask, task);
        }

        [Fact]
        public void ShouldDownload_ReturnsFalse()
        {
            StoreEntity store = new YahooStoreEntity();
            var shouldDownload = mock.Create<YahooDownloaderFactory>(TypedParameter.From(store)).ShouldDownload("1");
            Assert.False(shouldDownload);
        }

        [SuppressMessage("SonarLint", "S1481: Unused local variables should be removed",
            Justification = "We're testing a getter. We don't need the value but cannot call a getter without storing it")]
        [Fact]
        public void QuantityNew_DelegatesToDownloader()
        {
            StoreEntity store = new YahooStoreEntity { YahooStoreID = "Foo" };
            var testObject = mock.Create<YahooDownloaderFactory>(TypedParameter.From(store));

            var quantity = testObject.QuantityNew;
            mock.Mock<IYahooApiDownloader>().VerifyGet(x => x.QuantityNew);
        }

        [SuppressMessage("SonarLint", "S1481: Unused local variables should be removed",
            Justification = "We're testing a getter. We don't need the value but cannot call a getter without storing it")]
        [Fact]
        public void QuantitySaved_DelegatesToDownloader()
        {
            StoreEntity store = new YahooStoreEntity { YahooStoreID = "Foo" };
            var testObject = mock.Create<YahooDownloaderFactory>(TypedParameter.From(store));

            var quantity = testObject.QuantitySaved;
            mock.Mock<IYahooApiDownloader>().VerifyGet(x => x.QuantitySaved);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
