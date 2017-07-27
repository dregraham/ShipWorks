using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;
using ShipWorks.Startup;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Odbc
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Store", "Odbc")]
    public class OdbcRestDownloaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DataContext context;
        private readonly Mock<IProgressReporter> mockProgressReporter;
        private readonly DbConnection dbConnection;
        private readonly long downloadLogID;
        private OdbcRecord odbcRecord;
        private readonly OdbcStoreEntity store;

        public OdbcRestDownloaderTest(DatabaseFixture db)
        {
            DateTime utcNow = DateTime.UtcNow;

            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x),
                mock =>
                {
                    mock.Override<IOdbcDownloadCommandFactory>();
                });

            mock = context.Mock;
            mockProgressReporter = mock.Mock<IProgressReporter>();

            string map = @"{
                    ""Name"": ""Sql Server - 64x - NumbersTest"",
                    ""Entries"": [
                        {
                            ""Index"": 0,
                            ""ShipWorksField"": {
                                ""ContainingObjectName"": ""OrderEntity"",
                                ""Name"": ""OrderNumberComplete"",
                                ""DisplayName"": ""Order Number"",
                                ""ResolutionStrategy"": 0
                            },
                            ""ExternalField"": {
                                ""Column"": {
                                    ""Name"": ""OrderNumber""
                                }
                            }
                        },
                        {
                            ""Index"": 0,
                            ""ShipWorksField"": {
                                ""ContainingObjectName"": ""OrderEntity"",
                                ""Name"": ""ShipFirstName"",
                                ""DisplayName"": ""Ship First Name"",
                                ""ResolutionStrategy"": 0
                            },
                            ""ExternalField"": {
                                ""Column"": {
                                    ""Name"": ""FirstName""
                                }
                            }
                        }
                    ],
                    ""RecordIdentifierSource"": ""OrderNumber""
                }";

            store = Create.Store<OdbcStoreEntity>()
                .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                .Set(x => x.StoreName, "Odbc Store")
                .Set(x => x.StoreTypeCode = StoreTypeCode.Odbc)
                .Set(x=>x.ImportMap = map)
                .Set(x=>x.ImportStrategy = (int) OdbcImportStrategy.All)
                .Save();

            Create.Entity<StatusPresetEntity>()
                .Set(x => x.StoreID = store.StoreID)
                .Set(x => x.StatusTarget = 0)
                .Set(x => x.StatusText = "status")
                .Set(x => x.IsDefault = true)
                .Save();

            Create.Entity<StatusPresetEntity>()
                .Set(x => x.StoreID = store.StoreID)
                .Set(x => x.StatusTarget = 1)
                .Set(x => x.StatusText = "status")
                .Set(x => x.IsDefault = true)
                .Save();

            StatusPresetManager.CheckForChanges();

            downloadLogID = Create.Entity<DownloadEntity>()
                .Set(x => x.StoreID = store.StoreID)
                .Set(x => x.ComputerID = context.Computer.ComputerID)
                .Set(x => x.UserID = context.User.UserID)
                .Set(x => x.InitiatedBy = (int) DownloadInitiatedBy.User)
                .Set(x => x.Started = utcNow)
                .Set(x => x.Ended = null)
                .Set(x => x.Result = (int) DownloadResult.Unfinished)
                .Save().DownloadID;

            Mock<IOdbcCommand> odbcCommand = mock.CreateMock<IOdbcCommand>();
            odbcCommand.Setup(c => c.Execute())
                .Returns(() => new[] {odbcRecord});

            mock.Mock<IOdbcDownloadCommandFactory>()
                .Setup(f => f.CreateDownloadCommand(It.IsAny<OdbcStoreEntity>(), It.IsAny<IOdbcFieldMap>()))
                .Returns(odbcCommand.Object);


            dbConnection = SqlSession.Current.OpenConnection();
        }

        [Fact]
        public void Download_OrderNumberAndOrderNumberCompleteAreSet_WhenSingleOrderDownloaded()
        {
            odbcRecord = GetOdbcRecord("001", "Kevin");

            var testObject = mock.Create<OdbcStoreDownloader>(TypedParameter.From<StoreEntity>(store));
            testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            List<OrderEntity> orders;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                orders = new LinqMetaData(adapter).Order.Where(o => o.OrderNumber != 12345).ToList();
            }

            Assert.Equal(1, orders.Count);
            Assert.Equal("1", orders.Single().OrderNumberComplete);
            Assert.Equal("Kevin", orders.Single().ShipFirstName);
            Assert.Equal(1, orders.Single().OrderNumber);
        }

        [Fact]
        public void Download_DownloadDetailsStringDataSetToOrderNumberComplete()
        {
            odbcRecord = GetOdbcRecord("001", "Kevin");

            var testObject = mock.Create<OdbcStoreDownloader>(TypedParameter.From<StoreEntity>(store));
            testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            List<DownloadDetailEntity> downloadDetailEntities;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                downloadDetailEntities = new LinqMetaData(adapter).DownloadDetail.ToList();
            }

            Assert.Equal("1", downloadDetailEntities.Single().ExtraStringData1);
        }

        [Fact]
        public void Download_RunTwice_UpdatesExistingOrder()
        {
            odbcRecord = GetOdbcRecord("001", "Kevin");
            var testObject1 = mock.Create<OdbcStoreDownloader>(TypedParameter.From<StoreEntity>(store));
            testObject1.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            odbcRecord = GetOdbcRecord("001", "Alex");
            var testObject2 = mock.Create<OdbcStoreDownloader>(TypedParameter.From<StoreEntity>(store));
            testObject2.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            List<OrderEntity> orders;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                orders = new LinqMetaData(adapter).Order.Where(o => o.OrderNumber != 12345).ToList();
            }

            Assert.Equal(1, orders.Count);
            Assert.Equal("1", orders.Single().OrderNumberComplete);
            Assert.Equal("Alex", orders.Single().ShipFirstName);
        }

        [Fact]
        public void Download_RunTwice_WithOrdersWithDifferentAmountOfLeadingZeros_TreatsOrdersAsTheSame()
        {
            odbcRecord = GetOdbcRecord("55", "Kevin");
            var testObject1 = mock.Create<OdbcStoreDownloader>(TypedParameter.From<StoreEntity>(store));
            testObject1.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            odbcRecord = GetOdbcRecord("0055", "Alex");
            var testObject2 = mock.Create<OdbcStoreDownloader>(TypedParameter.From<StoreEntity>(store));
            testObject2.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            List<OrderEntity> orders;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                orders = new LinqMetaData(adapter).Order.Where(o => o.OrderNumber != 12345).ToList();
            }

            Assert.Equal(1, orders.Count);
            Assert.Equal("55", orders.Single().OrderNumberComplete);
            Assert.Equal("Alex", orders.Single().ShipFirstName);
        }

        private OdbcRecord GetOdbcRecord(string orderNumber, string FirstName)
        {
            var newRecord = new OdbcRecord("OrderNumber");
            newRecord.AddField("OrderNumber", orderNumber);
            newRecord.AddField("FirstName", FirstName);
            return newRecord;
        }

        public void Dispose()
        {
            mock?.Dispose();
            context?.Dispose();
            dbConnection?.Dispose();
        }
    }
}