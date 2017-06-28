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
	/// <summary>Implements the relations factory for the entity: GenericModuleStore. </summary>
	public partial class GenericModuleStoreRelations : StoreRelations
	{
		/// <summary>CTor</summary>
		public GenericModuleStoreRelations()
		{
		}

		/// <summary>Gets all relations of the GenericModuleStoreEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = base.GetAllRelations();
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and AmazonOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - AmazonOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation AmazonOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "AmazonOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, AmazonOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AmazonOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and ChannelAdvisorOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - ChannelAdvisorOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ChannelAdvisorOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ChannelAdvisorOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, ChannelAdvisorOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ChannelAdvisorOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and ClickCartProOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - ClickCartProOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ClickCartProOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ClickCartProOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, ClickCartProOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ClickCartProOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and CommerceInterfaceOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - CommerceInterfaceOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation CommerceInterfaceOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "CommerceInterfaceOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, CommerceInterfaceOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("CommerceInterfaceOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and DownloadEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - Download.StoreID
		/// </summary>
		public override IEntityRelation DownloadEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, DownloadFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DownloadEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and EbayOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - EbayOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation EbayOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "EbayOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, EbayOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EbayOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and EtsyOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - EtsyOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation EtsyOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "EtsyOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, EtsyOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EtsyOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and GrouponOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - GrouponOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation GrouponOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "GrouponOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, GrouponOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GrouponOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and LemonStandOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - LemonStandOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation LemonStandOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "LemonStandOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, LemonStandOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and MagentoOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - MagentoOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation MagentoOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "MagentoOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, MagentoOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("MagentoOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and MarketplaceAdvisorOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - MarketplaceAdvisorOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation MarketplaceAdvisorOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "MarketplaceAdvisorOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, MarketplaceAdvisorOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("MarketplaceAdvisorOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and NetworkSolutionsOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - NetworkSolutionsOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation NetworkSolutionsOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "NetworkSolutionsOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, NetworkSolutionsOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("NetworkSolutionsOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and NeweggOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - NeweggOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation NeweggOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "NeweggOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, NeweggOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("NeweggOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and OrderEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - Order.StoreID
		/// </summary>
		public override IEntityRelation OrderEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, OrderFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and OrderMotionOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - OrderMotionOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation OrderMotionOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderMotionOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, OrderMotionOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and OrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - OrderSearch.StoreID
		/// </summary>
		public override IEntityRelation OrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, OrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and PayPalOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - PayPalOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation PayPalOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "PayPalOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, PayPalOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PayPalOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and ProStoresOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - ProStoresOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ProStoresOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ProStoresOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, ProStoresOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and SearsOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - SearsOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation SearsOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "SearsOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, SearsOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SearsOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and ShopifyOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - ShopifyOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ShopifyOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ShopifyOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, ShopifyOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShopifyOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and StatusPresetEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - StatusPreset.StoreID
		/// </summary>
		public override IEntityRelation StatusPresetEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, StatusPresetFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StatusPresetEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and ThreeDCartOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - ThreeDCartOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ThreeDCartOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ThreeDCartOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, ThreeDCartOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ThreeDCartOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and WalmartOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - WalmartOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation WalmartOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "WalmartOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, WalmartOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("WalmartOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and YahooOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GenericModuleStore.StoreID - YahooOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation YahooOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "YahooOrderSearch" , true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, YahooOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GenericModuleStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("YahooOrderSearchEntity", false);
				return relation;
			}
		}



		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and StoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>
		internal IEntityRelation RelationToSuperTypeStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, false);
				relation.AddEntityFieldPair(StoreFields.StoreID, GenericModuleStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and MagentoStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeMagentoStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, MagentoStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between GenericModuleStoreEntity and MivaStoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeMivaStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(GenericModuleStoreFields.StoreID, MivaStoreFields.StoreID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		
		/// <summary>Returns the relation object the entity, to which this relation factory belongs, has with the subtype with the specified name</summary>
		/// <param name="subTypeEntityName">name of direct subtype which is a subtype of the current entity through the relation to return.</param>
		/// <returns>relation which makes the current entity a supertype of the subtype entity with the name specified, or null if not applicable/found</returns>
		public override IEntityRelation GetSubTypeRelation(string subTypeEntityName)
		{
			switch(subTypeEntityName)
			{
				case "MagentoStoreEntity":
					return this.RelationToSubTypeMagentoStoreEntity;
				case "MivaStoreEntity":
					return this.RelationToSubTypeMivaStoreEntity;
				default:
					return null;
			}
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
	internal static class StaticGenericModuleStoreRelations
	{
		internal static readonly IEntityRelation AmazonOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().AmazonOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ChannelAdvisorOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().ChannelAdvisorOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ClickCartProOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().ClickCartProOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation CommerceInterfaceOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().CommerceInterfaceOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation DownloadEntityUsingStoreIDStatic = new GenericModuleStoreRelations().DownloadEntityUsingStoreID;
		internal static readonly IEntityRelation EbayOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().EbayOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation EtsyOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().EtsyOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation GrouponOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().GrouponOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation LemonStandOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().LemonStandOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation MagentoOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().MagentoOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation MarketplaceAdvisorOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().MarketplaceAdvisorOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation NetworkSolutionsOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().NetworkSolutionsOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation NeweggOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().NeweggOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation OrderEntityUsingStoreIDStatic = new GenericModuleStoreRelations().OrderEntityUsingStoreID;
		internal static readonly IEntityRelation OrderMotionOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().OrderMotionOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation OrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().OrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation PayPalOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().PayPalOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ProStoresOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().ProStoresOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation SearsOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().SearsOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ShopifyOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().ShopifyOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation StatusPresetEntityUsingStoreIDStatic = new GenericModuleStoreRelations().StatusPresetEntityUsingStoreID;
		internal static readonly IEntityRelation ThreeDCartOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().ThreeDCartOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation WalmartOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().WalmartOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation YahooOrderSearchEntityUsingStoreIDStatic = new GenericModuleStoreRelations().YahooOrderSearchEntityUsingStoreID;

		/// <summary>CTor</summary>
		static StaticGenericModuleStoreRelations()
		{
		}
	}
}
