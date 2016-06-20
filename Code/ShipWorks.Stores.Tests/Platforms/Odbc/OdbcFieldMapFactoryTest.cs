﻿using Autofac.Extras.Moq;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Collections.Generic;
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
            Assert.Equal(18, testObject.CreateOrderItemFieldMap().Entries.Count());
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
            ExternalOdbcMappableField externalOrderMap = new ExternalOdbcMappableField(new OdbcTable("Order"), new OdbcColumn("Column"));
            orderMap.AddEntry(new OdbcFieldMapEntry(shipworksOrderMap, externalOrderMap));

            var itemMap = testObject.CreateOrderItemFieldMap();
            ShipWorksOdbcMappableField shipworksItemMap = new ShipWorksOdbcMappableField(OrderItemFields.ISBN, "ISBN");
            ExternalOdbcMappableField externalItemMap = new ExternalOdbcMappableField(new OdbcTable("OrderItem"), new OdbcColumn("OrderItemColumn"));
            itemMap.AddEntry(new OdbcFieldMapEntry(shipworksItemMap, externalItemMap));

            var addressMap = testObject.CreateAddressFieldMap();
            ShipWorksOdbcMappableField shipworksAddressMap = new ShipWorksOdbcMappableField(OrderFields.BillAddressValidationError, "BillAddressValidationError");
            ExternalOdbcMappableField externalAddressMap = new ExternalOdbcMappableField(new OdbcTable("BillAddressValidationError"), new OdbcColumn("BillAddressValidationError"));
            itemMap.AddEntry(new OdbcFieldMapEntry(shipworksAddressMap, externalAddressMap));

            var combinedMap = testObject.CreateFieldMapFrom(new List<OdbcFieldMap>() {orderMap, itemMap, addressMap});

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