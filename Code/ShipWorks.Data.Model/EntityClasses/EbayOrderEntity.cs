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
	/// <summary>Entity class which represents the entity 'EbayOrder'.<br/><br/></summary>
	[Serializable]
	public partial class EbayOrderEntity : OrderEntity
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<EbayCombinedOrderRelationEntity> _ebayCombinedOrderRelation;
		private EntityCollection<EbayOrderSearchEntity> _ebayOrderSearch;
		private EntityCollection<EbayStoreEntity> _ebayStoreCollectionViaEbayCombinedOrderRelation;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static new partial class MemberNames
		{
			/// <summary>Member name Customer</summary>
			public static readonly string Customer = "Customer";
			/// <summary>Member name Store</summary>
			public static readonly string Store = "Store";
			/// <summary>Member name EbayCombinedOrderRelation</summary>
			public static readonly string EbayCombinedOrderRelation = "EbayCombinedOrderRelation";
			/// <summary>Member name EbayOrderSearch</summary>
			public static readonly string EbayOrderSearch = "EbayOrderSearch";
			/// <summary>Member name Notes</summary>
			public static readonly string Notes = "Notes";
			/// <summary>Member name OrderCharges</summary>
			public static readonly string OrderCharges = "OrderCharges";
			/// <summary>Member name OrderItems</summary>
			public static readonly string OrderItems = "OrderItems";
			/// <summary>Member name OrderPaymentDetails</summary>
			public static readonly string OrderPaymentDetails = "OrderPaymentDetails";
			/// <summary>Member name OrderSearch</summary>
			public static readonly string OrderSearch = "OrderSearch";
			/// <summary>Member name Shipments</summary>
			public static readonly string Shipments = "Shipments";
			/// <summary>Member name ValidatedAddress</summary>
			public static readonly string ValidatedAddress = "ValidatedAddress";
			/// <summary>Member name EbayStoreCollectionViaEbayCombinedOrderRelation</summary>
			public static readonly string EbayStoreCollectionViaEbayCombinedOrderRelation = "EbayStoreCollectionViaEbayCombinedOrderRelation";
			/// <summary>Member name ShipmentCollectionViaValidatedAddress</summary>
			public static readonly string ShipmentCollectionViaValidatedAddress = "ShipmentCollectionViaValidatedAddress";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static EbayOrderEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public EbayOrderEntity()
		{
			InitClassEmpty();
			SetName("EbayOrderEntity");
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public EbayOrderEntity(IEntityFields2 fields):base(fields)
		{
			InitClassEmpty();
			SetName("EbayOrderEntity");
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this EbayOrderEntity</param>
		public EbayOrderEntity(IValidator validator):base(validator)
		{
			InitClassEmpty();
			SetName("EbayOrderEntity");
		}
				
		/// <summary> CTor</summary>
		/// <param name="orderID">PK value for EbayOrder which data should be fetched into this EbayOrder object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public EbayOrderEntity(System.Int64 orderID):base(orderID)
		{
			InitClassEmpty();

			SetName("EbayOrderEntity");
		}

		/// <summary> CTor</summary>
		/// <param name="orderID">PK value for EbayOrder which data should be fetched into this EbayOrder object</param>
		/// <param name="validator">The custom validator object for this EbayOrderEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public EbayOrderEntity(System.Int64 orderID, IValidator validator):base(orderID, validator)
		{
			InitClassEmpty();

			SetName("EbayOrderEntity");
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected EbayOrderEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_ebayCombinedOrderRelation = (EntityCollection<EbayCombinedOrderRelationEntity>)info.GetValue("_ebayCombinedOrderRelation", typeof(EntityCollection<EbayCombinedOrderRelationEntity>));
				_ebayOrderSearch = (EntityCollection<EbayOrderSearchEntity>)info.GetValue("_ebayOrderSearch", typeof(EntityCollection<EbayOrderSearchEntity>));
				_ebayStoreCollectionViaEbayCombinedOrderRelation = (EntityCollection<EbayStoreEntity>)info.GetValue("_ebayStoreCollectionViaEbayCombinedOrderRelation", typeof(EntityCollection<EbayStoreEntity>));
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
				case "EbayCombinedOrderRelation":
					this.EbayCombinedOrderRelation.Add((EbayCombinedOrderRelationEntity)entity);
					break;
				case "EbayOrderSearch":
					this.EbayOrderSearch.Add((EbayOrderSearchEntity)entity);
					break;
				case "EbayStoreCollectionViaEbayCombinedOrderRelation":
					this.EbayStoreCollectionViaEbayCombinedOrderRelation.IsReadOnly = false;
					this.EbayStoreCollectionViaEbayCombinedOrderRelation.Add((EbayStoreEntity)entity);
					this.EbayStoreCollectionViaEbayCombinedOrderRelation.IsReadOnly = true;
					break;
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
				case "EbayCombinedOrderRelation":
					toReturn.Add(Relations.EbayCombinedOrderRelationEntityUsingOrderID);
					break;
				case "EbayOrderSearch":
					toReturn.Add(Relations.EbayOrderSearchEntityUsingOrderID);
					break;
				case "EbayStoreCollectionViaEbayCombinedOrderRelation":
					toReturn.Add(Relations.EbayCombinedOrderRelationEntityUsingOrderID, "EbayOrderEntity__", "EbayCombinedOrderRelation_", JoinHint.None);
					toReturn.Add(EbayCombinedOrderRelationEntity.Relations.EbayStoreEntityUsingStoreID, "EbayCombinedOrderRelation_", string.Empty, JoinHint.None);
					break;
				default:
					toReturn = OrderEntity.GetRelationsForField(fieldName);
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
				case "EbayCombinedOrderRelation":
					this.EbayCombinedOrderRelation.Add((EbayCombinedOrderRelationEntity)relatedEntity);
					break;
				case "EbayOrderSearch":
					this.EbayOrderSearch.Add((EbayOrderSearchEntity)relatedEntity);
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
		protected override void UnsetRelatedEntity(IEntityCore relatedEntity, string fieldName, bool signalRelatedEntityManyToOne)
		{
			switch(fieldName)
			{
				case "EbayCombinedOrderRelation":
					this.PerformRelatedEntityRemoval(this.EbayCombinedOrderRelation, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "EbayOrderSearch":
					this.PerformRelatedEntityRemoval(this.EbayOrderSearch, relatedEntity, signalRelatedEntityManyToOne);
					break;
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
			toReturn.Add(this.EbayCombinedOrderRelation);
			toReturn.Add(this.EbayOrderSearch);
			toReturn.AddRange(base.GetMemberEntityCollections());
			return toReturn;
		}

		/// <summary>Gets a predicateexpression which filters on this entity</summary>
		/// <returns>ready to use predicateexpression</returns>
		/// <remarks>Only useful in entity fetches.</remarks>
		public new static IPredicateExpression GetEntityTypeFilter()
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetEntityTypeFilter("EbayOrderEntity", false);
		}
		
		/// <summary>Gets a predicateexpression which filters on this entity</summary>
		/// <param name="negate">Flag to produce a NOT filter, (true), or a normal filter (false). </param>
		/// <returns>ready to use predicateexpression</returns>
		/// <remarks>Only useful in entity fetches.</remarks>
		public new static IPredicateExpression GetEntityTypeFilter(bool negate)
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetEntityTypeFilter("EbayOrderEntity", negate);
		}

		/// <summary>ISerializable member. Does custom serialization so event handlers do not get serialized. Serializes members of this entity class and uses the base class' implementation to serialize the rest.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				info.AddValue("_ebayCombinedOrderRelation", ((_ebayCombinedOrderRelation!=null) && (_ebayCombinedOrderRelation.Count>0) && !this.MarkedForDeletion)?_ebayCombinedOrderRelation:null);
				info.AddValue("_ebayOrderSearch", ((_ebayOrderSearch!=null) && (_ebayOrderSearch.Count>0) && !this.MarkedForDeletion)?_ebayOrderSearch:null);
				info.AddValue("_ebayStoreCollectionViaEbayCombinedOrderRelation", ((_ebayStoreCollectionViaEbayCombinedOrderRelation!=null) && (_ebayStoreCollectionViaEbayCombinedOrderRelation.Count>0) && !this.MarkedForDeletion)?_ebayStoreCollectionViaEbayCombinedOrderRelation:null);
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
			return InheritanceInfoProviderSingleton.GetInstance().CheckIfIsSubTypeOf("EbayOrderEntity", ((ShipWorks.Data.Model.EntityType)typeOfEntity).ToString());
		}
				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new EbayOrderRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'EbayCombinedOrderRelation' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoEbayCombinedOrderRelation()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(EbayCombinedOrderRelationFields.OrderID, null, ComparisonOperator.Equal, this.OrderID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'EbayOrderSearch' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoEbayOrderSearch()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(EbayOrderSearchFields.OrderID, null, ComparisonOperator.Equal, this.OrderID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'EbayStore' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoEbayStoreCollectionViaEbayCombinedOrderRelation()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.Relations.AddRange(GetRelationsForFieldOfType("EbayStoreCollectionViaEbayCombinedOrderRelation"));
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(EbayOrderFields.OrderID, null, ComparisonOperator.Equal, this.OrderID, "EbayOrderEntity__"));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(EbayOrderEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._ebayCombinedOrderRelation);
			collectionsQueue.Enqueue(this._ebayOrderSearch);
			collectionsQueue.Enqueue(this._ebayStoreCollectionViaEbayCombinedOrderRelation);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._ebayCombinedOrderRelation = (EntityCollection<EbayCombinedOrderRelationEntity>) collectionsQueue.Dequeue();
			this._ebayOrderSearch = (EntityCollection<EbayOrderSearchEntity>) collectionsQueue.Dequeue();
			this._ebayStoreCollectionViaEbayCombinedOrderRelation = (EntityCollection<EbayStoreEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._ebayCombinedOrderRelation != null);
			toReturn |=(this._ebayOrderSearch != null);
			toReturn |= (this._ebayStoreCollectionViaEbayCombinedOrderRelation != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<EbayCombinedOrderRelationEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EbayCombinedOrderRelationEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<EbayOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EbayOrderSearchEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<EbayStoreEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EbayStoreEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = base.GetRelatedData();
			toReturn.Add("EbayCombinedOrderRelation", _ebayCombinedOrderRelation);
			toReturn.Add("EbayOrderSearch", _ebayOrderSearch);
			toReturn.Add("EbayStoreCollectionViaEbayCombinedOrderRelation", _ebayStoreCollectionViaEbayCombinedOrderRelation);
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
			_fieldsCustomProperties.Add("EbayOrderID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("EbayBuyerID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CombinedLocally", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SelectedShippingMethod", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SellingManagerRecord", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("GspEligible", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("GspFirstName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("GspLastName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("GspStreet1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("GspStreet2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("GspCity", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("GspStateProvince", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("GspPostalCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("GspCountryCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("GspReferenceID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupEbayItemCount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupEffectiveCheckoutStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupEffectivePaymentMethod", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupFeedbackLeftType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupFeedbackLeftComments", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupFeedbackReceivedType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupFeedbackReceivedComments", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RollupPayPalAddressStatus", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("GuaranteedDelivery", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ExtendedOrderID", fieldHashtable);
		}
		#endregion

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this EbayOrderEntity</param>
		private void InitClassEmpty()
		{
			InitClassMembers();

			// __LLBLGENPRO_USER_CODE_REGION_START InitClassEmpty
			// __LLBLGENPRO_USER_CODE_REGION_END


		}

		#region Class Property Declarations
		/// <summary> The relations object holding all relations of this entity with other entity classes.</summary>
		public new static EbayOrderRelations Relations
		{
			get	{ return new EbayOrderRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public new static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'EbayCombinedOrderRelation' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathEbayCombinedOrderRelation
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<EbayCombinedOrderRelationEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EbayCombinedOrderRelationEntityFactory))), (IEntityRelation)GetRelationsForField("EbayCombinedOrderRelation")[0], (int)ShipWorks.Data.Model.EntityType.EbayOrderEntity, (int)ShipWorks.Data.Model.EntityType.EbayCombinedOrderRelationEntity, 0, null, null, null, null, "EbayCombinedOrderRelation", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'EbayOrderSearch' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathEbayOrderSearch
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<EbayOrderSearchEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EbayOrderSearchEntityFactory))), (IEntityRelation)GetRelationsForField("EbayOrderSearch")[0], (int)ShipWorks.Data.Model.EntityType.EbayOrderEntity, (int)ShipWorks.Data.Model.EntityType.EbayOrderSearchEntity, 0, null, null, null, null, "EbayOrderSearch", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'EbayStore' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathEbayStoreCollectionViaEbayCombinedOrderRelation
		{
			get
			{
				IEntityRelation intermediateRelation = Relations.EbayCombinedOrderRelationEntityUsingOrderID;
				intermediateRelation.SetAliases(string.Empty, "EbayCombinedOrderRelation_");
				return new PrefetchPathElement2(new EntityCollection<EbayStoreEntity>(EntityFactoryCache2.GetEntityFactory(typeof(EbayStoreEntityFactory))), intermediateRelation,
					(int)ShipWorks.Data.Model.EntityType.EbayOrderEntity, (int)ShipWorks.Data.Model.EntityType.EbayStoreEntity, 0, null, null, GetRelationsForField("EbayStoreCollectionViaEbayCombinedOrderRelation"), null, "EbayStoreCollectionViaEbayCombinedOrderRelation", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToMany);
			}
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

		/// <summary> The EbayOrderID property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."EbayOrderID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Int64 EbayOrderID
		{
			get { return (System.Int64)GetValue((int)EbayOrderFieldIndex.EbayOrderID, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.EbayOrderID, value); }
		}

		/// <summary> The EbayBuyerID property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."EbayBuyerID"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String EbayBuyerID
		{
			get { return (System.String)GetValue((int)EbayOrderFieldIndex.EbayBuyerID, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.EbayBuyerID, value); }
		}

		/// <summary> The CombinedLocally property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."CombinedLocally"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Boolean CombinedLocally
		{
			get { return (System.Boolean)GetValue((int)EbayOrderFieldIndex.CombinedLocally, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.CombinedLocally, value); }
		}

		/// <summary> The SelectedShippingMethod property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."SelectedShippingMethod"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Int32 SelectedShippingMethod
		{
			get { return (System.Int32)GetValue((int)EbayOrderFieldIndex.SelectedShippingMethod, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.SelectedShippingMethod, value); }
		}

		/// <summary> The SellingManagerRecord property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."SellingManagerRecord"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> SellingManagerRecord
		{
			get { return (Nullable<System.Int32>)GetValue((int)EbayOrderFieldIndex.SellingManagerRecord, false); }
			set	{ SetValue((int)EbayOrderFieldIndex.SellingManagerRecord, value); }
		}

		/// <summary> The GspEligible property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."GspEligible"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Boolean GspEligible
		{
			get { return (System.Boolean)GetValue((int)EbayOrderFieldIndex.GspEligible, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.GspEligible, value); }
		}

		/// <summary> The GspFirstName property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."GspFirstName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 128<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String GspFirstName
		{
			get { return (System.String)GetValue((int)EbayOrderFieldIndex.GspFirstName, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.GspFirstName, value); }
		}

		/// <summary> The GspLastName property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."GspLastName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 128<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String GspLastName
		{
			get { return (System.String)GetValue((int)EbayOrderFieldIndex.GspLastName, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.GspLastName, value); }
		}

		/// <summary> The GspStreet1 property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."GspStreet1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 512<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String GspStreet1
		{
			get { return (System.String)GetValue((int)EbayOrderFieldIndex.GspStreet1, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.GspStreet1, value); }
		}

		/// <summary> The GspStreet2 property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."GspStreet2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 512<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String GspStreet2
		{
			get { return (System.String)GetValue((int)EbayOrderFieldIndex.GspStreet2, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.GspStreet2, value); }
		}

		/// <summary> The GspCity property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."GspCity"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 128<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String GspCity
		{
			get { return (System.String)GetValue((int)EbayOrderFieldIndex.GspCity, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.GspCity, value); }
		}

		/// <summary> The GspStateProvince property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."GspStateProvince"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 128<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String GspStateProvince
		{
			get { return (System.String)GetValue((int)EbayOrderFieldIndex.GspStateProvince, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.GspStateProvince, value); }
		}

		/// <summary> The GspPostalCode property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."GspPostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 9<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String GspPostalCode
		{
			get { return (System.String)GetValue((int)EbayOrderFieldIndex.GspPostalCode, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.GspPostalCode, value); }
		}

		/// <summary> The GspCountryCode property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."GspCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String GspCountryCode
		{
			get { return (System.String)GetValue((int)EbayOrderFieldIndex.GspCountryCode, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.GspCountryCode, value); }
		}

		/// <summary> The GspReferenceID property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."GspReferenceID"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 128<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String GspReferenceID
		{
			get { return (System.String)GetValue((int)EbayOrderFieldIndex.GspReferenceID, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.GspReferenceID, value); }
		}

		/// <summary> The RollupEbayItemCount property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."RollupEbayItemCount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Int32 RollupEbayItemCount
		{
			get { return (System.Int32)GetValue((int)EbayOrderFieldIndex.RollupEbayItemCount, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.RollupEbayItemCount, value); }
		}

		/// <summary> The RollupEffectiveCheckoutStatus property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."RollupEffectiveCheckoutStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> RollupEffectiveCheckoutStatus
		{
			get { return (Nullable<System.Int32>)GetValue((int)EbayOrderFieldIndex.RollupEffectiveCheckoutStatus, false); }
			set	{ SetValue((int)EbayOrderFieldIndex.RollupEffectiveCheckoutStatus, value); }
		}

		/// <summary> The RollupEffectivePaymentMethod property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."RollupEffectivePaymentMethod"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> RollupEffectivePaymentMethod
		{
			get { return (Nullable<System.Int32>)GetValue((int)EbayOrderFieldIndex.RollupEffectivePaymentMethod, false); }
			set	{ SetValue((int)EbayOrderFieldIndex.RollupEffectivePaymentMethod, value); }
		}

		/// <summary> The RollupFeedbackLeftType property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."RollupFeedbackLeftType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> RollupFeedbackLeftType
		{
			get { return (Nullable<System.Int32>)GetValue((int)EbayOrderFieldIndex.RollupFeedbackLeftType, false); }
			set	{ SetValue((int)EbayOrderFieldIndex.RollupFeedbackLeftType, value); }
		}

		/// <summary> The RollupFeedbackLeftComments property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."RollupFeedbackLeftComments"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 80<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String RollupFeedbackLeftComments
		{
			get { return (System.String)GetValue((int)EbayOrderFieldIndex.RollupFeedbackLeftComments, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.RollupFeedbackLeftComments, value); }
		}

		/// <summary> The RollupFeedbackReceivedType property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."RollupFeedbackReceivedType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> RollupFeedbackReceivedType
		{
			get { return (Nullable<System.Int32>)GetValue((int)EbayOrderFieldIndex.RollupFeedbackReceivedType, false); }
			set	{ SetValue((int)EbayOrderFieldIndex.RollupFeedbackReceivedType, value); }
		}

		/// <summary> The RollupFeedbackReceivedComments property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."RollupFeedbackReceivedComments"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 80<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String RollupFeedbackReceivedComments
		{
			get { return (System.String)GetValue((int)EbayOrderFieldIndex.RollupFeedbackReceivedComments, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.RollupFeedbackReceivedComments, value); }
		}

		/// <summary> The RollupPayPalAddressStatus property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."RollupPayPalAddressStatus"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> RollupPayPalAddressStatus
		{
			get { return (Nullable<System.Int32>)GetValue((int)EbayOrderFieldIndex.RollupPayPalAddressStatus, false); }
			set	{ SetValue((int)EbayOrderFieldIndex.RollupPayPalAddressStatus, value); }
		}

		/// <summary> The GuaranteedDelivery property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."GuaranteedDelivery"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.Boolean GuaranteedDelivery
		{
			get { return (System.Boolean)GetValue((int)EbayOrderFieldIndex.GuaranteedDelivery, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.GuaranteedDelivery, value); }
		}

		/// <summary> The ExtendedOrderID property of the Entity EbayOrder<br/><br/></summary>
		/// <remarks>Mapped on  table field: "EbayOrder"."ExtendedOrderID"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		[DataMember]
		public virtual System.String ExtendedOrderID
		{
			get { return (System.String)GetValue((int)EbayOrderFieldIndex.ExtendedOrderID, true); }
			set	{ SetValue((int)EbayOrderFieldIndex.ExtendedOrderID, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'EbayCombinedOrderRelationEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(EbayCombinedOrderRelationEntity))]
		[DataMember]
		public virtual EntityCollection<EbayCombinedOrderRelationEntity> EbayCombinedOrderRelation
		{
			get { return GetOrCreateEntityCollection<EbayCombinedOrderRelationEntity, EbayCombinedOrderRelationEntityFactory>("EbayOrder", true, false, ref _ebayCombinedOrderRelation);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'EbayOrderSearchEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(EbayOrderSearchEntity))]
		[DataMember]
		public virtual EntityCollection<EbayOrderSearchEntity> EbayOrderSearch
		{
			get { return GetOrCreateEntityCollection<EbayOrderSearchEntity, EbayOrderSearchEntityFactory>("EbayOrder", true, false, ref _ebayOrderSearch);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'EbayStoreEntity' which are related to this entity via a relation of type 'm:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(EbayStoreEntity))]
		[DataMember]
		public virtual EntityCollection<EbayStoreEntity> EbayStoreCollectionViaEbayCombinedOrderRelation
		{
			get { return GetOrCreateEntityCollection<EbayStoreEntity, EbayStoreEntityFactory>("EbayOrderCollectionViaEbayCombinedOrderRelation", false, true, ref _ebayStoreCollectionViaEbayCombinedOrderRelation);	}
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
			get { return (int)ShipWorks.Data.Model.EntityType.EbayOrderEntity; }
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
