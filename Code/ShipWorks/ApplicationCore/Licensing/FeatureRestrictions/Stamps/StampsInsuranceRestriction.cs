using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Stamps
{
    public class StampsInsuranceRestriction : FeatureRestriction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StampsInsuranceRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
        }

        /// <summary>
        /// The edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.StampsInsurance;

        /// <summary>
        /// Checks the license capabilities to see if StampsInsurance is enabled
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.StampsInsurance ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}