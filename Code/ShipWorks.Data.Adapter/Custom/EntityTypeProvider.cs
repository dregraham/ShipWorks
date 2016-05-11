using System;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.RelationClasses;
using ShipWorks.Data.Model;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Adapter.Custom
{	
	/// <summary>
	/// Utility functions for creating entities and retrieving EntityType information.
	/// </summary>
	public static class EntityTypeProvider
	{ 
        /// <summary>
        /// Gets the EntityType based on the the given entity type name.
        /// </summary>
		public static EntityType GetEntityType(string entityTypeName)
		{
			switch (entityTypeName)
			{
				case "StoreEntity": return EntityType.StoreEntity;
				case "AuditEntity": return EntityType.AuditEntity;
				case "ComputerEntity": return EntityType.ComputerEntity;
				case "UserEntity": return EntityType.UserEntity;
				case "PermissionEntity": return EntityType.PermissionEntity;
				case "OrderEntity": return EntityType.OrderEntity;
				case "FilterEntity": return EntityType.FilterEntity;
				case "FilterNodeEntity": return EntityType.FilterNodeEntity;
				case "FilterSequenceEntity": return EntityType.FilterSequenceEntity;
				case "FilterLayoutEntity": return EntityType.FilterLayoutEntity;
				case "CustomerEntity": return EntityType.CustomerEntity;
				case "OrderItemEntity": return EntityType.OrderItemEntity;
				case "FilterNodeContentEntity": return EntityType.FilterNodeContentEntity;
				case "UserSettingsEntity": return EntityType.UserSettingsEntity;
				case "GridColumnFormatEntity": return EntityType.GridColumnFormatEntity;
				case "GridColumnLayoutEntity": return EntityType.GridColumnLayoutEntity;
				case "GridColumnPositionEntity": return EntityType.GridColumnPositionEntity;
				case "ShopSiteStoreEntity": return EntityType.ShopSiteStoreEntity;
				case "DownloadEntity": return EntityType.DownloadEntity;
				case "DownloadDetailEntity": return EntityType.DownloadDetailEntity;
				case "OrderItemAttributeEntity": return EntityType.OrderItemAttributeEntity;
				case "OrderChargeEntity": return EntityType.OrderChargeEntity;
				case "StatusPresetEntity": return EntityType.StatusPresetEntity;
				case "OrderPaymentDetailEntity": return EntityType.OrderPaymentDetailEntity;
				case "ConfigurationEntity": return EntityType.ConfigurationEntity;
				case "FilterNodeContentDetailEntity": return EntityType.FilterNodeContentDetailEntity;
				case "SystemDataEntity": return EntityType.SystemDataEntity;
				case "TemplateEntity": return EntityType.TemplateEntity;
				case "ResourceEntity": return EntityType.ResourceEntity;
				case "TemplateFolderEntity": return EntityType.TemplateFolderEntity;
				case "LabelSheetEntity": return EntityType.LabelSheetEntity;
				case "TemplateComputerSettingsEntity": return EntityType.TemplateComputerSettingsEntity;
				case "TemplateUserSettingsEntity": return EntityType.TemplateUserSettingsEntity;
				case "ShipmentEntity": return EntityType.ShipmentEntity;
				case "FilterNodeColumnSettingsEntity": return EntityType.FilterNodeColumnSettingsEntity;
				case "TemplateStoreSettingsEntity": return EntityType.TemplateStoreSettingsEntity;
				case "EmailAccountEntity": return EntityType.EmailAccountEntity;
				case "EmailOutboundEntity": return EntityType.EmailOutboundEntity;
				case "ServerMessageSignoffEntity": return EntityType.ServerMessageSignoffEntity;
				case "VersionSignoffEntity": return EntityType.VersionSignoffEntity;
				case "ServerMessageEntity": return EntityType.ServerMessageEntity;
				case "UserColumnSettingsEntity": return EntityType.UserColumnSettingsEntity;
				case "ActionEntity": return EntityType.ActionEntity;
				case "ActionTaskEntity": return EntityType.ActionTaskEntity;
				case "ActionFilterTriggerEntity": return EntityType.ActionFilterTriggerEntity;
				case "ActionQueueEntity": return EntityType.ActionQueueEntity;
				case "ObjectReferenceEntity": return EntityType.ObjectReferenceEntity;
				case "NoteEntity": return EntityType.NoteEntity;
				case "PrintResultEntity": return EntityType.PrintResultEntity;
				case "OtherShipmentEntity": return EntityType.OtherShipmentEntity;
				case "EmailOutboundRelationEntity": return EntityType.EmailOutboundRelationEntity;
				case "AuditChangeDetailEntity": return EntityType.AuditChangeDetailEntity;
				case "AuditChangeEntity": return EntityType.AuditChangeEntity;
				case "ObjectLabelEntity": return EntityType.ObjectLabelEntity;
				case "ActionQueueStepEntity": return EntityType.ActionQueueStepEntity;
				case "ShippingOriginEntity": return EntityType.ShippingOriginEntity;
				case "ShipmentCustomsItemEntity": return EntityType.ShipmentCustomsItemEntity;
				case "UspsAccountEntity": return EntityType.UspsAccountEntity;
				case "ChannelAdvisorStoreEntity": return EntityType.ChannelAdvisorStoreEntity;
				case "ChannelAdvisorOrderItemEntity": return EntityType.ChannelAdvisorOrderItemEntity;
				case "ChannelAdvisorOrderEntity": return EntityType.ChannelAdvisorOrderEntity;
				case "DimensionsProfileEntity": return EntityType.DimensionsProfileEntity;
				case "UspsShipmentEntity": return EntityType.UspsShipmentEntity;
				case "PostalShipmentEntity": return EntityType.PostalShipmentEntity;
				case "ShippingProfileEntity": return EntityType.ShippingProfileEntity;
				case "FedExAccountEntity": return EntityType.FedExAccountEntity;
				case "UpsAccountEntity": return EntityType.UpsAccountEntity;
				case "PostalProfileEntity": return EntityType.PostalProfileEntity;
				case "UspsProfileEntity": return EntityType.UspsProfileEntity;
				case "OtherProfileEntity": return EntityType.OtherProfileEntity;
				case "ShippingPrintOutputRuleEntity": return EntityType.ShippingPrintOutputRuleEntity;
				case "ShippingPrintOutputEntity": return EntityType.ShippingPrintOutputEntity;
				case "ShippingDefaultsRuleEntity": return EntityType.ShippingDefaultsRuleEntity;
				case "ShippingProviderRuleEntity": return EntityType.ShippingProviderRuleEntity;
				case "InfopiaStoreEntity": return EntityType.InfopiaStoreEntity;
				case "InfopiaOrderItemEntity": return EntityType.InfopiaOrderItemEntity;
				case "FedExShipmentEntity": return EntityType.FedExShipmentEntity;
				case "FedExProfileEntity": return EntityType.FedExProfileEntity;
				case "FedExPackageEntity": return EntityType.FedExPackageEntity;
				case "PayPalStoreEntity": return EntityType.PayPalStoreEntity;
				case "PayPalOrderEntity": return EntityType.PayPalOrderEntity;
				case "FedExProfilePackageEntity": return EntityType.FedExProfilePackageEntity;
				case "AmazonStoreEntity": return EntityType.AmazonStoreEntity;
				case "AmazonOrderEntity": return EntityType.AmazonOrderEntity;
				case "AmazonASINEntity": return EntityType.AmazonASINEntity;
				case "UpsPackageEntity": return EntityType.UpsPackageEntity;
				case "UpsProfileEntity": return EntityType.UpsProfileEntity;
				case "UpsProfilePackageEntity": return EntityType.UpsProfilePackageEntity;
				case "UpsShipmentEntity": return EntityType.UpsShipmentEntity;
				case "AmazonOrderItemEntity": return EntityType.AmazonOrderItemEntity;
				case "FedExEndOfDayCloseEntity": return EntityType.FedExEndOfDayCloseEntity;
				case "EndiciaAccountEntity": return EntityType.EndiciaAccountEntity;
				case "EndiciaProfileEntity": return EntityType.EndiciaProfileEntity;
				case "EndiciaShipmentEntity": return EntityType.EndiciaShipmentEntity;
				case "EndiciaScanFormEntity": return EntityType.EndiciaScanFormEntity;
				case "WorldShipPackageEntity": return EntityType.WorldShipPackageEntity;
				case "WorldShipProcessedEntity": return EntityType.WorldShipProcessedEntity;
				case "WorldShipShipmentEntity": return EntityType.WorldShipShipmentEntity;
				case "WorldShipGoodsEntity": return EntityType.WorldShipGoodsEntity;
				case "EbayStoreEntity": return EntityType.EbayStoreEntity;
				case "EbayOrderEntity": return EntityType.EbayOrderEntity;
				case "EbayOrderItemEntity": return EntityType.EbayOrderItemEntity;
				case "MivaStoreEntity": return EntityType.MivaStoreEntity;
				case "MarketplaceAdvisorOrderEntity": return EntityType.MarketplaceAdvisorOrderEntity;
				case "MarketplaceAdvisorStoreEntity": return EntityType.MarketplaceAdvisorStoreEntity;
				case "YahooOrderEntity": return EntityType.YahooOrderEntity;
				case "YahooOrderItemEntity": return EntityType.YahooOrderItemEntity;
				case "YahooStoreEntity": return EntityType.YahooStoreEntity;
				case "YahooProductEntity": return EntityType.YahooProductEntity;
				case "MagentoOrderEntity": return EntityType.MagentoOrderEntity;
				case "MagentoStoreEntity": return EntityType.MagentoStoreEntity;
				case "ProStoresStoreEntity": return EntityType.ProStoresStoreEntity;
				case "ProStoresOrderEntity": return EntityType.ProStoresOrderEntity;
				case "AmeriCommerceStoreEntity": return EntityType.AmeriCommerceStoreEntity;
				case "NetworkSolutionsStoreEntity": return EntityType.NetworkSolutionsStoreEntity;
				case "NetworkSolutionsOrderEntity": return EntityType.NetworkSolutionsOrderEntity;
				case "VolusionStoreEntity": return EntityType.VolusionStoreEntity;
				case "OrderMotionStoreEntity": return EntityType.OrderMotionStoreEntity;
				case "OrderMotionOrderEntity": return EntityType.OrderMotionOrderEntity;
				case "ClickCartProOrderEntity": return EntityType.ClickCartProOrderEntity;
				case "CommerceInterfaceOrderEntity": return EntityType.CommerceInterfaceOrderEntity;
				case "SearchEntity": return EntityType.SearchEntity;
				case "GenericFileStoreEntity": return EntityType.GenericFileStoreEntity;
				case "GenericModuleStoreEntity": return EntityType.GenericModuleStoreEntity;
				case "FtpAccountEntity": return EntityType.FtpAccountEntity;
				case "MivaOrderItemAttributeEntity": return EntityType.MivaOrderItemAttributeEntity;
				case "NeweggStoreEntity": return EntityType.NeweggStoreEntity;
				case "NeweggOrderItemEntity": return EntityType.NeweggOrderItemEntity;
				case "NeweggOrderEntity": return EntityType.NeweggOrderEntity;
				case "ShopifyStoreEntity": return EntityType.ShopifyStoreEntity;
				case "EtsyOrderEntity": return EntityType.EtsyOrderEntity;
				case "ShopifyOrderEntity": return EntityType.ShopifyOrderEntity;
				case "ShopifyOrderItemEntity": return EntityType.ShopifyOrderItemEntity;
				case "EtsyStoreEntity": return EntityType.EtsyStoreEntity;
				case "BuyDotComStoreEntity": return EntityType.BuyDotComStoreEntity;
				case "BuyDotComOrderItemEntity": return EntityType.BuyDotComOrderItemEntity;
				case "ThreeDCartStoreEntity": return EntityType.ThreeDCartStoreEntity;
				case "ThreeDCartOrderItemEntity": return EntityType.ThreeDCartOrderItemEntity;
				case "SearsOrderEntity": return EntityType.SearsOrderEntity;
				case "SearsStoreEntity": return EntityType.SearsStoreEntity;
				case "SearsOrderItemEntity": return EntityType.SearsOrderItemEntity;
				case "OnTracAccountEntity": return EntityType.OnTracAccountEntity;
				case "BigCommerceStoreEntity": return EntityType.BigCommerceStoreEntity;
				case "OnTracProfileEntity": return EntityType.OnTracProfileEntity;
				case "BigCommerceOrderItemEntity": return EntityType.BigCommerceOrderItemEntity;
				case "OnTracShipmentEntity": return EntityType.OnTracShipmentEntity;
				case "UspsScanFormEntity": return EntityType.UspsScanFormEntity;
				case "IParcelShipmentEntity": return EntityType.IParcelShipmentEntity;
				case "IParcelProfileEntity": return EntityType.IParcelProfileEntity;
				case "IParcelAccountEntity": return EntityType.IParcelAccountEntity;
				case "IParcelProfilePackageEntity": return EntityType.IParcelProfilePackageEntity;
				case "IParcelPackageEntity": return EntityType.IParcelPackageEntity;
				case "ScanFormBatchEntity": return EntityType.ScanFormBatchEntity;
				case "ServiceStatusEntity": return EntityType.ServiceStatusEntity;
				case "ActionQueueSelectionEntity": return EntityType.ActionQueueSelectionEntity;
				case "ShippingSettingsEntity": return EntityType.ShippingSettingsEntity;
				case "BestRateShipmentEntity": return EntityType.BestRateShipmentEntity;
				case "BestRateProfileEntity": return EntityType.BestRateProfileEntity;
				case "ShipSenseKnowledgebaseEntity": return EntityType.ShipSenseKnowledgebaseEntity;
				case "InsurancePolicyEntity": return EntityType.InsurancePolicyEntity;
				case "EbayCombinedOrderRelationEntity": return EntityType.EbayCombinedOrderRelationEntity;
				case "ValidatedAddressEntity": return EntityType.ValidatedAddressEntity;
				case "GrouponOrderEntity": return EntityType.GrouponOrderEntity;
				case "GrouponOrderItemEntity": return EntityType.GrouponOrderItemEntity;
				case "GrouponStoreEntity": return EntityType.GrouponStoreEntity;
				case "ExcludedServiceTypeEntity": return EntityType.ExcludedServiceTypeEntity;
				case "ExcludedPackageTypeEntity": return EntityType.ExcludedPackageTypeEntity;
				case "LemonStandStoreEntity": return EntityType.LemonStandStoreEntity;
				case "LemonStandOrderEntity": return EntityType.LemonStandOrderEntity;
				case "LemonStandOrderItemEntity": return EntityType.LemonStandOrderItemEntity;
				case "AmazonShipmentEntity": return EntityType.AmazonShipmentEntity;
				case "AmazonProfileEntity": return EntityType.AmazonProfileEntity;
				case "SparkPayStoreEntity": return EntityType.SparkPayStoreEntity;
				case "OdbcStoreEntity": return EntityType.OdbcStoreEntity;
				}
			
			throw new ArgumentException(string.Format("Entity type name '{0}' is not valid.", entityTypeName));
		}

        /// <summary>
        /// Gets the entity type name based on the the given EntityType.
        /// </summary>
		public static string GetEntityTypeName(EntityType entityType)
		{
			IEntity2 entity = GeneralEntityFactory.Create(entityType);
			return entity.LLBLGenProEntityName;
		}
		
		/// <summary>
        /// Gets the System.Type based on the the given entity type name.
        /// </summary>
		public static Type GetSystemType(string entityTypeName)
		{
			return GetSystemType(GetEntityType(entityTypeName));
		}
		
        /// <summary>
        /// Gets the System.Type based on the the given EntityType.
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
				case EntityType.StoreEntity: return new StoreRelations();
				case EntityType.OrderEntity: return new OrderRelations();
				case EntityType.OrderItemEntity: return new OrderItemRelations();
				case EntityType.ShopSiteStoreEntity: return new ShopSiteStoreRelations();
				case EntityType.OrderItemAttributeEntity: return new OrderItemAttributeRelations();
				case EntityType.ChannelAdvisorStoreEntity: return new ChannelAdvisorStoreRelations();
				case EntityType.ChannelAdvisorOrderItemEntity: return new ChannelAdvisorOrderItemRelations();
				case EntityType.ChannelAdvisorOrderEntity: return new ChannelAdvisorOrderRelations();
				case EntityType.InfopiaStoreEntity: return new InfopiaStoreRelations();
				case EntityType.InfopiaOrderItemEntity: return new InfopiaOrderItemRelations();
				case EntityType.PayPalStoreEntity: return new PayPalStoreRelations();
				case EntityType.PayPalOrderEntity: return new PayPalOrderRelations();
				case EntityType.AmazonStoreEntity: return new AmazonStoreRelations();
				case EntityType.AmazonOrderEntity: return new AmazonOrderRelations();
				case EntityType.AmazonOrderItemEntity: return new AmazonOrderItemRelations();
				case EntityType.EbayStoreEntity: return new EbayStoreRelations();
				case EntityType.EbayOrderEntity: return new EbayOrderRelations();
				case EntityType.EbayOrderItemEntity: return new EbayOrderItemRelations();
				case EntityType.MivaStoreEntity: return new MivaStoreRelations();
				case EntityType.MarketplaceAdvisorOrderEntity: return new MarketplaceAdvisorOrderRelations();
				case EntityType.MarketplaceAdvisorStoreEntity: return new MarketplaceAdvisorStoreRelations();
				case EntityType.YahooOrderEntity: return new YahooOrderRelations();
				case EntityType.YahooOrderItemEntity: return new YahooOrderItemRelations();
				case EntityType.YahooStoreEntity: return new YahooStoreRelations();
				case EntityType.MagentoOrderEntity: return new MagentoOrderRelations();
				case EntityType.MagentoStoreEntity: return new MagentoStoreRelations();
				case EntityType.ProStoresStoreEntity: return new ProStoresStoreRelations();
				case EntityType.ProStoresOrderEntity: return new ProStoresOrderRelations();
				case EntityType.AmeriCommerceStoreEntity: return new AmeriCommerceStoreRelations();
				case EntityType.NetworkSolutionsStoreEntity: return new NetworkSolutionsStoreRelations();
				case EntityType.NetworkSolutionsOrderEntity: return new NetworkSolutionsOrderRelations();
				case EntityType.VolusionStoreEntity: return new VolusionStoreRelations();
				case EntityType.OrderMotionStoreEntity: return new OrderMotionStoreRelations();
				case EntityType.OrderMotionOrderEntity: return new OrderMotionOrderRelations();
				case EntityType.ClickCartProOrderEntity: return new ClickCartProOrderRelations();
				case EntityType.CommerceInterfaceOrderEntity: return new CommerceInterfaceOrderRelations();
				case EntityType.GenericFileStoreEntity: return new GenericFileStoreRelations();
				case EntityType.GenericModuleStoreEntity: return new GenericModuleStoreRelations();
				case EntityType.MivaOrderItemAttributeEntity: return new MivaOrderItemAttributeRelations();
				case EntityType.NeweggStoreEntity: return new NeweggStoreRelations();
				case EntityType.NeweggOrderItemEntity: return new NeweggOrderItemRelations();
				case EntityType.NeweggOrderEntity: return new NeweggOrderRelations();
				case EntityType.ShopifyStoreEntity: return new ShopifyStoreRelations();
				case EntityType.EtsyOrderEntity: return new EtsyOrderRelations();
				case EntityType.ShopifyOrderEntity: return new ShopifyOrderRelations();
				case EntityType.ShopifyOrderItemEntity: return new ShopifyOrderItemRelations();
				case EntityType.EtsyStoreEntity: return new EtsyStoreRelations();
				case EntityType.BuyDotComStoreEntity: return new BuyDotComStoreRelations();
				case EntityType.BuyDotComOrderItemEntity: return new BuyDotComOrderItemRelations();
				case EntityType.ThreeDCartStoreEntity: return new ThreeDCartStoreRelations();
				case EntityType.ThreeDCartOrderItemEntity: return new ThreeDCartOrderItemRelations();
				case EntityType.SearsOrderEntity: return new SearsOrderRelations();
				case EntityType.SearsStoreEntity: return new SearsStoreRelations();
				case EntityType.SearsOrderItemEntity: return new SearsOrderItemRelations();
				case EntityType.BigCommerceStoreEntity: return new BigCommerceStoreRelations();
				case EntityType.BigCommerceOrderItemEntity: return new BigCommerceOrderItemRelations();
				case EntityType.GrouponOrderEntity: return new GrouponOrderRelations();
				case EntityType.GrouponOrderItemEntity: return new GrouponOrderItemRelations();
				case EntityType.GrouponStoreEntity: return new GrouponStoreRelations();
				case EntityType.LemonStandStoreEntity: return new LemonStandStoreRelations();
				case EntityType.LemonStandOrderEntity: return new LemonStandOrderRelations();
				case EntityType.LemonStandOrderItemEntity: return new LemonStandOrderItemRelations();
				case EntityType.SparkPayStoreEntity: return new SparkPayStoreRelations();
				case EntityType.OdbcStoreEntity: return new OdbcStoreRelations();
				}
			
			throw new ArgumentException(string.Format("Entity type '{0}' is not valid or is not a part of a TargetPerEntity hierarchy.", entityType));
		}
	}
}

 