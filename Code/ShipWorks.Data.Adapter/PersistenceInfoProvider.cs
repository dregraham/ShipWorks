///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SqlServerSpecific.NET20
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Data;

using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Adapter
{
	/// <summary>
	/// Singleton implementation of the PersistenceInfoProvider. This class is the singleton wrapper through which the actual instance is retrieved.
	/// </summary>
	/// <remarks>It uses a single instance of an internal class. The access isn't marked with locks as the PersistenceInfoProviderBase class is threadsafe.</remarks>
	internal sealed class PersistenceInfoProviderSingleton
	{
		#region Class Member Declarations
		private static readonly IPersistenceInfoProvider _providerInstance = new PersistenceInfoProviderCore();
		#endregion
		
		/// <summary>private ctor to prevent instances of this class.</summary>
		private PersistenceInfoProviderSingleton()
		{
		}

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
			base.InitClass((167 + 0));
			InitActionEntityMappings();
			InitActionFilterTriggerEntityMappings();
			InitActionQueueEntityMappings();
			InitActionQueueSelectionEntityMappings();
			InitActionQueueStepEntityMappings();
			InitActionTaskEntityMappings();
			InitAmazonASINEntityMappings();
			InitAmazonOrderEntityMappings();
			InitAmazonOrderItemEntityMappings();
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
			InitStatusPresetEntityMappings();
			InitStoreEntityMappings();
			InitSystemDataEntityMappings();
			InitTemplateEntityMappings();
			InitTemplateComputerSettingsEntityMappings();
			InitTemplateFolderEntityMappings();
			InitTemplateStoreSettingsEntityMappings();
			InitTemplateUserSettingsEntityMappings();
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
			base.AddElementMapping( "ActionEntity", "ShipWorksLocal", @"dbo", "Action", 12 );
			base.AddElementFieldMapping( "ActionEntity", "ActionID", "ActionID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ActionEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ActionEntity", "Name", "Name", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ActionEntity", "Enabled", "Enabled", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "ActionEntity", "ComputerLimitedType", "ComputerLimitedType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "ActionEntity", "InternalComputerLimitedList", "ComputerLimitedList", false, (int)SqlDbType.VarChar, 150, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "ActionEntity", "StoreLimited", "StoreLimited", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 6 );
			base.AddElementFieldMapping( "ActionEntity", "InternalStoreLimitedList", "StoreLimitedList", false, (int)SqlDbType.NVarChar, 150, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "ActionEntity", "TriggerType", "TriggerType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 8 );
			base.AddElementFieldMapping( "ActionEntity", "TriggerSettings", "TriggerSettings", false, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "ActionEntity", "TaskSummary", "TaskSummary", false, (int)SqlDbType.NVarChar, 150, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "ActionEntity", "InternalOwner", "InternalOwner", true, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 11 );
		}
		/// <summary>Inits ActionFilterTriggerEntity's mappings</summary>
		private void InitActionFilterTriggerEntityMappings()
		{
			base.AddElementMapping( "ActionFilterTriggerEntity", "ShipWorksLocal", @"dbo", "ActionFilterTrigger", 5 );
			base.AddElementFieldMapping( "ActionFilterTriggerEntity", "ActionID", "ActionID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ActionFilterTriggerEntity", "FilterNodeID", "FilterNodeID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "ActionFilterTriggerEntity", "Direction", "Direction", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "ActionFilterTriggerEntity", "ComputerLimitedType", "ComputerLimitedType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "ActionFilterTriggerEntity", "InternalComputerLimitedList", "ComputerLimitedList", false, (int)SqlDbType.VarChar, 150, 0, 0, false, "", null, typeof(System.String), 4 );
		}
		/// <summary>Inits ActionQueueEntity's mappings</summary>
		private void InitActionQueueEntityMappings()
		{
			base.AddElementMapping( "ActionQueueEntity", "ShipWorksLocal", @"dbo", "ActionQueue", 14 );
			base.AddElementFieldMapping( "ActionQueueEntity", "ActionQueueID", "ActionQueueID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ActionQueueEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ActionQueueEntity", "ActionID", "ActionID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "ActionQueueEntity", "ActionName", "ActionName", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "ActionQueueEntity", "ActionQueueType", "ActionQueueType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "ActionQueueEntity", "ActionVersion", "ActionVersion", false, (int)SqlDbType.Binary, 8, 0, 0, false, "", null, typeof(System.Byte[]), 5 );
			base.AddElementFieldMapping( "ActionQueueEntity", "QueueVersion", "QueueVersion", false, (int)SqlDbType.Binary, 8, 0, 0, false, "", null, typeof(System.Byte[]), 6 );
			base.AddElementFieldMapping( "ActionQueueEntity", "TriggerDate", "TriggerDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 7 );
			base.AddElementFieldMapping( "ActionQueueEntity", "TriggerComputerID", "TriggerComputerID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 8 );
			base.AddElementFieldMapping( "ActionQueueEntity", "InternalComputerLimitedList", "ComputerLimitedList", false, (int)SqlDbType.VarChar, 150, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "ActionQueueEntity", "ObjectID", "ObjectID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 10 );
			base.AddElementFieldMapping( "ActionQueueEntity", "Status", "Status", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 11 );
			base.AddElementFieldMapping( "ActionQueueEntity", "NextStep", "NextStep", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 12 );
			base.AddElementFieldMapping( "ActionQueueEntity", "ContextLock", "ContextLock", true, (int)SqlDbType.NVarChar, 36, 0, 0, false, "", null, typeof(System.String), 13 );
		}
		/// <summary>Inits ActionQueueSelectionEntity's mappings</summary>
		private void InitActionQueueSelectionEntityMappings()
		{
			base.AddElementMapping( "ActionQueueSelectionEntity", "ShipWorksLocal", @"dbo", "ActionQueueSelection", 3 );
			base.AddElementFieldMapping( "ActionQueueSelectionEntity", "ActionQueueSelectionID", "ActionQueueSelectionID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ActionQueueSelectionEntity", "ActionQueueID", "ActionQueueID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "ActionQueueSelectionEntity", "ObjectID", "ObjectID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
		}
		/// <summary>Inits ActionQueueStepEntity's mappings</summary>
		private void InitActionQueueStepEntityMappings()
		{
			base.AddElementMapping( "ActionQueueStepEntity", "ShipWorksLocal", @"dbo", "ActionQueueStep", 18 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "ActionQueueStepID", "ActionQueueStepID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "ActionQueueID", "ActionQueueID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "StepStatus", "StepStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "StepIndex", "StepIndex", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "StepName", "StepName", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "TaskIdentifier", "TaskIdentifier", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "TaskSettings", "TaskSettings", false, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "InputSource", "InputSource", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 8 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "InputFilterNodeID", "InputFilterNodeID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 9 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "FilterCondition", "FilterCondition", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 10 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "FilterConditionNodeID", "FilterConditionNodeID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 11 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "FlowSuccess", "FlowSuccess", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 12 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "FlowSkipped", "FlowSkipped", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 13 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "FlowError", "FlowError", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 14 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "AttemptDate", "AttemptDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 15 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "AttemptError", "AttemptError", false, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "ActionQueueStepEntity", "AttemptCount", "AttemptCount", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 17 );
		}
		/// <summary>Inits ActionTaskEntity's mappings</summary>
		private void InitActionTaskEntityMappings()
		{
			base.AddElementMapping( "ActionTaskEntity", "ShipWorksLocal", @"dbo", "ActionTask", 12 );
			base.AddElementFieldMapping( "ActionTaskEntity", "ActionTaskID", "ActionTaskID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ActionTaskEntity", "ActionID", "ActionID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "ActionTaskEntity", "TaskIdentifier", "TaskIdentifier", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ActionTaskEntity", "TaskSettings", "TaskSettings", false, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "ActionTaskEntity", "StepIndex", "StepIndex", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "ActionTaskEntity", "InputSource", "InputSource", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
			base.AddElementFieldMapping( "ActionTaskEntity", "InputFilterNodeID", "InputFilterNodeID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 6 );
			base.AddElementFieldMapping( "ActionTaskEntity", "FilterCondition", "FilterCondition", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 7 );
			base.AddElementFieldMapping( "ActionTaskEntity", "FilterConditionNodeID", "FilterConditionNodeID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 8 );
			base.AddElementFieldMapping( "ActionTaskEntity", "FlowSuccess", "FlowSuccess", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 9 );
			base.AddElementFieldMapping( "ActionTaskEntity", "FlowSkipped", "FlowSkipped", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 10 );
			base.AddElementFieldMapping( "ActionTaskEntity", "FlowError", "FlowError", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 11 );
		}
		/// <summary>Inits AmazonASINEntity's mappings</summary>
		private void InitAmazonASINEntityMappings()
		{
			base.AddElementMapping( "AmazonASINEntity", "ShipWorksLocal", @"dbo", "AmazonASIN", 3 );
			base.AddElementFieldMapping( "AmazonASINEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "AmazonASINEntity", "SKU", "SKU", false, (int)SqlDbType.VarChar, 100, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "AmazonASINEntity", "AmazonASIN", "AmazonASIN", false, (int)SqlDbType.VarChar, 32, 0, 0, false, "", null, typeof(System.String), 2 );
		}
		/// <summary>Inits AmazonOrderEntity's mappings</summary>
		private void InitAmazonOrderEntityMappings()
		{
			base.AddElementMapping( "AmazonOrderEntity", "ShipWorksLocal", @"dbo", "AmazonOrder", 7 );
			base.AddElementFieldMapping( "AmazonOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "AmazonOrderEntity", "AmazonOrderID", "AmazonOrderID", false, (int)SqlDbType.VarChar, 32, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "AmazonOrderEntity", "AmazonCommission", "AmazonCommission", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 2 );
			base.AddElementFieldMapping( "AmazonOrderEntity", "FulfillmentChannel", "FulfillmentChannel", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "AmazonOrderEntity", "IsPrime", "IsPrime", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "AmazonOrderEntity", "EarliestExpectedDeliveryDate", "EarliestExpectedDeliveryDate", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 5 );
			base.AddElementFieldMapping( "AmazonOrderEntity", "LatestExpectedDeliveryDate", "LatestExpectedDeliveryDate", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 6 );
		}
		/// <summary>Inits AmazonOrderItemEntity's mappings</summary>
		private void InitAmazonOrderItemEntityMappings()
		{
			base.AddElementMapping( "AmazonOrderItemEntity", "ShipWorksLocal", @"dbo", "AmazonOrderItem", 4 );
			base.AddElementFieldMapping( "AmazonOrderItemEntity", "OrderItemID", "OrderItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "AmazonOrderItemEntity", "AmazonOrderItemCode", "AmazonOrderItemCode", false, (int)SqlDbType.NVarChar, 64, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "AmazonOrderItemEntity", "ASIN", "ASIN", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "AmazonOrderItemEntity", "ConditionNote", "ConditionNote", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 3 );
		}
		/// <summary>Inits AmazonStoreEntity's mappings</summary>
		private void InitAmazonStoreEntityMappings()
		{
			base.AddElementMapping( "AmazonStoreEntity", "ShipWorksLocal", @"dbo", "AmazonStore", 18 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "AmazonApi", "AmazonApi", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 1 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "AmazonApiRegion", "AmazonApiRegion", false, (int)SqlDbType.Char, 2, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "SellerCentralUsername", "SellerCentralUsername", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "SellerCentralPassword", "SellerCentralPassword", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "MerchantName", "MerchantName", false, (int)SqlDbType.VarChar, 64, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "MerchantToken", "MerchantToken", false, (int)SqlDbType.VarChar, 32, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "AccessKeyID", "AccessKeyID", false, (int)SqlDbType.VarChar, 32, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "AuthToken", "AuthToken", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "Cookie", "Cookie", false, (int)SqlDbType.Text, 2147483647, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "CookieExpires", "CookieExpires", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 10 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "CookieWaitUntil", "CookieWaitUntil", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 11 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "Certificate", "Certificate", true, (int)SqlDbType.VarBinary, 2048, 0, 0, false, "", null, typeof(System.Byte[]), 12 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "WeightDownloads", "WeightDownloads", false, (int)SqlDbType.Text, 2147483647, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "MerchantID", "MerchantID", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "MarketplaceID", "MarketplaceID", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "ExcludeFBA", "ExcludeFBA", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 16 );
			base.AddElementFieldMapping( "AmazonStoreEntity", "DomainName", "DomainName", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 17 );
		}
		/// <summary>Inits AmeriCommerceStoreEntity's mappings</summary>
		private void InitAmeriCommerceStoreEntityMappings()
		{
			base.AddElementMapping( "AmeriCommerceStoreEntity", "ShipWorksLocal", @"dbo", "AmeriCommerceStore", 6 );
			base.AddElementFieldMapping( "AmeriCommerceStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "AmeriCommerceStoreEntity", "Username", "Username", false, (int)SqlDbType.NVarChar, 70, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "AmeriCommerceStoreEntity", "Password", "Password", false, (int)SqlDbType.NVarChar, 70, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "AmeriCommerceStoreEntity", "StoreUrl", "StoreUrl", false, (int)SqlDbType.NVarChar, 350, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "AmeriCommerceStoreEntity", "StoreCode", "StoreCode", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "AmeriCommerceStoreEntity", "StatusCodes", "StatusCodes", false, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 5 );
		}
		/// <summary>Inits AuditEntity's mappings</summary>
		private void InitAuditEntityMappings()
		{
			base.AddElementMapping( "AuditEntity", "ShipWorksLocal", @"dbo", "Audit", 11 );
			base.AddElementFieldMapping( "AuditEntity", "AuditID", "AuditID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "AuditEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "AuditEntity", "TransactionID", "TransactionID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "AuditEntity", "UserID", "UserID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "AuditEntity", "ComputerID", "ComputerID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
			base.AddElementFieldMapping( "AuditEntity", "Reason", "Reason", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
			base.AddElementFieldMapping( "AuditEntity", "ReasonDetail", "ReasonDetail", true, (int)SqlDbType.VarChar, 100, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "AuditEntity", "Date", "Date", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 7 );
			base.AddElementFieldMapping( "AuditEntity", "Action", "Action", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 8 );
			base.AddElementFieldMapping( "AuditEntity", "ObjectID", "ObjectID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 9 );
			base.AddElementFieldMapping( "AuditEntity", "HasEvents", "HasEvents", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 10 );
		}
		/// <summary>Inits AuditChangeEntity's mappings</summary>
		private void InitAuditChangeEntityMappings()
		{
			base.AddElementMapping( "AuditChangeEntity", "ShipWorksLocal", @"dbo", "AuditChange", 4 );
			base.AddElementFieldMapping( "AuditChangeEntity", "AuditChangeID", "AuditChangeID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "AuditChangeEntity", "AuditID", "AuditID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "AuditChangeEntity", "ChangeType", "ChangeType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "AuditChangeEntity", "ObjectID", "ObjectID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
		}
		/// <summary>Inits AuditChangeDetailEntity's mappings</summary>
		private void InitAuditChangeDetailEntityMappings()
		{
			base.AddElementMapping( "AuditChangeDetailEntity", "ShipWorksLocal", @"dbo", "AuditChangeDetail", 10 );
			base.AddElementFieldMapping( "AuditChangeDetailEntity", "AuditChangeDetailID", "AuditChangeDetailID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "AuditChangeDetailEntity", "AuditChangeID", "AuditChangeID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "AuditChangeDetailEntity", "AuditID", "AuditID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "AuditChangeDetailEntity", "DisplayName", "DisplayName", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "AuditChangeDetailEntity", "DisplayFormat", "DisplayFormat", false, (int)SqlDbType.TinyInt, 0, 0, 3, false, "", null, typeof(System.Byte), 4 );
			base.AddElementFieldMapping( "AuditChangeDetailEntity", "DataType", "DataType", false, (int)SqlDbType.TinyInt, 0, 0, 3, false, "", null, typeof(System.Byte), 5 );
			base.AddElementFieldMapping( "AuditChangeDetailEntity", "TextOld", "TextOld", true, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "AuditChangeDetailEntity", "TextNew", "TextNew", true, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "AuditChangeDetailEntity", "VariantOld", "VariantOld", true, (int)SqlDbType.Variant, 0, 0, 0, false, "", null, typeof(System.Object), 8 );
			base.AddElementFieldMapping( "AuditChangeDetailEntity", "VariantNew", "VariantNew", true, (int)SqlDbType.Variant, 0, 0, 0, false, "", null, typeof(System.Object), 9 );
		}
		/// <summary>Inits BestRateProfileEntity's mappings</summary>
		private void InitBestRateProfileEntityMappings()
		{
			base.AddElementMapping( "BestRateProfileEntity", "ShipWorksLocal", @"dbo", "BestRateProfile", 9 );
			base.AddElementFieldMapping( "BestRateProfileEntity", "ShippingProfileID", "ShippingProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "BestRateProfileEntity", "DimsProfileID", "DimsProfileID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "BestRateProfileEntity", "DimsLength", "DimsLength", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 2 );
			base.AddElementFieldMapping( "BestRateProfileEntity", "DimsWidth", "DimsWidth", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 3 );
			base.AddElementFieldMapping( "BestRateProfileEntity", "DimsHeight", "DimsHeight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 4 );
			base.AddElementFieldMapping( "BestRateProfileEntity", "DimsWeight", "DimsWeight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 5 );
			base.AddElementFieldMapping( "BestRateProfileEntity", "DimsAddWeight", "DimsAddWeight", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 6 );
			base.AddElementFieldMapping( "BestRateProfileEntity", "Weight", "Weight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 7 );
			base.AddElementFieldMapping( "BestRateProfileEntity", "ServiceLevel", "ServiceLevel", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 8 );
		}
		/// <summary>Inits BestRateShipmentEntity's mappings</summary>
		private void InitBestRateShipmentEntityMappings()
		{
			base.AddElementMapping( "BestRateShipmentEntity", "ShipWorksLocal", @"dbo", "BestRateShipment", 10 );
			base.AddElementFieldMapping( "BestRateShipmentEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "BestRateShipmentEntity", "DimsProfileID", "DimsProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "BestRateShipmentEntity", "DimsLength", "DimsLength", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 2 );
			base.AddElementFieldMapping( "BestRateShipmentEntity", "DimsWidth", "DimsWidth", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 3 );
			base.AddElementFieldMapping( "BestRateShipmentEntity", "DimsHeight", "DimsHeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 4 );
			base.AddElementFieldMapping( "BestRateShipmentEntity", "DimsWeight", "DimsWeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 5 );
			base.AddElementFieldMapping( "BestRateShipmentEntity", "DimsAddWeight", "DimsAddWeight", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 6 );
			base.AddElementFieldMapping( "BestRateShipmentEntity", "ServiceLevel", "ServiceLevel", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 7 );
			base.AddElementFieldMapping( "BestRateShipmentEntity", "InsuranceValue", "InsuranceValue", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 8 );
			base.AddElementFieldMapping( "BestRateShipmentEntity", "RequestedLabelFormat", "RequestedLabelFormat", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 9 );
		}
		/// <summary>Inits BigCommerceOrderItemEntity's mappings</summary>
		private void InitBigCommerceOrderItemEntityMappings()
		{
			base.AddElementMapping( "BigCommerceOrderItemEntity", "ShipWorksLocal", @"dbo", "BigCommerceOrderItem", 6 );
			base.AddElementFieldMapping( "BigCommerceOrderItemEntity", "OrderItemID", "OrderItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "BigCommerceOrderItemEntity", "OrderAddressID", "OrderAddressID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "BigCommerceOrderItemEntity", "OrderProductID", "OrderProductID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "BigCommerceOrderItemEntity", "IsDigitalItem", "IsDigitalItem", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "BigCommerceOrderItemEntity", "EventDate", "EventDate", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 4 );
			base.AddElementFieldMapping( "BigCommerceOrderItemEntity", "EventName", "EventName", true, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 5 );
		}
		/// <summary>Inits BigCommerceStoreEntity's mappings</summary>
		private void InitBigCommerceStoreEntityMappings()
		{
			base.AddElementMapping( "BigCommerceStoreEntity", "ShipWorksLocal", @"dbo", "BigCommerceStore", 7 );
			base.AddElementFieldMapping( "BigCommerceStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "BigCommerceStoreEntity", "ApiUrl", "ApiUrl", false, (int)SqlDbType.NVarChar, 110, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "BigCommerceStoreEntity", "ApiUserName", "ApiUserName", false, (int)SqlDbType.NVarChar, 65, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "BigCommerceStoreEntity", "ApiToken", "ApiToken", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "BigCommerceStoreEntity", "StatusCodes", "StatusCodes", true, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "BigCommerceStoreEntity", "WeightUnitOfMeasure", "WeightUnitOfMeasure", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
			base.AddElementFieldMapping( "BigCommerceStoreEntity", "DownloadModifiedNumberOfDaysBack", "DownloadModifiedNumberOfDaysBack", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 6 );
		}
		/// <summary>Inits BuyDotComOrderItemEntity's mappings</summary>
		private void InitBuyDotComOrderItemEntityMappings()
		{
			base.AddElementMapping( "BuyDotComOrderItemEntity", "ShipWorksLocal", @"dbo", "BuyDotComOrderItem", 7 );
			base.AddElementFieldMapping( "BuyDotComOrderItemEntity", "OrderItemID", "OrderItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "BuyDotComOrderItemEntity", "ReceiptItemID", "ReceiptItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "BuyDotComOrderItemEntity", "ListingID", "ListingID", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "BuyDotComOrderItemEntity", "Shipping", "Shipping", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 3 );
			base.AddElementFieldMapping( "BuyDotComOrderItemEntity", "Tax", "Tax", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 4 );
			base.AddElementFieldMapping( "BuyDotComOrderItemEntity", "Commission", "Commission", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 5 );
			base.AddElementFieldMapping( "BuyDotComOrderItemEntity", "ItemFee", "ItemFee", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 6 );
		}
		/// <summary>Inits BuyDotComStoreEntity's mappings</summary>
		private void InitBuyDotComStoreEntityMappings()
		{
			base.AddElementMapping( "BuyDotComStoreEntity", "ShipWorksLocal", @"dbo", "BuyDotComStore", 3 );
			base.AddElementFieldMapping( "BuyDotComStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "BuyDotComStoreEntity", "FtpUsername", "FtpUsername", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "BuyDotComStoreEntity", "FtpPassword", "FtpPassword", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
		}
		/// <summary>Inits ChannelAdvisorOrderEntity's mappings</summary>
		private void InitChannelAdvisorOrderEntityMappings()
		{
			base.AddElementMapping( "ChannelAdvisorOrderEntity", "ShipWorksLocal", @"dbo", "ChannelAdvisorOrder", 10 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderEntity", "CustomOrderIdentifier", "CustomOrderIdentifier", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderEntity", "ResellerID", "ResellerID", false, (int)SqlDbType.NVarChar, 80, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderEntity", "OnlineShippingStatus", "OnlineShippingStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderEntity", "OnlineCheckoutStatus", "OnlineCheckoutStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderEntity", "OnlinePaymentStatus", "OnlinePaymentStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderEntity", "FlagStyle", "FlagStyle", false, (int)SqlDbType.NVarChar, 32, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderEntity", "FlagDescription", "FlagDescription", false, (int)SqlDbType.NVarChar, 80, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderEntity", "FlagType", "FlagType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 8 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderEntity", "MarketplaceNames", "MarketplaceNames", false, (int)SqlDbType.NVarChar, 1024, 0, 0, false, "", null, typeof(System.String), 9 );
		}
		/// <summary>Inits ChannelAdvisorOrderItemEntity's mappings</summary>
		private void InitChannelAdvisorOrderItemEntityMappings()
		{
			base.AddElementMapping( "ChannelAdvisorOrderItemEntity", "ShipWorksLocal", @"dbo", "ChannelAdvisorOrderItem", 10 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderItemEntity", "OrderItemID", "OrderItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderItemEntity", "MarketplaceName", "MarketplaceName", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderItemEntity", "MarketplaceStoreName", "MarketplaceStoreName", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderItemEntity", "MarketplaceBuyerID", "MarketplaceBuyerID", false, (int)SqlDbType.NVarChar, 80, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderItemEntity", "MarketplaceSalesID", "MarketplaceSalesID", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderItemEntity", "Classification", "Classification", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderItemEntity", "DistributionCenter", "DistributionCenter", false, (int)SqlDbType.NVarChar, 80, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderItemEntity", "HarmonizedCode", "HarmonizedCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderItemEntity", "IsFBA", "IsFBA", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 8 );
			base.AddElementFieldMapping( "ChannelAdvisorOrderItemEntity", "MPN", "MPN", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 9 );
		}
		/// <summary>Inits ChannelAdvisorStoreEntity's mappings</summary>
		private void InitChannelAdvisorStoreEntityMappings()
		{
			base.AddElementMapping( "ChannelAdvisorStoreEntity", "ShipWorksLocal", @"dbo", "ChannelAdvisorStore", 5 );
			base.AddElementFieldMapping( "ChannelAdvisorStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ChannelAdvisorStoreEntity", "AccountKey", "AccountKey", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "ChannelAdvisorStoreEntity", "ProfileID", "ProfileID", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "ChannelAdvisorStoreEntity", "AttributesToDownload", "AttributesToDownload", false, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "ChannelAdvisorStoreEntity", "ConsolidatorAsUsps", "ConsolidatorAsUsps", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 4 );
		}
		/// <summary>Inits ClickCartProOrderEntity's mappings</summary>
		private void InitClickCartProOrderEntityMappings()
		{
			base.AddElementMapping( "ClickCartProOrderEntity", "ShipWorksLocal", @"dbo", "ClickCartProOrder", 2 );
			base.AddElementFieldMapping( "ClickCartProOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ClickCartProOrderEntity", "ClickCartProOrderID", "ClickCartProOrderID", false, (int)SqlDbType.VarChar, 25, 0, 0, false, "", null, typeof(System.String), 1 );
		}
		/// <summary>Inits CommerceInterfaceOrderEntity's mappings</summary>
		private void InitCommerceInterfaceOrderEntityMappings()
		{
			base.AddElementMapping( "CommerceInterfaceOrderEntity", "ShipWorksLocal", @"dbo", "CommerceInterfaceOrder", 2 );
			base.AddElementFieldMapping( "CommerceInterfaceOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "CommerceInterfaceOrderEntity", "CommerceInterfaceOrderNumber", "CommerceInterfaceOrderNumber", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 1 );
		}
		/// <summary>Inits ComputerEntity's mappings</summary>
		private void InitComputerEntityMappings()
		{
			base.AddElementMapping( "ComputerEntity", "ShipWorksLocal", @"dbo", "Computer", 4 );
			base.AddElementFieldMapping( "ComputerEntity", "ComputerID", "ComputerID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ComputerEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ComputerEntity", "Identifier", "Identifier", false, (int)SqlDbType.UniqueIdentifier, 0, 0, 0, false, "", null, typeof(System.Guid), 2 );
			base.AddElementFieldMapping( "ComputerEntity", "Name", "Name", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
		}
		/// <summary>Inits ConfigurationEntity's mappings</summary>
		private void InitConfigurationEntityMappings()
		{
			base.AddElementMapping( "ConfigurationEntity", "ShipWorksLocal", @"dbo", "Configuration", 12 );
			base.AddElementFieldMapping( "ConfigurationEntity", "ConfigurationID", "ConfigurationID", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 0 );
			base.AddElementFieldMapping( "ConfigurationEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ConfigurationEntity", "LogOnMethod", "LogOnMethod", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "ConfigurationEntity", "AddressCasing", "AddressCasing", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "ConfigurationEntity", "CustomerCompareEmail", "CustomerCompareEmail", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 4 );
			base.AddElementFieldMapping( "ConfigurationEntity", "CustomerCompareAddress", "CustomerCompareAddress", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 5 );
			base.AddElementFieldMapping( "ConfigurationEntity", "CustomerUpdateBilling", "CustomerUpdateBilling", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 6 );
			base.AddElementFieldMapping( "ConfigurationEntity", "CustomerUpdateShipping", "CustomerUpdateShipping", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 7 );
			base.AddElementFieldMapping( "ConfigurationEntity", "CustomerUpdateModifiedBilling", "CustomerUpdateModifiedBilling", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 8 );
			base.AddElementFieldMapping( "ConfigurationEntity", "CustomerUpdateModifiedShipping", "CustomerUpdateModifiedShipping", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 9 );
			base.AddElementFieldMapping( "ConfigurationEntity", "AuditNewOrders", "AuditNewOrders", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 10 );
			base.AddElementFieldMapping( "ConfigurationEntity", "AuditDeletedOrders", "AuditDeletedOrders", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 11 );
		}
		/// <summary>Inits CustomerEntity's mappings</summary>
		private void InitCustomerEntityMappings()
		{
			base.AddElementMapping( "CustomerEntity", "ShipWorksLocal", @"dbo", "Customer", 35 );
			base.AddElementFieldMapping( "CustomerEntity", "CustomerID", "CustomerID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "CustomerEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "CustomerEntity", "BillFirstName", "BillFirstName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "CustomerEntity", "BillMiddleName", "BillMiddleName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "CustomerEntity", "BillLastName", "BillLastName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "CustomerEntity", "BillCompany", "BillCompany", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "CustomerEntity", "BillStreet1", "BillStreet1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "CustomerEntity", "BillStreet2", "BillStreet2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "CustomerEntity", "BillStreet3", "BillStreet3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "CustomerEntity", "BillCity", "BillCity", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "CustomerEntity", "BillStateProvCode", "BillStateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "CustomerEntity", "BillPostalCode", "BillPostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "CustomerEntity", "BillCountryCode", "BillCountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "CustomerEntity", "BillPhone", "BillPhone", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "CustomerEntity", "BillFax", "BillFax", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "CustomerEntity", "BillEmail", "BillEmail", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "CustomerEntity", "BillWebsite", "BillWebsite", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "CustomerEntity", "ShipFirstName", "ShipFirstName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 17 );
			base.AddElementFieldMapping( "CustomerEntity", "ShipMiddleName", "ShipMiddleName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "CustomerEntity", "ShipLastName", "ShipLastName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 19 );
			base.AddElementFieldMapping( "CustomerEntity", "ShipCompany", "ShipCompany", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 20 );
			base.AddElementFieldMapping( "CustomerEntity", "ShipStreet1", "ShipStreet1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 21 );
			base.AddElementFieldMapping( "CustomerEntity", "ShipStreet2", "ShipStreet2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 22 );
			base.AddElementFieldMapping( "CustomerEntity", "ShipStreet3", "ShipStreet3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 23 );
			base.AddElementFieldMapping( "CustomerEntity", "ShipCity", "ShipCity", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 24 );
			base.AddElementFieldMapping( "CustomerEntity", "ShipStateProvCode", "ShipStateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 25 );
			base.AddElementFieldMapping( "CustomerEntity", "ShipPostalCode", "ShipPostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 26 );
			base.AddElementFieldMapping( "CustomerEntity", "ShipCountryCode", "ShipCountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 27 );
			base.AddElementFieldMapping( "CustomerEntity", "ShipPhone", "ShipPhone", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 28 );
			base.AddElementFieldMapping( "CustomerEntity", "ShipFax", "ShipFax", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 29 );
			base.AddElementFieldMapping( "CustomerEntity", "ShipEmail", "ShipEmail", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 30 );
			base.AddElementFieldMapping( "CustomerEntity", "ShipWebsite", "ShipWebsite", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 31 );
			base.AddElementFieldMapping( "CustomerEntity", "RollupOrderCount", "RollupOrderCount", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 32 );
			base.AddElementFieldMapping( "CustomerEntity", "RollupOrderTotal", "RollupOrderTotal", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 33 );
			base.AddElementFieldMapping( "CustomerEntity", "RollupNoteCount", "RollupNoteCount", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 34 );
		}
		/// <summary>Inits DimensionsProfileEntity's mappings</summary>
		private void InitDimensionsProfileEntityMappings()
		{
			base.AddElementMapping( "DimensionsProfileEntity", "ShipWorksLocal", @"dbo", "DimensionsProfile", 6 );
			base.AddElementFieldMapping( "DimensionsProfileEntity", "DimensionsProfileID", "DimensionsProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "DimensionsProfileEntity", "Name", "Name", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "DimensionsProfileEntity", "Length", "Length", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 2 );
			base.AddElementFieldMapping( "DimensionsProfileEntity", "Width", "Width", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 3 );
			base.AddElementFieldMapping( "DimensionsProfileEntity", "Height", "Height", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 4 );
			base.AddElementFieldMapping( "DimensionsProfileEntity", "Weight", "Weight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 5 );
		}
		/// <summary>Inits DownloadEntity's mappings</summary>
		private void InitDownloadEntityMappings()
		{
			base.AddElementMapping( "DownloadEntity", "ShipWorksLocal", @"dbo", "Download", 13 );
			base.AddElementFieldMapping( "DownloadEntity", "DownloadID", "DownloadID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "DownloadEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "DownloadEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "DownloadEntity", "ComputerID", "ComputerID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "DownloadEntity", "UserID", "UserID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
			base.AddElementFieldMapping( "DownloadEntity", "InitiatedBy", "InitiatedBy", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
			base.AddElementFieldMapping( "DownloadEntity", "Started", "Started", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 6 );
			base.AddElementFieldMapping( "DownloadEntity", "Ended", "Ended", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 7 );
			base.AddElementFieldMapping( "DownloadEntity", "Duration", "Duration", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 8 );
			base.AddElementFieldMapping( "DownloadEntity", "QuantityTotal", "QuantityTotal", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 9 );
			base.AddElementFieldMapping( "DownloadEntity", "QuantityNew", "QuantityNew", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 10 );
			base.AddElementFieldMapping( "DownloadEntity", "Result", "Result", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 11 );
			base.AddElementFieldMapping( "DownloadEntity", "ErrorMessage", "ErrorMessage", true, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 12 );
		}
		/// <summary>Inits DownloadDetailEntity's mappings</summary>
		private void InitDownloadDetailEntityMappings()
		{
			base.AddElementMapping( "DownloadDetailEntity", "ShipWorksLocal", @"dbo", "DownloadDetail", 9 );
			base.AddElementFieldMapping( "DownloadDetailEntity", "DownloadedDetailID", "DownloadedDetailID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "DownloadDetailEntity", "DownloadID", "DownloadID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "DownloadDetailEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "DownloadDetailEntity", "InitialDownload", "InitialDownload", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "DownloadDetailEntity", "OrderNumber", "OrderNumber", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
			base.AddElementFieldMapping( "DownloadDetailEntity", "ExtraBigIntData1", "ExtraBigIntData1", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 5 );
			base.AddElementFieldMapping( "DownloadDetailEntity", "ExtraBigIntData2", "ExtraBigIntData2", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 6 );
			base.AddElementFieldMapping( "DownloadDetailEntity", "ExtraBigIntData3", "ExtraBigIntData3", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 7 );
			base.AddElementFieldMapping( "DownloadDetailEntity", "ExtraStringData1", "ExtraStringData1", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 8 );
		}
		/// <summary>Inits EbayCombinedOrderRelationEntity's mappings</summary>
		private void InitEbayCombinedOrderRelationEntityMappings()
		{
			base.AddElementMapping( "EbayCombinedOrderRelationEntity", "ShipWorksLocal", @"dbo", "EbayCombinedOrderRelation", 4 );
			base.AddElementFieldMapping( "EbayCombinedOrderRelationEntity", "EbayCombinedOrderRelationID", "EbayCombinedOrderRelationID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "EbayCombinedOrderRelationEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "EbayCombinedOrderRelationEntity", "EbayOrderID", "EbayOrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "EbayCombinedOrderRelationEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
		}
		/// <summary>Inits EbayOrderEntity's mappings</summary>
		private void InitEbayOrderEntityMappings()
		{
			base.AddElementMapping( "EbayOrderEntity", "ShipWorksLocal", @"dbo", "EbayOrder", 24 );
			base.AddElementFieldMapping( "EbayOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "EbayOrderEntity", "EbayOrderID", "EbayOrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "EbayOrderEntity", "EbayBuyerID", "EbayBuyerID", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "EbayOrderEntity", "CombinedLocally", "CombinedLocally", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "EbayOrderEntity", "SelectedShippingMethod", "SelectedShippingMethod", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "EbayOrderEntity", "SellingManagerRecord", "SellingManagerRecord", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
			base.AddElementFieldMapping( "EbayOrderEntity", "GspEligible", "GspEligible", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 6 );
			base.AddElementFieldMapping( "EbayOrderEntity", "GspFirstName", "GspFirstName", false, (int)SqlDbType.NVarChar, 128, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "EbayOrderEntity", "GspLastName", "GspLastName", false, (int)SqlDbType.NVarChar, 128, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "EbayOrderEntity", "GspStreet1", "GspStreet1", false, (int)SqlDbType.NVarChar, 512, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "EbayOrderEntity", "GspStreet2", "GspStreet2", false, (int)SqlDbType.NVarChar, 512, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "EbayOrderEntity", "GspCity", "GspCity", false, (int)SqlDbType.NVarChar, 128, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "EbayOrderEntity", "GspStateProvince", "GspStateProvince", false, (int)SqlDbType.NVarChar, 128, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "EbayOrderEntity", "GspPostalCode", "GspPostalCode", false, (int)SqlDbType.NVarChar, 9, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "EbayOrderEntity", "GspCountryCode", "GspCountryCode", false, (int)SqlDbType.NVarChar, 2, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "EbayOrderEntity", "GspReferenceID", "GspReferenceID", false, (int)SqlDbType.NVarChar, 128, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "EbayOrderEntity", "RollupEbayItemCount", "RollupEbayItemCount", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 16 );
			base.AddElementFieldMapping( "EbayOrderEntity", "RollupEffectiveCheckoutStatus", "RollupEffectiveCheckoutStatus", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 17 );
			base.AddElementFieldMapping( "EbayOrderEntity", "RollupEffectivePaymentMethod", "RollupEffectivePaymentMethod", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 18 );
			base.AddElementFieldMapping( "EbayOrderEntity", "RollupFeedbackLeftType", "RollupFeedbackLeftType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 19 );
			base.AddElementFieldMapping( "EbayOrderEntity", "RollupFeedbackLeftComments", "RollupFeedbackLeftComments", true, (int)SqlDbType.VarChar, 80, 0, 0, false, "", null, typeof(System.String), 20 );
			base.AddElementFieldMapping( "EbayOrderEntity", "RollupFeedbackReceivedType", "RollupFeedbackReceivedType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 21 );
			base.AddElementFieldMapping( "EbayOrderEntity", "RollupFeedbackReceivedComments", "RollupFeedbackReceivedComments", true, (int)SqlDbType.VarChar, 80, 0, 0, false, "", null, typeof(System.String), 22 );
			base.AddElementFieldMapping( "EbayOrderEntity", "RollupPayPalAddressStatus", "RollupPayPalAddressStatus", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 23 );
		}
		/// <summary>Inits EbayOrderItemEntity's mappings</summary>
		private void InitEbayOrderItemEntityMappings()
		{
			base.AddElementMapping( "EbayOrderItemEntity", "ShipWorksLocal", @"dbo", "EbayOrderItem", 18 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "OrderItemID", "OrderItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "LocalEbayOrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "EbayItemID", "EbayItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "EbayTransactionID", "EbayTransactionID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "SellingManagerRecord", "SellingManagerRecord", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "EffectiveCheckoutStatus", "EffectiveCheckoutStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "EffectivePaymentMethod", "EffectivePaymentMethod", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 6 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "PaymentStatus", "PaymentStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 7 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "PaymentMethod", "PaymentMethod", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 8 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "CompleteStatus", "CompleteStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 9 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "FeedbackLeftType", "FeedbackLeftType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 10 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "FeedbackLeftComments", "FeedbackLeftComments", false, (int)SqlDbType.NVarChar, 80, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "FeedbackReceivedType", "FeedbackReceivedType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 12 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "FeedbackReceivedComments", "FeedbackReceivedComments", false, (int)SqlDbType.NVarChar, 80, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "MyEbayPaid", "MyEbayPaid", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 14 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "MyEbayShipped", "MyEbayShipped", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 15 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "PayPalTransactionID", "PayPalTransactionID", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "EbayOrderItemEntity", "PayPalAddressStatus", "PayPalAddressStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 17 );
		}
		/// <summary>Inits EbayStoreEntity's mappings</summary>
		private void InitEbayStoreEntityMappings()
		{
			base.AddElementMapping( "EbayStoreEntity", "ShipWorksLocal", @"dbo", "EbayStore", 16 );
			base.AddElementFieldMapping( "EbayStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "EbayStoreEntity", "EBayUserID", "eBayUserID", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "EbayStoreEntity", "EBayToken", "eBayToken", false, (int)SqlDbType.Text, 2147483647, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "EbayStoreEntity", "EBayTokenExpire", "eBayTokenExpire", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 3 );
			base.AddElementFieldMapping( "EbayStoreEntity", "AcceptedPaymentList", "AcceptedPaymentList", false, (int)SqlDbType.VarChar, 30, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "EbayStoreEntity", "DownloadItemDetails", "DownloadItemDetails", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 5 );
			base.AddElementFieldMapping( "EbayStoreEntity", "DownloadOlderOrders", "DownloadOlderOrders", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 6 );
			base.AddElementFieldMapping( "EbayStoreEntity", "DownloadPayPalDetails", "DownloadPayPalDetails", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 7 );
			base.AddElementFieldMapping( "EbayStoreEntity", "PayPalApiCredentialType", "PayPalApiCredentialType", false, (int)SqlDbType.SmallInt, 0, 0, 5, false, "", null, typeof(System.Int16), 8 );
			base.AddElementFieldMapping( "EbayStoreEntity", "PayPalApiUserName", "PayPalApiUserName", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "EbayStoreEntity", "PayPalApiPassword", "PayPalApiPassword", false, (int)SqlDbType.NVarChar, 80, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "EbayStoreEntity", "PayPalApiSignature", "PayPalApiSignature", false, (int)SqlDbType.NVarChar, 80, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "EbayStoreEntity", "PayPalApiCertificate", "PayPalApiCertificate", true, (int)SqlDbType.VarBinary, 2048, 0, 0, false, "", null, typeof(System.Byte[]), 12 );
			base.AddElementFieldMapping( "EbayStoreEntity", "DomesticShippingService", "DomesticShippingService", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "EbayStoreEntity", "InternationalShippingService", "InternationalShippingService", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "EbayStoreEntity", "FeedbackUpdatedThrough", "FeedbackUpdatedThrough", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 15 );
		}
		/// <summary>Inits EmailAccountEntity's mappings</summary>
		private void InitEmailAccountEntityMappings()
		{
			base.AddElementMapping( "EmailAccountEntity", "ShipWorksLocal", @"dbo", "EmailAccount", 27 );
			base.AddElementFieldMapping( "EmailAccountEntity", "EmailAccountID", "EmailAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "EmailAccountEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "EmailAccountEntity", "AccountName", "AccountName", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "EmailAccountEntity", "DisplayName", "DisplayName", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "EmailAccountEntity", "EmailAddress", "EmailAddress", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "EmailAccountEntity", "IncomingServer", "IncomingServer", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "EmailAccountEntity", "IncomingServerType", "IncomingServerType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 6 );
			base.AddElementFieldMapping( "EmailAccountEntity", "IncomingPort", "IncomingPort", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 7 );
			base.AddElementFieldMapping( "EmailAccountEntity", "IncomingSecurityType", "IncomingSecurityType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 8 );
			base.AddElementFieldMapping( "EmailAccountEntity", "IncomingUsername", "IncomingUsername", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "EmailAccountEntity", "IncomingPassword", "IncomingPassword", false, (int)SqlDbType.NVarChar, 150, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "EmailAccountEntity", "OutgoingServer", "OutgoingServer", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "EmailAccountEntity", "OutgoingPort", "OutgoingPort", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 12 );
			base.AddElementFieldMapping( "EmailAccountEntity", "OutgoingSecurityType", "OutgoingSecurityType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 13 );
			base.AddElementFieldMapping( "EmailAccountEntity", "OutgoingCredentialSource", "OutgoingCredentialSource", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 14 );
			base.AddElementFieldMapping( "EmailAccountEntity", "OutgoingUsername", "OutgoingUsername", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "EmailAccountEntity", "OutgoingPassword", "OutgoingPassword", false, (int)SqlDbType.NVarChar, 150, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "EmailAccountEntity", "AutoSend", "AutoSend", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 17 );
			base.AddElementFieldMapping( "EmailAccountEntity", "AutoSendMinutes", "AutoSendMinutes", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 18 );
			base.AddElementFieldMapping( "EmailAccountEntity", "AutoSendLastTime", "AutoSendLastTime", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 19 );
			base.AddElementFieldMapping( "EmailAccountEntity", "LimitMessagesPerConnection", "LimitMessagesPerConnection", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 20 );
			base.AddElementFieldMapping( "EmailAccountEntity", "LimitMessagesPerConnectionQuantity", "LimitMessagesPerConnectionQuantity", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 21 );
			base.AddElementFieldMapping( "EmailAccountEntity", "LimitMessagesPerHour", "LimitMessagesPerHour", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 22 );
			base.AddElementFieldMapping( "EmailAccountEntity", "LimitMessagesPerHourQuantity", "LimitMessagesPerHourQuantity", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 23 );
			base.AddElementFieldMapping( "EmailAccountEntity", "LimitMessageInterval", "LimitMessageInterval", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 24 );
			base.AddElementFieldMapping( "EmailAccountEntity", "LimitMessageIntervalSeconds", "LimitMessageIntervalSeconds", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 25 );
			base.AddElementFieldMapping( "EmailAccountEntity", "InternalOwnerID", "InternalOwnerID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 26 );
		}
		/// <summary>Inits EmailOutboundEntity's mappings</summary>
		private void InitEmailOutboundEntityMappings()
		{
			base.AddElementMapping( "EmailOutboundEntity", "ShipWorksLocal", @"dbo", "EmailOutbound", 21 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "EmailOutboundID", "EmailOutboundID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "ContextID", "ContextID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "ContextType", "ContextType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "TemplateID", "TemplateID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "AccountID", "AccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 5 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "Visibility", "Visibility", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 6 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "FromAddress", "FromAddress", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "ToList", "ToList", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "CcList", "CcList", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "BccList", "BccList", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "Subject", "Subject", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "HtmlPartResourceID", "HtmlPartResourceID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 12 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "PlainPartResourceID", "PlainPartResourceID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 13 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "Encoding", "Encoding", true, (int)SqlDbType.VarChar, 20, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "ComposedDate", "ComposedDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 15 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "SentDate", "SentDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 16 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "DontSendBefore", "DontSendBefore", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 17 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "SendStatus", "SendStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 18 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "SendAttemptCount", "SendAttemptCount", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 19 );
			base.AddElementFieldMapping( "EmailOutboundEntity", "SendAttemptLastError", "SendAttemptLastError", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 20 );
		}
		/// <summary>Inits EmailOutboundRelationEntity's mappings</summary>
		private void InitEmailOutboundRelationEntityMappings()
		{
			base.AddElementMapping( "EmailOutboundRelationEntity", "ShipWorksLocal", @"dbo", "EmailOutboundRelation", 4 );
			base.AddElementFieldMapping( "EmailOutboundRelationEntity", "EmailOutboundRelationID", "EmailOutboundRelationID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "EmailOutboundRelationEntity", "EmailOutboundID", "EmailOutboundID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "EmailOutboundRelationEntity", "ObjectID", "ObjectID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "EmailOutboundRelationEntity", "RelationType", "RelationType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
		}
		/// <summary>Inits EndiciaAccountEntity's mappings</summary>
		private void InitEndiciaAccountEntityMappings()
		{
			base.AddElementMapping( "EndiciaAccountEntity", "ShipWorksLocal", @"dbo", "EndiciaAccount", 26 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "EndiciaAccountID", "EndiciaAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "EndiciaReseller", "EndiciaReseller", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 1 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "AccountNumber", "AccountNumber", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "SignupConfirmation", "SignupConfirmation", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "WebPassword", "WebPassword", false, (int)SqlDbType.NVarChar, 250, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "ApiInitialPassword", "ApiInitialPassword", false, (int)SqlDbType.NVarChar, 250, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "ApiUserPassword", "ApiUserPassword", false, (int)SqlDbType.NVarChar, 250, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "AccountType", "AccountType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 7 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "TestAccount", "TestAccount", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 8 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "CreatedByShipWorks", "CreatedByShipWorks", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 9 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "Description", "Description", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "FirstName", "FirstName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "LastName", "LastName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "Company", "Company", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "Street1", "Street1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "Street2", "Street2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "Street3", "Street3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "City", "City", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 17 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "StateProvCode", "StateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "PostalCode", "PostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 19 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "CountryCode", "CountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 20 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "Phone", "Phone", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 21 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "Fax", "Fax", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 22 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "Email", "Email", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 23 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "MailingPostalCode", "MailingPostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 24 );
			base.AddElementFieldMapping( "EndiciaAccountEntity", "ScanFormAddressSource", "ScanFormAddressSource", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 25 );
		}
		/// <summary>Inits EndiciaProfileEntity's mappings</summary>
		private void InitEndiciaProfileEntityMappings()
		{
			base.AddElementMapping( "EndiciaProfileEntity", "ShipWorksLocal", @"dbo", "EndiciaProfile", 5 );
			base.AddElementFieldMapping( "EndiciaProfileEntity", "ShippingProfileID", "ShippingProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "EndiciaProfileEntity", "EndiciaAccountID", "EndiciaAccountID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "EndiciaProfileEntity", "StealthPostage", "StealthPostage", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 2 );
			base.AddElementFieldMapping( "EndiciaProfileEntity", "ReferenceID", "ReferenceID", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "EndiciaProfileEntity", "ScanBasedReturn", "ScanBasedReturn", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 4 );
		}
		/// <summary>Inits EndiciaScanFormEntity's mappings</summary>
		private void InitEndiciaScanFormEntityMappings()
		{
			base.AddElementMapping( "EndiciaScanFormEntity", "ShipWorksLocal", @"dbo", "EndiciaScanForm", 7 );
			base.AddElementFieldMapping( "EndiciaScanFormEntity", "EndiciaScanFormID", "EndiciaScanFormID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "EndiciaScanFormEntity", "EndiciaAccountID", "EndiciaAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "EndiciaScanFormEntity", "EndiciaAccountNumber", "EndiciaAccountNumber", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "EndiciaScanFormEntity", "SubmissionID", "SubmissionID", false, (int)SqlDbType.VarChar, 100, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "EndiciaScanFormEntity", "CreatedDate", "CreatedDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 4 );
			base.AddElementFieldMapping( "EndiciaScanFormEntity", "ScanFormBatchID", "ScanFormBatchID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 5 );
			base.AddElementFieldMapping( "EndiciaScanFormEntity", "Description", "Description", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 6 );
		}
		/// <summary>Inits EndiciaShipmentEntity's mappings</summary>
		private void InitEndiciaShipmentEntityMappings()
		{
			base.AddElementMapping( "EndiciaShipmentEntity", "ShipWorksLocal", @"dbo", "EndiciaShipment", 10 );
			base.AddElementFieldMapping( "EndiciaShipmentEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "EndiciaShipmentEntity", "EndiciaAccountID", "EndiciaAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "EndiciaShipmentEntity", "OriginalEndiciaAccountID", "OriginalEndiciaAccountID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "EndiciaShipmentEntity", "StealthPostage", "StealthPostage", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "EndiciaShipmentEntity", "ReferenceID", "ReferenceID", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "EndiciaShipmentEntity", "TransactionID", "TransactionID", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
			base.AddElementFieldMapping( "EndiciaShipmentEntity", "RefundFormID", "RefundFormID", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 6 );
			base.AddElementFieldMapping( "EndiciaShipmentEntity", "ScanFormBatchID", "ScanFormBatchID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 7 );
			base.AddElementFieldMapping( "EndiciaShipmentEntity", "ScanBasedReturn", "ScanBasedReturn", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 8 );
			base.AddElementFieldMapping( "EndiciaShipmentEntity", "RequestedLabelFormat", "RequestedLabelFormat", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 9 );
		}
		/// <summary>Inits EtsyOrderEntity's mappings</summary>
		private void InitEtsyOrderEntityMappings()
		{
			base.AddElementMapping( "EtsyOrderEntity", "ShipWorksLocal", @"dbo", "EtsyOrder", 3 );
			base.AddElementFieldMapping( "EtsyOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "EtsyOrderEntity", "WasPaid", "WasPaid", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 1 );
			base.AddElementFieldMapping( "EtsyOrderEntity", "WasShipped", "WasShipped", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 2 );
		}
		/// <summary>Inits EtsyStoreEntity's mappings</summary>
		private void InitEtsyStoreEntityMappings()
		{
			base.AddElementMapping( "EtsyStoreEntity", "ShipWorksLocal", @"dbo", "EtsyStore", 6 );
			base.AddElementFieldMapping( "EtsyStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "EtsyStoreEntity", "EtsyShopID", "EtsyShopID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "EtsyStoreEntity", "EtsyLoginName", "EtsyLogin", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "EtsyStoreEntity", "EtsyStoreName", "EtsyStoreName", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "EtsyStoreEntity", "OAuthToken", "OAuthToken", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "EtsyStoreEntity", "OAuthTokenSecret", "OAuthTokenSecret", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 5 );
		}
		/// <summary>Inits ExcludedPackageTypeEntity's mappings</summary>
		private void InitExcludedPackageTypeEntityMappings()
		{
			base.AddElementMapping( "ExcludedPackageTypeEntity", "ShipWorksLocal", @"dbo", "ExcludedPackageType", 2 );
			base.AddElementFieldMapping( "ExcludedPackageTypeEntity", "ShipmentType", "ShipmentType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 0 );
			base.AddElementFieldMapping( "ExcludedPackageTypeEntity", "PackageType", "PackageType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 1 );
		}
		/// <summary>Inits ExcludedServiceTypeEntity's mappings</summary>
		private void InitExcludedServiceTypeEntityMappings()
		{
			base.AddElementMapping( "ExcludedServiceTypeEntity", "ShipWorksLocal", @"dbo", "ExcludedServiceType", 2 );
			base.AddElementFieldMapping( "ExcludedServiceTypeEntity", "ShipmentType", "ShipmentType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 0 );
			base.AddElementFieldMapping( "ExcludedServiceTypeEntity", "ServiceType", "ServiceType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 1 );
		}
		/// <summary>Inits FedExAccountEntity's mappings</summary>
		private void InitFedExAccountEntityMappings()
		{
			base.AddElementMapping( "FedExAccountEntity", "ShipWorksLocal", @"dbo", "FedExAccount", 20 );
			base.AddElementFieldMapping( "FedExAccountEntity", "FedExAccountID", "FedExAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "FedExAccountEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "FedExAccountEntity", "Description", "Description", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "FedExAccountEntity", "AccountNumber", "AccountNumber", false, (int)SqlDbType.NVarChar, 12, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "FedExAccountEntity", "SignatureRelease", "SignatureRelease", false, (int)SqlDbType.VarChar, 10, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "FedExAccountEntity", "MeterNumber", "MeterNumber", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "FedExAccountEntity", "SmartPostHubList", "SmartPostHubList", false, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "FedExAccountEntity", "FirstName", "FirstName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "FedExAccountEntity", "MiddleName", "MiddleName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "FedExAccountEntity", "LastName", "LastName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "FedExAccountEntity", "Company", "Company", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "FedExAccountEntity", "Street1", "Street1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "FedExAccountEntity", "Street2", "Street2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "FedExAccountEntity", "City", "City", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "FedExAccountEntity", "StateProvCode", "StateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "FedExAccountEntity", "PostalCode", "PostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "FedExAccountEntity", "CountryCode", "CountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "FedExAccountEntity", "Phone", "Phone", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 17 );
			base.AddElementFieldMapping( "FedExAccountEntity", "Email", "Email", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "FedExAccountEntity", "Website", "Website", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 19 );
		}
		/// <summary>Inits FedExEndOfDayCloseEntity's mappings</summary>
		private void InitFedExEndOfDayCloseEntityMappings()
		{
			base.AddElementMapping( "FedExEndOfDayCloseEntity", "ShipWorksLocal", @"dbo", "FedExEndOfDayClose", 5 );
			base.AddElementFieldMapping( "FedExEndOfDayCloseEntity", "FedExEndOfDayCloseID", "FedExEndOfDayCloseID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "FedExEndOfDayCloseEntity", "FedExAccountID", "FedExAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "FedExEndOfDayCloseEntity", "AccountNumber", "AccountNumber", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "FedExEndOfDayCloseEntity", "CloseDate", "CloseDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 3 );
			base.AddElementFieldMapping( "FedExEndOfDayCloseEntity", "IsSmartPost", "IsSmartPost", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 4 );
		}
		/// <summary>Inits FedExPackageEntity's mappings</summary>
		private void InitFedExPackageEntityMappings()
		{
			base.AddElementMapping( "FedExPackageEntity", "ShipWorksLocal", @"dbo", "FedExPackage", 34 );
			base.AddElementFieldMapping( "FedExPackageEntity", "FedExPackageID", "FedExPackageID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "FedExPackageEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "FedExPackageEntity", "Weight", "Weight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 2 );
			base.AddElementFieldMapping( "FedExPackageEntity", "DimsProfileID", "DimsProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "FedExPackageEntity", "DimsLength", "DimsLength", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 4 );
			base.AddElementFieldMapping( "FedExPackageEntity", "DimsWidth", "DimsWidth", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 5 );
			base.AddElementFieldMapping( "FedExPackageEntity", "DimsHeight", "DimsHeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 6 );
			base.AddElementFieldMapping( "FedExPackageEntity", "DimsWeight", "DimsWeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 7 );
			base.AddElementFieldMapping( "FedExPackageEntity", "DimsAddWeight", "DimsAddWeight", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 8 );
			base.AddElementFieldMapping( "FedExPackageEntity", "SkidPieces", "SkidPieces", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 9 );
			base.AddElementFieldMapping( "FedExPackageEntity", "Insurance", "Insurance", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 10 );
			base.AddElementFieldMapping( "FedExPackageEntity", "InsuranceValue", "InsuranceValue", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 11 );
			base.AddElementFieldMapping( "FedExPackageEntity", "InsurancePennyOne", "InsurancePennyOne", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 12 );
			base.AddElementFieldMapping( "FedExPackageEntity", "DeclaredValue", "DeclaredValue", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 13 );
			base.AddElementFieldMapping( "FedExPackageEntity", "TrackingNumber", "TrackingNumber", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "FedExPackageEntity", "PriorityAlert", "PriorityAlert", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 15 );
			base.AddElementFieldMapping( "FedExPackageEntity", "PriorityAlertEnhancementType", "PriorityAlertEnhancementType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 16 );
			base.AddElementFieldMapping( "FedExPackageEntity", "PriorityAlertDetailContent", "PriorityAlertDetailContent", false, (int)SqlDbType.NVarChar, 1024, 0, 0, false, "", null, typeof(System.String), 17 );
			base.AddElementFieldMapping( "FedExPackageEntity", "DryIceWeight", "DryIceWeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 18 );
			base.AddElementFieldMapping( "FedExPackageEntity", "ContainsAlcohol", "ContainsAlcohol", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 19 );
			base.AddElementFieldMapping( "FedExPackageEntity", "DangerousGoodsEnabled", "DangerousGoodsEnabled", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 20 );
			base.AddElementFieldMapping( "FedExPackageEntity", "DangerousGoodsType", "DangerousGoodsType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 21 );
			base.AddElementFieldMapping( "FedExPackageEntity", "DangerousGoodsAccessibilityType", "DangerousGoodsAccessibilityType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 22 );
			base.AddElementFieldMapping( "FedExPackageEntity", "DangerousGoodsCargoAircraftOnly", "DangerousGoodsCargoAircraftOnly", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 23 );
			base.AddElementFieldMapping( "FedExPackageEntity", "DangerousGoodsEmergencyContactPhone", "DangerousGoodsEmergencyContactPhone", false, (int)SqlDbType.NVarChar, 16, 0, 0, false, "", null, typeof(System.String), 24 );
			base.AddElementFieldMapping( "FedExPackageEntity", "DangerousGoodsOfferor", "DangerousGoodsOfferor", false, (int)SqlDbType.NVarChar, 128, 0, 0, false, "", null, typeof(System.String), 25 );
			base.AddElementFieldMapping( "FedExPackageEntity", "DangerousGoodsPackagingCount", "DangerousGoodsPackagingCount", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 26 );
			base.AddElementFieldMapping( "FedExPackageEntity", "HazardousMaterialNumber", "HazardousMaterialNumber", false, (int)SqlDbType.NVarChar, 16, 0, 0, false, "", null, typeof(System.String), 27 );
			base.AddElementFieldMapping( "FedExPackageEntity", "HazardousMaterialClass", "HazardousMaterialClass", false, (int)SqlDbType.NVarChar, 8, 0, 0, false, "", null, typeof(System.String), 28 );
			base.AddElementFieldMapping( "FedExPackageEntity", "HazardousMaterialProperName", "HazardousMaterialProperName", false, (int)SqlDbType.NVarChar, 64, 0, 0, false, "", null, typeof(System.String), 29 );
			base.AddElementFieldMapping( "FedExPackageEntity", "HazardousMaterialPackingGroup", "HazardousMaterialPackingGroup", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 30 );
			base.AddElementFieldMapping( "FedExPackageEntity", "HazardousMaterialQuantityValue", "HazardousMaterialQuantityValue", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 31 );
			base.AddElementFieldMapping( "FedExPackageEntity", "HazardousMaterialQuanityUnits", "HazardousMaterialQuanityUnits", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 32 );
			base.AddElementFieldMapping( "FedExPackageEntity", "HazardousMaterialTechnicalName", "HazardousMaterialTechnicalName", false, (int)SqlDbType.NVarChar, 64, 0, 0, false, "", null, typeof(System.String), 33 );
		}
		/// <summary>Inits FedExProfileEntity's mappings</summary>
		private void InitFedExProfileEntityMappings()
		{
			base.AddElementMapping( "FedExProfileEntity", "ShipWorksLocal", @"dbo", "FedExProfile", 36 );
			base.AddElementFieldMapping( "FedExProfileEntity", "ShippingProfileID", "ShippingProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "FedExProfileEntity", "FedExAccountID", "FedExAccountID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "FedExProfileEntity", "Service", "Service", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "FedExProfileEntity", "Signature", "Signature", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "FedExProfileEntity", "PackagingType", "PackagingType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "FedExProfileEntity", "NonStandardContainer", "NonStandardContainer", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 5 );
			base.AddElementFieldMapping( "FedExProfileEntity", "ReferenceFIMS", "ReferenceFIMS", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "FedExProfileEntity", "ReferenceCustomer", "ReferenceCustomer", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "FedExProfileEntity", "ReferenceInvoice", "ReferenceInvoice", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "FedExProfileEntity", "ReferencePO", "ReferencePO", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "FedExProfileEntity", "ReferenceShipmentIntegrity", "ReferenceShipmentIntegrity", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "FedExProfileEntity", "PayorTransportType", "PayorTransportType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 11 );
			base.AddElementFieldMapping( "FedExProfileEntity", "PayorTransportAccount", "PayorTransportAccount", true, (int)SqlDbType.VarChar, 12, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "FedExProfileEntity", "PayorDutiesType", "PayorDutiesType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 13 );
			base.AddElementFieldMapping( "FedExProfileEntity", "PayorDutiesAccount", "PayorDutiesAccount", true, (int)SqlDbType.VarChar, 12, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "FedExProfileEntity", "SaturdayDelivery", "SaturdayDelivery", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 15 );
			base.AddElementFieldMapping( "FedExProfileEntity", "EmailNotifySender", "EmailNotifySender", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 16 );
			base.AddElementFieldMapping( "FedExProfileEntity", "EmailNotifyRecipient", "EmailNotifyRecipient", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 17 );
			base.AddElementFieldMapping( "FedExProfileEntity", "EmailNotifyOther", "EmailNotifyOther", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 18 );
			base.AddElementFieldMapping( "FedExProfileEntity", "EmailNotifyOtherAddress", "EmailNotifyOtherAddress", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 19 );
			base.AddElementFieldMapping( "FedExProfileEntity", "EmailNotifyMessage", "EmailNotifyMessage", true, (int)SqlDbType.VarChar, 120, 0, 0, false, "", null, typeof(System.String), 20 );
			base.AddElementFieldMapping( "FedExProfileEntity", "ResidentialDetermination", "ResidentialDetermination", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 21 );
			base.AddElementFieldMapping( "FedExProfileEntity", "SmartPostIndicia", "SmartPostIndicia", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 22 );
			base.AddElementFieldMapping( "FedExProfileEntity", "SmartPostEndorsement", "SmartPostEndorsement", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 23 );
			base.AddElementFieldMapping( "FedExProfileEntity", "SmartPostConfirmation", "SmartPostConfirmation", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 24 );
			base.AddElementFieldMapping( "FedExProfileEntity", "SmartPostCustomerManifest", "SmartPostCustomerManifest", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 25 );
			base.AddElementFieldMapping( "FedExProfileEntity", "SmartPostHubID", "SmartPostHubID", true, (int)SqlDbType.VarChar, 10, 0, 0, false, "", null, typeof(System.String), 26 );
			base.AddElementFieldMapping( "FedExProfileEntity", "EmailNotifyBroker", "EmailNotifyBroker", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 27 );
			base.AddElementFieldMapping( "FedExProfileEntity", "DropoffType", "DropoffType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 28 );
			base.AddElementFieldMapping( "FedExProfileEntity", "OriginResidentialDetermination", "OriginResidentialDetermination", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 29 );
			base.AddElementFieldMapping( "FedExProfileEntity", "PayorTransportName", "PayorTransportName", true, (int)SqlDbType.NChar, 60, 0, 0, false, "", null, typeof(System.String), 30 );
			base.AddElementFieldMapping( "FedExProfileEntity", "ReturnType", "ReturnType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 31 );
			base.AddElementFieldMapping( "FedExProfileEntity", "RmaNumber", "RmaNumber", true, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 32 );
			base.AddElementFieldMapping( "FedExProfileEntity", "RmaReason", "RmaReason", true, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 33 );
			base.AddElementFieldMapping( "FedExProfileEntity", "ReturnSaturdayPickup", "ReturnSaturdayPickup", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 34 );
			base.AddElementFieldMapping( "FedExProfileEntity", "ReturnsClearance", "ReturnsClearance", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 35 );
		}
		/// <summary>Inits FedExProfilePackageEntity's mappings</summary>
		private void InitFedExProfilePackageEntityMappings()
		{
			base.AddElementMapping( "FedExProfilePackageEntity", "ShipWorksLocal", @"dbo", "FedExProfilePackage", 27 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "FedExProfilePackageID", "FedExProfilePackageID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "ShippingProfileID", "ShippingProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "Weight", "Weight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 2 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "DimsProfileID", "DimsProfileID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "DimsLength", "DimsLength", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 4 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "DimsWidth", "DimsWidth", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 5 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "DimsHeight", "DimsHeight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 6 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "DimsWeight", "DimsWeight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 7 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "DimsAddWeight", "DimsAddWeight", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 8 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "PriorityAlert", "PriorityAlert", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 9 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "PriorityAlertEnhancementType", "PriorityAlertEnhancementType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 10 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "PriorityAlertDetailContent", "PriorityAlertDetailContent", true, (int)SqlDbType.NVarChar, 1024, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "DryIceWeight", "DryIceWeight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 12 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "ContainsAlcohol", "ContainsAlcohol", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 13 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "DangerousGoodsEnabled", "DangerousGoodsEnabled", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 14 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "DangerousGoodsType", "DangerousGoodsType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 15 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "DangerousGoodsAccessibilityType", "DangerousGoodsAccessibilityType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 16 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "DangerousGoodsCargoAircraftOnly", "DangerousGoodsCargoAircraftOnly", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 17 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "DangerousGoodsEmergencyContactPhone", "DangerousGoodsEmergencyContactPhone", true, (int)SqlDbType.NVarChar, 16, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "DangerousGoodsOfferor", "DangerousGoodsOfferor", true, (int)SqlDbType.NVarChar, 128, 0, 0, false, "", null, typeof(System.String), 19 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "DangerousGoodsPackagingCount", "DangerousGoodsPackagingCount", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 20 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "HazardousMaterialNumber", "HazardousMaterialNumber", true, (int)SqlDbType.NVarChar, 16, 0, 0, false, "", null, typeof(System.String), 21 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "HazardousMaterialClass", "HazardousMaterialClass", true, (int)SqlDbType.NVarChar, 8, 0, 0, false, "", null, typeof(System.String), 22 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "HazardousMaterialProperName", "HazardousMaterialProperName", true, (int)SqlDbType.NVarChar, 64, 0, 0, false, "", null, typeof(System.String), 23 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "HazardousMaterialPackingGroup", "HazardousMaterialPackingGroup", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 24 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "HazardousMaterialQuantityValue", "HazardousMaterialQuantityValue", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 25 );
			base.AddElementFieldMapping( "FedExProfilePackageEntity", "HazardousMaterialQuanityUnits", "HazardousMaterialQuanityUnits", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 26 );
		}
		/// <summary>Inits FedExShipmentEntity's mappings</summary>
		private void InitFedExShipmentEntityMappings()
		{
			base.AddElementMapping( "FedExShipmentEntity", "ShipWorksLocal", @"dbo", "FedExShipment", 153 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "FedExAccountID", "FedExAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "MasterFormID", "MasterFormID", false, (int)SqlDbType.VarChar, 4, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "Service", "Service", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "Signature", "Signature", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "PackagingType", "PackagingType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "NonStandardContainer", "NonStandardContainer", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 6 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ReferenceFIMS", "ReferenceFIMS", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ReferenceCustomer", "ReferenceCustomer", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ReferenceInvoice", "ReferenceInvoice", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ReferencePO", "ReferencePO", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ReferenceShipmentIntegrity", "ReferenceShipmentIntegrity", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "PayorTransportType", "PayorTransportType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 12 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "PayorTransportName", "PayorTransportName", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "PayorTransportAccount", "PayorTransportAccount", false, (int)SqlDbType.VarChar, 12, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "PayorDutiesType", "PayorDutiesType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 15 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "PayorDutiesAccount", "PayorDutiesAccount", false, (int)SqlDbType.VarChar, 12, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "PayorDutiesName", "PayorDutiesName", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 17 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "PayorDutiesCountryCode", "PayorDutiesCountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "SaturdayDelivery", "SaturdayDelivery", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 19 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HomeDeliveryType", "HomeDeliveryType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 20 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HomeDeliveryInstructions", "HomeDeliveryInstructions", false, (int)SqlDbType.VarChar, 74, 0, 0, false, "", null, typeof(System.String), 21 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HomeDeliveryDate", "HomeDeliveryDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 22 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HomeDeliveryPhone", "HomeDeliveryPhone", false, (int)SqlDbType.VarChar, 24, 0, 0, false, "", null, typeof(System.String), 23 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "FreightInsidePickup", "FreightInsidePickup", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 24 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "FreightInsideDelivery", "FreightInsideDelivery", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 25 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "FreightBookingNumber", "FreightBookingNumber", false, (int)SqlDbType.VarChar, 12, 0, 0, false, "", null, typeof(System.String), 26 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "FreightLoadAndCount", "FreightLoadAndCount", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 27 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "EmailNotifyBroker", "EmailNotifyBroker", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 28 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "EmailNotifySender", "EmailNotifySender", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 29 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "EmailNotifyRecipient", "EmailNotifyRecipient", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 30 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "EmailNotifyOther", "EmailNotifyOther", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 31 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "EmailNotifyOtherAddress", "EmailNotifyOtherAddress", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 32 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "EmailNotifyMessage", "EmailNotifyMessage", false, (int)SqlDbType.VarChar, 120, 0, 0, false, "", null, typeof(System.String), 33 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodEnabled", "CodEnabled", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 34 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodAmount", "CodAmount", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 35 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodPaymentType", "CodPaymentType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 36 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodAddFreight", "CodAddFreight", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 37 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodOriginID", "CodOriginID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 38 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodFirstName", "CodFirstName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 39 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodLastName", "CodLastName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 40 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodCompany", "CodCompany", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 41 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodStreet1", "CodStreet1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 42 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodStreet2", "CodStreet2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 43 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodStreet3", "CodStreet3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 44 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodCity", "CodCity", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 45 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodStateProvCode", "CodStateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 46 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodPostalCode", "CodPostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 47 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodCountryCode", "CodCountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 48 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodPhone", "CodPhone", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 49 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodTrackingNumber", "CodTrackingNumber", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 50 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodTrackingFormID", "CodTrackingFormID", false, (int)SqlDbType.VarChar, 4, 0, 0, false, "", null, typeof(System.String), 51 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodTIN", "CodTIN", false, (int)SqlDbType.NVarChar, 24, 0, 0, false, "", null, typeof(System.String), 52 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodChargeBasis", "CodChargeBasis", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 53 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CodAccountNumber", "CodAccountNumber", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 54 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "BrokerEnabled", "BrokerEnabled", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 55 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "BrokerAccount", "BrokerAccount", false, (int)SqlDbType.NVarChar, 12, 0, 0, false, "", null, typeof(System.String), 56 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "BrokerFirstName", "BrokerFirstName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 57 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "BrokerLastName", "BrokerLastName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 58 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "BrokerCompany", "BrokerCompany", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 59 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "BrokerStreet1", "BrokerStreet1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 60 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "BrokerStreet2", "BrokerStreet2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 61 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "BrokerStreet3", "BrokerStreet3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 62 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "BrokerCity", "BrokerCity", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 63 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "BrokerStateProvCode", "BrokerStateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 64 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "BrokerPostalCode", "BrokerPostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 65 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "BrokerCountryCode", "BrokerCountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 66 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "BrokerPhone", "BrokerPhone", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 67 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "BrokerPhoneExtension", "BrokerPhoneExtension", false, (int)SqlDbType.NVarChar, 8, 0, 0, false, "", null, typeof(System.String), 68 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "BrokerEmail", "BrokerEmail", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 69 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CustomsAdmissibilityPackaging", "CustomsAdmissibilityPackaging", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 70 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CustomsRecipientTIN", "CustomsRecipientTIN", false, (int)SqlDbType.VarChar, 24, 0, 0, false, "", null, typeof(System.String), 71 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CustomsDocumentsOnly", "CustomsDocumentsOnly", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 72 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CustomsDocumentsDescription", "CustomsDocumentsDescription", false, (int)SqlDbType.NVarChar, 150, 0, 0, false, "", null, typeof(System.String), 73 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CustomsExportFilingOption", "CustomsExportFilingOption", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 74 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CustomsAESEEI", "CustomsAESEEI", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 75 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CustomsRecipientIdentificationType", "CustomsRecipientIdentificationType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 76 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CustomsRecipientIdentificationValue", "CustomsRecipientIdentificationValue", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 77 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CustomsOptionsType", "CustomsOptionsType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 78 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CustomsOptionsDesription", "CustomsOptionsDesription", false, (int)SqlDbType.NVarChar, 32, 0, 0, false, "", null, typeof(System.String), 79 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CommercialInvoice", "CommercialInvoice", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 80 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CommercialInvoiceFileElectronically", "CommercialInvoiceFileElectronically", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 81 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CommercialInvoiceTermsOfSale", "CommercialInvoiceTermsOfSale", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 82 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CommercialInvoicePurpose", "CommercialInvoicePurpose", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 83 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CommercialInvoiceComments", "CommercialInvoiceComments", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 84 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CommercialInvoiceFreight", "CommercialInvoiceFreight", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 85 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CommercialInvoiceInsurance", "CommercialInvoiceInsurance", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 86 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CommercialInvoiceOther", "CommercialInvoiceOther", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 87 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CommercialInvoiceReference", "CommercialInvoiceReference", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 88 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ImporterOfRecord", "ImporterOfRecord", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 89 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ImporterAccount", "ImporterAccount", false, (int)SqlDbType.NVarChar, 12, 0, 0, false, "", null, typeof(System.String), 90 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ImporterTIN", "ImporterTIN", false, (int)SqlDbType.NVarChar, 24, 0, 0, false, "", null, typeof(System.String), 91 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ImporterFirstName", "ImporterFirstName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 92 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ImporterLastName", "ImporterLastName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 93 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ImporterCompany", "ImporterCompany", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 94 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ImporterStreet1", "ImporterStreet1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 95 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ImporterStreet2", "ImporterStreet2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 96 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ImporterStreet3", "ImporterStreet3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 97 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ImporterCity", "ImporterCity", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 98 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ImporterStateProvCode", "ImporterStateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 99 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ImporterPostalCode", "ImporterPostalCode", false, (int)SqlDbType.NVarChar, 10, 0, 0, false, "", null, typeof(System.String), 100 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ImporterCountryCode", "ImporterCountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 101 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ImporterPhone", "ImporterPhone", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 102 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "SmartPostIndicia", "SmartPostIndicia", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 103 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "SmartPostEndorsement", "SmartPostEndorsement", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 104 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "SmartPostConfirmation", "SmartPostConfirmation", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 105 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "SmartPostCustomerManifest", "SmartPostCustomerManifest", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 106 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "SmartPostHubID", "SmartPostHubID", false, (int)SqlDbType.VarChar, 10, 0, 0, false, "", null, typeof(System.String), 107 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "SmartPostUspsApplicationId", "SmartPostUspsApplicationId", false, (int)SqlDbType.NVarChar, 10, 0, 0, false, "", null, typeof(System.String), 108 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "DropoffType", "DropoffType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 109 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "OriginResidentialDetermination", "OriginResidentialDetermination", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 110 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "FedExHoldAtLocationEnabled", "FedExHoldAtLocationEnabled", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 111 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldLocationId", "HoldLocationId", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 112 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldLocationType", "HoldLocationType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 113 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldContactId", "HoldContactId", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 114 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldPersonName", "HoldPersonName", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 115 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldTitle", "HoldTitle", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 116 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldCompanyName", "HoldCompanyName", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 117 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldPhoneNumber", "HoldPhoneNumber", true, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 118 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldPhoneExtension", "HoldPhoneExtension", true, (int)SqlDbType.NVarChar, 10, 0, 0, false, "", null, typeof(System.String), 119 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldPagerNumber", "HoldPagerNumber", true, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 120 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldFaxNumber", "HoldFaxNumber", true, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 121 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldEmailAddress", "HoldEmailAddress", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 122 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldStreet1", "HoldStreet1", true, (int)SqlDbType.NVarChar, 250, 0, 0, false, "", null, typeof(System.String), 123 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldStreet2", "HoldStreet2", true, (int)SqlDbType.NVarChar, 250, 0, 0, false, "", null, typeof(System.String), 124 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldStreet3", "HoldStreet3", true, (int)SqlDbType.NVarChar, 250, 0, 0, false, "", null, typeof(System.String), 125 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldCity", "HoldCity", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 126 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldStateOrProvinceCode", "HoldStateOrProvinceCode", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 127 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldPostalCode", "HoldPostalCode", true, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 128 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldUrbanizationCode", "HoldUrbanizationCode", true, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 129 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldCountryCode", "HoldCountryCode", true, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 130 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "HoldResidential", "HoldResidential", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 131 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CustomsNaftaEnabled", "CustomsNaftaEnabled", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 132 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CustomsNaftaPreferenceType", "CustomsNaftaPreferenceType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 133 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CustomsNaftaDeterminationCode", "CustomsNaftaDeterminationCode", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 134 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CustomsNaftaProducerId", "CustomsNaftaProducerId", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 135 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "CustomsNaftaNetCostMethod", "CustomsNaftaNetCostMethod", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 136 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ReturnType", "ReturnType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 137 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "RmaNumber", "RmaNumber", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 138 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "RmaReason", "RmaReason", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 139 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ReturnSaturdayPickup", "ReturnSaturdayPickup", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 140 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "TrafficInArmsLicenseNumber", "TrafficInArmsLicenseNumber", false, (int)SqlDbType.NVarChar, 32, 0, 0, false, "", null, typeof(System.String), 141 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "IntlExportDetailType", "IntlExportDetailType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 142 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "IntlExportDetailForeignTradeZoneCode", "IntlExportDetailForeignTradeZoneCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 143 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "IntlExportDetailEntryNumber", "IntlExportDetailEntryNumber", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 144 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "IntlExportDetailLicenseOrPermitNumber", "IntlExportDetailLicenseOrPermitNumber", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 145 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "IntlExportDetailLicenseOrPermitExpirationDate", "IntlExportDetailLicenseOrPermitExpirationDate", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 146 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "WeightUnitType", "WeightUnitType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 147 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "LinearUnitType", "LinearUnitType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 148 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "RequestedLabelFormat", "RequestedLabelFormat", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 149 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "FimsAirWaybill", "FimsAirWaybill", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 150 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "ReturnsClearance", "ReturnsClearance", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 151 );
			base.AddElementFieldMapping( "FedExShipmentEntity", "MaskedData", "MaskedData", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 152 );
		}
		/// <summary>Inits FilterEntity's mappings</summary>
		private void InitFilterEntityMappings()
		{
			base.AddElementMapping( "FilterEntity", "ShipWorksLocal", @"dbo", "Filter", 7 );
			base.AddElementFieldMapping( "FilterEntity", "FilterID", "FilterID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "FilterEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "FilterEntity", "Name", "Name", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "FilterEntity", "FilterTarget", "FilterTarget", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "FilterEntity", "IsFolder", "IsFolder", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 4 );
			base.AddElementFieldMapping( "FilterEntity", "Definition", "Definition", true, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "FilterEntity", "State", "State", false, (int)SqlDbType.TinyInt, 0, 0, 3, false, "", null, typeof(System.Byte), 6 );
		}
		/// <summary>Inits FilterLayoutEntity's mappings</summary>
		private void InitFilterLayoutEntityMappings()
		{
			base.AddElementMapping( "FilterLayoutEntity", "ShipWorksLocal", @"dbo", "FilterLayout", 5 );
			base.AddElementFieldMapping( "FilterLayoutEntity", "FilterLayoutID", "FilterLayoutID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "FilterLayoutEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "FilterLayoutEntity", "UserID", "UserID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "FilterLayoutEntity", "FilterTarget", "FilterTarget", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "FilterLayoutEntity", "FilterNodeID", "FilterNodeID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
		}
		/// <summary>Inits FilterNodeEntity's mappings</summary>
		private void InitFilterNodeEntityMappings()
		{
			base.AddElementMapping( "FilterNodeEntity", "ShipWorksLocal", @"dbo", "FilterNode", 7 );
			base.AddElementFieldMapping( "FilterNodeEntity", "FilterNodeID", "FilterNodeID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "FilterNodeEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "FilterNodeEntity", "ParentFilterNodeID", "ParentFilterNodeID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "FilterNodeEntity", "FilterSequenceID", "FilterSequenceID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "FilterNodeEntity", "FilterNodeContentID", "FilterNodeContentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
			base.AddElementFieldMapping( "FilterNodeEntity", "Created", "Created", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 5 );
			base.AddElementFieldMapping( "FilterNodeEntity", "Purpose", "Purpose", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 6 );
		}
		/// <summary>Inits FilterNodeColumnSettingsEntity's mappings</summary>
		private void InitFilterNodeColumnSettingsEntityMappings()
		{
			base.AddElementMapping( "FilterNodeColumnSettingsEntity", "ShipWorksLocal", @"dbo", "FilterNodeColumnSettings", 5 );
			base.AddElementFieldMapping( "FilterNodeColumnSettingsEntity", "FilterNodeColumnSettingsID", "FilterNodeColumnSettingsID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "FilterNodeColumnSettingsEntity", "UserID", "UserID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "FilterNodeColumnSettingsEntity", "FilterNodeID", "FilterNodeID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "FilterNodeColumnSettingsEntity", "Inherit", "Inherit", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "FilterNodeColumnSettingsEntity", "GridColumnLayoutID", "GridColumnLayoutID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
		}
		/// <summary>Inits FilterNodeContentEntity's mappings</summary>
		private void InitFilterNodeContentEntityMappings()
		{
			base.AddElementMapping( "FilterNodeContentEntity", "ShipWorksLocal", @"dbo", "FilterNodeContent", 10 );
			base.AddElementFieldMapping( "FilterNodeContentEntity", "FilterNodeContentID", "FilterNodeContentID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "FilterNodeContentEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "FilterNodeContentEntity", "CountVersion", "CountVersion", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "FilterNodeContentEntity", "Status", "Status", false, (int)SqlDbType.SmallInt, 0, 0, 5, false, "", null, typeof(System.Int16), 3 );
			base.AddElementFieldMapping( "FilterNodeContentEntity", "InitialCalculation", "InitialCalculation", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "FilterNodeContentEntity", "UpdateCalculation", "UpdateCalculation", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "FilterNodeContentEntity", "ColumnMask", "ColumnMask", false, (int)SqlDbType.VarBinary, 100, 0, 0, false, "", null, typeof(System.Byte[]), 6 );
			base.AddElementFieldMapping( "FilterNodeContentEntity", "JoinMask", "JoinMask", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 7 );
			base.AddElementFieldMapping( "FilterNodeContentEntity", "Cost", "Cost", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 8 );
			base.AddElementFieldMapping( "FilterNodeContentEntity", "Count", "Count", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 9 );
		}
		/// <summary>Inits FilterNodeContentDetailEntity's mappings</summary>
		private void InitFilterNodeContentDetailEntityMappings()
		{
			base.AddElementMapping( "FilterNodeContentDetailEntity", "ShipWorksLocal", @"dbo", "FilterNodeContentDetail", 2 );
			base.AddElementFieldMapping( "FilterNodeContentDetailEntity", "FilterNodeContentID", "FilterNodeContentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "FilterNodeContentDetailEntity", "ObjectID", "ObjectID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
		}
		/// <summary>Inits FilterSequenceEntity's mappings</summary>
		private void InitFilterSequenceEntityMappings()
		{
			base.AddElementMapping( "FilterSequenceEntity", "ShipWorksLocal", @"dbo", "FilterSequence", 5 );
			base.AddElementFieldMapping( "FilterSequenceEntity", "FilterSequenceID", "FilterSequenceID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "FilterSequenceEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "FilterSequenceEntity", "ParentFilterID", "ParentFilterID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "FilterSequenceEntity", "FilterID", "FilterID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "FilterSequenceEntity", "Position", "Position", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
		}
		/// <summary>Inits FtpAccountEntity's mappings</summary>
		private void InitFtpAccountEntityMappings()
		{
			base.AddElementMapping( "FtpAccountEntity", "ShipWorksLocal", @"dbo", "FtpAccount", 8 );
			base.AddElementFieldMapping( "FtpAccountEntity", "FtpAccountID", "FtpAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "FtpAccountEntity", "Host", "Host", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "FtpAccountEntity", "Username", "Username", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "FtpAccountEntity", "Password", "Password", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "FtpAccountEntity", "Port", "Port", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "FtpAccountEntity", "SecurityType", "SecurityType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
			base.AddElementFieldMapping( "FtpAccountEntity", "Passive", "Passive", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 6 );
			base.AddElementFieldMapping( "FtpAccountEntity", "InternalOwnerID", "InternalOwnerID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 7 );
		}
		/// <summary>Inits GenericFileStoreEntity's mappings</summary>
		private void InitGenericFileStoreEntityMappings()
		{
			base.AddElementMapping( "GenericFileStoreEntity", "ShipWorksLocal", @"dbo", "GenericFileStore", 20 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "FileFormat", "FileFormat", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 1 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "FileSource", "FileSource", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "DiskFolder", "DiskFolder", false, (int)SqlDbType.NVarChar, 355, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "FtpAccountID", "FtpAccountID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "FtpFolder", "FtpFolder", false, (int)SqlDbType.NVarChar, 355, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "EmailAccountID", "EmailAccountID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 6 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "EmailIncomingFolder", "EmailFolder", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "EmailFolderValidityID", "EmailFolderValidityID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 8 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "EmailFolderLastMessageID", "EmailFolderLastMessageID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 9 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "EmailOnlyUnread", "EmailOnlyUnread", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 10 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "NamePatternMatch", "NamePatternMatch", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "NamePatternSkip", "NamePatternSkip", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "SuccessAction", "SuccessAction", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 13 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "SuccessMoveFolder", "SuccessMoveFolder", false, (int)SqlDbType.NVarChar, 355, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "ErrorAction", "ErrorAction", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 15 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "ErrorMoveFolder", "ErrorMoveFolder", false, (int)SqlDbType.NVarChar, 355, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "XmlXsltFileName", "XmlXsltFileName", true, (int)SqlDbType.NVarChar, 355, 0, 0, false, "", null, typeof(System.String), 17 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "XmlXsltContent", "XmlXsltContent", true, (int)SqlDbType.NText, 1073741823, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "GenericFileStoreEntity", "FlatImportMap", "FlatImportMap", false, (int)SqlDbType.NText, 1073741823, 0, 0, false, "", null, typeof(System.String), 19 );
		}
		/// <summary>Inits GenericModuleStoreEntity's mappings</summary>
		private void InitGenericModuleStoreEntityMappings()
		{
			base.AddElementMapping( "GenericModuleStoreEntity", "ShipWorksLocal", @"dbo", "GenericModuleStore", 20 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModuleUsername", "ModuleUsername", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModulePassword", "ModulePassword", false, (int)SqlDbType.NVarChar, 80, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModuleUrl", "ModuleUrl", false, (int)SqlDbType.NVarChar, 350, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModuleVersion", "ModuleVersion", false, (int)SqlDbType.VarChar, 20, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModulePlatform", "ModulePlatform", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModuleDeveloper", "ModuleDeveloper", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModuleOnlineStoreCode", "ModuleOnlineStoreCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModuleStatusCodes", "ModuleStatusCodes", false, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModuleDownloadPageSize", "ModuleDownloadPageSize", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 9 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModuleRequestTimeout", "ModuleRequestTimeout", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 10 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModuleDownloadStrategy", "ModuleDownloadStrategy", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 11 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModuleOnlineStatusSupport", "ModuleOnlineStatusSupport", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 12 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModuleOnlineStatusDataType", "ModuleOnlineStatusDataType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 13 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModuleOnlineCustomerSupport", "ModuleOnlineCustomerSupport", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 14 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModuleOnlineCustomerDataType", "ModuleOnlineCustomerDataType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 15 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModuleOnlineShipmentDetails", "ModuleOnlineShipmentDetails", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 16 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModuleHttpExpect100Continue", "ModuleHttpExpect100Continue", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 17 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "ModuleResponseEncoding", "ModuleResponseEncoding", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 18 );
			base.AddElementFieldMapping( "GenericModuleStoreEntity", "SchemaVersion", "SchemaVersion", false, (int)SqlDbType.VarChar, 20, 0, 0, false, "", null, typeof(System.String), 19 );
		}
		/// <summary>Inits GridColumnFormatEntity's mappings</summary>
		private void InitGridColumnFormatEntityMappings()
		{
			base.AddElementMapping( "GridColumnFormatEntity", "ShipWorksLocal", @"dbo", "GridColumnFormat", 4 );
			base.AddElementFieldMapping( "GridColumnFormatEntity", "GridColumnFormatID", "GridColumnFormatID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "GridColumnFormatEntity", "UserID", "UserID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "GridColumnFormatEntity", "ColumnGuid", "ColumnGuid", false, (int)SqlDbType.UniqueIdentifier, 0, 0, 0, false, "", null, typeof(System.Guid), 2 );
			base.AddElementFieldMapping( "GridColumnFormatEntity", "Settings", "Settings", false, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 3 );
		}
		/// <summary>Inits GridColumnLayoutEntity's mappings</summary>
		private void InitGridColumnLayoutEntityMappings()
		{
			base.AddElementMapping( "GridColumnLayoutEntity", "ShipWorksLocal", @"dbo", "GridColumnLayout", 7 );
			base.AddElementFieldMapping( "GridColumnLayoutEntity", "GridColumnLayoutID", "GridColumnLayoutID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "GridColumnLayoutEntity", "DefinitionSet", "DefinitionSet", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 1 );
			base.AddElementFieldMapping( "GridColumnLayoutEntity", "DefaultSortColumnGuid", "DefaultSortColumnGuid", false, (int)SqlDbType.UniqueIdentifier, 0, 0, 0, false, "", null, typeof(System.Guid), 2 );
			base.AddElementFieldMapping( "GridColumnLayoutEntity", "DefaultSortOrder", "DefaultSortOrder", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "GridColumnLayoutEntity", "LastSortColumnGuid", "LastSortColumnGuid", false, (int)SqlDbType.UniqueIdentifier, 0, 0, 0, false, "", null, typeof(System.Guid), 4 );
			base.AddElementFieldMapping( "GridColumnLayoutEntity", "LastSortOrder", "LastSortOrder", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
			base.AddElementFieldMapping( "GridColumnLayoutEntity", "DetailViewSettings", "DetailViewSettings", true, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 6 );
		}
		/// <summary>Inits GridColumnPositionEntity's mappings</summary>
		private void InitGridColumnPositionEntityMappings()
		{
			base.AddElementMapping( "GridColumnPositionEntity", "ShipWorksLocal", @"dbo", "GridColumnPosition", 6 );
			base.AddElementFieldMapping( "GridColumnPositionEntity", "GridColumnPositionID", "GridColumnPositionID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "GridColumnPositionEntity", "GridColumnLayoutID", "GridColumnLayoutID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "GridColumnPositionEntity", "ColumnGuid", "ColumnGuid", false, (int)SqlDbType.UniqueIdentifier, 0, 0, 0, false, "", null, typeof(System.Guid), 2 );
			base.AddElementFieldMapping( "GridColumnPositionEntity", "Visible", "Visible", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "GridColumnPositionEntity", "Width", "Width", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "GridColumnPositionEntity", "Position", "Position", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
		}
		/// <summary>Inits GrouponOrderEntity's mappings</summary>
		private void InitGrouponOrderEntityMappings()
		{
			base.AddElementMapping( "GrouponOrderEntity", "ShipWorksLocal", @"dbo", "GrouponOrder", 2 );
			base.AddElementFieldMapping( "GrouponOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "GrouponOrderEntity", "GrouponOrderID", "GrouponOrderID", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
		}
		/// <summary>Inits GrouponOrderItemEntity's mappings</summary>
		private void InitGrouponOrderItemEntityMappings()
		{
			base.AddElementMapping( "GrouponOrderItemEntity", "ShipWorksLocal", @"dbo", "GrouponOrderItem", 6 );
			base.AddElementFieldMapping( "GrouponOrderItemEntity", "OrderItemID", "OrderItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "GrouponOrderItemEntity", "Permalink", "Permalink", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "GrouponOrderItemEntity", "ChannelSKUProvided", "ChannelSKUProvided", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "GrouponOrderItemEntity", "FulfillmentLineItemID", "FulfillmentLineItemID", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "GrouponOrderItemEntity", "BomSKU", "BomSKU", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "GrouponOrderItemEntity", "GrouponLineItemID", "GrouponLineItemID", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 5 );
		}
		/// <summary>Inits GrouponStoreEntity's mappings</summary>
		private void InitGrouponStoreEntityMappings()
		{
			base.AddElementMapping( "GrouponStoreEntity", "ShipWorksLocal", @"dbo", "GrouponStore", 3 );
			base.AddElementFieldMapping( "GrouponStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "GrouponStoreEntity", "SupplierID", "SupplierID", false, (int)SqlDbType.VarChar, 255, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "GrouponStoreEntity", "Token", "Token", false, (int)SqlDbType.VarChar, 255, 0, 0, false, "", null, typeof(System.String), 2 );
		}
		/// <summary>Inits InfopiaOrderItemEntity's mappings</summary>
		private void InitInfopiaOrderItemEntityMappings()
		{
			base.AddElementMapping( "InfopiaOrderItemEntity", "ShipWorksLocal", @"dbo", "InfopiaOrderItem", 4 );
			base.AddElementFieldMapping( "InfopiaOrderItemEntity", "OrderItemID", "OrderItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "InfopiaOrderItemEntity", "Marketplace", "Marketplace", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "InfopiaOrderItemEntity", "MarketplaceItemID", "MarketplaceItemID", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "InfopiaOrderItemEntity", "BuyerID", "BuyerID", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
		}
		/// <summary>Inits InfopiaStoreEntity's mappings</summary>
		private void InitInfopiaStoreEntityMappings()
		{
			base.AddElementMapping( "InfopiaStoreEntity", "ShipWorksLocal", @"dbo", "InfopiaStore", 2 );
			base.AddElementFieldMapping( "InfopiaStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "InfopiaStoreEntity", "ApiToken", "ApiToken", false, (int)SqlDbType.VarChar, 128, 0, 0, false, "", null, typeof(System.String), 1 );
		}
		/// <summary>Inits InsurancePolicyEntity's mappings</summary>
		private void InitInsurancePolicyEntityMappings()
		{
			base.AddElementMapping( "InsurancePolicyEntity", "ShipWorksLocal", @"dbo", "InsurancePolicy", 10 );
			base.AddElementFieldMapping( "InsurancePolicyEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "InsurancePolicyEntity", "InsureShipStoreName", "InsureShipStoreName", false, (int)SqlDbType.NVarChar, 75, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "InsurancePolicyEntity", "CreatedWithApi", "CreatedWithApi", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 2 );
			base.AddElementFieldMapping( "InsurancePolicyEntity", "ItemName", "ItemName", true, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "InsurancePolicyEntity", "Description", "Description", true, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "InsurancePolicyEntity", "ClaimType", "ClaimType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
			base.AddElementFieldMapping( "InsurancePolicyEntity", "DamageValue", "DamageValue", true, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 6 );
			base.AddElementFieldMapping( "InsurancePolicyEntity", "SubmissionDate", "SubmissionDate", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 7 );
			base.AddElementFieldMapping( "InsurancePolicyEntity", "ClaimID", "ClaimID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 8 );
			base.AddElementFieldMapping( "InsurancePolicyEntity", "EmailAddress", "EmailAddress", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 9 );
		}
		/// <summary>Inits IParcelAccountEntity's mappings</summary>
		private void InitIParcelAccountEntityMappings()
		{
			base.AddElementMapping( "IParcelAccountEntity", "ShipWorksLocal", @"dbo", "iParcelAccount", 18 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "IParcelAccountID", "iParcelAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "Username", "Username", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "Password", "Password", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "Description", "Description", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "FirstName", "FirstName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "MiddleName", "MiddleName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "LastName", "LastName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "Company", "Company", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "Street1", "Street1", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "Street2", "Street2", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "City", "City", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "StateProvCode", "StateProvCode", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "PostalCode", "PostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "CountryCode", "CountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "Phone", "Phone", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "Email", "Email", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "IParcelAccountEntity", "Website", "Website", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 17 );
		}
		/// <summary>Inits IParcelPackageEntity's mappings</summary>
		private void InitIParcelPackageEntityMappings()
		{
			base.AddElementMapping( "IParcelPackageEntity", "ShipWorksLocal", @"dbo", "iParcelPackage", 16 );
			base.AddElementFieldMapping( "IParcelPackageEntity", "IParcelPackageID", "iParcelPackageID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "IParcelPackageEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "IParcelPackageEntity", "Weight", "Weight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 2 );
			base.AddElementFieldMapping( "IParcelPackageEntity", "DimsProfileID", "DimsProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "IParcelPackageEntity", "DimsLength", "DimsLength", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 4 );
			base.AddElementFieldMapping( "IParcelPackageEntity", "DimsWidth", "DimsWidth", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 5 );
			base.AddElementFieldMapping( "IParcelPackageEntity", "DimsHeight", "DimsHeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 6 );
			base.AddElementFieldMapping( "IParcelPackageEntity", "DimsAddWeight", "DimsAddWeight", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 7 );
			base.AddElementFieldMapping( "IParcelPackageEntity", "DimsWeight", "DimsWeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 8 );
			base.AddElementFieldMapping( "IParcelPackageEntity", "Insurance", "Insurance", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 9 );
			base.AddElementFieldMapping( "IParcelPackageEntity", "InsuranceValue", "InsuranceValue", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 10 );
			base.AddElementFieldMapping( "IParcelPackageEntity", "InsurancePennyOne", "InsurancePennyOne", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 11 );
			base.AddElementFieldMapping( "IParcelPackageEntity", "DeclaredValue", "DeclaredValue", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 12 );
			base.AddElementFieldMapping( "IParcelPackageEntity", "TrackingNumber", "TrackingNumber", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "IParcelPackageEntity", "ParcelNumber", "ParcelNumber", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "IParcelPackageEntity", "SkuAndQuantities", "SkuAndQuantities", false, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 15 );
		}
		/// <summary>Inits IParcelProfileEntity's mappings</summary>
		private void InitIParcelProfileEntityMappings()
		{
			base.AddElementMapping( "IParcelProfileEntity", "ShipWorksLocal", @"dbo", "iParcelProfile", 8 );
			base.AddElementFieldMapping( "IParcelProfileEntity", "ShippingProfileID", "ShippingProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "IParcelProfileEntity", "IParcelAccountID", "iParcelAccountID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "IParcelProfileEntity", "Service", "Service", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "IParcelProfileEntity", "Reference", "Reference", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "IParcelProfileEntity", "TrackByEmail", "TrackByEmail", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 4 );
			base.AddElementFieldMapping( "IParcelProfileEntity", "TrackBySMS", "TrackBySMS", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 5 );
			base.AddElementFieldMapping( "IParcelProfileEntity", "IsDeliveryDutyPaid", "IsDeliveryDutyPaid", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 6 );
			base.AddElementFieldMapping( "IParcelProfileEntity", "SkuAndQuantities", "SkuAndQuantities", true, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 7 );
		}
		/// <summary>Inits IParcelProfilePackageEntity's mappings</summary>
		private void InitIParcelProfilePackageEntityMappings()
		{
			base.AddElementMapping( "IParcelProfilePackageEntity", "ShipWorksLocal", @"dbo", "iParcelProfilePackage", 9 );
			base.AddElementFieldMapping( "IParcelProfilePackageEntity", "IParcelProfilePackageID", "iParcelProfilePackageID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "IParcelProfilePackageEntity", "ShippingProfileID", "ShippingProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "IParcelProfilePackageEntity", "Weight", "Weight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 2 );
			base.AddElementFieldMapping( "IParcelProfilePackageEntity", "DimsProfileID", "DimsProfileID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "IParcelProfilePackageEntity", "DimsLength", "DimsLength", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 4 );
			base.AddElementFieldMapping( "IParcelProfilePackageEntity", "DimsWidth", "DimsWidth", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 5 );
			base.AddElementFieldMapping( "IParcelProfilePackageEntity", "DimsHeight", "DimsHeight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 6 );
			base.AddElementFieldMapping( "IParcelProfilePackageEntity", "DimsWeight", "DimsWeight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 7 );
			base.AddElementFieldMapping( "IParcelProfilePackageEntity", "DimsAddWeight", "DimsAddWeight", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 8 );
		}
		/// <summary>Inits IParcelShipmentEntity's mappings</summary>
		private void InitIParcelShipmentEntityMappings()
		{
			base.AddElementMapping( "IParcelShipmentEntity", "ShipWorksLocal", @"dbo", "iParcelShipment", 8 );
			base.AddElementFieldMapping( "IParcelShipmentEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "IParcelShipmentEntity", "IParcelAccountID", "iParcelAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "IParcelShipmentEntity", "Service", "Service", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "IParcelShipmentEntity", "Reference", "Reference", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "IParcelShipmentEntity", "TrackByEmail", "TrackByEmail", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 4 );
			base.AddElementFieldMapping( "IParcelShipmentEntity", "TrackBySMS", "TrackBySMS", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 5 );
			base.AddElementFieldMapping( "IParcelShipmentEntity", "IsDeliveryDutyPaid", "IsDeliveryDutyPaid", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 6 );
			base.AddElementFieldMapping( "IParcelShipmentEntity", "RequestedLabelFormat", "RequestedLabelFormat", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 7 );
		}
		/// <summary>Inits LabelSheetEntity's mappings</summary>
		private void InitLabelSheetEntityMappings()
		{
			base.AddElementMapping( "LabelSheetEntity", "ShipWorksLocal", @"dbo", "LabelSheet", 13 );
			base.AddElementFieldMapping( "LabelSheetEntity", "LabelSheetID", "LabelSheetID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "LabelSheetEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "LabelSheetEntity", "Name", "Name", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "LabelSheetEntity", "PaperSizeHeight", "PaperSizeHeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 3 );
			base.AddElementFieldMapping( "LabelSheetEntity", "PaperSizeWidth", "PaperSizeWidth", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 4 );
			base.AddElementFieldMapping( "LabelSheetEntity", "MarginTop", "MarginTop", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 5 );
			base.AddElementFieldMapping( "LabelSheetEntity", "MarginLeft", "MarginLeft", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 6 );
			base.AddElementFieldMapping( "LabelSheetEntity", "LabelHeight", "LabelHeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 7 );
			base.AddElementFieldMapping( "LabelSheetEntity", "LabelWidth", "LabelWidth", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 8 );
			base.AddElementFieldMapping( "LabelSheetEntity", "VerticalSpacing", "VerticalSpacing", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 9 );
			base.AddElementFieldMapping( "LabelSheetEntity", "HorizontalSpacing", "HorizontalSpacing", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 10 );
			base.AddElementFieldMapping( "LabelSheetEntity", "Rows", "Rows", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 11 );
			base.AddElementFieldMapping( "LabelSheetEntity", "Columns", "Columns", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 12 );
		}
		/// <summary>Inits MagentoOrderEntity's mappings</summary>
		private void InitMagentoOrderEntityMappings()
		{
			base.AddElementMapping( "MagentoOrderEntity", "ShipWorksLocal", @"dbo", "MagentoOrder", 2 );
			base.AddElementFieldMapping( "MagentoOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "MagentoOrderEntity", "MagentoOrderID", "MagentoOrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
		}
		/// <summary>Inits MagentoStoreEntity's mappings</summary>
		private void InitMagentoStoreEntityMappings()
		{
			base.AddElementMapping( "MagentoStoreEntity", "ShipWorksLocal", @"dbo", "MagentoStore", 3 );
			base.AddElementFieldMapping( "MagentoStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "MagentoStoreEntity", "MagentoTrackingEmails", "MagentoTrackingEmails", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 1 );
			base.AddElementFieldMapping( "MagentoStoreEntity", "MagentoConnect", "MagentoConnect", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 2 );
		}
		/// <summary>Inits MarketplaceAdvisorOrderEntity's mappings</summary>
		private void InitMarketplaceAdvisorOrderEntityMappings()
		{
			base.AddElementMapping( "MarketplaceAdvisorOrderEntity", "ShipWorksLocal", @"dbo", "MarketplaceAdvisorOrder", 5 );
			base.AddElementFieldMapping( "MarketplaceAdvisorOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "MarketplaceAdvisorOrderEntity", "BuyerNumber", "BuyerNumber", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "MarketplaceAdvisorOrderEntity", "SellerOrderNumber", "SellerOrderNumber", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "MarketplaceAdvisorOrderEntity", "InvoiceNumber", "InvoiceNumber", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "MarketplaceAdvisorOrderEntity", "ParcelID", "ParcelID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
		}
		/// <summary>Inits MarketplaceAdvisorStoreEntity's mappings</summary>
		private void InitMarketplaceAdvisorStoreEntityMappings()
		{
			base.AddElementMapping( "MarketplaceAdvisorStoreEntity", "ShipWorksLocal", @"dbo", "MarketplaceAdvisorStore", 5 );
			base.AddElementFieldMapping( "MarketplaceAdvisorStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "MarketplaceAdvisorStoreEntity", "Username", "Username", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "MarketplaceAdvisorStoreEntity", "Password", "Password", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "MarketplaceAdvisorStoreEntity", "AccountType", "AccountType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "MarketplaceAdvisorStoreEntity", "DownloadFlags", "DownloadFlags", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
		}
		/// <summary>Inits MivaOrderItemAttributeEntity's mappings</summary>
		private void InitMivaOrderItemAttributeEntityMappings()
		{
			base.AddElementMapping( "MivaOrderItemAttributeEntity", "ShipWorksLocal", @"dbo", "MivaOrderItemAttribute", 4 );
			base.AddElementFieldMapping( "MivaOrderItemAttributeEntity", "OrderItemAttributeID", "OrderItemAttributeID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "MivaOrderItemAttributeEntity", "MivaOptionCode", "MivaOptionCode", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "MivaOrderItemAttributeEntity", "MivaAttributeID", "MivaAttributeID", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "MivaOrderItemAttributeEntity", "MivaAttributeCode", "MivaAttributeCode", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 3 );
		}
		/// <summary>Inits MivaStoreEntity's mappings</summary>
		private void InitMivaStoreEntityMappings()
		{
			base.AddElementMapping( "MivaStoreEntity", "ShipWorksLocal", @"dbo", "MivaStore", 6 );
			base.AddElementFieldMapping( "MivaStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "MivaStoreEntity", "EncryptionPassphrase", "EncryptionPassphrase", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "MivaStoreEntity", "LiveManualOrderNumbers", "LiveManualOrderNumbers", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 2 );
			base.AddElementFieldMapping( "MivaStoreEntity", "SebenzaCheckoutDataEnabled", "SebenzaCheckoutDataEnabled", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "MivaStoreEntity", "OnlineUpdateStrategy", "OnlineUpdateStrategy", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "MivaStoreEntity", "OnlineUpdateStatusChangeEmail", "OnlineUpdateStatusChangeEmail", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 5 );
		}
		/// <summary>Inits NetworkSolutionsOrderEntity's mappings</summary>
		private void InitNetworkSolutionsOrderEntityMappings()
		{
			base.AddElementMapping( "NetworkSolutionsOrderEntity", "ShipWorksLocal", @"dbo", "NetworkSolutionsOrder", 2 );
			base.AddElementFieldMapping( "NetworkSolutionsOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "NetworkSolutionsOrderEntity", "NetworkSolutionsOrderID", "NetworkSolutionsOrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
		}
		/// <summary>Inits NetworkSolutionsStoreEntity's mappings</summary>
		private void InitNetworkSolutionsStoreEntityMappings()
		{
			base.AddElementMapping( "NetworkSolutionsStoreEntity", "ShipWorksLocal", @"dbo", "NetworkSolutionsStore", 5 );
			base.AddElementFieldMapping( "NetworkSolutionsStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "NetworkSolutionsStoreEntity", "UserToken", "UserToken", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "NetworkSolutionsStoreEntity", "DownloadOrderStatuses", "DownloadOrderStatuses", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "NetworkSolutionsStoreEntity", "StatusCodes", "StatusCodes", false, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "NetworkSolutionsStoreEntity", "StoreUrl", "StoreUrl", false, (int)SqlDbType.VarChar, 255, 0, 0, false, "", null, typeof(System.String), 4 );
		}
		/// <summary>Inits NeweggOrderEntity's mappings</summary>
		private void InitNeweggOrderEntityMappings()
		{
			base.AddElementMapping( "NeweggOrderEntity", "ShipWorksLocal", @"dbo", "NeweggOrder", 4 );
			base.AddElementFieldMapping( "NeweggOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "NeweggOrderEntity", "InvoiceNumber", "InvoiceNumber", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "NeweggOrderEntity", "RefundAmount", "RefundAmount", true, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 2 );
			base.AddElementFieldMapping( "NeweggOrderEntity", "IsAutoVoid", "IsAutoVoid", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
		}
		/// <summary>Inits NeweggOrderItemEntity's mappings</summary>
		private void InitNeweggOrderItemEntityMappings()
		{
			base.AddElementMapping( "NeweggOrderItemEntity", "ShipWorksLocal", @"dbo", "NeweggOrderItem", 7 );
			base.AddElementFieldMapping( "NeweggOrderItemEntity", "OrderItemID", "OrderItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "NeweggOrderItemEntity", "SellerPartNumber", "SellerPartNumber", true, (int)SqlDbType.VarChar, 64, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "NeweggOrderItemEntity", "NeweggItemNumber", "NeweggItemNumber", true, (int)SqlDbType.VarChar, 64, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "NeweggOrderItemEntity", "ManufacturerPartNumber", "ManufacturerPartNumber", true, (int)SqlDbType.VarChar, 64, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "NeweggOrderItemEntity", "ShippingStatusID", "ShippingStatusID", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "NeweggOrderItemEntity", "ShippingStatusDescription", "ShippingStatusDescription", true, (int)SqlDbType.VarChar, 32, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "NeweggOrderItemEntity", "QuantityShipped", "QuantityShipped", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 6 );
		}
		/// <summary>Inits NeweggStoreEntity's mappings</summary>
		private void InitNeweggStoreEntityMappings()
		{
			base.AddElementMapping( "NeweggStoreEntity", "ShipWorksLocal", @"dbo", "NeweggStore", 5 );
			base.AddElementFieldMapping( "NeweggStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "NeweggStoreEntity", "SellerID", "SellerID", false, (int)SqlDbType.VarChar, 10, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "NeweggStoreEntity", "SecretKey", "SecretKey", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "NeweggStoreEntity", "ExcludeFulfilledByNewegg", "ExcludeFulfilledByNewegg", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "NeweggStoreEntity", "Channel", "Channel", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
		}
		/// <summary>Inits NoteEntity's mappings</summary>
		private void InitNoteEntityMappings()
		{
			base.AddElementMapping( "NoteEntity", "ShipWorksLocal", @"dbo", "Note", 8 );
			base.AddElementFieldMapping( "NoteEntity", "NoteID", "NoteID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "NoteEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "NoteEntity", "ObjectID", "ObjectID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "NoteEntity", "UserID", "UserID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "NoteEntity", "Edited", "Edited", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 4 );
			base.AddElementFieldMapping( "NoteEntity", "Text", "Text", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "NoteEntity", "Source", "Source", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 6 );
			base.AddElementFieldMapping( "NoteEntity", "Visibility", "Visibility", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 7 );
		}
		/// <summary>Inits ObjectLabelEntity's mappings</summary>
		private void InitObjectLabelEntityMappings()
		{
			base.AddElementMapping( "ObjectLabelEntity", "ShipWorksLocal", @"dbo", "ObjectLabel", 6 );
			base.AddElementFieldMapping( "ObjectLabelEntity", "ObjectID", "ObjectID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ObjectLabelEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ObjectLabelEntity", "ObjectType", "ObjectType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "ObjectLabelEntity", "ParentID", "ParentID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "ObjectLabelEntity", "Label", "Label", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "ObjectLabelEntity", "IsDeleted", "IsDeleted", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 5 );
		}
		/// <summary>Inits ObjectReferenceEntity's mappings</summary>
		private void InitObjectReferenceEntityMappings()
		{
			base.AddElementMapping( "ObjectReferenceEntity", "ShipWorksLocal", @"dbo", "ObjectReference", 5 );
			base.AddElementFieldMapping( "ObjectReferenceEntity", "ObjectReferenceID", "ObjectReferenceID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ObjectReferenceEntity", "ConsumerID", "ConsumerID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "ObjectReferenceEntity", "ReferenceKey", "ReferenceKey", false, (int)SqlDbType.VarChar, 250, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ObjectReferenceEntity", "ObjectID", "ObjectID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "ObjectReferenceEntity", "Reason", "Reason", true, (int)SqlDbType.NVarChar, 250, 0, 0, false, "", null, typeof(System.String), 4 );
		}
		/// <summary>Inits OnTracAccountEntity's mappings</summary>
		private void InitOnTracAccountEntityMappings()
		{
			base.AddElementMapping( "OnTracAccountEntity", "ShipWorksLocal", @"dbo", "OnTracAccount", 16 );
			base.AddElementFieldMapping( "OnTracAccountEntity", "OnTracAccountID", "OnTracAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "OnTracAccountEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "OnTracAccountEntity", "AccountNumber", "AccountNumber", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "OnTracAccountEntity", "Password", "Password", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "OnTracAccountEntity", "Description", "Description", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "OnTracAccountEntity", "FirstName", "FirstName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "OnTracAccountEntity", "MiddleName", "MiddleName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "OnTracAccountEntity", "LastName", "LastName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "OnTracAccountEntity", "Company", "Company", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "OnTracAccountEntity", "Street1", "Street1", false, (int)SqlDbType.NVarChar, 43, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "OnTracAccountEntity", "City", "City", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "OnTracAccountEntity", "StateProvCode", "StateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "OnTracAccountEntity", "PostalCode", "PostalCode", false, (int)SqlDbType.NVarChar, 10, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "OnTracAccountEntity", "CountryCode", "CountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "OnTracAccountEntity", "Email", "Email", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "OnTracAccountEntity", "Phone", "Phone", false, (int)SqlDbType.NVarChar, 15, 0, 0, false, "", null, typeof(System.String), 15 );
		}
		/// <summary>Inits OnTracProfileEntity's mappings</summary>
		private void InitOnTracProfileEntityMappings()
		{
			base.AddElementMapping( "OnTracProfileEntity", "ShipWorksLocal", @"dbo", "OnTracProfile", 17 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "ShippingProfileID", "ShippingProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "OnTracAccountID", "OnTracAccountID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "ResidentialDetermination", "ResidentialDetermination", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "Service", "Service", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "SaturdayDelivery", "SaturdayDelivery", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 4 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "SignatureRequired", "SignatureRequired", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 5 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "PackagingType", "PackagingType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 6 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "Weight", "Weight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 7 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "DimsProfileID", "DimsProfileID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 8 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "DimsLength", "DimsLength", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 9 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "DimsWidth", "DimsWidth", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 10 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "DimsHeight", "DimsHeight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 11 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "DimsWeight", "DimsWeight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 12 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "DimsAddWeight", "DimsAddWeight", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 13 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "Reference1", "Reference1", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "Reference2", "Reference2", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "OnTracProfileEntity", "Instructions", "Instructions", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 16 );
		}
		/// <summary>Inits OnTracShipmentEntity's mappings</summary>
		private void InitOnTracShipmentEntityMappings()
		{
			base.AddElementMapping( "OnTracShipmentEntity", "ShipWorksLocal", @"dbo", "OnTracShipment", 22 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "OnTracAccountID", "OnTracAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "Service", "Service", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "IsCod", "IsCod", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "CodType", "CodType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "CodAmount", "CodAmount", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 5 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "SaturdayDelivery", "SaturdayDelivery", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 6 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "SignatureRequired", "SignatureRequired", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 7 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "PackagingType", "PackagingType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 8 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "Instructions", "Instructions", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "DimsProfileID", "DimsProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 10 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "DimsLength", "DimsLength", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 11 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "DimsWidth", "DimsWidth", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 12 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "DimsHeight", "DimsHeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 13 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "DimsWeight", "DimsWeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 14 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "DimsAddWeight", "DimsAddWeight", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 15 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "Reference1", "Reference1", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "Reference2", "Reference2", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 17 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "InsuranceValue", "InsuranceValue", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 18 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "InsurancePennyOne", "InsurancePennyOne", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 19 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "DeclaredValue", "DeclaredValue", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 20 );
			base.AddElementFieldMapping( "OnTracShipmentEntity", "RequestedLabelFormat", "RequestedLabelFormat", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 21 );
		}
		/// <summary>Inits OrderEntity's mappings</summary>
		private void InitOrderEntityMappings()
		{
			base.AddElementMapping( "OrderEntity", "ShipWorksLocal", @"dbo", "Order", 73 );
			base.AddElementFieldMapping( "OrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "OrderEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "OrderEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "OrderEntity", "CustomerID", "CustomerID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "OrderEntity", "OrderNumber", "OrderNumber", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
			base.AddElementFieldMapping( "OrderEntity", "OrderNumberComplete", "OrderNumberComplete", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "OrderEntity", "OrderDate", "OrderDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 6 );
			base.AddElementFieldMapping( "OrderEntity", "OrderTotal", "OrderTotal", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 7 );
			base.AddElementFieldMapping( "OrderEntity", "LocalStatus", "LocalStatus", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "OrderEntity", "IsManual", "IsManual", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 9 );
			base.AddElementFieldMapping( "OrderEntity", "OnlineLastModified", "OnlineLastModified", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 10 );
			base.AddElementFieldMapping( "OrderEntity", "OnlineCustomerID", "OnlineCustomerID", true, (int)SqlDbType.Variant, 0, 0, 0, false, "", null, typeof(System.Object), 11 );
			base.AddElementFieldMapping( "OrderEntity", "OnlineStatus", "OnlineStatus", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "OrderEntity", "OnlineStatusCode", "OnlineStatusCode", true, (int)SqlDbType.Variant, 0, 0, 0, false, "", null, typeof(System.Object), 13 );
			base.AddElementFieldMapping( "OrderEntity", "RequestedShipping", "RequestedShipping", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "OrderEntity", "BillFirstName", "BillFirstName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "OrderEntity", "BillMiddleName", "BillMiddleName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "OrderEntity", "BillLastName", "BillLastName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 17 );
			base.AddElementFieldMapping( "OrderEntity", "BillCompany", "BillCompany", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "OrderEntity", "BillStreet1", "BillStreet1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 19 );
			base.AddElementFieldMapping( "OrderEntity", "BillStreet2", "BillStreet2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 20 );
			base.AddElementFieldMapping( "OrderEntity", "BillStreet3", "BillStreet3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 21 );
			base.AddElementFieldMapping( "OrderEntity", "BillCity", "BillCity", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 22 );
			base.AddElementFieldMapping( "OrderEntity", "BillStateProvCode", "BillStateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 23 );
			base.AddElementFieldMapping( "OrderEntity", "BillPostalCode", "BillPostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 24 );
			base.AddElementFieldMapping( "OrderEntity", "BillCountryCode", "BillCountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 25 );
			base.AddElementFieldMapping( "OrderEntity", "BillPhone", "BillPhone", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 26 );
			base.AddElementFieldMapping( "OrderEntity", "BillFax", "BillFax", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 27 );
			base.AddElementFieldMapping( "OrderEntity", "BillEmail", "BillEmail", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 28 );
			base.AddElementFieldMapping( "OrderEntity", "BillWebsite", "BillWebsite", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 29 );
			base.AddElementFieldMapping( "OrderEntity", "BillAddressValidationSuggestionCount", "BillAddressValidationSuggestionCount", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 30 );
			base.AddElementFieldMapping( "OrderEntity", "BillAddressValidationStatus", "BillAddressValidationStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 31 );
			base.AddElementFieldMapping( "OrderEntity", "BillAddressValidationError", "BillAddressValidationError", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 32 );
			base.AddElementFieldMapping( "OrderEntity", "BillResidentialStatus", "BillResidentialStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 33 );
			base.AddElementFieldMapping( "OrderEntity", "BillPOBox", "BillPOBox", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 34 );
			base.AddElementFieldMapping( "OrderEntity", "BillUSTerritory", "BillUSTerritory", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 35 );
			base.AddElementFieldMapping( "OrderEntity", "BillMilitaryAddress", "BillMilitaryAddress", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 36 );
			base.AddElementFieldMapping( "OrderEntity", "ShipFirstName", "ShipFirstName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 37 );
			base.AddElementFieldMapping( "OrderEntity", "ShipMiddleName", "ShipMiddleName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 38 );
			base.AddElementFieldMapping( "OrderEntity", "ShipLastName", "ShipLastName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 39 );
			base.AddElementFieldMapping( "OrderEntity", "ShipCompany", "ShipCompany", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 40 );
			base.AddElementFieldMapping( "OrderEntity", "ShipStreet1", "ShipStreet1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 41 );
			base.AddElementFieldMapping( "OrderEntity", "ShipStreet2", "ShipStreet2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 42 );
			base.AddElementFieldMapping( "OrderEntity", "ShipStreet3", "ShipStreet3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 43 );
			base.AddElementFieldMapping( "OrderEntity", "ShipCity", "ShipCity", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 44 );
			base.AddElementFieldMapping( "OrderEntity", "ShipStateProvCode", "ShipStateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 45 );
			base.AddElementFieldMapping( "OrderEntity", "ShipPostalCode", "ShipPostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 46 );
			base.AddElementFieldMapping( "OrderEntity", "ShipCountryCode", "ShipCountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 47 );
			base.AddElementFieldMapping( "OrderEntity", "ShipPhone", "ShipPhone", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 48 );
			base.AddElementFieldMapping( "OrderEntity", "ShipFax", "ShipFax", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 49 );
			base.AddElementFieldMapping( "OrderEntity", "ShipEmail", "ShipEmail", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 50 );
			base.AddElementFieldMapping( "OrderEntity", "ShipWebsite", "ShipWebsite", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 51 );
			base.AddElementFieldMapping( "OrderEntity", "ShipAddressValidationSuggestionCount", "ShipAddressValidationSuggestionCount", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 52 );
			base.AddElementFieldMapping( "OrderEntity", "ShipAddressValidationStatus", "ShipAddressValidationStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 53 );
			base.AddElementFieldMapping( "OrderEntity", "ShipAddressValidationError", "ShipAddressValidationError", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 54 );
			base.AddElementFieldMapping( "OrderEntity", "ShipResidentialStatus", "ShipResidentialStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 55 );
			base.AddElementFieldMapping( "OrderEntity", "ShipPOBox", "ShipPOBox", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 56 );
			base.AddElementFieldMapping( "OrderEntity", "ShipUSTerritory", "ShipUSTerritory", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 57 );
			base.AddElementFieldMapping( "OrderEntity", "ShipMilitaryAddress", "ShipMilitaryAddress", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 58 );
			base.AddElementFieldMapping( "OrderEntity", "RollupItemCount", "RollupItemCount", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 59 );
			base.AddElementFieldMapping( "OrderEntity", "RollupItemName", "RollupItemName", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 60 );
			base.AddElementFieldMapping( "OrderEntity", "RollupItemCode", "RollupItemCode", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 61 );
			base.AddElementFieldMapping( "OrderEntity", "RollupItemSKU", "RollupItemSKU", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 62 );
			base.AddElementFieldMapping( "OrderEntity", "RollupItemLocation", "RollupItemLocation", true, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 63 );
			base.AddElementFieldMapping( "OrderEntity", "RollupItemQuantity", "RollupItemQuantity", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 64 );
			base.AddElementFieldMapping( "OrderEntity", "RollupItemTotalWeight", "RollupItemTotalWeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 65 );
			base.AddElementFieldMapping( "OrderEntity", "RollupNoteCount", "RollupNoteCount", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 66 );
			base.AddElementFieldMapping( "OrderEntity", "BillNameParseStatus", "BillNameParseStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 67 );
			base.AddElementFieldMapping( "OrderEntity", "BillUnparsedName", "BillUnparsedName", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 68 );
			base.AddElementFieldMapping( "OrderEntity", "ShipNameParseStatus", "ShipNameParseStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 69 );
			base.AddElementFieldMapping( "OrderEntity", "ShipUnparsedName", "ShipUnparsedName", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 70 );
			base.AddElementFieldMapping( "OrderEntity", "ShipSenseHashKey", "ShipSenseHashKey", false, (int)SqlDbType.NVarChar, 64, 0, 0, false, "", null, typeof(System.String), 71 );
			base.AddElementFieldMapping( "OrderEntity", "ShipSenseRecognitionStatus", "ShipSenseRecognitionStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 72 );
		}
		/// <summary>Inits OrderChargeEntity's mappings</summary>
		private void InitOrderChargeEntityMappings()
		{
			base.AddElementMapping( "OrderChargeEntity", "ShipWorksLocal", @"dbo", "OrderCharge", 6 );
			base.AddElementFieldMapping( "OrderChargeEntity", "OrderChargeID", "OrderChargeID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "OrderChargeEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "OrderChargeEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "OrderChargeEntity", "Type", "Type", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "OrderChargeEntity", "Description", "Description", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "OrderChargeEntity", "Amount", "Amount", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 5 );
		}
		/// <summary>Inits OrderItemEntity's mappings</summary>
		private void InitOrderItemEntityMappings()
		{
			base.AddElementMapping( "OrderItemEntity", "ShipWorksLocal", @"dbo", "OrderItem", 18 );
			base.AddElementFieldMapping( "OrderItemEntity", "OrderItemID", "OrderItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "OrderItemEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "OrderItemEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "OrderItemEntity", "Name", "Name", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "OrderItemEntity", "Code", "Code", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "OrderItemEntity", "SKU", "SKU", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "OrderItemEntity", "ISBN", "ISBN", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "OrderItemEntity", "UPC", "UPC", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "OrderItemEntity", "Description", "Description", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "OrderItemEntity", "Location", "Location", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "OrderItemEntity", "Image", "Image", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "OrderItemEntity", "Thumbnail", "Thumbnail", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "OrderItemEntity", "UnitPrice", "UnitPrice", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 12 );
			base.AddElementFieldMapping( "OrderItemEntity", "UnitCost", "UnitCost", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 13 );
			base.AddElementFieldMapping( "OrderItemEntity", "Weight", "Weight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 14 );
			base.AddElementFieldMapping( "OrderItemEntity", "Quantity", "Quantity", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 15 );
			base.AddElementFieldMapping( "OrderItemEntity", "LocalStatus", "LocalStatus", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "OrderItemEntity", "IsManual", "IsManual", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 17 );
		}
		/// <summary>Inits OrderItemAttributeEntity's mappings</summary>
		private void InitOrderItemAttributeEntityMappings()
		{
			base.AddElementMapping( "OrderItemAttributeEntity", "ShipWorksLocal", @"dbo", "OrderItemAttribute", 7 );
			base.AddElementFieldMapping( "OrderItemAttributeEntity", "OrderItemAttributeID", "OrderItemAttributeID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "OrderItemAttributeEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "OrderItemAttributeEntity", "OrderItemID", "OrderItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "OrderItemAttributeEntity", "Name", "Name", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "OrderItemAttributeEntity", "Description", "Description", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "OrderItemAttributeEntity", "UnitPrice", "UnitPrice", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 5 );
			base.AddElementFieldMapping( "OrderItemAttributeEntity", "IsManual", "IsManual", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 6 );
		}
		/// <summary>Inits OrderMotionOrderEntity's mappings</summary>
		private void InitOrderMotionOrderEntityMappings()
		{
			base.AddElementMapping( "OrderMotionOrderEntity", "ShipWorksLocal", @"dbo", "OrderMotionOrder", 4 );
			base.AddElementFieldMapping( "OrderMotionOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "OrderMotionOrderEntity", "OrderMotionShipmentID", "OrderMotionShipmentID", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 1 );
			base.AddElementFieldMapping( "OrderMotionOrderEntity", "OrderMotionPromotion", "OrderMotionPromotion", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "OrderMotionOrderEntity", "OrderMotionInvoiceNumber", "OrderMotionInvoiceNumber", false, (int)SqlDbType.NVarChar, 64, 0, 0, false, "", null, typeof(System.String), 3 );
		}
		/// <summary>Inits OrderMotionStoreEntity's mappings</summary>
		private void InitOrderMotionStoreEntityMappings()
		{
			base.AddElementMapping( "OrderMotionStoreEntity", "ShipWorksLocal", @"dbo", "OrderMotionStore", 3 );
			base.AddElementFieldMapping( "OrderMotionStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "OrderMotionStoreEntity", "OrderMotionEmailAccountID", "OrderMotionEmailAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "OrderMotionStoreEntity", "OrderMotionBizID", "OrderMotionBizID", false, (int)SqlDbType.Text, 2147483647, 0, 0, false, "", null, typeof(System.String), 2 );
		}
		/// <summary>Inits OrderPaymentDetailEntity's mappings</summary>
		private void InitOrderPaymentDetailEntityMappings()
		{
			base.AddElementMapping( "OrderPaymentDetailEntity", "ShipWorksLocal", @"dbo", "OrderPaymentDetail", 5 );
			base.AddElementFieldMapping( "OrderPaymentDetailEntity", "OrderPaymentDetailID", "OrderPaymentDetailID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "OrderPaymentDetailEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "OrderPaymentDetailEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "OrderPaymentDetailEntity", "Label", "Label", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "OrderPaymentDetailEntity", "Value", "Value", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 4 );
		}
		/// <summary>Inits OtherProfileEntity's mappings</summary>
		private void InitOtherProfileEntityMappings()
		{
			base.AddElementMapping( "OtherProfileEntity", "ShipWorksLocal", @"dbo", "OtherProfile", 3 );
			base.AddElementFieldMapping( "OtherProfileEntity", "ShippingProfileID", "ShippingProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "OtherProfileEntity", "Carrier", "Carrier", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "OtherProfileEntity", "Service", "Service", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
		}
		/// <summary>Inits OtherShipmentEntity's mappings</summary>
		private void InitOtherShipmentEntityMappings()
		{
			base.AddElementMapping( "OtherShipmentEntity", "ShipWorksLocal", @"dbo", "OtherShipment", 4 );
			base.AddElementFieldMapping( "OtherShipmentEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "OtherShipmentEntity", "Carrier", "Carrier", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "OtherShipmentEntity", "Service", "Service", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "OtherShipmentEntity", "InsuranceValue", "InsuranceValue", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 3 );
		}
		/// <summary>Inits PayPalOrderEntity's mappings</summary>
		private void InitPayPalOrderEntityMappings()
		{
			base.AddElementMapping( "PayPalOrderEntity", "ShipWorksLocal", @"dbo", "PayPalOrder", 5 );
			base.AddElementFieldMapping( "PayPalOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "PayPalOrderEntity", "TransactionID", "TransactionID", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "PayPalOrderEntity", "AddressStatus", "AddressStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "PayPalOrderEntity", "PayPalFee", "PayPalFee", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 3 );
			base.AddElementFieldMapping( "PayPalOrderEntity", "PaymentStatus", "PaymentStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
		}
		/// <summary>Inits PayPalStoreEntity's mappings</summary>
		private void InitPayPalStoreEntityMappings()
		{
			base.AddElementMapping( "PayPalStoreEntity", "ShipWorksLocal", @"dbo", "PayPalStore", 8 );
			base.AddElementFieldMapping( "PayPalStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "PayPalStoreEntity", "ApiCredentialType", "ApiCredentialType", false, (int)SqlDbType.SmallInt, 0, 0, 5, false, "", null, typeof(System.Int16), 1 );
			base.AddElementFieldMapping( "PayPalStoreEntity", "ApiUserName", "ApiUserName", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "PayPalStoreEntity", "ApiPassword", "ApiPassword", false, (int)SqlDbType.NVarChar, 80, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "PayPalStoreEntity", "ApiSignature", "ApiSignature", false, (int)SqlDbType.NVarChar, 80, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "PayPalStoreEntity", "ApiCertificate", "ApiCertificate", true, (int)SqlDbType.VarBinary, 2048, 0, 0, false, "", null, typeof(System.Byte[]), 5 );
			base.AddElementFieldMapping( "PayPalStoreEntity", "LastTransactionDate", "LastTransactionDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 6 );
			base.AddElementFieldMapping( "PayPalStoreEntity", "LastValidTransactionDate", "LastValidTransactionDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 7 );
		}
		/// <summary>Inits PermissionEntity's mappings</summary>
		private void InitPermissionEntityMappings()
		{
			base.AddElementMapping( "PermissionEntity", "ShipWorksLocal", @"dbo", "Permission", 4 );
			base.AddElementFieldMapping( "PermissionEntity", "PermissionID", "PermissionID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "PermissionEntity", "UserID", "UserID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "PermissionEntity", "PermissionType", "PermissionType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "PermissionEntity", "ObjectID", "ObjectID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
		}
		/// <summary>Inits PostalProfileEntity's mappings</summary>
		private void InitPostalProfileEntityMappings()
		{
			base.AddElementMapping( "PostalProfileEntity", "ShipWorksLocal", @"dbo", "PostalProfile", 22 );
			base.AddElementFieldMapping( "PostalProfileEntity", "ShippingProfileID", "ShippingProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "PostalProfileEntity", "Service", "Service", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 1 );
			base.AddElementFieldMapping( "PostalProfileEntity", "Confirmation", "Confirmation", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "PostalProfileEntity", "Weight", "Weight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 3 );
			base.AddElementFieldMapping( "PostalProfileEntity", "PackagingType", "PackagingType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "PostalProfileEntity", "DimsProfileID", "DimsProfileID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 5 );
			base.AddElementFieldMapping( "PostalProfileEntity", "DimsLength", "DimsLength", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 6 );
			base.AddElementFieldMapping( "PostalProfileEntity", "DimsWidth", "DimsWidth", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 7 );
			base.AddElementFieldMapping( "PostalProfileEntity", "DimsHeight", "DimsHeight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 8 );
			base.AddElementFieldMapping( "PostalProfileEntity", "DimsWeight", "DimsWeight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 9 );
			base.AddElementFieldMapping( "PostalProfileEntity", "DimsAddWeight", "DimsAddWeight", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 10 );
			base.AddElementFieldMapping( "PostalProfileEntity", "NonRectangular", "NonRectangular", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 11 );
			base.AddElementFieldMapping( "PostalProfileEntity", "NonMachinable", "NonMachinable", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 12 );
			base.AddElementFieldMapping( "PostalProfileEntity", "CustomsContentType", "CustomsContentType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 13 );
			base.AddElementFieldMapping( "PostalProfileEntity", "CustomsContentDescription", "CustomsContentDescription", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "PostalProfileEntity", "ExpressSignatureWaiver", "ExpressSignatureWaiver", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 15 );
			base.AddElementFieldMapping( "PostalProfileEntity", "SortType", "SortType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 16 );
			base.AddElementFieldMapping( "PostalProfileEntity", "EntryFacility", "EntryFacility", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 17 );
			base.AddElementFieldMapping( "PostalProfileEntity", "Memo1", "Memo1", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "PostalProfileEntity", "Memo2", "Memo2", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 19 );
			base.AddElementFieldMapping( "PostalProfileEntity", "Memo3", "Memo3", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 20 );
			base.AddElementFieldMapping( "PostalProfileEntity", "NoPostage", "NoPostage", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 21 );
		}
		/// <summary>Inits PostalShipmentEntity's mappings</summary>
		private void InitPostalShipmentEntityMappings()
		{
			base.AddElementMapping( "PostalShipmentEntity", "ShipWorksLocal", @"dbo", "PostalShipment", 22 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "Service", "Service", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 1 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "Confirmation", "Confirmation", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "PackagingType", "PackagingType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "DimsProfileID", "DimsProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "DimsLength", "DimsLength", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 5 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "DimsWidth", "DimsWidth", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 6 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "DimsHeight", "DimsHeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 7 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "DimsWeight", "DimsWeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 8 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "DimsAddWeight", "DimsAddWeight", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 9 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "NonRectangular", "NonRectangular", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 10 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "NonMachinable", "NonMachinable", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 11 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "CustomsContentType", "CustomsContentType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 12 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "CustomsContentDescription", "CustomsContentDescription", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "InsuranceValue", "InsuranceValue", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 14 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "ExpressSignatureWaiver", "ExpressSignatureWaiver", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 15 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "SortType", "SortType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 16 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "EntryFacility", "EntryFacility", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 17 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "Memo1", "Memo1", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "Memo2", "Memo2", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 19 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "Memo3", "Memo3", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 20 );
			base.AddElementFieldMapping( "PostalShipmentEntity", "NoPostage", "NoPostage", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 21 );
		}
		/// <summary>Inits PrintResultEntity's mappings</summary>
		private void InitPrintResultEntityMappings()
		{
			base.AddElementMapping( "PrintResultEntity", "ShipWorksLocal", @"dbo", "PrintResult", 23 );
			base.AddElementFieldMapping( "PrintResultEntity", "PrintResultID", "PrintResultID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "PrintResultEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "PrintResultEntity", "JobIdentifier", "JobIdentifier", false, (int)SqlDbType.UniqueIdentifier, 0, 0, 0, false, "", null, typeof(System.Guid), 2 );
			base.AddElementFieldMapping( "PrintResultEntity", "RelatedObjectID", "RelatedObjectID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "PrintResultEntity", "ContextObjectID", "ContextObjectID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
			base.AddElementFieldMapping( "PrintResultEntity", "TemplateID", "TemplateID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 5 );
			base.AddElementFieldMapping( "PrintResultEntity", "TemplateType", "TemplateType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 6 );
			base.AddElementFieldMapping( "PrintResultEntity", "OutputFormat", "OutputFormat", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 7 );
			base.AddElementFieldMapping( "PrintResultEntity", "LabelSheetID", "LabelSheetID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 8 );
			base.AddElementFieldMapping( "PrintResultEntity", "ComputerID", "ComputerID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 9 );
			base.AddElementFieldMapping( "PrintResultEntity", "ContentResourceID", "ContentResourceID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 10 );
			base.AddElementFieldMapping( "PrintResultEntity", "PrintDate", "PrintDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 11 );
			base.AddElementFieldMapping( "PrintResultEntity", "PrinterName", "PrinterName", false, (int)SqlDbType.NVarChar, 350, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "PrintResultEntity", "PaperSource", "PaperSource", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 13 );
			base.AddElementFieldMapping( "PrintResultEntity", "PaperSourceName", "PaperSourceName", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "PrintResultEntity", "Copies", "Copies", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 15 );
			base.AddElementFieldMapping( "PrintResultEntity", "Collated", "Collated", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 16 );
			base.AddElementFieldMapping( "PrintResultEntity", "PageMarginLeft", "PageMarginLeft", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 17 );
			base.AddElementFieldMapping( "PrintResultEntity", "PageMarginRight", "PageMarginRight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 18 );
			base.AddElementFieldMapping( "PrintResultEntity", "PageMarginBottom", "PageMarginBottom", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 19 );
			base.AddElementFieldMapping( "PrintResultEntity", "PageMarginTop", "PageMarginTop", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 20 );
			base.AddElementFieldMapping( "PrintResultEntity", "PageWidth", "PageWidth", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 21 );
			base.AddElementFieldMapping( "PrintResultEntity", "PageHeight", "PageHeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 22 );
		}
		/// <summary>Inits ProStoresOrderEntity's mappings</summary>
		private void InitProStoresOrderEntityMappings()
		{
			base.AddElementMapping( "ProStoresOrderEntity", "ShipWorksLocal", @"dbo", "ProStoresOrder", 4 );
			base.AddElementFieldMapping( "ProStoresOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ProStoresOrderEntity", "ConfirmationNumber", "ConfirmationNumber", false, (int)SqlDbType.VarChar, 12, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "ProStoresOrderEntity", "AuthorizedDate", "AuthorizedDate", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 2 );
			base.AddElementFieldMapping( "ProStoresOrderEntity", "AuthorizedBy", "AuthorizedBy", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
		}
		/// <summary>Inits ProStoresStoreEntity's mappings</summary>
		private void InitProStoresStoreEntityMappings()
		{
			base.AddElementMapping( "ProStoresStoreEntity", "ShipWorksLocal", @"dbo", "ProStoresStore", 17 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "ShortName", "ShortName", false, (int)SqlDbType.VarChar, 30, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "Username", "Username", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "LoginMethod", "LoginMethod", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "ApiEntryPoint", "ApiEntryPoint", false, (int)SqlDbType.VarChar, 300, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "ApiToken", "ApiToken", false, (int)SqlDbType.Text, 2147483647, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "ApiStorefrontUrl", "ApiStorefrontUrl", false, (int)SqlDbType.VarChar, 300, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "ApiTokenLogonUrl", "ApiTokenLogonUrl", false, (int)SqlDbType.VarChar, 300, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "ApiXteUrl", "ApiXteUrl", false, (int)SqlDbType.VarChar, 300, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "ApiRestSecureUrl", "ApiRestSecureUrl", false, (int)SqlDbType.VarChar, 300, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "ApiRestNonSecureUrl", "ApiRestNonSecureUrl", false, (int)SqlDbType.VarChar, 300, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "ApiRestScriptSuffix", "ApiRestScriptSuffix", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "LegacyAdminUrl", "LegacyAdminUrl", false, (int)SqlDbType.VarChar, 300, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "LegacyXtePath", "LegacyXtePath", false, (int)SqlDbType.VarChar, 75, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "LegacyPrefix", "LegacyPrefix", false, (int)SqlDbType.VarChar, 30, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "LegacyPassword", "LegacyPassword", false, (int)SqlDbType.VarChar, 150, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "ProStoresStoreEntity", "LegacyCanUpgrade", "LegacyCanUpgrade", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 16 );
		}
		/// <summary>Inits ResourceEntity's mappings</summary>
		private void InitResourceEntityMappings()
		{
			base.AddElementMapping( "ResourceEntity", "ShipWorksLocal", @"dbo", "Resource", 5 );
			base.AddElementFieldMapping( "ResourceEntity", "ResourceID", "ResourceID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ResourceEntity", "Data", "Data", false, (int)SqlDbType.VarBinary, 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ResourceEntity", "Checksum", "Checksum", false, (int)SqlDbType.Binary, 32, 0, 0, false, "", null, typeof(System.Byte[]), 2 );
			base.AddElementFieldMapping( "ResourceEntity", "Compressed", "Compressed", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "ResourceEntity", "Filename", "Filename", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 4 );
		}
		/// <summary>Inits ScanFormBatchEntity's mappings</summary>
		private void InitScanFormBatchEntityMappings()
		{
			base.AddElementMapping( "ScanFormBatchEntity", "ShipWorksLocal", @"dbo", "ScanFormBatch", 4 );
			base.AddElementFieldMapping( "ScanFormBatchEntity", "ScanFormBatchID", "ScanFormBatchID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ScanFormBatchEntity", "ShipmentType", "ShipmentType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 1 );
			base.AddElementFieldMapping( "ScanFormBatchEntity", "CreatedDate", "CreatedDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 2 );
			base.AddElementFieldMapping( "ScanFormBatchEntity", "ShipmentCount", "ShipmentCount", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
		}
		/// <summary>Inits SearchEntity's mappings</summary>
		private void InitSearchEntityMappings()
		{
			base.AddElementMapping( "SearchEntity", "ShipWorksLocal", @"dbo", "Search", 6 );
			base.AddElementFieldMapping( "SearchEntity", "SearchID", "SearchID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "SearchEntity", "Started", "Started", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 1 );
			base.AddElementFieldMapping( "SearchEntity", "Pinged", "Pinged", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 2 );
			base.AddElementFieldMapping( "SearchEntity", "FilterNodeID", "FilterNodeID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "SearchEntity", "UserID", "UserID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
			base.AddElementFieldMapping( "SearchEntity", "ComputerID", "ComputerID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 5 );
		}
		/// <summary>Inits SearsOrderEntity's mappings</summary>
		private void InitSearsOrderEntityMappings()
		{
			base.AddElementMapping( "SearsOrderEntity", "ShipWorksLocal", @"dbo", "SearsOrder", 6 );
			base.AddElementFieldMapping( "SearsOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "SearsOrderEntity", "PoNumber", "PoNumber", false, (int)SqlDbType.VarChar, 30, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "SearsOrderEntity", "PoNumberWithDate", "PoNumberWithDate", false, (int)SqlDbType.VarChar, 30, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "SearsOrderEntity", "LocationID", "LocationID", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "SearsOrderEntity", "Commission", "Commission", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 4 );
			base.AddElementFieldMapping( "SearsOrderEntity", "CustomerPickup", "CustomerPickup", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 5 );
		}
		/// <summary>Inits SearsOrderItemEntity's mappings</summary>
		private void InitSearsOrderItemEntityMappings()
		{
			base.AddElementMapping( "SearsOrderItemEntity", "ShipWorksLocal", @"dbo", "SearsOrderItem", 6 );
			base.AddElementFieldMapping( "SearsOrderItemEntity", "OrderItemID", "OrderItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "SearsOrderItemEntity", "LineNumber", "LineNumber", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 1 );
			base.AddElementFieldMapping( "SearsOrderItemEntity", "ItemID", "ItemID", false, (int)SqlDbType.VarChar, 300, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "SearsOrderItemEntity", "Commission", "Commission", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 3 );
			base.AddElementFieldMapping( "SearsOrderItemEntity", "Shipping", "Shipping", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 4 );
			base.AddElementFieldMapping( "SearsOrderItemEntity", "OnlineStatus", "OnlineStatus", false, (int)SqlDbType.VarChar, 20, 0, 0, false, "", null, typeof(System.String), 5 );
		}
		/// <summary>Inits SearsStoreEntity's mappings</summary>
		private void InitSearsStoreEntityMappings()
		{
			base.AddElementMapping( "SearsStoreEntity", "ShipWorksLocal", @"dbo", "SearsStore", 3 );
			base.AddElementFieldMapping( "SearsStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "SearsStoreEntity", "Email", "Email", false, (int)SqlDbType.NVarChar, 75, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "SearsStoreEntity", "Password", "Password", false, (int)SqlDbType.NVarChar, 75, 0, 0, false, "", null, typeof(System.String), 2 );
		}
		/// <summary>Inits ServerMessageEntity's mappings</summary>
		private void InitServerMessageEntityMappings()
		{
			base.AddElementMapping( "ServerMessageEntity", "ShipWorksLocal", @"dbo", "ServerMessage", 16 );
			base.AddElementFieldMapping( "ServerMessageEntity", "ServerMessageID", "ServerMessageID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ServerMessageEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ServerMessageEntity", "Number", "Number", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "ServerMessageEntity", "Published", "Published", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 3 );
			base.AddElementFieldMapping( "ServerMessageEntity", "Active", "Active", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 4 );
			base.AddElementFieldMapping( "ServerMessageEntity", "Dismissable", "Dismissable", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 5 );
			base.AddElementFieldMapping( "ServerMessageEntity", "Expires", "Expires", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 6 );
			base.AddElementFieldMapping( "ServerMessageEntity", "ResponseTo", "ResponseTo", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 7 );
			base.AddElementFieldMapping( "ServerMessageEntity", "ResponseAction", "ResponseAction", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 8 );
			base.AddElementFieldMapping( "ServerMessageEntity", "EditTo", "EditTo", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 9 );
			base.AddElementFieldMapping( "ServerMessageEntity", "Image", "Image", false, (int)SqlDbType.NVarChar, 350, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "ServerMessageEntity", "PrimaryText", "PrimaryText", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "ServerMessageEntity", "SecondaryText", "SecondaryText", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "ServerMessageEntity", "Actions", "Actions", false, (int)SqlDbType.NText, 1073741823, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "ServerMessageEntity", "Stores", "Stores", false, (int)SqlDbType.NText, 1073741823, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "ServerMessageEntity", "Shippers", "Shippers", false, (int)SqlDbType.NText, 1073741823, 0, 0, false, "", null, typeof(System.String), 15 );
		}
		/// <summary>Inits ServerMessageSignoffEntity's mappings</summary>
		private void InitServerMessageSignoffEntityMappings()
		{
			base.AddElementMapping( "ServerMessageSignoffEntity", "ShipWorksLocal", @"dbo", "ServerMessageSignoff", 6 );
			base.AddElementFieldMapping( "ServerMessageSignoffEntity", "ServerMessageSignoffID", "ServerMessageSignoffID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ServerMessageSignoffEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ServerMessageSignoffEntity", "ServerMessageID", "ServerMessageID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "ServerMessageSignoffEntity", "UserID", "UserID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "ServerMessageSignoffEntity", "ComputerID", "ComputerID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
			base.AddElementFieldMapping( "ServerMessageSignoffEntity", "Dismissed", "Dismissed", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 5 );
		}
		/// <summary>Inits ServiceStatusEntity's mappings</summary>
		private void InitServiceStatusEntityMappings()
		{
			base.AddElementMapping( "ServiceStatusEntity", "ShipWorksLocal", @"dbo", "ServiceStatus", 9 );
			base.AddElementFieldMapping( "ServiceStatusEntity", "ServiceStatusID", "ServiceStatusID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ServiceStatusEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ServiceStatusEntity", "ComputerID", "ComputerID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "ServiceStatusEntity", "ServiceType", "ServiceType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "ServiceStatusEntity", "LastStartDateTime", "LastStartDateTime", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 4 );
			base.AddElementFieldMapping( "ServiceStatusEntity", "LastStopDateTime", "LastStopDateTime", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 5 );
			base.AddElementFieldMapping( "ServiceStatusEntity", "LastCheckInDateTime", "LastCheckInDateTime", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 6 );
			base.AddElementFieldMapping( "ServiceStatusEntity", "ServiceFullName", "ServiceFullName", false, (int)SqlDbType.NVarChar, 256, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "ServiceStatusEntity", "ServiceDisplayName", "ServiceDisplayName", false, (int)SqlDbType.NVarChar, 256, 0, 0, false, "", null, typeof(System.String), 8 );
		}
		/// <summary>Inits ShipmentEntity's mappings</summary>
		private void InitShipmentEntityMappings()
		{
			base.AddElementMapping( "ShipmentEntity", "ShipWorksLocal", @"dbo", "Shipment", 73 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ShipmentEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ShipmentEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipmentType", "ShipmentType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "ShipmentEntity", "ContentWeight", "ContentWeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 4 );
			base.AddElementFieldMapping( "ShipmentEntity", "TotalWeight", "TotalWeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 5 );
			base.AddElementFieldMapping( "ShipmentEntity", "Processed", "Processed", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 6 );
			base.AddElementFieldMapping( "ShipmentEntity", "ProcessedDate", "ProcessedDate", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 7 );
			base.AddElementFieldMapping( "ShipmentEntity", "ProcessedUserID", "ProcessedUserID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 8 );
			base.AddElementFieldMapping( "ShipmentEntity", "ProcessedComputerID", "ProcessedComputerID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 9 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipDate", "ShipDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 10 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipmentCost", "ShipmentCost", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 11 );
			base.AddElementFieldMapping( "ShipmentEntity", "Voided", "Voided", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 12 );
			base.AddElementFieldMapping( "ShipmentEntity", "VoidedDate", "VoidedDate", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 13 );
			base.AddElementFieldMapping( "ShipmentEntity", "VoidedUserID", "VoidedUserID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 14 );
			base.AddElementFieldMapping( "ShipmentEntity", "VoidedComputerID", "VoidedComputerID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 15 );
			base.AddElementFieldMapping( "ShipmentEntity", "TrackingNumber", "TrackingNumber", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "ShipmentEntity", "CustomsGenerated", "CustomsGenerated", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 17 );
			base.AddElementFieldMapping( "ShipmentEntity", "CustomsValue", "CustomsValue", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 18 );
			base.AddElementFieldMapping( "ShipmentEntity", "RequestedLabelFormat", "RequestedLabelFormat", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 19 );
			base.AddElementFieldMapping( "ShipmentEntity", "ActualLabelFormat", "ActualLabelFormat", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 20 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipFirstName", "ShipFirstName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 21 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipMiddleName", "ShipMiddleName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 22 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipLastName", "ShipLastName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 23 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipCompany", "ShipCompany", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 24 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipStreet1", "ShipStreet1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 25 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipStreet2", "ShipStreet2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 26 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipStreet3", "ShipStreet3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 27 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipCity", "ShipCity", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 28 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipStateProvCode", "ShipStateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 29 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipPostalCode", "ShipPostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 30 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipCountryCode", "ShipCountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 31 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipPhone", "ShipPhone", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 32 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipEmail", "ShipEmail", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 33 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipAddressValidationSuggestionCount", "ShipAddressValidationSuggestionCount", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 34 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipAddressValidationStatus", "ShipAddressValidationStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 35 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipAddressValidationError", "ShipAddressValidationError", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 36 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipResidentialStatus", "ShipResidentialStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 37 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipPOBox", "ShipPOBox", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 38 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipUSTerritory", "ShipUSTerritory", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 39 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipMilitaryAddress", "ShipMilitaryAddress", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 40 );
			base.AddElementFieldMapping( "ShipmentEntity", "ResidentialDetermination", "ResidentialDetermination", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 41 );
			base.AddElementFieldMapping( "ShipmentEntity", "ResidentialResult", "ResidentialResult", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 42 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginOriginID", "OriginOriginID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 43 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginFirstName", "OriginFirstName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 44 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginMiddleName", "OriginMiddleName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 45 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginLastName", "OriginLastName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 46 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginCompany", "OriginCompany", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 47 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginStreet1", "OriginStreet1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 48 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginStreet2", "OriginStreet2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 49 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginStreet3", "OriginStreet3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 50 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginCity", "OriginCity", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 51 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginStateProvCode", "OriginStateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 52 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginPostalCode", "OriginPostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 53 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginCountryCode", "OriginCountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 54 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginPhone", "OriginPhone", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 55 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginFax", "OriginFax", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 56 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginEmail", "OriginEmail", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 57 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginWebsite", "OriginWebsite", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 58 );
			base.AddElementFieldMapping( "ShipmentEntity", "ReturnShipment", "ReturnShipment", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 59 );
			base.AddElementFieldMapping( "ShipmentEntity", "Insurance", "Insurance", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 60 );
			base.AddElementFieldMapping( "ShipmentEntity", "InsuranceProvider", "InsuranceProvider", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 61 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipNameParseStatus", "ShipNameParseStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 62 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipUnparsedName", "ShipUnparsedName", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 63 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginNameParseStatus", "OriginNameParseStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 64 );
			base.AddElementFieldMapping( "ShipmentEntity", "OriginUnparsedName", "OriginUnparsedName", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 65 );
			base.AddElementFieldMapping( "ShipmentEntity", "BestRateEvents", "BestRateEvents", false, (int)SqlDbType.TinyInt, 0, 0, 3, false, "", null, typeof(System.Byte), 66 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipSenseStatus", "ShipSenseStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 67 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipSenseChangeSets", "ShipSenseChangeSets", false, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 68 );
			base.AddElementFieldMapping( "ShipmentEntity", "ShipSenseEntry", "ShipSenseEntry", false, (int)SqlDbType.VarBinary, 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 69 );
			base.AddElementFieldMapping( "ShipmentEntity", "OnlineShipmentID", "OnlineShipmentID", false, (int)SqlDbType.VarChar, 128, 0, 0, false, "", null, typeof(System.String), 70 );
			base.AddElementFieldMapping( "ShipmentEntity", "BilledType", "BilledType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 71 );
			base.AddElementFieldMapping( "ShipmentEntity", "BilledWeight", "BilledWeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 72 );
		}
		/// <summary>Inits ShipmentCustomsItemEntity's mappings</summary>
		private void InitShipmentCustomsItemEntityMappings()
		{
			base.AddElementMapping( "ShipmentCustomsItemEntity", "ShipWorksLocal", @"dbo", "ShipmentCustomsItem", 11 );
			base.AddElementFieldMapping( "ShipmentCustomsItemEntity", "ShipmentCustomsItemID", "ShipmentCustomsItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ShipmentCustomsItemEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ShipmentCustomsItemEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "ShipmentCustomsItemEntity", "Description", "Description", false, (int)SqlDbType.NVarChar, 150, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "ShipmentCustomsItemEntity", "Quantity", "Quantity", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 4 );
			base.AddElementFieldMapping( "ShipmentCustomsItemEntity", "Weight", "Weight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 5 );
			base.AddElementFieldMapping( "ShipmentCustomsItemEntity", "UnitValue", "UnitValue", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 6 );
			base.AddElementFieldMapping( "ShipmentCustomsItemEntity", "CountryOfOrigin", "CountryOfOrigin", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "ShipmentCustomsItemEntity", "HarmonizedCode", "HarmonizedCode", false, (int)SqlDbType.VarChar, 14, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "ShipmentCustomsItemEntity", "NumberOfPieces", "NumberOfPieces", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 9 );
			base.AddElementFieldMapping( "ShipmentCustomsItemEntity", "UnitPriceAmount", "UnitPriceAmount", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 10 );
		}
		/// <summary>Inits ShippingDefaultsRuleEntity's mappings</summary>
		private void InitShippingDefaultsRuleEntityMappings()
		{
			base.AddElementMapping( "ShippingDefaultsRuleEntity", "ShipWorksLocal", @"dbo", "ShippingDefaultsRule", 6 );
			base.AddElementFieldMapping( "ShippingDefaultsRuleEntity", "ShippingDefaultsRuleID", "ShippingDefaultsRuleID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ShippingDefaultsRuleEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ShippingDefaultsRuleEntity", "ShipmentType", "ShipmentType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "ShippingDefaultsRuleEntity", "FilterNodeID", "FilterNodeID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
			base.AddElementFieldMapping( "ShippingDefaultsRuleEntity", "ShippingProfileID", "ShippingProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
			base.AddElementFieldMapping( "ShippingDefaultsRuleEntity", "Position", "Position", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
		}
		/// <summary>Inits ShippingOriginEntity's mappings</summary>
		private void InitShippingOriginEntityMappings()
		{
			base.AddElementMapping( "ShippingOriginEntity", "ShipWorksLocal", @"dbo", "ShippingOrigin", 18 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "ShippingOriginID", "ShippingOriginID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "Description", "Description", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "FirstName", "FirstName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "MiddleName", "MiddleName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "LastName", "LastName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "Company", "Company", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "Street1", "Street1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "Street2", "Street2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "Street3", "Street3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "City", "City", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "StateProvCode", "StateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "PostalCode", "PostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "CountryCode", "CountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "Phone", "Phone", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "Fax", "Fax", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "Email", "Email", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "ShippingOriginEntity", "Website", "Website", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 17 );
		}
		/// <summary>Inits ShippingPrintOutputEntity's mappings</summary>
		private void InitShippingPrintOutputEntityMappings()
		{
			base.AddElementMapping( "ShippingPrintOutputEntity", "ShipWorksLocal", @"dbo", "ShippingPrintOutput", 4 );
			base.AddElementFieldMapping( "ShippingPrintOutputEntity", "ShippingPrintOutputID", "ShippingPrintOutputID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ShippingPrintOutputEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ShippingPrintOutputEntity", "ShipmentType", "ShipmentType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "ShippingPrintOutputEntity", "Name", "Name", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
		}
		/// <summary>Inits ShippingPrintOutputRuleEntity's mappings</summary>
		private void InitShippingPrintOutputRuleEntityMappings()
		{
			base.AddElementMapping( "ShippingPrintOutputRuleEntity", "ShipWorksLocal", @"dbo", "ShippingPrintOutputRule", 4 );
			base.AddElementFieldMapping( "ShippingPrintOutputRuleEntity", "ShippingPrintOutputRuleID", "ShippingPrintOutputRuleID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ShippingPrintOutputRuleEntity", "ShippingPrintOutputID", "ShippingPrintOutputID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "ShippingPrintOutputRuleEntity", "FilterNodeID", "FilterNodeID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "ShippingPrintOutputRuleEntity", "TemplateID", "TemplateID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
		}
		/// <summary>Inits ShippingProfileEntity's mappings</summary>
		private void InitShippingProfileEntityMappings()
		{
			base.AddElementMapping( "ShippingProfileEntity", "ShipWorksLocal", @"dbo", "ShippingProfile", 11 );
			base.AddElementFieldMapping( "ShippingProfileEntity", "ShippingProfileID", "ShippingProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ShippingProfileEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ShippingProfileEntity", "Name", "Name", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ShippingProfileEntity", "ShipmentType", "ShipmentType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "ShippingProfileEntity", "ShipmentTypePrimary", "ShipmentTypePrimary", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 4 );
			base.AddElementFieldMapping( "ShippingProfileEntity", "OriginID", "OriginID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 5 );
			base.AddElementFieldMapping( "ShippingProfileEntity", "Insurance", "Insurance", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 6 );
			base.AddElementFieldMapping( "ShippingProfileEntity", "InsuranceInitialValueSource", "InsuranceInitialValueSource", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 7 );
			base.AddElementFieldMapping( "ShippingProfileEntity", "InsuranceInitialValueAmount", "InsuranceInitialValueAmount", true, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 8 );
			base.AddElementFieldMapping( "ShippingProfileEntity", "ReturnShipment", "ReturnShipment", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 9 );
			base.AddElementFieldMapping( "ShippingProfileEntity", "RequestedLabelFormat", "RequestedLabelFormat", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 10 );
		}
		/// <summary>Inits ShippingProviderRuleEntity's mappings</summary>
		private void InitShippingProviderRuleEntityMappings()
		{
			base.AddElementMapping( "ShippingProviderRuleEntity", "ShipWorksLocal", @"dbo", "ShippingProviderRule", 4 );
			base.AddElementFieldMapping( "ShippingProviderRuleEntity", "ShippingProviderRuleID", "ShippingProviderRuleID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ShippingProviderRuleEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "ShippingProviderRuleEntity", "FilterNodeID", "FilterNodeID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "ShippingProviderRuleEntity", "ShipmentType", "ShipmentType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
		}
		/// <summary>Inits ShippingSettingsEntity's mappings</summary>
		private void InitShippingSettingsEntityMappings()
		{
			base.AddElementMapping( "ShippingSettingsEntity", "ShipWorksLocal", @"dbo", "ShippingSettings", 51 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "ShippingSettingsID", "ShippingSettingsID", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 0 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "InternalActivated", "Activated", false, (int)SqlDbType.VarChar, 45, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "InternalConfigured", "Configured", false, (int)SqlDbType.VarChar, 45, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "InternalExcluded", "Excluded", false, (int)SqlDbType.VarChar, 45, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "DefaultType", "DefaultType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "BlankPhoneOption", "BlankPhoneOption", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "BlankPhoneNumber", "BlankPhoneNumber", false, (int)SqlDbType.NVarChar, 16, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "InsurancePolicy", "InsurancePolicy", false, (int)SqlDbType.NVarChar, 40, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "InsuranceLastAgreed", "InsuranceLastAgreed", true, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 8 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "FedExUsername", "FedExUsername", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "FedExPassword", "FedExPassword", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "FedExMaskAccount", "FedExMaskAccount", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 11 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "FedExThermalDocTab", "FedExThermalDocTab", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 12 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "FedExThermalDocTabType", "FedExThermalDocTabType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 13 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "FedExInsuranceProvider", "FedExInsuranceProvider", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 14 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "FedExInsurancePennyOne", "FedExInsurancePennyOne", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 15 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "UpsAccessKey", "UpsAccessKey", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "UpsInsuranceProvider", "UpsInsuranceProvider", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 17 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "UpsInsurancePennyOne", "UpsInsurancePennyOne", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 18 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "EndiciaCustomsCertify", "EndiciaCustomsCertify", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 19 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "EndiciaCustomsSigner", "EndiciaCustomsSigner", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 20 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "EndiciaThermalDocTab", "EndiciaThermalDocTab", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 21 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "EndiciaThermalDocTabType", "EndiciaThermalDocTabType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 22 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "EndiciaAutomaticExpress1", "EndiciaAutomaticExpress1", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 23 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "EndiciaAutomaticExpress1Account", "EndiciaAutomaticExpress1Account", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 24 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "EndiciaInsuranceProvider", "EndiciaInsuranceProvider", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 25 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "WorldShipLaunch", "WorldShipLaunch", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 26 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "UspsAutomaticExpress1", "UspsAutomaticExpress1", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 27 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "UspsAutomaticExpress1Account", "UspsAutomaticExpress1Account", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 28 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "UspsInsuranceProvider", "UspsInsuranceProvider", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 29 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "Express1EndiciaCustomsCertify", "Express1EndiciaCustomsCertify", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 30 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "Express1EndiciaCustomsSigner", "Express1EndiciaCustomsSigner", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 31 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "Express1EndiciaThermalDocTab", "Express1EndiciaThermalDocTab", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 32 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "Express1EndiciaThermalDocTabType", "Express1EndiciaThermalDocTabType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 33 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "Express1EndiciaSingleSource", "Express1EndiciaSingleSource", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 34 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "OnTracInsuranceProvider", "OnTracInsuranceProvider", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 35 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "OnTracInsurancePennyOne", "OnTracInsurancePennyOne", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 36 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "IParcelInsuranceProvider", "iParcelInsuranceProvider", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 37 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "IParcelInsurancePennyOne", "iParcelInsurancePennyOne", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 38 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "Express1UspsSingleSource", "Express1UspsSingleSource", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 39 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "UpsMailInnovationsEnabled", "UpsMailInnovationsEnabled", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 40 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "WorldShipMailInnovationsEnabled", "WorldShipMailInnovationsEnabled", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 41 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "InternalBestRateExcludedShipmentTypes", "BestRateExcludedShipmentTypes", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 42 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "ShipSenseEnabled", "ShipSenseEnabled", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 43 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "ShipSenseUniquenessXml", "ShipSenseUniquenessXml", false, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 44 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "ShipSenseProcessedShipmentID", "ShipSenseProcessedShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 45 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "ShipSenseEndShipmentID", "ShipSenseEndShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 46 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "AutoCreateShipments", "AutoCreateShipments", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 47 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "FedExFimsEnabled", "FedExFimsEnabled", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 48 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "FedExFimsUsername", "FedExFimsUsername", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 49 );
			base.AddElementFieldMapping( "ShippingSettingsEntity", "FedExFimsPassword", "FedExFimsPassword", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 50 );
		}
		/// <summary>Inits ShipSenseKnowledgebaseEntity's mappings</summary>
		private void InitShipSenseKnowledgebaseEntityMappings()
		{
			base.AddElementMapping( "ShipSenseKnowledgebaseEntity", "ShipWorksLocal", @"dbo", "ShipSenseKnowledgeBase", 2 );
			base.AddElementFieldMapping( "ShipSenseKnowledgebaseEntity", "Hash", "Hash", false, (int)SqlDbType.NVarChar, 64, 0, 0, false, "", null, typeof(System.String), 0 );
			base.AddElementFieldMapping( "ShipSenseKnowledgebaseEntity", "Entry", "Entry", false, (int)SqlDbType.VarBinary, 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
		}
		/// <summary>Inits ShopifyOrderEntity's mappings</summary>
		private void InitShopifyOrderEntityMappings()
		{
			base.AddElementMapping( "ShopifyOrderEntity", "ShipWorksLocal", @"dbo", "ShopifyOrder", 4 );
			base.AddElementFieldMapping( "ShopifyOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ShopifyOrderEntity", "ShopifyOrderID", "ShopifyOrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "ShopifyOrderEntity", "FulfillmentStatusCode", "FulfillmentStatusCode", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "ShopifyOrderEntity", "PaymentStatusCode", "PaymentStatusCode", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
		}
		/// <summary>Inits ShopifyOrderItemEntity's mappings</summary>
		private void InitShopifyOrderItemEntityMappings()
		{
			base.AddElementMapping( "ShopifyOrderItemEntity", "ShipWorksLocal", @"dbo", "ShopifyOrderItem", 3 );
			base.AddElementFieldMapping( "ShopifyOrderItemEntity", "OrderItemID", "OrderItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ShopifyOrderItemEntity", "ShopifyOrderItemID", "ShopifyOrderItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "ShopifyOrderItemEntity", "ShopifyProductID", "ShopifyProductID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
		}
		/// <summary>Inits ShopifyStoreEntity's mappings</summary>
		private void InitShopifyStoreEntityMappings()
		{
			base.AddElementMapping( "ShopifyStoreEntity", "ShipWorksLocal", @"dbo", "ShopifyStore", 5 );
			base.AddElementFieldMapping( "ShopifyStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ShopifyStoreEntity", "ShopifyShopUrlName", "ShopifyShopUrlName", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "ShopifyStoreEntity", "ShopifyShopDisplayName", "ShopifyShopDisplayName", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ShopifyStoreEntity", "ShopifyAccessToken", "ShopifyAccessToken", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "ShopifyStoreEntity", "ShopifyRequestedShippingOption", "ShopifyRequestedShippingOption", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
		}
		/// <summary>Inits ShopSiteStoreEntity's mappings</summary>
		private void InitShopSiteStoreEntityMappings()
		{
			base.AddElementMapping( "ShopSiteStoreEntity", "ShipWorksLocal", @"dbo", "ShopSiteStore", 7 );
			base.AddElementFieldMapping( "ShopSiteStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ShopSiteStoreEntity", "Username", "Username", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "ShopSiteStoreEntity", "Password", "Password", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ShopSiteStoreEntity", "CgiUrl", "CgiUrl", false, (int)SqlDbType.NVarChar, 350, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "ShopSiteStoreEntity", "RequireSSL", "RequireSSL", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 4 );
			base.AddElementFieldMapping( "ShopSiteStoreEntity", "DownloadPageSize", "DownloadPageSize", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
			base.AddElementFieldMapping( "ShopSiteStoreEntity", "RequestTimeout", "RequestTimeout", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 6 );
		}
		/// <summary>Inits StatusPresetEntity's mappings</summary>
		private void InitStatusPresetEntityMappings()
		{
			base.AddElementMapping( "StatusPresetEntity", "ShipWorksLocal", @"dbo", "StatusPreset", 6 );
			base.AddElementFieldMapping( "StatusPresetEntity", "StatusPresetID", "StatusPresetID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "StatusPresetEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "StatusPresetEntity", "StoreID", "StoreID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "StatusPresetEntity", "StatusTarget", "StatusTarget", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "StatusPresetEntity", "StatusText", "StatusText", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "StatusPresetEntity", "IsDefault", "IsDefault", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 5 );
		}
		/// <summary>Inits StoreEntity's mappings</summary>
		private void InitStoreEntityMappings()
		{
			base.AddElementMapping( "StoreEntity", "ShipWorksLocal", @"dbo", "Store", 30 );
			base.AddElementFieldMapping( "StoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "StoreEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "StoreEntity", "License", "License", false, (int)SqlDbType.NVarChar, 150, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "StoreEntity", "Edition", "Edition", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "StoreEntity", "TypeCode", "TypeCode", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "StoreEntity", "Enabled", "Enabled", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 5 );
			base.AddElementFieldMapping( "StoreEntity", "SetupComplete", "SetupComplete", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 6 );
			base.AddElementFieldMapping( "StoreEntity", "StoreName", "StoreName", false, (int)SqlDbType.NVarChar, 75, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "StoreEntity", "Company", "Company", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "StoreEntity", "Street1", "Street1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "StoreEntity", "Street2", "Street2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "StoreEntity", "Street3", "Street3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "StoreEntity", "City", "City", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "StoreEntity", "StateProvCode", "StateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "StoreEntity", "PostalCode", "PostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "StoreEntity", "CountryCode", "CountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "StoreEntity", "Phone", "Phone", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "StoreEntity", "Fax", "Fax", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 17 );
			base.AddElementFieldMapping( "StoreEntity", "Email", "Email", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "StoreEntity", "Website", "Website", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 19 );
			base.AddElementFieldMapping( "StoreEntity", "AutoDownload", "AutoDownload", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 20 );
			base.AddElementFieldMapping( "StoreEntity", "AutoDownloadMinutes", "AutoDownloadMinutes", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 21 );
			base.AddElementFieldMapping( "StoreEntity", "AutoDownloadOnlyAway", "AutoDownloadOnlyAway", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 22 );
			base.AddElementFieldMapping( "StoreEntity", "AddressValidationSetting", "AddressValidationSetting", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 23 );
			base.AddElementFieldMapping( "StoreEntity", "ComputerDownloadPolicy", "ComputerDownloadPolicy", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 24 );
			base.AddElementFieldMapping( "StoreEntity", "DefaultEmailAccountID", "DefaultEmailAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 25 );
			base.AddElementFieldMapping( "StoreEntity", "ManualOrderPrefix", "ManualOrderPrefix", false, (int)SqlDbType.NVarChar, 10, 0, 0, false, "", null, typeof(System.String), 26 );
			base.AddElementFieldMapping( "StoreEntity", "ManualOrderPostfix", "ManualOrderPostfix", false, (int)SqlDbType.NVarChar, 10, 0, 0, false, "", null, typeof(System.String), 27 );
			base.AddElementFieldMapping( "StoreEntity", "InitialDownloadDays", "InitialDownloadDays", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 28 );
			base.AddElementFieldMapping( "StoreEntity", "InitialDownloadOrder", "InitialDownloadOrder", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 29 );
		}
		/// <summary>Inits SystemDataEntity's mappings</summary>
		private void InitSystemDataEntityMappings()
		{
			base.AddElementMapping( "SystemDataEntity", "ShipWorksLocal", @"dbo", "SystemData", 5 );
			base.AddElementFieldMapping( "SystemDataEntity", "SystemDataID", "SystemDataID", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 0 );
			base.AddElementFieldMapping( "SystemDataEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "SystemDataEntity", "DatabaseID", "DatabaseID", false, (int)SqlDbType.UniqueIdentifier, 0, 0, 0, false, "", null, typeof(System.Guid), 2 );
			base.AddElementFieldMapping( "SystemDataEntity", "DateFiltersLastUpdate", "DateFiltersLastUpdate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 3 );
			base.AddElementFieldMapping( "SystemDataEntity", "TemplateVersion", "TemplateVersion", false, (int)SqlDbType.VarChar, 30, 0, 0, false, "", null, typeof(System.String), 4 );
		}
		/// <summary>Inits TemplateEntity's mappings</summary>
		private void InitTemplateEntityMappings()
		{
			base.AddElementMapping( "TemplateEntity", "ShipWorksLocal", @"dbo", "Template", 23 );
			base.AddElementFieldMapping( "TemplateEntity", "TemplateID", "TemplateID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "TemplateEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "TemplateEntity", "ParentFolderID", "ParentFolderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "TemplateEntity", "Name", "Name", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "TemplateEntity", "Xsl", "Xsl", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "TemplateEntity", "Type", "Type", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
			base.AddElementFieldMapping( "TemplateEntity", "Context", "Context", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 6 );
			base.AddElementFieldMapping( "TemplateEntity", "OutputFormat", "OutputFormat", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 7 );
			base.AddElementFieldMapping( "TemplateEntity", "OutputEncoding", "OutputEncoding", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "TemplateEntity", "PageMarginLeft", "PageMarginLeft", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 9 );
			base.AddElementFieldMapping( "TemplateEntity", "PageMarginRight", "PageMarginRight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 10 );
			base.AddElementFieldMapping( "TemplateEntity", "PageMarginBottom", "PageMarginBottom", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 11 );
			base.AddElementFieldMapping( "TemplateEntity", "PageMarginTop", "PageMarginTop", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 12 );
			base.AddElementFieldMapping( "TemplateEntity", "PageWidth", "PageWidth", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 13 );
			base.AddElementFieldMapping( "TemplateEntity", "PageHeight", "PageHeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 14 );
			base.AddElementFieldMapping( "TemplateEntity", "LabelSheetID", "LabelSheetID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 15 );
			base.AddElementFieldMapping( "TemplateEntity", "PrintCopies", "PrintCopies", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 16 );
			base.AddElementFieldMapping( "TemplateEntity", "PrintCollate", "PrintCollate", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 17 );
			base.AddElementFieldMapping( "TemplateEntity", "SaveFileName", "SaveFileName", false, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "TemplateEntity", "SaveFileFolder", "SaveFileFolder", false, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 19 );
			base.AddElementFieldMapping( "TemplateEntity", "SaveFilePrompt", "SaveFilePrompt", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 20 );
			base.AddElementFieldMapping( "TemplateEntity", "SaveFileBOM", "SaveFileBOM", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 21 );
			base.AddElementFieldMapping( "TemplateEntity", "SaveFileOnlineResources", "SaveFileOnlineResources", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 22 );
		}
		/// <summary>Inits TemplateComputerSettingsEntity's mappings</summary>
		private void InitTemplateComputerSettingsEntityMappings()
		{
			base.AddElementMapping( "TemplateComputerSettingsEntity", "ShipWorksLocal", @"dbo", "TemplateComputerSettings", 5 );
			base.AddElementFieldMapping( "TemplateComputerSettingsEntity", "TemplateComputerSettingsID", "TemplateComputerSettingsID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "TemplateComputerSettingsEntity", "TemplateID", "TemplateID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "TemplateComputerSettingsEntity", "ComputerID", "ComputerID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "TemplateComputerSettingsEntity", "PrinterName", "PrinterName", false, (int)SqlDbType.NVarChar, 350, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "TemplateComputerSettingsEntity", "PaperSource", "PaperSource", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
		}
		/// <summary>Inits TemplateFolderEntity's mappings</summary>
		private void InitTemplateFolderEntityMappings()
		{
			base.AddElementMapping( "TemplateFolderEntity", "ShipWorksLocal", @"dbo", "TemplateFolder", 4 );
			base.AddElementFieldMapping( "TemplateFolderEntity", "TemplateFolderID", "TemplateFolderID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "TemplateFolderEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "TemplateFolderEntity", "ParentFolderID", "ParentFolderID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "TemplateFolderEntity", "Name", "Name", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 3 );
		}
		/// <summary>Inits TemplateStoreSettingsEntity's mappings</summary>
		private void InitTemplateStoreSettingsEntityMappings()
		{
			base.AddElementMapping( "TemplateStoreSettingsEntity", "ShipWorksLocal", @"dbo", "TemplateStoreSettings", 9 );
			base.AddElementFieldMapping( "TemplateStoreSettingsEntity", "TemplateStoreSettingsID", "TemplateStoreSettingsID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "TemplateStoreSettingsEntity", "TemplateID", "TemplateID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "TemplateStoreSettingsEntity", "StoreID", "StoreID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "TemplateStoreSettingsEntity", "EmailUseDefault", "EmailUseDefault", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "TemplateStoreSettingsEntity", "EmailAccountID", "EmailAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
			base.AddElementFieldMapping( "TemplateStoreSettingsEntity", "EmailTo", "EmailTo", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "TemplateStoreSettingsEntity", "EmailCc", "EmailCc", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "TemplateStoreSettingsEntity", "EmailBcc", "EmailBcc", false, (int)SqlDbType.NVarChar, 2147483647, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "TemplateStoreSettingsEntity", "EmailSubject", "EmailSubject", false, (int)SqlDbType.NVarChar, 500, 0, 0, false, "", null, typeof(System.String), 8 );
		}
		/// <summary>Inits TemplateUserSettingsEntity's mappings</summary>
		private void InitTemplateUserSettingsEntityMappings()
		{
			base.AddElementMapping( "TemplateUserSettingsEntity", "ShipWorksLocal", @"dbo", "TemplateUserSettings", 7 );
			base.AddElementFieldMapping( "TemplateUserSettingsEntity", "TemplateUserSettingsID", "TemplateUserSettingsID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "TemplateUserSettingsEntity", "TemplateID", "TemplateID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "TemplateUserSettingsEntity", "UserID", "UserID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "TemplateUserSettingsEntity", "PreviewSource", "PreviewSource", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "TemplateUserSettingsEntity", "PreviewCount", "PreviewCount", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "TemplateUserSettingsEntity", "PreviewFilterNodeID", "PreviewFilterNodeID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 5 );
			base.AddElementFieldMapping( "TemplateUserSettingsEntity", "PreviewZoom", "PreviewZoom", false, (int)SqlDbType.NVarChar, 10, 0, 0, false, "", null, typeof(System.String), 6 );
		}
		/// <summary>Inits ThreeDCartOrderItemEntity's mappings</summary>
		private void InitThreeDCartOrderItemEntityMappings()
		{
			base.AddElementMapping( "ThreeDCartOrderItemEntity", "ShipWorksLocal", @"dbo", "ThreeDCartOrderItem", 2 );
			base.AddElementFieldMapping( "ThreeDCartOrderItemEntity", "OrderItemID", "OrderItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ThreeDCartOrderItemEntity", "ThreeDCartShipmentID", "ThreeDCartShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
		}
		/// <summary>Inits ThreeDCartStoreEntity's mappings</summary>
		private void InitThreeDCartStoreEntityMappings()
		{
			base.AddElementMapping( "ThreeDCartStoreEntity", "ShipWorksLocal", @"dbo", "ThreeDCartStore", 6 );
			base.AddElementFieldMapping( "ThreeDCartStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ThreeDCartStoreEntity", "StoreUrl", "StoreUrl", false, (int)SqlDbType.NVarChar, 110, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "ThreeDCartStoreEntity", "ApiUserKey", "ApiUserKey", false, (int)SqlDbType.NVarChar, 65, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ThreeDCartStoreEntity", "TimeZoneID", "TimeZoneID", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "ThreeDCartStoreEntity", "StatusCodes", "StatusCodes", true, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "ThreeDCartStoreEntity", "DownloadModifiedNumberOfDaysBack", "DownloadModifiedNumberOfDaysBack", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
		}
		/// <summary>Inits UpsAccountEntity's mappings</summary>
		private void InitUpsAccountEntityMappings()
		{
			base.AddElementMapping( "UpsAccountEntity", "ShipWorksLocal", @"dbo", "UpsAccount", 22 );
			base.AddElementFieldMapping( "UpsAccountEntity", "UpsAccountID", "UpsAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "UpsAccountEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "UpsAccountEntity", "Description", "Description", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "UpsAccountEntity", "AccountNumber", "AccountNumber", false, (int)SqlDbType.NVarChar, 10, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "UpsAccountEntity", "UserID", "UserID", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "UpsAccountEntity", "Password", "Password", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "UpsAccountEntity", "RateType", "RateType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 6 );
			base.AddElementFieldMapping( "UpsAccountEntity", "InvoiceAuth", "InvoiceAuth", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 7 );
			base.AddElementFieldMapping( "UpsAccountEntity", "FirstName", "FirstName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "UpsAccountEntity", "MiddleName", "MiddleName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "UpsAccountEntity", "LastName", "LastName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "UpsAccountEntity", "Company", "Company", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "UpsAccountEntity", "Street1", "Street1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "UpsAccountEntity", "Street2", "Street2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "UpsAccountEntity", "Street3", "Street3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "UpsAccountEntity", "City", "City", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "UpsAccountEntity", "StateProvCode", "StateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "UpsAccountEntity", "PostalCode", "PostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 17 );
			base.AddElementFieldMapping( "UpsAccountEntity", "CountryCode", "CountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "UpsAccountEntity", "Phone", "Phone", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 19 );
			base.AddElementFieldMapping( "UpsAccountEntity", "Email", "Email", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 20 );
			base.AddElementFieldMapping( "UpsAccountEntity", "Website", "Website", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 21 );
		}
		/// <summary>Inits UpsPackageEntity's mappings</summary>
		private void InitUpsPackageEntityMappings()
		{
			base.AddElementMapping( "UpsPackageEntity", "ShipWorksLocal", @"dbo", "UpsPackage", 25 );
			base.AddElementFieldMapping( "UpsPackageEntity", "UpsPackageID", "UpsPackageID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "UpsPackageEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "UpsPackageEntity", "PackagingType", "PackagingType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "UpsPackageEntity", "Weight", "Weight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 3 );
			base.AddElementFieldMapping( "UpsPackageEntity", "DimsProfileID", "DimsProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
			base.AddElementFieldMapping( "UpsPackageEntity", "DimsLength", "DimsLength", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 5 );
			base.AddElementFieldMapping( "UpsPackageEntity", "DimsWidth", "DimsWidth", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 6 );
			base.AddElementFieldMapping( "UpsPackageEntity", "DimsHeight", "DimsHeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 7 );
			base.AddElementFieldMapping( "UpsPackageEntity", "DimsWeight", "DimsWeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 8 );
			base.AddElementFieldMapping( "UpsPackageEntity", "DimsAddWeight", "DimsAddWeight", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 9 );
			base.AddElementFieldMapping( "UpsPackageEntity", "Insurance", "Insurance", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 10 );
			base.AddElementFieldMapping( "UpsPackageEntity", "InsuranceValue", "InsuranceValue", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 11 );
			base.AddElementFieldMapping( "UpsPackageEntity", "InsurancePennyOne", "InsurancePennyOne", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 12 );
			base.AddElementFieldMapping( "UpsPackageEntity", "DeclaredValue", "DeclaredValue", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 13 );
			base.AddElementFieldMapping( "UpsPackageEntity", "TrackingNumber", "TrackingNumber", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "UpsPackageEntity", "UspsTrackingNumber", "UspsTrackingNumber", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "UpsPackageEntity", "AdditionalHandlingEnabled", "AdditionalHandlingEnabled", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 16 );
			base.AddElementFieldMapping( "UpsPackageEntity", "VerbalConfirmationEnabled", "VerbalConfirmationEnabled", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 17 );
			base.AddElementFieldMapping( "UpsPackageEntity", "VerbalConfirmationName", "VerbalConfirmationName", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "UpsPackageEntity", "VerbalConfirmationPhone", "VerbalConfirmationPhone", false, (int)SqlDbType.NVarChar, 15, 0, 0, false, "", null, typeof(System.String), 19 );
			base.AddElementFieldMapping( "UpsPackageEntity", "VerbalConfirmationPhoneExtension", "VerbalConfirmationPhoneExtension", false, (int)SqlDbType.NVarChar, 4, 0, 0, false, "", null, typeof(System.String), 20 );
			base.AddElementFieldMapping( "UpsPackageEntity", "DryIceEnabled", "DryIceEnabled", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 21 );
			base.AddElementFieldMapping( "UpsPackageEntity", "DryIceRegulationSet", "DryIceRegulationSet", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 22 );
			base.AddElementFieldMapping( "UpsPackageEntity", "DryIceWeight", "DryIceWeight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 23 );
			base.AddElementFieldMapping( "UpsPackageEntity", "DryIceIsForMedicalUse", "DryIceIsForMedicalUse", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 24 );
		}
		/// <summary>Inits UpsProfileEntity's mappings</summary>
		private void InitUpsProfileEntityMappings()
		{
			base.AddElementMapping( "UpsProfileEntity", "ShipWorksLocal", @"dbo", "UpsProfile", 36 );
			base.AddElementFieldMapping( "UpsProfileEntity", "ShippingProfileID", "ShippingProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "UpsProfileEntity", "UpsAccountID", "UpsAccountID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "UpsProfileEntity", "Service", "Service", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "UpsProfileEntity", "SaturdayDelivery", "SaturdayDelivery", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "UpsProfileEntity", "ResidentialDetermination", "ResidentialDetermination", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 4 );
			base.AddElementFieldMapping( "UpsProfileEntity", "DeliveryConfirmation", "DeliveryConfirmation", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 5 );
			base.AddElementFieldMapping( "UpsProfileEntity", "ReferenceNumber", "ReferenceNumber", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "UpsProfileEntity", "ReferenceNumber2", "ReferenceNumber2", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "UpsProfileEntity", "PayorType", "PayorType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 8 );
			base.AddElementFieldMapping( "UpsProfileEntity", "PayorAccount", "PayorAccount", true, (int)SqlDbType.VarChar, 10, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "UpsProfileEntity", "PayorPostalCode", "PayorPostalCode", true, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "UpsProfileEntity", "PayorCountryCode", "PayorCountryCode", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "UpsProfileEntity", "EmailNotifySender", "EmailNotifySender", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 12 );
			base.AddElementFieldMapping( "UpsProfileEntity", "EmailNotifyRecipient", "EmailNotifyRecipient", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 13 );
			base.AddElementFieldMapping( "UpsProfileEntity", "EmailNotifyOther", "EmailNotifyOther", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 14 );
			base.AddElementFieldMapping( "UpsProfileEntity", "EmailNotifyOtherAddress", "EmailNotifyOtherAddress", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "UpsProfileEntity", "EmailNotifyFrom", "EmailNotifyFrom", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "UpsProfileEntity", "EmailNotifySubject", "EmailNotifySubject", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 17 );
			base.AddElementFieldMapping( "UpsProfileEntity", "EmailNotifyMessage", "EmailNotifyMessage", true, (int)SqlDbType.NVarChar, 120, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "UpsProfileEntity", "ReturnService", "ReturnService", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 19 );
			base.AddElementFieldMapping( "UpsProfileEntity", "ReturnUndeliverableEmail", "ReturnUndeliverableEmail", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 20 );
			base.AddElementFieldMapping( "UpsProfileEntity", "ReturnContents", "ReturnContents", true, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 21 );
			base.AddElementFieldMapping( "UpsProfileEntity", "Endorsement", "Endorsement", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 22 );
			base.AddElementFieldMapping( "UpsProfileEntity", "Subclassification", "Subclassification", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 23 );
			base.AddElementFieldMapping( "UpsProfileEntity", "PaperlessAdditionalDocumentation", "PaperlessAdditionalDocumentation", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 24 );
			base.AddElementFieldMapping( "UpsProfileEntity", "ShipperRelease", "ShipperRelease", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 25 );
			base.AddElementFieldMapping( "UpsProfileEntity", "CarbonNeutral", "CarbonNeutral", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 26 );
			base.AddElementFieldMapping( "UpsProfileEntity", "CommercialPaperlessInvoice", "CommercialPaperlessInvoice", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 27 );
			base.AddElementFieldMapping( "UpsProfileEntity", "CostCenter", "CostCenter", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 28 );
			base.AddElementFieldMapping( "UpsProfileEntity", "IrregularIndicator", "IrregularIndicator", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 29 );
			base.AddElementFieldMapping( "UpsProfileEntity", "Cn22Number", "Cn22Number", true, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 30 );
			base.AddElementFieldMapping( "UpsProfileEntity", "ShipmentChargeType", "ShipmentChargeType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 31 );
			base.AddElementFieldMapping( "UpsProfileEntity", "ShipmentChargeAccount", "ShipmentChargeAccount", true, (int)SqlDbType.VarChar, 10, 0, 0, false, "", null, typeof(System.String), 32 );
			base.AddElementFieldMapping( "UpsProfileEntity", "ShipmentChargePostalCode", "ShipmentChargePostalCode", true, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 33 );
			base.AddElementFieldMapping( "UpsProfileEntity", "ShipmentChargeCountryCode", "ShipmentChargeCountryCode", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 34 );
			base.AddElementFieldMapping( "UpsProfileEntity", "UspsPackageID", "UspsPackageID", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 35 );
		}
		/// <summary>Inits UpsProfilePackageEntity's mappings</summary>
		private void InitUpsProfilePackageEntityMappings()
		{
			base.AddElementMapping( "UpsProfilePackageEntity", "ShipWorksLocal", @"dbo", "UpsProfilePackage", 19 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "UpsProfilePackageID", "UpsProfilePackageID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "ShippingProfileID", "ShippingProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "PackagingType", "PackagingType", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "Weight", "Weight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 3 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "DimsProfileID", "DimsProfileID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "DimsLength", "DimsLength", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 5 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "DimsWidth", "DimsWidth", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 6 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "DimsHeight", "DimsHeight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 7 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "DimsWeight", "DimsWeight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 8 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "DimsAddWeight", "DimsAddWeight", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 9 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "AdditionalHandlingEnabled", "AdditionalHandlingEnabled", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 10 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "VerbalConfirmationEnabled", "VerbalConfirmationEnabled", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 11 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "VerbalConfirmationName", "VerbalConfirmationName", true, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "VerbalConfirmationPhone", "VerbalConfirmationPhone", true, (int)SqlDbType.NVarChar, 15, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "VerbalConfirmationPhoneExtension", "VerbalConfirmationPhoneExtension", true, (int)SqlDbType.NVarChar, 4, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "DryIceEnabled", "DryIceEnabled", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 15 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "DryIceRegulationSet", "DryIceRegulationSet", true, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 16 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "DryIceWeight", "DryIceWeight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 17 );
			base.AddElementFieldMapping( "UpsProfilePackageEntity", "DryIceIsForMedicalUse", "DryIceIsForMedicalUse", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 18 );
		}
		/// <summary>Inits UpsShipmentEntity's mappings</summary>
		private void InitUpsShipmentEntityMappings()
		{
			base.AddElementMapping( "UpsShipmentEntity", "ShipWorksLocal", @"dbo", "UpsShipment", 51 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "UpsAccountID", "UpsAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "Service", "Service", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 2 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "SaturdayDelivery", "SaturdayDelivery", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "CodEnabled", "CodEnabled", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 4 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "CodAmount", "CodAmount", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 5 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "CodPaymentType", "CodPaymentType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 6 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "DeliveryConfirmation", "DeliveryConfirmation", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 7 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "ReferenceNumber", "ReferenceNumber", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "ReferenceNumber2", "ReferenceNumber2", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "PayorType", "PayorType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 10 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "PayorAccount", "PayorAccount", false, (int)SqlDbType.VarChar, 10, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "PayorPostalCode", "PayorPostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "PayorCountryCode", "PayorCountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "EmailNotifySender", "EmailNotifySender", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 14 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "EmailNotifyRecipient", "EmailNotifyRecipient", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 15 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "EmailNotifyOther", "EmailNotifyOther", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 16 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "EmailNotifyOtherAddress", "EmailNotifyOtherAddress", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 17 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "EmailNotifyFrom", "EmailNotifyFrom", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "EmailNotifySubject", "EmailNotifySubject", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 19 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "EmailNotifyMessage", "EmailNotifyMessage", false, (int)SqlDbType.NVarChar, 120, 0, 0, false, "", null, typeof(System.String), 20 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "CustomsDocumentsOnly", "CustomsDocumentsOnly", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 21 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "CustomsDescription", "CustomsDescription", false, (int)SqlDbType.NVarChar, 150, 0, 0, false, "", null, typeof(System.String), 22 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "CommercialPaperlessInvoice", "CommercialPaperlessInvoice", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 23 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "CommercialInvoiceTermsOfSale", "CommercialInvoiceTermsOfSale", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 24 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "CommercialInvoicePurpose", "CommercialInvoicePurpose", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 25 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "CommercialInvoiceComments", "CommercialInvoiceComments", false, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 26 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "CommercialInvoiceFreight", "CommercialInvoiceFreight", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 27 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "CommercialInvoiceInsurance", "CommercialInvoiceInsurance", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 28 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "CommercialInvoiceOther", "CommercialInvoiceOther", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 29 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "WorldShipStatus", "WorldShipStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 30 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "PublishedCharges", "PublishedCharges", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 31 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "NegotiatedRate", "NegotiatedRate", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 32 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "ReturnService", "ReturnService", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 33 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "ReturnUndeliverableEmail", "ReturnUndeliverableEmail", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 34 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "ReturnContents", "ReturnContents", false, (int)SqlDbType.NVarChar, 300, 0, 0, false, "", null, typeof(System.String), 35 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "UspsTrackingNumber", "UspsTrackingNumber", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 36 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "Endorsement", "Endorsement", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 37 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "Subclassification", "Subclassification", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 38 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "PaperlessAdditionalDocumentation", "PaperlessAdditionalDocumentation", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 39 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "ShipperRelease", "ShipperRelease", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 40 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "CarbonNeutral", "CarbonNeutral", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 41 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "CostCenter", "CostCenter", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 42 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "IrregularIndicator", "IrregularIndicator", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 43 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "Cn22Number", "Cn22Number", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 44 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "ShipmentChargeType", "ShipmentChargeType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 45 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "ShipmentChargeAccount", "ShipmentChargeAccount", false, (int)SqlDbType.VarChar, 10, 0, 0, false, "", null, typeof(System.String), 46 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "ShipmentChargePostalCode", "ShipmentChargePostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 47 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "ShipmentChargeCountryCode", "ShipmentChargeCountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 48 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "UspsPackageID", "UspsPackageID", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 49 );
			base.AddElementFieldMapping( "UpsShipmentEntity", "RequestedLabelFormat", "RequestedLabelFormat", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 50 );
		}
		/// <summary>Inits UserEntity's mappings</summary>
		private void InitUserEntityMappings()
		{
			base.AddElementMapping( "UserEntity", "ShipWorksLocal", @"dbo", "User", 7 );
			base.AddElementFieldMapping( "UserEntity", "UserID", "UserID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "UserEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "UserEntity", "Username", "Username", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "UserEntity", "Password", "Password", false, (int)SqlDbType.NVarChar, 32, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "UserEntity", "Email", "Email", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "UserEntity", "IsAdmin", "IsAdmin", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 5 );
			base.AddElementFieldMapping( "UserEntity", "IsDeleted", "IsDeleted", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 6 );
		}
		/// <summary>Inits UserColumnSettingsEntity's mappings</summary>
		private void InitUserColumnSettingsEntityMappings()
		{
			base.AddElementMapping( "UserColumnSettingsEntity", "ShipWorksLocal", @"dbo", "UserColumnSettings", 5 );
			base.AddElementFieldMapping( "UserColumnSettingsEntity", "UserColumnSettingsID", "UserColumnSettingsID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "UserColumnSettingsEntity", "SettingsKey", "SettingsKey", false, (int)SqlDbType.UniqueIdentifier, 0, 0, 0, false, "", null, typeof(System.Guid), 1 );
			base.AddElementFieldMapping( "UserColumnSettingsEntity", "UserID", "UserID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "UserColumnSettingsEntity", "InitialSortType", "InitialSortType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 3 );
			base.AddElementFieldMapping( "UserColumnSettingsEntity", "GridColumnLayoutID", "GridColumnLayoutID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 4 );
		}
		/// <summary>Inits UserSettingsEntity's mappings</summary>
		private void InitUserSettingsEntityMappings()
		{
			base.AddElementMapping( "UserSettingsEntity", "ShipWorksLocal", @"dbo", "UserSettings", 15 );
			base.AddElementFieldMapping( "UserSettingsEntity", "UserID", "UserID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "UserSettingsEntity", "DisplayColorScheme", "DisplayColorScheme", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 1 );
			base.AddElementFieldMapping( "UserSettingsEntity", "DisplaySystemTray", "DisplaySystemTray", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 2 );
			base.AddElementFieldMapping( "UserSettingsEntity", "WindowLayout", "WindowLayout", false, (int)SqlDbType.VarBinary, 2147483647, 0, 0, false, "", null, typeof(System.Byte[]), 3 );
			base.AddElementFieldMapping( "UserSettingsEntity", "GridMenuLayout", "GridMenuLayout", true, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "UserSettingsEntity", "FilterInitialUseLastActive", "FilterInitialUseLastActive", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 5 );
			base.AddElementFieldMapping( "UserSettingsEntity", "FilterInitialSpecified", "FilterInitialSpecified", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 6 );
			base.AddElementFieldMapping( "UserSettingsEntity", "FilterInitialSortType", "FilterInitialSortType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 7 );
			base.AddElementFieldMapping( "UserSettingsEntity", "OrderFilterLastActive", "OrderFilterLastActive", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 8 );
			base.AddElementFieldMapping( "UserSettingsEntity", "OrderFilterExpandedFolders", "OrderFilterExpandedFolders", true, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "UserSettingsEntity", "ShippingWeightFormat", "ShippingWeightFormat", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 10 );
			base.AddElementFieldMapping( "UserSettingsEntity", "TemplateExpandedFolders", "TemplateExpandedFolders", true, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "UserSettingsEntity", "TemplateLastSelected", "TemplateLastSelected", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 12 );
			base.AddElementFieldMapping( "UserSettingsEntity", "CustomerFilterLastActive", "CustomerFilterLastActive", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 13 );
			base.AddElementFieldMapping( "UserSettingsEntity", "CustomerFilterExpandedFolders", "CustomerFilterExpandedFolders", true, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 14 );
		}
		/// <summary>Inits UspsAccountEntity's mappings</summary>
		private void InitUspsAccountEntityMappings()
		{
			base.AddElementMapping( "UspsAccountEntity", "ShipWorksLocal", @"dbo", "UspsAccount", 23 );
			base.AddElementFieldMapping( "UspsAccountEntity", "UspsAccountID", "UspsAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "UspsAccountEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 1 );
			base.AddElementFieldMapping( "UspsAccountEntity", "Description", "Description", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "UspsAccountEntity", "Username", "Username", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "UspsAccountEntity", "Password", "Password", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "UspsAccountEntity", "FirstName", "FirstName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "UspsAccountEntity", "MiddleName", "MiddleName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "UspsAccountEntity", "LastName", "LastName", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "UspsAccountEntity", "Company", "Company", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "UspsAccountEntity", "Street1", "Street1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "UspsAccountEntity", "Street2", "Street2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "UspsAccountEntity", "Street3", "Street3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "UspsAccountEntity", "City", "City", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "UspsAccountEntity", "StateProvCode", "StateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "UspsAccountEntity", "PostalCode", "PostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "UspsAccountEntity", "CountryCode", "CountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "UspsAccountEntity", "Phone", "Phone", false, (int)SqlDbType.NVarChar, 25, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "UspsAccountEntity", "Email", "Email", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 17 );
			base.AddElementFieldMapping( "UspsAccountEntity", "Website", "Website", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "UspsAccountEntity", "MailingPostalCode", "MailingPostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 19 );
			base.AddElementFieldMapping( "UspsAccountEntity", "UspsReseller", "UspsReseller", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 20 );
			base.AddElementFieldMapping( "UspsAccountEntity", "ContractType", "ContractType", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 21 );
			base.AddElementFieldMapping( "UspsAccountEntity", "CreatedDate", "CreatedDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 22 );
		}
		/// <summary>Inits UspsProfileEntity's mappings</summary>
		private void InitUspsProfileEntityMappings()
		{
			base.AddElementMapping( "UspsProfileEntity", "ShipWorksLocal", @"dbo", "UspsProfile", 5 );
			base.AddElementFieldMapping( "UspsProfileEntity", "ShippingProfileID", "ShippingProfileID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "UspsProfileEntity", "UspsAccountID", "UspsAccountID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "UspsProfileEntity", "HidePostage", "HidePostage", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 2 );
			base.AddElementFieldMapping( "UspsProfileEntity", "RequireFullAddressValidation", "RequireFullAddressValidation", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "UspsProfileEntity", "RateShop", "RateShop", true, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 4 );
		}
		/// <summary>Inits UspsScanFormEntity's mappings</summary>
		private void InitUspsScanFormEntityMappings()
		{
			base.AddElementMapping( "UspsScanFormEntity", "ShipWorksLocal", @"dbo", "UspsScanForm", 7 );
			base.AddElementFieldMapping( "UspsScanFormEntity", "UspsScanFormID", "UspsScanFormID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "UspsScanFormEntity", "UspsAccountID", "UspsAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "UspsScanFormEntity", "ScanFormTransactionID", "ScanFormTransactionID", false, (int)SqlDbType.VarChar, 100, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "UspsScanFormEntity", "ScanFormUrl", "ScanFormUrl", false, (int)SqlDbType.VarChar, 2048, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "UspsScanFormEntity", "CreatedDate", "CreatedDate", false, (int)SqlDbType.DateTime, 0, 0, 0, false, "", null, typeof(System.DateTime), 4 );
			base.AddElementFieldMapping( "UspsScanFormEntity", "ScanFormBatchID", "ScanFormBatchID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 5 );
			base.AddElementFieldMapping( "UspsScanFormEntity", "Description", "Description", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 6 );
		}
		/// <summary>Inits UspsShipmentEntity's mappings</summary>
		private void InitUspsShipmentEntityMappings()
		{
			base.AddElementMapping( "UspsShipmentEntity", "ShipWorksLocal", @"dbo", "UspsShipment", 10 );
			base.AddElementFieldMapping( "UspsShipmentEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "UspsShipmentEntity", "UspsAccountID", "UspsAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "UspsShipmentEntity", "HidePostage", "HidePostage", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 2 );
			base.AddElementFieldMapping( "UspsShipmentEntity", "RequireFullAddressValidation", "RequireFullAddressValidation", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "UspsShipmentEntity", "IntegratorTransactionID", "IntegratorTransactionID", false, (int)SqlDbType.UniqueIdentifier, 0, 0, 0, false, "", null, typeof(System.Guid), 4 );
			base.AddElementFieldMapping( "UspsShipmentEntity", "UspsTransactionID", "UspsTransactionID", false, (int)SqlDbType.UniqueIdentifier, 0, 0, 0, false, "", null, typeof(System.Guid), 5 );
			base.AddElementFieldMapping( "UspsShipmentEntity", "OriginalUspsAccountID", "OriginalUspsAccountID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 6 );
			base.AddElementFieldMapping( "UspsShipmentEntity", "ScanFormBatchID", "ScanFormBatchID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 7 );
			base.AddElementFieldMapping( "UspsShipmentEntity", "RequestedLabelFormat", "RequestedLabelFormat", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 8 );
			base.AddElementFieldMapping( "UspsShipmentEntity", "RateShop", "RateShop", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 9 );
		}
		/// <summary>Inits ValidatedAddressEntity's mappings</summary>
		private void InitValidatedAddressEntityMappings()
		{
			base.AddElementMapping( "ValidatedAddressEntity", "ShipWorksLocal", @"dbo", "ValidatedAddress", 15 );
			base.AddElementFieldMapping( "ValidatedAddressEntity", "ValidatedAddressID", "ValidatedAddressID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "ValidatedAddressEntity", "ConsumerID", "ConsumerID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "ValidatedAddressEntity", "AddressPrefix", "AddressPrefix", false, (int)SqlDbType.NVarChar, 10, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "ValidatedAddressEntity", "IsOriginal", "IsOriginal", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 3 );
			base.AddElementFieldMapping( "ValidatedAddressEntity", "Street1", "Street1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "ValidatedAddressEntity", "Street2", "Street2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "ValidatedAddressEntity", "Street3", "Street3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "ValidatedAddressEntity", "City", "City", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "ValidatedAddressEntity", "StateProvCode", "StateProvCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "ValidatedAddressEntity", "PostalCode", "PostalCode", false, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "ValidatedAddressEntity", "CountryCode", "CountryCode", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "ValidatedAddressEntity", "ResidentialStatus", "ResidentialStatus", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 11 );
			base.AddElementFieldMapping( "ValidatedAddressEntity", "POBox", "POBox", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 12 );
			base.AddElementFieldMapping( "ValidatedAddressEntity", "USTerritory", "USTerritory", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 13 );
			base.AddElementFieldMapping( "ValidatedAddressEntity", "MilitaryAddress", "MilitaryAddress", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 14 );
		}
		/// <summary>Inits VersionSignoffEntity's mappings</summary>
		private void InitVersionSignoffEntityMappings()
		{
			base.AddElementMapping( "VersionSignoffEntity", "ShipWorksLocal", @"dbo", "VersionSignoff", 4 );
			base.AddElementFieldMapping( "VersionSignoffEntity", "VersionSignoffID", "VersionSignoffID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "VersionSignoffEntity", "Version", "Version", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "VersionSignoffEntity", "UserID", "UserID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "VersionSignoffEntity", "ComputerID", "ComputerID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 3 );
		}
		/// <summary>Inits VolusionStoreEntity's mappings</summary>
		private void InitVolusionStoreEntityMappings()
		{
			base.AddElementMapping( "VolusionStoreEntity", "ShipWorksLocal", @"dbo", "VolusionStore", 10 );
			base.AddElementFieldMapping( "VolusionStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "VolusionStoreEntity", "StoreUrl", "StoreUrl", false, (int)SqlDbType.VarChar, 255, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "VolusionStoreEntity", "WebUserName", "WebUserName", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "VolusionStoreEntity", "WebPassword", "WebPassword", false, (int)SqlDbType.VarChar, 70, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "VolusionStoreEntity", "ApiPassword", "ApiPassword", false, (int)SqlDbType.VarChar, 100, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "VolusionStoreEntity", "PaymentMethods", "PaymentMethods", false, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "VolusionStoreEntity", "ShipmentMethods", "ShipmentMethods", false, (int)SqlDbType.Xml, 2147483647, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "VolusionStoreEntity", "DownloadOrderStatuses", "DownloadOrderStatuses", false, (int)SqlDbType.VarChar, 255, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "VolusionStoreEntity", "ServerTimeZone", "ServerTimeZone", false, (int)SqlDbType.VarChar, 30, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "VolusionStoreEntity", "ServerTimeZoneDST", "ServerTimeZoneDST", false, (int)SqlDbType.Bit, 0, 0, 0, false, "", null, typeof(System.Boolean), 9 );
		}
		/// <summary>Inits WorldShipGoodsEntity's mappings</summary>
		private void InitWorldShipGoodsEntityMappings()
		{
			base.AddElementMapping( "WorldShipGoodsEntity", "ShipWorksLocal", @"dbo", "WorldShipGoods", 11 );
			base.AddElementFieldMapping( "WorldShipGoodsEntity", "WorldShipGoodsID", "WorldShipGoodsID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "WorldShipGoodsEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "WorldShipGoodsEntity", "ShipmentCustomsItemID", "ShipmentCustomsItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 2 );
			base.AddElementFieldMapping( "WorldShipGoodsEntity", "Description", "Description", false, (int)SqlDbType.NVarChar, 150, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "WorldShipGoodsEntity", "TariffCode", "TariffCode", false, (int)SqlDbType.VarChar, 15, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "WorldShipGoodsEntity", "CountryOfOrigin", "CountryOfOrigin", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "WorldShipGoodsEntity", "Units", "Units", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 6 );
			base.AddElementFieldMapping( "WorldShipGoodsEntity", "UnitOfMeasure", "UnitOfMeasure", false, (int)SqlDbType.VarChar, 5, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "WorldShipGoodsEntity", "UnitPrice", "UnitPrice", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 8 );
			base.AddElementFieldMapping( "WorldShipGoodsEntity", "Weight", "Weight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 9 );
			base.AddElementFieldMapping( "WorldShipGoodsEntity", "InvoiceCurrencyCode", "InvoiceCurrencyCode", true, (int)SqlDbType.VarChar, 3, 0, 0, false, "", null, typeof(System.String), 10 );
		}
		/// <summary>Inits WorldShipPackageEntity's mappings</summary>
		private void InitWorldShipPackageEntityMappings()
		{
			base.AddElementMapping( "WorldShipPackageEntity", "ShipWorksLocal", @"dbo", "WorldShipPackage", 44 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "UpsPackageID", "UpsPackageID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "PackageType", "PackageType", false, (int)SqlDbType.VarChar, 35, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "Weight", "Weight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 3 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "ReferenceNumber", "ReferenceNumber", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "ReferenceNumber2", "ReferenceNumber2", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "CodOption", "CodOption", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "CodAmount", "CodAmount", false, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 7 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "CodCashOnly", "CodCashOnly", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "DeliveryConfirmation", "DeliveryConfirmation", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "DeliveryConfirmationSignature", "DeliveryConfirmationSignature", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "DeliveryConfirmationAdult", "DeliveryConfirmationAdult", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "Length", "Length", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 12 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "Width", "Width", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 13 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "Height", "Height", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 14 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "DeclaredValueAmount", "DeclaredValueAmount", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 15 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "DeclaredValueOption", "DeclaredValueOption", true, (int)SqlDbType.NChar, 2, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "CN22GoodsType", "CN22GoodsType", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 17 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "CN22Description", "CN22Description", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "PostalSubClass", "PostalSubClass", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 19 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "MIDeliveryConfirmation", "MIDeliveryConfirmation", true, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 20 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "QvnOption", "QvnOption", true, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 21 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "QvnFrom", "QvnFrom", true, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 22 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "QvnSubjectLine", "QvnSubjectLine", true, (int)SqlDbType.NVarChar, 18, 0, 0, false, "", null, typeof(System.String), 23 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "QvnMemo", "QvnMemo", true, (int)SqlDbType.NVarChar, 150, 0, 0, false, "", null, typeof(System.String), 24 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "Qvn1ShipNotify", "Qvn1ShipNotify", true, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 25 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "Qvn1ContactName", "Qvn1ContactName", true, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 26 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "Qvn1Email", "Qvn1Email", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 27 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "Qvn2ShipNotify", "Qvn2ShipNotify", true, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 28 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "Qvn2ContactName", "Qvn2ContactName", true, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 29 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "Qvn2Email", "Qvn2Email", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 30 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "Qvn3ShipNotify", "Qvn3ShipNotify", true, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 31 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "Qvn3ContactName", "Qvn3ContactName", true, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 32 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "Qvn3Email", "Qvn3Email", true, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 33 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "ShipperRelease", "ShipperRelease", true, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 34 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "AdditionalHandlingEnabled", "AdditionalHandlingEnabled", true, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 35 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "VerbalConfirmationOption", "VerbalConfirmationOption", true, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 36 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "VerbalConfirmationContactName", "VerbalConfirmationContactName", true, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 37 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "VerbalConfirmationTelephone", "VerbalConfirmationTelephone", true, (int)SqlDbType.NVarChar, 15, 0, 0, false, "", null, typeof(System.String), 38 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "DryIceRegulationSet", "DryIceRegulationSet", true, (int)SqlDbType.NVarChar, 5, 0, 0, false, "", null, typeof(System.String), 39 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "DryIceWeight", "DryIceWeight", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 40 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "DryIceMedicalPurpose", "DryIceMedicalPurpose", true, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 41 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "DryIceOption", "DryIceOption", true, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 42 );
			base.AddElementFieldMapping( "WorldShipPackageEntity", "DryIceWeightUnitOfMeasure", "DryIceWeightUnitOfMeasure", true, (int)SqlDbType.NVarChar, 10, 0, 0, false, "", null, typeof(System.String), 43 );
		}
		/// <summary>Inits WorldShipProcessedEntity's mappings</summary>
		private void InitWorldShipProcessedEntityMappings()
		{
			base.AddElementMapping( "WorldShipProcessedEntity", "ShipWorksLocal", @"dbo", "WorldShipProcessed", 17 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "WorldShipProcessedID", "WorldShipProcessedID", false, (int)SqlDbType.BigInt, 0, 0, 19, true, "SCOPE_IDENTITY()", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "ShipmentID", "ShipmentID", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "RowVersion", "RowVersion", false, (int)SqlDbType.Timestamp, 0, 0, 0, false, "", null, typeof(System.Byte[]), 2 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "PublishedCharges", "PublishedCharges", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 3 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "NegotiatedCharges", "NegotiatedCharges", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 4 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "TrackingNumber", "TrackingNumber", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "UspsTrackingNumber", "UspsTrackingNumber", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "ServiceType", "ServiceType", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "PackageType", "PackageType", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "UpsPackageID", "UpsPackageID", true, (int)SqlDbType.NVarChar, 20, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "DeclaredValueAmount", "DeclaredValueAmount", true, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 10 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "DeclaredValueOption", "DeclaredValueOption", true, (int)SqlDbType.NChar, 2, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "WorldShipShipmentID", "WorldShipShipmentID", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "VoidIndicator", "VoidIndicator", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "NumberOfPackages", "NumberOfPackages", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "LeadTrackingNumber", "LeadTrackingNumber", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "WorldShipProcessedEntity", "ShipmentIdCalculated", "ShipmentIdCalculated", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 16 );
		}
		/// <summary>Inits WorldShipShipmentEntity's mappings</summary>
		private void InitWorldShipShipmentEntityMappings()
		{
			base.AddElementMapping( "WorldShipShipmentEntity", "ShipWorksLocal", @"dbo", "WorldShipShipment", 66 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ShipmentID", "ShipmentID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "OrderNumber", "OrderNumber", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "FromCompanyOrName", "FromCompanyOrName", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 2 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "FromAttention", "FromAttention", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 3 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "FromAddress1", "FromAddress1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 4 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "FromAddress2", "FromAddress2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 5 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "FromAddress3", "FromAddress3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 6 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "FromCountryCode", "FromCountryCode", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 7 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "FromPostalCode", "FromPostalCode", false, (int)SqlDbType.VarChar, 20, 0, 0, false, "", null, typeof(System.String), 8 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "FromCity", "FromCity", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 9 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "FromStateProvCode", "FromStateProvCode", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 10 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "FromTelephone", "FromTelephone", false, (int)SqlDbType.VarChar, 25, 0, 0, false, "", null, typeof(System.String), 11 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "FromEmail", "FromEmail", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 12 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "FromAccountNumber", "FromAccountNumber", false, (int)SqlDbType.VarChar, 10, 0, 0, false, "", null, typeof(System.String), 13 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ToCustomerID", "ToCustomerID", false, (int)SqlDbType.NVarChar, 30, 0, 0, false, "", null, typeof(System.String), 14 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ToCompanyOrName", "ToCompanyOrName", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 15 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ToAttention", "ToAttention", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 16 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ToAddress1", "ToAddress1", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 17 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ToAddress2", "ToAddress2", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 18 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ToAddress3", "ToAddress3", false, (int)SqlDbType.NVarChar, 60, 0, 0, false, "", null, typeof(System.String), 19 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ToCountryCode", "ToCountryCode", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 20 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ToPostalCode", "ToPostalCode", false, (int)SqlDbType.VarChar, 20, 0, 0, false, "", null, typeof(System.String), 21 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ToCity", "ToCity", false, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 22 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ToStateProvCode", "ToStateProvCode", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 23 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ToTelephone", "ToTelephone", false, (int)SqlDbType.VarChar, 25, 0, 0, false, "", null, typeof(System.String), 24 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ToEmail", "ToEmail", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 25 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ToAccountNumber", "ToAccountNumber", false, (int)SqlDbType.VarChar, 10, 0, 0, false, "", null, typeof(System.String), 26 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ToResidential", "ToResidential", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 27 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ServiceType", "ServiceType", false, (int)SqlDbType.VarChar, 3, 0, 0, false, "", null, typeof(System.String), 28 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "BillTransportationTo", "BillTransportationTo", false, (int)SqlDbType.VarChar, 20, 0, 0, false, "", null, typeof(System.String), 29 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "SaturdayDelivery", "SaturdayDelivery", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 30 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "QvnOption", "QvnOption", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 31 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "QvnFrom", "QvnFrom", true, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 32 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "QvnSubjectLine", "QvnSubjectLine", true, (int)SqlDbType.NVarChar, 18, 0, 0, false, "", null, typeof(System.String), 33 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "QvnMemo", "QvnMemo", true, (int)SqlDbType.NVarChar, 150, 0, 0, false, "", null, typeof(System.String), 34 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "Qvn1ShipNotify", "Qvn1ShipNotify", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 35 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "Qvn1DeliveryNotify", "Qvn1DeliveryNotify", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 36 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "Qvn1ExceptionNotify", "Qvn1ExceptionNotify", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 37 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "Qvn1ContactName", "Qvn1ContactName", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 38 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "Qvn1Email", "Qvn1Email", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 39 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "Qvn2ShipNotify", "Qvn2ShipNotify", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 40 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "Qvn2DeliveryNotify", "Qvn2DeliveryNotify", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 41 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "Qvn2ExceptionNotify", "Qvn2ExceptionNotify", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 42 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "Qvn2ContactName", "Qvn2ContactName", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 43 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "Qvn2Email", "Qvn2Email", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 44 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "Qvn3ShipNotify", "Qvn3ShipNotify", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 45 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "Qvn3DeliveryNotify", "Qvn3DeliveryNotify", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 46 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "Qvn3ExceptionNotify", "Qvn3ExceptionNotify", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 47 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "Qvn3ContactName", "Qvn3ContactName", false, (int)SqlDbType.NVarChar, 35, 0, 0, false, "", null, typeof(System.String), 48 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "Qvn3Email", "Qvn3Email", false, (int)SqlDbType.NVarChar, 100, 0, 0, false, "", null, typeof(System.String), 49 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "CustomsDescriptionOfGoods", "CustomsDescriptionOfGoods", true, (int)SqlDbType.NVarChar, 150, 0, 0, false, "", null, typeof(System.String), 50 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "CustomsDocumentsOnly", "CustomsDocumentsOnly", true, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 51 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ShipperNumber", "ShipperNumber", false, (int)SqlDbType.VarChar, 10, 0, 0, false, "", null, typeof(System.String), 52 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "PackageCount", "PackageCount", false, (int)SqlDbType.Int, 0, 0, 10, false, "", null, typeof(System.Int32), 53 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "DeliveryConfirmation", "DeliveryConfirmation", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 54 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "DeliveryConfirmationAdult", "DeliveryConfirmationAdult", false, (int)SqlDbType.Char, 1, 0, 0, false, "", null, typeof(System.String), 55 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "InvoiceTermsOfSale", "InvoiceTermsOfSale", true, (int)SqlDbType.VarChar, 3, 0, 0, false, "", null, typeof(System.String), 56 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "InvoiceReasonForExport", "InvoiceReasonForExport", true, (int)SqlDbType.VarChar, 2, 0, 0, false, "", null, typeof(System.String), 57 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "InvoiceComments", "InvoiceComments", true, (int)SqlDbType.NVarChar, 200, 0, 0, false, "", null, typeof(System.String), 58 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "InvoiceCurrencyCode", "InvoiceCurrencyCode", true, (int)SqlDbType.VarChar, 3, 0, 0, false, "", null, typeof(System.String), 59 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "InvoiceChargesFreight", "InvoiceChargesFreight", true, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 60 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "InvoiceChargesInsurance", "InvoiceChargesInsurance", true, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 61 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "InvoiceChargesOther", "InvoiceChargesOther", true, (int)SqlDbType.Money, 0, 4, 19, false, "", null, typeof(System.Decimal), 62 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "ShipmentProcessedOnComputerID", "ShipmentProcessedOnComputerID", true, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 63 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "UspsEndorsement", "UspsEndorsement", true, (int)SqlDbType.NVarChar, 50, 0, 0, false, "", null, typeof(System.String), 64 );
			base.AddElementFieldMapping( "WorldShipShipmentEntity", "CarbonNeutral", "CarbonNeutral", true, (int)SqlDbType.Char, 10, 0, 0, false, "", null, typeof(System.String), 65 );
		}
		/// <summary>Inits YahooOrderEntity's mappings</summary>
		private void InitYahooOrderEntityMappings()
		{
			base.AddElementMapping( "YahooOrderEntity", "ShipWorksLocal", @"dbo", "YahooOrder", 2 );
			base.AddElementFieldMapping( "YahooOrderEntity", "OrderID", "OrderID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "YahooOrderEntity", "YahooOrderID", "YahooOrderID", false, (int)SqlDbType.VarChar, 50, 0, 0, false, "", null, typeof(System.String), 1 );
		}
		/// <summary>Inits YahooOrderItemEntity's mappings</summary>
		private void InitYahooOrderItemEntityMappings()
		{
			base.AddElementMapping( "YahooOrderItemEntity", "ShipWorksLocal", @"dbo", "YahooOrderItem", 2 );
			base.AddElementFieldMapping( "YahooOrderItemEntity", "OrderItemID", "OrderItemID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "YahooOrderItemEntity", "YahooProductID", "YahooProductID", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 1 );
		}
		/// <summary>Inits YahooProductEntity's mappings</summary>
		private void InitYahooProductEntityMappings()
		{
			base.AddElementMapping( "YahooProductEntity", "ShipWorksLocal", @"dbo", "YahooProduct", 3 );
			base.AddElementFieldMapping( "YahooProductEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "YahooProductEntity", "YahooProductID", "YahooProductID", false, (int)SqlDbType.NVarChar, 255, 0, 0, false, "", null, typeof(System.String), 1 );
			base.AddElementFieldMapping( "YahooProductEntity", "Weight", "Weight", false, (int)SqlDbType.Float, 0, 0, 38, false, "", null, typeof(System.Double), 2 );
		}
		/// <summary>Inits YahooStoreEntity's mappings</summary>
		private void InitYahooStoreEntityMappings()
		{
			base.AddElementMapping( "YahooStoreEntity", "ShipWorksLocal", @"dbo", "YahooStore", 3 );
			base.AddElementFieldMapping( "YahooStoreEntity", "StoreID", "StoreID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 0 );
			base.AddElementFieldMapping( "YahooStoreEntity", "YahooEmailAccountID", "YahooEmailAccountID", false, (int)SqlDbType.BigInt, 0, 0, 19, false, "", null, typeof(System.Int64), 1 );
			base.AddElementFieldMapping( "YahooStoreEntity", "TrackingUpdatePassword", "TrackingUpdatePassword", false, (int)SqlDbType.VarChar, 100, 0, 0, false, "", null, typeof(System.String), 2 );
		}

	}
}













