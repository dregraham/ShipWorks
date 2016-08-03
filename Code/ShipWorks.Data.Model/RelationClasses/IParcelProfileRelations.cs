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
	/// <summary>Implements the relations factory for the entity: IParcelProfile. </summary>
	public partial class IParcelProfileRelations
	{
		/// <summary>CTor</summary>
		public IParcelProfileRelations()
		{
		}

		/// <summary>Gets all relations of the IParcelProfileEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.IParcelProfilePackageEntityUsingShippingProfileID);
			toReturn.Add(this.ShippingProfileEntityUsingShippingProfileID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between IParcelProfileEntity and IParcelProfilePackageEntity over the 1:n relation they have, using the relation between the fields:
		/// IParcelProfile.ShippingProfileID - IParcelProfilePackage.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation IParcelProfilePackageEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Packages" , true);
				relation.AddEntityFieldPair(IParcelProfileFields.ShippingProfileID, IParcelProfilePackageFields.ShippingProfileID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("IParcelProfileEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("IParcelProfilePackageEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between IParcelProfileEntity and ShippingProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// IParcelProfile.ShippingProfileID - ShippingProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation ShippingProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "ShippingProfile", false);



				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, IParcelProfileFields.ShippingProfileID);

				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("IParcelProfileEntity", true);
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
	internal static class StaticIParcelProfileRelations
	{
		internal static readonly IEntityRelation IParcelProfilePackageEntityUsingShippingProfileIDStatic = new IParcelProfileRelations().IParcelProfilePackageEntityUsingShippingProfileID;
		internal static readonly IEntityRelation ShippingProfileEntityUsingShippingProfileIDStatic = new IParcelProfileRelations().ShippingProfileEntityUsingShippingProfileID;

		/// <summary>CTor</summary>
		static StaticIParcelProfileRelations()
		{
		}
	}
}
