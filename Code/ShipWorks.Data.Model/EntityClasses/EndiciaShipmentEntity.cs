﻿///////////////////////////////////////////////////////////////
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
	/// <summary>Entity class which represents the entity 'EndiciaShipment'.<br/><br/></summary>
	[Serializable]
	public partial class EndiciaShipmentEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private ScanFormBatchEntity _scanFormBatch;
		private PostalShipmentEntity _postalShipment;

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
			/// <summary>Member name PostalShipment</summary>
			public static readonly string PostalShipment = "PostalShipment";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static EndiciaShipmentEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public EndiciaShipmentEntity():base("EndiciaShipmentEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public EndiciaShipmentEntity(IEntityFields2 fields):base("EndiciaShipmentEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this EndiciaShipmentEntity</param>
		public EndiciaShipmentEntity(IValidator validator):base("EndiciaShipmentEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="shipmentID">PK value for EndiciaShipment which data should be fetched into this EndiciaShipment object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public EndiciaShipmentEntity(System.Int64 shipmentID):base("EndiciaShipmentEntity")
		{
			InitClassEmpty(null, null);
			this.ShipmentID = shipmentID;
		}

		/// <summary> CTor</summary>
		/// <param name="shipmentID">PK value for EndiciaShipment which data should be fetched into this EndiciaShipment object</param>
		/// <param name="validator">The custom validator object for this EndiciaShipmentEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public EndiciaShipmentEntity(System.Int64 shipmentID, IValidator validator):base("EndiciaShipmentEntity")
		{
			InitClassEmpty(validator, null);
			this.ShipmentID = shipmentID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected EndiciaShipmentEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_scanFormBatch = (ScanFormBatchEntity)info.GetValue("_scanFormBatch", typeof(ScanFormBatchEntity));
				if(_scanFormBatch!=null)
				{
					_scanFormBatch.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_postalShipment = (PostalShipmentEntity)info.GetValue("_postalShipment", typeof(PostalShipmentEntity));
				if(_postalShipment!=null)
				{
					_postalShipment.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((EndiciaShipmentFieldIndex)fieldIndex)
			{
				case EndiciaShipmentFieldIndex.ShipmentID:
					DesetupSyncPostalShipment(true, false);
					break;
				case EndiciaShipmentFieldIndex.ScanFormBatchID:
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
				case "PostalShipment":
					this.PostalShipment = (PostalShipmentEntity)entity;
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
				case "PostalShipment":
					toReturn.Add(Relations.PostalShipmentEntityUsingShipmentID);
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
				case "PostalShipment":
					SetupSyncPostalShipment(relatedEntity);
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
				case "PostalShipment":
					DesetupSyncPostalShipment(false, true);
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
			if(_postalShipment!=null)
			{
				toReturn.Add(_postalShipment);
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
				info.AddValue("_postalShipment", (!this.MarkedForDeletion?_postalShipment:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new EndiciaShipmentRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'ScanFormBatch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoScanFormBatch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ScanFormBatchFields.ScanFormBatchID, null, ComparisonOperator.Equal, this.ScanFormBatchID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'PostalShipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoPostalShipment()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(PostalShipmentFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(EndiciaShipmentEntityFactory));
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
			toReturn.Add("PostalShipment", _postalShipment);
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
			_fieldsCustomProperties.Add("EndiciaAccountID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginalEndiciaAccountID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("StealthPostage", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ReferenceID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("TransactionID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RefundFormID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ScanFormBatchID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ScanBasedReturn", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RequestedLabelFormat", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Insurance", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ReferenceID2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ReferenceID3", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ReferenceID4", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("GroupCode", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _scanFormBatch</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncScanFormBatch(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _scanFormBatch, new PropertyChangedEventHandler( OnScanFormBatchPropertyChanged ), "ScanFormBatch", ShipWorks.Data.Model.RelationClasses.StaticEndiciaShipmentRelations.ScanFormBatchEntityUsingScanFormBatchIDStatic, true, signalRelatedEntity, "EndiciaShipment", resetFKFields, new int[] { (int)EndiciaShipmentFieldIndex.ScanFormBatchID } );
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
				this.PerformSetupSyncRelatedEntity( _scanFormBatch, new PropertyChangedEventHandler( OnScanFormBatchPropertyChanged ), "ScanFormBatch", ShipWorks.Data.Model.RelationClasses.StaticEndiciaShipmentRelations.ScanFormBatchEntityUsingScanFormBatchIDStatic, true, new string[] {  } );
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

		/// <summary> Removes the sync logic for member _postalShipment</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncPostalShipment(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _postalShipment, new PropertyChangedEventHandler( OnPostalShipmentPropertyChanged ), "PostalShipment", ShipWorks.Data.Model.RelationClasses.StaticEndiciaShipmentRelations.PostalShipmentEntityUsingShipmentIDStatic, true, signalRelatedEntity, "Endicia", false, new int[] { (int)EndiciaShipmentFieldIndex.ShipmentID } );
			_postalShipment = null;
		}
		
		/// <summary> setups the sync logic for member _postalShipment</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncPostalShipment(IEntityCore relatedEntity)
		{
			if(_postalShipment!=relatedEntity)
			{
				DesetupSyncPostalShipment(true, true);
				_postalShipment = (PostalShipmentEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _postalShipment, new PropertyChangedEventHandler( OnPostalShipmentPropertyChanged ), "PostalShipment", ShipWorks.Data.Model.RelationClasses.StaticEndiciaShipmentRelations.PostalShipmentEntityUsingShipmentIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnPostalShipmentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this EndiciaShipmentEntity</param>
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
		public  static EndiciaShipmentRelations Relations
		{
			get	{ return new EndiciaShipmentRelations(); }
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
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(ScanFormBatchEntityFactory))),	(IEntityRelation)GetRelationsForField("ScanFormBatch")[0], (int)ShipWorks.Data.Model.EntityType.EndiciaShipmentEntity, (int)ShipWorks.Data.Model.EntityType.ScanFormBatchEntity, 0, null, null, null, null, "ScanFormBatch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'PostalShipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathPostalShipment
		{
			get { return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(PostalShipmentEntityFactory))), (IEntityRelation)GetRelationsForField("PostalShipment")[0], (int)ShipWorks.Data.Model.EntityType.EndiciaShipmentEntity, (int)ShipWorks.Data.Model.EntityType.PostalShipmentEntity, 0, null, null, null, null, "PostalShipment", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);	}
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

		/// <summary> The ShipmentID property of the Entity EndiciaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaShipment"."ShipmentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
		[DataMember]
		public virtual System.Int64 ShipmentID
		{
			get { return (System.Int64)GetValue((int)EndiciaShipmentFieldIndex.ShipmentID, true); }
			set	{ SetValue((int)EndiciaShipmentFieldIndex.ShipmentID, value); }
		}

		/// <summary> The EndiciaAccountID property of the Entity EndiciaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaShipment"."EndiciaAccountID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Int64 EndiciaAccountID
		{
			get { return (System.Int64)GetValue((int)EndiciaShipmentFieldIndex.EndiciaAccountID, true); }
			set	{ SetValue((int)EndiciaShipmentFieldIndex.EndiciaAccountID, value); }
		}

		/// <summary> The OriginalEndiciaAccountID property of the Entity EndiciaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaShipment"."OriginalEndiciaAccountID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int64> OriginalEndiciaAccountID
		{
			get { return (Nullable<System.Int64>)GetValue((int)EndiciaShipmentFieldIndex.OriginalEndiciaAccountID, false); }
			set	{ SetValue((int)EndiciaShipmentFieldIndex.OriginalEndiciaAccountID, value); }
		}

		/// <summary> The StealthPostage property of the Entity EndiciaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaShipment"."StealthPostage"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Boolean StealthPostage
		{
			get { return (System.Boolean)GetValue((int)EndiciaShipmentFieldIndex.StealthPostage, true); }
			set	{ SetValue((int)EndiciaShipmentFieldIndex.StealthPostage, value); }
		}

		/// <summary> The ReferenceID property of the Entity EndiciaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaShipment"."ReferenceID"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String ReferenceID
		{
			get { return (System.String)GetValue((int)EndiciaShipmentFieldIndex.ReferenceID, true); }
			set	{ SetValue((int)EndiciaShipmentFieldIndex.ReferenceID, value); }
		}

		/// <summary> The TransactionID property of the Entity EndiciaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaShipment"."TransactionID"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> TransactionID
		{
			get { return (Nullable<System.Int32>)GetValue((int)EndiciaShipmentFieldIndex.TransactionID, false); }
			set	{ SetValue((int)EndiciaShipmentFieldIndex.TransactionID, value); }
		}

		/// <summary> The RefundFormID property of the Entity EndiciaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaShipment"."RefundFormID"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> RefundFormID
		{
			get { return (Nullable<System.Int32>)GetValue((int)EndiciaShipmentFieldIndex.RefundFormID, false); }
			set	{ SetValue((int)EndiciaShipmentFieldIndex.RefundFormID, value); }
		}

		/// <summary> The ScanFormBatchID property of the Entity EndiciaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaShipment"."ScanFormBatchID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int64> ScanFormBatchID
		{
			get { return (Nullable<System.Int64>)GetValue((int)EndiciaShipmentFieldIndex.ScanFormBatchID, false); }
			set	{ SetValue((int)EndiciaShipmentFieldIndex.ScanFormBatchID, value); }
		}

		/// <summary> The ScanBasedReturn property of the Entity EndiciaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaShipment"."ScanBasedReturn"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Boolean ScanBasedReturn
		{
			get { return (System.Boolean)GetValue((int)EndiciaShipmentFieldIndex.ScanBasedReturn, true); }
			set	{ SetValue((int)EndiciaShipmentFieldIndex.ScanBasedReturn, value); }
		}

		/// <summary> The RequestedLabelFormat property of the Entity EndiciaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaShipment"."RequestedLabelFormat"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Int32 RequestedLabelFormat
		{
			get { return (System.Int32)GetValue((int)EndiciaShipmentFieldIndex.RequestedLabelFormat, true); }
			set	{ SetValue((int)EndiciaShipmentFieldIndex.RequestedLabelFormat, value); }
		}

		/// <summary> The Insurance property of the Entity EndiciaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaShipment"."Insurance"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Boolean Insurance
		{
			get { return (System.Boolean)GetValue((int)EndiciaShipmentFieldIndex.Insurance, true); }
			set	{ SetValue((int)EndiciaShipmentFieldIndex.Insurance, value); }
		}

		/// <summary> The ReferenceID2 property of the Entity EndiciaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaShipment"."ReferenceID2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String ReferenceID2
		{
			get { return (System.String)GetValue((int)EndiciaShipmentFieldIndex.ReferenceID2, true); }
			set	{ SetValue((int)EndiciaShipmentFieldIndex.ReferenceID2, value); }
		}

		/// <summary> The ReferenceID3 property of the Entity EndiciaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaShipment"."ReferenceID3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String ReferenceID3
		{
			get { return (System.String)GetValue((int)EndiciaShipmentFieldIndex.ReferenceID3, true); }
			set	{ SetValue((int)EndiciaShipmentFieldIndex.ReferenceID3, value); }
		}

		/// <summary> The ReferenceID4 property of the Entity EndiciaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaShipment"."ReferenceID4"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String ReferenceID4
		{
			get { return (System.String)GetValue((int)EndiciaShipmentFieldIndex.ReferenceID4, true); }
			set	{ SetValue((int)EndiciaShipmentFieldIndex.ReferenceID4, value); }
		}

		/// <summary> The GroupCode property of the Entity EndiciaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaShipment"."GroupCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String GroupCode
		{
			get { return (System.String)GetValue((int)EndiciaShipmentFieldIndex.GroupCode, true); }
			set	{ SetValue((int)EndiciaShipmentFieldIndex.GroupCode, value); }
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
					SetSingleRelatedEntityNavigator(value, "EndiciaShipment", "ScanFormBatch", _scanFormBatch, true); 
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'PostalShipmentEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned.<br/><br/>
		/// </summary>
		[Browsable(true)]
		[DataMember]
		public virtual PostalShipmentEntity PostalShipment
		{
			get { return _postalShipment; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncPostalShipment(value);
					CallSetRelatedEntityDuringDeserialization(value, "Endicia");
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_postalShipment !=null);
						DesetupSyncPostalShipment(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("PostalShipment");
						}
					}
					else
					{
						if(_postalShipment!=value)
						{
							((IEntity2)value).SetRelatedEntity(this, "Endicia");
							SetupSyncPostalShipment(value);
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
			get { return (int)ShipWorks.Data.Model.EntityType.EndiciaShipmentEntity; }
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
