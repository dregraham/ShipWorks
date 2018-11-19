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
	/// <summary>Entity class which represents the entity 'ProductVariant'.<br/><br/></summary>
	[Serializable]
	public partial class ProductVariantEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private EntityCollection<ProductBundleEntity> _productBundle;
		private EntityCollection<ProductVariantAliasEntity> _productVariantAlias;
		private EntityCollection<ProductVariantTypeAndValueEntity> _productVariantTypeAndValue;
		private ProductEntity _product;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name Product</summary>
			public static readonly string Product = "Product";
			/// <summary>Member name ProductBundle</summary>
			public static readonly string ProductBundle = "ProductBundle";
			/// <summary>Member name ProductVariantAlias</summary>
			public static readonly string ProductVariantAlias = "ProductVariantAlias";
			/// <summary>Member name ProductVariantTypeAndValue</summary>
			public static readonly string ProductVariantTypeAndValue = "ProductVariantTypeAndValue";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static ProductVariantEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public ProductVariantEntity():base("ProductVariantEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public ProductVariantEntity(IEntityFields2 fields):base("ProductVariantEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this ProductVariantEntity</param>
		public ProductVariantEntity(IValidator validator):base("ProductVariantEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="productVariantID">PK value for ProductVariant which data should be fetched into this ProductVariant object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ProductVariantEntity(System.Int64 productVariantID):base("ProductVariantEntity")
		{
			InitClassEmpty(null, null);
			this.ProductVariantID = productVariantID;
		}

		/// <summary> CTor</summary>
		/// <param name="productVariantID">PK value for ProductVariant which data should be fetched into this ProductVariant object</param>
		/// <param name="validator">The custom validator object for this ProductVariantEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ProductVariantEntity(System.Int64 productVariantID, IValidator validator):base("ProductVariantEntity")
		{
			InitClassEmpty(validator, null);
			this.ProductVariantID = productVariantID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected ProductVariantEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_productBundle = (EntityCollection<ProductBundleEntity>)info.GetValue("_productBundle", typeof(EntityCollection<ProductBundleEntity>));
				_productVariantAlias = (EntityCollection<ProductVariantAliasEntity>)info.GetValue("_productVariantAlias", typeof(EntityCollection<ProductVariantAliasEntity>));
				_productVariantTypeAndValue = (EntityCollection<ProductVariantTypeAndValueEntity>)info.GetValue("_productVariantTypeAndValue", typeof(EntityCollection<ProductVariantTypeAndValueEntity>));
				_product = (ProductEntity)info.GetValue("_product", typeof(ProductEntity));
				if(_product!=null)
				{
					_product.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((ProductVariantFieldIndex)fieldIndex)
			{
				case ProductVariantFieldIndex.ProductID:
					DesetupSyncProduct(true, false);
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
				case "Product":
					this.Product = (ProductEntity)entity;
					break;
				case "ProductBundle":
					this.ProductBundle.Add((ProductBundleEntity)entity);
					break;
				case "ProductVariantAlias":
					this.ProductVariantAlias.Add((ProductVariantAliasEntity)entity);
					break;
				case "ProductVariantTypeAndValue":
					this.ProductVariantTypeAndValue.Add((ProductVariantTypeAndValueEntity)entity);
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
				case "Product":
					toReturn.Add(Relations.ProductEntityUsingProductID);
					break;
				case "ProductBundle":
					toReturn.Add(Relations.ProductBundleEntityUsingChildProductVariantID);
					break;
				case "ProductVariantAlias":
					toReturn.Add(Relations.ProductVariantAliasEntityUsingProductVariantID);
					break;
				case "ProductVariantTypeAndValue":
					toReturn.Add(Relations.ProductVariantTypeAndValueEntityUsingProductVariantID);
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
				case "Product":
					SetupSyncProduct(relatedEntity);
					break;
				case "ProductBundle":
					this.ProductBundle.Add((ProductBundleEntity)relatedEntity);
					break;
				case "ProductVariantAlias":
					this.ProductVariantAlias.Add((ProductVariantAliasEntity)relatedEntity);
					break;
				case "ProductVariantTypeAndValue":
					this.ProductVariantTypeAndValue.Add((ProductVariantTypeAndValueEntity)relatedEntity);
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
				case "Product":
					DesetupSyncProduct(false, true);
					break;
				case "ProductBundle":
					this.PerformRelatedEntityRemoval(this.ProductBundle, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "ProductVariantAlias":
					this.PerformRelatedEntityRemoval(this.ProductVariantAlias, relatedEntity, signalRelatedEntityManyToOne);
					break;
				case "ProductVariantTypeAndValue":
					this.PerformRelatedEntityRemoval(this.ProductVariantTypeAndValue, relatedEntity, signalRelatedEntityManyToOne);
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
			if(_product!=null)
			{
				toReturn.Add(_product);
			}
			return toReturn;
		}
		
		/// <summary>Gets a list of all entity collections stored as member variables in this entity. Only 1:n related collections are returned.</summary>
		/// <returns>Collection with 0 or more IEntityCollection2 objects, referenced by this entity</returns>
		protected override List<IEntityCollection2> GetMemberEntityCollections()
		{
			List<IEntityCollection2> toReturn = new List<IEntityCollection2>();
			toReturn.Add(this.ProductBundle);
			toReturn.Add(this.ProductVariantAlias);
			toReturn.Add(this.ProductVariantTypeAndValue);
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
				info.AddValue("_productBundle", ((_productBundle!=null) && (_productBundle.Count>0) && !this.MarkedForDeletion)?_productBundle:null);
				info.AddValue("_productVariantAlias", ((_productVariantAlias!=null) && (_productVariantAlias.Count>0) && !this.MarkedForDeletion)?_productVariantAlias:null);
				info.AddValue("_productVariantTypeAndValue", ((_productVariantTypeAndValue!=null) && (_productVariantTypeAndValue.Count>0) && !this.MarkedForDeletion)?_productVariantTypeAndValue:null);
				info.AddValue("_product", (!this.MarkedForDeletion?_product:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new ProductVariantRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'ProductBundle' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoProductBundle()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ProductBundleFields.ChildProductVariantID, null, ComparisonOperator.Equal, this.ProductVariantID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'ProductVariantAlias' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoProductVariantAlias()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ProductVariantAliasFields.ProductVariantID, null, ComparisonOperator.Equal, this.ProductVariantID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entities of type 'ProductVariantTypeAndValue' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoProductVariantTypeAndValue()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ProductVariantTypeAndValueFields.ProductVariantID, null, ComparisonOperator.Equal, this.ProductVariantID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'Product' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoProduct()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ProductFields.ProductID, null, ComparisonOperator.Equal, this.ProductID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(ProductVariantEntityFactory));
		}
#if !CF
		/// <summary>Adds the member collections to the collections queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void AddToMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue) 
		{
			base.AddToMemberEntityCollectionsQueue(collectionsQueue);
			collectionsQueue.Enqueue(this._productBundle);
			collectionsQueue.Enqueue(this._productVariantAlias);
			collectionsQueue.Enqueue(this._productVariantTypeAndValue);
		}
		
		/// <summary>Gets the member collections queue from the queue (base first)</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		protected override void GetFromMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue)
		{
			base.GetFromMemberEntityCollectionsQueue(collectionsQueue);
			this._productBundle = (EntityCollection<ProductBundleEntity>) collectionsQueue.Dequeue();
			this._productVariantAlias = (EntityCollection<ProductVariantAliasEntity>) collectionsQueue.Dequeue();
			this._productVariantTypeAndValue = (EntityCollection<ProductVariantTypeAndValueEntity>) collectionsQueue.Dequeue();

		}
		
		/// <summary>Determines whether the entity has populated member collections</summary>
		/// <returns>true if the entity has populated member collections.</returns>
		protected override bool HasPopulatedMemberEntityCollections()
		{
			bool toReturn = false;
			toReturn |=(this._productBundle != null);
			toReturn |=(this._productVariantAlias != null);
			toReturn |=(this._productVariantTypeAndValue != null);
			return toReturn ? true : base.HasPopulatedMemberEntityCollections();
		}
		
		/// <summary>Creates the member entity collections queue.</summary>
		/// <param name="collectionsQueue">The collections queue.</param>
		/// <param name="requiredQueue">The required queue.</param>
		protected override void CreateMemberEntityCollectionsQueue(Queue<IEntityCollection2> collectionsQueue, Queue<bool> requiredQueue) 
		{
			base.CreateMemberEntityCollectionsQueue(collectionsQueue, requiredQueue);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ProductBundleEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ProductBundleEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ProductVariantAliasEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ProductVariantAliasEntityFactory))) : null);
			collectionsQueue.Enqueue(requiredQueue.Dequeue() ? new EntityCollection<ProductVariantTypeAndValueEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ProductVariantTypeAndValueEntityFactory))) : null);
		}
