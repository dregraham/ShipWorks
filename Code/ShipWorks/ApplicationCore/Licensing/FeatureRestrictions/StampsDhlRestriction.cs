using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public class StampsDhlRestriction : FeatureRestriction, IFeatureRestriction
    {
        /// <summary>
        /// The edition feature
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.StampsDhl;

        /// <summary>
        /// Checks the license capabilities to see if StampsRrDonnelleyConsolidator is enabled
        /// </summary>
        public EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.StampsDhl ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}