using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Interaction logic for OneBalanceCreateStampsAccountDialog.xaml
    /// </summary>
    public partial class OneBalanceCreateStampsAccountDialog : InteropWindow
    {
        public OneBalanceCreateStampsAccountDialog(IWin32Window owner) : base(owner, false)
        {
            InitializeComponent();
        }
    }
}
