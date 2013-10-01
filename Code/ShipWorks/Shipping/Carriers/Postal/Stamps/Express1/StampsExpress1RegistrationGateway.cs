﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
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
                // This throws a stamps exception if the account credentials are incorrect
                StampsApiSession.GetAccountInfo(new StampsAccountEntity
                {
                    Username = registration.UserName,
                    Password = registration.EncryptedPassword,
                    IsExpress1 = true
                });
            }
            catch (StampsException ex)
            {
                throw new Express1RegistrationException(ex.Message, ex);
            }
        }
    }
}
