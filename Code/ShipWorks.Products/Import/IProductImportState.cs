using System.ComponentModel;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Represents a state of the product import process
    /// </summary>
    public interface IProductImportState
    {
        /// <summary>
        /// Should products be reloaded after the dialog closes
        /// </summary>
        bool ShouldReloadProducts { get; }

        /// <summary>
        /// Is a close requested
        /// </summary>
        void CloseRequested(CancelEventArgs e);
    }
}