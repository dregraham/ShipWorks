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
	/// <summary>Entity class which represents the entity 'WorldShipProcessed'.<br/><br/></summary>
	[Serializable]
	public partial class WorldShipProcessedEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private WorldShipShipmentEntity _worldShipShipment;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name WorldShipShipment</summary>
			public static readonly string WorldShipShipment = "WorldShipShipment";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static WorldShipProcessedEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public WorldShipProcessedEntity():base("WorldShipProcessedEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public WorldShipProcessedEntity(IEntityFields2 fields):base("WorldShipProcessedEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this WorldShipProcessedEntity</param>
		public WorldShipProcessedEntity(IValidator validator):base("WorldShipProcessedEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="worldShipProcessedID">PK value for WorldShipProcessed which data should be fetched into this WorldShipProcessed object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public WorldShipProcessedEntity(System.Int64 worldShipProcessedID):base("WorldShipProcessedEntity")
		{
			InitClassEmpty(null, null);
			this.WorldShipProcessedID = worldShipProcessedID;
		}

		/// <summary> CTor</summary>
		/// <param name="worldShipProcessedID">PK value for WorldShipProcessed which data should be fetched into this WorldShipProcessed object</param>
		/// <param name="validator">The custom validator object for this WorldShipProcessedEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public WorldShipProcessedEntity(System.Int64 worldShipProcessedID, IValidator validator):base("WorldShipProcessedEntity")
		{
			InitClassEmpty(validator, null);
			this.WorldShipProcessedID = worldShipProcessedID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected WorldShipProcessedEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_worldShipShipment = (WorldShipShipmentEntity)info.GetValue("_worldShipShipment", typeof(WorldShipShipmentEntity));
				if(_worldShipShipment!=null)
				{
					_worldShipShipment.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((WorldShipProcessedFieldIndex)fieldIndex)
			{
				case WorldShipProcessedFieldIndex.ShipmentIdCalculated:
					DesetupSyncWorldShipShipment(true, false);
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
				case "WorldShipShipment":
					this.WorldShipShipment = (WorldShipShipmentEntity)entity;
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
				case "WorldShipShipment":
					toReturn.Add(Relations.WorldShipShipmentEntityUsingShipmentIdCalculated);
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
				case "WorldShipShipment":
					SetupSyncWorldShipShipment(relatedEntity);
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
				case "WorldShipShipment":
					DesetupSyncWorldShipShipment(false, true);
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
			if(_worldShipShipment!=null)
			{
				toReturn.Add(_worldShipShipment);
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
				info.AddValue("_worldShipShipment", (!this.MarkedForDeletion?_worldShipShipment:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new WorldShipProcessedRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'WorldShipShipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoWorldShipShipment()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(WorldShipShipmentFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentIdCalculated));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(WorldShipProcessedEntityFactory));
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
			toReturn.Add("WorldShipShipment", _worldShipShipment);
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
			_fieldsCustomProperties.Add("WorldShipProcessedID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipmentID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RowVersion", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PublishedCharges", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("NegotiatedCharges", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("TrackingNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("UspsTrackingNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ServiceType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PackageType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("UpsPackageID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DeclaredValueAmount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DeclaredValueOption", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("WorldShipShipmentID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("VoidIndicator", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("NumberOfPackages", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("LeadTrackingNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipmentIdCalculated", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _worldShipShipment</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncWorldShipShipment(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _worldShipShipment, new PropertyChangedEventHandler( OnWorldShipShipmentPropertyChanged ), "WorldShipShipment", ShipWorks.Data.Model.RelationClasses.StaticWorldShipProcessedRelations.WorldShipShipmentEntityUsingShipmentIdCalculatedStatic, true, signalRelatedEntity, "WorldShipProcessed", resetFKFields, new int[] { (int)WorldShipProcessedFieldIndex.ShipmentIdCalculated } );
			_worldShipShipment = null;
		}

		/// <summary> setups the sync logic for member _worldShipShipment</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncWorldShipShipment(IEntityCore relatedEntity)
		{
			if(_worldShipShipment!=relatedEntity)
			{
				DesetupSyncWorldShipShipment(true, true);
				_worldShipShipment = (WorldShipShipmentEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _worldShipShipment, new PropertyChangedEventHandler( OnWorldShipShipmentPropertyChanged ), "WorldShipShipment", ShipWorks.Data.Model.RelationClasses.StaticWorldShipProcessedRelations.WorldShipShipmentEntityUsingShipmentIdCalculatedStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnWorldShipShipmentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this WorldShipProcessedEntity</param>
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
		public  static WorldShipProcessedRelations Relations
		{
			get	{ return new WorldShipProcessedRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'WorldShipShipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathWorldShipShipment
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(WorldShipShipmentEntityFactory))),	(IEntityRelation)GetRelationsForField("WorldShipShipment")[0], (int)ShipWorks.Data.Model.EntityType.WorldShipProcessedEntity, (int)ShipWorks.Data.Model.EntityType.WorldShipShipmentEntity, 0, null, null, null, null, "WorldShipShipment", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The WorldShipProcessedID property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."WorldShipProcessedID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 WorldShipProcessedID
		{
			get { return (System.Int64)GetValue((int)WorldShipProcessedFieldIndex.WorldShipProcessedID, true); }
			set	{ SetValue((int)WorldShipProcessedFieldIndex.WorldShipProcessedID, value); }
		}

		/// <summary> The ShipmentID property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."ShipmentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String ShipmentID
		{
			get { return (System.String)GetValue((int)WorldShipProcessedFieldIndex.ShipmentID, true); }
			set	{ SetValue((int)WorldShipProcessedFieldIndex.ShipmentID, value); }
		}

		/// <summary> The RowVersion property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."RowVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] RowVersion
		{
			get { return (System.Byte[])GetValue((int)WorldShipProcessedFieldIndex.RowVersion, true); }

		}

		/// <summary> The PublishedCharges property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."PublishedCharges"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double PublishedCharges
		{
			get { return (System.Double)GetValue((int)WorldShipProcessedFieldIndex.PublishedCharges, true); }
			set	{ SetValue((int)WorldShipProcessedFieldIndex.PublishedCharges, value); }
		}

		/// <summary> The NegotiatedCharges property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."NegotiatedCharges"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double NegotiatedCharges
		{
			get { return (System.Double)GetValue((int)WorldShipProcessedFieldIndex.NegotiatedCharges, true); }
			set	{ SetValue((int)WorldShipProcessedFieldIndex.NegotiatedCharges, value); }
		}

		/// <summary> The TrackingNumber property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."TrackingNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String TrackingNumber
		{
			get { return (System.String)GetValue((int)WorldShipProcessedFieldIndex.TrackingNumber, true); }
			set	{ SetValue((int)WorldShipProcessedFieldIndex.TrackingNumber, value); }
		}

		/// <summary> The UspsTrackingNumber property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."UspsTrackingNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String UspsTrackingNumber
		{
			get { return (System.String)GetValue((int)WorldShipProcessedFieldIndex.UspsTrackingNumber, true); }
			set	{ SetValue((int)WorldShipProcessedFieldIndex.UspsTrackingNumber, value); }
		}

		/// <summary> The ServiceType property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."ServiceType"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String ServiceType
		{
			get { return (System.String)GetValue((int)WorldShipProcessedFieldIndex.ServiceType, true); }
			set	{ SetValue((int)WorldShipProcessedFieldIndex.ServiceType, value); }
		}

		/// <summary> The PackageType property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."PackageType"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String PackageType
		{
			get { return (System.String)GetValue((int)WorldShipProcessedFieldIndex.PackageType, true); }
			set	{ SetValue((int)WorldShipProcessedFieldIndex.PackageType, value); }
		}

		/// <summary> The UpsPackageID property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."UpsPackageID"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String UpsPackageID
		{
			get { return (System.String)GetValue((int)WorldShipProcessedFieldIndex.UpsPackageID, true); }
			set	{ SetValue((int)WorldShipProcessedFieldIndex.UpsPackageID, value); }
		}

		/// <summary> The DeclaredValueAmount property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."DeclaredValueAmount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DeclaredValueAmount
		{
			get { return (Nullable<System.Double>)GetValue((int)WorldShipProcessedFieldIndex.DeclaredValueAmount, false); }
			set	{ SetValue((int)WorldShipProcessedFieldIndex.DeclaredValueAmount, value); }
		}

		/// <summary> The DeclaredValueOption property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."DeclaredValueOption"<br/>
		/// Table field type characteristics (type, precision, scale, length): NChar, 0, 0, 2<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String DeclaredValueOption
		{
			get { return (System.String)GetValue((int)WorldShipProcessedFieldIndex.DeclaredValueOption, true); }
			set	{ SetValue((int)WorldShipProcessedFieldIndex.DeclaredValueOption, value); }
		}

		/// <summary> The WorldShipShipmentID property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."WorldShipShipmentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String WorldShipShipmentID
		{
			get { return (System.String)GetValue((int)WorldShipProcessedFieldIndex.WorldShipShipmentID, true); }
			set	{ SetValue((int)WorldShipProcessedFieldIndex.WorldShipShipmentID, value); }
		}

		/// <summary> The VoidIndicator property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."VoidIndicator"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String VoidIndicator
		{
			get { return (System.String)GetValue((int)WorldShipProcessedFieldIndex.VoidIndicator, true); }
			set	{ SetValue((int)WorldShipProcessedFieldIndex.VoidIndicator, value); }
		}

		/// <summary> The NumberOfPackages property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."NumberOfPackages"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String NumberOfPackages
		{
			get { return (System.String)GetValue((int)WorldShipProcessedFieldIndex.NumberOfPackages, true); }
			set	{ SetValue((int)WorldShipProcessedFieldIndex.NumberOfPackages, value); }
		}

		/// <summary> The LeadTrackingNumber property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."LeadTrackingNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String LeadTrackingNumber
		{
			get { return (System.String)GetValue((int)WorldShipProcessedFieldIndex.LeadTrackingNumber, true); }
			set	{ SetValue((int)WorldShipProcessedFieldIndex.LeadTrackingNumber, value); }
		}

		/// <summary> The ShipmentIdCalculated property of the Entity WorldShipProcessed<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipProcessed"."ShipmentIdCalculated"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> ShipmentIdCalculated
		{
			get { return (Nullable<System.Int64>)GetValue((int)WorldShipProcessedFieldIndex.ShipmentIdCalculated, false); }

		}

		/// <summary> Gets / sets related entity of type 'WorldShipShipmentEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual WorldShipShipmentEntity WorldShipShipment
		{
			get	{ return _worldShipShipment; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncWorldShipShipment(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "WorldShipProcessed", "WorldShipShipment", _worldShipShipment, true); 
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
			get { return (int)ShipWorks.Data.Model.EntityType.WorldShipProcessedEntity; }
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
