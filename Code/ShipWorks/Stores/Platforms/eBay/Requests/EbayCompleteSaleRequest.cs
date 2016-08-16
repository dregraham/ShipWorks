using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// Implements the eBay CompleteSale API
    /// </summary>
    public class EbayCompleteSaleRequest : EbayRequest<CompleteSaleResponseType, CompleteSaleRequestType, CompleteSaleResponseType>
    {
        CompleteSaleRequestType request;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayCompleteSaleRequest"/> class.
        /// </summary>
        [NDependIgnoreTooManyParams]
        public EbayCompleteSaleRequest(EbayToken token, long itemID, long transactionID, bool? isPaid, bool? isShipped, string trackingNumber, string shippingCarrier)
            : base(token, "CompleteSale")
        {
            request = new CompleteSaleRequestType();

            request.ItemID = itemID.ToString();
            request.TransactionID = transactionID.ToString();

            // Only set the Shipped property if we have a value; otherwise we want it to remain unchanged in eBay
            if (isShipped.HasValue)
            {
                request.Shipped = isShipped.Value;
                request.ShippedSpecified = true;
            }

            // Only set the Paid property if we have a value; otherwise we want it to remain unchanged in eBay
            if (isPaid.HasValue)
            {
                request.Paid = isPaid.Value;
                request.PaidSpecified = true;
            }

            ShipmentTrackingDetailsType trackingDetails = new ShipmentTrackingDetailsType();
            trackingDetails.ShipmentTrackingNumber = trackingNumber;
            trackingDetails.ShippingCarrierUsed = shippingCarrier;

            request.Shipment = new ShipmentType();
            request.Shipment.ShipmentTrackingDetails = new ShipmentTrackingDetailsType[] { trackingDetails };
        }

        /// <summary>
        /// Completes the sale for the given item/transaction.
        /// </summary>
        public override CompleteSaleResponseType Execute()
        {
            return SubmitRequest();
        }

        /// <summary>
        /// Gets the specific eBay request (GetUserRequestType, GetOrdersRequestType, etc.) containing any parameter data necessary to make the request.
        /// </summary>
        protected override AbstractRequestType CreateRequest()
        {
            return request;
        }
    }
}
