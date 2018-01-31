using System;
using System.Collections;
using System.Collections.Generic;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Download
{
    public class OdbcStoreDownloaderTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcStoreDownloaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData("int64", "5", true)]
        [InlineData("int64", "a", false)]
        [InlineData("int64", "5.5", false)]
        [InlineData("bigint", "5", true)]
        [InlineData("bigint", "abc", false)]
        [InlineData("nvarchar(17)", "5", true)]
        [InlineData("nvarchar(17)", "abcd", true)]
        [InlineData("int(2)", "5", true)]
        [InlineData("int identity", "5", true)]
        [InlineData("int(2)", "a", false)]
        [InlineData("int identity", "b", false)]
        [InlineData("INT64", "5", true)]
        [InlineData("INT64", "a", false)]
        public void ShouldDownload_ReturnsTrue_WhenOrderNumberCompleteIsMappedToNonNumericField(string type, string orderNumber, bool shouldDownload)
        {
            OdbcStoreEntity storeEntity = new OdbcStoreEntity();
            mock.Provide<StoreEntity>(storeEntity);

            IEnumerable<IOdbcFieldMapEntry> fieldMapEntries = new[]
            {
                new OdbcFieldMapEntry(
                    new ShipWorksOdbcMappableField(OrderFields.OrderNumberComplete, OdbcOrderFieldDescription.Number,
                        OdbcFieldValueResolutionStrategy.Default),
                    new ExternalOdbcMappableField(new OdbcColumn("name", type)))
            };

            mock.Mock<IOdbcFieldMap>()
                .Setup(f => f.FindEntriesBy(It.IsAny<EntityField2>()))
                .Returns(fieldMapEntries);

            var testObject = mock.Create<OdbcStoreDownloader>();
            Assert.Equal(shouldDownload, testObject.ShouldDownload(orderNumber));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}