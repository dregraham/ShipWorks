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
	/// Entity class which represents the entity 'UpsPackage'.<br/><br/>
	/// 
	/// </summary>
	[Serializable]
	public partial class UpsPackageEntity : CommonEntityBase, ISerializable
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations


		private UpsShipmentEntity _upsShipment;

		
		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name UpsShipment</summary>
			public static readonly string UpsShipment = "UpsShipment";



		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static UpsPackageEntity()
		{
			SetupCustomPropertyHashtables();
		}

		/// <summary> CTor</summary>
		public UpsPackageEntity():base("UpsPackageEntity")
		{
			InitClassEmpty(null, CreateFields());
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public UpsPackageEntity(IEntityFields2 fields):base("UpsPackageEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this UpsPackageEntity</param>
		public UpsPackageEntity(IValidator validator):base("UpsPackageEntity")
		{
			InitClassEmpty(validator, CreateFields());
		}
				

		/// <summary> CTor</summary>
		/// <param name="upsPackageID">PK value for UpsPackage which data should be fetched into this UpsPackage object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public UpsPackageEntity(System.Int64 upsPackageID):base("UpsPackageEntity")
		{
			InitClassEmpty(null, CreateFields());
			this.UpsPackageID = upsPackageID;
		}

		/// <summary> CTor</summary>
		/// <param name="upsPackageID">PK value for UpsPackage which data should be fetched into this UpsPackage object</param>
		/// <param name="validator">The custom validator object for this UpsPackageEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public UpsPackageEntity(System.Int64 upsPackageID, IValidator validator):base("UpsPackageEntity")
		{
			InitClassEmpty(validator, CreateFields());
			this.UpsPackageID = upsPackageID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected UpsPackageEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{


				_upsShipment = (UpsShipmentEntity)info.GetValue("_upsShipment", typeof(UpsShipmentEntity));
				if(_upsShipment!=null)
				{
					_upsShipment.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((UpsPackageFieldIndex)fieldIndex)
			{
				case UpsPackageFieldIndex.ShipmentID:
					DesetupSyncUpsShipment(true, false);
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
				case "UpsShipment":
					this.UpsShipment = (UpsShipmentEntity)entity;
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
			return UpsPackageEntity.GetRelationsForField(fieldName);
		}

		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		public static RelationCollection GetRelationsForField(string fieldName)
		{
			RelationCollection toReturn = new RelationCollection();
			switch(fieldName)
			{
				case "UpsShipment":
					toReturn.Add(UpsPackageEntity.Relations.UpsShipmentEntityUsingShipmentID);
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
				case "UpsShipment":
					SetupSyncUpsShipment(relatedEntity);
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
				case "UpsShipment":
					DesetupSyncUpsShipment(false, true);
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
			if(_upsShipment!=null)
			{
				toReturn.Add(_upsShipment);
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


				info.AddValue("_upsShipment", (!this.MarkedForDeletion?_upsShipment:null));

			}
			
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}

		/// <summary>Returns true if the original value for the field with the fieldIndex passed in, read from the persistent storage was NULL, false otherwise.
		/// Should not be used for testing if the current value is NULL, use <see cref="TestCurrentFieldValueForNull"/> for that.</summary>
		/// <param name="fieldIndex">Index of the field to test if that field was NULL in the persistent storage</param>
		/// <returns>true if the field with the passed in index was NULL in the persistent storage, false otherwise</returns>
		public bool TestOriginalFieldValueForNull(UpsPackageFieldIndex fieldIndex)
		{
			return base.Fields[(int)fieldIndex].IsNull;
		}
		
		/// <summary>Returns true if the current value for the field with the fieldIndex passed in represents null/not defined, false otherwise.
		/// Should not be used for testing if the original value (read from the db) is NULL</summary>
		/// <param name="fieldIndex">Index of the field to test if its currentvalue is null/undefined</param>
		/// <returns>true if the field's value isn't defined yet, false otherwise</returns>
		public bool TestCurrentFieldValueForNull(UpsPackageFieldIndex fieldIndex)
		{
			return base.CheckIfCurrentFieldValueIsNull((int)fieldIndex);
		}

				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			return new UpsPackageRelations().GetAllRelations();
		}
		



		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'UpsShipment' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUpsShipment()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UpsShipmentFields.ShipmentID, null, ComparisonOperator.Equal, this.ShipmentID));
			return bucket;
		}

	
		
		/// <summary>Creates entity fields object for this entity. Used in constructor to setup this entity in a polymorphic scenario.</summary>
		protected virtual IEntityFields2 CreateFields()
		{
			return EntityFieldsFactory.CreateEntityFieldsObject(ShipWorks.Data.Model.EntityType.UpsPackageEntity);
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
			return EntityFactoryCache2.GetEntityFactory(typeof(UpsPackageEntityFactory));
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
			toReturn.Add("UpsShipment", _upsShipment);



			return toReturn;
		}
		
		/// <summary> Adds the internals to the active context. </summary>
		protected override void AddInternalsToContext()
		{


			if(_upsShipment!=null)
			{
				_upsShipment.ActiveContext = base.ActiveContext;
			}

		}

		/// <summary> Initializes the class members</summary>
		protected virtual void InitClassMembers()
		{



			_upsShipment = null;

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

			_fieldsCustomProperties.Add("UpsPackageID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ShipmentID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PackagingType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Weight", fieldHashtable);
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

			_fieldsCustomProperties.Add("Insurance", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("InsuranceValue", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("InsurancePennyOne", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DeclaredValue", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("TrackingNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("UspsTrackingNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("AdditionalHandlingEnabled", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("VerbalConfirmationName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("VerbalConfirmationPhone", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("VerbalConfirmationPhoneExtension", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DryIceRegulationSet", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DryIceWeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DryIceIsForMedicalUse", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _upsShipment</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncUpsShipment(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _upsShipment, new PropertyChangedEventHandler( OnUpsShipmentPropertyChanged ), "UpsShipment", UpsPackageEntity.Relations.UpsShipmentEntityUsingShipmentID, true, signalRelatedEntity, "Packages", resetFKFields, new int[] { (int)UpsPackageFieldIndex.ShipmentID } );		
			_upsShipment = null;
		}

		/// <summary> setups the sync logic for member _upsShipment</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncUpsShipment(IEntity2 relatedEntity)
		{
			if(_upsShipment!=relatedEntity)
			{
				DesetupSyncUpsShipment(true, true);
				_upsShipment = (UpsShipmentEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _upsShipment, new PropertyChangedEventHandler( OnUpsShipmentPropertyChanged ), "UpsShipment", UpsPackageEntity.Relations.UpsShipmentEntityUsingShipmentID, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnUpsShipmentPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}


		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this UpsPackageEntity</param>
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
		public  static UpsPackageRelations Relations
		{
			get	{ return new UpsPackageRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}



		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UpsShipment' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUpsShipment
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(UpsShipmentEntityFactory))),
					(IEntityRelation)GetRelationsForField("UpsShipment")[0], (int)ShipWorks.Data.Model.EntityType.UpsPackageEntity, (int)ShipWorks.Data.Model.EntityType.UpsShipmentEntity, 0, null, null, null, null, "UpsShipment", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne);
			}
		}


		/// <summary> The custom properties for the type of this entity instance.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		public override Dictionary<string, string> CustomPropertiesOfType
		{
			get { return UpsPackageEntity.CustomProperties;}
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
			get { return UpsPackageEntity.FieldsCustomProperties;}
		}

		/// <summary> The UpsPackageID property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."UpsPackageID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 UpsPackageID
		{
			get { return (System.Int64)GetValue((int)UpsPackageFieldIndex.UpsPackageID, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.UpsPackageID, value); }
		}

		/// <summary> The ShipmentID property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."ShipmentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 ShipmentID
		{
			get { return (System.Int64)GetValue((int)UpsPackageFieldIndex.ShipmentID, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.ShipmentID, value); }
		}

		/// <summary> The PackagingType property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."PackagingType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 PackagingType
		{
			get { return (System.Int32)GetValue((int)UpsPackageFieldIndex.PackagingType, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.PackagingType, value); }
		}

		/// <summary> The Weight property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."Weight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double Weight
		{
			get { return (System.Double)GetValue((int)UpsPackageFieldIndex.Weight, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.Weight, value); }
		}

		/// <summary> The DimsProfileID property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."DimsProfileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 DimsProfileID
		{
			get { return (System.Int64)GetValue((int)UpsPackageFieldIndex.DimsProfileID, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.DimsProfileID, value); }
		}

		/// <summary> The DimsLength property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."DimsLength"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DimsLength
		{
			get { return (System.Double)GetValue((int)UpsPackageFieldIndex.DimsLength, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.DimsLength, value); }
		}

		/// <summary> The DimsWidth property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."DimsWidth"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DimsWidth
		{
			get { return (System.Double)GetValue((int)UpsPackageFieldIndex.DimsWidth, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.DimsWidth, value); }
		}

		/// <summary> The DimsHeight property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."DimsHeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DimsHeight
		{
			get { return (System.Double)GetValue((int)UpsPackageFieldIndex.DimsHeight, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.DimsHeight, value); }
		}

		/// <summary> The DimsWeight property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."DimsWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DimsWeight
		{
			get { return (System.Double)GetValue((int)UpsPackageFieldIndex.DimsWeight, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.DimsWeight, value); }
		}

		/// <summary> The DimsAddWeight property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."DimsAddWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean DimsAddWeight
		{
			get { return (System.Boolean)GetValue((int)UpsPackageFieldIndex.DimsAddWeight, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.DimsAddWeight, value); }
		}

		/// <summary> The Insurance property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."Insurance"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Insurance
		{
			get { return (System.Boolean)GetValue((int)UpsPackageFieldIndex.Insurance, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.Insurance, value); }
		}

		/// <summary> The InsuranceValue property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."InsuranceValue"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal InsuranceValue
		{
			get { return (System.Decimal)GetValue((int)UpsPackageFieldIndex.InsuranceValue, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.InsuranceValue, value); }
		}

		/// <summary> The InsurancePennyOne property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."InsurancePennyOne"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean InsurancePennyOne
		{
			get { return (System.Boolean)GetValue((int)UpsPackageFieldIndex.InsurancePennyOne, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.InsurancePennyOne, value); }
		}

		/// <summary> The DeclaredValue property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."DeclaredValue"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Decimal DeclaredValue
		{
			get { return (System.Decimal)GetValue((int)UpsPackageFieldIndex.DeclaredValue, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.DeclaredValue, value); }
		}

		/// <summary> The TrackingNumber property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."TrackingNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String TrackingNumber
		{
			get { return (System.String)GetValue((int)UpsPackageFieldIndex.TrackingNumber, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.TrackingNumber, value); }
		}

		/// <summary> The UspsTrackingNumber property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."UspsTrackingNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String UspsTrackingNumber
		{
			get { return (System.String)GetValue((int)UpsPackageFieldIndex.UspsTrackingNumber, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.UspsTrackingNumber, value); }
		}

		/// <summary> The AdditionalHandlingEnabled property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."AdditionalHandlingEnabled"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean AdditionalHandlingEnabled
		{
			get { return (System.Boolean)GetValue((int)UpsPackageFieldIndex.AdditionalHandlingEnabled, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.AdditionalHandlingEnabled, value); }
		}

		/// <summary> The VerbalConfirmationName property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."VerbalConfirmationName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String VerbalConfirmationName
		{
			get { return (System.String)GetValue((int)UpsPackageFieldIndex.VerbalConfirmationName, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.VerbalConfirmationName, value); }
		}

		/// <summary> The VerbalConfirmationPhone property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."VerbalConfirmationPhone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 15<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String VerbalConfirmationPhone
		{
			get { return (System.String)GetValue((int)UpsPackageFieldIndex.VerbalConfirmationPhone, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.VerbalConfirmationPhone, value); }
		}

		/// <summary> The VerbalConfirmationPhoneExtension property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."VerbalConfirmationPhoneExtension"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 4<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String VerbalConfirmationPhoneExtension
		{
			get { return (System.String)GetValue((int)UpsPackageFieldIndex.VerbalConfirmationPhoneExtension, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.VerbalConfirmationPhoneExtension, value); }
		}

		/// <summary> The DryIceRegulationSet property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."DryIceRegulationSet"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 DryIceRegulationSet
		{
			get { return (System.Int32)GetValue((int)UpsPackageFieldIndex.DryIceRegulationSet, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.DryIceRegulationSet, value); }
		}

		/// <summary> The DryIceWeight property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."DryIceWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double DryIceWeight
		{
			get { return (System.Double)GetValue((int)UpsPackageFieldIndex.DryIceWeight, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.DryIceWeight, value); }
		}

		/// <summary> The DryIceIsForMedicalUse property of the Entity UpsPackage<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "UpsPackage"."DryIceIsForMedicalUse"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean DryIceIsForMedicalUse
		{
			get { return (System.Boolean)GetValue((int)UpsPackageFieldIndex.DryIceIsForMedicalUse, true); }
			set	{ SetValue((int)UpsPackageFieldIndex.DryIceIsForMedicalUse, value); }
		}



		/// <summary> Gets / sets related entity of type 'UpsShipmentEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual UpsShipmentEntity UpsShipment
		{
			get
			{
				return _upsShipment;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncUpsShipment(value);
				}
				else
				{
					if(value==null)
					{
						if(_upsShipment != null)
						{
							_upsShipment.UnsetRelatedEntity(this, "Packages");
						}
					}
					else
					{
						if(_upsShipment!=value)
						{
							((IEntity2)value).SetRelatedEntity(this, "Packages");
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
			get { return (int)ShipWorks.Data.Model.EntityType.UpsPackageEntity; }
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
