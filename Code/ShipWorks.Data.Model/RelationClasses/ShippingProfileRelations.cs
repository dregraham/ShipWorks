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
	/// <summary>Implements the static Relations variant for the entity: ShippingProfile. </summary>
	public partial class ShippingProfileRelations
	{
		/// <summary>CTor</summary>
		public ShippingProfileRelations()
		{
		}

		/// <summary>Gets all relations of the ShippingProfileEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();

			toReturn.Add(this.BestRateProfileEntityUsingShippingProfileID);
			toReturn.Add(this.FedExProfileEntityUsingShippingProfileID);
			toReturn.Add(this.IParcelProfileEntityUsingShippingProfileID);
			toReturn.Add(this.OnTracProfileEntityUsingShippingProfileID);
			toReturn.Add(this.OtherProfileEntityUsingShippingProfileID);
			toReturn.Add(this.PostalProfileEntityUsingShippingProfileID);
			toReturn.Add(this.UpsProfileEntityUsingShippingProfileID);

			return toReturn;
		}

		#region Class Property Declarations


		/// <summary>Returns a new IEntityRelation object, between ShippingProfileEntity and BestRateProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// ShippingProfile.ShippingProfileID - BestRateProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation BestRateProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "BestRate", true);

				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, BestRateProfileFields.ShippingProfileID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("BestRateProfileEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShippingProfileEntity and FedExProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// ShippingProfile.ShippingProfileID - FedExProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation FedExProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "FedEx", true);

				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, FedExProfileFields.ShippingProfileID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FedExProfileEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShippingProfileEntity and IParcelProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// ShippingProfile.ShippingProfileID - IParcelProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation IParcelProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "IParcel", true);

				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, IParcelProfileFields.ShippingProfileID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("IParcelProfileEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShippingProfileEntity and OnTracProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// ShippingProfile.ShippingProfileID - OnTracProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation OnTracProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "OnTrac", true);

				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, OnTracProfileFields.ShippingProfileID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OnTracProfileEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShippingProfileEntity and OtherProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// ShippingProfile.ShippingProfileID - OtherProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation OtherProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "Other", true);

				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, OtherProfileFields.ShippingProfileID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OtherProfileEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShippingProfileEntity and PostalProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// ShippingProfile.ShippingProfileID - PostalProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation PostalProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "Postal", true);

				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, PostalProfileFields.ShippingProfileID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PostalProfileEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShippingProfileEntity and UpsProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// ShippingProfile.ShippingProfileID - UpsProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation UpsProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "Ups", true);

				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, UpsProfileFields.ShippingProfileID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsProfileEntity", false);
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
