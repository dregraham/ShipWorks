using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
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
        public static int MaxAllowedOrders => 1000;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayPotentialCombinedOrderFinder(Control owner)
        {
            MethodConditions.EnsureArgumentIsNotNull(owner, nameof(owner));

            this.owner = owner;
        }

        /// <summary>
        /// Asynchronously search for orders that can be combined with the provided orders.
        /// </summary>
        public async Task<EbayPotentialCombinedOrdersFoundEventArgs> SearchAsync(ICollection<long> orderKeys, object userState, EbayCombinedOrderType combineType)
        {
            MethodConditions.EnsureArgumentIsNotNull(orderKeys, nameof(orderKeys));

            // ensure we were given EbayOrderEntities
            if (orderKeys.Count > 0)
            {
                if (EntityUtility.GetEntityType(orderKeys.First()) != EntityType.OrderEntity)
                {
                    throw new InvalidOperationException("OrderEntity keys must be provided to the CombinedPaymentCandidateLoader");
                }
            }

            if (orderKeys.Count > MaxAllowedOrders)
            {
                throw new EbayException(string.Format("You can only select up to {0} orders for ShipWorks to try to combine at a time.", MaxAllowedOrders));
            }

            // configure object that will be the asynchronous state throughout the operation
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

            // execute the order search, making use of the initializer
            var results = await executor.ExecuteAsync(
                (IProgressReporter progress) => InitializeSearch(orderKeys, combineType, progress),
                PerformSearch,
                asyncState).ConfigureAwait(false);

            return BuildPotentialCandidates(results);
        }

        /// <summary>
        /// Initialize search
        /// </summary>
        /// <remarks>
        /// Initialization gets all of the EbayOrderEntities from their keys and prepares for executing the actual search
        /// </remarks>
        private static List<EbayCombinedOrderCandidate> InitializeSearch(ICollection<long> orderKeys, EbayCombinedOrderType combineType, IProgressReporter progress)
        {
            IEnumerable<EbayOrderEntity> orders = LoadOrders(orderKeys, combineType, progress);

            // check for cancel
            if (progress.IsCancelRequested)
            {
                return null;
            }

            return GroupOrdersIntoCombinationCandidates(combineType, orders);
        }

        /// <summary>
        /// Group a list of orders into combination candidates
        /// </summary>
        private static List<EbayCombinedOrderCandidate> GroupOrdersIntoCombinationCandidates(EbayCombinedOrderType combineType, IEnumerable<EbayOrderEntity> orders)
        {
            List<EbayCombinedOrderCandidate> combinedPayments = new List<EbayCombinedOrderCandidate>();

            var ordersByStoreID = from o in orders
                                  group o by o.StoreID into byStore
                                  select new { StoreID = byStore.Key, Orders = byStore };
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

            // return one item so the worker gets called
            return combinedPayments;
        }

        /// <summary>
        /// Load orders from the given keys
        /// </summary>
        private static IEnumerable<EbayOrderEntity> LoadOrders(ICollection<long> orderKeys, EbayCombinedOrderType combineType, IProgressReporter progress)
        {
            List<EbayOrderEntity> orders = new List<EbayOrderEntity>();

            // first get the orders
            int count = 0;
            foreach (long order in orderKeys)
            {
                if (progress.IsCancelRequested)
                {
                    return Enumerable.Empty<EbayOrderEntity>();
                }

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
                            (combineType == EbayCombinedOrderType.Ebay && eBayItems.All(i => EbayUtility.GetEffectivePaymentStatus(i) == EbayEffectivePaymentStatus.Incomplete)))
                        {
                            orders.Add(ebayOrder);
                        }
                    }
                }

                // progress
                progress.PercentComplete = (100 * count) / orderKeys.Count;
            }

            return orders;
        }

        /// <summary>
        /// Perform the search
        /// </summary>
        private void PerformSearch(EbayCombinedOrderCandidate combinedOrder, object state, BackgroundIssueAdder<EbayCombinedOrderCandidate> issueAdder)
        {
            var asyncState = (Dictionary<string, object>) state;
            List<EbayCombinedOrderCandidate> combinedOrders = (List<EbayCombinedOrderCandidate>) asyncState["CombinedOrderCollection"];

            // search for related orders
            combinedOrder.DiscoverRelatedOrders();

            // add this item to the main collection, but only if there's more than one order
            // since you can't combine an order with itself
            if (combinedOrder.Components.Count > 1)
            {
                combinedOrders.Add(combinedOrder);
            }
        }

        /// <summary>
        /// Searching for related orders is complete
        /// </summary>
        private EbayPotentialCombinedOrdersFoundEventArgs BuildPotentialCandidates(BackgroundExecutorCompletedEventArgs<EbayCombinedOrderCandidate> e)
        {
            Dictionary<string, object> state = (Dictionary<string, object>) e.UserState;

            List<EbayCombinedOrderCandidate> candidates = (List<EbayCombinedOrderCandidate>) state["CombinedOrderCollection"];
            if (e.Canceled)
            {
                candidates.Clear();
            }

            object userState = state["UserState"];
            EbayCombinedOrderType combineType = (EbayCombinedOrderType) state["CombineType"];

            return new EbayPotentialCombinedOrdersFoundEventArgs(owner, e.ErrorException, e.Canceled, userState, combineType, candidates);
        }
    }
}
