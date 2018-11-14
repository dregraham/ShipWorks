using System;
using System.Windows.Forms;

namespace ShipWorks.Products
{
    /// <summary>
    /// Main products view model
    /// </summary>
    public interface IProductsMode : IDisposable
    {
        /// <summary>
        /// Initialize the mode
        /// </summary>
        void Initialize(Action<Control> addControl, Action<Control> removeControl);
    }
}
