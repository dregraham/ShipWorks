﻿///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates.NET20
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.FactoryClasses
{
	/// <summary>Generates IEntityFields2 instances for different kind of Entities.</summary>
	public partial class EntityFieldsFactory
	{
		/// <summary>Private CTor, no instantiation possible.</summary>
		private EntityFieldsFactory()
		{
		}

		/// <summary>General factory entrance method which will return an EntityFields object with the format generated by the factory specified</summary>
		/// <param name="relatedEntityType">The type of entity the fields are for</param>
		/// <returns>The IEntityFields2 instance requested</returns>
		public static IEntityFields2 CreateEntityFieldsObject(ShipWorks.Data.Model.EntityType relatedEntityType)
		{
			IEntityFields2 fieldsToReturn=null;
			IInheritanceInfoProvider inheritanceProvider = InheritanceInfoProviderSingleton.GetInstance();
			IFieldInfoProvider fieldProvider = FieldInfoProviderSingleton.GetInstance();
			switch(relatedEntityType)
			{
				case ShipWorks.Data.Model.EntityType.ActionEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ActionEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ActionFilterTriggerEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ActionFilterTriggerEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ActionQueueEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ActionQueueEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ActionQueueSelectionEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ActionQueueSelectionEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ActionQueueStepEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ActionQueueStepEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ActionTaskEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ActionTaskEntity");
					break;
				case ShipWorks.Data.Model.EntityType.AmazonASINEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "AmazonASINEntity");
					break;
				case ShipWorks.Data.Model.EntityType.AmazonOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "AmazonOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.AmazonOrderItemEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "AmazonOrderItemEntity");
					break;
				case ShipWorks.Data.Model.EntityType.AmazonStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "AmazonStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.AmeriCommerceStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "AmeriCommerceStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.AuditEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "AuditEntity");
					break;
				case ShipWorks.Data.Model.EntityType.AuditChangeEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "AuditChangeEntity");
					break;
				case ShipWorks.Data.Model.EntityType.AuditChangeDetailEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "AuditChangeDetailEntity");
					break;
				case ShipWorks.Data.Model.EntityType.BestRateProfileEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "BestRateProfileEntity");
					break;
				case ShipWorks.Data.Model.EntityType.BestRateShipmentEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "BestRateShipmentEntity");
					break;
				case ShipWorks.Data.Model.EntityType.BigCommerceOrderItemEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "BigCommerceOrderItemEntity");
					break;
				case ShipWorks.Data.Model.EntityType.BigCommerceStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "BigCommerceStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.BuyDotComOrderItemEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "BuyDotComOrderItemEntity");
					break;
				case ShipWorks.Data.Model.EntityType.BuyDotComStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "BuyDotComStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ChannelAdvisorOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ChannelAdvisorOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ChannelAdvisorOrderItemEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ChannelAdvisorOrderItemEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ChannelAdvisorStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ChannelAdvisorStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ClickCartProOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ClickCartProOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.CommerceInterfaceOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "CommerceInterfaceOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ComputerEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ComputerEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ConfigurationEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ConfigurationEntity");
					break;
				case ShipWorks.Data.Model.EntityType.CustomerEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "CustomerEntity");
					break;
				case ShipWorks.Data.Model.EntityType.DimensionsProfileEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "DimensionsProfileEntity");
					break;
				case ShipWorks.Data.Model.EntityType.DownloadEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "DownloadEntity");
					break;
				case ShipWorks.Data.Model.EntityType.DownloadDetailEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "DownloadDetailEntity");
					break;
				case ShipWorks.Data.Model.EntityType.EbayCombinedOrderRelationEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "EbayCombinedOrderRelationEntity");
					break;
				case ShipWorks.Data.Model.EntityType.EbayOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "EbayOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.EbayOrderItemEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "EbayOrderItemEntity");
					break;
				case ShipWorks.Data.Model.EntityType.EbayStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "EbayStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.EmailAccountEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "EmailAccountEntity");
					break;
				case ShipWorks.Data.Model.EntityType.EmailOutboundEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "EmailOutboundEntity");
					break;
				case ShipWorks.Data.Model.EntityType.EmailOutboundRelationEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "EmailOutboundRelationEntity");
					break;
				case ShipWorks.Data.Model.EntityType.EndiciaAccountEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "EndiciaAccountEntity");
					break;
				case ShipWorks.Data.Model.EntityType.EndiciaProfileEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "EndiciaProfileEntity");
					break;
				case ShipWorks.Data.Model.EntityType.EndiciaScanFormEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "EndiciaScanFormEntity");
					break;
				case ShipWorks.Data.Model.EntityType.EndiciaShipmentEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "EndiciaShipmentEntity");
					break;
				case ShipWorks.Data.Model.EntityType.EtsyOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "EtsyOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.EtsyStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "EtsyStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.FedExAccountEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "FedExAccountEntity");
					break;
				case ShipWorks.Data.Model.EntityType.FedExEndOfDayCloseEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "FedExEndOfDayCloseEntity");
					break;
				case ShipWorks.Data.Model.EntityType.FedExPackageEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "FedExPackageEntity");
					break;
				case ShipWorks.Data.Model.EntityType.FedExProfileEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "FedExProfileEntity");
					break;
				case ShipWorks.Data.Model.EntityType.FedExProfilePackageEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "FedExProfilePackageEntity");
					break;
				case ShipWorks.Data.Model.EntityType.FedExShipmentEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "FedExShipmentEntity");
					break;
				case ShipWorks.Data.Model.EntityType.FilterEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "FilterEntity");
					break;
				case ShipWorks.Data.Model.EntityType.FilterLayoutEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "FilterLayoutEntity");
					break;
				case ShipWorks.Data.Model.EntityType.FilterNodeEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "FilterNodeEntity");
					break;
				case ShipWorks.Data.Model.EntityType.FilterNodeColumnSettingsEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "FilterNodeColumnSettingsEntity");
					break;
				case ShipWorks.Data.Model.EntityType.FilterNodeContentEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "FilterNodeContentEntity");
					break;
				case ShipWorks.Data.Model.EntityType.FilterNodeContentDetailEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "FilterNodeContentDetailEntity");
					break;
				case ShipWorks.Data.Model.EntityType.FilterSequenceEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "FilterSequenceEntity");
					break;
				case ShipWorks.Data.Model.EntityType.FtpAccountEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "FtpAccountEntity");
					break;
				case ShipWorks.Data.Model.EntityType.GenericFileStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "GenericFileStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.GenericModuleStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "GenericModuleStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.GridColumnFormatEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "GridColumnFormatEntity");
					break;
				case ShipWorks.Data.Model.EntityType.GridColumnLayoutEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "GridColumnLayoutEntity");
					break;
				case ShipWorks.Data.Model.EntityType.GridColumnPositionEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "GridColumnPositionEntity");
					break;
				case ShipWorks.Data.Model.EntityType.GrouponOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "GrouponOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.GrouponOrderItemEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "GrouponOrderItemEntity");
					break;
				case ShipWorks.Data.Model.EntityType.GrouponStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "GrouponStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.InfopiaOrderItemEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "InfopiaOrderItemEntity");
					break;
				case ShipWorks.Data.Model.EntityType.InfopiaStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "InfopiaStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.InsurancePolicyEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "InsurancePolicyEntity");
					break;
				case ShipWorks.Data.Model.EntityType.IParcelAccountEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "IParcelAccountEntity");
					break;
				case ShipWorks.Data.Model.EntityType.IParcelPackageEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "IParcelPackageEntity");
					break;
				case ShipWorks.Data.Model.EntityType.IParcelProfileEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "IParcelProfileEntity");
					break;
				case ShipWorks.Data.Model.EntityType.IParcelProfilePackageEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "IParcelProfilePackageEntity");
					break;
				case ShipWorks.Data.Model.EntityType.IParcelShipmentEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "IParcelShipmentEntity");
					break;
				case ShipWorks.Data.Model.EntityType.LabelSheetEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "LabelSheetEntity");
					break;
				case ShipWorks.Data.Model.EntityType.MagentoOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "MagentoOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.MagentoStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "MagentoStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.MarketplaceAdvisorOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "MarketplaceAdvisorOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.MarketplaceAdvisorStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "MarketplaceAdvisorStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.MivaOrderItemAttributeEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "MivaOrderItemAttributeEntity");
					break;
				case ShipWorks.Data.Model.EntityType.MivaStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "MivaStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.NetworkSolutionsOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "NetworkSolutionsOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.NetworkSolutionsStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "NetworkSolutionsStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.NeweggOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "NeweggOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.NeweggOrderItemEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "NeweggOrderItemEntity");
					break;
				case ShipWorks.Data.Model.EntityType.NeweggStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "NeweggStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.NoteEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "NoteEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ObjectLabelEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ObjectLabelEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ObjectReferenceEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ObjectReferenceEntity");
					break;
				case ShipWorks.Data.Model.EntityType.OnTracAccountEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "OnTracAccountEntity");
					break;
				case ShipWorks.Data.Model.EntityType.OnTracProfileEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "OnTracProfileEntity");
					break;
				case ShipWorks.Data.Model.EntityType.OnTracShipmentEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "OnTracShipmentEntity");
					break;
				case ShipWorks.Data.Model.EntityType.OrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "OrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.OrderChargeEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "OrderChargeEntity");
					break;
				case ShipWorks.Data.Model.EntityType.OrderItemEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "OrderItemEntity");
					break;
				case ShipWorks.Data.Model.EntityType.OrderItemAttributeEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "OrderItemAttributeEntity");
					break;
				case ShipWorks.Data.Model.EntityType.OrderMotionOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "OrderMotionOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.OrderMotionStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "OrderMotionStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.OrderPaymentDetailEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "OrderPaymentDetailEntity");
					break;
				case ShipWorks.Data.Model.EntityType.OtherProfileEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "OtherProfileEntity");
					break;
				case ShipWorks.Data.Model.EntityType.OtherShipmentEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "OtherShipmentEntity");
					break;
				case ShipWorks.Data.Model.EntityType.PayPalOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "PayPalOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.PayPalStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "PayPalStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.PermissionEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "PermissionEntity");
					break;
				case ShipWorks.Data.Model.EntityType.PostalProfileEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "PostalProfileEntity");
					break;
				case ShipWorks.Data.Model.EntityType.PostalShipmentEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "PostalShipmentEntity");
					break;
				case ShipWorks.Data.Model.EntityType.PrintResultEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "PrintResultEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ProStoresOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ProStoresOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ProStoresStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ProStoresStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ResourceEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ResourceEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ScanFormBatchEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ScanFormBatchEntity");
					break;
				case ShipWorks.Data.Model.EntityType.SearchEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "SearchEntity");
					break;
				case ShipWorks.Data.Model.EntityType.SearsOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "SearsOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.SearsOrderItemEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "SearsOrderItemEntity");
					break;
				case ShipWorks.Data.Model.EntityType.SearsStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "SearsStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ServerMessageEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ServerMessageEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ServerMessageSignoffEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ServerMessageSignoffEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ServiceStatusEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ServiceStatusEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ShipmentEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ShipmentEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ShipmentCustomsItemEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ShipmentCustomsItemEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ShippingDefaultsRuleEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ShippingDefaultsRuleEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ShippingOriginEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ShippingOriginEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ShippingPrintOutputEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ShippingPrintOutputEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ShippingPrintOutputRuleEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ShippingPrintOutputRuleEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ShippingProfileEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ShippingProfileEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ShippingProviderRuleEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ShippingProviderRuleEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ShippingSettingsEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ShippingSettingsEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ShipSenseKnowledgebaseEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ShipSenseKnowledgebaseEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ShopifyOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ShopifyOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ShopifyOrderItemEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ShopifyOrderItemEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ShopifyStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ShopifyStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ShopSiteStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ShopSiteStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.StampsAccountEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "StampsAccountEntity");
					break;
				case ShipWorks.Data.Model.EntityType.StampsProfileEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "StampsProfileEntity");
					break;
				case ShipWorks.Data.Model.EntityType.StampsScanFormEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "StampsScanFormEntity");
					break;
				case ShipWorks.Data.Model.EntityType.StampsShipmentEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "StampsShipmentEntity");
					break;
				case ShipWorks.Data.Model.EntityType.StatusPresetEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "StatusPresetEntity");
					break;
				case ShipWorks.Data.Model.EntityType.StoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "StoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.SystemDataEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "SystemDataEntity");
					break;
				case ShipWorks.Data.Model.EntityType.TemplateEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "TemplateEntity");
					break;
				case ShipWorks.Data.Model.EntityType.TemplateComputerSettingsEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "TemplateComputerSettingsEntity");
					break;
				case ShipWorks.Data.Model.EntityType.TemplateFolderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "TemplateFolderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.TemplateStoreSettingsEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "TemplateStoreSettingsEntity");
					break;
				case ShipWorks.Data.Model.EntityType.TemplateUserSettingsEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "TemplateUserSettingsEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ThreeDCartOrderItemEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ThreeDCartOrderItemEntity");
					break;
				case ShipWorks.Data.Model.EntityType.ThreeDCartStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "ThreeDCartStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.UpsAccountEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "UpsAccountEntity");
					break;
				case ShipWorks.Data.Model.EntityType.UpsPackageEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "UpsPackageEntity");
					break;
				case ShipWorks.Data.Model.EntityType.UpsProfileEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "UpsProfileEntity");
					break;
				case ShipWorks.Data.Model.EntityType.UpsProfilePackageEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "UpsProfilePackageEntity");
					break;
				case ShipWorks.Data.Model.EntityType.UpsShipmentEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "UpsShipmentEntity");
					break;
				case ShipWorks.Data.Model.EntityType.UserEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "UserEntity");
					break;
				case ShipWorks.Data.Model.EntityType.UserColumnSettingsEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "UserColumnSettingsEntity");
					break;
				case ShipWorks.Data.Model.EntityType.UserSettingsEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "UserSettingsEntity");
					break;
				case ShipWorks.Data.Model.EntityType.VersionSignoffEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "VersionSignoffEntity");
					break;
				case ShipWorks.Data.Model.EntityType.VolusionStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "VolusionStoreEntity");
					break;
				case ShipWorks.Data.Model.EntityType.WorldShipGoodsEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "WorldShipGoodsEntity");
					break;
				case ShipWorks.Data.Model.EntityType.WorldShipPackageEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "WorldShipPackageEntity");
					break;
				case ShipWorks.Data.Model.EntityType.WorldShipProcessedEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "WorldShipProcessedEntity");
					break;
				case ShipWorks.Data.Model.EntityType.WorldShipShipmentEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "WorldShipShipmentEntity");
					break;
				case ShipWorks.Data.Model.EntityType.YahooOrderEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "YahooOrderEntity");
					break;
				case ShipWorks.Data.Model.EntityType.YahooOrderItemEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "YahooOrderItemEntity");
					break;
				case ShipWorks.Data.Model.EntityType.YahooProductEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "YahooProductEntity");
					break;
				case ShipWorks.Data.Model.EntityType.YahooStoreEntity:
					fieldsToReturn = fieldProvider.GetEntityFields(inheritanceProvider, "YahooStoreEntity");
					break;
			}
			return fieldsToReturn;
		}
		
		/// <summary>General method which will return an array of IEntityFieldCore objects, used by the InheritanceInfoProvider. Only the fields defined in the entity are returned, no inherited fields.</summary>
		/// <param name="entityName">the name of the entity to get the fields for. Example: "CustomerEntity"</param>
		/// <returns>array of IEntityFieldCore fields, defined in the entity with the name specified</returns>
		internal static IEntityFieldCore[] CreateFields(string entityName)
		{
			IFieldInfoProvider fieldProvider = FieldInfoProviderSingleton.GetInstance();
			return fieldProvider.GetEntityFieldsArray(entityName);
		}
		



		#region Included Code

		#endregion
	}
}
