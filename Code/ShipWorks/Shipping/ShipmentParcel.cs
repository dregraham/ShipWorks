using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Represents a single parcel in a shipment.  This differs from "Packages" in that in ShipWorks, a package is a true entity for shipment providers
    /// that support the concept of multi-package shipments.  A parcel can be one of those too, but it also represents the singular package\parcel in the case
    /// of shipment providers that only have a shipment level.
    /// </summary>
    public class ShipmentParcel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentParcel(ShipmentEntity shipment, long? packageID, IInsuranceChoice insurance, DimensionsAdapter dimensions)
        {
            Shipment = shipment;
            PackageID = packageID;
            Insurance = insurance;
            Dimensions = dimensions;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentParcel(ShipmentEntity shipment, long? packageID, string trackingNumber, IInsuranceChoice insurance, DimensionsAdapter dimensions) :
            this(shipment, packageID, insurance, dimensions)
        {
            TrackingNumber = trackingNumber;
        }

        /// <summary>
        /// The shipment that holds the parcel
        /// </summary>
        public ShipmentEntity Shipment { get; }

        /// <summary>
        /// The PackageID for the package in the database, if the shipment type supports it
        /// </summary>
        public long? PackageID { get; }
        
        /// <summary>
        /// User-selected insurance option for the parcel
        /// </summary>
        public IInsuranceChoice Insurance { get; }

        /// <summary>
        /// Dimensions for the parcel
        /// </summary>
        public DimensionsAdapter Dimensions { get; }

        /// <summary>
        /// Gets or sets the weight (including add weight) of the package contents
        /// </summary>
        /// <value>
        /// The weight of the package contents
        /// </value>
        public double TotalWeight { get; set; }

        /// <summary>
        /// Tracking number for the package, if it exists
        /// </summary>
        public string TrackingNumber { get; private set; }
    }
}
