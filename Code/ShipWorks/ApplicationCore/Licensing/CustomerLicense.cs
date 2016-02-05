using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Stores;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using System.Linq;
using Interapptive.Shared;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Class to store customer license information
    /// </summary>
    public class CustomerLicense : ICustomerLicense
    {
        private readonly ITangoWebClient tangoWebClient;
        private readonly ICustomerLicenseWriter licenseWriter;
        private readonly Func<IChannelLimitDlg> channelLimitDlgFactory;
        private readonly IUpgradePlanDlgFactory upgradePlanDlgFactory;
        private readonly ILog log;
        private readonly IDeletionService deletionService;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public CustomerLicense(
            string key, 
            ITangoWebClient tangoWebClient, 
            ICustomerLicenseWriter licenseWriter, 
            Func<Type, ILog> logFactory, 
            IDeletionService deletionService, 
            Func<IChannelLimitDlg> channelLimitDlgFactory,
            IUpgradePlanDlgFactory upgradePlanDlgFactory)
        {
            Key = key;
            this.tangoWebClient = tangoWebClient;
            this.licenseWriter = licenseWriter;
			this.channelLimitDlgFactory = channelLimitDlgFactory;
            this.upgradePlanDlgFactory = upgradePlanDlgFactory;
            log = logFactory(typeof(CustomerLicense));
            this.deletionService = deletionService;
        }

        /// <summary>
        /// The license key
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Is the license legacy
        /// </summary>
        public bool IsLegacy => false;

        /// <summary>
        /// Reason the license is Disabled
        /// </summary>
        public string DisabledReason { get; set; }

        /// <summary>
        /// Is the license Disabled
        /// </summary>
        public bool IsDisabled => (!string.IsNullOrEmpty(DisabledReason));

        /// <summary>
        /// The license capabilities.
        /// </summary>
        private ILicenseCapabilities LicenseCapabilities { get; set; }

        /// <summary>
        /// Is the license over the ChannelLimit
        /// </summary>
        public bool IsOverChannelLimit
        {
            get
            {
                return (LicenseCapabilities.ActiveChannels > LicenseCapabilities.ChannelLimit) &&
                    !LicenseCapabilities.IsInTrial; 
            }
        }

        /// <summary>
        /// Is the license over the ChannelLimit
        /// </summary>
        public bool IsShipmentLimitReached
        {
            get
            {
                return (LicenseCapabilities.ProcessedShipments >= LicenseCapabilities.ShipmentLimit) &&
                    !LicenseCapabilities.IsInTrial;
            }
        }

        /// <summary>
        /// The number of licenses needed to be deleted to be in compliance
        /// </summary>
        public int NumberOfChannelsOverLimit
        {
            get
            {
                int numberOfChannelsOverLimit = 0;

                if (IsOverChannelLimit)
                {
                    numberOfChannelsOverLimit = LicenseCapabilities.ActiveChannels - LicenseCapabilities.ChannelLimit;
                }
                
                return numberOfChannelsOverLimit;
            }
        }

        /// <summary>
        /// Activate a new store
        /// </summary>
        public EnumResult<LicenseActivationState> Activate(StoreEntity store)
        {
            IAddStoreResponse response = tangoWebClient.AddStore(this, store);

            store.License = response.Key;

            if (response.Success)
            {
                return new EnumResult<LicenseActivationState>(LicenseActivationState.Active);
            }

            LicenseActivationState activationState = LicenseActivationState.Invalid;
            if (response.Error.IndexOf("OverChannelLimit", StringComparison.Ordinal) >= 0)
            {
                activationState = LicenseActivationState.OverChannelLimit;
            }

            return new EnumResult<LicenseActivationState>(activationState, response.Error);
        }

        /// <summary>
        /// Activates the customer license
        /// </summary>
        public void Activate(string email, string password)
        {
            // Activate the license via tango using the given username and password
            GenericResult<ActivationResponse> activationResponse = tangoWebClient.ActivateLicense(email, password);

            // Check to see if something went wrong and if so we throw
            if (!activationResponse.Success)
            {
                throw new ShipWorksLicenseException(activationResponse.Message);
            }

            Key = activationResponse.Context.Key;

            // Save license data to the data source
            Save();
        }

        /// <summary>
        /// Uses the ILicenseWriter provided in the constructor to save this instance
        /// to a data source.
        /// </summary>
        public void Save()
        {
            licenseWriter.Write(this);
        }


        /// <summary>
        /// If License is over the channel limit prompt user to delete channels
        /// </summary>
        public void EnforceChannelLimit()
        {
            Refresh();

            if (IsOverChannelLimit)
            {
                try
                {
                    IChannelLimitDlg channelLimitDlg = channelLimitDlgFactory();                  
                    channelLimitDlg.ShowDialog();
                }
                catch (ShipWorksLicenseException ex)
                {
                    log.Error("Error thrown when displaying channel limit dialog", ex);
                }
            }
        }

        /// <summary>
        /// If license is at shipment limit, prompt user to upgrade
        /// when attempting to process a shipment
        /// </summary>
        public void EnforceShipmentLimit()
        {
            Refresh();

            if (IsShipmentLimitReached)
            {
                IDialog dialog = upgradePlanDlgFactory.Create("You have reached your shipment limit for this billing cycle. Please upgrade your plan to process shipments.");
                dialog.ShowDialog();
            }
        }

        /// <summary>
        /// Refresh the License capabilities from Tango
        /// </summary>
        public void Refresh()
        {
            try
            {
                LicenseCapabilities = tangoWebClient.GetLicenseCapabilities(this);
            }
            catch (TangoException ex)
            {
                LicenseCapabilities = null; // may want to use a null object pattern here...
                DisabledReason = ex.Message;
                log.Warn(ex);
            }
        }

        /// <summary>
        /// IEnumerable of ActiveStores for the license
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ActiveStore> GetActiveStores()
        {
            try
            {
                return tangoWebClient.GetActiveStores(this);
            }
            catch (TangoException ex)
            {
                throw new ShipWorksLicenseException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Deletes the given store
        /// </summary>
        /// <param name="store"></param>
        public void DeleteStore(StoreEntity store)
        {
            if (store == null)
            {
                return;
            }

            // Save the key to use later
            string licenseKey = store.License;

            // Remove the store from the database
            log.Warn($"Deleting store: {store.StoreName}");
            deletionService.DeleteStore(store);

            // Tell tango to delete the licenseKey
            tangoWebClient.DeleteStore(this, licenseKey);
        }

        /// <summary>
        /// Delete the given channel
        /// </summary>
        public void DeleteChannel(StoreTypeCode storeType)
        {
            if (storeType == StoreTypeCode.Invalid)
            {
                return;
            }

            // Delete all of the local stores for the given StoreTypeCode
            log.Warn($"Deleting channel: {EnumHelper.GetDescription(storeType)}");
            deletionService.DeleteChannel(storeType);

            // Get a list of licenses that are active in tango and match the channel we are deleting
            // but are not in ShipWorks and tell tango to delete them
            IEnumerable<string> licensesToDelete = GetActiveStores().Where(a => a.StoreType == storeType).Select(a => a.StoreLicenseKey);
            tangoWebClient.DeleteStores(this, licensesToDelete);
        }
    }
}