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
	/// <summary>Entity class which represents the entity 'DhlEcommerceShipment'.<br/><br/></summary>
	[Serializable]
	public partial class DhlEcommerceShipmentEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private ScanFormBatchEntity _scanFormBatch;
		private ShipmentEntity _shipment;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name ScanFormBatch</summary>
			public static readonly string ScanFormBatch = "ScanFormBatch";
			/// <summary>Member name Shipment</summary>
			public static readonly string Shipment = "Shipment";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static DhlEcommerceShipmentEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public DhlEcommerceShipmentEntity():base("DhlEcommerceShipmentEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public DhlEcommerceShipmentEntity(IEntityFields2 fields):base("DhlEcommerceShipmentEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this DhlEcommerceShipmentEntity</param>
		public DhlEcommerceShipmentEntity(IValidator validator):base("DhlEcommerceShipmentEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="shipmentID">PK value for DhlEcommerceShipment which data should be fetched into this DhlEcommerceShipment object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public DhlEcommerceShipmentEntity(System.Int64 shipmentID):base("DhlEcommerceShipmentEntity")
		{
			InitClassEmpty(null, null);
			this.ShipmentID = shipmentID;
		}

		/// <summary> CTor</summary>
		/// <param name="shipmentID">PK value for DhlEcommerceShipment which data should be fetched into this DhlEcommerceShipment object</param>
		/// <param name="validator">The custom validator object for this DhlEcommerceShipmentEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public DhlEcommerceShipmentEntity(System.Int64 shipmentID, IValidator validator):base("DhlEcommerceShipmentEntity")
		{
			InitClassEmpty(validator, null);
			this.ShipmentID = shipmentID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected DhlEcommerceShipmentEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_scanFormBatch = (ScanFormBatchEntity)info.GetValue("_scanFormBatch", typeof(ScanFormBatchEntity));
				if(_scanFormBatch!=null)
				{
					_scanFormBatch.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_shipment = (ShipmentEntity)info.GetValue("_shipment", typeof(ShipmentEntity));
				if(_shipment!=null)
				{
					_shipment.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((DhlEcommerceShipmentFieldIndex)fieldIndex)
			{
				case DhlEcommerceShipmentFieldIndex.ShipmentID:
					DesetupSyncShipment(true, false);
					break;
				case DhlEcommerceShipmentFieldIndex.ScanFormBatchID:
					DesetupSyncScanFormBatch(true, false);
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
				case "ScanFormBatch":
					this.ScanFormBatch = (ScanFormBatchEntity)entity;
					break;
				case "Shipment":
					this.Shipment = (ShipmentEntity)entity;
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
				case "ScanFormBatch":
					toReturn.Add(Relations.ScanFormBatchEntityUsingScanFormBatchID);
					break;
				case "Shipment":
					toReturn.Add(Relations.ShipmentEntityUsingShipmentID);
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
				case "ScanFormBatch":
					SetupSyncScanFormBatch(relatedEntity);
					break;
				case "Shipment":
					SetupSyncShipment(relatedEntity);
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
				case "ScanFormBatch":
					DesetupSyncScanFormBatch(false, true);
					break;
				case "Shipment":
					DesetupSyncShipment(false, true);
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
			if(_scanFormBatch!=null)
			{
				toReturn.Add(_scanFormBatch);
			}
			if(_shipment!=null)
			{
				toReturn.Add(_shipment);
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
				info.AddValue("_scanFormBatch", (!this.MarkedForDeletion?_scanFormBatch:null));
				info.AddValue("_shipment", (!this.MarkedForDeletion?_shipment:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new DhlEcommerceShipmentRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'ScanFormBatch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoScanFormBatch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ScanFormBatchFields.ScanFormBatchID, null, ComparisonOperator.Equal, this.ScanFormBatchID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'Shipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoShipment()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ShipmentFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(DhlEcommerceShipmentEntityFactory));
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
			toReturn.Add("ScanFormBatch", _scanFormBatch);
			toReturn.Add("Shipment", _shipment);
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
			_fieldsCustomProperties.Add("ShipmentID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DhlEcommerceAccountID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Service", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DeliveredDutyPaid", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("NonMachinable", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SaturdayDelivery", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RequestedLabelFormat", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Contents", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("NonDelivery", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipEngineLabelID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("IntegratorTransactionID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("StampsTransactionID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ResidentialDelivery", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CustomsRecipientTin", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CustomsTaxIdType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CustomsTinIssuingAuthority", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ScanFormBatchID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PackagingType", fieldHashtable);
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
			_fieldsCustomProperties.Add("Reference1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InsuranceValue", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InsurancePennyOne", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _scanFormBatch</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncScanFormBatch(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _scanFormBatch, new PropertyChangedEventHandler( OnScanFormBatchPropertyChanged ), "ScanFormBatch", ShipWorks.Data.Model.RelationClasses.StaticDhlEcommerceShipmentRelations.ScanFormBatchEntityUsingScanFormBatchIDStatic, true, signalRelatedEntity, "DhlEcommerceShipment", resetFKFields, new int[] { (int)DhlEcommerceShipmentFieldIndex.ScanFormBatchID } );
			_scanFormBatch = null;
		}

		/// <summary> setups the sync logic for member _scanFormBatch</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncScanFormBatch(IEntityCore relatedEntity)
		{
			if(_scanFormBatch!=relatedEntity)
			{
				DesetupSyncScanFormBatch(true, true);
				_scanFormBatch = (ScanFormBatchEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _scanFormBatch, new PropertyChangedEventHandler( OnScanFormBatchPropertyChanged ), "ScanFormBatch", ShipWorks.Data.Model.RelationClasses.StaticDhlEcommerceShipmentRelations.ScanFormBatchEntityUsingScanFormBatchIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnScanFormBatchPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _shipment</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncShipment(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _shipment, new PropertyChangedEventHandler( OnShipmentPropertyChanged ), "Shipment", ShipWorks.Data.Model.RelationClasses.StaticDhlEcommerceShipmentRelations.ShipmentEntityUsingShipmentIDStatic, true, signalRelatedEntity, "DhlEcommerce", false, new int[] { (int)DhlEcommerceShipmentFieldIndex.ShipmentID } );
			_shipment = null;
		}
		
		/// <summary> setups the sync logic for member _shipment</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncShipment(IEntityCore relatedEntity)
		{
			if(_shipment!=relatedEntity)
			{
				DesetupSyncShipment(true, true);
				_shipment = (ShipmentEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _shipment, new PropertyChangedEventHandler( OnShipmentPropertyChanged ), "Shipment", ShipWorks.Data.Model.RelationClasses.StaticDhlEcommerceShipmentRelations.ShipmentEntityUsingShipmentIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnShipmentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this DhlEcommerceShipmentEntity</param>
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
		public  static DhlEcommerceShipmentRelations Relations
		{
			get	{ return new DhlEcommerceShipmentRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ScanFormBatch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathScanFormBatch
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(ScanFormBatchEntityFactory))),	(IEntityRelation)GetRelationsForField("ScanFormBatch")[0], (int)ShipWorks.Data.Model.EntityType.DhlEcommerceShipmentEntity, (int)ShipWorks.Data.Model.EntityType.ScanFormBatchEntity, 0, null, null, null, null, "ScanFormBatch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Shipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathShipment
		{
			get { return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(ShipmentEntityFactory))), (IEntityRelation)GetRelationsForField("Shipment")[0], (int)ShipWorks.Data.Model.EntityType.DhlEcommerceShipmentEntity, (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, 0, null, null, null, null, "Shipment", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);	}
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

		/// <summary> The ShipmentID property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."ShipmentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
		[DataMember]
		public virtual System.Int64 ShipmentID
		{
			get { return (System.Int64)GetValue((int)DhlEcommerceShipmentFieldIndex.ShipmentID, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.ShipmentID, value); }
		}

		/// <summary> The DhlEcommerceAccountID property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."DhlEcommerceAccountID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Int64 DhlEcommerceAccountID
		{
			get { return (System.Int64)GetValue((int)DhlEcommerceShipmentFieldIndex.DhlEcommerceAccountID, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.DhlEcommerceAccountID, value); }
		}

		/// <summary> The Service property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."Service"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Int32 Service
		{
			get { return (System.Int32)GetValue((int)DhlEcommerceShipmentFieldIndex.Service, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.Service, value); }
		}

		/// <summary> The DeliveredDutyPaid property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."DeliveredDutyPaid"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Boolean DeliveredDutyPaid
		{
			get { return (System.Boolean)GetValue((int)DhlEcommerceShipmentFieldIndex.DeliveredDutyPaid, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.DeliveredDutyPaid, value); }
		}

		/// <summary> The NonMachinable property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."NonMachinable"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Boolean NonMachinable
		{
			get { return (System.Boolean)GetValue((int)DhlEcommerceShipmentFieldIndex.NonMachinable, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.NonMachinable, value); }
		}

		/// <summary> The SaturdayDelivery property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."SaturdayDelivery"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Boolean SaturdayDelivery
		{
			get { return (System.Boolean)GetValue((int)DhlEcommerceShipmentFieldIndex.SaturdayDelivery, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.SaturdayDelivery, value); }
		}

		/// <summary> The RequestedLabelFormat property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."RequestedLabelFormat"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Int32 RequestedLabelFormat
		{
			get { return (System.Int32)GetValue((int)DhlEcommerceShipmentFieldIndex.RequestedLabelFormat, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.RequestedLabelFormat, value); }
		}

		/// <summary> The Contents property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."Contents"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Int32 Contents
		{
			get { return (System.Int32)GetValue((int)DhlEcommerceShipmentFieldIndex.Contents, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.Contents, value); }
		}

		/// <summary> The NonDelivery property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."NonDelivery"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Int32 NonDelivery
		{
			get { return (System.Int32)GetValue((int)DhlEcommerceShipmentFieldIndex.NonDelivery, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.NonDelivery, value); }
		}

		/// <summary> The ShipEngineLabelID property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."ShipEngineLabelID"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String ShipEngineLabelID
		{
			get { return (System.String)GetValue((int)DhlEcommerceShipmentFieldIndex.ShipEngineLabelID, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.ShipEngineLabelID, value); }
		}

		/// <summary> The IntegratorTransactionID property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."IntegratorTransactionID"<br/>
		/// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Guid> IntegratorTransactionID
		{
			get { return (Nullable<System.Guid>)GetValue((int)DhlEcommerceShipmentFieldIndex.IntegratorTransactionID, false); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.IntegratorTransactionID, value); }
		}

		/// <summary> The StampsTransactionID property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."StampsTransactionID"<br/>
		/// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Guid> StampsTransactionID
		{
			get { return (Nullable<System.Guid>)GetValue((int)DhlEcommerceShipmentFieldIndex.StampsTransactionID, false); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.StampsTransactionID, value); }
		}

		/// <summary> The ResidentialDelivery property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."ResidentialDelivery"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Boolean ResidentialDelivery
		{
			get { return (System.Boolean)GetValue((int)DhlEcommerceShipmentFieldIndex.ResidentialDelivery, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.ResidentialDelivery, value); }
		}

		/// <summary> The CustomsRecipientTin property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."CustomsRecipientTin"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String CustomsRecipientTin
		{
			get { return (System.String)GetValue((int)DhlEcommerceShipmentFieldIndex.CustomsRecipientTin, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.CustomsRecipientTin, value); }
		}

		/// <summary> The CustomsTaxIdType property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."CustomsTaxIdType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> CustomsTaxIdType
		{
			get { return (Nullable<System.Int32>)GetValue((int)DhlEcommerceShipmentFieldIndex.CustomsTaxIdType, false); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.CustomsTaxIdType, value); }
		}

		/// <summary> The CustomsTinIssuingAuthority property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."CustomsTinIssuingAuthority"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String CustomsTinIssuingAuthority
		{
			get { return (System.String)GetValue((int)DhlEcommerceShipmentFieldIndex.CustomsTinIssuingAuthority, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.CustomsTinIssuingAuthority, value); }
		}

		/// <summary> The ScanFormBatchID property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."ScanFormBatchID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int64> ScanFormBatchID
		{
			get { return (Nullable<System.Int64>)GetValue((int)DhlEcommerceShipmentFieldIndex.ScanFormBatchID, false); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.ScanFormBatchID, value); }
		}

		/// <summary> The PackagingType property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."PackagingType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Int32 PackagingType
		{
			get { return (System.Int32)GetValue((int)DhlEcommerceShipmentFieldIndex.PackagingType, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.PackagingType, value); }
		}

		/// <summary> The DimsProfileID property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."DimsProfileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Int64 DimsProfileID
		{
			get { return (System.Int64)GetValue((int)DhlEcommerceShipmentFieldIndex.DimsProfileID, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.DimsProfileID, value); }
		}

		/// <summary> The DimsLength property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."DimsLength"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Double DimsLength
		{
			get { return (System.Double)GetValue((int)DhlEcommerceShipmentFieldIndex.DimsLength, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.DimsLength, value); }
		}

		/// <summary> The DimsWidth property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."DimsWidth"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Double DimsWidth
		{
			get { return (System.Double)GetValue((int)DhlEcommerceShipmentFieldIndex.DimsWidth, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.DimsWidth, value); }
		}

		/// <summary> The DimsHeight property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."DimsHeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Double DimsHeight
		{
			get { return (System.Double)GetValue((int)DhlEcommerceShipmentFieldIndex.DimsHeight, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.DimsHeight, value); }
		}

		/// <summary> The DimsWeight property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."DimsWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Double DimsWeight
		{
			get { return (System.Double)GetValue((int)DhlEcommerceShipmentFieldIndex.DimsWeight, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.DimsWeight, value); }
		}

		/// <summary> The DimsAddWeight property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."DimsAddWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Boolean DimsAddWeight
		{
			get { return (System.Boolean)GetValue((int)DhlEcommerceShipmentFieldIndex.DimsAddWeight, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.DimsAddWeight, value); }
		}

		/// <summary> The Reference1 property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."Reference1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String Reference1
		{
			get { return (System.String)GetValue((int)DhlEcommerceShipmentFieldIndex.Reference1, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.Reference1, value); }
		}

		/// <summary> The InsuranceValue property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."InsuranceValue"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Decimal InsuranceValue
		{
			get { return (System.Decimal)GetValue((int)DhlEcommerceShipmentFieldIndex.InsuranceValue, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.InsuranceValue, value); }
		}

		/// <summary> The InsurancePennyOne property of the Entity DhlEcommerceShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceShipment"."InsurancePennyOne"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Boolean InsurancePennyOne
		{
			get { return (System.Boolean)GetValue((int)DhlEcommerceShipmentFieldIndex.InsurancePennyOne, true); }
			set	{ SetValue((int)DhlEcommerceShipmentFieldIndex.InsurancePennyOne, value); }
		}

		/// <summary> Gets / sets related entity of type 'ScanFormBatchEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		[DataMember]
		public virtual ScanFormBatchEntity ScanFormBatch
		{
			get	{ return _scanFormBatch; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncScanFormBatch(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "DhlEcommerceShipment", "ScanFormBatch", _scanFormBatch, true); 
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'ShipmentEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned.<br/><br/>
		/// </summary>
		[Browsable(true)]
		[DataMember]
		public virtual ShipmentEntity Shipment
		{
			get { return _shipment; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncShipment(value);
					CallSetRelatedEntityDuringDeserialization(value, "DhlEcommerce");
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_shipment !=null);
						DesetupSyncShipment(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("Shipment");
						}
					}
					else
					{
						if(_shipment!=value)
						{
							((IEntity2)value).SetRelatedEntity(this, "DhlEcommerce");
							SetupSyncShipment(value);
						}
					}
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
			get { return (int)ShipWorks.Data.Model.EntityType.DhlEcommerceShipmentEntity; }
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