#endif
		/// <summary>Gets all related data objects, stored by name. The name is the field name mapped onto the relation for that particular data element.</summary>
		/// <returns>Dictionary with per name the related referenced data element, which can be an entity collection or an entity or null</returns>
		protected override Dictionary<string, object> GetRelatedData()
		{
			Dictionary<string, object> toReturn = new Dictionary<string, object>();
			toReturn.Add("Product", _product);
			toReturn.Add("ProductBundle", _productBundle);
			toReturn.Add("ProductVariantAlias", _productVariantAlias);
			toReturn.Add("ProductVariantTypeAndValue", _productVariantTypeAndValue);
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
			_fieldsCustomProperties.Add("ProductVariantID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ProductID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CreatedDate", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Name", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("IsActive", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("UPC", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ASIN", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ISBN", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Weight", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Length", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Width", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("Height", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ImageUrl", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("BinLocation", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("HarmonizedCode", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("DeclaredValue", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("CountryOfOrigin", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _product</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncProduct(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _product, new PropertyChangedEventHandler( OnProductPropertyChanged ), "Product", ShipWorks.Data.Model.RelationClasses.StaticProductVariantRelations.ProductEntityUsingProductIDStatic, true, signalRelatedEntity, "ProductVariant", resetFKFields, new int[] { (int)ProductVariantFieldIndex.ProductID } );
			_product = null;
		}

		/// <summary> setups the sync logic for member _product</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncProduct(IEntityCore relatedEntity)
		{
			if(_product!=relatedEntity)
			{
				DesetupSyncProduct(true, true);
				_product = (ProductEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _product, new PropertyChangedEventHandler( OnProductPropertyChanged ), "Product", ShipWorks.Data.Model.RelationClasses.StaticProductVariantRelations.ProductEntityUsingProductIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnProductPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this ProductVariantEntity</param>
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
		public  static ProductVariantRelations Relations
		{
			get	{ return new ProductVariantRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ProductBundle' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathProductBundle
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<ProductBundleEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ProductBundleEntityFactory))), (IEntityRelation)GetRelationsForField("ProductBundle")[0], (int)ShipWorks.Data.Model.EntityType.ProductVariantEntity, (int)ShipWorks.Data.Model.EntityType.ProductBundleEntity, 0, null, null, null, null, "ProductBundle", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ProductVariantAlias' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathProductVariantAlias
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<ProductVariantAliasEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ProductVariantAliasEntityFactory))), (IEntityRelation)GetRelationsForField("ProductVariantAlias")[0], (int)ShipWorks.Data.Model.EntityType.ProductVariantEntity, (int)ShipWorks.Data.Model.EntityType.ProductVariantAliasEntity, 0, null, null, null, null, "ProductVariantAlias", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ProductVariantTypeAndValue' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathProductVariantTypeAndValue
		{
			get	{ return new PrefetchPathElement2( new EntityCollection<ProductVariantTypeAndValueEntity>(EntityFactoryCache2.GetEntityFactory(typeof(ProductVariantTypeAndValueEntityFactory))), (IEntityRelation)GetRelationsForField("ProductVariantTypeAndValue")[0], (int)ShipWorks.Data.Model.EntityType.ProductVariantEntity, (int)ShipWorks.Data.Model.EntityType.ProductVariantTypeAndValueEntity, 0, null, null, null, null, "ProductVariantTypeAndValue", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.OneToMany);	}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'Product' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathProduct
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(ProductEntityFactory))),	(IEntityRelation)GetRelationsForField("Product")[0], (int)ShipWorks.Data.Model.EntityType.ProductVariantEntity, (int)ShipWorks.Data.Model.EntityType.ProductEntity, 0, null, null, null, null, "Product", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The ProductVariantID property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."ProductVariantID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 ProductVariantID
		{
			get { return (System.Int64)GetValue((int)ProductVariantFieldIndex.ProductVariantID, true); }
			set	{ SetValue((int)ProductVariantFieldIndex.ProductVariantID, value); }
		}

		/// <summary> The ProductID property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."ProductID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 ProductID
		{
			get { return (System.Int64)GetValue((int)ProductVariantFieldIndex.ProductID, true); }
			set	{ SetValue((int)ProductVariantFieldIndex.ProductID, value); }
		}

		/// <summary> The CreatedDate property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."CreatedDate"<br/>
		/// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.DateTime CreatedDate
		{
			get { return (System.DateTime)GetValue((int)ProductVariantFieldIndex.CreatedDate, true); }
			set	{ SetValue((int)ProductVariantFieldIndex.CreatedDate, value); }
		}

		/// <summary> The Name property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."Name"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String Name
		{
			get { return (System.String)GetValue((int)ProductVariantFieldIndex.Name, true); }
			set	{ SetValue((int)ProductVariantFieldIndex.Name, value); }
		}

		/// <summary> The IsActive property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."IsActive"<br/>
		/// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Boolean IsActive
		{
			get { return (System.Boolean)GetValue((int)ProductVariantFieldIndex.IsActive, true); }
			set	{ SetValue((int)ProductVariantFieldIndex.IsActive, value); }
		}

		/// <summary> The UPC property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."UPC"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String UPC
		{
			get { return (System.String)GetValue((int)ProductVariantFieldIndex.UPC, true); }
			set	{ SetValue((int)ProductVariantFieldIndex.UPC, value); }
		}

		/// <summary> The ASIN property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."ASIN"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String ASIN
		{
			get { return (System.String)GetValue((int)ProductVariantFieldIndex.ASIN, true); }
			set	{ SetValue((int)ProductVariantFieldIndex.ASIN, value); }
		}

		/// <summary> The ISBN property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."ISBN"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String ISBN
		{
			get { return (System.String)GetValue((int)ProductVariantFieldIndex.ISBN, true); }
			set	{ SetValue((int)ProductVariantFieldIndex.ISBN, value); }
		}

		/// <summary> The Weight property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."Weight"<br/>
		/// Table field type characteristics (type, precision, scale, length): Decimal, 29, 9, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Decimal> Weight
		{
			get { return (Nullable<System.Decimal>)GetValue((int)ProductVariantFieldIndex.Weight, false); }
			set	{ SetValue((int)ProductVariantFieldIndex.Weight, value); }
		}

		/// <summary> The Length property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."Length"<br/>
		/// Table field type characteristics (type, precision, scale, length): Decimal, 10, 2, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Decimal> Length
		{
			get { return (Nullable<System.Decimal>)GetValue((int)ProductVariantFieldIndex.Length, false); }
			set	{ SetValue((int)ProductVariantFieldIndex.Length, value); }
		}

		/// <summary> The Width property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."Width"<br/>
		/// Table field type characteristics (type, precision, scale, length): Decimal, 10, 2, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Decimal> Width
		{
			get { return (Nullable<System.Decimal>)GetValue((int)ProductVariantFieldIndex.Width, false); }
			set	{ SetValue((int)ProductVariantFieldIndex.Width, value); }
		}

		/// <summary> The Height property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."Height"<br/>
		/// Table field type characteristics (type, precision, scale, length): Decimal, 10, 2, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Decimal> Height
		{
			get { return (Nullable<System.Decimal>)GetValue((int)ProductVariantFieldIndex.Height, false); }
			set	{ SetValue((int)ProductVariantFieldIndex.Height, value); }
		}

		/// <summary> The ImageUrl property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."ImageUrl"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String ImageUrl
		{
			get { return (System.String)GetValue((int)ProductVariantFieldIndex.ImageUrl, true); }
			set	{ SetValue((int)ProductVariantFieldIndex.ImageUrl, value); }
		}

		/// <summary> The BinLocation property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."BinLocation"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String BinLocation
		{
			get { return (System.String)GetValue((int)ProductVariantFieldIndex.BinLocation, true); }
			set	{ SetValue((int)ProductVariantFieldIndex.BinLocation, value); }
		}

		/// <summary> The HarmonizedCode property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."HarmonizedCode"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String HarmonizedCode
		{
			get { return (System.String)GetValue((int)ProductVariantFieldIndex.HarmonizedCode, true); }
			set	{ SetValue((int)ProductVariantFieldIndex.HarmonizedCode, value); }
		}

		/// <summary> The DeclaredValue property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."DeclaredValue"<br/>
		/// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual Nullable<System.Decimal> DeclaredValue
		{
			get { return (Nullable<System.Decimal>)GetValue((int)ProductVariantFieldIndex.DeclaredValue, false); }
			set	{ SetValue((int)ProductVariantFieldIndex.DeclaredValue, value); }
		}

		/// <summary> The CountryOfOrigin property of the Entity ProductVariant<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariant"."CountryOfOrigin"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
		public virtual System.String CountryOfOrigin
		{
			get { return (System.String)GetValue((int)ProductVariantFieldIndex.CountryOfOrigin, true); }
			set	{ SetValue((int)ProductVariantFieldIndex.CountryOfOrigin, value); }
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ProductBundleEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ProductBundleEntity))]
		public virtual EntityCollection<ProductBundleEntity> ProductBundle
		{
			get { return GetOrCreateEntityCollection<ProductBundleEntity, ProductBundleEntityFactory>("ProductVariant", true, false, ref _productBundle);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ProductVariantAliasEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ProductVariantAliasEntity))]
		public virtual EntityCollection<ProductVariantAliasEntity> ProductVariantAlias
		{
			get { return GetOrCreateEntityCollection<ProductVariantAliasEntity, ProductVariantAliasEntityFactory>("ProductVariant", true, false, ref _productVariantAlias);	}
		}

		/// <summary> Gets the EntityCollection with the related entities of type 'ProductVariantTypeAndValueEntity' which are related to this entity via a relation of type '1:n'. If the EntityCollection hasn't been fetched yet, the collection returned will be empty.<br/><br/></summary>
		[TypeContainedAttribute(typeof(ProductVariantTypeAndValueEntity))]
		public virtual EntityCollection<ProductVariantTypeAndValueEntity> ProductVariantTypeAndValue
		{
			get { return GetOrCreateEntityCollection<ProductVariantTypeAndValueEntity, ProductVariantTypeAndValueEntityFactory>("ProductVariant", true, false, ref _productVariantTypeAndValue);	}
		}

		/// <summary> Gets / sets related entity of type 'ProductEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual ProductEntity Product
		{
			get	{ return _product; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncProduct(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "ProductVariant", "Product", _product, true); 
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
			get { return (int)ShipWorks.Data.Model.EntityType.ProductVariantEntity; }
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
