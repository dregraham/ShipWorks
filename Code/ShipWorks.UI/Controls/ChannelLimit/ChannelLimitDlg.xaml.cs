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

namespace ShipWorks.UI.Controls.ChannelLimit
{
    /// <summary>
    /// Interaction logic for ChannelLimitDlg.xaml
    /// </summary>
    public partial class ChannelLimitDlg : IChannelLimitDlg
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelLimitDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelLimitDlg(IWin32Window owner) : this()
        {
            new WindowInteropHelper(this)
            {
                Owner = owner.Handle
            };
            
        }

        /// <summary>
        /// Updates the enabled state of the dlg based on the controls state
        /// </summary>
        private void OnEnabledChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            close.IsEnabled = (bool)dependencyPropertyChangedEventArgs.NewValue;
            Cursor = (bool)dependencyPropertyChangedEventArgs.NewValue ? Cursors.Arrow : Cursors.Wait;
        }
        
        /// <summary>
        /// Called when [click close].
        /// </summary>
        private void OnClickClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
