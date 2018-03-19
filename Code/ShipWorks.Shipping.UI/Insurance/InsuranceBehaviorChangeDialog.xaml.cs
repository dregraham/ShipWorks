using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Insurance;
using System.Windows.Forms;

namespace ShipWorks.Shipping.UI.Insurance
{
    /// <summary>
    /// Interaction logic for InsuranceBehaviorChangeDialog.xaml
    /// </summary>
    [Component(Service = typeof(IInsuranceBehaviorChangeDialog), RegisterAs = RegistrationType.SpecificService)]
    public partial class InsuranceBehaviorChangeDialog : IInsuranceBehaviorChangeDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceBehaviorChangeDialog(IWin32Window owner, IInsuranceBehaviorChangeViewModel viewModel) : 
            base(owner, viewModel, false)
        {
            InitializeComponent();
        }
    }
}
