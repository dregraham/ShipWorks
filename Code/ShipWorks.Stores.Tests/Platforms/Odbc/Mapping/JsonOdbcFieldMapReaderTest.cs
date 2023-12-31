﻿using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Extensions;
using log4net;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
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
        public void Constructor_ThrowsArgumentNullException_WhenSerializedMapIsNull()
        {
            string map = null;

            Assert.Throws<ArgumentNullException>(() =>new JsonOdbcFieldMapReader(map, log.Object));
        }

        [Fact]
        public void Constructor_ThrowsShipWorksOdbcException_WhenJsonReaderExceptionIsCaught()
        {
            string map = string.Empty;

            Assert.Throws<ShipWorksOdbcException>(() => new JsonOdbcFieldMapReader(map, log.Object));
        }

        [Fact]
        public void ReadEntry_ThrowsShipWorksOdbcException_WhenExceptionIsCaught()
        {
            var map = "{Not:\"A Map\"}";
            JsonOdbcFieldMapReader reader = new JsonOdbcFieldMapReader(map, log.Object);

            Assert.Throws<ShipWorksOdbcException>(() => reader.ReadEntry());
        }

        [Fact]
        public void ReadEntry_LogsError_WhenExceptionIsCaught()
        {
            var map = "{Not:\"A Map\"}";
            JsonOdbcFieldMapReader reader = new JsonOdbcFieldMapReader(map, log.Object);
            try
            {
                reader.ReadEntry();
            }
            catch (Exception)
            {
                log.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
            }
        }

        [Fact]
        public void ReadyEntry_ReturnsOdbcFieldMapEntry()
        {
            var map = GetSerializedFieldMap();
            JsonOdbcFieldMapReader reader = new JsonOdbcFieldMapReader(map, log.Object);

            OdbcFieldMapEntry entry = reader.ReadEntry();

            Assert.Equal(typeof(OdbcFieldMapEntry), entry.GetType());
        }

        [Fact]
        public void ReadyEntry_ReturnsNullWhenNoMoreEntriesExist()
        {
            var map = GetSerializedFieldMap();
            JsonOdbcFieldMapReader reader = new JsonOdbcFieldMapReader(map, log.Object);

            reader.ReadEntry();
            OdbcFieldMapEntry entry2 = reader.ReadEntry();

            Assert.Null(entry2);
        }

        [Fact]
        public void ReadRecordIdentifierSource_ReturnsEmptyString_WhenExceptionIsCaught()
        {
            var map = "{Not:\"A Map\"}";
            JsonOdbcFieldMapReader reader = new JsonOdbcFieldMapReader(map, log.Object);

            Assert.Equal(string.Empty, reader.ReadRecordIdentifierSource());
        }

        [Fact]
        public void ReadRecordIdentifierSource_ReturnsRecordIdentifierSourceFromMap()
        {
            var map = "{RecordIdentifierSource:\"ID\"}";
            JsonOdbcFieldMapReader reader = new JsonOdbcFieldMapReader(map, log.Object);

            Assert.Equal("ID", reader.ReadRecordIdentifierSource());
        }

        private string GetSerializedFieldMap()
        {
            OdbcFieldMap map =
                mock.Create<OdbcFieldMap>(new TypedParameter(typeof(IOdbcFieldMapIOFactory), GetIoFactory()));

            map.AddEntry(GetFieldMapEntry(GetShipWorksField(OrderFields.OrderNumber, "Order Number"),
                GetExternalField("SomeColumnName")));

            return map.Serialize();
        }

        private OdbcFieldMapEntry GetFieldMapEntry(ShipWorksOdbcMappableField shipworksField, ExternalOdbcMappableField externalField)
        {
            return new OdbcFieldMapEntry(shipworksField, externalField);
        }

        private ShipWorksOdbcMappableField GetShipWorksField(EntityField2 field, string displayName)
        {
            return new ShipWorksOdbcMappableField(field, displayName, OdbcFieldValueResolutionStrategy.Default);
        }

        private ExternalOdbcMappableField GetExternalField(string columnName)
        {
            return new ExternalOdbcMappableField(new OdbcColumn(columnName, "unknown"));
        }

        private IOdbcFieldMapIOFactory GetIoFactory()
        {
            var ioFactory = mock.Mock<IOdbcFieldMapIOFactory>();

            ioFactory.Setup(f => f.CreateWriter(It.IsAny<OdbcFieldMap>()))
                .Returns((OdbcFieldMap m) => new JsonOdbcFieldMapSerializer(m));
            ioFactory.Setup(f => f.CreateReader(It.IsAny<string>())).Returns<Stream>(s => new JsonOdbcFieldMapReader(s.ConvertToString(), log.Object));

            return ioFactory.Object;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}