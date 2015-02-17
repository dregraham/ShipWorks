///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates.NET20
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

	/// <summary>
	/// Entity class which represents the entity 'ScanFormBatch'.<br/><br/>
	/// 
	/// </summary>
	[Serializable]
	public partial class ScanFormBatchEntity : CommonEntityBase, ISerializable
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
			InitClassEmpty(null, CreateFields());
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
			InitClassEmpty(validator, CreateFields());
		}
				

		/// <summary> CTor</summary>
		/// <param name="scanFormBatchID">PK value for ScanFormBatch which data should be fetched into this ScanFormBatch object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ScanFormBatchEntity(System.Int64 scanFormBatchID):base("ScanFormBatchEntity")
		{
			InitClassEmpty(null, CreateFields());
			this.ScanFormBatchID = scanFormBatchID;
		}

		/// <summary> CTor</summary>
		/// <param name="scanFormBatchID">PK value for ScanFormBatch which data should be fetched into this ScanFormBatch object</param>
		/// <param name="validator">The custom validator object for this ScanFormBatchEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ScanFormBatchEntity(System.Int64 scanFormBatchID, IValidator validator):base("ScanFormBatchEntity")
		{
			InitClassEmpty(validator, CreateFields());
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



				base.FixupDeserialization(FieldInfoProviderSingleton.GetInstance());
			}
			
			// __LLBLGENPRO_USER_CODE_REGION_START DeserializationConstructor
			// __LLBLGENPRO_USER_CODE_REGION_END
		}

		
		/// <summary>Performs the desync setup when an FK field has been changed. The entity referenced based on the FK field will be dereferenced and sync info will be removed.</summary>
		/// <param name="fieldIndex">The fieldindex.</param>
		protected override void PerformDesyncSetupFKFieldChange(int fieldIndex)
		{
			switch((ScanFormBatchFieldIndex)fieldIndex)
			{
				default:
					base.PerformDesyncSetupFKFieldChange(fieldIndex);
					break;
			}
		}
				
		/// <summary>Gets the inheritance info provider instance of the project this entity instance is located in. </summary>
		/// <returns>ready to use inheritance info provider instance.</returns>
		protected override IInheritanceInfoProvider GetInheritanceInfoProvider()
		{
			return InheritanceInfoProviderSingleton.GetInstance();
		}
		
		/// <summary> Sets the related entity property to the entity specified. If the property is a collection, it will add the entity specified to that collection.</summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="entity">Entity to set as an related entity</param>
		/// <remarks>Used by prefetch path logic.</remarks>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void SetRelatedEntityProperty(string propertyName, IEntity2 entity)
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
					break;
			}
		}
		
		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		public override RelationCollection GetRelationsForFieldOfType(string fieldName)
		{
			return ScanFormBatchEntity.GetRelationsForField(fieldName);
		}

		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		public static RelationCollection GetRelationsForField(string fieldName)
		{
			RelationCollection toReturn = new RelationCollection();
			switch(fieldName)
			{

				case "EndiciaScanForms":
					toReturn.Add(ScanFormBatchEntity.Relations.EndiciaScanFormEntityUsingScanFormBatchID);
					break;
				case "EndiciaShipment":
					toReturn.Add(ScanFormBatchEntity.Relations.EndiciaShipmentEntityUsingScanFormBatchID);
					break;
				case "UspsScanForms":
					toReturn.Add(ScanFormBatchEntity.Relations.UspsScanFormEntityUsingScanFormBatchID);
					break;
				case "UspsShipment":
					toReturn.Add(ScanFormBatchEntity.Relations.UspsShipmentEntityUsingScanFormBatchID);
					break;


				default:

					break;				
			}
			return toReturn;
		}
