using System.Windows.Forms;
using ShipWorks.Data.Administration;

namespace ShipWorks.UI.Dialogs.QuickStart
{
    public partial class QuickStart : IQuickStart
    {
        public QuickStart(IWin32Window owner, IQuickStartViewModel viewModel) : base(owner, viewModel, false)
        {
            InitializeComponent();
        }
    }
}
