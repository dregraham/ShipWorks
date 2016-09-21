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
	/// <summary>Entity class which represents the entity 'UpsProfilePackage'.<br/><br/></summary>
	[Serializable]
	public partial class UpsProfilePackageEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private UpsProfileEntity _upsProfile;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name UpsProfile</summary>
			public static readonly string UpsProfile = "UpsProfile";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static UpsProfilePackageEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public UpsProfilePackageEntity():base("UpsProfilePackageEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public UpsProfilePackageEntity(IEntityFields2 fields):base("UpsProfilePackageEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this UpsProfilePackageEntity</param>
		public UpsProfilePackageEntity(IValidator validator):base("UpsProfilePackageEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="upsProfilePackageID">PK value for UpsProfilePackage which data should be fetched into this UpsProfilePackage object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public UpsProfilePackageEntity(System.Int64 upsProfilePackageID):base("UpsProfilePackageEntity")
		{
			InitClassEmpty(null, null);
			this.UpsProfilePackageID = upsProfilePackageID;
		}

		/// <summary> CTor</summary>
		/// <param name="upsProfilePackageID">PK value for UpsProfilePackage which data should be fetched into this UpsProfilePackage object</param>
		/// <param name="validator">The custom validator object for this UpsProfilePackageEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public UpsProfilePackageEntity(System.Int64 upsProfilePackageID, IValidator validator):base("UpsProfilePackageEntity")
		{
			InitClassEmpty(validator, null);
			this.UpsProfilePackageID = upsProfilePackageID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected UpsProfilePackageEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_upsProfile = (UpsProfileEntity)info.GetValue("_upsProfile", typeof(UpsProfileEntity));
				if(_upsProfile!=null)
				{
					_upsProfile.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((UpsProfilePackageFieldIndex)fieldIndex)
			{
				case UpsProfilePackageFieldIndex.ShippingProfileID:
					DesetupSyncUpsProfile(true, false);
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
				case "UpsProfile":
					this.UpsProfile = (UpsProfileEntity)entity;
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
				case "UpsProfile":
					toReturn.Add(Relations.UpsProfileEntityUsingShippingProfileID);
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
				case "UpsProfile":
					SetupSyncUpsProfile(relatedEntity);
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
				case "UpsProfile":
					DesetupSyncUpsProfile(false, true);
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
			if(_upsProfile!=null)
			{
				toReturn.Add(_upsProfile);
			}
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
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
				info.AddValue("_upsProfile", (!this.MarkedForDeletion?_upsProfile:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new UpsProfilePackageRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'UpsProfile' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUpsProfile()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UpsProfileFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(UpsProfilePackageEntityFactory));
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
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("UpsProfile", _upsProfile);
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
			_fieldsCustomProperties.Add("UpsProfilePackageID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShippingProfileID", fieldHashtable);
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
			_fieldsCustomProperties.Add("AdditionalHandlingEnabled", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("VerbalConfirmationEnabled", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("VerbalConfirmationName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("VerbalConfirmationPhone", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("VerbalConfirmationPhoneExtension", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DryIceEnabled", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DryIceRegulationSet", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DryIceWeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DryIceIsForMedicalUse", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _upsProfile</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncUpsProfile(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _upsProfile, new PropertyChangedEventHandler( OnUpsProfilePropertyChanged ), "UpsProfile", ShipWorks.Data.Model.RelationClasses.StaticUpsProfilePackageRelations.UpsProfileEntityUsingShippingProfileIDStatic, true, signalRelatedEntity, "Packages", resetFKFields, new int[] { (int)UpsProfilePackageFieldIndex.ShippingProfileID } );
			_upsProfile = null;
		}

		/// <summary> setups the sync logic for member _upsProfile</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncUpsProfile(IEntityCore relatedEntity)
		{
			if(_upsProfile!=relatedEntity)
			{
				DesetupSyncUpsProfile(true, true);
				_upsProfile = (UpsProfileEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _upsProfile, new PropertyChangedEventHandler( OnUpsProfilePropertyChanged ), "UpsProfile", ShipWorks.Data.Model.RelationClasses.StaticUpsProfilePackageRelations.UpsProfileEntityUsingShippingProfileIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnUpsProfilePropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this UpsProfilePackageEntity</param>
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
		public  static UpsProfilePackageRelations Relations
		{
			get	{ return new UpsProfilePackageRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UpsProfile' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUpsProfile
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(UpsProfileEntityFactory))),	(IEntityRelation)GetRelationsForField("UpsProfile")[0], (int)ShipWorks.Data.Model.EntityType.UpsProfilePackageEntity, (int)ShipWorks.Data.Model.EntityType.UpsProfileEntity, 0, null, null, null, null, "UpsProfile", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The UpsProfilePackageID property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."UpsProfilePackageID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 UpsProfilePackageID
		{
			get { return (System.Int64)GetValue((int)UpsProfilePackageFieldIndex.UpsProfilePackageID, true); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.UpsProfilePackageID, value); }
		}

		/// <summary> The ShippingProfileID property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."ShippingProfileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 ShippingProfileID
		{
			get { return (System.Int64)GetValue((int)UpsProfilePackageFieldIndex.ShippingProfileID, true); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.ShippingProfileID, value); }
		}

		/// <summary> The PackagingType property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."PackagingType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> PackagingType
		{
			get { return (Nullable<System.Int32>)GetValue((int)UpsProfilePackageFieldIndex.PackagingType, false); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.PackagingType, value); }
		}

		/// <summary> The Weight property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."Weight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> Weight
		{
			get { return (Nullable<System.Double>)GetValue((int)UpsProfilePackageFieldIndex.Weight, false); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.Weight, value); }
		}

		/// <summary> The DimsProfileID property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."DimsProfileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> DimsProfileID
		{
			get { return (Nullable<System.Int64>)GetValue((int)UpsProfilePackageFieldIndex.DimsProfileID, false); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.DimsProfileID, value); }
		}

		/// <summary> The DimsLength property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."DimsLength"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DimsLength
		{
			get { return (Nullable<System.Double>)GetValue((int)UpsProfilePackageFieldIndex.DimsLength, false); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.DimsLength, value); }
		}

		/// <summary> The DimsWidth property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."DimsWidth"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DimsWidth
		{
			get { return (Nullable<System.Double>)GetValue((int)UpsProfilePackageFieldIndex.DimsWidth, false); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.DimsWidth, value); }
		}

		/// <summary> The DimsHeight property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."DimsHeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DimsHeight
		{
			get { return (Nullable<System.Double>)GetValue((int)UpsProfilePackageFieldIndex.DimsHeight, false); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.DimsHeight, value); }
		}

		/// <summary> The DimsWeight property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."DimsWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DimsWeight
		{
			get { return (Nullable<System.Double>)GetValue((int)UpsProfilePackageFieldIndex.DimsWeight, false); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.DimsWeight, value); }
		}

		/// <summary> The DimsAddWeight property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."DimsAddWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> DimsAddWeight
		{
			get { return (Nullable<System.Boolean>)GetValue((int)UpsProfilePackageFieldIndex.DimsAddWeight, false); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.DimsAddWeight, value); }
		}

		/// <summary> The AdditionalHandlingEnabled property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."AdditionalHandlingEnabled"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> AdditionalHandlingEnabled
		{
			get { return (Nullable<System.Boolean>)GetValue((int)UpsProfilePackageFieldIndex.AdditionalHandlingEnabled, false); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.AdditionalHandlingEnabled, value); }
		}

		/// <summary> The VerbalConfirmationEnabled property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."VerbalConfirmationEnabled"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> VerbalConfirmationEnabled
		{
			get { return (Nullable<System.Boolean>)GetValue((int)UpsProfilePackageFieldIndex.VerbalConfirmationEnabled, false); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.VerbalConfirmationEnabled, value); }
		}

		/// <summary> The VerbalConfirmationName property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."VerbalConfirmationName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 35<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String VerbalConfirmationName
		{
			get { return (System.String)GetValue((int)UpsProfilePackageFieldIndex.VerbalConfirmationName, true); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.VerbalConfirmationName, value); }
		}

		/// <summary> The VerbalConfirmationPhone property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."VerbalConfirmationPhone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 15<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String VerbalConfirmationPhone
		{
			get { return (System.String)GetValue((int)UpsProfilePackageFieldIndex.VerbalConfirmationPhone, true); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.VerbalConfirmationPhone, value); }
		}

		/// <summary> The VerbalConfirmationPhoneExtension property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."VerbalConfirmationPhoneExtension"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 4<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String VerbalConfirmationPhoneExtension
		{
			get { return (System.String)GetValue((int)UpsProfilePackageFieldIndex.VerbalConfirmationPhoneExtension, true); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.VerbalConfirmationPhoneExtension, value); }
		}

		/// <summary> The DryIceEnabled property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."DryIceEnabled"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> DryIceEnabled
		{
			get { return (Nullable<System.Boolean>)GetValue((int)UpsProfilePackageFieldIndex.DryIceEnabled, false); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.DryIceEnabled, value); }
		}

		/// <summary> The DryIceRegulationSet property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."DryIceRegulationSet"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> DryIceRegulationSet
		{
			get { return (Nullable<System.Int32>)GetValue((int)UpsProfilePackageFieldIndex.DryIceRegulationSet, false); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.DryIceRegulationSet, value); }
		}

		/// <summary> The DryIceWeight property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."DryIceWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DryIceWeight
		{
			get { return (Nullable<System.Double>)GetValue((int)UpsProfilePackageFieldIndex.DryIceWeight, false); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.DryIceWeight, value); }
		}

		/// <summary> The DryIceIsForMedicalUse property of the Entity UpsProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UpsProfilePackage"."DryIceIsForMedicalUse"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> DryIceIsForMedicalUse
		{
			get { return (Nullable<System.Boolean>)GetValue((int)UpsProfilePackageFieldIndex.DryIceIsForMedicalUse, false); }
			set	{ SetValue((int)UpsProfilePackageFieldIndex.DryIceIsForMedicalUse, value); }
		}

		/// <summary> Gets / sets related entity of type 'UpsProfileEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual UpsProfileEntity UpsProfile
		{
			get	{ return _upsProfile; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncUpsProfile(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "Packages", "UpsProfile", _upsProfile, true); 
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
			get { return (int)ShipWorks.Data.Model.EntityType.UpsProfilePackageEntity; }
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
