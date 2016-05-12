using System.Collections.Generic;
using Autofac.Extras.Moq;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
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
        public void CreateOrderMap_ReturnsMapWithOrderAsDisplayName()
        {
            Assert.Equal("Order", testObject.CreateOrderFieldMap().DisplayName);
        }

        [Fact]
        public void CreateOrderMap_ReturnsMapWithCorrectNumberOfOrderMappingFields()
        {
            Assert.Equal(22, testObject.CreateOrderFieldMap().Entries.Count);
        }

        [Fact]
        public void CreateItemMap_ReturnsMapWithOrderAsDisplayName()
        {
            Assert.Equal("Item", testObject.CreateOrderItemFieldMap().DisplayName);
        }

        [Fact]
        public void CreateItemMap_ReturnsMapWithCorrectNumberOfOrderMappingFields()
        {
            Assert.Equal(18, testObject.CreateOrderItemFieldMap().Entries.Count);
        }

        [Fact]
        public void CreateAddressMap_ReturnsMapWithOrderAsDisplayName()
        {
            Assert.Equal("Address", testObject.CreateAddressFieldMap().DisplayName);
        }

        [Fact]
        public void CreateAddressMap_ReturnsMapWithCorrectNumberOfOrderMappingFields()
        {
            Assert.Equal(32, testObject.CreateAddressFieldMap().Entries.Count);
        }

        [Fact]
        public void CreateFieldMapFrom_ReturnsMapContainingAllEntriesOfProvidedMap()
        {
            var orderMap = testObject.CreateOrderFieldMap();
            var itemMap = testObject.CreateOrderItemFieldMap();
            var addressMap = testObject.CreateAddressFieldMap();

            var combinedMap = testObject.CreateFieldMapFrom(new List<OdbcFieldMap>() {orderMap, itemMap, addressMap});

            foreach (var entry in orderMap.Entries)
            {
                Assert.Contains(entry, combinedMap.Entries);
            }

            foreach (var entry in itemMap.Entries)
            {
                Assert.Contains(entry, combinedMap.Entries);
            }

            foreach (var entry in addressMap.Entries)
            {
                Assert.Contains(entry, combinedMap.Entries);
            }
        }
    }
}