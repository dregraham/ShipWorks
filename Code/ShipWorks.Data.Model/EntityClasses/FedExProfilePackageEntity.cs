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
	/// <summary>Entity class which represents the entity 'FedExProfilePackage'.<br/><br/></summary>
	[Serializable]
	public partial class FedExProfilePackageEntity : PackageProfileEntity
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static new partial class MemberNames
		{
			/// <summary>Member name ShippingProfile</summary>
			public static readonly string ShippingProfile = "ShippingProfile";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static FedExProfilePackageEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public FedExProfilePackageEntity()
		{
			InitClassEmpty();
			SetName("FedExProfilePackageEntity");
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public FedExProfilePackageEntity(IEntityFields2 fields):base(fields)
		{
			InitClassEmpty();
			SetName("FedExProfilePackageEntity");
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this FedExProfilePackageEntity</param>
		public FedExProfilePackageEntity(IValidator validator):base(validator)
		{
			InitClassEmpty();
			SetName("FedExProfilePackageEntity");
		}
				
		/// <summary> CTor</summary>
		/// <param name="packageProfileID">PK value for FedExProfilePackage which data should be fetched into this FedExProfilePackage object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public FedExProfilePackageEntity(System.Int64 packageProfileID):base(packageProfileID)
		{
			InitClassEmpty();

			SetName("FedExProfilePackageEntity");
		}

		/// <summary> CTor</summary>
		/// <param name="packageProfileID">PK value for FedExProfilePackage which data should be fetched into this FedExProfilePackage object</param>
		/// <param name="validator">The custom validator object for this FedExProfilePackageEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public FedExProfilePackageEntity(System.Int64 packageProfileID, IValidator validator):base(packageProfileID, validator)
		{
			InitClassEmpty();

			SetName("FedExProfilePackageEntity");
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected FedExProfilePackageEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
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
				default:
					toReturn = PackageProfileEntity.GetRelationsForField(fieldName);
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
			toReturn.AddRange(base.GetMemberEntityCollections());
			return toReturn;
		}

		/// <summary>Gets a predicateexpression which filters on this entity</summary>
		/// <returns>ready to use predicateexpression</returns>
		/// <remarks>Only useful in entity fetches.</remarks>
		public new static IPredicateExpression GetEntityTypeFilter()
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetEntityTypeFilter("FedExProfilePackageEntity", false);
		}
		
		/// <summary>Gets a predicateexpression which filters on this entity</summary>
		/// <param name="negate">Flag to produce a NOT filter, (true), or a normal filter (false). </param>
		/// <returns>ready to use predicateexpression</returns>
		/// <remarks>Only useful in entity fetches.</remarks>
		public new static IPredicateExpression GetEntityTypeFilter(bool negate)
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetEntityTypeFilter("FedExProfilePackageEntity", negate);
		}

		/// <summary>ISerializable member. Does custom serialization so event handlers do not get serialized. Serializes members of this entity class and uses the base class' implementation to serialize the rest.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
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
			return InheritanceInfoProviderSingleton.GetInstance().CheckIfIsSubTypeOf("FedExProfilePackageEntity", ((ShipWorks.Data.Model.EntityType)typeOfEntity).ToString());
		}
				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new FedExProfilePackageRelations().GetAllRelations();
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
			Dictionary<string, object> toReturn = base.GetRelatedData();
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
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ContainerType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("NumberOfContainers", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PackingDetailsCargoAircraftOnly", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PackingDetailsPackingInstructions", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BatteryMaterial", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BatteryPacking", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BatteryRegulatorySubtype", fieldHashtable);
		}
		#endregion

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this FedExProfilePackageEntity</param>
		private void InitClassEmpty()
		{
			InitClassMembers();

			// __LLBLGENPRO_USER_CODE_REGION_START InitClassEmpty
			// __LLBLGENPRO_USER_CODE_REGION_END


		}

		#region Class Property Declarations
		/// <summary> The relations object holding all relations of this entity with other entity classes.</summary>
		public new static FedExProfilePackageRelations Relations
		{
			get	{ return new FedExProfilePackageRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public new static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
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

		/// <summary> The ContainerType property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."ContainerType"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String ContainerType
		{
			get { return (System.String)GetValue((int)FedExProfilePackageFieldIndex.ContainerType, true); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.ContainerType, value); }
		}

		/// <summary> The NumberOfContainers property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."NumberOfContainers"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> NumberOfContainers
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfilePackageFieldIndex.NumberOfContainers, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.NumberOfContainers, value); }
		}

		/// <summary> The PackingDetailsCargoAircraftOnly property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."PackingDetailsCargoAircraftOnly"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> PackingDetailsCargoAircraftOnly
		{
			get { return (Nullable<System.Boolean>)GetValue((int)FedExProfilePackageFieldIndex.PackingDetailsCargoAircraftOnly, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.PackingDetailsCargoAircraftOnly, value); }
		}

		/// <summary> The PackingDetailsPackingInstructions property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."PackingDetailsPackingInstructions"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String PackingDetailsPackingInstructions
		{
			get { return (System.String)GetValue((int)FedExProfilePackageFieldIndex.PackingDetailsPackingInstructions, true); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.PackingDetailsPackingInstructions, value); }
		}

		/// <summary> The BatteryMaterial property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."BatteryMaterial"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<ShipWorks.Shipping.FedEx.FedExBatteryMaterialType> BatteryMaterial
		{
			get { return (Nullable<ShipWorks.Shipping.FedEx.FedExBatteryMaterialType>)GetValue((int)FedExProfilePackageFieldIndex.BatteryMaterial, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.BatteryMaterial, value); }
		}

		/// <summary> The BatteryPacking property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."BatteryPacking"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<ShipWorks.Shipping.FedEx.FedExBatteryPackingType> BatteryPacking
		{
			get { return (Nullable<ShipWorks.Shipping.FedEx.FedExBatteryPackingType>)GetValue((int)FedExProfilePackageFieldIndex.BatteryPacking, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.BatteryPacking, value); }
		}

		/// <summary> The BatteryRegulatorySubtype property of the Entity FedExProfilePackage<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfilePackage"."BatteryRegulatorySubtype"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<ShipWorks.Shipping.FedEx.FedExBatteryRegulatorySubType> BatteryRegulatorySubtype
		{
			get { return (Nullable<ShipWorks.Shipping.FedEx.FedExBatteryRegulatorySubType>)GetValue((int)FedExProfilePackageFieldIndex.BatteryRegulatorySubtype, false); }
			set	{ SetValue((int)FedExProfilePackageFieldIndex.BatteryRegulatorySubtype, value); }
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
