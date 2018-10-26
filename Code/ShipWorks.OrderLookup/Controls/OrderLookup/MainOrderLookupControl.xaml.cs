using System.Windows.Controls;
using GongSolutions.Wpf.DragDrop; 

namespace ShipWorks.OrderLookup.Controls.OrderLookup
{
    /// <summary>
    /// Interaction logic for MainOrderLookupControl.xaml
    /// </summary>
    public partial class MainOrderLookupControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MainOrderLookupControl()
        {
            InitializeComponent();

            // the only reason for this to be in here is to force the gong dll to be
            // included in the project.
            System.Windows.DataFormat _ = DragDrop.DataFormat;
        } 
    }
}
