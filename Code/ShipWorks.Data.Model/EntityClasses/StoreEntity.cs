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
using System.ComponentModel;
using System.Collections.Generic;
#if !CF
using System.Runtime.Serialization;
#endif
using System.Xml.Serialization;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.RelationClasses;

using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.EntityClasses
{
	// __LLBLGENPRO_USER_CODE_REGION_START AdditionalNamespaces
	// __LLBLGENPRO_USER_CODE_REGION_END
	/// <summary>Entity class which represents the entity 'Store'.<br/><br/></summary>
	[Serializable]
	public partial class StoreEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<AmazonOrderSearchEntity> _amazonOrderSearch;
		private EntityCollection<ChannelAdvisorOrderSearchEntity> _channelAdvisorOrderSearch;
		private EntityCollection<ClickCartProOrderSearchEntity> _clickCartProOrderSearch;
		private EntityCollection<CommerceInterfaceOrderSearchEntity> _commerceInterfaceOrderSearch;
		private EntityCollection<EbayOrderSearchEntity> _ebayOrderSearch;
		private EntityCollection<EtsyOrderSearchEntity> _etsyOrderSearch;
		private EntityCollection<GrouponOrderSearchEntity> _grouponOrderSearch;
		private EntityCollection<LemonStandOrderSearchEntity> _lemonStandOrderSearch;
		private EntityCollection<MagentoOrderSearchEntity> _magentoOrderSearch;
		private EntityCollection<MarketplaceAdvisorOrderSearchEntity> _marketplaceAdvisorOrderSearch;
		private EntityCollection<NetworkSolutionsOrderSearchEntity> _networkSolutionsOrderSearch;
		private EntityCollection<NeweggOrderSearchEntity> _neweggOrderSearch;
		private EntityCollection<OrderMotionOrderSearchEntity> _orderMotionOrderSearch;
		private EntityCollection<OrderSearchEntity> _orderSearch;
		private EntityCollection<PayPalOrderSearchEntity> _payPalOrderSearch;
		private EntityCollection<ProStoresOrderSearchEntity> _proStoresOrderSearch;
		private EntityCollection<SearsOrderSearchEntity> _searsOrderSearch;
		private EntityCollection<ShopifyOrderSearchEntity> _shopifyOrderSearch;
		private EntityCollection<ThreeDCartOrderSearchEntity> _threeDCartOrderSearch;
		private EntityCollection<WalmartOrderSearchEntity> _walmartOrderSearch;
		private EntityCollection<YahooOrderSearchEntity> _yahooOrderSearch;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name AmazonOrderSearch</summary>
			public static readonly string AmazonOrderSearch = "AmazonOrderSearch";
			/// <summary>Member name ChannelAdvisorOrderSearch</summary>
			public static readonly string ChannelAdvisorOrderSearch = "ChannelAdvisorOrderSearch";
			/// <summary>Member name ClickCartProOrderSearch</summary>
			public static readonly string ClickCartProOrderSearch = "ClickCartProOrderSearch";
			/// <summary>Member name CommerceInterfaceOrderSearch</summary>
			public static readonly string CommerceInterfaceOrderSearch = "CommerceInterfaceOrderSearch";
			/// <summary>Member name EbayOrderSearch</summary>
			public static readonly string EbayOrderSearch = "EbayOrderSearch";
			/// <summary>Member name EtsyOrderSearch</summary>
			public static readonly string EtsyOrderSearch = "EtsyOrderSearch";
			/// <summary>Member name GrouponOrderSearch</summary>
			public static readonly string GrouponOrderSearch = "GrouponOrderSearch";
			/// <summary>Member name LemonStandOrderSearch</summary>
			public static readonly string LemonStandOrderSearch = "LemonStandOrderSearch";
			/// <summary>Member name MagentoOrderSearch</summary>
			public static readonly string MagentoOrderSearch = "MagentoOrderSearch";
			/// <summary>Member name MarketplaceAdvisorOrderSearch</summary>
			public static readonly string MarketplaceAdvisorOrderSearch = "MarketplaceAdvisorOrderSearch";
			/// <summary>Member name NetworkSolutionsOrderSearch</summary>
			public static readonly string NetworkSolutionsOrderSearch = "NetworkSolutionsOrderSearch";
			/// <summary>Member name NeweggOrderSearch</summary>
			public static readonly string NeweggOrderSearch = "NeweggOrderSearch";
			/// <summary>Member name OrderMotionOrderSearch</summary>
			public static readonly string OrderMotionOrderSearch = "OrderMotionOrderSearch";
			/// <summary>Member name OrderSearch</summary>
			public static readonly string OrderSearch = "OrderSearch";
			/// <summary>Member name PayPalOrderSearch</summary>
			public static readonly string PayPalOrderSearch = "PayPalOrderSearch";
			/// <summary>Member name ProStoresOrderSearch</summary>
			public static readonly string ProStoresOrderSearch = "ProStoresOrderSearch";
			/// <summary>Member name SearsOrderSearch</summary>
			public static readonly string SearsOrderSearch = "SearsOrderSearch";
			/// <summary>Member name ShopifyOrderSearch</summary>
			public static readonly string ShopifyOrderSearch = "ShopifyOrderSearch";
			/// <summary>Member name ThreeDCartOrderSearch</summary>
			public static readonly string ThreeDCartOrderSearch = "ThreeDCartOrderSearch";
			/// <summary>Member name WalmartOrderSearch</summary>
			public static readonly string WalmartOrderSearch = "WalmartOrderSearch";
			/// <summary>Member name YahooOrderSearch</summary>
			public static readonly string YahooOrderSearch = "YahooOrderSearch";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static StoreEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public StoreEntity():base("StoreEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public StoreEntity(IEntityFields2 fields):base("StoreEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this StoreEntity</param>
		public StoreEntity(IValidator validator):base("StoreEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="storeID">PK value for Store which data should be fetched into this Store object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public StoreEntity(System.Int64 storeID):base("StoreEntity")
		{
			InitClassEmpty(null, null);
			this.StoreID = storeID;
		}

		/// <summary> CTor</summary>
		/// <param name="storeID">PK value for Store which data should be fetched into this Store object</param>
		/// <param name="validator">The custom validator object for this StoreEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public StoreEntity(System.Int64 storeID, IValidator validator):base("StoreEntity")
		{
			InitClassEmpty(validator, null);
			this.StoreID = storeID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected StoreEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_amazonOrderSearch = (EntityCollection<AmazonOrderSearchEntity>)info.GetValue("_amazonOrderSearch", typeof(EntityCollection<AmazonOrderSearchEntity>));
				_channelAdvisorOrderSearch = (EntityCollection<ChannelAdvisorOrderSearchEntity>)info.GetValue("_channelAdvisorOrderSearch", typeof(EntityCollection<ChannelAdvisorOrderSearchEntity>));
				_clickCartProOrderSearch = (EntityCollection<ClickCartProOrderSearchEntity>)info.GetValue("_clickCartProOrderSearch", typeof(EntityCollection<ClickCartProOrderSearchEntity>));
				_commerceInterfaceOrderSearch = (EntityCollection<CommerceInterfaceOrderSearchEntity>)info.GetValue("_commerceInterfaceOrderSearch", typeof(EntityCollection<CommerceInterfaceOrderSearchEntity>));
				_ebayOrderSearch = (EntityCollection<EbayOrderSearchEntity>)info.GetValue("_ebayOrderSearch", typeof(EntityCollection<EbayOrderSearchEntity>));
				_etsyOrderSearch = (EntityCollection<EtsyOrderSearchEntity>)info.GetValue("_etsyOrderSearch", typeof(EntityCollection<EtsyOrderSearchEntity>));
				_grouponOrderSearch = (EntityCollection<GrouponOrderSearchEntity>)info.GetValue("_grouponOrderSearch", typeof(EntityCollection<GrouponOrderSearchEntity>));
				_lemonStandOrderSearch = (EntityCollection<LemonStandOrderSearchEntity>)info.GetValue("_lemonStandOrderSearch", typeof(EntityCollection<LemonStandOrderSearchEntity>));
				_magentoOrderSearch = (EntityCollection<MagentoOrderSearchEntity>)info.GetValue("_magentoOrderSearch", typeof(EntityCollection<MagentoOrderSearchEntity>));
				_marketplaceAdvisorOrderSearch = (EntityCollection<MarketplaceAdvisorOrderSearchEntity>)info.GetValue("_marketplaceAdvisorOrderSearch", typeof(EntityCollection<MarketplaceAdvisorOrderSearchEntity>));
				_networkSolutionsOrderSearch = (EntityCollection<NetworkSolutionsOrderSearchEntity>)info.GetValue("_networkSolutionsOrderSearch", typeof(EntityCollection<NetworkSolutionsOrderSearchEntity>));
				_neweggOrderSearch = (EntityCollection<NeweggOrderSearchEntity>)info.GetValue("_neweggOrderSearch", typeof(EntityCollection<NeweggOrderSearchEntity>));
				_orderMotionOrderSearch = (EntityCollection<OrderMotionOrderSearchEntity>)info.GetValue("_orderMotionOrderSearch", typeof(EntityCollection<OrderMotionOrderSearchEntity>));
				_orderSearch = (EntityCollection<OrderSearchEntity>)info.GetValue("_orderSearch", typeof(EntityCollection<OrderSearchEntity>));
				_payPalOrderSearch = (EntityCollection<PayPalOrderSearchEntity>)info.GetValue("_payPalOrderSearch", typeof(EntityCollection<PayPalOrderSearchEntity>));
				_proStoresOrderSearch = (EntityCollection<ProStoresOrderSearchEntity>)info.GetValue("_proStoresOrderSearch", typeof(EntityCollection<ProStoresOrderSearchEntity>));
				_searsOrderSearch = (EntityCollection<SearsOrderSearchEntity>)info.GetValue("_searsOrderSearch", typeof(EntityCollection<SearsOrderSearchEntity>));
				_shopifyOrderSearch = (EntityCollection<ShopifyOrderSearchEntity>)info.GetValue("_shopifyOrderSearch", typeof(EntityCollection<ShopifyOrderSearchEntity>));
				_threeDCartOrderSearch = (EntityCollection<ThreeDCartOrderSearchEntity>)info.GetValue("_threeDCartOrderSearch", typeof(EntityCollection<ThreeDCartOrderSearchEntity>));
				_walmartOrderSearch = (EntityCollection<WalmartOrderSearchEntity>)info.GetValue("_walmartOrderSearch", typeof(EntityCollection<WalmartOrderSearchEntity>));
				_yahooOrderSearch = (EntityCollection<YahooOrderSearchEntity>)info.GetValue("_yahooOrderSearch", typeof(EntityCollection<YahooOrderSearchEntity>));
				this.FixupDeserialization(FieldInfoProviderSingleton.GetInstance());
			}
			// __LLBLGENPRO_USER_CODE_REGION_START DeserializationConstructor
			// __LLBLGENPRO_USER_CODE_REGION_END
		}


		/// <summary> Sets the related entity property to the entity specified. If the property is a collection, it will add the entity specified to that collection.</summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="entity">Entity to set as an related entity</param>
		/// <remarks>Used by prefetch path logic.</remarks>
		protected override void SetRelatedEntityProperty(string propertyName, IEntityCore entity)
		{
			switch(propertyName)
			{
				case "AmazonOrderSearch":
					this.AmazonOrderSearch.Add((AmazonOrderSearchEntity)entity);
					break;
				case "ChannelAdvisorOrderSearch":
					this.ChannelAdvisorOrderSearch.Add((ChannelAdvisorOrderSearchEntity)entity);
					break;
				case "ClickCartProOrderSearch":
					this.ClickCartProOrderSearch.Add((ClickCartProOrderSearchEntity)entity);
					break;
				case "CommerceInterfaceOrderSearch":
					this.CommerceInterfaceOrderSearch.Add((CommerceInterfaceOrderSearchEntity)entity);
					break;
				case "EbayOrderSearch":
					this.EbayOrderSearch.Add((EbayOrderSearchEntity)entity);
					break;
				case "EtsyOrderSearch":
					this.EtsyOrderSearch.Add((EtsyOrderSearchEntity)entity);
					break;
				case "GrouponOrderSearch":
					this.GrouponOrderSearch.Add((GrouponOrderSearchEntity)entity);
					break;
				case "LemonStandOrderSearch":
					this.LemonStandOrderSearch.Add((LemonStandOrderSearchEntity)entity);
					break;
				case "MagentoOrderSearch":
					this.MagentoOrderSearch.Add((MagentoOrderSearchEntity)entity);
					break;
				case "MarketplaceAdvisorOrderSearch":
					this.MarketplaceAdvisorOrderSearch.Add((MarketplaceAdvisorOrderSearchEntity)entity);
					break;
				case "NetworkSolutionsOrderSearch":
					this.NetworkSolutionsOrderSearch.Add((NetworkSolutionsOrderSearchEntity)entity);
					break;
				case "NeweggOrderSearch":
					this.NeweggOrderSearch.Add((NeweggOrderSearchEntity)entity);
					break;
				case "OrderMotionOrderSearch":
					this.OrderMotionOrderSearch.Add((OrderMotionOrderSearchEntity)entity);
					break;
				case "OrderSearch":
					this.OrderSearch.Add((OrderSearchEntity)entity);
					break;
				case "PayPalOrderSearch":
					this.PayPalOrderSearch.Add((PayPalOrderSearchEntity)entity);
					break;
				case "ProStoresOrderSearch":
					this.ProStoresOrderSearch.Add((ProStoresOrderSearchEntity)entity);
					break;
				case "SearsOrderSearch":
					this.SearsOrderSearch.Add((SearsOrderSearchEntity)entity);
					break;
				case "ShopifyOrderSearch":
					this.ShopifyOrderSearch.Add((ShopifyOrderSearchEntity)entity);
					break;
				case "ThreeDCartOrderSearch":
					this.ThreeDCartOrderSearch.Add((ThreeDCartOrderSearchEntity)entity);
					break;
				case "WalmartOrderSearch":
					this.WalmartOrderSearch.Add((WalmartOrderSearchEntity)entity);
					break;
				case "YahooOrderSearch":
					this.YahooOrderSearch.Add((YahooOrderSearchEntity)entity);
					break;
				default:
					this.OnSetRelatedEntityProperty(propertyName, entity);
					break;
			}
		}
		
		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		protected override RelationCollection GetRelationsForFieldOfType(string fieldName)
		{
			return GetRelationsForField(fieldName);
		}

		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		internal static RelationCollection GetRelationsForField(string fieldName)
		{
			RelationCollection toReturn = new RelationCollection();
			switch(fieldName)
			{
				case "AmazonOrderSearch":
					toReturn.Add(Relations.AmazonOrderSearchEntityUsingStoreID);
					break;
				case "ChannelAdvisorOrderSearch":
					toReturn.Add(Relations.ChannelAdvisorOrderSearchEntityUsingStoreID);
					break;
				case "ClickCartProOrderSearch":
					toReturn.Add(Relations.ClickCartProOrderSearchEntityUsingStoreID);
					break;
				case "CommerceInterfaceOrderSearch":
					toReturn.Add(Relations.CommerceInterfaceOrderSearchEntityUsingStoreID);
					break;
				case "EbayOrderSearch":
					toReturn.Add(Relations.EbayOrderSearchEntityUsingStoreID);
					break;
				case "EtsyOrderSearch":
					toReturn.Add(Relations.EtsyOrderSearchEntityUsingStoreID);
					break;
				case "GrouponOrderSearch":
					toReturn.Add(Relations.GrouponOrderSearchEntityUsingStoreID);
					break;
				case "LemonStandOrderSearch":
					toReturn.Add(Relations.LemonStandOrderSearchEntityUsingStoreID);
					break;
				case "MagentoOrderSearch":
					toReturn.Add(Relations.MagentoOrderSearchEntityUsingStoreID);
					break;
				case "MarketplaceAdvisorOrderSearch":
					toReturn.Add(Relations.MarketplaceAdvisorOrderSearchEntityUsingStoreID);
					break;
				case "NetworkSolutionsOrderSearch":
					toReturn.Add(Relations.NetworkSolutionsOrderSearchEntityUsingStoreID);
					break;
				case "NeweggOrderSearch":
					toReturn.Add(Relations.NeweggOrderSearchEntityUsingStoreID);
					break;
				case "OrderMotionOrderSearch":
					toReturn.Add(Relations.OrderMotionOrderSearchEntityUsingStoreID);
					break;
				case "OrderSearch":
					toReturn.Add(Relations.OrderSearchEntityUsingStoreID);
					break;
				case "PayPalOrderSearch":
					toReturn.Add(Relations.PayPalOrderSearchEntityUsingStoreID);
					break;
				case "ProStoresOrderSearch":
					toReturn.Add(Relations.ProStoresOrderSearchEntityUsingStoreID);
					break;
				case "SearsOrderSearch":
					toReturn.Add(Relations.SearsOrderSearchEntityUsingStoreID);
					break;
				case "ShopifyOrderSearch":
					toReturn.Add(Relations.ShopifyOrderSearchEntityUsingStoreID);
					break;
				case "ThreeDCartOrderSearch":
					toReturn.Add(Relations.ThreeDCartOrderSearchEntityUsingStoreID);
					break;
				case "WalmartOrderSearch":
					toReturn.Add(Relations.WalmartOrderSearchEntityUsingStoreID);
					break;
				case "YahooOrderSearch":
					toReturn.Add(Relations.YahooOrderSearchEntityUsingStoreID);
					break;
				default:
					break;				
			}
			return toReturn;
		}
#if !CF
		/// <summary>Checks if the relation mapped by the property with the name specified is a one way / single sided relation. If the passed in name is null, it/ will return true if the entity has any single-sided relation</summary>
		/// <param name="propertyName">Name of the property which is mapped onto the relation to check, or null to check if the entity has any relation/ which is single sided</param>
		/// <returns>true if the relation is single sided / one way (so the opposite relation isn't present), false otherwise</returns>
		protected override bool CheckOneWayRelations(string propertyName)
		{
			int numberOfOneWayRelations = 0;
			switch(propertyName)
			{
				case null:
					return ((numberOfOneWayRelations > 0) || base.CheckOneWayRelations(null));
				default:
					return base.CheckOneWayRelations(propertyName);
			}
		}
#endif
		/// <summary> Sets the internal parameter related to the fieldname passed to the instance relatedEntity. </summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		/// <param name="fieldName">Name of field mapped onto the relation which resolves in the instance relatedEntity</param>
		protected override void SetRelatedEntity(IEntityCore relatedEntity, string fieldName)
		{
			switch(fieldName)
			{
				case "AmazonOrderSearch":
					this.AmazonOrderSearch.Add((AmazonOrderSearchEntity)relatedEntity);
					break;
				case "ChannelAdvisorOrderSearch":
					this.ChannelAdvisorOrderSearch.Add((ChannelAdvisorOrderSearchEntity)relatedEntity);
					break;
				case "ClickCartProOrderSearch":
					this.ClickCartProOrderSearch.Add((ClickCartProOrderSearchEntity)relatedEntity);
					break;
				case "CommerceInterfaceOrderSearch":
					this.CommerceInterfaceOrderSearch.Add((CommerceInterfaceOrderSearchEntity)relatedEntity);
					break;
				case "EbayOrderSearch":
					this.EbayOrderSearch.Add((EbayOrderSearchEntity)relatedEntity);
					break;
				case "EtsyOrderSearch":
					this.EtsyOrderSearch.Add((EtsyOrderSearchEntity)relatedEntity);
					break;
				case "GrouponOrderSearch":
					this.GrouponOrderSearch.Add((GrouponOrderSearchEntity)relatedEntity);
					break;
				case "LemonStandOrderSearch":
					this.LemonStandOrderSearch.Add((LemonStandOrderSearchEntity)relatedEntity);
					break;
				case "MagentoOrderSearch":
					this.MagentoOrderSearch.Add((MagentoOrderSearchEntity)relatedEntity);
					break;
				case "MarketplaceAdvisorOrderSearch":
					this.MarketplaceAdvisorOrderSearch.Add((MarketplaceAdvisorOrderSearchEntity)relatedEntity);
					break;
				case "NetworkSolutionsOrderSearch":
					this.NetworkSolutionsOrderSearch.Add((NetworkSolutionsOrderSearchEntity)relatedEntity);
					break;
				case "NeweggOrderSearch":
					this.NeweggOrderSearch.Add((NeweggOrderSearchEntity)relatedEntity);
					break;
				case "OrderMotionOrderSearch":
					this.OrderMotionOrderSearch.Add((OrderMotionOrderSearchEntity)relatedEntity);
					break;
				case "OrderSearch":
					this.OrderSearch.Add((OrderSearchEntity)relatedEntity);
					break;
				case "PayPalOrderSearch":
					this.PayPalOrderSearch.Add((PayPalOrderSearchEntity)relatedEntity);
					break;
				case "ProStoresOrderSearch":
					this.ProStoresOrderSearch.Add((ProStoresOrderSearchEntity)relatedEntity);
					break;
				case "SearsOrderSearch":
					this.SearsOrderSearch.Add((SearsOrderSearchEntity)relatedEntity);
					break;
				case "ShopifyOrderSearch":
					this.ShopifyOrderSearch.Add((ShopifyOrderSearchEntity)relatedEntity);
					break;
				case "ThreeDCartOrderSearch":
					this.ThreeDCartOrderSearch.Add((ThreeDCartOrderSearchEntity)relatedEntity);
					break;
				case "WalmartOrderSearch":
					this.WalmartOrderSearch.Add((WalmartOrderSearchEntity)relatedEntity);
					break;
				case "YahooOrderSearch":
					this.YahooOrderSearch.Add((YahooOrderSearchEntity)relatedEntity);
					break;
				default:
					break;
			}
		}

		/// <summary> Unsets the internal parameter related to the fieldname passed to the instance relatedEntity. Reverses the actions taken by SetRelatedEntity() </summary>
		/// <param name="relatedEntity">Instance to unset as the related entity of type entityType</param>
		/// <param name="fieldName">Name of field mapped onto the relation which resolves in the instance relatedEntity</param>
		/// <param name="signalRelatedEntityManyToOne">if set to true it will notify the manytoone side, if applicable.</param>
		protected override void UnsetRelatedEntity(IEntityCore relatedEntity, string fieldName, bool signalRelatedEntityManyToOne)
		{
			switch(fieldName)
			{
				case "AmazonOrderSearch":
					this.PerformRelatedEntityRemoval(this.AmazonOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "ChannelAdvisorOrderSearch":
					this.PerformRelatedEntityRemoval(this.ChannelAdvisorOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "ClickCartProOrderSearch":
					this.PerformRelatedEntityRemoval(this.ClickCartProOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "CommerceInterfaceOrderSearch":
					this.PerformRelatedEntityRemoval(this.CommerceInterfaceOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "EbayOrderSearch":
					this.PerformRelatedEntityRemoval(this.EbayOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "EtsyOrderSearch":
					this.PerformRelatedEntityRemoval(this.EtsyOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "GrouponOrderSearch":
					this.PerformRelatedEntityRemoval(this.GrouponOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "LemonStandOrderSearch":
					this.PerformRelatedEntityRemoval(this.LemonStandOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "MagentoOrderSearch":
					this.PerformRelatedEntityRemoval(this.MagentoOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "MarketplaceAdvisorOrderSearch":
					this.PerformRelatedEntityRemoval(this.MarketplaceAdvisorOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "NetworkSolutionsOrderSearch":
					this.PerformRelatedEntityRemoval(this.NetworkSolutionsOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "NeweggOrderSearch":
					this.PerformRelatedEntityRemoval(this.NeweggOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "OrderMotionOrderSearch":
					this.PerformRelatedEntityRemoval(this.OrderMotionOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "OrderSearch":
					this.PerformRelatedEntityRemoval(this.OrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "PayPalOrderSearch":
					this.PerformRelatedEntityRemoval(this.PayPalOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "ProStoresOrderSearch":
					this.PerformRelatedEntityRemoval(this.ProStoresOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "SearsOrderSearch":
					this.PerformRelatedEntityRemoval(this.SearsOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "ShopifyOrderSearch":
					this.PerformRelatedEntityRemoval(this.ShopifyOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "ThreeDCartOrderSearch":
					this.PerformRelatedEntityRemoval(this.ThreeDCartOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "WalmartOrderSearch":
					this.PerformRelatedEntityRemoval(this.WalmartOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "YahooOrderSearch":
					this.PerformRelatedEntityRemoval(this.YahooOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				default:
					break;
			}
		}

		/// <summary> Gets a collection of related entities referenced by this entity which depend on this entity (this entity is the PK side of their FK fields). These entities will have to be persisted after this entity during a recursive save.</summary>
		/// <returns>Collection with 0 or more IEntity2 objects, referenced by this entity</returns>
		protected override List<IEntity2> GetDependingRelatedEntities()
		{
			List<IEntity2> toReturn = new List<IEntity2>();
			return toReturn;
		}
		
		/// <summary> Gets a collection of related entities referenced by this entity which this entity depends on (this entity is the FK side of their PK fields). These
		/// entities will have to be persisted before this entity during a recursive save.</summary>
		/// <returns>Collection with 0 or more IEntity2 objects, referenced by this entity</returns>
		protected override List<IEntity2> GetDependentRelatedEntities()
		{
			List<IEntity2> toReturn = new List<IEntity2>();
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.Add(this.AmazonOrderSearch);
			toReturn.Add(this.ChannelAdvisorOrderSearch);
			toReturn.Add(this.ClickCartProOrderSearch);
			toReturn.Add(this.CommerceInterfaceOrderSearch);
			toReturn.Add(this.EbayOrderSearch);
			toReturn.Add(this.EtsyOrderSearch);
			toReturn.Add(this.GrouponOrderSearch);
			toReturn.Add(this.LemonStandOrderSearch);
			toReturn.Add(this.MagentoOrderSearch);
			toReturn.Add(this.MarketplaceAdvisorOrderSearch);
			toReturn.Add(this.NetworkSolutionsOrderSearch);
			toReturn.Add(this.NeweggOrderSearch);
			toReturn.Add(this.OrderMotionOrderSearch);
			toReturn.Add(this.OrderSearch);
			toReturn.Add(this.PayPalOrderSearch);
			toReturn.Add(this.ProStoresOrderSearch);
			toReturn.Add(this.SearsOrderSearch);
			toReturn.Add(this.ShopifyOrderSearch);
			toReturn.Add(this.ThreeDCartOrderSearch);
			toReturn.Add(this.WalmartOrderSearch);
			toReturn.Add(this.YahooOrderSearch);
			return toReturn;
		}

		/// <summary>Gets a predicateexpression which filters on this entity</summary>
		/// <returns>ready to use predicateexpression</returns>
		/// <remarks>Only useful in entity fetches.</remarks>
		public  static IPredicateExpression GetEntityTypeFilter()
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetEntityTypeFilter("StoreEntity", false);
		}
		
		/// <summary>Gets a predicateexpression which filters on this entity</summary>
		/// <param name="negate">Flag to produce a NOT filter, (true), or a normal filter (false). </param>
		/// <returns>ready to use predicateexpression</returns>
		/// <remarks>Only useful in entity fetches.</remarks>
		public  static IPredicateExpression GetEntityTypeFilter(bool negate)
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetEntityTypeFilter("StoreEntity", negate);
		}

		/// <summary>ISerializable member. Does custom serialization so event handlers do not get serialized. Serializes members of this entity class and uses the base class' implementation to serialize the rest.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				info.AddValue("_amazonOrderSearch", ((_amazonOrderSearch!=null) && (_amazonOrderSearch.Count>0) && !this.MarkedForDeletion)?_amazonOrderSearch:null);
				info.AddValue("_channelAdvisorOrderSearch", ((_channelAdvisorOrderSearch!=null) && (_channelAdvisorOrderSearch.Count>0) && !this.MarkedForDeletion)?_channelAdvisorOrderSearch:null);
				info.AddValue("_clickCartProOrderSearch", ((_clickCartProOrderSearch!=null) && (_clickCartProOrderSearch.Count>0) && !this.MarkedForDeletion)?_clickCartProOrderSearch:null);
				info.AddValue("_commerceInterfaceOrderSearch", ((_commerceInterfaceOrderSearch!=null) && (_commerceInterfaceOrderSearch.Count>0) && !this.MarkedForDeletion)?_commerceInterfaceOrderSearch:null);
				info.AddValue("_ebayOrderSearch", ((_ebayOrderSearch!=null) && (_ebayOrderSearch.Count>0) && !this.MarkedForDeletion)?_ebayOrderSearch:null);
				info.AddValue("_etsyOrderSearch", ((_etsyOrderSearch!=null) && (_etsyOrderSearch.Count>0) && !this.MarkedForDeletion)?_etsyOrderSearch:null);
				info.AddValue("_grouponOrderSearch", ((_grouponOrderSearch!=null) && (_grouponOrderSearch.Count>0) && !this.MarkedForDeletion)?_grouponOrderSearch:null);
				info.AddValue("_lemonStandOrderSearch", ((_lemonStandOrderSearch!=null) && (_lemonStandOrderSearch.Count>0) && !this.MarkedForDeletion)?_lemonStandOrderSearch:null);
				info.AddValue("_magentoOrderSearch", ((_magentoOrderSearch!=null) && (_magentoOrderSearch.Count>0) && !this.MarkedForDeletion)?_magentoOrderSearch:null);
				info.AddValue("_marketplaceAdvisorOrderSearch", ((_marketplaceAdvisorOrderSearch!=null) && (_marketplaceAdvisorOrderSearch.Count>0) && !this.MarkedForDeletion)?_marketplaceAdvisorOrderSearch:null);
				info.AddValue("_networkSolutionsOrderSearch", ((_networkSolutionsOrderSearch!=null) && (_networkSolutionsOrderSearch.Count>0) && !this.MarkedForDeletion)?_networkSolutionsOrderSearch:null);
				info.AddValue("_neweggOrderSearch", ((_neweggOrderSearch!=null) && (_neweggOrderSearch.Count>0) && !this.MarkedForDeletion)?_neweggOrderSearch:null);
				info.AddValue("_orderMotionOrderSearch", ((_orderMotionOrderSearch!=null) && (_orderMotionOrderSearch.Count>0) && !this.MarkedForDeletion)?_orderMotionOrderSearch:null);
				info.AddValue("_orderSearch", ((_orderSearch!=null) && (_orderSearch.Count>0) && !this.MarkedForDeletion)?_orderSearch:null);
				info.AddValue("_payPalOrderSearch", ((_payPalOrderSearch!=null) && (_payPalOrderSearch.Count>0) && !this.MarkedForDeletion)?_payPalOrderSearch:null);
				info.AddValue("_proStoresOrderSearch", ((_proStoresOrderSearch!=null) && (_proStoresOrderSearch.Count>0) && !this.MarkedForDeletion)?_proStoresOrderSearch:null);
				info.AddValue("_searsOrderSearch", ((_searsOrderSearch!=null) && (_searsOrderSearch.Count>0) && !this.MarkedForDeletion)?_searsOrderSearch:null);
				info.AddValue("_shopifyOrderSearch", ((_shopifyOrderSearch!=null) && (_shopifyOrderSearch.Count>0) && !this.MarkedForDeletion)?_shopifyOrderSearch:null);
				info.AddValue("_threeDCartOrderSearch", ((_threeDCartOrderSearch!=null) && (_threeDCartOrderSearch.Count>0) && !this.MarkedForDeletion)?_threeDCartOrderSearch:null);
				info.AddValue("_walmartOrderSearch", ((_walmartOrderSearch!=null) && (_walmartOrderSearch.Count>0) && !this.MarkedForDeletion)?_walmartOrderSearch:null);
				info.AddValue("_yahooOrderSearch", ((_yahooOrderSearch!=null) && (_yahooOrderSearch.Count>0) && !this.MarkedForDeletion)?_yahooOrderSearch:null);
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}

		
		/// <summary>Determines whether this entity is a subType of the entity represented by the passed in enum value, which represents a value in the ShipWorks.Data.Model.EntityType enum</summary>
		/// <param name="typeOfEntity">Type of entity.</param>
		/// <returns>true if the passed in type is a supertype of this entity, otherwise false</returns>
		protected override bool CheckIfIsSubTypeOf(int typeOfEntity)
		{
			return InheritanceInfoProviderSingleton.GetInstance().CheckIfIsSubTypeOf("StoreEntity", ((ShipWorks.Data.Model.EntityType)typeOfEntity).ToString());
		}
				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new StoreRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'AmazonOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoAmazonOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(AmazonOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'ChannelAdvisorOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoChannelAdvisorOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ChannelAdvisorOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'ClickCartProOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoClickCartProOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ClickCartProOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'CommerceInterfaceOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoCommerceInterfaceOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(CommerceInterfaceOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'EbayOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoEbayOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(EbayOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'EtsyOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoEtsyOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(EtsyOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'GrouponOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoGrouponOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(GrouponOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'LemonStandOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoLemonStandOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(LemonStandOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'MagentoOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoMagentoOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(MagentoOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'MarketplaceAdvisorOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoMarketplaceAdvisorOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(MarketplaceAdvisorOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'NetworkSolutionsOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoNetworkSolutionsOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(NetworkSolutionsOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'NeweggOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoNeweggOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(NeweggOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'OrderMotionOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoOrderMotionOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(OrderMotionOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'OrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(OrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'PayPalOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoPayPalOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(PayPalOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'ProStoresOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoProStoresOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ProStoresOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'SearsOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoSearsOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(SearsOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'ShopifyOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoShopifyOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ShopifyOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'ThreeDCartOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoThreeDCartOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ThreeDCartOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'WalmartOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoWalmartOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(WalmartOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'YahooOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoYahooOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(YahooOrderSearchFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(StoreEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._amazonOrderSearch);
			collectionsQueue.Enqueue(this._channelAdvisorOrderSearch);
			collectionsQueue.Enqueue(this._clickCartProOrderSearch);
			collectionsQueue.Enqueue(this._commerceInterfaceOrderSearch);
			collectionsQueue.Enqueue(this._ebayOrderSearch);
			collectionsQueue.Enqueue(this._etsyOrderSearch);
			collectionsQueue.Enqueue(this._grouponOrderSearch);
			collectionsQueue.Enqueue(this._lemonStandOrderSearch);
			collectionsQueue.Enqueue(this._magentoOrderSearch);
			collectionsQueue.Enqueue(this._marketplaceAdvisorOrderSearch);
			collectionsQueue.Enqueue(this._networkSolutionsOrderSearch);
			collectionsQueue.Enqueue(this._neweggOrderSearch);
			collectionsQueue.Enqueue(this._orderMotionOrderSearch);
			collectionsQueue.Enqueue(this._orderSearch);
			collectionsQueue.Enqueue(this._payPalOrderSearch);
			collectionsQueue.Enqueue(this._proStoresOrderSearch);
			collectionsQueue.Enqueue(this._searsOrderSearch);
			collectionsQueue.Enqueue(this._shopifyOrderSearch);
			collectionsQueue.Enqueue(this._threeDCartOrderSearch);
			collectionsQueue.Enqueue(this._walmartOrderSearch);
			collectionsQueue.Enqueue(this._yahooOrderSearch);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._amazonOrderSearch = (EntityCollection<AmazonOrderSearchEntity>) collectionsQueue.Dequeue();
			this._channelAdvisorOrderSearch = (EntityCollection<ChannelAdvisorOrderSearchEntity>) collectionsQueue.Dequeue();
			this._clickCartProOrderSearch = (EntityCollection<ClickCartProOrderSearchEntity>) collectionsQueue.Dequeue();
			this._commerceInterfaceOrderSearch = (EntityCollection<CommerceInterfaceOrderSearchEntity>) collectionsQueue.Dequeue();
			this._ebayOrderSearch = (EntityCollection<EbayOrderSearchEntity>) collectionsQueue.Dequeue();
			this._etsyOrderSearch = (EntityCollection<EtsyOrderSearchEntity>) collectionsQueue.Dequeue();
			this._grouponOrderSearch = (EntityCollection<GrouponOrderSearchEntity>) collectionsQueue.Dequeue();
			this._lemonStandOrderSearch = (EntityCollection<LemonStandOrderSearchEntity>) collectionsQueue.Dequeue();
			this._magentoOrderSearch = (EntityCollection<MagentoOrderSearchEntity>) collectionsQueue.Dequeue();
			this._marketplaceAdvisorOrderSearch = (EntityCollection<MarketplaceAdvisorOrderSearchEntity>) collectionsQueue.Dequeue();
			this._networkSolutionsOrderSearch = (EntityCollection<NetworkSolutionsOrderSearchEntity>) collectionsQueue.Dequeue();
			this._neweggOrderSearch = (EntityCollection<NeweggOrderSearchEntity>) collectionsQueue.Dequeue();
			this._orderMotionOrderSearch = (EntityCollection<OrderMotionOrderSearchEntity>) collectionsQueue.Dequeue();
			this._orderSearch = (EntityCollection<OrderSearchEntity>) collectionsQueue.Dequeue();
			this._payPalOrderSearch = (EntityCollection<PayPalOrderSearchEntity>) collectionsQueue.Dequeue();
			this._proStoresOrderSearch = (EntityCollection<ProStoresOrderSearchEntity>) collectionsQueue.Dequeue();
			this._searsOrderSearch = (EntityCollection<SearsOrderSearchEntity>) collectionsQueue.Dequeue();
			this._shopifyOrderSearch = (EntityCollection<ShopifyOrderSearchEntity>) collectionsQueue.Dequeue();
			this._threeDCartOrderSearch = (EntityCollection<ThreeDCartOrderSearchEntity>) collectionsQueue.Dequeue();
			this._walmartOrderSearch = (EntityCollection<WalmartOrderSearchEntity>) collectionsQueue.Dequeue();
			this._yahooOrderSearch = (EntityCollection<YahooOrderSearchEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._amazonOrderSearch != null);
			toReturn |=(this._channelAdvisorOrderSearch != null);
			toReturn |=(this._clickCartProOrderSearch != null);
			toReturn |=(this._commerceInterfaceOrderSearch != null);
			toReturn |=(this._ebayOrderSearch != null);
			toReturn |=(this._etsyOrderSearch != null);
			toReturn |=(this._grouponOrderSearch != null);
			toReturn |=(this._lemonStandOrderSearch != null);
			toReturn |=(this._magentoOrderSearch != null);
			toReturn |=(this._marketplaceAdvisorOrderSearch != null);
			toReturn |=(this._networkSolutionsOrderSearch != null);
			toReturn |=(this._neweggOrderSearch != null);
			toReturn |=(this._orderMotionOrderSearch != null);
			toReturn |=(this._orderSearch != null);
			toReturn |=(this._payPalOrderSearch != null);
			toReturn |=(this._proStoresOrderSearch != null);
			toReturn |=(this._searsOrderSearch != null);
			toReturn |=(this._shopifyOrderSearch != null);
			toReturn |=(this._threeDCartOrderSearch != null);
			toReturn |=(this._walmartOrderSearch != null);
			toReturn |=(this._yahooOrderSearch != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<AmazonOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(AmazonOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ChannelAdvisorOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ChannelAdvisorOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ClickCartProOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ClickCartProOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<CommerceInterfaceOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(CommerceInterfaceOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<EbayOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EbayOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<EtsyOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EtsyOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<GrouponOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(GrouponOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<LemonStandOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(LemonStandOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<MagentoOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(MagentoOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<MarketplaceAdvisorOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(MarketplaceAdvisorOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<NetworkSolutionsOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(NetworkSolutionsOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<NeweggOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(NeweggOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<OrderMotionOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderMotionOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<OrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<PayPalOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(PayPalOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ProStoresOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ProStoresOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<SearsOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(SearsOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ShopifyOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ShopifyOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ThreeDCartOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ThreeDCartOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<WalmartOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(WalmartOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<YahooOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(YahooOrderSearchEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("AmazonOrderSearch", _amazonOrderSearch);
			toReturn.Add("ChannelAdvisorOrderSearch", _channelAdvisorOrderSearch);
			toReturn.Add("ClickCartProOrderSearch", _clickCartProOrderSearch);
			toReturn.Add("CommerceInterfaceOrderSearch", _commerceInterfaceOrderSearch);
			toReturn.Add("EbayOrderSearch", _ebayOrderSearch);
			toReturn.Add("EtsyOrderSearch", _etsyOrderSearch);
			toReturn.Add("GrouponOrderSearch", _grouponOrderSearch);
			toReturn.Add("LemonStandOrderSearch", _lemonStandOrderSearch);
			toReturn.Add("MagentoOrderSearch", _magentoOrderSearch);
			toReturn.Add("MarketplaceAdvisorOrderSearch", _marketplaceAdvisorOrderSearch);
			toReturn.Add("NetworkSolutionsOrderSearch", _networkSolutionsOrderSearch);
			toReturn.Add("NeweggOrderSearch", _neweggOrderSearch);
			toReturn.Add("OrderMotionOrderSearch", _orderMotionOrderSearch);
			toReturn.Add("OrderSearch", _orderSearch);
			toReturn.Add("PayPalOrderSearch", _payPalOrderSearch);
			toReturn.Add("ProStoresOrderSearch", _proStoresOrderSearch);
			toReturn.Add("SearsOrderSearch", _searsOrderSearch);
			toReturn.Add("ShopifyOrderSearch", _shopifyOrderSearch);
			toReturn.Add("ThreeDCartOrderSearch", _threeDCartOrderSearch);
			toReturn.Add("WalmartOrderSearch", _walmartOrderSearch);
			toReturn.Add("YahooOrderSearch", _yahooOrderSearch);
			return toReturn;
		}

		/// <summary> Initializes the class members</summary>
		private void InitClassMembers()
		{
			PerformDependencyInjection();
			
			// __LLBLGENPRO_USER_CODE_REGION_START InitClassMembers
			// __LLBLGENPRO_USER_CODE_REGION_END
			OnInitClassMembersComplete();
		}


		#region Custom Property Hashtable Setup
		/// <summary> Initializes the hashtables for the entity type and entity field custom properties. </summary>
		private static void SetupCustomPropertyHashtables()
		{
			_customProperties = new Dictionary<string, string>();
			_fieldsCustomProperties = new Dictionary<string, Dictionary<string, string>>();
			Dictionary<string, string> fieldHashtable;
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("StoreID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RowVersion", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("License", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Edition", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("TypeCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Enabled", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SetupComplete", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("StoreName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Company", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Street1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Street2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Street3", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("City", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("StateProvCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PostalCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CountryCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Phone", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Fax", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Email", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Website", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AutoDownload", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AutoDownloadMinutes", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AutoDownloadOnlyAway", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AddressValidationSetting", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ComputerDownloadPolicy", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DefaultEmailAccountID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ManualOrderPrefix", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ManualOrderPostfix", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InitialDownloadDays", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InitialDownloadOrder", fieldHashtable);
		}
		#endregion

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this StoreEntity</param>
		/// <param name="fields">Fields of this entity</param>
		private void InitClassEmpty(IValidator validator, IEntityFields2 fields)
		{
			OnInitializing();
			this.Fields = fields ?? CreateFields();
			this.Validator = validator;
			InitClassMembers();

			// __LLBLGENPRO_USER_CODE_REGION_START InitClassEmpty
			// __LLBLGENPRO_USER_CODE_REGION_END

			OnInitialized();

		}

		#region Class Property Declarations
		/// <summary> The relations object holding all relations of this entity with other entity classes.</summary>
		public  static StoreRelations Relations
		{
			get	{ return new StoreRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'AmazonOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathAmazonOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<AmazonOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(AmazonOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("AmazonOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.AmazonOrderSearchEntity, 0, null, null, null, null, "AmazonOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ChannelAdvisorOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathChannelAdvisorOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<ChannelAdvisorOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ChannelAdvisorOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("ChannelAdvisorOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.ChannelAdvisorOrderSearchEntity, 0, null, null, null, null, "ChannelAdvisorOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ClickCartProOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathClickCartProOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<ClickCartProOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ClickCartProOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("ClickCartProOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.ClickCartProOrderSearchEntity, 0, null, null, null, null, "ClickCartProOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'CommerceInterfaceOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathCommerceInterfaceOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<CommerceInterfaceOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(CommerceInterfaceOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("CommerceInterfaceOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.CommerceInterfaceOrderSearchEntity, 0, null, null, null, null, "CommerceInterfaceOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'EbayOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathEbayOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<EbayOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EbayOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("EbayOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.EbayOrderSearchEntity, 0, null, null, null, null, "EbayOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'EtsyOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathEtsyOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<EtsyOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EtsyOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("EtsyOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.EtsyOrderSearchEntity, 0, null, null, null, null, "EtsyOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'GrouponOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathGrouponOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<GrouponOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(GrouponOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("GrouponOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.GrouponOrderSearchEntity, 0, null, null, null, null, "GrouponOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'LemonStandOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathLemonStandOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<LemonStandOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(LemonStandOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("LemonStandOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.LemonStandOrderSearchEntity, 0, null, null, null, null, "LemonStandOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'MagentoOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathMagentoOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<MagentoOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(MagentoOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("MagentoOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.MagentoOrderSearchEntity, 0, null, null, null, null, "MagentoOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'MarketplaceAdvisorOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathMarketplaceAdvisorOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<MarketplaceAdvisorOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(MarketplaceAdvisorOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("MarketplaceAdvisorOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.MarketplaceAdvisorOrderSearchEntity, 0, null, null, null, null, "MarketplaceAdvisorOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'NetworkSolutionsOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathNetworkSolutionsOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<NetworkSolutionsOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(NetworkSolutionsOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("NetworkSolutionsOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.NetworkSolutionsOrderSearchEntity, 0, null, null, null, null, "NetworkSolutionsOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'NeweggOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathNeweggOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<NeweggOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(NeweggOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("NeweggOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.NeweggOrderSearchEntity, 0, null, null, null, null, "NeweggOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'OrderMotionOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathOrderMotionOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<OrderMotionOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderMotionOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("OrderMotionOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.OrderMotionOrderSearchEntity, 0, null, null, null, null, "OrderMotionOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'OrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<OrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("OrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.OrderSearchEntity, 0, null, null, null, null, "OrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'PayPalOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathPayPalOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<PayPalOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(PayPalOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("PayPalOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.PayPalOrderSearchEntity, 0, null, null, null, null, "PayPalOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ProStoresOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathProStoresOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<ProStoresOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ProStoresOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("ProStoresOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.ProStoresOrderSearchEntity, 0, null, null, null, null, "ProStoresOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'SearsOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathSearsOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<SearsOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(SearsOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("SearsOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.SearsOrderSearchEntity, 0, null, null, null, null, "SearsOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ShopifyOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathShopifyOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<ShopifyOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ShopifyOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("ShopifyOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.ShopifyOrderSearchEntity, 0, null, null, null, null, "ShopifyOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ThreeDCartOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathThreeDCartOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<ThreeDCartOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ThreeDCartOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("ThreeDCartOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.ThreeDCartOrderSearchEntity, 0, null, null, null, null, "ThreeDCartOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'WalmartOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathWalmartOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<WalmartOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(WalmartOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("WalmartOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.WalmartOrderSearchEntity, 0, null, null, null, null, "WalmartOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'YahooOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathYahooOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<YahooOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(YahooOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("YahooOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.StoreEntity, (int)ShipWorks.Data.Model.EntityType.YahooOrderSearchEntity, 0, null, null, null, null, "YahooOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}


		/// <summary> The custom properties for the type of this entity instance.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		protected override Dictionary<string, string> CustomPropertiesOfType
		{
			get { return CustomProperties;}
		}

		/// <summary> The custom properties for the fields of this entity type. The returned Hashtable contains per fieldname a hashtable of name-value pairs. </summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, Dictionary<string, string>> FieldsCustomProperties
		{
			get { return _fieldsCustomProperties;}
		}

		/// <summary> The custom properties for the fields of the type of this entity instance. The returned Hashtable contains per fieldname a hashtable of name-value pairs. </summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		protected override Dictionary<string, Dictionary<string, string>> FieldsCustomPropertiesOfType
		{
			get { return FieldsCustomProperties;}
		}

		/// <summary> The StoreID property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."StoreID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 StoreID
		{
			get { return (System.Int64)GetValue((int)StoreFieldIndex.StoreID, true); }
			set	{ SetValue((int)StoreFieldIndex.StoreID, value); }
		}

		/// <summary> The RowVersion property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."RowVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] RowVersion
		{
			get { return (System.Byte[])GetValue((int)StoreFieldIndex.RowVersion, true); }

		}

		/// <summary> The License property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."License"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String License
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.License, true); }
			set	{ SetValue((int)StoreFieldIndex.License, value); }
		}

		/// <summary> The Edition property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."Edition"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Edition
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.Edition, true); }
			set	{ SetValue((int)StoreFieldIndex.Edition, value); }
		}

		/// <summary> The TypeCode property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."TypeCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 TypeCode
		{
			get { return (System.Int32)GetValue((int)StoreFieldIndex.TypeCode, true); }
			set	{ SetValue((int)StoreFieldIndex.TypeCode, value); }
		}

		/// <summary> The Enabled property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."Enabled"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Enabled
		{
			get { return (System.Boolean)GetValue((int)StoreFieldIndex.Enabled, true); }
			set	{ SetValue((int)StoreFieldIndex.Enabled, value); }
		}

		/// <summary> The SetupComplete property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."SetupComplete"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean SetupComplete
		{
			get { return (System.Boolean)GetValue((int)StoreFieldIndex.SetupComplete, true); }
			set	{ SetValue((int)StoreFieldIndex.SetupComplete, value); }
		}

		/// <summary> The StoreName property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."StoreName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 75<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String StoreName
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.StoreName, true); }
			set	{ SetValue((int)StoreFieldIndex.StoreName, value); }
		}

		/// <summary> The Company property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."Company"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Company
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.Company, true); }
			set	{ SetValue((int)StoreFieldIndex.Company, value); }
		}

		/// <summary> The Street1 property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."Street1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Street1
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.Street1, true); }
			set	{ SetValue((int)StoreFieldIndex.Street1, value); }
		}

		/// <summary> The Street2 property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."Street2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Street2
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.Street2, true); }
			set	{ SetValue((int)StoreFieldIndex.Street2, value); }
		}

		/// <summary> The Street3 property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."Street3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Street3
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.Street3, true); }
			set	{ SetValue((int)StoreFieldIndex.Street3, value); }
		}

		/// <summary> The City property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."City"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String City
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.City, true); }
			set	{ SetValue((int)StoreFieldIndex.City, value); }
		}

		/// <summary> The StateProvCode property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."StateProvCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String StateProvCode
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.StateProvCode, true); }
			set	{ SetValue((int)StoreFieldIndex.StateProvCode, value); }
		}

		/// <summary> The PostalCode property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."PostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String PostalCode
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.PostalCode, true); }
			set	{ SetValue((int)StoreFieldIndex.PostalCode, value); }
		}

		/// <summary> The CountryCode property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."CountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CountryCode
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.CountryCode, true); }
			set	{ SetValue((int)StoreFieldIndex.CountryCode, value); }
		}

		/// <summary> The Phone property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."Phone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Phone
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.Phone, true); }
			set	{ SetValue((int)StoreFieldIndex.Phone, value); }
		}

		/// <summary> The Fax property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."Fax"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Fax
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.Fax, true); }
			set	{ SetValue((int)StoreFieldIndex.Fax, value); }
		}

		/// <summary> The Email property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."Email"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Email
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.Email, true); }
			set	{ SetValue((int)StoreFieldIndex.Email, value); }
		}

		/// <summary> The Website property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."Website"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Website
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.Website, true); }
			set	{ SetValue((int)StoreFieldIndex.Website, value); }
		}

		/// <summary> The AutoDownload property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."AutoDownload"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean AutoDownload
		{
			get { return (System.Boolean)GetValue((int)StoreFieldIndex.AutoDownload, true); }
			set	{ SetValue((int)StoreFieldIndex.AutoDownload, value); }
		}

		/// <summary> The AutoDownloadMinutes property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."AutoDownloadMinutes"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 AutoDownloadMinutes
		{
			get { return (System.Int32)GetValue((int)StoreFieldIndex.AutoDownloadMinutes, true); }
			set	{ SetValue((int)StoreFieldIndex.AutoDownloadMinutes, value); }
		}

		/// <summary> The AutoDownloadOnlyAway property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."AutoDownloadOnlyAway"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean AutoDownloadOnlyAway
		{
			get { return (System.Boolean)GetValue((int)StoreFieldIndex.AutoDownloadOnlyAway, true); }
			set	{ SetValue((int)StoreFieldIndex.AutoDownloadOnlyAway, value); }
		}

		/// <summary> The AddressValidationSetting property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."AddressValidationSetting"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 AddressValidationSetting
		{
			get { return (System.Int32)GetValue((int)StoreFieldIndex.AddressValidationSetting, true); }
			set	{ SetValue((int)StoreFieldIndex.AddressValidationSetting, value); }
		}

		/// <summary> The ComputerDownloadPolicy property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."ComputerDownloadPolicy"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ComputerDownloadPolicy
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.ComputerDownloadPolicy, true); }
			set	{ SetValue((int)StoreFieldIndex.ComputerDownloadPolicy, value); }
		}

		/// <summary> The DefaultEmailAccountID property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."DefaultEmailAccountID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 DefaultEmailAccountID
		{
			get { return (System.Int64)GetValue((int)StoreFieldIndex.DefaultEmailAccountID, true); }
			set	{ SetValue((int)StoreFieldIndex.DefaultEmailAccountID, value); }
		}

		/// <summary> The ManualOrderPrefix property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."ManualOrderPrefix"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ManualOrderPrefix
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.ManualOrderPrefix, true); }
			set	{ SetValue((int)StoreFieldIndex.ManualOrderPrefix, value); }
		}

		/// <summary> The ManualOrderPostfix property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."ManualOrderPostfix"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ManualOrderPostfix
		{
			get { return (System.String)GetValue((int)StoreFieldIndex.ManualOrderPostfix, true); }
			set	{ SetValue((int)StoreFieldIndex.ManualOrderPostfix, value); }
		}

		/// <summary> The InitialDownloadDays property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."InitialDownloadDays"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> InitialDownloadDays
		{
			get { return (Nullable<System.Int32>)GetValue((int)StoreFieldIndex.InitialDownloadDays, false); }
			set	{ SetValue((int)StoreFieldIndex.InitialDownloadDays, value); }
		}

		/// <summary> The InitialDownloadOrder property of the Entity Store<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Store"."InitialDownloadOrder"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> InitialDownloadOrder
		{
			get { return (Nullable<System.Int64>)GetValue((int)StoreFieldIndex.InitialDownloadOrder, false); }
			set	{ SetValue((int)StoreFieldIndex.InitialDownloadOrder, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'AmazonOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(AmazonOrderSearchEntity))]
		public virtual EntityCollection<AmazonOrderSearchEntity> AmazonOrderSearch
		{
			get { return GetOrCreateEntityCollection<AmazonOrderSearchEntity, AmazonOrderSearchEntityFactory>("Store", true, false, ref _amazonOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ChannelAdvisorOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ChannelAdvisorOrderSearchEntity))]
		public virtual EntityCollection<ChannelAdvisorOrderSearchEntity> ChannelAdvisorOrderSearch
		{
			get { return GetOrCreateEntityCollection<ChannelAdvisorOrderSearchEntity, ChannelAdvisorOrderSearchEntityFactory>("Store", true, false, ref _channelAdvisorOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ClickCartProOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ClickCartProOrderSearchEntity))]
		public virtual EntityCollection<ClickCartProOrderSearchEntity> ClickCartProOrderSearch
		{
			get { return GetOrCreateEntityCollection<ClickCartProOrderSearchEntity, ClickCartProOrderSearchEntityFactory>("Store", true, false, ref _clickCartProOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'CommerceInterfaceOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(CommerceInterfaceOrderSearchEntity))]
		public virtual EntityCollection<CommerceInterfaceOrderSearchEntity> CommerceInterfaceOrderSearch
		{
			get { return GetOrCreateEntityCollection<CommerceInterfaceOrderSearchEntity, CommerceInterfaceOrderSearchEntityFactory>("Store", true, false, ref _commerceInterfaceOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'EbayOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(EbayOrderSearchEntity))]
		public virtual EntityCollection<EbayOrderSearchEntity> EbayOrderSearch
		{
			get { return GetOrCreateEntityCollection<EbayOrderSearchEntity, EbayOrderSearchEntityFactory>("Store", true, false, ref _ebayOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'EtsyOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(EtsyOrderSearchEntity))]
		public virtual EntityCollection<EtsyOrderSearchEntity> EtsyOrderSearch
		{
			get { return GetOrCreateEntityCollection<EtsyOrderSearchEntity, EtsyOrderSearchEntityFactory>("Store", true, false, ref _etsyOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'GrouponOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(GrouponOrderSearchEntity))]
		public virtual EntityCollection<GrouponOrderSearchEntity> GrouponOrderSearch
		{
			get { return GetOrCreateEntityCollection<GrouponOrderSearchEntity, GrouponOrderSearchEntityFactory>("Store", true, false, ref _grouponOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'LemonStandOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(LemonStandOrderSearchEntity))]
		public virtual EntityCollection<LemonStandOrderSearchEntity> LemonStandOrderSearch
		{
			get { return GetOrCreateEntityCollection<LemonStandOrderSearchEntity, LemonStandOrderSearchEntityFactory>("Store", true, false, ref _lemonStandOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'MagentoOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(MagentoOrderSearchEntity))]
		public virtual EntityCollection<MagentoOrderSearchEntity> MagentoOrderSearch
		{
			get { return GetOrCreateEntityCollection<MagentoOrderSearchEntity, MagentoOrderSearchEntityFactory>("Store", true, false, ref _magentoOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'MarketplaceAdvisorOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(MarketplaceAdvisorOrderSearchEntity))]
		public virtual EntityCollection<MarketplaceAdvisorOrderSearchEntity> MarketplaceAdvisorOrderSearch
		{
			get { return GetOrCreateEntityCollection<MarketplaceAdvisorOrderSearchEntity, MarketplaceAdvisorOrderSearchEntityFactory>("Store", true, false, ref _marketplaceAdvisorOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'NetworkSolutionsOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(NetworkSolutionsOrderSearchEntity))]
		public virtual EntityCollection<NetworkSolutionsOrderSearchEntity> NetworkSolutionsOrderSearch
		{
			get { return GetOrCreateEntityCollection<NetworkSolutionsOrderSearchEntity, NetworkSolutionsOrderSearchEntityFactory>("Store", true, false, ref _networkSolutionsOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'NeweggOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(NeweggOrderSearchEntity))]
		public virtual EntityCollection<NeweggOrderSearchEntity> NeweggOrderSearch
		{
			get { return GetOrCreateEntityCollection<NeweggOrderSearchEntity, NeweggOrderSearchEntityFactory>("Store", true, false, ref _neweggOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'OrderMotionOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(OrderMotionOrderSearchEntity))]
		public virtual EntityCollection<OrderMotionOrderSearchEntity> OrderMotionOrderSearch
		{
			get { return GetOrCreateEntityCollection<OrderMotionOrderSearchEntity, OrderMotionOrderSearchEntityFactory>("Store", true, false, ref _orderMotionOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'OrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(OrderSearchEntity))]
		public virtual EntityCollection<OrderSearchEntity> OrderSearch
		{
			get { return GetOrCreateEntityCollection<OrderSearchEntity, OrderSearchEntityFactory>("Store", true, false, ref _orderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'PayPalOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(PayPalOrderSearchEntity))]
		public virtual EntityCollection<PayPalOrderSearchEntity> PayPalOrderSearch
		{
			get { return GetOrCreateEntityCollection<PayPalOrderSearchEntity, PayPalOrderSearchEntityFactory>("Store", true, false, ref _payPalOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ProStoresOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ProStoresOrderSearchEntity))]
		public virtual EntityCollection<ProStoresOrderSearchEntity> ProStoresOrderSearch
		{
			get { return GetOrCreateEntityCollection<ProStoresOrderSearchEntity, ProStoresOrderSearchEntityFactory>("Store", true, false, ref _proStoresOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'SearsOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(SearsOrderSearchEntity))]
		public virtual EntityCollection<SearsOrderSearchEntity> SearsOrderSearch
		{
			get { return GetOrCreateEntityCollection<SearsOrderSearchEntity, SearsOrderSearchEntityFactory>("Store", true, false, ref _searsOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ShopifyOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ShopifyOrderSearchEntity))]
		public virtual EntityCollection<ShopifyOrderSearchEntity> ShopifyOrderSearch
		{
			get { return GetOrCreateEntityCollection<ShopifyOrderSearchEntity, ShopifyOrderSearchEntityFactory>("Store", true, false, ref _shopifyOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ThreeDCartOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ThreeDCartOrderSearchEntity))]
		public virtual EntityCollection<ThreeDCartOrderSearchEntity> ThreeDCartOrderSearch
		{
			get { return GetOrCreateEntityCollection<ThreeDCartOrderSearchEntity, ThreeDCartOrderSearchEntityFactory>("Store", true, false, ref _threeDCartOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'WalmartOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(WalmartOrderSearchEntity))]
		public virtual EntityCollection<WalmartOrderSearchEntity> WalmartOrderSearch
		{
			get { return GetOrCreateEntityCollection<WalmartOrderSearchEntity, WalmartOrderSearchEntityFactory>("Store", true, false, ref _walmartOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'YahooOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(YahooOrderSearchEntity))]
		public virtual EntityCollection<YahooOrderSearchEntity> YahooOrderSearch
		{
			get { return GetOrCreateEntityCollection<YahooOrderSearchEntity, YahooOrderSearchEntityFactory>("Store", true, false, ref _yahooOrderSearch);	}
		}
	
		/// <summary> Gets the type of the hierarchy this entity is in. </summary>
		protected override InheritanceHierarchyType LLBLGenProIsInHierarchyOfType
		{
			get { return InheritanceHierarchyType.TargetPerEntity;}
		}
		
		/// <summary> Gets or sets a value indicating whether this entity is a subtype</summary>
		protected override bool LLBLGenProIsSubType
		{
			get { return false;}
		}
		
		/// <summary>Returns the ShipWorks.Data.Model.EntityType enum value for this entity.</summary>
		[Browsable(false), XmlIgnore]
		protected override int LLBLGenProEntityTypeValue 
		{ 
			get { return (int)ShipWorks.Data.Model.EntityType.StoreEntity; }
		}

		#endregion


		#region Custom Entity code
		
		// __LLBLGENPRO_USER_CODE_REGION_START CustomEntityCode
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Included code

		#endregion
	}
}
