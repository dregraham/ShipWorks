using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Stamps
{
    /// <summary>
    /// Stamps ascendia consolidator restriction
    /// </summary>
    public class StampsAscendiaConsolidatorRestriction : FeatureRestriction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messageHelper"></param>
        public StampsAscendiaConsolidatorRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
        }

        /// <summary>
        /// The edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.StampsAscendiaConsolidator;

        /// <summary>
        /// Checks the license capabilities to see if StampsAscendiaConsolidator is enabled
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.StampsAscendiaConsolidator ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}