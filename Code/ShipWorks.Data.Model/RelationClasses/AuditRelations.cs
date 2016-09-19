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
	/// <summary>Implements the relations factory for the entity: Audit. </summary>
	public partial class AuditRelations
	{
		/// <summary>CTor</summary>
		public AuditRelations()
		{
		}

		/// <summary>Gets all relations of the AuditEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.AuditChangeEntityUsingAuditID);
			toReturn.Add(this.ComputerEntityUsingComputerID);
			toReturn.Add(this.UserEntityUsingUserID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between AuditEntity and AuditChangeEntity over the 1:n relation they have, using the relation between the fields:
		/// Audit.AuditID - AuditChange.AuditID
		/// </summary>
		public virtual IEntityRelation AuditChangeEntityUsingAuditID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "AuditChanges" , true);
				relation.AddEntityFieldPair(AuditFields.AuditID, AuditChangeFields.AuditID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AuditEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AuditChangeEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between AuditEntity and ComputerEntity over the m:1 relation they have, using the relation between the fields:
		/// Audit.ComputerID - Computer.ComputerID
		/// </summary>
		public virtual IEntityRelation ComputerEntityUsingComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Computer", false);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, AuditFields.ComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AuditEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between AuditEntity and UserEntity over the m:1 relation they have, using the relation between the fields:
		/// Audit.UserID - User.UserID
		/// </summary>
		public virtual IEntityRelation UserEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "User", false);
				relation.AddEntityFieldPair(UserFields.UserID, AuditFields.UserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AuditEntity", true);
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
	internal static class StaticAuditRelations
	{
		internal static readonly IEntityRelation AuditChangeEntityUsingAuditIDStatic = new AuditRelations().AuditChangeEntityUsingAuditID;
		internal static readonly IEntityRelation ComputerEntityUsingComputerIDStatic = new AuditRelations().ComputerEntityUsingComputerID;
		internal static readonly IEntityRelation UserEntityUsingUserIDStatic = new AuditRelations().UserEntityUsingUserID;

		/// <summary>CTor</summary>
		static StaticAuditRelations()
		{
		}
	}
}
