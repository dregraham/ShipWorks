///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 5.0
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates
// Templates vendor: Solutions Design.
//////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model
{
	/// <summary>Singleton implementation of the PersistenceInfoProvider. This class is the singleton wrapper through which the actual instance is retrieved.</summary>
	/// <remarks>It uses a single instance of an internal class. The access isn't marked with locks as the PersistenceInfoProviderBase class is threadsafe.</remarks>
	internal static class PersistenceInfoProviderSingleton
	{
		#region Class Member Declarations
		private static readonly IPersistenceInfoProvider _providerInstance = new PersistenceInfoProviderCore();
		#endregion

		/// <summary>Dummy static constructor to make sure threadsafe initialization is performed.</summary>
		static PersistenceInfoProviderSingleton()
		{
		}

		/// <summary>Gets the singleton instance of the PersistenceInfoProviderCore</summary>
		/// <returns>Instance of the PersistenceInfoProvider.</returns>
		public static IPersistenceInfoProvider GetInstance()
		{
			return _providerInstance;
		}
	}

	/// <summary>Actual implementation of the PersistenceInfoProvider. Used by singleton wrapper.</summary>
	internal class PersistenceInfoProviderCore : PersistenceInfoProviderBase
	{
		/// <summary>Initializes a new instance of the <see cref="PersistenceInfoProviderCore"/> class.</summary>
		internal PersistenceInfoProviderCore()
		{
			Init();
		}

		/// <summary>Method which initializes the internal datastores with the structure of hierarchical types.</summary>
		private void Init()
		{
			this.InitClass(176);
			InitActionEntityMappings();
			InitActionFilterTriggerEntityMappings();
			InitActionQueueEntityMappings();
			InitActionQueueSelectionEntityMappings();
			InitActionQueueStepEntityMappings();
			InitActionTaskEntityMappings();
			InitAmazonASINEntityMappings();
			InitAmazonOrderEntityMappings();
			InitAmazonOrderItemEntityMappings();
			InitAmazonProfileEntityMappings();
			InitAmazonShipmentEntityMappings();
			InitAmazonStoreEntityMappings();
			InitAmeriCommerceStoreEntityMappings();
			InitAuditEntityMappings();
			InitAuditChangeEntityMappings();
			InitAuditChangeDetailEntityMappings();
			InitBestRateProfileEntityMappings();
			InitBestRateShipmentEntityMappings();
			InitBigCommerceOrderItemEntityMappings();
			InitBigCommerceStoreEntityMappings();
			InitBuyDotComOrderItemEntityMappings();
			InitBuyDotComStoreEntityMappings();
			InitChannelAdvisorOrderEntityMappings();
			InitChannelAdvisorOrderItemEntityMappings();
			InitChannelAdvisorStoreEntityMappings();
			InitClickCartProOrderEntityMappings();
			InitCommerceInterfaceOrderEntityMappings();
			InitComputerEntityMappings();
			InitConfigurationEntityMappings();
			InitCustomerEntityMappings();
			InitDimensionsProfileEntityMappings();
			InitDownloadEntityMappings();
			InitDownloadDetailEntityMappings();
			InitEbayCombinedOrderRelationEntityMappings();
			InitEbayOrderEntityMappings();
			InitEbayOrderItemEntityMappings();
			InitEbayStoreEntityMappings();
			InitEmailAccountEntityMappings();
			InitEmailOutboundEntityMappings();
			InitEmailOutboundRelationEntityMappings();
			InitEndiciaAccountEntityMappings();
			InitEndiciaProfileEntityMappings();
			InitEndiciaScanFormEntityMappings();
			InitEndiciaShipmentEntityMappings();
			InitEtsyOrderEntityMappings();
			InitEtsyStoreEntityMappings();
			InitExcludedPackageTypeEntityMappings();
			InitExcludedServiceTypeEntityMappings();
			InitFedExAccountEntityMappings();
			InitFedExEndOfDayCloseEntityMappings();
			InitFedExPackageEntityMappings();
			InitFedExProfileEntityMappings();
			InitFedExProfilePackageEntityMappings();
			InitFedExShipmentEntityMappings();
			InitFilterEntityMappings();
			InitFilterLayoutEntityMappings();
			InitFilterNodeEntityMappings();
			InitFilterNodeColumnSettingsEntityMappings();
			InitFilterNodeContentEntityMappings();
			InitFilterNodeContentDetailEntityMappings();
			InitFilterSequenceEntityMappings();
			InitFtpAccountEntityMappings();
			InitGenericFileStoreEntityMappings();
			InitGenericModuleStoreEntityMappings();
			InitGridColumnFormatEntityMappings();
			InitGridColumnLayoutEntityMappings();
			InitGridColumnPositionEntityMappings();
			InitGrouponOrderEntityMappings();
			InitGrouponOrderItemEntityMappings();
			InitGrouponStoreEntityMappings();
			InitInfopiaOrderItemEntityMappings();
			InitInfopiaStoreEntityMappings();
			InitInsurancePolicyEntityMappings();
			InitIParcelAccountEntityMappings();
			InitIParcelPackageEntityMappings();
			InitIParcelProfileEntityMappings();
			InitIParcelProfilePackageEntityMappings();
			InitIParcelShipmentEntityMappings();
			InitLabelSheetEntityMappings();
			InitLemonStandOrderEntityMappings();
			InitLemonStandOrderItemEntityMappings();
			InitLemonStandStoreEntityMappings();
			InitMagentoOrderEntityMappings();
			InitMagentoStoreEntityMappings();
			InitMarketplaceAdvisorOrderEntityMappings();
			InitMarketplaceAdvisorStoreEntityMappings();
			InitMivaOrderItemAttributeEntityMappings();
			InitMivaStoreEntityMappings();
			InitNetworkSolutionsOrderEntityMappings();
			InitNetworkSolutionsStoreEntityMappings();
			InitNeweggOrderEntityMappings();
			InitNeweggOrderItemEntityMappings();
			InitNeweggStoreEntityMappings();
			InitNoteEntityMappings();
			InitObjectLabelEntityMappings();
			InitObjectReferenceEntityMappings();
			InitOdbcStoreEntityMappings();
			InitOnTracAccountEntityMappings();
			InitOnTracProfileEntityMappings();
			InitOnTracShipmentEntityMappings();
			InitOrderEntityMappings();
			InitOrderChargeEntityMappings();
			InitOrderItemEntityMappings();
			InitOrderItemAttributeEntityMappings();
			InitOrderMotionOrderEntityMappings();
			InitOrderMotionStoreEntityMappings();
			InitOrderPaymentDetailEntityMappings();
			InitOtherProfileEntityMappings();
			InitOtherShipmentEntityMappings();
			InitPayPalOrderEntityMappings();
			InitPayPalStoreEntityMappings();
			InitPermissionEntityMappings();
			InitPostalProfileEntityMappings();
			InitPostalShipmentEntityMappings();
			InitPrintResultEntityMappings();
			InitProStoresOrderEntityMappings();
			InitProStoresStoreEntityMappings();
			InitResourceEntityMappings();
			InitScanFormBatchEntityMappings();
			InitSearchEntityMappings();
			InitSearsOrderEntityMappings();
			InitSearsOrderItemEntityMappings();
			InitSearsStoreEntityMappings();
			InitServerMessageEntityMappings();
			InitServerMessageSignoffEntityMappings();
			InitServiceStatusEntityMappings();
			InitShipmentEntityMappings();
			InitShipmentCustomsItemEntityMappings();
			InitShippingDefaultsRuleEntityMappings();
			InitShippingOriginEntityMappings();
			InitShippingPrintOutputEntityMappings();
			InitShippingPrintOutputRuleEntityMappings();
			InitShippingProfileEntityMappings();
			InitShippingProviderRuleEntityMappings();
			InitShippingSettingsEntityMappings();
			InitShipSenseKnowledgebaseEntityMappings();
			InitShopifyOrderEntityMappings();
			InitShopifyOrderItemEntityMappings();
			InitShopifyStoreEntityMappings();
			InitShopSiteStoreEntityMappings();
			InitSparkPayStoreEntityMappings();
			InitStatusPresetEntityMappings();
			InitStoreEntityMappings();
			InitSystemDataEntityMappings();
			InitTemplateEntityMappings();
			InitTemplateComputerSettingsEntityMappings();
			InitTemplateFolderEntityMappings();
			InitTemplateStoreSettingsEntityMappings();
			InitTemplateUserSettingsEntityMappings();
			InitThreeDCartOrderEntityMappings();
			InitThreeDCartOrderItemEntityMappings();
			InitThreeDCartStoreEntityMappings();
			InitUpsAccountEntityMappings();
			InitUpsPackageEntityMappings();
			InitUpsProfileEntityMappings();
			InitUpsProfilePackageEntityMappings();
			InitUpsShipmentEntityMappings();
			InitUserEntityMappings();
			InitUserColumnSettingsEntityMappings();
			InitUserSettingsEntityMappings();
			InitUspsAccountEntityMappings();
			InitUspsProfileEntityMappings();
			InitUspsScanFormEntityMappings();
			InitUspsShipmentEntityMappings();
			InitValidatedAddressEntityMappings();
			InitVersionSignoffEntityMappings();
			InitVolusionStoreEntityMappings();
			InitWalmartStoreEntityMappings();
			InitWorldShipGoodsEntityMappings();
			InitWorldShipPackageEntityMappings();
			InitWorldShipProcessedEntityMappings();
			InitWorldShipShipmentEntityMappings();
			InitYahooOrderEntityMappings();
			InitYahooOrderItemEntityMappings();
			InitYahooProductEntityMappings();
			InitYahooStoreEntityMappings();
		}

		/// <summary>Inits ActionEntity's mappings</summary>
		private void InitActionEntityMappings()
		{
			this.AddElementMapping("ActionEntity", @"ShipWorksLocal", @"dbo", "Action", 12, 0);
			this.AddElementFieldMapping("ActionEntity", "ActionID", "ActionID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ActionEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ActionEntity", "Name", "Name", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("ActionEntity", "Enabled", "Enabled", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("ActionEntity", "ComputerLimitedType", "ComputerLimitedType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("ActionEntity", "InternalComputerLimitedList", "ComputerLimitedList", false, "VarChar", 150, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("ActionEntity", "StoreLimited", "StoreLimited", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
			this.AddElementFieldMapping("ActionEntity", "InternalStoreLimitedList", "StoreLimitedList", false, "NVarChar", 150, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("ActionEntity", "TriggerType", "TriggerType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 8);
			this.AddElementFieldMapping("ActionEntity", "TriggerSettings", "TriggerSettings", false, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("ActionEntity", "TaskSummary", "TaskSummary", false, "NVarChar", 150, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("ActionEntity", "InternalOwner", "InternalOwner", true, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 11);
		}

		/// <summary>Inits ActionFilterTriggerEntity's mappings</summary>
		private void InitActionFilterTriggerEntityMappings()
		{
			this.AddElementMapping("ActionFilterTriggerEntity", @"ShipWorksLocal", @"dbo", "ActionFilterTrigger", 5, 0);
			this.AddElementFieldMapping("ActionFilterTriggerEntity", "ActionID", "ActionID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ActionFilterTriggerEntity", "FilterNodeID", "FilterNodeID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("ActionFilterTriggerEntity", "Direction", "Direction", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("ActionFilterTriggerEntity", "ComputerLimitedType", "ComputerLimitedType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("ActionFilterTriggerEntity", "InternalComputerLimitedList", "ComputerLimitedList", false, "VarChar", 150, 0, 0, false, "", null, typeof(System.String), 4);
		}

		/// <summary>Inits ActionQueueEntity's mappings</summary>
		private void InitActionQueueEntityMappings()
		{
			this.AddElementMapping("ActionQueueEntity", @"ShipWorksLocal", @"dbo", "ActionQueue", 15, 0);
			this.AddElementFieldMapping("ActionQueueEntity", "ActionQueueID", "ActionQueueID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ActionQueueEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ActionQueueEntity", "ActionID", "ActionID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("ActionQueueEntity", "ActionName", "ActionName", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("ActionQueueEntity", "ActionQueueType", "ActionQueueType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("ActionQueueEntity", "ActionVersion", "ActionVersion", false, "Binary", 8, 0, 0, false, "", null, typeof(System.Byte[]), 5);
			this.AddElementFieldMapping("ActionQueueEntity", "QueueVersion", "QueueVersion", false, "Binary", 8, 0, 0, false, "", null, typeof(System.Byte[]), 6);
			this.AddElementFieldMapping("ActionQueueEntity", "TriggerDate", "TriggerDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 7);
			this.AddElementFieldMapping("ActionQueueEntity", "TriggerComputerID", "TriggerComputerID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 8);
			this.AddElementFieldMapping("ActionQueueEntity", "InternalComputerLimitedList", "ComputerLimitedList", false, "VarChar", 150, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("ActionQueueEntity", "EntityID", "ObjectID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 10);
			this.AddElementFieldMapping("ActionQueueEntity", "Status", "Status", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 11);
			this.AddElementFieldMapping("ActionQueueEntity", "NextStep", "NextStep", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 12);
			this.AddElementFieldMapping("ActionQueueEntity", "ContextLock", "ContextLock", true, "NVarChar", 36, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("ActionQueueEntity", "ExtraData", "ExtraData", true, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 14);
		}

		/// <summary>Inits ActionQueueSelectionEntity's mappings</summary>
		private void InitActionQueueSelectionEntityMappings()
		{
			this.AddElementMapping("ActionQueueSelectionEntity", @"ShipWorksLocal", @"dbo", "ActionQueueSelection", 3, 0);
			this.AddElementFieldMapping("ActionQueueSelectionEntity", "ActionQueueSelectionID", "ActionQueueSelectionID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ActionQueueSelectionEntity", "ActionQueueID", "ActionQueueID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("ActionQueueSelectionEntity", "EntityID", "ObjectID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
		}

		/// <summary>Inits ActionQueueStepEntity's mappings</summary>
		private void InitActionQueueStepEntityMappings()
		{
			this.AddElementMapping("ActionQueueStepEntity", @"ShipWorksLocal", @"dbo", "ActionQueueStep", 18, 0);
			this.AddElementFieldMapping("ActionQueueStepEntity", "ActionQueueStepID", "ActionQueueStepID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ActionQueueStepEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ActionQueueStepEntity", "ActionQueueID", "ActionQueueID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("ActionQueueStepEntity", "StepStatus", "StepStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("ActionQueueStepEntity", "StepIndex", "StepIndex", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("ActionQueueStepEntity", "StepName", "StepName", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("ActionQueueStepEntity", "TaskIdentifier", "TaskIdentifier", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("ActionQueueStepEntity", "TaskSettings", "TaskSettings", false, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("ActionQueueStepEntity", "InputSource", "InputSource", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 8);
			this.AddElementFieldMapping("ActionQueueStepEntity", "InputFilterNodeID", "InputFilterNodeID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 9);
			this.AddElementFieldMapping("ActionQueueStepEntity", "FilterCondition", "FilterCondition", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 10);
			this.AddElementFieldMapping("ActionQueueStepEntity", "FilterConditionNodeID", "FilterConditionNodeID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 11);
			this.AddElementFieldMapping("ActionQueueStepEntity", "FlowSuccess", "FlowSuccess", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 12);
			this.AddElementFieldMapping("ActionQueueStepEntity", "FlowSkipped", "FlowSkipped", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 13);
			this.AddElementFieldMapping("ActionQueueStepEntity", "FlowError", "FlowError", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 14);
			this.AddElementFieldMapping("ActionQueueStepEntity", "AttemptDate", "AttemptDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 15);
			this.AddElementFieldMapping("ActionQueueStepEntity", "AttemptError", "AttemptError", false, "NVarChar", 500, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("ActionQueueStepEntity", "AttemptCount", "AttemptCount", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 17);
		}

		/// <summary>Inits ActionTaskEntity's mappings</summary>
		private void InitActionTaskEntityMappings()
		{
			this.AddElementMapping("ActionTaskEntity", @"ShipWorksLocal", @"dbo", "ActionTask", 12, 0);
			this.AddElementFieldMapping("ActionTaskEntity", "ActionTaskID", "ActionTaskID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ActionTaskEntity", "ActionID", "ActionID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("ActionTaskEntity", "TaskIdentifier", "TaskIdentifier", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("ActionTaskEntity", "TaskSettings", "TaskSettings", false, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("ActionTaskEntity", "StepIndex", "StepIndex", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("ActionTaskEntity", "InputSource", "InputSource", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("ActionTaskEntity", "InputFilterNodeID", "InputFilterNodeID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 6);
			this.AddElementFieldMapping("ActionTaskEntity", "FilterCondition", "FilterCondition", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 7);
			this.AddElementFieldMapping("ActionTaskEntity", "FilterConditionNodeID", "FilterConditionNodeID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 8);
			this.AddElementFieldMapping("ActionTaskEntity", "FlowSuccess", "FlowSuccess", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 9);
			this.AddElementFieldMapping("ActionTaskEntity", "FlowSkipped", "FlowSkipped", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 10);
			this.AddElementFieldMapping("ActionTaskEntity", "FlowError", "FlowError", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 11);
		}

		/// <summary>Inits AmazonASINEntity's mappings</summary>
		private void InitAmazonASINEntityMappings()
		{
			this.AddElementMapping("AmazonASINEntity", @"ShipWorksLocal", @"dbo", "AmazonASIN", 3, 0);
			this.AddElementFieldMapping("AmazonASINEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("AmazonASINEntity", "SKU", "SKU", false, "VarChar", 100, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("AmazonASINEntity", "AmazonASIN", "AmazonASIN", false, "VarChar", 32, 0, 0, false, "", null, typeof(System.String), 2);
		}

		/// <summary>Inits AmazonOrderEntity's mappings</summary>
		private void InitAmazonOrderEntityMappings()
		{
			this.AddElementMapping("AmazonOrderEntity", @"ShipWorksLocal", @"dbo", "AmazonOrder", 8, 0);
			this.AddElementFieldMapping("AmazonOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("AmazonOrderEntity", "AmazonOrderID", "AmazonOrderID", false, "VarChar", 32, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("AmazonOrderEntity", "AmazonCommission", "AmazonCommission", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 2);
			this.AddElementFieldMapping("AmazonOrderEntity", "FulfillmentChannel", "FulfillmentChannel", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("AmazonOrderEntity", "IsPrime", "IsPrime", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("AmazonOrderEntity", "EarliestExpectedDeliveryDate", "EarliestExpectedDeliveryDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 5);
			this.AddElementFieldMapping("AmazonOrderEntity", "LatestExpectedDeliveryDate", "LatestExpectedDeliveryDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 6);
			this.AddElementFieldMapping("AmazonOrderEntity", "PurchaseOrderNumber", "PurchaseOrderNumber", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 7);
		}

		/// <summary>Inits AmazonOrderItemEntity's mappings</summary>
		private void InitAmazonOrderItemEntityMappings()
		{
			this.AddElementMapping("AmazonOrderItemEntity", @"ShipWorksLocal", @"dbo", "AmazonOrderItem", 4, 0);
			this.AddElementFieldMapping("AmazonOrderItemEntity", "OrderItemID", "OrderItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("AmazonOrderItemEntity", "AmazonOrderItemCode", "AmazonOrderItemCode", false, "NVarChar", 64, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("AmazonOrderItemEntity", "ASIN", "ASIN", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("AmazonOrderItemEntity", "ConditionNote", "ConditionNote", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 3);
		}

		/// <summary>Inits AmazonProfileEntity's mappings</summary>
		private void InitAmazonProfileEntityMappings()
		{
			this.AddElementMapping("AmazonProfileEntity", @"ShipWorksLocal", @"dbo", "AmazonProfile", 9, 0);
			this.AddElementFieldMapping("AmazonProfileEntity", "ShippingProfileID", "ShippingProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("AmazonProfileEntity", "DimsProfileID", "DimsProfileID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("AmazonProfileEntity", "DimsLength", "DimsLength", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 2);
			this.AddElementFieldMapping("AmazonProfileEntity", "DimsWidth", "DimsWidth", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 3);
			this.AddElementFieldMapping("AmazonProfileEntity", "DimsHeight", "DimsHeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 4);
			this.AddElementFieldMapping("AmazonProfileEntity", "DimsWeight", "DimsWeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 5);
			this.AddElementFieldMapping("AmazonProfileEntity", "DimsAddWeight", "DimsAddWeight", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
			this.AddElementFieldMapping("AmazonProfileEntity", "DeliveryExperience", "DeliveryExperience", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 7);
			this.AddElementFieldMapping("AmazonProfileEntity", "Weight", "Weight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 8);
		}

		/// <summary>Inits AmazonShipmentEntity's mappings</summary>
		private void InitAmazonShipmentEntityMappings()
		{
			this.AddElementMapping("AmazonShipmentEntity", @"ShipWorksLocal", @"dbo", "AmazonShipment", 15, 0);
			this.AddElementFieldMapping("AmazonShipmentEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("AmazonShipmentEntity", "CarrierName", "CarrierName", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("AmazonShipmentEntity", "ShippingServiceName", "ShippingServiceName", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("AmazonShipmentEntity", "ShippingServiceID", "ShippingServiceID", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("AmazonShipmentEntity", "ShippingServiceOfferID", "ShippingServiceOfferID", false, "NVarChar", 250, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("AmazonShipmentEntity", "InsuranceValue", "InsuranceValue", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 5);
			this.AddElementFieldMapping("AmazonShipmentEntity", "DimsProfileID", "DimsProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 6);
			this.AddElementFieldMapping("AmazonShipmentEntity", "DimsLength", "DimsLength", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 7);
			this.AddElementFieldMapping("AmazonShipmentEntity", "DimsWidth", "DimsWidth", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 8);
			this.AddElementFieldMapping("AmazonShipmentEntity", "DimsHeight", "DimsHeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 9);
			this.AddElementFieldMapping("AmazonShipmentEntity", "DimsWeight", "DimsWeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 10);
			this.AddElementFieldMapping("AmazonShipmentEntity", "DimsAddWeight", "DimsAddWeight", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 11);
			this.AddElementFieldMapping("AmazonShipmentEntity", "DeliveryExperience", "DeliveryExperience", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 12);
			this.AddElementFieldMapping("AmazonShipmentEntity", "DeclaredValue", "DeclaredValue", true, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 13);
			this.AddElementFieldMapping("AmazonShipmentEntity", "AmazonUniqueShipmentID", "AmazonUniqueShipmentID", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 14);
		}

		/// <summary>Inits AmazonStoreEntity's mappings</summary>
		private void InitAmazonStoreEntityMappings()
		{
			this.AddElementMapping("AmazonStoreEntity", @"ShipWorksLocal", @"dbo", "AmazonStore", 19, 0);
			this.AddElementFieldMapping("AmazonStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("AmazonStoreEntity", "AmazonApi", "AmazonApi", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 1);
			this.AddElementFieldMapping("AmazonStoreEntity", "AmazonApiRegion", "AmazonApiRegion", false, "Char", 2, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("AmazonStoreEntity", "SellerCentralUsername", "SellerCentralUsername", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("AmazonStoreEntity", "SellerCentralPassword", "SellerCentralPassword", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("AmazonStoreEntity", "MerchantName", "MerchantName", false, "VarChar", 64, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("AmazonStoreEntity", "MerchantToken", "MerchantToken", false, "VarChar", 32, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("AmazonStoreEntity", "AccessKeyID", "AccessKeyID", false, "VarChar", 32, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("AmazonStoreEntity", "AuthToken", "AuthToken", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("AmazonStoreEntity", "Cookie", "Cookie", false, "Text", 2147483647, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("AmazonStoreEntity", "CookieExpires", "CookieExpires", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 10);
			this.AddElementFieldMapping("AmazonStoreEntity", "CookieWaitUntil", "CookieWaitUntil", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 11);
			this.AddElementFieldMapping("AmazonStoreEntity", "Certificate", "Certificate", true, "VarBinary", 2048, 0, 0, false, "", null, typeof(System.Byte[]), 12);
			this.AddElementFieldMapping("AmazonStoreEntity", "WeightDownloads", "WeightDownloads", false, "Text", 2147483647, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("AmazonStoreEntity", "MerchantID", "MerchantID", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("AmazonStoreEntity", "MarketplaceID", "MarketplaceID", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("AmazonStoreEntity", "ExcludeFBA", "ExcludeFBA", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 16);
			this.AddElementFieldMapping("AmazonStoreEntity", "DomainName", "DomainName", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 17);
			this.AddElementFieldMapping("AmazonStoreEntity", "AmazonShippingToken", "AmazonShippingToken", false, "NVarChar", 500, 0, 0, false, "", null, typeof(System.String), 18);
		}

		/// <summary>Inits AmeriCommerceStoreEntity's mappings</summary>
		private void InitAmeriCommerceStoreEntityMappings()
		{
			this.AddElementMapping("AmeriCommerceStoreEntity", @"ShipWorksLocal", @"dbo", "AmeriCommerceStore", 6, 0);
			this.AddElementFieldMapping("AmeriCommerceStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("AmeriCommerceStoreEntity", "Username", "Username", false, "NVarChar", 70, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("AmeriCommerceStoreEntity", "Password", "Password", false, "NVarChar", 70, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("AmeriCommerceStoreEntity", "StoreUrl", "StoreUrl", false, "NVarChar", 350, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("AmeriCommerceStoreEntity", "StoreCode", "StoreCode", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("AmeriCommerceStoreEntity", "StatusCodes", "StatusCodes", false, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 5);
		}

		/// <summary>Inits AuditEntity's mappings</summary>
		private void InitAuditEntityMappings()
		{
			this.AddElementMapping("AuditEntity", @"ShipWorksLocal", @"dbo", "Audit", 11, 0);
			this.AddElementFieldMapping("AuditEntity", "AuditID", "AuditID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("AuditEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("AuditEntity", "TransactionID", "TransactionID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("AuditEntity", "UserID", "UserID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("AuditEntity", "ComputerID", "ComputerID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
			this.AddElementFieldMapping("AuditEntity", "Reason", "Reason", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("AuditEntity", "ReasonDetail", "ReasonDetail", true, "VarChar", 100, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("AuditEntity", "Date", "Date", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 7);
			this.AddElementFieldMapping("AuditEntity", "Action", "Action", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 8);
			this.AddElementFieldMapping("AuditEntity", "EntityID", "ObjectID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 9);
			this.AddElementFieldMapping("AuditEntity", "HasEvents", "HasEvents", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 10);
		}

		/// <summary>Inits AuditChangeEntity's mappings</summary>
		private void InitAuditChangeEntityMappings()
		{
			this.AddElementMapping("AuditChangeEntity", @"ShipWorksLocal", @"dbo", "AuditChange", 4, 0);
			this.AddElementFieldMapping("AuditChangeEntity", "AuditChangeID", "AuditChangeID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("AuditChangeEntity", "AuditID", "AuditID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("AuditChangeEntity", "ChangeType", "ChangeType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("AuditChangeEntity", "EntityID", "ObjectID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
		}

		/// <summary>Inits AuditChangeDetailEntity's mappings</summary>
		private void InitAuditChangeDetailEntityMappings()
		{
			this.AddElementMapping("AuditChangeDetailEntity", @"ShipWorksLocal", @"dbo", "AuditChangeDetail", 10, 0);
			this.AddElementFieldMapping("AuditChangeDetailEntity", "AuditChangeDetailID", "AuditChangeDetailID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("AuditChangeDetailEntity", "AuditChangeID", "AuditChangeID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("AuditChangeDetailEntity", "AuditID", "AuditID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("AuditChangeDetailEntity", "DisplayName", "DisplayName", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("AuditChangeDetailEntity", "DisplayFormat", "DisplayFormat", false, "TinyInt", 0, 3, 0, false, "", null, typeof(System.Byte), 4);
			this.AddElementFieldMapping("AuditChangeDetailEntity", "DataType", "DataType", false, "TinyInt", 0, 3, 0, false, "", null, typeof(System.Byte), 5);
			this.AddElementFieldMapping("AuditChangeDetailEntity", "TextOld", "TextOld", true, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("AuditChangeDetailEntity", "TextNew", "TextNew", true, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("AuditChangeDetailEntity", "VariantOld", "VariantOld", true, "Variant", 0, 0, 0, false, "", null, typeof(System.Object), 8);
			this.AddElementFieldMapping("AuditChangeDetailEntity", "VariantNew", "VariantNew", true, "Variant", 0, 0, 0, false, "", null, typeof(System.Object), 9);
		}

		/// <summary>Inits BestRateProfileEntity's mappings</summary>
		private void InitBestRateProfileEntityMappings()
		{
			this.AddElementMapping("BestRateProfileEntity", @"ShipWorksLocal", @"dbo", "BestRateProfile", 9, 0);
			this.AddElementFieldMapping("BestRateProfileEntity", "ShippingProfileID", "ShippingProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("BestRateProfileEntity", "DimsProfileID", "DimsProfileID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("BestRateProfileEntity", "DimsLength", "DimsLength", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 2);
			this.AddElementFieldMapping("BestRateProfileEntity", "DimsWidth", "DimsWidth", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 3);
			this.AddElementFieldMapping("BestRateProfileEntity", "DimsHeight", "DimsHeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 4);
			this.AddElementFieldMapping("BestRateProfileEntity", "DimsWeight", "DimsWeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 5);
			this.AddElementFieldMapping("BestRateProfileEntity", "DimsAddWeight", "DimsAddWeight", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
			this.AddElementFieldMapping("BestRateProfileEntity", "Weight", "Weight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 7);
			this.AddElementFieldMapping("BestRateProfileEntity", "ServiceLevel", "ServiceLevel", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 8);
		}

		/// <summary>Inits BestRateShipmentEntity's mappings</summary>
		private void InitBestRateShipmentEntityMappings()
		{
			this.AddElementMapping("BestRateShipmentEntity", @"ShipWorksLocal", @"dbo", "BestRateShipment", 10, 0);
			this.AddElementFieldMapping("BestRateShipmentEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("BestRateShipmentEntity", "DimsProfileID", "DimsProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("BestRateShipmentEntity", "DimsLength", "DimsLength", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 2);
			this.AddElementFieldMapping("BestRateShipmentEntity", "DimsWidth", "DimsWidth", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 3);
			this.AddElementFieldMapping("BestRateShipmentEntity", "DimsHeight", "DimsHeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 4);
			this.AddElementFieldMapping("BestRateShipmentEntity", "DimsWeight", "DimsWeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 5);
			this.AddElementFieldMapping("BestRateShipmentEntity", "DimsAddWeight", "DimsAddWeight", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
			this.AddElementFieldMapping("BestRateShipmentEntity", "ServiceLevel", "ServiceLevel", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 7);
			this.AddElementFieldMapping("BestRateShipmentEntity", "InsuranceValue", "InsuranceValue", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 8);
			this.AddElementFieldMapping("BestRateShipmentEntity", "RequestedLabelFormat", "RequestedLabelFormat", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 9);
		}

		/// <summary>Inits BigCommerceOrderItemEntity's mappings</summary>
		private void InitBigCommerceOrderItemEntityMappings()
		{
			this.AddElementMapping("BigCommerceOrderItemEntity", @"ShipWorksLocal", @"dbo", "BigCommerceOrderItem", 6, 0);
			this.AddElementFieldMapping("BigCommerceOrderItemEntity", "OrderItemID", "OrderItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("BigCommerceOrderItemEntity", "OrderAddressID", "OrderAddressID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("BigCommerceOrderItemEntity", "OrderProductID", "OrderProductID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("BigCommerceOrderItemEntity", "IsDigitalItem", "IsDigitalItem", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("BigCommerceOrderItemEntity", "EventDate", "EventDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 4);
			this.AddElementFieldMapping("BigCommerceOrderItemEntity", "EventName", "EventName", true, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 5);
		}

		/// <summary>Inits BigCommerceStoreEntity's mappings</summary>
		private void InitBigCommerceStoreEntityMappings()
		{
			this.AddElementMapping("BigCommerceStoreEntity", @"ShipWorksLocal", @"dbo", "BigCommerceStore", 7, 0);
			this.AddElementFieldMapping("BigCommerceStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("BigCommerceStoreEntity", "ApiUrl", "ApiUrl", false, "NVarChar", 110, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("BigCommerceStoreEntity", "ApiUserName", "ApiUserName", false, "NVarChar", 65, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("BigCommerceStoreEntity", "ApiToken", "ApiToken", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("BigCommerceStoreEntity", "StatusCodes", "StatusCodes", true, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("BigCommerceStoreEntity", "WeightUnitOfMeasure", "WeightUnitOfMeasure", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("BigCommerceStoreEntity", "DownloadModifiedNumberOfDaysBack", "DownloadModifiedNumberOfDaysBack", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
		}

		/// <summary>Inits BuyDotComOrderItemEntity's mappings</summary>
		private void InitBuyDotComOrderItemEntityMappings()
		{
			this.AddElementMapping("BuyDotComOrderItemEntity", @"ShipWorksLocal", @"dbo", "BuyDotComOrderItem", 7, 0);
			this.AddElementFieldMapping("BuyDotComOrderItemEntity", "OrderItemID", "OrderItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("BuyDotComOrderItemEntity", "ReceiptItemID", "ReceiptItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("BuyDotComOrderItemEntity", "ListingID", "ListingID", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("BuyDotComOrderItemEntity", "Shipping", "Shipping", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 3);
			this.AddElementFieldMapping("BuyDotComOrderItemEntity", "Tax", "Tax", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 4);
			this.AddElementFieldMapping("BuyDotComOrderItemEntity", "Commission", "Commission", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 5);
			this.AddElementFieldMapping("BuyDotComOrderItemEntity", "ItemFee", "ItemFee", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 6);
		}

		/// <summary>Inits BuyDotComStoreEntity's mappings</summary>
		private void InitBuyDotComStoreEntityMappings()
		{
			this.AddElementMapping("BuyDotComStoreEntity", @"ShipWorksLocal", @"dbo", "BuyDotComStore", 3, 0);
			this.AddElementFieldMapping("BuyDotComStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("BuyDotComStoreEntity", "FtpUsername", "FtpUsername", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("BuyDotComStoreEntity", "FtpPassword", "FtpPassword", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
		}

		/// <summary>Inits ChannelAdvisorOrderEntity's mappings</summary>
		private void InitChannelAdvisorOrderEntityMappings()
		{
			this.AddElementMapping("ChannelAdvisorOrderEntity", @"ShipWorksLocal", @"dbo", "ChannelAdvisorOrder", 11, 0);
			this.AddElementFieldMapping("ChannelAdvisorOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ChannelAdvisorOrderEntity", "CustomOrderIdentifier", "CustomOrderIdentifier", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("ChannelAdvisorOrderEntity", "ResellerID", "ResellerID", false, "NVarChar", 80, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("ChannelAdvisorOrderEntity", "OnlineShippingStatus", "OnlineShippingStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("ChannelAdvisorOrderEntity", "OnlineCheckoutStatus", "OnlineCheckoutStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("ChannelAdvisorOrderEntity", "OnlinePaymentStatus", "OnlinePaymentStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("ChannelAdvisorOrderEntity", "FlagStyle", "FlagStyle", false, "NVarChar", 32, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("ChannelAdvisorOrderEntity", "FlagDescription", "FlagDescription", false, "NVarChar", 80, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("ChannelAdvisorOrderEntity", "FlagType", "FlagType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 8);
			this.AddElementFieldMapping("ChannelAdvisorOrderEntity", "MarketplaceNames", "MarketplaceNames", false, "NVarChar", 1024, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("ChannelAdvisorOrderEntity", "IsPrime", "IsPrime", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 10);
		}

		/// <summary>Inits ChannelAdvisorOrderItemEntity's mappings</summary>
		private void InitChannelAdvisorOrderItemEntityMappings()
		{
			this.AddElementMapping("ChannelAdvisorOrderItemEntity", @"ShipWorksLocal", @"dbo", "ChannelAdvisorOrderItem", 10, 0);
			this.AddElementFieldMapping("ChannelAdvisorOrderItemEntity", "OrderItemID", "OrderItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ChannelAdvisorOrderItemEntity", "MarketplaceName", "MarketplaceName", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("ChannelAdvisorOrderItemEntity", "MarketplaceStoreName", "MarketplaceStoreName", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("ChannelAdvisorOrderItemEntity", "MarketplaceBuyerID", "MarketplaceBuyerID", false, "NVarChar", 80, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("ChannelAdvisorOrderItemEntity", "MarketplaceSalesID", "MarketplaceSalesID", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("ChannelAdvisorOrderItemEntity", "Classification", "Classification", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("ChannelAdvisorOrderItemEntity", "DistributionCenter", "DistributionCenter", false, "NVarChar", 80, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("ChannelAdvisorOrderItemEntity", "HarmonizedCode", "HarmonizedCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("ChannelAdvisorOrderItemEntity", "IsFBA", "IsFBA", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 8);
			this.AddElementFieldMapping("ChannelAdvisorOrderItemEntity", "MPN", "MPN", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 9);
		}

		/// <summary>Inits ChannelAdvisorStoreEntity's mappings</summary>
		private void InitChannelAdvisorStoreEntityMappings()
		{
			this.AddElementMapping("ChannelAdvisorStoreEntity", @"ShipWorksLocal", @"dbo", "ChannelAdvisorStore", 9, 0);
			this.AddElementFieldMapping("ChannelAdvisorStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ChannelAdvisorStoreEntity", "AccountKey", "AccountKey", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("ChannelAdvisorStoreEntity", "ProfileID", "ProfileID", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("ChannelAdvisorStoreEntity", "AttributesToDownload", "AttributesToDownload", false, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("ChannelAdvisorStoreEntity", "ConsolidatorAsUsps", "ConsolidatorAsUsps", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 4);
			this.AddElementFieldMapping("ChannelAdvisorStoreEntity", "AmazonMerchantID", "AmazonMerchantID", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("ChannelAdvisorStoreEntity", "AmazonAuthToken", "AmazonAuthToken", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("ChannelAdvisorStoreEntity", "AmazonApiRegion", "AmazonApiRegion", false, "Char", 2, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("ChannelAdvisorStoreEntity", "AmazonShippingToken", "AmazonShippingToken", false, "NVarChar", 500, 0, 0, false, "", null, typeof(System.String), 8);
		}

		/// <summary>Inits ClickCartProOrderEntity's mappings</summary>
		private void InitClickCartProOrderEntityMappings()
		{
			this.AddElementMapping("ClickCartProOrderEntity", @"ShipWorksLocal", @"dbo", "ClickCartProOrder", 2, 0);
			this.AddElementFieldMapping("ClickCartProOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ClickCartProOrderEntity", "ClickCartProOrderID", "ClickCartProOrderID", false, "VarChar", 25, 0, 0, false, "", null, typeof(System.String), 1);
		}

		/// <summary>Inits CommerceInterfaceOrderEntity's mappings</summary>
		private void InitCommerceInterfaceOrderEntityMappings()
		{
			this.AddElementMapping("CommerceInterfaceOrderEntity", @"ShipWorksLocal", @"dbo", "CommerceInterfaceOrder", 2, 0);
			this.AddElementFieldMapping("CommerceInterfaceOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("CommerceInterfaceOrderEntity", "CommerceInterfaceOrderNumber", "CommerceInterfaceOrderNumber", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 1);
		}

		/// <summary>Inits ComputerEntity's mappings</summary>
		private void InitComputerEntityMappings()
		{
			this.AddElementMapping("ComputerEntity", @"ShipWorksLocal", @"dbo", "Computer", 4, 0);
			this.AddElementFieldMapping("ComputerEntity", "ComputerID", "ComputerID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ComputerEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ComputerEntity", "Identifier", "Identifier", false, "UniqueIdentifier", 0, 0, 0, false, "", null, typeof(System.Guid), 2);
			this.AddElementFieldMapping("ComputerEntity", "Name", "Name", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
		}

		/// <summary>Inits ConfigurationEntity's mappings</summary>
		private void InitConfigurationEntityMappings()
		{
			this.AddElementMapping("ConfigurationEntity", @"ShipWorksLocal", @"dbo", "Configuration", 13, 0);
			this.AddElementFieldMapping("ConfigurationEntity", "ConfigurationID", "ConfigurationID", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 0);
			this.AddElementFieldMapping("ConfigurationEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ConfigurationEntity", "LogOnMethod", "LogOnMethod", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("ConfigurationEntity", "AddressCasing", "AddressCasing", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("ConfigurationEntity", "CustomerCompareEmail", "CustomerCompareEmail", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 4);
			this.AddElementFieldMapping("ConfigurationEntity", "CustomerCompareAddress", "CustomerCompareAddress", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 5);
			this.AddElementFieldMapping("ConfigurationEntity", "CustomerUpdateBilling", "CustomerUpdateBilling", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
			this.AddElementFieldMapping("ConfigurationEntity", "CustomerUpdateShipping", "CustomerUpdateShipping", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 7);
			this.AddElementFieldMapping("ConfigurationEntity", "CustomerUpdateModifiedBilling", "CustomerUpdateModifiedBilling", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 8);
			this.AddElementFieldMapping("ConfigurationEntity", "CustomerUpdateModifiedShipping", "CustomerUpdateModifiedShipping", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 9);
			this.AddElementFieldMapping("ConfigurationEntity", "AuditNewOrders", "AuditNewOrders", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 10);
			this.AddElementFieldMapping("ConfigurationEntity", "AuditDeletedOrders", "AuditDeletedOrders", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 11);
			this.AddElementFieldMapping("ConfigurationEntity", "CustomerKey", "CustomerKey", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 12);
		}

		/// <summary>Inits CustomerEntity's mappings</summary>
		private void InitCustomerEntityMappings()
		{
			this.AddElementMapping("CustomerEntity", @"ShipWorksLocal", @"dbo", "Customer", 35, 0);
			this.AddElementFieldMapping("CustomerEntity", "CustomerID", "CustomerID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("CustomerEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("CustomerEntity", "BillFirstName", "BillFirstName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("CustomerEntity", "BillMiddleName", "BillMiddleName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("CustomerEntity", "BillLastName", "BillLastName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("CustomerEntity", "BillCompany", "BillCompany", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("CustomerEntity", "BillStreet1", "BillStreet1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("CustomerEntity", "BillStreet2", "BillStreet2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("CustomerEntity", "BillStreet3", "BillStreet3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("CustomerEntity", "BillCity", "BillCity", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("CustomerEntity", "BillStateProvCode", "BillStateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("CustomerEntity", "BillPostalCode", "BillPostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("CustomerEntity", "BillCountryCode", "BillCountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("CustomerEntity", "BillPhone", "BillPhone", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("CustomerEntity", "BillFax", "BillFax", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("CustomerEntity", "BillEmail", "BillEmail", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("CustomerEntity", "BillWebsite", "BillWebsite", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("CustomerEntity", "ShipFirstName", "ShipFirstName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 17);
			this.AddElementFieldMapping("CustomerEntity", "ShipMiddleName", "ShipMiddleName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("CustomerEntity", "ShipLastName", "ShipLastName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 19);
			this.AddElementFieldMapping("CustomerEntity", "ShipCompany", "ShipCompany", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 20);
			this.AddElementFieldMapping("CustomerEntity", "ShipStreet1", "ShipStreet1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 21);
			this.AddElementFieldMapping("CustomerEntity", "ShipStreet2", "ShipStreet2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 22);
			this.AddElementFieldMapping("CustomerEntity", "ShipStreet3", "ShipStreet3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 23);
			this.AddElementFieldMapping("CustomerEntity", "ShipCity", "ShipCity", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 24);
			this.AddElementFieldMapping("CustomerEntity", "ShipStateProvCode", "ShipStateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 25);
			this.AddElementFieldMapping("CustomerEntity", "ShipPostalCode", "ShipPostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 26);
			this.AddElementFieldMapping("CustomerEntity", "ShipCountryCode", "ShipCountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 27);
			this.AddElementFieldMapping("CustomerEntity", "ShipPhone", "ShipPhone", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 28);
			this.AddElementFieldMapping("CustomerEntity", "ShipFax", "ShipFax", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 29);
			this.AddElementFieldMapping("CustomerEntity", "ShipEmail", "ShipEmail", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 30);
			this.AddElementFieldMapping("CustomerEntity", "ShipWebsite", "ShipWebsite", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 31);
			this.AddElementFieldMapping("CustomerEntity", "RollupOrderCount", "RollupOrderCount", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 32);
			this.AddElementFieldMapping("CustomerEntity", "RollupOrderTotal", "RollupOrderTotal", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 33);
			this.AddElementFieldMapping("CustomerEntity", "RollupNoteCount", "RollupNoteCount", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 34);
		}

		/// <summary>Inits DimensionsProfileEntity's mappings</summary>
		private void InitDimensionsProfileEntityMappings()
		{
			this.AddElementMapping("DimensionsProfileEntity", @"ShipWorksLocal", @"dbo", "DimensionsProfile", 6, 0);
			this.AddElementFieldMapping("DimensionsProfileEntity", "DimensionsProfileID", "DimensionsProfileID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("DimensionsProfileEntity", "Name", "Name", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("DimensionsProfileEntity", "Length", "Length", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 2);
			this.AddElementFieldMapping("DimensionsProfileEntity", "Width", "Width", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 3);
			this.AddElementFieldMapping("DimensionsProfileEntity", "Height", "Height", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 4);
			this.AddElementFieldMapping("DimensionsProfileEntity", "Weight", "Weight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 5);
		}

		/// <summary>Inits DownloadEntity's mappings</summary>
		private void InitDownloadEntityMappings()
		{
			this.AddElementMapping("DownloadEntity", @"ShipWorksLocal", @"dbo", "Download", 13, 0);
			this.AddElementFieldMapping("DownloadEntity", "DownloadID", "DownloadID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("DownloadEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("DownloadEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("DownloadEntity", "ComputerID", "ComputerID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("DownloadEntity", "UserID", "UserID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
			this.AddElementFieldMapping("DownloadEntity", "InitiatedBy", "InitiatedBy", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("DownloadEntity", "Started", "Started", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 6);
			this.AddElementFieldMapping("DownloadEntity", "Ended", "Ended", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 7);
			this.AddElementFieldMapping("DownloadEntity", "Duration", "Duration", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 8);
			this.AddElementFieldMapping("DownloadEntity", "QuantityTotal", "QuantityTotal", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 9);
			this.AddElementFieldMapping("DownloadEntity", "QuantityNew", "QuantityNew", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 10);
			this.AddElementFieldMapping("DownloadEntity", "Result", "Result", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 11);
			this.AddElementFieldMapping("DownloadEntity", "ErrorMessage", "ErrorMessage", true, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 12);
		}

		/// <summary>Inits DownloadDetailEntity's mappings</summary>
		private void InitDownloadDetailEntityMappings()
		{
			this.AddElementMapping("DownloadDetailEntity", @"ShipWorksLocal", @"dbo", "DownloadDetail", 9, 0);
			this.AddElementFieldMapping("DownloadDetailEntity", "DownloadedDetailID", "DownloadedDetailID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("DownloadDetailEntity", "DownloadID", "DownloadID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("DownloadDetailEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("DownloadDetailEntity", "InitialDownload", "InitialDownload", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("DownloadDetailEntity", "OrderNumber", "OrderNumber", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
			this.AddElementFieldMapping("DownloadDetailEntity", "ExtraBigIntData1", "ExtraBigIntData1", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 5);
			this.AddElementFieldMapping("DownloadDetailEntity", "ExtraBigIntData2", "ExtraBigIntData2", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 6);
			this.AddElementFieldMapping("DownloadDetailEntity", "ExtraBigIntData3", "ExtraBigIntData3", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 7);
			this.AddElementFieldMapping("DownloadDetailEntity", "ExtraStringData1", "ExtraStringData1", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 8);
		}

		/// <summary>Inits EbayCombinedOrderRelationEntity's mappings</summary>
		private void InitEbayCombinedOrderRelationEntityMappings()
		{
			this.AddElementMapping("EbayCombinedOrderRelationEntity", @"ShipWorksLocal", @"dbo", "EbayCombinedOrderRelation", 4, 0);
			this.AddElementFieldMapping("EbayCombinedOrderRelationEntity", "EbayCombinedOrderRelationID", "EbayCombinedOrderRelationID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("EbayCombinedOrderRelationEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("EbayCombinedOrderRelationEntity", "EbayOrderID", "EbayOrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("EbayCombinedOrderRelationEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
		}

		/// <summary>Inits EbayOrderEntity's mappings</summary>
		private void InitEbayOrderEntityMappings()
		{
			this.AddElementMapping("EbayOrderEntity", @"ShipWorksLocal", @"dbo", "EbayOrder", 24, 0);
			this.AddElementFieldMapping("EbayOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("EbayOrderEntity", "EbayOrderID", "EbayOrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("EbayOrderEntity", "EbayBuyerID", "EbayBuyerID", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("EbayOrderEntity", "CombinedLocally", "CombinedLocally", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("EbayOrderEntity", "SelectedShippingMethod", "SelectedShippingMethod", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("EbayOrderEntity", "SellingManagerRecord", "SellingManagerRecord", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("EbayOrderEntity", "GspEligible", "GspEligible", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
			this.AddElementFieldMapping("EbayOrderEntity", "GspFirstName", "GspFirstName", false, "NVarChar", 128, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("EbayOrderEntity", "GspLastName", "GspLastName", false, "NVarChar", 128, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("EbayOrderEntity", "GspStreet1", "GspStreet1", false, "NVarChar", 512, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("EbayOrderEntity", "GspStreet2", "GspStreet2", false, "NVarChar", 512, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("EbayOrderEntity", "GspCity", "GspCity", false, "NVarChar", 128, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("EbayOrderEntity", "GspStateProvince", "GspStateProvince", false, "NVarChar", 128, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("EbayOrderEntity", "GspPostalCode", "GspPostalCode", false, "NVarChar", 9, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("EbayOrderEntity", "GspCountryCode", "GspCountryCode", false, "NVarChar", 2, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("EbayOrderEntity", "GspReferenceID", "GspReferenceID", false, "NVarChar", 128, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("EbayOrderEntity", "RollupEbayItemCount", "RollupEbayItemCount", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 16);
			this.AddElementFieldMapping("EbayOrderEntity", "RollupEffectiveCheckoutStatus", "RollupEffectiveCheckoutStatus", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 17);
			this.AddElementFieldMapping("EbayOrderEntity", "RollupEffectivePaymentMethod", "RollupEffectivePaymentMethod", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 18);
			this.AddElementFieldMapping("EbayOrderEntity", "RollupFeedbackLeftType", "RollupFeedbackLeftType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 19);
			this.AddElementFieldMapping("EbayOrderEntity", "RollupFeedbackLeftComments", "RollupFeedbackLeftComments", true, "VarChar", 80, 0, 0, false, "", null, typeof(System.String), 20);
			this.AddElementFieldMapping("EbayOrderEntity", "RollupFeedbackReceivedType", "RollupFeedbackReceivedType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 21);
			this.AddElementFieldMapping("EbayOrderEntity", "RollupFeedbackReceivedComments", "RollupFeedbackReceivedComments", true, "VarChar", 80, 0, 0, false, "", null, typeof(System.String), 22);
			this.AddElementFieldMapping("EbayOrderEntity", "RollupPayPalAddressStatus", "RollupPayPalAddressStatus", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 23);
		}

		/// <summary>Inits EbayOrderItemEntity's mappings</summary>
		private void InitEbayOrderItemEntityMappings()
		{
			this.AddElementMapping("EbayOrderItemEntity", @"ShipWorksLocal", @"dbo", "EbayOrderItem", 18, 0);
			this.AddElementFieldMapping("EbayOrderItemEntity", "OrderItemID", "OrderItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("EbayOrderItemEntity", "LocalEbayOrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("EbayOrderItemEntity", "EbayItemID", "EbayItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("EbayOrderItemEntity", "EbayTransactionID", "EbayTransactionID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("EbayOrderItemEntity", "SellingManagerRecord", "SellingManagerRecord", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("EbayOrderItemEntity", "EffectiveCheckoutStatus", "EffectiveCheckoutStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("EbayOrderItemEntity", "EffectivePaymentMethod", "EffectivePaymentMethod", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
			this.AddElementFieldMapping("EbayOrderItemEntity", "PaymentStatus", "PaymentStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 7);
			this.AddElementFieldMapping("EbayOrderItemEntity", "PaymentMethod", "PaymentMethod", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 8);
			this.AddElementFieldMapping("EbayOrderItemEntity", "CompleteStatus", "CompleteStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 9);
			this.AddElementFieldMapping("EbayOrderItemEntity", "FeedbackLeftType", "FeedbackLeftType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 10);
			this.AddElementFieldMapping("EbayOrderItemEntity", "FeedbackLeftComments", "FeedbackLeftComments", false, "NVarChar", 80, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("EbayOrderItemEntity", "FeedbackReceivedType", "FeedbackReceivedType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 12);
			this.AddElementFieldMapping("EbayOrderItemEntity", "FeedbackReceivedComments", "FeedbackReceivedComments", false, "NVarChar", 80, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("EbayOrderItemEntity", "MyEbayPaid", "MyEbayPaid", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 14);
			this.AddElementFieldMapping("EbayOrderItemEntity", "MyEbayShipped", "MyEbayShipped", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 15);
			this.AddElementFieldMapping("EbayOrderItemEntity", "PayPalTransactionID", "PayPalTransactionID", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("EbayOrderItemEntity", "PayPalAddressStatus", "PayPalAddressStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 17);
		}

		/// <summary>Inits EbayStoreEntity's mappings</summary>
		private void InitEbayStoreEntityMappings()
		{
			this.AddElementMapping("EbayStoreEntity", @"ShipWorksLocal", @"dbo", "EbayStore", 16, 0);
			this.AddElementFieldMapping("EbayStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("EbayStoreEntity", "EBayUserID", "eBayUserID", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("EbayStoreEntity", "EBayToken", "eBayToken", false, "Text", 2147483647, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("EbayStoreEntity", "EBayTokenExpire", "eBayTokenExpire", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 3);
			this.AddElementFieldMapping("EbayStoreEntity", "AcceptedPaymentList", "AcceptedPaymentList", false, "VarChar", 30, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("EbayStoreEntity", "DownloadItemDetails", "DownloadItemDetails", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 5);
			this.AddElementFieldMapping("EbayStoreEntity", "DownloadOlderOrders", "DownloadOlderOrders", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
			this.AddElementFieldMapping("EbayStoreEntity", "DownloadPayPalDetails", "DownloadPayPalDetails", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 7);
			this.AddElementFieldMapping("EbayStoreEntity", "PayPalApiCredentialType", "PayPalApiCredentialType", false, "SmallInt", 0, 5, 0, false, "", null, typeof(System.Int16), 8);
			this.AddElementFieldMapping("EbayStoreEntity", "PayPalApiUserName", "PayPalApiUserName", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("EbayStoreEntity", "PayPalApiPassword", "PayPalApiPassword", false, "NVarChar", 80, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("EbayStoreEntity", "PayPalApiSignature", "PayPalApiSignature", false, "NVarChar", 80, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("EbayStoreEntity", "PayPalApiCertificate", "PayPalApiCertificate", true, "VarBinary", 2048, 0, 0, false, "", null, typeof(System.Byte[]), 12);
			this.AddElementFieldMapping("EbayStoreEntity", "DomesticShippingService", "DomesticShippingService", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("EbayStoreEntity", "InternationalShippingService", "InternationalShippingService", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("EbayStoreEntity", "FeedbackUpdatedThrough", "FeedbackUpdatedThrough", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 15);
		}

		/// <summary>Inits EmailAccountEntity's mappings</summary>
		private void InitEmailAccountEntityMappings()
		{
			this.AddElementMapping("EmailAccountEntity", @"ShipWorksLocal", @"dbo", "EmailAccount", 27, 0);
			this.AddElementFieldMapping("EmailAccountEntity", "EmailAccountID", "EmailAccountID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("EmailAccountEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("EmailAccountEntity", "AccountName", "AccountName", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("EmailAccountEntity", "DisplayName", "DisplayName", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("EmailAccountEntity", "EmailAddress", "EmailAddress", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("EmailAccountEntity", "IncomingServer", "IncomingServer", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("EmailAccountEntity", "IncomingServerType", "IncomingServerType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
			this.AddElementFieldMapping("EmailAccountEntity", "IncomingPort", "IncomingPort", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 7);
			this.AddElementFieldMapping("EmailAccountEntity", "IncomingSecurityType", "IncomingSecurityType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 8);
			this.AddElementFieldMapping("EmailAccountEntity", "IncomingUsername", "IncomingUsername", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("EmailAccountEntity", "IncomingPassword", "IncomingPassword", false, "NVarChar", 150, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("EmailAccountEntity", "OutgoingServer", "OutgoingServer", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("EmailAccountEntity", "OutgoingPort", "OutgoingPort", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 12);
			this.AddElementFieldMapping("EmailAccountEntity", "OutgoingSecurityType", "OutgoingSecurityType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 13);
			this.AddElementFieldMapping("EmailAccountEntity", "OutgoingCredentialSource", "OutgoingCredentialSource", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 14);
			this.AddElementFieldMapping("EmailAccountEntity", "OutgoingUsername", "OutgoingUsername", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("EmailAccountEntity", "OutgoingPassword", "OutgoingPassword", false, "NVarChar", 150, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("EmailAccountEntity", "AutoSend", "AutoSend", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 17);
			this.AddElementFieldMapping("EmailAccountEntity", "AutoSendMinutes", "AutoSendMinutes", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 18);
			this.AddElementFieldMapping("EmailAccountEntity", "AutoSendLastTime", "AutoSendLastTime", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 19);
			this.AddElementFieldMapping("EmailAccountEntity", "LimitMessagesPerConnection", "LimitMessagesPerConnection", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 20);
			this.AddElementFieldMapping("EmailAccountEntity", "LimitMessagesPerConnectionQuantity", "LimitMessagesPerConnectionQuantity", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 21);
			this.AddElementFieldMapping("EmailAccountEntity", "LimitMessagesPerHour", "LimitMessagesPerHour", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 22);
			this.AddElementFieldMapping("EmailAccountEntity", "LimitMessagesPerHourQuantity", "LimitMessagesPerHourQuantity", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 23);
			this.AddElementFieldMapping("EmailAccountEntity", "LimitMessageInterval", "LimitMessageInterval", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 24);
			this.AddElementFieldMapping("EmailAccountEntity", "LimitMessageIntervalSeconds", "LimitMessageIntervalSeconds", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 25);
			this.AddElementFieldMapping("EmailAccountEntity", "InternalOwnerID", "InternalOwnerID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 26);
		}

		/// <summary>Inits EmailOutboundEntity's mappings</summary>
		private void InitEmailOutboundEntityMappings()
		{
			this.AddElementMapping("EmailOutboundEntity", @"ShipWorksLocal", @"dbo", "EmailOutbound", 21, 0);
			this.AddElementFieldMapping("EmailOutboundEntity", "EmailOutboundID", "EmailOutboundID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("EmailOutboundEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("EmailOutboundEntity", "ContextID", "ContextID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("EmailOutboundEntity", "ContextType", "ContextType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("EmailOutboundEntity", "TemplateID", "TemplateID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
			this.AddElementFieldMapping("EmailOutboundEntity", "AccountID", "AccountID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 5);
			this.AddElementFieldMapping("EmailOutboundEntity", "Visibility", "Visibility", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
			this.AddElementFieldMapping("EmailOutboundEntity", "FromAddress", "FromAddress", false, "NVarChar", 200, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("EmailOutboundEntity", "ToList", "ToList", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("EmailOutboundEntity", "CcList", "CcList", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("EmailOutboundEntity", "BccList", "BccList", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("EmailOutboundEntity", "Subject", "Subject", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("EmailOutboundEntity", "HtmlPartResourceID", "HtmlPartResourceID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 12);
			this.AddElementFieldMapping("EmailOutboundEntity", "PlainPartResourceID", "PlainPartResourceID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 13);
			this.AddElementFieldMapping("EmailOutboundEntity", "Encoding", "Encoding", true, "VarChar", 20, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("EmailOutboundEntity", "ComposedDate", "ComposedDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 15);
			this.AddElementFieldMapping("EmailOutboundEntity", "SentDate", "SentDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 16);
			this.AddElementFieldMapping("EmailOutboundEntity", "DontSendBefore", "DontSendBefore", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 17);
			this.AddElementFieldMapping("EmailOutboundEntity", "SendStatus", "SendStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 18);
			this.AddElementFieldMapping("EmailOutboundEntity", "SendAttemptCount", "SendAttemptCount", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 19);
			this.AddElementFieldMapping("EmailOutboundEntity", "SendAttemptLastError", "SendAttemptLastError", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 20);
		}

		/// <summary>Inits EmailOutboundRelationEntity's mappings</summary>
		private void InitEmailOutboundRelationEntityMappings()
		{
			this.AddElementMapping("EmailOutboundRelationEntity", @"ShipWorksLocal", @"dbo", "EmailOutboundRelation", 4, 0);
			this.AddElementFieldMapping("EmailOutboundRelationEntity", "EmailOutboundRelationID", "EmailOutboundRelationID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("EmailOutboundRelationEntity", "EmailOutboundID", "EmailOutboundID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("EmailOutboundRelationEntity", "EntityID", "ObjectID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("EmailOutboundRelationEntity", "RelationType", "RelationType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
		}

		/// <summary>Inits EndiciaAccountEntity's mappings</summary>
		private void InitEndiciaAccountEntityMappings()
		{
			this.AddElementMapping("EndiciaAccountEntity", @"ShipWorksLocal", @"dbo", "EndiciaAccount", 26, 0);
			this.AddElementFieldMapping("EndiciaAccountEntity", "EndiciaAccountID", "EndiciaAccountID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("EndiciaAccountEntity", "EndiciaReseller", "EndiciaReseller", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 1);
			this.AddElementFieldMapping("EndiciaAccountEntity", "AccountNumber", "AccountNumber", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("EndiciaAccountEntity", "SignupConfirmation", "SignupConfirmation", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("EndiciaAccountEntity", "WebPassword", "WebPassword", false, "NVarChar", 250, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("EndiciaAccountEntity", "ApiInitialPassword", "ApiInitialPassword", false, "NVarChar", 250, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("EndiciaAccountEntity", "ApiUserPassword", "ApiUserPassword", false, "NVarChar", 250, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("EndiciaAccountEntity", "AccountType", "AccountType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 7);
			this.AddElementFieldMapping("EndiciaAccountEntity", "TestAccount", "TestAccount", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 8);
			this.AddElementFieldMapping("EndiciaAccountEntity", "CreatedByShipWorks", "CreatedByShipWorks", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 9);
			this.AddElementFieldMapping("EndiciaAccountEntity", "Description", "Description", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("EndiciaAccountEntity", "FirstName", "FirstName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("EndiciaAccountEntity", "LastName", "LastName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("EndiciaAccountEntity", "Company", "Company", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("EndiciaAccountEntity", "Street1", "Street1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("EndiciaAccountEntity", "Street2", "Street2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("EndiciaAccountEntity", "Street3", "Street3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("EndiciaAccountEntity", "City", "City", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 17);
			this.AddElementFieldMapping("EndiciaAccountEntity", "StateProvCode", "StateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("EndiciaAccountEntity", "PostalCode", "PostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 19);
			this.AddElementFieldMapping("EndiciaAccountEntity", "CountryCode", "CountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 20);
			this.AddElementFieldMapping("EndiciaAccountEntity", "Phone", "Phone", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 21);
			this.AddElementFieldMapping("EndiciaAccountEntity", "Fax", "Fax", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 22);
			this.AddElementFieldMapping("EndiciaAccountEntity", "Email", "Email", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 23);
			this.AddElementFieldMapping("EndiciaAccountEntity", "MailingPostalCode", "MailingPostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 24);
			this.AddElementFieldMapping("EndiciaAccountEntity", "ScanFormAddressSource", "ScanFormAddressSource", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 25);
		}

		/// <summary>Inits EndiciaProfileEntity's mappings</summary>
		private void InitEndiciaProfileEntityMappings()
		{
			this.AddElementMapping("EndiciaProfileEntity", @"ShipWorksLocal", @"dbo", "EndiciaProfile", 5, 0);
			this.AddElementFieldMapping("EndiciaProfileEntity", "ShippingProfileID", "ShippingProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("EndiciaProfileEntity", "EndiciaAccountID", "EndiciaAccountID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("EndiciaProfileEntity", "StealthPostage", "StealthPostage", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 2);
			this.AddElementFieldMapping("EndiciaProfileEntity", "ReferenceID", "ReferenceID", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("EndiciaProfileEntity", "ScanBasedReturn", "ScanBasedReturn", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 4);
		}

		/// <summary>Inits EndiciaScanFormEntity's mappings</summary>
		private void InitEndiciaScanFormEntityMappings()
		{
			this.AddElementMapping("EndiciaScanFormEntity", @"ShipWorksLocal", @"dbo", "EndiciaScanForm", 7, 0);
			this.AddElementFieldMapping("EndiciaScanFormEntity", "EndiciaScanFormID", "EndiciaScanFormID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("EndiciaScanFormEntity", "EndiciaAccountID", "EndiciaAccountID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("EndiciaScanFormEntity", "EndiciaAccountNumber", "EndiciaAccountNumber", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("EndiciaScanFormEntity", "SubmissionID", "SubmissionID", false, "VarChar", 100, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("EndiciaScanFormEntity", "CreatedDate", "CreatedDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 4);
			this.AddElementFieldMapping("EndiciaScanFormEntity", "ScanFormBatchID", "ScanFormBatchID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 5);
			this.AddElementFieldMapping("EndiciaScanFormEntity", "Description", "Description", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 6);
		}

		/// <summary>Inits EndiciaShipmentEntity's mappings</summary>
		private void InitEndiciaShipmentEntityMappings()
		{
			this.AddElementMapping("EndiciaShipmentEntity", @"ShipWorksLocal", @"dbo", "EndiciaShipment", 10, 0);
			this.AddElementFieldMapping("EndiciaShipmentEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("EndiciaShipmentEntity", "EndiciaAccountID", "EndiciaAccountID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("EndiciaShipmentEntity", "OriginalEndiciaAccountID", "OriginalEndiciaAccountID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("EndiciaShipmentEntity", "StealthPostage", "StealthPostage", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("EndiciaShipmentEntity", "ReferenceID", "ReferenceID", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("EndiciaShipmentEntity", "TransactionID", "TransactionID", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("EndiciaShipmentEntity", "RefundFormID", "RefundFormID", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
			this.AddElementFieldMapping("EndiciaShipmentEntity", "ScanFormBatchID", "ScanFormBatchID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 7);
			this.AddElementFieldMapping("EndiciaShipmentEntity", "ScanBasedReturn", "ScanBasedReturn", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 8);
			this.AddElementFieldMapping("EndiciaShipmentEntity", "RequestedLabelFormat", "RequestedLabelFormat", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 9);
		}

		/// <summary>Inits EtsyOrderEntity's mappings</summary>
		private void InitEtsyOrderEntityMappings()
		{
			this.AddElementMapping("EtsyOrderEntity", @"ShipWorksLocal", @"dbo", "EtsyOrder", 3, 0);
			this.AddElementFieldMapping("EtsyOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("EtsyOrderEntity", "WasPaid", "WasPaid", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 1);
			this.AddElementFieldMapping("EtsyOrderEntity", "WasShipped", "WasShipped", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 2);
		}

		/// <summary>Inits EtsyStoreEntity's mappings</summary>
		private void InitEtsyStoreEntityMappings()
		{
			this.AddElementMapping("EtsyStoreEntity", @"ShipWorksLocal", @"dbo", "EtsyStore", 6, 0);
			this.AddElementFieldMapping("EtsyStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("EtsyStoreEntity", "EtsyShopID", "EtsyShopID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("EtsyStoreEntity", "EtsyLoginName", "EtsyLogin", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("EtsyStoreEntity", "EtsyStoreName", "EtsyStoreName", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("EtsyStoreEntity", "OAuthToken", "OAuthToken", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("EtsyStoreEntity", "OAuthTokenSecret", "OAuthTokenSecret", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 5);
		}

		/// <summary>Inits ExcludedPackageTypeEntity's mappings</summary>
		private void InitExcludedPackageTypeEntityMappings()
		{
			this.AddElementMapping("ExcludedPackageTypeEntity", @"ShipWorksLocal", @"dbo", "ExcludedPackageType", 2, 0);
			this.AddElementFieldMapping("ExcludedPackageTypeEntity", "ShipmentType", "ShipmentType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 0);
			this.AddElementFieldMapping("ExcludedPackageTypeEntity", "PackageType", "PackageType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 1);
		}

		/// <summary>Inits ExcludedServiceTypeEntity's mappings</summary>
		private void InitExcludedServiceTypeEntityMappings()
		{
			this.AddElementMapping("ExcludedServiceTypeEntity", @"ShipWorksLocal", @"dbo", "ExcludedServiceType", 2, 0);
			this.AddElementFieldMapping("ExcludedServiceTypeEntity", "ShipmentType", "ShipmentType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 0);
			this.AddElementFieldMapping("ExcludedServiceTypeEntity", "ServiceType", "ServiceType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 1);
		}

		/// <summary>Inits FedExAccountEntity's mappings</summary>
		private void InitFedExAccountEntityMappings()
		{
			this.AddElementMapping("FedExAccountEntity", @"ShipWorksLocal", @"dbo", "FedExAccount", 20, 0);
			this.AddElementFieldMapping("FedExAccountEntity", "FedExAccountID", "FedExAccountID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("FedExAccountEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("FedExAccountEntity", "Description", "Description", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("FedExAccountEntity", "AccountNumber", "AccountNumber", false, "NVarChar", 12, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("FedExAccountEntity", "SignatureRelease", "SignatureRelease", false, "VarChar", 10, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("FedExAccountEntity", "MeterNumber", "MeterNumber", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("FedExAccountEntity", "SmartPostHubList", "SmartPostHubList", false, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("FedExAccountEntity", "FirstName", "FirstName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("FedExAccountEntity", "MiddleName", "MiddleName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("FedExAccountEntity", "LastName", "LastName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("FedExAccountEntity", "Company", "Company", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("FedExAccountEntity", "Street1", "Street1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("FedExAccountEntity", "Street2", "Street2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("FedExAccountEntity", "City", "City", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("FedExAccountEntity", "StateProvCode", "StateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("FedExAccountEntity", "PostalCode", "PostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("FedExAccountEntity", "CountryCode", "CountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("FedExAccountEntity", "Phone", "Phone", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 17);
			this.AddElementFieldMapping("FedExAccountEntity", "Email", "Email", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("FedExAccountEntity", "Website", "Website", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 19);
		}

		/// <summary>Inits FedExEndOfDayCloseEntity's mappings</summary>
		private void InitFedExEndOfDayCloseEntityMappings()
		{
			this.AddElementMapping("FedExEndOfDayCloseEntity", @"ShipWorksLocal", @"dbo", "FedExEndOfDayClose", 5, 0);
			this.AddElementFieldMapping("FedExEndOfDayCloseEntity", "FedExEndOfDayCloseID", "FedExEndOfDayCloseID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("FedExEndOfDayCloseEntity", "FedExAccountID", "FedExAccountID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("FedExEndOfDayCloseEntity", "AccountNumber", "AccountNumber", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("FedExEndOfDayCloseEntity", "CloseDate", "CloseDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 3);
			this.AddElementFieldMapping("FedExEndOfDayCloseEntity", "IsSmartPost", "IsSmartPost", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 4);
		}

		/// <summary>Inits FedExPackageEntity's mappings</summary>
		private void InitFedExPackageEntityMappings()
		{
			this.AddElementMapping("FedExPackageEntity", @"ShipWorksLocal", @"dbo", "FedExPackage", 42, 0);
			this.AddElementFieldMapping("FedExPackageEntity", "FedExPackageID", "FedExPackageID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("FedExPackageEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("FedExPackageEntity", "Weight", "Weight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 2);
			this.AddElementFieldMapping("FedExPackageEntity", "DimsProfileID", "DimsProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("FedExPackageEntity", "DimsLength", "DimsLength", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 4);
			this.AddElementFieldMapping("FedExPackageEntity", "DimsWidth", "DimsWidth", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 5);
			this.AddElementFieldMapping("FedExPackageEntity", "DimsHeight", "DimsHeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 6);
			this.AddElementFieldMapping("FedExPackageEntity", "DimsWeight", "DimsWeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 7);
			this.AddElementFieldMapping("FedExPackageEntity", "DimsAddWeight", "DimsAddWeight", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 8);
			this.AddElementFieldMapping("FedExPackageEntity", "SkidPieces", "SkidPieces", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 9);
			this.AddElementFieldMapping("FedExPackageEntity", "Insurance", "Insurance", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 10);
			this.AddElementFieldMapping("FedExPackageEntity", "InsuranceValue", "InsuranceValue", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 11);
			this.AddElementFieldMapping("FedExPackageEntity", "InsurancePennyOne", "InsurancePennyOne", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 12);
			this.AddElementFieldMapping("FedExPackageEntity", "DeclaredValue", "DeclaredValue", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 13);
			this.AddElementFieldMapping("FedExPackageEntity", "TrackingNumber", "TrackingNumber", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("FedExPackageEntity", "PriorityAlert", "PriorityAlert", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 15);
			this.AddElementFieldMapping("FedExPackageEntity", "PriorityAlertEnhancementType", "PriorityAlertEnhancementType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 16);
			this.AddElementFieldMapping("FedExPackageEntity", "PriorityAlertDetailContent", "PriorityAlertDetailContent", false, "NVarChar", 1024, 0, 0, false, "", null, typeof(System.String), 17);
			this.AddElementFieldMapping("FedExPackageEntity", "DryIceWeight", "DryIceWeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 18);
			this.AddElementFieldMapping("FedExPackageEntity", "ContainsAlcohol", "ContainsAlcohol", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 19);
			this.AddElementFieldMapping("FedExPackageEntity", "DangerousGoodsEnabled", "DangerousGoodsEnabled", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 20);
			this.AddElementFieldMapping("FedExPackageEntity", "DangerousGoodsType", "DangerousGoodsType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 21);
			this.AddElementFieldMapping("FedExPackageEntity", "DangerousGoodsAccessibilityType", "DangerousGoodsAccessibilityType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 22);
			this.AddElementFieldMapping("FedExPackageEntity", "DangerousGoodsCargoAircraftOnly", "DangerousGoodsCargoAircraftOnly", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 23);
			this.AddElementFieldMapping("FedExPackageEntity", "DangerousGoodsEmergencyContactPhone", "DangerousGoodsEmergencyContactPhone", false, "NVarChar", 16, 0, 0, false, "", null, typeof(System.String), 24);
			this.AddElementFieldMapping("FedExPackageEntity", "DangerousGoodsOfferor", "DangerousGoodsOfferor", false, "NVarChar", 128, 0, 0, false, "", null, typeof(System.String), 25);
			this.AddElementFieldMapping("FedExPackageEntity", "DangerousGoodsPackagingCount", "DangerousGoodsPackagingCount", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 26);
			this.AddElementFieldMapping("FedExPackageEntity", "HazardousMaterialNumber", "HazardousMaterialNumber", false, "NVarChar", 16, 0, 0, false, "", null, typeof(System.String), 27);
			this.AddElementFieldMapping("FedExPackageEntity", "HazardousMaterialClass", "HazardousMaterialClass", false, "NVarChar", 8, 0, 0, false, "", null, typeof(System.String), 28);
			this.AddElementFieldMapping("FedExPackageEntity", "HazardousMaterialProperName", "HazardousMaterialProperName", false, "NVarChar", 64, 0, 0, false, "", null, typeof(System.String), 29);
			this.AddElementFieldMapping("FedExPackageEntity", "HazardousMaterialPackingGroup", "HazardousMaterialPackingGroup", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 30);
			this.AddElementFieldMapping("FedExPackageEntity", "HazardousMaterialQuantityValue", "HazardousMaterialQuantityValue", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 31);
			this.AddElementFieldMapping("FedExPackageEntity", "HazardousMaterialQuanityUnits", "HazardousMaterialQuanityUnits", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 32);
			this.AddElementFieldMapping("FedExPackageEntity", "HazardousMaterialTechnicalName", "HazardousMaterialTechnicalName", false, "NVarChar", 64, 0, 0, false, "", null, typeof(System.String), 33);
			this.AddElementFieldMapping("FedExPackageEntity", "SignatoryContactName", "SignatoryContactName", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 34);
			this.AddElementFieldMapping("FedExPackageEntity", "SignatoryTitle", "SignatoryTitle", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 35);
			this.AddElementFieldMapping("FedExPackageEntity", "SignatoryPlace", "SignatoryPlace", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 36);
			this.AddElementFieldMapping("FedExPackageEntity", "AlcoholRecipientType", "AlcoholRecipientType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 37);
			this.AddElementFieldMapping("FedExPackageEntity", "ContainerType", "ContainerType", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 38);
			this.AddElementFieldMapping("FedExPackageEntity", "NumberOfContainers", "NumberOfContainers", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 39);
			this.AddElementFieldMapping("FedExPackageEntity", "PackingDetailsCargoAircraftOnly", "PackingDetailsCargoAircraftOnly", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 40);
			this.AddElementFieldMapping("FedExPackageEntity", "PackingDetailsPackingInstructions", "PackingDetailsPackingInstructions", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 41);
		}

		/// <summary>Inits FedExProfileEntity's mappings</summary>
		private void InitFedExProfileEntityMappings()
		{
			this.AddElementMapping("FedExProfileEntity", @"ShipWorksLocal", @"dbo", "FedExProfile", 37, 0);
			this.AddElementFieldMapping("FedExProfileEntity", "ShippingProfileID", "ShippingProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("FedExProfileEntity", "FedExAccountID", "FedExAccountID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("FedExProfileEntity", "Service", "Service", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("FedExProfileEntity", "Signature", "Signature", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("FedExProfileEntity", "PackagingType", "PackagingType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("FedExProfileEntity", "NonStandardContainer", "NonStandardContainer", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 5);
			this.AddElementFieldMapping("FedExProfileEntity", "ReferenceCustomer", "ReferenceCustomer", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("FedExProfileEntity", "ReferenceInvoice", "ReferenceInvoice", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("FedExProfileEntity", "ReferencePO", "ReferencePO", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("FedExProfileEntity", "ReferenceShipmentIntegrity", "ReferenceShipmentIntegrity", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("FedExProfileEntity", "PayorTransportType", "PayorTransportType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 10);
			this.AddElementFieldMapping("FedExProfileEntity", "PayorTransportAccount", "PayorTransportAccount", true, "VarChar", 12, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("FedExProfileEntity", "PayorDutiesType", "PayorDutiesType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 12);
			this.AddElementFieldMapping("FedExProfileEntity", "PayorDutiesAccount", "PayorDutiesAccount", true, "VarChar", 12, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("FedExProfileEntity", "SaturdayDelivery", "SaturdayDelivery", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 14);
			this.AddElementFieldMapping("FedExProfileEntity", "EmailNotifySender", "EmailNotifySender", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 15);
			this.AddElementFieldMapping("FedExProfileEntity", "EmailNotifyRecipient", "EmailNotifyRecipient", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 16);
			this.AddElementFieldMapping("FedExProfileEntity", "EmailNotifyOther", "EmailNotifyOther", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 17);
			this.AddElementFieldMapping("FedExProfileEntity", "EmailNotifyOtherAddress", "EmailNotifyOtherAddress", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("FedExProfileEntity", "EmailNotifyMessage", "EmailNotifyMessage", true, "VarChar", 120, 0, 0, false, "", null, typeof(System.String), 19);
			this.AddElementFieldMapping("FedExProfileEntity", "ResidentialDetermination", "ResidentialDetermination", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 20);
			this.AddElementFieldMapping("FedExProfileEntity", "SmartPostIndicia", "SmartPostIndicia", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 21);
			this.AddElementFieldMapping("FedExProfileEntity", "SmartPostEndorsement", "SmartPostEndorsement", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 22);
			this.AddElementFieldMapping("FedExProfileEntity", "SmartPostConfirmation", "SmartPostConfirmation", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 23);
			this.AddElementFieldMapping("FedExProfileEntity", "SmartPostCustomerManifest", "SmartPostCustomerManifest", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 24);
			this.AddElementFieldMapping("FedExProfileEntity", "SmartPostHubID", "SmartPostHubID", true, "VarChar", 10, 0, 0, false, "", null, typeof(System.String), 25);
			this.AddElementFieldMapping("FedExProfileEntity", "EmailNotifyBroker", "EmailNotifyBroker", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 26);
			this.AddElementFieldMapping("FedExProfileEntity", "DropoffType", "DropoffType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 27);
			this.AddElementFieldMapping("FedExProfileEntity", "OriginResidentialDetermination", "OriginResidentialDetermination", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 28);
			this.AddElementFieldMapping("FedExProfileEntity", "PayorTransportName", "PayorTransportName", true, "NChar", 60, 0, 0, false, "", null, typeof(System.String), 29);
			this.AddElementFieldMapping("FedExProfileEntity", "ReturnType", "ReturnType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 30);
			this.AddElementFieldMapping("FedExProfileEntity", "RmaNumber", "RmaNumber", true, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 31);
			this.AddElementFieldMapping("FedExProfileEntity", "RmaReason", "RmaReason", true, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 32);
			this.AddElementFieldMapping("FedExProfileEntity", "ReturnSaturdayPickup", "ReturnSaturdayPickup", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 33);
			this.AddElementFieldMapping("FedExProfileEntity", "ReturnsClearance", "ReturnsClearance", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 34);
			this.AddElementFieldMapping("FedExProfileEntity", "ReferenceFIMS", "ReferenceFIMS", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 35);
			this.AddElementFieldMapping("FedExProfileEntity", "ThirdPartyConsignee", "ThirdPartyConsignee", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 36);
		}

		/// <summary>Inits FedExProfilePackageEntity's mappings</summary>
		private void InitFedExProfilePackageEntityMappings()
		{
			this.AddElementMapping("FedExProfilePackageEntity", @"ShipWorksLocal", @"dbo", "FedExProfilePackage", 34, 0);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "FedExProfilePackageID", "FedExProfilePackageID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "ShippingProfileID", "ShippingProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "Weight", "Weight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 2);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "DimsProfileID", "DimsProfileID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "DimsLength", "DimsLength", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 4);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "DimsWidth", "DimsWidth", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 5);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "DimsHeight", "DimsHeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 6);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "DimsWeight", "DimsWeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 7);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "DimsAddWeight", "DimsAddWeight", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 8);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "PriorityAlert", "PriorityAlert", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 9);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "PriorityAlertEnhancementType", "PriorityAlertEnhancementType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 10);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "PriorityAlertDetailContent", "PriorityAlertDetailContent", true, "NVarChar", 1024, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "DryIceWeight", "DryIceWeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 12);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "ContainsAlcohol", "ContainsAlcohol", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 13);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "DangerousGoodsEnabled", "DangerousGoodsEnabled", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 14);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "DangerousGoodsType", "DangerousGoodsType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 15);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "DangerousGoodsAccessibilityType", "DangerousGoodsAccessibilityType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 16);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "DangerousGoodsCargoAircraftOnly", "DangerousGoodsCargoAircraftOnly", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 17);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "DangerousGoodsEmergencyContactPhone", "DangerousGoodsEmergencyContactPhone", true, "NVarChar", 16, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "DangerousGoodsOfferor", "DangerousGoodsOfferor", true, "NVarChar", 128, 0, 0, false, "", null, typeof(System.String), 19);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "DangerousGoodsPackagingCount", "DangerousGoodsPackagingCount", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 20);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "HazardousMaterialNumber", "HazardousMaterialNumber", true, "NVarChar", 16, 0, 0, false, "", null, typeof(System.String), 21);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "HazardousMaterialClass", "HazardousMaterialClass", true, "NVarChar", 8, 0, 0, false, "", null, typeof(System.String), 22);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "HazardousMaterialProperName", "HazardousMaterialProperName", true, "NVarChar", 64, 0, 0, false, "", null, typeof(System.String), 23);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "HazardousMaterialPackingGroup", "HazardousMaterialPackingGroup", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 24);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "HazardousMaterialQuantityValue", "HazardousMaterialQuantityValue", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 25);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "HazardousMaterialQuanityUnits", "HazardousMaterialQuanityUnits", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 26);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "SignatoryContactName", "SignatoryContactName", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 27);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "SignatoryTitle", "SignatoryTitle", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 28);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "SignatoryPlace", "SignatoryPlace", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 29);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "ContainerType", "ContainerType", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 30);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "NumberOfContainers", "NumberOfContainers", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 31);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "PackingDetailsCargoAircraftOnly", "PackingDetailsCargoAircraftOnly", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 32);
			this.AddElementFieldMapping("FedExProfilePackageEntity", "PackingDetailsPackingInstructions", "PackingDetailsPackingInstructions", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 33);
		}

		/// <summary>Inits FedExShipmentEntity's mappings</summary>
		private void InitFedExShipmentEntityMappings()
		{
			this.AddElementMapping("FedExShipmentEntity", @"ShipWorksLocal", @"dbo", "FedExShipment", 156, 0);
			this.AddElementFieldMapping("FedExShipmentEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("FedExShipmentEntity", "FedExAccountID", "FedExAccountID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("FedExShipmentEntity", "MasterFormID", "MasterFormID", false, "VarChar", 4, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("FedExShipmentEntity", "Service", "Service", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("FedExShipmentEntity", "Signature", "Signature", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("FedExShipmentEntity", "PackagingType", "PackagingType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("FedExShipmentEntity", "NonStandardContainer", "NonStandardContainer", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
			this.AddElementFieldMapping("FedExShipmentEntity", "ReferenceCustomer", "ReferenceCustomer", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("FedExShipmentEntity", "ReferenceInvoice", "ReferenceInvoice", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("FedExShipmentEntity", "ReferencePO", "ReferencePO", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("FedExShipmentEntity", "ReferenceShipmentIntegrity", "ReferenceShipmentIntegrity", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("FedExShipmentEntity", "PayorTransportType", "PayorTransportType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 11);
			this.AddElementFieldMapping("FedExShipmentEntity", "PayorTransportName", "PayorTransportName", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("FedExShipmentEntity", "PayorTransportAccount", "PayorTransportAccount", false, "VarChar", 12, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("FedExShipmentEntity", "PayorDutiesType", "PayorDutiesType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 14);
			this.AddElementFieldMapping("FedExShipmentEntity", "PayorDutiesAccount", "PayorDutiesAccount", false, "VarChar", 12, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("FedExShipmentEntity", "PayorDutiesName", "PayorDutiesName", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("FedExShipmentEntity", "PayorDutiesCountryCode", "PayorDutiesCountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 17);
			this.AddElementFieldMapping("FedExShipmentEntity", "SaturdayDelivery", "SaturdayDelivery", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 18);
			this.AddElementFieldMapping("FedExShipmentEntity", "HomeDeliveryType", "HomeDeliveryType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 19);
			this.AddElementFieldMapping("FedExShipmentEntity", "HomeDeliveryInstructions", "HomeDeliveryInstructions", false, "VarChar", 74, 0, 0, false, "", null, typeof(System.String), 20);
			this.AddElementFieldMapping("FedExShipmentEntity", "HomeDeliveryDate", "HomeDeliveryDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 21);
			this.AddElementFieldMapping("FedExShipmentEntity", "HomeDeliveryPhone", "HomeDeliveryPhone", false, "VarChar", 24, 0, 0, false, "", null, typeof(System.String), 22);
			this.AddElementFieldMapping("FedExShipmentEntity", "FreightInsidePickup", "FreightInsidePickup", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 23);
			this.AddElementFieldMapping("FedExShipmentEntity", "FreightInsideDelivery", "FreightInsideDelivery", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 24);
			this.AddElementFieldMapping("FedExShipmentEntity", "FreightBookingNumber", "FreightBookingNumber", false, "VarChar", 12, 0, 0, false, "", null, typeof(System.String), 25);
			this.AddElementFieldMapping("FedExShipmentEntity", "FreightLoadAndCount", "FreightLoadAndCount", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 26);
			this.AddElementFieldMapping("FedExShipmentEntity", "EmailNotifyBroker", "EmailNotifyBroker", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 27);
			this.AddElementFieldMapping("FedExShipmentEntity", "EmailNotifySender", "EmailNotifySender", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 28);
			this.AddElementFieldMapping("FedExShipmentEntity", "EmailNotifyRecipient", "EmailNotifyRecipient", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 29);
			this.AddElementFieldMapping("FedExShipmentEntity", "EmailNotifyOther", "EmailNotifyOther", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 30);
			this.AddElementFieldMapping("FedExShipmentEntity", "EmailNotifyOtherAddress", "EmailNotifyOtherAddress", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 31);
			this.AddElementFieldMapping("FedExShipmentEntity", "EmailNotifyMessage", "EmailNotifyMessage", false, "VarChar", 120, 0, 0, false, "", null, typeof(System.String), 32);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodEnabled", "CodEnabled", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 33);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodAmount", "CodAmount", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 34);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodPaymentType", "CodPaymentType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 35);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodAddFreight", "CodAddFreight", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 36);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodOriginID", "CodOriginID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 37);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodFirstName", "CodFirstName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 38);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodLastName", "CodLastName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 39);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodCompany", "CodCompany", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 40);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodStreet1", "CodStreet1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 41);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodStreet2", "CodStreet2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 42);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodStreet3", "CodStreet3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 43);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodCity", "CodCity", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 44);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodStateProvCode", "CodStateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 45);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodPostalCode", "CodPostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 46);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodCountryCode", "CodCountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 47);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodPhone", "CodPhone", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 48);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodTrackingNumber", "CodTrackingNumber", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 49);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodTrackingFormID", "CodTrackingFormID", false, "VarChar", 4, 0, 0, false, "", null, typeof(System.String), 50);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodTIN", "CodTIN", false, "NVarChar", 24, 0, 0, false, "", null, typeof(System.String), 51);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodChargeBasis", "CodChargeBasis", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 52);
			this.AddElementFieldMapping("FedExShipmentEntity", "CodAccountNumber", "CodAccountNumber", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 53);
			this.AddElementFieldMapping("FedExShipmentEntity", "BrokerEnabled", "BrokerEnabled", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 54);
			this.AddElementFieldMapping("FedExShipmentEntity", "BrokerAccount", "BrokerAccount", false, "NVarChar", 12, 0, 0, false, "", null, typeof(System.String), 55);
			this.AddElementFieldMapping("FedExShipmentEntity", "BrokerFirstName", "BrokerFirstName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 56);
			this.AddElementFieldMapping("FedExShipmentEntity", "BrokerLastName", "BrokerLastName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 57);
			this.AddElementFieldMapping("FedExShipmentEntity", "BrokerCompany", "BrokerCompany", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 58);
			this.AddElementFieldMapping("FedExShipmentEntity", "BrokerStreet1", "BrokerStreet1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 59);
			this.AddElementFieldMapping("FedExShipmentEntity", "BrokerStreet2", "BrokerStreet2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 60);
			this.AddElementFieldMapping("FedExShipmentEntity", "BrokerStreet3", "BrokerStreet3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 61);
			this.AddElementFieldMapping("FedExShipmentEntity", "BrokerCity", "BrokerCity", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 62);
			this.AddElementFieldMapping("FedExShipmentEntity", "BrokerStateProvCode", "BrokerStateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 63);
			this.AddElementFieldMapping("FedExShipmentEntity", "BrokerPostalCode", "BrokerPostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 64);
			this.AddElementFieldMapping("FedExShipmentEntity", "BrokerCountryCode", "BrokerCountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 65);
			this.AddElementFieldMapping("FedExShipmentEntity", "BrokerPhone", "BrokerPhone", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 66);
			this.AddElementFieldMapping("FedExShipmentEntity", "BrokerPhoneExtension", "BrokerPhoneExtension", false, "NVarChar", 8, 0, 0, false, "", null, typeof(System.String), 67);
			this.AddElementFieldMapping("FedExShipmentEntity", "BrokerEmail", "BrokerEmail", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 68);
			this.AddElementFieldMapping("FedExShipmentEntity", "CustomsAdmissibilityPackaging", "CustomsAdmissibilityPackaging", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 69);
			this.AddElementFieldMapping("FedExShipmentEntity", "CustomsRecipientTIN", "CustomsRecipientTIN", false, "VarChar", 24, 0, 0, false, "", null, typeof(System.String), 70);
			this.AddElementFieldMapping("FedExShipmentEntity", "CustomsDocumentsOnly", "CustomsDocumentsOnly", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 71);
			this.AddElementFieldMapping("FedExShipmentEntity", "CustomsDocumentsDescription", "CustomsDocumentsDescription", false, "NVarChar", 150, 0, 0, false, "", null, typeof(System.String), 72);
			this.AddElementFieldMapping("FedExShipmentEntity", "CustomsExportFilingOption", "CustomsExportFilingOption", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 73);
			this.AddElementFieldMapping("FedExShipmentEntity", "CustomsAESEEI", "CustomsAESEEI", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 74);
			this.AddElementFieldMapping("FedExShipmentEntity", "CustomsRecipientIdentificationType", "CustomsRecipientIdentificationType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 75);
			this.AddElementFieldMapping("FedExShipmentEntity", "CustomsRecipientIdentificationValue", "CustomsRecipientIdentificationValue", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 76);
			this.AddElementFieldMapping("FedExShipmentEntity", "CustomsOptionsType", "CustomsOptionsType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 77);
			this.AddElementFieldMapping("FedExShipmentEntity", "CustomsOptionsDesription", "CustomsOptionsDesription", false, "NVarChar", 32, 0, 0, false, "", null, typeof(System.String), 78);
			this.AddElementFieldMapping("FedExShipmentEntity", "CommercialInvoice", "CommercialInvoice", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 79);
			this.AddElementFieldMapping("FedExShipmentEntity", "CommercialInvoiceFileElectronically", "CommercialInvoiceFileElectronically", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 80);
			this.AddElementFieldMapping("FedExShipmentEntity", "CommercialInvoiceTermsOfSale", "CommercialInvoiceTermsOfSale", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 81);
			this.AddElementFieldMapping("FedExShipmentEntity", "CommercialInvoicePurpose", "CommercialInvoicePurpose", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 82);
			this.AddElementFieldMapping("FedExShipmentEntity", "CommercialInvoiceComments", "CommercialInvoiceComments", false, "NVarChar", 200, 0, 0, false, "", null, typeof(System.String), 83);
			this.AddElementFieldMapping("FedExShipmentEntity", "CommercialInvoiceFreight", "CommercialInvoiceFreight", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 84);
			this.AddElementFieldMapping("FedExShipmentEntity", "CommercialInvoiceInsurance", "CommercialInvoiceInsurance", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 85);
			this.AddElementFieldMapping("FedExShipmentEntity", "CommercialInvoiceOther", "CommercialInvoiceOther", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 86);
			this.AddElementFieldMapping("FedExShipmentEntity", "CommercialInvoiceReference", "CommercialInvoiceReference", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 87);
			this.AddElementFieldMapping("FedExShipmentEntity", "ImporterOfRecord", "ImporterOfRecord", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 88);
			this.AddElementFieldMapping("FedExShipmentEntity", "ImporterAccount", "ImporterAccount", false, "NVarChar", 12, 0, 0, false, "", null, typeof(System.String), 89);
			this.AddElementFieldMapping("FedExShipmentEntity", "ImporterTIN", "ImporterTIN", false, "NVarChar", 24, 0, 0, false, "", null, typeof(System.String), 90);
			this.AddElementFieldMapping("FedExShipmentEntity", "ImporterFirstName", "ImporterFirstName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 91);
			this.AddElementFieldMapping("FedExShipmentEntity", "ImporterLastName", "ImporterLastName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 92);
			this.AddElementFieldMapping("FedExShipmentEntity", "ImporterCompany", "ImporterCompany", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 93);
			this.AddElementFieldMapping("FedExShipmentEntity", "ImporterStreet1", "ImporterStreet1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 94);
			this.AddElementFieldMapping("FedExShipmentEntity", "ImporterStreet2", "ImporterStreet2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 95);
			this.AddElementFieldMapping("FedExShipmentEntity", "ImporterStreet3", "ImporterStreet3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 96);
			this.AddElementFieldMapping("FedExShipmentEntity", "ImporterCity", "ImporterCity", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 97);
			this.AddElementFieldMapping("FedExShipmentEntity", "ImporterStateProvCode", "ImporterStateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 98);
			this.AddElementFieldMapping("FedExShipmentEntity", "ImporterPostalCode", "ImporterPostalCode", false, "NVarChar", 10, 0, 0, false, "", null, typeof(System.String), 99);
			this.AddElementFieldMapping("FedExShipmentEntity", "ImporterCountryCode", "ImporterCountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 100);
			this.AddElementFieldMapping("FedExShipmentEntity", "ImporterPhone", "ImporterPhone", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 101);
			this.AddElementFieldMapping("FedExShipmentEntity", "SmartPostIndicia", "SmartPostIndicia", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 102);
			this.AddElementFieldMapping("FedExShipmentEntity", "SmartPostEndorsement", "SmartPostEndorsement", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 103);
			this.AddElementFieldMapping("FedExShipmentEntity", "SmartPostConfirmation", "SmartPostConfirmation", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 104);
			this.AddElementFieldMapping("FedExShipmentEntity", "SmartPostCustomerManifest", "SmartPostCustomerManifest", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 105);
			this.AddElementFieldMapping("FedExShipmentEntity", "SmartPostHubID", "SmartPostHubID", false, "VarChar", 10, 0, 0, false, "", null, typeof(System.String), 106);
			this.AddElementFieldMapping("FedExShipmentEntity", "SmartPostUspsApplicationId", "SmartPostUspsApplicationId", false, "NVarChar", 10, 0, 0, false, "", null, typeof(System.String), 107);
			this.AddElementFieldMapping("FedExShipmentEntity", "DropoffType", "DropoffType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 108);
			this.AddElementFieldMapping("FedExShipmentEntity", "OriginResidentialDetermination", "OriginResidentialDetermination", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 109);
			this.AddElementFieldMapping("FedExShipmentEntity", "FedExHoldAtLocationEnabled", "FedExHoldAtLocationEnabled", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 110);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldLocationId", "HoldLocationId", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 111);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldLocationType", "HoldLocationType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 112);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldContactId", "HoldContactId", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 113);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldPersonName", "HoldPersonName", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 114);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldTitle", "HoldTitle", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 115);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldCompanyName", "HoldCompanyName", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 116);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldPhoneNumber", "HoldPhoneNumber", true, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 117);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldPhoneExtension", "HoldPhoneExtension", true, "NVarChar", 10, 0, 0, false, "", null, typeof(System.String), 118);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldPagerNumber", "HoldPagerNumber", true, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 119);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldFaxNumber", "HoldFaxNumber", true, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 120);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldEmailAddress", "HoldEmailAddress", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 121);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldStreet1", "HoldStreet1", true, "NVarChar", 250, 0, 0, false, "", null, typeof(System.String), 122);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldStreet2", "HoldStreet2", true, "NVarChar", 250, 0, 0, false, "", null, typeof(System.String), 123);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldStreet3", "HoldStreet3", true, "NVarChar", 250, 0, 0, false, "", null, typeof(System.String), 124);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldCity", "HoldCity", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 125);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldStateOrProvinceCode", "HoldStateOrProvinceCode", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 126);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldPostalCode", "HoldPostalCode", true, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 127);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldUrbanizationCode", "HoldUrbanizationCode", true, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 128);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldCountryCode", "HoldCountryCode", true, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 129);
			this.AddElementFieldMapping("FedExShipmentEntity", "HoldResidential", "HoldResidential", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 130);
			this.AddElementFieldMapping("FedExShipmentEntity", "CustomsNaftaEnabled", "CustomsNaftaEnabled", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 131);
			this.AddElementFieldMapping("FedExShipmentEntity", "CustomsNaftaPreferenceType", "CustomsNaftaPreferenceType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 132);
			this.AddElementFieldMapping("FedExShipmentEntity", "CustomsNaftaDeterminationCode", "CustomsNaftaDeterminationCode", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 133);
			this.AddElementFieldMapping("FedExShipmentEntity", "CustomsNaftaProducerId", "CustomsNaftaProducerId", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 134);
			this.AddElementFieldMapping("FedExShipmentEntity", "CustomsNaftaNetCostMethod", "CustomsNaftaNetCostMethod", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 135);
			this.AddElementFieldMapping("FedExShipmentEntity", "ReturnType", "ReturnType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 136);
			this.AddElementFieldMapping("FedExShipmentEntity", "RmaNumber", "RmaNumber", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 137);
			this.AddElementFieldMapping("FedExShipmentEntity", "RmaReason", "RmaReason", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 138);
			this.AddElementFieldMapping("FedExShipmentEntity", "ReturnSaturdayPickup", "ReturnSaturdayPickup", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 139);
			this.AddElementFieldMapping("FedExShipmentEntity", "TrafficInArmsLicenseNumber", "TrafficInArmsLicenseNumber", false, "NVarChar", 32, 0, 0, false, "", null, typeof(System.String), 140);
			this.AddElementFieldMapping("FedExShipmentEntity", "IntlExportDetailType", "IntlExportDetailType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 141);
			this.AddElementFieldMapping("FedExShipmentEntity", "IntlExportDetailForeignTradeZoneCode", "IntlExportDetailForeignTradeZoneCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 142);
			this.AddElementFieldMapping("FedExShipmentEntity", "IntlExportDetailEntryNumber", "IntlExportDetailEntryNumber", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 143);
			this.AddElementFieldMapping("FedExShipmentEntity", "IntlExportDetailLicenseOrPermitNumber", "IntlExportDetailLicenseOrPermitNumber", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 144);
			this.AddElementFieldMapping("FedExShipmentEntity", "IntlExportDetailLicenseOrPermitExpirationDate", "IntlExportDetailLicenseOrPermitExpirationDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 145);
			this.AddElementFieldMapping("FedExShipmentEntity", "WeightUnitType", "WeightUnitType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 146);
			this.AddElementFieldMapping("FedExShipmentEntity", "LinearUnitType", "LinearUnitType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 147);
			this.AddElementFieldMapping("FedExShipmentEntity", "RequestedLabelFormat", "RequestedLabelFormat", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 148);
			this.AddElementFieldMapping("FedExShipmentEntity", "FimsAirWaybill", "FimsAirWaybill", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 149);
			this.AddElementFieldMapping("FedExShipmentEntity", "ReturnsClearance", "ReturnsClearance", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 150);
			this.AddElementFieldMapping("FedExShipmentEntity", "MaskedData", "MaskedData", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 151);
			this.AddElementFieldMapping("FedExShipmentEntity", "ReferenceFIMS", "ReferenceFIMS", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 152);
			this.AddElementFieldMapping("FedExShipmentEntity", "ThirdPartyConsignee", "ThirdPartyConsignee", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 153);
			this.AddElementFieldMapping("FedExShipmentEntity", "Currency", "Currency", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 154);
			this.AddElementFieldMapping("FedExShipmentEntity", "InternationalTrafficInArmsService", "InternationalTrafficInArmsService", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 155);
		}

		/// <summary>Inits FilterEntity's mappings</summary>
		private void InitFilterEntityMappings()
		{
			this.AddElementMapping("FilterEntity", @"ShipWorksLocal", @"dbo", "Filter", 7, 0);
			this.AddElementFieldMapping("FilterEntity", "FilterID", "FilterID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("FilterEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("FilterEntity", "Name", "Name", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("FilterEntity", "FilterTarget", "FilterTarget", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("FilterEntity", "IsFolder", "IsFolder", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 4);
			this.AddElementFieldMapping("FilterEntity", "Definition", "Definition", true, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("FilterEntity", "State", "State", false, "TinyInt", 0, 3, 0, false, "", null, typeof(System.Byte), 6);
		}

		/// <summary>Inits FilterLayoutEntity's mappings</summary>
		private void InitFilterLayoutEntityMappings()
		{
			this.AddElementMapping("FilterLayoutEntity", @"ShipWorksLocal", @"dbo", "FilterLayout", 5, 0);
			this.AddElementFieldMapping("FilterLayoutEntity", "FilterLayoutID", "FilterLayoutID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("FilterLayoutEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("FilterLayoutEntity", "UserID", "UserID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("FilterLayoutEntity", "FilterTarget", "FilterTarget", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("FilterLayoutEntity", "FilterNodeID", "FilterNodeID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
		}

		/// <summary>Inits FilterNodeEntity's mappings</summary>
		private void InitFilterNodeEntityMappings()
		{
			this.AddElementMapping("FilterNodeEntity", @"ShipWorksLocal", @"dbo", "FilterNode", 7, 0);
			this.AddElementFieldMapping("FilterNodeEntity", "FilterNodeID", "FilterNodeID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("FilterNodeEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("FilterNodeEntity", "ParentFilterNodeID", "ParentFilterNodeID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("FilterNodeEntity", "FilterSequenceID", "FilterSequenceID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("FilterNodeEntity", "FilterNodeContentID", "FilterNodeContentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
			this.AddElementFieldMapping("FilterNodeEntity", "Created", "Created", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 5);
			this.AddElementFieldMapping("FilterNodeEntity", "Purpose", "Purpose", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
		}

		/// <summary>Inits FilterNodeColumnSettingsEntity's mappings</summary>
		private void InitFilterNodeColumnSettingsEntityMappings()
		{
			this.AddElementMapping("FilterNodeColumnSettingsEntity", @"ShipWorksLocal", @"dbo", "FilterNodeColumnSettings", 5, 0);
			this.AddElementFieldMapping("FilterNodeColumnSettingsEntity", "FilterNodeColumnSettingsID", "FilterNodeColumnSettingsID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("FilterNodeColumnSettingsEntity", "UserID", "UserID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("FilterNodeColumnSettingsEntity", "FilterNodeID", "FilterNodeID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("FilterNodeColumnSettingsEntity", "Inherit", "Inherit", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("FilterNodeColumnSettingsEntity", "GridColumnLayoutID", "GridColumnLayoutID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
		}

		/// <summary>Inits FilterNodeContentEntity's mappings</summary>
		private void InitFilterNodeContentEntityMappings()
		{
			this.AddElementMapping("FilterNodeContentEntity", @"ShipWorksLocal", @"dbo", "FilterNodeContent", 10, 0);
			this.AddElementFieldMapping("FilterNodeContentEntity", "FilterNodeContentID", "FilterNodeContentID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("FilterNodeContentEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("FilterNodeContentEntity", "CountVersion", "CountVersion", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("FilterNodeContentEntity", "Status", "Status", false, "SmallInt", 0, 5, 0, false, "", null, typeof(System.Int16), 3);
			this.AddElementFieldMapping("FilterNodeContentEntity", "InitialCalculation", "InitialCalculation", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("FilterNodeContentEntity", "UpdateCalculation", "UpdateCalculation", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("FilterNodeContentEntity", "ColumnMask", "ColumnMask", false, "VarBinary", 100, 0, 0, false, "", null, typeof(System.Byte[]), 6);
			this.AddElementFieldMapping("FilterNodeContentEntity", "JoinMask", "JoinMask", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 7);
			this.AddElementFieldMapping("FilterNodeContentEntity", "Cost", "Cost", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 8);
			this.AddElementFieldMapping("FilterNodeContentEntity", "Count", "Count", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 9);
		}

		/// <summary>Inits FilterNodeContentDetailEntity's mappings</summary>
		private void InitFilterNodeContentDetailEntityMappings()
		{
			this.AddElementMapping("FilterNodeContentDetailEntity", @"ShipWorksLocal", @"dbo", "FilterNodeContentDetail", 2, 2);
			this.AddElementFieldMapping("FilterNodeContentDetailEntity", "FilterNodeContentID", "FilterNodeContentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("FilterNodeContentDetailEntity", "EntityID", "ObjectID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
		}

		/// <summary>Inits FilterSequenceEntity's mappings</summary>
		private void InitFilterSequenceEntityMappings()
		{
			this.AddElementMapping("FilterSequenceEntity", @"ShipWorksLocal", @"dbo", "FilterSequence", 5, 0);
			this.AddElementFieldMapping("FilterSequenceEntity", "FilterSequenceID", "FilterSequenceID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("FilterSequenceEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("FilterSequenceEntity", "ParentFilterID", "ParentFilterID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("FilterSequenceEntity", "FilterID", "FilterID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("FilterSequenceEntity", "Position", "Position", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
		}

		/// <summary>Inits FtpAccountEntity's mappings</summary>
		private void InitFtpAccountEntityMappings()
		{
			this.AddElementMapping("FtpAccountEntity", @"ShipWorksLocal", @"dbo", "FtpAccount", 9, 0);
			this.AddElementFieldMapping("FtpAccountEntity", "FtpAccountID", "FtpAccountID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("FtpAccountEntity", "Host", "Host", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("FtpAccountEntity", "Username", "Username", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("FtpAccountEntity", "Password", "Password", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("FtpAccountEntity", "Port", "Port", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("FtpAccountEntity", "SecurityType", "SecurityType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("FtpAccountEntity", "Passive", "Passive", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
			this.AddElementFieldMapping("FtpAccountEntity", "InternalOwnerID", "InternalOwnerID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 7);
			this.AddElementFieldMapping("FtpAccountEntity", "ReuseControlConnectionSession", "ReuseControlConnectionSession", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 8);
		}

		/// <summary>Inits GenericFileStoreEntity's mappings</summary>
		private void InitGenericFileStoreEntityMappings()
		{
			this.AddElementMapping("GenericFileStoreEntity", @"ShipWorksLocal", @"dbo", "GenericFileStore", 20, 0);
			this.AddElementFieldMapping("GenericFileStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("GenericFileStoreEntity", "FileFormat", "FileFormat", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 1);
			this.AddElementFieldMapping("GenericFileStoreEntity", "FileSource", "FileSource", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("GenericFileStoreEntity", "DiskFolder", "DiskFolder", false, "NVarChar", 355, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("GenericFileStoreEntity", "FtpAccountID", "FtpAccountID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
			this.AddElementFieldMapping("GenericFileStoreEntity", "FtpFolder", "FtpFolder", false, "NVarChar", 355, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("GenericFileStoreEntity", "EmailAccountID", "EmailAccountID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 6);
			this.AddElementFieldMapping("GenericFileStoreEntity", "EmailIncomingFolder", "EmailFolder", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("GenericFileStoreEntity", "EmailFolderValidityID", "EmailFolderValidityID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 8);
			this.AddElementFieldMapping("GenericFileStoreEntity", "EmailFolderLastMessageID", "EmailFolderLastMessageID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 9);
			this.AddElementFieldMapping("GenericFileStoreEntity", "EmailOnlyUnread", "EmailOnlyUnread", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 10);
			this.AddElementFieldMapping("GenericFileStoreEntity", "NamePatternMatch", "NamePatternMatch", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("GenericFileStoreEntity", "NamePatternSkip", "NamePatternSkip", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("GenericFileStoreEntity", "SuccessAction", "SuccessAction", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 13);
			this.AddElementFieldMapping("GenericFileStoreEntity", "SuccessMoveFolder", "SuccessMoveFolder", false, "NVarChar", 355, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("GenericFileStoreEntity", "ErrorAction", "ErrorAction", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 15);
			this.AddElementFieldMapping("GenericFileStoreEntity", "ErrorMoveFolder", "ErrorMoveFolder", false, "NVarChar", 355, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("GenericFileStoreEntity", "XmlXsltFileName", "XmlXsltFileName", true, "NVarChar", 355, 0, 0, false, "", null, typeof(System.String), 17);
			this.AddElementFieldMapping("GenericFileStoreEntity", "XmlXsltContent", "XmlXsltContent", true, "NText", 1073741823, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("GenericFileStoreEntity", "FlatImportMap", "FlatImportMap", false, "NText", 1073741823, 0, 0, false, "", null, typeof(System.String), 19);
		}

		/// <summary>Inits GenericModuleStoreEntity's mappings</summary>
		private void InitGenericModuleStoreEntityMappings()
		{
			this.AddElementMapping("GenericModuleStoreEntity", @"ShipWorksLocal", @"dbo", "GenericModuleStore", 20, 0);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModuleUsername", "ModuleUsername", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModulePassword", "ModulePassword", false, "NVarChar", 80, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModuleUrl", "ModuleUrl", false, "NVarChar", 350, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModuleVersion", "ModuleVersion", false, "VarChar", 20, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModulePlatform", "ModulePlatform", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModuleDeveloper", "ModuleDeveloper", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModuleOnlineStoreCode", "ModuleOnlineStoreCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModuleStatusCodes", "ModuleStatusCodes", false, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModuleDownloadPageSize", "ModuleDownloadPageSize", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 9);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModuleRequestTimeout", "ModuleRequestTimeout", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 10);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModuleDownloadStrategy", "ModuleDownloadStrategy", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 11);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModuleOnlineStatusSupport", "ModuleOnlineStatusSupport", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 12);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModuleOnlineStatusDataType", "ModuleOnlineStatusDataType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 13);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModuleOnlineCustomerSupport", "ModuleOnlineCustomerSupport", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 14);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModuleOnlineCustomerDataType", "ModuleOnlineCustomerDataType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 15);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModuleOnlineShipmentDetails", "ModuleOnlineShipmentDetails", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 16);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModuleHttpExpect100Continue", "ModuleHttpExpect100Continue", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 17);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "ModuleResponseEncoding", "ModuleResponseEncoding", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 18);
			this.AddElementFieldMapping("GenericModuleStoreEntity", "SchemaVersion", "SchemaVersion", false, "VarChar", 20, 0, 0, false, "", null, typeof(System.String), 19);
		}

		/// <summary>Inits GridColumnFormatEntity's mappings</summary>
		private void InitGridColumnFormatEntityMappings()
		{
			this.AddElementMapping("GridColumnFormatEntity", @"ShipWorksLocal", @"dbo", "GridColumnFormat", 4, 0);
			this.AddElementFieldMapping("GridColumnFormatEntity", "GridColumnFormatID", "GridColumnFormatID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("GridColumnFormatEntity", "UserID", "UserID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("GridColumnFormatEntity", "ColumnGuid", "ColumnGuid", false, "UniqueIdentifier", 0, 0, 0, false, "", null, typeof(System.Guid), 2);
			this.AddElementFieldMapping("GridColumnFormatEntity", "Settings", "Settings", false, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 3);
		}

		/// <summary>Inits GridColumnLayoutEntity's mappings</summary>
		private void InitGridColumnLayoutEntityMappings()
		{
			this.AddElementMapping("GridColumnLayoutEntity", @"ShipWorksLocal", @"dbo", "GridColumnLayout", 7, 0);
			this.AddElementFieldMapping("GridColumnLayoutEntity", "GridColumnLayoutID", "GridColumnLayoutID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("GridColumnLayoutEntity", "DefinitionSet", "DefinitionSet", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 1);
			this.AddElementFieldMapping("GridColumnLayoutEntity", "DefaultSortColumnGuid", "DefaultSortColumnGuid", false, "UniqueIdentifier", 0, 0, 0, false, "", null, typeof(System.Guid), 2);
			this.AddElementFieldMapping("GridColumnLayoutEntity", "DefaultSortOrder", "DefaultSortOrder", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("GridColumnLayoutEntity", "LastSortColumnGuid", "LastSortColumnGuid", false, "UniqueIdentifier", 0, 0, 0, false, "", null, typeof(System.Guid), 4);
			this.AddElementFieldMapping("GridColumnLayoutEntity", "LastSortOrder", "LastSortOrder", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("GridColumnLayoutEntity", "DetailViewSettings", "DetailViewSettings", true, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 6);
		}

		/// <summary>Inits GridColumnPositionEntity's mappings</summary>
		private void InitGridColumnPositionEntityMappings()
		{
			this.AddElementMapping("GridColumnPositionEntity", @"ShipWorksLocal", @"dbo", "GridColumnPosition", 6, 0);
			this.AddElementFieldMapping("GridColumnPositionEntity", "GridColumnPositionID", "GridColumnPositionID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("GridColumnPositionEntity", "GridColumnLayoutID", "GridColumnLayoutID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("GridColumnPositionEntity", "ColumnGuid", "ColumnGuid", false, "UniqueIdentifier", 0, 0, 0, false, "", null, typeof(System.Guid), 2);
			this.AddElementFieldMapping("GridColumnPositionEntity", "Visible", "Visible", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("GridColumnPositionEntity", "Width", "Width", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("GridColumnPositionEntity", "Position", "Position", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
		}

		/// <summary>Inits GrouponOrderEntity's mappings</summary>
		private void InitGrouponOrderEntityMappings()
		{
			this.AddElementMapping("GrouponOrderEntity", @"ShipWorksLocal", @"dbo", "GrouponOrder", 2, 0);
			this.AddElementFieldMapping("GrouponOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("GrouponOrderEntity", "GrouponOrderID", "GrouponOrderID", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
		}

		/// <summary>Inits GrouponOrderItemEntity's mappings</summary>
		private void InitGrouponOrderItemEntityMappings()
		{
			this.AddElementMapping("GrouponOrderItemEntity", @"ShipWorksLocal", @"dbo", "GrouponOrderItem", 6, 0);
			this.AddElementFieldMapping("GrouponOrderItemEntity", "OrderItemID", "OrderItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("GrouponOrderItemEntity", "Permalink", "Permalink", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("GrouponOrderItemEntity", "ChannelSKUProvided", "ChannelSKUProvided", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("GrouponOrderItemEntity", "FulfillmentLineItemID", "FulfillmentLineItemID", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("GrouponOrderItemEntity", "BomSKU", "BomSKU", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("GrouponOrderItemEntity", "GrouponLineItemID", "GrouponLineItemID", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 5);
		}

		/// <summary>Inits GrouponStoreEntity's mappings</summary>
		private void InitGrouponStoreEntityMappings()
		{
			this.AddElementMapping("GrouponStoreEntity", @"ShipWorksLocal", @"dbo", "GrouponStore", 3, 0);
			this.AddElementFieldMapping("GrouponStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("GrouponStoreEntity", "SupplierID", "SupplierID", false, "VarChar", 255, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("GrouponStoreEntity", "Token", "Token", false, "VarChar", 255, 0, 0, false, "", null, typeof(System.String), 2);
		}

		/// <summary>Inits InfopiaOrderItemEntity's mappings</summary>
		private void InitInfopiaOrderItemEntityMappings()
		{
			this.AddElementMapping("InfopiaOrderItemEntity", @"ShipWorksLocal", @"dbo", "InfopiaOrderItem", 4, 0);
			this.AddElementFieldMapping("InfopiaOrderItemEntity", "OrderItemID", "OrderItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("InfopiaOrderItemEntity", "Marketplace", "Marketplace", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("InfopiaOrderItemEntity", "MarketplaceItemID", "MarketplaceItemID", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("InfopiaOrderItemEntity", "BuyerID", "BuyerID", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
		}

		/// <summary>Inits InfopiaStoreEntity's mappings</summary>
		private void InitInfopiaStoreEntityMappings()
		{
			this.AddElementMapping("InfopiaStoreEntity", @"ShipWorksLocal", @"dbo", "InfopiaStore", 2, 0);
			this.AddElementFieldMapping("InfopiaStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("InfopiaStoreEntity", "ApiToken", "ApiToken", false, "VarChar", 128, 0, 0, false, "", null, typeof(System.String), 1);
		}

		/// <summary>Inits InsurancePolicyEntity's mappings</summary>
		private void InitInsurancePolicyEntityMappings()
		{
			this.AddElementMapping("InsurancePolicyEntity", @"ShipWorksLocal", @"dbo", "InsurancePolicy", 10, 0);
			this.AddElementFieldMapping("InsurancePolicyEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("InsurancePolicyEntity", "InsureShipStoreName", "InsureShipStoreName", false, "NVarChar", 75, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("InsurancePolicyEntity", "CreatedWithApi", "CreatedWithApi", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 2);
			this.AddElementFieldMapping("InsurancePolicyEntity", "ItemName", "ItemName", true, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("InsurancePolicyEntity", "Description", "Description", true, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("InsurancePolicyEntity", "ClaimType", "ClaimType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("InsurancePolicyEntity", "DamageValue", "DamageValue", true, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 6);
			this.AddElementFieldMapping("InsurancePolicyEntity", "SubmissionDate", "SubmissionDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 7);
			this.AddElementFieldMapping("InsurancePolicyEntity", "ClaimID", "ClaimID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 8);
			this.AddElementFieldMapping("InsurancePolicyEntity", "EmailAddress", "EmailAddress", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 9);
		}

		/// <summary>Inits IParcelAccountEntity's mappings</summary>
		private void InitIParcelAccountEntityMappings()
		{
			this.AddElementMapping("IParcelAccountEntity", @"ShipWorksLocal", @"dbo", "iParcelAccount", 18, 0);
			this.AddElementFieldMapping("IParcelAccountEntity", "IParcelAccountID", "iParcelAccountID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("IParcelAccountEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("IParcelAccountEntity", "Username", "Username", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("IParcelAccountEntity", "Password", "Password", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("IParcelAccountEntity", "Description", "Description", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("IParcelAccountEntity", "FirstName", "FirstName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("IParcelAccountEntity", "MiddleName", "MiddleName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("IParcelAccountEntity", "LastName", "LastName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("IParcelAccountEntity", "Company", "Company", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("IParcelAccountEntity", "Street1", "Street1", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("IParcelAccountEntity", "Street2", "Street2", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("IParcelAccountEntity", "City", "City", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("IParcelAccountEntity", "StateProvCode", "StateProvCode", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("IParcelAccountEntity", "PostalCode", "PostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("IParcelAccountEntity", "CountryCode", "CountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("IParcelAccountEntity", "Phone", "Phone", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("IParcelAccountEntity", "Email", "Email", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("IParcelAccountEntity", "Website", "Website", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 17);
		}

		/// <summary>Inits IParcelPackageEntity's mappings</summary>
		private void InitIParcelPackageEntityMappings()
		{
			this.AddElementMapping("IParcelPackageEntity", @"ShipWorksLocal", @"dbo", "iParcelPackage", 16, 0);
			this.AddElementFieldMapping("IParcelPackageEntity", "IParcelPackageID", "iParcelPackageID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("IParcelPackageEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("IParcelPackageEntity", "Weight", "Weight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 2);
			this.AddElementFieldMapping("IParcelPackageEntity", "DimsProfileID", "DimsProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("IParcelPackageEntity", "DimsLength", "DimsLength", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 4);
			this.AddElementFieldMapping("IParcelPackageEntity", "DimsWidth", "DimsWidth", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 5);
			this.AddElementFieldMapping("IParcelPackageEntity", "DimsHeight", "DimsHeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 6);
			this.AddElementFieldMapping("IParcelPackageEntity", "DimsAddWeight", "DimsAddWeight", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 7);
			this.AddElementFieldMapping("IParcelPackageEntity", "DimsWeight", "DimsWeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 8);
			this.AddElementFieldMapping("IParcelPackageEntity", "Insurance", "Insurance", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 9);
			this.AddElementFieldMapping("IParcelPackageEntity", "InsuranceValue", "InsuranceValue", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 10);
			this.AddElementFieldMapping("IParcelPackageEntity", "InsurancePennyOne", "InsurancePennyOne", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 11);
			this.AddElementFieldMapping("IParcelPackageEntity", "DeclaredValue", "DeclaredValue", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 12);
			this.AddElementFieldMapping("IParcelPackageEntity", "TrackingNumber", "TrackingNumber", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("IParcelPackageEntity", "ParcelNumber", "ParcelNumber", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("IParcelPackageEntity", "SkuAndQuantities", "SkuAndQuantities", false, "NVarChar", 500, 0, 0, false, "", null, typeof(System.String), 15);
		}

		/// <summary>Inits IParcelProfileEntity's mappings</summary>
		private void InitIParcelProfileEntityMappings()
		{
			this.AddElementMapping("IParcelProfileEntity", @"ShipWorksLocal", @"dbo", "iParcelProfile", 8, 0);
			this.AddElementFieldMapping("IParcelProfileEntity", "ShippingProfileID", "ShippingProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("IParcelProfileEntity", "IParcelAccountID", "iParcelAccountID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("IParcelProfileEntity", "Service", "Service", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("IParcelProfileEntity", "Reference", "Reference", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("IParcelProfileEntity", "TrackByEmail", "TrackByEmail", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 4);
			this.AddElementFieldMapping("IParcelProfileEntity", "TrackBySMS", "TrackBySMS", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 5);
			this.AddElementFieldMapping("IParcelProfileEntity", "IsDeliveryDutyPaid", "IsDeliveryDutyPaid", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
			this.AddElementFieldMapping("IParcelProfileEntity", "SkuAndQuantities", "SkuAndQuantities", true, "NVarChar", 500, 0, 0, false, "", null, typeof(System.String), 7);
		}

		/// <summary>Inits IParcelProfilePackageEntity's mappings</summary>
		private void InitIParcelProfilePackageEntityMappings()
		{
			this.AddElementMapping("IParcelProfilePackageEntity", @"ShipWorksLocal", @"dbo", "iParcelProfilePackage", 9, 0);
			this.AddElementFieldMapping("IParcelProfilePackageEntity", "IParcelProfilePackageID", "iParcelProfilePackageID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("IParcelProfilePackageEntity", "ShippingProfileID", "ShippingProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("IParcelProfilePackageEntity", "Weight", "Weight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 2);
			this.AddElementFieldMapping("IParcelProfilePackageEntity", "DimsProfileID", "DimsProfileID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("IParcelProfilePackageEntity", "DimsLength", "DimsLength", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 4);
			this.AddElementFieldMapping("IParcelProfilePackageEntity", "DimsWidth", "DimsWidth", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 5);
			this.AddElementFieldMapping("IParcelProfilePackageEntity", "DimsHeight", "DimsHeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 6);
			this.AddElementFieldMapping("IParcelProfilePackageEntity", "DimsWeight", "DimsWeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 7);
			this.AddElementFieldMapping("IParcelProfilePackageEntity", "DimsAddWeight", "DimsAddWeight", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 8);
		}

		/// <summary>Inits IParcelShipmentEntity's mappings</summary>
		private void InitIParcelShipmentEntityMappings()
		{
			this.AddElementMapping("IParcelShipmentEntity", @"ShipWorksLocal", @"dbo", "iParcelShipment", 8, 0);
			this.AddElementFieldMapping("IParcelShipmentEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("IParcelShipmentEntity", "IParcelAccountID", "iParcelAccountID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("IParcelShipmentEntity", "Service", "Service", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("IParcelShipmentEntity", "Reference", "Reference", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("IParcelShipmentEntity", "TrackByEmail", "TrackByEmail", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 4);
			this.AddElementFieldMapping("IParcelShipmentEntity", "TrackBySMS", "TrackBySMS", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 5);
			this.AddElementFieldMapping("IParcelShipmentEntity", "IsDeliveryDutyPaid", "IsDeliveryDutyPaid", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
			this.AddElementFieldMapping("IParcelShipmentEntity", "RequestedLabelFormat", "RequestedLabelFormat", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 7);
		}

		/// <summary>Inits LabelSheetEntity's mappings</summary>
		private void InitLabelSheetEntityMappings()
		{
			this.AddElementMapping("LabelSheetEntity", @"ShipWorksLocal", @"dbo", "LabelSheet", 13, 0);
			this.AddElementFieldMapping("LabelSheetEntity", "LabelSheetID", "LabelSheetID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("LabelSheetEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("LabelSheetEntity", "Name", "Name", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("LabelSheetEntity", "PaperSizeHeight", "PaperSizeHeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 3);
			this.AddElementFieldMapping("LabelSheetEntity", "PaperSizeWidth", "PaperSizeWidth", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 4);
			this.AddElementFieldMapping("LabelSheetEntity", "MarginTop", "MarginTop", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 5);
			this.AddElementFieldMapping("LabelSheetEntity", "MarginLeft", "MarginLeft", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 6);
			this.AddElementFieldMapping("LabelSheetEntity", "LabelHeight", "LabelHeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 7);
			this.AddElementFieldMapping("LabelSheetEntity", "LabelWidth", "LabelWidth", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 8);
			this.AddElementFieldMapping("LabelSheetEntity", "VerticalSpacing", "VerticalSpacing", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 9);
			this.AddElementFieldMapping("LabelSheetEntity", "HorizontalSpacing", "HorizontalSpacing", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 10);
			this.AddElementFieldMapping("LabelSheetEntity", "Rows", "Rows", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 11);
			this.AddElementFieldMapping("LabelSheetEntity", "Columns", "Columns", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 12);
		}

		/// <summary>Inits LemonStandOrderEntity's mappings</summary>
		private void InitLemonStandOrderEntityMappings()
		{
			this.AddElementMapping("LemonStandOrderEntity", @"ShipWorksLocal", @"dbo", "LemonStandOrder", 2, 0);
			this.AddElementFieldMapping("LemonStandOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("LemonStandOrderEntity", "LemonStandOrderID", "LemonStandOrderID", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 1);
		}

		/// <summary>Inits LemonStandOrderItemEntity's mappings</summary>
		private void InitLemonStandOrderItemEntityMappings()
		{
			this.AddElementMapping("LemonStandOrderItemEntity", @"ShipWorksLocal", @"dbo", "LemonStandOrderItem", 4, 0);
			this.AddElementFieldMapping("LemonStandOrderItemEntity", "OrderItemID", "OrderItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("LemonStandOrderItemEntity", "UrlName", "UrlName", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("LemonStandOrderItemEntity", "ShortDescription", "ShortDescription", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("LemonStandOrderItemEntity", "Category", "Category", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 3);
		}

		/// <summary>Inits LemonStandStoreEntity's mappings</summary>
		private void InitLemonStandStoreEntityMappings()
		{
			this.AddElementMapping("LemonStandStoreEntity", @"ShipWorksLocal", @"dbo", "LemonStandStore", 4, 0);
			this.AddElementFieldMapping("LemonStandStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("LemonStandStoreEntity", "Token", "Token", false, "VarChar", 100, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("LemonStandStoreEntity", "StoreURL", "StoreURL", false, "VarChar", 255, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("LemonStandStoreEntity", "StatusCodes", "StatusCodes", true, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 3);
		}

		/// <summary>Inits MagentoOrderEntity's mappings</summary>
		private void InitMagentoOrderEntityMappings()
		{
			this.AddElementMapping("MagentoOrderEntity", @"ShipWorksLocal", @"dbo", "MagentoOrder", 2, 0);
			this.AddElementFieldMapping("MagentoOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("MagentoOrderEntity", "MagentoOrderID", "MagentoOrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
		}

		/// <summary>Inits MagentoStoreEntity's mappings</summary>
		private void InitMagentoStoreEntityMappings()
		{
			this.AddElementMapping("MagentoStoreEntity", @"ShipWorksLocal", @"dbo", "MagentoStore", 3, 0);
			this.AddElementFieldMapping("MagentoStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("MagentoStoreEntity", "MagentoTrackingEmails", "MagentoTrackingEmails", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 1);
			this.AddElementFieldMapping("MagentoStoreEntity", "MagentoVersion", "MagentoVersion", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
		}

		/// <summary>Inits MarketplaceAdvisorOrderEntity's mappings</summary>
		private void InitMarketplaceAdvisorOrderEntityMappings()
		{
			this.AddElementMapping("MarketplaceAdvisorOrderEntity", @"ShipWorksLocal", @"dbo", "MarketplaceAdvisorOrder", 5, 0);
			this.AddElementFieldMapping("MarketplaceAdvisorOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("MarketplaceAdvisorOrderEntity", "BuyerNumber", "BuyerNumber", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("MarketplaceAdvisorOrderEntity", "SellerOrderNumber", "SellerOrderNumber", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("MarketplaceAdvisorOrderEntity", "InvoiceNumber", "InvoiceNumber", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("MarketplaceAdvisorOrderEntity", "ParcelID", "ParcelID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
		}

		/// <summary>Inits MarketplaceAdvisorStoreEntity's mappings</summary>
		private void InitMarketplaceAdvisorStoreEntityMappings()
		{
			this.AddElementMapping("MarketplaceAdvisorStoreEntity", @"ShipWorksLocal", @"dbo", "MarketplaceAdvisorStore", 5, 0);
			this.AddElementFieldMapping("MarketplaceAdvisorStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("MarketplaceAdvisorStoreEntity", "Username", "Username", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("MarketplaceAdvisorStoreEntity", "Password", "Password", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("MarketplaceAdvisorStoreEntity", "AccountType", "AccountType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("MarketplaceAdvisorStoreEntity", "DownloadFlags", "DownloadFlags", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
		}

		/// <summary>Inits MivaOrderItemAttributeEntity's mappings</summary>
		private void InitMivaOrderItemAttributeEntityMappings()
		{
			this.AddElementMapping("MivaOrderItemAttributeEntity", @"ShipWorksLocal", @"dbo", "MivaOrderItemAttribute", 4, 0);
			this.AddElementFieldMapping("MivaOrderItemAttributeEntity", "OrderItemAttributeID", "OrderItemAttributeID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("MivaOrderItemAttributeEntity", "MivaOptionCode", "MivaOptionCode", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("MivaOrderItemAttributeEntity", "MivaAttributeID", "MivaAttributeID", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("MivaOrderItemAttributeEntity", "MivaAttributeCode", "MivaAttributeCode", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 3);
		}

		/// <summary>Inits MivaStoreEntity's mappings</summary>
		private void InitMivaStoreEntityMappings()
		{
			this.AddElementMapping("MivaStoreEntity", @"ShipWorksLocal", @"dbo", "MivaStore", 6, 0);
			this.AddElementFieldMapping("MivaStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("MivaStoreEntity", "EncryptionPassphrase", "EncryptionPassphrase", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("MivaStoreEntity", "LiveManualOrderNumbers", "LiveManualOrderNumbers", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 2);
			this.AddElementFieldMapping("MivaStoreEntity", "SebenzaCheckoutDataEnabled", "SebenzaCheckoutDataEnabled", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("MivaStoreEntity", "OnlineUpdateStrategy", "OnlineUpdateStrategy", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("MivaStoreEntity", "OnlineUpdateStatusChangeEmail", "OnlineUpdateStatusChangeEmail", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 5);
		}

		/// <summary>Inits NetworkSolutionsOrderEntity's mappings</summary>
		private void InitNetworkSolutionsOrderEntityMappings()
		{
			this.AddElementMapping("NetworkSolutionsOrderEntity", @"ShipWorksLocal", @"dbo", "NetworkSolutionsOrder", 2, 0);
			this.AddElementFieldMapping("NetworkSolutionsOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("NetworkSolutionsOrderEntity", "NetworkSolutionsOrderID", "NetworkSolutionsOrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
		}

		/// <summary>Inits NetworkSolutionsStoreEntity's mappings</summary>
		private void InitNetworkSolutionsStoreEntityMappings()
		{
			this.AddElementMapping("NetworkSolutionsStoreEntity", @"ShipWorksLocal", @"dbo", "NetworkSolutionsStore", 5, 0);
			this.AddElementFieldMapping("NetworkSolutionsStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("NetworkSolutionsStoreEntity", "UserToken", "UserToken", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("NetworkSolutionsStoreEntity", "DownloadOrderStatuses", "DownloadOrderStatuses", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("NetworkSolutionsStoreEntity", "StatusCodes", "StatusCodes", false, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("NetworkSolutionsStoreEntity", "StoreUrl", "StoreUrl", false, "VarChar", 255, 0, 0, false, "", null, typeof(System.String), 4);
		}

		/// <summary>Inits NeweggOrderEntity's mappings</summary>
		private void InitNeweggOrderEntityMappings()
		{
			this.AddElementMapping("NeweggOrderEntity", @"ShipWorksLocal", @"dbo", "NeweggOrder", 4, 0);
			this.AddElementFieldMapping("NeweggOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("NeweggOrderEntity", "InvoiceNumber", "InvoiceNumber", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("NeweggOrderEntity", "RefundAmount", "RefundAmount", true, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 2);
			this.AddElementFieldMapping("NeweggOrderEntity", "IsAutoVoid", "IsAutoVoid", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
		}

		/// <summary>Inits NeweggOrderItemEntity's mappings</summary>
		private void InitNeweggOrderItemEntityMappings()
		{
			this.AddElementMapping("NeweggOrderItemEntity", @"ShipWorksLocal", @"dbo", "NeweggOrderItem", 7, 0);
			this.AddElementFieldMapping("NeweggOrderItemEntity", "OrderItemID", "OrderItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("NeweggOrderItemEntity", "SellerPartNumber", "SellerPartNumber", true, "VarChar", 64, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("NeweggOrderItemEntity", "NeweggItemNumber", "NeweggItemNumber", true, "VarChar", 64, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("NeweggOrderItemEntity", "ManufacturerPartNumber", "ManufacturerPartNumber", true, "VarChar", 64, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("NeweggOrderItemEntity", "ShippingStatusID", "ShippingStatusID", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("NeweggOrderItemEntity", "ShippingStatusDescription", "ShippingStatusDescription", true, "VarChar", 32, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("NeweggOrderItemEntity", "QuantityShipped", "QuantityShipped", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
		}

		/// <summary>Inits NeweggStoreEntity's mappings</summary>
		private void InitNeweggStoreEntityMappings()
		{
			this.AddElementMapping("NeweggStoreEntity", @"ShipWorksLocal", @"dbo", "NeweggStore", 5, 0);
			this.AddElementFieldMapping("NeweggStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("NeweggStoreEntity", "SellerID", "SellerID", false, "VarChar", 10, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("NeweggStoreEntity", "SecretKey", "SecretKey", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("NeweggStoreEntity", "ExcludeFulfilledByNewegg", "ExcludeFulfilledByNewegg", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("NeweggStoreEntity", "Channel", "Channel", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
		}

		/// <summary>Inits NoteEntity's mappings</summary>
		private void InitNoteEntityMappings()
		{
			this.AddElementMapping("NoteEntity", @"ShipWorksLocal", @"dbo", "Note", 8, 0);
			this.AddElementFieldMapping("NoteEntity", "NoteID", "NoteID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("NoteEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("NoteEntity", "EntityID", "ObjectID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("NoteEntity", "UserID", "UserID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("NoteEntity", "Edited", "Edited", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 4);
			this.AddElementFieldMapping("NoteEntity", "Text", "Text", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("NoteEntity", "Source", "Source", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
			this.AddElementFieldMapping("NoteEntity", "Visibility", "Visibility", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 7);
		}

		/// <summary>Inits ObjectLabelEntity's mappings</summary>
		private void InitObjectLabelEntityMappings()
		{
			this.AddElementMapping("ObjectLabelEntity", @"ShipWorksLocal", @"dbo", "ObjectLabel", 6, 0);
			this.AddElementFieldMapping("ObjectLabelEntity", "EntityID", "ObjectID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ObjectLabelEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ObjectLabelEntity", "ObjectType", "ObjectType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("ObjectLabelEntity", "ParentID", "ParentID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("ObjectLabelEntity", "Label", "Label", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("ObjectLabelEntity", "IsDeleted", "IsDeleted", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 5);
		}

		/// <summary>Inits ObjectReferenceEntity's mappings</summary>
		private void InitObjectReferenceEntityMappings()
		{
			this.AddElementMapping("ObjectReferenceEntity", @"ShipWorksLocal", @"dbo", "ObjectReference", 5, 0);
			this.AddElementFieldMapping("ObjectReferenceEntity", "ObjectReferenceID", "ObjectReferenceID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ObjectReferenceEntity", "ConsumerID", "ConsumerID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("ObjectReferenceEntity", "ReferenceKey", "ReferenceKey", false, "VarChar", 250, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("ObjectReferenceEntity", "EntityID", "ObjectID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("ObjectReferenceEntity", "Reason", "Reason", true, "NVarChar", 250, 0, 0, false, "", null, typeof(System.String), 4);
		}

		/// <summary>Inits OdbcStoreEntity's mappings</summary>
		private void InitOdbcStoreEntityMappings()
		{
			this.AddElementMapping("OdbcStoreEntity", @"ShipWorksLocal", @"dbo", "OdbcStore", 12, 0);
			this.AddElementFieldMapping("OdbcStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("OdbcStoreEntity", "ImportConnectionString", "ImportConnectionString", false, "NVarChar", 2048, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("OdbcStoreEntity", "ImportMap", "ImportMap", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("OdbcStoreEntity", "ImportStrategy", "ImportStrategy", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("OdbcStoreEntity", "ImportColumnSourceType", "ImportColumnSourceType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("OdbcStoreEntity", "ImportColumnSource", "ImportColumnSource", false, "NVarChar", 2048, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("OdbcStoreEntity", "UploadStrategy", "UploadStrategy", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
			this.AddElementFieldMapping("OdbcStoreEntity", "UploadMap", "UploadMap", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("OdbcStoreEntity", "UploadColumnSourceType", "UploadColumnSourceType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 8);
			this.AddElementFieldMapping("OdbcStoreEntity", "UploadColumnSource", "UploadColumnSource", false, "NVarChar", 2048, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("OdbcStoreEntity", "UploadConnectionString", "UploadConnectionString", false, "NVarChar", 2048, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("OdbcStoreEntity", "ImportOrderItemStrategy", "ImportOrderItemStrategy", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 11);
		}

		/// <summary>Inits OnTracAccountEntity's mappings</summary>
		private void InitOnTracAccountEntityMappings()
		{
			this.AddElementMapping("OnTracAccountEntity", @"ShipWorksLocal", @"dbo", "OnTracAccount", 16, 0);
			this.AddElementFieldMapping("OnTracAccountEntity", "OnTracAccountID", "OnTracAccountID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("OnTracAccountEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("OnTracAccountEntity", "AccountNumber", "AccountNumber", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("OnTracAccountEntity", "Password", "Password", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("OnTracAccountEntity", "Description", "Description", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("OnTracAccountEntity", "FirstName", "FirstName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("OnTracAccountEntity", "MiddleName", "MiddleName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("OnTracAccountEntity", "LastName", "LastName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("OnTracAccountEntity", "Company", "Company", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("OnTracAccountEntity", "Street1", "Street1", false, "NVarChar", 43, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("OnTracAccountEntity", "City", "City", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("OnTracAccountEntity", "StateProvCode", "StateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("OnTracAccountEntity", "PostalCode", "PostalCode", false, "NVarChar", 10, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("OnTracAccountEntity", "CountryCode", "CountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("OnTracAccountEntity", "Email", "Email", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("OnTracAccountEntity", "Phone", "Phone", false, "NVarChar", 15, 0, 0, false, "", null, typeof(System.String), 15);
		}

		/// <summary>Inits OnTracProfileEntity's mappings</summary>
		private void InitOnTracProfileEntityMappings()
		{
			this.AddElementMapping("OnTracProfileEntity", @"ShipWorksLocal", @"dbo", "OnTracProfile", 17, 0);
			this.AddElementFieldMapping("OnTracProfileEntity", "ShippingProfileID", "ShippingProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("OnTracProfileEntity", "OnTracAccountID", "OnTracAccountID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("OnTracProfileEntity", "ResidentialDetermination", "ResidentialDetermination", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("OnTracProfileEntity", "Service", "Service", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("OnTracProfileEntity", "SaturdayDelivery", "SaturdayDelivery", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 4);
			this.AddElementFieldMapping("OnTracProfileEntity", "SignatureRequired", "SignatureRequired", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 5);
			this.AddElementFieldMapping("OnTracProfileEntity", "PackagingType", "PackagingType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
			this.AddElementFieldMapping("OnTracProfileEntity", "Weight", "Weight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 7);
			this.AddElementFieldMapping("OnTracProfileEntity", "DimsProfileID", "DimsProfileID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 8);
			this.AddElementFieldMapping("OnTracProfileEntity", "DimsLength", "DimsLength", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 9);
			this.AddElementFieldMapping("OnTracProfileEntity", "DimsWidth", "DimsWidth", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 10);
			this.AddElementFieldMapping("OnTracProfileEntity", "DimsHeight", "DimsHeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 11);
			this.AddElementFieldMapping("OnTracProfileEntity", "DimsWeight", "DimsWeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 12);
			this.AddElementFieldMapping("OnTracProfileEntity", "DimsAddWeight", "DimsAddWeight", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 13);
			this.AddElementFieldMapping("OnTracProfileEntity", "Reference1", "Reference1", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("OnTracProfileEntity", "Reference2", "Reference2", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("OnTracProfileEntity", "Instructions", "Instructions", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 16);
		}

		/// <summary>Inits OnTracShipmentEntity's mappings</summary>
		private void InitOnTracShipmentEntityMappings()
		{
			this.AddElementMapping("OnTracShipmentEntity", @"ShipWorksLocal", @"dbo", "OnTracShipment", 22, 0);
			this.AddElementFieldMapping("OnTracShipmentEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("OnTracShipmentEntity", "OnTracAccountID", "OnTracAccountID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("OnTracShipmentEntity", "Service", "Service", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("OnTracShipmentEntity", "IsCod", "IsCod", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("OnTracShipmentEntity", "CodType", "CodType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("OnTracShipmentEntity", "CodAmount", "CodAmount", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 5);
			this.AddElementFieldMapping("OnTracShipmentEntity", "SaturdayDelivery", "SaturdayDelivery", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
			this.AddElementFieldMapping("OnTracShipmentEntity", "SignatureRequired", "SignatureRequired", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 7);
			this.AddElementFieldMapping("OnTracShipmentEntity", "PackagingType", "PackagingType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 8);
			this.AddElementFieldMapping("OnTracShipmentEntity", "Instructions", "Instructions", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("OnTracShipmentEntity", "DimsProfileID", "DimsProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 10);
			this.AddElementFieldMapping("OnTracShipmentEntity", "DimsLength", "DimsLength", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 11);
			this.AddElementFieldMapping("OnTracShipmentEntity", "DimsWidth", "DimsWidth", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 12);
			this.AddElementFieldMapping("OnTracShipmentEntity", "DimsHeight", "DimsHeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 13);
			this.AddElementFieldMapping("OnTracShipmentEntity", "DimsWeight", "DimsWeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 14);
			this.AddElementFieldMapping("OnTracShipmentEntity", "DimsAddWeight", "DimsAddWeight", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 15);
			this.AddElementFieldMapping("OnTracShipmentEntity", "Reference1", "Reference1", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("OnTracShipmentEntity", "Reference2", "Reference2", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 17);
			this.AddElementFieldMapping("OnTracShipmentEntity", "InsuranceValue", "InsuranceValue", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 18);
			this.AddElementFieldMapping("OnTracShipmentEntity", "InsurancePennyOne", "InsurancePennyOne", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 19);
			this.AddElementFieldMapping("OnTracShipmentEntity", "DeclaredValue", "DeclaredValue", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 20);
			this.AddElementFieldMapping("OnTracShipmentEntity", "RequestedLabelFormat", "RequestedLabelFormat", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 21);
		}

		/// <summary>Inits OrderEntity's mappings</summary>
		private void InitOrderEntityMappings()
		{
			this.AddElementMapping("OrderEntity", @"ShipWorksLocal", @"dbo", "Order", 74, 0);
			this.AddElementFieldMapping("OrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("OrderEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("OrderEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("OrderEntity", "CustomerID", "CustomerID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("OrderEntity", "OrderNumber", "OrderNumber", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
			this.AddElementFieldMapping("OrderEntity", "OrderNumberComplete", "OrderNumberComplete", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("OrderEntity", "OrderDate", "OrderDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 6);
			this.AddElementFieldMapping("OrderEntity", "OrderTotal", "OrderTotal", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 7);
			this.AddElementFieldMapping("OrderEntity", "LocalStatus", "LocalStatus", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("OrderEntity", "IsManual", "IsManual", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 9);
			this.AddElementFieldMapping("OrderEntity", "OnlineLastModified", "OnlineLastModified", false, "DateTime2", 0, 7, 0, false, "", null, typeof(System.DateTime), 10);
			this.AddElementFieldMapping("OrderEntity", "OnlineCustomerID", "OnlineCustomerID", true, "Variant", 0, 0, 0, false, "", null, typeof(System.Object), 11);
			this.AddElementFieldMapping("OrderEntity", "OnlineStatus", "OnlineStatus", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("OrderEntity", "OnlineStatusCode", "OnlineStatusCode", true, "Variant", 0, 0, 0, false, "", null, typeof(System.Object), 13);
			this.AddElementFieldMapping("OrderEntity", "RequestedShipping", "RequestedShipping", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("OrderEntity", "BillFirstName", "BillFirstName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("OrderEntity", "BillMiddleName", "BillMiddleName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("OrderEntity", "BillLastName", "BillLastName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 17);
			this.AddElementFieldMapping("OrderEntity", "BillCompany", "BillCompany", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("OrderEntity", "BillStreet1", "BillStreet1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 19);
			this.AddElementFieldMapping("OrderEntity", "BillStreet2", "BillStreet2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 20);
			this.AddElementFieldMapping("OrderEntity", "BillStreet3", "BillStreet3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 21);
			this.AddElementFieldMapping("OrderEntity", "BillCity", "BillCity", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 22);
			this.AddElementFieldMapping("OrderEntity", "BillStateProvCode", "BillStateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 23);
			this.AddElementFieldMapping("OrderEntity", "BillPostalCode", "BillPostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 24);
			this.AddElementFieldMapping("OrderEntity", "BillCountryCode", "BillCountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 25);
			this.AddElementFieldMapping("OrderEntity", "BillPhone", "BillPhone", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 26);
			this.AddElementFieldMapping("OrderEntity", "BillFax", "BillFax", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 27);
			this.AddElementFieldMapping("OrderEntity", "BillEmail", "BillEmail", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 28);
			this.AddElementFieldMapping("OrderEntity", "BillWebsite", "BillWebsite", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 29);
			this.AddElementFieldMapping("OrderEntity", "BillAddressValidationSuggestionCount", "BillAddressValidationSuggestionCount", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 30);
			this.AddElementFieldMapping("OrderEntity", "BillAddressValidationStatus", "BillAddressValidationStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 31);
			this.AddElementFieldMapping("OrderEntity", "BillAddressValidationError", "BillAddressValidationError", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 32);
			this.AddElementFieldMapping("OrderEntity", "BillResidentialStatus", "BillResidentialStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 33);
			this.AddElementFieldMapping("OrderEntity", "BillPOBox", "BillPOBox", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 34);
			this.AddElementFieldMapping("OrderEntity", "BillUSTerritory", "BillUSTerritory", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 35);
			this.AddElementFieldMapping("OrderEntity", "BillMilitaryAddress", "BillMilitaryAddress", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 36);
			this.AddElementFieldMapping("OrderEntity", "ShipFirstName", "ShipFirstName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 37);
			this.AddElementFieldMapping("OrderEntity", "ShipMiddleName", "ShipMiddleName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 38);
			this.AddElementFieldMapping("OrderEntity", "ShipLastName", "ShipLastName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 39);
			this.AddElementFieldMapping("OrderEntity", "ShipCompany", "ShipCompany", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 40);
			this.AddElementFieldMapping("OrderEntity", "ShipStreet1", "ShipStreet1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 41);
			this.AddElementFieldMapping("OrderEntity", "ShipStreet2", "ShipStreet2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 42);
			this.AddElementFieldMapping("OrderEntity", "ShipStreet3", "ShipStreet3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 43);
			this.AddElementFieldMapping("OrderEntity", "ShipCity", "ShipCity", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 44);
			this.AddElementFieldMapping("OrderEntity", "ShipStateProvCode", "ShipStateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 45);
			this.AddElementFieldMapping("OrderEntity", "ShipPostalCode", "ShipPostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 46);
			this.AddElementFieldMapping("OrderEntity", "ShipCountryCode", "ShipCountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 47);
			this.AddElementFieldMapping("OrderEntity", "ShipPhone", "ShipPhone", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 48);
			this.AddElementFieldMapping("OrderEntity", "ShipFax", "ShipFax", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 49);
			this.AddElementFieldMapping("OrderEntity", "ShipEmail", "ShipEmail", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 50);
			this.AddElementFieldMapping("OrderEntity", "ShipWebsite", "ShipWebsite", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 51);
			this.AddElementFieldMapping("OrderEntity", "ShipAddressValidationSuggestionCount", "ShipAddressValidationSuggestionCount", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 52);
			this.AddElementFieldMapping("OrderEntity", "ShipAddressValidationStatus", "ShipAddressValidationStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 53);
			this.AddElementFieldMapping("OrderEntity", "ShipAddressValidationError", "ShipAddressValidationError", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 54);
			this.AddElementFieldMapping("OrderEntity", "ShipResidentialStatus", "ShipResidentialStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 55);
			this.AddElementFieldMapping("OrderEntity", "ShipPOBox", "ShipPOBox", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 56);
			this.AddElementFieldMapping("OrderEntity", "ShipUSTerritory", "ShipUSTerritory", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 57);
			this.AddElementFieldMapping("OrderEntity", "ShipMilitaryAddress", "ShipMilitaryAddress", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 58);
			this.AddElementFieldMapping("OrderEntity", "RollupItemCount", "RollupItemCount", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 59);
			this.AddElementFieldMapping("OrderEntity", "RollupItemName", "RollupItemName", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 60);
			this.AddElementFieldMapping("OrderEntity", "RollupItemCode", "RollupItemCode", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 61);
			this.AddElementFieldMapping("OrderEntity", "RollupItemSKU", "RollupItemSKU", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 62);
			this.AddElementFieldMapping("OrderEntity", "RollupItemLocation", "RollupItemLocation", true, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 63);
			this.AddElementFieldMapping("OrderEntity", "RollupItemQuantity", "RollupItemQuantity", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 64);
			this.AddElementFieldMapping("OrderEntity", "RollupItemTotalWeight", "RollupItemTotalWeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 65);
			this.AddElementFieldMapping("OrderEntity", "RollupNoteCount", "RollupNoteCount", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 66);
			this.AddElementFieldMapping("OrderEntity", "BillNameParseStatus", "BillNameParseStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 67);
			this.AddElementFieldMapping("OrderEntity", "BillUnparsedName", "BillUnparsedName", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 68);
			this.AddElementFieldMapping("OrderEntity", "ShipNameParseStatus", "ShipNameParseStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 69);
			this.AddElementFieldMapping("OrderEntity", "ShipUnparsedName", "ShipUnparsedName", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 70);
			this.AddElementFieldMapping("OrderEntity", "ShipSenseHashKey", "ShipSenseHashKey", false, "NVarChar", 64, 0, 0, false, "", null, typeof(System.String), 71);
			this.AddElementFieldMapping("OrderEntity", "ShipSenseRecognitionStatus", "ShipSenseRecognitionStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 72);
			this.AddElementFieldMapping("OrderEntity", "ShipAddressType", "ShipAddressType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 73);
		}

		/// <summary>Inits OrderChargeEntity's mappings</summary>
		private void InitOrderChargeEntityMappings()
		{
			this.AddElementMapping("OrderChargeEntity", @"ShipWorksLocal", @"dbo", "OrderCharge", 6, 0);
			this.AddElementFieldMapping("OrderChargeEntity", "OrderChargeID", "OrderChargeID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("OrderChargeEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("OrderChargeEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("OrderChargeEntity", "Type", "Type", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("OrderChargeEntity", "Description", "Description", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("OrderChargeEntity", "Amount", "Amount", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 5);
		}

		/// <summary>Inits OrderItemEntity's mappings</summary>
		private void InitOrderItemEntityMappings()
		{
			this.AddElementMapping("OrderItemEntity", @"ShipWorksLocal", @"dbo", "OrderItem", 18, 0);
			this.AddElementFieldMapping("OrderItemEntity", "OrderItemID", "OrderItemID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("OrderItemEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("OrderItemEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("OrderItemEntity", "Name", "Name", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("OrderItemEntity", "Code", "Code", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("OrderItemEntity", "SKU", "SKU", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("OrderItemEntity", "ISBN", "ISBN", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("OrderItemEntity", "UPC", "UPC", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("OrderItemEntity", "Description", "Description", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("OrderItemEntity", "Location", "Location", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("OrderItemEntity", "Image", "Image", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("OrderItemEntity", "Thumbnail", "Thumbnail", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("OrderItemEntity", "UnitPrice", "UnitPrice", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 12);
			this.AddElementFieldMapping("OrderItemEntity", "UnitCost", "UnitCost", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 13);
			this.AddElementFieldMapping("OrderItemEntity", "Weight", "Weight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 14);
			this.AddElementFieldMapping("OrderItemEntity", "Quantity", "Quantity", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 15);
			this.AddElementFieldMapping("OrderItemEntity", "LocalStatus", "LocalStatus", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("OrderItemEntity", "IsManual", "IsManual", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 17);
		}

		/// <summary>Inits OrderItemAttributeEntity's mappings</summary>
		private void InitOrderItemAttributeEntityMappings()
		{
			this.AddElementMapping("OrderItemAttributeEntity", @"ShipWorksLocal", @"dbo", "OrderItemAttribute", 7, 0);
			this.AddElementFieldMapping("OrderItemAttributeEntity", "OrderItemAttributeID", "OrderItemAttributeID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("OrderItemAttributeEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("OrderItemAttributeEntity", "OrderItemID", "OrderItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("OrderItemAttributeEntity", "Name", "Name", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("OrderItemAttributeEntity", "Description", "Description", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("OrderItemAttributeEntity", "UnitPrice", "UnitPrice", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 5);
			this.AddElementFieldMapping("OrderItemAttributeEntity", "IsManual", "IsManual", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
		}

		/// <summary>Inits OrderMotionOrderEntity's mappings</summary>
		private void InitOrderMotionOrderEntityMappings()
		{
			this.AddElementMapping("OrderMotionOrderEntity", @"ShipWorksLocal", @"dbo", "OrderMotionOrder", 4, 0);
			this.AddElementFieldMapping("OrderMotionOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("OrderMotionOrderEntity", "OrderMotionShipmentID", "OrderMotionShipmentID", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 1);
			this.AddElementFieldMapping("OrderMotionOrderEntity", "OrderMotionPromotion", "OrderMotionPromotion", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("OrderMotionOrderEntity", "OrderMotionInvoiceNumber", "OrderMotionInvoiceNumber", false, "NVarChar", 64, 0, 0, false, "", null, typeof(System.String), 3);
		}

		/// <summary>Inits OrderMotionStoreEntity's mappings</summary>
		private void InitOrderMotionStoreEntityMappings()
		{
			this.AddElementMapping("OrderMotionStoreEntity", @"ShipWorksLocal", @"dbo", "OrderMotionStore", 3, 0);
			this.AddElementFieldMapping("OrderMotionStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("OrderMotionStoreEntity", "OrderMotionEmailAccountID", "OrderMotionEmailAccountID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("OrderMotionStoreEntity", "OrderMotionBizID", "OrderMotionBizID", false, "Text", 2147483647, 0, 0, false, "", null, typeof(System.String), 2);
		}

		/// <summary>Inits OrderPaymentDetailEntity's mappings</summary>
		private void InitOrderPaymentDetailEntityMappings()
		{
			this.AddElementMapping("OrderPaymentDetailEntity", @"ShipWorksLocal", @"dbo", "OrderPaymentDetail", 5, 0);
			this.AddElementFieldMapping("OrderPaymentDetailEntity", "OrderPaymentDetailID", "OrderPaymentDetailID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("OrderPaymentDetailEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("OrderPaymentDetailEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("OrderPaymentDetailEntity", "Label", "Label", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("OrderPaymentDetailEntity", "Value", "Value", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 4);
		}

		/// <summary>Inits OtherProfileEntity's mappings</summary>
		private void InitOtherProfileEntityMappings()
		{
			this.AddElementMapping("OtherProfileEntity", @"ShipWorksLocal", @"dbo", "OtherProfile", 3, 0);
			this.AddElementFieldMapping("OtherProfileEntity", "ShippingProfileID", "ShippingProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("OtherProfileEntity", "Carrier", "Carrier", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("OtherProfileEntity", "Service", "Service", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
		}

		/// <summary>Inits OtherShipmentEntity's mappings</summary>
		private void InitOtherShipmentEntityMappings()
		{
			this.AddElementMapping("OtherShipmentEntity", @"ShipWorksLocal", @"dbo", "OtherShipment", 4, 0);
			this.AddElementFieldMapping("OtherShipmentEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("OtherShipmentEntity", "Carrier", "Carrier", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("OtherShipmentEntity", "Service", "Service", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("OtherShipmentEntity", "InsuranceValue", "InsuranceValue", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 3);
		}

		/// <summary>Inits PayPalOrderEntity's mappings</summary>
		private void InitPayPalOrderEntityMappings()
		{
			this.AddElementMapping("PayPalOrderEntity", @"ShipWorksLocal", @"dbo", "PayPalOrder", 5, 0);
			this.AddElementFieldMapping("PayPalOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("PayPalOrderEntity", "TransactionID", "TransactionID", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("PayPalOrderEntity", "AddressStatus", "AddressStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("PayPalOrderEntity", "PayPalFee", "PayPalFee", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 3);
			this.AddElementFieldMapping("PayPalOrderEntity", "PaymentStatus", "PaymentStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
		}

		/// <summary>Inits PayPalStoreEntity's mappings</summary>
		private void InitPayPalStoreEntityMappings()
		{
			this.AddElementMapping("PayPalStoreEntity", @"ShipWorksLocal", @"dbo", "PayPalStore", 8, 0);
			this.AddElementFieldMapping("PayPalStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("PayPalStoreEntity", "ApiCredentialType", "ApiCredentialType", false, "SmallInt", 0, 5, 0, false, "", null, typeof(System.Int16), 1);
			this.AddElementFieldMapping("PayPalStoreEntity", "ApiUserName", "ApiUserName", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("PayPalStoreEntity", "ApiPassword", "ApiPassword", false, "NVarChar", 80, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("PayPalStoreEntity", "ApiSignature", "ApiSignature", false, "NVarChar", 80, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("PayPalStoreEntity", "ApiCertificate", "ApiCertificate", true, "VarBinary", 2048, 0, 0, false, "", null, typeof(System.Byte[]), 5);
			this.AddElementFieldMapping("PayPalStoreEntity", "LastTransactionDate", "LastTransactionDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 6);
			this.AddElementFieldMapping("PayPalStoreEntity", "LastValidTransactionDate", "LastValidTransactionDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 7);
		}

		/// <summary>Inits PermissionEntity's mappings</summary>
		private void InitPermissionEntityMappings()
		{
			this.AddElementMapping("PermissionEntity", @"ShipWorksLocal", @"dbo", "Permission", 4, 0);
			this.AddElementFieldMapping("PermissionEntity", "PermissionID", "PermissionID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("PermissionEntity", "UserID", "UserID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("PermissionEntity", "PermissionType", "PermissionType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("PermissionEntity", "EntityID", "ObjectID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
		}

		/// <summary>Inits PostalProfileEntity's mappings</summary>
		private void InitPostalProfileEntityMappings()
		{
			this.AddElementMapping("PostalProfileEntity", @"ShipWorksLocal", @"dbo", "PostalProfile", 22, 0);
			this.AddElementFieldMapping("PostalProfileEntity", "ShippingProfileID", "ShippingProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("PostalProfileEntity", "Service", "Service", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 1);
			this.AddElementFieldMapping("PostalProfileEntity", "Confirmation", "Confirmation", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("PostalProfileEntity", "Weight", "Weight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 3);
			this.AddElementFieldMapping("PostalProfileEntity", "PackagingType", "PackagingType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("PostalProfileEntity", "DimsProfileID", "DimsProfileID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 5);
			this.AddElementFieldMapping("PostalProfileEntity", "DimsLength", "DimsLength", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 6);
			this.AddElementFieldMapping("PostalProfileEntity", "DimsWidth", "DimsWidth", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 7);
			this.AddElementFieldMapping("PostalProfileEntity", "DimsHeight", "DimsHeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 8);
			this.AddElementFieldMapping("PostalProfileEntity", "DimsWeight", "DimsWeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 9);
			this.AddElementFieldMapping("PostalProfileEntity", "DimsAddWeight", "DimsAddWeight", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 10);
			this.AddElementFieldMapping("PostalProfileEntity", "NonRectangular", "NonRectangular", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 11);
			this.AddElementFieldMapping("PostalProfileEntity", "NonMachinable", "NonMachinable", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 12);
			this.AddElementFieldMapping("PostalProfileEntity", "CustomsContentType", "CustomsContentType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 13);
			this.AddElementFieldMapping("PostalProfileEntity", "CustomsContentDescription", "CustomsContentDescription", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("PostalProfileEntity", "ExpressSignatureWaiver", "ExpressSignatureWaiver", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 15);
			this.AddElementFieldMapping("PostalProfileEntity", "SortType", "SortType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 16);
			this.AddElementFieldMapping("PostalProfileEntity", "EntryFacility", "EntryFacility", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 17);
			this.AddElementFieldMapping("PostalProfileEntity", "Memo1", "Memo1", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("PostalProfileEntity", "Memo2", "Memo2", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 19);
			this.AddElementFieldMapping("PostalProfileEntity", "Memo3", "Memo3", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 20);
			this.AddElementFieldMapping("PostalProfileEntity", "NoPostage", "NoPostage", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 21);
		}

		/// <summary>Inits PostalShipmentEntity's mappings</summary>
		private void InitPostalShipmentEntityMappings()
		{
			this.AddElementMapping("PostalShipmentEntity", @"ShipWorksLocal", @"dbo", "PostalShipment", 22, 0);
			this.AddElementFieldMapping("PostalShipmentEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("PostalShipmentEntity", "Service", "Service", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 1);
			this.AddElementFieldMapping("PostalShipmentEntity", "Confirmation", "Confirmation", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("PostalShipmentEntity", "PackagingType", "PackagingType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("PostalShipmentEntity", "DimsProfileID", "DimsProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
			this.AddElementFieldMapping("PostalShipmentEntity", "DimsLength", "DimsLength", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 5);
			this.AddElementFieldMapping("PostalShipmentEntity", "DimsWidth", "DimsWidth", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 6);
			this.AddElementFieldMapping("PostalShipmentEntity", "DimsHeight", "DimsHeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 7);
			this.AddElementFieldMapping("PostalShipmentEntity", "DimsWeight", "DimsWeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 8);
			this.AddElementFieldMapping("PostalShipmentEntity", "DimsAddWeight", "DimsAddWeight", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 9);
			this.AddElementFieldMapping("PostalShipmentEntity", "NonRectangular", "NonRectangular", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 10);
			this.AddElementFieldMapping("PostalShipmentEntity", "NonMachinable", "NonMachinable", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 11);
			this.AddElementFieldMapping("PostalShipmentEntity", "CustomsContentType", "CustomsContentType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 12);
			this.AddElementFieldMapping("PostalShipmentEntity", "CustomsContentDescription", "CustomsContentDescription", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("PostalShipmentEntity", "InsuranceValue", "InsuranceValue", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 14);
			this.AddElementFieldMapping("PostalShipmentEntity", "ExpressSignatureWaiver", "ExpressSignatureWaiver", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 15);
			this.AddElementFieldMapping("PostalShipmentEntity", "SortType", "SortType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 16);
			this.AddElementFieldMapping("PostalShipmentEntity", "EntryFacility", "EntryFacility", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 17);
			this.AddElementFieldMapping("PostalShipmentEntity", "Memo1", "Memo1", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("PostalShipmentEntity", "Memo2", "Memo2", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 19);
			this.AddElementFieldMapping("PostalShipmentEntity", "Memo3", "Memo3", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 20);
			this.AddElementFieldMapping("PostalShipmentEntity", "NoPostage", "NoPostage", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 21);
		}

		/// <summary>Inits PrintResultEntity's mappings</summary>
		private void InitPrintResultEntityMappings()
		{
			this.AddElementMapping("PrintResultEntity", @"ShipWorksLocal", @"dbo", "PrintResult", 23, 0);
			this.AddElementFieldMapping("PrintResultEntity", "PrintResultID", "PrintResultID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("PrintResultEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("PrintResultEntity", "JobIdentifier", "JobIdentifier", false, "UniqueIdentifier", 0, 0, 0, false, "", null, typeof(System.Guid), 2);
			this.AddElementFieldMapping("PrintResultEntity", "RelatedObjectID", "RelatedObjectID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("PrintResultEntity", "ContextObjectID", "ContextObjectID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
			this.AddElementFieldMapping("PrintResultEntity", "TemplateID", "TemplateID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 5);
			this.AddElementFieldMapping("PrintResultEntity", "TemplateType", "TemplateType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
			this.AddElementFieldMapping("PrintResultEntity", "OutputFormat", "OutputFormat", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 7);
			this.AddElementFieldMapping("PrintResultEntity", "LabelSheetID", "LabelSheetID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 8);
			this.AddElementFieldMapping("PrintResultEntity", "ComputerID", "ComputerID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 9);
			this.AddElementFieldMapping("PrintResultEntity", "ContentResourceID", "ContentResourceID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 10);
			this.AddElementFieldMapping("PrintResultEntity", "PrintDate", "PrintDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 11);
			this.AddElementFieldMapping("PrintResultEntity", "PrinterName", "PrinterName", false, "NVarChar", 350, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("PrintResultEntity", "PaperSource", "PaperSource", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 13);
			this.AddElementFieldMapping("PrintResultEntity", "PaperSourceName", "PaperSourceName", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("PrintResultEntity", "Copies", "Copies", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 15);
			this.AddElementFieldMapping("PrintResultEntity", "Collated", "Collated", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 16);
			this.AddElementFieldMapping("PrintResultEntity", "PageMarginLeft", "PageMarginLeft", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 17);
			this.AddElementFieldMapping("PrintResultEntity", "PageMarginRight", "PageMarginRight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 18);
			this.AddElementFieldMapping("PrintResultEntity", "PageMarginBottom", "PageMarginBottom", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 19);
			this.AddElementFieldMapping("PrintResultEntity", "PageMarginTop", "PageMarginTop", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 20);
			this.AddElementFieldMapping("PrintResultEntity", "PageWidth", "PageWidth", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 21);
			this.AddElementFieldMapping("PrintResultEntity", "PageHeight", "PageHeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 22);
		}

		/// <summary>Inits ProStoresOrderEntity's mappings</summary>
		private void InitProStoresOrderEntityMappings()
		{
			this.AddElementMapping("ProStoresOrderEntity", @"ShipWorksLocal", @"dbo", "ProStoresOrder", 4, 0);
			this.AddElementFieldMapping("ProStoresOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ProStoresOrderEntity", "ConfirmationNumber", "ConfirmationNumber", false, "VarChar", 12, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("ProStoresOrderEntity", "AuthorizedDate", "AuthorizedDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 2);
			this.AddElementFieldMapping("ProStoresOrderEntity", "AuthorizedBy", "AuthorizedBy", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
		}

		/// <summary>Inits ProStoresStoreEntity's mappings</summary>
		private void InitProStoresStoreEntityMappings()
		{
			this.AddElementMapping("ProStoresStoreEntity", @"ShipWorksLocal", @"dbo", "ProStoresStore", 17, 0);
			this.AddElementFieldMapping("ProStoresStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ProStoresStoreEntity", "ShortName", "ShortName", false, "VarChar", 30, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("ProStoresStoreEntity", "Username", "Username", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("ProStoresStoreEntity", "LoginMethod", "LoginMethod", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("ProStoresStoreEntity", "ApiEntryPoint", "ApiEntryPoint", false, "VarChar", 300, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("ProStoresStoreEntity", "ApiToken", "ApiToken", false, "Text", 2147483647, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("ProStoresStoreEntity", "ApiStorefrontUrl", "ApiStorefrontUrl", false, "VarChar", 300, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("ProStoresStoreEntity", "ApiTokenLogonUrl", "ApiTokenLogonUrl", false, "VarChar", 300, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("ProStoresStoreEntity", "ApiXteUrl", "ApiXteUrl", false, "VarChar", 300, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("ProStoresStoreEntity", "ApiRestSecureUrl", "ApiRestSecureUrl", false, "VarChar", 300, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("ProStoresStoreEntity", "ApiRestNonSecureUrl", "ApiRestNonSecureUrl", false, "VarChar", 300, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("ProStoresStoreEntity", "ApiRestScriptSuffix", "ApiRestScriptSuffix", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("ProStoresStoreEntity", "LegacyAdminUrl", "LegacyAdminUrl", false, "VarChar", 300, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("ProStoresStoreEntity", "LegacyXtePath", "LegacyXtePath", false, "VarChar", 75, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("ProStoresStoreEntity", "LegacyPrefix", "LegacyPrefix", false, "VarChar", 30, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("ProStoresStoreEntity", "LegacyPassword", "LegacyPassword", false, "VarChar", 150, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("ProStoresStoreEntity", "LegacyCanUpgrade", "LegacyCanUpgrade", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 16);
		}

		/// <summary>Inits ResourceEntity's mappings</summary>
		private void InitResourceEntityMappings()
		{
			this.AddElementMapping("ResourceEntity", @"ShipWorksLocal", @"dbo", "Resource", 5, 0);
			this.AddElementFieldMapping("ResourceEntity", "ResourceID", "ResourceID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ResourceEntity", "Data", "Data", false, "VarBinary", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ResourceEntity", "Checksum", "Checksum", false, "Binary", 32, 0, 0, false, "", null, typeof(System.Byte[]), 2);
			this.AddElementFieldMapping("ResourceEntity", "Compressed", "Compressed", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("ResourceEntity", "Filename", "Filename", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 4);
		}

		/// <summary>Inits ScanFormBatchEntity's mappings</summary>
		private void InitScanFormBatchEntityMappings()
		{
			this.AddElementMapping("ScanFormBatchEntity", @"ShipWorksLocal", @"dbo", "ScanFormBatch", 4, 0);
			this.AddElementFieldMapping("ScanFormBatchEntity", "ScanFormBatchID", "ScanFormBatchID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ScanFormBatchEntity", "ShipmentType", "ShipmentType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 1);
			this.AddElementFieldMapping("ScanFormBatchEntity", "CreatedDate", "CreatedDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 2);
			this.AddElementFieldMapping("ScanFormBatchEntity", "ShipmentCount", "ShipmentCount", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
		}

		/// <summary>Inits SearchEntity's mappings</summary>
		private void InitSearchEntityMappings()
		{
			this.AddElementMapping("SearchEntity", @"ShipWorksLocal", @"dbo", "Search", 6, 0);
			this.AddElementFieldMapping("SearchEntity", "SearchID", "SearchID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("SearchEntity", "Started", "Started", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 1);
			this.AddElementFieldMapping("SearchEntity", "Pinged", "Pinged", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 2);
			this.AddElementFieldMapping("SearchEntity", "FilterNodeID", "FilterNodeID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("SearchEntity", "UserID", "UserID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
			this.AddElementFieldMapping("SearchEntity", "ComputerID", "ComputerID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 5);
		}

		/// <summary>Inits SearsOrderEntity's mappings</summary>
		private void InitSearsOrderEntityMappings()
		{
			this.AddElementMapping("SearsOrderEntity", @"ShipWorksLocal", @"dbo", "SearsOrder", 6, 0);
			this.AddElementFieldMapping("SearsOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("SearsOrderEntity", "PoNumber", "PoNumber", false, "VarChar", 30, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("SearsOrderEntity", "PoNumberWithDate", "PoNumberWithDate", false, "VarChar", 30, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("SearsOrderEntity", "LocationID", "LocationID", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("SearsOrderEntity", "Commission", "Commission", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 4);
			this.AddElementFieldMapping("SearsOrderEntity", "CustomerPickup", "CustomerPickup", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 5);
		}

		/// <summary>Inits SearsOrderItemEntity's mappings</summary>
		private void InitSearsOrderItemEntityMappings()
		{
			this.AddElementMapping("SearsOrderItemEntity", @"ShipWorksLocal", @"dbo", "SearsOrderItem", 6, 0);
			this.AddElementFieldMapping("SearsOrderItemEntity", "OrderItemID", "OrderItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("SearsOrderItemEntity", "LineNumber", "LineNumber", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 1);
			this.AddElementFieldMapping("SearsOrderItemEntity", "ItemID", "ItemID", false, "VarChar", 300, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("SearsOrderItemEntity", "Commission", "Commission", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 3);
			this.AddElementFieldMapping("SearsOrderItemEntity", "Shipping", "Shipping", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 4);
			this.AddElementFieldMapping("SearsOrderItemEntity", "OnlineStatus", "OnlineStatus", false, "VarChar", 20, 0, 0, false, "", null, typeof(System.String), 5);
		}

		/// <summary>Inits SearsStoreEntity's mappings</summary>
		private void InitSearsStoreEntityMappings()
		{
			this.AddElementMapping("SearsStoreEntity", @"ShipWorksLocal", @"dbo", "SearsStore", 5, 0);
			this.AddElementFieldMapping("SearsStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("SearsStoreEntity", "SearsEmail", "SearsEmail", false, "NVarChar", 75, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("SearsStoreEntity", "Password", "Password", false, "NVarChar", 75, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("SearsStoreEntity", "SecretKey", "SecretKey", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("SearsStoreEntity", "SellerID", "SellerID", false, "NVarChar", 15, 0, 0, false, "", null, typeof(System.String), 4);
		}

		/// <summary>Inits ServerMessageEntity's mappings</summary>
		private void InitServerMessageEntityMappings()
		{
			this.AddElementMapping("ServerMessageEntity", @"ShipWorksLocal", @"dbo", "ServerMessage", 16, 0);
			this.AddElementFieldMapping("ServerMessageEntity", "ServerMessageID", "ServerMessageID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ServerMessageEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ServerMessageEntity", "Number", "Number", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("ServerMessageEntity", "Published", "Published", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 3);
			this.AddElementFieldMapping("ServerMessageEntity", "Active", "Active", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 4);
			this.AddElementFieldMapping("ServerMessageEntity", "Dismissable", "Dismissable", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 5);
			this.AddElementFieldMapping("ServerMessageEntity", "Expires", "Expires", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 6);
			this.AddElementFieldMapping("ServerMessageEntity", "ResponseTo", "ResponseTo", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 7);
			this.AddElementFieldMapping("ServerMessageEntity", "ResponseAction", "ResponseAction", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 8);
			this.AddElementFieldMapping("ServerMessageEntity", "EditTo", "EditTo", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 9);
			this.AddElementFieldMapping("ServerMessageEntity", "Image", "Image", false, "NVarChar", 350, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("ServerMessageEntity", "PrimaryText", "PrimaryText", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("ServerMessageEntity", "SecondaryText", "SecondaryText", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("ServerMessageEntity", "Actions", "Actions", false, "NText", 1073741823, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("ServerMessageEntity", "Stores", "Stores", false, "NText", 1073741823, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("ServerMessageEntity", "Shippers", "Shippers", false, "NText", 1073741823, 0, 0, false, "", null, typeof(System.String), 15);
		}

		/// <summary>Inits ServerMessageSignoffEntity's mappings</summary>
		private void InitServerMessageSignoffEntityMappings()
		{
			this.AddElementMapping("ServerMessageSignoffEntity", @"ShipWorksLocal", @"dbo", "ServerMessageSignoff", 6, 0);
			this.AddElementFieldMapping("ServerMessageSignoffEntity", "ServerMessageSignoffID", "ServerMessageSignoffID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ServerMessageSignoffEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ServerMessageSignoffEntity", "ServerMessageID", "ServerMessageID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("ServerMessageSignoffEntity", "UserID", "UserID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("ServerMessageSignoffEntity", "ComputerID", "ComputerID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
			this.AddElementFieldMapping("ServerMessageSignoffEntity", "Dismissed", "Dismissed", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 5);
		}

		/// <summary>Inits ServiceStatusEntity's mappings</summary>
		private void InitServiceStatusEntityMappings()
		{
			this.AddElementMapping("ServiceStatusEntity", @"ShipWorksLocal", @"dbo", "ServiceStatus", 9, 0);
			this.AddElementFieldMapping("ServiceStatusEntity", "ServiceStatusID", "ServiceStatusID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ServiceStatusEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ServiceStatusEntity", "ComputerID", "ComputerID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("ServiceStatusEntity", "ServiceType", "ServiceType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("ServiceStatusEntity", "LastStartDateTime", "LastStartDateTime", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 4);
			this.AddElementFieldMapping("ServiceStatusEntity", "LastStopDateTime", "LastStopDateTime", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 5);
			this.AddElementFieldMapping("ServiceStatusEntity", "LastCheckInDateTime", "LastCheckInDateTime", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 6);
			this.AddElementFieldMapping("ServiceStatusEntity", "ServiceFullName", "ServiceFullName", false, "NVarChar", 256, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("ServiceStatusEntity", "ServiceDisplayName", "ServiceDisplayName", false, "NVarChar", 256, 0, 0, false, "", null, typeof(System.String), 8);
		}

		/// <summary>Inits ShipmentEntity's mappings</summary>
		private void InitShipmentEntityMappings()
		{
			this.AddElementMapping("ShipmentEntity", @"ShipWorksLocal", @"dbo", "Shipment", 73, 0);
			this.AddElementFieldMapping("ShipmentEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ShipmentEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ShipmentEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("ShipmentEntity", "ShipmentType", "ShipmentType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("ShipmentEntity", "ContentWeight", "ContentWeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 4);
			this.AddElementFieldMapping("ShipmentEntity", "TotalWeight", "TotalWeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 5);
			this.AddElementFieldMapping("ShipmentEntity", "Processed", "Processed", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
			this.AddElementFieldMapping("ShipmentEntity", "ProcessedDate", "ProcessedDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 7);
			this.AddElementFieldMapping("ShipmentEntity", "ProcessedUserID", "ProcessedUserID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 8);
			this.AddElementFieldMapping("ShipmentEntity", "ProcessedComputerID", "ProcessedComputerID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 9);
			this.AddElementFieldMapping("ShipmentEntity", "ShipDate", "ShipDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 10);
			this.AddElementFieldMapping("ShipmentEntity", "ShipmentCost", "ShipmentCost", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 11);
			this.AddElementFieldMapping("ShipmentEntity", "Voided", "Voided", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 12);
			this.AddElementFieldMapping("ShipmentEntity", "VoidedDate", "VoidedDate", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 13);
			this.AddElementFieldMapping("ShipmentEntity", "VoidedUserID", "VoidedUserID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 14);
			this.AddElementFieldMapping("ShipmentEntity", "VoidedComputerID", "VoidedComputerID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 15);
			this.AddElementFieldMapping("ShipmentEntity", "TrackingNumber", "TrackingNumber", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("ShipmentEntity", "CustomsGenerated", "CustomsGenerated", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 17);
			this.AddElementFieldMapping("ShipmentEntity", "CustomsValue", "CustomsValue", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 18);
			this.AddElementFieldMapping("ShipmentEntity", "RequestedLabelFormat", "RequestedLabelFormat", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 19);
			this.AddElementFieldMapping("ShipmentEntity", "ActualLabelFormat", "ActualLabelFormat", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 20);
			this.AddElementFieldMapping("ShipmentEntity", "ShipFirstName", "ShipFirstName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 21);
			this.AddElementFieldMapping("ShipmentEntity", "ShipMiddleName", "ShipMiddleName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 22);
			this.AddElementFieldMapping("ShipmentEntity", "ShipLastName", "ShipLastName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 23);
			this.AddElementFieldMapping("ShipmentEntity", "ShipCompany", "ShipCompany", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 24);
			this.AddElementFieldMapping("ShipmentEntity", "ShipStreet1", "ShipStreet1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 25);
			this.AddElementFieldMapping("ShipmentEntity", "ShipStreet2", "ShipStreet2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 26);
			this.AddElementFieldMapping("ShipmentEntity", "ShipStreet3", "ShipStreet3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 27);
			this.AddElementFieldMapping("ShipmentEntity", "ShipCity", "ShipCity", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 28);
			this.AddElementFieldMapping("ShipmentEntity", "ShipStateProvCode", "ShipStateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 29);
			this.AddElementFieldMapping("ShipmentEntity", "ShipPostalCode", "ShipPostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 30);
			this.AddElementFieldMapping("ShipmentEntity", "ShipCountryCode", "ShipCountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 31);
			this.AddElementFieldMapping("ShipmentEntity", "ShipPhone", "ShipPhone", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 32);
			this.AddElementFieldMapping("ShipmentEntity", "ShipEmail", "ShipEmail", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 33);
			this.AddElementFieldMapping("ShipmentEntity", "ShipAddressValidationSuggestionCount", "ShipAddressValidationSuggestionCount", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 34);
			this.AddElementFieldMapping("ShipmentEntity", "ShipAddressValidationStatus", "ShipAddressValidationStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 35);
			this.AddElementFieldMapping("ShipmentEntity", "ShipAddressValidationError", "ShipAddressValidationError", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 36);
			this.AddElementFieldMapping("ShipmentEntity", "ShipResidentialStatus", "ShipResidentialStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 37);
			this.AddElementFieldMapping("ShipmentEntity", "ShipPOBox", "ShipPOBox", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 38);
			this.AddElementFieldMapping("ShipmentEntity", "ShipUSTerritory", "ShipUSTerritory", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 39);
			this.AddElementFieldMapping("ShipmentEntity", "ShipMilitaryAddress", "ShipMilitaryAddress", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 40);
			this.AddElementFieldMapping("ShipmentEntity", "ResidentialDetermination", "ResidentialDetermination", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 41);
			this.AddElementFieldMapping("ShipmentEntity", "ResidentialResult", "ResidentialResult", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 42);
			this.AddElementFieldMapping("ShipmentEntity", "OriginOriginID", "OriginOriginID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 43);
			this.AddElementFieldMapping("ShipmentEntity", "OriginFirstName", "OriginFirstName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 44);
			this.AddElementFieldMapping("ShipmentEntity", "OriginMiddleName", "OriginMiddleName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 45);
			this.AddElementFieldMapping("ShipmentEntity", "OriginLastName", "OriginLastName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 46);
			this.AddElementFieldMapping("ShipmentEntity", "OriginCompany", "OriginCompany", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 47);
			this.AddElementFieldMapping("ShipmentEntity", "OriginStreet1", "OriginStreet1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 48);
			this.AddElementFieldMapping("ShipmentEntity", "OriginStreet2", "OriginStreet2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 49);
			this.AddElementFieldMapping("ShipmentEntity", "OriginStreet3", "OriginStreet3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 50);
			this.AddElementFieldMapping("ShipmentEntity", "OriginCity", "OriginCity", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 51);
			this.AddElementFieldMapping("ShipmentEntity", "OriginStateProvCode", "OriginStateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 52);
			this.AddElementFieldMapping("ShipmentEntity", "OriginPostalCode", "OriginPostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 53);
			this.AddElementFieldMapping("ShipmentEntity", "OriginCountryCode", "OriginCountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 54);
			this.AddElementFieldMapping("ShipmentEntity", "OriginPhone", "OriginPhone", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 55);
			this.AddElementFieldMapping("ShipmentEntity", "OriginFax", "OriginFax", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 56);
			this.AddElementFieldMapping("ShipmentEntity", "OriginEmail", "OriginEmail", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 57);
			this.AddElementFieldMapping("ShipmentEntity", "OriginWebsite", "OriginWebsite", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 58);
			this.AddElementFieldMapping("ShipmentEntity", "ReturnShipment", "ReturnShipment", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 59);
			this.AddElementFieldMapping("ShipmentEntity", "Insurance", "Insurance", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 60);
			this.AddElementFieldMapping("ShipmentEntity", "InsuranceProvider", "InsuranceProvider", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 61);
			this.AddElementFieldMapping("ShipmentEntity", "ShipNameParseStatus", "ShipNameParseStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 62);
			this.AddElementFieldMapping("ShipmentEntity", "ShipUnparsedName", "ShipUnparsedName", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 63);
			this.AddElementFieldMapping("ShipmentEntity", "OriginNameParseStatus", "OriginNameParseStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 64);
			this.AddElementFieldMapping("ShipmentEntity", "OriginUnparsedName", "OriginUnparsedName", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 65);
			this.AddElementFieldMapping("ShipmentEntity", "BestRateEvents", "BestRateEvents", false, "TinyInt", 0, 3, 0, false, "", null, typeof(System.Byte), 66);
			this.AddElementFieldMapping("ShipmentEntity", "ShipSenseStatus", "ShipSenseStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 67);
			this.AddElementFieldMapping("ShipmentEntity", "ShipSenseChangeSets", "ShipSenseChangeSets", false, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 68);
			this.AddElementFieldMapping("ShipmentEntity", "ShipSenseEntry", "ShipSenseEntry", false, "VarBinary", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 69);
			this.AddElementFieldMapping("ShipmentEntity", "OnlineShipmentID", "OnlineShipmentID", false, "VarChar", 128, 0, 0, false, "", null, typeof(System.String), 70);
			this.AddElementFieldMapping("ShipmentEntity", "BilledType", "BilledType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 71);
			this.AddElementFieldMapping("ShipmentEntity", "BilledWeight", "BilledWeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 72);
		}

		/// <summary>Inits ShipmentCustomsItemEntity's mappings</summary>
		private void InitShipmentCustomsItemEntityMappings()
		{
			this.AddElementMapping("ShipmentCustomsItemEntity", @"ShipWorksLocal", @"dbo", "ShipmentCustomsItem", 11, 0);
			this.AddElementFieldMapping("ShipmentCustomsItemEntity", "ShipmentCustomsItemID", "ShipmentCustomsItemID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ShipmentCustomsItemEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ShipmentCustomsItemEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("ShipmentCustomsItemEntity", "Description", "Description", false, "NVarChar", 150, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("ShipmentCustomsItemEntity", "Quantity", "Quantity", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 4);
			this.AddElementFieldMapping("ShipmentCustomsItemEntity", "Weight", "Weight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 5);
			this.AddElementFieldMapping("ShipmentCustomsItemEntity", "UnitValue", "UnitValue", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 6);
			this.AddElementFieldMapping("ShipmentCustomsItemEntity", "CountryOfOrigin", "CountryOfOrigin", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("ShipmentCustomsItemEntity", "HarmonizedCode", "HarmonizedCode", false, "VarChar", 14, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("ShipmentCustomsItemEntity", "NumberOfPieces", "NumberOfPieces", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 9);
			this.AddElementFieldMapping("ShipmentCustomsItemEntity", "UnitPriceAmount", "UnitPriceAmount", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 10);
		}

		/// <summary>Inits ShippingDefaultsRuleEntity's mappings</summary>
		private void InitShippingDefaultsRuleEntityMappings()
		{
			this.AddElementMapping("ShippingDefaultsRuleEntity", @"ShipWorksLocal", @"dbo", "ShippingDefaultsRule", 6, 0);
			this.AddElementFieldMapping("ShippingDefaultsRuleEntity", "ShippingDefaultsRuleID", "ShippingDefaultsRuleID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ShippingDefaultsRuleEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ShippingDefaultsRuleEntity", "ShipmentType", "ShipmentType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("ShippingDefaultsRuleEntity", "FilterNodeID", "FilterNodeID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
			this.AddElementFieldMapping("ShippingDefaultsRuleEntity", "ShippingProfileID", "ShippingProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
			this.AddElementFieldMapping("ShippingDefaultsRuleEntity", "Position", "Position", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
		}

		/// <summary>Inits ShippingOriginEntity's mappings</summary>
		private void InitShippingOriginEntityMappings()
		{
			this.AddElementMapping("ShippingOriginEntity", @"ShipWorksLocal", @"dbo", "ShippingOrigin", 18, 0);
			this.AddElementFieldMapping("ShippingOriginEntity", "ShippingOriginID", "ShippingOriginID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ShippingOriginEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ShippingOriginEntity", "Description", "Description", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("ShippingOriginEntity", "FirstName", "FirstName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("ShippingOriginEntity", "MiddleName", "MiddleName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("ShippingOriginEntity", "LastName", "LastName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("ShippingOriginEntity", "Company", "Company", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("ShippingOriginEntity", "Street1", "Street1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("ShippingOriginEntity", "Street2", "Street2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("ShippingOriginEntity", "Street3", "Street3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("ShippingOriginEntity", "City", "City", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("ShippingOriginEntity", "StateProvCode", "StateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("ShippingOriginEntity", "PostalCode", "PostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("ShippingOriginEntity", "CountryCode", "CountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("ShippingOriginEntity", "Phone", "Phone", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("ShippingOriginEntity", "Fax", "Fax", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("ShippingOriginEntity", "Email", "Email", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("ShippingOriginEntity", "Website", "Website", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 17);
		}

		/// <summary>Inits ShippingPrintOutputEntity's mappings</summary>
		private void InitShippingPrintOutputEntityMappings()
		{
			this.AddElementMapping("ShippingPrintOutputEntity", @"ShipWorksLocal", @"dbo", "ShippingPrintOutput", 4, 0);
			this.AddElementFieldMapping("ShippingPrintOutputEntity", "ShippingPrintOutputID", "ShippingPrintOutputID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ShippingPrintOutputEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ShippingPrintOutputEntity", "ShipmentType", "ShipmentType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("ShippingPrintOutputEntity", "Name", "Name", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
		}

		/// <summary>Inits ShippingPrintOutputRuleEntity's mappings</summary>
		private void InitShippingPrintOutputRuleEntityMappings()
		{
			this.AddElementMapping("ShippingPrintOutputRuleEntity", @"ShipWorksLocal", @"dbo", "ShippingPrintOutputRule", 4, 0);
			this.AddElementFieldMapping("ShippingPrintOutputRuleEntity", "ShippingPrintOutputRuleID", "ShippingPrintOutputRuleID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ShippingPrintOutputRuleEntity", "ShippingPrintOutputID", "ShippingPrintOutputID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("ShippingPrintOutputRuleEntity", "FilterNodeID", "FilterNodeID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("ShippingPrintOutputRuleEntity", "TemplateID", "TemplateID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
		}

		/// <summary>Inits ShippingProfileEntity's mappings</summary>
		private void InitShippingProfileEntityMappings()
		{
			this.AddElementMapping("ShippingProfileEntity", @"ShipWorksLocal", @"dbo", "ShippingProfile", 11, 0);
			this.AddElementFieldMapping("ShippingProfileEntity", "ShippingProfileID", "ShippingProfileID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ShippingProfileEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ShippingProfileEntity", "Name", "Name", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("ShippingProfileEntity", "ShipmentType", "ShipmentType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("ShippingProfileEntity", "ShipmentTypePrimary", "ShipmentTypePrimary", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 4);
			this.AddElementFieldMapping("ShippingProfileEntity", "OriginID", "OriginID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 5);
			this.AddElementFieldMapping("ShippingProfileEntity", "Insurance", "Insurance", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
			this.AddElementFieldMapping("ShippingProfileEntity", "InsuranceInitialValueSource", "InsuranceInitialValueSource", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 7);
			this.AddElementFieldMapping("ShippingProfileEntity", "InsuranceInitialValueAmount", "InsuranceInitialValueAmount", true, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 8);
			this.AddElementFieldMapping("ShippingProfileEntity", "ReturnShipment", "ReturnShipment", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 9);
			this.AddElementFieldMapping("ShippingProfileEntity", "RequestedLabelFormat", "RequestedLabelFormat", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 10);
		}

		/// <summary>Inits ShippingProviderRuleEntity's mappings</summary>
		private void InitShippingProviderRuleEntityMappings()
		{
			this.AddElementMapping("ShippingProviderRuleEntity", @"ShipWorksLocal", @"dbo", "ShippingProviderRule", 4, 0);
			this.AddElementFieldMapping("ShippingProviderRuleEntity", "ShippingProviderRuleID", "ShippingProviderRuleID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ShippingProviderRuleEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("ShippingProviderRuleEntity", "FilterNodeID", "FilterNodeID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("ShippingProviderRuleEntity", "ShipmentType", "ShipmentType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
		}

		/// <summary>Inits ShippingSettingsEntity's mappings</summary>
		private void InitShippingSettingsEntityMappings()
		{
			this.AddElementMapping("ShippingSettingsEntity", @"ShipWorksLocal", @"dbo", "ShippingSettings", 52, 0);
			this.AddElementFieldMapping("ShippingSettingsEntity", "ShippingSettingsID", "ShippingSettingsID", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 0);
			this.AddElementFieldMapping("ShippingSettingsEntity", "InternalActivated", "Activated", false, "VarChar", 45, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("ShippingSettingsEntity", "InternalConfigured", "Configured", false, "VarChar", 45, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("ShippingSettingsEntity", "InternalExcluded", "Excluded", false, "VarChar", 45, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("ShippingSettingsEntity", "DefaultType", "DefaultType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("ShippingSettingsEntity", "BlankPhoneOption", "BlankPhoneOption", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("ShippingSettingsEntity", "BlankPhoneNumber", "BlankPhoneNumber", false, "NVarChar", 16, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("ShippingSettingsEntity", "InsurancePolicy", "InsurancePolicy", false, "NVarChar", 40, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("ShippingSettingsEntity", "InsuranceLastAgreed", "InsuranceLastAgreed", true, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 8);
			this.AddElementFieldMapping("ShippingSettingsEntity", "FedExUsername", "FedExUsername", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("ShippingSettingsEntity", "FedExPassword", "FedExPassword", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("ShippingSettingsEntity", "FedExMaskAccount", "FedExMaskAccount", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 11);
			this.AddElementFieldMapping("ShippingSettingsEntity", "FedExThermalDocTab", "FedExThermalDocTab", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 12);
			this.AddElementFieldMapping("ShippingSettingsEntity", "FedExThermalDocTabType", "FedExThermalDocTabType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 13);
			this.AddElementFieldMapping("ShippingSettingsEntity", "FedExInsuranceProvider", "FedExInsuranceProvider", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 14);
			this.AddElementFieldMapping("ShippingSettingsEntity", "FedExInsurancePennyOne", "FedExInsurancePennyOne", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 15);
			this.AddElementFieldMapping("ShippingSettingsEntity", "UpsAccessKey", "UpsAccessKey", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("ShippingSettingsEntity", "UpsInsuranceProvider", "UpsInsuranceProvider", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 17);
			this.AddElementFieldMapping("ShippingSettingsEntity", "UpsInsurancePennyOne", "UpsInsurancePennyOne", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 18);
			this.AddElementFieldMapping("ShippingSettingsEntity", "EndiciaCustomsCertify", "EndiciaCustomsCertify", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 19);
			this.AddElementFieldMapping("ShippingSettingsEntity", "EndiciaCustomsSigner", "EndiciaCustomsSigner", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 20);
			this.AddElementFieldMapping("ShippingSettingsEntity", "EndiciaThermalDocTab", "EndiciaThermalDocTab", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 21);
			this.AddElementFieldMapping("ShippingSettingsEntity", "EndiciaThermalDocTabType", "EndiciaThermalDocTabType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 22);
			this.AddElementFieldMapping("ShippingSettingsEntity", "EndiciaAutomaticExpress1", "EndiciaAutomaticExpress1", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 23);
			this.AddElementFieldMapping("ShippingSettingsEntity", "EndiciaAutomaticExpress1Account", "EndiciaAutomaticExpress1Account", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 24);
			this.AddElementFieldMapping("ShippingSettingsEntity", "EndiciaInsuranceProvider", "EndiciaInsuranceProvider", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 25);
			this.AddElementFieldMapping("ShippingSettingsEntity", "WorldShipLaunch", "WorldShipLaunch", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 26);
			this.AddElementFieldMapping("ShippingSettingsEntity", "UspsAutomaticExpress1", "UspsAutomaticExpress1", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 27);
			this.AddElementFieldMapping("ShippingSettingsEntity", "UspsAutomaticExpress1Account", "UspsAutomaticExpress1Account", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 28);
			this.AddElementFieldMapping("ShippingSettingsEntity", "UspsInsuranceProvider", "UspsInsuranceProvider", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 29);
			this.AddElementFieldMapping("ShippingSettingsEntity", "Express1EndiciaCustomsCertify", "Express1EndiciaCustomsCertify", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 30);
			this.AddElementFieldMapping("ShippingSettingsEntity", "Express1EndiciaCustomsSigner", "Express1EndiciaCustomsSigner", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 31);
			this.AddElementFieldMapping("ShippingSettingsEntity", "Express1EndiciaThermalDocTab", "Express1EndiciaThermalDocTab", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 32);
			this.AddElementFieldMapping("ShippingSettingsEntity", "Express1EndiciaThermalDocTabType", "Express1EndiciaThermalDocTabType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 33);
			this.AddElementFieldMapping("ShippingSettingsEntity", "Express1EndiciaSingleSource", "Express1EndiciaSingleSource", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 34);
			this.AddElementFieldMapping("ShippingSettingsEntity", "OnTracInsuranceProvider", "OnTracInsuranceProvider", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 35);
			this.AddElementFieldMapping("ShippingSettingsEntity", "OnTracInsurancePennyOne", "OnTracInsurancePennyOne", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 36);
			this.AddElementFieldMapping("ShippingSettingsEntity", "IParcelInsuranceProvider", "iParcelInsuranceProvider", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 37);
			this.AddElementFieldMapping("ShippingSettingsEntity", "IParcelInsurancePennyOne", "iParcelInsurancePennyOne", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 38);
			this.AddElementFieldMapping("ShippingSettingsEntity", "Express1UspsSingleSource", "Express1UspsSingleSource", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 39);
			this.AddElementFieldMapping("ShippingSettingsEntity", "UpsMailInnovationsEnabled", "UpsMailInnovationsEnabled", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 40);
			this.AddElementFieldMapping("ShippingSettingsEntity", "WorldShipMailInnovationsEnabled", "WorldShipMailInnovationsEnabled", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 41);
			this.AddElementFieldMapping("ShippingSettingsEntity", "InternalBestRateExcludedShipmentTypes", "BestRateExcludedShipmentTypes", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 42);
			this.AddElementFieldMapping("ShippingSettingsEntity", "ShipSenseEnabled", "ShipSenseEnabled", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 43);
			this.AddElementFieldMapping("ShippingSettingsEntity", "ShipSenseUniquenessXml", "ShipSenseUniquenessXml", false, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 44);
			this.AddElementFieldMapping("ShippingSettingsEntity", "ShipSenseProcessedShipmentID", "ShipSenseProcessedShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 45);
			this.AddElementFieldMapping("ShippingSettingsEntity", "ShipSenseEndShipmentID", "ShipSenseEndShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 46);
			this.AddElementFieldMapping("ShippingSettingsEntity", "AutoCreateShipments", "AutoCreateShipments", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 47);
			this.AddElementFieldMapping("ShippingSettingsEntity", "FedExFimsEnabled", "FedExFimsEnabled", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 48);
			this.AddElementFieldMapping("ShippingSettingsEntity", "FedExFimsUsername", "FedExFimsUsername", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 49);
			this.AddElementFieldMapping("ShippingSettingsEntity", "FedExFimsPassword", "FedExFimsPassword", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 50);
			this.AddElementFieldMapping("ShippingSettingsEntity", "ShipmentEditLimit", "ShipmentEditLimit", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 51);
		}

		/// <summary>Inits ShipSenseKnowledgebaseEntity's mappings</summary>
		private void InitShipSenseKnowledgebaseEntityMappings()
		{
			this.AddElementMapping("ShipSenseKnowledgebaseEntity", @"ShipWorksLocal", @"dbo", "ShipSenseKnowledgeBase", 2, 0);
			this.AddElementFieldMapping("ShipSenseKnowledgebaseEntity", "Hash", "Hash", false, "NVarChar", 64, 0, 0, false, "", null, typeof(System.String), 0);
			this.AddElementFieldMapping("ShipSenseKnowledgebaseEntity", "Entry", "Entry", false, "VarBinary", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
		}

		/// <summary>Inits ShopifyOrderEntity's mappings</summary>
		private void InitShopifyOrderEntityMappings()
		{
			this.AddElementMapping("ShopifyOrderEntity", @"ShipWorksLocal", @"dbo", "ShopifyOrder", 4, 0);
			this.AddElementFieldMapping("ShopifyOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ShopifyOrderEntity", "ShopifyOrderID", "ShopifyOrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("ShopifyOrderEntity", "FulfillmentStatusCode", "FulfillmentStatusCode", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("ShopifyOrderEntity", "PaymentStatusCode", "PaymentStatusCode", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
		}

		/// <summary>Inits ShopifyOrderItemEntity's mappings</summary>
		private void InitShopifyOrderItemEntityMappings()
		{
			this.AddElementMapping("ShopifyOrderItemEntity", @"ShipWorksLocal", @"dbo", "ShopifyOrderItem", 3, 0);
			this.AddElementFieldMapping("ShopifyOrderItemEntity", "OrderItemID", "OrderItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ShopifyOrderItemEntity", "ShopifyOrderItemID", "ShopifyOrderItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("ShopifyOrderItemEntity", "ShopifyProductID", "ShopifyProductID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
		}

		/// <summary>Inits ShopifyStoreEntity's mappings</summary>
		private void InitShopifyStoreEntityMappings()
		{
			this.AddElementMapping("ShopifyStoreEntity", @"ShipWorksLocal", @"dbo", "ShopifyStore", 5, 0);
			this.AddElementFieldMapping("ShopifyStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ShopifyStoreEntity", "ShopifyShopUrlName", "ShopifyShopUrlName", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("ShopifyStoreEntity", "ShopifyShopDisplayName", "ShopifyShopDisplayName", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("ShopifyStoreEntity", "ShopifyAccessToken", "ShopifyAccessToken", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("ShopifyStoreEntity", "ShopifyRequestedShippingOption", "ShopifyRequestedShippingOption", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
		}

		/// <summary>Inits ShopSiteStoreEntity's mappings</summary>
		private void InitShopSiteStoreEntityMappings()
		{
			this.AddElementMapping("ShopSiteStoreEntity", @"ShipWorksLocal", @"dbo", "ShopSiteStore", 7, 0);
			this.AddElementFieldMapping("ShopSiteStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ShopSiteStoreEntity", "Username", "Username", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("ShopSiteStoreEntity", "Password", "Password", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("ShopSiteStoreEntity", "CgiUrl", "CgiUrl", false, "NVarChar", 350, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("ShopSiteStoreEntity", "RequireSSL", "RequireSSL", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 4);
			this.AddElementFieldMapping("ShopSiteStoreEntity", "DownloadPageSize", "DownloadPageSize", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("ShopSiteStoreEntity", "RequestTimeout", "RequestTimeout", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
		}

		/// <summary>Inits SparkPayStoreEntity's mappings</summary>
		private void InitSparkPayStoreEntityMappings()
		{
			this.AddElementMapping("SparkPayStoreEntity", @"ShipWorksLocal", @"dbo", "SparkPayStore", 4, 0);
			this.AddElementFieldMapping("SparkPayStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("SparkPayStoreEntity", "Token", "Token", false, "NVarChar", 70, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("SparkPayStoreEntity", "StoreUrl", "StoreUrl", false, "NVarChar", 350, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("SparkPayStoreEntity", "StatusCodes", "StatusCodes", true, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 3);
		}

		/// <summary>Inits StatusPresetEntity's mappings</summary>
		private void InitStatusPresetEntityMappings()
		{
			this.AddElementMapping("StatusPresetEntity", @"ShipWorksLocal", @"dbo", "StatusPreset", 6, 0);
			this.AddElementFieldMapping("StatusPresetEntity", "StatusPresetID", "StatusPresetID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("StatusPresetEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("StatusPresetEntity", "StoreID", "StoreID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("StatusPresetEntity", "StatusTarget", "StatusTarget", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("StatusPresetEntity", "StatusText", "StatusText", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("StatusPresetEntity", "IsDefault", "IsDefault", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 5);
		}

		/// <summary>Inits StoreEntity's mappings</summary>
		private void InitStoreEntityMappings()
		{
			this.AddElementMapping("StoreEntity", @"ShipWorksLocal", @"dbo", "Store", 30, 0);
			this.AddElementFieldMapping("StoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("StoreEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("StoreEntity", "License", "License", false, "NVarChar", 150, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("StoreEntity", "Edition", "Edition", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("StoreEntity", "TypeCode", "TypeCode", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("StoreEntity", "Enabled", "Enabled", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 5);
			this.AddElementFieldMapping("StoreEntity", "SetupComplete", "SetupComplete", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
			this.AddElementFieldMapping("StoreEntity", "StoreName", "StoreName", false, "NVarChar", 75, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("StoreEntity", "Company", "Company", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("StoreEntity", "Street1", "Street1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("StoreEntity", "Street2", "Street2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("StoreEntity", "Street3", "Street3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("StoreEntity", "City", "City", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("StoreEntity", "StateProvCode", "StateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("StoreEntity", "PostalCode", "PostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("StoreEntity", "CountryCode", "CountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("StoreEntity", "Phone", "Phone", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("StoreEntity", "Fax", "Fax", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 17);
			this.AddElementFieldMapping("StoreEntity", "Email", "Email", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("StoreEntity", "Website", "Website", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 19);
			this.AddElementFieldMapping("StoreEntity", "AutoDownload", "AutoDownload", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 20);
			this.AddElementFieldMapping("StoreEntity", "AutoDownloadMinutes", "AutoDownloadMinutes", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 21);
			this.AddElementFieldMapping("StoreEntity", "AutoDownloadOnlyAway", "AutoDownloadOnlyAway", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 22);
			this.AddElementFieldMapping("StoreEntity", "AddressValidationSetting", "AddressValidationSetting", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 23);
			this.AddElementFieldMapping("StoreEntity", "ComputerDownloadPolicy", "ComputerDownloadPolicy", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 24);
			this.AddElementFieldMapping("StoreEntity", "DefaultEmailAccountID", "DefaultEmailAccountID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 25);
			this.AddElementFieldMapping("StoreEntity", "ManualOrderPrefix", "ManualOrderPrefix", false, "NVarChar", 10, 0, 0, false, "", null, typeof(System.String), 26);
			this.AddElementFieldMapping("StoreEntity", "ManualOrderPostfix", "ManualOrderPostfix", false, "NVarChar", 10, 0, 0, false, "", null, typeof(System.String), 27);
			this.AddElementFieldMapping("StoreEntity", "InitialDownloadDays", "InitialDownloadDays", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 28);
			this.AddElementFieldMapping("StoreEntity", "InitialDownloadOrder", "InitialDownloadOrder", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 29);
		}

		/// <summary>Inits SystemDataEntity's mappings</summary>
		private void InitSystemDataEntityMappings()
		{
			this.AddElementMapping("SystemDataEntity", @"ShipWorksLocal", @"dbo", "SystemData", 5, 0);
			this.AddElementFieldMapping("SystemDataEntity", "SystemDataID", "SystemDataID", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 0);
			this.AddElementFieldMapping("SystemDataEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("SystemDataEntity", "DatabaseID", "DatabaseID", false, "UniqueIdentifier", 0, 0, 0, false, "", null, typeof(System.Guid), 2);
			this.AddElementFieldMapping("SystemDataEntity", "DateFiltersLastUpdate", "DateFiltersLastUpdate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 3);
			this.AddElementFieldMapping("SystemDataEntity", "TemplateVersion", "TemplateVersion", false, "VarChar", 30, 0, 0, false, "", null, typeof(System.String), 4);
		}

		/// <summary>Inits TemplateEntity's mappings</summary>
		private void InitTemplateEntityMappings()
		{
			this.AddElementMapping("TemplateEntity", @"ShipWorksLocal", @"dbo", "Template", 23, 0);
			this.AddElementFieldMapping("TemplateEntity", "TemplateID", "TemplateID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("TemplateEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("TemplateEntity", "ParentFolderID", "ParentFolderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("TemplateEntity", "Name", "Name", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("TemplateEntity", "Xsl", "Xsl", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("TemplateEntity", "Type", "Type", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("TemplateEntity", "Context", "Context", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
			this.AddElementFieldMapping("TemplateEntity", "OutputFormat", "OutputFormat", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 7);
			this.AddElementFieldMapping("TemplateEntity", "OutputEncoding", "OutputEncoding", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("TemplateEntity", "PageMarginLeft", "PageMarginLeft", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 9);
			this.AddElementFieldMapping("TemplateEntity", "PageMarginRight", "PageMarginRight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 10);
			this.AddElementFieldMapping("TemplateEntity", "PageMarginBottom", "PageMarginBottom", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 11);
			this.AddElementFieldMapping("TemplateEntity", "PageMarginTop", "PageMarginTop", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 12);
			this.AddElementFieldMapping("TemplateEntity", "PageWidth", "PageWidth", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 13);
			this.AddElementFieldMapping("TemplateEntity", "PageHeight", "PageHeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 14);
			this.AddElementFieldMapping("TemplateEntity", "LabelSheetID", "LabelSheetID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 15);
			this.AddElementFieldMapping("TemplateEntity", "PrintCopies", "PrintCopies", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 16);
			this.AddElementFieldMapping("TemplateEntity", "PrintCollate", "PrintCollate", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 17);
			this.AddElementFieldMapping("TemplateEntity", "SaveFileName", "SaveFileName", false, "NVarChar", 500, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("TemplateEntity", "SaveFileFolder", "SaveFileFolder", false, "NVarChar", 500, 0, 0, false, "", null, typeof(System.String), 19);
			this.AddElementFieldMapping("TemplateEntity", "SaveFilePrompt", "SaveFilePrompt", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 20);
			this.AddElementFieldMapping("TemplateEntity", "SaveFileBOM", "SaveFileBOM", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 21);
			this.AddElementFieldMapping("TemplateEntity", "SaveFileOnlineResources", "SaveFileOnlineResources", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 22);
		}

		/// <summary>Inits TemplateComputerSettingsEntity's mappings</summary>
		private void InitTemplateComputerSettingsEntityMappings()
		{
			this.AddElementMapping("TemplateComputerSettingsEntity", @"ShipWorksLocal", @"dbo", "TemplateComputerSettings", 5, 0);
			this.AddElementFieldMapping("TemplateComputerSettingsEntity", "TemplateComputerSettingsID", "TemplateComputerSettingsID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("TemplateComputerSettingsEntity", "TemplateID", "TemplateID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("TemplateComputerSettingsEntity", "ComputerID", "ComputerID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("TemplateComputerSettingsEntity", "PrinterName", "PrinterName", false, "NVarChar", 350, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("TemplateComputerSettingsEntity", "PaperSource", "PaperSource", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
		}

		/// <summary>Inits TemplateFolderEntity's mappings</summary>
		private void InitTemplateFolderEntityMappings()
		{
			this.AddElementMapping("TemplateFolderEntity", @"ShipWorksLocal", @"dbo", "TemplateFolder", 4, 0);
			this.AddElementFieldMapping("TemplateFolderEntity", "TemplateFolderID", "TemplateFolderID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("TemplateFolderEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("TemplateFolderEntity", "ParentFolderID", "ParentFolderID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("TemplateFolderEntity", "Name", "Name", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 3);
		}

		/// <summary>Inits TemplateStoreSettingsEntity's mappings</summary>
		private void InitTemplateStoreSettingsEntityMappings()
		{
			this.AddElementMapping("TemplateStoreSettingsEntity", @"ShipWorksLocal", @"dbo", "TemplateStoreSettings", 9, 0);
			this.AddElementFieldMapping("TemplateStoreSettingsEntity", "TemplateStoreSettingsID", "TemplateStoreSettingsID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("TemplateStoreSettingsEntity", "TemplateID", "TemplateID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("TemplateStoreSettingsEntity", "StoreID", "StoreID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("TemplateStoreSettingsEntity", "EmailUseDefault", "EmailUseDefault", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("TemplateStoreSettingsEntity", "EmailAccountID", "EmailAccountID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
			this.AddElementFieldMapping("TemplateStoreSettingsEntity", "EmailTo", "EmailTo", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("TemplateStoreSettingsEntity", "EmailCc", "EmailCc", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("TemplateStoreSettingsEntity", "EmailBcc", "EmailBcc", false, "NVarChar", 2147483647, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("TemplateStoreSettingsEntity", "EmailSubject", "EmailSubject", false, "NVarChar", 500, 0, 0, false, "", null, typeof(System.String), 8);
		}

		/// <summary>Inits TemplateUserSettingsEntity's mappings</summary>
		private void InitTemplateUserSettingsEntityMappings()
		{
			this.AddElementMapping("TemplateUserSettingsEntity", @"ShipWorksLocal", @"dbo", "TemplateUserSettings", 7, 0);
			this.AddElementFieldMapping("TemplateUserSettingsEntity", "TemplateUserSettingsID", "TemplateUserSettingsID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("TemplateUserSettingsEntity", "TemplateID", "TemplateID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("TemplateUserSettingsEntity", "UserID", "UserID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("TemplateUserSettingsEntity", "PreviewSource", "PreviewSource", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("TemplateUserSettingsEntity", "PreviewCount", "PreviewCount", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("TemplateUserSettingsEntity", "PreviewFilterNodeID", "PreviewFilterNodeID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 5);
			this.AddElementFieldMapping("TemplateUserSettingsEntity", "PreviewZoom", "PreviewZoom", false, "NVarChar", 10, 0, 0, false, "", null, typeof(System.String), 6);
		}

		/// <summary>Inits ThreeDCartOrderEntity's mappings</summary>
		private void InitThreeDCartOrderEntityMappings()
		{
			this.AddElementMapping("ThreeDCartOrderEntity", @"ShipWorksLocal", @"dbo", "ThreeDCartOrder", 2, 0);
			this.AddElementFieldMapping("ThreeDCartOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ThreeDCartOrderEntity", "ThreeDCartOrderID", "ThreeDCartOrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
		}

		/// <summary>Inits ThreeDCartOrderItemEntity's mappings</summary>
		private void InitThreeDCartOrderItemEntityMappings()
		{
			this.AddElementMapping("ThreeDCartOrderItemEntity", @"ShipWorksLocal", @"dbo", "ThreeDCartOrderItem", 2, 0);
			this.AddElementFieldMapping("ThreeDCartOrderItemEntity", "OrderItemID", "OrderItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ThreeDCartOrderItemEntity", "ThreeDCartShipmentID", "ThreeDCartShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
		}

		/// <summary>Inits ThreeDCartStoreEntity's mappings</summary>
		private void InitThreeDCartStoreEntityMappings()
		{
			this.AddElementMapping("ThreeDCartStoreEntity", @"ShipWorksLocal", @"dbo", "ThreeDCartStore", 7, 0);
			this.AddElementFieldMapping("ThreeDCartStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ThreeDCartStoreEntity", "StoreUrl", "StoreUrl", false, "NVarChar", 110, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("ThreeDCartStoreEntity", "ApiUserKey", "ApiUserKey", false, "NVarChar", 65, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("ThreeDCartStoreEntity", "TimeZoneID", "TimeZoneID", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("ThreeDCartStoreEntity", "StatusCodes", "StatusCodes", true, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("ThreeDCartStoreEntity", "DownloadModifiedNumberOfDaysBack", "DownloadModifiedNumberOfDaysBack", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("ThreeDCartStoreEntity", "RestUser", "RestUser", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
		}

		/// <summary>Inits UpsAccountEntity's mappings</summary>
		private void InitUpsAccountEntityMappings()
		{
			this.AddElementMapping("UpsAccountEntity", @"ShipWorksLocal", @"dbo", "UpsAccount", 23, 0);
			this.AddElementFieldMapping("UpsAccountEntity", "UpsAccountID", "UpsAccountID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("UpsAccountEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("UpsAccountEntity", "Description", "Description", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("UpsAccountEntity", "AccountNumber", "AccountNumber", false, "NVarChar", 10, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("UpsAccountEntity", "UserID", "UserID", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("UpsAccountEntity", "Password", "Password", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("UpsAccountEntity", "RateType", "RateType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
			this.AddElementFieldMapping("UpsAccountEntity", "InvoiceAuth", "InvoiceAuth", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 7);
			this.AddElementFieldMapping("UpsAccountEntity", "FirstName", "FirstName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("UpsAccountEntity", "MiddleName", "MiddleName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("UpsAccountEntity", "LastName", "LastName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("UpsAccountEntity", "Company", "Company", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("UpsAccountEntity", "Street1", "Street1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("UpsAccountEntity", "Street2", "Street2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("UpsAccountEntity", "Street3", "Street3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("UpsAccountEntity", "City", "City", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("UpsAccountEntity", "StateProvCode", "StateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("UpsAccountEntity", "PostalCode", "PostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 17);
			this.AddElementFieldMapping("UpsAccountEntity", "CountryCode", "CountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("UpsAccountEntity", "Phone", "Phone", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 19);
			this.AddElementFieldMapping("UpsAccountEntity", "Email", "Email", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 20);
			this.AddElementFieldMapping("UpsAccountEntity", "Website", "Website", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 21);
			this.AddElementFieldMapping("UpsAccountEntity", "PromoStatus", "PromoStatus", false, "TinyInt", 0, 3, 0, false, "", null, typeof(System.Byte), 22);
		}

		/// <summary>Inits UpsPackageEntity's mappings</summary>
		private void InitUpsPackageEntityMappings()
		{
			this.AddElementMapping("UpsPackageEntity", @"ShipWorksLocal", @"dbo", "UpsPackage", 25, 0);
			this.AddElementFieldMapping("UpsPackageEntity", "UpsPackageID", "UpsPackageID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("UpsPackageEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("UpsPackageEntity", "PackagingType", "PackagingType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("UpsPackageEntity", "Weight", "Weight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 3);
			this.AddElementFieldMapping("UpsPackageEntity", "DimsProfileID", "DimsProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
			this.AddElementFieldMapping("UpsPackageEntity", "DimsLength", "DimsLength", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 5);
			this.AddElementFieldMapping("UpsPackageEntity", "DimsWidth", "DimsWidth", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 6);
			this.AddElementFieldMapping("UpsPackageEntity", "DimsHeight", "DimsHeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 7);
			this.AddElementFieldMapping("UpsPackageEntity", "DimsWeight", "DimsWeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 8);
			this.AddElementFieldMapping("UpsPackageEntity", "DimsAddWeight", "DimsAddWeight", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 9);
			this.AddElementFieldMapping("UpsPackageEntity", "Insurance", "Insurance", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 10);
			this.AddElementFieldMapping("UpsPackageEntity", "InsuranceValue", "InsuranceValue", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 11);
			this.AddElementFieldMapping("UpsPackageEntity", "InsurancePennyOne", "InsurancePennyOne", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 12);
			this.AddElementFieldMapping("UpsPackageEntity", "DeclaredValue", "DeclaredValue", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 13);
			this.AddElementFieldMapping("UpsPackageEntity", "TrackingNumber", "TrackingNumber", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("UpsPackageEntity", "UspsTrackingNumber", "UspsTrackingNumber", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("UpsPackageEntity", "AdditionalHandlingEnabled", "AdditionalHandlingEnabled", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 16);
			this.AddElementFieldMapping("UpsPackageEntity", "VerbalConfirmationEnabled", "VerbalConfirmationEnabled", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 17);
			this.AddElementFieldMapping("UpsPackageEntity", "VerbalConfirmationName", "VerbalConfirmationName", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("UpsPackageEntity", "VerbalConfirmationPhone", "VerbalConfirmationPhone", false, "NVarChar", 15, 0, 0, false, "", null, typeof(System.String), 19);
			this.AddElementFieldMapping("UpsPackageEntity", "VerbalConfirmationPhoneExtension", "VerbalConfirmationPhoneExtension", false, "NVarChar", 4, 0, 0, false, "", null, typeof(System.String), 20);
			this.AddElementFieldMapping("UpsPackageEntity", "DryIceEnabled", "DryIceEnabled", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 21);
			this.AddElementFieldMapping("UpsPackageEntity", "DryIceRegulationSet", "DryIceRegulationSet", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 22);
			this.AddElementFieldMapping("UpsPackageEntity", "DryIceWeight", "DryIceWeight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 23);
			this.AddElementFieldMapping("UpsPackageEntity", "DryIceIsForMedicalUse", "DryIceIsForMedicalUse", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 24);
		}

		/// <summary>Inits UpsProfileEntity's mappings</summary>
		private void InitUpsProfileEntityMappings()
		{
			this.AddElementMapping("UpsProfileEntity", @"ShipWorksLocal", @"dbo", "UpsProfile", 36, 0);
			this.AddElementFieldMapping("UpsProfileEntity", "ShippingProfileID", "ShippingProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("UpsProfileEntity", "UpsAccountID", "UpsAccountID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("UpsProfileEntity", "Service", "Service", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("UpsProfileEntity", "SaturdayDelivery", "SaturdayDelivery", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("UpsProfileEntity", "ResidentialDetermination", "ResidentialDetermination", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
			this.AddElementFieldMapping("UpsProfileEntity", "DeliveryConfirmation", "DeliveryConfirmation", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 5);
			this.AddElementFieldMapping("UpsProfileEntity", "ReferenceNumber", "ReferenceNumber", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("UpsProfileEntity", "ReferenceNumber2", "ReferenceNumber2", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("UpsProfileEntity", "PayorType", "PayorType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 8);
			this.AddElementFieldMapping("UpsProfileEntity", "PayorAccount", "PayorAccount", true, "VarChar", 10, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("UpsProfileEntity", "PayorPostalCode", "PayorPostalCode", true, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("UpsProfileEntity", "PayorCountryCode", "PayorCountryCode", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("UpsProfileEntity", "EmailNotifySender", "EmailNotifySender", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 12);
			this.AddElementFieldMapping("UpsProfileEntity", "EmailNotifyRecipient", "EmailNotifyRecipient", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 13);
			this.AddElementFieldMapping("UpsProfileEntity", "EmailNotifyOther", "EmailNotifyOther", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 14);
			this.AddElementFieldMapping("UpsProfileEntity", "EmailNotifyOtherAddress", "EmailNotifyOtherAddress", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("UpsProfileEntity", "EmailNotifyFrom", "EmailNotifyFrom", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("UpsProfileEntity", "EmailNotifySubject", "EmailNotifySubject", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 17);
			this.AddElementFieldMapping("UpsProfileEntity", "EmailNotifyMessage", "EmailNotifyMessage", true, "NVarChar", 120, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("UpsProfileEntity", "ReturnService", "ReturnService", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 19);
			this.AddElementFieldMapping("UpsProfileEntity", "ReturnUndeliverableEmail", "ReturnUndeliverableEmail", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 20);
			this.AddElementFieldMapping("UpsProfileEntity", "ReturnContents", "ReturnContents", true, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 21);
			this.AddElementFieldMapping("UpsProfileEntity", "Endorsement", "Endorsement", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 22);
			this.AddElementFieldMapping("UpsProfileEntity", "Subclassification", "Subclassification", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 23);
			this.AddElementFieldMapping("UpsProfileEntity", "PaperlessAdditionalDocumentation", "PaperlessAdditionalDocumentation", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 24);
			this.AddElementFieldMapping("UpsProfileEntity", "ShipperRelease", "ShipperRelease", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 25);
			this.AddElementFieldMapping("UpsProfileEntity", "CarbonNeutral", "CarbonNeutral", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 26);
			this.AddElementFieldMapping("UpsProfileEntity", "CommercialPaperlessInvoice", "CommercialPaperlessInvoice", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 27);
			this.AddElementFieldMapping("UpsProfileEntity", "CostCenter", "CostCenter", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 28);
			this.AddElementFieldMapping("UpsProfileEntity", "IrregularIndicator", "IrregularIndicator", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 29);
			this.AddElementFieldMapping("UpsProfileEntity", "Cn22Number", "Cn22Number", true, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 30);
			this.AddElementFieldMapping("UpsProfileEntity", "ShipmentChargeType", "ShipmentChargeType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 31);
			this.AddElementFieldMapping("UpsProfileEntity", "ShipmentChargeAccount", "ShipmentChargeAccount", true, "VarChar", 10, 0, 0, false, "", null, typeof(System.String), 32);
			this.AddElementFieldMapping("UpsProfileEntity", "ShipmentChargePostalCode", "ShipmentChargePostalCode", true, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 33);
			this.AddElementFieldMapping("UpsProfileEntity", "ShipmentChargeCountryCode", "ShipmentChargeCountryCode", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 34);
			this.AddElementFieldMapping("UpsProfileEntity", "UspsPackageID", "UspsPackageID", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 35);
		}

		/// <summary>Inits UpsProfilePackageEntity's mappings</summary>
		private void InitUpsProfilePackageEntityMappings()
		{
			this.AddElementMapping("UpsProfilePackageEntity", @"ShipWorksLocal", @"dbo", "UpsProfilePackage", 19, 0);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "UpsProfilePackageID", "UpsProfilePackageID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "ShippingProfileID", "ShippingProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "PackagingType", "PackagingType", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "Weight", "Weight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 3);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "DimsProfileID", "DimsProfileID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "DimsLength", "DimsLength", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 5);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "DimsWidth", "DimsWidth", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 6);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "DimsHeight", "DimsHeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 7);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "DimsWeight", "DimsWeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 8);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "DimsAddWeight", "DimsAddWeight", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 9);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "AdditionalHandlingEnabled", "AdditionalHandlingEnabled", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 10);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "VerbalConfirmationEnabled", "VerbalConfirmationEnabled", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 11);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "VerbalConfirmationName", "VerbalConfirmationName", true, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "VerbalConfirmationPhone", "VerbalConfirmationPhone", true, "NVarChar", 15, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "VerbalConfirmationPhoneExtension", "VerbalConfirmationPhoneExtension", true, "NVarChar", 4, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "DryIceEnabled", "DryIceEnabled", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 15);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "DryIceRegulationSet", "DryIceRegulationSet", true, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 16);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "DryIceWeight", "DryIceWeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 17);
			this.AddElementFieldMapping("UpsProfilePackageEntity", "DryIceIsForMedicalUse", "DryIceIsForMedicalUse", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 18);
		}

		/// <summary>Inits UpsShipmentEntity's mappings</summary>
		private void InitUpsShipmentEntityMappings()
		{
			this.AddElementMapping("UpsShipmentEntity", @"ShipWorksLocal", @"dbo", "UpsShipment", 51, 0);
			this.AddElementFieldMapping("UpsShipmentEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("UpsShipmentEntity", "UpsAccountID", "UpsAccountID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("UpsShipmentEntity", "Service", "Service", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 2);
			this.AddElementFieldMapping("UpsShipmentEntity", "SaturdayDelivery", "SaturdayDelivery", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("UpsShipmentEntity", "CodEnabled", "CodEnabled", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 4);
			this.AddElementFieldMapping("UpsShipmentEntity", "CodAmount", "CodAmount", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 5);
			this.AddElementFieldMapping("UpsShipmentEntity", "CodPaymentType", "CodPaymentType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
			this.AddElementFieldMapping("UpsShipmentEntity", "DeliveryConfirmation", "DeliveryConfirmation", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 7);
			this.AddElementFieldMapping("UpsShipmentEntity", "ReferenceNumber", "ReferenceNumber", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("UpsShipmentEntity", "ReferenceNumber2", "ReferenceNumber2", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("UpsShipmentEntity", "PayorType", "PayorType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 10);
			this.AddElementFieldMapping("UpsShipmentEntity", "PayorAccount", "PayorAccount", false, "VarChar", 10, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("UpsShipmentEntity", "PayorPostalCode", "PayorPostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("UpsShipmentEntity", "PayorCountryCode", "PayorCountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("UpsShipmentEntity", "EmailNotifySender", "EmailNotifySender", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 14);
			this.AddElementFieldMapping("UpsShipmentEntity", "EmailNotifyRecipient", "EmailNotifyRecipient", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 15);
			this.AddElementFieldMapping("UpsShipmentEntity", "EmailNotifyOther", "EmailNotifyOther", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 16);
			this.AddElementFieldMapping("UpsShipmentEntity", "EmailNotifyOtherAddress", "EmailNotifyOtherAddress", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 17);
			this.AddElementFieldMapping("UpsShipmentEntity", "EmailNotifyFrom", "EmailNotifyFrom", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("UpsShipmentEntity", "EmailNotifySubject", "EmailNotifySubject", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 19);
			this.AddElementFieldMapping("UpsShipmentEntity", "EmailNotifyMessage", "EmailNotifyMessage", false, "NVarChar", 120, 0, 0, false, "", null, typeof(System.String), 20);
			this.AddElementFieldMapping("UpsShipmentEntity", "CustomsDocumentsOnly", "CustomsDocumentsOnly", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 21);
			this.AddElementFieldMapping("UpsShipmentEntity", "CustomsDescription", "CustomsDescription", false, "NVarChar", 150, 0, 0, false, "", null, typeof(System.String), 22);
			this.AddElementFieldMapping("UpsShipmentEntity", "CommercialPaperlessInvoice", "CommercialPaperlessInvoice", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 23);
			this.AddElementFieldMapping("UpsShipmentEntity", "CommercialInvoiceTermsOfSale", "CommercialInvoiceTermsOfSale", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 24);
			this.AddElementFieldMapping("UpsShipmentEntity", "CommercialInvoicePurpose", "CommercialInvoicePurpose", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 25);
			this.AddElementFieldMapping("UpsShipmentEntity", "CommercialInvoiceComments", "CommercialInvoiceComments", false, "NVarChar", 200, 0, 0, false, "", null, typeof(System.String), 26);
			this.AddElementFieldMapping("UpsShipmentEntity", "CommercialInvoiceFreight", "CommercialInvoiceFreight", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 27);
			this.AddElementFieldMapping("UpsShipmentEntity", "CommercialInvoiceInsurance", "CommercialInvoiceInsurance", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 28);
			this.AddElementFieldMapping("UpsShipmentEntity", "CommercialInvoiceOther", "CommercialInvoiceOther", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 29);
			this.AddElementFieldMapping("UpsShipmentEntity", "WorldShipStatus", "WorldShipStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 30);
			this.AddElementFieldMapping("UpsShipmentEntity", "PublishedCharges", "PublishedCharges", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 31);
			this.AddElementFieldMapping("UpsShipmentEntity", "NegotiatedRate", "NegotiatedRate", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 32);
			this.AddElementFieldMapping("UpsShipmentEntity", "ReturnService", "ReturnService", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 33);
			this.AddElementFieldMapping("UpsShipmentEntity", "ReturnUndeliverableEmail", "ReturnUndeliverableEmail", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 34);
			this.AddElementFieldMapping("UpsShipmentEntity", "ReturnContents", "ReturnContents", false, "NVarChar", 300, 0, 0, false, "", null, typeof(System.String), 35);
			this.AddElementFieldMapping("UpsShipmentEntity", "UspsTrackingNumber", "UspsTrackingNumber", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 36);
			this.AddElementFieldMapping("UpsShipmentEntity", "Endorsement", "Endorsement", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 37);
			this.AddElementFieldMapping("UpsShipmentEntity", "Subclassification", "Subclassification", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 38);
			this.AddElementFieldMapping("UpsShipmentEntity", "PaperlessAdditionalDocumentation", "PaperlessAdditionalDocumentation", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 39);
			this.AddElementFieldMapping("UpsShipmentEntity", "ShipperRelease", "ShipperRelease", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 40);
			this.AddElementFieldMapping("UpsShipmentEntity", "CarbonNeutral", "CarbonNeutral", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 41);
			this.AddElementFieldMapping("UpsShipmentEntity", "CostCenter", "CostCenter", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 42);
			this.AddElementFieldMapping("UpsShipmentEntity", "IrregularIndicator", "IrregularIndicator", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 43);
			this.AddElementFieldMapping("UpsShipmentEntity", "Cn22Number", "Cn22Number", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 44);
			this.AddElementFieldMapping("UpsShipmentEntity", "ShipmentChargeType", "ShipmentChargeType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 45);
			this.AddElementFieldMapping("UpsShipmentEntity", "ShipmentChargeAccount", "ShipmentChargeAccount", false, "VarChar", 10, 0, 0, false, "", null, typeof(System.String), 46);
			this.AddElementFieldMapping("UpsShipmentEntity", "ShipmentChargePostalCode", "ShipmentChargePostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 47);
			this.AddElementFieldMapping("UpsShipmentEntity", "ShipmentChargeCountryCode", "ShipmentChargeCountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 48);
			this.AddElementFieldMapping("UpsShipmentEntity", "UspsPackageID", "UspsPackageID", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 49);
			this.AddElementFieldMapping("UpsShipmentEntity", "RequestedLabelFormat", "RequestedLabelFormat", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 50);
		}

		/// <summary>Inits UserEntity's mappings</summary>
		private void InitUserEntityMappings()
		{
			this.AddElementMapping("UserEntity", @"ShipWorksLocal", @"dbo", "User", 7, 0);
			this.AddElementFieldMapping("UserEntity", "UserID", "UserID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("UserEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("UserEntity", "Username", "Username", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("UserEntity", "Password", "Password", false, "NVarChar", 32, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("UserEntity", "Email", "Email", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("UserEntity", "IsAdmin", "IsAdmin", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 5);
			this.AddElementFieldMapping("UserEntity", "IsDeleted", "IsDeleted", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 6);
		}

		/// <summary>Inits UserColumnSettingsEntity's mappings</summary>
		private void InitUserColumnSettingsEntityMappings()
		{
			this.AddElementMapping("UserColumnSettingsEntity", @"ShipWorksLocal", @"dbo", "UserColumnSettings", 5, 0);
			this.AddElementFieldMapping("UserColumnSettingsEntity", "UserColumnSettingsID", "UserColumnSettingsID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("UserColumnSettingsEntity", "SettingsKey", "SettingsKey", false, "UniqueIdentifier", 0, 0, 0, false, "", null, typeof(System.Guid), 1);
			this.AddElementFieldMapping("UserColumnSettingsEntity", "UserID", "UserID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("UserColumnSettingsEntity", "InitialSortType", "InitialSortType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 3);
			this.AddElementFieldMapping("UserColumnSettingsEntity", "GridColumnLayoutID", "GridColumnLayoutID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 4);
		}

		/// <summary>Inits UserSettingsEntity's mappings</summary>
		private void InitUserSettingsEntityMappings()
		{
			this.AddElementMapping("UserSettingsEntity", @"ShipWorksLocal", @"dbo", "UserSettings", 17, 0);
			this.AddElementFieldMapping("UserSettingsEntity", "UserID", "UserID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("UserSettingsEntity", "DisplayColorScheme", "DisplayColorScheme", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 1);
			this.AddElementFieldMapping("UserSettingsEntity", "DisplaySystemTray", "DisplaySystemTray", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 2);
			this.AddElementFieldMapping("UserSettingsEntity", "WindowLayout", "WindowLayout", false, "VarBinary", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 3);
			this.AddElementFieldMapping("UserSettingsEntity", "GridMenuLayout", "GridMenuLayout", true, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("UserSettingsEntity", "FilterInitialUseLastActive", "FilterInitialUseLastActive", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 5);
			this.AddElementFieldMapping("UserSettingsEntity", "FilterInitialSpecified", "FilterInitialSpecified", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 6);
			this.AddElementFieldMapping("UserSettingsEntity", "FilterInitialSortType", "FilterInitialSortType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 7);
			this.AddElementFieldMapping("UserSettingsEntity", "OrderFilterLastActive", "OrderFilterLastActive", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 8);
			this.AddElementFieldMapping("UserSettingsEntity", "OrderFilterExpandedFolders", "OrderFilterExpandedFolders", true, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("UserSettingsEntity", "ShippingWeightFormat", "ShippingWeightFormat", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 10);
			this.AddElementFieldMapping("UserSettingsEntity", "TemplateExpandedFolders", "TemplateExpandedFolders", true, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("UserSettingsEntity", "TemplateLastSelected", "TemplateLastSelected", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 12);
			this.AddElementFieldMapping("UserSettingsEntity", "CustomerFilterLastActive", "CustomerFilterLastActive", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 13);
			this.AddElementFieldMapping("UserSettingsEntity", "CustomerFilterExpandedFolders", "CustomerFilterExpandedFolders", true, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("UserSettingsEntity", "NextGlobalPostNotificationDate", "NextGlobalPostNotificationDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 15);
			this.AddElementFieldMapping("UserSettingsEntity", "SingleScanSettings", "SingleScanSettings", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 16);
		}

		/// <summary>Inits UspsAccountEntity's mappings</summary>
		private void InitUspsAccountEntityMappings()
		{
			this.AddElementMapping("UspsAccountEntity", @"ShipWorksLocal", @"dbo", "UspsAccount", 25, 0);
			this.AddElementFieldMapping("UspsAccountEntity", "UspsAccountID", "UspsAccountID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("UspsAccountEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1);
			this.AddElementFieldMapping("UspsAccountEntity", "Description", "Description", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("UspsAccountEntity", "Username", "Username", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("UspsAccountEntity", "Password", "Password", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("UspsAccountEntity", "FirstName", "FirstName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("UspsAccountEntity", "MiddleName", "MiddleName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("UspsAccountEntity", "LastName", "LastName", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("UspsAccountEntity", "Company", "Company", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("UspsAccountEntity", "Street1", "Street1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("UspsAccountEntity", "Street2", "Street2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("UspsAccountEntity", "Street3", "Street3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("UspsAccountEntity", "City", "City", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("UspsAccountEntity", "StateProvCode", "StateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("UspsAccountEntity", "PostalCode", "PostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("UspsAccountEntity", "CountryCode", "CountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("UspsAccountEntity", "Phone", "Phone", false, "NVarChar", 25, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("UspsAccountEntity", "Email", "Email", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 17);
			this.AddElementFieldMapping("UspsAccountEntity", "Website", "Website", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("UspsAccountEntity", "MailingPostalCode", "MailingPostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 19);
			this.AddElementFieldMapping("UspsAccountEntity", "UspsReseller", "UspsReseller", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 20);
			this.AddElementFieldMapping("UspsAccountEntity", "ContractType", "ContractType", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 21);
			this.AddElementFieldMapping("UspsAccountEntity", "CreatedDate", "CreatedDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 22);
			this.AddElementFieldMapping("UspsAccountEntity", "PendingInitialAccount", "PendingInitialAccount", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 23);
			this.AddElementFieldMapping("UspsAccountEntity", "GlobalPostAvailability", "GlobalPostAvailability", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 24);
		}

		/// <summary>Inits UspsProfileEntity's mappings</summary>
		private void InitUspsProfileEntityMappings()
		{
			this.AddElementMapping("UspsProfileEntity", @"ShipWorksLocal", @"dbo", "UspsProfile", 5, 0);
			this.AddElementFieldMapping("UspsProfileEntity", "ShippingProfileID", "ShippingProfileID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("UspsProfileEntity", "UspsAccountID", "UspsAccountID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("UspsProfileEntity", "HidePostage", "HidePostage", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 2);
			this.AddElementFieldMapping("UspsProfileEntity", "RequireFullAddressValidation", "RequireFullAddressValidation", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("UspsProfileEntity", "RateShop", "RateShop", true, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 4);
		}

		/// <summary>Inits UspsScanFormEntity's mappings</summary>
		private void InitUspsScanFormEntityMappings()
		{
			this.AddElementMapping("UspsScanFormEntity", @"ShipWorksLocal", @"dbo", "UspsScanForm", 7, 0);
			this.AddElementFieldMapping("UspsScanFormEntity", "UspsScanFormID", "UspsScanFormID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("UspsScanFormEntity", "UspsAccountID", "UspsAccountID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("UspsScanFormEntity", "ScanFormTransactionID", "ScanFormTransactionID", false, "VarChar", 100, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("UspsScanFormEntity", "ScanFormUrl", "ScanFormUrl", false, "VarChar", 2048, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("UspsScanFormEntity", "CreatedDate", "CreatedDate", false, "DateTime", 0, 0, 0, false, "", null, typeof(System.DateTime), 4);
			this.AddElementFieldMapping("UspsScanFormEntity", "ScanFormBatchID", "ScanFormBatchID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 5);
			this.AddElementFieldMapping("UspsScanFormEntity", "Description", "Description", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 6);
		}

		/// <summary>Inits UspsShipmentEntity's mappings</summary>
		private void InitUspsShipmentEntityMappings()
		{
			this.AddElementMapping("UspsShipmentEntity", @"ShipWorksLocal", @"dbo", "UspsShipment", 10, 0);
			this.AddElementFieldMapping("UspsShipmentEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("UspsShipmentEntity", "UspsAccountID", "UspsAccountID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("UspsShipmentEntity", "HidePostage", "HidePostage", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 2);
			this.AddElementFieldMapping("UspsShipmentEntity", "RequireFullAddressValidation", "RequireFullAddressValidation", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("UspsShipmentEntity", "IntegratorTransactionID", "IntegratorTransactionID", false, "UniqueIdentifier", 0, 0, 0, false, "", null, typeof(System.Guid), 4);
			this.AddElementFieldMapping("UspsShipmentEntity", "UspsTransactionID", "UspsTransactionID", false, "UniqueIdentifier", 0, 0, 0, false, "", null, typeof(System.Guid), 5);
			this.AddElementFieldMapping("UspsShipmentEntity", "OriginalUspsAccountID", "OriginalUspsAccountID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 6);
			this.AddElementFieldMapping("UspsShipmentEntity", "ScanFormBatchID", "ScanFormBatchID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 7);
			this.AddElementFieldMapping("UspsShipmentEntity", "RequestedLabelFormat", "RequestedLabelFormat", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 8);
			this.AddElementFieldMapping("UspsShipmentEntity", "RateShop", "RateShop", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 9);
		}

		/// <summary>Inits ValidatedAddressEntity's mappings</summary>
		private void InitValidatedAddressEntityMappings()
		{
			this.AddElementMapping("ValidatedAddressEntity", @"ShipWorksLocal", @"dbo", "ValidatedAddress", 15, 0);
			this.AddElementFieldMapping("ValidatedAddressEntity", "ValidatedAddressID", "ValidatedAddressID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("ValidatedAddressEntity", "ConsumerID", "ConsumerID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("ValidatedAddressEntity", "AddressPrefix", "AddressPrefix", false, "NVarChar", 10, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("ValidatedAddressEntity", "IsOriginal", "IsOriginal", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 3);
			this.AddElementFieldMapping("ValidatedAddressEntity", "Street1", "Street1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("ValidatedAddressEntity", "Street2", "Street2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("ValidatedAddressEntity", "Street3", "Street3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("ValidatedAddressEntity", "City", "City", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("ValidatedAddressEntity", "StateProvCode", "StateProvCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("ValidatedAddressEntity", "PostalCode", "PostalCode", false, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("ValidatedAddressEntity", "CountryCode", "CountryCode", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("ValidatedAddressEntity", "ResidentialStatus", "ResidentialStatus", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 11);
			this.AddElementFieldMapping("ValidatedAddressEntity", "POBox", "POBox", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 12);
			this.AddElementFieldMapping("ValidatedAddressEntity", "USTerritory", "USTerritory", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 13);
			this.AddElementFieldMapping("ValidatedAddressEntity", "MilitaryAddress", "MilitaryAddress", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 14);
		}

		/// <summary>Inits VersionSignoffEntity's mappings</summary>
		private void InitVersionSignoffEntityMappings()
		{
			this.AddElementMapping("VersionSignoffEntity", @"ShipWorksLocal", @"dbo", "VersionSignoff", 4, 0);
			this.AddElementFieldMapping("VersionSignoffEntity", "VersionSignoffID", "VersionSignoffID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("VersionSignoffEntity", "Version", "Version", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("VersionSignoffEntity", "UserID", "UserID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("VersionSignoffEntity", "ComputerID", "ComputerID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 3);
		}

		/// <summary>Inits VolusionStoreEntity's mappings</summary>
		private void InitVolusionStoreEntityMappings()
		{
			this.AddElementMapping("VolusionStoreEntity", @"ShipWorksLocal", @"dbo", "VolusionStore", 10, 0);
			this.AddElementFieldMapping("VolusionStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("VolusionStoreEntity", "StoreUrl", "StoreUrl", false, "VarChar", 255, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("VolusionStoreEntity", "WebUserName", "WebUserName", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("VolusionStoreEntity", "WebPassword", "WebPassword", false, "VarChar", 70, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("VolusionStoreEntity", "ApiPassword", "ApiPassword", false, "VarChar", 100, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("VolusionStoreEntity", "PaymentMethods", "PaymentMethods", false, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("VolusionStoreEntity", "ShipmentMethods", "ShipmentMethods", false, "Xml", 2147483647, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("VolusionStoreEntity", "DownloadOrderStatuses", "DownloadOrderStatuses", false, "VarChar", 255, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("VolusionStoreEntity", "ServerTimeZone", "ServerTimeZone", false, "VarChar", 30, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("VolusionStoreEntity", "ServerTimeZoneDST", "ServerTimeZoneDST", false, "Bit", 0, 0, 0, false, "", null, typeof(System.Boolean), 9);
		}

		/// <summary>Inits WalmartStoreEntity's mappings</summary>
		private void InitWalmartStoreEntityMappings()
		{
			this.AddElementMapping("WalmartStoreEntity", @"ShipWorksLocal", @"dbo", "WalmartStore", 5, 0);
			this.AddElementFieldMapping("WalmartStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("WalmartStoreEntity", "ConsumerID", "ConsumerID", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("WalmartStoreEntity", "PrivateKey", "PrivateKey", false, "NVarChar", 2000, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("WalmartStoreEntity", "ChannelType", "ChannelType", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("WalmartStoreEntity", "DownloadModifiedNumberOfDaysBack", "DownloadModifiedNumberOfDaysBack", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 4);
		}

		/// <summary>Inits WorldShipGoodsEntity's mappings</summary>
		private void InitWorldShipGoodsEntityMappings()
		{
			this.AddElementMapping("WorldShipGoodsEntity", @"ShipWorksLocal", @"dbo", "WorldShipGoods", 11, 0);
			this.AddElementFieldMapping("WorldShipGoodsEntity", "WorldShipGoodsID", "WorldShipGoodsID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("WorldShipGoodsEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("WorldShipGoodsEntity", "ShipmentCustomsItemID", "ShipmentCustomsItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 2);
			this.AddElementFieldMapping("WorldShipGoodsEntity", "Description", "Description", false, "NVarChar", 150, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("WorldShipGoodsEntity", "TariffCode", "TariffCode", false, "VarChar", 15, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("WorldShipGoodsEntity", "CountryOfOrigin", "CountryOfOrigin", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("WorldShipGoodsEntity", "Units", "Units", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 6);
			this.AddElementFieldMapping("WorldShipGoodsEntity", "UnitOfMeasure", "UnitOfMeasure", false, "VarChar", 5, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("WorldShipGoodsEntity", "UnitPrice", "UnitPrice", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 8);
			this.AddElementFieldMapping("WorldShipGoodsEntity", "Weight", "Weight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 9);
			this.AddElementFieldMapping("WorldShipGoodsEntity", "InvoiceCurrencyCode", "InvoiceCurrencyCode", true, "VarChar", 3, 0, 0, false, "", null, typeof(System.String), 10);
		}

		/// <summary>Inits WorldShipPackageEntity's mappings</summary>
		private void InitWorldShipPackageEntityMappings()
		{
			this.AddElementMapping("WorldShipPackageEntity", @"ShipWorksLocal", @"dbo", "WorldShipPackage", 44, 0);
			this.AddElementFieldMapping("WorldShipPackageEntity", "UpsPackageID", "UpsPackageID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("WorldShipPackageEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("WorldShipPackageEntity", "PackageType", "PackageType", false, "VarChar", 35, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("WorldShipPackageEntity", "Weight", "Weight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 3);
			this.AddElementFieldMapping("WorldShipPackageEntity", "ReferenceNumber", "ReferenceNumber", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("WorldShipPackageEntity", "ReferenceNumber2", "ReferenceNumber2", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("WorldShipPackageEntity", "CodOption", "CodOption", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("WorldShipPackageEntity", "CodAmount", "CodAmount", false, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 7);
			this.AddElementFieldMapping("WorldShipPackageEntity", "CodCashOnly", "CodCashOnly", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("WorldShipPackageEntity", "DeliveryConfirmation", "DeliveryConfirmation", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("WorldShipPackageEntity", "DeliveryConfirmationSignature", "DeliveryConfirmationSignature", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("WorldShipPackageEntity", "DeliveryConfirmationAdult", "DeliveryConfirmationAdult", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("WorldShipPackageEntity", "Length", "Length", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 12);
			this.AddElementFieldMapping("WorldShipPackageEntity", "Width", "Width", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 13);
			this.AddElementFieldMapping("WorldShipPackageEntity", "Height", "Height", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 14);
			this.AddElementFieldMapping("WorldShipPackageEntity", "DeclaredValueAmount", "DeclaredValueAmount", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 15);
			this.AddElementFieldMapping("WorldShipPackageEntity", "DeclaredValueOption", "DeclaredValueOption", true, "NChar", 2, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("WorldShipPackageEntity", "CN22GoodsType", "CN22GoodsType", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 17);
			this.AddElementFieldMapping("WorldShipPackageEntity", "CN22Description", "CN22Description", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("WorldShipPackageEntity", "PostalSubClass", "PostalSubClass", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 19);
			this.AddElementFieldMapping("WorldShipPackageEntity", "MIDeliveryConfirmation", "MIDeliveryConfirmation", true, "Char", 1, 0, 0, false, "", null, typeof(System.String), 20);
			this.AddElementFieldMapping("WorldShipPackageEntity", "QvnOption", "QvnOption", true, "Char", 1, 0, 0, false, "", null, typeof(System.String), 21);
			this.AddElementFieldMapping("WorldShipPackageEntity", "QvnFrom", "QvnFrom", true, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 22);
			this.AddElementFieldMapping("WorldShipPackageEntity", "QvnSubjectLine", "QvnSubjectLine", true, "NVarChar", 18, 0, 0, false, "", null, typeof(System.String), 23);
			this.AddElementFieldMapping("WorldShipPackageEntity", "QvnMemo", "QvnMemo", true, "NVarChar", 150, 0, 0, false, "", null, typeof(System.String), 24);
			this.AddElementFieldMapping("WorldShipPackageEntity", "Qvn1ShipNotify", "Qvn1ShipNotify", true, "Char", 1, 0, 0, false, "", null, typeof(System.String), 25);
			this.AddElementFieldMapping("WorldShipPackageEntity", "Qvn1ContactName", "Qvn1ContactName", true, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 26);
			this.AddElementFieldMapping("WorldShipPackageEntity", "Qvn1Email", "Qvn1Email", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 27);
			this.AddElementFieldMapping("WorldShipPackageEntity", "Qvn2ShipNotify", "Qvn2ShipNotify", true, "Char", 1, 0, 0, false, "", null, typeof(System.String), 28);
			this.AddElementFieldMapping("WorldShipPackageEntity", "Qvn2ContactName", "Qvn2ContactName", true, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 29);
			this.AddElementFieldMapping("WorldShipPackageEntity", "Qvn2Email", "Qvn2Email", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 30);
			this.AddElementFieldMapping("WorldShipPackageEntity", "Qvn3ShipNotify", "Qvn3ShipNotify", true, "Char", 1, 0, 0, false, "", null, typeof(System.String), 31);
			this.AddElementFieldMapping("WorldShipPackageEntity", "Qvn3ContactName", "Qvn3ContactName", true, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 32);
			this.AddElementFieldMapping("WorldShipPackageEntity", "Qvn3Email", "Qvn3Email", true, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 33);
			this.AddElementFieldMapping("WorldShipPackageEntity", "ShipperRelease", "ShipperRelease", true, "Char", 1, 0, 0, false, "", null, typeof(System.String), 34);
			this.AddElementFieldMapping("WorldShipPackageEntity", "AdditionalHandlingEnabled", "AdditionalHandlingEnabled", true, "Char", 1, 0, 0, false, "", null, typeof(System.String), 35);
			this.AddElementFieldMapping("WorldShipPackageEntity", "VerbalConfirmationOption", "VerbalConfirmationOption", true, "Char", 1, 0, 0, false, "", null, typeof(System.String), 36);
			this.AddElementFieldMapping("WorldShipPackageEntity", "VerbalConfirmationContactName", "VerbalConfirmationContactName", true, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 37);
			this.AddElementFieldMapping("WorldShipPackageEntity", "VerbalConfirmationTelephone", "VerbalConfirmationTelephone", true, "NVarChar", 15, 0, 0, false, "", null, typeof(System.String), 38);
			this.AddElementFieldMapping("WorldShipPackageEntity", "DryIceRegulationSet", "DryIceRegulationSet", true, "NVarChar", 5, 0, 0, false, "", null, typeof(System.String), 39);
			this.AddElementFieldMapping("WorldShipPackageEntity", "DryIceWeight", "DryIceWeight", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 40);
			this.AddElementFieldMapping("WorldShipPackageEntity", "DryIceMedicalPurpose", "DryIceMedicalPurpose", true, "Char", 1, 0, 0, false, "", null, typeof(System.String), 41);
			this.AddElementFieldMapping("WorldShipPackageEntity", "DryIceOption", "DryIceOption", true, "Char", 1, 0, 0, false, "", null, typeof(System.String), 42);
			this.AddElementFieldMapping("WorldShipPackageEntity", "DryIceWeightUnitOfMeasure", "DryIceWeightUnitOfMeasure", true, "NVarChar", 10, 0, 0, false, "", null, typeof(System.String), 43);
		}

		/// <summary>Inits WorldShipProcessedEntity's mappings</summary>
		private void InitWorldShipProcessedEntityMappings()
		{
			this.AddElementMapping("WorldShipProcessedEntity", @"ShipWorksLocal", @"dbo", "WorldShipProcessed", 17, 0);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "WorldShipProcessedID", "WorldShipProcessedID", false, "BigInt", 0, 19, 0, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "ShipmentID", "ShipmentID", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "RowVersion", "RowVersion", false, "Timestamp", 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 2);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "PublishedCharges", "PublishedCharges", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 3);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "NegotiatedCharges", "NegotiatedCharges", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 4);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "TrackingNumber", "TrackingNumber", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "UspsTrackingNumber", "UspsTrackingNumber", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "ServiceType", "ServiceType", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "PackageType", "PackageType", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "UpsPackageID", "UpsPackageID", true, "NVarChar", 20, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "DeclaredValueAmount", "DeclaredValueAmount", true, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 10);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "DeclaredValueOption", "DeclaredValueOption", true, "NChar", 2, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "WorldShipShipmentID", "WorldShipShipmentID", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "VoidIndicator", "VoidIndicator", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "NumberOfPackages", "NumberOfPackages", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "LeadTrackingNumber", "LeadTrackingNumber", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("WorldShipProcessedEntity", "ShipmentIdCalculated", "ShipmentIdCalculated", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 16);
		}

		/// <summary>Inits WorldShipShipmentEntity's mappings</summary>
		private void InitWorldShipShipmentEntityMappings()
		{
			this.AddElementMapping("WorldShipShipmentEntity", @"ShipWorksLocal", @"dbo", "WorldShipShipment", 66, 0);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ShipmentID", "ShipmentID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "OrderNumber", "OrderNumber", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "FromCompanyOrName", "FromCompanyOrName", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "FromAttention", "FromAttention", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "FromAddress1", "FromAddress1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "FromAddress2", "FromAddress2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 5);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "FromAddress3", "FromAddress3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 6);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "FromCountryCode", "FromCountryCode", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 7);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "FromPostalCode", "FromPostalCode", false, "VarChar", 20, 0, 0, false, "", null, typeof(System.String), 8);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "FromCity", "FromCity", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 9);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "FromStateProvCode", "FromStateProvCode", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 10);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "FromTelephone", "FromTelephone", false, "VarChar", 25, 0, 0, false, "", null, typeof(System.String), 11);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "FromEmail", "FromEmail", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 12);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "FromAccountNumber", "FromAccountNumber", false, "VarChar", 10, 0, 0, false, "", null, typeof(System.String), 13);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ToCustomerID", "ToCustomerID", false, "NVarChar", 30, 0, 0, false, "", null, typeof(System.String), 14);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ToCompanyOrName", "ToCompanyOrName", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 15);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ToAttention", "ToAttention", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 16);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ToAddress1", "ToAddress1", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 17);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ToAddress2", "ToAddress2", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 18);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ToAddress3", "ToAddress3", false, "NVarChar", 60, 0, 0, false, "", null, typeof(System.String), 19);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ToCountryCode", "ToCountryCode", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 20);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ToPostalCode", "ToPostalCode", false, "VarChar", 20, 0, 0, false, "", null, typeof(System.String), 21);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ToCity", "ToCity", false, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 22);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ToStateProvCode", "ToStateProvCode", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 23);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ToTelephone", "ToTelephone", false, "VarChar", 25, 0, 0, false, "", null, typeof(System.String), 24);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ToEmail", "ToEmail", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 25);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ToAccountNumber", "ToAccountNumber", false, "VarChar", 10, 0, 0, false, "", null, typeof(System.String), 26);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ToResidential", "ToResidential", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 27);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ServiceType", "ServiceType", false, "VarChar", 3, 0, 0, false, "", null, typeof(System.String), 28);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "BillTransportationTo", "BillTransportationTo", false, "VarChar", 20, 0, 0, false, "", null, typeof(System.String), 29);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "SaturdayDelivery", "SaturdayDelivery", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 30);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "QvnOption", "QvnOption", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 31);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "QvnFrom", "QvnFrom", true, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 32);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "QvnSubjectLine", "QvnSubjectLine", true, "NVarChar", 18, 0, 0, false, "", null, typeof(System.String), 33);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "QvnMemo", "QvnMemo", true, "NVarChar", 150, 0, 0, false, "", null, typeof(System.String), 34);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "Qvn1ShipNotify", "Qvn1ShipNotify", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 35);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "Qvn1DeliveryNotify", "Qvn1DeliveryNotify", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 36);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "Qvn1ExceptionNotify", "Qvn1ExceptionNotify", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 37);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "Qvn1ContactName", "Qvn1ContactName", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 38);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "Qvn1Email", "Qvn1Email", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 39);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "Qvn2ShipNotify", "Qvn2ShipNotify", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 40);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "Qvn2DeliveryNotify", "Qvn2DeliveryNotify", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 41);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "Qvn2ExceptionNotify", "Qvn2ExceptionNotify", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 42);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "Qvn2ContactName", "Qvn2ContactName", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 43);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "Qvn2Email", "Qvn2Email", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 44);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "Qvn3ShipNotify", "Qvn3ShipNotify", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 45);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "Qvn3DeliveryNotify", "Qvn3DeliveryNotify", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 46);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "Qvn3ExceptionNotify", "Qvn3ExceptionNotify", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 47);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "Qvn3ContactName", "Qvn3ContactName", false, "NVarChar", 35, 0, 0, false, "", null, typeof(System.String), 48);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "Qvn3Email", "Qvn3Email", false, "NVarChar", 100, 0, 0, false, "", null, typeof(System.String), 49);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "CustomsDescriptionOfGoods", "CustomsDescriptionOfGoods", true, "NVarChar", 150, 0, 0, false, "", null, typeof(System.String), 50);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "CustomsDocumentsOnly", "CustomsDocumentsOnly", true, "Char", 1, 0, 0, false, "", null, typeof(System.String), 51);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ShipperNumber", "ShipperNumber", false, "VarChar", 10, 0, 0, false, "", null, typeof(System.String), 52);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "PackageCount", "PackageCount", false, "Int", 0, 10, 0, false, "", null, typeof(System.Int32), 53);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "DeliveryConfirmation", "DeliveryConfirmation", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 54);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "DeliveryConfirmationAdult", "DeliveryConfirmationAdult", false, "Char", 1, 0, 0, false, "", null, typeof(System.String), 55);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "InvoiceTermsOfSale", "InvoiceTermsOfSale", true, "VarChar", 3, 0, 0, false, "", null, typeof(System.String), 56);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "InvoiceReasonForExport", "InvoiceReasonForExport", true, "VarChar", 2, 0, 0, false, "", null, typeof(System.String), 57);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "InvoiceComments", "InvoiceComments", true, "NVarChar", 200, 0, 0, false, "", null, typeof(System.String), 58);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "InvoiceCurrencyCode", "InvoiceCurrencyCode", true, "VarChar", 3, 0, 0, false, "", null, typeof(System.String), 59);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "InvoiceChargesFreight", "InvoiceChargesFreight", true, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 60);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "InvoiceChargesInsurance", "InvoiceChargesInsurance", true, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 61);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "InvoiceChargesOther", "InvoiceChargesOther", true, "Money", 0, 19, 4, false, "", null, typeof(System.Decimal), 62);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "ShipmentProcessedOnComputerID", "ShipmentProcessedOnComputerID", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 63);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "UspsEndorsement", "UspsEndorsement", true, "NVarChar", 50, 0, 0, false, "", null, typeof(System.String), 64);
			this.AddElementFieldMapping("WorldShipShipmentEntity", "CarbonNeutral", "CarbonNeutral", true, "Char", 10, 0, 0, false, "", null, typeof(System.String), 65);
		}

		/// <summary>Inits YahooOrderEntity's mappings</summary>
		private void InitYahooOrderEntityMappings()
		{
			this.AddElementMapping("YahooOrderEntity", @"ShipWorksLocal", @"dbo", "YahooOrder", 2, 0);
			this.AddElementFieldMapping("YahooOrderEntity", "OrderID", "OrderID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("YahooOrderEntity", "YahooOrderID", "YahooOrderID", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 1);
		}

		/// <summary>Inits YahooOrderItemEntity's mappings</summary>
		private void InitYahooOrderItemEntityMappings()
		{
			this.AddElementMapping("YahooOrderItemEntity", @"ShipWorksLocal", @"dbo", "YahooOrderItem", 3, 0);
			this.AddElementFieldMapping("YahooOrderItemEntity", "OrderItemID", "OrderItemID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("YahooOrderItemEntity", "YahooProductID", "YahooProductID", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("YahooOrderItemEntity", "Url", "Url", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 2);
		}

		/// <summary>Inits YahooProductEntity's mappings</summary>
		private void InitYahooProductEntityMappings()
		{
			this.AddElementMapping("YahooProductEntity", @"ShipWorksLocal", @"dbo", "YahooProduct", 3, 0);
			this.AddElementFieldMapping("YahooProductEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("YahooProductEntity", "YahooProductID", "YahooProductID", false, "NVarChar", 255, 0, 0, false, "", null, typeof(System.String), 1);
			this.AddElementFieldMapping("YahooProductEntity", "Weight", "Weight", false, "Float", 0, 38, 0, false, "", null, typeof(System.Double), 2);
		}

		/// <summary>Inits YahooStoreEntity's mappings</summary>
		private void InitYahooStoreEntityMappings()
		{
			this.AddElementMapping("YahooStoreEntity", @"ShipWorksLocal", @"dbo", "YahooStore", 6, 0);
			this.AddElementFieldMapping("YahooStoreEntity", "StoreID", "StoreID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 0);
			this.AddElementFieldMapping("YahooStoreEntity", "YahooEmailAccountID", "YahooEmailAccountID", false, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 1);
			this.AddElementFieldMapping("YahooStoreEntity", "TrackingUpdatePassword", "TrackingUpdatePassword", false, "VarChar", 100, 0, 0, false, "", null, typeof(System.String), 2);
			this.AddElementFieldMapping("YahooStoreEntity", "YahooStoreID", "YahooStoreID", false, "VarChar", 50, 0, 0, false, "", null, typeof(System.String), 3);
			this.AddElementFieldMapping("YahooStoreEntity", "AccessToken", "AccessToken", false, "VarChar", 200, 0, 0, false, "", null, typeof(System.String), 4);
			this.AddElementFieldMapping("YahooStoreEntity", "BackupOrderNumber", "BackupOrderNumber", true, "BigInt", 0, 19, 0, false, "", null, typeof(System.Int64), 5);
		}

	}
}
