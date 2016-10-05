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
	/// <summary>Implements the relations factory for the entity: FedExProfilePackage. </summary>
	public partial class FedExProfilePackageRelations
	{
		/// <summary>CTor</summary>
		public FedExProfilePackageRelations()
		{
		}

		/// <summary>Gets all relations of the FedExProfilePackageEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.FedExProfileEntityUsingShippingProfileID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between FedExProfilePackageEntity and FedExProfileEntity over the m:1 relation they have, using the relation between the fields:
		/// FedExProfilePackage.ShippingProfileID - FedExProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation FedExProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "FedExProfile", false);
				relation.AddEntityFieldPair(FedExProfileFields.ShippingProfileID, FedExProfilePackageFields.ShippingProfileID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FedExProfileEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FedExProfilePackageEntity", true);
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
	internal static class StaticFedExProfilePackageRelations
	{
		internal static readonly IEntityRelation FedExProfileEntityUsingShippingProfileIDStatic = new FedExProfilePackageRelations().FedExProfileEntityUsingShippingProfileID;

		/// <summary>CTor</summary>
		static StaticFedExProfilePackageRelations()
		{
		}
	}
}
