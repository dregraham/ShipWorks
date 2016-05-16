using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Licensing.LicenseEnforcement
{
    /// <summary>
    /// Abstract class for a ChannelTypeEnforcer
    /// </summary>
    public abstract class ChannelTypeEnforcer : ILicenseEnforcer
    {
        protected readonly IChannelLimitDlgFactory channelLimitDlgFactory;
        protected readonly IStoreManager storeManager;
        protected readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ChannelTypeEnforcer(IChannelLimitDlgFactory channelLimitDlgFactory,
            ILog log,
            IStoreManager storeManager)
        {
            this.channelLimitDlgFactory = channelLimitDlgFactory;
            this.storeManager = storeManager;
            this.log = log;
        }

        /// <summary>
        /// The priority for this enforcer
        /// </summary>
        public virtual EnforcementPriority Priority => EnforcementPriority.Medium;

        /// <summary>
        /// The edition feature being enforced
        /// </summary>
        public abstract EditionFeature EditionFeature { get; }

        /// <summary>
        /// All Channel Types are allowed for trials.
        /// </summary>
        public bool AppliesTo(ILicenseCapabilities capabilities) => !capabilities.IsInTrial;

        /// <summary>
        /// Gets the store type code.
        /// </summary>
        protected abstract StoreTypeCode StoreTypeCode { get; }

        /// <summary>
        /// If the user has generic file stores when they shouldn't, display a dialog
        /// prompting them to delete them.
        /// </summary>
        public void Enforce(ILicenseCapabilities capabilities, EnforcementContext context, IWin32Window owner)
        {
            if (Enforce(capabilities, context).Value == ComplianceLevel.NotCompliant)
            {
                IDialog channelLimitDlg = channelLimitDlgFactory.GetChannelLimitDlg(owner, EditionFeature, context);
                channelLimitDlg.ShowDialog();
            }
        }

        /// <summary>
        /// Ensures that the user does not have any channels of StoreTypeCode, if they are not allowed.
        /// </summary>
        public EnumResult<ComplianceLevel> Enforce(ILicenseCapabilities capabilities, EnforcementContext context)
        {
            IEnumerable<StoreEntity> stores = storeManager.GetAllStores();

            if (!capabilities.IsChannelAllowed(StoreTypeCode) &&
                stores.Any(store => store.TypeCode == (int) StoreTypeCode))
            {
                string channelName = EnumHelper.GetDescription(StoreTypeCode);
                string error =
                    $"Your plan does not support the {channelName} channel. " +
                    $"Please upgrade your plan or delete your {channelName} channel " +
                    "to continue downloading orders and creating shipment labels.";

                return new EnumResult<ComplianceLevel>(ComplianceLevel.NotCompliant, error);
            }

            return new EnumResult<ComplianceLevel>(ComplianceLevel.Compliant, string.Empty);
        }
    }
}
