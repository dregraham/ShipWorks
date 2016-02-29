using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Endicia
{
    public class EndiciaScanBasedReturnsRestriction : FeatureRestriction
    {
        /// <summary>
        /// The edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.EndiciaScanBasedReturns;

        /// <summary>
        /// Checks the license capabilities to see if EndiciaScanBasedReturns is enabled
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.EndiciaScanBasedReturns ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}