///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.Linq
// Templates vendor: Solutions Design.
//////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using SD.LLBLGen.Pro.LinqSupportClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model.RelationClasses;

namespace ShipWorks.Data.Model.Linq
{
	/// <summary>Meta-data class for the construction of Linq queries which are to be executed using LLBLGen Pro code.</summary>
	public partial class LinqMetaData: ILinqMetaData
	{
		#region Class Member Declarations
		private IDataAccessAdapter _adapterToUse;
		private FunctionMappingStore _customFunctionMappings;
		private Context _contextToUse;
		#endregion
		
		/// <summary>CTor. Using this ctor will leave the IDataAccessAdapter object to use empty. To be able to execute the query, an IDataAccessAdapter instance
		/// is required, and has to be set on the LLBLGenProProvider2 object in the query to execute. </summary>
		public LinqMetaData() : this(null, null)
		{
		}
		
		/// <summary>CTor which accepts an IDataAccessAdapter implementing object, which will be used to execute queries created with this metadata class.</summary>
		/// <param name="adapterToUse">the IDataAccessAdapter to use in queries created with this meta data</param>
		/// <remarks> Be aware that the IDataAccessAdapter object set via this property is kept alive by the LLBLGenProQuery objects created with this meta data
		/// till they go out of scope.</remarks>
		public LinqMetaData(IDataAccessAdapter adapterToUse) : this (adapterToUse, null)
		{
		}

		/// <summary>CTor which accepts an IDataAccessAdapter implementing object, which will be used to execute queries created with this metadata class.</summary>
		/// <param name="adapterToUse">the IDataAccessAdapter to use in queries created with this meta data</param>
		/// <param name="customFunctionMappings">The custom function mappings to use. These take higher precedence than the ones in the DQE to use.</param>
		/// <remarks> Be aware that the IDataAccessAdapter object set via this property is kept alive by the LLBLGenProQuery objects created with this meta data
		/// till they go out of scope.</remarks>
		public LinqMetaData(IDataAccessAdapter adapterToUse, FunctionMappingStore customFunctionMappings)
		{
			_adapterToUse = adapterToUse;
			_customFunctionMappings = customFunctionMappings;
		}
	
