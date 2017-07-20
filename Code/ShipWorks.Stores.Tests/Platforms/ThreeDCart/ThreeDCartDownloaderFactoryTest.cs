using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ThreeDCart;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ThreeDCart
{
    public class ThreeDCartDownloaderFactoryTest : IDisposable
    {
        readonly AutoMock mock;

        public ThreeDCartDownloaderFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Constructor_CreatesLegacyDownloader_WhenApiIsSoap()
        {
            ThreeDCartStoreEntity store = new ThreeDCartStoreEntity { RestUser = false };
            var factoryMock = mock.MockFunc<ThreeDCartStoreEntity, IThreeDCartSoapDownloader>();

            mock.Create<ThreeDCartDownloaderFactory>(TypedParameter.From<StoreEntity>(store));

            factoryMock.Verify(x => x(store));
        }

        [Fact]
        public void Constructor_CreatesRestDownloader_WhenApiIsMWS()
        {
            ThreeDCartStoreEntity store = new ThreeDCartStoreEntity { RestUser = true };
            var factoryMock = mock.MockFunc<ThreeDCartStoreEntity, IThreeDCartRestDownloader>();

            mock.Create<ThreeDCartDownloaderFactory>(TypedParameter.From<StoreEntity>(store));

            factoryMock.Verify(x => x(store));
        }

        [Fact]
        public void Download_DelegatesToDownloader()
        {
            StoreEntity store = new ThreeDCartStoreEntity { RestUser = true };
            var testObject = mock.Create<ThreeDCartDownloaderFactory>(TypedParameter.From(store));

            var progress = mock.Mock<IProgressReporter>().Object;
            var dbConnection = mock.Mock<DbConnection>().Object;

            testObject.Download(progress, 1, dbConnection);
            mock.Mock<IThreeDCartRestDownloader>()
                .Verify(x => x.Download(progress, 1, dbConnection));
        }

        [SuppressMessage("SonarLint", "S1481: Unused local variables should be removed",
            Justification = "We're testing a getter. We don't need the value but cannot call a getter without storing it")]
        [Fact]
        public void QuantityNew_DelegatesToDownloader()
        {
            StoreEntity store = new ThreeDCartStoreEntity { RestUser = true };
            var testObject = mock.Create<ThreeDCartDownloaderFactory>(TypedParameter.From(store));

            var quantity = testObject.QuantityNew;
            mock.Mock<IThreeDCartRestDownloader>().VerifyGet(x => x.QuantityNew);
        }

        [SuppressMessage("SonarLint", "S1481: Unused local variables should be removed",
            Justification = "We're testing a getter. We don't need the value but cannot call a getter without storing it")]
        [Fact]
        public void QuantitySaved_DelegatesToDownloader()
        {
            StoreEntity store = new ThreeDCartStoreEntity { RestUser = true };
            var testObject = mock.Create<ThreeDCartDownloaderFactory>(TypedParameter.From(store));

            var quantity = testObject.QuantitySaved;
            mock.Mock<IThreeDCartRestDownloader>().VerifyGet(x => x.QuantitySaved);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
