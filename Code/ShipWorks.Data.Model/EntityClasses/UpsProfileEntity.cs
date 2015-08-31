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
	/// Entity class which represents the entity 'UpsProfile'.<br/><br/>
	/// 
	/// </summary>
	[Serializable]
	public partial class UpsProfileEntity : CommonEntityBase, ISerializable
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<UpsProfilePackageEntity> _packages;


		private ShippingProfileEntity _shippingProfile;
		
		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{

			/// <summary>Member name Packages</summary>
			public static readonly string Packages = "Packages";

			/// <summary>Member name ShippingProfile</summary>
			public static readonly string ShippingProfile = "ShippingProfile";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static UpsProfileEntity()
		{
			SetupCustomPropertyHashtables();
		}

		/// <summary> CTor</summary>
		public UpsProfileEntity():base("UpsProfileEntity")
		{
			InitClassEmpty(null, CreateFields());
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public UpsProfileEntity(IEntityFields2 fields):base("UpsProfileEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this UpsProfileEntity</param>
		public UpsProfileEntity(IValidator validator):base("UpsProfileEntity")
		{
			InitClassEmpty(validator, CreateFields());
		}
				

		/// <summary> CTor</summary>
		/// <param name="shippingProfileID">PK value for UpsProfile which data should be fetched into this UpsProfile object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public UpsProfileEntity(System.Int64 shippingProfileID):base("UpsProfileEntity")
		{
			InitClassEmpty(null, CreateFields());
			this.ShippingProfileID = shippingProfileID;
		}

		/// <summary> CTor</summary>
		/// <param name="shippingProfileID">PK value for UpsProfile which data should be fetched into this UpsProfile object</param>
		/// <param name="validator">The custom validator object for this UpsProfileEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public UpsProfileEntity(System.Int64 shippingProfileID, IValidator validator):base("UpsProfileEntity")
		{
			InitClassEmpty(validator, CreateFields());
			this.ShippingProfileID = shippingProfileID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected UpsProfileEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_packages = (EntityCollection<UpsProfilePackageEntity>)info.GetValue("_packages", typeof(EntityCollection<UpsProfilePackageEntity>));


				_shippingProfile = (ShippingProfileEntity)info.GetValue("_shippingProfile", typeof(ShippingProfileEntity));
				if(_shippingProfile!=null)
				{
					_shippingProfile.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				base.FixupDeserialization(FieldInfoProviderSingleton.GetInstance());
			}
			
			// __LLBLGENPRO_USER_CODE_REGION_START DeserializationConstructor
			// __LLBLGENPRO_USER_CODE_REGION_END
		}

		
		/// <summary>Performs the desync setup when an FK field has been changed. The entity referenced based on the FK field will be dereferenced and sync info will be removed.</summary>
		/// <param name="fieldIndex">The fieldindex.</param>
		protected override void PerformDesyncSetupFKFieldChange(int fieldIndex)
		{
			switch((UpsProfileFieldIndex)fieldIndex)
			{
				case UpsProfileFieldIndex.ShippingProfileID:
					DesetupSyncShippingProfile(true, false);
					break;
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

				case "Packages":
					this.Packages.Add((UpsProfilePackageEntity)entity);
					break;

				case "ShippingProfile":
					this.ShippingProfile = (ShippingProfileEntity)entity;
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
			return UpsProfileEntity.GetRelationsForField(fieldName);
		}

		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		public static RelationCollection GetRelationsForField(string fieldName)
		{
			RelationCollection toReturn = new RelationCollection();
			switch(fieldName)
			{

				case "Packages":
					toReturn.Add(UpsProfileEntity.Relations.UpsProfilePackageEntityUsingShippingProfileID);
					break;

				case "ShippingProfile":
					toReturn.Add(UpsProfileEntity.Relations.ShippingProfileEntityUsingShippingProfileID);
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

				case "Packages":
					this.Packages.Add((UpsProfilePackageEntity)relatedEntity);
					break;
				case "ShippingProfile":
					SetupSyncShippingProfile(relatedEntity);
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

				case "Packages":
					base.PerformRelatedEntityRemoval(this.Packages, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "ShippingProfile":
					DesetupSyncShippingProfile(false, true);
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

			if(_shippingProfile!=null)
			{
				toReturn.Add(_shippingProfile);
			}

			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. The contents of the ArrayList is used by the DataAccessAdapter to perform recursive saves. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		public override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.Add(this.Packages);

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
				info.AddValue("_packages", ((_packages!=null) && (_packages.Count>0) && !this.MarkedForDeletion)?_packages:null);


				info.AddValue("_shippingProfile", (!this.MarkedForDeletion?_shippingProfile:null));
			}
			
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}

		/// <summary>Returns true if the original value for the field with the fieldIndex passed in, read from the persistent storage was NULL, false otherwise.
		/// Should not be used for testing if the current value is NULL, use <see cref="TestCurrentFieldValueForNull"/> for that.</summary>
		/// <param name="fieldIndex">Index of the field to test if that field was NULL in the persistent storage</param>
		/// <returns>true if the field with the passed in index was NULL in the persistent storage, false otherwise</returns>
		public bool TestOriginalFieldValueForNull(UpsProfileFieldIndex fieldIndex)
		{
			return base.Fields[(int)fieldIndex].IsNull;
		}
		
		/// <summary>Returns true if the current value for the field with the fieldIndex passed in represents null/not defined, false otherwise.
		/// Should not be used for testing if the original value (read from the db) is NULL</summary>
		/// <param name="fieldIndex">Index of the field to test if its currentvalue is null/undefined</param>
		/// <returns>true if the field's value isn't defined yet, false otherwise</returns>
		public bool TestCurrentFieldValueForNull(UpsProfileFieldIndex fieldIndex)
		{
			return base.CheckIfCurrentFieldValueIsNull((int)fieldIndex);
		}

				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			return new UpsProfileRelations().GetAllRelations();
		}
		

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entities of type 'UpsProfilePackage' to this entity. Use DataAccessAdapter.FetchEntityCollection() to fetch these related entities.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoPackages()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UpsProfilePackageFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}



		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'ShippingProfile' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoShippingProfile()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ShippingProfileFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}
	
		
		/// <summary>Creates entity fields object for this entity. Used in constructor to setup this entity in a polymorphic scenario.</summary>
		protected virtual IEntityFields2 CreateFields()
		{
			return EntityFieldsFactory.CreateEntityFieldsObject(ShipWorks.Data.Model.EntityType.UpsProfileEntity);
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
			return EntityFactoryCache2.GetEntityFactory(typeof(UpsProfileEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._packages);

		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._packages = (EntityCollection<UpsProfilePackageEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			if (this._packages != null)
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
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<UpsProfilePackageEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsProfilePackageEntityFactory))) : null);

		}
