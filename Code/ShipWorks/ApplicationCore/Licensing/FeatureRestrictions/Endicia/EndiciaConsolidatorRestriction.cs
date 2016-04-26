using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Endicia
{
    public class EndiciaConsolidatorRestriction : FeatureRestriction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaConsolidatorRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
        }

        /// <summary>
        /// The edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.EndiciaConsolidator;

        /// <summary>
        /// Checks the license capabilities to see if Endicia consolidators are enabled
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.EndiciaConsolidator ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}