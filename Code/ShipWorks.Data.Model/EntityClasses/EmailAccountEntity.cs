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
	/// <summary>Entity class which represents the entity 'EmailAccount'.<br/><br/></summary>
	[Serializable]
	public partial class EmailAccountEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<OrderMotionStoreEntity> _orderMotionStore;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name OrderMotionStore</summary>
			public static readonly string OrderMotionStore = "OrderMotionStore";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static EmailAccountEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public EmailAccountEntity():base("EmailAccountEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public EmailAccountEntity(IEntityFields2 fields):base("EmailAccountEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this EmailAccountEntity</param>
		public EmailAccountEntity(IValidator validator):base("EmailAccountEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="emailAccountID">PK value for EmailAccount which data should be fetched into this EmailAccount object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public EmailAccountEntity(System.Int64 emailAccountID):base("EmailAccountEntity")
		{
			InitClassEmpty(null, null);
			this.EmailAccountID = emailAccountID;
		}

		/// <summary> CTor</summary>
		/// <param name="emailAccountID">PK value for EmailAccount which data should be fetched into this EmailAccount object</param>
		/// <param name="validator">The custom validator object for this EmailAccountEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public EmailAccountEntity(System.Int64 emailAccountID, IValidator validator):base("EmailAccountEntity")
		{
			InitClassEmpty(validator, null);
			this.EmailAccountID = emailAccountID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected EmailAccountEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_orderMotionStore = (EntityCollection<OrderMotionStoreEntity>)info.GetValue("_orderMotionStore", typeof(EntityCollection<OrderMotionStoreEntity>));
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
				case "OrderMotionStore":
					this.OrderMotionStore.Add((OrderMotionStoreEntity)entity);
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
				case "OrderMotionStore":
					toReturn.Add(Relations.OrderMotionStoreEntityUsingOrderMotionEmailAccountID);
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
				case "OrderMotionStore":
					this.OrderMotionStore.Add((OrderMotionStoreEntity)relatedEntity);
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
				case "OrderMotionStore":
					this.PerformRelatedEntityRemoval(this.OrderMotionStore, relatedEntity, signalRelatedEntityManyToOne);
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
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.Add(this.OrderMotionStore);
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
				info.AddValue("_orderMotionStore", ((_orderMotionStore!=null) && (_orderMotionStore.Count>0) && !this.MarkedForDeletion)?_orderMotionStore:null);
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new EmailAccountRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'OrderMotionStore' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoOrderMotionStore()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(OrderMotionStoreFields.OrderMotionEmailAccountID, null, ComparisonOperator.Equal, this.EmailAccountID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(EmailAccountEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._orderMotionStore);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._orderMotionStore = (EntityCollection<OrderMotionStoreEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._orderMotionStore != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<OrderMotionStoreEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderMotionStoreEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("OrderMotionStore", _orderMotionStore);
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
			_fieldsCustomProperties.Add("EmailAccountID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RowVersion", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AccountName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DisplayName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("EmailAddress", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("IncomingServer", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("IncomingServerType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("IncomingPort", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("IncomingSecurityType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("IncomingUsername", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("IncomingPassword", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OutgoingServer", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OutgoingPort", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OutgoingSecurityType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OutgoingCredentialSource", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OutgoingUsername", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OutgoingPassword", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AutoSend", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AutoSendMinutes", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AutoSendLastTime", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("LimitMessagesPerConnection", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("LimitMessagesPerConnectionQuantity", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("LimitMessagesPerHour", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("LimitMessagesPerHourQuantity", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("LimitMessageInterval", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("LimitMessageIntervalSeconds", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InternalOwnerID", fieldHashtable);
		}
		#endregion

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this EmailAccountEntity</param>
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
		public  static EmailAccountRelations Relations
		{
			get	{ return new EmailAccountRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'OrderMotionStore' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathOrderMotionStore
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<OrderMotionStoreEntity>(EntityFactoryCache2.GetEntityFactory(typeof(OrderMotionStoreEntityFactory))), (IEntityRelation)GetRelationsForField("OrderMotionStore")[0], (int)ShipWorks.Data.Model.EntityType.EmailAccountEntity, (int)ShipWorks.Data.Model.EntityType.OrderMotionStoreEntity, 0, null, null, null, null, "OrderMotionStore", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
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

		/// <summary> The EmailAccountID property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."EmailAccountID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 EmailAccountID
		{
			get { return (System.Int64)GetValue((int)EmailAccountFieldIndex.EmailAccountID, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.EmailAccountID, value); }
		}

		/// <summary> The RowVersion property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."RowVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] RowVersion
		{
			get { return (System.Byte[])GetValue((int)EmailAccountFieldIndex.RowVersion, true); }

		}

		/// <summary> The AccountName property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."AccountName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String AccountName
		{
			get { return (System.String)GetValue((int)EmailAccountFieldIndex.AccountName, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.AccountName, value); }
		}

		/// <summary> The DisplayName property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."DisplayName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String DisplayName
		{
			get { return (System.String)GetValue((int)EmailAccountFieldIndex.DisplayName, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.DisplayName, value); }
		}

		/// <summary> The EmailAddress property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."EmailAddress"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String EmailAddress
		{
			get { return (System.String)GetValue((int)EmailAccountFieldIndex.EmailAddress, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.EmailAddress, value); }
		}

		/// <summary> The IncomingServer property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."IncomingServer"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String IncomingServer
		{
			get { return (System.String)GetValue((int)EmailAccountFieldIndex.IncomingServer, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.IncomingServer, value); }
		}

		/// <summary> The IncomingServerType property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."IncomingServerType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 IncomingServerType
		{
			get { return (System.Int32)GetValue((int)EmailAccountFieldIndex.IncomingServerType, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.IncomingServerType, value); }
		}

		/// <summary> The IncomingPort property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."IncomingPort"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 IncomingPort
		{
			get { return (System.Int32)GetValue((int)EmailAccountFieldIndex.IncomingPort, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.IncomingPort, value); }
		}

		/// <summary> The IncomingSecurityType property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."IncomingSecurityType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 IncomingSecurityType
		{
			get { return (System.Int32)GetValue((int)EmailAccountFieldIndex.IncomingSecurityType, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.IncomingSecurityType, value); }
		}

		/// <summary> The IncomingUsername property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."IncomingUsername"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String IncomingUsername
		{
			get { return (System.String)GetValue((int)EmailAccountFieldIndex.IncomingUsername, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.IncomingUsername, value); }
		}

		/// <summary> The IncomingPassword property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."IncomingPassword"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String IncomingPassword
		{
			get { return (System.String)GetValue((int)EmailAccountFieldIndex.IncomingPassword, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.IncomingPassword, value); }
		}

		/// <summary> The OutgoingServer property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."OutgoingServer"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OutgoingServer
		{
			get { return (System.String)GetValue((int)EmailAccountFieldIndex.OutgoingServer, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.OutgoingServer, value); }
		}

		/// <summary> The OutgoingPort property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."OutgoingPort"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 OutgoingPort
		{
			get { return (System.Int32)GetValue((int)EmailAccountFieldIndex.OutgoingPort, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.OutgoingPort, value); }
		}

		/// <summary> The OutgoingSecurityType property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."OutgoingSecurityType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 OutgoingSecurityType
		{
			get { return (System.Int32)GetValue((int)EmailAccountFieldIndex.OutgoingSecurityType, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.OutgoingSecurityType, value); }
		}

		/// <summary> The OutgoingCredentialSource property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."OutgoingCredentialSource"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 OutgoingCredentialSource
		{
			get { return (System.Int32)GetValue((int)EmailAccountFieldIndex.OutgoingCredentialSource, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.OutgoingCredentialSource, value); }
		}

		/// <summary> The OutgoingUsername property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."OutgoingUsername"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OutgoingUsername
		{
			get { return (System.String)GetValue((int)EmailAccountFieldIndex.OutgoingUsername, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.OutgoingUsername, value); }
		}

		/// <summary> The OutgoingPassword property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."OutgoingPassword"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OutgoingPassword
		{
			get { return (System.String)GetValue((int)EmailAccountFieldIndex.OutgoingPassword, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.OutgoingPassword, value); }
		}

		/// <summary> The AutoSend property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."AutoSend"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean AutoSend
		{
			get { return (System.Boolean)GetValue((int)EmailAccountFieldIndex.AutoSend, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.AutoSend, value); }
		}

		/// <summary> The AutoSendMinutes property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."AutoSendMinutes"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 AutoSendMinutes
		{
			get { return (System.Int32)GetValue((int)EmailAccountFieldIndex.AutoSendMinutes, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.AutoSendMinutes, value); }
		}

		/// <summary> The AutoSendLastTime property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."AutoSendLastTime"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime AutoSendLastTime
		{
			get { return (System.DateTime)GetValue((int)EmailAccountFieldIndex.AutoSendLastTime, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.AutoSendLastTime, value); }
		}

		/// <summary> The LimitMessagesPerConnection property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."LimitMessagesPerConnection"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean LimitMessagesPerConnection
		{
			get { return (System.Boolean)GetValue((int)EmailAccountFieldIndex.LimitMessagesPerConnection, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.LimitMessagesPerConnection, value); }
		}

		/// <summary> The LimitMessagesPerConnectionQuantity property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."LimitMessagesPerConnectionQuantity"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 LimitMessagesPerConnectionQuantity
		{
			get { return (System.Int32)GetValue((int)EmailAccountFieldIndex.LimitMessagesPerConnectionQuantity, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.LimitMessagesPerConnectionQuantity, value); }
		}

		/// <summary> The LimitMessagesPerHour property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."LimitMessagesPerHour"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean LimitMessagesPerHour
		{
			get { return (System.Boolean)GetValue((int)EmailAccountFieldIndex.LimitMessagesPerHour, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.LimitMessagesPerHour, value); }
		}

		/// <summary> The LimitMessagesPerHourQuantity property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."LimitMessagesPerHourQuantity"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 LimitMessagesPerHourQuantity
		{
			get { return (System.Int32)GetValue((int)EmailAccountFieldIndex.LimitMessagesPerHourQuantity, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.LimitMessagesPerHourQuantity, value); }
		}

		/// <summary> The LimitMessageInterval property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."LimitMessageInterval"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean LimitMessageInterval
		{
			get { return (System.Boolean)GetValue((int)EmailAccountFieldIndex.LimitMessageInterval, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.LimitMessageInterval, value); }
		}

		/// <summary> The LimitMessageIntervalSeconds property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."LimitMessageIntervalSeconds"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 LimitMessageIntervalSeconds
		{
			get { return (System.Int32)GetValue((int)EmailAccountFieldIndex.LimitMessageIntervalSeconds, true); }
			set	{ SetValue((int)EmailAccountFieldIndex.LimitMessageIntervalSeconds, value); }
		}

		/// <summary> The InternalOwnerID property of the Entity EmailAccount<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailAccount"."InternalOwnerID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> InternalOwnerID
		{
			get { return (Nullable<System.Int64>)GetValue((int)EmailAccountFieldIndex.InternalOwnerID, false); }
			set	{ SetValue((int)EmailAccountFieldIndex.InternalOwnerID, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'OrderMotionStoreEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(OrderMotionStoreEntity))]
		public virtual EntityCollection<OrderMotionStoreEntity> OrderMotionStore
		{
			get { return GetOrCreateEntityCollection<OrderMotionStoreEntity, OrderMotionStoreEntityFactory>("OrderMotionEmailAccount", true, false, ref _orderMotionStore);	}
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
			get { return (int)ShipWorks.Data.Model.EntityType.EmailAccountEntity; }
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
