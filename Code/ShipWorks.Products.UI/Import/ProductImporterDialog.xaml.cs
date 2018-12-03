using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Interop;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Products.Import;

namespace ShipWorks.Products.UI.Import
{
    /// <summary>
    /// Interaction logic for ProductImporterDialog.xaml
    /// </summary>
    [Component]
    public partial class ProductImporterDialog : Window, IProductImporterDialog
    {
        public ProductImporterDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductImporterDialog(IProductImporterViewModel viewModel)
        {
            InitializeComponent();

            WindowStateSaver.Manage(this);

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
