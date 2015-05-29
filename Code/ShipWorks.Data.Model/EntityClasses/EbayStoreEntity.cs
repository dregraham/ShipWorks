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
	/// Entity class which represents the entity 'EbayStore'.<br/><br/>
	/// 
	/// </summary>
	[Serializable]
	public partial class EbayStoreEntity : StoreEntity, ISerializable
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<EbayCombinedOrderRelationEntity> _ebayCombinedOrderRelation;
		private EntityCollection<EbayOrderEntity> _ebayOrderCollectionViaEbayCombinedOrderRelation;


		
		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static new partial class MemberNames
		{


			/// <summary>Member name EbayCombinedOrderRelation</summary>
			public static readonly string EbayCombinedOrderRelation = "EbayCombinedOrderRelation";




			/// <summary>Member name EbayOrderCollectionViaEbayCombinedOrderRelation</summary>
			public static readonly string EbayOrderCollectionViaEbayCombinedOrderRelation = "EbayOrderCollectionViaEbayCombinedOrderRelation";


		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static EbayStoreEntity()
		{
			SetupCustomPropertyHashtables();
		}

		/// <summary> CTor</summary>
		public EbayStoreEntity()
		{
			SetName("EbayStoreEntity");
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public EbayStoreEntity(IEntityFields2 fields):base(fields)
		{
			SetName("EbayStoreEntity");
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this EbayStoreEntity</param>
		public EbayStoreEntity(IValidator validator):base(validator)
		{
			SetName("EbayStoreEntity");
		}
				

		/// <summary> CTor</summary>
		/// <param name="storeID">PK value for EbayStore which data should be fetched into this EbayStore object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public EbayStoreEntity(System.Int64 storeID):base(storeID)
		{
			SetName("EbayStoreEntity");
		}

		/// <summary> CTor</summary>
		/// <param name="storeID">PK value for EbayStore which data should be fetched into this EbayStore object</param>
		/// <param name="validator">The custom validator object for this EbayStoreEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public EbayStoreEntity(System.Int64 storeID, IValidator validator):base(storeID, validator)
		{
			SetName("EbayStoreEntity");
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected EbayStoreEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_ebayCombinedOrderRelation = (EntityCollection<EbayCombinedOrderRelationEntity>)info.GetValue("_ebayCombinedOrderRelation", typeof(EntityCollection<EbayCombinedOrderRelationEntity>));
				_ebayOrderCollectionViaEbayCombinedOrderRelation = (EntityCollection<EbayOrderEntity>)info.GetValue("_ebayOrderCollectionViaEbayCombinedOrderRelation", typeof(EntityCollection<EbayOrderEntity>));


				base.FixupDeserialization(FieldInfoProviderSingleton.GetInstance());
			}
			
			// __LLBLGENPRO_USER_CODE_REGION_START DeserializationConstructor
			// __LLBLGENPRO_USER_CODE_REGION_END
		}

		
		/// <summary>Performs the desync setup when an FK field has been changed. The entity referenced based on the FK field will be dereferenced and sync info will be removed.</summary>
		/// <param name="fieldIndex">The fieldindex.</param>
		protected override void PerformDesyncSetupFKFieldChange(int fieldIndex)
		{
			switch((EbayStoreFieldIndex)fieldIndex)
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

				case "EbayCombinedOrderRelation":
					this.EbayCombinedOrderRelation.Add((EbayCombinedOrderRelationEntity)entity);
					break;
				case "EbayOrderCollectionViaEbayCombinedOrderRelation":
					this.EbayOrderCollectionViaEbayCombinedOrderRelation.IsReadOnly = false;
					this.EbayOrderCollectionViaEbayCombinedOrderRelation.Add((EbayOrderEntity)entity);
					this.EbayOrderCollectionViaEbayCombinedOrderRelation.IsReadOnly = true;
					break;

				default:
						base.SetRelatedEntityProperty(propertyName, entity);
					break;
			}
		}
		
		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		public override RelationCollection GetRelationsForFieldOfType(string fieldName)
		{
			return EbayStoreEntity.GetRelationsForField(fieldName);
		}

		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		public static new RelationCollection GetRelationsForField(string fieldName)
		{
			RelationCollection toReturn = new RelationCollection();
			switch(fieldName)
			{

				case "EbayCombinedOrderRelation":
					toReturn.Add(EbayStoreEntity.Relations.EbayCombinedOrderRelationEntityUsingStoreID);
					break;
				case "EbayOrderCollectionViaEbayCombinedOrderRelation":
					toReturn.Add(EbayStoreEntity.Relations.EbayCombinedOrderRelationEntityUsingStoreID, "EbayStoreEntity__", "EbayCombinedOrderRelation_", JoinHint.None);
					toReturn.Add(EbayCombinedOrderRelationEntity.Relations.EbayOrderEntityUsingOrderID, "EbayCombinedOrderRelation_", string.Empty, JoinHint.None);
					break;

				default:
					toReturn = StoreEntity.GetRelationsForField(fieldName);
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

				case "EbayCombinedOrderRelation":
					this.EbayCombinedOrderRelation.Add((EbayCombinedOrderRelationEntity)relatedEntity);
					break;

				default:
					base.SetRelatedEntity(relatedEntity, fieldName);
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

				case "EbayCombinedOrderRelation":
					base.PerformRelatedEntityRemoval(this.EbayCombinedOrderRelation, relatedEntity, signalRelatedEntityManyToOne);
					break;

				default:
					base.UnsetRelatedEntity(relatedEntity, fieldName, signalRelatedEntityManyToOne);
					break;
			}
		}

		/// <summary> Gets a collection of related entities referenced by this entity which depend on this entity (this entity is the PK side of their FK fields). These entities will have to be persisted after this entity during a recursive save.</summary>
		/// <returns>Collection with 0 or more IEntity2 objects, referenced by this entity</returns>
		public override List<IEntity2> GetDependingRelatedEntities()
		{
			List<IEntity2> toReturn = new List<IEntity2>();

			toReturn.AddRange(base.GetDependingRelatedEntities());
			return toReturn;
		}
		
		/// <summary> Gets a collection of related entities referenced by this entity which this entity depends on (this entity is the FK side of their PK fields). These
		/// entities will have to be persisted before this entity during a recursive save.</summary>
		/// <returns>Collection with 0 or more IEntity2 objects, referenced by this entity</returns>
		public override List<IEntity2> GetDependentRelatedEntities()
		{
			List<IEntity2> toReturn = new List<IEntity2>();


			toReturn.AddRange(base.GetDependentRelatedEntities());
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. The contents of the ArrayList is used by the DataAccessAdapter to perform recursive saves. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		public override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.Add(this.EbayCombinedOrderRelation);
			toReturn.AddRange(base.GetMemberEntityCollections());
			return toReturn;
		}
		
		/// <summary>Gets the inheritance info for this entity, if applicable (it's then overriden) or null if not.</summary>
		/// <returns>InheritanceInfo object if this entity is in a hierarchy of type TargetPerEntity, or null otherwise</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override IInheritanceInfo GetInheritanceInfo()
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetInheritanceInfo("EbayStoreEntity", false);
		}
		
		/// <summary>Gets a predicateexpression which filters on this entity</summary>
		/// <returns>ready to use predicateexpression</returns>
		/// <remarks>Only useful in entity fetches.</remarks>
		public new static IPredicateExpression GetEntityTypeFilter()
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetEntityTypeFilter("EbayStoreEntity", false);
		}
		
		/// <summary>Gets a predicateexpression which filters on this entity</summary>
		/// <param name="negate">Flag to produce a NOT filter, (true), or a normal filter (false). </param>
		/// <returns>ready to use predicateexpression</returns>
		/// <remarks>Only useful in entity fetches.</remarks>
		public new static IPredicateExpression GetEntityTypeFilter(bool negate)
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetEntityTypeFilter("EbayStoreEntity", negate);
		}

		/// <summary>ISerializable member. Does custom serialization so event handlers do not get serialized. Serializes members of this entity class and uses the base class' implementation to serialize the rest.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				info.AddValue("_ebayCombinedOrderRelation", ((_ebayCombinedOrderRelation!=null) && (_ebayCombinedOrderRelation.Count>0) && !this.MarkedForDeletion)?_ebayCombinedOrderRelation:null);
				info.AddValue("_ebayOrderCollectionViaEbayCombinedOrderRelation", ((_ebayOrderCollectionViaEbayCombinedOrderRelation!=null) && (_ebayOrderCollectionViaEbayCombinedOrderRelation.Count>0) && !this.MarkedForDeletion)?_ebayOrderCollectionViaEbayCombinedOrderRelation:null);


			}
			
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}

		/// <summary>Returns true if the original value for the field with the fieldIndex passed in, read from the persistent storage was NULL, false otherwise.
		/// Should not be used for testing if the current value is NULL, use <see cref="TestCurrentFieldValueForNull"/> for that.</summary>
		/// <param name="fieldIndex">Index of the field to test if that field was NULL in the persistent storage</param>
		/// <returns>true if the field with the passed in index was NULL in the persistent storage, false otherwise</returns>
		public bool TestOriginalFieldValueForNull(EbayStoreFieldIndex fieldIndex)
		{
			return base.Fields[(int)fieldIndex].IsNull;
		}
		
		/// <summary>Returns true if the current value for the field with the fieldIndex passed in represents null/not defined, false otherwise.
		/// Should not be used for testing if the original value (read from the db) is NULL</summary>
		/// <param name="fieldIndex">Index of the field to test if its currentvalue is null/undefined</param>
		/// <returns>true if the field's value isn't defined yet, false otherwise</returns>
		public bool TestCurrentFieldValueForNull(EbayStoreFieldIndex fieldIndex)
		{
			return base.CheckIfCurrentFieldValueIsNull((int)fieldIndex);
		}
		
		/// <summary>Determines whether this entity is a subType of the entity represented by the passed in enum value, which represents a value in the ShipWorks.Data.Model.EntityType enum</summary>
		/// <param name="typeOfEntity">Type of entity.</param>
		/// <returns>true if the passed in type is a supertype of this entity, otherwise false</returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool CheckIfIsSubTypeOf(int typeOfEntity)
		{
			return InheritanceInfoProviderSingleton.GetInstance().CheckIfIsSubTypeOf("EbayStoreEntity", ((ShipWorks.Data.Model.EntityType)typeOfEntity).ToString());
		}
				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			return new EbayStoreRelations().GetAllRelations();
		}
		

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entities of type 'EbayCombinedOrderRelation' to this entity. Use DataAccessAdapter.FetchEntityCollection() to fetch these related entities.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoEbayCombinedOrderRelation()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(EbayCombinedOrderRelationFields.StoreID, null, ComparisonOperator.Equal, this.StoreID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entities of type 'EbayOrder' to this entity. Use DataAccessAdapter.FetchEntityCollection() to fetch these related entities.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoEbayOrderCollectionViaEbayCombinedOrderRelation()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.Relations.AddRange(GetRelationsForFieldOfType("EbayOrderCollectionViaEbayCombinedOrderRelation"));
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(EbayStoreFields.StoreID, null, ComparisonOperator.Equal, this.StoreID, "EbayStoreEntity__"));
			return bucket;
		}


	
		
		/// <summary>Creates entity fields object for this entity. Used in constructor to setup this entity in a polymorphic scenario.</summary>
		protected override IEntityFields2 CreateFields()
		{
			return EntityFieldsFactory.CreateEntityFieldsObject(ShipWorks.Data.Model.EntityType.EbayStoreEntity);
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
			return EntityFactoryCache2.GetEntityFactory(typeof(EbayStoreEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._ebayCombinedOrderRelation);
			collectionsQueue.Enqueue(this._ebayOrderCollectionViaEbayCombinedOrderRelation);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._ebayCombinedOrderRelation = (EntityCollection<EbayCombinedOrderRelationEntity>) collectionsQueue.Dequeue();
			this._ebayOrderCollectionViaEbayCombinedOrderRelation = (EntityCollection<EbayOrderEntity>) collectionsQueue.Dequeue();
		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			if (this._ebayCombinedOrderRelation != null)
			{
				return true;
			}
			if (this._ebayOrderCollectionViaEbayCombinedOrderRelation != null)
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
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<EbayCombinedOrderRelationEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EbayCombinedOrderRelationEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<EbayOrderEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EbayOrderEntityFactory))) : null);
		}
