///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 5.0
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates
// Templates vendor: Solutions Design.
////////////////////////////////////////////////////////////// 
using System;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;

namespace ShipWorks.Data.Model.FactoryClasses
{
	/// <summary>Factory class to produce DynamicQuery instances and EntityQuery instances</summary>
	public partial class QueryFactory
	{
		private int _aliasCounter = 0;

		/// <summary>Creates a new DynamicQuery instance with no alias set.</summary>
		/// <returns>Ready to use DynamicQuery instance</returns>
		public DynamicQuery Create()
		{
			return Create(string.Empty);
		}

		/// <summary>Creates a new DynamicQuery instance with the alias specified as the alias set.</summary>
		/// <param name="alias">The alias.</param>
		/// <returns>Ready to use DynamicQuery instance</returns>
		public DynamicQuery Create(string alias)
		{
			return new DynamicQuery(new ElementCreator(), alias, this.GetNextAliasCounterValue());
		}

		/// <summary>Creates a new DynamicQuery which wraps the specified TableValuedFunction call</summary>
		/// <param name="toWrap">The table valued function call to wrap.</param>
		/// <returns>toWrap wrapped in a DynamicQuery.</returns>
		public DynamicQuery Create(TableValuedFunctionCall toWrap)
		{
			return this.Create().From(new TvfCallWrapper(toWrap)).Select(toWrap.GetFieldsAsArray().Select(f => this.Field(toWrap.Alias, f.Alias)).ToArray());
		}

		/// <summary>Creates a new EntityQuery for the entity of the type specified with no alias set.</summary>
		/// <typeparam name="TEntity">The type of the entity to produce the query for.</typeparam>
		/// <returns>ready to use EntityQuery instance</returns>
		public EntityQuery<TEntity> Create<TEntity>()
			where TEntity : IEntityCore
		{
			return Create<TEntity>(string.Empty);
		}

		/// <summary>Creates a new EntityQuery for the entity of the type specified with the alias specified as the alias set.</summary>
		/// <typeparam name="TEntity">The type of the entity to produce the query for.</typeparam>
		/// <param name="alias">The alias.</param>
		/// <returns>ready to use EntityQuery instance</returns>
		public EntityQuery<TEntity> Create<TEntity>(string alias)
			where TEntity : IEntityCore
		{
			return new EntityQuery<TEntity>(new ElementCreator(), alias, this.GetNextAliasCounterValue());
		}
				
		/// <summary>Creates a new field object with the name specified and of resulttype 'object'. Used for referring to aliased fields in another projection.</summary>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns>Ready to use field object</returns>
		public EntityField2 Field(string fieldName)
		{
			return Field<object>(string.Empty, fieldName);
		}

		/// <summary>Creates a new field object with the name specified and of resulttype 'object'. Used for referring to aliased fields in another projection.</summary>
		/// <param name="targetAlias">The alias of the table/query to target.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns>Ready to use field object</returns>
		public EntityField2 Field(string targetAlias, string fieldName)
		{
			return Field<object>(targetAlias, fieldName);
		}

		/// <summary>Creates a new field object with the name specified and of resulttype 'TValue'. Used for referring to aliased fields in another projection.</summary>
		/// <typeparam name="TValue">The type of the value represented by the field.</typeparam>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns>Ready to use field object</returns>
		public EntityField2 Field<TValue>(string fieldName)
		{
			return Field<TValue>(string.Empty, fieldName);
		}

		/// <summary>Creates a new field object with the name specified and of resulttype 'TValue'. Used for referring to aliased fields in another projection.</summary>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="targetAlias">The alias of the table/query to target.</param>
		/// <param name="fieldName">Name of the field.</param>
		/// <returns>Ready to use field object</returns>
		public EntityField2 Field<TValue>(string targetAlias, string fieldName)
		{
			return new EntityField2(fieldName, targetAlias, typeof(TValue));
		}
		
		/// <summary>Gets the next alias counter value to produce artifical aliases with</summary>
		private int GetNextAliasCounterValue()
		{
			_aliasCounter++;
			return _aliasCounter;
		}
		