		/// <summary>returns the datasource to use in a Linq query for the entity type specified</summary>
		/// <param name="typeOfEntity">the type of the entity to get the datasource for</param>
		/// <returns>the requested datasource</returns>
		public IDataSource GetQueryableForEntity(int typeOfEntity)
		{
			IDataSource toReturn = null;
			switch((ShipWorks.Data.Model.EntityType)typeOfEntity)
			{
				case ShipWorks.Data.Model.EntityType.ActionEntity:
					toReturn = this.Action;
					break;
				case ShipWorks.Data.Model.EntityType.ActionFilterTriggerEntity:
					toReturn = this.ActionFilterTrigger;
					break;
				case ShipWorks.Data.Model.EntityType.ActionQueueEntity:
					toReturn = this.ActionQueue;
					break;
				case ShipWorks.Data.Model.EntityType.ActionQueueSelectionEntity:
					toReturn = this.ActionQueueSelection;
					break;
				case ShipWorks.Data.Model.EntityType.ActionQueueStepEntity:
					toReturn = this.ActionQueueStep;
					break;
				case ShipWorks.Data.Model.EntityType.ActionTaskEntity:
					toReturn = this.ActionTask;
					break;
				case ShipWorks.Data.Model.EntityType.AmazonASINEntity:
					toReturn = this.AmazonASIN;
					break;
				case ShipWorks.Data.Model.EntityType.AmazonOrderEntity:
					toReturn = this.AmazonOrder;
					break;
				case ShipWorks.Data.Model.EntityType.AmazonOrderItemEntity:
					toReturn = this.AmazonOrderItem;
					break;
				case ShipWorks.Data.Model.EntityType.AmazonStoreEntity:
					toReturn = this.AmazonStore;
					break;
				case ShipWorks.Data.Model.EntityType.AmeriCommerceStoreEntity:
					toReturn = this.AmeriCommerceStore;
					break;
				case ShipWorks.Data.Model.EntityType.AuditEntity:
					toReturn = this.Audit;
					break;
				case ShipWorks.Data.Model.EntityType.AuditChangeEntity:
					toReturn = this.AuditChange;
					break;
				case ShipWorks.Data.Model.EntityType.AuditChangeDetailEntity:
					toReturn = this.AuditChangeDetail;
					break;
				case ShipWorks.Data.Model.EntityType.BestRateProfileEntity:
					toReturn = this.BestRateProfile;
					break;
				case ShipWorks.Data.Model.EntityType.BestRateShipmentEntity:
					toReturn = this.BestRateShipment;
					break;
				case ShipWorks.Data.Model.EntityType.BigCommerceOrderItemEntity:
					toReturn = this.BigCommerceOrderItem;
					break;
				case ShipWorks.Data.Model.EntityType.BigCommerceStoreEntity:
					toReturn = this.BigCommerceStore;
					break;
				case ShipWorks.Data.Model.EntityType.BuyDotComOrderItemEntity:
					toReturn = this.BuyDotComOrderItem;
					break;
				case ShipWorks.Data.Model.EntityType.BuyDotComStoreEntity:
					toReturn = this.BuyDotComStore;
					break;
				case ShipWorks.Data.Model.EntityType.ChannelAdvisorOrderEntity:
					toReturn = this.ChannelAdvisorOrder;
					break;
				case ShipWorks.Data.Model.EntityType.ChannelAdvisorOrderItemEntity:
					toReturn = this.ChannelAdvisorOrderItem;
					break;
				case ShipWorks.Data.Model.EntityType.ChannelAdvisorStoreEntity:
					toReturn = this.ChannelAdvisorStore;
					break;
				case ShipWorks.Data.Model.EntityType.ClickCartProOrderEntity:
					toReturn = this.ClickCartProOrder;
					break;
				case ShipWorks.Data.Model.EntityType.CommerceInterfaceOrderEntity:
					toReturn = this.CommerceInterfaceOrder;
					break;
				case ShipWorks.Data.Model.EntityType.ComputerEntity:
					toReturn = this.Computer;
					break;
				case ShipWorks.Data.Model.EntityType.ConfigurationEntity:
					toReturn = this.Configuration;
					break;
				case ShipWorks.Data.Model.EntityType.CustomerEntity:
					toReturn = this.Customer;
					break;
				case ShipWorks.Data.Model.EntityType.DimensionsProfileEntity:
					toReturn = this.DimensionsProfile;
					break;
				case ShipWorks.Data.Model.EntityType.DownloadEntity:
					toReturn = this.Download;
					break;
				case ShipWorks.Data.Model.EntityType.DownloadDetailEntity:
					toReturn = this.DownloadDetail;
					break;
				case ShipWorks.Data.Model.EntityType.EbayCombinedOrderRelationEntity:
					toReturn = this.EbayCombinedOrderRelation;
					break;
				case ShipWorks.Data.Model.EntityType.EbayOrderEntity:
					toReturn = this.EbayOrder;
					break;
				case ShipWorks.Data.Model.EntityType.EbayOrderItemEntity:
					toReturn = this.EbayOrderItem;
					break;
				case ShipWorks.Data.Model.EntityType.EbayStoreEntity:
					toReturn = this.EbayStore;
					break;
				case ShipWorks.Data.Model.EntityType.EmailAccountEntity:
					toReturn = this.EmailAccount;
					break;
				case ShipWorks.Data.Model.EntityType.EmailOutboundEntity:
					toReturn = this.EmailOutbound;
					break;
				case ShipWorks.Data.Model.EntityType.EmailOutboundRelationEntity:
					toReturn = this.EmailOutboundRelation;
					break;
				case ShipWorks.Data.Model.EntityType.EndiciaAccountEntity:
					toReturn = this.EndiciaAccount;
					break;
				case ShipWorks.Data.Model.EntityType.EndiciaProfileEntity:
					toReturn = this.EndiciaProfile;
					break;
				case ShipWorks.Data.Model.EntityType.EndiciaScanFormEntity:
					toReturn = this.EndiciaScanForm;
					break;
				case ShipWorks.Data.Model.EntityType.EndiciaShipmentEntity:
					toReturn = this.EndiciaShipment;
					break;
				case ShipWorks.Data.Model.EntityType.EtsyOrderEntity:
					toReturn = this.EtsyOrder;
					break;
				case ShipWorks.Data.Model.EntityType.EtsyStoreEntity:
					toReturn = this.EtsyStore;
					break;
				case ShipWorks.Data.Model.EntityType.FedExAccountEntity:
					toReturn = this.FedExAccount;
					break;
				case ShipWorks.Data.Model.EntityType.FedExEndOfDayCloseEntity:
					toReturn = this.FedExEndOfDayClose;
					break;
				case ShipWorks.Data.Model.EntityType.FedExPackageEntity:
					toReturn = this.FedExPackage;
					break;
				case ShipWorks.Data.Model.EntityType.FedExProfileEntity:
					toReturn = this.FedExProfile;
					break;
				case ShipWorks.Data.Model.EntityType.FedExProfilePackageEntity:
					toReturn = this.FedExProfilePackage;
					break;
				case ShipWorks.Data.Model.EntityType.FedExShipmentEntity:
					toReturn = this.FedExShipment;
					break;
				case ShipWorks.Data.Model.EntityType.FilterEntity:
					toReturn = this.Filter;
					break;
				case ShipWorks.Data.Model.EntityType.FilterLayoutEntity:
					toReturn = this.FilterLayout;
					break;
				case ShipWorks.Data.Model.EntityType.FilterNodeEntity:
					toReturn = this.FilterNode;
					break;
				case ShipWorks.Data.Model.EntityType.FilterNodeColumnSettingsEntity:
					toReturn = this.FilterNodeColumnSettings;
					break;
				case ShipWorks.Data.Model.EntityType.FilterNodeContentEntity:
					toReturn = this.FilterNodeContent;
					break;
				case ShipWorks.Data.Model.EntityType.FilterNodeContentDetailEntity:
					toReturn = this.FilterNodeContentDetail;
					break;
				case ShipWorks.Data.Model.EntityType.FilterSequenceEntity:
					toReturn = this.FilterSequence;
					break;
				case ShipWorks.Data.Model.EntityType.FtpAccountEntity:
					toReturn = this.FtpAccount;
					break;
				case ShipWorks.Data.Model.EntityType.GenericFileStoreEntity:
					toReturn = this.GenericFileStore;
					break;
				case ShipWorks.Data.Model.EntityType.GenericModuleStoreEntity:
					toReturn = this.GenericModuleStore;
					break;
				case ShipWorks.Data.Model.EntityType.GridColumnFormatEntity:
					toReturn = this.GridColumnFormat;
					break;
				case ShipWorks.Data.Model.EntityType.GridColumnLayoutEntity:
					toReturn = this.GridColumnLayout;
					break;
				case ShipWorks.Data.Model.EntityType.GridColumnPositionEntity:
					toReturn = this.GridColumnPosition;
					break;
				case ShipWorks.Data.Model.EntityType.GrouponOrderEntity:
					toReturn = this.GrouponOrder;
					break;
				case ShipWorks.Data.Model.EntityType.GrouponOrderItemEntity:
					toReturn = this.GrouponOrderItem;
					break;
				case ShipWorks.Data.Model.EntityType.GrouponStoreEntity:
					toReturn = this.GrouponStore;
					break;
				case ShipWorks.Data.Model.EntityType.InfopiaOrderItemEntity:
					toReturn = this.InfopiaOrderItem;
					break;
				case ShipWorks.Data.Model.EntityType.InfopiaStoreEntity:
					toReturn = this.InfopiaStore;
					break;
				case ShipWorks.Data.Model.EntityType.InsurancePolicyEntity:
					toReturn = this.InsurancePolicy;
					break;
				case ShipWorks.Data.Model.EntityType.IParcelAccountEntity:
					toReturn = this.IParcelAccount;
					break;
				case ShipWorks.Data.Model.EntityType.IParcelPackageEntity:
					toReturn = this.IParcelPackage;
					break;
				case ShipWorks.Data.Model.EntityType.IParcelProfileEntity:
					toReturn = this.IParcelProfile;
					break;
				case ShipWorks.Data.Model.EntityType.IParcelProfilePackageEntity:
					toReturn = this.IParcelProfilePackage;
					break;
				case ShipWorks.Data.Model.EntityType.IParcelShipmentEntity:
					toReturn = this.IParcelShipment;
					break;
				case ShipWorks.Data.Model.EntityType.LabelSheetEntity:
					toReturn = this.LabelSheet;
					break;
				case ShipWorks.Data.Model.EntityType.MagentoOrderEntity:
					toReturn = this.MagentoOrder;
					break;
				case ShipWorks.Data.Model.EntityType.MagentoStoreEntity:
					toReturn = this.MagentoStore;
					break;
				case ShipWorks.Data.Model.EntityType.MarketplaceAdvisorOrderEntity:
					toReturn = this.MarketplaceAdvisorOrder;
					break;
				case ShipWorks.Data.Model.EntityType.MarketplaceAdvisorStoreEntity:
					toReturn = this.MarketplaceAdvisorStore;
					break;
				case ShipWorks.Data.Model.EntityType.MivaOrderItemAttributeEntity:
					toReturn = this.MivaOrderItemAttribute;
					break;
				case ShipWorks.Data.Model.EntityType.MivaStoreEntity:
					toReturn = this.MivaStore;
					break;
				case ShipWorks.Data.Model.EntityType.NetworkSolutionsOrderEntity:
					toReturn = this.NetworkSolutionsOrder;
					break;
				case ShipWorks.Data.Model.EntityType.NetworkSolutionsStoreEntity:
					toReturn = this.NetworkSolutionsStore;
					break;
				case ShipWorks.Data.Model.EntityType.NeweggOrderEntity:
					toReturn = this.NeweggOrder;
					break;
				case ShipWorks.Data.Model.EntityType.NeweggOrderItemEntity:
					toReturn = this.NeweggOrderItem;
					break;
				case ShipWorks.Data.Model.EntityType.NeweggStoreEntity:
					toReturn = this.NeweggStore;
					break;
				case ShipWorks.Data.Model.EntityType.NoteEntity:
					toReturn = this.Note;
					break;
				case ShipWorks.Data.Model.EntityType.ObjectLabelEntity:
					toReturn = this.ObjectLabel;
					break;
				case ShipWorks.Data.Model.EntityType.ObjectReferenceEntity:
					toReturn = this.ObjectReference;
					break;
				case ShipWorks.Data.Model.EntityType.OnTracAccountEntity:
					toReturn = this.OnTracAccount;
					break;
				case ShipWorks.Data.Model.EntityType.OnTracProfileEntity:
					toReturn = this.OnTracProfile;
					break;
				case ShipWorks.Data.Model.EntityType.OnTracShipmentEntity:
					toReturn = this.OnTracShipment;
					break;
				case ShipWorks.Data.Model.EntityType.OrderEntity:
					toReturn = this.Order;
					break;
				case ShipWorks.Data.Model.EntityType.OrderChargeEntity:
					toReturn = this.OrderCharge;
					break;
				case ShipWorks.Data.Model.EntityType.OrderItemEntity:
					toReturn = this.OrderItem;
					break;
				case ShipWorks.Data.Model.EntityType.OrderItemAttributeEntity:
					toReturn = this.OrderItemAttribute;
					break;
				case ShipWorks.Data.Model.EntityType.OrderMotionOrderEntity:
					toReturn = this.OrderMotionOrder;
					break;
				case ShipWorks.Data.Model.EntityType.OrderMotionStoreEntity:
					toReturn = this.OrderMotionStore;
					break;
				case ShipWorks.Data.Model.EntityType.OrderPaymentDetailEntity:
					toReturn = this.OrderPaymentDetail;
					break;
				case ShipWorks.Data.Model.EntityType.OtherProfileEntity:
					toReturn = this.OtherProfile;
					break;
				case ShipWorks.Data.Model.EntityType.OtherShipmentEntity:
					toReturn = this.OtherShipment;
					break;
				case ShipWorks.Data.Model.EntityType.PayPalOrderEntity:
					toReturn = this.PayPalOrder;
					break;
				case ShipWorks.Data.Model.EntityType.PayPalStoreEntity:
					toReturn = this.PayPalStore;
					break;
				case ShipWorks.Data.Model.EntityType.PermissionEntity:
					toReturn = this.Permission;
					break;
				case ShipWorks.Data.Model.EntityType.PostalProfileEntity:
					toReturn = this.PostalProfile;
					break;
				case ShipWorks.Data.Model.EntityType.PostalShipmentEntity:
					toReturn = this.PostalShipment;
					break;
				case ShipWorks.Data.Model.EntityType.PrintResultEntity:
					toReturn = this.PrintResult;
					break;
				case ShipWorks.Data.Model.EntityType.ProStoresOrderEntity:
					toReturn = this.ProStoresOrder;
					break;
				case ShipWorks.Data.Model.EntityType.ProStoresStoreEntity:
					toReturn = this.ProStoresStore;
					break;
				case ShipWorks.Data.Model.EntityType.ResourceEntity:
					toReturn = this.Resource;
					break;
				case ShipWorks.Data.Model.EntityType.ScanFormBatchEntity:
					toReturn = this.ScanFormBatch;
					break;
				case ShipWorks.Data.Model.EntityType.SearchEntity:
					toReturn = this.Search;
					break;
				case ShipWorks.Data.Model.EntityType.SearsOrderEntity:
					toReturn = this.SearsOrder;
					break;
				case ShipWorks.Data.Model.EntityType.SearsOrderItemEntity:
					toReturn = this.SearsOrderItem;
					break;
				case ShipWorks.Data.Model.EntityType.SearsStoreEntity:
					toReturn = this.SearsStore;
					break;
				case ShipWorks.Data.Model.EntityType.ServerMessageEntity:
					toReturn = this.ServerMessage;
					break;
				case ShipWorks.Data.Model.EntityType.ServerMessageSignoffEntity:
					toReturn = this.ServerMessageSignoff;
					break;
				case ShipWorks.Data.Model.EntityType.ServiceStatusEntity:
					toReturn = this.ServiceStatus;
					break;
				case ShipWorks.Data.Model.EntityType.ShipmentEntity:
					toReturn = this.Shipment;
					break;
				case ShipWorks.Data.Model.EntityType.ShipmentCustomsItemEntity:
					toReturn = this.ShipmentCustomsItem;
					break;
				case ShipWorks.Data.Model.EntityType.ShippingDefaultsRuleEntity:
					toReturn = this.ShippingDefaultsRule;
					break;
				case ShipWorks.Data.Model.EntityType.ShippingOriginEntity:
					toReturn = this.ShippingOrigin;
					break;
				case ShipWorks.Data.Model.EntityType.ShippingPrintOutputEntity:
					toReturn = this.ShippingPrintOutput;
					break;
				case ShipWorks.Data.Model.EntityType.ShippingPrintOutputRuleEntity:
					toReturn = this.ShippingPrintOutputRule;
					break;
				case ShipWorks.Data.Model.EntityType.ShippingProfileEntity:
					toReturn = this.ShippingProfile;
					break;
				case ShipWorks.Data.Model.EntityType.ShippingProviderRuleEntity:
					toReturn = this.ShippingProviderRule;
					break;
				case ShipWorks.Data.Model.EntityType.ShippingSettingsEntity:
					toReturn = this.ShippingSettings;
					break;
				case ShipWorks.Data.Model.EntityType.ShipSenseKnowledgebaseEntity:
					toReturn = this.ShipSenseKnowledgebase;
					break;
				case ShipWorks.Data.Model.EntityType.ShopifyOrderEntity:
					toReturn = this.ShopifyOrder;
					break;
				case ShipWorks.Data.Model.EntityType.ShopifyOrderItemEntity:
					toReturn = this.ShopifyOrderItem;
					break;
				case ShipWorks.Data.Model.EntityType.ShopifyStoreEntity:
					toReturn = this.ShopifyStore;
					break;
				case ShipWorks.Data.Model.EntityType.ShopSiteStoreEntity:
					toReturn = this.ShopSiteStore;
					break;
				case ShipWorks.Data.Model.EntityType.StampsAccountEntity:
					toReturn = this.StampsAccount;
					break;
				case ShipWorks.Data.Model.EntityType.StampsProfileEntity:
					toReturn = this.StampsProfile;
					break;
				case ShipWorks.Data.Model.EntityType.StampsScanFormEntity:
					toReturn = this.StampsScanForm;
					break;
				case ShipWorks.Data.Model.EntityType.StampsShipmentEntity:
					toReturn = this.StampsShipment;
					break;
				case ShipWorks.Data.Model.EntityType.StatusPresetEntity:
					toReturn = this.StatusPreset;
					break;
				case ShipWorks.Data.Model.EntityType.StoreEntity:
					toReturn = this.Store;
					break;
				case ShipWorks.Data.Model.EntityType.SystemDataEntity:
					toReturn = this.SystemData;
					break;
				case ShipWorks.Data.Model.EntityType.TemplateEntity:
					toReturn = this.Template;
					break;
				case ShipWorks.Data.Model.EntityType.TemplateComputerSettingsEntity:
					toReturn = this.TemplateComputerSettings;
					break;
				case ShipWorks.Data.Model.EntityType.TemplateFolderEntity:
					toReturn = this.TemplateFolder;
					break;
				case ShipWorks.Data.Model.EntityType.TemplateStoreSettingsEntity:
					toReturn = this.TemplateStoreSettings;
					break;
				case ShipWorks.Data.Model.EntityType.TemplateUserSettingsEntity:
					toReturn = this.TemplateUserSettings;
					break;
				case ShipWorks.Data.Model.EntityType.ThreeDCartOrderItemEntity:
					toReturn = this.ThreeDCartOrderItem;
					break;
				case ShipWorks.Data.Model.EntityType.ThreeDCartStoreEntity:
					toReturn = this.ThreeDCartStore;
					break;
				case ShipWorks.Data.Model.EntityType.UpsAccountEntity:
					toReturn = this.UpsAccount;
					break;
				case ShipWorks.Data.Model.EntityType.UpsPackageEntity:
					toReturn = this.UpsPackage;
					break;
				case ShipWorks.Data.Model.EntityType.UpsProfileEntity:
					toReturn = this.UpsProfile;
					break;
				case ShipWorks.Data.Model.EntityType.UpsProfilePackageEntity:
					toReturn = this.UpsProfilePackage;
					break;
				case ShipWorks.Data.Model.EntityType.UpsShipmentEntity:
					toReturn = this.UpsShipment;
					break;
				case ShipWorks.Data.Model.EntityType.UserEntity:
					toReturn = this.User;
					break;
				case ShipWorks.Data.Model.EntityType.UserColumnSettingsEntity:
					toReturn = this.UserColumnSettings;
					break;
				case ShipWorks.Data.Model.EntityType.UserSettingsEntity:
					toReturn = this.UserSettings;
					break;
				case ShipWorks.Data.Model.EntityType.VersionSignoffEntity:
					toReturn = this.VersionSignoff;
					break;
				case ShipWorks.Data.Model.EntityType.VolusionStoreEntity:
					toReturn = this.VolusionStore;
					break;
				case ShipWorks.Data.Model.EntityType.WorldShipGoodsEntity:
					toReturn = this.WorldShipGoods;
					break;
				case ShipWorks.Data.Model.EntityType.WorldShipPackageEntity:
					toReturn = this.WorldShipPackage;
					break;
				case ShipWorks.Data.Model.EntityType.WorldShipProcessedEntity:
					toReturn = this.WorldShipProcessed;
					break;
				case ShipWorks.Data.Model.EntityType.WorldShipShipmentEntity:
					toReturn = this.WorldShipShipment;
					break;
				case ShipWorks.Data.Model.EntityType.YahooOrderEntity:
					toReturn = this.YahooOrder;
					break;
				case ShipWorks.Data.Model.EntityType.YahooOrderItemEntity:
					toReturn = this.YahooOrderItem;
					break;
				case ShipWorks.Data.Model.EntityType.YahooProductEntity:
					toReturn = this.YahooProduct;
					break;
				case ShipWorks.Data.Model.EntityType.YahooStoreEntity:
					toReturn = this.YahooStore;
					break;
				default:
					toReturn = null;
					break;
			}
			return toReturn;
		}