#endif
		/// <summary>
		/// Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element. 
		/// </summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		public override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = base.GetRelatedData();

			toReturn.Add("EbayCombinedOrderRelation", _ebayCombinedOrderRelation);
			toReturn.Add("EbayOrderCollectionViaEbayCombinedOrderRelation", _ebayOrderCollectionViaEbayCombinedOrderRelation);

			return toReturn;
		}
		
		/// <summary> Adds the internals to the active context. </summary>
		protected override void AddInternalsToContext()
		{
			if(_ebayCombinedOrderRelation!=null)
			{
				_ebayCombinedOrderRelation.ActiveContext = base.ActiveContext;
			}
			if(_ebayOrderCollectionViaEbayCombinedOrderRelation!=null)
			{
				_ebayOrderCollectionViaEbayCombinedOrderRelation.ActiveContext = base.ActiveContext;
			}


			base.AddInternalsToContext();
		}

		/// <summary> Initializes the class members</summary>
		protected override void InitClassMembers()
		{
			base.InitClassMembers();
			_ebayCombinedOrderRelation = null;
			_ebayOrderCollectionViaEbayCombinedOrderRelation = null;



			
			// __LLBLGENPRO_USER_CODE_REGION_START InitClassMembers
			// __LLBLGENPRO_USER_CODE_REGION_END

		}

		#region Custom Property Hashtable Setup
		/// <summary> Initializes the hashtables for the entity type and entity field custom properties. </summary>
		private static void SetupCustomPropertyHashtables()
		{
			_customProperties = new Dictionary<string, string>();
			_fieldsCustomProperties = new Dictionary<string, Dictionary<string, string>>();

			Dictionary<string, string> fieldHashtable = null;
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("StoreID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EBayUserID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EBayToken", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EBayTokenExpire", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("AcceptedPaymentList", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DownloadItemDetails", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DownloadOlderOrders", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DownloadPayPalDetails", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PayPalApiCredentialType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PayPalApiUserName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PayPalApiPassword", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PayPalApiSignature", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PayPalApiCertificate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DomesticShippingService", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("InternationalShippingService", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("FeedbackUpdatedThrough", fieldHashtable);
		}
		#endregion



		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this EbayStoreEntity</param>
		/// <param name="fields">Fields of this entity</param>
		protected override void InitClassEmpty(IValidator validator, IEntityFields2 fields)
		{

			base.InitClassEmpty(validator, fields);

			
			// __LLBLGENPRO_USER_CODE_REGION_START InitClassEmpty
			// __LLBLGENPRO_USER_CODE_REGION_END


		}

		#region Class Property Declarations
		/// <summary> The relations object holding all relations of this entity with other entity classes.</summary>
		public new static EbayStoreRelations Relations
		{
			get	{ return new EbayStoreRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public new static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'EbayCombinedOrderRelation' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathEbayCombinedOrderRelation
		{
			get
			{
				return new PrefetchPathElement2( new EntityCollection<EbayCombinedOrderRelationEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EbayCombinedOrderRelationEntityFactory))),
					(IEntityRelation)GetRelationsForField("EbayCombinedOrderRelation")[0], (int)ShipWorks.Data.Model.EntityType.EbayStoreEntity, (int)ShipWorks.Data.Model.EntityType.EbayCombinedOrderRelationEntity, 0, null, null, null, null, "EbayCombinedOrderRelation", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);
			}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'EbayOrder' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathEbayOrderCollectionViaEbayCombinedOrderRelation
		{
			get
			{
				IEntityRelation intermediateRelation = EbayStoreEntity.Relations.EbayCombinedOrderRelationEntityUsingStoreID;
				intermediateRelation.SetAliases(string.Empty, "EbayCombinedOrderRelation_");
				return new PrefetchPathElement2(new EntityCollection<EbayOrderEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EbayOrderEntityFactory))), intermediateRelation,
					(int)ShipWorks.Data.Model.EntityType.EbayStoreEntity, (int)ShipWorks.Data.Model.EntityType.EbayOrderEntity, 0, null, null, GetRelationsForField("EbayOrderCollectionViaEbayCombinedOrderRelation"), null, "EbayOrderCollectionViaEbayCombinedOrderRelation", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToMany);
			}
		}



		/// <summary> The custom properties for the type of this entity instance.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		public override Dictionary<string, string> CustomPropertiesOfType
		{
			get { return EbayStoreEntity.CustomProperties;}
		}

		/// <summary> The custom properties for the fields of this entity type. The returned Hashtable contains per fieldname a hashtable of name-value
		/// pairs. </summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public new static Dictionary<string, Dictionary<string, string>> FieldsCustomProperties
		{
			get { return _fieldsCustomProperties;}
		}

		/// <summary> The custom properties for the fields of the type of this entity instance. The returned Hashtable contains per fieldname a hashtable of name-value pairs. </summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		public override Dictionary<string, Dictionary<string, string>> FieldsCustomPropertiesOfType
		{
			get { return EbayStoreEntity.FieldsCustomProperties;}
		}

		/// <summary> The StoreID property of the Entity EbayStore<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "EbayStore"."StoreID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
		public override System.Int64 StoreID
		{
			get { return (System.Int64)GetValue((int)EbayStoreFieldIndex.StoreID, true); }
			set	{ SetValue((int)EbayStoreFieldIndex.StoreID, value); }
		}

		/// <summary> The EBayUserID property of the Entity EbayStore<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "EbayStore"."eBayUserID"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String EBayUserID
		{
			get { return (System.String)GetValue((int)EbayStoreFieldIndex.EBayUserID, true); }
			set	{ SetValue((int)EbayStoreFieldIndex.EBayUserID, value); }
		}

		/// <summary> The EBayToken property of the Entity EbayStore<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "EbayStore"."eBayToken"<br/>
		/// Table field type characteristics (type, precision, scale, length): Text, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String EBayToken
		{
			get { return (System.String)GetValue((int)EbayStoreFieldIndex.EBayToken, true); }
			set	{ SetValue((int)EbayStoreFieldIndex.EBayToken, value); }
		}

		/// <summary> The EBayTokenExpire property of the Entity EbayStore<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "EbayStore"."eBayTokenExpire"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime EBayTokenExpire
		{
			get { return (System.DateTime)GetValue((int)EbayStoreFieldIndex.EBayTokenExpire, true); }
			set	{ SetValue((int)EbayStoreFieldIndex.EBayTokenExpire, value); }
		}

		/// <summary> The AcceptedPaymentList property of the Entity EbayStore<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "EbayStore"."AcceptedPaymentList"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String AcceptedPaymentList
		{
			get { return (System.String)GetValue((int)EbayStoreFieldIndex.AcceptedPaymentList, true); }
			set	{ SetValue((int)EbayStoreFieldIndex.AcceptedPaymentList, value); }
		}

		/// <summary> The DownloadItemDetails property of the Entity EbayStore<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "EbayStore"."DownloadItemDetails"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean DownloadItemDetails
		{
			get { return (System.Boolean)GetValue((int)EbayStoreFieldIndex.DownloadItemDetails, true); }
			set	{ SetValue((int)EbayStoreFieldIndex.DownloadItemDetails, value); }
		}

		/// <summary> The DownloadOlderOrders property of the Entity EbayStore<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "EbayStore"."DownloadOlderOrders"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean DownloadOlderOrders
		{
			get { return (System.Boolean)GetValue((int)EbayStoreFieldIndex.DownloadOlderOrders, true); }
			set	{ SetValue((int)EbayStoreFieldIndex.DownloadOlderOrders, value); }
		}

		/// <summary> The DownloadPayPalDetails property of the Entity EbayStore<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "EbayStore"."DownloadPayPalDetails"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean DownloadPayPalDetails
		{
			get { return (System.Boolean)GetValue((int)EbayStoreFieldIndex.DownloadPayPalDetails, true); }
			set	{ SetValue((int)EbayStoreFieldIndex.DownloadPayPalDetails, value); }
		}

		/// <summary> The PayPalApiCredentialType property of the Entity EbayStore<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "EbayStore"."PayPalApiCredentialType"<br/>
		/// Table field type characteristics (type, precision, scale, length): SmallInt, 5, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int16 PayPalApiCredentialType
		{
			get { return (System.Int16)GetValue((int)EbayStoreFieldIndex.PayPalApiCredentialType, true); }
			set	{ SetValue((int)EbayStoreFieldIndex.PayPalApiCredentialType, value); }
		}

		/// <summary> The PayPalApiUserName property of the Entity EbayStore<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "EbayStore"."PayPalApiUserName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String PayPalApiUserName
		{
			get { return (System.String)GetValue((int)EbayStoreFieldIndex.PayPalApiUserName, true); }
			set	{ SetValue((int)EbayStoreFieldIndex.PayPalApiUserName, value); }
		}

		/// <summary> The PayPalApiPassword property of the Entity EbayStore<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "EbayStore"."PayPalApiPassword"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String PayPalApiPassword
		{
			get { return (System.String)GetValue((int)EbayStoreFieldIndex.PayPalApiPassword, true); }
			set	{ SetValue((int)EbayStoreFieldIndex.PayPalApiPassword, value); }
		}

		/// <summary> The PayPalApiSignature property of the Entity EbayStore<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "EbayStore"."PayPalApiSignature"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String PayPalApiSignature
		{
			get { return (System.String)GetValue((int)EbayStoreFieldIndex.PayPalApiSignature, true); }
			set	{ SetValue((int)EbayStoreFieldIndex.PayPalApiSignature, value); }
		}

		/// <summary> The PayPalApiCertificate property of the Entity EbayStore<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "EbayStore"."PayPalApiCertificate"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2048<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.Byte[] PayPalApiCertificate
		{
			get { return (System.Byte[])GetValue((int)EbayStoreFieldIndex.PayPalApiCertificate, true); }
			set	{ SetValue((int)EbayStoreFieldIndex.PayPalApiCertificate, value); }
		}

		/// <summary> The DomesticShippingService property of the Entity EbayStore<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "EbayStore"."DomesticShippingService"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String DomesticShippingService
		{
			get { return (System.String)GetValue((int)EbayStoreFieldIndex.DomesticShippingService, true); }
			set	{ SetValue((int)EbayStoreFieldIndex.DomesticShippingService, value); }
		}

		/// <summary> The InternationalShippingService property of the Entity EbayStore<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "EbayStore"."InternationalShippingService"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String InternationalShippingService
		{
			get { return (System.String)GetValue((int)EbayStoreFieldIndex.InternationalShippingService, true); }
			set	{ SetValue((int)EbayStoreFieldIndex.InternationalShippingService, value); }
		}

		/// <summary> The FeedbackUpdatedThrough property of the Entity EbayStore<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "EbayStore"."FeedbackUpdatedThrough"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.DateTime> FeedbackUpdatedThrough
		{
			get { return (Nullable<System.DateTime>)GetValue((int)EbayStoreFieldIndex.FeedbackUpdatedThrough, false); }
			set	{ SetValue((int)EbayStoreFieldIndex.FeedbackUpdatedThrough, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'EbayCombinedOrderRelationEntity' which are related to this entity via a relation of type '1:n'.
		/// If the EntityCollection hasn't been fetched yet, the collection returned will be empty.</summary>
		[TypeContainedAttribute(typeof(EbayCombinedOrderRelationEntity))]
		public virtual EntityCollection<EbayCombinedOrderRelationEntity> EbayCombinedOrderRelation
		{
			get
			{
				if(_ebayCombinedOrderRelation==null)
				{
					_ebayCombinedOrderRelation = new EntityCollection<EbayCombinedOrderRelationEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EbayCombinedOrderRelationEntityFactory)));
					_ebayCombinedOrderRelation.SetContainingEntityInfo(this, "EbayStore");
				}
				return _ebayCombinedOrderRelation;
			}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'EbayOrderEntity' which are related to this entity via a relation of type 'm:n'.
		/// If the EntityCollection hasn't been fetched yet, the collection returned will be empty.</summary>
		[TypeContainedAttribute(typeof(EbayOrderEntity))]
		public virtual EntityCollection<EbayOrderEntity> EbayOrderCollectionViaEbayCombinedOrderRelation
		{
			get
			{
				if(_ebayOrderCollectionViaEbayCombinedOrderRelation==null)
				{
					_ebayOrderCollectionViaEbayCombinedOrderRelation = new EntityCollection<EbayOrderEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EbayOrderEntityFactory)));
					_ebayOrderCollectionViaEbayCombinedOrderRelation.IsReadOnly=true;
				}
				return _ebayOrderCollectionViaEbayCombinedOrderRelation;
			}
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
		public override int LLBLGenProEntityTypeValue 
		{ 
			get { return (int)ShipWorks.Data.Model.EntityType.EbayStoreEntity; }
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
