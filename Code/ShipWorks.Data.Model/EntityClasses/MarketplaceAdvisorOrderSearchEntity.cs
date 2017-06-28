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
	/// <summary>Entity class which represents the entity 'MarketplaceAdvisorOrderSearch'.<br/><br/></summary>
	[Serializable]
	public partial class MarketplaceAdvisorOrderSearchEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private MarketplaceAdvisorOrderEntity _marketplaceAdvisorOrder;
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
			/// <summary>Member name MarketplaceAdvisorOrder</summary>
			public static readonly string MarketplaceAdvisorOrder = "MarketplaceAdvisorOrder";
			/// <summary>Member name Store</summary>
			public static readonly string Store = "Store";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static MarketplaceAdvisorOrderSearchEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public MarketplaceAdvisorOrderSearchEntity():base("MarketplaceAdvisorOrderSearchEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public MarketplaceAdvisorOrderSearchEntity(IEntityFields2 fields):base("MarketplaceAdvisorOrderSearchEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this MarketplaceAdvisorOrderSearchEntity</param>
		public MarketplaceAdvisorOrderSearchEntity(IValidator validator):base("MarketplaceAdvisorOrderSearchEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="marketplaceAdvisorOrderSearchID">PK value for MarketplaceAdvisorOrderSearch which data should be fetched into this MarketplaceAdvisorOrderSearch object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public MarketplaceAdvisorOrderSearchEntity(System.Int64 marketplaceAdvisorOrderSearchID):base("MarketplaceAdvisorOrderSearchEntity")
		{
			InitClassEmpty(null, null);
			this.MarketplaceAdvisorOrderSearchID = marketplaceAdvisorOrderSearchID;
		}

		/// <summary> CTor</summary>
		/// <param name="marketplaceAdvisorOrderSearchID">PK value for MarketplaceAdvisorOrderSearch which data should be fetched into this MarketplaceAdvisorOrderSearch object</param>
		/// <param name="validator">The custom validator object for this MarketplaceAdvisorOrderSearchEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public MarketplaceAdvisorOrderSearchEntity(System.Int64 marketplaceAdvisorOrderSearchID, IValidator validator):base("MarketplaceAdvisorOrderSearchEntity")
		{
			InitClassEmpty(validator, null);
			this.MarketplaceAdvisorOrderSearchID = marketplaceAdvisorOrderSearchID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected MarketplaceAdvisorOrderSearchEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_marketplaceAdvisorOrder = (MarketplaceAdvisorOrderEntity)info.GetValue("_marketplaceAdvisorOrder", typeof(MarketplaceAdvisorOrderEntity));
				if(_marketplaceAdvisorOrder!=null)
				{
					_marketplaceAdvisorOrder.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((MarketplaceAdvisorOrderSearchFieldIndex)fieldIndex)
			{
				case MarketplaceAdvisorOrderSearchFieldIndex.OrderID:
					DesetupSyncMarketplaceAdvisorOrder(true, false);
					break;
				case MarketplaceAdvisorOrderSearchFieldIndex.StoreID:
					DesetupSyncStore(true, false);
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
				case "MarketplaceAdvisorOrder":
					this.MarketplaceAdvisorOrder = (MarketplaceAdvisorOrderEntity)entity;
					break;
				case "Store":
					this.Store = (StoreEntity)entity;
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
				case "MarketplaceAdvisorOrder":
					toReturn.Add(Relations.MarketplaceAdvisorOrderEntityUsingOrderID);
					break;
				case "Store":
					toReturn.Add(Relations.StoreEntityUsingStoreID);
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
				case "MarketplaceAdvisorOrder":
					SetupSyncMarketplaceAdvisorOrder(relatedEntity);
					break;
				case "Store":
					SetupSyncStore(relatedEntity);
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
				case "MarketplaceAdvisorOrder":
					DesetupSyncMarketplaceAdvisorOrder(false, true);
					break;
				case "Store":
					DesetupSyncStore(false, true);
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
			if(_marketplaceAdvisorOrder!=null)
			{
				toReturn.Add(_marketplaceAdvisorOrder);
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
				info.AddValue("_marketplaceAdvisorOrder", (!this.MarkedForDeletion?_marketplaceAdvisorOrder:null));
				info.AddValue("_store", (!this.MarkedForDeletion?_store:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new MarketplaceAdvisorOrderSearchRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'MarketplaceAdvisorOrder' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoMarketplaceAdvisorOrder()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(MarketplaceAdvisorOrderFields.OrderID, null, ComparisonOperator.Equal, this.OrderID));
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
			return EntityFactoryCache2.GetEntityFactory(typeof(MarketplaceAdvisorOrderSearchEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("MarketplaceAdvisorOrder", _marketplaceAdvisorOrder);
			toReturn.Add("Store", _store);
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
			_fieldsCustomProperties.Add("MarketplaceAdvisorOrderSearchID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OrderID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("StoreID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OrderNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OrderNumberComplete", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InvoiceNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SellerOrderNumber", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _marketplaceAdvisorOrder</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncMarketplaceAdvisorOrder(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _marketplaceAdvisorOrder, new PropertyChangedEventHandler( OnMarketplaceAdvisorOrderPropertyChanged ), "MarketplaceAdvisorOrder", ShipWorks.Data.Model.RelationClasses.StaticMarketplaceAdvisorOrderSearchRelations.MarketplaceAdvisorOrderEntityUsingOrderIDStatic, true, signalRelatedEntity, "MarketplaceAdvisorOrderSearch", resetFKFields, new int[] { (int)MarketplaceAdvisorOrderSearchFieldIndex.OrderID } );
			_marketplaceAdvisorOrder = null;
		}

		/// <summary> setups the sync logic for member _marketplaceAdvisorOrder</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncMarketplaceAdvisorOrder(IEntityCore relatedEntity)
		{
			if(_marketplaceAdvisorOrder!=relatedEntity)
			{
				DesetupSyncMarketplaceAdvisorOrder(true, true);
				_marketplaceAdvisorOrder = (MarketplaceAdvisorOrderEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _marketplaceAdvisorOrder, new PropertyChangedEventHandler( OnMarketplaceAdvisorOrderPropertyChanged ), "MarketplaceAdvisorOrder", ShipWorks.Data.Model.RelationClasses.StaticMarketplaceAdvisorOrderSearchRelations.MarketplaceAdvisorOrderEntityUsingOrderIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMarketplaceAdvisorOrderPropertyChanged( object sender, PropertyChangedEventArgs e )
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
			this.PerformDesetupSyncRelatedEntity( _store, new PropertyChangedEventHandler( OnStorePropertyChanged ), "Store", ShipWorks.Data.Model.RelationClasses.StaticMarketplaceAdvisorOrderSearchRelations.StoreEntityUsingStoreIDStatic, true, signalRelatedEntity, "MarketplaceAdvisorOrderSearch", resetFKFields, new int[] { (int)MarketplaceAdvisorOrderSearchFieldIndex.StoreID } );
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
				this.PerformSetupSyncRelatedEntity( _store, new PropertyChangedEventHandler( OnStorePropertyChanged ), "Store", ShipWorks.Data.Model.RelationClasses.StaticMarketplaceAdvisorOrderSearchRelations.StoreEntityUsingStoreIDStatic, true, new string[] {  } );
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
		/// <param name="validator">The validator object for this MarketplaceAdvisorOrderSearchEntity</param>
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
		public  static MarketplaceAdvisorOrderSearchRelations Relations
		{
			get	{ return new MarketplaceAdvisorOrderSearchRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'MarketplaceAdvisorOrder' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathMarketplaceAdvisorOrder
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(MarketplaceAdvisorOrderEntityFactory))),	(IEntityRelation)GetRelationsForField("MarketplaceAdvisorOrder")[0], (int)ShipWorks.Data.Model.EntityType.MarketplaceAdvisorOrderSearchEntity, (int)ShipWorks.Data.Model.EntityType.MarketplaceAdvisorOrderEntity, 0, null, null, null, null, "MarketplaceAdvisorOrder", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Store' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathStore
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(StoreEntityFactory))),	(IEntityRelation)GetRelationsForField("Store")[0], (int)ShipWorks.Data.Model.EntityType.MarketplaceAdvisorOrderSearchEntity, (int)ShipWorks.Data.Model.EntityType.StoreEntity, 0, null, null, null, null, "Store", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The MarketplaceAdvisorOrderSearchID property of the Entity MarketplaceAdvisorOrderSearch<br/><br/></summary>
		/// <remarks>Mapped on  table field: "MarketplaceAdvisorOrderSearch"."MarketplaceAdvisorOrderSearchID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 MarketplaceAdvisorOrderSearchID
		{
			get { return (System.Int64)GetValue((int)MarketplaceAdvisorOrderSearchFieldIndex.MarketplaceAdvisorOrderSearchID, true); }
			set	{ SetValue((int)MarketplaceAdvisorOrderSearchFieldIndex.MarketplaceAdvisorOrderSearchID, value); }
		}

		/// <summary> The OrderID property of the Entity MarketplaceAdvisorOrderSearch<br/><br/></summary>
		/// <remarks>Mapped on  table field: "MarketplaceAdvisorOrderSearch"."OrderID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 OrderID
		{
			get { return (System.Int64)GetValue((int)MarketplaceAdvisorOrderSearchFieldIndex.OrderID, true); }
			set	{ SetValue((int)MarketplaceAdvisorOrderSearchFieldIndex.OrderID, value); }
		}

		/// <summary> The StoreID property of the Entity MarketplaceAdvisorOrderSearch<br/><br/></summary>
		/// <remarks>Mapped on  table field: "MarketplaceAdvisorOrderSearch"."StoreID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 StoreID
		{
			get { return (System.Int64)GetValue((int)MarketplaceAdvisorOrderSearchFieldIndex.StoreID, true); }
			set	{ SetValue((int)MarketplaceAdvisorOrderSearchFieldIndex.StoreID, value); }
		}

		/// <summary> The OrderNumber property of the Entity MarketplaceAdvisorOrderSearch<br/><br/></summary>
		/// <remarks>Mapped on  table field: "MarketplaceAdvisorOrderSearch"."OrderNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 OrderNumber
		{
			get { return (System.Int64)GetValue((int)MarketplaceAdvisorOrderSearchFieldIndex.OrderNumber, true); }
			set	{ SetValue((int)MarketplaceAdvisorOrderSearchFieldIndex.OrderNumber, value); }
		}

		/// <summary> The OrderNumberComplete property of the Entity MarketplaceAdvisorOrderSearch<br/><br/></summary>
		/// <remarks>Mapped on  table field: "MarketplaceAdvisorOrderSearch"."OrderNumberComplete"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OrderNumberComplete
		{
			get { return (System.String)GetValue((int)MarketplaceAdvisorOrderSearchFieldIndex.OrderNumberComplete, true); }
			set	{ SetValue((int)MarketplaceAdvisorOrderSearchFieldIndex.OrderNumberComplete, value); }
		}

		/// <summary> The InvoiceNumber property of the Entity MarketplaceAdvisorOrderSearch<br/><br/></summary>
		/// <remarks>Mapped on  table field: "MarketplaceAdvisorOrderSearch"."InvoiceNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String InvoiceNumber
		{
			get { return (System.String)GetValue((int)MarketplaceAdvisorOrderSearchFieldIndex.InvoiceNumber, true); }
			set	{ SetValue((int)MarketplaceAdvisorOrderSearchFieldIndex.InvoiceNumber, value); }
		}

		/// <summary> The SellerOrderNumber property of the Entity MarketplaceAdvisorOrderSearch<br/><br/></summary>
		/// <remarks>Mapped on  table field: "MarketplaceAdvisorOrderSearch"."SellerOrderNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 SellerOrderNumber
		{
			get { return (System.Int64)GetValue((int)MarketplaceAdvisorOrderSearchFieldIndex.SellerOrderNumber, true); }
			set	{ SetValue((int)MarketplaceAdvisorOrderSearchFieldIndex.SellerOrderNumber, value); }
		}

		/// <summary> Gets / sets related entity of type 'MarketplaceAdvisorOrderEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual MarketplaceAdvisorOrderEntity MarketplaceAdvisorOrder
		{
			get	{ return _marketplaceAdvisorOrder; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncMarketplaceAdvisorOrder(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "MarketplaceAdvisorOrderSearch", "MarketplaceAdvisorOrder", _marketplaceAdvisorOrder, true); 
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
					SetSingleRelatedEntityNavigator(value, "MarketplaceAdvisorOrderSearch", "Store", _store, true); 
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
			get { return (int)ShipWorks.Data.Model.EntityType.MarketplaceAdvisorOrderSearchEntity; }
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
