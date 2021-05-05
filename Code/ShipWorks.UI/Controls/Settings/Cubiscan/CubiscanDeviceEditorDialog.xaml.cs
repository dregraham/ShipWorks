using System.Windows.Forms;

namespace ShipWorks.UI.Controls.Settings.Cubiscan
{
    public partial class CubiscanDeviceEditorDialog
    {
        public CubiscanDeviceEditorDialog(IWin32Window owner, ICubiscanDeviceEditorViewModel viewModel) : 
            base(owner, viewModel, false)
        {
            InitializeComponent();
        }
    }
}