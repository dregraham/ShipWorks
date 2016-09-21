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
	/// <summary>Entity class which represents the entity 'FilterSequence'.<br/><br/></summary>
	[Serializable]
	public partial class FilterSequenceEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<FilterNodeEntity> _nodesUsingSequence;
		private FilterEntity _filter;
		private FilterEntity _parent;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name Filter</summary>
			public static readonly string Filter = "Filter";
			/// <summary>Member name Parent</summary>
			public static readonly string Parent = "Parent";
			/// <summary>Member name NodesUsingSequence</summary>
			public static readonly string NodesUsingSequence = "NodesUsingSequence";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static FilterSequenceEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public FilterSequenceEntity():base("FilterSequenceEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public FilterSequenceEntity(IEntityFields2 fields):base("FilterSequenceEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this FilterSequenceEntity</param>
		public FilterSequenceEntity(IValidator validator):base("FilterSequenceEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="filterSequenceID">PK value for FilterSequence which data should be fetched into this FilterSequence object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public FilterSequenceEntity(System.Int64 filterSequenceID):base("FilterSequenceEntity")
		{
			InitClassEmpty(null, null);
			this.FilterSequenceID = filterSequenceID;
		}

		/// <summary> CTor</summary>
		/// <param name="filterSequenceID">PK value for FilterSequence which data should be fetched into this FilterSequence object</param>
		/// <param name="validator">The custom validator object for this FilterSequenceEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public FilterSequenceEntity(System.Int64 filterSequenceID, IValidator validator):base("FilterSequenceEntity")
		{
			InitClassEmpty(validator, null);
			this.FilterSequenceID = filterSequenceID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected FilterSequenceEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_nodesUsingSequence = (EntityCollection<FilterNodeEntity>)info.GetValue("_nodesUsingSequence", typeof(EntityCollection<FilterNodeEntity>));
				_filter = (FilterEntity)info.GetValue("_filter", typeof(FilterEntity));
				if(_filter!=null)
				{
					_filter.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_parent = (FilterEntity)info.GetValue("_parent", typeof(FilterEntity));
				if(_parent!=null)
				{
					_parent.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((FilterSequenceFieldIndex)fieldIndex)
			{
				case FilterSequenceFieldIndex.ParentFilterID:
					DesetupSyncParent(true, false);
					break;
				case FilterSequenceFieldIndex.FilterID:
					DesetupSyncFilter(true, false);
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
				case "Filter":
					this.Filter = (FilterEntity)entity;
					break;
				case "Parent":
					this.Parent = (FilterEntity)entity;
					break;
				case "NodesUsingSequence":
					this.NodesUsingSequence.Add((FilterNodeEntity)entity);
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
				case "Filter":
					toReturn.Add(Relations.FilterEntityUsingFilterID);
					break;
				case "Parent":
					toReturn.Add(Relations.FilterEntityUsingParentFilterID);
					break;
				case "NodesUsingSequence":
					toReturn.Add(Relations.FilterNodeEntityUsingFilterSequenceID);
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
				case "Filter":
					SetupSyncFilter(relatedEntity);
					break;
				case "Parent":
					SetupSyncParent(relatedEntity);
					break;
				case "NodesUsingSequence":
					this.NodesUsingSequence.Add((FilterNodeEntity)relatedEntity);
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
				case "Filter":
					DesetupSyncFilter(false, true);
					break;
				case "Parent":
					DesetupSyncParent(false, true);
					break;
				case "NodesUsingSequence":
					this.PerformRelatedEntityRemoval(this.NodesUsingSequence, relatedEntity, signalRelatedEntityManyToOne);
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
			if(_filter!=null)
			{
				toReturn.Add(_filter);
			}
			if(_parent!=null)
			{
				toReturn.Add(_parent);
			}
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.Add(this.NodesUsingSequence);
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
				info.AddValue("_nodesUsingSequence", ((_nodesUsingSequence!=null) && (_nodesUsingSequence.Count>0) && !this.MarkedForDeletion)?_nodesUsingSequence:null);
				info.AddValue("_filter", (!this.MarkedForDeletion?_filter:null));
				info.AddValue("_parent", (!this.MarkedForDeletion?_parent:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new FilterSequenceRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'FilterNode' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoNodesUsingSequence()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(FilterNodeFields.FilterSequenceID, null, ComparisonOperator.Equal, this.FilterSequenceID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'Filter' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoFilter()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(FilterFields.FilterID, null, ComparisonOperator.Equal, this.FilterID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'Filter' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoParent()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(FilterFields.FilterID, null, ComparisonOperator.Equal, this.ParentFilterID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(FilterSequenceEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._nodesUsingSequence);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._nodesUsingSequence = (EntityCollection<FilterNodeEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._nodesUsingSequence != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<FilterNodeEntity>(EntityFactoryCache2.GetEntityFactory(typeof(FilterNodeEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("Filter", _filter);
			toReturn.Add("Parent", _parent);
			toReturn.Add("NodesUsingSequence", _nodesUsingSequence);
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
			_fieldsCustomProperties.Add("FilterSequenceID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RowVersion", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ParentFilterID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FilterID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Position", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _filter</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncFilter(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _filter, new PropertyChangedEventHandler( OnFilterPropertyChanged ), "Filter", ShipWorks.Data.Model.RelationClasses.StaticFilterSequenceRelations.FilterEntityUsingFilterIDStatic, true, signalRelatedEntity, "UsedBySequences", resetFKFields, new int[] { (int)FilterSequenceFieldIndex.FilterID } );
			_filter = null;
		}

		/// <summary> setups the sync logic for member _filter</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncFilter(IEntityCore relatedEntity)
		{
			if(_filter!=relatedEntity)
			{
				DesetupSyncFilter(true, true);
				_filter = (FilterEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _filter, new PropertyChangedEventHandler( OnFilterPropertyChanged ), "Filter", ShipWorks.Data.Model.RelationClasses.StaticFilterSequenceRelations.FilterEntityUsingFilterIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnFilterPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _parent</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncParent(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _parent, new PropertyChangedEventHandler( OnParentPropertyChanged ), "Parent", ShipWorks.Data.Model.RelationClasses.StaticFilterSequenceRelations.FilterEntityUsingParentFilterIDStatic, true, signalRelatedEntity, "ChildSequences", resetFKFields, new int[] { (int)FilterSequenceFieldIndex.ParentFilterID } );
			_parent = null;
		}

		/// <summary> setups the sync logic for member _parent</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncParent(IEntityCore relatedEntity)
		{
			if(_parent!=relatedEntity)
			{
				DesetupSyncParent(true, true);
				_parent = (FilterEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _parent, new PropertyChangedEventHandler( OnParentPropertyChanged ), "Parent", ShipWorks.Data.Model.RelationClasses.StaticFilterSequenceRelations.FilterEntityUsingParentFilterIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnParentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this FilterSequenceEntity</param>
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
		public  static FilterSequenceRelations Relations
		{
			get	{ return new FilterSequenceRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'FilterNode' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathNodesUsingSequence
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<FilterNodeEntity>(EntityFactoryCache2.GetEntityFactory(typeof(FilterNodeEntityFactory))), (IEntityRelation)GetRelationsForField("NodesUsingSequence")[0], (int)ShipWorks.Data.Model.EntityType.FilterSequenceEntity, (int)ShipWorks.Data.Model.EntityType.FilterNodeEntity, 0, null, null, null, null, "NodesUsingSequence", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Filter' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathFilter
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(FilterEntityFactory))),	(IEntityRelation)GetRelationsForField("Filter")[0], (int)ShipWorks.Data.Model.EntityType.FilterSequenceEntity, (int)ShipWorks.Data.Model.EntityType.FilterEntity, 0, null, null, null, null, "Filter", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Filter' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathParent
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(FilterEntityFactory))),	(IEntityRelation)GetRelationsForField("Parent")[0], (int)ShipWorks.Data.Model.EntityType.FilterSequenceEntity, (int)ShipWorks.Data.Model.EntityType.FilterEntity, 0, null, null, null, null, "Parent", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The FilterSequenceID property of the Entity FilterSequence<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FilterSequence"."FilterSequenceID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 FilterSequenceID
		{
			get { return (System.Int64)GetValue((int)FilterSequenceFieldIndex.FilterSequenceID, true); }
			set	{ SetValue((int)FilterSequenceFieldIndex.FilterSequenceID, value); }
		}

		/// <summary> The RowVersion property of the Entity FilterSequence<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FilterSequence"."RowVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] RowVersion
		{
			get { return (System.Byte[])GetValue((int)FilterSequenceFieldIndex.RowVersion, true); }

		}

		/// <summary> The ParentFilterID property of the Entity FilterSequence<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FilterSequence"."ParentFilterID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> ParentFilterID
		{
			get { return (Nullable<System.Int64>)GetValue((int)FilterSequenceFieldIndex.ParentFilterID, false); }
			set	{ SetValue((int)FilterSequenceFieldIndex.ParentFilterID, value); }
		}

		/// <summary> The FilterID property of the Entity FilterSequence<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FilterSequence"."FilterID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 FilterID
		{
			get { return (System.Int64)GetValue((int)FilterSequenceFieldIndex.FilterID, true); }
			set	{ SetValue((int)FilterSequenceFieldIndex.FilterID, value); }
		}

		/// <summary> The Position property of the Entity FilterSequence<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FilterSequence"."Position"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 Position
		{
			get { return (System.Int32)GetValue((int)FilterSequenceFieldIndex.Position, true); }
			set	{ SetValue((int)FilterSequenceFieldIndex.Position, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'FilterNodeEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(FilterNodeEntity))]
		public virtual EntityCollection<FilterNodeEntity> NodesUsingSequence
		{
			get { return GetOrCreateEntityCollection<FilterNodeEntity, FilterNodeEntityFactory>("FilterSequence", true, false, ref _nodesUsingSequence);	}
		}

		/// <summary> Gets / sets related entity of type 'FilterEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual FilterEntity Filter
		{
			get	{ return _filter; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncFilter(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "UsedBySequences", "Filter", _filter, true); 
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'FilterEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual FilterEntity Parent
		{
			get	{ return _parent; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncParent(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "ChildSequences", "Parent", _parent, true); 
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
			get { return (int)ShipWorks.Data.Model.EntityType.FilterSequenceEntity; }
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
