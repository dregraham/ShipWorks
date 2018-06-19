﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.OnlineUpdating
{
    /// <summary>
    /// Client for updating an eBay order with shipped/paid status
    /// </summary>
    [Component]
    public class EbayOnlineUpdater : IEbayOnlineUpdater
    {
        // Logger
        private readonly ILog log;
        private readonly IEbayWebClient webClient;
        private readonly EbayCombineOrderSearchProvider orderSearchProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayOnlineUpdater(IEbayWebClient webClient, EbayCombineOrderSearchProvider orderSearchProvider, Func<Type, ILog> createLogger)
        {
            this.orderSearchProvider = orderSearchProvider;
            this.webClient = webClient;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Sends a message to the buyer associated with the entityID (shipment or order)
        /// </summary>
        [NDependIgnoreTooManyParams]
        public async Task SendMessage(IEbayStoreEntity store, long entityID, EbaySendMessageType messageType, string subject, string message, bool copySender)
        {
            List<EbayException> exceptions = new List<EbayException>();

            // Send the message for each item in the collection
            foreach (EbayOrderItemEntity orderItem in DetermineEbayItems(entityID))
            {
                try
                {
                    // Need the buyer and to convert the ShipWorks ebay message type to the
                    // type expected by eBay to send an eBay message
                    string buyerID = await FetchEbayOrderItemBuyerId(orderItem.Order, orderItem.OriginalOrderID).ConfigureAwait(false);
                    await SendMessage(CreateTransactionDetails(store, orderItem, buyerID), messageType, subject, message, copySender).ConfigureAwait(false);
                }
                catch (EbayException ex)
                {
                    // Log the exception and re-throw it
                    log.ErrorFormat("Exception sending a message for order {1}: {2}.", orderItem.Order.OrderNumber, ex.Message);
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new EbayException(string.Join($"{ Environment.NewLine }", exceptions.Select(e => e.Message)));
            }
        }

        /// <summary>
        /// create the ebay transaction details
        /// </summary>
        private EbayTransactionDetails CreateTransactionDetails(IEbayStoreEntity store, IEbayOrderItemEntity orderItem, string buyerId)
        {
            return new EbayTransactionDetails()
            {
                Token = EbayToken.FromStore(store),
                ItemID = orderItem.EbayItemID,
                TransactionID = orderItem.EbayTransactionID,
                BuyerID = buyerId
            };
        }

        /// <summary>
        /// Send a message with the given properties to the buyer of the order item specified
        /// </summary>
        private async Task SendMessage(EbayTransactionDetails transaction, EbaySendMessageType messageType, string subject, string message, bool copySender)
        {
            QuestionTypeCodeType ebayMessageType = EbayUtility.GetEbayQuestionTypeCode(messageType);

            try
            {
                // Fire off the message
                await Task.Run(() => webClient.SendMessage(transaction, ebayMessageType, subject, message, copySender))
                    .ConfigureAwait(false);
            }
            catch (EbayException ex)
            {
                // Log the exception and re-throw it
                log.ErrorFormat("Exception sending a message to {0} for order {1}, order item {2}: {3}.", transaction.BuyerID, transaction.TransactionID, transaction.ItemID, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get the EbayOrderSearchEntity for the given order and orderItemID
        /// </summary>
        private async Task<string> FetchEbayOrderItemBuyerId(IOrderEntity order, long originalOrderID)
        {
            IEnumerable<EbayOrderSearchEntity> orderSearchEntities = await orderSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

            return orderSearchEntities.FirstOrDefault(os => os.OriginalOrderID == originalOrderID)?.EbayBuyerID;
        }

        /// <summary>
        /// Leave Feedback
        /// </summary>
        public async Task LeaveFeedback(IEbayStoreEntity store, long entityID, CommentTypeCodeType feedbackType, string feedback)
        {
            List<EbayException> exceptions = new List<EbayException>();

            log.InfoFormat("Preparing to leave eBay feedback for entity id {0}", entityID);

            // Leave feedback for those that haven't had feedback left for them already
            foreach (EbayOrderItemEntity orderItem in DetermineEbayItems(entityID).Where(i => i.FeedbackLeftType == (int) EbayFeedbackType.None))
            {
                try
                {
                    await LeaveFeedback(store, orderItem, feedbackType, feedback).ConfigureAwait(false);
                }
                catch (EbayException ex)
                {
                    // Log the exception and re-throw it
                    log.ErrorFormat("Exception sending a message for order {0}, order item {1}: {2}.", orderItem.Order.OrderNumber, orderItem.EbayItemID, ex.Message);
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new EbayException(string.Join($"{ Environment.NewLine }", exceptions.Select(e => e.Message)));
            }
        }

        /// <summary>
        /// Leaves eBay feedback for an eBay Order Item
        /// </summary>
        private async Task LeaveFeedback(IEbayStoreEntity store, EbayOrderItemEntity orderItem, CommentTypeCodeType feedbackType, string feedback)
        {
            string error = string.Empty;

            try
            {
                // log that we're about to make the call
                log.InfoFormat("Preparing to leave eBay feedback for order id {0}, eBay Order ID {1}, eBay Transaction ID {2}.",
                    orderItem.OrderID, orderItem.EbayItemID, orderItem.EbayTransactionID);

                string buyerID = await FetchEbayOrderItemBuyerId(orderItem.Order, orderItem.OriginalOrderID).ConfigureAwait(false);

                await Task.Run(() => webClient.LeaveFeedback(CreateTransactionDetails(store, orderItem, buyerID), feedbackType, feedback))
                        .ConfigureAwait(false);

                log.InfoFormat("Successfully left feedback for order id {0}, eBay Order ID {1}, eBay Transaction ID {2}.",
                    orderItem.OrderID, orderItem.EbayItemID, orderItem.EbayTransactionID);

                // success, since no exception was raised
                orderItem.FeedbackLeftType = (int) EbayUtility.GetShipWorksFeedbackType(feedbackType);
                orderItem.FeedbackLeftComments = feedback;
            }
            catch (EbayException ex)
            {
                log.ErrorFormat("Failed to leave feedback for order id {0}, eBay Order ID {1}, eBay Transaction ID {2}: {3}",
                    orderItem.OrderID, orderItem.EbayItemID, orderItem.EbayTransactionID, ex.Message);

                // 834 is the error code for feedback already left
                if (ex.ErrorCode == "834")
                {
                    orderItem.FeedbackLeftType = (int) EbayFeedbackType.Unknown;
                    orderItem.FeedbackLeftComments = "(unknown)";

                    error = string.Format("Unable to leave feedback for Order{0}: Feedback has already been left.", orderItem.Order.OrderNumber);
                }
                else
                {
                    error = string.Format("Unable to leave feedback for Order {0}: {1}", orderItem.Order.OrderNumber, ex.Message);
                }
            }

            // save the item
            using (SqlAdapter adapter = new SqlAdapter())
            {
                // no need to refetch
                adapter.SaveEntity(orderItem);
            }

            // if an error occurred, raise an exception
            if (error.Length > 0)
            {
                throw new EbayException(error, (Exception) null);
            }
        }

        /// <summary>
        /// Determine the list of non-manual EBayOrderItemEntity objects related to the specified entityID, which can be an OrderID or OrderItemID.  The EBayOrder entity
        /// is attached to each item before it is returned.
        /// </summary>
        private List<EbayOrderItemEntity> DetermineEbayItems(long entityID)
        {
            List<EbayOrderItemEntity> items = new List<EbayOrderItemEntity>();

            EntityType entityType = EntityUtility.GetEntityType(entityID);
            switch (entityType)
            {
                case EntityType.OrderEntity:
                    {
                        // If an OrderID was provided, consider all child items.  By using OfType<EbayOrerItemEntity>, we get rid
                        // of any that are manual (but we double-check anyway)
                        items.AddRange(DataProvider.GetRelatedEntities(entityID, EntityType.OrderItemEntity).OfType<EbayOrderItemEntity>().Where(i => !i.IsManual));

                        break;
                    }

                case EntityType.OrderItemEntity:
                    {
                        // If its an item, attach the single item.
                        EbayOrderItemEntity orderItem = DataProvider.GetEntity(entityID) as EbayOrderItemEntity;
                        if (orderItem != null && !orderItem.IsManual)
                        {
                            items.Add(orderItem);
                        }

                        break;
                    }

                default:
                    {
                        throw new InvalidOperationException("eBay Messages can only be sent for an order or order item.");
                    }
            }

            foreach (EbayOrderItemEntity item in items.ToList())
            {
                // If the order isn't attached, get it now
                if (item.Order == null)
                {
                    item.Order = DataProvider.GetEntity(item.OrderID) as EbayOrderEntity;
                }

                // If we still don't have the order, or if it was manual, we can't go on.
                if (item.Order == null || item.Order.IsManual)
                {
                    log.InfoFormat("Unable to update for manual or deleted order {0}.", item.OrderID);
                    items.Remove(item);
                }

                // Check if its a manual item
                if (item.EbayItemID == 0 || item.IsManual)
                {
                    log.InfoFormat("Cannot send an eBay message for a locally created order item {0}.", item.OrderItemID);
                    items.Remove(item);
                }
            }

            return items;
        }

        /// <summary>
        /// Push the online status for an order.
        /// </summary>
        public void UpdateOnlineStatus(IEbayStoreEntity store, long orderID, bool? paid, bool? shipped)
        {
            UnitOfWork2 unitOfWork = new ManagedConnectionUnitOfWork2();
            UpdateOnlineStatus(store, orderID, paid, shipped, unitOfWork);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                unitOfWork.Commit(adapter);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Push the online status for an order.
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public void UpdateOnlineStatus(IEbayStoreEntity store, long orderID, bool? paid, bool? shipped, UnitOfWork2 unitOfWork)
        {
            var token = EbayToken.FromStore(store);
            EbayOrderEntity order = DataProvider.GetEntity(orderID) as EbayOrderEntity;

            // IsManual should actually never be true - instead order will be null, b\c manual orders (at this time) don't have a derived table presence.
            if (order == null || order.IsManual)
            {
                log.WarnFormat("Not updating online status for order {0} because it is manual or was deleted.", orderID);
                return;
            }

            // We need the items
            var items = DataProvider.GetRelatedEntities(orderID, EntityType.OrderItemEntity);

            // We only want the Ebay instances (i.e., the non-manual ones)
            order.OrderItems.Clear();
            order.OrderItems.AddRange(items.OfType<EbayOrderItemEntity>().Cast<OrderItemEntity>());

            // update each item
            foreach (EbayOrderItemEntity ebayItem in order.OrderItems.OfType<EbayOrderItemEntity>())
            {
                // This was a manually added item, we can't update ebay with it so continue on to the next one
                if (ebayItem.IsManual || ebayItem.EbayItemID == 0)
                {
                    log.WarnFormat("Not updating online status for order item {0} because it is manual or was deleted.", ebayItem.EbayItemID);
                    continue;
                }

                string trackingNumber = string.Empty;
                string carrierCode = ShippingCarrierCodeType.CustomCode.ToString();

                if (shipped.HasValue && shipped.Value)
                {
                    // We have shipping information, so get the shipment details
                    ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(order.OrderID);

                    if (shipment != null)
                    {
                        try
                        {
                            ShippingManager.EnsureShipmentLoaded(shipment);
                        }
                        catch (ObjectDeletedException)
                        {
                            log.InfoFormat("Not uploading tracking number since shipment {0} or related information has been deleted.", shipment.ShipmentID);
                            continue;
                        }
                        catch (SqlForeignKeyException)
                        {
                            log.InfoFormat("Not uploading tracking number since shipment {0} or related information has been deleted.", shipment.ShipmentID);
                            continue;
                        }

                        // Determine the carrier code and tracking info
                        GetShippingCarrierAndTracking(shipment, out carrierCode, out trackingNumber);

                        // can only upload tracking details if it's a supported carrier
                        if (carrierCode.Equals(ShippingCarrierCodeType.CustomCode.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            // We can't upload tracking details because this isn't a supported carrier, so create a
                            // note with the tracking number instead
                            string notesText = string.Format("Shipped {0} on {1}. Tracking number: {2}",
                                ShippingManager.GetOverriddenServiceUsed(shipment), shipment.ShipDate.ToShortDateString(), shipment.TrackingNumber);

                            webClient.AddUserNote(token, ebayItem.EbayItemID, ebayItem.EbayTransactionID, notesText);
                        }
                    }
                }

                // set paid value
                if (paid.HasValue &&
                    !paid.Value &&
                    ebayItem.EffectivePaymentMethod == (int) EbayEffectivePaymentMethod.PayPal &&
                    ebayItem.EffectiveCheckoutStatus == (int) EbayEffectivePaymentStatus.Paid)
                {
                    // cannot do this
                    throw new EbayException("Cannot change the Paid status of a completed PayPal payment.", "");
                }

                // log that we're about to make the request and send the request
                log.InfoFormat("Preparing to update eBay order status for order id {0}.", orderID);

                EbayTransactionDetails transaction = new EbayTransactionDetails()
                {
                    Token = token,
                    ItemID = ebayItem.EbayItemID,
                    TransactionID = ebayItem.EbayTransactionID
                };

                webClient.CompleteSale(transaction, paid, shipped, trackingNumber, carrierCode);

                // update the shipped flag
                if (shipped.HasValue)
                {
                    ebayItem.MyEbayShipped = shipped.Value;

                    // Only overwrite Local Status if it's blank
                    if (string.IsNullOrEmpty(order.LocalStatus))
                    {
                        order.LocalStatus = "Shipped";
                    }
                }

                // update the paid flag
                if (paid.HasValue)
                {
                    ebayItem.MyEbayPaid = paid.Value;
                }

                // Save the order on each iteration of the child item as this also recurses to save the child item,
                // and we want it updated right away.
                unitOfWork.AddForSave(order);
            }
        }

        /// <summary>
        /// A helper method to obtain the shipping carrier and tracking info from the shipment
        /// </summary>
        public static void GetShippingCarrierAndTracking(ShipmentEntity shipment, out string carrierCode, out string trackingNumber)
        {
            // default the type to Other
            ShippingCarrierCodeType carrierType = ShippingCarrierCodeType.CustomCode;
            bool useUpsMailInnovationsCarrierType = false;

            trackingNumber = shipment.TrackingNumber;

            // Is it a USPS shipment?
            switch ((ShipmentTypeCode) shipment.ShipmentType)
            {
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                    PostalServiceType service = (PostalServiceType) shipment.Postal.Service;

                    // The shipment is an Endicia/Usps shipment, check to see if it's DHL
                    if (ShipmentTypeManager.IsDhl(service))
                    {
                        // The DHL carrier for Endicia/Usps is:
                        carrierType = ShippingCarrierCodeType.DHL;
                    }
                    else if (ShipmentTypeManager.IsConsolidator(service))
                    {
                        carrierType = ShippingCarrierCodeType.Other;
                    }
                    else
                    {
                        // Use the default carrier for other Endicia/Usps types
                        carrierType = ShippingCarrierCodeType.USPS;
                    }
                    break;

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    carrierType = ShippingCarrierCodeType.UPS;

                    if (UpsUtility.IsUpsMiService((UpsServiceType) shipment.Ups.Service))
                    {
                        // Try to use the alternate tracking number for MI if it's set
                        if (!string.IsNullOrEmpty(shipment.Ups.UspsTrackingNumber))
                        {
                            trackingNumber = shipment.Ups.UspsTrackingNumber;
                        }

                        // International MI seems to use the normal UPS tracking number, so we'll just use that
                        if (!string.IsNullOrEmpty(trackingNumber))
                        {
                            // From eBay web service info:
                            // For those using UPS Mail Innovations, supply the value UPS-MI for UPS Mail Innovations.
                            // Buyers will subsequently be sent to the UPS Mail Innovations website for tracking.
                            useUpsMailInnovationsCarrierType = true;
                        }
                        else
                        {
                            // Mail Innovations but without a USPS tracking number will just get uploaded
                            // as an Other shipment.  Tracking will be whatever the user entered in the Reference 1 field
                            // in the SW WorldShip window.
                            carrierType = ShippingCarrierCodeType.Other;
                        }
                    }

                    break;

                case ShipmentTypeCode.FedEx:
                    carrierType = ShippingCarrierCodeType.FedEx;
                    break;

                case ShipmentTypeCode.OnTrac:
                    carrierType = ShippingCarrierCodeType.ONTRACK;
                    break;

                case ShipmentTypeCode.Other:
                    CarrierDescription description = ShippingManager.GetOtherCarrierDescription(shipment);

                    carrierType = description.IsUPS ? ShippingCarrierCodeType.UPS :
                        description.IsFedEx ? ShippingCarrierCodeType.FedEx :
                        description.IsUSPS ? ShippingCarrierCodeType.USPS :
                        description.IsDHL ? ShippingCarrierCodeType.DHL :
                        ShippingCarrierCodeType.Other;

                    break;
            }

            // Now check for any specialty cases.
            carrierCode = GetCarrierCodeForSpecialtyCases(carrierType, useUpsMailInnovationsCarrierType);
        }

        /// <summary>
        /// Determine carrier code for specialty cases like UPS MI and DHL
        /// </summary>
        private static string GetCarrierCodeForSpecialtyCases(ShippingCarrierCodeType carrierType, bool useUpsMailInnovationsCarrierType)
        {
            string carrierCode = carrierType.ToString();

            if (useUpsMailInnovationsCarrierType)
            {
                carrierCode = "UPS-MI";
            }
            else if (carrierType == ShippingCarrierCodeType.DHL)
            {
                carrierCode = "DHL Global Mail";
            }

            return carrierCode;
        }
    }
}
