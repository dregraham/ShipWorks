using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipSense.Hashing;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Shipping.Carriers.Postal
{
    public class PostalPackageAdapter : IPackageAdapter
    {
        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostalPackageAdapter"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public PostalPackageAdapter(ShipmentEntity shipment)
        {
            this.shipment = shipment;
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public double Length
        {
            get { return shipment.Postal.DimsLength; }
            set { shipment.Postal.DimsLength = value; }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double Width
        {
            get { return shipment.Postal.DimsWidth; }
            set { shipment.Postal.DimsWidth = value; }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public double Height
        {
            get { return shipment.Postal.DimsHeight; }
            set { shipment.Postal.DimsHeight = value; }
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
            get { return shipment.Postal.DimsWeight; }
            set { shipment.Postal.DimsWeight = value; }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        public bool ApplyAdditionalWeight
        {
            get { return shipment.Postal.DimsAddWeight; }
            set { shipment.Postal.DimsAddWeight = value; }
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
