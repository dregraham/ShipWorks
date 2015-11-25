
namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// An interface intended to be used for shuffling package data between classes.
    /// </summary>
    public interface IPackageAdapter
    {
        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        double Length { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        double Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        double Height { get; set; }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        double Weight { get; set; }
        
        /// <summary>
        /// Gets or sets the additional weight.
        /// </summary>
        double AdditionalWeight { get; set; }

        /// <summary>
        /// Gets or sets the whether the additional weight should be applied.
        /// </summary>
        bool ApplyAdditionalWeight { get; set; }

        /// <summary>
        /// Gets or sets the packaging type.
        /// </summary>
        int PackagingType { get; set; }

        /// <summary>
        /// Gets the hash code based on this package adapter's properties.
        /// </summary>
        string HashCode();
    }
}
