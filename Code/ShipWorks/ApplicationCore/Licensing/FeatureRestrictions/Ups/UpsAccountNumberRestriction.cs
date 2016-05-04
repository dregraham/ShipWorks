using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Ups
{
    /// <summary>
    /// Restricts ShipWorks to a specific set of Ups Account Numbers
    /// </summary>
    public class UpsAccountNumberRestriction : FeatureRestriction
    {
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messageHelper"></param>
        public UpsAccountNumberRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Works on the UpsAccountNumbers EditionFeature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.UpsAccountNumbers;

        /// <summary>
        /// Checks to see if the given account is allowed by the given ILicenseCapabilities
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            string account = data as string;

            // Check to see that the account is not blank or null
            // the capabilities has a collection of UpsAccountNumbers
            // the given account is not in the capabilities UpsAccountNumbers collection
            if (!string.IsNullOrWhiteSpace(account) && 
                capabilities.UpsAccountNumbers.Any() && 
                !capabilities.UpsAccountNumbers.Contains(account.ToLower()))
            {
                return EditionRestrictionLevel.Forbidden;
            }

            return EditionRestrictionLevel.None;
        }

        /// <summary>
        /// Nothing to handle, return false
        /// </summary>
        public override bool Handle(IWin32Window owner, ILicenseCapabilities capabilities, object data)
        {
            EditionRestrictionLevel level = Check(capabilities, data);
            
            if (level != EditionRestrictionLevel.None)
            {
                string account = data as string;
                messageHelper.ShowError(owner, $"You must contact Interapptive to enable use of UPS account '{account}'.");
            }

            return level == EditionRestrictionLevel.None;
        }
    }
}