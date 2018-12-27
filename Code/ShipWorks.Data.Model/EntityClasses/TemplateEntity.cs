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
	/// <summary>Entity class which represents the entity 'Template'.<br/><br/></summary>
	[Serializable]
	public partial class TemplateEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<TemplateComputerSettingsEntity> _computerSettings;
		private EntityCollection<TemplateStoreSettingsEntity> _storeSettings;
		private EntityCollection<TemplateUserSettingsEntity> _userSettings;
		private TemplateFolderEntity _parentFolder;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name ParentFolder</summary>
			public static readonly string ParentFolder = "ParentFolder";
			/// <summary>Member name ComputerSettings</summary>
			public static readonly string ComputerSettings = "ComputerSettings";
			/// <summary>Member name StoreSettings</summary>
			public static readonly string StoreSettings = "StoreSettings";
			/// <summary>Member name UserSettings</summary>
			public static readonly string UserSettings = "UserSettings";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static TemplateEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public TemplateEntity():base("TemplateEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public TemplateEntity(IEntityFields2 fields):base("TemplateEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this TemplateEntity</param>
		public TemplateEntity(IValidator validator):base("TemplateEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="templateID">PK value for Template which data should be fetched into this Template object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public TemplateEntity(System.Int64 templateID):base("TemplateEntity")
		{
			InitClassEmpty(null, null);
			this.TemplateID = templateID;
		}

		/// <summary> CTor</summary>
		/// <param name="templateID">PK value for Template which data should be fetched into this Template object</param>
		/// <param name="validator">The custom validator object for this TemplateEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public TemplateEntity(System.Int64 templateID, IValidator validator):base("TemplateEntity")
		{
			InitClassEmpty(validator, null);
			this.TemplateID = templateID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected TemplateEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_computerSettings = (EntityCollection<TemplateComputerSettingsEntity>)info.GetValue("_computerSettings", typeof(EntityCollection<TemplateComputerSettingsEntity>));
				_storeSettings = (EntityCollection<TemplateStoreSettingsEntity>)info.GetValue("_storeSettings", typeof(EntityCollection<TemplateStoreSettingsEntity>));
				_userSettings = (EntityCollection<TemplateUserSettingsEntity>)info.GetValue("_userSettings", typeof(EntityCollection<TemplateUserSettingsEntity>));
				_parentFolder = (TemplateFolderEntity)info.GetValue("_parentFolder", typeof(TemplateFolderEntity));
				if(_parentFolder!=null)
				{
					_parentFolder.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((TemplateFieldIndex)fieldIndex)
			{
				case TemplateFieldIndex.ParentFolderID:
					DesetupSyncParentFolder(true, false);
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
				case "ParentFolder":
					this.ParentFolder = (TemplateFolderEntity)entity;
					break;
				case "ComputerSettings":
					this.ComputerSettings.Add((TemplateComputerSettingsEntity)entity);
					break;
				case "StoreSettings":
					this.StoreSettings.Add((TemplateStoreSettingsEntity)entity);
					break;
				case "UserSettings":
					this.UserSettings.Add((TemplateUserSettingsEntity)entity);
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
				case "ParentFolder":
					toReturn.Add(Relations.TemplateFolderEntityUsingParentFolderID);
					break;
				case "ComputerSettings":
					toReturn.Add(Relations.TemplateComputerSettingsEntityUsingTemplateID);
					break;
				case "StoreSettings":
					toReturn.Add(Relations.TemplateStoreSettingsEntityUsingTemplateID);
					break;
				case "UserSettings":
					toReturn.Add(Relations.TemplateUserSettingsEntityUsingTemplateID);
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
				case "ParentFolder":
					SetupSyncParentFolder(relatedEntity);
					break;
				case "ComputerSettings":
					this.ComputerSettings.Add((TemplateComputerSettingsEntity)relatedEntity);
					break;
				case "StoreSettings":
					this.StoreSettings.Add((TemplateStoreSettingsEntity)relatedEntity);
					break;
				case "UserSettings":
					this.UserSettings.Add((TemplateUserSettingsEntity)relatedEntity);
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
				case "ParentFolder":
					DesetupSyncParentFolder(false, true);
					break;
				case "ComputerSettings":
					this.PerformRelatedEntityRemoval(this.ComputerSettings, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "StoreSettings":
					this.PerformRelatedEntityRemoval(this.StoreSettings, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "UserSettings":
					this.PerformRelatedEntityRemoval(this.UserSettings, relatedEntity, signalRelatedEntityManyToOne);
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
			if(_parentFolder!=null)
			{
				toReturn.Add(_parentFolder);
			}
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.Add(this.ComputerSettings);
			toReturn.Add(this.StoreSettings);
			toReturn.Add(this.UserSettings);
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
				info.AddValue("_computerSettings", ((_computerSettings!=null) && (_computerSettings.Count>0) && !this.MarkedForDeletion)?_computerSettings:null);
				info.AddValue("_storeSettings", ((_storeSettings!=null) && (_storeSettings.Count>0) && !this.MarkedForDeletion)?_storeSettings:null);
				info.AddValue("_userSettings", ((_userSettings!=null) && (_userSettings.Count>0) && !this.MarkedForDeletion)?_userSettings:null);
				info.AddValue("_parentFolder", (!this.MarkedForDeletion?_parentFolder:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new TemplateRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'TemplateComputerSettings' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoComputerSettings()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(TemplateComputerSettingsFields.TemplateID, null, ComparisonOperator.Equal, this.TemplateID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'TemplateStoreSettings' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoStoreSettings()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(TemplateStoreSettingsFields.TemplateID, null, ComparisonOperator.Equal, this.TemplateID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'TemplateUserSettings' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUserSettings()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(TemplateUserSettingsFields.TemplateID, null, ComparisonOperator.Equal, this.TemplateID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'TemplateFolder' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoParentFolder()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(TemplateFolderFields.TemplateFolderID, null, ComparisonOperator.Equal, this.ParentFolderID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(TemplateEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._computerSettings);
			collectionsQueue.Enqueue(this._storeSettings);
			collectionsQueue.Enqueue(this._userSettings);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._computerSettings = (EntityCollection<TemplateComputerSettingsEntity>) collectionsQueue.Dequeue();
			this._storeSettings = (EntityCollection<TemplateStoreSettingsEntity>) collectionsQueue.Dequeue();
			this._userSettings = (EntityCollection<TemplateUserSettingsEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._computerSettings != null);
			toReturn |=(this._storeSettings != null);
			toReturn |=(this._userSettings != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<TemplateComputerSettingsEntity>(EntityFactoryCache2.GetEntityFactory(typeof(TemplateComputerSettingsEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<TemplateStoreSettingsEntity>(EntityFactoryCache2.GetEntityFactory(typeof(TemplateStoreSettingsEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<TemplateUserSettingsEntity>(EntityFactoryCache2.GetEntityFactory(typeof(TemplateUserSettingsEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("ParentFolder", _parentFolder);
			toReturn.Add("ComputerSettings", _computerSettings);
			toReturn.Add("StoreSettings", _storeSettings);
			toReturn.Add("UserSettings", _userSettings);
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
			_fieldsCustomProperties.Add("TemplateID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("RowVersion", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ParentFolderID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Name", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Xsl", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Type", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Context", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OutputFormat", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("OutputEncoding", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PageMarginLeft", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PageMarginRight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PageMarginBottom", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PageMarginTop", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PageWidth", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PageHeight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("LabelSheetID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PrintCopies", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("PrintCollate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SaveFileName", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SaveFileFolder", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SaveFilePrompt", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SaveFileBOM", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("SaveFileOnlineResources", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _parentFolder</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncParentFolder(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _parentFolder, new PropertyChangedEventHandler( OnParentFolderPropertyChanged ), "ParentFolder", ShipWorks.Data.Model.RelationClasses.StaticTemplateRelations.TemplateFolderEntityUsingParentFolderIDStatic, true, signalRelatedEntity, "Templates", resetFKFields, new int[] { (int)TemplateFieldIndex.ParentFolderID } );
			_parentFolder = null;
		}

		/// <summary> setups the sync logic for member _parentFolder</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncParentFolder(IEntityCore relatedEntity)
		{
			if(_parentFolder!=relatedEntity)
			{
				DesetupSyncParentFolder(true, true);
				_parentFolder = (TemplateFolderEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _parentFolder, new PropertyChangedEventHandler( OnParentFolderPropertyChanged ), "ParentFolder", ShipWorks.Data.Model.RelationClasses.StaticTemplateRelations.TemplateFolderEntityUsingParentFolderIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnParentFolderPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this TemplateEntity</param>
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
		public  static TemplateRelations Relations
		{
			get	{ return new TemplateRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'TemplateComputerSettings' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathComputerSettings
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<TemplateComputerSettingsEntity>(EntityFactoryCache2.GetEntityFactory(typeof(TemplateComputerSettingsEntityFactory))), (IEntityRelation)GetRelationsForField("ComputerSettings")[0], (int)ShipWorks.Data.Model.EntityType.TemplateEntity, (int)ShipWorks.Data.Model.EntityType.TemplateComputerSettingsEntity, 0, null, null, null, null, "ComputerSettings", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'TemplateStoreSettings' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathStoreSettings
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<TemplateStoreSettingsEntity>(EntityFactoryCache2.GetEntityFactory(typeof(TemplateStoreSettingsEntityFactory))), (IEntityRelation)GetRelationsForField("StoreSettings")[0], (int)ShipWorks.Data.Model.EntityType.TemplateEntity, (int)ShipWorks.Data.Model.EntityType.TemplateStoreSettingsEntity, 0, null, null, null, null, "StoreSettings", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'TemplateUserSettings' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUserSettings
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<TemplateUserSettingsEntity>(EntityFactoryCache2.GetEntityFactory(typeof(TemplateUserSettingsEntityFactory))), (IEntityRelation)GetRelationsForField("UserSettings")[0], (int)ShipWorks.Data.Model.EntityType.TemplateEntity, (int)ShipWorks.Data.Model.EntityType.TemplateUserSettingsEntity, 0, null, null, null, null, "UserSettings", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'TemplateFolder' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathParentFolder
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(TemplateFolderEntityFactory))),	(IEntityRelation)GetRelationsForField("ParentFolder")[0], (int)ShipWorks.Data.Model.EntityType.TemplateEntity, (int)ShipWorks.Data.Model.EntityType.TemplateFolderEntity, 0, null, null, null, null, "ParentFolder", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The TemplateID property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."TemplateID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 TemplateID
		{
			get { return (System.Int64)GetValue((int)TemplateFieldIndex.TemplateID, true); }
			set	{ SetValue((int)TemplateFieldIndex.TemplateID, value); }
		}

		/// <summary> The RowVersion property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."RowVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] RowVersion
		{
			get { return (System.Byte[])GetValue((int)TemplateFieldIndex.RowVersion, true); }

		}

		/// <summary> The ParentFolderID property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."ParentFolderID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 ParentFolderID
		{
			get { return (System.Int64)GetValue((int)TemplateFieldIndex.ParentFolderID, true); }
			set	{ SetValue((int)TemplateFieldIndex.ParentFolderID, value); }
		}

		/// <summary> The Name property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."Name"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Name
		{
			get { return (System.String)GetValue((int)TemplateFieldIndex.Name, true); }
			set	{ SetValue((int)TemplateFieldIndex.Name, value); }
		}

		/// <summary> The Xsl property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."Xsl"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Xsl
		{
			get { return (System.String)GetValue((int)TemplateFieldIndex.Xsl, true); }
			set	{ SetValue((int)TemplateFieldIndex.Xsl, value); }
		}

		/// <summary> The Type property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."Type"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 Type
		{
			get { return (System.Int32)GetValue((int)TemplateFieldIndex.Type, true); }
			set	{ SetValue((int)TemplateFieldIndex.Type, value); }
		}

		/// <summary> The Context property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."Context"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 Context
		{
			get { return (System.Int32)GetValue((int)TemplateFieldIndex.Context, true); }
			set	{ SetValue((int)TemplateFieldIndex.Context, value); }
		}

		/// <summary> The OutputFormat property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."OutputFormat"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 OutputFormat
		{
			get { return (System.Int32)GetValue((int)TemplateFieldIndex.OutputFormat, true); }
			set	{ SetValue((int)TemplateFieldIndex.OutputFormat, value); }
		}

		/// <summary> The OutputEncoding property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."OutputEncoding"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String OutputEncoding
		{
			get { return (System.String)GetValue((int)TemplateFieldIndex.OutputEncoding, true); }
			set	{ SetValue((int)TemplateFieldIndex.OutputEncoding, value); }
		}

		/// <summary> The PageMarginLeft property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."PageMarginLeft"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double PageMarginLeft
		{
			get { return (System.Double)GetValue((int)TemplateFieldIndex.PageMarginLeft, true); }
			set	{ SetValue((int)TemplateFieldIndex.PageMarginLeft, value); }
		}

		/// <summary> The PageMarginRight property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."PageMarginRight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double PageMarginRight
		{
			get { return (System.Double)GetValue((int)TemplateFieldIndex.PageMarginRight, true); }
			set	{ SetValue((int)TemplateFieldIndex.PageMarginRight, value); }
		}

		/// <summary> The PageMarginBottom property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."PageMarginBottom"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double PageMarginBottom
		{
			get { return (System.Double)GetValue((int)TemplateFieldIndex.PageMarginBottom, true); }
			set	{ SetValue((int)TemplateFieldIndex.PageMarginBottom, value); }
		}

		/// <summary> The PageMarginTop property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."PageMarginTop"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double PageMarginTop
		{
			get { return (System.Double)GetValue((int)TemplateFieldIndex.PageMarginTop, true); }
			set	{ SetValue((int)TemplateFieldIndex.PageMarginTop, value); }
		}

		/// <summary> The PageWidth property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."PageWidth"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double PageWidth
		{
			get { return (System.Double)GetValue((int)TemplateFieldIndex.PageWidth, true); }
			set	{ SetValue((int)TemplateFieldIndex.PageWidth, value); }
		}

		/// <summary> The PageHeight property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."PageHeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Double PageHeight
		{
			get { return (System.Double)GetValue((int)TemplateFieldIndex.PageHeight, true); }
			set	{ SetValue((int)TemplateFieldIndex.PageHeight, value); }
		}

		/// <summary> The LabelSheetID property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."LabelSheetID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 LabelSheetID
		{
			get { return (System.Int64)GetValue((int)TemplateFieldIndex.LabelSheetID, true); }
			set	{ SetValue((int)TemplateFieldIndex.LabelSheetID, value); }
		}

		/// <summary> The PrintCopies property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."PrintCopies"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 PrintCopies
		{
			get { return (System.Int32)GetValue((int)TemplateFieldIndex.PrintCopies, true); }
			set	{ SetValue((int)TemplateFieldIndex.PrintCopies, value); }
		}

		/// <summary> The PrintCollate property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."PrintCollate"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean PrintCollate
		{
			get { return (System.Boolean)GetValue((int)TemplateFieldIndex.PrintCollate, true); }
			set	{ SetValue((int)TemplateFieldIndex.PrintCollate, value); }
		}

		/// <summary> The SaveFileName property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."SaveFileName"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String SaveFileName
		{
			get { return (System.String)GetValue((int)TemplateFieldIndex.SaveFileName, true); }
			set	{ SetValue((int)TemplateFieldIndex.SaveFileName, value); }
		}

		/// <summary> The SaveFileFolder property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."SaveFileFolder"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String SaveFileFolder
		{
			get { return (System.String)GetValue((int)TemplateFieldIndex.SaveFileFolder, true); }
			set	{ SetValue((int)TemplateFieldIndex.SaveFileFolder, value); }
		}

		/// <summary> The SaveFilePrompt property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."SaveFilePrompt"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 SaveFilePrompt
		{
			get { return (System.Int32)GetValue((int)TemplateFieldIndex.SaveFilePrompt, true); }
			set	{ SetValue((int)TemplateFieldIndex.SaveFilePrompt, value); }
		}

		/// <summary> The SaveFileBOM property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."SaveFileBOM"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean SaveFileBOM
		{
			get { return (System.Boolean)GetValue((int)TemplateFieldIndex.SaveFileBOM, true); }
			set	{ SetValue((int)TemplateFieldIndex.SaveFileBOM, value); }
		}

		/// <summary> The SaveFileOnlineResources property of the Entity Template<br/><br/></summary>
		/// <remarks>Mapped on  table field: "Template"."SaveFileOnlineResources"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean SaveFileOnlineResources
		{
			get { return (System.Boolean)GetValue((int)TemplateFieldIndex.SaveFileOnlineResources, true); }
			set	{ SetValue((int)TemplateFieldIndex.SaveFileOnlineResources, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'TemplateComputerSettingsEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(TemplateComputerSettingsEntity))]
		public virtual EntityCollection<TemplateComputerSettingsEntity> ComputerSettings
		{
			get { return GetOrCreateEntityCollection<TemplateComputerSettingsEntity, TemplateComputerSettingsEntityFactory>("Template", true, false, ref _computerSettings);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'TemplateStoreSettingsEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(TemplateStoreSettingsEntity))]
		public virtual EntityCollection<TemplateStoreSettingsEntity> StoreSettings
		{
			get { return GetOrCreateEntityCollection<TemplateStoreSettingsEntity, TemplateStoreSettingsEntityFactory>("Template", true, false, ref _storeSettings);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'TemplateUserSettingsEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(TemplateUserSettingsEntity))]
		public virtual EntityCollection<TemplateUserSettingsEntity> UserSettings
		{
			get { return GetOrCreateEntityCollection<TemplateUserSettingsEntity, TemplateUserSettingsEntityFactory>("Template", true, false, ref _userSettings);	}
		}

		/// <summary> Gets / sets related entity of type 'TemplateFolderEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual TemplateFolderEntity ParentFolder
		{
			get	{ return _parentFolder; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncParentFolder(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "Templates", "ParentFolder", _parentFolder, true); 
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
			get { return (int)ShipWorks.Data.Model.EntityType.TemplateEntity; }
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
