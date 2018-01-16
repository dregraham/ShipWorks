using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.UI.Controls
{
    /// <summary>
    /// Control with default message at the end of the AddStoreWizard
    /// </summary>
    [Component]
    public partial class StoreWizardFinishPageControl : UserControl, IStoreWizardFinishPageControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StoreWizardFinishPageControl()
        {
            InitializeComponent();
        }
    }
}
