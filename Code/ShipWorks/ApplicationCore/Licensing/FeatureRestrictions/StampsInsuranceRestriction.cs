using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public class StampsInsuranceRestriction : FeatureRestriction, IFeatureRestriction
    {
        /// <summary>
        /// The edition feature
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.StampsInsurance;

        /// <summary>
        /// Checks the license capabilities to see if StampsInsurance is enabled
        /// </summary>
        public EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.StampsInsurance ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}