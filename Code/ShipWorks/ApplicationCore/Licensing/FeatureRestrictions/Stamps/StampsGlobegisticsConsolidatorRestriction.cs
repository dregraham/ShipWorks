using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public class StampsGlobegisticsConsolidatorRestriction : FeatureRestriction, IFeatureRestriction
    {
        /// <summary>
        /// The edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.StampsGlobegisticsConsolidator;

        /// <summary>
        /// Checks the license capabilities to see if StampsGlobegisticsConsolidator is enabled
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.StampsGlobegisticsConsolidator ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}