﻿///////////////////////////////////////////////////////////////
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
	/// <summary>Implements the relations factory for the entity: Filter. </summary>
	public partial class FilterRelations
	{
		/// <summary>CTor</summary>
		public FilterRelations()
		{
		}

		/// <summary>Gets all relations of the FilterEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.FilterSequenceEntityUsingFilterID);
			toReturn.Add(this.FilterSequenceEntityUsingParentFilterID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between FilterEntity and FilterSequenceEntity over the 1:n relation they have, using the relation between the fields:
		/// Filter.FilterID - FilterSequence.FilterID
		/// </summary>
		public virtual IEntityRelation FilterSequenceEntityUsingFilterID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "UsedBySequences" , true);
				relation.AddEntityFieldPair(FilterFields.FilterID, FilterSequenceFields.FilterID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterSequenceEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between FilterEntity and FilterSequenceEntity over the 1:n relation they have, using the relation between the fields:
		/// Filter.FilterID - FilterSequence.ParentFilterID
		/// </summary>
		public virtual IEntityRelation FilterSequenceEntityUsingParentFilterID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ChildSequences" , true);
				relation.AddEntityFieldPair(FilterFields.FilterID, FilterSequenceFields.ParentFilterID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterSequenceEntity", false);
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
	internal static class StaticFilterRelations
	{
		internal static readonly IEntityRelation FilterSequenceEntityUsingFilterIDStatic = new FilterRelations().FilterSequenceEntityUsingFilterID;
		internal static readonly IEntityRelation FilterSequenceEntityUsingParentFilterIDStatic = new FilterRelations().FilterSequenceEntityUsingParentFilterID;

		/// <summary>CTor</summary>
		static StaticFilterRelations()
		{
		}
	}
}
