using System.Windows.Controls;
using GongSolutions.Wpf.DragDrop; 

namespace ShipWorks.OrderLookup.Controls.OrderLookup
{
    /// <summary>
    /// Interaction logic for OrderLookupControl.xaml
    /// </summary>
    public partial class OrderLookupControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupControl()
        {
            InitializeComponent();

            // the only reason for this to be in here is to force the gong dll to be
            // included in the project.
            System.Windows.DataFormat _ = DragDrop.DataFormat;
        } 
    }
}
