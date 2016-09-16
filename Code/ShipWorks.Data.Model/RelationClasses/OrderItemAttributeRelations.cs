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
	/// <summary>Implements the relations factory for the entity: OrderItemAttribute. </summary>
	public partial class OrderItemAttributeRelations : IRelationFactory
	{
		/// <summary>CTor</summary>
		public OrderItemAttributeRelations()
		{
		}

		/// <summary>Gets all relations of the OrderItemAttributeEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.OrderItemEntityUsingOrderItemID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between OrderItemAttributeEntity and OrderItemEntity over the m:1 relation they have, using the relation between the fields:
		/// OrderItemAttribute.OrderItemID - OrderItem.OrderItemID
		/// </summary>
		public virtual IEntityRelation OrderItemEntityUsingOrderItemID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "OrderItem", false);
				relation.AddEntityFieldPair(OrderItemFields.OrderItemID, OrderItemAttributeFields.OrderItemID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderItemEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderItemAttributeEntity", true);
				return relation;
			}
		}



		/// <summary>Returns a new IEntityRelation object, between OrderItemAttributeEntity and MivaOrderItemAttributeEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeMivaOrderItemAttributeEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(OrderItemAttributeFields.OrderItemAttributeID, MivaOrderItemAttributeFields.OrderItemAttributeID);
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
				case "MivaOrderItemAttributeEntity":
					return this.RelationToSubTypeMivaOrderItemAttributeEntity;
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
	internal static class StaticOrderItemAttributeRelations
	{
		internal static readonly IEntityRelation OrderItemEntityUsingOrderItemIDStatic = new OrderItemAttributeRelations().OrderItemEntityUsingOrderItemID;

		/// <summary>CTor</summary>
		static StaticOrderItemAttributeRelations()
		{
		}
	}
}
