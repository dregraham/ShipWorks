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
	/// <summary>Implements the relations factory for the entity: FilterSequence. </summary>
	public partial class FilterSequenceRelations
	{
		/// <summary>CTor</summary>
		public FilterSequenceRelations()
		{
		}

		/// <summary>Gets all relations of the FilterSequenceEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.FilterNodeEntityUsingFilterSequenceID);
			toReturn.Add(this.FilterEntityUsingFilterID);
			toReturn.Add(this.FilterEntityUsingParentFilterID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between FilterSequenceEntity and FilterNodeEntity over the 1:n relation they have, using the relation between the fields:
		/// FilterSequence.FilterSequenceID - FilterNode.FilterSequenceID
		/// </summary>
		public virtual IEntityRelation FilterNodeEntityUsingFilterSequenceID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "NodesUsingSequence" , true);
				relation.AddEntityFieldPair(FilterSequenceFields.FilterSequenceID, FilterNodeFields.FilterSequenceID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterSequenceEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between FilterSequenceEntity and FilterEntity over the m:1 relation they have, using the relation between the fields:
		/// FilterSequence.FilterID - Filter.FilterID
		/// </summary>
		public virtual IEntityRelation FilterEntityUsingFilterID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Filter", false);
				relation.AddEntityFieldPair(FilterFields.FilterID, FilterSequenceFields.FilterID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterSequenceEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between FilterSequenceEntity and FilterEntity over the m:1 relation they have, using the relation between the fields:
		/// FilterSequence.ParentFilterID - Filter.FilterID
		/// </summary>
		public virtual IEntityRelation FilterEntityUsingParentFilterID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Parent", false);
				relation.AddEntityFieldPair(FilterFields.FilterID, FilterSequenceFields.ParentFilterID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterSequenceEntity", true);
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
	internal static class StaticFilterSequenceRelations
	{
		internal static readonly IEntityRelation FilterNodeEntityUsingFilterSequenceIDStatic = new FilterSequenceRelations().FilterNodeEntityUsingFilterSequenceID;
		internal static readonly IEntityRelation FilterEntityUsingFilterIDStatic = new FilterSequenceRelations().FilterEntityUsingFilterID;
		internal static readonly IEntityRelation FilterEntityUsingParentFilterIDStatic = new FilterSequenceRelations().FilterEntityUsingParentFilterID;

		/// <summary>CTor</summary>
		static StaticFilterSequenceRelations()
		{
		}
	}
}