		/// <summary>returns the datasource to use in a Linq query when targeting ActionEntity instances in the database.</summary>
		public DataSource2<ActionEntity> Action
		{
			get { return new DataSource2<ActionEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ActionFilterTriggerEntity instances in the database.</summary>
		public DataSource2<ActionFilterTriggerEntity> ActionFilterTrigger
		{
			get { return new DataSource2<ActionFilterTriggerEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ActionQueueEntity instances in the database.</summary>
		public DataSource2<ActionQueueEntity> ActionQueue
		{
			get { return new DataSource2<ActionQueueEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ActionQueueSelectionEntity instances in the database.</summary>
		public DataSource2<ActionQueueSelectionEntity> ActionQueueSelection
		{
			get { return new DataSource2<ActionQueueSelectionEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ActionQueueStepEntity instances in the database.</summary>
		public DataSource2<ActionQueueStepEntity> ActionQueueStep
		{
			get { return new DataSource2<ActionQueueStepEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ActionTaskEntity instances in the database.</summary>
		public DataSource2<ActionTaskEntity> ActionTask
		{
			get { return new DataSource2<ActionTaskEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting AmazonASINEntity instances in the database.</summary>
		public DataSource2<AmazonASINEntity> AmazonASIN
		{
			get { return new DataSource2<AmazonASINEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting AmazonOrderEntity instances in the database.</summary>
		public DataSource2<AmazonOrderEntity> AmazonOrder
		{
			get { return new DataSource2<AmazonOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting AmazonOrderItemEntity instances in the database.</summary>
		public DataSource2<AmazonOrderItemEntity> AmazonOrderItem
		{
			get { return new DataSource2<AmazonOrderItemEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting AmazonStoreEntity instances in the database.</summary>
		public DataSource2<AmazonStoreEntity> AmazonStore
		{
			get { return new DataSource2<AmazonStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting AmeriCommerceStoreEntity instances in the database.</summary>
		public DataSource2<AmeriCommerceStoreEntity> AmeriCommerceStore
		{
			get { return new DataSource2<AmeriCommerceStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting AuditEntity instances in the database.</summary>
		public DataSource2<AuditEntity> Audit
		{
			get { return new DataSource2<AuditEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting AuditChangeEntity instances in the database.</summary>
		public DataSource2<AuditChangeEntity> AuditChange
		{
			get { return new DataSource2<AuditChangeEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting AuditChangeDetailEntity instances in the database.</summary>
		public DataSource2<AuditChangeDetailEntity> AuditChangeDetail
		{
			get { return new DataSource2<AuditChangeDetailEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting BestRateProfileEntity instances in the database.</summary>
		public DataSource2<BestRateProfileEntity> BestRateProfile
		{
			get { return new DataSource2<BestRateProfileEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting BestRateShipmentEntity instances in the database.</summary>
		public DataSource2<BestRateShipmentEntity> BestRateShipment
		{
			get { return new DataSource2<BestRateShipmentEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting BigCommerceOrderItemEntity instances in the database.</summary>
		public DataSource2<BigCommerceOrderItemEntity> BigCommerceOrderItem
		{
			get { return new DataSource2<BigCommerceOrderItemEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting BigCommerceStoreEntity instances in the database.</summary>
		public DataSource2<BigCommerceStoreEntity> BigCommerceStore
		{
			get { return new DataSource2<BigCommerceStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting BuyDotComOrderItemEntity instances in the database.</summary>
		public DataSource2<BuyDotComOrderItemEntity> BuyDotComOrderItem
		{
			get { return new DataSource2<BuyDotComOrderItemEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting BuyDotComStoreEntity instances in the database.</summary>
		public DataSource2<BuyDotComStoreEntity> BuyDotComStore
		{
			get { return new DataSource2<BuyDotComStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ChannelAdvisorOrderEntity instances in the database.</summary>
		public DataSource2<ChannelAdvisorOrderEntity> ChannelAdvisorOrder
		{
			get { return new DataSource2<ChannelAdvisorOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ChannelAdvisorOrderItemEntity instances in the database.</summary>
		public DataSource2<ChannelAdvisorOrderItemEntity> ChannelAdvisorOrderItem
		{
			get { return new DataSource2<ChannelAdvisorOrderItemEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ChannelAdvisorStoreEntity instances in the database.</summary>
		public DataSource2<ChannelAdvisorStoreEntity> ChannelAdvisorStore
		{
			get { return new DataSource2<ChannelAdvisorStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ClickCartProOrderEntity instances in the database.</summary>
		public DataSource2<ClickCartProOrderEntity> ClickCartProOrder
		{
			get { return new DataSource2<ClickCartProOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting CommerceInterfaceOrderEntity instances in the database.</summary>
		public DataSource2<CommerceInterfaceOrderEntity> CommerceInterfaceOrder
		{
			get { return new DataSource2<CommerceInterfaceOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ComputerEntity instances in the database.</summary>
		public DataSource2<ComputerEntity> Computer
		{
			get { return new DataSource2<ComputerEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ConfigurationEntity instances in the database.</summary>
		public DataSource2<ConfigurationEntity> Configuration
		{
			get { return new DataSource2<ConfigurationEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting CustomerEntity instances in the database.</summary>
		public DataSource2<CustomerEntity> Customer
		{
			get { return new DataSource2<CustomerEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting DimensionsProfileEntity instances in the database.</summary>
		public DataSource2<DimensionsProfileEntity> DimensionsProfile
		{
			get { return new DataSource2<DimensionsProfileEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting DownloadEntity instances in the database.</summary>
		public DataSource2<DownloadEntity> Download
		{
			get { return new DataSource2<DownloadEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting DownloadDetailEntity instances in the database.</summary>
		public DataSource2<DownloadDetailEntity> DownloadDetail
		{
			get { return new DataSource2<DownloadDetailEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting EbayCombinedOrderRelationEntity instances in the database.</summary>
		public DataSource2<EbayCombinedOrderRelationEntity> EbayCombinedOrderRelation
		{
			get { return new DataSource2<EbayCombinedOrderRelationEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting EbayOrderEntity instances in the database.</summary>
		public DataSource2<EbayOrderEntity> EbayOrder
		{
			get { return new DataSource2<EbayOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting EbayOrderItemEntity instances in the database.</summary>
		public DataSource2<EbayOrderItemEntity> EbayOrderItem
		{
			get { return new DataSource2<EbayOrderItemEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting EbayStoreEntity instances in the database.</summary>
		public DataSource2<EbayStoreEntity> EbayStore
		{
			get { return new DataSource2<EbayStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting EmailAccountEntity instances in the database.</summary>
		public DataSource2<EmailAccountEntity> EmailAccount
		{
			get { return new DataSource2<EmailAccountEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting EmailOutboundEntity instances in the database.</summary>
		public DataSource2<EmailOutboundEntity> EmailOutbound
		{
			get { return new DataSource2<EmailOutboundEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting EmailOutboundRelationEntity instances in the database.</summary>
		public DataSource2<EmailOutboundRelationEntity> EmailOutboundRelation
		{
			get { return new DataSource2<EmailOutboundRelationEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting EndiciaAccountEntity instances in the database.</summary>
		public DataSource2<EndiciaAccountEntity> EndiciaAccount
		{
			get { return new DataSource2<EndiciaAccountEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting EndiciaProfileEntity instances in the database.</summary>
		public DataSource2<EndiciaProfileEntity> EndiciaProfile
		{
			get { return new DataSource2<EndiciaProfileEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting EndiciaScanFormEntity instances in the database.</summary>
		public DataSource2<EndiciaScanFormEntity> EndiciaScanForm
		{
			get { return new DataSource2<EndiciaScanFormEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting EndiciaShipmentEntity instances in the database.</summary>
		public DataSource2<EndiciaShipmentEntity> EndiciaShipment
		{
			get { return new DataSource2<EndiciaShipmentEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting EtsyOrderEntity instances in the database.</summary>
		public DataSource2<EtsyOrderEntity> EtsyOrder
		{
			get { return new DataSource2<EtsyOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting EtsyStoreEntity instances in the database.</summary>
		public DataSource2<EtsyStoreEntity> EtsyStore
		{
			get { return new DataSource2<EtsyStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting FedExAccountEntity instances in the database.</summary>
		public DataSource2<FedExAccountEntity> FedExAccount
		{
			get { return new DataSource2<FedExAccountEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting FedExEndOfDayCloseEntity instances in the database.</summary>
		public DataSource2<FedExEndOfDayCloseEntity> FedExEndOfDayClose
		{
			get { return new DataSource2<FedExEndOfDayCloseEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting FedExPackageEntity instances in the database.</summary>
		public DataSource2<FedExPackageEntity> FedExPackage
		{
			get { return new DataSource2<FedExPackageEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting FedExProfileEntity instances in the database.</summary>
		public DataSource2<FedExProfileEntity> FedExProfile
		{
			get { return new DataSource2<FedExProfileEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting FedExProfilePackageEntity instances in the database.</summary>
		public DataSource2<FedExProfilePackageEntity> FedExProfilePackage
		{
			get { return new DataSource2<FedExProfilePackageEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting FedExShipmentEntity instances in the database.</summary>
		public DataSource2<FedExShipmentEntity> FedExShipment
		{
			get { return new DataSource2<FedExShipmentEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting FilterEntity instances in the database.</summary>
		public DataSource2<FilterEntity> Filter
		{
			get { return new DataSource2<FilterEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting FilterLayoutEntity instances in the database.</summary>
		public DataSource2<FilterLayoutEntity> FilterLayout
		{
			get { return new DataSource2<FilterLayoutEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting FilterNodeEntity instances in the database.</summary>
		public DataSource2<FilterNodeEntity> FilterNode
		{
			get { return new DataSource2<FilterNodeEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting FilterNodeColumnSettingsEntity instances in the database.</summary>
		public DataSource2<FilterNodeColumnSettingsEntity> FilterNodeColumnSettings
		{
			get { return new DataSource2<FilterNodeColumnSettingsEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting FilterNodeContentEntity instances in the database.</summary>
		public DataSource2<FilterNodeContentEntity> FilterNodeContent
		{
			get { return new DataSource2<FilterNodeContentEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting FilterNodeContentDetailEntity instances in the database.</summary>
		public DataSource2<FilterNodeContentDetailEntity> FilterNodeContentDetail
		{
			get { return new DataSource2<FilterNodeContentDetailEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting FilterSequenceEntity instances in the database.</summary>
		public DataSource2<FilterSequenceEntity> FilterSequence
		{
			get { return new DataSource2<FilterSequenceEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting FtpAccountEntity instances in the database.</summary>
		public DataSource2<FtpAccountEntity> FtpAccount
		{
			get { return new DataSource2<FtpAccountEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting GenericFileStoreEntity instances in the database.</summary>
		public DataSource2<GenericFileStoreEntity> GenericFileStore
		{
			get { return new DataSource2<GenericFileStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting GenericModuleStoreEntity instances in the database.</summary>
		public DataSource2<GenericModuleStoreEntity> GenericModuleStore
		{
			get { return new DataSource2<GenericModuleStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting GridColumnFormatEntity instances in the database.</summary>
		public DataSource2<GridColumnFormatEntity> GridColumnFormat
		{
			get { return new DataSource2<GridColumnFormatEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting GridColumnLayoutEntity instances in the database.</summary>
		public DataSource2<GridColumnLayoutEntity> GridColumnLayout
		{
			get { return new DataSource2<GridColumnLayoutEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting GridColumnPositionEntity instances in the database.</summary>
		public DataSource2<GridColumnPositionEntity> GridColumnPosition
		{
			get { return new DataSource2<GridColumnPositionEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting GrouponOrderEntity instances in the database.</summary>
		public DataSource2<GrouponOrderEntity> GrouponOrder
		{
			get { return new DataSource2<GrouponOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting GrouponOrderItemEntity instances in the database.</summary>
		public DataSource2<GrouponOrderItemEntity> GrouponOrderItem
		{
			get { return new DataSource2<GrouponOrderItemEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting GrouponStoreEntity instances in the database.</summary>
		public DataSource2<GrouponStoreEntity> GrouponStore
		{
			get { return new DataSource2<GrouponStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting InfopiaOrderItemEntity instances in the database.</summary>
		public DataSource2<InfopiaOrderItemEntity> InfopiaOrderItem
		{
			get { return new DataSource2<InfopiaOrderItemEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting InfopiaStoreEntity instances in the database.</summary>
		public DataSource2<InfopiaStoreEntity> InfopiaStore
		{
			get { return new DataSource2<InfopiaStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting InsurancePolicyEntity instances in the database.</summary>
		public DataSource2<InsurancePolicyEntity> InsurancePolicy
		{
			get { return new DataSource2<InsurancePolicyEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting IParcelAccountEntity instances in the database.</summary>
		public DataSource2<IParcelAccountEntity> IParcelAccount
		{
			get { return new DataSource2<IParcelAccountEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting IParcelPackageEntity instances in the database.</summary>
		public DataSource2<IParcelPackageEntity> IParcelPackage
		{
			get { return new DataSource2<IParcelPackageEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting IParcelProfileEntity instances in the database.</summary>
		public DataSource2<IParcelProfileEntity> IParcelProfile
		{
			get { return new DataSource2<IParcelProfileEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting IParcelProfilePackageEntity instances in the database.</summary>
		public DataSource2<IParcelProfilePackageEntity> IParcelProfilePackage
		{
			get { return new DataSource2<IParcelProfilePackageEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting IParcelShipmentEntity instances in the database.</summary>
		public DataSource2<IParcelShipmentEntity> IParcelShipment
		{
			get { return new DataSource2<IParcelShipmentEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting LabelSheetEntity instances in the database.</summary>
		public DataSource2<LabelSheetEntity> LabelSheet
		{
			get { return new DataSource2<LabelSheetEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting MagentoOrderEntity instances in the database.</summary>
		public DataSource2<MagentoOrderEntity> MagentoOrder
		{
			get { return new DataSource2<MagentoOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting MagentoStoreEntity instances in the database.</summary>
		public DataSource2<MagentoStoreEntity> MagentoStore
		{
			get { return new DataSource2<MagentoStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting MarketplaceAdvisorOrderEntity instances in the database.</summary>
		public DataSource2<MarketplaceAdvisorOrderEntity> MarketplaceAdvisorOrder
		{
			get { return new DataSource2<MarketplaceAdvisorOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting MarketplaceAdvisorStoreEntity instances in the database.</summary>
		public DataSource2<MarketplaceAdvisorStoreEntity> MarketplaceAdvisorStore
		{
			get { return new DataSource2<MarketplaceAdvisorStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting MivaOrderItemAttributeEntity instances in the database.</summary>
		public DataSource2<MivaOrderItemAttributeEntity> MivaOrderItemAttribute
		{
			get { return new DataSource2<MivaOrderItemAttributeEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting MivaStoreEntity instances in the database.</summary>
		public DataSource2<MivaStoreEntity> MivaStore
		{
			get { return new DataSource2<MivaStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting NetworkSolutionsOrderEntity instances in the database.</summary>
		public DataSource2<NetworkSolutionsOrderEntity> NetworkSolutionsOrder
		{
			get { return new DataSource2<NetworkSolutionsOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting NetworkSolutionsStoreEntity instances in the database.</summary>
		public DataSource2<NetworkSolutionsStoreEntity> NetworkSolutionsStore
		{
			get { return new DataSource2<NetworkSolutionsStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting NeweggOrderEntity instances in the database.</summary>
		public DataSource2<NeweggOrderEntity> NeweggOrder
		{
			get { return new DataSource2<NeweggOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting NeweggOrderItemEntity instances in the database.</summary>
		public DataSource2<NeweggOrderItemEntity> NeweggOrderItem
		{
			get { return new DataSource2<NeweggOrderItemEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting NeweggStoreEntity instances in the database.</summary>
		public DataSource2<NeweggStoreEntity> NeweggStore
		{
			get { return new DataSource2<NeweggStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting NoteEntity instances in the database.</summary>
		public DataSource2<NoteEntity> Note
		{
			get { return new DataSource2<NoteEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ObjectLabelEntity instances in the database.</summary>
		public DataSource2<ObjectLabelEntity> ObjectLabel
		{
			get { return new DataSource2<ObjectLabelEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ObjectReferenceEntity instances in the database.</summary>
		public DataSource2<ObjectReferenceEntity> ObjectReference
		{
			get { return new DataSource2<ObjectReferenceEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting OnTracAccountEntity instances in the database.</summary>
		public DataSource2<OnTracAccountEntity> OnTracAccount
		{
			get { return new DataSource2<OnTracAccountEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting OnTracProfileEntity instances in the database.</summary>
		public DataSource2<OnTracProfileEntity> OnTracProfile
		{
			get { return new DataSource2<OnTracProfileEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting OnTracShipmentEntity instances in the database.</summary>
		public DataSource2<OnTracShipmentEntity> OnTracShipment
		{
			get { return new DataSource2<OnTracShipmentEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting OrderEntity instances in the database.</summary>
		public DataSource2<OrderEntity> Order
		{
			get { return new DataSource2<OrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting OrderChargeEntity instances in the database.</summary>
		public DataSource2<OrderChargeEntity> OrderCharge
		{
			get { return new DataSource2<OrderChargeEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting OrderItemEntity instances in the database.</summary>
		public DataSource2<OrderItemEntity> OrderItem
		{
			get { return new DataSource2<OrderItemEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting OrderItemAttributeEntity instances in the database.</summary>
		public DataSource2<OrderItemAttributeEntity> OrderItemAttribute
		{
			get { return new DataSource2<OrderItemAttributeEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting OrderMotionOrderEntity instances in the database.</summary>
		public DataSource2<OrderMotionOrderEntity> OrderMotionOrder
		{
			get { return new DataSource2<OrderMotionOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting OrderMotionStoreEntity instances in the database.</summary>
		public DataSource2<OrderMotionStoreEntity> OrderMotionStore
		{
			get { return new DataSource2<OrderMotionStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting OrderPaymentDetailEntity instances in the database.</summary>
		public DataSource2<OrderPaymentDetailEntity> OrderPaymentDetail
		{
			get { return new DataSource2<OrderPaymentDetailEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting OtherProfileEntity instances in the database.</summary>
		public DataSource2<OtherProfileEntity> OtherProfile
		{
			get { return new DataSource2<OtherProfileEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting OtherShipmentEntity instances in the database.</summary>
		public DataSource2<OtherShipmentEntity> OtherShipment
		{
			get { return new DataSource2<OtherShipmentEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting PayPalOrderEntity instances in the database.</summary>
		public DataSource2<PayPalOrderEntity> PayPalOrder
		{
			get { return new DataSource2<PayPalOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting PayPalStoreEntity instances in the database.</summary>
		public DataSource2<PayPalStoreEntity> PayPalStore
		{
			get { return new DataSource2<PayPalStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting PermissionEntity instances in the database.</summary>
		public DataSource2<PermissionEntity> Permission
		{
			get { return new DataSource2<PermissionEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting PostalProfileEntity instances in the database.</summary>
		public DataSource2<PostalProfileEntity> PostalProfile
		{
			get { return new DataSource2<PostalProfileEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting PostalShipmentEntity instances in the database.</summary>
		public DataSource2<PostalShipmentEntity> PostalShipment
		{
			get { return new DataSource2<PostalShipmentEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting PrintResultEntity instances in the database.</summary>
		public DataSource2<PrintResultEntity> PrintResult
		{
			get { return new DataSource2<PrintResultEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ProStoresOrderEntity instances in the database.</summary>
		public DataSource2<ProStoresOrderEntity> ProStoresOrder
		{
			get { return new DataSource2<ProStoresOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ProStoresStoreEntity instances in the database.</summary>
		public DataSource2<ProStoresStoreEntity> ProStoresStore
		{
			get { return new DataSource2<ProStoresStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ResourceEntity instances in the database.</summary>
		public DataSource2<ResourceEntity> Resource
		{
			get { return new DataSource2<ResourceEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ScanFormBatchEntity instances in the database.</summary>
		public DataSource2<ScanFormBatchEntity> ScanFormBatch
		{
			get { return new DataSource2<ScanFormBatchEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting SearchEntity instances in the database.</summary>
		public DataSource2<SearchEntity> Search
		{
			get { return new DataSource2<SearchEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting SearsOrderEntity instances in the database.</summary>
		public DataSource2<SearsOrderEntity> SearsOrder
		{
			get { return new DataSource2<SearsOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting SearsOrderItemEntity instances in the database.</summary>
		public DataSource2<SearsOrderItemEntity> SearsOrderItem
		{
			get { return new DataSource2<SearsOrderItemEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting SearsStoreEntity instances in the database.</summary>
		public DataSource2<SearsStoreEntity> SearsStore
		{
			get { return new DataSource2<SearsStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ServerMessageEntity instances in the database.</summary>
		public DataSource2<ServerMessageEntity> ServerMessage
		{
			get { return new DataSource2<ServerMessageEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ServerMessageSignoffEntity instances in the database.</summary>
		public DataSource2<ServerMessageSignoffEntity> ServerMessageSignoff
		{
			get { return new DataSource2<ServerMessageSignoffEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ServiceStatusEntity instances in the database.</summary>
		public DataSource2<ServiceStatusEntity> ServiceStatus
		{
			get { return new DataSource2<ServiceStatusEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ShipmentEntity instances in the database.</summary>
		public DataSource2<ShipmentEntity> Shipment
		{
			get { return new DataSource2<ShipmentEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ShipmentCustomsItemEntity instances in the database.</summary>
		public DataSource2<ShipmentCustomsItemEntity> ShipmentCustomsItem
		{
			get { return new DataSource2<ShipmentCustomsItemEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ShippingDefaultsRuleEntity instances in the database.</summary>
		public DataSource2<ShippingDefaultsRuleEntity> ShippingDefaultsRule
		{
			get { return new DataSource2<ShippingDefaultsRuleEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ShippingOriginEntity instances in the database.</summary>
		public DataSource2<ShippingOriginEntity> ShippingOrigin
		{
			get { return new DataSource2<ShippingOriginEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ShippingPrintOutputEntity instances in the database.</summary>
		public DataSource2<ShippingPrintOutputEntity> ShippingPrintOutput
		{
			get { return new DataSource2<ShippingPrintOutputEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ShippingPrintOutputRuleEntity instances in the database.</summary>
		public DataSource2<ShippingPrintOutputRuleEntity> ShippingPrintOutputRule
		{
			get { return new DataSource2<ShippingPrintOutputRuleEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ShippingProfileEntity instances in the database.</summary>
		public DataSource2<ShippingProfileEntity> ShippingProfile
		{
			get { return new DataSource2<ShippingProfileEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ShippingProviderRuleEntity instances in the database.</summary>
		public DataSource2<ShippingProviderRuleEntity> ShippingProviderRule
		{
			get { return new DataSource2<ShippingProviderRuleEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ShippingSettingsEntity instances in the database.</summary>
		public DataSource2<ShippingSettingsEntity> ShippingSettings
		{
			get { return new DataSource2<ShippingSettingsEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ShipSenseKnowledgebaseEntity instances in the database.</summary>
		public DataSource2<ShipSenseKnowledgebaseEntity> ShipSenseKnowledgebase
		{
			get { return new DataSource2<ShipSenseKnowledgebaseEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ShopifyOrderEntity instances in the database.</summary>
		public DataSource2<ShopifyOrderEntity> ShopifyOrder
		{
			get { return new DataSource2<ShopifyOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ShopifyOrderItemEntity instances in the database.</summary>
		public DataSource2<ShopifyOrderItemEntity> ShopifyOrderItem
		{
			get { return new DataSource2<ShopifyOrderItemEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ShopifyStoreEntity instances in the database.</summary>
		public DataSource2<ShopifyStoreEntity> ShopifyStore
		{
			get { return new DataSource2<ShopifyStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ShopSiteStoreEntity instances in the database.</summary>
		public DataSource2<ShopSiteStoreEntity> ShopSiteStore
		{
			get { return new DataSource2<ShopSiteStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting StampsAccountEntity instances in the database.</summary>
		public DataSource2<StampsAccountEntity> StampsAccount
		{
			get { return new DataSource2<StampsAccountEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting StampsProfileEntity instances in the database.</summary>
		public DataSource2<StampsProfileEntity> StampsProfile
		{
			get { return new DataSource2<StampsProfileEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting StampsScanFormEntity instances in the database.</summary>
		public DataSource2<StampsScanFormEntity> StampsScanForm
		{
			get { return new DataSource2<StampsScanFormEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting StampsShipmentEntity instances in the database.</summary>
		public DataSource2<StampsShipmentEntity> StampsShipment
		{
			get { return new DataSource2<StampsShipmentEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting StatusPresetEntity instances in the database.</summary>
		public DataSource2<StatusPresetEntity> StatusPreset
		{
			get { return new DataSource2<StatusPresetEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting StoreEntity instances in the database.</summary>
		public DataSource2<StoreEntity> Store
		{
			get { return new DataSource2<StoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting SystemDataEntity instances in the database.</summary>
		public DataSource2<SystemDataEntity> SystemData
		{
			get { return new DataSource2<SystemDataEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting TemplateEntity instances in the database.</summary>
		public DataSource2<TemplateEntity> Template
		{
			get { return new DataSource2<TemplateEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting TemplateComputerSettingsEntity instances in the database.</summary>
		public DataSource2<TemplateComputerSettingsEntity> TemplateComputerSettings
		{
			get { return new DataSource2<TemplateComputerSettingsEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting TemplateFolderEntity instances in the database.</summary>
		public DataSource2<TemplateFolderEntity> TemplateFolder
		{
			get { return new DataSource2<TemplateFolderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting TemplateStoreSettingsEntity instances in the database.</summary>
		public DataSource2<TemplateStoreSettingsEntity> TemplateStoreSettings
		{
			get { return new DataSource2<TemplateStoreSettingsEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting TemplateUserSettingsEntity instances in the database.</summary>
		public DataSource2<TemplateUserSettingsEntity> TemplateUserSettings
		{
			get { return new DataSource2<TemplateUserSettingsEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ThreeDCartOrderItemEntity instances in the database.</summary>
		public DataSource2<ThreeDCartOrderItemEntity> ThreeDCartOrderItem
		{
			get { return new DataSource2<ThreeDCartOrderItemEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting ThreeDCartStoreEntity instances in the database.</summary>
		public DataSource2<ThreeDCartStoreEntity> ThreeDCartStore
		{
			get { return new DataSource2<ThreeDCartStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting UpsAccountEntity instances in the database.</summary>
		public DataSource2<UpsAccountEntity> UpsAccount
		{
			get { return new DataSource2<UpsAccountEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting UpsPackageEntity instances in the database.</summary>
		public DataSource2<UpsPackageEntity> UpsPackage
		{
			get { return new DataSource2<UpsPackageEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting UpsProfileEntity instances in the database.</summary>
		public DataSource2<UpsProfileEntity> UpsProfile
		{
			get { return new DataSource2<UpsProfileEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting UpsProfilePackageEntity instances in the database.</summary>
		public DataSource2<UpsProfilePackageEntity> UpsProfilePackage
		{
			get { return new DataSource2<UpsProfilePackageEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting UpsShipmentEntity instances in the database.</summary>
		public DataSource2<UpsShipmentEntity> UpsShipment
		{
			get { return new DataSource2<UpsShipmentEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting UserEntity instances in the database.</summary>
		public DataSource2<UserEntity> User
		{
			get { return new DataSource2<UserEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting UserColumnSettingsEntity instances in the database.</summary>
		public DataSource2<UserColumnSettingsEntity> UserColumnSettings
		{
			get { return new DataSource2<UserColumnSettingsEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting UserSettingsEntity instances in the database.</summary>
		public DataSource2<UserSettingsEntity> UserSettings
		{
			get { return new DataSource2<UserSettingsEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting VersionSignoffEntity instances in the database.</summary>
		public DataSource2<VersionSignoffEntity> VersionSignoff
		{
			get { return new DataSource2<VersionSignoffEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting VolusionStoreEntity instances in the database.</summary>
		public DataSource2<VolusionStoreEntity> VolusionStore
		{
			get { return new DataSource2<VolusionStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting WorldShipGoodsEntity instances in the database.</summary>
		public DataSource2<WorldShipGoodsEntity> WorldShipGoods
		{
			get { return new DataSource2<WorldShipGoodsEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting WorldShipPackageEntity instances in the database.</summary>
		public DataSource2<WorldShipPackageEntity> WorldShipPackage
		{
			get { return new DataSource2<WorldShipPackageEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting WorldShipProcessedEntity instances in the database.</summary>
		public DataSource2<WorldShipProcessedEntity> WorldShipProcessed
		{
			get { return new DataSource2<WorldShipProcessedEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting WorldShipShipmentEntity instances in the database.</summary>
		public DataSource2<WorldShipShipmentEntity> WorldShipShipment
		{
			get { return new DataSource2<WorldShipShipmentEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting YahooOrderEntity instances in the database.</summary>
		public DataSource2<YahooOrderEntity> YahooOrder
		{
			get { return new DataSource2<YahooOrderEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting YahooOrderItemEntity instances in the database.</summary>
		public DataSource2<YahooOrderItemEntity> YahooOrderItem
		{
			get { return new DataSource2<YahooOrderItemEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting YahooProductEntity instances in the database.</summary>
		public DataSource2<YahooProductEntity> YahooProduct
		{
			get { return new DataSource2<YahooProductEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		/// <summary>returns the datasource to use in a Linq query when targeting YahooStoreEntity instances in the database.</summary>
		public DataSource2<YahooStoreEntity> YahooStore
		{
			get { return new DataSource2<YahooStoreEntity>(_adapterToUse, new ElementCreator(), _customFunctionMappings, _contextToUse); }
		}
		
		
		#region Class Property Declarations
		/// <summary> Gets / sets the IDataAccessAdapter to use for the queries created with this meta data object.</summary>
		/// <remarks> Be aware that the IDataAccessAdapter object set via this property is kept alive by the LLBLGenProQuery objects created with this meta data
		/// till they go out of scope.</remarks>
		public IDataAccessAdapter AdapterToUse
		{
			get { return _adapterToUse;}
			set { _adapterToUse = value;}
		}

		/// <summary>Gets or sets the custom function mappings to use. These take higher precedence than the ones in the DQE to use</summary>
		public FunctionMappingStore CustomFunctionMappings
		{
			get { return _customFunctionMappings; }
			set { _customFunctionMappings = value; }
		}
		
		/// <summary>Gets or sets the Context instance to use for entity fetches.</summary>
		public Context ContextToUse
		{
			get { return _contextToUse;}
			set { _contextToUse = value;}
		}
		#endregion
	}
}