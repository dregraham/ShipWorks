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
	/// <summary>Implements the relations factory for the entity: UserColumnSettings. </summary>
	public partial class UserColumnSettingsRelations
	{
		/// <summary>CTor</summary>
		public UserColumnSettingsRelations()
		{
		}

		/// <summary>Gets all relations of the UserColumnSettingsEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.GridColumnLayoutEntityUsingGridColumnLayoutID);
			toReturn.Add(this.UserEntityUsingUserID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between UserColumnSettingsEntity and GridColumnLayoutEntity over the m:1 relation they have, using the relation between the fields:
		/// UserColumnSettings.GridColumnLayoutID - GridColumnLayout.GridColumnLayoutID
		/// </summary>
		public virtual IEntityRelation GridColumnLayoutEntityUsingGridColumnLayoutID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "", false);
				relation.AddEntityFieldPair(GridColumnLayoutFields.GridColumnLayoutID, UserColumnSettingsFields.GridColumnLayoutID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GridColumnLayoutEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserColumnSettingsEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between UserColumnSettingsEntity and UserEntity over the m:1 relation they have, using the relation between the fields:
		/// UserColumnSettings.UserID - User.UserID
		/// </summary>
		public virtual IEntityRelation UserEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "", false);
				relation.AddEntityFieldPair(UserFields.UserID, UserColumnSettingsFields.UserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserColumnSettingsEntity", true);
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
	internal static class StaticUserColumnSettingsRelations
	{
		internal static readonly IEntityRelation GridColumnLayoutEntityUsingGridColumnLayoutIDStatic = new UserColumnSettingsRelations().GridColumnLayoutEntityUsingGridColumnLayoutID;
		internal static readonly IEntityRelation UserEntityUsingUserIDStatic = new UserColumnSettingsRelations().UserEntityUsingUserID;

		/// <summary>CTor</summary>
		static StaticUserColumnSettingsRelations()
		{
		}
	}
}
