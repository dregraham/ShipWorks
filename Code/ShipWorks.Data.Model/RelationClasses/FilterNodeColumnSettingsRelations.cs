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
	/// <summary>Implements the relations factory for the entity: FilterNodeColumnSettings. </summary>
	public partial class FilterNodeColumnSettingsRelations
	{
		/// <summary>CTor</summary>
		public FilterNodeColumnSettingsRelations()
		{
		}

		/// <summary>Gets all relations of the FilterNodeColumnSettingsEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.FilterNodeEntityUsingFilterNodeID);
			toReturn.Add(this.GridColumnLayoutEntityUsingGridColumnLayoutID);
			toReturn.Add(this.UserEntityUsingUserID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between FilterNodeColumnSettingsEntity and FilterNodeEntity over the m:1 relation they have, using the relation between the fields:
		/// FilterNodeColumnSettings.FilterNodeID - FilterNode.FilterNodeID
		/// </summary>
		public virtual IEntityRelation FilterNodeEntityUsingFilterNodeID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "FilterNode", false);
				relation.AddEntityFieldPair(FilterNodeFields.FilterNodeID, FilterNodeColumnSettingsFields.FilterNodeID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeColumnSettingsEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between FilterNodeColumnSettingsEntity and GridColumnLayoutEntity over the m:1 relation they have, using the relation between the fields:
		/// FilterNodeColumnSettings.GridColumnLayoutID - GridColumnLayout.GridColumnLayoutID
		/// </summary>
		public virtual IEntityRelation GridColumnLayoutEntityUsingGridColumnLayoutID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "", false);
				relation.AddEntityFieldPair(GridColumnLayoutFields.GridColumnLayoutID, FilterNodeColumnSettingsFields.GridColumnLayoutID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GridColumnLayoutEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeColumnSettingsEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between FilterNodeColumnSettingsEntity and UserEntity over the m:1 relation they have, using the relation between the fields:
		/// FilterNodeColumnSettings.UserID - User.UserID
		/// </summary>
		public virtual IEntityRelation UserEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "User", false);
				relation.AddEntityFieldPair(UserFields.UserID, FilterNodeColumnSettingsFields.UserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeColumnSettingsEntity", true);
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
	internal static class StaticFilterNodeColumnSettingsRelations
	{
		internal static readonly IEntityRelation FilterNodeEntityUsingFilterNodeIDStatic = new FilterNodeColumnSettingsRelations().FilterNodeEntityUsingFilterNodeID;
		internal static readonly IEntityRelation GridColumnLayoutEntityUsingGridColumnLayoutIDStatic = new FilterNodeColumnSettingsRelations().GridColumnLayoutEntityUsingGridColumnLayoutID;
		internal static readonly IEntityRelation UserEntityUsingUserIDStatic = new FilterNodeColumnSettingsRelations().UserEntityUsingUserID;

		/// <summary>CTor</summary>
		static StaticFilterNodeColumnSettingsRelations()
		{
		}
	}
}
