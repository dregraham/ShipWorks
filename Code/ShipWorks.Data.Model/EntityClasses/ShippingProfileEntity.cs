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
	/// Entity class which represents the entity 'ShippingProfile'.<br/><br/>
	/// 
	/// </summary>
	[Serializable]
	public partial class ShippingProfileEntity : CommonEntityBase, ISerializable
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations



		private AmazonProfileEntity _amazon;
		private BestRateProfileEntity _bestRate;
		private FedExProfileEntity _fedEx;
		private IParcelProfileEntity _iParcel;
		private OnTracProfileEntity _onTrac;
		private OtherProfileEntity _other;
		private PostalProfileEntity _postal;
		private UpsProfileEntity _ups;
		
		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{



			/// <summary>Member name Amazon</summary>
			public static readonly string Amazon = "Amazon";
			/// <summary>Member name BestRate</summary>
			public static readonly string BestRate = "BestRate";
			/// <summary>Member name FedEx</summary>
			public static readonly string FedEx = "FedEx";
			/// <summary>Member name IParcel</summary>
			public static readonly string IParcel = "IParcel";
			/// <summary>Member name OnTrac</summary>
			public static readonly string OnTrac = "OnTrac";
			/// <summary>Member name Other</summary>
			public static readonly string Other = "Other";
			/// <summary>Member name Postal</summary>
			public static readonly string Postal = "Postal";
			/// <summary>Member name Ups</summary>
			public static readonly string Ups = "Ups";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static ShippingProfileEntity()
		{
			SetupCustomPropertyHashtables();
		}

		/// <summary> CTor</summary>
		public ShippingProfileEntity():base("ShippingProfileEntity")
		{
			InitClassEmpty(null, CreateFields());
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public ShippingProfileEntity(IEntityFields2 fields):base("ShippingProfileEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this ShippingProfileEntity</param>
		public ShippingProfileEntity(IValidator validator):base("ShippingProfileEntity")
		{
			InitClassEmpty(validator, CreateFields());
		}
				

		/// <summary> CTor</summary>
		/// <param name="shippingProfileID">PK value for ShippingProfile which data should be fetched into this ShippingProfile object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ShippingProfileEntity(System.Int64 shippingProfileID):base("ShippingProfileEntity")
		{
			InitClassEmpty(null, CreateFields());
			this.ShippingProfileID = shippingProfileID;
		}

		/// <summary> CTor</summary>
		/// <param name="shippingProfileID">PK value for ShippingProfile which data should be fetched into this ShippingProfile object</param>
		/// <param name="validator">The custom validator object for this ShippingProfileEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ShippingProfileEntity(System.Int64 shippingProfileID, IValidator validator):base("ShippingProfileEntity")
		{
			InitClassEmpty(validator, CreateFields());
			this.ShippingProfileID = shippingProfileID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected ShippingProfileEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{



				_amazon = (AmazonProfileEntity)info.GetValue("_amazon", typeof(AmazonProfileEntity));
				if(_amazon!=null)
				{
					_amazon.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_bestRate = (BestRateProfileEntity)info.GetValue("_bestRate", typeof(BestRateProfileEntity));
				if(_bestRate!=null)
				{
					_bestRate.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_fedEx = (FedExProfileEntity)info.GetValue("_fedEx", typeof(FedExProfileEntity));
				if(_fedEx!=null)
				{
					_fedEx.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_iParcel = (IParcelProfileEntity)info.GetValue("_iParcel", typeof(IParcelProfileEntity));
				if(_iParcel!=null)
				{
					_iParcel.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_onTrac = (OnTracProfileEntity)info.GetValue("_onTrac", typeof(OnTracProfileEntity));
				if(_onTrac!=null)
				{
					_onTrac.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_other = (OtherProfileEntity)info.GetValue("_other", typeof(OtherProfileEntity));
				if(_other!=null)
				{
					_other.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_postal = (PostalProfileEntity)info.GetValue("_postal", typeof(PostalProfileEntity));
				if(_postal!=null)
				{
					_postal.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_ups = (UpsProfileEntity)info.GetValue("_ups", typeof(UpsProfileEntity));
				if(_ups!=null)
				{
					_ups.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((ShippingProfileFieldIndex)fieldIndex)
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



				case "Amazon":
					this.Amazon = (AmazonProfileEntity)entity;
					break;
				case "BestRate":
					this.BestRate = (BestRateProfileEntity)entity;
					break;
				case "FedEx":
					this.FedEx = (FedExProfileEntity)entity;
					break;
				case "IParcel":
					this.IParcel = (IParcelProfileEntity)entity;
					break;
				case "OnTrac":
					this.OnTrac = (OnTracProfileEntity)entity;
					break;
				case "Other":
					this.Other = (OtherProfileEntity)entity;
					break;
				case "Postal":
					this.Postal = (PostalProfileEntity)entity;
					break;
				case "Ups":
					this.Ups = (UpsProfileEntity)entity;
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
			return ShippingProfileEntity.GetRelationsForField(fieldName);
		}

		/// <summary>Gets the relation objects which represent the relation the fieldName specified is mapped on. </summary>
		/// <param name="fieldName">Name of the field mapped onto the relation of which the relation objects have to be obtained.</param>
		/// <returns>RelationCollection with relation object(s) which represent the relation the field is maped on</returns>
		public static RelationCollection GetRelationsForField(string fieldName)
		{
			RelationCollection toReturn = new RelationCollection();
			switch(fieldName)
			{



				case "Amazon":
					toReturn.Add(ShippingProfileEntity.Relations.AmazonProfileEntityUsingShippingProfileID);
					break;
				case "BestRate":
					toReturn.Add(ShippingProfileEntity.Relations.BestRateProfileEntityUsingShippingProfileID);
					break;
				case "FedEx":
					toReturn.Add(ShippingProfileEntity.Relations.FedExProfileEntityUsingShippingProfileID);
					break;
				case "IParcel":
					toReturn.Add(ShippingProfileEntity.Relations.IParcelProfileEntityUsingShippingProfileID);
					break;
				case "OnTrac":
					toReturn.Add(ShippingProfileEntity.Relations.OnTracProfileEntityUsingShippingProfileID);
					break;
				case "Other":
					toReturn.Add(ShippingProfileEntity.Relations.OtherProfileEntityUsingShippingProfileID);
					break;
				case "Postal":
					toReturn.Add(ShippingProfileEntity.Relations.PostalProfileEntityUsingShippingProfileID);
					break;
				case "Ups":
					toReturn.Add(ShippingProfileEntity.Relations.UpsProfileEntityUsingShippingProfileID);
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


				case "Amazon":
					SetupSyncAmazon(relatedEntity);
					break;
				case "BestRate":
					SetupSyncBestRate(relatedEntity);
					break;
				case "FedEx":
					SetupSyncFedEx(relatedEntity);
					break;
				case "IParcel":
					SetupSyncIParcel(relatedEntity);
					break;
				case "OnTrac":
					SetupSyncOnTrac(relatedEntity);
					break;
				case "Other":
					SetupSyncOther(relatedEntity);
					break;
				case "Postal":
					SetupSyncPostal(relatedEntity);
					break;
				case "Ups":
					SetupSyncUps(relatedEntity);
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


				case "Amazon":
					DesetupSyncAmazon(false, true);
					break;
				case "BestRate":
					DesetupSyncBestRate(false, true);
					break;
				case "FedEx":
					DesetupSyncFedEx(false, true);
					break;
				case "IParcel":
					DesetupSyncIParcel(false, true);
					break;
				case "OnTrac":
					DesetupSyncOnTrac(false, true);
					break;
				case "Other":
					DesetupSyncOther(false, true);
					break;
				case "Postal":
					DesetupSyncPostal(false, true);
					break;
				case "Ups":
					DesetupSyncUps(false, true);
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
			if(_amazon!=null)
			{
				toReturn.Add(_amazon);
			}

			if(_bestRate!=null)
			{
				toReturn.Add(_bestRate);
			}

			if(_fedEx!=null)
			{
				toReturn.Add(_fedEx);
			}

			if(_iParcel!=null)
			{
				toReturn.Add(_iParcel);
			}

			if(_onTrac!=null)
			{
				toReturn.Add(_onTrac);
			}

			if(_other!=null)
			{
				toReturn.Add(_other);
			}

			if(_postal!=null)
			{
				toReturn.Add(_postal);
			}

			if(_ups!=null)
			{
				toReturn.Add(_ups);
			}

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



				info.AddValue("_amazon", (!this.MarkedForDeletion?_amazon:null));
				info.AddValue("_bestRate", (!this.MarkedForDeletion?_bestRate:null));
				info.AddValue("_fedEx", (!this.MarkedForDeletion?_fedEx:null));
				info.AddValue("_iParcel", (!this.MarkedForDeletion?_iParcel:null));
				info.AddValue("_onTrac", (!this.MarkedForDeletion?_onTrac:null));
				info.AddValue("_other", (!this.MarkedForDeletion?_other:null));
				info.AddValue("_postal", (!this.MarkedForDeletion?_postal:null));
				info.AddValue("_ups", (!this.MarkedForDeletion?_ups:null));
			}
			
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}

		/// <summary>Returns true if the original value for the field with the fieldIndex passed in, read from the persistent storage was NULL, false otherwise.
		/// Should not be used for testing if the current value is NULL, use <see cref="TestCurrentFieldValueForNull"/> for that.</summary>
		/// <param name="fieldIndex">Index of the field to test if that field was NULL in the persistent storage</param>
		/// <returns>true if the field with the passed in index was NULL in the persistent storage, false otherwise</returns>
		public bool TestOriginalFieldValueForNull(ShippingProfileFieldIndex fieldIndex)
		{
			return base.Fields[(int)fieldIndex].IsNull;
		}
		
		/// <summary>Returns true if the current value for the field with the fieldIndex passed in represents null/not defined, false otherwise.
		/// Should not be used for testing if the original value (read from the db) is NULL</summary>
		/// <param name="fieldIndex">Index of the field to test if its currentvalue is null/undefined</param>
		/// <returns>true if the field's value isn't defined yet, false otherwise</returns>
		public bool TestCurrentFieldValueForNull(ShippingProfileFieldIndex fieldIndex)
		{
			return base.CheckIfCurrentFieldValueIsNull((int)fieldIndex);
		}

				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		public override List<IEntityRelation> GetAllRelations()
		{
			return new ShippingProfileRelations().GetAllRelations();
		}
		




		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'AmazonProfile' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoAmazon()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(AmazonProfileFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'BestRateProfile' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoBestRate()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(BestRateProfileFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'FedExProfile' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoFedEx()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(FedExProfileFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'IParcelProfile' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoIParcel()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(IParcelProfileFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'OnTracProfile' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoOnTrac()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(OnTracProfileFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'OtherProfile' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoOther()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(OtherProfileFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'PostalProfile' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoPostal()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(PostalProfileFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch
		/// the related entity of type 'UpsProfile' to this entity. Use DataAccessAdapter.FetchNewEntity() to fetch this related entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoUps()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(UpsProfileFields.ShippingProfileID, null, ComparisonOperator.Equal, this.ShippingProfileID));
			return bucket;
		}
	
		
		/// <summary>Creates entity fields object for this entity. Used in constructor to setup this entity in a polymorphic scenario.</summary>
		protected virtual IEntityFields2 CreateFields()
		{
			return EntityFieldsFactory.CreateEntityFieldsObject(ShipWorks.Data.Model.EntityType.ShippingProfileEntity);
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
			return EntityFactoryCache2.GetEntityFactory(typeof(ShippingProfileEntityFactory));
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



			toReturn.Add("Amazon", _amazon);
			toReturn.Add("BestRate", _bestRate);
			toReturn.Add("FedEx", _fedEx);
			toReturn.Add("IParcel", _iParcel);
			toReturn.Add("OnTrac", _onTrac);
			toReturn.Add("Other", _other);
			toReturn.Add("Postal", _postal);
			toReturn.Add("Ups", _ups);
			return toReturn;
		}
		
		/// <summary> Adds the internals to the active context. </summary>
		protected override void AddInternalsToContext()
		{



			if(_amazon!=null)
			{
				_amazon.ActiveContext = base.ActiveContext;
			}
			if(_bestRate!=null)
			{
				_bestRate.ActiveContext = base.ActiveContext;
			}
			if(_fedEx!=null)
			{
				_fedEx.ActiveContext = base.ActiveContext;
			}
			if(_iParcel!=null)
			{
				_iParcel.ActiveContext = base.ActiveContext;
			}
			if(_onTrac!=null)
			{
				_onTrac.ActiveContext = base.ActiveContext;
			}
			if(_other!=null)
			{
				_other.ActiveContext = base.ActiveContext;
			}
			if(_postal!=null)
			{
				_postal.ActiveContext = base.ActiveContext;
			}
			if(_ups!=null)
			{
				_ups.ActiveContext = base.ActiveContext;
			}
		}

		/// <summary> Initializes the class members</summary>
		protected virtual void InitClassMembers()
		{




			_amazon = null;
			_bestRate = null;
			_fedEx = null;
			_iParcel = null;
			_onTrac = null;
			_other = null;
			_postal = null;
			_ups = null;
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

			_fieldsCustomProperties.Add("RowVersion", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Name", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ShipmentType", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ShipmentTypePrimary", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("OriginID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("Insurance", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("InsuranceInitialValueSource", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("InsuranceInitialValueAmount", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("ReturnShipment", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();

			_fieldsCustomProperties.Add("RequestedLabelFormat", fieldHashtable);
		}
		#endregion


		/// <summary> Removes the sync logic for member _amazon</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncAmazon(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _amazon, new PropertyChangedEventHandler( OnAmazonPropertyChanged ), "Amazon", ShippingProfileEntity.Relations.AmazonProfileEntityUsingShippingProfileID, false, signalRelatedEntity, "ShippingProfile", false, new int[] { (int)ShippingProfileFieldIndex.ShippingProfileID } );
			_amazon = null;
		}
		
		/// <summary> setups the sync logic for member _amazon</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncAmazon(IEntity2 relatedEntity)
		{
			if(_amazon!=relatedEntity)
			{
				DesetupSyncAmazon(true, true);
				_amazon = (AmazonProfileEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _amazon, new PropertyChangedEventHandler( OnAmazonPropertyChanged ), "Amazon", ShippingProfileEntity.Relations.AmazonProfileEntityUsingShippingProfileID, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnAmazonPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _bestRate</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncBestRate(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _bestRate, new PropertyChangedEventHandler( OnBestRatePropertyChanged ), "BestRate", ShippingProfileEntity.Relations.BestRateProfileEntityUsingShippingProfileID, false, signalRelatedEntity, "ShippingProfile", false, new int[] { (int)ShippingProfileFieldIndex.ShippingProfileID } );
			_bestRate = null;
		}
		
		/// <summary> setups the sync logic for member _bestRate</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncBestRate(IEntity2 relatedEntity)
		{
			if(_bestRate!=relatedEntity)
			{
				DesetupSyncBestRate(true, true);
				_bestRate = (BestRateProfileEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _bestRate, new PropertyChangedEventHandler( OnBestRatePropertyChanged ), "BestRate", ShippingProfileEntity.Relations.BestRateProfileEntityUsingShippingProfileID, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnBestRatePropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _fedEx</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncFedEx(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _fedEx, new PropertyChangedEventHandler( OnFedExPropertyChanged ), "FedEx", ShippingProfileEntity.Relations.FedExProfileEntityUsingShippingProfileID, false, signalRelatedEntity, "ShippingProfile", false, new int[] { (int)ShippingProfileFieldIndex.ShippingProfileID } );
			_fedEx = null;
		}
		
		/// <summary> setups the sync logic for member _fedEx</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncFedEx(IEntity2 relatedEntity)
		{
			if(_fedEx!=relatedEntity)
			{
				DesetupSyncFedEx(true, true);
				_fedEx = (FedExProfileEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _fedEx, new PropertyChangedEventHandler( OnFedExPropertyChanged ), "FedEx", ShippingProfileEntity.Relations.FedExProfileEntityUsingShippingProfileID, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnFedExPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _iParcel</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncIParcel(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _iParcel, new PropertyChangedEventHandler( OnIParcelPropertyChanged ), "IParcel", ShippingProfileEntity.Relations.IParcelProfileEntityUsingShippingProfileID, false, signalRelatedEntity, "ShippingProfile", false, new int[] { (int)ShippingProfileFieldIndex.ShippingProfileID } );
			_iParcel = null;
		}
		
		/// <summary> setups the sync logic for member _iParcel</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncIParcel(IEntity2 relatedEntity)
		{
			if(_iParcel!=relatedEntity)
			{
				DesetupSyncIParcel(true, true);
				_iParcel = (IParcelProfileEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _iParcel, new PropertyChangedEventHandler( OnIParcelPropertyChanged ), "IParcel", ShippingProfileEntity.Relations.IParcelProfileEntityUsingShippingProfileID, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnIParcelPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _onTrac</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncOnTrac(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _onTrac, new PropertyChangedEventHandler( OnOnTracPropertyChanged ), "OnTrac", ShippingProfileEntity.Relations.OnTracProfileEntityUsingShippingProfileID, false, signalRelatedEntity, "ShippingProfile", false, new int[] { (int)ShippingProfileFieldIndex.ShippingProfileID } );
			_onTrac = null;
		}
		
		/// <summary> setups the sync logic for member _onTrac</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncOnTrac(IEntity2 relatedEntity)
		{
			if(_onTrac!=relatedEntity)
			{
				DesetupSyncOnTrac(true, true);
				_onTrac = (OnTracProfileEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _onTrac, new PropertyChangedEventHandler( OnOnTracPropertyChanged ), "OnTrac", ShippingProfileEntity.Relations.OnTracProfileEntityUsingShippingProfileID, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnOnTracPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _other</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncOther(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _other, new PropertyChangedEventHandler( OnOtherPropertyChanged ), "Other", ShippingProfileEntity.Relations.OtherProfileEntityUsingShippingProfileID, false, signalRelatedEntity, "ShippingProfile", false, new int[] { (int)ShippingProfileFieldIndex.ShippingProfileID } );
			_other = null;
		}
		
		/// <summary> setups the sync logic for member _other</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncOther(IEntity2 relatedEntity)
		{
			if(_other!=relatedEntity)
			{
				DesetupSyncOther(true, true);
				_other = (OtherProfileEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _other, new PropertyChangedEventHandler( OnOtherPropertyChanged ), "Other", ShippingProfileEntity.Relations.OtherProfileEntityUsingShippingProfileID, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnOtherPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _postal</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncPostal(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _postal, new PropertyChangedEventHandler( OnPostalPropertyChanged ), "Postal", ShippingProfileEntity.Relations.PostalProfileEntityUsingShippingProfileID, false, signalRelatedEntity, "Profile", false, new int[] { (int)ShippingProfileFieldIndex.ShippingProfileID } );
			_postal = null;
		}
		
		/// <summary> setups the sync logic for member _postal</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncPostal(IEntity2 relatedEntity)
		{
			if(_postal!=relatedEntity)
			{
				DesetupSyncPostal(true, true);
				_postal = (PostalProfileEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _postal, new PropertyChangedEventHandler( OnPostalPropertyChanged ), "Postal", ShippingProfileEntity.Relations.PostalProfileEntityUsingShippingProfileID, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnPostalPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _ups</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncUps(bool signalRelatedEntity, bool resetFKFields)
		{
			base.PerformDesetupSyncRelatedEntity( _ups, new PropertyChangedEventHandler( OnUpsPropertyChanged ), "Ups", ShippingProfileEntity.Relations.UpsProfileEntityUsingShippingProfileID, false, signalRelatedEntity, "ShippingProfile", false, new int[] { (int)ShippingProfileFieldIndex.ShippingProfileID } );
			_ups = null;
		}
		
		/// <summary> setups the sync logic for member _ups</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncUps(IEntity2 relatedEntity)
		{
			if(_ups!=relatedEntity)
			{
				DesetupSyncUps(true, true);
				_ups = (UpsProfileEntity)relatedEntity;
				base.PerformSetupSyncRelatedEntity( _ups, new PropertyChangedEventHandler( OnUpsPropertyChanged ), "Ups", ShippingProfileEntity.Relations.UpsProfileEntityUsingShippingProfileID, false, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnUpsPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this ShippingProfileEntity</param>
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
		public  static ShippingProfileRelations Relations
		{
			get	{ return new ShippingProfileRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}




		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'AmazonProfile' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathAmazon
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(AmazonProfileEntityFactory))),
					(IEntityRelation)GetRelationsForField("Amazon")[0], (int)ShipWorks.Data.Model.EntityType.ShippingProfileEntity, (int)ShipWorks.Data.Model.EntityType.AmazonProfileEntity, 0, null, null, null, null, "Amazon", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);
			}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'BestRateProfile' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathBestRate
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(BestRateProfileEntityFactory))),
					(IEntityRelation)GetRelationsForField("BestRate")[0], (int)ShipWorks.Data.Model.EntityType.ShippingProfileEntity, (int)ShipWorks.Data.Model.EntityType.BestRateProfileEntity, 0, null, null, null, null, "BestRate", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);
			}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'FedExProfile' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathFedEx
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(FedExProfileEntityFactory))),
					(IEntityRelation)GetRelationsForField("FedEx")[0], (int)ShipWorks.Data.Model.EntityType.ShippingProfileEntity, (int)ShipWorks.Data.Model.EntityType.FedExProfileEntity, 0, null, null, null, null, "FedEx", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);
			}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'IParcelProfile' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathIParcel
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(IParcelProfileEntityFactory))),
					(IEntityRelation)GetRelationsForField("IParcel")[0], (int)ShipWorks.Data.Model.EntityType.ShippingProfileEntity, (int)ShipWorks.Data.Model.EntityType.IParcelProfileEntity, 0, null, null, null, null, "IParcel", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);
			}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'OnTracProfile' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathOnTrac
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(OnTracProfileEntityFactory))),
					(IEntityRelation)GetRelationsForField("OnTrac")[0], (int)ShipWorks.Data.Model.EntityType.ShippingProfileEntity, (int)ShipWorks.Data.Model.EntityType.OnTracProfileEntity, 0, null, null, null, null, "OnTrac", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);
			}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'OtherProfile' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathOther
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(OtherProfileEntityFactory))),
					(IEntityRelation)GetRelationsForField("Other")[0], (int)ShipWorks.Data.Model.EntityType.ShippingProfileEntity, (int)ShipWorks.Data.Model.EntityType.OtherProfileEntity, 0, null, null, null, null, "Other", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);
			}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'PostalProfile' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathPostal
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(PostalProfileEntityFactory))),
					(IEntityRelation)GetRelationsForField("Postal")[0], (int)ShipWorks.Data.Model.EntityType.ShippingProfileEntity, (int)ShipWorks.Data.Model.EntityType.PostalProfileEntity, 0, null, null, null, null, "Postal", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);
			}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'UpsProfile' 
		/// for this entity. Add the object returned by this property to an existing PrefetchPath2 instance.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathUps
		{
			get
			{
				return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(UpsProfileEntityFactory))),
					(IEntityRelation)GetRelationsForField("Ups")[0], (int)ShipWorks.Data.Model.EntityType.ShippingProfileEntity, (int)ShipWorks.Data.Model.EntityType.UpsProfileEntity, 0, null, null, null, null, "Ups", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToOne);
			}
		}

		/// <summary> The custom properties for the type of this entity instance.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[Browsable(false), XmlIgnore]
		public override Dictionary<string, string> CustomPropertiesOfType
		{
			get { return ShippingProfileEntity.CustomProperties;}
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
			get { return ShippingProfileEntity.FieldsCustomProperties;}
		}

		/// <summary> The ShippingProfileID property of the Entity ShippingProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingProfile"."ShippingProfileID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 ShippingProfileID
		{
			get { return (System.Int64)GetValue((int)ShippingProfileFieldIndex.ShippingProfileID, true); }
			set	{ SetValue((int)ShippingProfileFieldIndex.ShippingProfileID, value); }
		}

		/// <summary> The RowVersion property of the Entity ShippingProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingProfile"."RowVersion"<br/>
		/// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Byte[] RowVersion
		{
			get { return (System.Byte[])GetValue((int)ShippingProfileFieldIndex.RowVersion, true); }

		}

		/// <summary> The Name property of the Entity ShippingProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingProfile"."Name"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Name
		{
			get { return (System.String)GetValue((int)ShippingProfileFieldIndex.Name, true); }
			set	{ SetValue((int)ShippingProfileFieldIndex.Name, value); }
		}

		/// <summary> The ShipmentType property of the Entity ShippingProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingProfile"."ShipmentType"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int32 ShipmentType
		{
			get { return (System.Int32)GetValue((int)ShippingProfileFieldIndex.ShipmentType, true); }
			set	{ SetValue((int)ShippingProfileFieldIndex.ShipmentType, value); }
		}

		/// <summary> The ShipmentTypePrimary property of the Entity ShippingProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingProfile"."ShipmentTypePrimary"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean ShipmentTypePrimary
		{
			get { return (System.Boolean)GetValue((int)ShippingProfileFieldIndex.ShipmentTypePrimary, true); }
			set	{ SetValue((int)ShippingProfileFieldIndex.ShipmentTypePrimary, value); }
		}

		/// <summary> The OriginID property of the Entity ShippingProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingProfile"."OriginID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int64> OriginID
		{
			get { return (Nullable<System.Int64>)GetValue((int)ShippingProfileFieldIndex.OriginID, false); }
			set	{ SetValue((int)ShippingProfileFieldIndex.OriginID, value); }
		}

		/// <summary> The Insurance property of the Entity ShippingProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingProfile"."Insurance"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> Insurance
		{
			get { return (Nullable<System.Boolean>)GetValue((int)ShippingProfileFieldIndex.Insurance, false); }
			set	{ SetValue((int)ShippingProfileFieldIndex.Insurance, value); }
		}

		/// <summary> The InsuranceInitialValueSource property of the Entity ShippingProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingProfile"."InsuranceInitialValueSource"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> InsuranceInitialValueSource
		{
			get { return (Nullable<System.Int32>)GetValue((int)ShippingProfileFieldIndex.InsuranceInitialValueSource, false); }
			set	{ SetValue((int)ShippingProfileFieldIndex.InsuranceInitialValueSource, value); }
		}

		/// <summary> The InsuranceInitialValueAmount property of the Entity ShippingProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingProfile"."InsuranceInitialValueAmount"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Decimal> InsuranceInitialValueAmount
		{
			get { return (Nullable<System.Decimal>)GetValue((int)ShippingProfileFieldIndex.InsuranceInitialValueAmount, false); }
			set	{ SetValue((int)ShippingProfileFieldIndex.InsuranceInitialValueAmount, value); }
		}

		/// <summary> The ReturnShipment property of the Entity ShippingProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingProfile"."ReturnShipment"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Boolean> ReturnShipment
		{
			get { return (Nullable<System.Boolean>)GetValue((int)ShippingProfileFieldIndex.ReturnShipment, false); }
			set	{ SetValue((int)ShippingProfileFieldIndex.ReturnShipment, value); }
		}

		/// <summary> The RequestedLabelFormat property of the Entity ShippingProfile<br/><br/>
		/// </summary>
		/// <remarks>Mapped on  table field: "ShippingProfile"."RequestedLabelFormat"<br/>
		/// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Int32> RequestedLabelFormat
		{
			get { return (Nullable<System.Int32>)GetValue((int)ShippingProfileFieldIndex.RequestedLabelFormat, false); }
			set	{ SetValue((int)ShippingProfileFieldIndex.RequestedLabelFormat, value); }
		}




		/// <summary> Gets / sets related entity of type 'AmazonProfileEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual AmazonProfileEntity Amazon
		{
			get
			{
				return _amazon;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncAmazon(value);
					if((SerializationHelper.Optimization == SerializationOptimization.Fast) && (value!=null))
					{
						value.SetRelatedEntity(this, "ShippingProfile");
					}
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_amazon !=null);
						DesetupSyncAmazon(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("Amazon");
						}
					}
					else
					{
						if(_amazon!=value)
						{
							IEntity2 relatedEntity = (IEntity2)value;
							relatedEntity.SetRelatedEntity(this, "ShippingProfile");
							SetupSyncAmazon(relatedEntity);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'BestRateProfileEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual BestRateProfileEntity BestRate
		{
			get
			{
				return _bestRate;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncBestRate(value);
					if((SerializationHelper.Optimization == SerializationOptimization.Fast) && (value!=null))
					{
						value.SetRelatedEntity(this, "ShippingProfile");
					}
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_bestRate !=null);
						DesetupSyncBestRate(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("BestRate");
						}
					}
					else
					{
						if(_bestRate!=value)
						{
							IEntity2 relatedEntity = (IEntity2)value;
							relatedEntity.SetRelatedEntity(this, "ShippingProfile");
							SetupSyncBestRate(relatedEntity);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'FedExProfileEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual FedExProfileEntity FedEx
		{
			get
			{
				return _fedEx;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncFedEx(value);
					if((SerializationHelper.Optimization == SerializationOptimization.Fast) && (value!=null))
					{
						value.SetRelatedEntity(this, "ShippingProfile");
					}
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_fedEx !=null);
						DesetupSyncFedEx(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("FedEx");
						}
					}
					else
					{
						if(_fedEx!=value)
						{
							IEntity2 relatedEntity = (IEntity2)value;
							relatedEntity.SetRelatedEntity(this, "ShippingProfile");
							SetupSyncFedEx(relatedEntity);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'IParcelProfileEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual IParcelProfileEntity IParcel
		{
			get
			{
				return _iParcel;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncIParcel(value);
					if((SerializationHelper.Optimization == SerializationOptimization.Fast) && (value!=null))
					{
						value.SetRelatedEntity(this, "ShippingProfile");
					}
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_iParcel !=null);
						DesetupSyncIParcel(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("IParcel");
						}
					}
					else
					{
						if(_iParcel!=value)
						{
							IEntity2 relatedEntity = (IEntity2)value;
							relatedEntity.SetRelatedEntity(this, "ShippingProfile");
							SetupSyncIParcel(relatedEntity);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'OnTracProfileEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual OnTracProfileEntity OnTrac
		{
			get
			{
				return _onTrac;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncOnTrac(value);
					if((SerializationHelper.Optimization == SerializationOptimization.Fast) && (value!=null))
					{
						value.SetRelatedEntity(this, "ShippingProfile");
					}
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_onTrac !=null);
						DesetupSyncOnTrac(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("OnTrac");
						}
					}
					else
					{
						if(_onTrac!=value)
						{
							IEntity2 relatedEntity = (IEntity2)value;
							relatedEntity.SetRelatedEntity(this, "ShippingProfile");
							SetupSyncOnTrac(relatedEntity);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'OtherProfileEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual OtherProfileEntity Other
		{
			get
			{
				return _other;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncOther(value);
					if((SerializationHelper.Optimization == SerializationOptimization.Fast) && (value!=null))
					{
						value.SetRelatedEntity(this, "ShippingProfile");
					}
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_other !=null);
						DesetupSyncOther(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("Other");
						}
					}
					else
					{
						if(_other!=value)
						{
							IEntity2 relatedEntity = (IEntity2)value;
							relatedEntity.SetRelatedEntity(this, "ShippingProfile");
							SetupSyncOther(relatedEntity);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'PostalProfileEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual PostalProfileEntity Postal
		{
			get
			{
				return _postal;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncPostal(value);
					if((SerializationHelper.Optimization == SerializationOptimization.Fast) && (value!=null))
					{
						value.SetRelatedEntity(this, "Profile");
					}
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_postal !=null);
						DesetupSyncPostal(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("Postal");
						}
					}
					else
					{
						if(_postal!=value)
						{
							IEntity2 relatedEntity = (IEntity2)value;
							relatedEntity.SetRelatedEntity(this, "Profile");
							SetupSyncPostal(relatedEntity);
						}
					}
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'UpsProfileEntity' which has to be set using a fetch action earlier. If no related entity
		/// is set for this property, null is returned. This property is not visible in databound grids.</summary>
		[Browsable(false)]
		public virtual UpsProfileEntity Ups
		{
			get
			{
				return _ups;
			}
			set
			{
				if(base.IsDeserializing)
				{
					SetupSyncUps(value);
					if((SerializationHelper.Optimization == SerializationOptimization.Fast) && (value!=null))
					{
						value.SetRelatedEntity(this, "ShippingProfile");
					}
				}
				else
				{
					if(value==null)
					{
						bool raisePropertyChanged = (_ups !=null);
						DesetupSyncUps(true, true);
						if(raisePropertyChanged)
						{
							OnPropertyChanged("Ups");
						}
					}
					else
					{
						if(_ups!=value)
						{
							IEntity2 relatedEntity = (IEntity2)value;
							relatedEntity.SetRelatedEntity(this, "ShippingProfile");
							SetupSyncUps(relatedEntity);
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
			get { return (int)ShipWorks.Data.Model.EntityType.ShippingProfileEntity; }
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
