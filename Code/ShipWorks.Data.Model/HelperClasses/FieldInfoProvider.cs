///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 5.0
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.HelperClasses
{
	
	// __LLBLGENPRO_USER_CODE_REGION_START AdditionalNamespaces
	// __LLBLGENPRO_USER_CODE_REGION_END
	
	/// <summary>Singleton implementation of the FieldInfoProvider. This class is the singleton wrapper through which the actual instance is retrieved.</summary>
	/// <remarks>It uses a single instance of an internal class. The access isn't marked with locks as the FieldInfoProviderBase class is threadsafe.</remarks>
	internal static class FieldInfoProviderSingleton
	{
		#region Class Member Declarations
		private static readonly IFieldInfoProvider _providerInstance = new FieldInfoProviderCore();
		#endregion

		/// <summary>Dummy static constructor to make sure threadsafe initialization is performed.</summary>
		static FieldInfoProviderSingleton()
		{
		}

		/// <summary>Gets the singleton instance of the FieldInfoProviderCore</summary>
		/// <returns>Instance of the FieldInfoProvider.</returns>
		public static IFieldInfoProvider GetInstance()
		{
			return _providerInstance;
		}
	}

	/// <summary>Actual implementation of the FieldInfoProvider. Used by singleton wrapper.</summary>
	internal class FieldInfoProviderCore : FieldInfoProviderBase
	{
		/// <summary>Initializes a new instance of the <see cref="FieldInfoProviderCore"/> class.</summary>
		internal FieldInfoProviderCore()
		{
			Init();
		}

		/// <summary>Method which initializes the internal datastores.</summary>
		private void Init()
		{
			this.InitClass( (178 + 0));
			InitActionEntityInfos();
			InitActionFilterTriggerEntityInfos();
			InitActionQueueEntityInfos();
			InitActionQueueSelectionEntityInfos();
			InitActionQueueStepEntityInfos();
			InitActionTaskEntityInfos();
			InitAmazonASINEntityInfos();
			InitAmazonOrderEntityInfos();
			InitAmazonOrderItemEntityInfos();
			InitAmazonProfileEntityInfos();
			InitAmazonShipmentEntityInfos();
			InitAmazonStoreEntityInfos();
			InitAmeriCommerceStoreEntityInfos();
			InitAuditEntityInfos();
			InitAuditChangeEntityInfos();
			InitAuditChangeDetailEntityInfos();
			InitBestRateProfileEntityInfos();
			InitBestRateShipmentEntityInfos();
			InitBigCommerceOrderItemEntityInfos();
			InitBigCommerceStoreEntityInfos();
			InitBuyDotComOrderItemEntityInfos();
			InitBuyDotComStoreEntityInfos();
			InitChannelAdvisorOrderEntityInfos();
			InitChannelAdvisorOrderItemEntityInfos();
			InitChannelAdvisorStoreEntityInfos();
			InitClickCartProOrderEntityInfos();
			InitCommerceInterfaceOrderEntityInfos();
			InitComputerEntityInfos();
			InitConfigurationEntityInfos();
			InitCustomerEntityInfos();
			InitDimensionsProfileEntityInfos();
			InitDownloadEntityInfos();
			InitDownloadDetailEntityInfos();
			InitEbayCombinedOrderRelationEntityInfos();
			InitEbayOrderEntityInfos();
			InitEbayOrderItemEntityInfos();
			InitEbayStoreEntityInfos();
			InitEmailAccountEntityInfos();
			InitEmailOutboundEntityInfos();
			InitEmailOutboundRelationEntityInfos();
			InitEndiciaAccountEntityInfos();
			InitEndiciaProfileEntityInfos();
			InitEndiciaScanFormEntityInfos();
			InitEndiciaShipmentEntityInfos();
			InitEtsyOrderEntityInfos();
			InitEtsyStoreEntityInfos();
			InitExcludedPackageTypeEntityInfos();
			InitExcludedServiceTypeEntityInfos();
			InitFedExAccountEntityInfos();
			InitFedExEndOfDayCloseEntityInfos();
			InitFedExPackageEntityInfos();
			InitFedExProfileEntityInfos();
			InitFedExProfilePackageEntityInfos();
			InitFedExShipmentEntityInfos();
			InitFilterEntityInfos();
			InitFilterLayoutEntityInfos();
			InitFilterNodeEntityInfos();
			InitFilterNodeColumnSettingsEntityInfos();
			InitFilterNodeContentEntityInfos();
			InitFilterNodeContentDetailEntityInfos();
			InitFilterSequenceEntityInfos();
			InitFtpAccountEntityInfos();
			InitGenericFileStoreEntityInfos();
			InitGenericModuleStoreEntityInfos();
			InitGridColumnFormatEntityInfos();
			InitGridColumnLayoutEntityInfos();
			InitGridColumnPositionEntityInfos();
			InitGrouponOrderEntityInfos();
			InitGrouponOrderItemEntityInfos();
			InitGrouponStoreEntityInfos();
			InitInfopiaOrderItemEntityInfos();
			InitInfopiaStoreEntityInfos();
			InitInsurancePolicyEntityInfos();
			InitIParcelAccountEntityInfos();
			InitIParcelPackageEntityInfos();
			InitIParcelProfileEntityInfos();
			InitIParcelProfilePackageEntityInfos();
			InitIParcelShipmentEntityInfos();
			InitLabelSheetEntityInfos();
			InitLemonStandOrderEntityInfos();
			InitLemonStandOrderItemEntityInfos();
			InitLemonStandStoreEntityInfos();
			InitMagentoOrderEntityInfos();
			InitMagentoStoreEntityInfos();
			InitMarketplaceAdvisorOrderEntityInfos();
			InitMarketplaceAdvisorStoreEntityInfos();
			InitMivaOrderItemAttributeEntityInfos();
			InitMivaStoreEntityInfos();
			InitNetworkSolutionsOrderEntityInfos();
			InitNetworkSolutionsStoreEntityInfos();
			InitNeweggOrderEntityInfos();
			InitNeweggOrderItemEntityInfos();
			InitNeweggStoreEntityInfos();
			InitNoteEntityInfos();
			InitObjectLabelEntityInfos();
			InitObjectReferenceEntityInfos();
			InitOdbcStoreEntityInfos();
			InitOnTracAccountEntityInfos();
			InitOnTracProfileEntityInfos();
			InitOnTracShipmentEntityInfos();
			InitOrderEntityInfos();
			InitOrderChargeEntityInfos();
			InitOrderItemEntityInfos();
			InitOrderItemAttributeEntityInfos();
			InitOrderMotionOrderEntityInfos();
			InitOrderMotionStoreEntityInfos();
			InitOrderPaymentDetailEntityInfos();
			InitOtherProfileEntityInfos();
			InitOtherShipmentEntityInfos();
			InitPayPalOrderEntityInfos();
			InitPayPalStoreEntityInfos();
			InitPermissionEntityInfos();
			InitPostalProfileEntityInfos();
			InitPostalShipmentEntityInfos();
			InitPrintResultEntityInfos();
			InitProStoresOrderEntityInfos();
			InitProStoresStoreEntityInfos();
			InitResourceEntityInfos();
			InitScanFormBatchEntityInfos();
			InitSearchEntityInfos();
			InitSearsOrderEntityInfos();
			InitSearsOrderItemEntityInfos();
			InitSearsStoreEntityInfos();
			InitServerMessageEntityInfos();
			InitServerMessageSignoffEntityInfos();
			InitServiceStatusEntityInfos();
			InitShipmentEntityInfos();
			InitShipmentCustomsItemEntityInfos();
			InitShippingDefaultsRuleEntityInfos();
			InitShippingOriginEntityInfos();
			InitShippingPrintOutputEntityInfos();
			InitShippingPrintOutputRuleEntityInfos();
			InitShippingProfileEntityInfos();
			InitShippingProviderRuleEntityInfos();
			InitShippingSettingsEntityInfos();
			InitShipSenseKnowledgebaseEntityInfos();
			InitShopifyOrderEntityInfos();
			InitShopifyOrderItemEntityInfos();
			InitShopifyStoreEntityInfos();
			InitShopSiteStoreEntityInfos();
			InitSparkPayStoreEntityInfos();
			InitStatusPresetEntityInfos();
			InitStoreEntityInfos();
			InitSystemDataEntityInfos();
			InitTemplateEntityInfos();
			InitTemplateComputerSettingsEntityInfos();
			InitTemplateFolderEntityInfos();
			InitTemplateStoreSettingsEntityInfos();
			InitTemplateUserSettingsEntityInfos();
			InitThreeDCartOrderEntityInfos();
			InitThreeDCartOrderItemEntityInfos();
			InitThreeDCartStoreEntityInfos();
			InitUpsAccountEntityInfos();
			InitUpsPackageEntityInfos();
			InitUpsProfileEntityInfos();
			InitUpsProfilePackageEntityInfos();
			InitUpsShipmentEntityInfos();
			InitUserEntityInfos();
			InitUserColumnSettingsEntityInfos();
			InitUserSettingsEntityInfos();
			InitUspsAccountEntityInfos();
			InitUspsProfileEntityInfos();
			InitUspsScanFormEntityInfos();
			InitUspsShipmentEntityInfos();
			InitValidatedAddressEntityInfos();
			InitVersionSignoffEntityInfos();
			InitVolusionStoreEntityInfos();
			InitWalmartOrderEntityInfos();
			InitWalmartOrderItemEntityInfos();
			InitWalmartStoreEntityInfos();
			InitWorldShipGoodsEntityInfos();
			InitWorldShipPackageEntityInfos();
			InitWorldShipProcessedEntityInfos();
			InitWorldShipShipmentEntityInfos();
			InitYahooOrderEntityInfos();
			InitYahooOrderItemEntityInfos();
			InitYahooProductEntityInfos();
			InitYahooStoreEntityInfos();

			this.ConstructElementFieldStructures(InheritanceInfoProviderSingleton.GetInstance());
		}

		/// <summary>Inits ActionEntity's FieldInfo objects</summary>
		private void InitActionEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ActionFieldIndex), "ActionEntity");
			this.AddElementFieldInfo("ActionEntity", "ActionID", typeof(System.Int64), true, false, true, false,  (int)ActionFieldIndex.ActionID, 0, 0, 19);
			this.AddElementFieldInfo("ActionEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ActionFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("ActionEntity", "Name", typeof(System.String), false, false, false, false,  (int)ActionFieldIndex.Name, 50, 0, 0);
			this.AddElementFieldInfo("ActionEntity", "Enabled", typeof(System.Boolean), false, false, false, false,  (int)ActionFieldIndex.Enabled, 0, 0, 0);
			this.AddElementFieldInfo("ActionEntity", "ComputerLimitedType", typeof(System.Int32), false, false, false, false,  (int)ActionFieldIndex.ComputerLimitedType, 0, 0, 10);
			this.AddElementFieldInfo("ActionEntity", "InternalComputerLimitedList", typeof(System.String), false, false, false, false,  (int)ActionFieldIndex.InternalComputerLimitedList, 150, 0, 0);
			this.AddElementFieldInfo("ActionEntity", "StoreLimited", typeof(System.Boolean), false, false, false, false,  (int)ActionFieldIndex.StoreLimited, 0, 0, 0);
			this.AddElementFieldInfo("ActionEntity", "InternalStoreLimitedList", typeof(System.String), false, false, false, false,  (int)ActionFieldIndex.InternalStoreLimitedList, 150, 0, 0);
			this.AddElementFieldInfo("ActionEntity", "TriggerType", typeof(System.Int32), false, false, false, false,  (int)ActionFieldIndex.TriggerType, 0, 0, 10);
			this.AddElementFieldInfo("ActionEntity", "TriggerSettings", typeof(System.String), false, false, false, false,  (int)ActionFieldIndex.TriggerSettings, 2147483647, 0, 0);
			this.AddElementFieldInfo("ActionEntity", "TaskSummary", typeof(System.String), false, false, false, false,  (int)ActionFieldIndex.TaskSummary, 150, 0, 0);
			this.AddElementFieldInfo("ActionEntity", "InternalOwner", typeof(System.String), false, false, false, true,  (int)ActionFieldIndex.InternalOwner, 50, 0, 0);
		}
		/// <summary>Inits ActionFilterTriggerEntity's FieldInfo objects</summary>
		private void InitActionFilterTriggerEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ActionFilterTriggerFieldIndex), "ActionFilterTriggerEntity");
			this.AddElementFieldInfo("ActionFilterTriggerEntity", "ActionID", typeof(System.Int64), true, true, false, false,  (int)ActionFilterTriggerFieldIndex.ActionID, 0, 0, 19);
			this.AddElementFieldInfo("ActionFilterTriggerEntity", "FilterNodeID", typeof(System.Int64), false, false, false, false,  (int)ActionFilterTriggerFieldIndex.FilterNodeID, 0, 0, 19);
			this.AddElementFieldInfo("ActionFilterTriggerEntity", "Direction", typeof(System.Int32), false, false, false, false,  (int)ActionFilterTriggerFieldIndex.Direction, 0, 0, 10);
			this.AddElementFieldInfo("ActionFilterTriggerEntity", "ComputerLimitedType", typeof(System.Int32), false, false, false, false,  (int)ActionFilterTriggerFieldIndex.ComputerLimitedType, 0, 0, 10);
			this.AddElementFieldInfo("ActionFilterTriggerEntity", "InternalComputerLimitedList", typeof(System.String), false, false, false, false,  (int)ActionFilterTriggerFieldIndex.InternalComputerLimitedList, 150, 0, 0);
		}
		/// <summary>Inits ActionQueueEntity's FieldInfo objects</summary>
		private void InitActionQueueEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ActionQueueFieldIndex), "ActionQueueEntity");
			this.AddElementFieldInfo("ActionQueueEntity", "ActionQueueID", typeof(System.Int64), true, false, true, false,  (int)ActionQueueFieldIndex.ActionQueueID, 0, 0, 19);
			this.AddElementFieldInfo("ActionQueueEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ActionQueueFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("ActionQueueEntity", "ActionID", typeof(System.Int64), false, true, false, false,  (int)ActionQueueFieldIndex.ActionID, 0, 0, 19);
			this.AddElementFieldInfo("ActionQueueEntity", "ActionName", typeof(System.String), false, false, false, false,  (int)ActionQueueFieldIndex.ActionName, 50, 0, 0);
			this.AddElementFieldInfo("ActionQueueEntity", "ActionQueueType", typeof(System.Int32), false, false, false, false,  (int)ActionQueueFieldIndex.ActionQueueType, 0, 0, 10);
			this.AddElementFieldInfo("ActionQueueEntity", "ActionVersion", typeof(System.Byte[]), false, false, false, false,  (int)ActionQueueFieldIndex.ActionVersion, 8, 0, 0);
			this.AddElementFieldInfo("ActionQueueEntity", "QueueVersion", typeof(System.Byte[]), false, false, true, false,  (int)ActionQueueFieldIndex.QueueVersion, 8, 0, 0);
			this.AddElementFieldInfo("ActionQueueEntity", "TriggerDate", typeof(System.DateTime), false, false, false, false,  (int)ActionQueueFieldIndex.TriggerDate, 0, 0, 0);
			this.AddElementFieldInfo("ActionQueueEntity", "TriggerComputerID", typeof(System.Int64), false, true, false, false,  (int)ActionQueueFieldIndex.TriggerComputerID, 0, 0, 19);
			this.AddElementFieldInfo("ActionQueueEntity", "InternalComputerLimitedList", typeof(System.String), false, false, false, false,  (int)ActionQueueFieldIndex.InternalComputerLimitedList, 150, 0, 0);
			this.AddElementFieldInfo("ActionQueueEntity", "EntityID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)ActionQueueFieldIndex.EntityID, 0, 0, 19);
			this.AddElementFieldInfo("ActionQueueEntity", "Status", typeof(System.Int32), false, false, false, false,  (int)ActionQueueFieldIndex.Status, 0, 0, 10);
			this.AddElementFieldInfo("ActionQueueEntity", "NextStep", typeof(System.Int32), false, false, false, false,  (int)ActionQueueFieldIndex.NextStep, 0, 0, 10);
			this.AddElementFieldInfo("ActionQueueEntity", "ContextLock", typeof(System.String), false, false, false, true,  (int)ActionQueueFieldIndex.ContextLock, 36, 0, 0);
			this.AddElementFieldInfo("ActionQueueEntity", "ExtraData", typeof(System.String), false, false, false, true,  (int)ActionQueueFieldIndex.ExtraData, 2147483647, 0, 0);
		}
		/// <summary>Inits ActionQueueSelectionEntity's FieldInfo objects</summary>
		private void InitActionQueueSelectionEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ActionQueueSelectionFieldIndex), "ActionQueueSelectionEntity");
			this.AddElementFieldInfo("ActionQueueSelectionEntity", "ActionQueueSelectionID", typeof(System.Int64), true, false, true, false,  (int)ActionQueueSelectionFieldIndex.ActionQueueSelectionID, 0, 0, 19);
			this.AddElementFieldInfo("ActionQueueSelectionEntity", "ActionQueueID", typeof(System.Int64), false, true, false, false,  (int)ActionQueueSelectionFieldIndex.ActionQueueID, 0, 0, 19);
			this.AddElementFieldInfo("ActionQueueSelectionEntity", "EntityID", typeof(System.Int64), false, false, false, false,  (int)ActionQueueSelectionFieldIndex.EntityID, 0, 0, 19);
		}
		/// <summary>Inits ActionQueueStepEntity's FieldInfo objects</summary>
		private void InitActionQueueStepEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ActionQueueStepFieldIndex), "ActionQueueStepEntity");
			this.AddElementFieldInfo("ActionQueueStepEntity", "ActionQueueStepID", typeof(System.Int64), true, false, true, false,  (int)ActionQueueStepFieldIndex.ActionQueueStepID, 0, 0, 19);
			this.AddElementFieldInfo("ActionQueueStepEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ActionQueueStepFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("ActionQueueStepEntity", "ActionQueueID", typeof(System.Int64), false, true, false, false,  (int)ActionQueueStepFieldIndex.ActionQueueID, 0, 0, 19);
			this.AddElementFieldInfo("ActionQueueStepEntity", "StepStatus", typeof(System.Int32), false, false, false, false,  (int)ActionQueueStepFieldIndex.StepStatus, 0, 0, 10);
			this.AddElementFieldInfo("ActionQueueStepEntity", "StepIndex", typeof(System.Int32), false, false, false, false,  (int)ActionQueueStepFieldIndex.StepIndex, 0, 0, 10);
			this.AddElementFieldInfo("ActionQueueStepEntity", "StepName", typeof(System.String), false, false, false, false,  (int)ActionQueueStepFieldIndex.StepName, 100, 0, 0);
			this.AddElementFieldInfo("ActionQueueStepEntity", "TaskIdentifier", typeof(System.String), false, false, false, false,  (int)ActionQueueStepFieldIndex.TaskIdentifier, 50, 0, 0);
			this.AddElementFieldInfo("ActionQueueStepEntity", "TaskSettings", typeof(System.String), false, false, false, false,  (int)ActionQueueStepFieldIndex.TaskSettings, 2147483647, 0, 0);
			this.AddElementFieldInfo("ActionQueueStepEntity", "InputSource", typeof(System.Int32), false, false, false, false,  (int)ActionQueueStepFieldIndex.InputSource, 0, 0, 10);
			this.AddElementFieldInfo("ActionQueueStepEntity", "InputFilterNodeID", typeof(System.Int64), false, false, false, false,  (int)ActionQueueStepFieldIndex.InputFilterNodeID, 0, 0, 19);
			this.AddElementFieldInfo("ActionQueueStepEntity", "FilterCondition", typeof(System.Boolean), false, false, false, false,  (int)ActionQueueStepFieldIndex.FilterCondition, 0, 0, 0);
			this.AddElementFieldInfo("ActionQueueStepEntity", "FilterConditionNodeID", typeof(System.Int64), false, false, false, false,  (int)ActionQueueStepFieldIndex.FilterConditionNodeID, 0, 0, 19);
			this.AddElementFieldInfo("ActionQueueStepEntity", "FlowSuccess", typeof(System.Int32), false, false, false, false,  (int)ActionQueueStepFieldIndex.FlowSuccess, 0, 0, 10);
			this.AddElementFieldInfo("ActionQueueStepEntity", "FlowSkipped", typeof(System.Int32), false, false, false, false,  (int)ActionQueueStepFieldIndex.FlowSkipped, 0, 0, 10);
			this.AddElementFieldInfo("ActionQueueStepEntity", "FlowError", typeof(System.Int32), false, false, false, false,  (int)ActionQueueStepFieldIndex.FlowError, 0, 0, 10);
			this.AddElementFieldInfo("ActionQueueStepEntity", "AttemptDate", typeof(System.DateTime), false, false, false, false,  (int)ActionQueueStepFieldIndex.AttemptDate, 0, 0, 0);
			this.AddElementFieldInfo("ActionQueueStepEntity", "AttemptError", typeof(System.String), false, false, false, false,  (int)ActionQueueStepFieldIndex.AttemptError, 500, 0, 0);
			this.AddElementFieldInfo("ActionQueueStepEntity", "AttemptCount", typeof(System.Int32), false, false, false, false,  (int)ActionQueueStepFieldIndex.AttemptCount, 0, 0, 10);
		}
		/// <summary>Inits ActionTaskEntity's FieldInfo objects</summary>
		private void InitActionTaskEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ActionTaskFieldIndex), "ActionTaskEntity");
			this.AddElementFieldInfo("ActionTaskEntity", "ActionTaskID", typeof(System.Int64), true, false, true, false,  (int)ActionTaskFieldIndex.ActionTaskID, 0, 0, 19);
			this.AddElementFieldInfo("ActionTaskEntity", "ActionID", typeof(System.Int64), false, true, false, false,  (int)ActionTaskFieldIndex.ActionID, 0, 0, 19);
			this.AddElementFieldInfo("ActionTaskEntity", "TaskIdentifier", typeof(System.String), false, false, false, false,  (int)ActionTaskFieldIndex.TaskIdentifier, 50, 0, 0);
			this.AddElementFieldInfo("ActionTaskEntity", "TaskSettings", typeof(System.String), false, false, false, false,  (int)ActionTaskFieldIndex.TaskSettings, 2147483647, 0, 0);
			this.AddElementFieldInfo("ActionTaskEntity", "StepIndex", typeof(System.Int32), false, false, false, false,  (int)ActionTaskFieldIndex.StepIndex, 0, 0, 10);
			this.AddElementFieldInfo("ActionTaskEntity", "InputSource", typeof(System.Int32), false, false, false, false,  (int)ActionTaskFieldIndex.InputSource, 0, 0, 10);
			this.AddElementFieldInfo("ActionTaskEntity", "InputFilterNodeID", typeof(System.Int64), false, false, false, false,  (int)ActionTaskFieldIndex.InputFilterNodeID, 0, 0, 19);
			this.AddElementFieldInfo("ActionTaskEntity", "FilterCondition", typeof(System.Boolean), false, false, false, false,  (int)ActionTaskFieldIndex.FilterCondition, 0, 0, 0);
			this.AddElementFieldInfo("ActionTaskEntity", "FilterConditionNodeID", typeof(System.Int64), false, false, false, false,  (int)ActionTaskFieldIndex.FilterConditionNodeID, 0, 0, 19);
			this.AddElementFieldInfo("ActionTaskEntity", "FlowSuccess", typeof(System.Int32), false, false, false, false,  (int)ActionTaskFieldIndex.FlowSuccess, 0, 0, 10);
			this.AddElementFieldInfo("ActionTaskEntity", "FlowSkipped", typeof(System.Int32), false, false, false, false,  (int)ActionTaskFieldIndex.FlowSkipped, 0, 0, 10);
			this.AddElementFieldInfo("ActionTaskEntity", "FlowError", typeof(System.Int32), false, false, false, false,  (int)ActionTaskFieldIndex.FlowError, 0, 0, 10);
		}
		/// <summary>Inits AmazonASINEntity's FieldInfo objects</summary>
		private void InitAmazonASINEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(AmazonASINFieldIndex), "AmazonASINEntity");
			this.AddElementFieldInfo("AmazonASINEntity", "StoreID", typeof(System.Int64), true, true, false, false,  (int)AmazonASINFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("AmazonASINEntity", "SKU", typeof(System.String), true, false, false, false,  (int)AmazonASINFieldIndex.SKU, 100, 0, 0);
			this.AddElementFieldInfo("AmazonASINEntity", "AmazonASIN", typeof(System.String), false, false, false, false,  (int)AmazonASINFieldIndex.AmazonASIN, 32, 0, 0);
		}
		/// <summary>Inits AmazonOrderEntity's FieldInfo objects</summary>
		private void InitAmazonOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(AmazonOrderFieldIndex), "AmazonOrderEntity");
			this.AddElementFieldInfo("AmazonOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)AmazonOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("AmazonOrderEntity", "AmazonOrderID", typeof(System.String), false, false, false, false,  (int)AmazonOrderFieldIndex.AmazonOrderID, 32, 0, 0);
			this.AddElementFieldInfo("AmazonOrderEntity", "AmazonCommission", typeof(System.Decimal), false, false, false, false,  (int)AmazonOrderFieldIndex.AmazonCommission, 0, 4, 19);
			this.AddElementFieldInfo("AmazonOrderEntity", "FulfillmentChannel", typeof(System.Int32), false, false, false, false,  (int)AmazonOrderFieldIndex.FulfillmentChannel, 0, 0, 10);
			this.AddElementFieldInfo("AmazonOrderEntity", "IsPrime", typeof(System.Int32), false, false, false, false,  (int)AmazonOrderFieldIndex.IsPrime, 0, 0, 10);
			this.AddElementFieldInfo("AmazonOrderEntity", "EarliestExpectedDeliveryDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)AmazonOrderFieldIndex.EarliestExpectedDeliveryDate, 0, 0, 0);
			this.AddElementFieldInfo("AmazonOrderEntity", "LatestExpectedDeliveryDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)AmazonOrderFieldIndex.LatestExpectedDeliveryDate, 0, 0, 0);
			this.AddElementFieldInfo("AmazonOrderEntity", "PurchaseOrderNumber", typeof(System.String), false, false, false, false,  (int)AmazonOrderFieldIndex.PurchaseOrderNumber, 50, 0, 0);
		}
		/// <summary>Inits AmazonOrderItemEntity's FieldInfo objects</summary>
		private void InitAmazonOrderItemEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(AmazonOrderItemFieldIndex), "AmazonOrderItemEntity");
			this.AddElementFieldInfo("AmazonOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, true, false,  (int)AmazonOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("AmazonOrderItemEntity", "AmazonOrderItemCode", typeof(System.String), false, false, false, false,  (int)AmazonOrderItemFieldIndex.AmazonOrderItemCode, 64, 0, 0);
			this.AddElementFieldInfo("AmazonOrderItemEntity", "ASIN", typeof(System.String), false, false, false, false,  (int)AmazonOrderItemFieldIndex.ASIN, 255, 0, 0);
			this.AddElementFieldInfo("AmazonOrderItemEntity", "ConditionNote", typeof(System.String), false, false, false, false,  (int)AmazonOrderItemFieldIndex.ConditionNote, 255, 0, 0);
		}
		/// <summary>Inits AmazonProfileEntity's FieldInfo objects</summary>
		private void InitAmazonProfileEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(AmazonProfileFieldIndex), "AmazonProfileEntity");
			this.AddElementFieldInfo("AmazonProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)AmazonProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			this.AddElementFieldInfo("AmazonProfileEntity", "DimsProfileID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)AmazonProfileFieldIndex.DimsProfileID, 0, 0, 19);
			this.AddElementFieldInfo("AmazonProfileEntity", "DimsLength", typeof(Nullable<System.Double>), false, false, false, true,  (int)AmazonProfileFieldIndex.DimsLength, 0, 0, 38);
			this.AddElementFieldInfo("AmazonProfileEntity", "DimsWidth", typeof(Nullable<System.Double>), false, false, false, true,  (int)AmazonProfileFieldIndex.DimsWidth, 0, 0, 38);
			this.AddElementFieldInfo("AmazonProfileEntity", "DimsHeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)AmazonProfileFieldIndex.DimsHeight, 0, 0, 38);
			this.AddElementFieldInfo("AmazonProfileEntity", "DimsWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)AmazonProfileFieldIndex.DimsWeight, 0, 0, 38);
			this.AddElementFieldInfo("AmazonProfileEntity", "DimsAddWeight", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)AmazonProfileFieldIndex.DimsAddWeight, 0, 0, 0);
			this.AddElementFieldInfo("AmazonProfileEntity", "DeliveryExperience", typeof(Nullable<System.Int32>), false, false, false, true,  (int)AmazonProfileFieldIndex.DeliveryExperience, 0, 0, 10);
			this.AddElementFieldInfo("AmazonProfileEntity", "Weight", typeof(Nullable<System.Double>), false, false, false, true,  (int)AmazonProfileFieldIndex.Weight, 0, 0, 38);
		}
		/// <summary>Inits AmazonShipmentEntity's FieldInfo objects</summary>
		private void InitAmazonShipmentEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(AmazonShipmentFieldIndex), "AmazonShipmentEntity");
			this.AddElementFieldInfo("AmazonShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)AmazonShipmentFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("AmazonShipmentEntity", "CarrierName", typeof(System.String), false, false, false, false,  (int)AmazonShipmentFieldIndex.CarrierName, 50, 0, 0);
			this.AddElementFieldInfo("AmazonShipmentEntity", "ShippingServiceName", typeof(System.String), false, false, false, false,  (int)AmazonShipmentFieldIndex.ShippingServiceName, 50, 0, 0);
			this.AddElementFieldInfo("AmazonShipmentEntity", "ShippingServiceID", typeof(System.String), false, false, false, false,  (int)AmazonShipmentFieldIndex.ShippingServiceID, 50, 0, 0);
			this.AddElementFieldInfo("AmazonShipmentEntity", "ShippingServiceOfferID", typeof(System.String), false, false, false, false,  (int)AmazonShipmentFieldIndex.ShippingServiceOfferID, 250, 0, 0);
			this.AddElementFieldInfo("AmazonShipmentEntity", "InsuranceValue", typeof(System.Decimal), false, false, false, false,  (int)AmazonShipmentFieldIndex.InsuranceValue, 0, 4, 19);
			this.AddElementFieldInfo("AmazonShipmentEntity", "DimsProfileID", typeof(System.Int64), false, false, false, false,  (int)AmazonShipmentFieldIndex.DimsProfileID, 0, 0, 19);
			this.AddElementFieldInfo("AmazonShipmentEntity", "DimsLength", typeof(System.Double), false, false, false, false,  (int)AmazonShipmentFieldIndex.DimsLength, 0, 0, 38);
			this.AddElementFieldInfo("AmazonShipmentEntity", "DimsWidth", typeof(System.Double), false, false, false, false,  (int)AmazonShipmentFieldIndex.DimsWidth, 0, 0, 38);
			this.AddElementFieldInfo("AmazonShipmentEntity", "DimsHeight", typeof(System.Double), false, false, false, false,  (int)AmazonShipmentFieldIndex.DimsHeight, 0, 0, 38);
			this.AddElementFieldInfo("AmazonShipmentEntity", "DimsWeight", typeof(System.Double), false, false, false, false,  (int)AmazonShipmentFieldIndex.DimsWeight, 0, 0, 38);
			this.AddElementFieldInfo("AmazonShipmentEntity", "DimsAddWeight", typeof(System.Boolean), false, false, false, false,  (int)AmazonShipmentFieldIndex.DimsAddWeight, 0, 0, 0);
			this.AddElementFieldInfo("AmazonShipmentEntity", "DeliveryExperience", typeof(System.Int32), false, false, false, false,  (int)AmazonShipmentFieldIndex.DeliveryExperience, 0, 0, 10);
			this.AddElementFieldInfo("AmazonShipmentEntity", "DeclaredValue", typeof(Nullable<System.Decimal>), false, false, false, true,  (int)AmazonShipmentFieldIndex.DeclaredValue, 0, 4, 19);
			this.AddElementFieldInfo("AmazonShipmentEntity", "AmazonUniqueShipmentID", typeof(System.String), false, false, false, true,  (int)AmazonShipmentFieldIndex.AmazonUniqueShipmentID, 50, 0, 0);
		}
		/// <summary>Inits AmazonStoreEntity's FieldInfo objects</summary>
		private void InitAmazonStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(AmazonStoreFieldIndex), "AmazonStoreEntity");
			this.AddElementFieldInfo("AmazonStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)AmazonStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("AmazonStoreEntity", "AmazonApi", typeof(System.Int32), false, false, false, false,  (int)AmazonStoreFieldIndex.AmazonApi, 0, 0, 10);
			this.AddElementFieldInfo("AmazonStoreEntity", "AmazonApiRegion", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.AmazonApiRegion, 2, 0, 0);
			this.AddElementFieldInfo("AmazonStoreEntity", "SellerCentralUsername", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.SellerCentralUsername, 50, 0, 0);
			this.AddElementFieldInfo("AmazonStoreEntity", "SellerCentralPassword", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.SellerCentralPassword, 50, 0, 0);
			this.AddElementFieldInfo("AmazonStoreEntity", "MerchantName", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.MerchantName, 64, 0, 0);
			this.AddElementFieldInfo("AmazonStoreEntity", "MerchantToken", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.MerchantToken, 32, 0, 0);
			this.AddElementFieldInfo("AmazonStoreEntity", "AccessKeyID", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.AccessKeyID, 32, 0, 0);
			this.AddElementFieldInfo("AmazonStoreEntity", "AuthToken", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.AuthToken, 100, 0, 0);
			this.AddElementFieldInfo("AmazonStoreEntity", "Cookie", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.Cookie, 2147483647, 0, 0);
			this.AddElementFieldInfo("AmazonStoreEntity", "CookieExpires", typeof(System.DateTime), false, false, false, false,  (int)AmazonStoreFieldIndex.CookieExpires, 0, 0, 0);
			this.AddElementFieldInfo("AmazonStoreEntity", "CookieWaitUntil", typeof(System.DateTime), false, false, false, false,  (int)AmazonStoreFieldIndex.CookieWaitUntil, 0, 0, 0);
			this.AddElementFieldInfo("AmazonStoreEntity", "Certificate", typeof(System.Byte[]), false, false, false, true,  (int)AmazonStoreFieldIndex.Certificate, 2048, 0, 0);
			this.AddElementFieldInfo("AmazonStoreEntity", "WeightDownloads", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.WeightDownloads, 2147483647, 0, 0);
			this.AddElementFieldInfo("AmazonStoreEntity", "MerchantID", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.MerchantID, 50, 0, 0);
			this.AddElementFieldInfo("AmazonStoreEntity", "MarketplaceID", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.MarketplaceID, 50, 0, 0);
			this.AddElementFieldInfo("AmazonStoreEntity", "ExcludeFBA", typeof(System.Boolean), false, false, false, false,  (int)AmazonStoreFieldIndex.ExcludeFBA, 0, 0, 0);
			this.AddElementFieldInfo("AmazonStoreEntity", "DomainName", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.DomainName, 50, 0, 0);
			this.AddElementFieldInfo("AmazonStoreEntity", "AmazonShippingToken", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.AmazonShippingToken, 500, 0, 0);
		}
		/// <summary>Inits AmeriCommerceStoreEntity's FieldInfo objects</summary>
		private void InitAmeriCommerceStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(AmeriCommerceStoreFieldIndex), "AmeriCommerceStoreEntity");
			this.AddElementFieldInfo("AmeriCommerceStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)AmeriCommerceStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("AmeriCommerceStoreEntity", "Username", typeof(System.String), false, false, false, false,  (int)AmeriCommerceStoreFieldIndex.Username, 70, 0, 0);
			this.AddElementFieldInfo("AmeriCommerceStoreEntity", "Password", typeof(System.String), false, false, false, false,  (int)AmeriCommerceStoreFieldIndex.Password, 70, 0, 0);
			this.AddElementFieldInfo("AmeriCommerceStoreEntity", "StoreUrl", typeof(System.String), false, false, false, false,  (int)AmeriCommerceStoreFieldIndex.StoreUrl, 350, 0, 0);
			this.AddElementFieldInfo("AmeriCommerceStoreEntity", "StoreCode", typeof(System.Int32), false, false, false, false,  (int)AmeriCommerceStoreFieldIndex.StoreCode, 0, 0, 10);
			this.AddElementFieldInfo("AmeriCommerceStoreEntity", "StatusCodes", typeof(System.String), false, false, false, false,  (int)AmeriCommerceStoreFieldIndex.StatusCodes, 2147483647, 0, 0);
		}
		/// <summary>Inits AuditEntity's FieldInfo objects</summary>
		private void InitAuditEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(AuditFieldIndex), "AuditEntity");
			this.AddElementFieldInfo("AuditEntity", "AuditID", typeof(System.Int64), true, false, true, false,  (int)AuditFieldIndex.AuditID, 0, 0, 19);
			this.AddElementFieldInfo("AuditEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)AuditFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("AuditEntity", "TransactionID", typeof(System.Int64), false, false, false, false,  (int)AuditFieldIndex.TransactionID, 0, 0, 19);
			this.AddElementFieldInfo("AuditEntity", "UserID", typeof(System.Int64), false, true, false, false,  (int)AuditFieldIndex.UserID, 0, 0, 19);
			this.AddElementFieldInfo("AuditEntity", "ComputerID", typeof(System.Int64), false, true, false, false,  (int)AuditFieldIndex.ComputerID, 0, 0, 19);
			this.AddElementFieldInfo("AuditEntity", "Reason", typeof(System.Int32), false, false, false, false,  (int)AuditFieldIndex.Reason, 0, 0, 10);
			this.AddElementFieldInfo("AuditEntity", "ReasonDetail", typeof(System.String), false, false, false, true,  (int)AuditFieldIndex.ReasonDetail, 100, 0, 0);
			this.AddElementFieldInfo("AuditEntity", "Date", typeof(System.DateTime), false, false, false, false,  (int)AuditFieldIndex.Date, 0, 0, 0);
			this.AddElementFieldInfo("AuditEntity", "Action", typeof(System.Int32), false, false, false, false,  (int)AuditFieldIndex.Action, 0, 0, 10);
			this.AddElementFieldInfo("AuditEntity", "EntityID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)AuditFieldIndex.EntityID, 0, 0, 19);
			this.AddElementFieldInfo("AuditEntity", "HasEvents", typeof(System.Boolean), false, false, false, false,  (int)AuditFieldIndex.HasEvents, 0, 0, 0);
		}
		/// <summary>Inits AuditChangeEntity's FieldInfo objects</summary>
		private void InitAuditChangeEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(AuditChangeFieldIndex), "AuditChangeEntity");
			this.AddElementFieldInfo("AuditChangeEntity", "AuditChangeID", typeof(System.Int64), true, false, true, false,  (int)AuditChangeFieldIndex.AuditChangeID, 0, 0, 19);
			this.AddElementFieldInfo("AuditChangeEntity", "AuditID", typeof(System.Int64), false, true, false, false,  (int)AuditChangeFieldIndex.AuditID, 0, 0, 19);
			this.AddElementFieldInfo("AuditChangeEntity", "ChangeType", typeof(System.Int32), false, false, false, false,  (int)AuditChangeFieldIndex.ChangeType, 0, 0, 10);
			this.AddElementFieldInfo("AuditChangeEntity", "EntityID", typeof(System.Int64), false, false, false, false,  (int)AuditChangeFieldIndex.EntityID, 0, 0, 19);
		}
		/// <summary>Inits AuditChangeDetailEntity's FieldInfo objects</summary>
		private void InitAuditChangeDetailEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(AuditChangeDetailFieldIndex), "AuditChangeDetailEntity");
			this.AddElementFieldInfo("AuditChangeDetailEntity", "AuditChangeDetailID", typeof(System.Int64), true, false, true, false,  (int)AuditChangeDetailFieldIndex.AuditChangeDetailID, 0, 0, 19);
			this.AddElementFieldInfo("AuditChangeDetailEntity", "AuditChangeID", typeof(System.Int64), false, true, false, false,  (int)AuditChangeDetailFieldIndex.AuditChangeID, 0, 0, 19);
			this.AddElementFieldInfo("AuditChangeDetailEntity", "AuditID", typeof(System.Int64), false, false, false, false,  (int)AuditChangeDetailFieldIndex.AuditID, 0, 0, 19);
			this.AddElementFieldInfo("AuditChangeDetailEntity", "DisplayName", typeof(System.String), false, false, false, false,  (int)AuditChangeDetailFieldIndex.DisplayName, 50, 0, 0);
			this.AddElementFieldInfo("AuditChangeDetailEntity", "DisplayFormat", typeof(System.Byte), false, false, false, false,  (int)AuditChangeDetailFieldIndex.DisplayFormat, 0, 0, 3);
			this.AddElementFieldInfo("AuditChangeDetailEntity", "DataType", typeof(System.Byte), false, false, false, false,  (int)AuditChangeDetailFieldIndex.DataType, 0, 0, 3);
			this.AddElementFieldInfo("AuditChangeDetailEntity", "TextOld", typeof(System.String), false, false, false, true,  (int)AuditChangeDetailFieldIndex.TextOld, 2147483647, 0, 0);
			this.AddElementFieldInfo("AuditChangeDetailEntity", "TextNew", typeof(System.String), false, false, false, true,  (int)AuditChangeDetailFieldIndex.TextNew, 2147483647, 0, 0);
			this.AddElementFieldInfo("AuditChangeDetailEntity", "VariantOld", typeof(System.Object), false, false, false, true,  (int)AuditChangeDetailFieldIndex.VariantOld, 0, 0, 0);
			this.AddElementFieldInfo("AuditChangeDetailEntity", "VariantNew", typeof(System.Object), false, false, false, true,  (int)AuditChangeDetailFieldIndex.VariantNew, 0, 0, 0);
		}
		/// <summary>Inits BestRateProfileEntity's FieldInfo objects</summary>
		private void InitBestRateProfileEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(BestRateProfileFieldIndex), "BestRateProfileEntity");
			this.AddElementFieldInfo("BestRateProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)BestRateProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			this.AddElementFieldInfo("BestRateProfileEntity", "DimsProfileID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)BestRateProfileFieldIndex.DimsProfileID, 0, 0, 19);
			this.AddElementFieldInfo("BestRateProfileEntity", "DimsLength", typeof(Nullable<System.Double>), false, false, false, true,  (int)BestRateProfileFieldIndex.DimsLength, 0, 0, 38);
			this.AddElementFieldInfo("BestRateProfileEntity", "DimsWidth", typeof(Nullable<System.Double>), false, false, false, true,  (int)BestRateProfileFieldIndex.DimsWidth, 0, 0, 38);
			this.AddElementFieldInfo("BestRateProfileEntity", "DimsHeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)BestRateProfileFieldIndex.DimsHeight, 0, 0, 38);
			this.AddElementFieldInfo("BestRateProfileEntity", "DimsWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)BestRateProfileFieldIndex.DimsWeight, 0, 0, 38);
			this.AddElementFieldInfo("BestRateProfileEntity", "DimsAddWeight", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)BestRateProfileFieldIndex.DimsAddWeight, 0, 0, 0);
			this.AddElementFieldInfo("BestRateProfileEntity", "Weight", typeof(Nullable<System.Double>), false, false, false, true,  (int)BestRateProfileFieldIndex.Weight, 0, 0, 38);
			this.AddElementFieldInfo("BestRateProfileEntity", "ServiceLevel", typeof(Nullable<System.Int32>), false, false, false, true,  (int)BestRateProfileFieldIndex.ServiceLevel, 0, 0, 10);
		}
		/// <summary>Inits BestRateShipmentEntity's FieldInfo objects</summary>
		private void InitBestRateShipmentEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(BestRateShipmentFieldIndex), "BestRateShipmentEntity");
			this.AddElementFieldInfo("BestRateShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)BestRateShipmentFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("BestRateShipmentEntity", "DimsProfileID", typeof(System.Int64), false, false, false, false,  (int)BestRateShipmentFieldIndex.DimsProfileID, 0, 0, 19);
			this.AddElementFieldInfo("BestRateShipmentEntity", "DimsLength", typeof(System.Double), false, false, false, false,  (int)BestRateShipmentFieldIndex.DimsLength, 0, 0, 38);
			this.AddElementFieldInfo("BestRateShipmentEntity", "DimsWidth", typeof(System.Double), false, false, false, false,  (int)BestRateShipmentFieldIndex.DimsWidth, 0, 0, 38);
			this.AddElementFieldInfo("BestRateShipmentEntity", "DimsHeight", typeof(System.Double), false, false, false, false,  (int)BestRateShipmentFieldIndex.DimsHeight, 0, 0, 38);
			this.AddElementFieldInfo("BestRateShipmentEntity", "DimsWeight", typeof(System.Double), false, false, false, false,  (int)BestRateShipmentFieldIndex.DimsWeight, 0, 0, 38);
			this.AddElementFieldInfo("BestRateShipmentEntity", "DimsAddWeight", typeof(System.Boolean), false, false, false, false,  (int)BestRateShipmentFieldIndex.DimsAddWeight, 0, 0, 0);
			this.AddElementFieldInfo("BestRateShipmentEntity", "ServiceLevel", typeof(System.Int32), false, false, false, false,  (int)BestRateShipmentFieldIndex.ServiceLevel, 0, 0, 10);
			this.AddElementFieldInfo("BestRateShipmentEntity", "InsuranceValue", typeof(System.Decimal), false, false, false, false,  (int)BestRateShipmentFieldIndex.InsuranceValue, 0, 4, 19);
			this.AddElementFieldInfo("BestRateShipmentEntity", "RequestedLabelFormat", typeof(System.Int32), false, false, false, false,  (int)BestRateShipmentFieldIndex.RequestedLabelFormat, 0, 0, 10);
		}
		/// <summary>Inits BigCommerceOrderItemEntity's FieldInfo objects</summary>
		private void InitBigCommerceOrderItemEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(BigCommerceOrderItemFieldIndex), "BigCommerceOrderItemEntity");
			this.AddElementFieldInfo("BigCommerceOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, true, false,  (int)BigCommerceOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("BigCommerceOrderItemEntity", "OrderAddressID", typeof(System.Int64), false, false, false, false,  (int)BigCommerceOrderItemFieldIndex.OrderAddressID, 0, 0, 19);
			this.AddElementFieldInfo("BigCommerceOrderItemEntity", "OrderProductID", typeof(System.Int64), false, false, false, false,  (int)BigCommerceOrderItemFieldIndex.OrderProductID, 0, 0, 19);
			this.AddElementFieldInfo("BigCommerceOrderItemEntity", "IsDigitalItem", typeof(System.Boolean), false, false, false, false,  (int)BigCommerceOrderItemFieldIndex.IsDigitalItem, 0, 0, 0);
			this.AddElementFieldInfo("BigCommerceOrderItemEntity", "EventDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)BigCommerceOrderItemFieldIndex.EventDate, 0, 0, 0);
			this.AddElementFieldInfo("BigCommerceOrderItemEntity", "EventName", typeof(System.String), false, false, false, true,  (int)BigCommerceOrderItemFieldIndex.EventName, 255, 0, 0);
		}
		/// <summary>Inits BigCommerceStoreEntity's FieldInfo objects</summary>
		private void InitBigCommerceStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(BigCommerceStoreFieldIndex), "BigCommerceStoreEntity");
			this.AddElementFieldInfo("BigCommerceStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)BigCommerceStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("BigCommerceStoreEntity", "ApiUrl", typeof(System.String), false, false, false, false,  (int)BigCommerceStoreFieldIndex.ApiUrl, 110, 0, 0);
			this.AddElementFieldInfo("BigCommerceStoreEntity", "ApiUserName", typeof(System.String), false, false, false, false,  (int)BigCommerceStoreFieldIndex.ApiUserName, 65, 0, 0);
			this.AddElementFieldInfo("BigCommerceStoreEntity", "ApiToken", typeof(System.String), false, false, false, true,  (int)BigCommerceStoreFieldIndex.ApiToken, 100, 0, 0);
			this.AddElementFieldInfo("BigCommerceStoreEntity", "StatusCodes", typeof(System.String), false, false, false, true,  (int)BigCommerceStoreFieldIndex.StatusCodes, 2147483647, 0, 0);
			this.AddElementFieldInfo("BigCommerceStoreEntity", "WeightUnitOfMeasure", typeof(System.Int32), false, false, false, false,  (int)BigCommerceStoreFieldIndex.WeightUnitOfMeasure, 0, 0, 10);
			this.AddElementFieldInfo("BigCommerceStoreEntity", "DownloadModifiedNumberOfDaysBack", typeof(System.Int32), false, false, false, false,  (int)BigCommerceStoreFieldIndex.DownloadModifiedNumberOfDaysBack, 0, 0, 10);
		}
		/// <summary>Inits BuyDotComOrderItemEntity's FieldInfo objects</summary>
		private void InitBuyDotComOrderItemEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(BuyDotComOrderItemFieldIndex), "BuyDotComOrderItemEntity");
			this.AddElementFieldInfo("BuyDotComOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, true, false,  (int)BuyDotComOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("BuyDotComOrderItemEntity", "ReceiptItemID", typeof(System.Int64), false, false, false, false,  (int)BuyDotComOrderItemFieldIndex.ReceiptItemID, 0, 0, 19);
			this.AddElementFieldInfo("BuyDotComOrderItemEntity", "ListingID", typeof(System.Int32), false, false, false, false,  (int)BuyDotComOrderItemFieldIndex.ListingID, 0, 0, 10);
			this.AddElementFieldInfo("BuyDotComOrderItemEntity", "Shipping", typeof(System.Decimal), false, false, false, false,  (int)BuyDotComOrderItemFieldIndex.Shipping, 0, 4, 19);
			this.AddElementFieldInfo("BuyDotComOrderItemEntity", "Tax", typeof(System.Decimal), false, false, false, false,  (int)BuyDotComOrderItemFieldIndex.Tax, 0, 4, 19);
			this.AddElementFieldInfo("BuyDotComOrderItemEntity", "Commission", typeof(System.Decimal), false, false, false, false,  (int)BuyDotComOrderItemFieldIndex.Commission, 0, 4, 19);
			this.AddElementFieldInfo("BuyDotComOrderItemEntity", "ItemFee", typeof(System.Decimal), false, false, false, false,  (int)BuyDotComOrderItemFieldIndex.ItemFee, 0, 4, 19);
		}
		/// <summary>Inits BuyDotComStoreEntity's FieldInfo objects</summary>
		private void InitBuyDotComStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(BuyDotComStoreFieldIndex), "BuyDotComStoreEntity");
			this.AddElementFieldInfo("BuyDotComStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)BuyDotComStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("BuyDotComStoreEntity", "FtpUsername", typeof(System.String), false, false, false, false,  (int)BuyDotComStoreFieldIndex.FtpUsername, 50, 0, 0);
			this.AddElementFieldInfo("BuyDotComStoreEntity", "FtpPassword", typeof(System.String), false, false, false, false,  (int)BuyDotComStoreFieldIndex.FtpPassword, 50, 0, 0);
		}
		/// <summary>Inits ChannelAdvisorOrderEntity's FieldInfo objects</summary>
		private void InitChannelAdvisorOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ChannelAdvisorOrderFieldIndex), "ChannelAdvisorOrderEntity");
			this.AddElementFieldInfo("ChannelAdvisorOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)ChannelAdvisorOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("ChannelAdvisorOrderEntity", "CustomOrderIdentifier", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.CustomOrderIdentifier, 50, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorOrderEntity", "ResellerID", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.ResellerID, 80, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorOrderEntity", "OnlineShippingStatus", typeof(System.Int32), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.OnlineShippingStatus, 0, 0, 10);
			this.AddElementFieldInfo("ChannelAdvisorOrderEntity", "OnlineCheckoutStatus", typeof(System.Int32), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.OnlineCheckoutStatus, 0, 0, 10);
			this.AddElementFieldInfo("ChannelAdvisorOrderEntity", "OnlinePaymentStatus", typeof(System.Int32), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.OnlinePaymentStatus, 0, 0, 10);
			this.AddElementFieldInfo("ChannelAdvisorOrderEntity", "FlagStyle", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.FlagStyle, 32, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorOrderEntity", "FlagDescription", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.FlagDescription, 80, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorOrderEntity", "FlagType", typeof(System.Int32), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.FlagType, 0, 0, 10);
			this.AddElementFieldInfo("ChannelAdvisorOrderEntity", "MarketplaceNames", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.MarketplaceNames, 1024, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorOrderEntity", "IsPrime", typeof(System.Int32), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.IsPrime, 0, 0, 10);
		}
		/// <summary>Inits ChannelAdvisorOrderItemEntity's FieldInfo objects</summary>
		private void InitChannelAdvisorOrderItemEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ChannelAdvisorOrderItemFieldIndex), "ChannelAdvisorOrderItemEntity");
			this.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, true, false,  (int)ChannelAdvisorOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "MarketplaceName", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.MarketplaceName, 50, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "MarketplaceStoreName", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.MarketplaceStoreName, 100, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "MarketplaceBuyerID", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.MarketplaceBuyerID, 80, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "MarketplaceSalesID", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.MarketplaceSalesID, 50, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "Classification", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.Classification, 30, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "DistributionCenter", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.DistributionCenter, 80, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "HarmonizedCode", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.HarmonizedCode, 20, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "IsFBA", typeof(System.Boolean), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.IsFBA, 0, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "MPN", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.MPN, 50, 0, 0);
		}
		/// <summary>Inits ChannelAdvisorStoreEntity's FieldInfo objects</summary>
		private void InitChannelAdvisorStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ChannelAdvisorStoreFieldIndex), "ChannelAdvisorStoreEntity");
			this.AddElementFieldInfo("ChannelAdvisorStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("ChannelAdvisorStoreEntity", "AccountKey", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.AccountKey, 50, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorStoreEntity", "ProfileID", typeof(System.Int32), false, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.ProfileID, 0, 0, 10);
			this.AddElementFieldInfo("ChannelAdvisorStoreEntity", "AttributesToDownload", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.AttributesToDownload, 2147483647, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorStoreEntity", "ConsolidatorAsUsps", typeof(System.Boolean), false, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.ConsolidatorAsUsps, 0, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorStoreEntity", "AmazonMerchantID", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.AmazonMerchantID, 50, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorStoreEntity", "AmazonAuthToken", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.AmazonAuthToken, 100, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorStoreEntity", "AmazonApiRegion", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.AmazonApiRegion, 2, 0, 0);
			this.AddElementFieldInfo("ChannelAdvisorStoreEntity", "AmazonShippingToken", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.AmazonShippingToken, 500, 0, 0);
		}
		/// <summary>Inits ClickCartProOrderEntity's FieldInfo objects</summary>
		private void InitClickCartProOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ClickCartProOrderFieldIndex), "ClickCartProOrderEntity");
			this.AddElementFieldInfo("ClickCartProOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)ClickCartProOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("ClickCartProOrderEntity", "ClickCartProOrderID", typeof(System.String), false, false, false, false,  (int)ClickCartProOrderFieldIndex.ClickCartProOrderID, 25, 0, 0);
		}
		/// <summary>Inits CommerceInterfaceOrderEntity's FieldInfo objects</summary>
		private void InitCommerceInterfaceOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(CommerceInterfaceOrderFieldIndex), "CommerceInterfaceOrderEntity");
			this.AddElementFieldInfo("CommerceInterfaceOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)CommerceInterfaceOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("CommerceInterfaceOrderEntity", "CommerceInterfaceOrderNumber", typeof(System.String), false, false, false, false,  (int)CommerceInterfaceOrderFieldIndex.CommerceInterfaceOrderNumber, 60, 0, 0);
		}
		/// <summary>Inits ComputerEntity's FieldInfo objects</summary>
		private void InitComputerEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ComputerFieldIndex), "ComputerEntity");
			this.AddElementFieldInfo("ComputerEntity", "ComputerID", typeof(System.Int64), true, false, true, false,  (int)ComputerFieldIndex.ComputerID, 0, 0, 19);
			this.AddElementFieldInfo("ComputerEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ComputerFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("ComputerEntity", "Identifier", typeof(System.Guid), false, false, false, false,  (int)ComputerFieldIndex.Identifier, 0, 0, 0);
			this.AddElementFieldInfo("ComputerEntity", "Name", typeof(System.String), false, false, false, false,  (int)ComputerFieldIndex.Name, 50, 0, 0);
		}
		/// <summary>Inits ConfigurationEntity's FieldInfo objects</summary>
		private void InitConfigurationEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ConfigurationFieldIndex), "ConfigurationEntity");
			this.AddElementFieldInfo("ConfigurationEntity", "ConfigurationID", typeof(System.Boolean), true, false, false, false,  (int)ConfigurationFieldIndex.ConfigurationID, 0, 0, 0);
			this.AddElementFieldInfo("ConfigurationEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ConfigurationFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("ConfigurationEntity", "LogOnMethod", typeof(System.Int32), false, false, false, false,  (int)ConfigurationFieldIndex.LogOnMethod, 0, 0, 10);
			this.AddElementFieldInfo("ConfigurationEntity", "AddressCasing", typeof(System.Boolean), false, false, false, false,  (int)ConfigurationFieldIndex.AddressCasing, 0, 0, 0);
			this.AddElementFieldInfo("ConfigurationEntity", "CustomerCompareEmail", typeof(System.Boolean), false, false, false, false,  (int)ConfigurationFieldIndex.CustomerCompareEmail, 0, 0, 0);
			this.AddElementFieldInfo("ConfigurationEntity", "CustomerCompareAddress", typeof(System.Boolean), false, false, false, false,  (int)ConfigurationFieldIndex.CustomerCompareAddress, 0, 0, 0);
			this.AddElementFieldInfo("ConfigurationEntity", "CustomerUpdateBilling", typeof(System.Boolean), false, false, false, false,  (int)ConfigurationFieldIndex.CustomerUpdateBilling, 0, 0, 0);
			this.AddElementFieldInfo("ConfigurationEntity", "CustomerUpdateShipping", typeof(System.Boolean), false, false, false, false,  (int)ConfigurationFieldIndex.CustomerUpdateShipping, 0, 0, 0);
			this.AddElementFieldInfo("ConfigurationEntity", "CustomerUpdateModifiedBilling", typeof(System.Int32), false, false, false, false,  (int)ConfigurationFieldIndex.CustomerUpdateModifiedBilling, 0, 0, 10);
			this.AddElementFieldInfo("ConfigurationEntity", "CustomerUpdateModifiedShipping", typeof(System.Int32), false, false, false, false,  (int)ConfigurationFieldIndex.CustomerUpdateModifiedShipping, 0, 0, 10);
			this.AddElementFieldInfo("ConfigurationEntity", "AuditNewOrders", typeof(System.Boolean), false, false, false, false,  (int)ConfigurationFieldIndex.AuditNewOrders, 0, 0, 0);
			this.AddElementFieldInfo("ConfigurationEntity", "AuditDeletedOrders", typeof(System.Boolean), false, false, false, false,  (int)ConfigurationFieldIndex.AuditDeletedOrders, 0, 0, 0);
			this.AddElementFieldInfo("ConfigurationEntity", "CustomerKey", typeof(System.String), false, false, false, false,  (int)ConfigurationFieldIndex.CustomerKey, 2147483647, 0, 0);
		}
		/// <summary>Inits CustomerEntity's FieldInfo objects</summary>
		private void InitCustomerEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(CustomerFieldIndex), "CustomerEntity");
			this.AddElementFieldInfo("CustomerEntity", "CustomerID", typeof(System.Int64), true, false, true, false,  (int)CustomerFieldIndex.CustomerID, 0, 0, 19);
			this.AddElementFieldInfo("CustomerEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)CustomerFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "BillFirstName", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillFirstName, 30, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "BillMiddleName", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillMiddleName, 30, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "BillLastName", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillLastName, 30, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "BillCompany", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillCompany, 60, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "BillStreet1", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillStreet1, 60, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "BillStreet2", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillStreet2, 60, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "BillStreet3", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillStreet3, 60, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "BillCity", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillCity, 50, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "BillStateProvCode", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillStateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "BillPostalCode", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillPostalCode, 20, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "BillCountryCode", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillCountryCode, 50, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "BillPhone", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillPhone, 25, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "BillFax", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillFax, 35, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "BillEmail", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillEmail, 100, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "BillWebsite", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillWebsite, 50, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "ShipFirstName", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipFirstName, 30, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "ShipMiddleName", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipMiddleName, 30, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "ShipLastName", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipLastName, 30, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "ShipCompany", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipCompany, 60, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "ShipStreet1", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipStreet1, 60, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "ShipStreet2", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipStreet2, 60, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "ShipStreet3", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipStreet3, 60, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "ShipCity", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipCity, 50, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "ShipStateProvCode", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipStateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "ShipPostalCode", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipPostalCode, 20, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "ShipCountryCode", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipCountryCode, 50, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "ShipPhone", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipPhone, 25, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "ShipFax", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipFax, 35, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "ShipEmail", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipEmail, 100, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "ShipWebsite", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipWebsite, 50, 0, 0);
			this.AddElementFieldInfo("CustomerEntity", "RollupOrderCount", typeof(System.Int32), false, false, false, false,  (int)CustomerFieldIndex.RollupOrderCount, 0, 0, 10);
			this.AddElementFieldInfo("CustomerEntity", "RollupOrderTotal", typeof(System.Decimal), false, false, false, false,  (int)CustomerFieldIndex.RollupOrderTotal, 0, 4, 19);
			this.AddElementFieldInfo("CustomerEntity", "RollupNoteCount", typeof(System.Int32), false, false, false, false,  (int)CustomerFieldIndex.RollupNoteCount, 0, 0, 10);
		}
		/// <summary>Inits DimensionsProfileEntity's FieldInfo objects</summary>
		private void InitDimensionsProfileEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(DimensionsProfileFieldIndex), "DimensionsProfileEntity");
			this.AddElementFieldInfo("DimensionsProfileEntity", "DimensionsProfileID", typeof(System.Int64), true, false, true, false,  (int)DimensionsProfileFieldIndex.DimensionsProfileID, 0, 0, 19);
			this.AddElementFieldInfo("DimensionsProfileEntity", "Name", typeof(System.String), false, false, false, false,  (int)DimensionsProfileFieldIndex.Name, 50, 0, 0);
			this.AddElementFieldInfo("DimensionsProfileEntity", "Length", typeof(System.Double), false, false, false, false,  (int)DimensionsProfileFieldIndex.Length, 0, 0, 38);
			this.AddElementFieldInfo("DimensionsProfileEntity", "Width", typeof(System.Double), false, false, false, false,  (int)DimensionsProfileFieldIndex.Width, 0, 0, 38);
			this.AddElementFieldInfo("DimensionsProfileEntity", "Height", typeof(System.Double), false, false, false, false,  (int)DimensionsProfileFieldIndex.Height, 0, 0, 38);
			this.AddElementFieldInfo("DimensionsProfileEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)DimensionsProfileFieldIndex.Weight, 0, 0, 38);
		}
		/// <summary>Inits DownloadEntity's FieldInfo objects</summary>
		private void InitDownloadEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(DownloadFieldIndex), "DownloadEntity");
			this.AddElementFieldInfo("DownloadEntity", "DownloadID", typeof(System.Int64), true, false, true, false,  (int)DownloadFieldIndex.DownloadID, 0, 0, 19);
			this.AddElementFieldInfo("DownloadEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)DownloadFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("DownloadEntity", "StoreID", typeof(System.Int64), false, true, false, false,  (int)DownloadFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("DownloadEntity", "ComputerID", typeof(System.Int64), false, true, false, false,  (int)DownloadFieldIndex.ComputerID, 0, 0, 19);
			this.AddElementFieldInfo("DownloadEntity", "UserID", typeof(System.Int64), false, true, false, false,  (int)DownloadFieldIndex.UserID, 0, 0, 19);
			this.AddElementFieldInfo("DownloadEntity", "InitiatedBy", typeof(System.Int32), false, false, false, false,  (int)DownloadFieldIndex.InitiatedBy, 0, 0, 10);
			this.AddElementFieldInfo("DownloadEntity", "Started", typeof(System.DateTime), false, false, false, false,  (int)DownloadFieldIndex.Started, 0, 0, 0);
			this.AddElementFieldInfo("DownloadEntity", "Ended", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)DownloadFieldIndex.Ended, 0, 0, 0);
			this.AddElementFieldInfo("DownloadEntity", "Duration", typeof(Nullable<System.Int32>), false, false, true, true,  (int)DownloadFieldIndex.Duration, 0, 0, 10);
			this.AddElementFieldInfo("DownloadEntity", "QuantityTotal", typeof(Nullable<System.Int32>), false, false, false, true,  (int)DownloadFieldIndex.QuantityTotal, 0, 0, 10);
			this.AddElementFieldInfo("DownloadEntity", "QuantityNew", typeof(Nullable<System.Int32>), false, false, false, true,  (int)DownloadFieldIndex.QuantityNew, 0, 0, 10);
			this.AddElementFieldInfo("DownloadEntity", "Result", typeof(System.Int32), false, false, false, false,  (int)DownloadFieldIndex.Result, 0, 0, 10);
			this.AddElementFieldInfo("DownloadEntity", "ErrorMessage", typeof(System.String), false, false, false, true,  (int)DownloadFieldIndex.ErrorMessage, 2147483647, 0, 0);
		}
		/// <summary>Inits DownloadDetailEntity's FieldInfo objects</summary>
		private void InitDownloadDetailEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(DownloadDetailFieldIndex), "DownloadDetailEntity");
			this.AddElementFieldInfo("DownloadDetailEntity", "DownloadedDetailID", typeof(System.Int64), true, false, true, false,  (int)DownloadDetailFieldIndex.DownloadedDetailID, 0, 0, 19);
			this.AddElementFieldInfo("DownloadDetailEntity", "DownloadID", typeof(System.Int64), false, true, false, false,  (int)DownloadDetailFieldIndex.DownloadID, 0, 0, 19);
			this.AddElementFieldInfo("DownloadDetailEntity", "OrderID", typeof(System.Int64), false, false, false, false,  (int)DownloadDetailFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("DownloadDetailEntity", "InitialDownload", typeof(System.Boolean), false, false, false, false,  (int)DownloadDetailFieldIndex.InitialDownload, 0, 0, 0);
			this.AddElementFieldInfo("DownloadDetailEntity", "OrderNumber", typeof(Nullable<System.Int64>), false, false, false, true,  (int)DownloadDetailFieldIndex.OrderNumber, 0, 0, 19);
			this.AddElementFieldInfo("DownloadDetailEntity", "ExtraBigIntData1", typeof(Nullable<System.Int64>), false, false, false, true,  (int)DownloadDetailFieldIndex.ExtraBigIntData1, 0, 0, 19);
			this.AddElementFieldInfo("DownloadDetailEntity", "ExtraBigIntData2", typeof(Nullable<System.Int64>), false, false, false, true,  (int)DownloadDetailFieldIndex.ExtraBigIntData2, 0, 0, 19);
			this.AddElementFieldInfo("DownloadDetailEntity", "ExtraBigIntData3", typeof(Nullable<System.Int64>), false, false, false, true,  (int)DownloadDetailFieldIndex.ExtraBigIntData3, 0, 0, 19);
			this.AddElementFieldInfo("DownloadDetailEntity", "ExtraStringData1", typeof(System.String), false, false, false, true,  (int)DownloadDetailFieldIndex.ExtraStringData1, 50, 0, 0);
		}
		/// <summary>Inits EbayCombinedOrderRelationEntity's FieldInfo objects</summary>
		private void InitEbayCombinedOrderRelationEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(EbayCombinedOrderRelationFieldIndex), "EbayCombinedOrderRelationEntity");
			this.AddElementFieldInfo("EbayCombinedOrderRelationEntity", "EbayCombinedOrderRelationID", typeof(System.Int64), true, false, true, false,  (int)EbayCombinedOrderRelationFieldIndex.EbayCombinedOrderRelationID, 0, 0, 19);
			this.AddElementFieldInfo("EbayCombinedOrderRelationEntity", "OrderID", typeof(System.Int64), false, true, false, false,  (int)EbayCombinedOrderRelationFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("EbayCombinedOrderRelationEntity", "EbayOrderID", typeof(System.Int64), false, false, false, false,  (int)EbayCombinedOrderRelationFieldIndex.EbayOrderID, 0, 0, 19);
			this.AddElementFieldInfo("EbayCombinedOrderRelationEntity", "StoreID", typeof(System.Int64), false, true, false, false,  (int)EbayCombinedOrderRelationFieldIndex.StoreID, 0, 0, 19);
		}
		/// <summary>Inits EbayOrderEntity's FieldInfo objects</summary>
		private void InitEbayOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(EbayOrderFieldIndex), "EbayOrderEntity");
			this.AddElementFieldInfo("EbayOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)EbayOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("EbayOrderEntity", "EbayOrderID", typeof(System.Int64), false, false, false, false,  (int)EbayOrderFieldIndex.EbayOrderID, 0, 0, 19);
			this.AddElementFieldInfo("EbayOrderEntity", "EbayBuyerID", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.EbayBuyerID, 50, 0, 0);
			this.AddElementFieldInfo("EbayOrderEntity", "CombinedLocally", typeof(System.Boolean), false, false, false, false,  (int)EbayOrderFieldIndex.CombinedLocally, 0, 0, 0);
			this.AddElementFieldInfo("EbayOrderEntity", "SelectedShippingMethod", typeof(System.Int32), false, false, false, false,  (int)EbayOrderFieldIndex.SelectedShippingMethod, 0, 0, 10);
			this.AddElementFieldInfo("EbayOrderEntity", "SellingManagerRecord", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EbayOrderFieldIndex.SellingManagerRecord, 0, 0, 10);
			this.AddElementFieldInfo("EbayOrderEntity", "GspEligible", typeof(System.Boolean), false, false, false, false,  (int)EbayOrderFieldIndex.GspEligible, 0, 0, 0);
			this.AddElementFieldInfo("EbayOrderEntity", "GspFirstName", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspFirstName, 128, 0, 0);
			this.AddElementFieldInfo("EbayOrderEntity", "GspLastName", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspLastName, 128, 0, 0);
			this.AddElementFieldInfo("EbayOrderEntity", "GspStreet1", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspStreet1, 512, 0, 0);
			this.AddElementFieldInfo("EbayOrderEntity", "GspStreet2", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspStreet2, 512, 0, 0);
			this.AddElementFieldInfo("EbayOrderEntity", "GspCity", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspCity, 128, 0, 0);
			this.AddElementFieldInfo("EbayOrderEntity", "GspStateProvince", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspStateProvince, 128, 0, 0);
			this.AddElementFieldInfo("EbayOrderEntity", "GspPostalCode", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspPostalCode, 9, 0, 0);
			this.AddElementFieldInfo("EbayOrderEntity", "GspCountryCode", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspCountryCode, 2, 0, 0);
			this.AddElementFieldInfo("EbayOrderEntity", "GspReferenceID", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspReferenceID, 128, 0, 0);
			this.AddElementFieldInfo("EbayOrderEntity", "RollupEbayItemCount", typeof(System.Int32), false, false, false, false,  (int)EbayOrderFieldIndex.RollupEbayItemCount, 0, 0, 10);
			this.AddElementFieldInfo("EbayOrderEntity", "RollupEffectiveCheckoutStatus", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EbayOrderFieldIndex.RollupEffectiveCheckoutStatus, 0, 0, 10);
			this.AddElementFieldInfo("EbayOrderEntity", "RollupEffectivePaymentMethod", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EbayOrderFieldIndex.RollupEffectivePaymentMethod, 0, 0, 10);
			this.AddElementFieldInfo("EbayOrderEntity", "RollupFeedbackLeftType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EbayOrderFieldIndex.RollupFeedbackLeftType, 0, 0, 10);
			this.AddElementFieldInfo("EbayOrderEntity", "RollupFeedbackLeftComments", typeof(System.String), false, false, false, true,  (int)EbayOrderFieldIndex.RollupFeedbackLeftComments, 80, 0, 0);
			this.AddElementFieldInfo("EbayOrderEntity", "RollupFeedbackReceivedType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EbayOrderFieldIndex.RollupFeedbackReceivedType, 0, 0, 10);
			this.AddElementFieldInfo("EbayOrderEntity", "RollupFeedbackReceivedComments", typeof(System.String), false, false, false, true,  (int)EbayOrderFieldIndex.RollupFeedbackReceivedComments, 80, 0, 0);
			this.AddElementFieldInfo("EbayOrderEntity", "RollupPayPalAddressStatus", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EbayOrderFieldIndex.RollupPayPalAddressStatus, 0, 0, 10);
		}
		/// <summary>Inits EbayOrderItemEntity's FieldInfo objects</summary>
		private void InitEbayOrderItemEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(EbayOrderItemFieldIndex), "EbayOrderItemEntity");
			this.AddElementFieldInfo("EbayOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, true, false,  (int)EbayOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("EbayOrderItemEntity", "LocalEbayOrderID", typeof(System.Int64), false, true, false, false,  (int)EbayOrderItemFieldIndex.LocalEbayOrderID, 0, 0, 19);
			this.AddElementFieldInfo("EbayOrderItemEntity", "EbayItemID", typeof(System.Int64), false, false, false, false,  (int)EbayOrderItemFieldIndex.EbayItemID, 0, 0, 19);
			this.AddElementFieldInfo("EbayOrderItemEntity", "EbayTransactionID", typeof(System.Int64), false, false, false, false,  (int)EbayOrderItemFieldIndex.EbayTransactionID, 0, 0, 19);
			this.AddElementFieldInfo("EbayOrderItemEntity", "SellingManagerRecord", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.SellingManagerRecord, 0, 0, 10);
			this.AddElementFieldInfo("EbayOrderItemEntity", "EffectiveCheckoutStatus", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.EffectiveCheckoutStatus, 0, 0, 10);
			this.AddElementFieldInfo("EbayOrderItemEntity", "EffectivePaymentMethod", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.EffectivePaymentMethod, 0, 0, 10);
			this.AddElementFieldInfo("EbayOrderItemEntity", "PaymentStatus", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.PaymentStatus, 0, 0, 10);
			this.AddElementFieldInfo("EbayOrderItemEntity", "PaymentMethod", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.PaymentMethod, 0, 0, 10);
			this.AddElementFieldInfo("EbayOrderItemEntity", "CompleteStatus", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.CompleteStatus, 0, 0, 10);
			this.AddElementFieldInfo("EbayOrderItemEntity", "FeedbackLeftType", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.FeedbackLeftType, 0, 0, 10);
			this.AddElementFieldInfo("EbayOrderItemEntity", "FeedbackLeftComments", typeof(System.String), false, false, false, false,  (int)EbayOrderItemFieldIndex.FeedbackLeftComments, 80, 0, 0);
			this.AddElementFieldInfo("EbayOrderItemEntity", "FeedbackReceivedType", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.FeedbackReceivedType, 0, 0, 10);
			this.AddElementFieldInfo("EbayOrderItemEntity", "FeedbackReceivedComments", typeof(System.String), false, false, false, false,  (int)EbayOrderItemFieldIndex.FeedbackReceivedComments, 80, 0, 0);
			this.AddElementFieldInfo("EbayOrderItemEntity", "MyEbayPaid", typeof(System.Boolean), false, false, false, false,  (int)EbayOrderItemFieldIndex.MyEbayPaid, 0, 0, 0);
			this.AddElementFieldInfo("EbayOrderItemEntity", "MyEbayShipped", typeof(System.Boolean), false, false, false, false,  (int)EbayOrderItemFieldIndex.MyEbayShipped, 0, 0, 0);
			this.AddElementFieldInfo("EbayOrderItemEntity", "PayPalTransactionID", typeof(System.String), false, false, false, false,  (int)EbayOrderItemFieldIndex.PayPalTransactionID, 50, 0, 0);
			this.AddElementFieldInfo("EbayOrderItemEntity", "PayPalAddressStatus", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.PayPalAddressStatus, 0, 0, 10);
		}
		/// <summary>Inits EbayStoreEntity's FieldInfo objects</summary>
		private void InitEbayStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(EbayStoreFieldIndex), "EbayStoreEntity");
			this.AddElementFieldInfo("EbayStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)EbayStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("EbayStoreEntity", "EBayUserID", typeof(System.String), false, false, false, false,  (int)EbayStoreFieldIndex.EBayUserID, 50, 0, 0);
			this.AddElementFieldInfo("EbayStoreEntity", "EBayToken", typeof(System.String), false, false, false, false,  (int)EbayStoreFieldIndex.EBayToken, 2147483647, 0, 0);
			this.AddElementFieldInfo("EbayStoreEntity", "EBayTokenExpire", typeof(System.DateTime), false, false, false, false,  (int)EbayStoreFieldIndex.EBayTokenExpire, 0, 0, 0);
			this.AddElementFieldInfo("EbayStoreEntity", "AcceptedPaymentList", typeof(System.String), false, false, false, false,  (int)EbayStoreFieldIndex.AcceptedPaymentList, 30, 0, 0);
			this.AddElementFieldInfo("EbayStoreEntity", "DownloadItemDetails", typeof(System.Boolean), false, false, false, false,  (int)EbayStoreFieldIndex.DownloadItemDetails, 0, 0, 0);
			this.AddElementFieldInfo("EbayStoreEntity", "DownloadOlderOrders", typeof(System.Boolean), false, false, false, false,  (int)EbayStoreFieldIndex.DownloadOlderOrders, 0, 0, 0);
			this.AddElementFieldInfo("EbayStoreEntity", "DownloadPayPalDetails", typeof(System.Boolean), false, false, false, false,  (int)EbayStoreFieldIndex.DownloadPayPalDetails, 0, 0, 0);
			this.AddElementFieldInfo("EbayStoreEntity", "PayPalApiCredentialType", typeof(System.Int16), false, false, false, false,  (int)EbayStoreFieldIndex.PayPalApiCredentialType, 0, 0, 5);
			this.AddElementFieldInfo("EbayStoreEntity", "PayPalApiUserName", typeof(System.String), false, false, false, false,  (int)EbayStoreFieldIndex.PayPalApiUserName, 255, 0, 0);
			this.AddElementFieldInfo("EbayStoreEntity", "PayPalApiPassword", typeof(System.String), false, false, false, false,  (int)EbayStoreFieldIndex.PayPalApiPassword, 80, 0, 0);
			this.AddElementFieldInfo("EbayStoreEntity", "PayPalApiSignature", typeof(System.String), false, false, false, false,  (int)EbayStoreFieldIndex.PayPalApiSignature, 80, 0, 0);
			this.AddElementFieldInfo("EbayStoreEntity", "PayPalApiCertificate", typeof(System.Byte[]), false, false, false, true,  (int)EbayStoreFieldIndex.PayPalApiCertificate, 2048, 0, 0);
			this.AddElementFieldInfo("EbayStoreEntity", "DomesticShippingService", typeof(System.String), false, false, false, false,  (int)EbayStoreFieldIndex.DomesticShippingService, 50, 0, 0);
			this.AddElementFieldInfo("EbayStoreEntity", "InternationalShippingService", typeof(System.String), false, false, false, false,  (int)EbayStoreFieldIndex.InternationalShippingService, 50, 0, 0);
			this.AddElementFieldInfo("EbayStoreEntity", "FeedbackUpdatedThrough", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)EbayStoreFieldIndex.FeedbackUpdatedThrough, 0, 0, 0);
		}
		/// <summary>Inits EmailAccountEntity's FieldInfo objects</summary>
		private void InitEmailAccountEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(EmailAccountFieldIndex), "EmailAccountEntity");
			this.AddElementFieldInfo("EmailAccountEntity", "EmailAccountID", typeof(System.Int64), true, false, true, false,  (int)EmailAccountFieldIndex.EmailAccountID, 0, 0, 19);
			this.AddElementFieldInfo("EmailAccountEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)EmailAccountFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("EmailAccountEntity", "AccountName", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.AccountName, 50, 0, 0);
			this.AddElementFieldInfo("EmailAccountEntity", "DisplayName", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.DisplayName, 50, 0, 0);
			this.AddElementFieldInfo("EmailAccountEntity", "EmailAddress", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.EmailAddress, 100, 0, 0);
			this.AddElementFieldInfo("EmailAccountEntity", "IncomingServer", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.IncomingServer, 100, 0, 0);
			this.AddElementFieldInfo("EmailAccountEntity", "IncomingServerType", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.IncomingServerType, 0, 0, 10);
			this.AddElementFieldInfo("EmailAccountEntity", "IncomingPort", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.IncomingPort, 0, 0, 10);
			this.AddElementFieldInfo("EmailAccountEntity", "IncomingSecurityType", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.IncomingSecurityType, 0, 0, 10);
			this.AddElementFieldInfo("EmailAccountEntity", "IncomingUsername", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.IncomingUsername, 50, 0, 0);
			this.AddElementFieldInfo("EmailAccountEntity", "IncomingPassword", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.IncomingPassword, 150, 0, 0);
			this.AddElementFieldInfo("EmailAccountEntity", "OutgoingServer", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.OutgoingServer, 100, 0, 0);
			this.AddElementFieldInfo("EmailAccountEntity", "OutgoingPort", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.OutgoingPort, 0, 0, 10);
			this.AddElementFieldInfo("EmailAccountEntity", "OutgoingSecurityType", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.OutgoingSecurityType, 0, 0, 10);
			this.AddElementFieldInfo("EmailAccountEntity", "OutgoingCredentialSource", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.OutgoingCredentialSource, 0, 0, 10);
			this.AddElementFieldInfo("EmailAccountEntity", "OutgoingUsername", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.OutgoingUsername, 50, 0, 0);
			this.AddElementFieldInfo("EmailAccountEntity", "OutgoingPassword", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.OutgoingPassword, 150, 0, 0);
			this.AddElementFieldInfo("EmailAccountEntity", "AutoSend", typeof(System.Boolean), false, false, false, false,  (int)EmailAccountFieldIndex.AutoSend, 0, 0, 0);
			this.AddElementFieldInfo("EmailAccountEntity", "AutoSendMinutes", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.AutoSendMinutes, 0, 0, 10);
			this.AddElementFieldInfo("EmailAccountEntity", "AutoSendLastTime", typeof(System.DateTime), false, false, false, false,  (int)EmailAccountFieldIndex.AutoSendLastTime, 0, 0, 0);
			this.AddElementFieldInfo("EmailAccountEntity", "LimitMessagesPerConnection", typeof(System.Boolean), false, false, false, false,  (int)EmailAccountFieldIndex.LimitMessagesPerConnection, 0, 0, 0);
			this.AddElementFieldInfo("EmailAccountEntity", "LimitMessagesPerConnectionQuantity", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.LimitMessagesPerConnectionQuantity, 0, 0, 10);
			this.AddElementFieldInfo("EmailAccountEntity", "LimitMessagesPerHour", typeof(System.Boolean), false, false, false, false,  (int)EmailAccountFieldIndex.LimitMessagesPerHour, 0, 0, 0);
			this.AddElementFieldInfo("EmailAccountEntity", "LimitMessagesPerHourQuantity", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.LimitMessagesPerHourQuantity, 0, 0, 10);
			this.AddElementFieldInfo("EmailAccountEntity", "LimitMessageInterval", typeof(System.Boolean), false, false, false, false,  (int)EmailAccountFieldIndex.LimitMessageInterval, 0, 0, 0);
			this.AddElementFieldInfo("EmailAccountEntity", "LimitMessageIntervalSeconds", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.LimitMessageIntervalSeconds, 0, 0, 10);
			this.AddElementFieldInfo("EmailAccountEntity", "InternalOwnerID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)EmailAccountFieldIndex.InternalOwnerID, 0, 0, 19);
		}
		/// <summary>Inits EmailOutboundEntity's FieldInfo objects</summary>
		private void InitEmailOutboundEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(EmailOutboundFieldIndex), "EmailOutboundEntity");
			this.AddElementFieldInfo("EmailOutboundEntity", "EmailOutboundID", typeof(System.Int64), true, false, true, false,  (int)EmailOutboundFieldIndex.EmailOutboundID, 0, 0, 19);
			this.AddElementFieldInfo("EmailOutboundEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)EmailOutboundFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("EmailOutboundEntity", "ContextID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)EmailOutboundFieldIndex.ContextID, 0, 0, 19);
			this.AddElementFieldInfo("EmailOutboundEntity", "ContextType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EmailOutboundFieldIndex.ContextType, 0, 0, 10);
			this.AddElementFieldInfo("EmailOutboundEntity", "TemplateID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)EmailOutboundFieldIndex.TemplateID, 0, 0, 19);
			this.AddElementFieldInfo("EmailOutboundEntity", "AccountID", typeof(System.Int64), false, false, false, false,  (int)EmailOutboundFieldIndex.AccountID, 0, 0, 19);
			this.AddElementFieldInfo("EmailOutboundEntity", "Visibility", typeof(System.Int32), false, false, false, false,  (int)EmailOutboundFieldIndex.Visibility, 0, 0, 10);
			this.AddElementFieldInfo("EmailOutboundEntity", "FromAddress", typeof(System.String), false, false, false, false,  (int)EmailOutboundFieldIndex.FromAddress, 200, 0, 0);
			this.AddElementFieldInfo("EmailOutboundEntity", "ToList", typeof(System.String), false, false, false, false,  (int)EmailOutboundFieldIndex.ToList, 2147483647, 0, 0);
			this.AddElementFieldInfo("EmailOutboundEntity", "CcList", typeof(System.String), false, false, false, false,  (int)EmailOutboundFieldIndex.CcList, 2147483647, 0, 0);
			this.AddElementFieldInfo("EmailOutboundEntity", "BccList", typeof(System.String), false, false, false, false,  (int)EmailOutboundFieldIndex.BccList, 2147483647, 0, 0);
			this.AddElementFieldInfo("EmailOutboundEntity", "Subject", typeof(System.String), false, false, false, false,  (int)EmailOutboundFieldIndex.Subject, 300, 0, 0);
			this.AddElementFieldInfo("EmailOutboundEntity", "HtmlPartResourceID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)EmailOutboundFieldIndex.HtmlPartResourceID, 0, 0, 19);
			this.AddElementFieldInfo("EmailOutboundEntity", "PlainPartResourceID", typeof(System.Int64), false, false, false, false,  (int)EmailOutboundFieldIndex.PlainPartResourceID, 0, 0, 19);
			this.AddElementFieldInfo("EmailOutboundEntity", "Encoding", typeof(System.String), false, false, false, true,  (int)EmailOutboundFieldIndex.Encoding, 20, 0, 0);
			this.AddElementFieldInfo("EmailOutboundEntity", "ComposedDate", typeof(System.DateTime), false, false, false, false,  (int)EmailOutboundFieldIndex.ComposedDate, 0, 0, 0);
			this.AddElementFieldInfo("EmailOutboundEntity", "SentDate", typeof(System.DateTime), false, false, false, false,  (int)EmailOutboundFieldIndex.SentDate, 0, 0, 0);
			this.AddElementFieldInfo("EmailOutboundEntity", "DontSendBefore", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)EmailOutboundFieldIndex.DontSendBefore, 0, 0, 0);
			this.AddElementFieldInfo("EmailOutboundEntity", "SendStatus", typeof(System.Int32), false, false, false, false,  (int)EmailOutboundFieldIndex.SendStatus, 0, 0, 10);
			this.AddElementFieldInfo("EmailOutboundEntity", "SendAttemptCount", typeof(System.Int32), false, false, false, false,  (int)EmailOutboundFieldIndex.SendAttemptCount, 0, 0, 10);
			this.AddElementFieldInfo("EmailOutboundEntity", "SendAttemptLastError", typeof(System.String), false, false, false, false,  (int)EmailOutboundFieldIndex.SendAttemptLastError, 300, 0, 0);
		}
		/// <summary>Inits EmailOutboundRelationEntity's FieldInfo objects</summary>
		private void InitEmailOutboundRelationEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(EmailOutboundRelationFieldIndex), "EmailOutboundRelationEntity");
			this.AddElementFieldInfo("EmailOutboundRelationEntity", "EmailOutboundRelationID", typeof(System.Int64), true, false, true, false,  (int)EmailOutboundRelationFieldIndex.EmailOutboundRelationID, 0, 0, 19);
			this.AddElementFieldInfo("EmailOutboundRelationEntity", "EmailOutboundID", typeof(System.Int64), false, true, false, false,  (int)EmailOutboundRelationFieldIndex.EmailOutboundID, 0, 0, 19);
			this.AddElementFieldInfo("EmailOutboundRelationEntity", "EntityID", typeof(System.Int64), false, false, false, false,  (int)EmailOutboundRelationFieldIndex.EntityID, 0, 0, 19);
			this.AddElementFieldInfo("EmailOutboundRelationEntity", "RelationType", typeof(System.Int32), false, false, false, false,  (int)EmailOutboundRelationFieldIndex.RelationType, 0, 0, 10);
		}
		/// <summary>Inits EndiciaAccountEntity's FieldInfo objects</summary>
		private void InitEndiciaAccountEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(EndiciaAccountFieldIndex), "EndiciaAccountEntity");
			this.AddElementFieldInfo("EndiciaAccountEntity", "EndiciaAccountID", typeof(System.Int64), true, false, true, false,  (int)EndiciaAccountFieldIndex.EndiciaAccountID, 0, 0, 19);
			this.AddElementFieldInfo("EndiciaAccountEntity", "EndiciaReseller", typeof(System.Int32), false, false, false, false,  (int)EndiciaAccountFieldIndex.EndiciaReseller, 0, 0, 10);
			this.AddElementFieldInfo("EndiciaAccountEntity", "AccountNumber", typeof(System.String), false, false, false, true,  (int)EndiciaAccountFieldIndex.AccountNumber, 50, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "SignupConfirmation", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.SignupConfirmation, 30, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "WebPassword", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.WebPassword, 250, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "ApiInitialPassword", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.ApiInitialPassword, 250, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "ApiUserPassword", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.ApiUserPassword, 250, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "AccountType", typeof(System.Int32), false, false, false, false,  (int)EndiciaAccountFieldIndex.AccountType, 0, 0, 10);
			this.AddElementFieldInfo("EndiciaAccountEntity", "TestAccount", typeof(System.Boolean), false, false, false, false,  (int)EndiciaAccountFieldIndex.TestAccount, 0, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "CreatedByShipWorks", typeof(System.Boolean), false, false, false, false,  (int)EndiciaAccountFieldIndex.CreatedByShipWorks, 0, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "Description", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.Description, 50, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "FirstName", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.FirstName, 30, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "LastName", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.LastName, 30, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "Company", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.Company, 30, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "Street1", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.Street1, 60, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "Street2", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.Street2, 60, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "Street3", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.Street3, 60, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "City", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.City, 50, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.StateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.PostalCode, 20, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.CountryCode, 50, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "Phone", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.Phone, 25, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "Fax", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.Fax, 35, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "Email", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.Email, 100, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "MailingPostalCode", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.MailingPostalCode, 20, 0, 0);
			this.AddElementFieldInfo("EndiciaAccountEntity", "ScanFormAddressSource", typeof(System.Int32), false, false, false, false,  (int)EndiciaAccountFieldIndex.ScanFormAddressSource, 0, 0, 10);
		}
		/// <summary>Inits EndiciaProfileEntity's FieldInfo objects</summary>
		private void InitEndiciaProfileEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(EndiciaProfileFieldIndex), "EndiciaProfileEntity");
			this.AddElementFieldInfo("EndiciaProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)EndiciaProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			this.AddElementFieldInfo("EndiciaProfileEntity", "EndiciaAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)EndiciaProfileFieldIndex.EndiciaAccountID, 0, 0, 19);
			this.AddElementFieldInfo("EndiciaProfileEntity", "StealthPostage", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)EndiciaProfileFieldIndex.StealthPostage, 0, 0, 0);
			this.AddElementFieldInfo("EndiciaProfileEntity", "ReferenceID", typeof(System.String), false, false, false, true,  (int)EndiciaProfileFieldIndex.ReferenceID, 300, 0, 0);
			this.AddElementFieldInfo("EndiciaProfileEntity", "ScanBasedReturn", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)EndiciaProfileFieldIndex.ScanBasedReturn, 0, 0, 0);
		}
		/// <summary>Inits EndiciaScanFormEntity's FieldInfo objects</summary>
		private void InitEndiciaScanFormEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(EndiciaScanFormFieldIndex), "EndiciaScanFormEntity");
			this.AddElementFieldInfo("EndiciaScanFormEntity", "EndiciaScanFormID", typeof(System.Int64), true, false, true, false,  (int)EndiciaScanFormFieldIndex.EndiciaScanFormID, 0, 0, 19);
			this.AddElementFieldInfo("EndiciaScanFormEntity", "EndiciaAccountID", typeof(System.Int64), false, false, false, false,  (int)EndiciaScanFormFieldIndex.EndiciaAccountID, 0, 0, 19);
			this.AddElementFieldInfo("EndiciaScanFormEntity", "EndiciaAccountNumber", typeof(System.String), false, false, false, false,  (int)EndiciaScanFormFieldIndex.EndiciaAccountNumber, 50, 0, 0);
			this.AddElementFieldInfo("EndiciaScanFormEntity", "SubmissionID", typeof(System.String), false, false, false, false,  (int)EndiciaScanFormFieldIndex.SubmissionID, 100, 0, 0);
			this.AddElementFieldInfo("EndiciaScanFormEntity", "CreatedDate", typeof(System.DateTime), false, false, false, false,  (int)EndiciaScanFormFieldIndex.CreatedDate, 0, 0, 0);
			this.AddElementFieldInfo("EndiciaScanFormEntity", "ScanFormBatchID", typeof(System.Int64), false, true, false, false,  (int)EndiciaScanFormFieldIndex.ScanFormBatchID, 0, 0, 19);
			this.AddElementFieldInfo("EndiciaScanFormEntity", "Description", typeof(System.String), false, false, false, false,  (int)EndiciaScanFormFieldIndex.Description, 100, 0, 0);
		}
		/// <summary>Inits EndiciaShipmentEntity's FieldInfo objects</summary>
		private void InitEndiciaShipmentEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(EndiciaShipmentFieldIndex), "EndiciaShipmentEntity");
			this.AddElementFieldInfo("EndiciaShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)EndiciaShipmentFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("EndiciaShipmentEntity", "EndiciaAccountID", typeof(System.Int64), false, false, false, false,  (int)EndiciaShipmentFieldIndex.EndiciaAccountID, 0, 0, 19);
			this.AddElementFieldInfo("EndiciaShipmentEntity", "OriginalEndiciaAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)EndiciaShipmentFieldIndex.OriginalEndiciaAccountID, 0, 0, 19);
			this.AddElementFieldInfo("EndiciaShipmentEntity", "StealthPostage", typeof(System.Boolean), false, false, false, false,  (int)EndiciaShipmentFieldIndex.StealthPostage, 0, 0, 0);
			this.AddElementFieldInfo("EndiciaShipmentEntity", "ReferenceID", typeof(System.String), false, false, false, false,  (int)EndiciaShipmentFieldIndex.ReferenceID, 300, 0, 0);
			this.AddElementFieldInfo("EndiciaShipmentEntity", "TransactionID", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EndiciaShipmentFieldIndex.TransactionID, 0, 0, 10);
			this.AddElementFieldInfo("EndiciaShipmentEntity", "RefundFormID", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EndiciaShipmentFieldIndex.RefundFormID, 0, 0, 10);
			this.AddElementFieldInfo("EndiciaShipmentEntity", "ScanFormBatchID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)EndiciaShipmentFieldIndex.ScanFormBatchID, 0, 0, 19);
			this.AddElementFieldInfo("EndiciaShipmentEntity", "ScanBasedReturn", typeof(System.Boolean), false, false, false, false,  (int)EndiciaShipmentFieldIndex.ScanBasedReturn, 0, 0, 0);
			this.AddElementFieldInfo("EndiciaShipmentEntity", "RequestedLabelFormat", typeof(System.Int32), false, false, false, false,  (int)EndiciaShipmentFieldIndex.RequestedLabelFormat, 0, 0, 10);
		}
		/// <summary>Inits EtsyOrderEntity's FieldInfo objects</summary>
		private void InitEtsyOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(EtsyOrderFieldIndex), "EtsyOrderEntity");
			this.AddElementFieldInfo("EtsyOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)EtsyOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("EtsyOrderEntity", "WasPaid", typeof(System.Boolean), false, false, false, false,  (int)EtsyOrderFieldIndex.WasPaid, 0, 0, 0);
			this.AddElementFieldInfo("EtsyOrderEntity", "WasShipped", typeof(System.Boolean), false, false, false, false,  (int)EtsyOrderFieldIndex.WasShipped, 0, 0, 0);
		}
		/// <summary>Inits EtsyStoreEntity's FieldInfo objects</summary>
		private void InitEtsyStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(EtsyStoreFieldIndex), "EtsyStoreEntity");
			this.AddElementFieldInfo("EtsyStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)EtsyStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("EtsyStoreEntity", "EtsyShopID", typeof(System.Int64), false, false, false, false,  (int)EtsyStoreFieldIndex.EtsyShopID, 0, 0, 19);
			this.AddElementFieldInfo("EtsyStoreEntity", "EtsyLoginName", typeof(System.String), false, false, false, false,  (int)EtsyStoreFieldIndex.EtsyLoginName, 255, 0, 0);
			this.AddElementFieldInfo("EtsyStoreEntity", "EtsyStoreName", typeof(System.String), false, false, false, false,  (int)EtsyStoreFieldIndex.EtsyStoreName, 255, 0, 0);
			this.AddElementFieldInfo("EtsyStoreEntity", "OAuthToken", typeof(System.String), false, false, false, false,  (int)EtsyStoreFieldIndex.OAuthToken, 50, 0, 0);
			this.AddElementFieldInfo("EtsyStoreEntity", "OAuthTokenSecret", typeof(System.String), false, false, false, false,  (int)EtsyStoreFieldIndex.OAuthTokenSecret, 50, 0, 0);
		}
		/// <summary>Inits ExcludedPackageTypeEntity's FieldInfo objects</summary>
		private void InitExcludedPackageTypeEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ExcludedPackageTypeFieldIndex), "ExcludedPackageTypeEntity");
			this.AddElementFieldInfo("ExcludedPackageTypeEntity", "ShipmentType", typeof(System.Int32), true, false, false, false,  (int)ExcludedPackageTypeFieldIndex.ShipmentType, 0, 0, 10);
			this.AddElementFieldInfo("ExcludedPackageTypeEntity", "PackageType", typeof(System.Int32), true, false, false, false,  (int)ExcludedPackageTypeFieldIndex.PackageType, 0, 0, 10);
		}
		/// <summary>Inits ExcludedServiceTypeEntity's FieldInfo objects</summary>
		private void InitExcludedServiceTypeEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ExcludedServiceTypeFieldIndex), "ExcludedServiceTypeEntity");
			this.AddElementFieldInfo("ExcludedServiceTypeEntity", "ShipmentType", typeof(System.Int32), true, false, false, false,  (int)ExcludedServiceTypeFieldIndex.ShipmentType, 0, 0, 10);
			this.AddElementFieldInfo("ExcludedServiceTypeEntity", "ServiceType", typeof(System.Int32), true, false, false, false,  (int)ExcludedServiceTypeFieldIndex.ServiceType, 0, 0, 10);
		}
		/// <summary>Inits FedExAccountEntity's FieldInfo objects</summary>
		private void InitFedExAccountEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(FedExAccountFieldIndex), "FedExAccountEntity");
			this.AddElementFieldInfo("FedExAccountEntity", "FedExAccountID", typeof(System.Int64), true, false, true, false,  (int)FedExAccountFieldIndex.FedExAccountID, 0, 0, 19);
			this.AddElementFieldInfo("FedExAccountEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)FedExAccountFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "Description", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.Description, 50, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "AccountNumber", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.AccountNumber, 12, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "SignatureRelease", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.SignatureRelease, 10, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "MeterNumber", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.MeterNumber, 50, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "SmartPostHubList", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.SmartPostHubList, 2147483647, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "FirstName", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.FirstName, 30, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "MiddleName", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.MiddleName, 30, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "LastName", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.LastName, 30, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "Company", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.Company, 35, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "Street1", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.Street1, 60, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "Street2", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.Street2, 60, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "City", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.City, 50, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.StateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.PostalCode, 20, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.CountryCode, 50, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "Phone", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.Phone, 25, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "Email", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.Email, 100, 0, 0);
			this.AddElementFieldInfo("FedExAccountEntity", "Website", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.Website, 50, 0, 0);
		}
		/// <summary>Inits FedExEndOfDayCloseEntity's FieldInfo objects</summary>
		private void InitFedExEndOfDayCloseEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(FedExEndOfDayCloseFieldIndex), "FedExEndOfDayCloseEntity");
			this.AddElementFieldInfo("FedExEndOfDayCloseEntity", "FedExEndOfDayCloseID", typeof(System.Int64), true, false, true, false,  (int)FedExEndOfDayCloseFieldIndex.FedExEndOfDayCloseID, 0, 0, 19);
			this.AddElementFieldInfo("FedExEndOfDayCloseEntity", "FedExAccountID", typeof(System.Int64), false, false, false, false,  (int)FedExEndOfDayCloseFieldIndex.FedExAccountID, 0, 0, 19);
			this.AddElementFieldInfo("FedExEndOfDayCloseEntity", "AccountNumber", typeof(System.String), false, false, false, false,  (int)FedExEndOfDayCloseFieldIndex.AccountNumber, 50, 0, 0);
			this.AddElementFieldInfo("FedExEndOfDayCloseEntity", "CloseDate", typeof(System.DateTime), false, false, false, false,  (int)FedExEndOfDayCloseFieldIndex.CloseDate, 0, 0, 0);
			this.AddElementFieldInfo("FedExEndOfDayCloseEntity", "IsSmartPost", typeof(System.Boolean), false, false, false, false,  (int)FedExEndOfDayCloseFieldIndex.IsSmartPost, 0, 0, 0);
		}
		/// <summary>Inits FedExPackageEntity's FieldInfo objects</summary>
		private void InitFedExPackageEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(FedExPackageFieldIndex), "FedExPackageEntity");
			this.AddElementFieldInfo("FedExPackageEntity", "FedExPackageID", typeof(System.Int64), true, false, true, false,  (int)FedExPackageFieldIndex.FedExPackageID, 0, 0, 19);
			this.AddElementFieldInfo("FedExPackageEntity", "ShipmentID", typeof(System.Int64), false, true, false, false,  (int)FedExPackageFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("FedExPackageEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)FedExPackageFieldIndex.Weight, 0, 0, 38);
			this.AddElementFieldInfo("FedExPackageEntity", "DimsProfileID", typeof(System.Int64), false, false, false, false,  (int)FedExPackageFieldIndex.DimsProfileID, 0, 0, 19);
			this.AddElementFieldInfo("FedExPackageEntity", "DimsLength", typeof(System.Double), false, false, false, false,  (int)FedExPackageFieldIndex.DimsLength, 0, 0, 38);
			this.AddElementFieldInfo("FedExPackageEntity", "DimsWidth", typeof(System.Double), false, false, false, false,  (int)FedExPackageFieldIndex.DimsWidth, 0, 0, 38);
			this.AddElementFieldInfo("FedExPackageEntity", "DimsHeight", typeof(System.Double), false, false, false, false,  (int)FedExPackageFieldIndex.DimsHeight, 0, 0, 38);
			this.AddElementFieldInfo("FedExPackageEntity", "DimsWeight", typeof(System.Double), false, false, false, false,  (int)FedExPackageFieldIndex.DimsWeight, 0, 0, 38);
			this.AddElementFieldInfo("FedExPackageEntity", "DimsAddWeight", typeof(System.Boolean), false, false, false, false,  (int)FedExPackageFieldIndex.DimsAddWeight, 0, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "SkidPieces", typeof(System.Int32), false, false, false, false,  (int)FedExPackageFieldIndex.SkidPieces, 0, 0, 10);
			this.AddElementFieldInfo("FedExPackageEntity", "Insurance", typeof(System.Boolean), false, false, false, false,  (int)FedExPackageFieldIndex.Insurance, 0, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "InsuranceValue", typeof(System.Decimal), false, false, false, false,  (int)FedExPackageFieldIndex.InsuranceValue, 0, 4, 19);
			this.AddElementFieldInfo("FedExPackageEntity", "InsurancePennyOne", typeof(System.Boolean), false, false, false, false,  (int)FedExPackageFieldIndex.InsurancePennyOne, 0, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "DeclaredValue", typeof(System.Decimal), false, false, false, false,  (int)FedExPackageFieldIndex.DeclaredValue, 0, 4, 19);
			this.AddElementFieldInfo("FedExPackageEntity", "TrackingNumber", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.TrackingNumber, 50, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "PriorityAlert", typeof(System.Boolean), false, false, false, false,  (int)FedExPackageFieldIndex.PriorityAlert, 0, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "PriorityAlertEnhancementType", typeof(System.Int32), false, false, false, false,  (int)FedExPackageFieldIndex.PriorityAlertEnhancementType, 0, 0, 10);
			this.AddElementFieldInfo("FedExPackageEntity", "PriorityAlertDetailContent", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.PriorityAlertDetailContent, 1024, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "DryIceWeight", typeof(System.Double), false, false, false, false,  (int)FedExPackageFieldIndex.DryIceWeight, 0, 0, 38);
			this.AddElementFieldInfo("FedExPackageEntity", "ContainsAlcohol", typeof(System.Boolean), false, false, false, false,  (int)FedExPackageFieldIndex.ContainsAlcohol, 0, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "DangerousGoodsEnabled", typeof(System.Boolean), false, false, false, false,  (int)FedExPackageFieldIndex.DangerousGoodsEnabled, 0, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "DangerousGoodsType", typeof(System.Int32), false, false, false, false,  (int)FedExPackageFieldIndex.DangerousGoodsType, 0, 0, 10);
			this.AddElementFieldInfo("FedExPackageEntity", "DangerousGoodsAccessibilityType", typeof(System.Int32), false, false, false, false,  (int)FedExPackageFieldIndex.DangerousGoodsAccessibilityType, 0, 0, 10);
			this.AddElementFieldInfo("FedExPackageEntity", "DangerousGoodsCargoAircraftOnly", typeof(System.Boolean), false, false, false, false,  (int)FedExPackageFieldIndex.DangerousGoodsCargoAircraftOnly, 0, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "DangerousGoodsEmergencyContactPhone", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.DangerousGoodsEmergencyContactPhone, 16, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "DangerousGoodsOfferor", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.DangerousGoodsOfferor, 128, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "DangerousGoodsPackagingCount", typeof(System.Int32), false, false, false, false,  (int)FedExPackageFieldIndex.DangerousGoodsPackagingCount, 0, 0, 10);
			this.AddElementFieldInfo("FedExPackageEntity", "HazardousMaterialNumber", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.HazardousMaterialNumber, 16, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "HazardousMaterialClass", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.HazardousMaterialClass, 8, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "HazardousMaterialProperName", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.HazardousMaterialProperName, 64, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "HazardousMaterialPackingGroup", typeof(System.Int32), false, false, false, false,  (int)FedExPackageFieldIndex.HazardousMaterialPackingGroup, 0, 0, 10);
			this.AddElementFieldInfo("FedExPackageEntity", "HazardousMaterialQuantityValue", typeof(System.Double), false, false, false, false,  (int)FedExPackageFieldIndex.HazardousMaterialQuantityValue, 0, 0, 38);
			this.AddElementFieldInfo("FedExPackageEntity", "HazardousMaterialQuanityUnits", typeof(System.Int32), false, false, false, false,  (int)FedExPackageFieldIndex.HazardousMaterialQuanityUnits, 0, 0, 10);
			this.AddElementFieldInfo("FedExPackageEntity", "HazardousMaterialTechnicalName", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.HazardousMaterialTechnicalName, 64, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "SignatoryContactName", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.SignatoryContactName, 100, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "SignatoryTitle", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.SignatoryTitle, 100, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "SignatoryPlace", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.SignatoryPlace, 100, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "AlcoholRecipientType", typeof(System.Int32), false, false, false, false,  (int)FedExPackageFieldIndex.AlcoholRecipientType, 0, 0, 10);
			this.AddElementFieldInfo("FedExPackageEntity", "ContainerType", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.ContainerType, 100, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "NumberOfContainers", typeof(System.Int32), false, false, false, false,  (int)FedExPackageFieldIndex.NumberOfContainers, 0, 0, 10);
			this.AddElementFieldInfo("FedExPackageEntity", "PackingDetailsCargoAircraftOnly", typeof(System.Boolean), false, false, false, false,  (int)FedExPackageFieldIndex.PackingDetailsCargoAircraftOnly, 0, 0, 0);
			this.AddElementFieldInfo("FedExPackageEntity", "PackingDetailsPackingInstructions", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.PackingDetailsPackingInstructions, 50, 0, 0);
		}
		/// <summary>Inits FedExProfileEntity's FieldInfo objects</summary>
		private void InitFedExProfileEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(FedExProfileFieldIndex), "FedExProfileEntity");
			this.AddElementFieldInfo("FedExProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)FedExProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			this.AddElementFieldInfo("FedExProfileEntity", "FedExAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)FedExProfileFieldIndex.FedExAccountID, 0, 0, 19);
			this.AddElementFieldInfo("FedExProfileEntity", "Service", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.Service, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfileEntity", "Signature", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.Signature, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfileEntity", "PackagingType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.PackagingType, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfileEntity", "NonStandardContainer", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfileFieldIndex.NonStandardContainer, 0, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "ReferenceCustomer", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.ReferenceCustomer, 300, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "ReferenceInvoice", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.ReferenceInvoice, 300, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "ReferencePO", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.ReferencePO, 300, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "ReferenceShipmentIntegrity", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.ReferenceShipmentIntegrity, 300, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "PayorTransportType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.PayorTransportType, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfileEntity", "PayorTransportAccount", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.PayorTransportAccount, 12, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "PayorDutiesType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.PayorDutiesType, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfileEntity", "PayorDutiesAccount", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.PayorDutiesAccount, 12, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "SaturdayDelivery", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfileFieldIndex.SaturdayDelivery, 0, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "EmailNotifySender", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.EmailNotifySender, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfileEntity", "EmailNotifyRecipient", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.EmailNotifyRecipient, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfileEntity", "EmailNotifyOther", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.EmailNotifyOther, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfileEntity", "EmailNotifyOtherAddress", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.EmailNotifyOtherAddress, 100, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "EmailNotifyMessage", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.EmailNotifyMessage, 120, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "ResidentialDetermination", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.ResidentialDetermination, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfileEntity", "SmartPostIndicia", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.SmartPostIndicia, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfileEntity", "SmartPostEndorsement", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.SmartPostEndorsement, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfileEntity", "SmartPostConfirmation", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfileFieldIndex.SmartPostConfirmation, 0, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "SmartPostCustomerManifest", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.SmartPostCustomerManifest, 50, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "SmartPostHubID", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.SmartPostHubID, 10, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "EmailNotifyBroker", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.EmailNotifyBroker, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfileEntity", "DropoffType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.DropoffType, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfileEntity", "OriginResidentialDetermination", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.OriginResidentialDetermination, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfileEntity", "PayorTransportName", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.PayorTransportName, 60, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "ReturnType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.ReturnType, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfileEntity", "RmaNumber", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.RmaNumber, 30, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "RmaReason", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.RmaReason, 60, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "ReturnSaturdayPickup", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfileFieldIndex.ReturnSaturdayPickup, 0, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "ReturnsClearance", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfileFieldIndex.ReturnsClearance, 0, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "ReferenceFIMS", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.ReferenceFIMS, 300, 0, 0);
			this.AddElementFieldInfo("FedExProfileEntity", "ThirdPartyConsignee", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfileFieldIndex.ThirdPartyConsignee, 0, 0, 0);
		}
		/// <summary>Inits FedExProfilePackageEntity's FieldInfo objects</summary>
		private void InitFedExProfilePackageEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(FedExProfilePackageFieldIndex), "FedExProfilePackageEntity");
			this.AddElementFieldInfo("FedExProfilePackageEntity", "FedExProfilePackageID", typeof(System.Int64), true, false, true, false,  (int)FedExProfilePackageFieldIndex.FedExProfilePackageID, 0, 0, 19);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "ShippingProfileID", typeof(System.Int64), false, true, false, false,  (int)FedExProfilePackageFieldIndex.ShippingProfileID, 0, 0, 19);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "Weight", typeof(Nullable<System.Double>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.Weight, 0, 0, 38);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "DimsProfileID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DimsProfileID, 0, 0, 19);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "DimsLength", typeof(Nullable<System.Double>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DimsLength, 0, 0, 38);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "DimsWidth", typeof(Nullable<System.Double>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DimsWidth, 0, 0, 38);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "DimsHeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DimsHeight, 0, 0, 38);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "DimsWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DimsWeight, 0, 0, 38);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "DimsAddWeight", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DimsAddWeight, 0, 0, 0);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "PriorityAlert", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.PriorityAlert, 0, 0, 0);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "PriorityAlertEnhancementType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.PriorityAlertEnhancementType, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "PriorityAlertDetailContent", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.PriorityAlertDetailContent, 1024, 0, 0);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "DryIceWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DryIceWeight, 0, 0, 38);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "ContainsAlcohol", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.ContainsAlcohol, 0, 0, 0);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "DangerousGoodsEnabled", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DangerousGoodsEnabled, 0, 0, 0);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "DangerousGoodsType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DangerousGoodsType, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "DangerousGoodsAccessibilityType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DangerousGoodsAccessibilityType, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "DangerousGoodsCargoAircraftOnly", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DangerousGoodsCargoAircraftOnly, 0, 0, 0);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "DangerousGoodsEmergencyContactPhone", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DangerousGoodsEmergencyContactPhone, 16, 0, 0);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "DangerousGoodsOfferor", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DangerousGoodsOfferor, 128, 0, 0);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "DangerousGoodsPackagingCount", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DangerousGoodsPackagingCount, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "HazardousMaterialNumber", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.HazardousMaterialNumber, 16, 0, 0);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "HazardousMaterialClass", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.HazardousMaterialClass, 8, 0, 0);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "HazardousMaterialProperName", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.HazardousMaterialProperName, 64, 0, 0);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "HazardousMaterialPackingGroup", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.HazardousMaterialPackingGroup, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "HazardousMaterialQuantityValue", typeof(Nullable<System.Double>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.HazardousMaterialQuantityValue, 0, 0, 38);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "HazardousMaterialQuanityUnits", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.HazardousMaterialQuanityUnits, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "SignatoryContactName", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.SignatoryContactName, 100, 0, 0);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "SignatoryTitle", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.SignatoryTitle, 100, 0, 0);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "SignatoryPlace", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.SignatoryPlace, 100, 0, 0);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "ContainerType", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.ContainerType, 100, 0, 0);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "NumberOfContainers", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.NumberOfContainers, 0, 0, 10);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "PackingDetailsCargoAircraftOnly", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.PackingDetailsCargoAircraftOnly, 0, 0, 0);
			this.AddElementFieldInfo("FedExProfilePackageEntity", "PackingDetailsPackingInstructions", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.PackingDetailsPackingInstructions, 50, 0, 0);
		}
		/// <summary>Inits FedExShipmentEntity's FieldInfo objects</summary>
		private void InitFedExShipmentEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(FedExShipmentFieldIndex), "FedExShipmentEntity");
			this.AddElementFieldInfo("FedExShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)FedExShipmentFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("FedExShipmentEntity", "FedExAccountID", typeof(System.Int64), false, false, false, false,  (int)FedExShipmentFieldIndex.FedExAccountID, 0, 0, 19);
			this.AddElementFieldInfo("FedExShipmentEntity", "MasterFormID", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.MasterFormID, 4, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "Service", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.Service, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "Signature", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.Signature, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "PackagingType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.PackagingType, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "NonStandardContainer", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.NonStandardContainer, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ReferenceCustomer", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ReferenceCustomer, 300, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ReferenceInvoice", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ReferenceInvoice, 300, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ReferencePO", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ReferencePO, 300, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ReferenceShipmentIntegrity", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ReferenceShipmentIntegrity, 300, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "PayorTransportType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.PayorTransportType, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "PayorTransportName", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.PayorTransportName, 60, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "PayorTransportAccount", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.PayorTransportAccount, 12, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "PayorDutiesType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.PayorDutiesType, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "PayorDutiesAccount", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.PayorDutiesAccount, 12, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "PayorDutiesName", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.PayorDutiesName, 60, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "PayorDutiesCountryCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.PayorDutiesCountryCode, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "SaturdayDelivery", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.SaturdayDelivery, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HomeDeliveryType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.HomeDeliveryType, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "HomeDeliveryInstructions", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.HomeDeliveryInstructions, 74, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HomeDeliveryDate", typeof(System.DateTime), false, false, false, false,  (int)FedExShipmentFieldIndex.HomeDeliveryDate, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HomeDeliveryPhone", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.HomeDeliveryPhone, 24, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "FreightInsidePickup", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.FreightInsidePickup, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "FreightInsideDelivery", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.FreightInsideDelivery, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "FreightBookingNumber", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.FreightBookingNumber, 12, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "FreightLoadAndCount", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.FreightLoadAndCount, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "EmailNotifyBroker", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.EmailNotifyBroker, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "EmailNotifySender", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.EmailNotifySender, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "EmailNotifyRecipient", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.EmailNotifyRecipient, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "EmailNotifyOther", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.EmailNotifyOther, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "EmailNotifyOtherAddress", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.EmailNotifyOtherAddress, 100, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "EmailNotifyMessage", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.EmailNotifyMessage, 120, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodEnabled", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.CodEnabled, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodAmount", typeof(System.Decimal), false, false, false, false,  (int)FedExShipmentFieldIndex.CodAmount, 0, 4, 19);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodPaymentType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CodPaymentType, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodAddFreight", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.CodAddFreight, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodOriginID", typeof(System.Int64), false, false, false, false,  (int)FedExShipmentFieldIndex.CodOriginID, 0, 0, 19);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodFirstName", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodFirstName, 30, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodLastName", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodLastName, 30, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodCompany", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodCompany, 35, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodStreet1", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodStreet1, 60, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodStreet2", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodStreet2, 60, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodStreet3", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodStreet3, 60, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodCity", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodCity, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodStateProvCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodStateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodPostalCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodPostalCode, 20, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodCountryCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodCountryCode, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodPhone", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodPhone, 25, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodTrackingNumber", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodTrackingNumber, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodTrackingFormID", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodTrackingFormID, 4, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodTIN", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodTIN, 24, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodChargeBasis", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CodChargeBasis, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "CodAccountNumber", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodAccountNumber, 25, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "BrokerEnabled", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerEnabled, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "BrokerAccount", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerAccount, 12, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "BrokerFirstName", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerFirstName, 30, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "BrokerLastName", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerLastName, 30, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "BrokerCompany", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerCompany, 35, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "BrokerStreet1", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerStreet1, 60, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "BrokerStreet2", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerStreet2, 60, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "BrokerStreet3", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerStreet3, 60, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "BrokerCity", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerCity, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "BrokerStateProvCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerStateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "BrokerPostalCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerPostalCode, 20, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "BrokerCountryCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerCountryCode, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "BrokerPhone", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerPhone, 25, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "BrokerPhoneExtension", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerPhoneExtension, 8, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "BrokerEmail", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerEmail, 100, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CustomsAdmissibilityPackaging", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsAdmissibilityPackaging, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "CustomsRecipientTIN", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsRecipientTIN, 24, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CustomsDocumentsOnly", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsDocumentsOnly, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CustomsDocumentsDescription", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsDocumentsDescription, 150, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CustomsExportFilingOption", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsExportFilingOption, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "CustomsAESEEI", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsAESEEI, 100, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CustomsRecipientIdentificationType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsRecipientIdentificationType, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "CustomsRecipientIdentificationValue", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsRecipientIdentificationValue, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CustomsOptionsType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsOptionsType, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "CustomsOptionsDesription", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsOptionsDesription, 32, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoice", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoice, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoiceFileElectronically", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoiceFileElectronically, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoiceTermsOfSale", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoiceTermsOfSale, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoicePurpose", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoicePurpose, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoiceComments", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoiceComments, 200, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoiceFreight", typeof(System.Decimal), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoiceFreight, 0, 4, 19);
			this.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoiceInsurance", typeof(System.Decimal), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoiceInsurance, 0, 4, 19);
			this.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoiceOther", typeof(System.Decimal), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoiceOther, 0, 4, 19);
			this.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoiceReference", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoiceReference, 300, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ImporterOfRecord", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterOfRecord, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ImporterAccount", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterAccount, 12, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ImporterTIN", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterTIN, 24, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ImporterFirstName", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterFirstName, 30, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ImporterLastName", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterLastName, 30, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ImporterCompany", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterCompany, 35, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ImporterStreet1", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterStreet1, 60, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ImporterStreet2", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterStreet2, 60, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ImporterStreet3", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterStreet3, 60, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ImporterCity", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterCity, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ImporterStateProvCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterStateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ImporterPostalCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterPostalCode, 10, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ImporterCountryCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterCountryCode, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ImporterPhone", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterPhone, 25, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "SmartPostIndicia", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.SmartPostIndicia, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "SmartPostEndorsement", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.SmartPostEndorsement, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "SmartPostConfirmation", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.SmartPostConfirmation, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "SmartPostCustomerManifest", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.SmartPostCustomerManifest, 300, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "SmartPostHubID", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.SmartPostHubID, 10, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "SmartPostUspsApplicationId", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.SmartPostUspsApplicationId, 10, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "DropoffType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.DropoffType, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "OriginResidentialDetermination", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.OriginResidentialDetermination, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "FedExHoldAtLocationEnabled", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.FedExHoldAtLocationEnabled, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldLocationId", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldLocationId, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldLocationType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldLocationType, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldContactId", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldContactId, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldPersonName", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldPersonName, 100, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldTitle", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldTitle, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldCompanyName", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldCompanyName, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldPhoneNumber", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldPhoneNumber, 30, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldPhoneExtension", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldPhoneExtension, 10, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldPagerNumber", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldPagerNumber, 30, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldFaxNumber", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldFaxNumber, 30, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldEmailAddress", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldEmailAddress, 100, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldStreet1", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldStreet1, 250, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldStreet2", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldStreet2, 250, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldStreet3", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldStreet3, 250, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldCity", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldCity, 100, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldStateOrProvinceCode", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldStateOrProvinceCode, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldPostalCode", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldPostalCode, 20, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldUrbanizationCode", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldUrbanizationCode, 20, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldCountryCode", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldCountryCode, 20, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "HoldResidential", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldResidential, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CustomsNaftaEnabled", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsNaftaEnabled, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CustomsNaftaPreferenceType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsNaftaPreferenceType, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "CustomsNaftaDeterminationCode", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsNaftaDeterminationCode, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "CustomsNaftaProducerId", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsNaftaProducerId, 20, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "CustomsNaftaNetCostMethod", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsNaftaNetCostMethod, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "ReturnType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.ReturnType, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "RmaNumber", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.RmaNumber, 30, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "RmaReason", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.RmaReason, 60, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ReturnSaturdayPickup", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.ReturnSaturdayPickup, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "TrafficInArmsLicenseNumber", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.TrafficInArmsLicenseNumber, 32, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "IntlExportDetailType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.IntlExportDetailType, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "IntlExportDetailForeignTradeZoneCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.IntlExportDetailForeignTradeZoneCode, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "IntlExportDetailEntryNumber", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.IntlExportDetailEntryNumber, 20, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "IntlExportDetailLicenseOrPermitNumber", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.IntlExportDetailLicenseOrPermitNumber, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "IntlExportDetailLicenseOrPermitExpirationDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)FedExShipmentFieldIndex.IntlExportDetailLicenseOrPermitExpirationDate, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "WeightUnitType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.WeightUnitType, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "LinearUnitType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.LinearUnitType, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "RequestedLabelFormat", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.RequestedLabelFormat, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "FimsAirWaybill", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.FimsAirWaybill, 50, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ReturnsClearance", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.ReturnsClearance, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "MaskedData", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExShipmentFieldIndex.MaskedData, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "ReferenceFIMS", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ReferenceFIMS, 300, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "ThirdPartyConsignee", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.ThirdPartyConsignee, 0, 0, 0);
			this.AddElementFieldInfo("FedExShipmentEntity", "Currency", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExShipmentFieldIndex.Currency, 0, 0, 10);
			this.AddElementFieldInfo("FedExShipmentEntity", "InternationalTrafficInArmsService", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExShipmentFieldIndex.InternationalTrafficInArmsService, 0, 0, 0);
		}
		/// <summary>Inits FilterEntity's FieldInfo objects</summary>
		private void InitFilterEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(FilterFieldIndex), "FilterEntity");
			this.AddElementFieldInfo("FilterEntity", "FilterID", typeof(System.Int64), true, false, true, false,  (int)FilterFieldIndex.FilterID, 0, 0, 19);
			this.AddElementFieldInfo("FilterEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)FilterFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("FilterEntity", "Name", typeof(System.String), false, false, false, false,  (int)FilterFieldIndex.Name, 50, 0, 0);
			this.AddElementFieldInfo("FilterEntity", "FilterTarget", typeof(System.Int32), false, false, false, false,  (int)FilterFieldIndex.FilterTarget, 0, 0, 10);
			this.AddElementFieldInfo("FilterEntity", "IsFolder", typeof(System.Boolean), false, false, false, false,  (int)FilterFieldIndex.IsFolder, 0, 0, 0);
			this.AddElementFieldInfo("FilterEntity", "Definition", typeof(System.String), false, false, false, true,  (int)FilterFieldIndex.Definition, 2147483647, 0, 0);
			this.AddElementFieldInfo("FilterEntity", "State", typeof(System.Byte), false, false, false, false,  (int)FilterFieldIndex.State, 0, 0, 3);
		}
		/// <summary>Inits FilterLayoutEntity's FieldInfo objects</summary>
		private void InitFilterLayoutEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(FilterLayoutFieldIndex), "FilterLayoutEntity");
			this.AddElementFieldInfo("FilterLayoutEntity", "FilterLayoutID", typeof(System.Int64), true, false, true, false,  (int)FilterLayoutFieldIndex.FilterLayoutID, 0, 0, 19);
			this.AddElementFieldInfo("FilterLayoutEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)FilterLayoutFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("FilterLayoutEntity", "UserID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)FilterLayoutFieldIndex.UserID, 0, 0, 19);
			this.AddElementFieldInfo("FilterLayoutEntity", "FilterTarget", typeof(System.Int32), false, false, false, false,  (int)FilterLayoutFieldIndex.FilterTarget, 0, 0, 10);
			this.AddElementFieldInfo("FilterLayoutEntity", "FilterNodeID", typeof(System.Int64), false, true, false, false,  (int)FilterLayoutFieldIndex.FilterNodeID, 0, 0, 19);
		}
		/// <summary>Inits FilterNodeEntity's FieldInfo objects</summary>
		private void InitFilterNodeEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(FilterNodeFieldIndex), "FilterNodeEntity");
			this.AddElementFieldInfo("FilterNodeEntity", "FilterNodeID", typeof(System.Int64), true, false, true, false,  (int)FilterNodeFieldIndex.FilterNodeID, 0, 0, 19);
			this.AddElementFieldInfo("FilterNodeEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)FilterNodeFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("FilterNodeEntity", "ParentFilterNodeID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)FilterNodeFieldIndex.ParentFilterNodeID, 0, 0, 19);
			this.AddElementFieldInfo("FilterNodeEntity", "FilterSequenceID", typeof(System.Int64), false, true, false, false,  (int)FilterNodeFieldIndex.FilterSequenceID, 0, 0, 19);
			this.AddElementFieldInfo("FilterNodeEntity", "FilterNodeContentID", typeof(System.Int64), false, true, false, false,  (int)FilterNodeFieldIndex.FilterNodeContentID, 0, 0, 19);
			this.AddElementFieldInfo("FilterNodeEntity", "Created", typeof(System.DateTime), false, false, false, false,  (int)FilterNodeFieldIndex.Created, 0, 0, 0);
			this.AddElementFieldInfo("FilterNodeEntity", "Purpose", typeof(System.Int32), false, false, false, false,  (int)FilterNodeFieldIndex.Purpose, 0, 0, 10);
		}
		/// <summary>Inits FilterNodeColumnSettingsEntity's FieldInfo objects</summary>
		private void InitFilterNodeColumnSettingsEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(FilterNodeColumnSettingsFieldIndex), "FilterNodeColumnSettingsEntity");
			this.AddElementFieldInfo("FilterNodeColumnSettingsEntity", "FilterNodeColumnSettingsID", typeof(System.Int64), true, false, true, false,  (int)FilterNodeColumnSettingsFieldIndex.FilterNodeColumnSettingsID, 0, 0, 19);
			this.AddElementFieldInfo("FilterNodeColumnSettingsEntity", "UserID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)FilterNodeColumnSettingsFieldIndex.UserID, 0, 0, 19);
			this.AddElementFieldInfo("FilterNodeColumnSettingsEntity", "FilterNodeID", typeof(System.Int64), false, true, false, false,  (int)FilterNodeColumnSettingsFieldIndex.FilterNodeID, 0, 0, 19);
			this.AddElementFieldInfo("FilterNodeColumnSettingsEntity", "Inherit", typeof(System.Boolean), false, false, false, false,  (int)FilterNodeColumnSettingsFieldIndex.Inherit, 0, 0, 0);
			this.AddElementFieldInfo("FilterNodeColumnSettingsEntity", "GridColumnLayoutID", typeof(System.Int64), false, true, false, false,  (int)FilterNodeColumnSettingsFieldIndex.GridColumnLayoutID, 0, 0, 19);
		}
		/// <summary>Inits FilterNodeContentEntity's FieldInfo objects</summary>
		private void InitFilterNodeContentEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(FilterNodeContentFieldIndex), "FilterNodeContentEntity");
			this.AddElementFieldInfo("FilterNodeContentEntity", "FilterNodeContentID", typeof(System.Int64), true, false, true, false,  (int)FilterNodeContentFieldIndex.FilterNodeContentID, 0, 0, 19);
			this.AddElementFieldInfo("FilterNodeContentEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)FilterNodeContentFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("FilterNodeContentEntity", "CountVersion", typeof(System.Int64), false, false, false, false,  (int)FilterNodeContentFieldIndex.CountVersion, 0, 0, 19);
			this.AddElementFieldInfo("FilterNodeContentEntity", "Status", typeof(System.Int16), false, false, false, false,  (int)FilterNodeContentFieldIndex.Status, 0, 0, 5);
			this.AddElementFieldInfo("FilterNodeContentEntity", "InitialCalculation", typeof(System.String), false, false, false, false,  (int)FilterNodeContentFieldIndex.InitialCalculation, 2147483647, 0, 0);
			this.AddElementFieldInfo("FilterNodeContentEntity", "UpdateCalculation", typeof(System.String), false, false, false, false,  (int)FilterNodeContentFieldIndex.UpdateCalculation, 2147483647, 0, 0);
			this.AddElementFieldInfo("FilterNodeContentEntity", "ColumnMask", typeof(System.Byte[]), false, false, false, false,  (int)FilterNodeContentFieldIndex.ColumnMask, 100, 0, 0);
			this.AddElementFieldInfo("FilterNodeContentEntity", "JoinMask", typeof(System.Int32), false, false, false, false,  (int)FilterNodeContentFieldIndex.JoinMask, 0, 0, 10);
			this.AddElementFieldInfo("FilterNodeContentEntity", "Cost", typeof(System.Int32), false, false, false, false,  (int)FilterNodeContentFieldIndex.Cost, 0, 0, 10);
			this.AddElementFieldInfo("FilterNodeContentEntity", "Count", typeof(System.Int32), false, false, false, false,  (int)FilterNodeContentFieldIndex.Count, 0, 0, 10);
		}
		/// <summary>Inits FilterNodeContentDetailEntity's FieldInfo objects</summary>
		private void InitFilterNodeContentDetailEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(FilterNodeContentDetailFieldIndex), "FilterNodeContentDetailEntity");
			this.AddElementFieldInfo("FilterNodeContentDetailEntity", "FilterNodeContentID", typeof(System.Int64), true, true, false, false,  (int)FilterNodeContentDetailFieldIndex.FilterNodeContentID, 0, 0, 19);
			this.AddElementFieldInfo("FilterNodeContentDetailEntity", "EntityID", typeof(System.Int64), true, false, false, false,  (int)FilterNodeContentDetailFieldIndex.EntityID, 0, 0, 19);
		}
		/// <summary>Inits FilterSequenceEntity's FieldInfo objects</summary>
		private void InitFilterSequenceEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(FilterSequenceFieldIndex), "FilterSequenceEntity");
			this.AddElementFieldInfo("FilterSequenceEntity", "FilterSequenceID", typeof(System.Int64), true, false, true, false,  (int)FilterSequenceFieldIndex.FilterSequenceID, 0, 0, 19);
			this.AddElementFieldInfo("FilterSequenceEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)FilterSequenceFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("FilterSequenceEntity", "ParentFilterID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)FilterSequenceFieldIndex.ParentFilterID, 0, 0, 19);
			this.AddElementFieldInfo("FilterSequenceEntity", "FilterID", typeof(System.Int64), false, true, false, false,  (int)FilterSequenceFieldIndex.FilterID, 0, 0, 19);
			this.AddElementFieldInfo("FilterSequenceEntity", "Position", typeof(System.Int32), false, false, false, false,  (int)FilterSequenceFieldIndex.Position, 0, 0, 10);
		}
		/// <summary>Inits FtpAccountEntity's FieldInfo objects</summary>
		private void InitFtpAccountEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(FtpAccountFieldIndex), "FtpAccountEntity");
			this.AddElementFieldInfo("FtpAccountEntity", "FtpAccountID", typeof(System.Int64), true, false, true, false,  (int)FtpAccountFieldIndex.FtpAccountID, 0, 0, 19);
			this.AddElementFieldInfo("FtpAccountEntity", "Host", typeof(System.String), false, false, false, false,  (int)FtpAccountFieldIndex.Host, 100, 0, 0);
			this.AddElementFieldInfo("FtpAccountEntity", "Username", typeof(System.String), false, false, false, false,  (int)FtpAccountFieldIndex.Username, 50, 0, 0);
			this.AddElementFieldInfo("FtpAccountEntity", "Password", typeof(System.String), false, false, false, false,  (int)FtpAccountFieldIndex.Password, 50, 0, 0);
			this.AddElementFieldInfo("FtpAccountEntity", "Port", typeof(System.Int32), false, false, false, false,  (int)FtpAccountFieldIndex.Port, 0, 0, 10);
			this.AddElementFieldInfo("FtpAccountEntity", "SecurityType", typeof(System.Int32), false, false, false, false,  (int)FtpAccountFieldIndex.SecurityType, 0, 0, 10);
			this.AddElementFieldInfo("FtpAccountEntity", "Passive", typeof(System.Boolean), false, false, false, false,  (int)FtpAccountFieldIndex.Passive, 0, 0, 0);
			this.AddElementFieldInfo("FtpAccountEntity", "InternalOwnerID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)FtpAccountFieldIndex.InternalOwnerID, 0, 0, 19);
			this.AddElementFieldInfo("FtpAccountEntity", "ReuseControlConnectionSession", typeof(System.Boolean), false, false, false, false,  (int)FtpAccountFieldIndex.ReuseControlConnectionSession, 0, 0, 0);
		}
		/// <summary>Inits GenericFileStoreEntity's FieldInfo objects</summary>
		private void InitGenericFileStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(GenericFileStoreFieldIndex), "GenericFileStoreEntity");
			this.AddElementFieldInfo("GenericFileStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)GenericFileStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("GenericFileStoreEntity", "FileFormat", typeof(System.Int32), false, false, false, false,  (int)GenericFileStoreFieldIndex.FileFormat, 0, 0, 10);
			this.AddElementFieldInfo("GenericFileStoreEntity", "FileSource", typeof(System.Int32), false, false, false, false,  (int)GenericFileStoreFieldIndex.FileSource, 0, 0, 10);
			this.AddElementFieldInfo("GenericFileStoreEntity", "DiskFolder", typeof(System.String), false, false, false, false,  (int)GenericFileStoreFieldIndex.DiskFolder, 355, 0, 0);
			this.AddElementFieldInfo("GenericFileStoreEntity", "FtpAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)GenericFileStoreFieldIndex.FtpAccountID, 0, 0, 19);
			this.AddElementFieldInfo("GenericFileStoreEntity", "FtpFolder", typeof(System.String), false, false, false, false,  (int)GenericFileStoreFieldIndex.FtpFolder, 355, 0, 0);
			this.AddElementFieldInfo("GenericFileStoreEntity", "EmailAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)GenericFileStoreFieldIndex.EmailAccountID, 0, 0, 19);
			this.AddElementFieldInfo("GenericFileStoreEntity", "EmailIncomingFolder", typeof(System.String), false, false, false, false,  (int)GenericFileStoreFieldIndex.EmailIncomingFolder, 100, 0, 0);
			this.AddElementFieldInfo("GenericFileStoreEntity", "EmailFolderValidityID", typeof(System.Int64), false, false, false, false,  (int)GenericFileStoreFieldIndex.EmailFolderValidityID, 0, 0, 19);
			this.AddElementFieldInfo("GenericFileStoreEntity", "EmailFolderLastMessageID", typeof(System.Int64), false, false, false, false,  (int)GenericFileStoreFieldIndex.EmailFolderLastMessageID, 0, 0, 19);
			this.AddElementFieldInfo("GenericFileStoreEntity", "EmailOnlyUnread", typeof(System.Boolean), false, false, false, false,  (int)GenericFileStoreFieldIndex.EmailOnlyUnread, 0, 0, 0);
			this.AddElementFieldInfo("GenericFileStoreEntity", "NamePatternMatch", typeof(System.String), false, false, false, true,  (int)GenericFileStoreFieldIndex.NamePatternMatch, 50, 0, 0);
			this.AddElementFieldInfo("GenericFileStoreEntity", "NamePatternSkip", typeof(System.String), false, false, false, true,  (int)GenericFileStoreFieldIndex.NamePatternSkip, 50, 0, 0);
			this.AddElementFieldInfo("GenericFileStoreEntity", "SuccessAction", typeof(System.Int32), false, false, false, false,  (int)GenericFileStoreFieldIndex.SuccessAction, 0, 0, 10);
			this.AddElementFieldInfo("GenericFileStoreEntity", "SuccessMoveFolder", typeof(System.String), false, false, false, false,  (int)GenericFileStoreFieldIndex.SuccessMoveFolder, 355, 0, 0);
			this.AddElementFieldInfo("GenericFileStoreEntity", "ErrorAction", typeof(System.Int32), false, false, false, false,  (int)GenericFileStoreFieldIndex.ErrorAction, 0, 0, 10);
			this.AddElementFieldInfo("GenericFileStoreEntity", "ErrorMoveFolder", typeof(System.String), false, false, false, false,  (int)GenericFileStoreFieldIndex.ErrorMoveFolder, 355, 0, 0);
			this.AddElementFieldInfo("GenericFileStoreEntity", "XmlXsltFileName", typeof(System.String), false, false, false, true,  (int)GenericFileStoreFieldIndex.XmlXsltFileName, 355, 0, 0);
			this.AddElementFieldInfo("GenericFileStoreEntity", "XmlXsltContent", typeof(System.String), false, false, false, true,  (int)GenericFileStoreFieldIndex.XmlXsltContent, 1073741823, 0, 0);
			this.AddElementFieldInfo("GenericFileStoreEntity", "FlatImportMap", typeof(System.String), false, false, false, false,  (int)GenericFileStoreFieldIndex.FlatImportMap, 1073741823, 0, 0);
		}
		/// <summary>Inits GenericModuleStoreEntity's FieldInfo objects</summary>
		private void InitGenericModuleStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(GenericModuleStoreFieldIndex), "GenericModuleStoreEntity");
			this.AddElementFieldInfo("GenericModuleStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)GenericModuleStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleUsername", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleUsername, 50, 0, 0);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModulePassword", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModulePassword, 80, 0, 0);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleUrl", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleUrl, 350, 0, 0);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleVersion", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleVersion, 20, 0, 0);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModulePlatform", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModulePlatform, 50, 0, 0);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleDeveloper", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleDeveloper, 50, 0, 0);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleOnlineStoreCode", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleOnlineStoreCode, 50, 0, 0);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleStatusCodes", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleStatusCodes, 2147483647, 0, 0);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleDownloadPageSize", typeof(System.Int32), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleDownloadPageSize, 0, 0, 10);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleRequestTimeout", typeof(System.Int32), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleRequestTimeout, 0, 0, 10);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleDownloadStrategy", typeof(System.Int32), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleDownloadStrategy, 0, 0, 10);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleOnlineStatusSupport", typeof(System.Int32), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleOnlineStatusSupport, 0, 0, 10);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleOnlineStatusDataType", typeof(System.Int32), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleOnlineStatusDataType, 0, 0, 10);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleOnlineCustomerSupport", typeof(System.Boolean), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleOnlineCustomerSupport, 0, 0, 0);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleOnlineCustomerDataType", typeof(System.Int32), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleOnlineCustomerDataType, 0, 0, 10);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleOnlineShipmentDetails", typeof(System.Boolean), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleOnlineShipmentDetails, 0, 0, 0);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleHttpExpect100Continue", typeof(System.Boolean), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleHttpExpect100Continue, 0, 0, 0);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleResponseEncoding", typeof(System.Int32), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleResponseEncoding, 0, 0, 10);
			this.AddElementFieldInfo("GenericModuleStoreEntity", "SchemaVersion", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.SchemaVersion, 20, 0, 0);
		}
		/// <summary>Inits GridColumnFormatEntity's FieldInfo objects</summary>
		private void InitGridColumnFormatEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(GridColumnFormatFieldIndex), "GridColumnFormatEntity");
			this.AddElementFieldInfo("GridColumnFormatEntity", "GridColumnFormatID", typeof(System.Int64), true, false, true, false,  (int)GridColumnFormatFieldIndex.GridColumnFormatID, 0, 0, 19);
			this.AddElementFieldInfo("GridColumnFormatEntity", "UserID", typeof(System.Int64), false, true, false, false,  (int)GridColumnFormatFieldIndex.UserID, 0, 0, 19);
			this.AddElementFieldInfo("GridColumnFormatEntity", "ColumnGuid", typeof(System.Guid), false, false, false, false,  (int)GridColumnFormatFieldIndex.ColumnGuid, 0, 0, 0);
			this.AddElementFieldInfo("GridColumnFormatEntity", "Settings", typeof(System.String), false, false, false, false,  (int)GridColumnFormatFieldIndex.Settings, 2147483647, 0, 0);
		}
		/// <summary>Inits GridColumnLayoutEntity's FieldInfo objects</summary>
		private void InitGridColumnLayoutEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(GridColumnLayoutFieldIndex), "GridColumnLayoutEntity");
			this.AddElementFieldInfo("GridColumnLayoutEntity", "GridColumnLayoutID", typeof(System.Int64), true, false, true, false,  (int)GridColumnLayoutFieldIndex.GridColumnLayoutID, 0, 0, 19);
			this.AddElementFieldInfo("GridColumnLayoutEntity", "DefinitionSet", typeof(System.Int32), false, false, false, false,  (int)GridColumnLayoutFieldIndex.DefinitionSet, 0, 0, 10);
			this.AddElementFieldInfo("GridColumnLayoutEntity", "DefaultSortColumnGuid", typeof(System.Guid), false, false, false, false,  (int)GridColumnLayoutFieldIndex.DefaultSortColumnGuid, 0, 0, 0);
			this.AddElementFieldInfo("GridColumnLayoutEntity", "DefaultSortOrder", typeof(System.Int32), false, false, false, false,  (int)GridColumnLayoutFieldIndex.DefaultSortOrder, 0, 0, 10);
			this.AddElementFieldInfo("GridColumnLayoutEntity", "LastSortColumnGuid", typeof(System.Guid), false, false, false, false,  (int)GridColumnLayoutFieldIndex.LastSortColumnGuid, 0, 0, 0);
			this.AddElementFieldInfo("GridColumnLayoutEntity", "LastSortOrder", typeof(System.Int32), false, false, false, false,  (int)GridColumnLayoutFieldIndex.LastSortOrder, 0, 0, 10);
			this.AddElementFieldInfo("GridColumnLayoutEntity", "DetailViewSettings", typeof(System.String), false, false, false, true,  (int)GridColumnLayoutFieldIndex.DetailViewSettings, 2147483647, 0, 0);
		}
		/// <summary>Inits GridColumnPositionEntity's FieldInfo objects</summary>
		private void InitGridColumnPositionEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(GridColumnPositionFieldIndex), "GridColumnPositionEntity");
			this.AddElementFieldInfo("GridColumnPositionEntity", "GridColumnPositionID", typeof(System.Int64), true, false, true, false,  (int)GridColumnPositionFieldIndex.GridColumnPositionID, 0, 0, 19);
			this.AddElementFieldInfo("GridColumnPositionEntity", "GridColumnLayoutID", typeof(System.Int64), false, true, false, false,  (int)GridColumnPositionFieldIndex.GridColumnLayoutID, 0, 0, 19);
			this.AddElementFieldInfo("GridColumnPositionEntity", "ColumnGuid", typeof(System.Guid), false, false, false, false,  (int)GridColumnPositionFieldIndex.ColumnGuid, 0, 0, 0);
			this.AddElementFieldInfo("GridColumnPositionEntity", "Visible", typeof(System.Boolean), false, false, false, false,  (int)GridColumnPositionFieldIndex.Visible, 0, 0, 0);
			this.AddElementFieldInfo("GridColumnPositionEntity", "Width", typeof(System.Int32), false, false, false, false,  (int)GridColumnPositionFieldIndex.Width, 0, 0, 10);
			this.AddElementFieldInfo("GridColumnPositionEntity", "Position", typeof(System.Int32), false, false, false, false,  (int)GridColumnPositionFieldIndex.Position, 0, 0, 10);
		}
		/// <summary>Inits GrouponOrderEntity's FieldInfo objects</summary>
		private void InitGrouponOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(GrouponOrderFieldIndex), "GrouponOrderEntity");
			this.AddElementFieldInfo("GrouponOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)GrouponOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("GrouponOrderEntity", "GrouponOrderID", typeof(System.String), false, false, false, false,  (int)GrouponOrderFieldIndex.GrouponOrderID, 50, 0, 0);
		}
		/// <summary>Inits GrouponOrderItemEntity's FieldInfo objects</summary>
		private void InitGrouponOrderItemEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(GrouponOrderItemFieldIndex), "GrouponOrderItemEntity");
			this.AddElementFieldInfo("GrouponOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, true, false,  (int)GrouponOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("GrouponOrderItemEntity", "Permalink", typeof(System.String), false, false, false, false,  (int)GrouponOrderItemFieldIndex.Permalink, 255, 0, 0);
			this.AddElementFieldInfo("GrouponOrderItemEntity", "ChannelSKUProvided", typeof(System.String), false, false, false, false,  (int)GrouponOrderItemFieldIndex.ChannelSKUProvided, 255, 0, 0);
			this.AddElementFieldInfo("GrouponOrderItemEntity", "FulfillmentLineItemID", typeof(System.String), false, false, false, false,  (int)GrouponOrderItemFieldIndex.FulfillmentLineItemID, 255, 0, 0);
			this.AddElementFieldInfo("GrouponOrderItemEntity", "BomSKU", typeof(System.String), false, false, false, false,  (int)GrouponOrderItemFieldIndex.BomSKU, 255, 0, 0);
			this.AddElementFieldInfo("GrouponOrderItemEntity", "GrouponLineItemID", typeof(System.String), false, false, false, false,  (int)GrouponOrderItemFieldIndex.GrouponLineItemID, 255, 0, 0);
		}
		/// <summary>Inits GrouponStoreEntity's FieldInfo objects</summary>
		private void InitGrouponStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(GrouponStoreFieldIndex), "GrouponStoreEntity");
			this.AddElementFieldInfo("GrouponStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)GrouponStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("GrouponStoreEntity", "SupplierID", typeof(System.String), false, false, false, false,  (int)GrouponStoreFieldIndex.SupplierID, 255, 0, 0);
			this.AddElementFieldInfo("GrouponStoreEntity", "Token", typeof(System.String), false, false, false, false,  (int)GrouponStoreFieldIndex.Token, 255, 0, 0);
		}
		/// <summary>Inits InfopiaOrderItemEntity's FieldInfo objects</summary>
		private void InitInfopiaOrderItemEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(InfopiaOrderItemFieldIndex), "InfopiaOrderItemEntity");
			this.AddElementFieldInfo("InfopiaOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, true, false,  (int)InfopiaOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("InfopiaOrderItemEntity", "Marketplace", typeof(System.String), false, false, false, false,  (int)InfopiaOrderItemFieldIndex.Marketplace, 50, 0, 0);
			this.AddElementFieldInfo("InfopiaOrderItemEntity", "MarketplaceItemID", typeof(System.String), false, false, false, false,  (int)InfopiaOrderItemFieldIndex.MarketplaceItemID, 20, 0, 0);
			this.AddElementFieldInfo("InfopiaOrderItemEntity", "BuyerID", typeof(System.String), false, false, false, false,  (int)InfopiaOrderItemFieldIndex.BuyerID, 50, 0, 0);
		}
		/// <summary>Inits InfopiaStoreEntity's FieldInfo objects</summary>
		private void InitInfopiaStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(InfopiaStoreFieldIndex), "InfopiaStoreEntity");
			this.AddElementFieldInfo("InfopiaStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)InfopiaStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("InfopiaStoreEntity", "ApiToken", typeof(System.String), false, false, false, false,  (int)InfopiaStoreFieldIndex.ApiToken, 128, 0, 0);
		}
		/// <summary>Inits InsurancePolicyEntity's FieldInfo objects</summary>
		private void InitInsurancePolicyEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(InsurancePolicyFieldIndex), "InsurancePolicyEntity");
			this.AddElementFieldInfo("InsurancePolicyEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)InsurancePolicyFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("InsurancePolicyEntity", "InsureShipStoreName", typeof(System.String), false, false, false, false,  (int)InsurancePolicyFieldIndex.InsureShipStoreName, 75, 0, 0);
			this.AddElementFieldInfo("InsurancePolicyEntity", "CreatedWithApi", typeof(System.Boolean), false, false, false, false,  (int)InsurancePolicyFieldIndex.CreatedWithApi, 0, 0, 0);
			this.AddElementFieldInfo("InsurancePolicyEntity", "ItemName", typeof(System.String), false, false, false, true,  (int)InsurancePolicyFieldIndex.ItemName, 255, 0, 0);
			this.AddElementFieldInfo("InsurancePolicyEntity", "Description", typeof(System.String), false, false, false, true,  (int)InsurancePolicyFieldIndex.Description, 255, 0, 0);
			this.AddElementFieldInfo("InsurancePolicyEntity", "ClaimType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)InsurancePolicyFieldIndex.ClaimType, 0, 0, 10);
			this.AddElementFieldInfo("InsurancePolicyEntity", "DamageValue", typeof(Nullable<System.Decimal>), false, false, false, true,  (int)InsurancePolicyFieldIndex.DamageValue, 0, 4, 19);
			this.AddElementFieldInfo("InsurancePolicyEntity", "SubmissionDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)InsurancePolicyFieldIndex.SubmissionDate, 0, 0, 0);
			this.AddElementFieldInfo("InsurancePolicyEntity", "ClaimID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)InsurancePolicyFieldIndex.ClaimID, 0, 0, 19);
			this.AddElementFieldInfo("InsurancePolicyEntity", "EmailAddress", typeof(System.String), false, false, false, true,  (int)InsurancePolicyFieldIndex.EmailAddress, 100, 0, 0);
		}
		/// <summary>Inits IParcelAccountEntity's FieldInfo objects</summary>
		private void InitIParcelAccountEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(IParcelAccountFieldIndex), "IParcelAccountEntity");
			this.AddElementFieldInfo("IParcelAccountEntity", "IParcelAccountID", typeof(System.Int64), true, false, true, false,  (int)IParcelAccountFieldIndex.IParcelAccountID, 0, 0, 19);
			this.AddElementFieldInfo("IParcelAccountEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)IParcelAccountFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("IParcelAccountEntity", "Username", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Username, 50, 0, 0);
			this.AddElementFieldInfo("IParcelAccountEntity", "Password", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Password, 50, 0, 0);
			this.AddElementFieldInfo("IParcelAccountEntity", "Description", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Description, 50, 0, 0);
			this.AddElementFieldInfo("IParcelAccountEntity", "FirstName", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.FirstName, 30, 0, 0);
			this.AddElementFieldInfo("IParcelAccountEntity", "MiddleName", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.MiddleName, 30, 0, 0);
			this.AddElementFieldInfo("IParcelAccountEntity", "LastName", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.LastName, 30, 0, 0);
			this.AddElementFieldInfo("IParcelAccountEntity", "Company", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Company, 30, 0, 0);
			this.AddElementFieldInfo("IParcelAccountEntity", "Street1", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Street1, 50, 0, 0);
			this.AddElementFieldInfo("IParcelAccountEntity", "Street2", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Street2, 50, 0, 0);
			this.AddElementFieldInfo("IParcelAccountEntity", "City", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.City, 30, 0, 0);
			this.AddElementFieldInfo("IParcelAccountEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.StateProvCode, 30, 0, 0);
			this.AddElementFieldInfo("IParcelAccountEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.PostalCode, 20, 0, 0);
			this.AddElementFieldInfo("IParcelAccountEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.CountryCode, 50, 0, 0);
			this.AddElementFieldInfo("IParcelAccountEntity", "Phone", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Phone, 25, 0, 0);
			this.AddElementFieldInfo("IParcelAccountEntity", "Email", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Email, 100, 0, 0);
			this.AddElementFieldInfo("IParcelAccountEntity", "Website", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Website, 50, 0, 0);
		}
		/// <summary>Inits IParcelPackageEntity's FieldInfo objects</summary>
		private void InitIParcelPackageEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(IParcelPackageFieldIndex), "IParcelPackageEntity");
			this.AddElementFieldInfo("IParcelPackageEntity", "IParcelPackageID", typeof(System.Int64), true, false, true, false,  (int)IParcelPackageFieldIndex.IParcelPackageID, 0, 0, 19);
			this.AddElementFieldInfo("IParcelPackageEntity", "ShipmentID", typeof(System.Int64), false, true, false, false,  (int)IParcelPackageFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("IParcelPackageEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)IParcelPackageFieldIndex.Weight, 0, 0, 38);
			this.AddElementFieldInfo("IParcelPackageEntity", "DimsProfileID", typeof(System.Int64), false, false, false, false,  (int)IParcelPackageFieldIndex.DimsProfileID, 0, 0, 19);
			this.AddElementFieldInfo("IParcelPackageEntity", "DimsLength", typeof(System.Double), false, false, false, false,  (int)IParcelPackageFieldIndex.DimsLength, 0, 0, 38);
			this.AddElementFieldInfo("IParcelPackageEntity", "DimsWidth", typeof(System.Double), false, false, false, false,  (int)IParcelPackageFieldIndex.DimsWidth, 0, 0, 38);
			this.AddElementFieldInfo("IParcelPackageEntity", "DimsHeight", typeof(System.Double), false, false, false, false,  (int)IParcelPackageFieldIndex.DimsHeight, 0, 0, 38);
			this.AddElementFieldInfo("IParcelPackageEntity", "DimsAddWeight", typeof(System.Boolean), false, false, false, false,  (int)IParcelPackageFieldIndex.DimsAddWeight, 0, 0, 0);
			this.AddElementFieldInfo("IParcelPackageEntity", "DimsWeight", typeof(System.Double), false, false, false, false,  (int)IParcelPackageFieldIndex.DimsWeight, 0, 0, 38);
			this.AddElementFieldInfo("IParcelPackageEntity", "Insurance", typeof(System.Boolean), false, false, false, false,  (int)IParcelPackageFieldIndex.Insurance, 0, 0, 0);
			this.AddElementFieldInfo("IParcelPackageEntity", "InsuranceValue", typeof(System.Decimal), false, false, false, false,  (int)IParcelPackageFieldIndex.InsuranceValue, 0, 4, 19);
			this.AddElementFieldInfo("IParcelPackageEntity", "InsurancePennyOne", typeof(System.Boolean), false, false, false, false,  (int)IParcelPackageFieldIndex.InsurancePennyOne, 0, 0, 0);
			this.AddElementFieldInfo("IParcelPackageEntity", "DeclaredValue", typeof(System.Decimal), false, false, false, false,  (int)IParcelPackageFieldIndex.DeclaredValue, 0, 4, 19);
			this.AddElementFieldInfo("IParcelPackageEntity", "TrackingNumber", typeof(System.String), false, false, false, false,  (int)IParcelPackageFieldIndex.TrackingNumber, 50, 0, 0);
			this.AddElementFieldInfo("IParcelPackageEntity", "ParcelNumber", typeof(System.String), false, false, false, false,  (int)IParcelPackageFieldIndex.ParcelNumber, 50, 0, 0);
			this.AddElementFieldInfo("IParcelPackageEntity", "SkuAndQuantities", typeof(System.String), false, false, false, false,  (int)IParcelPackageFieldIndex.SkuAndQuantities, 500, 0, 0);
		}
		/// <summary>Inits IParcelProfileEntity's FieldInfo objects</summary>
		private void InitIParcelProfileEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(IParcelProfileFieldIndex), "IParcelProfileEntity");
			this.AddElementFieldInfo("IParcelProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)IParcelProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			this.AddElementFieldInfo("IParcelProfileEntity", "IParcelAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)IParcelProfileFieldIndex.IParcelAccountID, 0, 0, 19);
			this.AddElementFieldInfo("IParcelProfileEntity", "Service", typeof(Nullable<System.Int32>), false, false, false, true,  (int)IParcelProfileFieldIndex.Service, 0, 0, 10);
			this.AddElementFieldInfo("IParcelProfileEntity", "Reference", typeof(System.String), false, false, false, true,  (int)IParcelProfileFieldIndex.Reference, 300, 0, 0);
			this.AddElementFieldInfo("IParcelProfileEntity", "TrackByEmail", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)IParcelProfileFieldIndex.TrackByEmail, 0, 0, 0);
			this.AddElementFieldInfo("IParcelProfileEntity", "TrackBySMS", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)IParcelProfileFieldIndex.TrackBySMS, 0, 0, 0);
			this.AddElementFieldInfo("IParcelProfileEntity", "IsDeliveryDutyPaid", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)IParcelProfileFieldIndex.IsDeliveryDutyPaid, 0, 0, 0);
			this.AddElementFieldInfo("IParcelProfileEntity", "SkuAndQuantities", typeof(System.String), false, false, false, true,  (int)IParcelProfileFieldIndex.SkuAndQuantities, 500, 0, 0);
		}
		/// <summary>Inits IParcelProfilePackageEntity's FieldInfo objects</summary>
		private void InitIParcelProfilePackageEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(IParcelProfilePackageFieldIndex), "IParcelProfilePackageEntity");
			this.AddElementFieldInfo("IParcelProfilePackageEntity", "IParcelProfilePackageID", typeof(System.Int64), true, false, true, false,  (int)IParcelProfilePackageFieldIndex.IParcelProfilePackageID, 0, 0, 19);
			this.AddElementFieldInfo("IParcelProfilePackageEntity", "ShippingProfileID", typeof(System.Int64), false, true, false, false,  (int)IParcelProfilePackageFieldIndex.ShippingProfileID, 0, 0, 19);
			this.AddElementFieldInfo("IParcelProfilePackageEntity", "Weight", typeof(Nullable<System.Double>), false, false, false, true,  (int)IParcelProfilePackageFieldIndex.Weight, 0, 0, 38);
			this.AddElementFieldInfo("IParcelProfilePackageEntity", "DimsProfileID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)IParcelProfilePackageFieldIndex.DimsProfileID, 0, 0, 19);
			this.AddElementFieldInfo("IParcelProfilePackageEntity", "DimsLength", typeof(Nullable<System.Double>), false, false, false, true,  (int)IParcelProfilePackageFieldIndex.DimsLength, 0, 0, 38);
			this.AddElementFieldInfo("IParcelProfilePackageEntity", "DimsWidth", typeof(Nullable<System.Double>), false, false, false, true,  (int)IParcelProfilePackageFieldIndex.DimsWidth, 0, 0, 38);
			this.AddElementFieldInfo("IParcelProfilePackageEntity", "DimsHeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)IParcelProfilePackageFieldIndex.DimsHeight, 0, 0, 38);
			this.AddElementFieldInfo("IParcelProfilePackageEntity", "DimsWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)IParcelProfilePackageFieldIndex.DimsWeight, 0, 0, 38);
			this.AddElementFieldInfo("IParcelProfilePackageEntity", "DimsAddWeight", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)IParcelProfilePackageFieldIndex.DimsAddWeight, 0, 0, 0);
		}
		/// <summary>Inits IParcelShipmentEntity's FieldInfo objects</summary>
		private void InitIParcelShipmentEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(IParcelShipmentFieldIndex), "IParcelShipmentEntity");
			this.AddElementFieldInfo("IParcelShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)IParcelShipmentFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("IParcelShipmentEntity", "IParcelAccountID", typeof(System.Int64), false, false, false, false,  (int)IParcelShipmentFieldIndex.IParcelAccountID, 0, 0, 19);
			this.AddElementFieldInfo("IParcelShipmentEntity", "Service", typeof(System.Int32), false, false, false, false,  (int)IParcelShipmentFieldIndex.Service, 0, 0, 10);
			this.AddElementFieldInfo("IParcelShipmentEntity", "Reference", typeof(System.String), false, false, false, false,  (int)IParcelShipmentFieldIndex.Reference, 300, 0, 0);
			this.AddElementFieldInfo("IParcelShipmentEntity", "TrackByEmail", typeof(System.Boolean), false, false, false, false,  (int)IParcelShipmentFieldIndex.TrackByEmail, 0, 0, 0);
			this.AddElementFieldInfo("IParcelShipmentEntity", "TrackBySMS", typeof(System.Boolean), false, false, false, false,  (int)IParcelShipmentFieldIndex.TrackBySMS, 0, 0, 0);
			this.AddElementFieldInfo("IParcelShipmentEntity", "IsDeliveryDutyPaid", typeof(System.Boolean), false, false, false, false,  (int)IParcelShipmentFieldIndex.IsDeliveryDutyPaid, 0, 0, 0);
			this.AddElementFieldInfo("IParcelShipmentEntity", "RequestedLabelFormat", typeof(System.Int32), false, false, false, false,  (int)IParcelShipmentFieldIndex.RequestedLabelFormat, 0, 0, 10);
		}
		/// <summary>Inits LabelSheetEntity's FieldInfo objects</summary>
		private void InitLabelSheetEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(LabelSheetFieldIndex), "LabelSheetEntity");
			this.AddElementFieldInfo("LabelSheetEntity", "LabelSheetID", typeof(System.Int64), true, false, true, false,  (int)LabelSheetFieldIndex.LabelSheetID, 0, 0, 19);
			this.AddElementFieldInfo("LabelSheetEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)LabelSheetFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("LabelSheetEntity", "Name", typeof(System.String), false, false, false, false,  (int)LabelSheetFieldIndex.Name, 100, 0, 0);
			this.AddElementFieldInfo("LabelSheetEntity", "PaperSizeHeight", typeof(System.Double), false, false, false, false,  (int)LabelSheetFieldIndex.PaperSizeHeight, 0, 0, 38);
			this.AddElementFieldInfo("LabelSheetEntity", "PaperSizeWidth", typeof(System.Double), false, false, false, false,  (int)LabelSheetFieldIndex.PaperSizeWidth, 0, 0, 38);
			this.AddElementFieldInfo("LabelSheetEntity", "MarginTop", typeof(System.Double), false, false, false, false,  (int)LabelSheetFieldIndex.MarginTop, 0, 0, 38);
			this.AddElementFieldInfo("LabelSheetEntity", "MarginLeft", typeof(System.Double), false, false, false, false,  (int)LabelSheetFieldIndex.MarginLeft, 0, 0, 38);
			this.AddElementFieldInfo("LabelSheetEntity", "LabelHeight", typeof(System.Double), false, false, false, false,  (int)LabelSheetFieldIndex.LabelHeight, 0, 0, 38);
			this.AddElementFieldInfo("LabelSheetEntity", "LabelWidth", typeof(System.Double), false, false, false, false,  (int)LabelSheetFieldIndex.LabelWidth, 0, 0, 38);
			this.AddElementFieldInfo("LabelSheetEntity", "VerticalSpacing", typeof(System.Double), false, false, false, false,  (int)LabelSheetFieldIndex.VerticalSpacing, 0, 0, 38);
			this.AddElementFieldInfo("LabelSheetEntity", "HorizontalSpacing", typeof(System.Double), false, false, false, false,  (int)LabelSheetFieldIndex.HorizontalSpacing, 0, 0, 38);
			this.AddElementFieldInfo("LabelSheetEntity", "Rows", typeof(System.Int32), false, false, false, false,  (int)LabelSheetFieldIndex.Rows, 0, 0, 10);
			this.AddElementFieldInfo("LabelSheetEntity", "Columns", typeof(System.Int32), false, false, false, false,  (int)LabelSheetFieldIndex.Columns, 0, 0, 10);
		}
		/// <summary>Inits LemonStandOrderEntity's FieldInfo objects</summary>
		private void InitLemonStandOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(LemonStandOrderFieldIndex), "LemonStandOrderEntity");
			this.AddElementFieldInfo("LemonStandOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)LemonStandOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("LemonStandOrderEntity", "LemonStandOrderID", typeof(System.String), false, false, false, false,  (int)LemonStandOrderFieldIndex.LemonStandOrderID, 20, 0, 0);
		}
		/// <summary>Inits LemonStandOrderItemEntity's FieldInfo objects</summary>
		private void InitLemonStandOrderItemEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(LemonStandOrderItemFieldIndex), "LemonStandOrderItemEntity");
			this.AddElementFieldInfo("LemonStandOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, true, false,  (int)LemonStandOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("LemonStandOrderItemEntity", "UrlName", typeof(System.String), false, false, false, false,  (int)LemonStandOrderItemFieldIndex.UrlName, 100, 0, 0);
			this.AddElementFieldInfo("LemonStandOrderItemEntity", "ShortDescription", typeof(System.String), false, false, false, false,  (int)LemonStandOrderItemFieldIndex.ShortDescription, 255, 0, 0);
			this.AddElementFieldInfo("LemonStandOrderItemEntity", "Category", typeof(System.String), false, false, false, false,  (int)LemonStandOrderItemFieldIndex.Category, 100, 0, 0);
		}
		/// <summary>Inits LemonStandStoreEntity's FieldInfo objects</summary>
		private void InitLemonStandStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(LemonStandStoreFieldIndex), "LemonStandStoreEntity");
			this.AddElementFieldInfo("LemonStandStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)LemonStandStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("LemonStandStoreEntity", "Token", typeof(System.String), false, false, false, false,  (int)LemonStandStoreFieldIndex.Token, 100, 0, 0);
			this.AddElementFieldInfo("LemonStandStoreEntity", "StoreURL", typeof(System.String), false, false, false, false,  (int)LemonStandStoreFieldIndex.StoreURL, 255, 0, 0);
			this.AddElementFieldInfo("LemonStandStoreEntity", "StatusCodes", typeof(System.String), false, false, false, true,  (int)LemonStandStoreFieldIndex.StatusCodes, 2147483647, 0, 0);
		}
		/// <summary>Inits MagentoOrderEntity's FieldInfo objects</summary>
		private void InitMagentoOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(MagentoOrderFieldIndex), "MagentoOrderEntity");
			this.AddElementFieldInfo("MagentoOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)MagentoOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("MagentoOrderEntity", "MagentoOrderID", typeof(System.Int64), false, false, false, false,  (int)MagentoOrderFieldIndex.MagentoOrderID, 0, 0, 19);
		}
		/// <summary>Inits MagentoStoreEntity's FieldInfo objects</summary>
		private void InitMagentoStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(MagentoStoreFieldIndex), "MagentoStoreEntity");
			this.AddElementFieldInfo("MagentoStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)MagentoStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("MagentoStoreEntity", "MagentoTrackingEmails", typeof(System.Boolean), false, false, false, false,  (int)MagentoStoreFieldIndex.MagentoTrackingEmails, 0, 0, 0);
			this.AddElementFieldInfo("MagentoStoreEntity", "MagentoVersion", typeof(System.Int32), false, false, false, false,  (int)MagentoStoreFieldIndex.MagentoVersion, 0, 0, 10);
		}
		/// <summary>Inits MarketplaceAdvisorOrderEntity's FieldInfo objects</summary>
		private void InitMarketplaceAdvisorOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(MarketplaceAdvisorOrderFieldIndex), "MarketplaceAdvisorOrderEntity");
			this.AddElementFieldInfo("MarketplaceAdvisorOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)MarketplaceAdvisorOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("MarketplaceAdvisorOrderEntity", "BuyerNumber", typeof(System.Int64), false, false, false, false,  (int)MarketplaceAdvisorOrderFieldIndex.BuyerNumber, 0, 0, 19);
			this.AddElementFieldInfo("MarketplaceAdvisorOrderEntity", "SellerOrderNumber", typeof(System.Int64), false, false, false, false,  (int)MarketplaceAdvisorOrderFieldIndex.SellerOrderNumber, 0, 0, 19);
			this.AddElementFieldInfo("MarketplaceAdvisorOrderEntity", "InvoiceNumber", typeof(System.String), false, false, false, false,  (int)MarketplaceAdvisorOrderFieldIndex.InvoiceNumber, 50, 0, 0);
			this.AddElementFieldInfo("MarketplaceAdvisorOrderEntity", "ParcelID", typeof(System.Int64), false, false, false, false,  (int)MarketplaceAdvisorOrderFieldIndex.ParcelID, 0, 0, 19);
		}
		/// <summary>Inits MarketplaceAdvisorStoreEntity's FieldInfo objects</summary>
		private void InitMarketplaceAdvisorStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(MarketplaceAdvisorStoreFieldIndex), "MarketplaceAdvisorStoreEntity");
			this.AddElementFieldInfo("MarketplaceAdvisorStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)MarketplaceAdvisorStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("MarketplaceAdvisorStoreEntity", "Username", typeof(System.String), false, false, false, false,  (int)MarketplaceAdvisorStoreFieldIndex.Username, 50, 0, 0);
			this.AddElementFieldInfo("MarketplaceAdvisorStoreEntity", "Password", typeof(System.String), false, false, false, false,  (int)MarketplaceAdvisorStoreFieldIndex.Password, 50, 0, 0);
			this.AddElementFieldInfo("MarketplaceAdvisorStoreEntity", "AccountType", typeof(System.Int32), false, false, false, false,  (int)MarketplaceAdvisorStoreFieldIndex.AccountType, 0, 0, 10);
			this.AddElementFieldInfo("MarketplaceAdvisorStoreEntity", "DownloadFlags", typeof(System.Int32), false, false, false, false,  (int)MarketplaceAdvisorStoreFieldIndex.DownloadFlags, 0, 0, 10);
		}
		/// <summary>Inits MivaOrderItemAttributeEntity's FieldInfo objects</summary>
		private void InitMivaOrderItemAttributeEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(MivaOrderItemAttributeFieldIndex), "MivaOrderItemAttributeEntity");
			this.AddElementFieldInfo("MivaOrderItemAttributeEntity", "OrderItemAttributeID", typeof(System.Int64), true, false, true, false,  (int)MivaOrderItemAttributeFieldIndex.OrderItemAttributeID, 0, 0, 19);
			this.AddElementFieldInfo("MivaOrderItemAttributeEntity", "MivaOptionCode", typeof(System.String), false, false, false, false,  (int)MivaOrderItemAttributeFieldIndex.MivaOptionCode, 300, 0, 0);
			this.AddElementFieldInfo("MivaOrderItemAttributeEntity", "MivaAttributeID", typeof(System.Int32), false, false, false, false,  (int)MivaOrderItemAttributeFieldIndex.MivaAttributeID, 0, 0, 10);
			this.AddElementFieldInfo("MivaOrderItemAttributeEntity", "MivaAttributeCode", typeof(System.String), false, false, false, false,  (int)MivaOrderItemAttributeFieldIndex.MivaAttributeCode, 300, 0, 0);
		}
		/// <summary>Inits MivaStoreEntity's FieldInfo objects</summary>
		private void InitMivaStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(MivaStoreFieldIndex), "MivaStoreEntity");
			this.AddElementFieldInfo("MivaStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)MivaStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("MivaStoreEntity", "EncryptionPassphrase", typeof(System.String), false, false, false, false,  (int)MivaStoreFieldIndex.EncryptionPassphrase, 50, 0, 0);
			this.AddElementFieldInfo("MivaStoreEntity", "LiveManualOrderNumbers", typeof(System.Boolean), false, false, false, false,  (int)MivaStoreFieldIndex.LiveManualOrderNumbers, 0, 0, 0);
			this.AddElementFieldInfo("MivaStoreEntity", "SebenzaCheckoutDataEnabled", typeof(System.Boolean), false, false, false, false,  (int)MivaStoreFieldIndex.SebenzaCheckoutDataEnabled, 0, 0, 0);
			this.AddElementFieldInfo("MivaStoreEntity", "OnlineUpdateStrategy", typeof(System.Int32), false, false, false, false,  (int)MivaStoreFieldIndex.OnlineUpdateStrategy, 0, 0, 10);
			this.AddElementFieldInfo("MivaStoreEntity", "OnlineUpdateStatusChangeEmail", typeof(System.Boolean), false, false, false, false,  (int)MivaStoreFieldIndex.OnlineUpdateStatusChangeEmail, 0, 0, 0);
		}
		/// <summary>Inits NetworkSolutionsOrderEntity's FieldInfo objects</summary>
		private void InitNetworkSolutionsOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(NetworkSolutionsOrderFieldIndex), "NetworkSolutionsOrderEntity");
			this.AddElementFieldInfo("NetworkSolutionsOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)NetworkSolutionsOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("NetworkSolutionsOrderEntity", "NetworkSolutionsOrderID", typeof(System.Int64), false, false, false, false,  (int)NetworkSolutionsOrderFieldIndex.NetworkSolutionsOrderID, 0, 0, 19);
		}
		/// <summary>Inits NetworkSolutionsStoreEntity's FieldInfo objects</summary>
		private void InitNetworkSolutionsStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(NetworkSolutionsStoreFieldIndex), "NetworkSolutionsStoreEntity");
			this.AddElementFieldInfo("NetworkSolutionsStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)NetworkSolutionsStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("NetworkSolutionsStoreEntity", "UserToken", typeof(System.String), false, false, false, false,  (int)NetworkSolutionsStoreFieldIndex.UserToken, 50, 0, 0);
			this.AddElementFieldInfo("NetworkSolutionsStoreEntity", "DownloadOrderStatuses", typeof(System.String), false, false, false, false,  (int)NetworkSolutionsStoreFieldIndex.DownloadOrderStatuses, 50, 0, 0);
			this.AddElementFieldInfo("NetworkSolutionsStoreEntity", "StatusCodes", typeof(System.String), false, false, false, false,  (int)NetworkSolutionsStoreFieldIndex.StatusCodes, 2147483647, 0, 0);
			this.AddElementFieldInfo("NetworkSolutionsStoreEntity", "StoreUrl", typeof(System.String), false, false, false, false,  (int)NetworkSolutionsStoreFieldIndex.StoreUrl, 255, 0, 0);
		}
		/// <summary>Inits NeweggOrderEntity's FieldInfo objects</summary>
		private void InitNeweggOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(NeweggOrderFieldIndex), "NeweggOrderEntity");
			this.AddElementFieldInfo("NeweggOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)NeweggOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("NeweggOrderEntity", "InvoiceNumber", typeof(Nullable<System.Int64>), false, false, false, true,  (int)NeweggOrderFieldIndex.InvoiceNumber, 0, 0, 19);
			this.AddElementFieldInfo("NeweggOrderEntity", "RefundAmount", typeof(Nullable<System.Decimal>), false, false, false, true,  (int)NeweggOrderFieldIndex.RefundAmount, 0, 4, 19);
			this.AddElementFieldInfo("NeweggOrderEntity", "IsAutoVoid", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)NeweggOrderFieldIndex.IsAutoVoid, 0, 0, 0);
		}
		/// <summary>Inits NeweggOrderItemEntity's FieldInfo objects</summary>
		private void InitNeweggOrderItemEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(NeweggOrderItemFieldIndex), "NeweggOrderItemEntity");
			this.AddElementFieldInfo("NeweggOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, true, false,  (int)NeweggOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("NeweggOrderItemEntity", "SellerPartNumber", typeof(System.String), false, false, false, true,  (int)NeweggOrderItemFieldIndex.SellerPartNumber, 64, 0, 0);
			this.AddElementFieldInfo("NeweggOrderItemEntity", "NeweggItemNumber", typeof(System.String), false, false, false, true,  (int)NeweggOrderItemFieldIndex.NeweggItemNumber, 64, 0, 0);
			this.AddElementFieldInfo("NeweggOrderItemEntity", "ManufacturerPartNumber", typeof(System.String), false, false, false, true,  (int)NeweggOrderItemFieldIndex.ManufacturerPartNumber, 64, 0, 0);
			this.AddElementFieldInfo("NeweggOrderItemEntity", "ShippingStatusID", typeof(Nullable<System.Int32>), false, false, false, true,  (int)NeweggOrderItemFieldIndex.ShippingStatusID, 0, 0, 10);
			this.AddElementFieldInfo("NeweggOrderItemEntity", "ShippingStatusDescription", typeof(System.String), false, false, false, true,  (int)NeweggOrderItemFieldIndex.ShippingStatusDescription, 32, 0, 0);
			this.AddElementFieldInfo("NeweggOrderItemEntity", "QuantityShipped", typeof(Nullable<System.Int32>), false, false, false, true,  (int)NeweggOrderItemFieldIndex.QuantityShipped, 0, 0, 10);
		}
		/// <summary>Inits NeweggStoreEntity's FieldInfo objects</summary>
		private void InitNeweggStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(NeweggStoreFieldIndex), "NeweggStoreEntity");
			this.AddElementFieldInfo("NeweggStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)NeweggStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("NeweggStoreEntity", "SellerID", typeof(System.String), false, false, false, false,  (int)NeweggStoreFieldIndex.SellerID, 10, 0, 0);
			this.AddElementFieldInfo("NeweggStoreEntity", "SecretKey", typeof(System.String), false, false, false, false,  (int)NeweggStoreFieldIndex.SecretKey, 50, 0, 0);
			this.AddElementFieldInfo("NeweggStoreEntity", "ExcludeFulfilledByNewegg", typeof(System.Boolean), false, false, false, false,  (int)NeweggStoreFieldIndex.ExcludeFulfilledByNewegg, 0, 0, 0);
			this.AddElementFieldInfo("NeweggStoreEntity", "Channel", typeof(System.Int32), false, false, false, false,  (int)NeweggStoreFieldIndex.Channel, 0, 0, 10);
		}
		/// <summary>Inits NoteEntity's FieldInfo objects</summary>
		private void InitNoteEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(NoteFieldIndex), "NoteEntity");
			this.AddElementFieldInfo("NoteEntity", "NoteID", typeof(System.Int64), true, false, true, false,  (int)NoteFieldIndex.NoteID, 0, 0, 19);
			this.AddElementFieldInfo("NoteEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)NoteFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("NoteEntity", "EntityID", typeof(System.Int64), false, true, false, false,  (int)NoteFieldIndex.EntityID, 0, 0, 19);
			this.AddElementFieldInfo("NoteEntity", "UserID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)NoteFieldIndex.UserID, 0, 0, 19);
			this.AddElementFieldInfo("NoteEntity", "Edited", typeof(System.DateTime), false, false, false, false,  (int)NoteFieldIndex.Edited, 0, 0, 0);
			this.AddElementFieldInfo("NoteEntity", "Text", typeof(System.String), false, false, false, false,  (int)NoteFieldIndex.Text, 2147483647, 0, 0);
			this.AddElementFieldInfo("NoteEntity", "Source", typeof(System.Int32), false, false, false, false,  (int)NoteFieldIndex.Source, 0, 0, 10);
			this.AddElementFieldInfo("NoteEntity", "Visibility", typeof(System.Int32), false, false, false, false,  (int)NoteFieldIndex.Visibility, 0, 0, 10);
		}
		/// <summary>Inits ObjectLabelEntity's FieldInfo objects</summary>
		private void InitObjectLabelEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ObjectLabelFieldIndex), "ObjectLabelEntity");
			this.AddElementFieldInfo("ObjectLabelEntity", "EntityID", typeof(System.Int64), true, false, false, false,  (int)ObjectLabelFieldIndex.EntityID, 0, 0, 19);
			this.AddElementFieldInfo("ObjectLabelEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ObjectLabelFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("ObjectLabelEntity", "ObjectType", typeof(System.Int32), false, false, false, false,  (int)ObjectLabelFieldIndex.ObjectType, 0, 0, 10);
			this.AddElementFieldInfo("ObjectLabelEntity", "ParentID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)ObjectLabelFieldIndex.ParentID, 0, 0, 19);
			this.AddElementFieldInfo("ObjectLabelEntity", "Label", typeof(System.String), false, false, false, false,  (int)ObjectLabelFieldIndex.Label, 100, 0, 0);
			this.AddElementFieldInfo("ObjectLabelEntity", "IsDeleted", typeof(System.Boolean), false, false, false, false,  (int)ObjectLabelFieldIndex.IsDeleted, 0, 0, 0);
		}
		/// <summary>Inits ObjectReferenceEntity's FieldInfo objects</summary>
		private void InitObjectReferenceEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ObjectReferenceFieldIndex), "ObjectReferenceEntity");
			this.AddElementFieldInfo("ObjectReferenceEntity", "ObjectReferenceID", typeof(System.Int64), true, false, true, false,  (int)ObjectReferenceFieldIndex.ObjectReferenceID, 0, 0, 19);
			this.AddElementFieldInfo("ObjectReferenceEntity", "ConsumerID", typeof(System.Int64), false, false, false, false,  (int)ObjectReferenceFieldIndex.ConsumerID, 0, 0, 19);
			this.AddElementFieldInfo("ObjectReferenceEntity", "ReferenceKey", typeof(System.String), false, false, false, false,  (int)ObjectReferenceFieldIndex.ReferenceKey, 250, 0, 0);
			this.AddElementFieldInfo("ObjectReferenceEntity", "EntityID", typeof(System.Int64), false, false, false, false,  (int)ObjectReferenceFieldIndex.EntityID, 0, 0, 19);
			this.AddElementFieldInfo("ObjectReferenceEntity", "Reason", typeof(System.String), false, false, false, true,  (int)ObjectReferenceFieldIndex.Reason, 250, 0, 0);
		}
		/// <summary>Inits OdbcStoreEntity's FieldInfo objects</summary>
		private void InitOdbcStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(OdbcStoreFieldIndex), "OdbcStoreEntity");
			this.AddElementFieldInfo("OdbcStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)OdbcStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("OdbcStoreEntity", "ImportConnectionString", typeof(System.String), false, false, false, false,  (int)OdbcStoreFieldIndex.ImportConnectionString, 2048, 0, 0);
			this.AddElementFieldInfo("OdbcStoreEntity", "ImportMap", typeof(System.String), false, false, false, false,  (int)OdbcStoreFieldIndex.ImportMap, 2147483647, 0, 0);
			this.AddElementFieldInfo("OdbcStoreEntity", "ImportStrategy", typeof(System.Int32), false, false, false, false,  (int)OdbcStoreFieldIndex.ImportStrategy, 0, 0, 10);
			this.AddElementFieldInfo("OdbcStoreEntity", "ImportColumnSourceType", typeof(System.Int32), false, false, false, false,  (int)OdbcStoreFieldIndex.ImportColumnSourceType, 0, 0, 10);
			this.AddElementFieldInfo("OdbcStoreEntity", "ImportColumnSource", typeof(System.String), false, false, false, false,  (int)OdbcStoreFieldIndex.ImportColumnSource, 2048, 0, 0);
			this.AddElementFieldInfo("OdbcStoreEntity", "UploadStrategy", typeof(System.Int32), false, false, false, false,  (int)OdbcStoreFieldIndex.UploadStrategy, 0, 0, 10);
			this.AddElementFieldInfo("OdbcStoreEntity", "UploadMap", typeof(System.String), false, false, false, false,  (int)OdbcStoreFieldIndex.UploadMap, 2147483647, 0, 0);
			this.AddElementFieldInfo("OdbcStoreEntity", "UploadColumnSourceType", typeof(System.Int32), false, false, false, false,  (int)OdbcStoreFieldIndex.UploadColumnSourceType, 0, 0, 10);
			this.AddElementFieldInfo("OdbcStoreEntity", "UploadColumnSource", typeof(System.String), false, false, false, false,  (int)OdbcStoreFieldIndex.UploadColumnSource, 2048, 0, 0);
			this.AddElementFieldInfo("OdbcStoreEntity", "UploadConnectionString", typeof(System.String), false, false, false, false,  (int)OdbcStoreFieldIndex.UploadConnectionString, 2048, 0, 0);
			this.AddElementFieldInfo("OdbcStoreEntity", "ImportOrderItemStrategy", typeof(System.Int32), false, false, false, false,  (int)OdbcStoreFieldIndex.ImportOrderItemStrategy, 0, 0, 10);
		}
		/// <summary>Inits OnTracAccountEntity's FieldInfo objects</summary>
		private void InitOnTracAccountEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(OnTracAccountFieldIndex), "OnTracAccountEntity");
			this.AddElementFieldInfo("OnTracAccountEntity", "OnTracAccountID", typeof(System.Int64), true, false, true, false,  (int)OnTracAccountFieldIndex.OnTracAccountID, 0, 0, 19);
			this.AddElementFieldInfo("OnTracAccountEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)OnTracAccountFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("OnTracAccountEntity", "AccountNumber", typeof(System.Int32), false, false, false, false,  (int)OnTracAccountFieldIndex.AccountNumber, 0, 0, 10);
			this.AddElementFieldInfo("OnTracAccountEntity", "Password", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.Password, 50, 0, 0);
			this.AddElementFieldInfo("OnTracAccountEntity", "Description", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.Description, 50, 0, 0);
			this.AddElementFieldInfo("OnTracAccountEntity", "FirstName", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.FirstName, 30, 0, 0);
			this.AddElementFieldInfo("OnTracAccountEntity", "MiddleName", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.MiddleName, 30, 0, 0);
			this.AddElementFieldInfo("OnTracAccountEntity", "LastName", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.LastName, 30, 0, 0);
			this.AddElementFieldInfo("OnTracAccountEntity", "Company", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.Company, 30, 0, 0);
			this.AddElementFieldInfo("OnTracAccountEntity", "Street1", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.Street1, 43, 0, 0);
			this.AddElementFieldInfo("OnTracAccountEntity", "City", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.City, 25, 0, 0);
			this.AddElementFieldInfo("OnTracAccountEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.StateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("OnTracAccountEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.PostalCode, 10, 0, 0);
			this.AddElementFieldInfo("OnTracAccountEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.CountryCode, 50, 0, 0);
			this.AddElementFieldInfo("OnTracAccountEntity", "Email", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.Email, 50, 0, 0);
			this.AddElementFieldInfo("OnTracAccountEntity", "Phone", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.Phone, 15, 0, 0);
		}
		/// <summary>Inits OnTracProfileEntity's FieldInfo objects</summary>
		private void InitOnTracProfileEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(OnTracProfileFieldIndex), "OnTracProfileEntity");
			this.AddElementFieldInfo("OnTracProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)OnTracProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			this.AddElementFieldInfo("OnTracProfileEntity", "OnTracAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)OnTracProfileFieldIndex.OnTracAccountID, 0, 0, 19);
			this.AddElementFieldInfo("OnTracProfileEntity", "ResidentialDetermination", typeof(Nullable<System.Int32>), false, false, false, true,  (int)OnTracProfileFieldIndex.ResidentialDetermination, 0, 0, 10);
			this.AddElementFieldInfo("OnTracProfileEntity", "Service", typeof(Nullable<System.Int32>), false, false, false, true,  (int)OnTracProfileFieldIndex.Service, 0, 0, 10);
			this.AddElementFieldInfo("OnTracProfileEntity", "SaturdayDelivery", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)OnTracProfileFieldIndex.SaturdayDelivery, 0, 0, 0);
			this.AddElementFieldInfo("OnTracProfileEntity", "SignatureRequired", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)OnTracProfileFieldIndex.SignatureRequired, 0, 0, 0);
			this.AddElementFieldInfo("OnTracProfileEntity", "PackagingType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)OnTracProfileFieldIndex.PackagingType, 0, 0, 10);
			this.AddElementFieldInfo("OnTracProfileEntity", "Weight", typeof(Nullable<System.Double>), false, false, false, true,  (int)OnTracProfileFieldIndex.Weight, 0, 0, 38);
			this.AddElementFieldInfo("OnTracProfileEntity", "DimsProfileID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)OnTracProfileFieldIndex.DimsProfileID, 0, 0, 19);
			this.AddElementFieldInfo("OnTracProfileEntity", "DimsLength", typeof(Nullable<System.Double>), false, false, false, true,  (int)OnTracProfileFieldIndex.DimsLength, 0, 0, 38);
			this.AddElementFieldInfo("OnTracProfileEntity", "DimsWidth", typeof(Nullable<System.Double>), false, false, false, true,  (int)OnTracProfileFieldIndex.DimsWidth, 0, 0, 38);
			this.AddElementFieldInfo("OnTracProfileEntity", "DimsHeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)OnTracProfileFieldIndex.DimsHeight, 0, 0, 38);
			this.AddElementFieldInfo("OnTracProfileEntity", "DimsWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)OnTracProfileFieldIndex.DimsWeight, 0, 0, 38);
			this.AddElementFieldInfo("OnTracProfileEntity", "DimsAddWeight", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)OnTracProfileFieldIndex.DimsAddWeight, 0, 0, 0);
			this.AddElementFieldInfo("OnTracProfileEntity", "Reference1", typeof(System.String), false, false, false, true,  (int)OnTracProfileFieldIndex.Reference1, 300, 0, 0);
			this.AddElementFieldInfo("OnTracProfileEntity", "Reference2", typeof(System.String), false, false, false, true,  (int)OnTracProfileFieldIndex.Reference2, 300, 0, 0);
			this.AddElementFieldInfo("OnTracProfileEntity", "Instructions", typeof(System.String), false, false, false, true,  (int)OnTracProfileFieldIndex.Instructions, 300, 0, 0);
		}
		/// <summary>Inits OnTracShipmentEntity's FieldInfo objects</summary>
		private void InitOnTracShipmentEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(OnTracShipmentFieldIndex), "OnTracShipmentEntity");
			this.AddElementFieldInfo("OnTracShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)OnTracShipmentFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("OnTracShipmentEntity", "OnTracAccountID", typeof(System.Int64), false, false, false, false,  (int)OnTracShipmentFieldIndex.OnTracAccountID, 0, 0, 19);
			this.AddElementFieldInfo("OnTracShipmentEntity", "Service", typeof(System.Int32), false, false, false, false,  (int)OnTracShipmentFieldIndex.Service, 0, 0, 10);
			this.AddElementFieldInfo("OnTracShipmentEntity", "IsCod", typeof(System.Boolean), false, false, false, false,  (int)OnTracShipmentFieldIndex.IsCod, 0, 0, 0);
			this.AddElementFieldInfo("OnTracShipmentEntity", "CodType", typeof(System.Int32), false, false, false, false,  (int)OnTracShipmentFieldIndex.CodType, 0, 0, 10);
			this.AddElementFieldInfo("OnTracShipmentEntity", "CodAmount", typeof(System.Decimal), false, false, false, false,  (int)OnTracShipmentFieldIndex.CodAmount, 0, 4, 19);
			this.AddElementFieldInfo("OnTracShipmentEntity", "SaturdayDelivery", typeof(System.Boolean), false, false, false, false,  (int)OnTracShipmentFieldIndex.SaturdayDelivery, 0, 0, 0);
			this.AddElementFieldInfo("OnTracShipmentEntity", "SignatureRequired", typeof(System.Boolean), false, false, false, false,  (int)OnTracShipmentFieldIndex.SignatureRequired, 0, 0, 0);
			this.AddElementFieldInfo("OnTracShipmentEntity", "PackagingType", typeof(System.Int32), false, false, false, false,  (int)OnTracShipmentFieldIndex.PackagingType, 0, 0, 10);
			this.AddElementFieldInfo("OnTracShipmentEntity", "Instructions", typeof(System.String), false, false, false, false,  (int)OnTracShipmentFieldIndex.Instructions, 300, 0, 0);
			this.AddElementFieldInfo("OnTracShipmentEntity", "DimsProfileID", typeof(System.Int64), false, false, false, false,  (int)OnTracShipmentFieldIndex.DimsProfileID, 0, 0, 19);
			this.AddElementFieldInfo("OnTracShipmentEntity", "DimsLength", typeof(System.Double), false, false, false, false,  (int)OnTracShipmentFieldIndex.DimsLength, 0, 0, 38);
			this.AddElementFieldInfo("OnTracShipmentEntity", "DimsWidth", typeof(System.Double), false, false, false, false,  (int)OnTracShipmentFieldIndex.DimsWidth, 0, 0, 38);
			this.AddElementFieldInfo("OnTracShipmentEntity", "DimsHeight", typeof(System.Double), false, false, false, false,  (int)OnTracShipmentFieldIndex.DimsHeight, 0, 0, 38);
			this.AddElementFieldInfo("OnTracShipmentEntity", "DimsWeight", typeof(System.Double), false, false, false, false,  (int)OnTracShipmentFieldIndex.DimsWeight, 0, 0, 38);
			this.AddElementFieldInfo("OnTracShipmentEntity", "DimsAddWeight", typeof(System.Boolean), false, false, false, false,  (int)OnTracShipmentFieldIndex.DimsAddWeight, 0, 0, 0);
			this.AddElementFieldInfo("OnTracShipmentEntity", "Reference1", typeof(System.String), false, false, false, false,  (int)OnTracShipmentFieldIndex.Reference1, 300, 0, 0);
			this.AddElementFieldInfo("OnTracShipmentEntity", "Reference2", typeof(System.String), false, false, false, false,  (int)OnTracShipmentFieldIndex.Reference2, 300, 0, 0);
			this.AddElementFieldInfo("OnTracShipmentEntity", "InsuranceValue", typeof(System.Decimal), false, false, false, false,  (int)OnTracShipmentFieldIndex.InsuranceValue, 0, 4, 19);
			this.AddElementFieldInfo("OnTracShipmentEntity", "InsurancePennyOne", typeof(System.Boolean), false, false, false, false,  (int)OnTracShipmentFieldIndex.InsurancePennyOne, 0, 0, 0);
			this.AddElementFieldInfo("OnTracShipmentEntity", "DeclaredValue", typeof(System.Decimal), false, false, false, false,  (int)OnTracShipmentFieldIndex.DeclaredValue, 0, 4, 19);
			this.AddElementFieldInfo("OnTracShipmentEntity", "RequestedLabelFormat", typeof(System.Int32), false, false, false, false,  (int)OnTracShipmentFieldIndex.RequestedLabelFormat, 0, 0, 10);
		}
		/// <summary>Inits OrderEntity's FieldInfo objects</summary>
		private void InitOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(OrderFieldIndex), "OrderEntity");
			this.AddElementFieldInfo("OrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)OrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("OrderEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)OrderFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "StoreID", typeof(System.Int64), false, true, false, false,  (int)OrderFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("OrderEntity", "CustomerID", typeof(System.Int64), false, true, false, false,  (int)OrderFieldIndex.CustomerID, 0, 0, 19);
			this.AddElementFieldInfo("OrderEntity", "OrderNumber", typeof(System.Int64), false, false, false, false,  (int)OrderFieldIndex.OrderNumber, 0, 0, 19);
			this.AddElementFieldInfo("OrderEntity", "OrderNumberComplete", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.OrderNumberComplete, 50, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "OrderDate", typeof(System.DateTime), false, false, false, false,  (int)OrderFieldIndex.OrderDate, 0, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "OrderTotal", typeof(System.Decimal), false, false, false, false,  (int)OrderFieldIndex.OrderTotal, 0, 4, 19);
			this.AddElementFieldInfo("OrderEntity", "LocalStatus", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.LocalStatus, 100, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "IsManual", typeof(System.Boolean), false, false, false, false,  (int)OrderFieldIndex.IsManual, 0, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "OnlineLastModified", typeof(System.DateTime), false, false, false, false,  (int)OrderFieldIndex.OnlineLastModified, 0, 0, 7);
			this.AddElementFieldInfo("OrderEntity", "OnlineCustomerID", typeof(System.Object), false, false, false, true,  (int)OrderFieldIndex.OnlineCustomerID, 0, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "OnlineStatus", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.OnlineStatus, 100, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "OnlineStatusCode", typeof(System.Object), false, false, false, true,  (int)OrderFieldIndex.OnlineStatusCode, 0, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "RequestedShipping", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.RequestedShipping, 50, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillFirstName", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillFirstName, 30, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillMiddleName", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillMiddleName, 30, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillLastName", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillLastName, 30, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillCompany", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillCompany, 60, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillStreet1", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillStreet1, 60, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillStreet2", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillStreet2, 60, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillStreet3", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillStreet3, 60, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillCity", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillCity, 50, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillStateProvCode", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillStateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillPostalCode", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillPostalCode, 20, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillCountryCode", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillCountryCode, 50, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillPhone", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillPhone, 25, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillFax", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillFax, 35, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillEmail", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillEmail, 100, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillWebsite", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillWebsite, 50, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillAddressValidationSuggestionCount", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.BillAddressValidationSuggestionCount, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "BillAddressValidationStatus", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.BillAddressValidationStatus, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "BillAddressValidationError", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillAddressValidationError, 300, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "BillResidentialStatus", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.BillResidentialStatus, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "BillPOBox", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.BillPOBox, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "BillUSTerritory", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.BillUSTerritory, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "BillMilitaryAddress", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.BillMilitaryAddress, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "ShipFirstName", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipFirstName, 30, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipMiddleName", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipMiddleName, 30, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipLastName", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipLastName, 30, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipCompany", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipCompany, 60, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipStreet1", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipStreet1, 60, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipStreet2", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipStreet2, 60, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipStreet3", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipStreet3, 60, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipCity", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipCity, 50, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipStateProvCode", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipStateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipPostalCode", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipPostalCode, 20, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipCountryCode", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipCountryCode, 50, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipPhone", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipPhone, 25, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipFax", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipFax, 35, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipEmail", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipEmail, 100, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipWebsite", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipWebsite, 50, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipAddressValidationSuggestionCount", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipAddressValidationSuggestionCount, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "ShipAddressValidationStatus", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipAddressValidationStatus, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "ShipAddressValidationError", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipAddressValidationError, 300, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipResidentialStatus", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipResidentialStatus, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "ShipPOBox", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipPOBox, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "ShipUSTerritory", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipUSTerritory, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "ShipMilitaryAddress", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipMilitaryAddress, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "RollupItemCount", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.RollupItemCount, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "RollupItemName", typeof(System.String), false, false, true, true,  (int)OrderFieldIndex.RollupItemName, 300, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "RollupItemCode", typeof(System.String), false, false, true, true,  (int)OrderFieldIndex.RollupItemCode, 300, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "RollupItemSKU", typeof(System.String), false, false, true, true,  (int)OrderFieldIndex.RollupItemSKU, 100, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "RollupItemLocation", typeof(System.String), false, false, true, true,  (int)OrderFieldIndex.RollupItemLocation, 255, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "RollupItemQuantity", typeof(Nullable<System.Double>), false, false, true, true,  (int)OrderFieldIndex.RollupItemQuantity, 0, 0, 38);
			this.AddElementFieldInfo("OrderEntity", "RollupItemTotalWeight", typeof(System.Double), false, false, false, false,  (int)OrderFieldIndex.RollupItemTotalWeight, 0, 0, 38);
			this.AddElementFieldInfo("OrderEntity", "RollupNoteCount", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.RollupNoteCount, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "BillNameParseStatus", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.BillNameParseStatus, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "BillUnparsedName", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillUnparsedName, 100, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipNameParseStatus", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipNameParseStatus, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "ShipUnparsedName", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipUnparsedName, 100, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipSenseHashKey", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipSenseHashKey, 64, 0, 0);
			this.AddElementFieldInfo("OrderEntity", "ShipSenseRecognitionStatus", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipSenseRecognitionStatus, 0, 0, 10);
			this.AddElementFieldInfo("OrderEntity", "ShipAddressType", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipAddressType, 0, 0, 10);
		}
		/// <summary>Inits OrderChargeEntity's FieldInfo objects</summary>
		private void InitOrderChargeEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(OrderChargeFieldIndex), "OrderChargeEntity");
			this.AddElementFieldInfo("OrderChargeEntity", "OrderChargeID", typeof(System.Int64), true, false, true, false,  (int)OrderChargeFieldIndex.OrderChargeID, 0, 0, 19);
			this.AddElementFieldInfo("OrderChargeEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)OrderChargeFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("OrderChargeEntity", "OrderID", typeof(System.Int64), false, true, false, false,  (int)OrderChargeFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("OrderChargeEntity", "Type", typeof(System.String), false, false, false, false,  (int)OrderChargeFieldIndex.Type, 50, 0, 0);
			this.AddElementFieldInfo("OrderChargeEntity", "Description", typeof(System.String), false, false, false, false,  (int)OrderChargeFieldIndex.Description, 255, 0, 0);
			this.AddElementFieldInfo("OrderChargeEntity", "Amount", typeof(System.Decimal), false, false, false, false,  (int)OrderChargeFieldIndex.Amount, 0, 4, 19);
		}
		/// <summary>Inits OrderItemEntity's FieldInfo objects</summary>
		private void InitOrderItemEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(OrderItemFieldIndex), "OrderItemEntity");
			this.AddElementFieldInfo("OrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, true, false,  (int)OrderItemFieldIndex.OrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("OrderItemEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)OrderItemFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("OrderItemEntity", "OrderID", typeof(System.Int64), false, true, false, false,  (int)OrderItemFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("OrderItemEntity", "Name", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.Name, 300, 0, 0);
			this.AddElementFieldInfo("OrderItemEntity", "Code", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.Code, 300, 0, 0);
			this.AddElementFieldInfo("OrderItemEntity", "SKU", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.SKU, 100, 0, 0);
			this.AddElementFieldInfo("OrderItemEntity", "ISBN", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.ISBN, 30, 0, 0);
			this.AddElementFieldInfo("OrderItemEntity", "UPC", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.UPC, 30, 0, 0);
			this.AddElementFieldInfo("OrderItemEntity", "Description", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.Description, 2147483647, 0, 0);
			this.AddElementFieldInfo("OrderItemEntity", "Location", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.Location, 255, 0, 0);
			this.AddElementFieldInfo("OrderItemEntity", "Image", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.Image, 2147483647, 0, 0);
			this.AddElementFieldInfo("OrderItemEntity", "Thumbnail", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.Thumbnail, 2147483647, 0, 0);
			this.AddElementFieldInfo("OrderItemEntity", "UnitPrice", typeof(System.Decimal), false, false, false, false,  (int)OrderItemFieldIndex.UnitPrice, 0, 4, 19);
			this.AddElementFieldInfo("OrderItemEntity", "UnitCost", typeof(System.Decimal), false, false, false, false,  (int)OrderItemFieldIndex.UnitCost, 0, 4, 19);
			this.AddElementFieldInfo("OrderItemEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)OrderItemFieldIndex.Weight, 0, 0, 38);
			this.AddElementFieldInfo("OrderItemEntity", "Quantity", typeof(System.Double), false, false, false, false,  (int)OrderItemFieldIndex.Quantity, 0, 0, 38);
			this.AddElementFieldInfo("OrderItemEntity", "LocalStatus", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.LocalStatus, 255, 0, 0);
			this.AddElementFieldInfo("OrderItemEntity", "IsManual", typeof(System.Boolean), false, false, false, false,  (int)OrderItemFieldIndex.IsManual, 0, 0, 0);
		}
		/// <summary>Inits OrderItemAttributeEntity's FieldInfo objects</summary>
		private void InitOrderItemAttributeEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(OrderItemAttributeFieldIndex), "OrderItemAttributeEntity");
			this.AddElementFieldInfo("OrderItemAttributeEntity", "OrderItemAttributeID", typeof(System.Int64), true, false, true, false,  (int)OrderItemAttributeFieldIndex.OrderItemAttributeID, 0, 0, 19);
			this.AddElementFieldInfo("OrderItemAttributeEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)OrderItemAttributeFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("OrderItemAttributeEntity", "OrderItemID", typeof(System.Int64), false, true, false, false,  (int)OrderItemAttributeFieldIndex.OrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("OrderItemAttributeEntity", "Name", typeof(System.String), false, false, false, false,  (int)OrderItemAttributeFieldIndex.Name, 300, 0, 0);
			this.AddElementFieldInfo("OrderItemAttributeEntity", "Description", typeof(System.String), false, false, false, false,  (int)OrderItemAttributeFieldIndex.Description, 2147483647, 0, 0);
			this.AddElementFieldInfo("OrderItemAttributeEntity", "UnitPrice", typeof(System.Decimal), false, false, false, false,  (int)OrderItemAttributeFieldIndex.UnitPrice, 0, 4, 19);
			this.AddElementFieldInfo("OrderItemAttributeEntity", "IsManual", typeof(System.Boolean), false, false, false, false,  (int)OrderItemAttributeFieldIndex.IsManual, 0, 0, 0);
		}
		/// <summary>Inits OrderMotionOrderEntity's FieldInfo objects</summary>
		private void InitOrderMotionOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(OrderMotionOrderFieldIndex), "OrderMotionOrderEntity");
			this.AddElementFieldInfo("OrderMotionOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)OrderMotionOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("OrderMotionOrderEntity", "OrderMotionShipmentID", typeof(System.Int32), false, false, false, false,  (int)OrderMotionOrderFieldIndex.OrderMotionShipmentID, 0, 0, 10);
			this.AddElementFieldInfo("OrderMotionOrderEntity", "OrderMotionPromotion", typeof(System.String), false, false, false, false,  (int)OrderMotionOrderFieldIndex.OrderMotionPromotion, 50, 0, 0);
			this.AddElementFieldInfo("OrderMotionOrderEntity", "OrderMotionInvoiceNumber", typeof(System.String), false, false, false, false,  (int)OrderMotionOrderFieldIndex.OrderMotionInvoiceNumber, 64, 0, 0);
		}
		/// <summary>Inits OrderMotionStoreEntity's FieldInfo objects</summary>
		private void InitOrderMotionStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(OrderMotionStoreFieldIndex), "OrderMotionStoreEntity");
			this.AddElementFieldInfo("OrderMotionStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)OrderMotionStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("OrderMotionStoreEntity", "OrderMotionEmailAccountID", typeof(System.Int64), false, true, false, false,  (int)OrderMotionStoreFieldIndex.OrderMotionEmailAccountID, 0, 0, 19);
			this.AddElementFieldInfo("OrderMotionStoreEntity", "OrderMotionBizID", typeof(System.String), false, false, false, false,  (int)OrderMotionStoreFieldIndex.OrderMotionBizID, 2147483647, 0, 0);
		}
		/// <summary>Inits OrderPaymentDetailEntity's FieldInfo objects</summary>
		private void InitOrderPaymentDetailEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(OrderPaymentDetailFieldIndex), "OrderPaymentDetailEntity");
			this.AddElementFieldInfo("OrderPaymentDetailEntity", "OrderPaymentDetailID", typeof(System.Int64), true, false, true, false,  (int)OrderPaymentDetailFieldIndex.OrderPaymentDetailID, 0, 0, 19);
			this.AddElementFieldInfo("OrderPaymentDetailEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)OrderPaymentDetailFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("OrderPaymentDetailEntity", "OrderID", typeof(System.Int64), false, true, false, false,  (int)OrderPaymentDetailFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("OrderPaymentDetailEntity", "Label", typeof(System.String), false, false, false, false,  (int)OrderPaymentDetailFieldIndex.Label, 100, 0, 0);
			this.AddElementFieldInfo("OrderPaymentDetailEntity", "Value", typeof(System.String), false, false, false, false,  (int)OrderPaymentDetailFieldIndex.Value, 100, 0, 0);
		}
		/// <summary>Inits OtherProfileEntity's FieldInfo objects</summary>
		private void InitOtherProfileEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(OtherProfileFieldIndex), "OtherProfileEntity");
			this.AddElementFieldInfo("OtherProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)OtherProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			this.AddElementFieldInfo("OtherProfileEntity", "Carrier", typeof(System.String), false, false, false, true,  (int)OtherProfileFieldIndex.Carrier, 50, 0, 0);
			this.AddElementFieldInfo("OtherProfileEntity", "Service", typeof(System.String), false, false, false, true,  (int)OtherProfileFieldIndex.Service, 50, 0, 0);
		}
		/// <summary>Inits OtherShipmentEntity's FieldInfo objects</summary>
		private void InitOtherShipmentEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(OtherShipmentFieldIndex), "OtherShipmentEntity");
			this.AddElementFieldInfo("OtherShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)OtherShipmentFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("OtherShipmentEntity", "Carrier", typeof(System.String), false, false, false, false,  (int)OtherShipmentFieldIndex.Carrier, 50, 0, 0);
			this.AddElementFieldInfo("OtherShipmentEntity", "Service", typeof(System.String), false, false, false, false,  (int)OtherShipmentFieldIndex.Service, 50, 0, 0);
			this.AddElementFieldInfo("OtherShipmentEntity", "InsuranceValue", typeof(System.Decimal), false, false, false, false,  (int)OtherShipmentFieldIndex.InsuranceValue, 0, 4, 19);
		}
		/// <summary>Inits PayPalOrderEntity's FieldInfo objects</summary>
		private void InitPayPalOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(PayPalOrderFieldIndex), "PayPalOrderEntity");
			this.AddElementFieldInfo("PayPalOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)PayPalOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("PayPalOrderEntity", "TransactionID", typeof(System.String), false, false, false, false,  (int)PayPalOrderFieldIndex.TransactionID, 50, 0, 0);
			this.AddElementFieldInfo("PayPalOrderEntity", "AddressStatus", typeof(System.Int32), false, false, false, false,  (int)PayPalOrderFieldIndex.AddressStatus, 0, 0, 10);
			this.AddElementFieldInfo("PayPalOrderEntity", "PayPalFee", typeof(System.Decimal), false, false, false, false,  (int)PayPalOrderFieldIndex.PayPalFee, 0, 4, 19);
			this.AddElementFieldInfo("PayPalOrderEntity", "PaymentStatus", typeof(System.Int32), false, false, false, false,  (int)PayPalOrderFieldIndex.PaymentStatus, 0, 0, 10);
		}
		/// <summary>Inits PayPalStoreEntity's FieldInfo objects</summary>
		private void InitPayPalStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(PayPalStoreFieldIndex), "PayPalStoreEntity");
			this.AddElementFieldInfo("PayPalStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)PayPalStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("PayPalStoreEntity", "ApiCredentialType", typeof(System.Int16), false, false, false, false,  (int)PayPalStoreFieldIndex.ApiCredentialType, 0, 0, 5);
			this.AddElementFieldInfo("PayPalStoreEntity", "ApiUserName", typeof(System.String), false, false, false, false,  (int)PayPalStoreFieldIndex.ApiUserName, 255, 0, 0);
			this.AddElementFieldInfo("PayPalStoreEntity", "ApiPassword", typeof(System.String), false, false, false, false,  (int)PayPalStoreFieldIndex.ApiPassword, 80, 0, 0);
			this.AddElementFieldInfo("PayPalStoreEntity", "ApiSignature", typeof(System.String), false, false, false, false,  (int)PayPalStoreFieldIndex.ApiSignature, 80, 0, 0);
			this.AddElementFieldInfo("PayPalStoreEntity", "ApiCertificate", typeof(System.Byte[]), false, false, false, true,  (int)PayPalStoreFieldIndex.ApiCertificate, 2048, 0, 0);
			this.AddElementFieldInfo("PayPalStoreEntity", "LastTransactionDate", typeof(System.DateTime), false, false, false, false,  (int)PayPalStoreFieldIndex.LastTransactionDate, 0, 0, 0);
			this.AddElementFieldInfo("PayPalStoreEntity", "LastValidTransactionDate", typeof(System.DateTime), false, false, false, false,  (int)PayPalStoreFieldIndex.LastValidTransactionDate, 0, 0, 0);
		}
		/// <summary>Inits PermissionEntity's FieldInfo objects</summary>
		private void InitPermissionEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(PermissionFieldIndex), "PermissionEntity");
			this.AddElementFieldInfo("PermissionEntity", "PermissionID", typeof(System.Int64), true, false, true, false,  (int)PermissionFieldIndex.PermissionID, 0, 0, 19);
			this.AddElementFieldInfo("PermissionEntity", "UserID", typeof(System.Int64), false, true, false, false,  (int)PermissionFieldIndex.UserID, 0, 0, 19);
			this.AddElementFieldInfo("PermissionEntity", "PermissionType", typeof(System.Int32), false, false, false, false,  (int)PermissionFieldIndex.PermissionType, 0, 0, 10);
			this.AddElementFieldInfo("PermissionEntity", "EntityID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)PermissionFieldIndex.EntityID, 0, 0, 19);
		}
		/// <summary>Inits PostalProfileEntity's FieldInfo objects</summary>
		private void InitPostalProfileEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(PostalProfileFieldIndex), "PostalProfileEntity");
			this.AddElementFieldInfo("PostalProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)PostalProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			this.AddElementFieldInfo("PostalProfileEntity", "Service", typeof(Nullable<System.Int32>), false, false, false, true,  (int)PostalProfileFieldIndex.Service, 0, 0, 10);
			this.AddElementFieldInfo("PostalProfileEntity", "Confirmation", typeof(Nullable<System.Int32>), false, false, false, true,  (int)PostalProfileFieldIndex.Confirmation, 0, 0, 10);
			this.AddElementFieldInfo("PostalProfileEntity", "Weight", typeof(Nullable<System.Double>), false, false, false, true,  (int)PostalProfileFieldIndex.Weight, 0, 0, 38);
			this.AddElementFieldInfo("PostalProfileEntity", "PackagingType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)PostalProfileFieldIndex.PackagingType, 0, 0, 10);
			this.AddElementFieldInfo("PostalProfileEntity", "DimsProfileID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)PostalProfileFieldIndex.DimsProfileID, 0, 0, 19);
			this.AddElementFieldInfo("PostalProfileEntity", "DimsLength", typeof(Nullable<System.Double>), false, false, false, true,  (int)PostalProfileFieldIndex.DimsLength, 0, 0, 38);
			this.AddElementFieldInfo("PostalProfileEntity", "DimsWidth", typeof(Nullable<System.Double>), false, false, false, true,  (int)PostalProfileFieldIndex.DimsWidth, 0, 0, 38);
			this.AddElementFieldInfo("PostalProfileEntity", "DimsHeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)PostalProfileFieldIndex.DimsHeight, 0, 0, 38);
			this.AddElementFieldInfo("PostalProfileEntity", "DimsWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)PostalProfileFieldIndex.DimsWeight, 0, 0, 38);
			this.AddElementFieldInfo("PostalProfileEntity", "DimsAddWeight", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)PostalProfileFieldIndex.DimsAddWeight, 0, 0, 0);
			this.AddElementFieldInfo("PostalProfileEntity", "NonRectangular", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)PostalProfileFieldIndex.NonRectangular, 0, 0, 0);
			this.AddElementFieldInfo("PostalProfileEntity", "NonMachinable", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)PostalProfileFieldIndex.NonMachinable, 0, 0, 0);
			this.AddElementFieldInfo("PostalProfileEntity", "CustomsContentType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)PostalProfileFieldIndex.CustomsContentType, 0, 0, 10);
			this.AddElementFieldInfo("PostalProfileEntity", "CustomsContentDescription", typeof(System.String), false, false, false, true,  (int)PostalProfileFieldIndex.CustomsContentDescription, 50, 0, 0);
			this.AddElementFieldInfo("PostalProfileEntity", "ExpressSignatureWaiver", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)PostalProfileFieldIndex.ExpressSignatureWaiver, 0, 0, 0);
			this.AddElementFieldInfo("PostalProfileEntity", "SortType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)PostalProfileFieldIndex.SortType, 0, 0, 10);
			this.AddElementFieldInfo("PostalProfileEntity", "EntryFacility", typeof(Nullable<System.Int32>), false, false, false, true,  (int)PostalProfileFieldIndex.EntryFacility, 0, 0, 10);
			this.AddElementFieldInfo("PostalProfileEntity", "Memo1", typeof(System.String), false, false, false, true,  (int)PostalProfileFieldIndex.Memo1, 300, 0, 0);
			this.AddElementFieldInfo("PostalProfileEntity", "Memo2", typeof(System.String), false, false, false, true,  (int)PostalProfileFieldIndex.Memo2, 300, 0, 0);
			this.AddElementFieldInfo("PostalProfileEntity", "Memo3", typeof(System.String), false, false, false, true,  (int)PostalProfileFieldIndex.Memo3, 300, 0, 0);
			this.AddElementFieldInfo("PostalProfileEntity", "NoPostage", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)PostalProfileFieldIndex.NoPostage, 0, 0, 0);
		}
		/// <summary>Inits PostalShipmentEntity's FieldInfo objects</summary>
		private void InitPostalShipmentEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(PostalShipmentFieldIndex), "PostalShipmentEntity");
			this.AddElementFieldInfo("PostalShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)PostalShipmentFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("PostalShipmentEntity", "Service", typeof(System.Int32), false, false, false, false,  (int)PostalShipmentFieldIndex.Service, 0, 0, 10);
			this.AddElementFieldInfo("PostalShipmentEntity", "Confirmation", typeof(System.Int32), false, false, false, false,  (int)PostalShipmentFieldIndex.Confirmation, 0, 0, 10);
			this.AddElementFieldInfo("PostalShipmentEntity", "PackagingType", typeof(System.Int32), false, false, false, false,  (int)PostalShipmentFieldIndex.PackagingType, 0, 0, 10);
			this.AddElementFieldInfo("PostalShipmentEntity", "DimsProfileID", typeof(System.Int64), false, false, false, false,  (int)PostalShipmentFieldIndex.DimsProfileID, 0, 0, 19);
			this.AddElementFieldInfo("PostalShipmentEntity", "DimsLength", typeof(System.Double), false, false, false, false,  (int)PostalShipmentFieldIndex.DimsLength, 0, 0, 38);
			this.AddElementFieldInfo("PostalShipmentEntity", "DimsWidth", typeof(System.Double), false, false, false, false,  (int)PostalShipmentFieldIndex.DimsWidth, 0, 0, 38);
			this.AddElementFieldInfo("PostalShipmentEntity", "DimsHeight", typeof(System.Double), false, false, false, false,  (int)PostalShipmentFieldIndex.DimsHeight, 0, 0, 38);
			this.AddElementFieldInfo("PostalShipmentEntity", "DimsWeight", typeof(System.Double), false, false, false, false,  (int)PostalShipmentFieldIndex.DimsWeight, 0, 0, 38);
			this.AddElementFieldInfo("PostalShipmentEntity", "DimsAddWeight", typeof(System.Boolean), false, false, false, false,  (int)PostalShipmentFieldIndex.DimsAddWeight, 0, 0, 0);
			this.AddElementFieldInfo("PostalShipmentEntity", "NonRectangular", typeof(System.Boolean), false, false, false, false,  (int)PostalShipmentFieldIndex.NonRectangular, 0, 0, 0);
			this.AddElementFieldInfo("PostalShipmentEntity", "NonMachinable", typeof(System.Boolean), false, false, false, false,  (int)PostalShipmentFieldIndex.NonMachinable, 0, 0, 0);
			this.AddElementFieldInfo("PostalShipmentEntity", "CustomsContentType", typeof(System.Int32), false, false, false, false,  (int)PostalShipmentFieldIndex.CustomsContentType, 0, 0, 10);
			this.AddElementFieldInfo("PostalShipmentEntity", "CustomsContentDescription", typeof(System.String), false, false, false, false,  (int)PostalShipmentFieldIndex.CustomsContentDescription, 50, 0, 0);
			this.AddElementFieldInfo("PostalShipmentEntity", "InsuranceValue", typeof(System.Decimal), false, false, false, false,  (int)PostalShipmentFieldIndex.InsuranceValue, 0, 4, 19);
			this.AddElementFieldInfo("PostalShipmentEntity", "ExpressSignatureWaiver", typeof(System.Boolean), false, false, false, false,  (int)PostalShipmentFieldIndex.ExpressSignatureWaiver, 0, 0, 0);
			this.AddElementFieldInfo("PostalShipmentEntity", "SortType", typeof(System.Int32), false, false, false, false,  (int)PostalShipmentFieldIndex.SortType, 0, 0, 10);
			this.AddElementFieldInfo("PostalShipmentEntity", "EntryFacility", typeof(System.Int32), false, false, false, false,  (int)PostalShipmentFieldIndex.EntryFacility, 0, 0, 10);
			this.AddElementFieldInfo("PostalShipmentEntity", "Memo1", typeof(System.String), false, false, false, false,  (int)PostalShipmentFieldIndex.Memo1, 300, 0, 0);
			this.AddElementFieldInfo("PostalShipmentEntity", "Memo2", typeof(System.String), false, false, false, false,  (int)PostalShipmentFieldIndex.Memo2, 300, 0, 0);
			this.AddElementFieldInfo("PostalShipmentEntity", "Memo3", typeof(System.String), false, false, false, false,  (int)PostalShipmentFieldIndex.Memo3, 300, 0, 0);
			this.AddElementFieldInfo("PostalShipmentEntity", "NoPostage", typeof(System.Boolean), false, false, false, false,  (int)PostalShipmentFieldIndex.NoPostage, 0, 0, 0);
		}
		/// <summary>Inits PrintResultEntity's FieldInfo objects</summary>
		private void InitPrintResultEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(PrintResultFieldIndex), "PrintResultEntity");
			this.AddElementFieldInfo("PrintResultEntity", "PrintResultID", typeof(System.Int64), true, false, true, false,  (int)PrintResultFieldIndex.PrintResultID, 0, 0, 19);
			this.AddElementFieldInfo("PrintResultEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)PrintResultFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("PrintResultEntity", "JobIdentifier", typeof(System.Guid), false, false, false, false,  (int)PrintResultFieldIndex.JobIdentifier, 0, 0, 0);
			this.AddElementFieldInfo("PrintResultEntity", "RelatedObjectID", typeof(System.Int64), false, false, false, false,  (int)PrintResultFieldIndex.RelatedObjectID, 0, 0, 19);
			this.AddElementFieldInfo("PrintResultEntity", "ContextObjectID", typeof(System.Int64), false, false, false, false,  (int)PrintResultFieldIndex.ContextObjectID, 0, 0, 19);
			this.AddElementFieldInfo("PrintResultEntity", "TemplateID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)PrintResultFieldIndex.TemplateID, 0, 0, 19);
			this.AddElementFieldInfo("PrintResultEntity", "TemplateType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)PrintResultFieldIndex.TemplateType, 0, 0, 10);
			this.AddElementFieldInfo("PrintResultEntity", "OutputFormat", typeof(Nullable<System.Int32>), false, false, false, true,  (int)PrintResultFieldIndex.OutputFormat, 0, 0, 10);
			this.AddElementFieldInfo("PrintResultEntity", "LabelSheetID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)PrintResultFieldIndex.LabelSheetID, 0, 0, 19);
			this.AddElementFieldInfo("PrintResultEntity", "ComputerID", typeof(System.Int64), false, true, false, false,  (int)PrintResultFieldIndex.ComputerID, 0, 0, 19);
			this.AddElementFieldInfo("PrintResultEntity", "ContentResourceID", typeof(System.Int64), false, false, false, false,  (int)PrintResultFieldIndex.ContentResourceID, 0, 0, 19);
			this.AddElementFieldInfo("PrintResultEntity", "PrintDate", typeof(System.DateTime), false, false, false, false,  (int)PrintResultFieldIndex.PrintDate, 0, 0, 0);
			this.AddElementFieldInfo("PrintResultEntity", "PrinterName", typeof(System.String), false, false, false, false,  (int)PrintResultFieldIndex.PrinterName, 350, 0, 0);
			this.AddElementFieldInfo("PrintResultEntity", "PaperSource", typeof(System.Int32), false, false, false, false,  (int)PrintResultFieldIndex.PaperSource, 0, 0, 10);
			this.AddElementFieldInfo("PrintResultEntity", "PaperSourceName", typeof(System.String), false, false, false, false,  (int)PrintResultFieldIndex.PaperSourceName, 100, 0, 0);
			this.AddElementFieldInfo("PrintResultEntity", "Copies", typeof(System.Int32), false, false, false, false,  (int)PrintResultFieldIndex.Copies, 0, 0, 10);
			this.AddElementFieldInfo("PrintResultEntity", "Collated", typeof(System.Boolean), false, false, false, false,  (int)PrintResultFieldIndex.Collated, 0, 0, 0);
			this.AddElementFieldInfo("PrintResultEntity", "PageMarginLeft", typeof(System.Double), false, false, false, false,  (int)PrintResultFieldIndex.PageMarginLeft, 0, 0, 38);
			this.AddElementFieldInfo("PrintResultEntity", "PageMarginRight", typeof(System.Double), false, false, false, false,  (int)PrintResultFieldIndex.PageMarginRight, 0, 0, 38);
			this.AddElementFieldInfo("PrintResultEntity", "PageMarginBottom", typeof(System.Double), false, false, false, false,  (int)PrintResultFieldIndex.PageMarginBottom, 0, 0, 38);
			this.AddElementFieldInfo("PrintResultEntity", "PageMarginTop", typeof(System.Double), false, false, false, false,  (int)PrintResultFieldIndex.PageMarginTop, 0, 0, 38);
			this.AddElementFieldInfo("PrintResultEntity", "PageWidth", typeof(System.Double), false, false, false, false,  (int)PrintResultFieldIndex.PageWidth, 0, 0, 38);
			this.AddElementFieldInfo("PrintResultEntity", "PageHeight", typeof(System.Double), false, false, false, false,  (int)PrintResultFieldIndex.PageHeight, 0, 0, 38);
		}
		/// <summary>Inits ProStoresOrderEntity's FieldInfo objects</summary>
		private void InitProStoresOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ProStoresOrderFieldIndex), "ProStoresOrderEntity");
			this.AddElementFieldInfo("ProStoresOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)ProStoresOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("ProStoresOrderEntity", "ConfirmationNumber", typeof(System.String), false, false, false, false,  (int)ProStoresOrderFieldIndex.ConfirmationNumber, 12, 0, 0);
			this.AddElementFieldInfo("ProStoresOrderEntity", "AuthorizedDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ProStoresOrderFieldIndex.AuthorizedDate, 0, 0, 0);
			this.AddElementFieldInfo("ProStoresOrderEntity", "AuthorizedBy", typeof(System.String), false, false, false, false,  (int)ProStoresOrderFieldIndex.AuthorizedBy, 50, 0, 0);
		}
		/// <summary>Inits ProStoresStoreEntity's FieldInfo objects</summary>
		private void InitProStoresStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ProStoresStoreFieldIndex), "ProStoresStoreEntity");
			this.AddElementFieldInfo("ProStoresStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)ProStoresStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("ProStoresStoreEntity", "ShortName", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ShortName, 30, 0, 0);
			this.AddElementFieldInfo("ProStoresStoreEntity", "Username", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.Username, 50, 0, 0);
			this.AddElementFieldInfo("ProStoresStoreEntity", "LoginMethod", typeof(System.Int32), false, false, false, false,  (int)ProStoresStoreFieldIndex.LoginMethod, 0, 0, 10);
			this.AddElementFieldInfo("ProStoresStoreEntity", "ApiEntryPoint", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ApiEntryPoint, 300, 0, 0);
			this.AddElementFieldInfo("ProStoresStoreEntity", "ApiToken", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ApiToken, 2147483647, 0, 0);
			this.AddElementFieldInfo("ProStoresStoreEntity", "ApiStorefrontUrl", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ApiStorefrontUrl, 300, 0, 0);
			this.AddElementFieldInfo("ProStoresStoreEntity", "ApiTokenLogonUrl", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ApiTokenLogonUrl, 300, 0, 0);
			this.AddElementFieldInfo("ProStoresStoreEntity", "ApiXteUrl", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ApiXteUrl, 300, 0, 0);
			this.AddElementFieldInfo("ProStoresStoreEntity", "ApiRestSecureUrl", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ApiRestSecureUrl, 300, 0, 0);
			this.AddElementFieldInfo("ProStoresStoreEntity", "ApiRestNonSecureUrl", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ApiRestNonSecureUrl, 300, 0, 0);
			this.AddElementFieldInfo("ProStoresStoreEntity", "ApiRestScriptSuffix", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ApiRestScriptSuffix, 50, 0, 0);
			this.AddElementFieldInfo("ProStoresStoreEntity", "LegacyAdminUrl", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.LegacyAdminUrl, 300, 0, 0);
			this.AddElementFieldInfo("ProStoresStoreEntity", "LegacyXtePath", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.LegacyXtePath, 75, 0, 0);
			this.AddElementFieldInfo("ProStoresStoreEntity", "LegacyPrefix", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.LegacyPrefix, 30, 0, 0);
			this.AddElementFieldInfo("ProStoresStoreEntity", "LegacyPassword", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.LegacyPassword, 150, 0, 0);
			this.AddElementFieldInfo("ProStoresStoreEntity", "LegacyCanUpgrade", typeof(System.Boolean), false, false, false, false,  (int)ProStoresStoreFieldIndex.LegacyCanUpgrade, 0, 0, 0);
		}
		/// <summary>Inits ResourceEntity's FieldInfo objects</summary>
		private void InitResourceEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ResourceFieldIndex), "ResourceEntity");
			this.AddElementFieldInfo("ResourceEntity", "ResourceID", typeof(System.Int64), true, false, true, false,  (int)ResourceFieldIndex.ResourceID, 0, 0, 19);
			this.AddElementFieldInfo("ResourceEntity", "Data", typeof(System.Byte[]), false, false, false, false,  (int)ResourceFieldIndex.Data, 2147483647, 0, 0);
			this.AddElementFieldInfo("ResourceEntity", "Checksum", typeof(System.Byte[]), false, false, false, false,  (int)ResourceFieldIndex.Checksum, 32, 0, 0);
			this.AddElementFieldInfo("ResourceEntity", "Compressed", typeof(System.Boolean), false, false, false, false,  (int)ResourceFieldIndex.Compressed, 0, 0, 0);
			this.AddElementFieldInfo("ResourceEntity", "Filename", typeof(System.String), false, false, false, false,  (int)ResourceFieldIndex.Filename, 30, 0, 0);
		}
		/// <summary>Inits ScanFormBatchEntity's FieldInfo objects</summary>
		private void InitScanFormBatchEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ScanFormBatchFieldIndex), "ScanFormBatchEntity");
			this.AddElementFieldInfo("ScanFormBatchEntity", "ScanFormBatchID", typeof(System.Int64), true, false, true, false,  (int)ScanFormBatchFieldIndex.ScanFormBatchID, 0, 0, 19);
			this.AddElementFieldInfo("ScanFormBatchEntity", "ShipmentType", typeof(System.Int32), false, false, false, false,  (int)ScanFormBatchFieldIndex.ShipmentType, 0, 0, 10);
			this.AddElementFieldInfo("ScanFormBatchEntity", "CreatedDate", typeof(System.DateTime), false, false, false, false,  (int)ScanFormBatchFieldIndex.CreatedDate, 0, 0, 0);
			this.AddElementFieldInfo("ScanFormBatchEntity", "ShipmentCount", typeof(System.Int32), false, false, false, false,  (int)ScanFormBatchFieldIndex.ShipmentCount, 0, 0, 10);
		}
		/// <summary>Inits SearchEntity's FieldInfo objects</summary>
		private void InitSearchEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(SearchFieldIndex), "SearchEntity");
			this.AddElementFieldInfo("SearchEntity", "SearchID", typeof(System.Int64), true, false, true, false,  (int)SearchFieldIndex.SearchID, 0, 0, 19);
			this.AddElementFieldInfo("SearchEntity", "Started", typeof(System.DateTime), false, false, false, false,  (int)SearchFieldIndex.Started, 0, 0, 0);
			this.AddElementFieldInfo("SearchEntity", "Pinged", typeof(System.DateTime), false, false, false, false,  (int)SearchFieldIndex.Pinged, 0, 0, 0);
			this.AddElementFieldInfo("SearchEntity", "FilterNodeID", typeof(System.Int64), false, false, false, false,  (int)SearchFieldIndex.FilterNodeID, 0, 0, 19);
			this.AddElementFieldInfo("SearchEntity", "UserID", typeof(System.Int64), false, false, false, false,  (int)SearchFieldIndex.UserID, 0, 0, 19);
			this.AddElementFieldInfo("SearchEntity", "ComputerID", typeof(System.Int64), false, false, false, false,  (int)SearchFieldIndex.ComputerID, 0, 0, 19);
		}
		/// <summary>Inits SearsOrderEntity's FieldInfo objects</summary>
		private void InitSearsOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(SearsOrderFieldIndex), "SearsOrderEntity");
			this.AddElementFieldInfo("SearsOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)SearsOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("SearsOrderEntity", "PoNumber", typeof(System.String), false, false, false, false,  (int)SearsOrderFieldIndex.PoNumber, 30, 0, 0);
			this.AddElementFieldInfo("SearsOrderEntity", "PoNumberWithDate", typeof(System.String), false, false, false, false,  (int)SearsOrderFieldIndex.PoNumberWithDate, 30, 0, 0);
			this.AddElementFieldInfo("SearsOrderEntity", "LocationID", typeof(System.Int32), false, false, false, false,  (int)SearsOrderFieldIndex.LocationID, 0, 0, 10);
			this.AddElementFieldInfo("SearsOrderEntity", "Commission", typeof(System.Decimal), false, false, false, false,  (int)SearsOrderFieldIndex.Commission, 0, 4, 19);
			this.AddElementFieldInfo("SearsOrderEntity", "CustomerPickup", typeof(System.Boolean), false, false, false, false,  (int)SearsOrderFieldIndex.CustomerPickup, 0, 0, 0);
		}
		/// <summary>Inits SearsOrderItemEntity's FieldInfo objects</summary>
		private void InitSearsOrderItemEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(SearsOrderItemFieldIndex), "SearsOrderItemEntity");
			this.AddElementFieldInfo("SearsOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, true, false,  (int)SearsOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("SearsOrderItemEntity", "LineNumber", typeof(System.Int32), false, false, false, false,  (int)SearsOrderItemFieldIndex.LineNumber, 0, 0, 10);
			this.AddElementFieldInfo("SearsOrderItemEntity", "ItemID", typeof(System.String), false, false, false, false,  (int)SearsOrderItemFieldIndex.ItemID, 300, 0, 0);
			this.AddElementFieldInfo("SearsOrderItemEntity", "Commission", typeof(System.Decimal), false, false, false, false,  (int)SearsOrderItemFieldIndex.Commission, 0, 4, 19);
			this.AddElementFieldInfo("SearsOrderItemEntity", "Shipping", typeof(System.Decimal), false, false, false, false,  (int)SearsOrderItemFieldIndex.Shipping, 0, 4, 19);
			this.AddElementFieldInfo("SearsOrderItemEntity", "OnlineStatus", typeof(System.String), false, false, false, false,  (int)SearsOrderItemFieldIndex.OnlineStatus, 20, 0, 0);
		}
		/// <summary>Inits SearsStoreEntity's FieldInfo objects</summary>
		private void InitSearsStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(SearsStoreFieldIndex), "SearsStoreEntity");
			this.AddElementFieldInfo("SearsStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)SearsStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("SearsStoreEntity", "SearsEmail", typeof(System.String), false, false, false, false,  (int)SearsStoreFieldIndex.SearsEmail, 75, 0, 0);
			this.AddElementFieldInfo("SearsStoreEntity", "Password", typeof(System.String), false, false, false, false,  (int)SearsStoreFieldIndex.Password, 75, 0, 0);
			this.AddElementFieldInfo("SearsStoreEntity", "SecretKey", typeof(System.String), false, false, false, false,  (int)SearsStoreFieldIndex.SecretKey, 255, 0, 0);
			this.AddElementFieldInfo("SearsStoreEntity", "SellerID", typeof(System.String), false, false, false, false,  (int)SearsStoreFieldIndex.SellerID, 15, 0, 0);
		}
		/// <summary>Inits ServerMessageEntity's FieldInfo objects</summary>
		private void InitServerMessageEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ServerMessageFieldIndex), "ServerMessageEntity");
			this.AddElementFieldInfo("ServerMessageEntity", "ServerMessageID", typeof(System.Int64), true, false, true, false,  (int)ServerMessageFieldIndex.ServerMessageID, 0, 0, 19);
			this.AddElementFieldInfo("ServerMessageEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ServerMessageFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("ServerMessageEntity", "Number", typeof(System.Int32), false, false, false, false,  (int)ServerMessageFieldIndex.Number, 0, 0, 10);
			this.AddElementFieldInfo("ServerMessageEntity", "Published", typeof(System.DateTime), false, false, false, false,  (int)ServerMessageFieldIndex.Published, 0, 0, 0);
			this.AddElementFieldInfo("ServerMessageEntity", "Active", typeof(System.Boolean), false, false, false, false,  (int)ServerMessageFieldIndex.Active, 0, 0, 0);
			this.AddElementFieldInfo("ServerMessageEntity", "Dismissable", typeof(System.Boolean), false, false, false, false,  (int)ServerMessageFieldIndex.Dismissable, 0, 0, 0);
			this.AddElementFieldInfo("ServerMessageEntity", "Expires", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ServerMessageFieldIndex.Expires, 0, 0, 0);
			this.AddElementFieldInfo("ServerMessageEntity", "ResponseTo", typeof(Nullable<System.Int32>), false, false, false, true,  (int)ServerMessageFieldIndex.ResponseTo, 0, 0, 10);
			this.AddElementFieldInfo("ServerMessageEntity", "ResponseAction", typeof(Nullable<System.Int32>), false, false, false, true,  (int)ServerMessageFieldIndex.ResponseAction, 0, 0, 10);
			this.AddElementFieldInfo("ServerMessageEntity", "EditTo", typeof(Nullable<System.Int32>), false, false, false, true,  (int)ServerMessageFieldIndex.EditTo, 0, 0, 10);
			this.AddElementFieldInfo("ServerMessageEntity", "Image", typeof(System.String), false, false, false, false,  (int)ServerMessageFieldIndex.Image, 350, 0, 0);
			this.AddElementFieldInfo("ServerMessageEntity", "PrimaryText", typeof(System.String), false, false, false, false,  (int)ServerMessageFieldIndex.PrimaryText, 30, 0, 0);
			this.AddElementFieldInfo("ServerMessageEntity", "SecondaryText", typeof(System.String), false, false, false, false,  (int)ServerMessageFieldIndex.SecondaryText, 60, 0, 0);
			this.AddElementFieldInfo("ServerMessageEntity", "Actions", typeof(System.String), false, false, false, false,  (int)ServerMessageFieldIndex.Actions, 1073741823, 0, 0);
			this.AddElementFieldInfo("ServerMessageEntity", "Stores", typeof(System.String), false, false, false, false,  (int)ServerMessageFieldIndex.Stores, 1073741823, 0, 0);
			this.AddElementFieldInfo("ServerMessageEntity", "Shippers", typeof(System.String), false, false, false, false,  (int)ServerMessageFieldIndex.Shippers, 1073741823, 0, 0);
		}
		/// <summary>Inits ServerMessageSignoffEntity's FieldInfo objects</summary>
		private void InitServerMessageSignoffEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ServerMessageSignoffFieldIndex), "ServerMessageSignoffEntity");
			this.AddElementFieldInfo("ServerMessageSignoffEntity", "ServerMessageSignoffID", typeof(System.Int64), true, false, true, false,  (int)ServerMessageSignoffFieldIndex.ServerMessageSignoffID, 0, 0, 19);
			this.AddElementFieldInfo("ServerMessageSignoffEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ServerMessageSignoffFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("ServerMessageSignoffEntity", "ServerMessageID", typeof(System.Int64), false, true, false, false,  (int)ServerMessageSignoffFieldIndex.ServerMessageID, 0, 0, 19);
			this.AddElementFieldInfo("ServerMessageSignoffEntity", "UserID", typeof(System.Int64), false, false, false, false,  (int)ServerMessageSignoffFieldIndex.UserID, 0, 0, 19);
			this.AddElementFieldInfo("ServerMessageSignoffEntity", "ComputerID", typeof(System.Int64), false, true, false, false,  (int)ServerMessageSignoffFieldIndex.ComputerID, 0, 0, 19);
			this.AddElementFieldInfo("ServerMessageSignoffEntity", "Dismissed", typeof(System.DateTime), false, false, false, false,  (int)ServerMessageSignoffFieldIndex.Dismissed, 0, 0, 0);
		}
		/// <summary>Inits ServiceStatusEntity's FieldInfo objects</summary>
		private void InitServiceStatusEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ServiceStatusFieldIndex), "ServiceStatusEntity");
			this.AddElementFieldInfo("ServiceStatusEntity", "ServiceStatusID", typeof(System.Int64), true, false, true, false,  (int)ServiceStatusFieldIndex.ServiceStatusID, 0, 0, 19);
			this.AddElementFieldInfo("ServiceStatusEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ServiceStatusFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("ServiceStatusEntity", "ComputerID", typeof(System.Int64), false, true, false, false,  (int)ServiceStatusFieldIndex.ComputerID, 0, 0, 19);
			this.AddElementFieldInfo("ServiceStatusEntity", "ServiceType", typeof(System.Int32), false, false, false, false,  (int)ServiceStatusFieldIndex.ServiceType, 0, 0, 10);
			this.AddElementFieldInfo("ServiceStatusEntity", "LastStartDateTime", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ServiceStatusFieldIndex.LastStartDateTime, 0, 0, 0);
			this.AddElementFieldInfo("ServiceStatusEntity", "LastStopDateTime", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ServiceStatusFieldIndex.LastStopDateTime, 0, 0, 0);
			this.AddElementFieldInfo("ServiceStatusEntity", "LastCheckInDateTime", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ServiceStatusFieldIndex.LastCheckInDateTime, 0, 0, 0);
			this.AddElementFieldInfo("ServiceStatusEntity", "ServiceFullName", typeof(System.String), false, false, false, false,  (int)ServiceStatusFieldIndex.ServiceFullName, 256, 0, 0);
			this.AddElementFieldInfo("ServiceStatusEntity", "ServiceDisplayName", typeof(System.String), false, false, false, false,  (int)ServiceStatusFieldIndex.ServiceDisplayName, 256, 0, 0);
		}
		/// <summary>Inits ShipmentEntity's FieldInfo objects</summary>
		private void InitShipmentEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ShipmentFieldIndex), "ShipmentEntity");
			this.AddElementFieldInfo("ShipmentEntity", "ShipmentID", typeof(System.Int64), true, false, true, false,  (int)ShipmentFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("ShipmentEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ShipmentFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OrderID", typeof(System.Int64), false, true, false, false,  (int)ShipmentFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("ShipmentEntity", "ShipmentType", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipmentType, 0, 0, 10);
			this.AddElementFieldInfo("ShipmentEntity", "ContentWeight", typeof(System.Double), false, false, false, false,  (int)ShipmentFieldIndex.ContentWeight, 0, 0, 38);
			this.AddElementFieldInfo("ShipmentEntity", "TotalWeight", typeof(System.Double), false, false, false, false,  (int)ShipmentFieldIndex.TotalWeight, 0, 0, 38);
			this.AddElementFieldInfo("ShipmentEntity", "Processed", typeof(System.Boolean), false, false, false, false,  (int)ShipmentFieldIndex.Processed, 0, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ProcessedDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ShipmentFieldIndex.ProcessedDate, 0, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ProcessedUserID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)ShipmentFieldIndex.ProcessedUserID, 0, 0, 19);
			this.AddElementFieldInfo("ShipmentEntity", "ProcessedComputerID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)ShipmentFieldIndex.ProcessedComputerID, 0, 0, 19);
			this.AddElementFieldInfo("ShipmentEntity", "ShipDate", typeof(System.DateTime), false, false, false, false,  (int)ShipmentFieldIndex.ShipDate, 0, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ShipmentCost", typeof(System.Decimal), false, false, false, false,  (int)ShipmentFieldIndex.ShipmentCost, 0, 4, 19);
			this.AddElementFieldInfo("ShipmentEntity", "Voided", typeof(System.Boolean), false, false, false, false,  (int)ShipmentFieldIndex.Voided, 0, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "VoidedDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ShipmentFieldIndex.VoidedDate, 0, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "VoidedUserID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)ShipmentFieldIndex.VoidedUserID, 0, 0, 19);
			this.AddElementFieldInfo("ShipmentEntity", "VoidedComputerID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)ShipmentFieldIndex.VoidedComputerID, 0, 0, 19);
			this.AddElementFieldInfo("ShipmentEntity", "TrackingNumber", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.TrackingNumber, 50, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "CustomsGenerated", typeof(System.Boolean), false, false, false, false,  (int)ShipmentFieldIndex.CustomsGenerated, 0, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "CustomsValue", typeof(System.Decimal), false, false, false, false,  (int)ShipmentFieldIndex.CustomsValue, 0, 4, 19);
			this.AddElementFieldInfo("ShipmentEntity", "RequestedLabelFormat", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.RequestedLabelFormat, 0, 0, 10);
			this.AddElementFieldInfo("ShipmentEntity", "ActualLabelFormat", typeof(Nullable<System.Int32>), false, false, false, true,  (int)ShipmentFieldIndex.ActualLabelFormat, 0, 0, 10);
			this.AddElementFieldInfo("ShipmentEntity", "ShipFirstName", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipFirstName, 30, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ShipMiddleName", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipMiddleName, 30, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ShipLastName", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipLastName, 30, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ShipCompany", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipCompany, 60, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ShipStreet1", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipStreet1, 60, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ShipStreet2", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipStreet2, 60, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ShipStreet3", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipStreet3, 60, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ShipCity", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipCity, 50, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ShipStateProvCode", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipStateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ShipPostalCode", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipPostalCode, 20, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ShipCountryCode", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipCountryCode, 50, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ShipPhone", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipPhone, 25, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ShipEmail", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipEmail, 100, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ShipAddressValidationSuggestionCount", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipAddressValidationSuggestionCount, 0, 0, 10);
			this.AddElementFieldInfo("ShipmentEntity", "ShipAddressValidationStatus", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipAddressValidationStatus, 0, 0, 10);
			this.AddElementFieldInfo("ShipmentEntity", "ShipAddressValidationError", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipAddressValidationError, 300, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ShipResidentialStatus", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipResidentialStatus, 0, 0, 10);
			this.AddElementFieldInfo("ShipmentEntity", "ShipPOBox", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipPOBox, 0, 0, 10);
			this.AddElementFieldInfo("ShipmentEntity", "ShipUSTerritory", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipUSTerritory, 0, 0, 10);
			this.AddElementFieldInfo("ShipmentEntity", "ShipMilitaryAddress", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipMilitaryAddress, 0, 0, 10);
			this.AddElementFieldInfo("ShipmentEntity", "ResidentialDetermination", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ResidentialDetermination, 0, 0, 10);
			this.AddElementFieldInfo("ShipmentEntity", "ResidentialResult", typeof(System.Boolean), false, false, false, false,  (int)ShipmentFieldIndex.ResidentialResult, 0, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OriginOriginID", typeof(System.Int64), false, false, false, false,  (int)ShipmentFieldIndex.OriginOriginID, 0, 0, 19);
			this.AddElementFieldInfo("ShipmentEntity", "OriginFirstName", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginFirstName, 30, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OriginMiddleName", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginMiddleName, 30, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OriginLastName", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginLastName, 30, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OriginCompany", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginCompany, 60, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OriginStreet1", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginStreet1, 60, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OriginStreet2", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginStreet2, 60, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OriginStreet3", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginStreet3, 60, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OriginCity", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginCity, 50, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OriginStateProvCode", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginStateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OriginPostalCode", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginPostalCode, 20, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OriginCountryCode", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginCountryCode, 50, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OriginPhone", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginPhone, 25, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OriginFax", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginFax, 35, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OriginEmail", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginEmail, 100, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OriginWebsite", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginWebsite, 50, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ReturnShipment", typeof(System.Boolean), false, false, false, false,  (int)ShipmentFieldIndex.ReturnShipment, 0, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "Insurance", typeof(System.Boolean), false, false, false, false,  (int)ShipmentFieldIndex.Insurance, 0, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "InsuranceProvider", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.InsuranceProvider, 0, 0, 10);
			this.AddElementFieldInfo("ShipmentEntity", "ShipNameParseStatus", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipNameParseStatus, 0, 0, 10);
			this.AddElementFieldInfo("ShipmentEntity", "ShipUnparsedName", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipUnparsedName, 100, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OriginNameParseStatus", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.OriginNameParseStatus, 0, 0, 10);
			this.AddElementFieldInfo("ShipmentEntity", "OriginUnparsedName", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginUnparsedName, 100, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "BestRateEvents", typeof(System.Byte), false, false, false, false,  (int)ShipmentFieldIndex.BestRateEvents, 0, 0, 3);
			this.AddElementFieldInfo("ShipmentEntity", "ShipSenseStatus", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipSenseStatus, 0, 0, 10);
			this.AddElementFieldInfo("ShipmentEntity", "ShipSenseChangeSets", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipSenseChangeSets, 2147483647, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "ShipSenseEntry", typeof(System.Byte[]), false, false, false, false,  (int)ShipmentFieldIndex.ShipSenseEntry, 2147483647, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "OnlineShipmentID", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OnlineShipmentID, 128, 0, 0);
			this.AddElementFieldInfo("ShipmentEntity", "BilledType", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.BilledType, 0, 0, 10);
			this.AddElementFieldInfo("ShipmentEntity", "BilledWeight", typeof(System.Double), false, false, false, false,  (int)ShipmentFieldIndex.BilledWeight, 0, 0, 38);
		}
		/// <summary>Inits ShipmentCustomsItemEntity's FieldInfo objects</summary>
		private void InitShipmentCustomsItemEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ShipmentCustomsItemFieldIndex), "ShipmentCustomsItemEntity");
			this.AddElementFieldInfo("ShipmentCustomsItemEntity", "ShipmentCustomsItemID", typeof(System.Int64), true, false, true, false,  (int)ShipmentCustomsItemFieldIndex.ShipmentCustomsItemID, 0, 0, 19);
			this.AddElementFieldInfo("ShipmentCustomsItemEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ShipmentCustomsItemFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("ShipmentCustomsItemEntity", "ShipmentID", typeof(System.Int64), false, true, false, false,  (int)ShipmentCustomsItemFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("ShipmentCustomsItemEntity", "Description", typeof(System.String), false, false, false, false,  (int)ShipmentCustomsItemFieldIndex.Description, 150, 0, 0);
			this.AddElementFieldInfo("ShipmentCustomsItemEntity", "Quantity", typeof(System.Double), false, false, false, false,  (int)ShipmentCustomsItemFieldIndex.Quantity, 0, 0, 38);
			this.AddElementFieldInfo("ShipmentCustomsItemEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)ShipmentCustomsItemFieldIndex.Weight, 0, 0, 38);
			this.AddElementFieldInfo("ShipmentCustomsItemEntity", "UnitValue", typeof(System.Decimal), false, false, false, false,  (int)ShipmentCustomsItemFieldIndex.UnitValue, 0, 4, 19);
			this.AddElementFieldInfo("ShipmentCustomsItemEntity", "CountryOfOrigin", typeof(System.String), false, false, false, false,  (int)ShipmentCustomsItemFieldIndex.CountryOfOrigin, 50, 0, 0);
			this.AddElementFieldInfo("ShipmentCustomsItemEntity", "HarmonizedCode", typeof(System.String), false, false, false, false,  (int)ShipmentCustomsItemFieldIndex.HarmonizedCode, 14, 0, 0);
			this.AddElementFieldInfo("ShipmentCustomsItemEntity", "NumberOfPieces", typeof(System.Int32), false, false, false, false,  (int)ShipmentCustomsItemFieldIndex.NumberOfPieces, 0, 0, 10);
			this.AddElementFieldInfo("ShipmentCustomsItemEntity", "UnitPriceAmount", typeof(System.Decimal), false, false, false, false,  (int)ShipmentCustomsItemFieldIndex.UnitPriceAmount, 0, 4, 19);
		}
		/// <summary>Inits ShippingDefaultsRuleEntity's FieldInfo objects</summary>
		private void InitShippingDefaultsRuleEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ShippingDefaultsRuleFieldIndex), "ShippingDefaultsRuleEntity");
			this.AddElementFieldInfo("ShippingDefaultsRuleEntity", "ShippingDefaultsRuleID", typeof(System.Int64), true, false, true, false,  (int)ShippingDefaultsRuleFieldIndex.ShippingDefaultsRuleID, 0, 0, 19);
			this.AddElementFieldInfo("ShippingDefaultsRuleEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ShippingDefaultsRuleFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("ShippingDefaultsRuleEntity", "ShipmentType", typeof(System.Int32), false, false, false, false,  (int)ShippingDefaultsRuleFieldIndex.ShipmentType, 0, 0, 10);
			this.AddElementFieldInfo("ShippingDefaultsRuleEntity", "FilterNodeID", typeof(System.Int64), false, false, false, false,  (int)ShippingDefaultsRuleFieldIndex.FilterNodeID, 0, 0, 19);
			this.AddElementFieldInfo("ShippingDefaultsRuleEntity", "ShippingProfileID", typeof(System.Int64), false, false, false, false,  (int)ShippingDefaultsRuleFieldIndex.ShippingProfileID, 0, 0, 19);
			this.AddElementFieldInfo("ShippingDefaultsRuleEntity", "Position", typeof(System.Int32), false, false, false, false,  (int)ShippingDefaultsRuleFieldIndex.Position, 0, 0, 10);
		}
		/// <summary>Inits ShippingOriginEntity's FieldInfo objects</summary>
		private void InitShippingOriginEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ShippingOriginFieldIndex), "ShippingOriginEntity");
			this.AddElementFieldInfo("ShippingOriginEntity", "ShippingOriginID", typeof(System.Int64), true, false, true, false,  (int)ShippingOriginFieldIndex.ShippingOriginID, 0, 0, 19);
			this.AddElementFieldInfo("ShippingOriginEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ShippingOriginFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("ShippingOriginEntity", "Description", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Description, 50, 0, 0);
			this.AddElementFieldInfo("ShippingOriginEntity", "FirstName", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.FirstName, 30, 0, 0);
			this.AddElementFieldInfo("ShippingOriginEntity", "MiddleName", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.MiddleName, 30, 0, 0);
			this.AddElementFieldInfo("ShippingOriginEntity", "LastName", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.LastName, 30, 0, 0);
			this.AddElementFieldInfo("ShippingOriginEntity", "Company", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Company, 35, 0, 0);
			this.AddElementFieldInfo("ShippingOriginEntity", "Street1", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Street1, 60, 0, 0);
			this.AddElementFieldInfo("ShippingOriginEntity", "Street2", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Street2, 60, 0, 0);
			this.AddElementFieldInfo("ShippingOriginEntity", "Street3", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Street3, 60, 0, 0);
			this.AddElementFieldInfo("ShippingOriginEntity", "City", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.City, 50, 0, 0);
			this.AddElementFieldInfo("ShippingOriginEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.StateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("ShippingOriginEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.PostalCode, 20, 0, 0);
			this.AddElementFieldInfo("ShippingOriginEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.CountryCode, 50, 0, 0);
			this.AddElementFieldInfo("ShippingOriginEntity", "Phone", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Phone, 25, 0, 0);
			this.AddElementFieldInfo("ShippingOriginEntity", "Fax", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Fax, 35, 0, 0);
			this.AddElementFieldInfo("ShippingOriginEntity", "Email", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Email, 100, 0, 0);
			this.AddElementFieldInfo("ShippingOriginEntity", "Website", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Website, 50, 0, 0);
		}
		/// <summary>Inits ShippingPrintOutputEntity's FieldInfo objects</summary>
		private void InitShippingPrintOutputEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ShippingPrintOutputFieldIndex), "ShippingPrintOutputEntity");
			this.AddElementFieldInfo("ShippingPrintOutputEntity", "ShippingPrintOutputID", typeof(System.Int64), true, false, true, false,  (int)ShippingPrintOutputFieldIndex.ShippingPrintOutputID, 0, 0, 19);
			this.AddElementFieldInfo("ShippingPrintOutputEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ShippingPrintOutputFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("ShippingPrintOutputEntity", "ShipmentType", typeof(System.Int32), false, false, false, false,  (int)ShippingPrintOutputFieldIndex.ShipmentType, 0, 0, 10);
			this.AddElementFieldInfo("ShippingPrintOutputEntity", "Name", typeof(System.String), false, false, false, false,  (int)ShippingPrintOutputFieldIndex.Name, 50, 0, 0);
		}
		/// <summary>Inits ShippingPrintOutputRuleEntity's FieldInfo objects</summary>
		private void InitShippingPrintOutputRuleEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ShippingPrintOutputRuleFieldIndex), "ShippingPrintOutputRuleEntity");
			this.AddElementFieldInfo("ShippingPrintOutputRuleEntity", "ShippingPrintOutputRuleID", typeof(System.Int64), true, false, true, false,  (int)ShippingPrintOutputRuleFieldIndex.ShippingPrintOutputRuleID, 0, 0, 19);
			this.AddElementFieldInfo("ShippingPrintOutputRuleEntity", "ShippingPrintOutputID", typeof(System.Int64), false, true, false, false,  (int)ShippingPrintOutputRuleFieldIndex.ShippingPrintOutputID, 0, 0, 19);
			this.AddElementFieldInfo("ShippingPrintOutputRuleEntity", "FilterNodeID", typeof(System.Int64), false, false, false, false,  (int)ShippingPrintOutputRuleFieldIndex.FilterNodeID, 0, 0, 19);
			this.AddElementFieldInfo("ShippingPrintOutputRuleEntity", "TemplateID", typeof(System.Int64), false, false, false, false,  (int)ShippingPrintOutputRuleFieldIndex.TemplateID, 0, 0, 19);
		}
		/// <summary>Inits ShippingProfileEntity's FieldInfo objects</summary>
		private void InitShippingProfileEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ShippingProfileFieldIndex), "ShippingProfileEntity");
			this.AddElementFieldInfo("ShippingProfileEntity", "ShippingProfileID", typeof(System.Int64), true, false, true, false,  (int)ShippingProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			this.AddElementFieldInfo("ShippingProfileEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ShippingProfileFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("ShippingProfileEntity", "Name", typeof(System.String), false, false, false, false,  (int)ShippingProfileFieldIndex.Name, 50, 0, 0);
			this.AddElementFieldInfo("ShippingProfileEntity", "ShipmentType", typeof(System.Int32), false, false, false, false,  (int)ShippingProfileFieldIndex.ShipmentType, 0, 0, 10);
			this.AddElementFieldInfo("ShippingProfileEntity", "ShipmentTypePrimary", typeof(System.Boolean), false, false, false, false,  (int)ShippingProfileFieldIndex.ShipmentTypePrimary, 0, 0, 0);
			this.AddElementFieldInfo("ShippingProfileEntity", "OriginID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)ShippingProfileFieldIndex.OriginID, 0, 0, 19);
			this.AddElementFieldInfo("ShippingProfileEntity", "Insurance", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)ShippingProfileFieldIndex.Insurance, 0, 0, 0);
			this.AddElementFieldInfo("ShippingProfileEntity", "InsuranceInitialValueSource", typeof(Nullable<System.Int32>), false, false, false, true,  (int)ShippingProfileFieldIndex.InsuranceInitialValueSource, 0, 0, 10);
			this.AddElementFieldInfo("ShippingProfileEntity", "InsuranceInitialValueAmount", typeof(Nullable<System.Decimal>), false, false, false, true,  (int)ShippingProfileFieldIndex.InsuranceInitialValueAmount, 0, 4, 19);
			this.AddElementFieldInfo("ShippingProfileEntity", "ReturnShipment", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)ShippingProfileFieldIndex.ReturnShipment, 0, 0, 0);
			this.AddElementFieldInfo("ShippingProfileEntity", "RequestedLabelFormat", typeof(Nullable<System.Int32>), false, false, false, true,  (int)ShippingProfileFieldIndex.RequestedLabelFormat, 0, 0, 10);
		}
		/// <summary>Inits ShippingProviderRuleEntity's FieldInfo objects</summary>
		private void InitShippingProviderRuleEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ShippingProviderRuleFieldIndex), "ShippingProviderRuleEntity");
			this.AddElementFieldInfo("ShippingProviderRuleEntity", "ShippingProviderRuleID", typeof(System.Int64), true, false, true, false,  (int)ShippingProviderRuleFieldIndex.ShippingProviderRuleID, 0, 0, 19);
			this.AddElementFieldInfo("ShippingProviderRuleEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ShippingProviderRuleFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("ShippingProviderRuleEntity", "FilterNodeID", typeof(System.Int64), false, false, false, false,  (int)ShippingProviderRuleFieldIndex.FilterNodeID, 0, 0, 19);
			this.AddElementFieldInfo("ShippingProviderRuleEntity", "ShipmentType", typeof(System.Int32), false, false, false, false,  (int)ShippingProviderRuleFieldIndex.ShipmentType, 0, 0, 10);
		}
		/// <summary>Inits ShippingSettingsEntity's FieldInfo objects</summary>
		private void InitShippingSettingsEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ShippingSettingsFieldIndex), "ShippingSettingsEntity");
			this.AddElementFieldInfo("ShippingSettingsEntity", "ShippingSettingsID", typeof(System.Boolean), true, false, false, false,  (int)ShippingSettingsFieldIndex.ShippingSettingsID, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "InternalActivated", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.InternalActivated, 45, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "InternalConfigured", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.InternalConfigured, 45, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "InternalExcluded", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.InternalExcluded, 45, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "DefaultType", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.DefaultType, 0, 0, 10);
			this.AddElementFieldInfo("ShippingSettingsEntity", "BlankPhoneOption", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.BlankPhoneOption, 0, 0, 10);
			this.AddElementFieldInfo("ShippingSettingsEntity", "BlankPhoneNumber", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.BlankPhoneNumber, 16, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "InsurancePolicy", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.InsurancePolicy, 40, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "InsuranceLastAgreed", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ShippingSettingsFieldIndex.InsuranceLastAgreed, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "FedExUsername", typeof(System.String), false, false, false, true,  (int)ShippingSettingsFieldIndex.FedExUsername, 50, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "FedExPassword", typeof(System.String), false, false, false, true,  (int)ShippingSettingsFieldIndex.FedExPassword, 50, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "FedExMaskAccount", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.FedExMaskAccount, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "FedExThermalDocTab", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.FedExThermalDocTab, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "FedExThermalDocTabType", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.FedExThermalDocTabType, 0, 0, 10);
			this.AddElementFieldInfo("ShippingSettingsEntity", "FedExInsuranceProvider", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.FedExInsuranceProvider, 0, 0, 10);
			this.AddElementFieldInfo("ShippingSettingsEntity", "FedExInsurancePennyOne", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.FedExInsurancePennyOne, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "UpsAccessKey", typeof(System.String), false, false, false, true,  (int)ShippingSettingsFieldIndex.UpsAccessKey, 50, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "UpsInsuranceProvider", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.UpsInsuranceProvider, 0, 0, 10);
			this.AddElementFieldInfo("ShippingSettingsEntity", "UpsInsurancePennyOne", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.UpsInsurancePennyOne, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "EndiciaCustomsCertify", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.EndiciaCustomsCertify, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "EndiciaCustomsSigner", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.EndiciaCustomsSigner, 100, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "EndiciaThermalDocTab", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.EndiciaThermalDocTab, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "EndiciaThermalDocTabType", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.EndiciaThermalDocTabType, 0, 0, 10);
			this.AddElementFieldInfo("ShippingSettingsEntity", "EndiciaAutomaticExpress1", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.EndiciaAutomaticExpress1, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "EndiciaAutomaticExpress1Account", typeof(System.Int64), false, false, false, false,  (int)ShippingSettingsFieldIndex.EndiciaAutomaticExpress1Account, 0, 0, 19);
			this.AddElementFieldInfo("ShippingSettingsEntity", "EndiciaInsuranceProvider", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.EndiciaInsuranceProvider, 0, 0, 10);
			this.AddElementFieldInfo("ShippingSettingsEntity", "WorldShipLaunch", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.WorldShipLaunch, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "UspsAutomaticExpress1", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.UspsAutomaticExpress1, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "UspsAutomaticExpress1Account", typeof(System.Int64), false, false, false, false,  (int)ShippingSettingsFieldIndex.UspsAutomaticExpress1Account, 0, 0, 19);
			this.AddElementFieldInfo("ShippingSettingsEntity", "UspsInsuranceProvider", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.UspsInsuranceProvider, 0, 0, 10);
			this.AddElementFieldInfo("ShippingSettingsEntity", "Express1EndiciaCustomsCertify", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.Express1EndiciaCustomsCertify, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "Express1EndiciaCustomsSigner", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.Express1EndiciaCustomsSigner, 100, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "Express1EndiciaThermalDocTab", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.Express1EndiciaThermalDocTab, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "Express1EndiciaThermalDocTabType", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.Express1EndiciaThermalDocTabType, 0, 0, 10);
			this.AddElementFieldInfo("ShippingSettingsEntity", "Express1EndiciaSingleSource", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.Express1EndiciaSingleSource, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "OnTracInsuranceProvider", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.OnTracInsuranceProvider, 0, 0, 10);
			this.AddElementFieldInfo("ShippingSettingsEntity", "OnTracInsurancePennyOne", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.OnTracInsurancePennyOne, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "IParcelInsuranceProvider", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.IParcelInsuranceProvider, 0, 0, 10);
			this.AddElementFieldInfo("ShippingSettingsEntity", "IParcelInsurancePennyOne", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.IParcelInsurancePennyOne, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "Express1UspsSingleSource", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.Express1UspsSingleSource, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "UpsMailInnovationsEnabled", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.UpsMailInnovationsEnabled, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "WorldShipMailInnovationsEnabled", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.WorldShipMailInnovationsEnabled, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "InternalBestRateExcludedShipmentTypes", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.InternalBestRateExcludedShipmentTypes, 30, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "ShipSenseEnabled", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.ShipSenseEnabled, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "ShipSenseUniquenessXml", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.ShipSenseUniquenessXml, 2147483647, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "ShipSenseProcessedShipmentID", typeof(System.Int64), false, false, false, false,  (int)ShippingSettingsFieldIndex.ShipSenseProcessedShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("ShippingSettingsEntity", "ShipSenseEndShipmentID", typeof(System.Int64), false, false, false, false,  (int)ShippingSettingsFieldIndex.ShipSenseEndShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("ShippingSettingsEntity", "AutoCreateShipments", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.AutoCreateShipments, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "FedExFimsEnabled", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.FedExFimsEnabled, 0, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "FedExFimsUsername", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.FedExFimsUsername, 50, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "FedExFimsPassword", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.FedExFimsPassword, 50, 0, 0);
			this.AddElementFieldInfo("ShippingSettingsEntity", "ShipmentEditLimit", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.ShipmentEditLimit, 0, 0, 10);
		}
		/// <summary>Inits ShipSenseKnowledgebaseEntity's FieldInfo objects</summary>
		private void InitShipSenseKnowledgebaseEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ShipSenseKnowledgebaseFieldIndex), "ShipSenseKnowledgebaseEntity");
			this.AddElementFieldInfo("ShipSenseKnowledgebaseEntity", "Hash", typeof(System.String), true, false, false, false,  (int)ShipSenseKnowledgebaseFieldIndex.Hash, 64, 0, 0);
			this.AddElementFieldInfo("ShipSenseKnowledgebaseEntity", "Entry", typeof(System.Byte[]), false, false, false, false,  (int)ShipSenseKnowledgebaseFieldIndex.Entry, 2147483647, 0, 0);
		}
		/// <summary>Inits ShopifyOrderEntity's FieldInfo objects</summary>
		private void InitShopifyOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ShopifyOrderFieldIndex), "ShopifyOrderEntity");
			this.AddElementFieldInfo("ShopifyOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)ShopifyOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("ShopifyOrderEntity", "ShopifyOrderID", typeof(System.Int64), false, false, false, false,  (int)ShopifyOrderFieldIndex.ShopifyOrderID, 0, 0, 19);
			this.AddElementFieldInfo("ShopifyOrderEntity", "FulfillmentStatusCode", typeof(System.Int32), false, false, false, false,  (int)ShopifyOrderFieldIndex.FulfillmentStatusCode, 0, 0, 10);
			this.AddElementFieldInfo("ShopifyOrderEntity", "PaymentStatusCode", typeof(System.Int32), false, false, false, false,  (int)ShopifyOrderFieldIndex.PaymentStatusCode, 0, 0, 10);
		}
		/// <summary>Inits ShopifyOrderItemEntity's FieldInfo objects</summary>
		private void InitShopifyOrderItemEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ShopifyOrderItemFieldIndex), "ShopifyOrderItemEntity");
			this.AddElementFieldInfo("ShopifyOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, true, false,  (int)ShopifyOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("ShopifyOrderItemEntity", "ShopifyOrderItemID", typeof(System.Int64), false, false, false, false,  (int)ShopifyOrderItemFieldIndex.ShopifyOrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("ShopifyOrderItemEntity", "ShopifyProductID", typeof(System.Int64), false, false, false, false,  (int)ShopifyOrderItemFieldIndex.ShopifyProductID, 0, 0, 19);
		}
		/// <summary>Inits ShopifyStoreEntity's FieldInfo objects</summary>
		private void InitShopifyStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ShopifyStoreFieldIndex), "ShopifyStoreEntity");
			this.AddElementFieldInfo("ShopifyStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)ShopifyStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("ShopifyStoreEntity", "ShopifyShopUrlName", typeof(System.String), false, false, false, false,  (int)ShopifyStoreFieldIndex.ShopifyShopUrlName, 100, 0, 0);
			this.AddElementFieldInfo("ShopifyStoreEntity", "ShopifyShopDisplayName", typeof(System.String), false, false, false, false,  (int)ShopifyStoreFieldIndex.ShopifyShopDisplayName, 100, 0, 0);
			this.AddElementFieldInfo("ShopifyStoreEntity", "ShopifyAccessToken", typeof(System.String), false, false, false, false,  (int)ShopifyStoreFieldIndex.ShopifyAccessToken, 255, 0, 0);
			this.AddElementFieldInfo("ShopifyStoreEntity", "ShopifyRequestedShippingOption", typeof(System.Int32), false, false, false, false,  (int)ShopifyStoreFieldIndex.ShopifyRequestedShippingOption, 0, 0, 10);
		}
		/// <summary>Inits ShopSiteStoreEntity's FieldInfo objects</summary>
		private void InitShopSiteStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ShopSiteStoreFieldIndex), "ShopSiteStoreEntity");
			this.AddElementFieldInfo("ShopSiteStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)ShopSiteStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("ShopSiteStoreEntity", "Username", typeof(System.String), false, false, false, false,  (int)ShopSiteStoreFieldIndex.Username, 50, 0, 0);
			this.AddElementFieldInfo("ShopSiteStoreEntity", "Password", typeof(System.String), false, false, false, false,  (int)ShopSiteStoreFieldIndex.Password, 50, 0, 0);
			this.AddElementFieldInfo("ShopSiteStoreEntity", "CgiUrl", typeof(System.String), false, false, false, false,  (int)ShopSiteStoreFieldIndex.CgiUrl, 350, 0, 0);
			this.AddElementFieldInfo("ShopSiteStoreEntity", "RequireSSL", typeof(System.Boolean), false, false, false, false,  (int)ShopSiteStoreFieldIndex.RequireSSL, 0, 0, 0);
			this.AddElementFieldInfo("ShopSiteStoreEntity", "DownloadPageSize", typeof(System.Int32), false, false, false, false,  (int)ShopSiteStoreFieldIndex.DownloadPageSize, 0, 0, 10);
			this.AddElementFieldInfo("ShopSiteStoreEntity", "RequestTimeout", typeof(System.Int32), false, false, false, false,  (int)ShopSiteStoreFieldIndex.RequestTimeout, 0, 0, 10);
		}
		/// <summary>Inits SparkPayStoreEntity's FieldInfo objects</summary>
		private void InitSparkPayStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(SparkPayStoreFieldIndex), "SparkPayStoreEntity");
			this.AddElementFieldInfo("SparkPayStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)SparkPayStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("SparkPayStoreEntity", "Token", typeof(System.String), false, false, false, false,  (int)SparkPayStoreFieldIndex.Token, 70, 0, 0);
			this.AddElementFieldInfo("SparkPayStoreEntity", "StoreUrl", typeof(System.String), false, false, false, false,  (int)SparkPayStoreFieldIndex.StoreUrl, 350, 0, 0);
			this.AddElementFieldInfo("SparkPayStoreEntity", "StatusCodes", typeof(System.String), false, false, false, true,  (int)SparkPayStoreFieldIndex.StatusCodes, 2147483647, 0, 0);
		}
		/// <summary>Inits StatusPresetEntity's FieldInfo objects</summary>
		private void InitStatusPresetEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(StatusPresetFieldIndex), "StatusPresetEntity");
			this.AddElementFieldInfo("StatusPresetEntity", "StatusPresetID", typeof(System.Int64), true, false, true, false,  (int)StatusPresetFieldIndex.StatusPresetID, 0, 0, 19);
			this.AddElementFieldInfo("StatusPresetEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)StatusPresetFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("StatusPresetEntity", "StoreID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)StatusPresetFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("StatusPresetEntity", "StatusTarget", typeof(System.Int32), false, false, false, false,  (int)StatusPresetFieldIndex.StatusTarget, 0, 0, 10);
			this.AddElementFieldInfo("StatusPresetEntity", "StatusText", typeof(System.String), false, false, false, false,  (int)StatusPresetFieldIndex.StatusText, 300, 0, 0);
			this.AddElementFieldInfo("StatusPresetEntity", "IsDefault", typeof(System.Boolean), false, false, false, false,  (int)StatusPresetFieldIndex.IsDefault, 0, 0, 0);
		}
		/// <summary>Inits StoreEntity's FieldInfo objects</summary>
		private void InitStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(StoreFieldIndex), "StoreEntity");
			this.AddElementFieldInfo("StoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)StoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("StoreEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)StoreFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "License", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.License, 150, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "Edition", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Edition, 2147483647, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "TypeCode", typeof(System.Int32), false, false, false, false,  (int)StoreFieldIndex.TypeCode, 0, 0, 10);
			this.AddElementFieldInfo("StoreEntity", "Enabled", typeof(System.Boolean), false, false, false, false,  (int)StoreFieldIndex.Enabled, 0, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "SetupComplete", typeof(System.Boolean), false, false, false, false,  (int)StoreFieldIndex.SetupComplete, 0, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "StoreName", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.StoreName, 75, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "Company", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Company, 60, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "Street1", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Street1, 60, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "Street2", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Street2, 60, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "Street3", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Street3, 60, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "City", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.City, 50, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.StateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.PostalCode, 20, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.CountryCode, 50, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "Phone", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Phone, 25, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "Fax", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Fax, 35, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "Email", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Email, 100, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "Website", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Website, 50, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "AutoDownload", typeof(System.Boolean), false, false, false, false,  (int)StoreFieldIndex.AutoDownload, 0, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "AutoDownloadMinutes", typeof(System.Int32), false, false, false, false,  (int)StoreFieldIndex.AutoDownloadMinutes, 0, 0, 10);
			this.AddElementFieldInfo("StoreEntity", "AutoDownloadOnlyAway", typeof(System.Boolean), false, false, false, false,  (int)StoreFieldIndex.AutoDownloadOnlyAway, 0, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "AddressValidationSetting", typeof(System.Int32), false, false, false, false,  (int)StoreFieldIndex.AddressValidationSetting, 0, 0, 10);
			this.AddElementFieldInfo("StoreEntity", "ComputerDownloadPolicy", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.ComputerDownloadPolicy, 2147483647, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "DefaultEmailAccountID", typeof(System.Int64), false, false, false, false,  (int)StoreFieldIndex.DefaultEmailAccountID, 0, 0, 19);
			this.AddElementFieldInfo("StoreEntity", "ManualOrderPrefix", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.ManualOrderPrefix, 10, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "ManualOrderPostfix", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.ManualOrderPostfix, 10, 0, 0);
			this.AddElementFieldInfo("StoreEntity", "InitialDownloadDays", typeof(Nullable<System.Int32>), false, false, false, true,  (int)StoreFieldIndex.InitialDownloadDays, 0, 0, 10);
			this.AddElementFieldInfo("StoreEntity", "InitialDownloadOrder", typeof(Nullable<System.Int64>), false, false, false, true,  (int)StoreFieldIndex.InitialDownloadOrder, 0, 0, 19);
		}
		/// <summary>Inits SystemDataEntity's FieldInfo objects</summary>
		private void InitSystemDataEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(SystemDataFieldIndex), "SystemDataEntity");
			this.AddElementFieldInfo("SystemDataEntity", "SystemDataID", typeof(System.Boolean), true, false, false, false,  (int)SystemDataFieldIndex.SystemDataID, 0, 0, 0);
			this.AddElementFieldInfo("SystemDataEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)SystemDataFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("SystemDataEntity", "DatabaseID", typeof(System.Guid), false, false, false, false,  (int)SystemDataFieldIndex.DatabaseID, 0, 0, 0);
			this.AddElementFieldInfo("SystemDataEntity", "DateFiltersLastUpdate", typeof(System.DateTime), false, false, false, false,  (int)SystemDataFieldIndex.DateFiltersLastUpdate, 0, 0, 0);
			this.AddElementFieldInfo("SystemDataEntity", "TemplateVersion", typeof(System.String), false, false, false, false,  (int)SystemDataFieldIndex.TemplateVersion, 30, 0, 0);
		}
		/// <summary>Inits TemplateEntity's FieldInfo objects</summary>
		private void InitTemplateEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(TemplateFieldIndex), "TemplateEntity");
			this.AddElementFieldInfo("TemplateEntity", "TemplateID", typeof(System.Int64), true, false, true, false,  (int)TemplateFieldIndex.TemplateID, 0, 0, 19);
			this.AddElementFieldInfo("TemplateEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)TemplateFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("TemplateEntity", "ParentFolderID", typeof(System.Int64), false, true, false, false,  (int)TemplateFieldIndex.ParentFolderID, 0, 0, 19);
			this.AddElementFieldInfo("TemplateEntity", "Name", typeof(System.String), false, false, false, false,  (int)TemplateFieldIndex.Name, 100, 0, 0);
			this.AddElementFieldInfo("TemplateEntity", "Xsl", typeof(System.String), false, false, false, false,  (int)TemplateFieldIndex.Xsl, 2147483647, 0, 0);
			this.AddElementFieldInfo("TemplateEntity", "Type", typeof(System.Int32), false, false, false, false,  (int)TemplateFieldIndex.Type, 0, 0, 10);
			this.AddElementFieldInfo("TemplateEntity", "Context", typeof(System.Int32), false, false, false, false,  (int)TemplateFieldIndex.Context, 0, 0, 10);
			this.AddElementFieldInfo("TemplateEntity", "OutputFormat", typeof(System.Int32), false, false, false, false,  (int)TemplateFieldIndex.OutputFormat, 0, 0, 10);
			this.AddElementFieldInfo("TemplateEntity", "OutputEncoding", typeof(System.String), false, false, false, false,  (int)TemplateFieldIndex.OutputEncoding, 20, 0, 0);
			this.AddElementFieldInfo("TemplateEntity", "PageMarginLeft", typeof(System.Double), false, false, false, false,  (int)TemplateFieldIndex.PageMarginLeft, 0, 0, 38);
			this.AddElementFieldInfo("TemplateEntity", "PageMarginRight", typeof(System.Double), false, false, false, false,  (int)TemplateFieldIndex.PageMarginRight, 0, 0, 38);
			this.AddElementFieldInfo("TemplateEntity", "PageMarginBottom", typeof(System.Double), false, false, false, false,  (int)TemplateFieldIndex.PageMarginBottom, 0, 0, 38);
			this.AddElementFieldInfo("TemplateEntity", "PageMarginTop", typeof(System.Double), false, false, false, false,  (int)TemplateFieldIndex.PageMarginTop, 0, 0, 38);
			this.AddElementFieldInfo("TemplateEntity", "PageWidth", typeof(System.Double), false, false, false, false,  (int)TemplateFieldIndex.PageWidth, 0, 0, 38);
			this.AddElementFieldInfo("TemplateEntity", "PageHeight", typeof(System.Double), false, false, false, false,  (int)TemplateFieldIndex.PageHeight, 0, 0, 38);
			this.AddElementFieldInfo("TemplateEntity", "LabelSheetID", typeof(System.Int64), false, false, false, false,  (int)TemplateFieldIndex.LabelSheetID, 0, 0, 19);
			this.AddElementFieldInfo("TemplateEntity", "PrintCopies", typeof(System.Int32), false, false, false, false,  (int)TemplateFieldIndex.PrintCopies, 0, 0, 10);
			this.AddElementFieldInfo("TemplateEntity", "PrintCollate", typeof(System.Boolean), false, false, false, false,  (int)TemplateFieldIndex.PrintCollate, 0, 0, 0);
			this.AddElementFieldInfo("TemplateEntity", "SaveFileName", typeof(System.String), false, false, false, false,  (int)TemplateFieldIndex.SaveFileName, 500, 0, 0);
			this.AddElementFieldInfo("TemplateEntity", "SaveFileFolder", typeof(System.String), false, false, false, false,  (int)TemplateFieldIndex.SaveFileFolder, 500, 0, 0);
			this.AddElementFieldInfo("TemplateEntity", "SaveFilePrompt", typeof(System.Int32), false, false, false, false,  (int)TemplateFieldIndex.SaveFilePrompt, 0, 0, 10);
			this.AddElementFieldInfo("TemplateEntity", "SaveFileBOM", typeof(System.Boolean), false, false, false, false,  (int)TemplateFieldIndex.SaveFileBOM, 0, 0, 0);
			this.AddElementFieldInfo("TemplateEntity", "SaveFileOnlineResources", typeof(System.Boolean), false, false, false, false,  (int)TemplateFieldIndex.SaveFileOnlineResources, 0, 0, 0);
		}
		/// <summary>Inits TemplateComputerSettingsEntity's FieldInfo objects</summary>
		private void InitTemplateComputerSettingsEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(TemplateComputerSettingsFieldIndex), "TemplateComputerSettingsEntity");
			this.AddElementFieldInfo("TemplateComputerSettingsEntity", "TemplateComputerSettingsID", typeof(System.Int64), true, false, true, false,  (int)TemplateComputerSettingsFieldIndex.TemplateComputerSettingsID, 0, 0, 19);
			this.AddElementFieldInfo("TemplateComputerSettingsEntity", "TemplateID", typeof(System.Int64), false, true, false, false,  (int)TemplateComputerSettingsFieldIndex.TemplateID, 0, 0, 19);
			this.AddElementFieldInfo("TemplateComputerSettingsEntity", "ComputerID", typeof(System.Int64), false, true, false, false,  (int)TemplateComputerSettingsFieldIndex.ComputerID, 0, 0, 19);
			this.AddElementFieldInfo("TemplateComputerSettingsEntity", "PrinterName", typeof(System.String), false, false, false, false,  (int)TemplateComputerSettingsFieldIndex.PrinterName, 350, 0, 0);
			this.AddElementFieldInfo("TemplateComputerSettingsEntity", "PaperSource", typeof(System.Int32), false, false, false, false,  (int)TemplateComputerSettingsFieldIndex.PaperSource, 0, 0, 10);
		}
		/// <summary>Inits TemplateFolderEntity's FieldInfo objects</summary>
		private void InitTemplateFolderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(TemplateFolderFieldIndex), "TemplateFolderEntity");
			this.AddElementFieldInfo("TemplateFolderEntity", "TemplateFolderID", typeof(System.Int64), true, false, true, false,  (int)TemplateFolderFieldIndex.TemplateFolderID, 0, 0, 19);
			this.AddElementFieldInfo("TemplateFolderEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)TemplateFolderFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("TemplateFolderEntity", "ParentFolderID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)TemplateFolderFieldIndex.ParentFolderID, 0, 0, 19);
			this.AddElementFieldInfo("TemplateFolderEntity", "Name", typeof(System.String), false, false, false, false,  (int)TemplateFolderFieldIndex.Name, 100, 0, 0);
		}
		/// <summary>Inits TemplateStoreSettingsEntity's FieldInfo objects</summary>
		private void InitTemplateStoreSettingsEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(TemplateStoreSettingsFieldIndex), "TemplateStoreSettingsEntity");
			this.AddElementFieldInfo("TemplateStoreSettingsEntity", "TemplateStoreSettingsID", typeof(System.Int64), true, false, true, false,  (int)TemplateStoreSettingsFieldIndex.TemplateStoreSettingsID, 0, 0, 19);
			this.AddElementFieldInfo("TemplateStoreSettingsEntity", "TemplateID", typeof(System.Int64), false, true, false, false,  (int)TemplateStoreSettingsFieldIndex.TemplateID, 0, 0, 19);
			this.AddElementFieldInfo("TemplateStoreSettingsEntity", "StoreID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)TemplateStoreSettingsFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("TemplateStoreSettingsEntity", "EmailUseDefault", typeof(System.Boolean), false, false, false, false,  (int)TemplateStoreSettingsFieldIndex.EmailUseDefault, 0, 0, 0);
			this.AddElementFieldInfo("TemplateStoreSettingsEntity", "EmailAccountID", typeof(System.Int64), false, false, false, false,  (int)TemplateStoreSettingsFieldIndex.EmailAccountID, 0, 0, 19);
			this.AddElementFieldInfo("TemplateStoreSettingsEntity", "EmailTo", typeof(System.String), false, false, false, false,  (int)TemplateStoreSettingsFieldIndex.EmailTo, 2147483647, 0, 0);
			this.AddElementFieldInfo("TemplateStoreSettingsEntity", "EmailCc", typeof(System.String), false, false, false, false,  (int)TemplateStoreSettingsFieldIndex.EmailCc, 2147483647, 0, 0);
			this.AddElementFieldInfo("TemplateStoreSettingsEntity", "EmailBcc", typeof(System.String), false, false, false, false,  (int)TemplateStoreSettingsFieldIndex.EmailBcc, 2147483647, 0, 0);
			this.AddElementFieldInfo("TemplateStoreSettingsEntity", "EmailSubject", typeof(System.String), false, false, false, false,  (int)TemplateStoreSettingsFieldIndex.EmailSubject, 500, 0, 0);
		}
		/// <summary>Inits TemplateUserSettingsEntity's FieldInfo objects</summary>
		private void InitTemplateUserSettingsEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(TemplateUserSettingsFieldIndex), "TemplateUserSettingsEntity");
			this.AddElementFieldInfo("TemplateUserSettingsEntity", "TemplateUserSettingsID", typeof(System.Int64), true, false, true, false,  (int)TemplateUserSettingsFieldIndex.TemplateUserSettingsID, 0, 0, 19);
			this.AddElementFieldInfo("TemplateUserSettingsEntity", "TemplateID", typeof(System.Int64), false, true, false, false,  (int)TemplateUserSettingsFieldIndex.TemplateID, 0, 0, 19);
			this.AddElementFieldInfo("TemplateUserSettingsEntity", "UserID", typeof(System.Int64), false, true, false, false,  (int)TemplateUserSettingsFieldIndex.UserID, 0, 0, 19);
			this.AddElementFieldInfo("TemplateUserSettingsEntity", "PreviewSource", typeof(System.Int32), false, false, false, false,  (int)TemplateUserSettingsFieldIndex.PreviewSource, 0, 0, 10);
			this.AddElementFieldInfo("TemplateUserSettingsEntity", "PreviewCount", typeof(System.Int32), false, false, false, false,  (int)TemplateUserSettingsFieldIndex.PreviewCount, 0, 0, 10);
			this.AddElementFieldInfo("TemplateUserSettingsEntity", "PreviewFilterNodeID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)TemplateUserSettingsFieldIndex.PreviewFilterNodeID, 0, 0, 19);
			this.AddElementFieldInfo("TemplateUserSettingsEntity", "PreviewZoom", typeof(System.String), false, false, false, false,  (int)TemplateUserSettingsFieldIndex.PreviewZoom, 10, 0, 0);
		}
		/// <summary>Inits ThreeDCartOrderEntity's FieldInfo objects</summary>
		private void InitThreeDCartOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ThreeDCartOrderFieldIndex), "ThreeDCartOrderEntity");
			this.AddElementFieldInfo("ThreeDCartOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)ThreeDCartOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("ThreeDCartOrderEntity", "ThreeDCartOrderID", typeof(System.Int64), false, false, false, false,  (int)ThreeDCartOrderFieldIndex.ThreeDCartOrderID, 0, 0, 19);
		}
		/// <summary>Inits ThreeDCartOrderItemEntity's FieldInfo objects</summary>
		private void InitThreeDCartOrderItemEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ThreeDCartOrderItemFieldIndex), "ThreeDCartOrderItemEntity");
			this.AddElementFieldInfo("ThreeDCartOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, true, false,  (int)ThreeDCartOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("ThreeDCartOrderItemEntity", "ThreeDCartShipmentID", typeof(System.Int64), false, false, false, false,  (int)ThreeDCartOrderItemFieldIndex.ThreeDCartShipmentID, 0, 0, 19);
		}
		/// <summary>Inits ThreeDCartStoreEntity's FieldInfo objects</summary>
		private void InitThreeDCartStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ThreeDCartStoreFieldIndex), "ThreeDCartStoreEntity");
			this.AddElementFieldInfo("ThreeDCartStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)ThreeDCartStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("ThreeDCartStoreEntity", "StoreUrl", typeof(System.String), false, false, false, false,  (int)ThreeDCartStoreFieldIndex.StoreUrl, 110, 0, 0);
			this.AddElementFieldInfo("ThreeDCartStoreEntity", "ApiUserKey", typeof(System.String), false, false, false, false,  (int)ThreeDCartStoreFieldIndex.ApiUserKey, 65, 0, 0);
			this.AddElementFieldInfo("ThreeDCartStoreEntity", "TimeZoneID", typeof(System.String), false, false, false, true,  (int)ThreeDCartStoreFieldIndex.TimeZoneID, 100, 0, 0);
			this.AddElementFieldInfo("ThreeDCartStoreEntity", "StatusCodes", typeof(System.String), false, false, false, true,  (int)ThreeDCartStoreFieldIndex.StatusCodes, 2147483647, 0, 0);
			this.AddElementFieldInfo("ThreeDCartStoreEntity", "DownloadModifiedNumberOfDaysBack", typeof(System.Int32), false, false, false, false,  (int)ThreeDCartStoreFieldIndex.DownloadModifiedNumberOfDaysBack, 0, 0, 10);
			this.AddElementFieldInfo("ThreeDCartStoreEntity", "RestUser", typeof(System.Boolean), false, false, false, false,  (int)ThreeDCartStoreFieldIndex.RestUser, 0, 0, 0);
		}
		/// <summary>Inits UpsAccountEntity's FieldInfo objects</summary>
		private void InitUpsAccountEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(UpsAccountFieldIndex), "UpsAccountEntity");
			this.AddElementFieldInfo("UpsAccountEntity", "UpsAccountID", typeof(System.Int64), true, false, true, false,  (int)UpsAccountFieldIndex.UpsAccountID, 0, 0, 19);
			this.AddElementFieldInfo("UpsAccountEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)UpsAccountFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "Description", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Description, 50, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "AccountNumber", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.AccountNumber, 10, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "UserID", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.UserID, 25, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "Password", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Password, 25, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "RateType", typeof(System.Int32), false, false, false, false,  (int)UpsAccountFieldIndex.RateType, 0, 0, 10);
			this.AddElementFieldInfo("UpsAccountEntity", "InvoiceAuth", typeof(System.Boolean), false, false, false, false,  (int)UpsAccountFieldIndex.InvoiceAuth, 0, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "FirstName", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.FirstName, 30, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "MiddleName", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.MiddleName, 30, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "LastName", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.LastName, 30, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "Company", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Company, 30, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "Street1", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Street1, 60, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "Street2", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Street2, 60, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "Street3", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Street3, 60, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "City", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.City, 50, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.StateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.PostalCode, 20, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.CountryCode, 50, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "Phone", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Phone, 25, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "Email", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Email, 100, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "Website", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Website, 50, 0, 0);
			this.AddElementFieldInfo("UpsAccountEntity", "PromoStatus", typeof(System.Byte), false, false, false, false,  (int)UpsAccountFieldIndex.PromoStatus, 0, 0, 3);
		}
		/// <summary>Inits UpsPackageEntity's FieldInfo objects</summary>
		private void InitUpsPackageEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(UpsPackageFieldIndex), "UpsPackageEntity");
			this.AddElementFieldInfo("UpsPackageEntity", "UpsPackageID", typeof(System.Int64), true, false, true, false,  (int)UpsPackageFieldIndex.UpsPackageID, 0, 0, 19);
			this.AddElementFieldInfo("UpsPackageEntity", "ShipmentID", typeof(System.Int64), false, true, false, false,  (int)UpsPackageFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("UpsPackageEntity", "PackagingType", typeof(System.Int32), false, false, false, false,  (int)UpsPackageFieldIndex.PackagingType, 0, 0, 10);
			this.AddElementFieldInfo("UpsPackageEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)UpsPackageFieldIndex.Weight, 0, 0, 38);
			this.AddElementFieldInfo("UpsPackageEntity", "DimsProfileID", typeof(System.Int64), false, false, false, false,  (int)UpsPackageFieldIndex.DimsProfileID, 0, 0, 19);
			this.AddElementFieldInfo("UpsPackageEntity", "DimsLength", typeof(System.Double), false, false, false, false,  (int)UpsPackageFieldIndex.DimsLength, 0, 0, 38);
			this.AddElementFieldInfo("UpsPackageEntity", "DimsWidth", typeof(System.Double), false, false, false, false,  (int)UpsPackageFieldIndex.DimsWidth, 0, 0, 38);
			this.AddElementFieldInfo("UpsPackageEntity", "DimsHeight", typeof(System.Double), false, false, false, false,  (int)UpsPackageFieldIndex.DimsHeight, 0, 0, 38);
			this.AddElementFieldInfo("UpsPackageEntity", "DimsWeight", typeof(System.Double), false, false, false, false,  (int)UpsPackageFieldIndex.DimsWeight, 0, 0, 38);
			this.AddElementFieldInfo("UpsPackageEntity", "DimsAddWeight", typeof(System.Boolean), false, false, false, false,  (int)UpsPackageFieldIndex.DimsAddWeight, 0, 0, 0);
			this.AddElementFieldInfo("UpsPackageEntity", "Insurance", typeof(System.Boolean), false, false, false, false,  (int)UpsPackageFieldIndex.Insurance, 0, 0, 0);
			this.AddElementFieldInfo("UpsPackageEntity", "InsuranceValue", typeof(System.Decimal), false, false, false, false,  (int)UpsPackageFieldIndex.InsuranceValue, 0, 4, 19);
			this.AddElementFieldInfo("UpsPackageEntity", "InsurancePennyOne", typeof(System.Boolean), false, false, false, false,  (int)UpsPackageFieldIndex.InsurancePennyOne, 0, 0, 0);
			this.AddElementFieldInfo("UpsPackageEntity", "DeclaredValue", typeof(System.Decimal), false, false, false, false,  (int)UpsPackageFieldIndex.DeclaredValue, 0, 4, 19);
			this.AddElementFieldInfo("UpsPackageEntity", "TrackingNumber", typeof(System.String), false, false, false, false,  (int)UpsPackageFieldIndex.TrackingNumber, 50, 0, 0);
			this.AddElementFieldInfo("UpsPackageEntity", "UspsTrackingNumber", typeof(System.String), false, false, false, false,  (int)UpsPackageFieldIndex.UspsTrackingNumber, 50, 0, 0);
			this.AddElementFieldInfo("UpsPackageEntity", "AdditionalHandlingEnabled", typeof(System.Boolean), false, false, false, false,  (int)UpsPackageFieldIndex.AdditionalHandlingEnabled, 0, 0, 0);
			this.AddElementFieldInfo("UpsPackageEntity", "VerbalConfirmationEnabled", typeof(System.Boolean), false, false, false, false,  (int)UpsPackageFieldIndex.VerbalConfirmationEnabled, 0, 0, 0);
			this.AddElementFieldInfo("UpsPackageEntity", "VerbalConfirmationName", typeof(System.String), false, false, false, false,  (int)UpsPackageFieldIndex.VerbalConfirmationName, 35, 0, 0);
			this.AddElementFieldInfo("UpsPackageEntity", "VerbalConfirmationPhone", typeof(System.String), false, false, false, false,  (int)UpsPackageFieldIndex.VerbalConfirmationPhone, 15, 0, 0);
			this.AddElementFieldInfo("UpsPackageEntity", "VerbalConfirmationPhoneExtension", typeof(System.String), false, false, false, false,  (int)UpsPackageFieldIndex.VerbalConfirmationPhoneExtension, 4, 0, 0);
			this.AddElementFieldInfo("UpsPackageEntity", "DryIceEnabled", typeof(System.Boolean), false, false, false, false,  (int)UpsPackageFieldIndex.DryIceEnabled, 0, 0, 0);
			this.AddElementFieldInfo("UpsPackageEntity", "DryIceRegulationSet", typeof(System.Int32), false, false, false, false,  (int)UpsPackageFieldIndex.DryIceRegulationSet, 0, 0, 10);
			this.AddElementFieldInfo("UpsPackageEntity", "DryIceWeight", typeof(System.Double), false, false, false, false,  (int)UpsPackageFieldIndex.DryIceWeight, 0, 0, 38);
			this.AddElementFieldInfo("UpsPackageEntity", "DryIceIsForMedicalUse", typeof(System.Boolean), false, false, false, false,  (int)UpsPackageFieldIndex.DryIceIsForMedicalUse, 0, 0, 0);
		}
		/// <summary>Inits UpsProfileEntity's FieldInfo objects</summary>
		private void InitUpsProfileEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(UpsProfileFieldIndex), "UpsProfileEntity");
			this.AddElementFieldInfo("UpsProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)UpsProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			this.AddElementFieldInfo("UpsProfileEntity", "UpsAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)UpsProfileFieldIndex.UpsAccountID, 0, 0, 19);
			this.AddElementFieldInfo("UpsProfileEntity", "Service", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.Service, 0, 0, 10);
			this.AddElementFieldInfo("UpsProfileEntity", "SaturdayDelivery", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfileFieldIndex.SaturdayDelivery, 0, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "ResidentialDetermination", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.ResidentialDetermination, 0, 0, 10);
			this.AddElementFieldInfo("UpsProfileEntity", "DeliveryConfirmation", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.DeliveryConfirmation, 0, 0, 10);
			this.AddElementFieldInfo("UpsProfileEntity", "ReferenceNumber", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.ReferenceNumber, 300, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "ReferenceNumber2", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.ReferenceNumber2, 300, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "PayorType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.PayorType, 0, 0, 10);
			this.AddElementFieldInfo("UpsProfileEntity", "PayorAccount", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.PayorAccount, 10, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "PayorPostalCode", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.PayorPostalCode, 20, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "PayorCountryCode", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.PayorCountryCode, 50, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "EmailNotifySender", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.EmailNotifySender, 0, 0, 10);
			this.AddElementFieldInfo("UpsProfileEntity", "EmailNotifyRecipient", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.EmailNotifyRecipient, 0, 0, 10);
			this.AddElementFieldInfo("UpsProfileEntity", "EmailNotifyOther", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.EmailNotifyOther, 0, 0, 10);
			this.AddElementFieldInfo("UpsProfileEntity", "EmailNotifyOtherAddress", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.EmailNotifyOtherAddress, 100, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "EmailNotifyFrom", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.EmailNotifyFrom, 100, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "EmailNotifySubject", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.EmailNotifySubject, 0, 0, 10);
			this.AddElementFieldInfo("UpsProfileEntity", "EmailNotifyMessage", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.EmailNotifyMessage, 120, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "ReturnService", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.ReturnService, 0, 0, 10);
			this.AddElementFieldInfo("UpsProfileEntity", "ReturnUndeliverableEmail", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.ReturnUndeliverableEmail, 100, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "ReturnContents", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.ReturnContents, 300, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "Endorsement", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.Endorsement, 0, 0, 10);
			this.AddElementFieldInfo("UpsProfileEntity", "Subclassification", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.Subclassification, 0, 0, 10);
			this.AddElementFieldInfo("UpsProfileEntity", "PaperlessAdditionalDocumentation", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfileFieldIndex.PaperlessAdditionalDocumentation, 0, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "ShipperRelease", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfileFieldIndex.ShipperRelease, 0, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "CarbonNeutral", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfileFieldIndex.CarbonNeutral, 0, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "CommercialPaperlessInvoice", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfileFieldIndex.CommercialPaperlessInvoice, 0, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "CostCenter", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.CostCenter, 100, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "IrregularIndicator", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.IrregularIndicator, 0, 0, 10);
			this.AddElementFieldInfo("UpsProfileEntity", "Cn22Number", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.Cn22Number, 255, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "ShipmentChargeType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.ShipmentChargeType, 0, 0, 10);
			this.AddElementFieldInfo("UpsProfileEntity", "ShipmentChargeAccount", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.ShipmentChargeAccount, 10, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "ShipmentChargePostalCode", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.ShipmentChargePostalCode, 20, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "ShipmentChargeCountryCode", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.ShipmentChargeCountryCode, 50, 0, 0);
			this.AddElementFieldInfo("UpsProfileEntity", "UspsPackageID", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.UspsPackageID, 100, 0, 0);
		}
		/// <summary>Inits UpsProfilePackageEntity's FieldInfo objects</summary>
		private void InitUpsProfilePackageEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(UpsProfilePackageFieldIndex), "UpsProfilePackageEntity");
			this.AddElementFieldInfo("UpsProfilePackageEntity", "UpsProfilePackageID", typeof(System.Int64), true, false, true, false,  (int)UpsProfilePackageFieldIndex.UpsProfilePackageID, 0, 0, 19);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "ShippingProfileID", typeof(System.Int64), false, true, false, false,  (int)UpsProfilePackageFieldIndex.ShippingProfileID, 0, 0, 19);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "PackagingType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.PackagingType, 0, 0, 10);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "Weight", typeof(Nullable<System.Double>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.Weight, 0, 0, 38);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "DimsProfileID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DimsProfileID, 0, 0, 19);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "DimsLength", typeof(Nullable<System.Double>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DimsLength, 0, 0, 38);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "DimsWidth", typeof(Nullable<System.Double>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DimsWidth, 0, 0, 38);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "DimsHeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DimsHeight, 0, 0, 38);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "DimsWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DimsWeight, 0, 0, 38);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "DimsAddWeight", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DimsAddWeight, 0, 0, 0);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "AdditionalHandlingEnabled", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.AdditionalHandlingEnabled, 0, 0, 0);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "VerbalConfirmationEnabled", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.VerbalConfirmationEnabled, 0, 0, 0);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "VerbalConfirmationName", typeof(System.String), false, false, false, true,  (int)UpsProfilePackageFieldIndex.VerbalConfirmationName, 35, 0, 0);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "VerbalConfirmationPhone", typeof(System.String), false, false, false, true,  (int)UpsProfilePackageFieldIndex.VerbalConfirmationPhone, 15, 0, 0);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "VerbalConfirmationPhoneExtension", typeof(System.String), false, false, false, true,  (int)UpsProfilePackageFieldIndex.VerbalConfirmationPhoneExtension, 4, 0, 0);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "DryIceEnabled", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DryIceEnabled, 0, 0, 0);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "DryIceRegulationSet", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DryIceRegulationSet, 0, 0, 10);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "DryIceWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DryIceWeight, 0, 0, 38);
			this.AddElementFieldInfo("UpsProfilePackageEntity", "DryIceIsForMedicalUse", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DryIceIsForMedicalUse, 0, 0, 0);
		}
		/// <summary>Inits UpsShipmentEntity's FieldInfo objects</summary>
		private void InitUpsShipmentEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(UpsShipmentFieldIndex), "UpsShipmentEntity");
			this.AddElementFieldInfo("UpsShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)UpsShipmentFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("UpsShipmentEntity", "UpsAccountID", typeof(System.Int64), false, false, false, false,  (int)UpsShipmentFieldIndex.UpsAccountID, 0, 0, 19);
			this.AddElementFieldInfo("UpsShipmentEntity", "Service", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.Service, 0, 0, 10);
			this.AddElementFieldInfo("UpsShipmentEntity", "SaturdayDelivery", typeof(System.Boolean), false, false, false, false,  (int)UpsShipmentFieldIndex.SaturdayDelivery, 0, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "CodEnabled", typeof(System.Boolean), false, false, false, false,  (int)UpsShipmentFieldIndex.CodEnabled, 0, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "CodAmount", typeof(System.Decimal), false, false, false, false,  (int)UpsShipmentFieldIndex.CodAmount, 0, 4, 19);
			this.AddElementFieldInfo("UpsShipmentEntity", "CodPaymentType", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.CodPaymentType, 0, 0, 10);
			this.AddElementFieldInfo("UpsShipmentEntity", "DeliveryConfirmation", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.DeliveryConfirmation, 0, 0, 10);
			this.AddElementFieldInfo("UpsShipmentEntity", "ReferenceNumber", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.ReferenceNumber, 300, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "ReferenceNumber2", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.ReferenceNumber2, 300, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "PayorType", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.PayorType, 0, 0, 10);
			this.AddElementFieldInfo("UpsShipmentEntity", "PayorAccount", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.PayorAccount, 10, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "PayorPostalCode", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.PayorPostalCode, 20, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "PayorCountryCode", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.PayorCountryCode, 50, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "EmailNotifySender", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.EmailNotifySender, 0, 0, 10);
			this.AddElementFieldInfo("UpsShipmentEntity", "EmailNotifyRecipient", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.EmailNotifyRecipient, 0, 0, 10);
			this.AddElementFieldInfo("UpsShipmentEntity", "EmailNotifyOther", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.EmailNotifyOther, 0, 0, 10);
			this.AddElementFieldInfo("UpsShipmentEntity", "EmailNotifyOtherAddress", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.EmailNotifyOtherAddress, 100, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "EmailNotifyFrom", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.EmailNotifyFrom, 100, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "EmailNotifySubject", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.EmailNotifySubject, 0, 0, 10);
			this.AddElementFieldInfo("UpsShipmentEntity", "EmailNotifyMessage", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.EmailNotifyMessage, 120, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "CustomsDocumentsOnly", typeof(System.Boolean), false, false, false, false,  (int)UpsShipmentFieldIndex.CustomsDocumentsOnly, 0, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "CustomsDescription", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.CustomsDescription, 150, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "CommercialPaperlessInvoice", typeof(System.Boolean), false, false, false, false,  (int)UpsShipmentFieldIndex.CommercialPaperlessInvoice, 0, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "CommercialInvoiceTermsOfSale", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.CommercialInvoiceTermsOfSale, 0, 0, 10);
			this.AddElementFieldInfo("UpsShipmentEntity", "CommercialInvoicePurpose", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.CommercialInvoicePurpose, 0, 0, 10);
			this.AddElementFieldInfo("UpsShipmentEntity", "CommercialInvoiceComments", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.CommercialInvoiceComments, 200, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "CommercialInvoiceFreight", typeof(System.Decimal), false, false, false, false,  (int)UpsShipmentFieldIndex.CommercialInvoiceFreight, 0, 4, 19);
			this.AddElementFieldInfo("UpsShipmentEntity", "CommercialInvoiceInsurance", typeof(System.Decimal), false, false, false, false,  (int)UpsShipmentFieldIndex.CommercialInvoiceInsurance, 0, 4, 19);
			this.AddElementFieldInfo("UpsShipmentEntity", "CommercialInvoiceOther", typeof(System.Decimal), false, false, false, false,  (int)UpsShipmentFieldIndex.CommercialInvoiceOther, 0, 4, 19);
			this.AddElementFieldInfo("UpsShipmentEntity", "WorldShipStatus", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.WorldShipStatus, 0, 0, 10);
			this.AddElementFieldInfo("UpsShipmentEntity", "PublishedCharges", typeof(System.Decimal), false, false, false, false,  (int)UpsShipmentFieldIndex.PublishedCharges, 0, 4, 19);
			this.AddElementFieldInfo("UpsShipmentEntity", "NegotiatedRate", typeof(System.Boolean), false, false, false, false,  (int)UpsShipmentFieldIndex.NegotiatedRate, 0, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "ReturnService", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.ReturnService, 0, 0, 10);
			this.AddElementFieldInfo("UpsShipmentEntity", "ReturnUndeliverableEmail", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.ReturnUndeliverableEmail, 100, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "ReturnContents", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.ReturnContents, 300, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "UspsTrackingNumber", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.UspsTrackingNumber, 50, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "Endorsement", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.Endorsement, 0, 0, 10);
			this.AddElementFieldInfo("UpsShipmentEntity", "Subclassification", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.Subclassification, 0, 0, 10);
			this.AddElementFieldInfo("UpsShipmentEntity", "PaperlessAdditionalDocumentation", typeof(System.Boolean), false, false, false, false,  (int)UpsShipmentFieldIndex.PaperlessAdditionalDocumentation, 0, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "ShipperRelease", typeof(System.Boolean), false, false, false, false,  (int)UpsShipmentFieldIndex.ShipperRelease, 0, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "CarbonNeutral", typeof(System.Boolean), false, false, false, false,  (int)UpsShipmentFieldIndex.CarbonNeutral, 0, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "CostCenter", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.CostCenter, 100, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "IrregularIndicator", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.IrregularIndicator, 0, 0, 10);
			this.AddElementFieldInfo("UpsShipmentEntity", "Cn22Number", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.Cn22Number, 255, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "ShipmentChargeType", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.ShipmentChargeType, 0, 0, 10);
			this.AddElementFieldInfo("UpsShipmentEntity", "ShipmentChargeAccount", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.ShipmentChargeAccount, 10, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "ShipmentChargePostalCode", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.ShipmentChargePostalCode, 20, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "ShipmentChargeCountryCode", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.ShipmentChargeCountryCode, 50, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "UspsPackageID", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.UspsPackageID, 100, 0, 0);
			this.AddElementFieldInfo("UpsShipmentEntity", "RequestedLabelFormat", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.RequestedLabelFormat, 0, 0, 10);
		}
		/// <summary>Inits UserEntity's FieldInfo objects</summary>
		private void InitUserEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(UserFieldIndex), "UserEntity");
			this.AddElementFieldInfo("UserEntity", "UserID", typeof(System.Int64), true, false, true, false,  (int)UserFieldIndex.UserID, 0, 0, 19);
			this.AddElementFieldInfo("UserEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)UserFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("UserEntity", "Username", typeof(System.String), false, false, false, false,  (int)UserFieldIndex.Username, 30, 0, 0);
			this.AddElementFieldInfo("UserEntity", "Password", typeof(System.String), false, false, false, false,  (int)UserFieldIndex.Password, 32, 0, 0);
			this.AddElementFieldInfo("UserEntity", "Email", typeof(System.String), false, false, false, false,  (int)UserFieldIndex.Email, 255, 0, 0);
			this.AddElementFieldInfo("UserEntity", "IsAdmin", typeof(System.Boolean), false, false, false, false,  (int)UserFieldIndex.IsAdmin, 0, 0, 0);
			this.AddElementFieldInfo("UserEntity", "IsDeleted", typeof(System.Boolean), false, false, false, false,  (int)UserFieldIndex.IsDeleted, 0, 0, 0);
		}
		/// <summary>Inits UserColumnSettingsEntity's FieldInfo objects</summary>
		private void InitUserColumnSettingsEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(UserColumnSettingsFieldIndex), "UserColumnSettingsEntity");
			this.AddElementFieldInfo("UserColumnSettingsEntity", "UserColumnSettingsID", typeof(System.Int64), true, false, true, false,  (int)UserColumnSettingsFieldIndex.UserColumnSettingsID, 0, 0, 19);
			this.AddElementFieldInfo("UserColumnSettingsEntity", "SettingsKey", typeof(System.Guid), false, false, false, false,  (int)UserColumnSettingsFieldIndex.SettingsKey, 0, 0, 0);
			this.AddElementFieldInfo("UserColumnSettingsEntity", "UserID", typeof(System.Int64), false, true, false, false,  (int)UserColumnSettingsFieldIndex.UserID, 0, 0, 19);
			this.AddElementFieldInfo("UserColumnSettingsEntity", "InitialSortType", typeof(System.Int32), false, false, false, false,  (int)UserColumnSettingsFieldIndex.InitialSortType, 0, 0, 10);
			this.AddElementFieldInfo("UserColumnSettingsEntity", "GridColumnLayoutID", typeof(System.Int64), false, true, false, false,  (int)UserColumnSettingsFieldIndex.GridColumnLayoutID, 0, 0, 19);
		}
		/// <summary>Inits UserSettingsEntity's FieldInfo objects</summary>
		private void InitUserSettingsEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(UserSettingsFieldIndex), "UserSettingsEntity");
			this.AddElementFieldInfo("UserSettingsEntity", "UserID", typeof(System.Int64), true, true, false, false,  (int)UserSettingsFieldIndex.UserID, 0, 0, 19);
			this.AddElementFieldInfo("UserSettingsEntity", "DisplayColorScheme", typeof(System.Int32), false, false, false, false,  (int)UserSettingsFieldIndex.DisplayColorScheme, 0, 0, 10);
			this.AddElementFieldInfo("UserSettingsEntity", "DisplaySystemTray", typeof(System.Boolean), false, false, false, false,  (int)UserSettingsFieldIndex.DisplaySystemTray, 0, 0, 0);
			this.AddElementFieldInfo("UserSettingsEntity", "WindowLayout", typeof(System.Byte[]), false, false, false, false,  (int)UserSettingsFieldIndex.WindowLayout, 2147483647, 0, 0);
			this.AddElementFieldInfo("UserSettingsEntity", "GridMenuLayout", typeof(System.String), false, false, false, true,  (int)UserSettingsFieldIndex.GridMenuLayout, 2147483647, 0, 0);
			this.AddElementFieldInfo("UserSettingsEntity", "FilterInitialUseLastActive", typeof(System.Boolean), false, false, false, false,  (int)UserSettingsFieldIndex.FilterInitialUseLastActive, 0, 0, 0);
			this.AddElementFieldInfo("UserSettingsEntity", "FilterInitialSpecified", typeof(System.Int64), false, false, false, false,  (int)UserSettingsFieldIndex.FilterInitialSpecified, 0, 0, 19);
			this.AddElementFieldInfo("UserSettingsEntity", "FilterInitialSortType", typeof(System.Int32), false, false, false, false,  (int)UserSettingsFieldIndex.FilterInitialSortType, 0, 0, 10);
			this.AddElementFieldInfo("UserSettingsEntity", "OrderFilterLastActive", typeof(System.Int64), false, false, false, false,  (int)UserSettingsFieldIndex.OrderFilterLastActive, 0, 0, 19);
			this.AddElementFieldInfo("UserSettingsEntity", "OrderFilterExpandedFolders", typeof(System.String), false, false, false, true,  (int)UserSettingsFieldIndex.OrderFilterExpandedFolders, 2147483647, 0, 0);
			this.AddElementFieldInfo("UserSettingsEntity", "ShippingWeightFormat", typeof(System.Int32), false, false, false, false,  (int)UserSettingsFieldIndex.ShippingWeightFormat, 0, 0, 10);
			this.AddElementFieldInfo("UserSettingsEntity", "TemplateExpandedFolders", typeof(System.String), false, false, false, true,  (int)UserSettingsFieldIndex.TemplateExpandedFolders, 2147483647, 0, 0);
			this.AddElementFieldInfo("UserSettingsEntity", "TemplateLastSelected", typeof(System.Int64), false, false, false, false,  (int)UserSettingsFieldIndex.TemplateLastSelected, 0, 0, 19);
			this.AddElementFieldInfo("UserSettingsEntity", "CustomerFilterLastActive", typeof(System.Int64), false, false, false, false,  (int)UserSettingsFieldIndex.CustomerFilterLastActive, 0, 0, 19);
			this.AddElementFieldInfo("UserSettingsEntity", "CustomerFilterExpandedFolders", typeof(System.String), false, false, false, true,  (int)UserSettingsFieldIndex.CustomerFilterExpandedFolders, 2147483647, 0, 0);
			this.AddElementFieldInfo("UserSettingsEntity", "NextGlobalPostNotificationDate", typeof(System.DateTime), false, false, false, false,  (int)UserSettingsFieldIndex.NextGlobalPostNotificationDate, 0, 0, 0);
			this.AddElementFieldInfo("UserSettingsEntity", "SingleScanSettings", typeof(System.Int32), false, false, false, false,  (int)UserSettingsFieldIndex.SingleScanSettings, 0, 0, 10);
			this.AddElementFieldInfo("UserSettingsEntity", "AutoWeigh", typeof(System.Boolean), false, false, false, false,  (int)UserSettingsFieldIndex.AutoWeigh, 0, 0, 0);
		}
		/// <summary>Inits UspsAccountEntity's FieldInfo objects</summary>
		private void InitUspsAccountEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(UspsAccountFieldIndex), "UspsAccountEntity");
			this.AddElementFieldInfo("UspsAccountEntity", "UspsAccountID", typeof(System.Int64), true, false, true, false,  (int)UspsAccountFieldIndex.UspsAccountID, 0, 0, 19);
			this.AddElementFieldInfo("UspsAccountEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)UspsAccountFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "Description", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Description, 50, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "Username", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Username, 50, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "Password", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Password, 100, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "FirstName", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.FirstName, 30, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "MiddleName", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.MiddleName, 30, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "LastName", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.LastName, 30, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "Company", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Company, 30, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "Street1", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Street1, 60, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "Street2", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Street2, 60, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "Street3", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Street3, 60, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "City", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.City, 50, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.StateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.PostalCode, 20, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.CountryCode, 50, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "Phone", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Phone, 25, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "Email", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Email, 100, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "Website", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Website, 50, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "MailingPostalCode", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.MailingPostalCode, 20, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "UspsReseller", typeof(System.Int32), false, false, false, false,  (int)UspsAccountFieldIndex.UspsReseller, 0, 0, 10);
			this.AddElementFieldInfo("UspsAccountEntity", "ContractType", typeof(System.Int32), false, false, false, false,  (int)UspsAccountFieldIndex.ContractType, 0, 0, 10);
			this.AddElementFieldInfo("UspsAccountEntity", "CreatedDate", typeof(System.DateTime), false, false, false, false,  (int)UspsAccountFieldIndex.CreatedDate, 0, 0, 0);
			this.AddElementFieldInfo("UspsAccountEntity", "PendingInitialAccount", typeof(System.Int32), false, false, false, false,  (int)UspsAccountFieldIndex.PendingInitialAccount, 0, 0, 10);
			this.AddElementFieldInfo("UspsAccountEntity", "GlobalPostAvailability", typeof(System.Int32), false, false, false, false,  (int)UspsAccountFieldIndex.GlobalPostAvailability, 0, 0, 10);
		}
		/// <summary>Inits UspsProfileEntity's FieldInfo objects</summary>
		private void InitUspsProfileEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(UspsProfileFieldIndex), "UspsProfileEntity");
			this.AddElementFieldInfo("UspsProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)UspsProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			this.AddElementFieldInfo("UspsProfileEntity", "UspsAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)UspsProfileFieldIndex.UspsAccountID, 0, 0, 19);
			this.AddElementFieldInfo("UspsProfileEntity", "HidePostage", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UspsProfileFieldIndex.HidePostage, 0, 0, 0);
			this.AddElementFieldInfo("UspsProfileEntity", "RequireFullAddressValidation", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UspsProfileFieldIndex.RequireFullAddressValidation, 0, 0, 0);
			this.AddElementFieldInfo("UspsProfileEntity", "RateShop", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UspsProfileFieldIndex.RateShop, 0, 0, 0);
		}
		/// <summary>Inits UspsScanFormEntity's FieldInfo objects</summary>
		private void InitUspsScanFormEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(UspsScanFormFieldIndex), "UspsScanFormEntity");
			this.AddElementFieldInfo("UspsScanFormEntity", "UspsScanFormID", typeof(System.Int64), true, false, true, false,  (int)UspsScanFormFieldIndex.UspsScanFormID, 0, 0, 19);
			this.AddElementFieldInfo("UspsScanFormEntity", "UspsAccountID", typeof(System.Int64), false, false, false, false,  (int)UspsScanFormFieldIndex.UspsAccountID, 0, 0, 19);
			this.AddElementFieldInfo("UspsScanFormEntity", "ScanFormTransactionID", typeof(System.String), false, false, false, false,  (int)UspsScanFormFieldIndex.ScanFormTransactionID, 100, 0, 0);
			this.AddElementFieldInfo("UspsScanFormEntity", "ScanFormUrl", typeof(System.String), false, false, false, false,  (int)UspsScanFormFieldIndex.ScanFormUrl, 2048, 0, 0);
			this.AddElementFieldInfo("UspsScanFormEntity", "CreatedDate", typeof(System.DateTime), false, false, false, false,  (int)UspsScanFormFieldIndex.CreatedDate, 0, 0, 0);
			this.AddElementFieldInfo("UspsScanFormEntity", "ScanFormBatchID", typeof(System.Int64), false, true, false, false,  (int)UspsScanFormFieldIndex.ScanFormBatchID, 0, 0, 19);
			this.AddElementFieldInfo("UspsScanFormEntity", "Description", typeof(System.String), false, false, false, false,  (int)UspsScanFormFieldIndex.Description, 100, 0, 0);
		}
		/// <summary>Inits UspsShipmentEntity's FieldInfo objects</summary>
		private void InitUspsShipmentEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(UspsShipmentFieldIndex), "UspsShipmentEntity");
			this.AddElementFieldInfo("UspsShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)UspsShipmentFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("UspsShipmentEntity", "UspsAccountID", typeof(System.Int64), false, false, false, false,  (int)UspsShipmentFieldIndex.UspsAccountID, 0, 0, 19);
			this.AddElementFieldInfo("UspsShipmentEntity", "HidePostage", typeof(System.Boolean), false, false, false, false,  (int)UspsShipmentFieldIndex.HidePostage, 0, 0, 0);
			this.AddElementFieldInfo("UspsShipmentEntity", "RequireFullAddressValidation", typeof(System.Boolean), false, false, false, false,  (int)UspsShipmentFieldIndex.RequireFullAddressValidation, 0, 0, 0);
			this.AddElementFieldInfo("UspsShipmentEntity", "IntegratorTransactionID", typeof(System.Guid), false, false, false, false,  (int)UspsShipmentFieldIndex.IntegratorTransactionID, 0, 0, 0);
			this.AddElementFieldInfo("UspsShipmentEntity", "UspsTransactionID", typeof(System.Guid), false, false, false, false,  (int)UspsShipmentFieldIndex.UspsTransactionID, 0, 0, 0);
			this.AddElementFieldInfo("UspsShipmentEntity", "OriginalUspsAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)UspsShipmentFieldIndex.OriginalUspsAccountID, 0, 0, 19);
			this.AddElementFieldInfo("UspsShipmentEntity", "ScanFormBatchID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)UspsShipmentFieldIndex.ScanFormBatchID, 0, 0, 19);
			this.AddElementFieldInfo("UspsShipmentEntity", "RequestedLabelFormat", typeof(System.Int32), false, false, false, false,  (int)UspsShipmentFieldIndex.RequestedLabelFormat, 0, 0, 10);
			this.AddElementFieldInfo("UspsShipmentEntity", "RateShop", typeof(System.Boolean), false, false, false, false,  (int)UspsShipmentFieldIndex.RateShop, 0, 0, 0);
		}
		/// <summary>Inits ValidatedAddressEntity's FieldInfo objects</summary>
		private void InitValidatedAddressEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(ValidatedAddressFieldIndex), "ValidatedAddressEntity");
			this.AddElementFieldInfo("ValidatedAddressEntity", "ValidatedAddressID", typeof(System.Int64), true, false, true, false,  (int)ValidatedAddressFieldIndex.ValidatedAddressID, 0, 0, 19);
			this.AddElementFieldInfo("ValidatedAddressEntity", "ConsumerID", typeof(System.Int64), false, true, false, false,  (int)ValidatedAddressFieldIndex.ConsumerID, 0, 0, 19);
			this.AddElementFieldInfo("ValidatedAddressEntity", "AddressPrefix", typeof(System.String), false, false, false, false,  (int)ValidatedAddressFieldIndex.AddressPrefix, 10, 0, 0);
			this.AddElementFieldInfo("ValidatedAddressEntity", "IsOriginal", typeof(System.Boolean), false, false, false, false,  (int)ValidatedAddressFieldIndex.IsOriginal, 0, 0, 0);
			this.AddElementFieldInfo("ValidatedAddressEntity", "Street1", typeof(System.String), false, false, false, false,  (int)ValidatedAddressFieldIndex.Street1, 60, 0, 0);
			this.AddElementFieldInfo("ValidatedAddressEntity", "Street2", typeof(System.String), false, false, false, false,  (int)ValidatedAddressFieldIndex.Street2, 60, 0, 0);
			this.AddElementFieldInfo("ValidatedAddressEntity", "Street3", typeof(System.String), false, false, false, false,  (int)ValidatedAddressFieldIndex.Street3, 60, 0, 0);
			this.AddElementFieldInfo("ValidatedAddressEntity", "City", typeof(System.String), false, false, false, false,  (int)ValidatedAddressFieldIndex.City, 50, 0, 0);
			this.AddElementFieldInfo("ValidatedAddressEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)ValidatedAddressFieldIndex.StateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("ValidatedAddressEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)ValidatedAddressFieldIndex.PostalCode, 20, 0, 0);
			this.AddElementFieldInfo("ValidatedAddressEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)ValidatedAddressFieldIndex.CountryCode, 50, 0, 0);
			this.AddElementFieldInfo("ValidatedAddressEntity", "ResidentialStatus", typeof(System.Int32), false, false, false, false,  (int)ValidatedAddressFieldIndex.ResidentialStatus, 0, 0, 10);
			this.AddElementFieldInfo("ValidatedAddressEntity", "POBox", typeof(System.Int32), false, false, false, false,  (int)ValidatedAddressFieldIndex.POBox, 0, 0, 10);
			this.AddElementFieldInfo("ValidatedAddressEntity", "USTerritory", typeof(System.Int32), false, false, false, false,  (int)ValidatedAddressFieldIndex.USTerritory, 0, 0, 10);
			this.AddElementFieldInfo("ValidatedAddressEntity", "MilitaryAddress", typeof(System.Int32), false, false, false, false,  (int)ValidatedAddressFieldIndex.MilitaryAddress, 0, 0, 10);
		}
		/// <summary>Inits VersionSignoffEntity's FieldInfo objects</summary>
		private void InitVersionSignoffEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(VersionSignoffFieldIndex), "VersionSignoffEntity");
			this.AddElementFieldInfo("VersionSignoffEntity", "VersionSignoffID", typeof(System.Int64), true, false, true, false,  (int)VersionSignoffFieldIndex.VersionSignoffID, 0, 0, 19);
			this.AddElementFieldInfo("VersionSignoffEntity", "Version", typeof(System.String), false, false, false, false,  (int)VersionSignoffFieldIndex.Version, 30, 0, 0);
			this.AddElementFieldInfo("VersionSignoffEntity", "UserID", typeof(System.Int64), false, false, false, false,  (int)VersionSignoffFieldIndex.UserID, 0, 0, 19);
			this.AddElementFieldInfo("VersionSignoffEntity", "ComputerID", typeof(System.Int64), false, true, false, false,  (int)VersionSignoffFieldIndex.ComputerID, 0, 0, 19);
		}
		/// <summary>Inits VolusionStoreEntity's FieldInfo objects</summary>
		private void InitVolusionStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(VolusionStoreFieldIndex), "VolusionStoreEntity");
			this.AddElementFieldInfo("VolusionStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)VolusionStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("VolusionStoreEntity", "StoreUrl", typeof(System.String), false, false, false, false,  (int)VolusionStoreFieldIndex.StoreUrl, 255, 0, 0);
			this.AddElementFieldInfo("VolusionStoreEntity", "WebUserName", typeof(System.String), false, false, false, false,  (int)VolusionStoreFieldIndex.WebUserName, 50, 0, 0);
			this.AddElementFieldInfo("VolusionStoreEntity", "WebPassword", typeof(System.String), false, false, false, false,  (int)VolusionStoreFieldIndex.WebPassword, 70, 0, 0);
			this.AddElementFieldInfo("VolusionStoreEntity", "ApiPassword", typeof(System.String), false, false, false, false,  (int)VolusionStoreFieldIndex.ApiPassword, 100, 0, 0);
			this.AddElementFieldInfo("VolusionStoreEntity", "PaymentMethods", typeof(System.String), false, false, false, false,  (int)VolusionStoreFieldIndex.PaymentMethods, 2147483647, 0, 0);
			this.AddElementFieldInfo("VolusionStoreEntity", "ShipmentMethods", typeof(System.String), false, false, false, false,  (int)VolusionStoreFieldIndex.ShipmentMethods, 2147483647, 0, 0);
			this.AddElementFieldInfo("VolusionStoreEntity", "DownloadOrderStatuses", typeof(System.String), false, false, false, false,  (int)VolusionStoreFieldIndex.DownloadOrderStatuses, 255, 0, 0);
			this.AddElementFieldInfo("VolusionStoreEntity", "ServerTimeZone", typeof(System.String), false, false, false, false,  (int)VolusionStoreFieldIndex.ServerTimeZone, 30, 0, 0);
			this.AddElementFieldInfo("VolusionStoreEntity", "ServerTimeZoneDST", typeof(System.Boolean), false, false, false, false,  (int)VolusionStoreFieldIndex.ServerTimeZoneDST, 0, 0, 0);
		}
		/// <summary>Inits WalmartOrderEntity's FieldInfo objects</summary>
		private void InitWalmartOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(WalmartOrderFieldIndex), "WalmartOrderEntity");
			this.AddElementFieldInfo("WalmartOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)WalmartOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("WalmartOrderEntity", "PurchaseOrderID", typeof(System.String), false, false, false, false,  (int)WalmartOrderFieldIndex.PurchaseOrderID, 32, 0, 0);
			this.AddElementFieldInfo("WalmartOrderEntity", "CustomerOrderID", typeof(System.String), false, false, false, false,  (int)WalmartOrderFieldIndex.CustomerOrderID, 50, 0, 0);
			this.AddElementFieldInfo("WalmartOrderEntity", "EstimatedDeliveryDate", typeof(System.DateTime), false, false, false, false,  (int)WalmartOrderFieldIndex.EstimatedDeliveryDate, 0, 0, 0);
			this.AddElementFieldInfo("WalmartOrderEntity", "EstimatedShipDate", typeof(System.DateTime), false, false, false, false,  (int)WalmartOrderFieldIndex.EstimatedShipDate, 0, 0, 0);
		}
		/// <summary>Inits WalmartOrderItemEntity's FieldInfo objects</summary>
		private void InitWalmartOrderItemEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(WalmartOrderItemFieldIndex), "WalmartOrderItemEntity");
			this.AddElementFieldInfo("WalmartOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, true, false,  (int)WalmartOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("WalmartOrderItemEntity", "LineNumber", typeof(System.String), false, false, false, false,  (int)WalmartOrderItemFieldIndex.LineNumber, 20, 0, 0);
		}
		/// <summary>Inits WalmartStoreEntity's FieldInfo objects</summary>
		private void InitWalmartStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(WalmartStoreFieldIndex), "WalmartStoreEntity");
			this.AddElementFieldInfo("WalmartStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)WalmartStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("WalmartStoreEntity", "ConsumerID", typeof(System.String), false, false, false, false,  (int)WalmartStoreFieldIndex.ConsumerID, 50, 0, 0);
			this.AddElementFieldInfo("WalmartStoreEntity", "PrivateKey", typeof(System.String), false, false, false, false,  (int)WalmartStoreFieldIndex.PrivateKey, 2000, 0, 0);
			this.AddElementFieldInfo("WalmartStoreEntity", "ChannelType", typeof(System.String), false, false, false, false,  (int)WalmartStoreFieldIndex.ChannelType, 50, 0, 0);
			this.AddElementFieldInfo("WalmartStoreEntity", "DownloadModifiedNumberOfDaysBack", typeof(System.Int32), false, false, false, false,  (int)WalmartStoreFieldIndex.DownloadModifiedNumberOfDaysBack, 0, 0, 10);
		}
		/// <summary>Inits WorldShipGoodsEntity's FieldInfo objects</summary>
		private void InitWorldShipGoodsEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(WorldShipGoodsFieldIndex), "WorldShipGoodsEntity");
			this.AddElementFieldInfo("WorldShipGoodsEntity", "WorldShipGoodsID", typeof(System.Int64), true, false, true, false,  (int)WorldShipGoodsFieldIndex.WorldShipGoodsID, 0, 0, 19);
			this.AddElementFieldInfo("WorldShipGoodsEntity", "ShipmentID", typeof(System.Int64), false, true, false, false,  (int)WorldShipGoodsFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("WorldShipGoodsEntity", "ShipmentCustomsItemID", typeof(System.Int64), false, false, false, false,  (int)WorldShipGoodsFieldIndex.ShipmentCustomsItemID, 0, 0, 19);
			this.AddElementFieldInfo("WorldShipGoodsEntity", "Description", typeof(System.String), false, false, false, false,  (int)WorldShipGoodsFieldIndex.Description, 150, 0, 0);
			this.AddElementFieldInfo("WorldShipGoodsEntity", "TariffCode", typeof(System.String), false, false, false, false,  (int)WorldShipGoodsFieldIndex.TariffCode, 15, 0, 0);
			this.AddElementFieldInfo("WorldShipGoodsEntity", "CountryOfOrigin", typeof(System.String), false, false, false, false,  (int)WorldShipGoodsFieldIndex.CountryOfOrigin, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipGoodsEntity", "Units", typeof(System.Int32), false, false, false, false,  (int)WorldShipGoodsFieldIndex.Units, 0, 0, 10);
			this.AddElementFieldInfo("WorldShipGoodsEntity", "UnitOfMeasure", typeof(System.String), false, false, false, false,  (int)WorldShipGoodsFieldIndex.UnitOfMeasure, 5, 0, 0);
			this.AddElementFieldInfo("WorldShipGoodsEntity", "UnitPrice", typeof(System.Decimal), false, false, false, false,  (int)WorldShipGoodsFieldIndex.UnitPrice, 0, 4, 19);
			this.AddElementFieldInfo("WorldShipGoodsEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)WorldShipGoodsFieldIndex.Weight, 0, 0, 38);
			this.AddElementFieldInfo("WorldShipGoodsEntity", "InvoiceCurrencyCode", typeof(System.String), false, false, false, true,  (int)WorldShipGoodsFieldIndex.InvoiceCurrencyCode, 3, 0, 0);
		}
		/// <summary>Inits WorldShipPackageEntity's FieldInfo objects</summary>
		private void InitWorldShipPackageEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(WorldShipPackageFieldIndex), "WorldShipPackageEntity");
			this.AddElementFieldInfo("WorldShipPackageEntity", "UpsPackageID", typeof(System.Int64), true, false, false, false,  (int)WorldShipPackageFieldIndex.UpsPackageID, 0, 0, 19);
			this.AddElementFieldInfo("WorldShipPackageEntity", "ShipmentID", typeof(System.Int64), false, true, false, false,  (int)WorldShipPackageFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("WorldShipPackageEntity", "PackageType", typeof(System.String), false, false, false, false,  (int)WorldShipPackageFieldIndex.PackageType, 35, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)WorldShipPackageFieldIndex.Weight, 0, 0, 38);
			this.AddElementFieldInfo("WorldShipPackageEntity", "ReferenceNumber", typeof(System.String), false, false, false, false,  (int)WorldShipPackageFieldIndex.ReferenceNumber, 35, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "ReferenceNumber2", typeof(System.String), false, false, false, false,  (int)WorldShipPackageFieldIndex.ReferenceNumber2, 35, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "CodOption", typeof(System.String), false, false, false, false,  (int)WorldShipPackageFieldIndex.CodOption, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "CodAmount", typeof(System.Decimal), false, false, false, false,  (int)WorldShipPackageFieldIndex.CodAmount, 0, 4, 19);
			this.AddElementFieldInfo("WorldShipPackageEntity", "CodCashOnly", typeof(System.String), false, false, false, false,  (int)WorldShipPackageFieldIndex.CodCashOnly, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "DeliveryConfirmation", typeof(System.String), false, false, false, false,  (int)WorldShipPackageFieldIndex.DeliveryConfirmation, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "DeliveryConfirmationSignature", typeof(System.String), false, false, false, false,  (int)WorldShipPackageFieldIndex.DeliveryConfirmationSignature, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "DeliveryConfirmationAdult", typeof(System.String), false, false, false, false,  (int)WorldShipPackageFieldIndex.DeliveryConfirmationAdult, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "Length", typeof(System.Int32), false, false, false, false,  (int)WorldShipPackageFieldIndex.Length, 0, 0, 10);
			this.AddElementFieldInfo("WorldShipPackageEntity", "Width", typeof(System.Int32), false, false, false, false,  (int)WorldShipPackageFieldIndex.Width, 0, 0, 10);
			this.AddElementFieldInfo("WorldShipPackageEntity", "Height", typeof(System.Int32), false, false, false, false,  (int)WorldShipPackageFieldIndex.Height, 0, 0, 10);
			this.AddElementFieldInfo("WorldShipPackageEntity", "DeclaredValueAmount", typeof(Nullable<System.Double>), false, false, false, true,  (int)WorldShipPackageFieldIndex.DeclaredValueAmount, 0, 0, 38);
			this.AddElementFieldInfo("WorldShipPackageEntity", "DeclaredValueOption", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.DeclaredValueOption, 2, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "CN22GoodsType", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.CN22GoodsType, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "CN22Description", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.CN22Description, 100, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "PostalSubClass", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.PostalSubClass, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "MIDeliveryConfirmation", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.MIDeliveryConfirmation, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "QvnOption", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.QvnOption, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "QvnFrom", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.QvnFrom, 35, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "QvnSubjectLine", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.QvnSubjectLine, 18, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "QvnMemo", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.QvnMemo, 150, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "Qvn1ShipNotify", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn1ShipNotify, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "Qvn1ContactName", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn1ContactName, 35, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "Qvn1Email", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn1Email, 100, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "Qvn2ShipNotify", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn2ShipNotify, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "Qvn2ContactName", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn2ContactName, 35, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "Qvn2Email", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn2Email, 100, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "Qvn3ShipNotify", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn3ShipNotify, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "Qvn3ContactName", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn3ContactName, 35, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "Qvn3Email", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn3Email, 100, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "ShipperRelease", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.ShipperRelease, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "AdditionalHandlingEnabled", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.AdditionalHandlingEnabled, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "VerbalConfirmationOption", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.VerbalConfirmationOption, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "VerbalConfirmationContactName", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.VerbalConfirmationContactName, 35, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "VerbalConfirmationTelephone", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.VerbalConfirmationTelephone, 15, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "DryIceRegulationSet", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.DryIceRegulationSet, 5, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "DryIceWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)WorldShipPackageFieldIndex.DryIceWeight, 0, 0, 38);
			this.AddElementFieldInfo("WorldShipPackageEntity", "DryIceMedicalPurpose", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.DryIceMedicalPurpose, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "DryIceOption", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.DryIceOption, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipPackageEntity", "DryIceWeightUnitOfMeasure", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.DryIceWeightUnitOfMeasure, 10, 0, 0);
		}
		/// <summary>Inits WorldShipProcessedEntity's FieldInfo objects</summary>
		private void InitWorldShipProcessedEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(WorldShipProcessedFieldIndex), "WorldShipProcessedEntity");
			this.AddElementFieldInfo("WorldShipProcessedEntity", "WorldShipProcessedID", typeof(System.Int64), true, false, true, false,  (int)WorldShipProcessedFieldIndex.WorldShipProcessedID, 0, 0, 19);
			this.AddElementFieldInfo("WorldShipProcessedEntity", "ShipmentID", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.ShipmentID, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipProcessedEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)WorldShipProcessedFieldIndex.RowVersion, 2147483647, 0, 0);
			this.AddElementFieldInfo("WorldShipProcessedEntity", "PublishedCharges", typeof(System.Double), false, false, false, false,  (int)WorldShipProcessedFieldIndex.PublishedCharges, 0, 0, 38);
			this.AddElementFieldInfo("WorldShipProcessedEntity", "NegotiatedCharges", typeof(System.Double), false, false, false, false,  (int)WorldShipProcessedFieldIndex.NegotiatedCharges, 0, 0, 38);
			this.AddElementFieldInfo("WorldShipProcessedEntity", "TrackingNumber", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.TrackingNumber, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipProcessedEntity", "UspsTrackingNumber", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.UspsTrackingNumber, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipProcessedEntity", "ServiceType", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.ServiceType, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipProcessedEntity", "PackageType", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.PackageType, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipProcessedEntity", "UpsPackageID", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.UpsPackageID, 20, 0, 0);
			this.AddElementFieldInfo("WorldShipProcessedEntity", "DeclaredValueAmount", typeof(Nullable<System.Double>), false, false, false, true,  (int)WorldShipProcessedFieldIndex.DeclaredValueAmount, 0, 0, 38);
			this.AddElementFieldInfo("WorldShipProcessedEntity", "DeclaredValueOption", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.DeclaredValueOption, 2, 0, 0);
			this.AddElementFieldInfo("WorldShipProcessedEntity", "WorldShipShipmentID", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.WorldShipShipmentID, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipProcessedEntity", "VoidIndicator", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.VoidIndicator, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipProcessedEntity", "NumberOfPackages", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.NumberOfPackages, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipProcessedEntity", "LeadTrackingNumber", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.LeadTrackingNumber, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipProcessedEntity", "ShipmentIdCalculated", typeof(Nullable<System.Int64>), false, true, true, true,  (int)WorldShipProcessedFieldIndex.ShipmentIdCalculated, 0, 0, 19);
		}
		/// <summary>Inits WorldShipShipmentEntity's FieldInfo objects</summary>
		private void InitWorldShipShipmentEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(WorldShipShipmentFieldIndex), "WorldShipShipmentEntity");
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)WorldShipShipmentFieldIndex.ShipmentID, 0, 0, 19);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "OrderNumber", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.OrderNumber, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "FromCompanyOrName", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromCompanyOrName, 35, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "FromAttention", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromAttention, 35, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "FromAddress1", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromAddress1, 60, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "FromAddress2", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromAddress2, 60, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "FromAddress3", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromAddress3, 60, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "FromCountryCode", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromCountryCode, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "FromPostalCode", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromPostalCode, 20, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "FromCity", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromCity, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "FromStateProvCode", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromStateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "FromTelephone", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromTelephone, 25, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "FromEmail", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromEmail, 100, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "FromAccountNumber", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromAccountNumber, 10, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ToCustomerID", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToCustomerID, 30, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ToCompanyOrName", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToCompanyOrName, 35, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ToAttention", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToAttention, 35, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ToAddress1", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToAddress1, 60, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ToAddress2", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToAddress2, 60, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ToAddress3", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToAddress3, 60, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ToCountryCode", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToCountryCode, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ToPostalCode", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToPostalCode, 20, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ToCity", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToCity, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ToStateProvCode", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToStateProvCode, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ToTelephone", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToTelephone, 25, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ToEmail", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToEmail, 100, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ToAccountNumber", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToAccountNumber, 10, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ToResidential", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToResidential, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ServiceType", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ServiceType, 3, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "BillTransportationTo", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.BillTransportationTo, 20, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "SaturdayDelivery", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.SaturdayDelivery, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "QvnOption", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.QvnOption, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "QvnFrom", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.QvnFrom, 35, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "QvnSubjectLine", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.QvnSubjectLine, 18, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "QvnMemo", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.QvnMemo, 150, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn1ShipNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn1ShipNotify, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn1DeliveryNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn1DeliveryNotify, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn1ExceptionNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn1ExceptionNotify, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn1ContactName", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn1ContactName, 35, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn1Email", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn1Email, 100, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn2ShipNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn2ShipNotify, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn2DeliveryNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn2DeliveryNotify, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn2ExceptionNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn2ExceptionNotify, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn2ContactName", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn2ContactName, 35, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn2Email", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn2Email, 100, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn3ShipNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn3ShipNotify, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn3DeliveryNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn3DeliveryNotify, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn3ExceptionNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn3ExceptionNotify, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn3ContactName", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn3ContactName, 35, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn3Email", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn3Email, 100, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "CustomsDescriptionOfGoods", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.CustomsDescriptionOfGoods, 150, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "CustomsDocumentsOnly", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.CustomsDocumentsOnly, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ShipperNumber", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ShipperNumber, 10, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "PackageCount", typeof(System.Int32), false, false, false, false,  (int)WorldShipShipmentFieldIndex.PackageCount, 0, 0, 10);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "DeliveryConfirmation", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.DeliveryConfirmation, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "DeliveryConfirmationAdult", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.DeliveryConfirmationAdult, 1, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "InvoiceTermsOfSale", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.InvoiceTermsOfSale, 3, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "InvoiceReasonForExport", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.InvoiceReasonForExport, 2, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "InvoiceComments", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.InvoiceComments, 200, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "InvoiceCurrencyCode", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.InvoiceCurrencyCode, 3, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "InvoiceChargesFreight", typeof(Nullable<System.Decimal>), false, false, false, true,  (int)WorldShipShipmentFieldIndex.InvoiceChargesFreight, 0, 4, 19);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "InvoiceChargesInsurance", typeof(Nullable<System.Decimal>), false, false, false, true,  (int)WorldShipShipmentFieldIndex.InvoiceChargesInsurance, 0, 4, 19);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "InvoiceChargesOther", typeof(Nullable<System.Decimal>), false, false, false, true,  (int)WorldShipShipmentFieldIndex.InvoiceChargesOther, 0, 4, 19);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "ShipmentProcessedOnComputerID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)WorldShipShipmentFieldIndex.ShipmentProcessedOnComputerID, 0, 0, 19);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "UspsEndorsement", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.UspsEndorsement, 50, 0, 0);
			this.AddElementFieldInfo("WorldShipShipmentEntity", "CarbonNeutral", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.CarbonNeutral, 10, 0, 0);
		}
		/// <summary>Inits YahooOrderEntity's FieldInfo objects</summary>
		private void InitYahooOrderEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(YahooOrderFieldIndex), "YahooOrderEntity");
			this.AddElementFieldInfo("YahooOrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)YahooOrderFieldIndex.OrderID, 0, 0, 19);
			this.AddElementFieldInfo("YahooOrderEntity", "YahooOrderID", typeof(System.String), false, false, false, false,  (int)YahooOrderFieldIndex.YahooOrderID, 50, 0, 0);
		}
		/// <summary>Inits YahooOrderItemEntity's FieldInfo objects</summary>
		private void InitYahooOrderItemEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(YahooOrderItemFieldIndex), "YahooOrderItemEntity");
			this.AddElementFieldInfo("YahooOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, true, false,  (int)YahooOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			this.AddElementFieldInfo("YahooOrderItemEntity", "YahooProductID", typeof(System.String), false, false, false, false,  (int)YahooOrderItemFieldIndex.YahooProductID, 255, 0, 0);
			this.AddElementFieldInfo("YahooOrderItemEntity", "Url", typeof(System.String), false, false, false, false,  (int)YahooOrderItemFieldIndex.Url, 255, 0, 0);
		}
		/// <summary>Inits YahooProductEntity's FieldInfo objects</summary>
		private void InitYahooProductEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(YahooProductFieldIndex), "YahooProductEntity");
			this.AddElementFieldInfo("YahooProductEntity", "StoreID", typeof(System.Int64), true, true, false, false,  (int)YahooProductFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("YahooProductEntity", "YahooProductID", typeof(System.String), true, false, false, false,  (int)YahooProductFieldIndex.YahooProductID, 255, 0, 0);
			this.AddElementFieldInfo("YahooProductEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)YahooProductFieldIndex.Weight, 0, 0, 38);
		}
		/// <summary>Inits YahooStoreEntity's FieldInfo objects</summary>
		private void InitYahooStoreEntityInfos()
		{
			this.AddFieldIndexEnumForElementName(typeof(YahooStoreFieldIndex), "YahooStoreEntity");
			this.AddElementFieldInfo("YahooStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)YahooStoreFieldIndex.StoreID, 0, 0, 19);
			this.AddElementFieldInfo("YahooStoreEntity", "YahooEmailAccountID", typeof(System.Int64), false, true, false, false,  (int)YahooStoreFieldIndex.YahooEmailAccountID, 0, 0, 19);
			this.AddElementFieldInfo("YahooStoreEntity", "TrackingUpdatePassword", typeof(System.String), false, false, false, false,  (int)YahooStoreFieldIndex.TrackingUpdatePassword, 100, 0, 0);
			this.AddElementFieldInfo("YahooStoreEntity", "YahooStoreID", typeof(System.String), false, false, false, false,  (int)YahooStoreFieldIndex.YahooStoreID, 50, 0, 0);
			this.AddElementFieldInfo("YahooStoreEntity", "AccessToken", typeof(System.String), false, false, false, false,  (int)YahooStoreFieldIndex.AccessToken, 200, 0, 0);
			this.AddElementFieldInfo("YahooStoreEntity", "BackupOrderNumber", typeof(Nullable<System.Int64>), false, false, false, true,  (int)YahooStoreFieldIndex.BackupOrderNumber, 0, 0, 19);
		}
		
	}
}




