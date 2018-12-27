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
	/// <summary>Implements the relations factory for the entity: Template. </summary>
	public partial class TemplateRelations
	{
		/// <summary>CTor</summary>
		public TemplateRelations()
		{
		}

		/// <summary>Gets all relations of the TemplateEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.TemplateComputerSettingsEntityUsingTemplateID);
			toReturn.Add(this.TemplateStoreSettingsEntityUsingTemplateID);
			toReturn.Add(this.TemplateUserSettingsEntityUsingTemplateID);
			toReturn.Add(this.TemplateFolderEntityUsingParentFolderID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between TemplateEntity and TemplateComputerSettingsEntity over the 1:n relation they have, using the relation between the fields:
		/// Template.TemplateID - TemplateComputerSettings.TemplateID
		/// </summary>
		public virtual IEntityRelation TemplateComputerSettingsEntityUsingTemplateID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ComputerSettings" , true);
				relation.AddEntityFieldPair(TemplateFields.TemplateID, TemplateComputerSettingsFields.TemplateID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateComputerSettingsEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between TemplateEntity and TemplateStoreSettingsEntity over the 1:n relation they have, using the relation between the fields:
		/// Template.TemplateID - TemplateStoreSettings.TemplateID
		/// </summary>
		public virtual IEntityRelation TemplateStoreSettingsEntityUsingTemplateID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "StoreSettings" , true);
				relation.AddEntityFieldPair(TemplateFields.TemplateID, TemplateStoreSettingsFields.TemplateID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateStoreSettingsEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between TemplateEntity and TemplateUserSettingsEntity over the 1:n relation they have, using the relation between the fields:
		/// Template.TemplateID - TemplateUserSettings.TemplateID
		/// </summary>
		public virtual IEntityRelation TemplateUserSettingsEntityUsingTemplateID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "UserSettings" , true);
				relation.AddEntityFieldPair(TemplateFields.TemplateID, TemplateUserSettingsFields.TemplateID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateUserSettingsEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between TemplateEntity and TemplateFolderEntity over the m:1 relation they have, using the relation between the fields:
		/// Template.ParentFolderID - TemplateFolder.TemplateFolderID
		/// </summary>
		public virtual IEntityRelation TemplateFolderEntityUsingParentFolderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "ParentFolder", false);
				relation.AddEntityFieldPair(TemplateFolderFields.TemplateFolderID, TemplateFields.ParentFolderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateFolderEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateEntity", true);
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
	internal static class StaticTemplateRelations
	{
		internal static readonly IEntityRelation TemplateComputerSettingsEntityUsingTemplateIDStatic = new TemplateRelations().TemplateComputerSettingsEntityUsingTemplateID;
		internal static readonly IEntityRelation TemplateStoreSettingsEntityUsingTemplateIDStatic = new TemplateRelations().TemplateStoreSettingsEntityUsingTemplateID;
		internal static readonly IEntityRelation TemplateUserSettingsEntityUsingTemplateIDStatic = new TemplateRelations().TemplateUserSettingsEntityUsingTemplateID;
		internal static readonly IEntityRelation TemplateFolderEntityUsingParentFolderIDStatic = new TemplateRelations().TemplateFolderEntityUsingParentFolderID;

		/// <summary>CTor</summary>
		static StaticTemplateRelations()
		{
		}
	}
}
