using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Registration
{
    /// <summary>
    /// An implementation of the IExpress1RegistrationRepository interface. This will use the UspsAccountManager 
    /// to save an Express1 registration to the USPS account table.
    /// </summary>
    public class UspsExpress1RegistrationRepository : IExpress1RegistrationRepository
    {
        /// <summary>
        /// Saves the Express1 registration to the appropriate data source (USPS account table or Endicia account table).
        /// </summary>
        /// <param name="registration">The registration object containing the Express1 account info being saved.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public long Save(Express1Registration registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException("registration");
            }

            // Create a new USPS account entity that will get saved to the database
            UspsAccountEntity uspsAccount = registration.AccountId.HasValue ? 
                UspsAccountManager.GetAccount(registration.AccountId.Value) : 
                new UspsAccountEntity();

            // Initialize the nulls to default values and denote that the account is for Express1
            uspsAccount.InitializeNullsToDefault();
            uspsAccount.UspsReseller = (int)UspsResellerType.Express1;

            uspsAccount.ContractType = (int)UspsAccountContractType.NotApplicable;

            // Translate the registration data into a USPS account entity
            uspsAccount.Username = registration.UserName;
            uspsAccount.Password = registration.EncryptedPassword;

            uspsAccount.FirstName = registration.MailingAddress.FirstName;
            uspsAccount.MiddleName = registration.MailingAddress.MiddleName;
            uspsAccount.LastName = registration.MailingAddress.LastName;
            uspsAccount.Company = registration.MailingAddress.Company;

            // Address info
            uspsAccount.Street1 = registration.MailingAddress.Street1;
            uspsAccount.Street2 = registration.MailingAddress.Street2;
            uspsAccount.Street3 = registration.MailingAddress.Street3;
            uspsAccount.City = registration.MailingAddress.City;
            uspsAccount.PostalCode = registration.MailingAddress.PostalCode;
            uspsAccount.CountryCode = Geography.GetCountryCode(registration.MailingAddress.CountryCode);
            uspsAccount.StateProvCode = Geography.GetStateProvCode(registration.MailingAddress.StateProvCode);
            uspsAccount.MailingPostalCode = registration.MailingAddress.PostalCode;

            // Contact information (website is not collected required by Express1, so the registration is not
            // collecting this information)
            uspsAccount.Phone = registration.Phone10Digits;
            uspsAccount.Email = registration.Email;
            uspsAccount.Website = string.Empty;

            uspsAccount.CreatedDate = DateTime.UtcNow;
            
            // Persist the account entity to the database
            UspsAccountManager.SaveAccount(uspsAccount);

            // If this is the only account, update this shipment type profiles with this account
            List<UspsAccountEntity> accounts = UspsAccountManager.GetAccounts(UspsResellerType.Express1, false);
            if (accounts.Count == 1)
            {
                UspsAccountEntity accountEntity = accounts.First();

                // Update any profiles to use this account if this is the only account
                // in the system. This is to account for the situation where there a multiple
                // profiles that may be associated with a previous account that has since
                // been deleted. 
                foreach (ShippingProfileEntity shippingProfileEntity in ShippingProfileManager.Profiles.Where(p => p.ShipmentType == (int)ShipmentTypeCode.Express1Usps))
                {
                    if (shippingProfileEntity.Postal.Usps.UspsAccountID.HasValue)
                    {
                        shippingProfileEntity.Postal.Usps.UspsAccountID = accountEntity.UspsAccountID;
                        ShippingProfileManager.SaveProfile(shippingProfileEntity);
                    }
                }
            }

            // Update the account contract type
            Express1UspsShipmentType uspsShipmentType = (Express1UspsShipmentType)ShipmentTypeManager.GetType(ShipmentTypeCode.Express1Usps);
            uspsShipmentType.UpdateContractType(uspsAccount);

            return uspsAccount.UspsAccountID;
        }


        /// <summary>
        /// Uses the UspsAccountManager to Delete the carrier account (if it exists) associated with the 
        /// given registration from the uspsAccount table.
        /// </summary>
        /// <param name="registration">The registration object containing the Express1 account info being deleted.</param>
        public void Delete(Express1Registration registration)
        {
            if (registration != null && registration.AccountId.HasValue)
            {
                // The registration has an account ID associated with it, implying there is an account 
                // entity associated with it. 
                UspsAccountEntity uspsAccountEntity = UspsAccountManager.GetAccount(registration.AccountId.Value);
                if (uspsAccountEntity != null)
                {
                    // We've confirmed this account still does exist and needs to be deleted
                    UspsAccountManager.DeleteAccount(uspsAccountEntity);
                }
            }
        }
    }
}
