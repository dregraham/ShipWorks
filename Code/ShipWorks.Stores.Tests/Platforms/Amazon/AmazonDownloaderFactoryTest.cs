using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Amazon
{
    public class AmazonDownloaderFactoryTest : IDisposable
    {
        readonly AutoMock mock;

        public AmazonDownloaderFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Constructor_CreatesLegacyDownloader_WhenApiIsSoap()
        {
            StoreEntity store = new AmazonStoreEntity { AmazonApi = (int) AmazonApi.LegacySoap };
            var factoryMock = mock.MockFunc<StoreEntity, IAmazonLegacyDownloader>();

            mock.Create<AmazonDownloaderFactory>(TypedParameter.From(store));

            factoryMock.Verify(x => x(store));
        }

        [Fact]
        public void Constructor_CreatesMwsDownloader_WhenApiIsMWS()
        {
            StoreEntity store = new AmazonStoreEntity { AmazonApi = (int) AmazonApi.MarketplaceWebService };
            var factoryMock = mock.MockFunc<StoreEntity, IAmazonMwsDownloader>();

            mock.Create<AmazonDownloaderFactory>(TypedParameter.From(store));

            factoryMock.Verify(x => x(store));
        }

        [Fact]
        public void Download_DelegatesToDownloader()
        {
            StoreEntity store = new AmazonStoreEntity { AmazonApi = (int) AmazonApi.MarketplaceWebService };
            var testObject = mock.Create<AmazonDownloaderFactory>(TypedParameter.From(store));

            var progress = mock.Mock<IProgressReporter>().Object;
            var dbConnection = mock.Mock<DbConnection>().Object;

            testObject.Download(progress, 1, dbConnection);
            mock.Mock<IAmazonMwsDownloader>()
                .Verify(x => x.Download(progress, 1, dbConnection));
        }

        [SuppressMessage("SonarLint", "S1481: Unused local variables should be removed",
            Justification = "We're testing a getter. We don't need the value but cannot call a getter without storing it")]
        [Fact]
        public void QuantityNew_DelegatesToDownloader()
        {
            StoreEntity store = new AmazonStoreEntity { AmazonApi = (int) AmazonApi.MarketplaceWebService };
            var testObject = mock.Create<AmazonDownloaderFactory>(TypedParameter.From(store));

            var quantity = testObject.QuantityNew;
            mock.Mock<IAmazonMwsDownloader>().VerifyGet(x => x.QuantityNew);
        }

        [SuppressMessage("SonarLint", "S1481: Unused local variables should be removed",
            Justification = "We're testing a getter. We don't need the value but cannot call a getter without storing it")]
        [Fact]
        public void QuantitySaved_DelegatesToDownloader()
        {
            StoreEntity store = new AmazonStoreEntity { AmazonApi = (int) AmazonApi.MarketplaceWebService };
            var testObject = mock.Create<AmazonDownloaderFactory>(TypedParameter.From(store));

            var quantity = testObject.QuantitySaved;
            mock.Mock<IAmazonMwsDownloader>().VerifyGet(x => x.QuantitySaved);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
