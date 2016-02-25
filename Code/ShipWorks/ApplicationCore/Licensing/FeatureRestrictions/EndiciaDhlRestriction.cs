using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public class EndiciaDhlRestriction : FeatureRestriction, IFeatureRestriction
    {
        /// <summary>
        /// The edition feature
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.EndiciaDhl;

        /// <summary>
        /// Checks the license capabilities to see if EndiciaDhl is enabled
        /// </summary>
        public EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.EndiciaDhl ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}