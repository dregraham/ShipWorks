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
	/// <summary>Entity class which represents the entity 'FilterNode'.<br/><br/></summary>
	[Serializable]
	public partial class FilterNodeEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<FilterNodeEntity> _childNodes;
		private FilterNodeEntity _parentNode;
		private FilterNodeContentEntity _filterNodeContent;
		private FilterSequenceEntity _filterSequence;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name ParentNode</summary>
			public static readonly string ParentNode = "ParentNode";
			/// <summary>Member name FilterNodeContent</summary>
			public static readonly string FilterNodeContent = "FilterNodeContent";
			/// <summary>Member name FilterSequence</summary>
			public static readonly string FilterSequence = "FilterSequence";
			/// <summary>Member name ChildNodes</summary>
			public static readonly string ChildNodes = "ChildNodes";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static FilterNodeEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public FilterNodeEntity():base("FilterNodeEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public FilterNodeEntity(IEntityFields2 fields):base("FilterNodeEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this FilterNodeEntity</param>
		public FilterNodeEntity(IValidator validator):base("FilterNodeEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="filterNodeID">PK value for FilterNode which data should be fetched into this FilterNode object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public FilterNodeEntity(System.Int64 filterNodeID):base("FilterNodeEntity")
		{
			InitClassEmpty(null, null);
			this.FilterNodeID = filterNodeID;
		}

		/// <summary> CTor</summary>
		/// <param name="filterNodeID">PK value for FilterNode which data should be fetched into this FilterNode object</param>
		/// <param name="validator">The custom validator object for this FilterNodeEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public FilterNodeEntity(System.Int64 filterNodeID, IValidator validator):base("FilterNodeEntity")
		{
			InitClassEmpty(validator, null);
			this.FilterNodeID = filterNodeID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected FilterNodeEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_childNodes = (EntityCollection<FilterNodeEntity>)info.GetValue("_childNodes", typeof(EntityCollection<FilterNodeEntity>));
				_parentNode = (FilterNodeEntity)info.GetValue("_parentNode", typeof(FilterNodeEntity));
				if(_parentNode!=null)
				{
					_parentNode.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_filterNodeContent = (FilterNodeContentEntity)info.GetValue("_filterNodeContent", typeof(FilterNodeContentEntity));
				if(_filterNodeContent!=null)
				{
					_filterNodeContent.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_filterSequence = (FilterSequenceEntity)info.GetValue("_filterSequence", typeof(FilterSequenceEntity));
				if(_filterSequence!=null)
				{
					_filterSequence.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((FilterNodeFieldIndex)fieldIndex)
			{
				case FilterNodeFieldIndex.ParentFilterNodeID:
					DesetupSyncParentNode(true, false);
					break;
				case FilterNodeFieldIndex.FilterSequenceID:
					DesetupSyncFilterSequence(true, false);
					break;
				case FilterNodeFieldIndex.FilterNodeContentID:
					DesetupSyncFilterNodeContent(true, false);
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
				case "ParentNode":
					this.ParentNode = (FilterNodeEntity)entity;
					break;
				case "FilterNodeContent":
					this.FilterNodeContent = (FilterNodeContentEntity)entity;
					break;
				case "FilterSequence":
					this.FilterSequence = (FilterSequenceEntity)entity;
					break;
				case "ChildNodes":
					this.ChildNodes.Add((FilterNodeEntity)entity);
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
				case "ParentNode":
					toReturn.Add(Relations.FilterNodeEntityUsingFilterNodeIDParentFilterNodeID);
					break;
				case "FilterNodeContent":
					toReturn.Add(Relations.FilterNodeContentEntityUsingFilterNodeContentID);
					break;
				case "FilterSequence":
					toReturn.Add(Relations.FilterSequenceEntityUsingFilterSequenceID);
					break;
				case "ChildNodes":
					toReturn.Add(Relations.FilterNodeEntityUsingParentFilterNodeID);
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
				case "FilterNodeContent":
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
				case "ParentNode":
					SetupSyncParentNode(relatedEntity);
					break;
				case "FilterNodeContent":
					SetupSyncFilterNodeContent(relatedEntity);
					break;
				case "FilterSequence":
					SetupSyncFilterSequence(relatedEntity);
					break;
				case "ChildNodes":
					this.ChildNodes.Add((FilterNodeEntity)relatedEntity);
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
				case "ParentNode":
					DesetupSyncParentNode(false, true);
					break;
				case "FilterNodeContent":
					DesetupSyncFilterNodeContent(false, true);
					break;
				case "FilterSequence":
					DesetupSyncFilterSequence(false, true);
					break;
				case "ChildNodes":
					this.PerformRelatedEntityRemoval(this.ChildNodes, relatedEntity, signalRelatedEntityManyToOne);
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
			if(_parentNode!=null)
			{
				toReturn.Add(_parentNode);
			}
			if(_filterNodeContent!=null)
			{
				toReturn.Add(_filterNodeContent);
			}
			if(_filterSequence!=null)
			{
				toReturn.Add(_filterSequence);
			}
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.Add(this.ChildNodes);
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
				info.AddValue("_childNodes", ((_childNodes!=null) && (_childNodes.Count>0) && !this.MarkedForDeletion)?_childNodes:null);
				info.AddValue("_parentNode", (!this.MarkedForDeletion?_parentNode:null));
				info.AddValue("_filterNodeContent", (!this.MarkedForDeletion?_filterNodeContent:null));
				info.AddValue("_filterSequence", (!this.MarkedForDeletion?_filterSequence:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new FilterNodeRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'FilterNode' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoChildNodes()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(FilterNodeFields.ParentFilterNodeID, null, ComparisonOperator.Equal, this.FilterNodeID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'FilterNode' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoParentNode()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(FilterNodeFields.FilterNodeID, null, ComparisonOperator.Equal, this.ParentFilterNodeID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'FilterNodeContent' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoFilterNodeContent()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(FilterNodeContentFields.FilterNodeContentID, null, ComparisonOperator.Equal, this.FilterNodeContentID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'FilterSequence' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoFilterSequence()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(FilterSequenceFields.FilterSequenceID, null, ComparisonOperator.Equal, this.FilterSequenceID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(FilterNodeEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._childNodes);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._childNodes = (EntityCollection<FilterNodeEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._childNodes != null);
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
			toReturn.Add("ParentNode", _parentNode);
			toReturn.Add("FilterNodeContent", _filterNodeContent);
			toReturn.Add("FilterSequence", _filterSequence);
			toReturn.Add("ChildNodes", _childNodes);
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
			_fieldsCustomProperties.Add("FilterNodeID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RowVersion", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ParentFilterNodeID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FilterSequenceID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FilterNodeContentID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Created", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Purpose", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _parentNode</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncParentNode(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _parentNode, new PropertyChangedEventHandler( OnParentNodePropertyChanged ), "ParentNode", ShipWorks.Data.Model.RelationClasses.StaticFilterNodeRelations.FilterNodeEntityUsingFilterNodeIDParentFilterNodeIDStatic, true, signalRelatedEntity, "ChildNodes", resetFKFields, new int[] { (int)FilterNodeFieldIndex.ParentFilterNodeID } );
			_parentNode = null;
		}

		/// <summary> setups the sync logic for member _parentNode</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncParentNode(IEntityCore relatedEntity)
		{
			if(_parentNode!=relatedEntity)
			{
				DesetupSyncParentNode(true, true);
				_parentNode = (FilterNodeEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _parentNode, new PropertyChangedEventHandler( OnParentNodePropertyChanged ), "ParentNode", ShipWorks.Data.Model.RelationClasses.StaticFilterNodeRelations.FilterNodeEntityUsingFilterNodeIDParentFilterNodeIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnParentNodePropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _filterNodeContent</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncFilterNodeContent(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _filterNodeContent, new PropertyChangedEventHandler( OnFilterNodeContentPropertyChanged ), "FilterNodeContent", ShipWorks.Data.Model.RelationClasses.StaticFilterNodeRelations.FilterNodeContentEntityUsingFilterNodeContentIDStatic, true, signalRelatedEntity, "", resetFKFields, new int[] { (int)FilterNodeFieldIndex.FilterNodeContentID } );
			_filterNodeContent = null;
		}

		/// <summary> setups the sync logic for member _filterNodeContent</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncFilterNodeContent(IEntityCore relatedEntity)
		{
			if(_filterNodeContent!=relatedEntity)
			{
				DesetupSyncFilterNodeContent(true, true);
				_filterNodeContent = (FilterNodeContentEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _filterNodeContent, new PropertyChangedEventHandler( OnFilterNodeContentPropertyChanged ), "FilterNodeContent", ShipWorks.Data.Model.RelationClasses.StaticFilterNodeRelations.FilterNodeContentEntityUsingFilterNodeContentIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnFilterNodeContentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _filterSequence</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncFilterSequence(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _filterSequence, new PropertyChangedEventHandler( OnFilterSequencePropertyChanged ), "FilterSequence", ShipWorks.Data.Model.RelationClasses.StaticFilterNodeRelations.FilterSequenceEntityUsingFilterSequenceIDStatic, true, signalRelatedEntity, "NodesUsingSequence", resetFKFields, new int[] { (int)FilterNodeFieldIndex.FilterSequenceID } );
			_filterSequence = null;
		}

		/// <summary> setups the sync logic for member _filterSequence</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncFilterSequence(IEntityCore relatedEntity)
		{
			if(_filterSequence!=relatedEntity)
			{
				DesetupSyncFilterSequence(true, true);
				_filterSequence = (FilterSequenceEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _filterSequence, new PropertyChangedEventHandler( OnFilterSequencePropertyChanged ), "FilterSequence", ShipWorks.Data.Model.RelationClasses.StaticFilterNodeRelations.FilterSequenceEntityUsingFilterSequenceIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnFilterSequencePropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this FilterNodeEntity</param>
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
		public  static FilterNodeRelations Relations
		{
			get	{ return new FilterNodeRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'FilterNode' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathChildNodes
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<FilterNodeEntity>(EntityFactoryCache2.GetEntityFactory(typeof(FilterNodeEntityFactory))), (IEntityRelation)GetRelationsForField("ChildNodes")[0], (int)ShipWorks.Data.Model.EntityType.FilterNodeEntity, (int)ShipWorks.Data.Model.EntityType.FilterNodeEntity, 0, null, null, null, null, "ChildNodes", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'FilterNode' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathParentNode
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(FilterNodeEntityFactory))),	(IEntityRelation)GetRelationsForField("ParentNode")[0], (int)ShipWorks.Data.Model.EntityType.FilterNodeEntity, (int)ShipWorks.Data.Model.EntityType.FilterNodeEntity, 0, null, null, null, null, "ParentNode", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'FilterNodeContent' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathFilterNodeContent
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(FilterNodeContentEntityFactory))),	(IEntityRelation)GetRelationsForField("FilterNodeContent")[0], (int)ShipWorks.Data.Model.EntityType.FilterNodeEntity, (int)ShipWorks.Data.Model.EntityType.FilterNodeContentEntity, 0, null, null, null, null, "FilterNodeContent", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'FilterSequence' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathFilterSequence
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(FilterSequenceEntityFactory))),	(IEntityRelation)GetRelationsForField("FilterSequence")[0], (int)ShipWorks.Data.Model.EntityType.FilterNodeEntity, (int)ShipWorks.Data.Model.EntityType.FilterSequenceEntity, 0, null, null, null, null, "FilterSequence", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The FilterNodeID property of the Entity FilterNode<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FilterNode"."FilterNodeID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 FilterNodeID
		{
			get { return (System.Int64)GetValue((int)FilterNodeFieldIndex.FilterNodeID, true); }
			set	{ SetValue((int)FilterNodeFieldIndex.FilterNodeID, value); }
		}

		/// <summary> The RowVersion property of the Entity FilterNode<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FilterNode"."RowVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] RowVersion
		{
			get { return (System.Byte[])GetValue((int)FilterNodeFieldIndex.RowVersion, true); }

		}

		/// <summary> The ParentFilterNodeID property of the Entity FilterNode<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FilterNode"."ParentFilterNodeID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> ParentFilterNodeID
		{
			get { return (Nullable<System.Int64>)GetValue((int)FilterNodeFieldIndex.ParentFilterNodeID, false); }
			set	{ SetValue((int)FilterNodeFieldIndex.ParentFilterNodeID, value); }
		}

		/// <summary> The FilterSequenceID property of the Entity FilterNode<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FilterNode"."FilterSequenceID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 FilterSequenceID
		{
			get { return (System.Int64)GetValue((int)FilterNodeFieldIndex.FilterSequenceID, true); }
			set	{ SetValue((int)FilterNodeFieldIndex.FilterSequenceID, value); }
		}

		/// <summary> The FilterNodeContentID property of the Entity FilterNode<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FilterNode"."FilterNodeContentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 FilterNodeContentID
		{
			get { return (System.Int64)GetValue((int)FilterNodeFieldIndex.FilterNodeContentID, true); }
			set	{ SetValue((int)FilterNodeFieldIndex.FilterNodeContentID, value); }
		}

		/// <summary> The Created property of the Entity FilterNode<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FilterNode"."Created"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime Created
		{
			get { return (System.DateTime)GetValue((int)FilterNodeFieldIndex.Created, true); }
			set	{ SetValue((int)FilterNodeFieldIndex.Created, value); }
		}

		/// <summary> The Purpose property of the Entity FilterNode<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FilterNode"."Purpose"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 Purpose
		{
			get { return (System.Int32)GetValue((int)FilterNodeFieldIndex.Purpose, true); }
			set	{ SetValue((int)FilterNodeFieldIndex.Purpose, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'FilterNodeEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(FilterNodeEntity))]
		public virtual EntityCollection<FilterNodeEntity> ChildNodes
		{
			get { return GetOrCreateEntityCollection<FilterNodeEntity, FilterNodeEntityFactory>("ParentNode", true, false, ref _childNodes);	}
		}

		/// <summary> Gets / sets related entity of type 'FilterNodeEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual FilterNodeEntity ParentNode
		{
			get	{ return _parentNode; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncParentNode(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "ChildNodes", "ParentNode", _parentNode, true); 
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'FilterNodeContentEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual FilterNodeContentEntity FilterNodeContent
		{
			get	{ return _filterNodeContent; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncFilterNodeContent(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "", "FilterNodeContent", _filterNodeContent, false); 
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'FilterSequenceEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual FilterSequenceEntity FilterSequence
		{
			get	{ return _filterSequence; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncFilterSequence(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "NodesUsingSequence", "FilterSequence", _filterSequence, true); 
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
			get { return (int)ShipWorks.Data.Model.EntityType.FilterNodeEntity; }
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
