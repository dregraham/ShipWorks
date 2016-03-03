using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.ShipmentTypeFunctionality
{
    /// <summary>
    /// An implementation of the IFeatureRestriction interface that checks whether shipping account conversion
    /// is restricted based on an instance of ILicenseCapabilities.
    /// </summary>
    /// <seealso cref="ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.FeatureRestriction" />
    public class ShippingAccountConversionRestriction : ShipmentTypeFunctionalityRestriction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingAccountConversionRestriction"/> class.
        /// </summary>
        public ShippingAccountConversionRestriction()
            : base(ShipmentTypeRestrictionType.ShippingAccountConversion)
        {
        }

        /// <summary>
        /// Gets the edition feature.
        /// </summary>
        public override EditionFeature EditionFeature
        {
            get { return EditionFeature.ShippingAccountConversion; }
        }
    }
}
