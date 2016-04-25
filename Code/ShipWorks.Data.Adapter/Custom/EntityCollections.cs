using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Adapter.Custom
{	
	/// <summary>
	/// Strongly typed collection of StoreEntity
	/// </summary>
	public class StoreCollection : EntityCollection<StoreEntity>
	{
        /// <summary>
        /// Gets the count of all StoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all StoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static StoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static StoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of AuditEntity
	/// </summary>
	public class AuditCollection : EntityCollection<AuditEntity>
	{
        /// <summary>
        /// Gets the count of all AuditEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AuditEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static AuditCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AuditCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ComputerEntity
	/// </summary>
	public class ComputerCollection : EntityCollection<ComputerEntity>
	{
        /// <summary>
        /// Gets the count of all ComputerEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ComputerEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ComputerCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ComputerCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of UserEntity
	/// </summary>
	public class UserCollection : EntityCollection<UserEntity>
	{
        /// <summary>
        /// Gets the count of all UserEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UserEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static UserCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UserCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of PermissionEntity
	/// </summary>
	public class PermissionCollection : EntityCollection<PermissionEntity>
	{
        /// <summary>
        /// Gets the count of all PermissionEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all PermissionEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static PermissionCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static PermissionCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of OrderEntity
	/// </summary>
	public class OrderCollection : EntityCollection<OrderEntity>
	{
        /// <summary>
        /// Gets the count of all OrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static OrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of FilterEntity
	/// </summary>
	public class FilterCollection : EntityCollection<FilterEntity>
	{
        /// <summary>
        /// Gets the count of all FilterEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FilterEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static FilterCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FilterCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of FilterNodeEntity
	/// </summary>
	public class FilterNodeCollection : EntityCollection<FilterNodeEntity>
	{
        /// <summary>
        /// Gets the count of all FilterNodeEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FilterNodeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static FilterNodeCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FilterNodeCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of FilterSequenceEntity
	/// </summary>
	public class FilterSequenceCollection : EntityCollection<FilterSequenceEntity>
	{
        /// <summary>
        /// Gets the count of all FilterSequenceEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FilterSequenceEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static FilterSequenceCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FilterSequenceCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of FilterLayoutEntity
	/// </summary>
	public class FilterLayoutCollection : EntityCollection<FilterLayoutEntity>
	{
        /// <summary>
        /// Gets the count of all FilterLayoutEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FilterLayoutEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static FilterLayoutCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FilterLayoutCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of CustomerEntity
	/// </summary>
	public class CustomerCollection : EntityCollection<CustomerEntity>
	{
        /// <summary>
        /// Gets the count of all CustomerEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all CustomerEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static CustomerCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static CustomerCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of OrderItemEntity
	/// </summary>
	public class OrderItemCollection : EntityCollection<OrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all OrderItemEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static OrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of FilterNodeContentEntity
	/// </summary>
	public class FilterNodeContentCollection : EntityCollection<FilterNodeContentEntity>
	{
        /// <summary>
        /// Gets the count of all FilterNodeContentEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FilterNodeContentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static FilterNodeContentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FilterNodeContentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of UserSettingsEntity
	/// </summary>
	public class UserSettingsCollection : EntityCollection<UserSettingsEntity>
	{
        /// <summary>
        /// Gets the count of all UserSettingsEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UserSettingsEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static UserSettingsCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UserSettingsCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of GridColumnFormatEntity
	/// </summary>
	public class GridColumnFormatCollection : EntityCollection<GridColumnFormatEntity>
	{
        /// <summary>
        /// Gets the count of all GridColumnFormatEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GridColumnFormatEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static GridColumnFormatCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GridColumnFormatCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GridColumnLayoutEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static GridColumnLayoutCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GridColumnLayoutCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GridColumnPositionEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static GridColumnPositionCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GridColumnPositionCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ShopSiteStoreEntity
	/// </summary>
	public class ShopSiteStoreCollection : EntityCollection<ShopSiteStoreEntity>
	{
        /// <summary>
        /// Gets the count of all ShopSiteStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShopSiteStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ShopSiteStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShopSiteStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of DownloadEntity
	/// </summary>
	public class DownloadCollection : EntityCollection<DownloadEntity>
	{
        /// <summary>
        /// Gets the count of all DownloadEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all DownloadEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static DownloadCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static DownloadCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all DownloadDetailEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static DownloadDetailCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static DownloadDetailCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of OrderItemAttributeEntity
	/// </summary>
	public class OrderItemAttributeCollection : EntityCollection<OrderItemAttributeEntity>
	{
        /// <summary>
        /// Gets the count of all OrderItemAttributeEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OrderItemAttributeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static OrderItemAttributeCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OrderItemAttributeCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of OrderChargeEntity
	/// </summary>
	public class OrderChargeCollection : EntityCollection<OrderChargeEntity>
	{
        /// <summary>
        /// Gets the count of all OrderChargeEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OrderChargeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static OrderChargeCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OrderChargeCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of StatusPresetEntity
	/// </summary>
	public class StatusPresetCollection : EntityCollection<StatusPresetEntity>
	{
        /// <summary>
        /// Gets the count of all StatusPresetEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all StatusPresetEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static StatusPresetCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static StatusPresetCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of OrderPaymentDetailEntity
	/// </summary>
	public class OrderPaymentDetailCollection : EntityCollection<OrderPaymentDetailEntity>
	{
        /// <summary>
        /// Gets the count of all OrderPaymentDetailEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OrderPaymentDetailEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static OrderPaymentDetailCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OrderPaymentDetailCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ConfigurationEntity
	/// </summary>
	public class ConfigurationCollection : EntityCollection<ConfigurationEntity>
	{
        /// <summary>
        /// Gets the count of all ConfigurationEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ConfigurationEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ConfigurationCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ConfigurationCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of FilterNodeContentDetailEntity
	/// </summary>
	public class FilterNodeContentDetailCollection : EntityCollection<FilterNodeContentDetailEntity>
	{
        /// <summary>
        /// Gets the count of all FilterNodeContentDetailEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FilterNodeContentDetailEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static FilterNodeContentDetailCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FilterNodeContentDetailCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of SystemDataEntity
	/// </summary>
	public class SystemDataCollection : EntityCollection<SystemDataEntity>
	{
        /// <summary>
        /// Gets the count of all SystemDataEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all SystemDataEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static SystemDataCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static SystemDataCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all TemplateEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static TemplateCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static TemplateCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ResourceEntity
	/// </summary>
	public class ResourceCollection : EntityCollection<ResourceEntity>
	{
        /// <summary>
        /// Gets the count of all ResourceEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ResourceEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ResourceCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ResourceCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of TemplateFolderEntity
	/// </summary>
	public class TemplateFolderCollection : EntityCollection<TemplateFolderEntity>
	{
        /// <summary>
        /// Gets the count of all TemplateFolderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all TemplateFolderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static TemplateFolderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static TemplateFolderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of LabelSheetEntity
	/// </summary>
	public class LabelSheetCollection : EntityCollection<LabelSheetEntity>
	{
        /// <summary>
        /// Gets the count of all LabelSheetEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all LabelSheetEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static LabelSheetCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static LabelSheetCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of TemplateComputerSettingsEntity
	/// </summary>
	public class TemplateComputerSettingsCollection : EntityCollection<TemplateComputerSettingsEntity>
	{
        /// <summary>
        /// Gets the count of all TemplateComputerSettingsEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all TemplateComputerSettingsEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static TemplateComputerSettingsCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static TemplateComputerSettingsCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of TemplateUserSettingsEntity
	/// </summary>
	public class TemplateUserSettingsCollection : EntityCollection<TemplateUserSettingsEntity>
	{
        /// <summary>
        /// Gets the count of all TemplateUserSettingsEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all TemplateUserSettingsEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static TemplateUserSettingsCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static TemplateUserSettingsCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ShipmentEntity
	/// </summary>
	public class ShipmentCollection : EntityCollection<ShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all ShipmentEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of FilterNodeColumnSettingsEntity
	/// </summary>
	public class FilterNodeColumnSettingsCollection : EntityCollection<FilterNodeColumnSettingsEntity>
	{
        /// <summary>
        /// Gets the count of all FilterNodeColumnSettingsEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FilterNodeColumnSettingsEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static FilterNodeColumnSettingsCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FilterNodeColumnSettingsCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of TemplateStoreSettingsEntity
	/// </summary>
	public class TemplateStoreSettingsCollection : EntityCollection<TemplateStoreSettingsEntity>
	{
        /// <summary>
        /// Gets the count of all TemplateStoreSettingsEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all TemplateStoreSettingsEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static TemplateStoreSettingsCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static TemplateStoreSettingsCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of EmailAccountEntity
	/// </summary>
	public class EmailAccountCollection : EntityCollection<EmailAccountEntity>
	{
        /// <summary>
        /// Gets the count of all EmailAccountEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EmailAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static EmailAccountCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EmailAccountCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EmailOutboundEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static EmailOutboundCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EmailOutboundCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ServerMessageSignoffEntity
	/// </summary>
	public class ServerMessageSignoffCollection : EntityCollection<ServerMessageSignoffEntity>
	{
        /// <summary>
        /// Gets the count of all ServerMessageSignoffEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ServerMessageSignoffEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ServerMessageSignoffCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ServerMessageSignoffCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of VersionSignoffEntity
	/// </summary>
	public class VersionSignoffCollection : EntityCollection<VersionSignoffEntity>
	{
        /// <summary>
        /// Gets the count of all VersionSignoffEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all VersionSignoffEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static VersionSignoffCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static VersionSignoffCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ServerMessageEntity
	/// </summary>
	public class ServerMessageCollection : EntityCollection<ServerMessageEntity>
	{
        /// <summary>
        /// Gets the count of all ServerMessageEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ServerMessageEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ServerMessageCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ServerMessageCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of UserColumnSettingsEntity
	/// </summary>
	public class UserColumnSettingsCollection : EntityCollection<UserColumnSettingsEntity>
	{
        /// <summary>
        /// Gets the count of all UserColumnSettingsEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UserColumnSettingsEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static UserColumnSettingsCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UserColumnSettingsCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ActionEntity
	/// </summary>
	public class ActionCollection : EntityCollection<ActionEntity>
	{
        /// <summary>
        /// Gets the count of all ActionEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ActionEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ActionCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ActionCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ActionTaskEntity
	/// </summary>
	public class ActionTaskCollection : EntityCollection<ActionTaskEntity>
	{
        /// <summary>
        /// Gets the count of all ActionTaskEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ActionTaskEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ActionTaskCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ActionTaskCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ActionFilterTriggerEntity
	/// </summary>
	public class ActionFilterTriggerCollection : EntityCollection<ActionFilterTriggerEntity>
	{
        /// <summary>
        /// Gets the count of all ActionFilterTriggerEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ActionFilterTriggerEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ActionFilterTriggerCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ActionFilterTriggerCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ActionQueueEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ActionQueueCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ActionQueueCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ObjectReferenceEntity
	/// </summary>
	public class ObjectReferenceCollection : EntityCollection<ObjectReferenceEntity>
	{
        /// <summary>
        /// Gets the count of all ObjectReferenceEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ObjectReferenceEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ObjectReferenceCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ObjectReferenceCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of NoteEntity
	/// </summary>
	public class NoteCollection : EntityCollection<NoteEntity>
	{
        /// <summary>
        /// Gets the count of all NoteEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all NoteEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static NoteCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static NoteCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of PrintResultEntity
	/// </summary>
	public class PrintResultCollection : EntityCollection<PrintResultEntity>
	{
        /// <summary>
        /// Gets the count of all PrintResultEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all PrintResultEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static PrintResultCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static PrintResultCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of OtherShipmentEntity
	/// </summary>
	public class OtherShipmentCollection : EntityCollection<OtherShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all OtherShipmentEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OtherShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static OtherShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OtherShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of EmailOutboundRelationEntity
	/// </summary>
	public class EmailOutboundRelationCollection : EntityCollection<EmailOutboundRelationEntity>
	{
        /// <summary>
        /// Gets the count of all EmailOutboundRelationEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EmailOutboundRelationEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static EmailOutboundRelationCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EmailOutboundRelationCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of AuditChangeDetailEntity
	/// </summary>
	public class AuditChangeDetailCollection : EntityCollection<AuditChangeDetailEntity>
	{
        /// <summary>
        /// Gets the count of all AuditChangeDetailEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AuditChangeDetailEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static AuditChangeDetailCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AuditChangeDetailCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of AuditChangeEntity
	/// </summary>
	public class AuditChangeCollection : EntityCollection<AuditChangeEntity>
	{
        /// <summary>
        /// Gets the count of all AuditChangeEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AuditChangeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static AuditChangeCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AuditChangeCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ObjectLabelEntity
	/// </summary>
	public class ObjectLabelCollection : EntityCollection<ObjectLabelEntity>
	{
        /// <summary>
        /// Gets the count of all ObjectLabelEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ObjectLabelEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ObjectLabelCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ObjectLabelCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ActionQueueStepEntity
	/// </summary>
	public class ActionQueueStepCollection : EntityCollection<ActionQueueStepEntity>
	{
        /// <summary>
        /// Gets the count of all ActionQueueStepEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ActionQueueStepEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ActionQueueStepCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ActionQueueStepCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ShippingOriginEntity
	/// </summary>
	public class ShippingOriginCollection : EntityCollection<ShippingOriginEntity>
	{
        /// <summary>
        /// Gets the count of all ShippingOriginEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShippingOriginEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ShippingOriginCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShippingOriginCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ShipmentCustomsItemEntity
	/// </summary>
	public class ShipmentCustomsItemCollection : EntityCollection<ShipmentCustomsItemEntity>
	{
        /// <summary>
        /// Gets the count of all ShipmentCustomsItemEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShipmentCustomsItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ShipmentCustomsItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShipmentCustomsItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of UspsAccountEntity
	/// </summary>
	public class UspsAccountCollection : EntityCollection<UspsAccountEntity>
	{
        /// <summary>
        /// Gets the count of all UspsAccountEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UspsAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static UspsAccountCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UspsAccountCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ChannelAdvisorStoreEntity
	/// </summary>
	public class ChannelAdvisorStoreCollection : EntityCollection<ChannelAdvisorStoreEntity>
	{
        /// <summary>
        /// Gets the count of all ChannelAdvisorStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ChannelAdvisorStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ChannelAdvisorStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ChannelAdvisorStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ChannelAdvisorOrderItemEntity
	/// </summary>
	public class ChannelAdvisorOrderItemCollection : EntityCollection<ChannelAdvisorOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all ChannelAdvisorOrderItemEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ChannelAdvisorOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ChannelAdvisorOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ChannelAdvisorOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ChannelAdvisorOrderEntity
	/// </summary>
	public class ChannelAdvisorOrderCollection : EntityCollection<ChannelAdvisorOrderEntity>
	{
        /// <summary>
        /// Gets the count of all ChannelAdvisorOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ChannelAdvisorOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ChannelAdvisorOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ChannelAdvisorOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of DimensionsProfileEntity
	/// </summary>
	public class DimensionsProfileCollection : EntityCollection<DimensionsProfileEntity>
	{
        /// <summary>
        /// Gets the count of all DimensionsProfileEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all DimensionsProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static DimensionsProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static DimensionsProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of UspsShipmentEntity
	/// </summary>
	public class UspsShipmentCollection : EntityCollection<UspsShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all UspsShipmentEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UspsShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static UspsShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UspsShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of PostalShipmentEntity
	/// </summary>
	public class PostalShipmentCollection : EntityCollection<PostalShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all PostalShipmentEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all PostalShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static PostalShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static PostalShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ShippingProfileEntity
	/// </summary>
	public class ShippingProfileCollection : EntityCollection<ShippingProfileEntity>
	{
        /// <summary>
        /// Gets the count of all ShippingProfileEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShippingProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ShippingProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShippingProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of FedExAccountEntity
	/// </summary>
	public class FedExAccountCollection : EntityCollection<FedExAccountEntity>
	{
        /// <summary>
        /// Gets the count of all FedExAccountEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FedExAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static FedExAccountCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FedExAccountCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of UpsAccountEntity
	/// </summary>
	public class UpsAccountCollection : EntityCollection<UpsAccountEntity>
	{
        /// <summary>
        /// Gets the count of all UpsAccountEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static UpsAccountCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsAccountCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of PostalProfileEntity
	/// </summary>
	public class PostalProfileCollection : EntityCollection<PostalProfileEntity>
	{
        /// <summary>
        /// Gets the count of all PostalProfileEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all PostalProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static PostalProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static PostalProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of UspsProfileEntity
	/// </summary>
	public class UspsProfileCollection : EntityCollection<UspsProfileEntity>
	{
        /// <summary>
        /// Gets the count of all UspsProfileEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UspsProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static UspsProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UspsProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of OtherProfileEntity
	/// </summary>
	public class OtherProfileCollection : EntityCollection<OtherProfileEntity>
	{
        /// <summary>
        /// Gets the count of all OtherProfileEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OtherProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static OtherProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OtherProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ShippingPrintOutputRuleEntity
	/// </summary>
	public class ShippingPrintOutputRuleCollection : EntityCollection<ShippingPrintOutputRuleEntity>
	{
        /// <summary>
        /// Gets the count of all ShippingPrintOutputRuleEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShippingPrintOutputRuleEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ShippingPrintOutputRuleCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShippingPrintOutputRuleCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ShippingPrintOutputEntity
	/// </summary>
	public class ShippingPrintOutputCollection : EntityCollection<ShippingPrintOutputEntity>
	{
        /// <summary>
        /// Gets the count of all ShippingPrintOutputEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShippingPrintOutputEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ShippingPrintOutputCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShippingPrintOutputCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ShippingDefaultsRuleEntity
	/// </summary>
	public class ShippingDefaultsRuleCollection : EntityCollection<ShippingDefaultsRuleEntity>
	{
        /// <summary>
        /// Gets the count of all ShippingDefaultsRuleEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShippingDefaultsRuleEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ShippingDefaultsRuleCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShippingDefaultsRuleCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ShippingProviderRuleEntity
	/// </summary>
	public class ShippingProviderRuleCollection : EntityCollection<ShippingProviderRuleEntity>
	{
        /// <summary>
        /// Gets the count of all ShippingProviderRuleEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShippingProviderRuleEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ShippingProviderRuleCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShippingProviderRuleCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of InfopiaStoreEntity
	/// </summary>
	public class InfopiaStoreCollection : EntityCollection<InfopiaStoreEntity>
	{
        /// <summary>
        /// Gets the count of all InfopiaStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all InfopiaStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static InfopiaStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static InfopiaStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of InfopiaOrderItemEntity
	/// </summary>
	public class InfopiaOrderItemCollection : EntityCollection<InfopiaOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all InfopiaOrderItemEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all InfopiaOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static InfopiaOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static InfopiaOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of FedExShipmentEntity
	/// </summary>
	public class FedExShipmentCollection : EntityCollection<FedExShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all FedExShipmentEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FedExShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static FedExShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FedExShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of FedExProfileEntity
	/// </summary>
	public class FedExProfileCollection : EntityCollection<FedExProfileEntity>
	{
        /// <summary>
        /// Gets the count of all FedExProfileEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FedExProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static FedExProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FedExProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of FedExPackageEntity
	/// </summary>
	public class FedExPackageCollection : EntityCollection<FedExPackageEntity>
	{
        /// <summary>
        /// Gets the count of all FedExPackageEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FedExPackageEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static FedExPackageCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FedExPackageCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of PayPalStoreEntity
	/// </summary>
	public class PayPalStoreCollection : EntityCollection<PayPalStoreEntity>
	{
        /// <summary>
        /// Gets the count of all PayPalStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all PayPalStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static PayPalStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static PayPalStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of PayPalOrderEntity
	/// </summary>
	public class PayPalOrderCollection : EntityCollection<PayPalOrderEntity>
	{
        /// <summary>
        /// Gets the count of all PayPalOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all PayPalOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static PayPalOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static PayPalOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of FedExProfilePackageEntity
	/// </summary>
	public class FedExProfilePackageCollection : EntityCollection<FedExProfilePackageEntity>
	{
        /// <summary>
        /// Gets the count of all FedExProfilePackageEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FedExProfilePackageEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static FedExProfilePackageCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FedExProfilePackageCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of AmazonStoreEntity
	/// </summary>
	public class AmazonStoreCollection : EntityCollection<AmazonStoreEntity>
	{
        /// <summary>
        /// Gets the count of all AmazonStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AmazonStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static AmazonStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AmazonStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of AmazonOrderEntity
	/// </summary>
	public class AmazonOrderCollection : EntityCollection<AmazonOrderEntity>
	{
        /// <summary>
        /// Gets the count of all AmazonOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AmazonOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static AmazonOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AmazonOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of AmazonASINEntity
	/// </summary>
	public class AmazonASINCollection : EntityCollection<AmazonASINEntity>
	{
        /// <summary>
        /// Gets the count of all AmazonASINEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AmazonASINEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static AmazonASINCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AmazonASINCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of UpsPackageEntity
	/// </summary>
	public class UpsPackageCollection : EntityCollection<UpsPackageEntity>
	{
        /// <summary>
        /// Gets the count of all UpsPackageEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsPackageEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static UpsPackageCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsPackageCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of UpsProfileEntity
	/// </summary>
	public class UpsProfileCollection : EntityCollection<UpsProfileEntity>
	{
        /// <summary>
        /// Gets the count of all UpsProfileEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static UpsProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsProfilePackageEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static UpsProfilePackageCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsProfilePackageCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of UpsShipmentEntity
	/// </summary>
	public class UpsShipmentCollection : EntityCollection<UpsShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all UpsShipmentEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UpsShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static UpsShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UpsShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of AmazonOrderItemEntity
	/// </summary>
	public class AmazonOrderItemCollection : EntityCollection<AmazonOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all AmazonOrderItemEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AmazonOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static AmazonOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AmazonOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of FedExEndOfDayCloseEntity
	/// </summary>
	public class FedExEndOfDayCloseCollection : EntityCollection<FedExEndOfDayCloseEntity>
	{
        /// <summary>
        /// Gets the count of all FedExEndOfDayCloseEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FedExEndOfDayCloseEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static FedExEndOfDayCloseCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FedExEndOfDayCloseCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of EndiciaAccountEntity
	/// </summary>
	public class EndiciaAccountCollection : EntityCollection<EndiciaAccountEntity>
	{
        /// <summary>
        /// Gets the count of all EndiciaAccountEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EndiciaAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static EndiciaAccountCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EndiciaAccountCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EndiciaProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static EndiciaProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EndiciaProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of EndiciaShipmentEntity
	/// </summary>
	public class EndiciaShipmentCollection : EntityCollection<EndiciaShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all EndiciaShipmentEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EndiciaShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static EndiciaShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EndiciaShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of EndiciaScanFormEntity
	/// </summary>
	public class EndiciaScanFormCollection : EntityCollection<EndiciaScanFormEntity>
	{
        /// <summary>
        /// Gets the count of all EndiciaScanFormEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EndiciaScanFormEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static EndiciaScanFormCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EndiciaScanFormCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of WorldShipPackageEntity
	/// </summary>
	public class WorldShipPackageCollection : EntityCollection<WorldShipPackageEntity>
	{
        /// <summary>
        /// Gets the count of all WorldShipPackageEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all WorldShipPackageEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static WorldShipPackageCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static WorldShipPackageCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all WorldShipProcessedEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static WorldShipProcessedCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static WorldShipProcessedCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all WorldShipShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static WorldShipShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static WorldShipShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of WorldShipGoodsEntity
	/// </summary>
	public class WorldShipGoodsCollection : EntityCollection<WorldShipGoodsEntity>
	{
        /// <summary>
        /// Gets the count of all WorldShipGoodsEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all WorldShipGoodsEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static WorldShipGoodsCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static WorldShipGoodsCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of EbayStoreEntity
	/// </summary>
	public class EbayStoreCollection : EntityCollection<EbayStoreEntity>
	{
        /// <summary>
        /// Gets the count of all EbayStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EbayStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static EbayStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EbayStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of EbayOrderEntity
	/// </summary>
	public class EbayOrderCollection : EntityCollection<EbayOrderEntity>
	{
        /// <summary>
        /// Gets the count of all EbayOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EbayOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static EbayOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EbayOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EbayOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static EbayOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EbayOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of MivaStoreEntity
	/// </summary>
	public class MivaStoreCollection : EntityCollection<MivaStoreEntity>
	{
        /// <summary>
        /// Gets the count of all MivaStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all MivaStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static MivaStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static MivaStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of MarketplaceAdvisorOrderEntity
	/// </summary>
	public class MarketplaceAdvisorOrderCollection : EntityCollection<MarketplaceAdvisorOrderEntity>
	{
        /// <summary>
        /// Gets the count of all MarketplaceAdvisorOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all MarketplaceAdvisorOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static MarketplaceAdvisorOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static MarketplaceAdvisorOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of MarketplaceAdvisorStoreEntity
	/// </summary>
	public class MarketplaceAdvisorStoreCollection : EntityCollection<MarketplaceAdvisorStoreEntity>
	{
        /// <summary>
        /// Gets the count of all MarketplaceAdvisorStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all MarketplaceAdvisorStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static MarketplaceAdvisorStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static MarketplaceAdvisorStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of YahooOrderEntity
	/// </summary>
	public class YahooOrderCollection : EntityCollection<YahooOrderEntity>
	{
        /// <summary>
        /// Gets the count of all YahooOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all YahooOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static YahooOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static YahooOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all YahooOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static YahooOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static YahooOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of YahooStoreEntity
	/// </summary>
	public class YahooStoreCollection : EntityCollection<YahooStoreEntity>
	{
        /// <summary>
        /// Gets the count of all YahooStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all YahooStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static YahooStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static YahooStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	
	
	/// <summary>
	/// Strongly typed collection of YahooProductEntity
	/// </summary>
	public class YahooProductCollection : EntityCollection<YahooProductEntity>
	{
        /// <summary>
        /// Gets the count of all YahooProductEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all YahooProductEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static YahooProductCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static YahooProductCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of MagentoOrderEntity
	/// </summary>
	public class MagentoOrderCollection : EntityCollection<MagentoOrderEntity>
	{
        /// <summary>
        /// Gets the count of all MagentoOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all MagentoOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static MagentoOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static MagentoOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of MagentoStoreEntity
	/// </summary>
	public class MagentoStoreCollection : EntityCollection<MagentoStoreEntity>
	{
        /// <summary>
        /// Gets the count of all MagentoStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all MagentoStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static MagentoStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static MagentoStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ProStoresStoreEntity
	/// </summary>
	public class ProStoresStoreCollection : EntityCollection<ProStoresStoreEntity>
	{
        /// <summary>
        /// Gets the count of all ProStoresStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ProStoresStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ProStoresStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ProStoresStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ProStoresOrderEntity
	/// </summary>
	public class ProStoresOrderCollection : EntityCollection<ProStoresOrderEntity>
	{
        /// <summary>
        /// Gets the count of all ProStoresOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ProStoresOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ProStoresOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ProStoresOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of AmeriCommerceStoreEntity
	/// </summary>
	public class AmeriCommerceStoreCollection : EntityCollection<AmeriCommerceStoreEntity>
	{
        /// <summary>
        /// Gets the count of all AmeriCommerceStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AmeriCommerceStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static AmeriCommerceStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AmeriCommerceStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of NetworkSolutionsStoreEntity
	/// </summary>
	public class NetworkSolutionsStoreCollection : EntityCollection<NetworkSolutionsStoreEntity>
	{
        /// <summary>
        /// Gets the count of all NetworkSolutionsStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all NetworkSolutionsStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static NetworkSolutionsStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static NetworkSolutionsStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of NetworkSolutionsOrderEntity
	/// </summary>
	public class NetworkSolutionsOrderCollection : EntityCollection<NetworkSolutionsOrderEntity>
	{
        /// <summary>
        /// Gets the count of all NetworkSolutionsOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all NetworkSolutionsOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static NetworkSolutionsOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static NetworkSolutionsOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of VolusionStoreEntity
	/// </summary>
	public class VolusionStoreCollection : EntityCollection<VolusionStoreEntity>
	{
        /// <summary>
        /// Gets the count of all VolusionStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all VolusionStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static VolusionStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static VolusionStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of OrderMotionStoreEntity
	/// </summary>
	public class OrderMotionStoreCollection : EntityCollection<OrderMotionStoreEntity>
	{
        /// <summary>
        /// Gets the count of all OrderMotionStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OrderMotionStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static OrderMotionStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OrderMotionStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of OrderMotionOrderEntity
	/// </summary>
	public class OrderMotionOrderCollection : EntityCollection<OrderMotionOrderEntity>
	{
        /// <summary>
        /// Gets the count of all OrderMotionOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OrderMotionOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static OrderMotionOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OrderMotionOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ClickCartProOrderEntity
	/// </summary>
	public class ClickCartProOrderCollection : EntityCollection<ClickCartProOrderEntity>
	{
        /// <summary>
        /// Gets the count of all ClickCartProOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ClickCartProOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ClickCartProOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ClickCartProOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of CommerceInterfaceOrderEntity
	/// </summary>
	public class CommerceInterfaceOrderCollection : EntityCollection<CommerceInterfaceOrderEntity>
	{
        /// <summary>
        /// Gets the count of all CommerceInterfaceOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all CommerceInterfaceOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static CommerceInterfaceOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static CommerceInterfaceOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of SearchEntity
	/// </summary>
	public class SearchCollection : EntityCollection<SearchEntity>
	{
        /// <summary>
        /// Gets the count of all SearchEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all SearchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static SearchCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static SearchCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of GenericFileStoreEntity
	/// </summary>
	public class GenericFileStoreCollection : EntityCollection<GenericFileStoreEntity>
	{
        /// <summary>
        /// Gets the count of all GenericFileStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GenericFileStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static GenericFileStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GenericFileStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of GenericModuleStoreEntity
	/// </summary>
	public class GenericModuleStoreCollection : EntityCollection<GenericModuleStoreEntity>
	{
        /// <summary>
        /// Gets the count of all GenericModuleStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GenericModuleStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static GenericModuleStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GenericModuleStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of FtpAccountEntity
	/// </summary>
	public class FtpAccountCollection : EntityCollection<FtpAccountEntity>
	{
        /// <summary>
        /// Gets the count of all FtpAccountEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all FtpAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static FtpAccountCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static FtpAccountCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of MivaOrderItemAttributeEntity
	/// </summary>
	public class MivaOrderItemAttributeCollection : EntityCollection<MivaOrderItemAttributeEntity>
	{
        /// <summary>
        /// Gets the count of all MivaOrderItemAttributeEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all MivaOrderItemAttributeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static MivaOrderItemAttributeCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static MivaOrderItemAttributeCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of NeweggStoreEntity
	/// </summary>
	public class NeweggStoreCollection : EntityCollection<NeweggStoreEntity>
	{
        /// <summary>
        /// Gets the count of all NeweggStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all NeweggStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static NeweggStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static NeweggStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of NeweggOrderItemEntity
	/// </summary>
	public class NeweggOrderItemCollection : EntityCollection<NeweggOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all NeweggOrderItemEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all NeweggOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static NeweggOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static NeweggOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of NeweggOrderEntity
	/// </summary>
	public class NeweggOrderCollection : EntityCollection<NeweggOrderEntity>
	{
        /// <summary>
        /// Gets the count of all NeweggOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all NeweggOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static NeweggOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static NeweggOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ShopifyStoreEntity
	/// </summary>
	public class ShopifyStoreCollection : EntityCollection<ShopifyStoreEntity>
	{
        /// <summary>
        /// Gets the count of all ShopifyStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShopifyStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ShopifyStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShopifyStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of EtsyOrderEntity
	/// </summary>
	public class EtsyOrderCollection : EntityCollection<EtsyOrderEntity>
	{
        /// <summary>
        /// Gets the count of all EtsyOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EtsyOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static EtsyOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EtsyOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ShopifyOrderEntity
	/// </summary>
	public class ShopifyOrderCollection : EntityCollection<ShopifyOrderEntity>
	{
        /// <summary>
        /// Gets the count of all ShopifyOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShopifyOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ShopifyOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShopifyOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShopifyOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ShopifyOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShopifyOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of EtsyStoreEntity
	/// </summary>
	public class EtsyStoreCollection : EntityCollection<EtsyStoreEntity>
	{
        /// <summary>
        /// Gets the count of all EtsyStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EtsyStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static EtsyStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EtsyStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of BuyDotComStoreEntity
	/// </summary>
	public class BuyDotComStoreCollection : EntityCollection<BuyDotComStoreEntity>
	{
        /// <summary>
        /// Gets the count of all BuyDotComStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all BuyDotComStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static BuyDotComStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static BuyDotComStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of BuyDotComOrderItemEntity
	/// </summary>
	public class BuyDotComOrderItemCollection : EntityCollection<BuyDotComOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all BuyDotComOrderItemEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all BuyDotComOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static BuyDotComOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static BuyDotComOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ThreeDCartStoreEntity
	/// </summary>
	public class ThreeDCartStoreCollection : EntityCollection<ThreeDCartStoreEntity>
	{
        /// <summary>
        /// Gets the count of all ThreeDCartStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ThreeDCartStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ThreeDCartStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ThreeDCartStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ThreeDCartOrderItemEntity
	/// </summary>
	public class ThreeDCartOrderItemCollection : EntityCollection<ThreeDCartOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all ThreeDCartOrderItemEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ThreeDCartOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ThreeDCartOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ThreeDCartOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of SearsOrderEntity
	/// </summary>
	public class SearsOrderCollection : EntityCollection<SearsOrderEntity>
	{
        /// <summary>
        /// Gets the count of all SearsOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all SearsOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static SearsOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static SearsOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of SearsStoreEntity
	/// </summary>
	public class SearsStoreCollection : EntityCollection<SearsStoreEntity>
	{
        /// <summary>
        /// Gets the count of all SearsStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all SearsStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static SearsStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static SearsStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of SearsOrderItemEntity
	/// </summary>
	public class SearsOrderItemCollection : EntityCollection<SearsOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all SearsOrderItemEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all SearsOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static SearsOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static SearsOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of OnTracAccountEntity
	/// </summary>
	public class OnTracAccountCollection : EntityCollection<OnTracAccountEntity>
	{
        /// <summary>
        /// Gets the count of all OnTracAccountEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OnTracAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static OnTracAccountCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OnTracAccountCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of BigCommerceStoreEntity
	/// </summary>
	public class BigCommerceStoreCollection : EntityCollection<BigCommerceStoreEntity>
	{
        /// <summary>
        /// Gets the count of all BigCommerceStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all BigCommerceStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static BigCommerceStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static BigCommerceStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of OnTracProfileEntity
	/// </summary>
	public class OnTracProfileCollection : EntityCollection<OnTracProfileEntity>
	{
        /// <summary>
        /// Gets the count of all OnTracProfileEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OnTracProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static OnTracProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OnTracProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of BigCommerceOrderItemEntity
	/// </summary>
	public class BigCommerceOrderItemCollection : EntityCollection<BigCommerceOrderItemEntity>
	{
        /// <summary>
        /// Gets the count of all BigCommerceOrderItemEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all BigCommerceOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static BigCommerceOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static BigCommerceOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of OnTracShipmentEntity
	/// </summary>
	public class OnTracShipmentCollection : EntityCollection<OnTracShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all OnTracShipmentEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OnTracShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static OnTracShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OnTracShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of UspsScanFormEntity
	/// </summary>
	public class UspsScanFormCollection : EntityCollection<UspsScanFormEntity>
	{
        /// <summary>
        /// Gets the count of all UspsScanFormEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all UspsScanFormEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static UspsScanFormCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static UspsScanFormCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of IParcelShipmentEntity
	/// </summary>
	public class IParcelShipmentCollection : EntityCollection<IParcelShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all IParcelShipmentEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all IParcelShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static IParcelShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static IParcelShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of IParcelProfileEntity
	/// </summary>
	public class IParcelProfileCollection : EntityCollection<IParcelProfileEntity>
	{
        /// <summary>
        /// Gets the count of all IParcelProfileEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all IParcelProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static IParcelProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static IParcelProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of IParcelAccountEntity
	/// </summary>
	public class IParcelAccountCollection : EntityCollection<IParcelAccountEntity>
	{
        /// <summary>
        /// Gets the count of all IParcelAccountEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all IParcelAccountEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static IParcelAccountCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static IParcelAccountCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of IParcelProfilePackageEntity
	/// </summary>
	public class IParcelProfilePackageCollection : EntityCollection<IParcelProfilePackageEntity>
	{
        /// <summary>
        /// Gets the count of all IParcelProfilePackageEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all IParcelProfilePackageEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
        {
            RelationPredicateBucket bucket = null;
            
            if (filter != null)
            {
                bucket = new RelationPredicateBucket(filter);
            }

            return adapter.GetDbCount(new IParcelProfilePackageEntityFactory().CreateFields(), bucket);
        }
		
        /// <summary>
        /// Fetch a new collection object that matches the specified filter.
        /// </summary>
        public static IParcelProfilePackageCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static IParcelProfilePackageCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
        {
            IParcelProfilePackageCollection collection = new IParcelProfilePackageCollection();

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
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all IParcelPackageEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static IParcelPackageCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static IParcelPackageCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ScanFormBatchEntity
	/// </summary>
	public class ScanFormBatchCollection : EntityCollection<ScanFormBatchEntity>
	{
        /// <summary>
        /// Gets the count of all ScanFormBatchEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ScanFormBatchEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ScanFormBatchCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ScanFormBatchCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ServiceStatusEntity
	/// </summary>
	public class ServiceStatusCollection : EntityCollection<ServiceStatusEntity>
	{
        /// <summary>
        /// Gets the count of all ServiceStatusEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ServiceStatusEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ServiceStatusCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ServiceStatusCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ActionQueueSelectionEntity
	/// </summary>
	public class ActionQueueSelectionCollection : EntityCollection<ActionQueueSelectionEntity>
	{
        /// <summary>
        /// Gets the count of all ActionQueueSelectionEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ActionQueueSelectionEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ActionQueueSelectionCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ActionQueueSelectionCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ShippingSettingsEntity
	/// </summary>
	public class ShippingSettingsCollection : EntityCollection<ShippingSettingsEntity>
	{
        /// <summary>
        /// Gets the count of all ShippingSettingsEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShippingSettingsEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ShippingSettingsCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShippingSettingsCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of BestRateShipmentEntity
	/// </summary>
	public class BestRateShipmentCollection : EntityCollection<BestRateShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all BestRateShipmentEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all BestRateShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static BestRateShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static BestRateShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of BestRateProfileEntity
	/// </summary>
	public class BestRateProfileCollection : EntityCollection<BestRateProfileEntity>
	{
        /// <summary>
        /// Gets the count of all BestRateProfileEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all BestRateProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static BestRateProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static BestRateProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ShipSenseKnowledgebaseEntity
	/// </summary>
	public class ShipSenseKnowledgebaseCollection : EntityCollection<ShipSenseKnowledgebaseEntity>
	{
        /// <summary>
        /// Gets the count of all ShipSenseKnowledgebaseEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ShipSenseKnowledgebaseEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ShipSenseKnowledgebaseCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ShipSenseKnowledgebaseCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of InsurancePolicyEntity
	/// </summary>
	public class InsurancePolicyCollection : EntityCollection<InsurancePolicyEntity>
	{
        /// <summary>
        /// Gets the count of all InsurancePolicyEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all InsurancePolicyEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static InsurancePolicyCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static InsurancePolicyCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of EbayCombinedOrderRelationEntity
	/// </summary>
	public class EbayCombinedOrderRelationCollection : EntityCollection<EbayCombinedOrderRelationEntity>
	{
        /// <summary>
        /// Gets the count of all EbayCombinedOrderRelationEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all EbayCombinedOrderRelationEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static EbayCombinedOrderRelationCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static EbayCombinedOrderRelationCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ValidatedAddressEntity
	/// </summary>
	public class ValidatedAddressCollection : EntityCollection<ValidatedAddressEntity>
	{
        /// <summary>
        /// Gets the count of all ValidatedAddressEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ValidatedAddressEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ValidatedAddressCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ValidatedAddressCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of GrouponOrderEntity
	/// </summary>
	public class GrouponOrderCollection : EntityCollection<GrouponOrderEntity>
	{
        /// <summary>
        /// Gets the count of all GrouponOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GrouponOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static GrouponOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GrouponOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GrouponOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static GrouponOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GrouponOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of GrouponStoreEntity
	/// </summary>
	public class GrouponStoreCollection : EntityCollection<GrouponStoreEntity>
	{
        /// <summary>
        /// Gets the count of all GrouponStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all GrouponStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static GrouponStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static GrouponStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ExcludedServiceTypeEntity
	/// </summary>
	public class ExcludedServiceTypeCollection : EntityCollection<ExcludedServiceTypeEntity>
	{
        /// <summary>
        /// Gets the count of all ExcludedServiceTypeEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ExcludedServiceTypeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ExcludedServiceTypeCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ExcludedServiceTypeCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of ExcludedPackageTypeEntity
	/// </summary>
	public class ExcludedPackageTypeCollection : EntityCollection<ExcludedPackageTypeEntity>
	{
        /// <summary>
        /// Gets the count of all ExcludedPackageTypeEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all ExcludedPackageTypeEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static ExcludedPackageTypeCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static ExcludedPackageTypeCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of LemonStandStoreEntity
	/// </summary>
	public class LemonStandStoreCollection : EntityCollection<LemonStandStoreEntity>
	{
        /// <summary>
        /// Gets the count of all LemonStandStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all LemonStandStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static LemonStandStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static LemonStandStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of LemonStandOrderEntity
	/// </summary>
	public class LemonStandOrderCollection : EntityCollection<LemonStandOrderEntity>
	{
        /// <summary>
        /// Gets the count of all LemonStandOrderEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all LemonStandOrderEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static LemonStandOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static LemonStandOrderCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all LemonStandOrderItemEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static LemonStandOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static LemonStandOrderItemCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of AmazonShipmentEntity
	/// </summary>
	public class AmazonShipmentCollection : EntityCollection<AmazonShipmentEntity>
	{
        /// <summary>
        /// Gets the count of all AmazonShipmentEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AmazonShipmentEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static AmazonShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AmazonShipmentCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of AmazonProfileEntity
	/// </summary>
	public class AmazonProfileCollection : EntityCollection<AmazonProfileEntity>
	{
        /// <summary>
        /// Gets the count of all AmazonProfileEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all AmazonProfileEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static AmazonProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static AmazonProfileCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	/// Strongly typed collection of OdbcStoreEntity
	/// </summary>
	public class OdbcStoreCollection : EntityCollection<OdbcStoreEntity>
	{
        /// <summary>
        /// Gets the count of all OdbcStoreEntity rows
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter)
        {
            return GetCount(adapter, null);
        }

        /// <summary>
        /// Gets the count of all OdbcStoreEntity rows filtered by the given predicate
        /// </summary>
        public static int GetCount(DataAccessAdapterBase adapter, IPredicate filter)
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
        public static OdbcStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter)
        {
			return Fetch(adapter, filter, null);
        }
        
		/// <summary>
        /// Fetch a new collection object that matches the specified filter and uses the given prefetch.
        /// </summary>
        public static OdbcStoreCollection Fetch(DataAccessAdapterBase adapter, IPredicate filter, IPrefetchPath2 prefetchPath)
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
	
	
}

