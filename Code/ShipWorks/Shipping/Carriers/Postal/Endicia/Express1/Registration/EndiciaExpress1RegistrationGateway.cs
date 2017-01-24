using System;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.Registration
{
    /// <summary>
    /// An Express1RegistrationGateway implementation for integrating the Endicia API client into the Express1 registration process.
    /// </summary>
    [Component(RegistrationType.Self)]
    public class EndiciaExpress1RegistrationGateway : Express1RegistrationGateway
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaExpress1RegistrationGateway"/> class.
        /// </summary>
        public EndiciaExpress1RegistrationGateway()
            : base(new Express1EndiciaConnectionDetails())
        { }

        /// <summary>
        /// Verifies that the specified user name and password map to a valid account
        /// </summary>
        /// <param name="registration">Registration that defines the account to test</param>
        public override void VerifyAccount(Express1Registration registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }

            try
            {
                // Pull out the info from the registration to bounce a request off of the
                // Endicia API client to determine if the account credentials are valid
                EndiciaAccountEntity account = new EndiciaAccountEntity
                {
                    EndiciaReseller = (int) EndiciaReseller.Express1,
                    AccountNumber = registration.UserName,
                    ApiUserPassword = registration.EncryptedPassword
                };

                (new EndiciaApiClient()).GetAccountStatus(account);
            }
            catch (EndiciaException ex)
            {
                throw new Express1RegistrationException(ex.Message, ex);
            }
        }
    }
}
