using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Stamps
{
    /// <summary>
    /// Stamps globegistics consolidator
    /// </summary>
    public class StampsGlobegisticsConsolidatorRestriction : FeatureRestriction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StampsGlobegisticsConsolidatorRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
        }

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