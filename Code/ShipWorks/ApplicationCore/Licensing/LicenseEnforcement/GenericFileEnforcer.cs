using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{
    /// <summary>
    ///
    /// </summary>
    public class GenericFileEnforcer : ILicenseEnforcer
    {
        private readonly IChannelLimitDlgFactory channelLimitDlgFactory;
        private readonly IStoreManager storeManager;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileEnforcer(IChannelLimitDlgFactory channelLimitDlgFactory, Func<Type, ILog> logFactory, IStoreManager storeManager)
        {
            this.channelLimitDlgFactory = channelLimitDlgFactory;
            this.storeManager = storeManager;
            log = logFactory(typeof(ChannelCountEnforcer));
        }

        /// <summary>
        /// The priority for this enforcer
        /// </summary>
        public int Priortity { get; }

        /// <summary>
        /// The edition feature being enforced
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.GenericFile;

        /// <summary>
        /// If the user has generic file stores when they shouldn't, display a dialog
        /// prompting them to delete them.
        /// </summary>
        /// <param name="capabilities">The users license capabilities.</param>
        /// <param name="context">The enforcement context.</param>
        /// <param name="owner">The window to own the created dialog.</param>
        public void Enforce(ILicenseCapabilities capabilities, EnforcementContext context, IWin32Window owner)
        {
            if (Enforce(capabilities, context).Value == ComplianceLevel.NotCompliant)
            {
                try
                {
                    IChannelLimitDlg channelLimitDlg = channelLimitDlgFactory.GetChannelLimitDlg(owner);
                    channelLimitDlg.ShowDialog();
                }
                catch (ShipWorksLicenseException ex)
                {
                    log.Error("Error thrown when displaying channel limit dialog", ex);
                }
            }
        }

        /// <summary>
        /// Ensures that the user does not have any generic file stores, if they are not allowed.
        /// </summary>
        /// <param name="capabilities">The users license capabilities.</param>
        /// <param name="context">The enforcement context.</param>
        public EnumResult<ComplianceLevel> Enforce(ILicenseCapabilities capabilities, EnforcementContext context)
        {
            IEnumerable<StoreEntity> stores = storeManager.GetAllStores();

            if (stores.Any(store => store.TypeCode == (int)StoreTypeCode.GenericFile))
            {
                string error =
                    "Your plan does not support the generic file channel. Please upgrade your plan or delete your generic file channel to continue downloading orders and creating shipment labels.";

                return new EnumResult<ComplianceLevel>(ComplianceLevel.NotCompliant, error);
            }

            return new EnumResult<ComplianceLevel>(ComplianceLevel.Compliant, string.Empty);
        }
    }
}