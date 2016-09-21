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
	/// <summary>Implements the relations factory for the entity: FilterLayout. </summary>
	public partial class FilterLayoutRelations
	{
		/// <summary>CTor</summary>
		public FilterLayoutRelations()
		{
		}

		/// <summary>Gets all relations of the FilterLayoutEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.FilterNodeEntityUsingFilterNodeID);
			toReturn.Add(this.UserEntityUsingUserID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between FilterLayoutEntity and FilterNodeEntity over the m:1 relation they have, using the relation between the fields:
		/// FilterLayout.FilterNodeID - FilterNode.FilterNodeID
		/// </summary>
		public virtual IEntityRelation FilterNodeEntityUsingFilterNodeID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "FilterNode", false);
				relation.AddEntityFieldPair(FilterNodeFields.FilterNodeID, FilterLayoutFields.FilterNodeID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterLayoutEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between FilterLayoutEntity and UserEntity over the m:1 relation they have, using the relation between the fields:
		/// FilterLayout.UserID - User.UserID
		/// </summary>
		public virtual IEntityRelation UserEntityUsingUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "User", false);
				relation.AddEntityFieldPair(UserFields.UserID, FilterLayoutFields.UserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterLayoutEntity", true);
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
	internal static class StaticFilterLayoutRelations
	{
		internal static readonly IEntityRelation FilterNodeEntityUsingFilterNodeIDStatic = new FilterLayoutRelations().FilterNodeEntityUsingFilterNodeID;
		internal static readonly IEntityRelation UserEntityUsingUserIDStatic = new FilterLayoutRelations().UserEntityUsingUserID;

		/// <summary>CTor</summary>
		static StaticFilterLayoutRelations()
		{
		}
	}
}
