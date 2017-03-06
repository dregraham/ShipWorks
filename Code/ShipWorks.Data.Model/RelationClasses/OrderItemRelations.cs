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
	/// <summary>Implements the relations factory for the entity: OrderItem. </summary>
	public partial class OrderItemRelations : IRelationFactory
	{
		/// <summary>CTor</summary>
		public OrderItemRelations()
		{
		}

		/// <summary>Gets all relations of the OrderItemEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.OrderItemAttributeEntityUsingOrderItemID);
			toReturn.Add(this.OrderEntityUsingOrderID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between OrderItemEntity and OrderItemAttributeEntity over the 1:n relation they have, using the relation between the fields:
		/// OrderItem.OrderItemID - OrderItemAttribute.OrderItemID
		/// </summary>
		public virtual IEntityRelation OrderItemAttributeEntityUsingOrderItemID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderItemAttributes" , true);
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, OrderItemAttributeFields.OrderItemID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderItemEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderItemAttributeEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between OrderItemEntity and OrderEntity over the m:1 relation they have, using the relation between the fields:
		/// OrderItem.OrderID - Order.OrderID
		/// </summary>
		public virtual IEntityRelation OrderEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Order", false);
				relation.AddEntityFieldPair(OrderFields.OrderID, OrderItemFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderItemEntity", true);
				return relation;
			}
		}



		/// <summary>Returns a new IEntityRelation object, between OrderItemEntity and AmazonOrderItemEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeAmazonOrderItemEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, AmazonOrderItemFields.OrderItemID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderItemEntity and BigCommerceOrderItemEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeBigCommerceOrderItemEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, BigCommerceOrderItemFields.OrderItemID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderItemEntity and BuyDotComOrderItemEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeBuyDotComOrderItemEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, BuyDotComOrderItemFields.OrderItemID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderItemEntity and ChannelAdvisorOrderItemEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeChannelAdvisorOrderItemEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, ChannelAdvisorOrderItemFields.OrderItemID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderItemEntity and EbayOrderItemEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeEbayOrderItemEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, EbayOrderItemFields.OrderItemID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderItemEntity and GrouponOrderItemEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeGrouponOrderItemEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, GrouponOrderItemFields.OrderItemID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderItemEntity and InfopiaOrderItemEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeInfopiaOrderItemEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, InfopiaOrderItemFields.OrderItemID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderItemEntity and LemonStandOrderItemEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeLemonStandOrderItemEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, LemonStandOrderItemFields.OrderItemID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderItemEntity and NeweggOrderItemEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeNeweggOrderItemEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, NeweggOrderItemFields.OrderItemID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderItemEntity and SearsOrderItemEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeSearsOrderItemEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, SearsOrderItemFields.OrderItemID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderItemEntity and ShopifyOrderItemEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeShopifyOrderItemEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, ShopifyOrderItemFields.OrderItemID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderItemEntity and ThreeDCartOrderItemEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeThreeDCartOrderItemEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, ThreeDCartOrderItemFields.OrderItemID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderItemEntity and YahooOrderItemEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeYahooOrderItemEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, YahooOrderItemFields.OrderItemID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderItemEntity and WalmartOrderItemEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeWalmartOrderItemEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, WalmartOrderItemFields.OrderItemID);
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
				case "AmazonOrderItemEntity":
					return this.RelationToSubTypeAmazonOrderItemEntity;
				case "BigCommerceOrderItemEntity":
					return this.RelationToSubTypeBigCommerceOrderItemEntity;
				case "BuyDotComOrderItemEntity":
					return this.RelationToSubTypeBuyDotComOrderItemEntity;
				case "ChannelAdvisorOrderItemEntity":
					return this.RelationToSubTypeChannelAdvisorOrderItemEntity;
				case "EbayOrderItemEntity":
					return this.RelationToSubTypeEbayOrderItemEntity;
				case "GrouponOrderItemEntity":
					return this.RelationToSubTypeGrouponOrderItemEntity;
				case "InfopiaOrderItemEntity":
					return this.RelationToSubTypeInfopiaOrderItemEntity;
				case "LemonStandOrderItemEntity":
					return this.RelationToSubTypeLemonStandOrderItemEntity;
				case "NeweggOrderItemEntity":
					return this.RelationToSubTypeNeweggOrderItemEntity;
				case "SearsOrderItemEntity":
					return this.RelationToSubTypeSearsOrderItemEntity;
				case "ShopifyOrderItemEntity":
					return this.RelationToSubTypeShopifyOrderItemEntity;
				case "ThreeDCartOrderItemEntity":
					return this.RelationToSubTypeThreeDCartOrderItemEntity;
				case "YahooOrderItemEntity":
					return this.RelationToSubTypeYahooOrderItemEntity;
				case "WalmartOrderItemEntity":
					return this.RelationToSubTypeWalmartOrderItemEntity;
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
	internal static class StaticOrderItemRelations
	{
		internal static readonly IEntityRelation OrderItemAttributeEntityUsingOrderItemIDStatic = new OrderItemRelations().OrderItemAttributeEntityUsingOrderItemID;
		internal static readonly IEntityRelation OrderEntityUsingOrderIDStatic = new OrderItemRelations().OrderEntityUsingOrderID;

		/// <summary>CTor</summary>
		static StaticOrderItemRelations()
		{
		}
	}
}
