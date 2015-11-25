using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// A package adapter that amounts to an implementation of the null object
    /// pattern. 
    /// </summary>
    public class NullPackageAdapter : IPackageAdapter
    {
        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public double Length
        {
            get { return 0; }
            set{}
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public double Width
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public double Height
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        public double Weight
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        public double AdditionalWeight
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        public bool ApplyAdditionalWeight
        {
            get { return true; }
            set { }
        }

        /// <summary>
        /// Gets or sets the packaging type.
        /// </summary>
        public int PackagingType
        {
            get { return 0; }
            set { }
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
