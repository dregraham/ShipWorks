﻿///////////////////////////////////////////////////////////////
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
	/// <summary>Implements the relations factory for the entity: Store. </summary>
	public partial class StoreRelations : IRelationFactory
	{
		/// <summary>CTor</summary>
		public StoreRelations()
		{
		}

		/// <summary>Gets all relations of the StoreEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.DownloadEntityUsingStoreID);
			toReturn.Add(this.OrderEntityUsingStoreID);
			toReturn.Add(this.OrderSearchEntityUsingStoreID);
			toReturn.Add(this.StatusPresetEntityUsingStoreID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between StoreEntity and DownloadEntity over the 1:n relation they have, using the relation between the fields:
		/// Store.StoreID - Download.StoreID
		/// </summary>
		public virtual IEntityRelation DownloadEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(StoreFields.StoreID, DownloadFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DownloadEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between StoreEntity and OrderEntity over the 1:n relation they have, using the relation between the fields:
		/// Store.StoreID - Order.StoreID
		/// </summary>
		public virtual IEntityRelation OrderEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(StoreFields.StoreID, OrderFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between StoreEntity and OrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// Store.StoreID - OrderSearch.StoreID
		/// </summary>
		public virtual IEntityRelation OrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderSearch" , true);
				relation.AddEntityFieldPair(StoreFields.StoreID, OrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between StoreEntity and StatusPresetEntity over the 1:n relation they have, using the relation between the fields:
		/// Store.StoreID - StatusPreset.StoreID
		/// </summary>
		public virtual IEntityRelation StatusPresetEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(StoreFields.StoreID, StatusPresetFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StatusPresetEntity", false);
				return relation;
			}
		}





		/// <summary>Returns a new IEntityRelation object, between StoreEntity and AmazonStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeAmazonStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, AmazonStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and AmeriCommerceStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeAmeriCommerceStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, AmeriCommerceStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and BigCommerceStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeBigCommerceStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, BigCommerceStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and BuyDotComStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeBuyDotComStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, BuyDotComStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and ChannelAdvisorStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeChannelAdvisorStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, ChannelAdvisorStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and EbayStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeEbayStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, EbayStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and EtsyStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeEtsyStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, EtsyStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and GenericFileStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeGenericFileStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, GenericFileStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and GenericModuleStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeGenericModuleStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, GenericModuleStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and GrouponStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeGrouponStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, GrouponStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and InfopiaStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeInfopiaStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, InfopiaStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and JetStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeJetStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, JetStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and LemonStandStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeLemonStandStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, LemonStandStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and MarketplaceAdvisorStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeMarketplaceAdvisorStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, MarketplaceAdvisorStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and NetworkSolutionsStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeNetworkSolutionsStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, NetworkSolutionsStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and NeweggStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeNeweggStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, NeweggStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and OdbcStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeOdbcStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, OdbcStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and OrderMotionStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeOrderMotionStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, OrderMotionStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and OverstockStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeOverstockStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, OverstockStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and PayPalStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypePayPalStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, PayPalStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and ProStoresStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeProStoresStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, ProStoresStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and SearsStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeSearsStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, SearsStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and ShopifyStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeShopifyStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, ShopifyStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and ShopSiteStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeShopSiteStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, ShopSiteStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and SparkPayStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeSparkPayStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, SparkPayStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and ThreeDCartStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeThreeDCartStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, ThreeDCartStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and VolusionStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeVolusionStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, VolusionStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and WalmartStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeWalmartStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, WalmartStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and YahooStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeYahooStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, YahooStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between StoreEntity and RakutenStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeRakutenStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(StoreFields.StoreID, RakutenStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		
		/// <summary>Returns the relation object the entity, to which this relation factory belongs, has with the subtype with the specified name</summary>
		/// <param name="subTypeEntityName">name of direct subtype which is a subtype of the current entity through the relation to return.</param>
		/// <returns>relation which makes the current entity a supertype of the subtype entity with the name specified, or null if not applicable/found</returns>
		public virtual IEntityRelation GetSubTypeRelation(string subTypeEntityName)
		{
			switch(subTypeEntityName)
			{
				case "AmazonStoreEntity":
					return this.RelationToSubTypeAmazonStoreEntity;
				case "AmeriCommerceStoreEntity":
					return this.RelationToSubTypeAmeriCommerceStoreEntity;
				case "BigCommerceStoreEntity":
					return this.RelationToSubTypeBigCommerceStoreEntity;
				case "BuyDotComStoreEntity":
					return this.RelationToSubTypeBuyDotComStoreEntity;
				case "ChannelAdvisorStoreEntity":
					return this.RelationToSubTypeChannelAdvisorStoreEntity;
				case "EbayStoreEntity":
					return this.RelationToSubTypeEbayStoreEntity;
				case "EtsyStoreEntity":
					return this.RelationToSubTypeEtsyStoreEntity;
				case "GenericFileStoreEntity":
					return this.RelationToSubTypeGenericFileStoreEntity;
				case "GenericModuleStoreEntity":
					return this.RelationToSubTypeGenericModuleStoreEntity;
				case "GrouponStoreEntity":
					return this.RelationToSubTypeGrouponStoreEntity;
				case "InfopiaStoreEntity":
					return this.RelationToSubTypeInfopiaStoreEntity;
				case "JetStoreEntity":
					return this.RelationToSubTypeJetStoreEntity;
				case "LemonStandStoreEntity":
					return this.RelationToSubTypeLemonStandStoreEntity;
				case "MarketplaceAdvisorStoreEntity":
					return this.RelationToSubTypeMarketplaceAdvisorStoreEntity;
				case "NetworkSolutionsStoreEntity":
					return this.RelationToSubTypeNetworkSolutionsStoreEntity;
				case "NeweggStoreEntity":
					return this.RelationToSubTypeNeweggStoreEntity;
				case "OdbcStoreEntity":
					return this.RelationToSubTypeOdbcStoreEntity;
				case "OrderMotionStoreEntity":
					return this.RelationToSubTypeOrderMotionStoreEntity;
				case "OverstockStoreEntity":
					return this.RelationToSubTypeOverstockStoreEntity;
				case "PayPalStoreEntity":
					return this.RelationToSubTypePayPalStoreEntity;
				case "ProStoresStoreEntity":
					return this.RelationToSubTypeProStoresStoreEntity;
				case "SearsStoreEntity":
					return this.RelationToSubTypeSearsStoreEntity;
				case "ShopifyStoreEntity":
					return this.RelationToSubTypeShopifyStoreEntity;
				case "ShopSiteStoreEntity":
					return this.RelationToSubTypeShopSiteStoreEntity;
				case "SparkPayStoreEntity":
					return this.RelationToSubTypeSparkPayStoreEntity;
				case "ThreeDCartStoreEntity":
					return this.RelationToSubTypeThreeDCartStoreEntity;
				case "VolusionStoreEntity":
					return this.RelationToSubTypeVolusionStoreEntity;
				case "WalmartStoreEntity":
					return this.RelationToSubTypeWalmartStoreEntity;
				case "YahooStoreEntity":
					return this.RelationToSubTypeYahooStoreEntity;
				case "RakutenStoreEntity":
					return this.RelationToSubTypeRakutenStoreEntity;
				default:
					return null;
			}
		}
		
		/// <summary>Returns the relation object the entity, to which this relation factory belongs, has with its supertype, if applicable.</summary>
		/// <returns>relation which makes the current entity a subtype of its supertype entity or null if not applicable/found</returns>
		public virtual IEntityRelation GetSuperTypeRelation()
		{
			return null;
		}

		#endregion

		#region Included Code

		#endregion
	}
	
	/// <summary>Static class which is used for providing relationship instances which are re-used internally for syncing</summary>
	internal static class StaticStoreRelations
	{
		internal static readonly IEntityRelation DownloadEntityUsingStoreIDStatic = new StoreRelations().DownloadEntityUsingStoreID;
		internal static readonly IEntityRelation OrderEntityUsingStoreIDStatic = new StoreRelations().OrderEntityUsingStoreID;
		internal static readonly IEntityRelation OrderSearchEntityUsingStoreIDStatic = new StoreRelations().OrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation StatusPresetEntityUsingStoreIDStatic = new StoreRelations().StatusPresetEntityUsingStoreID;

		/// <summary>CTor</summary>
		static StaticStoreRelations()
		{
		}
	}
}
