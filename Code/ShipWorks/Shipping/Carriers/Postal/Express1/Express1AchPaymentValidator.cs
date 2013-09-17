using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Validates that the ACH info on a Express1PaymentInfo is valid
    /// </summary>
    public class Express1AchPaymentValidator : IExpress1PaymentValidator
    {
        private readonly List<Express1ValidationError> validationErrors;

        /// <summary>
        /// Constructor that accepts an Express1PaymentInfo
        /// </summary>
        public Express1AchPaymentValidator()
        {
            validationErrors = new List<Express1ValidationError>();
        }

        /// <summary>
        /// Validates payment information.
        /// </summary>
        /// <returns>A collection of Express1ValidationError objects. An empty collection indicates
        /// the payment information is valid; a non-empty collection indicates that the payment info
        /// did not pass validation.</returns>
        public IEnumerable<Express1ValidationError> ValidatePaymentInfo(Express1PaymentInfo paymentInfo)
        {
            if (paymentInfo.PaymentType == Express1PaymentType.CreditCard)
            {
                validationErrors.Add(new Express1ValidationError(Express1ValidationErrorMessages.InvalidPaymentTypeCreditCard));
                return validationErrors;
            }

            validationErrors.AddRange(Express1Utilities.ValidateDataIsProvided(paymentInfo.AchAccountNumber, Express1ValidationErrorMessages.InvalidAchAccountNumber));
            validationErrors.AddRange(Express1Utilities.ValidateDataIsProvided(paymentInfo.AchAccountHolderName, Express1ValidationErrorMessages.InvalidAchAccountHolderName));
            validationErrors.AddRange(Express1Utilities.ValidateDataIsProvided(paymentInfo.AchBankName, Express1ValidationErrorMessages.InvalidAchBankName));
            validationErrors.AddRange(Express1Utilities.ValidateDataIsProvided(paymentInfo.AchRoutingId, Express1ValidationErrorMessages.InvalidAchRoutingId));
            validationErrors.AddRange(Express1Utilities.ValidateDataIsProvided(paymentInfo.AchAccountName, Express1ValidationErrorMessages.InvalidAchAccountName));

            return validationErrors;
        }
    }
}
