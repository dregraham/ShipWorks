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
	
	/// <summary>Entity class which represents the entity 'UpsRateTable'.<br/><br/></summary>
	[Serializable]
	public partial class UpsRateTableEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END
			
	{
		#region Class Member Declarations
		private EntityCollection<UpsLocalRateEntity> _upsLocalRate;
		private EntityCollection<UpsLocalRateSurchargeEntity> _upsLocalRateSurcharge;
		private UpsAccountEntity _upsAccount;

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
			/// <summary>Member name UpsLocalRate</summary>
			public static readonly string UpsLocalRate = "UpsLocalRate";
			/// <summary>Member name UpsLocalRateSurcharge</summary>
			public static readonly string UpsLocalRateSurcharge = "UpsLocalRateSurcharge";
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
				_upsLocalRate = (EntityCollection<UpsLocalRateEntity>)info.GetValue("_upsLocalRate", typeof(EntityCollection<UpsLocalRateEntity>));
				_upsLocalRateSurcharge = (EntityCollection<UpsLocalRateSurchargeEntity>)info.GetValue("_upsLocalRateSurcharge", typeof(EntityCollection<UpsLocalRateSurchargeEntity>));
				_upsAccount = (UpsAccountEntity)info.GetValue("_upsAccount", typeof(UpsAccountEntity));
				if(_upsAccount!=null)
				{
					_upsAccount.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((UpsRateTableFieldIndex)fieldIndex)
			{
				case UpsRateTableFieldIndex.UpsAccountID:
					DesetupSyncUpsAccount(true, false);
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
				case "UpsAccount":
					this.UpsAccount = (UpsAccountEntity)entity;
					break;
				case "UpsLocalRate":
					this.UpsLocalRate.Add((UpsLocalRateEntity)entity);
					break;
				case "UpsLocalRateSurcharge":
					this.UpsLocalRateSurcharge.Add((UpsLocalRateSurchargeEntity)entity);
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
					toReturn.Add(Relations.UpsAccountEntityUsingUpsAccountID);
					break;
				case "UpsLocalRate":
					toReturn.Add(Relations.UpsLocalRateEntityUsingUpsRateTableID);
					break;
				case "UpsLocalRateSurcharge":
					toReturn.Add(Relations.UpsLocalRateSurchargeEntityUsingUpsRateTableID);
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
					SetupSyncUpsAccount(relatedEntity);
					break;
				case "UpsLocalRate":
					this.UpsLocalRate.Add((UpsLocalRateEntity)relatedEntity);
					break;
				case "UpsLocalRateSurcharge":
					this.UpsLocalRateSurcharge.Add((UpsLocalRateSurchargeEntity)relatedEntity);
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
					DesetupSyncUpsAccount(false, true);
					break;
				case "UpsLocalRate":
					this.PerformRelatedEntityRemoval(this.UpsLocalRate, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "UpsLocalRateSurcharge":
					this.PerformRelatedEntityRemoval(this.UpsLocalRateSurcharge, relatedEntity, signalRelatedEntityManyToOne);
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
			if(_upsAccount!=null)
			{
				toReturn.Add(_upsAccount);
			}
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.Add(this.UpsLocalRate);
			toReturn.Add(this.UpsLocalRateSurcharge);
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
				info.AddValue("_upsLocalRate", ((_upsLocalRate!=null) && (_upsLocalRate.Count>0) && !this.MarkedForDeletion)?_upsLocalRate:null);
				info.AddValue("_upsLocalRateSurcharge", ((_upsLocalRateSurcharge!=null) && (_upsLocalRateSurcharge.Count>0) && !this.MarkedForDeletion)?_upsLocalRateSurcharge:null);
				info.AddValue("_upsAccount", (!this.MarkedForDeletion?_upsAccount:null));
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

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'UpsLocalRate' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUpsLocalRate()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UpsLocalRateFields.UpsRateTableID, null, ComparisonOperator.Equal, this.UpsRateTableID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'UpsLocalRateSurcharge' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUpsLocalRateSurcharge()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UpsLocalRateSurchargeFields.UpsRateTableID, null, ComparisonOperator.Equal, this.UpsRateTableID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'UpsAccount' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUpsAccount()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UpsAccountFields.UpsAccountID, null, ComparisonOperator.Equal, this.UpsAccountID));
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
			collectionsQueue.Enqueue(this._upsLocalRate);
			collectionsQueue.Enqueue(this._upsLocalRateSurcharge);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._upsLocalRate = (EntityCollection<UpsLocalRateEntity>) collectionsQueue.Dequeue();
			this._upsLocalRateSurcharge = (EntityCollection<UpsLocalRateSurchargeEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._upsLocalRate != null);
			toReturn |=(this._upsLocalRateSurcharge != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<UpsLocalRateEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsLocalRateEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<UpsLocalRateSurchargeEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsLocalRateSurchargeEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("UpsAccount", _upsAccount);
			toReturn.Add("UpsLocalRate", _upsLocalRate);
			toReturn.Add("UpsLocalRateSurcharge", _upsLocalRateSurcharge);
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
			_fieldsCustomProperties.Add("UpsAccountID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("UploadDate", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _upsAccount</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncUpsAccount(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _upsAccount, new PropertyChangedEventHandler( OnUpsAccountPropertyChanged ), "UpsAccount", ShipWorks.Data.Model.RelationClasses.StaticUpsRateTableRelations.UpsAccountEntityUsingUpsAccountIDStatic, true, signalRelatedEntity, "UpsRateTable", resetFKFields, new int[] { (int)UpsRateTableFieldIndex.UpsAccountID } );
			_upsAccount = null;
		}

		/// <summary> setups the sync logic for member _upsAccount</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncUpsAccount(IEntityCore relatedEntity)
		{
			if(_upsAccount!=relatedEntity)
			{
				DesetupSyncUpsAccount(true, true);
				_upsAccount = (UpsAccountEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _upsAccount, new PropertyChangedEventHandler( OnUpsAccountPropertyChanged ), "UpsAccount", ShipWorks.Data.Model.RelationClasses.StaticUpsRateTableRelations.UpsAccountEntityUsingUpsAccountIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnUpsAccountPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

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

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UpsLocalRate' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUpsLocalRate
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<UpsLocalRateEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsLocalRateEntityFactory))), (IEntityRelation)GetRelationsForField("UpsLocalRate")[0], (int)ShipWorks.Data.Model.EntityType.UpsRateTableEntity, (int)ShipWorks.Data.Model.EntityType.UpsLocalRateEntity, 0, null, null, null, null, "UpsLocalRate", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UpsLocalRateSurcharge' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUpsLocalRateSurcharge
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<UpsLocalRateSurchargeEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsLocalRateSurchargeEntityFactory))), (IEntityRelation)GetRelationsForField("UpsLocalRateSurcharge")[0], (int)ShipWorks.Data.Model.EntityType.UpsRateTableEntity, (int)ShipWorks.Data.Model.EntityType.UpsLocalRateSurchargeEntity, 0, null, null, null, null, "UpsLocalRateSurcharge", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UpsAccount' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUpsAccount
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(UpsAccountEntityFactory))),	(IEntityRelation)GetRelationsForField("UpsAccount")[0], (int)ShipWorks.Data.Model.EntityType.UpsRateTableEntity, (int)ShipWorks.Data.Model.EntityType.UpsAccountEntity, 0, null, null, null, null, "UpsAccount", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The UpsAccountID property of the Entity UpsRateTable<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsRateTable"."UpsAccountID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 UpsAccountID
		{
			get { return (System.Int64)GetValue((int)UpsRateTableFieldIndex.UpsAccountID, true); }
			set	{ SetValue((int)UpsRateTableFieldIndex.UpsAccountID, value); }
		}

		/// <summary> The UploadDate property of the Entity UpsRateTable<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsRateTable"."UploadDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime UploadDate
		{
			get { return (System.DateTime)GetValue((int)UpsRateTableFieldIndex.UploadDate, true); }
			set	{ SetValue((int)UpsRateTableFieldIndex.UploadDate, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'UpsLocalRateEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(UpsLocalRateEntity))]
		public virtual EntityCollection<UpsLocalRateEntity> UpsLocalRate
		{
			get { return GetOrCreateEntityCollection<UpsLocalRateEntity, UpsLocalRateEntityFactory>("UpsRateTable", true, false, ref _upsLocalRate);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'UpsLocalRateSurchargeEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(UpsLocalRateSurchargeEntity))]
		public virtual EntityCollection<UpsLocalRateSurchargeEntity> UpsLocalRateSurcharge
		{
			get { return GetOrCreateEntityCollection<UpsLocalRateSurchargeEntity, UpsLocalRateSurchargeEntityFactory>("UpsRateTable", true, false, ref _upsLocalRateSurcharge);	}
		}

		/// <summary> Gets / sets related entity of type 'UpsAccountEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual UpsAccountEntity UpsAccount
		{
			get	{ return _upsAccount; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncUpsAccount(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "UpsRateTable", "UpsAccount", _upsAccount, true); 
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
