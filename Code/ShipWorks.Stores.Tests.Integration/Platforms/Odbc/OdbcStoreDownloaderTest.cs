﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
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
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Odbc
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Store", "Odbc")]
    public class OdbcStoreDownloaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DataContext context;
        private readonly Mock<IProgressReporter> mockProgressReporter;
        private readonly DbConnection dbConnection;
        private readonly IDownloadEntity downloadLog;
        private OdbcRecord odbcRecord;
        private readonly OdbcStoreEntity store;

        public OdbcStoreDownloaderTest(DatabaseFixture db)
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
                .Set(x => x.ImportMap = map)
                .Set(x => x.ImportStrategy = (int) OdbcImportStrategy.All)
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

            downloadLog = Create.Entity<DownloadEntity>()
                .Set(x => x.StoreID = store.StoreID)
                .Set(x => x.ComputerID = context.Computer.ComputerID)
                .Set(x => x.UserID = context.User.UserID)
                .Set(x => x.InitiatedBy = (int) DownloadInitiatedBy.User)
                .Set(x => x.Started = utcNow)
                .Set(x => x.Ended = null)
                .Set(x => x.Result = (int) DownloadResult.Unfinished)
                .Save();

            Mock<IOdbcCommand> odbcCommand = mock.CreateMock<IOdbcCommand>();
            odbcCommand.Setup(c => c.Execute())
                .Returns(() => new[] { odbcRecord });

            mock.Mock<IOdbcDownloadCommandFactory>()
                .Setup(f => f.CreateDownloadCommand(It.IsAny<OdbcStoreEntity>(), It.IsAny<IOdbcFieldMap>()))
                .Returns(odbcCommand.Object);

            dbConnection = SqlSession.Current.OpenConnection();
        }

        [Fact]
        public async Task DownloadByOrderNumber_DownloadsNewOrder_WhenOrderNotFound()
        {
            odbcRecord = GetOdbcRecord("1", "Kevin");

            Mock<IOdbcCommand> odbcCommand = mock.CreateMock<IOdbcCommand>();
            odbcCommand.Setup(c => c.Execute())
                .Returns(() => new[] { odbcRecord });

            mock.Mock<IOdbcDownloadCommandFactory>()
                .Setup(f => f.CreateDownloadCommand(It.IsAny<OdbcStoreEntity>(), AnyString, It.IsAny<IOdbcFieldMap>()))
                .Returns(odbcCommand.Object);

            store.ImportStrategy = (int) OdbcImportStrategy.OnDemand;
            var testObject = mock.Create<OdbcStoreDownloader>(TypedParameter.From<StoreEntity>(store));
            await testObject.Download("1", downloadLog.DownloadID, dbConnection);

            odbcCommand.Verify(v => v.Execute(), Times.Once);
        }

        [Fact]
        public async Task DownloadByOrderNumber_DownloadsExistingOrder()
        {
            odbcRecord = GetOdbcRecord("1", "Kevin");

            Create.Order<OrderEntity>(store, context.Customer)
                .Set(o => o.OrderNumber = 1)
                .Save();

            Mock<IOdbcCommand> odbcCommand = mock.CreateMock<IOdbcCommand>();
            odbcCommand.Setup(c => c.Execute())
                .Returns(() => new[] { odbcRecord });

            mock.Mock<IOdbcDownloadCommandFactory>()
                .Setup(f => f.CreateDownloadCommand(It.IsAny<OdbcStoreEntity>(), AnyString, It.IsAny<IOdbcFieldMap>()))
                .Returns(odbcCommand.Object);

            store.ImportStrategy = (int) OdbcImportStrategy.OnDemand;
            var testObject = mock.Create<OdbcStoreDownloader>(TypedParameter.From<StoreEntity>(store));
            await testObject.Download("1", downloadLog.DownloadID, dbConnection);

            odbcCommand.Verify(v => v.Execute(), Times.Once);
        }
        [Fact]
        public async Task DownloadByOrderNumber_ReplacesLineItems_WhenDownloadingExistingOrder()
        {
            odbcRecord = GetOdbcRecord("1", "Kevin");

            Create.Order<OrderEntity>(store, context.Customer)
                .WithItem(i => i.Set(item => item.Name = "blah"))
                .Set(o => o.OrderNumber = 1)
                .Save();

            Mock<IOdbcCommand> odbcCommand = mock.CreateMock<IOdbcCommand>();
            odbcCommand.Setup(c => c.Execute())
                .Returns(() => new[] { odbcRecord });

            mock.Mock<IOdbcDownloadCommandFactory>()
                .Setup(f => f.CreateDownloadCommand(It.IsAny<OdbcStoreEntity>(), AnyString, It.IsAny<IOdbcFieldMap>()))
                .Returns(odbcCommand.Object);

            store.ImportStrategy = (int) OdbcImportStrategy.OnDemand;
            var testObject = mock.Create<OdbcStoreDownloader>(TypedParameter.From<StoreEntity>(store));
            await testObject.Download("1", downloadLog.DownloadID, dbConnection);

            List<OrderItemEntity> orderItems;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                orderItems = new LinqMetaData(adapter).OrderItem.ToList();
            }

            Assert.True(orderItems.None());
        }

        [Fact]
        public async Task Download_OrderNumberAndOrderNumberCompleteAreSet_WhenSingleOrderDownloaded()
        {
            odbcRecord = GetOdbcRecord("001", "Kevin");

            var testObject = mock.Create<OdbcStoreDownloader>(TypedParameter.From<StoreEntity>(store));
            await testObject.Download(mockProgressReporter.Object, downloadLog, dbConnection);

            List<OrderEntity> orders;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                orders = new LinqMetaData(adapter).Order.Where(o => o.OrderNumber != 12345).ToList();
            }

            Assert.Equal(1, orders.Count);
            Assert.Equal("001", orders.Single().OrderNumberComplete);
            Assert.Equal("Kevin", orders.Single().ShipFirstName);
            Assert.Equal(1, orders.Single().OrderNumber);
        }

        [Fact]
        public async Task Download_ThrowsDownloadException_WhenStoreImportStrategyIsOnDemand()
        {
            odbcRecord = GetOdbcRecord("001", "Kevin");
            store.ImportStrategy = (int) OdbcImportStrategy.OnDemand;

            var testObject = mock.Create<OdbcStoreDownloader>(TypedParameter.From<StoreEntity>(store));

            var exception = await Assert.ThrowsAsync<DownloadException>(() => testObject.Download(mockProgressReporter.Object, downloadLog, dbConnection));
            Assert.StartsWith("The store, Odbc Store, is set to download orders on order search only.", exception.Message);
        }

        [Fact]
        public async Task Download_DownloadDetailsStringDataSetToOrderNumberComplete()
        {
            odbcRecord = GetOdbcRecord("001", "Kevin");

            var testObject = mock.Create<OdbcStoreDownloader>(TypedParameter.From<StoreEntity>(store));
            await testObject.Download(mockProgressReporter.Object, downloadLog, dbConnection);

            List<DownloadDetailEntity> downloadDetailEntities;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                downloadDetailEntities = new LinqMetaData(adapter).DownloadDetail.ToList();
            }

            Assert.Equal("001", downloadDetailEntities.Single().ExtraStringData1);
        }

        [Fact]
        public async Task Download_RunTwice_UpdatesExistingOrder()
        {
            odbcRecord = GetOdbcRecord("001", "Kevin");
            var testObject1 = mock.Create<OdbcStoreDownloader>(TypedParameter.From<StoreEntity>(store));
            await testObject1.Download(mockProgressReporter.Object, downloadLog, dbConnection);

            odbcRecord = GetOdbcRecord("001", "Alex");
            var testObject2 = mock.Create<OdbcStoreDownloader>(TypedParameter.From<StoreEntity>(store));
            await testObject2.Download(mockProgressReporter.Object, downloadLog, dbConnection);

            List<OrderEntity> orders;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                orders = new LinqMetaData(adapter).Order.Where(o => o.OrderNumber != 12345).ToList();
            }

            Assert.Equal(1, orders.Count);
            Assert.Equal("001", orders.Single().OrderNumberComplete);
            Assert.Equal("Alex", orders.Single().ShipFirstName);
        }

        [Fact]
        public async Task Download_RunTwice_WithOrdersWithDifferentAmountOfLeadingZeros_TreatsOrdersAsTheSame()
        {
            odbcRecord = GetOdbcRecord("55", "Kevin");
            var testObject1 = mock.Create<OdbcStoreDownloader>(TypedParameter.From<StoreEntity>(store));
            await testObject1.Download(mockProgressReporter.Object, downloadLog, dbConnection);

            odbcRecord = GetOdbcRecord("0055", "Alex");
            var testObject2 = mock.Create<OdbcStoreDownloader>(TypedParameter.From<StoreEntity>(store));
            await testObject2.Download(mockProgressReporter.Object, downloadLog, dbConnection);

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