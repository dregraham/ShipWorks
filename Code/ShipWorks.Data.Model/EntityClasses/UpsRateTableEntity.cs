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
	/// <summary>Entity class which represents the entity 'UpsRateTable'.<br/><br/></summary>
	[Serializable]
	public partial class UpsRateTableEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<UpsAccountEntity> _upsAccount;
		private EntityCollection<UpsRateEntity> _upsRate;
		private EntityCollection<UpsRateSurchargeEntity> _upsRateSurcharge;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name UpsAccount</summary>
			public static readonly string UpsAccount = "UpsAccount";
			/// <summary>Member name UpsRate</summary>
			public static readonly string UpsRate = "UpsRate";
			/// <summary>Member name UpsRateSurcharge</summary>
			public static readonly string UpsRateSurcharge = "UpsRateSurcharge";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static UpsRateTableEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public UpsRateTableEntity():base("UpsRateTableEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public UpsRateTableEntity(IEntityFields2 fields):base("UpsRateTableEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this UpsRateTableEntity</param>
		public UpsRateTableEntity(IValidator validator):base("UpsRateTableEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="upsRateTableID">PK value for UpsRateTable which data should be fetched into this UpsRateTable object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public UpsRateTableEntity(System.Int64 upsRateTableID):base("UpsRateTableEntity")
		{
			InitClassEmpty(null, null);
			this.UpsRateTableID = upsRateTableID;
		}

		/// <summary> CTor</summary>
		/// <param name="upsRateTableID">PK value for UpsRateTable which data should be fetched into this UpsRateTable object</param>
		/// <param name="validator">The custom validator object for this UpsRateTableEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public UpsRateTableEntity(System.Int64 upsRateTableID, IValidator validator):base("UpsRateTableEntity")
		{
			InitClassEmpty(validator, null);
			this.UpsRateTableID = upsRateTableID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected UpsRateTableEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_upsAccount = (EntityCollection<UpsAccountEntity>)info.GetValue("_upsAccount", typeof(EntityCollection<UpsAccountEntity>));
				_upsRate = (EntityCollection<UpsRateEntity>)info.GetValue("_upsRate", typeof(EntityCollection<UpsRateEntity>));
				_upsRateSurcharge = (EntityCollection<UpsRateSurchargeEntity>)info.GetValue("_upsRateSurcharge", typeof(EntityCollection<UpsRateSurchargeEntity>));
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
				case "UpsAccount":
					this.UpsAccount.Add((UpsAccountEntity)entity);
					break;
				case "UpsRate":
					this.UpsRate.Add((UpsRateEntity)entity);
					break;
				case "UpsRateSurcharge":
					this.UpsRateSurcharge.Add((UpsRateSurchargeEntity)entity);
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
				case "UpsAccount":
					toReturn.Add(Relations.UpsAccountEntityUsingUpsRateTableID);
					break;
				case "UpsRate":
					toReturn.Add(Relations.UpsRateEntityUsingUpsRateTableID);
					break;
				case "UpsRateSurcharge":
					toReturn.Add(Relations.UpsRateSurchargeEntityUsingUpsRateTableID);
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
				case "UpsAccount":
					this.UpsAccount.Add((UpsAccountEntity)relatedEntity);
					break;
				case "UpsRate":
					this.UpsRate.Add((UpsRateEntity)relatedEntity);
					break;
				case "UpsRateSurcharge":
					this.UpsRateSurcharge.Add((UpsRateSurchargeEntity)relatedEntity);
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
				case "UpsAccount":
					this.PerformRelatedEntityRemoval(this.UpsAccount, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "UpsRate":
					this.PerformRelatedEntityRemoval(this.UpsRate, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "UpsRateSurcharge":
					this.PerformRelatedEntityRemoval(this.UpsRateSurcharge, relatedEntity, signalRelatedEntityManyToOne);
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
			toReturn.Add(this.UpsAccount);
			toReturn.Add(this.UpsRate);
			toReturn.Add(this.UpsRateSurcharge);
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
				info.AddValue("_upsAccount", ((_upsAccount!=null) && (_upsAccount.Count>0) && !this.MarkedForDeletion)?_upsAccount:null);
				info.AddValue("_upsRate", ((_upsRate!=null) && (_upsRate.Count>0) && !this.MarkedForDeletion)?_upsRate:null);
				info.AddValue("_upsRateSurcharge", ((_upsRateSurcharge!=null) && (_upsRateSurcharge.Count>0) && !this.MarkedForDeletion)?_upsRateSurcharge:null);
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new UpsRateTableRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'UpsAccount' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUpsAccount()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UpsAccountFields.UpsRateTableID, null, ComparisonOperator.Equal, this.UpsRateTableID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'UpsRate' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUpsRate()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UpsRateFields.UpsRateTableID, null, ComparisonOperator.Equal, this.UpsRateTableID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'UpsRateSurcharge' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUpsRateSurcharge()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UpsRateSurchargeFields.UpsRateTableID, null, ComparisonOperator.Equal, this.UpsRateTableID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(UpsRateTableEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._upsAccount);
			collectionsQueue.Enqueue(this._upsRate);
			collectionsQueue.Enqueue(this._upsRateSurcharge);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._upsAccount = (EntityCollection<UpsAccountEntity>) collectionsQueue.Dequeue();
			this._upsRate = (EntityCollection<UpsRateEntity>) collectionsQueue.Dequeue();
			this._upsRateSurcharge = (EntityCollection<UpsRateSurchargeEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._upsAccount != null);
			toReturn |=(this._upsRate != null);
			toReturn |=(this._upsRateSurcharge != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<UpsAccountEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsAccountEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<UpsRateEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsRateEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<UpsRateSurchargeEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsRateSurchargeEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("UpsAccount", _upsAccount);
			toReturn.Add("UpsRate", _upsRate);
			toReturn.Add("UpsRateSurcharge", _upsRateSurcharge);
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
			_fieldsCustomProperties.Add("UpsRateTableID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("UploadDate", fieldHashtable);
		}
		#endregion

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this UpsRateTableEntity</param>
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
		public  static UpsRateTableRelations Relations
		{
			get	{ return new UpsRateTableRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UpsAccount' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUpsAccount
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<UpsAccountEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsAccountEntityFactory))), (IEntityRelation)GetRelationsForField("UpsAccount")[0], (int)ShipWorks.Data.Model.EntityType.UpsRateTableEntity, (int)ShipWorks.Data.Model.EntityType.UpsAccountEntity, 0, null, null, null, null, "UpsAccount", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UpsRate' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUpsRate
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<UpsRateEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsRateEntityFactory))), (IEntityRelation)GetRelationsForField("UpsRate")[0], (int)ShipWorks.Data.Model.EntityType.UpsRateTableEntity, (int)ShipWorks.Data.Model.EntityType.UpsRateEntity, 0, null, null, null, null, "UpsRate", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UpsRateSurcharge' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUpsRateSurcharge
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<UpsRateSurchargeEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsRateSurchargeEntityFactory))), (IEntityRelation)GetRelationsForField("UpsRateSurcharge")[0], (int)ShipWorks.Data.Model.EntityType.UpsRateTableEntity, (int)ShipWorks.Data.Model.EntityType.UpsRateSurchargeEntity, 0, null, null, null, null, "UpsRateSurcharge", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
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

		/// <summary> The UpsRateTableID property of the Entity UpsRateTable<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsRateTable"."UpsRateTableID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
		public virtual System.Int64 UpsRateTableID
		{
			get { return (System.Int64)GetValue((int)UpsRateTableFieldIndex.UpsRateTableID, true); }
			set	{ SetValue((int)UpsRateTableFieldIndex.UpsRateTableID, value); }
		}

		/// <summary> The UploadDate property of the Entity UpsRateTable<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsRateTable"."UploadDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime2, 7, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime UploadDate
		{
			get { return (System.DateTime)GetValue((int)UpsRateTableFieldIndex.UploadDate, true); }
			set	{ SetValue((int)UpsRateTableFieldIndex.UploadDate, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'UpsAccountEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(UpsAccountEntity))]
		public virtual EntityCollection<UpsAccountEntity> UpsAccount
		{
			get { return GetOrCreateEntityCollection<UpsAccountEntity, UpsAccountEntityFactory>("UpsRateTable", true, false, ref _upsAccount);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'UpsRateEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(UpsRateEntity))]
		public virtual EntityCollection<UpsRateEntity> UpsRate
		{
			get { return GetOrCreateEntityCollection<UpsRateEntity, UpsRateEntityFactory>("UpsRateTable", true, false, ref _upsRate);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'UpsRateSurchargeEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(UpsRateSurchargeEntity))]
		public virtual EntityCollection<UpsRateSurchargeEntity> UpsRateSurcharge
		{
			get { return GetOrCreateEntityCollection<UpsRateSurchargeEntity, UpsRateSurchargeEntityFactory>("UpsRateTable", true, false, ref _upsRateSurcharge);	}
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
			get { return (int)ShipWorks.Data.Model.EntityType.UpsRateTableEntity; }
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
