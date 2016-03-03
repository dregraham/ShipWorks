using ShipWorks.Editions;

namespace ShipWorks.ApplicationCore.Licensing.FeatureRestrictions.ShipmentTypeFunctionality
{
    /// An implementation of the IFeatureRestriction interface that checks whether processing 
    /// shipments is restricted based on an instance of ILicenseCapabilities.
    public class ProcessShipmentRestriction : ShipmentTypeFunctionalityRestriction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessShipmentRestriction"/> class.
        /// </summary>
        public ProcessShipmentRestriction()
            : base(ShipmentTypeRestrictionType.Processing)
        {
        }

        /// <summary>
        /// The edition feature
        /// </summary>
        public override EditionFeature EditionFeature => EditionFeature.ProcessShipment;
    }
}