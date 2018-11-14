using System;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Products
{
    /// <summary>
    /// Main products mode view model
    /// </summary>
    [Component]
    public class ProductsMode : IProductsMode
    {
        private readonly IProductsViewHost view;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductsMode(IProductsViewHost view)
        {
            this.view = view;
        }

        /// <summary>
        /// Initialize the mode
        /// </summary>
        public void Initialize(Action<Control> addControl, Action<Control> removeControl)
        {
            view.Initialize(this, addControl, removeControl);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // Do nothing for now
        }
    }
}
