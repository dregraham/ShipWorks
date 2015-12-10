using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
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
        [SuppressMessage("SonarQube", "CS0067:The event is never used", Justification = "It is being used, but this message is still being shown.")]
        public virtual event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        private readonly PropertyChangedHandler handler;

        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonPackageAdapter"/> class.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public AmazonPackageAdapter(ShipmentEntity shipment)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged, () => PropertyChanging);
            this.shipment = shipment;
        }

        /// <summary>
        /// Gets or sets the index of this package adapter in a list of package adapters.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int Index { get; set; } = 1;

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double Length
        {
            get { return shipment.Amazon.DimsLength; }
            set
            {
                handler.Set(nameof(Length), v => shipment.Amazon.DimsLength = value, shipment.Amazon.DimsLength, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double Width
        {
            get { return shipment.Amazon.DimsWidth; }
            set
            {
                handler.Set(nameof(Width), v => shipment.Amazon.DimsWidth = value, shipment.Amazon.DimsWidth, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double Height
        {
            get { return shipment.Amazon.DimsHeight; }
            set
            {
                handler.Set(nameof(Height), v => shipment.Amazon.DimsHeight = value, shipment.Amazon.DimsHeight, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double Weight
        {
            get { return shipment.ContentWeight; }
            set
            {
                handler.Set(nameof(Weight), v => shipment.ContentWeight = value, shipment.ContentWeight, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double AdditionalWeight
        {
            get { return shipment.Amazon.DimsWeight; }
            set
            {
                handler.Set(nameof(AdditionalWeight), v => shipment.Amazon.DimsWeight = value, shipment.Amazon.DimsWeight, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ApplyAdditionalWeight
        {
            get { return shipment.Amazon.DimsAddWeight; }
            set
            {
                handler.Set(nameof(ApplyAdditionalWeight), v => shipment.Amazon.DimsAddWeight = value, shipment.Amazon.DimsAddWeight, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the packaging type.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public PackageTypeBinding PackagingType { get; set; } = null;

        /// <summary>
        /// Gets or sets the dims length.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double DimsLength
        {
            get { return shipment.Amazon.DimsLength; }
            set
            {
                handler.Set(nameof(DimsLength), v => shipment.Amazon.DimsLength = value, shipment.Amazon.DimsLength, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the dims width.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double DimsWidth
        {
            get { return shipment.Amazon.DimsWidth; }
            set
            {
                handler.Set(nameof(DimsWidth), v => shipment.Amazon.DimsWidth = value, shipment.Amazon.DimsWidth, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the dims height.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double DimsHeight
        {
            get { return shipment.Amazon.DimsHeight; }
            set
            {
                handler.Set(nameof(DimsHeight), v => shipment.Amazon.DimsHeight = value, shipment.Amazon.DimsHeight, value, false);
            }
        }

        /// <summary>
        /// Gets or sets the dimension profile id.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long DimsProfileID
        {
            get { return shipment.Amazon.DimsProfileID; }
            set
            {
                handler.Set(nameof(DimsProfileID), v => shipment.Amazon.DimsProfileID = value, shipment.Amazon.DimsProfileID, value, false);
            }
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
