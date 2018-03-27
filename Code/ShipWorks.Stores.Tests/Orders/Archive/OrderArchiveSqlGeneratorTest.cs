using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Archive;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Orders.Archive
{
    public class OrderArchiveSqlGeneratorTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly OrderArchiveSqlGenerator testObject;

        public OrderArchiveSqlGeneratorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<OrderArchiveSqlGenerator>();
        }

        [Fact]
        public void CopyDatabaseSql_ReturnsNonEmptyString()
        {
            Assert.False(testObject.CopyDatabaseSql(AnyString, AnyDate, AnyString).IsNullOrWhiteSpace());
        }

        [Fact]
        public void ArchiveOrderDataSql_ReturnsNonEmptyString()
        {
            Assert.False(testObject.ArchiveOrderDataSql(AnyString, DateTime.Now, It.IsAny<OrderArchiverOrderDataComparisonType>()).IsNullOrWhiteSpace());
        }

        [Fact]
        public void ArchiveOrderDataSql_HasCorrectDate()
        {
            DateTime maxDateTime = DateTime.Now;
            string archiveSql = testObject.ArchiveOrderDataSql(AnyString, maxDateTime, It.IsAny<OrderArchiverOrderDataComparisonType>());

            Assert.True(archiveSql.Contains(maxDateTime.Date.ToString("yyyy-MM-dd HH:mm:ss")));
        }

        [Fact]
        public void ArchiveOrderDataSql_HasCurrentlySupportedEntities()
        {
            DateTime maxDateTime = DateTime.Now;
            string archiveSql = testObject.ArchiveOrderDataSql(AnyString, maxDateTime, It.IsAny<OrderArchiverOrderDataComparisonType>()).ToUpperInvariant();

            foreach (var entityType in CurrentlySupportedEntities)
            {
                string searchText = $"PurgeEntities '{entityType.Name.Replace("Entity", string.Empty)}'".ToUpperInvariant();
                Assert.True(archiveSql.Contains(searchText), $"{searchText} not found.");
            }
        }

        [Fact]
        public void ArchiveOrderDataSql_NoNewEntitiesHaveBeenAdded()
        {
            string newEntities = string.Empty;
            var newEntitiesBuilder = new StringBuilder();

            Assembly mscorlib = typeof(AmazonOrderEntity).Assembly;
            foreach (Type type in mscorlib.GetTypes()
                .Where(t => !t.IsInterface && t.Name.EndsWith("Entity") && !t.Name.StartsWith("ReadOnly"))
                .Except(CurrentlySupportedEntities)
                .Except(ExcludedEntities)
                .OrderBy(t => t.Name))
            {
                if (!CurrentlySupportedEntities.Contains(type))
                {
                    newEntitiesBuilder.AppendLine($"{type.Name},");
                }
            }

            newEntities = newEntitiesBuilder.ToString();

            if (!newEntities.IsNullOrWhiteSpace())
            {
                newEntities = $"It seems that new entities have been added.  Determine if they should be added " +
                              $"to order/shipment archiving and add them to the appropriate " +
                              $"test properties, CurrentlySupportedEntities or ExcludedEntities. If adding new " +
                              $"entities that should be archived, be sure to update ArchiveOrderData.sql." +
                              $"{Environment.NewLine}{Environment.NewLine}{newEntities}";
            }

            Assert.Equal(string.Empty, newEntities);
        }

        private IEnumerable<Type> CurrentlySupportedEntities
        {
            get
            {
                return new Type[]
                {
                    typeof(AmazonOrderEntity),
                    typeof(AmazonOrderItemEntity),
                    typeof(AmazonOrderSearchEntity),
                    typeof(AmazonShipmentEntity),
                    typeof(AsendiaShipmentEntity),
                    typeof(AuditEntity),
                    typeof(AuditChangeEntity),
                    typeof(AuditChangeDetailEntity),
                    typeof(BestRateShipmentEntity),
                    typeof(BigCommerceOrderItemEntity),
                    typeof(BuyDotComOrderItemEntity),
                    typeof(ChannelAdvisorOrderEntity),
                    typeof(ChannelAdvisorOrderItemEntity),
                    typeof(ChannelAdvisorOrderSearchEntity),
                    typeof(ClickCartProOrderEntity),
                    typeof(ClickCartProOrderSearchEntity),
                    typeof(CommerceInterfaceOrderEntity),
                    typeof(CommerceInterfaceOrderSearchEntity),
                    typeof(DhlExpressPackageEntity),
                    typeof(DhlExpressShipmentEntity),
                    typeof(DownloadDetailEntity),
                    typeof(EbayCombinedOrderRelationEntity),
                    typeof(EbayOrderEntity),
                    typeof(EbayOrderItemEntity),
                    typeof(EbayOrderSearchEntity),
                    typeof(EmailOutboundEntity),
                    typeof(EmailOutboundRelationEntity),
                    typeof(EndiciaShipmentEntity),
                    typeof(EtsyOrderEntity),
                    typeof(EtsyOrderItemEntity),
                    typeof(FedExPackageEntity),
                    typeof(FedExShipmentEntity),
                    typeof(GenericModuleOrderEntity),
                    typeof(GenericModuleOrderItemEntity),
                    typeof(GrouponOrderEntity),
                    typeof(GrouponOrderItemEntity),
                    typeof(GrouponOrderSearchEntity),
                    typeof(InfopiaOrderItemEntity),
                    typeof(InsurancePolicyEntity),
                    typeof(IParcelPackageEntity),
                    typeof(IParcelShipmentEntity),
                    typeof(JetOrderEntity),
                    typeof(JetOrderItemEntity),
                    typeof(JetOrderSearchEntity),
                    typeof(LemonStandOrderEntity),
                    typeof(LemonStandOrderItemEntity),
                    typeof(LemonStandOrderSearchEntity),
                    typeof(MagentoOrderEntity),
                    typeof(MagentoOrderSearchEntity),
                    typeof(MarketplaceAdvisorOrderEntity),
                    typeof(MarketplaceAdvisorOrderSearchEntity),
                    typeof(MivaOrderItemAttributeEntity),
                    typeof(NetworkSolutionsOrderEntity),
                    typeof(NetworkSolutionsOrderSearchEntity),
                    typeof(NeweggOrderEntity),
                    typeof(NeweggOrderItemEntity),
                    typeof(NoteEntity),
                    typeof(ObjectLabelEntity),
                    typeof(ObjectReferenceEntity),
                    typeof(OnTracShipmentEntity),
                    typeof(OrderEntity),
                    typeof(OrderChargeEntity),
                    typeof(OrderItemEntity),
                    typeof(OrderItemAttributeEntity),
                    typeof(OrderMotionOrderEntity),
                    typeof(OrderMotionOrderSearchEntity),
                    typeof(OrderPaymentDetailEntity),
                    typeof(OrderSearchEntity),
                    typeof(OtherShipmentEntity),
                    typeof(PayPalOrderEntity),
                    typeof(PayPalOrderSearchEntity),
                    typeof(PostalShipmentEntity),
                    typeof(PrintResultEntity),
                    typeof(ProStoresOrderEntity),
                    typeof(SearsOrderEntity),
                    typeof(SearsOrderItemEntity),
                    typeof(SearsOrderSearchEntity),
                    typeof(ShipmentEntity),
                    typeof(ShipmentCustomsItemEntity),
                    typeof(ShipmentReturnItemEntity),
                    typeof(ShopifyOrderEntity),
                    typeof(ShopifyOrderItemEntity),
                    typeof(ShopifyOrderSearchEntity),
                    typeof(ThreeDCartOrderEntity),
                    typeof(ThreeDCartOrderItemEntity),
                    typeof(ThreeDCartOrderSearchEntity),
                    typeof(UpsPackageEntity),
                    typeof(UpsShipmentEntity),
                    typeof(UspsShipmentEntity),
                    typeof(ValidatedAddressEntity),
                    typeof(WalmartOrderEntity),
                    typeof(WalmartOrderItemEntity),
                    typeof(WalmartOrderSearchEntity),
                    typeof(WorldShipGoodsEntity),
                    typeof(WorldShipPackageEntity),
                    typeof(WorldShipProcessedEntity),
                    typeof(WorldShipShipmentEntity),
                    typeof(YahooOrderEntity),
                    typeof(YahooOrderItemEntity),
                    typeof(YahooOrderSearchEntity)
                };
            }
        }
        private IEnumerable<Type> ExcludedEntities
        {
            get
            {
                return new Type[]
                {
                    typeof(ActionEntity),
                    typeof(ActionFilterTriggerEntity),
                    typeof(ActionQueueEntity),
                    typeof(ActionQueueSelectionEntity),
                    typeof(ActionQueueStepEntity),
                    typeof(ActionTaskEntity),
                    typeof(AmazonASINEntity),
                    typeof(AmazonProfileEntity),
                    typeof(AmazonServiceTypeEntity),
                    typeof(AmazonStoreEntity),
                    typeof(AmeriCommerceStoreEntity),
                    typeof(AsendiaAccountEntity),
                    typeof(AsendiaProfileEntity),
                    typeof(BestRateProfileEntity),
                    typeof(BigCommerceStoreEntity),
                    typeof(BuyDotComStoreEntity),
                    typeof(ChannelAdvisorStoreEntity),
                    typeof(ComputerEntity),
                    typeof(ConfigurationEntity),
                    typeof(CustomerEntity),
                    typeof(DhlExpressAccountEntity),
                    typeof(DhlExpressProfileEntity),
                    typeof(DhlExpressProfilePackageEntity),
                    typeof(DimensionsProfileEntity),
                    typeof(DownloadEntity),
                    typeof(EbayStoreEntity),
                    typeof(EmailAccountEntity),
                    typeof(EndiciaAccountEntity),
                    typeof(EndiciaProfileEntity),
                    typeof(EndiciaScanFormEntity),
                    typeof(EtsyStoreEntity),
                    typeof(ExcludedPackageTypeEntity),
                    typeof(ExcludedServiceTypeEntity),
                    typeof(FedExAccountEntity),
                    typeof(FedExEndOfDayCloseEntity),
                    typeof(FedExProfileEntity),
                    typeof(FedExProfilePackageEntity),
                    typeof(FilterEntity),
                    typeof(FilterLayoutEntity),
                    typeof(FilterNodeColumnSettingsEntity),
                    typeof(FilterNodeContentDetailEntity),
                    typeof(FilterNodeContentEntity),
                    typeof(FilterNodeEntity),
                    typeof(FilterSequenceEntity),
                    typeof(FtpAccountEntity),
                    typeof(GenericFileStoreEntity),
                    typeof(GenericModuleStoreEntity),
                    typeof(GridColumnFormatEntity),
                    typeof(GridColumnLayoutEntity),
                    typeof(GridColumnPositionEntity),
                    typeof(GrouponStoreEntity),
                    typeof(InfopiaStoreEntity),
                    typeof(IParcelAccountEntity),
                    typeof(IParcelProfileEntity),
                    typeof(IParcelProfilePackageEntity),
                    typeof(JetStoreEntity),
                    typeof(LabelSheetEntity),
                    typeof(LemonStandStoreEntity),
                    typeof(MagentoStoreEntity),
                    typeof(MarketplaceAdvisorStoreEntity),
                    typeof(MivaStoreEntity),
                    typeof(NetworkSolutionsStoreEntity),
                    typeof(NeweggStoreEntity),
                    typeof(NullEntity),
                    typeof(OdbcStoreEntity),
                    typeof(OnTracAccountEntity),
                    typeof(OnTracProfileEntity),
                    typeof(OrderMotionStoreEntity),
                    typeof(OtherProfileEntity),
                    typeof(PayPalStoreEntity),
                    typeof(PermissionEntity),
                    typeof(PostalProfileEntity),
                    typeof(ProStoresOrderSearchEntity),
                    typeof(ProStoresStoreEntity),
                    typeof(ResourceEntity),
                    typeof(ScanFormBatchEntity),
                    typeof(SearchEntity),
                    typeof(SearsStoreEntity),
                    typeof(ServerMessageEntity),
                    typeof(ServerMessageSignoffEntity),
                    typeof(ServiceStatusEntity),
                    typeof(ShippingDefaultsRuleEntity),
                    typeof(ShippingOriginEntity),
                    typeof(ShippingPrintOutputEntity),
                    typeof(ShippingPrintOutputRuleEntity),
                    typeof(ShippingProfileEntity),
                    typeof(ShippingProviderRuleEntity),
                    typeof(ShippingSettingsEntity),
                    typeof(ShipSenseKnowledgebaseEntity),
                    typeof(ShopifyStoreEntity),
                    typeof(ShopSiteStoreEntity),
                    typeof(SparkPayStoreEntity),
                    typeof(StatusPresetEntity),
                    typeof(StoreEntity),
                    typeof(SystemDataEntity),
                    typeof(TemplateComputerSettingsEntity),
                    typeof(TemplateEntity),
                    typeof(TemplateFolderEntity),
                    typeof(TemplateStoreSettingsEntity),
                    typeof(TemplateUserSettingsEntity),
                    typeof(ThreeDCartStoreEntity),
                    typeof(UpsAccountEntity),
                    typeof(UpsLetterRateEntity),
                    typeof(UpsLocalRatingDeliveryAreaSurchargeEntity),
                    typeof(UpsLocalRatingZoneEntity),
                    typeof(UpsLocalRatingZoneFileEntity),
                    typeof(UpsPackageRateEntity),
                    typeof(UpsPricePerPoundEntity),
                    typeof(UpsProfileEntity),
                    typeof(UpsProfilePackageEntity),
                    typeof(UpsRateSurchargeEntity),
                    typeof(UpsRateTableEntity),
                    typeof(UserColumnSettingsEntity),
                    typeof(UserEntity),
                    typeof(UserSettingsEntity),
                    typeof(UspsAccountEntity),
                    typeof(UspsProfileEntity),
                    typeof(UspsScanFormEntity),
                    typeof(VersionSignoffEntity),
                    typeof(VolusionStoreEntity),
                    typeof(WalmartStoreEntity),
                    typeof(YahooProductEntity),
                    typeof(YahooStoreEntity)
                };
            }
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
