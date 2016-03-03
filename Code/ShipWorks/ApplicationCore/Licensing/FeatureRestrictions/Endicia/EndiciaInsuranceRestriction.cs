using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Endicia
{
    public class EndiciaInsuranceRestriction : FeatureRestriction
    {
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
