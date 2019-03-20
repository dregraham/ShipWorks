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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls.Import
{
    /// <summary>
    /// Interaction logic for OdbcImportParameterizedQueryControl.xaml
    /// </summary>
    public partial class OdbcImportParameterizedQueryControl : UserControl
    {
        public OdbcImportParameterizedQueryControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Delegates scrolling of the datagrid to the scrollviewer
        /// </summary>
        private void MouseWheelScrolled(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - (float) e.Delta / 3);
        }
    }
}
