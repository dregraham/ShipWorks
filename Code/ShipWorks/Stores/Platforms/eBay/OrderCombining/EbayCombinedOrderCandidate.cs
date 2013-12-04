using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
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
                    & EbayOrderFields.EbayBuyerID == BuyerID & EbayOrderFields.EbayOrderID == 0 & OrderFields.OnlineLastModified >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(14)));

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
                        bool add = (CombinedOrderType == EbayCombinedOrderType.Local) ? 
                            EbayUtility.GetEffectiveCheckoutStatus((EbayOrderItemEntity)foundOrder.OrderItems.First()) == EbayEffectiveCheckoutStatus.Paid :
                            EbayUtility.GetEffectiveCheckoutStatus((EbayOrderItemEntity)foundOrder.OrderItems.First()) == EbayEffectiveCheckoutStatus.Incomplete;

                        if (add)
                        {
                            // add it to the this combined order
                            components.Add(new EbayCombinedOrderComponent(foundOrder, true));
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

            // make sure we have all order details in memory
            toCombine.ForEach(c => EnsureRelatedEntities(c.Order));

            if (CombinedOrderType == EbayCombinedOrderType.Ebay)
            {
                EbayStoreEntity ebayStore = StoreManager.GetStore(StoreID) as EbayStoreEntity;

                // build the collection of transactions to be combined
                List<TransactionType> transactions = new List<TransactionType>();
                foreach (EbayCombinedOrderComponent component in toCombine)
                {
                    foreach (EbayOrderItemEntity orderItem in component.Order.OrderItems)
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
            return CombineLocalOrders(toCombine, ebayOrderID);
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
        /// Creates a new order with the order items of the provided orders.  If this new order
        /// is an actual combined order on eBay (not just local) ebayOrderID will be non-null
        /// </summary>
        private bool CombineLocalOrders(List<EbayCombinedOrderComponent> toCombine, long? ebayOrderID)
        {
            if (toCombine.Count == 0)
            {
                // do nothing
                return true;
            }

            // create a new master order baesd on the most recent Order to be combined.  Choosing the most recent to copy
            // will prevent orders from being re-downloaded since a more recent order may be turned into an orderitem
            EbayOrderEntity orderTemplate = toCombine.OrderByDescending(c => c.Order.OnlineLastModified).First().Order;

            EbayStoreEntity storeEntity = StoreManager.GetStore(orderTemplate.StoreID) as EbayStoreEntity;
            if (storeEntity == null)
            {
                log.WarnFormat("Unable to combine orders for buyer {0}, store has been deleted.", BuyerID);
                return false;
            }

            EbayOrderEntity newOrder = StoreTypeManager.GetType(storeEntity).CreateOrderInstance() as EbayOrderEntity;
            CopyOrderFields(orderTemplate, newOrder);

            // reset the rollup count
            newOrder.RollupItemCount = 0;
            newOrder.RollupEbayItemCount = 0;
            newOrder.RollupNoteCount = 0;

            // generate a new ID
            AssignOrderNumber(newOrder);

            if (ebayOrderID.HasValue)
            {
                // apply the eBay order id, thus making it a Combined Payment to ShipWorks
                newOrder.EbayOrderID = ebayOrderID.Value;
            }
            else
            {
                // add a -C to denote a locally combined order
                newOrder.ApplyOrderNumberPostfix("-C");
                newOrder.EbayOrderID = 0;
            }

            // keep track of any moved notes and shipments
            List<ShipmentEntity> movedShipments = new List<ShipmentEntity>();
            List<NoteEntity> movedNotes = new List<NoteEntity>();

            foreach (EbayCombinedOrderComponent component in toCombine)
            {
                EbayOrderEntity sourceOrder = component.Order;

                MergeOrder(sourceOrder, newOrder, movedShipments, movedNotes);
            }

            CleanupOrder(newOrder);

            // the order total needs to be redone
            newOrder.OrderTotal = OrderUtility.CalculateTotal(newOrder);

            try
            {
                // save the new order
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // save the order and all moved items, payments, charges, etc.
                    adapter.SaveAndRefetch(newOrder);

                    // save shipments
                    movedShipments.ForEach(s => ShippingManager.SaveShipment(s));

                    // save notes
                    movedNotes.ForEach(n => NoteManager.SaveNote(n));

                    // delete the old orders
                    toCombine.ForEach(c => DeletionService.DeleteOrder(c.Order.OrderID));

                    // commit the transaction
                    adapter.Commit();
                }
            }
            catch (ORMQueryExecutionException ex)
            {
                // An ORM query exception could occur if one of the mapped tables no longer exists or cannot be found
                // in the database
                
                // Log the details of the original exception
                log.Error(string.Format("An ORM exception occurred: {0}. {1}", ex.Message, ex.QueryExecuted));

                // Now construct a more user-friendly error message in the form of an eBay exception
                string orderParameterName = "@OrderID1";
                string errorMessage = "An error occurred while saving a combined order. ";

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
                            errorMessage += string.Format("Failed to delete one of the old orders being combined (order number {0}).", orderBeingDeleted.OrderNumber);
                        }
                    }
                }

                throw new EbayException(errorMessage, ex);
            }

            return true;
        }

        /// <summary>
        /// Cleanup unnecessary data as a result of the merge
        /// </summary>
        private void CleanupOrder(EbayOrderEntity newOrder)
        {
            // detach all order charges and remove them from the collection
            List<OrderChargeEntity> orderCharges = newOrder.OrderCharges.ToList();
            orderCharges.ForEach(c => c.Order = null);

            // add back in the charges, combined based on type
            foreach (string chargeType in orderCharges.Select(c => c.Type).Distinct())
            {
                // for eBay Combined Payments, don't carry over the standard charges to the new order
                if (newOrder.EbayOrderID > 0 &&
                    (chargeType == "TAX" || chargeType == "SHIPPING" || chargeType == "ADJUST"))
                {
                    continue;
                }

                // get the total for this type
                decimal sum = orderCharges.Sum(c => c.Type == chargeType ? c.Amount : 0);
                string description = orderCharges.First(c => c.Type == chargeType).Description;

                OrderChargeEntity orderCharge = new OrderChargeEntity()
                {
                    Type = chargeType,
                    Description = description,
                    Amount = sum
                };

                newOrder.OrderCharges.Add(orderCharge);
            }

            // for eBay combined payments, add in the standard charges
            if (newOrder.EbayOrderID > 0)
            {
                // get/fix the shipping amount
                OrderChargeEntity shippingCharge = GetCharge(newOrder, "SHIPPING", true);
                shippingCharge.Description = "Shipping";
                shippingCharge.Amount = ShippingCost;

                // add in the adjustment
                if (Adjustment > 0)
                {
                    OrderChargeEntity adjustmentCharge = GetCharge(newOrder, "ADJUST", true);
                    adjustmentCharge.Description = "Adjustment";
                    adjustmentCharge.Amount = Adjustment;
                }

                // calculate the tax amount. Calculating this last so that previously added/included charges get counted
                decimal tax = CalculateTaxAmount(newOrder);
                if (tax > 0)
                {
                    OrderChargeEntity taxCharge = GetCharge(newOrder, "TAX", true);
                    taxCharge.Description = "Sales Tax";
                    taxCharge.Amount = tax;
                }
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
        /// Gets the largest OrderNumber we have in our database for non-manual orders for this store.  If no
        /// such orders exist, zero is returned.
        /// </summary>
        protected long GetMaxOrderNumber(long storeID)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                object result = adapter.GetScalar(
                    OrderFields.OrderNumber,
                    null, AggregateFunction.Max,
                    OrderFields.StoreID == storeID & OrderFields.IsManual == false);

                long orderNumber = result is DBNull ? 0 : (long)result;

                log.InfoFormat("MAX(OrderNumber) = {0}", orderNumber);

                return orderNumber;
            }
        }

        /// <summary>
        /// Get the specified charge for the order
        /// </summary>
        private OrderChargeEntity GetCharge(OrderEntity order, string type, bool autoCreate)
        {
            OrderChargeEntity orderCharge = order.OrderCharges.FirstOrDefault(c => c.Type == type);

            if (orderCharge == null)
            {
                if (autoCreate)
                {
                    // create a new one
                    orderCharge = new OrderChargeEntity();
                    orderCharge.Order = order;
                    orderCharge.Type = type;
                }
            }

            return orderCharge;
        }

        /// <summary>
        /// Assigns the next order number to the order
        /// </summary>
        private void AssignOrderNumber(EbayOrderEntity order)
        {
            order.OrderNumber = GetMaxOrderNumber(order.StoreID) + 1;
        }

        /// <summary>
        /// Moves child rows from one order to another
        /// </summary>
        private void MergeOrder(EbayOrderEntity sourceOrder, EbayOrderEntity newOrder, List<ShipmentEntity> movedShipments, List<NoteEntity> movedNotes)
        {
            EnsureRelatedEntities(sourceOrder);

            // move the order items.  ToList intentional since changing an item's Order changes that collection
            foreach (OrderItemEntity orderItem in sourceOrder.OrderItems.ToList())
            {
                orderItem.Order = newOrder;
            }

            // combine order charges
            foreach (OrderChargeEntity orderCharge in sourceOrder.OrderCharges.ToList())
            {
                orderCharge.Order = newOrder;
            }

            // move the payment details
            foreach (OrderPaymentDetailEntity detail in sourceOrder.OrderPaymentDetails.ToList())
            {
                detail.Order = newOrder;
            }

            // move any shipments
            List<ShipmentEntity> oldShipments = ShippingManager.GetShipments(sourceOrder.OrderID, false);
            oldShipments.ForEach(s =>
            {
                // assign it to the new order 
                s.Order = newOrder;

                // track the moved shipment
                movedShipments.Add(s);
            });

            // move any notes
            using (SqlAdapter adapter = new SqlAdapter())
            {
                EntityCollection<NoteEntity> oldOrderNotes = new EntityCollection<NoteEntity>();
                adapter.FetchEntityCollection(oldOrderNotes, NoteManager.GetNotesQuery(sourceOrder.OrderID));

                foreach (NoteEntity note in oldOrderNotes)
                {
                    // change the order the note is associated with
                    note.Order = newOrder;

                    // update the note text
                    note.Text = String.Format("From merged order {0}: \r\n{1}", sourceOrder.OrderNumberComplete, note.Text);

                    // keep track of the moved note so it can be saved later
                    movedNotes.Add(note);
                }
            }
        }

        /// <summary>
        /// Ensures all related entity collections are loaded on the provided order.
        /// </summary>
        private void EnsureRelatedEntities(EbayOrderEntity order)
        {
            if (order.OrderItems.Count == 0)
            {
                // load order items
                order.OrderItems.AddRange(DataProvider.GetRelatedEntities(order.OrderID, EntityType.EbayOrderItemEntity).Cast<OrderItemEntity>());
            }

            if (order.OrderCharges.Count == 0)
            {
                // load charges
                order.OrderCharges.AddRange(DataProvider.GetRelatedEntities(order.OrderID, EntityType.OrderChargeEntity).Cast<OrderChargeEntity>());
            }

            if (order.OrderPaymentDetails.Count == 0)
            {
                // load payment details
                order.OrderPaymentDetails.AddRange(DataProvider.GetRelatedEntities(order.OrderID, EntityType.OrderPaymentDetailEntity).Cast<OrderPaymentDetailEntity>());
            }
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
    }
}
