
namespace ShipWorks.Shipping.ShipSense.Packaging
{
    /// <summary>
    /// A package adapter that amounts to an implementation of the null object
    /// pattern. This is intended for shipment types where ShipSense is not
    /// applicable.
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
    }
}
