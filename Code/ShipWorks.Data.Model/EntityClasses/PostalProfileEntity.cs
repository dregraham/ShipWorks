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
	/// Entity class which represents the entity 'PostalProfile'.<br/><br/>
	/// 
	/// </summary>
	[Serializable]
	public partial class PostalProfileEntity : CommonEntityBase, ISerializable
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations



		private EndiciaProfileEntity _endicia;
		private ShippingProfileEntity _profile;
		private UspsProfileEntity _usps;
		
		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{



			/// <summary>Member name Endicia</summary>
			public static readonly string Endicia = "Endicia";
			/// <summary>Member name Profile</summary>
			public static readonly string Profile = "Profile";
			/// <summary>Member name Usps</summary>
			public static readonly string Usps = "Usps";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static PostalProfileEntity()
		{
			SetupCustomPropertyHashtables();
		}

		/// <summary> CTor</summary>
		public PostalProfileEntity():base("PostalProfileEntity")
		{
			InitClassEmpty(null, CreateFields());
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public PostalProfileEntity(IEntityFields2 fields):base("PostalProfileEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this PostalProfileEntity</param>
		public PostalProfileEntity(IValidator validator):base("PostalProfileEntity")
		{
			InitClassEmpty(validator, CreateFields());
		}
				

		/// <summary> CTor</summary>
		/// <param name="shippingProfileID">PK value for PostalProfile which data should be fetched into this PostalProfile object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public PostalProfileEntity(System.Int64 shippingProfileID):base("PostalProfileEntity")
		{
			InitClassEmpty(null, CreateFields());
			this.ShippingProfileID = shippingProfileID;
		}

		/// <summary> CTor</summary>
		/// <param name="shippingProfileID">PK value for PostalProfile which data should be fetched into this PostalProfile object</param>
		/// <param name="validator">The custom validator object for this PostalProfileEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public PostalProfileEntity(System.Int64 shippingProfileID, IValidator validator):base("PostalProfileEntity")
		{
			InitClassEmpty(validator, CreateFields());
			this.ShippingProfileID = shippingProfileID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected PostalProfileEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{



				_endicia = (EndiciaProfileEntity)info.GetValue("_endicia", typeof(EndiciaProfileEntity));
				if(_endicia!=null)
				{
					_endicia.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_profile = (ShippingProfileEntity)info.GetValue("_profile", typeof(ShippingProfileEntity));
				if(_profile!=null)
				{
					_profile.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_usps = (UspsProfileEntity)info.GetValue("_usps", typeof(UspsProfileEntity));
				if(_usps!=null)
				{
					_usps.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((PostalProfileFieldIndex)fieldIndex)
			{
				case PostalProfileFieldIndex.ShippingProfileID:
					DesetupSyncProfile(true, false);
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



				case "Endicia":
					this.Endicia = (EndiciaProfileEntity)entity;
					break;
				case "Profile":
					this.Profile = (ShippingProfileEntity)entity;
					break;
				case "Usps":
					this.Usps = (UspsProfileEntity)entity;
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
			return PostalProfileEntity.GetRelationsForField(fieldName);
		}

		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		public static RelationCollection GetRelationsForField(string fieldName)
		{
			RelationCollection toReturn = new RelationCollection();
			switch(fieldName)
			{



				case "Endicia":
					toReturn.Add(PostalProfileEntity.Relations.EndiciaProfileEntityUsingShippingProfileID);
					break;
				case "Profile":
					toReturn.Add(PostalProfileEntity.Relations.ShippingProfileEntityUsingShippingProfileID);
					break;
				case "Usps":
					toReturn.Add(PostalProfileEntity.Relations.UspsProfileEntityUsingShippingProfileID);
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


				case "Endicia":
					SetupSyncEndicia(relatedEntity);
					break;
				case "Profile":
					SetupSyncProfile(relatedEntity);
					break;
				case "Usps":
					SetupSyncUsps(relatedEntity);
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


				case "Endicia":
					DesetupSyncEndicia(false, true);
					break;
				case "Profile":
					DesetupSyncProfile(false, true);
					break;
				case "Usps":
					DesetupSyncUsps(false, true);
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
			if(_endicia!=null)
			{
				toReturn.Add(_endicia);
			}



			if(_usps!=null)
			{
				toReturn.Add(_usps);
			}

			return toReturn;
		}
		
		/// <summary> Gets a collection of related entities referenced by this entity which this entity depends on (this entity is the FK side of their PK fields). These
		/// entities will have to be persisted before this entity during a recursive save.</summary>
		/// <returns>Collection with 0 or more IEntity2 objects, referenced by this entity</returns>
		public override List<IEntity2> GetDependentRelatedEntities()
		{
			List<IEntity2> toReturn = new List<IEntity2>();



			if(_profile!=null)
			{
				toReturn.Add(_profile);
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



				info.AddValue("_endicia", (!this.MarkedForDeletion?_endicia:null));
				info.AddValue("_profile", (!this.MarkedForDeletion?_profile:null));
				info.AddValue("_usps", (!this.MarkedForDeletion?_usps:null));
			}
			
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}

		/// <summary>Returns true if the original value for the field with the fieldIndex passed in, read from the persistent storage was NULL, false otherwise.
		/// Should not be used for testing if the current value is NULL, use <see cref="TestCurrentFieldValueForNull"/> for that.</summary>
		/// <param name="fieldIndex">Index of the field to test if that field was NULL in the persistent storage</param>
		/// <returns>true if the field with the passed in index was NULL in the persistent storage, false otherwise</returns>
		public bool TestOriginalFieldValueForNull(PostalProfileFieldIndex fieldIndex)
		{
			return base.Fields[(int)fieldIndex].IsNull;
		}
		
		/// <summary>Returns true if the current value for the field with the fieldIndex passed in represents null/not defined, false otherwise.
		/// Should not be used for testing if the original value (read from the db) is NULL</summary>
		/// <param name="fieldIndex">Index of the field to test if its currentvalue is null/undefined</param>
		/// <returns>true if the field's value isn't defined yet, false otherwise</returns>
		public bool TestCurrentFieldValueForNull(PostalProfileFieldIndex fieldIndex)
		{
			return base.CheckIfCurrentFieldValueIsNull((int)fieldIndex);
		}

				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			return new PostalProfileRelations().GetAllRelations();
		}
		




		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'EndiciaProfile' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoEndicia()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(EndiciaProfileFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'ShippingProfile' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoProfile()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ShippingProfileFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'UspsProfile' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUsps()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UspsProfileFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}
	
		
		/// <summary>Creates entity fields object for this entity. Used in constructor to setup this entity in a polymorphic scenario.</summary>
		protected virtual IEntityFields2 CreateFields()
		{
			return EntityFieldsFactory.CreateEntityFieldsObject(ShipWorks.Data.Model.EntityType.PostalProfileEntity);
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
			return EntityFactoryCache2.GetEntityFactory(typeof(PostalProfileEntityFactory));
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



			toReturn.Add("Endicia", _endicia);
			toReturn.Add("Profile", _profile);
			toReturn.Add("Usps", _usps);
			return toReturn;
		}
		
		/// <summary> Adds the internals to the active context. </summary>
		protected override void AddInternalsToContext()
		{



			if(_endicia!=null)
			{
				_endicia.ActiveContext = base.ActiveContext;
			}
			if(_profile!=null)
			{
				_profile.ActiveContext = base.ActiveContext;
			}
			if(_usps!=null)
			{
				_usps.ActiveContext = base.ActiveContext;
			}
		}

		/// <summary> Initializes the class members</summary>
		protected virtual void InitClassMembers()
		{




			_endicia = null;
			_profile = null;
			_usps = null;
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

			_fieldsCustomProperties.Add("ShippingProfileID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Service", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Confirmation", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Weight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("PackagingType", fieldHashtable);
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

			_fieldsCustomProperties.Add("NonRectangular", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("NonMachinable", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsContentType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("CustomsContentDescription", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ExpressSignatureWaiver", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("SortType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("EntryFacility", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Memo1", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Memo2", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Memo3", fieldHashtable);
		}
		#endregion


		/// <summary> Removes the sync logic for member _endicia</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncEndicia(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _endicia, new PropertyChangedEventHandler( OnEndiciaPropertyChanged ), "Endicia", PostalProfileEntity.Relations.EndiciaProfileEntityUsingShippingProfileID, false, signalRelatedEntity, "PostalProfile", false, new int[] { (int)PostalProfileFieldIndex.ShippingProfileID } );
			_endicia = null;
		}
		
		/// <summary> setups the sync logic for member _endicia</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncEndicia(IEntity2 relatedEntity)
		{
			if(_endicia!=relatedEntity)
			{
				DesetupSyncEndicia(true, true);
				_endicia = (EndiciaProfileEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _endicia, new PropertyChangedEventHandler( OnEndiciaPropertyChanged ), "Endicia", PostalProfileEntity.Relations.EndiciaProfileEntityUsingShippingProfileID, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnEndiciaPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _profile</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncProfile(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _profile, new PropertyChangedEventHandler( OnProfilePropertyChanged ), "Profile", PostalProfileEntity.Relations.ShippingProfileEntityUsingShippingProfileID, true, signalRelatedEntity, "Postal", false, new int[] { (int)PostalProfileFieldIndex.ShippingProfileID } );
			_profile = null;
		}
		
		/// <summary> setups the sync logic for member _profile</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncProfile(IEntity2 relatedEntity)
		{
			if(_profile!=relatedEntity)
			{
				DesetupSyncProfile(true, true);
				_profile = (ShippingProfileEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _profile, new PropertyChangedEventHandler( OnProfilePropertyChanged ), "Profile", PostalProfileEntity.Relations.ShippingProfileEntityUsingShippingProfileID, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnProfilePropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _usps</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncUsps(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _usps, new PropertyChangedEventHandler( OnUspsPropertyChanged ), "Usps", PostalProfileEntity.Relations.UspsProfileEntityUsingShippingProfileID, false, signalRelatedEntity, "PostalProfile", false, new int[] { (int)PostalProfileFieldIndex.ShippingProfileID } );
			_usps = null;
		}
		
		/// <summary> setups the sync logic for member _usps</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncUsps(IEntity2 relatedEntity)
		{
			if(_usps!=relatedEntity)
			{
				DesetupSyncUsps(true, true);
				_usps = (UspsProfileEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _usps, new PropertyChangedEventHandler( OnUspsPropertyChanged ), "Usps", PostalProfileEntity.Relations.UspsProfileEntityUsingShippingProfileID, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnUspsPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this PostalProfileEntity</param>
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
		public  static PostalProfileRelations Relations
		{
			get	{ return new PostalProfileRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}




		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'EndiciaProfile' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathEndicia
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(EndiciaProfileEntityFactory))),
					(IEntityRelation)GetRelationsForField("Endicia")[0], (int)ShipWorks.Data.Model.EntityType.PostalProfileEntity, (int)ShipWorks.Data.Model.EntityType.EndiciaProfileEntity, 0, null, null, null, null, "Endicia", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);
			}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ShippingProfile' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathProfile
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(ShippingProfileEntityFactory))),
					(IEntityRelation)GetRelationsForField("Profile")[0], (int)ShipWorks.Data.Model.EntityType.PostalProfileEntity, (int)ShipWorks.Data.Model.EntityType.ShippingProfileEntity, 0, null, null, null, null, "Profile", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);
			}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UspsProfile' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUsps
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(UspsProfileEntityFactory))),
					(IEntityRelation)GetRelationsForField("Usps")[0], (int)ShipWorks.Data.Model.EntityType.PostalProfileEntity, (int)ShipWorks.Data.Model.EntityType.UspsProfileEntity, 0, null, null, null, null, "Usps", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);
			}
		}

		/// <summary> The custom properties for the type of this entity instance.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		public override Dictionary<string, string> CustomPropertiesOfType
		{
			get { return PostalProfileEntity.CustomProperties;}
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
			get { return PostalProfileEntity.FieldsCustomProperties;}
		}

		/// <summary> The ShippingProfileID property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."ShippingProfileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
		public virtual System.Int64 ShippingProfileID
		{
			get { return (System.Int64)GetValue((int)PostalProfileFieldIndex.ShippingProfileID, true); }
			set	{ SetValue((int)PostalProfileFieldIndex.ShippingProfileID, value); }
		}

		/// <summary> The Service property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."Service"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> Service
		{
			get { return (Nullable<System.Int32>)GetValue((int)PostalProfileFieldIndex.Service, false); }
			set	{ SetValue((int)PostalProfileFieldIndex.Service, value); }
		}

		/// <summary> The Confirmation property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."Confirmation"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> Confirmation
		{
			get { return (Nullable<System.Int32>)GetValue((int)PostalProfileFieldIndex.Confirmation, false); }
			set	{ SetValue((int)PostalProfileFieldIndex.Confirmation, value); }
		}

		/// <summary> The Weight property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."Weight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> Weight
		{
			get { return (Nullable<System.Double>)GetValue((int)PostalProfileFieldIndex.Weight, false); }
			set	{ SetValue((int)PostalProfileFieldIndex.Weight, value); }
		}

		/// <summary> The PackagingType property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."PackagingType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> PackagingType
		{
			get { return (Nullable<System.Int32>)GetValue((int)PostalProfileFieldIndex.PackagingType, false); }
			set	{ SetValue((int)PostalProfileFieldIndex.PackagingType, value); }
		}

		/// <summary> The DimsProfileID property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."DimsProfileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> DimsProfileID
		{
			get { return (Nullable<System.Int64>)GetValue((int)PostalProfileFieldIndex.DimsProfileID, false); }
			set	{ SetValue((int)PostalProfileFieldIndex.DimsProfileID, value); }
		}

		/// <summary> The DimsLength property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."DimsLength"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DimsLength
		{
			get { return (Nullable<System.Double>)GetValue((int)PostalProfileFieldIndex.DimsLength, false); }
			set	{ SetValue((int)PostalProfileFieldIndex.DimsLength, value); }
		}

		/// <summary> The DimsWidth property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."DimsWidth"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DimsWidth
		{
			get { return (Nullable<System.Double>)GetValue((int)PostalProfileFieldIndex.DimsWidth, false); }
			set	{ SetValue((int)PostalProfileFieldIndex.DimsWidth, value); }
		}

		/// <summary> The DimsHeight property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."DimsHeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DimsHeight
		{
			get { return (Nullable<System.Double>)GetValue((int)PostalProfileFieldIndex.DimsHeight, false); }
			set	{ SetValue((int)PostalProfileFieldIndex.DimsHeight, value); }
		}

		/// <summary> The DimsWeight property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."DimsWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Double> DimsWeight
		{
			get { return (Nullable<System.Double>)GetValue((int)PostalProfileFieldIndex.DimsWeight, false); }
			set	{ SetValue((int)PostalProfileFieldIndex.DimsWeight, value); }
		}

		/// <summary> The DimsAddWeight property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."DimsAddWeight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> DimsAddWeight
		{
			get { return (Nullable<System.Boolean>)GetValue((int)PostalProfileFieldIndex.DimsAddWeight, false); }
			set	{ SetValue((int)PostalProfileFieldIndex.DimsAddWeight, value); }
		}

		/// <summary> The NonRectangular property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."NonRectangular"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> NonRectangular
		{
			get { return (Nullable<System.Boolean>)GetValue((int)PostalProfileFieldIndex.NonRectangular, false); }
			set	{ SetValue((int)PostalProfileFieldIndex.NonRectangular, value); }
		}

		/// <summary> The NonMachinable property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."NonMachinable"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> NonMachinable
		{
			get { return (Nullable<System.Boolean>)GetValue((int)PostalProfileFieldIndex.NonMachinable, false); }
			set	{ SetValue((int)PostalProfileFieldIndex.NonMachinable, value); }
		}

		/// <summary> The CustomsContentType property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."CustomsContentType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> CustomsContentType
		{
			get { return (Nullable<System.Int32>)GetValue((int)PostalProfileFieldIndex.CustomsContentType, false); }
			set	{ SetValue((int)PostalProfileFieldIndex.CustomsContentType, value); }
		}

		/// <summary> The CustomsContentDescription property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."CustomsContentDescription"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String CustomsContentDescription
		{
			get { return (System.String)GetValue((int)PostalProfileFieldIndex.CustomsContentDescription, true); }
			set	{ SetValue((int)PostalProfileFieldIndex.CustomsContentDescription, value); }
		}

		/// <summary> The ExpressSignatureWaiver property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."ExpressSignatureWaiver"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> ExpressSignatureWaiver
		{
			get { return (Nullable<System.Boolean>)GetValue((int)PostalProfileFieldIndex.ExpressSignatureWaiver, false); }
			set	{ SetValue((int)PostalProfileFieldIndex.ExpressSignatureWaiver, value); }
		}

		/// <summary> The SortType property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."SortType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> SortType
		{
			get { return (Nullable<System.Int32>)GetValue((int)PostalProfileFieldIndex.SortType, false); }
			set	{ SetValue((int)PostalProfileFieldIndex.SortType, value); }
		}

		/// <summary> The EntryFacility property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."EntryFacility"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> EntryFacility
		{
			get { return (Nullable<System.Int32>)GetValue((int)PostalProfileFieldIndex.EntryFacility, false); }
			set	{ SetValue((int)PostalProfileFieldIndex.EntryFacility, value); }
		}

		/// <summary> The Memo1 property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."Memo1"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Memo1
		{
			get { return (System.String)GetValue((int)PostalProfileFieldIndex.Memo1, true); }
			set	{ SetValue((int)PostalProfileFieldIndex.Memo1, value); }
		}

		/// <summary> The Memo2 property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."Memo2"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Memo2
		{
			get { return (System.String)GetValue((int)PostalProfileFieldIndex.Memo2, true); }
			set	{ SetValue((int)PostalProfileFieldIndex.Memo2, value); }
		}

		/// <summary> The Memo3 property of the Entity PostalProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "PostalProfile"."Memo3"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String Memo3
		{
			get { return (System.String)GetValue((int)PostalProfileFieldIndex.Memo3, true); }
			set	{ SetValue((int)PostalProfileFieldIndex.Memo3, value); }
		}




		/// <summary> Gets / sets related entity of type 'EndiciaProfileEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual EndiciaProfileEntity Endicia
		{
			get
			{
				return _endicia;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncEndicia(value);
					if((SerializationHelper.Optimization == SerializationOptimization.Fast) && (value!=null))
					{
						value.SetRelatedEntity(this, "PostalProfile");
					}
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_endicia !=null);
						DesetupSyncEndicia(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("Endicia");
						}
					}
					else
					{
						if(_endicia!=value)
						{
							IEntity2 relatedEntity = (IEntity2)value;
							relatedEntity.SetRelatedEntity(this, "PostalProfile");
							SetupSyncEndicia(relatedEntity);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'ShippingProfileEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual ShippingProfileEntity Profile
		{
			get
			{
				return _profile;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncProfile(value);
					if((SerializationHelper.Optimization == SerializationOptimization.Fast) && (value!=null))
					{
						value.SetRelatedEntity(this, "Postal");
					}
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_profile !=null);
						DesetupSyncProfile(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("Profile");
						}
					}
					else
					{
						if(_profile!=value)
						{
							IEntity2 relatedEntity = (IEntity2)value;
							relatedEntity.SetRelatedEntity(this, "Postal");
							SetupSyncProfile(relatedEntity);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'UspsProfileEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual UspsProfileEntity Usps
		{
			get
			{
				return _usps;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncUsps(value);
					if((SerializationHelper.Optimization == SerializationOptimization.Fast) && (value!=null))
					{
						value.SetRelatedEntity(this, "PostalProfile");
					}
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_usps !=null);
						DesetupSyncUsps(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("Usps");
						}
					}
					else
					{
						if(_usps!=value)
						{
							IEntity2 relatedEntity = (IEntity2)value;
							relatedEntity.SetRelatedEntity(this, "PostalProfile");
							SetupSyncUsps(relatedEntity);
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
			get { return (int)ShipWorks.Data.Model.EntityType.PostalProfileEntity; }
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
