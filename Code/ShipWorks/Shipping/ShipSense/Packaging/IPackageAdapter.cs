
namespace ShipWorks.Shipping.ShipSense.Packaging
{
    /// <summary>
    /// An interface intended to be used for shuffling package data between 
    /// a ShipSense knowledge base entry and carriers.
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
    }
}
