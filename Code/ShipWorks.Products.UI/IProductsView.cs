using System.Windows;

namespace ShipWorks.Products.UI
{
    /// <summary>
    /// Main products mode view
    /// </summary>
    public interface IProductsView
    {
        /// <summary>
        /// Get the UIElement representing the view
        /// </summary>
        UIElement UIElement { get; }
    }
}