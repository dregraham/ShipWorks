using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Magento
{
    public class MagentoDownloaderFactoryTest : IDisposable
    {
        readonly AutoMock mock;

        public MagentoDownloaderFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(MagentoVersion.MagentoConnect)]
        [InlineData(MagentoVersion.MagentoTwo)]
        [InlineData(MagentoVersion.PhpFile)]
        public void Constructor_CreatesModuleDownloader_WhenVersionIsNotTwoRest(MagentoVersion version)
        {
            StoreEntity store = new MagentoStoreEntity { MagentoVersion = (int) version };
            var factoryMock = mock.MockFunc<StoreEntity, IMagentoModuleDownloader>();

            mock.Create<MagentoDownloaderFactory>(TypedParameter.From(store));

            factoryMock.Verify(x => x(store));
        }

        [Fact]
        public void Constructor_CreatesRestDownloader_WhenVersionIsTwoRest()
        {
            StoreEntity store = new MagentoStoreEntity { MagentoVersion = (int) MagentoVersion.MagentoTwoREST };
            var factoryMock = mock.MockFunc<StoreEntity, IMagentoTwoRestDownloader>();

            mock.Create<MagentoDownloaderFactory>(TypedParameter.From(store));

            factoryMock.Verify(x => x(store));
        }

        [Fact]
        public void Download_DelegatesToDownloader()
        {
            StoreEntity store = new MagentoStoreEntity { MagentoVersion = (int) MagentoVersion.MagentoTwoREST };
            var testObject = mock.Create<MagentoDownloaderFactory>(TypedParameter.From(store));

            var progress = mock.Mock<IProgressReporter>().Object;
            var dbConnection = mock.Mock<DbConnection>().Object;

            testObject.Download(progress, 1, dbConnection);
            mock.Mock<IMagentoTwoRestDownloader>()
                .Verify(x => x.Download(progress, 1, dbConnection));
        }

        [SuppressMessage("SonarLint", "S1481: Unused local variables should be removed",
            Justification = "We're testing a getter. We don't need the value but cannot call a getter without storing it")]
        [Fact]
        public void QuantityNew_DelegatesToDownloader()
        {
            StoreEntity store = new MagentoStoreEntity { MagentoVersion = (int) MagentoVersion.MagentoTwoREST };
            var testObject = mock.Create<MagentoDownloaderFactory>(TypedParameter.From(store));

            var quantity = testObject.QuantityNew;
            mock.Mock<IMagentoTwoRestDownloader>().VerifyGet(x => x.QuantityNew);
        }

        [SuppressMessage("SonarLint", "S1481: Unused local variables should be removed",
            Justification = "We're testing a getter. We don't need the value but cannot call a getter without storing it")]
        [Fact]
        public void QuantitySaved_DelegatesToDownloader()
        {
            StoreEntity store = new MagentoStoreEntity { MagentoVersion = (int) MagentoVersion.MagentoTwoREST };
            var testObject = mock.Create<MagentoDownloaderFactory>(TypedParameter.From(store));

            var quantity = testObject.QuantitySaved;
            mock.Mock<IMagentoTwoRestDownloader>().VerifyGet(x => x.QuantitySaved);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
