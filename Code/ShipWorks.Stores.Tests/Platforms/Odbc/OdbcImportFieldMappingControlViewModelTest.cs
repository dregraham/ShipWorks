using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.UI.Platforms.Odbc;
using System;
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

                Assert.Equal(odbcFieldMap, testObject.OrderFieldMap.Map);
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

                Assert.Equal(odbcFieldMap, testObject.AddressFieldMap.Map);
            }
        }

        [Fact]
        public void Constructor_ItemFieldMapSetByCreateItemFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                OdbcFieldMap odbcFieldMap = mock.Create<OdbcFieldMap>();
                mock.Mock<IOdbcFieldMapFactory>()
                    .Setup(f => f.CreateOrderItemFieldMap())
                    .Returns(odbcFieldMap);

                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();

                Assert.Equal(odbcFieldMap, testObject.ItemFieldMap.Map);
            }
        }

        [Fact]
        public void Constructor_SelectedFieldMapIsOrderFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();

                Assert.Equal(testObject.OrderFieldMap, testObject.SelectedFieldMap);
            }
        }

        [Fact]
        public void Constructor_FieldMaps_ContainsOrderFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();

                Assert.Equal(new[] {testObject.OrderFieldMap, testObject.AddressFieldMap, testObject.ItemFieldMap},
                    testObject.FieldMaps);
            }
        }

        [Fact]
        public void Constructor_OrderFieldMapDisplayName_IsOrder()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();

                Assert.Equal("Order", testObject.OrderFieldMap.DisplayName);
            }
        }

        [Fact]
        public void Constructor_AddressFieldMapDisplayName_IsOrder()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();

                Assert.Equal("Address", testObject.AddressFieldMap.DisplayName);
            }
        }

        [Fact]
        public void Constructor_ItemFieldMapDisplayName_IsOrder()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();

                Assert.Equal("Item", testObject.ItemFieldMap.DisplayName);
            }
        }

        [Fact]
        public void Load_ThrowsArgumentNullException_WhenStoreIsNull()
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
                Mock<OdbcStoreEntity> store = new Mock<OdbcStoreEntity>();
                OdbcImportFieldMappingControlViewModel testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(store.Object);
                Assert.NotNull(testObject.Tables);
            }
        }

        [Fact]
        public void Load_DisplaysErrorMessage_WhenShipWorksOdbcExceptionIsThrown()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<OdbcStoreEntity> store = new Mock<OdbcStoreEntity>();
                Mock<IOdbcSchema> shema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IMessageHelper> messageHelper = mock.Mock<IMessageHelper>();
                shema.Setup(s => s.Load(dataSource.Object)).Throws<ShipWorksOdbcException>();
                OdbcImportFieldMappingControlViewModel testObject =
                    mock.Create<OdbcImportFieldMappingControlViewModel>();

                testObject.Load(store.Object);

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
    }
}