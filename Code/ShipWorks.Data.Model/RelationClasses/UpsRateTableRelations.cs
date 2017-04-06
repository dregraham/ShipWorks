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
			toReturn.Add(this.UpsLocalRateEntityUsingUpsRateTableID);
			toReturn.Add(this.UpsLocalRateSurchargeEntityUsingUpsRateTableID);
			toReturn.Add(this.UpsAccountEntityUsingUpsAccountID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between UpsRateTableEntity and UpsLocalRateEntity over the 1:n relation they have, using the relation between the fields:
		/// UpsRateTable.UpsRateTableID - UpsLocalRate.UpsRateTableID
		/// </summary>
		public virtual IEntityRelation UpsLocalRateEntityUsingUpsRateTableID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "UpsLocalRate" , true);
				relation.AddEntityFieldPair(UpsRateTableFields.UpsRateTableID, UpsLocalRateFields.UpsRateTableID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsRateTableEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsLocalRateEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UpsRateTableEntity and UpsLocalRateSurchargeEntity over the 1:n relation they have, using the relation between the fields:
		/// UpsRateTable.UpsRateTableID - UpsLocalRateSurcharge.UpsRateTableID
		/// </summary>
		public virtual IEntityRelation UpsLocalRateSurchargeEntityUsingUpsRateTableID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "UpsLocalRateSurcharge" , true);
				relation.AddEntityFieldPair(UpsRateTableFields.UpsRateTableID, UpsLocalRateSurchargeFields.UpsRateTableID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsRateTableEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsLocalRateSurchargeEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between UpsRateTableEntity and UpsAccountEntity over the m:1 relation they have, using the relation between the fields:
		/// UpsRateTable.UpsAccountID - UpsAccount.UpsAccountID
		/// </summary>
		public virtual IEntityRelation UpsAccountEntityUsingUpsAccountID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "UpsAccount", false);
				relation.AddEntityFieldPair(UpsAccountFields.UpsAccountID, UpsRateTableFields.UpsAccountID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsAccountEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsRateTableEntity", true);
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
		internal static readonly IEntityRelation UpsLocalRateEntityUsingUpsRateTableIDStatic = new UpsRateTableRelations().UpsLocalRateEntityUsingUpsRateTableID;
		internal static readonly IEntityRelation UpsLocalRateSurchargeEntityUsingUpsRateTableIDStatic = new UpsRateTableRelations().UpsLocalRateSurchargeEntityUsingUpsRateTableID;
		internal static readonly IEntityRelation UpsAccountEntityUsingUpsAccountIDStatic = new UpsRateTableRelations().UpsAccountEntityUsingUpsAccountID;

		/// <summary>CTor</summary>
		static StaticUpsRateTableRelations()
		{
		}
	}
}
