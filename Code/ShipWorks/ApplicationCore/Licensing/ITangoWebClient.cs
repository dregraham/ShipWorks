using System;
using System.Collections.Generic;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using ShipWorks.Shipping.Insurance.InsureShip;

namespace ShipWorks.ApplicationCore.Licensing
{
    public interface ITangoWebClient
    {
        /// <summary>
        /// Activate the given license key to the specified store identifier
        /// </summary>
        LicenseAccountDetail ActivateLicense(string licenseKey, StoreEntity store);

        /// <summary>
        /// Get the status of the specified license
        /// </summary>
        ILicenseAccountDetail GetLicenseStatus(string licenseKey, StoreEntity store);

        /// <summary>
        /// Get the status of the specified license
        /// </summary>
        Dictionary<string, string> GetCounterRatesCredentials(StoreEntity store);

        /// <summary>
        /// Returns an InsureShipAffiliate for the specified store.
        /// If one cannot be found, an InsureShipException is thrown.
        /// </summary>
        InsureShipAffiliate GetInsureShipAffiliate(StoreEntity store);

        /// <summary>
        /// Get the status of the specified license
        /// </summary>
        Dictionary<string, string> GetCarrierCertificateVerificationData(StoreEntity store);

        /// <summary>
        /// Request a trial for use with the specified store. If a trial already exists, a new one will not be created.
        /// </summary>
        TrialDetail GetTrial(StoreEntity store);

        /// <summary>
        /// Extend the trial for the given store
        /// </summary>
        TrialDetail ExtendTrial(StoreEntity store);

        /// <summary>
        /// Associates a Usps account created in ShipWorks as the users free Stamps.com account
        /// </summary>
        AssociateShipWorksWithItselfResponse AssociateShipworksWithItself(AssociateShipworksWithItselfRequest associateShipworksWithItselfRequest);

        /// <summary>
        /// Activates ShipWorks using the given user
        /// </summary>
        GenericResult<IActivationResponse> ActivateLicense(string email, string password);

        /// <summary>
        /// Send the user their username using the specified email address
        /// </summary>
        void SendAccountUsername(string email, string username);

        /// <summary>
        /// Sends Postal balances for postal services.
        /// </summary>
        void LogPostageEvent(decimal balance, decimal purchaseAmount, ShipmentTypeCode shipmentTypeCode, string accountIdentifier);

        /// <summary>
        /// Log the given processed shipment to Tango.  isRetry is only for internal interapptive purposes to handle rare cases where shipments a customer
        /// insured did not make it up into tango, but the shipment did actually process.
        /// </summary>
        string LogShipment(StoreEntity store, ShipmentEntity shipment, bool isRetry = false);

        /// <summary>
        /// Void the given processed shipment to Tango
        /// </summary>
        void VoidShipment(StoreEntity store, ShipmentEntity shipment);

        /// <summary>
        /// Records License agreement and generates an Interapptive-tracked policy number.
        /// </summary>
        string GenerateInsurancePolicyNumber(StoreEntity store);

        /// <summary>
        /// Get the freemium status of the given store
        /// </summary>
        LicenseAccountDetail GetFreemiumStatus(EbayStoreEntity store);

        /// <summary>
        /// Create a new freemium store in tango
        /// </summary>
        void CreateFreemiumStore(StoreEntity store, PersonAdapter accountAddress, EndiciaPaymentInfo paymentInfo, string dazzleAccount, bool validateOnly);

        /// <summary>
        /// Associate the given freemium account# with the given store
        /// </summary>
        void SetFreemiumAccountNumber(StoreEntity store, string freemiumAccount);

        /// <summary>
        /// Update the platform, developer, and version info for the given generic store
        /// </summary>
        void UpdateGenericModuleInfo(GenericModuleStoreEntity store, string platform, string developer, string version);

        /// <summary>
        /// Upgrade the given store to the 'Paid' version of freemium with the given endicia service plan.
        /// </summary>
        void UpgradeFreemiumStore(StoreEntity store, int endiciaServicePlan);

        /// <summary>
        /// Upgrade the given trial to not be in an 'Edition' mode
        /// </summary>
        void UpgradeEditionTrial(StoreEntity store);

        /// <summary>
        /// Ensure the connection to the given URI is a valid interapptive secure connection
        /// </summary>
        void ValidateSecureConnection(Uri uri);

        /// <summary>
        /// Gets a collection of nudges from Tango.
        /// </summary>
        IEnumerable<Nudge> GetNudges(IEnumerable<StoreEntity> stores);

        /// <summary>
        /// Logs the nudge option back to Tango. Intended to record which option was selected by the user.
        /// </summary>
        void LogNudgeOption(NudgeOption option);

        /// <summary>
        /// Sends USPS account info to Tango.
        /// </summary>
        /// <param name="account">The account.</param>
        void LogUspsAccount(UspsAccountEntity account);

        /// <summary>
        /// Gets the license capabilities.
        /// </summary>
        ILicenseCapabilities GetLicenseCapabilities(ICustomerLicense license);

        /// <summary>
        /// Gets the active stores from Tango.
        /// </summary>
        IEnumerable<ActiveStore> GetActiveStores(ICustomerLicense license);

        /// <summary>
        /// Deletes a store from Tango.
        /// </summary>
        void DeleteStore(ICustomerLicense customerLicense, string storeLicenseKey);

        /// <summary>
        /// Deletes multiple stores from Tango.
        /// </summary>
        void DeleteStores(ICustomerLicense customerLicense, IEnumerable<string> storeLicenseKeys);

        /// <summary>
        /// Makes a request to Tango to add a store
        /// </summary>
        IAddStoreResponse AddStore(ICustomerLicense license, StoreEntity store);

        /// <summary>
        /// Associates a free Stamps.com account with a customer license.
        /// </summary>
        void AssociateStampsUsernameWithLicense(string licenseKey, string stampsUsername, string stampsPassword);

        /// <summary>
        /// Gets the Tango customer id for a license.
        /// </summary>
        string GetTangoCustomerId();
    }
}