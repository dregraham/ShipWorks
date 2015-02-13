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
	/// Entity class which represents the entity 'ShippingSettings'.<br/><br/>
	/// 
	/// </summary>
	[Serializable]
	public partial class ShippingSettingsEntity : CommonEntityBase, ISerializable
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
		public static partial class MemberNames
		{




		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static ShippingSettingsEntity()
		{
			SetupCustomPropertyHashtables();
		}

		/// <summary> CTor</summary>
		public ShippingSettingsEntity():base("ShippingSettingsEntity")
		{
			InitClassEmpty(null, CreateFields());
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public ShippingSettingsEntity(IEntityFields2 fields):base("ShippingSettingsEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this ShippingSettingsEntity</param>
		public ShippingSettingsEntity(IValidator validator):base("ShippingSettingsEntity")
		{
			InitClassEmpty(validator, CreateFields());
		}
				

		/// <summary> CTor</summary>
		/// <param name="shippingSettingsID">PK value for ShippingSettings which data should be fetched into this ShippingSettings object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ShippingSettingsEntity(System.Boolean shippingSettingsID):base("ShippingSettingsEntity")
		{
			InitClassEmpty(null, CreateFields());
			this.ShippingSettingsID = shippingSettingsID;
		}

		/// <summary> CTor</summary>
		/// <param name="shippingSettingsID">PK value for ShippingSettings which data should be fetched into this ShippingSettings object</param>
		/// <param name="validator">The custom validator object for this ShippingSettingsEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ShippingSettingsEntity(System.Boolean shippingSettingsID, IValidator validator):base("ShippingSettingsEntity")
		{
			InitClassEmpty(validator, CreateFields());
			this.ShippingSettingsID = shippingSettingsID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected ShippingSettingsEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{




				base.FixupDeserialization(FieldInfoProviderSingleton.GetInstance());
			}
			
			// __LLBLGENPRO_USER_CODE_REGION_START DeserializationConstructor
			// __LLBLGENPRO_USER_CODE_REGION_END
		}

		
		/// <summary>Performs the desync setup when an FK field has been changed. The entity referenced based on the FK field will be dereferenced and sync info will be removed.</summary>
		/// <param name="fieldIndex">The fieldindex.</param>
		protected override void PerformDesyncSetupFKFieldChange(int fieldIndex)
		{
			switch((ShippingSettingsFieldIndex)fieldIndex)
			{
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




				default:
					break;
			}
		}
		
		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		public override RelationCollection GetRelationsForFieldOfType(string fieldName)
		{
			return ShippingSettingsEntity.GetRelationsForField(fieldName);
		}

		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		public static RelationCollection GetRelationsForField(string fieldName)
		{
			RelationCollection toReturn = new RelationCollection();
			switch(fieldName)
			{




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




			}
			
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}

		/// <summary>Returns true if the original value for the field with the fieldIndex passed in, read from the persistent storage was NULL, false otherwise.
		/// Should not be used for testing if the current value is NULL, use <see cref="TestCurrentFieldValueForNull"/> for that.</summary>
		/// <param name="fieldIndex">Index of the field to test if that field was NULL in the persistent storage</param>
		/// <returns>true if the field with the passed in index was NULL in the persistent storage, false otherwise</returns>
		public bool TestOriginalFieldValueForNull(ShippingSettingsFieldIndex fieldIndex)
		{
			return base.Fields[(int)fieldIndex].IsNull;
		}
		
		/// <summary>Returns true if the current value for the field with the fieldIndex passed in represents null/not defined, false otherwise.
		/// Should not be used for testing if the original value (read from the db) is NULL</summary>
		/// <param name="fieldIndex">Index of the field to test if its currentvalue is null/undefined</param>
		/// <returns>true if the field's value isn't defined yet, false otherwise</returns>
		public bool TestCurrentFieldValueForNull(ShippingSettingsFieldIndex fieldIndex)
		{
			return base.CheckIfCurrentFieldValueIsNull((int)fieldIndex);
		}

				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			return new ShippingSettingsRelations().GetAllRelations();
		}
		




	
		
		/// <summary>Creates entity fields object for this entity. Used in constructor to setup this entity in a polymorphic scenario.</summary>
		protected virtual IEntityFields2 CreateFields()
		{
			return EntityFieldsFactory.CreateEntityFieldsObject(ShipWorks.Data.Model.EntityType.ShippingSettingsEntity);
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
			return EntityFactoryCache2.GetEntityFactory(typeof(ShippingSettingsEntityFactory));
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




			return toReturn;
		}
		
		/// <summary> Adds the internals to the active context. </summary>
		protected override void AddInternalsToContext()
		{




		}

		/// <summary> Initializes the class members</summary>
		protected virtual void InitClassMembers()
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

			Dictionary<string, string> fieldHashtable = null;
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ShippingSettingsID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("InternalActivated", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("InternalConfigured", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("InternalExcluded", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("DefaultType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BlankPhoneOption", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("BlankPhoneNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("InsurancePolicy", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("InsuranceLastAgreed", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("FedExUsername", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("FedExPassword", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("FedExMaskAccount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("FedExThermalDocTab", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("FedExThermalDocTabType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("FedExInsuranceProvider", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("FedExInsurancePennyOne", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("UpsAccessKey", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("UpsInsuranceProvider", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("UpsInsurancePennyOne", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EndiciaCustomsCertify", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EndiciaCustomsSigner", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EndiciaThermalDocTab", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EndiciaThermalDocTabType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EndiciaAutomaticExpress1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EndiciaAutomaticExpress1Account", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EndiciaInsuranceProvider", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EndiciaUspsAutomaticExpedited", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EndiciaUspsAutomaticExpeditedAccount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("WorldShipLaunch", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("UspsAutomaticExpress1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("UspsAutomaticExpress1Account", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("StampsUspsAutomaticExpedited", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("StampsUspsAutomaticExpeditedAccount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Express1EndiciaCustomsCertify", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Express1EndiciaCustomsSigner", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Express1EndiciaThermalDocTab", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Express1EndiciaThermalDocTabType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Express1EndiciaSingleSource", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("OnTracInsuranceProvider", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("OnTracInsurancePennyOne", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("IParcelInsuranceProvider", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("IParcelInsurancePennyOne", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Express1UspsSingleSource", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("UpsMailInnovationsEnabled", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("WorldShipMailInnovationsEnabled", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("InternalBestRateExcludedShipmentTypes", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ShipSenseEnabled", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ShipSenseUniquenessXml", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ShipSenseProcessedShipmentID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ShipSenseEndShipmentID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("AutoCreateShipments", fieldHashtable);
		}
		#endregion



		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this ShippingSettingsEntity</param>
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
		public  static ShippingSettingsRelations Relations
		{
			get	{ return new ShippingSettingsRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}





		/// <summary> The custom properties for the type of this entity instance.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		public override Dictionary<string, string> CustomPropertiesOfType
		{
			get { return ShippingSettingsEntity.CustomProperties;}
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
			get { return ShippingSettingsEntity.FieldsCustomProperties;}
		}

		/// <summary> The ShippingSettingsID property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."ShippingSettingsID"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
		public virtual System.Boolean ShippingSettingsID
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.ShippingSettingsID, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.ShippingSettingsID, value); }
		}

		/// <summary> The InternalActivated property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."Activated"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 45<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String InternalActivated
		{
			get { return (System.String)GetValue((int)ShippingSettingsFieldIndex.InternalActivated, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.InternalActivated, value); }
		}

		/// <summary> The InternalConfigured property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."Configured"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 45<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String InternalConfigured
		{
			get { return (System.String)GetValue((int)ShippingSettingsFieldIndex.InternalConfigured, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.InternalConfigured, value); }
		}

		/// <summary> The InternalExcluded property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."Excluded"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 45<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String InternalExcluded
		{
			get { return (System.String)GetValue((int)ShippingSettingsFieldIndex.InternalExcluded, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.InternalExcluded, value); }
		}

		/// <summary> The DefaultType property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."DefaultType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 DefaultType
		{
			get { return (System.Int32)GetValue((int)ShippingSettingsFieldIndex.DefaultType, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.DefaultType, value); }
		}

		/// <summary> The BlankPhoneOption property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."BlankPhoneOption"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 BlankPhoneOption
		{
			get { return (System.Int32)GetValue((int)ShippingSettingsFieldIndex.BlankPhoneOption, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.BlankPhoneOption, value); }
		}

		/// <summary> The BlankPhoneNumber property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."BlankPhoneNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 16<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String BlankPhoneNumber
		{
			get { return (System.String)GetValue((int)ShippingSettingsFieldIndex.BlankPhoneNumber, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.BlankPhoneNumber, value); }
		}

		/// <summary> The InsurancePolicy property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."InsurancePolicy"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 40<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String InsurancePolicy
		{
			get { return (System.String)GetValue((int)ShippingSettingsFieldIndex.InsurancePolicy, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.InsurancePolicy, value); }
		}

		/// <summary> The InsuranceLastAgreed property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."InsuranceLastAgreed"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.DateTime> InsuranceLastAgreed
		{
			get { return (Nullable<System.DateTime>)GetValue((int)ShippingSettingsFieldIndex.InsuranceLastAgreed, false); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.InsuranceLastAgreed, value); }
		}

		/// <summary> The FedExUsername property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."FedExUsername"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String FedExUsername
		{
			get { return (System.String)GetValue((int)ShippingSettingsFieldIndex.FedExUsername, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.FedExUsername, value); }
		}

		/// <summary> The FedExPassword property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."FedExPassword"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String FedExPassword
		{
			get { return (System.String)GetValue((int)ShippingSettingsFieldIndex.FedExPassword, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.FedExPassword, value); }
		}

		/// <summary> The FedExMaskAccount property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."FedExMaskAccount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean FedExMaskAccount
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.FedExMaskAccount, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.FedExMaskAccount, value); }
		}

		/// <summary> The FedExThermalDocTab property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."FedExThermalDocTab"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean FedExThermalDocTab
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.FedExThermalDocTab, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.FedExThermalDocTab, value); }
		}

		/// <summary> The FedExThermalDocTabType property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."FedExThermalDocTabType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 FedExThermalDocTabType
		{
			get { return (System.Int32)GetValue((int)ShippingSettingsFieldIndex.FedExThermalDocTabType, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.FedExThermalDocTabType, value); }
		}

		/// <summary> The FedExInsuranceProvider property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."FedExInsuranceProvider"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 FedExInsuranceProvider
		{
			get { return (System.Int32)GetValue((int)ShippingSettingsFieldIndex.FedExInsuranceProvider, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.FedExInsuranceProvider, value); }
		}

		/// <summary> The FedExInsurancePennyOne property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."FedExInsurancePennyOne"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean FedExInsurancePennyOne
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.FedExInsurancePennyOne, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.FedExInsurancePennyOne, value); }
		}

		/// <summary> The UpsAccessKey property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."UpsAccessKey"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String UpsAccessKey
		{
			get { return (System.String)GetValue((int)ShippingSettingsFieldIndex.UpsAccessKey, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.UpsAccessKey, value); }
		}

		/// <summary> The UpsInsuranceProvider property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."UpsInsuranceProvider"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 UpsInsuranceProvider
		{
			get { return (System.Int32)GetValue((int)ShippingSettingsFieldIndex.UpsInsuranceProvider, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.UpsInsuranceProvider, value); }
		}

		/// <summary> The UpsInsurancePennyOne property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."UpsInsurancePennyOne"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean UpsInsurancePennyOne
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.UpsInsurancePennyOne, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.UpsInsurancePennyOne, value); }
		}

		/// <summary> The EndiciaCustomsCertify property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."EndiciaCustomsCertify"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean EndiciaCustomsCertify
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.EndiciaCustomsCertify, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.EndiciaCustomsCertify, value); }
		}

		/// <summary> The EndiciaCustomsSigner property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."EndiciaCustomsSigner"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String EndiciaCustomsSigner
		{
			get { return (System.String)GetValue((int)ShippingSettingsFieldIndex.EndiciaCustomsSigner, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.EndiciaCustomsSigner, value); }
		}

		/// <summary> The EndiciaThermalDocTab property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."EndiciaThermalDocTab"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean EndiciaThermalDocTab
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.EndiciaThermalDocTab, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.EndiciaThermalDocTab, value); }
		}

		/// <summary> The EndiciaThermalDocTabType property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."EndiciaThermalDocTabType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 EndiciaThermalDocTabType
		{
			get { return (System.Int32)GetValue((int)ShippingSettingsFieldIndex.EndiciaThermalDocTabType, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.EndiciaThermalDocTabType, value); }
		}

		/// <summary> The EndiciaAutomaticExpress1 property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."EndiciaAutomaticExpress1"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean EndiciaAutomaticExpress1
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.EndiciaAutomaticExpress1, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.EndiciaAutomaticExpress1, value); }
		}

		/// <summary> The EndiciaAutomaticExpress1Account property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."EndiciaAutomaticExpress1Account"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 EndiciaAutomaticExpress1Account
		{
			get { return (System.Int64)GetValue((int)ShippingSettingsFieldIndex.EndiciaAutomaticExpress1Account, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.EndiciaAutomaticExpress1Account, value); }
		}

		/// <summary> The EndiciaInsuranceProvider property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."EndiciaInsuranceProvider"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 EndiciaInsuranceProvider
		{
			get { return (System.Int32)GetValue((int)ShippingSettingsFieldIndex.EndiciaInsuranceProvider, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.EndiciaInsuranceProvider, value); }
		}

		/// <summary> The EndiciaUspsAutomaticExpedited property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."EndiciaUspsAutomaticExpedited"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean EndiciaUspsAutomaticExpedited
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.EndiciaUspsAutomaticExpedited, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.EndiciaUspsAutomaticExpedited, value); }
		}

		/// <summary> The EndiciaUspsAutomaticExpeditedAccount property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."EndiciaUspsAutomaticExpeditedAccount"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 EndiciaUspsAutomaticExpeditedAccount
		{
			get { return (System.Int64)GetValue((int)ShippingSettingsFieldIndex.EndiciaUspsAutomaticExpeditedAccount, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.EndiciaUspsAutomaticExpeditedAccount, value); }
		}

		/// <summary> The WorldShipLaunch property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."WorldShipLaunch"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean WorldShipLaunch
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.WorldShipLaunch, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.WorldShipLaunch, value); }
		}

		/// <summary> The UspsAutomaticExpress1 property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."UspsAutomaticExpress1"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean UspsAutomaticExpress1
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.UspsAutomaticExpress1, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.UspsAutomaticExpress1, value); }
		}

		/// <summary> The UspsAutomaticExpress1Account property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."UspsAutomaticExpress1Account"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 UspsAutomaticExpress1Account
		{
			get { return (System.Int64)GetValue((int)ShippingSettingsFieldIndex.UspsAutomaticExpress1Account, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.UspsAutomaticExpress1Account, value); }
		}

		/// <summary> The StampsUspsAutomaticExpedited property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."StampsUspsAutomaticExpedited"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean StampsUspsAutomaticExpedited
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.StampsUspsAutomaticExpedited, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.StampsUspsAutomaticExpedited, value); }
		}

		/// <summary> The StampsUspsAutomaticExpeditedAccount property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."StampsUspsAutomaticExpeditedAccount"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 StampsUspsAutomaticExpeditedAccount
		{
			get { return (System.Int64)GetValue((int)ShippingSettingsFieldIndex.StampsUspsAutomaticExpeditedAccount, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.StampsUspsAutomaticExpeditedAccount, value); }
		}

		/// <summary> The Express1EndiciaCustomsCertify property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."Express1EndiciaCustomsCertify"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Express1EndiciaCustomsCertify
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.Express1EndiciaCustomsCertify, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.Express1EndiciaCustomsCertify, value); }
		}

		/// <summary> The Express1EndiciaCustomsSigner property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."Express1EndiciaCustomsSigner"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Express1EndiciaCustomsSigner
		{
			get { return (System.String)GetValue((int)ShippingSettingsFieldIndex.Express1EndiciaCustomsSigner, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.Express1EndiciaCustomsSigner, value); }
		}

		/// <summary> The Express1EndiciaThermalDocTab property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."Express1EndiciaThermalDocTab"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Express1EndiciaThermalDocTab
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.Express1EndiciaThermalDocTab, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.Express1EndiciaThermalDocTab, value); }
		}

		/// <summary> The Express1EndiciaThermalDocTabType property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."Express1EndiciaThermalDocTabType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 Express1EndiciaThermalDocTabType
		{
			get { return (System.Int32)GetValue((int)ShippingSettingsFieldIndex.Express1EndiciaThermalDocTabType, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.Express1EndiciaThermalDocTabType, value); }
		}

		/// <summary> The Express1EndiciaSingleSource property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."Express1EndiciaSingleSource"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Express1EndiciaSingleSource
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.Express1EndiciaSingleSource, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.Express1EndiciaSingleSource, value); }
		}

		/// <summary> The OnTracInsuranceProvider property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."OnTracInsuranceProvider"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 OnTracInsuranceProvider
		{
			get { return (System.Int32)GetValue((int)ShippingSettingsFieldIndex.OnTracInsuranceProvider, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.OnTracInsuranceProvider, value); }
		}

		/// <summary> The OnTracInsurancePennyOne property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."OnTracInsurancePennyOne"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean OnTracInsurancePennyOne
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.OnTracInsurancePennyOne, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.OnTracInsurancePennyOne, value); }
		}

		/// <summary> The IParcelInsuranceProvider property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."iParcelInsuranceProvider"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 IParcelInsuranceProvider
		{
			get { return (System.Int32)GetValue((int)ShippingSettingsFieldIndex.IParcelInsuranceProvider, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.IParcelInsuranceProvider, value); }
		}

		/// <summary> The IParcelInsurancePennyOne property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."iParcelInsurancePennyOne"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean IParcelInsurancePennyOne
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.IParcelInsurancePennyOne, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.IParcelInsurancePennyOne, value); }
		}

		/// <summary> The Express1UspsSingleSource property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."Express1UspsSingleSource"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Express1UspsSingleSource
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.Express1UspsSingleSource, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.Express1UspsSingleSource, value); }
		}

		/// <summary> The UpsMailInnovationsEnabled property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."UpsMailInnovationsEnabled"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean UpsMailInnovationsEnabled
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.UpsMailInnovationsEnabled, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.UpsMailInnovationsEnabled, value); }
		}

		/// <summary> The WorldShipMailInnovationsEnabled property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."WorldShipMailInnovationsEnabled"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean WorldShipMailInnovationsEnabled
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.WorldShipMailInnovationsEnabled, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.WorldShipMailInnovationsEnabled, value); }
		}

		/// <summary> The InternalBestRateExcludedShipmentTypes property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."BestRateExcludedShipmentTypes"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String InternalBestRateExcludedShipmentTypes
		{
			get { return (System.String)GetValue((int)ShippingSettingsFieldIndex.InternalBestRateExcludedShipmentTypes, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.InternalBestRateExcludedShipmentTypes, value); }
		}

		/// <summary> The ShipSenseEnabled property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."ShipSenseEnabled"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean ShipSenseEnabled
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.ShipSenseEnabled, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.ShipSenseEnabled, value); }
		}

		/// <summary> The ShipSenseUniquenessXml property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."ShipSenseUniquenessXml"<br/>
		/// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String ShipSenseUniquenessXml
		{
			get { return (System.String)GetValue((int)ShippingSettingsFieldIndex.ShipSenseUniquenessXml, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.ShipSenseUniquenessXml, value); }
		}

		/// <summary> The ShipSenseProcessedShipmentID property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."ShipSenseProcessedShipmentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 ShipSenseProcessedShipmentID
		{
			get { return (System.Int64)GetValue((int)ShippingSettingsFieldIndex.ShipSenseProcessedShipmentID, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.ShipSenseProcessedShipmentID, value); }
		}

		/// <summary> The ShipSenseEndShipmentID property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."ShipSenseEndShipmentID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 ShipSenseEndShipmentID
		{
			get { return (System.Int64)GetValue((int)ShippingSettingsFieldIndex.ShipSenseEndShipmentID, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.ShipSenseEndShipmentID, value); }
		}

		/// <summary> The AutoCreateShipments property of the Entity ShippingSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingSettings"."AutoCreateShipments"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean AutoCreateShipments
		{
			get { return (System.Boolean)GetValue((int)ShippingSettingsFieldIndex.AutoCreateShipments, true); }
			set	{ SetValue((int)ShippingSettingsFieldIndex.AutoCreateShipments, value); }
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
			get { return (int)ShipWorks.Data.Model.EntityType.ShippingSettingsEntity; }
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
