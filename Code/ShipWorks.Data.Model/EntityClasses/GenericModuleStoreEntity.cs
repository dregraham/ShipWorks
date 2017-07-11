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
	/// <summary>Entity class which represents the entity 'GenericModuleStore'.<br/><br/></summary>
	[Serializable]
	public partial class GenericModuleStoreEntity : StoreEntity
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
		public static new partial class MemberNames
		{
			/// <summary>Member name OrderSearch</summary>
			public static readonly string OrderSearch = "OrderSearch";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static GenericModuleStoreEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public GenericModuleStoreEntity()
		{
			InitClassEmpty();
			SetName("GenericModuleStoreEntity");
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public GenericModuleStoreEntity(IEntityFields2 fields):base(fields)
		{
			InitClassEmpty();
			SetName("GenericModuleStoreEntity");
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this GenericModuleStoreEntity</param>
		public GenericModuleStoreEntity(IValidator validator):base(validator)
		{
			InitClassEmpty();
			SetName("GenericModuleStoreEntity");
		}
				
		/// <summary> CTor</summary>
		/// <param name="storeID">PK value for GenericModuleStore which data should be fetched into this GenericModuleStore object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public GenericModuleStoreEntity(System.Int64 storeID):base(storeID)
		{
			InitClassEmpty();

			SetName("GenericModuleStoreEntity");
		}

		/// <summary> CTor</summary>
		/// <param name="storeID">PK value for GenericModuleStore which data should be fetched into this GenericModuleStore object</param>
		/// <param name="validator">The custom validator object for this GenericModuleStoreEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public GenericModuleStoreEntity(System.Int64 storeID, IValidator validator):base(storeID, validator)
		{
			InitClassEmpty();

			SetName("GenericModuleStoreEntity");
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected GenericModuleStoreEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{

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
					base.SetRelatedEntityProperty(propertyName, entity);
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
		internal static new RelationCollection GetRelationsForField(string fieldName)
		{
			RelationCollection toReturn = new RelationCollection();
			switch(fieldName)
			{
				default:
					toReturn = StoreEntity.GetRelationsForField(fieldName);
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
					base.SetRelatedEntity(relatedEntity, fieldName);
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
					base.UnsetRelatedEntity(relatedEntity, fieldName, signalRelatedEntityManyToOne);
					break;
			}
		}

		/// <summary> Gets a collection of related entities referenced by this entity which depend on this entity (this entity is the PK side of their FK fields). These entities will have to be persisted after this entity during a recursive save.</summary>
		/// <returns>Collection with 0 or more IEntity2 objects, referenced by this entity</returns>
		protected override List<IEntity2> GetDependingRelatedEntities()
		{
			List<IEntity2> toReturn = new List<IEntity2>();
			toReturn.AddRange(base.GetDependingRelatedEntities());
			return toReturn;
		}
		
		/// <summary> Gets a collection of related entities referenced by this entity which this entity depends on (this entity is the FK side of their PK fields). These
		/// entities will have to be persisted before this entity during a recursive save.</summary>
		/// <returns>Collection with 0 or more IEntity2 objects, referenced by this entity</returns>
		protected override List<IEntity2> GetDependentRelatedEntities()
		{
			List<IEntity2> toReturn = new List<IEntity2>();
			toReturn.AddRange(base.GetDependentRelatedEntities());
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.AddRange(base.GetMemberEntityCollections());
			return toReturn;
		}

		/// <summary>Gets a predicateexpression which filters on this entity</summary>
		/// <returns>ready to use predicateexpression</returns>
		/// <remarks>Only useful in entity fetches.</remarks>
		public new static IPredicateExpression GetEntityTypeFilter()
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetEntityTypeFilter("GenericModuleStoreEntity", false);
		}
		
		/// <summary>Gets a predicateexpression which filters on this entity</summary>
		/// <param name="negate">Flag to produce a NOT filter, (true), or a normal filter (false). </param>
		/// <returns>ready to use predicateexpression</returns>
		/// <remarks>Only useful in entity fetches.</remarks>
		public new static IPredicateExpression GetEntityTypeFilter(bool negate)
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetEntityTypeFilter("GenericModuleStoreEntity", negate);
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

		
		/// <summary>Determines whether this entity is a subType of the entity represented by the passed in enum value, which represents a value in the ShipWorks.Data.Model.EntityType enum</summary>
		/// <param name="typeOfEntity">Type of entity.</param>
		/// <returns>true if the passed in type is a supertype of this entity, otherwise false</returns>
		protected override bool CheckIfIsSubTypeOf(int typeOfEntity)
		{
			return InheritanceInfoProviderSingleton.GetInstance().CheckIfIsSubTypeOf("GenericModuleStoreEntity", ((ShipWorks.Data.Model.EntityType)typeOfEntity).ToString());
		}
				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new GenericModuleStoreRelations().GetAllRelations();
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(GenericModuleStoreEntityFactory));
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
			Dictionary<string, object> toReturn = base.GetRelatedData();
			return toReturn;
		}

		/// <summary> Initializes the class members</summary>
		private void InitClassMembers()
		{
			
			// __LLBLGENPRO_USER_CODE_REGION_START InitClassMembers
			// __LLBLGENPRO_USER_CODE_REGION_END
		}


		#region Custom Property Hashtable Setup
		/// <summary> Initializes the hashtables for the entity type and entity field custom properties. </summary>
		private static void SetupCustomPropertyHashtables()
		{
			_customProperties = new Dictionary<string, string>();
			_fieldsCustomProperties = new Dictionary<string, Dictionary<string, string>>();
			Dictionary<string, string> fieldHashtable;
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModuleUsername", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModulePassword", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModuleUrl", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModuleVersion", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModulePlatform", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModuleDeveloper", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModuleOnlineStoreCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModuleStatusCodes", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModuleDownloadPageSize", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModuleRequestTimeout", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModuleDownloadStrategy", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModuleOnlineStatusSupport", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModuleOnlineStatusDataType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModuleOnlineCustomerSupport", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModuleOnlineCustomerDataType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModuleOnlineShipmentDetails", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModuleHttpExpect100Continue", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ModuleResponseEncoding", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SchemaVersion", fieldHashtable);
		}
		#endregion

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this GenericModuleStoreEntity</param>
		private void InitClassEmpty()
		{
			InitClassMembers();

			// __LLBLGENPRO_USER_CODE_REGION_START InitClassEmpty
			// __LLBLGENPRO_USER_CODE_REGION_END


		}

		#region Class Property Declarations
		/// <summary> The relations object holding all relations of this entity with other entity classes.</summary>
		public new static GenericModuleStoreRelations Relations
		{
			get	{ return new GenericModuleStoreRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public new static Dictionary<string, string> CustomProperties
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
		public new static Dictionary<string, Dictionary<string, string>> FieldsCustomProperties
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

		/// <summary> The ModuleUsername property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModuleUsername"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ModuleUsername
		{
			get { return (System.String)GetValue((int)GenericModuleStoreFieldIndex.ModuleUsername, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModuleUsername, value); }
		}

		/// <summary> The ModulePassword property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModulePassword"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ModulePassword
		{
			get { return (System.String)GetValue((int)GenericModuleStoreFieldIndex.ModulePassword, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModulePassword, value); }
		}

		/// <summary> The ModuleUrl property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModuleUrl"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 350<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ModuleUrl
		{
			get { return (System.String)GetValue((int)GenericModuleStoreFieldIndex.ModuleUrl, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModuleUrl, value); }
		}

		/// <summary> The ModuleVersion property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModuleVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ModuleVersion
		{
			get { return (System.String)GetValue((int)GenericModuleStoreFieldIndex.ModuleVersion, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModuleVersion, value); }
		}

		/// <summary> The ModulePlatform property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModulePlatform"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ModulePlatform
		{
			get { return (System.String)GetValue((int)GenericModuleStoreFieldIndex.ModulePlatform, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModulePlatform, value); }
		}

		/// <summary> The ModuleDeveloper property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModuleDeveloper"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ModuleDeveloper
		{
			get { return (System.String)GetValue((int)GenericModuleStoreFieldIndex.ModuleDeveloper, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModuleDeveloper, value); }
		}

		/// <summary> The ModuleOnlineStoreCode property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModuleOnlineStoreCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ModuleOnlineStoreCode
		{
			get { return (System.String)GetValue((int)GenericModuleStoreFieldIndex.ModuleOnlineStoreCode, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModuleOnlineStoreCode, value); }
		}

		/// <summary> The ModuleStatusCodes property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModuleStatusCodes"<br/>
		/// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ModuleStatusCodes
		{
			get { return (System.String)GetValue((int)GenericModuleStoreFieldIndex.ModuleStatusCodes, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModuleStatusCodes, value); }
		}

		/// <summary> The ModuleDownloadPageSize property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModuleDownloadPageSize"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ModuleDownloadPageSize
		{
			get { return (System.Int32)GetValue((int)GenericModuleStoreFieldIndex.ModuleDownloadPageSize, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModuleDownloadPageSize, value); }
		}

		/// <summary> The ModuleRequestTimeout property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModuleRequestTimeout"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ModuleRequestTimeout
		{
			get { return (System.Int32)GetValue((int)GenericModuleStoreFieldIndex.ModuleRequestTimeout, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModuleRequestTimeout, value); }
		}

		/// <summary> The ModuleDownloadStrategy property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModuleDownloadStrategy"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ModuleDownloadStrategy
		{
			get { return (System.Int32)GetValue((int)GenericModuleStoreFieldIndex.ModuleDownloadStrategy, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModuleDownloadStrategy, value); }
		}

		/// <summary> The ModuleOnlineStatusSupport property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModuleOnlineStatusSupport"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ModuleOnlineStatusSupport
		{
			get { return (System.Int32)GetValue((int)GenericModuleStoreFieldIndex.ModuleOnlineStatusSupport, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModuleOnlineStatusSupport, value); }
		}

		/// <summary> The ModuleOnlineStatusDataType property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModuleOnlineStatusDataType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ModuleOnlineStatusDataType
		{
			get { return (System.Int32)GetValue((int)GenericModuleStoreFieldIndex.ModuleOnlineStatusDataType, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModuleOnlineStatusDataType, value); }
		}

		/// <summary> The ModuleOnlineCustomerSupport property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModuleOnlineCustomerSupport"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean ModuleOnlineCustomerSupport
		{
			get { return (System.Boolean)GetValue((int)GenericModuleStoreFieldIndex.ModuleOnlineCustomerSupport, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModuleOnlineCustomerSupport, value); }
		}

		/// <summary> The ModuleOnlineCustomerDataType property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModuleOnlineCustomerDataType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ModuleOnlineCustomerDataType
		{
			get { return (System.Int32)GetValue((int)GenericModuleStoreFieldIndex.ModuleOnlineCustomerDataType, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModuleOnlineCustomerDataType, value); }
		}

		/// <summary> The ModuleOnlineShipmentDetails property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModuleOnlineShipmentDetails"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean ModuleOnlineShipmentDetails
		{
			get { return (System.Boolean)GetValue((int)GenericModuleStoreFieldIndex.ModuleOnlineShipmentDetails, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModuleOnlineShipmentDetails, value); }
		}

		/// <summary> The ModuleHttpExpect100Continue property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModuleHttpExpect100Continue"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean ModuleHttpExpect100Continue
		{
			get { return (System.Boolean)GetValue((int)GenericModuleStoreFieldIndex.ModuleHttpExpect100Continue, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModuleHttpExpect100Continue, value); }
		}

		/// <summary> The ModuleResponseEncoding property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."ModuleResponseEncoding"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ModuleResponseEncoding
		{
			get { return (System.Int32)GetValue((int)GenericModuleStoreFieldIndex.ModuleResponseEncoding, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.ModuleResponseEncoding, value); }
		}

		/// <summary> The SchemaVersion property of the Entity GenericModuleStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "GenericModuleStore"."SchemaVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String SchemaVersion
		{
			get { return (System.String)GetValue((int)GenericModuleStoreFieldIndex.SchemaVersion, true); }
			set	{ SetValue((int)GenericModuleStoreFieldIndex.SchemaVersion, value); }
		}
	
		/// <summary> Gets the type of the hierarchy this entity is in. </summary>
		protected override InheritanceHierarchyType LLBLGenProIsInHierarchyOfType
		{
			get { return InheritanceHierarchyType.TargetPerEntity;}
		}
		
		/// <summary> Gets or sets a value indicating whether this entity is a subtype</summary>
		protected override bool LLBLGenProIsSubType
		{
			get { return true;}
		}
		
		/// <summary>Returns the ShipWorks.Data.Model.EntityType enum value for this entity.</summary>
		[Browsable(false), XmlIgnore]
		protected override int LLBLGenProEntityTypeValue 
		{ 
			get { return (int)ShipWorks.Data.Model.EntityType.GenericModuleStoreEntity; }
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
