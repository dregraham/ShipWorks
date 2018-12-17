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
	/// <summary>Entity class which represents the entity 'ProductVariantAttributeValue'.<br/><br/></summary>
	[Serializable]
	public partial class ProductVariantAttributeValueEntity : CommonEntityBase
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfaces
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private ProductAttributeEntity _productAttribute;
		private ProductVariantEntity _productVariant;

		// __LLBLGENPRO_USER_CODE_REGION_START PrivateMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Statics
		private static Dictionary<string, string>	_customProperties;
		private static Dictionary<string, Dictionary<string, string>>	_fieldsCustomProperties;

		/// <summary>All names of fields mapped onto a relation. Usable for in-memory filtering</summary>
		public static partial class MemberNames
		{
			/// <summary>Member name ProductAttribute</summary>
			public static readonly string ProductAttribute = "ProductAttribute";
			/// <summary>Member name ProductVariant</summary>
			public static readonly string ProductVariant = "ProductVariant";
		}
		#endregion
		
		/// <summary> Static CTor for setting up custom property hashtables. Is executed before the first instance of this entity class or derived classes is constructed. </summary>
		static ProductVariantAttributeValueEntity()
		{
			SetupCustomPropertyHashtables();
		}
		
		/// <summary> CTor</summary>
		public ProductVariantAttributeValueEntity():base("ProductVariantAttributeValueEntity")
		{
			InitClassEmpty(null, null);
		}

		/// <summary> CTor</summary>
		/// <remarks>For framework usage.</remarks>
		/// <param name="fields">Fields object to set as the fields for this entity.</param>
		public ProductVariantAttributeValueEntity(IEntityFields2 fields):base("ProductVariantAttributeValueEntity")
		{
			InitClassEmpty(null, fields);
		}

		/// <summary> CTor</summary>
		/// <param name="validator">The custom validator object for this ProductVariantAttributeValueEntity</param>
		public ProductVariantAttributeValueEntity(IValidator validator):base("ProductVariantAttributeValueEntity")
		{
			InitClassEmpty(validator, null);
		}
				
		/// <summary> CTor</summary>
		/// <param name="productVariantAttributeValueID">PK value for ProductVariantAttributeValue which data should be fetched into this ProductVariantAttributeValue object</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ProductVariantAttributeValueEntity(System.Int64 productVariantAttributeValueID):base("ProductVariantAttributeValueEntity")
		{
			InitClassEmpty(null, null);
			this.ProductVariantAttributeValueID = productVariantAttributeValueID;
		}

		/// <summary> CTor</summary>
		/// <param name="productVariantAttributeValueID">PK value for ProductVariantAttributeValue which data should be fetched into this ProductVariantAttributeValue object</param>
		/// <param name="validator">The custom validator object for this ProductVariantAttributeValueEntity</param>
		/// <remarks>The entity is not fetched by this constructor. Use a DataAccessAdapter for that.</remarks>
		public ProductVariantAttributeValueEntity(System.Int64 productVariantAttributeValueID, IValidator validator):base("ProductVariantAttributeValueEntity")
		{
			InitClassEmpty(validator, null);
			this.ProductVariantAttributeValueID = productVariantAttributeValueID;
		}

		/// <summary> Protected CTor for deserialization</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected ProductVariantAttributeValueEntity(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if(SerializationHelper.Optimization != SerializationOptimization.Fast) 
			{
				_productAttribute = (ProductAttributeEntity)info.GetValue("_productAttribute", typeof(ProductAttributeEntity));
				if(_productAttribute!=null)
				{
					_productAttribute.AfterSave+=new EventHandler(OnEntityAfterSave);
				}
				_productVariant = (ProductVariantEntity)info.GetValue("_productVariant", typeof(ProductVariantEntity));
				if(_productVariant!=null)
				{
					_productVariant.AfterSave+=new EventHandler(OnEntityAfterSave);
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
			switch((ProductVariantAttributeValueFieldIndex)fieldIndex)
			{
				case ProductVariantAttributeValueFieldIndex.ProductVariantID:
					DesetupSyncProductVariant(true, false);
					break;
				case ProductVariantAttributeValueFieldIndex.ProductAttributeID:
					DesetupSyncProductAttribute(true, false);
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
				case "ProductAttribute":
					this.ProductAttribute = (ProductAttributeEntity)entity;
					break;
				case "ProductVariant":
					this.ProductVariant = (ProductVariantEntity)entity;
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
				case "ProductAttribute":
					toReturn.Add(Relations.ProductAttributeEntityUsingProductAttributeID);
					break;
				case "ProductVariant":
					toReturn.Add(Relations.ProductVariantEntityUsingProductVariantID);
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
				case "ProductAttribute":
					SetupSyncProductAttribute(relatedEntity);
					break;
				case "ProductVariant":
					SetupSyncProductVariant(relatedEntity);
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
				case "ProductAttribute":
					DesetupSyncProductAttribute(false, true);
					break;
				case "ProductVariant":
					DesetupSyncProductVariant(false, true);
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
			if(_productAttribute!=null)
			{
				toReturn.Add(_productAttribute);
			}
			if(_productVariant!=null)
			{
				toReturn.Add(_productVariant);
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
				info.AddValue("_productAttribute", (!this.MarkedForDeletion?_productAttribute:null));
				info.AddValue("_productVariant", (!this.MarkedForDeletion?_productVariant:null));
			}
			// __LLBLGENPRO_USER_CODE_REGION_START GetObjectInfo
			// __LLBLGENPRO_USER_CODE_REGION_END
			base.GetObjectData(info, context);
		}


				
		/// <summary>Gets a list of all the EntityRelation objects the type of this instance has.</summary>
		/// <returns>A list of all the EntityRelation objects the type of this instance has. Hierarchy relations are excluded.</returns>
		protected override List<IEntityRelation> GetAllRelations()
		{
			return new ProductVariantAttributeValueRelations().GetAllRelations();
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'ProductAttribute' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoProductAttribute()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ProductAttributeFields.ProductAttributeID, null, ComparisonOperator.Equal, this.ProductAttributeID));
			return bucket;
		}

		/// <summary> Creates a new IRelationPredicateBucket object which contains the predicate expression and relation collection to fetch the related entity of type 'ProductVariant' to this entity.</summary>
		/// <returns></returns>
		public virtual IRelationPredicateBucket GetRelationInfoProductVariant()
		{
			IRelationPredicateBucket bucket = new RelationPredicateBucket();
			bucket.PredicateExpression.Add(new FieldCompareValuePredicate(ProductVariantFields.ProductVariantID, null, ComparisonOperator.Equal, this.ProductVariantID));
			return bucket;
		}
		

		/// <summary>Creates a new instance of the factory related to this entity</summary>
		protected override IEntityFactory2 CreateEntityFactory()
		{
			return EntityFactoryCache2.GetEntityFactory(typeof(ProductVariantAttributeValueEntityFactory));
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
			toReturn.Add("ProductAttribute", _productAttribute);
			toReturn.Add("ProductVariant", _productVariant);
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
			_fieldsCustomProperties.Add("ProductVariantAttributeValueID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ProductVariantID", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("AttributeValue", fieldHashtable);
			fieldHashtable = new Dictionary<string, string>();
			_fieldsCustomProperties.Add("ProductAttributeID", fieldHashtable);
		}
		#endregion

		/// <summary> Removes the sync logic for member _productAttribute</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncProductAttribute(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _productAttribute, new PropertyChangedEventHandler( OnProductAttributePropertyChanged ), "ProductAttribute", ShipWorks.Data.Model.RelationClasses.StaticProductVariantAttributeValueRelations.ProductAttributeEntityUsingProductAttributeIDStatic, true, signalRelatedEntity, "ProductVariantAttributeValue", resetFKFields, new int[] { (int)ProductVariantAttributeValueFieldIndex.ProductAttributeID } );
			_productAttribute = null;
		}

		/// <summary> setups the sync logic for member _productAttribute</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncProductAttribute(IEntityCore relatedEntity)
		{
			if(_productAttribute!=relatedEntity)
			{
				DesetupSyncProductAttribute(true, true);
				_productAttribute = (ProductAttributeEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _productAttribute, new PropertyChangedEventHandler( OnProductAttributePropertyChanged ), "ProductAttribute", ShipWorks.Data.Model.RelationClasses.StaticProductVariantAttributeValueRelations.ProductAttributeEntityUsingProductAttributeIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnProductAttributePropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Removes the sync logic for member _productVariant</summary>
		/// <param name="signalRelatedEntity">If set to true, it will call the related entity's UnsetRelatedEntity method</param>
		/// <param name="resetFKFields">if set to true it will also reset the FK fields pointing to the related entity</param>
		private void DesetupSyncProductVariant(bool signalRelatedEntity, bool resetFKFields)
		{
			this.PerformDesetupSyncRelatedEntity( _productVariant, new PropertyChangedEventHandler( OnProductVariantPropertyChanged ), "ProductVariant", ShipWorks.Data.Model.RelationClasses.StaticProductVariantAttributeValueRelations.ProductVariantEntityUsingProductVariantIDStatic, true, signalRelatedEntity, "Attributes", resetFKFields, new int[] { (int)ProductVariantAttributeValueFieldIndex.ProductVariantID } );
			_productVariant = null;
		}

		/// <summary> setups the sync logic for member _productVariant</summary>
		/// <param name="relatedEntity">Instance to set as the related entity of type entityType</param>
		private void SetupSyncProductVariant(IEntityCore relatedEntity)
		{
			if(_productVariant!=relatedEntity)
			{
				DesetupSyncProductVariant(true, true);
				_productVariant = (ProductVariantEntity)relatedEntity;
				this.PerformSetupSyncRelatedEntity( _productVariant, new PropertyChangedEventHandler( OnProductVariantPropertyChanged ), "ProductVariant", ShipWorks.Data.Model.RelationClasses.StaticProductVariantAttributeValueRelations.ProductVariantEntityUsingProductVariantIDStatic, true, new string[] {  } );
			}
		}
		
		/// <summary>Handles property change events of properties in a related entity.</summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnProductVariantPropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			switch( e.PropertyName )
			{
				default:
					break;
			}
		}

		/// <summary> Initializes the class with empty data, as if it is a new Entity.</summary>
		/// <param name="validator">The validator object for this ProductVariantAttributeValueEntity</param>
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
		public  static ProductVariantAttributeValueRelations Relations
		{
			get	{ return new ProductVariantAttributeValueRelations(); }
		}
		
		/// <summary> The custom properties for this entity type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public  static Dictionary<string, string> CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ProductAttribute' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathProductAttribute
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(ProductAttributeEntityFactory))),	(IEntityRelation)GetRelationsForField("ProductAttribute")[0], (int)ShipWorks.Data.Model.EntityType.ProductVariantAttributeValueEntity, (int)ShipWorks.Data.Model.EntityType.ProductAttributeEntity, 0, null, null, null, null, "ProductAttribute", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
		}

		/// <summary> Creates a new PrefetchPathElement2 object which contains all the information to prefetch the related entities of type 'ProductVariant' for this entity.</summary>
		/// <returns>Ready to use IPrefetchPathElement2 implementation.</returns>
		public static IPrefetchPathElement2 PrefetchPathProductVariant
		{
			get	{ return new PrefetchPathElement2(new EntityCollection(EntityFactoryCache2.GetEntityFactory(typeof(ProductVariantEntityFactory))),	(IEntityRelation)GetRelationsForField("ProductVariant")[0], (int)ShipWorks.Data.Model.EntityType.ProductVariantAttributeValueEntity, (int)ShipWorks.Data.Model.EntityType.ProductVariantEntity, 0, null, null, null, null, "ProductVariant", SD.LLBLGen.Pro.ORMSupportClasses.RelationType.ManyToOne); }
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

		/// <summary> The ProductVariantAttributeValueID property of the Entity ProductVariantAttributeValue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariantAttributeValue"."ProductVariantAttributeValueID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
		public virtual System.Int64 ProductVariantAttributeValueID
		{
			get { return (System.Int64)GetValue((int)ProductVariantAttributeValueFieldIndex.ProductVariantAttributeValueID, true); }
			set	{ SetValue((int)ProductVariantAttributeValueFieldIndex.ProductVariantAttributeValueID, value); }
		}

		/// <summary> The ProductVariantID property of the Entity ProductVariantAttributeValue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariantAttributeValue"."ProductVariantID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 ProductVariantID
		{
			get { return (System.Int64)GetValue((int)ProductVariantAttributeValueFieldIndex.ProductVariantID, true); }
			set	{ SetValue((int)ProductVariantAttributeValueFieldIndex.ProductVariantID, value); }
		}

		/// <summary> The AttributeValue property of the Entity ProductVariantAttributeValue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariantAttributeValue"."AttributeValue"<br/>
		/// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.String AttributeValue
		{
			get { return (System.String)GetValue((int)ProductVariantAttributeValueFieldIndex.AttributeValue, true); }
			set	{ SetValue((int)ProductVariantAttributeValueFieldIndex.AttributeValue, value); }
		}

		/// <summary> The ProductAttributeID property of the Entity ProductVariantAttributeValue<br/><br/></summary>
		/// <remarks>Mapped on  table field: "ProductVariantAttributeValue"."ProductAttributeID"<br/>
		/// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
		/// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
		public virtual System.Int64 ProductAttributeID
		{
			get { return (System.Int64)GetValue((int)ProductVariantAttributeValueFieldIndex.ProductAttributeID, true); }
			set	{ SetValue((int)ProductVariantAttributeValueFieldIndex.ProductAttributeID, value); }
		}

		/// <summary> Gets / sets related entity of type 'ProductAttributeEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual ProductAttributeEntity ProductAttribute
		{
			get	{ return _productAttribute; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncProductAttribute(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "ProductVariantAttributeValue", "ProductAttribute", _productAttribute, true); 
				}
			}
		}

		/// <summary> Gets / sets related entity of type 'ProductVariantEntity' which has to be set using a fetch action earlier. If no related entity is set for this property, null is returned..<br/><br/></summary>
		[Browsable(true)]
		public virtual ProductVariantEntity ProductVariant
		{
			get	{ return _productVariant; }
			set
			{
				if(this.IsDeserializing)
				{
					SetupSyncProductVariant(value);
				}
				else
				{
					SetSingleRelatedEntityNavigator(value, "Attributes", "ProductVariant", _productVariant, true); 
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
			get { return (int)ShipWorks.Data.Model.EntityType.ProductVariantAttributeValueEntity; }
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
