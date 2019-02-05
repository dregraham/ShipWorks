using System.Diagnostics.CodeAnalysis;
using System.Windows.Interop;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Stores.Platforms.Shopify;

namespace ShipWorks.Stores.UI.Platforms.Shopify
{
    /// <summary>
    /// Interaction logic for ShopifyCreateTokenDialog.xaml
    /// </summary>
    [Component]
    public partial class ShopifyCreateTokenDialog : IShopifyCreateTokenDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyCreateTokenDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyCreateTokenDialog(IShopifyCreateTokenViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }

        /// <summary>
        /// Set the owner of this window
        /// </summary>
        [SuppressMessage("SonarQube", "S1848:Objects should not be created to be dropped immediately without being used",
            Justification = "The interop helper is only used temporarily to set this window's owner")]
        [SuppressMessage("Recommendations", "RECS0026:Objects should not be created to be dropped immediately without being used",
            Justification = "The interop helper is only used temporarily to set this window's owner")]
        public void LoadOwner(System.Windows.Forms.IWin32Window owner)
        {
            new WindowInteropHelper(this)
            {
                Owner = owner.Handle
            };
        }
    }
}
