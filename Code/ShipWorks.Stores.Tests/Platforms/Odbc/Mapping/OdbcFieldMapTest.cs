using Autofac.Extras.Moq;
using Interapptive.Shared.Extensions;
using log4net;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac.Features.Indexed;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Upload.FieldValueResolvers;
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
        public void Load_SetsEntries_WhenPassedStream()
        {
            Mock<IOdbcFieldValueResolver> resolver = mock.Mock<IOdbcFieldValueResolver>();

            Mock<IIndex<OdbcFieldValueResolutionStrategy, IOdbcFieldValueResolver>> repo = mock.MockRepository.Create<IIndex<OdbcFieldValueResolutionStrategy, IOdbcFieldValueResolver>>();
            repo.Setup(x => x[It.IsAny<OdbcFieldValueResolutionStrategy>()]).Returns(resolver.Object);
            mock.Provide(repo.Object);

            Stream stream = GetStreamWithFieldMap();
            OdbcFieldMap map = mock.Create<OdbcFieldMap>();

            map.Load(stream);

            IOdbcFieldMapEntry entry = map.Entries.FirstOrDefault();

            Assert.Equal("Order Number", entry.ShipWorksField.DisplayName);
            Assert.Equal(OrderFields.OrderNumber.Name, entry.ShipWorksField.Name);
            Assert.Equal(OrderFields.OrderNumber.ContainingObjectName, entry.ShipWorksField.ContainingObjectName);

            Assert.Equal("SomeColumnName", entry.ExternalField.Column.Name);
        }

        [Fact]
        public void Load_SetsEntries_WhenPassedStreamAndThereAreMultipleEntries()
        {
            Mock<IOdbcFieldValueResolver> resolver = mock.Mock<IOdbcFieldValueResolver>();

            Mock<IIndex<OdbcFieldValueResolutionStrategy, IOdbcFieldValueResolver>> repo = mock.MockRepository.Create<IIndex<OdbcFieldValueResolutionStrategy, IOdbcFieldValueResolver>>();
            repo.Setup(x => x[It.IsAny<OdbcFieldValueResolutionStrategy>()]).Returns(resolver.Object);
            mock.Provide(repo.Object);

            Stream stream = GetStreamWithFieldMap();
            OdbcFieldMap map = mock.Create<OdbcFieldMap>();

            map.Load(stream);

            map.AddEntry(GetFieldMapEntry(GetShipWorksField(OrderFields.BillFirstName, "Bill First Name"),
                GetExternalField("SomeColumnName2")));

            IOdbcFieldMapEntry entry1 = map.Entries.FirstOrDefault();
            Assert.Equal("Order Number", entry1.ShipWorksField.DisplayName);
            Assert.Equal(OrderFields.OrderNumber.Name, entry1.ShipWorksField.Name);
            Assert.Equal(OrderFields.OrderNumber.ContainingObjectName, entry1.ShipWorksField.ContainingObjectName);
            Assert.Equal("SomeColumnName", entry1.ExternalField.Column.Name);

            IOdbcFieldMapEntry entry2 = map.Entries.Skip(1).FirstOrDefault();
            Assert.Equal("Bill First Name", entry2.ShipWorksField.DisplayName);
            Assert.Equal(OrderFields.BillFirstName.Name, entry2.ShipWorksField.Name);
            Assert.Equal(OrderFields.BillFirstName.ContainingObjectName, entry2.ShipWorksField.ContainingObjectName);
            Assert.Equal("SomeColumnName2", entry2.ExternalField.Column.Name);
        }

        [Fact]
        public void Load_SetsEntries_WhenPassedSerializedMap()
        {
            Mock<IOdbcFieldValueResolver> resolver = mock.Mock<IOdbcFieldValueResolver>();

            Mock<IIndex<OdbcFieldValueResolutionStrategy, IOdbcFieldValueResolver>> repo = mock.MockRepository.Create<IIndex<OdbcFieldValueResolutionStrategy, IOdbcFieldValueResolver>>();
            repo.Setup(x => x[It.IsAny<OdbcFieldValueResolutionStrategy>()]).Returns(resolver.Object);
            mock.Provide(repo.Object);

            string stream = GetStreamWithFieldMap().ConvertToString();
            OdbcFieldMap map = mock.Create<OdbcFieldMap>();

            map.Load(stream);

            IOdbcFieldMapEntry entry = map.Entries.FirstOrDefault();

            Assert.Equal("Order Number", entry.ShipWorksField.DisplayName);
            Assert.Equal(OrderFields.OrderNumber.Name, entry.ShipWorksField.Name);
            Assert.Equal(OrderFields.OrderNumber.ContainingObjectName, entry.ShipWorksField.ContainingObjectName);

            Assert.Equal("SomeColumnName", entry.ExternalField.Column.Name);
        }

        [Fact]
        public void Load_SetsEntries_WhenPassedSerializedMapAndThereAreMultipleEntries()
        {
            Mock<IOdbcFieldValueResolver> resolver = mock.Mock<IOdbcFieldValueResolver>();

            Mock<IIndex<OdbcFieldValueResolutionStrategy, IOdbcFieldValueResolver>> repo = mock.MockRepository.Create<IIndex<OdbcFieldValueResolutionStrategy, IOdbcFieldValueResolver>>();
            repo.Setup(x => x[It.IsAny<OdbcFieldValueResolutionStrategy>()]).Returns(resolver.Object);
            mock.Provide(repo.Object);

            string stream = GetStreamWithFieldMap().ConvertToString();
            OdbcFieldMap map = mock.Create<OdbcFieldMap>();

            map.Load(stream);

            map.AddEntry(GetFieldMapEntry(GetShipWorksField(OrderFields.BillFirstName, "Bill First Name"),
                GetExternalField("SomeColumnName2")));

            IOdbcFieldMapEntry entry1 = map.Entries.FirstOrDefault();
            Assert.Equal("Order Number", entry1.ShipWorksField.DisplayName);
            Assert.Equal(OrderFields.OrderNumber.Name, entry1.ShipWorksField.Name);
            Assert.Equal(OrderFields.OrderNumber.ContainingObjectName, entry1.ShipWorksField.ContainingObjectName);
            Assert.Equal("SomeColumnName", entry1.ExternalField.Column.Name);

            IOdbcFieldMapEntry entry2 = map.Entries.Skip(1).FirstOrDefault();
            Assert.Equal("Bill First Name", entry2.ShipWorksField.DisplayName);
            Assert.Equal(OrderFields.BillFirstName.Name, entry2.ShipWorksField.Name);
            Assert.Equal(OrderFields.BillFirstName.ContainingObjectName, entry2.ShipWorksField.ContainingObjectName);
            Assert.Equal("SomeColumnName2", entry2.ExternalField.Column.Name);
        }

        [Fact]
        public void CopyToEntity_SetsFieldToEmptyString_WhenExternalFieldValueIsEmptyString()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcFieldMap>();

                var shipworksField = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksField.Setup(e => e.Value).Returns(string.Empty);
                shipworksField.Setup(e => e.Name).Returns("BillFirstName");
                shipworksField.Setup(e => e.ContainingObjectName).Returns("OrderEntity");

                var entry = mock.Mock<IOdbcFieldMapEntry>();
                entry.Setup(e => e.ShipWorksField).Returns(shipworksField.Object);


                testObject.AddEntry(entry.Object);

                OrderEntity order = new OrderEntity {BillFirstName = "bob"};

                testObject.CopyToEntity(order);

                Assert.Empty(order.BillFirstName);
            }
        }

        [Fact]
        public void CopyToEntity_DoesNotCopyToEntity_WhenContainingObjectNameNotEntityName()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcFieldMap>();

                var shipworksField = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksField.Setup(e => e.Value).Returns("bill");
                shipworksField.Setup(e => e.Name).Returns("BillFirstName");
                shipworksField.Setup(e => e.ContainingObjectName).Returns("OrderItemEntity");

                var entry = mock.Mock<IOdbcFieldMapEntry>();
                entry.Setup(e => e.ShipWorksField).Returns(shipworksField.Object);


                testObject.AddEntry(entry.Object);

                OrderEntity order = new OrderEntity { BillFirstName = "bob" };

                testObject.CopyToEntity(order);

                Assert.Equal("bob", order.BillFirstName);
            }
        }

        [Fact]
        public void CopyToEntity_DoesNotCopyToEntity_WhenIndexDoesNotMatchPassedIndex()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcFieldMap>();

                var shipworksField = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksField.Setup(e => e.Value).Returns("joe");
                shipworksField.Setup(e => e.Name).Returns("BillFirstName");
                shipworksField.Setup(e => e.ContainingObjectName).Returns("OrderEntity");

                var entry = mock.Mock<IOdbcFieldMapEntry>();
                entry.Setup(e => e.ShipWorksField).Returns(shipworksField.Object);

                testObject.AddEntry(entry.Object);

                OrderEntity order = new OrderEntity { BillFirstName = "bob" };

                testObject.CopyToEntity(order, 1);

                Assert.Equal("bob", order.BillFirstName);
            }
        }

        [Fact]
        public void CopyToEntity_CopiesToEntity_WhenIndexMatchesPassedIndex()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcFieldMap>();

                var shipworksField = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksField.Setup(e => e.Value).Returns("joe");
                shipworksField.Setup(e => e.Name).Returns("BillFirstName");
                shipworksField.Setup(e => e.ContainingObjectName).Returns("OrderEntity");

                var entry = mock.Mock<IOdbcFieldMapEntry>();
                entry.Setup(e => e.ShipWorksField).Returns(shipworksField.Object);
                entry.Setup(e => e.Index).Returns(1);

                testObject.AddEntry(entry.Object);

                OrderEntity order = new OrderEntity { BillFirstName = "bob" };

                testObject.CopyToEntity(order, 1);

                Assert.Equal("joe", order.BillFirstName);
            }
        }

        [Fact]
        public void CopyToEntity_CopiesValuesToEntity()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcFieldMap>();

                var shipworksField = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksField.Setup(e => e.Value).Returns("joe");
                shipworksField.Setup(e => e.Name).Returns("BillFirstName");
                shipworksField.Setup(e => e.ContainingObjectName).Returns("OrderEntity");

                var entry = mock.Mock<IOdbcFieldMapEntry>();
                entry.Setup(e => e.ShipWorksField).Returns(shipworksField.Object);

                testObject.AddEntry(entry.Object);

                OrderEntity order = new OrderEntity { BillFirstName = "bob" };

                testObject.CopyToEntity(order);

                Assert.Equal("joe", order.BillFirstName);
            }
        }

        [Fact]
        public void ApplyValues_DelegatesToLoadExternalField()
        {
            using (var mock = AutoMock.GetLoose())
            {
                OdbcFieldMap testObject = mock.Create<OdbcFieldMap>();

                var shipWorksOdbcMappableField = mock.Mock<IShipWorksOdbcMappableField>();
                var externalOdbcMappableField = mock.Mock<IExternalOdbcMappableField>();
                externalOdbcMappableField.Setup(e => e.Value).Returns("blah");

                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ShipWorksField).Returns(shipWorksOdbcMappableField.Object);
                mapEntry.Setup(e => e.ExternalField).Returns(externalOdbcMappableField.Object);

                testObject.AddEntry(mapEntry.Object);

                OdbcRecord odbcRecord = new OdbcRecord(string.Empty);

                testObject.ApplyValues(odbcRecord);

                mapEntry.Verify(m => m.LoadExternalField(It.Is<OdbcRecord>(r => r == odbcRecord)), Times.Once);
            }
        }

        [Fact]
        public void ApplyValues_DelegatesToCopyValueToShipworksField()
        {
            using (var mock = AutoMock.GetLoose())
            {
                OdbcFieldMap testObject = mock.Create<OdbcFieldMap>();

                var shipWorksOdbcMappableField = mock.Mock<IShipWorksOdbcMappableField>();
                var externalOdbcMappableField = mock.Mock<IExternalOdbcMappableField>();
                externalOdbcMappableField.Setup(e => e.Value).Returns("blah");

                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ShipWorksField).Returns(shipWorksOdbcMappableField.Object);
                mapEntry.Setup(e => e.ExternalField).Returns(externalOdbcMappableField.Object);

                testObject.AddEntry(mapEntry.Object);

                OdbcRecord odbcRecord = new OdbcRecord(string.Empty);

                testObject.ApplyValues(odbcRecord);

                mapEntry.Verify(m => m.CopyExternalValueToShipWorksField(), Times.Once);
            }
        }

        [Fact]
        public void ApplyValues_DelegatesToLoadShipWorksField()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IOdbcFieldValueResolver> resolver = mock.Mock<IOdbcFieldValueResolver>();

                Mock<IIndex<OdbcFieldValueResolutionStrategy, IOdbcFieldValueResolver>> repo = mock.MockRepository.Create<IIndex<OdbcFieldValueResolutionStrategy, IOdbcFieldValueResolver>>();
                repo.Setup(x => x[It.IsAny<OdbcFieldValueResolutionStrategy>()]).Returns(resolver.Object);
                mock.Provide(repo.Object);

                OdbcFieldMap testObject = mock.Create<OdbcFieldMap>();

                var shipWorksOdbcMappableField = mock.Mock<IShipWorksOdbcMappableField>();
                var externalOdbcMappableField = mock.Mock<IExternalOdbcMappableField>();

                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ShipWorksField).Returns(shipWorksOdbcMappableField.Object);
                mapEntry.Setup(e => e.ExternalField).Returns(externalOdbcMappableField.Object);
                testObject.AddEntry(mapEntry.Object);

                IEnumerable<IEntity2> entities = new List<IEntity2>() {new ShipmentEntity()};

                testObject.ApplyValues(entities, repo.Object);

                mapEntry.Verify(m => m.LoadShipWorksField(It.Is<IEntity2>(r => r.Equals(entities.FirstOrDefault())), It.IsAny<IOdbcFieldValueResolver>()), Times.Once);
            }
        }

        [Fact]
        public void ApplyValues_DoesNotDelegatesToLoadShipWorksField_WhenEntitiesIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                OdbcFieldMap testObject = mock.Create<OdbcFieldMap>();

                Mock<IOdbcFieldValueResolver> resolver = mock.Mock<IOdbcFieldValueResolver>();

                Mock<IIndex<OdbcFieldValueResolutionStrategy, IOdbcFieldValueResolver>> repo = mock.MockRepository.Create<IIndex<OdbcFieldValueResolutionStrategy, IOdbcFieldValueResolver>>();
                repo.Setup(x => x[It.IsAny<OdbcFieldValueResolutionStrategy>()]).Returns(resolver.Object);
                mock.Provide(repo.Object);

                var shipWorksOdbcMappableField = mock.Mock<IShipWorksOdbcMappableField>();
                var externalOdbcMappableField = mock.Mock<IExternalOdbcMappableField>();
                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ShipWorksField).Returns(shipWorksOdbcMappableField.Object);
                mapEntry.Setup(e => e.ExternalField).Returns(externalOdbcMappableField.Object);
                testObject.AddEntry(mapEntry.Object);

                IEnumerable<IEntity2> entities = null;

                testObject.ApplyValues(entities, repo.Object);

                mapEntry.Verify(m => m.LoadShipWorksField(null, It.IsAny<IOdbcFieldValueResolver>()), Times.Never);
            }
        }

        [Fact]
        public void FindEntriesBy_ReturnsCorrectEntry_WhenMapContainsGivenField()
        {
            OdbcFieldMap testObject = mock.Create<OdbcFieldMap>();
            IOdbcFieldMapEntry expectedEntry = GetFieldMapEntry(GetShipWorksField(OrderFields.BillFirstName, "Bill First Name"), GetExternalField("SomeColumnName2"));
            testObject.AddEntry(expectedEntry);

            IOdbcFieldMapEntry returnedEntry = testObject.FindEntriesBy(OrderFields.BillFirstName).FirstOrDefault();

            Assert.Equal(expectedEntry, returnedEntry);
        }

        [Fact]
        public void FindEntriesBy_ReturnsEmptyCollection_WhenMapDoesNotContainGivenField()
        {
            OdbcFieldMap testObject = mock.Create<OdbcFieldMap>();
            IOdbcFieldMapEntry mapEntry = GetFieldMapEntry(GetShipWorksField(OrderFields.BillFirstName, "Bill First Name"), GetExternalField("SomeColumnName2"));
            testObject.AddEntry(mapEntry);

            var returnedEntries = testObject.FindEntriesBy(OrderItemAttributeFields.IsManual);

            Assert.Empty(returnedEntries);
        }

        [Fact]
        public void FindEntriesBy_ReturnsCorrectEntry_WhenMapContainsGivenField_AndIncludeWhenShipWorksFieldIsNullIsTrue()
        {
            OdbcFieldMap testObject = mock.Create<OdbcFieldMap>();
            IOdbcFieldMapEntry expectedEntry = GetFieldMapEntry(GetShipWorksField(OrderFields.BillFirstName, "Bill First Name"), GetExternalField("SomeColumnName2"));
            testObject.AddEntry(expectedEntry);

            IOdbcFieldMapEntry returnedEntry = testObject.FindEntriesBy(OrderFields.BillFirstName, true).FirstOrDefault();

            Assert.Equal(expectedEntry, returnedEntry);
        }

        [Fact]
        public void FindEntriesBy_ReturnsEmptyCollection_WhenMapDoesNotContainGivenField_AndIncludeWhenShipWorksFieldIsNullIsTrue()
        {
            OdbcFieldMap testObject = mock.Create<OdbcFieldMap>();
            IOdbcFieldMapEntry mapEntry = GetFieldMapEntry(GetShipWorksField(OrderFields.BillFirstName, "Bill First Name"), GetExternalField("SomeColumnName2"));
            testObject.AddEntry(mapEntry);

            var returnedEntries = testObject.FindEntriesBy(OrderItemAttributeFields.IsManual, true);

            Assert.Empty(returnedEntries);
        }

        [Fact]
        public void FindEntriesBy_ReturnsCorrectEntry_WhenMapContainsGivenField_AndIncludeWhenShipWorksFieldIsNullIsFalse_AndShipWorksFieldIsNotNull()
        {
            OdbcFieldMap testObject = mock.Create<OdbcFieldMap>();

            var mockedShipWorksField = GetMockedShipWorksField(mock, OrderFields.BillFirstName, "bob");

            Mock<IOdbcFieldMapEntry> entry = mock.Mock<IOdbcFieldMapEntry>();
            entry.SetupGet(e => e.ShipWorksField).Returns(mockedShipWorksField.Object);
            testObject.AddEntry(entry.Object);

            IOdbcFieldMapEntry returnedEntry = testObject.FindEntriesBy(OrderFields.BillFirstName, false).FirstOrDefault();

            Assert.Equal(entry.Object, returnedEntry);
        }

        [Fact]
        public void FindEntriesBy_ReturnsEmptyCollection_WhenMapContainsGivenField_AndIncludeWhenShipWorksFieldIsNullIsFalse_AndShipWorksFieldIsNull()
        {
            OdbcFieldMap testObject = mock.Create<OdbcFieldMap>();

            var mockedShipWorksField = GetMockedShipWorksField(mock, OrderFields.BillFirstName, null);

            Mock<IOdbcFieldMapEntry> entry = mock.Mock<IOdbcFieldMapEntry>();
            entry.SetupGet(e => e.ShipWorksField).Returns(mockedShipWorksField.Object);
            testObject.AddEntry(entry.Object);

            var returnedEntries = testObject.FindEntriesBy(OrderFields.BillFirstName, false);

            Assert.Empty(returnedEntries);
        }

        [Fact]
        public void Clone_SavesToASteam()
        {
            var fieldMapWriter = mock.Mock<IOdbcFieldMapWriter>();
            var fieldMapReader = mock.Mock<IOdbcFieldMapReader>();

            var ioFactory = mock.Mock<IOdbcFieldMapIOFactory>();
            ioFactory.Setup(f => f.CreateWriter(It.IsAny<OdbcFieldMap>())).Returns(fieldMapWriter.Object);
            ioFactory.Setup(f => f.CreateReader(It.IsAny<Stream>())).Returns(fieldMapReader.Object);

            var testObject = mock.Create<OdbcFieldMap>();

            testObject.Clone();

            fieldMapWriter.Verify(f=>f.Write(It.IsAny<Stream>()), Times.Once);
        }

        [Fact]
        public void Clone_LoadsFromSteam()
        {
            var fieldMapWriter = mock.Mock<IOdbcFieldMapWriter>();
            var fieldMapReader = mock.Mock<IOdbcFieldMapReader>();

            var ioFactory = mock.Mock<IOdbcFieldMapIOFactory>();
            ioFactory.Setup(f => f.CreateWriter(It.IsAny<OdbcFieldMap>())).Returns(fieldMapWriter.Object);
            ioFactory.Setup(f => f.CreateReader(It.IsAny<Stream>())).Returns(fieldMapReader.Object);

            var testObject = mock.Create<OdbcFieldMap>();

            testObject.Clone();

            fieldMapReader.Verify(f => f.ReadEntry(), Times.Once);
        }

        private Stream GetStreamWithFieldMap()
        {
            MemoryStream stream = new MemoryStream();

            OdbcFieldMap map = new OdbcFieldMap(GetIoFactory());

            map.AddEntry(GetFieldMapEntry(GetShipWorksField(OrderFields.OrderNumber, "Order Number"),
                GetExternalField("SomeColumnName")));

            map.Save(stream);

            return stream;
        }

        private OdbcFieldMapEntry GetFieldMapEntry(ShipWorksOdbcMappableField shipworksField, ExternalOdbcMappableField externalField)
        {
            return new OdbcFieldMapEntry(shipworksField, externalField);
        }

        private ShipWorksOdbcMappableField GetShipWorksField(EntityField2 field, string displayName)
        {
            return new ShipWorksOdbcMappableField(field, displayName, OdbcFieldValueResolutionStrategy.Default);
        }

        private Mock<IShipWorksOdbcMappableField> GetMockedShipWorksField(AutoMock mockToUse, EntityField2 field, object shipWorksValue)
        {
            var odbcMappableField = mockToUse.Mock<IShipWorksOdbcMappableField>();
            odbcMappableField.SetupGet(f => f.Name).Returns(field.Name);
            odbcMappableField.SetupGet(f => f.ContainingObjectName).Returns(field.ContainingObjectName);
            odbcMappableField.SetupGet(f => f.Value).Returns(shipWorksValue);
            return odbcMappableField;
        }

        private ExternalOdbcMappableField GetExternalField(string columnName)
        {
            return new ExternalOdbcMappableField(new OdbcColumn(columnName));
        }

        private IOdbcFieldMapIOFactory GetIoFactory()
        {
            var log = mock.Mock<ILog>();

            var ioFactory = mock.Mock<IOdbcFieldMapIOFactory>();

            ioFactory.Setup(f => f.CreateWriter(It.IsAny<OdbcFieldMap>())).Returns((OdbcFieldMap m) => new JsonOdbcFieldMapWriter(m));
            ioFactory.Setup(f => f.CreateReader(It.IsAny<Stream>())).Returns<Stream>(s => new JsonOdbcFieldMapReader(s.ConvertToString(), log.Object));
            ioFactory.Setup(f => f.CreateReader(It.IsAny<string>())).Returns<string>(s => new JsonOdbcFieldMapReader(s, log.Object));

            return ioFactory.Object;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}