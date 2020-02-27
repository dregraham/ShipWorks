using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// Interaction logic for OneBalanceErrorDialog.xaml
    /// </summary>
    public partial class OneBalanceErrorDialog : Window
    {
        public OneBalanceErrorDialog(string message)
        {
            var viewModel = new OneBalanceErrorViewModel()
            {
                Message = message
            };
            this.DataContext = viewModel;
            InitializeComponent();
        }
    }
}
