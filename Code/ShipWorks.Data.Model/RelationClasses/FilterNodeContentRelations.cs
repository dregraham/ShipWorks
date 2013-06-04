///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates.NET20
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
	/// <summary>Implements the static Relations variant for the entity: FilterNodeContent. </summary>
	public partial class FilterNodeContentRelations
	{
		/// <summary>CTor</summary>
		public FilterNodeContentRelations()
		{
		}

		/// <summary>Gets all relations of the FilterNodeContentEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.FilterNodeEntityUsingFilterNodeContentID);
			toReturn.Add(this.FilterNodeContentDetailEntityUsingFilterNodeContentID);


			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between FilterNodeContentEntity and FilterNodeEntity over the 1:n relation they have, using the relation between the fields:
		/// FilterNodeContent.FilterNodeContentID - FilterNode.FilterNodeContentID
		/// </summary>
		public virtual IEntityRelation FilterNodeEntityUsingFilterNodeContentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(FilterNodeContentFields.FilterNodeContentID, FilterNodeFields.FilterNodeContentID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeContentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between FilterNodeContentEntity and FilterNodeContentDetailEntity over the 1:n relation they have, using the relation between the fields:
		/// FilterNodeContent.FilterNodeContentID - FilterNodeContentDetail.FilterNodeContentID
		/// </summary>
		public virtual IEntityRelation FilterNodeContentDetailEntityUsingFilterNodeContentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "" , true);
				relation.AddEntityFieldPair(FilterNodeContentFields.FilterNodeContentID, FilterNodeContentDetailFields.FilterNodeContentID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeContentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FilterNodeContentDetailEntity", false);
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
}
