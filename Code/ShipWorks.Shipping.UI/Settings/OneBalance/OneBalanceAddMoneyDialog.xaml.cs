using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// Interaction logic for OneBalanceAddMoneyDialog.xaml
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(IOneBalanceAddMoneyDialog))]
    public partial class OneBalanceAddMoneyDialog : InteropWindow, IOneBalanceAddMoneyDialog
    {
        public OneBalanceAddMoneyDialog(IWin32Window owner, IPostageWebClient webClient, IMessageHelper messageHelper) : base(owner, false)
        {
            this.DataContext = new OneBalanceAddMoneyViewModel(webClient, this, messageHelper);
            InitializeComponent();
        }
    }
}