#if !CF
		/// <summary>Checks if the relation mapped by the property with the name specified is a one way / single sided relation. If the passed in name is null, it
		/// will return true if the entity has any single-sided relation</summary>
		/// <param name="propertyName">Name of the property which is mapped onto the relation to check, or null to check if the entity has any relation/ which is single sided</param>
		/// <returns>true if the relation is single sided / one way (so the opposite relation isn't present), false otherwise</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool CheckOneWayRelations(string propertyName)
		{
			// use template trick to calculate the # of single-sided / oneway relations
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
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void SetRelatedEntity(IEntity2 relatedEntity, string fieldName)
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
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void UnsetRelatedEntity(IEntity2 relatedEntity, string fieldName, bool signalRelatedEntityManyToOne)
		{
			switch(fieldName)
			{

				case "EndiciaScanForms":
					base.PerformRelatedEntityRemoval(this.EndiciaScanForms, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "EndiciaShipment":
					base.PerformRelatedEntityRemoval(this.EndiciaShipment, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "UspsScanForms":
					base.PerformRelatedEntityRemoval(this.UspsScanForms, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "UspsShipment":
					base.PerformRelatedEntityRemoval(this.UspsShipment, relatedEntity, signalRelatedEntityManyToOne);
					break;

				default:
					break;
			}
		}

		/// <summary> Gets a collection of related entities referenced by this entity which depend on this entity (this entity is the PK side of their FK fields). These entities will have to be persisted after this entity during a recursive save.</summary>
		/// <returns>Collection with 0 or more IEntity2 objects, referenced by this entity</returns>
		public override List<IEntity2> GetDependingRelatedEntities()
		{
			List<IEntity2> toReturn = new List<IEntity2>();

			return toReturn;
		}
		
		/// <summary> Gets a collection of related entities referenced by this entity which this entity depends on (this entity is the FK side of their PK fields). These
		/// entities will have to be persisted before this entity during a recursive save.</summary>
		/// <returns>Collection with 0 or more IEntity2 objects, referenced by this entity</returns>
		public override List<IEntity2> GetDependentRelatedEntities()
		{
			List<IEntity2> toReturn = new List<IEntity2>();


			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. The contents of the ArrayList is used by the DataAccessAdapter to perform recursive saves. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		public override List<IEntityCollection2> GetMemberEntityCollections()
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
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
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

		/// <summary>Returns true if the original value for the field with the fieldIndex passed in, read from the persistent storage was NULL, false otherwise.
		/// Should not be used for testing if the current value is NULL, use <see cref="TestCurrentFieldValueForNull"/> for that.</summary>
		/// <param name="fieldIndex">Index of the field to test if that field was NULL in the persistent storage</param>
		/// <returns>true if the field with the passed in index was NULL in the persistent storage, false otherwise</returns>
		public bool TestOriginalFieldValueForNull(ScanFormBatchFieldIndex fieldIndex)
		{
			return base.Fields[(int)fieldIndex].IsNull;
		}
		
		/// <summary>Returns true if the current value for the field with the fieldIndex passed in represents null/not defined, false otherwise.
		/// Should not be used for testing if the original value (read from the db) is NULL</summary>
		/// <param name="fieldIndex">Index of the field to test if its currentvalue is null/undefined</param>
		/// <returns>true if the field's value isn't defined yet, false otherwise</returns>
		public bool TestCurrentFieldValueForNull(ScanFormBatchFieldIndex fieldIndex)
		{
			return base.CheckIfCurrentFieldValueIsNull((int)fieldIndex);
		}

				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			return new ScanFormBatchRelations().GetAllRelations();
		}
		

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entities of type 'EndiciaScanForm' to this entity. Use DataAccessAdapter.FetchEntityCollection() to fetch these related entities.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoEndiciaScanForms()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(EndiciaScanFormFields.ScanFormBatchID, null, ComparisonOperator.Equal, this.ScanFormBatchID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entities of type 'EndiciaShipment' to this entity. Use DataAccessAdapter.FetchEntityCollection() to fetch these related entities.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoEndiciaShipment()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(EndiciaShipmentFields.ScanFormBatchID, null, ComparisonOperator.Equal, this.ScanFormBatchID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entities of type 'UspsScanForm' to this entity. Use DataAccessAdapter.FetchEntityCollection() to fetch these related entities.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUspsScanForms()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UspsScanFormFields.ScanFormBatchID, null, ComparisonOperator.Equal, this.ScanFormBatchID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entities of type 'UspsShipment' to this entity. Use DataAccessAdapter.FetchEntityCollection() to fetch these related entities.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUspsShipment()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UspsShipmentFields.ScanFormBatchID, null, ComparisonOperator.Equal, this.ScanFormBatchID));
			return bucket;
		}



	
		
		/// <summary>Creates entity fields object for this entity. Used in constructor to setup this entity in a polymorphic scenario.</summary>
		protected virtual IEntityFields2 CreateFields()
		{
			return EntityFieldsFactory.CreateEntityFieldsObject(ShipWorks.Data.Model.EntityType.ScanFormBatchEntity);
		}

		/// <summary>
		/// Creates the ITypeDefaultValue instance used to provide default values for value types which aren't of type nullable(of T)
		/// </summary>
		/// <returns></returns>
		protected override ITypeDefaultValue CreateTypeDefaultValueProvider()
		{
			return new TypeDefaultValue();
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
			if (this._endiciaScanForms != null)
			{
				return true;
			}
			if (this._endiciaShipment != null)
			{
				return true;
			}
			if (this._uspsScanForms != null)
			{
				return true;
			}
			if (this._uspsShipment != null)
			{
				return true;
			}

			return base.HasPopulatedMemberEntityCollections();
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
		/// <summary>
		/// Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element. 
		/// </summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		public override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();

			toReturn.Add("EndiciaScanForms", _endiciaScanForms);
			toReturn.Add("EndiciaShipment", _endiciaShipment);
			toReturn.Add("UspsScanForms", _uspsScanForms);
			toReturn.Add("UspsShipment", _uspsShipment);


			return toReturn;
		}
		
		/// <summary> Adds the internals to the active context. </summary>
		protected override void AddInternalsToContext()
		{
			if(_endiciaScanForms!=null)
			{
				_endiciaScanForms.ActiveContext = base.ActiveContext;
			}
			if(_endiciaShipment!=null)
			{
				_endiciaShipment.ActiveContext = base.ActiveContext;
			}
			if(_uspsScanForms!=null)
			{
				_uspsScanForms.ActiveContext = base.ActiveContext;
			}
			if(_uspsShipment!=null)
			{
				_uspsShipment.ActiveContext = base.ActiveContext;
			}



		}

		/// <summary> Initializes the class members</summary>
		protected virtual void InitClassMembers()
		{

			_endiciaScanForms = null;
			_endiciaShipment = null;
			_uspsScanForms = null;
			_uspsShipment = null;



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

			Dictionary<string, string> fieldHashtable = null;
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
		protected virtual void InitClassEmpty(IValidator validator, IEntityFields2 fields)
		{
			OnInitializing();
			base.Fields = fields;
			base.IsNew=true;
			base.Validator = validator;
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

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'EndiciaScanForm' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathEndiciaScanForms
		{
			get
			{
				return new PrefetchPathElement2( new EntityCollection<EndiciaScanFormEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EndiciaScanFormEntityFactory))),
					(IEntityRelation)GetRelationsForField("EndiciaScanForms")[0], (int)ShipWorks.Data.Model.EntityType.ScanFormBatchEntity, (int)ShipWorks.Data.Model.EntityType.EndiciaScanFormEntity, 0, null, null, null, null, "EndiciaScanForms", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);
			}
		}
		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'EndiciaShipment' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathEndiciaShipment
		{
			get
			{
				return new PrefetchPathElement2( new EntityCollection<EndiciaShipmentEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EndiciaShipmentEntityFactory))),
					(IEntityRelation)GetRelationsForField("EndiciaShipment")[0], (int)ShipWorks.Data.Model.EntityType.ScanFormBatchEntity, (int)ShipWorks.Data.Model.EntityType.EndiciaShipmentEntity, 0, null, null, null, null, "EndiciaShipment", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);
			}
		}
		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UspsScanForm' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUspsScanForms
		{
			get
			{
				return new PrefetchPathElement2( new EntityCollection<UspsScanFormEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UspsScanFormEntityFactory))),
					(IEntityRelation)GetRelationsForField("UspsScanForms")[0], (int)ShipWorks.Data.Model.EntityType.ScanFormBatchEntity, (int)ShipWorks.Data.Model.EntityType.UspsScanFormEntity, 0, null, null, null, null, "UspsScanForms", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);
			}
		}
		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UspsShipment' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUspsShipment
		{
			get
			{
				return new PrefetchPathElement2( new EntityCollection<UspsShipmentEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UspsShipmentEntityFactory))),
					(IEntityRelation)GetRelationsForField("UspsShipment")[0], (int)ShipWorks.Data.Model.EntityType.ScanFormBatchEntity, (int)ShipWorks.Data.Model.EntityType.UspsShipmentEntity, 0, null, null, null, null, "UspsShipment", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);
			}
		}




		/// <summary> The custom properties for the type of this entity instance.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		public override Dictionary<string, string> CustomPropertiesOfType
		{
			get { return ScanFormBatchEntity.CustomProperties;}
		}

		/// <summary> The custom properties for the fields of this entity type. The returned Hashtable contains per fieldname a hashtable of name-value
		/// pairs. </summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, Dictionary<string, string>> FieldsCustomProperties
		{
			get { return _fieldsCustomProperties;}
		}

		/// <summary> The custom properties for the fields of the type of this entity instance. The returned Hashtable contains per fieldname a hashtable of name-value pairs. </summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		public override Dictionary<string, Dictionary<string, string>> FieldsCustomPropertiesOfType
		{
			get { return ScanFormBatchEntity.FieldsCustomProperties;}
		}

		/// <summary> The ScanFormBatchID property of the Entity ScanFormBatch<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ScanFormBatch"."ScanFormBatchID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 ScanFormBatchID
		{
			get { return (System.Int64)GetValue((int)ScanFormBatchFieldIndex.ScanFormBatchID, true); }
			set	{ SetValue((int)ScanFormBatchFieldIndex.ScanFormBatchID, value); }
		}

		/// <summary> The ShipmentType property of the Entity ScanFormBatch<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ScanFormBatch"."ShipmentType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipmentType
		{
			get { return (System.Int32)GetValue((int)ScanFormBatchFieldIndex.ShipmentType, true); }
			set	{ SetValue((int)ScanFormBatchFieldIndex.ShipmentType, value); }
		}

		/// <summary> The CreatedDate property of the Entity ScanFormBatch<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ScanFormBatch"."CreatedDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime CreatedDate
		{
			get { return (System.DateTime)GetValue((int)ScanFormBatchFieldIndex.CreatedDate, true); }
			set	{ SetValue((int)ScanFormBatchFieldIndex.CreatedDate, value); }
		}

		/// <summary> The ShipmentCount property of the Entity ScanFormBatch<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ScanFormBatch"."ShipmentCount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipmentCount
		{
			get { return (System.Int32)GetValue((int)ScanFormBatchFieldIndex.ShipmentCount, true); }
			set	{ SetValue((int)ScanFormBatchFieldIndex.ShipmentCount, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'EndiciaScanFormEntity' which are related to this entity via a relation of type '1:n'.
		/// If the EntityCollection hasn't been fetched yet, the collection returned will be empty.</summary>
		[TypeContainedAttribute(typeof(EndiciaScanFormEntity))]
		public virtual EntityCollection<EndiciaScanFormEntity> EndiciaScanForms
		{
			get
			{
				if(_endiciaScanForms==null)
				{
					_endiciaScanForms = new EntityCollection<EndiciaScanFormEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EndiciaScanFormEntityFactory)));
					_endiciaScanForms.SetContainingEntityInfo(this, "ScanFormBatch");
				}
				return _endiciaScanForms;
			}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'EndiciaShipmentEntity' which are related to this entity via a relation of type '1:n'.
		/// If the EntityCollection hasn't been fetched yet, the collection returned will be empty.</summary>
		[TypeContainedAttribute(typeof(EndiciaShipmentEntity))]
		public virtual EntityCollection<EndiciaShipmentEntity> EndiciaShipment
		{
			get
			{
				if(_endiciaShipment==null)
				{
					_endiciaShipment = new EntityCollection<EndiciaShipmentEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EndiciaShipmentEntityFactory)));
					_endiciaShipment.SetContainingEntityInfo(this, "ScanFormBatch");
				}
				return _endiciaShipment;
			}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'UspsScanFormEntity' which are related to this entity via a relation of type '1:n'.
		/// If the EntityCollection hasn't been fetched yet, the collection returned will be empty.</summary>
		[TypeContainedAttribute(typeof(UspsScanFormEntity))]
		public virtual EntityCollection<UspsScanFormEntity> UspsScanForms
		{
			get
			{
				if(_uspsScanForms==null)
				{
					_uspsScanForms = new EntityCollection<UspsScanFormEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UspsScanFormEntityFactory)));
					_uspsScanForms.SetContainingEntityInfo(this, "ScanFormBatch");
				}
				return _uspsScanForms;
			}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'UspsShipmentEntity' which are related to this entity via a relation of type '1:n'.
		/// If the EntityCollection hasn't been fetched yet, the collection returned will be empty.</summary>
		[TypeContainedAttribute(typeof(UspsShipmentEntity))]
		public virtual EntityCollection<UspsShipmentEntity> UspsShipment
		{
			get
			{
				if(_uspsShipment==null)
				{
					_uspsShipment = new EntityCollection<UspsShipmentEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UspsShipmentEntityFactory)));
					_uspsShipment.SetContainingEntityInfo(this, "ScanFormBatch");
				}
				return _uspsShipment;
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
		public override int LLBLGenProEntityTypeValue 
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