#endif
		/// <summary>
		/// Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element. 
		/// </summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		public override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();

			toReturn.Add("Packages", _packages);

			toReturn.Add("ShippingProfile", _shippingProfile);
			return toReturn;
		}
		
		/// <summary> Adds the internals to the active context. </summary>
		protected override void AddInternalsToContext()
		{
			if(_packages!=null)
			{
				_packages.ActiveContext = base.ActiveContext;
			}


			if(_shippingProfile!=null)
			{
				_shippingProfile.ActiveContext = base.ActiveContext;
			}
		}

		/// <summary> Initializes the class members</summary>
		protected virtual void InitClassMembers()
		{

			_packages = null;


			_shippingProfile = null;
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

			_fieldsCustomProperties.Add("ShippingProfileID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("UpsAccountID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Service", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("SaturdayDelivery", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ResidentialDetermination", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DeliveryConfirmation", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ReferenceNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ReferenceNumber2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PayorType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PayorAccount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PayorPostalCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PayorCountryCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EmailNotifySender", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EmailNotifyRecipient", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EmailNotifyOther", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EmailNotifyOtherAddress", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EmailNotifyFrom", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EmailNotifySubject", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EmailNotifyMessage", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ReturnService", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ReturnUndeliverableEmail", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ReturnContents", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Endorsement", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Subclassification", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PaperlessAdditionalDocumentation", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ShipperRelease", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CarbonNeutral", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CommercialPaperlessInvoice", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CostCenter", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("IrregularIndicator", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Cn22Number", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ShipmentChargeType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ShipmentChargeAccount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ShipmentChargePostalCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ShipmentChargeCountryCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("UspsPackageID", fieldHashtable);
		}
		#endregion


		/// <summary> Removes the sync logic for member _shippingProfile</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncShippingProfile(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _shippingProfile, new PropertyChangedEventHandler( OnShippingProfilePropertyChanged ), "ShippingProfile", UpsProfileEntity.Relations.ShippingProfileEntityUsingShippingProfileID, true, signalRelatedEntity, "Ups", false, new int[] { (int)UpsProfileFieldIndex.ShippingProfileID } );
			_shippingProfile = null;
		}
		
		/// <summary> setups the sync logic for member _shippingProfile</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncShippingProfile(IEntity2 relatedEntity)
		{
			if(_shippingProfile!=relatedEntity)
			{
				DesetupSyncShippingProfile(true, true);
				_shippingProfile = (ShippingProfileEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _shippingProfile, new PropertyChangedEventHandler( OnShippingProfilePropertyChanged ), "ShippingProfile", UpsProfileEntity.Relations.ShippingProfileEntityUsingShippingProfileID, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnShippingProfilePropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this UpsProfileEntity</param>
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
		public  static UpsProfileRelations Relations
		{
			get	{ return new UpsProfileRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UpsProfilePackage' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathPackages
		{
			get
			{
				return new PrefetchPathElement2( new EntityCollection<UpsProfilePackageEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsProfilePackageEntityFactory))),
					(IEntityRelation)GetRelationsForField("Packages")[0], (int)ShipWorks.Data.Model.EntityType.UpsProfileEntity, (int)ShipWorks.Data.Model.EntityType.UpsProfilePackageEntity, 0, null, null, null, null, "Packages", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);
			}
		}



		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ShippingProfile' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathShippingProfile
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(ShippingProfileEntityFactory))),
					(IEntityRelation)GetRelationsForField("ShippingProfile")[0], (int)ShipWorks.Data.Model.EntityType.UpsProfileEntity, (int)ShipWorks.Data.Model.EntityType.ShippingProfileEntity, 0, null, null, null, null, "ShippingProfile", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);
			}
		}

		/// <summary> The custom properties for the type of this entity instance.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		public override Dictionary<string, string> CustomPropertiesOfType
		{
			get { return UpsProfileEntity.CustomProperties;}
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
			get { return UpsProfileEntity.FieldsCustomProperties;}
		}

		/// <summary> The ShippingProfileID property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."ShippingProfileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
		public virtual System.Int64 ShippingProfileID
		{
			get { return (System.Int64)GetValue((int)UpsProfileFieldIndex.ShippingProfileID, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.ShippingProfileID, value); }
		}

		/// <summary> The UpsAccountID property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."UpsAccountID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> UpsAccountID
		{
			get { return (Nullable<System.Int64>)GetValue((int)UpsProfileFieldIndex.UpsAccountID, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.UpsAccountID, value); }
		}

		/// <summary> The Service property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."Service"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> Service
		{
			get { return (Nullable<System.Int32>)GetValue((int)UpsProfileFieldIndex.Service, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.Service, value); }
		}

		/// <summary> The SaturdayDelivery property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."SaturdayDelivery"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> SaturdayDelivery
		{
			get { return (Nullable<System.Boolean>)GetValue((int)UpsProfileFieldIndex.SaturdayDelivery, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.SaturdayDelivery, value); }
		}

		/// <summary> The ResidentialDetermination property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."ResidentialDetermination"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> ResidentialDetermination
		{
			get { return (Nullable<System.Int32>)GetValue((int)UpsProfileFieldIndex.ResidentialDetermination, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.ResidentialDetermination, value); }
		}

		/// <summary> The DeliveryConfirmation property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."DeliveryConfirmation"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> DeliveryConfirmation
		{
			get { return (Nullable<System.Int32>)GetValue((int)UpsProfileFieldIndex.DeliveryConfirmation, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.DeliveryConfirmation, value); }
		}

		/// <summary> The ReferenceNumber property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."ReferenceNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String ReferenceNumber
		{
			get { return (System.String)GetValue((int)UpsProfileFieldIndex.ReferenceNumber, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.ReferenceNumber, value); }
		}

		/// <summary> The ReferenceNumber2 property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."ReferenceNumber2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String ReferenceNumber2
		{
			get { return (System.String)GetValue((int)UpsProfileFieldIndex.ReferenceNumber2, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.ReferenceNumber2, value); }
		}

		/// <summary> The PayorType property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."PayorType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> PayorType
		{
			get { return (Nullable<System.Int32>)GetValue((int)UpsProfileFieldIndex.PayorType, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.PayorType, value); }
		}

		/// <summary> The PayorAccount property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."PayorAccount"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String PayorAccount
		{
			get { return (System.String)GetValue((int)UpsProfileFieldIndex.PayorAccount, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.PayorAccount, value); }
		}

		/// <summary> The PayorPostalCode property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."PayorPostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String PayorPostalCode
		{
			get { return (System.String)GetValue((int)UpsProfileFieldIndex.PayorPostalCode, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.PayorPostalCode, value); }
		}

		/// <summary> The PayorCountryCode property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."PayorCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String PayorCountryCode
		{
			get { return (System.String)GetValue((int)UpsProfileFieldIndex.PayorCountryCode, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.PayorCountryCode, value); }
		}

		/// <summary> The EmailNotifySender property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."EmailNotifySender"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> EmailNotifySender
		{
			get { return (Nullable<System.Int32>)GetValue((int)UpsProfileFieldIndex.EmailNotifySender, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.EmailNotifySender, value); }
		}

		/// <summary> The EmailNotifyRecipient property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."EmailNotifyRecipient"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> EmailNotifyRecipient
		{
			get { return (Nullable<System.Int32>)GetValue((int)UpsProfileFieldIndex.EmailNotifyRecipient, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.EmailNotifyRecipient, value); }
		}

		/// <summary> The EmailNotifyOther property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."EmailNotifyOther"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> EmailNotifyOther
		{
			get { return (Nullable<System.Int32>)GetValue((int)UpsProfileFieldIndex.EmailNotifyOther, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.EmailNotifyOther, value); }
		}

		/// <summary> The EmailNotifyOtherAddress property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."EmailNotifyOtherAddress"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String EmailNotifyOtherAddress
		{
			get { return (System.String)GetValue((int)UpsProfileFieldIndex.EmailNotifyOtherAddress, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.EmailNotifyOtherAddress, value); }
		}

		/// <summary> The EmailNotifyFrom property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."EmailNotifyFrom"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String EmailNotifyFrom
		{
			get { return (System.String)GetValue((int)UpsProfileFieldIndex.EmailNotifyFrom, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.EmailNotifyFrom, value); }
		}

		/// <summary> The EmailNotifySubject property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."EmailNotifySubject"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> EmailNotifySubject
		{
			get { return (Nullable<System.Int32>)GetValue((int)UpsProfileFieldIndex.EmailNotifySubject, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.EmailNotifySubject, value); }
		}

		/// <summary> The EmailNotifyMessage property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."EmailNotifyMessage"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 120<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String EmailNotifyMessage
		{
			get { return (System.String)GetValue((int)UpsProfileFieldIndex.EmailNotifyMessage, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.EmailNotifyMessage, value); }
		}

		/// <summary> The ReturnService property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."ReturnService"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> ReturnService
		{
			get { return (Nullable<System.Int32>)GetValue((int)UpsProfileFieldIndex.ReturnService, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.ReturnService, value); }
		}

		/// <summary> The ReturnUndeliverableEmail property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."ReturnUndeliverableEmail"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String ReturnUndeliverableEmail
		{
			get { return (System.String)GetValue((int)UpsProfileFieldIndex.ReturnUndeliverableEmail, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.ReturnUndeliverableEmail, value); }
		}

		/// <summary> The ReturnContents property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."ReturnContents"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String ReturnContents
		{
			get { return (System.String)GetValue((int)UpsProfileFieldIndex.ReturnContents, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.ReturnContents, value); }
		}

		/// <summary> The Endorsement property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."Endorsement"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> Endorsement
		{
			get { return (Nullable<System.Int32>)GetValue((int)UpsProfileFieldIndex.Endorsement, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.Endorsement, value); }
		}

		/// <summary> The Subclassification property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."Subclassification"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> Subclassification
		{
			get { return (Nullable<System.Int32>)GetValue((int)UpsProfileFieldIndex.Subclassification, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.Subclassification, value); }
		}

		/// <summary> The PaperlessAdditionalDocumentation property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."PaperlessAdditionalDocumentation"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> PaperlessAdditionalDocumentation
		{
			get { return (Nullable<System.Boolean>)GetValue((int)UpsProfileFieldIndex.PaperlessAdditionalDocumentation, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.PaperlessAdditionalDocumentation, value); }
		}

		/// <summary> The ShipperRelease property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."ShipperRelease"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> ShipperRelease
		{
			get { return (Nullable<System.Boolean>)GetValue((int)UpsProfileFieldIndex.ShipperRelease, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.ShipperRelease, value); }
		}

		/// <summary> The CarbonNeutral property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."CarbonNeutral"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> CarbonNeutral
		{
			get { return (Nullable<System.Boolean>)GetValue((int)UpsProfileFieldIndex.CarbonNeutral, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.CarbonNeutral, value); }
		}

		/// <summary> The CommercialPaperlessInvoice property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."CommercialPaperlessInvoice"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> CommercialPaperlessInvoice
		{
			get { return (Nullable<System.Boolean>)GetValue((int)UpsProfileFieldIndex.CommercialPaperlessInvoice, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.CommercialPaperlessInvoice, value); }
		}

		/// <summary> The CostCenter property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."CostCenter"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String CostCenter
		{
			get { return (System.String)GetValue((int)UpsProfileFieldIndex.CostCenter, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.CostCenter, value); }
		}

		/// <summary> The IrregularIndicator property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."IrregularIndicator"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> IrregularIndicator
		{
			get { return (Nullable<System.Int32>)GetValue((int)UpsProfileFieldIndex.IrregularIndicator, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.IrregularIndicator, value); }
		}

		/// <summary> The Cn22Number property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."Cn22Number"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Cn22Number
		{
			get { return (System.String)GetValue((int)UpsProfileFieldIndex.Cn22Number, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.Cn22Number, value); }
		}

		/// <summary> The ShipmentChargeType property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."ShipmentChargeType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> ShipmentChargeType
		{
			get { return (Nullable<System.Int32>)GetValue((int)UpsProfileFieldIndex.ShipmentChargeType, false); }
			set	{ SetValue((int)UpsProfileFieldIndex.ShipmentChargeType, value); }
		}

		/// <summary> The ShipmentChargeAccount property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."ShipmentChargeAccount"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String ShipmentChargeAccount
		{
			get { return (System.String)GetValue((int)UpsProfileFieldIndex.ShipmentChargeAccount, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.ShipmentChargeAccount, value); }
		}

		/// <summary> The ShipmentChargePostalCode property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."ShipmentChargePostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String ShipmentChargePostalCode
		{
			get { return (System.String)GetValue((int)UpsProfileFieldIndex.ShipmentChargePostalCode, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.ShipmentChargePostalCode, value); }
		}

		/// <summary> The ShipmentChargeCountryCode property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."ShipmentChargeCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String ShipmentChargeCountryCode
		{
			get { return (System.String)GetValue((int)UpsProfileFieldIndex.ShipmentChargeCountryCode, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.ShipmentChargeCountryCode, value); }
		}

		/// <summary> The UspsPackageID property of the Entity UpsProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsProfile"."UspsPackageID"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String UspsPackageID
		{
			get { return (System.String)GetValue((int)UpsProfileFieldIndex.UspsPackageID, true); }
			set	{ SetValue((int)UpsProfileFieldIndex.UspsPackageID, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'UpsProfilePackageEntity' which are related to this entity via a relation of type '1:n'.
		/// If the EntityCollection hasn't been fetched yet, the collection returned will be empty.</summary>
		[TypeContainedAttribute(typeof(UpsProfilePackageEntity))]
		public virtual EntityCollection<UpsProfilePackageEntity> Packages
		{
			get
			{
				if(_packages==null)
				{
					_packages = new EntityCollection<UpsProfilePackageEntity>(EntityFactoryCache2.GetEntityFactory(typeof(UpsProfilePackageEntityFactory)));
					_packages.SetContainingEntityInfo(this, "UpsProfile");
				}
				return _packages;
			}
		}



		/// <summary> Gets / sets related entity of type 'ShippingProfileEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual ShippingProfileEntity ShippingProfile
		{
			get
			{
				return _shippingProfile;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncShippingProfile(value);
					if((SerializationHelper.Optimization == SerializationOptimization.Fast) && (value!=null))
					{
						value.SetRelatedEntity(this, "Ups");
					}
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_shippingProfile !=null);
						DesetupSyncShippingProfile(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("ShippingProfile");
						}
					}
					else
					{
						if(_shippingProfile!=value)
						{
							IEntity2 relatedEntity = (IEntity2)value;
							relatedEntity.SetRelatedEntity(this, "Ups");
							SetupSyncShippingProfile(relatedEntity);
						}
					}
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
		public override int LLBLGenProEntityTypeValue 
		{ 
			get { return (int)ShipWorks.Data.Model.EntityType.UpsProfileEntity; }
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
