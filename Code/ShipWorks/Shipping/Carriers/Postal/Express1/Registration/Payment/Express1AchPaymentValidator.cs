using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Registration.Payment
{
    /// <summary>
    /// Validates that the ACH info on an Express1PaymentInfo is valid
    /// </summary>
    public class Express1AchPaymentValidator : IExpress1PaymentValidator
    {
        public const string InvalidPaymentTypeCreditCard = "ShipWorks is unable to validate ACH payment info because the payment type is currently Credit Card.";
        public const string InvalidAchAccountNumber = "The ACH account number is required.";
        public const string InvalidAchAccountHolderName = "The ACH account holder name is required.";
        public const string InvalidAchBankName = "The ACH bank name is required.";
        public const string InvalidAchRoutingId = "The ACH routing number is required.";
        public const string InvalidAchAccountName = "The ACH account name is required.";

        /// <summary>
        /// Validates payment information.
        /// </summary>
        /// <returns>A collection of Express1ValidationError objects. An empty collection indicates
        /// the payment information is valid; a non-empty collection indicates that the payment info
        /// did not pass validation.</returns>
        public IEnumerable<Express1ValidationError> ValidatePaymentInfo(Express1PaymentInfo paymentInfo)
        {
            List<Express1ValidationError> validationErrors = new List<Express1ValidationError>();

            if (paymentInfo.PaymentType == Express1PaymentType.CreditCard)
            {
                validationErrors.Add(new Express1ValidationError(InvalidPaymentTypeCreditCard));
                return validationErrors;
            }

            validationErrors.AddRange(Express1Utilities.ValidateDataIsProvided(paymentInfo.AchAccountNumber, InvalidAchAccountNumber));
            validationErrors.AddRange(Express1Utilities.ValidateDataIsProvided(paymentInfo.AchAccountHolderName, InvalidAchAccountHolderName));
            validationErrors.AddRange(Express1Utilities.ValidateDataIsProvided(paymentInfo.AchBankName, InvalidAchBankName));
            validationErrors.AddRange(Express1Utilities.ValidateDataIsProvided(paymentInfo.AchRoutingId, InvalidAchRoutingId));
            validationErrors.AddRange(Express1Utilities.ValidateDataIsProvided(paymentInfo.AchAccountName, InvalidAchAccountName));

            return validationErrors;
        }
    }
}
