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
	/// <summary>Implements the relations factory for the entity: GridColumnLayout. </summary>
	public partial class GridColumnLayoutRelations
	{
		/// <summary>CTor</summary>
		public GridColumnLayoutRelations()
		{
		}

		/// <summary>Gets all relations of the GridColumnLayoutEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.FilterNodeColumnSettingsEntityUsingGridColumnLayoutID);
			toReturn.Add(this.GridColumnPositionEntityUsingGridColumnLayoutID);
			toReturn.Add(this.UserColumnSettingsEntityUsingGridColumnLayoutID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between GridColumnLayoutEntity and FilterNodeColumnSettingsEntity over the 1:n relation they have, using the relation between the fields:
		/// GridColumnLayout.GridColumnLayoutID - FilterNodeColumnSettings.GridColumnLayoutID
		/// </summary>
		public virtual IEntityRelation FilterNodeColumnSettingsEntityUsingGridColumnLayoutID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(GridColumnLayoutFields.GridColumnLayoutID, FilterNodeColumnSettingsFields.GridColumnLayoutID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GridColumnLayoutEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeColumnSettingsEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GridColumnLayoutEntity and GridColumnPositionEntity over the 1:n relation they have, using the relation between the fields:
		/// GridColumnLayout.GridColumnLayoutID - GridColumnPosition.GridColumnLayoutID
		/// </summary>
		public virtual IEntityRelation GridColumnPositionEntityUsingGridColumnLayoutID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "GridColumnPositions" , true);
				relation.AddEntityFieldPair(GridColumnLayoutFields.GridColumnLayoutID, GridColumnPositionFields.GridColumnLayoutID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GridColumnLayoutEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GridColumnPositionEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between GridColumnLayoutEntity and UserColumnSettingsEntity over the 1:n relation they have, using the relation between the fields:
		/// GridColumnLayout.GridColumnLayoutID - UserColumnSettings.GridColumnLayoutID
		/// </summary>
		public virtual IEntityRelation UserColumnSettingsEntityUsingGridColumnLayoutID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(GridColumnLayoutFields.GridColumnLayoutID, UserColumnSettingsFields.GridColumnLayoutID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GridColumnLayoutEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserColumnSettingsEntity", false);
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
	internal static class StaticGridColumnLayoutRelations
	{
		internal static readonly IEntityRelation FilterNodeColumnSettingsEntityUsingGridColumnLayoutIDStatic = new GridColumnLayoutRelations().FilterNodeColumnSettingsEntityUsingGridColumnLayoutID;
		internal static readonly IEntityRelation GridColumnPositionEntityUsingGridColumnLayoutIDStatic = new GridColumnLayoutRelations().GridColumnPositionEntityUsingGridColumnLayoutID;
		internal static readonly IEntityRelation UserColumnSettingsEntityUsingGridColumnLayoutIDStatic = new GridColumnLayoutRelations().UserColumnSettingsEntityUsingGridColumnLayoutID;

		/// <summary>CTor</summary>
		static StaticGridColumnLayoutRelations()
		{
		}
	}
}
