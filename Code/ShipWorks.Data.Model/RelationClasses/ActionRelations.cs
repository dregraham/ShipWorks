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
	/// <summary>Implements the relations factory for the entity: Action. </summary>
	public partial class ActionRelations
	{
		/// <summary>CTor</summary>
		public ActionRelations()
		{
		}

		/// <summary>Gets all relations of the ActionEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.ActionQueueEntityUsingActionID);
			toReturn.Add(this.ActionTaskEntityUsingActionID);
			toReturn.Add(this.ActionFilterTriggerEntityUsingActionID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between ActionEntity and ActionQueueEntity over the 1:n relation they have, using the relation between the fields:
		/// Action.ActionID - ActionQueue.ActionID
		/// </summary>
		public virtual IEntityRelation ActionQueueEntityUsingActionID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(ActionFields.ActionID, ActionQueueFields.ActionID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ActionEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ActionQueueEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ActionEntity and ActionTaskEntity over the 1:n relation they have, using the relation between the fields:
		/// Action.ActionID - ActionTask.ActionID
		/// </summary>
		public virtual IEntityRelation ActionTaskEntityUsingActionID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(ActionFields.ActionID, ActionTaskFields.ActionID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ActionEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ActionTaskEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ActionEntity and ActionFilterTriggerEntity over the 1:1 relation they have, using the relation between the fields:
		/// Action.ActionID - ActionFilterTrigger.ActionID
		/// </summary>
		public virtual IEntityRelation ActionFilterTriggerEntityUsingActionID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "", true);

				relation.AddEntityFieldPair(ActionFields.ActionID, ActionFilterTriggerFields.ActionID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ActionEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ActionFilterTriggerEntity", false);
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
	internal static class StaticActionRelations
	{
		internal static readonly IEntityRelation ActionQueueEntityUsingActionIDStatic = new ActionRelations().ActionQueueEntityUsingActionID;
		internal static readonly IEntityRelation ActionTaskEntityUsingActionIDStatic = new ActionRelations().ActionTaskEntityUsingActionID;
		internal static readonly IEntityRelation ActionFilterTriggerEntityUsingActionIDStatic = new ActionRelations().ActionFilterTriggerEntityUsingActionID;

		/// <summary>CTor</summary>
		static StaticActionRelations()
		{
		}
	}
}
