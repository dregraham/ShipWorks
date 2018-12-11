using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.Custom
{	
	/// <summary>
	/// Strongly typed collection of ActionEntity
	/// </summary>
	public class ActionCollection : EntityCollection<ActionEntity>
	{
        /// <summary>
        /// Gets the count of all ActionEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ActionEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ActionEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ActionCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ActionCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ActionCollection collection = new ActionCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ActionFilterTriggerEntity
	/// </summary>
	public class ActionFilterTriggerCollection : EntityCollection<ActionFilterTriggerEntity>
	{
        /// <summary>
        /// Gets the count of all ActionFilterTriggerEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ActionFilterTriggerEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ActionFilterTriggerEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ActionFilterTriggerCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ActionFilterTriggerCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ActionFilterTriggerCollection collection = new ActionFilterTriggerCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ActionQueueEntity
	/// </summary>
	public class ActionQueueCollection : EntityCollection<ActionQueueEntity>
	{
        /// <summary>
        /// Gets the count of all ActionQueueEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ActionQueueEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ActionQueueEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ActionQueueCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ActionQueueCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ActionQueueCollection collection = new ActionQueueCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ActionQueueSelectionEntity
	/// </summary>
	public class ActionQueueSelectionCollection : EntityCollection<ActionQueueSelectionEntity>
	{
        /// <summary>
        /// Gets the count of all ActionQueueSelectionEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ActionQueueSelectionEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ActionQueueSelectionEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ActionQueueSelectionCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ActionQueueSelectionCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ActionQueueSelectionCollection collection = new ActionQueueSelectionCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ActionQueueStepEntity
	/// </summary>
	public class ActionQueueStepCollection : EntityCollection<ActionQueueStepEntity>
	{
        /// <summary>
        /// Gets the count of all ActionQueueStepEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ActionQueueStepEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ActionQueueStepEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ActionQueueStepCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ActionQueueStepCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ActionQueueStepCollection collection = new ActionQueueStepCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ActionTaskEntity
	/// </summary>
	public class ActionTaskCollection : EntityCollection<ActionTaskEntity>
	{
        /// <summary>
        /// Gets the count of all ActionTaskEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ActionTaskEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ActionTaskEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ActionTaskCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ActionTaskCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ActionTaskCollection collection = new ActionTaskCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of AmazonASINEntity
	/// </summary>
	public class AmazonASINCollection : EntityCollection<AmazonASINEntity>
	{
        /// <summary>
        /// Gets the count of all AmazonASINEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AmazonASINEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new AmazonASINEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static AmazonASINCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AmazonASINCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            AmazonASINCollection collection = new AmazonASINCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of AmazonOrderEntity
	/// </summary>
	public class AmazonOrderCollection : EntityCollection<AmazonOrderEntity>
	{
        /// <summary>
        /// Gets the count of all AmazonOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AmazonOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new AmazonOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static AmazonOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AmazonOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            AmazonOrderCollection collection = new AmazonOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of AmazonOrderItemEntity
	/// </summary>
	public class AmazonOrderItemCollection : EntityCollection<AmazonOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all AmazonOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AmazonOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new AmazonOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static AmazonOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AmazonOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            AmazonOrderItemCollection collection = new AmazonOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of AmazonOrderSearchEntity
	/// </summary>
	public class AmazonOrderSearchCollection : EntityCollection<AmazonOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all AmazonOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AmazonOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new AmazonOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static AmazonOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AmazonOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            AmazonOrderSearchCollection collection = new AmazonOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of AmazonProfileEntity
	/// </summary>
	public class AmazonProfileCollection : EntityCollection<AmazonProfileEntity>
	{
        /// <summary>
        /// Gets the count of all AmazonProfileEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AmazonProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new AmazonProfileEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static AmazonProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AmazonProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            AmazonProfileCollection collection = new AmazonProfileCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of AmazonServiceTypeEntity
	/// </summary>
	public class AmazonServiceTypeCollection : EntityCollection<AmazonServiceTypeEntity>
	{
        /// <summary>
        /// Gets the count of all AmazonServiceTypeEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AmazonServiceTypeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new AmazonServiceTypeEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static AmazonServiceTypeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AmazonServiceTypeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            AmazonServiceTypeCollection collection = new AmazonServiceTypeCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of AmazonShipmentEntity
	/// </summary>
	public class AmazonShipmentCollection : EntityCollection<AmazonShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all AmazonShipmentEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AmazonShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new AmazonShipmentEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static AmazonShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AmazonShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            AmazonShipmentCollection collection = new AmazonShipmentCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of AmazonStoreEntity
	/// </summary>
	public class AmazonStoreCollection : EntityCollection<AmazonStoreEntity>
	{
        /// <summary>
        /// Gets the count of all AmazonStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AmazonStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new AmazonStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static AmazonStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AmazonStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            AmazonStoreCollection collection = new AmazonStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of AmeriCommerceStoreEntity
	/// </summary>
	public class AmeriCommerceStoreCollection : EntityCollection<AmeriCommerceStoreEntity>
	{
        /// <summary>
        /// Gets the count of all AmeriCommerceStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AmeriCommerceStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new AmeriCommerceStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static AmeriCommerceStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AmeriCommerceStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            AmeriCommerceStoreCollection collection = new AmeriCommerceStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of AsendiaAccountEntity
	/// </summary>
	public class AsendiaAccountCollection : EntityCollection<AsendiaAccountEntity>
	{
        /// <summary>
        /// Gets the count of all AsendiaAccountEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AsendiaAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new AsendiaAccountEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static AsendiaAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AsendiaAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            AsendiaAccountCollection collection = new AsendiaAccountCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of AsendiaProfileEntity
	/// </summary>
	public class AsendiaProfileCollection : EntityCollection<AsendiaProfileEntity>
	{
        /// <summary>
        /// Gets the count of all AsendiaProfileEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AsendiaProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new AsendiaProfileEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static AsendiaProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AsendiaProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            AsendiaProfileCollection collection = new AsendiaProfileCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of AsendiaShipmentEntity
	/// </summary>
	public class AsendiaShipmentCollection : EntityCollection<AsendiaShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all AsendiaShipmentEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AsendiaShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new AsendiaShipmentEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static AsendiaShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AsendiaShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            AsendiaShipmentCollection collection = new AsendiaShipmentCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of AuditEntity
	/// </summary>
	public class AuditCollection : EntityCollection<AuditEntity>
	{
        /// <summary>
        /// Gets the count of all AuditEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AuditEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new AuditEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static AuditCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AuditCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            AuditCollection collection = new AuditCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of AuditChangeEntity
	/// </summary>
	public class AuditChangeCollection : EntityCollection<AuditChangeEntity>
	{
        /// <summary>
        /// Gets the count of all AuditChangeEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AuditChangeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new AuditChangeEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static AuditChangeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AuditChangeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            AuditChangeCollection collection = new AuditChangeCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of AuditChangeDetailEntity
	/// </summary>
	public class AuditChangeDetailCollection : EntityCollection<AuditChangeDetailEntity>
	{
        /// <summary>
        /// Gets the count of all AuditChangeDetailEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AuditChangeDetailEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new AuditChangeDetailEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static AuditChangeDetailCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AuditChangeDetailCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            AuditChangeDetailCollection collection = new AuditChangeDetailCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of BestRateProfileEntity
	/// </summary>
	public class BestRateProfileCollection : EntityCollection<BestRateProfileEntity>
	{
        /// <summary>
        /// Gets the count of all BestRateProfileEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all BestRateProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new BestRateProfileEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static BestRateProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static BestRateProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            BestRateProfileCollection collection = new BestRateProfileCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of BestRateShipmentEntity
	/// </summary>
	public class BestRateShipmentCollection : EntityCollection<BestRateShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all BestRateShipmentEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all BestRateShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new BestRateShipmentEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static BestRateShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static BestRateShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            BestRateShipmentCollection collection = new BestRateShipmentCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of BigCommerceOrderItemEntity
	/// </summary>
	public class BigCommerceOrderItemCollection : EntityCollection<BigCommerceOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all BigCommerceOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all BigCommerceOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new BigCommerceOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static BigCommerceOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static BigCommerceOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            BigCommerceOrderItemCollection collection = new BigCommerceOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of BigCommerceStoreEntity
	/// </summary>
	public class BigCommerceStoreCollection : EntityCollection<BigCommerceStoreEntity>
	{
        /// <summary>
        /// Gets the count of all BigCommerceStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all BigCommerceStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new BigCommerceStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static BigCommerceStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static BigCommerceStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            BigCommerceStoreCollection collection = new BigCommerceStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of BuyDotComOrderItemEntity
	/// </summary>
	public class BuyDotComOrderItemCollection : EntityCollection<BuyDotComOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all BuyDotComOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all BuyDotComOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new BuyDotComOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static BuyDotComOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static BuyDotComOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            BuyDotComOrderItemCollection collection = new BuyDotComOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of BuyDotComStoreEntity
	/// </summary>
	public class BuyDotComStoreCollection : EntityCollection<BuyDotComStoreEntity>
	{
        /// <summary>
        /// Gets the count of all BuyDotComStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all BuyDotComStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new BuyDotComStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static BuyDotComStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static BuyDotComStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            BuyDotComStoreCollection collection = new BuyDotComStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ChannelAdvisorOrderEntity
	/// </summary>
	public class ChannelAdvisorOrderCollection : EntityCollection<ChannelAdvisorOrderEntity>
	{
        /// <summary>
        /// Gets the count of all ChannelAdvisorOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ChannelAdvisorOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ChannelAdvisorOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ChannelAdvisorOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ChannelAdvisorOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ChannelAdvisorOrderCollection collection = new ChannelAdvisorOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ChannelAdvisorOrderItemEntity
	/// </summary>
	public class ChannelAdvisorOrderItemCollection : EntityCollection<ChannelAdvisorOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all ChannelAdvisorOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ChannelAdvisorOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ChannelAdvisorOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ChannelAdvisorOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ChannelAdvisorOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ChannelAdvisorOrderItemCollection collection = new ChannelAdvisorOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ChannelAdvisorOrderSearchEntity
	/// </summary>
	public class ChannelAdvisorOrderSearchCollection : EntityCollection<ChannelAdvisorOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all ChannelAdvisorOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ChannelAdvisorOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ChannelAdvisorOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ChannelAdvisorOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ChannelAdvisorOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ChannelAdvisorOrderSearchCollection collection = new ChannelAdvisorOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ChannelAdvisorStoreEntity
	/// </summary>
	public class ChannelAdvisorStoreCollection : EntityCollection<ChannelAdvisorStoreEntity>
	{
        /// <summary>
        /// Gets the count of all ChannelAdvisorStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ChannelAdvisorStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ChannelAdvisorStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ChannelAdvisorStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ChannelAdvisorStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ChannelAdvisorStoreCollection collection = new ChannelAdvisorStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ClickCartProOrderEntity
	/// </summary>
	public class ClickCartProOrderCollection : EntityCollection<ClickCartProOrderEntity>
	{
        /// <summary>
        /// Gets the count of all ClickCartProOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ClickCartProOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ClickCartProOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ClickCartProOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ClickCartProOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ClickCartProOrderCollection collection = new ClickCartProOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ClickCartProOrderSearchEntity
	/// </summary>
	public class ClickCartProOrderSearchCollection : EntityCollection<ClickCartProOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all ClickCartProOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ClickCartProOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ClickCartProOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ClickCartProOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ClickCartProOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ClickCartProOrderSearchCollection collection = new ClickCartProOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of CommerceInterfaceOrderEntity
	/// </summary>
	public class CommerceInterfaceOrderCollection : EntityCollection<CommerceInterfaceOrderEntity>
	{
        /// <summary>
        /// Gets the count of all CommerceInterfaceOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all CommerceInterfaceOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new CommerceInterfaceOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static CommerceInterfaceOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static CommerceInterfaceOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            CommerceInterfaceOrderCollection collection = new CommerceInterfaceOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of CommerceInterfaceOrderSearchEntity
	/// </summary>
	public class CommerceInterfaceOrderSearchCollection : EntityCollection<CommerceInterfaceOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all CommerceInterfaceOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all CommerceInterfaceOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new CommerceInterfaceOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static CommerceInterfaceOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static CommerceInterfaceOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            CommerceInterfaceOrderSearchCollection collection = new CommerceInterfaceOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ComputerEntity
	/// </summary>
	public class ComputerCollection : EntityCollection<ComputerEntity>
	{
        /// <summary>
        /// Gets the count of all ComputerEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ComputerEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ComputerEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ComputerCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ComputerCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ComputerCollection collection = new ComputerCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ConfigurationEntity
	/// </summary>
	public class ConfigurationCollection : EntityCollection<ConfigurationEntity>
	{
        /// <summary>
        /// Gets the count of all ConfigurationEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ConfigurationEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ConfigurationEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ConfigurationCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ConfigurationCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ConfigurationCollection collection = new ConfigurationCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of CustomerEntity
	/// </summary>
	public class CustomerCollection : EntityCollection<CustomerEntity>
	{
        /// <summary>
        /// Gets the count of all CustomerEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all CustomerEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new CustomerEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static CustomerCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static CustomerCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            CustomerCollection collection = new CustomerCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of DhlExpressAccountEntity
	/// </summary>
	public class DhlExpressAccountCollection : EntityCollection<DhlExpressAccountEntity>
	{
        /// <summary>
        /// Gets the count of all DhlExpressAccountEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all DhlExpressAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new DhlExpressAccountEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static DhlExpressAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static DhlExpressAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            DhlExpressAccountCollection collection = new DhlExpressAccountCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of DhlExpressPackageEntity
	/// </summary>
	public class DhlExpressPackageCollection : EntityCollection<DhlExpressPackageEntity>
	{
        /// <summary>
        /// Gets the count of all DhlExpressPackageEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all DhlExpressPackageEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new DhlExpressPackageEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static DhlExpressPackageCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static DhlExpressPackageCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            DhlExpressPackageCollection collection = new DhlExpressPackageCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of DhlExpressProfileEntity
	/// </summary>
	public class DhlExpressProfileCollection : EntityCollection<DhlExpressProfileEntity>
	{
        /// <summary>
        /// Gets the count of all DhlExpressProfileEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all DhlExpressProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new DhlExpressProfileEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static DhlExpressProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static DhlExpressProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            DhlExpressProfileCollection collection = new DhlExpressProfileCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of DhlExpressShipmentEntity
	/// </summary>
	public class DhlExpressShipmentCollection : EntityCollection<DhlExpressShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all DhlExpressShipmentEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all DhlExpressShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new DhlExpressShipmentEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static DhlExpressShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static DhlExpressShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            DhlExpressShipmentCollection collection = new DhlExpressShipmentCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of DimensionsProfileEntity
	/// </summary>
	public class DimensionsProfileCollection : EntityCollection<DimensionsProfileEntity>
	{
        /// <summary>
        /// Gets the count of all DimensionsProfileEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all DimensionsProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new DimensionsProfileEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static DimensionsProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static DimensionsProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            DimensionsProfileCollection collection = new DimensionsProfileCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of DownloadEntity
	/// </summary>
	public class DownloadCollection : EntityCollection<DownloadEntity>
	{
        /// <summary>
        /// Gets the count of all DownloadEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all DownloadEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new DownloadEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static DownloadCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static DownloadCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            DownloadCollection collection = new DownloadCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of DownloadDetailEntity
	/// </summary>
	public class DownloadDetailCollection : EntityCollection<DownloadDetailEntity>
	{
        /// <summary>
        /// Gets the count of all DownloadDetailEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all DownloadDetailEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new DownloadDetailEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static DownloadDetailCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static DownloadDetailCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            DownloadDetailCollection collection = new DownloadDetailCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of EbayCombinedOrderRelationEntity
	/// </summary>
	public class EbayCombinedOrderRelationCollection : EntityCollection<EbayCombinedOrderRelationEntity>
	{
        /// <summary>
        /// Gets the count of all EbayCombinedOrderRelationEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EbayCombinedOrderRelationEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new EbayCombinedOrderRelationEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static EbayCombinedOrderRelationCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EbayCombinedOrderRelationCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            EbayCombinedOrderRelationCollection collection = new EbayCombinedOrderRelationCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of EbayOrderEntity
	/// </summary>
	public class EbayOrderCollection : EntityCollection<EbayOrderEntity>
	{
        /// <summary>
        /// Gets the count of all EbayOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EbayOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new EbayOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static EbayOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EbayOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            EbayOrderCollection collection = new EbayOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of EbayOrderItemEntity
	/// </summary>
	public class EbayOrderItemCollection : EntityCollection<EbayOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all EbayOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EbayOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new EbayOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static EbayOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EbayOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            EbayOrderItemCollection collection = new EbayOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of EbayOrderSearchEntity
	/// </summary>
	public class EbayOrderSearchCollection : EntityCollection<EbayOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all EbayOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EbayOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new EbayOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static EbayOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EbayOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            EbayOrderSearchCollection collection = new EbayOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of EbayStoreEntity
	/// </summary>
	public class EbayStoreCollection : EntityCollection<EbayStoreEntity>
	{
        /// <summary>
        /// Gets the count of all EbayStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EbayStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new EbayStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static EbayStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EbayStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            EbayStoreCollection collection = new EbayStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of EmailAccountEntity
	/// </summary>
	public class EmailAccountCollection : EntityCollection<EmailAccountEntity>
	{
        /// <summary>
        /// Gets the count of all EmailAccountEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EmailAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new EmailAccountEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static EmailAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EmailAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            EmailAccountCollection collection = new EmailAccountCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of EmailOutboundEntity
	/// </summary>
	public class EmailOutboundCollection : EntityCollection<EmailOutboundEntity>
	{
        /// <summary>
        /// Gets the count of all EmailOutboundEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EmailOutboundEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new EmailOutboundEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static EmailOutboundCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EmailOutboundCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            EmailOutboundCollection collection = new EmailOutboundCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of EmailOutboundRelationEntity
	/// </summary>
	public class EmailOutboundRelationCollection : EntityCollection<EmailOutboundRelationEntity>
	{
        /// <summary>
        /// Gets the count of all EmailOutboundRelationEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EmailOutboundRelationEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new EmailOutboundRelationEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static EmailOutboundRelationCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EmailOutboundRelationCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            EmailOutboundRelationCollection collection = new EmailOutboundRelationCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of EndiciaAccountEntity
	/// </summary>
	public class EndiciaAccountCollection : EntityCollection<EndiciaAccountEntity>
	{
        /// <summary>
        /// Gets the count of all EndiciaAccountEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EndiciaAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new EndiciaAccountEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static EndiciaAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EndiciaAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            EndiciaAccountCollection collection = new EndiciaAccountCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of EndiciaProfileEntity
	/// </summary>
	public class EndiciaProfileCollection : EntityCollection<EndiciaProfileEntity>
	{
        /// <summary>
        /// Gets the count of all EndiciaProfileEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EndiciaProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new EndiciaProfileEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static EndiciaProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EndiciaProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            EndiciaProfileCollection collection = new EndiciaProfileCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of EndiciaScanFormEntity
	/// </summary>
	public class EndiciaScanFormCollection : EntityCollection<EndiciaScanFormEntity>
	{
        /// <summary>
        /// Gets the count of all EndiciaScanFormEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EndiciaScanFormEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new EndiciaScanFormEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static EndiciaScanFormCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EndiciaScanFormCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            EndiciaScanFormCollection collection = new EndiciaScanFormCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of EndiciaShipmentEntity
	/// </summary>
	public class EndiciaShipmentCollection : EntityCollection<EndiciaShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all EndiciaShipmentEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EndiciaShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new EndiciaShipmentEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static EndiciaShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EndiciaShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            EndiciaShipmentCollection collection = new EndiciaShipmentCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of EtsyOrderEntity
	/// </summary>
	public class EtsyOrderCollection : EntityCollection<EtsyOrderEntity>
	{
        /// <summary>
        /// Gets the count of all EtsyOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EtsyOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new EtsyOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static EtsyOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EtsyOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            EtsyOrderCollection collection = new EtsyOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of EtsyOrderItemEntity
	/// </summary>
	public class EtsyOrderItemCollection : EntityCollection<EtsyOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all EtsyOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EtsyOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new EtsyOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static EtsyOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EtsyOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            EtsyOrderItemCollection collection = new EtsyOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of EtsyStoreEntity
	/// </summary>
	public class EtsyStoreCollection : EntityCollection<EtsyStoreEntity>
	{
        /// <summary>
        /// Gets the count of all EtsyStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EtsyStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new EtsyStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static EtsyStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EtsyStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            EtsyStoreCollection collection = new EtsyStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ExcludedPackageTypeEntity
	/// </summary>
	public class ExcludedPackageTypeCollection : EntityCollection<ExcludedPackageTypeEntity>
	{
        /// <summary>
        /// Gets the count of all ExcludedPackageTypeEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ExcludedPackageTypeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ExcludedPackageTypeEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ExcludedPackageTypeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ExcludedPackageTypeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ExcludedPackageTypeCollection collection = new ExcludedPackageTypeCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ExcludedServiceTypeEntity
	/// </summary>
	public class ExcludedServiceTypeCollection : EntityCollection<ExcludedServiceTypeEntity>
	{
        /// <summary>
        /// Gets the count of all ExcludedServiceTypeEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ExcludedServiceTypeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ExcludedServiceTypeEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ExcludedServiceTypeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ExcludedServiceTypeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ExcludedServiceTypeCollection collection = new ExcludedServiceTypeCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of FedExAccountEntity
	/// </summary>
	public class FedExAccountCollection : EntityCollection<FedExAccountEntity>
	{
        /// <summary>
        /// Gets the count of all FedExAccountEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FedExAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new FedExAccountEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static FedExAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FedExAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            FedExAccountCollection collection = new FedExAccountCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of FedExEndOfDayCloseEntity
	/// </summary>
	public class FedExEndOfDayCloseCollection : EntityCollection<FedExEndOfDayCloseEntity>
	{
        /// <summary>
        /// Gets the count of all FedExEndOfDayCloseEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FedExEndOfDayCloseEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new FedExEndOfDayCloseEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static FedExEndOfDayCloseCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FedExEndOfDayCloseCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            FedExEndOfDayCloseCollection collection = new FedExEndOfDayCloseCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of FedExPackageEntity
	/// </summary>
	public class FedExPackageCollection : EntityCollection<FedExPackageEntity>
	{
        /// <summary>
        /// Gets the count of all FedExPackageEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FedExPackageEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new FedExPackageEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static FedExPackageCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FedExPackageCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            FedExPackageCollection collection = new FedExPackageCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of FedExProfileEntity
	/// </summary>
	public class FedExProfileCollection : EntityCollection<FedExProfileEntity>
	{
        /// <summary>
        /// Gets the count of all FedExProfileEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FedExProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new FedExProfileEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static FedExProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FedExProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            FedExProfileCollection collection = new FedExProfileCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of FedExProfilePackageEntity
	/// </summary>
	public class FedExProfilePackageCollection : EntityCollection<FedExProfilePackageEntity>
	{
        /// <summary>
        /// Gets the count of all FedExProfilePackageEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FedExProfilePackageEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new FedExProfilePackageEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static FedExProfilePackageCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FedExProfilePackageCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            FedExProfilePackageCollection collection = new FedExProfilePackageCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of FedExShipmentEntity
	/// </summary>
	public class FedExShipmentCollection : EntityCollection<FedExShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all FedExShipmentEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FedExShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new FedExShipmentEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static FedExShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FedExShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            FedExShipmentCollection collection = new FedExShipmentCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of FilterEntity
	/// </summary>
	public class FilterCollection : EntityCollection<FilterEntity>
	{
        /// <summary>
        /// Gets the count of all FilterEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FilterEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new FilterEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static FilterCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FilterCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            FilterCollection collection = new FilterCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of FilterLayoutEntity
	/// </summary>
	public class FilterLayoutCollection : EntityCollection<FilterLayoutEntity>
	{
        /// <summary>
        /// Gets the count of all FilterLayoutEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FilterLayoutEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new FilterLayoutEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static FilterLayoutCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FilterLayoutCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            FilterLayoutCollection collection = new FilterLayoutCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of FilterNodeEntity
	/// </summary>
	public class FilterNodeCollection : EntityCollection<FilterNodeEntity>
	{
        /// <summary>
        /// Gets the count of all FilterNodeEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FilterNodeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new FilterNodeEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static FilterNodeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FilterNodeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            FilterNodeCollection collection = new FilterNodeCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of FilterNodeColumnSettingsEntity
	/// </summary>
	public class FilterNodeColumnSettingsCollection : EntityCollection<FilterNodeColumnSettingsEntity>
	{
        /// <summary>
        /// Gets the count of all FilterNodeColumnSettingsEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FilterNodeColumnSettingsEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new FilterNodeColumnSettingsEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static FilterNodeColumnSettingsCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FilterNodeColumnSettingsCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            FilterNodeColumnSettingsCollection collection = new FilterNodeColumnSettingsCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of FilterNodeContentEntity
	/// </summary>
	public class FilterNodeContentCollection : EntityCollection<FilterNodeContentEntity>
	{
        /// <summary>
        /// Gets the count of all FilterNodeContentEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FilterNodeContentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new FilterNodeContentEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static FilterNodeContentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FilterNodeContentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            FilterNodeContentCollection collection = new FilterNodeContentCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of FilterNodeContentDetailEntity
	/// </summary>
	public class FilterNodeContentDetailCollection : EntityCollection<FilterNodeContentDetailEntity>
	{
        /// <summary>
        /// Gets the count of all FilterNodeContentDetailEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FilterNodeContentDetailEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new FilterNodeContentDetailEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static FilterNodeContentDetailCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FilterNodeContentDetailCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            FilterNodeContentDetailCollection collection = new FilterNodeContentDetailCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of FilterSequenceEntity
	/// </summary>
	public class FilterSequenceCollection : EntityCollection<FilterSequenceEntity>
	{
        /// <summary>
        /// Gets the count of all FilterSequenceEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FilterSequenceEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new FilterSequenceEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static FilterSequenceCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FilterSequenceCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            FilterSequenceCollection collection = new FilterSequenceCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of FtpAccountEntity
	/// </summary>
	public class FtpAccountCollection : EntityCollection<FtpAccountEntity>
	{
        /// <summary>
        /// Gets the count of all FtpAccountEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FtpAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new FtpAccountEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static FtpAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FtpAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            FtpAccountCollection collection = new FtpAccountCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of GenericFileStoreEntity
	/// </summary>
	public class GenericFileStoreCollection : EntityCollection<GenericFileStoreEntity>
	{
        /// <summary>
        /// Gets the count of all GenericFileStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GenericFileStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new GenericFileStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static GenericFileStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GenericFileStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            GenericFileStoreCollection collection = new GenericFileStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of GenericModuleOrderEntity
	/// </summary>
	public class GenericModuleOrderCollection : EntityCollection<GenericModuleOrderEntity>
	{
        /// <summary>
        /// Gets the count of all GenericModuleOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GenericModuleOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new GenericModuleOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static GenericModuleOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GenericModuleOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            GenericModuleOrderCollection collection = new GenericModuleOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of GenericModuleOrderItemEntity
	/// </summary>
	public class GenericModuleOrderItemCollection : EntityCollection<GenericModuleOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all GenericModuleOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GenericModuleOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new GenericModuleOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static GenericModuleOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GenericModuleOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            GenericModuleOrderItemCollection collection = new GenericModuleOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of GenericModuleStoreEntity
	/// </summary>
	public class GenericModuleStoreCollection : EntityCollection<GenericModuleStoreEntity>
	{
        /// <summary>
        /// Gets the count of all GenericModuleStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GenericModuleStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new GenericModuleStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static GenericModuleStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GenericModuleStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            GenericModuleStoreCollection collection = new GenericModuleStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of GridColumnFormatEntity
	/// </summary>
	public class GridColumnFormatCollection : EntityCollection<GridColumnFormatEntity>
	{
        /// <summary>
        /// Gets the count of all GridColumnFormatEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GridColumnFormatEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new GridColumnFormatEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static GridColumnFormatCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GridColumnFormatCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            GridColumnFormatCollection collection = new GridColumnFormatCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of GridColumnLayoutEntity
	/// </summary>
	public class GridColumnLayoutCollection : EntityCollection<GridColumnLayoutEntity>
	{
        /// <summary>
        /// Gets the count of all GridColumnLayoutEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GridColumnLayoutEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new GridColumnLayoutEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static GridColumnLayoutCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GridColumnLayoutCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            GridColumnLayoutCollection collection = new GridColumnLayoutCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of GridColumnPositionEntity
	/// </summary>
	public class GridColumnPositionCollection : EntityCollection<GridColumnPositionEntity>
	{
        /// <summary>
        /// Gets the count of all GridColumnPositionEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GridColumnPositionEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new GridColumnPositionEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static GridColumnPositionCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GridColumnPositionCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            GridColumnPositionCollection collection = new GridColumnPositionCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of GrouponOrderEntity
	/// </summary>
	public class GrouponOrderCollection : EntityCollection<GrouponOrderEntity>
	{
        /// <summary>
        /// Gets the count of all GrouponOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GrouponOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new GrouponOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static GrouponOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GrouponOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            GrouponOrderCollection collection = new GrouponOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of GrouponOrderItemEntity
	/// </summary>
	public class GrouponOrderItemCollection : EntityCollection<GrouponOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all GrouponOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GrouponOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new GrouponOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static GrouponOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GrouponOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            GrouponOrderItemCollection collection = new GrouponOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of GrouponOrderSearchEntity
	/// </summary>
	public class GrouponOrderSearchCollection : EntityCollection<GrouponOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all GrouponOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GrouponOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new GrouponOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static GrouponOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GrouponOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            GrouponOrderSearchCollection collection = new GrouponOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of GrouponStoreEntity
	/// </summary>
	public class GrouponStoreCollection : EntityCollection<GrouponStoreEntity>
	{
        /// <summary>
        /// Gets the count of all GrouponStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GrouponStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new GrouponStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static GrouponStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GrouponStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            GrouponStoreCollection collection = new GrouponStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of InfopiaOrderItemEntity
	/// </summary>
	public class InfopiaOrderItemCollection : EntityCollection<InfopiaOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all InfopiaOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all InfopiaOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new InfopiaOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static InfopiaOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static InfopiaOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            InfopiaOrderItemCollection collection = new InfopiaOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of InfopiaStoreEntity
	/// </summary>
	public class InfopiaStoreCollection : EntityCollection<InfopiaStoreEntity>
	{
        /// <summary>
        /// Gets the count of all InfopiaStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all InfopiaStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new InfopiaStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static InfopiaStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static InfopiaStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            InfopiaStoreCollection collection = new InfopiaStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of InsurancePolicyEntity
	/// </summary>
	public class InsurancePolicyCollection : EntityCollection<InsurancePolicyEntity>
	{
        /// <summary>
        /// Gets the count of all InsurancePolicyEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all InsurancePolicyEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new InsurancePolicyEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static InsurancePolicyCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static InsurancePolicyCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            InsurancePolicyCollection collection = new InsurancePolicyCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of IParcelAccountEntity
	/// </summary>
	public class IParcelAccountCollection : EntityCollection<IParcelAccountEntity>
	{
        /// <summary>
        /// Gets the count of all IParcelAccountEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all IParcelAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new IParcelAccountEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static IParcelAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static IParcelAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            IParcelAccountCollection collection = new IParcelAccountCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of IParcelPackageEntity
	/// </summary>
	public class IParcelPackageCollection : EntityCollection<IParcelPackageEntity>
	{
        /// <summary>
        /// Gets the count of all IParcelPackageEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all IParcelPackageEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new IParcelPackageEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static IParcelPackageCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static IParcelPackageCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            IParcelPackageCollection collection = new IParcelPackageCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of IParcelProfileEntity
	/// </summary>
	public class IParcelProfileCollection : EntityCollection<IParcelProfileEntity>
	{
        /// <summary>
        /// Gets the count of all IParcelProfileEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all IParcelProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new IParcelProfileEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static IParcelProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static IParcelProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            IParcelProfileCollection collection = new IParcelProfileCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of IParcelShipmentEntity
	/// </summary>
	public class IParcelShipmentCollection : EntityCollection<IParcelShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all IParcelShipmentEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all IParcelShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new IParcelShipmentEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static IParcelShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static IParcelShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            IParcelShipmentCollection collection = new IParcelShipmentCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of JetOrderEntity
	/// </summary>
	public class JetOrderCollection : EntityCollection<JetOrderEntity>
	{
        /// <summary>
        /// Gets the count of all JetOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all JetOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new JetOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static JetOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static JetOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            JetOrderCollection collection = new JetOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of JetOrderItemEntity
	/// </summary>
	public class JetOrderItemCollection : EntityCollection<JetOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all JetOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all JetOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new JetOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static JetOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static JetOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            JetOrderItemCollection collection = new JetOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of JetOrderSearchEntity
	/// </summary>
	public class JetOrderSearchCollection : EntityCollection<JetOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all JetOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all JetOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new JetOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static JetOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static JetOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            JetOrderSearchCollection collection = new JetOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of JetStoreEntity
	/// </summary>
	public class JetStoreCollection : EntityCollection<JetStoreEntity>
	{
        /// <summary>
        /// Gets the count of all JetStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all JetStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new JetStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static JetStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static JetStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            JetStoreCollection collection = new JetStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of LabelSheetEntity
	/// </summary>
	public class LabelSheetCollection : EntityCollection<LabelSheetEntity>
	{
        /// <summary>
        /// Gets the count of all LabelSheetEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all LabelSheetEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new LabelSheetEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static LabelSheetCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static LabelSheetCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            LabelSheetCollection collection = new LabelSheetCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of LemonStandOrderEntity
	/// </summary>
	public class LemonStandOrderCollection : EntityCollection<LemonStandOrderEntity>
	{
        /// <summary>
        /// Gets the count of all LemonStandOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all LemonStandOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new LemonStandOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static LemonStandOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static LemonStandOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            LemonStandOrderCollection collection = new LemonStandOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of LemonStandOrderItemEntity
	/// </summary>
	public class LemonStandOrderItemCollection : EntityCollection<LemonStandOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all LemonStandOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all LemonStandOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new LemonStandOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static LemonStandOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static LemonStandOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            LemonStandOrderItemCollection collection = new LemonStandOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of LemonStandOrderSearchEntity
	/// </summary>
	public class LemonStandOrderSearchCollection : EntityCollection<LemonStandOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all LemonStandOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all LemonStandOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new LemonStandOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static LemonStandOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static LemonStandOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            LemonStandOrderSearchCollection collection = new LemonStandOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of LemonStandStoreEntity
	/// </summary>
	public class LemonStandStoreCollection : EntityCollection<LemonStandStoreEntity>
	{
        /// <summary>
        /// Gets the count of all LemonStandStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all LemonStandStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new LemonStandStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static LemonStandStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static LemonStandStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            LemonStandStoreCollection collection = new LemonStandStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of MagentoOrderEntity
	/// </summary>
	public class MagentoOrderCollection : EntityCollection<MagentoOrderEntity>
	{
        /// <summary>
        /// Gets the count of all MagentoOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all MagentoOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new MagentoOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static MagentoOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static MagentoOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            MagentoOrderCollection collection = new MagentoOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of MagentoOrderSearchEntity
	/// </summary>
	public class MagentoOrderSearchCollection : EntityCollection<MagentoOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all MagentoOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all MagentoOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new MagentoOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static MagentoOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static MagentoOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            MagentoOrderSearchCollection collection = new MagentoOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of MagentoStoreEntity
	/// </summary>
	public class MagentoStoreCollection : EntityCollection<MagentoStoreEntity>
	{
        /// <summary>
        /// Gets the count of all MagentoStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all MagentoStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new MagentoStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static MagentoStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static MagentoStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            MagentoStoreCollection collection = new MagentoStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of MarketplaceAdvisorOrderEntity
	/// </summary>
	public class MarketplaceAdvisorOrderCollection : EntityCollection<MarketplaceAdvisorOrderEntity>
	{
        /// <summary>
        /// Gets the count of all MarketplaceAdvisorOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all MarketplaceAdvisorOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new MarketplaceAdvisorOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static MarketplaceAdvisorOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static MarketplaceAdvisorOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            MarketplaceAdvisorOrderCollection collection = new MarketplaceAdvisorOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of MarketplaceAdvisorOrderSearchEntity
	/// </summary>
	public class MarketplaceAdvisorOrderSearchCollection : EntityCollection<MarketplaceAdvisorOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all MarketplaceAdvisorOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all MarketplaceAdvisorOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new MarketplaceAdvisorOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static MarketplaceAdvisorOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static MarketplaceAdvisorOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            MarketplaceAdvisorOrderSearchCollection collection = new MarketplaceAdvisorOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of MarketplaceAdvisorStoreEntity
	/// </summary>
	public class MarketplaceAdvisorStoreCollection : EntityCollection<MarketplaceAdvisorStoreEntity>
	{
        /// <summary>
        /// Gets the count of all MarketplaceAdvisorStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all MarketplaceAdvisorStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new MarketplaceAdvisorStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static MarketplaceAdvisorStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static MarketplaceAdvisorStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            MarketplaceAdvisorStoreCollection collection = new MarketplaceAdvisorStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of MivaOrderItemAttributeEntity
	/// </summary>
	public class MivaOrderItemAttributeCollection : EntityCollection<MivaOrderItemAttributeEntity>
	{
        /// <summary>
        /// Gets the count of all MivaOrderItemAttributeEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all MivaOrderItemAttributeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new MivaOrderItemAttributeEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static MivaOrderItemAttributeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static MivaOrderItemAttributeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            MivaOrderItemAttributeCollection collection = new MivaOrderItemAttributeCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of MivaStoreEntity
	/// </summary>
	public class MivaStoreCollection : EntityCollection<MivaStoreEntity>
	{
        /// <summary>
        /// Gets the count of all MivaStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all MivaStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new MivaStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static MivaStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static MivaStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            MivaStoreCollection collection = new MivaStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of NetworkSolutionsOrderEntity
	/// </summary>
	public class NetworkSolutionsOrderCollection : EntityCollection<NetworkSolutionsOrderEntity>
	{
        /// <summary>
        /// Gets the count of all NetworkSolutionsOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all NetworkSolutionsOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new NetworkSolutionsOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static NetworkSolutionsOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static NetworkSolutionsOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            NetworkSolutionsOrderCollection collection = new NetworkSolutionsOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of NetworkSolutionsOrderSearchEntity
	/// </summary>
	public class NetworkSolutionsOrderSearchCollection : EntityCollection<NetworkSolutionsOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all NetworkSolutionsOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all NetworkSolutionsOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new NetworkSolutionsOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static NetworkSolutionsOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static NetworkSolutionsOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            NetworkSolutionsOrderSearchCollection collection = new NetworkSolutionsOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of NetworkSolutionsStoreEntity
	/// </summary>
	public class NetworkSolutionsStoreCollection : EntityCollection<NetworkSolutionsStoreEntity>
	{
        /// <summary>
        /// Gets the count of all NetworkSolutionsStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all NetworkSolutionsStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new NetworkSolutionsStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static NetworkSolutionsStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static NetworkSolutionsStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            NetworkSolutionsStoreCollection collection = new NetworkSolutionsStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of NeweggOrderEntity
	/// </summary>
	public class NeweggOrderCollection : EntityCollection<NeweggOrderEntity>
	{
        /// <summary>
        /// Gets the count of all NeweggOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all NeweggOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new NeweggOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static NeweggOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static NeweggOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            NeweggOrderCollection collection = new NeweggOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of NeweggOrderItemEntity
	/// </summary>
	public class NeweggOrderItemCollection : EntityCollection<NeweggOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all NeweggOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all NeweggOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new NeweggOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static NeweggOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static NeweggOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            NeweggOrderItemCollection collection = new NeweggOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of NeweggStoreEntity
	/// </summary>
	public class NeweggStoreCollection : EntityCollection<NeweggStoreEntity>
	{
        /// <summary>
        /// Gets the count of all NeweggStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all NeweggStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new NeweggStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static NeweggStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static NeweggStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            NeweggStoreCollection collection = new NeweggStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of NoteEntity
	/// </summary>
	public class NoteCollection : EntityCollection<NoteEntity>
	{
        /// <summary>
        /// Gets the count of all NoteEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all NoteEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new NoteEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static NoteCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static NoteCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            NoteCollection collection = new NoteCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ObjectLabelEntity
	/// </summary>
	public class ObjectLabelCollection : EntityCollection<ObjectLabelEntity>
	{
        /// <summary>
        /// Gets the count of all ObjectLabelEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ObjectLabelEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ObjectLabelEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ObjectLabelCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ObjectLabelCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ObjectLabelCollection collection = new ObjectLabelCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ObjectReferenceEntity
	/// </summary>
	public class ObjectReferenceCollection : EntityCollection<ObjectReferenceEntity>
	{
        /// <summary>
        /// Gets the count of all ObjectReferenceEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ObjectReferenceEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ObjectReferenceEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ObjectReferenceCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ObjectReferenceCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ObjectReferenceCollection collection = new ObjectReferenceCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OdbcStoreEntity
	/// </summary>
	public class OdbcStoreCollection : EntityCollection<OdbcStoreEntity>
	{
        /// <summary>
        /// Gets the count of all OdbcStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OdbcStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OdbcStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OdbcStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OdbcStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OdbcStoreCollection collection = new OdbcStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OnTracAccountEntity
	/// </summary>
	public class OnTracAccountCollection : EntityCollection<OnTracAccountEntity>
	{
        /// <summary>
        /// Gets the count of all OnTracAccountEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OnTracAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OnTracAccountEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OnTracAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OnTracAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OnTracAccountCollection collection = new OnTracAccountCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OnTracProfileEntity
	/// </summary>
	public class OnTracProfileCollection : EntityCollection<OnTracProfileEntity>
	{
        /// <summary>
        /// Gets the count of all OnTracProfileEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OnTracProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OnTracProfileEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OnTracProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OnTracProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OnTracProfileCollection collection = new OnTracProfileCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OnTracShipmentEntity
	/// </summary>
	public class OnTracShipmentCollection : EntityCollection<OnTracShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all OnTracShipmentEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OnTracShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OnTracShipmentEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OnTracShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OnTracShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OnTracShipmentCollection collection = new OnTracShipmentCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OrderEntity
	/// </summary>
	public class OrderCollection : EntityCollection<OrderEntity>
	{
        /// <summary>
        /// Gets the count of all OrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OrderCollection collection = new OrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OrderChargeEntity
	/// </summary>
	public class OrderChargeCollection : EntityCollection<OrderChargeEntity>
	{
        /// <summary>
        /// Gets the count of all OrderChargeEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OrderChargeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OrderChargeEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OrderChargeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OrderChargeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OrderChargeCollection collection = new OrderChargeCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OrderItemEntity
	/// </summary>
	public class OrderItemCollection : EntityCollection<OrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all OrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OrderItemCollection collection = new OrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OrderItemAttributeEntity
	/// </summary>
	public class OrderItemAttributeCollection : EntityCollection<OrderItemAttributeEntity>
	{
        /// <summary>
        /// Gets the count of all OrderItemAttributeEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OrderItemAttributeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OrderItemAttributeEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OrderItemAttributeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OrderItemAttributeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OrderItemAttributeCollection collection = new OrderItemAttributeCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OrderMotionOrderEntity
	/// </summary>
	public class OrderMotionOrderCollection : EntityCollection<OrderMotionOrderEntity>
	{
        /// <summary>
        /// Gets the count of all OrderMotionOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OrderMotionOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OrderMotionOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OrderMotionOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OrderMotionOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OrderMotionOrderCollection collection = new OrderMotionOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OrderMotionOrderSearchEntity
	/// </summary>
	public class OrderMotionOrderSearchCollection : EntityCollection<OrderMotionOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all OrderMotionOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OrderMotionOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OrderMotionOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OrderMotionOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OrderMotionOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OrderMotionOrderSearchCollection collection = new OrderMotionOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OrderMotionStoreEntity
	/// </summary>
	public class OrderMotionStoreCollection : EntityCollection<OrderMotionStoreEntity>
	{
        /// <summary>
        /// Gets the count of all OrderMotionStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OrderMotionStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OrderMotionStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OrderMotionStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OrderMotionStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OrderMotionStoreCollection collection = new OrderMotionStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OrderPaymentDetailEntity
	/// </summary>
	public class OrderPaymentDetailCollection : EntityCollection<OrderPaymentDetailEntity>
	{
        /// <summary>
        /// Gets the count of all OrderPaymentDetailEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OrderPaymentDetailEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OrderPaymentDetailEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OrderPaymentDetailCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OrderPaymentDetailCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OrderPaymentDetailCollection collection = new OrderPaymentDetailCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OrderSearchEntity
	/// </summary>
	public class OrderSearchCollection : EntityCollection<OrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all OrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OrderSearchCollection collection = new OrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OtherProfileEntity
	/// </summary>
	public class OtherProfileCollection : EntityCollection<OtherProfileEntity>
	{
        /// <summary>
        /// Gets the count of all OtherProfileEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OtherProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OtherProfileEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OtherProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OtherProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OtherProfileCollection collection = new OtherProfileCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OtherShipmentEntity
	/// </summary>
	public class OtherShipmentCollection : EntityCollection<OtherShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all OtherShipmentEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OtherShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OtherShipmentEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OtherShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OtherShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OtherShipmentCollection collection = new OtherShipmentCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OverstockOrderEntity
	/// </summary>
	public class OverstockOrderCollection : EntityCollection<OverstockOrderEntity>
	{
        /// <summary>
        /// Gets the count of all OverstockOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OverstockOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OverstockOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OverstockOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OverstockOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OverstockOrderCollection collection = new OverstockOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OverstockOrderItemEntity
	/// </summary>
	public class OverstockOrderItemCollection : EntityCollection<OverstockOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all OverstockOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OverstockOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OverstockOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OverstockOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OverstockOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OverstockOrderItemCollection collection = new OverstockOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OverstockOrderSearchEntity
	/// </summary>
	public class OverstockOrderSearchCollection : EntityCollection<OverstockOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all OverstockOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OverstockOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OverstockOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OverstockOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OverstockOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OverstockOrderSearchCollection collection = new OverstockOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of OverstockStoreEntity
	/// </summary>
	public class OverstockStoreCollection : EntityCollection<OverstockStoreEntity>
	{
        /// <summary>
        /// Gets the count of all OverstockStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OverstockStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new OverstockStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static OverstockStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OverstockStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            OverstockStoreCollection collection = new OverstockStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of PackageProfileEntity
	/// </summary>
	public class PackageProfileCollection : EntityCollection<PackageProfileEntity>
	{
        /// <summary>
        /// Gets the count of all PackageProfileEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all PackageProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new PackageProfileEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static PackageProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static PackageProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            PackageProfileCollection collection = new PackageProfileCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of PayPalOrderEntity
	/// </summary>
	public class PayPalOrderCollection : EntityCollection<PayPalOrderEntity>
	{
        /// <summary>
        /// Gets the count of all PayPalOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all PayPalOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new PayPalOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static PayPalOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static PayPalOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            PayPalOrderCollection collection = new PayPalOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of PayPalOrderSearchEntity
	/// </summary>
	public class PayPalOrderSearchCollection : EntityCollection<PayPalOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all PayPalOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all PayPalOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new PayPalOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static PayPalOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static PayPalOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            PayPalOrderSearchCollection collection = new PayPalOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of PayPalStoreEntity
	/// </summary>
	public class PayPalStoreCollection : EntityCollection<PayPalStoreEntity>
	{
        /// <summary>
        /// Gets the count of all PayPalStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all PayPalStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new PayPalStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static PayPalStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static PayPalStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            PayPalStoreCollection collection = new PayPalStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of PermissionEntity
	/// </summary>
	public class PermissionCollection : EntityCollection<PermissionEntity>
	{
        /// <summary>
        /// Gets the count of all PermissionEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all PermissionEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new PermissionEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static PermissionCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static PermissionCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            PermissionCollection collection = new PermissionCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of PostalProfileEntity
	/// </summary>
	public class PostalProfileCollection : EntityCollection<PostalProfileEntity>
	{
        /// <summary>
        /// Gets the count of all PostalProfileEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all PostalProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new PostalProfileEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static PostalProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static PostalProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            PostalProfileCollection collection = new PostalProfileCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of PostalShipmentEntity
	/// </summary>
	public class PostalShipmentCollection : EntityCollection<PostalShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all PostalShipmentEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all PostalShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new PostalShipmentEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static PostalShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static PostalShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            PostalShipmentCollection collection = new PostalShipmentCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of PrintResultEntity
	/// </summary>
	public class PrintResultCollection : EntityCollection<PrintResultEntity>
	{
        /// <summary>
        /// Gets the count of all PrintResultEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all PrintResultEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new PrintResultEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static PrintResultCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static PrintResultCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            PrintResultCollection collection = new PrintResultCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ProcessedShipmentEntity
	/// </summary>
	public class ProcessedShipmentCollection : EntityCollection<ProcessedShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all ProcessedShipmentEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ProcessedShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ProcessedShipmentEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ProcessedShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ProcessedShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ProcessedShipmentCollection collection = new ProcessedShipmentCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ProductEntity
	/// </summary>
	public class ProductCollection : EntityCollection<ProductEntity>
	{
        /// <summary>
        /// Gets the count of all ProductEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ProductEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ProductEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ProductCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ProductCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ProductCollection collection = new ProductCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ProductAttributeEntity
	/// </summary>
	public class ProductAttributeCollection : EntityCollection<ProductAttributeEntity>
	{
        /// <summary>
        /// Gets the count of all ProductAttributeEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ProductAttributeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ProductAttributeEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ProductAttributeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ProductAttributeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ProductAttributeCollection collection = new ProductAttributeCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ProductBundleEntity
	/// </summary>
	public class ProductBundleCollection : EntityCollection<ProductBundleEntity>
	{
        /// <summary>
        /// Gets the count of all ProductBundleEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ProductBundleEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ProductBundleEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ProductBundleCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ProductBundleCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ProductBundleCollection collection = new ProductBundleCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ProductListItemEntity
	/// </summary>
	public class ProductListItemCollection : EntityCollection<ProductListItemEntity>
	{
        /// <summary>
        /// Gets the count of all ProductListItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ProductListItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ProductListItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ProductListItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ProductListItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ProductListItemCollection collection = new ProductListItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ProductVariantEntity
	/// </summary>
	public class ProductVariantCollection : EntityCollection<ProductVariantEntity>
	{
        /// <summary>
        /// Gets the count of all ProductVariantEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ProductVariantEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ProductVariantEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ProductVariantCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ProductVariantCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ProductVariantCollection collection = new ProductVariantCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ProductVariantAliasEntity
	/// </summary>
	public class ProductVariantAliasCollection : EntityCollection<ProductVariantAliasEntity>
	{
        /// <summary>
        /// Gets the count of all ProductVariantAliasEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ProductVariantAliasEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ProductVariantAliasEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ProductVariantAliasCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ProductVariantAliasCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ProductVariantAliasCollection collection = new ProductVariantAliasCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ProductVariantAttributeEntity
	/// </summary>
	public class ProductVariantAttributeCollection : EntityCollection<ProductVariantAttributeEntity>
	{
        /// <summary>
        /// Gets the count of all ProductVariantAttributeEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ProductVariantAttributeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ProductVariantAttributeEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ProductVariantAttributeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ProductVariantAttributeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ProductVariantAttributeCollection collection = new ProductVariantAttributeCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ProStoresOrderEntity
	/// </summary>
	public class ProStoresOrderCollection : EntityCollection<ProStoresOrderEntity>
	{
        /// <summary>
        /// Gets the count of all ProStoresOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ProStoresOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ProStoresOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ProStoresOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ProStoresOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ProStoresOrderCollection collection = new ProStoresOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ProStoresOrderSearchEntity
	/// </summary>
	public class ProStoresOrderSearchCollection : EntityCollection<ProStoresOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all ProStoresOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ProStoresOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ProStoresOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ProStoresOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ProStoresOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ProStoresOrderSearchCollection collection = new ProStoresOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ProStoresStoreEntity
	/// </summary>
	public class ProStoresStoreCollection : EntityCollection<ProStoresStoreEntity>
	{
        /// <summary>
        /// Gets the count of all ProStoresStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ProStoresStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ProStoresStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ProStoresStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ProStoresStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ProStoresStoreCollection collection = new ProStoresStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ResourceEntity
	/// </summary>
	public class ResourceCollection : EntityCollection<ResourceEntity>
	{
        /// <summary>
        /// Gets the count of all ResourceEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ResourceEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ResourceEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ResourceCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ResourceCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ResourceCollection collection = new ResourceCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ScanFormBatchEntity
	/// </summary>
	public class ScanFormBatchCollection : EntityCollection<ScanFormBatchEntity>
	{
        /// <summary>
        /// Gets the count of all ScanFormBatchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ScanFormBatchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ScanFormBatchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ScanFormBatchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ScanFormBatchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ScanFormBatchCollection collection = new ScanFormBatchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of SearchEntity
	/// </summary>
	public class SearchCollection : EntityCollection<SearchEntity>
	{
        /// <summary>
        /// Gets the count of all SearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all SearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new SearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static SearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static SearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            SearchCollection collection = new SearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of SearsOrderEntity
	/// </summary>
	public class SearsOrderCollection : EntityCollection<SearsOrderEntity>
	{
        /// <summary>
        /// Gets the count of all SearsOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all SearsOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new SearsOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static SearsOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static SearsOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            SearsOrderCollection collection = new SearsOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of SearsOrderItemEntity
	/// </summary>
	public class SearsOrderItemCollection : EntityCollection<SearsOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all SearsOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all SearsOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new SearsOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static SearsOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static SearsOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            SearsOrderItemCollection collection = new SearsOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of SearsOrderSearchEntity
	/// </summary>
	public class SearsOrderSearchCollection : EntityCollection<SearsOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all SearsOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all SearsOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new SearsOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static SearsOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static SearsOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            SearsOrderSearchCollection collection = new SearsOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of SearsStoreEntity
	/// </summary>
	public class SearsStoreCollection : EntityCollection<SearsStoreEntity>
	{
        /// <summary>
        /// Gets the count of all SearsStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all SearsStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new SearsStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static SearsStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static SearsStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            SearsStoreCollection collection = new SearsStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ServerMessageEntity
	/// </summary>
	public class ServerMessageCollection : EntityCollection<ServerMessageEntity>
	{
        /// <summary>
        /// Gets the count of all ServerMessageEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ServerMessageEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ServerMessageEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ServerMessageCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ServerMessageCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ServerMessageCollection collection = new ServerMessageCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ServerMessageSignoffEntity
	/// </summary>
	public class ServerMessageSignoffCollection : EntityCollection<ServerMessageSignoffEntity>
	{
        /// <summary>
        /// Gets the count of all ServerMessageSignoffEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ServerMessageSignoffEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ServerMessageSignoffEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ServerMessageSignoffCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ServerMessageSignoffCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ServerMessageSignoffCollection collection = new ServerMessageSignoffCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ServiceStatusEntity
	/// </summary>
	public class ServiceStatusCollection : EntityCollection<ServiceStatusEntity>
	{
        /// <summary>
        /// Gets the count of all ServiceStatusEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ServiceStatusEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ServiceStatusEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ServiceStatusCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ServiceStatusCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ServiceStatusCollection collection = new ServiceStatusCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShipmentEntity
	/// </summary>
	public class ShipmentCollection : EntityCollection<ShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all ShipmentEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShipmentEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShipmentCollection collection = new ShipmentCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShipmentCustomsItemEntity
	/// </summary>
	public class ShipmentCustomsItemCollection : EntityCollection<ShipmentCustomsItemEntity>
	{
        /// <summary>
        /// Gets the count of all ShipmentCustomsItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShipmentCustomsItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShipmentCustomsItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShipmentCustomsItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShipmentCustomsItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShipmentCustomsItemCollection collection = new ShipmentCustomsItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShipmentReturnItemEntity
	/// </summary>
	public class ShipmentReturnItemCollection : EntityCollection<ShipmentReturnItemEntity>
	{
        /// <summary>
        /// Gets the count of all ShipmentReturnItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShipmentReturnItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShipmentReturnItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShipmentReturnItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShipmentReturnItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShipmentReturnItemCollection collection = new ShipmentReturnItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShippingDefaultsRuleEntity
	/// </summary>
	public class ShippingDefaultsRuleCollection : EntityCollection<ShippingDefaultsRuleEntity>
	{
        /// <summary>
        /// Gets the count of all ShippingDefaultsRuleEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShippingDefaultsRuleEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShippingDefaultsRuleEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShippingDefaultsRuleCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShippingDefaultsRuleCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShippingDefaultsRuleCollection collection = new ShippingDefaultsRuleCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShippingOriginEntity
	/// </summary>
	public class ShippingOriginCollection : EntityCollection<ShippingOriginEntity>
	{
        /// <summary>
        /// Gets the count of all ShippingOriginEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShippingOriginEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShippingOriginEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShippingOriginCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShippingOriginCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShippingOriginCollection collection = new ShippingOriginCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShippingPrintOutputEntity
	/// </summary>
	public class ShippingPrintOutputCollection : EntityCollection<ShippingPrintOutputEntity>
	{
        /// <summary>
        /// Gets the count of all ShippingPrintOutputEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShippingPrintOutputEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShippingPrintOutputEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShippingPrintOutputCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShippingPrintOutputCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShippingPrintOutputCollection collection = new ShippingPrintOutputCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShippingPrintOutputRuleEntity
	/// </summary>
	public class ShippingPrintOutputRuleCollection : EntityCollection<ShippingPrintOutputRuleEntity>
	{
        /// <summary>
        /// Gets the count of all ShippingPrintOutputRuleEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShippingPrintOutputRuleEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShippingPrintOutputRuleEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShippingPrintOutputRuleCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShippingPrintOutputRuleCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShippingPrintOutputRuleCollection collection = new ShippingPrintOutputRuleCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShippingProfileEntity
	/// </summary>
	public class ShippingProfileCollection : EntityCollection<ShippingProfileEntity>
	{
        /// <summary>
        /// Gets the count of all ShippingProfileEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShippingProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShippingProfileEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShippingProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShippingProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShippingProfileCollection collection = new ShippingProfileCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShippingProviderRuleEntity
	/// </summary>
	public class ShippingProviderRuleCollection : EntityCollection<ShippingProviderRuleEntity>
	{
        /// <summary>
        /// Gets the count of all ShippingProviderRuleEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShippingProviderRuleEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShippingProviderRuleEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShippingProviderRuleCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShippingProviderRuleCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShippingProviderRuleCollection collection = new ShippingProviderRuleCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShippingSettingsEntity
	/// </summary>
	public class ShippingSettingsCollection : EntityCollection<ShippingSettingsEntity>
	{
        /// <summary>
        /// Gets the count of all ShippingSettingsEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShippingSettingsEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShippingSettingsEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShippingSettingsCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShippingSettingsCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShippingSettingsCollection collection = new ShippingSettingsCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShipSenseKnowledgebaseEntity
	/// </summary>
	public class ShipSenseKnowledgebaseCollection : EntityCollection<ShipSenseKnowledgebaseEntity>
	{
        /// <summary>
        /// Gets the count of all ShipSenseKnowledgebaseEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShipSenseKnowledgebaseEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShipSenseKnowledgebaseEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShipSenseKnowledgebaseCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShipSenseKnowledgebaseCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShipSenseKnowledgebaseCollection collection = new ShipSenseKnowledgebaseCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShopifyOrderEntity
	/// </summary>
	public class ShopifyOrderCollection : EntityCollection<ShopifyOrderEntity>
	{
        /// <summary>
        /// Gets the count of all ShopifyOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShopifyOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShopifyOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShopifyOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShopifyOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShopifyOrderCollection collection = new ShopifyOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShopifyOrderItemEntity
	/// </summary>
	public class ShopifyOrderItemCollection : EntityCollection<ShopifyOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all ShopifyOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShopifyOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShopifyOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShopifyOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShopifyOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShopifyOrderItemCollection collection = new ShopifyOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShopifyOrderSearchEntity
	/// </summary>
	public class ShopifyOrderSearchCollection : EntityCollection<ShopifyOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all ShopifyOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShopifyOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShopifyOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShopifyOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShopifyOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShopifyOrderSearchCollection collection = new ShopifyOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShopifyStoreEntity
	/// </summary>
	public class ShopifyStoreCollection : EntityCollection<ShopifyStoreEntity>
	{
        /// <summary>
        /// Gets the count of all ShopifyStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShopifyStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShopifyStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShopifyStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShopifyStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShopifyStoreCollection collection = new ShopifyStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShopSiteStoreEntity
	/// </summary>
	public class ShopSiteStoreCollection : EntityCollection<ShopSiteStoreEntity>
	{
        /// <summary>
        /// Gets the count of all ShopSiteStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShopSiteStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShopSiteStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShopSiteStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShopSiteStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShopSiteStoreCollection collection = new ShopSiteStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ShortcutEntity
	/// </summary>
	public class ShortcutCollection : EntityCollection<ShortcutEntity>
	{
        /// <summary>
        /// Gets the count of all ShortcutEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShortcutEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ShortcutEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ShortcutCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShortcutCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ShortcutCollection collection = new ShortcutCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of SparkPayStoreEntity
	/// </summary>
	public class SparkPayStoreCollection : EntityCollection<SparkPayStoreEntity>
	{
        /// <summary>
        /// Gets the count of all SparkPayStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all SparkPayStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new SparkPayStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static SparkPayStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static SparkPayStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            SparkPayStoreCollection collection = new SparkPayStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of StatusPresetEntity
	/// </summary>
	public class StatusPresetCollection : EntityCollection<StatusPresetEntity>
	{
        /// <summary>
        /// Gets the count of all StatusPresetEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all StatusPresetEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new StatusPresetEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static StatusPresetCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static StatusPresetCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            StatusPresetCollection collection = new StatusPresetCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of StoreEntity
	/// </summary>
	public class StoreCollection : EntityCollection<StoreEntity>
	{
        /// <summary>
        /// Gets the count of all StoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all StoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new StoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static StoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static StoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            StoreCollection collection = new StoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of SystemDataEntity
	/// </summary>
	public class SystemDataCollection : EntityCollection<SystemDataEntity>
	{
        /// <summary>
        /// Gets the count of all SystemDataEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all SystemDataEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new SystemDataEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static SystemDataCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static SystemDataCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            SystemDataCollection collection = new SystemDataCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of TemplateEntity
	/// </summary>
	public class TemplateCollection : EntityCollection<TemplateEntity>
	{
        /// <summary>
        /// Gets the count of all TemplateEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all TemplateEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new TemplateEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static TemplateCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static TemplateCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            TemplateCollection collection = new TemplateCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of TemplateComputerSettingsEntity
	/// </summary>
	public class TemplateComputerSettingsCollection : EntityCollection<TemplateComputerSettingsEntity>
	{
        /// <summary>
        /// Gets the count of all TemplateComputerSettingsEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all TemplateComputerSettingsEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new TemplateComputerSettingsEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static TemplateComputerSettingsCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static TemplateComputerSettingsCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            TemplateComputerSettingsCollection collection = new TemplateComputerSettingsCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of TemplateFolderEntity
	/// </summary>
	public class TemplateFolderCollection : EntityCollection<TemplateFolderEntity>
	{
        /// <summary>
        /// Gets the count of all TemplateFolderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all TemplateFolderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new TemplateFolderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static TemplateFolderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static TemplateFolderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            TemplateFolderCollection collection = new TemplateFolderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of TemplateStoreSettingsEntity
	/// </summary>
	public class TemplateStoreSettingsCollection : EntityCollection<TemplateStoreSettingsEntity>
	{
        /// <summary>
        /// Gets the count of all TemplateStoreSettingsEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all TemplateStoreSettingsEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new TemplateStoreSettingsEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static TemplateStoreSettingsCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static TemplateStoreSettingsCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            TemplateStoreSettingsCollection collection = new TemplateStoreSettingsCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of TemplateUserSettingsEntity
	/// </summary>
	public class TemplateUserSettingsCollection : EntityCollection<TemplateUserSettingsEntity>
	{
        /// <summary>
        /// Gets the count of all TemplateUserSettingsEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all TemplateUserSettingsEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new TemplateUserSettingsEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static TemplateUserSettingsCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static TemplateUserSettingsCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            TemplateUserSettingsCollection collection = new TemplateUserSettingsCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ThreeDCartOrderEntity
	/// </summary>
	public class ThreeDCartOrderCollection : EntityCollection<ThreeDCartOrderEntity>
	{
        /// <summary>
        /// Gets the count of all ThreeDCartOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ThreeDCartOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ThreeDCartOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ThreeDCartOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ThreeDCartOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ThreeDCartOrderCollection collection = new ThreeDCartOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ThreeDCartOrderItemEntity
	/// </summary>
	public class ThreeDCartOrderItemCollection : EntityCollection<ThreeDCartOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all ThreeDCartOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ThreeDCartOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ThreeDCartOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ThreeDCartOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ThreeDCartOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ThreeDCartOrderItemCollection collection = new ThreeDCartOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ThreeDCartOrderSearchEntity
	/// </summary>
	public class ThreeDCartOrderSearchCollection : EntityCollection<ThreeDCartOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all ThreeDCartOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ThreeDCartOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ThreeDCartOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ThreeDCartOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ThreeDCartOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ThreeDCartOrderSearchCollection collection = new ThreeDCartOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ThreeDCartStoreEntity
	/// </summary>
	public class ThreeDCartStoreCollection : EntityCollection<ThreeDCartStoreEntity>
	{
        /// <summary>
        /// Gets the count of all ThreeDCartStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ThreeDCartStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ThreeDCartStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ThreeDCartStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ThreeDCartStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ThreeDCartStoreCollection collection = new ThreeDCartStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UpsAccountEntity
	/// </summary>
	public class UpsAccountCollection : EntityCollection<UpsAccountEntity>
	{
        /// <summary>
        /// Gets the count of all UpsAccountEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UpsAccountEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UpsAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UpsAccountCollection collection = new UpsAccountCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UpsLetterRateEntity
	/// </summary>
	public class UpsLetterRateCollection : EntityCollection<UpsLetterRateEntity>
	{
        /// <summary>
        /// Gets the count of all UpsLetterRateEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsLetterRateEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UpsLetterRateEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UpsLetterRateCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsLetterRateCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UpsLetterRateCollection collection = new UpsLetterRateCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UpsLocalRatingDeliveryAreaSurchargeEntity
	/// </summary>
	public class UpsLocalRatingDeliveryAreaSurchargeCollection : EntityCollection<UpsLocalRatingDeliveryAreaSurchargeEntity>
	{
        /// <summary>
        /// Gets the count of all UpsLocalRatingDeliveryAreaSurchargeEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsLocalRatingDeliveryAreaSurchargeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UpsLocalRatingDeliveryAreaSurchargeEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UpsLocalRatingDeliveryAreaSurchargeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsLocalRatingDeliveryAreaSurchargeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UpsLocalRatingDeliveryAreaSurchargeCollection collection = new UpsLocalRatingDeliveryAreaSurchargeCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UpsLocalRatingZoneEntity
	/// </summary>
	public class UpsLocalRatingZoneCollection : EntityCollection<UpsLocalRatingZoneEntity>
	{
        /// <summary>
        /// Gets the count of all UpsLocalRatingZoneEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsLocalRatingZoneEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UpsLocalRatingZoneEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UpsLocalRatingZoneCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsLocalRatingZoneCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UpsLocalRatingZoneCollection collection = new UpsLocalRatingZoneCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UpsLocalRatingZoneFileEntity
	/// </summary>
	public class UpsLocalRatingZoneFileCollection : EntityCollection<UpsLocalRatingZoneFileEntity>
	{
        /// <summary>
        /// Gets the count of all UpsLocalRatingZoneFileEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsLocalRatingZoneFileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UpsLocalRatingZoneFileEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UpsLocalRatingZoneFileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsLocalRatingZoneFileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UpsLocalRatingZoneFileCollection collection = new UpsLocalRatingZoneFileCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UpsPackageEntity
	/// </summary>
	public class UpsPackageCollection : EntityCollection<UpsPackageEntity>
	{
        /// <summary>
        /// Gets the count of all UpsPackageEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsPackageEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UpsPackageEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UpsPackageCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsPackageCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UpsPackageCollection collection = new UpsPackageCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UpsPackageRateEntity
	/// </summary>
	public class UpsPackageRateCollection : EntityCollection<UpsPackageRateEntity>
	{
        /// <summary>
        /// Gets the count of all UpsPackageRateEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsPackageRateEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UpsPackageRateEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UpsPackageRateCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsPackageRateCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UpsPackageRateCollection collection = new UpsPackageRateCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UpsPricePerPoundEntity
	/// </summary>
	public class UpsPricePerPoundCollection : EntityCollection<UpsPricePerPoundEntity>
	{
        /// <summary>
        /// Gets the count of all UpsPricePerPoundEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsPricePerPoundEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UpsPricePerPoundEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UpsPricePerPoundCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsPricePerPoundCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UpsPricePerPoundCollection collection = new UpsPricePerPoundCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UpsProfileEntity
	/// </summary>
	public class UpsProfileCollection : EntityCollection<UpsProfileEntity>
	{
        /// <summary>
        /// Gets the count of all UpsProfileEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UpsProfileEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UpsProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UpsProfileCollection collection = new UpsProfileCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UpsProfilePackageEntity
	/// </summary>
	public class UpsProfilePackageCollection : EntityCollection<UpsProfilePackageEntity>
	{
        /// <summary>
        /// Gets the count of all UpsProfilePackageEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsProfilePackageEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UpsProfilePackageEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UpsProfilePackageCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsProfilePackageCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UpsProfilePackageCollection collection = new UpsProfilePackageCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UpsRateSurchargeEntity
	/// </summary>
	public class UpsRateSurchargeCollection : EntityCollection<UpsRateSurchargeEntity>
	{
        /// <summary>
        /// Gets the count of all UpsRateSurchargeEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsRateSurchargeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UpsRateSurchargeEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UpsRateSurchargeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsRateSurchargeCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UpsRateSurchargeCollection collection = new UpsRateSurchargeCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UpsRateTableEntity
	/// </summary>
	public class UpsRateTableCollection : EntityCollection<UpsRateTableEntity>
	{
        /// <summary>
        /// Gets the count of all UpsRateTableEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsRateTableEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UpsRateTableEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UpsRateTableCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsRateTableCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UpsRateTableCollection collection = new UpsRateTableCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UpsShipmentEntity
	/// </summary>
	public class UpsShipmentCollection : EntityCollection<UpsShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all UpsShipmentEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UpsShipmentEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UpsShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UpsShipmentCollection collection = new UpsShipmentCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UserEntity
	/// </summary>
	public class UserCollection : EntityCollection<UserEntity>
	{
        /// <summary>
        /// Gets the count of all UserEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UserEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UserEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UserCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UserCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UserCollection collection = new UserCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UserColumnSettingsEntity
	/// </summary>
	public class UserColumnSettingsCollection : EntityCollection<UserColumnSettingsEntity>
	{
        /// <summary>
        /// Gets the count of all UserColumnSettingsEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UserColumnSettingsEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UserColumnSettingsEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UserColumnSettingsCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UserColumnSettingsCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UserColumnSettingsCollection collection = new UserColumnSettingsCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UserSettingsEntity
	/// </summary>
	public class UserSettingsCollection : EntityCollection<UserSettingsEntity>
	{
        /// <summary>
        /// Gets the count of all UserSettingsEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UserSettingsEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UserSettingsEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UserSettingsCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UserSettingsCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UserSettingsCollection collection = new UserSettingsCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UspsAccountEntity
	/// </summary>
	public class UspsAccountCollection : EntityCollection<UspsAccountEntity>
	{
        /// <summary>
        /// Gets the count of all UspsAccountEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UspsAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UspsAccountEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UspsAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UspsAccountCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UspsAccountCollection collection = new UspsAccountCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UspsProfileEntity
	/// </summary>
	public class UspsProfileCollection : EntityCollection<UspsProfileEntity>
	{
        /// <summary>
        /// Gets the count of all UspsProfileEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UspsProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UspsProfileEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UspsProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UspsProfileCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UspsProfileCollection collection = new UspsProfileCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UspsScanFormEntity
	/// </summary>
	public class UspsScanFormCollection : EntityCollection<UspsScanFormEntity>
	{
        /// <summary>
        /// Gets the count of all UspsScanFormEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UspsScanFormEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UspsScanFormEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UspsScanFormCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UspsScanFormCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UspsScanFormCollection collection = new UspsScanFormCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of UspsShipmentEntity
	/// </summary>
	public class UspsShipmentCollection : EntityCollection<UspsShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all UspsShipmentEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UspsShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new UspsShipmentEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static UspsShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UspsShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            UspsShipmentCollection collection = new UspsShipmentCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of ValidatedAddressEntity
	/// </summary>
	public class ValidatedAddressCollection : EntityCollection<ValidatedAddressEntity>
	{
        /// <summary>
        /// Gets the count of all ValidatedAddressEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ValidatedAddressEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new ValidatedAddressEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static ValidatedAddressCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ValidatedAddressCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            ValidatedAddressCollection collection = new ValidatedAddressCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of VersionSignoffEntity
	/// </summary>
	public class VersionSignoffCollection : EntityCollection<VersionSignoffEntity>
	{
        /// <summary>
        /// Gets the count of all VersionSignoffEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all VersionSignoffEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new VersionSignoffEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static VersionSignoffCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static VersionSignoffCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            VersionSignoffCollection collection = new VersionSignoffCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of VolusionStoreEntity
	/// </summary>
	public class VolusionStoreCollection : EntityCollection<VolusionStoreEntity>
	{
        /// <summary>
        /// Gets the count of all VolusionStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all VolusionStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new VolusionStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static VolusionStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static VolusionStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            VolusionStoreCollection collection = new VolusionStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of WalmartOrderEntity
	/// </summary>
	public class WalmartOrderCollection : EntityCollection<WalmartOrderEntity>
	{
        /// <summary>
        /// Gets the count of all WalmartOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all WalmartOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new WalmartOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static WalmartOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static WalmartOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            WalmartOrderCollection collection = new WalmartOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of WalmartOrderItemEntity
	/// </summary>
	public class WalmartOrderItemCollection : EntityCollection<WalmartOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all WalmartOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all WalmartOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new WalmartOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static WalmartOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static WalmartOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            WalmartOrderItemCollection collection = new WalmartOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of WalmartOrderSearchEntity
	/// </summary>
	public class WalmartOrderSearchCollection : EntityCollection<WalmartOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all WalmartOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all WalmartOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new WalmartOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static WalmartOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static WalmartOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            WalmartOrderSearchCollection collection = new WalmartOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of WalmartStoreEntity
	/// </summary>
	public class WalmartStoreCollection : EntityCollection<WalmartStoreEntity>
	{
        /// <summary>
        /// Gets the count of all WalmartStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all WalmartStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new WalmartStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static WalmartStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static WalmartStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            WalmartStoreCollection collection = new WalmartStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of WorldShipGoodsEntity
	/// </summary>
	public class WorldShipGoodsCollection : EntityCollection<WorldShipGoodsEntity>
	{
        /// <summary>
        /// Gets the count of all WorldShipGoodsEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all WorldShipGoodsEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new WorldShipGoodsEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static WorldShipGoodsCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static WorldShipGoodsCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            WorldShipGoodsCollection collection = new WorldShipGoodsCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of WorldShipPackageEntity
	/// </summary>
	public class WorldShipPackageCollection : EntityCollection<WorldShipPackageEntity>
	{
        /// <summary>
        /// Gets the count of all WorldShipPackageEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all WorldShipPackageEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new WorldShipPackageEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static WorldShipPackageCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static WorldShipPackageCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            WorldShipPackageCollection collection = new WorldShipPackageCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of WorldShipProcessedEntity
	/// </summary>
	public class WorldShipProcessedCollection : EntityCollection<WorldShipProcessedEntity>
	{
        /// <summary>
        /// Gets the count of all WorldShipProcessedEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all WorldShipProcessedEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new WorldShipProcessedEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static WorldShipProcessedCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static WorldShipProcessedCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            WorldShipProcessedCollection collection = new WorldShipProcessedCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of WorldShipShipmentEntity
	/// </summary>
	public class WorldShipShipmentCollection : EntityCollection<WorldShipShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all WorldShipShipmentEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all WorldShipShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new WorldShipShipmentEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static WorldShipShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static WorldShipShipmentCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            WorldShipShipmentCollection collection = new WorldShipShipmentCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of YahooOrderEntity
	/// </summary>
	public class YahooOrderCollection : EntityCollection<YahooOrderEntity>
	{
        /// <summary>
        /// Gets the count of all YahooOrderEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all YahooOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new YahooOrderEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static YahooOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static YahooOrderCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            YahooOrderCollection collection = new YahooOrderCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of YahooOrderItemEntity
	/// </summary>
	public class YahooOrderItemCollection : EntityCollection<YahooOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all YahooOrderItemEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all YahooOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new YahooOrderItemEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static YahooOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static YahooOrderItemCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            YahooOrderItemCollection collection = new YahooOrderItemCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of YahooOrderSearchEntity
	/// </summary>
	public class YahooOrderSearchCollection : EntityCollection<YahooOrderSearchEntity>
	{
        /// <summary>
        /// Gets the count of all YahooOrderSearchEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all YahooOrderSearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new YahooOrderSearchEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static YahooOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static YahooOrderSearchCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            YahooOrderSearchCollection collection = new YahooOrderSearchCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of YahooProductEntity
	/// </summary>
	public class YahooProductCollection : EntityCollection<YahooProductEntity>
	{
        /// <summary>
        /// Gets the count of all YahooProductEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all YahooProductEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new YahooProductEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static YahooProductCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static YahooProductCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            YahooProductCollection collection = new YahooProductCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
	/// <summary>
	/// Strongly typed collection of YahooStoreEntity
	/// </summary>
	public class YahooStoreCollection : EntityCollection<YahooStoreEntity>
	{
        /// <summary>
        /// Gets the count of all YahooStoreEntity rows
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all YahooStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(IDataAccessAdapter adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new YahooStoreEntityFactory().CreateFields(), bucket);
        }

        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static YahooStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }

		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static YahooStoreCollection Fetch(IDataAccessAdapter adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            YahooStoreCollection collection = new YahooStoreCollection();

            RelationPredicateBucket bucket = null;

            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            adapter.FetchEntityCollection(collection, bucket, prefetchPath);

            return collection;
        }
	}

	
}