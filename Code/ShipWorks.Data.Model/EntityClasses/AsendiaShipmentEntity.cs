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
	
	/// <summary>Entity class which represents the entity 'AsendiaShipment'.<br/><br/></summary>
	[Serializable]
	public partial class AsendiaShipmentEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END
			
	{
		#region Class Member Declarations
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
			/// <summary>Member name Shipment</summary>
			public static readonly string Shipment = "Shipment";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static AsendiaShipmentEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public AsendiaShipmentEntity():base("AsendiaShipmentEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public AsendiaShipmentEntity(IEntityFields2 fields):base("AsendiaShipmentEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this AsendiaShipmentEntity</param>
		public AsendiaShipmentEntity(IValidator validator):base("AsendiaShipmentEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="shipmentID">PK value for AsendiaShipment which data should be fetched into this AsendiaShipment object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public AsendiaShipmentEntity(System.Int64 shipmentID):base("AsendiaShipmentEntity")
		{
			InitClassEmpty(null, null);
			this.ShipmentID = shipmentID;
		}

		/// <summary> CTor</summary>
		/// <param name="shipmentID">PK value for AsendiaShipment which data should be fetched into this AsendiaShipment object</param>
		/// <param name="validator">The custom validator object for this AsendiaShipmentEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public AsendiaShipmentEntity(System.Int64 shipmentID, IValidator validator):base("AsendiaShipmentEntity")
		{
			InitClassEmpty(validator, null);
			this.ShipmentID = shipmentID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected AsendiaShipmentEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
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
			switch((AsendiaShipmentFieldIndex)fieldIndex)
			{
				case AsendiaShipmentFieldIndex.ShipmentID:
					DesetupSyncShipment(true, false);
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
			return new AsendiaShipmentRelations().GetAllRelations();
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
			return EntityFactoryCache2.GetEntityFactory(typeof(AsendiaShipmentEntityFactory));
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
			_fieldsCustomProperties.Add("AsendiaAccountID", fieldHashtable);
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
			_fieldsCustomProperties.Add("DimsProfileID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DimsLength", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DimsWidth", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DimsHeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DimsAddWeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DimsWeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Insurance", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InsuranceValue", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("TrackingNumber", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _shipment</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncShipment(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _shipment, new PropertyChangedEventHandler( OnShipmentPropertyChanged ), "Shipment", ShipWorks.Data.Model.RelationClasses.StaticAsendiaShipmentRelations.ShipmentEntityUsingShipmentIDStatic, true, signalRelatedEntity, "AsendiaShipment", false, new int[] { (int)AsendiaShipmentFieldIndex.ShipmentID } );
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
				this.PerformSetupSyncRelatedEntity( _shipment, new PropertyChangedEventHandler( OnShipmentPropertyChanged ), "Shipment", ShipWorks.Data.Model.RelationClasses.StaticAsendiaShipmentRelations.ShipmentEntityUsingShipmentIDStatic, true, new string[] {  } );
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
		/// <param name="validator">The validator object for this AsendiaShipmentEntity</param>
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
		public  static AsendiaShipmentRelations Relations
		{
			get	{ return new AsendiaShipmentRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Shipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathShipment
		{
			get { return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(ShipmentEntityFactory))), (IEntityRelation)GetRelationsForField("Shipment")[0], (int)ShipWorks.Data.Model.EntityType.AsendiaShipmentEntity, (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, 0, null, null, null, null, "Shipment", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);	}
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

		/// <summary> The ShipmentID property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."ShipmentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
		public virtual System.Int64 ShipmentID
		{
			get { return (System.Int64)GetValue((int)AsendiaShipmentFieldIndex.ShipmentID, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.ShipmentID, value); }
		}

		/// <summary> The AsendiaAccountID property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."AsendiaAccountID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 AsendiaAccountID
		{
			get { return (System.Int64)GetValue((int)AsendiaShipmentFieldIndex.AsendiaAccountID, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.AsendiaAccountID, value); }
		}

		/// <summary> The Service property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."Service"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 Service
		{
			get { return (System.Int32)GetValue((int)AsendiaShipmentFieldIndex.Service, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.Service, value); }
		}

		/// <summary> The DeliveredDutyPaid property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."DeliveredDutyPaid"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean DeliveredDutyPaid
		{
			get { return (System.Boolean)GetValue((int)AsendiaShipmentFieldIndex.DeliveredDutyPaid, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.DeliveredDutyPaid, value); }
		}

		/// <summary> The NonMachinable property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."NonMachinable"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean NonMachinable
		{
			get { return (System.Boolean)GetValue((int)AsendiaShipmentFieldIndex.NonMachinable, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.NonMachinable, value); }
		}

		/// <summary> The SaturdayDelivery property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."SaturdayDelivery"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean SaturdayDelivery
		{
			get { return (System.Boolean)GetValue((int)AsendiaShipmentFieldIndex.SaturdayDelivery, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.SaturdayDelivery, value); }
		}

		/// <summary> The RequestedLabelFormat property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."RequestedLabelFormat"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 RequestedLabelFormat
		{
			get { return (System.Int32)GetValue((int)AsendiaShipmentFieldIndex.RequestedLabelFormat, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.RequestedLabelFormat, value); }
		}

		/// <summary> The Contents property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."Contents"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 Contents
		{
			get { return (System.Int32)GetValue((int)AsendiaShipmentFieldIndex.Contents, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.Contents, value); }
		}

		/// <summary> The NonDelivery property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."NonDelivery"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 NonDelivery
		{
			get { return (System.Int32)GetValue((int)AsendiaShipmentFieldIndex.NonDelivery, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.NonDelivery, value); }
		}

		/// <summary> The ShipEngineLabelID property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."ShipEngineLabelID"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 12<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipEngineLabelID
		{
			get { return (System.String)GetValue((int)AsendiaShipmentFieldIndex.ShipEngineLabelID, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.ShipEngineLabelID, value); }
		}

		/// <summary> The DimsProfileID property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."DimsProfileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 DimsProfileID
		{
			get { return (System.Int64)GetValue((int)AsendiaShipmentFieldIndex.DimsProfileID, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.DimsProfileID, value); }
		}

		/// <summary> The DimsLength property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."DimsLength"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DimsLength
		{
			get { return (System.Double)GetValue((int)AsendiaShipmentFieldIndex.DimsLength, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.DimsLength, value); }
		}

		/// <summary> The DimsWidth property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."DimsWidth"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DimsWidth
		{
			get { return (System.Double)GetValue((int)AsendiaShipmentFieldIndex.DimsWidth, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.DimsWidth, value); }
		}

		/// <summary> The DimsHeight property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."DimsHeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DimsHeight
		{
			get { return (System.Double)GetValue((int)AsendiaShipmentFieldIndex.DimsHeight, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.DimsHeight, value); }
		}

		/// <summary> The DimsAddWeight property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."DimsAddWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean DimsAddWeight
		{
			get { return (System.Boolean)GetValue((int)AsendiaShipmentFieldIndex.DimsAddWeight, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.DimsAddWeight, value); }
		}

		/// <summary> The DimsWeight property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."DimsWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DimsWeight
		{
			get { return (System.Double)GetValue((int)AsendiaShipmentFieldIndex.DimsWeight, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.DimsWeight, value); }
		}

		/// <summary> The Insurance property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."Insurance"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Insurance
		{
			get { return (System.Boolean)GetValue((int)AsendiaShipmentFieldIndex.Insurance, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.Insurance, value); }
		}

		/// <summary> The InsuranceValue property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."InsuranceValue"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal InsuranceValue
		{
			get { return (System.Decimal)GetValue((int)AsendiaShipmentFieldIndex.InsuranceValue, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.InsuranceValue, value); }
		}

		/// <summary> The TrackingNumber property of the Entity AsendiaShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "AsendiaShipment"."TrackingNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String TrackingNumber
		{
			get { return (System.String)GetValue((int)AsendiaShipmentFieldIndex.TrackingNumber, true); }
			set	{ SetValue((int)AsendiaShipmentFieldIndex.TrackingNumber, value); }
		}

		/// <summary> Gets / sets related entity of type 'ShipmentEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned.<br/><br/>
		/// </summary>
		[Browsable(true)]
		public virtual ShipmentEntity Shipment
		{
			get { return _shipment; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncShipment(value);
					CallSetRelatedEntityDuringDeserialization(value, "AsendiaShipment");
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
							((IEntity2)value).SetRelatedEntity(this, "AsendiaShipment");
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
			get { return (int)ShipWorks.Data.Model.EntityType.AsendiaShipmentEntity; }
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
