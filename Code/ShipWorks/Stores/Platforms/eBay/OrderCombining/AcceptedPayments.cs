using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.OrderCombining
{
    /// <summary>
    /// Utility for parsing the AcceptedPaymentsList csv on eBay stores
    /// that determines what payment types to accept when creating Combined Payments
    /// on eBay.
    /// </summary>
    public static class AcceptedPayments
    {
        /// <summary>
        /// Gets the display string for the eBay payment method types
        /// </summary>
        public static string GetBuyerPaymentMethodString(BuyerPaymentMethodCodeType code)
        {
            switch (code)
            {
                case BuyerPaymentMethodCodeType.AmEx: return "American Express";
                case BuyerPaymentMethodCodeType.CashInPerson: return "Cash in Person";
                case BuyerPaymentMethodCodeType.CashOnPickup: return "Cash on Pickup";
                case BuyerPaymentMethodCodeType.CCAccepted: return "Credit Card";
                case BuyerPaymentMethodCodeType.COD: return "Cash on Delivery";
                case BuyerPaymentMethodCodeType.Discover: return "Discover";
                case BuyerPaymentMethodCodeType.MOCC: return "Money Order/Cashier Check";
                case BuyerPaymentMethodCodeType.MoneyXferAccepted: return "Direct Transfer";
                case BuyerPaymentMethodCodeType.Other: return "Other";
                case BuyerPaymentMethodCodeType.OtherOnlinePayments: return "Other Online Payments";
                case BuyerPaymentMethodCodeType.Paymate: return "Paymate";
                case BuyerPaymentMethodCodeType.PaymentSeeDescription: return "See Item Description";
                case BuyerPaymentMethodCodeType.PayPal: return "PayPal";
                case BuyerPaymentMethodCodeType.PersonalCheck: return "Personal Check";
                case BuyerPaymentMethodCodeType.VisaMC: return "Visa/Mastercard";

                default:
                    return "Unknown";
            }
        }

        /// <summary>
        /// Parses the value we store in the database for the Accepted Payments 
        /// </summary>
        public static List<BuyerPaymentMethodCodeType> ParseList(string acceptedPaymentsList)
        {
            List<BuyerPaymentMethodCodeType> paymentMethods = new List<BuyerPaymentMethodCodeType>();
            foreach (string piece in acceptedPaymentsList.Split(','))
            {
                int paymentMethod = 0;
                if (int.TryParse(piece, out paymentMethod))
                {
                    paymentMethods.Add((BuyerPaymentMethodCodeType)paymentMethod);
                }

            }

            return paymentMethods;
        }

        /// <summary>
        /// Creates the value serialized for AcceptedPaymentsList on the eBay store
        /// </summary>
        public static string AssembleValue(List<BuyerPaymentMethodCodeType> acceptedPayments)
        {
            return String.Join(",", acceptedPayments.ConvertAll(p => ((int)p).ToString()).ToArray());
        }
    }
}
