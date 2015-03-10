using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using System.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.Linq;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Data.Model;
using log4net;
using ShipWorks.Data;
using ShipWorks.Stores.Content;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using Interapptive.Shared.Business;
using ShipWorks.Stores.Platforms.Ebay.Tokens;

namespace ShipWorks.Stores.Platforms.Ebay.OrderCombining
{
    /// <summary>
    /// Represents information for combining eBay orders.
    /// </summary>
    public class EbayCombinedOrderCandidate
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(EbayCombinedOrderCandidate));

        List<EbayCombinedOrderComponent> components = new List<EbayCombinedOrderComponent>();

        bool shippingOverridden = false;
        decimal shippingCost;

        bool adjustmentOverridden = false;
        decimal adjustment;


        /// <summary>
        /// Constructor for specifying a buyer and the user-selected orders by that buyer
        /// </summary>
        public EbayCombinedOrderCandidate(long storeID, EbayCombinedOrderType combinedOrderType, string buyerID, List<EbayOrderEntity> orders)
        {
            if (orders.Count == 0)
            {
                throw new InvalidOperationException("A CombinedOrder must contain at least one order.");
            }

            StoreID = storeID;
            BuyerID = buyerID;
            CombinedOrderType = combinedOrderType;

            // create a component for each order, mark as included because the user explicitly chose these orders
            orders.ForEach(o => components.Add(new EbayCombinedOrderComponent(o, true)));
        }

        /// <summary>
        /// The StoreID this will be combined for
        /// </summary>
        public long StoreID
        {
            get;
            set;
        }

        /// <summary>
        /// The buyer ID that purchased all of the items included
        /// </summary>
        public string BuyerID
        {
            get;
            set;
        }

        /// <summary>
        /// Orders that may or may not be included in the final combined order
        /// </summary>
        public IList<EbayCombinedOrderComponent> Components
        {
            get { return components.AsReadOnly(); }
        }

        /// <summary>
        /// How to combine the order: online or local
        /// </summary>
        public EbayCombinedOrderType CombinedOrderType
        {
            get;
            set;
        }

        /// <summary>
        /// Shipping and Handling is to be taxed
        /// </summary>
        public bool TaxShipping
        {
            get;
            set;
        }

        /// <summary>
        /// State the sales tax is applied for
        /// </summary>
        public string TaxState
        {
            get;
            set;
        }

        /// <summary>
        /// Percent to be applied to the order
        /// </summary>
        public decimal TaxPercent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the shipping service to use when creating an eBay Invoice/Combined Payment
        /// </summary>
        public string ShippingService
        {
            get;
            set;
        }

        /// <summary>
        /// Cost of shipping for the to-be-created combined payment
        /// </summary>
        public decimal ShippingCost
        {
            get
            {
                if (shippingOverridden)
                {
                    return shippingCost;
                }
                else
                {
                    // shipping cost is the sum of the shipping charges on contained orders
                    return components.Where(c => c.Included).Sum(c => c.Order.OrderCharges.Where(charge => charge.Type == "SHIPPING").Sum(charge => charge.Amount));
                }
            }
            set
            {
                shippingOverridden = true;
                shippingCost = value;
            }
        }

        /// <summary>
        /// Cost of shipping for the to-be-created combined payment
        /// </summary>
        public decimal Adjustment
        {
            get
            {
                if (adjustmentOverridden)
                {
                    return adjustment;
                }
                else
                {
                    // shipping cost is the sum of the shipping charges on contained orders
                    return components.Where(c => c.Included).Sum(c => c.Order.OrderCharges.Where(charge => charge.Type == "ADJUSTMENT").Sum(charge => charge.Amount));
                }
            }
            set
            {
                adjustmentOverridden = true;
                adjustment = value;
            }
        }

        /// <summary>
        /// Searches the database for orders from this buyer.  Any orders found that aren't already Components are 
        /// added; meaning that any non-selected qualifying orders are added.
        /// </summary>
        public void DiscoverRelatedOrders()
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                // we know there's at least one order/component.  Enforced in constructor.
                EbayOrderEntity order = components.First().Order;

                EntityCollection<EbayOrderEntity> orders = new EntityCollection<EbayOrderEntity>();

                // we're only allowing combining on orders that aren't already a part of an eBay Order
                RelationPredicateBucket bucket = new RelationPredicateBucket(EbayOrderFields.StoreID == order.StoreID
                    & EbayOrderFields.EbayBuyerID == BuyerID & OrderFields.OnlineLastModified >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(14)));

                // Get order items and charges
                PrefetchPath2 prefetchPath = new PrefetchPath2(EntityType.EbayOrderEntity);
                prefetchPath.Add(OrderEntity.PrefetchPathOrderItems);
                prefetchPath.Add(OrderEntity.PrefetchPathOrderCharges);
                prefetchPath.Add(OrderEntity.PrefetchPathOrderPaymentDetails);

                adapter.FetchEntityCollection(orders, bucket, prefetchPath);

                // Find and keep track of unique orders
                foreach (EbayOrderEntity foundOrder in orders)
                {
                    if (!components.Any(c => c.Order.OrderID == foundOrder.OrderID))
                    {
                        // Get the list of eBay items from all of the items
                        List<EbayOrderItemEntity> eBayItems = foundOrder.OrderItems.OfType<EbayOrderItemEntity>().ToList();

                        bool add = (eBayItems.Count >= 1) && (CombinedOrderType == EbayCombinedOrderType.Local) ?
                            eBayItems.All(i => EbayUtility.GetEffectivePaymentStatus(i) == EbayEffectivePaymentStatus.Paid) :
                            eBayItems.All(i => EbayUtility.GetEffectivePaymentStatus(i) == EbayEffectivePaymentStatus.Incomplete);

                        if (add)
                        {
                            // add it to the this combined order.  For ones we find that aren't in the original selection, don't check them by default for the combined order screen.
                            components.Add(new EbayCombinedOrderComponent(foundOrder, false));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Performs the order combining on those selected contained orders
        /// </summary>
        public bool Combine()
        {
            List<EbayCombinedOrderComponent> toCombine = components.Where(c => c.Included).ToList();

            long? ebayOrderID = null;

            // If combining on eBay, do that first
            if (CombinedOrderType == EbayCombinedOrderType.Ebay)
            {
                EbayStoreEntity ebayStore = StoreManager.GetStore(StoreID) as EbayStoreEntity;

                // build the collection of transactions to be combined
                List<TransactionType> transactions = new List<TransactionType>();

                // Go throughy each order that is going to be combined, and build a list of all eBay transactions from it
                foreach (EbayOrderEntity order in toCombine.Select(c => c.Order))
                {
                    // Ensure the items are loaded for this order
                    if (order.OrderItems.Count == 0)
                    {
                        order.OrderItems.AddRange(DataProvider.GetRelatedEntities(order.OrderID, EntityType.OrderItemEntity).Cast<OrderItemEntity>());
                    }

                    // Create transactions based on each eBay item
                    foreach (EbayOrderItemEntity orderItem in order.OrderItems.OfType<EbayOrderItemEntity>())
                    {
                        // build a TransactionType to identify the transaction with eBay
                        TransactionType transaction = new TransactionType()
                        {
                            TransactionID = orderItem.EbayTransactionID.ToString(),
                            Item = new ItemType() { ItemID = orderItem.EbayItemID.ToString() }
                        };

                        // add it to the list of transactions to be sent to eBay
                        transactions.Add(transaction);
                    }
                }

                // use the most recent order being combined as the template
                EbayOrderEntity orderTemplate = toCombine.OrderByDescending(c => c.Order.OnlineLastModified).First().Order;

                EbayWebClient webClient = new EbayWebClient(EbayToken.FromStore(ebayStore));

                // Combine the orders through eBay and pull out the new eBay order ID
                ebayOrderID = webClient.CombineOrders(
                    transactions,
                    GetCombinedPaymentTotal(toCombine),
                    AcceptedPayments.ParseList(ebayStore.AcceptedPaymentList),
                    ShippingCost,
                    orderTemplate.ShipCountryCode,
                    ShippingService,
                    TaxPercent,
                    TaxState,
                    TaxShipping);
            }

            // create the local combined order
            bool result = false;
            try
            {
                SqlAdapterRetry<SqlDeadlockException> sqlDeadlockRetry = new SqlAdapterRetry<SqlDeadlockException>(5, -5, string.Format("EbayCombinedOrderCandidate.CombineLocalOrders for ebayOrderID {0}", ebayOrderID));
                sqlDeadlockRetry.ExecuteWithRetry((SqlAdapter adapter) =>
                {
                    result = CombineLocalOrders(adapter, toCombine, ebayOrderID);

                    // Don't commit because sqlDeadlockRetry will do the commit
                });
            }
            catch (ORMQueryExecutionException ex)
            {
                // An ORM query exception could occur if one of the mapped tables no longer exists or cannot be found
                // in the database
                // Log the details of the original exception
                log.Error(string.Format("An ORM exception occurred: {0}. {1}", ex.Message, ex.QueryExecuted));

                // Now construct a more user-friendly error message in the form of an eBay exception
                string errorMessage = "An error occurred while saving a combined order. ";
                long? orderNumberBeingDeleted = GetOrderNumberFromORMQueryExecutionException(ex);

                if (orderNumberBeingDeleted.HasValue)
                {
                    errorMessage += string.Format("Failed to delete one of the old orders being combined (order number {0}).", orderNumberBeingDeleted.Value);
                }

                throw new EbayException(errorMessage, ex);
            }
            catch (SqlDeadlockException ex)
            {
                // Log the details of the original exception
                log.Error(string.Format("A SqlDeadlockException exception occurred: {0}", ex.Message));

                throw new EbayException("An error occurred while saving a combined order. ", ex);
            }

            return result;
        }

        /// <summary>
        /// Creates a new order with the order items of the provided orders.  If this new order
        /// is an actual combined order on eBay (not just local) ebayOrderID will be non-null
        /// </summary>
        private bool CombineLocalOrders(SqlAdapter adapter, List<EbayCombinedOrderComponent> toCombine, long? ebayOrderID)
        {
            // Do nothing
            if (toCombine.Count == 0)
            {
                return true;
            }

            bool locallyCombinedOnly = !ebayOrderID.HasValue;
            // Create a new master order based on the most recent order to be combined
            EbayOrderEntity primaryOrder = toCombine.OrderByDescending(c => c.Order.OnlineLastModified).First().Order;

            EbayStoreEntity store = StoreManager.GetStore(primaryOrder.StoreID) as EbayStoreEntity;
            if (store == null)
            {
                log.WarnFormat("Unable to combine orders for buyer {0}, store has been deleted.", BuyerID);
                return false;
            }

            EbayOrderEntity newOrder = StoreTypeManager.GetType(store).CreateOrder() as EbayOrderEntity;
            CopyOrderFields(primaryOrder, newOrder);

            // reset the rollup count
            newOrder.RollupItemCount = 0;
            newOrder.RollupEbayItemCount = 0;
            newOrder.RollupNoteCount = 0;

            // Generate a new order number
            newOrder.OrderNumber = OrderUtility.GetNextOrderNumber(store.StoreID);

            if (!locallyCombinedOnly)
            {
                // apply the eBay order id, thus making it a Combined Payment to ShipWorks
                newOrder.EbayOrderID = ebayOrderID.Value;
            }
            else
            {
                // add a -C to denote a locally combined order
                newOrder.ApplyOrderNumberPostfix("-C");
                newOrder.CombinedLocally = true;
                newOrder.EbayOrderID = 0;
            }

            // At this point, EbayCombinedOrderRelation should be an empty collection as this order was just created.
            EntityCollection<EbayCombinedOrderRelationEntity> combinedOrderRelations = newOrder.EbayCombinedOrderRelation;

            // For every order that is to be combined, we need to merge in items, charges, and payment details
            foreach (EbayCombinedOrderComponent component in toCombine)
            {
                EbayOrderEntity sourceOrder = component.Order;

                MoveOrderItems(sourceOrder, newOrder);
                MovePaymentDetails(sourceOrder, newOrder);
                MoveCharges(sourceOrder, newOrder);

                if (locallyCombinedOnly)
                {
                    if (component.Order.CombinedLocally)
                    {
                        IPredicateExpression relationFilter = new PredicateExpression();
                        relationFilter.Add(EbayCombinedOrderRelationFields.OrderID == component.Order.OrderID);
                        relationFilter.AddWithAnd(EbayCombinedOrderRelationFields.StoreID == component.Order.StoreID);

                        RelationPredicateBucket relationPredicateBucket = new RelationPredicateBucket();
                        relationPredicateBucket.PredicateExpression.Add(relationFilter);

                        using (EntityCollection<EbayCombinedOrderRelationEntity> relationCollection = new EntityCollection<EbayCombinedOrderRelationEntity>())
                        {
                            SqlAdapter.Default.FetchEntityCollection(relationCollection, relationPredicateBucket);
                            foreach (EbayCombinedOrderRelationEntity oldRelation in relationCollection)
                            {
                                AddCombinedOrderRelation(combinedOrderRelations, oldRelation.EbayOrderID, oldRelation.StoreID);
                                adapter.DeleteEntity(oldRelation);
                            }
                        }
                    }
                    else if (component.Order.EbayOrderID != 0 && combinedOrderRelations.All(e => e.EbayOrderID != component.Order.EbayOrderID))
                    {
                        // Create new EbayCombinedOrderRelation
                        AddCombinedOrderRelation(
                            combinedOrderRelations,
                            component.Order.EbayOrderID,
                            component.Order.StoreID);
                    }
                }
            }

            // For order we create on eBay, some charges are applied based on what the user entered or what we calculate
            if (newOrder.EbayOrderID > 0)
            {
                ApplyCalculatedCharges(newOrder);
            }

            // the order total needs to be redone
            newOrder.OrderTotal = OrderUtility.CalculateTotal(newOrder);

            // Save the order, which will include all moved items, payments, and charges
            adapter.SaveAndRefetch(newOrder);

            // Now we need to go through each old order, copy over the notes and shipments, and delete the original order
            foreach (OrderEntity order in toCombine.Select(c => c.Order))
            {
                OrderUtility.CopyNotes(order.OrderID, newOrder);

                OrderUtility.CopyShipments(order.OrderID, newOrder);

                DeletionService.DeleteOrder(order.OrderID, adapter);
            }

            // Everything has been set on the order, so calculate the hash key
            OrderUtility.PopulateOrderDetails(newOrder, adapter);
            OrderUtility.UpdateShipSenseHashKey(newOrder);

            adapter.SaveAndRefetch(newOrder);

            return true;
        }

        /// <summary>
        /// Adds the combined order relation.
        /// </summary>
        /// <param name="combinedOrderRelations">The combined order relations.</param>
        /// <param name="ebayOrderId">The ebay order identifier.</param>
        /// <param name="storeId">The store identifier.</param>
        private static void AddCombinedOrderRelation(EntityCollection<EbayCombinedOrderRelationEntity> combinedOrderRelations, long ebayOrderId, long storeId)
        {
            combinedOrderRelations.Add(new EbayCombinedOrderRelationEntity()
            {
                EbayOrderID = ebayOrderId,
                StoreID = storeId
            });
        }

        /// <summary>
        /// Move the items from the source order to the target order
        /// </summary>
        private void MoveOrderItems(EbayOrderEntity source, EbayOrderEntity target)
        {
            // Ensure the items are loaded on the source
            if (source.OrderItems.Count == 0)
            {
                source.OrderItems.AddRange(DataProvider.GetRelatedEntities(source.OrderID, EntityType.OrderItemEntity).Cast<OrderItemEntity>());
            }

            // Re-associate the items with the new order.  The old order is just going to get deleted anyway
            foreach (OrderItemEntity orderItem in source.OrderItems.ToList())
            {
                orderItem.Order = target;
            }
        }

        /// <summary>
        /// Move the payment details from the source order to the target order
        /// </summary>
        private void MovePaymentDetails(EbayOrderEntity source, EbayOrderEntity target)
        {
            // Ensure payment details are loaded on the source
            if (source.OrderPaymentDetails.Count == 0)
            {
                source.OrderPaymentDetails.AddRange(DataProvider.GetRelatedEntities(source.OrderID, EntityType.OrderPaymentDetailEntity).Cast<OrderPaymentDetailEntity>());
            }

            // Re-associate with the new order.  The old order is just going to get deleted anyway
            foreach (OrderPaymentDetailEntity detail in source.OrderPaymentDetails.ToList())
            {
                detail.Order = target;
            }
        }

        /// <summary>
        /// Move the charges from the source order to the target order
        /// </summary>
        private void MoveCharges(EbayOrderEntity source, EbayOrderEntity target)
        {
            // Ensure charges are loaded on the source
            if (source.OrderCharges.Count == 0)
            {
                source.OrderCharges.AddRange(DataProvider.GetRelatedEntities(source.OrderID, EntityType.OrderChargeEntity).Cast<OrderChargeEntity>());
            }

            // Go through each charge, creating or updating that charge on the target order
            foreach (OrderChargeEntity sourceCharge in source.OrderCharges.ToList())
            {
                // If we combined it on eBay, then the standard charges are going to get applied based on what the user specified when they created they order, not
                // the existing charges
                if (target.EbayOrderID > 0 && new List<string> { "TAX", "SHIPPING", "ADJUST", "OTHER" }.Contains(sourceCharge.Type))
                {
                    continue;
                }

                OrderChargeEntity targetCharge = GetCharge(target, sourceCharge.Type);
                targetCharge.Description = sourceCharge.Description;
                targetCharge.Amount += sourceCharge.Amount;
            }
        }

        /// <summary>
        /// Apply charges calculated from creating a combined payment on eBay
        /// </summary>
        private void ApplyCalculatedCharges(EbayOrderEntity targetOrder)
        {
            // get/fix the shipping amount
            OrderChargeEntity shippingCharge = GetCharge(targetOrder, "SHIPPING");
            shippingCharge.Description = "Shipping";
            shippingCharge.Amount = ShippingCost;

            // add in the adjustment, if any
            if (Adjustment > 0)
            {
                OrderChargeEntity adjustmentCharge = GetCharge(targetOrder, "ADJUST");
                adjustmentCharge.Description = "Adjustment";
                adjustmentCharge.Amount = Adjustment;
            }

            // calculate the tax amount. Calculating this last so that previously added/included charges get counted
            decimal tax = CalculateTaxAmount(targetOrder);
            if (tax > 0)
            {
                OrderChargeEntity taxCharge = GetCharge(targetOrder, "TAX");
                taxCharge.Description = "Sales Tax";
                taxCharge.Amount = tax;
            }
        }

        /// <summary>
        /// Calculate the tax to be assessed to the new combined order
        /// </summary>
        private decimal CalculateTaxAmount(EbayOrderEntity newOrder)
        {
            decimal taxTotal = 0;
            if (TaxPercent > 0)
            {
                // this total will include order items, custom charges, and the adjustment
                decimal orderTotal = OrderUtility.CalculateTotal(newOrder);

                // now include shipping if it is specified as being taxed
                if (!TaxShipping)
                {
                    orderTotal -= ShippingCost;
                }

                // calc the tax amount
                taxTotal = (TaxPercent / 100) * orderTotal;
            }

            return taxTotal;
        }

        /// <summary>
        /// Get the specified charge for the order, and creates one if necessary
        /// </summary>
        private OrderChargeEntity GetCharge(OrderEntity order, string type)
        {
            OrderChargeEntity orderCharge = order.OrderCharges.FirstOrDefault(c => c.Type == type);

            if (orderCharge == null)
            {
                orderCharge = new OrderChargeEntity();
                orderCharge.Order = order;
                orderCharge.Type = type;
            }

            return orderCharge;
        }

        /// <summary>
        /// Calculates the order total to send to eBay when creating the Combined Payment
        /// </summary>
        private double GetCombinedPaymentTotal(List<EbayCombinedOrderComponent> toCombine)
        {
            decimal total = 0;

            toCombine.ForEach(c =>
            {
                total += c.Order.OrderTotal;
            });

            // Find the current shipping costs
            decimal currentShippingCost = toCombine.Sum(c => c.Order.OrderCharges.Sum(charge => charge.Type == "SHIPPING" ? charge.Amount : 0));

            // subtract out the current shipping cost
            total -= currentShippingCost;

            // add in the overridden shipping cost
            total += ShippingCost;

            // return the double value for eBay
            return Convert.ToDouble(total);
        }

        /// <summary>
        /// Copies the non-key fields from one order to another
        /// </summary>
        private void CopyOrderFields(EbayOrderEntity oldOrder, EbayOrderEntity newOrder)
        {
            foreach (IEntityField2 field in oldOrder.Fields)
            {
                if (!(field.IsPrimaryKey || field.IsReadOnly || field.FieldIndex == (int)OrderFieldIndex.OrderNumberComplete))
                {
                    // copy its value to the neworder
                    newOrder.SetNewFieldValue(field.FieldIndex, field.CurrentValue);
                }
            }
        }

        /// <summary>
        /// Helper method to try and extract an order number from an ORMQueryExecutionException
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private long? GetOrderNumberFromORMQueryExecutionException(ORMQueryExecutionException ex)
        {
            // Now construct a more user-friendly error message in the form of an eBay exception
            string orderParameterName = "@OrderID1";

            if (ex.Parameters.Count > 0)
            {
                // Use the value in the order ID parameter to look up the order number being deleted so we can 
                // indicate which order the error occurred on
                System.Data.SqlClient.SqlParameter parameter = ex.Parameters[0] as System.Data.SqlClient.SqlParameter;
                if (parameter != null && parameter.ParameterName == orderParameterName)
                {
                    OrderEntity orderBeingDeleted = DataProvider.GetEntity(long.Parse(parameter.Value.ToString())) as OrderEntity;
                    if (orderBeingDeleted != null)
                    {
                        return orderBeingDeleted.OrderNumber;
                    }
                }
            }

            return null;
        }
    }
}
