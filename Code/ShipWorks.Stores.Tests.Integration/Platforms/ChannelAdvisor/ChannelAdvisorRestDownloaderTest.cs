﻿using System;
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
                .Set(x => x.RefreshToken = "refreshToken")
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
                .Set(x => x.InitiatedBy = (int)DownloadInitiatedBy.User)
                .Set(x => x.Started = utcNow)
                .Set(x => x.Ended = null)
                .Set(x => x.Result = (int)DownloadResult.Unfinished)
                .Save().DownloadID;
            
            client = mock.Mock<IChannelAdvisorRestClient>();
            client.Setup(c => c.GetOrders(It.Is<DateTime>(x => x < DateTime.UtcNow.AddDays(-29)), It.IsAny<string>()))
                .Returns(() => firstBatch);

            client.Setup(c => c.GetProduct(It.IsAny<int>(), It.IsAny<string>())).Returns(SingleItem());

            order = SingleOrder();
            firstBatch = new ChannelAdvisorOrderResult()
            {
                OdataContext = "",
                OdataNextLink = "",
                ResultCount = 1,
                Orders = new List<ChannelAdvisorOrder>() {order}
            };

            this.dbConnection = SqlSession.Current.OpenConnection();
            testObject = mock.Create<ChannelAdvisorRestDownloader>(new TypedParameter(typeof(StoreEntity), store));
        }

        [Fact]
        public void Download_GetsAccessTokenFromClient()
        {
            testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);
            client.Verify(c => c.GetAccessToken(It.IsAny<string>()));
        }

        [Fact]
        public void Download_GetsOrdersFromClientUsingCreatedDateFromPreviousGetOrdersResponse()
        {
            testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            client.Verify(c => c.GetOrders(order.CreatedDateUtc, It.IsAny<string>()));
        }

        [Fact]
        public void Download_SetsOrderNotes_WhenOrderContainsNotes()
        {
            testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            NoteEntity note;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                note = new LinqMetaData(adapter).Note.SingleOrDefault();
            }

            Assert.Equal("This is a public note", note.Text);
        }

        private ChannelAdvisorOrder SingleOrder()
        {
            return new ChannelAdvisorOrder()
            {
                Fulfillments = new List<ChannelAdvisorFulfillment>(),
                Items = new List<ChannelAdvisorOrderItem>()
                {
                    new ChannelAdvisorOrderItem()
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