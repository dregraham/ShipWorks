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
	/// <summary>Implements the relations factory for the entity: UspsShipment. </summary>
	public partial class UspsShipmentRelations
	{
		/// <summary>CTor</summary>
		public UspsShipmentRelations()
		{
		}

		/// <summary>Gets all relations of the UspsShipmentEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.PostalShipmentEntityUsingShipmentID);
			toReturn.Add(this.ScanFormBatchEntityUsingScanFormBatchID);
			return toReturn;
		}

		#region Class Property Declarations


		/// <summary>Returns a new IEntityRelation object, between UspsShipmentEntity and PostalShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// UspsShipment.ShipmentID - PostalShipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation PostalShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "PostalShipment", false);



				relation.AddEntityFieldPair(PostalShipmentFields.ShipmentID, UspsShipmentFields.ShipmentID);

				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PostalShipmentEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UspsShipmentEntity", true);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between UspsShipmentEntity and ScanFormBatchEntity over the m:1 relation they have, using the relation between the fields:
		/// UspsShipment.ScanFormBatchID - ScanFormBatch.ScanFormBatchID
		/// </summary>
		public virtual IEntityRelation ScanFormBatchEntityUsingScanFormBatchID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "ScanFormBatch", false);
				relation.AddEntityFieldPair(ScanFormBatchFields.ScanFormBatchID, UspsShipmentFields.ScanFormBatchID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ScanFormBatchEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UspsShipmentEntity", true);
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
	internal static class StaticUspsShipmentRelations
	{
		internal static readonly IEntityRelation PostalShipmentEntityUsingShipmentIDStatic = new UspsShipmentRelations().PostalShipmentEntityUsingShipmentID;
		internal static readonly IEntityRelation ScanFormBatchEntityUsingScanFormBatchIDStatic = new UspsShipmentRelations().ScanFormBatchEntityUsingScanFormBatchID;

		/// <summary>CTor</summary>
		static StaticUspsShipmentRelations()
		{
		}
	}
}
