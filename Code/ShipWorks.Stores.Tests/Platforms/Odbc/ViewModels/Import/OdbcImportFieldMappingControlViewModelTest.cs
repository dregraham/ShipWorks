using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.UI.Platforms.Odbc;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.ViewModels.Import
{
    public class OdbcImportFieldMappingControlViewModelTest
    {
        [Fact]
        public void Load_ThrowsArgumentNullException_WhenDataSourceIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

                Assert.Throws<ArgumentNullException>(() => testObject.Load(null));
            }
        }

        [Fact]
        public void Load_OrderFieldMapSetByCreateOrderFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();

                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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
                var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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

        [Fact]
        public void Load_SetsNumberOfItemsToZero_WhenLoadedMapDoesNotContainItemEntries()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string mapPath = "ShipWorks.Stores.Tests.Platforms.Odbc.Artifacts.MapWithNoItems.json";

                List<string> columnNames = new List<string>()
                {
                    "OrderId",
                    "OrderDate",
                    "OnlineLastModified"
                };

                var testObject = CreateViewModelWithLoadedEntries(mock, columnNames, mapPath);

                Assert.Equal(0, testObject.NumberOfItemsPerOrder);
            }
        }

        [Fact]
        public void Load_SetsNumberOfItemAttributesToZero_WhenLoadedMapDoesNotContainItemAttributeEntries()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string mapPath = "ShipWorks.Stores.Tests.Platforms.Odbc.Artifacts.MapWithItemsAndNoAttributes.json";

                List<string> columnNames = new List<string>()
                {
                    "OrderId",
                    "OnlineLastModified",
                    "ItemName"
                };

                var testObject = CreateViewModelWithLoadedEntries(mock, columnNames, mapPath);

                Assert.Equal(0, testObject.NumberOfAttributesPerItem);
            }
        }

        [Fact]
        public void Load_SetsNumberOfItems_WhenLoadedMapContainsItemEntries()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string mapPath = "ShipWorks.Stores.Tests.Platforms.Odbc.Artifacts.MapWithItemsAndNoAttributes.json";

                List<string> columnNames = new List<string>()
                {
                    "OrderId",
                    "OrderDate",
                    "ItemName"
                };

                var testObject = CreateViewModelWithLoadedEntries(mock, columnNames, mapPath);

                Assert.Equal(1, testObject.NumberOfItemsPerOrder);
            }
        }

        [Fact]
        public void Load_SetsNumberOfItemAttributes_WhenLoadedMapContainsItemAttributeEntries()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string mapPath = "ShipWorks.Stores.Tests.Platforms.Odbc.Artifacts.MapWithItemsAndWithAttributes.json";

                List<string> columnNames = new List<string>()
                {
                    "OrderId",
                    "OnlineLastModified",
                    "ItemName",
                    "ItemAttributeName"
                };

                var testObject = CreateViewModelWithLoadedEntries(mock, columnNames, mapPath);

                Assert.Equal(1, testObject.NumberOfAttributesPerItem);
            }
        }

        [Fact]
        public void Load_SetsCorrectNumberOfItemAttributes_WhenLoadedMapContainsItemAttributeEntriesThatAreNotInOrder()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string mapPath = "ShipWorks.Stores.Tests.Platforms.Odbc.Artifacts.MapWithItemsAndWithAttributesOutOfOrder.json";

                List<string> columnNames = new List<string>()
                {
                    "OrderId",
                    "OnlineLastModified",
                    "ItemName1",
                    "ItemAttributeName1",
                    "ItemAttributeName3",
                    "ItemName2",
                    "ItemAttributeName2"
                };

                var testObject = CreateViewModelWithLoadedEntries(mock, columnNames, mapPath);

                Assert.Equal(3, testObject.NumberOfAttributesPerItem);
            }
        }

        [Fact]
        public void ValidateRequiredMappingFields_DelegatesToMessageHelper_WhenOneRequiredFieldIsNotMapped()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string mapPath = "ShipWorks.Stores.Tests.Platforms.Odbc.Artifacts.MapWithNoItems.json";

                List<string> columnNames = new List<string>()
                {
                    "OrderDate",
                    "OnlineLastModified"
                };

                var messageHelper = mock.Mock<IMessageHelper>();

                var testObject = CreateViewModelWithLoadedEntries(mock, columnNames, mapPath);
                testObject.ValidateRequiredMappingFields();

                messageHelper.Verify(m => m.ShowError(It.IsAny<string>()), Times.Once);
            }
        }

        [Fact]
        public void ValidateRequiredMappingFields_DelegatesToMessageHelper_WhenMultipleRequiredFieldsAreNotMapped()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string mapPath = "ShipWorks.Stores.Tests.Platforms.Odbc.Artifacts.MapWithNoItems.json";

                List<string> columnNames = new List<string>()
                {
                    "OrderDate"
                };

                var messageHelper = mock.Mock<IMessageHelper>();

                var testObject = CreateViewModelWithLoadedEntries(mock, columnNames, mapPath);
                testObject.ValidateRequiredMappingFields();

                messageHelper.Verify(m => m.ShowError(It.IsAny<string>()), Times.Once);
            }
        }

        [Fact]
        public void ValidateRequiredMappingFields_DelegatesToMessageHelper_WhenMultiLineOrdersIsSelectedAndRecordIdentifierIsNotSet()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string mapPath = "ShipWorks.Stores.Tests.Platforms.Odbc.Artifacts.MapWithItemsAndNoAttributes.json";

                List<string> columnNames = new List<string>()
                {
                    "OrderNumber",
                    "OrderDate",
                    "OnlineLastModified",
                    "ItemName"
                };
                var messageHelper = mock.Mock<IMessageHelper>();

                var testObject = CreateViewModelWithLoadedEntries(mock, columnNames, mapPath);

                testObject.ValidateRequiredMappingFields();

                messageHelper.Verify(m => m.ShowError(It.IsAny<string>()), Times.Once);
            }
        }

        [Fact]
        public void ValidateRequiredMappingFields_ReturnsTrue_WhenAllRequiredFieldsAreMapped()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string mapPath = "ShipWorks.Stores.Tests.Platforms.Odbc.Artifacts.MapWithNoItems.json";

                List<string> columnNames = new List<string>()
                {
                    "OrderId",
                    "OrderDate",
                    "OnlineLastModified"
                };

                var testObject = CreateViewModelWithLoadedEntries(mock, columnNames, mapPath);

                Assert.True(testObject.ValidateRequiredMappingFields());
            }
        }

        private static OdbcImportMappingControlViewModel CreateViewModelWithLoadedEntries(AutoMock mock, List<string> columnNames, string mapPath)
        {
            JsonOdbcFieldMapIOFactory ioFactory = mock.Create<JsonOdbcFieldMapIOFactory>();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("foo", typeof(string));
            dataTable.Columns.Add("bar", typeof(string));
            dataTable.Columns.Add("baz", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));

            foreach (string columnName in columnNames)
            {
                dataTable.Rows.Add(string.Empty, string.Empty, string.Empty, columnName);
            }

            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.GetSchema(It.IsAny<string>(), It.IsAny<string[]>())).Returns(dataTable);

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.SetupGet(d => d.Name).Returns("SomeName");
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcColumnSource table = mock.Create<OdbcColumnSource>(new TypedParameter(typeof(string), "Table"));

            table.Load(dataSource.Object, "Table", OdbcColumnSourceType.Table);

            dataTable.Dispose();

            Mock<Func<string, IOdbcColumnSource>> repo = mock.MockRepository.Create<Func<string, IOdbcColumnSource>>();
            repo.Setup(r => r(It.IsAny<string>())).Returns(table);

            var store = new OdbcStoreEntity
            {
                ImportMap = EmbeddedResourceHelper.GetEmbeddedResourceString(mapPath),
                ImportColumnSourceType = (int)OdbcColumnSourceType.Table,
                ImportColumnSource = "Table",
                ImportStrategy = (int) OdbcImportStrategy.ByModifiedTime
            };

            Mock<IOdbcDataSourceService> dataSourceService = mock.Mock<IOdbcDataSourceService>();
            dataSourceService.Setup(s => s.GetImportDataSource(store)).Returns(dataSource.Object);

            var mapFactory = mock.Create<OdbcFieldMapFactory>(new TypedParameter(typeof(IOdbcFieldMapIOFactory), ioFactory));

            var testObject = mock.Create<OdbcImportMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory),
                mapFactory), new TypedParameter(typeof(Func<string, IOdbcColumnSource>), repo.Object));

            testObject.Load(store);

            return testObject;
        }
    }
}