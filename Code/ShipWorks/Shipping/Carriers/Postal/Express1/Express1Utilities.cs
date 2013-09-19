using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Helper methods for Express1
    /// </summary>
    public static class Express1Utilities
    {
        /// <summary>
        /// Simple validation that just checks that the string is not empty.
        /// </summary>
        /// <param name="registrationField">The value to check.</param>
        /// <param name="validationMessage">The validation error message.</param>
        /// <returns>A List of Express1ValidationError objects.</returns>
        public static IEnumerable<Express1ValidationError> ValidateDataIsProvided(string registrationField, string validationMessage)
        {
            List<Express1ValidationError> errors = new List<Express1ValidationError>();

            if (string.IsNullOrWhiteSpace(registrationField))
            {
                errors.Add(new Express1ValidationError(validationMessage));
            }

            return errors;
        }

        /// <summary>
        /// Indicates if the postal service method for the given shipment would be cheaper for the customer if using Express1
        /// </summary>
        public static bool IsPostageSavingService(ShipmentEntity shipment)
        {
            return IsPostageSavingService((PostalServiceType)shipment.Postal.Service);
        }

        /// <summary>
        /// Returns if the given postal service was used for the given shipment (the shipments service is ignored) 
        /// </summary>
        public static bool IsPostageSavingService(PostalServiceType postalServiceType)
        {
            // There are domestic zones that arent cheaper, but for now we are simplifying
            switch (postalServiceType)
            {
                case PostalServiceType.PriorityMail:
                case PostalServiceType.ExpressMail:
                case PostalServiceType.ExpressMailPremium:
                case PostalServiceType.InternationalPriority:
                case PostalServiceType.InternationalExpress:
                    return true;
            }

            return false;
        }
    }
}
