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
	/// <summary>Entity class which represents the entity 'FedExPackage'.<br/><br/></summary>
	[Serializable]
	public partial class FedExPackageEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private FedExShipmentEntity _fedExShipment;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name FedExShipment</summary>
			public static readonly string FedExShipment = "FedExShipment";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static FedExPackageEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public FedExPackageEntity():base("FedExPackageEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public FedExPackageEntity(IEntityFields2 fields):base("FedExPackageEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this FedExPackageEntity</param>
		public FedExPackageEntity(IValidator validator):base("FedExPackageEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="fedExPackageID">PK value for FedExPackage which data should be fetched into this FedExPackage object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public FedExPackageEntity(System.Int64 fedExPackageID):base("FedExPackageEntity")
		{
			InitClassEmpty(null, null);
			this.FedExPackageID = fedExPackageID;
		}

		/// <summary> CTor</summary>
		/// <param name="fedExPackageID">PK value for FedExPackage which data should be fetched into this FedExPackage object</param>
		/// <param name="validator">The custom validator object for this FedExPackageEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public FedExPackageEntity(System.Int64 fedExPackageID, IValidator validator):base("FedExPackageEntity")
		{
			InitClassEmpty(validator, null);
			this.FedExPackageID = fedExPackageID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected FedExPackageEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_fedExShipment = (FedExShipmentEntity)info.GetValue("_fedExShipment", typeof(FedExShipmentEntity));
				if(_fedExShipment!=null)
				{
					_fedExShipment.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((FedExPackageFieldIndex)fieldIndex)
			{
				case FedExPackageFieldIndex.ShipmentID:
					DesetupSyncFedExShipment(true, false);
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
				case "FedExShipment":
					this.FedExShipment = (FedExShipmentEntity)entity;
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
				case "FedExShipment":
					toReturn.Add(Relations.FedExShipmentEntityUsingShipmentID);
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
				case "FedExShipment":
					SetupSyncFedExShipment(relatedEntity);
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
				case "FedExShipment":
					DesetupSyncFedExShipment(false, true);
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
			if(_fedExShipment!=null)
			{
				toReturn.Add(_fedExShipment);
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
				info.AddValue("_fedExShipment", (!this.MarkedForDeletion?_fedExShipment:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new FedExPackageRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'FedExShipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoFedExShipment()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(FedExShipmentFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(FedExPackageEntityFactory));
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
			toReturn.Add("FedExShipment", _fedExShipment);
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
			_fieldsCustomProperties.Add("FedExPackageID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipmentID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Weight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DimsProfileID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DimsLength", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DimsWidth", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DimsHeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DimsWeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DimsAddWeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SkidPieces", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Insurance", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InsuranceValue", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InsurancePennyOne", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DeclaredValue", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("TrackingNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PriorityAlert", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PriorityAlertEnhancementType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PriorityAlertDetailContent", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DryIceWeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ContainsAlcohol", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DangerousGoodsEnabled", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DangerousGoodsType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DangerousGoodsAccessibilityType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DangerousGoodsCargoAircraftOnly", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DangerousGoodsEmergencyContactPhone", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DangerousGoodsOfferor", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DangerousGoodsPackagingCount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("HazardousMaterialNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("HazardousMaterialClass", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("HazardousMaterialProperName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("HazardousMaterialPackingGroup", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("HazardousMaterialQuantityValue", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("HazardousMaterialQuanityUnits", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("HazardousMaterialTechnicalName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SignatoryContactName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SignatoryTitle", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SignatoryPlace", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AlcoholRecipientType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ContainerType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("NumberOfContainers", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PackingDetailsCargoAircraftOnly", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PackingDetailsPackingInstructions", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BatteryMaterial", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BatteryPacking", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BatteryRegulatorySubtype", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FreightPackaging", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FreightPieces", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _fedExShipment</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncFedExShipment(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _fedExShipment, new PropertyChangedEventHandler( OnFedExShipmentPropertyChanged ), "FedExShipment", ShipWorks.Data.Model.RelationClasses.StaticFedExPackageRelations.FedExShipmentEntityUsingShipmentIDStatic, true, signalRelatedEntity, "Packages", resetFKFields, new int[] { (int)FedExPackageFieldIndex.ShipmentID } );
			_fedExShipment = null;
		}

		/// <summary> setups the sync logic for member _fedExShipment</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncFedExShipment(IEntityCore relatedEntity)
		{
			if(_fedExShipment!=relatedEntity)
			{
				DesetupSyncFedExShipment(true, true);
				_fedExShipment = (FedExShipmentEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _fedExShipment, new PropertyChangedEventHandler( OnFedExShipmentPropertyChanged ), "FedExShipment", ShipWorks.Data.Model.RelationClasses.StaticFedExPackageRelations.FedExShipmentEntityUsingShipmentIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnFedExShipmentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this FedExPackageEntity</param>
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
		public  static FedExPackageRelations Relations
		{
			get	{ return new FedExPackageRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'FedExShipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathFedExShipment
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(FedExShipmentEntityFactory))),	(IEntityRelation)GetRelationsForField("FedExShipment")[0], (int)ShipWorks.Data.Model.EntityType.FedExPackageEntity, (int)ShipWorks.Data.Model.EntityType.FedExShipmentEntity, 0, null, null, null, null, "FedExShipment", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The FedExPackageID property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."FedExPackageID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 FedExPackageID
		{
			get { return (System.Int64)GetValue((int)FedExPackageFieldIndex.FedExPackageID, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.FedExPackageID, value); }
		}

		/// <summary> The ShipmentID property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."ShipmentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 ShipmentID
		{
			get { return (System.Int64)GetValue((int)FedExPackageFieldIndex.ShipmentID, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.ShipmentID, value); }
		}

		/// <summary> The Weight property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."Weight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double Weight
		{
			get { return (System.Double)GetValue((int)FedExPackageFieldIndex.Weight, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.Weight, value); }
		}

		/// <summary> The DimsProfileID property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."DimsProfileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 DimsProfileID
		{
			get { return (System.Int64)GetValue((int)FedExPackageFieldIndex.DimsProfileID, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.DimsProfileID, value); }
		}

		/// <summary> The DimsLength property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."DimsLength"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DimsLength
		{
			get { return (System.Double)GetValue((int)FedExPackageFieldIndex.DimsLength, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.DimsLength, value); }
		}

		/// <summary> The DimsWidth property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."DimsWidth"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DimsWidth
		{
			get { return (System.Double)GetValue((int)FedExPackageFieldIndex.DimsWidth, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.DimsWidth, value); }
		}

		/// <summary> The DimsHeight property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."DimsHeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DimsHeight
		{
			get { return (System.Double)GetValue((int)FedExPackageFieldIndex.DimsHeight, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.DimsHeight, value); }
		}

		/// <summary> The DimsWeight property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."DimsWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DimsWeight
		{
			get { return (System.Double)GetValue((int)FedExPackageFieldIndex.DimsWeight, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.DimsWeight, value); }
		}

		/// <summary> The DimsAddWeight property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."DimsAddWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean DimsAddWeight
		{
			get { return (System.Boolean)GetValue((int)FedExPackageFieldIndex.DimsAddWeight, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.DimsAddWeight, value); }
		}

		/// <summary> The SkidPieces property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."SkidPieces"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 SkidPieces
		{
			get { return (System.Int32)GetValue((int)FedExPackageFieldIndex.SkidPieces, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.SkidPieces, value); }
		}

		/// <summary> The Insurance property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."Insurance"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Insurance
		{
			get { return (System.Boolean)GetValue((int)FedExPackageFieldIndex.Insurance, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.Insurance, value); }
		}

		/// <summary> The InsuranceValue property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."InsuranceValue"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal InsuranceValue
		{
			get { return (System.Decimal)GetValue((int)FedExPackageFieldIndex.InsuranceValue, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.InsuranceValue, value); }
		}

		/// <summary> The InsurancePennyOne property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."InsurancePennyOne"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean InsurancePennyOne
		{
			get { return (System.Boolean)GetValue((int)FedExPackageFieldIndex.InsurancePennyOne, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.InsurancePennyOne, value); }
		}

		/// <summary> The DeclaredValue property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."DeclaredValue"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal DeclaredValue
		{
			get { return (System.Decimal)GetValue((int)FedExPackageFieldIndex.DeclaredValue, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.DeclaredValue, value); }
		}

		/// <summary> The TrackingNumber property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."TrackingNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String TrackingNumber
		{
			get { return (System.String)GetValue((int)FedExPackageFieldIndex.TrackingNumber, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.TrackingNumber, value); }
		}

		/// <summary> The PriorityAlert property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."PriorityAlert"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean PriorityAlert
		{
			get { return (System.Boolean)GetValue((int)FedExPackageFieldIndex.PriorityAlert, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.PriorityAlert, value); }
		}

		/// <summary> The PriorityAlertEnhancementType property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."PriorityAlertEnhancementType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 PriorityAlertEnhancementType
		{
			get { return (System.Int32)GetValue((int)FedExPackageFieldIndex.PriorityAlertEnhancementType, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.PriorityAlertEnhancementType, value); }
		}

		/// <summary> The PriorityAlertDetailContent property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."PriorityAlertDetailContent"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 1024<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String PriorityAlertDetailContent
		{
			get { return (System.String)GetValue((int)FedExPackageFieldIndex.PriorityAlertDetailContent, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.PriorityAlertDetailContent, value); }
		}

		/// <summary> The DryIceWeight property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."DryIceWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DryIceWeight
		{
			get { return (System.Double)GetValue((int)FedExPackageFieldIndex.DryIceWeight, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.DryIceWeight, value); }
		}

		/// <summary> The ContainsAlcohol property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."ContainsAlcohol"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean ContainsAlcohol
		{
			get { return (System.Boolean)GetValue((int)FedExPackageFieldIndex.ContainsAlcohol, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.ContainsAlcohol, value); }
		}

		/// <summary> The DangerousGoodsEnabled property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."DangerousGoodsEnabled"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean DangerousGoodsEnabled
		{
			get { return (System.Boolean)GetValue((int)FedExPackageFieldIndex.DangerousGoodsEnabled, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.DangerousGoodsEnabled, value); }
		}

		/// <summary> The DangerousGoodsType property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."DangerousGoodsType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 DangerousGoodsType
		{
			get { return (System.Int32)GetValue((int)FedExPackageFieldIndex.DangerousGoodsType, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.DangerousGoodsType, value); }
		}

		/// <summary> The DangerousGoodsAccessibilityType property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."DangerousGoodsAccessibilityType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 DangerousGoodsAccessibilityType
		{
			get { return (System.Int32)GetValue((int)FedExPackageFieldIndex.DangerousGoodsAccessibilityType, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.DangerousGoodsAccessibilityType, value); }
		}

		/// <summary> The DangerousGoodsCargoAircraftOnly property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."DangerousGoodsCargoAircraftOnly"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean DangerousGoodsCargoAircraftOnly
		{
			get { return (System.Boolean)GetValue((int)FedExPackageFieldIndex.DangerousGoodsCargoAircraftOnly, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.DangerousGoodsCargoAircraftOnly, value); }
		}

		/// <summary> The DangerousGoodsEmergencyContactPhone property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."DangerousGoodsEmergencyContactPhone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 16<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String DangerousGoodsEmergencyContactPhone
		{
			get { return (System.String)GetValue((int)FedExPackageFieldIndex.DangerousGoodsEmergencyContactPhone, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.DangerousGoodsEmergencyContactPhone, value); }
		}

		/// <summary> The DangerousGoodsOfferor property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."DangerousGoodsOfferor"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 128<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String DangerousGoodsOfferor
		{
			get { return (System.String)GetValue((int)FedExPackageFieldIndex.DangerousGoodsOfferor, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.DangerousGoodsOfferor, value); }
		}

		/// <summary> The DangerousGoodsPackagingCount property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."DangerousGoodsPackagingCount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 DangerousGoodsPackagingCount
		{
			get { return (System.Int32)GetValue((int)FedExPackageFieldIndex.DangerousGoodsPackagingCount, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.DangerousGoodsPackagingCount, value); }
		}

		/// <summary> The HazardousMaterialNumber property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."HazardousMaterialNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 16<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String HazardousMaterialNumber
		{
			get { return (System.String)GetValue((int)FedExPackageFieldIndex.HazardousMaterialNumber, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.HazardousMaterialNumber, value); }
		}

		/// <summary> The HazardousMaterialClass property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."HazardousMaterialClass"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 8<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String HazardousMaterialClass
		{
			get { return (System.String)GetValue((int)FedExPackageFieldIndex.HazardousMaterialClass, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.HazardousMaterialClass, value); }
		}

		/// <summary> The HazardousMaterialProperName property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."HazardousMaterialProperName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String HazardousMaterialProperName
		{
			get { return (System.String)GetValue((int)FedExPackageFieldIndex.HazardousMaterialProperName, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.HazardousMaterialProperName, value); }
		}

		/// <summary> The HazardousMaterialPackingGroup property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."HazardousMaterialPackingGroup"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 HazardousMaterialPackingGroup
		{
			get { return (System.Int32)GetValue((int)FedExPackageFieldIndex.HazardousMaterialPackingGroup, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.HazardousMaterialPackingGroup, value); }
		}

		/// <summary> The HazardousMaterialQuantityValue property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."HazardousMaterialQuantityValue"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double HazardousMaterialQuantityValue
		{
			get { return (System.Double)GetValue((int)FedExPackageFieldIndex.HazardousMaterialQuantityValue, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.HazardousMaterialQuantityValue, value); }
		}

		/// <summary> The HazardousMaterialQuanityUnits property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."HazardousMaterialQuanityUnits"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 HazardousMaterialQuanityUnits
		{
			get { return (System.Int32)GetValue((int)FedExPackageFieldIndex.HazardousMaterialQuanityUnits, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.HazardousMaterialQuanityUnits, value); }
		}

		/// <summary> The HazardousMaterialTechnicalName property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."HazardousMaterialTechnicalName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String HazardousMaterialTechnicalName
		{
			get { return (System.String)GetValue((int)FedExPackageFieldIndex.HazardousMaterialTechnicalName, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.HazardousMaterialTechnicalName, value); }
		}

		/// <summary> The SignatoryContactName property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."SignatoryContactName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String SignatoryContactName
		{
			get { return (System.String)GetValue((int)FedExPackageFieldIndex.SignatoryContactName, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.SignatoryContactName, value); }
		}

		/// <summary> The SignatoryTitle property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."SignatoryTitle"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String SignatoryTitle
		{
			get { return (System.String)GetValue((int)FedExPackageFieldIndex.SignatoryTitle, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.SignatoryTitle, value); }
		}

		/// <summary> The SignatoryPlace property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."SignatoryPlace"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String SignatoryPlace
		{
			get { return (System.String)GetValue((int)FedExPackageFieldIndex.SignatoryPlace, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.SignatoryPlace, value); }
		}

		/// <summary> The AlcoholRecipientType property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."AlcoholRecipientType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 AlcoholRecipientType
		{
			get { return (System.Int32)GetValue((int)FedExPackageFieldIndex.AlcoholRecipientType, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.AlcoholRecipientType, value); }
		}

		/// <summary> The ContainerType property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."ContainerType"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ContainerType
		{
			get { return (System.String)GetValue((int)FedExPackageFieldIndex.ContainerType, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.ContainerType, value); }
		}

		/// <summary> The NumberOfContainers property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."NumberOfContainers"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 NumberOfContainers
		{
			get { return (System.Int32)GetValue((int)FedExPackageFieldIndex.NumberOfContainers, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.NumberOfContainers, value); }
		}

		/// <summary> The PackingDetailsCargoAircraftOnly property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."PackingDetailsCargoAircraftOnly"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean PackingDetailsCargoAircraftOnly
		{
			get { return (System.Boolean)GetValue((int)FedExPackageFieldIndex.PackingDetailsCargoAircraftOnly, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.PackingDetailsCargoAircraftOnly, value); }
		}

		/// <summary> The PackingDetailsPackingInstructions property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."PackingDetailsPackingInstructions"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String PackingDetailsPackingInstructions
		{
			get { return (System.String)GetValue((int)FedExPackageFieldIndex.PackingDetailsPackingInstructions, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.PackingDetailsPackingInstructions, value); }
		}

		/// <summary> The BatteryMaterial property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."BatteryMaterial"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual ShipWorks.Shipping.FedEx.FedExBatteryMaterialType BatteryMaterial
		{
			get { return (ShipWorks.Shipping.FedEx.FedExBatteryMaterialType)GetValue((int)FedExPackageFieldIndex.BatteryMaterial, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.BatteryMaterial, value); }
		}

		/// <summary> The BatteryPacking property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."BatteryPacking"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual ShipWorks.Shipping.FedEx.FedExBatteryPackingType BatteryPacking
		{
			get { return (ShipWorks.Shipping.FedEx.FedExBatteryPackingType)GetValue((int)FedExPackageFieldIndex.BatteryPacking, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.BatteryPacking, value); }
		}

		/// <summary> The BatteryRegulatorySubtype property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."BatteryRegulatorySubtype"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual ShipWorks.Shipping.FedEx.FedExBatteryRegulatorySubType BatteryRegulatorySubtype
		{
			get { return (ShipWorks.Shipping.FedEx.FedExBatteryRegulatorySubType)GetValue((int)FedExPackageFieldIndex.BatteryRegulatorySubtype, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.BatteryRegulatorySubtype, value); }
		}

		/// <summary> The FreightPackaging property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."FreightPackaging"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual ShipWorks.Shipping.FedEx.FedExFreightPhysicalPackagingType FreightPackaging
		{
			get { return (ShipWorks.Shipping.FedEx.FedExFreightPhysicalPackagingType)GetValue((int)FedExPackageFieldIndex.FreightPackaging, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.FreightPackaging, value); }
		}

		/// <summary> The FreightPieces property of the Entity FedExPackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExPackage"."FreightPieces"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 FreightPieces
		{
			get { return (System.Int32)GetValue((int)FedExPackageFieldIndex.FreightPieces, true); }
			set	{ SetValue((int)FedExPackageFieldIndex.FreightPieces, value); }
		}

		/// <summary> Gets / sets related entity of type 'FedExShipmentEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual FedExShipmentEntity FedExShipment
		{
			get	{ return _fedExShipment; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncFedExShipment(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "Packages", "FedExShipment", _fedExShipment, true); 
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
			get { return (int)ShipWorks.Data.Model.EntityType.FedExPackageEntity; }
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
