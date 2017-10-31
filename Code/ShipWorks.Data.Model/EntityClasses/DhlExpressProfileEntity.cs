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
	/// <summary>Entity class which represents the entity 'DhlExpressProfile'.<br/><br/></summary>
	[Serializable]
	public partial class DhlExpressProfileEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<DhlExpressProfilePackageEntity> _packages;
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
		static DhlExpressProfileEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public DhlExpressProfileEntity():base("DhlExpressProfileEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public DhlExpressProfileEntity(IEntityFields2 fields):base("DhlExpressProfileEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this DhlExpressProfileEntity</param>
		public DhlExpressProfileEntity(IValidator validator):base("DhlExpressProfileEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="shippingProfileID">PK value for DhlExpressProfile which data should be fetched into this DhlExpressProfile object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public DhlExpressProfileEntity(System.Int64 shippingProfileID):base("DhlExpressProfileEntity")
		{
			InitClassEmpty(null, null);
			this.ShippingProfileID = shippingProfileID;
		}

		/// <summary> CTor</summary>
		/// <param name="shippingProfileID">PK value for DhlExpressProfile which data should be fetched into this DhlExpressProfile object</param>
		/// <param name="validator">The custom validator object for this DhlExpressProfileEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public DhlExpressProfileEntity(System.Int64 shippingProfileID, IValidator validator):base("DhlExpressProfileEntity")
		{
			InitClassEmpty(validator, null);
			this.ShippingProfileID = shippingProfileID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected DhlExpressProfileEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_packages = (EntityCollection<DhlExpressProfilePackageEntity>)info.GetValue("_packages", typeof(EntityCollection<DhlExpressProfilePackageEntity>));
				_shippingProfile = (ShippingProfileEntity)info.GetValue("_shippingProfile", typeof(ShippingProfileEntity));
				if(_shippingProfile!=null)
				{
					_shippingProfile.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((DhlExpressProfileFieldIndex)fieldIndex)
			{
				case DhlExpressProfileFieldIndex.ShippingProfileID:
					DesetupSyncShippingProfile(true, false);
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
				case "Packages":
					this.Packages.Add((DhlExpressProfilePackageEntity)entity);
					break;
				case "ShippingProfile":
					this.ShippingProfile = (ShippingProfileEntity)entity;
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
				case "Packages":
					toReturn.Add(Relations.DhlExpressProfilePackageEntityUsingShippingProfileID);
					break;
				case "ShippingProfile":
					toReturn.Add(Relations.ShippingProfileEntityUsingShippingProfileID);
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
					this.Packages.Add((DhlExpressProfilePackageEntity)relatedEntity);
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
		protected override void UnsetRelatedEntity(IEntityCore relatedEntity, string fieldName, bool signalRelatedEntityManyToOne)
		{
			switch(fieldName)
			{
				case "Packages":
					this.PerformRelatedEntityRemoval(this.Packages, relatedEntity, signalRelatedEntityManyToOne);
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
			if(_shippingProfile!=null)
			{
				toReturn.Add(_shippingProfile);
			}

			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.Add(this.Packages);
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
				info.AddValue("_packages", ((_packages!=null) && (_packages.Count>0) && !this.MarkedForDeletion)?_packages:null);
				info.AddValue("_shippingProfile", (!this.MarkedForDeletion?_shippingProfile:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new DhlExpressProfileRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'DhlExpressProfilePackage' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoPackages()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(DhlExpressProfilePackageFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'ShippingProfile' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoShippingProfile()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ShippingProfileFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(DhlExpressProfileEntityFactory));
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
			this._packages = (EntityCollection<DhlExpressProfilePackageEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._packages != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<DhlExpressProfilePackageEntity>(EntityFactoryCache2.GetEntityFactory(typeof(DhlExpressProfilePackageEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("Packages", _packages);
			toReturn.Add("ShippingProfile", _shippingProfile);
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
			_fieldsCustomProperties.Add("ShippingProfileID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShipEngineAccountID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Service", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DeliveryDutyPaid", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("NonMachinable", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SaturdayDelivery", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Contents", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("NonDelivery", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _shippingProfile</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncShippingProfile(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _shippingProfile, new PropertyChangedEventHandler( OnShippingProfilePropertyChanged ), "ShippingProfile", ShipWorks.Data.Model.RelationClasses.StaticDhlExpressProfileRelations.ShippingProfileEntityUsingShippingProfileIDStatic, true, signalRelatedEntity, "DhlExpress", false, new int[] { (int)DhlExpressProfileFieldIndex.ShippingProfileID } );
			_shippingProfile = null;
		}
		
		/// <summary> setups the sync logic for member _shippingProfile</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncShippingProfile(IEntityCore relatedEntity)
		{
			if(_shippingProfile!=relatedEntity)
			{
				DesetupSyncShippingProfile(true, true);
				_shippingProfile = (ShippingProfileEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _shippingProfile, new PropertyChangedEventHandler( OnShippingProfilePropertyChanged ), "ShippingProfile", ShipWorks.Data.Model.RelationClasses.StaticDhlExpressProfileRelations.ShippingProfileEntityUsingShippingProfileIDStatic, true, new string[] {  } );
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
		/// <param name="validator">The validator object for this DhlExpressProfileEntity</param>
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
		public  static DhlExpressProfileRelations Relations
		{
			get	{ return new DhlExpressProfileRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'DhlExpressProfilePackage' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathPackages
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<DhlExpressProfilePackageEntity>(EntityFactoryCache2.GetEntityFactory(typeof(DhlExpressProfilePackageEntityFactory))), (IEntityRelation)GetRelationsForField("Packages")[0], (int)ShipWorks.Data.Model.EntityType.DhlExpressProfileEntity, (int)ShipWorks.Data.Model.EntityType.DhlExpressProfilePackageEntity, 0, null, null, null, null, "Packages", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ShippingProfile' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathShippingProfile
		{
			get { return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(ShippingProfileEntityFactory))), (IEntityRelation)GetRelationsForField("ShippingProfile")[0], (int)ShipWorks.Data.Model.EntityType.DhlExpressProfileEntity, (int)ShipWorks.Data.Model.EntityType.ShippingProfileEntity, 0, null, null, null, null, "ShippingProfile", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);	}
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

		/// <summary> The ShippingProfileID property of the Entity DhlExpressProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlExpressProfile"."ShippingProfileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
		public virtual System.Int64 ShippingProfileID
		{
			get { return (System.Int64)GetValue((int)DhlExpressProfileFieldIndex.ShippingProfileID, true); }
			set	{ SetValue((int)DhlExpressProfileFieldIndex.ShippingProfileID, value); }
		}

		/// <summary> The ShipEngineAccountID property of the Entity DhlExpressProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlExpressProfile"."ShipEngineAccountID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> ShipEngineAccountID
		{
			get { return (Nullable<System.Int64>)GetValue((int)DhlExpressProfileFieldIndex.ShipEngineAccountID, false); }
			set	{ SetValue((int)DhlExpressProfileFieldIndex.ShipEngineAccountID, value); }
		}

		/// <summary> The Service property of the Entity DhlExpressProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlExpressProfile"."Service"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> Service
		{
			get { return (Nullable<System.Int32>)GetValue((int)DhlExpressProfileFieldIndex.Service, false); }
			set	{ SetValue((int)DhlExpressProfileFieldIndex.Service, value); }
		}

		/// <summary> The DeliveryDutyPaid property of the Entity DhlExpressProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlExpressProfile"."DeliveryDutyPaid"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> DeliveryDutyPaid
		{
			get { return (Nullable<System.Boolean>)GetValue((int)DhlExpressProfileFieldIndex.DeliveryDutyPaid, false); }
			set	{ SetValue((int)DhlExpressProfileFieldIndex.DeliveryDutyPaid, value); }
		}

		/// <summary> The NonMachinable property of the Entity DhlExpressProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlExpressProfile"."NonMachinable"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> NonMachinable
		{
			get { return (Nullable<System.Boolean>)GetValue((int)DhlExpressProfileFieldIndex.NonMachinable, false); }
			set	{ SetValue((int)DhlExpressProfileFieldIndex.NonMachinable, value); }
		}

		/// <summary> The SaturdayDelivery property of the Entity DhlExpressProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlExpressProfile"."SaturdayDelivery"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> SaturdayDelivery
		{
			get { return (Nullable<System.Boolean>)GetValue((int)DhlExpressProfileFieldIndex.SaturdayDelivery, false); }
			set	{ SetValue((int)DhlExpressProfileFieldIndex.SaturdayDelivery, value); }
		}

		/// <summary> The Contents property of the Entity DhlExpressProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlExpressProfile"."Contents"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> Contents
		{
			get { return (Nullable<System.Int32>)GetValue((int)DhlExpressProfileFieldIndex.Contents, false); }
			set	{ SetValue((int)DhlExpressProfileFieldIndex.Contents, value); }
		}

		/// <summary> The NonDelivery property of the Entity DhlExpressProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "DhlExpressProfile"."NonDelivery"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> NonDelivery
		{
			get { return (Nullable<System.Int32>)GetValue((int)DhlExpressProfileFieldIndex.NonDelivery, false); }
			set	{ SetValue((int)DhlExpressProfileFieldIndex.NonDelivery, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'DhlExpressProfilePackageEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(DhlExpressProfilePackageEntity))]
		public virtual EntityCollection<DhlExpressProfilePackageEntity> Packages
		{
			get { return GetOrCreateEntityCollection<DhlExpressProfilePackageEntity, DhlExpressProfilePackageEntityFactory>("DhlExpressProfile", true, false, ref _packages);	}
		}

		/// <summary> Gets / sets related entity of type 'ShippingProfileEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned.<br/><br/>
		/// </summary>
		[Browsable(true)]
		public virtual ShippingProfileEntity ShippingProfile
		{
			get { return _shippingProfile; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncShippingProfile(value);
					CallSetRelatedEntityDuringDeserialization(value, "DhlExpress");
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
							((IEntity2)value).SetRelatedEntity(this, "DhlExpress");
							SetupSyncShippingProfile(value);
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
		protected override int LLBLGenProEntityTypeValue 
		{ 
			get { return (int)ShipWorks.Data.Model.EntityType.DhlExpressProfileEntity; }
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
