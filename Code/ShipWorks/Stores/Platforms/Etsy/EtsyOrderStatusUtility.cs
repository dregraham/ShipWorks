using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Etsy.Enums;

namespace ShipWorks.Stores.Platforms.Etsy
{
    /// <summary>
    /// Handles updating Etsy Statuses Locally.
    /// </summary>
    public static class EtsyOrderStatusUtility
    {
        /// <summary>
        /// Updates Order Paid Status immediately
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="wasPaid">If null, leaves unchanged.</param>
        /// <param name="wasShipped">If null, leaves unchanged.</param>
        public static void UpdateOrderStatus(EtsyOrderEntity order, bool? wasPaid, bool? wasShipped)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();

            UpdateOrderStatus(order, wasPaid, wasShipped, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Updates Order Status in DB and adds it to unitOfWork
        /// </summary>
        /// <param name="wasPaid">If null, leaves unchanged.</param>
        /// <param name="wasShipped">If null, leaves unchanged.</param>
        public static void UpdateOrderStatus(EtsyOrderEntity order, bool? wasPaid, bool? wasShipped, UnitOfWork2 unitOfWork)
        {
            MethodConditions.EnsureArgumentIsNotNull(unitOfWork, nameof(unitOfWork));
            MethodConditions.EnsureArgumentIsNotNull(order, nameof(order));

            if (wasShipped.HasValue)
            {
                order.WasShipped = wasShipped.Value;
            }

            if (wasPaid.HasValue)
            {
                order.WasPaid = wasPaid.Value;
            }

            unitOfWork.AddForSave(order);
        }

        /// <summary>
        /// This function updates the entity with appropriate status information based on shipped/paid status.
        /// If both shipped and paid, order is complete, else open.
        /// THIS SHOULD ONLY BE CALLED BY THE ONFIELDCHANGE FUNCTION DEFINED FOR THE EtsyOrderEntity.
        /// </summary>
        public static void UpdateEntityOrderStatus(EtsyOrderEntity orderEntity)
        {
            MethodConditions.EnsureArgumentIsNotNull(orderEntity, nameof(orderEntity));

            EtsyOrderStatus orderStatus = EtsyOrderStatus.Open;

            if (orderEntity.WasShipped && orderEntity.WasPaid)
            {
                orderStatus = EtsyOrderStatus.Complete;
            }

            orderEntity.OnlineStatus = EnumHelper.GetDescription(orderStatus);
            orderEntity.OnlineStatusCode = orderStatus;
        }

        /// <summary>
        /// Given a list of orders of a specific status, return the order numbers where their etsy status has changed.
        /// If order not found, Online Order Status set to not found
        /// </summary>
        public static List<EtsyOrderEntity> GetOrdersWithChangedStatus(EtsyStoreEntity store, List<EtsyOrderEntity> orders, string etsyFieldName, bool currentStatus)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
            MethodConditions.EnsureArgumentIsNotNull(orders, nameof(orders));

            if (string.IsNullOrWhiteSpace(etsyFieldName))
            {
                throw new ArgumentException("etsyFieldName must be provided and not blank.", "etsyFieldName");
            }

            List<EtsyOrderEntity> changedOrders = new List<EtsyOrderEntity>();
            EtsyWebClient webClient = new EtsyWebClient(store);

            //Create tempOrder Queue to iterate through.
            List<EtsyOrderEntity> tempOrders = new List<EtsyOrderEntity>();
            tempOrders.AddRange(orders);

            while (tempOrders.Any())
            {
                ProcessBatch(etsyFieldName, currentStatus, changedOrders, webClient, tempOrders);
            }

            //return the order objects of the changed orders
            return changedOrders;
        }

        /// <summary>
        /// Process a batch of orders. Add changed orders to changedOrders list. Remove processed orders from tempOrders.
        /// </summary>
        private static void ProcessBatch(string etsyFieldName, bool currentStatus, List<EtsyOrderEntity> changedOrders, EtsyWebClient webClient, List<EtsyOrderEntity> tempOrders)
        {
            // A null reference error was being thrown.  Discovered by Crash Reports.
            // Let's figure out what is null....
            MethodConditions.EnsureArgumentIsNotNull(changedOrders, nameof(changedOrders));
            MethodConditions.EnsureArgumentIsNotNull(webClient, nameof(webClient));
            MethodConditions.EnsureArgumentIsNotNull(tempOrders, nameof(tempOrders));

            int batchSize = EtsyEndpoints.GetOrderLimit;
            bool batchComplete = false;

            while (!batchComplete)
            {
                //Get a batch of order numbers
                var pageOfOrders = (from x in tempOrders.Take(batchSize)
                                    select x).ToList();

                try
                {
                    //there might be less orders than the batch size.
                    batchSize = pageOfOrders.Count();

                    //format the order numbers into a comma separated list
                    string formattedOrderNumbers = string.Join(",", pageOfOrders.Select(x => x.OrderNumber.ToString()));

                    //send list to etsy and add the updated orders to list of changed orders.
                    IEnumerable<long> changedOrderNumbers = webClient.GetOrderNumbersWithChangedStatus(formattedOrderNumbers, etsyFieldName, currentStatus);

                    changedOrders.AddRange((from x in pageOfOrders
                                            where changedOrderNumbers.Contains(x.OrderNumber)
                                            select x).ToList());

                    batchComplete = true;
                }
                catch (EtsyException ex)
                {
                    //If there are more than 1 orders in the batch and etsy threw a 404, split the batch in half.
                    // if there is a 404 and there was only one order, skip this order.
                    if (ex.InnerException is WebException &&
                        ((HttpWebResponse) ((WebException) ex.InnerException).Response).StatusCode == HttpStatusCode.NotFound)
                    {
                        if (batchSize == 1)
                        {
                            //this order
                            batchComplete = true;
                            var missingOrder = pageOfOrders[0];
                            MarkOrderAsNotFound(missingOrder);
                        }
                        else
                        {
                            //split batch in half
                            batchSize = batchSize / 2;
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            //remove the processed orders
            tempOrders.RemoveRange(0, batchSize);
        }

        /// <summary>
        /// If the order is not found by Etsy, use this function to update the online status
        /// </summary>
        public static void MarkOrderAsNotFound(EtsyOrderEntity missingOrder)
        {
            UnitOfWork2 unitOfWork = new UnitOfWork2();

            MarkOrderAsNotFound(missingOrder, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);

                adapter.Commit();
            }
        }

        /// <summary>
        /// If the order is not found by Etsy, use this function to update the online status
        /// </summary>
        public static void MarkOrderAsNotFound(EtsyOrderEntity missingOrder, UnitOfWork2 unitOfWork)
        {
            missingOrder.OnlineStatus = EnumHelper.GetDescription(EtsyOrderStatus.NotFound);
            missingOrder.OnlineStatusCode = EtsyOrderStatus.NotFound;

            unitOfWork.AddForSave(missingOrder);
            using (var adapter = new SqlAdapter())
            {
                adapter.SaveEntity(missingOrder);
            }
        }
    }
}
