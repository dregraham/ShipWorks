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
	/// <summary>Implements the relations factory for the entity: DhlExpressProfilePackage. </summary>
	public partial class DhlExpressProfilePackageRelations
	{
		/// <summary>CTor</summary>
		public DhlExpressProfilePackageRelations()
		{
		}

		/// <summary>Gets all relations of the DhlExpressProfilePackageEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.DhlExpressProfileEntityUsingShippingProfileID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between DhlExpressProfilePackageEntity and DhlExpressProfileEntity over the m:1 relation they have, using the relation between the fields:
		/// DhlExpressProfilePackage.ShippingProfileID - DhlExpressProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation DhlExpressProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "DhlExpressProfile", false);
				relation.AddEntityFieldPair(DhlExpressProfileFields.ShippingProfileID, DhlExpressProfilePackageFields.ShippingProfileID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DhlExpressProfileEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DhlExpressProfilePackageEntity", true);
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
	internal static class StaticDhlExpressProfilePackageRelations
	{
		internal static readonly IEntityRelation DhlExpressProfileEntityUsingShippingProfileIDStatic = new DhlExpressProfilePackageRelations().DhlExpressProfileEntityUsingShippingProfileID;

		/// <summary>CTor</summary>
		static StaticDhlExpressProfilePackageRelations()
		{
		}
	}
}
