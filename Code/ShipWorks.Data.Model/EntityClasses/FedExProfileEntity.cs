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
	/// <summary>Entity class which represents the entity 'FedExProfile'.<br/><br/></summary>
	[Serializable]
	public partial class FedExProfileEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
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
			/// <summary>Member name ShippingProfile</summary>
			public static readonly string ShippingProfile = "ShippingProfile";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static FedExProfileEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public FedExProfileEntity():base("FedExProfileEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public FedExProfileEntity(IEntityFields2 fields):base("FedExProfileEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this FedExProfileEntity</param>
		public FedExProfileEntity(IValidator validator):base("FedExProfileEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="shippingProfileID">PK value for FedExProfile which data should be fetched into this FedExProfile object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public FedExProfileEntity(System.Int64 shippingProfileID):base("FedExProfileEntity")
		{
			InitClassEmpty(null, null);
			this.ShippingProfileID = shippingProfileID;
		}

		/// <summary> CTor</summary>
		/// <param name="shippingProfileID">PK value for FedExProfile which data should be fetched into this FedExProfile object</param>
		/// <param name="validator">The custom validator object for this FedExProfileEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public FedExProfileEntity(System.Int64 shippingProfileID, IValidator validator):base("FedExProfileEntity")
		{
			InitClassEmpty(validator, null);
			this.ShippingProfileID = shippingProfileID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected FedExProfileEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
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
			switch((FedExProfileFieldIndex)fieldIndex)
			{
				case FedExProfileFieldIndex.ShippingProfileID:
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
			return new FedExProfileRelations().GetAllRelations();
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
			return EntityFactoryCache2.GetEntityFactory(typeof(FedExProfileEntityFactory));
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
			_fieldsCustomProperties.Add("FedExAccountID", fieldHashtable);
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
			_fieldsCustomProperties.Add("PayorTransportAccount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PayorDutiesType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PayorDutiesAccount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SaturdayDelivery", fieldHashtable);
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
			_fieldsCustomProperties.Add("ResidentialDetermination", fieldHashtable);
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
			_fieldsCustomProperties.Add("EmailNotifyBroker", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DropoffType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OriginResidentialDetermination", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PayorTransportName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ReturnType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RmaNumber", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RmaReason", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ReturnSaturdayPickup", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ReturnsClearance", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ReferenceFIMS", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ThirdPartyConsignee", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CreateCommercialInvoice", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FileElectronically", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CustomsRecipientTIN", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CustomsRecipientTINType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PayorCountryCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PayorPostalCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DeliveredDutyPaid", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CustomsTinIssuingAuthority", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _shippingProfile</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncShippingProfile(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _shippingProfile, new PropertyChangedEventHandler( OnShippingProfilePropertyChanged ), "ShippingProfile", ShipWorks.Data.Model.RelationClasses.StaticFedExProfileRelations.ShippingProfileEntityUsingShippingProfileIDStatic, true, signalRelatedEntity, "FedEx", false, new int[] { (int)FedExProfileFieldIndex.ShippingProfileID } );
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
				this.PerformSetupSyncRelatedEntity( _shippingProfile, new PropertyChangedEventHandler( OnShippingProfilePropertyChanged ), "ShippingProfile", ShipWorks.Data.Model.RelationClasses.StaticFedExProfileRelations.ShippingProfileEntityUsingShippingProfileIDStatic, true, new string[] {  } );
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
		/// <param name="validator">The validator object for this FedExProfileEntity</param>
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
		public  static FedExProfileRelations Relations
		{
			get	{ return new FedExProfileRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ShippingProfile' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathShippingProfile
		{
			get { return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(ShippingProfileEntityFactory))), (IEntityRelation)GetRelationsForField("ShippingProfile")[0], (int)ShipWorks.Data.Model.EntityType.FedExProfileEntity, (int)ShipWorks.Data.Model.EntityType.ShippingProfileEntity, 0, null, null, null, null, "ShippingProfile", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);	}
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

		/// <summary> The ShippingProfileID property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."ShippingProfileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
		[DataMember]
		public virtual System.Int64 ShippingProfileID
		{
			get { return (System.Int64)GetValue((int)FedExProfileFieldIndex.ShippingProfileID, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.ShippingProfileID, value); }
		}

		/// <summary> The FedExAccountID property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."FedExAccountID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int64> FedExAccountID
		{
			get { return (Nullable<System.Int64>)GetValue((int)FedExProfileFieldIndex.FedExAccountID, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.FedExAccountID, value); }
		}

		/// <summary> The Service property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."Service"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> Service
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfileFieldIndex.Service, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.Service, value); }
		}

		/// <summary> The Signature property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."Signature"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> Signature
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfileFieldIndex.Signature, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.Signature, value); }
		}

		/// <summary> The PackagingType property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."PackagingType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> PackagingType
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfileFieldIndex.PackagingType, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.PackagingType, value); }
		}

		/// <summary> The NonStandardContainer property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."NonStandardContainer"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Boolean> NonStandardContainer
		{
			get { return (Nullable<System.Boolean>)GetValue((int)FedExProfileFieldIndex.NonStandardContainer, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.NonStandardContainer, value); }
		}

		/// <summary> The ReferenceCustomer property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."ReferenceCustomer"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String ReferenceCustomer
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.ReferenceCustomer, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.ReferenceCustomer, value); }
		}

		/// <summary> The ReferenceInvoice property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."ReferenceInvoice"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String ReferenceInvoice
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.ReferenceInvoice, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.ReferenceInvoice, value); }
		}

		/// <summary> The ReferencePO property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."ReferencePO"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String ReferencePO
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.ReferencePO, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.ReferencePO, value); }
		}

		/// <summary> The ReferenceShipmentIntegrity property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."ReferenceShipmentIntegrity"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String ReferenceShipmentIntegrity
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.ReferenceShipmentIntegrity, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.ReferenceShipmentIntegrity, value); }
		}

		/// <summary> The PayorTransportType property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."PayorTransportType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> PayorTransportType
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfileFieldIndex.PayorTransportType, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.PayorTransportType, value); }
		}

		/// <summary> The PayorTransportAccount property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."PayorTransportAccount"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 12<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String PayorTransportAccount
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.PayorTransportAccount, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.PayorTransportAccount, value); }
		}

		/// <summary> The PayorDutiesType property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."PayorDutiesType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> PayorDutiesType
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfileFieldIndex.PayorDutiesType, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.PayorDutiesType, value); }
		}

		/// <summary> The PayorDutiesAccount property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."PayorDutiesAccount"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 12<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String PayorDutiesAccount
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.PayorDutiesAccount, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.PayorDutiesAccount, value); }
		}

		/// <summary> The SaturdayDelivery property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."SaturdayDelivery"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Boolean> SaturdayDelivery
		{
			get { return (Nullable<System.Boolean>)GetValue((int)FedExProfileFieldIndex.SaturdayDelivery, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.SaturdayDelivery, value); }
		}

		/// <summary> The EmailNotifySender property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."EmailNotifySender"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> EmailNotifySender
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfileFieldIndex.EmailNotifySender, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.EmailNotifySender, value); }
		}

		/// <summary> The EmailNotifyRecipient property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."EmailNotifyRecipient"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> EmailNotifyRecipient
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfileFieldIndex.EmailNotifyRecipient, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.EmailNotifyRecipient, value); }
		}

		/// <summary> The EmailNotifyOther property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."EmailNotifyOther"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> EmailNotifyOther
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfileFieldIndex.EmailNotifyOther, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.EmailNotifyOther, value); }
		}

		/// <summary> The EmailNotifyOtherAddress property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."EmailNotifyOtherAddress"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String EmailNotifyOtherAddress
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.EmailNotifyOtherAddress, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.EmailNotifyOtherAddress, value); }
		}

		/// <summary> The EmailNotifyMessage property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."EmailNotifyMessage"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 120<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String EmailNotifyMessage
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.EmailNotifyMessage, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.EmailNotifyMessage, value); }
		}

		/// <summary> The ResidentialDetermination property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."ResidentialDetermination"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> ResidentialDetermination
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfileFieldIndex.ResidentialDetermination, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.ResidentialDetermination, value); }
		}

		/// <summary> The SmartPostIndicia property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."SmartPostIndicia"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> SmartPostIndicia
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfileFieldIndex.SmartPostIndicia, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.SmartPostIndicia, value); }
		}

		/// <summary> The SmartPostEndorsement property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."SmartPostEndorsement"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> SmartPostEndorsement
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfileFieldIndex.SmartPostEndorsement, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.SmartPostEndorsement, value); }
		}

		/// <summary> The SmartPostConfirmation property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."SmartPostConfirmation"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Boolean> SmartPostConfirmation
		{
			get { return (Nullable<System.Boolean>)GetValue((int)FedExProfileFieldIndex.SmartPostConfirmation, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.SmartPostConfirmation, value); }
		}

		/// <summary> The SmartPostCustomerManifest property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."SmartPostCustomerManifest"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String SmartPostCustomerManifest
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.SmartPostCustomerManifest, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.SmartPostCustomerManifest, value); }
		}

		/// <summary> The SmartPostHubID property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."SmartPostHubID"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 10<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String SmartPostHubID
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.SmartPostHubID, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.SmartPostHubID, value); }
		}

		/// <summary> The EmailNotifyBroker property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."EmailNotifyBroker"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> EmailNotifyBroker
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfileFieldIndex.EmailNotifyBroker, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.EmailNotifyBroker, value); }
		}

		/// <summary> The DropoffType property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."DropoffType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> DropoffType
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfileFieldIndex.DropoffType, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.DropoffType, value); }
		}

		/// <summary> The OriginResidentialDetermination property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."OriginResidentialDetermination"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> OriginResidentialDetermination
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfileFieldIndex.OriginResidentialDetermination, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.OriginResidentialDetermination, value); }
		}

		/// <summary> The PayorTransportName property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."PayorTransportName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String PayorTransportName
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.PayorTransportName, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.PayorTransportName, value); }
		}

		/// <summary> The ReturnType property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."ReturnType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> ReturnType
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfileFieldIndex.ReturnType, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.ReturnType, value); }
		}

		/// <summary> The RmaNumber property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."RmaNumber"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String RmaNumber
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.RmaNumber, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.RmaNumber, value); }
		}

		/// <summary> The RmaReason property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."RmaReason"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String RmaReason
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.RmaReason, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.RmaReason, value); }
		}

		/// <summary> The ReturnSaturdayPickup property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."ReturnSaturdayPickup"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Boolean> ReturnSaturdayPickup
		{
			get { return (Nullable<System.Boolean>)GetValue((int)FedExProfileFieldIndex.ReturnSaturdayPickup, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.ReturnSaturdayPickup, value); }
		}

		/// <summary> The ReturnsClearance property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."ReturnsClearance"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Boolean> ReturnsClearance
		{
			get { return (Nullable<System.Boolean>)GetValue((int)FedExProfileFieldIndex.ReturnsClearance, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.ReturnsClearance, value); }
		}

		/// <summary> The ReferenceFIMS property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."ReferenceFIMS"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String ReferenceFIMS
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.ReferenceFIMS, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.ReferenceFIMS, value); }
		}

		/// <summary> The ThirdPartyConsignee property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."ThirdPartyConsignee"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Boolean> ThirdPartyConsignee
		{
			get { return (Nullable<System.Boolean>)GetValue((int)FedExProfileFieldIndex.ThirdPartyConsignee, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.ThirdPartyConsignee, value); }
		}

		/// <summary> The CreateCommercialInvoice property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."CreateCommercialInvoice"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Boolean> CreateCommercialInvoice
		{
			get { return (Nullable<System.Boolean>)GetValue((int)FedExProfileFieldIndex.CreateCommercialInvoice, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.CreateCommercialInvoice, value); }
		}

		/// <summary> The FileElectronically property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."FileElectronically"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Boolean> FileElectronically
		{
			get { return (Nullable<System.Boolean>)GetValue((int)FedExProfileFieldIndex.FileElectronically, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.FileElectronically, value); }
		}

		/// <summary> The CustomsRecipientTIN property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."CustomsRecipientTIN"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 24<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String CustomsRecipientTIN
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.CustomsRecipientTIN, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.CustomsRecipientTIN, value); }
		}

		/// <summary> The CustomsRecipientTINType property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."CustomsRecipientTINType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Int32> CustomsRecipientTINType
		{
			get { return (Nullable<System.Int32>)GetValue((int)FedExProfileFieldIndex.CustomsRecipientTINType, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.CustomsRecipientTINType, value); }
		}

		/// <summary> The PayorCountryCode property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."PayorCountryCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String PayorCountryCode
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.PayorCountryCode, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.PayorCountryCode, value); }
		}

		/// <summary> The PayorPostalCode property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."PayorPostalCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String PayorPostalCode
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.PayorPostalCode, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.PayorPostalCode, value); }
		}

		/// <summary> The DeliveredDutyPaid property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."DeliveredDutyPaid"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual Nullable<System.Boolean> DeliveredDutyPaid
		{
			get { return (Nullable<System.Boolean>)GetValue((int)FedExProfileFieldIndex.DeliveredDutyPaid, false); }
			set	{ SetValue((int)FedExProfileFieldIndex.DeliveredDutyPaid, value); }
		}

		/// <summary> The CustomsTinIssuingAuthority property of the Entity FedExProfile<br/><br/></summary>
		/// <remarks>Mapped on  table field: "FedExProfile"."CustomsTinIssuingAuthority"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		[DataMember]
		public virtual System.String CustomsTinIssuingAuthority
		{
			get { return (System.String)GetValue((int)FedExProfileFieldIndex.CustomsTinIssuingAuthority, true); }
			set	{ SetValue((int)FedExProfileFieldIndex.CustomsTinIssuingAuthority, value); }
		}

		/// <summary> Gets / sets related entity of type 'ShippingProfileEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned.<br/><br/>
		/// </summary>
		[Browsable(true)]
		[DataMember]
		public virtual ShippingProfileEntity ShippingProfile
		{
			get { return _shippingProfile; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncShippingProfile(value);
					CallSetRelatedEntityDuringDeserialization(value, "FedEx");
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
							((IEntity2)value).SetRelatedEntity(this, "FedEx");
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
			get { return (int)ShipWorks.Data.Model.EntityType.FedExProfileEntity; }
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
