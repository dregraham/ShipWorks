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
	/// <summary>Implements the relations factory for the entity: UpsShipment. </summary>
	public partial class UpsShipmentRelations
	{
		/// <summary>CTor</summary>
		public UpsShipmentRelations()
		{
		}

		/// <summary>Gets all relations of the UpsShipmentEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.UpsPackageEntityUsingShipmentID);
			toReturn.Add(this.ShipmentEntityUsingShipmentID);
			toReturn.Add(this.WorldShipShipmentEntityUsingShipmentID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between UpsShipmentEntity and UpsPackageEntity over the 1:n relation they have, using the relation between the fields:
		/// UpsShipment.ShipmentID - UpsPackage.ShipmentID
		/// </summary>
		public virtual IEntityRelation UpsPackageEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Packages" , true);
				relation.AddEntityFieldPair(UpsShipmentFields.ShipmentID, UpsPackageFields.ShipmentID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsPackageEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UpsShipmentEntity and ShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// UpsShipment.ShipmentID - Shipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation ShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "Shipment", false);



				relation.AddEntityFieldPair(ShipmentFields.ShipmentID, UpsShipmentFields.ShipmentID);

				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsShipmentEntity", true);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UpsShipmentEntity and WorldShipShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// UpsShipment.ShipmentID - WorldShipShipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation WorldShipShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "", true);

				relation.AddEntityFieldPair(UpsShipmentFields.ShipmentID, WorldShipShipmentFields.ShipmentID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("WorldShipShipmentEntity", false);
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
	internal static class StaticUpsShipmentRelations
	{
		internal static readonly IEntityRelation UpsPackageEntityUsingShipmentIDStatic = new UpsShipmentRelations().UpsPackageEntityUsingShipmentID;
		internal static readonly IEntityRelation ShipmentEntityUsingShipmentIDStatic = new UpsShipmentRelations().ShipmentEntityUsingShipmentID;
		internal static readonly IEntityRelation WorldShipShipmentEntityUsingShipmentIDStatic = new UpsShipmentRelations().WorldShipShipmentEntityUsingShipmentID;

		/// <summary>CTor</summary>
		static StaticUpsShipmentRelations()
		{
		}
	}
}
