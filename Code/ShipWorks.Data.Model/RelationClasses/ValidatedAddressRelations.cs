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
	/// <summary>Implements the static Relations variant for the entity: ValidatedAddress. </summary>
	public partial class ValidatedAddressRelations
	{
		/// <summary>CTor</summary>
		public ValidatedAddressRelations()
		{
		}

		/// <summary>Gets all relations of the ValidatedAddressEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();


			toReturn.Add(this.OrderEntityUsingConsumerID);
			toReturn.Add(this.ShipmentEntityUsingConsumerID);
			return toReturn;
		}

		#region Class Property Declarations



		/// <summary>Returns a new IEntityRelation object, between ValidatedAddressEntity and OrderEntity over the m:1 relation they have, using the relation between the fields:
		/// ValidatedAddress.ConsumerID - Order.OrderID
		/// </summary>
		public virtual IEntityRelation OrderEntityUsingConsumerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Order", false);
				relation.AddEntityFieldPair(OrderFields.OrderID, ValidatedAddressFields.ConsumerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ValidatedAddressEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between ValidatedAddressEntity and ShipmentEntity over the m:1 relation they have, using the relation between the fields:
		/// ValidatedAddress.ConsumerID - Shipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation ShipmentEntityUsingConsumerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Shipment", false);
				relation.AddEntityFieldPair(ShipmentFields.ShipmentID, ValidatedAddressFields.ConsumerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ValidatedAddressEntity", true);
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
