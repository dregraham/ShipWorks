using System;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Ups
{
    /// <summary>
    /// Limits # of UPS accounts
    /// </summary>
    public class UpsAccountLimitRestriction : FeatureRestriction
    {
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messageHelper"></param>
        public UpsAccountLimitRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Gets the edition feature.
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.UpsAccountLimit;

        /// <summary>
        /// Checks the restriction for this feature
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            int numberOfInstalledAccounts = (int) data;
            switch (capabilities.UpsStatus)
            {
                // For Discount Tier1 and Subsidized, the limit in capabilities is set properly
                case UpsStatus.Discount:
                case UpsStatus.Subsidized:
                case UpsStatus.Tier1:
                    return numberOfInstalledAccounts > capabilities.UpsAccountLimit ?
                        EditionRestrictionLevel.Forbidden :
                        EditionRestrictionLevel.None;

                // For Tier2, Tier3 and None there is no account limit
                case UpsStatus.None:
                case UpsStatus.Tier2:
                case UpsStatus.Tier3:
                    return EditionRestrictionLevel.None;

                default:
                    throw new ArgumentOutOfRangeException($"Unknown capability {capabilities.UpsStatus}");
            }
        }

        /// <summary>
        /// If restriction level is not none, throw.
        /// </summary>
        /// <exception cref="UpsException"></exception>
        public override bool Handle(IWin32Window owner, ILicenseCapabilities capabilities, object data)
        {
            EditionRestrictionLevel restriction = Check(capabilities, data);
            if (restriction != EditionRestrictionLevel.None)
            {
                messageHelper.ShowError(EnumHelper.GetDescription(EditionFeature));
                return false;
            }

            return true;
        }
    }
}
