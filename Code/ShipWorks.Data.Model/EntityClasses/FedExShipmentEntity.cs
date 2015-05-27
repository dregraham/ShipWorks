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
	/// Entity class which represents the entity 'FedExShipment'.<br/><br/>
	/// 
	/// </summary>
	[Serializable]
	public partial class FedExShipmentEntity : CommonEntityBase, ISerializable
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<FedExPackageEntity> _packages;


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

			/// <summary>Member name Packages</summary>
			public static readonly string Packages = "Packages";

			/// <summary>Member name Shipment</summary>
			public static readonly string Shipment = "Shipment";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static FedExShipmentEntity()
		{
			SetupCustomPropertyHashtables();
		}

		/// <summary> CTor</summary>
		public FedExShipmentEntity():base("FedExShipmentEntity")
		{
			InitClassEmpty(null, CreateFields());
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public FedExShipmentEntity(IEntityFields2 fields):base("FedExShipmentEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this FedExShipmentEntity</param>
		public FedExShipmentEntity(IValidator validator):base("FedExShipmentEntity")
		{
			InitClassEmpty(validator, CreateFields());
		}
				

		/// <summary> CTor</summary>
		/// <param name="shipmentID">PK value for FedExShipment which data should be fetched into this FedExShipment object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public FedExShipmentEntity(System.Int64 shipmentID):base("FedExShipmentEntity")
		{
			InitClassEmpty(null, CreateFields());
			this.ShipmentID = shipmentID;
		}

		/// <summary> CTor</summary>
		/// <param name="shipmentID">PK value for FedExShipment which data should be fetched into this FedExShipment object</param>
		/// <param name="validator">The custom validator object for this FedExShipmentEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public FedExShipmentEntity(System.Int64 shipmentID, IValidator validator):base("FedExShipmentEntity")
		{
			InitClassEmpty(validator, CreateFields());
			this.ShipmentID = shipmentID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected FedExShipmentEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_packages = (EntityCollection<FedExPackageEntity>)info.GetValue("_packages", typeof(EntityCollection<FedExPackageEntity>));


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
			switch((FedExShipmentFieldIndex)fieldIndex)
			{
				case FedExShipmentFieldIndex.ShipmentID:
					DesetupSyncShipment(true, false);
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
					this.Packages.Add((FedExPackageEntity)entity);
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
			return FedExShipmentEntity.GetRelationsForField(fieldName);
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
					toReturn.Add(FedExShipmentEntity.Relations.FedExPackageEntityUsingShipmentID);
					break;

				case "Shipment":
					toReturn.Add(FedExShipmentEntity.Relations.ShipmentEntityUsingShipmentID);
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
					this.Packages.Add((FedExPackageEntity)relatedEntity);
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

				case "Packages":
					base.PerformRelatedEntityRemoval(this.Packages, relatedEntity, signalRelatedEntityManyToOne);
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
		public bool TestOriginalFieldValueForNull(FedExShipmentFieldIndex fieldIndex)
		{
			return base.Fields[(int)fieldIndex].IsNull;
		}
		
		/// <summary>Returns true if the current value for the field with the fieldIndex passed in represents null/not defined, false otherwise.
		/// Should not be used for testing if the original value (read from the db) is NULL</summary>
		/// <param name="fieldIndex">Index of the field to test if its currentvalue is null/undefined</param>
		/// <returns>true if the field's value isn't defined yet, false otherwise</returns>
		public bool TestCurrentFieldValueForNull(FedExShipmentFieldIndex fieldIndex)
		{
			return base.CheckIfCurrentFieldValueIsNull((int)fieldIndex);
		}

				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			return new FedExShipmentRelations().GetAllRelations();
		}
		

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entities of type 'FedExPackage' to this entity. Use DataAccessAdapter.FetchEntityCollection() to fetch these related entities.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoPackages()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(FedExPackageFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
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
			return EntityFieldsFactory.CreateEntityFieldsObject(ShipWorks.Data.Model.EntityType.FedExShipmentEntity);
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
			return EntityFactoryCache2.GetEntityFactory(typeof(FedExShipmentEntityFactory));
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
			this._packages = (EntityCollection<FedExPackageEntity>) collectionsQueue.Dequeue();

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
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<FedExPackageEntity>(EntityFactoryCache2.GetEntityFactory(typeof(FedExPackageEntityFactory))) : null);

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

			toReturn.Add("Shipment", _shipment);
			return toReturn;
		}
		
		/// <summary> Adds the internals to the active context. </summary>
		protected override void AddInternalsToContext()
		{
			if(_packages!=null)
			{
				_packages.ActiveContext = base.ActiveContext;
			}


			if(_shipment!=null)
			{
				_shipment.ActiveContext = base.ActiveContext;
			}
		}

		/// <summary> Initializes the class members</summary>
		protected virtual void InitClassMembers()
		{

			_packages = null;


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

			_fieldsCustomProperties.Add("FedExAccountID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("MasterFormID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Service", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Signature", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PackagingType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("NonStandardContainer", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ReferenceCustomer", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ReferenceInvoice", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ReferencePO", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ReferenceShipmentIntegrity", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PayorTransportType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PayorTransportName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PayorTransportAccount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PayorDutiesType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PayorDutiesAccount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PayorDutiesName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PayorDutiesCountryCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("SaturdayDelivery", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HomeDeliveryType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HomeDeliveryInstructions", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HomeDeliveryDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HomeDeliveryPhone", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("FreightInsidePickup", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("FreightInsideDelivery", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("FreightBookingNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("FreightLoadAndCount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EmailNotifyBroker", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EmailNotifySender", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EmailNotifyRecipient", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EmailNotifyOther", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EmailNotifyOtherAddress", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EmailNotifyMessage", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodEnabled", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodAmount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodPaymentType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodAddFreight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodOriginID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodFirstName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodLastName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodCompany", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodStreet1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodStreet2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodStreet3", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodCity", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodStateProvCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodPostalCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodCountryCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodPhone", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodTrackingNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodTrackingFormID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodTIN", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodChargeBasis", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CodAccountNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BrokerEnabled", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BrokerAccount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BrokerFirstName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BrokerLastName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BrokerCompany", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BrokerStreet1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BrokerStreet2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BrokerStreet3", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BrokerCity", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BrokerStateProvCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BrokerPostalCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BrokerCountryCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BrokerPhone", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BrokerPhoneExtension", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BrokerEmail", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsAdmissibilityPackaging", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsRecipientTIN", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsDocumentsOnly", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsDocumentsDescription", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsExportFilingOption", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsAESEEI", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsRecipientIdentificationType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsRecipientIdentificationValue", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsOptionsType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsOptionsDesription", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CommercialInvoice", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CommercialInvoiceFileElectronically", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CommercialInvoiceTermsOfSale", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CommercialInvoicePurpose", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CommercialInvoiceComments", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CommercialInvoiceFreight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CommercialInvoiceInsurance", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CommercialInvoiceOther", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CommercialInvoiceReference", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ImporterOfRecord", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ImporterAccount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ImporterTIN", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ImporterFirstName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ImporterLastName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ImporterCompany", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ImporterStreet1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ImporterStreet2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ImporterStreet3", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ImporterCity", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ImporterStateProvCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ImporterPostalCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ImporterCountryCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ImporterPhone", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("SmartPostIndicia", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("SmartPostEndorsement", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("SmartPostConfirmation", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("SmartPostCustomerManifest", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("SmartPostHubID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("SmartPostUspsApplicationId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DropoffType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("OriginResidentialDetermination", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("FedExHoldAtLocationEnabled", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldLocationId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldLocationType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldContactId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldPersonName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldTitle", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldCompanyName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldPhoneNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldPhoneExtension", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldPagerNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldFaxNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldEmailAddress", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldStreet1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldStreet2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldStreet3", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldCity", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldStateOrProvinceCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldPostalCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldUrbanizationCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldCountryCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("HoldResidential", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsNaftaEnabled", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsNaftaPreferenceType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsNaftaDeterminationCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsNaftaProducerId", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsNaftaNetCostMethod", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ReturnType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("RmaNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("RmaReason", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ReturnSaturdayPickup", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("TrafficInArmsLicenseNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("IntlExportDetailType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("IntlExportDetailForeignTradeZoneCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("IntlExportDetailEntryNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("IntlExportDetailLicenseOrPermitNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("IntlExportDetailLicenseOrPermitExpirationDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("WeightUnitType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("LinearUnitType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("RequestedLabelFormat", fieldHashtable);
		}
		#endregion


		/// <summary> Removes the sync logic for member _shipment</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncShipment(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _shipment, new PropertyChangedEventHandler( OnShipmentPropertyChanged ), "Shipment", FedExShipmentEntity.Relations.ShipmentEntityUsingShipmentID, true, signalRelatedEntity, "FedEx", false, new int[] { (int)FedExShipmentFieldIndex.ShipmentID } );
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
				base.PerformSetupSyncRelatedEntity( _shipment, new PropertyChangedEventHandler( OnShipmentPropertyChanged ), "Shipment", FedExShipmentEntity.Relations.ShipmentEntityUsingShipmentID, true, new string[] {  } );
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
		/// <param name="validator">The validator object for this FedExShipmentEntity</param>
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
		public  static FedExShipmentRelations Relations
		{
			get	{ return new FedExShipmentRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'FedExPackage' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathPackages
		{
			get
			{
				return new PrefetchPathElement2( new EntityCollection<FedExPackageEntity>(EntityFactoryCache2.GetEntityFactory(typeof(FedExPackageEntityFactory))),
					(IEntityRelation)GetRelationsForField("Packages")[0], (int)ShipWorks.Data.Model.EntityType.FedExShipmentEntity, (int)ShipWorks.Data.Model.EntityType.FedExPackageEntity, 0, null, null, null, null, "Packages", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);
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
					(IEntityRelation)GetRelationsForField("Shipment")[0], (int)ShipWorks.Data.Model.EntityType.FedExShipmentEntity, (int)ShipWorks.Data.Model.EntityType.ShipmentEntity, 0, null, null, null, null, "Shipment", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);
			}
		}

		/// <summary> The custom properties for the type of this entity instance.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		public override Dictionary<string, string> CustomPropertiesOfType
		{
			get { return FedExShipmentEntity.CustomProperties;}
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
			get { return FedExShipmentEntity.FieldsCustomProperties;}
		}

		/// <summary> The ShipmentID property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ShipmentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
		public virtual System.Int64 ShipmentID
		{
			get { return (System.Int64)GetValue((int)FedExShipmentFieldIndex.ShipmentID, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ShipmentID, value); }
		}

		/// <summary> The FedExAccountID property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."FedExAccountID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 FedExAccountID
		{
			get { return (System.Int64)GetValue((int)FedExShipmentFieldIndex.FedExAccountID, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.FedExAccountID, value); }
		}

		/// <summary> The MasterFormID property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."MasterFormID"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 4<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String MasterFormID
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.MasterFormID, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.MasterFormID, value); }
		}

		/// <summary> The Service property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."Service"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 Service
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.Service, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.Service, value); }
		}

		/// <summary> The Signature property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."Signature"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 Signature
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.Signature, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.Signature, value); }
		}

		/// <summary> The PackagingType property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."PackagingType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 PackagingType
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.PackagingType, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.PackagingType, value); }
		}

		/// <summary> The NonStandardContainer property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."NonStandardContainer"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean NonStandardContainer
		{
			get { return (System.Boolean)GetValue((int)FedExShipmentFieldIndex.NonStandardContainer, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.NonStandardContainer, value); }
		}

		/// <summary> The ReferenceCustomer property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ReferenceCustomer"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ReferenceCustomer
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ReferenceCustomer, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ReferenceCustomer, value); }
		}

		/// <summary> The ReferenceInvoice property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ReferenceInvoice"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ReferenceInvoice
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ReferenceInvoice, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ReferenceInvoice, value); }
		}

		/// <summary> The ReferencePO property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ReferencePO"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ReferencePO
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ReferencePO, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ReferencePO, value); }
		}

		/// <summary> The ReferenceShipmentIntegrity property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ReferenceShipmentIntegrity"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ReferenceShipmentIntegrity
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ReferenceShipmentIntegrity, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ReferenceShipmentIntegrity, value); }
		}

		/// <summary> The PayorTransportType property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."PayorTransportType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 PayorTransportType
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.PayorTransportType, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.PayorTransportType, value); }
		}

		/// <summary> The PayorTransportName property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."PayorTransportName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String PayorTransportName
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.PayorTransportName, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.PayorTransportName, value); }
		}

		/// <summary> The PayorTransportAccount property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."PayorTransportAccount"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 12<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String PayorTransportAccount
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.PayorTransportAccount, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.PayorTransportAccount, value); }
		}

		/// <summary> The PayorDutiesType property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."PayorDutiesType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 PayorDutiesType
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.PayorDutiesType, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.PayorDutiesType, value); }
		}

		/// <summary> The PayorDutiesAccount property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."PayorDutiesAccount"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 12<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String PayorDutiesAccount
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.PayorDutiesAccount, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.PayorDutiesAccount, value); }
		}

		/// <summary> The PayorDutiesName property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."PayorDutiesName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String PayorDutiesName
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.PayorDutiesName, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.PayorDutiesName, value); }
		}

		/// <summary> The PayorDutiesCountryCode property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."PayorDutiesCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String PayorDutiesCountryCode
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.PayorDutiesCountryCode, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.PayorDutiesCountryCode, value); }
		}

		/// <summary> The SaturdayDelivery property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."SaturdayDelivery"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean SaturdayDelivery
		{
			get { return (System.Boolean)GetValue((int)FedExShipmentFieldIndex.SaturdayDelivery, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.SaturdayDelivery, value); }
		}

		/// <summary> The HomeDeliveryType property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HomeDeliveryType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 HomeDeliveryType
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.HomeDeliveryType, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HomeDeliveryType, value); }
		}

		/// <summary> The HomeDeliveryInstructions property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HomeDeliveryInstructions"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 74<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String HomeDeliveryInstructions
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HomeDeliveryInstructions, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HomeDeliveryInstructions, value); }
		}

		/// <summary> The HomeDeliveryDate property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HomeDeliveryDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime HomeDeliveryDate
		{
			get { return (System.DateTime)GetValue((int)FedExShipmentFieldIndex.HomeDeliveryDate, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HomeDeliveryDate, value); }
		}

		/// <summary> The HomeDeliveryPhone property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HomeDeliveryPhone"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 24<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String HomeDeliveryPhone
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HomeDeliveryPhone, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HomeDeliveryPhone, value); }
		}

		/// <summary> The FreightInsidePickup property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."FreightInsidePickup"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean FreightInsidePickup
		{
			get { return (System.Boolean)GetValue((int)FedExShipmentFieldIndex.FreightInsidePickup, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.FreightInsidePickup, value); }
		}

		/// <summary> The FreightInsideDelivery property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."FreightInsideDelivery"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean FreightInsideDelivery
		{
			get { return (System.Boolean)GetValue((int)FedExShipmentFieldIndex.FreightInsideDelivery, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.FreightInsideDelivery, value); }
		}

		/// <summary> The FreightBookingNumber property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."FreightBookingNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 12<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String FreightBookingNumber
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.FreightBookingNumber, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.FreightBookingNumber, value); }
		}

		/// <summary> The FreightLoadAndCount property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."FreightLoadAndCount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 FreightLoadAndCount
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.FreightLoadAndCount, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.FreightLoadAndCount, value); }
		}

		/// <summary> The EmailNotifyBroker property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."EmailNotifyBroker"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 EmailNotifyBroker
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.EmailNotifyBroker, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.EmailNotifyBroker, value); }
		}

		/// <summary> The EmailNotifySender property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."EmailNotifySender"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 EmailNotifySender
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.EmailNotifySender, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.EmailNotifySender, value); }
		}

		/// <summary> The EmailNotifyRecipient property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."EmailNotifyRecipient"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 EmailNotifyRecipient
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.EmailNotifyRecipient, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.EmailNotifyRecipient, value); }
		}

		/// <summary> The EmailNotifyOther property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."EmailNotifyOther"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 EmailNotifyOther
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.EmailNotifyOther, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.EmailNotifyOther, value); }
		}

		/// <summary> The EmailNotifyOtherAddress property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."EmailNotifyOtherAddress"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String EmailNotifyOtherAddress
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.EmailNotifyOtherAddress, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.EmailNotifyOtherAddress, value); }
		}

		/// <summary> The EmailNotifyMessage property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."EmailNotifyMessage"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 120<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String EmailNotifyMessage
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.EmailNotifyMessage, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.EmailNotifyMessage, value); }
		}

		/// <summary> The CodEnabled property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodEnabled"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean CodEnabled
		{
			get { return (System.Boolean)GetValue((int)FedExShipmentFieldIndex.CodEnabled, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodEnabled, value); }
		}

		/// <summary> The CodAmount property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodAmount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal CodAmount
		{
			get { return (System.Decimal)GetValue((int)FedExShipmentFieldIndex.CodAmount, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodAmount, value); }
		}

		/// <summary> The CodPaymentType property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodPaymentType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 CodPaymentType
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.CodPaymentType, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodPaymentType, value); }
		}

		/// <summary> The CodAddFreight property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodAddFreight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean CodAddFreight
		{
			get { return (System.Boolean)GetValue((int)FedExShipmentFieldIndex.CodAddFreight, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodAddFreight, value); }
		}

		/// <summary> The CodOriginID property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodOriginID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 CodOriginID
		{
			get { return (System.Int64)GetValue((int)FedExShipmentFieldIndex.CodOriginID, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodOriginID, value); }
		}

		/// <summary> The CodFirstName property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodFirstName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodFirstName
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CodFirstName, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodFirstName, value); }
		}

		/// <summary> The CodLastName property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodLastName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodLastName
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CodLastName, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodLastName, value); }
		}

		/// <summary> The CodCompany property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodCompany"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodCompany
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CodCompany, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodCompany, value); }
		}

		/// <summary> The CodStreet1 property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodStreet1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodStreet1
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CodStreet1, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodStreet1, value); }
		}

		/// <summary> The CodStreet2 property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodStreet2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodStreet2
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CodStreet2, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodStreet2, value); }
		}

		/// <summary> The CodStreet3 property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodStreet3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodStreet3
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CodStreet3, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodStreet3, value); }
		}

		/// <summary> The CodCity property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodCity"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodCity
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CodCity, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodCity, value); }
		}

		/// <summary> The CodStateProvCode property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodStateProvCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodStateProvCode
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CodStateProvCode, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodStateProvCode, value); }
		}

		/// <summary> The CodPostalCode property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodPostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodPostalCode
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CodPostalCode, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodPostalCode, value); }
		}

		/// <summary> The CodCountryCode property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodCountryCode
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CodCountryCode, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodCountryCode, value); }
		}

		/// <summary> The CodPhone property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodPhone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodPhone
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CodPhone, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodPhone, value); }
		}

		/// <summary> The CodTrackingNumber property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodTrackingNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodTrackingNumber
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CodTrackingNumber, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodTrackingNumber, value); }
		}

		/// <summary> The CodTrackingFormID property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodTrackingFormID"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 4<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodTrackingFormID
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CodTrackingFormID, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodTrackingFormID, value); }
		}

		/// <summary> The CodTIN property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodTIN"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 24<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodTIN
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CodTIN, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodTIN, value); }
		}

		/// <summary> The CodChargeBasis property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodChargeBasis"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 CodChargeBasis
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.CodChargeBasis, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodChargeBasis, value); }
		}

		/// <summary> The CodAccountNumber property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CodAccountNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CodAccountNumber
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CodAccountNumber, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CodAccountNumber, value); }
		}

		/// <summary> The BrokerEnabled property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."BrokerEnabled"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean BrokerEnabled
		{
			get { return (System.Boolean)GetValue((int)FedExShipmentFieldIndex.BrokerEnabled, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.BrokerEnabled, value); }
		}

		/// <summary> The BrokerAccount property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."BrokerAccount"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 12<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BrokerAccount
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.BrokerAccount, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.BrokerAccount, value); }
		}

		/// <summary> The BrokerFirstName property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."BrokerFirstName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BrokerFirstName
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.BrokerFirstName, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.BrokerFirstName, value); }
		}

		/// <summary> The BrokerLastName property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."BrokerLastName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BrokerLastName
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.BrokerLastName, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.BrokerLastName, value); }
		}

		/// <summary> The BrokerCompany property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."BrokerCompany"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BrokerCompany
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.BrokerCompany, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.BrokerCompany, value); }
		}

		/// <summary> The BrokerStreet1 property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."BrokerStreet1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BrokerStreet1
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.BrokerStreet1, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.BrokerStreet1, value); }
		}

		/// <summary> The BrokerStreet2 property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."BrokerStreet2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BrokerStreet2
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.BrokerStreet2, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.BrokerStreet2, value); }
		}

		/// <summary> The BrokerStreet3 property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."BrokerStreet3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BrokerStreet3
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.BrokerStreet3, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.BrokerStreet3, value); }
		}

		/// <summary> The BrokerCity property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."BrokerCity"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BrokerCity
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.BrokerCity, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.BrokerCity, value); }
		}

		/// <summary> The BrokerStateProvCode property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."BrokerStateProvCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BrokerStateProvCode
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.BrokerStateProvCode, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.BrokerStateProvCode, value); }
		}

		/// <summary> The BrokerPostalCode property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."BrokerPostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BrokerPostalCode
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.BrokerPostalCode, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.BrokerPostalCode, value); }
		}

		/// <summary> The BrokerCountryCode property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."BrokerCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BrokerCountryCode
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.BrokerCountryCode, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.BrokerCountryCode, value); }
		}

		/// <summary> The BrokerPhone property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."BrokerPhone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BrokerPhone
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.BrokerPhone, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.BrokerPhone, value); }
		}

		/// <summary> The BrokerPhoneExtension property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."BrokerPhoneExtension"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 8<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BrokerPhoneExtension
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.BrokerPhoneExtension, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.BrokerPhoneExtension, value); }
		}

		/// <summary> The BrokerEmail property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."BrokerEmail"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BrokerEmail
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.BrokerEmail, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.BrokerEmail, value); }
		}

		/// <summary> The CustomsAdmissibilityPackaging property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CustomsAdmissibilityPackaging"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 CustomsAdmissibilityPackaging
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.CustomsAdmissibilityPackaging, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CustomsAdmissibilityPackaging, value); }
		}

		/// <summary> The CustomsRecipientTIN property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CustomsRecipientTIN"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 24<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CustomsRecipientTIN
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CustomsRecipientTIN, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CustomsRecipientTIN, value); }
		}

		/// <summary> The CustomsDocumentsOnly property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CustomsDocumentsOnly"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean CustomsDocumentsOnly
		{
			get { return (System.Boolean)GetValue((int)FedExShipmentFieldIndex.CustomsDocumentsOnly, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CustomsDocumentsOnly, value); }
		}

		/// <summary> The CustomsDocumentsDescription property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CustomsDocumentsDescription"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CustomsDocumentsDescription
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CustomsDocumentsDescription, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CustomsDocumentsDescription, value); }
		}

		/// <summary> The CustomsExportFilingOption property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CustomsExportFilingOption"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 CustomsExportFilingOption
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.CustomsExportFilingOption, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CustomsExportFilingOption, value); }
		}

		/// <summary> The CustomsAESEEI property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CustomsAESEEI"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CustomsAESEEI
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CustomsAESEEI, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CustomsAESEEI, value); }
		}

		/// <summary> The CustomsRecipientIdentificationType property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CustomsRecipientIdentificationType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 CustomsRecipientIdentificationType
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.CustomsRecipientIdentificationType, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CustomsRecipientIdentificationType, value); }
		}

		/// <summary> The CustomsRecipientIdentificationValue property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CustomsRecipientIdentificationValue"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CustomsRecipientIdentificationValue
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CustomsRecipientIdentificationValue, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CustomsRecipientIdentificationValue, value); }
		}

		/// <summary> The CustomsOptionsType property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CustomsOptionsType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 CustomsOptionsType
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.CustomsOptionsType, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CustomsOptionsType, value); }
		}

		/// <summary> The CustomsOptionsDesription property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CustomsOptionsDesription"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 32<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CustomsOptionsDesription
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CustomsOptionsDesription, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CustomsOptionsDesription, value); }
		}

		/// <summary> The CommercialInvoice property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CommercialInvoice"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean CommercialInvoice
		{
			get { return (System.Boolean)GetValue((int)FedExShipmentFieldIndex.CommercialInvoice, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CommercialInvoice, value); }
		}

		/// <summary> The CommercialInvoiceFileElectronically property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CommercialInvoiceFileElectronically"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean CommercialInvoiceFileElectronically
		{
			get { return (System.Boolean)GetValue((int)FedExShipmentFieldIndex.CommercialInvoiceFileElectronically, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CommercialInvoiceFileElectronically, value); }
		}

		/// <summary> The CommercialInvoiceTermsOfSale property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CommercialInvoiceTermsOfSale"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 CommercialInvoiceTermsOfSale
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.CommercialInvoiceTermsOfSale, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CommercialInvoiceTermsOfSale, value); }
		}

		/// <summary> The CommercialInvoicePurpose property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CommercialInvoicePurpose"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 CommercialInvoicePurpose
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.CommercialInvoicePurpose, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CommercialInvoicePurpose, value); }
		}

		/// <summary> The CommercialInvoiceComments property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CommercialInvoiceComments"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 200<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CommercialInvoiceComments
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CommercialInvoiceComments, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CommercialInvoiceComments, value); }
		}

		/// <summary> The CommercialInvoiceFreight property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CommercialInvoiceFreight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal CommercialInvoiceFreight
		{
			get { return (System.Decimal)GetValue((int)FedExShipmentFieldIndex.CommercialInvoiceFreight, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CommercialInvoiceFreight, value); }
		}

		/// <summary> The CommercialInvoiceInsurance property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CommercialInvoiceInsurance"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal CommercialInvoiceInsurance
		{
			get { return (System.Decimal)GetValue((int)FedExShipmentFieldIndex.CommercialInvoiceInsurance, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CommercialInvoiceInsurance, value); }
		}

		/// <summary> The CommercialInvoiceOther property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CommercialInvoiceOther"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal CommercialInvoiceOther
		{
			get { return (System.Decimal)GetValue((int)FedExShipmentFieldIndex.CommercialInvoiceOther, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CommercialInvoiceOther, value); }
		}

		/// <summary> The CommercialInvoiceReference property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CommercialInvoiceReference"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CommercialInvoiceReference
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CommercialInvoiceReference, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CommercialInvoiceReference, value); }
		}

		/// <summary> The ImporterOfRecord property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ImporterOfRecord"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean ImporterOfRecord
		{
			get { return (System.Boolean)GetValue((int)FedExShipmentFieldIndex.ImporterOfRecord, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ImporterOfRecord, value); }
		}

		/// <summary> The ImporterAccount property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ImporterAccount"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 12<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ImporterAccount
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ImporterAccount, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ImporterAccount, value); }
		}

		/// <summary> The ImporterTIN property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ImporterTIN"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 24<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ImporterTIN
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ImporterTIN, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ImporterTIN, value); }
		}

		/// <summary> The ImporterFirstName property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ImporterFirstName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ImporterFirstName
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ImporterFirstName, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ImporterFirstName, value); }
		}

		/// <summary> The ImporterLastName property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ImporterLastName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ImporterLastName
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ImporterLastName, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ImporterLastName, value); }
		}

		/// <summary> The ImporterCompany property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ImporterCompany"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ImporterCompany
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ImporterCompany, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ImporterCompany, value); }
		}

		/// <summary> The ImporterStreet1 property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ImporterStreet1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ImporterStreet1
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ImporterStreet1, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ImporterStreet1, value); }
		}

		/// <summary> The ImporterStreet2 property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ImporterStreet2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ImporterStreet2
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ImporterStreet2, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ImporterStreet2, value); }
		}

		/// <summary> The ImporterStreet3 property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ImporterStreet3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ImporterStreet3
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ImporterStreet3, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ImporterStreet3, value); }
		}

		/// <summary> The ImporterCity property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ImporterCity"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ImporterCity
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ImporterCity, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ImporterCity, value); }
		}

		/// <summary> The ImporterStateProvCode property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ImporterStateProvCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ImporterStateProvCode
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ImporterStateProvCode, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ImporterStateProvCode, value); }
		}

		/// <summary> The ImporterPostalCode property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ImporterPostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ImporterPostalCode
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ImporterPostalCode, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ImporterPostalCode, value); }
		}

		/// <summary> The ImporterCountryCode property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ImporterCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ImporterCountryCode
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ImporterCountryCode, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ImporterCountryCode, value); }
		}

		/// <summary> The ImporterPhone property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ImporterPhone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ImporterPhone
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.ImporterPhone, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ImporterPhone, value); }
		}

		/// <summary> The SmartPostIndicia property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."SmartPostIndicia"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 SmartPostIndicia
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.SmartPostIndicia, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.SmartPostIndicia, value); }
		}

		/// <summary> The SmartPostEndorsement property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."SmartPostEndorsement"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 SmartPostEndorsement
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.SmartPostEndorsement, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.SmartPostEndorsement, value); }
		}

		/// <summary> The SmartPostConfirmation property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."SmartPostConfirmation"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean SmartPostConfirmation
		{
			get { return (System.Boolean)GetValue((int)FedExShipmentFieldIndex.SmartPostConfirmation, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.SmartPostConfirmation, value); }
		}

		/// <summary> The SmartPostCustomerManifest property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."SmartPostCustomerManifest"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String SmartPostCustomerManifest
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.SmartPostCustomerManifest, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.SmartPostCustomerManifest, value); }
		}

		/// <summary> The SmartPostHubID property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."SmartPostHubID"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String SmartPostHubID
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.SmartPostHubID, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.SmartPostHubID, value); }
		}

		/// <summary> The SmartPostUspsApplicationId property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."SmartPostUspsApplicationId"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String SmartPostUspsApplicationId
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.SmartPostUspsApplicationId, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.SmartPostUspsApplicationId, value); }
		}

		/// <summary> The DropoffType property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."DropoffType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 DropoffType
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.DropoffType, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.DropoffType, value); }
		}

		/// <summary> The OriginResidentialDetermination property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."OriginResidentialDetermination"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 OriginResidentialDetermination
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.OriginResidentialDetermination, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.OriginResidentialDetermination, value); }
		}

		/// <summary> The FedExHoldAtLocationEnabled property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."FedExHoldAtLocationEnabled"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean FedExHoldAtLocationEnabled
		{
			get { return (System.Boolean)GetValue((int)FedExShipmentFieldIndex.FedExHoldAtLocationEnabled, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.FedExHoldAtLocationEnabled, value); }
		}

		/// <summary> The HoldLocationId property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldLocationId"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldLocationId
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldLocationId, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldLocationId, value); }
		}

		/// <summary> The HoldLocationType property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldLocationType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> HoldLocationType
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExShipmentFieldIndex.HoldLocationType, false); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldLocationType, value); }
		}

		/// <summary> The HoldContactId property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldContactId"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldContactId
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldContactId, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldContactId, value); }
		}

		/// <summary> The HoldPersonName property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldPersonName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldPersonName
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldPersonName, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldPersonName, value); }
		}

		/// <summary> The HoldTitle property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldTitle"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldTitle
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldTitle, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldTitle, value); }
		}

		/// <summary> The HoldCompanyName property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldCompanyName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldCompanyName
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldCompanyName, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldCompanyName, value); }
		}

		/// <summary> The HoldPhoneNumber property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldPhoneNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldPhoneNumber
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldPhoneNumber, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldPhoneNumber, value); }
		}

		/// <summary> The HoldPhoneExtension property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldPhoneExtension"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldPhoneExtension
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldPhoneExtension, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldPhoneExtension, value); }
		}

		/// <summary> The HoldPagerNumber property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldPagerNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldPagerNumber
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldPagerNumber, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldPagerNumber, value); }
		}

		/// <summary> The HoldFaxNumber property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldFaxNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldFaxNumber
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldFaxNumber, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldFaxNumber, value); }
		}

		/// <summary> The HoldEmailAddress property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldEmailAddress"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldEmailAddress
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldEmailAddress, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldEmailAddress, value); }
		}

		/// <summary> The HoldStreet1 property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldStreet1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldStreet1
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldStreet1, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldStreet1, value); }
		}

		/// <summary> The HoldStreet2 property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldStreet2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldStreet2
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldStreet2, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldStreet2, value); }
		}

		/// <summary> The HoldStreet3 property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldStreet3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 250<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldStreet3
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldStreet3, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldStreet3, value); }
		}

		/// <summary> The HoldCity property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldCity"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldCity
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldCity, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldCity, value); }
		}

		/// <summary> The HoldStateOrProvinceCode property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldStateOrProvinceCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldStateOrProvinceCode
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldStateOrProvinceCode, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldStateOrProvinceCode, value); }
		}

		/// <summary> The HoldPostalCode property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldPostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldPostalCode
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldPostalCode, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldPostalCode, value); }
		}

		/// <summary> The HoldUrbanizationCode property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldUrbanizationCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldUrbanizationCode
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldUrbanizationCode, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldUrbanizationCode, value); }
		}

		/// <summary> The HoldCountryCode property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HoldCountryCode
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.HoldCountryCode, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldCountryCode, value); }
		}

		/// <summary> The HoldResidential property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."HoldResidential"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> HoldResidential
		{
			get { return (Nullable<System.Boolean>)GetValue((int)FedExShipmentFieldIndex.HoldResidential, false); }
			set	{ SetValue((int)FedExShipmentFieldIndex.HoldResidential, value); }
		}

		/// <summary> The CustomsNaftaEnabled property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CustomsNaftaEnabled"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean CustomsNaftaEnabled
		{
			get { return (System.Boolean)GetValue((int)FedExShipmentFieldIndex.CustomsNaftaEnabled, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CustomsNaftaEnabled, value); }
		}

		/// <summary> The CustomsNaftaPreferenceType property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CustomsNaftaPreferenceType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 CustomsNaftaPreferenceType
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.CustomsNaftaPreferenceType, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CustomsNaftaPreferenceType, value); }
		}

		/// <summary> The CustomsNaftaDeterminationCode property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CustomsNaftaDeterminationCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 CustomsNaftaDeterminationCode
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.CustomsNaftaDeterminationCode, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CustomsNaftaDeterminationCode, value); }
		}

		/// <summary> The CustomsNaftaProducerId property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CustomsNaftaProducerId"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String CustomsNaftaProducerId
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.CustomsNaftaProducerId, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CustomsNaftaProducerId, value); }
		}

		/// <summary> The CustomsNaftaNetCostMethod property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."CustomsNaftaNetCostMethod"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 CustomsNaftaNetCostMethod
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.CustomsNaftaNetCostMethod, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.CustomsNaftaNetCostMethod, value); }
		}

		/// <summary> The ReturnType property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ReturnType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ReturnType
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.ReturnType, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ReturnType, value); }
		}

		/// <summary> The RmaNumber property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."RmaNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String RmaNumber
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.RmaNumber, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.RmaNumber, value); }
		}

		/// <summary> The RmaReason property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."RmaReason"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String RmaReason
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.RmaReason, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.RmaReason, value); }
		}

		/// <summary> The ReturnSaturdayPickup property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."ReturnSaturdayPickup"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean ReturnSaturdayPickup
		{
			get { return (System.Boolean)GetValue((int)FedExShipmentFieldIndex.ReturnSaturdayPickup, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.ReturnSaturdayPickup, value); }
		}

		/// <summary> The TrafficInArmsLicenseNumber property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."TrafficInArmsLicenseNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 32<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String TrafficInArmsLicenseNumber
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.TrafficInArmsLicenseNumber, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.TrafficInArmsLicenseNumber, value); }
		}

		/// <summary> The IntlExportDetailType property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."IntlExportDetailType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 IntlExportDetailType
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.IntlExportDetailType, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.IntlExportDetailType, value); }
		}

		/// <summary> The IntlExportDetailForeignTradeZoneCode property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."IntlExportDetailForeignTradeZoneCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String IntlExportDetailForeignTradeZoneCode
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.IntlExportDetailForeignTradeZoneCode, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.IntlExportDetailForeignTradeZoneCode, value); }
		}

		/// <summary> The IntlExportDetailEntryNumber property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."IntlExportDetailEntryNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String IntlExportDetailEntryNumber
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.IntlExportDetailEntryNumber, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.IntlExportDetailEntryNumber, value); }
		}

		/// <summary> The IntlExportDetailLicenseOrPermitNumber property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."IntlExportDetailLicenseOrPermitNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String IntlExportDetailLicenseOrPermitNumber
		{
			get { return (System.String)GetValue((int)FedExShipmentFieldIndex.IntlExportDetailLicenseOrPermitNumber, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.IntlExportDetailLicenseOrPermitNumber, value); }
		}

		/// <summary> The IntlExportDetailLicenseOrPermitExpirationDate property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."IntlExportDetailLicenseOrPermitExpirationDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.DateTime> IntlExportDetailLicenseOrPermitExpirationDate
		{
			get { return (Nullable<System.DateTime>)GetValue((int)FedExShipmentFieldIndex.IntlExportDetailLicenseOrPermitExpirationDate, false); }
			set	{ SetValue((int)FedExShipmentFieldIndex.IntlExportDetailLicenseOrPermitExpirationDate, value); }
		}

		/// <summary> The WeightUnitType property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."WeightUnitType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 WeightUnitType
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.WeightUnitType, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.WeightUnitType, value); }
		}

		/// <summary> The LinearUnitType property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."LinearUnitType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 LinearUnitType
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.LinearUnitType, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.LinearUnitType, value); }
		}

		/// <summary> The RequestedLabelFormat property of the Entity FedExShipment<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FedExShipment"."RequestedLabelFormat"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 RequestedLabelFormat
		{
			get { return (System.Int32)GetValue((int)FedExShipmentFieldIndex.RequestedLabelFormat, true); }
			set	{ SetValue((int)FedExShipmentFieldIndex.RequestedLabelFormat, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'FedExPackageEntity' which are related to this entity via a relation of type '1:n'.
		/// If the EntityCollection hasn't been fetched yet, the collection returned will be empty.</summary>
		[TypeContainedAttribute(typeof(FedExPackageEntity))]
		public virtual EntityCollection<FedExPackageEntity> Packages
		{
			get
			{
				if(_packages==null)
				{
					_packages = new EntityCollection<FedExPackageEntity>(EntityFactoryCache2.GetEntityFactory(typeof(FedExPackageEntityFactory)));
					_packages.SetContainingEntityInfo(this, "FedExShipment");
				}
				return _packages;
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
						value.SetRelatedEntity(this, "FedEx");
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
							relatedEntity.SetRelatedEntity(this, "FedEx");
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
			get { return (int)ShipWorks.Data.Model.EntityType.FedExShipmentEntity; }
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
