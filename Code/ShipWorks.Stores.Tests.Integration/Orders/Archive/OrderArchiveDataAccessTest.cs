using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Startup;
using ShipWorks.Stores.Orders.Archive;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Orders.Archive
{
    [Collection("Database collection")]
    [Trait("Category", "SmokeTest")]
    public class OrderArchiveDataAccessTest
    {
        private readonly DataContext context;
        private Mock<IAsyncMessageHelper> asyncMessageHelper;
        private IOrderArchiveDataAccess testObject;
        private ISqlAdapterFactory sqlAdapterFactory;

        public OrderArchiveDataAccessTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IMessageHelper>();
                asyncMessageHelper = mock.Override<IAsyncMessageHelper>();
                sqlAdapterFactory = mock.Create<ISqlAdapterFactory>();
            });


            asyncMessageHelper.Setup(x => x.ShowProgressDialog(AnyString, AnyString, It.IsAny<IProgressProvider>(), TimeSpan.Zero))
                .ReturnsAsync(context.Mock.Build<ISingleItemProgressDialog>());

            testObject = context.Mock.Create<IOrderArchiveDataAccess>();
        }

        [Fact]
        public void EnableArchiveTriggersSql_EnablesTriggers()
        {
            using (DbConnection conn = new SqlConnection(SqlSession.Current.Configuration.GetConnectionString()))
            {
                testObject.DisableArchiveTriggers(conn);
                testObject.EnableArchiveTriggers(conn);

                // Run it a second time to make sure that the enabling sql is re-runable.
                testObject.EnableArchiveTriggers(conn);

                string triggerNames = string.Join("_EnforceReadonly', '", ReadonlyTableNames) + "_EnforceReadonly";
                string sql = $"SELECT COUNT(*) FROM sys.triggers WHERE [name] IN ('{triggerNames}')";
                int actualCount = 0;

                using (ISqlAdapter adapter = new SqlAdapter(conn))
                {
                    actualCount = (int) adapter.ExecuteScalarQuery(new RetrievalQuery(new SqlCommand(sql)));
                }

                Assert.Equal(ReadonlyTableNames.Count(), actualCount);

                testObject.DisableArchiveTriggers(conn);
            }
        }

        [Fact]
        public void DisableArchiveTriggersSql_EnablesTriggers()
        {
            int actualCount = 0;
            using (DbConnection conn = new SqlConnection(SqlSession.Current.Configuration.GetConnectionString()))
            {
                testObject.EnableArchiveTriggers(conn);
                testObject.DisableArchiveTriggers(conn);

                // Run it a second time to make sure that the disabling sql is re-runable.
                testObject.DisableArchiveTriggers(conn);

                string triggerNames = string.Join("_EnforceReadonly', '", ReadonlyTableNames) + "_EnforceReadonly";
                string sql = $"SELECT COUNT(*) FROM sys.triggers WHERE [name] IN ('{triggerNames}')";

                using (ISqlAdapter adapter = new SqlAdapter(conn))
                {
                    actualCount = (int) adapter.ExecuteScalarQuery(new RetrievalQuery(new SqlCommand(sql)));
                }
            }

            Assert.Equal(0, actualCount);
        }

        /// <summary>
        /// Tables that should be treated as "read only" in archive databases
        /// </summary>
        public static IEnumerable<string> ReadonlyTableNames => new[]
            {
                "Action",
                "ActionFilterTrigger",
                "ActionQueue",
                "ActionQueueSelection",
                "ActionQueueStep",
                "ActionTask",
                "AmazonASIN",
                "AmazonOrder",
                "AmazonOrderItem",
                "AmazonOrderSearch",
                "AmazonProfile",
                "AmazonServiceType",
                "AmazonShipment",
                "AmazonStore",
                "AmeriCommerceStore",
                "AsendiaAccount",
                "AsendiaProfile",
                "AsendiaShipment",
                "BestRateProfile",
                "BestRateShipment",
                "BigCommerceOrderItem",
                "BigCommerceStore",
                "BuyDotComOrderItem",
                "BuyDotComStore",
                "ChannelAdvisorOrder",
                "ChannelAdvisorOrderItem",
                "ChannelAdvisorOrderSearch",
                "ChannelAdvisorStore",
                "ClickCartProOrder",
                "ClickCartProOrderSearch",
                "CommerceInterfaceOrder",
                "CommerceInterfaceOrderSearch",
                "Configuration",
                "Customer",
                "DhlExpressAccount",
                "DhlExpressPackage",
                "DhlExpressProfile",
                "DhlExpressProfilePackage",
                "DhlExpressShipment",
                "DimensionsProfile",
                "Download",
                "DownloadDetail",
                "EbayCombinedOrderRelation",
                "EbayOrder",
                "EbayOrderItem",
                "EbayOrderSearch",
                "EbayStore",
                "EmailAccount",
                "EmailOutbound",
                "EmailOutboundRelation",
                "EndiciaAccount",
                "EndiciaProfile",
                "EndiciaScanForm",
                "EndiciaShipment",
                "EtsyOrder",
                "EtsyOrderItem",
                "EtsyStore",
                "ExcludedPackageType",
                "ExcludedServiceType",
                "FedExAccount",
                "FedExEndOfDayClose",
                "FedExPackage",
                "FedExProfile",
                "FedExProfilePackage",
                "FedExShipment",
                "FtpAccount",
                "GenericFileStore",
                "GenericModuleOrder",
                "GenericModuleOrderItem",
                "GenericModuleStore",
                "GrouponOrder",
                "GrouponOrderItem",
                "GrouponOrderSearch",
                "GrouponStore",
                "InfopiaOrderItem",
                "InfopiaStore",
                "InsurancePolicy",
                "iParcelAccount",
                "iParcelPackage",
                "iParcelProfile",
                "iParcelProfilePackage",
                "iParcelShipment",
                "JetOrder",
                "JetOrderItem",
                "JetOrderSearch",
                "JetStore",
                "LabelSheet",
                "LemonStandOrder",
                "LemonStandOrderItem",
                "LemonStandOrderSearch",
                "LemonStandStore",
                "MagentoOrder",
                "MagentoOrderSearch",
                "MagentoStore",
                "MarketplaceAdvisorOrder",
                "MarketplaceAdvisorOrderSearch",
                "MarketplaceAdvisorStore",
                "MivaOrderItemAttribute",
                "MivaStore",
                "NetworkSolutionsOrder",
                "NetworkSolutionsOrderSearch",
                "NetworkSolutionsStore",
                "NeweggOrder",
                "NeweggOrderItem",
                "NeweggStore",
                "Note",
                "OdbcStore",
                "OnTracAccount",
                "OnTracProfile",
                "OnTracShipment",
                "Order",
                "OrderCharge",
                "OrderItem",
                "OrderItemAttribute",
                "OrderMotionOrder",
                "OrderMotionOrderSearch",
                "OrderMotionStore",
                "OrderPaymentDetail",
                "OrderSearch",
                "OtherProfile",
                "OtherShipment",
                "PayPalOrder",
                "PayPalOrderSearch",
                "PayPalStore",
                "PostalProfile",
                "PostalShipment",
                "ProStoresOrder",
                "ProStoresOrderSearch",
                "ProStoresStore",
                "ScanFormBatch",
                "Scheduling_BLOB_TRIGGERS",
                "Scheduling_CALENDARS",
                "Scheduling_CRON_TRIGGERS",
                "Scheduling_FIRED_TRIGGERS",
                "Scheduling_JOB_DETAILS",
                "Scheduling_LOCKS",
                "Scheduling_PAUSED_TRIGGER_GRPS",
                "Scheduling_SCHEDULER_STATE",
                "Scheduling_SIMPLE_TRIGGERS",
                "Scheduling_SIMPROP_TRIGGERS",
                "Scheduling_TRIGGERS",
                "SearsOrder",
                "SearsOrderItem",
                "SearsOrderSearch",
                "SearsStore",
                "Shipment",
                "ShipmentCustomsItem",
                "ShipmentReturnItem",
                "ShippingDefaultsRule",
                "ShippingOrigin",
                "ShippingPrintOutputRule",
                "ShippingProfile",
                "ShippingProviderRule",
                "ShipSenseKnowledgeBase",
                "ShopifyOrder",
                "ShopifyOrderItem",
                "ShopifyOrderSearch",
                "ShopifyStore",
                "ShopSiteStore",
                "SparkPayStore",
                "StatusPreset",
                "Store",
                "ThreeDCartOrder",
                "ThreeDCartOrderItem",
                "ThreeDCartOrderSearch",
                "ThreeDCartStore",
                "UpsAccount",
                "UpsLetterRate",
                "UpsLocalRatingDeliveryAreaSurcharge",
                "UpsLocalRatingZone",
                "UpsLocalRatingZoneFile",
                "UpsPackage",
                "UpsPackageRate",
                "UpsPricePerPound",
                "UpsProfile",
                "UpsProfilePackage",
                "UpsRateSurcharge",
                "UpsRateTable",
                "UpsShipment",
                "UspsAccount",
                "UspsProfile",
                "UspsScanForm",
                "UspsShipment",
                "ValidatedAddress",
                "VolusionStore",
                "WalmartOrder",
                "WalmartOrderItem",
                "WalmartOrderSearch",
                "WalmartStore",
                "WorldShipGoods",
                "WorldShipPackage",
                "WorldShipShipment",
                "YahooOrder",
                "YahooOrderItem",
                "YahooOrderSearch",
                "YahooProduct",
                "YahooStore",
            };
    }
}
