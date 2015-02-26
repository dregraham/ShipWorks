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
	/// <summary>Implements the static Relations variant for the entity: ScanFormBatch. </summary>
	public partial class ScanFormBatchRelations
	{
		/// <summary>CTor</summary>
		public ScanFormBatchRelations()
		{
		}

		/// <summary>Gets all relations of the ScanFormBatchEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.EndiciaScanFormEntityUsingScanFormBatchID);
			toReturn.Add(this.EndiciaShipmentEntityUsingScanFormBatchID);
			toReturn.Add(this.UspsScanFormEntityUsingScanFormBatchID);
			toReturn.Add(this.UspsShipmentEntityUsingScanFormBatchID);


			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between ScanFormBatchEntity and EndiciaScanFormEntity over the 1:n relation they have, using the relation between the fields:
		/// ScanFormBatch.ScanFormBatchID - EndiciaScanForm.ScanFormBatchID
		/// </summary>
		public virtual IEntityRelation EndiciaScanFormEntityUsingScanFormBatchID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "EndiciaScanForms" , true);
				relation.AddEntityFieldPair(ScanFormBatchFields.ScanFormBatchID, EndiciaScanFormFields.ScanFormBatchID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ScanFormBatchEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EndiciaScanFormEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ScanFormBatchEntity and EndiciaShipmentEntity over the 1:n relation they have, using the relation between the fields:
		/// ScanFormBatch.ScanFormBatchID - EndiciaShipment.ScanFormBatchID
		/// </summary>
		public virtual IEntityRelation EndiciaShipmentEntityUsingScanFormBatchID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "EndiciaShipment" , true);
				relation.AddEntityFieldPair(ScanFormBatchFields.ScanFormBatchID, EndiciaShipmentFields.ScanFormBatchID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ScanFormBatchEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EndiciaShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ScanFormBatchEntity and UspsScanFormEntity over the 1:n relation they have, using the relation between the fields:
		/// ScanFormBatch.ScanFormBatchID - UspsScanForm.ScanFormBatchID
		/// </summary>
		public virtual IEntityRelation UspsScanFormEntityUsingScanFormBatchID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "UspsScanForms" , true);
				relation.AddEntityFieldPair(ScanFormBatchFields.ScanFormBatchID, UspsScanFormFields.ScanFormBatchID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ScanFormBatchEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UspsScanFormEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ScanFormBatchEntity and UspsShipmentEntity over the 1:n relation they have, using the relation between the fields:
		/// ScanFormBatch.ScanFormBatchID - UspsShipment.ScanFormBatchID
		/// </summary>
		public virtual IEntityRelation UspsShipmentEntityUsingScanFormBatchID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "UspsShipment" , true);
				relation.AddEntityFieldPair(ScanFormBatchFields.ScanFormBatchID, UspsShipmentFields.ScanFormBatchID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ScanFormBatchEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UspsShipmentEntity", false);
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
