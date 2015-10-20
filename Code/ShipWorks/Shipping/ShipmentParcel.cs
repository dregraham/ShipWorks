using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Represents a single parcel in a shipment.  This differs from "Packages" in that in ShipWorks, a package is a true entity for shipment providers
    /// that support the concept of multi-package shipments.  A parcel can be one of those too, but it also represents the singular package\parcel in the case
    /// of shipment providers that only have a shipment level.
    /// </summary>
    public class ShipmentParcel
    {
        // The shipment the parcel represents
        ShipmentEntity shipment;

        // Package ID in the database, if the shipment type supports it
        long? packageID;

        // What the user has opted into for insurance for this parcel
        InsuranceChoice insurance;

        // Dimensions for the parcel
        DimensionsAdapter dimensions;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentParcel(ShipmentEntity shipment, long? packageID, InsuranceChoice insurance, DimensionsAdapter dimensions)
        {
            this.shipment = shipment;
            this.packageID = packageID;
            this.insurance = insurance;
            this.dimensions = dimensions;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentParcel(ShipmentEntity shipment, long? packageID, string trackingNumber, InsuranceChoice insurance, DimensionsAdapter dimensions) :
            this(shipment, packageID, insurance, dimensions)
        {
            TrackingNumber = trackingNumber;
        }

        /// <summary>
        /// The shipment that holds the parcel
        /// </summary>
        public ShipmentEntity Shipment
        {
            get { return shipment; }
        }

        /// <summary>
        /// The PackageID for the package in the database, if the shipment type supports it
        /// </summary>
        public long? PackageID
        {
            get { return packageID; }
        }
        
        /// <summary>
        /// User-selected insurance option for the parcel
        /// </summary>
        public InsuranceChoice Insurance
        {
            get { return insurance; }
        }

        /// <summary>
        /// Dimensions for the parcel
        /// </summary>
        public DimensionsAdapter Dimensions
        {
            get { return dimensions; }
        }

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
