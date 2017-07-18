using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using Autofac.Core;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericFile;
using ShipWorks.Stores.Platforms.GenericFile.Formats.Csv;
using ShipWorks.Stores.Platforms.GenericFile.Formats.Excel;
using ShipWorks.Stores.Platforms.GenericFile.Formats.Xml;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.GenericFile
{
    public class GenericFileDownloaderFactoryTest : IDisposable
    {
        readonly AutoMock mock;

        public GenericFileDownloaderFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Constructor_CreatesCsvDownloader_WhenFormatIsCsv()
        {
            GenericFileStoreEntity store = new GenericFileStoreEntity { FileFormat = (int) GenericFileFormat.Csv };
            var factoryMock = mock.MockFunc<GenericFileStoreEntity, IGenericFileCsvDownloader>();

            mock.Create<GenericFileDownloaderFactory>(TypedParameter.From<StoreEntity>(store));

            factoryMock.Verify(x => x(store));
        }

        [Fact]
        public void Constructor_CreatesExcelDownloader_WhenFormatIsExcel()
        {
            GenericFileStoreEntity store = new GenericFileStoreEntity { FileFormat = (int) GenericFileFormat.Excel };
            var factoryMock = mock.MockFunc<GenericFileStoreEntity, IGenericFileExcelDownloader>();

            mock.Create<GenericFileDownloaderFactory>(TypedParameter.From<StoreEntity>(store));

            factoryMock.Verify(x => x(store));
        }

        [Fact]
        public void Constructor_CreatesXmlDownloader_WhenFormatIsXml()
        {
            GenericFileStoreEntity store = new GenericFileStoreEntity { FileFormat = (int) GenericFileFormat.Xml };
            var factoryMock = mock.MockFunc<GenericFileStoreEntity, IGenericFileXmlDownloader>();

            mock.Create<GenericFileDownloaderFactory>(TypedParameter.From<StoreEntity>(store));

            factoryMock.Verify(x => x(store));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(6)]
        public void Constructor_ThrowsInvalidOperationException_WhenFormatIsInvalid(int format)
        {
            GenericFileStoreEntity store = new GenericFileStoreEntity { FileFormat = format };

            var typedException = Assert.Throws<DependencyResolutionException>(() =>
                mock.Create<GenericFileDownloaderFactory>(TypedParameter.From<StoreEntity>(store)));
            Assert.IsAssignableFrom<InvalidOperationException>(typedException.GetBaseException());
        }

        [Fact]
        public void Download_DelegatesToDownloader()
        {
            StoreEntity store = new GenericFileStoreEntity { FileFormat = (int) GenericFileFormat.Xml };
            var testObject = mock.Create<GenericFileDownloaderFactory>(TypedParameter.From(store));

            var progress = mock.Mock<IProgressReporter>().Object;
            var dbConnection = mock.Mock<DbConnection>().Object;

            testObject.Download(progress, 1, dbConnection);
            mock.Mock<IGenericFileXmlDownloader>()
                .Verify(x => x.Download(progress, 1, dbConnection));
        }

        [SuppressMessage("SonarLint", "S1481: Unused local variables should be removed",
            Justification = "We're testing a getter. We don't need the value but cannot call a getter without storing it")]
        [Fact]
        public void QuantityNew_DelegatesToDownloader()
        {
            StoreEntity store = new GenericFileStoreEntity { FileFormat = (int) GenericFileFormat.Xml };
            var testObject = mock.Create<GenericFileDownloaderFactory>(TypedParameter.From(store));

            var quantity = testObject.QuantityNew;
            mock.Mock<IGenericFileXmlDownloader>().VerifyGet(x => x.QuantityNew);
        }

        [SuppressMessage("SonarLint", "S1481: Unused local variables should be removed",
            Justification = "We're testing a getter. We don't need the value but cannot call a getter without storing it")]
        [Fact]
        public void QuantitySaved_DelegatesToDownloader()
        {
            StoreEntity store = new GenericFileStoreEntity { FileFormat = (int) GenericFileFormat.Xml };
            var testObject = mock.Create<GenericFileDownloaderFactory>(TypedParameter.From(store));

            var quantity = testObject.QuantitySaved;
            mock.Mock<IGenericFileXmlDownloader>().VerifyGet(x => x.QuantitySaved);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
