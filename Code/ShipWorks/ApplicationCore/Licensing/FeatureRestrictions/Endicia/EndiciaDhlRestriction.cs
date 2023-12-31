﻿using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.Endicia
{
    public class EndiciaDhlRestriction : FeatureRestriction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaDhlRestriction(IMessageHelper messageHelper) : base(messageHelper)
        {
        }

        /// <summary>
        /// The edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.EndiciaDhl;

        /// <summary>
        /// Checks the license capabilities to see if EndiciaDhl is enabled
        /// </summary>
        public override EditionRestrictionLevel Check(ILicenseCapabilities capabilities, object data)
        {
            return capabilities.EndiciaDhl ? EditionRestrictionLevel.None : EditionRestrictionLevel.Hidden;
        }
    }
}