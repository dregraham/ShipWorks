using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipSense.Hashing;
using ShipWorks.Shipping.ShipSense.Packaging;
using System.Diagnostics.CodeAnalysis;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Other
{
    /// <summary>
    /// The OtherShipmentType only supports the weight field of the package adapter.
    /// </summary>
    [SuppressMessage("SonarQube", "S3237:\"value\" parameters should be used", 
        Justification = "This package adapter does not set much data, so the value parameter is not needed")]
    public class OtherPackageAdapter : IPackageAdapter
    {
        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Initializes a new instance of the <see cref="OtherPackageAdapter"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public OtherPackageAdapter(ShipmentEntity shipment)
        {
            this.shipment = shipment;
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public double Length
        {
            get { return 0; }
            set { /* We don't care about this value */ }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double Width
        {
            get { return 0; }
            set { /* We don't care about this value */ }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public double Height
        {
            get { return 0; }
            set { /* We don't care about this value */ }
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        public double Weight
        {
            get { return shipment.ContentWeight; }
            set { shipment.ContentWeight = value; }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        public double AdditionalWeight
        {
            get { return 0; }
            set { /* We don't care about this value */ }
        }


        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        public bool ApplyAdditionalWeight
        {
            get { return true; }
            set { /* We don't care about this value */ }
        }

        /// <summary>
        /// Gets the hash code based on this package adapter's properties.
        /// </summary>
        public string HashCode()
        {
            StringHash stringHash = new StringHash();

            string rawValue = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", Length, Width, Height, Weight, AdditionalWeight, ApplyAdditionalWeight);

            return stringHash.Hash(rawValue, string.Empty);
        }
    }
}
