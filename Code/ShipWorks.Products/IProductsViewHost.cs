using System;
using System.Windows.Forms;

namespace ShipWorks.Products
{
    /// <summary>
    /// Main UI host control for the products view
    /// </summary>
    public interface IProductsViewHost
    {
        /// <summary>
        /// Initialize
        /// </summary>
        void Initialize(IProductsMode productsMode, Action<Control> addControl, Action<Control> removeControl);
    }
}