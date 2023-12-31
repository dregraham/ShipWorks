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
	/// <summary>Implements the relations factory for the entity: GrouponOrder. </summary>
	public partial class GrouponOrderRelations : OrderRelations
	{
		/// <summary>CTor</summary>
		public GrouponOrderRelations()
		{
		}

		/// <summary>Gets all relations of the GrouponOrderEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = base.GetAllRelations();
			toReturn.Add(this.GrouponOrderSearchEntityUsingOrderID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between GrouponOrderEntity and GrouponOrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GrouponOrder.OrderID - GrouponOrderSearch.OrderID
		/// </summary>
		public virtual IEntityRelation GrouponOrderSearchEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "GrouponOrderSearch" , true);
				relation.AddEntityFieldPair(GrouponOrderFields.OrderID, GrouponOrderSearchFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GrouponOrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GrouponOrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GrouponOrderEntity and NoteEntity over the 1:n relation they have, using the relation between the fields:
		/// GrouponOrder.OrderID - Note.EntityID
		/// </summary>
		public override IEntityRelation NoteEntityUsingEntityID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Notes" , true);
				relation.AddEntityFieldPair(GrouponOrderFields.OrderID, NoteFields.EntityID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GrouponOrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("NoteEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GrouponOrderEntity and OrderChargeEntity over the 1:n relation they have, using the relation between the fields:
		/// GrouponOrder.OrderID - OrderCharge.OrderID
		/// </summary>
		public override IEntityRelation OrderChargeEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderCharges" , true);
				relation.AddEntityFieldPair(GrouponOrderFields.OrderID, OrderChargeFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GrouponOrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderChargeEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GrouponOrderEntity and OrderItemEntity over the 1:n relation they have, using the relation between the fields:
		/// GrouponOrder.OrderID - OrderItem.OrderID
		/// </summary>
		public override IEntityRelation OrderItemEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderItems" , true);
				relation.AddEntityFieldPair(GrouponOrderFields.OrderID, OrderItemFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GrouponOrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderItemEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GrouponOrderEntity and OrderPaymentDetailEntity over the 1:n relation they have, using the relation between the fields:
		/// GrouponOrder.OrderID - OrderPaymentDetail.OrderID
		/// </summary>
		public override IEntityRelation OrderPaymentDetailEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderPaymentDetails" , true);
				relation.AddEntityFieldPair(GrouponOrderFields.OrderID, OrderPaymentDetailFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GrouponOrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderPaymentDetailEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GrouponOrderEntity and OrderSearchEntity over the 1:n relation they have, using the relation between the fields:
		/// GrouponOrder.OrderID - OrderSearch.OrderID
		/// </summary>
		public override IEntityRelation OrderSearchEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderSearch" , true);
				relation.AddEntityFieldPair(GrouponOrderFields.OrderID, OrderSearchFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GrouponOrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderSearchEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GrouponOrderEntity and ShipmentEntity over the 1:n relation they have, using the relation between the fields:
		/// GrouponOrder.OrderID - Shipment.OrderID
		/// </summary>
		public override IEntityRelation ShipmentEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Shipments" , true);
				relation.AddEntityFieldPair(GrouponOrderFields.OrderID, ShipmentFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GrouponOrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GrouponOrderEntity and ValidatedAddressEntity over the 1:n relation they have, using the relation between the fields:
		/// GrouponOrder.OrderID - ValidatedAddress.ConsumerID
		/// </summary>
		public override IEntityRelation ValidatedAddressEntityUsingConsumerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ValidatedAddress" , true);
				relation.AddEntityFieldPair(GrouponOrderFields.OrderID, ValidatedAddressFields.ConsumerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GrouponOrderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ValidatedAddressEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between GrouponOrderEntity and CustomerEntity over the m:1 relation they have, using the relation between the fields:
		/// GrouponOrder.CustomerID - Customer.CustomerID
		/// </summary>
		public override IEntityRelation CustomerEntityUsingCustomerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Customer", false);
				relation.AddEntityFieldPair(CustomerFields.CustomerID, GrouponOrderFields.CustomerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("CustomerEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GrouponOrderEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between GrouponOrderEntity and StoreEntity over the m:1 relation they have, using the relation between the fields:
		/// GrouponOrder.StoreID - Store.StoreID
		/// </summary>
		public override IEntityRelation StoreEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Store", false);
				relation.AddEntityFieldPair(StoreFields.StoreID, GrouponOrderFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StoreEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GrouponOrderEntity", true);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GrouponOrderEntity and OrderEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>
		internal IEntityRelation RelationToSuperTypeOrderEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, false);
				relation.AddEntityFieldPair(OrderFields.OrderID, GrouponOrderFields.OrderID);
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
	internal static class StaticGrouponOrderRelations
	{
		internal static readonly IEntityRelation GrouponOrderSearchEntityUsingOrderIDStatic = new GrouponOrderRelations().GrouponOrderSearchEntityUsingOrderID;
		internal static readonly IEntityRelation NoteEntityUsingEntityIDStatic = new GrouponOrderRelations().NoteEntityUsingEntityID;
		internal static readonly IEntityRelation OrderChargeEntityUsingOrderIDStatic = new GrouponOrderRelations().OrderChargeEntityUsingOrderID;
		internal static readonly IEntityRelation OrderItemEntityUsingOrderIDStatic = new GrouponOrderRelations().OrderItemEntityUsingOrderID;
		internal static readonly IEntityRelation OrderPaymentDetailEntityUsingOrderIDStatic = new GrouponOrderRelations().OrderPaymentDetailEntityUsingOrderID;
		internal static readonly IEntityRelation OrderSearchEntityUsingOrderIDStatic = new GrouponOrderRelations().OrderSearchEntityUsingOrderID;
		internal static readonly IEntityRelation ShipmentEntityUsingOrderIDStatic = new GrouponOrderRelations().ShipmentEntityUsingOrderID;
		internal static readonly IEntityRelation ValidatedAddressEntityUsingConsumerIDStatic = new GrouponOrderRelations().ValidatedAddressEntityUsingConsumerID;
		internal static readonly IEntityRelation CustomerEntityUsingCustomerIDStatic = new GrouponOrderRelations().CustomerEntityUsingCustomerID;
		internal static readonly IEntityRelation StoreEntityUsingStoreIDStatic = new GrouponOrderRelations().StoreEntityUsingStoreID;

		/// <summary>CTor</summary>
		static StaticGrouponOrderRelations()
		{
		}
	}
}
