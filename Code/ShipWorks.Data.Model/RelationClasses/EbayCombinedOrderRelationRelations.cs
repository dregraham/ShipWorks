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
	/// <summary>Implements the relations factory for the entity: EbayCombinedOrderRelation. </summary>
	public partial class EbayCombinedOrderRelationRelations
	{
		/// <summary>CTor</summary>
		public EbayCombinedOrderRelationRelations()
		{
		}

		/// <summary>Gets all relations of the EbayCombinedOrderRelationEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.EbayOrderEntityUsingOrderID);
			toReturn.Add(this.EbayStoreEntityUsingStoreID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between EbayCombinedOrderRelationEntity and EbayOrderEntity over the m:1 relation they have, using the relation between the fields:
		/// EbayCombinedOrderRelation.OrderID - EbayOrder.OrderID
		/// </summary>
		public virtual IEntityRelation EbayOrderEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "EbayOrder", false);
				relation.AddEntityFieldPair(EbayOrderFields.OrderID, EbayCombinedOrderRelationFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EbayOrderEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EbayCombinedOrderRelationEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between EbayCombinedOrderRelationEntity and EbayStoreEntity over the m:1 relation they have, using the relation between the fields:
		/// EbayCombinedOrderRelation.StoreID - EbayStore.StoreID
		/// </summary>
		public virtual IEntityRelation EbayStoreEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "EbayStore", false);
				relation.AddEntityFieldPair(EbayStoreFields.StoreID, EbayCombinedOrderRelationFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EbayStoreEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EbayCombinedOrderRelationEntity", true);
				return relation;
			}
		}
		/// <summary>stub, not used in this entity, only for TargetPerEntity entities.</summary>
		public virtual IEntityRelation GetSubTypeRelation(string subTypeEntityName) { return null; }
		/// <summary>stub, not used in this entity, only for TargetPerEntity entities.</summary>
		public virtual IEntityRelation GetSuperTypeRelation() { return null;}
		#endregion

		#region Included Code

		#endregion
	}
	
	/// <summary>Static class which is used for providing relationship instances which are re-used internally for syncing</summary>
	internal static class StaticEbayCombinedOrderRelationRelations
	{
		internal static readonly IEntityRelation EbayOrderEntityUsingOrderIDStatic = new EbayCombinedOrderRelationRelations().EbayOrderEntityUsingOrderID;
		internal static readonly IEntityRelation EbayStoreEntityUsingStoreIDStatic = new EbayCombinedOrderRelationRelations().EbayStoreEntityUsingStoreID;

		/// <summary>CTor</summary>
		static StaticEbayCombinedOrderRelationRelations()
		{
		}
	}
}
