using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
