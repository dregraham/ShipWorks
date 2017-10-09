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
	/// <summary>Implements the relations factory for the entity: DhlExpressShipment. </summary>
	public partial class DhlExpressShipmentRelations
	{
		/// <summary>CTor</summary>
		public DhlExpressShipmentRelations()
		{
		}

		/// <summary>Gets all relations of the DhlExpressShipmentEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.DhlExpressPackageEntityUsingShipmentID);
			toReturn.Add(this.ShipmentEntityUsingShipmentID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between DhlExpressShipmentEntity and DhlExpressPackageEntity over the 1:n relation they have, using the relation between the fields:
		/// DhlExpressShipment.ShipmentID - DhlExpressPackage.ShipmentID
		/// </summary>
		public virtual IEntityRelation DhlExpressPackageEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Packages" , true);
				relation.AddEntityFieldPair(DhlExpressShipmentFields.ShipmentID, DhlExpressPackageFields.ShipmentID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DhlExpressShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DhlExpressPackageEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between DhlExpressShipmentEntity and ShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// DhlExpressShipment.ShipmentID - Shipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation ShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "Shipment", false);



				relation.AddEntityFieldPair(ShipmentFields.ShipmentID, DhlExpressShipmentFields.ShipmentID);

				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("DhlExpressShipmentEntity", true);
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
	internal static class StaticDhlExpressShipmentRelations
	{
		internal static readonly IEntityRelation DhlExpressPackageEntityUsingShipmentIDStatic = new DhlExpressShipmentRelations().DhlExpressPackageEntityUsingShipmentID;
		internal static readonly IEntityRelation ShipmentEntityUsingShipmentIDStatic = new DhlExpressShipmentRelations().ShipmentEntityUsingShipmentID;

		/// <summary>CTor</summary>
		static StaticDhlExpressShipmentRelations()
		{
		}
	}
}
