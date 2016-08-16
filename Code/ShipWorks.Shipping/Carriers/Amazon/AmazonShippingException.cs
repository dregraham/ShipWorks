using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Base class for AmazonShipperExceptions
    /// </summary>
    [Serializable]
    public class AmazonShippingException : ShippingException
    {
        private string message;
        private readonly IDictionary<string, string> errorTranslation = new Dictionary<string, string> {
            { "insufficientfunds",
                "The requested shipping label purchase was rejected because of insufficient funds in the seller's account." },
            { "invalidrequest",
                "Request has missing or invalid parameters and cannot be parsed." },
            { "invalidshipfromaddress",
                "The specified Ship From Address is invalid. Specify a valid address." },
            { "invalidshippingserviceofferid",
                "The specified ShippingServiceOfferId value is invalid." },
            { "labelcancelwindowexpired",
                "The cancellation window for requesting a label refund has expired. Cancellation policies vary by carrier. " +
                "For more information about carrier cancellation policies, see the Seller Central Help." },
            { "shipmentalreadyexists",
                "One or more items specified in a call to the CreateShipment operation have already shipped. Specify only unshipped items." },
            { "shipmentrequestdetailstoorestrictive",
                "The specified ShipmentRequestDetails and ShippingServiceId values are so restrictive that no shipping service offer is available that can fulfill the request." },
            { "shippingserviceoffernotavailable",
                "The specified ShippingServiceOfferId value is no longer valid." },
            { "termsandconditionsnotaccepted",
                "The seller has not yet agreed to Amazon's or the carrier's terms and conditions. You can accept terms and condition in Seller Central." },
            { "invalidstate",
                "The request cannot be applied to the shipment in its current state (for example, a shipment in the RefundApplied state cannot be canceled)." },
            { "itemsnotinorder",
                "Items specified in a call to the CreateShipment operation are not part of the order specified in the same call." },
            { "regionnotsupported",
                "The order specified is from a marketplace where the Merchant Fulfillment API section is not supported." },
            { "shippingservicenotavailable",
                "The shipping service specified does not exist or is not available for the specified parameters (for example, Weight)." },
            { "resourcenotfound",
                "The resource specified (such as ShipmentId or AmazonOrderId) does not exist." }
        };

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
        /// Constructor
        /// </summary>
        protected AmazonShippingException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        { }

        /// <summary>
        /// Gets the error code
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
                string lowerCaseCode = Code?.ToLower();
                return lowerCaseCode != null && errorTranslation.ContainsKey(lowerCaseCode) ?
                    errorTranslation[lowerCaseCode] :
                    message;
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
            string replacedError = error.Trim().Replace("shipmentRequestDetails.", "")
                .Replace("packageDimensions.", "")
                .Replace(".value", "")
                .Replace("Value '0' at", "")
                .Replace("Value null at", "")
                .Replace("shipFromAddress.", "Ship from address ")
                .Replace("addressLine1", "line1")
                .Replace("addressLine2", "line2")
                .Replace("addressLine3", "line3")
                .Replace(
                    "failed to satisfy constraint: Member must have value greater than or equal to 0.001", "must be greater than 0.")
                .Replace(
                    "failed to satisfy constraint: Member must not be null", "cannot be blank.")
                .Replace("'", "")
                .Trim();

            return char.ToUpper(replacedError[0]) + replacedError.Substring(1);
        }
    }
}