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
	/// <summary>Implements the relations factory for the entity: ChannelAdvisorOrderSearch. </summary>
	public partial class ChannelAdvisorOrderSearchRelations
	{
		/// <summary>CTor</summary>
		public ChannelAdvisorOrderSearchRelations()
		{
		}

		/// <summary>Gets all relations of the ChannelAdvisorOrderSearchEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.ChannelAdvisorOrderEntityUsingOrderID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between ChannelAdvisorOrderSearchEntity and ChannelAdvisorOrderEntity over the m:1 relation they have, using the relation between the fields:
		/// ChannelAdvisorOrderSearch.OrderID - ChannelAdvisorOrder.OrderID
		/// </summary>
		public virtual IEntityRelation ChannelAdvisorOrderEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "ChannelAdvisorOrder", false);
				relation.AddEntityFieldPair(ChannelAdvisorOrderFields.OrderID, ChannelAdvisorOrderSearchFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ChannelAdvisorOrderEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ChannelAdvisorOrderSearchEntity", true);
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
	internal static class StaticChannelAdvisorOrderSearchRelations
	{
		internal static readonly IEntityRelation ChannelAdvisorOrderEntityUsingOrderIDStatic = new ChannelAdvisorOrderSearchRelations().ChannelAdvisorOrderEntityUsingOrderID;

		/// <summary>CTor</summary>
		static StaticChannelAdvisorOrderSearchRelations()
		{
		}
	}
}
