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
	/// <summary>Entity class which represents the entity 'WorldShipPackage'.<br/><br/></summary>
	[Serializable]
	public partial class WorldShipPackageEntity : CommonEntityBase
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
		static WorldShipPackageEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public WorldShipPackageEntity():base("WorldShipPackageEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public WorldShipPackageEntity(IEntityFields2 fields):base("WorldShipPackageEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this WorldShipPackageEntity</param>
		public WorldShipPackageEntity(IValidator validator):base("WorldShipPackageEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="upsPackageID">PK value for WorldShipPackage which data should be fetched into this WorldShipPackage object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public WorldShipPackageEntity(System.Int64 upsPackageID):base("WorldShipPackageEntity")
		{
			InitClassEmpty(null, null);
			this.UpsPackageID = upsPackageID;
		}

		/// <summary> CTor</summary>
		/// <param name="upsPackageID">PK value for WorldShipPackage which data should be fetched into this WorldShipPackage object</param>
		/// <param name="validator">The custom validator object for this WorldShipPackageEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public WorldShipPackageEntity(System.Int64 upsPackageID, IValidator validator):base("WorldShipPackageEntity")
		{
			InitClassEmpty(validator, null);
			this.UpsPackageID = upsPackageID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected WorldShipPackageEntity(SerializationInfo info, StreamingContext context) : base(info, context)
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
			switch((WorldShipPackageFieldIndex)fieldIndex)
			{
				case WorldShipPackageFieldIndex.ShipmentID:
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
					toReturn.Add(Relations.WorldShipShipmentEntityUsingShipmentID);
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
			return new WorldShipPackageRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'WorldShipShipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoWorldShipShipment()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(WorldShipShipmentFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(WorldShipPackageEntityFactory));
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
			_fieldsCustomProperties.Add("UpsPackageID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipmentID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PackageType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Weight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ReferenceNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ReferenceNumber2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CodOption", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CodAmount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CodCashOnly", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DeliveryConfirmation", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DeliveryConfirmationSignature", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DeliveryConfirmationAdult", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Length", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Width", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Height", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DeclaredValueAmount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DeclaredValueOption", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CN22GoodsType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CN22Description", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PostalSubClass", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("MIDeliveryConfirmation", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("QvnOption", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("QvnFrom", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("QvnSubjectLine", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("QvnMemo", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn1ShipNotify", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn1ContactName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn1Email", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn2ShipNotify", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn2ContactName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn2Email", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn3ShipNotify", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn3ContactName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn3Email", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipperRelease", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AdditionalHandlingEnabled", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("VerbalConfirmationOption", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("VerbalConfirmationContactName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("VerbalConfirmationTelephone", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DryIceRegulationSet", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DryIceWeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DryIceMedicalPurpose", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DryIceOption", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DryIceWeightUnitOfMeasure", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _worldShipShipment</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncWorldShipShipment(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _worldShipShipment, new PropertyChangedEventHandler( OnWorldShipShipmentPropertyChanged ), "WorldShipShipment", ShipWorks.Data.Model.RelationClasses.StaticWorldShipPackageRelations.WorldShipShipmentEntityUsingShipmentIDStatic, true, signalRelatedEntity, "Packages", resetFKFields, new int[] { (int)WorldShipPackageFieldIndex.ShipmentID } );
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
				this.PerformSetupSyncRelatedEntity( _worldShipShipment, new PropertyChangedEventHandler( OnWorldShipShipmentPropertyChanged ), "WorldShipShipment", ShipWorks.Data.Model.RelationClasses.StaticWorldShipPackageRelations.WorldShipShipmentEntityUsingShipmentIDStatic, true, new string[] {  } );
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
		/// <param name="validator">The validator object for this WorldShipPackageEntity</param>
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
		public  static WorldShipPackageRelations Relations
		{
			get	{ return new WorldShipPackageRelations(); }
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
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(WorldShipShipmentEntityFactory))),	(IEntityRelation)GetRelationsForField("WorldShipShipment")[0], (int)ShipWorks.Data.Model.EntityType.WorldShipPackageEntity, (int)ShipWorks.Data.Model.EntityType.WorldShipShipmentEntity, 0, null, null, null, null, "WorldShipShipment", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The UpsPackageID property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."UpsPackageID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
		public virtual System.Int64 UpsPackageID
		{
			get { return (System.Int64)GetValue((int)WorldShipPackageFieldIndex.UpsPackageID, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.UpsPackageID, value); }
		}

		/// <summary> The ShipmentID property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."ShipmentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 ShipmentID
		{
			get { return (System.Int64)GetValue((int)WorldShipPackageFieldIndex.ShipmentID, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.ShipmentID, value); }
		}

		/// <summary> The PackageType property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."PackageType"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String PackageType
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.PackageType, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.PackageType, value); }
		}

		/// <summary> The Weight property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."Weight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double Weight
		{
			get { return (System.Double)GetValue((int)WorldShipPackageFieldIndex.Weight, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.Weight, value); }
		}

		/// <summary> The ReferenceNumber property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."ReferenceNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ReferenceNumber
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.ReferenceNumber, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.ReferenceNumber, value); }
		}

		/// <summary> The ReferenceNumber2 property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."ReferenceNumber2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ReferenceNumber2
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.ReferenceNumber2, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.ReferenceNumber2, value); }
		}

		/// <summary> The CodOption property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."CodOption"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodOption
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.CodOption, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.CodOption, value); }
		}

		/// <summary> The CodAmount property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."CodAmount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal CodAmount
		{
			get { return (System.Decimal)GetValue((int)WorldShipPackageFieldIndex.CodAmount, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.CodAmount, value); }
		}

		/// <summary> The CodCashOnly property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."CodCashOnly"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodCashOnly
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.CodCashOnly, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.CodCashOnly, value); }
		}

		/// <summary> The DeliveryConfirmation property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."DeliveryConfirmation"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String DeliveryConfirmation
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.DeliveryConfirmation, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.DeliveryConfirmation, value); }
		}

		/// <summary> The DeliveryConfirmationSignature property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."DeliveryConfirmationSignature"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String DeliveryConfirmationSignature
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.DeliveryConfirmationSignature, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.DeliveryConfirmationSignature, value); }
		}

		/// <summary> The DeliveryConfirmationAdult property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."DeliveryConfirmationAdult"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String DeliveryConfirmationAdult
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.DeliveryConfirmationAdult, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.DeliveryConfirmationAdult, value); }
		}

		/// <summary> The Length property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."Length"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 Length
		{
			get { return (System.Int32)GetValue((int)WorldShipPackageFieldIndex.Length, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.Length, value); }
		}

		/// <summary> The Width property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."Width"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 Width
		{
			get { return (System.Int32)GetValue((int)WorldShipPackageFieldIndex.Width, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.Width, value); }
		}

		/// <summary> The Height property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."Height"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 Height
		{
			get { return (System.Int32)GetValue((int)WorldShipPackageFieldIndex.Height, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.Height, value); }
		}

		/// <summary> The DeclaredValueAmount property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."DeclaredValueAmount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DeclaredValueAmount
		{
			get { return (Nullable<System.Double>)GetValue((int)WorldShipPackageFieldIndex.DeclaredValueAmount, false); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.DeclaredValueAmount, value); }
		}

		/// <summary> The DeclaredValueOption property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."DeclaredValueOption"<br/>
		/// Table field type characteristics (type, precision, scale, length): NChar, 0, 0, 2<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String DeclaredValueOption
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.DeclaredValueOption, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.DeclaredValueOption, value); }
		}

		/// <summary> The CN22GoodsType property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."CN22GoodsType"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String CN22GoodsType
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.CN22GoodsType, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.CN22GoodsType, value); }
		}

		/// <summary> The CN22Description property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."CN22Description"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String CN22Description
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.CN22Description, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.CN22Description, value); }
		}

		/// <summary> The PostalSubClass property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."PostalSubClass"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String PostalSubClass
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.PostalSubClass, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.PostalSubClass, value); }
		}

		/// <summary> The MIDeliveryConfirmation property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."MIDeliveryConfirmation"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String MIDeliveryConfirmation
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.MIDeliveryConfirmation, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.MIDeliveryConfirmation, value); }
		}

		/// <summary> The QvnOption property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."QvnOption"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String QvnOption
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.QvnOption, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.QvnOption, value); }
		}

		/// <summary> The QvnFrom property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."QvnFrom"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String QvnFrom
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.QvnFrom, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.QvnFrom, value); }
		}

		/// <summary> The QvnSubjectLine property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."QvnSubjectLine"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 18<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String QvnSubjectLine
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.QvnSubjectLine, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.QvnSubjectLine, value); }
		}

		/// <summary> The QvnMemo property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."QvnMemo"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String QvnMemo
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.QvnMemo, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.QvnMemo, value); }
		}

		/// <summary> The Qvn1ShipNotify property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."Qvn1ShipNotify"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Qvn1ShipNotify
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.Qvn1ShipNotify, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.Qvn1ShipNotify, value); }
		}

		/// <summary> The Qvn1ContactName property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."Qvn1ContactName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Qvn1ContactName
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.Qvn1ContactName, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.Qvn1ContactName, value); }
		}

		/// <summary> The Qvn1Email property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."Qvn1Email"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Qvn1Email
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.Qvn1Email, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.Qvn1Email, value); }
		}

		/// <summary> The Qvn2ShipNotify property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."Qvn2ShipNotify"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Qvn2ShipNotify
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.Qvn2ShipNotify, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.Qvn2ShipNotify, value); }
		}

		/// <summary> The Qvn2ContactName property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."Qvn2ContactName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Qvn2ContactName
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.Qvn2ContactName, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.Qvn2ContactName, value); }
		}

		/// <summary> The Qvn2Email property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."Qvn2Email"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Qvn2Email
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.Qvn2Email, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.Qvn2Email, value); }
		}

		/// <summary> The Qvn3ShipNotify property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."Qvn3ShipNotify"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Qvn3ShipNotify
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.Qvn3ShipNotify, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.Qvn3ShipNotify, value); }
		}

		/// <summary> The Qvn3ContactName property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."Qvn3ContactName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Qvn3ContactName
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.Qvn3ContactName, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.Qvn3ContactName, value); }
		}

		/// <summary> The Qvn3Email property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."Qvn3Email"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Qvn3Email
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.Qvn3Email, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.Qvn3Email, value); }
		}

		/// <summary> The ShipperRelease property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."ShipperRelease"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String ShipperRelease
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.ShipperRelease, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.ShipperRelease, value); }
		}

		/// <summary> The AdditionalHandlingEnabled property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."AdditionalHandlingEnabled"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String AdditionalHandlingEnabled
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.AdditionalHandlingEnabled, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.AdditionalHandlingEnabled, value); }
		}

		/// <summary> The VerbalConfirmationOption property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."VerbalConfirmationOption"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String VerbalConfirmationOption
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.VerbalConfirmationOption, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.VerbalConfirmationOption, value); }
		}

		/// <summary> The VerbalConfirmationContactName property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."VerbalConfirmationContactName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String VerbalConfirmationContactName
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.VerbalConfirmationContactName, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.VerbalConfirmationContactName, value); }
		}

		/// <summary> The VerbalConfirmationTelephone property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."VerbalConfirmationTelephone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 15<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String VerbalConfirmationTelephone
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.VerbalConfirmationTelephone, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.VerbalConfirmationTelephone, value); }
		}

		/// <summary> The DryIceRegulationSet property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."DryIceRegulationSet"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 5<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String DryIceRegulationSet
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.DryIceRegulationSet, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.DryIceRegulationSet, value); }
		}

		/// <summary> The DryIceWeight property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."DryIceWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DryIceWeight
		{
			get { return (Nullable<System.Double>)GetValue((int)WorldShipPackageFieldIndex.DryIceWeight, false); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.DryIceWeight, value); }
		}

		/// <summary> The DryIceMedicalPurpose property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."DryIceMedicalPurpose"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String DryIceMedicalPurpose
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.DryIceMedicalPurpose, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.DryIceMedicalPurpose, value); }
		}

		/// <summary> The DryIceOption property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."DryIceOption"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String DryIceOption
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.DryIceOption, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.DryIceOption, value); }
		}

		/// <summary> The DryIceWeightUnitOfMeasure property of the Entity WorldShipPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipPackage"."DryIceWeightUnitOfMeasure"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String DryIceWeightUnitOfMeasure
		{
			get { return (System.String)GetValue((int)WorldShipPackageFieldIndex.DryIceWeightUnitOfMeasure, true); }
			set	{ SetValue((int)WorldShipPackageFieldIndex.DryIceWeightUnitOfMeasure, value); }
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
					SetSingleRelatedEntityNavigator(value, "Packages", "WorldShipShipment", _worldShipShipment, true); 
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
			get { return (int)ShipWorks.Data.Model.EntityType.WorldShipPackageEntity; }
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
