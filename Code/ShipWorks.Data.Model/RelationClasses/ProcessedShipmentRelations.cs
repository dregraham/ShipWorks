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
	/// <summary>Implements the relations factory for the entity: ProcessedShipment. </summary>
	public partial class ProcessedShipmentRelations
	{
		/// <summary>CTor</summary>
		public ProcessedShipmentRelations()
		{
		}

		/// <summary>Gets all relations of the ProcessedShipmentEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.ComputerEntityUsingProcessedComputerID);
			toReturn.Add(this.ComputerEntityUsingVoidedComputerID);
			toReturn.Add(this.UserEntityUsingProcessedUserID);
			toReturn.Add(this.UserEntityUsingVoidedUserID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between ProcessedShipmentEntity and ComputerEntity over the m:1 relation they have, using the relation between the fields:
		/// ProcessedShipment.ProcessedComputerID - Computer.ComputerID
		/// </summary>
		public virtual IEntityRelation ComputerEntityUsingProcessedComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "", false);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, ProcessedShipmentFields.ProcessedComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProcessedShipmentEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between ProcessedShipmentEntity and ComputerEntity over the m:1 relation they have, using the relation between the fields:
		/// ProcessedShipment.VoidedComputerID - Computer.ComputerID
		/// </summary>
		public virtual IEntityRelation ComputerEntityUsingVoidedComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "", false);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, ProcessedShipmentFields.VoidedComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProcessedShipmentEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between ProcessedShipmentEntity and UserEntity over the m:1 relation they have, using the relation between the fields:
		/// ProcessedShipment.ProcessedUserID - User.UserID
		/// </summary>
		public virtual IEntityRelation UserEntityUsingProcessedUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "", false);
				relation.AddEntityFieldPair(UserFields.UserID, ProcessedShipmentFields.ProcessedUserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProcessedShipmentEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between ProcessedShipmentEntity and UserEntity over the m:1 relation they have, using the relation between the fields:
		/// ProcessedShipment.VoidedUserID - User.UserID
		/// </summary>
		public virtual IEntityRelation UserEntityUsingVoidedUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "", false);
				relation.AddEntityFieldPair(UserFields.UserID, ProcessedShipmentFields.VoidedUserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ProcessedShipmentEntity", true);
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
	internal static class StaticProcessedShipmentRelations
	{
		internal static readonly IEntityRelation ComputerEntityUsingProcessedComputerIDStatic = new ProcessedShipmentRelations().ComputerEntityUsingProcessedComputerID;
		internal static readonly IEntityRelation ComputerEntityUsingVoidedComputerIDStatic = new ProcessedShipmentRelations().ComputerEntityUsingVoidedComputerID;
		internal static readonly IEntityRelation UserEntityUsingProcessedUserIDStatic = new ProcessedShipmentRelations().UserEntityUsingProcessedUserID;
		internal static readonly IEntityRelation UserEntityUsingVoidedUserIDStatic = new ProcessedShipmentRelations().UserEntityUsingVoidedUserID;

		/// <summary>CTor</summary>
		static StaticProcessedShipmentRelations()
		{
		}
	}
}
