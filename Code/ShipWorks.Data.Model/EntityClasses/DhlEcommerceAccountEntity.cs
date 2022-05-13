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
	/// <summary>Entity class which represents the entity 'DhlEcommerceAccount'.<br/><br/></summary>
	[Serializable]
	public partial class DhlEcommerceAccountEntity : CommonEntityBase
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
		static DhlEcommerceAccountEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public DhlEcommerceAccountEntity():base("DhlEcommerceAccountEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public DhlEcommerceAccountEntity(IEntityFields2 fields):base("DhlEcommerceAccountEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this DhlEcommerceAccountEntity</param>
		public DhlEcommerceAccountEntity(IValidator validator):base("DhlEcommerceAccountEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="dhlEcommerceAccountID">PK value for DhlEcommerceAccount which data should be fetched into this DhlEcommerceAccount object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public DhlEcommerceAccountEntity(System.Int64 dhlEcommerceAccountID):base("DhlEcommerceAccountEntity")
		{
			InitClassEmpty(null, null);
			this.DhlEcommerceAccountID = dhlEcommerceAccountID;
		}

		/// <summary> CTor</summary>
		/// <param name="dhlEcommerceAccountID">PK value for DhlEcommerceAccount which data should be fetched into this DhlEcommerceAccount object</param>
		/// <param name="validator">The custom validator object for this DhlEcommerceAccountEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public DhlEcommerceAccountEntity(System.Int64 dhlEcommerceAccountID, IValidator validator):base("DhlEcommerceAccountEntity")
		{
			InitClassEmpty(validator, null);
			this.DhlEcommerceAccountID = dhlEcommerceAccountID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected DhlEcommerceAccountEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				this.FixupDeserialization(FieldInfoProviderSingleton.GetInstance());
			}
			// __LLBLGENPRO_USER_CODE_REGION_START DeserializationConstructor
			// __LLBLGENPRO_USER_CODE_REGION_END
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
			return new DhlEcommerceAccountRelations().GetAllRelations();
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(DhlEcommerceAccountEntityFactory));
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
			_fieldsCustomProperties.Add("DhlEcommerceAccountID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RowVersion", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipEngineCarrierId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ClientId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ApiSecret", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PickupNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DistributionCenter", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SoldTo", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Description", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FirstName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("MiddleName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("LastName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Company", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Street1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("City", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("StateProvCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PostalCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CountryCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Phone", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Email", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CreatedDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Street2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Street3", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AncillaryEndorsement", fieldHashtable);
		}
		#endregion

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this DhlEcommerceAccountEntity</param>
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
		public  static DhlEcommerceAccountRelations Relations
		{
			get	{ return new DhlEcommerceAccountRelations(); }
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

		/// <summary> The DhlEcommerceAccountID property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."DhlEcommerceAccountID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		[DataMember]
		public virtual System.Int64 DhlEcommerceAccountID
		{
			get { return (System.Int64)GetValue((int)DhlEcommerceAccountFieldIndex.DhlEcommerceAccountID, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.DhlEcommerceAccountID, value); }
		}

		/// <summary> The RowVersion property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."RowVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Byte[] RowVersion
		{
			get { return (System.Byte[])GetValue((int)DhlEcommerceAccountFieldIndex.RowVersion, true); }

		}

		/// <summary> The ShipEngineCarrierId property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."ShipEngineCarrierId"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String ShipEngineCarrierId
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.ShipEngineCarrierId, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.ShipEngineCarrierId, value); }
		}

		/// <summary> The ClientId property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."ClientId"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String ClientId
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.ClientId, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.ClientId, value); }
		}

		/// <summary> The ApiSecret property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."ApiSecret"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 400<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String ApiSecret
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.ApiSecret, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.ApiSecret, value); }
		}

		/// <summary> The PickupNumber property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."PickupNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String PickupNumber
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.PickupNumber, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.PickupNumber, value); }
		}

		/// <summary> The DistributionCenter property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."DistributionCenter"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 12<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String DistributionCenter
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.DistributionCenter, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.DistributionCenter, value); }
		}

		/// <summary> The SoldTo property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."SoldTo"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String SoldTo
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.SoldTo, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.SoldTo, value); }
		}

		/// <summary> The Description property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."Description"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String Description
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.Description, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.Description, value); }
		}

		/// <summary> The FirstName property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."FirstName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String FirstName
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.FirstName, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.FirstName, value); }
		}

		/// <summary> The MiddleName property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."MiddleName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String MiddleName
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.MiddleName, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.MiddleName, value); }
		}

		/// <summary> The LastName property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."LastName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String LastName
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.LastName, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.LastName, value); }
		}

		/// <summary> The Company property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."Company"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String Company
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.Company, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.Company, value); }
		}

		/// <summary> The Street1 property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."Street1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String Street1
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.Street1, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.Street1, value); }
		}

		/// <summary> The City property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."City"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String City
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.City, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.City, value); }
		}

		/// <summary> The StateProvCode property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."StateProvCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String StateProvCode
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.StateProvCode, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.StateProvCode, value); }
		}

		/// <summary> The PostalCode property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."PostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String PostalCode
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.PostalCode, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.PostalCode, value); }
		}

		/// <summary> The CountryCode property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."CountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String CountryCode
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.CountryCode, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.CountryCode, value); }
		}

		/// <summary> The Phone property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."Phone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 26<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String Phone
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.Phone, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.Phone, value); }
		}

		/// <summary> The Email property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."Email"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String Email
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.Email, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.Email, value); }
		}

		/// <summary> The CreatedDate property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."CreatedDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.DateTime CreatedDate
		{
			get { return (System.DateTime)GetValue((int)DhlEcommerceAccountFieldIndex.CreatedDate, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.CreatedDate, value); }
		}

		/// <summary> The Street2 property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."Street2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String Street2
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.Street2, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.Street2, value); }
		}

		/// <summary> The Street3 property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."Street3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String Street3
		{
			get { return (System.String)GetValue((int)DhlEcommerceAccountFieldIndex.Street3, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.Street3, value); }
		}

		/// <summary> The AncillaryEndorsement property of the Entity DhlEcommerceAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlEcommerceAccount"."AncillaryEndorsement"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Int32 AncillaryEndorsement
		{
			get { return (System.Int32)GetValue((int)DhlEcommerceAccountFieldIndex.AncillaryEndorsement, true); }
			set	{ SetValue((int)DhlEcommerceAccountFieldIndex.AncillaryEndorsement, value); }
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
			get { return (int)ShipWorks.Data.Model.EntityType.DhlEcommerceAccountEntity; }
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
