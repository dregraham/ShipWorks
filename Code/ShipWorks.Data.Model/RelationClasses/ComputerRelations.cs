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
	/// <summary>Implements the relations factory for the entity: Computer. </summary>
	public partial class ComputerRelations
	{
		/// <summary>CTor</summary>
		public ComputerRelations()
		{
		}

		/// <summary>Gets all relations of the ComputerEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.ActionQueueEntityUsingTriggerComputerID);
			toReturn.Add(this.AuditEntityUsingComputerID);
			toReturn.Add(this.DownloadEntityUsingComputerID);
			toReturn.Add(this.PrintResultEntityUsingComputerID);
			toReturn.Add(this.ServerMessageSignoffEntityUsingComputerID);
			toReturn.Add(this.ServiceStatusEntityUsingComputerID);
			toReturn.Add(this.ShipmentEntityUsingProcessedComputerID);
			toReturn.Add(this.ShipmentEntityUsingVoidedComputerID);
			toReturn.Add(this.TemplateComputerSettingsEntityUsingComputerID);
			toReturn.Add(this.VersionSignoffEntityUsingComputerID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between ComputerEntity and ActionQueueEntity over the 1:n relation they have, using the relation between the fields:
		/// Computer.ComputerID - ActionQueue.TriggerComputerID
		/// </summary>
		public virtual IEntityRelation ActionQueueEntityUsingTriggerComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, ActionQueueFields.TriggerComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ActionQueueEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ComputerEntity and AuditEntity over the 1:n relation they have, using the relation between the fields:
		/// Computer.ComputerID - Audit.ComputerID
		/// </summary>
		public virtual IEntityRelation AuditEntityUsingComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Audit" , true);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, AuditFields.ComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AuditEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ComputerEntity and DownloadEntity over the 1:n relation they have, using the relation between the fields:
		/// Computer.ComputerID - Download.ComputerID
		/// </summary>
		public virtual IEntityRelation DownloadEntityUsingComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, DownloadFields.ComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DownloadEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ComputerEntity and PrintResultEntity over the 1:n relation they have, using the relation between the fields:
		/// Computer.ComputerID - PrintResult.ComputerID
		/// </summary>
		public virtual IEntityRelation PrintResultEntityUsingComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, PrintResultFields.ComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PrintResultEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ComputerEntity and ServerMessageSignoffEntity over the 1:n relation they have, using the relation between the fields:
		/// Computer.ComputerID - ServerMessageSignoff.ComputerID
		/// </summary>
		public virtual IEntityRelation ServerMessageSignoffEntityUsingComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, ServerMessageSignoffFields.ComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ServerMessageSignoffEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ComputerEntity and ServiceStatusEntity over the 1:n relation they have, using the relation between the fields:
		/// Computer.ComputerID - ServiceStatus.ComputerID
		/// </summary>
		public virtual IEntityRelation ServiceStatusEntityUsingComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ServiceStatus" , true);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, ServiceStatusFields.ComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ServiceStatusEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ComputerEntity and ShipmentEntity over the 1:n relation they have, using the relation between the fields:
		/// Computer.ComputerID - Shipment.ProcessedComputerID
		/// </summary>
		public virtual IEntityRelation ShipmentEntityUsingProcessedComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, ShipmentFields.ProcessedComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ComputerEntity and ShipmentEntity over the 1:n relation they have, using the relation between the fields:
		/// Computer.ComputerID - Shipment.VoidedComputerID
		/// </summary>
		public virtual IEntityRelation ShipmentEntityUsingVoidedComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, ShipmentFields.VoidedComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ComputerEntity and TemplateComputerSettingsEntity over the 1:n relation they have, using the relation between the fields:
		/// Computer.ComputerID - TemplateComputerSettings.ComputerID
		/// </summary>
		public virtual IEntityRelation TemplateComputerSettingsEntityUsingComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, TemplateComputerSettingsFields.ComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("TemplateComputerSettingsEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ComputerEntity and VersionSignoffEntity over the 1:n relation they have, using the relation between the fields:
		/// Computer.ComputerID - VersionSignoff.ComputerID
		/// </summary>
		public virtual IEntityRelation VersionSignoffEntityUsingComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, VersionSignoffFields.ComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("VersionSignoffEntity", false);
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
	internal static class StaticComputerRelations
	{
		internal static readonly IEntityRelation ActionQueueEntityUsingTriggerComputerIDStatic = new ComputerRelations().ActionQueueEntityUsingTriggerComputerID;
		internal static readonly IEntityRelation AuditEntityUsingComputerIDStatic = new ComputerRelations().AuditEntityUsingComputerID;
		internal static readonly IEntityRelation DownloadEntityUsingComputerIDStatic = new ComputerRelations().DownloadEntityUsingComputerID;
		internal static readonly IEntityRelation PrintResultEntityUsingComputerIDStatic = new ComputerRelations().PrintResultEntityUsingComputerID;
		internal static readonly IEntityRelation ServerMessageSignoffEntityUsingComputerIDStatic = new ComputerRelations().ServerMessageSignoffEntityUsingComputerID;
		internal static readonly IEntityRelation ServiceStatusEntityUsingComputerIDStatic = new ComputerRelations().ServiceStatusEntityUsingComputerID;
		internal static readonly IEntityRelation ShipmentEntityUsingProcessedComputerIDStatic = new ComputerRelations().ShipmentEntityUsingProcessedComputerID;
		internal static readonly IEntityRelation ShipmentEntityUsingVoidedComputerIDStatic = new ComputerRelations().ShipmentEntityUsingVoidedComputerID;
		internal static readonly IEntityRelation TemplateComputerSettingsEntityUsingComputerIDStatic = new ComputerRelations().TemplateComputerSettingsEntityUsingComputerID;
		internal static readonly IEntityRelation VersionSignoffEntityUsingComputerIDStatic = new ComputerRelations().VersionSignoffEntityUsingComputerID;

		/// <summary>CTor</summary>
		static StaticComputerRelations()
		{
		}
	}
}
