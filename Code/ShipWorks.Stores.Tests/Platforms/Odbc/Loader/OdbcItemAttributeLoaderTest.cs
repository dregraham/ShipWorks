using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Loader
{
    public class OdbcItemAttributeLoaderTest
    {
        [Fact]
        public void Load_AddsItemAttributeToItem_WhenMapContainsItemAttributeField()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var map = mock.Mock<IOdbcFieldMap>();

                var shipworksField = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksField.SetupGet(e => e.QualifiedName).Returns("OrderItemAttribute.Name");
                shipworksField.Setup(e => e.Value).Returns("Large");

                OdbcColumn column = new OdbcColumn("Size");

                var externalField = mock.Mock<IExternalOdbcMappableField>();
                externalField.Setup(f => f.Column).Returns(column);

                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ShipWorksField).Returns(shipworksField.Object);
                mapEntry.Setup(e => e.ExternalField).Returns(externalField.Object);
                mapEntry.Setup(e => e.Index).Returns(5);

                map.Object.AddEntry(mapEntry.Object);
                map.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>(), false)).
                    Returns(new List<IOdbcFieldMapEntry> {mapEntry.Object});

                OdbcItemAttributeLoader testObject = new OdbcItemAttributeLoader();
                OrderItemEntity item = new OrderItemEntity();

                testObject.Load(map.Object, item, 5);

                Assert.Equal("Size", item.OrderItemAttributes.First().Name);
                Assert.Equal("Large", item.OrderItemAttributes.First().Description);
            }
        }

        [Fact]
        public void Load_DoesNotAddItemAttributeToItem_WhenMapContainsItemAttributeFieldAndIndexDoesNotMatch()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var map = mock.Mock<IOdbcFieldMap>();

                var shipworksField = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksField.SetupGet(e => e.QualifiedName).Returns("OrderItemAttribute.Name");
                shipworksField.Setup(e => e.Value).Returns("Large");

                OdbcColumn column = new OdbcColumn("Size");

                var externalField = mock.Mock<IExternalOdbcMappableField>();
                externalField.Setup(f => f.Column).Returns(column);

                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ShipWorksField).Returns(shipworksField.Object);
                mapEntry.Setup(e => e.ExternalField).Returns(externalField.Object);
                mapEntry.Setup(e => e.Index).Returns(0);

                map.Object.AddEntry(mapEntry.Object);
                map.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>(), false)).
                    Returns(new List<IOdbcFieldMapEntry> { mapEntry.Object });

                OdbcItemAttributeLoader testObject = new OdbcItemAttributeLoader();
                OrderItemEntity item = new OrderItemEntity();

                testObject.Load(map.Object, item, 5);

                Assert.Empty(item.OrderItemAttributes);
            }
        }

        [Fact]
        public void Load_DoesNotAddAttributesToItem_WhenMapDoesNotContainsItemAttributeFields()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var map = mock.Mock<IOdbcFieldMap>();

                var shipworksField = mock.Mock<IShipWorksOdbcMappableField>();
                shipworksField.SetupGet(e => e.QualifiedName).Returns("OrderItemAttribute.Name");

                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ShipWorksField).Returns(shipworksField.Object);
                mapEntry.Setup(e => e.Index).Returns(0);

                map.Object.AddEntry(mapEntry.Object);

                OdbcItemAttributeLoader testObject = new OdbcItemAttributeLoader();
                OrderItemEntity item = new OrderItemEntity();

                testObject.Load(map.Object, item, 0);

                Assert.Empty(item.OrderItemAttributes);
            }
        }
    }
}