using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Authorization;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// An implementation of the ICompleteSaleRequest interface that is responsible for
    /// making requests to eBay to mark an item/transaction as paid and/or shipped.
    /// </summary>
    public class EbayCompleteSaleRequest : EbayRequest, ICompleteSaleRequest
    {
        private CompleteSaleRequestType request;


        /// <summary>
        /// Initializes a new instance of the <see cref="EbayCompleteSaleRequest"/> class.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        public EbayCompleteSaleRequest(TokenData tokenData)
            : base(tokenData, "CompleteSale")
        {
            request = new CompleteSaleRequestType()
            {
                Shipment = new ShipmentType()
            };
        }

        /// <summary>
        /// Completes the sale for the given item/transaction.
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="isPaid">if set to <c>true</c> [is paid].</param>
        /// <param name="isShipped">if set to <c>true</c> [is shipped].</param>
        /// <param name="trackingNumber">The tracking number.</param>
        /// <param name="shippingCarrier">The carrier.</param>
        /// <returns>An CompleteSaleResponseType object.</returns>
        public CompleteSaleResponseType CompleteSale(string itemId, string transactionId, bool? isPaid, bool? isShipped, string trackingNumber, string shippingCarrier)
        {
            request.ItemID = itemId;
            request.TransactionID = transactionId;

            if (isShipped.HasValue)
            {
                // Only set the Shipped property if we have a value; otherwise we want it
                // to remain unchanged in eBay
                request.Shipped = isShipped.Value;
                request.ShippedSpecified = true;
            }

            if (isPaid.HasValue)
            {
                // Only set the Paid property if we have a value; otherwise we want it
                // to remain unchanged in eBay
                request.Paid = isPaid.Value;
                request.PaidSpecified = true;
            }

            ShipmentTrackingDetailsType trackingDetails = new ShipmentTrackingDetailsType();
            trackingDetails.ShipmentTrackingNumber = trackingNumber;
            trackingDetails.ShippingCarrierUsed = shippingCarrier.ToString();

            request.Shipment.ShipmentTrackingDetails = new ShipmentTrackingDetailsType[] { trackingDetails };

            CompleteSaleResponseType response = SubmitRequest() as CompleteSaleResponseType;
            if (response == null)
            {
                throw new EbayException("Unable to complete the sale with eBay.");
            }

            return response;
        }

        /// <summary>
        /// Gets the name of the call as it is known to eBay. This value gets used
        /// as a query string parameter sent to eBay.
        /// </summary>
        /// <returns></returns>
        public override string GetEbayCallName()
        {
            return "CompleteSale";
        }

        /// <summary>
        /// Gets the specific eBay request (GetUserRequestType, GetOrdersRequestType, etc.) containing any parameter data necessary to make the request.
        /// </summary>
        /// <returns>An CompleteSaleRequestType object.</returns>
        public override AbstractRequestType GetEbayRequest()
        {
            return request;
        }
    }
}
