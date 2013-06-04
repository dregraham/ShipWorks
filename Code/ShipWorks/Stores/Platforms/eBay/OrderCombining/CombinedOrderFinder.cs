using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Windows.Forms;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using System.Threading;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Stores.Platforms.Ebay.Enums;

namespace ShipWorks.Stores.Platforms.Ebay.OrderCombining
{
    /// <summary>
    /// Loads orders which may be combined into combined payments with the
    /// provided set of orders
    /// </summary>
    public class CombinedOrderFinder
    {
        // Owning control, for showing errors and such
        Control owner;

        /// <summary>
        /// Only allowing to attempt to combine 1000 at a time
        /// </summary>
        public static int MaxAllowedOrders
        {
            get { return 1000; }
        }

        /// <summary>
        /// Event raised when the discovery and loading is complete
        /// </summary>
        public event CombinedOrdersLoadedEventHandler LoadComplete;

        /// <summary>
        /// Constructor
        /// </summary>
        public CombinedOrderFinder(Control owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            this.owner = owner;
        }

        /// <summary>
        /// Asyncronously search for orders that can be combined with the provided orders.
        /// </summary>
        public void LoadAsync(ICollection<long> orderKeys, object userState, CombineType combineType)
        {
            #region validation

            if (orderKeys == null)
            {
                throw new ArgumentNullException("orderKeys");
            }

            if (orderKeys.Count > MaxAllowedOrders)
            {
                throw new InvalidOperationException("Too many orders trying to load at once.");
            }
           
            // ensure we were given EbayOrderEntities
            if (orderKeys.Count > 0)
            {
                if (EntityUtility.GetEntityType(orderKeys.First()) != EntityType.OrderEntity)
                {
                    throw new InvalidOperationException("OrderEntity keys must be provided to the CombinedPaymentCandidateLoader");
                }
            }

            #endregion

            // configure object that will be the asyncronous state throughout the operation
            Dictionary<string, object> asyncState = new Dictionary<string, object>();
            asyncState.Add("UserState", userState);
            asyncState.Add("CombinedOrderCollection", new List<CombinedOrder>());
            asyncState.Add("CombineType", combineType);

            // setup to execute in the background
            BackgroundExecutor<CombinedOrder> executor = new BackgroundExecutor<CombinedOrder>(owner,
                "Finding Orders to Combine",
                "ShipWorks is searching for orders to be combined.",
                "Searching {0} of {1}...");

            // we are going to handle the exception in OnSearchComplete
            executor.PropagateException = true;

            // What to do when its all done
            executor.ExecuteCompleted += new BackgroundExecutorCompletedEventHandler<CombinedOrder>(OnSearchComplete);

            // execute the order search, making use of the initializer
            executor.ExecuteAsync(

                // Initialization gets all of the EbayOrderEntities from their keys and prepares for executing the actual search
                (IProgressReporter progress) =>
                {
                    List<EbayOrderEntity> orders = new List<EbayOrderEntity>();

                    #region Turn selected order keys to order entities

                    // first get the orders
                    int count = 0;
                    foreach (long order in orderKeys)
                    {
                        // progress
                        progress.Detail = String.Format("Loading order {0} of {1}...", ++count, orderKeys.Count);

                        EbayOrderEntity ebayOrder = DataProvider.GetEntity(order) as EbayOrderEntity;
                        if (ebayOrder != null)
                        {
                            // load the order items for this order entity if needed
                            if (ebayOrder.OrderItems.Count == 0)
                            {
                                ebayOrder.OrderItems.AddRange(DataProvider.GetRelatedEntities(ebayOrder.OrderID, EntityType.EbayOrderItemEntity).Cast<OrderItemEntity>());
                            }

                            // Local Combining can only be done on Paid orders.  Combined Payments can only be done on unpaid orders
                            if ((combineType == CombineType.Local && EbayUtility.GetEffectiveCheckoutStatus((EbayOrderItemEntity)ebayOrder.OrderItems.First()) == EbayEffectiveCheckoutStatus.Paid) ||
                                (combineType == CombineType.EbayCombinedPayment && EbayUtility.GetEffectiveCheckoutStatus((EbayOrderItemEntity)ebayOrder.OrderItems.First()) == EbayEffectiveCheckoutStatus.Incomplete))
                            {
                                orders.Add(ebayOrder);
                            }
                        }

                        // progress
                        progress.PercentComplete = (100 * count) / orderKeys.Count;
                    }

                    // check for cancel
                    if (progress.IsCancelRequested)
                    {
                        return null;
                    }

                    #endregion

                    #region Group entities by Store and Buyer

                    // now group the orders by store and buyer
                    var ordersByStoreID = from o in orders
                                          group o by o.StoreID into byStore
                                          select new { StoreID = byStore.Key, Orders = byStore };

                    List<CombinedOrder> combinedPayments = new List<CombinedOrder>();
                    foreach (var store in ordersByStoreID)
                    {
                        // group each of these orders by the buyer
                        var byBuyer = from o in store.Orders
                                      group o by o.EbayBuyerID into g
                                      select new { EbayBuyerID = g.Key, Orders = g };

                        foreach (var buyer in byBuyer)
                        {
                            CombinedOrder combinedPayment = new CombinedOrder(store.StoreID, combineType, buyer.EbayBuyerID, buyer.Orders.ToList());
                            combinedPayments.Add(combinedPayment);
                        }
                    }

                    #endregion

                    // return one item so the worker gets called 
                    return combinedPayments;
                },

                // Worker
                (CombinedOrder combinedOrder, object state, BackgroundIssueAdder<CombinedOrder> issueAdder) =>
                {
                    List<CombinedOrder> combinedOrders = (List<CombinedOrder>)asyncState["CombinedOrderCollection"];

                    // search for related orders
                    combinedOrder.DiscoverRelatedOrders();

                    // add this item to the main collection, but only if there's more than one order
                    // since you can't combine an order with itself
                    if (combinedOrder.Components.Count > 1)
                    {
                        combinedOrders.Add(combinedOrder);
                    }
                },
                    
                // collection for collating results
                (object)asyncState);
        }

        /// <summary>
        /// Searching for related orders is complete
        /// </summary>
        private void OnSearchComplete(object sender, BackgroundExecutorCompletedEventArgs<CombinedOrder> e)
        {
            Dictionary<string, object> state = (Dictionary<string, object>)e.UserState;

            List<CombinedOrder> combinedPayments = (List<CombinedOrder>)state["CombinedOrderCollection"];
            if (e.Canceled)
            {
                combinedPayments.Clear();
            }

            // fire the LoadCompleted event handler
            OnLoadComplete(e.ErrorException, e.Canceled, state, combinedPayments);
        }

        /// <summary>
        /// The loading operation is complete, raises the event.
        /// </summary>
        private void OnLoadComplete(Exception error, bool canceled, Dictionary<string,object> state, List<CombinedOrder> combinedPayments)
        {
            object userState = state["UserState"];
            CombineType combineType = (CombineType)state["CombineType"];

            CombinedOrdersLoadedEventHandler handler = LoadComplete;
            if (handler != null)
            {
                CombinedOrdersLoadedEventArgs args = new CombinedOrdersLoadedEventArgs(owner, error, canceled, userState, combineType, combinedPayments);
                handler(this, args);
            }
        }
    }
}
