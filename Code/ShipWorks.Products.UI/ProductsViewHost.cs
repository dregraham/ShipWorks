using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Products.UI
{
    /// <summary>
    ///
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(IProductsViewHost))]
    public class ProductsViewHost : Control, IProductsViewHost
    {
        private readonly Func<IProductsMode, IProductsView> createProductsView;
        private readonly ElementHost host;
        private Action<Control> removeControl;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductsViewHost()
        {
            Dock = DockStyle.Fill;
            host = new ElementHost
            {
                Dock = DockStyle.Fill,
            };

            Controls.Add(host);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductsViewHost(Func<IProductsMode, IProductsView> createProductsView) : this()
        {
            this.createProductsView = createProductsView;
        }

        /// <summary>
        /// Initialize the control
        /// </summary>
        public void Initialize(IProductsMode productsMode, Action<Control> addControl, Action<Control> removeControl)
        {
            this.removeControl = removeControl;

            host.Child = createProductsView(productsMode).UIElement;

            addControl(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            removeControl?.Invoke(this);
        }
    }
}
