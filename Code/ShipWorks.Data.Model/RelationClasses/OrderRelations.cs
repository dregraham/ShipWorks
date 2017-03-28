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
	/// <summary>Implements the relations factory for the entity: Order. </summary>
	public partial class OrderRelations : IRelationFactory
	{
		/// <summary>CTor</summary>
		public OrderRelations()
		{
		}

		/// <summary>Gets all relations of the OrderEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.NoteEntityUsingEntityID);
			toReturn.Add(this.OrderChargeEntityUsingOrderID);
			toReturn.Add(this.OrderItemEntityUsingOrderID);
			toReturn.Add(this.OrderPaymentDetailEntityUsingOrderID);
			toReturn.Add(this.ShipmentEntityUsingOrderID);
			toReturn.Add(this.ValidatedAddressEntityUsingConsumerID);
			toReturn.Add(this.CustomerEntityUsingCustomerID);
			toReturn.Add(this.StoreEntityUsingStoreID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between OrderEntity and NoteEntity over the 1:n relation they have, using the relation between the fields:
		/// Order.OrderID - Note.EntityID
		/// </summary>
		public virtual IEntityRelation NoteEntityUsingEntityID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Notes" , true);
				relation.AddEntityFieldPair(OrderFields.OrderID, NoteFields.EntityID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("NoteEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderEntity and OrderChargeEntity over the 1:n relation they have, using the relation between the fields:
		/// Order.OrderID - OrderCharge.OrderID
		/// </summary>
		public virtual IEntityRelation OrderChargeEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderCharges" , true);
				relation.AddEntityFieldPair(OrderFields.OrderID, OrderChargeFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderChargeEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderEntity and OrderItemEntity over the 1:n relation they have, using the relation between the fields:
		/// Order.OrderID - OrderItem.OrderID
		/// </summary>
		public virtual IEntityRelation OrderItemEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderItems" , true);
				relation.AddEntityFieldPair(OrderFields.OrderID, OrderItemFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderItemEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderEntity and OrderPaymentDetailEntity over the 1:n relation they have, using the relation between the fields:
		/// Order.OrderID - OrderPaymentDetail.OrderID
		/// </summary>
		public virtual IEntityRelation OrderPaymentDetailEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderPaymentDetails" , true);
				relation.AddEntityFieldPair(OrderFields.OrderID, OrderPaymentDetailFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderPaymentDetailEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderEntity and ShipmentEntity over the 1:n relation they have, using the relation between the fields:
		/// Order.OrderID - Shipment.OrderID
		/// </summary>
		public virtual IEntityRelation ShipmentEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Shipments" , true);
				relation.AddEntityFieldPair(OrderFields.OrderID, ShipmentFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between OrderEntity and ValidatedAddressEntity over the 1:n relation they have, using the relation between the fields:
		/// Order.OrderID - ValidatedAddress.ConsumerID
		/// </summary>
		public virtual IEntityRelation ValidatedAddressEntityUsingConsumerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ValidatedAddress" , true);
				relation.AddEntityFieldPair(OrderFields.OrderID, ValidatedAddressFields.ConsumerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ValidatedAddressEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between OrderEntity and CustomerEntity over the m:1 relation they have, using the relation between the fields:
		/// Order.CustomerID - Customer.CustomerID
		/// </summary>
		public virtual IEntityRelation CustomerEntityUsingCustomerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Customer", false);
				relation.AddEntityFieldPair(CustomerFields.CustomerID, OrderFields.CustomerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("CustomerEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and StoreEntity over the m:1 relation they have, using the relation between the fields:
		/// Order.StoreID - Store.StoreID
		/// </summary>
		public virtual IEntityRelation StoreEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Store", false);
				relation.AddEntityFieldPair(StoreFields.StoreID, OrderFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StoreEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", true);
				return relation;
			}
		}



		/// <summary>Returns a new IEntityRelation object, between OrderEntity and AmazonOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeAmazonOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, AmazonOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and ChannelAdvisorOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeChannelAdvisorOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, ChannelAdvisorOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and ClickCartProOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeClickCartProOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, ClickCartProOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and CommerceInterfaceOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeCommerceInterfaceOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, CommerceInterfaceOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and EbayOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeEbayOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, EbayOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and EtsyOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeEtsyOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, EtsyOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and GrouponOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeGrouponOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, GrouponOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and LemonStandOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeLemonStandOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, LemonStandOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and MagentoOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeMagentoOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, MagentoOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and MarketplaceAdvisorOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeMarketplaceAdvisorOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, MarketplaceAdvisorOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and NetworkSolutionsOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeNetworkSolutionsOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, NetworkSolutionsOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and NeweggOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeNeweggOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, NeweggOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and OrderMotionOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeOrderMotionOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, OrderMotionOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and PayPalOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypePayPalOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, PayPalOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and ProStoresOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeProStoresOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, ProStoresOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and SearsOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeSearsOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, SearsOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and ShopifyOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeShopifyOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, ShopifyOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and ThreeDCartOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeThreeDCartOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, ThreeDCartOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and WalmartOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeWalmartOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, WalmartOrderFields.OrderID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between OrderEntity and YahooOrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeYahooOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderFields.OrderID, YahooOrderFields.OrderID);
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
				case "AmazonOrderEntity":
					return this.RelationToSubTypeAmazonOrderEntity;
				case "ChannelAdvisorOrderEntity":
					return this.RelationToSubTypeChannelAdvisorOrderEntity;
				case "ClickCartProOrderEntity":
					return this.RelationToSubTypeClickCartProOrderEntity;
				case "CommerceInterfaceOrderEntity":
					return this.RelationToSubTypeCommerceInterfaceOrderEntity;
				case "EbayOrderEntity":
					return this.RelationToSubTypeEbayOrderEntity;
				case "EtsyOrderEntity":
					return this.RelationToSubTypeEtsyOrderEntity;
				case "GrouponOrderEntity":
					return this.RelationToSubTypeGrouponOrderEntity;
				case "LemonStandOrderEntity":
					return this.RelationToSubTypeLemonStandOrderEntity;
				case "MagentoOrderEntity":
					return this.RelationToSubTypeMagentoOrderEntity;
				case "MarketplaceAdvisorOrderEntity":
					return this.RelationToSubTypeMarketplaceAdvisorOrderEntity;
				case "NetworkSolutionsOrderEntity":
					return this.RelationToSubTypeNetworkSolutionsOrderEntity;
				case "NeweggOrderEntity":
					return this.RelationToSubTypeNeweggOrderEntity;
				case "OrderMotionOrderEntity":
					return this.RelationToSubTypeOrderMotionOrderEntity;
				case "PayPalOrderEntity":
					return this.RelationToSubTypePayPalOrderEntity;
				case "ProStoresOrderEntity":
					return this.RelationToSubTypeProStoresOrderEntity;
				case "SearsOrderEntity":
					return this.RelationToSubTypeSearsOrderEntity;
				case "ShopifyOrderEntity":
					return this.RelationToSubTypeShopifyOrderEntity;
				case "ThreeDCartOrderEntity":
					return this.RelationToSubTypeThreeDCartOrderEntity;
				case "WalmartOrderEntity":
					return this.RelationToSubTypeWalmartOrderEntity;
				case "YahooOrderEntity":
					return this.RelationToSubTypeYahooOrderEntity;
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
	internal static class StaticOrderRelations
	{
		internal static readonly IEntityRelation NoteEntityUsingEntityIDStatic = new OrderRelations().NoteEntityUsingEntityID;
		internal static readonly IEntityRelation OrderChargeEntityUsingOrderIDStatic = new OrderRelations().OrderChargeEntityUsingOrderID;
		internal static readonly IEntityRelation OrderItemEntityUsingOrderIDStatic = new OrderRelations().OrderItemEntityUsingOrderID;
		internal static readonly IEntityRelation OrderPaymentDetailEntityUsingOrderIDStatic = new OrderRelations().OrderPaymentDetailEntityUsingOrderID;
		internal static readonly IEntityRelation ShipmentEntityUsingOrderIDStatic = new OrderRelations().ShipmentEntityUsingOrderID;
		internal static readonly IEntityRelation ValidatedAddressEntityUsingConsumerIDStatic = new OrderRelations().ValidatedAddressEntityUsingConsumerID;
		internal static readonly IEntityRelation CustomerEntityUsingCustomerIDStatic = new OrderRelations().CustomerEntityUsingCustomerID;
		internal static readonly IEntityRelation StoreEntityUsingStoreIDStatic = new OrderRelations().StoreEntityUsingStoreID;

		/// <summary>CTor</summary>
		static StaticOrderRelations()
		{
		}
	}
}
