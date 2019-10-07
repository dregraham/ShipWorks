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
	/// <summary>Entity class which represents the entity 'UserSettings'.<br/><br/></summary>
	[Serializable]
	public partial class UserSettingsEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private UserEntity _user;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name User</summary>
			public static readonly string User = "User";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static UserSettingsEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public UserSettingsEntity():base("UserSettingsEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public UserSettingsEntity(IEntityFields2 fields):base("UserSettingsEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this UserSettingsEntity</param>
		public UserSettingsEntity(IValidator validator):base("UserSettingsEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="userID">PK value for UserSettings which data should be fetched into this UserSettings object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public UserSettingsEntity(System.Int64 userID):base("UserSettingsEntity")
		{
			InitClassEmpty(null, null);
			this.UserID = userID;
		}

		/// <summary> CTor</summary>
		/// <param name="userID">PK value for UserSettings which data should be fetched into this UserSettings object</param>
		/// <param name="validator">The custom validator object for this UserSettingsEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public UserSettingsEntity(System.Int64 userID, IValidator validator):base("UserSettingsEntity")
		{
			InitClassEmpty(validator, null);
			this.UserID = userID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected UserSettingsEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_user = (UserEntity)info.GetValue("_user", typeof(UserEntity));
				if(_user!=null)
				{
					_user.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((UserSettingsFieldIndex)fieldIndex)
			{
				case UserSettingsFieldIndex.UserID:
					DesetupSyncUser(true, false);
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
				case "User":
					this.User = (UserEntity)entity;
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
				case "User":
					toReturn.Add(Relations.UserEntityUsingUserID);
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
				case "User":
					SetupSyncUser(relatedEntity);
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
				case "User":
					DesetupSyncUser(false, true);
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
			if(_user!=null)
			{
				toReturn.Add(_user);
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
				info.AddValue("_user", (!this.MarkedForDeletion?_user:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new UserSettingsRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'User' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUser()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UserFields.UserID, null, ComparisonOperator.Equal, this.UserID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(UserSettingsEntityFactory));
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
			toReturn.Add("User", _user);
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
			_fieldsCustomProperties.Add("UserID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DisplayColorScheme", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DisplaySystemTray", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("WindowLayout", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("GridMenuLayout", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FilterInitialUseLastActive", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FilterInitialSpecified", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("FilterInitialSortType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OrderFilterLastActive", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OrderFilterExpandedFolders", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShippingWeightFormat", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("TemplateExpandedFolders", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("TemplateLastSelected", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CustomerFilterLastActive", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CustomerFilterExpandedFolders", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SingleScanSettings", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AutoWeigh", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DialogSettings", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("UIMode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OrderLookupLayout", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("LastReleaseNotesSeen", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AutoPrintRequireValidation", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("MinimizeRibbon", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ShowQAToolbarBelowRibbon", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _user</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncUser(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _user, new PropertyChangedEventHandler( OnUserPropertyChanged ), "User", ShipWorks.Data.Model.RelationClasses.StaticUserSettingsRelations.UserEntityUsingUserIDStatic, true, signalRelatedEntity, "Settings", false, new int[] { (int)UserSettingsFieldIndex.UserID } );
			_user = null;
		}
		
		/// <summary> setups the sync logic for member _user</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncUser(IEntityCore relatedEntity)
		{
			if(_user!=relatedEntity)
			{
				DesetupSyncUser(true, true);
				_user = (UserEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _user, new PropertyChangedEventHandler( OnUserPropertyChanged ), "User", ShipWorks.Data.Model.RelationClasses.StaticUserSettingsRelations.UserEntityUsingUserIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnUserPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this UserSettingsEntity</param>
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
		public  static UserSettingsRelations Relations
		{
			get	{ return new UserSettingsRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'User' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUser
		{
			get { return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(UserEntityFactory))), (IEntityRelation)GetRelationsForField("User")[0], (int)ShipWorks.Data.Model.EntityType.UserSettingsEntity, (int)ShipWorks.Data.Model.EntityType.UserEntity, 0, null, null, null, null, "User", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);	}
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

		/// <summary> The UserID property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."UserID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
		public virtual System.Int64 UserID
		{
			get { return (System.Int64)GetValue((int)UserSettingsFieldIndex.UserID, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.UserID, value); }
		}

		/// <summary> The DisplayColorScheme property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."DisplayColorScheme"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 DisplayColorScheme
		{
			get { return (System.Int32)GetValue((int)UserSettingsFieldIndex.DisplayColorScheme, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.DisplayColorScheme, value); }
		}

		/// <summary> The DisplaySystemTray property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."DisplaySystemTray"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean DisplaySystemTray
		{
			get { return (System.Boolean)GetValue((int)UserSettingsFieldIndex.DisplaySystemTray, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.DisplaySystemTray, value); }
		}

		/// <summary> The WindowLayout property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."WindowLayout"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] WindowLayout
		{
			get { return (System.Byte[])GetValue((int)UserSettingsFieldIndex.WindowLayout, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.WindowLayout, value); }
		}

		/// <summary> The GridMenuLayout property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."GridMenuLayout"<br/>
		/// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String GridMenuLayout
		{
			get { return (System.String)GetValue((int)UserSettingsFieldIndex.GridMenuLayout, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.GridMenuLayout, value); }
		}

		/// <summary> The FilterInitialUseLastActive property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."FilterInitialUseLastActive"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean FilterInitialUseLastActive
		{
			get { return (System.Boolean)GetValue((int)UserSettingsFieldIndex.FilterInitialUseLastActive, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.FilterInitialUseLastActive, value); }
		}

		/// <summary> The FilterInitialSpecified property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."FilterInitialSpecified"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 FilterInitialSpecified
		{
			get { return (System.Int64)GetValue((int)UserSettingsFieldIndex.FilterInitialSpecified, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.FilterInitialSpecified, value); }
		}

		/// <summary> The FilterInitialSortType property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."FilterInitialSortType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 FilterInitialSortType
		{
			get { return (System.Int32)GetValue((int)UserSettingsFieldIndex.FilterInitialSortType, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.FilterInitialSortType, value); }
		}

		/// <summary> The OrderFilterLastActive property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."OrderFilterLastActive"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 OrderFilterLastActive
		{
			get { return (System.Int64)GetValue((int)UserSettingsFieldIndex.OrderFilterLastActive, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.OrderFilterLastActive, value); }
		}

		/// <summary> The OrderFilterExpandedFolders property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."OrderFilterExpandedFolders"<br/>
		/// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String OrderFilterExpandedFolders
		{
			get { return (System.String)GetValue((int)UserSettingsFieldIndex.OrderFilterExpandedFolders, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.OrderFilterExpandedFolders, value); }
		}

		/// <summary> The ShippingWeightFormat property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."ShippingWeightFormat"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShippingWeightFormat
		{
			get { return (System.Int32)GetValue((int)UserSettingsFieldIndex.ShippingWeightFormat, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.ShippingWeightFormat, value); }
		}

		/// <summary> The TemplateExpandedFolders property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."TemplateExpandedFolders"<br/>
		/// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String TemplateExpandedFolders
		{
			get { return (System.String)GetValue((int)UserSettingsFieldIndex.TemplateExpandedFolders, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.TemplateExpandedFolders, value); }
		}

		/// <summary> The TemplateLastSelected property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."TemplateLastSelected"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 TemplateLastSelected
		{
			get { return (System.Int64)GetValue((int)UserSettingsFieldIndex.TemplateLastSelected, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.TemplateLastSelected, value); }
		}

		/// <summary> The CustomerFilterLastActive property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."CustomerFilterLastActive"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 CustomerFilterLastActive
		{
			get { return (System.Int64)GetValue((int)UserSettingsFieldIndex.CustomerFilterLastActive, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.CustomerFilterLastActive, value); }
		}

		/// <summary> The CustomerFilterExpandedFolders property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."CustomerFilterExpandedFolders"<br/>
		/// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String CustomerFilterExpandedFolders
		{
			get { return (System.String)GetValue((int)UserSettingsFieldIndex.CustomerFilterExpandedFolders, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.CustomerFilterExpandedFolders, value); }
		}

		/// <summary> The SingleScanSettings property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."SingleScanSettings"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 SingleScanSettings
		{
			get { return (System.Int32)GetValue((int)UserSettingsFieldIndex.SingleScanSettings, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.SingleScanSettings, value); }
		}

		/// <summary> The AutoWeigh property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."AutoWeigh"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean AutoWeigh
		{
			get { return (System.Boolean)GetValue((int)UserSettingsFieldIndex.AutoWeigh, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.AutoWeigh, value); }
		}

		/// <summary> The DialogSettings property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."DialogSettings"<br/>
		/// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String DialogSettings
		{
			get { return (System.String)GetValue((int)UserSettingsFieldIndex.DialogSettings, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.DialogSettings, value); }
		}

		/// <summary> The UIMode property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."UIMode"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual ShipWorks.Settings.UIMode UIMode
		{
			get { return (ShipWorks.Settings.UIMode)GetValue((int)UserSettingsFieldIndex.UIMode, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.UIMode, value); }
		}

		/// <summary> The OrderLookupLayout property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."OrderLookupLayout"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String OrderLookupLayout
		{
			get { return (System.String)GetValue((int)UserSettingsFieldIndex.OrderLookupLayout, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.OrderLookupLayout, value); }
		}

		/// <summary> The LastReleaseNotesSeen property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."LastReleaseNotesSeen"<br/>
		/// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 25<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String LastReleaseNotesSeen
		{
			get { return (System.String)GetValue((int)UserSettingsFieldIndex.LastReleaseNotesSeen, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.LastReleaseNotesSeen, value); }
		}

		/// <summary> The AutoPrintRequireValidation property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."AutoPrintRequireValidation"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean AutoPrintRequireValidation
		{
			get { return (System.Boolean)GetValue((int)UserSettingsFieldIndex.AutoPrintRequireValidation, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.AutoPrintRequireValidation, value); }
		}

		/// <summary> The MinimizeRibbon property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."MinimizeRibbon"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean MinimizeRibbon
		{
			get { return (System.Boolean)GetValue((int)UserSettingsFieldIndex.MinimizeRibbon, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.MinimizeRibbon, value); }
		}

		/// <summary> The ShowQAToolbarBelowRibbon property of the Entity UserSettings<br/><br/></summary>
		/// <remarks>Mapped on  table field: "UserSettings"."ShowQAToolbarBelowRibbon"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean ShowQAToolbarBelowRibbon
		{
			get { return (System.Boolean)GetValue((int)UserSettingsFieldIndex.ShowQAToolbarBelowRibbon, true); }
			set	{ SetValue((int)UserSettingsFieldIndex.ShowQAToolbarBelowRibbon, value); }
		}

		/// <summary> Gets / sets related entity of type 'UserEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned.<br/><br/>
		/// </summary>
		[Browsable(true)]
		public virtual UserEntity User
		{
			get { return _user; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncUser(value);
					CallSetRelatedEntityDuringDeserialization(value, "Settings");
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_user !=null);
						DesetupSyncUser(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("User");
						}
					}
					else
					{
						if(_user!=value)
						{
							((IEntity2)value).SetRelatedEntity(this, "Settings");
							SetupSyncUser(value);
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
			get { return (int)ShipWorks.Data.Model.EntityType.UserSettingsEntity; }
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
