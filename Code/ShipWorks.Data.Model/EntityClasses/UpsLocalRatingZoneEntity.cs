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
	
	/// <summary>Entity class which represents the entity 'UpsLocalRatingZone'.<br/><br/></summary>
	[Serializable]
	public partial class UpsLocalRatingZoneEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END
			
	{
		#region Class Member Declarations
		private UpsLocalRatingZoneFileEntity _upsLocalRatingZoneFile;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name UpsLocalRatingZoneFile</summary>
			public static readonly string UpsLocalRatingZoneFile = "UpsLocalRatingZoneFile";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static UpsLocalRatingZoneEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public UpsLocalRatingZoneEntity():base("UpsLocalRatingZoneEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public UpsLocalRatingZoneEntity(IEntityFields2 fields):base("UpsLocalRatingZoneEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this UpsLocalRatingZoneEntity</param>
		public UpsLocalRatingZoneEntity(IValidator validator):base("UpsLocalRatingZoneEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="zoneID">PK value for UpsLocalRatingZone which data should be fetched into this UpsLocalRatingZone object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public UpsLocalRatingZoneEntity(System.Int64 zoneID):base("UpsLocalRatingZoneEntity")
		{
			InitClassEmpty(null, null);
			this.ZoneID = zoneID;
		}

		/// <summary> CTor</summary>
		/// <param name="zoneID">PK value for UpsLocalRatingZone which data should be fetched into this UpsLocalRatingZone object</param>
		/// <param name="validator">The custom validator object for this UpsLocalRatingZoneEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public UpsLocalRatingZoneEntity(System.Int64 zoneID, IValidator validator):base("UpsLocalRatingZoneEntity")
		{
			InitClassEmpty(validator, null);
			this.ZoneID = zoneID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected UpsLocalRatingZoneEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_upsLocalRatingZoneFile = (UpsLocalRatingZoneFileEntity)info.GetValue("_upsLocalRatingZoneFile", typeof(UpsLocalRatingZoneFileEntity));
				if(_upsLocalRatingZoneFile!=null)
				{
					_upsLocalRatingZoneFile.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((UpsLocalRatingZoneFieldIndex)fieldIndex)
			{
				case UpsLocalRatingZoneFieldIndex.ZoneFileID:
					DesetupSyncUpsLocalRatingZoneFile(true, false);
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
				case "UpsLocalRatingZoneFile":
					this.UpsLocalRatingZoneFile = (UpsLocalRatingZoneFileEntity)entity;
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
				case "UpsLocalRatingZoneFile":
					toReturn.Add(Relations.UpsLocalRatingZoneFileEntityUsingZoneFileID);
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
				case "UpsLocalRatingZoneFile":
					SetupSyncUpsLocalRatingZoneFile(relatedEntity);
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
				case "UpsLocalRatingZoneFile":
					DesetupSyncUpsLocalRatingZoneFile(false, true);
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
			if(_upsLocalRatingZoneFile!=null)
			{
				toReturn.Add(_upsLocalRatingZoneFile);
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
				info.AddValue("_upsLocalRatingZoneFile", (!this.MarkedForDeletion?_upsLocalRatingZoneFile:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new UpsLocalRatingZoneRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'UpsLocalRatingZoneFile' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUpsLocalRatingZoneFile()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UpsLocalRatingZoneFileFields.ZoneFileID, null, ComparisonOperator.Equal, this.ZoneFileID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(UpsLocalRatingZoneEntityFactory));
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
			toReturn.Add("UpsLocalRatingZoneFile", _upsLocalRatingZoneFile);
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
			_fieldsCustomProperties.Add("ZoneID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ZoneFileID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginZipFloor", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginZipCeiling", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DestinationZipFloor", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DestinationZipCeiling", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Service", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Zone", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _upsLocalRatingZoneFile</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncUpsLocalRatingZoneFile(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _upsLocalRatingZoneFile, new PropertyChangedEventHandler( OnUpsLocalRatingZoneFilePropertyChanged ), "UpsLocalRatingZoneFile", ShipWorks.Data.Model.RelationClasses.StaticUpsLocalRatingZoneRelations.UpsLocalRatingZoneFileEntityUsingZoneFileIDStatic, true, signalRelatedEntity, "UpsLocalRatingZone", resetFKFields, new int[] { (int)UpsLocalRatingZoneFieldIndex.ZoneFileID } );
			_upsLocalRatingZoneFile = null;
		}

		/// <summary> setups the sync logic for member _upsLocalRatingZoneFile</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncUpsLocalRatingZoneFile(IEntityCore relatedEntity)
		{
			if(_upsLocalRatingZoneFile!=relatedEntity)
			{
				DesetupSyncUpsLocalRatingZoneFile(true, true);
				_upsLocalRatingZoneFile = (UpsLocalRatingZoneFileEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _upsLocalRatingZoneFile, new PropertyChangedEventHandler( OnUpsLocalRatingZoneFilePropertyChanged ), "UpsLocalRatingZoneFile", ShipWorks.Data.Model.RelationClasses.StaticUpsLocalRatingZoneRelations.UpsLocalRatingZoneFileEntityUsingZoneFileIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnUpsLocalRatingZoneFilePropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this UpsLocalRatingZoneEntity</param>
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
		public  static UpsLocalRatingZoneRelations Relations
		{
			get	{ return new UpsLocalRatingZoneRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UpsLocalRatingZoneFile' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUpsLocalRatingZoneFile
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(UpsLocalRatingZoneFileEntityFactory))),	(IEntityRelation)GetRelationsForField("UpsLocalRatingZoneFile")[0], (int)ShipWorks.Data.Model.EntityType.UpsLocalRatingZoneEntity, (int)ShipWorks.Data.Model.EntityType.UpsLocalRatingZoneFileEntity, 0, null, null, null, null, "UpsLocalRatingZoneFile", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The ZoneID property of the Entity UpsLocalRatingZone<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsLocalRatingZone"."ZoneID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 ZoneID
		{
			get { return (System.Int64)GetValue((int)UpsLocalRatingZoneFieldIndex.ZoneID, true); }
			set	{ SetValue((int)UpsLocalRatingZoneFieldIndex.ZoneID, value); }
		}

		/// <summary> The ZoneFileID property of the Entity UpsLocalRatingZone<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsLocalRatingZone"."ZoneFileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 ZoneFileID
		{
			get { return (System.Int64)GetValue((int)UpsLocalRatingZoneFieldIndex.ZoneFileID, true); }
			set	{ SetValue((int)UpsLocalRatingZoneFieldIndex.ZoneFileID, value); }
		}

		/// <summary> The OriginZipFloor property of the Entity UpsLocalRatingZone<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsLocalRatingZone"."OriginZipFloor"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 5<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginZipFloor
		{
			get { return (System.String)GetValue((int)UpsLocalRatingZoneFieldIndex.OriginZipFloor, true); }
			set	{ SetValue((int)UpsLocalRatingZoneFieldIndex.OriginZipFloor, value); }
		}

		/// <summary> The OriginZipCeiling property of the Entity UpsLocalRatingZone<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsLocalRatingZone"."OriginZipCeiling"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 5<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OriginZipCeiling
		{
			get { return (System.String)GetValue((int)UpsLocalRatingZoneFieldIndex.OriginZipCeiling, true); }
			set	{ SetValue((int)UpsLocalRatingZoneFieldIndex.OriginZipCeiling, value); }
		}

		/// <summary> The DestinationZipFloor property of the Entity UpsLocalRatingZone<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsLocalRatingZone"."DestinationZipFloor"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 5<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String DestinationZipFloor
		{
			get { return (System.String)GetValue((int)UpsLocalRatingZoneFieldIndex.DestinationZipFloor, true); }
			set	{ SetValue((int)UpsLocalRatingZoneFieldIndex.DestinationZipFloor, value); }
		}

		/// <summary> The DestinationZipCeiling property of the Entity UpsLocalRatingZone<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsLocalRatingZone"."DestinationZipCeiling"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 5<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String DestinationZipCeiling
		{
			get { return (System.String)GetValue((int)UpsLocalRatingZoneFieldIndex.DestinationZipCeiling, true); }
			set	{ SetValue((int)UpsLocalRatingZoneFieldIndex.DestinationZipCeiling, value); }
		}

		/// <summary> The Service property of the Entity UpsLocalRatingZone<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsLocalRatingZone"."Service"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 Service
		{
			get { return (System.Int32)GetValue((int)UpsLocalRatingZoneFieldIndex.Service, true); }
			set	{ SetValue((int)UpsLocalRatingZoneFieldIndex.Service, value); }
		}

		/// <summary> The Zone property of the Entity UpsLocalRatingZone<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsLocalRatingZone"."Zone"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 Zone
		{
			get { return (System.Int32)GetValue((int)UpsLocalRatingZoneFieldIndex.Zone, true); }
			set	{ SetValue((int)UpsLocalRatingZoneFieldIndex.Zone, value); }
		}

		/// <summary> Gets / sets related entity of type 'UpsLocalRatingZoneFileEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual UpsLocalRatingZoneFileEntity UpsLocalRatingZoneFile
		{
			get	{ return _upsLocalRatingZoneFile; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncUpsLocalRatingZoneFile(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "UpsLocalRatingZone", "UpsLocalRatingZoneFile", _upsLocalRatingZoneFile, true); 
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
			get { return (int)ShipWorks.Data.Model.EntityType.UpsLocalRatingZoneEntity; }
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
