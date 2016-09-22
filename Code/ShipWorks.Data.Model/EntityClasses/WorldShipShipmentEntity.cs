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
	/// <summary>Entity class which represents the entity 'WorldShipShipment'.<br/><br/></summary>
	[Serializable]
	public partial class WorldShipShipmentEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<WorldShipGoodsEntity> _goods;
		private EntityCollection<WorldShipPackageEntity> _packages;
		private EntityCollection<WorldShipProcessedEntity> _worldShipProcessed;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name Goods</summary>
			public static readonly string Goods = "Goods";
			/// <summary>Member name Packages</summary>
			public static readonly string Packages = "Packages";
			/// <summary>Member name WorldShipProcessed</summary>
			public static readonly string WorldShipProcessed = "WorldShipProcessed";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static WorldShipShipmentEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public WorldShipShipmentEntity():base("WorldShipShipmentEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public WorldShipShipmentEntity(IEntityFields2 fields):base("WorldShipShipmentEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this WorldShipShipmentEntity</param>
		public WorldShipShipmentEntity(IValidator validator):base("WorldShipShipmentEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="shipmentID">PK value for WorldShipShipment which data should be fetched into this WorldShipShipment object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public WorldShipShipmentEntity(System.Int64 shipmentID):base("WorldShipShipmentEntity")
		{
			InitClassEmpty(null, null);
			this.ShipmentID = shipmentID;
		}

		/// <summary> CTor</summary>
		/// <param name="shipmentID">PK value for WorldShipShipment which data should be fetched into this WorldShipShipment object</param>
		/// <param name="validator">The custom validator object for this WorldShipShipmentEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public WorldShipShipmentEntity(System.Int64 shipmentID, IValidator validator):base("WorldShipShipmentEntity")
		{
			InitClassEmpty(validator, null);
			this.ShipmentID = shipmentID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected WorldShipShipmentEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_goods = (EntityCollection<WorldShipGoodsEntity>)info.GetValue("_goods", typeof(EntityCollection<WorldShipGoodsEntity>));
				_packages = (EntityCollection<WorldShipPackageEntity>)info.GetValue("_packages", typeof(EntityCollection<WorldShipPackageEntity>));
				_worldShipProcessed = (EntityCollection<WorldShipProcessedEntity>)info.GetValue("_worldShipProcessed", typeof(EntityCollection<WorldShipProcessedEntity>));
				this.FixupDeserialization(FieldInfoProviderSingleton.GetInstance());
			}
			// __LLBLGENPRO_USER_CODE_REGION_START DeserializationConstructor
			// __LLBLGENPRO_USER_CODE_REGION_END
		}

		
		/// <summary>Performs the desync setup when an FK field has been changed. The entity referenced based on the FK field will be dereferenced and sync info will be removed.</summary>
		/// <param name="fieldIndex">The fieldindex.</param>
		protected override void PerformDesyncSetupFKFieldChange(int fieldIndex)
		{
			switch((WorldShipShipmentFieldIndex)fieldIndex)
			{
				case WorldShipShipmentFieldIndex.ShipmentID:

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
				case "Goods":
					this.Goods.Add((WorldShipGoodsEntity)entity);
					break;
				case "Packages":
					this.Packages.Add((WorldShipPackageEntity)entity);
					break;
				case "WorldShipProcessed":
					this.WorldShipProcessed.Add((WorldShipProcessedEntity)entity);
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
				case "Goods":
					toReturn.Add(Relations.WorldShipGoodsEntityUsingShipmentID);
					break;
				case "Packages":
					toReturn.Add(Relations.WorldShipPackageEntityUsingShipmentID);
					break;
				case "WorldShipProcessed":
					toReturn.Add(Relations.WorldShipProcessedEntityUsingShipmentIdCalculated);
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
				case "Packages":
					this.Packages.Add((WorldShipPackageEntity)relatedEntity);
					break;
				case "WorldShipProcessed":
					this.WorldShipProcessed.Add((WorldShipProcessedEntity)relatedEntity);
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
				case "Packages":
					this.PerformRelatedEntityRemoval(this.Packages, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "WorldShipProcessed":
					this.PerformRelatedEntityRemoval(this.WorldShipProcessed, relatedEntity, signalRelatedEntityManyToOne);
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
			toReturn.Add(this.Goods);
			toReturn.Add(this.Packages);
			toReturn.Add(this.WorldShipProcessed);
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
				info.AddValue("_goods", ((_goods!=null) && (_goods.Count>0) && !this.MarkedForDeletion)?_goods:null);
				info.AddValue("_packages", ((_packages!=null) && (_packages.Count>0) && !this.MarkedForDeletion)?_packages:null);
				info.AddValue("_worldShipProcessed", ((_worldShipProcessed!=null) && (_worldShipProcessed.Count>0) && !this.MarkedForDeletion)?_worldShipProcessed:null);
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new WorldShipShipmentRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'WorldShipGoods' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoGoods()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(WorldShipGoodsFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'WorldShipPackage' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoPackages()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(WorldShipPackageFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'WorldShipProcessed' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoWorldShipProcessed()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(WorldShipProcessedFields.ShipmentIdCalculated, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(WorldShipShipmentEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._goods);
			collectionsQueue.Enqueue(this._packages);
			collectionsQueue.Enqueue(this._worldShipProcessed);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._goods = (EntityCollection<WorldShipGoodsEntity>) collectionsQueue.Dequeue();
			this._packages = (EntityCollection<WorldShipPackageEntity>) collectionsQueue.Dequeue();
			this._worldShipProcessed = (EntityCollection<WorldShipProcessedEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._goods != null);
			toReturn |=(this._packages != null);
			toReturn |=(this._worldShipProcessed != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<WorldShipGoodsEntity>(EntityFactoryCache2.GetEntityFactory(typeof(WorldShipGoodsEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<WorldShipPackageEntity>(EntityFactoryCache2.GetEntityFactory(typeof(WorldShipPackageEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<WorldShipProcessedEntity>(EntityFactoryCache2.GetEntityFactory(typeof(WorldShipProcessedEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("Goods", _goods);
			toReturn.Add("Packages", _packages);
			toReturn.Add("WorldShipProcessed", _worldShipProcessed);
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
			_fieldsCustomProperties.Add("ShipmentID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OrderNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FromCompanyOrName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FromAttention", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FromAddress1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FromAddress2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FromAddress3", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FromCountryCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FromPostalCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FromCity", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FromStateProvCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FromTelephone", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FromEmail", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FromAccountNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ToCustomerID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ToCompanyOrName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ToAttention", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ToAddress1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ToAddress2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ToAddress3", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ToCountryCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ToPostalCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ToCity", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ToStateProvCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ToTelephone", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ToEmail", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ToAccountNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ToResidential", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ServiceType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BillTransportationTo", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SaturdayDelivery", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("QvnOption", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("QvnFrom", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("QvnSubjectLine", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("QvnMemo", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn1ShipNotify", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn1DeliveryNotify", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn1ExceptionNotify", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn1ContactName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn1Email", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn2ShipNotify", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn2DeliveryNotify", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn2ExceptionNotify", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn2ContactName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn2Email", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn3ShipNotify", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn3DeliveryNotify", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn3ExceptionNotify", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn3ContactName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Qvn3Email", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CustomsDescriptionOfGoods", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CustomsDocumentsOnly", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipperNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PackageCount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DeliveryConfirmation", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DeliveryConfirmationAdult", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InvoiceTermsOfSale", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InvoiceReasonForExport", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InvoiceComments", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InvoiceCurrencyCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InvoiceChargesFreight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InvoiceChargesInsurance", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("InvoiceChargesOther", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipmentProcessedOnComputerID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("UspsEndorsement", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CarbonNeutral", fieldHashtable);
		}
		#endregion

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this WorldShipShipmentEntity</param>
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
		public  static WorldShipShipmentRelations Relations
		{
			get	{ return new WorldShipShipmentRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'WorldShipGoods' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathGoods
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<WorldShipGoodsEntity>(EntityFactoryCache2.GetEntityFactory(typeof(WorldShipGoodsEntityFactory))), (IEntityRelation)GetRelationsForField("Goods")[0], (int)ShipWorks.Data.Model.EntityType.WorldShipShipmentEntity, (int)ShipWorks.Data.Model.EntityType.WorldShipGoodsEntity, 0, null, null, null, null, "Goods", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'WorldShipPackage' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathPackages
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<WorldShipPackageEntity>(EntityFactoryCache2.GetEntityFactory(typeof(WorldShipPackageEntityFactory))), (IEntityRelation)GetRelationsForField("Packages")[0], (int)ShipWorks.Data.Model.EntityType.WorldShipShipmentEntity, (int)ShipWorks.Data.Model.EntityType.WorldShipPackageEntity, 0, null, null, null, null, "Packages", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'WorldShipProcessed' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathWorldShipProcessed
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<WorldShipProcessedEntity>(EntityFactoryCache2.GetEntityFactory(typeof(WorldShipProcessedEntityFactory))), (IEntityRelation)GetRelationsForField("WorldShipProcessed")[0], (int)ShipWorks.Data.Model.EntityType.WorldShipShipmentEntity, (int)ShipWorks.Data.Model.EntityType.WorldShipProcessedEntity, 0, null, null, null, null, "WorldShipProcessed", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
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

		/// <summary> The ShipmentID property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ShipmentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
		public virtual System.Int64 ShipmentID
		{
			get { return (System.Int64)GetValue((int)WorldShipShipmentFieldIndex.ShipmentID, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ShipmentID, value); }
		}

		/// <summary> The OrderNumber property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."OrderNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OrderNumber
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.OrderNumber, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.OrderNumber, value); }
		}

		/// <summary> The FromCompanyOrName property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."FromCompanyOrName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String FromCompanyOrName
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.FromCompanyOrName, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.FromCompanyOrName, value); }
		}

		/// <summary> The FromAttention property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."FromAttention"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String FromAttention
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.FromAttention, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.FromAttention, value); }
		}

		/// <summary> The FromAddress1 property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."FromAddress1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String FromAddress1
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.FromAddress1, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.FromAddress1, value); }
		}

		/// <summary> The FromAddress2 property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."FromAddress2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String FromAddress2
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.FromAddress2, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.FromAddress2, value); }
		}

		/// <summary> The FromAddress3 property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."FromAddress3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String FromAddress3
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.FromAddress3, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.FromAddress3, value); }
		}

		/// <summary> The FromCountryCode property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."FromCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String FromCountryCode
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.FromCountryCode, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.FromCountryCode, value); }
		}

		/// <summary> The FromPostalCode property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."FromPostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String FromPostalCode
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.FromPostalCode, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.FromPostalCode, value); }
		}

		/// <summary> The FromCity property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."FromCity"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String FromCity
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.FromCity, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.FromCity, value); }
		}

		/// <summary> The FromStateProvCode property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."FromStateProvCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String FromStateProvCode
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.FromStateProvCode, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.FromStateProvCode, value); }
		}

		/// <summary> The FromTelephone property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."FromTelephone"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String FromTelephone
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.FromTelephone, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.FromTelephone, value); }
		}

		/// <summary> The FromEmail property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."FromEmail"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String FromEmail
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.FromEmail, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.FromEmail, value); }
		}

		/// <summary> The FromAccountNumber property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."FromAccountNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String FromAccountNumber
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.FromAccountNumber, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.FromAccountNumber, value); }
		}

		/// <summary> The ToCustomerID property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ToCustomerID"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ToCustomerID
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.ToCustomerID, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ToCustomerID, value); }
		}

		/// <summary> The ToCompanyOrName property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ToCompanyOrName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ToCompanyOrName
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.ToCompanyOrName, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ToCompanyOrName, value); }
		}

		/// <summary> The ToAttention property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ToAttention"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ToAttention
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.ToAttention, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ToAttention, value); }
		}

		/// <summary> The ToAddress1 property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ToAddress1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ToAddress1
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.ToAddress1, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ToAddress1, value); }
		}

		/// <summary> The ToAddress2 property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ToAddress2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ToAddress2
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.ToAddress2, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ToAddress2, value); }
		}

		/// <summary> The ToAddress3 property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ToAddress3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ToAddress3
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.ToAddress3, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ToAddress3, value); }
		}

		/// <summary> The ToCountryCode property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ToCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ToCountryCode
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.ToCountryCode, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ToCountryCode, value); }
		}

		/// <summary> The ToPostalCode property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ToPostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ToPostalCode
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.ToPostalCode, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ToPostalCode, value); }
		}

		/// <summary> The ToCity property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ToCity"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ToCity
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.ToCity, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ToCity, value); }
		}

		/// <summary> The ToStateProvCode property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ToStateProvCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ToStateProvCode
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.ToStateProvCode, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ToStateProvCode, value); }
		}

		/// <summary> The ToTelephone property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ToTelephone"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ToTelephone
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.ToTelephone, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ToTelephone, value); }
		}

		/// <summary> The ToEmail property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ToEmail"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ToEmail
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.ToEmail, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ToEmail, value); }
		}

		/// <summary> The ToAccountNumber property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ToAccountNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ToAccountNumber
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.ToAccountNumber, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ToAccountNumber, value); }
		}

		/// <summary> The ToResidential property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ToResidential"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ToResidential
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.ToResidential, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ToResidential, value); }
		}

		/// <summary> The ServiceType property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ServiceType"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 3<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ServiceType
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.ServiceType, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ServiceType, value); }
		}

		/// <summary> The BillTransportationTo property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."BillTransportationTo"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BillTransportationTo
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.BillTransportationTo, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.BillTransportationTo, value); }
		}

		/// <summary> The SaturdayDelivery property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."SaturdayDelivery"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String SaturdayDelivery
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.SaturdayDelivery, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.SaturdayDelivery, value); }
		}

		/// <summary> The QvnOption property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."QvnOption"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String QvnOption
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.QvnOption, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.QvnOption, value); }
		}

		/// <summary> The QvnFrom property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."QvnFrom"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String QvnFrom
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.QvnFrom, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.QvnFrom, value); }
		}

		/// <summary> The QvnSubjectLine property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."QvnSubjectLine"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 18<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String QvnSubjectLine
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.QvnSubjectLine, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.QvnSubjectLine, value); }
		}

		/// <summary> The QvnMemo property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."QvnMemo"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String QvnMemo
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.QvnMemo, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.QvnMemo, value); }
		}

		/// <summary> The Qvn1ShipNotify property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."Qvn1ShipNotify"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Qvn1ShipNotify
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.Qvn1ShipNotify, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.Qvn1ShipNotify, value); }
		}

		/// <summary> The Qvn1DeliveryNotify property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."Qvn1DeliveryNotify"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Qvn1DeliveryNotify
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.Qvn1DeliveryNotify, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.Qvn1DeliveryNotify, value); }
		}

		/// <summary> The Qvn1ExceptionNotify property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."Qvn1ExceptionNotify"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Qvn1ExceptionNotify
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.Qvn1ExceptionNotify, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.Qvn1ExceptionNotify, value); }
		}

		/// <summary> The Qvn1ContactName property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."Qvn1ContactName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Qvn1ContactName
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.Qvn1ContactName, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.Qvn1ContactName, value); }
		}

		/// <summary> The Qvn1Email property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."Qvn1Email"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Qvn1Email
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.Qvn1Email, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.Qvn1Email, value); }
		}

		/// <summary> The Qvn2ShipNotify property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."Qvn2ShipNotify"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Qvn2ShipNotify
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.Qvn2ShipNotify, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.Qvn2ShipNotify, value); }
		}

		/// <summary> The Qvn2DeliveryNotify property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."Qvn2DeliveryNotify"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Qvn2DeliveryNotify
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.Qvn2DeliveryNotify, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.Qvn2DeliveryNotify, value); }
		}

		/// <summary> The Qvn2ExceptionNotify property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."Qvn2ExceptionNotify"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Qvn2ExceptionNotify
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.Qvn2ExceptionNotify, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.Qvn2ExceptionNotify, value); }
		}

		/// <summary> The Qvn2ContactName property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."Qvn2ContactName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Qvn2ContactName
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.Qvn2ContactName, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.Qvn2ContactName, value); }
		}

		/// <summary> The Qvn2Email property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."Qvn2Email"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Qvn2Email
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.Qvn2Email, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.Qvn2Email, value); }
		}

		/// <summary> The Qvn3ShipNotify property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."Qvn3ShipNotify"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Qvn3ShipNotify
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.Qvn3ShipNotify, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.Qvn3ShipNotify, value); }
		}

		/// <summary> The Qvn3DeliveryNotify property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."Qvn3DeliveryNotify"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Qvn3DeliveryNotify
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.Qvn3DeliveryNotify, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.Qvn3DeliveryNotify, value); }
		}

		/// <summary> The Qvn3ExceptionNotify property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."Qvn3ExceptionNotify"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Qvn3ExceptionNotify
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.Qvn3ExceptionNotify, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.Qvn3ExceptionNotify, value); }
		}

		/// <summary> The Qvn3ContactName property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."Qvn3ContactName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Qvn3ContactName
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.Qvn3ContactName, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.Qvn3ContactName, value); }
		}

		/// <summary> The Qvn3Email property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."Qvn3Email"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Qvn3Email
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.Qvn3Email, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.Qvn3Email, value); }
		}

		/// <summary> The CustomsDescriptionOfGoods property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."CustomsDescriptionOfGoods"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String CustomsDescriptionOfGoods
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.CustomsDescriptionOfGoods, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.CustomsDescriptionOfGoods, value); }
		}

		/// <summary> The CustomsDocumentsOnly property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."CustomsDocumentsOnly"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String CustomsDocumentsOnly
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.CustomsDocumentsOnly, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.CustomsDocumentsOnly, value); }
		}

		/// <summary> The ShipperNumber property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ShipperNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipperNumber
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.ShipperNumber, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ShipperNumber, value); }
		}

		/// <summary> The PackageCount property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."PackageCount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 PackageCount
		{
			get { return (System.Int32)GetValue((int)WorldShipShipmentFieldIndex.PackageCount, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.PackageCount, value); }
		}

		/// <summary> The DeliveryConfirmation property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."DeliveryConfirmation"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String DeliveryConfirmation
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.DeliveryConfirmation, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.DeliveryConfirmation, value); }
		}

		/// <summary> The DeliveryConfirmationAdult property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."DeliveryConfirmationAdult"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 1<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String DeliveryConfirmationAdult
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.DeliveryConfirmationAdult, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.DeliveryConfirmationAdult, value); }
		}

		/// <summary> The InvoiceTermsOfSale property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."InvoiceTermsOfSale"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 3<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String InvoiceTermsOfSale
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.InvoiceTermsOfSale, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.InvoiceTermsOfSale, value); }
		}

		/// <summary> The InvoiceReasonForExport property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."InvoiceReasonForExport"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 2<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String InvoiceReasonForExport
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.InvoiceReasonForExport, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.InvoiceReasonForExport, value); }
		}

		/// <summary> The InvoiceComments property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."InvoiceComments"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 200<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String InvoiceComments
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.InvoiceComments, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.InvoiceComments, value); }
		}

		/// <summary> The InvoiceCurrencyCode property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."InvoiceCurrencyCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 3<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String InvoiceCurrencyCode
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.InvoiceCurrencyCode, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.InvoiceCurrencyCode, value); }
		}

		/// <summary> The InvoiceChargesFreight property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."InvoiceChargesFreight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Decimal> InvoiceChargesFreight
		{
			get { return (Nullable<System.Decimal>)GetValue((int)WorldShipShipmentFieldIndex.InvoiceChargesFreight, false); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.InvoiceChargesFreight, value); }
		}

		/// <summary> The InvoiceChargesInsurance property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."InvoiceChargesInsurance"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Decimal> InvoiceChargesInsurance
		{
			get { return (Nullable<System.Decimal>)GetValue((int)WorldShipShipmentFieldIndex.InvoiceChargesInsurance, false); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.InvoiceChargesInsurance, value); }
		}

		/// <summary> The InvoiceChargesOther property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."InvoiceChargesOther"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Decimal> InvoiceChargesOther
		{
			get { return (Nullable<System.Decimal>)GetValue((int)WorldShipShipmentFieldIndex.InvoiceChargesOther, false); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.InvoiceChargesOther, value); }
		}

		/// <summary> The ShipmentProcessedOnComputerID property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."ShipmentProcessedOnComputerID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> ShipmentProcessedOnComputerID
		{
			get { return (Nullable<System.Int64>)GetValue((int)WorldShipShipmentFieldIndex.ShipmentProcessedOnComputerID, false); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.ShipmentProcessedOnComputerID, value); }
		}

		/// <summary> The UspsEndorsement property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."UspsEndorsement"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String UspsEndorsement
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.UspsEndorsement, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.UspsEndorsement, value); }
		}

		/// <summary> The CarbonNeutral property of the Entity WorldShipShipment<br/><br/></summary>
		/// <remarks>Mapped on  table field: "WorldShipShipment"."CarbonNeutral"<br/>
		/// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 10<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String CarbonNeutral
		{
			get { return (System.String)GetValue((int)WorldShipShipmentFieldIndex.CarbonNeutral, true); }
			set	{ SetValue((int)WorldShipShipmentFieldIndex.CarbonNeutral, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'WorldShipGoodsEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(WorldShipGoodsEntity))]
		public virtual EntityCollection<WorldShipGoodsEntity> Goods
		{
			get { return GetOrCreateEntityCollection<WorldShipGoodsEntity, WorldShipGoodsEntityFactory>("", false, false, ref _goods);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'WorldShipPackageEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(WorldShipPackageEntity))]
		public virtual EntityCollection<WorldShipPackageEntity> Packages
		{
			get { return GetOrCreateEntityCollection<WorldShipPackageEntity, WorldShipPackageEntityFactory>("WorldShipShipment", true, false, ref _packages);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'WorldShipProcessedEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(WorldShipProcessedEntity))]
		public virtual EntityCollection<WorldShipProcessedEntity> WorldShipProcessed
		{
			get { return GetOrCreateEntityCollection<WorldShipProcessedEntity, WorldShipProcessedEntityFactory>("WorldShipShipment", true, false, ref _worldShipProcessed);	}
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
			get { return (int)ShipWorks.Data.Model.EntityType.WorldShipShipmentEntity; }
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
