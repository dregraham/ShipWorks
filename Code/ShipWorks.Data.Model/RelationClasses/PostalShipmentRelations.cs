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
	/// <summary>Implements the static Relations variant for the entity: PostalShipment. </summary>
	public partial class PostalShipmentRelations
	{
		/// <summary>CTor</summary>
		public PostalShipmentRelations()
		{
		}

		/// <summary>Gets all relations of the PostalShipmentEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();

			toReturn.Add(this.EndiciaShipmentEntityUsingShipmentID);
			toReturn.Add(this.ShipmentEntityUsingShipmentID);
			toReturn.Add(this.UspsShipmentEntityUsingShipmentID);

			return toReturn;
		}

		#region Class Property Declarations


		/// <summary>Returns a new IEntityRelation object, between PostalShipmentEntity and EndiciaShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// PostalShipment.ShipmentID - EndiciaShipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation EndiciaShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "Endicia", true);

				relation.AddEntityFieldPair(PostalShipmentFields.ShipmentID, EndiciaShipmentFields.ShipmentID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PostalShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EndiciaShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between PostalShipmentEntity and ShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// PostalShipment.ShipmentID - Shipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation ShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "Shipment", false);



				relation.AddEntityFieldPair(ShipmentFields.ShipmentID, PostalShipmentFields.ShipmentID);

				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PostalShipmentEntity", true);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between PostalShipmentEntity and UspsShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// PostalShipment.ShipmentID - UspsShipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation UspsShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "Usps", true);

				relation.AddEntityFieldPair(PostalShipmentFields.ShipmentID, UspsShipmentFields.ShipmentID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PostalShipmentEntity", true);
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
