﻿using System;
using System.IO;
using Autofac;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
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
        public void Write_WritesSerializedMapToStream()
        {
            JsonOdbcFieldMapWriter testObject = new JsonOdbcFieldMapWriter();

            Mock<IOdbcFieldMapIOFactory> ioFactory = mock.Mock<IOdbcFieldMapIOFactory>();
            ioFactory.Setup(f => f.CreateWriter()).Returns(testObject);

            OdbcFieldMap map = new OdbcFieldMap(ioFactory.Object);

            

            OdbcTable table = new OdbcTable("some table");
            OdbcColumn column = new OdbcColumn("OrderNumberColumn");

            ExternalOdbcMappableField externalOdbcMappableField = new ExternalOdbcMappableField(table, column);
            ShipWorksOdbcMappableField shipworksOdbcMappableField = new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number");

            OdbcFieldMapEntry entry = new OdbcFieldMapEntry(shipworksOdbcMappableField, externalOdbcMappableField);
            map.ExternalTableName = "some external tablename";
            map.DisplayName = "some display name";

            map.AddEntry(entry);

            MemoryStream stream = new MemoryStream();

            testObject.Write(map, stream);

            stream.Position = 0;

            var streamreader = new StreamReader(stream);

            string result = streamreader.ReadToEnd();
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}