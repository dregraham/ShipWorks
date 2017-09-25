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
	/// <summary>Entity class which represents the entity 'Order'.<br/><br/></summary>
	[Serializable]
	public partial class OrderEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<NoteEntity> _notes;
		private EntityCollection<OrderChargeEntity> _orderCharges;
		private EntityCollection<OrderItemEntity> _orderItems;
		private EntityCollection<OrderPaymentDetailEntity> _orderPaymentDetails;
		private EntityCollection<OrderSearchEntity> _orderSearch;
		private EntityCollection<ShipmentEntity> _shipments;
		private EntityCollection<ValidatedAddressEntity> _validatedAddress;
		private EntityCollection<ShipmentEntity> _shipmentCollectionViaValidatedAddress;
		private CustomerEntity _customer;
		private StoreEntity _store;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name Customer</summary>
			public static readonly string Customer = "Customer";
			/// <summary>Member name Store</summary>
			public static readonly string Store = "Store";
			/// <summary>Member name Notes</summary>
			public static readonly string Notes = "Notes";
			/// <summary>Member name OrderCharges</summary>
			public static readonly string OrderCharges = "OrderCharges";
			/// <summary>Member name OrderItems</summary>
			public static readonly string OrderItems = "OrderItems";
			/// <summary>Member name OrderPaymentDetails</summary>
			public static readonly string OrderPaymentDetails = "OrderPaymentDetails";
			/// <summary>Member name OrderSearch</summary>
			public static readonly string OrderSearch = "OrderSearch";
			/// <summary>Member name Shipments</summary>
			public static readonly string Shipments = "Shipments";
			/// <summary>Member name ValidatedAddress</summary>
			public static readonly string ValidatedAddress = "ValidatedAddress";
			/// <summary>Member name ShipmentCollectionViaValidatedAddress</summary>
			public static readonly string ShipmentCollectionViaValidatedAddress = "ShipmentCollectionViaValidatedAddress";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static OrderEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public OrderEntity():base("OrderEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public OrderEntity(IEntityFields2 fields):base("OrderEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this OrderEntity</param>
		public OrderEntity(IValidator validator):base("OrderEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="orderID">PK value for Order which data should be fetched into this Order object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public OrderEntity(System.Int64 orderID):base("OrderEntity")
		{
			InitClassEmpty(null, null);
			this.OrderID = orderID;
		}

		/// <summary> CTor</summary>
		/// <param name="orderID">PK value for Order which data should be fetched into this Order object</param>
		/// <param name="validator">The custom validator object for this OrderEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public OrderEntity(System.Int64 orderID, IValidator validator):base("OrderEntity")
		{
			InitClassEmpty(validator, null);
			this.OrderID = orderID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected OrderEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_notes = (EntityCollection<NoteEntity>)info.GetValue("_notes", typeof(EntityCollection<NoteEntity>));
				_orderCharges = (EntityCollection<OrderChargeEntity>)info.GetValue("_orderCharges", typeof(EntityCollection<OrderChargeEntity>));
				_orderItems = (EntityCollection<OrderItemEntity>)info.GetValue("_orderItems", typeof(EntityCollection<OrderItemEntity>));
				_orderPaymentDetails = (EntityCollection<OrderPaymentDetailEntity>)info.GetValue("_orderPaymentDetails", typeof(EntityCollection<OrderPaymentDetailEntity>));
				_orderSearch = (EntityCollection<OrderSearchEntity>)info.GetValue("_orderSearch", typeof(EntityCollection<OrderSearchEntity>));
				_shipments = (EntityCollection<ShipmentEntity>)info.GetValue("_shipments", typeof(EntityCollection<ShipmentEntity>));
				_validatedAddress = (EntityCollection<ValidatedAddressEntity>)info.GetValue("_validatedAddress", typeof(EntityCollection<ValidatedAddressEntity>));
				_shipmentCollectionViaValidatedAddress = (EntityCollection<ShipmentEntity>)info.GetValue("_shipmentCollectionViaValidatedAddress", typeof(EntityCollection<ShipmentEntity>));
				_customer = (CustomerEntity)info.GetValue("_customer", typeof(CustomerEntity));
				if(_customer!=null)
				{
					_customer.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_store = (StoreEntity)info.GetValue("_store", typeof(StoreEntity));
				if(_store!=null)
				{
					_store.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((OrderFieldIndex)fieldIndex)
			{
				case OrderFieldIndex.StoreID:
					DesetupSyncStore(true, false);
					break;
				case OrderFieldIndex.CustomerID:
					DesetupSyncCustomer(true, false);
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
				case "Customer":
					this.Customer = (CustomerEntity)entity;
					break;
				case "Store":
					this.Store = (StoreEntity)entity;
					break;
				case "Notes":
					this.Notes.Add((NoteEntity)entity);
					break;
				case "OrderCharges":
					this.OrderCharges.Add((OrderChargeEntity)entity);
					break;
				case "OrderItems":
					this.OrderItems.Add((OrderItemEntity)entity);
					break;
				case "OrderPaymentDetails":
					this.OrderPaymentDetails.Add((OrderPaymentDetailEntity)entity);
					break;
				case "OrderSearch":
					this.OrderSearch.Add((OrderSearchEntity)entity);
					break;
				case "Shipments":
					this.Shipments.Add((ShipmentEntity)entity);
					break;
				case "ValidatedAddress":
					this.ValidatedAddress.Add((ValidatedAddressEntity)entity);
					break;
				case "ShipmentCollectionViaValidatedAddress":
					this.ShipmentCollectionViaValidatedAddress.IsReadOnly = false;
					this.ShipmentCollectionViaValidatedAddress.Add((ShipmentEntity)entity);
					this.ShipmentCollectionViaValidatedAddress.IsReadOnly = true;
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
				case "Customer":
					toReturn.Add(Relations.CustomerEntityUsingCustomerID);
					break;
				case "Store":
					toReturn.Add(Relations.StoreEntityUsingStoreID);
					break;
				case "Notes":
					toReturn.Add(Relations.NoteEntityUsingEntityID);
					break;
				case "OrderCharges":
					toReturn.Add(Relations.OrderChargeEntityUsingOrderID);
					break;
				case "OrderItems":
					toReturn.Add(Relations.OrderItemEntityUsingOrderID);
					break;
				case "OrderPaymentDetails":
					toReturn.Add(Relations.OrderPaymentDetailEntityUsingOrderID);
					break;
				case "OrderSearch":
					toReturn.Add(Relations.OrderSearchEntityUsingOrderID);
					break;
				case "Shipments":
					toReturn.Add(Relations.ShipmentEntityUsingOrderID);
					break;
				case "ValidatedAddress":
					toReturn.Add(Relations.ValidatedAddressEntityUsingConsumerID);
					break;
				case "ShipmentCollectionViaValidatedAddress":
					toReturn.Add(Relations.ValidatedAddressEntityUsingConsumerID, "OrderEntity__", "ValidatedAddress_", JoinHint.None);
					toReturn.Add(ValidatedAddressEntity.Relations.ShipmentEntityUsingConsumerID, "ValidatedAddress_", string.Empty, JoinHint.None);
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
			int numberOfOneWayRelations = 0+1;
			switch(propertyName)
			{
				case null:
					return ((numberOfOneWayRelations > 0) || base.CheckOneWayRelations(null));
				case "Store":
					return true;
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
				case "Customer":
					SetupSyncCustomer(relatedEntity);
					break;
				case "Store":
					SetupSyncStore(relatedEntity);
					break;
				case "Notes":
					this.Notes.Add((NoteEntity)relatedEntity);
					break;
				case "OrderCharges":
					this.OrderCharges.Add((OrderChargeEntity)relatedEntity);
					break;
				case "OrderItems":
					this.OrderItems.Add((OrderItemEntity)relatedEntity);
					break;
				case "OrderPaymentDetails":
					this.OrderPaymentDetails.Add((OrderPaymentDetailEntity)relatedEntity);
					break;
				case "OrderSearch":
					this.OrderSearch.Add((OrderSearchEntity)relatedEntity);
					break;
				case "Shipments":
					this.Shipments.Add((ShipmentEntity)relatedEntity);
					break;
				case "ValidatedAddress":
					this.ValidatedAddress.Add((ValidatedAddressEntity)relatedEntity);
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
				case "Customer":
					DesetupSyncCustomer(false, true);
					break;
				case "Store":
					DesetupSyncStore(false, true);
					break;
				case "Notes":
					this.PerformRelatedEntityRemoval(this.Notes, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "OrderCharges":
					this.PerformRelatedEntityRemoval(this.OrderCharges, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "OrderItems":
					this.PerformRelatedEntityRemoval(this.OrderItems, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "OrderPaymentDetails":
					this.PerformRelatedEntityRemoval(this.OrderPaymentDetails, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "OrderSearch":
					this.PerformRelatedEntityRemoval(this.OrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "Shipments":
					this.PerformRelatedEntityRemoval(this.Shipments, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "ValidatedAddress":
					this.PerformRelatedEntityRemoval(this.ValidatedAddress, relatedEntity, signalRelatedEntityManyToOne);
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
			return toReturn;
		}
		
		/// <summary> Gets a collection of related entities referenced by this entity which this entity depends on (this entity is the FK side of their PK fields). These
		/// entities will have to be persisted before this entity during a recursive save.</summary>
		/// <returns>Collection with 0 or more IEntity2 objects, referenced by this entity</returns>
		protected override List<IEntity2> GetDependentRelatedEntities()
		{
			List<IEntity2> toReturn = new List<IEntity2>();
			if(_customer!=null)
			{
				toReturn.Add(_customer);
			}
			if(_store!=null)
			{
				toReturn.Add(_store);
			}
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.Add(this.Notes);
			toReturn.Add(this.OrderCharges);
			toReturn.Add(this.OrderItems);
			toReturn.Add(this.OrderPaymentDetails);
			toReturn.Add(this.OrderSearch);
			toReturn.Add(this.Shipments);
			toReturn.Add(this.ValidatedAddress);
			return toReturn;
		}

		/// <summary>Gets a predicateexpression which filters on this entity</summary>
		/// <returns>ready to use predicateexpression</returns>
		/// <remarks>Only useful in entity fetches.</remarks>
		public  static IPredicateExpression GetEntityTypeFilter()
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetEntityTypeFilter("OrderEntity", false);
		}
		
		/// <summary>Gets a predicateexpression which filters on this entity</summary>
		/// <param name="negate">Flag to produce a NOT filter, (true), or a normal filter (false). </param>
		/// <returns>ready to use predicateexpression</returns>
		/// <remarks>Only useful in entity fetches.</remarks>
		public  static IPredicateExpression GetEntityTypeFilter(bool negate)
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetEntityTypeFilter("OrderEntity", negate);
		}

		/// <summary>ISerializable member. Does custom serialization so event handlers do not get serialized. Serializes members of this entity class and uses the base class' implementation to serialize the rest.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				info.AddValue("_notes", ((_notes!=null) && (_notes.Count>0) && !this.MarkedForDeletion)?_notes:null);
				info.AddValue("_orderCharges", ((_orderCharges!=null) && (_orderCharges.Count>0) && !this.MarkedForDeletion)?_orderCharges:null);
				info.AddValue("_orderItems", ((_orderItems!=null) && (_orderItems.Count>0) && !this.MarkedForDeletion)?_orderItems:null);
				info.AddValue("_orderPaymentDetails", ((_orderPaymentDetails!=null) && (_orderPaymentDetails.Count>0) && !this.MarkedForDeletion)?_orderPaymentDetails:null);
				info.AddValue("_orderSearch", ((_orderSearch!=null) && (_orderSearch.Count>0) && !this.MarkedForDeletion)?_orderSearch:null);
				info.AddValue("_shipments", ((_shipments!=null) && (_shipments.Count>0) && !this.MarkedForDeletion)?_shipments:null);
				info.AddValue("_validatedAddress", ((_validatedAddress!=null) && (_validatedAddress.Count>0) && !this.MarkedForDeletion)?_validatedAddress:null);
				info.AddValue("_shipmentCollectionViaValidatedAddress", ((_shipmentCollectionViaValidatedAddress!=null) && (_shipmentCollectionViaValidatedAddress.Count>0) && !this.MarkedForDeletion)?_shipmentCollectionViaValidatedAddress:null);
				info.AddValue("_customer", (!this.MarkedForDeletion?_customer:null));
				info.AddValue("_store", (!this.MarkedForDeletion?_store:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}

		
		/// <summary>Determines whether this entity is a subType of the entity represented by the passed in enum value, which represents a value in the ShipWorks.Data.Model.EntityType enum</summary>
		/// <param name="typeOfEntity">Type of entity.</param>
		/// <returns>true if the passed in type is a supertype of this entity, otherwise false</returns>
		protected override bool CheckIfIsSubTypeOf(int typeOfEntity)
		{
			return InheritanceInfoProviderSingleton.GetInstance().CheckIfIsSubTypeOf("OrderEntity", ((ShipWorks.Data.Model.EntityType)typeOfEntity).ToString());
		}
				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new OrderRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'Note' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoNotes()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(NoteFields.EntityID, null, ComparisonOperator.Equal, this.OrderID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'OrderCharge' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoOrderCharges()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(OrderChargeFields.OrderID, null, ComparisonOperator.Equal, this.OrderID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'OrderItem' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoOrderItems()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(OrderItemFields.OrderID, null, ComparisonOperator.Equal, this.OrderID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'OrderPaymentDetail' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoOrderPaymentDetails()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(OrderPaymentDetailFields.OrderID, null, ComparisonOperator.Equal, this.OrderID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'OrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(OrderSearchFields.OrderID, null, ComparisonOperator.Equal, this.OrderID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'Shipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoShipments()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ShipmentFields.OrderID, null, ComparisonOperator.Equal, this.OrderID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'ValidatedAddress' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoValidatedAddress()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ValidatedAddressFields.ConsumerID, null, ComparisonOperator.Equal, this.OrderID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'Shipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoShipmentCollectionViaValidatedAddress()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.Relations.AddRange(GetRelationsForFieldOfType("ShipmentCollectionViaValidatedAddress"));
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(OrderFields.OrderID, null, ComparisonOperator.Equal, this.OrderID, "OrderEntity__"));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'Customer' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoCustomer()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(CustomerFields.CustomerID, null, ComparisonOperator.Equal, this.CustomerID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'Store' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoStore()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(StoreFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(OrderEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._notes);
			collectionsQueue.Enqueue(this._orderCharges);
			collectionsQueue.Enqueue(this._orderItems);
			collectionsQueue.Enqueue(this._orderPaymentDetails);
			collectionsQueue.Enqueue(this._orderSearch);
			collectionsQueue.Enqueue(this._shipments);
			collectionsQueue.Enqueue(this._validatedAddress);
			collectionsQueue.Enqueue(this._shipmentCollectionViaValidatedAddress);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._notes = (EntityCollection<NoteEntity>) collectionsQueue.Dequeue();
			this._orderCharges = (EntityCollection<OrderChargeEntity>) collectionsQueue.Dequeue();
			this._orderItems = (EntityCollection<OrderItemEntity>) collectionsQueue.Dequeue();
			this._orderPaymentDetails = (EntityCollection<OrderPaymentDetailEntity>) collectionsQueue.Dequeue();
			this._orderSearch = (EntityCollection<OrderSearchEntity>) collectionsQueue.Dequeue();
			this._shipments = (EntityCollection<ShipmentEntity>) collectionsQueue.Dequeue();
			this._validatedAddress = (EntityCollection<ValidatedAddressEntity>) collectionsQueue.Dequeue();
			this._shipmentCollectionViaValidatedAddress = (EntityCollection<ShipmentEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._notes != null);
			toReturn |=(this._orderCharges != null);
			toReturn |=(this._orderItems != null);
			toReturn |=(this._orderPaymentDetails != null);
			toReturn |=(this._orderSearch != null);
			toReturn |=(this._shipments != null);
			toReturn |=(this._validatedAddress != null);
			toReturn |= (this._shipmentCollectionViaValidatedAddress != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<NoteEntity>(EntityFactoryCache2.GetEntityFactory(typeof(NoteEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<OrderChargeEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderChargeEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<OrderItemEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderItemEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<OrderPaymentDetailEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderPaymentDetailEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<OrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ShipmentEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ShipmentEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ValidatedAddressEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ValidatedAddressEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ShipmentEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ShipmentEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("Customer", _customer);
			toReturn.Add("Store", _store);
			toReturn.Add("Notes", _notes);
			toReturn.Add("OrderCharges", _orderCharges);
			toReturn.Add("OrderItems", _orderItems);
			toReturn.Add("OrderPaymentDetails", _orderPaymentDetails);
			toReturn.Add("OrderSearch", _orderSearch);
			toReturn.Add("Shipments", _shipments);
			toReturn.Add("ValidatedAddress", _validatedAddress);
			toReturn.Add("ShipmentCollectionViaValidatedAddress", _shipmentCollectionViaValidatedAddress);
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
			_fieldsCustomProperties.Add("OrderID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RowVersion", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("StoreID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CustomerID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OrderNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OrderNumberComplete", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OrderDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OrderTotal", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("LocalStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("IsManual", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OnlineLastModified", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OnlineCustomerID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OnlineStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OnlineStatusCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RequestedShipping", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillFirstName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillMiddleName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillLastName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillCompany", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillStreet1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillStreet2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillStreet3", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillCity", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillStateProvCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillPostalCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillCountryCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillPhone", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillFax", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillEmail", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillWebsite", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillAddressValidationSuggestionCount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillAddressValidationStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillAddressValidationError", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillResidentialStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillPOBox", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillUSTerritory", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillMilitaryAddress", fieldHashtable);
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
			_fieldsCustomProperties.Add("ShipFax", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipEmail", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipWebsite", fieldHashtable);
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
			_fieldsCustomProperties.Add("RollupItemCount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupItemName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupItemCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupItemSKU", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupItemLocation", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupItemQuantity", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupItemTotalWeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupNoteCount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillNameParseStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillUnparsedName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipNameParseStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipUnparsedName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipSenseHashKey", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipSenseRecognitionStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipAddressType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CombineSplitStatus", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _customer</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncCustomer(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _customer, new PropertyChangedEventHandler( OnCustomerPropertyChanged ), "Customer", ShipWorks.Data.Model.RelationClasses.StaticOrderRelations.CustomerEntityUsingCustomerIDStatic, true, signalRelatedEntity, "Order", resetFKFields, new int[] { (int)OrderFieldIndex.CustomerID } );
			_customer = null;
		}

		/// <summary> setups the sync logic for member _customer</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncCustomer(IEntityCore relatedEntity)
		{
			if(_customer!=relatedEntity)
			{
				DesetupSyncCustomer(true, true);
				_customer = (CustomerEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _customer, new PropertyChangedEventHandler( OnCustomerPropertyChanged ), "Customer", ShipWorks.Data.Model.RelationClasses.StaticOrderRelations.CustomerEntityUsingCustomerIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnCustomerPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _store</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncStore(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _store, new PropertyChangedEventHandler( OnStorePropertyChanged ), "Store", ShipWorks.Data.Model.RelationClasses.StaticOrderRelations.StoreEntityUsingStoreIDStatic, true, signalRelatedEntity, "", resetFKFields, new int[] { (int)OrderFieldIndex.StoreID } );
			_store = null;
		}

		/// <summary> setups the sync logic for member _store</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncStore(IEntityCore relatedEntity)
		{
			if(_store!=relatedEntity)
			{
				DesetupSyncStore(true, true);
				_store = (StoreEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _store, new PropertyChangedEventHandler( OnStorePropertyChanged ), "Store", ShipWorks.Data.Model.RelationClasses.StaticOrderRelations.StoreEntityUsingStoreIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnStorePropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this OrderEntity</param>
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
		public  static OrderRelations Relations
		{
			get	{ return new OrderRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Note' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathNotes
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<NoteEntity>(EntityFactoryCache2.GetEntityFactory(typeof(NoteEntityFactory))), (IEntityRelation)GetRelationsForField("Notes")[0], (int)ShipWorks.Data.Model.EntityType.OrderEntity, (int)ShipWorks.Data.Model.EntityType.NoteEntity, 0, null, null, null, null, "Notes", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'OrderCharge' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathOrderCharges
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<OrderChargeEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderChargeEntityFactory))), (IEntityRelation)GetRelationsForField("OrderCharges")[0], (int)ShipWorks.Data.Model.EntityType.OrderEntity, (int)ShipWorks.Data.Model.EntityType.OrderChargeEntity, 0, null, null, null, null, "OrderCharges", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'OrderItem' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathOrderItems
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<OrderItemEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderItemEntityFactory))), (IEntityRelation)GetRelationsForField("OrderItems")[0], (int)ShipWorks.Data.Model.EntityType.OrderEntity, (int)ShipWorks.Data.Model.EntityType.OrderItemEntity, 0, null, null, null, null, "OrderItems", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'OrderPaymentDetail' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathOrderPaymentDetails
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<OrderPaymentDetailEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderPaymentDetailEntityFactory))), (IEntityRelation)GetRelationsForField("OrderPaymentDetails")[0], (int)ShipWorks.Data.Model.EntityType.OrderEntity, (int)ShipWorks.Data.Model.EntityType.OrderPaymentDetailEntity, 0, null, null, null, null, "OrderPaymentDetails", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'OrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<OrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("OrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.OrderEntity, (int)ShipWorks.Data.Model.EntityType.OrderSearchEntity, 0, null, null, null, null, "OrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Shipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathShipments
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<ShipmentEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ShipmentEntityFactory))), (IEntityRelation)GetRelationsForField("Shipments")[0], (int)ShipWorks.Data.Model.EntityType.OrderEntity, (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, 0, null, null, null, null, "Shipments", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ValidatedAddress' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathValidatedAddress
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<ValidatedAddressEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ValidatedAddressEntityFactory))), (IEntityRelation)GetRelationsForField("ValidatedAddress")[0], (int)ShipWorks.Data.Model.EntityType.OrderEntity, (int)ShipWorks.Data.Model.EntityType.ValidatedAddressEntity, 0, null, null, null, null, "ValidatedAddress", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Shipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathShipmentCollectionViaValidatedAddress
		{
			get
			{
				IEntityRelation intermediateRelation = Relations.ValidatedAddressEntityUsingConsumerID;
				intermediateRelation.SetAliases(string.Empty, "ValidatedAddress_");
				return new PrefetchPathElement2(new EntityCollection<ShipmentEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ShipmentEntityFactory))), intermediateRelation,
					(int)ShipWorks.Data.Model.EntityType.OrderEntity, (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, 0, null, null, GetRelationsForField("ShipmentCollectionViaValidatedAddress"), null, "ShipmentCollectionViaValidatedAddress", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToMany);
			}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Customer' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathCustomer
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(CustomerEntityFactory))),	(IEntityRelation)GetRelationsForField("Customer")[0], (int)ShipWorks.Data.Model.EntityType.OrderEntity, (int)ShipWorks.Data.Model.EntityType.CustomerEntity, 0, null, null, null, null, "Customer", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Store' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathStore
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(StoreEntityFactory))),	(IEntityRelation)GetRelationsForField("Store")[0], (int)ShipWorks.Data.Model.EntityType.OrderEntity, (int)ShipWorks.Data.Model.EntityType.StoreEntity, 0, null, null, null, null, "Store", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The OrderID property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."OrderID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 OrderID
		{
			get { return (System.Int64)GetValue((int)OrderFieldIndex.OrderID, true); }
			set	{ SetValue((int)OrderFieldIndex.OrderID, value); }
		}

		/// <summary> The RowVersion property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."RowVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] RowVersion
		{
			get { return (System.Byte[])GetValue((int)OrderFieldIndex.RowVersion, true); }

		}

		/// <summary> The StoreID property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."StoreID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 StoreID
		{
			get { return (System.Int64)GetValue((int)OrderFieldIndex.StoreID, true); }
			set	{ SetValue((int)OrderFieldIndex.StoreID, value); }
		}

		/// <summary> The CustomerID property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."CustomerID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 CustomerID
		{
			get { return (System.Int64)GetValue((int)OrderFieldIndex.CustomerID, true); }
			set	{ SetValue((int)OrderFieldIndex.CustomerID, value); }
		}

		/// <summary> The OrderNumber property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."OrderNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 OrderNumber
		{
			get { return (System.Int64)GetValue((int)OrderFieldIndex.OrderNumber, true); }
			set	{ SetValue((int)OrderFieldIndex.OrderNumber, value); }
		}

		/// <summary> The OrderNumberComplete property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."OrderNumberComplete"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OrderNumberComplete
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.OrderNumberComplete, true); }
			set	{ SetValue((int)OrderFieldIndex.OrderNumberComplete, value); }
		}

		/// <summary> The OrderDate property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."OrderDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime OrderDate
		{
			get { return (System.DateTime)GetValue((int)OrderFieldIndex.OrderDate, true); }
			set	{ SetValue((int)OrderFieldIndex.OrderDate, value); }
		}

		/// <summary> The OrderTotal property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."OrderTotal"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal OrderTotal
		{
			get { return (System.Decimal)GetValue((int)OrderFieldIndex.OrderTotal, true); }
			set	{ SetValue((int)OrderFieldIndex.OrderTotal, value); }
		}

		/// <summary> The LocalStatus property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."LocalStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String LocalStatus
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.LocalStatus, true); }
			set	{ SetValue((int)OrderFieldIndex.LocalStatus, value); }
		}

		/// <summary> The IsManual property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."IsManual"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean IsManual
		{
			get { return (System.Boolean)GetValue((int)OrderFieldIndex.IsManual, true); }
			set	{ SetValue((int)OrderFieldIndex.IsManual, value); }
		}

		/// <summary> The OnlineLastModified property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."OnlineLastModified"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime2, 7, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime OnlineLastModified
		{
			get { return (System.DateTime)GetValue((int)OrderFieldIndex.OnlineLastModified, true); }
			set	{ SetValue((int)OrderFieldIndex.OnlineLastModified, value); }
		}

		/// <summary> The OnlineCustomerID property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."OnlineCustomerID"<br/>
		/// Table field type characteristics (type, precision, scale, length): Variant, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.Object OnlineCustomerID
		{
			get { return (System.Object)GetValue((int)OrderFieldIndex.OnlineCustomerID, true); }
			set	{ SetValue((int)OrderFieldIndex.OnlineCustomerID, value); }
		}

		/// <summary> The OnlineStatus property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."OnlineStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OnlineStatus
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.OnlineStatus, true); }
			set	{ SetValue((int)OrderFieldIndex.OnlineStatus, value); }
		}

		/// <summary> The OnlineStatusCode property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."OnlineStatusCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): Variant, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.Object OnlineStatusCode
		{
			get { return (System.Object)GetValue((int)OrderFieldIndex.OnlineStatusCode, true); }
			set	{ SetValue((int)OrderFieldIndex.OnlineStatusCode, value); }
		}

		/// <summary> The RequestedShipping property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."RequestedShipping"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String RequestedShipping
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.RequestedShipping, true); }
			set	{ SetValue((int)OrderFieldIndex.RequestedShipping, value); }
		}

		/// <summary> The BillFirstName property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillFirstName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillFirstName
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillFirstName, true); }
			set	{ SetValue((int)OrderFieldIndex.BillFirstName, value); }
		}

		/// <summary> The BillMiddleName property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillMiddleName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillMiddleName
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillMiddleName, true); }
			set	{ SetValue((int)OrderFieldIndex.BillMiddleName, value); }
		}

		/// <summary> The BillLastName property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillLastName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillLastName
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillLastName, true); }
			set	{ SetValue((int)OrderFieldIndex.BillLastName, value); }
		}

		/// <summary> The BillCompany property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillCompany"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillCompany
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillCompany, true); }
			set	{ SetValue((int)OrderFieldIndex.BillCompany, value); }
		}

		/// <summary> The BillStreet1 property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillStreet1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillStreet1
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillStreet1, true); }
			set	{ SetValue((int)OrderFieldIndex.BillStreet1, value); }
		}

		/// <summary> The BillStreet2 property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillStreet2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillStreet2
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillStreet2, true); }
			set	{ SetValue((int)OrderFieldIndex.BillStreet2, value); }
		}

		/// <summary> The BillStreet3 property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillStreet3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillStreet3
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillStreet3, true); }
			set	{ SetValue((int)OrderFieldIndex.BillStreet3, value); }
		}

		/// <summary> The BillCity property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillCity"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillCity
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillCity, true); }
			set	{ SetValue((int)OrderFieldIndex.BillCity, value); }
		}

		/// <summary> The BillStateProvCode property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillStateProvCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillStateProvCode
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillStateProvCode, true); }
			set	{ SetValue((int)OrderFieldIndex.BillStateProvCode, value); }
		}

		/// <summary> The BillPostalCode property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillPostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillPostalCode
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillPostalCode, true); }
			set	{ SetValue((int)OrderFieldIndex.BillPostalCode, value); }
		}

		/// <summary> The BillCountryCode property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillCountryCode
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillCountryCode, true); }
			set	{ SetValue((int)OrderFieldIndex.BillCountryCode, value); }
		}

		/// <summary> The BillPhone property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillPhone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillPhone
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillPhone, true); }
			set	{ SetValue((int)OrderFieldIndex.BillPhone, value); }
		}

		/// <summary> The BillFax property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillFax"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillFax
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillFax, true); }
			set	{ SetValue((int)OrderFieldIndex.BillFax, value); }
		}

		/// <summary> The BillEmail property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillEmail"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillEmail
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillEmail, true); }
			set	{ SetValue((int)OrderFieldIndex.BillEmail, value); }
		}

		/// <summary> The BillWebsite property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillWebsite"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillWebsite
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillWebsite, true); }
			set	{ SetValue((int)OrderFieldIndex.BillWebsite, value); }
		}

		/// <summary> The BillAddressValidationSuggestionCount property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillAddressValidationSuggestionCount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 BillAddressValidationSuggestionCount
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.BillAddressValidationSuggestionCount, true); }
			set	{ SetValue((int)OrderFieldIndex.BillAddressValidationSuggestionCount, value); }
		}

		/// <summary> The BillAddressValidationStatus property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillAddressValidationStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 BillAddressValidationStatus
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.BillAddressValidationStatus, true); }
			set	{ SetValue((int)OrderFieldIndex.BillAddressValidationStatus, value); }
		}

		/// <summary> The BillAddressValidationError property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillAddressValidationError"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillAddressValidationError
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillAddressValidationError, true); }
			set	{ SetValue((int)OrderFieldIndex.BillAddressValidationError, value); }
		}

		/// <summary> The BillResidentialStatus property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillResidentialStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 BillResidentialStatus
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.BillResidentialStatus, true); }
			set	{ SetValue((int)OrderFieldIndex.BillResidentialStatus, value); }
		}

		/// <summary> The BillPOBox property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillPOBox"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 BillPOBox
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.BillPOBox, true); }
			set	{ SetValue((int)OrderFieldIndex.BillPOBox, value); }
		}

		/// <summary> The BillUSTerritory property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillUSTerritory"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 BillUSTerritory
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.BillUSTerritory, true); }
			set	{ SetValue((int)OrderFieldIndex.BillUSTerritory, value); }
		}

		/// <summary> The BillMilitaryAddress property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillMilitaryAddress"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 BillMilitaryAddress
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.BillMilitaryAddress, true); }
			set	{ SetValue((int)OrderFieldIndex.BillMilitaryAddress, value); }
		}

		/// <summary> The ShipFirstName property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipFirstName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipFirstName
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipFirstName, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipFirstName, value); }
		}

		/// <summary> The ShipMiddleName property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipMiddleName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipMiddleName
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipMiddleName, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipMiddleName, value); }
		}

		/// <summary> The ShipLastName property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipLastName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipLastName
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipLastName, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipLastName, value); }
		}

		/// <summary> The ShipCompany property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipCompany"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipCompany
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipCompany, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipCompany, value); }
		}

		/// <summary> The ShipStreet1 property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipStreet1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipStreet1
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipStreet1, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipStreet1, value); }
		}

		/// <summary> The ShipStreet2 property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipStreet2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipStreet2
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipStreet2, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipStreet2, value); }
		}

		/// <summary> The ShipStreet3 property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipStreet3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipStreet3
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipStreet3, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipStreet3, value); }
		}

		/// <summary> The ShipCity property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipCity"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipCity
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipCity, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipCity, value); }
		}

		/// <summary> The ShipStateProvCode property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipStateProvCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipStateProvCode
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipStateProvCode, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipStateProvCode, value); }
		}

		/// <summary> The ShipPostalCode property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipPostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipPostalCode
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipPostalCode, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipPostalCode, value); }
		}

		/// <summary> The ShipCountryCode property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipCountryCode
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipCountryCode, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipCountryCode, value); }
		}

		/// <summary> The ShipPhone property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipPhone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipPhone
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipPhone, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipPhone, value); }
		}

		/// <summary> The ShipFax property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipFax"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipFax
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipFax, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipFax, value); }
		}

		/// <summary> The ShipEmail property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipEmail"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipEmail
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipEmail, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipEmail, value); }
		}

		/// <summary> The ShipWebsite property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipWebsite"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipWebsite
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipWebsite, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipWebsite, value); }
		}

		/// <summary> The ShipAddressValidationSuggestionCount property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipAddressValidationSuggestionCount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipAddressValidationSuggestionCount
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.ShipAddressValidationSuggestionCount, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipAddressValidationSuggestionCount, value); }
		}

		/// <summary> The ShipAddressValidationStatus property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipAddressValidationStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipAddressValidationStatus
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.ShipAddressValidationStatus, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipAddressValidationStatus, value); }
		}

		/// <summary> The ShipAddressValidationError property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipAddressValidationError"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipAddressValidationError
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipAddressValidationError, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipAddressValidationError, value); }
		}

		/// <summary> The ShipResidentialStatus property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipResidentialStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipResidentialStatus
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.ShipResidentialStatus, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipResidentialStatus, value); }
		}

		/// <summary> The ShipPOBox property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipPOBox"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipPOBox
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.ShipPOBox, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipPOBox, value); }
		}

		/// <summary> The ShipUSTerritory property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipUSTerritory"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipUSTerritory
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.ShipUSTerritory, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipUSTerritory, value); }
		}

		/// <summary> The ShipMilitaryAddress property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipMilitaryAddress"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipMilitaryAddress
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.ShipMilitaryAddress, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipMilitaryAddress, value); }
		}

		/// <summary> The RollupItemCount property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."RollupItemCount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 RollupItemCount
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.RollupItemCount, true); }
			set	{ SetValue((int)OrderFieldIndex.RollupItemCount, value); }
		}

		/// <summary> The RollupItemName property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."RollupItemName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String RollupItemName
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.RollupItemName, true); }

		}

		/// <summary> The RollupItemCode property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."RollupItemCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String RollupItemCode
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.RollupItemCode, true); }

		}

		/// <summary> The RollupItemSKU property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."RollupItemSKU"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String RollupItemSKU
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.RollupItemSKU, true); }

		}

		/// <summary> The RollupItemLocation property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."RollupItemLocation"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String RollupItemLocation
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.RollupItemLocation, true); }

		}

		/// <summary> The RollupItemQuantity property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."RollupItemQuantity"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> RollupItemQuantity
		{
			get { return (Nullable<System.Double>)GetValue((int)OrderFieldIndex.RollupItemQuantity, false); }

		}

		/// <summary> The RollupItemTotalWeight property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."RollupItemTotalWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double RollupItemTotalWeight
		{
			get { return (System.Double)GetValue((int)OrderFieldIndex.RollupItemTotalWeight, true); }
			set	{ SetValue((int)OrderFieldIndex.RollupItemTotalWeight, value); }
		}

		/// <summary> The RollupNoteCount property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."RollupNoteCount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 RollupNoteCount
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.RollupNoteCount, true); }
			set	{ SetValue((int)OrderFieldIndex.RollupNoteCount, value); }
		}

		/// <summary> The BillNameParseStatus property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillNameParseStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 BillNameParseStatus
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.BillNameParseStatus, true); }
			set	{ SetValue((int)OrderFieldIndex.BillNameParseStatus, value); }
		}

		/// <summary> The BillUnparsedName property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."BillUnparsedName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillUnparsedName
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.BillUnparsedName, true); }
			set	{ SetValue((int)OrderFieldIndex.BillUnparsedName, value); }
		}

		/// <summary> The ShipNameParseStatus property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipNameParseStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipNameParseStatus
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.ShipNameParseStatus, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipNameParseStatus, value); }
		}

		/// <summary> The ShipUnparsedName property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipUnparsedName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipUnparsedName
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipUnparsedName, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipUnparsedName, value); }
		}

		/// <summary> The ShipSenseHashKey property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipSenseHashKey"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipSenseHashKey
		{
			get { return (System.String)GetValue((int)OrderFieldIndex.ShipSenseHashKey, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipSenseHashKey, value); }
		}

		/// <summary> The ShipSenseRecognitionStatus property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipSenseRecognitionStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipSenseRecognitionStatus
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.ShipSenseRecognitionStatus, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipSenseRecognitionStatus, value); }
		}

		/// <summary> The ShipAddressType property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."ShipAddressType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipAddressType
		{
			get { return (System.Int32)GetValue((int)OrderFieldIndex.ShipAddressType, true); }
			set	{ SetValue((int)OrderFieldIndex.ShipAddressType, value); }
		}

		/// <summary> The CombineSplitStatus property of the Entity Order<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Order"."CombineSplitStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual Interapptive.Shared.Enums.CombineSplitStatusType CombineSplitStatus
		{
			get { return (Interapptive.Shared.Enums.CombineSplitStatusType)GetValue((int)OrderFieldIndex.CombineSplitStatus, true); }
			set	{ SetValue((int)OrderFieldIndex.CombineSplitStatus, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'NoteEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(NoteEntity))]
		public virtual EntityCollection<NoteEntity> Notes
		{
			get { return GetOrCreateEntityCollection<NoteEntity, NoteEntityFactory>("Order", true, false, ref _notes);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'OrderChargeEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(OrderChargeEntity))]
		public virtual EntityCollection<OrderChargeEntity> OrderCharges
		{
			get { return GetOrCreateEntityCollection<OrderChargeEntity, OrderChargeEntityFactory>("Order", true, false, ref _orderCharges);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'OrderItemEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(OrderItemEntity))]
		public virtual EntityCollection<OrderItemEntity> OrderItems
		{
			get { return GetOrCreateEntityCollection<OrderItemEntity, OrderItemEntityFactory>("Order", true, false, ref _orderItems);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'OrderPaymentDetailEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(OrderPaymentDetailEntity))]
		public virtual EntityCollection<OrderPaymentDetailEntity> OrderPaymentDetails
		{
			get { return GetOrCreateEntityCollection<OrderPaymentDetailEntity, OrderPaymentDetailEntityFactory>("Order", true, false, ref _orderPaymentDetails);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'OrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(OrderSearchEntity))]
		public virtual EntityCollection<OrderSearchEntity> OrderSearch
		{
			get { return GetOrCreateEntityCollection<OrderSearchEntity, OrderSearchEntityFactory>("Order", true, false, ref _orderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ShipmentEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ShipmentEntity))]
		public virtual EntityCollection<ShipmentEntity> Shipments
		{
			get { return GetOrCreateEntityCollection<ShipmentEntity, ShipmentEntityFactory>("Order", true, false, ref _shipments);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ValidatedAddressEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ValidatedAddressEntity))]
		public virtual EntityCollection<ValidatedAddressEntity> ValidatedAddress
		{
			get { return GetOrCreateEntityCollection<ValidatedAddressEntity, ValidatedAddressEntityFactory>("Order", true, false, ref _validatedAddress);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ShipmentEntity' which are related to this entity via a relation of type 'm:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ShipmentEntity))]
		public virtual EntityCollection<ShipmentEntity> ShipmentCollectionViaValidatedAddress
		{
			get { return GetOrCreateEntityCollection<ShipmentEntity, ShipmentEntityFactory>("OrderCollectionViaValidatedAddress", false, true, ref _shipmentCollectionViaValidatedAddress);	}
		}

		/// <summary> Gets / sets related entity of type 'CustomerEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual CustomerEntity Customer
		{
			get	{ return _customer; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncCustomer(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "Order", "Customer", _customer, true); 
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'StoreEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual StoreEntity Store
		{
			get	{ return _store; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncStore(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "", "Store", _store, false); 
				}
			}
		}
	
		/// <summary> Gets the type of the hierarchy this entity is in. </summary>
		protected override InheritanceHierarchyType LLBLGenProIsInHierarchyOfType
		{
			get { return InheritanceHierarchyType.TargetPerEntity;}
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
			get { return (int)ShipWorks.Data.Model.EntityType.OrderEntity; }
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
