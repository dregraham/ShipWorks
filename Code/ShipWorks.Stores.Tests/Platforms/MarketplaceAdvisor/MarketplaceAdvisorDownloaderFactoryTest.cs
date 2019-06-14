﻿using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.MarketplaceAdvisor
{
    public class MarketplaceAdvisorDownloaderFactoryTest : IDisposable
    {
        private readonly AutoMock mock;

        public MarketplaceAdvisorDownloaderFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(MarketplaceAdvisorAccountType.LegacyCorporate)]
        [InlineData(MarketplaceAdvisorAccountType.LegacyStandard)]
        public void Constructor_CreatesLegacyDownloader_WhenApiIsSoap(MarketplaceAdvisorAccountType accountType)
        {
            MarketplaceAdvisorStoreEntity store = new MarketplaceAdvisorStoreEntity { AccountType = (int) accountType };
            var factoryMock = mock.MockFunc<MarketplaceAdvisorStoreEntity, IMarketplaceAdvisorLegacyDownloader>();

            mock.Create<MarketplaceAdvisorDownloaderFactory>(TypedParameter.From<StoreEntity>(store));

            factoryMock.Verify(x => x(store));
        }

        [Fact]
        public void Constructor_CreatesOMSDownloader_WhenApiIsMWS()
        {
            MarketplaceAdvisorStoreEntity store = new MarketplaceAdvisorStoreEntity { AccountType = (int) MarketplaceAdvisorAccountType.OMS };
            var factoryMock = mock.MockFunc<MarketplaceAdvisorStoreEntity, IMarketplaceAdvisorOmsDownloader>();

            mock.Create<MarketplaceAdvisorDownloaderFactory>(TypedParameter.From<StoreEntity>(store));

            factoryMock.Verify(x => x(store));
        }

        [Fact]
        public void Download_DelegatesToDownloader()
        {
            StoreEntity store = new MarketplaceAdvisorStoreEntity { AccountType = (int) MarketplaceAdvisorAccountType.OMS };
            var testObject = mock.Create<MarketplaceAdvisorDownloaderFactory>(TypedParameter.From(store));

            var progress = mock.Mock<IProgressReporter>().Object;
            var dbConnection = mock.Mock<DbConnection>().Object;
            var downloadLog = mock.Build<IDownloadEntity>();

            testObject.Download(progress, downloadLog, dbConnection);
            mock.Mock<IMarketplaceAdvisorOmsDownloader>()
                .Verify(x => x.Download(progress, downloadLog, dbConnection));
        }

        [Fact]
        public void DownloadWithOrderNumber_ReturnsCompletedTask()
        {
            StoreEntity store = new MarketplaceAdvisorStoreEntity();
            var testObject = mock.Create<MarketplaceAdvisorDownloaderFactory>(TypedParameter.From(store));

            var dbConnection = mock.Mock<DbConnection>().Object;

            var task = testObject.Download("blah", 1, dbConnection);

            Assert.Equal(Task.CompletedTask, task);
        }

        [Fact]
        public void ShouldDownload_ReturnsFalse()
        {
            StoreEntity store = new MarketplaceAdvisorStoreEntity();
            var shouldDownload = mock.Create<MarketplaceAdvisorDownloaderFactory>(TypedParameter.From(store)).ShouldDownload("1");
            Assert.False(shouldDownload);
        }

        [SuppressMessage("SonarLint", "S1481: Unused local variables should be removed",
            Justification = "We're testing a getter. We don't need the value but cannot call a getter without storing it")]
        [Fact]
        public void QuantityNew_DelegatesToDownloader()
        {
            StoreEntity store = new MarketplaceAdvisorStoreEntity { AccountType = (int) MarketplaceAdvisorAccountType.OMS };
            var testObject = mock.Create<MarketplaceAdvisorDownloaderFactory>(TypedParameter.From(store));

            var quantity = testObject.QuantityNew;
            mock.Mock<IMarketplaceAdvisorOmsDownloader>().VerifyGet(x => x.QuantityNew);
        }

        [SuppressMessage("SonarLint", "S1481: Unused local variables should be removed",
            Justification = "We're testing a getter. We don't need the value but cannot call a getter without storing it")]
        [Fact]
        public void QuantitySaved_DelegatesToDownloader()
        {
            StoreEntity store = new MarketplaceAdvisorStoreEntity { AccountType = (int) MarketplaceAdvisorAccountType.OMS };
            var testObject = mock.Create<MarketplaceAdvisorDownloaderFactory>(TypedParameter.From(store));

            var quantity = testObject.QuantitySaved;
            mock.Mock<IMarketplaceAdvisorOmsDownloader>().VerifyGet(x => x.QuantitySaved);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
