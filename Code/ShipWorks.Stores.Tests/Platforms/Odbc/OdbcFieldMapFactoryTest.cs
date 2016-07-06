using Autofac.Extras.Moq;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Linq;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcFieldMapFactoryTest
    {
        readonly OdbcFieldMapFactory testObject;

        public OdbcFieldMapFactoryTest()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                testObject = mock.Create<OdbcFieldMapFactory>();
            }
        }

        [Fact]
        public void CreateOrderMap_ReturnsMapWithCorrectNumberOfOrderMappingFields()
        {
            Assert.Equal(21, testObject.CreateOrderFieldMap().Entries.Count());
        }

        [Fact]
        public void CreateItemMap_ReturnsMapWithCorrectNumberOfOrderMappingFields()
        {
            Assert.Equal(17, testObject.CreateOrderItemFieldMap(0).Entries.Count());
        }

        [Fact]
        public void CreateItemMap_ReturnsMapWithCorrectIndexSet()
        {
            var itemMap = testObject.CreateOrderItemFieldMap(5);

            Assert.True(itemMap.Entries.All(e => e.Index == 5));
        }

        [Fact]
        public void CreateAddressMap_ReturnsMapWithCorrectNumberOfOrderMappingFields()
        {
            Assert.Equal(32, testObject.CreateAddressFieldMap().Entries.Count());
        }

        [Fact]
        public void CreateFieldMapFrom_ReturnsMapContainingAllEntriesThatAreMappedOfProvidedMap()
        {
            OdbcFieldMap orderMap = testObject.CreateOrderFieldMap();
            ShipWorksOdbcMappableField shipworksOrderMap = new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number");
            ExternalOdbcMappableField externalOrderMap = new ExternalOdbcMappableField(new OdbcColumnSource("Order"), new OdbcColumn("Column"));
            orderMap.AddEntry(new OdbcFieldMapEntry(shipworksOrderMap, externalOrderMap));

            var itemMap = testObject.CreateOrderItemFieldMap(0);
            ShipWorksOdbcMappableField shipworksItemMap = new ShipWorksOdbcMappableField(OrderItemFields.ISBN, "ISBN");
            ExternalOdbcMappableField externalItemMap = new ExternalOdbcMappableField(new OdbcColumnSource("OrderItem"), new OdbcColumn("OrderItemColumn"));
            itemMap.AddEntry(new OdbcFieldMapEntry(shipworksItemMap, externalItemMap));

            var addressMap = testObject.CreateAddressFieldMap();
            ShipWorksOdbcMappableField shipworksAddressMap = new ShipWorksOdbcMappableField(OrderFields.BillAddressValidationError, "BillAddressValidationError");
            ExternalOdbcMappableField externalAddressMap = new ExternalOdbcMappableField(new OdbcColumnSource("BillAddressValidationError"), new OdbcColumn("BillAddressValidationError"));
            itemMap.AddEntry(new OdbcFieldMapEntry(shipworksAddressMap, externalAddressMap));

            var combinedMap = testObject.CreateFieldMapFrom(orderMap.Entries.Concat(addressMap.Entries).Concat(itemMap.Entries));

            foreach (var entry in orderMap.Entries.Where(e => e.ExternalField.Column != null))
            {
                Assert.Contains(entry, combinedMap.Entries);
            }

            foreach (var entry in itemMap.Entries.Where(e => e.ExternalField.Column != null))
            {
                Assert.Contains(entry, combinedMap.Entries);
            }

            foreach (var entry in addressMap.Entries.Where(e => e.ExternalField.Column != null))
            {
                Assert.Contains(entry, combinedMap.Entries);
            }
        }
    }
}