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
	/// <summary>Entity class which represents the entity 'EndiciaAccount'.<br/><br/></summary>
	[Serializable]
	public partial class EndiciaAccountEntity : CommonEntityBase
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
		static EndiciaAccountEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public EndiciaAccountEntity():base("EndiciaAccountEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public EndiciaAccountEntity(IEntityFields2 fields):base("EndiciaAccountEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this EndiciaAccountEntity</param>
		public EndiciaAccountEntity(IValidator validator):base("EndiciaAccountEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="endiciaAccountID">PK value for EndiciaAccount which data should be fetched into this EndiciaAccount object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public EndiciaAccountEntity(System.Int64 endiciaAccountID):base("EndiciaAccountEntity")
		{
			InitClassEmpty(null, null);
			this.EndiciaAccountID = endiciaAccountID;
		}

		/// <summary> CTor</summary>
		/// <param name="endiciaAccountID">PK value for EndiciaAccount which data should be fetched into this EndiciaAccount object</param>
		/// <param name="validator">The custom validator object for this EndiciaAccountEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public EndiciaAccountEntity(System.Int64 endiciaAccountID, IValidator validator):base("EndiciaAccountEntity")
		{
			InitClassEmpty(validator, null);
			this.EndiciaAccountID = endiciaAccountID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected EndiciaAccountEntity(SerializationInfo info, StreamingContext context) : base(info, context)
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
			return new EndiciaAccountRelations().GetAllRelations();
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(EndiciaAccountEntityFactory));
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
			_fieldsCustomProperties.Add("EndiciaAccountID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("EndiciaReseller", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AccountNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SignupConfirmation", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("WebPassword", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ApiInitialPassword", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ApiUserPassword", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AccountType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("TestAccount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CreatedByShipWorks", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Description", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FirstName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("LastName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Company", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Street1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Street2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Street3", fieldHashtable);
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
			_fieldsCustomProperties.Add("Fax", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Email", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("MailingPostalCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ScanFormAddressSource", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AcceptedFCMILetterWarning", fieldHashtable);
		}
		#endregion

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this EndiciaAccountEntity</param>
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
		public  static EndiciaAccountRelations Relations
		{
			get	{ return new EndiciaAccountRelations(); }
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

		/// <summary> The EndiciaAccountID property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."EndiciaAccountID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 EndiciaAccountID
		{
			get { return (System.Int64)GetValue((int)EndiciaAccountFieldIndex.EndiciaAccountID, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.EndiciaAccountID, value); }
		}

		/// <summary> The EndiciaReseller property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."EndiciaReseller"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 EndiciaReseller
		{
			get { return (System.Int32)GetValue((int)EndiciaAccountFieldIndex.EndiciaReseller, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.EndiciaReseller, value); }
		}

		/// <summary> The AccountNumber property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."AccountNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String AccountNumber
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.AccountNumber, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.AccountNumber, value); }
		}

		/// <summary> The SignupConfirmation property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."SignupConfirmation"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String SignupConfirmation
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.SignupConfirmation, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.SignupConfirmation, value); }
		}

		/// <summary> The WebPassword property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."WebPassword"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String WebPassword
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.WebPassword, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.WebPassword, value); }
		}

		/// <summary> The ApiInitialPassword property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."ApiInitialPassword"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ApiInitialPassword
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.ApiInitialPassword, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.ApiInitialPassword, value); }
		}

		/// <summary> The ApiUserPassword property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."ApiUserPassword"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ApiUserPassword
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.ApiUserPassword, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.ApiUserPassword, value); }
		}

		/// <summary> The AccountType property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."AccountType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 AccountType
		{
			get { return (System.Int32)GetValue((int)EndiciaAccountFieldIndex.AccountType, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.AccountType, value); }
		}

		/// <summary> The TestAccount property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."TestAccount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean TestAccount
		{
			get { return (System.Boolean)GetValue((int)EndiciaAccountFieldIndex.TestAccount, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.TestAccount, value); }
		}

		/// <summary> The CreatedByShipWorks property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."CreatedByShipWorks"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean CreatedByShipWorks
		{
			get { return (System.Boolean)GetValue((int)EndiciaAccountFieldIndex.CreatedByShipWorks, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.CreatedByShipWorks, value); }
		}

		/// <summary> The Description property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."Description"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Description
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.Description, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.Description, value); }
		}

		/// <summary> The FirstName property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."FirstName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String FirstName
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.FirstName, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.FirstName, value); }
		}

		/// <summary> The LastName property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."LastName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String LastName
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.LastName, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.LastName, value); }
		}

		/// <summary> The Company property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."Company"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Company
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.Company, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.Company, value); }
		}

		/// <summary> The Street1 property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."Street1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Street1
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.Street1, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.Street1, value); }
		}

		/// <summary> The Street2 property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."Street2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Street2
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.Street2, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.Street2, value); }
		}

		/// <summary> The Street3 property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."Street3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Street3
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.Street3, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.Street3, value); }
		}

		/// <summary> The City property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."City"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String City
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.City, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.City, value); }
		}

		/// <summary> The StateProvCode property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."StateProvCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String StateProvCode
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.StateProvCode, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.StateProvCode, value); }
		}

		/// <summary> The PostalCode property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."PostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String PostalCode
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.PostalCode, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.PostalCode, value); }
		}

		/// <summary> The CountryCode property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."CountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CountryCode
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.CountryCode, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.CountryCode, value); }
		}

		/// <summary> The Phone property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."Phone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Phone
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.Phone, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.Phone, value); }
		}

		/// <summary> The Fax property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."Fax"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Fax
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.Fax, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.Fax, value); }
		}

		/// <summary> The Email property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."Email"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Email
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.Email, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.Email, value); }
		}

		/// <summary> The MailingPostalCode property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."MailingPostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String MailingPostalCode
		{
			get { return (System.String)GetValue((int)EndiciaAccountFieldIndex.MailingPostalCode, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.MailingPostalCode, value); }
		}

		/// <summary> The ScanFormAddressSource property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."ScanFormAddressSource"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ScanFormAddressSource
		{
			get { return (System.Int32)GetValue((int)EndiciaAccountFieldIndex.ScanFormAddressSource, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.ScanFormAddressSource, value); }
		}

		/// <summary> The AcceptedFCMILetterWarning property of the Entity EndiciaAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EndiciaAccount"."AcceptedFCMILetterWarning"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean AcceptedFCMILetterWarning
		{
			get { return (System.Boolean)GetValue((int)EndiciaAccountFieldIndex.AcceptedFCMILetterWarning, true); }
			set	{ SetValue((int)EndiciaAccountFieldIndex.AcceptedFCMILetterWarning, value); }
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
			get { return (int)ShipWorks.Data.Model.EntityType.EndiciaAccountEntity; }
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