		/// <summary>Creates and returns a new EntityQuery for the Action entity</summary>
		public EntityQuery<ActionEntity> Action
		{
			get { return Create<ActionEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ActionFilterTrigger entity</summary>
		public EntityQuery<ActionFilterTriggerEntity> ActionFilterTrigger
		{
			get { return Create<ActionFilterTriggerEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ActionQueue entity</summary>
		public EntityQuery<ActionQueueEntity> ActionQueue
		{
			get { return Create<ActionQueueEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ActionQueueSelection entity</summary>
		public EntityQuery<ActionQueueSelectionEntity> ActionQueueSelection
		{
			get { return Create<ActionQueueSelectionEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ActionQueueStep entity</summary>
		public EntityQuery<ActionQueueStepEntity> ActionQueueStep
		{
			get { return Create<ActionQueueStepEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ActionTask entity</summary>
		public EntityQuery<ActionTaskEntity> ActionTask
		{
			get { return Create<ActionTaskEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the AmazonASIN entity</summary>
		public EntityQuery<AmazonASINEntity> AmazonASIN
		{
			get { return Create<AmazonASINEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the AmazonOrder entity</summary>
		public EntityQuery<AmazonOrderEntity> AmazonOrder
		{
			get { return Create<AmazonOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the AmazonOrderItem entity</summary>
		public EntityQuery<AmazonOrderItemEntity> AmazonOrderItem
		{
			get { return Create<AmazonOrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the AmazonProfile entity</summary>
		public EntityQuery<AmazonProfileEntity> AmazonProfile
		{
			get { return Create<AmazonProfileEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the AmazonShipment entity</summary>
		public EntityQuery<AmazonShipmentEntity> AmazonShipment
		{
			get { return Create<AmazonShipmentEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the AmazonStore entity</summary>
		public EntityQuery<AmazonStoreEntity> AmazonStore
		{
			get { return Create<AmazonStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the AmeriCommerceStore entity</summary>
		public EntityQuery<AmeriCommerceStoreEntity> AmeriCommerceStore
		{
			get { return Create<AmeriCommerceStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Audit entity</summary>
		public EntityQuery<AuditEntity> Audit
		{
			get { return Create<AuditEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the AuditChange entity</summary>
		public EntityQuery<AuditChangeEntity> AuditChange
		{
			get { return Create<AuditChangeEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the AuditChangeDetail entity</summary>
		public EntityQuery<AuditChangeDetailEntity> AuditChangeDetail
		{
			get { return Create<AuditChangeDetailEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the BestRateProfile entity</summary>
		public EntityQuery<BestRateProfileEntity> BestRateProfile
		{
			get { return Create<BestRateProfileEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the BestRateShipment entity</summary>
		public EntityQuery<BestRateShipmentEntity> BestRateShipment
		{
			get { return Create<BestRateShipmentEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the BigCommerceOrderItem entity</summary>
		public EntityQuery<BigCommerceOrderItemEntity> BigCommerceOrderItem
		{
			get { return Create<BigCommerceOrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the BigCommerceStore entity</summary>
		public EntityQuery<BigCommerceStoreEntity> BigCommerceStore
		{
			get { return Create<BigCommerceStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the BuyDotComOrderItem entity</summary>
		public EntityQuery<BuyDotComOrderItemEntity> BuyDotComOrderItem
		{
			get { return Create<BuyDotComOrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the BuyDotComStore entity</summary>
		public EntityQuery<BuyDotComStoreEntity> BuyDotComStore
		{
			get { return Create<BuyDotComStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ChannelAdvisorOrder entity</summary>
		public EntityQuery<ChannelAdvisorOrderEntity> ChannelAdvisorOrder
		{
			get { return Create<ChannelAdvisorOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ChannelAdvisorOrderItem entity</summary>
		public EntityQuery<ChannelAdvisorOrderItemEntity> ChannelAdvisorOrderItem
		{
			get { return Create<ChannelAdvisorOrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ChannelAdvisorStore entity</summary>
		public EntityQuery<ChannelAdvisorStoreEntity> ChannelAdvisorStore
		{
			get { return Create<ChannelAdvisorStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ClickCartProOrder entity</summary>
		public EntityQuery<ClickCartProOrderEntity> ClickCartProOrder
		{
			get { return Create<ClickCartProOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the CommerceInterfaceOrder entity</summary>
		public EntityQuery<CommerceInterfaceOrderEntity> CommerceInterfaceOrder
		{
			get { return Create<CommerceInterfaceOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Computer entity</summary>
		public EntityQuery<ComputerEntity> Computer
		{
			get { return Create<ComputerEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Configuration entity</summary>
		public EntityQuery<ConfigurationEntity> Configuration
		{
			get { return Create<ConfigurationEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Customer entity</summary>
		public EntityQuery<CustomerEntity> Customer
		{
			get { return Create<CustomerEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the DimensionsProfile entity</summary>
		public EntityQuery<DimensionsProfileEntity> DimensionsProfile
		{
			get { return Create<DimensionsProfileEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Download entity</summary>
		public EntityQuery<DownloadEntity> Download
		{
			get { return Create<DownloadEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the DownloadDetail entity</summary>
		public EntityQuery<DownloadDetailEntity> DownloadDetail
		{
			get { return Create<DownloadDetailEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the EbayCombinedOrderRelation entity</summary>
		public EntityQuery<EbayCombinedOrderRelationEntity> EbayCombinedOrderRelation
		{
			get { return Create<EbayCombinedOrderRelationEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the EbayOrder entity</summary>
		public EntityQuery<EbayOrderEntity> EbayOrder
		{
			get { return Create<EbayOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the EbayOrderItem entity</summary>
		public EntityQuery<EbayOrderItemEntity> EbayOrderItem
		{
			get { return Create<EbayOrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the EbayStore entity</summary>
		public EntityQuery<EbayStoreEntity> EbayStore
		{
			get { return Create<EbayStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the EmailAccount entity</summary>
		public EntityQuery<EmailAccountEntity> EmailAccount
		{
			get { return Create<EmailAccountEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the EmailOutbound entity</summary>
		public EntityQuery<EmailOutboundEntity> EmailOutbound
		{
			get { return Create<EmailOutboundEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the EmailOutboundRelation entity</summary>
		public EntityQuery<EmailOutboundRelationEntity> EmailOutboundRelation
		{
			get { return Create<EmailOutboundRelationEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the EndiciaAccount entity</summary>
		public EntityQuery<EndiciaAccountEntity> EndiciaAccount
		{
			get { return Create<EndiciaAccountEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the EndiciaProfile entity</summary>
		public EntityQuery<EndiciaProfileEntity> EndiciaProfile
		{
			get { return Create<EndiciaProfileEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the EndiciaScanForm entity</summary>
		public EntityQuery<EndiciaScanFormEntity> EndiciaScanForm
		{
			get { return Create<EndiciaScanFormEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the EndiciaShipment entity</summary>
		public EntityQuery<EndiciaShipmentEntity> EndiciaShipment
		{
			get { return Create<EndiciaShipmentEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the EtsyOrder entity</summary>
		public EntityQuery<EtsyOrderEntity> EtsyOrder
		{
			get { return Create<EtsyOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the EtsyOrderItem entity</summary>
		public EntityQuery<EtsyOrderItemEntity> EtsyOrderItem
		{
			get { return Create<EtsyOrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the EtsyStore entity</summary>
		public EntityQuery<EtsyStoreEntity> EtsyStore
		{
			get { return Create<EtsyStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ExcludedPackageType entity</summary>
		public EntityQuery<ExcludedPackageTypeEntity> ExcludedPackageType
		{
			get { return Create<ExcludedPackageTypeEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ExcludedServiceType entity</summary>
		public EntityQuery<ExcludedServiceTypeEntity> ExcludedServiceType
		{
			get { return Create<ExcludedServiceTypeEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the FedExAccount entity</summary>
		public EntityQuery<FedExAccountEntity> FedExAccount
		{
			get { return Create<FedExAccountEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the FedExEndOfDayClose entity</summary>
		public EntityQuery<FedExEndOfDayCloseEntity> FedExEndOfDayClose
		{
			get { return Create<FedExEndOfDayCloseEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the FedExPackage entity</summary>
		public EntityQuery<FedExPackageEntity> FedExPackage
		{
			get { return Create<FedExPackageEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the FedExProfile entity</summary>
		public EntityQuery<FedExProfileEntity> FedExProfile
		{
			get { return Create<FedExProfileEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the FedExProfilePackage entity</summary>
		public EntityQuery<FedExProfilePackageEntity> FedExProfilePackage
		{
			get { return Create<FedExProfilePackageEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the FedExShipment entity</summary>
		public EntityQuery<FedExShipmentEntity> FedExShipment
		{
			get { return Create<FedExShipmentEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Filter entity</summary>
		public EntityQuery<FilterEntity> Filter
		{
			get { return Create<FilterEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the FilterLayout entity</summary>
		public EntityQuery<FilterLayoutEntity> FilterLayout
		{
			get { return Create<FilterLayoutEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the FilterNode entity</summary>
		public EntityQuery<FilterNodeEntity> FilterNode
		{
			get { return Create<FilterNodeEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the FilterNodeColumnSettings entity</summary>
		public EntityQuery<FilterNodeColumnSettingsEntity> FilterNodeColumnSettings
		{
			get { return Create<FilterNodeColumnSettingsEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the FilterNodeContent entity</summary>
		public EntityQuery<FilterNodeContentEntity> FilterNodeContent
		{
			get { return Create<FilterNodeContentEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the FilterNodeContentDetail entity</summary>
		public EntityQuery<FilterNodeContentDetailEntity> FilterNodeContentDetail
		{
			get { return Create<FilterNodeContentDetailEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the FilterSequence entity</summary>
		public EntityQuery<FilterSequenceEntity> FilterSequence
		{
			get { return Create<FilterSequenceEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the FtpAccount entity</summary>
		public EntityQuery<FtpAccountEntity> FtpAccount
		{
			get { return Create<FtpAccountEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the GenericFileStore entity</summary>
		public EntityQuery<GenericFileStoreEntity> GenericFileStore
		{
			get { return Create<GenericFileStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the GenericModuleStore entity</summary>
		public EntityQuery<GenericModuleStoreEntity> GenericModuleStore
		{
			get { return Create<GenericModuleStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the GridColumnFormat entity</summary>
		public EntityQuery<GridColumnFormatEntity> GridColumnFormat
		{
			get { return Create<GridColumnFormatEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the GridColumnLayout entity</summary>
		public EntityQuery<GridColumnLayoutEntity> GridColumnLayout
		{
			get { return Create<GridColumnLayoutEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the GridColumnPosition entity</summary>
		public EntityQuery<GridColumnPositionEntity> GridColumnPosition
		{
			get { return Create<GridColumnPositionEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the GrouponOrder entity</summary>
		public EntityQuery<GrouponOrderEntity> GrouponOrder
		{
			get { return Create<GrouponOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the GrouponOrderItem entity</summary>
		public EntityQuery<GrouponOrderItemEntity> GrouponOrderItem
		{
			get { return Create<GrouponOrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the GrouponStore entity</summary>
		public EntityQuery<GrouponStoreEntity> GrouponStore
		{
			get { return Create<GrouponStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the InfopiaOrderItem entity</summary>
		public EntityQuery<InfopiaOrderItemEntity> InfopiaOrderItem
		{
			get { return Create<InfopiaOrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the InfopiaStore entity</summary>
		public EntityQuery<InfopiaStoreEntity> InfopiaStore
		{
			get { return Create<InfopiaStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the InsurancePolicy entity</summary>
		public EntityQuery<InsurancePolicyEntity> InsurancePolicy
		{
			get { return Create<InsurancePolicyEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the IParcelAccount entity</summary>
		public EntityQuery<IParcelAccountEntity> IParcelAccount
		{
			get { return Create<IParcelAccountEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the IParcelPackage entity</summary>
		public EntityQuery<IParcelPackageEntity> IParcelPackage
		{
			get { return Create<IParcelPackageEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the IParcelProfile entity</summary>
		public EntityQuery<IParcelProfileEntity> IParcelProfile
		{
			get { return Create<IParcelProfileEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the IParcelProfilePackage entity</summary>
		public EntityQuery<IParcelProfilePackageEntity> IParcelProfilePackage
		{
			get { return Create<IParcelProfilePackageEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the IParcelShipment entity</summary>
		public EntityQuery<IParcelShipmentEntity> IParcelShipment
		{
			get { return Create<IParcelShipmentEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the JetOrder entity</summary>
		public EntityQuery<JetOrderEntity> JetOrder
		{
			get { return Create<JetOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the JetOrderItem entity</summary>
		public EntityQuery<JetOrderItemEntity> JetOrderItem
		{
			get { return Create<JetOrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the JetStore entity</summary>
		public EntityQuery<JetStoreEntity> JetStore
		{
			get { return Create<JetStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the LabelSheet entity</summary>
		public EntityQuery<LabelSheetEntity> LabelSheet
		{
			get { return Create<LabelSheetEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the LemonStandOrder entity</summary>
		public EntityQuery<LemonStandOrderEntity> LemonStandOrder
		{
			get { return Create<LemonStandOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the LemonStandOrderItem entity</summary>
		public EntityQuery<LemonStandOrderItemEntity> LemonStandOrderItem
		{
			get { return Create<LemonStandOrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the LemonStandStore entity</summary>
		public EntityQuery<LemonStandStoreEntity> LemonStandStore
		{
			get { return Create<LemonStandStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the MagentoOrder entity</summary>
		public EntityQuery<MagentoOrderEntity> MagentoOrder
		{
			get { return Create<MagentoOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the MagentoStore entity</summary>
		public EntityQuery<MagentoStoreEntity> MagentoStore
		{
			get { return Create<MagentoStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the MarketplaceAdvisorOrder entity</summary>
		public EntityQuery<MarketplaceAdvisorOrderEntity> MarketplaceAdvisorOrder
		{
			get { return Create<MarketplaceAdvisorOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the MarketplaceAdvisorStore entity</summary>
		public EntityQuery<MarketplaceAdvisorStoreEntity> MarketplaceAdvisorStore
		{
			get { return Create<MarketplaceAdvisorStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the MivaOrderItemAttribute entity</summary>
		public EntityQuery<MivaOrderItemAttributeEntity> MivaOrderItemAttribute
		{
			get { return Create<MivaOrderItemAttributeEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the MivaStore entity</summary>
		public EntityQuery<MivaStoreEntity> MivaStore
		{
			get { return Create<MivaStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the NetworkSolutionsOrder entity</summary>
		public EntityQuery<NetworkSolutionsOrderEntity> NetworkSolutionsOrder
		{
			get { return Create<NetworkSolutionsOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the NetworkSolutionsStore entity</summary>
		public EntityQuery<NetworkSolutionsStoreEntity> NetworkSolutionsStore
		{
			get { return Create<NetworkSolutionsStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the NeweggOrder entity</summary>
		public EntityQuery<NeweggOrderEntity> NeweggOrder
		{
			get { return Create<NeweggOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the NeweggOrderItem entity</summary>
		public EntityQuery<NeweggOrderItemEntity> NeweggOrderItem
		{
			get { return Create<NeweggOrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the NeweggStore entity</summary>
		public EntityQuery<NeweggStoreEntity> NeweggStore
		{
			get { return Create<NeweggStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Note entity</summary>
		public EntityQuery<NoteEntity> Note
		{
			get { return Create<NoteEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ObjectLabel entity</summary>
		public EntityQuery<ObjectLabelEntity> ObjectLabel
		{
			get { return Create<ObjectLabelEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ObjectReference entity</summary>
		public EntityQuery<ObjectReferenceEntity> ObjectReference
		{
			get { return Create<ObjectReferenceEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the OdbcStore entity</summary>
		public EntityQuery<OdbcStoreEntity> OdbcStore
		{
			get { return Create<OdbcStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the OnTracAccount entity</summary>
		public EntityQuery<OnTracAccountEntity> OnTracAccount
		{
			get { return Create<OnTracAccountEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the OnTracProfile entity</summary>
		public EntityQuery<OnTracProfileEntity> OnTracProfile
		{
			get { return Create<OnTracProfileEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the OnTracShipment entity</summary>
		public EntityQuery<OnTracShipmentEntity> OnTracShipment
		{
			get { return Create<OnTracShipmentEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Order entity</summary>
		public EntityQuery<OrderEntity> Order
		{
			get { return Create<OrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the OrderCharge entity</summary>
		public EntityQuery<OrderChargeEntity> OrderCharge
		{
			get { return Create<OrderChargeEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the OrderItem entity</summary>
		public EntityQuery<OrderItemEntity> OrderItem
		{
			get { return Create<OrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the OrderItemAttribute entity</summary>
		public EntityQuery<OrderItemAttributeEntity> OrderItemAttribute
		{
			get { return Create<OrderItemAttributeEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the OrderMotionOrder entity</summary>
		public EntityQuery<OrderMotionOrderEntity> OrderMotionOrder
		{
			get { return Create<OrderMotionOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the OrderMotionStore entity</summary>
		public EntityQuery<OrderMotionStoreEntity> OrderMotionStore
		{
			get { return Create<OrderMotionStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the OrderPaymentDetail entity</summary>
		public EntityQuery<OrderPaymentDetailEntity> OrderPaymentDetail
		{
			get { return Create<OrderPaymentDetailEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the OtherProfile entity</summary>
		public EntityQuery<OtherProfileEntity> OtherProfile
		{
			get { return Create<OtherProfileEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the OtherShipment entity</summary>
		public EntityQuery<OtherShipmentEntity> OtherShipment
		{
			get { return Create<OtherShipmentEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the PayPalOrder entity</summary>
		public EntityQuery<PayPalOrderEntity> PayPalOrder
		{
			get { return Create<PayPalOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the PayPalStore entity</summary>
		public EntityQuery<PayPalStoreEntity> PayPalStore
		{
			get { return Create<PayPalStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Permission entity</summary>
		public EntityQuery<PermissionEntity> Permission
		{
			get { return Create<PermissionEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the PostalProfile entity</summary>
		public EntityQuery<PostalProfileEntity> PostalProfile
		{
			get { return Create<PostalProfileEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the PostalShipment entity</summary>
		public EntityQuery<PostalShipmentEntity> PostalShipment
		{
			get { return Create<PostalShipmentEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the PrintResult entity</summary>
		public EntityQuery<PrintResultEntity> PrintResult
		{
			get { return Create<PrintResultEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ProStoresOrder entity</summary>
		public EntityQuery<ProStoresOrderEntity> ProStoresOrder
		{
			get { return Create<ProStoresOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ProStoresStore entity</summary>
		public EntityQuery<ProStoresStoreEntity> ProStoresStore
		{
			get { return Create<ProStoresStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Resource entity</summary>
		public EntityQuery<ResourceEntity> Resource
		{
			get { return Create<ResourceEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ScanFormBatch entity</summary>
		public EntityQuery<ScanFormBatchEntity> ScanFormBatch
		{
			get { return Create<ScanFormBatchEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Search entity</summary>
		public EntityQuery<SearchEntity> Search
		{
			get { return Create<SearchEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the SearsOrder entity</summary>
		public EntityQuery<SearsOrderEntity> SearsOrder
		{
			get { return Create<SearsOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the SearsOrderItem entity</summary>
		public EntityQuery<SearsOrderItemEntity> SearsOrderItem
		{
			get { return Create<SearsOrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the SearsStore entity</summary>
		public EntityQuery<SearsStoreEntity> SearsStore
		{
			get { return Create<SearsStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ServerMessage entity</summary>
		public EntityQuery<ServerMessageEntity> ServerMessage
		{
			get { return Create<ServerMessageEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ServerMessageSignoff entity</summary>
		public EntityQuery<ServerMessageSignoffEntity> ServerMessageSignoff
		{
			get { return Create<ServerMessageSignoffEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ServiceStatus entity</summary>
		public EntityQuery<ServiceStatusEntity> ServiceStatus
		{
			get { return Create<ServiceStatusEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Shipment entity</summary>
		public EntityQuery<ShipmentEntity> Shipment
		{
			get { return Create<ShipmentEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ShipmentCustomsItem entity</summary>
		public EntityQuery<ShipmentCustomsItemEntity> ShipmentCustomsItem
		{
			get { return Create<ShipmentCustomsItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ShippingDefaultsRule entity</summary>
		public EntityQuery<ShippingDefaultsRuleEntity> ShippingDefaultsRule
		{
			get { return Create<ShippingDefaultsRuleEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ShippingOrigin entity</summary>
		public EntityQuery<ShippingOriginEntity> ShippingOrigin
		{
			get { return Create<ShippingOriginEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ShippingPrintOutput entity</summary>
		public EntityQuery<ShippingPrintOutputEntity> ShippingPrintOutput
		{
			get { return Create<ShippingPrintOutputEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ShippingPrintOutputRule entity</summary>
		public EntityQuery<ShippingPrintOutputRuleEntity> ShippingPrintOutputRule
		{
			get { return Create<ShippingPrintOutputRuleEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ShippingProfile entity</summary>
		public EntityQuery<ShippingProfileEntity> ShippingProfile
		{
			get { return Create<ShippingProfileEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ShippingProviderRule entity</summary>
		public EntityQuery<ShippingProviderRuleEntity> ShippingProviderRule
		{
			get { return Create<ShippingProviderRuleEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ShippingSettings entity</summary>
		public EntityQuery<ShippingSettingsEntity> ShippingSettings
		{
			get { return Create<ShippingSettingsEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ShipSenseKnowledgebase entity</summary>
		public EntityQuery<ShipSenseKnowledgebaseEntity> ShipSenseKnowledgebase
		{
			get { return Create<ShipSenseKnowledgebaseEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ShopifyOrder entity</summary>
		public EntityQuery<ShopifyOrderEntity> ShopifyOrder
		{
			get { return Create<ShopifyOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ShopifyOrderItem entity</summary>
		public EntityQuery<ShopifyOrderItemEntity> ShopifyOrderItem
		{
			get { return Create<ShopifyOrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ShopifyStore entity</summary>
		public EntityQuery<ShopifyStoreEntity> ShopifyStore
		{
			get { return Create<ShopifyStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ShopSiteStore entity</summary>
		public EntityQuery<ShopSiteStoreEntity> ShopSiteStore
		{
			get { return Create<ShopSiteStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the SparkPayStore entity</summary>
		public EntityQuery<SparkPayStoreEntity> SparkPayStore
		{
			get { return Create<SparkPayStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the StatusPreset entity</summary>
		public EntityQuery<StatusPresetEntity> StatusPreset
		{
			get { return Create<StatusPresetEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Store entity</summary>
		public EntityQuery<StoreEntity> Store
		{
			get { return Create<StoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the SystemData entity</summary>
		public EntityQuery<SystemDataEntity> SystemData
		{
			get { return Create<SystemDataEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the Template entity</summary>
		public EntityQuery<TemplateEntity> Template
		{
			get { return Create<TemplateEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the TemplateComputerSettings entity</summary>
		public EntityQuery<TemplateComputerSettingsEntity> TemplateComputerSettings
		{
			get { return Create<TemplateComputerSettingsEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the TemplateFolder entity</summary>
		public EntityQuery<TemplateFolderEntity> TemplateFolder
		{
			get { return Create<TemplateFolderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the TemplateStoreSettings entity</summary>
		public EntityQuery<TemplateStoreSettingsEntity> TemplateStoreSettings
		{
			get { return Create<TemplateStoreSettingsEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the TemplateUserSettings entity</summary>
		public EntityQuery<TemplateUserSettingsEntity> TemplateUserSettings
		{
			get { return Create<TemplateUserSettingsEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ThreeDCartOrder entity</summary>
		public EntityQuery<ThreeDCartOrderEntity> ThreeDCartOrder
		{
			get { return Create<ThreeDCartOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ThreeDCartOrderItem entity</summary>
		public EntityQuery<ThreeDCartOrderItemEntity> ThreeDCartOrderItem
		{
			get { return Create<ThreeDCartOrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ThreeDCartStore entity</summary>
		public EntityQuery<ThreeDCartStoreEntity> ThreeDCartStore
		{
			get { return Create<ThreeDCartStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UpsAccount entity</summary>
		public EntityQuery<UpsAccountEntity> UpsAccount
		{
			get { return Create<UpsAccountEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UpsLetterRate entity</summary>
		public EntityQuery<UpsLetterRateEntity> UpsLetterRate
		{
			get { return Create<UpsLetterRateEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UpsLocalRatingDeliveryAreaSurcharge entity</summary>
		public EntityQuery<UpsLocalRatingDeliveryAreaSurchargeEntity> UpsLocalRatingDeliveryAreaSurcharge
		{
			get { return Create<UpsLocalRatingDeliveryAreaSurchargeEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UpsLocalRatingZone entity</summary>
		public EntityQuery<UpsLocalRatingZoneEntity> UpsLocalRatingZone
		{
			get { return Create<UpsLocalRatingZoneEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UpsLocalRatingZoneFile entity</summary>
		public EntityQuery<UpsLocalRatingZoneFileEntity> UpsLocalRatingZoneFile
		{
			get { return Create<UpsLocalRatingZoneFileEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UpsPackage entity</summary>
		public EntityQuery<UpsPackageEntity> UpsPackage
		{
			get { return Create<UpsPackageEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UpsPackageRate entity</summary>
		public EntityQuery<UpsPackageRateEntity> UpsPackageRate
		{
			get { return Create<UpsPackageRateEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UpsPricePerPound entity</summary>
		public EntityQuery<UpsPricePerPoundEntity> UpsPricePerPound
		{
			get { return Create<UpsPricePerPoundEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UpsProfile entity</summary>
		public EntityQuery<UpsProfileEntity> UpsProfile
		{
			get { return Create<UpsProfileEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UpsProfilePackage entity</summary>
		public EntityQuery<UpsProfilePackageEntity> UpsProfilePackage
		{
			get { return Create<UpsProfilePackageEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UpsRateSurcharge entity</summary>
		public EntityQuery<UpsRateSurchargeEntity> UpsRateSurcharge
		{
			get { return Create<UpsRateSurchargeEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UpsRateTable entity</summary>
		public EntityQuery<UpsRateTableEntity> UpsRateTable
		{
			get { return Create<UpsRateTableEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UpsShipment entity</summary>
		public EntityQuery<UpsShipmentEntity> UpsShipment
		{
			get { return Create<UpsShipmentEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the User entity</summary>
		public EntityQuery<UserEntity> User
		{
			get { return Create<UserEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UserColumnSettings entity</summary>
		public EntityQuery<UserColumnSettingsEntity> UserColumnSettings
		{
			get { return Create<UserColumnSettingsEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UserSettings entity</summary>
		public EntityQuery<UserSettingsEntity> UserSettings
		{
			get { return Create<UserSettingsEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UspsAccount entity</summary>
		public EntityQuery<UspsAccountEntity> UspsAccount
		{
			get { return Create<UspsAccountEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UspsProfile entity</summary>
		public EntityQuery<UspsProfileEntity> UspsProfile
		{
			get { return Create<UspsProfileEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UspsScanForm entity</summary>
		public EntityQuery<UspsScanFormEntity> UspsScanForm
		{
			get { return Create<UspsScanFormEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the UspsShipment entity</summary>
		public EntityQuery<UspsShipmentEntity> UspsShipment
		{
			get { return Create<UspsShipmentEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the ValidatedAddress entity</summary>
		public EntityQuery<ValidatedAddressEntity> ValidatedAddress
		{
			get { return Create<ValidatedAddressEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the VersionSignoff entity</summary>
		public EntityQuery<VersionSignoffEntity> VersionSignoff
		{
			get { return Create<VersionSignoffEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the VolusionStore entity</summary>
		public EntityQuery<VolusionStoreEntity> VolusionStore
		{
			get { return Create<VolusionStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the WalmartOrder entity</summary>
		public EntityQuery<WalmartOrderEntity> WalmartOrder
		{
			get { return Create<WalmartOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the WalmartOrderItem entity</summary>
		public EntityQuery<WalmartOrderItemEntity> WalmartOrderItem
		{
			get { return Create<WalmartOrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the WalmartStore entity</summary>
		public EntityQuery<WalmartStoreEntity> WalmartStore
		{
			get { return Create<WalmartStoreEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the WorldShipGoods entity</summary>
		public EntityQuery<WorldShipGoodsEntity> WorldShipGoods
		{
			get { return Create<WorldShipGoodsEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the WorldShipPackage entity</summary>
		public EntityQuery<WorldShipPackageEntity> WorldShipPackage
		{
			get { return Create<WorldShipPackageEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the WorldShipProcessed entity</summary>
		public EntityQuery<WorldShipProcessedEntity> WorldShipProcessed
		{
			get { return Create<WorldShipProcessedEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the WorldShipShipment entity</summary>
		public EntityQuery<WorldShipShipmentEntity> WorldShipShipment
		{
			get { return Create<WorldShipShipmentEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the YahooOrder entity</summary>
		public EntityQuery<YahooOrderEntity> YahooOrder
		{
			get { return Create<YahooOrderEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the YahooOrderItem entity</summary>
		public EntityQuery<YahooOrderItemEntity> YahooOrderItem
		{
			get { return Create<YahooOrderItemEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the YahooProduct entity</summary>
		public EntityQuery<YahooProductEntity> YahooProduct
		{
			get { return Create<YahooProductEntity>(); }
		}

		/// <summary>Creates and returns a new EntityQuery for the YahooStore entity</summary>
		public EntityQuery<YahooStoreEntity> YahooStore
		{
			get { return Create<YahooStoreEntity>(); }
		}


 
	}
}