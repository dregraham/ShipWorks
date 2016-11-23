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
	/// <summary>Entity class which represents the entity 'FedExProfilePackage'.<br/><br/></summary>
	[Serializable]
	public partial class FedExProfilePackageEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private FedExProfileEntity _fedExProfile;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name FedExProfile</summary>
			public static readonly string FedExProfile = "FedExProfile";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static FedExProfilePackageEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public FedExProfilePackageEntity():base("FedExProfilePackageEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public FedExProfilePackageEntity(IEntityFields2 fields):base("FedExProfilePackageEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this FedExProfilePackageEntity</param>
		public FedExProfilePackageEntity(IValidator validator):base("FedExProfilePackageEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="fedExProfilePackageID">PK value for FedExProfilePackage which data should be fetched into this FedExProfilePackage object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public FedExProfilePackageEntity(System.Int64 fedExProfilePackageID):base("FedExProfilePackageEntity")
		{
			InitClassEmpty(null, null);
			this.FedExProfilePackageID = fedExProfilePackageID;
		}

		/// <summary> CTor</summary>
		/// <param name="fedExProfilePackageID">PK value for FedExProfilePackage which data should be fetched into this FedExProfilePackage object</param>
		/// <param name="validator">The custom validator object for this FedExProfilePackageEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public FedExProfilePackageEntity(System.Int64 fedExProfilePackageID, IValidator validator):base("FedExProfilePackageEntity")
		{
			InitClassEmpty(validator, null);
			this.FedExProfilePackageID = fedExProfilePackageID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected FedExProfilePackageEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_fedExProfile = (FedExProfileEntity)info.GetValue("_fedExProfile", typeof(FedExProfileEntity));
				if(_fedExProfile!=null)
				{
					_fedExProfile.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((FedExProfilePackageFieldIndex)fieldIndex)
			{
				case FedExProfilePackageFieldIndex.ShippingProfileID:
					DesetupSyncFedExProfile(true, false);
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
				case "FedExProfile":
					this.FedExProfile = (FedExProfileEntity)entity;
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
				case "FedExProfile":
					toReturn.Add(Relations.FedExProfileEntityUsingShippingProfileID);
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
				case "FedExProfile":
					SetupSyncFedExProfile(relatedEntity);
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
				case "FedExProfile":
					DesetupSyncFedExProfile(false, true);
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
			if(_fedExProfile!=null)
			{
				toReturn.Add(_fedExProfile);
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
				info.AddValue("_fedExProfile", (!this.MarkedForDeletion?_fedExProfile:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new FedExProfilePackageRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'FedExProfile' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoFedExProfile()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(FedExProfileFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(FedExProfilePackageEntityFactory));
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
			toReturn.Add("FedExProfile", _fedExProfile);
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
			_fieldsCustomProperties.Add("FedExProfilePackageID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShippingProfileID", fieldHashtable);
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
			_fieldsCustomProperties.Add("PriorityAlert", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PriorityAlertEnhancementType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PriorityAlertDetailContent", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DryIceWeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ContainsAlcohol", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DangerousGoodsEnabled", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DangerousGoodsType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DangerousGoodsAccessibilityType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DangerousGoodsCargoAircraftOnly", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DangerousGoodsEmergencyContactPhone", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DangerousGoodsOfferor", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DangerousGoodsPackagingCount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("HazardousMaterialNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("HazardousMaterialClass", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("HazardousMaterialProperName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("HazardousMaterialPackingGroup", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("HazardousMaterialQuantityValue", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("HazardousMaterialQuanityUnits", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SignatoryContactName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SignatoryTitle", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SignatoryPlace", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _fedExProfile</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncFedExProfile(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _fedExProfile, new PropertyChangedEventHandler( OnFedExProfilePropertyChanged ), "FedExProfile", ShipWorks.Data.Model.RelationClasses.StaticFedExProfilePackageRelations.FedExProfileEntityUsingShippingProfileIDStatic, true, signalRelatedEntity, "Packages", resetFKFields, new int[] { (int)FedExProfilePackageFieldIndex.ShippingProfileID } );
			_fedExProfile = null;
		}

		/// <summary> setups the sync logic for member _fedExProfile</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncFedExProfile(IEntityCore relatedEntity)
		{
			if(_fedExProfile!=relatedEntity)
			{
				DesetupSyncFedExProfile(true, true);
				_fedExProfile = (FedExProfileEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _fedExProfile, new PropertyChangedEventHandler( OnFedExProfilePropertyChanged ), "FedExProfile", ShipWorks.Data.Model.RelationClasses.StaticFedExProfilePackageRelations.FedExProfileEntityUsingShippingProfileIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnFedExProfilePropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this FedExProfilePackageEntity</param>
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
		public  static FedExProfilePackageRelations Relations
		{
			get	{ return new FedExProfilePackageRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'FedExProfile' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathFedExProfile
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(FedExProfileEntityFactory))),	(IEntityRelation)GetRelationsForField("FedExProfile")[0], (int)ShipWorks.Data.Model.EntityType.FedExProfilePackageEntity, (int)ShipWorks.Data.Model.EntityType.FedExProfileEntity, 0, null, null, null, null, "FedExProfile", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The FedExProfilePackageID property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."FedExProfilePackageID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 FedExProfilePackageID
		{
			get { return (System.Int64)GetValue((int)FedExProfilePackageFieldIndex.FedExProfilePackageID, true); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.FedExProfilePackageID, value); }
		}

		/// <summary> The ShippingProfileID property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."ShippingProfileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 ShippingProfileID
		{
			get { return (System.Int64)GetValue((int)FedExProfilePackageFieldIndex.ShippingProfileID, true); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.ShippingProfileID, value); }
		}

		/// <summary> The Weight property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."Weight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> Weight
		{
			get { return (Nullable<System.Double>)GetValue((int)FedExProfilePackageFieldIndex.Weight, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.Weight, value); }
		}

		/// <summary> The DimsProfileID property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."DimsProfileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> DimsProfileID
		{
			get { return (Nullable<System.Int64>)GetValue((int)FedExProfilePackageFieldIndex.DimsProfileID, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.DimsProfileID, value); }
		}

		/// <summary> The DimsLength property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."DimsLength"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DimsLength
		{
			get { return (Nullable<System.Double>)GetValue((int)FedExProfilePackageFieldIndex.DimsLength, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.DimsLength, value); }
		}

		/// <summary> The DimsWidth property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."DimsWidth"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DimsWidth
		{
			get { return (Nullable<System.Double>)GetValue((int)FedExProfilePackageFieldIndex.DimsWidth, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.DimsWidth, value); }
		}

		/// <summary> The DimsHeight property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."DimsHeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DimsHeight
		{
			get { return (Nullable<System.Double>)GetValue((int)FedExProfilePackageFieldIndex.DimsHeight, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.DimsHeight, value); }
		}

		/// <summary> The DimsWeight property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."DimsWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DimsWeight
		{
			get { return (Nullable<System.Double>)GetValue((int)FedExProfilePackageFieldIndex.DimsWeight, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.DimsWeight, value); }
		}

		/// <summary> The DimsAddWeight property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."DimsAddWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> DimsAddWeight
		{
			get { return (Nullable<System.Boolean>)GetValue((int)FedExProfilePackageFieldIndex.DimsAddWeight, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.DimsAddWeight, value); }
		}

		/// <summary> The PriorityAlert property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."PriorityAlert"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> PriorityAlert
		{
			get { return (Nullable<System.Boolean>)GetValue((int)FedExProfilePackageFieldIndex.PriorityAlert, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.PriorityAlert, value); }
		}

		/// <summary> The PriorityAlertEnhancementType property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."PriorityAlertEnhancementType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> PriorityAlertEnhancementType
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfilePackageFieldIndex.PriorityAlertEnhancementType, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.PriorityAlertEnhancementType, value); }
		}

		/// <summary> The PriorityAlertDetailContent property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."PriorityAlertDetailContent"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 1024<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String PriorityAlertDetailContent
		{
			get { return (System.String)GetValue((int)FedExProfilePackageFieldIndex.PriorityAlertDetailContent, true); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.PriorityAlertDetailContent, value); }
		}

		/// <summary> The DryIceWeight property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."DryIceWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DryIceWeight
		{
			get { return (Nullable<System.Double>)GetValue((int)FedExProfilePackageFieldIndex.DryIceWeight, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.DryIceWeight, value); }
		}

		/// <summary> The ContainsAlcohol property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."ContainsAlcohol"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> ContainsAlcohol
		{
			get { return (Nullable<System.Boolean>)GetValue((int)FedExProfilePackageFieldIndex.ContainsAlcohol, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.ContainsAlcohol, value); }
		}

		/// <summary> The DangerousGoodsEnabled property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."DangerousGoodsEnabled"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> DangerousGoodsEnabled
		{
			get { return (Nullable<System.Boolean>)GetValue((int)FedExProfilePackageFieldIndex.DangerousGoodsEnabled, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.DangerousGoodsEnabled, value); }
		}

		/// <summary> The DangerousGoodsType property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."DangerousGoodsType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> DangerousGoodsType
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfilePackageFieldIndex.DangerousGoodsType, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.DangerousGoodsType, value); }
		}

		/// <summary> The DangerousGoodsAccessibilityType property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."DangerousGoodsAccessibilityType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> DangerousGoodsAccessibilityType
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfilePackageFieldIndex.DangerousGoodsAccessibilityType, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.DangerousGoodsAccessibilityType, value); }
		}

		/// <summary> The DangerousGoodsCargoAircraftOnly property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."DangerousGoodsCargoAircraftOnly"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> DangerousGoodsCargoAircraftOnly
		{
			get { return (Nullable<System.Boolean>)GetValue((int)FedExProfilePackageFieldIndex.DangerousGoodsCargoAircraftOnly, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.DangerousGoodsCargoAircraftOnly, value); }
		}

		/// <summary> The DangerousGoodsEmergencyContactPhone property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."DangerousGoodsEmergencyContactPhone"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 16<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String DangerousGoodsEmergencyContactPhone
		{
			get { return (System.String)GetValue((int)FedExProfilePackageFieldIndex.DangerousGoodsEmergencyContactPhone, true); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.DangerousGoodsEmergencyContactPhone, value); }
		}

		/// <summary> The DangerousGoodsOfferor property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."DangerousGoodsOfferor"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 128<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String DangerousGoodsOfferor
		{
			get { return (System.String)GetValue((int)FedExProfilePackageFieldIndex.DangerousGoodsOfferor, true); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.DangerousGoodsOfferor, value); }
		}

		/// <summary> The DangerousGoodsPackagingCount property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."DangerousGoodsPackagingCount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> DangerousGoodsPackagingCount
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfilePackageFieldIndex.DangerousGoodsPackagingCount, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.DangerousGoodsPackagingCount, value); }
		}

		/// <summary> The HazardousMaterialNumber property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."HazardousMaterialNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 16<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HazardousMaterialNumber
		{
			get { return (System.String)GetValue((int)FedExProfilePackageFieldIndex.HazardousMaterialNumber, true); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.HazardousMaterialNumber, value); }
		}

		/// <summary> The HazardousMaterialClass property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."HazardousMaterialClass"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 8<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HazardousMaterialClass
		{
			get { return (System.String)GetValue((int)FedExProfilePackageFieldIndex.HazardousMaterialClass, true); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.HazardousMaterialClass, value); }
		}

		/// <summary> The HazardousMaterialProperName property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."HazardousMaterialProperName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HazardousMaterialProperName
		{
			get { return (System.String)GetValue((int)FedExProfilePackageFieldIndex.HazardousMaterialProperName, true); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.HazardousMaterialProperName, value); }
		}

		/// <summary> The HazardousMaterialPackingGroup property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."HazardousMaterialPackingGroup"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> HazardousMaterialPackingGroup
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfilePackageFieldIndex.HazardousMaterialPackingGroup, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.HazardousMaterialPackingGroup, value); }
		}

		/// <summary> The HazardousMaterialQuantityValue property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."HazardousMaterialQuantityValue"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> HazardousMaterialQuantityValue
		{
			get { return (Nullable<System.Double>)GetValue((int)FedExProfilePackageFieldIndex.HazardousMaterialQuantityValue, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.HazardousMaterialQuantityValue, value); }
		}

		/// <summary> The HazardousMaterialQuanityUnits property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."HazardousMaterialQuanityUnits"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> HazardousMaterialQuanityUnits
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfilePackageFieldIndex.HazardousMaterialQuanityUnits, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.HazardousMaterialQuanityUnits, value); }
		}

		/// <summary> The SignatoryContactName property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."SignatoryContactName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String SignatoryContactName
		{
			get { return (System.String)GetValue((int)FedExProfilePackageFieldIndex.SignatoryContactName, true); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.SignatoryContactName, value); }
		}

		/// <summary> The SignatoryTitle property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."SignatoryTitle"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String SignatoryTitle
		{
			get { return (System.String)GetValue((int)FedExProfilePackageFieldIndex.SignatoryTitle, true); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.SignatoryTitle, value); }
		}

		/// <summary> The SignatoryPlace property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."SignatoryPlace"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String SignatoryPlace
		{
			get { return (System.String)GetValue((int)FedExProfilePackageFieldIndex.SignatoryPlace, true); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.SignatoryPlace, value); }
		}

		/// <summary> Gets / sets related entity of type 'FedExProfileEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual FedExProfileEntity FedExProfile
		{
			get	{ return _fedExProfile; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncFedExProfile(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "Packages", "FedExProfile", _fedExProfile, true); 
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
			get { return (int)ShipWorks.Data.Model.EntityType.FedExProfilePackageEntity; }
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
