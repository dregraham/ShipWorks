using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.RelationClasses;
using ShipWorks.Data.Model;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.Custom
{	
	/// <summary>
	/// Utility functions for creating entities and retrieving EntityType information.
	/// </summary>
	public static class EntityTypeProvider
	{ 
        /// <summary>
        /// Gets the EntityType based on the given entity type name.
        /// </summary>
		public static EntityType GetEntityType(string entityTypeName)
		{
			switch (entityTypeName)
			{
				case "ActionEntity": return EntityType.ActionEntity;
				case "ActionFilterTriggerEntity": return EntityType.ActionFilterTriggerEntity;
				case "ActionQueueEntity": return EntityType.ActionQueueEntity;
				case "ActionQueueSelectionEntity": return EntityType.ActionQueueSelectionEntity;
				case "ActionQueueStepEntity": return EntityType.ActionQueueStepEntity;
				case "ActionTaskEntity": return EntityType.ActionTaskEntity;
				case "AmazonASINEntity": return EntityType.AmazonASINEntity;
				case "AmazonOrderEntity": return EntityType.AmazonOrderEntity;
				case "AmazonOrderItemEntity": return EntityType.AmazonOrderItemEntity;
				case "AmazonProfileEntity": return EntityType.AmazonProfileEntity;
				case "AmazonServiceTypeEntity": return EntityType.AmazonServiceTypeEntity;
				case "AmazonShipmentEntity": return EntityType.AmazonShipmentEntity;
				case "AmazonStoreEntity": return EntityType.AmazonStoreEntity;
				case "AmeriCommerceStoreEntity": return EntityType.AmeriCommerceStoreEntity;
				case "AuditEntity": return EntityType.AuditEntity;
				case "AuditChangeEntity": return EntityType.AuditChangeEntity;
				case "AuditChangeDetailEntity": return EntityType.AuditChangeDetailEntity;
				case "BestRateProfileEntity": return EntityType.BestRateProfileEntity;
				case "BestRateShipmentEntity": return EntityType.BestRateShipmentEntity;
				case "BigCommerceOrderItemEntity": return EntityType.BigCommerceOrderItemEntity;
				case "BigCommerceStoreEntity": return EntityType.BigCommerceStoreEntity;
				case "BuyDotComOrderItemEntity": return EntityType.BuyDotComOrderItemEntity;
				case "BuyDotComStoreEntity": return EntityType.BuyDotComStoreEntity;
				case "ChannelAdvisorOrderEntity": return EntityType.ChannelAdvisorOrderEntity;
				case "ChannelAdvisorOrderItemEntity": return EntityType.ChannelAdvisorOrderItemEntity;
				case "ChannelAdvisorStoreEntity": return EntityType.ChannelAdvisorStoreEntity;
				case "ClickCartProOrderEntity": return EntityType.ClickCartProOrderEntity;
				case "CommerceInterfaceOrderEntity": return EntityType.CommerceInterfaceOrderEntity;
				case "ComputerEntity": return EntityType.ComputerEntity;
				case "ConfigurationEntity": return EntityType.ConfigurationEntity;
				case "CustomerEntity": return EntityType.CustomerEntity;
				case "DimensionsProfileEntity": return EntityType.DimensionsProfileEntity;
				case "DownloadEntity": return EntityType.DownloadEntity;
				case "DownloadDetailEntity": return EntityType.DownloadDetailEntity;
				case "EbayCombinedOrderRelationEntity": return EntityType.EbayCombinedOrderRelationEntity;
				case "EbayOrderEntity": return EntityType.EbayOrderEntity;
				case "EbayOrderItemEntity": return EntityType.EbayOrderItemEntity;
				case "EbayStoreEntity": return EntityType.EbayStoreEntity;
				case "EmailAccountEntity": return EntityType.EmailAccountEntity;
				case "EmailOutboundEntity": return EntityType.EmailOutboundEntity;
				case "EmailOutboundRelationEntity": return EntityType.EmailOutboundRelationEntity;
				case "EndiciaAccountEntity": return EntityType.EndiciaAccountEntity;
				case "EndiciaProfileEntity": return EntityType.EndiciaProfileEntity;
				case "EndiciaScanFormEntity": return EntityType.EndiciaScanFormEntity;
				case "EndiciaShipmentEntity": return EntityType.EndiciaShipmentEntity;
				case "EtsyOrderEntity": return EntityType.EtsyOrderEntity;
				case "EtsyOrderItemEntity": return EntityType.EtsyOrderItemEntity;
				case "EtsyStoreEntity": return EntityType.EtsyStoreEntity;
				case "ExcludedPackageTypeEntity": return EntityType.ExcludedPackageTypeEntity;
				case "ExcludedServiceTypeEntity": return EntityType.ExcludedServiceTypeEntity;
				case "FedExAccountEntity": return EntityType.FedExAccountEntity;
				case "FedExEndOfDayCloseEntity": return EntityType.FedExEndOfDayCloseEntity;
				case "FedExPackageEntity": return EntityType.FedExPackageEntity;
				case "FedExProfileEntity": return EntityType.FedExProfileEntity;
				case "FedExProfilePackageEntity": return EntityType.FedExProfilePackageEntity;
				case "FedExShipmentEntity": return EntityType.FedExShipmentEntity;
				case "FilterEntity": return EntityType.FilterEntity;
				case "FilterLayoutEntity": return EntityType.FilterLayoutEntity;
				case "FilterNodeEntity": return EntityType.FilterNodeEntity;
				case "FilterNodeColumnSettingsEntity": return EntityType.FilterNodeColumnSettingsEntity;
				case "FilterNodeContentEntity": return EntityType.FilterNodeContentEntity;
				case "FilterNodeContentDetailEntity": return EntityType.FilterNodeContentDetailEntity;
				case "FilterSequenceEntity": return EntityType.FilterSequenceEntity;
				case "FtpAccountEntity": return EntityType.FtpAccountEntity;
				case "GenericFileStoreEntity": return EntityType.GenericFileStoreEntity;
				case "GenericModuleStoreEntity": return EntityType.GenericModuleStoreEntity;
				case "GridColumnFormatEntity": return EntityType.GridColumnFormatEntity;
				case "GridColumnLayoutEntity": return EntityType.GridColumnLayoutEntity;
				case "GridColumnPositionEntity": return EntityType.GridColumnPositionEntity;
				case "GrouponOrderEntity": return EntityType.GrouponOrderEntity;
				case "GrouponOrderItemEntity": return EntityType.GrouponOrderItemEntity;
				case "GrouponStoreEntity": return EntityType.GrouponStoreEntity;
				case "InfopiaOrderItemEntity": return EntityType.InfopiaOrderItemEntity;
				case "InfopiaStoreEntity": return EntityType.InfopiaStoreEntity;
				case "InsurancePolicyEntity": return EntityType.InsurancePolicyEntity;
				case "IParcelAccountEntity": return EntityType.IParcelAccountEntity;
				case "IParcelPackageEntity": return EntityType.IParcelPackageEntity;
				case "IParcelProfileEntity": return EntityType.IParcelProfileEntity;
				case "IParcelProfilePackageEntity": return EntityType.IParcelProfilePackageEntity;
				case "IParcelShipmentEntity": return EntityType.IParcelShipmentEntity;
				case "JetOrderEntity": return EntityType.JetOrderEntity;
				case "JetOrderItemEntity": return EntityType.JetOrderItemEntity;
				case "JetStoreEntity": return EntityType.JetStoreEntity;
				case "LabelSheetEntity": return EntityType.LabelSheetEntity;
				case "LemonStandOrderEntity": return EntityType.LemonStandOrderEntity;
				case "LemonStandOrderItemEntity": return EntityType.LemonStandOrderItemEntity;
				case "LemonStandStoreEntity": return EntityType.LemonStandStoreEntity;
				case "MagentoOrderEntity": return EntityType.MagentoOrderEntity;
				case "MagentoStoreEntity": return EntityType.MagentoStoreEntity;
				case "MarketplaceAdvisorOrderEntity": return EntityType.MarketplaceAdvisorOrderEntity;
				case "MarketplaceAdvisorStoreEntity": return EntityType.MarketplaceAdvisorStoreEntity;
				case "MivaOrderItemAttributeEntity": return EntityType.MivaOrderItemAttributeEntity;
				case "MivaStoreEntity": return EntityType.MivaStoreEntity;
				case "NetworkSolutionsOrderEntity": return EntityType.NetworkSolutionsOrderEntity;
				case "NetworkSolutionsStoreEntity": return EntityType.NetworkSolutionsStoreEntity;
				case "NeweggOrderEntity": return EntityType.NeweggOrderEntity;
				case "NeweggOrderItemEntity": return EntityType.NeweggOrderItemEntity;
				case "NeweggStoreEntity": return EntityType.NeweggStoreEntity;
				case "NoteEntity": return EntityType.NoteEntity;
				case "ObjectLabelEntity": return EntityType.ObjectLabelEntity;
				case "ObjectReferenceEntity": return EntityType.ObjectReferenceEntity;
				case "OdbcStoreEntity": return EntityType.OdbcStoreEntity;
				case "OnTracAccountEntity": return EntityType.OnTracAccountEntity;
				case "OnTracProfileEntity": return EntityType.OnTracProfileEntity;
				case "OnTracShipmentEntity": return EntityType.OnTracShipmentEntity;
				case "OrderEntity": return EntityType.OrderEntity;
				case "OrderChargeEntity": return EntityType.OrderChargeEntity;
				case "OrderItemEntity": return EntityType.OrderItemEntity;
				case "OrderItemAttributeEntity": return EntityType.OrderItemAttributeEntity;
				case "OrderMotionOrderEntity": return EntityType.OrderMotionOrderEntity;
				case "OrderMotionStoreEntity": return EntityType.OrderMotionStoreEntity;
				case "OrderPaymentDetailEntity": return EntityType.OrderPaymentDetailEntity;
				case "OtherProfileEntity": return EntityType.OtherProfileEntity;
				case "OtherShipmentEntity": return EntityType.OtherShipmentEntity;
				case "PayPalOrderEntity": return EntityType.PayPalOrderEntity;
				case "PayPalStoreEntity": return EntityType.PayPalStoreEntity;
				case "PermissionEntity": return EntityType.PermissionEntity;
				case "PostalProfileEntity": return EntityType.PostalProfileEntity;
				case "PostalShipmentEntity": return EntityType.PostalShipmentEntity;
				case "PrintResultEntity": return EntityType.PrintResultEntity;
				case "ProStoresOrderEntity": return EntityType.ProStoresOrderEntity;
				case "ProStoresStoreEntity": return EntityType.ProStoresStoreEntity;
				case "ResourceEntity": return EntityType.ResourceEntity;
				case "ScanFormBatchEntity": return EntityType.ScanFormBatchEntity;
				case "SearchEntity": return EntityType.SearchEntity;
				case "SearsOrderEntity": return EntityType.SearsOrderEntity;
				case "SearsOrderItemEntity": return EntityType.SearsOrderItemEntity;
				case "SearsStoreEntity": return EntityType.SearsStoreEntity;
				case "ServerMessageEntity": return EntityType.ServerMessageEntity;
				case "ServerMessageSignoffEntity": return EntityType.ServerMessageSignoffEntity;
				case "ServiceStatusEntity": return EntityType.ServiceStatusEntity;
				case "ShipmentEntity": return EntityType.ShipmentEntity;
				case "ShipmentCustomsItemEntity": return EntityType.ShipmentCustomsItemEntity;
				case "ShipmentReturnItemEntity": return EntityType.ShipmentReturnItemEntity;
				case "ShippingDefaultsRuleEntity": return EntityType.ShippingDefaultsRuleEntity;
				case "ShippingOriginEntity": return EntityType.ShippingOriginEntity;
				case "ShippingPrintOutputEntity": return EntityType.ShippingPrintOutputEntity;
				case "ShippingPrintOutputRuleEntity": return EntityType.ShippingPrintOutputRuleEntity;
				case "ShippingProfileEntity": return EntityType.ShippingProfileEntity;
				case "ShippingProviderRuleEntity": return EntityType.ShippingProviderRuleEntity;
				case "ShippingSettingsEntity": return EntityType.ShippingSettingsEntity;
				case "ShipSenseKnowledgebaseEntity": return EntityType.ShipSenseKnowledgebaseEntity;
				case "ShopifyOrderEntity": return EntityType.ShopifyOrderEntity;
				case "ShopifyOrderItemEntity": return EntityType.ShopifyOrderItemEntity;
				case "ShopifyStoreEntity": return EntityType.ShopifyStoreEntity;
				case "ShopSiteStoreEntity": return EntityType.ShopSiteStoreEntity;
				case "SparkPayStoreEntity": return EntityType.SparkPayStoreEntity;
				case "StatusPresetEntity": return EntityType.StatusPresetEntity;
				case "StoreEntity": return EntityType.StoreEntity;
				case "SystemDataEntity": return EntityType.SystemDataEntity;
				case "TemplateEntity": return EntityType.TemplateEntity;
				case "TemplateComputerSettingsEntity": return EntityType.TemplateComputerSettingsEntity;
				case "TemplateFolderEntity": return EntityType.TemplateFolderEntity;
				case "TemplateStoreSettingsEntity": return EntityType.TemplateStoreSettingsEntity;
				case "TemplateUserSettingsEntity": return EntityType.TemplateUserSettingsEntity;
				case "ThreeDCartOrderEntity": return EntityType.ThreeDCartOrderEntity;
				case "ThreeDCartOrderItemEntity": return EntityType.ThreeDCartOrderItemEntity;
				case "ThreeDCartStoreEntity": return EntityType.ThreeDCartStoreEntity;
				case "UpsAccountEntity": return EntityType.UpsAccountEntity;
				case "UpsLetterRateEntity": return EntityType.UpsLetterRateEntity;
				case "UpsLocalRatingDeliveryAreaSurchargeEntity": return EntityType.UpsLocalRatingDeliveryAreaSurchargeEntity;
				case "UpsLocalRatingZoneEntity": return EntityType.UpsLocalRatingZoneEntity;
				case "UpsLocalRatingZoneFileEntity": return EntityType.UpsLocalRatingZoneFileEntity;
				case "UpsPackageEntity": return EntityType.UpsPackageEntity;
				case "UpsPackageRateEntity": return EntityType.UpsPackageRateEntity;
				case "UpsPricePerPoundEntity": return EntityType.UpsPricePerPoundEntity;
				case "UpsProfileEntity": return EntityType.UpsProfileEntity;
				case "UpsProfilePackageEntity": return EntityType.UpsProfilePackageEntity;
				case "UpsRateSurchargeEntity": return EntityType.UpsRateSurchargeEntity;
				case "UpsRateTableEntity": return EntityType.UpsRateTableEntity;
				case "UpsShipmentEntity": return EntityType.UpsShipmentEntity;
				case "UserEntity": return EntityType.UserEntity;
				case "UserColumnSettingsEntity": return EntityType.UserColumnSettingsEntity;
				case "UserSettingsEntity": return EntityType.UserSettingsEntity;
				case "UspsAccountEntity": return EntityType.UspsAccountEntity;
				case "UspsProfileEntity": return EntityType.UspsProfileEntity;
				case "UspsScanFormEntity": return EntityType.UspsScanFormEntity;
				case "UspsShipmentEntity": return EntityType.UspsShipmentEntity;
				case "ValidatedAddressEntity": return EntityType.ValidatedAddressEntity;
				case "VersionSignoffEntity": return EntityType.VersionSignoffEntity;
				case "VolusionStoreEntity": return EntityType.VolusionStoreEntity;
				case "WalmartOrderEntity": return EntityType.WalmartOrderEntity;
				case "WalmartOrderItemEntity": return EntityType.WalmartOrderItemEntity;
				case "WalmartStoreEntity": return EntityType.WalmartStoreEntity;
				case "WorldShipGoodsEntity": return EntityType.WorldShipGoodsEntity;
				case "WorldShipPackageEntity": return EntityType.WorldShipPackageEntity;
				case "WorldShipProcessedEntity": return EntityType.WorldShipProcessedEntity;
				case "WorldShipShipmentEntity": return EntityType.WorldShipShipmentEntity;
				case "YahooOrderEntity": return EntityType.YahooOrderEntity;
				case "YahooOrderItemEntity": return EntityType.YahooOrderItemEntity;
				case "YahooProductEntity": return EntityType.YahooProductEntity;
				case "YahooStoreEntity": return EntityType.YahooStoreEntity;
			}

			throw new ArgumentException($"Entity type name 'entityTypeName' is not valid.");
		}

        /// <summary>
        /// Gets the entity type name based on the given EntityType.
        /// </summary>
		public static string GetEntityTypeName(EntityType entityType)
		{
			IEntity2 entity = GeneralEntityFactory.Create(entityType);
			return entity.LLBLGenProEntityName;
		}

		/// <summary>
        /// Gets the System.Type based on the given entity type name.
        /// </summary>
		public static Type GetSystemType(string entityTypeName)
		{
			return GetSystemType(GetEntityType(entityTypeName));
		}

        /// <summary>
        /// Gets the System.Type based on the given EntityType.
        /// </summary>
		public static Type GetSystemType(EntityType entityType)
		{
			IEntity2 entity = GeneralEntityFactory.Create(entityType);
			return entity.GetType();
		}

        /// <summary>
        /// Creates an entity instance based on the given name.
        /// </summary>
		public static IEntity2 CreateEntity(string entityTypeName)
		{
			return GeneralEntityFactory.Create(GetEntityType(entityTypeName));
		}

        /// <summary>
        /// Get the IRelationFactory that provides inheritance relationship information for the specified EntityType.
        /// </summary>
		public static IRelationFactory GetInheritanceRelationFactory(EntityType entityType)
		{
			switch (entityType)
			{
				case EntityType.AmazonOrderEntity: return new AmazonOrderRelations();
				case EntityType.AmazonOrderItemEntity: return new AmazonOrderItemRelations();
				case EntityType.AmazonStoreEntity: return new AmazonStoreRelations();
				case EntityType.AmeriCommerceStoreEntity: return new AmeriCommerceStoreRelations();
				case EntityType.BigCommerceOrderItemEntity: return new BigCommerceOrderItemRelations();
				case EntityType.BigCommerceStoreEntity: return new BigCommerceStoreRelations();
				case EntityType.BuyDotComOrderItemEntity: return new BuyDotComOrderItemRelations();
				case EntityType.BuyDotComStoreEntity: return new BuyDotComStoreRelations();
				case EntityType.ChannelAdvisorOrderEntity: return new ChannelAdvisorOrderRelations();
				case EntityType.ChannelAdvisorOrderItemEntity: return new ChannelAdvisorOrderItemRelations();
				case EntityType.ChannelAdvisorStoreEntity: return new ChannelAdvisorStoreRelations();
				case EntityType.ClickCartProOrderEntity: return new ClickCartProOrderRelations();
				case EntityType.CommerceInterfaceOrderEntity: return new CommerceInterfaceOrderRelations();
				case EntityType.EbayOrderEntity: return new EbayOrderRelations();
				case EntityType.EbayOrderItemEntity: return new EbayOrderItemRelations();
				case EntityType.EbayStoreEntity: return new EbayStoreRelations();
				case EntityType.EtsyOrderEntity: return new EtsyOrderRelations();
				case EntityType.EtsyOrderItemEntity: return new EtsyOrderItemRelations();
				case EntityType.EtsyStoreEntity: return new EtsyStoreRelations();
				case EntityType.GenericFileStoreEntity: return new GenericFileStoreRelations();
				case EntityType.GenericModuleStoreEntity: return new GenericModuleStoreRelations();
				case EntityType.GrouponOrderEntity: return new GrouponOrderRelations();
				case EntityType.GrouponOrderItemEntity: return new GrouponOrderItemRelations();
				case EntityType.GrouponStoreEntity: return new GrouponStoreRelations();
				case EntityType.InfopiaOrderItemEntity: return new InfopiaOrderItemRelations();
				case EntityType.InfopiaStoreEntity: return new InfopiaStoreRelations();
				case EntityType.JetOrderEntity: return new JetOrderRelations();
				case EntityType.JetOrderItemEntity: return new JetOrderItemRelations();
				case EntityType.JetStoreEntity: return new JetStoreRelations();
				case EntityType.LemonStandOrderEntity: return new LemonStandOrderRelations();
				case EntityType.LemonStandOrderItemEntity: return new LemonStandOrderItemRelations();
				case EntityType.LemonStandStoreEntity: return new LemonStandStoreRelations();
				case EntityType.MagentoOrderEntity: return new MagentoOrderRelations();
				case EntityType.MagentoStoreEntity: return new MagentoStoreRelations();
				case EntityType.MarketplaceAdvisorOrderEntity: return new MarketplaceAdvisorOrderRelations();
				case EntityType.MarketplaceAdvisorStoreEntity: return new MarketplaceAdvisorStoreRelations();
				case EntityType.MivaOrderItemAttributeEntity: return new MivaOrderItemAttributeRelations();
				case EntityType.MivaStoreEntity: return new MivaStoreRelations();
				case EntityType.NetworkSolutionsOrderEntity: return new NetworkSolutionsOrderRelations();
				case EntityType.NetworkSolutionsStoreEntity: return new NetworkSolutionsStoreRelations();
				case EntityType.NeweggOrderEntity: return new NeweggOrderRelations();
				case EntityType.NeweggOrderItemEntity: return new NeweggOrderItemRelations();
				case EntityType.NeweggStoreEntity: return new NeweggStoreRelations();
				case EntityType.OdbcStoreEntity: return new OdbcStoreRelations();
				case EntityType.OrderEntity: return new OrderRelations();
				case EntityType.OrderItemEntity: return new OrderItemRelations();
				case EntityType.OrderItemAttributeEntity: return new OrderItemAttributeRelations();
				case EntityType.OrderMotionOrderEntity: return new OrderMotionOrderRelations();
				case EntityType.OrderMotionStoreEntity: return new OrderMotionStoreRelations();
				case EntityType.PayPalOrderEntity: return new PayPalOrderRelations();
				case EntityType.PayPalStoreEntity: return new PayPalStoreRelations();
				case EntityType.ProStoresOrderEntity: return new ProStoresOrderRelations();
				case EntityType.ProStoresStoreEntity: return new ProStoresStoreRelations();
				case EntityType.SearsOrderEntity: return new SearsOrderRelations();
				case EntityType.SearsOrderItemEntity: return new SearsOrderItemRelations();
				case EntityType.SearsStoreEntity: return new SearsStoreRelations();
				case EntityType.ShopifyOrderEntity: return new ShopifyOrderRelations();
				case EntityType.ShopifyOrderItemEntity: return new ShopifyOrderItemRelations();
				case EntityType.ShopifyStoreEntity: return new ShopifyStoreRelations();
				case EntityType.ShopSiteStoreEntity: return new ShopSiteStoreRelations();
				case EntityType.SparkPayStoreEntity: return new SparkPayStoreRelations();
				case EntityType.StoreEntity: return new StoreRelations();
				case EntityType.ThreeDCartOrderEntity: return new ThreeDCartOrderRelations();
				case EntityType.ThreeDCartOrderItemEntity: return new ThreeDCartOrderItemRelations();
				case EntityType.ThreeDCartStoreEntity: return new ThreeDCartStoreRelations();
				case EntityType.VolusionStoreEntity: return new VolusionStoreRelations();
				case EntityType.WalmartOrderEntity: return new WalmartOrderRelations();
				case EntityType.WalmartOrderItemEntity: return new WalmartOrderItemRelations();
				case EntityType.WalmartStoreEntity: return new WalmartStoreRelations();
				case EntityType.YahooOrderEntity: return new YahooOrderRelations();
				case EntityType.YahooOrderItemEntity: return new YahooOrderItemRelations();
				case EntityType.YahooStoreEntity: return new YahooStoreRelations();
			}

			throw new ArgumentException($"Entity type '{entityType}' is not valid or is not a part of a TargetPerEntity hierarchy.");
		}
	}
}