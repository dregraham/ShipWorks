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
	/// <summary>Implements the relations factory for the entity: ShopifyOrder. </summary>
	public partial class ShopifyOrderRelations : OrderRelations
	{
		/// <summary>CTor</summary>
		public ShopifyOrderRelations()
		{
		}

		/// <summary>Gets all relations of the ShopifyOrderEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = base.GetAllRelations();
			toReturn.Add(this.ShopifyOrderSearchEntityUsingOrderID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between ShopifyOrderEntity and NoteEntity over the 1:n relation they have, using the relation between the fields:
		/// ShopifyOrder.OrderID - Note.EntityID
		/// </summary>
		public override IEntityRelation NoteEntityUsingEntityID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Notes" , true);
				relation.AddEntityFieldPair(ShopifyOrderFields.OrderID, NoteFields.EntityID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShopifyOrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("NoteEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShopifyOrderEntity and OrderChargeEntity over the 1:n relation they have, using the relation between the fields:
		/// ShopifyOrder.OrderID - OrderCharge.OrderID
		/// </summary>
		public override IEntityRelation OrderChargeEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderCharges" , true);
				relation.AddEntityFieldPair(ShopifyOrderFields.OrderID, OrderChargeFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShopifyOrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderChargeEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShopifyOrderEntity and OrderItemEntity over the 1:n relation they have, using the relation between the fields:
		/// ShopifyOrder.OrderID - OrderItem.OrderID
		/// </summary>
		public override IEntityRelation OrderItemEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderItems" , true);
				relation.AddEntityFieldPair(ShopifyOrderFields.OrderID, OrderItemFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShopifyOrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderItemEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShopifyOrderEntity and OrderPaymentDetailEntity over the 1:n relation they have, using the relation between the fields:
		/// ShopifyOrder.OrderID - OrderPaymentDetail.OrderID
		/// </summary>
		public override IEntityRelation OrderPaymentDetailEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderPaymentDetails" , true);
				relation.AddEntityFieldPair(ShopifyOrderFields.OrderID, OrderPaymentDetailFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShopifyOrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderPaymentDetailEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShopifyOrderEntity and OrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ShopifyOrder.OrderID - OrderSearch.OrderID
		/// </summary>
		public override IEntityRelation OrderSearchEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderSearch" , true);
				relation.AddEntityFieldPair(ShopifyOrderFields.OrderID, OrderSearchFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShopifyOrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShopifyOrderEntity and ShipmentEntity over the 1:n relation they have, using the relation between the fields:
		/// ShopifyOrder.OrderID - Shipment.OrderID
		/// </summary>
		public override IEntityRelation ShipmentEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Shipments" , true);
				relation.AddEntityFieldPair(ShopifyOrderFields.OrderID, ShipmentFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShopifyOrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShopifyOrderEntity and ShopifyOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// ShopifyOrder.OrderID - ShopifyOrderSearch.OrderID
		/// </summary>
		public virtual IEntityRelation ShopifyOrderSearchEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ShopifyOrderSearch" , true);
				relation.AddEntityFieldPair(ShopifyOrderFields.OrderID, ShopifyOrderSearchFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShopifyOrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShopifyOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShopifyOrderEntity and ValidatedAddressEntity over the 1:n relation they have, using the relation between the fields:
		/// ShopifyOrder.OrderID - ValidatedAddress.ConsumerID
		/// </summary>
		public override IEntityRelation ValidatedAddressEntityUsingConsumerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ValidatedAddress" , true);
				relation.AddEntityFieldPair(ShopifyOrderFields.OrderID, ValidatedAddressFields.ConsumerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShopifyOrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ValidatedAddressEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between ShopifyOrderEntity and CustomerEntity over the m:1 relation they have, using the relation between the fields:
		/// ShopifyOrder.CustomerID - Customer.CustomerID
		/// </summary>
		public override IEntityRelation CustomerEntityUsingCustomerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Customer", false);
				relation.AddEntityFieldPair(CustomerFields.CustomerID, ShopifyOrderFields.CustomerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("CustomerEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShopifyOrderEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between ShopifyOrderEntity and StoreEntity over the m:1 relation they have, using the relation between the fields:
		/// ShopifyOrder.StoreID - Store.StoreID
		/// </summary>
		public override IEntityRelation StoreEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Store", false);
				relation.AddEntityFieldPair(StoreFields.StoreID, ShopifyOrderFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StoreEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShopifyOrderEntity", true);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShopifyOrderEntity and OrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>
		internal IEntityRelation RelationToSuperTypeOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, false);
				relation.AddEntityFieldPair(OrderFields.OrderID, ShopifyOrderFields.OrderID);
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
			return this.RelationToSuperTypeOrderEntity;
		}

		#endregion

		#region Included Code

		#endregion
	}
	
	/// <summary>Static class which is used for providing relationship instances which are re-used internally for syncing</summary>
	internal static class StaticShopifyOrderRelations
	{
		internal static readonly IEntityRelation NoteEntityUsingEntityIDStatic = new ShopifyOrderRelations().NoteEntityUsingEntityID;
		internal static readonly IEntityRelation OrderChargeEntityUsingOrderIDStatic = new ShopifyOrderRelations().OrderChargeEntityUsingOrderID;
		internal static readonly IEntityRelation OrderItemEntityUsingOrderIDStatic = new ShopifyOrderRelations().OrderItemEntityUsingOrderID;
		internal static readonly IEntityRelation OrderPaymentDetailEntityUsingOrderIDStatic = new ShopifyOrderRelations().OrderPaymentDetailEntityUsingOrderID;
		internal static readonly IEntityRelation OrderSearchEntityUsingOrderIDStatic = new ShopifyOrderRelations().OrderSearchEntityUsingOrderID;
		internal static readonly IEntityRelation ShipmentEntityUsingOrderIDStatic = new ShopifyOrderRelations().ShipmentEntityUsingOrderID;
		internal static readonly IEntityRelation ShopifyOrderSearchEntityUsingOrderIDStatic = new ShopifyOrderRelations().ShopifyOrderSearchEntityUsingOrderID;
		internal static readonly IEntityRelation ValidatedAddressEntityUsingConsumerIDStatic = new ShopifyOrderRelations().ValidatedAddressEntityUsingConsumerID;
		internal static readonly IEntityRelation CustomerEntityUsingCustomerIDStatic = new ShopifyOrderRelations().CustomerEntityUsingCustomerID;
		internal static readonly IEntityRelation StoreEntityUsingStoreIDStatic = new ShopifyOrderRelations().StoreEntityUsingStoreID;

		/// <summary>CTor</summary>
		static StaticShopifyOrderRelations()
		{
		}
	}
}
