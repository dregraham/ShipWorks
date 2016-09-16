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
	/// <summary>Entity class which represents the entity 'ScanFormBatch'.<br/><br/></summary>
	[Serializable]
	public partial class ScanFormBatchEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<EndiciaScanFormEntity> _endiciaScanForms;
		private EntityCollection<EndiciaShipmentEntity> _endiciaShipment;
		private EntityCollection<UspsScanFormEntity> _uspsScanForms;
		private EntityCollection<UspsShipmentEntity> _uspsShipment;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name EndiciaScanForms</summary>
			public static readonly string EndiciaScanForms = "EndiciaScanForms";
			/// <summary>Member name EndiciaShipment</summary>
			public static readonly string EndiciaShipment = "EndiciaShipment";
			/// <summary>Member name UspsScanForms</summary>
			public static readonly string UspsScanForms = "UspsScanForms";
			/// <summary>Member name UspsShipment</summary>
			public static readonly string UspsShipment = "UspsShipment";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static ScanFormBatchEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public ScanFormBatchEntity():base("ScanFormBatchEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public ScanFormBatchEntity(IEntityFields2 fields):base("ScanFormBatchEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this ScanFormBatchEntity</param>
		public ScanFormBatchEntity(IValidator validator):base("ScanFormBatchEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="scanFormBatchID">PK value for ScanFormBatch which data should be fetched into this ScanFormBatch object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ScanFormBatchEntity(System.Int64 scanFormBatchID):base("ScanFormBatchEntity")
		{
			InitClassEmpty(null, null);
			this.ScanFormBatchID = scanFormBatchID;
		}

		/// <summary> CTor</summary>
		/// <param name="scanFormBatchID">PK value for ScanFormBatch which data should be fetched into this ScanFormBatch object</param>
		/// <param name="validator">The custom validator object for this ScanFormBatchEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ScanFormBatchEntity(System.Int64 scanFormBatchID, IValidator validator):base("ScanFormBatchEntity")
		{
			InitClassEmpty(validator, null);
			this.ScanFormBatchID = scanFormBatchID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected ScanFormBatchEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_endiciaScanForms = (EntityCollection<EndiciaScanFormEntity>)info.GetValue("_endiciaScanForms", typeof(EntityCollection<EndiciaScanFormEntity>));
				_endiciaShipment = (EntityCollection<EndiciaShipmentEntity>)info.GetValue("_endiciaShipment", typeof(EntityCollection<EndiciaShipmentEntity>));
				_uspsScanForms = (EntityCollection<UspsScanFormEntity>)info.GetValue("_uspsScanForms", typeof(EntityCollection<UspsScanFormEntity>));
				_uspsShipment = (EntityCollection<UspsShipmentEntity>)info.GetValue("_uspsShipment", typeof(EntityCollection<UspsShipmentEntity>));
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
				case "EndiciaScanForms":
					this.EndiciaScanForms.Add((EndiciaScanFormEntity)entity);
					break;
				case "EndiciaShipment":
					this.EndiciaShipment.Add((EndiciaShipmentEntity)entity);
					break;
				case "UspsScanForms":
					this.UspsScanForms.Add((UspsScanFormEntity)entity);
					break;
				case "UspsShipment":
					this.UspsShipment.Add((UspsShipmentEntity)entity);
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
				case "EndiciaScanForms":
					toReturn.Add(Relations.EndiciaScanFormEntityUsingScanFormBatchID);
					break;
				case "EndiciaShipment":
					toReturn.Add(Relations.EndiciaShipmentEntityUsingScanFormBatchID);
					break;
				case "UspsScanForms":
					toReturn.Add(Relations.UspsScanFormEntityUsingScanFormBatchID);
					break;
				case "UspsShipment":
					toReturn.Add(Relations.UspsShipmentEntityUsingScanFormBatchID);
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
				case "EndiciaScanForms":
					this.EndiciaScanForms.Add((EndiciaScanFormEntity)relatedEntity);
					break;
				case "EndiciaShipment":
					this.EndiciaShipment.Add((EndiciaShipmentEntity)relatedEntity);
					break;
				case "UspsScanForms":
					this.UspsScanForms.Add((UspsScanFormEntity)relatedEntity);
					break;
				case "UspsShipment":
					this.UspsShipment.Add((UspsShipmentEntity)relatedEntity);
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
				case "EndiciaScanForms":
					this.PerformRelatedEntityRemoval(this.EndiciaScanForms, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "EndiciaShipment":
					this.PerformRelatedEntityRemoval(this.EndiciaShipment, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "UspsScanForms":
					this.PerformRelatedEntityRemoval(this.UspsScanForms, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "UspsShipment":
					this.PerformRelatedEntityRemoval(this.UspsShipment, relatedEntity, signalRelatedEntityManyToOne);
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
			toReturn.Add(this.EndiciaScanForms);
			toReturn.Add(this.EndiciaShipment);
			toReturn.Add(this.UspsScanForms);
			toReturn.Add(this.UspsShipment);
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
				info.AddValue("_endiciaScanForms", ((_endiciaScanForms!=null) && (_endiciaScanForms.Count>0) && !this.MarkedForDeletion)?_endiciaScanForms:null);
				info.AddValue("_endiciaShipment", ((_endiciaShipment!=null) && (_endiciaShipment.Count>0) && !this.MarkedForDeletion)?_endiciaShipment:null);
				info.AddValue("_uspsScanForms", ((_uspsScanForms!=null) && (_uspsScanForms.Count>0) && !this.MarkedForDeletion)?_uspsScanForms:null);
				info.AddValue("_uspsShipment", ((_uspsShipment!=null) && (_uspsShipment.Count>0) && !this.MarkedForDeletion)?_uspsShipment:null);
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new ScanFormBatchRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'EndiciaScanForm' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoEndiciaScanForms()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(EndiciaScanFormFields.ScanFormBatchID, null, ComparisonOperator.Equal, this.ScanFormBatchID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'EndiciaShipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoEndiciaShipment()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(EndiciaShipmentFields.ScanFormBatchID, null, ComparisonOperator.Equal, this.ScanFormBatchID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'UspsScanForm' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUspsScanForms()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UspsScanFormFields.ScanFormBatchID, null, ComparisonOperator.Equal, this.ScanFormBatchID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'UspsShipment' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUspsShipment()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UspsShipmentFields.ScanFormBatchID, null, ComparisonOperator.Equal, this.ScanFormBatchID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(ScanFormBatchEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._endiciaScanForms);
			collectionsQueue.Enqueue(this._endiciaShipment);
			collectionsQueue.Enqueue(this._uspsScanForms);
			collectionsQueue.Enqueue(this._uspsShipment);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._endiciaScanForms = (EntityCollection<EndiciaScanFormEntity>) collectionsQueue.Dequeue();
			this._endiciaShipment = (EntityCollection<EndiciaShipmentEntity>) collectionsQueue.Dequeue();
			this._uspsScanForms = (EntityCollection<UspsScanFormEntity>) collectionsQueue.Dequeue();
			this._uspsShipment = (EntityCollection<UspsShipmentEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._endiciaScanForms != null);
			toReturn |=(this._endiciaShipment != null);
			toReturn |=(this._uspsScanForms != null);
			toReturn |=(this._uspsShipment != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<EndiciaScanFormEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EndiciaScanFormEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<EndiciaShipmentEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EndiciaShipmentEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<UspsScanFormEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UspsScanFormEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<UspsShipmentEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UspsShipmentEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("EndiciaScanForms", _endiciaScanForms);
			toReturn.Add("EndiciaShipment", _endiciaShipment);
			toReturn.Add("UspsScanForms", _uspsScanForms);
			toReturn.Add("UspsShipment", _uspsShipment);
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
			_fieldsCustomProperties.Add("ScanFormBatchID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipmentType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CreatedDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipmentCount", fieldHashtable);
		}
		#endregion

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this ScanFormBatchEntity</param>
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
		public  static ScanFormBatchRelations Relations
		{
			get	{ return new ScanFormBatchRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'EndiciaScanForm' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathEndiciaScanForms
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<EndiciaScanFormEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EndiciaScanFormEntityFactory))), (IEntityRelation)GetRelationsForField("EndiciaScanForms")[0], (int)ShipWorks.Data.Model.EntityType.ScanFormBatchEntity, (int)ShipWorks.Data.Model.EntityType.EndiciaScanFormEntity, 0, null, null, null, null, "EndiciaScanForms", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'EndiciaShipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathEndiciaShipment
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<EndiciaShipmentEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EndiciaShipmentEntityFactory))), (IEntityRelation)GetRelationsForField("EndiciaShipment")[0], (int)ShipWorks.Data.Model.EntityType.ScanFormBatchEntity, (int)ShipWorks.Data.Model.EntityType.EndiciaShipmentEntity, 0, null, null, null, null, "EndiciaShipment", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UspsScanForm' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUspsScanForms
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<UspsScanFormEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UspsScanFormEntityFactory))), (IEntityRelation)GetRelationsForField("UspsScanForms")[0], (int)ShipWorks.Data.Model.EntityType.ScanFormBatchEntity, (int)ShipWorks.Data.Model.EntityType.UspsScanFormEntity, 0, null, null, null, null, "UspsScanForms", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UspsShipment' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUspsShipment
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<UspsShipmentEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UspsShipmentEntityFactory))), (IEntityRelation)GetRelationsForField("UspsShipment")[0], (int)ShipWorks.Data.Model.EntityType.ScanFormBatchEntity, (int)ShipWorks.Data.Model.EntityType.UspsShipmentEntity, 0, null, null, null, null, "UspsShipment", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
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

		/// <summary> The ScanFormBatchID property of the Entity ScanFormBatch<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ScanFormBatch"."ScanFormBatchID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 ScanFormBatchID
		{
			get { return (System.Int64)GetValue((int)ScanFormBatchFieldIndex.ScanFormBatchID, true); }
			set	{ SetValue((int)ScanFormBatchFieldIndex.ScanFormBatchID, value); }
		}

		/// <summary> The ShipmentType property of the Entity ScanFormBatch<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ScanFormBatch"."ShipmentType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipmentType
		{
			get { return (System.Int32)GetValue((int)ScanFormBatchFieldIndex.ShipmentType, true); }
			set	{ SetValue((int)ScanFormBatchFieldIndex.ShipmentType, value); }
		}

		/// <summary> The CreatedDate property of the Entity ScanFormBatch<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ScanFormBatch"."CreatedDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime CreatedDate
		{
			get { return (System.DateTime)GetValue((int)ScanFormBatchFieldIndex.CreatedDate, true); }
			set	{ SetValue((int)ScanFormBatchFieldIndex.CreatedDate, value); }
		}

		/// <summary> The ShipmentCount property of the Entity ScanFormBatch<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ScanFormBatch"."ShipmentCount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipmentCount
		{
			get { return (System.Int32)GetValue((int)ScanFormBatchFieldIndex.ShipmentCount, true); }
			set	{ SetValue((int)ScanFormBatchFieldIndex.ShipmentCount, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'EndiciaScanFormEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(EndiciaScanFormEntity))]
		public virtual EntityCollection<EndiciaScanFormEntity> EndiciaScanForms
		{
			get { return GetOrCreateEntityCollection<EndiciaScanFormEntity, EndiciaScanFormEntityFactory>("ScanFormBatch", true, false, ref _endiciaScanForms);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'EndiciaShipmentEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(EndiciaShipmentEntity))]
		public virtual EntityCollection<EndiciaShipmentEntity> EndiciaShipment
		{
			get { return GetOrCreateEntityCollection<EndiciaShipmentEntity, EndiciaShipmentEntityFactory>("ScanFormBatch", true, false, ref _endiciaShipment);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'UspsScanFormEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(UspsScanFormEntity))]
		public virtual EntityCollection<UspsScanFormEntity> UspsScanForms
		{
			get { return GetOrCreateEntityCollection<UspsScanFormEntity, UspsScanFormEntityFactory>("ScanFormBatch", true, false, ref _uspsScanForms);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'UspsShipmentEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(UspsShipmentEntity))]
		public virtual EntityCollection<UspsShipmentEntity> UspsShipment
		{
			get { return GetOrCreateEntityCollection<UspsShipmentEntity, UspsShipmentEntityFactory>("ScanFormBatch", true, false, ref _uspsShipment);	}
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
			get { return (int)ShipWorks.Data.Model.EntityType.ScanFormBatchEntity; }
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
