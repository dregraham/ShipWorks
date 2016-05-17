using System;
using System.IO;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class JsonOdbcFieldMapReaderTest : IDisposable
    {
        private readonly AutoMock mock;

        public JsonOdbcFieldMapReaderTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void ReadDisplayName_ReturnsDisplayName()
        {
            var log = mock.Mock<ILog>();

            MemoryStream stream = new MemoryStream();
            StreamWriter sw = new StreamWriter(stream);
            sw.Write("{\"Entries\":[{\"ShipWorksField\":{\"ElementName\":\"OrderEntity\",\"ElementFieldValue\":\"OrderNumber\",\"DisplayName\":\"Order Number\"},\"ExternalField\":{\"Table\":{\"Name\":\"some table\"},\"Column\":{\"Name\":\"OrderNumberColumn\"},\"DisplayName\":\"some table OrderNumberColumn\"}}],\"DisplayName\":\"some display name\",\"ExternalTableName\":\"some external tablename\"}");
            sw.Flush();

            JsonOdbcFieldMapReader reader = new JsonOdbcFieldMapReader(stream, log.Object);

            Assert.Equal("", reader.ReadDisplayName());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}