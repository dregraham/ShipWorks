using System;
using System.Windows;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Interaction logic for OneBalanceCreateStampsAccountDialog.xaml
    /// </summary>
    public partial class OneBalanceCreateStampsAccountDialog : InteropWindow
    {
        public OneBalanceCreateStampsAccountDialog()
        {
            InitializeComponent();
        }


        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.OpenUrl("https://registration.stamps.com/registration/#!&p=profile", this);
        }
    }
}
