using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public class StampsDhlConsolidatorRestriction : FeatureRestriction, IFeatureRestriction
    {
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