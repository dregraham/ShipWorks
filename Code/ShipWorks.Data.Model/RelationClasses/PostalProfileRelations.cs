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
	/// <summary>Implements the relations factory for the entity: PostalProfile. </summary>
	public partial class PostalProfileRelations
	{
		/// <summary>CTor</summary>
		public PostalProfileRelations()
		{
		}

		/// <summary>Gets all relations of the PostalProfileEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.EndiciaProfileEntityUsingShippingProfileID);
			toReturn.Add(this.ShippingProfileEntityUsingShippingProfileID);
			toReturn.Add(this.UspsProfileEntityUsingShippingProfileID);
			return toReturn;
		}

		#region Class Property Declarations


		/// <summary>Returns a new IEntityRelation object, between PostalProfileEntity and EndiciaProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// PostalProfile.ShippingProfileID - EndiciaProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation EndiciaProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "Endicia", true);

				relation.AddEntityFieldPair(PostalProfileFields.ShippingProfileID, EndiciaProfileFields.ShippingProfileID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PostalProfileEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EndiciaProfileEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between PostalProfileEntity and ShippingProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// PostalProfile.ShippingProfileID - ShippingProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation ShippingProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "Profile", false);



				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, PostalProfileFields.ShippingProfileID);

				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PostalProfileEntity", true);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between PostalProfileEntity and UspsProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// PostalProfile.ShippingProfileID - UspsProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation UspsProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "Usps", true);

				relation.AddEntityFieldPair(PostalProfileFields.ShippingProfileID, UspsProfileFields.ShippingProfileID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PostalProfileEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UspsProfileEntity", false);
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
	internal static class StaticPostalProfileRelations
	{
		internal static readonly IEntityRelation EndiciaProfileEntityUsingShippingProfileIDStatic = new PostalProfileRelations().EndiciaProfileEntityUsingShippingProfileID;
		internal static readonly IEntityRelation ShippingProfileEntityUsingShippingProfileIDStatic = new PostalProfileRelations().ShippingProfileEntityUsingShippingProfileID;
		internal static readonly IEntityRelation UspsProfileEntityUsingShippingProfileIDStatic = new PostalProfileRelations().UspsProfileEntityUsingShippingProfileID;

		/// <summary>CTor</summary>
		static StaticPostalProfileRelations()
		{
		}
	}
}
