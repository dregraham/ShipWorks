using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public class WarehouseRestriction : FeatureRestriction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
        }

        /// <summary>
        /// The edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.Warehouse;

        /// <summary>
        /// Checks the license capabilities to see if Warehouse is enabled
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.Warehouse ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}