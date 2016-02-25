﻿using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions
{
    public class StampsAscendiaConsolidatorRestriction : FeatureRestriction, IFeatureRestriction
    {
        /// <summary>
        /// The edition feature
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.StampsAscendiaConsolidator;

        /// <summary>
        /// Checks the license capabilities to see if StampsAscendiaConsolidator is enabled
        /// </summary>
        public EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.StampsAscendiaConsolidator ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}