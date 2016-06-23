using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.IO;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class JsonOdbcFieldMapWriterTest : IDisposable
    {
        private readonly AutoMock mock;

        public JsonOdbcFieldMapWriterTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void Write_ThrowsArgumentNullException_WhenStreamIsNull()
        {
            OdbcFieldMap map = mock.Create<OdbcFieldMap>();
            JsonOdbcFieldMapWriter testObject = new JsonOdbcFieldMapWriter(map);

            Assert.Throws<ArgumentNullException>(() => testObject.Write(null));
        }

        [Fact]
        public void JsonOdbcFieldMapWriter_ThrowsArgumentNullException_WhenMapIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new JsonOdbcFieldMapWriter(null));
        }

        [Fact]
        public void Write_WritesSerializedMapToStream()
        {
            string expectedResult =
                "{\"Entries\":[{\"Index\":0,\"ShipWorksField\":{\"ContainingObjectName\":\"OrderEntity\",\"Name\":\"OrderNumber\",\"DisplayName\":\"Order Number\"},\"ExternalField\":{\"Table\":{\"Name\":\"some table\"},\"Column\":{\"Name\":\"OrderNumberColumn\"},\"DisplayName\":\"some table OrderNumberColumn\"}}]}";

            Mock<IOdbcFieldMapIOFactory> ioFactory = mock.Mock<IOdbcFieldMapIOFactory>();
            OdbcFieldMap map = new OdbcFieldMap(ioFactory.Object);

            JsonOdbcFieldMapWriter testObject = new JsonOdbcFieldMapWriter(map);
            ioFactory.Setup(f => f.CreateWriter(map)).Returns(testObject);

            OdbcTable table = new OdbcTable("some table");
            OdbcColumn column = new OdbcColumn("OrderNumberColumn");

            ExternalOdbcMappableField externalOdbcMappableField = new ExternalOdbcMappableField(table, column);
            ShipWorksOdbcMappableField shipworksOdbcMappableField =
                new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number");

            OdbcFieldMapEntry entry = new OdbcFieldMapEntry(shipworksOdbcMappableField, externalOdbcMappableField);

            map.AddEntry(entry);

            using (MemoryStream stream = new MemoryStream())
            {
                testObject.Write(stream);
                stream.Position = 0;
                using (var streamReader = new StreamReader(stream))
                {
                    Assert.Equal(expectedResult, streamReader.ReadToEnd());
                }
            }
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}