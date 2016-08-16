using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using System.Text.RegularExpressions;
using Interapptive.Shared;
using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Ebay.Enums;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Class for retrieiving human readable text for eBay enums
    /// </summary>
    public static class EbayUtility
    {
        // Maps the ebay ShippingService "token" string to a usable string
        static Dictionary<string, string> shippingMethods = new Dictionary<string, string>();

        // Used to extract shipping method name from ebay shipping code
        static Regex shipMethodRegex = new Regex("[A-Z0-9][a-z]+", RegexOptions.Compiled);

        // Salt for descrypting sandbox credentials
        static string sandboxCredentialSaltValue = "apptive";

        /// <summary>
        /// Static constructor
        /// </summary>
        [NDependIgnoreLongMethod]
        static EbayUtility()
        {
            shippingMethods.Add("NotSelected", "");
            shippingMethods.Add("InternationalNotSelected", "");

            shippingMethods.Add("Other", "Other (see description)");
            shippingMethods.Add("OtherInternational", "Other Intl (see description)");

            shippingMethods.Add("ShippingMethodStandard", "Standard Flat Rate");
            shippingMethods.Add("ShippingMethodExpress", "Expedited Flat Rate");
            shippingMethods.Add("CustomCode", "Unknown Service");

            shippingMethods.Add("LocalDelivery", "Local Delivery/Pickup");
            shippingMethods.Add("FreightShipping", "Freight Shipping");
            shippingMethods.Add("FreightShippingInternational", "Freight Shipping International");
            shippingMethods.Add("ShippingMethodOvernight", "Overnight Flat Rate");
            shippingMethods.Add("StandardInternational", "Standard Intl Flat Rate");
            shippingMethods.Add("ExpeditedInternational", "Expedited Intl Flat Rate");

            shippingMethods.Add("UPSGround", "UPS Ground");
            shippingMethods.Add("UPS3rdDay", "UPS 3rd Day");
            shippingMethods.Add("UPS2ndDay", "UPS 2nd Day");
            shippingMethods.Add("UPSNextDay", "UPS Next Day");
            shippingMethods.Add("UPSNextDayAir", "UPS Next Day Air");
            shippingMethods.Add("UPS2DayAirAM", "UPS Next Day Air");
            shippingMethods.Add("UPSWorldWideExpressPlus", "UPS Worldwide Express Plus");
            shippingMethods.Add("UPSWorldWideExpress", "UPS Worldwide Express");
            shippingMethods.Add("UPSWorldWideExpedited", "UPS Worldwide Expedited");
            shippingMethods.Add("UPSStandardToCanada", "UPS Standard To Canada");
            shippingMethods.Add("UPSWorldWideExpressPlusBox10kg", "UPS Worldwide Express Plus (10Kg Box)");
            shippingMethods.Add("UPSWorldWideExpressPlusBox25kg", "UPS Worldwide Express Plus (25Kg Box)");
            shippingMethods.Add("UPSWorldWideExpressBox10kg", "UPS Worldwide Express (10Kg Box)");
            shippingMethods.Add("UPSWorldWideExpressBox25kg", "UPS Worldwide Express (25Kg Box)");

            shippingMethods.Add("USPSPriority", "USPS Priority");
            shippingMethods.Add("USPSParcel", "USPS Parcel");
            shippingMethods.Add("USPSMedia", "USPS Media");
            shippingMethods.Add("USPSFirstClass", "USPS First Class");
            shippingMethods.Add("USPSExpressMail", "USPS Express Mail");
            shippingMethods.Add("USPSGlobalExpress", "USPS Global Express Mail");
            shippingMethods.Add("USPSGlobalPriority", "USPS Global Priority Mail");
            shippingMethods.Add("USPSEconomyParcel", "USPS Economy Parcel Post");
            shippingMethods.Add("USPSEconomyLetter", "USPS Economy Letter Post");
            shippingMethods.Add("USPSAirmailLetter", "USPS Airmail Letter Post");
            shippingMethods.Add("USPSAirmailParcel", "USPS Airmail Parcel Post");
            shippingMethods.Add("USPSGround", "USPS Ground");
            shippingMethods.Add("USPSExpressFlatRateEnvelope", "USPS Express Flat Rate Envelope");
            shippingMethods.Add("USPSGlobalPriorityLargeEnvelope", "USPS Global Priority Large Envelope");
            shippingMethods.Add("USPSGlobalPrioritySmallEnvelope", "USPS Global Priority Small Envelope");
            shippingMethods.Add("USPSPriorityFlatRateBox", "USPS Priority Flat Rate Box");
            shippingMethods.Add("USPSPriorityFlatRateEnvelope", "USPS Priority Flat Rate Envelope");
        }

        /// <summary>
        /// Gets the sandbox developer credential.
        /// </summary>
        public static string SandboxDeveloperCredential
        {
            get { return SecureText.Decrypt("9EjIiY68ZC9AmdbCllBY3u7iAPOS7JnSlecbcc8Jf80=", sandboxCredentialSaltValue); }
        }

        /// <summary>
        /// Gets the sandbox application credential.
        /// </summary>
        public static string SandboxApplicationCredential
        {
            get { return SecureText.Decrypt("d0t5tDPECtuYnbRi9LAXAsLrbVXGyhsVAW3Jo3giRNY=", sandboxCredentialSaltValue); }
        }

        /// <summary>
        /// Gets the sandbox certificate credential.
        /// </summary>
        public static string SandboxCertificateCredential
        {
            get { return SecureText.Decrypt("Z9/F10grv8Vkkz/UU1Zk4I26AJ6ZBA2EdcCSgQMbUvM=", sandboxCredentialSaltValue); }
        }

        /// <summary>
        /// KeyValuePairs of eBay shipping method codes and human displayable text
        /// </summary>
        public static List<KeyValuePair<string, string>> ShippingMethods
        {
            get { return shippingMethods.ToList(); }
        }

        /// <summary>
        /// The payment method used to pay for the transaction
        /// </summary>
        public static EbayEffectivePaymentMethod GetEffectivePaymentMethod(EbayOrderItemEntity orderItem)
        {
            BuyerPaymentMethodCodeType paymentMethod = (BuyerPaymentMethodCodeType) orderItem.PaymentMethod;
            CompleteStatusCodeType completeStatus = (CompleteStatusCodeType) orderItem.CompleteStatus;

            return GetEffectivePaymentMethod(paymentMethod, completeStatus);
        }

        /// <summary>
        /// The payment method used to pay for the transaction
        /// </summary>
        public static EbayEffectivePaymentMethod GetEffectivePaymentMethod(BuyerPaymentMethodCodeType paymentMethod, CompleteStatusCodeType completeStatus)
        {
            // If its paypal then regardless of completestatus we can return paypal
            if (paymentMethod == BuyerPaymentMethodCodeType.PayPal)
            {
                return EbayEffectivePaymentMethod.PayPal;
            }

             // If checkout is not complete, then payment method is not chosen
            if (completeStatus == CompleteStatusCodeType.Incomplete)
            {
                return EbayEffectivePaymentMethod.NotChosen;
            }

            // Checkout is complete, so we return the specified payment method
            switch (paymentMethod)
            {
                case BuyerPaymentMethodCodeType.MOCC: return EbayEffectivePaymentMethod.MoneyOrderOrCashiersCheck;
                case BuyerPaymentMethodCodeType.PersonalCheck: return EbayEffectivePaymentMethod.PersonalCheck;
                case BuyerPaymentMethodCodeType.COD: return EbayEffectivePaymentMethod.Cod;
                case BuyerPaymentMethodCodeType.Other: return EbayEffectivePaymentMethod.Other;
                case BuyerPaymentMethodCodeType.PayPal: return EbayEffectivePaymentMethod.PayPal;
                case BuyerPaymentMethodCodeType.None: return EbayEffectivePaymentMethod.NotSpecified;
                case BuyerPaymentMethodCodeType.MoneyXferAccepted: return EbayEffectivePaymentMethod.MoneyTransferCip;
                case BuyerPaymentMethodCodeType.CashOnPickup: return EbayEffectivePaymentMethod.CashOnPickup;
                case BuyerPaymentMethodCodeType.VisaMC: return EbayEffectivePaymentMethod.VisaMastercard;
                case BuyerPaymentMethodCodeType.AmEx: return EbayEffectivePaymentMethod.AmericanExpress;
                case BuyerPaymentMethodCodeType.Discover: return EbayEffectivePaymentMethod.DiscoverCard;
                case BuyerPaymentMethodCodeType.PaymentSeeDescription: return EbayEffectivePaymentMethod.SeeDescription;
                case BuyerPaymentMethodCodeType.CCAccepted: return EbayEffectivePaymentMethod.CreditCard;
                case BuyerPaymentMethodCodeType.OtherOnlinePayments: return EbayEffectivePaymentMethod.OnlinePayment;

                default: return EbayEffectivePaymentMethod.Unknown;
            }
        }

        /// <summary>
        /// Get effective payment status of the given item
        /// </summary>
        public static EbayEffectivePaymentStatus GetEffectivePaymentStatus(EbayOrderItemEntity orderItem)
        {
            CompleteStatusCodeType completeStatus = (CompleteStatusCodeType) orderItem.CompleteStatus;
            PaymentStatusCodeType paymentStatus = (PaymentStatusCodeType) orderItem.PaymentStatus;

            // If there was any type of failure, just return the generic failure type
            if (paymentStatus == PaymentStatusCodeType.BuyerECheckBounced ||
                paymentStatus == PaymentStatusCodeType.BuyerCreditCardFailed ||
                paymentStatus == PaymentStatusCodeType.BuyerFailedPaymentReportedBySeller)
            {
                return EbayEffectivePaymentStatus.Failed;
            }

            // If its paypal processing, thats what we display
            if (paymentStatus == PaymentStatusCodeType.PayPalPaymentInProcess)
            {
                return EbayEffectivePaymentStatus.PaymentPendingPayPal;
            }

            // If it's marked as paid in My eBay, consider that paid as the seller manually set that
            if (orderItem.MyEbayPaid)
            {
                return EbayEffectivePaymentStatus.Paid;
            }

            // Payment is in process
            if (paymentStatus == PaymentStatusCodeType.PaymentInProcess || completeStatus == CompleteStatusCodeType.Pending)
            {
                return EbayEffectivePaymentStatus.PaymentPending;
            }

            return EbayEffectivePaymentStatus.Incomplete;
        }

        /// <summary>
        /// Determines if the item represents a completed checkout status state
        /// </summary>
        public static bool IsCheckoutStatusComplete(EbayOrderItemEntity orderItem)
        {
            if (orderItem.Order.IsManual)
            {
                return true;
            }

            return orderItem.EffectiveCheckoutStatus == (int) EbayEffectivePaymentStatus.Paid;
        }

        /// <summary>
        /// Returns true if the given status type indicates the auction is paid
        /// </summary>
        public static bool IsPaidStatusPaid(PaidStatusCodeType paidStatus)
        {
            switch (paidStatus)
            {
                case PaidStatusCodeType.MarkedAsPaid:
                case PaidStatusCodeType.PaidWithEscrow:
                case PaidStatusCodeType.PaidWithPaisaPay:
                case PaidStatusCodeType.PaidWithPayPal:
                case PaidStatusCodeType.PaidCOD:
                case PaidStatusCodeType.PaidWithPaisaPayEscrow:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a string for the type of feedback left. (Positive, Negative, Neutral, or Unknown)
        /// </summary>
        public static string GetFeedbackTypeString(CommentTypeCodeType feedbackType)
        {
            if (feedbackType == CommentTypeCodeType.Positive) return "Positive";
            if (feedbackType == CommentTypeCodeType.Negative) return "Negative";
            if (feedbackType == CommentTypeCodeType.Neutral) return "Neutral";
            if (feedbackType == CommentTypeCodeType.Withdrawn) return "Withdrawn";

            return "Unknown";
        }

        /// <summary>
        /// Get the shipment method name for the given ebay code.
        /// </summary>
        public static string GetShipmentMethodName(string method)
        {
            if (string.IsNullOrEmpty(method))
            {
                return "";
            }

            string value = "";
            if (shippingMethods.TryGetValue(method, out value))
            {
                return value;
            }

            return shipMethodRegex.Replace(method, " $0");
        }

        /// <summary>
        /// Get the name of the given PayPal address status.
        /// </summary>
        public static string GetAddressStatusName(AddressStatusCodeType status)
        {
            switch (status)
            {
                case AddressStatusCodeType.None: return "";
                case AddressStatusCodeType.Unconfirmed: return "Unconfirmed";
                case AddressStatusCodeType.Confirmed: return "Confirmed";
            }

            return "Unknown";
        }

        /// <summary>
        /// Get the eBay API value represented by our own type
        /// </summary>
        public static QuestionTypeCodeType GetEbayQuestionTypeCode(EbaySendMessageType messageType)
        {
            switch (messageType)
            {
                case EbaySendMessageType.CustomCode:
                    return QuestionTypeCodeType.CustomCode;
                case EbaySendMessageType.CustomizedSubject:
                    return QuestionTypeCodeType.CustomizedSubject;
                case EbaySendMessageType.General:
                    return QuestionTypeCodeType.General;
                case EbaySendMessageType.MultipleItemShipping:
                    return QuestionTypeCodeType.MultipleItemShipping;
                case EbaySendMessageType.Payment:
                    return QuestionTypeCodeType.Payment;
                case EbaySendMessageType.Shipping:
                    return QuestionTypeCodeType.Shipping;
                default:
                    return QuestionTypeCodeType.None;
            }
        }

        /// <summary>
        /// Get the ShipWorks value represented by the ebay feedback (comment) type
        /// </summary>
        public static EbayFeedbackType GetShipWorksFeedbackType(CommentTypeCodeType commentType)
        {
            switch (commentType)
            {
                case CommentTypeCodeType.Positive: return EbayFeedbackType.Positive;
                case CommentTypeCodeType.Negative: return EbayFeedbackType.Negative;
                case CommentTypeCodeType.Neutral: return EbayFeedbackType.Neutral;
                case CommentTypeCodeType.Withdrawn: return EbayFeedbackType.Withdrawn;
                case CommentTypeCodeType.IndependentlyWithdrawn: return EbayFeedbackType.IndependentlyWithdrawn;
                case CommentTypeCodeType.CustomCode: return EbayFeedbackType.CustomCode;
            }

            throw new InvalidOperationException("Unhandled ebay comment type: " + commentType);
        }

        /// <summary>
        /// Get the display name for the given order status code
        /// </summary>
        public static string GetOrderStatusName(OrderStatusCodeType orderStatus)
        {
            switch (orderStatus)
            {
                case OrderStatusCodeType.Active: return "Active";
                case OrderStatusCodeType.Cancelled: return "Cancelled";
                case OrderStatusCodeType.Completed: return "Completed";
                case OrderStatusCodeType.Inactive: return "Inactive";
                case OrderStatusCodeType.Shipped: return "Shipped";
                case OrderStatusCodeType.CancelPending: return "Cancel Pending";
            }

            return "Invalid";
        }
    }
}
