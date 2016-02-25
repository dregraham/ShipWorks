using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public class EndiciaScanBasedReturnsRestriction : FeatureRestriction, IFeatureRestriction
    {
        /// <summary>
        /// The edition feature
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.EndiciaScanBasedReturns;

        /// <summary>
        /// Checks the license capabilities to see if EndiciaScanBasedReturns is enabled
        /// </summary>
        public EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.EndiciaScanBasedReturns ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}