﻿///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates.NET20
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
	/// <summary>Implements the static Relations variant for the entity: AmazonOrderItem. </summary>
	public partial class AmazonOrderItemRelations : OrderItemRelations
	{
		/// <summary>CTor</summary>
		public AmazonOrderItemRelations()
		{
		}

		/// <summary>Gets all relations of the AmazonOrderItemEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = base.GetAllRelations();



			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between AmazonOrderItemEntity and OrderItemAttributeEntity over the 1:n relation they have, using the relation between the fields:
		/// AmazonOrderItem.OrderItemID - OrderItemAttribute.OrderItemID
		/// </summary>
		public override IEntityRelation OrderItemAttributeEntityUsingOrderItemID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "OrderItemAttributes" , true);
				relation.AddEntityFieldPair(AmazonOrderItemFields.OrderItemID, OrderItemAttributeFields.OrderItemID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AmazonOrderItemEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderItemAttributeEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between AmazonOrderItemEntity and OrderEntity over the m:1 relation they have, using the relation between the fields:
		/// AmazonOrderItem.OrderID - Order.OrderID
		/// </summary>
		public override IEntityRelation OrderEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Order", false);
				relation.AddEntityFieldPair(OrderFields.OrderID, AmazonOrderItemFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AmazonOrderItemEntity", true);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between AmazonOrderItemEntity and OrderItemEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy, and is using the relation between the fields:
		/// AmazonOrderItem.OrderItemID - OrderItem.OrderItemID
		/// </summary>
		internal IEntityRelation RelationToSuperTypeOrderItemEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, false);

	
	
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, AmazonOrderItemFields.OrderItemID);
	
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
			return this.RelationToSuperTypeOrderItemEntity;
		}

		#endregion

		#region Included Code

		#endregion
	}
}
