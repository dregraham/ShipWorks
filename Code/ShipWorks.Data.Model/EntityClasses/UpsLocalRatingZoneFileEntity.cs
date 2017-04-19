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
	
	/// <summary>Entity class which represents the entity 'UpsLocalRatingZoneFile'.<br/><br/></summary>
	[Serializable]
	public partial class UpsLocalRatingZoneFileEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END
			
	{
		#region Class Member Declarations
		private EntityCollection<UpsLocalRatingDeliveryAreaSurchargeEntity> _upsLocalRatingDeliveryAreaSurcharge;
		private EntityCollection<UpsLocalRatingZoneEntity> _upsLocalRatingZone;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name UpsLocalRatingDeliveryAreaSurcharge</summary>
			public static readonly string UpsLocalRatingDeliveryAreaSurcharge = "UpsLocalRatingDeliveryAreaSurcharge";
			/// <summary>Member name UpsLocalRatingZone</summary>
			public static readonly string UpsLocalRatingZone = "UpsLocalRatingZone";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static UpsLocalRatingZoneFileEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public UpsLocalRatingZoneFileEntity():base("UpsLocalRatingZoneFileEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public UpsLocalRatingZoneFileEntity(IEntityFields2 fields):base("UpsLocalRatingZoneFileEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this UpsLocalRatingZoneFileEntity</param>
		public UpsLocalRatingZoneFileEntity(IValidator validator):base("UpsLocalRatingZoneFileEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="zoneFileID">PK value for UpsLocalRatingZoneFile which data should be fetched into this UpsLocalRatingZoneFile object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public UpsLocalRatingZoneFileEntity(System.Int64 zoneFileID):base("UpsLocalRatingZoneFileEntity")
		{
			InitClassEmpty(null, null);
			this.ZoneFileID = zoneFileID;
		}

		/// <summary> CTor</summary>
		/// <param name="zoneFileID">PK value for UpsLocalRatingZoneFile which data should be fetched into this UpsLocalRatingZoneFile object</param>
		/// <param name="validator">The custom validator object for this UpsLocalRatingZoneFileEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public UpsLocalRatingZoneFileEntity(System.Int64 zoneFileID, IValidator validator):base("UpsLocalRatingZoneFileEntity")
		{
			InitClassEmpty(validator, null);
			this.ZoneFileID = zoneFileID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected UpsLocalRatingZoneFileEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_upsLocalRatingDeliveryAreaSurcharge = (EntityCollection<UpsLocalRatingDeliveryAreaSurchargeEntity>)info.GetValue("_upsLocalRatingDeliveryAreaSurcharge", typeof(EntityCollection<UpsLocalRatingDeliveryAreaSurchargeEntity>));
				_upsLocalRatingZone = (EntityCollection<UpsLocalRatingZoneEntity>)info.GetValue("_upsLocalRatingZone", typeof(EntityCollection<UpsLocalRatingZoneEntity>));
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
				case "UpsLocalRatingDeliveryAreaSurcharge":
					this.UpsLocalRatingDeliveryAreaSurcharge.Add((UpsLocalRatingDeliveryAreaSurchargeEntity)entity);
					break;
				case "UpsLocalRatingZone":
					this.UpsLocalRatingZone.Add((UpsLocalRatingZoneEntity)entity);
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
				case "UpsLocalRatingDeliveryAreaSurcharge":
					toReturn.Add(Relations.UpsLocalRatingDeliveryAreaSurchargeEntityUsingZoneFileID);
					break;
				case "UpsLocalRatingZone":
					toReturn.Add(Relations.UpsLocalRatingZoneEntityUsingZoneFileID);
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
				case "UpsLocalRatingDeliveryAreaSurcharge":
					this.UpsLocalRatingDeliveryAreaSurcharge.Add((UpsLocalRatingDeliveryAreaSurchargeEntity)relatedEntity);
					break;
				case "UpsLocalRatingZone":
					this.UpsLocalRatingZone.Add((UpsLocalRatingZoneEntity)relatedEntity);
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
				case "UpsLocalRatingDeliveryAreaSurcharge":
					this.PerformRelatedEntityRemoval(this.UpsLocalRatingDeliveryAreaSurcharge, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "UpsLocalRatingZone":
					this.PerformRelatedEntityRemoval(this.UpsLocalRatingZone, relatedEntity, signalRelatedEntityManyToOne);
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
			toReturn.Add(this.UpsLocalRatingDeliveryAreaSurcharge);
			toReturn.Add(this.UpsLocalRatingZone);
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
				info.AddValue("_upsLocalRatingDeliveryAreaSurcharge", ((_upsLocalRatingDeliveryAreaSurcharge!=null) && (_upsLocalRatingDeliveryAreaSurcharge.Count>0) && !this.MarkedForDeletion)?_upsLocalRatingDeliveryAreaSurcharge:null);
				info.AddValue("_upsLocalRatingZone", ((_upsLocalRatingZone!=null) && (_upsLocalRatingZone.Count>0) && !this.MarkedForDeletion)?_upsLocalRatingZone:null);
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new UpsLocalRatingZoneFileRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'UpsLocalRatingDeliveryAreaSurcharge' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUpsLocalRatingDeliveryAreaSurcharge()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UpsLocalRatingDeliveryAreaSurchargeFields.ZoneFileID, null, ComparisonOperator.Equal, this.ZoneFileID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'UpsLocalRatingZone' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUpsLocalRatingZone()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UpsLocalRatingZoneFields.ZoneFileID, null, ComparisonOperator.Equal, this.ZoneFileID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(UpsLocalRatingZoneFileEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._upsLocalRatingDeliveryAreaSurcharge);
			collectionsQueue.Enqueue(this._upsLocalRatingZone);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._upsLocalRatingDeliveryAreaSurcharge = (EntityCollection<UpsLocalRatingDeliveryAreaSurchargeEntity>) collectionsQueue.Dequeue();
			this._upsLocalRatingZone = (EntityCollection<UpsLocalRatingZoneEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._upsLocalRatingDeliveryAreaSurcharge != null);
			toReturn |=(this._upsLocalRatingZone != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<UpsLocalRatingDeliveryAreaSurchargeEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsLocalRatingDeliveryAreaSurchargeEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<UpsLocalRatingZoneEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsLocalRatingZoneEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("UpsLocalRatingDeliveryAreaSurcharge", _upsLocalRatingDeliveryAreaSurcharge);
			toReturn.Add("UpsLocalRatingZone", _upsLocalRatingZone);
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
			_fieldsCustomProperties.Add("ZoneFileID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("UploadDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FileContent", fieldHashtable);
		}
		#endregion

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this UpsLocalRatingZoneFileEntity</param>
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
		public  static UpsLocalRatingZoneFileRelations Relations
		{
			get	{ return new UpsLocalRatingZoneFileRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UpsLocalRatingDeliveryAreaSurcharge' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUpsLocalRatingDeliveryAreaSurcharge
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<UpsLocalRatingDeliveryAreaSurchargeEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsLocalRatingDeliveryAreaSurchargeEntityFactory))), (IEntityRelation)GetRelationsForField("UpsLocalRatingDeliveryAreaSurcharge")[0], (int)ShipWorks.Data.Model.EntityType.UpsLocalRatingZoneFileEntity, (int)ShipWorks.Data.Model.EntityType.UpsLocalRatingDeliveryAreaSurchargeEntity, 0, null, null, null, null, "UpsLocalRatingDeliveryAreaSurcharge", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UpsLocalRatingZone' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUpsLocalRatingZone
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<UpsLocalRatingZoneEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsLocalRatingZoneEntityFactory))), (IEntityRelation)GetRelationsForField("UpsLocalRatingZone")[0], (int)ShipWorks.Data.Model.EntityType.UpsLocalRatingZoneFileEntity, (int)ShipWorks.Data.Model.EntityType.UpsLocalRatingZoneEntity, 0, null, null, null, null, "UpsLocalRatingZone", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
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

		/// <summary> The ZoneFileID property of the Entity UpsLocalRatingZoneFile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsLocalRatingZoneFile"."ZoneFileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 ZoneFileID
		{
			get { return (System.Int64)GetValue((int)UpsLocalRatingZoneFileFieldIndex.ZoneFileID, true); }
			set	{ SetValue((int)UpsLocalRatingZoneFileFieldIndex.ZoneFileID, value); }
		}

		/// <summary> The UploadDate property of the Entity UpsLocalRatingZoneFile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsLocalRatingZoneFile"."UploadDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime2, 7, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime UploadDate
		{
			get { return (System.DateTime)GetValue((int)UpsLocalRatingZoneFileFieldIndex.UploadDate, true); }
			set	{ SetValue((int)UpsLocalRatingZoneFileFieldIndex.UploadDate, value); }
		}

		/// <summary> The FileContent property of the Entity UpsLocalRatingZoneFile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsLocalRatingZoneFile"."FileContent"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] FileContent
		{
			get { return (System.Byte[])GetValue((int)UpsLocalRatingZoneFileFieldIndex.FileContent, true); }
			set	{ SetValue((int)UpsLocalRatingZoneFileFieldIndex.FileContent, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'UpsLocalRatingDeliveryAreaSurchargeEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(UpsLocalRatingDeliveryAreaSurchargeEntity))]
		public virtual EntityCollection<UpsLocalRatingDeliveryAreaSurchargeEntity> UpsLocalRatingDeliveryAreaSurcharge
		{
			get { return GetOrCreateEntityCollection<UpsLocalRatingDeliveryAreaSurchargeEntity, UpsLocalRatingDeliveryAreaSurchargeEntityFactory>("UpsLocalRatingZoneFile", true, false, ref _upsLocalRatingDeliveryAreaSurcharge);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'UpsLocalRatingZoneEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(UpsLocalRatingZoneEntity))]
		public virtual EntityCollection<UpsLocalRatingZoneEntity> UpsLocalRatingZone
		{
			get { return GetOrCreateEntityCollection<UpsLocalRatingZoneEntity, UpsLocalRatingZoneEntityFactory>("UpsLocalRatingZoneFile", true, false, ref _upsLocalRatingZone);	}
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
			get { return (int)ShipWorks.Data.Model.EntityType.UpsLocalRatingZoneFileEntity; }
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
