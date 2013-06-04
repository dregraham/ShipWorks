﻿///////////////////////////////////////////////////////////////
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
	/// <summary>Implements the static Relations variant for the entity: WorldShipShipment. </summary>
	public partial class WorldShipShipmentRelations
	{
		/// <summary>CTor</summary>
		public WorldShipShipmentRelations()
		{
		}

		/// <summary>Gets all relations of the WorldShipShipmentEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.WorldShipGoodsEntityUsingShipmentID);
			toReturn.Add(this.WorldShipPackageEntityUsingShipmentID);
			toReturn.Add(this.WorldShipProcessedEntityUsingShipmentID);
			toReturn.Add(this.UpsShipmentEntityUsingShipmentID);

			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between WorldShipShipmentEntity and WorldShipGoodsEntity over the 1:n relation they have, using the relation between the fields:
		/// WorldShipShipment.ShipmentID - WorldShipGoods.ShipmentID
		/// </summary>
		public virtual IEntityRelation WorldShipGoodsEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Goods" , true);
				relation.AddEntityFieldPair(WorldShipShipmentFields.ShipmentID, WorldShipGoodsFields.ShipmentID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("WorldShipShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("WorldShipGoodsEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between WorldShipShipmentEntity and WorldShipPackageEntity over the 1:n relation they have, using the relation between the fields:
		/// WorldShipShipment.ShipmentID - WorldShipPackage.ShipmentID
		/// </summary>
		public virtual IEntityRelation WorldShipPackageEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "Packages" , true);
				relation.AddEntityFieldPair(WorldShipShipmentFields.ShipmentID, WorldShipPackageFields.ShipmentID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("WorldShipShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("WorldShipPackageEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between WorldShipShipmentEntity and WorldShipProcessedEntity over the 1:n relation they have, using the relation between the fields:
		/// WorldShipShipment.ShipmentID - WorldShipProcessed.ShipmentID
		/// </summary>
		public virtual IEntityRelation WorldShipProcessedEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "WorldShipProcessed" , true);
				relation.AddEntityFieldPair(WorldShipShipmentFields.ShipmentID, WorldShipProcessedFields.ShipmentID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("WorldShipShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("WorldShipProcessedEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between WorldShipShipmentEntity and UpsShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// WorldShipShipment.ShipmentID - UpsShipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation UpsShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "", false);



				relation.AddEntityFieldPair(UpsShipmentFields.ShipmentID, WorldShipShipmentFields.ShipmentID);

				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsShipmentEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("WorldShipShipmentEntity", true);
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
