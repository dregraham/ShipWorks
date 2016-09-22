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
	/// <summary>Implements the relations factory for the entity: TemplateUserSettings. </summary>
	public partial class TemplateUserSettingsRelations
	{
		/// <summary>CTor</summary>
		public TemplateUserSettingsRelations()
		{
		}

		/// <summary>Gets all relations of the TemplateUserSettingsEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.TemplateEntityUsingTemplateID);
			toReturn.Add(this.UserEntityUsingUserID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between TemplateUserSettingsEntity and TemplateEntity over the m:1 relation they have, using the relation between the fields:
		/// TemplateUserSettings.TemplateID - Template.TemplateID
		/// </summary>
		public virtual IEntityRelation TemplateEntityUsingTemplateID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Template", false);
				relation.AddEntityFieldPair(TemplateFields.TemplateID, TemplateUserSettingsFields.TemplateID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateUserSettingsEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between TemplateUserSettingsEntity and UserEntity over the m:1 relation they have, using the relation between the fields:
		/// TemplateUserSettings.UserID - User.UserID
		/// </summary>
		public virtual IEntityRelation UserEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "", false);
				relation.AddEntityFieldPair(UserFields.UserID, TemplateUserSettingsFields.UserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateUserSettingsEntity", true);
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
	internal static class StaticTemplateUserSettingsRelations
	{
		internal static readonly IEntityRelation TemplateEntityUsingTemplateIDStatic = new TemplateUserSettingsRelations().TemplateEntityUsingTemplateID;
		internal static readonly IEntityRelation UserEntityUsingUserIDStatic = new TemplateUserSettingsRelations().UserEntityUsingUserID;

		/// <summary>CTor</summary>
		static StaticTemplateUserSettingsRelations()
		{
		}
	}
}
