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
	/// <summary>Entity class which represents the entity 'Customer'.<br/><br/></summary>
	[Serializable]
	public partial class CustomerEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<OrderEntity> _order;
		private EntityCollection<StoreEntity> _storeCollectionViaOrder;

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
			/// <summary>Member name StoreCollectionViaOrder</summary>
			public static readonly string StoreCollectionViaOrder = "StoreCollectionViaOrder";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static CustomerEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public CustomerEntity():base("CustomerEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public CustomerEntity(IEntityFields2 fields):base("CustomerEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this CustomerEntity</param>
		public CustomerEntity(IValidator validator):base("CustomerEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="customerID">PK value for Customer which data should be fetched into this Customer object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public CustomerEntity(System.Int64 customerID):base("CustomerEntity")
		{
			InitClassEmpty(null, null);
			this.CustomerID = customerID;
		}

		/// <summary> CTor</summary>
		/// <param name="customerID">PK value for Customer which data should be fetched into this Customer object</param>
		/// <param name="validator">The custom validator object for this CustomerEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public CustomerEntity(System.Int64 customerID, IValidator validator):base("CustomerEntity")
		{
			InitClassEmpty(validator, null);
			this.CustomerID = customerID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected CustomerEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_order = (EntityCollection<OrderEntity>)info.GetValue("_order", typeof(EntityCollection<OrderEntity>));
				_storeCollectionViaOrder = (EntityCollection<StoreEntity>)info.GetValue("_storeCollectionViaOrder", typeof(EntityCollection<StoreEntity>));
				this.FixupDeserialization(FieldInfoProviderSingleton.GetInstance());
			}
			// __LLBLGENPRO_USER_CODE_REGION_START DeserializationConstructor
			// __LLBLGENPRO_USER_CODE_REGION_END
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
					this.Order.Add((OrderEntity)entity);
					break;
				case "StoreCollectionViaOrder":
					this.StoreCollectionViaOrder.IsReadOnly = false;
					this.StoreCollectionViaOrder.Add((StoreEntity)entity);
					this.StoreCollectionViaOrder.IsReadOnly = true;
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
					toReturn.Add(Relations.OrderEntityUsingCustomerID);
					break;
				case "StoreCollectionViaOrder":
					toReturn.Add(Relations.OrderEntityUsingCustomerID, "CustomerEntity__", "Order_", JoinHint.None);
					toReturn.Add(OrderEntity.Relations.StoreEntityUsingStoreID, "Order_", string.Empty, JoinHint.None);
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
					this.Order.Add((OrderEntity)relatedEntity);
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
					this.PerformRelatedEntityRemoval(this.Order, relatedEntity, signalRelatedEntityManyToOne);
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
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.Add(this.Order);
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
				info.AddValue("_order", ((_order!=null) && (_order.Count>0) && !this.MarkedForDeletion)?_order:null);
				info.AddValue("_storeCollectionViaOrder", ((_storeCollectionViaOrder!=null) && (_storeCollectionViaOrder.Count>0) && !this.MarkedForDeletion)?_storeCollectionViaOrder:null);
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new CustomerRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'Order' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoOrder()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(OrderFields.CustomerID, null, ComparisonOperator.Equal, this.CustomerID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'Store' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoStoreCollectionViaOrder()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.Relations.AddRange(GetRelationsForFieldOfType("StoreCollectionViaOrder"));
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(CustomerFields.CustomerID, null, ComparisonOperator.Equal, this.CustomerID, "CustomerEntity__"));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(CustomerEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._order);
			collectionsQueue.Enqueue(this._storeCollectionViaOrder);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._order = (EntityCollection<OrderEntity>) collectionsQueue.Dequeue();
			this._storeCollectionViaOrder = (EntityCollection<StoreEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._order != null);
			toReturn |= (this._storeCollectionViaOrder != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<OrderEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<StoreEntity>(EntityFactoryCache2.GetEntityFactory(typeof(StoreEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("Order", _order);
			toReturn.Add("StoreCollectionViaOrder", _storeCollectionViaOrder);
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
			_fieldsCustomProperties.Add("CustomerID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RowVersion", fieldHashtable);
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
			_fieldsCustomProperties.Add("RollupOrderCount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupOrderTotal", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupNoteCount", fieldHashtable);
		}
		#endregion

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this CustomerEntity</param>
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
		public  static CustomerRelations Relations
		{
			get	{ return new CustomerRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Order' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathOrder
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<OrderEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderEntityFactory))), (IEntityRelation)GetRelationsForField("Order")[0], (int)ShipWorks.Data.Model.EntityType.CustomerEntity, (int)ShipWorks.Data.Model.EntityType.OrderEntity, 0, null, null, null, null, "Order", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Store' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathStoreCollectionViaOrder
		{
			get
			{
				IEntityRelation intermediateRelation = Relations.OrderEntityUsingCustomerID;
				intermediateRelation.SetAliases(string.Empty, "Order_");
				return new PrefetchPathElement2(new EntityCollection<StoreEntity>(EntityFactoryCache2.GetEntityFactory(typeof(StoreEntityFactory))), intermediateRelation,
					(int)ShipWorks.Data.Model.EntityType.CustomerEntity, (int)ShipWorks.Data.Model.EntityType.StoreEntity, 0, null, null, GetRelationsForField("StoreCollectionViaOrder"), null, "StoreCollectionViaOrder", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToMany);
			}
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

		/// <summary> The CustomerID property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."CustomerID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 CustomerID
		{
			get { return (System.Int64)GetValue((int)CustomerFieldIndex.CustomerID, true); }
			set	{ SetValue((int)CustomerFieldIndex.CustomerID, value); }
		}

		/// <summary> The RowVersion property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."RowVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] RowVersion
		{
			get { return (System.Byte[])GetValue((int)CustomerFieldIndex.RowVersion, true); }

		}

		/// <summary> The BillFirstName property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."BillFirstName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillFirstName
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.BillFirstName, true); }
			set	{ SetValue((int)CustomerFieldIndex.BillFirstName, value); }
		}

		/// <summary> The BillMiddleName property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."BillMiddleName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillMiddleName
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.BillMiddleName, true); }
			set	{ SetValue((int)CustomerFieldIndex.BillMiddleName, value); }
		}

		/// <summary> The BillLastName property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."BillLastName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillLastName
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.BillLastName, true); }
			set	{ SetValue((int)CustomerFieldIndex.BillLastName, value); }
		}

		/// <summary> The BillCompany property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."BillCompany"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillCompany
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.BillCompany, true); }
			set	{ SetValue((int)CustomerFieldIndex.BillCompany, value); }
		}

		/// <summary> The BillStreet1 property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."BillStreet1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillStreet1
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.BillStreet1, true); }
			set	{ SetValue((int)CustomerFieldIndex.BillStreet1, value); }
		}

		/// <summary> The BillStreet2 property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."BillStreet2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillStreet2
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.BillStreet2, true); }
			set	{ SetValue((int)CustomerFieldIndex.BillStreet2, value); }
		}

		/// <summary> The BillStreet3 property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."BillStreet3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillStreet3
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.BillStreet3, true); }
			set	{ SetValue((int)CustomerFieldIndex.BillStreet3, value); }
		}

		/// <summary> The BillCity property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."BillCity"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillCity
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.BillCity, true); }
			set	{ SetValue((int)CustomerFieldIndex.BillCity, value); }
		}

		/// <summary> The BillStateProvCode property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."BillStateProvCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillStateProvCode
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.BillStateProvCode, true); }
			set	{ SetValue((int)CustomerFieldIndex.BillStateProvCode, value); }
		}

		/// <summary> The BillPostalCode property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."BillPostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillPostalCode
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.BillPostalCode, true); }
			set	{ SetValue((int)CustomerFieldIndex.BillPostalCode, value); }
		}

		/// <summary> The BillCountryCode property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."BillCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillCountryCode
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.BillCountryCode, true); }
			set	{ SetValue((int)CustomerFieldIndex.BillCountryCode, value); }
		}

		/// <summary> The BillPhone property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."BillPhone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillPhone
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.BillPhone, true); }
			set	{ SetValue((int)CustomerFieldIndex.BillPhone, value); }
		}

		/// <summary> The BillFax property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."BillFax"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillFax
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.BillFax, true); }
			set	{ SetValue((int)CustomerFieldIndex.BillFax, value); }
		}

		/// <summary> The BillEmail property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."BillEmail"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillEmail
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.BillEmail, true); }
			set	{ SetValue((int)CustomerFieldIndex.BillEmail, value); }
		}

		/// <summary> The BillWebsite property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."BillWebsite"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillWebsite
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.BillWebsite, true); }
			set	{ SetValue((int)CustomerFieldIndex.BillWebsite, value); }
		}

		/// <summary> The ShipFirstName property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."ShipFirstName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipFirstName
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.ShipFirstName, true); }
			set	{ SetValue((int)CustomerFieldIndex.ShipFirstName, value); }
		}

		/// <summary> The ShipMiddleName property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."ShipMiddleName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipMiddleName
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.ShipMiddleName, true); }
			set	{ SetValue((int)CustomerFieldIndex.ShipMiddleName, value); }
		}

		/// <summary> The ShipLastName property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."ShipLastName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipLastName
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.ShipLastName, true); }
			set	{ SetValue((int)CustomerFieldIndex.ShipLastName, value); }
		}

		/// <summary> The ShipCompany property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."ShipCompany"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipCompany
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.ShipCompany, true); }
			set	{ SetValue((int)CustomerFieldIndex.ShipCompany, value); }
		}

		/// <summary> The ShipStreet1 property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."ShipStreet1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipStreet1
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.ShipStreet1, true); }
			set	{ SetValue((int)CustomerFieldIndex.ShipStreet1, value); }
		}

		/// <summary> The ShipStreet2 property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."ShipStreet2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipStreet2
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.ShipStreet2, true); }
			set	{ SetValue((int)CustomerFieldIndex.ShipStreet2, value); }
		}

		/// <summary> The ShipStreet3 property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."ShipStreet3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipStreet3
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.ShipStreet3, true); }
			set	{ SetValue((int)CustomerFieldIndex.ShipStreet3, value); }
		}

		/// <summary> The ShipCity property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."ShipCity"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipCity
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.ShipCity, true); }
			set	{ SetValue((int)CustomerFieldIndex.ShipCity, value); }
		}

		/// <summary> The ShipStateProvCode property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."ShipStateProvCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipStateProvCode
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.ShipStateProvCode, true); }
			set	{ SetValue((int)CustomerFieldIndex.ShipStateProvCode, value); }
		}

		/// <summary> The ShipPostalCode property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."ShipPostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipPostalCode
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.ShipPostalCode, true); }
			set	{ SetValue((int)CustomerFieldIndex.ShipPostalCode, value); }
		}

		/// <summary> The ShipCountryCode property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."ShipCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipCountryCode
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.ShipCountryCode, true); }
			set	{ SetValue((int)CustomerFieldIndex.ShipCountryCode, value); }
		}

		/// <summary> The ShipPhone property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."ShipPhone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipPhone
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.ShipPhone, true); }
			set	{ SetValue((int)CustomerFieldIndex.ShipPhone, value); }
		}

		/// <summary> The ShipFax property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."ShipFax"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipFax
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.ShipFax, true); }
			set	{ SetValue((int)CustomerFieldIndex.ShipFax, value); }
		}

		/// <summary> The ShipEmail property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."ShipEmail"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipEmail
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.ShipEmail, true); }
			set	{ SetValue((int)CustomerFieldIndex.ShipEmail, value); }
		}

		/// <summary> The ShipWebsite property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."ShipWebsite"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipWebsite
		{
			get { return (System.String)GetValue((int)CustomerFieldIndex.ShipWebsite, true); }
			set	{ SetValue((int)CustomerFieldIndex.ShipWebsite, value); }
		}

		/// <summary> The RollupOrderCount property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."RollupOrderCount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 RollupOrderCount
		{
			get { return (System.Int32)GetValue((int)CustomerFieldIndex.RollupOrderCount, true); }
			set	{ SetValue((int)CustomerFieldIndex.RollupOrderCount, value); }
		}

		/// <summary> The RollupOrderTotal property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."RollupOrderTotal"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal RollupOrderTotal
		{
			get { return (System.Decimal)GetValue((int)CustomerFieldIndex.RollupOrderTotal, true); }
			set	{ SetValue((int)CustomerFieldIndex.RollupOrderTotal, value); }
		}

		/// <summary> The RollupNoteCount property of the Entity Customer<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Customer"."RollupNoteCount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 RollupNoteCount
		{
			get { return (System.Int32)GetValue((int)CustomerFieldIndex.RollupNoteCount, true); }
			set	{ SetValue((int)CustomerFieldIndex.RollupNoteCount, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'OrderEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(OrderEntity))]
		public virtual EntityCollection<OrderEntity> Order
		{
			get { return GetOrCreateEntityCollection<OrderEntity, OrderEntityFactory>("Customer", true, false, ref _order);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'StoreEntity' which are related to this entity via a relation of type 'm:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(StoreEntity))]
		public virtual EntityCollection<StoreEntity> StoreCollectionViaOrder
		{
			get { return GetOrCreateEntityCollection<StoreEntity, StoreEntityFactory>("", false, true, ref _storeCollectionViaOrder);	}
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
			get { return (int)ShipWorks.Data.Model.EntityType.CustomerEntity; }
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
