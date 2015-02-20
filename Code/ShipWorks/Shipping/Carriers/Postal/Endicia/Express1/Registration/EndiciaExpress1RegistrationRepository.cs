using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.Registration
{
    /// <summary>
    /// An implementation of the IExpress1RegistrationRepository interface. This will use the EndiciaAccountManager 
    /// to save an Express1 registration to the Endicia account table.
    /// </summary>
    public class EndiciaExpress1RegistrationRepository : IExpress1RegistrationRepository
    {
        /// <summary>
        /// Saves the Express1 registration to the appropriate data source (Usps account table or Endicia account table).
        /// </summary>
        /// <param name="registration">The registration object containing the Express1 account info being saved.</param>
        /// <returns></returns>
        public long Save(Express1Registration registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }

            
            // Create a new usps account entity that will get saved to the database
            EndiciaAccountEntity endiciaAccount = registration.AccountId.HasValue ?
                EndiciaAccountManager.GetAccount(registration.AccountId.Value) :
                new EndiciaAccountEntity();

            // Initialize the nulls to default values and denote that the account is for Express1
            endiciaAccount.InitializeNullsToDefault();
            endiciaAccount.EndiciaReseller = (int)EndiciaReseller.Express1;

            // Translate the registration data into an Endicia account entity
            endiciaAccount.AccountNumber = registration.UserName;
            endiciaAccount.ApiUserPassword = registration.EncryptedPassword;

            endiciaAccount.FirstName = registration.MailingAddress.FirstName;
            endiciaAccount.LastName = registration.MailingAddress.LastName;
            endiciaAccount.Company = registration.MailingAddress.Company;

            // Address info
            endiciaAccount.Street1 = registration.MailingAddress.Street1;
            endiciaAccount.Street2 = registration.MailingAddress.Street2;
            endiciaAccount.Street3 = registration.MailingAddress.Street3;
            endiciaAccount.City = registration.MailingAddress.City;
            endiciaAccount.PostalCode = registration.MailingAddress.PostalCode;
            endiciaAccount.CountryCode = Geography.GetCountryCode(registration.MailingAddress.CountryCode);
            endiciaAccount.StateProvCode = Geography.GetStateProvCode(registration.MailingAddress.StateProvCode);
            endiciaAccount.MailingPostalCode = registration.MailingAddress.PostalCode;

            endiciaAccount.Phone = registration.Phone10Digits;
            endiciaAccount.Email = registration.Email;
            endiciaAccount.Fax = registration.MailingAddress.Fax;
            
            endiciaAccount.CreatedByShipWorks = !registration.AccountId.HasValue;
            endiciaAccount.AccountType = (int)EndiciaAccountType.Standard;
            endiciaAccount.ScanFormAddressSource = (int)EndiciaScanFormAddressSource.Provider;
            endiciaAccount.TestAccount = Express1EndiciaUtility.UseTestServer;
            
            endiciaAccount.Description = EndiciaAccountManager.GetDefaultDescription(endiciaAccount);

            // Persist the account entity to the database
            EndiciaAccountManager.SaveAccount(endiciaAccount);

            // If this is the only account, update this shipment type profiles with this account
            List<EndiciaAccountEntity> accounts = EndiciaAccountManager.GetAccounts((EndiciaReseller)endiciaAccount.EndiciaReseller, false);
            if (accounts.Count == 1)
            {
                EndiciaAccountEntity accountEntity = accounts.First();

                // Update any profiles to use this account if this is the only account
                // in the system. This is to account for the situation where there a multiple
                // profiles that may be associated with a previous account that has since
                // been deleted. 
                foreach (ShippingProfileEntity shippingProfileEntity in ShippingProfileManager.Profiles.Where(p => p.ShipmentType == (int)ShipmentTypeCode.Express1Endicia && p.Postal.Endicia.EndiciaAccountID != null))
                {
                    if (shippingProfileEntity.Postal.Endicia.EndiciaAccountID.HasValue)
                    {
                        shippingProfileEntity.Postal.Endicia.EndiciaAccountID = accountEntity.EndiciaAccountID;
                        ShippingProfileManager.SaveProfile(shippingProfileEntity); 
                    }
                }
            }

            return endiciaAccount.EndiciaAccountID;
        }

        /// <summary>
        /// Deletes the carrier account (if it exists) associated with the given registration from the appropriate data
        /// source (Usps account table or Endicia account table).
        /// </summary>
        /// <param name="registration">The registration object containing the Express1 account info being deleted.</param>
        public void Delete(Express1Registration registration)
        {
            if (registration != null && registration.AccountId.HasValue)
            {
                // The registration has an account ID associated with it, implying there is an account 
                // entity associated with it. 
                EndiciaAccountEntity endiciaAccountEntity = EndiciaAccountManager.GetAccount(registration.AccountId.Value);
                if (endiciaAccountEntity != null)
                {
                    // We've confirmed this account still does exist and needs to be deleted
                    EndiciaAccountManager.DeleteAccount(endiciaAccountEntity);
                }
            }
        }
    }
}
