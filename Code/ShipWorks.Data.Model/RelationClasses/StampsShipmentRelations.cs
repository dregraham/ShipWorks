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
	/// <summary>Implements the static Relations variant for the entity: StampsShipment. </summary>
	public partial class StampsShipmentRelations
	{
		/// <summary>CTor</summary>
		public StampsShipmentRelations()
		{
		}

		/// <summary>Gets all relations of the StampsShipmentEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();

			toReturn.Add(this.PostalShipmentEntityUsingShipmentID);
			toReturn.Add(this.ScanFormBatchEntityUsingScanFormBatchID);
			return toReturn;
		}

		#region Class Property Declarations


		/// <summary>Returns a new IEntityRelation object, between StampsShipmentEntity and PostalShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// StampsShipment.ShipmentID - PostalShipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation PostalShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "PostalShipment", false);



				relation.AddEntityFieldPair(PostalShipmentFields.ShipmentID, UspsShipmentFields.ShipmentID);

				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PostalShipmentEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StampsShipmentEntity", true);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between StampsShipmentEntity and ScanFormBatchEntity over the m:1 relation they have, using the relation between the fields:
		/// StampsShipment.ScanFormBatchID - ScanFormBatch.ScanFormBatchID
		/// </summary>
		public virtual IEntityRelation ScanFormBatchEntityUsingScanFormBatchID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "ScanFormBatch", false);
                relation.AddEntityFieldPair(ScanFormBatchFields.ScanFormBatchID, UspsShipmentFields.ScanFormBatchID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ScanFormBatchEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("StampsShipmentEntity", true);
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
