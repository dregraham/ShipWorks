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
	/// <summary>Implements the relations factory for the entity: LemonStandStore. </summary>
	public partial class LemonStandStoreRelations : StoreRelations
	{
		/// <summary>CTor</summary>
		public LemonStandStoreRelations()
		{
		}

		/// <summary>Gets all relations of the LemonStandStoreEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = base.GetAllRelations();
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and AmazonOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - AmazonOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation AmazonOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "AmazonOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, AmazonOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AmazonOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and ChannelAdvisorOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - ChannelAdvisorOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ChannelAdvisorOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ChannelAdvisorOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, ChannelAdvisorOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ChannelAdvisorOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and ClickCartProOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - ClickCartProOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ClickCartProOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ClickCartProOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, ClickCartProOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ClickCartProOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and CommerceInterfaceOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - CommerceInterfaceOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation CommerceInterfaceOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "CommerceInterfaceOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, CommerceInterfaceOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("CommerceInterfaceOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and DownloadEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - Download.StoreID
		/// </summary>
		public override IEntityRelation DownloadEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, DownloadFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DownloadEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and EbayOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - EbayOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation EbayOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "EbayOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, EbayOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EbayOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and EtsyOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - EtsyOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation EtsyOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "EtsyOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, EtsyOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EtsyOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and GrouponOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - GrouponOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation GrouponOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "GrouponOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, GrouponOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GrouponOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and LemonStandOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - LemonStandOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation LemonStandOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "LemonStandOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, LemonStandOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and MagentoOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - MagentoOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation MagentoOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "MagentoOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, MagentoOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("MagentoOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and MarketplaceAdvisorOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - MarketplaceAdvisorOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation MarketplaceAdvisorOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "MarketplaceAdvisorOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, MarketplaceAdvisorOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("MarketplaceAdvisorOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and NetworkSolutionsOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - NetworkSolutionsOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation NetworkSolutionsOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "NetworkSolutionsOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, NetworkSolutionsOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("NetworkSolutionsOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and NeweggOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - NeweggOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation NeweggOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "NeweggOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, NeweggOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("NeweggOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and OrderEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - Order.StoreID
		/// </summary>
		public override IEntityRelation OrderEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, OrderFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and OrderMotionOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - OrderMotionOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation OrderMotionOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderMotionOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, OrderMotionOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and PayPalOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - PayPalOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation PayPalOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "PayPalOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, PayPalOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PayPalOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and ProStoresOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - ProStoresOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ProStoresOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ProStoresOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, ProStoresOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and SearsOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - SearsOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation SearsOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "SearsOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, SearsOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SearsOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and ShopifyOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - ShopifyOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ShopifyOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ShopifyOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, ShopifyOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShopifyOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and StatusPresetEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - StatusPreset.StoreID
		/// </summary>
		public override IEntityRelation StatusPresetEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, StatusPresetFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StatusPresetEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and ThreeDCartOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - ThreeDCartOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ThreeDCartOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ThreeDCartOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, ThreeDCartOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ThreeDCartOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and WalmartOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - WalmartOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation WalmartOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "WalmartOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, WalmartOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("WalmartOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and YahooOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// LemonStandStore.StoreID - YahooOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation YahooOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "YahooOrderSearch" , true);
				relation.AddEntityFieldPair(LemonStandStoreFields.StoreID, YahooOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("YahooOrderSearchEntity", false);
				return relation;
			}
		}



		/// <summary>Returns a new IEntityRelation object, between LemonStandStoreEntity and StoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>
		internal IEntityRelation RelationToSuperTypeStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, false);
				relation.AddEntityFieldPair(StoreFields.StoreID, LemonStandStoreFields.StoreID);
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
	internal static class StaticLemonStandStoreRelations
	{
		internal static readonly IEntityRelation AmazonOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().AmazonOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ChannelAdvisorOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().ChannelAdvisorOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ClickCartProOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().ClickCartProOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation CommerceInterfaceOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().CommerceInterfaceOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation DownloadEntityUsingStoreIDStatic = new LemonStandStoreRelations().DownloadEntityUsingStoreID;
		internal static readonly IEntityRelation EbayOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().EbayOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation EtsyOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().EtsyOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation GrouponOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().GrouponOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation LemonStandOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().LemonStandOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation MagentoOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().MagentoOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation MarketplaceAdvisorOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().MarketplaceAdvisorOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation NetworkSolutionsOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().NetworkSolutionsOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation NeweggOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().NeweggOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation OrderEntityUsingStoreIDStatic = new LemonStandStoreRelations().OrderEntityUsingStoreID;
		internal static readonly IEntityRelation OrderMotionOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().OrderMotionOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation PayPalOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().PayPalOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ProStoresOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().ProStoresOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation SearsOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().SearsOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ShopifyOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().ShopifyOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation StatusPresetEntityUsingStoreIDStatic = new LemonStandStoreRelations().StatusPresetEntityUsingStoreID;
		internal static readonly IEntityRelation ThreeDCartOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().ThreeDCartOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation WalmartOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().WalmartOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation YahooOrderSearchEntityUsingStoreIDStatic = new LemonStandStoreRelations().YahooOrderSearchEntityUsingStoreID;

		/// <summary>CTor</summary>
		static StaticLemonStandStoreRelations()
		{
		}
	}
}
