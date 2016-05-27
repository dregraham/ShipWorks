using Autofac.Extras.Moq;
using log4net;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.IO;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class JsonOdbcFieldMapReaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<ILog> log;

        public JsonOdbcFieldMapReaderTest()
        {
            mock = AutoMock.GetLoose();
            log = mock.Mock<ILog>();
        }

        [Fact]
        public void ReadExternalTableName_ReturnsExternalName()
        {
            Stream stream = GetStreamWithFieldMap();
            JsonOdbcFieldMapReader reader = new JsonOdbcFieldMapReader(stream, log.Object);

            Assert.Equal("OdbcFieldMapExternalTableName", reader.ReadExternalTableName());
        }

        [Fact]
        public void ReadyEntry_ReturnsOdbcFieldMapEntry()
        {
            Stream stream = GetStreamWithFieldMap();
            JsonOdbcFieldMapReader reader = new JsonOdbcFieldMapReader(stream, log.Object);

            OdbcFieldMapEntry entry = reader.ReadEntry();

            Assert.Equal(typeof(OdbcFieldMapEntry), entry.GetType());
        }

        [Fact]
        public void ReadyEntry_ReturnsNullWhenNoMoreEntriesExist()
        {
            Stream stream = GetStreamWithFieldMap();
            JsonOdbcFieldMapReader reader = new JsonOdbcFieldMapReader(stream, log.Object);

            reader.ReadEntry();
            OdbcFieldMapEntry entry2 = reader.ReadEntry();

            Assert.Null(entry2);
        }

        private Stream GetStreamWithFieldMap()
        {
            MemoryStream stream = new MemoryStream();

            OdbcFieldMap map = new OdbcFieldMap(GetIoFactory())
            {
                ExternalTableName = "OdbcFieldMapExternalTableName"
            };

            map.AddEntry(GetFieldMapEntry(GetShipWorksField(OrderFields.OrderNumber, "Order Number"),
                GetExternalField("SomeTableName", "SomeColumnName")));

            map.Save(stream);

            return stream;
        }

        private OdbcFieldMapEntry GetFieldMapEntry(ShipWorksOdbcMappableField shipworksField, ExternalOdbcMappableField externalField)
        {
            return new OdbcFieldMapEntry(shipworksField, externalField);
        }

        private ShipWorksOdbcMappableField GetShipWorksField(EntityField2 field, string displayName)
        {
            return new ShipWorksOdbcMappableField(field, displayName);
        }

        private ExternalOdbcMappableField GetExternalField(string tableName, string columnName)
        {
            return new ExternalOdbcMappableField(new OdbcTable(tableName), new OdbcColumn(columnName));
        }

        private IOdbcFieldMapIOFactory GetIoFactory()
        {
            var ioFactory = mock.Mock<IOdbcFieldMapIOFactory>();

            ioFactory.Setup(f => f.CreateWriter(It.IsAny<OdbcFieldMap>()))
                .Returns((OdbcFieldMap m) => new JsonOdbcFieldMapWriter(m));
            ioFactory.Setup(f => f.CreateReader(It.IsAny<Stream>())).Returns<Stream>(s => new JsonOdbcFieldMapReader(s, log.Object));

            return ioFactory.Object;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}