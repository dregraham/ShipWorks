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
	/// <summary>Entity class which represents the entity 'EmailOutbound'.<br/><br/></summary>
	[Serializable]
	public partial class EmailOutboundEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<EmailOutboundRelationEntity> _relatedObjects;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name RelatedObjects</summary>
			public static readonly string RelatedObjects = "RelatedObjects";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static EmailOutboundEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public EmailOutboundEntity():base("EmailOutboundEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public EmailOutboundEntity(IEntityFields2 fields):base("EmailOutboundEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this EmailOutboundEntity</param>
		public EmailOutboundEntity(IValidator validator):base("EmailOutboundEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="emailOutboundID">PK value for EmailOutbound which data should be fetched into this EmailOutbound object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public EmailOutboundEntity(System.Int64 emailOutboundID):base("EmailOutboundEntity")
		{
			InitClassEmpty(null, null);
			this.EmailOutboundID = emailOutboundID;
		}

		/// <summary> CTor</summary>
		/// <param name="emailOutboundID">PK value for EmailOutbound which data should be fetched into this EmailOutbound object</param>
		/// <param name="validator">The custom validator object for this EmailOutboundEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public EmailOutboundEntity(System.Int64 emailOutboundID, IValidator validator):base("EmailOutboundEntity")
		{
			InitClassEmpty(validator, null);
			this.EmailOutboundID = emailOutboundID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected EmailOutboundEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_relatedObjects = (EntityCollection<EmailOutboundRelationEntity>)info.GetValue("_relatedObjects", typeof(EntityCollection<EmailOutboundRelationEntity>));
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
				case "RelatedObjects":
					this.RelatedObjects.Add((EmailOutboundRelationEntity)entity);
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
				case "RelatedObjects":
					toReturn.Add(Relations.EmailOutboundRelationEntityUsingEmailOutboundID);
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
				case "RelatedObjects":
					this.RelatedObjects.Add((EmailOutboundRelationEntity)relatedEntity);
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
				case "RelatedObjects":
					this.PerformRelatedEntityRemoval(this.RelatedObjects, relatedEntity, signalRelatedEntityManyToOne);
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
			toReturn.Add(this.RelatedObjects);
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
				info.AddValue("_relatedObjects", ((_relatedObjects!=null) && (_relatedObjects.Count>0) && !this.MarkedForDeletion)?_relatedObjects:null);
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new EmailOutboundRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'EmailOutboundRelation' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoRelatedObjects()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(EmailOutboundRelationFields.EmailOutboundID, null, ComparisonOperator.Equal, this.EmailOutboundID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(EmailOutboundEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._relatedObjects);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._relatedObjects = (EntityCollection<EmailOutboundRelationEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._relatedObjects != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<EmailOutboundRelationEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EmailOutboundRelationEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("RelatedObjects", _relatedObjects);
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
			_fieldsCustomProperties.Add("EmailOutboundID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RowVersion", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ContextID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ContextType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("TemplateID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AccountID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Visibility", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FromAddress", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ToList", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CcList", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BccList", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Subject", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("HtmlPartResourceID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PlainPartResourceID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Encoding", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ComposedDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SentDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DontSendBefore", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SendStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SendAttemptCount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SendAttemptLastError", fieldHashtable);
		}
		#endregion

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this EmailOutboundEntity</param>
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
		public  static EmailOutboundRelations Relations
		{
			get	{ return new EmailOutboundRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'EmailOutboundRelation' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathRelatedObjects
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<EmailOutboundRelationEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EmailOutboundRelationEntityFactory))), (IEntityRelation)GetRelationsForField("RelatedObjects")[0], (int)ShipWorks.Data.Model.EntityType.EmailOutboundEntity, (int)ShipWorks.Data.Model.EntityType.EmailOutboundRelationEntity, 0, null, null, null, null, "RelatedObjects", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
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

		/// <summary> The EmailOutboundID property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."EmailOutboundID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 EmailOutboundID
		{
			get { return (System.Int64)GetValue((int)EmailOutboundFieldIndex.EmailOutboundID, true); }
			set	{ SetValue((int)EmailOutboundFieldIndex.EmailOutboundID, value); }
		}

		/// <summary> The RowVersion property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."RowVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] RowVersion
		{
			get { return (System.Byte[])GetValue((int)EmailOutboundFieldIndex.RowVersion, true); }

		}

		/// <summary> The ContextID property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."ContextID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> ContextID
		{
			get { return (Nullable<System.Int64>)GetValue((int)EmailOutboundFieldIndex.ContextID, false); }
			set	{ SetValue((int)EmailOutboundFieldIndex.ContextID, value); }
		}

		/// <summary> The ContextType property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."ContextType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> ContextType
		{
			get { return (Nullable<System.Int32>)GetValue((int)EmailOutboundFieldIndex.ContextType, false); }
			set	{ SetValue((int)EmailOutboundFieldIndex.ContextType, value); }
		}

		/// <summary> The TemplateID property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."TemplateID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> TemplateID
		{
			get { return (Nullable<System.Int64>)GetValue((int)EmailOutboundFieldIndex.TemplateID, false); }
			set	{ SetValue((int)EmailOutboundFieldIndex.TemplateID, value); }
		}

		/// <summary> The AccountID property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."AccountID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 AccountID
		{
			get { return (System.Int64)GetValue((int)EmailOutboundFieldIndex.AccountID, true); }
			set	{ SetValue((int)EmailOutboundFieldIndex.AccountID, value); }
		}

		/// <summary> The Visibility property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."Visibility"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 Visibility
		{
			get { return (System.Int32)GetValue((int)EmailOutboundFieldIndex.Visibility, true); }
			set	{ SetValue((int)EmailOutboundFieldIndex.Visibility, value); }
		}

		/// <summary> The FromAddress property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."FromAddress"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 200<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String FromAddress
		{
			get { return (System.String)GetValue((int)EmailOutboundFieldIndex.FromAddress, true); }
			set	{ SetValue((int)EmailOutboundFieldIndex.FromAddress, value); }
		}

		/// <summary> The ToList property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."ToList"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ToList
		{
			get { return (System.String)GetValue((int)EmailOutboundFieldIndex.ToList, true); }
			set	{ SetValue((int)EmailOutboundFieldIndex.ToList, value); }
		}

		/// <summary> The CcList property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."CcList"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CcList
		{
			get { return (System.String)GetValue((int)EmailOutboundFieldIndex.CcList, true); }
			set	{ SetValue((int)EmailOutboundFieldIndex.CcList, value); }
		}

		/// <summary> The BccList property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."BccList"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BccList
		{
			get { return (System.String)GetValue((int)EmailOutboundFieldIndex.BccList, true); }
			set	{ SetValue((int)EmailOutboundFieldIndex.BccList, value); }
		}

		/// <summary> The Subject property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."Subject"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Subject
		{
			get { return (System.String)GetValue((int)EmailOutboundFieldIndex.Subject, true); }
			set	{ SetValue((int)EmailOutboundFieldIndex.Subject, value); }
		}

		/// <summary> The HtmlPartResourceID property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."HtmlPartResourceID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> HtmlPartResourceID
		{
			get { return (Nullable<System.Int64>)GetValue((int)EmailOutboundFieldIndex.HtmlPartResourceID, false); }
			set	{ SetValue((int)EmailOutboundFieldIndex.HtmlPartResourceID, value); }
		}

		/// <summary> The PlainPartResourceID property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."PlainPartResourceID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 PlainPartResourceID
		{
			get { return (System.Int64)GetValue((int)EmailOutboundFieldIndex.PlainPartResourceID, true); }
			set	{ SetValue((int)EmailOutboundFieldIndex.PlainPartResourceID, value); }
		}

		/// <summary> The Encoding property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."Encoding"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Encoding
		{
			get { return (System.String)GetValue((int)EmailOutboundFieldIndex.Encoding, true); }
			set	{ SetValue((int)EmailOutboundFieldIndex.Encoding, value); }
		}

		/// <summary> The ComposedDate property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."ComposedDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime ComposedDate
		{
			get { return (System.DateTime)GetValue((int)EmailOutboundFieldIndex.ComposedDate, true); }
			set	{ SetValue((int)EmailOutboundFieldIndex.ComposedDate, value); }
		}

		/// <summary> The SentDate property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."SentDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime SentDate
		{
			get { return (System.DateTime)GetValue((int)EmailOutboundFieldIndex.SentDate, true); }
			set	{ SetValue((int)EmailOutboundFieldIndex.SentDate, value); }
		}

		/// <summary> The DontSendBefore property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."DontSendBefore"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.DateTime> DontSendBefore
		{
			get { return (Nullable<System.DateTime>)GetValue((int)EmailOutboundFieldIndex.DontSendBefore, false); }
			set	{ SetValue((int)EmailOutboundFieldIndex.DontSendBefore, value); }
		}

		/// <summary> The SendStatus property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."SendStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 SendStatus
		{
			get { return (System.Int32)GetValue((int)EmailOutboundFieldIndex.SendStatus, true); }
			set	{ SetValue((int)EmailOutboundFieldIndex.SendStatus, value); }
		}

		/// <summary> The SendAttemptCount property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."SendAttemptCount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 SendAttemptCount
		{
			get { return (System.Int32)GetValue((int)EmailOutboundFieldIndex.SendAttemptCount, true); }
			set	{ SetValue((int)EmailOutboundFieldIndex.SendAttemptCount, value); }
		}

		/// <summary> The SendAttemptLastError property of the Entity EmailOutbound<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EmailOutbound"."SendAttemptLastError"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String SendAttemptLastError
		{
			get { return (System.String)GetValue((int)EmailOutboundFieldIndex.SendAttemptLastError, true); }
			set	{ SetValue((int)EmailOutboundFieldIndex.SendAttemptLastError, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'EmailOutboundRelationEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(EmailOutboundRelationEntity))]
		public virtual EntityCollection<EmailOutboundRelationEntity> RelatedObjects
		{
			get { return GetOrCreateEntityCollection<EmailOutboundRelationEntity, EmailOutboundRelationEntityFactory>("EmailOutbound", true, false, ref _relatedObjects);	}
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
			get { return (int)ShipWorks.Data.Model.EntityType.EmailOutboundEntity; }
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
