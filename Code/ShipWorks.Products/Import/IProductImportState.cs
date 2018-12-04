using System.ComponentModel;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Represents a state of the product import process
    /// </summary>
    public interface IProductImportState
    {
        void CloseRequested(CancelEventArgs e);
    }
}