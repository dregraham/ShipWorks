using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Stores;
using System.Collections.ObjectModel;
using ShipWorks.Data.Model;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Provides the data to the template processing engine
    /// </summary>
    public class TemplateInput
    {
        List<long> originalKeys;

        List<long> contextKeys;
        TemplateInputContext contextType;

        List<long> storeKeys;
        List<long> customerKeys;
        List<long> orderKeys;

        /// <summary>
        /// The Context is the type contained in the keys.  Each key must be of the same entity type.
        /// </summary>
        public TemplateInput(List<long> originalKeys, List<long> contextKeys, TemplateInputContext contextType)
        {
            if (originalKeys == null)
            {
                throw new ArgumentNullException("originalKeys");
            }

            if (contextKeys == null)
            {
                throw new ArgumentNullException("contextKeys");
            }

            if (contextType == TemplateInputContext.Auto)
            {
                throw new ArgumentException("contextType must have already been resolved from Auto", "contextType");
            }

            this.originalKeys = originalKeys.Distinct().ToList();
            this.contextKeys = contextKeys.Distinct().ToList();

            // Determine the context entity type
            this.contextType = contextType;
        }

        /// <summary>
        /// The original keys the user selected, which were used to generate the context keys
        /// </summary>
        public IList<long> OriginalKeys
        {
            get { return new ReadOnlyCollection<long>(originalKeys); }
        }

        /// <summary>
        /// The context keys sent to be used as input.  These were translated based on template context from the original keys collection, which 
        /// is the collectino of keys the user initially selected.
        /// </summary>
        public IList<long> ContextKeys
        {
            get { return new ReadOnlyCollection<long>(contextKeys); }
        }

        /// <summary>
        /// The context type of each of the context keys
        /// </summary>
        public TemplateInputContext ContextType
        {
            get { return contextType; }
        }

        /// <summary>
        /// Get all the stores of the items in the context
        /// </summary>
        public IEnumerable<StoreEntity> GetStores()
        {
            EnsureStoreKeys();

            foreach (long storeID in storeKeys)
            {
                yield return StoreManager.GetStore(storeID);
            }
        }

        /// <summary>
        /// Get the primary keys of all the customers in context
        /// </summary>
        public IEnumerable<long> GetCustomerKeys()
        {
            EnsureContextCustomerKeys();

            foreach (long key in customerKeys)
            {
                yield return key;
            }
        }

        /// <summary>
        /// Get all the order keys in the context that are related to the specified customer
        /// </summary>
        public IEnumerable<long> GetOrderKeys(long customerID)
        {
            EnsureContextCustomerKeys();

            // Optimization: If the context is orders, and there is only one customer (meaning all the orders in the context
            // are for the same customer), then we just have to return the context keys, which will be the same as the order keys.
            if (contextType == TemplateInputContext.Order && customerKeys.Count == 1)
            {
                return contextKeys;
            }

            // Get all of the customer's orders
            List<long> customersOrderKeys = DataProvider.GetRelatedKeys(customerID, EntityType.OrderEntity);

            // If the context is customer, we return all of the customer's orders
            if (contextType == TemplateInputContext.Customer)
            {
                return customersOrderKeys;
            }

            // Otherwise filter the customers order keys by the order keys in our context
            else
            {
                EnsureContextOrderKeys();

                return FilterKeys(customersOrderKeys, orderKeys);
            }
        }

        /// <summary>
        /// Get all the order item keys in context that are related to the specific order
        /// </summary>
        public IEnumerable<long> GetOrderItemKeys(long orderID)
        {
            // Optimization: If the context is items, and there is only one order (meaning all the items in the context
            // are for the same order), then we just have to return the context keys, which will be the same as the item keys.
            if (contextType == TemplateInputContext.OrderItem)
            {
                EnsureContextOrderKeys();

                if (orderKeys.Count == 1)
                {
                    return contextKeys;
                }
            }

            // Get all of the order's items
            List<long> ordersItemKeys = DataProvider.GetRelatedKeys(orderID, EntityType.OrderItemEntity);

            // if the context is item filter by the items that are in context
            if (contextType == TemplateInputContext.OrderItem)
            {
                return FilterKeys(ordersItemKeys, contextKeys);
            }
            else
            {
                return ordersItemKeys;
            }
        }

        /// <summary>
        /// Get all the shipment keys in the context that are related to the specified order
        /// </summary>
        public IEnumerable<long> GetShipmentKeys(long orderID)
        {
            // Optimization: If the context is shipments, and there is only one order (meaning all the shipments in the context
            // are for the same order), then we just have to return the context keys, which will be the same as the shipment keys.
            if (contextType == TemplateInputContext.Shipment)
            {
                EnsureContextOrderKeys();

                if (orderKeys.Count == 1)
                {
                    return contextKeys;
                }
            }

            // Get all of the order's shipments
            List<long> ordersShipmentKeys = DataProvider.GetRelatedKeys(orderID, EntityType.ShipmentEntity);

            // if the context is shipment filter by the shipments that are in context
            if (contextType == TemplateInputContext.Shipment)
            {
                return FilterKeys(ordersShipmentKeys, contextKeys);
            }
            else
            {
                return ordersShipmentKeys;
            }
        }

        /// <summary>
        /// Ensure all the store keys have been determined.
        /// </summary>
        private void EnsureStoreKeys()
        {
            if (storeKeys != null)
            {
                return;
            }

            // Translate the given keys into store keys
            storeKeys = DataProvider.GetRelatedKeys(contextKeys.Count > 0 ? contextKeys : originalKeys, EntityType.StoreEntity);
        }

        /// <summary>
        /// Ensure the customer keys based on the context have been loaded
        /// </summary>
        private void EnsureContextCustomerKeys()
        {
            if (customerKeys == null)
            {
                if (contextType == TemplateInputContext.Customer)
                {
                    customerKeys = contextKeys;
                }
                else
                {
                    customerKeys = DataProvider.GetRelatedKeys(contextKeys, EntityType.CustomerEntity);
                }
            }
        }

        /// <summary>
        /// Ensure the list of all order keys in context.  This list is only required when the context is down-stream
        /// of OrderEntity - such as Shipment and Package.
        /// </summary>
        private void EnsureContextOrderKeys()
        {
            if (contextType == TemplateInputContext.Customer)
            {
                throw new InvalidCastException("Unnecessary call to EnsureContextOrderKeys");
            }

            if (orderKeys == null)
            {
                if (contextType == TemplateInputContext.Order)
                {
                    orderKeys = contextKeys;
                }
                else
                {
                    orderKeys = DataProvider.GetRelatedKeys(contextKeys, EntityType.OrderEntity);
                }
            }
        }

        /// <summary>
        /// Return a list of keys from the keys collection that are also in the filter
        /// </summary>
        private IEnumerable<long> FilterKeys(List<long> keys, List<long> filter)
        {
            foreach (long key in keys)
            {
                if (filter.Contains(key))
                {
                    yield return key;
                }
            }
        }
    }
}
