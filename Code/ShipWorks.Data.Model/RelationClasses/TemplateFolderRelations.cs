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
	/// <summary>Implements the relations factory for the entity: TemplateFolder. </summary>
	public partial class TemplateFolderRelations
	{
		/// <summary>CTor</summary>
		public TemplateFolderRelations()
		{
		}

		/// <summary>Gets all relations of the TemplateFolderEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.TemplateEntityUsingParentFolderID);
			toReturn.Add(this.TemplateFolderEntityUsingParentFolderID);
			toReturn.Add(this.TemplateFolderEntityUsingTemplateFolderIDParentFolderID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between TemplateFolderEntity and TemplateEntity over the 1:n relation they have, using the relation between the fields:
		/// TemplateFolder.TemplateFolderID - Template.ParentFolderID
		/// </summary>
		public virtual IEntityRelation TemplateEntityUsingParentFolderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Templates" , true);
				relation.AddEntityFieldPair(TemplateFolderFields.TemplateFolderID, TemplateFields.ParentFolderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateFolderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between TemplateFolderEntity and TemplateFolderEntity over the 1:n relation they have, using the relation between the fields:
		/// TemplateFolder.TemplateFolderID - TemplateFolder.ParentFolderID
		/// </summary>
		public virtual IEntityRelation TemplateFolderEntityUsingParentFolderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ChildFolders" , true);
				relation.AddEntityFieldPair(TemplateFolderFields.TemplateFolderID, TemplateFolderFields.ParentFolderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateFolderEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateFolderEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between TemplateFolderEntity and TemplateFolderEntity over the m:1 relation they have, using the relation between the fields:
		/// TemplateFolder.ParentFolderID - TemplateFolder.TemplateFolderID
		/// </summary>
		public virtual IEntityRelation TemplateFolderEntityUsingTemplateFolderIDParentFolderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "ParentFolder", false);
				relation.AddEntityFieldPair(TemplateFolderFields.TemplateFolderID, TemplateFolderFields.ParentFolderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateFolderEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateFolderEntity", true);
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
	internal static class StaticTemplateFolderRelations
	{
		internal static readonly IEntityRelation TemplateEntityUsingParentFolderIDStatic = new TemplateFolderRelations().TemplateEntityUsingParentFolderID;
		internal static readonly IEntityRelation TemplateFolderEntityUsingParentFolderIDStatic = new TemplateFolderRelations().TemplateFolderEntityUsingParentFolderID;
		internal static readonly IEntityRelation TemplateFolderEntityUsingTemplateFolderIDParentFolderIDStatic = new TemplateFolderRelations().TemplateFolderEntityUsingTemplateFolderIDParentFolderID;

		/// <summary>CTor</summary>
		static StaticTemplateFolderRelations()
		{
		}
	}
}
