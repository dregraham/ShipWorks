using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Stamps
{
    /// <summary>
    /// Stamps Dhl Consolidator restriction
    /// </summary>
    public class StampsDhlConsolidatorRestriction : FeatureRestriction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StampsDhlConsolidatorRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
        }

        /// <summary>
        /// The edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.StampsDhlConsolidator;

        /// <summary>
        /// Checks the license capabilities to see if StampsDhlConsolidator is enabled
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.StampsDhlConsolidator ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}