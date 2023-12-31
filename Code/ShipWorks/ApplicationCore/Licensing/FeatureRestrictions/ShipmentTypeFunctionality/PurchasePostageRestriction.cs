﻿using Interapptive.Shared.UI;
using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.ShipmentTypeFunctionality
{
    /// An implementation of the IFeatureRestriction interface that checks whether
    /// purchasing postage is restricted based on an instance of ILicenseCapabilities.
    public class PurchasePostageRestriction : ShipmentTypeFunctionalityRestriction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PurchasePostageRestriction"/> class.
        /// </summary>
        public PurchasePostageRestriction(IMessageHelper messageHelper)
            : base(ShipmentTypeRestrictionType.Purchasing, messageHelper)
        {
        }

        /// <summary>
        /// The edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.PurchasePostage;
    }
}