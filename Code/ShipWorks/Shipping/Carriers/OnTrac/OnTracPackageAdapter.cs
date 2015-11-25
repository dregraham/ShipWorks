using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipSense.Hashing;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    public class OnTracPackageAdapter : IPackageAdapter
    {
        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnTracPackageAdapter"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public OnTracPackageAdapter(ShipmentEntity shipment)
        {
            this.shipment = shipment;
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public double Length
        {
            get { return shipment.OnTrac.DimsLength; }
            set { shipment.OnTrac.DimsLength = value; }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double Width
        {
            get { return shipment.OnTrac.DimsWidth; }
            set { shipment.OnTrac.DimsWidth = value; }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public double Height
        {
            get { return shipment.OnTrac.DimsHeight; }
            set { shipment.OnTrac.DimsHeight = value; }
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
            get { return shipment.OnTrac.DimsWeight; }
            set { shipment.OnTrac.DimsWeight = value; }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        public bool ApplyAdditionalWeight
        {
            get { return shipment.OnTrac.DimsAddWeight; }
            set { shipment.OnTrac.DimsAddWeight = value; }
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
