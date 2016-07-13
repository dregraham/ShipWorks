using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels;
using System;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcUploadMappingControlViewModelTest
    {
        [Fact]
        public void Constructor_OrderFieldMapSetByCreateOrderFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();

                var testObject = mock.Create<OdbcUploadMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

                Assert.NotEmpty(testObject.Shipment.Entries);
            }
        }

        [Fact]
        public void Constructor_AddressFieldMapSetByCreateAddressFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcUploadMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

                Assert.NotEmpty(testObject.ShipmentAddress.Entries);
            }
        }

        [Fact]
        public void Constructor_SelectedFieldMapIsOrderFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcUploadMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

                Assert.Equal(testObject.Shipment, testObject.SelectedFieldMap);
            }
        }

        [Fact]
        public void Constructor_OrderMapDisplayName_IsOrder()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcUploadMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

                Assert.Equal("Shipment", testObject.Shipment.DisplayName);
            }
        }

        [Fact]
        public void Constructor_AddressMapDisplayName_IsOrder()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcUploadMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

                Assert.Equal("Address", testObject.ShipmentAddress.DisplayName);
            }
        }

        [Fact]
        public void Load_ThrowsArgumentNullException_WhenDataSourceIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcUploadMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

                Assert.Throws<ArgumentNullException>(() => testObject.LoadColumnSource(null));
            }
        }

        [Fact]
        public void Save_ThrowsArgumentNullException_WhenStoreIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcUploadMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));

                Assert.Throws<ArgumentNullException>(() => testObject.Save(null));
            }
        }
    }
}