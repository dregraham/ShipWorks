using System.Windows;
using System.Windows.Controls;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Products.UI
{
    /// <summary>
    /// Main products mode view
    /// </summary>
    [Component]
    public partial class ProductsView : UserControl, IProductsView
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProductsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductsView(IProductsMode productsMode) : this()
        {
            DataContext = productsMode;
        }

        /// <summary>
        /// Get the UIElement representing the view
        /// </summary>
        public UIElement UIElement => this;
    }
}
