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
	/// <summary>Implements the relations factory for the entity: AuditChange. </summary>
	public partial class AuditChangeRelations
	{
		/// <summary>CTor</summary>
		public AuditChangeRelations()
		{
		}

		/// <summary>Gets all relations of the AuditChangeEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.AuditChangeDetailEntityUsingAuditChangeID);
			toReturn.Add(this.AuditEntityUsingAuditID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between AuditChangeEntity and AuditChangeDetailEntity over the 1:n relation they have, using the relation between the fields:
		/// AuditChange.AuditChangeID - AuditChangeDetail.AuditChangeID
		/// </summary>
		public virtual IEntityRelation AuditChangeDetailEntityUsingAuditChangeID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "AuditChangeDetails" , true);
				relation.AddEntityFieldPair(AuditChangeFields.AuditChangeID, AuditChangeDetailFields.AuditChangeID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AuditChangeEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AuditChangeDetailEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between AuditChangeEntity and AuditEntity over the m:1 relation they have, using the relation between the fields:
		/// AuditChange.AuditID - Audit.AuditID
		/// </summary>
		public virtual IEntityRelation AuditEntityUsingAuditID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Audit", false);
				relation.AddEntityFieldPair(AuditFields.AuditID, AuditChangeFields.AuditID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AuditEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AuditChangeEntity", true);
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
	internal static class StaticAuditChangeRelations
	{
		internal static readonly IEntityRelation AuditChangeDetailEntityUsingAuditChangeIDStatic = new AuditChangeRelations().AuditChangeDetailEntityUsingAuditChangeID;
		internal static readonly IEntityRelation AuditEntityUsingAuditIDStatic = new AuditChangeRelations().AuditEntityUsingAuditID;

		/// <summary>CTor</summary>
		static StaticAuditChangeRelations()
		{
		}
	}
}
