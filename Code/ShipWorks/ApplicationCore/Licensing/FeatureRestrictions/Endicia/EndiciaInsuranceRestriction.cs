using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Endicia
{
    /// <summary>
    /// Endicia insurance restriction
    /// </summary>
    public class EndiciaInsuranceRestriction : FeatureRestriction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaInsuranceRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
        }

        /// <summary>
        /// Gets the edition feature.
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.EndiciaInsurance;

        /// <summary>
        /// Checks the restriction for this feature
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.EndiciaInsurance ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}
