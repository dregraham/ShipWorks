using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
        [SuppressMessage("SonarQube", "S2290:Field-like events should not be virtual", Justification = "Event is virtual to allow tests to fire it")]
        public virtual event PropertyChangedEventHandler PropertyChanged;
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
        /// Gets or sets the index of this package adapter in a list of package adapters.
        /// </summary>
        public int Index { get; set; } = 1;

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
        public PackageTypeBinding PackagingType { get; set; } = null;

        /// <summary>
        /// Gets or sets the dims length.
        /// </summary>
        public double DimsLength
        {
            get { return shipment.Amazon.DimsLength; }
            set { shipment.Amazon.DimsLength = value; }
        }

        /// <summary>
        /// Gets or sets the dims width.
        /// </summary>
        public double DimsWidth
        {
            get { return shipment.Amazon.DimsWidth; }
            set { shipment.Amazon.DimsWidth = value; }
        }

        /// <summary>
        /// Gets or sets the dims height.
        /// </summary>
        public double DimsHeight
        {
            get { return shipment.Amazon.DimsHeight; }
            set { shipment.Amazon.DimsHeight = value; }
        }

        /// <summary>
        /// Gets or sets the dimension profile id.
        /// </summary>
        public long DimsProfileID
        {
            get { return shipment.Amazon.DimsProfileID; }
            set { shipment.Amazon.DimsProfileID = value; }
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
