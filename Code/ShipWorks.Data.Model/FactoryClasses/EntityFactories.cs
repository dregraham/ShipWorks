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
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model.RelationClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.FactoryClasses
{
	
    // __LLBLGENPRO_USER_CODE_REGION_START AdditionalNamespaces
    // __LLBLGENPRO_USER_CODE_REGION_END
	
	/// <summary>general base class for the generated factories</summary>
	[Serializable]
	public partial class EntityFactoryBase2<TEntity> : EntityFactoryCore2
		where TEntity : EntityBase2, IEntity2
	{
		private readonly ShipWorks.Data.Model.EntityType _typeOfEntity;
		private readonly bool _isInHierarchy;
		
		/// <summary>CTor</summary>
		/// <param name="entityName">Name of the entity.</param>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <param name="isInHierarchy">If true, the entity of this factory is in an inheritance hierarchy, false otherwise</param>
		public EntityFactoryBase2(string entityName, ShipWorks.Data.Model.EntityType typeOfEntity, bool isInHierarchy) : base(entityName)
		{
			_typeOfEntity = typeOfEntity;
			_isInHierarchy = isInHierarchy;
		}
		
		/// <summary>Creates, using the generated EntityFieldsFactory, the IEntityFields2 object for the entity to create.</summary>
		/// <returns>Empty IEntityFields2 object.</returns>
		public override IEntityFields2 CreateFields()
		{
			return EntityFieldsFactory.CreateEntityFieldsObject(_typeOfEntity);
		}
		
		/// <summary>Creates a new entity instance using the GeneralEntityFactory in the generated code, using the passed in entitytype value</summary>
		/// <param name="entityTypeValue">The entity type value of the entity to create an instance for.</param>
		/// <returns>new IEntity instance</returns>
		public override IEntity2 CreateEntityFromEntityTypeValue(int entityTypeValue)
		{
			return GeneralEntityFactory.Create((ShipWorks.Data.Model.EntityType)entityTypeValue);
		}

		/// <summary>Creates the relations collection to the entity to join all targets so this entity can be fetched. </summary>
		/// <param name="objectAlias">The object alias to use for the elements in the relations.</param>
		/// <returns>null if the entity isn't in a hierarchy of type TargetPerEntity, otherwise the relations collection needed to join all targets together to fetch all subtypes of this entity and this entity itself</returns>
		public override IRelationCollection CreateHierarchyRelations(string objectAlias) 
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetHierarchyRelations(this.ForEntityName, objectAlias);
		}

		/// <summary>This method retrieves, using the InheritanceInfoprovider, the factory for the entity represented by the values passed in.</summary>
		/// <param name="fieldValues">Field values read from the db, to determine which factory to return, based on the field values passed in.</param>
		/// <param name="entityFieldStartIndexesPerEntity">indexes into values where per entity type their own fields start.</param>
		/// <returns>the factory for the entity which is represented by the values passed in.</returns>
		public override IEntityFactory2 GetEntityFactory(object[] fieldValues, Dictionary<string, int> entityFieldStartIndexesPerEntity) 
		{
			IEntityFactory2 toReturn = (IEntityFactory2)InheritanceInfoProviderSingleton.GetInstance().GetEntityFactory(this.ForEntityName, fieldValues, entityFieldStartIndexesPerEntity);
			if(toReturn == null)
			{
				toReturn = this;
			}
			return toReturn;
		}
		
		/// <summary>Gets a predicateexpression which filters on the entity with type belonging to this factory.</summary>
		/// <param name="negate">Flag to produce a NOT filter, (true), or a normal filter (false). </param>
		/// <param name="objectAlias">The object alias to use for the predicate(s).</param>
		/// <returns>ready to use predicateexpression, or an empty predicate expression if the belonging entity isn't a hierarchical type.</returns>
		public override IPredicateExpression GetEntityTypeFilter(bool negate, string objectAlias) 
		{
			return InheritanceInfoProviderSingleton.GetInstance().GetEntityTypeFilter(this.ForEntityName, objectAlias, negate);
		}
						
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity which this factory belongs to.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<TEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return _isInHierarchy ? new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields(this.ForEntityName), InheritanceInfoProviderSingleton.GetInstance(), null) : base.CreateHierarchyFields();
		}
	}

	/// <summary>Factory to create new, empty ActionEntity objects.</summary>
	[Serializable]
	public partial class ActionEntityFactory : EntityFactoryBase2<ActionEntity> {
		/// <summary>CTor</summary>
		public ActionEntityFactory() : base("ActionEntity", ShipWorks.Data.Model.EntityType.ActionEntity, false) { }
		
		/// <summary>Creates a new ActionEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ActionEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ActionFilterTriggerEntity objects.</summary>
	[Serializable]
	public partial class ActionFilterTriggerEntityFactory : EntityFactoryBase2<ActionFilterTriggerEntity> {
		/// <summary>CTor</summary>
		public ActionFilterTriggerEntityFactory() : base("ActionFilterTriggerEntity", ShipWorks.Data.Model.EntityType.ActionFilterTriggerEntity, false) { }
		
		/// <summary>Creates a new ActionFilterTriggerEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ActionFilterTriggerEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionFilterTriggerUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ActionQueueEntity objects.</summary>
	[Serializable]
	public partial class ActionQueueEntityFactory : EntityFactoryBase2<ActionQueueEntity> {
		/// <summary>CTor</summary>
		public ActionQueueEntityFactory() : base("ActionQueueEntity", ShipWorks.Data.Model.EntityType.ActionQueueEntity, false) { }
		
		/// <summary>Creates a new ActionQueueEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ActionQueueEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionQueueUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ActionQueueSelectionEntity objects.</summary>
	[Serializable]
	public partial class ActionQueueSelectionEntityFactory : EntityFactoryBase2<ActionQueueSelectionEntity> {
		/// <summary>CTor</summary>
		public ActionQueueSelectionEntityFactory() : base("ActionQueueSelectionEntity", ShipWorks.Data.Model.EntityType.ActionQueueSelectionEntity, false) { }
		
		/// <summary>Creates a new ActionQueueSelectionEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ActionQueueSelectionEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionQueueSelectionUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ActionQueueStepEntity objects.</summary>
	[Serializable]
	public partial class ActionQueueStepEntityFactory : EntityFactoryBase2<ActionQueueStepEntity> {
		/// <summary>CTor</summary>
		public ActionQueueStepEntityFactory() : base("ActionQueueStepEntity", ShipWorks.Data.Model.EntityType.ActionQueueStepEntity, false) { }
		
		/// <summary>Creates a new ActionQueueStepEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ActionQueueStepEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionQueueStepUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ActionTaskEntity objects.</summary>
	[Serializable]
	public partial class ActionTaskEntityFactory : EntityFactoryBase2<ActionTaskEntity> {
		/// <summary>CTor</summary>
		public ActionTaskEntityFactory() : base("ActionTaskEntity", ShipWorks.Data.Model.EntityType.ActionTaskEntity, false) { }
		
		/// <summary>Creates a new ActionTaskEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ActionTaskEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionTaskUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty AmazonASINEntity objects.</summary>
	[Serializable]
	public partial class AmazonASINEntityFactory : EntityFactoryBase2<AmazonASINEntity> {
		/// <summary>CTor</summary>
		public AmazonASINEntityFactory() : base("AmazonASINEntity", ShipWorks.Data.Model.EntityType.AmazonASINEntity, false) { }
		
		/// <summary>Creates a new AmazonASINEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AmazonASINEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonASINUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty AmazonOrderEntity objects.</summary>
	[Serializable]
	public partial class AmazonOrderEntityFactory : EntityFactoryBase2<AmazonOrderEntity> {
		/// <summary>CTor</summary>
		public AmazonOrderEntityFactory() : base("AmazonOrderEntity", ShipWorks.Data.Model.EntityType.AmazonOrderEntity, true) { }
		
		/// <summary>Creates a new AmazonOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AmazonOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty AmazonOrderItemEntity objects.</summary>
	[Serializable]
	public partial class AmazonOrderItemEntityFactory : EntityFactoryBase2<AmazonOrderItemEntity> {
		/// <summary>CTor</summary>
		public AmazonOrderItemEntityFactory() : base("AmazonOrderItemEntity", ShipWorks.Data.Model.EntityType.AmazonOrderItemEntity, true) { }
		
		/// <summary>Creates a new AmazonOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AmazonOrderItemEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonOrderItemUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty AmazonOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class AmazonOrderSearchEntityFactory : EntityFactoryBase2<AmazonOrderSearchEntity> {
		/// <summary>CTor</summary>
		public AmazonOrderSearchEntityFactory() : base("AmazonOrderSearchEntity", ShipWorks.Data.Model.EntityType.AmazonOrderSearchEntity, false) { }
		
		/// <summary>Creates a new AmazonOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AmazonOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty AmazonProfileEntity objects.</summary>
	[Serializable]
	public partial class AmazonProfileEntityFactory : EntityFactoryBase2<AmazonProfileEntity> {
		/// <summary>CTor</summary>
		public AmazonProfileEntityFactory() : base("AmazonProfileEntity", ShipWorks.Data.Model.EntityType.AmazonProfileEntity, false) { }
		
		/// <summary>Creates a new AmazonProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AmazonProfileEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonProfileUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty AmazonShipmentEntity objects.</summary>
	[Serializable]
	public partial class AmazonShipmentEntityFactory : EntityFactoryBase2<AmazonShipmentEntity> {
		/// <summary>CTor</summary>
		public AmazonShipmentEntityFactory() : base("AmazonShipmentEntity", ShipWorks.Data.Model.EntityType.AmazonShipmentEntity, false) { }
		
		/// <summary>Creates a new AmazonShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AmazonShipmentEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonShipmentUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty AmazonStoreEntity objects.</summary>
	[Serializable]
	public partial class AmazonStoreEntityFactory : EntityFactoryBase2<AmazonStoreEntity> {
		/// <summary>CTor</summary>
		public AmazonStoreEntityFactory() : base("AmazonStoreEntity", ShipWorks.Data.Model.EntityType.AmazonStoreEntity, true) { }
		
		/// <summary>Creates a new AmazonStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AmazonStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty AmeriCommerceStoreEntity objects.</summary>
	[Serializable]
	public partial class AmeriCommerceStoreEntityFactory : EntityFactoryBase2<AmeriCommerceStoreEntity> {
		/// <summary>CTor</summary>
		public AmeriCommerceStoreEntityFactory() : base("AmeriCommerceStoreEntity", ShipWorks.Data.Model.EntityType.AmeriCommerceStoreEntity, true) { }
		
		/// <summary>Creates a new AmeriCommerceStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AmeriCommerceStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmeriCommerceStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty AuditEntity objects.</summary>
	[Serializable]
	public partial class AuditEntityFactory : EntityFactoryBase2<AuditEntity> {
		/// <summary>CTor</summary>
		public AuditEntityFactory() : base("AuditEntity", ShipWorks.Data.Model.EntityType.AuditEntity, false) { }
		
		/// <summary>Creates a new AuditEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AuditEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewAuditUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty AuditChangeEntity objects.</summary>
	[Serializable]
	public partial class AuditChangeEntityFactory : EntityFactoryBase2<AuditChangeEntity> {
		/// <summary>CTor</summary>
		public AuditChangeEntityFactory() : base("AuditChangeEntity", ShipWorks.Data.Model.EntityType.AuditChangeEntity, false) { }
		
		/// <summary>Creates a new AuditChangeEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AuditChangeEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewAuditChangeUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty AuditChangeDetailEntity objects.</summary>
	[Serializable]
	public partial class AuditChangeDetailEntityFactory : EntityFactoryBase2<AuditChangeDetailEntity> {
		/// <summary>CTor</summary>
		public AuditChangeDetailEntityFactory() : base("AuditChangeDetailEntity", ShipWorks.Data.Model.EntityType.AuditChangeDetailEntity, false) { }
		
		/// <summary>Creates a new AuditChangeDetailEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AuditChangeDetailEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewAuditChangeDetailUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty BestRateProfileEntity objects.</summary>
	[Serializable]
	public partial class BestRateProfileEntityFactory : EntityFactoryBase2<BestRateProfileEntity> {
		/// <summary>CTor</summary>
		public BestRateProfileEntityFactory() : base("BestRateProfileEntity", ShipWorks.Data.Model.EntityType.BestRateProfileEntity, false) { }
		
		/// <summary>Creates a new BestRateProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new BestRateProfileEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewBestRateProfileUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty BestRateShipmentEntity objects.</summary>
	[Serializable]
	public partial class BestRateShipmentEntityFactory : EntityFactoryBase2<BestRateShipmentEntity> {
		/// <summary>CTor</summary>
		public BestRateShipmentEntityFactory() : base("BestRateShipmentEntity", ShipWorks.Data.Model.EntityType.BestRateShipmentEntity, false) { }
		
		/// <summary>Creates a new BestRateShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new BestRateShipmentEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewBestRateShipmentUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty BigCommerceOrderItemEntity objects.</summary>
	[Serializable]
	public partial class BigCommerceOrderItemEntityFactory : EntityFactoryBase2<BigCommerceOrderItemEntity> {
		/// <summary>CTor</summary>
		public BigCommerceOrderItemEntityFactory() : base("BigCommerceOrderItemEntity", ShipWorks.Data.Model.EntityType.BigCommerceOrderItemEntity, true) { }
		
		/// <summary>Creates a new BigCommerceOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new BigCommerceOrderItemEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewBigCommerceOrderItemUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty BigCommerceStoreEntity objects.</summary>
	[Serializable]
	public partial class BigCommerceStoreEntityFactory : EntityFactoryBase2<BigCommerceStoreEntity> {
		/// <summary>CTor</summary>
		public BigCommerceStoreEntityFactory() : base("BigCommerceStoreEntity", ShipWorks.Data.Model.EntityType.BigCommerceStoreEntity, true) { }
		
		/// <summary>Creates a new BigCommerceStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new BigCommerceStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewBigCommerceStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty BuyDotComOrderItemEntity objects.</summary>
	[Serializable]
	public partial class BuyDotComOrderItemEntityFactory : EntityFactoryBase2<BuyDotComOrderItemEntity> {
		/// <summary>CTor</summary>
		public BuyDotComOrderItemEntityFactory() : base("BuyDotComOrderItemEntity", ShipWorks.Data.Model.EntityType.BuyDotComOrderItemEntity, true) { }
		
		/// <summary>Creates a new BuyDotComOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new BuyDotComOrderItemEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewBuyDotComOrderItemUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty BuyDotComStoreEntity objects.</summary>
	[Serializable]
	public partial class BuyDotComStoreEntityFactory : EntityFactoryBase2<BuyDotComStoreEntity> {
		/// <summary>CTor</summary>
		public BuyDotComStoreEntityFactory() : base("BuyDotComStoreEntity", ShipWorks.Data.Model.EntityType.BuyDotComStoreEntity, true) { }
		
		/// <summary>Creates a new BuyDotComStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new BuyDotComStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewBuyDotComStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ChannelAdvisorOrderEntity objects.</summary>
	[Serializable]
	public partial class ChannelAdvisorOrderEntityFactory : EntityFactoryBase2<ChannelAdvisorOrderEntity> {
		/// <summary>CTor</summary>
		public ChannelAdvisorOrderEntityFactory() : base("ChannelAdvisorOrderEntity", ShipWorks.Data.Model.EntityType.ChannelAdvisorOrderEntity, true) { }
		
		/// <summary>Creates a new ChannelAdvisorOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ChannelAdvisorOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewChannelAdvisorOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ChannelAdvisorOrderItemEntity objects.</summary>
	[Serializable]
	public partial class ChannelAdvisorOrderItemEntityFactory : EntityFactoryBase2<ChannelAdvisorOrderItemEntity> {
		/// <summary>CTor</summary>
		public ChannelAdvisorOrderItemEntityFactory() : base("ChannelAdvisorOrderItemEntity", ShipWorks.Data.Model.EntityType.ChannelAdvisorOrderItemEntity, true) { }
		
		/// <summary>Creates a new ChannelAdvisorOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ChannelAdvisorOrderItemEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewChannelAdvisorOrderItemUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ChannelAdvisorOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class ChannelAdvisorOrderSearchEntityFactory : EntityFactoryBase2<ChannelAdvisorOrderSearchEntity> {
		/// <summary>CTor</summary>
		public ChannelAdvisorOrderSearchEntityFactory() : base("ChannelAdvisorOrderSearchEntity", ShipWorks.Data.Model.EntityType.ChannelAdvisorOrderSearchEntity, false) { }
		
		/// <summary>Creates a new ChannelAdvisorOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ChannelAdvisorOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewChannelAdvisorOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ChannelAdvisorStoreEntity objects.</summary>
	[Serializable]
	public partial class ChannelAdvisorStoreEntityFactory : EntityFactoryBase2<ChannelAdvisorStoreEntity> {
		/// <summary>CTor</summary>
		public ChannelAdvisorStoreEntityFactory() : base("ChannelAdvisorStoreEntity", ShipWorks.Data.Model.EntityType.ChannelAdvisorStoreEntity, true) { }
		
		/// <summary>Creates a new ChannelAdvisorStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ChannelAdvisorStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewChannelAdvisorStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ClickCartProOrderEntity objects.</summary>
	[Serializable]
	public partial class ClickCartProOrderEntityFactory : EntityFactoryBase2<ClickCartProOrderEntity> {
		/// <summary>CTor</summary>
		public ClickCartProOrderEntityFactory() : base("ClickCartProOrderEntity", ShipWorks.Data.Model.EntityType.ClickCartProOrderEntity, true) { }
		
		/// <summary>Creates a new ClickCartProOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ClickCartProOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewClickCartProOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ClickCartProOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class ClickCartProOrderSearchEntityFactory : EntityFactoryBase2<ClickCartProOrderSearchEntity> {
		/// <summary>CTor</summary>
		public ClickCartProOrderSearchEntityFactory() : base("ClickCartProOrderSearchEntity", ShipWorks.Data.Model.EntityType.ClickCartProOrderSearchEntity, false) { }
		
		/// <summary>Creates a new ClickCartProOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ClickCartProOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewClickCartProOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty CommerceInterfaceOrderEntity objects.</summary>
	[Serializable]
	public partial class CommerceInterfaceOrderEntityFactory : EntityFactoryBase2<CommerceInterfaceOrderEntity> {
		/// <summary>CTor</summary>
		public CommerceInterfaceOrderEntityFactory() : base("CommerceInterfaceOrderEntity", ShipWorks.Data.Model.EntityType.CommerceInterfaceOrderEntity, true) { }
		
		/// <summary>Creates a new CommerceInterfaceOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new CommerceInterfaceOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewCommerceInterfaceOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty CommerceInterfaceOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class CommerceInterfaceOrderSearchEntityFactory : EntityFactoryBase2<CommerceInterfaceOrderSearchEntity> {
		/// <summary>CTor</summary>
		public CommerceInterfaceOrderSearchEntityFactory() : base("CommerceInterfaceOrderSearchEntity", ShipWorks.Data.Model.EntityType.CommerceInterfaceOrderSearchEntity, false) { }
		
		/// <summary>Creates a new CommerceInterfaceOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new CommerceInterfaceOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewCommerceInterfaceOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ComputerEntity objects.</summary>
	[Serializable]
	public partial class ComputerEntityFactory : EntityFactoryBase2<ComputerEntity> {
		/// <summary>CTor</summary>
		public ComputerEntityFactory() : base("ComputerEntity", ShipWorks.Data.Model.EntityType.ComputerEntity, false) { }
		
		/// <summary>Creates a new ComputerEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ComputerEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewComputerUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ConfigurationEntity objects.</summary>
	[Serializable]
	public partial class ConfigurationEntityFactory : EntityFactoryBase2<ConfigurationEntity> {
		/// <summary>CTor</summary>
		public ConfigurationEntityFactory() : base("ConfigurationEntity", ShipWorks.Data.Model.EntityType.ConfigurationEntity, false) { }
		
		/// <summary>Creates a new ConfigurationEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ConfigurationEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewConfigurationUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty CustomerEntity objects.</summary>
	[Serializable]
	public partial class CustomerEntityFactory : EntityFactoryBase2<CustomerEntity> {
		/// <summary>CTor</summary>
		public CustomerEntityFactory() : base("CustomerEntity", ShipWorks.Data.Model.EntityType.CustomerEntity, false) { }
		
		/// <summary>Creates a new CustomerEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new CustomerEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewCustomerUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty DimensionsProfileEntity objects.</summary>
	[Serializable]
	public partial class DimensionsProfileEntityFactory : EntityFactoryBase2<DimensionsProfileEntity> {
		/// <summary>CTor</summary>
		public DimensionsProfileEntityFactory() : base("DimensionsProfileEntity", ShipWorks.Data.Model.EntityType.DimensionsProfileEntity, false) { }
		
		/// <summary>Creates a new DimensionsProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new DimensionsProfileEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewDimensionsProfileUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty DownloadEntity objects.</summary>
	[Serializable]
	public partial class DownloadEntityFactory : EntityFactoryBase2<DownloadEntity> {
		/// <summary>CTor</summary>
		public DownloadEntityFactory() : base("DownloadEntity", ShipWorks.Data.Model.EntityType.DownloadEntity, false) { }
		
		/// <summary>Creates a new DownloadEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new DownloadEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewDownloadUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty DownloadDetailEntity objects.</summary>
	[Serializable]
	public partial class DownloadDetailEntityFactory : EntityFactoryBase2<DownloadDetailEntity> {
		/// <summary>CTor</summary>
		public DownloadDetailEntityFactory() : base("DownloadDetailEntity", ShipWorks.Data.Model.EntityType.DownloadDetailEntity, false) { }
		
		/// <summary>Creates a new DownloadDetailEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new DownloadDetailEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewDownloadDetailUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty EbayCombinedOrderRelationEntity objects.</summary>
	[Serializable]
	public partial class EbayCombinedOrderRelationEntityFactory : EntityFactoryBase2<EbayCombinedOrderRelationEntity> {
		/// <summary>CTor</summary>
		public EbayCombinedOrderRelationEntityFactory() : base("EbayCombinedOrderRelationEntity", ShipWorks.Data.Model.EntityType.EbayCombinedOrderRelationEntity, false) { }
		
		/// <summary>Creates a new EbayCombinedOrderRelationEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EbayCombinedOrderRelationEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewEbayCombinedOrderRelationUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty EbayOrderEntity objects.</summary>
	[Serializable]
	public partial class EbayOrderEntityFactory : EntityFactoryBase2<EbayOrderEntity> {
		/// <summary>CTor</summary>
		public EbayOrderEntityFactory() : base("EbayOrderEntity", ShipWorks.Data.Model.EntityType.EbayOrderEntity, true) { }
		
		/// <summary>Creates a new EbayOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EbayOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewEbayOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty EbayOrderItemEntity objects.</summary>
	[Serializable]
	public partial class EbayOrderItemEntityFactory : EntityFactoryBase2<EbayOrderItemEntity> {
		/// <summary>CTor</summary>
		public EbayOrderItemEntityFactory() : base("EbayOrderItemEntity", ShipWorks.Data.Model.EntityType.EbayOrderItemEntity, true) { }
		
		/// <summary>Creates a new EbayOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EbayOrderItemEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewEbayOrderItemUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty EbayOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class EbayOrderSearchEntityFactory : EntityFactoryBase2<EbayOrderSearchEntity> {
		/// <summary>CTor</summary>
		public EbayOrderSearchEntityFactory() : base("EbayOrderSearchEntity", ShipWorks.Data.Model.EntityType.EbayOrderSearchEntity, false) { }
		
		/// <summary>Creates a new EbayOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EbayOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEbayOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty EbayStoreEntity objects.</summary>
	[Serializable]
	public partial class EbayStoreEntityFactory : EntityFactoryBase2<EbayStoreEntity> {
		/// <summary>CTor</summary>
		public EbayStoreEntityFactory() : base("EbayStoreEntity", ShipWorks.Data.Model.EntityType.EbayStoreEntity, true) { }
		
		/// <summary>Creates a new EbayStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EbayStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewEbayStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty EmailAccountEntity objects.</summary>
	[Serializable]
	public partial class EmailAccountEntityFactory : EntityFactoryBase2<EmailAccountEntity> {
		/// <summary>CTor</summary>
		public EmailAccountEntityFactory() : base("EmailAccountEntity", ShipWorks.Data.Model.EntityType.EmailAccountEntity, false) { }
		
		/// <summary>Creates a new EmailAccountEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EmailAccountEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewEmailAccountUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty EmailOutboundEntity objects.</summary>
	[Serializable]
	public partial class EmailOutboundEntityFactory : EntityFactoryBase2<EmailOutboundEntity> {
		/// <summary>CTor</summary>
		public EmailOutboundEntityFactory() : base("EmailOutboundEntity", ShipWorks.Data.Model.EntityType.EmailOutboundEntity, false) { }
		
		/// <summary>Creates a new EmailOutboundEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EmailOutboundEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewEmailOutboundUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty EmailOutboundRelationEntity objects.</summary>
	[Serializable]
	public partial class EmailOutboundRelationEntityFactory : EntityFactoryBase2<EmailOutboundRelationEntity> {
		/// <summary>CTor</summary>
		public EmailOutboundRelationEntityFactory() : base("EmailOutboundRelationEntity", ShipWorks.Data.Model.EntityType.EmailOutboundRelationEntity, false) { }
		
		/// <summary>Creates a new EmailOutboundRelationEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EmailOutboundRelationEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewEmailOutboundRelationUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty EndiciaAccountEntity objects.</summary>
	[Serializable]
	public partial class EndiciaAccountEntityFactory : EntityFactoryBase2<EndiciaAccountEntity> {
		/// <summary>CTor</summary>
		public EndiciaAccountEntityFactory() : base("EndiciaAccountEntity", ShipWorks.Data.Model.EntityType.EndiciaAccountEntity, false) { }
		
		/// <summary>Creates a new EndiciaAccountEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EndiciaAccountEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewEndiciaAccountUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty EndiciaProfileEntity objects.</summary>
	[Serializable]
	public partial class EndiciaProfileEntityFactory : EntityFactoryBase2<EndiciaProfileEntity> {
		/// <summary>CTor</summary>
		public EndiciaProfileEntityFactory() : base("EndiciaProfileEntity", ShipWorks.Data.Model.EntityType.EndiciaProfileEntity, false) { }
		
		/// <summary>Creates a new EndiciaProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EndiciaProfileEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewEndiciaProfileUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty EndiciaScanFormEntity objects.</summary>
	[Serializable]
	public partial class EndiciaScanFormEntityFactory : EntityFactoryBase2<EndiciaScanFormEntity> {
		/// <summary>CTor</summary>
		public EndiciaScanFormEntityFactory() : base("EndiciaScanFormEntity", ShipWorks.Data.Model.EntityType.EndiciaScanFormEntity, false) { }
		
		/// <summary>Creates a new EndiciaScanFormEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EndiciaScanFormEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewEndiciaScanFormUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty EndiciaShipmentEntity objects.</summary>
	[Serializable]
	public partial class EndiciaShipmentEntityFactory : EntityFactoryBase2<EndiciaShipmentEntity> {
		/// <summary>CTor</summary>
		public EndiciaShipmentEntityFactory() : base("EndiciaShipmentEntity", ShipWorks.Data.Model.EntityType.EndiciaShipmentEntity, false) { }
		
		/// <summary>Creates a new EndiciaShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EndiciaShipmentEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewEndiciaShipmentUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty EtsyOrderEntity objects.</summary>
	[Serializable]
	public partial class EtsyOrderEntityFactory : EntityFactoryBase2<EtsyOrderEntity> {
		/// <summary>CTor</summary>
		public EtsyOrderEntityFactory() : base("EtsyOrderEntity", ShipWorks.Data.Model.EntityType.EtsyOrderEntity, true) { }
		
		/// <summary>Creates a new EtsyOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EtsyOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewEtsyOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty EtsyStoreEntity objects.</summary>
	[Serializable]
	public partial class EtsyStoreEntityFactory : EntityFactoryBase2<EtsyStoreEntity> {
		/// <summary>CTor</summary>
		public EtsyStoreEntityFactory() : base("EtsyStoreEntity", ShipWorks.Data.Model.EntityType.EtsyStoreEntity, true) { }
		
		/// <summary>Creates a new EtsyStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EtsyStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewEtsyStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ExcludedPackageTypeEntity objects.</summary>
	[Serializable]
	public partial class ExcludedPackageTypeEntityFactory : EntityFactoryBase2<ExcludedPackageTypeEntity> {
		/// <summary>CTor</summary>
		public ExcludedPackageTypeEntityFactory() : base("ExcludedPackageTypeEntity", ShipWorks.Data.Model.EntityType.ExcludedPackageTypeEntity, false) { }
		
		/// <summary>Creates a new ExcludedPackageTypeEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ExcludedPackageTypeEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewExcludedPackageTypeUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ExcludedServiceTypeEntity objects.</summary>
	[Serializable]
	public partial class ExcludedServiceTypeEntityFactory : EntityFactoryBase2<ExcludedServiceTypeEntity> {
		/// <summary>CTor</summary>
		public ExcludedServiceTypeEntityFactory() : base("ExcludedServiceTypeEntity", ShipWorks.Data.Model.EntityType.ExcludedServiceTypeEntity, false) { }
		
		/// <summary>Creates a new ExcludedServiceTypeEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ExcludedServiceTypeEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewExcludedServiceTypeUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty FedExAccountEntity objects.</summary>
	[Serializable]
	public partial class FedExAccountEntityFactory : EntityFactoryBase2<FedExAccountEntity> {
		/// <summary>CTor</summary>
		public FedExAccountEntityFactory() : base("FedExAccountEntity", ShipWorks.Data.Model.EntityType.FedExAccountEntity, false) { }
		
		/// <summary>Creates a new FedExAccountEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FedExAccountEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExAccountUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty FedExEndOfDayCloseEntity objects.</summary>
	[Serializable]
	public partial class FedExEndOfDayCloseEntityFactory : EntityFactoryBase2<FedExEndOfDayCloseEntity> {
		/// <summary>CTor</summary>
		public FedExEndOfDayCloseEntityFactory() : base("FedExEndOfDayCloseEntity", ShipWorks.Data.Model.EntityType.FedExEndOfDayCloseEntity, false) { }
		
		/// <summary>Creates a new FedExEndOfDayCloseEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FedExEndOfDayCloseEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExEndOfDayCloseUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty FedExPackageEntity objects.</summary>
	[Serializable]
	public partial class FedExPackageEntityFactory : EntityFactoryBase2<FedExPackageEntity> {
		/// <summary>CTor</summary>
		public FedExPackageEntityFactory() : base("FedExPackageEntity", ShipWorks.Data.Model.EntityType.FedExPackageEntity, false) { }
		
		/// <summary>Creates a new FedExPackageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FedExPackageEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExPackageUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty FedExProfileEntity objects.</summary>
	[Serializable]
	public partial class FedExProfileEntityFactory : EntityFactoryBase2<FedExProfileEntity> {
		/// <summary>CTor</summary>
		public FedExProfileEntityFactory() : base("FedExProfileEntity", ShipWorks.Data.Model.EntityType.FedExProfileEntity, false) { }
		
		/// <summary>Creates a new FedExProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FedExProfileEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExProfileUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty FedExProfilePackageEntity objects.</summary>
	[Serializable]
	public partial class FedExProfilePackageEntityFactory : EntityFactoryBase2<FedExProfilePackageEntity> {
		/// <summary>CTor</summary>
		public FedExProfilePackageEntityFactory() : base("FedExProfilePackageEntity", ShipWorks.Data.Model.EntityType.FedExProfilePackageEntity, false) { }
		
		/// <summary>Creates a new FedExProfilePackageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FedExProfilePackageEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExProfilePackageUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty FedExShipmentEntity objects.</summary>
	[Serializable]
	public partial class FedExShipmentEntityFactory : EntityFactoryBase2<FedExShipmentEntity> {
		/// <summary>CTor</summary>
		public FedExShipmentEntityFactory() : base("FedExShipmentEntity", ShipWorks.Data.Model.EntityType.FedExShipmentEntity, false) { }
		
		/// <summary>Creates a new FedExShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FedExShipmentEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExShipmentUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty FilterEntity objects.</summary>
	[Serializable]
	public partial class FilterEntityFactory : EntityFactoryBase2<FilterEntity> {
		/// <summary>CTor</summary>
		public FilterEntityFactory() : base("FilterEntity", ShipWorks.Data.Model.EntityType.FilterEntity, false) { }
		
		/// <summary>Creates a new FilterEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FilterEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty FilterLayoutEntity objects.</summary>
	[Serializable]
	public partial class FilterLayoutEntityFactory : EntityFactoryBase2<FilterLayoutEntity> {
		/// <summary>CTor</summary>
		public FilterLayoutEntityFactory() : base("FilterLayoutEntity", ShipWorks.Data.Model.EntityType.FilterLayoutEntity, false) { }
		
		/// <summary>Creates a new FilterLayoutEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FilterLayoutEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterLayoutUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty FilterNodeEntity objects.</summary>
	[Serializable]
	public partial class FilterNodeEntityFactory : EntityFactoryBase2<FilterNodeEntity> {
		/// <summary>CTor</summary>
		public FilterNodeEntityFactory() : base("FilterNodeEntity", ShipWorks.Data.Model.EntityType.FilterNodeEntity, false) { }
		
		/// <summary>Creates a new FilterNodeEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FilterNodeEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterNodeUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty FilterNodeColumnSettingsEntity objects.</summary>
	[Serializable]
	public partial class FilterNodeColumnSettingsEntityFactory : EntityFactoryBase2<FilterNodeColumnSettingsEntity> {
		/// <summary>CTor</summary>
		public FilterNodeColumnSettingsEntityFactory() : base("FilterNodeColumnSettingsEntity", ShipWorks.Data.Model.EntityType.FilterNodeColumnSettingsEntity, false) { }
		
		/// <summary>Creates a new FilterNodeColumnSettingsEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FilterNodeColumnSettingsEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterNodeColumnSettingsUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty FilterNodeContentEntity objects.</summary>
	[Serializable]
	public partial class FilterNodeContentEntityFactory : EntityFactoryBase2<FilterNodeContentEntity> {
		/// <summary>CTor</summary>
		public FilterNodeContentEntityFactory() : base("FilterNodeContentEntity", ShipWorks.Data.Model.EntityType.FilterNodeContentEntity, false) { }
		
		/// <summary>Creates a new FilterNodeContentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FilterNodeContentEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterNodeContentUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty FilterNodeContentDetailEntity objects.</summary>
	[Serializable]
	public partial class FilterNodeContentDetailEntityFactory : EntityFactoryBase2<FilterNodeContentDetailEntity> {
		/// <summary>CTor</summary>
		public FilterNodeContentDetailEntityFactory() : base("FilterNodeContentDetailEntity", ShipWorks.Data.Model.EntityType.FilterNodeContentDetailEntity, false) { }
		
		/// <summary>Creates a new FilterNodeContentDetailEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FilterNodeContentDetailEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterNodeContentDetailUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty FilterSequenceEntity objects.</summary>
	[Serializable]
	public partial class FilterSequenceEntityFactory : EntityFactoryBase2<FilterSequenceEntity> {
		/// <summary>CTor</summary>
		public FilterSequenceEntityFactory() : base("FilterSequenceEntity", ShipWorks.Data.Model.EntityType.FilterSequenceEntity, false) { }
		
		/// <summary>Creates a new FilterSequenceEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FilterSequenceEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterSequenceUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty FtpAccountEntity objects.</summary>
	[Serializable]
	public partial class FtpAccountEntityFactory : EntityFactoryBase2<FtpAccountEntity> {
		/// <summary>CTor</summary>
		public FtpAccountEntityFactory() : base("FtpAccountEntity", ShipWorks.Data.Model.EntityType.FtpAccountEntity, false) { }
		
		/// <summary>Creates a new FtpAccountEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FtpAccountEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewFtpAccountUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty GenericFileStoreEntity objects.</summary>
	[Serializable]
	public partial class GenericFileStoreEntityFactory : EntityFactoryBase2<GenericFileStoreEntity> {
		/// <summary>CTor</summary>
		public GenericFileStoreEntityFactory() : base("GenericFileStoreEntity", ShipWorks.Data.Model.EntityType.GenericFileStoreEntity, true) { }
		
		/// <summary>Creates a new GenericFileStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GenericFileStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewGenericFileStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty GenericModuleStoreEntity objects.</summary>
	[Serializable]
	public partial class GenericModuleStoreEntityFactory : EntityFactoryBase2<GenericModuleStoreEntity> {
		/// <summary>CTor</summary>
		public GenericModuleStoreEntityFactory() : base("GenericModuleStoreEntity", ShipWorks.Data.Model.EntityType.GenericModuleStoreEntity, true) { }
		
		/// <summary>Creates a new GenericModuleStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GenericModuleStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewGenericModuleStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty GridColumnFormatEntity objects.</summary>
	[Serializable]
	public partial class GridColumnFormatEntityFactory : EntityFactoryBase2<GridColumnFormatEntity> {
		/// <summary>CTor</summary>
		public GridColumnFormatEntityFactory() : base("GridColumnFormatEntity", ShipWorks.Data.Model.EntityType.GridColumnFormatEntity, false) { }
		
		/// <summary>Creates a new GridColumnFormatEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GridColumnFormatEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewGridColumnFormatUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty GridColumnLayoutEntity objects.</summary>
	[Serializable]
	public partial class GridColumnLayoutEntityFactory : EntityFactoryBase2<GridColumnLayoutEntity> {
		/// <summary>CTor</summary>
		public GridColumnLayoutEntityFactory() : base("GridColumnLayoutEntity", ShipWorks.Data.Model.EntityType.GridColumnLayoutEntity, false) { }
		
		/// <summary>Creates a new GridColumnLayoutEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GridColumnLayoutEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewGridColumnLayoutUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty GridColumnPositionEntity objects.</summary>
	[Serializable]
	public partial class GridColumnPositionEntityFactory : EntityFactoryBase2<GridColumnPositionEntity> {
		/// <summary>CTor</summary>
		public GridColumnPositionEntityFactory() : base("GridColumnPositionEntity", ShipWorks.Data.Model.EntityType.GridColumnPositionEntity, false) { }
		
		/// <summary>Creates a new GridColumnPositionEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GridColumnPositionEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewGridColumnPositionUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty GrouponOrderEntity objects.</summary>
	[Serializable]
	public partial class GrouponOrderEntityFactory : EntityFactoryBase2<GrouponOrderEntity> {
		/// <summary>CTor</summary>
		public GrouponOrderEntityFactory() : base("GrouponOrderEntity", ShipWorks.Data.Model.EntityType.GrouponOrderEntity, true) { }
		
		/// <summary>Creates a new GrouponOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GrouponOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewGrouponOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty GrouponOrderItemEntity objects.</summary>
	[Serializable]
	public partial class GrouponOrderItemEntityFactory : EntityFactoryBase2<GrouponOrderItemEntity> {
		/// <summary>CTor</summary>
		public GrouponOrderItemEntityFactory() : base("GrouponOrderItemEntity", ShipWorks.Data.Model.EntityType.GrouponOrderItemEntity, true) { }
		
		/// <summary>Creates a new GrouponOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GrouponOrderItemEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewGrouponOrderItemUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty GrouponOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class GrouponOrderSearchEntityFactory : EntityFactoryBase2<GrouponOrderSearchEntity> {
		/// <summary>CTor</summary>
		public GrouponOrderSearchEntityFactory() : base("GrouponOrderSearchEntity", ShipWorks.Data.Model.EntityType.GrouponOrderSearchEntity, false) { }
		
		/// <summary>Creates a new GrouponOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GrouponOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGrouponOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty GrouponStoreEntity objects.</summary>
	[Serializable]
	public partial class GrouponStoreEntityFactory : EntityFactoryBase2<GrouponStoreEntity> {
		/// <summary>CTor</summary>
		public GrouponStoreEntityFactory() : base("GrouponStoreEntity", ShipWorks.Data.Model.EntityType.GrouponStoreEntity, true) { }
		
		/// <summary>Creates a new GrouponStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GrouponStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewGrouponStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty InfopiaOrderItemEntity objects.</summary>
	[Serializable]
	public partial class InfopiaOrderItemEntityFactory : EntityFactoryBase2<InfopiaOrderItemEntity> {
		/// <summary>CTor</summary>
		public InfopiaOrderItemEntityFactory() : base("InfopiaOrderItemEntity", ShipWorks.Data.Model.EntityType.InfopiaOrderItemEntity, true) { }
		
		/// <summary>Creates a new InfopiaOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new InfopiaOrderItemEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewInfopiaOrderItemUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty InfopiaStoreEntity objects.</summary>
	[Serializable]
	public partial class InfopiaStoreEntityFactory : EntityFactoryBase2<InfopiaStoreEntity> {
		/// <summary>CTor</summary>
		public InfopiaStoreEntityFactory() : base("InfopiaStoreEntity", ShipWorks.Data.Model.EntityType.InfopiaStoreEntity, true) { }
		
		/// <summary>Creates a new InfopiaStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new InfopiaStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewInfopiaStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty InsurancePolicyEntity objects.</summary>
	[Serializable]
	public partial class InsurancePolicyEntityFactory : EntityFactoryBase2<InsurancePolicyEntity> {
		/// <summary>CTor</summary>
		public InsurancePolicyEntityFactory() : base("InsurancePolicyEntity", ShipWorks.Data.Model.EntityType.InsurancePolicyEntity, false) { }
		
		/// <summary>Creates a new InsurancePolicyEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new InsurancePolicyEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewInsurancePolicyUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty IParcelAccountEntity objects.</summary>
	[Serializable]
	public partial class IParcelAccountEntityFactory : EntityFactoryBase2<IParcelAccountEntity> {
		/// <summary>CTor</summary>
		public IParcelAccountEntityFactory() : base("IParcelAccountEntity", ShipWorks.Data.Model.EntityType.IParcelAccountEntity, false) { }
		
		/// <summary>Creates a new IParcelAccountEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new IParcelAccountEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewIParcelAccountUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty IParcelPackageEntity objects.</summary>
	[Serializable]
	public partial class IParcelPackageEntityFactory : EntityFactoryBase2<IParcelPackageEntity> {
		/// <summary>CTor</summary>
		public IParcelPackageEntityFactory() : base("IParcelPackageEntity", ShipWorks.Data.Model.EntityType.IParcelPackageEntity, false) { }
		
		/// <summary>Creates a new IParcelPackageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new IParcelPackageEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewIParcelPackageUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty IParcelProfileEntity objects.</summary>
	[Serializable]
	public partial class IParcelProfileEntityFactory : EntityFactoryBase2<IParcelProfileEntity> {
		/// <summary>CTor</summary>
		public IParcelProfileEntityFactory() : base("IParcelProfileEntity", ShipWorks.Data.Model.EntityType.IParcelProfileEntity, false) { }
		
		/// <summary>Creates a new IParcelProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new IParcelProfileEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewIParcelProfileUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty IParcelProfilePackageEntity objects.</summary>
	[Serializable]
	public partial class IParcelProfilePackageEntityFactory : EntityFactoryBase2<IParcelProfilePackageEntity> {
		/// <summary>CTor</summary>
		public IParcelProfilePackageEntityFactory() : base("IParcelProfilePackageEntity", ShipWorks.Data.Model.EntityType.IParcelProfilePackageEntity, false) { }
		
		/// <summary>Creates a new IParcelProfilePackageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new IParcelProfilePackageEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewIParcelProfilePackageUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty IParcelShipmentEntity objects.</summary>
	[Serializable]
	public partial class IParcelShipmentEntityFactory : EntityFactoryBase2<IParcelShipmentEntity> {
		/// <summary>CTor</summary>
		public IParcelShipmentEntityFactory() : base("IParcelShipmentEntity", ShipWorks.Data.Model.EntityType.IParcelShipmentEntity, false) { }
		
		/// <summary>Creates a new IParcelShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new IParcelShipmentEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewIParcelShipmentUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty LabelSheetEntity objects.</summary>
	[Serializable]
	public partial class LabelSheetEntityFactory : EntityFactoryBase2<LabelSheetEntity> {
		/// <summary>CTor</summary>
		public LabelSheetEntityFactory() : base("LabelSheetEntity", ShipWorks.Data.Model.EntityType.LabelSheetEntity, false) { }
		
		/// <summary>Creates a new LabelSheetEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new LabelSheetEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewLabelSheetUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty LemonStandOrderEntity objects.</summary>
	[Serializable]
	public partial class LemonStandOrderEntityFactory : EntityFactoryBase2<LemonStandOrderEntity> {
		/// <summary>CTor</summary>
		public LemonStandOrderEntityFactory() : base("LemonStandOrderEntity", ShipWorks.Data.Model.EntityType.LemonStandOrderEntity, true) { }
		
		/// <summary>Creates a new LemonStandOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new LemonStandOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewLemonStandOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty LemonStandOrderItemEntity objects.</summary>
	[Serializable]
	public partial class LemonStandOrderItemEntityFactory : EntityFactoryBase2<LemonStandOrderItemEntity> {
		/// <summary>CTor</summary>
		public LemonStandOrderItemEntityFactory() : base("LemonStandOrderItemEntity", ShipWorks.Data.Model.EntityType.LemonStandOrderItemEntity, true) { }
		
		/// <summary>Creates a new LemonStandOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new LemonStandOrderItemEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewLemonStandOrderItemUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty LemonStandOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class LemonStandOrderSearchEntityFactory : EntityFactoryBase2<LemonStandOrderSearchEntity> {
		/// <summary>CTor</summary>
		public LemonStandOrderSearchEntityFactory() : base("LemonStandOrderSearchEntity", ShipWorks.Data.Model.EntityType.LemonStandOrderSearchEntity, false) { }
		
		/// <summary>Creates a new LemonStandOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new LemonStandOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewLemonStandOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty LemonStandStoreEntity objects.</summary>
	[Serializable]
	public partial class LemonStandStoreEntityFactory : EntityFactoryBase2<LemonStandStoreEntity> {
		/// <summary>CTor</summary>
		public LemonStandStoreEntityFactory() : base("LemonStandStoreEntity", ShipWorks.Data.Model.EntityType.LemonStandStoreEntity, true) { }
		
		/// <summary>Creates a new LemonStandStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new LemonStandStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewLemonStandStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty MagentoOrderEntity objects.</summary>
	[Serializable]
	public partial class MagentoOrderEntityFactory : EntityFactoryBase2<MagentoOrderEntity> {
		/// <summary>CTor</summary>
		public MagentoOrderEntityFactory() : base("MagentoOrderEntity", ShipWorks.Data.Model.EntityType.MagentoOrderEntity, true) { }
		
		/// <summary>Creates a new MagentoOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new MagentoOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewMagentoOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty MagentoOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class MagentoOrderSearchEntityFactory : EntityFactoryBase2<MagentoOrderSearchEntity> {
		/// <summary>CTor</summary>
		public MagentoOrderSearchEntityFactory() : base("MagentoOrderSearchEntity", ShipWorks.Data.Model.EntityType.MagentoOrderSearchEntity, false) { }
		
		/// <summary>Creates a new MagentoOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new MagentoOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewMagentoOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty MagentoStoreEntity objects.</summary>
	[Serializable]
	public partial class MagentoStoreEntityFactory : EntityFactoryBase2<MagentoStoreEntity> {
		/// <summary>CTor</summary>
		public MagentoStoreEntityFactory() : base("MagentoStoreEntity", ShipWorks.Data.Model.EntityType.MagentoStoreEntity, true) { }
		
		/// <summary>Creates a new MagentoStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new MagentoStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewMagentoStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty MarketplaceAdvisorOrderEntity objects.</summary>
	[Serializable]
	public partial class MarketplaceAdvisorOrderEntityFactory : EntityFactoryBase2<MarketplaceAdvisorOrderEntity> {
		/// <summary>CTor</summary>
		public MarketplaceAdvisorOrderEntityFactory() : base("MarketplaceAdvisorOrderEntity", ShipWorks.Data.Model.EntityType.MarketplaceAdvisorOrderEntity, true) { }
		
		/// <summary>Creates a new MarketplaceAdvisorOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new MarketplaceAdvisorOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewMarketplaceAdvisorOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty MarketplaceAdvisorOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class MarketplaceAdvisorOrderSearchEntityFactory : EntityFactoryBase2<MarketplaceAdvisorOrderSearchEntity> {
		/// <summary>CTor</summary>
		public MarketplaceAdvisorOrderSearchEntityFactory() : base("MarketplaceAdvisorOrderSearchEntity", ShipWorks.Data.Model.EntityType.MarketplaceAdvisorOrderSearchEntity, false) { }
		
		/// <summary>Creates a new MarketplaceAdvisorOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new MarketplaceAdvisorOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewMarketplaceAdvisorOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty MarketplaceAdvisorStoreEntity objects.</summary>
	[Serializable]
	public partial class MarketplaceAdvisorStoreEntityFactory : EntityFactoryBase2<MarketplaceAdvisorStoreEntity> {
		/// <summary>CTor</summary>
		public MarketplaceAdvisorStoreEntityFactory() : base("MarketplaceAdvisorStoreEntity", ShipWorks.Data.Model.EntityType.MarketplaceAdvisorStoreEntity, true) { }
		
		/// <summary>Creates a new MarketplaceAdvisorStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new MarketplaceAdvisorStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewMarketplaceAdvisorStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty MivaOrderItemAttributeEntity objects.</summary>
	[Serializable]
	public partial class MivaOrderItemAttributeEntityFactory : EntityFactoryBase2<MivaOrderItemAttributeEntity> {
		/// <summary>CTor</summary>
		public MivaOrderItemAttributeEntityFactory() : base("MivaOrderItemAttributeEntity", ShipWorks.Data.Model.EntityType.MivaOrderItemAttributeEntity, true) { }
		
		/// <summary>Creates a new MivaOrderItemAttributeEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new MivaOrderItemAttributeEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewMivaOrderItemAttributeUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty MivaStoreEntity objects.</summary>
	[Serializable]
	public partial class MivaStoreEntityFactory : EntityFactoryBase2<MivaStoreEntity> {
		/// <summary>CTor</summary>
		public MivaStoreEntityFactory() : base("MivaStoreEntity", ShipWorks.Data.Model.EntityType.MivaStoreEntity, true) { }
		
		/// <summary>Creates a new MivaStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new MivaStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewMivaStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty NetworkSolutionsOrderEntity objects.</summary>
	[Serializable]
	public partial class NetworkSolutionsOrderEntityFactory : EntityFactoryBase2<NetworkSolutionsOrderEntity> {
		/// <summary>CTor</summary>
		public NetworkSolutionsOrderEntityFactory() : base("NetworkSolutionsOrderEntity", ShipWorks.Data.Model.EntityType.NetworkSolutionsOrderEntity, true) { }
		
		/// <summary>Creates a new NetworkSolutionsOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new NetworkSolutionsOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewNetworkSolutionsOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty NetworkSolutionsOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class NetworkSolutionsOrderSearchEntityFactory : EntityFactoryBase2<NetworkSolutionsOrderSearchEntity> {
		/// <summary>CTor</summary>
		public NetworkSolutionsOrderSearchEntityFactory() : base("NetworkSolutionsOrderSearchEntity", ShipWorks.Data.Model.EntityType.NetworkSolutionsOrderSearchEntity, false) { }
		
		/// <summary>Creates a new NetworkSolutionsOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new NetworkSolutionsOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewNetworkSolutionsOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty NetworkSolutionsStoreEntity objects.</summary>
	[Serializable]
	public partial class NetworkSolutionsStoreEntityFactory : EntityFactoryBase2<NetworkSolutionsStoreEntity> {
		/// <summary>CTor</summary>
		public NetworkSolutionsStoreEntityFactory() : base("NetworkSolutionsStoreEntity", ShipWorks.Data.Model.EntityType.NetworkSolutionsStoreEntity, true) { }
		
		/// <summary>Creates a new NetworkSolutionsStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new NetworkSolutionsStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewNetworkSolutionsStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty NeweggOrderEntity objects.</summary>
	[Serializable]
	public partial class NeweggOrderEntityFactory : EntityFactoryBase2<NeweggOrderEntity> {
		/// <summary>CTor</summary>
		public NeweggOrderEntityFactory() : base("NeweggOrderEntity", ShipWorks.Data.Model.EntityType.NeweggOrderEntity, true) { }
		
		/// <summary>Creates a new NeweggOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new NeweggOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewNeweggOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty NeweggOrderItemEntity objects.</summary>
	[Serializable]
	public partial class NeweggOrderItemEntityFactory : EntityFactoryBase2<NeweggOrderItemEntity> {
		/// <summary>CTor</summary>
		public NeweggOrderItemEntityFactory() : base("NeweggOrderItemEntity", ShipWorks.Data.Model.EntityType.NeweggOrderItemEntity, true) { }
		
		/// <summary>Creates a new NeweggOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new NeweggOrderItemEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewNeweggOrderItemUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty NeweggStoreEntity objects.</summary>
	[Serializable]
	public partial class NeweggStoreEntityFactory : EntityFactoryBase2<NeweggStoreEntity> {
		/// <summary>CTor</summary>
		public NeweggStoreEntityFactory() : base("NeweggStoreEntity", ShipWorks.Data.Model.EntityType.NeweggStoreEntity, true) { }
		
		/// <summary>Creates a new NeweggStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new NeweggStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewNeweggStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty NoteEntity objects.</summary>
	[Serializable]
	public partial class NoteEntityFactory : EntityFactoryBase2<NoteEntity> {
		/// <summary>CTor</summary>
		public NoteEntityFactory() : base("NoteEntity", ShipWorks.Data.Model.EntityType.NoteEntity, false) { }
		
		/// <summary>Creates a new NoteEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new NoteEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewNoteUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ObjectLabelEntity objects.</summary>
	[Serializable]
	public partial class ObjectLabelEntityFactory : EntityFactoryBase2<ObjectLabelEntity> {
		/// <summary>CTor</summary>
		public ObjectLabelEntityFactory() : base("ObjectLabelEntity", ShipWorks.Data.Model.EntityType.ObjectLabelEntity, false) { }
		
		/// <summary>Creates a new ObjectLabelEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ObjectLabelEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewObjectLabelUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ObjectReferenceEntity objects.</summary>
	[Serializable]
	public partial class ObjectReferenceEntityFactory : EntityFactoryBase2<ObjectReferenceEntity> {
		/// <summary>CTor</summary>
		public ObjectReferenceEntityFactory() : base("ObjectReferenceEntity", ShipWorks.Data.Model.EntityType.ObjectReferenceEntity, false) { }
		
		/// <summary>Creates a new ObjectReferenceEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ObjectReferenceEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewObjectReferenceUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty OdbcStoreEntity objects.</summary>
	[Serializable]
	public partial class OdbcStoreEntityFactory : EntityFactoryBase2<OdbcStoreEntity> {
		/// <summary>CTor</summary>
		public OdbcStoreEntityFactory() : base("OdbcStoreEntity", ShipWorks.Data.Model.EntityType.OdbcStoreEntity, true) { }
		
		/// <summary>Creates a new OdbcStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OdbcStoreEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOdbcStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty OnTracAccountEntity objects.</summary>
	[Serializable]
	public partial class OnTracAccountEntityFactory : EntityFactoryBase2<OnTracAccountEntity> {
		/// <summary>CTor</summary>
		public OnTracAccountEntityFactory() : base("OnTracAccountEntity", ShipWorks.Data.Model.EntityType.OnTracAccountEntity, false) { }
		
		/// <summary>Creates a new OnTracAccountEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OnTracAccountEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewOnTracAccountUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty OnTracProfileEntity objects.</summary>
	[Serializable]
	public partial class OnTracProfileEntityFactory : EntityFactoryBase2<OnTracProfileEntity> {
		/// <summary>CTor</summary>
		public OnTracProfileEntityFactory() : base("OnTracProfileEntity", ShipWorks.Data.Model.EntityType.OnTracProfileEntity, false) { }
		
		/// <summary>Creates a new OnTracProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OnTracProfileEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewOnTracProfileUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty OnTracShipmentEntity objects.</summary>
	[Serializable]
	public partial class OnTracShipmentEntityFactory : EntityFactoryBase2<OnTracShipmentEntity> {
		/// <summary>CTor</summary>
		public OnTracShipmentEntityFactory() : base("OnTracShipmentEntity", ShipWorks.Data.Model.EntityType.OnTracShipmentEntity, false) { }
		
		/// <summary>Creates a new OnTracShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OnTracShipmentEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewOnTracShipmentUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty OrderEntity objects.</summary>
	[Serializable]
	public partial class OrderEntityFactory : EntityFactoryBase2<OrderEntity> {
		/// <summary>CTor</summary>
		public OrderEntityFactory() : base("OrderEntity", ShipWorks.Data.Model.EntityType.OrderEntity, true) { }
		
		/// <summary>Creates a new OrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty OrderChargeEntity objects.</summary>
	[Serializable]
	public partial class OrderChargeEntityFactory : EntityFactoryBase2<OrderChargeEntity> {
		/// <summary>CTor</summary>
		public OrderChargeEntityFactory() : base("OrderChargeEntity", ShipWorks.Data.Model.EntityType.OrderChargeEntity, false) { }
		
		/// <summary>Creates a new OrderChargeEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OrderChargeEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderChargeUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty OrderItemEntity objects.</summary>
	[Serializable]
	public partial class OrderItemEntityFactory : EntityFactoryBase2<OrderItemEntity> {
		/// <summary>CTor</summary>
		public OrderItemEntityFactory() : base("OrderItemEntity", ShipWorks.Data.Model.EntityType.OrderItemEntity, true) { }
		
		/// <summary>Creates a new OrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OrderItemEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderItemUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty OrderItemAttributeEntity objects.</summary>
	[Serializable]
	public partial class OrderItemAttributeEntityFactory : EntityFactoryBase2<OrderItemAttributeEntity> {
		/// <summary>CTor</summary>
		public OrderItemAttributeEntityFactory() : base("OrderItemAttributeEntity", ShipWorks.Data.Model.EntityType.OrderItemAttributeEntity, true) { }
		
		/// <summary>Creates a new OrderItemAttributeEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OrderItemAttributeEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderItemAttributeUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty OrderMotionOrderEntity objects.</summary>
	[Serializable]
	public partial class OrderMotionOrderEntityFactory : EntityFactoryBase2<OrderMotionOrderEntity> {
		/// <summary>CTor</summary>
		public OrderMotionOrderEntityFactory() : base("OrderMotionOrderEntity", ShipWorks.Data.Model.EntityType.OrderMotionOrderEntity, true) { }
		
		/// <summary>Creates a new OrderMotionOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OrderMotionOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderMotionOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty OrderMotionOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class OrderMotionOrderSearchEntityFactory : EntityFactoryBase2<OrderMotionOrderSearchEntity> {
		/// <summary>CTor</summary>
		public OrderMotionOrderSearchEntityFactory() : base("OrderMotionOrderSearchEntity", ShipWorks.Data.Model.EntityType.OrderMotionOrderSearchEntity, false) { }
		
		/// <summary>Creates a new OrderMotionOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OrderMotionOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderMotionOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty OrderMotionStoreEntity objects.</summary>
	[Serializable]
	public partial class OrderMotionStoreEntityFactory : EntityFactoryBase2<OrderMotionStoreEntity> {
		/// <summary>CTor</summary>
		public OrderMotionStoreEntityFactory() : base("OrderMotionStoreEntity", ShipWorks.Data.Model.EntityType.OrderMotionStoreEntity, true) { }
		
		/// <summary>Creates a new OrderMotionStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OrderMotionStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderMotionStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty OrderPaymentDetailEntity objects.</summary>
	[Serializable]
	public partial class OrderPaymentDetailEntityFactory : EntityFactoryBase2<OrderPaymentDetailEntity> {
		/// <summary>CTor</summary>
		public OrderPaymentDetailEntityFactory() : base("OrderPaymentDetailEntity", ShipWorks.Data.Model.EntityType.OrderPaymentDetailEntity, false) { }
		
		/// <summary>Creates a new OrderPaymentDetailEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OrderPaymentDetailEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderPaymentDetailUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty OrderSearchEntity objects.</summary>
	[Serializable]
	public partial class OrderSearchEntityFactory : EntityFactoryBase2<OrderSearchEntity> {
		/// <summary>CTor</summary>
		public OrderSearchEntityFactory() : base("OrderSearchEntity", ShipWorks.Data.Model.EntityType.OrderSearchEntity, false) { }
		
		/// <summary>Creates a new OrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty OtherProfileEntity objects.</summary>
	[Serializable]
	public partial class OtherProfileEntityFactory : EntityFactoryBase2<OtherProfileEntity> {
		/// <summary>CTor</summary>
		public OtherProfileEntityFactory() : base("OtherProfileEntity", ShipWorks.Data.Model.EntityType.OtherProfileEntity, false) { }
		
		/// <summary>Creates a new OtherProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OtherProfileEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewOtherProfileUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty OtherShipmentEntity objects.</summary>
	[Serializable]
	public partial class OtherShipmentEntityFactory : EntityFactoryBase2<OtherShipmentEntity> {
		/// <summary>CTor</summary>
		public OtherShipmentEntityFactory() : base("OtherShipmentEntity", ShipWorks.Data.Model.EntityType.OtherShipmentEntity, false) { }
		
		/// <summary>Creates a new OtherShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OtherShipmentEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewOtherShipmentUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty PayPalOrderEntity objects.</summary>
	[Serializable]
	public partial class PayPalOrderEntityFactory : EntityFactoryBase2<PayPalOrderEntity> {
		/// <summary>CTor</summary>
		public PayPalOrderEntityFactory() : base("PayPalOrderEntity", ShipWorks.Data.Model.EntityType.PayPalOrderEntity, true) { }
		
		/// <summary>Creates a new PayPalOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new PayPalOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewPayPalOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty PayPalOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class PayPalOrderSearchEntityFactory : EntityFactoryBase2<PayPalOrderSearchEntity> {
		/// <summary>CTor</summary>
		public PayPalOrderSearchEntityFactory() : base("PayPalOrderSearchEntity", ShipWorks.Data.Model.EntityType.PayPalOrderSearchEntity, false) { }
		
		/// <summary>Creates a new PayPalOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new PayPalOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewPayPalOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty PayPalStoreEntity objects.</summary>
	[Serializable]
	public partial class PayPalStoreEntityFactory : EntityFactoryBase2<PayPalStoreEntity> {
		/// <summary>CTor</summary>
		public PayPalStoreEntityFactory() : base("PayPalStoreEntity", ShipWorks.Data.Model.EntityType.PayPalStoreEntity, true) { }
		
		/// <summary>Creates a new PayPalStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new PayPalStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewPayPalStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty PermissionEntity objects.</summary>
	[Serializable]
	public partial class PermissionEntityFactory : EntityFactoryBase2<PermissionEntity> {
		/// <summary>CTor</summary>
		public PermissionEntityFactory() : base("PermissionEntity", ShipWorks.Data.Model.EntityType.PermissionEntity, false) { }
		
		/// <summary>Creates a new PermissionEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new PermissionEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewPermissionUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty PostalProfileEntity objects.</summary>
	[Serializable]
	public partial class PostalProfileEntityFactory : EntityFactoryBase2<PostalProfileEntity> {
		/// <summary>CTor</summary>
		public PostalProfileEntityFactory() : base("PostalProfileEntity", ShipWorks.Data.Model.EntityType.PostalProfileEntity, false) { }
		
		/// <summary>Creates a new PostalProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new PostalProfileEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewPostalProfileUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty PostalShipmentEntity objects.</summary>
	[Serializable]
	public partial class PostalShipmentEntityFactory : EntityFactoryBase2<PostalShipmentEntity> {
		/// <summary>CTor</summary>
		public PostalShipmentEntityFactory() : base("PostalShipmentEntity", ShipWorks.Data.Model.EntityType.PostalShipmentEntity, false) { }
		
		/// <summary>Creates a new PostalShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new PostalShipmentEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewPostalShipmentUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty PrintResultEntity objects.</summary>
	[Serializable]
	public partial class PrintResultEntityFactory : EntityFactoryBase2<PrintResultEntity> {
		/// <summary>CTor</summary>
		public PrintResultEntityFactory() : base("PrintResultEntity", ShipWorks.Data.Model.EntityType.PrintResultEntity, false) { }
		
		/// <summary>Creates a new PrintResultEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new PrintResultEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewPrintResultUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ProStoresOrderEntity objects.</summary>
	[Serializable]
	public partial class ProStoresOrderEntityFactory : EntityFactoryBase2<ProStoresOrderEntity> {
		/// <summary>CTor</summary>
		public ProStoresOrderEntityFactory() : base("ProStoresOrderEntity", ShipWorks.Data.Model.EntityType.ProStoresOrderEntity, true) { }
		
		/// <summary>Creates a new ProStoresOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ProStoresOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewProStoresOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ProStoresOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class ProStoresOrderSearchEntityFactory : EntityFactoryBase2<ProStoresOrderSearchEntity> {
		/// <summary>CTor</summary>
		public ProStoresOrderSearchEntityFactory() : base("ProStoresOrderSearchEntity", ShipWorks.Data.Model.EntityType.ProStoresOrderSearchEntity, false) { }
		
		/// <summary>Creates a new ProStoresOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ProStoresOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewProStoresOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ProStoresStoreEntity objects.</summary>
	[Serializable]
	public partial class ProStoresStoreEntityFactory : EntityFactoryBase2<ProStoresStoreEntity> {
		/// <summary>CTor</summary>
		public ProStoresStoreEntityFactory() : base("ProStoresStoreEntity", ShipWorks.Data.Model.EntityType.ProStoresStoreEntity, true) { }
		
		/// <summary>Creates a new ProStoresStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ProStoresStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewProStoresStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ResourceEntity objects.</summary>
	[Serializable]
	public partial class ResourceEntityFactory : EntityFactoryBase2<ResourceEntity> {
		/// <summary>CTor</summary>
		public ResourceEntityFactory() : base("ResourceEntity", ShipWorks.Data.Model.EntityType.ResourceEntity, false) { }
		
		/// <summary>Creates a new ResourceEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ResourceEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewResourceUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ScanFormBatchEntity objects.</summary>
	[Serializable]
	public partial class ScanFormBatchEntityFactory : EntityFactoryBase2<ScanFormBatchEntity> {
		/// <summary>CTor</summary>
		public ScanFormBatchEntityFactory() : base("ScanFormBatchEntity", ShipWorks.Data.Model.EntityType.ScanFormBatchEntity, false) { }
		
		/// <summary>Creates a new ScanFormBatchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ScanFormBatchEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewScanFormBatchUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty SearchEntity objects.</summary>
	[Serializable]
	public partial class SearchEntityFactory : EntityFactoryBase2<SearchEntity> {
		/// <summary>CTor</summary>
		public SearchEntityFactory() : base("SearchEntity", ShipWorks.Data.Model.EntityType.SearchEntity, false) { }
		
		/// <summary>Creates a new SearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SearchEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewSearchUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty SearsOrderEntity objects.</summary>
	[Serializable]
	public partial class SearsOrderEntityFactory : EntityFactoryBase2<SearsOrderEntity> {
		/// <summary>CTor</summary>
		public SearsOrderEntityFactory() : base("SearsOrderEntity", ShipWorks.Data.Model.EntityType.SearsOrderEntity, true) { }
		
		/// <summary>Creates a new SearsOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SearsOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewSearsOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty SearsOrderItemEntity objects.</summary>
	[Serializable]
	public partial class SearsOrderItemEntityFactory : EntityFactoryBase2<SearsOrderItemEntity> {
		/// <summary>CTor</summary>
		public SearsOrderItemEntityFactory() : base("SearsOrderItemEntity", ShipWorks.Data.Model.EntityType.SearsOrderItemEntity, true) { }
		
		/// <summary>Creates a new SearsOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SearsOrderItemEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewSearsOrderItemUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty SearsOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class SearsOrderSearchEntityFactory : EntityFactoryBase2<SearsOrderSearchEntity> {
		/// <summary>CTor</summary>
		public SearsOrderSearchEntityFactory() : base("SearsOrderSearchEntity", ShipWorks.Data.Model.EntityType.SearsOrderSearchEntity, false) { }
		
		/// <summary>Creates a new SearsOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SearsOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSearsOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty SearsStoreEntity objects.</summary>
	[Serializable]
	public partial class SearsStoreEntityFactory : EntityFactoryBase2<SearsStoreEntity> {
		/// <summary>CTor</summary>
		public SearsStoreEntityFactory() : base("SearsStoreEntity", ShipWorks.Data.Model.EntityType.SearsStoreEntity, true) { }
		
		/// <summary>Creates a new SearsStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SearsStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewSearsStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ServerMessageEntity objects.</summary>
	[Serializable]
	public partial class ServerMessageEntityFactory : EntityFactoryBase2<ServerMessageEntity> {
		/// <summary>CTor</summary>
		public ServerMessageEntityFactory() : base("ServerMessageEntity", ShipWorks.Data.Model.EntityType.ServerMessageEntity, false) { }
		
		/// <summary>Creates a new ServerMessageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ServerMessageEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewServerMessageUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ServerMessageSignoffEntity objects.</summary>
	[Serializable]
	public partial class ServerMessageSignoffEntityFactory : EntityFactoryBase2<ServerMessageSignoffEntity> {
		/// <summary>CTor</summary>
		public ServerMessageSignoffEntityFactory() : base("ServerMessageSignoffEntity", ShipWorks.Data.Model.EntityType.ServerMessageSignoffEntity, false) { }
		
		/// <summary>Creates a new ServerMessageSignoffEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ServerMessageSignoffEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewServerMessageSignoffUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ServiceStatusEntity objects.</summary>
	[Serializable]
	public partial class ServiceStatusEntityFactory : EntityFactoryBase2<ServiceStatusEntity> {
		/// <summary>CTor</summary>
		public ServiceStatusEntityFactory() : base("ServiceStatusEntity", ShipWorks.Data.Model.EntityType.ServiceStatusEntity, false) { }
		
		/// <summary>Creates a new ServiceStatusEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ServiceStatusEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewServiceStatusUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ShipmentEntity objects.</summary>
	[Serializable]
	public partial class ShipmentEntityFactory : EntityFactoryBase2<ShipmentEntity> {
		/// <summary>CTor</summary>
		public ShipmentEntityFactory() : base("ShipmentEntity", ShipWorks.Data.Model.EntityType.ShipmentEntity, false) { }
		
		/// <summary>Creates a new ShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShipmentEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewShipmentUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ShipmentCustomsItemEntity objects.</summary>
	[Serializable]
	public partial class ShipmentCustomsItemEntityFactory : EntityFactoryBase2<ShipmentCustomsItemEntity> {
		/// <summary>CTor</summary>
		public ShipmentCustomsItemEntityFactory() : base("ShipmentCustomsItemEntity", ShipWorks.Data.Model.EntityType.ShipmentCustomsItemEntity, false) { }
		
		/// <summary>Creates a new ShipmentCustomsItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShipmentCustomsItemEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewShipmentCustomsItemUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ShippingDefaultsRuleEntity objects.</summary>
	[Serializable]
	public partial class ShippingDefaultsRuleEntityFactory : EntityFactoryBase2<ShippingDefaultsRuleEntity> {
		/// <summary>CTor</summary>
		public ShippingDefaultsRuleEntityFactory() : base("ShippingDefaultsRuleEntity", ShipWorks.Data.Model.EntityType.ShippingDefaultsRuleEntity, false) { }
		
		/// <summary>Creates a new ShippingDefaultsRuleEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShippingDefaultsRuleEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingDefaultsRuleUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ShippingOriginEntity objects.</summary>
	[Serializable]
	public partial class ShippingOriginEntityFactory : EntityFactoryBase2<ShippingOriginEntity> {
		/// <summary>CTor</summary>
		public ShippingOriginEntityFactory() : base("ShippingOriginEntity", ShipWorks.Data.Model.EntityType.ShippingOriginEntity, false) { }
		
		/// <summary>Creates a new ShippingOriginEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShippingOriginEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingOriginUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ShippingPrintOutputEntity objects.</summary>
	[Serializable]
	public partial class ShippingPrintOutputEntityFactory : EntityFactoryBase2<ShippingPrintOutputEntity> {
		/// <summary>CTor</summary>
		public ShippingPrintOutputEntityFactory() : base("ShippingPrintOutputEntity", ShipWorks.Data.Model.EntityType.ShippingPrintOutputEntity, false) { }
		
		/// <summary>Creates a new ShippingPrintOutputEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShippingPrintOutputEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingPrintOutputUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ShippingPrintOutputRuleEntity objects.</summary>
	[Serializable]
	public partial class ShippingPrintOutputRuleEntityFactory : EntityFactoryBase2<ShippingPrintOutputRuleEntity> {
		/// <summary>CTor</summary>
		public ShippingPrintOutputRuleEntityFactory() : base("ShippingPrintOutputRuleEntity", ShipWorks.Data.Model.EntityType.ShippingPrintOutputRuleEntity, false) { }
		
		/// <summary>Creates a new ShippingPrintOutputRuleEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShippingPrintOutputRuleEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingPrintOutputRuleUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ShippingProfileEntity objects.</summary>
	[Serializable]
	public partial class ShippingProfileEntityFactory : EntityFactoryBase2<ShippingProfileEntity> {
		/// <summary>CTor</summary>
		public ShippingProfileEntityFactory() : base("ShippingProfileEntity", ShipWorks.Data.Model.EntityType.ShippingProfileEntity, false) { }
		
		/// <summary>Creates a new ShippingProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShippingProfileEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingProfileUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ShippingProviderRuleEntity objects.</summary>
	[Serializable]
	public partial class ShippingProviderRuleEntityFactory : EntityFactoryBase2<ShippingProviderRuleEntity> {
		/// <summary>CTor</summary>
		public ShippingProviderRuleEntityFactory() : base("ShippingProviderRuleEntity", ShipWorks.Data.Model.EntityType.ShippingProviderRuleEntity, false) { }
		
		/// <summary>Creates a new ShippingProviderRuleEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShippingProviderRuleEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingProviderRuleUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ShippingSettingsEntity objects.</summary>
	[Serializable]
	public partial class ShippingSettingsEntityFactory : EntityFactoryBase2<ShippingSettingsEntity> {
		/// <summary>CTor</summary>
		public ShippingSettingsEntityFactory() : base("ShippingSettingsEntity", ShipWorks.Data.Model.EntityType.ShippingSettingsEntity, false) { }
		
		/// <summary>Creates a new ShippingSettingsEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShippingSettingsEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingSettingsUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ShipSenseKnowledgebaseEntity objects.</summary>
	[Serializable]
	public partial class ShipSenseKnowledgebaseEntityFactory : EntityFactoryBase2<ShipSenseKnowledgebaseEntity> {
		/// <summary>CTor</summary>
		public ShipSenseKnowledgebaseEntityFactory() : base("ShipSenseKnowledgebaseEntity", ShipWorks.Data.Model.EntityType.ShipSenseKnowledgebaseEntity, false) { }
		
		/// <summary>Creates a new ShipSenseKnowledgebaseEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShipSenseKnowledgebaseEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewShipSenseKnowledgebaseUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ShopifyOrderEntity objects.</summary>
	[Serializable]
	public partial class ShopifyOrderEntityFactory : EntityFactoryBase2<ShopifyOrderEntity> {
		/// <summary>CTor</summary>
		public ShopifyOrderEntityFactory() : base("ShopifyOrderEntity", ShipWorks.Data.Model.EntityType.ShopifyOrderEntity, true) { }
		
		/// <summary>Creates a new ShopifyOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShopifyOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewShopifyOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ShopifyOrderItemEntity objects.</summary>
	[Serializable]
	public partial class ShopifyOrderItemEntityFactory : EntityFactoryBase2<ShopifyOrderItemEntity> {
		/// <summary>CTor</summary>
		public ShopifyOrderItemEntityFactory() : base("ShopifyOrderItemEntity", ShipWorks.Data.Model.EntityType.ShopifyOrderItemEntity, true) { }
		
		/// <summary>Creates a new ShopifyOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShopifyOrderItemEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewShopifyOrderItemUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ShopifyOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class ShopifyOrderSearchEntityFactory : EntityFactoryBase2<ShopifyOrderSearchEntity> {
		/// <summary>CTor</summary>
		public ShopifyOrderSearchEntityFactory() : base("ShopifyOrderSearchEntity", ShipWorks.Data.Model.EntityType.ShopifyOrderSearchEntity, false) { }
		
		/// <summary>Creates a new ShopifyOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShopifyOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShopifyOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ShopifyStoreEntity objects.</summary>
	[Serializable]
	public partial class ShopifyStoreEntityFactory : EntityFactoryBase2<ShopifyStoreEntity> {
		/// <summary>CTor</summary>
		public ShopifyStoreEntityFactory() : base("ShopifyStoreEntity", ShipWorks.Data.Model.EntityType.ShopifyStoreEntity, true) { }
		
		/// <summary>Creates a new ShopifyStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShopifyStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewShopifyStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ShopSiteStoreEntity objects.</summary>
	[Serializable]
	public partial class ShopSiteStoreEntityFactory : EntityFactoryBase2<ShopSiteStoreEntity> {
		/// <summary>CTor</summary>
		public ShopSiteStoreEntityFactory() : base("ShopSiteStoreEntity", ShipWorks.Data.Model.EntityType.ShopSiteStoreEntity, true) { }
		
		/// <summary>Creates a new ShopSiteStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShopSiteStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewShopSiteStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty SparkPayStoreEntity objects.</summary>
	[Serializable]
	public partial class SparkPayStoreEntityFactory : EntityFactoryBase2<SparkPayStoreEntity> {
		/// <summary>CTor</summary>
		public SparkPayStoreEntityFactory() : base("SparkPayStoreEntity", ShipWorks.Data.Model.EntityType.SparkPayStoreEntity, true) { }
		
		/// <summary>Creates a new SparkPayStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SparkPayStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewSparkPayStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty StatusPresetEntity objects.</summary>
	[Serializable]
	public partial class StatusPresetEntityFactory : EntityFactoryBase2<StatusPresetEntity> {
		/// <summary>CTor</summary>
		public StatusPresetEntityFactory() : base("StatusPresetEntity", ShipWorks.Data.Model.EntityType.StatusPresetEntity, false) { }
		
		/// <summary>Creates a new StatusPresetEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new StatusPresetEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewStatusPresetUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty StoreEntity objects.</summary>
	[Serializable]
	public partial class StoreEntityFactory : EntityFactoryBase2<StoreEntity> {
		/// <summary>CTor</summary>
		public StoreEntityFactory() : base("StoreEntity", ShipWorks.Data.Model.EntityType.StoreEntity, true) { }
		
		/// <summary>Creates a new StoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new StoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty SystemDataEntity objects.</summary>
	[Serializable]
	public partial class SystemDataEntityFactory : EntityFactoryBase2<SystemDataEntity> {
		/// <summary>CTor</summary>
		public SystemDataEntityFactory() : base("SystemDataEntity", ShipWorks.Data.Model.EntityType.SystemDataEntity, false) { }
		
		/// <summary>Creates a new SystemDataEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SystemDataEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewSystemDataUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty TemplateEntity objects.</summary>
	[Serializable]
	public partial class TemplateEntityFactory : EntityFactoryBase2<TemplateEntity> {
		/// <summary>CTor</summary>
		public TemplateEntityFactory() : base("TemplateEntity", ShipWorks.Data.Model.EntityType.TemplateEntity, false) { }
		
		/// <summary>Creates a new TemplateEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new TemplateEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewTemplateUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty TemplateComputerSettingsEntity objects.</summary>
	[Serializable]
	public partial class TemplateComputerSettingsEntityFactory : EntityFactoryBase2<TemplateComputerSettingsEntity> {
		/// <summary>CTor</summary>
		public TemplateComputerSettingsEntityFactory() : base("TemplateComputerSettingsEntity", ShipWorks.Data.Model.EntityType.TemplateComputerSettingsEntity, false) { }
		
		/// <summary>Creates a new TemplateComputerSettingsEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new TemplateComputerSettingsEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewTemplateComputerSettingsUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty TemplateFolderEntity objects.</summary>
	[Serializable]
	public partial class TemplateFolderEntityFactory : EntityFactoryBase2<TemplateFolderEntity> {
		/// <summary>CTor</summary>
		public TemplateFolderEntityFactory() : base("TemplateFolderEntity", ShipWorks.Data.Model.EntityType.TemplateFolderEntity, false) { }
		
		/// <summary>Creates a new TemplateFolderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new TemplateFolderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewTemplateFolderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty TemplateStoreSettingsEntity objects.</summary>
	[Serializable]
	public partial class TemplateStoreSettingsEntityFactory : EntityFactoryBase2<TemplateStoreSettingsEntity> {
		/// <summary>CTor</summary>
		public TemplateStoreSettingsEntityFactory() : base("TemplateStoreSettingsEntity", ShipWorks.Data.Model.EntityType.TemplateStoreSettingsEntity, false) { }
		
		/// <summary>Creates a new TemplateStoreSettingsEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new TemplateStoreSettingsEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewTemplateStoreSettingsUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty TemplateUserSettingsEntity objects.</summary>
	[Serializable]
	public partial class TemplateUserSettingsEntityFactory : EntityFactoryBase2<TemplateUserSettingsEntity> {
		/// <summary>CTor</summary>
		public TemplateUserSettingsEntityFactory() : base("TemplateUserSettingsEntity", ShipWorks.Data.Model.EntityType.TemplateUserSettingsEntity, false) { }
		
		/// <summary>Creates a new TemplateUserSettingsEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new TemplateUserSettingsEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewTemplateUserSettingsUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ThreeDCartOrderEntity objects.</summary>
	[Serializable]
	public partial class ThreeDCartOrderEntityFactory : EntityFactoryBase2<ThreeDCartOrderEntity> {
		/// <summary>CTor</summary>
		public ThreeDCartOrderEntityFactory() : base("ThreeDCartOrderEntity", ShipWorks.Data.Model.EntityType.ThreeDCartOrderEntity, true) { }
		
		/// <summary>Creates a new ThreeDCartOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ThreeDCartOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewThreeDCartOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ThreeDCartOrderItemEntity objects.</summary>
	[Serializable]
	public partial class ThreeDCartOrderItemEntityFactory : EntityFactoryBase2<ThreeDCartOrderItemEntity> {
		/// <summary>CTor</summary>
		public ThreeDCartOrderItemEntityFactory() : base("ThreeDCartOrderItemEntity", ShipWorks.Data.Model.EntityType.ThreeDCartOrderItemEntity, true) { }
		
		/// <summary>Creates a new ThreeDCartOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ThreeDCartOrderItemEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewThreeDCartOrderItemUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ThreeDCartOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class ThreeDCartOrderSearchEntityFactory : EntityFactoryBase2<ThreeDCartOrderSearchEntity> {
		/// <summary>CTor</summary>
		public ThreeDCartOrderSearchEntityFactory() : base("ThreeDCartOrderSearchEntity", ShipWorks.Data.Model.EntityType.ThreeDCartOrderSearchEntity, false) { }
		
		/// <summary>Creates a new ThreeDCartOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ThreeDCartOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewThreeDCartOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ThreeDCartStoreEntity objects.</summary>
	[Serializable]
	public partial class ThreeDCartStoreEntityFactory : EntityFactoryBase2<ThreeDCartStoreEntity> {
		/// <summary>CTor</summary>
		public ThreeDCartStoreEntityFactory() : base("ThreeDCartStoreEntity", ShipWorks.Data.Model.EntityType.ThreeDCartStoreEntity, true) { }
		
		/// <summary>Creates a new ThreeDCartStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ThreeDCartStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewThreeDCartStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UpsAccountEntity objects.</summary>
	[Serializable]
	public partial class UpsAccountEntityFactory : EntityFactoryBase2<UpsAccountEntity> {
		/// <summary>CTor</summary>
		public UpsAccountEntityFactory() : base("UpsAccountEntity", ShipWorks.Data.Model.EntityType.UpsAccountEntity, false) { }
		
		/// <summary>Creates a new UpsAccountEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsAccountEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsAccountUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UpsLetterRateEntity objects.</summary>
	[Serializable]
	public partial class UpsLetterRateEntityFactory : EntityFactoryBase2<UpsLetterRateEntity> {
		/// <summary>CTor</summary>
		public UpsLetterRateEntityFactory() : base("UpsLetterRateEntity", ShipWorks.Data.Model.EntityType.UpsLetterRateEntity, false) { }
		
		/// <summary>Creates a new UpsLetterRateEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsLetterRateEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsLetterRateUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UpsLocalRatingDeliveryAreaSurchargeEntity objects.</summary>
	[Serializable]
	public partial class UpsLocalRatingDeliveryAreaSurchargeEntityFactory : EntityFactoryBase2<UpsLocalRatingDeliveryAreaSurchargeEntity> {
		/// <summary>CTor</summary>
		public UpsLocalRatingDeliveryAreaSurchargeEntityFactory() : base("UpsLocalRatingDeliveryAreaSurchargeEntity", ShipWorks.Data.Model.EntityType.UpsLocalRatingDeliveryAreaSurchargeEntity, false) { }
		
		/// <summary>Creates a new UpsLocalRatingDeliveryAreaSurchargeEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsLocalRatingDeliveryAreaSurchargeEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsLocalRatingDeliveryAreaSurchargeUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UpsLocalRatingZoneEntity objects.</summary>
	[Serializable]
	public partial class UpsLocalRatingZoneEntityFactory : EntityFactoryBase2<UpsLocalRatingZoneEntity> {
		/// <summary>CTor</summary>
		public UpsLocalRatingZoneEntityFactory() : base("UpsLocalRatingZoneEntity", ShipWorks.Data.Model.EntityType.UpsLocalRatingZoneEntity, false) { }
		
		/// <summary>Creates a new UpsLocalRatingZoneEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsLocalRatingZoneEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsLocalRatingZoneUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UpsLocalRatingZoneFileEntity objects.</summary>
	[Serializable]
	public partial class UpsLocalRatingZoneFileEntityFactory : EntityFactoryBase2<UpsLocalRatingZoneFileEntity> {
		/// <summary>CTor</summary>
		public UpsLocalRatingZoneFileEntityFactory() : base("UpsLocalRatingZoneFileEntity", ShipWorks.Data.Model.EntityType.UpsLocalRatingZoneFileEntity, false) { }
		
		/// <summary>Creates a new UpsLocalRatingZoneFileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsLocalRatingZoneFileEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsLocalRatingZoneFileUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UpsPackageEntity objects.</summary>
	[Serializable]
	public partial class UpsPackageEntityFactory : EntityFactoryBase2<UpsPackageEntity> {
		/// <summary>CTor</summary>
		public UpsPackageEntityFactory() : base("UpsPackageEntity", ShipWorks.Data.Model.EntityType.UpsPackageEntity, false) { }
		
		/// <summary>Creates a new UpsPackageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsPackageEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsPackageUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UpsPackageRateEntity objects.</summary>
	[Serializable]
	public partial class UpsPackageRateEntityFactory : EntityFactoryBase2<UpsPackageRateEntity> {
		/// <summary>CTor</summary>
		public UpsPackageRateEntityFactory() : base("UpsPackageRateEntity", ShipWorks.Data.Model.EntityType.UpsPackageRateEntity, false) { }
		
		/// <summary>Creates a new UpsPackageRateEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsPackageRateEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsPackageRateUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UpsPricePerPoundEntity objects.</summary>
	[Serializable]
	public partial class UpsPricePerPoundEntityFactory : EntityFactoryBase2<UpsPricePerPoundEntity> {
		/// <summary>CTor</summary>
		public UpsPricePerPoundEntityFactory() : base("UpsPricePerPoundEntity", ShipWorks.Data.Model.EntityType.UpsPricePerPoundEntity, false) { }
		
		/// <summary>Creates a new UpsPricePerPoundEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsPricePerPoundEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsPricePerPoundUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UpsProfileEntity objects.</summary>
	[Serializable]
	public partial class UpsProfileEntityFactory : EntityFactoryBase2<UpsProfileEntity> {
		/// <summary>CTor</summary>
		public UpsProfileEntityFactory() : base("UpsProfileEntity", ShipWorks.Data.Model.EntityType.UpsProfileEntity, false) { }
		
		/// <summary>Creates a new UpsProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsProfileEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsProfileUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UpsProfilePackageEntity objects.</summary>
	[Serializable]
	public partial class UpsProfilePackageEntityFactory : EntityFactoryBase2<UpsProfilePackageEntity> {
		/// <summary>CTor</summary>
		public UpsProfilePackageEntityFactory() : base("UpsProfilePackageEntity", ShipWorks.Data.Model.EntityType.UpsProfilePackageEntity, false) { }
		
		/// <summary>Creates a new UpsProfilePackageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsProfilePackageEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsProfilePackageUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UpsRateSurchargeEntity objects.</summary>
	[Serializable]
	public partial class UpsRateSurchargeEntityFactory : EntityFactoryBase2<UpsRateSurchargeEntity> {
		/// <summary>CTor</summary>
		public UpsRateSurchargeEntityFactory() : base("UpsRateSurchargeEntity", ShipWorks.Data.Model.EntityType.UpsRateSurchargeEntity, false) { }
		
		/// <summary>Creates a new UpsRateSurchargeEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsRateSurchargeEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsRateSurchargeUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UpsRateTableEntity objects.</summary>
	[Serializable]
	public partial class UpsRateTableEntityFactory : EntityFactoryBase2<UpsRateTableEntity> {
		/// <summary>CTor</summary>
		public UpsRateTableEntityFactory() : base("UpsRateTableEntity", ShipWorks.Data.Model.EntityType.UpsRateTableEntity, false) { }
		
		/// <summary>Creates a new UpsRateTableEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsRateTableEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsRateTableUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UpsShipmentEntity objects.</summary>
	[Serializable]
	public partial class UpsShipmentEntityFactory : EntityFactoryBase2<UpsShipmentEntity> {
		/// <summary>CTor</summary>
		public UpsShipmentEntityFactory() : base("UpsShipmentEntity", ShipWorks.Data.Model.EntityType.UpsShipmentEntity, false) { }
		
		/// <summary>Creates a new UpsShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsShipmentEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsShipmentUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UserEntity objects.</summary>
	[Serializable]
	public partial class UserEntityFactory : EntityFactoryBase2<UserEntity> {
		/// <summary>CTor</summary>
		public UserEntityFactory() : base("UserEntity", ShipWorks.Data.Model.EntityType.UserEntity, false) { }
		
		/// <summary>Creates a new UserEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UserEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewUserUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UserColumnSettingsEntity objects.</summary>
	[Serializable]
	public partial class UserColumnSettingsEntityFactory : EntityFactoryBase2<UserColumnSettingsEntity> {
		/// <summary>CTor</summary>
		public UserColumnSettingsEntityFactory() : base("UserColumnSettingsEntity", ShipWorks.Data.Model.EntityType.UserColumnSettingsEntity, false) { }
		
		/// <summary>Creates a new UserColumnSettingsEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UserColumnSettingsEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewUserColumnSettingsUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UserSettingsEntity objects.</summary>
	[Serializable]
	public partial class UserSettingsEntityFactory : EntityFactoryBase2<UserSettingsEntity> {
		/// <summary>CTor</summary>
		public UserSettingsEntityFactory() : base("UserSettingsEntity", ShipWorks.Data.Model.EntityType.UserSettingsEntity, false) { }
		
		/// <summary>Creates a new UserSettingsEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UserSettingsEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewUserSettingsUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UspsAccountEntity objects.</summary>
	[Serializable]
	public partial class UspsAccountEntityFactory : EntityFactoryBase2<UspsAccountEntity> {
		/// <summary>CTor</summary>
		public UspsAccountEntityFactory() : base("UspsAccountEntity", ShipWorks.Data.Model.EntityType.UspsAccountEntity, false) { }
		
		/// <summary>Creates a new UspsAccountEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UspsAccountEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewUspsAccountUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UspsProfileEntity objects.</summary>
	[Serializable]
	public partial class UspsProfileEntityFactory : EntityFactoryBase2<UspsProfileEntity> {
		/// <summary>CTor</summary>
		public UspsProfileEntityFactory() : base("UspsProfileEntity", ShipWorks.Data.Model.EntityType.UspsProfileEntity, false) { }
		
		/// <summary>Creates a new UspsProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UspsProfileEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewUspsProfileUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UspsScanFormEntity objects.</summary>
	[Serializable]
	public partial class UspsScanFormEntityFactory : EntityFactoryBase2<UspsScanFormEntity> {
		/// <summary>CTor</summary>
		public UspsScanFormEntityFactory() : base("UspsScanFormEntity", ShipWorks.Data.Model.EntityType.UspsScanFormEntity, false) { }
		
		/// <summary>Creates a new UspsScanFormEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UspsScanFormEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewUspsScanFormUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty UspsShipmentEntity objects.</summary>
	[Serializable]
	public partial class UspsShipmentEntityFactory : EntityFactoryBase2<UspsShipmentEntity> {
		/// <summary>CTor</summary>
		public UspsShipmentEntityFactory() : base("UspsShipmentEntity", ShipWorks.Data.Model.EntityType.UspsShipmentEntity, false) { }
		
		/// <summary>Creates a new UspsShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UspsShipmentEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewUspsShipmentUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty ValidatedAddressEntity objects.</summary>
	[Serializable]
	public partial class ValidatedAddressEntityFactory : EntityFactoryBase2<ValidatedAddressEntity> {
		/// <summary>CTor</summary>
		public ValidatedAddressEntityFactory() : base("ValidatedAddressEntity", ShipWorks.Data.Model.EntityType.ValidatedAddressEntity, false) { }
		
		/// <summary>Creates a new ValidatedAddressEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ValidatedAddressEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewValidatedAddressUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty VersionSignoffEntity objects.</summary>
	[Serializable]
	public partial class VersionSignoffEntityFactory : EntityFactoryBase2<VersionSignoffEntity> {
		/// <summary>CTor</summary>
		public VersionSignoffEntityFactory() : base("VersionSignoffEntity", ShipWorks.Data.Model.EntityType.VersionSignoffEntity, false) { }
		
		/// <summary>Creates a new VersionSignoffEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new VersionSignoffEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewVersionSignoffUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty VolusionStoreEntity objects.</summary>
	[Serializable]
	public partial class VolusionStoreEntityFactory : EntityFactoryBase2<VolusionStoreEntity> {
		/// <summary>CTor</summary>
		public VolusionStoreEntityFactory() : base("VolusionStoreEntity", ShipWorks.Data.Model.EntityType.VolusionStoreEntity, true) { }
		
		/// <summary>Creates a new VolusionStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new VolusionStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewVolusionStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty WalmartOrderEntity objects.</summary>
	[Serializable]
	public partial class WalmartOrderEntityFactory : EntityFactoryBase2<WalmartOrderEntity> {
		/// <summary>CTor</summary>
		public WalmartOrderEntityFactory() : base("WalmartOrderEntity", ShipWorks.Data.Model.EntityType.WalmartOrderEntity, true) { }
		
		/// <summary>Creates a new WalmartOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new WalmartOrderEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewWalmartOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty WalmartOrderItemEntity objects.</summary>
	[Serializable]
	public partial class WalmartOrderItemEntityFactory : EntityFactoryBase2<WalmartOrderItemEntity> {
		/// <summary>CTor</summary>
		public WalmartOrderItemEntityFactory() : base("WalmartOrderItemEntity", ShipWorks.Data.Model.EntityType.WalmartOrderItemEntity, true) { }
		
		/// <summary>Creates a new WalmartOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new WalmartOrderItemEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewWalmartOrderItemUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty WalmartOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class WalmartOrderSearchEntityFactory : EntityFactoryBase2<WalmartOrderSearchEntity> {
		/// <summary>CTor</summary>
		public WalmartOrderSearchEntityFactory() : base("WalmartOrderSearchEntity", ShipWorks.Data.Model.EntityType.WalmartOrderSearchEntity, false) { }
		
		/// <summary>Creates a new WalmartOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new WalmartOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewWalmartOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty WalmartStoreEntity objects.</summary>
	[Serializable]
	public partial class WalmartStoreEntityFactory : EntityFactoryBase2<WalmartStoreEntity> {
		/// <summary>CTor</summary>
		public WalmartStoreEntityFactory() : base("WalmartStoreEntity", ShipWorks.Data.Model.EntityType.WalmartStoreEntity, true) { }
		
		/// <summary>Creates a new WalmartStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new WalmartStoreEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewWalmartStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty WorldShipGoodsEntity objects.</summary>
	[Serializable]
	public partial class WorldShipGoodsEntityFactory : EntityFactoryBase2<WorldShipGoodsEntity> {
		/// <summary>CTor</summary>
		public WorldShipGoodsEntityFactory() : base("WorldShipGoodsEntity", ShipWorks.Data.Model.EntityType.WorldShipGoodsEntity, false) { }
		
		/// <summary>Creates a new WorldShipGoodsEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new WorldShipGoodsEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewWorldShipGoodsUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty WorldShipPackageEntity objects.</summary>
	[Serializable]
	public partial class WorldShipPackageEntityFactory : EntityFactoryBase2<WorldShipPackageEntity> {
		/// <summary>CTor</summary>
		public WorldShipPackageEntityFactory() : base("WorldShipPackageEntity", ShipWorks.Data.Model.EntityType.WorldShipPackageEntity, false) { }
		
		/// <summary>Creates a new WorldShipPackageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new WorldShipPackageEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewWorldShipPackageUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty WorldShipProcessedEntity objects.</summary>
	[Serializable]
	public partial class WorldShipProcessedEntityFactory : EntityFactoryBase2<WorldShipProcessedEntity> {
		/// <summary>CTor</summary>
		public WorldShipProcessedEntityFactory() : base("WorldShipProcessedEntity", ShipWorks.Data.Model.EntityType.WorldShipProcessedEntity, false) { }
		
		/// <summary>Creates a new WorldShipProcessedEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new WorldShipProcessedEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewWorldShipProcessedUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty WorldShipShipmentEntity objects.</summary>
	[Serializable]
	public partial class WorldShipShipmentEntityFactory : EntityFactoryBase2<WorldShipShipmentEntity> {
		/// <summary>CTor</summary>
		public WorldShipShipmentEntityFactory() : base("WorldShipShipmentEntity", ShipWorks.Data.Model.EntityType.WorldShipShipmentEntity, false) { }
		
		/// <summary>Creates a new WorldShipShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new WorldShipShipmentEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewWorldShipShipmentUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty YahooOrderEntity objects.</summary>
	[Serializable]
	public partial class YahooOrderEntityFactory : EntityFactoryBase2<YahooOrderEntity> {
		/// <summary>CTor</summary>
		public YahooOrderEntityFactory() : base("YahooOrderEntity", ShipWorks.Data.Model.EntityType.YahooOrderEntity, true) { }
		
		/// <summary>Creates a new YahooOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new YahooOrderEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewYahooOrderUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty YahooOrderItemEntity objects.</summary>
	[Serializable]
	public partial class YahooOrderItemEntityFactory : EntityFactoryBase2<YahooOrderItemEntity> {
		/// <summary>CTor</summary>
		public YahooOrderItemEntityFactory() : base("YahooOrderItemEntity", ShipWorks.Data.Model.EntityType.YahooOrderItemEntity, true) { }
		
		/// <summary>Creates a new YahooOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new YahooOrderItemEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewYahooOrderItemUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty YahooOrderSearchEntity objects.</summary>
	[Serializable]
	public partial class YahooOrderSearchEntityFactory : EntityFactoryBase2<YahooOrderSearchEntity> {
		/// <summary>CTor</summary>
		public YahooOrderSearchEntityFactory() : base("YahooOrderSearchEntity", ShipWorks.Data.Model.EntityType.YahooOrderSearchEntity, false) { }
		
		/// <summary>Creates a new YahooOrderSearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new YahooOrderSearchEntity(fields);
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewYahooOrderSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty YahooProductEntity objects.</summary>
	[Serializable]
	public partial class YahooProductEntityFactory : EntityFactoryBase2<YahooProductEntity> {
		/// <summary>CTor</summary>
		public YahooProductEntityFactory() : base("YahooProductEntity", ShipWorks.Data.Model.EntityType.YahooProductEntity, false) { }
		
		/// <summary>Creates a new YahooProductEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new YahooProductEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewYahooProductUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty YahooStoreEntity objects.</summary>
	[Serializable]
	public partial class YahooStoreEntityFactory : EntityFactoryBase2<YahooStoreEntity> {
		/// <summary>CTor</summary>
		public YahooStoreEntityFactory() : base("YahooStoreEntity", ShipWorks.Data.Model.EntityType.YahooStoreEntity, true) { }
		
		/// <summary>Creates a new YahooStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new YahooStoreEntity(fields);
            // __LLBLGENPRO_USER_CODE_REGION_START CreateNewYahooStoreUsingFields
            // __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		#region Included Code

		#endregion
	}

	/// <summary>Factory to create new, empty Entity objects based on the entity type specified. Uses  entity specific factory objects</summary>
	[Serializable]
	public partial class GeneralEntityFactory
	{
		/// <summary>Creates a new, empty Entity object of the type specified</summary>
		/// <param name="entityTypeToCreate">The entity type to create.</param>
		/// <returns>A new, empty Entity object.</returns>
		public static IEntity2 Create(ShipWorks.Data.Model.EntityType entityTypeToCreate)
		{
			IEntityFactory2 factoryToUse = null;
			switch(entityTypeToCreate)
			{
				case ShipWorks.Data.Model.EntityType.ActionEntity:
					factoryToUse = new ActionEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ActionFilterTriggerEntity:
					factoryToUse = new ActionFilterTriggerEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ActionQueueEntity:
					factoryToUse = new ActionQueueEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ActionQueueSelectionEntity:
					factoryToUse = new ActionQueueSelectionEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ActionQueueStepEntity:
					factoryToUse = new ActionQueueStepEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ActionTaskEntity:
					factoryToUse = new ActionTaskEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.AmazonASINEntity:
					factoryToUse = new AmazonASINEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.AmazonOrderEntity:
					factoryToUse = new AmazonOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.AmazonOrderItemEntity:
					factoryToUse = new AmazonOrderItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.AmazonOrderSearchEntity:
					factoryToUse = new AmazonOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.AmazonProfileEntity:
					factoryToUse = new AmazonProfileEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.AmazonShipmentEntity:
					factoryToUse = new AmazonShipmentEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.AmazonStoreEntity:
					factoryToUse = new AmazonStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.AmeriCommerceStoreEntity:
					factoryToUse = new AmeriCommerceStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.AuditEntity:
					factoryToUse = new AuditEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.AuditChangeEntity:
					factoryToUse = new AuditChangeEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.AuditChangeDetailEntity:
					factoryToUse = new AuditChangeDetailEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.BestRateProfileEntity:
					factoryToUse = new BestRateProfileEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.BestRateShipmentEntity:
					factoryToUse = new BestRateShipmentEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.BigCommerceOrderItemEntity:
					factoryToUse = new BigCommerceOrderItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.BigCommerceStoreEntity:
					factoryToUse = new BigCommerceStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.BuyDotComOrderItemEntity:
					factoryToUse = new BuyDotComOrderItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.BuyDotComStoreEntity:
					factoryToUse = new BuyDotComStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ChannelAdvisorOrderEntity:
					factoryToUse = new ChannelAdvisorOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ChannelAdvisorOrderItemEntity:
					factoryToUse = new ChannelAdvisorOrderItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ChannelAdvisorOrderSearchEntity:
					factoryToUse = new ChannelAdvisorOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ChannelAdvisorStoreEntity:
					factoryToUse = new ChannelAdvisorStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ClickCartProOrderEntity:
					factoryToUse = new ClickCartProOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ClickCartProOrderSearchEntity:
					factoryToUse = new ClickCartProOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.CommerceInterfaceOrderEntity:
					factoryToUse = new CommerceInterfaceOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.CommerceInterfaceOrderSearchEntity:
					factoryToUse = new CommerceInterfaceOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ComputerEntity:
					factoryToUse = new ComputerEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ConfigurationEntity:
					factoryToUse = new ConfigurationEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.CustomerEntity:
					factoryToUse = new CustomerEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.DimensionsProfileEntity:
					factoryToUse = new DimensionsProfileEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.DownloadEntity:
					factoryToUse = new DownloadEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.DownloadDetailEntity:
					factoryToUse = new DownloadDetailEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.EbayCombinedOrderRelationEntity:
					factoryToUse = new EbayCombinedOrderRelationEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.EbayOrderEntity:
					factoryToUse = new EbayOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.EbayOrderItemEntity:
					factoryToUse = new EbayOrderItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.EbayOrderSearchEntity:
					factoryToUse = new EbayOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.EbayStoreEntity:
					factoryToUse = new EbayStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.EmailAccountEntity:
					factoryToUse = new EmailAccountEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.EmailOutboundEntity:
					factoryToUse = new EmailOutboundEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.EmailOutboundRelationEntity:
					factoryToUse = new EmailOutboundRelationEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.EndiciaAccountEntity:
					factoryToUse = new EndiciaAccountEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.EndiciaProfileEntity:
					factoryToUse = new EndiciaProfileEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.EndiciaScanFormEntity:
					factoryToUse = new EndiciaScanFormEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.EndiciaShipmentEntity:
					factoryToUse = new EndiciaShipmentEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.EtsyOrderEntity:
					factoryToUse = new EtsyOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.EtsyStoreEntity:
					factoryToUse = new EtsyStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ExcludedPackageTypeEntity:
					factoryToUse = new ExcludedPackageTypeEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ExcludedServiceTypeEntity:
					factoryToUse = new ExcludedServiceTypeEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.FedExAccountEntity:
					factoryToUse = new FedExAccountEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.FedExEndOfDayCloseEntity:
					factoryToUse = new FedExEndOfDayCloseEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.FedExPackageEntity:
					factoryToUse = new FedExPackageEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.FedExProfileEntity:
					factoryToUse = new FedExProfileEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.FedExProfilePackageEntity:
					factoryToUse = new FedExProfilePackageEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.FedExShipmentEntity:
					factoryToUse = new FedExShipmentEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.FilterEntity:
					factoryToUse = new FilterEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.FilterLayoutEntity:
					factoryToUse = new FilterLayoutEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.FilterNodeEntity:
					factoryToUse = new FilterNodeEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.FilterNodeColumnSettingsEntity:
					factoryToUse = new FilterNodeColumnSettingsEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.FilterNodeContentEntity:
					factoryToUse = new FilterNodeContentEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.FilterNodeContentDetailEntity:
					factoryToUse = new FilterNodeContentDetailEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.FilterSequenceEntity:
					factoryToUse = new FilterSequenceEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.FtpAccountEntity:
					factoryToUse = new FtpAccountEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.GenericFileStoreEntity:
					factoryToUse = new GenericFileStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.GenericModuleStoreEntity:
					factoryToUse = new GenericModuleStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.GridColumnFormatEntity:
					factoryToUse = new GridColumnFormatEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.GridColumnLayoutEntity:
					factoryToUse = new GridColumnLayoutEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.GridColumnPositionEntity:
					factoryToUse = new GridColumnPositionEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.GrouponOrderEntity:
					factoryToUse = new GrouponOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.GrouponOrderItemEntity:
					factoryToUse = new GrouponOrderItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.GrouponOrderSearchEntity:
					factoryToUse = new GrouponOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.GrouponStoreEntity:
					factoryToUse = new GrouponStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.InfopiaOrderItemEntity:
					factoryToUse = new InfopiaOrderItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.InfopiaStoreEntity:
					factoryToUse = new InfopiaStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.InsurancePolicyEntity:
					factoryToUse = new InsurancePolicyEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.IParcelAccountEntity:
					factoryToUse = new IParcelAccountEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.IParcelPackageEntity:
					factoryToUse = new IParcelPackageEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.IParcelProfileEntity:
					factoryToUse = new IParcelProfileEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.IParcelProfilePackageEntity:
					factoryToUse = new IParcelProfilePackageEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.IParcelShipmentEntity:
					factoryToUse = new IParcelShipmentEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.LabelSheetEntity:
					factoryToUse = new LabelSheetEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.LemonStandOrderEntity:
					factoryToUse = new LemonStandOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.LemonStandOrderItemEntity:
					factoryToUse = new LemonStandOrderItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.LemonStandOrderSearchEntity:
					factoryToUse = new LemonStandOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.LemonStandStoreEntity:
					factoryToUse = new LemonStandStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.MagentoOrderEntity:
					factoryToUse = new MagentoOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.MagentoOrderSearchEntity:
					factoryToUse = new MagentoOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.MagentoStoreEntity:
					factoryToUse = new MagentoStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.MarketplaceAdvisorOrderEntity:
					factoryToUse = new MarketplaceAdvisorOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.MarketplaceAdvisorOrderSearchEntity:
					factoryToUse = new MarketplaceAdvisorOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.MarketplaceAdvisorStoreEntity:
					factoryToUse = new MarketplaceAdvisorStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.MivaOrderItemAttributeEntity:
					factoryToUse = new MivaOrderItemAttributeEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.MivaStoreEntity:
					factoryToUse = new MivaStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.NetworkSolutionsOrderEntity:
					factoryToUse = new NetworkSolutionsOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.NetworkSolutionsOrderSearchEntity:
					factoryToUse = new NetworkSolutionsOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.NetworkSolutionsStoreEntity:
					factoryToUse = new NetworkSolutionsStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.NeweggOrderEntity:
					factoryToUse = new NeweggOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.NeweggOrderItemEntity:
					factoryToUse = new NeweggOrderItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.NeweggStoreEntity:
					factoryToUse = new NeweggStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.NoteEntity:
					factoryToUse = new NoteEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ObjectLabelEntity:
					factoryToUse = new ObjectLabelEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ObjectReferenceEntity:
					factoryToUse = new ObjectReferenceEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.OdbcStoreEntity:
					factoryToUse = new OdbcStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.OnTracAccountEntity:
					factoryToUse = new OnTracAccountEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.OnTracProfileEntity:
					factoryToUse = new OnTracProfileEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.OnTracShipmentEntity:
					factoryToUse = new OnTracShipmentEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.OrderEntity:
					factoryToUse = new OrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.OrderChargeEntity:
					factoryToUse = new OrderChargeEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.OrderItemEntity:
					factoryToUse = new OrderItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.OrderItemAttributeEntity:
					factoryToUse = new OrderItemAttributeEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.OrderMotionOrderEntity:
					factoryToUse = new OrderMotionOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.OrderMotionOrderSearchEntity:
					factoryToUse = new OrderMotionOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.OrderMotionStoreEntity:
					factoryToUse = new OrderMotionStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.OrderPaymentDetailEntity:
					factoryToUse = new OrderPaymentDetailEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.OrderSearchEntity:
					factoryToUse = new OrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.OtherProfileEntity:
					factoryToUse = new OtherProfileEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.OtherShipmentEntity:
					factoryToUse = new OtherShipmentEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.PayPalOrderEntity:
					factoryToUse = new PayPalOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.PayPalOrderSearchEntity:
					factoryToUse = new PayPalOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.PayPalStoreEntity:
					factoryToUse = new PayPalStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.PermissionEntity:
					factoryToUse = new PermissionEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.PostalProfileEntity:
					factoryToUse = new PostalProfileEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.PostalShipmentEntity:
					factoryToUse = new PostalShipmentEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.PrintResultEntity:
					factoryToUse = new PrintResultEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ProStoresOrderEntity:
					factoryToUse = new ProStoresOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ProStoresOrderSearchEntity:
					factoryToUse = new ProStoresOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ProStoresStoreEntity:
					factoryToUse = new ProStoresStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ResourceEntity:
					factoryToUse = new ResourceEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ScanFormBatchEntity:
					factoryToUse = new ScanFormBatchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.SearchEntity:
					factoryToUse = new SearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.SearsOrderEntity:
					factoryToUse = new SearsOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.SearsOrderItemEntity:
					factoryToUse = new SearsOrderItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.SearsOrderSearchEntity:
					factoryToUse = new SearsOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.SearsStoreEntity:
					factoryToUse = new SearsStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ServerMessageEntity:
					factoryToUse = new ServerMessageEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ServerMessageSignoffEntity:
					factoryToUse = new ServerMessageSignoffEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ServiceStatusEntity:
					factoryToUse = new ServiceStatusEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ShipmentEntity:
					factoryToUse = new ShipmentEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ShipmentCustomsItemEntity:
					factoryToUse = new ShipmentCustomsItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ShippingDefaultsRuleEntity:
					factoryToUse = new ShippingDefaultsRuleEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ShippingOriginEntity:
					factoryToUse = new ShippingOriginEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ShippingPrintOutputEntity:
					factoryToUse = new ShippingPrintOutputEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ShippingPrintOutputRuleEntity:
					factoryToUse = new ShippingPrintOutputRuleEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ShippingProfileEntity:
					factoryToUse = new ShippingProfileEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ShippingProviderRuleEntity:
					factoryToUse = new ShippingProviderRuleEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ShippingSettingsEntity:
					factoryToUse = new ShippingSettingsEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ShipSenseKnowledgebaseEntity:
					factoryToUse = new ShipSenseKnowledgebaseEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ShopifyOrderEntity:
					factoryToUse = new ShopifyOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ShopifyOrderItemEntity:
					factoryToUse = new ShopifyOrderItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ShopifyOrderSearchEntity:
					factoryToUse = new ShopifyOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ShopifyStoreEntity:
					factoryToUse = new ShopifyStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ShopSiteStoreEntity:
					factoryToUse = new ShopSiteStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.SparkPayStoreEntity:
					factoryToUse = new SparkPayStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.StatusPresetEntity:
					factoryToUse = new StatusPresetEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.StoreEntity:
					factoryToUse = new StoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.SystemDataEntity:
					factoryToUse = new SystemDataEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.TemplateEntity:
					factoryToUse = new TemplateEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.TemplateComputerSettingsEntity:
					factoryToUse = new TemplateComputerSettingsEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.TemplateFolderEntity:
					factoryToUse = new TemplateFolderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.TemplateStoreSettingsEntity:
					factoryToUse = new TemplateStoreSettingsEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.TemplateUserSettingsEntity:
					factoryToUse = new TemplateUserSettingsEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ThreeDCartOrderEntity:
					factoryToUse = new ThreeDCartOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ThreeDCartOrderItemEntity:
					factoryToUse = new ThreeDCartOrderItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ThreeDCartOrderSearchEntity:
					factoryToUse = new ThreeDCartOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ThreeDCartStoreEntity:
					factoryToUse = new ThreeDCartStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsAccountEntity:
					factoryToUse = new UpsAccountEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsLetterRateEntity:
					factoryToUse = new UpsLetterRateEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsLocalRatingDeliveryAreaSurchargeEntity:
					factoryToUse = new UpsLocalRatingDeliveryAreaSurchargeEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsLocalRatingZoneEntity:
					factoryToUse = new UpsLocalRatingZoneEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsLocalRatingZoneFileEntity:
					factoryToUse = new UpsLocalRatingZoneFileEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsPackageEntity:
					factoryToUse = new UpsPackageEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsPackageRateEntity:
					factoryToUse = new UpsPackageRateEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsPricePerPoundEntity:
					factoryToUse = new UpsPricePerPoundEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsProfileEntity:
					factoryToUse = new UpsProfileEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsProfilePackageEntity:
					factoryToUse = new UpsProfilePackageEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsRateSurchargeEntity:
					factoryToUse = new UpsRateSurchargeEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsRateTableEntity:
					factoryToUse = new UpsRateTableEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsShipmentEntity:
					factoryToUse = new UpsShipmentEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UserEntity:
					factoryToUse = new UserEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UserColumnSettingsEntity:
					factoryToUse = new UserColumnSettingsEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UserSettingsEntity:
					factoryToUse = new UserSettingsEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UspsAccountEntity:
					factoryToUse = new UspsAccountEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UspsProfileEntity:
					factoryToUse = new UspsProfileEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UspsScanFormEntity:
					factoryToUse = new UspsScanFormEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UspsShipmentEntity:
					factoryToUse = new UspsShipmentEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ValidatedAddressEntity:
					factoryToUse = new ValidatedAddressEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.VersionSignoffEntity:
					factoryToUse = new VersionSignoffEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.VolusionStoreEntity:
					factoryToUse = new VolusionStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.WalmartOrderEntity:
					factoryToUse = new WalmartOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.WalmartOrderItemEntity:
					factoryToUse = new WalmartOrderItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.WalmartOrderSearchEntity:
					factoryToUse = new WalmartOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.WalmartStoreEntity:
					factoryToUse = new WalmartStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.WorldShipGoodsEntity:
					factoryToUse = new WorldShipGoodsEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.WorldShipPackageEntity:
					factoryToUse = new WorldShipPackageEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.WorldShipProcessedEntity:
					factoryToUse = new WorldShipProcessedEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.WorldShipShipmentEntity:
					factoryToUse = new WorldShipShipmentEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.YahooOrderEntity:
					factoryToUse = new YahooOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.YahooOrderItemEntity:
					factoryToUse = new YahooOrderItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.YahooOrderSearchEntity:
					factoryToUse = new YahooOrderSearchEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.YahooProductEntity:
					factoryToUse = new YahooProductEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.YahooStoreEntity:
					factoryToUse = new YahooStoreEntityFactory();
					break;
			}
			IEntity2 toReturn = null;
			if(factoryToUse != null)
			{
				toReturn = factoryToUse.Create();
			}
			return toReturn;
		}		
	}
		
	/// <summary>Class which is used to obtain the entity factory based on the .NET type of the entity. </summary>
	[Serializable]
	public static class EntityFactoryFactory
	{
		private static Dictionary<Type, IEntityFactory2> _factoryPerType = new Dictionary<Type, IEntityFactory2>();

		/// <summary>Initializes the <see cref="EntityFactoryFactory"/> class.</summary>
		static EntityFactoryFactory()
		{
			Array entityTypeValues = Enum.GetValues(typeof(ShipWorks.Data.Model.EntityType));
			foreach(int entityTypeValue in entityTypeValues)
			{
				IEntity2 dummy = GeneralEntityFactory.Create((ShipWorks.Data.Model.EntityType)entityTypeValue);
				_factoryPerType.Add(dummy.GetType(), dummy.GetEntityFactory());
			}
		}

		/// <summary>Gets the factory of the entity with the .NET type specified</summary>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <returns>factory to use or null if not found</returns>
		public static IEntityFactory2 GetFactory(Type typeOfEntity)
		{
			IEntityFactory2 toReturn = null;
			_factoryPerType.TryGetValue(typeOfEntity, out toReturn);
			return toReturn;
		}

		/// <summary>Gets the factory of the entity with the ShipWorks.Data.Model.EntityType specified</summary>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <returns>factory to use or null if not found</returns>
		public static IEntityFactory2 GetFactory(ShipWorks.Data.Model.EntityType typeOfEntity)
		{
			return GetFactory(GeneralEntityFactory.Create(typeOfEntity).GetType());
		}
	}
		
	/// <summary>Element creator for creating project elements from somewhere else, like inside Linq providers.</summary>
	public class ElementCreator : ElementCreatorBase, IElementCreator2
	{
		/// <summary>Gets the factory of the Entity type with the ShipWorks.Data.Model.EntityType value passed in</summary>
		/// <param name="entityTypeValue">The entity type value.</param>
		/// <returns>the entity factory of the entity type or null if not found</returns>
		public IEntityFactory2 GetFactory(int entityTypeValue)
		{
			return (IEntityFactory2)this.GetFactoryImpl(entityTypeValue);
		}
		
		/// <summary>Gets the factory of the Entity type with the .NET type passed in</summary>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <returns>the entity factory of the entity type or null if not found</returns>
		public IEntityFactory2 GetFactory(Type typeOfEntity)
		{
			return (IEntityFactory2)this.GetFactoryImpl(typeOfEntity);
		}

		/// <summary>Creates a new resultset fields object with the number of field slots reserved as specified</summary>
		/// <param name="numberOfFields">The number of fields.</param>
		/// <returns>ready to use resultsetfields object</returns>
		public IEntityFields2 CreateResultsetFields(int numberOfFields)
		{
			return new ResultsetFields(numberOfFields);
		}
		
		/// <summary>Obtains the inheritance info provider instance from the singleton </summary>
		/// <returns>The singleton instance of the inheritance info provider</returns>
		public override IInheritanceInfoProvider ObtainInheritanceInfoProviderInstance()
		{
			return InheritanceInfoProviderSingleton.GetInstance();
		}


		/// <summary>Creates a new dynamic relation instance</summary>
		/// <param name="leftOperand">The left operand.</param>
		/// <returns>ready to use dynamic relation</returns>
		public override IDynamicRelation CreateDynamicRelation(DerivedTableDefinition leftOperand)
		{
			return new DynamicRelation(leftOperand);
		}

		/// <summary>Creates a new dynamic relation instance</summary>
		/// <param name="leftOperand">The left operand.</param>
		/// <param name="joinType">Type of the join. If None is specified, Inner is assumed.</param>
		/// <param name="rightOperand">The right operand.</param>
		/// <param name="onClause">The on clause for the join.</param>
		/// <returns>ready to use dynamic relation</returns>
		public override IDynamicRelation CreateDynamicRelation(DerivedTableDefinition leftOperand, JoinHint joinType, DerivedTableDefinition rightOperand, IPredicate onClause)
		{
			return new DynamicRelation(leftOperand, joinType, rightOperand, onClause);
		}

		/// <summary>Creates a new dynamic relation instance</summary>
		/// <param name="leftOperand">The left operand.</param>
		/// <param name="joinType">Type of the join. If None is specified, Inner is assumed.</param>
		/// <param name="rightOperand">The right operand.</param>
		/// <param name="aliasLeftOperand">The alias of the left operand. If you don't want to / need to alias the right operand (only alias if you have to), specify string.Empty.</param>
		/// <param name="onClause">The on clause for the join.</param>
		/// <returns>ready to use dynamic relation</returns>
		public override IDynamicRelation CreateDynamicRelation(IEntityFieldCore leftOperand, JoinHint joinType, DerivedTableDefinition rightOperand, string aliasLeftOperand, IPredicate onClause)
		{
			return new DynamicRelation(leftOperand, joinType, rightOperand, aliasLeftOperand, onClause);
		}


		/// <summary>Creates a new dynamic relation instance</summary>
		/// <param name="leftOperand">The left operand.</param>
		/// <param name="joinType">Type of the join. If None is specified, Inner is assumed.</param>
		/// <param name="rightOperandEntityName">Name of the entity, which is used as the right operand.</param>
		/// <param name="aliasRightOperand">The alias of the right operand. If you don't want to / need to alias the right operand (only alias if you have to), specify string.Empty.</param>
		/// <param name="onClause">The on clause for the join.</param>
		/// <returns>ready to use dynamic relation</returns>
		public override IDynamicRelation CreateDynamicRelation(DerivedTableDefinition leftOperand, JoinHint joinType, string rightOperandEntityName, string aliasRightOperand, IPredicate onClause)
		{
			return new DynamicRelation(leftOperand, joinType, (ShipWorks.Data.Model.EntityType)Enum.Parse(typeof(ShipWorks.Data.Model.EntityType), rightOperandEntityName, false), aliasRightOperand, onClause);
		}

		/// <summary>Creates a new dynamic relation instance</summary>
		/// <param name="leftOperandEntityName">Name of the entity which is used as the left operand.</param>
		/// <param name="joinType">Type of the join. If None is specified, Inner is assumed.</param>
		/// <param name="rightOperandEntityName">Name of the entity, which is used as the right operand.</param>
		/// <param name="aliasLeftOperand">The alias of the left operand. If you don't want to / need to alias the right operand (only alias if you have to), specify string.Empty.</param>
		/// <param name="aliasRightOperand">The alias of the right operand. If you don't want to / need to alias the right operand (only alias if you have to), specify string.Empty.</param>
		/// <param name="onClause">The on clause for the join.</param>
		/// <returns>ready to use dynamic relation</returns>
		public override IDynamicRelation CreateDynamicRelation(string leftOperandEntityName, JoinHint joinType, string rightOperandEntityName, string aliasLeftOperand, string aliasRightOperand, IPredicate onClause)
		{
			return new DynamicRelation((ShipWorks.Data.Model.EntityType)Enum.Parse(typeof(ShipWorks.Data.Model.EntityType), leftOperandEntityName, false), joinType, (ShipWorks.Data.Model.EntityType)Enum.Parse(typeof(ShipWorks.Data.Model.EntityType), rightOperandEntityName, false), aliasLeftOperand, aliasRightOperand, onClause);
		}
		
		/// <summary>Creates a new dynamic relation instance</summary>
		/// <param name="leftOperand">The left operand.</param>
		/// <param name="joinType">Type of the join. If None is specified, Inner is assumed.</param>
		/// <param name="rightOperandEntityName">Name of the entity, which is used as the right operand.</param>
		/// <param name="aliasLeftOperand">The alias of the left operand. If you don't want to / need to alias the right operand (only alias if you have to), specify string.Empty.</param>
		/// <param name="aliasRightOperand">The alias of the right operand. If you don't want to / need to alias the right operand (only alias if you have to), specify string.Empty.</param>
		/// <param name="onClause">The on clause for the join.</param>
		/// <returns>ready to use dynamic relation</returns>
		public override IDynamicRelation CreateDynamicRelation(IEntityFieldCore leftOperand, JoinHint joinType, string rightOperandEntityName, string aliasLeftOperand, string aliasRightOperand, IPredicate onClause)
		{
			return new DynamicRelation(leftOperand, joinType, (ShipWorks.Data.Model.EntityType)Enum.Parse(typeof(ShipWorks.Data.Model.EntityType), rightOperandEntityName, false), aliasLeftOperand, aliasRightOperand, onClause);
		}
		
		/// <summary>Implementation of the routine which gets the factory of the Entity type with the ShipWorks.Data.Model.EntityType value passed in</summary>
		/// <param name="entityTypeValue">The entity type value.</param>
		/// <returns>the entity factory of the entity type or null if not found</returns>
		protected override IEntityFactoryCore GetFactoryImpl(int entityTypeValue)
		{
			return EntityFactoryFactory.GetFactory((ShipWorks.Data.Model.EntityType)entityTypeValue);
		}

		/// <summary>Implementation of the routine which gets the factory of the Entity type with the .NET type passed in</summary>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <returns>the entity factory of the entity type or null if not found</returns>
		protected override IEntityFactoryCore GetFactoryImpl(Type typeOfEntity)
		{
			return EntityFactoryFactory.GetFactory(typeOfEntity);
		}

	}
}
