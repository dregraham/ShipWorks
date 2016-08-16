using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Stamps
{
    /// <summary>
    /// Stamps Dhl Restriction
    /// </summary>
    public class StampsDhlRestriction : FeatureRestriction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StampsDhlRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
        }

        /// <summary>
        /// The edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.StampsDhl;

        /// <summary>
        /// Checks the license capabilities to see if StampsRrDonnelleyConsolidator is enabled
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.StampsDhl ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}