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
	/// <summary>Implements the relations factory for the entity: VolusionStore. </summary>
	public partial class VolusionStoreRelations : StoreRelations
	{
		/// <summary>CTor</summary>
		public VolusionStoreRelations()
		{
		}

		/// <summary>Gets all relations of the VolusionStoreEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = base.GetAllRelations();
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and AmazonOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - AmazonOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation AmazonOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "AmazonOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, AmazonOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AmazonOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and ChannelAdvisorOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - ChannelAdvisorOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ChannelAdvisorOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ChannelAdvisorOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, ChannelAdvisorOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ChannelAdvisorOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and ClickCartProOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - ClickCartProOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ClickCartProOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ClickCartProOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, ClickCartProOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ClickCartProOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and CommerceInterfaceOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - CommerceInterfaceOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation CommerceInterfaceOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "CommerceInterfaceOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, CommerceInterfaceOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("CommerceInterfaceOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and DownloadEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - Download.StoreID
		/// </summary>
		public override IEntityRelation DownloadEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, DownloadFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DownloadEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and EbayOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - EbayOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation EbayOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "EbayOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, EbayOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EbayOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and EtsyOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - EtsyOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation EtsyOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "EtsyOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, EtsyOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EtsyOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and GrouponOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - GrouponOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation GrouponOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "GrouponOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, GrouponOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GrouponOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and LemonStandOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - LemonStandOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation LemonStandOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "LemonStandOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, LemonStandOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("LemonStandOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and MagentoOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - MagentoOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation MagentoOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "MagentoOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, MagentoOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("MagentoOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and MarketplaceAdvisorOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - MarketplaceAdvisorOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation MarketplaceAdvisorOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "MarketplaceAdvisorOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, MarketplaceAdvisorOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("MarketplaceAdvisorOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and NetworkSolutionsOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - NetworkSolutionsOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation NetworkSolutionsOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "NetworkSolutionsOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, NetworkSolutionsOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("NetworkSolutionsOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and NeweggOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - NeweggOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation NeweggOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "NeweggOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, NeweggOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("NeweggOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and OrderEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - Order.StoreID
		/// </summary>
		public override IEntityRelation OrderEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, OrderFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and OrderMotionOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - OrderMotionOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation OrderMotionOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderMotionOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, OrderMotionOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderMotionOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and OrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - OrderSearch.StoreID
		/// </summary>
		public override IEntityRelation OrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, OrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and PayPalOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - PayPalOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation PayPalOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "PayPalOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, PayPalOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PayPalOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and ProStoresOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - ProStoresOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ProStoresOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ProStoresOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, ProStoresOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProStoresOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and SearsOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - SearsOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation SearsOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "SearsOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, SearsOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("SearsOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and ShopifyOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - ShopifyOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ShopifyOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ShopifyOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, ShopifyOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShopifyOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and StatusPresetEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - StatusPreset.StoreID
		/// </summary>
		public override IEntityRelation StatusPresetEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, StatusPresetFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StatusPresetEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and ThreeDCartOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - ThreeDCartOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation ThreeDCartOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ThreeDCartOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, ThreeDCartOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ThreeDCartOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and WalmartOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - WalmartOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation WalmartOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "WalmartOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, WalmartOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("WalmartOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and YahooOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// VolusionStore.StoreID - YahooOrderSearch.StoreID
		/// </summary>
		public override IEntityRelation YahooOrderSearchEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "YahooOrderSearch" , true);
				relation.AddEntityFieldPair(VolusionStoreFields.StoreID, YahooOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VolusionStoreEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("YahooOrderSearchEntity", false);
				return relation;
			}
		}



		/// <summary>Returns a new IEntityRelation object, between VolusionStoreEntity and StoreEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>
		internal IEntityRelation RelationToSuperTypeStoreEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, false);
				relation.AddEntityFieldPair(StoreFields.StoreID, VolusionStoreFields.StoreID);
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
	internal static class StaticVolusionStoreRelations
	{
		internal static readonly IEntityRelation AmazonOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().AmazonOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ChannelAdvisorOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().ChannelAdvisorOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ClickCartProOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().ClickCartProOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation CommerceInterfaceOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().CommerceInterfaceOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation DownloadEntityUsingStoreIDStatic = new VolusionStoreRelations().DownloadEntityUsingStoreID;
		internal static readonly IEntityRelation EbayOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().EbayOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation EtsyOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().EtsyOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation GrouponOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().GrouponOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation LemonStandOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().LemonStandOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation MagentoOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().MagentoOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation MarketplaceAdvisorOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().MarketplaceAdvisorOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation NetworkSolutionsOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().NetworkSolutionsOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation NeweggOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().NeweggOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation OrderEntityUsingStoreIDStatic = new VolusionStoreRelations().OrderEntityUsingStoreID;
		internal static readonly IEntityRelation OrderMotionOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().OrderMotionOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation OrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().OrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation PayPalOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().PayPalOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ProStoresOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().ProStoresOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation SearsOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().SearsOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation ShopifyOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().ShopifyOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation StatusPresetEntityUsingStoreIDStatic = new VolusionStoreRelations().StatusPresetEntityUsingStoreID;
		internal static readonly IEntityRelation ThreeDCartOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().ThreeDCartOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation WalmartOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().WalmartOrderSearchEntityUsingStoreID;
		internal static readonly IEntityRelation YahooOrderSearchEntityUsingStoreIDStatic = new VolusionStoreRelations().YahooOrderSearchEntityUsingStoreID;

		/// <summary>CTor</summary>
		static StaticVolusionStoreRelations()
		{
		}
	}
}
