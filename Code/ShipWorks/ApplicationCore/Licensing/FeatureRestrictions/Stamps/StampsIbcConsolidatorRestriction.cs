﻿using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Stamps
{
    public class StampsIbcConsolidatorRestriction : FeatureRestriction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StampsIbcConsolidatorRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
        }

        /// <summary>
        /// The edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.StampsIbcConsolidator;

        /// <summary>
        /// Checks the license capabilities to see if StampsIbcConsolidator is enabled
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.StampsIbcConsolidator ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}