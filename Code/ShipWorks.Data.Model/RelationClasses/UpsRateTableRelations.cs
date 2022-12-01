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
			toReturn.Add(this.UpsLetterRateEntityUsingUpsRateTableID);
			toReturn.Add(this.UpsPackageRateEntityUsingUpsRateTableID);
			toReturn.Add(this.UpsPricePerPoundEntityUsingUpsRateTableID);
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

		/// <summary>Returns a new IEntityRelation object, between UpsRateTableEntity and UpsLetterRateEntity over the 1:n relation they have, using the relation between the fields:
		/// UpsRateTable.UpsRateTableID - UpsLetterRate.UpsRateTableID
		/// </summary>
		public virtual IEntityRelation UpsLetterRateEntityUsingUpsRateTableID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "UpsLetterRate" , true);
				relation.AddEntityFieldPair(UpsRateTableFields.UpsRateTableID, UpsLetterRateFields.UpsRateTableID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsRateTableEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsLetterRateEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UpsRateTableEntity and UpsPackageRateEntity over the 1:n relation they have, using the relation between the fields:
		/// UpsRateTable.UpsRateTableID - UpsPackageRate.UpsRateTableID
		/// </summary>
		public virtual IEntityRelation UpsPackageRateEntityUsingUpsRateTableID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "UpsPackageRate" , true);
				relation.AddEntityFieldPair(UpsRateTableFields.UpsRateTableID, UpsPackageRateFields.UpsRateTableID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsRateTableEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsPackageRateEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UpsRateTableEntity and UpsPricePerPoundEntity over the 1:n relation they have, using the relation between the fields:
		/// UpsRateTable.UpsRateTableID - UpsPricePerPound.UpsRateTableID
		/// </summary>
		public virtual IEntityRelation UpsPricePerPoundEntityUsingUpsRateTableID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "UpsPricePerPound" , true);
				relation.AddEntityFieldPair(UpsRateTableFields.UpsRateTableID, UpsPricePerPoundFields.UpsRateTableID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsRateTableEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsPricePerPoundEntity", false);
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
		internal static readonly IEntityRelation UpsLetterRateEntityUsingUpsRateTableIDStatic = new UpsRateTableRelations().UpsLetterRateEntityUsingUpsRateTableID;
		internal static readonly IEntityRelation UpsPackageRateEntityUsingUpsRateTableIDStatic = new UpsRateTableRelations().UpsPackageRateEntityUsingUpsRateTableID;
		internal static readonly IEntityRelation UpsPricePerPoundEntityUsingUpsRateTableIDStatic = new UpsRateTableRelations().UpsPricePerPoundEntityUsingUpsRateTableID;
		internal static readonly IEntityRelation UpsRateSurchargeEntityUsingUpsRateTableIDStatic = new UpsRateTableRelations().UpsRateSurchargeEntityUsingUpsRateTableID;

		/// <summary>CTor</summary>
		static StaticUpsRateTableRelations()
		{
		}
	}
}
