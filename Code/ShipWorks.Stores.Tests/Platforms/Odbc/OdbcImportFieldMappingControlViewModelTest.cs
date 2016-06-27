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
                var mapFactory = mock.Create<OdbcFieldMapFactory>();

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

                Assert.NotEmpty(testObject.Order.Entries);
            }
        }

        [Fact]
        public void Constructor_AddressFieldMapSetByCreateAddressFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

                Assert.NotEmpty(testObject.Address.Entries);
            }
        }

        [Fact]
        public void Constructor_ItemFieldMapSetByCreateItemFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

                testObject.NumberOfItemsPerOrder = 1;
                testObject.IsSingleLineOrder = true;

                Assert.NotEmpty(testObject.Items[0].Entries);
            }
        }

        [Fact]
        public void Constructor_SelectedFieldMapIsOrderFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

                Assert.Equal(testObject.Order, testObject.SelectedFieldMap);
            }
        }

        [Fact]
        public void Constructor_OrderMapDisplayName_IsOrder()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

                Assert.Equal("Order", testObject.Order.DisplayName);
            }
        }

        [Fact]
        public void Constructor_AddressMapDisplayName_IsOrder()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

                Assert.Equal("Address", testObject.Address.DisplayName);
            }
        }

        [Fact]
        public void Load_ThrowsArgumentNullException_WhenDataSourceIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

                Assert.Throws<ArgumentNullException>(() => testObject.Load(null));
            }
        }

        [Fact]
        public void Load_LoadsSchemaTables()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IOdbcDataSource> odbcDataSource = new Mock<IOdbcDataSource>();
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

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
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

                testObject.Load(dataSource.Object);

                messageHelper.Verify(m => m.ShowError(It.IsAny<string>()), Times.Once);
            }
        }

        [Fact]
        public void Save_ThrowsArgumentNullException_WhenStoreIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

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

                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(mock.Mock<IOdbcDataSource>().Object);

                var table = mock.Mock<IOdbcColumnSource>();

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

                var table1 = mock.Mock<IOdbcColumnSource>();
                var table2 = mock2.Mock<IOdbcColumnSource>();

                Mock<IOdbcFieldMapIOFactory> mapIOFactory = mock.Mock<IOdbcFieldMapIOFactory>();
                OdbcFieldMap odbcFieldMap = new OdbcFieldMap(mapIOFactory.Object);
                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.SetupGet(e => e.ExternalField)
                    .Returns(new ExternalOdbcMappableField(table1.Object, new OdbcColumn("test")));
                mapEntry.SetupGet(f => f.ShipWorksField)
                    .Returns(new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number"));
                odbcFieldMap.AddEntry(mapEntry.Object);

                Mock<IOdbcFieldMapFactory> fieldMapFactory = mock.Mock<IOdbcFieldMapFactory>();
                fieldMapFactory.Setup(f => f.CreateFieldMapFrom(It.IsAny<IEnumerable<IOdbcFieldMapEntry>>()))
                    .Returns(odbcFieldMap);
                fieldMapFactory.Setup(f => f.CreateOrderFieldMap()).Returns(odbcFieldMap);
                fieldMapFactory.Setup(f => f.CreateAddressFieldMap()).Returns(odbcFieldMap);

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(mock.Mock<IOdbcDataSource>().Object);

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


                var table1 = mock.Mock<IOdbcColumnSource>();
                var table2 = mock2.Mock<IOdbcColumnSource>();

                Mock<IOdbcFieldMapIOFactory> mapIOFactory = mock.Mock<IOdbcFieldMapIOFactory>();
                OdbcFieldMap odbcFieldMap = new OdbcFieldMap(mapIOFactory.Object);
                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.SetupGet(e => e.ExternalField)
                    .Returns(new ExternalOdbcMappableField(table1.Object, new OdbcColumn("test")));
                mapEntry.SetupGet(f => f.ShipWorksField)
                    .Returns(new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number"));
                odbcFieldMap.AddEntry(mapEntry.Object);

                Mock<IOdbcFieldMapFactory> fieldMapFactory = mock.Mock<IOdbcFieldMapFactory>();
                fieldMapFactory.Setup(f => f.CreateFieldMapFrom(It.IsAny<IEnumerable<IOdbcFieldMapEntry>>()))
                    .Returns(odbcFieldMap);
                fieldMapFactory.Setup(f => f.CreateOrderFieldMap()).Returns(odbcFieldMap);
                fieldMapFactory.Setup(f => f.CreateAddressFieldMap()).Returns(odbcFieldMap);

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(mock.Mock<IOdbcDataSource>().Object);

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

                var table1 = mock.Mock<IOdbcColumnSource>();
                var table2 = mock2.Mock<IOdbcColumnSource>();

                Mock<IOdbcFieldMapIOFactory> mapIOFactory = mock.Mock<IOdbcFieldMapIOFactory>();
                OdbcFieldMap odbcFieldMap = new OdbcFieldMap(mapIOFactory.Object);

                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.SetupGet(e => e.ExternalField)
                    .Returns(new ExternalOdbcMappableField(table1.Object, new OdbcColumn("test")));
                mapEntry.SetupGet(f => f.ShipWorksField)
                    .Returns(new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number"));
                odbcFieldMap.AddEntry(mapEntry.Object);

                Mock<IOdbcFieldMapFactory> fieldMapFactory = mock.Mock<IOdbcFieldMapFactory>();
                fieldMapFactory.Setup(f => f.CreateFieldMapFrom(It.IsAny<IEnumerable<IOdbcFieldMapEntry>>()))
                    .Returns(odbcFieldMap);
                fieldMapFactory.Setup(f => f.CreateOrderFieldMap()).Returns(odbcFieldMap);
                fieldMapFactory.Setup(f => f.CreateAddressFieldMap()).Returns(odbcFieldMap);

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(mock.Mock<IOdbcDataSource>().Object);

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

                var table1 = mock.Mock<IOdbcColumnSource>();
                var table2 = mock2.Mock<IOdbcColumnSource>();

                Mock<IOdbcFieldMapIOFactory> mapIOFactory = mock.Mock<IOdbcFieldMapIOFactory>();
                OdbcFieldMap odbcFieldMap = new OdbcFieldMap(mapIOFactory.Object);
                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.SetupGet(e => e.ExternalField)
                    .Returns(new ExternalOdbcMappableField(table1.Object, new OdbcColumn("test")));
                mapEntry.SetupGet(f => f.ShipWorksField)
                    .Returns(new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number"));
                odbcFieldMap.AddEntry(mapEntry.Object);

                Mock<IOdbcFieldMapFactory> fieldMapFactory = mock.Mock<IOdbcFieldMapFactory>();
                fieldMapFactory.Setup(f => f.CreateFieldMapFrom(It.IsAny<IEnumerable<IOdbcFieldMapEntry>>()))
                    .Returns(odbcFieldMap);
                fieldMapFactory.Setup(f => f.CreateOrderFieldMap()).Returns(odbcFieldMap);
                fieldMapFactory.Setup(f => f.CreateAddressFieldMap()).Returns(odbcFieldMap);

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(mock.Mock<IOdbcDataSource>().Object);

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


                var table1 = mock.Mock<IOdbcColumnSource>();
                var table2 = mock2.Mock<IOdbcColumnSource>();

                Mock<IOdbcFieldMapIOFactory> mapIOFactory = mock.Mock<IOdbcFieldMapIOFactory>();
                OdbcFieldMap odbcFieldMap = new OdbcFieldMap(mapIOFactory.Object);
                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.SetupGet(e => e.ExternalField)
                    .Returns(new ExternalOdbcMappableField(table1.Object, new OdbcColumn("test")));
                mapEntry.SetupGet(f => f.ShipWorksField)
                    .Returns(new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number"));
                odbcFieldMap.AddEntry(mapEntry.Object);

                Mock<IOdbcFieldMapFactory> fieldMapFactory = mock.Mock<IOdbcFieldMapFactory>();
                fieldMapFactory.Setup(f => f.CreateFieldMapFrom(It.IsAny<IEnumerable<IOdbcFieldMapEntry>>()))
                    .Returns(odbcFieldMap);
                fieldMapFactory.Setup(f => f.CreateOrderFieldMap()).Returns(odbcFieldMap);
                fieldMapFactory.Setup(f => f.CreateAddressFieldMap()).Returns(odbcFieldMap);

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(mock.Mock<IOdbcDataSource>().Object);


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

                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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

                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(dataSource.Object);
                testObject.SelectedTable = new OdbcColumnSource("My table");

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

                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(dataSource.Object);
                testObject.SelectedTable = new OdbcColumnSource("My table");
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

                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(dataSource.Object);
                testObject.MapName = "My data source";
                testObject.SelectedTable = new OdbcColumnSource("My table");

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

                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(dataSource.Object);
                testObject.MapName = "My data source";
                testObject.SelectedTable = new OdbcColumnSource("My old table");
                testObject.SelectedTable = new OdbcColumnSource("My new table");

                Assert.Equal("My data source - My new table", testObject.MapName);
            }
        }

        [Fact]
        public void SetSingleLineItemTrue_MakesDisplayMapsContainOnlyOrderAddressAndSingleItem_WhenNumberOfItemsIsZero()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
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