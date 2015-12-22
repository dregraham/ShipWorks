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
using Interapptive.Shared;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Stores.Platforms.Ebay.Enums;

namespace ShipWorks.Stores.Platforms.Ebay.OrderCombining
{
    /// <summary>
    /// Loads orders which may be combined into combined payments with the
    /// provided set of orders
    /// </summary>
    public class EbayPotentialCombinedOrderFinder
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
        /// Event raised when the discovery is complete
        /// </summary>
        public event EbayPotentialCombinedOrdersFoundEventHandler SearchComplete;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayPotentialCombinedOrderFinder(Control owner)
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
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public void SearchAsync(ICollection<long> orderKeys, object userState, EbayCombinedOrderType combineType)
        {
            #region validation

            if (orderKeys == null)
            {
                throw new ArgumentNullException("orderKeys");
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

            if (orderKeys.Count > MaxAllowedOrders)
            {
                throw new EbayException(string.Format("You can only select up to {0} orders for ShipWorks to try to combine at a time.", MaxAllowedOrders));
            }

            // configure object that will be the asyncronous state throughout the operation
            Dictionary<string, object> asyncState = new Dictionary<string, object>();
            asyncState.Add("UserState", userState);
            asyncState.Add("CombinedOrderCollection", new List<EbayCombinedOrderCandidate>());
            asyncState.Add("CombineType", combineType);

            // setup to execute in the background
            BackgroundExecutor<EbayCombinedOrderCandidate> executor = new BackgroundExecutor<EbayCombinedOrderCandidate>(owner,
                "Finding Orders to Combine",
                "ShipWorks is searching for orders to be combined.",
                "Searching {0} of {1}...");

            // we are going to handle the exception in OnSearchComplete
            executor.PropagateException = true;

            // What to do when its all done
            executor.ExecuteCompleted += new BackgroundExecutorCompletedEventHandler<EbayCombinedOrderCandidate>(OnSearchComplete);

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
                                ebayOrder.OrderItems.AddRange(DataProvider.GetRelatedEntities(ebayOrder.OrderID, EntityType.OrderItemEntity).Cast<OrderItemEntity>());
                            }

                            // Get the list of eBay items from all of the items
                            List<EbayOrderItemEntity> eBayItems = ebayOrder.OrderItems.OfType<EbayOrderItemEntity>().ToList();

                            // If this order actually has eBay items...
                            if (eBayItems.Count >= 1)
                            {
                                // Local Combining can only be done on Paid orders.  Combined Payments can only be done on unpaid orders
                                if ((combineType == EbayCombinedOrderType.Local && eBayItems.All(i => EbayUtility.GetEffectivePaymentStatus(i) == EbayEffectivePaymentStatus.Paid)) ||
                                    (combineType == EbayCombinedOrderType.Ebay  && eBayItems.All(i => EbayUtility.GetEffectivePaymentStatus(i) == EbayEffectivePaymentStatus.Incomplete)))
                                {
                                    orders.Add(ebayOrder);
                                }
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

                    List<EbayCombinedOrderCandidate> combinedPayments = new List<EbayCombinedOrderCandidate>();
                    foreach (var store in ordersByStoreID)
                    {
                        // group each of these orders by the buyer
                        var byBuyer = from o in store.Orders
                                      group o by o.EbayBuyerID into g
                                      select new { EbayBuyerID = g.Key, Orders = g };

                        foreach (var buyer in byBuyer)
                        {
                            EbayCombinedOrderCandidate combinedPayment = new EbayCombinedOrderCandidate(store.StoreID, combineType, buyer.EbayBuyerID, buyer.Orders.ToList());
                            combinedPayments.Add(combinedPayment);
                        }
                    }

                    #endregion

                    // return one item so the worker gets called 
                    return combinedPayments;
                },

                // Worker
                (EbayCombinedOrderCandidate combinedOrder, object state, BackgroundIssueAdder<EbayCombinedOrderCandidate> issueAdder) =>
                {
                    List<EbayCombinedOrderCandidate> combinedOrders = (List<EbayCombinedOrderCandidate>)asyncState["CombinedOrderCollection"];

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
        private void OnSearchComplete(object sender, BackgroundExecutorCompletedEventArgs<EbayCombinedOrderCandidate> e)
        {
            Dictionary<string, object> state = (Dictionary<string, object>)e.UserState;

            List<EbayCombinedOrderCandidate> candidates = (List<EbayCombinedOrderCandidate>)state["CombinedOrderCollection"];
            if (e.Canceled)
            {
                candidates.Clear();
            }

            // fire the SearchCompleted event handler
            RaiseSearchComplete(e.ErrorException, e.Canceled, state, candidates);
        }

        /// <summary>
        /// The searchhing operation is complete, raises the event.
        /// </summary>
        private void RaiseSearchComplete(Exception error, bool canceled, Dictionary<string, object> state, List<EbayCombinedOrderCandidate> candidates)
        {
            object userState = state["UserState"];
            EbayCombinedOrderType combineType = (EbayCombinedOrderType)state["CombineType"];

            EbayPotentialCombinedOrdersFoundEventHandler handler = SearchComplete;
            if (handler != null)
            {
                EbayPotentialCombinedOrdersFoundEventArgs args = new EbayPotentialCombinedOrdersFoundEventArgs(owner, error, canceled, userState, combineType, candidates);
                handler(this, args);
            }
        }
    }
}
