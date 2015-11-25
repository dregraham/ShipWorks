using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipSense.Hashing;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Implementation of IPackageAdapter for Amazon and ShipSense.
    /// </summary>
    public class AmazonPackageAdapter : IPackageAdapter
    {
        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonPackageAdapter"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public AmazonPackageAdapter(ShipmentEntity shipment)
        {
            this.shipment = shipment;
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public double Length
        {
            get { return shipment.Amazon.DimsLength; }
            set { shipment.Amazon.DimsLength = value; }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double Width
        {
            get { return shipment.Amazon.DimsWidth; }
            set { shipment.Amazon.DimsWidth = value; }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public double Height
        {
            get { return shipment.Amazon.DimsHeight; }
            set { shipment.Amazon.DimsHeight = value; }
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
            get { return shipment.Amazon.DimsWeight; }
            set { shipment.Amazon.DimsWeight = value; }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        public bool ApplyAdditionalWeight
        {
            get { return shipment.Amazon.DimsAddWeight; }
            set { shipment.Amazon.DimsAddWeight = value; }
        }

        /// <summary>
        /// Gets or sets the packaging type.
        /// </summary>
        public int PackagingType
        {
            get { return 0; }
            set {  }
        }

        /// <summary>
        /// Gets the hash code based on this package adapter's properties.
        /// </summary>
        public string HashCode()
        {
            StringHash stringHash = new StringHash();

            string rawValue = $"{Length}-{Width}-{Height}-{Weight}-{AdditionalWeight}-{ApplyAdditionalWeight}";

            return stringHash.Hash(rawValue, string.Empty);
        }
    }
}
