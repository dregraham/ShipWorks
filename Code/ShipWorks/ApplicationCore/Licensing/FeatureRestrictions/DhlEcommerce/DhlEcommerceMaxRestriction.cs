using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.DhlEcommerce
{
    /// <summary>
    /// DHL eCommerce Max restriction
    /// </summary>
    public class DhlEcommerceMaxRestriction : FeatureRestriction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceMaxRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
        }

        /// <summary>
        /// The edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.DhlEcommerceMax;

        /// <summary>
        /// Checks the license capabilities to see if DHL eCommerce Max is enabled
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.DhlEcommerceMax ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}