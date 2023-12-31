using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration.Payment;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Registration
{
    /// <summary>
    /// An interface for being able to validate payment information.
    /// </summary>
    public interface IExpress1PaymentValidator
    {
        /// <summary>
        /// Validates payment information.
        /// </summary>
        /// <returns>A collection of Express1ValidationError objects. An empty collection indicates
        /// the payment information is valid; a non-empty collection indicates that the payment info
        /// did not pass validation.</returns>
        IEnumerable<Express1ValidationError> ValidatePaymentInfo(Express1PaymentInfo paymentInfo);
    }
}
