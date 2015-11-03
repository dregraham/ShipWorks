using System;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Base class for AmazonShipperExceptions
    /// </summary>
    [Serializable]
    public class AmazonShippingException : ShippingException
    {
        private string message;

        public AmazonShippingException()
            : this(null, null, null)
        {}

        public AmazonShippingException(string message)
            : this(message, null, null)
        { }

        public AmazonShippingException(string message, Exception ex)
            : this(message, null, ex)
        { }

        public AmazonShippingException(string message, string code)
            : this(message, code, null)
        {}

        public AmazonShippingException(string message, string code, Exception innerException)
            : base(message, innerException)
        {
            this.message = message;
            Code = code;
        }

        public string Code { get; }

        /// <summary>
        /// Checks 
        /// </summary>
        public override string Message
        {
            get
            {
                // Sometimes Amazon does not give us an error message
                // If the error message is empty then we will provide 
                // our own message based on the code.
                if (Code != null && string.IsNullOrEmpty(message))
                {
                    switch (Code.ToLower())
                    {
                        case "insufficientfunds":
                            message = "The requested shipping label purchase was rejected because of insufficient funds in the seller's account.";
                            break;
                        case "invalidrequest":
                            message = "Request has missing or invalid parameters and cannot be parsed.";
                            break;
                        case "invalidshipfromaddress":
                            message = "The specified Ship From Address is invalid. Specify a valid address.";
                            break;
                        case "invalidshippingserviceofferid":
                            message = "The specified ShippingServiceOfferId value is invalid.";
                            break;
                        case "labelcancelwindowexpired":
                            message = "The cancellation window for requesting a label refund has expired. Cancellation policies vary by carrier. For more information about carrier cancellation policies, see the Seller Central Help.";
                            break;
                        case "shipmentalreadyexists":
                            message = "One or more items specified in a call to the CreateShipment operation have already shipped. Specify only unshipped items.";
                            break;
                        case "shipmentrequestdetailstoorestrictive":
                            message = "The specified ShipmentRequestDetails and ShippingServiceId values are so restrictive that no shipping service offer is available that can fulfill the request.";
                            break;
                        case "shippingserviceoffernotavailable":
                            message = "The specified ShippingServiceOfferId value is no longer valid.";
                            break;
                        case "termsandconditionsnotaccepted":
                            message = "The seller has not yet agreed to Amazon's or the carrier's terms and conditions. You can accept terms and condition in Seller Central.";
                            break;
                        case "invalidstate":
                            message = "The request cannot be applied to the shipment in its current state (for example, a shipment in the RefundApplied state cannot be canceled).";
                            break;
                        case "itemsnotinorder":
                            message = "Items specified in a call to the CreateShipment operation are not part of the order specified in the same call.";
                            break;
                        case "regionnotsupported":
                            message = "The order specified is from a marketplace where the Merchant Fulfillment API section is not supported.";
                            break;
                        case "shippingservicenotavailable":
                            message = "The shipping service specified does not exist or is not available for the specified parameters (for example, Weight).";
                            break;
                        default:
                            message = "";
                            break;
                    }
                }
                return message;
            }
        }
    }
}