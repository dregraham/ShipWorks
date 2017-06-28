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
using System.Collections;
using System.Collections.Generic;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.RelationClasses
{
	/// <summary>Implements the relations factory for the entity: OrderMotionStore. </summary>
	public partial class OrderMotionStoreRelations : StoreRelations
	{
		/// <summary>CTor</summary>
		public OrderMotionStoreRelations()
		{
		}

		/// <summary>Gets all relations of the OrderMotionStoreEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = base.GetAllRelations();
			toReturn.Add(this.EmailAccountEntityUsingOrderMotionEmailAccountID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and AmazonOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - AmazonOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation AmazonOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "AmazonOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, AmazonOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AmazonOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and ChannelAdvisorOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - ChannelAdvisorOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ChannelAdvisorOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ChannelAdvisorOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, ChannelAdvisorOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ChannelAdvisorOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and ClickCartProOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - ClickCartProOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ClickCartProOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ClickCartProOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, ClickCartProOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ClickCartProOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and CommerceInterfaceOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - CommerceInterfaceOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation CommerceInterfaceOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "CommerceInterfaceOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, CommerceInterfaceOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("CommerceInterfaceOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and DownloadEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - Download.StoreID
		/// </summary>
		public override IEntityRelation DownloadEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, DownloadFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DownloadEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and EbayOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - EbayOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation EbayOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "EbayOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, EbayOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EbayOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and EtsyOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - EtsyOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation EtsyOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "EtsyOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, EtsyOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EtsyOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and GrouponOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - GrouponOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation GrouponOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "GrouponOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, GrouponOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GrouponOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and LemonStandOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - LemonStandOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation LemonStandOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "LemonStandOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, LemonStandOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and MagentoOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - MagentoOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation MagentoOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "MagentoOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, MagentoOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("MagentoOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and MarketplaceAdvisorOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - MarketplaceAdvisorOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation MarketplaceAdvisorOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "MarketplaceAdvisorOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, MarketplaceAdvisorOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("MarketplaceAdvisorOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and NetworkSolutionsOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - NetworkSolutionsOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation NetworkSolutionsOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "NetworkSolutionsOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, NetworkSolutionsOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("NetworkSolutionsOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and NeweggOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - NeweggOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation NeweggOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "NeweggOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, NeweggOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("NeweggOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and OrderEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - Order.StoreID
		/// </summary>
		public override IEntityRelation OrderEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, OrderFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and OrderMotionOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - OrderMotionOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation OrderMotionOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderMotionOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, OrderMotionOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and PayPalOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - PayPalOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation PayPalOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "PayPalOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, PayPalOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PayPalOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and ProStoresOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - ProStoresOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ProStoresOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ProStoresOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, ProStoresOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and SearsOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - SearsOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation SearsOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "SearsOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, SearsOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SearsOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and ShopifyOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - ShopifyOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ShopifyOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ShopifyOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, ShopifyOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShopifyOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and StatusPresetEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - StatusPreset.StoreID
		/// </summary>
		public override IEntityRelation StatusPresetEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, StatusPresetFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StatusPresetEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and ThreeDCartOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - ThreeDCartOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ThreeDCartOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ThreeDCartOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, ThreeDCartOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ThreeDCartOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and WalmartOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - WalmartOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation WalmartOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "WalmartOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, WalmartOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("WalmartOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and YahooOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderMotionStore.StoreID - YahooOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation YahooOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "YahooOrderSearch" , true);
				relation.AddEntityFieldPair(OrderMotionStoreFields.StoreID, YahooOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("YahooOrderSearchEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and EmailAccountEntity over the m:1 relation they have, using the relation between the fields:
		/// OrderMotionStore.OrderMotionEmailAccountID - EmailAccount.EmailAccountID
		/// </summary>
		public virtual IEntityRelation EmailAccountEntityUsingOrderMotionEmailAccountID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "OrderMotionEmailAccount", false);
				relation.AddEntityFieldPair(EmailAccountFields.EmailAccountID, OrderMotionStoreFields.OrderMotionEmailAccountID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EmailAccountEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionStoreEntity", true);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderMotionStoreEntity and StoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>
		internal IEntityRelation RelationToSuperTypeStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, false);
				relation.AddEntityFieldPair(StoreFields.StoreID, OrderMotionStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}

		
		/// <summary>Returns the relation object the entity, to which this relation factory belongs, has with the subtype with the specified name</summary>
		/// <param name="subTypeEntityName">name of direct subtype which is a subtype of the current entity through the relation to return.</param>
		/// <returns>relation which makes the current entity a supertype of the subtype entity with the name specified, or null if not applicable/found</returns>
		public override IEntityRelation GetSubTypeRelation(string subTypeEntityName)
		{
			return null;
		}
		
		/// <summary>Returns the relation object the entity, to which this relation factory belongs, has with its supertype, if applicable.</summary>
		/// <returns>relation which makes the current entity a subtype of its supertype entity or null if not applicable/found</returns>
		public override IEntityRelation GetSuperTypeRelation()
		{
			return this.RelationToSuperTypeStoreEntity;
		}

		#endregion

		#region Included Code

		#endregion
	}
	
	/// <summary>Static class which is used for providing relationship instances which are re-used internally for syncing</summary>
	internal static class StaticOrderMotionStoreRelations
	{
		internal static readonly IEntityRelation AmazonOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().AmazonOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ChannelAdvisorOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().ChannelAdvisorOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ClickCartProOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().ClickCartProOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation CommerceInterfaceOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().CommerceInterfaceOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation DownloadEntityUsingStoreIDStatic = new OrderMotionStoreRelations().DownloadEntityUsingStoreID;
		internal static readonly IEntityRelation EbayOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().EbayOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation EtsyOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().EtsyOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation GrouponOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().GrouponOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation LemonStandOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().LemonStandOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation MagentoOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().MagentoOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation MarketplaceAdvisorOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().MarketplaceAdvisorOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation NetworkSolutionsOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().NetworkSolutionsOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation NeweggOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().NeweggOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation OrderEntityUsingStoreIDStatic = new OrderMotionStoreRelations().OrderEntityUsingStoreID;
		internal static readonly IEntityRelation OrderMotionOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().OrderMotionOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation PayPalOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().PayPalOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ProStoresOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().ProStoresOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation SearsOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().SearsOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ShopifyOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().ShopifyOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation StatusPresetEntityUsingStoreIDStatic = new OrderMotionStoreRelations().StatusPresetEntityUsingStoreID;
		internal static readonly IEntityRelation ThreeDCartOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().ThreeDCartOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation WalmartOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().WalmartOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation YahooOrderSearchEntityUsingStoreIDStatic = new OrderMotionStoreRelations().YahooOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation EmailAccountEntityUsingOrderMotionEmailAccountIDStatic = new OrderMotionStoreRelations().EmailAccountEntityUsingOrderMotionEmailAccountID;

		/// <summary>CTor</summary>
		static StaticOrderMotionStoreRelations()
		{
		}
	}
}
