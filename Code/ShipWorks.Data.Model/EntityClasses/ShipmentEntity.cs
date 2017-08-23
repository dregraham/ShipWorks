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
using System.ComponentModel;
using System.Collections.Generic;
#if !CF
using System.Runtime.Serialization;
#endif
using System.Xml.Serialization;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.RelationClasses;

using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.EntityClasses
{
	// __LLBLGENPRO_USER_CODE_REGION_START AdditionalNamespaces
	// __LLBLGENPRO_USER_CODE_REGION_END
	/// <summary>Entity class which represents the entity 'Shipment'.<br/><br/></summary>
	[Serializable]
	public partial class ShipmentEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<ShipmentCustomsItemEntity> _customsItems;
		private EntityCollection<ShipmentReturnItemEntity> _returnItems;
		private EntityCollection<ValidatedAddressEntity> _validatedAddress;
		private EntityCollection<OrderEntity> _orderCollectionViaValidatedAddress;
		private OrderEntity _order;
		private AmazonShipmentEntity _amazon;
		private BestRateShipmentEntity _bestRate;
		private FedExShipmentEntity _fedEx;
		private InsurancePolicyEntity _insurancePolicy;
		private IParcelShipmentEntity _iParcel;
		private OnTracShipmentEntity _onTrac;
		private OtherShipmentEntity _other;
		private PostalShipmentEntity _postal;
		private UpsShipmentEntity _ups;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name Order</summary>
			public static readonly string Order = "Order";
			/// <summary>Member name CustomsItems</summary>
			public static readonly string CustomsItems = "CustomsItems";
			/// <summary>Member name ReturnItems</summary>
			public static readonly string ReturnItems = "ReturnItems";
			/// <summary>Member name ValidatedAddress</summary>
			public static readonly string ValidatedAddress = "ValidatedAddress";
			/// <summary>Member name OrderCollectionViaValidatedAddress</summary>
			public static readonly string OrderCollectionViaValidatedAddress = "OrderCollectionViaValidatedAddress";
			/// <summary>Member name Amazon</summary>
			public static readonly string Amazon = "Amazon";
			/// <summary>Member name BestRate</summary>
			public static readonly string BestRate = "BestRate";
			/// <summary>Member name FedEx</summary>
			public static readonly string FedEx = "FedEx";
			/// <summary>Member name InsurancePolicy</summary>
			public static readonly string InsurancePolicy = "InsurancePolicy";
			/// <summary>Member name IParcel</summary>
			public static readonly string IParcel = "IParcel";
			/// <summary>Member name OnTrac</summary>
			public static readonly string OnTrac = "OnTrac";
			/// <summary>Member name Other</summary>
			public static readonly string Other = "Other";
			/// <summary>Member name Postal</summary>
			public static readonly string Postal = "Postal";
			/// <summary>Member name Ups</summary>
			public static readonly string Ups = "Ups";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static ShipmentEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public ShipmentEntity():base("ShipmentEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public ShipmentEntity(IEntityFields2 fields):base("ShipmentEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this ShipmentEntity</param>
		public ShipmentEntity(IValidator validator):base("ShipmentEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="shipmentID">PK value for Shipment which data should be fetched into this Shipment object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ShipmentEntity(System.Int64 shipmentID):base("ShipmentEntity")
		{
			InitClassEmpty(null, null);
			this.ShipmentID = shipmentID;
		}

		/// <summary> CTor</summary>
		/// <param name="shipmentID">PK value for Shipment which data should be fetched into this Shipment object</param>
		/// <param name="validator">The custom validator object for this ShipmentEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ShipmentEntity(System.Int64 shipmentID, IValidator validator):base("ShipmentEntity")
		{
			InitClassEmpty(validator, null);
			this.ShipmentID = shipmentID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected ShipmentEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_customsItems = (EntityCollection<ShipmentCustomsItemEntity>)info.GetValue("_customsItems", typeof(EntityCollection<ShipmentCustomsItemEntity>));
				_returnItems = (EntityCollection<ShipmentReturnItemEntity>)info.GetValue("_returnItems", typeof(EntityCollection<ShipmentReturnItemEntity>));
				_validatedAddress = (EntityCollection<ValidatedAddressEntity>)info.GetValue("_validatedAddress", typeof(EntityCollection<ValidatedAddressEntity>));
				_orderCollectionViaValidatedAddress = (EntityCollection<OrderEntity>)info.GetValue("_orderCollectionViaValidatedAddress", typeof(EntityCollection<OrderEntity>));
				_order = (OrderEntity)info.GetValue("_order", typeof(OrderEntity));
				if(_order!=null)
				{
					_order.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_amazon = (AmazonShipmentEntity)info.GetValue("_amazon", typeof(AmazonShipmentEntity));
				if(_amazon!=null)
				{
					_amazon.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_bestRate = (BestRateShipmentEntity)info.GetValue("_bestRate", typeof(BestRateShipmentEntity));
				if(_bestRate!=null)
				{
					_bestRate.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_fedEx = (FedExShipmentEntity)info.GetValue("_fedEx", typeof(FedExShipmentEntity));
				if(_fedEx!=null)
				{
					_fedEx.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_insurancePolicy = (InsurancePolicyEntity)info.GetValue("_insurancePolicy", typeof(InsurancePolicyEntity));
				if(_insurancePolicy!=null)
				{
					_insurancePolicy.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_iParcel = (IParcelShipmentEntity)info.GetValue("_iParcel", typeof(IParcelShipmentEntity));
				if(_iParcel!=null)
				{
					_iParcel.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_onTrac = (OnTracShipmentEntity)info.GetValue("_onTrac", typeof(OnTracShipmentEntity));
				if(_onTrac!=null)
				{
					_onTrac.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_other = (OtherShipmentEntity)info.GetValue("_other", typeof(OtherShipmentEntity));
				if(_other!=null)
				{
					_other.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_postal = (PostalShipmentEntity)info.GetValue("_postal", typeof(PostalShipmentEntity));
				if(_postal!=null)
				{
					_postal.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_ups = (UpsShipmentEntity)info.GetValue("_ups", typeof(UpsShipmentEntity));
				if(_ups!=null)
				{
					_ups.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				this.FixupDeserialization(FieldInfoProviderSingleton.GetInstance());
			}
			// __LLBLGENPRO_USER_CODE_REGION_START DeserializationConstructor
			// __LLBLGENPRO_USER_CODE_REGION_END
		}

		
		/// <summary>Performs the desync setup when an FK field has been changed. The entity referenced based on the FK field will be dereferenced and sync info will be removed.</summary>
		/// <param name="fieldIndex">The fieldindex.</param>
		protected override void PerformDesyncSetupFKFieldChange(int fieldIndex)
		{
			switch((ShipmentFieldIndex)fieldIndex)
			{
				case ShipmentFieldIndex.OrderID:
					DesetupSyncOrder(true, false);
					break;
				case ShipmentFieldIndex.ProcessedUserID:

					break;
				case ShipmentFieldIndex.ProcessedComputerID:

					break;
				case ShipmentFieldIndex.VoidedUserID:

					break;
				case ShipmentFieldIndex.VoidedComputerID:

					break;
				default:
					base.PerformDesyncSetupFKFieldChange(fieldIndex);
					break;
			}
		}

		/// <summary> Sets the related entity property to the entity specified. If the property is a collection, it will add the entity specified to that collection.</summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="entity">Entity to set as an related entity</param>
		/// <remarks>Used by prefetch path logic.</remarks>
		protected override void SetRelatedEntityProperty(string propertyName, IEntityCore entity)
		{
			switch(propertyName)
			{
				case "Order":
					this.Order = (OrderEntity)entity;
					break;
				case "CustomsItems":
					this.CustomsItems.Add((ShipmentCustomsItemEntity)entity);
					break;
				case "ReturnItems":
					this.ReturnItems.Add((ShipmentReturnItemEntity)entity);
					break;
				case "ValidatedAddress":
					this.ValidatedAddress.Add((ValidatedAddressEntity)entity);
					break;
				case "OrderCollectionViaValidatedAddress":
					this.OrderCollectionViaValidatedAddress.IsReadOnly = false;
					this.OrderCollectionViaValidatedAddress.Add((OrderEntity)entity);
					this.OrderCollectionViaValidatedAddress.IsReadOnly = true;
					break;
				case "Amazon":
					this.Amazon = (AmazonShipmentEntity)entity;
					break;
				case "BestRate":
					this.BestRate = (BestRateShipmentEntity)entity;
					break;
				case "FedEx":
					this.FedEx = (FedExShipmentEntity)entity;
					break;
				case "InsurancePolicy":
					this.InsurancePolicy = (InsurancePolicyEntity)entity;
					break;
				case "IParcel":
					this.IParcel = (IParcelShipmentEntity)entity;
					break;
				case "OnTrac":
					this.OnTrac = (OnTracShipmentEntity)entity;
					break;
				case "Other":
					this.Other = (OtherShipmentEntity)entity;
					break;
				case "Postal":
					this.Postal = (PostalShipmentEntity)entity;
					break;
				case "Ups":
					this.Ups = (UpsShipmentEntity)entity;
					break;
				default:
					this.OnSetRelatedEntityProperty(propertyName, entity);
					break;
			}
		}
		
		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		protected override RelationCollection GetRelationsForFieldOfType(string fieldName)
		{
			return GetRelationsForField(fieldName);
		}

		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		internal static RelationCollection GetRelationsForField(string fieldName)
		{
			RelationCollection toReturn = new RelationCollection();
			switch(fieldName)
			{
				case "Order":
					toReturn.Add(Relations.OrderEntityUsingOrderID);
					break;
				case "CustomsItems":
					toReturn.Add(Relations.ShipmentCustomsItemEntityUsingShipmentID);
					break;
				case "ReturnItems":
					toReturn.Add(Relations.ShipmentReturnItemEntityUsingShipmentID);
					break;
				case "ValidatedAddress":
					toReturn.Add(Relations.ValidatedAddressEntityUsingConsumerID);
					break;
				case "OrderCollectionViaValidatedAddress":
					toReturn.Add(Relations.ValidatedAddressEntityUsingConsumerID, "ShipmentEntity__", "ValidatedAddress_", JoinHint.None);
					toReturn.Add(ValidatedAddressEntity.Relations.OrderEntityUsingConsumerID, "ValidatedAddress_", string.Empty, JoinHint.None);
					break;
				case "Amazon":
					toReturn.Add(Relations.AmazonShipmentEntityUsingShipmentID);
					break;
				case "BestRate":
					toReturn.Add(Relations.BestRateShipmentEntityUsingShipmentID);
					break;
				case "FedEx":
					toReturn.Add(Relations.FedExShipmentEntityUsingShipmentID);
					break;
				case "InsurancePolicy":
					toReturn.Add(Relations.InsurancePolicyEntityUsingShipmentID);
					break;
				case "IParcel":
					toReturn.Add(Relations.IParcelShipmentEntityUsingShipmentID);
					break;
				case "OnTrac":
					toReturn.Add(Relations.OnTracShipmentEntityUsingShipmentID);
					break;
				case "Other":
					toReturn.Add(Relations.OtherShipmentEntityUsingShipmentID);
					break;
				case "Postal":
					toReturn.Add(Relations.PostalShipmentEntityUsingShipmentID);
					break;
				case "Ups":
					toReturn.Add(Relations.UpsShipmentEntityUsingShipmentID);
					break;
				default:
					break;				
			}
			return toReturn;
		}
#if !CF
		/// <summary>Checks if the relation mapped by the property with the name specified is a one way / single sided relation. If the passed in name is null, it/ will return true if the entity has any single-sided relation</summary>
		/// <param name="propertyName">Name of the property which is mapped onto the relation to check, or null to check if the entity has any relation/ which is single sided</param>
		/// <returns>true if the relation is single sided / one way (so the opposite relation isn't present), false otherwise</returns>
		protected override bool CheckOneWayRelations(string propertyName)
		{
			int numberOfOneWayRelations = 0;
			switch(propertyName)
			{
				case null:
					return ((numberOfOneWayRelations > 0) || base.CheckOneWayRelations(null));
				default:
					return base.CheckOneWayRelations(propertyName);
			}
		}
#endif
		/// <summary> Sets the internal parameter related to the fieldname passed to the instance relatedEntity. </summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		/// <param name="fieldName">Name of field mapped onto the relation which resolves in the instance relatedEntity</param>
		protected override void SetRelatedEntity(IEntityCore relatedEntity, string fieldName)
		{
			switch(fieldName)
			{
				case "Order":
					SetupSyncOrder(relatedEntity);
					break;
				case "CustomsItems":
					this.CustomsItems.Add((ShipmentCustomsItemEntity)relatedEntity);
					break;
				case "ReturnItems":
					this.ReturnItems.Add((ShipmentReturnItemEntity)relatedEntity);
					break;
				case "ValidatedAddress":
					this.ValidatedAddress.Add((ValidatedAddressEntity)relatedEntity);
					break;
				case "Amazon":
					SetupSyncAmazon(relatedEntity);
					break;
				case "BestRate":
					SetupSyncBestRate(relatedEntity);
					break;
				case "FedEx":
					SetupSyncFedEx(relatedEntity);
					break;
				case "InsurancePolicy":
					SetupSyncInsurancePolicy(relatedEntity);
					break;
				case "IParcel":
					SetupSyncIParcel(relatedEntity);
					break;
				case "OnTrac":
					SetupSyncOnTrac(relatedEntity);
					break;
				case "Other":
					SetupSyncOther(relatedEntity);
					break;
				case "Postal":
					SetupSyncPostal(relatedEntity);
					break;
				case "Ups":
					SetupSyncUps(relatedEntity);
					break;
				default:
					break;
			}
		}

		/// <summary> Unsets the internal parameter related to the fieldname passed to the instance relatedEntity. Reverses the actions taken by SetRelatedEntity() </summary>
		/// <param name="relatedEntity">Instance to unset as the related entity of type entityType</param>
		/// <param name="fieldName">Name of field mapped onto the relation which resolves in the instance relatedEntity</param>
		/// <param name="signalRelatedEntityManyToOne">if set to true it will notify the manytoone side, if applicable.</param>
		protected override void UnsetRelatedEntity(IEntityCore relatedEntity, string fieldName, bool signalRelatedEntityManyToOne)
		{
			switch(fieldName)
			{
				case "Order":
					DesetupSyncOrder(false, true);
					break;
				case "CustomsItems":
					this.PerformRelatedEntityRemoval(this.CustomsItems, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "ReturnItems":
					this.PerformRelatedEntityRemoval(this.ReturnItems, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "ValidatedAddress":
					this.PerformRelatedEntityRemoval(this.ValidatedAddress, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "Amazon":
					DesetupSyncAmazon(false, true);
					break;
				case "BestRate":
					DesetupSyncBestRate(false, true);
					break;
				case "FedEx":
					DesetupSyncFedEx(false, true);
					break;
				case "InsurancePolicy":
					DesetupSyncInsurancePolicy(false, true);
					break;
				case "IParcel":
					DesetupSyncIParcel(false, true);
					break;
				case "OnTrac":
					DesetupSyncOnTrac(false, true);
					break;
				case "Other":
					DesetupSyncOther(false, true);
					break;
				case "Postal":
					DesetupSyncPostal(false, true);
					break;
				case "Ups":
					DesetupSyncUps(false, true);
					break;
				default:
					break;
			}
		}

		/// <summary> Gets a collection of related entities referenced by this entity which depend on this entity (this entity is the PK side of their FK fields). These entities will have to be persisted after this entity during a recursive save.</summary>
		/// <returns>Collection with 0 or more IEntity2 objects, referenced by this entity</returns>
		protected override List<IEntity2> GetDependingRelatedEntities()
		{
			List<IEntity2> toReturn = new List<IEntity2>();
			if(_amazon!=null)
			{
				toReturn.Add(_amazon);
			}

			if(_bestRate!=null)
			{
				toReturn.Add(_bestRate);
			}

			if(_fedEx!=null)
			{
				toReturn.Add(_fedEx);
			}

			if(_insurancePolicy!=null)
			{
				toReturn.Add(_insurancePolicy);
			}

			if(_iParcel!=null)
			{
				toReturn.Add(_iParcel);
			}

			if(_onTrac!=null)
			{
				toReturn.Add(_onTrac);
			}

			if(_other!=null)
			{
				toReturn.Add(_other);
			}

			if(_postal!=null)
			{
				toReturn.Add(_postal);
			}

			if(_ups!=null)
			{
				toReturn.Add(_ups);
			}

			return toReturn;
		}
		
		/// <summary> Gets a collection of related entities referenced by this entity which this entity depends on (this entity is the FK side of their PK fields). These
		/// entities will have to be persisted before this entity during a recursive save.</summary>
		/// <returns>Collection with 0 or more IEntity2 objects, referenced by this entity</returns>
		protected override List<IEntity2> GetDependentRelatedEntities()
		{
			List<IEntity2> toReturn = new List<IEntity2>();
			if(_order!=null)
			{
				toReturn.Add(_order);
			}


















			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.Add(this.CustomsItems);
			toReturn.Add(this.ReturnItems);
			toReturn.Add(this.ValidatedAddress);
			return toReturn;
		}

		/// <summary>ISerializable member. Does custom serialization so event handlers do not get serialized. Serializes members of this entity class and uses the base class' implementation to serialize the rest.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				info.AddValue("_customsItems", ((_customsItems!=null) && (_customsItems.Count>0) && !this.MarkedForDeletion)?_customsItems:null);
				info.AddValue("_returnItems", ((_returnItems!=null) && (_returnItems.Count>0) && !this.MarkedForDeletion)?_returnItems:null);
				info.AddValue("_validatedAddress", ((_validatedAddress!=null) && (_validatedAddress.Count>0) && !this.MarkedForDeletion)?_validatedAddress:null);
				info.AddValue("_orderCollectionViaValidatedAddress", ((_orderCollectionViaValidatedAddress!=null) && (_orderCollectionViaValidatedAddress.Count>0) && !this.MarkedForDeletion)?_orderCollectionViaValidatedAddress:null);
				info.AddValue("_order", (!this.MarkedForDeletion?_order:null));
				info.AddValue("_amazon", (!this.MarkedForDeletion?_amazon:null));
				info.AddValue("_bestRate", (!this.MarkedForDeletion?_bestRate:null));
				info.AddValue("_fedEx", (!this.MarkedForDeletion?_fedEx:null));
				info.AddValue("_insurancePolicy", (!this.MarkedForDeletion?_insurancePolicy:null));
				info.AddValue("_iParcel", (!this.MarkedForDeletion?_iParcel:null));
				info.AddValue("_onTrac", (!this.MarkedForDeletion?_onTrac:null));
				info.AddValue("_other", (!this.MarkedForDeletion?_other:null));
				info.AddValue("_postal", (!this.MarkedForDeletion?_postal:null));
				info.AddValue("_ups", (!this.MarkedForDeletion?_ups:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new ShipmentRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'ShipmentCustomsItem' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoCustomsItems()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ShipmentCustomsItemFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'ShipmentReturnItem' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoReturnItems()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ShipmentReturnItemFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'ValidatedAddress' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoValidatedAddress()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ValidatedAddressFields.ConsumerID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'Order' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoOrderCollectionViaValidatedAddress()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.Relations.AddRange(GetRelationsForFieldOfType("OrderCollectionViaValidatedAddress"));
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ShipmentFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID, "ShipmentEntity__"));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'Order' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoOrder()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(OrderFields.OrderID, null, ComparisonOperator.Equal, this.OrderID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'AmazonShipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoAmazon()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(AmazonShipmentFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'BestRateShipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoBestRate()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(BestRateShipmentFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'FedExShipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoFedEx()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(FedExShipmentFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'InsurancePolicy' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoInsurancePolicy()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(InsurancePolicyFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'IParcelShipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoIParcel()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(IParcelShipmentFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'OnTracShipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoOnTrac()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(OnTracShipmentFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'OtherShipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoOther()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(OtherShipmentFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'PostalShipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoPostal()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(PostalShipmentFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'UpsShipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUps()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UpsShipmentFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(ShipmentEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._customsItems);
			collectionsQueue.Enqueue(this._returnItems);
			collectionsQueue.Enqueue(this._validatedAddress);
			collectionsQueue.Enqueue(this._orderCollectionViaValidatedAddress);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._customsItems = (EntityCollection<ShipmentCustomsItemEntity>) collectionsQueue.Dequeue();
			this._returnItems = (EntityCollection<ShipmentReturnItemEntity>) collectionsQueue.Dequeue();
			this._validatedAddress = (EntityCollection<ValidatedAddressEntity>) collectionsQueue.Dequeue();
			this._orderCollectionViaValidatedAddress = (EntityCollection<OrderEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._customsItems != null);
			toReturn |=(this._returnItems != null);
			toReturn |=(this._validatedAddress != null);
			toReturn |= (this._orderCollectionViaValidatedAddress != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ShipmentCustomsItemEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ShipmentCustomsItemEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ShipmentReturnItemEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ShipmentReturnItemEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ValidatedAddressEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ValidatedAddressEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<OrderEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("Order", _order);
			toReturn.Add("CustomsItems", _customsItems);
			toReturn.Add("ReturnItems", _returnItems);
			toReturn.Add("ValidatedAddress", _validatedAddress);
			toReturn.Add("OrderCollectionViaValidatedAddress", _orderCollectionViaValidatedAddress);
			toReturn.Add("Amazon", _amazon);
			toReturn.Add("BestRate", _bestRate);
			toReturn.Add("FedEx", _fedEx);
			toReturn.Add("InsurancePolicy", _insurancePolicy);
			toReturn.Add("IParcel", _iParcel);
			toReturn.Add("OnTrac", _onTrac);
			toReturn.Add("Other", _other);
			toReturn.Add("Postal", _postal);
			toReturn.Add("Ups", _ups);
			return toReturn;
		}

		/// <summary> Initializes the class members</summary>
		private void InitClassMembers()
		{
			PerformDependencyInjection();
			
			// __LLBLGENPRO_USER_CODE_REGION_START InitClassMembers
			// __LLBLGENPRO_USER_CODE_REGION_END
			OnInitClassMembersComplete();
		}


		#region Custom Property Hashtable Setup
		/// <summary> Initializes the hashtables for the entity type and entity field custom properties. </summary>
		private static void SetupCustomPropertyHashtables()
		{
			_customProperties = new Dictionary<string, string>();
			_fieldsCustomProperties = new Dictionary<string, Dictionary<string, string>>();
			Dictionary<string, string> fieldHashtable;
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipmentID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RowVersion", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OrderID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipmentType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ContentWeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("TotalWeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Processed", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ProcessedDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ProcessedUserID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ProcessedComputerID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipmentCost", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Voided", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("VoidedDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("VoidedUserID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("VoidedComputerID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("TrackingNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CustomsGenerated", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CustomsValue", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RequestedLabelFormat", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ActualLabelFormat", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipFirstName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipMiddleName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipLastName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipCompany", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipStreet1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipStreet2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipStreet3", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipCity", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipStateProvCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipPostalCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipCountryCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipPhone", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipEmail", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipAddressValidationSuggestionCount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipAddressValidationStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipAddressValidationError", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipResidentialStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipPOBox", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipUSTerritory", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipMilitaryAddress", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ResidentialDetermination", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ResidentialResult", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginOriginID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginFirstName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginMiddleName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginLastName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginCompany", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginStreet1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginStreet2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginStreet3", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginCity", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginStateProvCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginPostalCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginCountryCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginPhone", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginFax", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginEmail", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginWebsite", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ReturnShipment", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Insurance", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InsuranceProvider", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipNameParseStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipUnparsedName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginNameParseStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginUnparsedName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BestRateEvents", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipSenseStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipSenseChangeSets", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipSenseEntry", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OnlineShipmentID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BilledType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BilledWeight", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _order</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncOrder(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _order, new PropertyChangedEventHandler( OnOrderPropertyChanged ), "Order", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.OrderEntityUsingOrderIDStatic, true, signalRelatedEntity, "Shipments", resetFKFields, new int[] { (int)ShipmentFieldIndex.OrderID } );
			_order = null;
		}

		/// <summary> setups the sync logic for member _order</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncOrder(IEntityCore relatedEntity)
		{
			if(_order!=relatedEntity)
			{
				DesetupSyncOrder(true, true);
				_order = (OrderEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _order, new PropertyChangedEventHandler( OnOrderPropertyChanged ), "Order", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.OrderEntityUsingOrderIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnOrderPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _amazon</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncAmazon(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _amazon, new PropertyChangedEventHandler( OnAmazonPropertyChanged ), "Amazon", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.AmazonShipmentEntityUsingShipmentIDStatic, false, signalRelatedEntity, "Shipment", false, new int[] { (int)ShipmentFieldIndex.ShipmentID } );
			_amazon = null;
		}
		
		/// <summary> setups the sync logic for member _amazon</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncAmazon(IEntityCore relatedEntity)
		{
			if(_amazon!=relatedEntity)
			{
				DesetupSyncAmazon(true, true);
				_amazon = (AmazonShipmentEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _amazon, new PropertyChangedEventHandler( OnAmazonPropertyChanged ), "Amazon", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.AmazonShipmentEntityUsingShipmentIDStatic, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnAmazonPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _bestRate</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncBestRate(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _bestRate, new PropertyChangedEventHandler( OnBestRatePropertyChanged ), "BestRate", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.BestRateShipmentEntityUsingShipmentIDStatic, false, signalRelatedEntity, "Shipment", false, new int[] { (int)ShipmentFieldIndex.ShipmentID } );
			_bestRate = null;
		}
		
		/// <summary> setups the sync logic for member _bestRate</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncBestRate(IEntityCore relatedEntity)
		{
			if(_bestRate!=relatedEntity)
			{
				DesetupSyncBestRate(true, true);
				_bestRate = (BestRateShipmentEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _bestRate, new PropertyChangedEventHandler( OnBestRatePropertyChanged ), "BestRate", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.BestRateShipmentEntityUsingShipmentIDStatic, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnBestRatePropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _fedEx</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncFedEx(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _fedEx, new PropertyChangedEventHandler( OnFedExPropertyChanged ), "FedEx", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.FedExShipmentEntityUsingShipmentIDStatic, false, signalRelatedEntity, "Shipment", false, new int[] { (int)ShipmentFieldIndex.ShipmentID } );
			_fedEx = null;
		}
		
		/// <summary> setups the sync logic for member _fedEx</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncFedEx(IEntityCore relatedEntity)
		{
			if(_fedEx!=relatedEntity)
			{
				DesetupSyncFedEx(true, true);
				_fedEx = (FedExShipmentEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _fedEx, new PropertyChangedEventHandler( OnFedExPropertyChanged ), "FedEx", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.FedExShipmentEntityUsingShipmentIDStatic, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnFedExPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _insurancePolicy</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncInsurancePolicy(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _insurancePolicy, new PropertyChangedEventHandler( OnInsurancePolicyPropertyChanged ), "InsurancePolicy", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.InsurancePolicyEntityUsingShipmentIDStatic, false, signalRelatedEntity, "Shipment", false, new int[] { (int)ShipmentFieldIndex.ShipmentID } );
			_insurancePolicy = null;
		}
		
		/// <summary> setups the sync logic for member _insurancePolicy</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncInsurancePolicy(IEntityCore relatedEntity)
		{
			if(_insurancePolicy!=relatedEntity)
			{
				DesetupSyncInsurancePolicy(true, true);
				_insurancePolicy = (InsurancePolicyEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _insurancePolicy, new PropertyChangedEventHandler( OnInsurancePolicyPropertyChanged ), "InsurancePolicy", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.InsurancePolicyEntityUsingShipmentIDStatic, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnInsurancePolicyPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _iParcel</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncIParcel(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _iParcel, new PropertyChangedEventHandler( OnIParcelPropertyChanged ), "IParcel", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.IParcelShipmentEntityUsingShipmentIDStatic, false, signalRelatedEntity, "Shipment", false, new int[] { (int)ShipmentFieldIndex.ShipmentID } );
			_iParcel = null;
		}
		
		/// <summary> setups the sync logic for member _iParcel</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncIParcel(IEntityCore relatedEntity)
		{
			if(_iParcel!=relatedEntity)
			{
				DesetupSyncIParcel(true, true);
				_iParcel = (IParcelShipmentEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _iParcel, new PropertyChangedEventHandler( OnIParcelPropertyChanged ), "IParcel", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.IParcelShipmentEntityUsingShipmentIDStatic, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnIParcelPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _onTrac</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncOnTrac(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _onTrac, new PropertyChangedEventHandler( OnOnTracPropertyChanged ), "OnTrac", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.OnTracShipmentEntityUsingShipmentIDStatic, false, signalRelatedEntity, "Shipment", false, new int[] { (int)ShipmentFieldIndex.ShipmentID } );
			_onTrac = null;
		}
		
		/// <summary> setups the sync logic for member _onTrac</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncOnTrac(IEntityCore relatedEntity)
		{
			if(_onTrac!=relatedEntity)
			{
				DesetupSyncOnTrac(true, true);
				_onTrac = (OnTracShipmentEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _onTrac, new PropertyChangedEventHandler( OnOnTracPropertyChanged ), "OnTrac", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.OnTracShipmentEntityUsingShipmentIDStatic, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnOnTracPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _other</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncOther(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _other, new PropertyChangedEventHandler( OnOtherPropertyChanged ), "Other", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.OtherShipmentEntityUsingShipmentIDStatic, false, signalRelatedEntity, "Shipment", false, new int[] { (int)ShipmentFieldIndex.ShipmentID } );
			_other = null;
		}
		
		/// <summary> setups the sync logic for member _other</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncOther(IEntityCore relatedEntity)
		{
			if(_other!=relatedEntity)
			{
				DesetupSyncOther(true, true);
				_other = (OtherShipmentEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _other, new PropertyChangedEventHandler( OnOtherPropertyChanged ), "Other", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.OtherShipmentEntityUsingShipmentIDStatic, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnOtherPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _postal</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncPostal(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _postal, new PropertyChangedEventHandler( OnPostalPropertyChanged ), "Postal", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.PostalShipmentEntityUsingShipmentIDStatic, false, signalRelatedEntity, "Shipment", false, new int[] { (int)ShipmentFieldIndex.ShipmentID } );
			_postal = null;
		}
		
		/// <summary> setups the sync logic for member _postal</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncPostal(IEntityCore relatedEntity)
		{
			if(_postal!=relatedEntity)
			{
				DesetupSyncPostal(true, true);
				_postal = (PostalShipmentEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _postal, new PropertyChangedEventHandler( OnPostalPropertyChanged ), "Postal", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.PostalShipmentEntityUsingShipmentIDStatic, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnPostalPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _ups</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncUps(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _ups, new PropertyChangedEventHandler( OnUpsPropertyChanged ), "Ups", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.UpsShipmentEntityUsingShipmentIDStatic, false, signalRelatedEntity, "Shipment", false, new int[] { (int)ShipmentFieldIndex.ShipmentID } );
			_ups = null;
		}
		
		/// <summary> setups the sync logic for member _ups</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncUps(IEntityCore relatedEntity)
		{
			if(_ups!=relatedEntity)
			{
				DesetupSyncUps(true, true);
				_ups = (UpsShipmentEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _ups, new PropertyChangedEventHandler( OnUpsPropertyChanged ), "Ups", ShipWorks.Data.Model.RelationClasses.StaticShipmentRelations.UpsShipmentEntityUsingShipmentIDStatic, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnUpsPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this ShipmentEntity</param>
		/// <param name="fields">Fields of this entity</param>
		private void InitClassEmpty(IValidator validator, IEntityFields2 fields)
		{
			OnInitializing();
			this.Fields = fields ?? CreateFields();
			this.Validator = validator;
			InitClassMembers();

			// __LLBLGENPRO_USER_CODE_REGION_START InitClassEmpty
			// __LLBLGENPRO_USER_CODE_REGION_END

			OnInitialized();

		}

		#region Class Property Declarations
		/// <summary> The relations object holding all relations of this entity with other entity classes.</summary>
		public  static ShipmentRelations Relations
		{
			get	{ return new ShipmentRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ShipmentCustomsItem' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathCustomsItems
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<ShipmentCustomsItemEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ShipmentCustomsItemEntityFactory))), (IEntityRelation)GetRelationsForField("CustomsItems")[0], (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, (int)ShipWorks.Data.Model.EntityType.ShipmentCustomsItemEntity, 0, null, null, null, null, "CustomsItems", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ShipmentReturnItem' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathReturnItems
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<ShipmentReturnItemEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ShipmentReturnItemEntityFactory))), (IEntityRelation)GetRelationsForField("ReturnItems")[0], (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, (int)ShipWorks.Data.Model.EntityType.ShipmentReturnItemEntity, 0, null, null, null, null, "ReturnItems", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ValidatedAddress' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathValidatedAddress
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<ValidatedAddressEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ValidatedAddressEntityFactory))), (IEntityRelation)GetRelationsForField("ValidatedAddress")[0], (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, (int)ShipWorks.Data.Model.EntityType.ValidatedAddressEntity, 0, null, null, null, null, "ValidatedAddress", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Order' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathOrderCollectionViaValidatedAddress
		{
			get
			{
				IEntityRelation intermediateRelation = Relations.ValidatedAddressEntityUsingConsumerID;
				intermediateRelation.SetAliases(string.Empty, "ValidatedAddress_");
				return new PrefetchPathElement2(new EntityCollection<OrderEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderEntityFactory))), intermediateRelation,
					(int)ShipWorks.Data.Model.EntityType.ShipmentEntity, (int)ShipWorks.Data.Model.EntityType.OrderEntity, 0, null, null, GetRelationsForField("OrderCollectionViaValidatedAddress"), null, "OrderCollectionViaValidatedAddress", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToMany);
			}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Order' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathOrder
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(OrderEntityFactory))),	(IEntityRelation)GetRelationsForField("Order")[0], (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, (int)ShipWorks.Data.Model.EntityType.OrderEntity, 0, null, null, null, null, "Order", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'AmazonShipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathAmazon
		{
			get { return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(AmazonShipmentEntityFactory))), (IEntityRelation)GetRelationsForField("Amazon")[0], (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, (int)ShipWorks.Data.Model.EntityType.AmazonShipmentEntity, 0, null, null, null, null, "Amazon", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'BestRateShipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathBestRate
		{
			get { return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(BestRateShipmentEntityFactory))), (IEntityRelation)GetRelationsForField("BestRate")[0], (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, (int)ShipWorks.Data.Model.EntityType.BestRateShipmentEntity, 0, null, null, null, null, "BestRate", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'FedExShipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathFedEx
		{
			get { return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(FedExShipmentEntityFactory))), (IEntityRelation)GetRelationsForField("FedEx")[0], (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, (int)ShipWorks.Data.Model.EntityType.FedExShipmentEntity, 0, null, null, null, null, "FedEx", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'InsurancePolicy' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathInsurancePolicy
		{
			get { return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(InsurancePolicyEntityFactory))), (IEntityRelation)GetRelationsForField("InsurancePolicy")[0], (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, (int)ShipWorks.Data.Model.EntityType.InsurancePolicyEntity, 0, null, null, null, null, "InsurancePolicy", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'IParcelShipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathIParcel
		{
			get { return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(IParcelShipmentEntityFactory))), (IEntityRelation)GetRelationsForField("IParcel")[0], (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, (int)ShipWorks.Data.Model.EntityType.IParcelShipmentEntity, 0, null, null, null, null, "IParcel", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'OnTracShipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathOnTrac
		{
			get { return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(OnTracShipmentEntityFactory))), (IEntityRelation)GetRelationsForField("OnTrac")[0], (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, (int)ShipWorks.Data.Model.EntityType.OnTracShipmentEntity, 0, null, null, null, null, "OnTrac", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'OtherShipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathOther
		{
			get { return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(OtherShipmentEntityFactory))), (IEntityRelation)GetRelationsForField("Other")[0], (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, (int)ShipWorks.Data.Model.EntityType.OtherShipmentEntity, 0, null, null, null, null, "Other", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'PostalShipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathPostal
		{
			get { return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(PostalShipmentEntityFactory))), (IEntityRelation)GetRelationsForField("Postal")[0], (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, (int)ShipWorks.Data.Model.EntityType.PostalShipmentEntity, 0, null, null, null, null, "Postal", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UpsShipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUps
		{
			get { return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(UpsShipmentEntityFactory))), (IEntityRelation)GetRelationsForField("Ups")[0], (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, (int)ShipWorks.Data.Model.EntityType.UpsShipmentEntity, 0, null, null, null, null, "Ups", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);	}
		}


		/// <summary> The custom properties for the type of this entity instance.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		protected override Dictionary<string, string> CustomPropertiesOfType
		{
			get { return CustomProperties;}
		}

		/// <summary> The custom properties for the fields of this entity type. The returned Hashtable contains per fieldname a hashtable of name-value pairs. </summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, Dictionary<string, string>> FieldsCustomProperties
		{
			get { return _fieldsCustomProperties;}
		}

		/// <summary> The custom properties for the fields of the type of this entity instance. The returned Hashtable contains per fieldname a hashtable of name-value pairs. </summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		protected override Dictionary<string, Dictionary<string, string>> FieldsCustomPropertiesOfType
		{
			get { return FieldsCustomProperties;}
		}

		/// <summary> The ShipmentID property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipmentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 ShipmentID
		{
			get { return (System.Int64)GetValue((int)ShipmentFieldIndex.ShipmentID, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipmentID, value); }
		}

		/// <summary> The RowVersion property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."RowVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] RowVersion
		{
			get { return (System.Byte[])GetValue((int)ShipmentFieldIndex.RowVersion, true); }

		}

		/// <summary> The OrderID property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OrderID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 OrderID
		{
			get { return (System.Int64)GetValue((int)ShipmentFieldIndex.OrderID, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OrderID, value); }
		}

		/// <summary> The ShipmentType property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipmentType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipmentType
		{
			get { return (System.Int32)GetValue((int)ShipmentFieldIndex.ShipmentType, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipmentType, value); }
		}

		/// <summary> The ContentWeight property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ContentWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double ContentWeight
		{
			get { return (System.Double)GetValue((int)ShipmentFieldIndex.ContentWeight, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ContentWeight, value); }
		}

		/// <summary> The TotalWeight property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."TotalWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double TotalWeight
		{
			get { return (System.Double)GetValue((int)ShipmentFieldIndex.TotalWeight, true); }
			set	{ SetValue((int)ShipmentFieldIndex.TotalWeight, value); }
		}

		/// <summary> The Processed property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."Processed"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Processed
		{
			get { return (System.Boolean)GetValue((int)ShipmentFieldIndex.Processed, true); }
			set	{ SetValue((int)ShipmentFieldIndex.Processed, value); }
		}

		/// <summary> The ProcessedDate property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ProcessedDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.DateTime> ProcessedDate
		{
			get { return (Nullable<System.DateTime>)GetValue((int)ShipmentFieldIndex.ProcessedDate, false); }
			set	{ SetValue((int)ShipmentFieldIndex.ProcessedDate, value); }
		}

		/// <summary> The ProcessedUserID property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ProcessedUserID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> ProcessedUserID
		{
			get { return (Nullable<System.Int64>)GetValue((int)ShipmentFieldIndex.ProcessedUserID, false); }
			set	{ SetValue((int)ShipmentFieldIndex.ProcessedUserID, value); }
		}

		/// <summary> The ProcessedComputerID property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ProcessedComputerID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> ProcessedComputerID
		{
			get { return (Nullable<System.Int64>)GetValue((int)ShipmentFieldIndex.ProcessedComputerID, false); }
			set	{ SetValue((int)ShipmentFieldIndex.ProcessedComputerID, value); }
		}

		/// <summary> The ShipDate property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime ShipDate
		{
			get { return (System.DateTime)GetValue((int)ShipmentFieldIndex.ShipDate, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipDate, value); }
		}

		/// <summary> The ShipmentCost property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipmentCost"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal ShipmentCost
		{
			get { return (System.Decimal)GetValue((int)ShipmentFieldIndex.ShipmentCost, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipmentCost, value); }
		}

		/// <summary> The Voided property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."Voided"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Voided
		{
			get { return (System.Boolean)GetValue((int)ShipmentFieldIndex.Voided, true); }
			set	{ SetValue((int)ShipmentFieldIndex.Voided, value); }
		}

		/// <summary> The VoidedDate property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."VoidedDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.DateTime> VoidedDate
		{
			get { return (Nullable<System.DateTime>)GetValue((int)ShipmentFieldIndex.VoidedDate, false); }
			set	{ SetValue((int)ShipmentFieldIndex.VoidedDate, value); }
		}

		/// <summary> The VoidedUserID property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."VoidedUserID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> VoidedUserID
		{
			get { return (Nullable<System.Int64>)GetValue((int)ShipmentFieldIndex.VoidedUserID, false); }
			set	{ SetValue((int)ShipmentFieldIndex.VoidedUserID, value); }
		}

		/// <summary> The VoidedComputerID property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."VoidedComputerID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> VoidedComputerID
		{
			get { return (Nullable<System.Int64>)GetValue((int)ShipmentFieldIndex.VoidedComputerID, false); }
			set	{ SetValue((int)ShipmentFieldIndex.VoidedComputerID, value); }
		}

		/// <summary> The TrackingNumber property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."TrackingNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String TrackingNumber
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.TrackingNumber, true); }
			set	{ SetValue((int)ShipmentFieldIndex.TrackingNumber, value); }
		}

		/// <summary> The CustomsGenerated property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."CustomsGenerated"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean CustomsGenerated
		{
			get { return (System.Boolean)GetValue((int)ShipmentFieldIndex.CustomsGenerated, true); }
			set	{ SetValue((int)ShipmentFieldIndex.CustomsGenerated, value); }
		}

		/// <summary> The CustomsValue property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."CustomsValue"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal CustomsValue
		{
			get { return (System.Decimal)GetValue((int)ShipmentFieldIndex.CustomsValue, true); }
			set	{ SetValue((int)ShipmentFieldIndex.CustomsValue, value); }
		}

		/// <summary> The RequestedLabelFormat property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."RequestedLabelFormat"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 RequestedLabelFormat
		{
			get { return (System.Int32)GetValue((int)ShipmentFieldIndex.RequestedLabelFormat, true); }
			set	{ SetValue((int)ShipmentFieldIndex.RequestedLabelFormat, value); }
		}

		/// <summary> The ActualLabelFormat property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ActualLabelFormat"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> ActualLabelFormat
		{
			get { return (Nullable<System.Int32>)GetValue((int)ShipmentFieldIndex.ActualLabelFormat, false); }
			set	{ SetValue((int)ShipmentFieldIndex.ActualLabelFormat, value); }
		}

		/// <summary> The ShipFirstName property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipFirstName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipFirstName
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.ShipFirstName, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipFirstName, value); }
		}

		/// <summary> The ShipMiddleName property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipMiddleName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipMiddleName
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.ShipMiddleName, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipMiddleName, value); }
		}

		/// <summary> The ShipLastName property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipLastName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipLastName
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.ShipLastName, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipLastName, value); }
		}

		/// <summary> The ShipCompany property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipCompany"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipCompany
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.ShipCompany, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipCompany, value); }
		}

		/// <summary> The ShipStreet1 property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipStreet1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipStreet1
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.ShipStreet1, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipStreet1, value); }
		}

		/// <summary> The ShipStreet2 property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipStreet2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipStreet2
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.ShipStreet2, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipStreet2, value); }
		}

		/// <summary> The ShipStreet3 property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipStreet3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipStreet3
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.ShipStreet3, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipStreet3, value); }
		}

		/// <summary> The ShipCity property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipCity"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipCity
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.ShipCity, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipCity, value); }
		}

		/// <summary> The ShipStateProvCode property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipStateProvCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipStateProvCode
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.ShipStateProvCode, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipStateProvCode, value); }
		}

		/// <summary> The ShipPostalCode property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipPostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipPostalCode
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.ShipPostalCode, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipPostalCode, value); }
		}

		/// <summary> The ShipCountryCode property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipCountryCode
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.ShipCountryCode, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipCountryCode, value); }
		}

		/// <summary> The ShipPhone property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipPhone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipPhone
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.ShipPhone, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipPhone, value); }
		}

		/// <summary> The ShipEmail property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipEmail"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipEmail
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.ShipEmail, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipEmail, value); }
		}

		/// <summary> The ShipAddressValidationSuggestionCount property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipAddressValidationSuggestionCount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipAddressValidationSuggestionCount
		{
			get { return (System.Int32)GetValue((int)ShipmentFieldIndex.ShipAddressValidationSuggestionCount, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipAddressValidationSuggestionCount, value); }
		}

		/// <summary> The ShipAddressValidationStatus property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipAddressValidationStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipAddressValidationStatus
		{
			get { return (System.Int32)GetValue((int)ShipmentFieldIndex.ShipAddressValidationStatus, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipAddressValidationStatus, value); }
		}

		/// <summary> The ShipAddressValidationError property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipAddressValidationError"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipAddressValidationError
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.ShipAddressValidationError, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipAddressValidationError, value); }
		}

		/// <summary> The ShipResidentialStatus property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipResidentialStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipResidentialStatus
		{
			get { return (System.Int32)GetValue((int)ShipmentFieldIndex.ShipResidentialStatus, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipResidentialStatus, value); }
		}

		/// <summary> The ShipPOBox property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipPOBox"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipPOBox
		{
			get { return (System.Int32)GetValue((int)ShipmentFieldIndex.ShipPOBox, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipPOBox, value); }
		}

		/// <summary> The ShipUSTerritory property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipUSTerritory"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipUSTerritory
		{
			get { return (System.Int32)GetValue((int)ShipmentFieldIndex.ShipUSTerritory, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipUSTerritory, value); }
		}

		/// <summary> The ShipMilitaryAddress property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipMilitaryAddress"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipMilitaryAddress
		{
			get { return (System.Int32)GetValue((int)ShipmentFieldIndex.ShipMilitaryAddress, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipMilitaryAddress, value); }
		}

		/// <summary> The ResidentialDetermination property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ResidentialDetermination"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ResidentialDetermination
		{
			get { return (System.Int32)GetValue((int)ShipmentFieldIndex.ResidentialDetermination, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ResidentialDetermination, value); }
		}

		/// <summary> The ResidentialResult property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ResidentialResult"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean ResidentialResult
		{
			get { return (System.Boolean)GetValue((int)ShipmentFieldIndex.ResidentialResult, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ResidentialResult, value); }
		}

		/// <summary> The OriginOriginID property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginOriginID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 OriginOriginID
		{
			get { return (System.Int64)GetValue((int)ShipmentFieldIndex.OriginOriginID, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginOriginID, value); }
		}

		/// <summary> The OriginFirstName property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginFirstName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginFirstName
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OriginFirstName, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginFirstName, value); }
		}

		/// <summary> The OriginMiddleName property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginMiddleName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginMiddleName
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OriginMiddleName, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginMiddleName, value); }
		}

		/// <summary> The OriginLastName property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginLastName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginLastName
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OriginLastName, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginLastName, value); }
		}

		/// <summary> The OriginCompany property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginCompany"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginCompany
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OriginCompany, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginCompany, value); }
		}

		/// <summary> The OriginStreet1 property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginStreet1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginStreet1
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OriginStreet1, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginStreet1, value); }
		}

		/// <summary> The OriginStreet2 property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginStreet2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginStreet2
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OriginStreet2, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginStreet2, value); }
		}

		/// <summary> The OriginStreet3 property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginStreet3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginStreet3
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OriginStreet3, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginStreet3, value); }
		}

		/// <summary> The OriginCity property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginCity"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginCity
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OriginCity, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginCity, value); }
		}

		/// <summary> The OriginStateProvCode property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginStateProvCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginStateProvCode
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OriginStateProvCode, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginStateProvCode, value); }
		}

		/// <summary> The OriginPostalCode property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginPostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginPostalCode
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OriginPostalCode, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginPostalCode, value); }
		}

		/// <summary> The OriginCountryCode property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginCountryCode
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OriginCountryCode, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginCountryCode, value); }
		}

		/// <summary> The OriginPhone property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginPhone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginPhone
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OriginPhone, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginPhone, value); }
		}

		/// <summary> The OriginFax property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginFax"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginFax
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OriginFax, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginFax, value); }
		}

		/// <summary> The OriginEmail property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginEmail"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginEmail
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OriginEmail, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginEmail, value); }
		}

		/// <summary> The OriginWebsite property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginWebsite"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginWebsite
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OriginWebsite, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginWebsite, value); }
		}

		/// <summary> The ReturnShipment property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ReturnShipment"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean ReturnShipment
		{
			get { return (System.Boolean)GetValue((int)ShipmentFieldIndex.ReturnShipment, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ReturnShipment, value); }
		}

		/// <summary> The Insurance property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."Insurance"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Insurance
		{
			get { return (System.Boolean)GetValue((int)ShipmentFieldIndex.Insurance, true); }
			set	{ SetValue((int)ShipmentFieldIndex.Insurance, value); }
		}

		/// <summary> The InsuranceProvider property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."InsuranceProvider"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 InsuranceProvider
		{
			get { return (System.Int32)GetValue((int)ShipmentFieldIndex.InsuranceProvider, true); }
			set	{ SetValue((int)ShipmentFieldIndex.InsuranceProvider, value); }
		}

		/// <summary> The ShipNameParseStatus property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipNameParseStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipNameParseStatus
		{
			get { return (System.Int32)GetValue((int)ShipmentFieldIndex.ShipNameParseStatus, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipNameParseStatus, value); }
		}

		/// <summary> The ShipUnparsedName property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipUnparsedName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipUnparsedName
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.ShipUnparsedName, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipUnparsedName, value); }
		}

		/// <summary> The OriginNameParseStatus property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginNameParseStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 OriginNameParseStatus
		{
			get { return (System.Int32)GetValue((int)ShipmentFieldIndex.OriginNameParseStatus, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginNameParseStatus, value); }
		}

		/// <summary> The OriginUnparsedName property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OriginUnparsedName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginUnparsedName
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OriginUnparsedName, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OriginUnparsedName, value); }
		}

		/// <summary> The BestRateEvents property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."BestRateEvents"<br/>
		/// Table field type characteristics (type, precision, scale, length): TinyInt, 3, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte BestRateEvents
		{
			get { return (System.Byte)GetValue((int)ShipmentFieldIndex.BestRateEvents, true); }
			set	{ SetValue((int)ShipmentFieldIndex.BestRateEvents, value); }
		}

		/// <summary> The ShipSenseStatus property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipSenseStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipSenseStatus
		{
			get { return (System.Int32)GetValue((int)ShipmentFieldIndex.ShipSenseStatus, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipSenseStatus, value); }
		}

		/// <summary> The ShipSenseChangeSets property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipSenseChangeSets"<br/>
		/// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipSenseChangeSets
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.ShipSenseChangeSets, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipSenseChangeSets, value); }
		}

		/// <summary> The ShipSenseEntry property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."ShipSenseEntry"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] ShipSenseEntry
		{
			get { return (System.Byte[])GetValue((int)ShipmentFieldIndex.ShipSenseEntry, true); }
			set	{ SetValue((int)ShipmentFieldIndex.ShipSenseEntry, value); }
		}

		/// <summary> The OnlineShipmentID property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."OnlineShipmentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 128<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OnlineShipmentID
		{
			get { return (System.String)GetValue((int)ShipmentFieldIndex.OnlineShipmentID, true); }
			set	{ SetValue((int)ShipmentFieldIndex.OnlineShipmentID, value); }
		}

		/// <summary> The BilledType property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."BilledType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 BilledType
		{
			get { return (System.Int32)GetValue((int)ShipmentFieldIndex.BilledType, true); }
			set	{ SetValue((int)ShipmentFieldIndex.BilledType, value); }
		}

		/// <summary> The BilledWeight property of the Entity Shipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Shipment"."BilledWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double BilledWeight
		{
			get { return (System.Double)GetValue((int)ShipmentFieldIndex.BilledWeight, true); }
			set	{ SetValue((int)ShipmentFieldIndex.BilledWeight, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ShipmentCustomsItemEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ShipmentCustomsItemEntity))]
		public virtual EntityCollection<ShipmentCustomsItemEntity> CustomsItems
		{
			get { return GetOrCreateEntityCollection<ShipmentCustomsItemEntity, ShipmentCustomsItemEntityFactory>("Shipment", true, false, ref _customsItems);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ShipmentReturnItemEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ShipmentReturnItemEntity))]
		public virtual EntityCollection<ShipmentReturnItemEntity> ReturnItems
		{
			get { return GetOrCreateEntityCollection<ShipmentReturnItemEntity, ShipmentReturnItemEntityFactory>("Shipment", true, false, ref _returnItems);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ValidatedAddressEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ValidatedAddressEntity))]
		public virtual EntityCollection<ValidatedAddressEntity> ValidatedAddress
		{
			get { return GetOrCreateEntityCollection<ValidatedAddressEntity, ValidatedAddressEntityFactory>("Shipment", true, false, ref _validatedAddress);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'OrderEntity' which are related to this entity via a relation of type 'm:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(OrderEntity))]
		public virtual EntityCollection<OrderEntity> OrderCollectionViaValidatedAddress
		{
			get { return GetOrCreateEntityCollection<OrderEntity, OrderEntityFactory>("ShipmentCollectionViaValidatedAddress", false, true, ref _orderCollectionViaValidatedAddress);	}
		}

		/// <summary> Gets / sets related entity of type 'OrderEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual OrderEntity Order
		{
			get	{ return _order; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncOrder(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "Shipments", "Order", _order, true); 
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'AmazonShipmentEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned.<br/><br/>
		/// </summary>
		[Browsable(true)]
		public virtual AmazonShipmentEntity Amazon
		{
			get { return _amazon; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncAmazon(value);
					CallSetRelatedEntityDuringDeserialization(value, "Shipment");
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_amazon !=null);
						DesetupSyncAmazon(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("Amazon");
						}
					}
					else
					{
						if(_amazon!=value)
						{
							((IEntity2)value).SetRelatedEntity(this, "Shipment");
							SetupSyncAmazon(value);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'BestRateShipmentEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned.<br/><br/>
		/// </summary>
		[Browsable(true)]
		public virtual BestRateShipmentEntity BestRate
		{
			get { return _bestRate; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncBestRate(value);
					CallSetRelatedEntityDuringDeserialization(value, "Shipment");
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_bestRate !=null);
						DesetupSyncBestRate(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("BestRate");
						}
					}
					else
					{
						if(_bestRate!=value)
						{
							((IEntity2)value).SetRelatedEntity(this, "Shipment");
							SetupSyncBestRate(value);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'FedExShipmentEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned.<br/><br/>
		/// </summary>
		[Browsable(true)]
		public virtual FedExShipmentEntity FedEx
		{
			get { return _fedEx; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncFedEx(value);
					CallSetRelatedEntityDuringDeserialization(value, "Shipment");
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_fedEx !=null);
						DesetupSyncFedEx(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("FedEx");
						}
					}
					else
					{
						if(_fedEx!=value)
						{
							((IEntity2)value).SetRelatedEntity(this, "Shipment");
							SetupSyncFedEx(value);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'InsurancePolicyEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned.<br/><br/>
		/// </summary>
		[Browsable(true)]
		public virtual InsurancePolicyEntity InsurancePolicy
		{
			get { return _insurancePolicy; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncInsurancePolicy(value);
					CallSetRelatedEntityDuringDeserialization(value, "Shipment");
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_insurancePolicy !=null);
						DesetupSyncInsurancePolicy(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("InsurancePolicy");
						}
					}
					else
					{
						if(_insurancePolicy!=value)
						{
							((IEntity2)value).SetRelatedEntity(this, "Shipment");
							SetupSyncInsurancePolicy(value);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'IParcelShipmentEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned.<br/><br/>
		/// </summary>
		[Browsable(true)]
		public virtual IParcelShipmentEntity IParcel
		{
			get { return _iParcel; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncIParcel(value);
					CallSetRelatedEntityDuringDeserialization(value, "Shipment");
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_iParcel !=null);
						DesetupSyncIParcel(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("IParcel");
						}
					}
					else
					{
						if(_iParcel!=value)
						{
							((IEntity2)value).SetRelatedEntity(this, "Shipment");
							SetupSyncIParcel(value);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'OnTracShipmentEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned.<br/><br/>
		/// </summary>
		[Browsable(true)]
		public virtual OnTracShipmentEntity OnTrac
		{
			get { return _onTrac; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncOnTrac(value);
					CallSetRelatedEntityDuringDeserialization(value, "Shipment");
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_onTrac !=null);
						DesetupSyncOnTrac(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("OnTrac");
						}
					}
					else
					{
						if(_onTrac!=value)
						{
							((IEntity2)value).SetRelatedEntity(this, "Shipment");
							SetupSyncOnTrac(value);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'OtherShipmentEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned.<br/><br/>
		/// </summary>
		[Browsable(true)]
		public virtual OtherShipmentEntity Other
		{
			get { return _other; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncOther(value);
					CallSetRelatedEntityDuringDeserialization(value, "Shipment");
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_other !=null);
						DesetupSyncOther(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("Other");
						}
					}
					else
					{
						if(_other!=value)
						{
							((IEntity2)value).SetRelatedEntity(this, "Shipment");
							SetupSyncOther(value);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'PostalShipmentEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned.<br/><br/>
		/// </summary>
		[Browsable(true)]
		public virtual PostalShipmentEntity Postal
		{
			get { return _postal; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncPostal(value);
					CallSetRelatedEntityDuringDeserialization(value, "Shipment");
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_postal !=null);
						DesetupSyncPostal(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("Postal");
						}
					}
					else
					{
						if(_postal!=value)
						{
							((IEntity2)value).SetRelatedEntity(this, "Shipment");
							SetupSyncPostal(value);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'UpsShipmentEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned.<br/><br/>
		/// </summary>
		[Browsable(true)]
		public virtual UpsShipmentEntity Ups
		{
			get { return _ups; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncUps(value);
					CallSetRelatedEntityDuringDeserialization(value, "Shipment");
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_ups !=null);
						DesetupSyncUps(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("Ups");
						}
					}
					else
					{
						if(_ups!=value)
						{
							((IEntity2)value).SetRelatedEntity(this, "Shipment");
							SetupSyncUps(value);
						}
					}
				}
			}
		}
	
		/// <summary> Gets the type of the hierarchy this entity is in. </summary>
		protected override InheritanceHierarchyType LLBLGenProIsInHierarchyOfType
		{
			get { return InheritanceHierarchyType.None;}
		}
		
		/// <summary> Gets or sets a value indicating whether this entity is a subtype</summary>
		protected override bool LLBLGenProIsSubType
		{
			get { return false;}
		}
		
		/// <summary>Returns the ShipWorks.Data.Model.EntityType enum value for this entity.</summary>
		[Browsable(false), XmlIgnore]
		protected override int LLBLGenProEntityTypeValue 
		{ 
			get { return (int)ShipWorks.Data.Model.EntityType.ShipmentEntity; }
		}

		#endregion


		#region Custom Entity code
		
		// __LLBLGENPRO_USER_CODE_REGION_START CustomEntityCode
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Included code

		#endregion
	}
}
