﻿using System;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Registration
{
    /// <summary>
    /// Gateway class for integrating with Express1 and USPS
    /// </summary>
    [Component(RegistrationType.Self)]
    public class UspsExpress1RegistrationGateway : Express1RegistrationGateway
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UspsExpress1RegistrationGateway()
            : base(new Express1UspsConnectionDetails())
        {
        }

        /// <summary>
        /// Verifies that the specified user name and password map to a valid account. An Express1RegistrationException
        /// is thrown if the account credentials are incorrect.
        /// </summary>
        /// <param name="registration">Registration that defines the account to test</param>
        /// <exception cref="System.ArgumentNullException">registration</exception>
        /// <exception cref="Express1RegistrationException">Thrown if the credentials could not be verified.</exception>
        public override void VerifyAccount(Express1Registration registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }

            try
            {
                // This throws a USPS exception if the account credentials are incorrect
                new Express1UspsWebClient().GetAccountInfo(new UspsAccountEntity
                {
                    Username = registration.UserName,
                    Password = registration.EncryptedPassword,
                    UspsReseller = (int) UspsResellerType.Express1
                });
            }
            catch (UspsException ex)
            {
                throw new Express1RegistrationException(ex.Message, ex);
            }
        }
    }
}
