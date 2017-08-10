using System;
using System.Data.Common;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Upload
{
    public class OdbcTableUploadQueryTest : IDisposable
    {
        private readonly AutoMock mock;
        public OdbcTableUploadQueryTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void GenerateSql_GeneratesSqlContainingMappedFields()
        {
            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            Mock<IShipWorksOdbcDataAdapter> adapter = mock.Mock<IShipWorksOdbcDataAdapter>();
            Mock<IShipWorksOdbcCommandBuilder> cmdBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();
            cmdBuilder.Setup(c => c.QuoteIdentifier(It.IsAny<string>())).Returns((string c) => c);

            OdbcFieldMapEntry orderNumberCompleteEntry = new OdbcFieldMapEntry(
                new ShipWorksOdbcMappableField(OrderFields.OrderNumberComplete, "Order Number", OdbcFieldValueResolutionStrategy.Default),
                new ExternalOdbcMappableField(new OdbcColumn("ExternalOrderNumberCompleteColumn")));

            Mock<IOdbcFieldMap> fieldMap = mock.Mock<IOdbcFieldMap>();
            fieldMap.Setup(f => f.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { orderNumberCompleteEntry });
            fieldMap.SetupGet(f => f.Entries)
                .Returns(new[]
                {
                   orderNumberCompleteEntry,
                    new OdbcFieldMapEntry(
                        new ShipWorksOdbcMappableField(ShipmentFields.TrackingNumber, "Tracking Number", OdbcFieldValueResolutionStrategy.Default),
                        new ExternalOdbcMappableField(new OdbcColumn("ExternalShipmentTrackingField")))
                });
            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcDataAdapter(It.IsAny<string>(), It.IsAny<DbConnection>()))
                .Returns(adapter.Object);
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcCommandBuilder(It.IsAny<IShipWorksOdbcDataAdapter>()))
                .Returns(cmdBuilder.Object);
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcStoreEntity store = new OdbcStoreEntity
            {
                UploadColumnSourceType = (int)OdbcColumnSourceType.Table,
                UploadColumnSource = "Orders"
            };

            OdbcTableUploadQuery testObject = mock.Create<OdbcTableUploadQuery>(new TypedParameter(typeof(OdbcStoreEntity), store));
            string result = testObject.GenerateSql();

            Assert.Contains("ExternalShipmentTrackingField", result);
        }

        [Fact]
        public void GenerateSql_GeneratesSqlContainingStoreColumnSource()
        {
            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            Mock<IShipWorksOdbcDataAdapter> adapter = mock.Mock<IShipWorksOdbcDataAdapter>();
            Mock<IShipWorksOdbcCommandBuilder> cmdBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();
            cmdBuilder.Setup(c => c.QuoteIdentifier(It.IsAny<string>())).Returns((string c) => c);

            OdbcFieldMapEntry orderNumberCompleteEntry = new OdbcFieldMapEntry(
                new ShipWorksOdbcMappableField(OrderFields.OrderNumberComplete, "Order Number", OdbcFieldValueResolutionStrategy.Default),
                new ExternalOdbcMappableField(new OdbcColumn("ExternalOrderNumberCompleteColumn")));

            Mock<IOdbcFieldMap> fieldMap = mock.Mock<IOdbcFieldMap>();
            fieldMap.Setup(f => f.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] {orderNumberCompleteEntry});
            fieldMap.SetupGet(f => f.Entries)
                .Returns(new[]
                {
                   orderNumberCompleteEntry,
                    new OdbcFieldMapEntry(
                        new ShipWorksOdbcMappableField(ShipmentFields.TrackingNumber, "Tracking Number", OdbcFieldValueResolutionStrategy.Default),
                        new ExternalOdbcMappableField(new OdbcColumn("ExternalShipmentTrackingField")))
                });
            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcDataAdapter(It.IsAny<string>(), It.IsAny<DbConnection>()))
                .Returns(adapter.Object);
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcCommandBuilder(It.IsAny<IShipWorksOdbcDataAdapter>()))
                .Returns(cmdBuilder.Object);
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcStoreEntity store = new OdbcStoreEntity
            {
                UploadColumnSourceType = (int)OdbcColumnSourceType.Table,
                UploadColumnSource = "Orders"
            };

            OdbcTableUploadQuery testObject = mock.Create<OdbcTableUploadQuery>(new TypedParameter(typeof(OdbcStoreEntity), store));
            string result = testObject.GenerateSql();

            Assert.Contains(store.UploadColumnSource, result);
        }

        [Fact]
        public void GenerateSql_ThrowsShipWorksOdbcException_WhenFieldMapDoesNotContainOrderNumberComplete()
        {
            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            Mock<IShipWorksOdbcDataAdapter> adapter = mock.Mock<IShipWorksOdbcDataAdapter>();
            Mock<IShipWorksOdbcCommandBuilder> cmdBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();
            mock.Mock<IOdbcFieldMap>();
            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcDataAdapter(It.IsAny<string>(), It.IsAny<DbConnection>()))
                .Returns(adapter.Object);
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcCommandBuilder(It.IsAny<IShipWorksOdbcDataAdapter>()))
                .Returns(cmdBuilder.Object);
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcStoreEntity store = new OdbcStoreEntity()
            {
                UploadColumnSourceType = (int)OdbcColumnSourceType.Table,
                UploadColumnSource = "Orders"
            };

            OdbcTableUploadQuery testObject = mock.Create<OdbcTableUploadQuery>(new TypedParameter(typeof(OdbcStoreEntity), store));

            Assert.Throws<ShipWorksOdbcException>(() => testObject.GenerateSql());
        }

        [Fact]
        public void GenerateSql_ThrowsShipWorksOdbcException_WhenStoreUploadColumnSourceTypeIsNotTable()
        {
            OdbcStoreEntity store = new OdbcStoreEntity
            {
                UploadColumnSourceType = (int) OdbcColumnSourceType.CustomQuery
            };
            OdbcTableUploadQuery testObject = mock.Create<OdbcTableUploadQuery>(new TypedParameter(typeof(OdbcStoreEntity), store));

            Assert.Throws<ShipWorksOdbcException>(() => testObject.GenerateSql());
        }

        [Fact]
        public void ConfigureCommand_ChangesCommandText()
        {
            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            Mock<IShipWorksOdbcDataAdapter> adapter = mock.Mock<IShipWorksOdbcDataAdapter>();
            Mock<IShipWorksOdbcCommandBuilder> cmdBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();
            cmdBuilder.Setup(c => c.QuoteIdentifier(It.IsAny<string>())).Returns((string c) => c);

            ShipWorksOdbcMappableField orderNumberCompleteField = new ShipWorksOdbcMappableField(OrderFields.OrderNumberComplete, "Order Number",
                OdbcFieldValueResolutionStrategy.Default);
            orderNumberCompleteField.LoadValue(123);

            OdbcFieldMapEntry orderNumberCompleteEntry = new OdbcFieldMapEntry(
                orderNumberCompleteField,
                new ExternalOdbcMappableField(new OdbcColumn("ExternalOrderNumberCompleteColumn")));

            Mock<IOdbcFieldMap> fieldMap = mock.Mock<IOdbcFieldMap>();
            fieldMap.Setup(f => f.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { orderNumberCompleteEntry });
            fieldMap.SetupGet(f => f.Entries)
                .Returns(new[]
                {
                   orderNumberCompleteEntry,
                    new OdbcFieldMapEntry(
                        new ShipWorksOdbcMappableField(ShipmentFields.TrackingNumber, "Tracking Number", OdbcFieldValueResolutionStrategy.Default),
                        new ExternalOdbcMappableField(new OdbcColumn("ExternalShipmentTrackingField")))
                });
            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcDataAdapter(It.IsAny<string>(), It.IsAny<DbConnection>()))
                .Returns(adapter.Object);
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcCommandBuilder(It.IsAny<IShipWorksOdbcDataAdapter>()))
                .Returns(cmdBuilder.Object);
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();

            OdbcStoreEntity store = new OdbcStoreEntity
            {
                UploadColumnSourceType = (int)OdbcColumnSourceType.Table,
                UploadColumnSource = "Orders"
            };

            OdbcTableUploadQuery testObject = mock.Create<OdbcTableUploadQuery>(new TypedParameter(typeof(OdbcStoreEntity), store));
            testObject.ConfigureCommand(command.Object);

            command.Verify(c => c.ChangeCommandText(It.IsAny<string>()));
        }

        [Fact]
        public void ConfigureCommand_AddsParametersForEachEntry()
        {
            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            Mock<IShipWorksOdbcDataAdapter> adapter = mock.Mock<IShipWorksOdbcDataAdapter>();
            Mock<IShipWorksOdbcCommandBuilder> cmdBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();
            cmdBuilder.Setup(c => c.QuoteIdentifier(It.IsAny<string>())).Returns((string c) => c);

            ShipWorksOdbcMappableField orderNumberCompleteField = new ShipWorksOdbcMappableField(OrderFields.OrderNumberComplete,
                "Order Number", OdbcFieldValueResolutionStrategy.Default);
            orderNumberCompleteField.LoadValue(123);

            OdbcFieldMapEntry orderNumberCompleteEntry = new OdbcFieldMapEntry(
                orderNumberCompleteField,
                new ExternalOdbcMappableField(new OdbcColumn("ExternalOrderNumberCompleteColumn")));

            Mock<IOdbcFieldMap> fieldMap = mock.Mock<IOdbcFieldMap>();
            fieldMap.Setup(f => f.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { orderNumberCompleteEntry });
            fieldMap.SetupGet(f => f.Entries)
                .Returns(new[]
                {
                   orderNumberCompleteEntry,
                    new OdbcFieldMapEntry(
                        new ShipWorksOdbcMappableField(ShipmentFields.TrackingNumber, "Tracking Number", OdbcFieldValueResolutionStrategy.Default),
                        new ExternalOdbcMappableField(new OdbcColumn("ExternalShipmentTrackingField")))
                });
            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcDataAdapter(It.IsAny<string>(), It.IsAny<DbConnection>()))
                .Returns(adapter.Object);
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcCommandBuilder(It.IsAny<IShipWorksOdbcDataAdapter>()))
                .Returns(cmdBuilder.Object);
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();

            OdbcStoreEntity store = new OdbcStoreEntity
            {
                UploadColumnSourceType = (int)OdbcColumnSourceType.Table,
                UploadColumnSource = "Orders"
            };

            OdbcTableUploadQuery testObject = mock.Create<OdbcTableUploadQuery>(new TypedParameter(typeof(OdbcStoreEntity), store));
            testObject.ConfigureCommand(command.Object);

            command.Verify(c=>c.AddParameter(It.IsAny<string>(), It.IsAny<object>()), Times.Exactly(2));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}