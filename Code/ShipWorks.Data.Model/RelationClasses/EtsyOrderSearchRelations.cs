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
	/// <summary>Implements the relations factory for the entity: EtsyOrderSearch. </summary>
	public partial class EtsyOrderSearchRelations
	{
		/// <summary>CTor</summary>
		public EtsyOrderSearchRelations()
		{
		}

		/// <summary>Gets all relations of the EtsyOrderSearchEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.EtsyOrderEntityUsingOrderID);
			toReturn.Add(this.StoreEntityUsingStoreID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between EtsyOrderSearchEntity and EtsyOrderEntity over the m:1 relation they have, using the relation between the fields:
		/// EtsyOrderSearch.OrderID - EtsyOrder.OrderID
		/// </summary>
		public virtual IEntityRelation EtsyOrderEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "EtsyOrder", false);
				relation.AddEntityFieldPair(EtsyOrderFields.OrderID, EtsyOrderSearchFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EtsyOrderEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EtsyOrderSearchEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between EtsyOrderSearchEntity and StoreEntity over the m:1 relation they have, using the relation between the fields:
		/// EtsyOrderSearch.StoreID - Store.StoreID
		/// </summary>
		public virtual IEntityRelation StoreEntityUsingStoreID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Store", false);
				relation.AddEntityFieldPair(StoreFields.StoreID, EtsyOrderSearchFields.StoreID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StoreEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EtsyOrderSearchEntity", true);
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
	internal static class StaticEtsyOrderSearchRelations
	{
		internal static readonly IEntityRelation EtsyOrderEntityUsingOrderIDStatic = new EtsyOrderSearchRelations().EtsyOrderEntityUsingOrderID;
		internal static readonly IEntityRelation StoreEntityUsingStoreIDStatic = new EtsyOrderSearchRelations().StoreEntityUsingStoreID;

		/// <summary>CTor</summary>
		static StaticEtsyOrderSearchRelations()
		{
		}
	}
}
