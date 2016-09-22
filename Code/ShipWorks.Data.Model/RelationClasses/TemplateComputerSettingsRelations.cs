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
	/// <summary>Implements the relations factory for the entity: TemplateComputerSettings. </summary>
	public partial class TemplateComputerSettingsRelations
	{
		/// <summary>CTor</summary>
		public TemplateComputerSettingsRelations()
		{
		}

		/// <summary>Gets all relations of the TemplateComputerSettingsEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.ComputerEntityUsingComputerID);
			toReturn.Add(this.TemplateEntityUsingTemplateID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between TemplateComputerSettingsEntity and ComputerEntity over the m:1 relation they have, using the relation between the fields:
		/// TemplateComputerSettings.ComputerID - Computer.ComputerID
		/// </summary>
		public virtual IEntityRelation ComputerEntityUsingComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "", false);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, TemplateComputerSettingsFields.ComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateComputerSettingsEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between TemplateComputerSettingsEntity and TemplateEntity over the m:1 relation they have, using the relation between the fields:
		/// TemplateComputerSettings.TemplateID - Template.TemplateID
		/// </summary>
		public virtual IEntityRelation TemplateEntityUsingTemplateID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Template", false);
				relation.AddEntityFieldPair(TemplateFields.TemplateID, TemplateComputerSettingsFields.TemplateID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateComputerSettingsEntity", true);
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
	internal static class StaticTemplateComputerSettingsRelations
	{
		internal static readonly IEntityRelation ComputerEntityUsingComputerIDStatic = new TemplateComputerSettingsRelations().ComputerEntityUsingComputerID;
		internal static readonly IEntityRelation TemplateEntityUsingTemplateIDStatic = new TemplateComputerSettingsRelations().TemplateEntityUsingTemplateID;

		/// <summary>CTor</summary>
		static StaticTemplateComputerSettingsRelations()
		{
		}
	}
}
