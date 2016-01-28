using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Stores;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using System.Linq;

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
        private readonly ILog log;
        private IDeletionService deletionService;
        private IStoreManager storeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicense(string key, ITangoWebClient tangoWebClient, ICustomerLicenseWriter licenseWriter, Func<Type, ILog> logFactory, IDeletionService deletionService, IStoreManager storeManager, Func<IChannelLimitDlg> channelLimitDlgFactory)
        {
            Key = key;
            this.tangoWebClient = tangoWebClient;
            this.licenseWriter = licenseWriter;
			this.channelLimitDlgFactory = channelLimitDlgFactory;
            log = logFactory(typeof(CustomerLicense));
            this.deletionService = deletionService;
            this.storeManager = storeManager;
        }

        /// <summary>
        /// The license key
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Activate a new store
        /// </summary>
        public EnumResult<LicenseActivationState> Activate(StoreEntity store)
        {
            AddStoreResponse response = tangoWebClient.AddStore(this, store);

            store.License = response.Key;

            return response.Success ?
                new EnumResult<LicenseActivationState>(LicenseActivationState.Active) :
                new EnumResult<LicenseActivationState>(LicenseActivationState.Invalid, response.Error);
        }
       
        /// <summary>
        /// If License is over the channel limit prompt user to delete channels
        /// </summary>
        public void EnforceChannelLimit()
        {
            Refresh();

            if (IsOverChannelLimit)
            {
                IChannelLimitDlg channelLimitDlg = channelLimitDlgFactory();
                channelLimitDlg.ShowDialog();
            }
        }

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
        public ILicenseCapabilities LicenseCapabilities { get; set; }
        
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
            return tangoWebClient.GetActiveStores(this);
        }

        /// <summary>
        /// Deletes the given store
        /// </summary>
        /// <param name="store"></param>
        public void DeleteStore(StoreEntity store)
        {
            log.Warn($"Deleting store: {store.StoreName}");

            // grab the stores license
            string license = store.License;

            // Remove the store from ShipWorks
            deletionService.DeleteStore(store);
            
            // Delete the stores in tango
            tangoWebClient.DeleteStore(this, license);
        }

        /// <summary>
        /// Delete the given channel
        /// </summary>
        public void DeleteChannel(StoreTypeCode storeType)
        {
            log.Warn($"Deleting channel: {EnumHelper.GetDescription(storeType)}");

            // Get all of the stores that match the type we want to remove
            IEnumerable<StoreEntity> localStoresToDelete = storeManager.GetAllStores().Where(s => s.TypeCode == (int)storeType);

            // Get a list of licenses we are about to delete
            List<string> licensesToDelete = new List<string>();
            localStoresToDelete.ToList().ForEach(s => licensesToDelete.Add(s.License));

            // remove the local stores individually 
            localStoresToDelete.ToList().ForEach(DeleteStore);

            // Delete the stores in tango
            tangoWebClient.DeleteStores(this, licensesToDelete);
        }

        /// <summary>
        /// Is the license over the ChannelLimit
        /// </summary>
        public bool IsOverChannelLimit => 
            LicenseCapabilities.ActiveChannels > LicenseCapabilities.ChannelLimit;
    }
}