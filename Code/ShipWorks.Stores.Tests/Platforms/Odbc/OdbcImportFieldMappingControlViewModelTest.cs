using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.UI.Platforms.Odbc;
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
        public void Constructor_OrderFieldMapSetByCreateOrderFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                OdbcFieldMap odbcFieldMap = mock.Create<OdbcFieldMap>();
                mock.Mock<IOdbcFieldMapFactory>()
                    .Setup(f => f.CreateOrderFieldMap())
                    .Returns(odbcFieldMap);

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();

                Assert.Equal(odbcFieldMap, testObject.DisplayFieldMaps[0].Map);
            }
        }

        [Fact]
        public void Constructor_AddressFieldMapSetByCreateAddressFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                OdbcFieldMap odbcFieldMap = mock.Create<OdbcFieldMap>();
                mock.Mock<IOdbcFieldMapFactory>()
                    .Setup(f => f.CreateAddressFieldMap())
                    .Returns(odbcFieldMap);

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();

                Assert.Equal(odbcFieldMap, testObject.DisplayFieldMaps[1].Map);
            }
        }

        [Fact]
        public void Constructor_ItemFieldMapSetByCreateItemFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                OdbcFieldMap odbcFieldMap = mock.Create<OdbcFieldMap>();
                mock.Mock<IOdbcFieldMapFactory>()
                    .Setup(f => f.CreateOrderItemFieldMap(0))
                    .Returns(odbcFieldMap);

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.NumberOfItemsPerOrder = 1;
                testObject.IsSingleLineOrder = true;

                Assert.Equal(odbcFieldMap, testObject.DisplayFieldMaps[2].Map);
            }
        }

        [Fact]
        public void Constructor_SelectedFieldMapIsOrderFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();

                Assert.Equal(testObject.DisplayFieldMaps[0], testObject.SelectedFieldMap);
            }
        }

        [Fact]
        public void Constructor_FieldMaps_ContainsTwoFieldMaps()
        {
            using (var mock = AutoMock.GetLoose())
            {

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();

                Assert.Equal(2, testObject.DisplayFieldMaps.Count);
            }
        }

        [Fact]
        public void Constructor_FirstFieldMapDisplayName_IsOrder()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();

                Assert.Equal("Order", testObject.DisplayFieldMaps[0].DisplayName);
            }
        }

        [Fact]
        public void Constructor_SecondFieldMapDisplayName_IsOrder()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();

                Assert.Equal("Address", testObject.DisplayFieldMaps[1].DisplayName);
            }
        }

        [Fact]
        public void Load_ThrowsArgumentNullException_WhenDataSourceIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                OdbcImportFieldMappingControlViewModel testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                Assert.Throws<ArgumentNullException>(() => testObject.Load(null));
            }
        }

        [Fact]
        public void Load_LoadsSchemaTables()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IOdbcDataSource> odbcDataSource = new Mock<IOdbcDataSource>();
                OdbcImportFieldMappingControlViewModel testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(odbcDataSource.Object);
                Assert.NotNull(testObject.Tables);
            }
        }

        [Fact]
        public void Load_DisplaysErrorMessage_WhenShipWorksOdbcExceptionIsThrown()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IOdbcSchema> shema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IMessageHelper> messageHelper = mock.Mock<IMessageHelper>();
                shema.Setup(s => s.Load(dataSource.Object)).Throws<ShipWorksOdbcException>();
                OdbcImportFieldMappingControlViewModel testObject =
                    mock.Create<OdbcImportFieldMappingControlViewModel>();

                testObject.Load(dataSource.Object);

                messageHelper.Verify(m => m.ShowError(It.IsAny<string>()), Times.Once);
            }
        }

        [Fact]
        public void Save_ThrowsArgumentNullException_WhenStoreIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                OdbcImportFieldMappingControlViewModel testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                Assert.Throws<ArgumentNullException>(() => testObject.Save(null));
            }
        }

        [Fact]
        public void SetSelectedTable_QuestionBoxNotDisplayed_WhenOrignalValueIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var messageHelper = mock.Mock<IMessageHelper>();
                messageHelper.Setup(GetShowMessageExpression());

                OdbcImportFieldMappingControlViewModel testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(mock.Mock<IOdbcDataSource>().Object);

                var table = mock.Mock<IOdbcTable>();

                testObject.SelectedTable = table.Object;

                messageHelper.Verify(GetShowMessageExpression(), Times.Never);
            }
        }

        [Fact]
        public void SetSelectedTable_QuestionBoxDisplayed_WhenOriginalValueIsNotNull_AndFieldsAreMapped()
        {
            using (var mock = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var messageHelper = mock.Mock<IMessageHelper>();
                messageHelper.Setup(GetShowMessageExpression());

                OdbcImportFieldMappingControlViewModel testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(mock.Mock<IOdbcDataSource>().Object);

                var table1 = mock.Mock<IOdbcTable>();
                var table2 = mock2.Mock<IOdbcTable>();

                Mock<IOdbcFieldMapIOFactory> mapIOFactory = mock.Mock<IOdbcFieldMapIOFactory>();
                OdbcFieldMap odbcFieldMap = new OdbcFieldMap(mapIOFactory.Object);
                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.SetupGet(e => e.ExternalField)
                    .Returns(new ExternalOdbcMappableField(table1.Object, new OdbcColumn("test")));
                mapEntry.SetupGet(f => f.ShipWorksField)
                    .Returns(new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number"));
                odbcFieldMap.AddEntry(mapEntry.Object);

                Mock<IOdbcFieldMapFactory> fieldMapFactory = mock.Mock<IOdbcFieldMapFactory>();
                fieldMapFactory.Setup(f => f.CreateFieldMapFrom(It.IsAny<IEnumerable<OdbcFieldMap>>()))
                    .Returns(odbcFieldMap);

                testObject.SelectedTable = table1.Object;
                testObject.TableChangedCommand.Execute(null);

                testObject.SelectedTable = table2.Object;
                testObject.TableChangedCommand.Execute(null);

                messageHelper.Verify(GetShowMessageExpression(), Times.Once);
            }
        }

        [Fact]
        public void SetSelectedTable_TableDoesNotChange_WhenUserDeclinesToContinue()
        {
            using (var mock = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var messageHelper = mock.Mock<IMessageHelper>();
                messageHelper.Setup(GetShowMessageExpression()).Returns(DialogResult.No);

                OdbcImportFieldMappingControlViewModel testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(mock.Mock<IOdbcDataSource>().Object);

                var table1 = mock.Mock<IOdbcTable>();
                var table2 = mock2.Mock<IOdbcTable>();

                Mock<IOdbcFieldMapIOFactory> mapIOFactory = mock.Mock<IOdbcFieldMapIOFactory>();
                OdbcFieldMap odbcFieldMap = new OdbcFieldMap(mapIOFactory.Object);
                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.SetupGet(e => e.ExternalField)
                    .Returns(new ExternalOdbcMappableField(table1.Object, new OdbcColumn("test")));
                mapEntry.SetupGet(f => f.ShipWorksField)
                    .Returns(new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number"));
                odbcFieldMap.AddEntry(mapEntry.Object);

                Mock<IOdbcFieldMapFactory> fieldMapFactory = mock.Mock<IOdbcFieldMapFactory>();
                fieldMapFactory.Setup(f => f.CreateFieldMapFrom(It.IsAny<IEnumerable<OdbcFieldMap>>()))
                    .Returns(odbcFieldMap);

                testObject.SelectedTable = table1.Object;
                testObject.TableChangedCommand.Execute(null);

                testObject.SelectedTable = table2.Object;
                testObject.TableChangedCommand.Execute(null);

                Assert.Equal(table1.Object, testObject.SelectedTable);
            }
        }

        [Fact]
        public void SetSelectedTable_ColumnsNotUpdated_WhenUserDeclinesToContinue()
        {
            using (var mock = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var messageHelper = mock.Mock<IMessageHelper>();
                messageHelper.Setup(GetShowMessageExpression()).Returns(DialogResult.No);

                OdbcImportFieldMappingControlViewModel testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(mock.Mock<IOdbcDataSource>().Object);

                var table1 = mock.Mock<IOdbcTable>();
                var table2 = mock2.Mock<IOdbcTable>();

                Mock<IOdbcFieldMapIOFactory> mapIOFactory = mock.Mock<IOdbcFieldMapIOFactory>();
                OdbcFieldMap odbcFieldMap = new OdbcFieldMap(mapIOFactory.Object);
                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.SetupGet(e => e.ExternalField)
                    .Returns(new ExternalOdbcMappableField(table1.Object, new OdbcColumn("test")));
                mapEntry.SetupGet(f => f.ShipWorksField)
                    .Returns(new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number"));
                odbcFieldMap.AddEntry(mapEntry.Object);

                Mock<IOdbcFieldMapFactory> fieldMapFactory = mock.Mock<IOdbcFieldMapFactory>();
                fieldMapFactory.Setup(f => f.CreateFieldMapFrom(It.IsAny<IEnumerable<OdbcFieldMap>>()))
                    .Returns(odbcFieldMap);

                testObject.SelectedTable = table1.Object;
                testObject.TableChangedCommand.Execute(null);

                var columnsBeforeChange = testObject.Columns;

                testObject.SelectedTable = table2.Object;
                testObject.TableChangedCommand.Execute(null);

                Assert.Equal(columnsBeforeChange, testObject.Columns);
            }
        }

        [Fact]
        public void SetSelectedTable_TableChanges_WhenUserContinues()
        {
            using (var mock = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var messageHelper = mock.Mock<IMessageHelper>();
                messageHelper.Setup(GetShowMessageExpression()).Returns(DialogResult.Yes);

                OdbcImportFieldMappingControlViewModel testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(mock.Mock<IOdbcDataSource>().Object);

                var table1 = mock.Mock<IOdbcTable>();
                var table2 = mock2.Mock<IOdbcTable>();

                Mock<IOdbcFieldMapIOFactory> mapIOFactory = mock.Mock<IOdbcFieldMapIOFactory>();
                OdbcFieldMap odbcFieldMap = new OdbcFieldMap(mapIOFactory.Object);
                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.SetupGet(e => e.ExternalField)
                    .Returns(new ExternalOdbcMappableField(table1.Object, new OdbcColumn("test")));
                mapEntry.SetupGet(f => f.ShipWorksField)
                    .Returns(new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number"));
                odbcFieldMap.AddEntry(mapEntry.Object);

                Mock<IOdbcFieldMapFactory> fieldMapFactory = mock.Mock<IOdbcFieldMapFactory>();
                fieldMapFactory.Setup(f => f.CreateFieldMapFrom(It.IsAny<IEnumerable<OdbcFieldMap>>()))
                    .Returns(odbcFieldMap);

                testObject.SelectedTable = table1.Object;
                testObject.TableChangedCommand.Execute(null);

                testObject.SelectedTable = table2.Object;
                testObject.TableChangedCommand.Execute(null);

                Assert.NotEqual(table1.Object, testObject.SelectedTable);
                Assert.Equal(table2.Object, testObject.SelectedTable);
            }
        }

        [Fact]
        public void SetSelectedTable_ColumnsUpdated_WhenUserContinues()
        {
            using (var mock = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var messageHelper = mock.Mock<IMessageHelper>();
                messageHelper.Setup(GetShowMessageExpression()).Returns(DialogResult.Yes);

                OdbcImportFieldMappingControlViewModel testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(mock.Mock<IOdbcDataSource>().Object);

                var table1 = mock.Mock<IOdbcTable>();
                var table2 = mock2.Mock<IOdbcTable>();

                Mock<IOdbcFieldMapIOFactory> mapIOFactory = mock.Mock<IOdbcFieldMapIOFactory>();
                OdbcFieldMap odbcFieldMap = new OdbcFieldMap(mapIOFactory.Object);
                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.SetupGet(e => e.ExternalField)
                    .Returns(new ExternalOdbcMappableField(table1.Object, new OdbcColumn("test")));
                mapEntry.SetupGet(f => f.ShipWorksField)
                    .Returns(new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number"));
                odbcFieldMap.AddEntry(mapEntry.Object);

                Mock<IOdbcFieldMapFactory> fieldMapFactory = mock.Mock<IOdbcFieldMapFactory>();
                fieldMapFactory.Setup(f => f.CreateFieldMapFrom(It.IsAny<IEnumerable<OdbcFieldMap>>()))
                    .Returns(odbcFieldMap);

                testObject.SelectedTable = table1.Object;
                testObject.TableChangedCommand.Execute(null);

                var columnsBeforeChange = testObject.Columns;

                testObject.SelectedTable = table2.Object;
                testObject.TableChangedCommand.Execute(null);

                Assert.NotEqual(columnsBeforeChange, testObject.Columns);
            }
        }

        [Fact]
        public void MapName_IsSetToDsn_WhenMapNameAndSelectedTableIsEmptyAndDsnIsSet()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var dataSource = mock.Mock<IOdbcDataSource>();
                dataSource.Setup(d => d.Name).Returns("My data source");

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(dataSource.Object);

                Assert.Equal("My data source", testObject.MapName);
            }
        }

        [Fact]
        public void MapName_IsSetToDsnAndSelectedTable_WhenMapNameIsEmptyAndSelectedTableAndDsnIsSet()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var dataSource = mock.Mock<IOdbcDataSource>();
                dataSource.Setup(d => d.Name).Returns("My data source");

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(dataSource.Object);
                testObject.SelectedTable = new OdbcTable("My table");

                Assert.Equal("My data source - My table", testObject.MapName);
            }
        }

        [Fact]
        public void SetSelectedTable_DoesNotAutomaticallySetMapName_WhenMapNameDoesNotMatchAGeneratedName()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var dataSource = mock.Mock<IOdbcDataSource>();
                dataSource.Setup(d => d.Name).Returns("My data source");

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(dataSource.Object);
                testObject.SelectedTable = new OdbcTable("My table");
                testObject.MapName = "I entered my own map name";

                Assert.Equal("I entered my own map name", testObject.MapName);
            }
        }

        [Fact]
        public void SetSelectedTable_SetsMapNameToDsnAndNewTableName_WhenMapNameIsDsn()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var dataSource = mock.Mock<IOdbcDataSource>();
                dataSource.Setup(d => d.Name).Returns("My data source");

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(dataSource.Object);
                testObject.MapName = "My data source";
                testObject.SelectedTable = new OdbcTable("My table");

                Assert.Equal("My data source - My table", testObject.MapName);
            }
        }

        [Fact]
        public void SetSelectedTable_SetsMapNameToDsnAndNewTableName_WhenMapNameIsDsnAndOldTableName()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var dataSource = mock.Mock<IOdbcDataSource>();
                dataSource.Setup(d => d.Name).Returns("My data source");

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(dataSource.Object);
                testObject.MapName = "My data source";
                testObject.SelectedTable = new OdbcTable("My old table");
                testObject.SelectedTable = new OdbcTable("My new table");

                Assert.Equal("My data source - My new table", testObject.MapName);
            }
        }

        [Fact]
        public void SetSingleLineItemTrue_MakesDisplayMapsContainOnlyOrderAddressAndSingleItem_WhenNumberOfItemsIsZero()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.NumberOfItemsPerOrder = 0;
                testObject.IsSingleLineOrder = true;
                var expectedMapNames = new List<string>() {"Order", "Address", "Item 1"};
                var actualMapNames = testObject.DisplayFieldMaps.Select(m => m.DisplayName).ToList();

                Assert.Equal(expectedMapNames, actualMapNames);
            }
        }

        [Fact]
        public void SetSingleLineItemTrue_SetsNumberOfItemsBackToOne()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.NumberOfItemsPerOrder = 5;

                testObject.IsSingleLineOrder = true;
                IEnumerable<OdbcFieldMapDisplay> itemMaps = testObject.DisplayFieldMaps.Where(m => m.DisplayName.Contains("Item"));

                Assert.Equal(1, itemMaps.Count());
            }
        }

        [Fact]
        public void SetSingleLineItemTrue_ChangesItemMapDisplayNameFromItemToItem1_WhenDisplayMapsContainsItemMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.NumberOfItemsPerOrder = 1;
                testObject.IsSingleLineOrder = false;

                // So we can see that it was previously Item
                Assert.Equal("Item", testObject.DisplayFieldMaps[2].DisplayName);

                testObject.IsSingleLineOrder = true;

                // Now it has changed to Item 1
                Assert.Equal("Item 1", testObject.DisplayFieldMaps[2].DisplayName);
            }
        }

        [Fact]
        public void SetSingleLineItemFalse_AddsItemMapToDisplayMaps_WhenNumberOfItemsPerOrderIsZero()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.NumberOfItemsPerOrder = 0;
                testObject.IsSingleLineOrder = false;

                Assert.Equal("Item", testObject.DisplayFieldMaps[2].DisplayName);
            }
        }

        [Fact]
        public void SetSingleLineItemFalse_ChangesFirstItemMapDisplayNameFromItem1ToItem_WhenDisplayMapsContainsItemMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.NumberOfItemsPerOrder = 1;
                testObject.IsSingleLineOrder = true;

                // So we can see that it was previously Item 1
                Assert.Equal("Item 1", testObject.DisplayFieldMaps[2].DisplayName);

                testObject.IsSingleLineOrder = false;

                // Now it has changed to Item
                Assert.Equal("Item", testObject.DisplayFieldMaps[2].DisplayName);
            }
        }

        [Fact]
        public void SetSingleLineItemFalse_DropsAllButFirstItemMap_WhenDisplayMapsContainsMultipleItemMaps()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.NumberOfItemsPerOrder = 20;
                testObject.IsSingleLineOrder = false;

                IEnumerable<OdbcFieldMapDisplay> itemMaps = testObject.DisplayFieldMaps.Where(m => m.DisplayName.Contains("Item"));

                Assert.Equal(1, itemMaps.Count());
            }
        }

        [Fact]
        public void SetNumberOfItemsPerOrder_DoesNotAddItemMaps_WhenSingleLineOrderIsFalse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.NumberOfItemsPerOrder = 2;

                testObject.IsSingleLineOrder = false;

                IEnumerable<OdbcFieldMapDisplay> itemMaps = testObject.DisplayFieldMaps.Where(m => m.DisplayName.Contains("Item"));

                Assert.Equal(1, itemMaps.Count());
            }
        }

        [Fact]
        public void SetNumberOfItemsPerOrder_AddsTheCorrectNumberOfItemMaps_WhenNumberOfItemsIncreases()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.IsSingleLineOrder = true;

                testObject.NumberOfItemsPerOrder = 2;
                var startingNumberOfItems = testObject.DisplayFieldMaps.Count(m => m.DisplayName.Contains("Item"));

                testObject.NumberOfItemsPerOrder = 7;
                var endNumberOfItems = testObject.DisplayFieldMaps.Count(m => m.DisplayName.Contains("Item"));

                Assert.Equal(5, endNumberOfItems - startingNumberOfItems);
            }
        }

        [Fact]
        public void SetNumberOfItemsPerOrder_AddsItemMapWithCorrectNumberOfAttributes()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var map = mock.Create<OdbcFieldMapFactory>();

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), map));
                testObject.IsSingleLineOrder = true;
                testObject.NumberOfAttributesPerItem = 10;
                testObject.NumberOfItemsPerOrder = 1;

                IEnumerable<OdbcFieldMapDisplay> itemMaps = testObject.DisplayFieldMaps.Where(m => m.DisplayName.Contains("Item"));
                var numberOfAttributeEntriesInItemMap = itemMaps.FirstOrDefault().Map.Entries.Count(e => e.ShipWorksField.DisplayName.Contains("Attribute"));

                Assert.Equal(10, numberOfAttributeEntriesInItemMap);
            }
        }

        [Fact]
        public void SetNumberOfItemsPerOrder_RemovesTheCorrectNumberOfItemMaps_WhenNumberOfItemsDecreases()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.IsSingleLineOrder = true;
                testObject.NumberOfItemsPerOrder = 7;

                var startingNumberOfItems = testObject.DisplayFieldMaps.Count(m => m.DisplayName.Contains("Item"));

                testObject.NumberOfItemsPerOrder = 2;

                var endNumberOfItems = testObject.DisplayFieldMaps.Count(m => m.DisplayName.Contains("Item"));

                Assert.Equal(5, startingNumberOfItems - endNumberOfItems);
            }
        }

        [Fact]
        public void SetNumberOfAttributesPerItem_AddsCorrectNumberOfAttributesToExistingItemMaps_WhenNumberOfAttributesIncreases()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var map = mock.Create<OdbcFieldMapFactory>();

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), map));
                testObject.IsSingleLineOrder = true;
                testObject.NumberOfAttributesPerItem = 1;
                testObject.NumberOfItemsPerOrder = 1;

                IEnumerable<OdbcFieldMapDisplay> itemMaps = testObject.DisplayFieldMaps.Where(m => m.DisplayName.Contains("Item"));
                var startingNumberOfAttributeEntriesInItemMap = itemMaps.FirstOrDefault().Map.Entries.Count(e => e.ShipWorksField.DisplayName.Contains("Attribute"));

                testObject.NumberOfAttributesPerItem = 11;

                itemMaps = testObject.DisplayFieldMaps.Where(m => m.DisplayName.Contains("Item"));
                var endNumberOfAttributeEntriesInItemMap = itemMaps.FirstOrDefault().Map.Entries.Count(e => e.ShipWorksField.DisplayName.Contains("Attribute"));

                Assert.Equal(10, endNumberOfAttributeEntriesInItemMap - startingNumberOfAttributeEntriesInItemMap);
            }
        }

        [Fact]
        public void SetNumberOfAttributesPerItem_RemovesCorrectNumberOfAttributesToExistingItemMaps_WhenNumberOfAttributesDecreases()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var map = mock.Create<OdbcFieldMapFactory>();

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), map));
                testObject.IsSingleLineOrder = true;
                testObject.NumberOfAttributesPerItem = 11;
                testObject.NumberOfItemsPerOrder = 1;

                IEnumerable<OdbcFieldMapDisplay> itemMaps = testObject.DisplayFieldMaps.Where(m => m.DisplayName.Contains("Item"));
                var startingNumberOfAttributeEntriesInItemMap = itemMaps.FirstOrDefault().Map.Entries.Count(e => e.ShipWorksField.DisplayName.Contains("Attribute"));

                testObject.NumberOfAttributesPerItem = 1;

                itemMaps = testObject.DisplayFieldMaps.Where(m => m.DisplayName.Contains("Item"));
                var endNumberOfAttributeEntriesInItemMap = itemMaps.FirstOrDefault().Map.Entries.Count(e => e.ShipWorksField.DisplayName.Contains("Attribute"));

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