using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using System.Linq;
using Interapptive.Shared.Security;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Provides security around payment details
    /// </summary>
    public static class PaymentDetailSecurity
    {
        /// <summary>
        /// Protect the specified payment detail
        /// </summary>
        public static void Protect(OrderPaymentDetailEntity detail)
        {
            if (IsCreditCardNumber(detail) || IsCcv(detail))
            {
                detail.Value = SecureText.Encrypt(detail.Value, detail.Label);
            }
        }

        /// <summary>
        /// Read the Value property from the previously protected payment detail
        /// </summary>
        public static string ReadValue(OrderPaymentDetailEntity detail)
        {
            if (IsCreditCardNumber(detail) || IsCcv(detail))
            {
                string text = SecureText.Decrypt(detail.Value, detail.Label);

                if (UserSession.Security.HasPermission(PermissionType.OrdersViewPaymentData, detail.OrderPaymentDetailID))
                {
                    return text;
                }
                else
                {
                    return MaskCreditCardNumber(text);
                }
            }

            return detail.Value;
        }

        /// <summary>
        /// Determines if the given payment detail is a CC Verification number
        /// </summary>
        public static bool IsCcv(OrderPaymentDetailEntity detail)
        {
            if (detail == null)
            {
                throw new ArgumentNullException("detail");
            }

            if (detail.Label.StartsWith("CCV", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determins if the given payment detail is a CC number
        /// </summary>
        public static bool IsCreditCardNumber(OrderPaymentDetailEntity detail)
        {
            if (detail == null)
            {
                throw new ArgumentNullException("detail");
            }

            if (detail.Label.StartsWith("cc_number", StringComparison.OrdinalIgnoreCase) ||
                detail.Label.StartsWith("card_num", StringComparison.OrdinalIgnoreCase) ||
                detail.Label.StartsWith("cardnum", StringComparison.OrdinalIgnoreCase) ||
                detail.Label.StartsWith("Card Number", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Indicates if the given detail value represents an expiration date
        /// </summary>
        public static bool IsExpirationDate(OrderPaymentDetailEntity detail)
        {
            if (detail == null)
            {
                throw new ArgumentNullException("detail");
            }

            if (detail.Label.StartsWith("cc_exp", StringComparison.OrdinalIgnoreCase) ||
                detail.Label.StartsWith("exp_date", StringComparison.OrdinalIgnoreCase) ||
                detail.Label.StartsWith("expiration", StringComparison.OrdinalIgnoreCase) ||
                detail.Label.EndsWith("expires", StringComparison.OrdinalIgnoreCase) ||
                detail.Label.StartsWith("Card Expiration",StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Replace all but the last 4 characters with *
        /// </summary>
        public static string MaskCreditCardNumber(string number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }

            int length = number.Length;

            if (length > 4)
            {
                number = number.Substring(length - 4, 4);
            }

            return number.PadLeft(length, 'x');
        }

        /// <summary>
        /// Given order details, return the expiration date. Empty String if none found.
        /// </summary>
        public static string GetExpiration(List<OrderPaymentDetailEntity> details)
        {
            var detail = details.FirstOrDefault(d => PaymentDetailSecurity.IsExpirationDate(d));
            if (detail != null)
            {
                return detail.Value;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Retrieves the credit card number from the detail set (if any)
        /// </summary>
        public static string GetCreditCardNumber(List<OrderPaymentDetailEntity> details)
        {
            var detail = details.FirstOrDefault(d => PaymentDetailSecurity.IsCreditCardNumber(d));
            if (detail != null)
            {
                return PaymentDetailSecurity.ReadValue(detail);
            }
            else
            {
                return "";
            }
        }
    }
}
