﻿///////////////////////////////////////////////////////////////
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
	/// Entity class which represents the entity 'FilterNodeColumnSettings'.<br/><br/>
	/// 
	/// </summary>
	[Serializable]
	public partial class FilterNodeColumnSettingsEntity : CommonEntityBase, ISerializable
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations


		private FilterNodeEntity _filterNode;

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
			/// <summary>Member name FilterNode</summary>
			public static readonly string FilterNode = "FilterNode";

			/// <summary>Member name User</summary>
			public static readonly string User = "User";



		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static FilterNodeColumnSettingsEntity()
		{
			SetupCustomPropertyHashtables();
		}

		/// <summary> CTor</summary>
		public FilterNodeColumnSettingsEntity():base("FilterNodeColumnSettingsEntity")
		{
			InitClassEmpty(null, CreateFields());
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public FilterNodeColumnSettingsEntity(IEntityFields2 fields):base("FilterNodeColumnSettingsEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this FilterNodeColumnSettingsEntity</param>
		public FilterNodeColumnSettingsEntity(IValidator validator):base("FilterNodeColumnSettingsEntity")
		{
			InitClassEmpty(validator, CreateFields());
		}
				

		/// <summary> CTor</summary>
		/// <param name="filterNodeColumnSettingsID">PK value for FilterNodeColumnSettings which data should be fetched into this FilterNodeColumnSettings object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public FilterNodeColumnSettingsEntity(System.Int64 filterNodeColumnSettingsID):base("FilterNodeColumnSettingsEntity")
		{
			InitClassEmpty(null, CreateFields());
			this.FilterNodeColumnSettingsID = filterNodeColumnSettingsID;
		}

		/// <summary> CTor</summary>
		/// <param name="filterNodeColumnSettingsID">PK value for FilterNodeColumnSettings which data should be fetched into this FilterNodeColumnSettings object</param>
		/// <param name="validator">The custom validator object for this FilterNodeColumnSettingsEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public FilterNodeColumnSettingsEntity(System.Int64 filterNodeColumnSettingsID, IValidator validator):base("FilterNodeColumnSettingsEntity")
		{
			InitClassEmpty(validator, CreateFields());
			this.FilterNodeColumnSettingsID = filterNodeColumnSettingsID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected FilterNodeColumnSettingsEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{


				_filterNode = (FilterNodeEntity)info.GetValue("_filterNode", typeof(FilterNodeEntity));
				if(_filterNode!=null)
				{
					_filterNode.AfterSave+=new EventHandler(OnEntityAfterSave);
				}

				_user = (UserEntity)info.GetValue("_user", typeof(UserEntity));
				if(_user!=null)
				{
					_user.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((FilterNodeColumnSettingsFieldIndex)fieldIndex)
			{
				case FilterNodeColumnSettingsFieldIndex.UserID:
					DesetupSyncUser(true, false);
					break;
				case FilterNodeColumnSettingsFieldIndex.FilterNodeID:
					DesetupSyncFilterNode(true, false);
					break;
				case FilterNodeColumnSettingsFieldIndex.GridColumnLayoutID:

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
				case "FilterNode":
					this.FilterNode = (FilterNodeEntity)entity;
					break;

				case "User":
					this.User = (UserEntity)entity;
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
			return FilterNodeColumnSettingsEntity.GetRelationsForField(fieldName);
		}

		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		public static RelationCollection GetRelationsForField(string fieldName)
		{
			RelationCollection toReturn = new RelationCollection();
			switch(fieldName)
			{
				case "FilterNode":
					toReturn.Add(FilterNodeColumnSettingsEntity.Relations.FilterNodeEntityUsingFilterNodeID);
					break;

				case "User":
					toReturn.Add(FilterNodeColumnSettingsEntity.Relations.UserEntityUsingUserID);
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
			int numberOfOneWayRelations = 0+1+1;
			switch(propertyName)
			{
				case null:
					return ((numberOfOneWayRelations > 0) || base.CheckOneWayRelations(null));
				case "FilterNode":
					return true;

				case "User":
					return true;

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
				case "FilterNode":
					SetupSyncFilterNode(relatedEntity);
					break;

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
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void UnsetRelatedEntity(IEntity2 relatedEntity, string fieldName, bool signalRelatedEntityManyToOne)
		{
			switch(fieldName)
			{
				case "FilterNode":
					DesetupSyncFilterNode(false, true);
					break;

				case "User":
					DesetupSyncUser(false, true);
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
			if(_filterNode!=null)
			{
				toReturn.Add(_filterNode);
			}

			if(_user!=null)
			{
				toReturn.Add(_user);
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


				info.AddValue("_filterNode", (!this.MarkedForDeletion?_filterNode:null));

				info.AddValue("_user", (!this.MarkedForDeletion?_user:null));

			}
			
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}

		/// <summary>Returns true if the original value for the field with the fieldIndex passed in, read from the persistent storage was NULL, false otherwise.
		/// Should not be used for testing if the current value is NULL, use <see cref="TestCurrentFieldValueForNull"/> for that.</summary>
		/// <param name="fieldIndex">Index of the field to test if that field was NULL in the persistent storage</param>
		/// <returns>true if the field with the passed in index was NULL in the persistent storage, false otherwise</returns>
		public bool TestOriginalFieldValueForNull(FilterNodeColumnSettingsFieldIndex fieldIndex)
		{
			return base.Fields[(int)fieldIndex].IsNull;
		}
		
		/// <summary>Returns true if the current value for the field with the fieldIndex passed in represents null/not defined, false otherwise.
		/// Should not be used for testing if the original value (read from the db) is NULL</summary>
		/// <param name="fieldIndex">Index of the field to test if its currentvalue is null/undefined</param>
		/// <returns>true if the field's value isn't defined yet, false otherwise</returns>
		public bool TestCurrentFieldValueForNull(FilterNodeColumnSettingsFieldIndex fieldIndex)
		{
			return base.CheckIfCurrentFieldValueIsNull((int)fieldIndex);
		}

				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			return new FilterNodeColumnSettingsRelations().GetAllRelations();
		}
		



		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'FilterNode' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoFilterNode()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(FilterNodeFields.FilterNodeID, null, ComparisonOperator.Equal, this.FilterNodeID));
			return bucket;
		}


		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'User' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUser()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UserFields.UserID, null, ComparisonOperator.Equal, this.UserID));
			return bucket;
		}

	
		
		/// <summary>Creates entity fields object for this entity. Used in constructor to setup this entity in a polymorphic scenario.</summary>
		protected virtual IEntityFields2 CreateFields()
		{
			return EntityFieldsFactory.CreateEntityFieldsObject(ShipWorks.Data.Model.EntityType.FilterNodeColumnSettingsEntity);
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
			return EntityFactoryCache2.GetEntityFactory(typeof(FilterNodeColumnSettingsEntityFactory));
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
			toReturn.Add("FilterNode", _filterNode);

			toReturn.Add("User", _user);



			return toReturn;
		}
		
		/// <summary> Adds the internals to the active context. </summary>
		protected override void AddInternalsToContext()
		{


			if(_filterNode!=null)
			{
				_filterNode.ActiveContext = base.ActiveContext;
			}

			if(_user!=null)
			{
				_user.ActiveContext = base.ActiveContext;
			}

		}

		/// <summary> Initializes the class members</summary>
		protected virtual void InitClassMembers()
		{



			_filterNode = null;

			_user = null;

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

			_fieldsCustomProperties.Add("FilterNodeColumnSettingsID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("UserID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("FilterNodeID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Inherit", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("GridColumnLayoutID", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _filterNode</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncFilterNode(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _filterNode, new PropertyChangedEventHandler( OnFilterNodePropertyChanged ), "FilterNode", FilterNodeColumnSettingsEntity.Relations.FilterNodeEntityUsingFilterNodeID, true, signalRelatedEntity, "", resetFKFields, new int[] { (int)FilterNodeColumnSettingsFieldIndex.FilterNodeID } );		
			_filterNode = null;
		}

		/// <summary> setups the sync logic for member _filterNode</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncFilterNode(IEntity2 relatedEntity)
		{
			if(_filterNode!=relatedEntity)
			{
				DesetupSyncFilterNode(true, true);
				_filterNode = (FilterNodeEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _filterNode, new PropertyChangedEventHandler( OnFilterNodePropertyChanged ), "FilterNode", FilterNodeColumnSettingsEntity.Relations.FilterNodeEntityUsingFilterNodeID, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnFilterNodePropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}


		/// <summary> Removes the sync logic for member _user</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncUser(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _user, new PropertyChangedEventHandler( OnUserPropertyChanged ), "User", FilterNodeColumnSettingsEntity.Relations.UserEntityUsingUserID, true, signalRelatedEntity, "", resetFKFields, new int[] { (int)FilterNodeColumnSettingsFieldIndex.UserID } );		
			_user = null;
		}

		/// <summary> setups the sync logic for member _user</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncUser(IEntity2 relatedEntity)
		{
			if(_user!=relatedEntity)
			{
				DesetupSyncUser(true, true);
				_user = (UserEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _user, new PropertyChangedEventHandler( OnUserPropertyChanged ), "User", FilterNodeColumnSettingsEntity.Relations.UserEntityUsingUserID, true, new string[] {  } );
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
		/// <param name="validator">The validator object for this FilterNodeColumnSettingsEntity</param>
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
		public  static FilterNodeColumnSettingsRelations Relations
		{
			get	{ return new FilterNodeColumnSettingsRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}



		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'FilterNode' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathFilterNode
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(FilterNodeEntityFactory))),
					(IEntityRelation)GetRelationsForField("FilterNode")[0], (int)ShipWorks.Data.Model.EntityType.FilterNodeColumnSettingsEntity, (int)ShipWorks.Data.Model.EntityType.FilterNodeEntity, 0, null, null, null, null, "FilterNode", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne);
			}
		}


		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'User' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUser
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(UserEntityFactory))),
					(IEntityRelation)GetRelationsForField("User")[0], (int)ShipWorks.Data.Model.EntityType.FilterNodeColumnSettingsEntity, (int)ShipWorks.Data.Model.EntityType.UserEntity, 0, null, null, null, null, "User", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne);
			}
		}


		/// <summary> The custom properties for the type of this entity instance.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		public override Dictionary<string, string> CustomPropertiesOfType
		{
			get { return FilterNodeColumnSettingsEntity.CustomProperties;}
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
			get { return FilterNodeColumnSettingsEntity.FieldsCustomProperties;}
		}

		/// <summary> The FilterNodeColumnSettingsID property of the Entity FilterNodeColumnSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FilterNodeColumnSettings"."FilterNodeColumnSettingsID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 FilterNodeColumnSettingsID
		{
			get { return (System.Int64)GetValue((int)FilterNodeColumnSettingsFieldIndex.FilterNodeColumnSettingsID, true); }
			set	{ SetValue((int)FilterNodeColumnSettingsFieldIndex.FilterNodeColumnSettingsID, value); }
		}

		/// <summary> The UserID property of the Entity FilterNodeColumnSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FilterNodeColumnSettings"."UserID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> UserID
		{
			get { return (Nullable<System.Int64>)GetValue((int)FilterNodeColumnSettingsFieldIndex.UserID, false); }
			set	{ SetValue((int)FilterNodeColumnSettingsFieldIndex.UserID, value); }
		}

		/// <summary> The FilterNodeID property of the Entity FilterNodeColumnSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FilterNodeColumnSettings"."FilterNodeID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 FilterNodeID
		{
			get { return (System.Int64)GetValue((int)FilterNodeColumnSettingsFieldIndex.FilterNodeID, true); }
			set	{ SetValue((int)FilterNodeColumnSettingsFieldIndex.FilterNodeID, value); }
		}

		/// <summary> The Inherit property of the Entity FilterNodeColumnSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FilterNodeColumnSettings"."Inherit"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean Inherit
		{
			get { return (System.Boolean)GetValue((int)FilterNodeColumnSettingsFieldIndex.Inherit, true); }
			set	{ SetValue((int)FilterNodeColumnSettingsFieldIndex.Inherit, value); }
		}

		/// <summary> The GridColumnLayoutID property of the Entity FilterNodeColumnSettings<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "FilterNodeColumnSettings"."GridColumnLayoutID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 GridColumnLayoutID
		{
			get { return (System.Int64)GetValue((int)FilterNodeColumnSettingsFieldIndex.GridColumnLayoutID, true); }
			set	{ SetValue((int)FilterNodeColumnSettingsFieldIndex.GridColumnLayoutID, value); }
		}



		/// <summary> Gets / sets related entity of type 'FilterNodeEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual FilterNodeEntity FilterNode
		{
			get
			{
				return _filterNode;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncFilterNode(value);
				}
				else
				{
					if(value==null)
					{
						if(_filterNode != null)
						{
							UnsetRelatedEntity(_filterNode, "FilterNode");
						}
					}
					else
					{
						if(_filterNode!=value)
						{
							SetRelatedEntity((IEntity2)value, "FilterNode");
						}
					}
				}
			}
		}


		/// <summary> Gets / sets related entity of type 'UserEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual UserEntity User
		{
			get
			{
				return _user;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncUser(value);
				}
				else
				{
					if(value==null)
					{
						if(_user != null)
						{
							UnsetRelatedEntity(_user, "User");
						}
					}
					else
					{
						if(_user!=value)
						{
							SetRelatedEntity((IEntity2)value, "User");
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
			get { return (int)ShipWorks.Data.Model.EntityType.FilterNodeColumnSettingsEntity; }
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
