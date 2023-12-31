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
	/// <summary>Implements the relations factory for the entity: ShippingProfile. </summary>
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
			toReturn.Add(this.PackageProfileEntityUsingShippingProfileID);
			toReturn.Add(this.AmazonSFPProfileEntityUsingShippingProfileID);
			toReturn.Add(this.AmazonSWAProfileEntityUsingShippingProfileID);
			toReturn.Add(this.AsendiaProfileEntityUsingShippingProfileID);
			toReturn.Add(this.BestRateProfileEntityUsingShippingProfileID);
			toReturn.Add(this.DhlEcommerceProfileEntityUsingShippingProfileID);
			toReturn.Add(this.DhlExpressProfileEntityUsingShippingProfileID);
			toReturn.Add(this.FedExProfileEntityUsingShippingProfileID);
			toReturn.Add(this.IParcelProfileEntityUsingShippingProfileID);
			toReturn.Add(this.OnTracProfileEntityUsingShippingProfileID);
			toReturn.Add(this.OtherProfileEntityUsingShippingProfileID);
			toReturn.Add(this.PostalProfileEntityUsingShippingProfileID);
			toReturn.Add(this.UpsProfileEntityUsingShippingProfileID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between ShippingProfileEntity and PackageProfileEntity over the 1:n relation they have, using the relation between the fields:
		/// ShippingProfile.ShippingProfileID - PackageProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation PackageProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Packages" , true);
				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, PackageProfileFields.ShippingProfileID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PackageProfileEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShippingProfileEntity and AmazonSFPProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// ShippingProfile.ShippingProfileID - AmazonSFPProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation AmazonSFPProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "AmazonSFP", true);

				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, AmazonSFPProfileFields.ShippingProfileID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AmazonSFPProfileEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShippingProfileEntity and AmazonSWAProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// ShippingProfile.ShippingProfileID - AmazonSWAProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation AmazonSWAProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "AmazonSWA", true);

				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, AmazonSWAProfileFields.ShippingProfileID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AmazonSWAProfileEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShippingProfileEntity and AsendiaProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// ShippingProfile.ShippingProfileID - AsendiaProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation AsendiaProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "Asendia", true);

				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, AsendiaProfileFields.ShippingProfileID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AsendiaProfileEntity", false);
				return relation;
			}
		}

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

		/// <summary>Returns a new IEntityRelation object, between ShippingProfileEntity and DhlEcommerceProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// ShippingProfile.ShippingProfileID - DhlEcommerceProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation DhlEcommerceProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "DhlEcommerce", true);

				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, DhlEcommerceProfileFields.ShippingProfileID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DhlEcommerceProfileEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShippingProfileEntity and DhlExpressProfileEntity over the 1:1 relation they have, using the relation between the fields:
		/// ShippingProfile.ShippingProfileID - DhlExpressProfile.ShippingProfileID
		/// </summary>
		public virtual IEntityRelation DhlExpressProfileEntityUsingShippingProfileID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "DhlExpress", true);

				relation.AddEntityFieldPair(ShippingProfileFields.ShippingProfileID, DhlExpressProfileFields.ShippingProfileID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShippingProfileEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DhlExpressProfileEntity", false);
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
	
	/// <summary>Static class which is used for providing relationship instances which are re-used internally for syncing</summary>
	internal static class StaticShippingProfileRelations
	{
		internal static readonly IEntityRelation PackageProfileEntityUsingShippingProfileIDStatic = new ShippingProfileRelations().PackageProfileEntityUsingShippingProfileID;
		internal static readonly IEntityRelation AmazonSFPProfileEntityUsingShippingProfileIDStatic = new ShippingProfileRelations().AmazonSFPProfileEntityUsingShippingProfileID;
		internal static readonly IEntityRelation AmazonSWAProfileEntityUsingShippingProfileIDStatic = new ShippingProfileRelations().AmazonSWAProfileEntityUsingShippingProfileID;
		internal static readonly IEntityRelation AsendiaProfileEntityUsingShippingProfileIDStatic = new ShippingProfileRelations().AsendiaProfileEntityUsingShippingProfileID;
		internal static readonly IEntityRelation BestRateProfileEntityUsingShippingProfileIDStatic = new ShippingProfileRelations().BestRateProfileEntityUsingShippingProfileID;
		internal static readonly IEntityRelation DhlEcommerceProfileEntityUsingShippingProfileIDStatic = new ShippingProfileRelations().DhlEcommerceProfileEntityUsingShippingProfileID;
		internal static readonly IEntityRelation DhlExpressProfileEntityUsingShippingProfileIDStatic = new ShippingProfileRelations().DhlExpressProfileEntityUsingShippingProfileID;
		internal static readonly IEntityRelation FedExProfileEntityUsingShippingProfileIDStatic = new ShippingProfileRelations().FedExProfileEntityUsingShippingProfileID;
		internal static readonly IEntityRelation IParcelProfileEntityUsingShippingProfileIDStatic = new ShippingProfileRelations().IParcelProfileEntityUsingShippingProfileID;
		internal static readonly IEntityRelation OnTracProfileEntityUsingShippingProfileIDStatic = new ShippingProfileRelations().OnTracProfileEntityUsingShippingProfileID;
		internal static readonly IEntityRelation OtherProfileEntityUsingShippingProfileIDStatic = new ShippingProfileRelations().OtherProfileEntityUsingShippingProfileID;
		internal static readonly IEntityRelation PostalProfileEntityUsingShippingProfileIDStatic = new ShippingProfileRelations().PostalProfileEntityUsingShippingProfileID;
		internal static readonly IEntityRelation UpsProfileEntityUsingShippingProfileIDStatic = new ShippingProfileRelations().UpsProfileEntityUsingShippingProfileID;

		/// <summary>CTor</summary>
		static StaticShippingProfileRelations()
		{
		}
	}
}
