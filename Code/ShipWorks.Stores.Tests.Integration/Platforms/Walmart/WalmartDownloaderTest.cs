using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;
using ShipWorks.Startup;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Stores.Platforms.Walmart.DTO;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Walmart
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class WalmartDownloaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DataContext context;
        private readonly Mock<IProgressReporter> mockProgressReporter;
        private readonly WalmartStoreEntity store;
        private ordersListType firstBatch;
        private readonly DbConnection dbConnection;
        private readonly WalmartDownloader testObject;
        private readonly DateTime utcNow;
        private readonly long downloadLogID;

        public WalmartDownloaderTest(DatabaseFixture db)
        {
            utcNow = DateTime.UtcNow;

            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x),
                mock =>
                {
                    mock.Override<IWalmartRequestSigner>();
                    mock.Override<IWalmartWebClient>();
                    mock.Override<IDateTimeProvider>();
                });
            
            mock = context.Mock;
            mockProgressReporter = mock.Mock<IProgressReporter>();

            store = Create.Store<WalmartStoreEntity>()
                           .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                           .Set(x => x.StoreName, "Walmart Store")
                           .Set(x=>x.StoreTypeCode = StoreTypeCode.Walmart)
                           .Save();

            Create.Entity<StatusPresetEntity>()
                .Set(x => x.StoreID = store.StoreID)
                .Set(x => x.StatusTarget = 0)
                .Set(x=>x.StatusText = "status")
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
            
            mock.Mock<IWalmartWebClient>()
                .Setup(c => c.GetOrders(store, It.IsAny<DateTime>()))
                .Returns(() => firstBatch);

            firstBatch = new ordersListType()
            {
                meta = new metaType()
                {
                    totalCount = 0
                }
            };

            this.dbConnection = SqlSession.Current.OpenConnection();
            testObject = mock.Create<WalmartDownloader>(new TypedParameter(typeof(StoreEntity), store));
        }

        [Fact]
        public void Download_SetsProgressToNoOrdersToDownload()
        {
            testObject.Download(mockProgressReporter.Object, 0, dbConnection);

            mockProgressReporter.VerifySet(reporter=>reporter.Detail = "No orders to download.", Times.Once);
        }

        [Fact]
        public void Download_RequestsOrdersFromDaysBackSpecifiedByDownloadModifiedNumberOfDaysBack()
        {
            store.DownloadModifiedNumberOfDaysBack = 5;
            mock.Mock<IDateTimeProvider>()
                .SetupGet(d => d.UtcNow)
                .Returns(utcNow);

            testObject.Download(mockProgressReporter.Object, 0, dbConnection);
            
            mock.Mock<IWalmartWebClient>()
                .Verify(client=>client.GetOrders(store, utcNow.AddDays(-5)), Times.Once);
        }

        [Fact]
        public void Download_SavesSingleOrder_WhenSingleOrderInFirstBatch()
        {
            firstBatch = CreateSingleOrderListType("5", "6", null);

            testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            WalmartOrderEntity createdOrder;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                createdOrder = new LinqMetaData(adapter).WalmartOrder.SingleOrDefault();
            }

            Assert.NotNull(createdOrder);
            Assert.Equal("5", createdOrder.CustomerOrderID);
            Assert.Equal("6", createdOrder.PurchaseOrderID);
            Assert.Equal(5, createdOrder.OrderNumber);
            mock.Mock<IWalmartWebClient>().Verify(c=>c.GetOrders(It.IsAny<WalmartStoreEntity>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Download_SavesOrdersFromMultipleBatches_WhenLinkToNextBatchProvided()
        {
            string nextCursor = "http://www.shipworks.com/blah?x=42";
            firstBatch = CreateSingleOrderListType("5", "6", nextCursor);

            var secondBatch = CreateSingleOrderListType("42", "55", null);
            mock.Mock<IWalmartWebClient>()
                .Setup(c => c.GetOrders(store, nextCursor))
                .Returns(secondBatch);

            testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            List<WalmartOrderEntity> createdOrders;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                createdOrders = new LinqMetaData(adapter).WalmartOrder.ToList();
            }

            Assert.Equal(2, createdOrders.Count);
            Assert.Equal(1, createdOrders.Count(o => o.OrderNumber == 5));
            Assert.Equal(1, createdOrders.Count(o => o.OrderNumber == 42));
        }

        private static ordersListType CreateSingleOrderListType(string customerOrderId, string purchaseOrderId, string nextCursor)
        {
            return new ordersListType()
            {
                meta = new metaType()
                {
                    totalCount = 1,
                    totalCountSpecified = true,
                    nextCursor = nextCursor
                },
                elements = new[]
                {
                    new Order()
                    {
                        customerOrderId = customerOrderId,
                        purchaseOrderId = purchaseOrderId,

                        customerEmailId = "bob@shipworks.com",
                        orderDate = new DateTime(2017, 3, 9, 16, 20, 1, DateTimeKind.Utc),
                        shippingInfo = new shippingInfoType()
                        {
                            estimatedDeliveryDate = new DateTime(2017, 3, 11),
                            estimatedShipDate = new DateTime(2017, 3, 10),
                            methodCode = shippingMethodCodeType.WhiteGlove,
                            phone = "3145551212",
                            postalAddress = new postalAddressType()
                            {
                                name = "Homer J. Simpson",
                                address1 = "1 S Memorial Dr",
                                address2 = "Suite 2000",
                                city = "St Louis",
                                state = "MO",
                                postalCode = "63110",
                                country = "USA"
                            }
                        },
                        orderLines = new[]
                        {
                            new orderLineType()
                            {
                                lineNumber = "42",
                                item = new itemType()
                                {
                                    productName = "26&amp;#8221; Doll Making Tool",
                                    sku = "DollTool"
                                },
                                charges = new[]
                                {
                                    new chargeType()
                                    {
                                        chargeType1 = "PRODUCT",
                                        chargeName = "ItemPrice",
                                        chargeAmount = new moneyType()
                                        {
                                            amount = 3.50M,
                                            currency = currencyType.USD
                                        },
                                        tax = new taxType()
                                        {
                                            taxName = "Tax1",
                                            taxAmount = new moneyType()
                                            {
                                                amount = .75M,
                                                currency = currencyType.USD
                                            }
                                        }
                                    }
                                },
                                orderLineQuantity = new quantityType()
                                {
                                    unitOfMeasurement = "EACH",
                                    amount = "1"
                                },
                                statusDate = new DateTime(2017, 3, 11, 16, 20, 0, DateTimeKind.Utc),
                                orderLineStatuses = new[]
                                {
                                    new orderLineStatusType()
                                    {
                                        status = orderLineStatusValueType.Acknowledged,
                                        statusQuantity = new quantityType()
                                        {
                                            amount = "1",
                                            unitOfMeasurement = "EACH"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        public void Dispose()
        {
            dbConnection.Dispose();
            context.Dispose();
        }
    }
}