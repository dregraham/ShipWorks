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
	/// <summary>Implements the relations factory for the entity: UpsRateTable. </summary>
	public partial class UpsRateTableRelations
	{
		/// <summary>CTor</summary>
		public UpsRateTableRelations()
		{
		}

		/// <summary>Gets all relations of the UpsRateTableEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.UpsAccountEntityUsingUpsRateTableID);
			toReturn.Add(this.UpsRateEntityUsingUpsRateTableID);
			toReturn.Add(this.UpsRateSurchargeEntityUsingUpsRateTableID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between UpsRateTableEntity and UpsAccountEntity over the 1:n relation they have, using the relation between the fields:
		/// UpsRateTable.UpsRateTableID - UpsAccount.UpsRateTableID
		/// </summary>
		public virtual IEntityRelation UpsAccountEntityUsingUpsRateTableID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "UpsAccount" , true);
				relation.AddEntityFieldPair(UpsRateTableFields.UpsRateTableID, UpsAccountFields.UpsRateTableID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsRateTableEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsAccountEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UpsRateTableEntity and UpsRateEntity over the 1:n relation they have, using the relation between the fields:
		/// UpsRateTable.UpsRateTableID - UpsRate.UpsRateTableID
		/// </summary>
		public virtual IEntityRelation UpsRateEntityUsingUpsRateTableID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "UpsRate" , true);
				relation.AddEntityFieldPair(UpsRateTableFields.UpsRateTableID, UpsRateFields.UpsRateTableID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsRateTableEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsRateEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UpsRateTableEntity and UpsRateSurchargeEntity over the 1:n relation they have, using the relation between the fields:
		/// UpsRateTable.UpsRateTableID - UpsRateSurcharge.UpsRateTableID
		/// </summary>
		public virtual IEntityRelation UpsRateSurchargeEntityUsingUpsRateTableID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "UpsRateSurcharge" , true);
				relation.AddEntityFieldPair(UpsRateTableFields.UpsRateTableID, UpsRateSurchargeFields.UpsRateTableID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsRateTableEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsRateSurchargeEntity", false);
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
	internal static class StaticUpsRateTableRelations
	{
		internal static readonly IEntityRelation UpsAccountEntityUsingUpsRateTableIDStatic = new UpsRateTableRelations().UpsAccountEntityUsingUpsRateTableID;
		internal static readonly IEntityRelation UpsRateEntityUsingUpsRateTableIDStatic = new UpsRateTableRelations().UpsRateEntityUsingUpsRateTableID;
		internal static readonly IEntityRelation UpsRateSurchargeEntityUsingUpsRateTableIDStatic = new UpsRateTableRelations().UpsRateSurchargeEntityUsingUpsRateTableID;

		/// <summary>CTor</summary>
		static StaticUpsRateTableRelations()
		{
		}
	}
}
