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
	public partial class EntityFactoryBase2 : EntityFactoryCore2
	{
		private string _entityName;
		private ShipWorks.Data.Model.EntityType _typeOfEntity;
		
		/// <summary>CTor</summary>
		/// <param name="entityName">Name of the entity.</param>
		/// <param name="typeOfEntity">The type of entity.</param>
		public EntityFactoryBase2(string entityName, ShipWorks.Data.Model.EntityType typeOfEntity)
		{
			_entityName = entityName;
			_typeOfEntity = typeOfEntity;
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
			return InheritanceInfoProviderSingleton.GetInstance().GetHierarchyRelations(_entityName, objectAlias);
		}

		/// <summary>This method retrieves, using the InheritanceInfoprovider, the factory for the entity represented by the values passed in.</summary>
		/// <param name="fieldValues">Field values read from the db, to determine which factory to return, based on the field values passed in.</param>
		/// <param name="entityFieldStartIndexesPerEntity">indexes into values where per entity type their own fields start.</param>
		/// <returns>the factory for the entity which is represented by the values passed in.</returns>
		public override IEntityFactory2 GetEntityFactory(object[] fieldValues, Dictionary<string, int> entityFieldStartIndexesPerEntity) 
		{
			IEntityFactory2 toReturn = (IEntityFactory2)InheritanceInfoProviderSingleton.GetInstance().GetEntityFactory(_entityName, fieldValues, entityFieldStartIndexesPerEntity);
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
				
		/// <summary>returns the name of the entity this factory is for, e.g. "EmployeeEntity"</summary>
		public override string ForEntityName 
		{ 
			get { return _entityName; }
		}
	}
	
	/// <summary>Factory to create new, empty ActionEntity objects.</summary>
	[Serializable]
	public partial class ActionEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ActionEntityFactory() : base("ActionEntity", ShipWorks.Data.Model.EntityType.ActionEntity) { }

		/// <summary>Creates a new, empty ActionEntity object.</summary>
		/// <returns>A new, empty ActionEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ActionEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAction
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ActionEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ActionEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ActionEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ActionFilterTriggerEntity objects.</summary>
	[Serializable]
	public partial class ActionFilterTriggerEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ActionFilterTriggerEntityFactory() : base("ActionFilterTriggerEntity", ShipWorks.Data.Model.EntityType.ActionFilterTriggerEntity) { }

		/// <summary>Creates a new, empty ActionFilterTriggerEntity object.</summary>
		/// <returns>A new, empty ActionFilterTriggerEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ActionFilterTriggerEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionFilterTrigger
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ActionFilterTriggerEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ActionFilterTriggerEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionFilterTriggerUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ActionFilterTriggerEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ActionQueueEntity objects.</summary>
	[Serializable]
	public partial class ActionQueueEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ActionQueueEntityFactory() : base("ActionQueueEntity", ShipWorks.Data.Model.EntityType.ActionQueueEntity) { }

		/// <summary>Creates a new, empty ActionQueueEntity object.</summary>
		/// <returns>A new, empty ActionQueueEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ActionQueueEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionQueue
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ActionQueueEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ActionQueueEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionQueueUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ActionQueueEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ActionQueueSelectionEntity objects.</summary>
	[Serializable]
	public partial class ActionQueueSelectionEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ActionQueueSelectionEntityFactory() : base("ActionQueueSelectionEntity", ShipWorks.Data.Model.EntityType.ActionQueueSelectionEntity) { }

		/// <summary>Creates a new, empty ActionQueueSelectionEntity object.</summary>
		/// <returns>A new, empty ActionQueueSelectionEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ActionQueueSelectionEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionQueueSelection
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ActionQueueSelectionEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ActionQueueSelectionEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionQueueSelectionUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ActionQueueSelectionEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ActionQueueStepEntity objects.</summary>
	[Serializable]
	public partial class ActionQueueStepEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ActionQueueStepEntityFactory() : base("ActionQueueStepEntity", ShipWorks.Data.Model.EntityType.ActionQueueStepEntity) { }

		/// <summary>Creates a new, empty ActionQueueStepEntity object.</summary>
		/// <returns>A new, empty ActionQueueStepEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ActionQueueStepEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionQueueStep
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ActionQueueStepEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ActionQueueStepEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionQueueStepUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ActionQueueStepEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ActionTaskEntity objects.</summary>
	[Serializable]
	public partial class ActionTaskEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ActionTaskEntityFactory() : base("ActionTaskEntity", ShipWorks.Data.Model.EntityType.ActionTaskEntity) { }

		/// <summary>Creates a new, empty ActionTaskEntity object.</summary>
		/// <returns>A new, empty ActionTaskEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ActionTaskEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionTask
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ActionTaskEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ActionTaskEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewActionTaskUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ActionTaskEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty AmazonASINEntity objects.</summary>
	[Serializable]
	public partial class AmazonASINEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public AmazonASINEntityFactory() : base("AmazonASINEntity", ShipWorks.Data.Model.EntityType.AmazonASINEntity) { }

		/// <summary>Creates a new, empty AmazonASINEntity object.</summary>
		/// <returns>A new, empty AmazonASINEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new AmazonASINEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonASIN
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new AmazonASINEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AmazonASINEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonASINUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<AmazonASINEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty AmazonOrderEntity objects.</summary>
	[Serializable]
	public partial class AmazonOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public AmazonOrderEntityFactory() : base("AmazonOrderEntity", ShipWorks.Data.Model.EntityType.AmazonOrderEntity) { }

		/// <summary>Creates a new, empty AmazonOrderEntity object.</summary>
		/// <returns>A new, empty AmazonOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new AmazonOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new AmazonOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AmazonOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<AmazonOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("AmazonOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty AmazonOrderItemEntity objects.</summary>
	[Serializable]
	public partial class AmazonOrderItemEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public AmazonOrderItemEntityFactory() : base("AmazonOrderItemEntity", ShipWorks.Data.Model.EntityType.AmazonOrderItemEntity) { }

		/// <summary>Creates a new, empty AmazonOrderItemEntity object.</summary>
		/// <returns>A new, empty AmazonOrderItemEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new AmazonOrderItemEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonOrderItem
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new AmazonOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AmazonOrderItemEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonOrderItemUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<AmazonOrderItemEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("AmazonOrderItemEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty AmazonProfileEntity objects.</summary>
	[Serializable]
	public partial class AmazonProfileEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public AmazonProfileEntityFactory() : base("AmazonProfileEntity", ShipWorks.Data.Model.EntityType.AmazonProfileEntity) { }

		/// <summary>Creates a new, empty AmazonProfileEntity object.</summary>
		/// <returns>A new, empty AmazonProfileEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new AmazonProfileEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonProfile
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new AmazonProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AmazonProfileEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonProfileUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<AmazonProfileEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty AmazonShipmentEntity objects.</summary>
	[Serializable]
	public partial class AmazonShipmentEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public AmazonShipmentEntityFactory() : base("AmazonShipmentEntity", ShipWorks.Data.Model.EntityType.AmazonShipmentEntity) { }

		/// <summary>Creates a new, empty AmazonShipmentEntity object.</summary>
		/// <returns>A new, empty AmazonShipmentEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new AmazonShipmentEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonShipment
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new AmazonShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AmazonShipmentEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonShipmentUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<AmazonShipmentEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty AmazonStoreEntity objects.</summary>
	[Serializable]
	public partial class AmazonStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public AmazonStoreEntityFactory() : base("AmazonStoreEntity", ShipWorks.Data.Model.EntityType.AmazonStoreEntity) { }

		/// <summary>Creates a new, empty AmazonStoreEntity object.</summary>
		/// <returns>A new, empty AmazonStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new AmazonStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new AmazonStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AmazonStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmazonStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<AmazonStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("AmazonStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty AmeriCommerceStoreEntity objects.</summary>
	[Serializable]
	public partial class AmeriCommerceStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public AmeriCommerceStoreEntityFactory() : base("AmeriCommerceStoreEntity", ShipWorks.Data.Model.EntityType.AmeriCommerceStoreEntity) { }

		/// <summary>Creates a new, empty AmeriCommerceStoreEntity object.</summary>
		/// <returns>A new, empty AmeriCommerceStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new AmeriCommerceStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmeriCommerceStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new AmeriCommerceStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AmeriCommerceStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAmeriCommerceStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<AmeriCommerceStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("AmeriCommerceStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty AuditEntity objects.</summary>
	[Serializable]
	public partial class AuditEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public AuditEntityFactory() : base("AuditEntity", ShipWorks.Data.Model.EntityType.AuditEntity) { }

		/// <summary>Creates a new, empty AuditEntity object.</summary>
		/// <returns>A new, empty AuditEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new AuditEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAudit
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new AuditEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AuditEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAuditUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<AuditEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty AuditChangeEntity objects.</summary>
	[Serializable]
	public partial class AuditChangeEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public AuditChangeEntityFactory() : base("AuditChangeEntity", ShipWorks.Data.Model.EntityType.AuditChangeEntity) { }

		/// <summary>Creates a new, empty AuditChangeEntity object.</summary>
		/// <returns>A new, empty AuditChangeEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new AuditChangeEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAuditChange
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new AuditChangeEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AuditChangeEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAuditChangeUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<AuditChangeEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty AuditChangeDetailEntity objects.</summary>
	[Serializable]
	public partial class AuditChangeDetailEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public AuditChangeDetailEntityFactory() : base("AuditChangeDetailEntity", ShipWorks.Data.Model.EntityType.AuditChangeDetailEntity) { }

		/// <summary>Creates a new, empty AuditChangeDetailEntity object.</summary>
		/// <returns>A new, empty AuditChangeDetailEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new AuditChangeDetailEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAuditChangeDetail
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new AuditChangeDetailEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new AuditChangeDetailEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewAuditChangeDetailUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<AuditChangeDetailEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty BestRateProfileEntity objects.</summary>
	[Serializable]
	public partial class BestRateProfileEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public BestRateProfileEntityFactory() : base("BestRateProfileEntity", ShipWorks.Data.Model.EntityType.BestRateProfileEntity) { }

		/// <summary>Creates a new, empty BestRateProfileEntity object.</summary>
		/// <returns>A new, empty BestRateProfileEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new BestRateProfileEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewBestRateProfile
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new BestRateProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new BestRateProfileEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewBestRateProfileUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<BestRateProfileEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty BestRateShipmentEntity objects.</summary>
	[Serializable]
	public partial class BestRateShipmentEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public BestRateShipmentEntityFactory() : base("BestRateShipmentEntity", ShipWorks.Data.Model.EntityType.BestRateShipmentEntity) { }

		/// <summary>Creates a new, empty BestRateShipmentEntity object.</summary>
		/// <returns>A new, empty BestRateShipmentEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new BestRateShipmentEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewBestRateShipment
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new BestRateShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new BestRateShipmentEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewBestRateShipmentUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<BestRateShipmentEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty BigCommerceOrderItemEntity objects.</summary>
	[Serializable]
	public partial class BigCommerceOrderItemEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public BigCommerceOrderItemEntityFactory() : base("BigCommerceOrderItemEntity", ShipWorks.Data.Model.EntityType.BigCommerceOrderItemEntity) { }

		/// <summary>Creates a new, empty BigCommerceOrderItemEntity object.</summary>
		/// <returns>A new, empty BigCommerceOrderItemEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new BigCommerceOrderItemEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewBigCommerceOrderItem
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new BigCommerceOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new BigCommerceOrderItemEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewBigCommerceOrderItemUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<BigCommerceOrderItemEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("BigCommerceOrderItemEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty BigCommerceStoreEntity objects.</summary>
	[Serializable]
	public partial class BigCommerceStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public BigCommerceStoreEntityFactory() : base("BigCommerceStoreEntity", ShipWorks.Data.Model.EntityType.BigCommerceStoreEntity) { }

		/// <summary>Creates a new, empty BigCommerceStoreEntity object.</summary>
		/// <returns>A new, empty BigCommerceStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new BigCommerceStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewBigCommerceStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new BigCommerceStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new BigCommerceStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewBigCommerceStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<BigCommerceStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("BigCommerceStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty BuyDotComOrderItemEntity objects.</summary>
	[Serializable]
	public partial class BuyDotComOrderItemEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public BuyDotComOrderItemEntityFactory() : base("BuyDotComOrderItemEntity", ShipWorks.Data.Model.EntityType.BuyDotComOrderItemEntity) { }

		/// <summary>Creates a new, empty BuyDotComOrderItemEntity object.</summary>
		/// <returns>A new, empty BuyDotComOrderItemEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new BuyDotComOrderItemEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewBuyDotComOrderItem
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new BuyDotComOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new BuyDotComOrderItemEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewBuyDotComOrderItemUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<BuyDotComOrderItemEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("BuyDotComOrderItemEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty BuyDotComStoreEntity objects.</summary>
	[Serializable]
	public partial class BuyDotComStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public BuyDotComStoreEntityFactory() : base("BuyDotComStoreEntity", ShipWorks.Data.Model.EntityType.BuyDotComStoreEntity) { }

		/// <summary>Creates a new, empty BuyDotComStoreEntity object.</summary>
		/// <returns>A new, empty BuyDotComStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new BuyDotComStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewBuyDotComStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new BuyDotComStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new BuyDotComStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewBuyDotComStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<BuyDotComStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("BuyDotComStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ChannelAdvisorOrderEntity objects.</summary>
	[Serializable]
	public partial class ChannelAdvisorOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ChannelAdvisorOrderEntityFactory() : base("ChannelAdvisorOrderEntity", ShipWorks.Data.Model.EntityType.ChannelAdvisorOrderEntity) { }

		/// <summary>Creates a new, empty ChannelAdvisorOrderEntity object.</summary>
		/// <returns>A new, empty ChannelAdvisorOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ChannelAdvisorOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewChannelAdvisorOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ChannelAdvisorOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ChannelAdvisorOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewChannelAdvisorOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ChannelAdvisorOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("ChannelAdvisorOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ChannelAdvisorOrderItemEntity objects.</summary>
	[Serializable]
	public partial class ChannelAdvisorOrderItemEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ChannelAdvisorOrderItemEntityFactory() : base("ChannelAdvisorOrderItemEntity", ShipWorks.Data.Model.EntityType.ChannelAdvisorOrderItemEntity) { }

		/// <summary>Creates a new, empty ChannelAdvisorOrderItemEntity object.</summary>
		/// <returns>A new, empty ChannelAdvisorOrderItemEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ChannelAdvisorOrderItemEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewChannelAdvisorOrderItem
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ChannelAdvisorOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ChannelAdvisorOrderItemEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewChannelAdvisorOrderItemUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ChannelAdvisorOrderItemEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("ChannelAdvisorOrderItemEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ChannelAdvisorStoreEntity objects.</summary>
	[Serializable]
	public partial class ChannelAdvisorStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ChannelAdvisorStoreEntityFactory() : base("ChannelAdvisorStoreEntity", ShipWorks.Data.Model.EntityType.ChannelAdvisorStoreEntity) { }

		/// <summary>Creates a new, empty ChannelAdvisorStoreEntity object.</summary>
		/// <returns>A new, empty ChannelAdvisorStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ChannelAdvisorStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewChannelAdvisorStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ChannelAdvisorStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ChannelAdvisorStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewChannelAdvisorStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ChannelAdvisorStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("ChannelAdvisorStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ClickCartProOrderEntity objects.</summary>
	[Serializable]
	public partial class ClickCartProOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ClickCartProOrderEntityFactory() : base("ClickCartProOrderEntity", ShipWorks.Data.Model.EntityType.ClickCartProOrderEntity) { }

		/// <summary>Creates a new, empty ClickCartProOrderEntity object.</summary>
		/// <returns>A new, empty ClickCartProOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ClickCartProOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewClickCartProOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ClickCartProOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ClickCartProOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewClickCartProOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ClickCartProOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("ClickCartProOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty CommerceInterfaceOrderEntity objects.</summary>
	[Serializable]
	public partial class CommerceInterfaceOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public CommerceInterfaceOrderEntityFactory() : base("CommerceInterfaceOrderEntity", ShipWorks.Data.Model.EntityType.CommerceInterfaceOrderEntity) { }

		/// <summary>Creates a new, empty CommerceInterfaceOrderEntity object.</summary>
		/// <returns>A new, empty CommerceInterfaceOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new CommerceInterfaceOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewCommerceInterfaceOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new CommerceInterfaceOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new CommerceInterfaceOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewCommerceInterfaceOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<CommerceInterfaceOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("CommerceInterfaceOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ComputerEntity objects.</summary>
	[Serializable]
	public partial class ComputerEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ComputerEntityFactory() : base("ComputerEntity", ShipWorks.Data.Model.EntityType.ComputerEntity) { }

		/// <summary>Creates a new, empty ComputerEntity object.</summary>
		/// <returns>A new, empty ComputerEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ComputerEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewComputer
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ComputerEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ComputerEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewComputerUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ComputerEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ConfigurationEntity objects.</summary>
	[Serializable]
	public partial class ConfigurationEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ConfigurationEntityFactory() : base("ConfigurationEntity", ShipWorks.Data.Model.EntityType.ConfigurationEntity) { }

		/// <summary>Creates a new, empty ConfigurationEntity object.</summary>
		/// <returns>A new, empty ConfigurationEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ConfigurationEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewConfiguration
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ConfigurationEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ConfigurationEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewConfigurationUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ConfigurationEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty CustomerEntity objects.</summary>
	[Serializable]
	public partial class CustomerEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public CustomerEntityFactory() : base("CustomerEntity", ShipWorks.Data.Model.EntityType.CustomerEntity) { }

		/// <summary>Creates a new, empty CustomerEntity object.</summary>
		/// <returns>A new, empty CustomerEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new CustomerEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewCustomer
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new CustomerEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new CustomerEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewCustomerUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<CustomerEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty DimensionsProfileEntity objects.</summary>
	[Serializable]
	public partial class DimensionsProfileEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public DimensionsProfileEntityFactory() : base("DimensionsProfileEntity", ShipWorks.Data.Model.EntityType.DimensionsProfileEntity) { }

		/// <summary>Creates a new, empty DimensionsProfileEntity object.</summary>
		/// <returns>A new, empty DimensionsProfileEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new DimensionsProfileEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewDimensionsProfile
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new DimensionsProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new DimensionsProfileEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewDimensionsProfileUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<DimensionsProfileEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty DownloadEntity objects.</summary>
	[Serializable]
	public partial class DownloadEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public DownloadEntityFactory() : base("DownloadEntity", ShipWorks.Data.Model.EntityType.DownloadEntity) { }

		/// <summary>Creates a new, empty DownloadEntity object.</summary>
		/// <returns>A new, empty DownloadEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new DownloadEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewDownload
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new DownloadEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new DownloadEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewDownloadUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<DownloadEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty DownloadDetailEntity objects.</summary>
	[Serializable]
	public partial class DownloadDetailEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public DownloadDetailEntityFactory() : base("DownloadDetailEntity", ShipWorks.Data.Model.EntityType.DownloadDetailEntity) { }

		/// <summary>Creates a new, empty DownloadDetailEntity object.</summary>
		/// <returns>A new, empty DownloadDetailEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new DownloadDetailEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewDownloadDetail
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new DownloadDetailEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new DownloadDetailEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewDownloadDetailUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<DownloadDetailEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty EbayCombinedOrderRelationEntity objects.</summary>
	[Serializable]
	public partial class EbayCombinedOrderRelationEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public EbayCombinedOrderRelationEntityFactory() : base("EbayCombinedOrderRelationEntity", ShipWorks.Data.Model.EntityType.EbayCombinedOrderRelationEntity) { }

		/// <summary>Creates a new, empty EbayCombinedOrderRelationEntity object.</summary>
		/// <returns>A new, empty EbayCombinedOrderRelationEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new EbayCombinedOrderRelationEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEbayCombinedOrderRelation
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new EbayCombinedOrderRelationEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EbayCombinedOrderRelationEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEbayCombinedOrderRelationUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<EbayCombinedOrderRelationEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty EbayOrderEntity objects.</summary>
	[Serializable]
	public partial class EbayOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public EbayOrderEntityFactory() : base("EbayOrderEntity", ShipWorks.Data.Model.EntityType.EbayOrderEntity) { }

		/// <summary>Creates a new, empty EbayOrderEntity object.</summary>
		/// <returns>A new, empty EbayOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new EbayOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEbayOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new EbayOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EbayOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEbayOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<EbayOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("EbayOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty EbayOrderItemEntity objects.</summary>
	[Serializable]
	public partial class EbayOrderItemEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public EbayOrderItemEntityFactory() : base("EbayOrderItemEntity", ShipWorks.Data.Model.EntityType.EbayOrderItemEntity) { }

		/// <summary>Creates a new, empty EbayOrderItemEntity object.</summary>
		/// <returns>A new, empty EbayOrderItemEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new EbayOrderItemEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEbayOrderItem
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new EbayOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EbayOrderItemEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEbayOrderItemUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<EbayOrderItemEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("EbayOrderItemEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty EbayStoreEntity objects.</summary>
	[Serializable]
	public partial class EbayStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public EbayStoreEntityFactory() : base("EbayStoreEntity", ShipWorks.Data.Model.EntityType.EbayStoreEntity) { }

		/// <summary>Creates a new, empty EbayStoreEntity object.</summary>
		/// <returns>A new, empty EbayStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new EbayStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEbayStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new EbayStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EbayStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEbayStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<EbayStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("EbayStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty EmailAccountEntity objects.</summary>
	[Serializable]
	public partial class EmailAccountEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public EmailAccountEntityFactory() : base("EmailAccountEntity", ShipWorks.Data.Model.EntityType.EmailAccountEntity) { }

		/// <summary>Creates a new, empty EmailAccountEntity object.</summary>
		/// <returns>A new, empty EmailAccountEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new EmailAccountEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEmailAccount
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new EmailAccountEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EmailAccountEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEmailAccountUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<EmailAccountEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty EmailOutboundEntity objects.</summary>
	[Serializable]
	public partial class EmailOutboundEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public EmailOutboundEntityFactory() : base("EmailOutboundEntity", ShipWorks.Data.Model.EntityType.EmailOutboundEntity) { }

		/// <summary>Creates a new, empty EmailOutboundEntity object.</summary>
		/// <returns>A new, empty EmailOutboundEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new EmailOutboundEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEmailOutbound
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new EmailOutboundEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EmailOutboundEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEmailOutboundUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<EmailOutboundEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty EmailOutboundRelationEntity objects.</summary>
	[Serializable]
	public partial class EmailOutboundRelationEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public EmailOutboundRelationEntityFactory() : base("EmailOutboundRelationEntity", ShipWorks.Data.Model.EntityType.EmailOutboundRelationEntity) { }

		/// <summary>Creates a new, empty EmailOutboundRelationEntity object.</summary>
		/// <returns>A new, empty EmailOutboundRelationEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new EmailOutboundRelationEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEmailOutboundRelation
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new EmailOutboundRelationEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EmailOutboundRelationEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEmailOutboundRelationUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<EmailOutboundRelationEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty EndiciaAccountEntity objects.</summary>
	[Serializable]
	public partial class EndiciaAccountEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public EndiciaAccountEntityFactory() : base("EndiciaAccountEntity", ShipWorks.Data.Model.EntityType.EndiciaAccountEntity) { }

		/// <summary>Creates a new, empty EndiciaAccountEntity object.</summary>
		/// <returns>A new, empty EndiciaAccountEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new EndiciaAccountEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEndiciaAccount
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new EndiciaAccountEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EndiciaAccountEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEndiciaAccountUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<EndiciaAccountEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty EndiciaProfileEntity objects.</summary>
	[Serializable]
	public partial class EndiciaProfileEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public EndiciaProfileEntityFactory() : base("EndiciaProfileEntity", ShipWorks.Data.Model.EntityType.EndiciaProfileEntity) { }

		/// <summary>Creates a new, empty EndiciaProfileEntity object.</summary>
		/// <returns>A new, empty EndiciaProfileEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new EndiciaProfileEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEndiciaProfile
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new EndiciaProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EndiciaProfileEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEndiciaProfileUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<EndiciaProfileEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty EndiciaScanFormEntity objects.</summary>
	[Serializable]
	public partial class EndiciaScanFormEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public EndiciaScanFormEntityFactory() : base("EndiciaScanFormEntity", ShipWorks.Data.Model.EntityType.EndiciaScanFormEntity) { }

		/// <summary>Creates a new, empty EndiciaScanFormEntity object.</summary>
		/// <returns>A new, empty EndiciaScanFormEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new EndiciaScanFormEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEndiciaScanForm
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new EndiciaScanFormEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EndiciaScanFormEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEndiciaScanFormUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<EndiciaScanFormEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty EndiciaShipmentEntity objects.</summary>
	[Serializable]
	public partial class EndiciaShipmentEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public EndiciaShipmentEntityFactory() : base("EndiciaShipmentEntity", ShipWorks.Data.Model.EntityType.EndiciaShipmentEntity) { }

		/// <summary>Creates a new, empty EndiciaShipmentEntity object.</summary>
		/// <returns>A new, empty EndiciaShipmentEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new EndiciaShipmentEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEndiciaShipment
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new EndiciaShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EndiciaShipmentEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEndiciaShipmentUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<EndiciaShipmentEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty EtsyOrderEntity objects.</summary>
	[Serializable]
	public partial class EtsyOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public EtsyOrderEntityFactory() : base("EtsyOrderEntity", ShipWorks.Data.Model.EntityType.EtsyOrderEntity) { }

		/// <summary>Creates a new, empty EtsyOrderEntity object.</summary>
		/// <returns>A new, empty EtsyOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new EtsyOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEtsyOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new EtsyOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EtsyOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEtsyOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<EtsyOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("EtsyOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty EtsyStoreEntity objects.</summary>
	[Serializable]
	public partial class EtsyStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public EtsyStoreEntityFactory() : base("EtsyStoreEntity", ShipWorks.Data.Model.EntityType.EtsyStoreEntity) { }

		/// <summary>Creates a new, empty EtsyStoreEntity object.</summary>
		/// <returns>A new, empty EtsyStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new EtsyStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEtsyStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new EtsyStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new EtsyStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewEtsyStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<EtsyStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("EtsyStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ExcludedPackageTypeEntity objects.</summary>
	[Serializable]
	public partial class ExcludedPackageTypeEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ExcludedPackageTypeEntityFactory() : base("ExcludedPackageTypeEntity", ShipWorks.Data.Model.EntityType.ExcludedPackageTypeEntity) { }

		/// <summary>Creates a new, empty ExcludedPackageTypeEntity object.</summary>
		/// <returns>A new, empty ExcludedPackageTypeEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ExcludedPackageTypeEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewExcludedPackageType
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ExcludedPackageTypeEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ExcludedPackageTypeEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewExcludedPackageTypeUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ExcludedPackageTypeEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ExcludedServiceTypeEntity objects.</summary>
	[Serializable]
	public partial class ExcludedServiceTypeEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ExcludedServiceTypeEntityFactory() : base("ExcludedServiceTypeEntity", ShipWorks.Data.Model.EntityType.ExcludedServiceTypeEntity) { }

		/// <summary>Creates a new, empty ExcludedServiceTypeEntity object.</summary>
		/// <returns>A new, empty ExcludedServiceTypeEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ExcludedServiceTypeEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewExcludedServiceType
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ExcludedServiceTypeEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ExcludedServiceTypeEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewExcludedServiceTypeUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ExcludedServiceTypeEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty FedExAccountEntity objects.</summary>
	[Serializable]
	public partial class FedExAccountEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public FedExAccountEntityFactory() : base("FedExAccountEntity", ShipWorks.Data.Model.EntityType.FedExAccountEntity) { }

		/// <summary>Creates a new, empty FedExAccountEntity object.</summary>
		/// <returns>A new, empty FedExAccountEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new FedExAccountEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExAccount
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new FedExAccountEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FedExAccountEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExAccountUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<FedExAccountEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty FedExEndOfDayCloseEntity objects.</summary>
	[Serializable]
	public partial class FedExEndOfDayCloseEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public FedExEndOfDayCloseEntityFactory() : base("FedExEndOfDayCloseEntity", ShipWorks.Data.Model.EntityType.FedExEndOfDayCloseEntity) { }

		/// <summary>Creates a new, empty FedExEndOfDayCloseEntity object.</summary>
		/// <returns>A new, empty FedExEndOfDayCloseEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new FedExEndOfDayCloseEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExEndOfDayClose
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new FedExEndOfDayCloseEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FedExEndOfDayCloseEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExEndOfDayCloseUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<FedExEndOfDayCloseEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty FedExPackageEntity objects.</summary>
	[Serializable]
	public partial class FedExPackageEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public FedExPackageEntityFactory() : base("FedExPackageEntity", ShipWorks.Data.Model.EntityType.FedExPackageEntity) { }

		/// <summary>Creates a new, empty FedExPackageEntity object.</summary>
		/// <returns>A new, empty FedExPackageEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new FedExPackageEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExPackage
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new FedExPackageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FedExPackageEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExPackageUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<FedExPackageEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty FedExProfileEntity objects.</summary>
	[Serializable]
	public partial class FedExProfileEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public FedExProfileEntityFactory() : base("FedExProfileEntity", ShipWorks.Data.Model.EntityType.FedExProfileEntity) { }

		/// <summary>Creates a new, empty FedExProfileEntity object.</summary>
		/// <returns>A new, empty FedExProfileEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new FedExProfileEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExProfile
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new FedExProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FedExProfileEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExProfileUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<FedExProfileEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty FedExProfilePackageEntity objects.</summary>
	[Serializable]
	public partial class FedExProfilePackageEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public FedExProfilePackageEntityFactory() : base("FedExProfilePackageEntity", ShipWorks.Data.Model.EntityType.FedExProfilePackageEntity) { }

		/// <summary>Creates a new, empty FedExProfilePackageEntity object.</summary>
		/// <returns>A new, empty FedExProfilePackageEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new FedExProfilePackageEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExProfilePackage
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new FedExProfilePackageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FedExProfilePackageEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExProfilePackageUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<FedExProfilePackageEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty FedExShipmentEntity objects.</summary>
	[Serializable]
	public partial class FedExShipmentEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public FedExShipmentEntityFactory() : base("FedExShipmentEntity", ShipWorks.Data.Model.EntityType.FedExShipmentEntity) { }

		/// <summary>Creates a new, empty FedExShipmentEntity object.</summary>
		/// <returns>A new, empty FedExShipmentEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new FedExShipmentEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExShipment
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new FedExShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FedExShipmentEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFedExShipmentUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<FedExShipmentEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty FilterEntity objects.</summary>
	[Serializable]
	public partial class FilterEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public FilterEntityFactory() : base("FilterEntity", ShipWorks.Data.Model.EntityType.FilterEntity) { }

		/// <summary>Creates a new, empty FilterEntity object.</summary>
		/// <returns>A new, empty FilterEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new FilterEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilter
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new FilterEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FilterEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<FilterEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty FilterLayoutEntity objects.</summary>
	[Serializable]
	public partial class FilterLayoutEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public FilterLayoutEntityFactory() : base("FilterLayoutEntity", ShipWorks.Data.Model.EntityType.FilterLayoutEntity) { }

		/// <summary>Creates a new, empty FilterLayoutEntity object.</summary>
		/// <returns>A new, empty FilterLayoutEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new FilterLayoutEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterLayout
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new FilterLayoutEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FilterLayoutEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterLayoutUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<FilterLayoutEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty FilterNodeEntity objects.</summary>
	[Serializable]
	public partial class FilterNodeEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public FilterNodeEntityFactory() : base("FilterNodeEntity", ShipWorks.Data.Model.EntityType.FilterNodeEntity) { }

		/// <summary>Creates a new, empty FilterNodeEntity object.</summary>
		/// <returns>A new, empty FilterNodeEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new FilterNodeEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterNode
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new FilterNodeEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FilterNodeEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterNodeUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<FilterNodeEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty FilterNodeColumnSettingsEntity objects.</summary>
	[Serializable]
	public partial class FilterNodeColumnSettingsEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public FilterNodeColumnSettingsEntityFactory() : base("FilterNodeColumnSettingsEntity", ShipWorks.Data.Model.EntityType.FilterNodeColumnSettingsEntity) { }

		/// <summary>Creates a new, empty FilterNodeColumnSettingsEntity object.</summary>
		/// <returns>A new, empty FilterNodeColumnSettingsEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new FilterNodeColumnSettingsEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterNodeColumnSettings
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new FilterNodeColumnSettingsEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FilterNodeColumnSettingsEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterNodeColumnSettingsUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<FilterNodeColumnSettingsEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty FilterNodeContentEntity objects.</summary>
	[Serializable]
	public partial class FilterNodeContentEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public FilterNodeContentEntityFactory() : base("FilterNodeContentEntity", ShipWorks.Data.Model.EntityType.FilterNodeContentEntity) { }

		/// <summary>Creates a new, empty FilterNodeContentEntity object.</summary>
		/// <returns>A new, empty FilterNodeContentEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new FilterNodeContentEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterNodeContent
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new FilterNodeContentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FilterNodeContentEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterNodeContentUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<FilterNodeContentEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty FilterNodeContentDetailEntity objects.</summary>
	[Serializable]
	public partial class FilterNodeContentDetailEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public FilterNodeContentDetailEntityFactory() : base("FilterNodeContentDetailEntity", ShipWorks.Data.Model.EntityType.FilterNodeContentDetailEntity) { }

		/// <summary>Creates a new, empty FilterNodeContentDetailEntity object.</summary>
		/// <returns>A new, empty FilterNodeContentDetailEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new FilterNodeContentDetailEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterNodeContentDetail
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new FilterNodeContentDetailEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FilterNodeContentDetailEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterNodeContentDetailUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<FilterNodeContentDetailEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty FilterSequenceEntity objects.</summary>
	[Serializable]
	public partial class FilterSequenceEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public FilterSequenceEntityFactory() : base("FilterSequenceEntity", ShipWorks.Data.Model.EntityType.FilterSequenceEntity) { }

		/// <summary>Creates a new, empty FilterSequenceEntity object.</summary>
		/// <returns>A new, empty FilterSequenceEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new FilterSequenceEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterSequence
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new FilterSequenceEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FilterSequenceEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFilterSequenceUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<FilterSequenceEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty FtpAccountEntity objects.</summary>
	[Serializable]
	public partial class FtpAccountEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public FtpAccountEntityFactory() : base("FtpAccountEntity", ShipWorks.Data.Model.EntityType.FtpAccountEntity) { }

		/// <summary>Creates a new, empty FtpAccountEntity object.</summary>
		/// <returns>A new, empty FtpAccountEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new FtpAccountEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFtpAccount
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new FtpAccountEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new FtpAccountEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewFtpAccountUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<FtpAccountEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty GenericFileStoreEntity objects.</summary>
	[Serializable]
	public partial class GenericFileStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public GenericFileStoreEntityFactory() : base("GenericFileStoreEntity", ShipWorks.Data.Model.EntityType.GenericFileStoreEntity) { }

		/// <summary>Creates a new, empty GenericFileStoreEntity object.</summary>
		/// <returns>A new, empty GenericFileStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new GenericFileStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGenericFileStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new GenericFileStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GenericFileStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGenericFileStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<GenericFileStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("GenericFileStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty GenericModuleStoreEntity objects.</summary>
	[Serializable]
	public partial class GenericModuleStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public GenericModuleStoreEntityFactory() : base("GenericModuleStoreEntity", ShipWorks.Data.Model.EntityType.GenericModuleStoreEntity) { }

		/// <summary>Creates a new, empty GenericModuleStoreEntity object.</summary>
		/// <returns>A new, empty GenericModuleStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new GenericModuleStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGenericModuleStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new GenericModuleStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GenericModuleStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGenericModuleStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<GenericModuleStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("GenericModuleStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty GridColumnFormatEntity objects.</summary>
	[Serializable]
	public partial class GridColumnFormatEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public GridColumnFormatEntityFactory() : base("GridColumnFormatEntity", ShipWorks.Data.Model.EntityType.GridColumnFormatEntity) { }

		/// <summary>Creates a new, empty GridColumnFormatEntity object.</summary>
		/// <returns>A new, empty GridColumnFormatEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new GridColumnFormatEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGridColumnFormat
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new GridColumnFormatEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GridColumnFormatEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGridColumnFormatUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<GridColumnFormatEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty GridColumnLayoutEntity objects.</summary>
	[Serializable]
	public partial class GridColumnLayoutEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public GridColumnLayoutEntityFactory() : base("GridColumnLayoutEntity", ShipWorks.Data.Model.EntityType.GridColumnLayoutEntity) { }

		/// <summary>Creates a new, empty GridColumnLayoutEntity object.</summary>
		/// <returns>A new, empty GridColumnLayoutEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new GridColumnLayoutEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGridColumnLayout
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new GridColumnLayoutEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GridColumnLayoutEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGridColumnLayoutUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<GridColumnLayoutEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty GridColumnPositionEntity objects.</summary>
	[Serializable]
	public partial class GridColumnPositionEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public GridColumnPositionEntityFactory() : base("GridColumnPositionEntity", ShipWorks.Data.Model.EntityType.GridColumnPositionEntity) { }

		/// <summary>Creates a new, empty GridColumnPositionEntity object.</summary>
		/// <returns>A new, empty GridColumnPositionEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new GridColumnPositionEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGridColumnPosition
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new GridColumnPositionEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GridColumnPositionEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGridColumnPositionUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<GridColumnPositionEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty GrouponOrderEntity objects.</summary>
	[Serializable]
	public partial class GrouponOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public GrouponOrderEntityFactory() : base("GrouponOrderEntity", ShipWorks.Data.Model.EntityType.GrouponOrderEntity) { }

		/// <summary>Creates a new, empty GrouponOrderEntity object.</summary>
		/// <returns>A new, empty GrouponOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new GrouponOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGrouponOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new GrouponOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GrouponOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGrouponOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<GrouponOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("GrouponOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty GrouponOrderItemEntity objects.</summary>
	[Serializable]
	public partial class GrouponOrderItemEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public GrouponOrderItemEntityFactory() : base("GrouponOrderItemEntity", ShipWorks.Data.Model.EntityType.GrouponOrderItemEntity) { }

		/// <summary>Creates a new, empty GrouponOrderItemEntity object.</summary>
		/// <returns>A new, empty GrouponOrderItemEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new GrouponOrderItemEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGrouponOrderItem
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new GrouponOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GrouponOrderItemEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGrouponOrderItemUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<GrouponOrderItemEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("GrouponOrderItemEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty GrouponStoreEntity objects.</summary>
	[Serializable]
	public partial class GrouponStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public GrouponStoreEntityFactory() : base("GrouponStoreEntity", ShipWorks.Data.Model.EntityType.GrouponStoreEntity) { }

		/// <summary>Creates a new, empty GrouponStoreEntity object.</summary>
		/// <returns>A new, empty GrouponStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new GrouponStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGrouponStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new GrouponStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new GrouponStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewGrouponStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<GrouponStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("GrouponStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty InfopiaOrderItemEntity objects.</summary>
	[Serializable]
	public partial class InfopiaOrderItemEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public InfopiaOrderItemEntityFactory() : base("InfopiaOrderItemEntity", ShipWorks.Data.Model.EntityType.InfopiaOrderItemEntity) { }

		/// <summary>Creates a new, empty InfopiaOrderItemEntity object.</summary>
		/// <returns>A new, empty InfopiaOrderItemEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new InfopiaOrderItemEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewInfopiaOrderItem
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new InfopiaOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new InfopiaOrderItemEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewInfopiaOrderItemUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<InfopiaOrderItemEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("InfopiaOrderItemEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty InfopiaStoreEntity objects.</summary>
	[Serializable]
	public partial class InfopiaStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public InfopiaStoreEntityFactory() : base("InfopiaStoreEntity", ShipWorks.Data.Model.EntityType.InfopiaStoreEntity) { }

		/// <summary>Creates a new, empty InfopiaStoreEntity object.</summary>
		/// <returns>A new, empty InfopiaStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new InfopiaStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewInfopiaStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new InfopiaStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new InfopiaStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewInfopiaStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<InfopiaStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("InfopiaStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty InsurancePolicyEntity objects.</summary>
	[Serializable]
	public partial class InsurancePolicyEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public InsurancePolicyEntityFactory() : base("InsurancePolicyEntity", ShipWorks.Data.Model.EntityType.InsurancePolicyEntity) { }

		/// <summary>Creates a new, empty InsurancePolicyEntity object.</summary>
		/// <returns>A new, empty InsurancePolicyEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new InsurancePolicyEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewInsurancePolicy
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new InsurancePolicyEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new InsurancePolicyEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewInsurancePolicyUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<InsurancePolicyEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty IParcelAccountEntity objects.</summary>
	[Serializable]
	public partial class IParcelAccountEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public IParcelAccountEntityFactory() : base("IParcelAccountEntity", ShipWorks.Data.Model.EntityType.IParcelAccountEntity) { }

		/// <summary>Creates a new, empty IParcelAccountEntity object.</summary>
		/// <returns>A new, empty IParcelAccountEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new IParcelAccountEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewIParcelAccount
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new IParcelAccountEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new IParcelAccountEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewIParcelAccountUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<IParcelAccountEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty IParcelPackageEntity objects.</summary>
	[Serializable]
	public partial class IParcelPackageEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public IParcelPackageEntityFactory() : base("IParcelPackageEntity", ShipWorks.Data.Model.EntityType.IParcelPackageEntity) { }

		/// <summary>Creates a new, empty IParcelPackageEntity object.</summary>
		/// <returns>A new, empty IParcelPackageEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new IParcelPackageEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewIParcelPackage
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new IParcelPackageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new IParcelPackageEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewIParcelPackageUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<IParcelPackageEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty IParcelProfileEntity objects.</summary>
	[Serializable]
	public partial class IParcelProfileEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public IParcelProfileEntityFactory() : base("IParcelProfileEntity", ShipWorks.Data.Model.EntityType.IParcelProfileEntity) { }

		/// <summary>Creates a new, empty IParcelProfileEntity object.</summary>
		/// <returns>A new, empty IParcelProfileEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new IParcelProfileEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewIParcelProfile
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new IParcelProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new IParcelProfileEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewIParcelProfileUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<IParcelProfileEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty IParcelProfilePackageEntity objects.</summary>
	[Serializable]
	public partial class IParcelProfilePackageEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public IParcelProfilePackageEntityFactory() : base("IParcelProfilePackageEntity", ShipWorks.Data.Model.EntityType.IParcelProfilePackageEntity) { }

		/// <summary>Creates a new, empty IParcelProfilePackageEntity object.</summary>
		/// <returns>A new, empty IParcelProfilePackageEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new IParcelProfilePackageEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewIParcelProfilePackage
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new IParcelProfilePackageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new IParcelProfilePackageEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewIParcelProfilePackageUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<IParcelProfilePackageEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty IParcelShipmentEntity objects.</summary>
	[Serializable]
	public partial class IParcelShipmentEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public IParcelShipmentEntityFactory() : base("IParcelShipmentEntity", ShipWorks.Data.Model.EntityType.IParcelShipmentEntity) { }

		/// <summary>Creates a new, empty IParcelShipmentEntity object.</summary>
		/// <returns>A new, empty IParcelShipmentEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new IParcelShipmentEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewIParcelShipment
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new IParcelShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new IParcelShipmentEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewIParcelShipmentUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<IParcelShipmentEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty LabelSheetEntity objects.</summary>
	[Serializable]
	public partial class LabelSheetEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public LabelSheetEntityFactory() : base("LabelSheetEntity", ShipWorks.Data.Model.EntityType.LabelSheetEntity) { }

		/// <summary>Creates a new, empty LabelSheetEntity object.</summary>
		/// <returns>A new, empty LabelSheetEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new LabelSheetEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewLabelSheet
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new LabelSheetEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new LabelSheetEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewLabelSheetUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<LabelSheetEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty LemonStandOrderEntity objects.</summary>
	[Serializable]
	public partial class LemonStandOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public LemonStandOrderEntityFactory() : base("LemonStandOrderEntity", ShipWorks.Data.Model.EntityType.LemonStandOrderEntity) { }

		/// <summary>Creates a new, empty LemonStandOrderEntity object.</summary>
		/// <returns>A new, empty LemonStandOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new LemonStandOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewLemonStandOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new LemonStandOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new LemonStandOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewLemonStandOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<LemonStandOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("LemonStandOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty LemonStandOrderItemEntity objects.</summary>
	[Serializable]
	public partial class LemonStandOrderItemEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public LemonStandOrderItemEntityFactory() : base("LemonStandOrderItemEntity", ShipWorks.Data.Model.EntityType.LemonStandOrderItemEntity) { }

		/// <summary>Creates a new, empty LemonStandOrderItemEntity object.</summary>
		/// <returns>A new, empty LemonStandOrderItemEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new LemonStandOrderItemEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewLemonStandOrderItem
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new LemonStandOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new LemonStandOrderItemEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewLemonStandOrderItemUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<LemonStandOrderItemEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("LemonStandOrderItemEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty LemonStandStoreEntity objects.</summary>
	[Serializable]
	public partial class LemonStandStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public LemonStandStoreEntityFactory() : base("LemonStandStoreEntity", ShipWorks.Data.Model.EntityType.LemonStandStoreEntity) { }

		/// <summary>Creates a new, empty LemonStandStoreEntity object.</summary>
		/// <returns>A new, empty LemonStandStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new LemonStandStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewLemonStandStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new LemonStandStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new LemonStandStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewLemonStandStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<LemonStandStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("LemonStandStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty MagentoOrderEntity objects.</summary>
	[Serializable]
	public partial class MagentoOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public MagentoOrderEntityFactory() : base("MagentoOrderEntity", ShipWorks.Data.Model.EntityType.MagentoOrderEntity) { }

		/// <summary>Creates a new, empty MagentoOrderEntity object.</summary>
		/// <returns>A new, empty MagentoOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new MagentoOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewMagentoOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new MagentoOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new MagentoOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewMagentoOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<MagentoOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("MagentoOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty MagentoStoreEntity objects.</summary>
	[Serializable]
	public partial class MagentoStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public MagentoStoreEntityFactory() : base("MagentoStoreEntity", ShipWorks.Data.Model.EntityType.MagentoStoreEntity) { }

		/// <summary>Creates a new, empty MagentoStoreEntity object.</summary>
		/// <returns>A new, empty MagentoStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new MagentoStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewMagentoStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new MagentoStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new MagentoStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewMagentoStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<MagentoStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("MagentoStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty MarketplaceAdvisorOrderEntity objects.</summary>
	[Serializable]
	public partial class MarketplaceAdvisorOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public MarketplaceAdvisorOrderEntityFactory() : base("MarketplaceAdvisorOrderEntity", ShipWorks.Data.Model.EntityType.MarketplaceAdvisorOrderEntity) { }

		/// <summary>Creates a new, empty MarketplaceAdvisorOrderEntity object.</summary>
		/// <returns>A new, empty MarketplaceAdvisorOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new MarketplaceAdvisorOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewMarketplaceAdvisorOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new MarketplaceAdvisorOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new MarketplaceAdvisorOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewMarketplaceAdvisorOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<MarketplaceAdvisorOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("MarketplaceAdvisorOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty MarketplaceAdvisorStoreEntity objects.</summary>
	[Serializable]
	public partial class MarketplaceAdvisorStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public MarketplaceAdvisorStoreEntityFactory() : base("MarketplaceAdvisorStoreEntity", ShipWorks.Data.Model.EntityType.MarketplaceAdvisorStoreEntity) { }

		/// <summary>Creates a new, empty MarketplaceAdvisorStoreEntity object.</summary>
		/// <returns>A new, empty MarketplaceAdvisorStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new MarketplaceAdvisorStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewMarketplaceAdvisorStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new MarketplaceAdvisorStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new MarketplaceAdvisorStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewMarketplaceAdvisorStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<MarketplaceAdvisorStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("MarketplaceAdvisorStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty MivaOrderItemAttributeEntity objects.</summary>
	[Serializable]
	public partial class MivaOrderItemAttributeEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public MivaOrderItemAttributeEntityFactory() : base("MivaOrderItemAttributeEntity", ShipWorks.Data.Model.EntityType.MivaOrderItemAttributeEntity) { }

		/// <summary>Creates a new, empty MivaOrderItemAttributeEntity object.</summary>
		/// <returns>A new, empty MivaOrderItemAttributeEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new MivaOrderItemAttributeEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewMivaOrderItemAttribute
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new MivaOrderItemAttributeEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new MivaOrderItemAttributeEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewMivaOrderItemAttributeUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<MivaOrderItemAttributeEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("MivaOrderItemAttributeEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty MivaStoreEntity objects.</summary>
	[Serializable]
	public partial class MivaStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public MivaStoreEntityFactory() : base("MivaStoreEntity", ShipWorks.Data.Model.EntityType.MivaStoreEntity) { }

		/// <summary>Creates a new, empty MivaStoreEntity object.</summary>
		/// <returns>A new, empty MivaStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new MivaStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewMivaStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new MivaStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new MivaStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewMivaStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<MivaStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("MivaStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty NetworkSolutionsOrderEntity objects.</summary>
	[Serializable]
	public partial class NetworkSolutionsOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public NetworkSolutionsOrderEntityFactory() : base("NetworkSolutionsOrderEntity", ShipWorks.Data.Model.EntityType.NetworkSolutionsOrderEntity) { }

		/// <summary>Creates a new, empty NetworkSolutionsOrderEntity object.</summary>
		/// <returns>A new, empty NetworkSolutionsOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new NetworkSolutionsOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewNetworkSolutionsOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new NetworkSolutionsOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new NetworkSolutionsOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewNetworkSolutionsOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<NetworkSolutionsOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("NetworkSolutionsOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty NetworkSolutionsStoreEntity objects.</summary>
	[Serializable]
	public partial class NetworkSolutionsStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public NetworkSolutionsStoreEntityFactory() : base("NetworkSolutionsStoreEntity", ShipWorks.Data.Model.EntityType.NetworkSolutionsStoreEntity) { }

		/// <summary>Creates a new, empty NetworkSolutionsStoreEntity object.</summary>
		/// <returns>A new, empty NetworkSolutionsStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new NetworkSolutionsStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewNetworkSolutionsStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new NetworkSolutionsStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new NetworkSolutionsStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewNetworkSolutionsStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<NetworkSolutionsStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("NetworkSolutionsStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty NeweggOrderEntity objects.</summary>
	[Serializable]
	public partial class NeweggOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public NeweggOrderEntityFactory() : base("NeweggOrderEntity", ShipWorks.Data.Model.EntityType.NeweggOrderEntity) { }

		/// <summary>Creates a new, empty NeweggOrderEntity object.</summary>
		/// <returns>A new, empty NeweggOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new NeweggOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewNeweggOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new NeweggOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new NeweggOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewNeweggOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<NeweggOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("NeweggOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty NeweggOrderItemEntity objects.</summary>
	[Serializable]
	public partial class NeweggOrderItemEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public NeweggOrderItemEntityFactory() : base("NeweggOrderItemEntity", ShipWorks.Data.Model.EntityType.NeweggOrderItemEntity) { }

		/// <summary>Creates a new, empty NeweggOrderItemEntity object.</summary>
		/// <returns>A new, empty NeweggOrderItemEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new NeweggOrderItemEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewNeweggOrderItem
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new NeweggOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new NeweggOrderItemEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewNeweggOrderItemUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<NeweggOrderItemEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("NeweggOrderItemEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty NeweggStoreEntity objects.</summary>
	[Serializable]
	public partial class NeweggStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public NeweggStoreEntityFactory() : base("NeweggStoreEntity", ShipWorks.Data.Model.EntityType.NeweggStoreEntity) { }

		/// <summary>Creates a new, empty NeweggStoreEntity object.</summary>
		/// <returns>A new, empty NeweggStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new NeweggStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewNeweggStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new NeweggStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new NeweggStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewNeweggStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<NeweggStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("NeweggStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty NoteEntity objects.</summary>
	[Serializable]
	public partial class NoteEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public NoteEntityFactory() : base("NoteEntity", ShipWorks.Data.Model.EntityType.NoteEntity) { }

		/// <summary>Creates a new, empty NoteEntity object.</summary>
		/// <returns>A new, empty NoteEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new NoteEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewNote
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new NoteEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new NoteEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewNoteUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<NoteEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ObjectLabelEntity objects.</summary>
	[Serializable]
	public partial class ObjectLabelEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ObjectLabelEntityFactory() : base("ObjectLabelEntity", ShipWorks.Data.Model.EntityType.ObjectLabelEntity) { }

		/// <summary>Creates a new, empty ObjectLabelEntity object.</summary>
		/// <returns>A new, empty ObjectLabelEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ObjectLabelEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewObjectLabel
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ObjectLabelEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ObjectLabelEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewObjectLabelUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ObjectLabelEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ObjectReferenceEntity objects.</summary>
	[Serializable]
	public partial class ObjectReferenceEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ObjectReferenceEntityFactory() : base("ObjectReferenceEntity", ShipWorks.Data.Model.EntityType.ObjectReferenceEntity) { }

		/// <summary>Creates a new, empty ObjectReferenceEntity object.</summary>
		/// <returns>A new, empty ObjectReferenceEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ObjectReferenceEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewObjectReference
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ObjectReferenceEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ObjectReferenceEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewObjectReferenceUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ObjectReferenceEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty OnTracAccountEntity objects.</summary>
	[Serializable]
	public partial class OnTracAccountEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public OnTracAccountEntityFactory() : base("OnTracAccountEntity", ShipWorks.Data.Model.EntityType.OnTracAccountEntity) { }

		/// <summary>Creates a new, empty OnTracAccountEntity object.</summary>
		/// <returns>A new, empty OnTracAccountEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new OnTracAccountEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOnTracAccount
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new OnTracAccountEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OnTracAccountEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOnTracAccountUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<OnTracAccountEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty OnTracProfileEntity objects.</summary>
	[Serializable]
	public partial class OnTracProfileEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public OnTracProfileEntityFactory() : base("OnTracProfileEntity", ShipWorks.Data.Model.EntityType.OnTracProfileEntity) { }

		/// <summary>Creates a new, empty OnTracProfileEntity object.</summary>
		/// <returns>A new, empty OnTracProfileEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new OnTracProfileEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOnTracProfile
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new OnTracProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OnTracProfileEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOnTracProfileUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<OnTracProfileEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty OnTracShipmentEntity objects.</summary>
	[Serializable]
	public partial class OnTracShipmentEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public OnTracShipmentEntityFactory() : base("OnTracShipmentEntity", ShipWorks.Data.Model.EntityType.OnTracShipmentEntity) { }

		/// <summary>Creates a new, empty OnTracShipmentEntity object.</summary>
		/// <returns>A new, empty OnTracShipmentEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new OnTracShipmentEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOnTracShipment
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new OnTracShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OnTracShipmentEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOnTracShipmentUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<OnTracShipmentEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty OrderEntity objects.</summary>
	[Serializable]
	public partial class OrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public OrderEntityFactory() : base("OrderEntity", ShipWorks.Data.Model.EntityType.OrderEntity) { }

		/// <summary>Creates a new, empty OrderEntity object.</summary>
		/// <returns>A new, empty OrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new OrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new OrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<OrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("OrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty OrderChargeEntity objects.</summary>
	[Serializable]
	public partial class OrderChargeEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public OrderChargeEntityFactory() : base("OrderChargeEntity", ShipWorks.Data.Model.EntityType.OrderChargeEntity) { }

		/// <summary>Creates a new, empty OrderChargeEntity object.</summary>
		/// <returns>A new, empty OrderChargeEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new OrderChargeEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderCharge
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new OrderChargeEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OrderChargeEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderChargeUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<OrderChargeEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty OrderItemEntity objects.</summary>
	[Serializable]
	public partial class OrderItemEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public OrderItemEntityFactory() : base("OrderItemEntity", ShipWorks.Data.Model.EntityType.OrderItemEntity) { }

		/// <summary>Creates a new, empty OrderItemEntity object.</summary>
		/// <returns>A new, empty OrderItemEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new OrderItemEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderItem
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new OrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OrderItemEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderItemUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<OrderItemEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("OrderItemEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty OrderItemAttributeEntity objects.</summary>
	[Serializable]
	public partial class OrderItemAttributeEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public OrderItemAttributeEntityFactory() : base("OrderItemAttributeEntity", ShipWorks.Data.Model.EntityType.OrderItemAttributeEntity) { }

		/// <summary>Creates a new, empty OrderItemAttributeEntity object.</summary>
		/// <returns>A new, empty OrderItemAttributeEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new OrderItemAttributeEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderItemAttribute
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new OrderItemAttributeEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OrderItemAttributeEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderItemAttributeUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<OrderItemAttributeEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("OrderItemAttributeEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty OrderMotionOrderEntity objects.</summary>
	[Serializable]
	public partial class OrderMotionOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public OrderMotionOrderEntityFactory() : base("OrderMotionOrderEntity", ShipWorks.Data.Model.EntityType.OrderMotionOrderEntity) { }

		/// <summary>Creates a new, empty OrderMotionOrderEntity object.</summary>
		/// <returns>A new, empty OrderMotionOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new OrderMotionOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderMotionOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new OrderMotionOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OrderMotionOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderMotionOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<OrderMotionOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("OrderMotionOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty OrderMotionStoreEntity objects.</summary>
	[Serializable]
	public partial class OrderMotionStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public OrderMotionStoreEntityFactory() : base("OrderMotionStoreEntity", ShipWorks.Data.Model.EntityType.OrderMotionStoreEntity) { }

		/// <summary>Creates a new, empty OrderMotionStoreEntity object.</summary>
		/// <returns>A new, empty OrderMotionStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new OrderMotionStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderMotionStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new OrderMotionStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OrderMotionStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderMotionStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<OrderMotionStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("OrderMotionStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty OrderPaymentDetailEntity objects.</summary>
	[Serializable]
	public partial class OrderPaymentDetailEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public OrderPaymentDetailEntityFactory() : base("OrderPaymentDetailEntity", ShipWorks.Data.Model.EntityType.OrderPaymentDetailEntity) { }

		/// <summary>Creates a new, empty OrderPaymentDetailEntity object.</summary>
		/// <returns>A new, empty OrderPaymentDetailEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new OrderPaymentDetailEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderPaymentDetail
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new OrderPaymentDetailEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OrderPaymentDetailEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOrderPaymentDetailUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<OrderPaymentDetailEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty OtherProfileEntity objects.</summary>
	[Serializable]
	public partial class OtherProfileEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public OtherProfileEntityFactory() : base("OtherProfileEntity", ShipWorks.Data.Model.EntityType.OtherProfileEntity) { }

		/// <summary>Creates a new, empty OtherProfileEntity object.</summary>
		/// <returns>A new, empty OtherProfileEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new OtherProfileEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOtherProfile
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new OtherProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OtherProfileEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOtherProfileUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<OtherProfileEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty OtherShipmentEntity objects.</summary>
	[Serializable]
	public partial class OtherShipmentEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public OtherShipmentEntityFactory() : base("OtherShipmentEntity", ShipWorks.Data.Model.EntityType.OtherShipmentEntity) { }

		/// <summary>Creates a new, empty OtherShipmentEntity object.</summary>
		/// <returns>A new, empty OtherShipmentEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new OtherShipmentEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOtherShipment
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new OtherShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new OtherShipmentEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewOtherShipmentUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<OtherShipmentEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty PayPalOrderEntity objects.</summary>
	[Serializable]
	public partial class PayPalOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public PayPalOrderEntityFactory() : base("PayPalOrderEntity", ShipWorks.Data.Model.EntityType.PayPalOrderEntity) { }

		/// <summary>Creates a new, empty PayPalOrderEntity object.</summary>
		/// <returns>A new, empty PayPalOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new PayPalOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewPayPalOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new PayPalOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new PayPalOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewPayPalOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<PayPalOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("PayPalOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty PayPalStoreEntity objects.</summary>
	[Serializable]
	public partial class PayPalStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public PayPalStoreEntityFactory() : base("PayPalStoreEntity", ShipWorks.Data.Model.EntityType.PayPalStoreEntity) { }

		/// <summary>Creates a new, empty PayPalStoreEntity object.</summary>
		/// <returns>A new, empty PayPalStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new PayPalStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewPayPalStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new PayPalStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new PayPalStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewPayPalStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<PayPalStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("PayPalStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty PermissionEntity objects.</summary>
	[Serializable]
	public partial class PermissionEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public PermissionEntityFactory() : base("PermissionEntity", ShipWorks.Data.Model.EntityType.PermissionEntity) { }

		/// <summary>Creates a new, empty PermissionEntity object.</summary>
		/// <returns>A new, empty PermissionEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new PermissionEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewPermission
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new PermissionEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new PermissionEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewPermissionUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<PermissionEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty PostalProfileEntity objects.</summary>
	[Serializable]
	public partial class PostalProfileEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public PostalProfileEntityFactory() : base("PostalProfileEntity", ShipWorks.Data.Model.EntityType.PostalProfileEntity) { }

		/// <summary>Creates a new, empty PostalProfileEntity object.</summary>
		/// <returns>A new, empty PostalProfileEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new PostalProfileEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewPostalProfile
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new PostalProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new PostalProfileEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewPostalProfileUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<PostalProfileEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty PostalShipmentEntity objects.</summary>
	[Serializable]
	public partial class PostalShipmentEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public PostalShipmentEntityFactory() : base("PostalShipmentEntity", ShipWorks.Data.Model.EntityType.PostalShipmentEntity) { }

		/// <summary>Creates a new, empty PostalShipmentEntity object.</summary>
		/// <returns>A new, empty PostalShipmentEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new PostalShipmentEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewPostalShipment
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new PostalShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new PostalShipmentEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewPostalShipmentUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<PostalShipmentEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty PrintResultEntity objects.</summary>
	[Serializable]
	public partial class PrintResultEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public PrintResultEntityFactory() : base("PrintResultEntity", ShipWorks.Data.Model.EntityType.PrintResultEntity) { }

		/// <summary>Creates a new, empty PrintResultEntity object.</summary>
		/// <returns>A new, empty PrintResultEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new PrintResultEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewPrintResult
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new PrintResultEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new PrintResultEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewPrintResultUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<PrintResultEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ProStoresOrderEntity objects.</summary>
	[Serializable]
	public partial class ProStoresOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ProStoresOrderEntityFactory() : base("ProStoresOrderEntity", ShipWorks.Data.Model.EntityType.ProStoresOrderEntity) { }

		/// <summary>Creates a new, empty ProStoresOrderEntity object.</summary>
		/// <returns>A new, empty ProStoresOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ProStoresOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewProStoresOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ProStoresOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ProStoresOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewProStoresOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ProStoresOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("ProStoresOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ProStoresStoreEntity objects.</summary>
	[Serializable]
	public partial class ProStoresStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ProStoresStoreEntityFactory() : base("ProStoresStoreEntity", ShipWorks.Data.Model.EntityType.ProStoresStoreEntity) { }

		/// <summary>Creates a new, empty ProStoresStoreEntity object.</summary>
		/// <returns>A new, empty ProStoresStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ProStoresStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewProStoresStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ProStoresStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ProStoresStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewProStoresStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ProStoresStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("ProStoresStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ResourceEntity objects.</summary>
	[Serializable]
	public partial class ResourceEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ResourceEntityFactory() : base("ResourceEntity", ShipWorks.Data.Model.EntityType.ResourceEntity) { }

		/// <summary>Creates a new, empty ResourceEntity object.</summary>
		/// <returns>A new, empty ResourceEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ResourceEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewResource
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ResourceEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ResourceEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewResourceUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ResourceEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ScanFormBatchEntity objects.</summary>
	[Serializable]
	public partial class ScanFormBatchEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ScanFormBatchEntityFactory() : base("ScanFormBatchEntity", ShipWorks.Data.Model.EntityType.ScanFormBatchEntity) { }

		/// <summary>Creates a new, empty ScanFormBatchEntity object.</summary>
		/// <returns>A new, empty ScanFormBatchEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ScanFormBatchEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewScanFormBatch
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ScanFormBatchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ScanFormBatchEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewScanFormBatchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ScanFormBatchEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty SearchEntity objects.</summary>
	[Serializable]
	public partial class SearchEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public SearchEntityFactory() : base("SearchEntity", ShipWorks.Data.Model.EntityType.SearchEntity) { }

		/// <summary>Creates a new, empty SearchEntity object.</summary>
		/// <returns>A new, empty SearchEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new SearchEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSearch
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new SearchEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SearchEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSearchUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<SearchEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty SearsOrderEntity objects.</summary>
	[Serializable]
	public partial class SearsOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public SearsOrderEntityFactory() : base("SearsOrderEntity", ShipWorks.Data.Model.EntityType.SearsOrderEntity) { }

		/// <summary>Creates a new, empty SearsOrderEntity object.</summary>
		/// <returns>A new, empty SearsOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new SearsOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSearsOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new SearsOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SearsOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSearsOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<SearsOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("SearsOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty SearsOrderItemEntity objects.</summary>
	[Serializable]
	public partial class SearsOrderItemEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public SearsOrderItemEntityFactory() : base("SearsOrderItemEntity", ShipWorks.Data.Model.EntityType.SearsOrderItemEntity) { }

		/// <summary>Creates a new, empty SearsOrderItemEntity object.</summary>
		/// <returns>A new, empty SearsOrderItemEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new SearsOrderItemEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSearsOrderItem
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new SearsOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SearsOrderItemEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSearsOrderItemUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<SearsOrderItemEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("SearsOrderItemEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty SearsStoreEntity objects.</summary>
	[Serializable]
	public partial class SearsStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public SearsStoreEntityFactory() : base("SearsStoreEntity", ShipWorks.Data.Model.EntityType.SearsStoreEntity) { }

		/// <summary>Creates a new, empty SearsStoreEntity object.</summary>
		/// <returns>A new, empty SearsStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new SearsStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSearsStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new SearsStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SearsStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSearsStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<SearsStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("SearsStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ServerMessageEntity objects.</summary>
	[Serializable]
	public partial class ServerMessageEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ServerMessageEntityFactory() : base("ServerMessageEntity", ShipWorks.Data.Model.EntityType.ServerMessageEntity) { }

		/// <summary>Creates a new, empty ServerMessageEntity object.</summary>
		/// <returns>A new, empty ServerMessageEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ServerMessageEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewServerMessage
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ServerMessageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ServerMessageEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewServerMessageUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ServerMessageEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ServerMessageSignoffEntity objects.</summary>
	[Serializable]
	public partial class ServerMessageSignoffEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ServerMessageSignoffEntityFactory() : base("ServerMessageSignoffEntity", ShipWorks.Data.Model.EntityType.ServerMessageSignoffEntity) { }

		/// <summary>Creates a new, empty ServerMessageSignoffEntity object.</summary>
		/// <returns>A new, empty ServerMessageSignoffEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ServerMessageSignoffEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewServerMessageSignoff
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ServerMessageSignoffEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ServerMessageSignoffEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewServerMessageSignoffUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ServerMessageSignoffEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ServiceStatusEntity objects.</summary>
	[Serializable]
	public partial class ServiceStatusEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ServiceStatusEntityFactory() : base("ServiceStatusEntity", ShipWorks.Data.Model.EntityType.ServiceStatusEntity) { }

		/// <summary>Creates a new, empty ServiceStatusEntity object.</summary>
		/// <returns>A new, empty ServiceStatusEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ServiceStatusEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewServiceStatus
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ServiceStatusEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ServiceStatusEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewServiceStatusUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ServiceStatusEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ShipmentEntity objects.</summary>
	[Serializable]
	public partial class ShipmentEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ShipmentEntityFactory() : base("ShipmentEntity", ShipWorks.Data.Model.EntityType.ShipmentEntity) { }

		/// <summary>Creates a new, empty ShipmentEntity object.</summary>
		/// <returns>A new, empty ShipmentEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ShipmentEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShipment
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShipmentEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShipmentUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ShipmentEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ShipmentCustomsItemEntity objects.</summary>
	[Serializable]
	public partial class ShipmentCustomsItemEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ShipmentCustomsItemEntityFactory() : base("ShipmentCustomsItemEntity", ShipWorks.Data.Model.EntityType.ShipmentCustomsItemEntity) { }

		/// <summary>Creates a new, empty ShipmentCustomsItemEntity object.</summary>
		/// <returns>A new, empty ShipmentCustomsItemEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ShipmentCustomsItemEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShipmentCustomsItem
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ShipmentCustomsItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShipmentCustomsItemEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShipmentCustomsItemUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ShipmentCustomsItemEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ShippingDefaultsRuleEntity objects.</summary>
	[Serializable]
	public partial class ShippingDefaultsRuleEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ShippingDefaultsRuleEntityFactory() : base("ShippingDefaultsRuleEntity", ShipWorks.Data.Model.EntityType.ShippingDefaultsRuleEntity) { }

		/// <summary>Creates a new, empty ShippingDefaultsRuleEntity object.</summary>
		/// <returns>A new, empty ShippingDefaultsRuleEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ShippingDefaultsRuleEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingDefaultsRule
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ShippingDefaultsRuleEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShippingDefaultsRuleEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingDefaultsRuleUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ShippingDefaultsRuleEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ShippingOriginEntity objects.</summary>
	[Serializable]
	public partial class ShippingOriginEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ShippingOriginEntityFactory() : base("ShippingOriginEntity", ShipWorks.Data.Model.EntityType.ShippingOriginEntity) { }

		/// <summary>Creates a new, empty ShippingOriginEntity object.</summary>
		/// <returns>A new, empty ShippingOriginEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ShippingOriginEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingOrigin
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ShippingOriginEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShippingOriginEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingOriginUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ShippingOriginEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ShippingPrintOutputEntity objects.</summary>
	[Serializable]
	public partial class ShippingPrintOutputEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ShippingPrintOutputEntityFactory() : base("ShippingPrintOutputEntity", ShipWorks.Data.Model.EntityType.ShippingPrintOutputEntity) { }

		/// <summary>Creates a new, empty ShippingPrintOutputEntity object.</summary>
		/// <returns>A new, empty ShippingPrintOutputEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ShippingPrintOutputEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingPrintOutput
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ShippingPrintOutputEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShippingPrintOutputEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingPrintOutputUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ShippingPrintOutputEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ShippingPrintOutputRuleEntity objects.</summary>
	[Serializable]
	public partial class ShippingPrintOutputRuleEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ShippingPrintOutputRuleEntityFactory() : base("ShippingPrintOutputRuleEntity", ShipWorks.Data.Model.EntityType.ShippingPrintOutputRuleEntity) { }

		/// <summary>Creates a new, empty ShippingPrintOutputRuleEntity object.</summary>
		/// <returns>A new, empty ShippingPrintOutputRuleEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ShippingPrintOutputRuleEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingPrintOutputRule
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ShippingPrintOutputRuleEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShippingPrintOutputRuleEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingPrintOutputRuleUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ShippingPrintOutputRuleEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ShippingProfileEntity objects.</summary>
	[Serializable]
	public partial class ShippingProfileEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ShippingProfileEntityFactory() : base("ShippingProfileEntity", ShipWorks.Data.Model.EntityType.ShippingProfileEntity) { }

		/// <summary>Creates a new, empty ShippingProfileEntity object.</summary>
		/// <returns>A new, empty ShippingProfileEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ShippingProfileEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingProfile
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ShippingProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShippingProfileEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingProfileUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ShippingProfileEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ShippingProviderRuleEntity objects.</summary>
	[Serializable]
	public partial class ShippingProviderRuleEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ShippingProviderRuleEntityFactory() : base("ShippingProviderRuleEntity", ShipWorks.Data.Model.EntityType.ShippingProviderRuleEntity) { }

		/// <summary>Creates a new, empty ShippingProviderRuleEntity object.</summary>
		/// <returns>A new, empty ShippingProviderRuleEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ShippingProviderRuleEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingProviderRule
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ShippingProviderRuleEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShippingProviderRuleEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingProviderRuleUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ShippingProviderRuleEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ShippingSettingsEntity objects.</summary>
	[Serializable]
	public partial class ShippingSettingsEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ShippingSettingsEntityFactory() : base("ShippingSettingsEntity", ShipWorks.Data.Model.EntityType.ShippingSettingsEntity) { }

		/// <summary>Creates a new, empty ShippingSettingsEntity object.</summary>
		/// <returns>A new, empty ShippingSettingsEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ShippingSettingsEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingSettings
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ShippingSettingsEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShippingSettingsEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShippingSettingsUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ShippingSettingsEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ShipSenseKnowledgebaseEntity objects.</summary>
	[Serializable]
	public partial class ShipSenseKnowledgebaseEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ShipSenseKnowledgebaseEntityFactory() : base("ShipSenseKnowledgebaseEntity", ShipWorks.Data.Model.EntityType.ShipSenseKnowledgebaseEntity) { }

		/// <summary>Creates a new, empty ShipSenseKnowledgebaseEntity object.</summary>
		/// <returns>A new, empty ShipSenseKnowledgebaseEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ShipSenseKnowledgebaseEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShipSenseKnowledgebase
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ShipSenseKnowledgebaseEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShipSenseKnowledgebaseEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShipSenseKnowledgebaseUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ShipSenseKnowledgebaseEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ShopifyOrderEntity objects.</summary>
	[Serializable]
	public partial class ShopifyOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ShopifyOrderEntityFactory() : base("ShopifyOrderEntity", ShipWorks.Data.Model.EntityType.ShopifyOrderEntity) { }

		/// <summary>Creates a new, empty ShopifyOrderEntity object.</summary>
		/// <returns>A new, empty ShopifyOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ShopifyOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShopifyOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ShopifyOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShopifyOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShopifyOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ShopifyOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("ShopifyOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ShopifyOrderItemEntity objects.</summary>
	[Serializable]
	public partial class ShopifyOrderItemEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ShopifyOrderItemEntityFactory() : base("ShopifyOrderItemEntity", ShipWorks.Data.Model.EntityType.ShopifyOrderItemEntity) { }

		/// <summary>Creates a new, empty ShopifyOrderItemEntity object.</summary>
		/// <returns>A new, empty ShopifyOrderItemEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ShopifyOrderItemEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShopifyOrderItem
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ShopifyOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShopifyOrderItemEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShopifyOrderItemUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ShopifyOrderItemEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("ShopifyOrderItemEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ShopifyStoreEntity objects.</summary>
	[Serializable]
	public partial class ShopifyStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ShopifyStoreEntityFactory() : base("ShopifyStoreEntity", ShipWorks.Data.Model.EntityType.ShopifyStoreEntity) { }

		/// <summary>Creates a new, empty ShopifyStoreEntity object.</summary>
		/// <returns>A new, empty ShopifyStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ShopifyStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShopifyStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ShopifyStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShopifyStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShopifyStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ShopifyStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("ShopifyStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ShopSiteStoreEntity objects.</summary>
	[Serializable]
	public partial class ShopSiteStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ShopSiteStoreEntityFactory() : base("ShopSiteStoreEntity", ShipWorks.Data.Model.EntityType.ShopSiteStoreEntity) { }

		/// <summary>Creates a new, empty ShopSiteStoreEntity object.</summary>
		/// <returns>A new, empty ShopSiteStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ShopSiteStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShopSiteStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ShopSiteStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ShopSiteStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewShopSiteStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ShopSiteStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("ShopSiteStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty SparkPayStoreEntity objects.</summary>
	[Serializable]
	public partial class SparkPayStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public SparkPayStoreEntityFactory() : base("SparkPayStoreEntity", ShipWorks.Data.Model.EntityType.SparkPayStoreEntity) { }

		/// <summary>Creates a new, empty SparkPayStoreEntity object.</summary>
		/// <returns>A new, empty SparkPayStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new SparkPayStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSparkPayStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new SparkPayStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SparkPayStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSparkPayStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<SparkPayStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("SparkPayStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty StatusPresetEntity objects.</summary>
	[Serializable]
	public partial class StatusPresetEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public StatusPresetEntityFactory() : base("StatusPresetEntity", ShipWorks.Data.Model.EntityType.StatusPresetEntity) { }

		/// <summary>Creates a new, empty StatusPresetEntity object.</summary>
		/// <returns>A new, empty StatusPresetEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new StatusPresetEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewStatusPreset
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new StatusPresetEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new StatusPresetEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewStatusPresetUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<StatusPresetEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty StoreEntity objects.</summary>
	[Serializable]
	public partial class StoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public StoreEntityFactory() : base("StoreEntity", ShipWorks.Data.Model.EntityType.StoreEntity) { }

		/// <summary>Creates a new, empty StoreEntity object.</summary>
		/// <returns>A new, empty StoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new StoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new StoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new StoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<StoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("StoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty SystemDataEntity objects.</summary>
	[Serializable]
	public partial class SystemDataEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public SystemDataEntityFactory() : base("SystemDataEntity", ShipWorks.Data.Model.EntityType.SystemDataEntity) { }

		/// <summary>Creates a new, empty SystemDataEntity object.</summary>
		/// <returns>A new, empty SystemDataEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new SystemDataEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSystemData
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new SystemDataEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new SystemDataEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewSystemDataUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<SystemDataEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty TemplateEntity objects.</summary>
	[Serializable]
	public partial class TemplateEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public TemplateEntityFactory() : base("TemplateEntity", ShipWorks.Data.Model.EntityType.TemplateEntity) { }

		/// <summary>Creates a new, empty TemplateEntity object.</summary>
		/// <returns>A new, empty TemplateEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new TemplateEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewTemplate
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new TemplateEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new TemplateEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewTemplateUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<TemplateEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty TemplateComputerSettingsEntity objects.</summary>
	[Serializable]
	public partial class TemplateComputerSettingsEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public TemplateComputerSettingsEntityFactory() : base("TemplateComputerSettingsEntity", ShipWorks.Data.Model.EntityType.TemplateComputerSettingsEntity) { }

		/// <summary>Creates a new, empty TemplateComputerSettingsEntity object.</summary>
		/// <returns>A new, empty TemplateComputerSettingsEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new TemplateComputerSettingsEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewTemplateComputerSettings
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new TemplateComputerSettingsEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new TemplateComputerSettingsEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewTemplateComputerSettingsUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<TemplateComputerSettingsEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty TemplateFolderEntity objects.</summary>
	[Serializable]
	public partial class TemplateFolderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public TemplateFolderEntityFactory() : base("TemplateFolderEntity", ShipWorks.Data.Model.EntityType.TemplateFolderEntity) { }

		/// <summary>Creates a new, empty TemplateFolderEntity object.</summary>
		/// <returns>A new, empty TemplateFolderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new TemplateFolderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewTemplateFolder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new TemplateFolderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new TemplateFolderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewTemplateFolderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<TemplateFolderEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty TemplateStoreSettingsEntity objects.</summary>
	[Serializable]
	public partial class TemplateStoreSettingsEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public TemplateStoreSettingsEntityFactory() : base("TemplateStoreSettingsEntity", ShipWorks.Data.Model.EntityType.TemplateStoreSettingsEntity) { }

		/// <summary>Creates a new, empty TemplateStoreSettingsEntity object.</summary>
		/// <returns>A new, empty TemplateStoreSettingsEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new TemplateStoreSettingsEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewTemplateStoreSettings
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new TemplateStoreSettingsEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new TemplateStoreSettingsEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewTemplateStoreSettingsUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<TemplateStoreSettingsEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty TemplateUserSettingsEntity objects.</summary>
	[Serializable]
	public partial class TemplateUserSettingsEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public TemplateUserSettingsEntityFactory() : base("TemplateUserSettingsEntity", ShipWorks.Data.Model.EntityType.TemplateUserSettingsEntity) { }

		/// <summary>Creates a new, empty TemplateUserSettingsEntity object.</summary>
		/// <returns>A new, empty TemplateUserSettingsEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new TemplateUserSettingsEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewTemplateUserSettings
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new TemplateUserSettingsEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new TemplateUserSettingsEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewTemplateUserSettingsUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<TemplateUserSettingsEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ThreeDCartOrderItemEntity objects.</summary>
	[Serializable]
	public partial class ThreeDCartOrderItemEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ThreeDCartOrderItemEntityFactory() : base("ThreeDCartOrderItemEntity", ShipWorks.Data.Model.EntityType.ThreeDCartOrderItemEntity) { }

		/// <summary>Creates a new, empty ThreeDCartOrderItemEntity object.</summary>
		/// <returns>A new, empty ThreeDCartOrderItemEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ThreeDCartOrderItemEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewThreeDCartOrderItem
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ThreeDCartOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ThreeDCartOrderItemEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewThreeDCartOrderItemUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ThreeDCartOrderItemEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("ThreeDCartOrderItemEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ThreeDCartStoreEntity objects.</summary>
	[Serializable]
	public partial class ThreeDCartStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ThreeDCartStoreEntityFactory() : base("ThreeDCartStoreEntity", ShipWorks.Data.Model.EntityType.ThreeDCartStoreEntity) { }

		/// <summary>Creates a new, empty ThreeDCartStoreEntity object.</summary>
		/// <returns>A new, empty ThreeDCartStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ThreeDCartStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewThreeDCartStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ThreeDCartStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ThreeDCartStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewThreeDCartStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ThreeDCartStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("ThreeDCartStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty UpsAccountEntity objects.</summary>
	[Serializable]
	public partial class UpsAccountEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public UpsAccountEntityFactory() : base("UpsAccountEntity", ShipWorks.Data.Model.EntityType.UpsAccountEntity) { }

		/// <summary>Creates a new, empty UpsAccountEntity object.</summary>
		/// <returns>A new, empty UpsAccountEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new UpsAccountEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsAccount
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new UpsAccountEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsAccountEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsAccountUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<UpsAccountEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty UpsPackageEntity objects.</summary>
	[Serializable]
	public partial class UpsPackageEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public UpsPackageEntityFactory() : base("UpsPackageEntity", ShipWorks.Data.Model.EntityType.UpsPackageEntity) { }

		/// <summary>Creates a new, empty UpsPackageEntity object.</summary>
		/// <returns>A new, empty UpsPackageEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new UpsPackageEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsPackage
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new UpsPackageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsPackageEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsPackageUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<UpsPackageEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty UpsProfileEntity objects.</summary>
	[Serializable]
	public partial class UpsProfileEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public UpsProfileEntityFactory() : base("UpsProfileEntity", ShipWorks.Data.Model.EntityType.UpsProfileEntity) { }

		/// <summary>Creates a new, empty UpsProfileEntity object.</summary>
		/// <returns>A new, empty UpsProfileEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new UpsProfileEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsProfile
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new UpsProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsProfileEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsProfileUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<UpsProfileEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty UpsProfilePackageEntity objects.</summary>
	[Serializable]
	public partial class UpsProfilePackageEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public UpsProfilePackageEntityFactory() : base("UpsProfilePackageEntity", ShipWorks.Data.Model.EntityType.UpsProfilePackageEntity) { }

		/// <summary>Creates a new, empty UpsProfilePackageEntity object.</summary>
		/// <returns>A new, empty UpsProfilePackageEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new UpsProfilePackageEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsProfilePackage
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new UpsProfilePackageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsProfilePackageEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsProfilePackageUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<UpsProfilePackageEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty UpsShipmentEntity objects.</summary>
	[Serializable]
	public partial class UpsShipmentEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public UpsShipmentEntityFactory() : base("UpsShipmentEntity", ShipWorks.Data.Model.EntityType.UpsShipmentEntity) { }

		/// <summary>Creates a new, empty UpsShipmentEntity object.</summary>
		/// <returns>A new, empty UpsShipmentEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new UpsShipmentEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsShipment
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new UpsShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UpsShipmentEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUpsShipmentUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<UpsShipmentEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty UserEntity objects.</summary>
	[Serializable]
	public partial class UserEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public UserEntityFactory() : base("UserEntity", ShipWorks.Data.Model.EntityType.UserEntity) { }

		/// <summary>Creates a new, empty UserEntity object.</summary>
		/// <returns>A new, empty UserEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new UserEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUser
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new UserEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UserEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUserUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<UserEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty UserColumnSettingsEntity objects.</summary>
	[Serializable]
	public partial class UserColumnSettingsEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public UserColumnSettingsEntityFactory() : base("UserColumnSettingsEntity", ShipWorks.Data.Model.EntityType.UserColumnSettingsEntity) { }

		/// <summary>Creates a new, empty UserColumnSettingsEntity object.</summary>
		/// <returns>A new, empty UserColumnSettingsEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new UserColumnSettingsEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUserColumnSettings
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new UserColumnSettingsEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UserColumnSettingsEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUserColumnSettingsUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<UserColumnSettingsEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty UserSettingsEntity objects.</summary>
	[Serializable]
	public partial class UserSettingsEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public UserSettingsEntityFactory() : base("UserSettingsEntity", ShipWorks.Data.Model.EntityType.UserSettingsEntity) { }

		/// <summary>Creates a new, empty UserSettingsEntity object.</summary>
		/// <returns>A new, empty UserSettingsEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new UserSettingsEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUserSettings
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new UserSettingsEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UserSettingsEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUserSettingsUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<UserSettingsEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty UspsAccountEntity objects.</summary>
	[Serializable]
	public partial class UspsAccountEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public UspsAccountEntityFactory() : base("UspsAccountEntity", ShipWorks.Data.Model.EntityType.UspsAccountEntity) { }

		/// <summary>Creates a new, empty UspsAccountEntity object.</summary>
		/// <returns>A new, empty UspsAccountEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new UspsAccountEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUspsAccount
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new UspsAccountEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UspsAccountEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUspsAccountUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<UspsAccountEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty UspsProfileEntity objects.</summary>
	[Serializable]
	public partial class UspsProfileEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public UspsProfileEntityFactory() : base("UspsProfileEntity", ShipWorks.Data.Model.EntityType.UspsProfileEntity) { }

		/// <summary>Creates a new, empty UspsProfileEntity object.</summary>
		/// <returns>A new, empty UspsProfileEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new UspsProfileEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUspsProfile
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new UspsProfileEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UspsProfileEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUspsProfileUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<UspsProfileEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty UspsScanFormEntity objects.</summary>
	[Serializable]
	public partial class UspsScanFormEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public UspsScanFormEntityFactory() : base("UspsScanFormEntity", ShipWorks.Data.Model.EntityType.UspsScanFormEntity) { }

		/// <summary>Creates a new, empty UspsScanFormEntity object.</summary>
		/// <returns>A new, empty UspsScanFormEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new UspsScanFormEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUspsScanForm
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new UspsScanFormEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UspsScanFormEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUspsScanFormUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<UspsScanFormEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty UspsShipmentEntity objects.</summary>
	[Serializable]
	public partial class UspsShipmentEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public UspsShipmentEntityFactory() : base("UspsShipmentEntity", ShipWorks.Data.Model.EntityType.UspsShipmentEntity) { }

		/// <summary>Creates a new, empty UspsShipmentEntity object.</summary>
		/// <returns>A new, empty UspsShipmentEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new UspsShipmentEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUspsShipment
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new UspsShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new UspsShipmentEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewUspsShipmentUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<UspsShipmentEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty ValidatedAddressEntity objects.</summary>
	[Serializable]
	public partial class ValidatedAddressEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public ValidatedAddressEntityFactory() : base("ValidatedAddressEntity", ShipWorks.Data.Model.EntityType.ValidatedAddressEntity) { }

		/// <summary>Creates a new, empty ValidatedAddressEntity object.</summary>
		/// <returns>A new, empty ValidatedAddressEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new ValidatedAddressEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewValidatedAddress
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new ValidatedAddressEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new ValidatedAddressEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewValidatedAddressUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<ValidatedAddressEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty VersionSignoffEntity objects.</summary>
	[Serializable]
	public partial class VersionSignoffEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public VersionSignoffEntityFactory() : base("VersionSignoffEntity", ShipWorks.Data.Model.EntityType.VersionSignoffEntity) { }

		/// <summary>Creates a new, empty VersionSignoffEntity object.</summary>
		/// <returns>A new, empty VersionSignoffEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new VersionSignoffEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewVersionSignoff
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new VersionSignoffEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new VersionSignoffEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewVersionSignoffUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<VersionSignoffEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty VolusionStoreEntity objects.</summary>
	[Serializable]
	public partial class VolusionStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public VolusionStoreEntityFactory() : base("VolusionStoreEntity", ShipWorks.Data.Model.EntityType.VolusionStoreEntity) { }

		/// <summary>Creates a new, empty VolusionStoreEntity object.</summary>
		/// <returns>A new, empty VolusionStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new VolusionStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewVolusionStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new VolusionStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new VolusionStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewVolusionStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<VolusionStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("VolusionStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty WorldShipGoodsEntity objects.</summary>
	[Serializable]
	public partial class WorldShipGoodsEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public WorldShipGoodsEntityFactory() : base("WorldShipGoodsEntity", ShipWorks.Data.Model.EntityType.WorldShipGoodsEntity) { }

		/// <summary>Creates a new, empty WorldShipGoodsEntity object.</summary>
		/// <returns>A new, empty WorldShipGoodsEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new WorldShipGoodsEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewWorldShipGoods
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new WorldShipGoodsEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new WorldShipGoodsEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewWorldShipGoodsUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<WorldShipGoodsEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty WorldShipPackageEntity objects.</summary>
	[Serializable]
	public partial class WorldShipPackageEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public WorldShipPackageEntityFactory() : base("WorldShipPackageEntity", ShipWorks.Data.Model.EntityType.WorldShipPackageEntity) { }

		/// <summary>Creates a new, empty WorldShipPackageEntity object.</summary>
		/// <returns>A new, empty WorldShipPackageEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new WorldShipPackageEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewWorldShipPackage
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new WorldShipPackageEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new WorldShipPackageEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewWorldShipPackageUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<WorldShipPackageEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty WorldShipProcessedEntity objects.</summary>
	[Serializable]
	public partial class WorldShipProcessedEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public WorldShipProcessedEntityFactory() : base("WorldShipProcessedEntity", ShipWorks.Data.Model.EntityType.WorldShipProcessedEntity) { }

		/// <summary>Creates a new, empty WorldShipProcessedEntity object.</summary>
		/// <returns>A new, empty WorldShipProcessedEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new WorldShipProcessedEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewWorldShipProcessed
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new WorldShipProcessedEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new WorldShipProcessedEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewWorldShipProcessedUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<WorldShipProcessedEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty WorldShipShipmentEntity objects.</summary>
	[Serializable]
	public partial class WorldShipShipmentEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public WorldShipShipmentEntityFactory() : base("WorldShipShipmentEntity", ShipWorks.Data.Model.EntityType.WorldShipShipmentEntity) { }

		/// <summary>Creates a new, empty WorldShipShipmentEntity object.</summary>
		/// <returns>A new, empty WorldShipShipmentEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new WorldShipShipmentEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewWorldShipShipment
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new WorldShipShipmentEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new WorldShipShipmentEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewWorldShipShipmentUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<WorldShipShipmentEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty YahooOrderEntity objects.</summary>
	[Serializable]
	public partial class YahooOrderEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public YahooOrderEntityFactory() : base("YahooOrderEntity", ShipWorks.Data.Model.EntityType.YahooOrderEntity) { }

		/// <summary>Creates a new, empty YahooOrderEntity object.</summary>
		/// <returns>A new, empty YahooOrderEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new YahooOrderEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewYahooOrder
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new YahooOrderEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new YahooOrderEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewYahooOrderUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<YahooOrderEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("YahooOrderEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty YahooOrderItemEntity objects.</summary>
	[Serializable]
	public partial class YahooOrderItemEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public YahooOrderItemEntityFactory() : base("YahooOrderItemEntity", ShipWorks.Data.Model.EntityType.YahooOrderItemEntity) { }

		/// <summary>Creates a new, empty YahooOrderItemEntity object.</summary>
		/// <returns>A new, empty YahooOrderItemEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new YahooOrderItemEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewYahooOrderItem
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new YahooOrderItemEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new YahooOrderItemEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewYahooOrderItemUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<YahooOrderItemEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("YahooOrderItemEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
		}
		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty YahooProductEntity objects.</summary>
	[Serializable]
	public partial class YahooProductEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public YahooProductEntityFactory() : base("YahooProductEntity", ShipWorks.Data.Model.EntityType.YahooProductEntity) { }

		/// <summary>Creates a new, empty YahooProductEntity object.</summary>
		/// <returns>A new, empty YahooProductEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new YahooProductEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewYahooProduct
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new YahooProductEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new YahooProductEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewYahooProductUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<YahooProductEntity>(this);
		}
		

		#region Included Code

		#endregion
	}	
	/// <summary>Factory to create new, empty YahooStoreEntity objects.</summary>
	[Serializable]
	public partial class YahooStoreEntityFactory : EntityFactoryBase2 {
		/// <summary>CTor</summary>
		public YahooStoreEntityFactory() : base("YahooStoreEntity", ShipWorks.Data.Model.EntityType.YahooStoreEntity) { }

		/// <summary>Creates a new, empty YahooStoreEntity object.</summary>
		/// <returns>A new, empty YahooStoreEntity object.</returns>
		public override IEntity2 Create() {
			IEntity2 toReturn = new YahooStoreEntity();
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewYahooStore
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new YahooStoreEntity instance but uses a special constructor which will set the Fields object of the new IEntity2 instance to the passed in fields object.</summary>
		/// <param name="fields">Populated IEntityFields2 object for the new IEntity2 to create</param>
		/// <returns>Fully created and populated (due to the IEntityFields2 object) IEntity2 object</returns>
		public override IEntity2 Create(IEntityFields2 fields) {
			IEntity2 toReturn = new YahooStoreEntity(fields);
			
			// __LLBLGENPRO_USER_CODE_REGION_START CreateNewYahooStoreUsingFields
			// __LLBLGENPRO_USER_CODE_REGION_END
			return toReturn;
		}
		
		/// <summary>Creates a new generic EntityCollection(Of T) for the entity to which this factory belongs.</summary>
		/// <returns>ready to use generic EntityCollection(Of T) with this factory set as the factory</returns>
		public override IEntityCollection2 CreateEntityCollection()
		{
			return new EntityCollection<YahooStoreEntity>(this);
		}
		
		/// <summary>Creates the hierarchy fields for the entity to which this factory belongs.</summary>
		/// <returns>IEntityFields2 object with the fields of all the entities in teh hierarchy of this entity or the fields of this entity if the entity isn't in a hierarchy.</returns>
		public override IEntityFields2 CreateHierarchyFields() 
		{
			return new EntityFields2(InheritanceInfoProviderSingleton.GetInstance().GetHierarchyFields("YahooStoreEntity"), InheritanceInfoProviderSingleton.GetInstance(), null);
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
				case ShipWorks.Data.Model.EntityType.ChannelAdvisorStoreEntity:
					factoryToUse = new ChannelAdvisorStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ClickCartProOrderEntity:
					factoryToUse = new ClickCartProOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.CommerceInterfaceOrderEntity:
					factoryToUse = new CommerceInterfaceOrderEntityFactory();
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
				case ShipWorks.Data.Model.EntityType.LemonStandStoreEntity:
					factoryToUse = new LemonStandStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.MagentoOrderEntity:
					factoryToUse = new MagentoOrderEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.MagentoStoreEntity:
					factoryToUse = new MagentoStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.MarketplaceAdvisorOrderEntity:
					factoryToUse = new MarketplaceAdvisorOrderEntityFactory();
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
				case ShipWorks.Data.Model.EntityType.OrderMotionStoreEntity:
					factoryToUse = new OrderMotionStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.OrderPaymentDetailEntity:
					factoryToUse = new OrderPaymentDetailEntityFactory();
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
				case ShipWorks.Data.Model.EntityType.ThreeDCartOrderItemEntity:
					factoryToUse = new ThreeDCartOrderItemEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.ThreeDCartStoreEntity:
					factoryToUse = new ThreeDCartStoreEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsAccountEntity:
					factoryToUse = new UpsAccountEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsPackageEntity:
					factoryToUse = new UpsPackageEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsProfileEntity:
					factoryToUse = new UpsProfileEntityFactory();
					break;
				case ShipWorks.Data.Model.EntityType.UpsProfilePackageEntity:
					factoryToUse = new UpsProfilePackageEntityFactory();
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
#if CF
		/// <summary>Gets the factory of the entity with the ShipWorks.Data.Model.EntityType specified</summary>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <returns>factory to use or null if not found</returns>
		public static IEntityFactory2 GetFactory(ShipWorks.Data.Model.EntityType typeOfEntity)
		{
			return GeneralEntityFactory.Create(typeOfEntity).GetEntityFactory();
		}
#else
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
#endif		
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
		
		/// <summary>Obtains the inheritance info provider instance from the singleton </summary>
		/// <returns>The singleton instance of the inheritance info provider</returns>
		public override IInheritanceInfoProvider ObtainInheritanceInfoProviderInstance()
		{
			return InheritanceInfoProviderSingleton.GetInstance();
		}
		
		/// <summary>Implementation of the routine which gets the factory of the Entity type with the ShipWorks.Data.Model.EntityType value passed in</summary>
		/// <param name="entityTypeValue">The entity type value.</param>
		/// <returns>the entity factory of the entity type or null if not found</returns>
		protected override IEntityFactoryCore GetFactoryImpl(int entityTypeValue)
		{
			return EntityFactoryFactory.GetFactory((ShipWorks.Data.Model.EntityType)entityTypeValue);
		}
#if !CF		
		/// <summary>Implementation of the routine which gets the factory of the Entity type with the .NET type passed in</summary>
		/// <param name="typeOfEntity">The type of entity.</param>
		/// <returns>the entity factory of the entity type or null if not found</returns>
		protected override IEntityFactoryCore GetFactoryImpl(Type typeOfEntity)
		{
			return EntityFactoryFactory.GetFactory(typeOfEntity);
		}
#endif
	}
}
