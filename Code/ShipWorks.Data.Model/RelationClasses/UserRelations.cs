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
	/// <summary>Implements the relations factory for the entity: User. </summary>
	public partial class UserRelations
	{
		/// <summary>CTor</summary>
		public UserRelations()
		{
		}

		/// <summary>Gets all relations of the UserEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.AuditEntityUsingUserID);
			toReturn.Add(this.DownloadEntityUsingUserID);
			toReturn.Add(this.FilterLayoutEntityUsingUserID);
			toReturn.Add(this.FilterNodeColumnSettingsEntityUsingUserID);
			toReturn.Add(this.GridColumnFormatEntityUsingUserID);
			toReturn.Add(this.NoteEntityUsingUserID);
			toReturn.Add(this.PermissionEntityUsingUserID);
			toReturn.Add(this.ShipmentEntityUsingProcessedUserID);
			toReturn.Add(this.ShipmentEntityUsingVoidedUserID);
			toReturn.Add(this.TemplateUserSettingsEntityUsingUserID);
			toReturn.Add(this.UserColumnSettingsEntityUsingUserID);
			toReturn.Add(this.UserShortcutOverridesEntityUsingUserID);
			toReturn.Add(this.UserSettingsEntityUsingUserID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between UserEntity and AuditEntity over the 1:n relation they have, using the relation between the fields:
		/// User.UserID - Audit.UserID
		/// </summary>
		public virtual IEntityRelation AuditEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(UserFields.UserID, AuditFields.UserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AuditEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UserEntity and DownloadEntity over the 1:n relation they have, using the relation between the fields:
		/// User.UserID - Download.UserID
		/// </summary>
		public virtual IEntityRelation DownloadEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(UserFields.UserID, DownloadFields.UserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DownloadEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UserEntity and FilterLayoutEntity over the 1:n relation they have, using the relation between the fields:
		/// User.UserID - FilterLayout.UserID
		/// </summary>
		public virtual IEntityRelation FilterLayoutEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(UserFields.UserID, FilterLayoutFields.UserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterLayoutEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UserEntity and FilterNodeColumnSettingsEntity over the 1:n relation they have, using the relation between the fields:
		/// User.UserID - FilterNodeColumnSettings.UserID
		/// </summary>
		public virtual IEntityRelation FilterNodeColumnSettingsEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(UserFields.UserID, FilterNodeColumnSettingsFields.UserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeColumnSettingsEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UserEntity and GridColumnFormatEntity over the 1:n relation they have, using the relation between the fields:
		/// User.UserID - GridColumnFormat.UserID
		/// </summary>
		public virtual IEntityRelation GridColumnFormatEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(UserFields.UserID, GridColumnFormatFields.UserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("GridColumnFormatEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UserEntity and NoteEntity over the 1:n relation they have, using the relation between the fields:
		/// User.UserID - Note.UserID
		/// </summary>
		public virtual IEntityRelation NoteEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(UserFields.UserID, NoteFields.UserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("NoteEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UserEntity and PermissionEntity over the 1:n relation they have, using the relation between the fields:
		/// User.UserID - Permission.UserID
		/// </summary>
		public virtual IEntityRelation PermissionEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(UserFields.UserID, PermissionFields.UserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PermissionEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UserEntity and ShipmentEntity over the 1:n relation they have, using the relation between the fields:
		/// User.UserID - Shipment.ProcessedUserID
		/// </summary>
		public virtual IEntityRelation ShipmentEntityUsingProcessedUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(UserFields.UserID, ShipmentFields.ProcessedUserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UserEntity and ShipmentEntity over the 1:n relation they have, using the relation between the fields:
		/// User.UserID - Shipment.VoidedUserID
		/// </summary>
		public virtual IEntityRelation ShipmentEntityUsingVoidedUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(UserFields.UserID, ShipmentFields.VoidedUserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UserEntity and TemplateUserSettingsEntity over the 1:n relation they have, using the relation between the fields:
		/// User.UserID - TemplateUserSettings.UserID
		/// </summary>
		public virtual IEntityRelation TemplateUserSettingsEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(UserFields.UserID, TemplateUserSettingsFields.UserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateUserSettingsEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UserEntity and UserColumnSettingsEntity over the 1:n relation they have, using the relation between the fields:
		/// User.UserID - UserColumnSettings.UserID
		/// </summary>
		public virtual IEntityRelation UserColumnSettingsEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(UserFields.UserID, UserColumnSettingsFields.UserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserColumnSettingsEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UserEntity and UserShortcutOverridesEntity over the 1:n relation they have, using the relation between the fields:
		/// User.UserID - UserShortcutOverrides.UserID
		/// </summary>
		public virtual IEntityRelation UserShortcutOverridesEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ShortcutOverrides" , true);
				relation.AddEntityFieldPair(UserFields.UserID, UserShortcutOverridesFields.UserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserShortcutOverridesEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UserEntity and UserSettingsEntity over the 1:1 relation they have, using the relation between the fields:
		/// User.UserID - UserSettings.UserID
		/// </summary>
		public virtual IEntityRelation UserSettingsEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "Settings", true);

				relation.AddEntityFieldPair(UserFields.UserID, UserSettingsFields.UserID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserSettingsEntity", false);
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
	internal static class StaticUserRelations
	{
		internal static readonly IEntityRelation AuditEntityUsingUserIDStatic = new UserRelations().AuditEntityUsingUserID;
		internal static readonly IEntityRelation DownloadEntityUsingUserIDStatic = new UserRelations().DownloadEntityUsingUserID;
		internal static readonly IEntityRelation FilterLayoutEntityUsingUserIDStatic = new UserRelations().FilterLayoutEntityUsingUserID;
		internal static readonly IEntityRelation FilterNodeColumnSettingsEntityUsingUserIDStatic = new UserRelations().FilterNodeColumnSettingsEntityUsingUserID;
		internal static readonly IEntityRelation GridColumnFormatEntityUsingUserIDStatic = new UserRelations().GridColumnFormatEntityUsingUserID;
		internal static readonly IEntityRelation NoteEntityUsingUserIDStatic = new UserRelations().NoteEntityUsingUserID;
		internal static readonly IEntityRelation PermissionEntityUsingUserIDStatic = new UserRelations().PermissionEntityUsingUserID;
		internal static readonly IEntityRelation ShipmentEntityUsingProcessedUserIDStatic = new UserRelations().ShipmentEntityUsingProcessedUserID;
		internal static readonly IEntityRelation ShipmentEntityUsingVoidedUserIDStatic = new UserRelations().ShipmentEntityUsingVoidedUserID;
		internal static readonly IEntityRelation TemplateUserSettingsEntityUsingUserIDStatic = new UserRelations().TemplateUserSettingsEntityUsingUserID;
		internal static readonly IEntityRelation UserColumnSettingsEntityUsingUserIDStatic = new UserRelations().UserColumnSettingsEntityUsingUserID;
		internal static readonly IEntityRelation UserShortcutOverridesEntityUsingUserIDStatic = new UserRelations().UserShortcutOverridesEntityUsingUserID;
		internal static readonly IEntityRelation UserSettingsEntityUsingUserIDStatic = new UserRelations().UserSettingsEntityUsingUserID;

		/// <summary>CTor</summary>
		static StaticUserRelations()
		{
		}
	}
}
