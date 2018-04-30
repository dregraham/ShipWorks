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
	/// <summary>Implements the relations factory for the entity: PackageProfile. </summary>
	public partial class PackageProfileRelations : IRelationFactory
	{
		/// <summary>CTor</summary>
		public PackageProfileRelations()
		{
		}

		/// <summary>Gets all relations of the PackageProfileEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.ShippingProfileEntityUsingShippingProfileID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between PackageProfileEntity and ShippingProfileEntity over the m:1 relation they have, using the relation between the fields:
		/// PackageProfile.ShippingProfileID - ShippingProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation ShippingProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "ShippingProfile", false);
				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, PackageProfileFields.ShippingProfileID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PackageProfileEntity", true);
				return relation;
			}
		}



		/// <summary>Returns a new IEntityRelation object, between PackageProfileEntity and FedExProfilePackageEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeFedExProfilePackageEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(PackageProfileFields.PackageProfileID, FedExProfilePackageFields.PackageProfileID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between PackageProfileEntity and UpsProfilePackageEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>		
		internal IEntityRelation RelationToSubTypeUpsProfilePackageEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, true);
				relation.AddEntityFieldPair(PackageProfileFields.PackageProfileID, UpsProfilePackageFields.PackageProfileID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}
		
		/// <summary>Returns the relation object the entity, to which this relation factory belongs, has with the subtype with the specified name</summary>
		/// <param name="subTypeEntityName">name of direct subtype which is a subtype of the current entity through the relation to return.</param>
		/// <returns>relation which makes the current entity a supertype of the subtype entity with the name specified, or null if not applicable/found</returns>
		public virtual IEntityRelation GetSubTypeRelation(string subTypeEntityName)
		{
			switch(subTypeEntityName)
			{
				case "FedExProfilePackageEntity":
					return this.RelationToSubTypeFedExProfilePackageEntity;
				case "UpsProfilePackageEntity":
					return this.RelationToSubTypeUpsProfilePackageEntity;
				default:
					return null;
			}
		}
		
		/// <summary>Returns the relation object the entity, to which this relation factory belongs, has with its supertype, if applicable.</summary>
		/// <returns>relation which makes the current entity a subtype of its supertype entity or null if not applicable/found</returns>
		public virtual IEntityRelation GetSuperTypeRelation()
		{
			return null;
		}

		#endregion

		#region Included Code

		#endregion
	}
	
	/// <summary>Static class which is used for providing relationship instances which are re-used internally for syncing</summary>
	internal static class StaticPackageProfileRelations
	{
		internal static readonly IEntityRelation ShippingProfileEntityUsingShippingProfileIDStatic = new PackageProfileRelations().ShippingProfileEntityUsingShippingProfileID;

		/// <summary>CTor</summary>
		static StaticPackageProfileRelations()
		{
		}
	}
}
