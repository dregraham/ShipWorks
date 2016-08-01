using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.UI.Platforms.Odbc;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcImportFieldMappingControlViewModelTest
    {
        [Fact]
        public void Load_OrderFieldMapSetByCreateOrderFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());


                Assert.NotEmpty(testObject.Order.Entries);
            }
        }

        [Fact]
        public void Load_AddressFieldMapSetByCreateAddressFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                Assert.NotEmpty(testObject.Address.Entries);
            }
        }

        [Fact]
        public void Load_ItemFieldMapSetByCreateItemFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                testObject.NumberOfItemsPerOrder = 1;
                testObject.IsSingleLineOrder = true;

                Assert.NotEmpty(testObject.Items[0].Entries);
            }
        }

        [Fact]
        public void Load_SelectedFieldMapIsOrderFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                Assert.Equal(testObject.Order, testObject.SelectedFieldMap);
            }
        }

        [Fact]
        public void Load_OrderMapDisplayName_IsOrder()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                Assert.Equal("Order", testObject.Order.DisplayName);
            }
        }

        [Fact]
        public void Load_AddressMapDisplayName_IsOrder()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                Assert.Equal("Address", testObject.Address.DisplayName);
            }
        }

        [Fact]
        public void SetSingleLineItemTrue_MakesDisplayMapsContainOnlyOrderAddressAndSingleItem_WhenNumberOfItemsIsZero()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                testObject.NumberOfItemsPerOrder = 0;
                testObject.IsSingleLineOrder = true;
                var expectedMapNames = new List<string>() {"Order", "Address", "Item 1"};
                var actualMapNames = new List<string>()
                {
                    testObject.Order.DisplayName,
                    testObject.Address.DisplayName,
                    testObject.Items[0].DisplayName
                };

                Assert.Equal(expectedMapNames, actualMapNames);
            }
        }

        [Fact]
        public void SetSingleLineItemTrue_SetsNumberOfItemsBackToOne()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                testObject.NumberOfItemsPerOrder = 5;

                testObject.IsSingleLineOrder = true;

                Assert.Equal(1, testObject.Items.Count);
            }
        }

        [Fact]
        public void SetSingleLineItemTrue_ChangesItemMapDisplayNameFromItemToItem1_WhenDisplayMapsContainsItemMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                testObject.NumberOfItemsPerOrder = 1;
                testObject.IsSingleLineOrder = false;

                // So we can see that it was previously Item
                Assert.Equal("Item", testObject.Items[0].DisplayName);

                testObject.IsSingleLineOrder = true;

                // Now it has changed to Item 1
                Assert.Equal("Item 1", testObject.Items[0].DisplayName);
            }
        }

        [Fact]
        public void SetSingleLineItemFalse_AddsItemMapToDisplayMaps_WhenNumberOfItemsPerOrderIsZero()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                testObject.NumberOfItemsPerOrder = 0;
                testObject.IsSingleLineOrder = false;

                Assert.Equal("Item", testObject.Items[0].DisplayName);
            }
        }

        [Fact]
        public void SetSingleLineItemFalse_ChangesFirstItemMapDisplayNameFromItem1ToItem_WhenDisplayMapsContainsItemMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                testObject.NumberOfItemsPerOrder = 1;
                testObject.IsSingleLineOrder = true;

                // So we can see that it was previously Item 1
                Assert.Equal("Item 1", testObject.Items[0].DisplayName);

                testObject.IsSingleLineOrder = false;

                // Now it has changed to Item
                Assert.Equal("Item", testObject.Items[0].DisplayName);
            }
        }

        [Fact]
        public void SetSingleLineItemFalse_DropsAllButFirstItemMap_WhenDisplayMapsContainsMultipleItemMaps()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                testObject.NumberOfItemsPerOrder = 20;
                testObject.IsSingleLineOrder = false;

                Assert.Equal(1, testObject.Items.Count);
            }
        }

        [Fact]
        public void SetNumberOfItemsPerOrder_DoesNotAddItemMaps_WhenSingleLineOrderIsFalse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                testObject.NumberOfItemsPerOrder = 2;

                testObject.IsSingleLineOrder = false;

                Assert.Equal(1, testObject.Items.Count);
            }
        }

        [Fact]
        public void SetNumberOfItemsPerOrder_AddsTheCorrectNumberOfItemMaps_WhenNumberOfItemsIncreases()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                testObject.IsSingleLineOrder = true;

                testObject.NumberOfItemsPerOrder = 2;
                var startingNumberOfItems = testObject.Items.Count;

                testObject.NumberOfItemsPerOrder = 7;
                var endNumberOfItems = testObject.Items.Count;

                Assert.Equal(5, endNumberOfItems - startingNumberOfItems);
            }
        }

        [Fact]
        public void SetNumberOfItemsPerOrder_AddsItemMapWithCorrectNumberOfAttributes()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                testObject.IsSingleLineOrder = true;
                testObject.NumberOfAttributesPerItem = 10;
                testObject.NumberOfItemsPerOrder = 1;

                IEnumerable<OdbcFieldMapDisplay> itemMaps = testObject.Items;
                var numberOfAttributeEntriesInItemMap = itemMaps.FirstOrDefault()?.Entries.Count(e => e.ShipWorksField.DisplayName.Contains("Attribute"));

                Assert.Equal(10, numberOfAttributeEntriesInItemMap);
            }
        }

        [Fact]
        public void SetNumberOfItemsPerOrder_RemovesTheCorrectNumberOfItemMaps_WhenNumberOfItemsDecreases()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                testObject.IsSingleLineOrder = true;
                testObject.NumberOfItemsPerOrder = 7;

                var startingNumberOfItems = testObject.Items.Count;

                testObject.NumberOfItemsPerOrder = 2;

                var endNumberOfItems = testObject.Items.Count;

                Assert.Equal(5, startingNumberOfItems - endNumberOfItems);
            }
        }

        [Fact]
        public void SetNumberOfAttributesPerItem_AddsCorrectNumberOfAttributesToExistingItemMaps_WhenNumberOfAttributesIncreases()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                testObject.IsSingleLineOrder = true;
                testObject.NumberOfAttributesPerItem = 1;
                testObject.NumberOfItemsPerOrder = 1;

                IEnumerable<OdbcFieldMapDisplay> itemMaps = testObject.Items;
                var startingNumberOfAttributeEntriesInItemMap = itemMaps.FirstOrDefault()?.Entries.Count(e => e.ShipWorksField.DisplayName.Contains("Attribute"));

                testObject.NumberOfAttributesPerItem = 11;

                itemMaps = testObject.Items;
                var endNumberOfAttributeEntriesInItemMap = itemMaps.FirstOrDefault()?.Entries.Count(e => e.ShipWorksField.DisplayName.Contains("Attribute"));

                Assert.Equal(10, endNumberOfAttributeEntriesInItemMap - startingNumberOfAttributeEntriesInItemMap);
            }
        }

        [Fact]
        public void SetNumberOfAttributesPerItem_RemovesCorrectNumberOfAttributesToExistingItemMaps_WhenNumberOfAttributesDecreases()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

                testObject.IsSingleLineOrder = true;
                testObject.NumberOfAttributesPerItem = 11;
                testObject.NumberOfItemsPerOrder = 1;

                IEnumerable<OdbcFieldMapDisplay> itemMaps = testObject.Items;
                var startingNumberOfAttributeEntriesInItemMap = itemMaps.FirstOrDefault()?.Entries.Count(e => e.ShipWorksField.DisplayName.Contains("Attribute"));

                testObject.NumberOfAttributesPerItem = 1;

                itemMaps = testObject.Items;
                var endNumberOfAttributeEntriesInItemMap = itemMaps.FirstOrDefault()?.Entries.Count(e => e.ShipWorksField.DisplayName.Contains("Attribute"));

                Assert.Equal(10, startingNumberOfAttributeEntriesInItemMap - endNumberOfAttributeEntriesInItemMap);
            }
        }

        private static Expression<Func<IMessageHelper, DialogResult>> GetShowMessageExpression()
        {
            return messageHelper =>
                messageHelper.ShowQuestion(It.IsAny<MessageBoxIcon>(), It.IsAny<MessageBoxButtons>(), It.IsAny<string>());
        }
    }
}