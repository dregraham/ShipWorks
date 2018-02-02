using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Insurance;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Interop;

namespace ShipWorks.Shipping.UI.Insurance
{
    /// <summary>
    /// Interaction logic for InsuranceBehaviorChangeDialog.xaml
    /// </summary>
    [Component]
    public partial class InsuranceBehaviorChangeDialog : Window, IInsuranceBehaviorChangeDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceBehaviorChangeDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceBehaviorChangeDialog(IInsuranceBehaviorChangeViewModel viewModel) : this()
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
