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
	/// <summary>Entity class which represents the entity 'YahooStore'.<br/><br/></summary>
	[Serializable]
	public partial class YahooStoreEntity : StoreEntity
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EmailAccountEntity _yahooEmailAccount;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static new partial class MemberNames
		{
			/// <summary>Member name YahooEmailAccount</summary>
			public static readonly string YahooEmailAccount = "YahooEmailAccount";
			/// <summary>Member name AmazonOrderSearch</summary>
			public static readonly string AmazonOrderSearch = "AmazonOrderSearch";
			/// <summary>Member name ChannelAdvisorOrderSearch</summary>
			public static readonly string ChannelAdvisorOrderSearch = "ChannelAdvisorOrderSearch";
			/// <summary>Member name ClickCartProOrderSearch</summary>
			public static readonly string ClickCartProOrderSearch = "ClickCartProOrderSearch";
			/// <summary>Member name CommerceInterfaceOrderSearch</summary>
			public static readonly string CommerceInterfaceOrderSearch = "CommerceInterfaceOrderSearch";
			/// <summary>Member name EbayOrderSearch</summary>
			public static readonly string EbayOrderSearch = "EbayOrderSearch";
			/// <summary>Member name EtsyOrderSearch</summary>
			public static readonly string EtsyOrderSearch = "EtsyOrderSearch";
			/// <summary>Member name GrouponOrderSearch</summary>
			public static readonly string GrouponOrderSearch = "GrouponOrderSearch";
			/// <summary>Member name LemonStandOrderSearch</summary>
			public static readonly string LemonStandOrderSearch = "LemonStandOrderSearch";
			/// <summary>Member name MagentoOrderSearch</summary>
			public static readonly string MagentoOrderSearch = "MagentoOrderSearch";
			/// <summary>Member name MarketplaceAdvisorOrderSearch</summary>
			public static readonly string MarketplaceAdvisorOrderSearch = "MarketplaceAdvisorOrderSearch";
			/// <summary>Member name NetworkSolutionsOrderSearch</summary>
			public static readonly string NetworkSolutionsOrderSearch = "NetworkSolutionsOrderSearch";
			/// <summary>Member name NeweggOrderSearch</summary>
			public static readonly string NeweggOrderSearch = "NeweggOrderSearch";
			/// <summary>Member name OrderMotionOrderSearch</summary>
			public static readonly string OrderMotionOrderSearch = "OrderMotionOrderSearch";
			/// <summary>Member name OrderSearch</summary>
			public static readonly string OrderSearch = "OrderSearch";
			/// <summary>Member name PayPalOrderSearch</summary>
			public static readonly string PayPalOrderSearch = "PayPalOrderSearch";
			/// <summary>Member name ProStoresOrderSearch</summary>
			public static readonly string ProStoresOrderSearch = "ProStoresOrderSearch";
			/// <summary>Member name SearsOrderSearch</summary>
			public static readonly string SearsOrderSearch = "SearsOrderSearch";
			/// <summary>Member name ShopifyOrderSearch</summary>
			public static readonly string ShopifyOrderSearch = "ShopifyOrderSearch";
			/// <summary>Member name ThreeDCartOrderSearch</summary>
			public static readonly string ThreeDCartOrderSearch = "ThreeDCartOrderSearch";
			/// <summary>Member name WalmartOrderSearch</summary>
			public static readonly string WalmartOrderSearch = "WalmartOrderSearch";
			/// <summary>Member name YahooOrderSearch</summary>
			public static readonly string YahooOrderSearch = "YahooOrderSearch";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static YahooStoreEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public YahooStoreEntity()
		{
			InitClassEmpty();
			SetName("YahooStoreEntity");
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public YahooStoreEntity(IEntityFields2 fields):base(fields)
		{
			InitClassEmpty();
			SetName("YahooStoreEntity");
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this YahooStoreEntity</param>
		public YahooStoreEntity(IValidator validator):base(validator)
		{
			InitClassEmpty();
			SetName("YahooStoreEntity");
		}
				
		/// <summary> CTor</summary>
		/// <param name="storeID">PK value for YahooStore which data should be fetched into this YahooStore object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public YahooStoreEntity(System.Int64 storeID):base(storeID)
		{
			InitClassEmpty();

			SetName("YahooStoreEntity");
		}

		/// <summary> CTor</summary>
		/// <param name="storeID">PK value for YahooStore which data should be fetched into this YahooStore object</param>
		/// <param name="validator">The custom validator object for this YahooStoreEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public YahooStoreEntity(System.Int64 storeID, IValidator validator):base(storeID, validator)
		{
			InitClassEmpty();

			SetName("YahooStoreEntity");
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected YahooStoreEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_yahooEmailAccount = (EmailAccountEntity)info.GetValue("_yahooEmailAccount", typeof(EmailAccountEntity));
				if(_yahooEmailAccount!=null)
				{
					_yahooEmailAccount.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((YahooStoreFieldIndex)fieldIndex)
			{
				case YahooStoreFieldIndex.YahooEmailAccountID:
					DesetupSyncYahooEmailAccount(true, false);
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
				case "YahooEmailAccount":
					this.YahooEmailAccount = (EmailAccountEntity)entity;
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
				case "YahooEmailAccount":
					toReturn.Add(Relations.EmailAccountEntityUsingYahooEmailAccountID);
					break;
				default:
					toReturn = StoreEntity.GetRelationsForField(fieldName);
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
			int numberOfOneWayRelations = 0+1;
			switch(propertyName)
			{
				case null:
					return ((numberOfOneWayRelations > 0) || base.CheckOneWayRelations(null));
				case "YahooEmailAccount":
					return true;
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
				case "YahooEmailAccount":
					SetupSyncYahooEmailAccount(relatedEntity);
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
				case "YahooEmailAccount":
					DesetupSyncYahooEmailAccount(false, true);
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
			if(_yahooEmailAccount!=null)
			{
				toReturn.Add(_yahooEmailAccount);
			}
			toReturn.AddRange(base.GetDependentRelatedEntities());
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.AddRange(base.GetMemberEntityCollections());
			return toReturn;
		}

		/// <summary>Gets a predicateexpression which filters on this entity</summary>
		/// <returns>ready to use predicateexpression</returns>
		/// <remarks>Only useful in entity fetches.</remarks>
		public new static IPredicateExpression GetEntityTypeFilter()
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetEntityTypeFilter("YahooStoreEntity", false);
		}
		
		/// <summary>Gets a predicateexpression which filters on this entity</summary>
		/// <param name="negate">Flag to produce a NOT filter, (true), or a normal filter (false). </param>
		/// <returns>ready to use predicateexpression</returns>
		/// <remarks>Only useful in entity fetches.</remarks>
		public new static IPredicateExpression GetEntityTypeFilter(bool negate)
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetEntityTypeFilter("YahooStoreEntity", negate);
		}

		/// <summary>ISerializable member. Does custom serialization so event handlers do not get serialized. Serializes members of this entity class and uses the base class' implementation to serialize the rest.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				info.AddValue("_yahooEmailAccount", (!this.MarkedForDeletion?_yahooEmailAccount:null));
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
			return InheritanceInfoProviderSingleton.GetInstance().CheckIfIsSubTypeOf("YahooStoreEntity", ((ShipWorks.Data.Model.EntityType)typeOfEntity).ToString());
		}
				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new YahooStoreRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'EmailAccount' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoYahooEmailAccount()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(EmailAccountFields.EmailAccountID, null, ComparisonOperator.Equal, this.YahooEmailAccountID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(YahooStoreEntityFactory));
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
			Dictionary<string, object> toReturn = base.GetRelatedData();
			toReturn.Add("YahooEmailAccount", _yahooEmailAccount);
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
			_fieldsCustomProperties.Add("YahooEmailAccountID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("TrackingUpdatePassword", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("YahooStoreID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AccessToken", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BackupOrderNumber", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _yahooEmailAccount</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncYahooEmailAccount(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _yahooEmailAccount, new PropertyChangedEventHandler( OnYahooEmailAccountPropertyChanged ), "YahooEmailAccount", ShipWorks.Data.Model.RelationClasses.StaticYahooStoreRelations.EmailAccountEntityUsingYahooEmailAccountIDStatic, true, signalRelatedEntity, "", resetFKFields, new int[] { (int)YahooStoreFieldIndex.YahooEmailAccountID } );
			_yahooEmailAccount = null;
		}

		/// <summary> setups the sync logic for member _yahooEmailAccount</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncYahooEmailAccount(IEntityCore relatedEntity)
		{
			if(_yahooEmailAccount!=relatedEntity)
			{
				DesetupSyncYahooEmailAccount(true, true);
				_yahooEmailAccount = (EmailAccountEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _yahooEmailAccount, new PropertyChangedEventHandler( OnYahooEmailAccountPropertyChanged ), "YahooEmailAccount", ShipWorks.Data.Model.RelationClasses.StaticYahooStoreRelations.EmailAccountEntityUsingYahooEmailAccountIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnYahooEmailAccountPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this YahooStoreEntity</param>
		private void InitClassEmpty()
		{
			InitClassMembers();

			// __LLBLGENPRO_USER_CODE_REGION_START InitClassEmpty
			// __LLBLGENPRO_USER_CODE_REGION_END


		}

		#region Class Property Declarations
		/// <summary> The relations object holding all relations of this entity with other entity classes.</summary>
		public new static YahooStoreRelations Relations
		{
			get	{ return new YahooStoreRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public new static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'EmailAccount' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathYahooEmailAccount
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(EmailAccountEntityFactory))),	(IEntityRelation)GetRelationsForField("YahooEmailAccount")[0], (int)ShipWorks.Data.Model.EntityType.YahooStoreEntity, (int)ShipWorks.Data.Model.EntityType.EmailAccountEntity, 0, null, null, null, null, "YahooEmailAccount", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The YahooEmailAccountID property of the Entity YahooStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "YahooStore"."YahooEmailAccountID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 YahooEmailAccountID
		{
			get { return (System.Int64)GetValue((int)YahooStoreFieldIndex.YahooEmailAccountID, true); }
			set	{ SetValue((int)YahooStoreFieldIndex.YahooEmailAccountID, value); }
		}

		/// <summary> The TrackingUpdatePassword property of the Entity YahooStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "YahooStore"."TrackingUpdatePassword"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String TrackingUpdatePassword
		{
			get { return (System.String)GetValue((int)YahooStoreFieldIndex.TrackingUpdatePassword, true); }
			set	{ SetValue((int)YahooStoreFieldIndex.TrackingUpdatePassword, value); }
		}

		/// <summary> The YahooStoreID property of the Entity YahooStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "YahooStore"."YahooStoreID"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String YahooStoreID
		{
			get { return (System.String)GetValue((int)YahooStoreFieldIndex.YahooStoreID, true); }
			set	{ SetValue((int)YahooStoreFieldIndex.YahooStoreID, value); }
		}

		/// <summary> The AccessToken property of the Entity YahooStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "YahooStore"."AccessToken"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 200<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String AccessToken
		{
			get { return (System.String)GetValue((int)YahooStoreFieldIndex.AccessToken, true); }
			set	{ SetValue((int)YahooStoreFieldIndex.AccessToken, value); }
		}

		/// <summary> The BackupOrderNumber property of the Entity YahooStore<br/><br/></summary>
		/// <remarks>Mapped on  table field: "YahooStore"."BackupOrderNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> BackupOrderNumber
		{
			get { return (Nullable<System.Int64>)GetValue((int)YahooStoreFieldIndex.BackupOrderNumber, false); }
			set	{ SetValue((int)YahooStoreFieldIndex.BackupOrderNumber, value); }
		}

		/// <summary> Gets / sets related entity of type 'EmailAccountEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual EmailAccountEntity YahooEmailAccount
		{
			get	{ return _yahooEmailAccount; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncYahooEmailAccount(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "", "YahooEmailAccount", _yahooEmailAccount, false); 
				}
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
		protected override int LLBLGenProEntityTypeValue 
		{ 
			get { return (int)ShipWorks.Data.Model.EntityType.YahooStoreEntity; }
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
