using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.UI.Dialogs.DefaultPrinters
{
    /// <summary>
    /// Interaction logic for DefaultPrinters.xaml
    /// </summary>
    public partial class DefaultPrinters : IDefaultPrinters
    {
        public DefaultPrinters(IWin32Window owner, DefaultPrintersViewModel viewModel) : base(owner, viewModel, false)
        {
            InitializeComponent();
        }
    }
}
