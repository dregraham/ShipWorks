using System;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// An implementation of the IExpress1RegistrationRepository interface. This will use the StampsAccountManager 
    /// to save an Express1 registration to the Stamps account table.
    /// </summary>
    public class StampsExpress1RegistrationRepository : IExpress1RegistrationRepository
    {
        /// <summary>
        /// Saves the Express1 registration to the appropriate data source (Stamps account table or Endicia account table).
        /// </summary>
        /// <param name="registration">The registration object containing the Express1 account info being saved.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public long Save(Express1Registration registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }

            // Create a new stamps account entity that will get saved to the database
            StampsAccountEntity stampsAccount = registration.AccountId.HasValue ? 
                StampsAccountManager.GetAccount(registration.AccountId.Value) : 
                new StampsAccountEntity();

            // Initialize the nulls to default values and denote that the account is for Express1
            stampsAccount.InitializeNullsToDefault();
            stampsAccount.IsExpress1 = true;

            // Translate the registration data into a Stamps account entity
            stampsAccount.Username = registration.UserName;
            stampsAccount.Password = registration.EncryptedPassword;

            stampsAccount.FirstName = registration.MailingAddress.FirstName;
            stampsAccount.MiddleName = registration.MailingAddress.MiddleName;
            stampsAccount.LastName = registration.MailingAddress.LastName;
            stampsAccount.Company = registration.MailingAddress.Company;

            // Address info
            stampsAccount.Street1 = registration.MailingAddress.Street1;
            stampsAccount.Street2 = registration.MailingAddress.Street2;
            stampsAccount.Street3 = registration.MailingAddress.Street3;
            stampsAccount.City = registration.MailingAddress.City;
            stampsAccount.PostalCode = registration.MailingAddress.PostalCode;
            stampsAccount.CountryCode = Geography.GetStateProvCode(registration.MailingAddress.CountryCode);
            stampsAccount.StateProvCode = Geography.GetStateProvCode(registration.MailingAddress.StateProvCode);
            stampsAccount.MailingPostalCode = registration.MailingAddress.PostalCode;

            // Contact information (website is not collected required by Express1, so the registration is not
            // collecting this information)
            stampsAccount.Phone = registration.Phone10Digits;
            stampsAccount.Email = registration.Email;
            stampsAccount.Website = string.Empty;
            
            // Persist the account entity to the database
            StampsAccountManager.SaveAccount(stampsAccount);

            return stampsAccount.StampsAccountID;
        }


        /// <summary>
        /// Uses the StampsAccountManager to Delete the carrier account (if it exists) associated with the 
        /// given registration from the StampsAccount table.
        /// </summary>
        /// <param name="registration">The registration object containing the Express1 account info being deleted.</param>
        public void Delete(Express1Registration registration)
        {
            if (registration != null && registration.AccountId.HasValue)
            {
                // The registration has an account ID associated with it, implying there is an account 
                // entity associated with it. 
                StampsAccountEntity stampsAccountEntity = StampsAccountManager.GetAccount(registration.AccountId.Value);
                if (stampsAccountEntity != null)
                {
                    // We've confirmed this account still does exist and needs to be deleted
                    StampsAccountManager.DeleteAccount(stampsAccountEntity);
                }
            }
        }
    }
}
