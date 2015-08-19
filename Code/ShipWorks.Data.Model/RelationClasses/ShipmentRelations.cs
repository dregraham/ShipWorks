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
	/// <summary>Implements the static Relations variant for the entity: Shipment. </summary>
	public partial class ShipmentRelations
	{
		/// <summary>CTor</summary>
		public ShipmentRelations()
		{
		}

		/// <summary>Gets all relations of the ShipmentEntity as a list of IEntityRelation objects.</summary>
		/// <returns>a list of IEntityRelation objects</returns>
		public virtual List<IEntityRelation> GetAllRelations()
		{
			List<IEntityRelation> toReturn = new List<IEntityRelation>();
			toReturn.Add(this.ShipmentCustomsItemEntityUsingShipmentID);
			toReturn.Add(this.ValidatedAddressEntityUsingConsumerID);
			toReturn.Add(this.AmazonShipmentEntityUsingShipmentID);
			toReturn.Add(this.BestRateShipmentEntityUsingShipmentID);
			toReturn.Add(this.FedExShipmentEntityUsingShipmentID);
			toReturn.Add(this.InsurancePolicyEntityUsingShipmentID);
			toReturn.Add(this.IParcelShipmentEntityUsingShipmentID);
			toReturn.Add(this.OnTracShipmentEntityUsingShipmentID);
			toReturn.Add(this.OtherShipmentEntityUsingShipmentID);
			toReturn.Add(this.PostalShipmentEntityUsingShipmentID);
			toReturn.Add(this.UpsShipmentEntityUsingShipmentID);
			toReturn.Add(this.ComputerEntityUsingVoidedComputerID);
			toReturn.Add(this.ComputerEntityUsingProcessedComputerID);
			toReturn.Add(this.OrderEntityUsingOrderID);
			toReturn.Add(this.UserEntityUsingVoidedUserID);
			toReturn.Add(this.UserEntityUsingProcessedUserID);
			return toReturn;
		}

		#region Class Property Declarations

		/// <summary>Returns a new IEntityRelation object, between ShipmentEntity and ShipmentCustomsItemEntity over the 1:n relation they have, using the relation between the fields:
		/// Shipment.ShipmentID - ShipmentCustomsItem.ShipmentID
		/// </summary>
		public virtual IEntityRelation ShipmentCustomsItemEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "CustomsItems" , true);
				relation.AddEntityFieldPair(ShipmentFields.ShipmentID, ShipmentCustomsItemFields.ShipmentID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentCustomsItemEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShipmentEntity and ValidatedAddressEntity over the 1:n relation they have, using the relation between the fields:
		/// Shipment.ShipmentID - ValidatedAddress.ConsumerID
		/// </summary>
		public virtual IEntityRelation ValidatedAddressEntityUsingConsumerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany, "ValidatedAddress" , true);
				relation.AddEntityFieldPair(ShipmentFields.ShipmentID, ValidatedAddressFields.ConsumerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ValidatedAddressEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShipmentEntity and AmazonShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// Shipment.ShipmentID - AmazonShipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation AmazonShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "AmazonShipment", true);

				relation.AddEntityFieldPair(ShipmentFields.ShipmentID, AmazonShipmentFields.ShipmentID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("AmazonShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShipmentEntity and BestRateShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// Shipment.ShipmentID - BestRateShipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation BestRateShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "BestRate", true);

				relation.AddEntityFieldPair(ShipmentFields.ShipmentID, BestRateShipmentFields.ShipmentID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("BestRateShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShipmentEntity and FedExShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// Shipment.ShipmentID - FedExShipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation FedExShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "FedEx", true);

				relation.AddEntityFieldPair(ShipmentFields.ShipmentID, FedExShipmentFields.ShipmentID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("FedExShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShipmentEntity and InsurancePolicyEntity over the 1:1 relation they have, using the relation between the fields:
		/// Shipment.ShipmentID - InsurancePolicy.ShipmentID
		/// </summary>
		public virtual IEntityRelation InsurancePolicyEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "InsurancePolicy", true);

				relation.AddEntityFieldPair(ShipmentFields.ShipmentID, InsurancePolicyFields.ShipmentID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("InsurancePolicyEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShipmentEntity and IParcelShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// Shipment.ShipmentID - IParcelShipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation IParcelShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "IParcel", true);

				relation.AddEntityFieldPair(ShipmentFields.ShipmentID, IParcelShipmentFields.ShipmentID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("IParcelShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShipmentEntity and OnTracShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// Shipment.ShipmentID - OnTracShipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation OnTracShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "OnTrac", true);

				relation.AddEntityFieldPair(ShipmentFields.ShipmentID, OnTracShipmentFields.ShipmentID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OnTracShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShipmentEntity and OtherShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// Shipment.ShipmentID - OtherShipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation OtherShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "Other", true);

				relation.AddEntityFieldPair(ShipmentFields.ShipmentID, OtherShipmentFields.ShipmentID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OtherShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShipmentEntity and PostalShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// Shipment.ShipmentID - PostalShipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation PostalShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "Postal", true);

				relation.AddEntityFieldPair(ShipmentFields.ShipmentID, PostalShipmentFields.ShipmentID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("PostalShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShipmentEntity and UpsShipmentEntity over the 1:1 relation they have, using the relation between the fields:
		/// Shipment.ShipmentID - UpsShipment.ShipmentID
		/// </summary>
		public virtual IEntityRelation UpsShipmentEntityUsingShipmentID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne, "Ups", true);

				relation.AddEntityFieldPair(ShipmentFields.ShipmentID, UpsShipmentFields.ShipmentID);



				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", true);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UpsShipmentEntity", false);
				return relation;
			}
		}

		/// <summary>Returns a new IEntityRelation object, between ShipmentEntity and ComputerEntity over the m:1 relation they have, using the relation between the fields:
		/// Shipment.VoidedComputerID - Computer.ComputerID
		/// </summary>
		public virtual IEntityRelation ComputerEntityUsingVoidedComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "", false);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, ShipmentFields.VoidedComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between ShipmentEntity and ComputerEntity over the m:1 relation they have, using the relation between the fields:
		/// Shipment.ProcessedComputerID - Computer.ComputerID
		/// </summary>
		public virtual IEntityRelation ComputerEntityUsingProcessedComputerID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "", false);
				relation.AddEntityFieldPair(ComputerFields.ComputerID, ShipmentFields.ProcessedComputerID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ComputerEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between ShipmentEntity and OrderEntity over the m:1 relation they have, using the relation between the fields:
		/// Shipment.OrderID - Order.OrderID
		/// </summary>
		public virtual IEntityRelation OrderEntityUsingOrderID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "Order", false);
				relation.AddEntityFieldPair(OrderFields.OrderID, ShipmentFields.OrderID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("OrderEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between ShipmentEntity and UserEntity over the m:1 relation they have, using the relation between the fields:
		/// Shipment.VoidedUserID - User.UserID
		/// </summary>
		public virtual IEntityRelation UserEntityUsingVoidedUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "", false);
				relation.AddEntityFieldPair(UserFields.UserID, ShipmentFields.VoidedUserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", true);
				return relation;
			}
		}
		/// <summary>Returns a new IEntityRelation object, between ShipmentEntity and UserEntity over the m:1 relation they have, using the relation between the fields:
		/// Shipment.ProcessedUserID - User.UserID
		/// </summary>
		public virtual IEntityRelation UserEntityUsingProcessedUserID
		{
			get
			{
				IEntityRelation relation = new EntityRelation(SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne, "", false);
				relation.AddEntityFieldPair(UserFields.UserID, ShipmentFields.ProcessedUserID);
				relation.InheritanceInfoPkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("UserEntity", false);
				relation.InheritanceInfoFkSideEntity = InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("ShipmentEntity", true);
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
