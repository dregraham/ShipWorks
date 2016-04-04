using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{
    /// <summary>
    /// Checks to see that all of the stores in ShipWorks
    /// are in sync with the stores in Tango
    /// </summary>
    public class ChannelSyncEnforcer : ILicenseEnforcer
    {
        private readonly IChannelLimitDlgFactory channelLimitDlgFactory;
        private readonly ILicenseService licenseService;
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelSyncEnforcer(IChannelLimitDlgFactory channelLimitDlgFactory, ILicenseService licenseService, IStoreManager storeManager)
        {
            this.channelLimitDlgFactory = channelLimitDlgFactory;
            this.licenseService = licenseService;
            this.storeManager = storeManager;
        }

        /// <summary>
        /// High Priority
        /// </summary>
        public EnforcementPriority Priority => EnforcementPriority.High;

        /// <summary>
        /// Works on ChannelCount
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.ChannelCount;

        /// <summary>
        /// Checks to see if we are in compliant, if not display a dlg showing the channels to delete
        /// if after the dlg closes we will throw an exception if still not compliant
        /// </summary>
        public void Enforce(ILicenseCapabilities capabilities, EnforcementContext context, IWin32Window owner)
        {
            if (Enforce(capabilities, context).Value == ComplianceLevel.NotCompliant)
            {
                IDialog channelLimitDlg = channelLimitDlgFactory.GetChannelLimitDlg(owner, EditionFeature, context);
                channelLimitDlg.ShowDialog();
            }

            EnumResult<ComplianceLevel> enforcementStatus = Enforce(capabilities, context);

            // After the dlg is closed if we are still not compliant throw a license exception
            if (enforcementStatus.Value == ComplianceLevel.NotCompliant)
            {
                throw new ShipWorksLicenseException(enforcementStatus.Message);
            }
        }

        /// <summary>
        /// Returns not compliant when the number of local channels exceeds the number allowed
        /// </summary>
        /// <returns></returns>
        public EnumResult<ComplianceLevel> Enforce(ILicenseCapabilities capabilities, EnforcementContext context)
        {
            if (context == EnforcementContext.Login)
            {
                ICustomerLicense license = licenseService.GetLicenses().FirstOrDefault() as ICustomerLicense;

                if (license != null)
                {
                    // Get a list of active stores in tango
                    IActiveStore[] tangoStores = license.GetActiveStores().ToArray();

                    // get a list of stores in the ShipWorks database
                    IEnumerable<StoreEntity> localStores = storeManager.GetAllStores().ToArray();

                    // loop through each store in ShipWorks and see if tango knows about it
                    foreach (
                        StoreEntity store in
                            localStores.Where(store => tangoStores.All(s => s.StoreLicenseKey != store.License)))
                    {
                        // Call activate on all of the stores that tango does not know about
                        EnumResult<LicenseActivationState> activationResult = license.Activate(store);

                        // If it failed then we return not compliant
                        if (activationResult.Value == LicenseActivationState.MaxChannelsExceeded)
                        {
                            return new EnumResult<ComplianceLevel>(ComplianceLevel.NotCompliant,
                                $"{activationResult.Message} Please delete {ChannelsToDelete(license, localStores)} channel(s).");
                        }

                        storeManager.SaveStore(store);
                    }
                }
            }
            // We got this far so we are compliant
            return new EnumResult<ComplianceLevel>(ComplianceLevel.Compliant, string.Empty);
        }

        /// <summary>
        /// Compares the number of channels from the active stores response to the local number
        /// </summary>
        private int ChannelsToDelete(ICustomerLicense customerLicense, IEnumerable<StoreEntity> localStores)
        {
            // Get an up to date list of stores in tango
            IEnumerable<IActiveStore> activeStores = customerLicense.GetActiveStores();

            // Find all of the channels in tango (not actual individual stores but channels e.g. Magento, Amazon, ChannelAdvisor)
            List<StoreTypeCode> allowedChannels = activeStores.Select(activeStore => activeStore.StoreType).Distinct().ToList();

            // Check to see if there are any local stores that do not belong to a channel that we are allowed to have
            return localStores.Count(store => allowedChannels.All(t => t != (StoreTypeCode) store.TypeCode));
        }

    }
}