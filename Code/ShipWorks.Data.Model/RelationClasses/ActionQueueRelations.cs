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
	/// <summary>Implements the relations factory for the entity: ActionQueue. </summary>
	public partial class ActionQueueRelations
	{
		/// <summary>CTor</summary>
		public ActionQueueRelations()
		{
		}

		/// <summary>Gets all relations of the ActionQueueEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.ActionQueueSelectionEntityUsingActionQueueID);
			toReturn.Add(this.ActionQueueStepEntityUsingActionQueueID);
			toReturn.Add(this.ActionEntityUsingActionID);
			toReturn.Add(this.ComputerEntityUsingTriggerComputerID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between ActionQueueEntity and ActionQueueSelectionEntity over the 1:n relation they have, using the relation between the fields:
		/// ActionQueue.ActionQueueID - ActionQueueSelection.ActionQueueID
		/// </summary>
		public virtual IEntityRelation ActionQueueSelectionEntityUsingActionQueueID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ActionQueueSelection" , true);
				relation.AddEntityFieldPair(ActionQueueFields.ActionQueueID, ActionQueueSelectionFields.ActionQueueID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ActionQueueEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ActionQueueSelectionEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ActionQueueEntity and ActionQueueStepEntity over the 1:n relation they have, using the relation between the fields:
		/// ActionQueue.ActionQueueID - ActionQueueStep.ActionQueueID
		/// </summary>
		public virtual IEntityRelation ActionQueueStepEntityUsingActionQueueID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Steps" , true);
				relation.AddEntityFieldPair(ActionQueueFields.ActionQueueID, ActionQueueStepFields.ActionQueueID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ActionQueueEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ActionQueueStepEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between ActionQueueEntity and ActionEntity over the m:1 relation they have, using the relation between the fields:
		/// ActionQueue.ActionID - Action.ActionID
		/// </summary>
		public virtual IEntityRelation ActionEntityUsingActionID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "", false);
				relation.AddEntityFieldPair(ActionFields.ActionID, ActionQueueFields.ActionID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ActionEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ActionQueueEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between ActionQueueEntity and ComputerEntity over the m:1 relation they have, using the relation between the fields:
		/// ActionQueue.TriggerComputerID - Computer.ComputerID
		/// </summary>
		public virtual IEntityRelation ComputerEntityUsingTriggerComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "", false);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, ActionQueueFields.TriggerComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ActionQueueEntity", true);
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
	internal static class StaticActionQueueRelations
	{
		internal static readonly IEntityRelation ActionQueueSelectionEntityUsingActionQueueIDStatic = new ActionQueueRelations().ActionQueueSelectionEntityUsingActionQueueID;
		internal static readonly IEntityRelation ActionQueueStepEntityUsingActionQueueIDStatic = new ActionQueueRelations().ActionQueueStepEntityUsingActionQueueID;
		internal static readonly IEntityRelation ActionEntityUsingActionIDStatic = new ActionQueueRelations().ActionEntityUsingActionID;
		internal static readonly IEntityRelation ComputerEntityUsingTriggerComputerIDStatic = new ActionQueueRelations().ComputerEntityUsingTriggerComputerID;

		/// <summary>CTor</summary>
		static StaticActionQueueRelations()
		{
		}
	}
}
