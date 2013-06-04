using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Provides an overview of a single ShipWorks shipment.  Provies away to describe common shipment properties in one data object.
    /// </summary>
    public class ShipmentCommonDetail
    {
        string originAccount;
        ShipmentTypeCode? originalShipmentType;

        int serviceType;

        int packagingType;
        double packageLength;
        double packageWidth;
        double packageHeight;

        /// <summary>
        ///  The account being used to send the shipment
        /// </summary>
        public string OriginAccount
        {
            get { return originAccount; }
            set { originAccount = value; }
        }

        /// <summary>
        /// The shipment type the user actually selected (like Endicia -> Express1)
        /// </summary>
        public ShipmentTypeCode? OriginalShipmentType
        {
            get { return originalShipmentType; }
            set { originalShipmentType = value; }
        }

        /// <summary>
        /// The service type used - relative to the shipment type
        /// </summary>
        public int ServiceType
        {
            get { return serviceType; }
            set { serviceType = value; }
        }

        /// <summary>
        /// The packaging type used - relative to the shipment type
        /// </summary>
        public int PackagingType
        {
            get { return packagingType; }
            set { packagingType = value; }
        }

        /// <summary>
        /// Package length of the 1st package
        /// </summary>
        public double PackageLength
        {
            get { return packageLength; }
            set { packageLength = value; }
        }

        /// <summary>
        /// Package width of the 1st package
        /// </summary>
        public double PackageWidth
        {
            get { return packageWidth; }
            set { packageWidth = value; }
        }

        /// <summary>
        /// Package height of the 1st package
        /// </summary>
        public double PackageHeight
        {
            get { return packageHeight; }
            set { packageHeight = value; }
        }
    }
}
