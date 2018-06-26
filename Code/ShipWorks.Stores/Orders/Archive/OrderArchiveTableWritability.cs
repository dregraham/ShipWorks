﻿using System.Collections.Generic;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Class that defines tables that should be treated as "readonly" in archive databases
    /// </summary>
    public static class OrderArchiveTableWritability
    {
        /// <summary>
        /// Tables that should be treated as "readonly" in archive databases
        /// </summary>
        public static IEnumerable<string> ReadonlyTableNames => new[]
            {
                "'Action'",
                "'ActionFilterTrigger'",
                "'ActionQueue'",
                "'ActionQueueSelection'",
                "'ActionQueueStep'",
                "'ActionTask'",
                "'AmazonASIN'",
                "'AmazonOrder'",
                "'AmazonOrderItem'",
                "'AmazonOrderSearch'",
                "'AmazonProfile'",
                "'AmazonServiceType'",
                "'AmazonShipment'",
                "'AmazonStore'",
                "'AmeriCommerceStore'",
                "'AsendiaAccount'",
                "'AsendiaProfile'",
                "'AsendiaShipment'",
                "'BestRateProfile'",
                "'BestRateShipment'",
                "'BigCommerceOrderItem'",
                "'BigCommerceStore'",
                "'BuyDotComOrderItem'",
                "'BuyDotComStore'",
                "'ChannelAdvisorOrder'",
                "'ChannelAdvisorOrderItem'",
                "'ChannelAdvisorOrderSearch'",
                "'ChannelAdvisorStore'",
                "'ClickCartProOrder'",
                "'ClickCartProOrderSearch'",
                "'CommerceInterfaceOrder'",
                "'CommerceInterfaceOrderSearch'",
                "'Customer'",
                "'DhlExpressAccount'",
                "'DhlExpressPackage'",
                "'DhlExpressProfile'",
                "'DhlExpressShipment'",
                "'DimensionsProfile'",
                "'Download'",
                "'DownloadDetail'",
                "'EbayCombinedOrderRelation'",
                "'EbayOrder'",
                "'EbayOrderItem'",
                "'EbayOrderSearch'",
                "'EbayStore'",
                "'EmailAccount'",
                "'EmailOutbound'",
                "'EmailOutboundRelation'",
                "'EndiciaAccount'",
                "'EndiciaProfile'",
                "'EndiciaScanForm'",
                "'EndiciaShipment'",
                "'EtsyOrder'",
                "'EtsyOrderItem'",
                "'EtsyStore'",
                "'ExcludedPackageType'",
                "'ExcludedServiceType'",
                "'FedExAccount'",
                "'FedExEndOfDayClose'",
                "'FedExPackage'",
                "'FedExProfile'",
                "'FedExProfilePackage'",
                "'FedExShipment'",
                "'FtpAccount'",
                "'GenericFileStore'",
                "'GenericModuleOrder'",
                "'GenericModuleOrderItem'",
                "'GenericModuleStore'",
                "'GrouponOrder'",
                "'GrouponOrderItem'",
                "'GrouponOrderSearch'",
                "'GrouponStore'",
                "'InfopiaOrderItem'",
                "'InfopiaStore'",
                "'InsurancePolicy'",
                "'iParcelAccount'",
                "'iParcelPackage'",
                "'iParcelProfile'",
                "'iParcelShipment'",
                "'JetOrder'",
                "'JetOrderItem'",
                "'JetOrderSearch'",
                "'JetStore'",
                "'LabelSheet'",
                "'LemonStandOrder'",
                "'LemonStandOrderItem'",
                "'LemonStandOrderSearch'",
                "'LemonStandStore'",
                "'MagentoOrder'",
                "'MagentoOrderSearch'",
                "'MagentoStore'",
                "'MarketplaceAdvisorOrder'",
                "'MarketplaceAdvisorOrderSearch'",
                "'MarketplaceAdvisorStore'",
                "'MivaOrderItemAttribute'",
                "'MivaStore'",
                "'NetworkSolutionsOrder'",
                "'NetworkSolutionsOrderSearch'",
                "'NetworkSolutionsStore'",
                "'NeweggOrder'",
                "'NeweggOrderItem'",
                "'NeweggStore'",
                "'Note'",
                "'OdbcStore'",
                "'OnTracAccount'",
                "'OnTracProfile'",
                "'OnTracShipment'",
                "'Order'",
                "'OrderCharge'",
                "'OrderItem'",
                "'OrderItemAttribute'",
                "'OrderMotionOrder'",
                "'OrderMotionOrderSearch'",
                "'OrderMotionStore'",
                "'OrderPaymentDetail'",
                "'OrderSearch'",
                "'OtherProfile'",
                "'OtherShipment'",
                "'OverstockOrder'",
                "'OverstockOrderItem'",
                "'OverstockOrderSearch'",
                "'OverstockStore'",
                "'PayPalOrder'",
                "'PayPalOrderSearch'",
                "'PayPalStore'",
                "'PostalProfile'",
                "'PostalShipment'",
                "'ProStoresOrder'",
                "'ProStoresOrderSearch'",
                "'ProStoresStore'",
                "'ScanFormBatch'",
                "'Scheduling_BLOB_TRIGGERS'",
                "'Scheduling_CALENDARS'",
                "'Scheduling_CRON_TRIGGERS'",
                "'Scheduling_FIRED_TRIGGERS'",
                "'Scheduling_JOB_DETAILS'",
                "'Scheduling_LOCKS'",
                "'Scheduling_PAUSED_TRIGGER_GRPS'",
                "'Scheduling_SCHEDULER_STATE'",
                "'Scheduling_SIMPLE_TRIGGERS'",
                "'Scheduling_SIMPROP_TRIGGERS'",
                "'Scheduling_TRIGGERS'",
                "'SearsOrder'",
                "'SearsOrderItem'",
                "'SearsOrderSearch'",
                "'SearsStore'",
                "'Shipment'",
                "'ShipmentCustomsItem'",
                "'ShipmentReturnItem'",
                "'ShippingDefaultsRule'",
                "'ShippingOrigin'",
                "'ShippingPrintOutputRule'",
                "'ShippingProfile'",
                "'ShippingProviderRule'",
                "'ShipSenseKnowledgeBase'",
                "'ShopifyOrder'",
                "'ShopifyOrderItem'",
                "'ShopifyOrderSearch'",
                "'ShopifyStore'",
                "'ShopSiteStore'",
                "'SparkPayStore'",
                "'StatusPreset'",
                "'Store'",
                "'ThreeDCartOrder'",
                "'ThreeDCartOrderItem'",
                "'ThreeDCartOrderSearch'",
                "'ThreeDCartStore'",
                "'UpsAccount'",
                "'UpsLetterRate'",
                "'UpsLocalRatingDeliveryAreaSurcharge'",
                "'UpsLocalRatingZone'",
                "'UpsLocalRatingZoneFile'",
                "'UpsPackage'",
                "'UpsPackageRate'",
                "'UpsPricePerPound'",
                "'UpsProfile'",
                "'UpsProfilePackage'",
                "'UpsRateSurcharge'",
                "'UpsRateTable'",
                "'UpsShipment'",
                "'UspsAccount'",
                "'UspsProfile'",
                "'UspsScanForm'",
                "'UspsShipment'",
                "'ValidatedAddress'",
                "'VolusionStore'",
                "'WalmartOrder'",
                "'WalmartOrderItem'",
                "'WalmartOrderSearch'",
                "'WalmartStore'",
                "'WorldShipGoods'",
                "'WorldShipPackage'",
                "'WorldShipShipment'",
                "'YahooOrder'",
                "'YahooOrderItem'",
                "'YahooOrderSearch'",
                "'YahooProduct'",
                "'YahooStore'",
            };
    }
}
