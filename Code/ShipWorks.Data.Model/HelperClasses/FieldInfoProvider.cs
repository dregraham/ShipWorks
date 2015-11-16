///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates.NET20
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.HelperClasses
{
	
	// __LLBLGENPRO_USER_CODE_REGION_START AdditionalNamespaces
	// __LLBLGENPRO_USER_CODE_REGION_END
	
	/// <summary>
	/// Singleton implementation of the FieldInfoProvider. This class is the singleton wrapper through which the actual instance is retrieved.
	/// </summary>
	/// <remarks>It uses a single instance of an internal class. The access isn't marked with locks as the FieldInfoProviderBase class is threadsafe.</remarks>
	internal sealed class FieldInfoProviderSingleton
	{
		#region Class Member Declarations
		private static readonly IFieldInfoProvider _providerInstance = new FieldInfoProviderCore();
		#endregion
		
		/// <summary>private ctor to prevent instances of this class.</summary>
		private FieldInfoProviderSingleton()
		{
		}

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
			base.InitClass( (172 + 0));
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
			InitStatusPresetEntityInfos();
			InitStoreEntityInfos();
			InitSystemDataEntityInfos();
			InitTemplateEntityInfos();
			InitTemplateComputerSettingsEntityInfos();
			InitTemplateFolderEntityInfos();
			InitTemplateStoreSettingsEntityInfos();
			InitTemplateUserSettingsEntityInfos();
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
			InitWorldShipGoodsEntityInfos();
			InitWorldShipPackageEntityInfos();
			InitWorldShipProcessedEntityInfos();
			InitWorldShipShipmentEntityInfos();
			InitYahooOrderEntityInfos();
			InitYahooOrderItemEntityInfos();
			InitYahooProductEntityInfos();
			InitYahooStoreEntityInfos();

			base.ConstructElementFieldStructures(InheritanceInfoProviderSingleton.GetInstance());
		}

		/// <summary>Inits ActionEntity's FieldInfo objects</summary>
		private void InitActionEntityInfos()
		{
			base.AddElementFieldInfo("ActionEntity", "ActionID", typeof(System.Int64), true, false, true, false,  (int)ActionFieldIndex.ActionID, 0, 0, 19);
			base.AddElementFieldInfo("ActionEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ActionFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("ActionEntity", "Name", typeof(System.String), false, false, false, false,  (int)ActionFieldIndex.Name, 50, 0, 0);
			base.AddElementFieldInfo("ActionEntity", "Enabled", typeof(System.Boolean), false, false, false, false,  (int)ActionFieldIndex.Enabled, 0, 0, 0);
			base.AddElementFieldInfo("ActionEntity", "ComputerLimitedType", typeof(System.Int32), false, false, false, false,  (int)ActionFieldIndex.ComputerLimitedType, 0, 0, 10);
			base.AddElementFieldInfo("ActionEntity", "InternalComputerLimitedList", typeof(System.String), false, false, false, false,  (int)ActionFieldIndex.InternalComputerLimitedList, 150, 0, 0);
			base.AddElementFieldInfo("ActionEntity", "StoreLimited", typeof(System.Boolean), false, false, false, false,  (int)ActionFieldIndex.StoreLimited, 0, 0, 0);
			base.AddElementFieldInfo("ActionEntity", "InternalStoreLimitedList", typeof(System.String), false, false, false, false,  (int)ActionFieldIndex.InternalStoreLimitedList, 150, 0, 0);
			base.AddElementFieldInfo("ActionEntity", "TriggerType", typeof(System.Int32), false, false, false, false,  (int)ActionFieldIndex.TriggerType, 0, 0, 10);
			base.AddElementFieldInfo("ActionEntity", "TriggerSettings", typeof(System.String), false, false, false, false,  (int)ActionFieldIndex.TriggerSettings, 2147483647, 0, 0);
			base.AddElementFieldInfo("ActionEntity", "TaskSummary", typeof(System.String), false, false, false, false,  (int)ActionFieldIndex.TaskSummary, 150, 0, 0);
			base.AddElementFieldInfo("ActionEntity", "InternalOwner", typeof(System.String), false, false, false, true,  (int)ActionFieldIndex.InternalOwner, 50, 0, 0);
		}
		/// <summary>Inits ActionFilterTriggerEntity's FieldInfo objects</summary>
		private void InitActionFilterTriggerEntityInfos()
		{
			base.AddElementFieldInfo("ActionFilterTriggerEntity", "ActionID", typeof(System.Int64), true, true, false, false,  (int)ActionFilterTriggerFieldIndex.ActionID, 0, 0, 19);
			base.AddElementFieldInfo("ActionFilterTriggerEntity", "FilterNodeID", typeof(System.Int64), false, false, false, false,  (int)ActionFilterTriggerFieldIndex.FilterNodeID, 0, 0, 19);
			base.AddElementFieldInfo("ActionFilterTriggerEntity", "Direction", typeof(System.Int32), false, false, false, false,  (int)ActionFilterTriggerFieldIndex.Direction, 0, 0, 10);
			base.AddElementFieldInfo("ActionFilterTriggerEntity", "ComputerLimitedType", typeof(System.Int32), false, false, false, false,  (int)ActionFilterTriggerFieldIndex.ComputerLimitedType, 0, 0, 10);
			base.AddElementFieldInfo("ActionFilterTriggerEntity", "InternalComputerLimitedList", typeof(System.String), false, false, false, false,  (int)ActionFilterTriggerFieldIndex.InternalComputerLimitedList, 150, 0, 0);
		}
		/// <summary>Inits ActionQueueEntity's FieldInfo objects</summary>
		private void InitActionQueueEntityInfos()
		{
			base.AddElementFieldInfo("ActionQueueEntity", "ActionQueueID", typeof(System.Int64), true, false, true, false,  (int)ActionQueueFieldIndex.ActionQueueID, 0, 0, 19);
			base.AddElementFieldInfo("ActionQueueEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ActionQueueFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("ActionQueueEntity", "ActionID", typeof(System.Int64), false, true, false, false,  (int)ActionQueueFieldIndex.ActionID, 0, 0, 19);
			base.AddElementFieldInfo("ActionQueueEntity", "ActionName", typeof(System.String), false, false, false, false,  (int)ActionQueueFieldIndex.ActionName, 50, 0, 0);
			base.AddElementFieldInfo("ActionQueueEntity", "ActionQueueType", typeof(System.Int32), false, false, false, false,  (int)ActionQueueFieldIndex.ActionQueueType, 0, 0, 10);
			base.AddElementFieldInfo("ActionQueueEntity", "ActionVersion", typeof(System.Byte[]), false, false, false, false,  (int)ActionQueueFieldIndex.ActionVersion, 8, 0, 0);
			base.AddElementFieldInfo("ActionQueueEntity", "QueueVersion", typeof(System.Byte[]), false, false, true, false,  (int)ActionQueueFieldIndex.QueueVersion, 8, 0, 0);
			base.AddElementFieldInfo("ActionQueueEntity", "TriggerDate", typeof(System.DateTime), false, false, false, false,  (int)ActionQueueFieldIndex.TriggerDate, 0, 0, 0);
			base.AddElementFieldInfo("ActionQueueEntity", "TriggerComputerID", typeof(System.Int64), false, true, false, false,  (int)ActionQueueFieldIndex.TriggerComputerID, 0, 0, 19);
			base.AddElementFieldInfo("ActionQueueEntity", "InternalComputerLimitedList", typeof(System.String), false, false, false, false,  (int)ActionQueueFieldIndex.InternalComputerLimitedList, 150, 0, 0);
			base.AddElementFieldInfo("ActionQueueEntity", "ObjectID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)ActionQueueFieldIndex.ObjectID, 0, 0, 19);
			base.AddElementFieldInfo("ActionQueueEntity", "Status", typeof(System.Int32), false, false, false, false,  (int)ActionQueueFieldIndex.Status, 0, 0, 10);
			base.AddElementFieldInfo("ActionQueueEntity", "NextStep", typeof(System.Int32), false, false, false, false,  (int)ActionQueueFieldIndex.NextStep, 0, 0, 10);
			base.AddElementFieldInfo("ActionQueueEntity", "ContextLock", typeof(System.String), false, false, false, true,  (int)ActionQueueFieldIndex.ContextLock, 36, 0, 0);
		}
		/// <summary>Inits ActionQueueSelectionEntity's FieldInfo objects</summary>
		private void InitActionQueueSelectionEntityInfos()
		{
			base.AddElementFieldInfo("ActionQueueSelectionEntity", "ActionQueueSelectionID", typeof(System.Int64), true, false, true, false,  (int)ActionQueueSelectionFieldIndex.ActionQueueSelectionID, 0, 0, 19);
			base.AddElementFieldInfo("ActionQueueSelectionEntity", "ActionQueueID", typeof(System.Int64), false, true, false, false,  (int)ActionQueueSelectionFieldIndex.ActionQueueID, 0, 0, 19);
			base.AddElementFieldInfo("ActionQueueSelectionEntity", "ObjectID", typeof(System.Int64), false, false, false, false,  (int)ActionQueueSelectionFieldIndex.ObjectID, 0, 0, 19);
		}
		/// <summary>Inits ActionQueueStepEntity's FieldInfo objects</summary>
		private void InitActionQueueStepEntityInfos()
		{
			base.AddElementFieldInfo("ActionQueueStepEntity", "ActionQueueStepID", typeof(System.Int64), true, false, true, false,  (int)ActionQueueStepFieldIndex.ActionQueueStepID, 0, 0, 19);
			base.AddElementFieldInfo("ActionQueueStepEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ActionQueueStepFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("ActionQueueStepEntity", "ActionQueueID", typeof(System.Int64), false, true, false, false,  (int)ActionQueueStepFieldIndex.ActionQueueID, 0, 0, 19);
			base.AddElementFieldInfo("ActionQueueStepEntity", "StepStatus", typeof(System.Int32), false, false, false, false,  (int)ActionQueueStepFieldIndex.StepStatus, 0, 0, 10);
			base.AddElementFieldInfo("ActionQueueStepEntity", "StepIndex", typeof(System.Int32), false, false, false, false,  (int)ActionQueueStepFieldIndex.StepIndex, 0, 0, 10);
			base.AddElementFieldInfo("ActionQueueStepEntity", "StepName", typeof(System.String), false, false, false, false,  (int)ActionQueueStepFieldIndex.StepName, 100, 0, 0);
			base.AddElementFieldInfo("ActionQueueStepEntity", "TaskIdentifier", typeof(System.String), false, false, false, false,  (int)ActionQueueStepFieldIndex.TaskIdentifier, 50, 0, 0);
			base.AddElementFieldInfo("ActionQueueStepEntity", "TaskSettings", typeof(System.String), false, false, false, false,  (int)ActionQueueStepFieldIndex.TaskSettings, 2147483647, 0, 0);
			base.AddElementFieldInfo("ActionQueueStepEntity", "InputSource", typeof(System.Int32), false, false, false, false,  (int)ActionQueueStepFieldIndex.InputSource, 0, 0, 10);
			base.AddElementFieldInfo("ActionQueueStepEntity", "InputFilterNodeID", typeof(System.Int64), false, false, false, false,  (int)ActionQueueStepFieldIndex.InputFilterNodeID, 0, 0, 19);
			base.AddElementFieldInfo("ActionQueueStepEntity", "FilterCondition", typeof(System.Boolean), false, false, false, false,  (int)ActionQueueStepFieldIndex.FilterCondition, 0, 0, 0);
			base.AddElementFieldInfo("ActionQueueStepEntity", "FilterConditionNodeID", typeof(System.Int64), false, false, false, false,  (int)ActionQueueStepFieldIndex.FilterConditionNodeID, 0, 0, 19);
			base.AddElementFieldInfo("ActionQueueStepEntity", "FlowSuccess", typeof(System.Int32), false, false, false, false,  (int)ActionQueueStepFieldIndex.FlowSuccess, 0, 0, 10);
			base.AddElementFieldInfo("ActionQueueStepEntity", "FlowSkipped", typeof(System.Int32), false, false, false, false,  (int)ActionQueueStepFieldIndex.FlowSkipped, 0, 0, 10);
			base.AddElementFieldInfo("ActionQueueStepEntity", "FlowError", typeof(System.Int32), false, false, false, false,  (int)ActionQueueStepFieldIndex.FlowError, 0, 0, 10);
			base.AddElementFieldInfo("ActionQueueStepEntity", "AttemptDate", typeof(System.DateTime), false, false, false, false,  (int)ActionQueueStepFieldIndex.AttemptDate, 0, 0, 0);
			base.AddElementFieldInfo("ActionQueueStepEntity", "AttemptError", typeof(System.String), false, false, false, false,  (int)ActionQueueStepFieldIndex.AttemptError, 500, 0, 0);
			base.AddElementFieldInfo("ActionQueueStepEntity", "AttemptCount", typeof(System.Int32), false, false, false, false,  (int)ActionQueueStepFieldIndex.AttemptCount, 0, 0, 10);
		}
		/// <summary>Inits ActionTaskEntity's FieldInfo objects</summary>
		private void InitActionTaskEntityInfos()
		{
			base.AddElementFieldInfo("ActionTaskEntity", "ActionTaskID", typeof(System.Int64), true, false, true, false,  (int)ActionTaskFieldIndex.ActionTaskID, 0, 0, 19);
			base.AddElementFieldInfo("ActionTaskEntity", "ActionID", typeof(System.Int64), false, true, false, false,  (int)ActionTaskFieldIndex.ActionID, 0, 0, 19);
			base.AddElementFieldInfo("ActionTaskEntity", "TaskIdentifier", typeof(System.String), false, false, false, false,  (int)ActionTaskFieldIndex.TaskIdentifier, 50, 0, 0);
			base.AddElementFieldInfo("ActionTaskEntity", "TaskSettings", typeof(System.String), false, false, false, false,  (int)ActionTaskFieldIndex.TaskSettings, 2147483647, 0, 0);
			base.AddElementFieldInfo("ActionTaskEntity", "StepIndex", typeof(System.Int32), false, false, false, false,  (int)ActionTaskFieldIndex.StepIndex, 0, 0, 10);
			base.AddElementFieldInfo("ActionTaskEntity", "InputSource", typeof(System.Int32), false, false, false, false,  (int)ActionTaskFieldIndex.InputSource, 0, 0, 10);
			base.AddElementFieldInfo("ActionTaskEntity", "InputFilterNodeID", typeof(System.Int64), false, false, false, false,  (int)ActionTaskFieldIndex.InputFilterNodeID, 0, 0, 19);
			base.AddElementFieldInfo("ActionTaskEntity", "FilterCondition", typeof(System.Boolean), false, false, false, false,  (int)ActionTaskFieldIndex.FilterCondition, 0, 0, 0);
			base.AddElementFieldInfo("ActionTaskEntity", "FilterConditionNodeID", typeof(System.Int64), false, false, false, false,  (int)ActionTaskFieldIndex.FilterConditionNodeID, 0, 0, 19);
			base.AddElementFieldInfo("ActionTaskEntity", "FlowSuccess", typeof(System.Int32), false, false, false, false,  (int)ActionTaskFieldIndex.FlowSuccess, 0, 0, 10);
			base.AddElementFieldInfo("ActionTaskEntity", "FlowSkipped", typeof(System.Int32), false, false, false, false,  (int)ActionTaskFieldIndex.FlowSkipped, 0, 0, 10);
			base.AddElementFieldInfo("ActionTaskEntity", "FlowError", typeof(System.Int32), false, false, false, false,  (int)ActionTaskFieldIndex.FlowError, 0, 0, 10);
		}
		/// <summary>Inits AmazonASINEntity's FieldInfo objects</summary>
		private void InitAmazonASINEntityInfos()
		{
			base.AddElementFieldInfo("AmazonASINEntity", "StoreID", typeof(System.Int64), true, true, false, false,  (int)AmazonASINFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("AmazonASINEntity", "SKU", typeof(System.String), true, false, false, false,  (int)AmazonASINFieldIndex.SKU, 100, 0, 0);
			base.AddElementFieldInfo("AmazonASINEntity", "AmazonASIN", typeof(System.String), false, false, false, false,  (int)AmazonASINFieldIndex.AmazonASIN, 32, 0, 0);
		}
		/// <summary>Inits AmazonOrderEntity's FieldInfo objects</summary>
		private void InitAmazonOrderEntityInfos()
		{
			base.AddElementFieldInfo("AmazonOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)AmazonOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("AmazonOrderEntity", "AmazonOrderID", typeof(System.String), false, false, false, false,  (int)AmazonOrderFieldIndex.AmazonOrderID, 32, 0, 0);
			base.AddElementFieldInfo("AmazonOrderEntity", "AmazonCommission", typeof(System.Decimal), false, false, false, false,  (int)AmazonOrderFieldIndex.AmazonCommission, 0, 4, 19);
			base.AddElementFieldInfo("AmazonOrderEntity", "FulfillmentChannel", typeof(System.Int32), false, false, false, false,  (int)AmazonOrderFieldIndex.FulfillmentChannel, 0, 0, 10);
			base.AddElementFieldInfo("AmazonOrderEntity", "IsPrime", typeof(System.Int32), false, false, false, false,  (int)AmazonOrderFieldIndex.IsPrime, 0, 0, 10);
			base.AddElementFieldInfo("AmazonOrderEntity", "EarliestExpectedDeliveryDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)AmazonOrderFieldIndex.EarliestExpectedDeliveryDate, 0, 0, 0);
			base.AddElementFieldInfo("AmazonOrderEntity", "LatestExpectedDeliveryDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)AmazonOrderFieldIndex.LatestExpectedDeliveryDate, 0, 0, 0);
		}
		/// <summary>Inits AmazonOrderItemEntity's FieldInfo objects</summary>
		private void InitAmazonOrderItemEntityInfos()
		{
			base.AddElementFieldInfo("AmazonOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, false, false,  (int)AmazonOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			base.AddElementFieldInfo("AmazonOrderItemEntity", "AmazonOrderItemCode", typeof(System.String), false, false, false, false,  (int)AmazonOrderItemFieldIndex.AmazonOrderItemCode, 64, 0, 0);
			base.AddElementFieldInfo("AmazonOrderItemEntity", "ASIN", typeof(System.String), false, false, false, false,  (int)AmazonOrderItemFieldIndex.ASIN, 255, 0, 0);
			base.AddElementFieldInfo("AmazonOrderItemEntity", "ConditionNote", typeof(System.String), false, false, false, false,  (int)AmazonOrderItemFieldIndex.ConditionNote, 255, 0, 0);
		}
		/// <summary>Inits AmazonProfileEntity's FieldInfo objects</summary>
		private void InitAmazonProfileEntityInfos()
		{
			base.AddElementFieldInfo("AmazonProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)AmazonProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			base.AddElementFieldInfo("AmazonProfileEntity", "DimsProfileID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)AmazonProfileFieldIndex.DimsProfileID, 0, 0, 19);
			base.AddElementFieldInfo("AmazonProfileEntity", "DimsLength", typeof(Nullable<System.Double>), false, false, false, true,  (int)AmazonProfileFieldIndex.DimsLength, 0, 0, 38);
			base.AddElementFieldInfo("AmazonProfileEntity", "DimsWidth", typeof(Nullable<System.Double>), false, false, false, true,  (int)AmazonProfileFieldIndex.DimsWidth, 0, 0, 38);
			base.AddElementFieldInfo("AmazonProfileEntity", "DimsHeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)AmazonProfileFieldIndex.DimsHeight, 0, 0, 38);
			base.AddElementFieldInfo("AmazonProfileEntity", "DimsWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)AmazonProfileFieldIndex.DimsWeight, 0, 0, 38);
			base.AddElementFieldInfo("AmazonProfileEntity", "DimsAddWeight", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)AmazonProfileFieldIndex.DimsAddWeight, 0, 0, 0);
			base.AddElementFieldInfo("AmazonProfileEntity", "DeliveryExperience", typeof(Nullable<System.Int32>), false, false, false, true,  (int)AmazonProfileFieldIndex.DeliveryExperience, 0, 0, 10);
			base.AddElementFieldInfo("AmazonProfileEntity", "Weight", typeof(Nullable<System.Double>), false, false, false, true,  (int)AmazonProfileFieldIndex.Weight, 0, 0, 38);
		}
		/// <summary>Inits AmazonShipmentEntity's FieldInfo objects</summary>
		private void InitAmazonShipmentEntityInfos()
		{
			base.AddElementFieldInfo("AmazonShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)AmazonShipmentFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("AmazonShipmentEntity", "CarrierName", typeof(System.String), false, false, false, false,  (int)AmazonShipmentFieldIndex.CarrierName, 50, 0, 0);
			base.AddElementFieldInfo("AmazonShipmentEntity", "ShippingServiceName", typeof(System.String), false, false, false, false,  (int)AmazonShipmentFieldIndex.ShippingServiceName, 50, 0, 0);
			base.AddElementFieldInfo("AmazonShipmentEntity", "ShippingServiceID", typeof(System.String), false, false, false, false,  (int)AmazonShipmentFieldIndex.ShippingServiceID, 50, 0, 0);
			base.AddElementFieldInfo("AmazonShipmentEntity", "ShippingServiceOfferID", typeof(System.String), false, false, false, false,  (int)AmazonShipmentFieldIndex.ShippingServiceOfferID, 50, 0, 0);
			base.AddElementFieldInfo("AmazonShipmentEntity", "InsuranceValue", typeof(System.Decimal), false, false, false, false,  (int)AmazonShipmentFieldIndex.InsuranceValue, 0, 4, 19);
			base.AddElementFieldInfo("AmazonShipmentEntity", "DimsProfileID", typeof(System.Int64), false, false, false, false,  (int)AmazonShipmentFieldIndex.DimsProfileID, 0, 0, 19);
			base.AddElementFieldInfo("AmazonShipmentEntity", "DimsLength", typeof(System.Double), false, false, false, false,  (int)AmazonShipmentFieldIndex.DimsLength, 0, 0, 38);
			base.AddElementFieldInfo("AmazonShipmentEntity", "DimsWidth", typeof(System.Double), false, false, false, false,  (int)AmazonShipmentFieldIndex.DimsWidth, 0, 0, 38);
			base.AddElementFieldInfo("AmazonShipmentEntity", "DimsHeight", typeof(System.Double), false, false, false, false,  (int)AmazonShipmentFieldIndex.DimsHeight, 0, 0, 38);
			base.AddElementFieldInfo("AmazonShipmentEntity", "DimsWeight", typeof(System.Double), false, false, false, false,  (int)AmazonShipmentFieldIndex.DimsWeight, 0, 0, 38);
			base.AddElementFieldInfo("AmazonShipmentEntity", "DimsAddWeight", typeof(System.Boolean), false, false, false, false,  (int)AmazonShipmentFieldIndex.DimsAddWeight, 0, 0, 0);
			base.AddElementFieldInfo("AmazonShipmentEntity", "DeliveryExperience", typeof(System.Int32), false, false, false, false,  (int)AmazonShipmentFieldIndex.DeliveryExperience, 0, 0, 10);
			base.AddElementFieldInfo("AmazonShipmentEntity", "DeclaredValue", typeof(Nullable<System.Decimal>), false, false, false, true,  (int)AmazonShipmentFieldIndex.DeclaredValue, 0, 4, 19);
			base.AddElementFieldInfo("AmazonShipmentEntity", "AmazonUniqueShipmentID", typeof(System.String), false, false, false, true,  (int)AmazonShipmentFieldIndex.AmazonUniqueShipmentID, 50, 0, 0);
		}
		/// <summary>Inits AmazonStoreEntity's FieldInfo objects</summary>
		private void InitAmazonStoreEntityInfos()
		{
			base.AddElementFieldInfo("AmazonStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)AmazonStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("AmazonStoreEntity", "AmazonApi", typeof(System.Int32), false, false, false, false,  (int)AmazonStoreFieldIndex.AmazonApi, 0, 0, 10);
			base.AddElementFieldInfo("AmazonStoreEntity", "AmazonApiRegion", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.AmazonApiRegion, 2, 0, 0);
			base.AddElementFieldInfo("AmazonStoreEntity", "SellerCentralUsername", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.SellerCentralUsername, 50, 0, 0);
			base.AddElementFieldInfo("AmazonStoreEntity", "SellerCentralPassword", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.SellerCentralPassword, 50, 0, 0);
			base.AddElementFieldInfo("AmazonStoreEntity", "MerchantName", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.MerchantName, 64, 0, 0);
			base.AddElementFieldInfo("AmazonStoreEntity", "MerchantToken", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.MerchantToken, 32, 0, 0);
			base.AddElementFieldInfo("AmazonStoreEntity", "AccessKeyID", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.AccessKeyID, 32, 0, 0);
			base.AddElementFieldInfo("AmazonStoreEntity", "AuthToken", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.AuthToken, 100, 0, 0);
			base.AddElementFieldInfo("AmazonStoreEntity", "Cookie", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.Cookie, 2147483647, 0, 0);
			base.AddElementFieldInfo("AmazonStoreEntity", "CookieExpires", typeof(System.DateTime), false, false, false, false,  (int)AmazonStoreFieldIndex.CookieExpires, 0, 0, 0);
			base.AddElementFieldInfo("AmazonStoreEntity", "CookieWaitUntil", typeof(System.DateTime), false, false, false, false,  (int)AmazonStoreFieldIndex.CookieWaitUntil, 0, 0, 0);
			base.AddElementFieldInfo("AmazonStoreEntity", "Certificate", typeof(System.Byte[]), false, false, false, true,  (int)AmazonStoreFieldIndex.Certificate, 2048, 0, 0);
			base.AddElementFieldInfo("AmazonStoreEntity", "WeightDownloads", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.WeightDownloads, 2147483647, 0, 0);
			base.AddElementFieldInfo("AmazonStoreEntity", "MerchantID", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.MerchantID, 50, 0, 0);
			base.AddElementFieldInfo("AmazonStoreEntity", "MarketplaceID", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.MarketplaceID, 50, 0, 0);
			base.AddElementFieldInfo("AmazonStoreEntity", "ExcludeFBA", typeof(System.Boolean), false, false, false, false,  (int)AmazonStoreFieldIndex.ExcludeFBA, 0, 0, 0);
			base.AddElementFieldInfo("AmazonStoreEntity", "DomainName", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.DomainName, 50, 0, 0);
			base.AddElementFieldInfo("AmazonStoreEntity", "AmazonShippingToken", typeof(System.String), false, false, false, false,  (int)AmazonStoreFieldIndex.AmazonShippingToken, 500, 0, 0);
		}
		/// <summary>Inits AmeriCommerceStoreEntity's FieldInfo objects</summary>
		private void InitAmeriCommerceStoreEntityInfos()
		{
			base.AddElementFieldInfo("AmeriCommerceStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)AmeriCommerceStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("AmeriCommerceStoreEntity", "Username", typeof(System.String), false, false, false, false,  (int)AmeriCommerceStoreFieldIndex.Username, 70, 0, 0);
			base.AddElementFieldInfo("AmeriCommerceStoreEntity", "Password", typeof(System.String), false, false, false, false,  (int)AmeriCommerceStoreFieldIndex.Password, 70, 0, 0);
			base.AddElementFieldInfo("AmeriCommerceStoreEntity", "StoreUrl", typeof(System.String), false, false, false, false,  (int)AmeriCommerceStoreFieldIndex.StoreUrl, 350, 0, 0);
			base.AddElementFieldInfo("AmeriCommerceStoreEntity", "StoreCode", typeof(System.Int32), false, false, false, false,  (int)AmeriCommerceStoreFieldIndex.StoreCode, 0, 0, 10);
			base.AddElementFieldInfo("AmeriCommerceStoreEntity", "StatusCodes", typeof(System.String), false, false, false, false,  (int)AmeriCommerceStoreFieldIndex.StatusCodes, 2147483647, 0, 0);
		}
		/// <summary>Inits AuditEntity's FieldInfo objects</summary>
		private void InitAuditEntityInfos()
		{
			base.AddElementFieldInfo("AuditEntity", "AuditID", typeof(System.Int64), true, false, true, false,  (int)AuditFieldIndex.AuditID, 0, 0, 19);
			base.AddElementFieldInfo("AuditEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)AuditFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("AuditEntity", "TransactionID", typeof(System.Int64), false, false, false, false,  (int)AuditFieldIndex.TransactionID, 0, 0, 19);
			base.AddElementFieldInfo("AuditEntity", "UserID", typeof(System.Int64), false, true, false, false,  (int)AuditFieldIndex.UserID, 0, 0, 19);
			base.AddElementFieldInfo("AuditEntity", "ComputerID", typeof(System.Int64), false, true, false, false,  (int)AuditFieldIndex.ComputerID, 0, 0, 19);
			base.AddElementFieldInfo("AuditEntity", "Reason", typeof(System.Int32), false, false, false, false,  (int)AuditFieldIndex.Reason, 0, 0, 10);
			base.AddElementFieldInfo("AuditEntity", "ReasonDetail", typeof(System.String), false, false, false, true,  (int)AuditFieldIndex.ReasonDetail, 100, 0, 0);
			base.AddElementFieldInfo("AuditEntity", "Date", typeof(System.DateTime), false, false, false, false,  (int)AuditFieldIndex.Date, 0, 0, 0);
			base.AddElementFieldInfo("AuditEntity", "Action", typeof(System.Int32), false, false, false, false,  (int)AuditFieldIndex.Action, 0, 0, 10);
			base.AddElementFieldInfo("AuditEntity", "ObjectID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)AuditFieldIndex.ObjectID, 0, 0, 19);
			base.AddElementFieldInfo("AuditEntity", "HasEvents", typeof(System.Boolean), false, false, false, false,  (int)AuditFieldIndex.HasEvents, 0, 0, 0);
		}
		/// <summary>Inits AuditChangeEntity's FieldInfo objects</summary>
		private void InitAuditChangeEntityInfos()
		{
			base.AddElementFieldInfo("AuditChangeEntity", "AuditChangeID", typeof(System.Int64), true, false, true, false,  (int)AuditChangeFieldIndex.AuditChangeID, 0, 0, 19);
			base.AddElementFieldInfo("AuditChangeEntity", "AuditID", typeof(System.Int64), false, true, false, false,  (int)AuditChangeFieldIndex.AuditID, 0, 0, 19);
			base.AddElementFieldInfo("AuditChangeEntity", "ChangeType", typeof(System.Int32), false, false, false, false,  (int)AuditChangeFieldIndex.ChangeType, 0, 0, 10);
			base.AddElementFieldInfo("AuditChangeEntity", "ObjectID", typeof(System.Int64), false, false, false, false,  (int)AuditChangeFieldIndex.ObjectID, 0, 0, 19);
		}
		/// <summary>Inits AuditChangeDetailEntity's FieldInfo objects</summary>
		private void InitAuditChangeDetailEntityInfos()
		{
			base.AddElementFieldInfo("AuditChangeDetailEntity", "AuditChangeDetailID", typeof(System.Int64), true, false, true, false,  (int)AuditChangeDetailFieldIndex.AuditChangeDetailID, 0, 0, 19);
			base.AddElementFieldInfo("AuditChangeDetailEntity", "AuditChangeID", typeof(System.Int64), false, true, false, false,  (int)AuditChangeDetailFieldIndex.AuditChangeID, 0, 0, 19);
			base.AddElementFieldInfo("AuditChangeDetailEntity", "AuditID", typeof(System.Int64), false, false, false, false,  (int)AuditChangeDetailFieldIndex.AuditID, 0, 0, 19);
			base.AddElementFieldInfo("AuditChangeDetailEntity", "DisplayName", typeof(System.String), false, false, false, false,  (int)AuditChangeDetailFieldIndex.DisplayName, 50, 0, 0);
			base.AddElementFieldInfo("AuditChangeDetailEntity", "DisplayFormat", typeof(System.Byte), false, false, false, false,  (int)AuditChangeDetailFieldIndex.DisplayFormat, 0, 0, 3);
			base.AddElementFieldInfo("AuditChangeDetailEntity", "DataType", typeof(System.Byte), false, false, false, false,  (int)AuditChangeDetailFieldIndex.DataType, 0, 0, 3);
			base.AddElementFieldInfo("AuditChangeDetailEntity", "TextOld", typeof(System.String), false, false, false, true,  (int)AuditChangeDetailFieldIndex.TextOld, 2147483647, 0, 0);
			base.AddElementFieldInfo("AuditChangeDetailEntity", "TextNew", typeof(System.String), false, false, false, true,  (int)AuditChangeDetailFieldIndex.TextNew, 2147483647, 0, 0);
			base.AddElementFieldInfo("AuditChangeDetailEntity", "VariantOld", typeof(System.Object), false, false, false, true,  (int)AuditChangeDetailFieldIndex.VariantOld, 0, 0, 0);
			base.AddElementFieldInfo("AuditChangeDetailEntity", "VariantNew", typeof(System.Object), false, false, false, true,  (int)AuditChangeDetailFieldIndex.VariantNew, 0, 0, 0);
		}
		/// <summary>Inits BestRateProfileEntity's FieldInfo objects</summary>
		private void InitBestRateProfileEntityInfos()
		{
			base.AddElementFieldInfo("BestRateProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)BestRateProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			base.AddElementFieldInfo("BestRateProfileEntity", "DimsProfileID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)BestRateProfileFieldIndex.DimsProfileID, 0, 0, 19);
			base.AddElementFieldInfo("BestRateProfileEntity", "DimsLength", typeof(Nullable<System.Double>), false, false, false, true,  (int)BestRateProfileFieldIndex.DimsLength, 0, 0, 38);
			base.AddElementFieldInfo("BestRateProfileEntity", "DimsWidth", typeof(Nullable<System.Double>), false, false, false, true,  (int)BestRateProfileFieldIndex.DimsWidth, 0, 0, 38);
			base.AddElementFieldInfo("BestRateProfileEntity", "DimsHeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)BestRateProfileFieldIndex.DimsHeight, 0, 0, 38);
			base.AddElementFieldInfo("BestRateProfileEntity", "DimsWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)BestRateProfileFieldIndex.DimsWeight, 0, 0, 38);
			base.AddElementFieldInfo("BestRateProfileEntity", "DimsAddWeight", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)BestRateProfileFieldIndex.DimsAddWeight, 0, 0, 0);
			base.AddElementFieldInfo("BestRateProfileEntity", "Weight", typeof(Nullable<System.Double>), false, false, false, true,  (int)BestRateProfileFieldIndex.Weight, 0, 0, 38);
			base.AddElementFieldInfo("BestRateProfileEntity", "ServiceLevel", typeof(Nullable<System.Int32>), false, false, false, true,  (int)BestRateProfileFieldIndex.ServiceLevel, 0, 0, 10);
		}
		/// <summary>Inits BestRateShipmentEntity's FieldInfo objects</summary>
		private void InitBestRateShipmentEntityInfos()
		{
			base.AddElementFieldInfo("BestRateShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)BestRateShipmentFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("BestRateShipmentEntity", "DimsProfileID", typeof(System.Int64), false, false, false, false,  (int)BestRateShipmentFieldIndex.DimsProfileID, 0, 0, 19);
			base.AddElementFieldInfo("BestRateShipmentEntity", "DimsLength", typeof(System.Double), false, false, false, false,  (int)BestRateShipmentFieldIndex.DimsLength, 0, 0, 38);
			base.AddElementFieldInfo("BestRateShipmentEntity", "DimsWidth", typeof(System.Double), false, false, false, false,  (int)BestRateShipmentFieldIndex.DimsWidth, 0, 0, 38);
			base.AddElementFieldInfo("BestRateShipmentEntity", "DimsHeight", typeof(System.Double), false, false, false, false,  (int)BestRateShipmentFieldIndex.DimsHeight, 0, 0, 38);
			base.AddElementFieldInfo("BestRateShipmentEntity", "DimsWeight", typeof(System.Double), false, false, false, false,  (int)BestRateShipmentFieldIndex.DimsWeight, 0, 0, 38);
			base.AddElementFieldInfo("BestRateShipmentEntity", "DimsAddWeight", typeof(System.Boolean), false, false, false, false,  (int)BestRateShipmentFieldIndex.DimsAddWeight, 0, 0, 0);
			base.AddElementFieldInfo("BestRateShipmentEntity", "ServiceLevel", typeof(System.Int32), false, false, false, false,  (int)BestRateShipmentFieldIndex.ServiceLevel, 0, 0, 10);
			base.AddElementFieldInfo("BestRateShipmentEntity", "InsuranceValue", typeof(System.Decimal), false, false, false, false,  (int)BestRateShipmentFieldIndex.InsuranceValue, 0, 4, 19);
			base.AddElementFieldInfo("BestRateShipmentEntity", "RequestedLabelFormat", typeof(System.Int32), false, false, false, false,  (int)BestRateShipmentFieldIndex.RequestedLabelFormat, 0, 0, 10);
		}
		/// <summary>Inits BigCommerceOrderItemEntity's FieldInfo objects</summary>
		private void InitBigCommerceOrderItemEntityInfos()
		{
			base.AddElementFieldInfo("BigCommerceOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, false, false,  (int)BigCommerceOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			base.AddElementFieldInfo("BigCommerceOrderItemEntity", "OrderAddressID", typeof(System.Int64), false, false, false, false,  (int)BigCommerceOrderItemFieldIndex.OrderAddressID, 0, 0, 19);
			base.AddElementFieldInfo("BigCommerceOrderItemEntity", "OrderProductID", typeof(System.Int64), false, false, false, false,  (int)BigCommerceOrderItemFieldIndex.OrderProductID, 0, 0, 19);
			base.AddElementFieldInfo("BigCommerceOrderItemEntity", "IsDigitalItem", typeof(System.Boolean), false, false, false, false,  (int)BigCommerceOrderItemFieldIndex.IsDigitalItem, 0, 0, 0);
			base.AddElementFieldInfo("BigCommerceOrderItemEntity", "EventDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)BigCommerceOrderItemFieldIndex.EventDate, 0, 0, 0);
			base.AddElementFieldInfo("BigCommerceOrderItemEntity", "EventName", typeof(System.String), false, false, false, true,  (int)BigCommerceOrderItemFieldIndex.EventName, 255, 0, 0);
		}
		/// <summary>Inits BigCommerceStoreEntity's FieldInfo objects</summary>
		private void InitBigCommerceStoreEntityInfos()
		{
			base.AddElementFieldInfo("BigCommerceStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)BigCommerceStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("BigCommerceStoreEntity", "ApiUrl", typeof(System.String), false, false, false, false,  (int)BigCommerceStoreFieldIndex.ApiUrl, 110, 0, 0);
			base.AddElementFieldInfo("BigCommerceStoreEntity", "ApiUserName", typeof(System.String), false, false, false, false,  (int)BigCommerceStoreFieldIndex.ApiUserName, 65, 0, 0);
			base.AddElementFieldInfo("BigCommerceStoreEntity", "ApiToken", typeof(System.String), false, false, false, true,  (int)BigCommerceStoreFieldIndex.ApiToken, 100, 0, 0);
			base.AddElementFieldInfo("BigCommerceStoreEntity", "StatusCodes", typeof(System.String), false, false, false, true,  (int)BigCommerceStoreFieldIndex.StatusCodes, 2147483647, 0, 0);
			base.AddElementFieldInfo("BigCommerceStoreEntity", "WeightUnitOfMeasure", typeof(System.Int32), false, false, false, false,  (int)BigCommerceStoreFieldIndex.WeightUnitOfMeasure, 0, 0, 10);
			base.AddElementFieldInfo("BigCommerceStoreEntity", "DownloadModifiedNumberOfDaysBack", typeof(System.Int32), false, false, false, false,  (int)BigCommerceStoreFieldIndex.DownloadModifiedNumberOfDaysBack, 0, 0, 10);
		}
		/// <summary>Inits BuyDotComOrderItemEntity's FieldInfo objects</summary>
		private void InitBuyDotComOrderItemEntityInfos()
		{
			base.AddElementFieldInfo("BuyDotComOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, false, false,  (int)BuyDotComOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			base.AddElementFieldInfo("BuyDotComOrderItemEntity", "ReceiptItemID", typeof(System.Int64), false, false, false, false,  (int)BuyDotComOrderItemFieldIndex.ReceiptItemID, 0, 0, 19);
			base.AddElementFieldInfo("BuyDotComOrderItemEntity", "ListingID", typeof(System.Int32), false, false, false, false,  (int)BuyDotComOrderItemFieldIndex.ListingID, 0, 0, 10);
			base.AddElementFieldInfo("BuyDotComOrderItemEntity", "Shipping", typeof(System.Decimal), false, false, false, false,  (int)BuyDotComOrderItemFieldIndex.Shipping, 0, 4, 19);
			base.AddElementFieldInfo("BuyDotComOrderItemEntity", "Tax", typeof(System.Decimal), false, false, false, false,  (int)BuyDotComOrderItemFieldIndex.Tax, 0, 4, 19);
			base.AddElementFieldInfo("BuyDotComOrderItemEntity", "Commission", typeof(System.Decimal), false, false, false, false,  (int)BuyDotComOrderItemFieldIndex.Commission, 0, 4, 19);
			base.AddElementFieldInfo("BuyDotComOrderItemEntity", "ItemFee", typeof(System.Decimal), false, false, false, false,  (int)BuyDotComOrderItemFieldIndex.ItemFee, 0, 4, 19);
		}
		/// <summary>Inits BuyDotComStoreEntity's FieldInfo objects</summary>
		private void InitBuyDotComStoreEntityInfos()
		{
			base.AddElementFieldInfo("BuyDotComStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)BuyDotComStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("BuyDotComStoreEntity", "FtpUsername", typeof(System.String), false, false, false, false,  (int)BuyDotComStoreFieldIndex.FtpUsername, 50, 0, 0);
			base.AddElementFieldInfo("BuyDotComStoreEntity", "FtpPassword", typeof(System.String), false, false, false, false,  (int)BuyDotComStoreFieldIndex.FtpPassword, 50, 0, 0);
		}
		/// <summary>Inits ChannelAdvisorOrderEntity's FieldInfo objects</summary>
		private void InitChannelAdvisorOrderEntityInfos()
		{
			base.AddElementFieldInfo("ChannelAdvisorOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("ChannelAdvisorOrderEntity", "CustomOrderIdentifier", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.CustomOrderIdentifier, 50, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorOrderEntity", "ResellerID", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.ResellerID, 80, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorOrderEntity", "OnlineShippingStatus", typeof(System.Int32), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.OnlineShippingStatus, 0, 0, 10);
			base.AddElementFieldInfo("ChannelAdvisorOrderEntity", "OnlineCheckoutStatus", typeof(System.Int32), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.OnlineCheckoutStatus, 0, 0, 10);
			base.AddElementFieldInfo("ChannelAdvisorOrderEntity", "OnlinePaymentStatus", typeof(System.Int32), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.OnlinePaymentStatus, 0, 0, 10);
			base.AddElementFieldInfo("ChannelAdvisorOrderEntity", "FlagStyle", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.FlagStyle, 32, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorOrderEntity", "FlagDescription", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.FlagDescription, 80, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorOrderEntity", "FlagType", typeof(System.Int32), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.FlagType, 0, 0, 10);
			base.AddElementFieldInfo("ChannelAdvisorOrderEntity", "MarketplaceNames", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.MarketplaceNames, 1024, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorOrderEntity", "IsPrime", typeof(System.Int32), false, false, false, false,  (int)ChannelAdvisorOrderFieldIndex.IsPrime, 0, 0, 10);
		}
		/// <summary>Inits ChannelAdvisorOrderItemEntity's FieldInfo objects</summary>
		private void InitChannelAdvisorOrderItemEntityInfos()
		{
			base.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			base.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "MarketplaceName", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.MarketplaceName, 50, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "MarketplaceStoreName", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.MarketplaceStoreName, 100, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "MarketplaceBuyerID", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.MarketplaceBuyerID, 80, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "MarketplaceSalesID", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.MarketplaceSalesID, 50, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "Classification", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.Classification, 30, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "DistributionCenter", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.DistributionCenter, 80, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "HarmonizedCode", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.HarmonizedCode, 20, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "IsFBA", typeof(System.Boolean), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.IsFBA, 0, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorOrderItemEntity", "MPN", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorOrderItemFieldIndex.MPN, 50, 0, 0);
		}
		/// <summary>Inits ChannelAdvisorStoreEntity's FieldInfo objects</summary>
		private void InitChannelAdvisorStoreEntityInfos()
		{
			base.AddElementFieldInfo("ChannelAdvisorStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("ChannelAdvisorStoreEntity", "AccountKey", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.AccountKey, 50, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorStoreEntity", "ProfileID", typeof(System.Int32), false, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.ProfileID, 0, 0, 10);
			base.AddElementFieldInfo("ChannelAdvisorStoreEntity", "AttributesToDownload", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.AttributesToDownload, 2147483647, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorStoreEntity", "ConsolidatorAsUsps", typeof(System.Boolean), false, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.ConsolidatorAsUsps, 0, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorStoreEntity", "AmazonMerchantID", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.AmazonMerchantID, 50, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorStoreEntity", "AmazonAuthToken", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.AmazonAuthToken, 100, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorStoreEntity", "AmazonApiRegion", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.AmazonApiRegion, 2, 0, 0);
			base.AddElementFieldInfo("ChannelAdvisorStoreEntity", "AmazonShippingToken", typeof(System.String), false, false, false, false,  (int)ChannelAdvisorStoreFieldIndex.AmazonShippingToken, 500, 0, 0);
		}
		/// <summary>Inits ClickCartProOrderEntity's FieldInfo objects</summary>
		private void InitClickCartProOrderEntityInfos()
		{
			base.AddElementFieldInfo("ClickCartProOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)ClickCartProOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("ClickCartProOrderEntity", "ClickCartProOrderID", typeof(System.String), false, false, false, false,  (int)ClickCartProOrderFieldIndex.ClickCartProOrderID, 25, 0, 0);
		}
		/// <summary>Inits CommerceInterfaceOrderEntity's FieldInfo objects</summary>
		private void InitCommerceInterfaceOrderEntityInfos()
		{
			base.AddElementFieldInfo("CommerceInterfaceOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)CommerceInterfaceOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("CommerceInterfaceOrderEntity", "CommerceInterfaceOrderNumber", typeof(System.String), false, false, false, false,  (int)CommerceInterfaceOrderFieldIndex.CommerceInterfaceOrderNumber, 60, 0, 0);
		}
		/// <summary>Inits ComputerEntity's FieldInfo objects</summary>
		private void InitComputerEntityInfos()
		{
			base.AddElementFieldInfo("ComputerEntity", "ComputerID", typeof(System.Int64), true, false, true, false,  (int)ComputerFieldIndex.ComputerID, 0, 0, 19);
			base.AddElementFieldInfo("ComputerEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ComputerFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("ComputerEntity", "Identifier", typeof(System.Guid), false, false, false, false,  (int)ComputerFieldIndex.Identifier, 0, 0, 0);
			base.AddElementFieldInfo("ComputerEntity", "Name", typeof(System.String), false, false, false, false,  (int)ComputerFieldIndex.Name, 50, 0, 0);
		}
		/// <summary>Inits ConfigurationEntity's FieldInfo objects</summary>
		private void InitConfigurationEntityInfos()
		{
			base.AddElementFieldInfo("ConfigurationEntity", "ConfigurationID", typeof(System.Boolean), true, false, false, false,  (int)ConfigurationFieldIndex.ConfigurationID, 0, 0, 0);
			base.AddElementFieldInfo("ConfigurationEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ConfigurationFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("ConfigurationEntity", "LogOnMethod", typeof(System.Int32), false, false, false, false,  (int)ConfigurationFieldIndex.LogOnMethod, 0, 0, 10);
			base.AddElementFieldInfo("ConfigurationEntity", "AddressCasing", typeof(System.Boolean), false, false, false, false,  (int)ConfigurationFieldIndex.AddressCasing, 0, 0, 0);
			base.AddElementFieldInfo("ConfigurationEntity", "CustomerCompareEmail", typeof(System.Boolean), false, false, false, false,  (int)ConfigurationFieldIndex.CustomerCompareEmail, 0, 0, 0);
			base.AddElementFieldInfo("ConfigurationEntity", "CustomerCompareAddress", typeof(System.Boolean), false, false, false, false,  (int)ConfigurationFieldIndex.CustomerCompareAddress, 0, 0, 0);
			base.AddElementFieldInfo("ConfigurationEntity", "CustomerUpdateBilling", typeof(System.Boolean), false, false, false, false,  (int)ConfigurationFieldIndex.CustomerUpdateBilling, 0, 0, 0);
			base.AddElementFieldInfo("ConfigurationEntity", "CustomerUpdateShipping", typeof(System.Boolean), false, false, false, false,  (int)ConfigurationFieldIndex.CustomerUpdateShipping, 0, 0, 0);
			base.AddElementFieldInfo("ConfigurationEntity", "CustomerUpdateModifiedBilling", typeof(System.Int32), false, false, false, false,  (int)ConfigurationFieldIndex.CustomerUpdateModifiedBilling, 0, 0, 10);
			base.AddElementFieldInfo("ConfigurationEntity", "CustomerUpdateModifiedShipping", typeof(System.Int32), false, false, false, false,  (int)ConfigurationFieldIndex.CustomerUpdateModifiedShipping, 0, 0, 10);
			base.AddElementFieldInfo("ConfigurationEntity", "AuditNewOrders", typeof(System.Boolean), false, false, false, false,  (int)ConfigurationFieldIndex.AuditNewOrders, 0, 0, 0);
			base.AddElementFieldInfo("ConfigurationEntity", "AuditDeletedOrders", typeof(System.Boolean), false, false, false, false,  (int)ConfigurationFieldIndex.AuditDeletedOrders, 0, 0, 0);
		}
		/// <summary>Inits CustomerEntity's FieldInfo objects</summary>
		private void InitCustomerEntityInfos()
		{
			base.AddElementFieldInfo("CustomerEntity", "CustomerID", typeof(System.Int64), true, false, true, false,  (int)CustomerFieldIndex.CustomerID, 0, 0, 19);
			base.AddElementFieldInfo("CustomerEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)CustomerFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "BillFirstName", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillFirstName, 30, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "BillMiddleName", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillMiddleName, 30, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "BillLastName", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillLastName, 30, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "BillCompany", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillCompany, 60, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "BillStreet1", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillStreet1, 60, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "BillStreet2", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillStreet2, 60, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "BillStreet3", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillStreet3, 60, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "BillCity", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillCity, 50, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "BillStateProvCode", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillStateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "BillPostalCode", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillPostalCode, 20, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "BillCountryCode", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillCountryCode, 50, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "BillPhone", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillPhone, 25, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "BillFax", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillFax, 35, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "BillEmail", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillEmail, 100, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "BillWebsite", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.BillWebsite, 50, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "ShipFirstName", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipFirstName, 30, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "ShipMiddleName", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipMiddleName, 30, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "ShipLastName", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipLastName, 30, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "ShipCompany", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipCompany, 60, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "ShipStreet1", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipStreet1, 60, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "ShipStreet2", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipStreet2, 60, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "ShipStreet3", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipStreet3, 60, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "ShipCity", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipCity, 50, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "ShipStateProvCode", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipStateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "ShipPostalCode", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipPostalCode, 20, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "ShipCountryCode", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipCountryCode, 50, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "ShipPhone", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipPhone, 25, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "ShipFax", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipFax, 35, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "ShipEmail", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipEmail, 100, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "ShipWebsite", typeof(System.String), false, false, false, false,  (int)CustomerFieldIndex.ShipWebsite, 50, 0, 0);
			base.AddElementFieldInfo("CustomerEntity", "RollupOrderCount", typeof(System.Int32), false, false, false, false,  (int)CustomerFieldIndex.RollupOrderCount, 0, 0, 10);
			base.AddElementFieldInfo("CustomerEntity", "RollupOrderTotal", typeof(System.Decimal), false, false, false, false,  (int)CustomerFieldIndex.RollupOrderTotal, 0, 4, 19);
			base.AddElementFieldInfo("CustomerEntity", "RollupNoteCount", typeof(System.Int32), false, false, false, false,  (int)CustomerFieldIndex.RollupNoteCount, 0, 0, 10);
		}
		/// <summary>Inits DimensionsProfileEntity's FieldInfo objects</summary>
		private void InitDimensionsProfileEntityInfos()
		{
			base.AddElementFieldInfo("DimensionsProfileEntity", "DimensionsProfileID", typeof(System.Int64), true, false, true, false,  (int)DimensionsProfileFieldIndex.DimensionsProfileID, 0, 0, 19);
			base.AddElementFieldInfo("DimensionsProfileEntity", "Name", typeof(System.String), false, false, false, false,  (int)DimensionsProfileFieldIndex.Name, 50, 0, 0);
			base.AddElementFieldInfo("DimensionsProfileEntity", "Length", typeof(System.Double), false, false, false, false,  (int)DimensionsProfileFieldIndex.Length, 0, 0, 38);
			base.AddElementFieldInfo("DimensionsProfileEntity", "Width", typeof(System.Double), false, false, false, false,  (int)DimensionsProfileFieldIndex.Width, 0, 0, 38);
			base.AddElementFieldInfo("DimensionsProfileEntity", "Height", typeof(System.Double), false, false, false, false,  (int)DimensionsProfileFieldIndex.Height, 0, 0, 38);
			base.AddElementFieldInfo("DimensionsProfileEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)DimensionsProfileFieldIndex.Weight, 0, 0, 38);
		}
		/// <summary>Inits DownloadEntity's FieldInfo objects</summary>
		private void InitDownloadEntityInfos()
		{
			base.AddElementFieldInfo("DownloadEntity", "DownloadID", typeof(System.Int64), true, false, true, false,  (int)DownloadFieldIndex.DownloadID, 0, 0, 19);
			base.AddElementFieldInfo("DownloadEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)DownloadFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("DownloadEntity", "StoreID", typeof(System.Int64), false, true, false, false,  (int)DownloadFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("DownloadEntity", "ComputerID", typeof(System.Int64), false, true, false, false,  (int)DownloadFieldIndex.ComputerID, 0, 0, 19);
			base.AddElementFieldInfo("DownloadEntity", "UserID", typeof(System.Int64), false, true, false, false,  (int)DownloadFieldIndex.UserID, 0, 0, 19);
			base.AddElementFieldInfo("DownloadEntity", "InitiatedBy", typeof(System.Int32), false, false, false, false,  (int)DownloadFieldIndex.InitiatedBy, 0, 0, 10);
			base.AddElementFieldInfo("DownloadEntity", "Started", typeof(System.DateTime), false, false, false, false,  (int)DownloadFieldIndex.Started, 0, 0, 0);
			base.AddElementFieldInfo("DownloadEntity", "Ended", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)DownloadFieldIndex.Ended, 0, 0, 0);
			base.AddElementFieldInfo("DownloadEntity", "Duration", typeof(Nullable<System.Int32>), false, false, true, true,  (int)DownloadFieldIndex.Duration, 0, 0, 10);
			base.AddElementFieldInfo("DownloadEntity", "QuantityTotal", typeof(Nullable<System.Int32>), false, false, false, true,  (int)DownloadFieldIndex.QuantityTotal, 0, 0, 10);
			base.AddElementFieldInfo("DownloadEntity", "QuantityNew", typeof(Nullable<System.Int32>), false, false, false, true,  (int)DownloadFieldIndex.QuantityNew, 0, 0, 10);
			base.AddElementFieldInfo("DownloadEntity", "Result", typeof(System.Int32), false, false, false, false,  (int)DownloadFieldIndex.Result, 0, 0, 10);
			base.AddElementFieldInfo("DownloadEntity", "ErrorMessage", typeof(System.String), false, false, false, true,  (int)DownloadFieldIndex.ErrorMessage, 2147483647, 0, 0);
		}
		/// <summary>Inits DownloadDetailEntity's FieldInfo objects</summary>
		private void InitDownloadDetailEntityInfos()
		{
			base.AddElementFieldInfo("DownloadDetailEntity", "DownloadedDetailID", typeof(System.Int64), true, false, true, false,  (int)DownloadDetailFieldIndex.DownloadedDetailID, 0, 0, 19);
			base.AddElementFieldInfo("DownloadDetailEntity", "DownloadID", typeof(System.Int64), false, true, false, false,  (int)DownloadDetailFieldIndex.DownloadID, 0, 0, 19);
			base.AddElementFieldInfo("DownloadDetailEntity", "OrderID", typeof(System.Int64), false, false, false, false,  (int)DownloadDetailFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("DownloadDetailEntity", "InitialDownload", typeof(System.Boolean), false, false, false, false,  (int)DownloadDetailFieldIndex.InitialDownload, 0, 0, 0);
			base.AddElementFieldInfo("DownloadDetailEntity", "OrderNumber", typeof(Nullable<System.Int64>), false, false, false, true,  (int)DownloadDetailFieldIndex.OrderNumber, 0, 0, 19);
			base.AddElementFieldInfo("DownloadDetailEntity", "ExtraBigIntData1", typeof(Nullable<System.Int64>), false, false, false, true,  (int)DownloadDetailFieldIndex.ExtraBigIntData1, 0, 0, 19);
			base.AddElementFieldInfo("DownloadDetailEntity", "ExtraBigIntData2", typeof(Nullable<System.Int64>), false, false, false, true,  (int)DownloadDetailFieldIndex.ExtraBigIntData2, 0, 0, 19);
			base.AddElementFieldInfo("DownloadDetailEntity", "ExtraBigIntData3", typeof(Nullable<System.Int64>), false, false, false, true,  (int)DownloadDetailFieldIndex.ExtraBigIntData3, 0, 0, 19);
			base.AddElementFieldInfo("DownloadDetailEntity", "ExtraStringData1", typeof(System.String), false, false, false, true,  (int)DownloadDetailFieldIndex.ExtraStringData1, 50, 0, 0);
		}
		/// <summary>Inits EbayCombinedOrderRelationEntity's FieldInfo objects</summary>
		private void InitEbayCombinedOrderRelationEntityInfos()
		{
			base.AddElementFieldInfo("EbayCombinedOrderRelationEntity", "EbayCombinedOrderRelationID", typeof(System.Int64), true, false, true, false,  (int)EbayCombinedOrderRelationFieldIndex.EbayCombinedOrderRelationID, 0, 0, 19);
			base.AddElementFieldInfo("EbayCombinedOrderRelationEntity", "OrderID", typeof(System.Int64), false, true, false, false,  (int)EbayCombinedOrderRelationFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("EbayCombinedOrderRelationEntity", "EbayOrderID", typeof(System.Int64), false, false, false, false,  (int)EbayCombinedOrderRelationFieldIndex.EbayOrderID, 0, 0, 19);
			base.AddElementFieldInfo("EbayCombinedOrderRelationEntity", "StoreID", typeof(System.Int64), false, true, false, false,  (int)EbayCombinedOrderRelationFieldIndex.StoreID, 0, 0, 19);
		}
		/// <summary>Inits EbayOrderEntity's FieldInfo objects</summary>
		private void InitEbayOrderEntityInfos()
		{
			base.AddElementFieldInfo("EbayOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)EbayOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("EbayOrderEntity", "EbayOrderID", typeof(System.Int64), false, false, false, false,  (int)EbayOrderFieldIndex.EbayOrderID, 0, 0, 19);
			base.AddElementFieldInfo("EbayOrderEntity", "EbayBuyerID", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.EbayBuyerID, 50, 0, 0);
			base.AddElementFieldInfo("EbayOrderEntity", "CombinedLocally", typeof(System.Boolean), false, false, false, false,  (int)EbayOrderFieldIndex.CombinedLocally, 0, 0, 0);
			base.AddElementFieldInfo("EbayOrderEntity", "SelectedShippingMethod", typeof(System.Int32), false, false, false, false,  (int)EbayOrderFieldIndex.SelectedShippingMethod, 0, 0, 10);
			base.AddElementFieldInfo("EbayOrderEntity", "SellingManagerRecord", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EbayOrderFieldIndex.SellingManagerRecord, 0, 0, 10);
			base.AddElementFieldInfo("EbayOrderEntity", "GspEligible", typeof(System.Boolean), false, false, false, false,  (int)EbayOrderFieldIndex.GspEligible, 0, 0, 0);
			base.AddElementFieldInfo("EbayOrderEntity", "GspFirstName", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspFirstName, 128, 0, 0);
			base.AddElementFieldInfo("EbayOrderEntity", "GspLastName", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspLastName, 128, 0, 0);
			base.AddElementFieldInfo("EbayOrderEntity", "GspStreet1", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspStreet1, 512, 0, 0);
			base.AddElementFieldInfo("EbayOrderEntity", "GspStreet2", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspStreet2, 512, 0, 0);
			base.AddElementFieldInfo("EbayOrderEntity", "GspCity", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspCity, 128, 0, 0);
			base.AddElementFieldInfo("EbayOrderEntity", "GspStateProvince", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspStateProvince, 128, 0, 0);
			base.AddElementFieldInfo("EbayOrderEntity", "GspPostalCode", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspPostalCode, 9, 0, 0);
			base.AddElementFieldInfo("EbayOrderEntity", "GspCountryCode", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspCountryCode, 2, 0, 0);
			base.AddElementFieldInfo("EbayOrderEntity", "GspReferenceID", typeof(System.String), false, false, false, false,  (int)EbayOrderFieldIndex.GspReferenceID, 128, 0, 0);
			base.AddElementFieldInfo("EbayOrderEntity", "RollupEbayItemCount", typeof(System.Int32), false, false, false, false,  (int)EbayOrderFieldIndex.RollupEbayItemCount, 0, 0, 10);
			base.AddElementFieldInfo("EbayOrderEntity", "RollupEffectiveCheckoutStatus", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EbayOrderFieldIndex.RollupEffectiveCheckoutStatus, 0, 0, 10);
			base.AddElementFieldInfo("EbayOrderEntity", "RollupEffectivePaymentMethod", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EbayOrderFieldIndex.RollupEffectivePaymentMethod, 0, 0, 10);
			base.AddElementFieldInfo("EbayOrderEntity", "RollupFeedbackLeftType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EbayOrderFieldIndex.RollupFeedbackLeftType, 0, 0, 10);
			base.AddElementFieldInfo("EbayOrderEntity", "RollupFeedbackLeftComments", typeof(System.String), false, false, false, true,  (int)EbayOrderFieldIndex.RollupFeedbackLeftComments, 80, 0, 0);
			base.AddElementFieldInfo("EbayOrderEntity", "RollupFeedbackReceivedType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EbayOrderFieldIndex.RollupFeedbackReceivedType, 0, 0, 10);
			base.AddElementFieldInfo("EbayOrderEntity", "RollupFeedbackReceivedComments", typeof(System.String), false, false, false, true,  (int)EbayOrderFieldIndex.RollupFeedbackReceivedComments, 80, 0, 0);
			base.AddElementFieldInfo("EbayOrderEntity", "RollupPayPalAddressStatus", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EbayOrderFieldIndex.RollupPayPalAddressStatus, 0, 0, 10);
		}
		/// <summary>Inits EbayOrderItemEntity's FieldInfo objects</summary>
		private void InitEbayOrderItemEntityInfos()
		{
			base.AddElementFieldInfo("EbayOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, false, false,  (int)EbayOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			base.AddElementFieldInfo("EbayOrderItemEntity", "LocalEbayOrderID", typeof(System.Int64), false, true, false, false,  (int)EbayOrderItemFieldIndex.LocalEbayOrderID, 0, 0, 19);
			base.AddElementFieldInfo("EbayOrderItemEntity", "EbayItemID", typeof(System.Int64), false, false, false, false,  (int)EbayOrderItemFieldIndex.EbayItemID, 0, 0, 19);
			base.AddElementFieldInfo("EbayOrderItemEntity", "EbayTransactionID", typeof(System.Int64), false, false, false, false,  (int)EbayOrderItemFieldIndex.EbayTransactionID, 0, 0, 19);
			base.AddElementFieldInfo("EbayOrderItemEntity", "SellingManagerRecord", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.SellingManagerRecord, 0, 0, 10);
			base.AddElementFieldInfo("EbayOrderItemEntity", "EffectiveCheckoutStatus", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.EffectiveCheckoutStatus, 0, 0, 10);
			base.AddElementFieldInfo("EbayOrderItemEntity", "EffectivePaymentMethod", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.EffectivePaymentMethod, 0, 0, 10);
			base.AddElementFieldInfo("EbayOrderItemEntity", "PaymentStatus", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.PaymentStatus, 0, 0, 10);
			base.AddElementFieldInfo("EbayOrderItemEntity", "PaymentMethod", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.PaymentMethod, 0, 0, 10);
			base.AddElementFieldInfo("EbayOrderItemEntity", "CompleteStatus", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.CompleteStatus, 0, 0, 10);
			base.AddElementFieldInfo("EbayOrderItemEntity", "FeedbackLeftType", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.FeedbackLeftType, 0, 0, 10);
			base.AddElementFieldInfo("EbayOrderItemEntity", "FeedbackLeftComments", typeof(System.String), false, false, false, false,  (int)EbayOrderItemFieldIndex.FeedbackLeftComments, 80, 0, 0);
			base.AddElementFieldInfo("EbayOrderItemEntity", "FeedbackReceivedType", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.FeedbackReceivedType, 0, 0, 10);
			base.AddElementFieldInfo("EbayOrderItemEntity", "FeedbackReceivedComments", typeof(System.String), false, false, false, false,  (int)EbayOrderItemFieldIndex.FeedbackReceivedComments, 80, 0, 0);
			base.AddElementFieldInfo("EbayOrderItemEntity", "MyEbayPaid", typeof(System.Boolean), false, false, false, false,  (int)EbayOrderItemFieldIndex.MyEbayPaid, 0, 0, 0);
			base.AddElementFieldInfo("EbayOrderItemEntity", "MyEbayShipped", typeof(System.Boolean), false, false, false, false,  (int)EbayOrderItemFieldIndex.MyEbayShipped, 0, 0, 0);
			base.AddElementFieldInfo("EbayOrderItemEntity", "PayPalTransactionID", typeof(System.String), false, false, false, false,  (int)EbayOrderItemFieldIndex.PayPalTransactionID, 50, 0, 0);
			base.AddElementFieldInfo("EbayOrderItemEntity", "PayPalAddressStatus", typeof(System.Int32), false, false, false, false,  (int)EbayOrderItemFieldIndex.PayPalAddressStatus, 0, 0, 10);
		}
		/// <summary>Inits EbayStoreEntity's FieldInfo objects</summary>
		private void InitEbayStoreEntityInfos()
		{
			base.AddElementFieldInfo("EbayStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)EbayStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("EbayStoreEntity", "EBayUserID", typeof(System.String), false, false, false, false,  (int)EbayStoreFieldIndex.EBayUserID, 50, 0, 0);
			base.AddElementFieldInfo("EbayStoreEntity", "EBayToken", typeof(System.String), false, false, false, false,  (int)EbayStoreFieldIndex.EBayToken, 2147483647, 0, 0);
			base.AddElementFieldInfo("EbayStoreEntity", "EBayTokenExpire", typeof(System.DateTime), false, false, false, false,  (int)EbayStoreFieldIndex.EBayTokenExpire, 0, 0, 0);
			base.AddElementFieldInfo("EbayStoreEntity", "AcceptedPaymentList", typeof(System.String), false, false, false, false,  (int)EbayStoreFieldIndex.AcceptedPaymentList, 30, 0, 0);
			base.AddElementFieldInfo("EbayStoreEntity", "DownloadItemDetails", typeof(System.Boolean), false, false, false, false,  (int)EbayStoreFieldIndex.DownloadItemDetails, 0, 0, 0);
			base.AddElementFieldInfo("EbayStoreEntity", "DownloadOlderOrders", typeof(System.Boolean), false, false, false, false,  (int)EbayStoreFieldIndex.DownloadOlderOrders, 0, 0, 0);
			base.AddElementFieldInfo("EbayStoreEntity", "DownloadPayPalDetails", typeof(System.Boolean), false, false, false, false,  (int)EbayStoreFieldIndex.DownloadPayPalDetails, 0, 0, 0);
			base.AddElementFieldInfo("EbayStoreEntity", "PayPalApiCredentialType", typeof(System.Int16), false, false, false, false,  (int)EbayStoreFieldIndex.PayPalApiCredentialType, 0, 0, 5);
			base.AddElementFieldInfo("EbayStoreEntity", "PayPalApiUserName", typeof(System.String), false, false, false, false,  (int)EbayStoreFieldIndex.PayPalApiUserName, 255, 0, 0);
			base.AddElementFieldInfo("EbayStoreEntity", "PayPalApiPassword", typeof(System.String), false, false, false, false,  (int)EbayStoreFieldIndex.PayPalApiPassword, 80, 0, 0);
			base.AddElementFieldInfo("EbayStoreEntity", "PayPalApiSignature", typeof(System.String), false, false, false, false,  (int)EbayStoreFieldIndex.PayPalApiSignature, 80, 0, 0);
			base.AddElementFieldInfo("EbayStoreEntity", "PayPalApiCertificate", typeof(System.Byte[]), false, false, false, true,  (int)EbayStoreFieldIndex.PayPalApiCertificate, 2048, 0, 0);
			base.AddElementFieldInfo("EbayStoreEntity", "DomesticShippingService", typeof(System.String), false, false, false, false,  (int)EbayStoreFieldIndex.DomesticShippingService, 50, 0, 0);
			base.AddElementFieldInfo("EbayStoreEntity", "InternationalShippingService", typeof(System.String), false, false, false, false,  (int)EbayStoreFieldIndex.InternationalShippingService, 50, 0, 0);
			base.AddElementFieldInfo("EbayStoreEntity", "FeedbackUpdatedThrough", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)EbayStoreFieldIndex.FeedbackUpdatedThrough, 0, 0, 0);
		}
		/// <summary>Inits EmailAccountEntity's FieldInfo objects</summary>
		private void InitEmailAccountEntityInfos()
		{
			base.AddElementFieldInfo("EmailAccountEntity", "EmailAccountID", typeof(System.Int64), true, false, true, false,  (int)EmailAccountFieldIndex.EmailAccountID, 0, 0, 19);
			base.AddElementFieldInfo("EmailAccountEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)EmailAccountFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("EmailAccountEntity", "AccountName", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.AccountName, 50, 0, 0);
			base.AddElementFieldInfo("EmailAccountEntity", "DisplayName", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.DisplayName, 50, 0, 0);
			base.AddElementFieldInfo("EmailAccountEntity", "EmailAddress", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.EmailAddress, 100, 0, 0);
			base.AddElementFieldInfo("EmailAccountEntity", "IncomingServer", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.IncomingServer, 100, 0, 0);
			base.AddElementFieldInfo("EmailAccountEntity", "IncomingServerType", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.IncomingServerType, 0, 0, 10);
			base.AddElementFieldInfo("EmailAccountEntity", "IncomingPort", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.IncomingPort, 0, 0, 10);
			base.AddElementFieldInfo("EmailAccountEntity", "IncomingSecurityType", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.IncomingSecurityType, 0, 0, 10);
			base.AddElementFieldInfo("EmailAccountEntity", "IncomingUsername", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.IncomingUsername, 50, 0, 0);
			base.AddElementFieldInfo("EmailAccountEntity", "IncomingPassword", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.IncomingPassword, 150, 0, 0);
			base.AddElementFieldInfo("EmailAccountEntity", "OutgoingServer", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.OutgoingServer, 100, 0, 0);
			base.AddElementFieldInfo("EmailAccountEntity", "OutgoingPort", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.OutgoingPort, 0, 0, 10);
			base.AddElementFieldInfo("EmailAccountEntity", "OutgoingSecurityType", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.OutgoingSecurityType, 0, 0, 10);
			base.AddElementFieldInfo("EmailAccountEntity", "OutgoingCredentialSource", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.OutgoingCredentialSource, 0, 0, 10);
			base.AddElementFieldInfo("EmailAccountEntity", "OutgoingUsername", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.OutgoingUsername, 50, 0, 0);
			base.AddElementFieldInfo("EmailAccountEntity", "OutgoingPassword", typeof(System.String), false, false, false, false,  (int)EmailAccountFieldIndex.OutgoingPassword, 150, 0, 0);
			base.AddElementFieldInfo("EmailAccountEntity", "AutoSend", typeof(System.Boolean), false, false, false, false,  (int)EmailAccountFieldIndex.AutoSend, 0, 0, 0);
			base.AddElementFieldInfo("EmailAccountEntity", "AutoSendMinutes", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.AutoSendMinutes, 0, 0, 10);
			base.AddElementFieldInfo("EmailAccountEntity", "AutoSendLastTime", typeof(System.DateTime), false, false, false, false,  (int)EmailAccountFieldIndex.AutoSendLastTime, 0, 0, 0);
			base.AddElementFieldInfo("EmailAccountEntity", "LimitMessagesPerConnection", typeof(System.Boolean), false, false, false, false,  (int)EmailAccountFieldIndex.LimitMessagesPerConnection, 0, 0, 0);
			base.AddElementFieldInfo("EmailAccountEntity", "LimitMessagesPerConnectionQuantity", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.LimitMessagesPerConnectionQuantity, 0, 0, 10);
			base.AddElementFieldInfo("EmailAccountEntity", "LimitMessagesPerHour", typeof(System.Boolean), false, false, false, false,  (int)EmailAccountFieldIndex.LimitMessagesPerHour, 0, 0, 0);
			base.AddElementFieldInfo("EmailAccountEntity", "LimitMessagesPerHourQuantity", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.LimitMessagesPerHourQuantity, 0, 0, 10);
			base.AddElementFieldInfo("EmailAccountEntity", "LimitMessageInterval", typeof(System.Boolean), false, false, false, false,  (int)EmailAccountFieldIndex.LimitMessageInterval, 0, 0, 0);
			base.AddElementFieldInfo("EmailAccountEntity", "LimitMessageIntervalSeconds", typeof(System.Int32), false, false, false, false,  (int)EmailAccountFieldIndex.LimitMessageIntervalSeconds, 0, 0, 10);
			base.AddElementFieldInfo("EmailAccountEntity", "InternalOwnerID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)EmailAccountFieldIndex.InternalOwnerID, 0, 0, 19);
		}
		/// <summary>Inits EmailOutboundEntity's FieldInfo objects</summary>
		private void InitEmailOutboundEntityInfos()
		{
			base.AddElementFieldInfo("EmailOutboundEntity", "EmailOutboundID", typeof(System.Int64), true, false, true, false,  (int)EmailOutboundFieldIndex.EmailOutboundID, 0, 0, 19);
			base.AddElementFieldInfo("EmailOutboundEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)EmailOutboundFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("EmailOutboundEntity", "ContextID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)EmailOutboundFieldIndex.ContextID, 0, 0, 19);
			base.AddElementFieldInfo("EmailOutboundEntity", "ContextType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EmailOutboundFieldIndex.ContextType, 0, 0, 10);
			base.AddElementFieldInfo("EmailOutboundEntity", "TemplateID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)EmailOutboundFieldIndex.TemplateID, 0, 0, 19);
			base.AddElementFieldInfo("EmailOutboundEntity", "AccountID", typeof(System.Int64), false, false, false, false,  (int)EmailOutboundFieldIndex.AccountID, 0, 0, 19);
			base.AddElementFieldInfo("EmailOutboundEntity", "Visibility", typeof(System.Int32), false, false, false, false,  (int)EmailOutboundFieldIndex.Visibility, 0, 0, 10);
			base.AddElementFieldInfo("EmailOutboundEntity", "FromAddress", typeof(System.String), false, false, false, false,  (int)EmailOutboundFieldIndex.FromAddress, 200, 0, 0);
			base.AddElementFieldInfo("EmailOutboundEntity", "ToList", typeof(System.String), false, false, false, false,  (int)EmailOutboundFieldIndex.ToList, 2147483647, 0, 0);
			base.AddElementFieldInfo("EmailOutboundEntity", "CcList", typeof(System.String), false, false, false, false,  (int)EmailOutboundFieldIndex.CcList, 2147483647, 0, 0);
			base.AddElementFieldInfo("EmailOutboundEntity", "BccList", typeof(System.String), false, false, false, false,  (int)EmailOutboundFieldIndex.BccList, 2147483647, 0, 0);
			base.AddElementFieldInfo("EmailOutboundEntity", "Subject", typeof(System.String), false, false, false, false,  (int)EmailOutboundFieldIndex.Subject, 300, 0, 0);
			base.AddElementFieldInfo("EmailOutboundEntity", "HtmlPartResourceID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)EmailOutboundFieldIndex.HtmlPartResourceID, 0, 0, 19);
			base.AddElementFieldInfo("EmailOutboundEntity", "PlainPartResourceID", typeof(System.Int64), false, false, false, false,  (int)EmailOutboundFieldIndex.PlainPartResourceID, 0, 0, 19);
			base.AddElementFieldInfo("EmailOutboundEntity", "Encoding", typeof(System.String), false, false, false, true,  (int)EmailOutboundFieldIndex.Encoding, 20, 0, 0);
			base.AddElementFieldInfo("EmailOutboundEntity", "ComposedDate", typeof(System.DateTime), false, false, false, false,  (int)EmailOutboundFieldIndex.ComposedDate, 0, 0, 0);
			base.AddElementFieldInfo("EmailOutboundEntity", "SentDate", typeof(System.DateTime), false, false, false, false,  (int)EmailOutboundFieldIndex.SentDate, 0, 0, 0);
			base.AddElementFieldInfo("EmailOutboundEntity", "DontSendBefore", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)EmailOutboundFieldIndex.DontSendBefore, 0, 0, 0);
			base.AddElementFieldInfo("EmailOutboundEntity", "SendStatus", typeof(System.Int32), false, false, false, false,  (int)EmailOutboundFieldIndex.SendStatus, 0, 0, 10);
			base.AddElementFieldInfo("EmailOutboundEntity", "SendAttemptCount", typeof(System.Int32), false, false, false, false,  (int)EmailOutboundFieldIndex.SendAttemptCount, 0, 0, 10);
			base.AddElementFieldInfo("EmailOutboundEntity", "SendAttemptLastError", typeof(System.String), false, false, false, false,  (int)EmailOutboundFieldIndex.SendAttemptLastError, 300, 0, 0);
		}
		/// <summary>Inits EmailOutboundRelationEntity's FieldInfo objects</summary>
		private void InitEmailOutboundRelationEntityInfos()
		{
			base.AddElementFieldInfo("EmailOutboundRelationEntity", "EmailOutboundRelationID", typeof(System.Int64), true, false, true, false,  (int)EmailOutboundRelationFieldIndex.EmailOutboundRelationID, 0, 0, 19);
			base.AddElementFieldInfo("EmailOutboundRelationEntity", "EmailOutboundID", typeof(System.Int64), false, true, false, false,  (int)EmailOutboundRelationFieldIndex.EmailOutboundID, 0, 0, 19);
			base.AddElementFieldInfo("EmailOutboundRelationEntity", "ObjectID", typeof(System.Int64), false, false, false, false,  (int)EmailOutboundRelationFieldIndex.ObjectID, 0, 0, 19);
			base.AddElementFieldInfo("EmailOutboundRelationEntity", "RelationType", typeof(System.Int32), false, false, false, false,  (int)EmailOutboundRelationFieldIndex.RelationType, 0, 0, 10);
		}
		/// <summary>Inits EndiciaAccountEntity's FieldInfo objects</summary>
		private void InitEndiciaAccountEntityInfos()
		{
			base.AddElementFieldInfo("EndiciaAccountEntity", "EndiciaAccountID", typeof(System.Int64), true, false, true, false,  (int)EndiciaAccountFieldIndex.EndiciaAccountID, 0, 0, 19);
			base.AddElementFieldInfo("EndiciaAccountEntity", "EndiciaReseller", typeof(System.Int32), false, false, false, false,  (int)EndiciaAccountFieldIndex.EndiciaReseller, 0, 0, 10);
			base.AddElementFieldInfo("EndiciaAccountEntity", "AccountNumber", typeof(System.String), false, false, false, true,  (int)EndiciaAccountFieldIndex.AccountNumber, 50, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "SignupConfirmation", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.SignupConfirmation, 30, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "WebPassword", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.WebPassword, 250, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "ApiInitialPassword", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.ApiInitialPassword, 250, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "ApiUserPassword", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.ApiUserPassword, 250, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "AccountType", typeof(System.Int32), false, false, false, false,  (int)EndiciaAccountFieldIndex.AccountType, 0, 0, 10);
			base.AddElementFieldInfo("EndiciaAccountEntity", "TestAccount", typeof(System.Boolean), false, false, false, false,  (int)EndiciaAccountFieldIndex.TestAccount, 0, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "CreatedByShipWorks", typeof(System.Boolean), false, false, false, false,  (int)EndiciaAccountFieldIndex.CreatedByShipWorks, 0, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "Description", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.Description, 50, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "FirstName", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.FirstName, 30, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "LastName", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.LastName, 30, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "Company", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.Company, 30, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "Street1", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.Street1, 60, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "Street2", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.Street2, 60, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "Street3", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.Street3, 60, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "City", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.City, 50, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.StateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.PostalCode, 20, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.CountryCode, 50, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "Phone", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.Phone, 25, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "Fax", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.Fax, 35, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "Email", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.Email, 100, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "MailingPostalCode", typeof(System.String), false, false, false, false,  (int)EndiciaAccountFieldIndex.MailingPostalCode, 20, 0, 0);
			base.AddElementFieldInfo("EndiciaAccountEntity", "ScanFormAddressSource", typeof(System.Int32), false, false, false, false,  (int)EndiciaAccountFieldIndex.ScanFormAddressSource, 0, 0, 10);
		}
		/// <summary>Inits EndiciaProfileEntity's FieldInfo objects</summary>
		private void InitEndiciaProfileEntityInfos()
		{
			base.AddElementFieldInfo("EndiciaProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)EndiciaProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			base.AddElementFieldInfo("EndiciaProfileEntity", "EndiciaAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)EndiciaProfileFieldIndex.EndiciaAccountID, 0, 0, 19);
			base.AddElementFieldInfo("EndiciaProfileEntity", "StealthPostage", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)EndiciaProfileFieldIndex.StealthPostage, 0, 0, 0);
			base.AddElementFieldInfo("EndiciaProfileEntity", "ReferenceID", typeof(System.String), false, false, false, true,  (int)EndiciaProfileFieldIndex.ReferenceID, 300, 0, 0);
			base.AddElementFieldInfo("EndiciaProfileEntity", "ScanBasedReturn", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)EndiciaProfileFieldIndex.ScanBasedReturn, 0, 0, 0);
		}
		/// <summary>Inits EndiciaScanFormEntity's FieldInfo objects</summary>
		private void InitEndiciaScanFormEntityInfos()
		{
			base.AddElementFieldInfo("EndiciaScanFormEntity", "EndiciaScanFormID", typeof(System.Int64), true, false, true, false,  (int)EndiciaScanFormFieldIndex.EndiciaScanFormID, 0, 0, 19);
			base.AddElementFieldInfo("EndiciaScanFormEntity", "EndiciaAccountID", typeof(System.Int64), false, false, false, false,  (int)EndiciaScanFormFieldIndex.EndiciaAccountID, 0, 0, 19);
			base.AddElementFieldInfo("EndiciaScanFormEntity", "EndiciaAccountNumber", typeof(System.String), false, false, false, false,  (int)EndiciaScanFormFieldIndex.EndiciaAccountNumber, 50, 0, 0);
			base.AddElementFieldInfo("EndiciaScanFormEntity", "SubmissionID", typeof(System.String), false, false, false, false,  (int)EndiciaScanFormFieldIndex.SubmissionID, 100, 0, 0);
			base.AddElementFieldInfo("EndiciaScanFormEntity", "CreatedDate", typeof(System.DateTime), false, false, false, false,  (int)EndiciaScanFormFieldIndex.CreatedDate, 0, 0, 0);
			base.AddElementFieldInfo("EndiciaScanFormEntity", "ScanFormBatchID", typeof(System.Int64), false, true, false, false,  (int)EndiciaScanFormFieldIndex.ScanFormBatchID, 0, 0, 19);
			base.AddElementFieldInfo("EndiciaScanFormEntity", "Description", typeof(System.String), false, false, false, false,  (int)EndiciaScanFormFieldIndex.Description, 100, 0, 0);
		}
		/// <summary>Inits EndiciaShipmentEntity's FieldInfo objects</summary>
		private void InitEndiciaShipmentEntityInfos()
		{
			base.AddElementFieldInfo("EndiciaShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)EndiciaShipmentFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("EndiciaShipmentEntity", "EndiciaAccountID", typeof(System.Int64), false, false, false, false,  (int)EndiciaShipmentFieldIndex.EndiciaAccountID, 0, 0, 19);
			base.AddElementFieldInfo("EndiciaShipmentEntity", "OriginalEndiciaAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)EndiciaShipmentFieldIndex.OriginalEndiciaAccountID, 0, 0, 19);
			base.AddElementFieldInfo("EndiciaShipmentEntity", "StealthPostage", typeof(System.Boolean), false, false, false, false,  (int)EndiciaShipmentFieldIndex.StealthPostage, 0, 0, 0);
			base.AddElementFieldInfo("EndiciaShipmentEntity", "ReferenceID", typeof(System.String), false, false, false, false,  (int)EndiciaShipmentFieldIndex.ReferenceID, 300, 0, 0);
			base.AddElementFieldInfo("EndiciaShipmentEntity", "TransactionID", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EndiciaShipmentFieldIndex.TransactionID, 0, 0, 10);
			base.AddElementFieldInfo("EndiciaShipmentEntity", "RefundFormID", typeof(Nullable<System.Int32>), false, false, false, true,  (int)EndiciaShipmentFieldIndex.RefundFormID, 0, 0, 10);
			base.AddElementFieldInfo("EndiciaShipmentEntity", "ScanFormBatchID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)EndiciaShipmentFieldIndex.ScanFormBatchID, 0, 0, 19);
			base.AddElementFieldInfo("EndiciaShipmentEntity", "ScanBasedReturn", typeof(System.Boolean), false, false, false, false,  (int)EndiciaShipmentFieldIndex.ScanBasedReturn, 0, 0, 0);
			base.AddElementFieldInfo("EndiciaShipmentEntity", "RequestedLabelFormat", typeof(System.Int32), false, false, false, false,  (int)EndiciaShipmentFieldIndex.RequestedLabelFormat, 0, 0, 10);
		}
		/// <summary>Inits EtsyOrderEntity's FieldInfo objects</summary>
		private void InitEtsyOrderEntityInfos()
		{
			base.AddElementFieldInfo("EtsyOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)EtsyOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("EtsyOrderEntity", "WasPaid", typeof(System.Boolean), false, false, false, false,  (int)EtsyOrderFieldIndex.WasPaid, 0, 0, 0);
			base.AddElementFieldInfo("EtsyOrderEntity", "WasShipped", typeof(System.Boolean), false, false, false, false,  (int)EtsyOrderFieldIndex.WasShipped, 0, 0, 0);
		}
		/// <summary>Inits EtsyStoreEntity's FieldInfo objects</summary>
		private void InitEtsyStoreEntityInfos()
		{
			base.AddElementFieldInfo("EtsyStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)EtsyStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("EtsyStoreEntity", "EtsyShopID", typeof(System.Int64), false, false, false, false,  (int)EtsyStoreFieldIndex.EtsyShopID, 0, 0, 19);
			base.AddElementFieldInfo("EtsyStoreEntity", "EtsyLoginName", typeof(System.String), false, false, false, false,  (int)EtsyStoreFieldIndex.EtsyLoginName, 255, 0, 0);
			base.AddElementFieldInfo("EtsyStoreEntity", "EtsyStoreName", typeof(System.String), false, false, false, false,  (int)EtsyStoreFieldIndex.EtsyStoreName, 255, 0, 0);
			base.AddElementFieldInfo("EtsyStoreEntity", "OAuthToken", typeof(System.String), false, false, false, false,  (int)EtsyStoreFieldIndex.OAuthToken, 50, 0, 0);
			base.AddElementFieldInfo("EtsyStoreEntity", "OAuthTokenSecret", typeof(System.String), false, false, false, false,  (int)EtsyStoreFieldIndex.OAuthTokenSecret, 50, 0, 0);
		}
		/// <summary>Inits ExcludedPackageTypeEntity's FieldInfo objects</summary>
		private void InitExcludedPackageTypeEntityInfos()
		{
			base.AddElementFieldInfo("ExcludedPackageTypeEntity", "ShipmentType", typeof(System.Int32), true, false, false, false,  (int)ExcludedPackageTypeFieldIndex.ShipmentType, 0, 0, 10);
			base.AddElementFieldInfo("ExcludedPackageTypeEntity", "PackageType", typeof(System.Int32), true, false, false, false,  (int)ExcludedPackageTypeFieldIndex.PackageType, 0, 0, 10);
		}
		/// <summary>Inits ExcludedServiceTypeEntity's FieldInfo objects</summary>
		private void InitExcludedServiceTypeEntityInfos()
		{
			base.AddElementFieldInfo("ExcludedServiceTypeEntity", "ShipmentType", typeof(System.Int32), true, false, false, false,  (int)ExcludedServiceTypeFieldIndex.ShipmentType, 0, 0, 10);
			base.AddElementFieldInfo("ExcludedServiceTypeEntity", "ServiceType", typeof(System.Int32), true, false, false, false,  (int)ExcludedServiceTypeFieldIndex.ServiceType, 0, 0, 10);
		}
		/// <summary>Inits FedExAccountEntity's FieldInfo objects</summary>
		private void InitFedExAccountEntityInfos()
		{
			base.AddElementFieldInfo("FedExAccountEntity", "FedExAccountID", typeof(System.Int64), true, false, true, false,  (int)FedExAccountFieldIndex.FedExAccountID, 0, 0, 19);
			base.AddElementFieldInfo("FedExAccountEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)FedExAccountFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "Description", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.Description, 50, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "AccountNumber", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.AccountNumber, 12, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "SignatureRelease", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.SignatureRelease, 10, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "MeterNumber", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.MeterNumber, 50, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "SmartPostHubList", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.SmartPostHubList, 2147483647, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "FirstName", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.FirstName, 30, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "MiddleName", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.MiddleName, 30, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "LastName", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.LastName, 30, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "Company", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.Company, 35, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "Street1", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.Street1, 60, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "Street2", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.Street2, 60, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "City", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.City, 50, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.StateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.PostalCode, 20, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.CountryCode, 50, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "Phone", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.Phone, 25, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "Email", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.Email, 100, 0, 0);
			base.AddElementFieldInfo("FedExAccountEntity", "Website", typeof(System.String), false, false, false, false,  (int)FedExAccountFieldIndex.Website, 50, 0, 0);
		}
		/// <summary>Inits FedExEndOfDayCloseEntity's FieldInfo objects</summary>
		private void InitFedExEndOfDayCloseEntityInfos()
		{
			base.AddElementFieldInfo("FedExEndOfDayCloseEntity", "FedExEndOfDayCloseID", typeof(System.Int64), true, false, true, false,  (int)FedExEndOfDayCloseFieldIndex.FedExEndOfDayCloseID, 0, 0, 19);
			base.AddElementFieldInfo("FedExEndOfDayCloseEntity", "FedExAccountID", typeof(System.Int64), false, false, false, false,  (int)FedExEndOfDayCloseFieldIndex.FedExAccountID, 0, 0, 19);
			base.AddElementFieldInfo("FedExEndOfDayCloseEntity", "AccountNumber", typeof(System.String), false, false, false, false,  (int)FedExEndOfDayCloseFieldIndex.AccountNumber, 50, 0, 0);
			base.AddElementFieldInfo("FedExEndOfDayCloseEntity", "CloseDate", typeof(System.DateTime), false, false, false, false,  (int)FedExEndOfDayCloseFieldIndex.CloseDate, 0, 0, 0);
			base.AddElementFieldInfo("FedExEndOfDayCloseEntity", "IsSmartPost", typeof(System.Boolean), false, false, false, false,  (int)FedExEndOfDayCloseFieldIndex.IsSmartPost, 0, 0, 0);
		}
		/// <summary>Inits FedExPackageEntity's FieldInfo objects</summary>
		private void InitFedExPackageEntityInfos()
		{
			base.AddElementFieldInfo("FedExPackageEntity", "FedExPackageID", typeof(System.Int64), true, false, true, false,  (int)FedExPackageFieldIndex.FedExPackageID, 0, 0, 19);
			base.AddElementFieldInfo("FedExPackageEntity", "ShipmentID", typeof(System.Int64), false, true, false, false,  (int)FedExPackageFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("FedExPackageEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)FedExPackageFieldIndex.Weight, 0, 0, 38);
			base.AddElementFieldInfo("FedExPackageEntity", "DimsProfileID", typeof(System.Int64), false, false, false, false,  (int)FedExPackageFieldIndex.DimsProfileID, 0, 0, 19);
			base.AddElementFieldInfo("FedExPackageEntity", "DimsLength", typeof(System.Double), false, false, false, false,  (int)FedExPackageFieldIndex.DimsLength, 0, 0, 38);
			base.AddElementFieldInfo("FedExPackageEntity", "DimsWidth", typeof(System.Double), false, false, false, false,  (int)FedExPackageFieldIndex.DimsWidth, 0, 0, 38);
			base.AddElementFieldInfo("FedExPackageEntity", "DimsHeight", typeof(System.Double), false, false, false, false,  (int)FedExPackageFieldIndex.DimsHeight, 0, 0, 38);
			base.AddElementFieldInfo("FedExPackageEntity", "DimsWeight", typeof(System.Double), false, false, false, false,  (int)FedExPackageFieldIndex.DimsWeight, 0, 0, 38);
			base.AddElementFieldInfo("FedExPackageEntity", "DimsAddWeight", typeof(System.Boolean), false, false, false, false,  (int)FedExPackageFieldIndex.DimsAddWeight, 0, 0, 0);
			base.AddElementFieldInfo("FedExPackageEntity", "SkidPieces", typeof(System.Int32), false, false, false, false,  (int)FedExPackageFieldIndex.SkidPieces, 0, 0, 10);
			base.AddElementFieldInfo("FedExPackageEntity", "Insurance", typeof(System.Boolean), false, false, false, false,  (int)FedExPackageFieldIndex.Insurance, 0, 0, 0);
			base.AddElementFieldInfo("FedExPackageEntity", "InsuranceValue", typeof(System.Decimal), false, false, false, false,  (int)FedExPackageFieldIndex.InsuranceValue, 0, 4, 19);
			base.AddElementFieldInfo("FedExPackageEntity", "InsurancePennyOne", typeof(System.Boolean), false, false, false, false,  (int)FedExPackageFieldIndex.InsurancePennyOne, 0, 0, 0);
			base.AddElementFieldInfo("FedExPackageEntity", "DeclaredValue", typeof(System.Decimal), false, false, false, false,  (int)FedExPackageFieldIndex.DeclaredValue, 0, 4, 19);
			base.AddElementFieldInfo("FedExPackageEntity", "TrackingNumber", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.TrackingNumber, 50, 0, 0);
			base.AddElementFieldInfo("FedExPackageEntity", "PriorityAlert", typeof(System.Boolean), false, false, false, false,  (int)FedExPackageFieldIndex.PriorityAlert, 0, 0, 0);
			base.AddElementFieldInfo("FedExPackageEntity", "PriorityAlertEnhancementType", typeof(System.Int32), false, false, false, false,  (int)FedExPackageFieldIndex.PriorityAlertEnhancementType, 0, 0, 10);
			base.AddElementFieldInfo("FedExPackageEntity", "PriorityAlertDetailContent", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.PriorityAlertDetailContent, 1024, 0, 0);
			base.AddElementFieldInfo("FedExPackageEntity", "DryIceWeight", typeof(System.Double), false, false, false, false,  (int)FedExPackageFieldIndex.DryIceWeight, 0, 0, 38);
			base.AddElementFieldInfo("FedExPackageEntity", "ContainsAlcohol", typeof(System.Boolean), false, false, false, false,  (int)FedExPackageFieldIndex.ContainsAlcohol, 0, 0, 0);
			base.AddElementFieldInfo("FedExPackageEntity", "DangerousGoodsEnabled", typeof(System.Boolean), false, false, false, false,  (int)FedExPackageFieldIndex.DangerousGoodsEnabled, 0, 0, 0);
			base.AddElementFieldInfo("FedExPackageEntity", "DangerousGoodsType", typeof(System.Int32), false, false, false, false,  (int)FedExPackageFieldIndex.DangerousGoodsType, 0, 0, 10);
			base.AddElementFieldInfo("FedExPackageEntity", "DangerousGoodsAccessibilityType", typeof(System.Int32), false, false, false, false,  (int)FedExPackageFieldIndex.DangerousGoodsAccessibilityType, 0, 0, 10);
			base.AddElementFieldInfo("FedExPackageEntity", "DangerousGoodsCargoAircraftOnly", typeof(System.Boolean), false, false, false, false,  (int)FedExPackageFieldIndex.DangerousGoodsCargoAircraftOnly, 0, 0, 0);
			base.AddElementFieldInfo("FedExPackageEntity", "DangerousGoodsEmergencyContactPhone", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.DangerousGoodsEmergencyContactPhone, 16, 0, 0);
			base.AddElementFieldInfo("FedExPackageEntity", "DangerousGoodsOfferor", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.DangerousGoodsOfferor, 128, 0, 0);
			base.AddElementFieldInfo("FedExPackageEntity", "DangerousGoodsPackagingCount", typeof(System.Int32), false, false, false, false,  (int)FedExPackageFieldIndex.DangerousGoodsPackagingCount, 0, 0, 10);
			base.AddElementFieldInfo("FedExPackageEntity", "HazardousMaterialNumber", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.HazardousMaterialNumber, 16, 0, 0);
			base.AddElementFieldInfo("FedExPackageEntity", "HazardousMaterialClass", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.HazardousMaterialClass, 8, 0, 0);
			base.AddElementFieldInfo("FedExPackageEntity", "HazardousMaterialProperName", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.HazardousMaterialProperName, 64, 0, 0);
			base.AddElementFieldInfo("FedExPackageEntity", "HazardousMaterialPackingGroup", typeof(System.Int32), false, false, false, false,  (int)FedExPackageFieldIndex.HazardousMaterialPackingGroup, 0, 0, 10);
			base.AddElementFieldInfo("FedExPackageEntity", "HazardousMaterialQuantityValue", typeof(System.Double), false, false, false, false,  (int)FedExPackageFieldIndex.HazardousMaterialQuantityValue, 0, 0, 38);
			base.AddElementFieldInfo("FedExPackageEntity", "HazardousMaterialQuanityUnits", typeof(System.Int32), false, false, false, false,  (int)FedExPackageFieldIndex.HazardousMaterialQuanityUnits, 0, 0, 10);
			base.AddElementFieldInfo("FedExPackageEntity", "HazardousMaterialTechnicalName", typeof(System.String), false, false, false, false,  (int)FedExPackageFieldIndex.HazardousMaterialTechnicalName, 64, 0, 0);
		}
		/// <summary>Inits FedExProfileEntity's FieldInfo objects</summary>
		private void InitFedExProfileEntityInfos()
		{
			base.AddElementFieldInfo("FedExProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)FedExProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			base.AddElementFieldInfo("FedExProfileEntity", "FedExAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)FedExProfileFieldIndex.FedExAccountID, 0, 0, 19);
			base.AddElementFieldInfo("FedExProfileEntity", "Service", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.Service, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfileEntity", "Signature", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.Signature, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfileEntity", "PackagingType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.PackagingType, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfileEntity", "NonStandardContainer", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfileFieldIndex.NonStandardContainer, 0, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "ReferenceCustomer", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.ReferenceCustomer, 300, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "ReferenceInvoice", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.ReferenceInvoice, 300, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "ReferencePO", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.ReferencePO, 300, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "ReferenceShipmentIntegrity", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.ReferenceShipmentIntegrity, 300, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "PayorTransportType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.PayorTransportType, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfileEntity", "PayorTransportAccount", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.PayorTransportAccount, 12, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "PayorDutiesType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.PayorDutiesType, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfileEntity", "PayorDutiesAccount", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.PayorDutiesAccount, 12, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "SaturdayDelivery", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfileFieldIndex.SaturdayDelivery, 0, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "EmailNotifySender", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.EmailNotifySender, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfileEntity", "EmailNotifyRecipient", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.EmailNotifyRecipient, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfileEntity", "EmailNotifyOther", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.EmailNotifyOther, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfileEntity", "EmailNotifyOtherAddress", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.EmailNotifyOtherAddress, 100, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "EmailNotifyMessage", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.EmailNotifyMessage, 120, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "ResidentialDetermination", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.ResidentialDetermination, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfileEntity", "SmartPostIndicia", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.SmartPostIndicia, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfileEntity", "SmartPostEndorsement", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.SmartPostEndorsement, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfileEntity", "SmartPostConfirmation", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfileFieldIndex.SmartPostConfirmation, 0, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "SmartPostCustomerManifest", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.SmartPostCustomerManifest, 50, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "SmartPostHubID", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.SmartPostHubID, 10, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "EmailNotifyBroker", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.EmailNotifyBroker, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfileEntity", "DropoffType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.DropoffType, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfileEntity", "OriginResidentialDetermination", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.OriginResidentialDetermination, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfileEntity", "PayorTransportName", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.PayorTransportName, 60, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "ReturnType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfileFieldIndex.ReturnType, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfileEntity", "RmaNumber", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.RmaNumber, 30, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "RmaReason", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.RmaReason, 60, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "ReturnSaturdayPickup", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfileFieldIndex.ReturnSaturdayPickup, 0, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "ReturnsClearance", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfileFieldIndex.ReturnsClearance, 0, 0, 0);
			base.AddElementFieldInfo("FedExProfileEntity", "ReferenceFIMS", typeof(System.String), false, false, false, true,  (int)FedExProfileFieldIndex.ReferenceFIMS, 300, 0, 0);
		}
		/// <summary>Inits FedExProfilePackageEntity's FieldInfo objects</summary>
		private void InitFedExProfilePackageEntityInfos()
		{
			base.AddElementFieldInfo("FedExProfilePackageEntity", "FedExProfilePackageID", typeof(System.Int64), true, false, true, false,  (int)FedExProfilePackageFieldIndex.FedExProfilePackageID, 0, 0, 19);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "ShippingProfileID", typeof(System.Int64), false, true, false, false,  (int)FedExProfilePackageFieldIndex.ShippingProfileID, 0, 0, 19);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "Weight", typeof(Nullable<System.Double>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.Weight, 0, 0, 38);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "DimsProfileID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DimsProfileID, 0, 0, 19);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "DimsLength", typeof(Nullable<System.Double>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DimsLength, 0, 0, 38);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "DimsWidth", typeof(Nullable<System.Double>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DimsWidth, 0, 0, 38);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "DimsHeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DimsHeight, 0, 0, 38);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "DimsWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DimsWeight, 0, 0, 38);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "DimsAddWeight", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DimsAddWeight, 0, 0, 0);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "PriorityAlert", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.PriorityAlert, 0, 0, 0);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "PriorityAlertEnhancementType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.PriorityAlertEnhancementType, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "PriorityAlertDetailContent", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.PriorityAlertDetailContent, 1024, 0, 0);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "DryIceWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DryIceWeight, 0, 0, 38);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "ContainsAlcohol", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.ContainsAlcohol, 0, 0, 0);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "DangerousGoodsEnabled", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DangerousGoodsEnabled, 0, 0, 0);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "DangerousGoodsType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DangerousGoodsType, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "DangerousGoodsAccessibilityType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DangerousGoodsAccessibilityType, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "DangerousGoodsCargoAircraftOnly", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DangerousGoodsCargoAircraftOnly, 0, 0, 0);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "DangerousGoodsEmergencyContactPhone", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DangerousGoodsEmergencyContactPhone, 16, 0, 0);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "DangerousGoodsOfferor", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DangerousGoodsOfferor, 128, 0, 0);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "DangerousGoodsPackagingCount", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.DangerousGoodsPackagingCount, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "HazardousMaterialNumber", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.HazardousMaterialNumber, 16, 0, 0);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "HazardousMaterialClass", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.HazardousMaterialClass, 8, 0, 0);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "HazardousMaterialProperName", typeof(System.String), false, false, false, true,  (int)FedExProfilePackageFieldIndex.HazardousMaterialProperName, 64, 0, 0);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "HazardousMaterialPackingGroup", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.HazardousMaterialPackingGroup, 0, 0, 10);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "HazardousMaterialQuantityValue", typeof(Nullable<System.Double>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.HazardousMaterialQuantityValue, 0, 0, 38);
			base.AddElementFieldInfo("FedExProfilePackageEntity", "HazardousMaterialQuanityUnits", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExProfilePackageFieldIndex.HazardousMaterialQuanityUnits, 0, 0, 10);
		}
		/// <summary>Inits FedExShipmentEntity's FieldInfo objects</summary>
		private void InitFedExShipmentEntityInfos()
		{
			base.AddElementFieldInfo("FedExShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)FedExShipmentFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("FedExShipmentEntity", "FedExAccountID", typeof(System.Int64), false, false, false, false,  (int)FedExShipmentFieldIndex.FedExAccountID, 0, 0, 19);
			base.AddElementFieldInfo("FedExShipmentEntity", "MasterFormID", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.MasterFormID, 4, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "Service", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.Service, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "Signature", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.Signature, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "PackagingType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.PackagingType, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "NonStandardContainer", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.NonStandardContainer, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ReferenceCustomer", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ReferenceCustomer, 300, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ReferenceInvoice", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ReferenceInvoice, 300, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ReferencePO", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ReferencePO, 300, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ReferenceShipmentIntegrity", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ReferenceShipmentIntegrity, 300, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "PayorTransportType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.PayorTransportType, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "PayorTransportName", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.PayorTransportName, 60, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "PayorTransportAccount", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.PayorTransportAccount, 12, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "PayorDutiesType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.PayorDutiesType, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "PayorDutiesAccount", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.PayorDutiesAccount, 12, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "PayorDutiesName", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.PayorDutiesName, 60, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "PayorDutiesCountryCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.PayorDutiesCountryCode, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "SaturdayDelivery", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.SaturdayDelivery, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HomeDeliveryType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.HomeDeliveryType, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "HomeDeliveryInstructions", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.HomeDeliveryInstructions, 74, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HomeDeliveryDate", typeof(System.DateTime), false, false, false, false,  (int)FedExShipmentFieldIndex.HomeDeliveryDate, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HomeDeliveryPhone", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.HomeDeliveryPhone, 24, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "FreightInsidePickup", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.FreightInsidePickup, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "FreightInsideDelivery", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.FreightInsideDelivery, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "FreightBookingNumber", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.FreightBookingNumber, 12, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "FreightLoadAndCount", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.FreightLoadAndCount, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "EmailNotifyBroker", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.EmailNotifyBroker, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "EmailNotifySender", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.EmailNotifySender, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "EmailNotifyRecipient", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.EmailNotifyRecipient, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "EmailNotifyOther", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.EmailNotifyOther, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "EmailNotifyOtherAddress", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.EmailNotifyOtherAddress, 100, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "EmailNotifyMessage", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.EmailNotifyMessage, 120, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodEnabled", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.CodEnabled, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodAmount", typeof(System.Decimal), false, false, false, false,  (int)FedExShipmentFieldIndex.CodAmount, 0, 4, 19);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodPaymentType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CodPaymentType, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodAddFreight", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.CodAddFreight, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodOriginID", typeof(System.Int64), false, false, false, false,  (int)FedExShipmentFieldIndex.CodOriginID, 0, 0, 19);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodFirstName", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodFirstName, 30, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodLastName", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodLastName, 30, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodCompany", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodCompany, 35, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodStreet1", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodStreet1, 60, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodStreet2", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodStreet2, 60, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodStreet3", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodStreet3, 60, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodCity", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodCity, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodStateProvCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodStateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodPostalCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodPostalCode, 20, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodCountryCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodCountryCode, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodPhone", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodPhone, 25, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodTrackingNumber", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodTrackingNumber, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodTrackingFormID", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodTrackingFormID, 4, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodTIN", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodTIN, 24, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodChargeBasis", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CodChargeBasis, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "CodAccountNumber", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CodAccountNumber, 25, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "BrokerEnabled", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerEnabled, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "BrokerAccount", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerAccount, 12, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "BrokerFirstName", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerFirstName, 30, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "BrokerLastName", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerLastName, 30, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "BrokerCompany", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerCompany, 35, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "BrokerStreet1", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerStreet1, 60, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "BrokerStreet2", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerStreet2, 60, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "BrokerStreet3", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerStreet3, 60, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "BrokerCity", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerCity, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "BrokerStateProvCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerStateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "BrokerPostalCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerPostalCode, 20, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "BrokerCountryCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerCountryCode, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "BrokerPhone", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerPhone, 25, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "BrokerPhoneExtension", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerPhoneExtension, 8, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "BrokerEmail", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.BrokerEmail, 100, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CustomsAdmissibilityPackaging", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsAdmissibilityPackaging, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "CustomsRecipientTIN", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsRecipientTIN, 24, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CustomsDocumentsOnly", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsDocumentsOnly, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CustomsDocumentsDescription", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsDocumentsDescription, 150, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CustomsExportFilingOption", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsExportFilingOption, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "CustomsAESEEI", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsAESEEI, 100, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CustomsRecipientIdentificationType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsRecipientIdentificationType, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "CustomsRecipientIdentificationValue", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsRecipientIdentificationValue, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CustomsOptionsType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsOptionsType, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "CustomsOptionsDesription", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsOptionsDesription, 32, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoice", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoice, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoiceFileElectronically", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoiceFileElectronically, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoiceTermsOfSale", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoiceTermsOfSale, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoicePurpose", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoicePurpose, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoiceComments", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoiceComments, 200, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoiceFreight", typeof(System.Decimal), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoiceFreight, 0, 4, 19);
			base.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoiceInsurance", typeof(System.Decimal), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoiceInsurance, 0, 4, 19);
			base.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoiceOther", typeof(System.Decimal), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoiceOther, 0, 4, 19);
			base.AddElementFieldInfo("FedExShipmentEntity", "CommercialInvoiceReference", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CommercialInvoiceReference, 300, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ImporterOfRecord", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterOfRecord, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ImporterAccount", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterAccount, 12, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ImporterTIN", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterTIN, 24, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ImporterFirstName", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterFirstName, 30, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ImporterLastName", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterLastName, 30, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ImporterCompany", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterCompany, 35, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ImporterStreet1", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterStreet1, 60, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ImporterStreet2", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterStreet2, 60, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ImporterStreet3", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterStreet3, 60, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ImporterCity", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterCity, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ImporterStateProvCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterStateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ImporterPostalCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterPostalCode, 10, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ImporterCountryCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterCountryCode, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ImporterPhone", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ImporterPhone, 25, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "SmartPostIndicia", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.SmartPostIndicia, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "SmartPostEndorsement", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.SmartPostEndorsement, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "SmartPostConfirmation", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.SmartPostConfirmation, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "SmartPostCustomerManifest", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.SmartPostCustomerManifest, 300, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "SmartPostHubID", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.SmartPostHubID, 10, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "SmartPostUspsApplicationId", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.SmartPostUspsApplicationId, 10, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "DropoffType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.DropoffType, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "OriginResidentialDetermination", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.OriginResidentialDetermination, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "FedExHoldAtLocationEnabled", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.FedExHoldAtLocationEnabled, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldLocationId", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldLocationId, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldLocationType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldLocationType, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldContactId", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldContactId, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldPersonName", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldPersonName, 100, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldTitle", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldTitle, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldCompanyName", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldCompanyName, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldPhoneNumber", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldPhoneNumber, 30, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldPhoneExtension", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldPhoneExtension, 10, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldPagerNumber", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldPagerNumber, 30, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldFaxNumber", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldFaxNumber, 30, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldEmailAddress", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldEmailAddress, 100, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldStreet1", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldStreet1, 250, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldStreet2", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldStreet2, 250, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldStreet3", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldStreet3, 250, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldCity", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldCity, 100, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldStateOrProvinceCode", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldStateOrProvinceCode, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldPostalCode", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldPostalCode, 20, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldUrbanizationCode", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldUrbanizationCode, 20, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldCountryCode", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldCountryCode, 20, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "HoldResidential", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)FedExShipmentFieldIndex.HoldResidential, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CustomsNaftaEnabled", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsNaftaEnabled, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CustomsNaftaPreferenceType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsNaftaPreferenceType, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "CustomsNaftaDeterminationCode", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsNaftaDeterminationCode, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "CustomsNaftaProducerId", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsNaftaProducerId, 20, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "CustomsNaftaNetCostMethod", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.CustomsNaftaNetCostMethod, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "ReturnType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.ReturnType, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "RmaNumber", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.RmaNumber, 30, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "RmaReason", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.RmaReason, 60, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ReturnSaturdayPickup", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.ReturnSaturdayPickup, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "TrafficInArmsLicenseNumber", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.TrafficInArmsLicenseNumber, 32, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "IntlExportDetailType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.IntlExportDetailType, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "IntlExportDetailForeignTradeZoneCode", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.IntlExportDetailForeignTradeZoneCode, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "IntlExportDetailEntryNumber", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.IntlExportDetailEntryNumber, 20, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "IntlExportDetailLicenseOrPermitNumber", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.IntlExportDetailLicenseOrPermitNumber, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "IntlExportDetailLicenseOrPermitExpirationDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)FedExShipmentFieldIndex.IntlExportDetailLicenseOrPermitExpirationDate, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "WeightUnitType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.WeightUnitType, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "LinearUnitType", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.LinearUnitType, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "RequestedLabelFormat", typeof(System.Int32), false, false, false, false,  (int)FedExShipmentFieldIndex.RequestedLabelFormat, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "FimsAirWaybill", typeof(System.String), false, false, false, true,  (int)FedExShipmentFieldIndex.FimsAirWaybill, 50, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "ReturnsClearance", typeof(System.Boolean), false, false, false, false,  (int)FedExShipmentFieldIndex.ReturnsClearance, 0, 0, 0);
			base.AddElementFieldInfo("FedExShipmentEntity", "MaskedData", typeof(Nullable<System.Int32>), false, false, false, true,  (int)FedExShipmentFieldIndex.MaskedData, 0, 0, 10);
			base.AddElementFieldInfo("FedExShipmentEntity", "ReferenceFIMS", typeof(System.String), false, false, false, false,  (int)FedExShipmentFieldIndex.ReferenceFIMS, 300, 0, 0);
		}
		/// <summary>Inits FilterEntity's FieldInfo objects</summary>
		private void InitFilterEntityInfos()
		{
			base.AddElementFieldInfo("FilterEntity", "FilterID", typeof(System.Int64), true, false, true, false,  (int)FilterFieldIndex.FilterID, 0, 0, 19);
			base.AddElementFieldInfo("FilterEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)FilterFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("FilterEntity", "Name", typeof(System.String), false, false, false, false,  (int)FilterFieldIndex.Name, 50, 0, 0);
			base.AddElementFieldInfo("FilterEntity", "FilterTarget", typeof(System.Int32), false, false, false, false,  (int)FilterFieldIndex.FilterTarget, 0, 0, 10);
			base.AddElementFieldInfo("FilterEntity", "IsFolder", typeof(System.Boolean), false, false, false, false,  (int)FilterFieldIndex.IsFolder, 0, 0, 0);
			base.AddElementFieldInfo("FilterEntity", "Definition", typeof(System.String), false, false, false, true,  (int)FilterFieldIndex.Definition, 2147483647, 0, 0);
			base.AddElementFieldInfo("FilterEntity", "State", typeof(System.Byte), false, false, false, false,  (int)FilterFieldIndex.State, 0, 0, 3);
		}
		/// <summary>Inits FilterLayoutEntity's FieldInfo objects</summary>
		private void InitFilterLayoutEntityInfos()
		{
			base.AddElementFieldInfo("FilterLayoutEntity", "FilterLayoutID", typeof(System.Int64), true, false, true, false,  (int)FilterLayoutFieldIndex.FilterLayoutID, 0, 0, 19);
			base.AddElementFieldInfo("FilterLayoutEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)FilterLayoutFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("FilterLayoutEntity", "UserID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)FilterLayoutFieldIndex.UserID, 0, 0, 19);
			base.AddElementFieldInfo("FilterLayoutEntity", "FilterTarget", typeof(System.Int32), false, false, false, false,  (int)FilterLayoutFieldIndex.FilterTarget, 0, 0, 10);
			base.AddElementFieldInfo("FilterLayoutEntity", "FilterNodeID", typeof(System.Int64), false, true, false, false,  (int)FilterLayoutFieldIndex.FilterNodeID, 0, 0, 19);
		}
		/// <summary>Inits FilterNodeEntity's FieldInfo objects</summary>
		private void InitFilterNodeEntityInfos()
		{
			base.AddElementFieldInfo("FilterNodeEntity", "FilterNodeID", typeof(System.Int64), true, false, true, false,  (int)FilterNodeFieldIndex.FilterNodeID, 0, 0, 19);
			base.AddElementFieldInfo("FilterNodeEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)FilterNodeFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("FilterNodeEntity", "ParentFilterNodeID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)FilterNodeFieldIndex.ParentFilterNodeID, 0, 0, 19);
			base.AddElementFieldInfo("FilterNodeEntity", "FilterSequenceID", typeof(System.Int64), false, true, false, false,  (int)FilterNodeFieldIndex.FilterSequenceID, 0, 0, 19);
			base.AddElementFieldInfo("FilterNodeEntity", "FilterNodeContentID", typeof(System.Int64), false, true, false, false,  (int)FilterNodeFieldIndex.FilterNodeContentID, 0, 0, 19);
			base.AddElementFieldInfo("FilterNodeEntity", "Created", typeof(System.DateTime), false, false, false, false,  (int)FilterNodeFieldIndex.Created, 0, 0, 0);
			base.AddElementFieldInfo("FilterNodeEntity", "Purpose", typeof(System.Int32), false, false, false, false,  (int)FilterNodeFieldIndex.Purpose, 0, 0, 10);
		}
		/// <summary>Inits FilterNodeColumnSettingsEntity's FieldInfo objects</summary>
		private void InitFilterNodeColumnSettingsEntityInfos()
		{
			base.AddElementFieldInfo("FilterNodeColumnSettingsEntity", "FilterNodeColumnSettingsID", typeof(System.Int64), true, false, true, false,  (int)FilterNodeColumnSettingsFieldIndex.FilterNodeColumnSettingsID, 0, 0, 19);
			base.AddElementFieldInfo("FilterNodeColumnSettingsEntity", "UserID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)FilterNodeColumnSettingsFieldIndex.UserID, 0, 0, 19);
			base.AddElementFieldInfo("FilterNodeColumnSettingsEntity", "FilterNodeID", typeof(System.Int64), false, true, false, false,  (int)FilterNodeColumnSettingsFieldIndex.FilterNodeID, 0, 0, 19);
			base.AddElementFieldInfo("FilterNodeColumnSettingsEntity", "Inherit", typeof(System.Boolean), false, false, false, false,  (int)FilterNodeColumnSettingsFieldIndex.Inherit, 0, 0, 0);
			base.AddElementFieldInfo("FilterNodeColumnSettingsEntity", "GridColumnLayoutID", typeof(System.Int64), false, true, false, false,  (int)FilterNodeColumnSettingsFieldIndex.GridColumnLayoutID, 0, 0, 19);
		}
		/// <summary>Inits FilterNodeContentEntity's FieldInfo objects</summary>
		private void InitFilterNodeContentEntityInfos()
		{
			base.AddElementFieldInfo("FilterNodeContentEntity", "FilterNodeContentID", typeof(System.Int64), true, false, true, false,  (int)FilterNodeContentFieldIndex.FilterNodeContentID, 0, 0, 19);
			base.AddElementFieldInfo("FilterNodeContentEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)FilterNodeContentFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("FilterNodeContentEntity", "CountVersion", typeof(System.Int64), false, false, false, false,  (int)FilterNodeContentFieldIndex.CountVersion, 0, 0, 19);
			base.AddElementFieldInfo("FilterNodeContentEntity", "Status", typeof(System.Int16), false, false, false, false,  (int)FilterNodeContentFieldIndex.Status, 0, 0, 5);
			base.AddElementFieldInfo("FilterNodeContentEntity", "InitialCalculation", typeof(System.String), false, false, false, false,  (int)FilterNodeContentFieldIndex.InitialCalculation, 2147483647, 0, 0);
			base.AddElementFieldInfo("FilterNodeContentEntity", "UpdateCalculation", typeof(System.String), false, false, false, false,  (int)FilterNodeContentFieldIndex.UpdateCalculation, 2147483647, 0, 0);
			base.AddElementFieldInfo("FilterNodeContentEntity", "ColumnMask", typeof(System.Byte[]), false, false, false, false,  (int)FilterNodeContentFieldIndex.ColumnMask, 100, 0, 0);
			base.AddElementFieldInfo("FilterNodeContentEntity", "JoinMask", typeof(System.Int32), false, false, false, false,  (int)FilterNodeContentFieldIndex.JoinMask, 0, 0, 10);
			base.AddElementFieldInfo("FilterNodeContentEntity", "Cost", typeof(System.Int32), false, false, false, false,  (int)FilterNodeContentFieldIndex.Cost, 0, 0, 10);
			base.AddElementFieldInfo("FilterNodeContentEntity", "Count", typeof(System.Int32), false, false, false, false,  (int)FilterNodeContentFieldIndex.Count, 0, 0, 10);
		}
		/// <summary>Inits FilterNodeContentDetailEntity's FieldInfo objects</summary>
		private void InitFilterNodeContentDetailEntityInfos()
		{
			base.AddElementFieldInfo("FilterNodeContentDetailEntity", "FilterNodeContentID", typeof(System.Int64), false, true, false, false,  (int)FilterNodeContentDetailFieldIndex.FilterNodeContentID, 0, 0, 19);
			base.AddElementFieldInfo("FilterNodeContentDetailEntity", "ObjectID", typeof(System.Int64), false, false, false, false,  (int)FilterNodeContentDetailFieldIndex.ObjectID, 0, 0, 19);
		}
		/// <summary>Inits FilterSequenceEntity's FieldInfo objects</summary>
		private void InitFilterSequenceEntityInfos()
		{
			base.AddElementFieldInfo("FilterSequenceEntity", "FilterSequenceID", typeof(System.Int64), true, false, true, false,  (int)FilterSequenceFieldIndex.FilterSequenceID, 0, 0, 19);
			base.AddElementFieldInfo("FilterSequenceEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)FilterSequenceFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("FilterSequenceEntity", "ParentFilterID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)FilterSequenceFieldIndex.ParentFilterID, 0, 0, 19);
			base.AddElementFieldInfo("FilterSequenceEntity", "FilterID", typeof(System.Int64), false, true, false, false,  (int)FilterSequenceFieldIndex.FilterID, 0, 0, 19);
			base.AddElementFieldInfo("FilterSequenceEntity", "Position", typeof(System.Int32), false, false, false, false,  (int)FilterSequenceFieldIndex.Position, 0, 0, 10);
		}
		/// <summary>Inits FtpAccountEntity's FieldInfo objects</summary>
		private void InitFtpAccountEntityInfos()
		{
			base.AddElementFieldInfo("FtpAccountEntity", "FtpAccountID", typeof(System.Int64), true, false, true, false,  (int)FtpAccountFieldIndex.FtpAccountID, 0, 0, 19);
			base.AddElementFieldInfo("FtpAccountEntity", "Host", typeof(System.String), false, false, false, false,  (int)FtpAccountFieldIndex.Host, 100, 0, 0);
			base.AddElementFieldInfo("FtpAccountEntity", "Username", typeof(System.String), false, false, false, false,  (int)FtpAccountFieldIndex.Username, 50, 0, 0);
			base.AddElementFieldInfo("FtpAccountEntity", "Password", typeof(System.String), false, false, false, false,  (int)FtpAccountFieldIndex.Password, 50, 0, 0);
			base.AddElementFieldInfo("FtpAccountEntity", "Port", typeof(System.Int32), false, false, false, false,  (int)FtpAccountFieldIndex.Port, 0, 0, 10);
			base.AddElementFieldInfo("FtpAccountEntity", "SecurityType", typeof(System.Int32), false, false, false, false,  (int)FtpAccountFieldIndex.SecurityType, 0, 0, 10);
			base.AddElementFieldInfo("FtpAccountEntity", "Passive", typeof(System.Boolean), false, false, false, false,  (int)FtpAccountFieldIndex.Passive, 0, 0, 0);
			base.AddElementFieldInfo("FtpAccountEntity", "InternalOwnerID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)FtpAccountFieldIndex.InternalOwnerID, 0, 0, 19);
		}
		/// <summary>Inits GenericFileStoreEntity's FieldInfo objects</summary>
		private void InitGenericFileStoreEntityInfos()
		{
			base.AddElementFieldInfo("GenericFileStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)GenericFileStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("GenericFileStoreEntity", "FileFormat", typeof(System.Int32), false, false, false, false,  (int)GenericFileStoreFieldIndex.FileFormat, 0, 0, 10);
			base.AddElementFieldInfo("GenericFileStoreEntity", "FileSource", typeof(System.Int32), false, false, false, false,  (int)GenericFileStoreFieldIndex.FileSource, 0, 0, 10);
			base.AddElementFieldInfo("GenericFileStoreEntity", "DiskFolder", typeof(System.String), false, false, false, false,  (int)GenericFileStoreFieldIndex.DiskFolder, 355, 0, 0);
			base.AddElementFieldInfo("GenericFileStoreEntity", "FtpAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)GenericFileStoreFieldIndex.FtpAccountID, 0, 0, 19);
			base.AddElementFieldInfo("GenericFileStoreEntity", "FtpFolder", typeof(System.String), false, false, false, false,  (int)GenericFileStoreFieldIndex.FtpFolder, 355, 0, 0);
			base.AddElementFieldInfo("GenericFileStoreEntity", "EmailAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)GenericFileStoreFieldIndex.EmailAccountID, 0, 0, 19);
			base.AddElementFieldInfo("GenericFileStoreEntity", "EmailIncomingFolder", typeof(System.String), false, false, false, false,  (int)GenericFileStoreFieldIndex.EmailIncomingFolder, 100, 0, 0);
			base.AddElementFieldInfo("GenericFileStoreEntity", "EmailFolderValidityID", typeof(System.Int64), false, false, false, false,  (int)GenericFileStoreFieldIndex.EmailFolderValidityID, 0, 0, 19);
			base.AddElementFieldInfo("GenericFileStoreEntity", "EmailFolderLastMessageID", typeof(System.Int64), false, false, false, false,  (int)GenericFileStoreFieldIndex.EmailFolderLastMessageID, 0, 0, 19);
			base.AddElementFieldInfo("GenericFileStoreEntity", "EmailOnlyUnread", typeof(System.Boolean), false, false, false, false,  (int)GenericFileStoreFieldIndex.EmailOnlyUnread, 0, 0, 0);
			base.AddElementFieldInfo("GenericFileStoreEntity", "NamePatternMatch", typeof(System.String), false, false, false, true,  (int)GenericFileStoreFieldIndex.NamePatternMatch, 50, 0, 0);
			base.AddElementFieldInfo("GenericFileStoreEntity", "NamePatternSkip", typeof(System.String), false, false, false, true,  (int)GenericFileStoreFieldIndex.NamePatternSkip, 50, 0, 0);
			base.AddElementFieldInfo("GenericFileStoreEntity", "SuccessAction", typeof(System.Int32), false, false, false, false,  (int)GenericFileStoreFieldIndex.SuccessAction, 0, 0, 10);
			base.AddElementFieldInfo("GenericFileStoreEntity", "SuccessMoveFolder", typeof(System.String), false, false, false, false,  (int)GenericFileStoreFieldIndex.SuccessMoveFolder, 355, 0, 0);
			base.AddElementFieldInfo("GenericFileStoreEntity", "ErrorAction", typeof(System.Int32), false, false, false, false,  (int)GenericFileStoreFieldIndex.ErrorAction, 0, 0, 10);
			base.AddElementFieldInfo("GenericFileStoreEntity", "ErrorMoveFolder", typeof(System.String), false, false, false, false,  (int)GenericFileStoreFieldIndex.ErrorMoveFolder, 355, 0, 0);
			base.AddElementFieldInfo("GenericFileStoreEntity", "XmlXsltFileName", typeof(System.String), false, false, false, true,  (int)GenericFileStoreFieldIndex.XmlXsltFileName, 355, 0, 0);
			base.AddElementFieldInfo("GenericFileStoreEntity", "XmlXsltContent", typeof(System.String), false, false, false, true,  (int)GenericFileStoreFieldIndex.XmlXsltContent, 1073741823, 0, 0);
			base.AddElementFieldInfo("GenericFileStoreEntity", "FlatImportMap", typeof(System.String), false, false, false, false,  (int)GenericFileStoreFieldIndex.FlatImportMap, 1073741823, 0, 0);
		}
		/// <summary>Inits GenericModuleStoreEntity's FieldInfo objects</summary>
		private void InitGenericModuleStoreEntityInfos()
		{
			base.AddElementFieldInfo("GenericModuleStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)GenericModuleStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleUsername", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleUsername, 50, 0, 0);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModulePassword", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModulePassword, 80, 0, 0);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleUrl", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleUrl, 350, 0, 0);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleVersion", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleVersion, 20, 0, 0);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModulePlatform", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModulePlatform, 50, 0, 0);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleDeveloper", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleDeveloper, 50, 0, 0);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleOnlineStoreCode", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleOnlineStoreCode, 50, 0, 0);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleStatusCodes", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleStatusCodes, 2147483647, 0, 0);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleDownloadPageSize", typeof(System.Int32), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleDownloadPageSize, 0, 0, 10);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleRequestTimeout", typeof(System.Int32), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleRequestTimeout, 0, 0, 10);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleDownloadStrategy", typeof(System.Int32), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleDownloadStrategy, 0, 0, 10);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleOnlineStatusSupport", typeof(System.Int32), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleOnlineStatusSupport, 0, 0, 10);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleOnlineStatusDataType", typeof(System.Int32), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleOnlineStatusDataType, 0, 0, 10);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleOnlineCustomerSupport", typeof(System.Boolean), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleOnlineCustomerSupport, 0, 0, 0);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleOnlineCustomerDataType", typeof(System.Int32), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleOnlineCustomerDataType, 0, 0, 10);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleOnlineShipmentDetails", typeof(System.Boolean), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleOnlineShipmentDetails, 0, 0, 0);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleHttpExpect100Continue", typeof(System.Boolean), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleHttpExpect100Continue, 0, 0, 0);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "ModuleResponseEncoding", typeof(System.Int32), false, false, false, false,  (int)GenericModuleStoreFieldIndex.ModuleResponseEncoding, 0, 0, 10);
			base.AddElementFieldInfo("GenericModuleStoreEntity", "SchemaVersion", typeof(System.String), false, false, false, false,  (int)GenericModuleStoreFieldIndex.SchemaVersion, 20, 0, 0);
		}
		/// <summary>Inits GridColumnFormatEntity's FieldInfo objects</summary>
		private void InitGridColumnFormatEntityInfos()
		{
			base.AddElementFieldInfo("GridColumnFormatEntity", "GridColumnFormatID", typeof(System.Int64), true, false, true, false,  (int)GridColumnFormatFieldIndex.GridColumnFormatID, 0, 0, 19);
			base.AddElementFieldInfo("GridColumnFormatEntity", "UserID", typeof(System.Int64), false, true, false, false,  (int)GridColumnFormatFieldIndex.UserID, 0, 0, 19);
			base.AddElementFieldInfo("GridColumnFormatEntity", "ColumnGuid", typeof(System.Guid), false, false, false, false,  (int)GridColumnFormatFieldIndex.ColumnGuid, 0, 0, 0);
			base.AddElementFieldInfo("GridColumnFormatEntity", "Settings", typeof(System.String), false, false, false, false,  (int)GridColumnFormatFieldIndex.Settings, 2147483647, 0, 0);
		}
		/// <summary>Inits GridColumnLayoutEntity's FieldInfo objects</summary>
		private void InitGridColumnLayoutEntityInfos()
		{
			base.AddElementFieldInfo("GridColumnLayoutEntity", "GridColumnLayoutID", typeof(System.Int64), true, false, true, false,  (int)GridColumnLayoutFieldIndex.GridColumnLayoutID, 0, 0, 19);
			base.AddElementFieldInfo("GridColumnLayoutEntity", "DefinitionSet", typeof(System.Int32), false, false, false, false,  (int)GridColumnLayoutFieldIndex.DefinitionSet, 0, 0, 10);
			base.AddElementFieldInfo("GridColumnLayoutEntity", "DefaultSortColumnGuid", typeof(System.Guid), false, false, false, false,  (int)GridColumnLayoutFieldIndex.DefaultSortColumnGuid, 0, 0, 0);
			base.AddElementFieldInfo("GridColumnLayoutEntity", "DefaultSortOrder", typeof(System.Int32), false, false, false, false,  (int)GridColumnLayoutFieldIndex.DefaultSortOrder, 0, 0, 10);
			base.AddElementFieldInfo("GridColumnLayoutEntity", "LastSortColumnGuid", typeof(System.Guid), false, false, false, false,  (int)GridColumnLayoutFieldIndex.LastSortColumnGuid, 0, 0, 0);
			base.AddElementFieldInfo("GridColumnLayoutEntity", "LastSortOrder", typeof(System.Int32), false, false, false, false,  (int)GridColumnLayoutFieldIndex.LastSortOrder, 0, 0, 10);
			base.AddElementFieldInfo("GridColumnLayoutEntity", "DetailViewSettings", typeof(System.String), false, false, false, true,  (int)GridColumnLayoutFieldIndex.DetailViewSettings, 2147483647, 0, 0);
		}
		/// <summary>Inits GridColumnPositionEntity's FieldInfo objects</summary>
		private void InitGridColumnPositionEntityInfos()
		{
			base.AddElementFieldInfo("GridColumnPositionEntity", "GridColumnPositionID", typeof(System.Int64), true, false, true, false,  (int)GridColumnPositionFieldIndex.GridColumnPositionID, 0, 0, 19);
			base.AddElementFieldInfo("GridColumnPositionEntity", "GridColumnLayoutID", typeof(System.Int64), false, true, false, false,  (int)GridColumnPositionFieldIndex.GridColumnLayoutID, 0, 0, 19);
			base.AddElementFieldInfo("GridColumnPositionEntity", "ColumnGuid", typeof(System.Guid), false, false, false, false,  (int)GridColumnPositionFieldIndex.ColumnGuid, 0, 0, 0);
			base.AddElementFieldInfo("GridColumnPositionEntity", "Visible", typeof(System.Boolean), false, false, false, false,  (int)GridColumnPositionFieldIndex.Visible, 0, 0, 0);
			base.AddElementFieldInfo("GridColumnPositionEntity", "Width", typeof(System.Int32), false, false, false, false,  (int)GridColumnPositionFieldIndex.Width, 0, 0, 10);
			base.AddElementFieldInfo("GridColumnPositionEntity", "Position", typeof(System.Int32), false, false, false, false,  (int)GridColumnPositionFieldIndex.Position, 0, 0, 10);
		}
		/// <summary>Inits GrouponOrderEntity's FieldInfo objects</summary>
		private void InitGrouponOrderEntityInfos()
		{
			base.AddElementFieldInfo("GrouponOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)GrouponOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("GrouponOrderEntity", "GrouponOrderID", typeof(System.String), false, false, false, false,  (int)GrouponOrderFieldIndex.GrouponOrderID, 50, 0, 0);
		}
		/// <summary>Inits GrouponOrderItemEntity's FieldInfo objects</summary>
		private void InitGrouponOrderItemEntityInfos()
		{
			base.AddElementFieldInfo("GrouponOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, false, false,  (int)GrouponOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			base.AddElementFieldInfo("GrouponOrderItemEntity", "Permalink", typeof(System.String), false, false, false, false,  (int)GrouponOrderItemFieldIndex.Permalink, 255, 0, 0);
			base.AddElementFieldInfo("GrouponOrderItemEntity", "ChannelSKUProvided", typeof(System.String), false, false, false, false,  (int)GrouponOrderItemFieldIndex.ChannelSKUProvided, 255, 0, 0);
			base.AddElementFieldInfo("GrouponOrderItemEntity", "FulfillmentLineItemID", typeof(System.String), false, false, false, false,  (int)GrouponOrderItemFieldIndex.FulfillmentLineItemID, 255, 0, 0);
			base.AddElementFieldInfo("GrouponOrderItemEntity", "BomSKU", typeof(System.String), false, false, false, false,  (int)GrouponOrderItemFieldIndex.BomSKU, 255, 0, 0);
			base.AddElementFieldInfo("GrouponOrderItemEntity", "GrouponLineItemID", typeof(System.String), false, false, false, false,  (int)GrouponOrderItemFieldIndex.GrouponLineItemID, 255, 0, 0);
		}
		/// <summary>Inits GrouponStoreEntity's FieldInfo objects</summary>
		private void InitGrouponStoreEntityInfos()
		{
			base.AddElementFieldInfo("GrouponStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)GrouponStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("GrouponStoreEntity", "SupplierID", typeof(System.String), false, false, false, false,  (int)GrouponStoreFieldIndex.SupplierID, 255, 0, 0);
			base.AddElementFieldInfo("GrouponStoreEntity", "Token", typeof(System.String), false, false, false, false,  (int)GrouponStoreFieldIndex.Token, 255, 0, 0);
		}
		/// <summary>Inits InfopiaOrderItemEntity's FieldInfo objects</summary>
		private void InitInfopiaOrderItemEntityInfos()
		{
			base.AddElementFieldInfo("InfopiaOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, false, false,  (int)InfopiaOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			base.AddElementFieldInfo("InfopiaOrderItemEntity", "Marketplace", typeof(System.String), false, false, false, false,  (int)InfopiaOrderItemFieldIndex.Marketplace, 50, 0, 0);
			base.AddElementFieldInfo("InfopiaOrderItemEntity", "MarketplaceItemID", typeof(System.String), false, false, false, false,  (int)InfopiaOrderItemFieldIndex.MarketplaceItemID, 20, 0, 0);
			base.AddElementFieldInfo("InfopiaOrderItemEntity", "BuyerID", typeof(System.String), false, false, false, false,  (int)InfopiaOrderItemFieldIndex.BuyerID, 50, 0, 0);
		}
		/// <summary>Inits InfopiaStoreEntity's FieldInfo objects</summary>
		private void InitInfopiaStoreEntityInfos()
		{
			base.AddElementFieldInfo("InfopiaStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)InfopiaStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("InfopiaStoreEntity", "ApiToken", typeof(System.String), false, false, false, false,  (int)InfopiaStoreFieldIndex.ApiToken, 128, 0, 0);
		}
		/// <summary>Inits InsurancePolicyEntity's FieldInfo objects</summary>
		private void InitInsurancePolicyEntityInfos()
		{
			base.AddElementFieldInfo("InsurancePolicyEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)InsurancePolicyFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("InsurancePolicyEntity", "InsureShipStoreName", typeof(System.String), false, false, false, false,  (int)InsurancePolicyFieldIndex.InsureShipStoreName, 75, 0, 0);
			base.AddElementFieldInfo("InsurancePolicyEntity", "CreatedWithApi", typeof(System.Boolean), false, false, false, false,  (int)InsurancePolicyFieldIndex.CreatedWithApi, 0, 0, 0);
			base.AddElementFieldInfo("InsurancePolicyEntity", "ItemName", typeof(System.String), false, false, false, true,  (int)InsurancePolicyFieldIndex.ItemName, 255, 0, 0);
			base.AddElementFieldInfo("InsurancePolicyEntity", "Description", typeof(System.String), false, false, false, true,  (int)InsurancePolicyFieldIndex.Description, 255, 0, 0);
			base.AddElementFieldInfo("InsurancePolicyEntity", "ClaimType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)InsurancePolicyFieldIndex.ClaimType, 0, 0, 10);
			base.AddElementFieldInfo("InsurancePolicyEntity", "DamageValue", typeof(Nullable<System.Decimal>), false, false, false, true,  (int)InsurancePolicyFieldIndex.DamageValue, 0, 4, 19);
			base.AddElementFieldInfo("InsurancePolicyEntity", "SubmissionDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)InsurancePolicyFieldIndex.SubmissionDate, 0, 0, 0);
			base.AddElementFieldInfo("InsurancePolicyEntity", "ClaimID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)InsurancePolicyFieldIndex.ClaimID, 0, 0, 19);
			base.AddElementFieldInfo("InsurancePolicyEntity", "EmailAddress", typeof(System.String), false, false, false, true,  (int)InsurancePolicyFieldIndex.EmailAddress, 100, 0, 0);
		}
		/// <summary>Inits IParcelAccountEntity's FieldInfo objects</summary>
		private void InitIParcelAccountEntityInfos()
		{
			base.AddElementFieldInfo("IParcelAccountEntity", "IParcelAccountID", typeof(System.Int64), true, false, true, false,  (int)IParcelAccountFieldIndex.IParcelAccountID, 0, 0, 19);
			base.AddElementFieldInfo("IParcelAccountEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)IParcelAccountFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("IParcelAccountEntity", "Username", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Username, 50, 0, 0);
			base.AddElementFieldInfo("IParcelAccountEntity", "Password", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Password, 50, 0, 0);
			base.AddElementFieldInfo("IParcelAccountEntity", "Description", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Description, 50, 0, 0);
			base.AddElementFieldInfo("IParcelAccountEntity", "FirstName", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.FirstName, 30, 0, 0);
			base.AddElementFieldInfo("IParcelAccountEntity", "MiddleName", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.MiddleName, 30, 0, 0);
			base.AddElementFieldInfo("IParcelAccountEntity", "LastName", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.LastName, 30, 0, 0);
			base.AddElementFieldInfo("IParcelAccountEntity", "Company", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Company, 30, 0, 0);
			base.AddElementFieldInfo("IParcelAccountEntity", "Street1", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Street1, 50, 0, 0);
			base.AddElementFieldInfo("IParcelAccountEntity", "Street2", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Street2, 50, 0, 0);
			base.AddElementFieldInfo("IParcelAccountEntity", "City", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.City, 30, 0, 0);
			base.AddElementFieldInfo("IParcelAccountEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.StateProvCode, 30, 0, 0);
			base.AddElementFieldInfo("IParcelAccountEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.PostalCode, 20, 0, 0);
			base.AddElementFieldInfo("IParcelAccountEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.CountryCode, 50, 0, 0);
			base.AddElementFieldInfo("IParcelAccountEntity", "Phone", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Phone, 25, 0, 0);
			base.AddElementFieldInfo("IParcelAccountEntity", "Email", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Email, 100, 0, 0);
			base.AddElementFieldInfo("IParcelAccountEntity", "Website", typeof(System.String), false, false, false, false,  (int)IParcelAccountFieldIndex.Website, 50, 0, 0);
		}
		/// <summary>Inits IParcelPackageEntity's FieldInfo objects</summary>
		private void InitIParcelPackageEntityInfos()
		{
			base.AddElementFieldInfo("IParcelPackageEntity", "IParcelPackageID", typeof(System.Int64), true, false, true, false,  (int)IParcelPackageFieldIndex.IParcelPackageID, 0, 0, 19);
			base.AddElementFieldInfo("IParcelPackageEntity", "ShipmentID", typeof(System.Int64), false, true, false, false,  (int)IParcelPackageFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("IParcelPackageEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)IParcelPackageFieldIndex.Weight, 0, 0, 38);
			base.AddElementFieldInfo("IParcelPackageEntity", "DimsProfileID", typeof(System.Int64), false, false, false, false,  (int)IParcelPackageFieldIndex.DimsProfileID, 0, 0, 19);
			base.AddElementFieldInfo("IParcelPackageEntity", "DimsLength", typeof(System.Double), false, false, false, false,  (int)IParcelPackageFieldIndex.DimsLength, 0, 0, 38);
			base.AddElementFieldInfo("IParcelPackageEntity", "DimsWidth", typeof(System.Double), false, false, false, false,  (int)IParcelPackageFieldIndex.DimsWidth, 0, 0, 38);
			base.AddElementFieldInfo("IParcelPackageEntity", "DimsHeight", typeof(System.Double), false, false, false, false,  (int)IParcelPackageFieldIndex.DimsHeight, 0, 0, 38);
			base.AddElementFieldInfo("IParcelPackageEntity", "DimsAddWeight", typeof(System.Boolean), false, false, false, false,  (int)IParcelPackageFieldIndex.DimsAddWeight, 0, 0, 0);
			base.AddElementFieldInfo("IParcelPackageEntity", "DimsWeight", typeof(System.Double), false, false, false, false,  (int)IParcelPackageFieldIndex.DimsWeight, 0, 0, 38);
			base.AddElementFieldInfo("IParcelPackageEntity", "Insurance", typeof(System.Boolean), false, false, false, false,  (int)IParcelPackageFieldIndex.Insurance, 0, 0, 0);
			base.AddElementFieldInfo("IParcelPackageEntity", "InsuranceValue", typeof(System.Decimal), false, false, false, false,  (int)IParcelPackageFieldIndex.InsuranceValue, 0, 4, 19);
			base.AddElementFieldInfo("IParcelPackageEntity", "InsurancePennyOne", typeof(System.Boolean), false, false, false, false,  (int)IParcelPackageFieldIndex.InsurancePennyOne, 0, 0, 0);
			base.AddElementFieldInfo("IParcelPackageEntity", "DeclaredValue", typeof(System.Decimal), false, false, false, false,  (int)IParcelPackageFieldIndex.DeclaredValue, 0, 4, 19);
			base.AddElementFieldInfo("IParcelPackageEntity", "TrackingNumber", typeof(System.String), false, false, false, false,  (int)IParcelPackageFieldIndex.TrackingNumber, 50, 0, 0);
			base.AddElementFieldInfo("IParcelPackageEntity", "ParcelNumber", typeof(System.String), false, false, false, false,  (int)IParcelPackageFieldIndex.ParcelNumber, 50, 0, 0);
			base.AddElementFieldInfo("IParcelPackageEntity", "SkuAndQuantities", typeof(System.String), false, false, false, false,  (int)IParcelPackageFieldIndex.SkuAndQuantities, 500, 0, 0);
		}
		/// <summary>Inits IParcelProfileEntity's FieldInfo objects</summary>
		private void InitIParcelProfileEntityInfos()
		{
			base.AddElementFieldInfo("IParcelProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)IParcelProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			base.AddElementFieldInfo("IParcelProfileEntity", "IParcelAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)IParcelProfileFieldIndex.IParcelAccountID, 0, 0, 19);
			base.AddElementFieldInfo("IParcelProfileEntity", "Service", typeof(Nullable<System.Int32>), false, false, false, true,  (int)IParcelProfileFieldIndex.Service, 0, 0, 10);
			base.AddElementFieldInfo("IParcelProfileEntity", "Reference", typeof(System.String), false, false, false, true,  (int)IParcelProfileFieldIndex.Reference, 300, 0, 0);
			base.AddElementFieldInfo("IParcelProfileEntity", "TrackByEmail", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)IParcelProfileFieldIndex.TrackByEmail, 0, 0, 0);
			base.AddElementFieldInfo("IParcelProfileEntity", "TrackBySMS", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)IParcelProfileFieldIndex.TrackBySMS, 0, 0, 0);
			base.AddElementFieldInfo("IParcelProfileEntity", "IsDeliveryDutyPaid", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)IParcelProfileFieldIndex.IsDeliveryDutyPaid, 0, 0, 0);
			base.AddElementFieldInfo("IParcelProfileEntity", "SkuAndQuantities", typeof(System.String), false, false, false, true,  (int)IParcelProfileFieldIndex.SkuAndQuantities, 500, 0, 0);
		}
		/// <summary>Inits IParcelProfilePackageEntity's FieldInfo objects</summary>
		private void InitIParcelProfilePackageEntityInfos()
		{
			base.AddElementFieldInfo("IParcelProfilePackageEntity", "IParcelProfilePackageID", typeof(System.Int64), true, false, true, false,  (int)IParcelProfilePackageFieldIndex.IParcelProfilePackageID, 0, 0, 19);
			base.AddElementFieldInfo("IParcelProfilePackageEntity", "ShippingProfileID", typeof(System.Int64), false, true, false, false,  (int)IParcelProfilePackageFieldIndex.ShippingProfileID, 0, 0, 19);
			base.AddElementFieldInfo("IParcelProfilePackageEntity", "Weight", typeof(Nullable<System.Double>), false, false, false, true,  (int)IParcelProfilePackageFieldIndex.Weight, 0, 0, 38);
			base.AddElementFieldInfo("IParcelProfilePackageEntity", "DimsProfileID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)IParcelProfilePackageFieldIndex.DimsProfileID, 0, 0, 19);
			base.AddElementFieldInfo("IParcelProfilePackageEntity", "DimsLength", typeof(Nullable<System.Double>), false, false, false, true,  (int)IParcelProfilePackageFieldIndex.DimsLength, 0, 0, 38);
			base.AddElementFieldInfo("IParcelProfilePackageEntity", "DimsWidth", typeof(Nullable<System.Double>), false, false, false, true,  (int)IParcelProfilePackageFieldIndex.DimsWidth, 0, 0, 38);
			base.AddElementFieldInfo("IParcelProfilePackageEntity", "DimsHeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)IParcelProfilePackageFieldIndex.DimsHeight, 0, 0, 38);
			base.AddElementFieldInfo("IParcelProfilePackageEntity", "DimsWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)IParcelProfilePackageFieldIndex.DimsWeight, 0, 0, 38);
			base.AddElementFieldInfo("IParcelProfilePackageEntity", "DimsAddWeight", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)IParcelProfilePackageFieldIndex.DimsAddWeight, 0, 0, 0);
		}
		/// <summary>Inits IParcelShipmentEntity's FieldInfo objects</summary>
		private void InitIParcelShipmentEntityInfos()
		{
			base.AddElementFieldInfo("IParcelShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)IParcelShipmentFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("IParcelShipmentEntity", "IParcelAccountID", typeof(System.Int64), false, false, false, false,  (int)IParcelShipmentFieldIndex.IParcelAccountID, 0, 0, 19);
			base.AddElementFieldInfo("IParcelShipmentEntity", "Service", typeof(System.Int32), false, false, false, false,  (int)IParcelShipmentFieldIndex.Service, 0, 0, 10);
			base.AddElementFieldInfo("IParcelShipmentEntity", "Reference", typeof(System.String), false, false, false, false,  (int)IParcelShipmentFieldIndex.Reference, 300, 0, 0);
			base.AddElementFieldInfo("IParcelShipmentEntity", "TrackByEmail", typeof(System.Boolean), false, false, false, false,  (int)IParcelShipmentFieldIndex.TrackByEmail, 0, 0, 0);
			base.AddElementFieldInfo("IParcelShipmentEntity", "TrackBySMS", typeof(System.Boolean), false, false, false, false,  (int)IParcelShipmentFieldIndex.TrackBySMS, 0, 0, 0);
			base.AddElementFieldInfo("IParcelShipmentEntity", "IsDeliveryDutyPaid", typeof(System.Boolean), false, false, false, false,  (int)IParcelShipmentFieldIndex.IsDeliveryDutyPaid, 0, 0, 0);
			base.AddElementFieldInfo("IParcelShipmentEntity", "RequestedLabelFormat", typeof(System.Int32), false, false, false, false,  (int)IParcelShipmentFieldIndex.RequestedLabelFormat, 0, 0, 10);
		}
		/// <summary>Inits LabelSheetEntity's FieldInfo objects</summary>
		private void InitLabelSheetEntityInfos()
		{
			base.AddElementFieldInfo("LabelSheetEntity", "LabelSheetID", typeof(System.Int64), true, false, true, false,  (int)LabelSheetFieldIndex.LabelSheetID, 0, 0, 19);
			base.AddElementFieldInfo("LabelSheetEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)LabelSheetFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("LabelSheetEntity", "Name", typeof(System.String), false, false, false, false,  (int)LabelSheetFieldIndex.Name, 100, 0, 0);
			base.AddElementFieldInfo("LabelSheetEntity", "PaperSizeHeight", typeof(System.Double), false, false, false, false,  (int)LabelSheetFieldIndex.PaperSizeHeight, 0, 0, 38);
			base.AddElementFieldInfo("LabelSheetEntity", "PaperSizeWidth", typeof(System.Double), false, false, false, false,  (int)LabelSheetFieldIndex.PaperSizeWidth, 0, 0, 38);
			base.AddElementFieldInfo("LabelSheetEntity", "MarginTop", typeof(System.Double), false, false, false, false,  (int)LabelSheetFieldIndex.MarginTop, 0, 0, 38);
			base.AddElementFieldInfo("LabelSheetEntity", "MarginLeft", typeof(System.Double), false, false, false, false,  (int)LabelSheetFieldIndex.MarginLeft, 0, 0, 38);
			base.AddElementFieldInfo("LabelSheetEntity", "LabelHeight", typeof(System.Double), false, false, false, false,  (int)LabelSheetFieldIndex.LabelHeight, 0, 0, 38);
			base.AddElementFieldInfo("LabelSheetEntity", "LabelWidth", typeof(System.Double), false, false, false, false,  (int)LabelSheetFieldIndex.LabelWidth, 0, 0, 38);
			base.AddElementFieldInfo("LabelSheetEntity", "VerticalSpacing", typeof(System.Double), false, false, false, false,  (int)LabelSheetFieldIndex.VerticalSpacing, 0, 0, 38);
			base.AddElementFieldInfo("LabelSheetEntity", "HorizontalSpacing", typeof(System.Double), false, false, false, false,  (int)LabelSheetFieldIndex.HorizontalSpacing, 0, 0, 38);
			base.AddElementFieldInfo("LabelSheetEntity", "Rows", typeof(System.Int32), false, false, false, false,  (int)LabelSheetFieldIndex.Rows, 0, 0, 10);
			base.AddElementFieldInfo("LabelSheetEntity", "Columns", typeof(System.Int32), false, false, false, false,  (int)LabelSheetFieldIndex.Columns, 0, 0, 10);
		}
		/// <summary>Inits LemonStandOrderEntity's FieldInfo objects</summary>
		private void InitLemonStandOrderEntityInfos()
		{
			base.AddElementFieldInfo("LemonStandOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)LemonStandOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("LemonStandOrderEntity", "LemonStandOrderID", typeof(System.String), false, false, false, false,  (int)LemonStandOrderFieldIndex.LemonStandOrderID, 20, 0, 0);
		}
		/// <summary>Inits LemonStandOrderItemEntity's FieldInfo objects</summary>
		private void InitLemonStandOrderItemEntityInfos()
		{
			base.AddElementFieldInfo("LemonStandOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, false, false,  (int)LemonStandOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			base.AddElementFieldInfo("LemonStandOrderItemEntity", "UrlName", typeof(System.String), false, false, false, false,  (int)LemonStandOrderItemFieldIndex.UrlName, 100, 0, 0);
			base.AddElementFieldInfo("LemonStandOrderItemEntity", "ShortDescription", typeof(System.String), false, false, false, false,  (int)LemonStandOrderItemFieldIndex.ShortDescription, 255, 0, 0);
			base.AddElementFieldInfo("LemonStandOrderItemEntity", "Category", typeof(System.String), false, false, false, false,  (int)LemonStandOrderItemFieldIndex.Category, 100, 0, 0);
		}
		/// <summary>Inits LemonStandStoreEntity's FieldInfo objects</summary>
		private void InitLemonStandStoreEntityInfos()
		{
			base.AddElementFieldInfo("LemonStandStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)LemonStandStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("LemonStandStoreEntity", "Token", typeof(System.String), false, false, false, false,  (int)LemonStandStoreFieldIndex.Token, 100, 0, 0);
			base.AddElementFieldInfo("LemonStandStoreEntity", "StoreURL", typeof(System.String), false, false, false, false,  (int)LemonStandStoreFieldIndex.StoreURL, 255, 0, 0);
			base.AddElementFieldInfo("LemonStandStoreEntity", "StatusCodes", typeof(System.String), false, false, false, true,  (int)LemonStandStoreFieldIndex.StatusCodes, 2147483647, 0, 0);
		}
		/// <summary>Inits MagentoOrderEntity's FieldInfo objects</summary>
		private void InitMagentoOrderEntityInfos()
		{
			base.AddElementFieldInfo("MagentoOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)MagentoOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("MagentoOrderEntity", "MagentoOrderID", typeof(System.Int64), false, false, false, false,  (int)MagentoOrderFieldIndex.MagentoOrderID, 0, 0, 19);
		}
		/// <summary>Inits MagentoStoreEntity's FieldInfo objects</summary>
		private void InitMagentoStoreEntityInfos()
		{
			base.AddElementFieldInfo("MagentoStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)MagentoStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("MagentoStoreEntity", "MagentoTrackingEmails", typeof(System.Boolean), false, false, false, false,  (int)MagentoStoreFieldIndex.MagentoTrackingEmails, 0, 0, 0);
			base.AddElementFieldInfo("MagentoStoreEntity", "MagentoConnect", typeof(System.Boolean), false, false, false, false,  (int)MagentoStoreFieldIndex.MagentoConnect, 0, 0, 0);
		}
		/// <summary>Inits MarketplaceAdvisorOrderEntity's FieldInfo objects</summary>
		private void InitMarketplaceAdvisorOrderEntityInfos()
		{
			base.AddElementFieldInfo("MarketplaceAdvisorOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)MarketplaceAdvisorOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("MarketplaceAdvisorOrderEntity", "BuyerNumber", typeof(System.Int64), false, false, false, false,  (int)MarketplaceAdvisorOrderFieldIndex.BuyerNumber, 0, 0, 19);
			base.AddElementFieldInfo("MarketplaceAdvisorOrderEntity", "SellerOrderNumber", typeof(System.Int64), false, false, false, false,  (int)MarketplaceAdvisorOrderFieldIndex.SellerOrderNumber, 0, 0, 19);
			base.AddElementFieldInfo("MarketplaceAdvisorOrderEntity", "InvoiceNumber", typeof(System.String), false, false, false, false,  (int)MarketplaceAdvisorOrderFieldIndex.InvoiceNumber, 50, 0, 0);
			base.AddElementFieldInfo("MarketplaceAdvisorOrderEntity", "ParcelID", typeof(System.Int64), false, false, false, false,  (int)MarketplaceAdvisorOrderFieldIndex.ParcelID, 0, 0, 19);
		}
		/// <summary>Inits MarketplaceAdvisorStoreEntity's FieldInfo objects</summary>
		private void InitMarketplaceAdvisorStoreEntityInfos()
		{
			base.AddElementFieldInfo("MarketplaceAdvisorStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)MarketplaceAdvisorStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("MarketplaceAdvisorStoreEntity", "Username", typeof(System.String), false, false, false, false,  (int)MarketplaceAdvisorStoreFieldIndex.Username, 50, 0, 0);
			base.AddElementFieldInfo("MarketplaceAdvisorStoreEntity", "Password", typeof(System.String), false, false, false, false,  (int)MarketplaceAdvisorStoreFieldIndex.Password, 50, 0, 0);
			base.AddElementFieldInfo("MarketplaceAdvisorStoreEntity", "AccountType", typeof(System.Int32), false, false, false, false,  (int)MarketplaceAdvisorStoreFieldIndex.AccountType, 0, 0, 10);
			base.AddElementFieldInfo("MarketplaceAdvisorStoreEntity", "DownloadFlags", typeof(System.Int32), false, false, false, false,  (int)MarketplaceAdvisorStoreFieldIndex.DownloadFlags, 0, 0, 10);
		}
		/// <summary>Inits MivaOrderItemAttributeEntity's FieldInfo objects</summary>
		private void InitMivaOrderItemAttributeEntityInfos()
		{
			base.AddElementFieldInfo("MivaOrderItemAttributeEntity", "OrderItemAttributeID", typeof(System.Int64), true, false, false, false,  (int)MivaOrderItemAttributeFieldIndex.OrderItemAttributeID, 0, 0, 19);
			base.AddElementFieldInfo("MivaOrderItemAttributeEntity", "MivaOptionCode", typeof(System.String), false, false, false, false,  (int)MivaOrderItemAttributeFieldIndex.MivaOptionCode, 300, 0, 0);
			base.AddElementFieldInfo("MivaOrderItemAttributeEntity", "MivaAttributeID", typeof(System.Int32), false, false, false, false,  (int)MivaOrderItemAttributeFieldIndex.MivaAttributeID, 0, 0, 10);
			base.AddElementFieldInfo("MivaOrderItemAttributeEntity", "MivaAttributeCode", typeof(System.String), false, false, false, false,  (int)MivaOrderItemAttributeFieldIndex.MivaAttributeCode, 300, 0, 0);
		}
		/// <summary>Inits MivaStoreEntity's FieldInfo objects</summary>
		private void InitMivaStoreEntityInfos()
		{
			base.AddElementFieldInfo("MivaStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)MivaStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("MivaStoreEntity", "EncryptionPassphrase", typeof(System.String), false, false, false, false,  (int)MivaStoreFieldIndex.EncryptionPassphrase, 50, 0, 0);
			base.AddElementFieldInfo("MivaStoreEntity", "LiveManualOrderNumbers", typeof(System.Boolean), false, false, false, false,  (int)MivaStoreFieldIndex.LiveManualOrderNumbers, 0, 0, 0);
			base.AddElementFieldInfo("MivaStoreEntity", "SebenzaCheckoutDataEnabled", typeof(System.Boolean), false, false, false, false,  (int)MivaStoreFieldIndex.SebenzaCheckoutDataEnabled, 0, 0, 0);
			base.AddElementFieldInfo("MivaStoreEntity", "OnlineUpdateStrategy", typeof(System.Int32), false, false, false, false,  (int)MivaStoreFieldIndex.OnlineUpdateStrategy, 0, 0, 10);
			base.AddElementFieldInfo("MivaStoreEntity", "OnlineUpdateStatusChangeEmail", typeof(System.Boolean), false, false, false, false,  (int)MivaStoreFieldIndex.OnlineUpdateStatusChangeEmail, 0, 0, 0);
		}
		/// <summary>Inits NetworkSolutionsOrderEntity's FieldInfo objects</summary>
		private void InitNetworkSolutionsOrderEntityInfos()
		{
			base.AddElementFieldInfo("NetworkSolutionsOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)NetworkSolutionsOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("NetworkSolutionsOrderEntity", "NetworkSolutionsOrderID", typeof(System.Int64), false, false, false, false,  (int)NetworkSolutionsOrderFieldIndex.NetworkSolutionsOrderID, 0, 0, 19);
		}
		/// <summary>Inits NetworkSolutionsStoreEntity's FieldInfo objects</summary>
		private void InitNetworkSolutionsStoreEntityInfos()
		{
			base.AddElementFieldInfo("NetworkSolutionsStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)NetworkSolutionsStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("NetworkSolutionsStoreEntity", "UserToken", typeof(System.String), false, false, false, false,  (int)NetworkSolutionsStoreFieldIndex.UserToken, 50, 0, 0);
			base.AddElementFieldInfo("NetworkSolutionsStoreEntity", "DownloadOrderStatuses", typeof(System.String), false, false, false, false,  (int)NetworkSolutionsStoreFieldIndex.DownloadOrderStatuses, 50, 0, 0);
			base.AddElementFieldInfo("NetworkSolutionsStoreEntity", "StatusCodes", typeof(System.String), false, false, false, false,  (int)NetworkSolutionsStoreFieldIndex.StatusCodes, 2147483647, 0, 0);
			base.AddElementFieldInfo("NetworkSolutionsStoreEntity", "StoreUrl", typeof(System.String), false, false, false, false,  (int)NetworkSolutionsStoreFieldIndex.StoreUrl, 255, 0, 0);
		}
		/// <summary>Inits NeweggOrderEntity's FieldInfo objects</summary>
		private void InitNeweggOrderEntityInfos()
		{
			base.AddElementFieldInfo("NeweggOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)NeweggOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("NeweggOrderEntity", "InvoiceNumber", typeof(Nullable<System.Int64>), false, false, false, true,  (int)NeweggOrderFieldIndex.InvoiceNumber, 0, 0, 19);
			base.AddElementFieldInfo("NeweggOrderEntity", "RefundAmount", typeof(Nullable<System.Decimal>), false, false, false, true,  (int)NeweggOrderFieldIndex.RefundAmount, 0, 4, 19);
			base.AddElementFieldInfo("NeweggOrderEntity", "IsAutoVoid", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)NeweggOrderFieldIndex.IsAutoVoid, 0, 0, 0);
		}
		/// <summary>Inits NeweggOrderItemEntity's FieldInfo objects</summary>
		private void InitNeweggOrderItemEntityInfos()
		{
			base.AddElementFieldInfo("NeweggOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, false, false,  (int)NeweggOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			base.AddElementFieldInfo("NeweggOrderItemEntity", "SellerPartNumber", typeof(System.String), false, false, false, true,  (int)NeweggOrderItemFieldIndex.SellerPartNumber, 64, 0, 0);
			base.AddElementFieldInfo("NeweggOrderItemEntity", "NeweggItemNumber", typeof(System.String), false, false, false, true,  (int)NeweggOrderItemFieldIndex.NeweggItemNumber, 64, 0, 0);
			base.AddElementFieldInfo("NeweggOrderItemEntity", "ManufacturerPartNumber", typeof(System.String), false, false, false, true,  (int)NeweggOrderItemFieldIndex.ManufacturerPartNumber, 64, 0, 0);
			base.AddElementFieldInfo("NeweggOrderItemEntity", "ShippingStatusID", typeof(Nullable<System.Int32>), false, false, false, true,  (int)NeweggOrderItemFieldIndex.ShippingStatusID, 0, 0, 10);
			base.AddElementFieldInfo("NeweggOrderItemEntity", "ShippingStatusDescription", typeof(System.String), false, false, false, true,  (int)NeweggOrderItemFieldIndex.ShippingStatusDescription, 32, 0, 0);
			base.AddElementFieldInfo("NeweggOrderItemEntity", "QuantityShipped", typeof(Nullable<System.Int32>), false, false, false, true,  (int)NeweggOrderItemFieldIndex.QuantityShipped, 0, 0, 10);
		}
		/// <summary>Inits NeweggStoreEntity's FieldInfo objects</summary>
		private void InitNeweggStoreEntityInfos()
		{
			base.AddElementFieldInfo("NeweggStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)NeweggStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("NeweggStoreEntity", "SellerID", typeof(System.String), false, false, false, false,  (int)NeweggStoreFieldIndex.SellerID, 10, 0, 0);
			base.AddElementFieldInfo("NeweggStoreEntity", "SecretKey", typeof(System.String), false, false, false, false,  (int)NeweggStoreFieldIndex.SecretKey, 50, 0, 0);
			base.AddElementFieldInfo("NeweggStoreEntity", "ExcludeFulfilledByNewegg", typeof(System.Boolean), false, false, false, false,  (int)NeweggStoreFieldIndex.ExcludeFulfilledByNewegg, 0, 0, 0);
			base.AddElementFieldInfo("NeweggStoreEntity", "Channel", typeof(System.Int32), false, false, false, false,  (int)NeweggStoreFieldIndex.Channel, 0, 0, 10);
		}
		/// <summary>Inits NoteEntity's FieldInfo objects</summary>
		private void InitNoteEntityInfos()
		{
			base.AddElementFieldInfo("NoteEntity", "NoteID", typeof(System.Int64), true, false, true, false,  (int)NoteFieldIndex.NoteID, 0, 0, 19);
			base.AddElementFieldInfo("NoteEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)NoteFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("NoteEntity", "ObjectID", typeof(System.Int64), false, true, false, false,  (int)NoteFieldIndex.ObjectID, 0, 0, 19);
			base.AddElementFieldInfo("NoteEntity", "UserID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)NoteFieldIndex.UserID, 0, 0, 19);
			base.AddElementFieldInfo("NoteEntity", "Edited", typeof(System.DateTime), false, false, false, false,  (int)NoteFieldIndex.Edited, 0, 0, 0);
			base.AddElementFieldInfo("NoteEntity", "Text", typeof(System.String), false, false, false, false,  (int)NoteFieldIndex.Text, 2147483647, 0, 0);
			base.AddElementFieldInfo("NoteEntity", "Source", typeof(System.Int32), false, false, false, false,  (int)NoteFieldIndex.Source, 0, 0, 10);
			base.AddElementFieldInfo("NoteEntity", "Visibility", typeof(System.Int32), false, false, false, false,  (int)NoteFieldIndex.Visibility, 0, 0, 10);
		}
		/// <summary>Inits ObjectLabelEntity's FieldInfo objects</summary>
		private void InitObjectLabelEntityInfos()
		{
			base.AddElementFieldInfo("ObjectLabelEntity", "ObjectID", typeof(System.Int64), true, false, false, false,  (int)ObjectLabelFieldIndex.ObjectID, 0, 0, 19);
			base.AddElementFieldInfo("ObjectLabelEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ObjectLabelFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("ObjectLabelEntity", "ObjectType", typeof(System.Int32), false, false, false, false,  (int)ObjectLabelFieldIndex.ObjectType, 0, 0, 10);
			base.AddElementFieldInfo("ObjectLabelEntity", "ParentID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)ObjectLabelFieldIndex.ParentID, 0, 0, 19);
			base.AddElementFieldInfo("ObjectLabelEntity", "Label", typeof(System.String), false, false, false, false,  (int)ObjectLabelFieldIndex.Label, 100, 0, 0);
			base.AddElementFieldInfo("ObjectLabelEntity", "IsDeleted", typeof(System.Boolean), false, false, false, false,  (int)ObjectLabelFieldIndex.IsDeleted, 0, 0, 0);
		}
		/// <summary>Inits ObjectReferenceEntity's FieldInfo objects</summary>
		private void InitObjectReferenceEntityInfos()
		{
			base.AddElementFieldInfo("ObjectReferenceEntity", "ObjectReferenceID", typeof(System.Int64), true, false, true, false,  (int)ObjectReferenceFieldIndex.ObjectReferenceID, 0, 0, 19);
			base.AddElementFieldInfo("ObjectReferenceEntity", "ConsumerID", typeof(System.Int64), false, false, false, false,  (int)ObjectReferenceFieldIndex.ConsumerID, 0, 0, 19);
			base.AddElementFieldInfo("ObjectReferenceEntity", "ReferenceKey", typeof(System.String), false, false, false, false,  (int)ObjectReferenceFieldIndex.ReferenceKey, 250, 0, 0);
			base.AddElementFieldInfo("ObjectReferenceEntity", "ObjectID", typeof(System.Int64), false, false, false, false,  (int)ObjectReferenceFieldIndex.ObjectID, 0, 0, 19);
			base.AddElementFieldInfo("ObjectReferenceEntity", "Reason", typeof(System.String), false, false, false, true,  (int)ObjectReferenceFieldIndex.Reason, 250, 0, 0);
		}
		/// <summary>Inits OnTracAccountEntity's FieldInfo objects</summary>
		private void InitOnTracAccountEntityInfos()
		{
			base.AddElementFieldInfo("OnTracAccountEntity", "OnTracAccountID", typeof(System.Int64), true, false, true, false,  (int)OnTracAccountFieldIndex.OnTracAccountID, 0, 0, 19);
			base.AddElementFieldInfo("OnTracAccountEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)OnTracAccountFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("OnTracAccountEntity", "AccountNumber", typeof(System.Int32), false, false, false, false,  (int)OnTracAccountFieldIndex.AccountNumber, 0, 0, 10);
			base.AddElementFieldInfo("OnTracAccountEntity", "Password", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.Password, 50, 0, 0);
			base.AddElementFieldInfo("OnTracAccountEntity", "Description", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.Description, 50, 0, 0);
			base.AddElementFieldInfo("OnTracAccountEntity", "FirstName", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.FirstName, 30, 0, 0);
			base.AddElementFieldInfo("OnTracAccountEntity", "MiddleName", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.MiddleName, 30, 0, 0);
			base.AddElementFieldInfo("OnTracAccountEntity", "LastName", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.LastName, 30, 0, 0);
			base.AddElementFieldInfo("OnTracAccountEntity", "Company", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.Company, 30, 0, 0);
			base.AddElementFieldInfo("OnTracAccountEntity", "Street1", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.Street1, 43, 0, 0);
			base.AddElementFieldInfo("OnTracAccountEntity", "City", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.City, 25, 0, 0);
			base.AddElementFieldInfo("OnTracAccountEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.StateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("OnTracAccountEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.PostalCode, 10, 0, 0);
			base.AddElementFieldInfo("OnTracAccountEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.CountryCode, 50, 0, 0);
			base.AddElementFieldInfo("OnTracAccountEntity", "Email", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.Email, 50, 0, 0);
			base.AddElementFieldInfo("OnTracAccountEntity", "Phone", typeof(System.String), false, false, false, false,  (int)OnTracAccountFieldIndex.Phone, 15, 0, 0);
		}
		/// <summary>Inits OnTracProfileEntity's FieldInfo objects</summary>
		private void InitOnTracProfileEntityInfos()
		{
			base.AddElementFieldInfo("OnTracProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)OnTracProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			base.AddElementFieldInfo("OnTracProfileEntity", "OnTracAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)OnTracProfileFieldIndex.OnTracAccountID, 0, 0, 19);
			base.AddElementFieldInfo("OnTracProfileEntity", "ResidentialDetermination", typeof(Nullable<System.Int32>), false, false, false, true,  (int)OnTracProfileFieldIndex.ResidentialDetermination, 0, 0, 10);
			base.AddElementFieldInfo("OnTracProfileEntity", "Service", typeof(Nullable<System.Int32>), false, false, false, true,  (int)OnTracProfileFieldIndex.Service, 0, 0, 10);
			base.AddElementFieldInfo("OnTracProfileEntity", "SaturdayDelivery", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)OnTracProfileFieldIndex.SaturdayDelivery, 0, 0, 0);
			base.AddElementFieldInfo("OnTracProfileEntity", "SignatureRequired", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)OnTracProfileFieldIndex.SignatureRequired, 0, 0, 0);
			base.AddElementFieldInfo("OnTracProfileEntity", "PackagingType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)OnTracProfileFieldIndex.PackagingType, 0, 0, 10);
			base.AddElementFieldInfo("OnTracProfileEntity", "Weight", typeof(Nullable<System.Double>), false, false, false, true,  (int)OnTracProfileFieldIndex.Weight, 0, 0, 38);
			base.AddElementFieldInfo("OnTracProfileEntity", "DimsProfileID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)OnTracProfileFieldIndex.DimsProfileID, 0, 0, 19);
			base.AddElementFieldInfo("OnTracProfileEntity", "DimsLength", typeof(Nullable<System.Double>), false, false, false, true,  (int)OnTracProfileFieldIndex.DimsLength, 0, 0, 38);
			base.AddElementFieldInfo("OnTracProfileEntity", "DimsWidth", typeof(Nullable<System.Double>), false, false, false, true,  (int)OnTracProfileFieldIndex.DimsWidth, 0, 0, 38);
			base.AddElementFieldInfo("OnTracProfileEntity", "DimsHeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)OnTracProfileFieldIndex.DimsHeight, 0, 0, 38);
			base.AddElementFieldInfo("OnTracProfileEntity", "DimsWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)OnTracProfileFieldIndex.DimsWeight, 0, 0, 38);
			base.AddElementFieldInfo("OnTracProfileEntity", "DimsAddWeight", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)OnTracProfileFieldIndex.DimsAddWeight, 0, 0, 0);
			base.AddElementFieldInfo("OnTracProfileEntity", "Reference1", typeof(System.String), false, false, false, true,  (int)OnTracProfileFieldIndex.Reference1, 300, 0, 0);
			base.AddElementFieldInfo("OnTracProfileEntity", "Reference2", typeof(System.String), false, false, false, true,  (int)OnTracProfileFieldIndex.Reference2, 300, 0, 0);
			base.AddElementFieldInfo("OnTracProfileEntity", "Instructions", typeof(System.String), false, false, false, true,  (int)OnTracProfileFieldIndex.Instructions, 300, 0, 0);
		}
		/// <summary>Inits OnTracShipmentEntity's FieldInfo objects</summary>
		private void InitOnTracShipmentEntityInfos()
		{
			base.AddElementFieldInfo("OnTracShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)OnTracShipmentFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("OnTracShipmentEntity", "OnTracAccountID", typeof(System.Int64), false, false, false, false,  (int)OnTracShipmentFieldIndex.OnTracAccountID, 0, 0, 19);
			base.AddElementFieldInfo("OnTracShipmentEntity", "Service", typeof(System.Int32), false, false, false, false,  (int)OnTracShipmentFieldIndex.Service, 0, 0, 10);
			base.AddElementFieldInfo("OnTracShipmentEntity", "IsCod", typeof(System.Boolean), false, false, false, false,  (int)OnTracShipmentFieldIndex.IsCod, 0, 0, 0);
			base.AddElementFieldInfo("OnTracShipmentEntity", "CodType", typeof(System.Int32), false, false, false, false,  (int)OnTracShipmentFieldIndex.CodType, 0, 0, 10);
			base.AddElementFieldInfo("OnTracShipmentEntity", "CodAmount", typeof(System.Decimal), false, false, false, false,  (int)OnTracShipmentFieldIndex.CodAmount, 0, 4, 19);
			base.AddElementFieldInfo("OnTracShipmentEntity", "SaturdayDelivery", typeof(System.Boolean), false, false, false, false,  (int)OnTracShipmentFieldIndex.SaturdayDelivery, 0, 0, 0);
			base.AddElementFieldInfo("OnTracShipmentEntity", "SignatureRequired", typeof(System.Boolean), false, false, false, false,  (int)OnTracShipmentFieldIndex.SignatureRequired, 0, 0, 0);
			base.AddElementFieldInfo("OnTracShipmentEntity", "PackagingType", typeof(System.Int32), false, false, false, false,  (int)OnTracShipmentFieldIndex.PackagingType, 0, 0, 10);
			base.AddElementFieldInfo("OnTracShipmentEntity", "Instructions", typeof(System.String), false, false, false, false,  (int)OnTracShipmentFieldIndex.Instructions, 300, 0, 0);
			base.AddElementFieldInfo("OnTracShipmentEntity", "DimsProfileID", typeof(System.Int64), false, false, false, false,  (int)OnTracShipmentFieldIndex.DimsProfileID, 0, 0, 19);
			base.AddElementFieldInfo("OnTracShipmentEntity", "DimsLength", typeof(System.Double), false, false, false, false,  (int)OnTracShipmentFieldIndex.DimsLength, 0, 0, 38);
			base.AddElementFieldInfo("OnTracShipmentEntity", "DimsWidth", typeof(System.Double), false, false, false, false,  (int)OnTracShipmentFieldIndex.DimsWidth, 0, 0, 38);
			base.AddElementFieldInfo("OnTracShipmentEntity", "DimsHeight", typeof(System.Double), false, false, false, false,  (int)OnTracShipmentFieldIndex.DimsHeight, 0, 0, 38);
			base.AddElementFieldInfo("OnTracShipmentEntity", "DimsWeight", typeof(System.Double), false, false, false, false,  (int)OnTracShipmentFieldIndex.DimsWeight, 0, 0, 38);
			base.AddElementFieldInfo("OnTracShipmentEntity", "DimsAddWeight", typeof(System.Boolean), false, false, false, false,  (int)OnTracShipmentFieldIndex.DimsAddWeight, 0, 0, 0);
			base.AddElementFieldInfo("OnTracShipmentEntity", "Reference1", typeof(System.String), false, false, false, false,  (int)OnTracShipmentFieldIndex.Reference1, 300, 0, 0);
			base.AddElementFieldInfo("OnTracShipmentEntity", "Reference2", typeof(System.String), false, false, false, false,  (int)OnTracShipmentFieldIndex.Reference2, 300, 0, 0);
			base.AddElementFieldInfo("OnTracShipmentEntity", "InsuranceValue", typeof(System.Decimal), false, false, false, false,  (int)OnTracShipmentFieldIndex.InsuranceValue, 0, 4, 19);
			base.AddElementFieldInfo("OnTracShipmentEntity", "InsurancePennyOne", typeof(System.Boolean), false, false, false, false,  (int)OnTracShipmentFieldIndex.InsurancePennyOne, 0, 0, 0);
			base.AddElementFieldInfo("OnTracShipmentEntity", "DeclaredValue", typeof(System.Decimal), false, false, false, false,  (int)OnTracShipmentFieldIndex.DeclaredValue, 0, 4, 19);
			base.AddElementFieldInfo("OnTracShipmentEntity", "RequestedLabelFormat", typeof(System.Int32), false, false, false, false,  (int)OnTracShipmentFieldIndex.RequestedLabelFormat, 0, 0, 10);
		}
		/// <summary>Inits OrderEntity's FieldInfo objects</summary>
		private void InitOrderEntityInfos()
		{
			base.AddElementFieldInfo("OrderEntity", "OrderID", typeof(System.Int64), true, false, true, false,  (int)OrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("OrderEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)OrderFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "StoreID", typeof(System.Int64), false, true, false, false,  (int)OrderFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("OrderEntity", "CustomerID", typeof(System.Int64), false, true, false, false,  (int)OrderFieldIndex.CustomerID, 0, 0, 19);
			base.AddElementFieldInfo("OrderEntity", "OrderNumber", typeof(System.Int64), false, false, false, false,  (int)OrderFieldIndex.OrderNumber, 0, 0, 19);
			base.AddElementFieldInfo("OrderEntity", "OrderNumberComplete", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.OrderNumberComplete, 50, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "OrderDate", typeof(System.DateTime), false, false, false, false,  (int)OrderFieldIndex.OrderDate, 0, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "OrderTotal", typeof(System.Decimal), false, false, false, false,  (int)OrderFieldIndex.OrderTotal, 0, 4, 19);
			base.AddElementFieldInfo("OrderEntity", "LocalStatus", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.LocalStatus, 100, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "IsManual", typeof(System.Boolean), false, false, false, false,  (int)OrderFieldIndex.IsManual, 0, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "OnlineLastModified", typeof(System.DateTime), false, false, false, false,  (int)OrderFieldIndex.OnlineLastModified, 0, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "OnlineCustomerID", typeof(System.Object), false, false, false, true,  (int)OrderFieldIndex.OnlineCustomerID, 0, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "OnlineStatus", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.OnlineStatus, 100, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "OnlineStatusCode", typeof(System.Object), false, false, false, true,  (int)OrderFieldIndex.OnlineStatusCode, 0, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "RequestedShipping", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.RequestedShipping, 50, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillFirstName", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillFirstName, 30, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillMiddleName", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillMiddleName, 30, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillLastName", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillLastName, 30, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillCompany", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillCompany, 60, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillStreet1", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillStreet1, 60, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillStreet2", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillStreet2, 60, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillStreet3", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillStreet3, 60, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillCity", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillCity, 50, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillStateProvCode", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillStateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillPostalCode", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillPostalCode, 20, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillCountryCode", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillCountryCode, 50, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillPhone", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillPhone, 25, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillFax", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillFax, 35, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillEmail", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillEmail, 100, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillWebsite", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillWebsite, 50, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillAddressValidationSuggestionCount", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.BillAddressValidationSuggestionCount, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "BillAddressValidationStatus", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.BillAddressValidationStatus, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "BillAddressValidationError", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillAddressValidationError, 300, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "BillResidentialStatus", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.BillResidentialStatus, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "BillPOBox", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.BillPOBox, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "BillUSTerritory", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.BillUSTerritory, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "BillMilitaryAddress", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.BillMilitaryAddress, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "ShipFirstName", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipFirstName, 30, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipMiddleName", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipMiddleName, 30, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipLastName", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipLastName, 30, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipCompany", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipCompany, 60, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipStreet1", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipStreet1, 60, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipStreet2", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipStreet2, 60, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipStreet3", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipStreet3, 60, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipCity", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipCity, 50, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipStateProvCode", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipStateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipPostalCode", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipPostalCode, 20, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipCountryCode", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipCountryCode, 50, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipPhone", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipPhone, 25, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipFax", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipFax, 35, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipEmail", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipEmail, 100, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipWebsite", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipWebsite, 50, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipAddressValidationSuggestionCount", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipAddressValidationSuggestionCount, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "ShipAddressValidationStatus", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipAddressValidationStatus, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "ShipAddressValidationError", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipAddressValidationError, 300, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipResidentialStatus", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipResidentialStatus, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "ShipPOBox", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipPOBox, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "ShipUSTerritory", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipUSTerritory, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "ShipMilitaryAddress", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipMilitaryAddress, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "RollupItemCount", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.RollupItemCount, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "RollupItemName", typeof(System.String), false, false, true, true,  (int)OrderFieldIndex.RollupItemName, 300, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "RollupItemCode", typeof(System.String), false, false, true, true,  (int)OrderFieldIndex.RollupItemCode, 300, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "RollupItemSKU", typeof(System.String), false, false, true, true,  (int)OrderFieldIndex.RollupItemSKU, 100, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "RollupItemLocation", typeof(System.String), false, false, true, true,  (int)OrderFieldIndex.RollupItemLocation, 255, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "RollupItemQuantity", typeof(Nullable<System.Double>), false, false, true, true,  (int)OrderFieldIndex.RollupItemQuantity, 0, 0, 38);
			base.AddElementFieldInfo("OrderEntity", "RollupItemTotalWeight", typeof(System.Double), false, false, false, false,  (int)OrderFieldIndex.RollupItemTotalWeight, 0, 0, 38);
			base.AddElementFieldInfo("OrderEntity", "RollupNoteCount", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.RollupNoteCount, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "BillNameParseStatus", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.BillNameParseStatus, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "BillUnparsedName", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.BillUnparsedName, 100, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipNameParseStatus", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipNameParseStatus, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "ShipUnparsedName", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipUnparsedName, 100, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipSenseHashKey", typeof(System.String), false, false, false, false,  (int)OrderFieldIndex.ShipSenseHashKey, 64, 0, 0);
			base.AddElementFieldInfo("OrderEntity", "ShipSenseRecognitionStatus", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipSenseRecognitionStatus, 0, 0, 10);
			base.AddElementFieldInfo("OrderEntity", "ShipAddressType", typeof(System.Int32), false, false, false, false,  (int)OrderFieldIndex.ShipAddressType, 0, 0, 10);
		}
		/// <summary>Inits OrderChargeEntity's FieldInfo objects</summary>
		private void InitOrderChargeEntityInfos()
		{
			base.AddElementFieldInfo("OrderChargeEntity", "OrderChargeID", typeof(System.Int64), true, false, true, false,  (int)OrderChargeFieldIndex.OrderChargeID, 0, 0, 19);
			base.AddElementFieldInfo("OrderChargeEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)OrderChargeFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("OrderChargeEntity", "OrderID", typeof(System.Int64), false, true, false, false,  (int)OrderChargeFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("OrderChargeEntity", "Type", typeof(System.String), false, false, false, false,  (int)OrderChargeFieldIndex.Type, 50, 0, 0);
			base.AddElementFieldInfo("OrderChargeEntity", "Description", typeof(System.String), false, false, false, false,  (int)OrderChargeFieldIndex.Description, 255, 0, 0);
			base.AddElementFieldInfo("OrderChargeEntity", "Amount", typeof(System.Decimal), false, false, false, false,  (int)OrderChargeFieldIndex.Amount, 0, 4, 19);
		}
		/// <summary>Inits OrderItemEntity's FieldInfo objects</summary>
		private void InitOrderItemEntityInfos()
		{
			base.AddElementFieldInfo("OrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, true, false,  (int)OrderItemFieldIndex.OrderItemID, 0, 0, 19);
			base.AddElementFieldInfo("OrderItemEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)OrderItemFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("OrderItemEntity", "OrderID", typeof(System.Int64), false, true, false, false,  (int)OrderItemFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("OrderItemEntity", "Name", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.Name, 300, 0, 0);
			base.AddElementFieldInfo("OrderItemEntity", "Code", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.Code, 300, 0, 0);
			base.AddElementFieldInfo("OrderItemEntity", "SKU", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.SKU, 100, 0, 0);
			base.AddElementFieldInfo("OrderItemEntity", "ISBN", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.ISBN, 30, 0, 0);
			base.AddElementFieldInfo("OrderItemEntity", "UPC", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.UPC, 30, 0, 0);
			base.AddElementFieldInfo("OrderItemEntity", "Description", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.Description, 2147483647, 0, 0);
			base.AddElementFieldInfo("OrderItemEntity", "Location", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.Location, 255, 0, 0);
			base.AddElementFieldInfo("OrderItemEntity", "Image", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.Image, 2147483647, 0, 0);
			base.AddElementFieldInfo("OrderItemEntity", "Thumbnail", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.Thumbnail, 2147483647, 0, 0);
			base.AddElementFieldInfo("OrderItemEntity", "UnitPrice", typeof(System.Decimal), false, false, false, false,  (int)OrderItemFieldIndex.UnitPrice, 0, 4, 19);
			base.AddElementFieldInfo("OrderItemEntity", "UnitCost", typeof(System.Decimal), false, false, false, false,  (int)OrderItemFieldIndex.UnitCost, 0, 4, 19);
			base.AddElementFieldInfo("OrderItemEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)OrderItemFieldIndex.Weight, 0, 0, 38);
			base.AddElementFieldInfo("OrderItemEntity", "Quantity", typeof(System.Double), false, false, false, false,  (int)OrderItemFieldIndex.Quantity, 0, 0, 38);
			base.AddElementFieldInfo("OrderItemEntity", "LocalStatus", typeof(System.String), false, false, false, false,  (int)OrderItemFieldIndex.LocalStatus, 255, 0, 0);
			base.AddElementFieldInfo("OrderItemEntity", "IsManual", typeof(System.Boolean), false, false, false, false,  (int)OrderItemFieldIndex.IsManual, 0, 0, 0);
		}
		/// <summary>Inits OrderItemAttributeEntity's FieldInfo objects</summary>
		private void InitOrderItemAttributeEntityInfos()
		{
			base.AddElementFieldInfo("OrderItemAttributeEntity", "OrderItemAttributeID", typeof(System.Int64), true, false, true, false,  (int)OrderItemAttributeFieldIndex.OrderItemAttributeID, 0, 0, 19);
			base.AddElementFieldInfo("OrderItemAttributeEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)OrderItemAttributeFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("OrderItemAttributeEntity", "OrderItemID", typeof(System.Int64), false, true, false, false,  (int)OrderItemAttributeFieldIndex.OrderItemID, 0, 0, 19);
			base.AddElementFieldInfo("OrderItemAttributeEntity", "Name", typeof(System.String), false, false, false, false,  (int)OrderItemAttributeFieldIndex.Name, 300, 0, 0);
			base.AddElementFieldInfo("OrderItemAttributeEntity", "Description", typeof(System.String), false, false, false, false,  (int)OrderItemAttributeFieldIndex.Description, 2147483647, 0, 0);
			base.AddElementFieldInfo("OrderItemAttributeEntity", "UnitPrice", typeof(System.Decimal), false, false, false, false,  (int)OrderItemAttributeFieldIndex.UnitPrice, 0, 4, 19);
			base.AddElementFieldInfo("OrderItemAttributeEntity", "IsManual", typeof(System.Boolean), false, false, false, false,  (int)OrderItemAttributeFieldIndex.IsManual, 0, 0, 0);
		}
		/// <summary>Inits OrderMotionOrderEntity's FieldInfo objects</summary>
		private void InitOrderMotionOrderEntityInfos()
		{
			base.AddElementFieldInfo("OrderMotionOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)OrderMotionOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("OrderMotionOrderEntity", "OrderMotionShipmentID", typeof(System.Int32), false, false, false, false,  (int)OrderMotionOrderFieldIndex.OrderMotionShipmentID, 0, 0, 10);
			base.AddElementFieldInfo("OrderMotionOrderEntity", "OrderMotionPromotion", typeof(System.String), false, false, false, false,  (int)OrderMotionOrderFieldIndex.OrderMotionPromotion, 50, 0, 0);
			base.AddElementFieldInfo("OrderMotionOrderEntity", "OrderMotionInvoiceNumber", typeof(System.String), false, false, false, false,  (int)OrderMotionOrderFieldIndex.OrderMotionInvoiceNumber, 64, 0, 0);
		}
		/// <summary>Inits OrderMotionStoreEntity's FieldInfo objects</summary>
		private void InitOrderMotionStoreEntityInfos()
		{
			base.AddElementFieldInfo("OrderMotionStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)OrderMotionStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("OrderMotionStoreEntity", "OrderMotionEmailAccountID", typeof(System.Int64), false, true, false, false,  (int)OrderMotionStoreFieldIndex.OrderMotionEmailAccountID, 0, 0, 19);
			base.AddElementFieldInfo("OrderMotionStoreEntity", "OrderMotionBizID", typeof(System.String), false, false, false, false,  (int)OrderMotionStoreFieldIndex.OrderMotionBizID, 2147483647, 0, 0);
		}
		/// <summary>Inits OrderPaymentDetailEntity's FieldInfo objects</summary>
		private void InitOrderPaymentDetailEntityInfos()
		{
			base.AddElementFieldInfo("OrderPaymentDetailEntity", "OrderPaymentDetailID", typeof(System.Int64), true, false, true, false,  (int)OrderPaymentDetailFieldIndex.OrderPaymentDetailID, 0, 0, 19);
			base.AddElementFieldInfo("OrderPaymentDetailEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)OrderPaymentDetailFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("OrderPaymentDetailEntity", "OrderID", typeof(System.Int64), false, true, false, false,  (int)OrderPaymentDetailFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("OrderPaymentDetailEntity", "Label", typeof(System.String), false, false, false, false,  (int)OrderPaymentDetailFieldIndex.Label, 100, 0, 0);
			base.AddElementFieldInfo("OrderPaymentDetailEntity", "Value", typeof(System.String), false, false, false, false,  (int)OrderPaymentDetailFieldIndex.Value, 100, 0, 0);
		}
		/// <summary>Inits OtherProfileEntity's FieldInfo objects</summary>
		private void InitOtherProfileEntityInfos()
		{
			base.AddElementFieldInfo("OtherProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)OtherProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			base.AddElementFieldInfo("OtherProfileEntity", "Carrier", typeof(System.String), false, false, false, true,  (int)OtherProfileFieldIndex.Carrier, 50, 0, 0);
			base.AddElementFieldInfo("OtherProfileEntity", "Service", typeof(System.String), false, false, false, true,  (int)OtherProfileFieldIndex.Service, 50, 0, 0);
		}
		/// <summary>Inits OtherShipmentEntity's FieldInfo objects</summary>
		private void InitOtherShipmentEntityInfos()
		{
			base.AddElementFieldInfo("OtherShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)OtherShipmentFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("OtherShipmentEntity", "Carrier", typeof(System.String), false, false, false, false,  (int)OtherShipmentFieldIndex.Carrier, 50, 0, 0);
			base.AddElementFieldInfo("OtherShipmentEntity", "Service", typeof(System.String), false, false, false, false,  (int)OtherShipmentFieldIndex.Service, 50, 0, 0);
			base.AddElementFieldInfo("OtherShipmentEntity", "InsuranceValue", typeof(System.Decimal), false, false, false, false,  (int)OtherShipmentFieldIndex.InsuranceValue, 0, 4, 19);
		}
		/// <summary>Inits PayPalOrderEntity's FieldInfo objects</summary>
		private void InitPayPalOrderEntityInfos()
		{
			base.AddElementFieldInfo("PayPalOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)PayPalOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("PayPalOrderEntity", "TransactionID", typeof(System.String), false, false, false, false,  (int)PayPalOrderFieldIndex.TransactionID, 50, 0, 0);
			base.AddElementFieldInfo("PayPalOrderEntity", "AddressStatus", typeof(System.Int32), false, false, false, false,  (int)PayPalOrderFieldIndex.AddressStatus, 0, 0, 10);
			base.AddElementFieldInfo("PayPalOrderEntity", "PayPalFee", typeof(System.Decimal), false, false, false, false,  (int)PayPalOrderFieldIndex.PayPalFee, 0, 4, 19);
			base.AddElementFieldInfo("PayPalOrderEntity", "PaymentStatus", typeof(System.Int32), false, false, false, false,  (int)PayPalOrderFieldIndex.PaymentStatus, 0, 0, 10);
		}
		/// <summary>Inits PayPalStoreEntity's FieldInfo objects</summary>
		private void InitPayPalStoreEntityInfos()
		{
			base.AddElementFieldInfo("PayPalStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)PayPalStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("PayPalStoreEntity", "ApiCredentialType", typeof(System.Int16), false, false, false, false,  (int)PayPalStoreFieldIndex.ApiCredentialType, 0, 0, 5);
			base.AddElementFieldInfo("PayPalStoreEntity", "ApiUserName", typeof(System.String), false, false, false, false,  (int)PayPalStoreFieldIndex.ApiUserName, 255, 0, 0);
			base.AddElementFieldInfo("PayPalStoreEntity", "ApiPassword", typeof(System.String), false, false, false, false,  (int)PayPalStoreFieldIndex.ApiPassword, 80, 0, 0);
			base.AddElementFieldInfo("PayPalStoreEntity", "ApiSignature", typeof(System.String), false, false, false, false,  (int)PayPalStoreFieldIndex.ApiSignature, 80, 0, 0);
			base.AddElementFieldInfo("PayPalStoreEntity", "ApiCertificate", typeof(System.Byte[]), false, false, false, true,  (int)PayPalStoreFieldIndex.ApiCertificate, 2048, 0, 0);
			base.AddElementFieldInfo("PayPalStoreEntity", "LastTransactionDate", typeof(System.DateTime), false, false, false, false,  (int)PayPalStoreFieldIndex.LastTransactionDate, 0, 0, 0);
			base.AddElementFieldInfo("PayPalStoreEntity", "LastValidTransactionDate", typeof(System.DateTime), false, false, false, false,  (int)PayPalStoreFieldIndex.LastValidTransactionDate, 0, 0, 0);
		}
		/// <summary>Inits PermissionEntity's FieldInfo objects</summary>
		private void InitPermissionEntityInfos()
		{
			base.AddElementFieldInfo("PermissionEntity", "PermissionID", typeof(System.Int64), true, false, true, false,  (int)PermissionFieldIndex.PermissionID, 0, 0, 19);
			base.AddElementFieldInfo("PermissionEntity", "UserID", typeof(System.Int64), false, true, false, false,  (int)PermissionFieldIndex.UserID, 0, 0, 19);
			base.AddElementFieldInfo("PermissionEntity", "PermissionType", typeof(System.Int32), false, false, false, false,  (int)PermissionFieldIndex.PermissionType, 0, 0, 10);
			base.AddElementFieldInfo("PermissionEntity", "ObjectID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)PermissionFieldIndex.ObjectID, 0, 0, 19);
		}
		/// <summary>Inits PostalProfileEntity's FieldInfo objects</summary>
		private void InitPostalProfileEntityInfos()
		{
			base.AddElementFieldInfo("PostalProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)PostalProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			base.AddElementFieldInfo("PostalProfileEntity", "Service", typeof(Nullable<System.Int32>), false, false, false, true,  (int)PostalProfileFieldIndex.Service, 0, 0, 10);
			base.AddElementFieldInfo("PostalProfileEntity", "Confirmation", typeof(Nullable<System.Int32>), false, false, false, true,  (int)PostalProfileFieldIndex.Confirmation, 0, 0, 10);
			base.AddElementFieldInfo("PostalProfileEntity", "Weight", typeof(Nullable<System.Double>), false, false, false, true,  (int)PostalProfileFieldIndex.Weight, 0, 0, 38);
			base.AddElementFieldInfo("PostalProfileEntity", "PackagingType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)PostalProfileFieldIndex.PackagingType, 0, 0, 10);
			base.AddElementFieldInfo("PostalProfileEntity", "DimsProfileID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)PostalProfileFieldIndex.DimsProfileID, 0, 0, 19);
			base.AddElementFieldInfo("PostalProfileEntity", "DimsLength", typeof(Nullable<System.Double>), false, false, false, true,  (int)PostalProfileFieldIndex.DimsLength, 0, 0, 38);
			base.AddElementFieldInfo("PostalProfileEntity", "DimsWidth", typeof(Nullable<System.Double>), false, false, false, true,  (int)PostalProfileFieldIndex.DimsWidth, 0, 0, 38);
			base.AddElementFieldInfo("PostalProfileEntity", "DimsHeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)PostalProfileFieldIndex.DimsHeight, 0, 0, 38);
			base.AddElementFieldInfo("PostalProfileEntity", "DimsWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)PostalProfileFieldIndex.DimsWeight, 0, 0, 38);
			base.AddElementFieldInfo("PostalProfileEntity", "DimsAddWeight", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)PostalProfileFieldIndex.DimsAddWeight, 0, 0, 0);
			base.AddElementFieldInfo("PostalProfileEntity", "NonRectangular", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)PostalProfileFieldIndex.NonRectangular, 0, 0, 0);
			base.AddElementFieldInfo("PostalProfileEntity", "NonMachinable", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)PostalProfileFieldIndex.NonMachinable, 0, 0, 0);
			base.AddElementFieldInfo("PostalProfileEntity", "CustomsContentType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)PostalProfileFieldIndex.CustomsContentType, 0, 0, 10);
			base.AddElementFieldInfo("PostalProfileEntity", "CustomsContentDescription", typeof(System.String), false, false, false, true,  (int)PostalProfileFieldIndex.CustomsContentDescription, 50, 0, 0);
			base.AddElementFieldInfo("PostalProfileEntity", "ExpressSignatureWaiver", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)PostalProfileFieldIndex.ExpressSignatureWaiver, 0, 0, 0);
			base.AddElementFieldInfo("PostalProfileEntity", "SortType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)PostalProfileFieldIndex.SortType, 0, 0, 10);
			base.AddElementFieldInfo("PostalProfileEntity", "EntryFacility", typeof(Nullable<System.Int32>), false, false, false, true,  (int)PostalProfileFieldIndex.EntryFacility, 0, 0, 10);
			base.AddElementFieldInfo("PostalProfileEntity", "Memo1", typeof(System.String), false, false, false, true,  (int)PostalProfileFieldIndex.Memo1, 300, 0, 0);
			base.AddElementFieldInfo("PostalProfileEntity", "Memo2", typeof(System.String), false, false, false, true,  (int)PostalProfileFieldIndex.Memo2, 300, 0, 0);
			base.AddElementFieldInfo("PostalProfileEntity", "Memo3", typeof(System.String), false, false, false, true,  (int)PostalProfileFieldIndex.Memo3, 300, 0, 0);
			base.AddElementFieldInfo("PostalProfileEntity", "NoPostage", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)PostalProfileFieldIndex.NoPostage, 0, 0, 0);
		}
		/// <summary>Inits PostalShipmentEntity's FieldInfo objects</summary>
		private void InitPostalShipmentEntityInfos()
		{
			base.AddElementFieldInfo("PostalShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)PostalShipmentFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("PostalShipmentEntity", "Service", typeof(System.Int32), false, false, false, false,  (int)PostalShipmentFieldIndex.Service, 0, 0, 10);
			base.AddElementFieldInfo("PostalShipmentEntity", "Confirmation", typeof(System.Int32), false, false, false, false,  (int)PostalShipmentFieldIndex.Confirmation, 0, 0, 10);
			base.AddElementFieldInfo("PostalShipmentEntity", "PackagingType", typeof(System.Int32), false, false, false, false,  (int)PostalShipmentFieldIndex.PackagingType, 0, 0, 10);
			base.AddElementFieldInfo("PostalShipmentEntity", "DimsProfileID", typeof(System.Int64), false, false, false, false,  (int)PostalShipmentFieldIndex.DimsProfileID, 0, 0, 19);
			base.AddElementFieldInfo("PostalShipmentEntity", "DimsLength", typeof(System.Double), false, false, false, false,  (int)PostalShipmentFieldIndex.DimsLength, 0, 0, 38);
			base.AddElementFieldInfo("PostalShipmentEntity", "DimsWidth", typeof(System.Double), false, false, false, false,  (int)PostalShipmentFieldIndex.DimsWidth, 0, 0, 38);
			base.AddElementFieldInfo("PostalShipmentEntity", "DimsHeight", typeof(System.Double), false, false, false, false,  (int)PostalShipmentFieldIndex.DimsHeight, 0, 0, 38);
			base.AddElementFieldInfo("PostalShipmentEntity", "DimsWeight", typeof(System.Double), false, false, false, false,  (int)PostalShipmentFieldIndex.DimsWeight, 0, 0, 38);
			base.AddElementFieldInfo("PostalShipmentEntity", "DimsAddWeight", typeof(System.Boolean), false, false, false, false,  (int)PostalShipmentFieldIndex.DimsAddWeight, 0, 0, 0);
			base.AddElementFieldInfo("PostalShipmentEntity", "NonRectangular", typeof(System.Boolean), false, false, false, false,  (int)PostalShipmentFieldIndex.NonRectangular, 0, 0, 0);
			base.AddElementFieldInfo("PostalShipmentEntity", "NonMachinable", typeof(System.Boolean), false, false, false, false,  (int)PostalShipmentFieldIndex.NonMachinable, 0, 0, 0);
			base.AddElementFieldInfo("PostalShipmentEntity", "CustomsContentType", typeof(System.Int32), false, false, false, false,  (int)PostalShipmentFieldIndex.CustomsContentType, 0, 0, 10);
			base.AddElementFieldInfo("PostalShipmentEntity", "CustomsContentDescription", typeof(System.String), false, false, false, false,  (int)PostalShipmentFieldIndex.CustomsContentDescription, 50, 0, 0);
			base.AddElementFieldInfo("PostalShipmentEntity", "InsuranceValue", typeof(System.Decimal), false, false, false, false,  (int)PostalShipmentFieldIndex.InsuranceValue, 0, 4, 19);
			base.AddElementFieldInfo("PostalShipmentEntity", "ExpressSignatureWaiver", typeof(System.Boolean), false, false, false, false,  (int)PostalShipmentFieldIndex.ExpressSignatureWaiver, 0, 0, 0);
			base.AddElementFieldInfo("PostalShipmentEntity", "SortType", typeof(System.Int32), false, false, false, false,  (int)PostalShipmentFieldIndex.SortType, 0, 0, 10);
			base.AddElementFieldInfo("PostalShipmentEntity", "EntryFacility", typeof(System.Int32), false, false, false, false,  (int)PostalShipmentFieldIndex.EntryFacility, 0, 0, 10);
			base.AddElementFieldInfo("PostalShipmentEntity", "Memo1", typeof(System.String), false, false, false, false,  (int)PostalShipmentFieldIndex.Memo1, 300, 0, 0);
			base.AddElementFieldInfo("PostalShipmentEntity", "Memo2", typeof(System.String), false, false, false, false,  (int)PostalShipmentFieldIndex.Memo2, 300, 0, 0);
			base.AddElementFieldInfo("PostalShipmentEntity", "Memo3", typeof(System.String), false, false, false, false,  (int)PostalShipmentFieldIndex.Memo3, 300, 0, 0);
			base.AddElementFieldInfo("PostalShipmentEntity", "NoPostage", typeof(System.Boolean), false, false, false, false,  (int)PostalShipmentFieldIndex.NoPostage, 0, 0, 0);
		}
		/// <summary>Inits PrintResultEntity's FieldInfo objects</summary>
		private void InitPrintResultEntityInfos()
		{
			base.AddElementFieldInfo("PrintResultEntity", "PrintResultID", typeof(System.Int64), true, false, true, false,  (int)PrintResultFieldIndex.PrintResultID, 0, 0, 19);
			base.AddElementFieldInfo("PrintResultEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)PrintResultFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("PrintResultEntity", "JobIdentifier", typeof(System.Guid), false, false, false, false,  (int)PrintResultFieldIndex.JobIdentifier, 0, 0, 0);
			base.AddElementFieldInfo("PrintResultEntity", "RelatedObjectID", typeof(System.Int64), false, false, false, false,  (int)PrintResultFieldIndex.RelatedObjectID, 0, 0, 19);
			base.AddElementFieldInfo("PrintResultEntity", "ContextObjectID", typeof(System.Int64), false, false, false, false,  (int)PrintResultFieldIndex.ContextObjectID, 0, 0, 19);
			base.AddElementFieldInfo("PrintResultEntity", "TemplateID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)PrintResultFieldIndex.TemplateID, 0, 0, 19);
			base.AddElementFieldInfo("PrintResultEntity", "TemplateType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)PrintResultFieldIndex.TemplateType, 0, 0, 10);
			base.AddElementFieldInfo("PrintResultEntity", "OutputFormat", typeof(Nullable<System.Int32>), false, false, false, true,  (int)PrintResultFieldIndex.OutputFormat, 0, 0, 10);
			base.AddElementFieldInfo("PrintResultEntity", "LabelSheetID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)PrintResultFieldIndex.LabelSheetID, 0, 0, 19);
			base.AddElementFieldInfo("PrintResultEntity", "ComputerID", typeof(System.Int64), false, true, false, false,  (int)PrintResultFieldIndex.ComputerID, 0, 0, 19);
			base.AddElementFieldInfo("PrintResultEntity", "ContentResourceID", typeof(System.Int64), false, false, false, false,  (int)PrintResultFieldIndex.ContentResourceID, 0, 0, 19);
			base.AddElementFieldInfo("PrintResultEntity", "PrintDate", typeof(System.DateTime), false, false, false, false,  (int)PrintResultFieldIndex.PrintDate, 0, 0, 0);
			base.AddElementFieldInfo("PrintResultEntity", "PrinterName", typeof(System.String), false, false, false, false,  (int)PrintResultFieldIndex.PrinterName, 350, 0, 0);
			base.AddElementFieldInfo("PrintResultEntity", "PaperSource", typeof(System.Int32), false, false, false, false,  (int)PrintResultFieldIndex.PaperSource, 0, 0, 10);
			base.AddElementFieldInfo("PrintResultEntity", "PaperSourceName", typeof(System.String), false, false, false, false,  (int)PrintResultFieldIndex.PaperSourceName, 100, 0, 0);
			base.AddElementFieldInfo("PrintResultEntity", "Copies", typeof(System.Int32), false, false, false, false,  (int)PrintResultFieldIndex.Copies, 0, 0, 10);
			base.AddElementFieldInfo("PrintResultEntity", "Collated", typeof(System.Boolean), false, false, false, false,  (int)PrintResultFieldIndex.Collated, 0, 0, 0);
			base.AddElementFieldInfo("PrintResultEntity", "PageMarginLeft", typeof(System.Double), false, false, false, false,  (int)PrintResultFieldIndex.PageMarginLeft, 0, 0, 38);
			base.AddElementFieldInfo("PrintResultEntity", "PageMarginRight", typeof(System.Double), false, false, false, false,  (int)PrintResultFieldIndex.PageMarginRight, 0, 0, 38);
			base.AddElementFieldInfo("PrintResultEntity", "PageMarginBottom", typeof(System.Double), false, false, false, false,  (int)PrintResultFieldIndex.PageMarginBottom, 0, 0, 38);
			base.AddElementFieldInfo("PrintResultEntity", "PageMarginTop", typeof(System.Double), false, false, false, false,  (int)PrintResultFieldIndex.PageMarginTop, 0, 0, 38);
			base.AddElementFieldInfo("PrintResultEntity", "PageWidth", typeof(System.Double), false, false, false, false,  (int)PrintResultFieldIndex.PageWidth, 0, 0, 38);
			base.AddElementFieldInfo("PrintResultEntity", "PageHeight", typeof(System.Double), false, false, false, false,  (int)PrintResultFieldIndex.PageHeight, 0, 0, 38);
		}
		/// <summary>Inits ProStoresOrderEntity's FieldInfo objects</summary>
		private void InitProStoresOrderEntityInfos()
		{
			base.AddElementFieldInfo("ProStoresOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)ProStoresOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("ProStoresOrderEntity", "ConfirmationNumber", typeof(System.String), false, false, false, false,  (int)ProStoresOrderFieldIndex.ConfirmationNumber, 12, 0, 0);
			base.AddElementFieldInfo("ProStoresOrderEntity", "AuthorizedDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ProStoresOrderFieldIndex.AuthorizedDate, 0, 0, 0);
			base.AddElementFieldInfo("ProStoresOrderEntity", "AuthorizedBy", typeof(System.String), false, false, false, false,  (int)ProStoresOrderFieldIndex.AuthorizedBy, 50, 0, 0);
		}
		/// <summary>Inits ProStoresStoreEntity's FieldInfo objects</summary>
		private void InitProStoresStoreEntityInfos()
		{
			base.AddElementFieldInfo("ProStoresStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)ProStoresStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("ProStoresStoreEntity", "ShortName", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ShortName, 30, 0, 0);
			base.AddElementFieldInfo("ProStoresStoreEntity", "Username", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.Username, 50, 0, 0);
			base.AddElementFieldInfo("ProStoresStoreEntity", "LoginMethod", typeof(System.Int32), false, false, false, false,  (int)ProStoresStoreFieldIndex.LoginMethod, 0, 0, 10);
			base.AddElementFieldInfo("ProStoresStoreEntity", "ApiEntryPoint", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ApiEntryPoint, 300, 0, 0);
			base.AddElementFieldInfo("ProStoresStoreEntity", "ApiToken", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ApiToken, 2147483647, 0, 0);
			base.AddElementFieldInfo("ProStoresStoreEntity", "ApiStorefrontUrl", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ApiStorefrontUrl, 300, 0, 0);
			base.AddElementFieldInfo("ProStoresStoreEntity", "ApiTokenLogonUrl", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ApiTokenLogonUrl, 300, 0, 0);
			base.AddElementFieldInfo("ProStoresStoreEntity", "ApiXteUrl", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ApiXteUrl, 300, 0, 0);
			base.AddElementFieldInfo("ProStoresStoreEntity", "ApiRestSecureUrl", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ApiRestSecureUrl, 300, 0, 0);
			base.AddElementFieldInfo("ProStoresStoreEntity", "ApiRestNonSecureUrl", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ApiRestNonSecureUrl, 300, 0, 0);
			base.AddElementFieldInfo("ProStoresStoreEntity", "ApiRestScriptSuffix", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.ApiRestScriptSuffix, 50, 0, 0);
			base.AddElementFieldInfo("ProStoresStoreEntity", "LegacyAdminUrl", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.LegacyAdminUrl, 300, 0, 0);
			base.AddElementFieldInfo("ProStoresStoreEntity", "LegacyXtePath", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.LegacyXtePath, 75, 0, 0);
			base.AddElementFieldInfo("ProStoresStoreEntity", "LegacyPrefix", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.LegacyPrefix, 30, 0, 0);
			base.AddElementFieldInfo("ProStoresStoreEntity", "LegacyPassword", typeof(System.String), false, false, false, false,  (int)ProStoresStoreFieldIndex.LegacyPassword, 150, 0, 0);
			base.AddElementFieldInfo("ProStoresStoreEntity", "LegacyCanUpgrade", typeof(System.Boolean), false, false, false, false,  (int)ProStoresStoreFieldIndex.LegacyCanUpgrade, 0, 0, 0);
		}
		/// <summary>Inits ResourceEntity's FieldInfo objects</summary>
		private void InitResourceEntityInfos()
		{
			base.AddElementFieldInfo("ResourceEntity", "ResourceID", typeof(System.Int64), true, false, true, false,  (int)ResourceFieldIndex.ResourceID, 0, 0, 19);
			base.AddElementFieldInfo("ResourceEntity", "Data", typeof(System.Byte[]), false, false, false, false,  (int)ResourceFieldIndex.Data, 2147483647, 0, 0);
			base.AddElementFieldInfo("ResourceEntity", "Checksum", typeof(System.Byte[]), false, false, false, false,  (int)ResourceFieldIndex.Checksum, 32, 0, 0);
			base.AddElementFieldInfo("ResourceEntity", "Compressed", typeof(System.Boolean), false, false, false, false,  (int)ResourceFieldIndex.Compressed, 0, 0, 0);
			base.AddElementFieldInfo("ResourceEntity", "Filename", typeof(System.String), false, false, false, false,  (int)ResourceFieldIndex.Filename, 30, 0, 0);
		}
		/// <summary>Inits ScanFormBatchEntity's FieldInfo objects</summary>
		private void InitScanFormBatchEntityInfos()
		{
			base.AddElementFieldInfo("ScanFormBatchEntity", "ScanFormBatchID", typeof(System.Int64), true, false, true, false,  (int)ScanFormBatchFieldIndex.ScanFormBatchID, 0, 0, 19);
			base.AddElementFieldInfo("ScanFormBatchEntity", "ShipmentType", typeof(System.Int32), false, false, false, false,  (int)ScanFormBatchFieldIndex.ShipmentType, 0, 0, 10);
			base.AddElementFieldInfo("ScanFormBatchEntity", "CreatedDate", typeof(System.DateTime), false, false, false, false,  (int)ScanFormBatchFieldIndex.CreatedDate, 0, 0, 0);
			base.AddElementFieldInfo("ScanFormBatchEntity", "ShipmentCount", typeof(System.Int32), false, false, false, false,  (int)ScanFormBatchFieldIndex.ShipmentCount, 0, 0, 10);
		}
		/// <summary>Inits SearchEntity's FieldInfo objects</summary>
		private void InitSearchEntityInfos()
		{
			base.AddElementFieldInfo("SearchEntity", "SearchID", typeof(System.Int64), true, false, true, false,  (int)SearchFieldIndex.SearchID, 0, 0, 19);
			base.AddElementFieldInfo("SearchEntity", "Started", typeof(System.DateTime), false, false, false, false,  (int)SearchFieldIndex.Started, 0, 0, 0);
			base.AddElementFieldInfo("SearchEntity", "Pinged", typeof(System.DateTime), false, false, false, false,  (int)SearchFieldIndex.Pinged, 0, 0, 0);
			base.AddElementFieldInfo("SearchEntity", "FilterNodeID", typeof(System.Int64), false, false, false, false,  (int)SearchFieldIndex.FilterNodeID, 0, 0, 19);
			base.AddElementFieldInfo("SearchEntity", "UserID", typeof(System.Int64), false, false, false, false,  (int)SearchFieldIndex.UserID, 0, 0, 19);
			base.AddElementFieldInfo("SearchEntity", "ComputerID", typeof(System.Int64), false, false, false, false,  (int)SearchFieldIndex.ComputerID, 0, 0, 19);
		}
		/// <summary>Inits SearsOrderEntity's FieldInfo objects</summary>
		private void InitSearsOrderEntityInfos()
		{
			base.AddElementFieldInfo("SearsOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)SearsOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("SearsOrderEntity", "PoNumber", typeof(System.String), false, false, false, false,  (int)SearsOrderFieldIndex.PoNumber, 30, 0, 0);
			base.AddElementFieldInfo("SearsOrderEntity", "PoNumberWithDate", typeof(System.String), false, false, false, false,  (int)SearsOrderFieldIndex.PoNumberWithDate, 30, 0, 0);
			base.AddElementFieldInfo("SearsOrderEntity", "LocationID", typeof(System.Int32), false, false, false, false,  (int)SearsOrderFieldIndex.LocationID, 0, 0, 10);
			base.AddElementFieldInfo("SearsOrderEntity", "Commission", typeof(System.Decimal), false, false, false, false,  (int)SearsOrderFieldIndex.Commission, 0, 4, 19);
			base.AddElementFieldInfo("SearsOrderEntity", "CustomerPickup", typeof(System.Boolean), false, false, false, false,  (int)SearsOrderFieldIndex.CustomerPickup, 0, 0, 0);
		}
		/// <summary>Inits SearsOrderItemEntity's FieldInfo objects</summary>
		private void InitSearsOrderItemEntityInfos()
		{
			base.AddElementFieldInfo("SearsOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, false, false,  (int)SearsOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			base.AddElementFieldInfo("SearsOrderItemEntity", "LineNumber", typeof(System.Int32), false, false, false, false,  (int)SearsOrderItemFieldIndex.LineNumber, 0, 0, 10);
			base.AddElementFieldInfo("SearsOrderItemEntity", "ItemID", typeof(System.String), false, false, false, false,  (int)SearsOrderItemFieldIndex.ItemID, 300, 0, 0);
			base.AddElementFieldInfo("SearsOrderItemEntity", "Commission", typeof(System.Decimal), false, false, false, false,  (int)SearsOrderItemFieldIndex.Commission, 0, 4, 19);
			base.AddElementFieldInfo("SearsOrderItemEntity", "Shipping", typeof(System.Decimal), false, false, false, false,  (int)SearsOrderItemFieldIndex.Shipping, 0, 4, 19);
			base.AddElementFieldInfo("SearsOrderItemEntity", "OnlineStatus", typeof(System.String), false, false, false, false,  (int)SearsOrderItemFieldIndex.OnlineStatus, 20, 0, 0);
		}
		/// <summary>Inits SearsStoreEntity's FieldInfo objects</summary>
		private void InitSearsStoreEntityInfos()
		{
			base.AddElementFieldInfo("SearsStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)SearsStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("SearsStoreEntity", "Email", typeof(System.String), false, false, false, false,  (int)SearsStoreFieldIndex.Email, 75, 0, 0);
			base.AddElementFieldInfo("SearsStoreEntity", "Password", typeof(System.String), false, false, false, false,  (int)SearsStoreFieldIndex.Password, 75, 0, 0);
		}
		/// <summary>Inits ServerMessageEntity's FieldInfo objects</summary>
		private void InitServerMessageEntityInfos()
		{
			base.AddElementFieldInfo("ServerMessageEntity", "ServerMessageID", typeof(System.Int64), true, false, true, false,  (int)ServerMessageFieldIndex.ServerMessageID, 0, 0, 19);
			base.AddElementFieldInfo("ServerMessageEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ServerMessageFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("ServerMessageEntity", "Number", typeof(System.Int32), false, false, false, false,  (int)ServerMessageFieldIndex.Number, 0, 0, 10);
			base.AddElementFieldInfo("ServerMessageEntity", "Published", typeof(System.DateTime), false, false, false, false,  (int)ServerMessageFieldIndex.Published, 0, 0, 0);
			base.AddElementFieldInfo("ServerMessageEntity", "Active", typeof(System.Boolean), false, false, false, false,  (int)ServerMessageFieldIndex.Active, 0, 0, 0);
			base.AddElementFieldInfo("ServerMessageEntity", "Dismissable", typeof(System.Boolean), false, false, false, false,  (int)ServerMessageFieldIndex.Dismissable, 0, 0, 0);
			base.AddElementFieldInfo("ServerMessageEntity", "Expires", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ServerMessageFieldIndex.Expires, 0, 0, 0);
			base.AddElementFieldInfo("ServerMessageEntity", "ResponseTo", typeof(Nullable<System.Int32>), false, false, false, true,  (int)ServerMessageFieldIndex.ResponseTo, 0, 0, 10);
			base.AddElementFieldInfo("ServerMessageEntity", "ResponseAction", typeof(Nullable<System.Int32>), false, false, false, true,  (int)ServerMessageFieldIndex.ResponseAction, 0, 0, 10);
			base.AddElementFieldInfo("ServerMessageEntity", "EditTo", typeof(Nullable<System.Int32>), false, false, false, true,  (int)ServerMessageFieldIndex.EditTo, 0, 0, 10);
			base.AddElementFieldInfo("ServerMessageEntity", "Image", typeof(System.String), false, false, false, false,  (int)ServerMessageFieldIndex.Image, 350, 0, 0);
			base.AddElementFieldInfo("ServerMessageEntity", "PrimaryText", typeof(System.String), false, false, false, false,  (int)ServerMessageFieldIndex.PrimaryText, 30, 0, 0);
			base.AddElementFieldInfo("ServerMessageEntity", "SecondaryText", typeof(System.String), false, false, false, false,  (int)ServerMessageFieldIndex.SecondaryText, 60, 0, 0);
			base.AddElementFieldInfo("ServerMessageEntity", "Actions", typeof(System.String), false, false, false, false,  (int)ServerMessageFieldIndex.Actions, 1073741823, 0, 0);
			base.AddElementFieldInfo("ServerMessageEntity", "Stores", typeof(System.String), false, false, false, false,  (int)ServerMessageFieldIndex.Stores, 1073741823, 0, 0);
			base.AddElementFieldInfo("ServerMessageEntity", "Shippers", typeof(System.String), false, false, false, false,  (int)ServerMessageFieldIndex.Shippers, 1073741823, 0, 0);
		}
		/// <summary>Inits ServerMessageSignoffEntity's FieldInfo objects</summary>
		private void InitServerMessageSignoffEntityInfos()
		{
			base.AddElementFieldInfo("ServerMessageSignoffEntity", "ServerMessageSignoffID", typeof(System.Int64), true, false, true, false,  (int)ServerMessageSignoffFieldIndex.ServerMessageSignoffID, 0, 0, 19);
			base.AddElementFieldInfo("ServerMessageSignoffEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ServerMessageSignoffFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("ServerMessageSignoffEntity", "ServerMessageID", typeof(System.Int64), false, true, false, false,  (int)ServerMessageSignoffFieldIndex.ServerMessageID, 0, 0, 19);
			base.AddElementFieldInfo("ServerMessageSignoffEntity", "UserID", typeof(System.Int64), false, false, false, false,  (int)ServerMessageSignoffFieldIndex.UserID, 0, 0, 19);
			base.AddElementFieldInfo("ServerMessageSignoffEntity", "ComputerID", typeof(System.Int64), false, true, false, false,  (int)ServerMessageSignoffFieldIndex.ComputerID, 0, 0, 19);
			base.AddElementFieldInfo("ServerMessageSignoffEntity", "Dismissed", typeof(System.DateTime), false, false, false, false,  (int)ServerMessageSignoffFieldIndex.Dismissed, 0, 0, 0);
		}
		/// <summary>Inits ServiceStatusEntity's FieldInfo objects</summary>
		private void InitServiceStatusEntityInfos()
		{
			base.AddElementFieldInfo("ServiceStatusEntity", "ServiceStatusID", typeof(System.Int64), true, false, true, false,  (int)ServiceStatusFieldIndex.ServiceStatusID, 0, 0, 19);
			base.AddElementFieldInfo("ServiceStatusEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ServiceStatusFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("ServiceStatusEntity", "ComputerID", typeof(System.Int64), false, true, false, false,  (int)ServiceStatusFieldIndex.ComputerID, 0, 0, 19);
			base.AddElementFieldInfo("ServiceStatusEntity", "ServiceType", typeof(System.Int32), false, false, false, false,  (int)ServiceStatusFieldIndex.ServiceType, 0, 0, 10);
			base.AddElementFieldInfo("ServiceStatusEntity", "LastStartDateTime", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ServiceStatusFieldIndex.LastStartDateTime, 0, 0, 0);
			base.AddElementFieldInfo("ServiceStatusEntity", "LastStopDateTime", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ServiceStatusFieldIndex.LastStopDateTime, 0, 0, 0);
			base.AddElementFieldInfo("ServiceStatusEntity", "LastCheckInDateTime", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ServiceStatusFieldIndex.LastCheckInDateTime, 0, 0, 0);
			base.AddElementFieldInfo("ServiceStatusEntity", "ServiceFullName", typeof(System.String), false, false, false, false,  (int)ServiceStatusFieldIndex.ServiceFullName, 256, 0, 0);
			base.AddElementFieldInfo("ServiceStatusEntity", "ServiceDisplayName", typeof(System.String), false, false, false, false,  (int)ServiceStatusFieldIndex.ServiceDisplayName, 256, 0, 0);
		}
		/// <summary>Inits ShipmentEntity's FieldInfo objects</summary>
		private void InitShipmentEntityInfos()
		{
			base.AddElementFieldInfo("ShipmentEntity", "ShipmentID", typeof(System.Int64), true, false, true, false,  (int)ShipmentFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("ShipmentEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ShipmentFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OrderID", typeof(System.Int64), false, true, false, false,  (int)ShipmentFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("ShipmentEntity", "ShipmentType", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipmentType, 0, 0, 10);
			base.AddElementFieldInfo("ShipmentEntity", "ContentWeight", typeof(System.Double), false, false, false, false,  (int)ShipmentFieldIndex.ContentWeight, 0, 0, 38);
			base.AddElementFieldInfo("ShipmentEntity", "TotalWeight", typeof(System.Double), false, false, false, false,  (int)ShipmentFieldIndex.TotalWeight, 0, 0, 38);
			base.AddElementFieldInfo("ShipmentEntity", "Processed", typeof(System.Boolean), false, false, false, false,  (int)ShipmentFieldIndex.Processed, 0, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ProcessedDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ShipmentFieldIndex.ProcessedDate, 0, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ProcessedUserID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)ShipmentFieldIndex.ProcessedUserID, 0, 0, 19);
			base.AddElementFieldInfo("ShipmentEntity", "ProcessedComputerID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)ShipmentFieldIndex.ProcessedComputerID, 0, 0, 19);
			base.AddElementFieldInfo("ShipmentEntity", "ShipDate", typeof(System.DateTime), false, false, false, false,  (int)ShipmentFieldIndex.ShipDate, 0, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ShipmentCost", typeof(System.Decimal), false, false, false, false,  (int)ShipmentFieldIndex.ShipmentCost, 0, 4, 19);
			base.AddElementFieldInfo("ShipmentEntity", "Voided", typeof(System.Boolean), false, false, false, false,  (int)ShipmentFieldIndex.Voided, 0, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "VoidedDate", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ShipmentFieldIndex.VoidedDate, 0, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "VoidedUserID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)ShipmentFieldIndex.VoidedUserID, 0, 0, 19);
			base.AddElementFieldInfo("ShipmentEntity", "VoidedComputerID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)ShipmentFieldIndex.VoidedComputerID, 0, 0, 19);
			base.AddElementFieldInfo("ShipmentEntity", "TrackingNumber", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.TrackingNumber, 50, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "CustomsGenerated", typeof(System.Boolean), false, false, false, false,  (int)ShipmentFieldIndex.CustomsGenerated, 0, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "CustomsValue", typeof(System.Decimal), false, false, false, false,  (int)ShipmentFieldIndex.CustomsValue, 0, 4, 19);
			base.AddElementFieldInfo("ShipmentEntity", "RequestedLabelFormat", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.RequestedLabelFormat, 0, 0, 10);
			base.AddElementFieldInfo("ShipmentEntity", "ActualLabelFormat", typeof(Nullable<System.Int32>), false, false, false, true,  (int)ShipmentFieldIndex.ActualLabelFormat, 0, 0, 10);
			base.AddElementFieldInfo("ShipmentEntity", "ShipFirstName", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipFirstName, 30, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ShipMiddleName", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipMiddleName, 30, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ShipLastName", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipLastName, 30, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ShipCompany", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipCompany, 60, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ShipStreet1", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipStreet1, 60, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ShipStreet2", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipStreet2, 60, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ShipStreet3", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipStreet3, 60, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ShipCity", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipCity, 50, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ShipStateProvCode", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipStateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ShipPostalCode", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipPostalCode, 20, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ShipCountryCode", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipCountryCode, 50, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ShipPhone", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipPhone, 25, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ShipEmail", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipEmail, 100, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ShipAddressValidationSuggestionCount", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipAddressValidationSuggestionCount, 0, 0, 10);
			base.AddElementFieldInfo("ShipmentEntity", "ShipAddressValidationStatus", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipAddressValidationStatus, 0, 0, 10);
			base.AddElementFieldInfo("ShipmentEntity", "ShipAddressValidationError", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipAddressValidationError, 300, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ShipResidentialStatus", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipResidentialStatus, 0, 0, 10);
			base.AddElementFieldInfo("ShipmentEntity", "ShipPOBox", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipPOBox, 0, 0, 10);
			base.AddElementFieldInfo("ShipmentEntity", "ShipUSTerritory", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipUSTerritory, 0, 0, 10);
			base.AddElementFieldInfo("ShipmentEntity", "ShipMilitaryAddress", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipMilitaryAddress, 0, 0, 10);
			base.AddElementFieldInfo("ShipmentEntity", "ResidentialDetermination", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ResidentialDetermination, 0, 0, 10);
			base.AddElementFieldInfo("ShipmentEntity", "ResidentialResult", typeof(System.Boolean), false, false, false, false,  (int)ShipmentFieldIndex.ResidentialResult, 0, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OriginOriginID", typeof(System.Int64), false, false, false, false,  (int)ShipmentFieldIndex.OriginOriginID, 0, 0, 19);
			base.AddElementFieldInfo("ShipmentEntity", "OriginFirstName", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginFirstName, 30, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OriginMiddleName", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginMiddleName, 30, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OriginLastName", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginLastName, 30, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OriginCompany", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginCompany, 60, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OriginStreet1", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginStreet1, 60, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OriginStreet2", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginStreet2, 60, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OriginStreet3", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginStreet3, 60, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OriginCity", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginCity, 50, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OriginStateProvCode", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginStateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OriginPostalCode", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginPostalCode, 20, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OriginCountryCode", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginCountryCode, 50, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OriginPhone", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginPhone, 25, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OriginFax", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginFax, 35, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OriginEmail", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginEmail, 100, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OriginWebsite", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginWebsite, 50, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ReturnShipment", typeof(System.Boolean), false, false, false, false,  (int)ShipmentFieldIndex.ReturnShipment, 0, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "Insurance", typeof(System.Boolean), false, false, false, false,  (int)ShipmentFieldIndex.Insurance, 0, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "InsuranceProvider", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.InsuranceProvider, 0, 0, 10);
			base.AddElementFieldInfo("ShipmentEntity", "ShipNameParseStatus", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipNameParseStatus, 0, 0, 10);
			base.AddElementFieldInfo("ShipmentEntity", "ShipUnparsedName", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipUnparsedName, 100, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OriginNameParseStatus", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.OriginNameParseStatus, 0, 0, 10);
			base.AddElementFieldInfo("ShipmentEntity", "OriginUnparsedName", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OriginUnparsedName, 100, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "BestRateEvents", typeof(System.Byte), false, false, false, false,  (int)ShipmentFieldIndex.BestRateEvents, 0, 0, 3);
			base.AddElementFieldInfo("ShipmentEntity", "ShipSenseStatus", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.ShipSenseStatus, 0, 0, 10);
			base.AddElementFieldInfo("ShipmentEntity", "ShipSenseChangeSets", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.ShipSenseChangeSets, 2147483647, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "ShipSenseEntry", typeof(System.Byte[]), false, false, false, false,  (int)ShipmentFieldIndex.ShipSenseEntry, 2147483647, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "OnlineShipmentID", typeof(System.String), false, false, false, false,  (int)ShipmentFieldIndex.OnlineShipmentID, 128, 0, 0);
			base.AddElementFieldInfo("ShipmentEntity", "BilledType", typeof(System.Int32), false, false, false, false,  (int)ShipmentFieldIndex.BilledType, 0, 0, 10);
			base.AddElementFieldInfo("ShipmentEntity", "BilledWeight", typeof(System.Double), false, false, false, false,  (int)ShipmentFieldIndex.BilledWeight, 0, 0, 38);
		}
		/// <summary>Inits ShipmentCustomsItemEntity's FieldInfo objects</summary>
		private void InitShipmentCustomsItemEntityInfos()
		{
			base.AddElementFieldInfo("ShipmentCustomsItemEntity", "ShipmentCustomsItemID", typeof(System.Int64), true, false, true, false,  (int)ShipmentCustomsItemFieldIndex.ShipmentCustomsItemID, 0, 0, 19);
			base.AddElementFieldInfo("ShipmentCustomsItemEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ShipmentCustomsItemFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("ShipmentCustomsItemEntity", "ShipmentID", typeof(System.Int64), false, true, false, false,  (int)ShipmentCustomsItemFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("ShipmentCustomsItemEntity", "Description", typeof(System.String), false, false, false, false,  (int)ShipmentCustomsItemFieldIndex.Description, 150, 0, 0);
			base.AddElementFieldInfo("ShipmentCustomsItemEntity", "Quantity", typeof(System.Double), false, false, false, false,  (int)ShipmentCustomsItemFieldIndex.Quantity, 0, 0, 38);
			base.AddElementFieldInfo("ShipmentCustomsItemEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)ShipmentCustomsItemFieldIndex.Weight, 0, 0, 38);
			base.AddElementFieldInfo("ShipmentCustomsItemEntity", "UnitValue", typeof(System.Decimal), false, false, false, false,  (int)ShipmentCustomsItemFieldIndex.UnitValue, 0, 4, 19);
			base.AddElementFieldInfo("ShipmentCustomsItemEntity", "CountryOfOrigin", typeof(System.String), false, false, false, false,  (int)ShipmentCustomsItemFieldIndex.CountryOfOrigin, 50, 0, 0);
			base.AddElementFieldInfo("ShipmentCustomsItemEntity", "HarmonizedCode", typeof(System.String), false, false, false, false,  (int)ShipmentCustomsItemFieldIndex.HarmonizedCode, 14, 0, 0);
			base.AddElementFieldInfo("ShipmentCustomsItemEntity", "NumberOfPieces", typeof(System.Int32), false, false, false, false,  (int)ShipmentCustomsItemFieldIndex.NumberOfPieces, 0, 0, 10);
			base.AddElementFieldInfo("ShipmentCustomsItemEntity", "UnitPriceAmount", typeof(System.Decimal), false, false, false, false,  (int)ShipmentCustomsItemFieldIndex.UnitPriceAmount, 0, 4, 19);
		}
		/// <summary>Inits ShippingDefaultsRuleEntity's FieldInfo objects</summary>
		private void InitShippingDefaultsRuleEntityInfos()
		{
			base.AddElementFieldInfo("ShippingDefaultsRuleEntity", "ShippingDefaultsRuleID", typeof(System.Int64), true, false, true, false,  (int)ShippingDefaultsRuleFieldIndex.ShippingDefaultsRuleID, 0, 0, 19);
			base.AddElementFieldInfo("ShippingDefaultsRuleEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ShippingDefaultsRuleFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("ShippingDefaultsRuleEntity", "ShipmentType", typeof(System.Int32), false, false, false, false,  (int)ShippingDefaultsRuleFieldIndex.ShipmentType, 0, 0, 10);
			base.AddElementFieldInfo("ShippingDefaultsRuleEntity", "FilterNodeID", typeof(System.Int64), false, false, false, false,  (int)ShippingDefaultsRuleFieldIndex.FilterNodeID, 0, 0, 19);
			base.AddElementFieldInfo("ShippingDefaultsRuleEntity", "ShippingProfileID", typeof(System.Int64), false, false, false, false,  (int)ShippingDefaultsRuleFieldIndex.ShippingProfileID, 0, 0, 19);
			base.AddElementFieldInfo("ShippingDefaultsRuleEntity", "Position", typeof(System.Int32), false, false, false, false,  (int)ShippingDefaultsRuleFieldIndex.Position, 0, 0, 10);
		}
		/// <summary>Inits ShippingOriginEntity's FieldInfo objects</summary>
		private void InitShippingOriginEntityInfos()
		{
			base.AddElementFieldInfo("ShippingOriginEntity", "ShippingOriginID", typeof(System.Int64), true, false, true, false,  (int)ShippingOriginFieldIndex.ShippingOriginID, 0, 0, 19);
			base.AddElementFieldInfo("ShippingOriginEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ShippingOriginFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("ShippingOriginEntity", "Description", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Description, 50, 0, 0);
			base.AddElementFieldInfo("ShippingOriginEntity", "FirstName", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.FirstName, 30, 0, 0);
			base.AddElementFieldInfo("ShippingOriginEntity", "MiddleName", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.MiddleName, 30, 0, 0);
			base.AddElementFieldInfo("ShippingOriginEntity", "LastName", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.LastName, 30, 0, 0);
			base.AddElementFieldInfo("ShippingOriginEntity", "Company", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Company, 35, 0, 0);
			base.AddElementFieldInfo("ShippingOriginEntity", "Street1", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Street1, 60, 0, 0);
			base.AddElementFieldInfo("ShippingOriginEntity", "Street2", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Street2, 60, 0, 0);
			base.AddElementFieldInfo("ShippingOriginEntity", "Street3", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Street3, 60, 0, 0);
			base.AddElementFieldInfo("ShippingOriginEntity", "City", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.City, 50, 0, 0);
			base.AddElementFieldInfo("ShippingOriginEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.StateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("ShippingOriginEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.PostalCode, 20, 0, 0);
			base.AddElementFieldInfo("ShippingOriginEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.CountryCode, 50, 0, 0);
			base.AddElementFieldInfo("ShippingOriginEntity", "Phone", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Phone, 25, 0, 0);
			base.AddElementFieldInfo("ShippingOriginEntity", "Fax", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Fax, 35, 0, 0);
			base.AddElementFieldInfo("ShippingOriginEntity", "Email", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Email, 100, 0, 0);
			base.AddElementFieldInfo("ShippingOriginEntity", "Website", typeof(System.String), false, false, false, false,  (int)ShippingOriginFieldIndex.Website, 50, 0, 0);
		}
		/// <summary>Inits ShippingPrintOutputEntity's FieldInfo objects</summary>
		private void InitShippingPrintOutputEntityInfos()
		{
			base.AddElementFieldInfo("ShippingPrintOutputEntity", "ShippingPrintOutputID", typeof(System.Int64), true, false, true, false,  (int)ShippingPrintOutputFieldIndex.ShippingPrintOutputID, 0, 0, 19);
			base.AddElementFieldInfo("ShippingPrintOutputEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ShippingPrintOutputFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("ShippingPrintOutputEntity", "ShipmentType", typeof(System.Int32), false, false, false, false,  (int)ShippingPrintOutputFieldIndex.ShipmentType, 0, 0, 10);
			base.AddElementFieldInfo("ShippingPrintOutputEntity", "Name", typeof(System.String), false, false, false, false,  (int)ShippingPrintOutputFieldIndex.Name, 50, 0, 0);
		}
		/// <summary>Inits ShippingPrintOutputRuleEntity's FieldInfo objects</summary>
		private void InitShippingPrintOutputRuleEntityInfos()
		{
			base.AddElementFieldInfo("ShippingPrintOutputRuleEntity", "ShippingPrintOutputRuleID", typeof(System.Int64), true, false, true, false,  (int)ShippingPrintOutputRuleFieldIndex.ShippingPrintOutputRuleID, 0, 0, 19);
			base.AddElementFieldInfo("ShippingPrintOutputRuleEntity", "ShippingPrintOutputID", typeof(System.Int64), false, true, false, false,  (int)ShippingPrintOutputRuleFieldIndex.ShippingPrintOutputID, 0, 0, 19);
			base.AddElementFieldInfo("ShippingPrintOutputRuleEntity", "FilterNodeID", typeof(System.Int64), false, false, false, false,  (int)ShippingPrintOutputRuleFieldIndex.FilterNodeID, 0, 0, 19);
			base.AddElementFieldInfo("ShippingPrintOutputRuleEntity", "TemplateID", typeof(System.Int64), false, false, false, false,  (int)ShippingPrintOutputRuleFieldIndex.TemplateID, 0, 0, 19);
		}
		/// <summary>Inits ShippingProfileEntity's FieldInfo objects</summary>
		private void InitShippingProfileEntityInfos()
		{
			base.AddElementFieldInfo("ShippingProfileEntity", "ShippingProfileID", typeof(System.Int64), true, false, true, false,  (int)ShippingProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			base.AddElementFieldInfo("ShippingProfileEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ShippingProfileFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("ShippingProfileEntity", "Name", typeof(System.String), false, false, false, false,  (int)ShippingProfileFieldIndex.Name, 50, 0, 0);
			base.AddElementFieldInfo("ShippingProfileEntity", "ShipmentType", typeof(System.Int32), false, false, false, false,  (int)ShippingProfileFieldIndex.ShipmentType, 0, 0, 10);
			base.AddElementFieldInfo("ShippingProfileEntity", "ShipmentTypePrimary", typeof(System.Boolean), false, false, false, false,  (int)ShippingProfileFieldIndex.ShipmentTypePrimary, 0, 0, 0);
			base.AddElementFieldInfo("ShippingProfileEntity", "OriginID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)ShippingProfileFieldIndex.OriginID, 0, 0, 19);
			base.AddElementFieldInfo("ShippingProfileEntity", "Insurance", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)ShippingProfileFieldIndex.Insurance, 0, 0, 0);
			base.AddElementFieldInfo("ShippingProfileEntity", "InsuranceInitialValueSource", typeof(Nullable<System.Int32>), false, false, false, true,  (int)ShippingProfileFieldIndex.InsuranceInitialValueSource, 0, 0, 10);
			base.AddElementFieldInfo("ShippingProfileEntity", "InsuranceInitialValueAmount", typeof(Nullable<System.Decimal>), false, false, false, true,  (int)ShippingProfileFieldIndex.InsuranceInitialValueAmount, 0, 4, 19);
			base.AddElementFieldInfo("ShippingProfileEntity", "ReturnShipment", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)ShippingProfileFieldIndex.ReturnShipment, 0, 0, 0);
			base.AddElementFieldInfo("ShippingProfileEntity", "RequestedLabelFormat", typeof(Nullable<System.Int32>), false, false, false, true,  (int)ShippingProfileFieldIndex.RequestedLabelFormat, 0, 0, 10);
		}
		/// <summary>Inits ShippingProviderRuleEntity's FieldInfo objects</summary>
		private void InitShippingProviderRuleEntityInfos()
		{
			base.AddElementFieldInfo("ShippingProviderRuleEntity", "ShippingProviderRuleID", typeof(System.Int64), true, false, true, false,  (int)ShippingProviderRuleFieldIndex.ShippingProviderRuleID, 0, 0, 19);
			base.AddElementFieldInfo("ShippingProviderRuleEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)ShippingProviderRuleFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("ShippingProviderRuleEntity", "FilterNodeID", typeof(System.Int64), false, false, false, false,  (int)ShippingProviderRuleFieldIndex.FilterNodeID, 0, 0, 19);
			base.AddElementFieldInfo("ShippingProviderRuleEntity", "ShipmentType", typeof(System.Int32), false, false, false, false,  (int)ShippingProviderRuleFieldIndex.ShipmentType, 0, 0, 10);
		}
		/// <summary>Inits ShippingSettingsEntity's FieldInfo objects</summary>
		private void InitShippingSettingsEntityInfos()
		{
			base.AddElementFieldInfo("ShippingSettingsEntity", "ShippingSettingsID", typeof(System.Boolean), true, false, false, false,  (int)ShippingSettingsFieldIndex.ShippingSettingsID, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "InternalActivated", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.InternalActivated, 45, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "InternalConfigured", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.InternalConfigured, 45, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "InternalExcluded", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.InternalExcluded, 45, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "DefaultType", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.DefaultType, 0, 0, 10);
			base.AddElementFieldInfo("ShippingSettingsEntity", "BlankPhoneOption", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.BlankPhoneOption, 0, 0, 10);
			base.AddElementFieldInfo("ShippingSettingsEntity", "BlankPhoneNumber", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.BlankPhoneNumber, 16, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "InsurancePolicy", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.InsurancePolicy, 40, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "InsuranceLastAgreed", typeof(Nullable<System.DateTime>), false, false, false, true,  (int)ShippingSettingsFieldIndex.InsuranceLastAgreed, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "FedExUsername", typeof(System.String), false, false, false, true,  (int)ShippingSettingsFieldIndex.FedExUsername, 50, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "FedExPassword", typeof(System.String), false, false, false, true,  (int)ShippingSettingsFieldIndex.FedExPassword, 50, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "FedExMaskAccount", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.FedExMaskAccount, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "FedExThermalDocTab", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.FedExThermalDocTab, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "FedExThermalDocTabType", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.FedExThermalDocTabType, 0, 0, 10);
			base.AddElementFieldInfo("ShippingSettingsEntity", "FedExInsuranceProvider", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.FedExInsuranceProvider, 0, 0, 10);
			base.AddElementFieldInfo("ShippingSettingsEntity", "FedExInsurancePennyOne", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.FedExInsurancePennyOne, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "UpsAccessKey", typeof(System.String), false, false, false, true,  (int)ShippingSettingsFieldIndex.UpsAccessKey, 50, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "UpsInsuranceProvider", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.UpsInsuranceProvider, 0, 0, 10);
			base.AddElementFieldInfo("ShippingSettingsEntity", "UpsInsurancePennyOne", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.UpsInsurancePennyOne, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "EndiciaCustomsCertify", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.EndiciaCustomsCertify, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "EndiciaCustomsSigner", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.EndiciaCustomsSigner, 100, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "EndiciaThermalDocTab", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.EndiciaThermalDocTab, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "EndiciaThermalDocTabType", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.EndiciaThermalDocTabType, 0, 0, 10);
			base.AddElementFieldInfo("ShippingSettingsEntity", "EndiciaAutomaticExpress1", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.EndiciaAutomaticExpress1, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "EndiciaAutomaticExpress1Account", typeof(System.Int64), false, false, false, false,  (int)ShippingSettingsFieldIndex.EndiciaAutomaticExpress1Account, 0, 0, 19);
			base.AddElementFieldInfo("ShippingSettingsEntity", "EndiciaInsuranceProvider", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.EndiciaInsuranceProvider, 0, 0, 10);
			base.AddElementFieldInfo("ShippingSettingsEntity", "WorldShipLaunch", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.WorldShipLaunch, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "UspsAutomaticExpress1", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.UspsAutomaticExpress1, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "UspsAutomaticExpress1Account", typeof(System.Int64), false, false, false, false,  (int)ShippingSettingsFieldIndex.UspsAutomaticExpress1Account, 0, 0, 19);
			base.AddElementFieldInfo("ShippingSettingsEntity", "UspsInsuranceProvider", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.UspsInsuranceProvider, 0, 0, 10);
			base.AddElementFieldInfo("ShippingSettingsEntity", "Express1EndiciaCustomsCertify", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.Express1EndiciaCustomsCertify, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "Express1EndiciaCustomsSigner", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.Express1EndiciaCustomsSigner, 100, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "Express1EndiciaThermalDocTab", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.Express1EndiciaThermalDocTab, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "Express1EndiciaThermalDocTabType", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.Express1EndiciaThermalDocTabType, 0, 0, 10);
			base.AddElementFieldInfo("ShippingSettingsEntity", "Express1EndiciaSingleSource", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.Express1EndiciaSingleSource, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "OnTracInsuranceProvider", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.OnTracInsuranceProvider, 0, 0, 10);
			base.AddElementFieldInfo("ShippingSettingsEntity", "OnTracInsurancePennyOne", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.OnTracInsurancePennyOne, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "IParcelInsuranceProvider", typeof(System.Int32), false, false, false, false,  (int)ShippingSettingsFieldIndex.IParcelInsuranceProvider, 0, 0, 10);
			base.AddElementFieldInfo("ShippingSettingsEntity", "IParcelInsurancePennyOne", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.IParcelInsurancePennyOne, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "Express1UspsSingleSource", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.Express1UspsSingleSource, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "UpsMailInnovationsEnabled", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.UpsMailInnovationsEnabled, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "WorldShipMailInnovationsEnabled", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.WorldShipMailInnovationsEnabled, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "InternalBestRateExcludedShipmentTypes", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.InternalBestRateExcludedShipmentTypes, 30, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "ShipSenseEnabled", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.ShipSenseEnabled, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "ShipSenseUniquenessXml", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.ShipSenseUniquenessXml, 2147483647, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "ShipSenseProcessedShipmentID", typeof(System.Int64), false, false, false, false,  (int)ShippingSettingsFieldIndex.ShipSenseProcessedShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("ShippingSettingsEntity", "ShipSenseEndShipmentID", typeof(System.Int64), false, false, false, false,  (int)ShippingSettingsFieldIndex.ShipSenseEndShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("ShippingSettingsEntity", "AutoCreateShipments", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.AutoCreateShipments, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "FedExFimsEnabled", typeof(System.Boolean), false, false, false, false,  (int)ShippingSettingsFieldIndex.FedExFimsEnabled, 0, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "FedExFimsUsername", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.FedExFimsUsername, 50, 0, 0);
			base.AddElementFieldInfo("ShippingSettingsEntity", "FedExFimsPassword", typeof(System.String), false, false, false, false,  (int)ShippingSettingsFieldIndex.FedExFimsPassword, 50, 0, 0);
		}
		/// <summary>Inits ShipSenseKnowledgebaseEntity's FieldInfo objects</summary>
		private void InitShipSenseKnowledgebaseEntityInfos()
		{
			base.AddElementFieldInfo("ShipSenseKnowledgebaseEntity", "Hash", typeof(System.String), true, false, false, false,  (int)ShipSenseKnowledgebaseFieldIndex.Hash, 64, 0, 0);
			base.AddElementFieldInfo("ShipSenseKnowledgebaseEntity", "Entry", typeof(System.Byte[]), false, false, false, false,  (int)ShipSenseKnowledgebaseFieldIndex.Entry, 2147483647, 0, 0);
		}
		/// <summary>Inits ShopifyOrderEntity's FieldInfo objects</summary>
		private void InitShopifyOrderEntityInfos()
		{
			base.AddElementFieldInfo("ShopifyOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)ShopifyOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("ShopifyOrderEntity", "ShopifyOrderID", typeof(System.Int64), false, false, false, false,  (int)ShopifyOrderFieldIndex.ShopifyOrderID, 0, 0, 19);
			base.AddElementFieldInfo("ShopifyOrderEntity", "FulfillmentStatusCode", typeof(System.Int32), false, false, false, false,  (int)ShopifyOrderFieldIndex.FulfillmentStatusCode, 0, 0, 10);
			base.AddElementFieldInfo("ShopifyOrderEntity", "PaymentStatusCode", typeof(System.Int32), false, false, false, false,  (int)ShopifyOrderFieldIndex.PaymentStatusCode, 0, 0, 10);
		}
		/// <summary>Inits ShopifyOrderItemEntity's FieldInfo objects</summary>
		private void InitShopifyOrderItemEntityInfos()
		{
			base.AddElementFieldInfo("ShopifyOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, false, false,  (int)ShopifyOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			base.AddElementFieldInfo("ShopifyOrderItemEntity", "ShopifyOrderItemID", typeof(System.Int64), false, false, false, false,  (int)ShopifyOrderItemFieldIndex.ShopifyOrderItemID, 0, 0, 19);
			base.AddElementFieldInfo("ShopifyOrderItemEntity", "ShopifyProductID", typeof(System.Int64), false, false, false, false,  (int)ShopifyOrderItemFieldIndex.ShopifyProductID, 0, 0, 19);
		}
		/// <summary>Inits ShopifyStoreEntity's FieldInfo objects</summary>
		private void InitShopifyStoreEntityInfos()
		{
			base.AddElementFieldInfo("ShopifyStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)ShopifyStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("ShopifyStoreEntity", "ShopifyShopUrlName", typeof(System.String), false, false, false, false,  (int)ShopifyStoreFieldIndex.ShopifyShopUrlName, 100, 0, 0);
			base.AddElementFieldInfo("ShopifyStoreEntity", "ShopifyShopDisplayName", typeof(System.String), false, false, false, false,  (int)ShopifyStoreFieldIndex.ShopifyShopDisplayName, 100, 0, 0);
			base.AddElementFieldInfo("ShopifyStoreEntity", "ShopifyAccessToken", typeof(System.String), false, false, false, false,  (int)ShopifyStoreFieldIndex.ShopifyAccessToken, 255, 0, 0);
			base.AddElementFieldInfo("ShopifyStoreEntity", "ShopifyRequestedShippingOption", typeof(System.Int32), false, false, false, false,  (int)ShopifyStoreFieldIndex.ShopifyRequestedShippingOption, 0, 0, 10);
		}
		/// <summary>Inits ShopSiteStoreEntity's FieldInfo objects</summary>
		private void InitShopSiteStoreEntityInfos()
		{
			base.AddElementFieldInfo("ShopSiteStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)ShopSiteStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("ShopSiteStoreEntity", "Username", typeof(System.String), false, false, false, false,  (int)ShopSiteStoreFieldIndex.Username, 50, 0, 0);
			base.AddElementFieldInfo("ShopSiteStoreEntity", "Password", typeof(System.String), false, false, false, false,  (int)ShopSiteStoreFieldIndex.Password, 50, 0, 0);
			base.AddElementFieldInfo("ShopSiteStoreEntity", "CgiUrl", typeof(System.String), false, false, false, false,  (int)ShopSiteStoreFieldIndex.CgiUrl, 350, 0, 0);
			base.AddElementFieldInfo("ShopSiteStoreEntity", "RequireSSL", typeof(System.Boolean), false, false, false, false,  (int)ShopSiteStoreFieldIndex.RequireSSL, 0, 0, 0);
			base.AddElementFieldInfo("ShopSiteStoreEntity", "DownloadPageSize", typeof(System.Int32), false, false, false, false,  (int)ShopSiteStoreFieldIndex.DownloadPageSize, 0, 0, 10);
			base.AddElementFieldInfo("ShopSiteStoreEntity", "RequestTimeout", typeof(System.Int32), false, false, false, false,  (int)ShopSiteStoreFieldIndex.RequestTimeout, 0, 0, 10);
		}
		/// <summary>Inits StatusPresetEntity's FieldInfo objects</summary>
		private void InitStatusPresetEntityInfos()
		{
			base.AddElementFieldInfo("StatusPresetEntity", "StatusPresetID", typeof(System.Int64), true, false, true, false,  (int)StatusPresetFieldIndex.StatusPresetID, 0, 0, 19);
			base.AddElementFieldInfo("StatusPresetEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)StatusPresetFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("StatusPresetEntity", "StoreID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)StatusPresetFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("StatusPresetEntity", "StatusTarget", typeof(System.Int32), false, false, false, false,  (int)StatusPresetFieldIndex.StatusTarget, 0, 0, 10);
			base.AddElementFieldInfo("StatusPresetEntity", "StatusText", typeof(System.String), false, false, false, false,  (int)StatusPresetFieldIndex.StatusText, 300, 0, 0);
			base.AddElementFieldInfo("StatusPresetEntity", "IsDefault", typeof(System.Boolean), false, false, false, false,  (int)StatusPresetFieldIndex.IsDefault, 0, 0, 0);
		}
		/// <summary>Inits StoreEntity's FieldInfo objects</summary>
		private void InitStoreEntityInfos()
		{
			base.AddElementFieldInfo("StoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)StoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("StoreEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)StoreFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "License", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.License, 150, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "Edition", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Edition, 2147483647, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "TypeCode", typeof(System.Int32), false, false, false, false,  (int)StoreFieldIndex.TypeCode, 0, 0, 10);
			base.AddElementFieldInfo("StoreEntity", "Enabled", typeof(System.Boolean), false, false, false, false,  (int)StoreFieldIndex.Enabled, 0, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "SetupComplete", typeof(System.Boolean), false, false, false, false,  (int)StoreFieldIndex.SetupComplete, 0, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "StoreName", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.StoreName, 75, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "Company", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Company, 60, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "Street1", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Street1, 60, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "Street2", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Street2, 60, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "Street3", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Street3, 60, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "City", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.City, 50, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.StateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.PostalCode, 20, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.CountryCode, 50, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "Phone", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Phone, 25, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "Fax", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Fax, 35, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "Email", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Email, 100, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "Website", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.Website, 50, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "AutoDownload", typeof(System.Boolean), false, false, false, false,  (int)StoreFieldIndex.AutoDownload, 0, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "AutoDownloadMinutes", typeof(System.Int32), false, false, false, false,  (int)StoreFieldIndex.AutoDownloadMinutes, 0, 0, 10);
			base.AddElementFieldInfo("StoreEntity", "AutoDownloadOnlyAway", typeof(System.Boolean), false, false, false, false,  (int)StoreFieldIndex.AutoDownloadOnlyAway, 0, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "AddressValidationSetting", typeof(System.Int32), false, false, false, false,  (int)StoreFieldIndex.AddressValidationSetting, 0, 0, 10);
			base.AddElementFieldInfo("StoreEntity", "ComputerDownloadPolicy", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.ComputerDownloadPolicy, 2147483647, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "DefaultEmailAccountID", typeof(System.Int64), false, false, false, false,  (int)StoreFieldIndex.DefaultEmailAccountID, 0, 0, 19);
			base.AddElementFieldInfo("StoreEntity", "ManualOrderPrefix", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.ManualOrderPrefix, 10, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "ManualOrderPostfix", typeof(System.String), false, false, false, false,  (int)StoreFieldIndex.ManualOrderPostfix, 10, 0, 0);
			base.AddElementFieldInfo("StoreEntity", "InitialDownloadDays", typeof(Nullable<System.Int32>), false, false, false, true,  (int)StoreFieldIndex.InitialDownloadDays, 0, 0, 10);
			base.AddElementFieldInfo("StoreEntity", "InitialDownloadOrder", typeof(Nullable<System.Int64>), false, false, false, true,  (int)StoreFieldIndex.InitialDownloadOrder, 0, 0, 19);
		}
		/// <summary>Inits SystemDataEntity's FieldInfo objects</summary>
		private void InitSystemDataEntityInfos()
		{
			base.AddElementFieldInfo("SystemDataEntity", "SystemDataID", typeof(System.Boolean), true, false, false, false,  (int)SystemDataFieldIndex.SystemDataID, 0, 0, 0);
			base.AddElementFieldInfo("SystemDataEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)SystemDataFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("SystemDataEntity", "DatabaseID", typeof(System.Guid), false, false, false, false,  (int)SystemDataFieldIndex.DatabaseID, 0, 0, 0);
			base.AddElementFieldInfo("SystemDataEntity", "DateFiltersLastUpdate", typeof(System.DateTime), false, false, false, false,  (int)SystemDataFieldIndex.DateFiltersLastUpdate, 0, 0, 0);
			base.AddElementFieldInfo("SystemDataEntity", "TemplateVersion", typeof(System.String), false, false, false, false,  (int)SystemDataFieldIndex.TemplateVersion, 30, 0, 0);
		}
		/// <summary>Inits TemplateEntity's FieldInfo objects</summary>
		private void InitTemplateEntityInfos()
		{
			base.AddElementFieldInfo("TemplateEntity", "TemplateID", typeof(System.Int64), true, false, true, false,  (int)TemplateFieldIndex.TemplateID, 0, 0, 19);
			base.AddElementFieldInfo("TemplateEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)TemplateFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("TemplateEntity", "ParentFolderID", typeof(System.Int64), false, true, false, false,  (int)TemplateFieldIndex.ParentFolderID, 0, 0, 19);
			base.AddElementFieldInfo("TemplateEntity", "Name", typeof(System.String), false, false, false, false,  (int)TemplateFieldIndex.Name, 100, 0, 0);
			base.AddElementFieldInfo("TemplateEntity", "Xsl", typeof(System.String), false, false, false, false,  (int)TemplateFieldIndex.Xsl, 2147483647, 0, 0);
			base.AddElementFieldInfo("TemplateEntity", "Type", typeof(System.Int32), false, false, false, false,  (int)TemplateFieldIndex.Type, 0, 0, 10);
			base.AddElementFieldInfo("TemplateEntity", "Context", typeof(System.Int32), false, false, false, false,  (int)TemplateFieldIndex.Context, 0, 0, 10);
			base.AddElementFieldInfo("TemplateEntity", "OutputFormat", typeof(System.Int32), false, false, false, false,  (int)TemplateFieldIndex.OutputFormat, 0, 0, 10);
			base.AddElementFieldInfo("TemplateEntity", "OutputEncoding", typeof(System.String), false, false, false, false,  (int)TemplateFieldIndex.OutputEncoding, 20, 0, 0);
			base.AddElementFieldInfo("TemplateEntity", "PageMarginLeft", typeof(System.Double), false, false, false, false,  (int)TemplateFieldIndex.PageMarginLeft, 0, 0, 38);
			base.AddElementFieldInfo("TemplateEntity", "PageMarginRight", typeof(System.Double), false, false, false, false,  (int)TemplateFieldIndex.PageMarginRight, 0, 0, 38);
			base.AddElementFieldInfo("TemplateEntity", "PageMarginBottom", typeof(System.Double), false, false, false, false,  (int)TemplateFieldIndex.PageMarginBottom, 0, 0, 38);
			base.AddElementFieldInfo("TemplateEntity", "PageMarginTop", typeof(System.Double), false, false, false, false,  (int)TemplateFieldIndex.PageMarginTop, 0, 0, 38);
			base.AddElementFieldInfo("TemplateEntity", "PageWidth", typeof(System.Double), false, false, false, false,  (int)TemplateFieldIndex.PageWidth, 0, 0, 38);
			base.AddElementFieldInfo("TemplateEntity", "PageHeight", typeof(System.Double), false, false, false, false,  (int)TemplateFieldIndex.PageHeight, 0, 0, 38);
			base.AddElementFieldInfo("TemplateEntity", "LabelSheetID", typeof(System.Int64), false, false, false, false,  (int)TemplateFieldIndex.LabelSheetID, 0, 0, 19);
			base.AddElementFieldInfo("TemplateEntity", "PrintCopies", typeof(System.Int32), false, false, false, false,  (int)TemplateFieldIndex.PrintCopies, 0, 0, 10);
			base.AddElementFieldInfo("TemplateEntity", "PrintCollate", typeof(System.Boolean), false, false, false, false,  (int)TemplateFieldIndex.PrintCollate, 0, 0, 0);
			base.AddElementFieldInfo("TemplateEntity", "SaveFileName", typeof(System.String), false, false, false, false,  (int)TemplateFieldIndex.SaveFileName, 500, 0, 0);
			base.AddElementFieldInfo("TemplateEntity", "SaveFileFolder", typeof(System.String), false, false, false, false,  (int)TemplateFieldIndex.SaveFileFolder, 500, 0, 0);
			base.AddElementFieldInfo("TemplateEntity", "SaveFilePrompt", typeof(System.Int32), false, false, false, false,  (int)TemplateFieldIndex.SaveFilePrompt, 0, 0, 10);
			base.AddElementFieldInfo("TemplateEntity", "SaveFileBOM", typeof(System.Boolean), false, false, false, false,  (int)TemplateFieldIndex.SaveFileBOM, 0, 0, 0);
			base.AddElementFieldInfo("TemplateEntity", "SaveFileOnlineResources", typeof(System.Boolean), false, false, false, false,  (int)TemplateFieldIndex.SaveFileOnlineResources, 0, 0, 0);
		}
		/// <summary>Inits TemplateComputerSettingsEntity's FieldInfo objects</summary>
		private void InitTemplateComputerSettingsEntityInfos()
		{
			base.AddElementFieldInfo("TemplateComputerSettingsEntity", "TemplateComputerSettingsID", typeof(System.Int64), true, false, true, false,  (int)TemplateComputerSettingsFieldIndex.TemplateComputerSettingsID, 0, 0, 19);
			base.AddElementFieldInfo("TemplateComputerSettingsEntity", "TemplateID", typeof(System.Int64), false, true, false, false,  (int)TemplateComputerSettingsFieldIndex.TemplateID, 0, 0, 19);
			base.AddElementFieldInfo("TemplateComputerSettingsEntity", "ComputerID", typeof(System.Int64), false, true, false, false,  (int)TemplateComputerSettingsFieldIndex.ComputerID, 0, 0, 19);
			base.AddElementFieldInfo("TemplateComputerSettingsEntity", "PrinterName", typeof(System.String), false, false, false, false,  (int)TemplateComputerSettingsFieldIndex.PrinterName, 350, 0, 0);
			base.AddElementFieldInfo("TemplateComputerSettingsEntity", "PaperSource", typeof(System.Int32), false, false, false, false,  (int)TemplateComputerSettingsFieldIndex.PaperSource, 0, 0, 10);
		}
		/// <summary>Inits TemplateFolderEntity's FieldInfo objects</summary>
		private void InitTemplateFolderEntityInfos()
		{
			base.AddElementFieldInfo("TemplateFolderEntity", "TemplateFolderID", typeof(System.Int64), true, false, true, false,  (int)TemplateFolderFieldIndex.TemplateFolderID, 0, 0, 19);
			base.AddElementFieldInfo("TemplateFolderEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)TemplateFolderFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("TemplateFolderEntity", "ParentFolderID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)TemplateFolderFieldIndex.ParentFolderID, 0, 0, 19);
			base.AddElementFieldInfo("TemplateFolderEntity", "Name", typeof(System.String), false, false, false, false,  (int)TemplateFolderFieldIndex.Name, 100, 0, 0);
		}
		/// <summary>Inits TemplateStoreSettingsEntity's FieldInfo objects</summary>
		private void InitTemplateStoreSettingsEntityInfos()
		{
			base.AddElementFieldInfo("TemplateStoreSettingsEntity", "TemplateStoreSettingsID", typeof(System.Int64), true, false, true, false,  (int)TemplateStoreSettingsFieldIndex.TemplateStoreSettingsID, 0, 0, 19);
			base.AddElementFieldInfo("TemplateStoreSettingsEntity", "TemplateID", typeof(System.Int64), false, true, false, false,  (int)TemplateStoreSettingsFieldIndex.TemplateID, 0, 0, 19);
			base.AddElementFieldInfo("TemplateStoreSettingsEntity", "StoreID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)TemplateStoreSettingsFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("TemplateStoreSettingsEntity", "EmailUseDefault", typeof(System.Boolean), false, false, false, false,  (int)TemplateStoreSettingsFieldIndex.EmailUseDefault, 0, 0, 0);
			base.AddElementFieldInfo("TemplateStoreSettingsEntity", "EmailAccountID", typeof(System.Int64), false, false, false, false,  (int)TemplateStoreSettingsFieldIndex.EmailAccountID, 0, 0, 19);
			base.AddElementFieldInfo("TemplateStoreSettingsEntity", "EmailTo", typeof(System.String), false, false, false, false,  (int)TemplateStoreSettingsFieldIndex.EmailTo, 2147483647, 0, 0);
			base.AddElementFieldInfo("TemplateStoreSettingsEntity", "EmailCc", typeof(System.String), false, false, false, false,  (int)TemplateStoreSettingsFieldIndex.EmailCc, 2147483647, 0, 0);
			base.AddElementFieldInfo("TemplateStoreSettingsEntity", "EmailBcc", typeof(System.String), false, false, false, false,  (int)TemplateStoreSettingsFieldIndex.EmailBcc, 2147483647, 0, 0);
			base.AddElementFieldInfo("TemplateStoreSettingsEntity", "EmailSubject", typeof(System.String), false, false, false, false,  (int)TemplateStoreSettingsFieldIndex.EmailSubject, 500, 0, 0);
		}
		/// <summary>Inits TemplateUserSettingsEntity's FieldInfo objects</summary>
		private void InitTemplateUserSettingsEntityInfos()
		{
			base.AddElementFieldInfo("TemplateUserSettingsEntity", "TemplateUserSettingsID", typeof(System.Int64), true, false, true, false,  (int)TemplateUserSettingsFieldIndex.TemplateUserSettingsID, 0, 0, 19);
			base.AddElementFieldInfo("TemplateUserSettingsEntity", "TemplateID", typeof(System.Int64), false, true, false, false,  (int)TemplateUserSettingsFieldIndex.TemplateID, 0, 0, 19);
			base.AddElementFieldInfo("TemplateUserSettingsEntity", "UserID", typeof(System.Int64), false, true, false, false,  (int)TemplateUserSettingsFieldIndex.UserID, 0, 0, 19);
			base.AddElementFieldInfo("TemplateUserSettingsEntity", "PreviewSource", typeof(System.Int32), false, false, false, false,  (int)TemplateUserSettingsFieldIndex.PreviewSource, 0, 0, 10);
			base.AddElementFieldInfo("TemplateUserSettingsEntity", "PreviewCount", typeof(System.Int32), false, false, false, false,  (int)TemplateUserSettingsFieldIndex.PreviewCount, 0, 0, 10);
			base.AddElementFieldInfo("TemplateUserSettingsEntity", "PreviewFilterNodeID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)TemplateUserSettingsFieldIndex.PreviewFilterNodeID, 0, 0, 19);
			base.AddElementFieldInfo("TemplateUserSettingsEntity", "PreviewZoom", typeof(System.String), false, false, false, false,  (int)TemplateUserSettingsFieldIndex.PreviewZoom, 10, 0, 0);
		}
		/// <summary>Inits ThreeDCartOrderItemEntity's FieldInfo objects</summary>
		private void InitThreeDCartOrderItemEntityInfos()
		{
			base.AddElementFieldInfo("ThreeDCartOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, false, false,  (int)ThreeDCartOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			base.AddElementFieldInfo("ThreeDCartOrderItemEntity", "ThreeDCartShipmentID", typeof(System.Int64), false, false, false, false,  (int)ThreeDCartOrderItemFieldIndex.ThreeDCartShipmentID, 0, 0, 19);
		}
		/// <summary>Inits ThreeDCartStoreEntity's FieldInfo objects</summary>
		private void InitThreeDCartStoreEntityInfos()
		{
			base.AddElementFieldInfo("ThreeDCartStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)ThreeDCartStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("ThreeDCartStoreEntity", "StoreUrl", typeof(System.String), false, false, false, false,  (int)ThreeDCartStoreFieldIndex.StoreUrl, 110, 0, 0);
			base.AddElementFieldInfo("ThreeDCartStoreEntity", "ApiUserKey", typeof(System.String), false, false, false, false,  (int)ThreeDCartStoreFieldIndex.ApiUserKey, 65, 0, 0);
			base.AddElementFieldInfo("ThreeDCartStoreEntity", "TimeZoneID", typeof(System.String), false, false, false, true,  (int)ThreeDCartStoreFieldIndex.TimeZoneID, 100, 0, 0);
			base.AddElementFieldInfo("ThreeDCartStoreEntity", "StatusCodes", typeof(System.String), false, false, false, true,  (int)ThreeDCartStoreFieldIndex.StatusCodes, 2147483647, 0, 0);
			base.AddElementFieldInfo("ThreeDCartStoreEntity", "DownloadModifiedNumberOfDaysBack", typeof(System.Int32), false, false, false, false,  (int)ThreeDCartStoreFieldIndex.DownloadModifiedNumberOfDaysBack, 0, 0, 10);
		}
		/// <summary>Inits UpsAccountEntity's FieldInfo objects</summary>
		private void InitUpsAccountEntityInfos()
		{
			base.AddElementFieldInfo("UpsAccountEntity", "UpsAccountID", typeof(System.Int64), true, false, true, false,  (int)UpsAccountFieldIndex.UpsAccountID, 0, 0, 19);
			base.AddElementFieldInfo("UpsAccountEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)UpsAccountFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "Description", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Description, 50, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "AccountNumber", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.AccountNumber, 10, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "UserID", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.UserID, 25, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "Password", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Password, 25, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "RateType", typeof(System.Int32), false, false, false, false,  (int)UpsAccountFieldIndex.RateType, 0, 0, 10);
			base.AddElementFieldInfo("UpsAccountEntity", "InvoiceAuth", typeof(System.Boolean), false, false, false, false,  (int)UpsAccountFieldIndex.InvoiceAuth, 0, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "FirstName", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.FirstName, 30, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "MiddleName", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.MiddleName, 30, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "LastName", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.LastName, 30, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "Company", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Company, 30, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "Street1", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Street1, 60, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "Street2", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Street2, 60, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "Street3", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Street3, 60, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "City", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.City, 50, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.StateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.PostalCode, 20, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.CountryCode, 50, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "Phone", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Phone, 25, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "Email", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Email, 100, 0, 0);
			base.AddElementFieldInfo("UpsAccountEntity", "Website", typeof(System.String), false, false, false, false,  (int)UpsAccountFieldIndex.Website, 50, 0, 0);
		}
		/// <summary>Inits UpsPackageEntity's FieldInfo objects</summary>
		private void InitUpsPackageEntityInfos()
		{
			base.AddElementFieldInfo("UpsPackageEntity", "UpsPackageID", typeof(System.Int64), true, false, true, false,  (int)UpsPackageFieldIndex.UpsPackageID, 0, 0, 19);
			base.AddElementFieldInfo("UpsPackageEntity", "ShipmentID", typeof(System.Int64), false, true, false, false,  (int)UpsPackageFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("UpsPackageEntity", "PackagingType", typeof(System.Int32), false, false, false, false,  (int)UpsPackageFieldIndex.PackagingType, 0, 0, 10);
			base.AddElementFieldInfo("UpsPackageEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)UpsPackageFieldIndex.Weight, 0, 0, 38);
			base.AddElementFieldInfo("UpsPackageEntity", "DimsProfileID", typeof(System.Int64), false, false, false, false,  (int)UpsPackageFieldIndex.DimsProfileID, 0, 0, 19);
			base.AddElementFieldInfo("UpsPackageEntity", "DimsLength", typeof(System.Double), false, false, false, false,  (int)UpsPackageFieldIndex.DimsLength, 0, 0, 38);
			base.AddElementFieldInfo("UpsPackageEntity", "DimsWidth", typeof(System.Double), false, false, false, false,  (int)UpsPackageFieldIndex.DimsWidth, 0, 0, 38);
			base.AddElementFieldInfo("UpsPackageEntity", "DimsHeight", typeof(System.Double), false, false, false, false,  (int)UpsPackageFieldIndex.DimsHeight, 0, 0, 38);
			base.AddElementFieldInfo("UpsPackageEntity", "DimsWeight", typeof(System.Double), false, false, false, false,  (int)UpsPackageFieldIndex.DimsWeight, 0, 0, 38);
			base.AddElementFieldInfo("UpsPackageEntity", "DimsAddWeight", typeof(System.Boolean), false, false, false, false,  (int)UpsPackageFieldIndex.DimsAddWeight, 0, 0, 0);
			base.AddElementFieldInfo("UpsPackageEntity", "Insurance", typeof(System.Boolean), false, false, false, false,  (int)UpsPackageFieldIndex.Insurance, 0, 0, 0);
			base.AddElementFieldInfo("UpsPackageEntity", "InsuranceValue", typeof(System.Decimal), false, false, false, false,  (int)UpsPackageFieldIndex.InsuranceValue, 0, 4, 19);
			base.AddElementFieldInfo("UpsPackageEntity", "InsurancePennyOne", typeof(System.Boolean), false, false, false, false,  (int)UpsPackageFieldIndex.InsurancePennyOne, 0, 0, 0);
			base.AddElementFieldInfo("UpsPackageEntity", "DeclaredValue", typeof(System.Decimal), false, false, false, false,  (int)UpsPackageFieldIndex.DeclaredValue, 0, 4, 19);
			base.AddElementFieldInfo("UpsPackageEntity", "TrackingNumber", typeof(System.String), false, false, false, false,  (int)UpsPackageFieldIndex.TrackingNumber, 50, 0, 0);
			base.AddElementFieldInfo("UpsPackageEntity", "UspsTrackingNumber", typeof(System.String), false, false, false, false,  (int)UpsPackageFieldIndex.UspsTrackingNumber, 50, 0, 0);
			base.AddElementFieldInfo("UpsPackageEntity", "AdditionalHandlingEnabled", typeof(System.Boolean), false, false, false, false,  (int)UpsPackageFieldIndex.AdditionalHandlingEnabled, 0, 0, 0);
			base.AddElementFieldInfo("UpsPackageEntity", "VerbalConfirmationEnabled", typeof(System.Boolean), false, false, false, false,  (int)UpsPackageFieldIndex.VerbalConfirmationEnabled, 0, 0, 0);
			base.AddElementFieldInfo("UpsPackageEntity", "VerbalConfirmationName", typeof(System.String), false, false, false, false,  (int)UpsPackageFieldIndex.VerbalConfirmationName, 35, 0, 0);
			base.AddElementFieldInfo("UpsPackageEntity", "VerbalConfirmationPhone", typeof(System.String), false, false, false, false,  (int)UpsPackageFieldIndex.VerbalConfirmationPhone, 15, 0, 0);
			base.AddElementFieldInfo("UpsPackageEntity", "VerbalConfirmationPhoneExtension", typeof(System.String), false, false, false, false,  (int)UpsPackageFieldIndex.VerbalConfirmationPhoneExtension, 4, 0, 0);
			base.AddElementFieldInfo("UpsPackageEntity", "DryIceEnabled", typeof(System.Boolean), false, false, false, false,  (int)UpsPackageFieldIndex.DryIceEnabled, 0, 0, 0);
			base.AddElementFieldInfo("UpsPackageEntity", "DryIceRegulationSet", typeof(System.Int32), false, false, false, false,  (int)UpsPackageFieldIndex.DryIceRegulationSet, 0, 0, 10);
			base.AddElementFieldInfo("UpsPackageEntity", "DryIceWeight", typeof(System.Double), false, false, false, false,  (int)UpsPackageFieldIndex.DryIceWeight, 0, 0, 38);
			base.AddElementFieldInfo("UpsPackageEntity", "DryIceIsForMedicalUse", typeof(System.Boolean), false, false, false, false,  (int)UpsPackageFieldIndex.DryIceIsForMedicalUse, 0, 0, 0);
		}
		/// <summary>Inits UpsProfileEntity's FieldInfo objects</summary>
		private void InitUpsProfileEntityInfos()
		{
			base.AddElementFieldInfo("UpsProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)UpsProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			base.AddElementFieldInfo("UpsProfileEntity", "UpsAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)UpsProfileFieldIndex.UpsAccountID, 0, 0, 19);
			base.AddElementFieldInfo("UpsProfileEntity", "Service", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.Service, 0, 0, 10);
			base.AddElementFieldInfo("UpsProfileEntity", "SaturdayDelivery", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfileFieldIndex.SaturdayDelivery, 0, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "ResidentialDetermination", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.ResidentialDetermination, 0, 0, 10);
			base.AddElementFieldInfo("UpsProfileEntity", "DeliveryConfirmation", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.DeliveryConfirmation, 0, 0, 10);
			base.AddElementFieldInfo("UpsProfileEntity", "ReferenceNumber", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.ReferenceNumber, 300, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "ReferenceNumber2", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.ReferenceNumber2, 300, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "PayorType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.PayorType, 0, 0, 10);
			base.AddElementFieldInfo("UpsProfileEntity", "PayorAccount", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.PayorAccount, 10, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "PayorPostalCode", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.PayorPostalCode, 20, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "PayorCountryCode", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.PayorCountryCode, 50, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "EmailNotifySender", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.EmailNotifySender, 0, 0, 10);
			base.AddElementFieldInfo("UpsProfileEntity", "EmailNotifyRecipient", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.EmailNotifyRecipient, 0, 0, 10);
			base.AddElementFieldInfo("UpsProfileEntity", "EmailNotifyOther", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.EmailNotifyOther, 0, 0, 10);
			base.AddElementFieldInfo("UpsProfileEntity", "EmailNotifyOtherAddress", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.EmailNotifyOtherAddress, 100, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "EmailNotifyFrom", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.EmailNotifyFrom, 100, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "EmailNotifySubject", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.EmailNotifySubject, 0, 0, 10);
			base.AddElementFieldInfo("UpsProfileEntity", "EmailNotifyMessage", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.EmailNotifyMessage, 120, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "ReturnService", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.ReturnService, 0, 0, 10);
			base.AddElementFieldInfo("UpsProfileEntity", "ReturnUndeliverableEmail", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.ReturnUndeliverableEmail, 100, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "ReturnContents", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.ReturnContents, 300, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "Endorsement", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.Endorsement, 0, 0, 10);
			base.AddElementFieldInfo("UpsProfileEntity", "Subclassification", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.Subclassification, 0, 0, 10);
			base.AddElementFieldInfo("UpsProfileEntity", "PaperlessAdditionalDocumentation", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfileFieldIndex.PaperlessAdditionalDocumentation, 0, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "ShipperRelease", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfileFieldIndex.ShipperRelease, 0, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "CarbonNeutral", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfileFieldIndex.CarbonNeutral, 0, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "CommercialPaperlessInvoice", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfileFieldIndex.CommercialPaperlessInvoice, 0, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "CostCenter", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.CostCenter, 100, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "IrregularIndicator", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.IrregularIndicator, 0, 0, 10);
			base.AddElementFieldInfo("UpsProfileEntity", "Cn22Number", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.Cn22Number, 255, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "ShipmentChargeType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfileFieldIndex.ShipmentChargeType, 0, 0, 10);
			base.AddElementFieldInfo("UpsProfileEntity", "ShipmentChargeAccount", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.ShipmentChargeAccount, 10, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "ShipmentChargePostalCode", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.ShipmentChargePostalCode, 20, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "ShipmentChargeCountryCode", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.ShipmentChargeCountryCode, 50, 0, 0);
			base.AddElementFieldInfo("UpsProfileEntity", "UspsPackageID", typeof(System.String), false, false, false, true,  (int)UpsProfileFieldIndex.UspsPackageID, 100, 0, 0);
		}
		/// <summary>Inits UpsProfilePackageEntity's FieldInfo objects</summary>
		private void InitUpsProfilePackageEntityInfos()
		{
			base.AddElementFieldInfo("UpsProfilePackageEntity", "UpsProfilePackageID", typeof(System.Int64), true, false, true, false,  (int)UpsProfilePackageFieldIndex.UpsProfilePackageID, 0, 0, 19);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "ShippingProfileID", typeof(System.Int64), false, true, false, false,  (int)UpsProfilePackageFieldIndex.ShippingProfileID, 0, 0, 19);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "PackagingType", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.PackagingType, 0, 0, 10);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "Weight", typeof(Nullable<System.Double>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.Weight, 0, 0, 38);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "DimsProfileID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DimsProfileID, 0, 0, 19);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "DimsLength", typeof(Nullable<System.Double>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DimsLength, 0, 0, 38);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "DimsWidth", typeof(Nullable<System.Double>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DimsWidth, 0, 0, 38);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "DimsHeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DimsHeight, 0, 0, 38);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "DimsWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DimsWeight, 0, 0, 38);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "DimsAddWeight", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DimsAddWeight, 0, 0, 0);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "AdditionalHandlingEnabled", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.AdditionalHandlingEnabled, 0, 0, 0);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "VerbalConfirmationEnabled", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.VerbalConfirmationEnabled, 0, 0, 0);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "VerbalConfirmationName", typeof(System.String), false, false, false, true,  (int)UpsProfilePackageFieldIndex.VerbalConfirmationName, 35, 0, 0);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "VerbalConfirmationPhone", typeof(System.String), false, false, false, true,  (int)UpsProfilePackageFieldIndex.VerbalConfirmationPhone, 15, 0, 0);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "VerbalConfirmationPhoneExtension", typeof(System.String), false, false, false, true,  (int)UpsProfilePackageFieldIndex.VerbalConfirmationPhoneExtension, 4, 0, 0);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "DryIceEnabled", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DryIceEnabled, 0, 0, 0);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "DryIceRegulationSet", typeof(Nullable<System.Int32>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DryIceRegulationSet, 0, 0, 10);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "DryIceWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DryIceWeight, 0, 0, 38);
			base.AddElementFieldInfo("UpsProfilePackageEntity", "DryIceIsForMedicalUse", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UpsProfilePackageFieldIndex.DryIceIsForMedicalUse, 0, 0, 0);
		}
		/// <summary>Inits UpsShipmentEntity's FieldInfo objects</summary>
		private void InitUpsShipmentEntityInfos()
		{
			base.AddElementFieldInfo("UpsShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)UpsShipmentFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("UpsShipmentEntity", "UpsAccountID", typeof(System.Int64), false, false, false, false,  (int)UpsShipmentFieldIndex.UpsAccountID, 0, 0, 19);
			base.AddElementFieldInfo("UpsShipmentEntity", "Service", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.Service, 0, 0, 10);
			base.AddElementFieldInfo("UpsShipmentEntity", "SaturdayDelivery", typeof(System.Boolean), false, false, false, false,  (int)UpsShipmentFieldIndex.SaturdayDelivery, 0, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "CodEnabled", typeof(System.Boolean), false, false, false, false,  (int)UpsShipmentFieldIndex.CodEnabled, 0, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "CodAmount", typeof(System.Decimal), false, false, false, false,  (int)UpsShipmentFieldIndex.CodAmount, 0, 4, 19);
			base.AddElementFieldInfo("UpsShipmentEntity", "CodPaymentType", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.CodPaymentType, 0, 0, 10);
			base.AddElementFieldInfo("UpsShipmentEntity", "DeliveryConfirmation", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.DeliveryConfirmation, 0, 0, 10);
			base.AddElementFieldInfo("UpsShipmentEntity", "ReferenceNumber", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.ReferenceNumber, 300, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "ReferenceNumber2", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.ReferenceNumber2, 300, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "PayorType", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.PayorType, 0, 0, 10);
			base.AddElementFieldInfo("UpsShipmentEntity", "PayorAccount", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.PayorAccount, 10, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "PayorPostalCode", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.PayorPostalCode, 20, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "PayorCountryCode", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.PayorCountryCode, 50, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "EmailNotifySender", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.EmailNotifySender, 0, 0, 10);
			base.AddElementFieldInfo("UpsShipmentEntity", "EmailNotifyRecipient", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.EmailNotifyRecipient, 0, 0, 10);
			base.AddElementFieldInfo("UpsShipmentEntity", "EmailNotifyOther", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.EmailNotifyOther, 0, 0, 10);
			base.AddElementFieldInfo("UpsShipmentEntity", "EmailNotifyOtherAddress", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.EmailNotifyOtherAddress, 100, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "EmailNotifyFrom", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.EmailNotifyFrom, 100, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "EmailNotifySubject", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.EmailNotifySubject, 0, 0, 10);
			base.AddElementFieldInfo("UpsShipmentEntity", "EmailNotifyMessage", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.EmailNotifyMessage, 120, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "CustomsDocumentsOnly", typeof(System.Boolean), false, false, false, false,  (int)UpsShipmentFieldIndex.CustomsDocumentsOnly, 0, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "CustomsDescription", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.CustomsDescription, 150, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "CommercialPaperlessInvoice", typeof(System.Boolean), false, false, false, false,  (int)UpsShipmentFieldIndex.CommercialPaperlessInvoice, 0, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "CommercialInvoiceTermsOfSale", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.CommercialInvoiceTermsOfSale, 0, 0, 10);
			base.AddElementFieldInfo("UpsShipmentEntity", "CommercialInvoicePurpose", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.CommercialInvoicePurpose, 0, 0, 10);
			base.AddElementFieldInfo("UpsShipmentEntity", "CommercialInvoiceComments", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.CommercialInvoiceComments, 200, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "CommercialInvoiceFreight", typeof(System.Decimal), false, false, false, false,  (int)UpsShipmentFieldIndex.CommercialInvoiceFreight, 0, 4, 19);
			base.AddElementFieldInfo("UpsShipmentEntity", "CommercialInvoiceInsurance", typeof(System.Decimal), false, false, false, false,  (int)UpsShipmentFieldIndex.CommercialInvoiceInsurance, 0, 4, 19);
			base.AddElementFieldInfo("UpsShipmentEntity", "CommercialInvoiceOther", typeof(System.Decimal), false, false, false, false,  (int)UpsShipmentFieldIndex.CommercialInvoiceOther, 0, 4, 19);
			base.AddElementFieldInfo("UpsShipmentEntity", "WorldShipStatus", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.WorldShipStatus, 0, 0, 10);
			base.AddElementFieldInfo("UpsShipmentEntity", "PublishedCharges", typeof(System.Decimal), false, false, false, false,  (int)UpsShipmentFieldIndex.PublishedCharges, 0, 4, 19);
			base.AddElementFieldInfo("UpsShipmentEntity", "NegotiatedRate", typeof(System.Boolean), false, false, false, false,  (int)UpsShipmentFieldIndex.NegotiatedRate, 0, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "ReturnService", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.ReturnService, 0, 0, 10);
			base.AddElementFieldInfo("UpsShipmentEntity", "ReturnUndeliverableEmail", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.ReturnUndeliverableEmail, 100, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "ReturnContents", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.ReturnContents, 300, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "UspsTrackingNumber", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.UspsTrackingNumber, 50, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "Endorsement", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.Endorsement, 0, 0, 10);
			base.AddElementFieldInfo("UpsShipmentEntity", "Subclassification", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.Subclassification, 0, 0, 10);
			base.AddElementFieldInfo("UpsShipmentEntity", "PaperlessAdditionalDocumentation", typeof(System.Boolean), false, false, false, false,  (int)UpsShipmentFieldIndex.PaperlessAdditionalDocumentation, 0, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "ShipperRelease", typeof(System.Boolean), false, false, false, false,  (int)UpsShipmentFieldIndex.ShipperRelease, 0, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "CarbonNeutral", typeof(System.Boolean), false, false, false, false,  (int)UpsShipmentFieldIndex.CarbonNeutral, 0, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "CostCenter", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.CostCenter, 100, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "IrregularIndicator", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.IrregularIndicator, 0, 0, 10);
			base.AddElementFieldInfo("UpsShipmentEntity", "Cn22Number", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.Cn22Number, 255, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "ShipmentChargeType", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.ShipmentChargeType, 0, 0, 10);
			base.AddElementFieldInfo("UpsShipmentEntity", "ShipmentChargeAccount", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.ShipmentChargeAccount, 10, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "ShipmentChargePostalCode", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.ShipmentChargePostalCode, 20, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "ShipmentChargeCountryCode", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.ShipmentChargeCountryCode, 50, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "UspsPackageID", typeof(System.String), false, false, false, false,  (int)UpsShipmentFieldIndex.UspsPackageID, 100, 0, 0);
			base.AddElementFieldInfo("UpsShipmentEntity", "RequestedLabelFormat", typeof(System.Int32), false, false, false, false,  (int)UpsShipmentFieldIndex.RequestedLabelFormat, 0, 0, 10);
		}
		/// <summary>Inits UserEntity's FieldInfo objects</summary>
		private void InitUserEntityInfos()
		{
			base.AddElementFieldInfo("UserEntity", "UserID", typeof(System.Int64), true, false, true, false,  (int)UserFieldIndex.UserID, 0, 0, 19);
			base.AddElementFieldInfo("UserEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)UserFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("UserEntity", "Username", typeof(System.String), false, false, false, false,  (int)UserFieldIndex.Username, 30, 0, 0);
			base.AddElementFieldInfo("UserEntity", "Password", typeof(System.String), false, false, false, false,  (int)UserFieldIndex.Password, 32, 0, 0);
			base.AddElementFieldInfo("UserEntity", "Email", typeof(System.String), false, false, false, false,  (int)UserFieldIndex.Email, 255, 0, 0);
			base.AddElementFieldInfo("UserEntity", "IsAdmin", typeof(System.Boolean), false, false, false, false,  (int)UserFieldIndex.IsAdmin, 0, 0, 0);
			base.AddElementFieldInfo("UserEntity", "IsDeleted", typeof(System.Boolean), false, false, false, false,  (int)UserFieldIndex.IsDeleted, 0, 0, 0);
		}
		/// <summary>Inits UserColumnSettingsEntity's FieldInfo objects</summary>
		private void InitUserColumnSettingsEntityInfos()
		{
			base.AddElementFieldInfo("UserColumnSettingsEntity", "UserColumnSettingsID", typeof(System.Int64), true, false, true, false,  (int)UserColumnSettingsFieldIndex.UserColumnSettingsID, 0, 0, 19);
			base.AddElementFieldInfo("UserColumnSettingsEntity", "SettingsKey", typeof(System.Guid), false, false, false, false,  (int)UserColumnSettingsFieldIndex.SettingsKey, 0, 0, 0);
			base.AddElementFieldInfo("UserColumnSettingsEntity", "UserID", typeof(System.Int64), false, true, false, false,  (int)UserColumnSettingsFieldIndex.UserID, 0, 0, 19);
			base.AddElementFieldInfo("UserColumnSettingsEntity", "InitialSortType", typeof(System.Int32), false, false, false, false,  (int)UserColumnSettingsFieldIndex.InitialSortType, 0, 0, 10);
			base.AddElementFieldInfo("UserColumnSettingsEntity", "GridColumnLayoutID", typeof(System.Int64), false, true, false, false,  (int)UserColumnSettingsFieldIndex.GridColumnLayoutID, 0, 0, 19);
		}
		/// <summary>Inits UserSettingsEntity's FieldInfo objects</summary>
		private void InitUserSettingsEntityInfos()
		{
			base.AddElementFieldInfo("UserSettingsEntity", "UserID", typeof(System.Int64), true, true, false, false,  (int)UserSettingsFieldIndex.UserID, 0, 0, 19);
			base.AddElementFieldInfo("UserSettingsEntity", "DisplayColorScheme", typeof(System.Int32), false, false, false, false,  (int)UserSettingsFieldIndex.DisplayColorScheme, 0, 0, 10);
			base.AddElementFieldInfo("UserSettingsEntity", "DisplaySystemTray", typeof(System.Boolean), false, false, false, false,  (int)UserSettingsFieldIndex.DisplaySystemTray, 0, 0, 0);
			base.AddElementFieldInfo("UserSettingsEntity", "WindowLayout", typeof(System.Byte[]), false, false, false, false,  (int)UserSettingsFieldIndex.WindowLayout, 2147483647, 0, 0);
			base.AddElementFieldInfo("UserSettingsEntity", "GridMenuLayout", typeof(System.String), false, false, false, true,  (int)UserSettingsFieldIndex.GridMenuLayout, 2147483647, 0, 0);
			base.AddElementFieldInfo("UserSettingsEntity", "FilterInitialUseLastActive", typeof(System.Boolean), false, false, false, false,  (int)UserSettingsFieldIndex.FilterInitialUseLastActive, 0, 0, 0);
			base.AddElementFieldInfo("UserSettingsEntity", "FilterInitialSpecified", typeof(System.Int64), false, false, false, false,  (int)UserSettingsFieldIndex.FilterInitialSpecified, 0, 0, 19);
			base.AddElementFieldInfo("UserSettingsEntity", "FilterInitialSortType", typeof(System.Int32), false, false, false, false,  (int)UserSettingsFieldIndex.FilterInitialSortType, 0, 0, 10);
			base.AddElementFieldInfo("UserSettingsEntity", "OrderFilterLastActive", typeof(System.Int64), false, false, false, false,  (int)UserSettingsFieldIndex.OrderFilterLastActive, 0, 0, 19);
			base.AddElementFieldInfo("UserSettingsEntity", "OrderFilterExpandedFolders", typeof(System.String), false, false, false, true,  (int)UserSettingsFieldIndex.OrderFilterExpandedFolders, 2147483647, 0, 0);
			base.AddElementFieldInfo("UserSettingsEntity", "ShippingWeightFormat", typeof(System.Int32), false, false, false, false,  (int)UserSettingsFieldIndex.ShippingWeightFormat, 0, 0, 10);
			base.AddElementFieldInfo("UserSettingsEntity", "TemplateExpandedFolders", typeof(System.String), false, false, false, true,  (int)UserSettingsFieldIndex.TemplateExpandedFolders, 2147483647, 0, 0);
			base.AddElementFieldInfo("UserSettingsEntity", "TemplateLastSelected", typeof(System.Int64), false, false, false, false,  (int)UserSettingsFieldIndex.TemplateLastSelected, 0, 0, 19);
			base.AddElementFieldInfo("UserSettingsEntity", "CustomerFilterLastActive", typeof(System.Int64), false, false, false, false,  (int)UserSettingsFieldIndex.CustomerFilterLastActive, 0, 0, 19);
			base.AddElementFieldInfo("UserSettingsEntity", "CustomerFilterExpandedFolders", typeof(System.String), false, false, false, true,  (int)UserSettingsFieldIndex.CustomerFilterExpandedFolders, 2147483647, 0, 0);
		}
		/// <summary>Inits UspsAccountEntity's FieldInfo objects</summary>
		private void InitUspsAccountEntityInfos()
		{
			base.AddElementFieldInfo("UspsAccountEntity", "UspsAccountID", typeof(System.Int64), true, false, true, false,  (int)UspsAccountFieldIndex.UspsAccountID, 0, 0, 19);
			base.AddElementFieldInfo("UspsAccountEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)UspsAccountFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "Description", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Description, 50, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "Username", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Username, 50, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "Password", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Password, 100, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "FirstName", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.FirstName, 30, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "MiddleName", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.MiddleName, 30, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "LastName", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.LastName, 30, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "Company", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Company, 30, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "Street1", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Street1, 60, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "Street2", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Street2, 60, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "Street3", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Street3, 60, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "City", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.City, 50, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.StateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.PostalCode, 20, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.CountryCode, 50, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "Phone", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Phone, 25, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "Email", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Email, 100, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "Website", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.Website, 50, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "MailingPostalCode", typeof(System.String), false, false, false, false,  (int)UspsAccountFieldIndex.MailingPostalCode, 20, 0, 0);
			base.AddElementFieldInfo("UspsAccountEntity", "UspsReseller", typeof(System.Int32), false, false, false, false,  (int)UspsAccountFieldIndex.UspsReseller, 0, 0, 10);
			base.AddElementFieldInfo("UspsAccountEntity", "ContractType", typeof(System.Int32), false, false, false, false,  (int)UspsAccountFieldIndex.ContractType, 0, 0, 10);
			base.AddElementFieldInfo("UspsAccountEntity", "CreatedDate", typeof(System.DateTime), false, false, false, false,  (int)UspsAccountFieldIndex.CreatedDate, 0, 0, 0);
		}
		/// <summary>Inits UspsProfileEntity's FieldInfo objects</summary>
		private void InitUspsProfileEntityInfos()
		{
			base.AddElementFieldInfo("UspsProfileEntity", "ShippingProfileID", typeof(System.Int64), true, true, false, false,  (int)UspsProfileFieldIndex.ShippingProfileID, 0, 0, 19);
			base.AddElementFieldInfo("UspsProfileEntity", "UspsAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)UspsProfileFieldIndex.UspsAccountID, 0, 0, 19);
			base.AddElementFieldInfo("UspsProfileEntity", "HidePostage", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UspsProfileFieldIndex.HidePostage, 0, 0, 0);
			base.AddElementFieldInfo("UspsProfileEntity", "RequireFullAddressValidation", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UspsProfileFieldIndex.RequireFullAddressValidation, 0, 0, 0);
			base.AddElementFieldInfo("UspsProfileEntity", "RateShop", typeof(Nullable<System.Boolean>), false, false, false, true,  (int)UspsProfileFieldIndex.RateShop, 0, 0, 0);
		}
		/// <summary>Inits UspsScanFormEntity's FieldInfo objects</summary>
		private void InitUspsScanFormEntityInfos()
		{
			base.AddElementFieldInfo("UspsScanFormEntity", "UspsScanFormID", typeof(System.Int64), true, false, true, false,  (int)UspsScanFormFieldIndex.UspsScanFormID, 0, 0, 19);
			base.AddElementFieldInfo("UspsScanFormEntity", "UspsAccountID", typeof(System.Int64), false, false, false, false,  (int)UspsScanFormFieldIndex.UspsAccountID, 0, 0, 19);
			base.AddElementFieldInfo("UspsScanFormEntity", "ScanFormTransactionID", typeof(System.String), false, false, false, false,  (int)UspsScanFormFieldIndex.ScanFormTransactionID, 100, 0, 0);
			base.AddElementFieldInfo("UspsScanFormEntity", "ScanFormUrl", typeof(System.String), false, false, false, false,  (int)UspsScanFormFieldIndex.ScanFormUrl, 2048, 0, 0);
			base.AddElementFieldInfo("UspsScanFormEntity", "CreatedDate", typeof(System.DateTime), false, false, false, false,  (int)UspsScanFormFieldIndex.CreatedDate, 0, 0, 0);
			base.AddElementFieldInfo("UspsScanFormEntity", "ScanFormBatchID", typeof(System.Int64), false, true, false, false,  (int)UspsScanFormFieldIndex.ScanFormBatchID, 0, 0, 19);
			base.AddElementFieldInfo("UspsScanFormEntity", "Description", typeof(System.String), false, false, false, false,  (int)UspsScanFormFieldIndex.Description, 100, 0, 0);
		}
		/// <summary>Inits UspsShipmentEntity's FieldInfo objects</summary>
		private void InitUspsShipmentEntityInfos()
		{
			base.AddElementFieldInfo("UspsShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)UspsShipmentFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("UspsShipmentEntity", "UspsAccountID", typeof(System.Int64), false, false, false, false,  (int)UspsShipmentFieldIndex.UspsAccountID, 0, 0, 19);
			base.AddElementFieldInfo("UspsShipmentEntity", "HidePostage", typeof(System.Boolean), false, false, false, false,  (int)UspsShipmentFieldIndex.HidePostage, 0, 0, 0);
			base.AddElementFieldInfo("UspsShipmentEntity", "RequireFullAddressValidation", typeof(System.Boolean), false, false, false, false,  (int)UspsShipmentFieldIndex.RequireFullAddressValidation, 0, 0, 0);
			base.AddElementFieldInfo("UspsShipmentEntity", "IntegratorTransactionID", typeof(System.Guid), false, false, false, false,  (int)UspsShipmentFieldIndex.IntegratorTransactionID, 0, 0, 0);
			base.AddElementFieldInfo("UspsShipmentEntity", "UspsTransactionID", typeof(System.Guid), false, false, false, false,  (int)UspsShipmentFieldIndex.UspsTransactionID, 0, 0, 0);
			base.AddElementFieldInfo("UspsShipmentEntity", "OriginalUspsAccountID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)UspsShipmentFieldIndex.OriginalUspsAccountID, 0, 0, 19);
			base.AddElementFieldInfo("UspsShipmentEntity", "ScanFormBatchID", typeof(Nullable<System.Int64>), false, true, false, true,  (int)UspsShipmentFieldIndex.ScanFormBatchID, 0, 0, 19);
			base.AddElementFieldInfo("UspsShipmentEntity", "RequestedLabelFormat", typeof(System.Int32), false, false, false, false,  (int)UspsShipmentFieldIndex.RequestedLabelFormat, 0, 0, 10);
			base.AddElementFieldInfo("UspsShipmentEntity", "RateShop", typeof(System.Boolean), false, false, false, false,  (int)UspsShipmentFieldIndex.RateShop, 0, 0, 0);
		}
		/// <summary>Inits ValidatedAddressEntity's FieldInfo objects</summary>
		private void InitValidatedAddressEntityInfos()
		{
			base.AddElementFieldInfo("ValidatedAddressEntity", "ValidatedAddressID", typeof(System.Int64), true, false, true, false,  (int)ValidatedAddressFieldIndex.ValidatedAddressID, 0, 0, 19);
			base.AddElementFieldInfo("ValidatedAddressEntity", "ConsumerID", typeof(System.Int64), false, true, false, false,  (int)ValidatedAddressFieldIndex.ConsumerID, 0, 0, 19);
			base.AddElementFieldInfo("ValidatedAddressEntity", "AddressPrefix", typeof(System.String), false, false, false, false,  (int)ValidatedAddressFieldIndex.AddressPrefix, 10, 0, 0);
			base.AddElementFieldInfo("ValidatedAddressEntity", "IsOriginal", typeof(System.Boolean), false, false, false, false,  (int)ValidatedAddressFieldIndex.IsOriginal, 0, 0, 0);
			base.AddElementFieldInfo("ValidatedAddressEntity", "Street1", typeof(System.String), false, false, false, false,  (int)ValidatedAddressFieldIndex.Street1, 60, 0, 0);
			base.AddElementFieldInfo("ValidatedAddressEntity", "Street2", typeof(System.String), false, false, false, false,  (int)ValidatedAddressFieldIndex.Street2, 60, 0, 0);
			base.AddElementFieldInfo("ValidatedAddressEntity", "Street3", typeof(System.String), false, false, false, false,  (int)ValidatedAddressFieldIndex.Street3, 60, 0, 0);
			base.AddElementFieldInfo("ValidatedAddressEntity", "City", typeof(System.String), false, false, false, false,  (int)ValidatedAddressFieldIndex.City, 50, 0, 0);
			base.AddElementFieldInfo("ValidatedAddressEntity", "StateProvCode", typeof(System.String), false, false, false, false,  (int)ValidatedAddressFieldIndex.StateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("ValidatedAddressEntity", "PostalCode", typeof(System.String), false, false, false, false,  (int)ValidatedAddressFieldIndex.PostalCode, 20, 0, 0);
			base.AddElementFieldInfo("ValidatedAddressEntity", "CountryCode", typeof(System.String), false, false, false, false,  (int)ValidatedAddressFieldIndex.CountryCode, 50, 0, 0);
			base.AddElementFieldInfo("ValidatedAddressEntity", "ResidentialStatus", typeof(System.Int32), false, false, false, false,  (int)ValidatedAddressFieldIndex.ResidentialStatus, 0, 0, 10);
			base.AddElementFieldInfo("ValidatedAddressEntity", "POBox", typeof(System.Int32), false, false, false, false,  (int)ValidatedAddressFieldIndex.POBox, 0, 0, 10);
			base.AddElementFieldInfo("ValidatedAddressEntity", "USTerritory", typeof(System.Int32), false, false, false, false,  (int)ValidatedAddressFieldIndex.USTerritory, 0, 0, 10);
			base.AddElementFieldInfo("ValidatedAddressEntity", "MilitaryAddress", typeof(System.Int32), false, false, false, false,  (int)ValidatedAddressFieldIndex.MilitaryAddress, 0, 0, 10);
		}
		/// <summary>Inits VersionSignoffEntity's FieldInfo objects</summary>
		private void InitVersionSignoffEntityInfos()
		{
			base.AddElementFieldInfo("VersionSignoffEntity", "VersionSignoffID", typeof(System.Int64), true, false, true, false,  (int)VersionSignoffFieldIndex.VersionSignoffID, 0, 0, 19);
			base.AddElementFieldInfo("VersionSignoffEntity", "Version", typeof(System.String), false, false, false, false,  (int)VersionSignoffFieldIndex.Version, 30, 0, 0);
			base.AddElementFieldInfo("VersionSignoffEntity", "UserID", typeof(System.Int64), false, false, false, false,  (int)VersionSignoffFieldIndex.UserID, 0, 0, 19);
			base.AddElementFieldInfo("VersionSignoffEntity", "ComputerID", typeof(System.Int64), false, true, false, false,  (int)VersionSignoffFieldIndex.ComputerID, 0, 0, 19);
		}
		/// <summary>Inits VolusionStoreEntity's FieldInfo objects</summary>
		private void InitVolusionStoreEntityInfos()
		{
			base.AddElementFieldInfo("VolusionStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)VolusionStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("VolusionStoreEntity", "StoreUrl", typeof(System.String), false, false, false, false,  (int)VolusionStoreFieldIndex.StoreUrl, 255, 0, 0);
			base.AddElementFieldInfo("VolusionStoreEntity", "WebUserName", typeof(System.String), false, false, false, false,  (int)VolusionStoreFieldIndex.WebUserName, 50, 0, 0);
			base.AddElementFieldInfo("VolusionStoreEntity", "WebPassword", typeof(System.String), false, false, false, false,  (int)VolusionStoreFieldIndex.WebPassword, 70, 0, 0);
			base.AddElementFieldInfo("VolusionStoreEntity", "ApiPassword", typeof(System.String), false, false, false, false,  (int)VolusionStoreFieldIndex.ApiPassword, 100, 0, 0);
			base.AddElementFieldInfo("VolusionStoreEntity", "PaymentMethods", typeof(System.String), false, false, false, false,  (int)VolusionStoreFieldIndex.PaymentMethods, 2147483647, 0, 0);
			base.AddElementFieldInfo("VolusionStoreEntity", "ShipmentMethods", typeof(System.String), false, false, false, false,  (int)VolusionStoreFieldIndex.ShipmentMethods, 2147483647, 0, 0);
			base.AddElementFieldInfo("VolusionStoreEntity", "DownloadOrderStatuses", typeof(System.String), false, false, false, false,  (int)VolusionStoreFieldIndex.DownloadOrderStatuses, 255, 0, 0);
			base.AddElementFieldInfo("VolusionStoreEntity", "ServerTimeZone", typeof(System.String), false, false, false, false,  (int)VolusionStoreFieldIndex.ServerTimeZone, 30, 0, 0);
			base.AddElementFieldInfo("VolusionStoreEntity", "ServerTimeZoneDST", typeof(System.Boolean), false, false, false, false,  (int)VolusionStoreFieldIndex.ServerTimeZoneDST, 0, 0, 0);
		}
		/// <summary>Inits WorldShipGoodsEntity's FieldInfo objects</summary>
		private void InitWorldShipGoodsEntityInfos()
		{
			base.AddElementFieldInfo("WorldShipGoodsEntity", "WorldShipGoodsID", typeof(System.Int64), true, false, true, false,  (int)WorldShipGoodsFieldIndex.WorldShipGoodsID, 0, 0, 19);
			base.AddElementFieldInfo("WorldShipGoodsEntity", "ShipmentID", typeof(System.Int64), false, true, false, false,  (int)WorldShipGoodsFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("WorldShipGoodsEntity", "ShipmentCustomsItemID", typeof(System.Int64), false, false, false, false,  (int)WorldShipGoodsFieldIndex.ShipmentCustomsItemID, 0, 0, 19);
			base.AddElementFieldInfo("WorldShipGoodsEntity", "Description", typeof(System.String), false, false, false, false,  (int)WorldShipGoodsFieldIndex.Description, 150, 0, 0);
			base.AddElementFieldInfo("WorldShipGoodsEntity", "TariffCode", typeof(System.String), false, false, false, false,  (int)WorldShipGoodsFieldIndex.TariffCode, 15, 0, 0);
			base.AddElementFieldInfo("WorldShipGoodsEntity", "CountryOfOrigin", typeof(System.String), false, false, false, false,  (int)WorldShipGoodsFieldIndex.CountryOfOrigin, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipGoodsEntity", "Units", typeof(System.Int32), false, false, false, false,  (int)WorldShipGoodsFieldIndex.Units, 0, 0, 10);
			base.AddElementFieldInfo("WorldShipGoodsEntity", "UnitOfMeasure", typeof(System.String), false, false, false, false,  (int)WorldShipGoodsFieldIndex.UnitOfMeasure, 5, 0, 0);
			base.AddElementFieldInfo("WorldShipGoodsEntity", "UnitPrice", typeof(System.Decimal), false, false, false, false,  (int)WorldShipGoodsFieldIndex.UnitPrice, 0, 4, 19);
			base.AddElementFieldInfo("WorldShipGoodsEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)WorldShipGoodsFieldIndex.Weight, 0, 0, 38);
			base.AddElementFieldInfo("WorldShipGoodsEntity", "InvoiceCurrencyCode", typeof(System.String), false, false, false, true,  (int)WorldShipGoodsFieldIndex.InvoiceCurrencyCode, 3, 0, 0);
		}
		/// <summary>Inits WorldShipPackageEntity's FieldInfo objects</summary>
		private void InitWorldShipPackageEntityInfos()
		{
			base.AddElementFieldInfo("WorldShipPackageEntity", "UpsPackageID", typeof(System.Int64), true, false, false, false,  (int)WorldShipPackageFieldIndex.UpsPackageID, 0, 0, 19);
			base.AddElementFieldInfo("WorldShipPackageEntity", "ShipmentID", typeof(System.Int64), false, true, false, false,  (int)WorldShipPackageFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("WorldShipPackageEntity", "PackageType", typeof(System.String), false, false, false, false,  (int)WorldShipPackageFieldIndex.PackageType, 35, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)WorldShipPackageFieldIndex.Weight, 0, 0, 38);
			base.AddElementFieldInfo("WorldShipPackageEntity", "ReferenceNumber", typeof(System.String), false, false, false, false,  (int)WorldShipPackageFieldIndex.ReferenceNumber, 35, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "ReferenceNumber2", typeof(System.String), false, false, false, false,  (int)WorldShipPackageFieldIndex.ReferenceNumber2, 35, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "CodOption", typeof(System.String), false, false, false, false,  (int)WorldShipPackageFieldIndex.CodOption, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "CodAmount", typeof(System.Decimal), false, false, false, false,  (int)WorldShipPackageFieldIndex.CodAmount, 0, 4, 19);
			base.AddElementFieldInfo("WorldShipPackageEntity", "CodCashOnly", typeof(System.String), false, false, false, false,  (int)WorldShipPackageFieldIndex.CodCashOnly, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "DeliveryConfirmation", typeof(System.String), false, false, false, false,  (int)WorldShipPackageFieldIndex.DeliveryConfirmation, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "DeliveryConfirmationSignature", typeof(System.String), false, false, false, false,  (int)WorldShipPackageFieldIndex.DeliveryConfirmationSignature, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "DeliveryConfirmationAdult", typeof(System.String), false, false, false, false,  (int)WorldShipPackageFieldIndex.DeliveryConfirmationAdult, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "Length", typeof(System.Int32), false, false, false, false,  (int)WorldShipPackageFieldIndex.Length, 0, 0, 10);
			base.AddElementFieldInfo("WorldShipPackageEntity", "Width", typeof(System.Int32), false, false, false, false,  (int)WorldShipPackageFieldIndex.Width, 0, 0, 10);
			base.AddElementFieldInfo("WorldShipPackageEntity", "Height", typeof(System.Int32), false, false, false, false,  (int)WorldShipPackageFieldIndex.Height, 0, 0, 10);
			base.AddElementFieldInfo("WorldShipPackageEntity", "DeclaredValueAmount", typeof(Nullable<System.Double>), false, false, false, true,  (int)WorldShipPackageFieldIndex.DeclaredValueAmount, 0, 0, 38);
			base.AddElementFieldInfo("WorldShipPackageEntity", "DeclaredValueOption", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.DeclaredValueOption, 2, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "CN22GoodsType", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.CN22GoodsType, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "CN22Description", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.CN22Description, 100, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "PostalSubClass", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.PostalSubClass, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "MIDeliveryConfirmation", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.MIDeliveryConfirmation, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "QvnOption", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.QvnOption, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "QvnFrom", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.QvnFrom, 35, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "QvnSubjectLine", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.QvnSubjectLine, 18, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "QvnMemo", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.QvnMemo, 150, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "Qvn1ShipNotify", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn1ShipNotify, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "Qvn1ContactName", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn1ContactName, 35, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "Qvn1Email", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn1Email, 100, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "Qvn2ShipNotify", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn2ShipNotify, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "Qvn2ContactName", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn2ContactName, 35, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "Qvn2Email", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn2Email, 100, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "Qvn3ShipNotify", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn3ShipNotify, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "Qvn3ContactName", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn3ContactName, 35, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "Qvn3Email", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.Qvn3Email, 100, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "ShipperRelease", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.ShipperRelease, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "AdditionalHandlingEnabled", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.AdditionalHandlingEnabled, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "VerbalConfirmationOption", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.VerbalConfirmationOption, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "VerbalConfirmationContactName", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.VerbalConfirmationContactName, 35, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "VerbalConfirmationTelephone", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.VerbalConfirmationTelephone, 15, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "DryIceRegulationSet", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.DryIceRegulationSet, 5, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "DryIceWeight", typeof(Nullable<System.Double>), false, false, false, true,  (int)WorldShipPackageFieldIndex.DryIceWeight, 0, 0, 38);
			base.AddElementFieldInfo("WorldShipPackageEntity", "DryIceMedicalPurpose", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.DryIceMedicalPurpose, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "DryIceOption", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.DryIceOption, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipPackageEntity", "DryIceWeightUnitOfMeasure", typeof(System.String), false, false, false, true,  (int)WorldShipPackageFieldIndex.DryIceWeightUnitOfMeasure, 10, 0, 0);
		}
		/// <summary>Inits WorldShipProcessedEntity's FieldInfo objects</summary>
		private void InitWorldShipProcessedEntityInfos()
		{
			base.AddElementFieldInfo("WorldShipProcessedEntity", "WorldShipProcessedID", typeof(System.Int64), true, false, true, false,  (int)WorldShipProcessedFieldIndex.WorldShipProcessedID, 0, 0, 19);
			base.AddElementFieldInfo("WorldShipProcessedEntity", "ShipmentID", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.ShipmentID, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipProcessedEntity", "RowVersion", typeof(System.Byte[]), false, false, true, false,  (int)WorldShipProcessedFieldIndex.RowVersion, 0, 0, 0);
			base.AddElementFieldInfo("WorldShipProcessedEntity", "PublishedCharges", typeof(System.Double), false, false, false, false,  (int)WorldShipProcessedFieldIndex.PublishedCharges, 0, 0, 38);
			base.AddElementFieldInfo("WorldShipProcessedEntity", "NegotiatedCharges", typeof(System.Double), false, false, false, false,  (int)WorldShipProcessedFieldIndex.NegotiatedCharges, 0, 0, 38);
			base.AddElementFieldInfo("WorldShipProcessedEntity", "TrackingNumber", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.TrackingNumber, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipProcessedEntity", "UspsTrackingNumber", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.UspsTrackingNumber, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipProcessedEntity", "ServiceType", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.ServiceType, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipProcessedEntity", "PackageType", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.PackageType, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipProcessedEntity", "UpsPackageID", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.UpsPackageID, 20, 0, 0);
			base.AddElementFieldInfo("WorldShipProcessedEntity", "DeclaredValueAmount", typeof(Nullable<System.Double>), false, false, false, true,  (int)WorldShipProcessedFieldIndex.DeclaredValueAmount, 0, 0, 38);
			base.AddElementFieldInfo("WorldShipProcessedEntity", "DeclaredValueOption", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.DeclaredValueOption, 2, 0, 0);
			base.AddElementFieldInfo("WorldShipProcessedEntity", "WorldShipShipmentID", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.WorldShipShipmentID, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipProcessedEntity", "VoidIndicator", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.VoidIndicator, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipProcessedEntity", "NumberOfPackages", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.NumberOfPackages, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipProcessedEntity", "LeadTrackingNumber", typeof(System.String), false, false, false, true,  (int)WorldShipProcessedFieldIndex.LeadTrackingNumber, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipProcessedEntity", "ShipmentIdCalculated", typeof(Nullable<System.Int64>), false, true, true, true,  (int)WorldShipProcessedFieldIndex.ShipmentIdCalculated, 0, 0, 19);
		}
		/// <summary>Inits WorldShipShipmentEntity's FieldInfo objects</summary>
		private void InitWorldShipShipmentEntityInfos()
		{
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ShipmentID", typeof(System.Int64), true, true, false, false,  (int)WorldShipShipmentFieldIndex.ShipmentID, 0, 0, 19);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "OrderNumber", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.OrderNumber, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "FromCompanyOrName", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromCompanyOrName, 35, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "FromAttention", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromAttention, 35, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "FromAddress1", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromAddress1, 60, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "FromAddress2", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromAddress2, 60, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "FromAddress3", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromAddress3, 60, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "FromCountryCode", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromCountryCode, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "FromPostalCode", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromPostalCode, 20, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "FromCity", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromCity, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "FromStateProvCode", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromStateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "FromTelephone", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromTelephone, 25, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "FromEmail", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromEmail, 100, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "FromAccountNumber", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.FromAccountNumber, 10, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ToCustomerID", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToCustomerID, 30, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ToCompanyOrName", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToCompanyOrName, 35, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ToAttention", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToAttention, 35, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ToAddress1", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToAddress1, 60, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ToAddress2", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToAddress2, 60, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ToAddress3", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToAddress3, 60, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ToCountryCode", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToCountryCode, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ToPostalCode", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToPostalCode, 20, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ToCity", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToCity, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ToStateProvCode", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToStateProvCode, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ToTelephone", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToTelephone, 25, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ToEmail", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToEmail, 100, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ToAccountNumber", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToAccountNumber, 10, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ToResidential", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ToResidential, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ServiceType", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ServiceType, 3, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "BillTransportationTo", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.BillTransportationTo, 20, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "SaturdayDelivery", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.SaturdayDelivery, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "QvnOption", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.QvnOption, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "QvnFrom", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.QvnFrom, 35, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "QvnSubjectLine", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.QvnSubjectLine, 18, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "QvnMemo", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.QvnMemo, 150, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn1ShipNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn1ShipNotify, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn1DeliveryNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn1DeliveryNotify, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn1ExceptionNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn1ExceptionNotify, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn1ContactName", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn1ContactName, 35, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn1Email", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn1Email, 100, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn2ShipNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn2ShipNotify, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn2DeliveryNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn2DeliveryNotify, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn2ExceptionNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn2ExceptionNotify, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn2ContactName", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn2ContactName, 35, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn2Email", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn2Email, 100, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn3ShipNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn3ShipNotify, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn3DeliveryNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn3DeliveryNotify, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn3ExceptionNotify", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn3ExceptionNotify, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn3ContactName", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn3ContactName, 35, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "Qvn3Email", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.Qvn3Email, 100, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "CustomsDescriptionOfGoods", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.CustomsDescriptionOfGoods, 150, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "CustomsDocumentsOnly", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.CustomsDocumentsOnly, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ShipperNumber", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.ShipperNumber, 10, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "PackageCount", typeof(System.Int32), false, false, false, false,  (int)WorldShipShipmentFieldIndex.PackageCount, 0, 0, 10);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "DeliveryConfirmation", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.DeliveryConfirmation, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "DeliveryConfirmationAdult", typeof(System.String), false, false, false, false,  (int)WorldShipShipmentFieldIndex.DeliveryConfirmationAdult, 1, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "InvoiceTermsOfSale", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.InvoiceTermsOfSale, 3, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "InvoiceReasonForExport", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.InvoiceReasonForExport, 2, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "InvoiceComments", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.InvoiceComments, 200, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "InvoiceCurrencyCode", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.InvoiceCurrencyCode, 3, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "InvoiceChargesFreight", typeof(Nullable<System.Decimal>), false, false, false, true,  (int)WorldShipShipmentFieldIndex.InvoiceChargesFreight, 0, 4, 19);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "InvoiceChargesInsurance", typeof(Nullable<System.Decimal>), false, false, false, true,  (int)WorldShipShipmentFieldIndex.InvoiceChargesInsurance, 0, 4, 19);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "InvoiceChargesOther", typeof(Nullable<System.Decimal>), false, false, false, true,  (int)WorldShipShipmentFieldIndex.InvoiceChargesOther, 0, 4, 19);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "ShipmentProcessedOnComputerID", typeof(Nullable<System.Int64>), false, false, false, true,  (int)WorldShipShipmentFieldIndex.ShipmentProcessedOnComputerID, 0, 0, 19);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "UspsEndorsement", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.UspsEndorsement, 50, 0, 0);
			base.AddElementFieldInfo("WorldShipShipmentEntity", "CarbonNeutral", typeof(System.String), false, false, false, true,  (int)WorldShipShipmentFieldIndex.CarbonNeutral, 10, 0, 0);
		}
		/// <summary>Inits YahooOrderEntity's FieldInfo objects</summary>
		private void InitYahooOrderEntityInfos()
		{
			base.AddElementFieldInfo("YahooOrderEntity", "OrderID", typeof(System.Int64), true, false, false, false,  (int)YahooOrderFieldIndex.OrderID, 0, 0, 19);
			base.AddElementFieldInfo("YahooOrderEntity", "YahooOrderID", typeof(System.String), false, false, false, false,  (int)YahooOrderFieldIndex.YahooOrderID, 50, 0, 0);
		}
		/// <summary>Inits YahooOrderItemEntity's FieldInfo objects</summary>
		private void InitYahooOrderItemEntityInfos()
		{
			base.AddElementFieldInfo("YahooOrderItemEntity", "OrderItemID", typeof(System.Int64), true, false, false, false,  (int)YahooOrderItemFieldIndex.OrderItemID, 0, 0, 19);
			base.AddElementFieldInfo("YahooOrderItemEntity", "YahooProductID", typeof(System.String), false, false, false, false,  (int)YahooOrderItemFieldIndex.YahooProductID, 255, 0, 0);
		}
		/// <summary>Inits YahooProductEntity's FieldInfo objects</summary>
		private void InitYahooProductEntityInfos()
		{
			base.AddElementFieldInfo("YahooProductEntity", "StoreID", typeof(System.Int64), true, true, false, false,  (int)YahooProductFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("YahooProductEntity", "YahooProductID", typeof(System.String), true, false, false, false,  (int)YahooProductFieldIndex.YahooProductID, 255, 0, 0);
			base.AddElementFieldInfo("YahooProductEntity", "Weight", typeof(System.Double), false, false, false, false,  (int)YahooProductFieldIndex.Weight, 0, 0, 38);
		}
		/// <summary>Inits YahooStoreEntity's FieldInfo objects</summary>
		private void InitYahooStoreEntityInfos()
		{
			base.AddElementFieldInfo("YahooStoreEntity", "StoreID", typeof(System.Int64), true, false, false, false,  (int)YahooStoreFieldIndex.StoreID, 0, 0, 19);
			base.AddElementFieldInfo("YahooStoreEntity", "YahooEmailAccountID", typeof(System.Int64), false, true, false, false,  (int)YahooStoreFieldIndex.YahooEmailAccountID, 0, 0, 19);
			base.AddElementFieldInfo("YahooStoreEntity", "TrackingUpdatePassword", typeof(System.String), false, false, false, false,  (int)YahooStoreFieldIndex.TrackingUpdatePassword, 100, 0, 0);
		}
		
	}
}




