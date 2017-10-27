using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Enums;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Stores.Platforms.Amazon.OnlineUpdating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using Xunit.Abstractions;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using System.Data.Common;
using ShipWorks.Stores.Communication;
using Interapptive.Shared.Utility;
using System.Xml;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Amazon
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Store", "Amazon")]
    public class AmazonMwsDownloaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DataContext context;
        private readonly Mock<IProgressReporter> mockProgressReporter;
        private readonly DbConnection dbConnection;
        private readonly AmazonMwsDownloader testObject;
        private readonly long downloadLogID;
        Mock<IAmazonMwsClient> client;
        private readonly DateTime utcNow;

        public AmazonMwsDownloaderTest(DatabaseFixture db, ITestOutputHelper output)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x),
                mock =>
                {
                    mock.Override<IAmazonMwsClient>();
                    mock.Override<IDownloadStartingPoint>();
                });

            //context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            utcNow = DateTime.UtcNow;

            mock = context.Mock;

            mockProgressReporter = mock.Mock<IProgressReporter>();

            var store = Create.Store<AmazonStoreEntity>()
                .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                .Set(x => x.StoreName, "Amazon Store")
                .Set(x => x.StoreTypeCode = StoreTypeCode.Amazon)
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

            string responseXml = "<?xml version=\"1.0\"?><ListOrdersResponse xmlns=\"https://mws.amazonservices.com/Orders/2013-09-01\"><ListOrdersResult><Orders><Order><LatestShipDate>2017-10-17T06:59:59Z</LatestShipDate><OrderType>StandardOrder</OrderType><PurchaseDate>2017-10-13T20:46:54Z</PurchaseDate><AmazonOrderId>114-7693151-3018628</AmazonOrderId><BuyerEmail>rxfw7zgwk8b9z3t@marketplace.amazon.com</BuyerEmail><IsReplacementOrder>false</IsReplacementOrder><LastUpdateDate>2017-10-15T21:12:34Z</LastUpdateDate><NumberOfItemsShipped>1</NumberOfItemsShipped><ShipServiceLevel>Std US D2D Dom</ShipServiceLevel><OrderStatus>Shipped</OrderStatus><SalesChannel>Amazon.com</SalesChannel><ShippedByAmazonTFM>false</ShippedByAmazonTFM><IsBusinessOrder>false</IsBusinessOrder><LatestDeliveryDate>2017-10-24T06:59:59Z</LatestDeliveryDate><NumberOfItemsUnshipped>0</NumberOfItemsUnshipped><PaymentMethodDetails><PaymentMethodDetail>Standard</PaymentMethodDetail></PaymentMethodDetails><BuyerName>brenda</BuyerName><EarliestDeliveryDate>2017-10-19T07:00:00Z</EarliestDeliveryDate><OrderTotal><CurrencyCode>USD</CurrencyCode><Amount>0</Amount></OrderTotal><IsPremiumOrder>false</IsPremiumOrder><EarliestShipDate>2017-10-16T07:00:00Z</EarliestShipDate><MarketplaceId>ATVPDKIKX0DER</MarketplaceId><FulfillmentChannel>MFN</FulfillmentChannel><PaymentMethod>Other</PaymentMethod><ShippingAddress><StateOrRegion>ALABAMA</StateOrRegion><City>PAINT ROCK</City><Phone>256-776-9350</Phone><CountryCode>US</CountryCode><PostalCode>35764</PostalCode><Name>BRENDA FISK</Name><AddressLine1>197 TIPTON STREET</AddressLine1></ShippingAddress><IsPrime>false</IsPrime><ShipmentServiceLevelCategory>Standard</ShipmentServiceLevelCategory></Order><Order><LatestShipDate>2017-10-19T06:59:59Z</LatestShipDate><OrderType>StandardOrder</OrderType><PurchaseDate>2017-10-16T18:43:24Z</PurchaseDate><AmazonOrderId>114-4193977-1105043</AmazonOrderId><BuyerEmail>cr4yqs4qhz7w8dc@marketplace.amazon.com</BuyerEmail><IsReplacementOrder>false</IsReplacementOrder><LastUpdateDate>2017-10-16T20:49:38Z</LastUpdateDate><NumberOfItemsShipped>1</NumberOfItemsShipped><ShipServiceLevel>Std US D2D Dom</ShipServiceLevel><OrderStatus>Shipped</OrderStatus><SalesChannel>Amazon.com</SalesChannel><ShippedByAmazonTFM>false</ShippedByAmazonTFM><IsBusinessOrder>false</IsBusinessOrder><LatestDeliveryDate>2017-10-26T06:59:59Z</LatestDeliveryDate><NumberOfItemsUnshipped>0</NumberOfItemsUnshipped><PaymentMethodDetails><PaymentMethodDetail>Standard</PaymentMethodDetail></PaymentMethodDetails><BuyerName>Brenda</BuyerName><EarliestDeliveryDate>2017-10-20T07:00:00Z</EarliestDeliveryDate><OrderTotal><CurrencyCode>USD</CurrencyCode><Amount>0</Amount></OrderTotal><IsPremiumOrder>false</IsPremiumOrder><EarliestShipDate>2017-10-17T07:00:00Z</EarliestShipDate><MarketplaceId>ATVPDKIKX0DER</MarketplaceId><FulfillmentChannel>MFN</FulfillmentChannel><PaymentMethod>Other</PaymentMethod><ShippingAddress><StateOrRegion>TX</StateOrRegion><City>GRANBURY</City><Phone>817-559-3164</Phone><CountryCode>US</CountryCode><PostalCode>76048-3216</PostalCode><Name>Brenda P Harris</Name><AddressLine1>5508 ARROWHEAD DR</AddressLine1></ShippingAddress><IsPrime>false</IsPrime><ShipmentServiceLevelCategory>Standard</ShipmentServiceLevelCategory></Order></Orders><LastUpdatedBefore>2017-10-26T19:06:31Z</LastUpdatedBefore></ListOrdersResult><ResponseMetadata><RequestId>39e0e42f-b9b7-470c-8588-52e6a0455c1e</RequestId></ResponseMetadata></ListOrdersResponse>";
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(responseXml);

            // pre-load the appropriate namespace for xpath querying using the prefix "amz:"
            XPathNamespaceNavigator xpath = new XPathNamespaceNavigator(xmlDocument);
            xpath.Namespaces.AddNamespace("amz", "https://mws.amazonservices.com/Orders/2013-09-01");

            client = mock.CreateMock<IAmazonMwsClient>();
            client.Setup(c => c.ClockInSyncWithMWS()).ReturnsAsync(true);
            client.Setup(c => c.TestServiceStatus()).Returns(Task.CompletedTask);
            client.Setup(c => c.GetOrders(It.IsAny<DateTime>(), It.IsAny<Func<XPathNamespaceNavigator, Task<bool>>>()))
                .Callback<DateTime?, Func<XPathNamespaceNavigator, Task<bool>>>((d, func) =>
                {
                    func(xpath).Wait();
                })
                .Returns(Task.CompletedTask);                

            mock.MockFunc<AmazonStoreEntity, IAmazonMwsClient>(client);

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

            mock.Mock<IDownloadStartingPoint>()
                .Setup(p => p.OnlineLastModified(It.IsAny<IStoreEntity>()))
                .ReturnsAsync(utcNow);

            testObject = mock.Create<AmazonMwsDownloader>(TypedParameter.From<StoreEntity>(store));
        }

        [Fact]
        public async Task CreatesMultipleCustomersWhenBuyerNameMatches()
        {
            await testObject.Download(mockProgressReporter.Object, downloadLogID, dbConnection);
            
            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                var query = new QueryFactory().Customer;

                var customers = await sqlAdapter.FetchQueryAsync(query);
                int brendas = customers.Cast<ICustomerEntity>().Where(c => c.BillFirstName.ToLower() == "brenda").Count();
                Assert.Equal(2, brendas);
            }
        }
      
        public void Dispose() => context.Dispose();
    }
}