using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Processing;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using System.Diagnostics;
using Interapptive.Shared;
using ShipWorks.Stores;

namespace ShipWorks.Templates.Emailing
{
    /// <summary>
    /// Used to determine what settings to use based on a template and input set
    /// </summary>
    public class EmailSettingsResolver
    {

        bool useMostRecentOrderForAll = false;

        /// <summary>
        /// Raised when the user must specify which set of email settings to use.  Must be handled.
        /// </summary>
        public event EmailSettingsResolveEventHandler ResolveSettings;

        // User associated state
        object tag;

        /// <summary>
        /// Determine what settings to use for the given template and result set.  If more than one store is a possiblity,
        /// the ResolveSettings event is raised.
        /// </summary>
        [NDependIgnoreLongMethod]
        public long? DetermineStore(TemplateEntity template, IList<TemplateResult> results)
        {
            // Make sure the event is handled
            if (ResolveSettings == null)
            {
                throw new InvalidOperationException("The ResolveSettings event must be handled.");
            }

            List<long> stores = new List<long>();

            // Determine all the store's there might be
            foreach (TemplateResult result in results)
            {
                foreach (StoreEntity store in result.XPathSource.Input.GetStores())
                {
                    if (!stores.Contains(store.StoreID))
                    {
                        stores.Add(store.StoreID);
                    }
                }
            }
            
            // If there is just one store, then we're done
            if (stores.Count == 1)
            {
                return stores[0];
            }

            CustomerEntity customer = null;

            // There is more than one store.  The only way this should happen is if a customer was selected or for a label or report.
            if (template.Type == (int) TemplateType.Standard)
            {
                // Has to be a customer
                if (EntityUtility.GetEntityType(results[0].XPathSource.Input.ContextKeys[0]) !=  EntityType.CustomerEntity)
                {
                    throw new InvalidOperationException("Somehow ended up with multiple stores for a standard template where selection was not a customer.");
                }

                Debug.Assert(results.Count == 1);
                Debug.Assert(results[0].XPathSource.Input.ContextKeys.Count == 1);

                customer = (CustomerEntity) DataProvider.GetEntity(results[0].XPathSource.Input.ContextKeys[0]);
            }

            bool allUseDefault = true;

            // Now determine if the store's use different settings.  If the all end up using the same settings, it doesn't really matter.
            foreach (long storeID in stores)
            {
                TemplateStoreSettingsEntity settings = TemplateHelper.GetStoreSettings(template, storeID);

                if (!settings.EmailUseDefault)
                {
                    allUseDefault = false;
                }
            }

            // If the all use the default settings, then which store to use doesn't matter, since they'll all resolve to the same thing anyway.
            // Possible to have zero stores for a customer who has no orders
            if (allUseDefault && stores.Count > 0)
            {
                return stores[0];
            }

            // If the user has already answered the question and indicated they always want to use the most recent order
            if (useMostRecentOrderForAll)
            {
                return GetStoreFromMostRecentOrder(customer);
            }

            // If there are no stores (like for a customer with no orders), let the user pick from all stores
            if (stores.Count == 0)
            {
                stores = StoreManager.GetAllStores().Select(s => s.StoreID).ToList();
            }

            // If there is actually only one store in the system, use it
            if (stores.Count == 1)
            {
                return stores[0];
            }

            // Raise the event
            EmailSettingsResolveEventArgs args = new EmailSettingsResolveEventArgs((TemplateType) template.Type, stores, customer);
            ResolveSettings(this, args);

            // Cancelation means null return
            if (args.Cancel)
            {
                return null;
            }

            // Use the most recent order of the customer to determine what storeid to use
            if (args.UseMostRecentOrder)
            {
                useMostRecentOrderForAll = args.UseMostRecentOrderForRest;

                return GetStoreFromMostRecentOrder(customer);
            }
            else
            {
                Debug.Assert(args.UseSpecificStoreID.HasValue);

                return args.UseSpecificStoreID;
            }
        }

        /// <summary>
        /// Get the StoreID of the most recent order placed found in the result set
        /// </summary>
        private long GetStoreFromMostRecentOrder(CustomerEntity customer)
        {
            if (customer == null)
            {
                throw new InvalidOperationException("Tried to use most recent order when there was no customer.");
            }

            OrderEntity mostRecent = null;

            // Get all the order's of the customer
            foreach (OrderEntity order in DataProvider.GetRelatedEntities(customer.CustomerID, EntityType.OrderEntity))
            {
                if (mostRecent == null)
                {
                    mostRecent = order;
                }
                else
                {
                    if (order.OrderDate > mostRecent.OrderDate)
                    {
                        mostRecent = order;
                    }
                }
            }

            if (mostRecent != null)
            {
                return mostRecent.StoreID;
            }
            // Its possible it could be null if the customer had no orders
            else
            {
                // Just use the first store.  What else could we do?
                return StoreManager.GetAllStores()[0].StoreID;
            }
        }

        /// <summary>
        /// User associated state
        /// </summary>
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }
    }
}
