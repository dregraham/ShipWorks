using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShipWorks.ApplicationCore.Licensing;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace ShipWorks.UI.Controls.UpgradePlan
{
    /// <summary>
    /// Interaction logic for UpgradePlanDlg.xaml
    /// </summary>
    public partial class UpgradePlanDlg : IDialog
    {
        public UpgradePlanDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UpgradePlanDlg(IWin32Window owner) : this()
        {
            new WindowInteropHelper(this)
            {
                Owner = owner.Handle
            };

        }

        private void OnClickCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
