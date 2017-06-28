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
	/// <summary>Implements the relations factory for the entity: ProStoresStore. </summary>
	public partial class ProStoresStoreRelations : StoreRelations
	{
		/// <summary>CTor</summary>
		public ProStoresStoreRelations()
		{
		}

		/// <summary>Gets all relations of the ProStoresStoreEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = base.GetAllRelations();
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and AmazonOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - AmazonOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation AmazonOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "AmazonOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, AmazonOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AmazonOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and ChannelAdvisorOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - ChannelAdvisorOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ChannelAdvisorOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ChannelAdvisorOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, ChannelAdvisorOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ChannelAdvisorOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and ClickCartProOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - ClickCartProOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ClickCartProOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ClickCartProOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, ClickCartProOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ClickCartProOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and CommerceInterfaceOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - CommerceInterfaceOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation CommerceInterfaceOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "CommerceInterfaceOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, CommerceInterfaceOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("CommerceInterfaceOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and DownloadEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - Download.StoreID
		/// </summary>
		public override IEntityRelation DownloadEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, DownloadFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DownloadEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and EbayOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - EbayOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation EbayOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "EbayOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, EbayOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EbayOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and EtsyOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - EtsyOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation EtsyOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "EtsyOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, EtsyOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EtsyOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and GrouponOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - GrouponOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation GrouponOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "GrouponOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, GrouponOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GrouponOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and LemonStandOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - LemonStandOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation LemonStandOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "LemonStandOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, LemonStandOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and MagentoOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - MagentoOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation MagentoOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "MagentoOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, MagentoOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("MagentoOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and MarketplaceAdvisorOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - MarketplaceAdvisorOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation MarketplaceAdvisorOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "MarketplaceAdvisorOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, MarketplaceAdvisorOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("MarketplaceAdvisorOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and NetworkSolutionsOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - NetworkSolutionsOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation NetworkSolutionsOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "NetworkSolutionsOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, NetworkSolutionsOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("NetworkSolutionsOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and NeweggOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - NeweggOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation NeweggOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "NeweggOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, NeweggOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("NeweggOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and OrderEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - Order.StoreID
		/// </summary>
		public override IEntityRelation OrderEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, OrderFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and OrderMotionOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - OrderMotionOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation OrderMotionOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderMotionOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, OrderMotionOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and PayPalOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - PayPalOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation PayPalOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "PayPalOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, PayPalOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PayPalOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and ProStoresOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - ProStoresOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ProStoresOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ProStoresOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, ProStoresOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and SearsOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - SearsOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation SearsOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "SearsOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, SearsOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SearsOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and ShopifyOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - ShopifyOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ShopifyOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ShopifyOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, ShopifyOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShopifyOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and StatusPresetEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - StatusPreset.StoreID
		/// </summary>
		public override IEntityRelation StatusPresetEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, StatusPresetFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StatusPresetEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and ThreeDCartOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - ThreeDCartOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ThreeDCartOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ThreeDCartOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, ThreeDCartOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ThreeDCartOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and WalmartOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - WalmartOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation WalmartOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "WalmartOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, WalmartOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("WalmartOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and YahooOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ProStoresStore.StoreID - YahooOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation YahooOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "YahooOrderSearch" , true);
				relation.AddEntityFieldPair(ProStoresStoreFields.StoreID, YahooOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("YahooOrderSearchEntity", false);
				return relation;
			}
		}



		/// <summary>Returns a new IEntityRelation object, between ProStoresStoreEntity and StoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>
		internal IEntityRelation RelationToSuperTypeStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, false);
				relation.AddEntityFieldPair(StoreFields.StoreID, ProStoresStoreFields.StoreID);
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
	internal static class StaticProStoresStoreRelations
	{
		internal static readonly IEntityRelation AmazonOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().AmazonOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ChannelAdvisorOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().ChannelAdvisorOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ClickCartProOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().ClickCartProOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation CommerceInterfaceOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().CommerceInterfaceOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation DownloadEntityUsingStoreIDStatic = new ProStoresStoreRelations().DownloadEntityUsingStoreID;
		internal static readonly IEntityRelation EbayOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().EbayOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation EtsyOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().EtsyOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation GrouponOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().GrouponOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation LemonStandOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().LemonStandOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation MagentoOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().MagentoOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation MarketplaceAdvisorOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().MarketplaceAdvisorOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation NetworkSolutionsOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().NetworkSolutionsOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation NeweggOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().NeweggOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation OrderEntityUsingStoreIDStatic = new ProStoresStoreRelations().OrderEntityUsingStoreID;
		internal static readonly IEntityRelation OrderMotionOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().OrderMotionOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation PayPalOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().PayPalOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ProStoresOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().ProStoresOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation SearsOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().SearsOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ShopifyOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().ShopifyOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation StatusPresetEntityUsingStoreIDStatic = new ProStoresStoreRelations().StatusPresetEntityUsingStoreID;
		internal static readonly IEntityRelation ThreeDCartOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().ThreeDCartOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation WalmartOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().WalmartOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation YahooOrderSearchEntityUsingStoreIDStatic = new ProStoresStoreRelations().YahooOrderSearchEntityUsingStoreID;

		/// <summary>CTor</summary>
		static StaticProStoresStoreRelations()
		{
		}
	}
}
