using System;
using System.Linq;
using System.Collections.Generic;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for working with the interapptive license server
    /// </summary>
    public class TangoWebClientWrapper : ITangoWebClient
    {
        /// <summary>
        /// Activate the given license key to the specified store identifier
        /// </summary>
        public virtual LicenseAccountDetail ActivateLicense(string licenseKey, StoreEntity store)
        {
            return TangoWebClient.ActivateLicense(licenseKey, store);
        }

        /// <summary>
        /// Get the status of the specified license
        /// </summary>
        public virtual LicenseAccountDetail GetLicenseStatus(string licenseKey, StoreEntity store)
        {
            return TangoWebClient.GetLicenseStatus(licenseKey, store);
        }

        /// <summary>
        /// Get the status of the specified license
        /// </summary>
        public virtual Dictionary<string, string> GetCounterRatesCredentials(StoreEntity store)
        {
            return TangoWebClient.GetCounterRatesCredentials(store);
        }

        /// <summary>
        /// Get the status of the specified license
        /// </summary>
        public virtual Dictionary<string, string> GetCarrierCertificateVerificationData(StoreEntity store)
        {
            return TangoWebClient.GetCarrierCertificateVerificationData(store);
        }

        /// <summary>
        /// Request a trial for use with the specified store. If a trial already exists, a new one will not be created.
        /// </summary>
        public virtual TrialDetail GetTrial(StoreEntity store)
        {
            return TangoWebClient.GetTrial(store);
        }

        /// <summary>
        /// Extend the trial for the given store
        /// </summary>
        public virtual TrialDetail ExtendTrial(StoreEntity store)
        {
            return TangoWebClient.ExtendTrial(store);
        }

        /// <summary>
        /// Activates shipworks for the given user
        /// </summary>
        public GenericResult<ActivationResponse> ActivateLicense(string email, string password)
        {
            return TangoWebClient.ActivateLicense(email, password);
        }

        /// <summary>
        /// Send the user their username using the specified email address
        /// </summary>
        public virtual void SendAccountUsername(string email, string username)
        {
            TangoWebClient.SendAccountUsername(email, username);
        }

        /// <summary>
        /// Sends Postal balances for postal services.
        /// </summary>
        public virtual void LogPostageEvent(decimal balance, decimal purchaseAmount, ShipmentTypeCode shipmentTypeCode, string accountIdentifier)
        {
            // Send licenses for each distinct customer ID of the enabled stores. This could take a couple of seconds 
            // depending on the number of stores. May want to look into caching this information, but that could result
            // in stale license data. Since customers aren't buying postage all the time, the additonal overhead to ensure
            // accuracy may not be that big of a deal.
            List<StoreEntity> stores = StoreManager.GetAllStores();
            IEnumerable<LicenseAccountDetail> licenses = stores.Select(store => TangoWebClient.GetLicenseStatus(store.License, store)).Where(l => l.Active);

            // We only need to send up one license for each distinct customer ID
            IEnumerable<LicenseAccountDetail> licensesForLogging = licenses.GroupBy(l => l.TangoCustomerID).Select(grp => grp.First());
            foreach (LicenseAccountDetail license in licensesForLogging)
            {
                TangoWebClient.LogPostageEvent(license, balance, purchaseAmount, shipmentTypeCode, accountIdentifier);
            }
        }

        /// <summary>
        /// Log the given processed shipment to Tango.  isRetry is only for internal interapptive purposes to handle rare cases where shipments a customer
        /// insured did not make it up into tango, but the shipment did actually process.
        /// </summary>
        public virtual void LogShipment(StoreEntity store, ShipmentEntity shipment, bool isRetry = false)
        {
            TangoWebClient.LogShipment(store, shipment, isRetry);
        }

        /// <summary>
        /// Void the given processed shipment to Tango
        /// </summary>
        public virtual void VoidShipment(StoreEntity store, ShipmentEntity shipment)
        {
            TangoWebClient.VoidShipment(store, shipment);
        }

        /// <summary>
        /// Records License agreement and generates an Interapptive-tracked policy number.
        /// </summary>
        public virtual string GenerateInsurancePolicyNumber(StoreEntity store)
        {
            return TangoWebClient.GenerateInsurancePolicyNumber(store);
        }

        /// <summary>
        /// Get the freemium status of the given store
        /// </summary>
        public LicenseAccountDetail GetFreemiumStatus(EbayStoreEntity store)
        {
            return TangoWebClient.GetFreemiumStatus(store);
        }

        /// <summary>
        /// Create a new freemium store in tango
        /// </summary>
        public virtual void CreateFreemiumStore(StoreEntity store, PersonAdapter accountAddress, EndiciaPaymentInfo paymentInfo, string dazzleAccount, bool validateOnly)
        {
            TangoWebClient.CreateFreemiumStore(store, accountAddress, paymentInfo, dazzleAccount, validateOnly);
        }

        /// <summary>
        /// Associate the given freemium account# with the given store
        /// </summary>
        public virtual void SetFreemiumAccountNumber(StoreEntity store, string freemiumAccount)
        {
            TangoWebClient.SetFreemiumAccountNumber(store, freemiumAccount);
        }

        /// <summary>
        /// Update the platform, developer, and version info for the given generic store
        /// </summary>
        public virtual void UpdateGenericModuleInfo(GenericModuleStoreEntity store, string platform, string developer, string version)
        {
            TangoWebClient.UpdateGenericModuleInfo(store, platform, developer, version);
        }

        /// <summary>
        /// Upgrade the given store to the 'Paid' version of freemium with the given endicia service plan.
        /// </summary>
        public virtual void UpgradeFreemiumStore(StoreEntity store, int endiciaServicePlan)
        {
            TangoWebClient.UpgradeFreemiumStore(store, endiciaServicePlan);
        }

        /// <summary>
        /// Upgrade the given trial to not be in an 'Edition' mode
        /// </summary>
        public virtual void UpgradeEditionTrial(StoreEntity store)
        {
            TangoWebClient.UpgradeEditionTrial(store);
        }

        /// <summary>
        /// Ensure the connection to the given URI is a valid interapptive secure connection
        /// </summary>
        public virtual void ValidateSecureConnection(Uri uri)
        {
            TangoWebClient.ValidateSecureConnection(uri);
        }

        /// <summary>
        /// Gets the nudges.
        /// </summary>
        public virtual IEnumerable<Nudge> GetNudges(IEnumerable<StoreEntity> stores)
        {
            List<Nudge> nudges = new List<Nudge>();
            
            foreach (StoreEntity store in stores)
            {
                nudges.AddRange(TangoWebClient.GetNudges(store));
            }

            return nudges.GroupBy(n => n.NudgeID).Select(group => group.First());
        }

        /// <summary>
        /// Logs the nudge option back to Tango. Intended to record which option was selected by the user.
        /// </summary>
        public virtual void LogNudgeOption(NudgeOption option)
        {
            TangoWebClient.LogNudgeOption(option);
        }

        /// <summary>
        /// Sends Usps account info to Tango.
        /// </summary>
        /// <param name="account">The account.</param>
        public virtual void LogUspsAccount(UspsAccountEntity account)
        {
            // Send licenses for each distinct customer ID of the enabled stores. This could take a couple of seconds 
            // depending on the number of stores. May want to look into caching this information, but that could result
            // in stale license data. Since customers aren't buying postage all the time, the additonal overhead to ensure
            // accuracy may not be that big of a deal.
            List<StoreEntity> stores = StoreManager.GetAllStores();
            IEnumerable<LicenseAccountDetail> licenses = stores.Select(store => TangoWebClient.GetLicenseStatus(store.License, store)).Where(l => l.Active);

            // We only need to send up one license for each distinct customer ID
            IEnumerable<LicenseAccountDetail> licensesForLogging = licenses.GroupBy(l => l.TangoCustomerID).Select(grp => grp.First());
            foreach (LicenseAccountDetail license in licensesForLogging)
            {
                TangoWebClient.LogStampsAccount(license,
                                                PostalUtility.GetUspsShipmentTypeForUspsResellerType((UspsResellerType) account.UspsReseller).ShipmentTypeCode,
                                                account.Username,
                                                (UspsAccountContractType)account.ContractType);
            }
        }
    }
}