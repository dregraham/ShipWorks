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
	/// <summary>Entity class which represents the entity 'ProcessedShipment'.<br/><br/></summary>
	[Serializable]
	public partial class ProcessedShipmentEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static ProcessedShipmentEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public ProcessedShipmentEntity():base("ProcessedShipmentEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public ProcessedShipmentEntity(IEntityFields2 fields):base("ProcessedShipmentEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this ProcessedShipmentEntity</param>
		public ProcessedShipmentEntity(IValidator validator):base("ProcessedShipmentEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="shipmentID">PK value for ProcessedShipment which data should be fetched into this ProcessedShipment object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ProcessedShipmentEntity(System.Int64 shipmentID):base("ProcessedShipmentEntity")
		{
			InitClassEmpty(null, null);
			this.ShipmentID = shipmentID;
		}

		/// <summary> CTor</summary>
		/// <param name="shipmentID">PK value for ProcessedShipment which data should be fetched into this ProcessedShipment object</param>
		/// <param name="validator">The custom validator object for this ProcessedShipmentEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ProcessedShipmentEntity(System.Int64 shipmentID, IValidator validator):base("ProcessedShipmentEntity")
		{
			InitClassEmpty(validator, null);
			this.ShipmentID = shipmentID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected ProcessedShipmentEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				this.FixupDeserialization(FieldInfoProviderSingleton.GetInstance());
			}
			// __LLBLGENPRO_USER_CODE_REGION_START DeserializationConstructor
			// __LLBLGENPRO_USER_CODE_REGION_END
		}

		
		/// <summary>Performs the desync setup when an FK field has been changed. The entity referenced based on the FK field will be dereferenced and sync info will be removed.</summary>
		/// <param name="fieldIndex">The fieldindex.</param>
		protected override void PerformDesyncSetupFKFieldChange(int fieldIndex)
		{
			switch((ProcessedShipmentFieldIndex)fieldIndex)
			{
				case ProcessedShipmentFieldIndex.ProcessedUserID:

					break;
				case ProcessedShipmentFieldIndex.ProcessedComputerID:

					break;
				case ProcessedShipmentFieldIndex.VoidedUserID:

					break;
				case ProcessedShipmentFieldIndex.VoidedComputerID:

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
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new ProcessedShipmentRelations().GetAllRelations();
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(ProcessedShipmentEntityFactory));
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
			_fieldsCustomProperties.Add("ShipmentType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Insurance", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InsuranceProvider", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ProcessedDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ProcessedUserID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ProcessedComputerID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Voided", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("VoidedDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("VoidedUserID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("VoidedComputerID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("TotalWeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("TrackingNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipmentCost", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipSenseStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipAddressValidationStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipResidentialStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipPOBox", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipMilitaryAddress", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RequestedLabelFormat", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ActualLabelFormat", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OrderID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OrderNumberComplete", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Service", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipUSTerritory", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ProcessedWithUiMode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CombineSplitStatus", fieldHashtable);
		}
		#endregion

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this ProcessedShipmentEntity</param>
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
		public  static ProcessedShipmentRelations Relations
		{
			get	{ return new ProcessedShipmentRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
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

		/// <summary> The ShipmentID property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."ShipmentID"<br/>
		/// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
		public virtual System.Int64 ShipmentID
		{
			get { return (System.Int64)GetValue((int)ProcessedShipmentFieldIndex.ShipmentID, true); }
			set	{ SetValue((int)ProcessedShipmentFieldIndex.ShipmentID, value); }
		}

		/// <summary> The ShipmentType property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."ShipmentType"<br/>
		/// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual ShipWorks.Shipping.ShipmentTypeCode ShipmentType
		{
			get { return (ShipWorks.Shipping.ShipmentTypeCode)GetValue((int)ProcessedShipmentFieldIndex.ShipmentType, true); }

		}

		/// <summary> The ShipDate property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."ShipDate"<br/>
		/// View field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime ShipDate
		{
			get { return (System.DateTime)GetValue((int)ProcessedShipmentFieldIndex.ShipDate, true); }

		}

		/// <summary> The Insurance property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."Insurance"<br/>
		/// View field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Insurance
		{
			get { return (System.Boolean)GetValue((int)ProcessedShipmentFieldIndex.Insurance, true); }

		}

		/// <summary> The InsuranceProvider property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."InsuranceProvider"<br/>
		/// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 InsuranceProvider
		{
			get { return (System.Int32)GetValue((int)ProcessedShipmentFieldIndex.InsuranceProvider, true); }

		}

		/// <summary> The ProcessedDate property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."ProcessedDate"<br/>
		/// View field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.DateTime> ProcessedDate
		{
			get { return (Nullable<System.DateTime>)GetValue((int)ProcessedShipmentFieldIndex.ProcessedDate, false); }

		}

		/// <summary> The ProcessedUserID property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."ProcessedUserID"<br/>
		/// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> ProcessedUserID
		{
			get { return (Nullable<System.Int64>)GetValue((int)ProcessedShipmentFieldIndex.ProcessedUserID, false); }

		}

		/// <summary> The ProcessedComputerID property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."ProcessedComputerID"<br/>
		/// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> ProcessedComputerID
		{
			get { return (Nullable<System.Int64>)GetValue((int)ProcessedShipmentFieldIndex.ProcessedComputerID, false); }

		}

		/// <summary> The Voided property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."Voided"<br/>
		/// View field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Voided
		{
			get { return (System.Boolean)GetValue((int)ProcessedShipmentFieldIndex.Voided, true); }

		}

		/// <summary> The VoidedDate property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."VoidedDate"<br/>
		/// View field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.DateTime> VoidedDate
		{
			get { return (Nullable<System.DateTime>)GetValue((int)ProcessedShipmentFieldIndex.VoidedDate, false); }

		}

		/// <summary> The VoidedUserID property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."VoidedUserID"<br/>
		/// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> VoidedUserID
		{
			get { return (Nullable<System.Int64>)GetValue((int)ProcessedShipmentFieldIndex.VoidedUserID, false); }

		}

		/// <summary> The VoidedComputerID property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."VoidedComputerID"<br/>
		/// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> VoidedComputerID
		{
			get { return (Nullable<System.Int64>)GetValue((int)ProcessedShipmentFieldIndex.VoidedComputerID, false); }

		}

		/// <summary> The TotalWeight property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."TotalWeight"<br/>
		/// View field type characteristics (type, precision, scale, length): Decimal, 29, 9, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal TotalWeight
		{
			get { return (System.Decimal)GetValue((int)ProcessedShipmentFieldIndex.TotalWeight, true); }

		}

		/// <summary> The TrackingNumber property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."TrackingNumber"<br/>
		/// View field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String TrackingNumber
		{
			get { return (System.String)GetValue((int)ProcessedShipmentFieldIndex.TrackingNumber, true); }

		}

		/// <summary> The ShipmentCost property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."ShipmentCost"<br/>
		/// View field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal ShipmentCost
		{
			get { return (System.Decimal)GetValue((int)ProcessedShipmentFieldIndex.ShipmentCost, true); }

		}

		/// <summary> The ShipSenseStatus property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."ShipSenseStatus"<br/>
		/// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipSenseStatus
		{
			get { return (System.Int32)GetValue((int)ProcessedShipmentFieldIndex.ShipSenseStatus, true); }

		}

		/// <summary> The ShipAddressValidationStatus property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."ShipAddressValidationStatus"<br/>
		/// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipAddressValidationStatus
		{
			get { return (System.Int32)GetValue((int)ProcessedShipmentFieldIndex.ShipAddressValidationStatus, true); }

		}

		/// <summary> The ShipResidentialStatus property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."ShipResidentialStatus"<br/>
		/// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipResidentialStatus
		{
			get { return (System.Int32)GetValue((int)ProcessedShipmentFieldIndex.ShipResidentialStatus, true); }

		}

		/// <summary> The ShipPOBox property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."ShipPOBox"<br/>
		/// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipPOBox
		{
			get { return (System.Int32)GetValue((int)ProcessedShipmentFieldIndex.ShipPOBox, true); }

		}

		/// <summary> The ShipMilitaryAddress property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."ShipMilitaryAddress"<br/>
		/// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipMilitaryAddress
		{
			get { return (System.Int32)GetValue((int)ProcessedShipmentFieldIndex.ShipMilitaryAddress, true); }

		}

		/// <summary> The RequestedLabelFormat property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."RequestedLabelFormat"<br/>
		/// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 RequestedLabelFormat
		{
			get { return (System.Int32)GetValue((int)ProcessedShipmentFieldIndex.RequestedLabelFormat, true); }

		}

		/// <summary> The ActualLabelFormat property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."ActualLabelFormat"<br/>
		/// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> ActualLabelFormat
		{
			get { return (Nullable<System.Int32>)GetValue((int)ProcessedShipmentFieldIndex.ActualLabelFormat, false); }

		}

		/// <summary> The OrderID property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."OrderID"<br/>
		/// View field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 OrderID
		{
			get { return (System.Int64)GetValue((int)ProcessedShipmentFieldIndex.OrderID, true); }

		}

		/// <summary> The OrderNumberComplete property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."OrderNumberComplete"<br/>
		/// View field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OrderNumberComplete
		{
			get { return (System.String)GetValue((int)ProcessedShipmentFieldIndex.OrderNumberComplete, true); }

		}

		/// <summary> The Service property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."Service"<br/>
		/// View field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Service
		{
			get { return (System.String)GetValue((int)ProcessedShipmentFieldIndex.Service, true); }

		}

		/// <summary> The ShipUSTerritory property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."ShipUSTerritory"<br/>
		/// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipUSTerritory
		{
			get { return (System.Int32)GetValue((int)ProcessedShipmentFieldIndex.ShipUSTerritory, true); }
			set	{ SetValue((int)ProcessedShipmentFieldIndex.ShipUSTerritory, value); }
		}

		/// <summary> The ProcessedWithUiMode property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."ProcessedWithUiMode"<br/>
		/// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> ProcessedWithUiMode
		{
			get { return (Nullable<System.Int32>)GetValue((int)ProcessedShipmentFieldIndex.ProcessedWithUiMode, false); }
			set	{ SetValue((int)ProcessedShipmentFieldIndex.ProcessedWithUiMode, value); }
		}

		/// <summary> The CombineSplitStatus property of the Entity ProcessedShipment<br/><br/></summary>
		/// <remarks>Mapped on  view field: "ProcessedShipmentsView"."CombineSplitStatus"<br/>
		/// View field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// View field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 CombineSplitStatus
		{
			get { return (System.Int32)GetValue((int)ProcessedShipmentFieldIndex.CombineSplitStatus, true); }
			set	{ SetValue((int)ProcessedShipmentFieldIndex.CombineSplitStatus, value); }
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
			get { return (int)ShipWorks.Data.Model.EntityType.ProcessedShipmentEntity; }
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
