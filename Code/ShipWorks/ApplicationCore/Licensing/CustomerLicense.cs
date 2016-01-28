using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Stores;
using ShipWorks.Data.Model.EntityClasses;

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

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicense(string key, ITangoWebClient tangoWebClient, ICustomerLicenseWriter licenseWriter, Func<Type, ILog> logFactory, Func<IChannelLimitDlg> channelLimitDlgFactory)
        {
            Key = key;
            this.tangoWebClient = tangoWebClient;
            this.licenseWriter = licenseWriter;
            this.channelLimitDlgFactory = channelLimitDlgFactory;
            log = logFactory(typeof(CustomerLicense));
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
        /// Is the license over the ChannelLimit
        /// </summary>
        public bool IsOverChannelLimit => 
            LicenseCapabilities.ActiveChannels > LicenseCapabilities.ChannelLimit;
    }
}