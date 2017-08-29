using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;
using ShipWorks.Startup;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.ChannelAdvisor
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Store", "ChannelAdvisor")]
    public class ChannelAdvisorRestDownloaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DataContext context;
        private readonly Mock<IProgressReporter> mockProgressReporter;
        private readonly ChannelAdvisorOrderResult firstBatch;
        private readonly DbConnection dbConnection;
        private readonly ChannelAdvisorRestDownloader testObject;
        private readonly DateTime utcNow;
        private readonly long downloadLogID;
        private readonly Mock<IChannelAdvisorRestClient> client;
        private readonly ChannelAdvisorOrder order;

        public ChannelAdvisorRestDownloaderTest(DatabaseFixture db)
        {
            utcNow = DateTime.UtcNow;

            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x),
                mock =>
                {
                    mock.Override<IChannelAdvisorRestClient>();
                });

            mock = context.Mock;
            mockProgressReporter = mock.Mock<IProgressReporter>();

            var store = Create.Store<ChannelAdvisorStoreEntity>()
                .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                .Set(x => x.StoreName, "Channel Store")
                .Set(x => x.StoreTypeCode = StoreTypeCode.ChannelAdvisor)
                // Encrypted "refreshToken"
                .Set(x => x.RefreshToken = "717TxeCurhOsOo6942NICQ==")
                .Set(x => x.AttributesToDownload = "<Attributes></Attributes>")
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

            var distributionCenters = new List<ChannelAdvisorDistributionCenter>()
            {
                new ChannelAdvisorDistributionCenter()
                {
                    ID = 1,
                    Code = "SW"
                }
            };

            client = mock.Mock<IChannelAdvisorRestClient>();
            client.Setup(c => c.GetOrders(It.Is<DateTime>(x => x < DateTime.UtcNow.AddDays(-29)), It.IsAny<string>()))
                .Returns(() => firstBatch);
            client.Setup(c => c.GetDistributionCenters(It.IsAny<string>()))
                .Returns(new ChannelAdvisorDistributionCenterResponse() { DistributionCenters = distributionCenters });

            client.Setup(c => c.GetProduct(It.IsAny<int>(), It.IsAny<string>())).Returns(SingleItem());

            order = SingleOrder();
            firstBatch = new ChannelAdvisorOrderResult()
            {
                OdataContext = "",
                OdataNextLink = "",
                ResultCount = 1,
                Orders = new List<ChannelAdvisorOrder>() { order }
            };

            dbConnection = SqlSession.Current.OpenConnection();
            testObject = mock.Create<ChannelAdvisorRestDownloader>(TypedParameter.From<StoreEntity>(store));
        }

        [Fact]
        public async Task Download_GetDistributionCenters()
        {
            await testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);
            client.Verify(c => c.GetDistributionCenters("refreshToken"), Times.Once);
        }

        [Fact]
        public async Task Download_SetsProgressDetailWithOrderCount()
        {
            await testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);
            mockProgressReporter.VerifySet(r => r.Detail = $"Downloading {firstBatch.ResultCount} orders...");
        }

        [Fact]
        public async Task Download_SetsOrderNotes_WhenOrderContainsNotes()
        {
            await testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            NoteEntity note;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                note = new LinqMetaData(adapter).Note.SingleOrDefault();
            }

            Assert.Equal("This is a public note", note.Text);
        }

        [Fact]
        public async Task Download_SetsDistributionCode()
        {
            await testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            ChannelAdvisorOrderItemEntity item;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                item = new LinqMetaData(adapter).ChannelAdvisorOrderItem.SingleOrDefault();
            }

            Assert.Equal("SW", item.DistributionCenter);
        }

        private ChannelAdvisorOrder SingleOrder()
        {
            return new ChannelAdvisorOrder()
            {
                Fulfillments = new List<ChannelAdvisorFulfillment>()
                {
                    new ChannelAdvisorFulfillment()
                    {
                        ID = 123,
                        DistributionCenterID = 1
                    }
                },
                Items = new List<ChannelAdvisorOrderItem>()
                {
                    new ChannelAdvisorOrderItem()
                    {
                        FulfillmentItems = new List<ChannelAdvisorFulfillmentItem>()
                        {
                            new ChannelAdvisorFulfillmentItem()
                            {
                                FulfillmentID = 123
                            }
                        }
                    }
                },
                CreatedDateUtc = utcNow.AddDays(5),
                PublicNotes = "This is a public note"
            };
        }

        private ChannelAdvisorProduct SingleItem()
        {
            return new ChannelAdvisorProduct()
            {
                Attributes = new List<ChannelAdvisorProductAttribute>(),
                Weight = 123,
                WarehouseLocation = "location",
                Classification = "classification",
                Cost = 123,
                HarmonizedCode = "harmonizedcode",
                ISBN = "isbn",
                UPC = "upc",
                MPN = "mpn",
                Description = "description",
                Images = new List<ChannelAdvisorProductImage>()
            };
        }

        public void Dispose()
        {
            mock?.Dispose();
            context?.Dispose();
            dbConnection?.Dispose();
        }
    }
}