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
	/// <summary>Implements the relations factory for the entity: FilterNode. </summary>
	public partial class FilterNodeRelations
	{
		/// <summary>CTor</summary>
		public FilterNodeRelations()
		{
		}

		/// <summary>Gets all relations of the FilterNodeEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.FilterLayoutEntityUsingFilterNodeID);
			toReturn.Add(this.FilterNodeEntityUsingParentFilterNodeID);
			toReturn.Add(this.FilterNodeColumnSettingsEntityUsingFilterNodeID);
			toReturn.Add(this.FilterNodeEntityUsingFilterNodeIDParentFilterNodeID);
			toReturn.Add(this.FilterNodeContentEntityUsingFilterNodeContentID);
			toReturn.Add(this.FilterSequenceEntityUsingFilterSequenceID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between FilterNodeEntity and FilterLayoutEntity over the 1:n relation they have, using the relation between the fields:
		/// FilterNode.FilterNodeID - FilterLayout.FilterNodeID
		/// </summary>
		public virtual IEntityRelation FilterLayoutEntityUsingFilterNodeID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(FilterNodeFields.FilterNodeID, FilterLayoutFields.FilterNodeID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterLayoutEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between FilterNodeEntity and FilterNodeEntity over the 1:n relation they have, using the relation between the fields:
		/// FilterNode.FilterNodeID - FilterNode.ParentFilterNodeID
		/// </summary>
		public virtual IEntityRelation FilterNodeEntityUsingParentFilterNodeID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ChildNodes" , true);
				relation.AddEntityFieldPair(FilterNodeFields.FilterNodeID, FilterNodeFields.ParentFilterNodeID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between FilterNodeEntity and FilterNodeColumnSettingsEntity over the 1:n relation they have, using the relation between the fields:
		/// FilterNode.FilterNodeID - FilterNodeColumnSettings.FilterNodeID
		/// </summary>
		public virtual IEntityRelation FilterNodeColumnSettingsEntityUsingFilterNodeID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(FilterNodeFields.FilterNodeID, FilterNodeColumnSettingsFields.FilterNodeID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeColumnSettingsEntity", false);
				return relation;
			}
		}


		/// <summary>Returns a new IEntityRelation object, between FilterNodeEntity and FilterNodeEntity over the m:1 relation they have, using the relation between the fields:
		/// FilterNode.ParentFilterNodeID - FilterNode.FilterNodeID
		/// </summary>
		public virtual IEntityRelation FilterNodeEntityUsingFilterNodeIDParentFilterNodeID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "ParentNode", false);
				relation.AddEntityFieldPair(FilterNodeFields.FilterNodeID, FilterNodeFields.ParentFilterNodeID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between FilterNodeEntity and FilterNodeContentEntity over the m:1 relation they have, using the relation between the fields:
		/// FilterNode.FilterNodeContentID - FilterNodeContent.FilterNodeContentID
		/// </summary>
		public virtual IEntityRelation FilterNodeContentEntityUsingFilterNodeContentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "FilterNodeContent", false);
				relation.AddEntityFieldPair(FilterNodeContentFields.FilterNodeContentID, FilterNodeFields.FilterNodeContentID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeContentEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between FilterNodeEntity and FilterSequenceEntity over the m:1 relation they have, using the relation between the fields:
		/// FilterNode.FilterSequenceID - FilterSequence.FilterSequenceID
		/// </summary>
		public virtual IEntityRelation FilterSequenceEntityUsingFilterSequenceID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "FilterSequence", false);
				relation.AddEntityFieldPair(FilterSequenceFields.FilterSequenceID, FilterNodeFields.FilterSequenceID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterSequenceEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeEntity", true);
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
	internal static class StaticFilterNodeRelations
	{
		internal static readonly IEntityRelation FilterLayoutEntityUsingFilterNodeIDStatic = new FilterNodeRelations().FilterLayoutEntityUsingFilterNodeID;
		internal static readonly IEntityRelation FilterNodeEntityUsingParentFilterNodeIDStatic = new FilterNodeRelations().FilterNodeEntityUsingParentFilterNodeID;
		internal static readonly IEntityRelation FilterNodeColumnSettingsEntityUsingFilterNodeIDStatic = new FilterNodeRelations().FilterNodeColumnSettingsEntityUsingFilterNodeID;
		internal static readonly IEntityRelation FilterNodeEntityUsingFilterNodeIDParentFilterNodeIDStatic = new FilterNodeRelations().FilterNodeEntityUsingFilterNodeIDParentFilterNodeID;
		internal static readonly IEntityRelation FilterNodeContentEntityUsingFilterNodeContentIDStatic = new FilterNodeRelations().FilterNodeContentEntityUsingFilterNodeContentID;
		internal static readonly IEntityRelation FilterSequenceEntityUsingFilterSequenceIDStatic = new FilterNodeRelations().FilterSequenceEntityUsingFilterSequenceID;

		/// <summary>CTor</summary>
		static StaticFilterNodeRelations()
		{
		}
	}
}
