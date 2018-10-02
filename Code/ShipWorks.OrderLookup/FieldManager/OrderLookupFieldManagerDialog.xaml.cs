using System.Diagnostics.CodeAnalysis;
using System.Windows.Interop;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;

namespace ShipWorks.OrderLookup.FieldManager
{
    /// <summary>
    /// Interaction logic for FieldManagerDialog.xaml
    /// </summary>
    [Component]
    public partial class OrderLookupFieldManagerDialog : IOrderLookupFieldManagerDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupFieldManagerDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupFieldManagerDialog(IOrderLookupFieldManager viewModel) : this()
        {
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

        /// <summary>
        /// Handle when the title border is clicked so that we can allow the window to be dragged
        /// </summary>
        private void TitleBorderMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) =>
            DragMove();
    }
}
