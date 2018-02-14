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
	/// <summary>Implements the relations factory for the entity: UpsProfilePackage. </summary>
	public partial class UpsProfilePackageRelations : PackageProfileRelations
	{
		/// <summary>CTor</summary>
		public UpsProfilePackageRelations()
		{
		}

		/// <summary>Gets all relations of the UpsProfilePackageEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = base.GetAllRelations();
			toReturn.Add(this.UpsProfileEntityUsingShippingProfileID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between UpsProfilePackageEntity and ShippingProfileEntity over the m:1 relation they have, using the relation between the fields:
		/// UpsProfilePackage.ShippingProfileID - ShippingProfile.ShippingProfileID
		/// </summary>
		public override IEntityRelation ShippingProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "ShippingProfile", false);
				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, UpsProfilePackageFields.ShippingProfileID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsProfilePackageEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between UpsProfilePackageEntity and UpsProfileEntity over the m:1 relation they have, using the relation between the fields:
		/// UpsProfilePackage.ShippingProfileID - UpsProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation UpsProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "UpsProfile", false);
				relation.AddEntityFieldPair(UpsProfileFields.ShippingProfileID, UpsProfilePackageFields.ShippingProfileID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsProfileEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsProfilePackageEntity", true);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UpsProfilePackageEntity and PackageProfileEntity over the 1:1 relation they have, which is used to build a target per entity hierarchy</summary>
		internal IEntityRelation RelationToSuperTypePackageProfileEntity
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, false);
				relation.AddEntityFieldPair(PackageProfileFields.PackageProfileID, UpsProfilePackageFields.PackageProfileID);
				relation.IsHierarchyRelation=true;
				return relation;
			}
		}

		
		/// <summary>Returns the relation object the entity, to which this relation factory belongs, has with the subtype with the specified name</summary>
		/// <param name="subTypeEntityName">name of direct subtype which is a subtype of the current entity through the relation to return.</param>
		/// <returns>relation which makes the current entity a supertype of the subtype entity with the name specified, or null if not applicable/found</returns>
		public override IEntityRelation GetSubTypeRelation(string subTypeEntityName)
		{
			return null;
		}
		
		/// <summary>Returns the relation object the entity, to which this relation factory belongs, has with its supertype, if applicable.</summary>
		/// <returns>relation which makes the current entity a subtype of its supertype entity or null if not applicable/found</returns>
		public override IEntityRelation GetSuperTypeRelation()
		{
			return this.RelationToSuperTypePackageProfileEntity;
		}

		#endregion

		#region Included Code

		#endregion
	}
	
	/// <summary>Static class which is used for providing relationship instances which are re-used internally for syncing</summary>
	internal static class StaticUpsProfilePackageRelations
	{
		internal static readonly IEntityRelation ShippingProfileEntityUsingShippingProfileIDStatic = new UpsProfilePackageRelations().ShippingProfileEntityUsingShippingProfileID;
		internal static readonly IEntityRelation UpsProfileEntityUsingShippingProfileIDStatic = new UpsProfilePackageRelations().UpsProfileEntityUsingShippingProfileID;

		/// <summary>CTor</summary>
		static StaticUpsProfilePackageRelations()
		{
		}
	}
}
