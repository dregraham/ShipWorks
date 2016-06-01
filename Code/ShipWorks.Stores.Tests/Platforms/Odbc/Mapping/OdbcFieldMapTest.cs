using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using log4net;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.IO;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class OdbcFieldMapTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcFieldMapTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void Save_DelegatesToOdbcFieldMapIOFactoryForOdbcFieldMapWriter()
        {
            Mock<IOdbcFieldMapWriter> odbcWriter = mock.Mock<IOdbcFieldMapWriter>();
            Mock<IOdbcFieldMapIOFactory> ioFactory = mock.Mock<IOdbcFieldMapIOFactory>();

            OdbcFieldMap testObject = mock.Create<OdbcFieldMap>();
            ioFactory.Setup(f => f.CreateWriter(testObject)).Returns(odbcWriter.Object);


            testObject.Save(new MemoryStream());

            ioFactory.Verify(f => f.CreateWriter(testObject));
        }

        [Fact]
        public void Save_DelegatesToOdbcFieldMapWriter()
        {
            Mock<IOdbcFieldMapWriter> odbcWriter = mock.Mock<IOdbcFieldMapWriter>();
            Mock<IOdbcFieldMapIOFactory> ioFactory = mock.Mock<IOdbcFieldMapIOFactory>();
            OdbcFieldMap testObject = mock.Create<OdbcFieldMap>();
            ioFactory.Setup(f => f.CreateWriter(testObject)).Returns(odbcWriter.Object);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                testObject.Save(memoryStream);

                odbcWriter.Verify(w => w.Write(memoryStream));
            }
        }

        [Fact]
        public void Load_SetsExternamTableName()
        {
            Stream stream = GetStreamWithFieldMap();
            OdbcFieldMap map = new OdbcFieldMap(GetIoFactory());

            map.Load(stream);

            Assert.Equal("OdbcFieldMapExternalTableName", map.ExternalTableName);
        }

        [Fact]
        public void Load_SetsEntries()
        {
            Stream stream = GetStreamWithFieldMap();
            OdbcFieldMap map = new OdbcFieldMap(GetIoFactory());

            map.Load(stream);

            IOdbcFieldMapEntry entry = map.Entries.FirstOrDefault();

            Assert.Equal("Order Number", entry.ShipWorksField.DisplayName);
            Assert.Equal(OrderFields.OrderNumber.Name, entry.ShipWorksField.Name);
            Assert.Equal(OrderFields.OrderNumber.ContainingObjectName, entry.ShipWorksField.ContainingObjectName);

            Assert.Equal("SomeColumnName", entry.ExternalField.Column.Name);
            Assert.Equal("SomeTableName", entry.ExternalField.Table.Name);
        }

        [Fact]
        public void Load_SetsEntries_WhenThereAreMultipleEntries()
        {
            Stream stream = GetStreamWithFieldMap();
            OdbcFieldMap map = new OdbcFieldMap(GetIoFactory());

            map.Load(stream);

            map.AddEntry(GetFieldMapEntry(GetShipWorksField(OrderFields.BillFirstName, "Bill First Name"),
                GetExternalField("SomeTableName2", "SomeColumnName2")));

            IOdbcFieldMapEntry entry1 = map.Entries.FirstOrDefault();
            Assert.Equal("Order Number", entry1.ShipWorksField.DisplayName);
            Assert.Equal(OrderFields.OrderNumber.Name, entry1.ShipWorksField.Name);
            Assert.Equal(OrderFields.OrderNumber.ContainingObjectName, entry1.ShipWorksField.ContainingObjectName);
            Assert.Equal("SomeColumnName", entry1.ExternalField.Column.Name);
            Assert.Equal("SomeTableName", entry1.ExternalField.Table.Name);

            IOdbcFieldMapEntry entry2 = map.Entries.Skip(1).FirstOrDefault();
            Assert.Equal("Bill First Name", entry2.ShipWorksField.DisplayName);
            Assert.Equal(OrderFields.BillFirstName.Name, entry2.ShipWorksField.Name);
            Assert.Equal(OrderFields.BillFirstName.ContainingObjectName, entry2.ShipWorksField.ContainingObjectName);
            Assert.Equal("SomeColumnName2", entry2.ExternalField.Column.Name);
            Assert.Equal("SomeTableName2", entry2.ExternalField.Table.Name);
        }

        [Fact]
        public void CopyToEntity_WithNullExternalFieldValue_DoesNotCopyToEntity()
        {
            Stream stream = GetStreamWithFieldMap();
            OdbcFieldMap map = new OdbcFieldMap(GetIoFactory());

            // Load a stream with the order number mapped to a field whos value is null
            map.Load(stream);
            map.AddEntry(GetFieldMapEntry(GetShipWorksField(OrderFields.BillFirstName, "Bill First Name"),
                GetExternalField("SomeTableName2", "SomeColumnName2")));

            var record = new OdbcRecord();
            record.AddField("SomeColumnName2", "Mirza");

            // Load some values into the map
            map.ApplyValues(record);

            OrderEntity order = new OrderEntity {OrderNumber = 123};
            map.CopyToEntity(order);

            // assert that we did not overwrite the order number with null
            Assert.Equal(123, order.OrderNumber);
        }

        [Fact]
        public void CopyToEntity_CopiesValuesToEntity()
        {
            Stream stream = GetStreamWithFieldMap();
            OdbcFieldMap map = new OdbcFieldMap(GetIoFactory());

            map.Load(stream);

            map.AddEntry(GetFieldMapEntry(GetShipWorksField(OrderFields.BillFirstName, "Bill First Name"),
                GetExternalField("SomeTableName2", "SomeColumnName2")));

            var record = new OdbcRecord();
            record.AddField("SomeColumnName2", "Mirza");

            // Load some values into the map
            map.ApplyValues(record);

            OrderEntity order = new OrderEntity();

            map.CopyToEntity(order);

            Assert.Equal("Mirza", order.BillFirstName);
        }

        [Fact]
        public void ApplyValues_AppliesValuesToEntries()
        {
            Stream stream = GetStreamWithFieldMap();
            OdbcFieldMap map = new OdbcFieldMap(GetIoFactory());

            map.Load(stream);

            map.AddEntry(GetFieldMapEntry(GetShipWorksField(OrderFields.BillFirstName, "Bill First Name"),
                GetExternalField("SomeTableName2", "SomeColumnName2")));

            var record = new OdbcRecord();
            record.AddField("SomeColumnName2", "Mirza");

            map.ApplyValues(record);

            IOdbcFieldMapEntry testObject = map.Entries.FirstOrDefault(e => e.ExternalField?.Value?.ToString() == "Mirza");

            Assert.NotNull(testObject);
        }

        [Fact]
        public void ResetValues_ResetsEntryValues()
        {
            Stream stream = GetStreamWithFieldMap();
            OdbcFieldMap map = new OdbcFieldMap(GetIoFactory());

            map.Load(stream);

            map.AddEntry(GetFieldMapEntry(GetShipWorksField(OrderFields.BillFirstName, "Bill First Name"),
                GetExternalField("SomeTableName2", "SomeColumnName2")));

            var record = new OdbcRecord();
            record.AddField("SomeColumnName2", "Mirza");

            map.ApplyValues(record);

            // Assert that the map has values
            IOdbcFieldMapEntry testObject = map.Entries.FirstOrDefault(e => e.ExternalField?.Value?.ToString() == "Mirza");
            Assert.NotNull(testObject);

            // reset the values
            map.ResetValues();

            // Assert that the map no longer has values
            IOdbcFieldMapEntry testObject2 = map.Entries.FirstOrDefault(e => e.ExternalField?.Value?.ToString() == "Mirza");
            Assert.Null(testObject2);
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
            var log = mock.Mock<ILog>();

            var ioFactory = mock.Mock<IOdbcFieldMapIOFactory>();

            ioFactory.Setup(f => f.CreateWriter(It.IsAny<OdbcFieldMap>())).Returns((OdbcFieldMap m) => new JsonOdbcFieldMapWriter(m));
            ioFactory.Setup(f => f.CreateReader(It.IsAny<Stream>())).Returns<Stream>(s => new JsonOdbcFieldMapReader(s.ConvertToString(), log.Object));

            return ioFactory.Object;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}