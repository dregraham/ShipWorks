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
	/// Entity class which represents the entity 'AmazonShipment'.<br/><br/>
	/// 
	/// </summary>
	[Serializable]
	public partial class AmazonShipmentEntity : CommonEntityBase, ISerializable
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations


		private AmazonAccountEntity _amazonAccount;
		private ShipmentEntity _shipment;
		
		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name AmazonAccount</summary>
			public static readonly string AmazonAccount = "AmazonAccount";


			/// <summary>Member name Shipment</summary>
			public static readonly string Shipment = "Shipment";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static AmazonShipmentEntity()
		{
			SetupCustomPropertyHashtables();
		}

		/// <summary> CTor</summary>
		public AmazonShipmentEntity():base("AmazonShipmentEntity")
		{
			InitClassEmpty(null, CreateFields());
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public AmazonShipmentEntity(IEntityFields2 fields):base("AmazonShipmentEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this AmazonShipmentEntity</param>
		public AmazonShipmentEntity(IValidator validator):base("AmazonShipmentEntity")
		{
			InitClassEmpty(validator, CreateFields());
		}
				

		/// <summary> CTor</summary>
		/// <param name="shipmentID">PK value for AmazonShipment which data should be fetched into this AmazonShipment object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public AmazonShipmentEntity(System.Int64 shipmentID):base("AmazonShipmentEntity")
		{
			InitClassEmpty(null, CreateFields());
			this.ShipmentID = shipmentID;
		}

		/// <summary> CTor</summary>
		/// <param name="shipmentID">PK value for AmazonShipment which data should be fetched into this AmazonShipment object</param>
		/// <param name="validator">The custom validator object for this AmazonShipmentEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public AmazonShipmentEntity(System.Int64 shipmentID, IValidator validator):base("AmazonShipmentEntity")
		{
			InitClassEmpty(validator, CreateFields());
			this.ShipmentID = shipmentID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected AmazonShipmentEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{


				_amazonAccount = (AmazonAccountEntity)info.GetValue("_amazonAccount", typeof(AmazonAccountEntity));
				if(_amazonAccount!=null)
				{
					_amazonAccount.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_shipment = (ShipmentEntity)info.GetValue("_shipment", typeof(ShipmentEntity));
				if(_shipment!=null)
				{
					_shipment.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((AmazonShipmentFieldIndex)fieldIndex)
			{
				case AmazonShipmentFieldIndex.ShipmentID:
					DesetupSyncShipment(true, false);
					break;
				case AmazonShipmentFieldIndex.AmazonAccountID:
					DesetupSyncAmazonAccount(true, false);
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
				case "AmazonAccount":
					this.AmazonAccount = (AmazonAccountEntity)entity;
					break;


				case "Shipment":
					this.Shipment = (ShipmentEntity)entity;
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
			return AmazonShipmentEntity.GetRelationsForField(fieldName);
		}

		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		public static RelationCollection GetRelationsForField(string fieldName)
		{
			RelationCollection toReturn = new RelationCollection();
			switch(fieldName)
			{
				case "AmazonAccount":
					toReturn.Add(AmazonShipmentEntity.Relations.AmazonAccountEntityUsingAmazonAccountID);
					break;


				case "Shipment":
					toReturn.Add(AmazonShipmentEntity.Relations.ShipmentEntityUsingShipmentID);
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
				case "AmazonAccount":
					SetupSyncAmazonAccount(relatedEntity);
					break;

				case "Shipment":
					SetupSyncShipment(relatedEntity);
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
				case "AmazonAccount":
					DesetupSyncAmazonAccount(false, true);
					break;

				case "Shipment":
					DesetupSyncShipment(false, true);
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
			if(_amazonAccount!=null)
			{
				toReturn.Add(_amazonAccount);
			}
			if(_shipment!=null)
			{
				toReturn.Add(_shipment);
			}

			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. The contents of the ArrayList is used by the DataAccessAdapter to perform recursive saves. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		public override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();


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


				info.AddValue("_amazonAccount", (!this.MarkedForDeletion?_amazonAccount:null));
				info.AddValue("_shipment", (!this.MarkedForDeletion?_shipment:null));
			}
			
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}

		/// <summary>Returns true if the original value for the field with the fieldIndex passed in, read from the persistent storage was NULL, false otherwise.
		/// Should not be used for testing if the current value is NULL, use <see cref="TestCurrentFieldValueForNull"/> for that.</summary>
		/// <param name="fieldIndex">Index of the field to test if that field was NULL in the persistent storage</param>
		/// <returns>true if the field with the passed in index was NULL in the persistent storage, false otherwise</returns>
		public bool TestOriginalFieldValueForNull(AmazonShipmentFieldIndex fieldIndex)
		{
			return base.Fields[(int)fieldIndex].IsNull;
		}
		
		/// <summary>Returns true if the current value for the field with the fieldIndex passed in represents null/not defined, false otherwise.
		/// Should not be used for testing if the original value (read from the db) is NULL</summary>
		/// <param name="fieldIndex">Index of the field to test if its currentvalue is null/undefined</param>
		/// <returns>true if the field's value isn't defined yet, false otherwise</returns>
		public bool TestCurrentFieldValueForNull(AmazonShipmentFieldIndex fieldIndex)
		{
			return base.CheckIfCurrentFieldValueIsNull((int)fieldIndex);
		}

				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			return new AmazonShipmentRelations().GetAllRelations();
		}
		



		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'AmazonAccount' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoAmazonAccount()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(AmazonAccountFields.AmazonAccountID, null, ComparisonOperator.Equal, this.AmazonAccountID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'Shipment' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoShipment()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ShipmentFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}
	
		
		/// <summary>Creates entity fields object for this entity. Used in constructor to setup this entity in a polymorphic scenario.</summary>
		protected virtual IEntityFields2 CreateFields()
		{
			return EntityFieldsFactory.CreateEntityFieldsObject(ShipWorks.Data.Model.EntityType.AmazonShipmentEntity);
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
			return EntityFactoryCache2.GetEntityFactory(typeof(AmazonShipmentEntityFactory));
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


			return base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);


		}
#endif
		/// <summary>
		/// Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element. 
		/// </summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		public override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("AmazonAccount", _amazonAccount);


			toReturn.Add("Shipment", _shipment);
			return toReturn;
		}
		
		/// <summary> Adds the internals to the active context. </summary>
		protected override void AddInternalsToContext()
		{


			if(_amazonAccount!=null)
			{
				_amazonAccount.ActiveContext = base.ActiveContext;
			}
			if(_shipment!=null)
			{
				_shipment.ActiveContext = base.ActiveContext;
			}
		}

		/// <summary> Initializes the class members</summary>
		protected virtual void InitClassMembers()
		{



			_amazonAccount = null;
			_shipment = null;
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

			_fieldsCustomProperties.Add("ShipmentID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("AmazonAccountID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CarrierName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ShippingServiceName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ShippingServiceID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ShippingServiceOfferID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("InsuranceValue", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DimsProfileID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DimsLength", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DimsWidth", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DimsHeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DimsWeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DimsAddWeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DateMustArriveBy", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DeliveryExperience", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CarrierWillPickUp", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DeclaredValue", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("AmazonUniqueShipmentID", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _amazonAccount</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncAmazonAccount(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _amazonAccount, new PropertyChangedEventHandler( OnAmazonAccountPropertyChanged ), "AmazonAccount", AmazonShipmentEntity.Relations.AmazonAccountEntityUsingAmazonAccountID, true, signalRelatedEntity, "AmazonShipment", resetFKFields, new int[] { (int)AmazonShipmentFieldIndex.AmazonAccountID } );		
			_amazonAccount = null;
		}

		/// <summary> setups the sync logic for member _amazonAccount</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncAmazonAccount(IEntity2 relatedEntity)
		{
			if(_amazonAccount!=relatedEntity)
			{
				DesetupSyncAmazonAccount(true, true);
				_amazonAccount = (AmazonAccountEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _amazonAccount, new PropertyChangedEventHandler( OnAmazonAccountPropertyChanged ), "AmazonAccount", AmazonShipmentEntity.Relations.AmazonAccountEntityUsingAmazonAccountID, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnAmazonAccountPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _shipment</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncShipment(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _shipment, new PropertyChangedEventHandler( OnShipmentPropertyChanged ), "Shipment", AmazonShipmentEntity.Relations.ShipmentEntityUsingShipmentID, true, signalRelatedEntity, "Amazon", false, new int[] { (int)AmazonShipmentFieldIndex.ShipmentID } );
			_shipment = null;
		}
		
		/// <summary> setups the sync logic for member _shipment</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncShipment(IEntity2 relatedEntity)
		{
			if(_shipment!=relatedEntity)
			{
				DesetupSyncShipment(true, true);
				_shipment = (ShipmentEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _shipment, new PropertyChangedEventHandler( OnShipmentPropertyChanged ), "Shipment", AmazonShipmentEntity.Relations.ShipmentEntityUsingShipmentID, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnShipmentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this AmazonShipmentEntity</param>
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
		public  static AmazonShipmentRelations Relations
		{
			get	{ return new AmazonShipmentRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}



		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'AmazonAccount' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathAmazonAccount
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(AmazonAccountEntityFactory))),
					(IEntityRelation)GetRelationsForField("AmazonAccount")[0], (int)ShipWorks.Data.Model.EntityType.AmazonShipmentEntity, (int)ShipWorks.Data.Model.EntityType.AmazonAccountEntity, 0, null, null, null, null, "AmazonAccount", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne);
			}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Shipment' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathShipment
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(ShipmentEntityFactory))),
					(IEntityRelation)GetRelationsForField("Shipment")[0], (int)ShipWorks.Data.Model.EntityType.AmazonShipmentEntity, (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, 0, null, null, null, null, "Shipment", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);
			}
		}

		/// <summary> The custom properties for the type of this entity instance.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		public override Dictionary<string, string> CustomPropertiesOfType
		{
			get { return AmazonShipmentEntity.CustomProperties;}
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
			get { return AmazonShipmentEntity.FieldsCustomProperties;}
		}

		/// <summary> The ShipmentID property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."ShipmentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
		public virtual System.Int64 ShipmentID
		{
			get { return (System.Int64)GetValue((int)AmazonShipmentFieldIndex.ShipmentID, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.ShipmentID, value); }
		}

		/// <summary> The AmazonAccountID property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."AmazonAccountID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 AmazonAccountID
		{
			get { return (System.Int64)GetValue((int)AmazonShipmentFieldIndex.AmazonAccountID, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.AmazonAccountID, value); }
		}

		/// <summary> The CarrierName property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."CarrierName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CarrierName
		{
			get { return (System.String)GetValue((int)AmazonShipmentFieldIndex.CarrierName, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.CarrierName, value); }
		}

		/// <summary> The ShippingServiceName property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."ShippingServiceName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShippingServiceName
		{
			get { return (System.String)GetValue((int)AmazonShipmentFieldIndex.ShippingServiceName, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.ShippingServiceName, value); }
		}

		/// <summary> The ShippingServiceID property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."ShippingServiceID"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShippingServiceID
		{
			get { return (System.String)GetValue((int)AmazonShipmentFieldIndex.ShippingServiceID, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.ShippingServiceID, value); }
		}

		/// <summary> The ShippingServiceOfferID property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."ShippingServiceOfferID"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShippingServiceOfferID
		{
			get { return (System.String)GetValue((int)AmazonShipmentFieldIndex.ShippingServiceOfferID, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.ShippingServiceOfferID, value); }
		}

		/// <summary> The InsuranceValue property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."InsuranceValue"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal InsuranceValue
		{
			get { return (System.Decimal)GetValue((int)AmazonShipmentFieldIndex.InsuranceValue, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.InsuranceValue, value); }
		}

		/// <summary> The DimsProfileID property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."DimsProfileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 DimsProfileID
		{
			get { return (System.Int64)GetValue((int)AmazonShipmentFieldIndex.DimsProfileID, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.DimsProfileID, value); }
		}

		/// <summary> The DimsLength property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."DimsLength"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DimsLength
		{
			get { return (System.Double)GetValue((int)AmazonShipmentFieldIndex.DimsLength, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.DimsLength, value); }
		}

		/// <summary> The DimsWidth property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."DimsWidth"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DimsWidth
		{
			get { return (System.Double)GetValue((int)AmazonShipmentFieldIndex.DimsWidth, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.DimsWidth, value); }
		}

		/// <summary> The DimsHeight property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."DimsHeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DimsHeight
		{
			get { return (System.Double)GetValue((int)AmazonShipmentFieldIndex.DimsHeight, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.DimsHeight, value); }
		}

		/// <summary> The DimsWeight property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."DimsWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DimsWeight
		{
			get { return (System.Double)GetValue((int)AmazonShipmentFieldIndex.DimsWeight, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.DimsWeight, value); }
		}

		/// <summary> The DimsAddWeight property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."DimsAddWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean DimsAddWeight
		{
			get { return (System.Boolean)GetValue((int)AmazonShipmentFieldIndex.DimsAddWeight, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.DimsAddWeight, value); }
		}

		/// <summary> The DateMustArriveBy property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."DateMustArriveBy"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime DateMustArriveBy
		{
			get { return (System.DateTime)GetValue((int)AmazonShipmentFieldIndex.DateMustArriveBy, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.DateMustArriveBy, value); }
		}

		/// <summary> The DeliveryExperience property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."DeliveryExperience"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 DeliveryExperience
		{
			get { return (System.Int32)GetValue((int)AmazonShipmentFieldIndex.DeliveryExperience, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.DeliveryExperience, value); }
		}

		/// <summary> The CarrierWillPickUp property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."CarrierWillPickUp"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean CarrierWillPickUp
		{
			get { return (System.Boolean)GetValue((int)AmazonShipmentFieldIndex.CarrierWillPickUp, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.CarrierWillPickUp, value); }
		}

		/// <summary> The DeclaredValue property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."DeclaredValue"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Decimal> DeclaredValue
		{
			get { return (Nullable<System.Decimal>)GetValue((int)AmazonShipmentFieldIndex.DeclaredValue, false); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.DeclaredValue, value); }
		}

		/// <summary> The AmazonUniqueShipmentID property of the Entity AmazonShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "AmazonShipment"."AmazonUniqueShipmentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String AmazonUniqueShipmentID
		{
			get { return (System.String)GetValue((int)AmazonShipmentFieldIndex.AmazonUniqueShipmentID, true); }
			set	{ SetValue((int)AmazonShipmentFieldIndex.AmazonUniqueShipmentID, value); }
		}



		/// <summary> Gets / sets related entity of type 'AmazonAccountEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual AmazonAccountEntity AmazonAccount
		{
			get
			{
				return _amazonAccount;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncAmazonAccount(value);
				}
				else
				{
					if(value==null)
					{
						if(_amazonAccount != null)
						{
							_amazonAccount.UnsetRelatedEntity(this, "AmazonShipment");
						}
					}
					else
					{
						if(_amazonAccount!=value)
						{
							((IEntity2)value).SetRelatedEntity(this, "AmazonShipment");
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'ShipmentEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual ShipmentEntity Shipment
		{
			get
			{
				return _shipment;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncShipment(value);
					if((SerializationHelper.Optimization == SerializationOptimization.Fast) && (value!=null))
					{
						value.SetRelatedEntity(this, "Amazon");
					}
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_shipment !=null);
						DesetupSyncShipment(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("Shipment");
						}
					}
					else
					{
						if(_shipment!=value)
						{
							IEntity2 relatedEntity = (IEntity2)value;
							relatedEntity.SetRelatedEntity(this, "Amazon");
							SetupSyncShipment(relatedEntity);
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
			get { return (int)ShipWorks.Data.Model.EntityType.AmazonShipmentEntity; }
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
