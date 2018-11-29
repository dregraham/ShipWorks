using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        /// <summary>
        /// Override the SelectAll command binding to stop customers from potentially loading all products into memory
        /// </summary>
        private void OnSelectAllExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
