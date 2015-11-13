using System;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Base class for AmazonShipperExceptions
    /// </summary>
    [Serializable]
    public class AmazonShippingException : ShippingException
    {
        private string message;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonShippingException()
            : this(null, null, null)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonShippingException(string message)
            : this(message, null, null)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonShippingException(string message, Exception ex)
            : this(message, null, ex)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonShippingException(string message, string code)
            : this(message, code, null)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonShippingException(string message, string code, Exception innerException)
            : base(message, innerException)
        {
            this.message = message;
            Code = code;
        }

        /// <summary>
        /// Gets the erro code
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Checks
        /// </summary>
        public override string Message
        {
            get
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    message = Code;
                }

                if (message.Contains("validation error"))
                {
                    return TransformValidationErrors(message);
                }

                // Sometimes Amazon does not give us an error message
                // If the error message is empty then we will provide
                // our own message based on the code.
                switch (Code?.ToLower())
                {
                    case "insufficientfunds":
                        return "The requested shipping label purchase was rejected because of insufficient funds in the seller's account.";
                    case "invalidrequest":
                        return "Request has missing or invalid parameters and cannot be parsed.";
                    case "invalidshipfromaddress":
                        return "The specified Ship From Address is invalid. Specify a valid address.";
                    case "invalidshippingserviceofferid":
                        return "The specified ShippingServiceOfferId value is invalid.";
                    case "labelcancelwindowexpired":
                        return "The cancellation window for requesting a label refund has expired. Cancellation policies vary by carrier. For more information about carrier cancellation policies, see the Seller Central Help.";
                    case "shipmentalreadyexists":
                        return "One or more items specified in a call to the CreateShipment operation have already shipped. Specify only unshipped items.";
                    case "shipmentrequestdetailstoorestrictive":
                        return "The specified ShipmentRequestDetails and ShippingServiceId values are so restrictive that no shipping service offer is available that can fulfill the request.";
                    case "shippingserviceoffernotavailable":
                        return "The specified ShippingServiceOfferId value is no longer valid.";
                    case "termsandconditionsnotaccepted":
                        return "The seller has not yet agreed to Amazon's or the carrier's terms and conditions. You can accept terms and condition in Seller Central.";
                    case "invalidstate":
                        return "The request cannot be applied to the shipment in its current state (for example, a shipment in the RefundApplied state cannot be canceled).";
                    case "itemsnotinorder":
                        return "Items specified in a call to the CreateShipment operation are not part of the order specified in the same call.";
                    case "regionnotsupported":
                        return "The order specified is from a marketplace where the Merchant Fulfillment API section is not supported.";
                    case "shippingservicenotavailable":
                        return "The shipping service specified does not exist or is not available for the specified parameters (for example, Weight).";
                    default:
                        return message;
                }
            }
        }

        /// <summary>
        /// Transforms a list of validation errors
        /// </summary>
        private static string TransformValidationErrors(string message)
        {
            string errors = message.Substring(message.IndexOf(':') + 1);

            string[] errorArray = errors.Split(';');

            return errorArray.Aggregate("", (current, error) => current + $"{TransformError(error)} \n");
        }

        /// <summary>
        /// Transforms error into more readable error
        /// </summary>
        private static string TransformError(string error)
        {
            error = error.Replace("shipmentRequestDetails.", "");
            error = error.Replace("packageDimensions.", "");
            error = error.Replace(".value", "");
            error = error.Replace("Value '0' at", "");
            error = error.Replace("Value null at", "");
            error = error.Replace("shipFromAddress.", "Ship from address ");

            error = error.Replace("addressLine1", "line1");
            error = error.Replace("addressLine2", "line2");
            error = error.Replace("addressLine3", "line3");

            error = error.Replace(
                "failed to satisfy constraint: Member must have value greater than or equal to 0.001", "must be greater than 0.");
            error = error.Replace(
                "failed to satisfy constraint: Member must not be null", "cannnot be blank.");

            error = error.Replace("'", "");

            error = error.Trim();

            return char.ToUpper(error[0]) + error.Substring(1).Trim();
        }
    }
}