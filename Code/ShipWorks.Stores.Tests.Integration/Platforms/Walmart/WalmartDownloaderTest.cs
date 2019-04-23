﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;
using ShipWorks.Startup;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Stores.Platforms.Walmart.DTO;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Walmart
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Store", "Walmart")]
    public class WalmartDownloaderTest : IDisposable
    {
        private readonly DatabaseFixture db;
        private AutoMock mock;
        private DataContext context;
        private Mock<IProgressReporter> mockProgressReporter;
        private WalmartStoreEntity store;
        private ordersListType firstBatch;
        private DbConnection dbConnection;
        private WalmartDownloader testObject;
        private readonly DateTime utcNow;
        private long downloadLogID;

        public WalmartDownloaderTest(DatabaseFixture db)
        {
            this.db = db;
            utcNow = DateTime.UtcNow;
        }

        [Fact]
        public async Task Download_SetsProgressToNoOrdersToDownload()
        {
            SetupContextWithFakeWebClient();

            await testObject.Download(mockProgressReporter.Object, 0, dbConnection);

            mockProgressReporter.VerifySet(reporter => reporter.Detail = "No orders to download.", Times.Once);
        }

        [Fact]
        public async Task Download_RequestsOrdersFromDaysBackSpecifiedByDownloadModifiedNumberOfDaysBack()
        {
            SetupContextWithFakeWebClient();
            store.DownloadModifiedNumberOfDaysBack = 5;
            mock.Mock<IDateTimeProvider>()
                .SetupGet(d => d.UtcNow)
                .Returns(utcNow);

            await testObject.Download(mockProgressReporter.Object, 0, dbConnection);

            mock.Mock<IWalmartWebClient>()
                .Verify(client => client.GetOrders(store, utcNow.AddDays(-5)), Times.Once);
        }

        [Fact]
        public async Task Download_SavesSingleOrder_WhenSingleOrderInFirstBatch()
        {
            SetupContextWithFakeWebClient();

            firstBatch = CreateSingleOrderListType("5", "6", null);

            await testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            WalmartOrderEntity createdOrder;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                createdOrder = new LinqMetaData(adapter).WalmartOrder.SingleOrDefault();
            }

            Assert.NotNull(createdOrder);
            Assert.Equal("5", createdOrder.CustomerOrderID);
            Assert.Equal("6", createdOrder.PurchaseOrderID);
            Assert.Equal(6, createdOrder.OrderNumber);
            mock.Mock<IWalmartWebClient>().Verify(c => c.GetOrders(It.IsAny<WalmartStoreEntity>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Download_SavesOrdersFromMultipleBatches_WhenLinkToNextBatchProvided()
        {
            SetupContextWithFakeWebClient();

            string nextCursor = "http://www.shipworks.com/blah?x=42";
            firstBatch = CreateSingleOrderListType("5", "6", nextCursor);

            var secondBatch = CreateSingleOrderListType("42", "55", null);
            mock.Mock<IWalmartWebClient>()
                .Setup(c => c.GetOrders(store, nextCursor))
                .Returns(secondBatch);

            await testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            List<WalmartOrderEntity> createdOrders;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                createdOrders = new LinqMetaData(adapter).WalmartOrder.ToList();
            }

            Assert.Equal(2, createdOrders.Count);
            Assert.Equal(1, createdOrders.Count(o => o.OrderNumber == 6));
            Assert.Equal(1, createdOrders.Count(o => o.OrderNumber == 55));
        }

        [Fact]
        public async Task Download_SuccessfullyProcessesXml()
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x),
                autoMock =>
                {
                    autoMock.Override<IHttpRequestSubmitterFactory>();
                    autoMock.Override<IDateTimeProvider>();
                    autoMock.Override<Func<ApiLogSource, string, IApiLogEntry>>();
                });
            SetupCommon();

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.SetupSequence(r => r.ReadResult())
                .Returns("<OAuthTokenDTO xmlns=\"\"><accessToken>eyJraWQiOiIzN2JmOWQ5MS04ZDRkLTQwYjEtODU4NS1mNzhlZDc3MjM4MDQiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiZGlyIn0..yI0d0EFgt3gpEMQj.APvV5upgh1nBRcZPG6d9a5PYNAwayZdIZc4s-J42Ol-Zk6V9liVWmgs4qyPdkghfLKWwAyOBoc5g1vxRsYQNOeDH_p7KM_dO3D80g8adtGDkcAoEimyhP0inGHOjT2PziwIWjtRbE5bm8WTZN7wwJPV5SFqBP-XvS03OdPcZ2V_f0mVln5EdzaRt0BmpvlVDgqn_9Pe_jIYadWX-qc1N_Lh6oe3Q6bUKTJIhb6N-v8dNDO6qyRbeWo0_0M1s7g_mBKIp2FeO8a_ezIkpjtRv81SwXxEDqFacT2O46GlILO0Nfh7lbI2HZK2eUPrC-XVCiLiGoKsQDxHxD_Po0QrH0OxD6jnQvUfiqpgsULsvtbsSuHpUNl-OlIFhxODRxyIILsjAoWaQi9yEEV5swLVTdCix7e8ZKJPWQKEi55e7WYZm8vJqIUaaKrgdbw8HDlOWiJcrzzKdg-Hk3QBxnuoT4wqiaJaKb3uIQUtPzo5Jn_58PIxHh1WawpJOOmTH5RFgGTHlMB-5nBKG0iW1Bzm_vlg0NU_ZlYjvTWWDGOldk18WbzBr88XUI_jyRylFp_gYxc2peAyPnhZtOJGC4-7Eudbjz7QtClntVenjcG9h0k-xUejD0fcvCQUCty8S0ZfvCMEhrOJXEYCfI9-ESFFzpRh0EDCyfllS2Ugor4ZtUBjztXsj5sz2tp1wOSG7QR22K8rBKBFSyaKdIrNoLoucCEFFqlK_WSxfgsvUhspr4ZztxzPCejv3pKu9XYOV5nM6_qRsQY9ub88kQQKf2ZwNUnuYM_JYcvr2P9R551Pqqssk2KmbU42P35a2t5xJo9h921tGxUNjS9k4LAaO9g.Em_yWzxnUwwvs90IRZzH-Q</accessToken><tokenType>Bearer</tokenType><expiresIn>900</expiresIn></OAuthTokenDTO>")
                .Returns(
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                    "<ns3:list xmlns:ns2=\"http://walmart.com/mp/orders\" xmlns:ns3=\"http://walmart.com/mp/v3/orders\" xmlns:ns4=\"http://walmart.com/\">" +
                    "<ns3:meta><ns3:totalCount>177</ns3:totalCount><ns3:limit>200</ns3:limit></ns3:meta>" +
                    "<ns3:elements>" +
                    "<ns3:order>" +
                    "<ns3:purchaseOrderId>6</ns3:purchaseOrderId><ns3:customerOrderId>5</ns3:customerOrderId><ns3:customerEmailId>91DDE88D93C64028BE36D7771638A274@relay.walmart.com</ns3:customerEmailId><ns3:orderDate>2017-08-22T22:08:59.000Z</ns3:orderDate><ns3:shippingInfo><ns3:phone>6022843111</ns3:phone><ns3:estimatedDeliveryDate>2017-09-05T06:00:00.000Z</ns3:estimatedDeliveryDate><ns3:estimatedShipDate>2017-08-25T06:00:00.000Z</ns3:estimatedShipDate><ns3:methodCode>Value</ns3:methodCode><ns3:postalAddress><ns3:name>michele  gulli</ns3:name><ns3:address1>8818 N 47th Lane</ns3:address1><ns3:city>Glendale</ns3:city><ns3:state>AZ</ns3:state><ns3:postalCode>85302</ns3:postalCode><ns3:country>USA</ns3:country><ns3:addressType>RESIDENTIAL</ns3:addressType></ns3:postalAddress></ns3:shippingInfo><ns3:orderLines><ns3:orderLine><ns3:lineNumber>1</ns3:lineNumber><ns3:item><ns3:productName>Malaseb Shampoo 16 oz</ns3:productName><ns3:sku>00000125</ns3:sku></ns3:item><ns3:charges><ns3:charge><ns3:chargeType>PRODUCT</ns3:chargeType><ns3:chargeName>ItemPrice</ns3:chargeName><ns3:chargeAmount><ns3:currency>USD</ns3:currency><ns3:amount>31.48</ns3:amount></ns3:chargeAmount></ns3:charge><ns3:charge/></ns3:charges><ns3:orderLineQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:orderLineQuantity><ns3:statusDate>2017-08-23T13:04:39.000Z</ns3:statusDate><ns3:orderLineStatuses><ns3:orderLineStatus><ns3:status>Shipped</ns3:status><ns3:statusQuantity><ns3:unitOfMeasurement>EACH</ns3:unitOfMeasurement><ns3:amount>1</ns3:amount></ns3:statusQuantity><ns3:trackingInfo><ns3:shipDateTime>2017-08-23T13:04:40.000Z</ns3:shipDateTime><ns3:carrierName><ns3:carrier>USPS</ns3:carrier></ns3:carrierName><ns3:methodCode>Value</ns3:methodCode><ns3:trackingNumber>9405511899223042388290</ns3:trackingNumber><ns3:trackingURL>http://walmart.narvar.com/walmart/tracking/usps?&amp;type=MP&amp;seller_id=13699&amp;promise_date=09/05/2017&amp;dzip=85302&amp;tracking_numbers=9405511899223042388290</ns3:trackingURL></ns3:trackingInfo></ns3:orderLineStatus></ns3:orderLineStatuses></ns3:orderLine></ns3:orderLines>" +
                    "</ns3:order>" +
                    "</ns3:elements>" +
                    "</ns3:list>");

            responseReader.SetupGet(r => r.HttpWebResponse.StatusCode)
                .Returns(System.Net.HttpStatusCode.OK);

            var variableRequestSubmitter = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpVariableRequestSubmitter());

            variableRequestSubmitter.Setup(s => s.GetResponse()).Returns(responseReader.Object);
            
            store.DownloadModifiedNumberOfDaysBack = 5;
            mock.Mock<IDateTimeProvider>()
                .SetupGet(d => d.UtcNow)
                .Returns(utcNow);

            await testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);

            WalmartOrderEntity createdOrder;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                createdOrder = new LinqMetaData(adapter).WalmartOrder.SingleOrDefault();
            }

            Assert.NotNull(createdOrder);
            Assert.Equal("5", createdOrder.CustomerOrderID);
            Assert.Equal("6", createdOrder.PurchaseOrderID);
            Assert.Equal(6, createdOrder.OrderNumber);
        }

        private void SetupContextWithFakeWebClient()
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x),
                autoMock =>
                {
                    autoMock.Override<IWalmartWebClient>();
                    autoMock.Override<IDateTimeProvider>();
                });

            SetupCommon();

            mock.Mock<IWalmartWebClient>()
                .Setup(c => c.GetOrders(store, It.IsAny<DateTime>()))
                .Returns(() => firstBatch);

            firstBatch = new ordersListType
            {
                meta = new metaType
                {
                    totalCount = 0
                }
            };
        }

        private void SetupCommon()
        {
            mock = context.Mock;
            mockProgressReporter = mock.Mock<IProgressReporter>();

            store = Create.Store<WalmartStoreEntity>()
                .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                .Set(x => x.StoreName, "Walmart Store")
                .Set(x => x.StoreTypeCode = StoreTypeCode.Walmart)
                .Set(x => x.ClientID = "ClientID")
                .Set(x => x.ClientSecret = "ClientSecret")
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

            dbConnection = SqlSession.Current.OpenConnection();
            testObject = mock.Create<WalmartDownloader>(new TypedParameter(typeof(StoreEntity), store));
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
            mock?.Dispose();
            context?.Dispose();
            dbConnection?.Dispose();
        }
    }
}