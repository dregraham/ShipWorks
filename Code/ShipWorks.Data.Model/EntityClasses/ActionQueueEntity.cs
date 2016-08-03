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
	/// <summary>Entity class which represents the entity 'ActionQueue'.<br/><br/></summary>
	[Serializable]
	public partial class ActionQueueEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<ActionQueueSelectionEntity> _actionQueueSelection;
		private EntityCollection<ActionQueueStepEntity> _steps;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name ActionQueueSelection</summary>
			public static readonly string ActionQueueSelection = "ActionQueueSelection";
			/// <summary>Member name Steps</summary>
			public static readonly string Steps = "Steps";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static ActionQueueEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public ActionQueueEntity():base("ActionQueueEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public ActionQueueEntity(IEntityFields2 fields):base("ActionQueueEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this ActionQueueEntity</param>
		public ActionQueueEntity(IValidator validator):base("ActionQueueEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="actionQueueID">PK value for ActionQueue which data should be fetched into this ActionQueue object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ActionQueueEntity(System.Int64 actionQueueID):base("ActionQueueEntity")
		{
			InitClassEmpty(null, null);
			this.ActionQueueID = actionQueueID;
		}

		/// <summary> CTor</summary>
		/// <param name="actionQueueID">PK value for ActionQueue which data should be fetched into this ActionQueue object</param>
		/// <param name="validator">The custom validator object for this ActionQueueEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ActionQueueEntity(System.Int64 actionQueueID, IValidator validator):base("ActionQueueEntity")
		{
			InitClassEmpty(validator, null);
			this.ActionQueueID = actionQueueID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected ActionQueueEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_actionQueueSelection = (EntityCollection<ActionQueueSelectionEntity>)info.GetValue("_actionQueueSelection", typeof(EntityCollection<ActionQueueSelectionEntity>));
				_steps = (EntityCollection<ActionQueueStepEntity>)info.GetValue("_steps", typeof(EntityCollection<ActionQueueStepEntity>));
				this.FixupDeserialization(FieldInfoProviderSingleton.GetInstance());
			}
			// __LLBLGENPRO_USER_CODE_REGION_START DeserializationConstructor
			// __LLBLGENPRO_USER_CODE_REGION_END
		}

		
		/// <summary>Performs the desync setup when an FK field has been changed. The entity referenced based on the FK field will be dereferenced and sync info will be removed.</summary>
		/// <param name="fieldIndex">The fieldindex.</param>
		protected override void PerformDesyncSetupFKFieldChange(int fieldIndex)
		{
			switch((ActionQueueFieldIndex)fieldIndex)
			{
				case ActionQueueFieldIndex.ActionID:

					break;
				case ActionQueueFieldIndex.TriggerComputerID:

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
				case "ActionQueueSelection":
					this.ActionQueueSelection.Add((ActionQueueSelectionEntity)entity);
					break;
				case "Steps":
					this.Steps.Add((ActionQueueStepEntity)entity);
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
				case "ActionQueueSelection":
					toReturn.Add(Relations.ActionQueueSelectionEntityUsingActionQueueID);
					break;
				case "Steps":
					toReturn.Add(Relations.ActionQueueStepEntityUsingActionQueueID);
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
				case "ActionQueueSelection":
					this.ActionQueueSelection.Add((ActionQueueSelectionEntity)relatedEntity);
					break;
				case "Steps":
					this.Steps.Add((ActionQueueStepEntity)relatedEntity);
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
				case "ActionQueueSelection":
					this.PerformRelatedEntityRemoval(this.ActionQueueSelection, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "Steps":
					this.PerformRelatedEntityRemoval(this.Steps, relatedEntity, signalRelatedEntityManyToOne);
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
			toReturn.Add(this.ActionQueueSelection);
			toReturn.Add(this.Steps);
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
				info.AddValue("_actionQueueSelection", ((_actionQueueSelection!=null) && (_actionQueueSelection.Count>0) && !this.MarkedForDeletion)?_actionQueueSelection:null);
				info.AddValue("_steps", ((_steps!=null) && (_steps.Count>0) && !this.MarkedForDeletion)?_steps:null);
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new ActionQueueRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'ActionQueueSelection' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoActionQueueSelection()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ActionQueueSelectionFields.ActionQueueID, null, ComparisonOperator.Equal, this.ActionQueueID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'ActionQueueStep' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoSteps()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ActionQueueStepFields.ActionQueueID, null, ComparisonOperator.Equal, this.ActionQueueID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(ActionQueueEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._actionQueueSelection);
			collectionsQueue.Enqueue(this._steps);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._actionQueueSelection = (EntityCollection<ActionQueueSelectionEntity>) collectionsQueue.Dequeue();
			this._steps = (EntityCollection<ActionQueueStepEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._actionQueueSelection != null);
			toReturn |=(this._steps != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ActionQueueSelectionEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ActionQueueSelectionEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ActionQueueStepEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ActionQueueStepEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("ActionQueueSelection", _actionQueueSelection);
			toReturn.Add("Steps", _steps);
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
			_fieldsCustomProperties.Add("ActionQueueID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RowVersion", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ActionID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ActionName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ActionQueueType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ActionVersion", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("QueueVersion", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("TriggerDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("TriggerComputerID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InternalComputerLimitedList", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("EntityID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Status", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("NextStep", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ContextLock", fieldHashtable);
		}
		#endregion

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this ActionQueueEntity</param>
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
		public  static ActionQueueRelations Relations
		{
			get	{ return new ActionQueueRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ActionQueueSelection' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathActionQueueSelection
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<ActionQueueSelectionEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ActionQueueSelectionEntityFactory))), (IEntityRelation)GetRelationsForField("ActionQueueSelection")[0], (int)ShipWorks.Data.Model.EntityType.ActionQueueEntity, (int)ShipWorks.Data.Model.EntityType.ActionQueueSelectionEntity, 0, null, null, null, null, "ActionQueueSelection", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ActionQueueStep' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathSteps
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<ActionQueueStepEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ActionQueueStepEntityFactory))), (IEntityRelation)GetRelationsForField("Steps")[0], (int)ShipWorks.Data.Model.EntityType.ActionQueueEntity, (int)ShipWorks.Data.Model.EntityType.ActionQueueStepEntity, 0, null, null, null, null, "Steps", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
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

		/// <summary> The ActionQueueID property of the Entity ActionQueue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ActionQueue"."ActionQueueID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 ActionQueueID
		{
			get { return (System.Int64)GetValue((int)ActionQueueFieldIndex.ActionQueueID, true); }
			set	{ SetValue((int)ActionQueueFieldIndex.ActionQueueID, value); }
		}

		/// <summary> The RowVersion property of the Entity ActionQueue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ActionQueue"."RowVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] RowVersion
		{
			get { return (System.Byte[])GetValue((int)ActionQueueFieldIndex.RowVersion, true); }

		}

		/// <summary> The ActionID property of the Entity ActionQueue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ActionQueue"."ActionID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 ActionID
		{
			get { return (System.Int64)GetValue((int)ActionQueueFieldIndex.ActionID, true); }
			set	{ SetValue((int)ActionQueueFieldIndex.ActionID, value); }
		}

		/// <summary> The ActionName property of the Entity ActionQueue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ActionQueue"."ActionName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ActionName
		{
			get { return (System.String)GetValue((int)ActionQueueFieldIndex.ActionName, true); }
			set	{ SetValue((int)ActionQueueFieldIndex.ActionName, value); }
		}

		/// <summary> The ActionQueueType property of the Entity ActionQueue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ActionQueue"."ActionQueueType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ActionQueueType
		{
			get { return (System.Int32)GetValue((int)ActionQueueFieldIndex.ActionQueueType, true); }
			set	{ SetValue((int)ActionQueueFieldIndex.ActionQueueType, value); }
		}

		/// <summary> The ActionVersion property of the Entity ActionQueue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ActionQueue"."ActionVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): Binary, 0, 0, 8<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] ActionVersion
		{
			get { return (System.Byte[])GetValue((int)ActionQueueFieldIndex.ActionVersion, true); }
			set	{ SetValue((int)ActionQueueFieldIndex.ActionVersion, value); }
		}

		/// <summary> The QueueVersion property of the Entity ActionQueue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ActionQueue"."QueueVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): Binary, 0, 0, 8<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] QueueVersion
		{
			get { return (System.Byte[])GetValue((int)ActionQueueFieldIndex.QueueVersion, true); }

		}

		/// <summary> The TriggerDate property of the Entity ActionQueue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ActionQueue"."TriggerDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime TriggerDate
		{
			get { return (System.DateTime)GetValue((int)ActionQueueFieldIndex.TriggerDate, true); }
			set	{ SetValue((int)ActionQueueFieldIndex.TriggerDate, value); }
		}

		/// <summary> The TriggerComputerID property of the Entity ActionQueue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ActionQueue"."TriggerComputerID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 TriggerComputerID
		{
			get { return (System.Int64)GetValue((int)ActionQueueFieldIndex.TriggerComputerID, true); }
			set	{ SetValue((int)ActionQueueFieldIndex.TriggerComputerID, value); }
		}

		/// <summary> The InternalComputerLimitedList property of the Entity ActionQueue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ActionQueue"."ComputerLimitedList"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 150<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String InternalComputerLimitedList
		{
			get { return (System.String)GetValue((int)ActionQueueFieldIndex.InternalComputerLimitedList, true); }
			set	{ SetValue((int)ActionQueueFieldIndex.InternalComputerLimitedList, value); }
		}

		/// <summary> The EntityID property of the Entity ActionQueue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ActionQueue"."ObjectID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> EntityID
		{
			get { return (Nullable<System.Int64>)GetValue((int)ActionQueueFieldIndex.EntityID, false); }
			set	{ SetValue((int)ActionQueueFieldIndex.EntityID, value); }
		}

		/// <summary> The Status property of the Entity ActionQueue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ActionQueue"."Status"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 Status
		{
			get { return (System.Int32)GetValue((int)ActionQueueFieldIndex.Status, true); }
			set	{ SetValue((int)ActionQueueFieldIndex.Status, value); }
		}

		/// <summary> The NextStep property of the Entity ActionQueue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ActionQueue"."NextStep"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 NextStep
		{
			get { return (System.Int32)GetValue((int)ActionQueueFieldIndex.NextStep, true); }
			set	{ SetValue((int)ActionQueueFieldIndex.NextStep, value); }
		}

		/// <summary> The ContextLock property of the Entity ActionQueue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ActionQueue"."ContextLock"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 36<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String ContextLock
		{
			get { return (System.String)GetValue((int)ActionQueueFieldIndex.ContextLock, true); }
			set	{ SetValue((int)ActionQueueFieldIndex.ContextLock, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ActionQueueSelectionEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ActionQueueSelectionEntity))]
		public virtual EntityCollection<ActionQueueSelectionEntity> ActionQueueSelection
		{
			get { return GetOrCreateEntityCollection<ActionQueueSelectionEntity, ActionQueueSelectionEntityFactory>("ActionQueue", true, false, ref _actionQueueSelection);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ActionQueueStepEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ActionQueueStepEntity))]
		public virtual EntityCollection<ActionQueueStepEntity> Steps
		{
			get { return GetOrCreateEntityCollection<ActionQueueStepEntity, ActionQueueStepEntityFactory>("ActionQueue", true, false, ref _steps);	}
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
			get { return (int)ShipWorks.Data.Model.EntityType.ActionQueueEntity; }
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
