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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.UI.Profiles
{
    /// <summary>
    /// Interaction logic for ShippingProfileManagerDialog.xaml
    /// </summary>
    public partial class ShippingProfileManagerDialog : InteropWindow
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileManagerDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Automatically resizes the grid columns to get rid of the extra space after the last column header
        /// </summary>
        private void ProfileViewTargetUpdated(object sender, DataTransferEventArgs e)
        {
            GridView view = profileView.View as GridView;

            if (view != null && view.Columns.Count > 0)
            {
                foreach (var column in view.Columns)
                {
                    // Forcing change
                    if (double.IsNaN(column.Width))
                    {
                        column.Width = 1;
                    }
                    column.Width = double.NaN;
                }
            }
        }
    }
}
