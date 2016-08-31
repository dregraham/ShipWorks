using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Upload;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.ViewModels.Upload
{
    public class OdbcUploadMappingControlViewModelTest
    {
        [Fact]
        public void Constructor_ShipmentFieldMapSetByCreateOrderFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();

                var testObject = mock.Create<OdbcUploadMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

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
                testObject.Load(new OdbcStoreEntity());

                Assert.NotEmpty(testObject.ShipmentAddress.Entries);
            }
        }

        [Fact]
        public void Constructor_SelectedFieldMapIsShipmentFieldMap()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mapFactory = mock.Create<OdbcFieldMapFactory>();
                var testObject = mock.Create<OdbcUploadMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory), mapFactory));
                testObject.Load(new OdbcStoreEntity());

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
                testObject.Load(new OdbcStoreEntity());

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
                testObject.Load(new OdbcStoreEntity());

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

                Assert.Throws<ArgumentNullException>(() => testObject.Load(null));
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

        [Fact]
        public void ValidateRequiredMappingFields_DelegatesToMessageHelper_WhenOneRequiredFieldIsNotMapped()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string mapPath = "ShipWorks.Stores.Tests.Platforms.Odbc.Artifacts.UploadMap.json";

                List<string> columnNames = new List<string>()
                {
                    "OrderNumber"
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
                string mapPath = "ShipWorks.Stores.Tests.Platforms.Odbc.Artifacts.UploadMap.json";

                List<string> columnNames = new List<string>()
                {
                    "ShipDate"
                };

                var messageHelper = mock.Mock<IMessageHelper>();

                var testObject = CreateViewModelWithLoadedEntries(mock, columnNames, mapPath);
                testObject.ValidateRequiredMappingFields();

                messageHelper.Verify(m => m.ShowError(It.IsAny<string>()), Times.Once);
            }
        }

        [Fact]
        public void ValidateRequiredMappingFields_DelegatesToMessageHelper_WhenExternalFieldIsMappedMutlipleTimes()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string mapPath = "ShipWorks.Stores.Tests.Platforms.Odbc.Artifacts.UploadMapWithDuplicateEntries.json";

                List<string> columnNames = new List<string>()
                {
                    "OrderNumber",
                    "TrackingNumber",
                    "ShipDate"
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
                string mapPath = "ShipWorks.Stores.Tests.Platforms.Odbc.Artifacts.UploadMap.json";

                List<string> columnNames = new List<string>()
                {
                    "OrderNumber",
                    "TrackingNumber",
                    "ShipDate"
                };


                var testObject = CreateViewModelWithLoadedEntries(mock, columnNames, mapPath);

                Assert.True(testObject.ValidateRequiredMappingFields());
            }
        }

        private static OdbcUploadMappingControlViewModel CreateViewModelWithLoadedEntries(AutoMock mock, List<string> columnNames, string mapPath)
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
                UploadMap = EmbeddedResourceHelper.GetEmbeddedResourceString(mapPath),
                UploadColumnSourceType = (int)OdbcColumnSourceType.Table,
                UploadColumnSource = "Table"
            };

            Mock<IOdbcDataSourceService> dataSourceService = mock.Mock<IOdbcDataSourceService>();
            dataSourceService.Setup(s => s.GetUploadDataSource(store)).Returns(dataSource.Object);

            var mapFactory = mock.Create<OdbcFieldMapFactory>(new TypedParameter(typeof(IOdbcFieldMapIOFactory), ioFactory));

            var testObject = mock.Create<OdbcUploadMappingControlViewModel>(new TypedParameter(typeof(IOdbcFieldMapFactory),
                mapFactory), new TypedParameter(typeof(Func<string, IOdbcColumnSource>), repo.Object));

            testObject.Load(store);

            return testObject;
        }
    }
}