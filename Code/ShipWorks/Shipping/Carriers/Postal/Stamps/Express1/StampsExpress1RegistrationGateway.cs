using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// Gateway class for integrating with Express1 and Stamps
    /// </summary>
    public class StampsExpress1RegistrationGateway : Express1RegistrationGateway
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StampsExpress1RegistrationGateway()
            : base(new Express1StampsConnectionDetails())
        {
        }

        /// <summary>
        /// Verifies that the specified username and password map to a valid account
        /// </summary>
        /// <param name="registration">Registration that defines the account to test</param>
        /// <param name="errors">List into which verification errors should be placed</param>
        /// <returns>True if the account is valid, false otherwise</returns>
        public override void VerifyAccount(Express1Registration registration, ICollection<Express1ValidationError> errors)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }

            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }

            try
            {
                StampsApiSession.GetAccountInfo(new StampsAccountEntity
                {
                    Username = registration.UserName,
                    Password = registration.Password
                });
            }
            catch (StampsException ex)
            {
                errors.Add(new Express1ValidationError("ShipWorks was unable to communicate with Express1 using this account information:\n\n" + ex.Message));
            }
        }
    }
}
